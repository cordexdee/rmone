
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Linq;
using System.Collections;
using uGovernIT.Utility.Entities;


namespace uGovernIT.Web
{
    public partial class AddTicketEmail : UserControl
    {
        public string ModuleName { get; set; }
        public string publicTicketId { get; set; }
        public string UsersId { get; set; }

        private string localpath;
        private string relativepath;
        private string requestType;
        protected string UrlPrefix = "";
        DataRow ticketitem;
        

        public string EmailBody
        {
            get { return htmlEditorTicketEmailBody.Html; }
            set { }
        }

        public string EmailSubject { get { return txtSubject.Text; } set { } }
        UserProfile User;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserProfileManager userProfileManager = null;
        ConfigurationVariableManager configManager = null;
        ServicesManager servicesManager = null;
        EmailsManager ObjEmailsManager = null;

        protected override void OnInit(EventArgs e)
        {
            userProfileManager = new UserProfileManager(context);
            configManager = new ConfigurationVariableManager(context);
            servicesManager = new ServicesManager(context);
            ObjEmailsManager = new EmailsManager(context);

            User = HttpContext.Current.CurrentUser();
            txtEmailCC.Text = User.Email;
            ppeEmailTo.UserTokenBoxAdd.ValidationSettings.RequiredField.IsRequired = true;
            ppeEmailTo.UserTokenBoxAdd.ValidationSettings.ValidationGroup = "SendTicketEmail";
            ppeEmailTo.UserTokenBoxAdd.ValidationSettings.RequiredField.ErrorText = "Please Enter!";
            ppeEmailTo.UserTokenBoxAdd.ValidationSettings.ErrorDisplayMode = DevExpress.Web.ErrorDisplayMode.Text;

            if (!string.IsNullOrEmpty(UsersId))
                ppeEmailTo.SetValues(UsersId);

            if (Request["sendsurvey"] == "true")
            {
                tremailto.Visible = false;
                tremailcc.Visible = false;
                trTicketCopy.Visible = false;
                trattachment.Visible = false;
                tractionbuttons.Visible = false;

                if (Request["SelectedModule"] != null && Request["SelectedModule"] != "")
                {
                    txtSubject.Text = string.Format("Survey: {0}", Request["surveyName"]);
                    string moduleName = Convert.ToString(Request["SelectedModule"]);
                    string greeting = configManager.GetValue("Greeting");
                    string signature = configManager.GetValue("Signature");

                    string surveyEmailBody = configManager.GetValue("SurveyEmailBody");

                    string emailBody = string.Empty;

                    string tValue = string.Empty;
                    //Load Survey detail for current module
                    Services srv = new Services();
                    if (moduleName.ToLower()=="generic")
                        srv = servicesManager.LoadSurveybySurvey(UGITUtility.StringToInt(Request["ServiceID"]));
                    else
                        srv = servicesManager.LoadSurvey(moduleName);


                    HtmlAnchor a = new HtmlAnchor();
                    if (srv != null && srv.IsActivated)
                    {
                        
                        //tValue = string.Format("<dx:ASPxHyperLink ID='ad' runat='server' EncodeHtml='true' NavigateUrl='{0}?control=serviceswizard&module={1}&ticketid={1}&isdlg=1&ServiceID={2}' Target='_blank'>Please click here to give us your feedback.</dx:ASPxHyperLink>", UGITUtility.ToAbsoluteUrl("/layouts/uGovernIT/uGovernITConfiguration.aspx"), moduleName, srv.ID);
                        tValue = string.Format(" <a href='{0}?control=serviceswizard&module={1}&ticketid={1}&isdlg=1&ServiceID={2}' target='_blank'>Please click here to give us your feedback.<br/> <br/></a>", UGITUtility.ToAbsoluteUrl("/layouts/uGovernIT/uGovernITConfiguration.aspx"), moduleName, srv.ID);

                        //textWithoutTokens = textWithoutTokens.Replace(tokens[i], tValue);
                        //emailBody.AppendFormat(tValue);
                    }

                    if (string.IsNullOrEmpty(surveyEmailBody))
                    {
                        emailBody = string.Format(@"{0} <br/><br/>
                                                Please fill out the survey.<br/><br/> {1}
                                                {2}<br/> ", greeting, tValue, signature);
                    }
                    else
                    {
                        emailBody = string.Format(@"{0} <br/><br/>
                                                {1}.<br/><br/>{2}
                                                {3}<br/>", greeting, surveyEmailBody, tValue, signature);
                    }

                    htmlEditorTicketEmailBody.Html = emailBody;
                }
            }
            else
            {
                ticketitem = Ticket.GetCurrentTicket(context, ModuleName, publicTicketId);
                requestType = Request["type"];

                if (requestType != "projectreport" && requestType != "openTicketReport" && requestType != "projectsummaryreport"
                    && requestType != "tsksummary" && requestType != "tskReport" && requestType != "queryReport" && requestType != "surveyfeedbackreport")
                {
                    // if request[type] is wiki is use for send wiki link.
                    if (requestType == "wiki")
                    {
                        trTicketCopy.Visible = false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UsersId))
                        {
                            string[] arrayUserID = UsersId.Split(',');
                            string strEmail = string.Empty;
                            string strName = string.Empty;
                            foreach (string userItem in arrayUserID)
                            {
                                UserProfile spUserItem = userProfileManager.GetUserById(userItem);
                                if (spUserItem != null)
                                {
                                    if (!string.IsNullOrEmpty(spUserItem.Email))
                                    {
                                        if (strEmail != string.Empty)
                                            strEmail += ";";
                                        strEmail += spUserItem.Email;
                                    }
                                    if (!string.IsNullOrEmpty(spUserItem.Name))
                                    {
                                        if (strName != string.Empty)
                                            strName += ", ";
                                        strName += spUserItem.Name;
                                    }
                                }
                                else
                                {
                                    List<UserProfile> group = userProfileManager.GetUserInfosById(userItem);
                                    if (group != null)
                                    {
                                        foreach (UserProfile oUser in group)
                                        {
                                            if (!string.IsNullOrEmpty(oUser.Email))
                                            {
                                                if (strEmail != string.Empty)
                                                    strEmail += ";";
                                                strEmail += oUser.Email;
                                            }
                                            if (!string.IsNullOrEmpty(oUser.Name))
                                            {
                                                if (strName != string.Empty)
                                                    strName += ", ";
                                                strName += oUser.Name;
                                            }
                                        }
                                    }
                                }
                            }

                            //txtEmailTo.Text = strEmail;
                            
                            string ticketType = UGITUtility.moduleTypeName(ModuleName);
                            txtSubject.Text = string.Format("{0} {1}: {2}", ticketType, ticketitem[DatabaseObjects.Columns.TicketId], ticketitem[DatabaseObjects.Columns.Title]);

                            StringBuilder emailBody = new StringBuilder();
                            emailBody.AppendFormat("Hi {0} <br><br>", strName);
                            emailBody.AppendFormat("You are being contacted in regards to {0} <strong>{1}: {2}</strong>.<br><br>",
                                                   ticketType.ToLower(), ticketitem[DatabaseObjects.Columns.TicketId], ticketitem[DatabaseObjects.Columns.Title]);
                            emailBody.AppendFormat("[Your content goes here]<br><br>Thank you,<br>{0}<br>", User.Name);
                            htmlEditorTicketEmailBody.Html = emailBody.ToString();

                            // Check if storing a copy of outgoing emails is enabled
                            ModuleViewManager moduleManager = new ModuleViewManager(context);
                            DataRow moduleitem = moduleManager.GetDataTable().Select(string.Format("{0}='{1}'",DatabaseObjects.Columns.ModuleName, ModuleName))[0]; //  uGITCache.GetModuleDetails(ModuleName, SPContext.Current.Web);
                            if (moduleitem.Table.Columns.Contains(DatabaseObjects.Columns.StoreTicketEmails) && UGITUtility.StringToBoolean(moduleitem[DatabaseObjects.Columns.StoreTicketEmails]))
                            {
                                trTicketCopy.Visible = true;
                                chbTicketCopy.Checked = true;
                            }
                            else
                            {
                                trTicketCopy.Visible = false;
                                chbTicketCopy.Checked = false;
                            }
                        }
                        else if (requestType == "appaymentdetails" && ModuleName == ModuleNames.VFM)
                        {
                            //SPList icSPList = SPListHelper.GetSPList(DatabaseObjects.Lists.VendorSOWInvoiceDetail);
                            //SPQuery icQuery = new SPQuery();
                            //icQuery.ViewFields = string.Concat(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.SOWInvoiceLookup),
                            //                                   string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.VendorPOLineItemLookup),
                            //                                   string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.InvoiceItemAmount),
                            //                                   string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.InvoiceNumber)
                            //                                   );
                            //icQuery.ViewFieldsOnly = true;
                            //icQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.SOWInvoiceLookup, publicTicketId);
                            //SPListItemCollection icCollecion = icSPList.GetItems(icQuery);
                            //string apEmailTo = uGITCache.GetConfigVariableValue(ConfigConstants.APPaymentEmailTo, spWeb);
                            //ppeEmailTo.CommaSeparatedAccounts = apEmailTo.Trim();
                            //string apEmailCc = uGITCache.GetConfigVariableValue(ConfigConstants.APPaymentEmailCC, spWeb);
                            //if (!string.IsNullOrEmpty(apEmailCc))
                            //    txtEmailCC.Text = apEmailCc.Trim();
                                                            
                            //string apPaymentSubject = ticketitem != null ? Convert.ToString(uHelper.GetSPItemValue(ticketitem, DatabaseObjects.Columns.Title)) : string.Empty;
                            //txtSubject.Text = apPaymentSubject;
                            //string invoiceNo = ticketitem != null ? Convert.ToString(uHelper.GetSPItemValue(ticketitem, DatabaseObjects.Columns.InvoiceNumber)) : string.Empty;
                            //StringBuilder emailBody = new StringBuilder();
                            //emailBody.AppendFormat("Invoice # <b>{0}</b> has been approved for payment.<br><br>", invoiceNo);
                            //emailBody.AppendFormat("Please apply payment as follows:<br><br>");
                            //if (icCollecion != null && icCollecion.Count > 0)
                            //{
                            //    emailBody.AppendFormat("<table style='width:500px !important;text-align:left !important;border-collapse: collapse !important;border-top: 2px solid !important;border-bottom: 2px solid !important;'><tr><th align='center' style='border-bottom: 2px solid !important;'>PO Line #</th><th align='right' style='border-bottom: 2px solid !important;padding-right:4px;'>Total</th></tr>");
                            //    DataTable invoiceData = icCollecion.GetDataTable();
                            //    var groupByPOLineItem = invoiceData.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.VendorPOLineItemLookup)).OrderBy(x => x.Key);
                            //    foreach (var polineitem in groupByPOLineItem)
                            //    {
                            //        StringBuilder temrow = new StringBuilder();
                            //        temrow.AppendFormat("<tr>");
                            //        temrow.AppendFormat("<td align='center'>{0}</td>", polineitem.Key);
                            //        DataRow[] drColl = polineitem.ToArray();
                            //        double poItemTotal = 0;
                            //        if (drColl != null && drColl.Length > 0)
                            //        {
                            //            double.TryParse(Convert.ToString(drColl.CopyToDataTable().Compute(string.Format("Sum({0})", DatabaseObjects.Columns.InvoiceItemAmount), string.Empty)), out poItemTotal);
                            //            poItemTotal = Math.Round(poItemTotal, 2);
                            //        }
                            //        if (poItemTotal == 0)//do not add the po line item having total =0
                            //            continue;

                            //        temrow.AppendFormat("<td align='right' style='padding-right:4px;'>{0}</td>", uHelper.FormatNumber(poItemTotal, "withdollaronly"));

                            //        temrow.AppendFormat("</tr>");

                            //        emailBody.AppendFormat(temrow.ToString());
                            //    }

                            //    emailBody.AppendFormat("<tr>");
                            //    emailBody.AppendFormat("<td align='center' style='border-top: 2px solid !important;'><b>Total: </b></td>");
                            //    if (invoiceData != null && invoiceData.Rows.Count > 0)
                            //        emailBody.AppendFormat("<td align='right' style='border-top: 2px solid !important;padding-right:4px;'>{0}</td>", uHelper.FormatNumber(Math.Round(Convert.ToDouble(invoiceData.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.InvoiceItemAmount), string.Empty)), 2), "withdollaronly"));
                            //    else
                            //        emailBody.AppendFormat("<td align='right' style='border-top: 2px solid !important;padding-right:4px;'>$0</td>");

                            //    emailBody.AppendFormat("</tr></table><br><br>");
                            //}
                            //else
                            //{
                            //    emailBody.AppendFormat("<table style='width:500px !important;text-align:left !important;border-collapse: collapse !important;border-top: 2px solid !important;border-bottom: 2px solid !important;'><tr><th align='center' style='border-bottom: 2px solid !important;'>PO Line #</th><th align='right' style='border-bottom: 2px solid !important;padding-right:4px;'>Total</th></tr>");
                            //    emailBody.AppendFormat("<tr>");
                            //    emailBody.AppendFormat("<td align='center' style='border-top: 2px solid !important;'><b>Total: </b></td>");
                            //    emailBody.AppendFormat("<td align='right' style='border-top: 2px solid !important;padding-right:4px;'>$0</td>");
                            //    emailBody.AppendFormat("</tr></table><br><br>");

                            //}

                            //string apEmailSignature = uGITCache.GetConfigVariableValue(ConfigConstants.APPaymentEmailSignature, spWeb);
                            //emailBody.AppendFormat(apEmailSignature);
                            //htmlEditorTicketEmailBody.Html = emailBody.ToString();
                            //trTicketCopy.Visible = false;
                        }
                    }
                }
                else
                {
                    trTicketCopy.Visible = false;
                    localpath = Request["localpath"];
                    if (localpath != string.Empty)
                    {
                        relativepath = Request["relativepath"];
                        string fileName = localpath.Substring(localpath.LastIndexOf('\\') + 1, localpath.Length - (localpath.LastIndexOf('\\') + 1));
                        HtmlAnchor path = new HtmlAnchor() { ID = "aprojectReport", InnerText = fileName, HRef = UGITUtility.GetAbsoluteURL(relativepath) };
                        pAttachment.Controls.Add(path);
                    }
                    switch (requestType)
                    {
                        case "openTicketReport":
                            txtSubject.Text = "Open Ticket Summary Report";
                            break;
                        case "projectreport":
                            txtSubject.Text = "Project Status Report";
                            break;
                        case "tskReport":
                            txtSubject.Text = "Task Project Status Report";
                            break;
                        case "tsksummary":
                            txtSubject.Text = "Task Summary Report";
                            break;
                        case "queryReport":
                            txtSubject.Text = "Query Report";
                            break;
                        case "projectsummaryreport":
                            txtSubject.Text = "Project Summary Report";
                            break;
                        case "surveyfeedbackreport":
                            txtSubject.Text = "Survey Feedback Report";
                            break;
                        default:
                            break;
                    }

                    htmlEditorTicketEmailBody.Html = "Hi, <br> <br> [your content goes here]<br><br>Regards,";
                }
            }
        }
        private StringBuilder GetUserEmailId()
        {
            StringBuilder userEmailTo = new StringBuilder();
            List<string> userId = UGITUtility.ConvertStringToList(ppeEmailTo.GetValues(), ",");
            foreach (string entity in userId)
            {
                User = userProfileManager.GetUserById(entity);

                if (User == null) // for User Groups
                {
                    List<UserProfile> lstGroupUsers = userProfileManager.GetUsersByGroupID(entity);

                    foreach (UserProfile userProfileItem in lstGroupUsers)
                    {
                        User = userProfileManager.GetUserById(userProfileItem.Id);

                        if (User != null && !string.IsNullOrEmpty(User.Email))
                        {
                            if (userEmailTo.Length != 0)
                                userEmailTo.Append(";");
                            //userEmailTo.Append(User.Email);
                            userEmailTo.Append(!string.IsNullOrEmpty(User.NotificationEmail) ? User.NotificationEmail : User.Email);
                        }
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(User.Email))
                    {
                        if (userEmailTo.Length != 0)
                            userEmailTo.Append(";");
                        userEmailTo.Append(!string.IsNullOrEmpty(User.NotificationEmail) ? User.NotificationEmail : User.Email);
                    }
                }
            }
            return userEmailTo;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
            if (!Page.IsValid)
                return;

            //Replace img src with absolute url
            if (!string.IsNullOrWhiteSpace(htmlEditorTicketEmailBody.Html))
                htmlEditorTicketEmailBody.Html = UGITUtility.ReplaceImageSrcWithAbsoluteUrl(htmlEditorTicketEmailBody.Html);
            
            string mailTo = string.Empty;
            //SPDelta 155(Start:-Survey complete functionality)
            //if (ppeEmailTo.Attributes.Count > 0) //SPDelta 155(Commented:-Survey complete functionality)
            //{
            //    StringBuilder userEmailTo = GetUserEmailId();
            //    if (userEmailTo.Length > 0)
            //        mailTo += string.Format("{0}", userEmailTo);
            //}//SPDelta 155(Commented:-Survey complete functionality)
            if (!string.IsNullOrWhiteSpace(ppeEmailTo.GetValues()))
            {
                StringBuilder userEmailTo = GetUserEmailId();
                if (userEmailTo.Length > 0)
                    mailTo += string.Format("{0}", userEmailTo);
            }
            //SPDelta 155(End:-Survey complete functionality)


            // if request[type] is wiki is use for send wiki link.
            if (requestType == "wiki")
            {
                string[] attachments;
                HttpFileCollection file = Request.Files;
                attachments = new string[file.Count];
                for (int i = 0; i < file.Count; i++)
                {
                    if (file[i] != null && file[i].ContentLength > 0 && !string.IsNullOrEmpty(file[i].FileName))
                    {
                        HttpPostedFile filetype = Request.Files[i];

                        string outputFilePath = string.Empty;
                        string outputPath = uHelper.GetTempFolderPath();
                        outputFilePath = System.IO.Path.Combine(outputPath, file[i].FileName);
                        if (File.Exists(outputFilePath))
                        {
                            File.Delete(outputFilePath);
                        }
                        filetype.SaveAs(outputFilePath);
                        attachments[i] = outputFilePath;

                        if (filetype.ContentType.Contains("application/x-msdownload"))
                        {
                            lblerror.Visible = true;
                            lblerror.Text = file[i].FileName + " has invalid file.";
                            return;
                        }

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(Request.Files[i].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Request.Files[i].ContentLength);
                        }
                    }
                }

                string wikilink = string.Format("<a href='{0}&ticketId={1}'>{1}: {2}</a>", UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails"),
                                                publicTicketId, ticketitem[DatabaseObjects.Columns.Title]);
                string TicketemailBody = string.Format("{0}<br />Please click here to visit the wiki:<br />{1}", htmlEditorTicketEmailBody.Html, wikilink);
                
                if (!string.IsNullOrEmpty(mailTo))
                {
                    MailMessenger mail = new MailMessenger(context);
                    mail.SendMail(mailTo, txtSubject.Text, txtEmailCC.Text, TicketemailBody, true, attachments);
                }
            }
            else if (requestType == "projectreport" || requestType == "openTicketReport" || requestType == "projectsummaryreport"
                || requestType == "queryReport" || requestType == "tskReport" || requestType == "tsksummary" || requestType == "surveyfeedbackreport" || requestType == "appaymentdetails")
            {
                string TicketemailBody = htmlEditorTicketEmailBody.Html;

                string[] attachments = new string[1];
                attachments[0] = string.Empty;

                if(!string.IsNullOrEmpty(localpath))
                attachments[0] = localpath;

                string ticketPublicId = string.Empty;
                if(requestType== "appaymentdetails")
                {
                    ticketPublicId = Request["currentTicketPublicID"];
                 
                }

                if (!string.IsNullOrEmpty(mailTo))
                {
                    MailMessenger mail = new MailMessenger(context);
                    mail.SendMail(mailTo, txtSubject.Text, txtEmailCC.Text, TicketemailBody, true, attachments);
                }
            }
            else
            {
                /*
                DataRow emailTicketItem = null;
                if (chbTicketCopy.Checked)
                {
                    //DataTable lcList = SPListHelper.GetSPList(DatabaseObjects.Tables.TicketEmails);
                    //emailTicketItem = lcList.AddItem();
                }
                */

                Email emailTicketItem = new Email();

                string TicketemailBody = string.Format("{0}<br>{1}", htmlEditorTicketEmailBody.Html, uHelper.GetTicketDetailsForEmailFooter(context, ticketitem, ModuleName, true));

                string[] attachments;
                HttpFileCollection file = Request.Files;
                attachments = new string[file.Count];
                for (int i = 0; i < file.Count; i++)
                {
                    if (file[i] != null && file[i].ContentLength > 0 && !string.IsNullOrEmpty(file[i].FileName))
                    {
                        HttpPostedFile filetype = Request.Files[i];

                        string outputFilePath = string.Empty;
                        string outputPath = uHelper.GetTempFolderPath();
                        System.IO.FileInfo fileInfo= new System.IO.FileInfo(file[i].FileName);
                        string fileName = string.Empty;
                        if (fileInfo != null)
                            fileName = fileInfo.Name;
                        if (string.IsNullOrEmpty(fileName))
                            continue;
                        outputFilePath = System.IO.Path.Combine(outputPath, fileName);
                        if (File.Exists(outputFilePath))
                        {
                            File.Delete(outputFilePath);
                        }
                        filetype.SaveAs(outputFilePath);
                        attachments[i] = outputFilePath;

                        if (filetype.ContentType.Contains("application/x-msdownload"))
                        {
                            lblerror.Visible = true;
                            lblerror.Text = file[i].FileName + " has invalid file.";
                            return;
                        }

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(Request.Files[i].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Request.Files[i].ContentLength);
                        }

                        //if (chbTicketCopy.Checked)
                        //    emailTicketItem.Attachments.Add(file[i].FileName, fileData);
                    }
                }

