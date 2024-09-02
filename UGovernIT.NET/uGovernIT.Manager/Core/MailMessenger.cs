using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public interface IMailMessenger : IManagerBase<Email>
    {
        string SendMail(string mailTo, string subject, string CC, string bodyText, bool isHTMLBody);
        string SendMail(string mailTo, string subject, string CC, string bodyText, bool isHTMLBody, string[] attachments, bool sendOnBackground = true, bool UseLoggedInUserAccountToSendMail = false, string saveToTicketId = null);

        long CreateOutgoingMailInstance(string emailStatus, string moduleId, string ticketID, string to, string cc, string subject, string body,ref DataRow dataRow);
    }

    public class MailMessenger : ManagerBase<Email>, IMailMessenger
    {
        private ConfigurationVariableManager _configurationVariableHelper;
        private UserProfile _userProfile;
        private ApplicationContext _context;
        private ITicketManager ticketManager;
        private ModuleViewManager viewManager;
        public long ticketEmailID { get; set; }

        public MailMessenger(ApplicationContext context) : base(context)
        {
            _configurationVariableHelper = new ConfigurationVariableManager(this.dbContext);
            store = new MailMessengerStore(this.dbContext);

            _userProfile = context.CurrentUser;
            _context = context;
            ticketManager= new TicketManager(context);
        }

        public string SendMail(string mailTo, string subject, string CC, string bodyText, Boolean isHTMLBody)
        {
            // Check if email notifications are enabled
            bool sendEmail = _configurationVariableHelper.GetValueAsBool(ConfigConstants.SendEmail);
            if (!sendEmail)
                return string.Empty;

            string[] attachments = { };
            return SendMailInternal(mailTo, subject, CC, bodyText, isHTMLBody, attachments, false);
        }

        public void SendMail(string mailTo, string subject, string CC, string bodyText, Boolean isHTMLBody, bool sendOnBackground = true)
        {
            string[] attachments = { };
            SendMail(mailTo, subject, CC, bodyText, isHTMLBody, attachments, sendOnBackground, false);
        }

        public string SendMail(string mailTo, string subject, string CC, string bodyText, Boolean isHTMLBody, string[] attachments, bool sendOnBackground = true, bool UseLoggedInUserAccountToSendMail = false,string saveToTicketId = null)
        {
            // Check if email notifications are enabled
            bool sendEmail = _configurationVariableHelper.GetValueAsBool(ConfigConstants.SendEmail);
            if (!sendEmail)
                return string.Empty;

            // Send email on background thread if running currently on foreground thread AND caller wants us to
            if (sendOnBackground)
            {
                SendMailOnBackgroundThread(mailTo, subject, CC, bodyText, isHTMLBody, attachments, UseLoggedInUserAccountToSendMail,saveToTicketId);
                return string.Empty;
            }
            else
            {
                return SendMailInternal(mailTo, subject, CC, bodyText, isHTMLBody, attachments, UseLoggedInUserAccountToSendMail,saveToTicketId);
            }
        }

        // Send email out on background thread - note duplicate emails ids are removed from mailTo & CC
        private void SendMailOnBackgroundThread(string mailTo, string subject, string CC, string bodyText, Boolean isHTMLBody, string[] attachments, bool UseLoggedInUserAccountToSendMail, string saveToTicketId = null)
        {
            Thread mailThread = new Thread(delegate ()
            {
                SendMailInternal(mailTo, subject, CC, bodyText, isHTMLBody, attachments, UseLoggedInUserAccountToSendMail,saveToTicketId);
            });
            mailThread.Start();
        }

        private Byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            Byte[] ImageData = new Byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            return ImageData; //return the byte data
        }

        private string SendMailInternal(string mailTo, string subject, string CC, string bodyText, Boolean isHTMLBody, string[] attachments, bool UseLoggedInUserAccountToSendMail, string saveToTicketId = null)
        {
            string errorMessage = string.Empty;
            SmtpClient smtpclient = null;
            MailMessage mailMessage = null;
            StreamReader fileStream = null;
            DataRow ticketEmailItem = null;
            try
            {

                // Create TicketMail item if ticket Id passed in
                if (!string.IsNullOrWhiteSpace(saveToTicketId))
                {
                    string moduleId = UGITUtility.ObjectToString(uHelper.getModuleIdByTicketID(_context, saveToTicketId));
                    CreateOutgoingMailInstance(Constants.EmailStatus.InProgress, moduleId, saveToTicketId, mailTo, CC, subject, bodyText, ref ticketEmailItem);
                }

                smtpclient = new SmtpClient();
                string mailReplyTo = "";

                // Remove duplicates
                mailTo = UGITUtility.RemoveDuplicateEmails(mailTo);
                CC = UGITUtility.RemoveDuplicateEmails(CC);

                // If we don't have a To: but have a cc:, move cc: in To: instead
                if (string.IsNullOrEmpty(mailTo) && !string.IsNullOrEmpty(CC))
                {
                    mailTo = CC;
                    CC = null;
                }

                // If we don't have To: or cc:, abort!
                if (string.IsNullOrEmpty(mailTo))
                {
                    errorMessage = string.Format("Received email with empty mailTo: {0}", subject);
                    UpdateEmailSendStatus(ticketEmailItem, Constants.EmailStatus.Failed, errorMessage);
                    ULog.WriteLog(errorMessage);
                    return errorMessage;
                }

                mailMessage = new MailMessage();
                // mailMessage.From = new MailAddress(iniobj.SmtpFrom);
                mailMessage.To.Add(mailTo);
                if (!string.IsNullOrEmpty(mailReplyTo))
                    mailMessage.ReplyToList.Add(new MailAddress(mailReplyTo));

                
                #region Replace tokens like Logo
                if (isHTMLBody)
                {
                    string[] tokens = uHelper.GetMyTokens(bodyText).Distinct().ToArray();
                    foreach (string token in tokens)
                    {
                        switch (token.ToLower())
                        {
                            case "[$logo$]":
                                {
                                    bodyText = bodyText.Replace(token, "<img src=\"cid:companylogo\" alt=\"Logo\"  />");
                                    //Load logo from document library if exist otherwise pick it from template folder
                                    string logoUrl = _configurationVariableHelper.GetValue(ConfigConstants.ReportLogo);
                                    string filePath = null;
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(logoUrl) && !logoUrl.ToLower().Contains("_layouts/15"))
                                            filePath = System.Web.Hosting.HostingEnvironment.MapPath(UGITUtility.GetAbsoluteURL(logoUrl));
                                    }
                                    catch (Exception)
                                    {
                                        // Crashes from timer job if relative path from templates folder
                                    }
                                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                                    {

                                        fileStream = new StreamReader(filePath);
                                        fileStream.BaseStream.Position = 0;
                                        string ContentType = string.Empty;

                                        switch (Path.GetExtension(filePath))
                                        {
                                            case ".svg":
                                                ContentType = "image/svg+xml";
                                                break;
                                            default:
                                                ContentType = "image/jpeg";
                                                break;
                                        }

                                        //LinkedResource logo = new LinkedResource(fileStream.BaseStream, "image/jpeg");
                                        LinkedResource logo = new LinkedResource(fileStream.BaseStream, ContentType);
                                        //LinkedResource logo = new LinkedResource(fileStream.BaseStream);
                                        logo.ContentId = "companylogo";

                                        try
                                        {
                                            AlternateView aView = AlternateView.CreateAlternateViewFromString(bodyText, null, MediaTypeNames.Text.Html);
                                            mailMessage.AlternateViews.Add(aView);
                                            aView.LinkedResources.Add(logo);
                                        }
                                        catch (Exception ex)
                                        {
                                            bodyText = bodyText.Replace(token, string.Empty);
                                            ULog.WriteException(ex);
                                            if (fileStream != null)
                                                fileStream.Dispose();
                                        }
                                    }
                                    else
                                    {
                                        string logoPhysicalImgUrl = System.Web.Hosting.HostingEnvironment.MapPath(logoUrl);
                                        if (System.IO.File.Exists(logoPhysicalImgUrl))
                                        {
                                            try
                                            {
                                                LinkedResource logo = new LinkedResource(logoPhysicalImgUrl, "image/jpeg");
                                                logo.ContentId = "companylogo";
                                                AlternateView aView = AlternateView.CreateAlternateViewFromString(bodyText, null, MediaTypeNames.Text.Html);
                                                mailMessage.AlternateViews.Add(aView);
                                                aView.LinkedResources.Add(logo);
                                            }
                                            catch (Exception ex)
                                            {
                                                bodyText = bodyText.Replace(token, string.Empty);
                                                ULog.WriteException(ex);
                                            }
                                        }
                                        else
                                        {
                                            bodyText = bodyText.Replace(token, string.Empty);
                                        }
                                    }

                                }
                                break;
                            default:
                                {
                                    bodyText = bodyText.Replace(token, string.Empty);
                                }
                                break;
                        }
                    }
                }
                #endregion

                mailMessage.Subject = subject;
                mailMessage.Body = isHTMLBody ? bodyText : UGITUtility.StripHTML(bodyText);
                mailMessage.IsBodyHtml = isHTMLBody;
                if (!string.IsNullOrEmpty(CC))
                {
                    //implement for multiple cc.
                    string EmailCC = CC.Replace(',', ';').Replace(' ', ';');
                    string[] sendEmailCC = EmailCC.Split(';');
                    foreach (string EmailCCitem in sendEmailCC)
                    {
                        if (!string.IsNullOrEmpty(EmailCCitem))
                            mailMessage.CC.Add(EmailCCitem);
                    }
                }

                //mail attachment part.
                if (attachments.Length > 0)
                {
                    foreach (string filename in attachments)
                    {
                        if (!string.IsNullOrEmpty(filename))
                        {
                            mailMessage.Attachments.Add(new Attachment(filename));
                        }
                    }
                }

                // Send message
                //SmtpConfiguration iniobj = new SmtpConfiguration();
                string decryptedCredentials = string.Empty;
                var smtpSettings = _context.ConfigManager.GetValueAsClassObj(ConfigConstants.SmtpCredentials, typeof(SmtpConfiguration)) as SmtpConfiguration;
                string fromEmailDisplayName = _context.ConfigManager.GetValue("FromEmailDisplayName");

                if (string.IsNullOrEmpty(fromEmailDisplayName))
                {
                    //displayName = string.Empty;
                    //NeedToFix:
                    fromEmailDisplayName = "Service Prime Support";
                }

                if (smtpSettings != null)
                {
                    var smtpEmailFrom = uHelper.GetFromEmailId(_context, UseLoggedInUserAccountToSendMail);

                    //NeedToFix: currently emails are going thru ugovernit.com only
                    if (!smtpEmailFrom.EndsWith("@ugovernit.com", StringComparison.InvariantCultureIgnoreCase))
                    {
                        smtpEmailFrom = smtpSettings.SmtpFrom;
                        //fromEmailDisplayName = "Service Prime Support";
                    }

                    if (UseLoggedInUserAccountToSendMail == true)
                        mailMessage.From = new MailAddress(smtpEmailFrom, _context.CurrentUser.Name); //new MailAddress(smtpSettings.SmtpFrom);                                        
                    else
                        mailMessage.From = new MailAddress(smtpEmailFrom, fromEmailDisplayName); //new MailAddress(smtpSettings.SmtpFrom);                                        

                    smtpclient.Host = smtpSettings.Host;
                    smtpclient.Port = smtpSettings.PortNo;
                    smtpclient.EnableSsl = smtpSettings.SSL;
                    smtpclient.TargetName = "STARTTLS/smtp.office365.com";

                    if (string.IsNullOrEmpty(smtpSettings.UserName))
                    {
                        smtpclient.UseDefaultCredentials = true;
                    }
                    else
                    {
                        decryptedCredentials = uGovernITCrypto.Decrypt(smtpSettings.Password, "ugitapass");
                        smtpclient.Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, decryptedCredentials);
                    }
                    smtpclient.Send(mailMessage);

                    if (ticketEmailID != 0)
                    {
                        SetEmailSentStatus(ticketEmailID, Constants.EmailStatus.Delivered);
                        // Add TicketEmailItemId to TicketComment if Comment is added through Add Comment popup and NotifyRequestor is checked
                        if (!string.IsNullOrEmpty(ActionHistory.ActionName) && ActionHistory.ActionName == "addcomment")
                            UpdateTicketComment(Convert.ToString(ticketEmailItem[DatabaseObjects.Columns.TicketId]),UGITUtility.StringToInt(ticketEmailItem[DatabaseObjects.Columns.ID]));
                    }
                }
                else
                {
                    string SMTPFROM = "support@ugovernit.com";// ConfigurationManager.AppSettings["SMTPFROM"];
                    string NetWorkHost = "smtp.office365.com";//ConfigurationManager.AppSettings["NetWorkHost"];
                    string DefaultCredential = ConfigurationManager.AppSettings["DefaultCredential"];
                    string Port = ConfigurationManager.AppSettings["Port"];
                    

                    if (UseLoggedInUserAccountToSendMail == true)
                        mailMessage.From = new MailAddress(SMTPFROM, _context.CurrentUser.Name); //new MailAddress(smtpSettings.SmtpFrom);                                        
                    else
                        mailMessage.From = new MailAddress(SMTPFROM, fromEmailDisplayName); //new MailAddress(smtpSettings.SmtpFrom);      

                    smtpclient.Host = NetWorkHost;
                    smtpclient.Port = Convert.ToInt32(Port);

                    smtpclient.Send(mailMessage);
                    if (ticketEmailID != 0)
                    {
                        SetEmailSentStatus(ticketEmailID, Constants.EmailStatus.Delivered);
                        // Add TicketEmailItemId to TicketComment if Comment is added through Add Comment popup and NotifyRequestor is checked
                        if (!string.IsNullOrEmpty(ActionHistory.ActionName) && ActionHistory.ActionName == "addcomment")
                            UpdateTicketComment(Convert.ToString(ticketEmailItem[DatabaseObjects.Columns.TicketId]), UGITUtility.StringToInt(ticketEmailItem[DatabaseObjects.Columns.ID]));
                    }
                }

                // Log outgoing mail
                string message;
                if (string.IsNullOrEmpty(CC))
                    message = string.Format("SendMail -> Subject: {0}, To: {1}", subject, mailTo);
                else
                    message = string.Format("SendMail -> Subject: {0}, To: {1}, cc: {2}", subject, mailTo, CC);

                ULog.WriteLog(message);
                ULog.WriteUGITLog("", $"{message}\nStatus: {Constants.EmailStatus.Delivered}",Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _context.TenantID);
            }
            catch (SmtpException ex)
            {
                if (string.IsNullOrEmpty(CC))
                    errorMessage = string.Format("SendMail SMTP FAILED -> Subject: {0}, To: {1}", subject, mailTo);
                else
                    errorMessage = string.Format("SendMail SMTP FAILED -> Subject: {0}, To: {1}, cc: {2}", subject, mailTo, CC);

                ULog.WriteException(ex, errorMessage);
                ULog.WriteUGITLog("", $"{errorMessage}\nError: {ex.Message}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _context.TenantID);
                
                if (ticketEmailID != 0)
                {
                    SetEmailSentStatus(ticketEmailID, Constants.EmailStatus.Failed);
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(CC))
                    errorMessage = string.Format("SendMail FAILED -> Subject: {0}, To: {1}", subject, mailTo);
                else
                    errorMessage = string.Format("SendMail FAILED -> Subject: {0}, To: {1}, cc: {2}", subject, mailTo, CC);

                ULog.WriteException(ex, errorMessage);
                ULog.WriteUGITLog("", $"{errorMessage}\nError: {ex.Message}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), _context.TenantID);

                if (ticketEmailID != 0)
                {
                    SetEmailSentStatus(ticketEmailID, Constants.EmailStatus.Failed);
                }
            }
            finally
            {
                if (mailMessage != null)
                    mailMessage.Dispose();

                if (smtpclient != null)
                    smtpclient.Dispose();

                if (fileStream != null)
                    fileStream.Dispose();
            }

            return errorMessage;
        }

        private void SetEmailSentStatus(long ticketEmailID, string status)
        {
            EmailsManager ObjEmailsManager = new EmailsManager(_context);
            Email emailTicketItem = new Email();
            emailTicketItem = ObjEmailsManager.LoadByID(ticketEmailID);
            if (emailTicketItem != null)
            {
                emailTicketItem.EmailStatus = status;
                ObjEmailsManager.Update(emailTicketItem);
            }
        }

        public long CreateOutgoingMailInstance(string emailStatus, string moduleId, string ticketID, string to, string cc, string subject, string body,ref DataRow dataRow)
        {
            string module = uHelper.getModuleNameByModuleId(_context, Convert.ToInt32(moduleId));
            if (string.IsNullOrEmpty(module) && !string.IsNullOrEmpty(ticketID))
                module = uHelper.getModuleNameByTicketId(ticketID);

            var eModel = new Email
            {
                Title = subject,
                EmailIDTo = to,
                EmailIDCC = cc,
                MailSubject = subject,
                ModuleNameLookup = module, //moduleId,
                TicketId = ticketID,
                IsIncomingMail = false,
                EmailStatus = emailStatus,
                EscalationEmailBody = body,
                //EmailIDFrom = "info@ugovernit.com"//SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                EmailIDFrom = uHelper.GetFromEmailId(_context)
            };
            store.Insert(eModel);
            this.ticketEmailID = eModel.ID;
            DataTable dataTable = UGITUtility.ObjectToData(eModel);
            if (dataTable != null && dataTable.Rows.Count > 0)
                dataRow = dataTable.Rows[0];
            //return store.Insert(eModel);
            return eModel.ID;
        }

        private void UpdateEmailSendStatus(DataRow ticketEmailItem, string emailStatus, string errorDetail)
        {
            if (ticketEmailItem == null)
                return;

            ticketEmailItem[DatabaseObjects.Columns.EmailStatus] = emailStatus;
            ticketEmailItem[DatabaseObjects.Columns.EmailError] = errorDetail;
            Email email = store.Get(ticketEmailItem[DatabaseObjects.Columns.ID]);
            if (email != null)
            {
                email.EmailStatus = emailStatus;
                email.EmailError = errorDetail;
                store.Update(email);
            }
        }
        #region Method to add TicketEmail itemId to the ticket comment
        /// <summary>
        /// Method to add TicketEmail itemId to the ticket comment
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="ticketEmailItemId"></param>
        /// <param name="thisWeb"></param>
        public void UpdateTicketComment(string ticketID, int ticketEmailItemId)
        {
            long moduleId = uHelper.getModuleIdByTicketID(_context, ticketID);
            DataRow currentTicket =Ticket.GetCurrentTicket(_context,moduleId,ticketID);

            if (currentTicket != null)
            {
                viewManager = new ModuleViewManager(_context);
                UGITModule uGITModule= viewManager.GetByID(moduleId);
                string comment = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketComment]);
                currentTicket[DatabaseObjects.Columns.TicketComment] = comment + Constants.Separator + Convert.ToString(ticketEmailItemId);
                ticketManager.Save(uGITModule, currentTicket);
            }

            ActionHistory.ActionName = string.Empty;
        }
        #endregion Method to add TicketEmail itemId to the ticket comment
    }
}