                long ticketEmailID = 0; 
                if (chbTicketCopy.Checked)
                {
                    /*
                    emailTicketItem[DatabaseObjects.Columns.Title] = txtSubject.Text;
                    emailTicketItem[DatabaseObjects.Columns.EmailIDTo] = mailTo;
                    emailTicketItem[DatabaseObjects.Columns.EmailIDCC] = txtEmailCC.Text;
                    emailTicketItem[DatabaseObjects.Columns.MailSubject] = txtSubject.Text;
                    emailTicketItem[DatabaseObjects.Columns.EscalationEmailBody] = TicketemailBody;
                    emailTicketItem[DatabaseObjects.Columns.TicketId] = publicTicketId;
                    //emailTicketItem[DatabaseObjects.Columns.ModuleNameLookup] = UGITUtility.getModuleIdByModuleName(ModuleName);
                    //emailTicketItem[DatabaseObjects.Columns.EmailIDFrom] = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                    emailTicketItem[DatabaseObjects.Columns.IsIncomingMail] = false;
                    emailTicketItem[DatabaseObjects.Columns.EmailStatus] = Constants.EmailStatus.InProgress;

                    //emailTicketItem.UpdateOverwriteVersion();

                    //ticketEmailID = emailTicketItem.ID;
                    */

                    emailTicketItem.Title = txtSubject.Text;
                    emailTicketItem.EmailIDTo = mailTo;
                    emailTicketItem.EmailIDCC = txtEmailCC.Text;
                    emailTicketItem.MailSubject = txtSubject.Text;
                    emailTicketItem.EscalationEmailBody = TicketemailBody;
                    emailTicketItem.TicketId = publicTicketId;
                    emailTicketItem.ModuleNameLookup = ModuleName;
                    emailTicketItem.EmailIDFrom = MailHelper.GetFromEmailId(false);
                    emailTicketItem.IsIncomingMail = false;
                    emailTicketItem.EmailStatus = Constants.EmailStatus.InProgress;

                    ObjEmailsManager.Insert(emailTicketItem);
                    ticketEmailID = emailTicketItem.ID;
                }

                if (!string.IsNullOrEmpty(mailTo))
                {
                    MailMessenger mail = new MailMessenger(context);
                    mail.ticketEmailID = ticketEmailID;
                    mail.SendMail(mailTo, txtSubject.Text, txtEmailCC.Text, TicketemailBody, true, attachments);
                }
            }

            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void htmlEditorTicketEmailBody_HtmlCorrecting(object sender, DevExpress.Web.ASPxHtmlEditor.HtmlCorrectingEventArgs e)
        {
            e.Handled = true;
        }

        protected void htmlEditorTicketEmailBody_ImageFileSaving(object source, DevExpress.Web.FileSavingEventArgs e)
        {
            if (e.IsValid)
            {
                string filename = GetImageFilePath(htmlEditorTicketEmailBody.SettingsDialogs.InsertImageDialog.SettingsImageUpload.UploadFolder, e.UploadedFile.FileName);
                using (System.Drawing.Image image = GetImageToSave(e.UploadedFile.FileContent))
                {
                    image.Save(MapPath(filename), image.RawFormat);
                }
                e.SavedFileUrl = ResolveClientUrl(filename);
                e.Cancel = true;
            }
        }
        private string GetImageFilePath(string folder, string fileNameWithExtension)
        {
            string fileExtenstion = Path.GetExtension(fileNameWithExtension);
            string fileName = Path.GetFileNameWithoutExtension(fileNameWithExtension);
            string mappedFolder = MapPath(folder);
            string tmpFileName = fileName + fileExtenstion;
            int uniqueCheckCounter = 1;
            while (File.Exists(Path.Combine(mappedFolder, tmpFileName)))
                tmpFileName = string.Format("{0}({1}){2}", fileName, uniqueCheckCounter++, fileExtenstion);
            return folder + tmpFileName;
        }
        private System.Drawing.Image GetImageToSave(Stream stream)
        {
            System.Drawing.Image uploadedImage = System.Drawing.Image.FromStream(stream);
            return uploadedImage;
        }
        protected void cvTO_ServerValidate(object source, ServerValidateEventArgs args)
        {
            List<UserProfile> userList = userProfileManager.GetUserInfosById(ppeEmailTo.GetValues());
            if (userList.Count == 0)
            {
                args.IsValid = false;
            }
        }
    }
}
