using System;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class EmailHelper
    {
        private ApplicationContext _applicationContext;
        private ConfigurationVariableManager _configVariable = null;
        public EmailHelper(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _configVariable = new ConfigurationVariableManager(_applicationContext);
        }

        public string SendEmailToTenantAdminAccount(string receiverAddress, string accountId, string userName, string password,string email = null)
        {
            string accountIdForMail = uGovernITCrypto.Encrypt(accountId, "ugitpass");
            string userNameForMail = uGovernITCrypto.Encrypt(userName, "ugitpass");
            string passwordForMail = uGovernITCrypto.Encrypt(password, "ugitpass");
            var sb = new StringBuilder();

            string productsubject = UGITUtility.ObjectToString(ConfigurationManager.AppSettings["ProductEmailSubject"]);
            string productName = UGITUtility.ObjectToString(ConfigurationManager.AppSettings["ProductName"]);
            string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/TenantLogin?id={accountIdForMail}&acc={userNameForMail}&di={passwordForMail}";
            string credentialsMessage = _configVariable.GetValue(ConfigConstants.CredentialsMail);
            var mail = new MailMessenger(_applicationContext);
            if (string.IsNullOrEmpty(credentialsMessage))
            {
                // html body
                sb.Append("<html>");
                sb.Append("<head></head>");
                sb.Append("<body>");
                sb.Append("<p>");
                sb.Append("Welcome to "+ productName + "!");
                sb.Append("<br>");
                sb.Append("Your account details are as follows.");
                sb.Append("<br>");
                sb.Append("<strong>");
                sb.AppendFormat("Account Id : {0}", accountId);
                sb.Append("<br>");
                sb.AppendFormat("User Id : {0}", userName);
                sb.Append("<br>");
                sb.AppendFormat("Password : {0}", password);
                sb.Append("<br>");
                sb.AppendFormat("Email : {0}", email);
                sb.Append("<br>");
                sb.Append("</strong>");
                sb.Append("<br>");
                sb.Append("<br>");
                sb.AppendFormat("Please <a href={0}> click here  </a> to start your trial.", SiteUrl);
                sb.Append("<br>");
                sb.Append("<br>");
                sb.Append("Thanks & Regards");
                sb.Append("<br>");
                sb.Append("HelpDesk Team");
                sb.Append("<br>");
                sb.Append(productName);
                sb.Append("<br>");
                sb.Append("</p>");
                sb.Append("</body>");
                sb.Append("</html>");
                return mail.SendMail(receiverAddress, productsubject, "", sb.ToString(), true); 
            }
            else
            {
                return mail.SendMail(receiverAddress, productsubject, "", credentialsMessage.Replace("[$accountid$]", accountId).Replace("[$username$]", userName).Replace("[$password$]", password).Replace("[$email$]", email).Replace("[$url$]", SiteUrl), true);
            }
        }


        public string SendVerficationEmail(string receiverAddress, string id)
        {
            var sb = new StringBuilder();
            string subject = UGITUtility.ObjectToString(ConfigurationManager.AppSettings["ProductEmailVerificationSubject"]);
            string productName = UGITUtility.ObjectToString(ConfigurationManager.AppSettings["ProductName"]);
            string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/TenantCreation?id={id}";

            string verificationMessage = _configVariable.GetValue(ConfigConstants.VerificationMail);
            var mail = new MailMessenger(_applicationContext);

            if (string.IsNullOrEmpty(verificationMessage))
            {
                // html body
                sb.Append("<html>");
                sb.Append("<head></head>");
                sb.Append("<body>");
                sb.Append("<p>");
                sb.Append("Welcome to " + productName + "!");
                sb.Append("<br>");
                sb.Append("Please verify your email by clicking on the link below:");
                sb.Append("<br>");
                sb.Append("<strong>");
                sb.Append("</strong>");
                sb.Append("<br>");
                sb.AppendFormat("Please <a href={0}> click here  </a> to verify your email.", SiteUrl);
                sb.Append("<br>");
                sb.Append("<br>");
                sb.Append("Thanks & Regards");
                sb.Append("<br>");
                sb.Append("HelpDesk Team");
                sb.Append("<br>");
                sb.Append(productName);
                sb.Append("<br>");
                sb.Append("</p>");
                sb.Append("</body>");
                sb.Append("</html>");
                return mail.SendMail(receiverAddress, subject, "", sb.ToString(), true); 
            }
            else
            {
                return mail.SendMail(receiverAddress, subject, "", verificationMessage.Replace("[$url$]", SiteUrl), true);
            }
        }

        public string SendEmailForPurchaseNotification(string receiverAddress, string UserName)
        {
            string subject = $"{ UserName } Purchase!";
            StringBuilder sb = new StringBuilder();
            sb.Append("<Html>");
            sb.Append("<head></head>");
            sb.Append("<body>");
            sb.Append("Dear Service Prime members,  ");
            sb.Append("<br>");
            sb.AppendFormat("This is to inform you that tenant {0} has placed a request to purchase Service Prime.", UserName);
            sb.Append("<br>");
            sb.AppendFormat("Please get in touch with the tenant {0} team.", UserName);
            sb.Append("<br>");
            sb.Append("</body>");
            sb.Append("</Html>");
            var mail = new MailMessenger(_applicationContext);
            return mail.SendMail(receiverAddress, subject, "", sb.ToString(), true);
        }


        public void SendMailToUserAboutForgetPassword(string tenantId, string accountId, string Name, string userName, string password,string email)
        {
            string accountIdForMail = uGovernITCrypto.Encrypt(accountId, "ugitpass");
            string userNameForMail = uGovernITCrypto.Encrypt(userName, "ugitpass");
            string passwordForMail = uGovernITCrypto.Encrypt(password, "ugitpass");
            string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/TenantLogin?id={accountIdForMail}&acc={userNameForMail}&di={passwordForMail}";

            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head></head>");
            sb.Append("<body>");

            sb.Append("<p>");
            sb.Append("Welcome to Service Prime!");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append($"Hello {Name}");
            sb.Append("<br>");
            // sb.Append("<strong>");
            //// sb.AppendFormat("Service Prime Ticket : {0} created and your password has been reset", ticketId);
            // sb.Append("</strong>");
            // sb.Append("<br>");
            sb.AppendFormat("Please find details below:");
            sb.Append("<br>");
            sb.AppendFormat("AccountId: {0}", accountId);
            sb.Append("<br>");
            sb.AppendFormat("Username: {0}", userName);
            sb.Append("<br>");
            sb.AppendFormat("Password: {0}", password);
            sb.Append("<br>");
            sb.Append("<br>");
            sb.AppendFormat("Please <a href={0}> click here  </a> to  login with new password.", SiteUrl);
            sb.AppendFormat("<br>");
            sb.AppendFormat("If you did not personally request this change, please contact support. at support@serviceprime.com");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("Thanks & Regards");
            sb.Append("<br>");
            sb.Append("HelpDesk Team");
            sb.Append("<br>");
            sb.Append("Service Prime");
            sb.Append("<br>");

            sb.Append("</p>");
            sb.Append("</body>");
            sb.Append("</html>");
            String subject = "Your password is successfully reset!";
            var mail = new MailMessenger(_applicationContext);
            var response = mail.SendMail(email, subject, "", sb.ToString(), true);
        }

        public void SendForgetPasswordMailToUser(string user, string accountId, string Name, string userName, string email ,string supportemailid, string tenantName)
        {
            string ResetPasswordUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}Account/ChangePasswordNew?accid={user}&resetUserPwd=1";

            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head></head>");
            sb.Append("<body>");

            sb.Append("<p>");
            sb.AppendFormat("Welcome to {0}!", tenantName);
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append($"Hello {Name}");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.AppendFormat("Please find details below:");
            sb.Append("<br>");
            sb.AppendFormat("AccountId: {0}", accountId);
            sb.Append("<br>");
            sb.AppendFormat("Username: {0}", userName);
            sb.Append("<br>");
            
            sb.Append("<br>");
            sb.AppendFormat("Please <a href={0}> click here  </a> to  change the password.", ResetPasswordUrl);
            sb.AppendFormat("<br>");
            sb.AppendFormat("If you did not personally request this change, please contact support. at {0}", supportemailid);
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("Thanks & Regards,");
            sb.Append("<br>");
            sb.Append("HelpDesk Team");
            sb.Append("<br>");
            sb.Append(tenantName);
            sb.Append("<br>");

            sb.Append("</p>");
            sb.Append("</body>");
            sb.Append("</html>");
            String subject = string.Format("{0} account password reset", tenantName);
            var mail = new MailMessenger(_applicationContext);
            mail.SendMail(email, subject, "", sb.ToString(), true, true);                            
        }

        public object SendEmailToSuperAdmin(AccountInfo accountInfo)
        {
            var sb = new StringBuilder();
            string subject = "New Tenant has been created successfully!";

            // html body
            sb.Append("<html>");
            sb.Append("<head></head>");
            sb.Append("<body>");
            sb.Append("<p>");
            sb.Append("Hi Super Admin,");
            sb.Append("<br>");
            sb.Append($"New Tenant created on {ConfigurationManager.AppSettings["SiteUrl"]} with below details:");
            sb.Append("<br>");
            sb.Append("<strong>");
            sb.AppendFormat("Account Id : {0}", accountInfo.AccountID);
            sb.Append("<br>");
            sb.AppendFormat("User Id : {0}", accountInfo.UserName);
            sb.Append("<br>");
            sb.AppendFormat("Email : {0}", accountInfo.Email);
            sb.Append("<br>");
            sb.Append("</strong>");
            sb.Append("<br>");
            sb.Append("<br>");
            sb.Append("Thanks & Regards");
            sb.Append("<br>");
            sb.Append("HelpDesk Team");
            sb.Append("<br>");
            sb.Append("Service Prime");
            sb.Append("<br>");
            sb.Append("</p>");
            sb.Append("</body>");
            sb.Append("</html>");

            UserRoleManager uRole = new UserRoleManager(_applicationContext);
            var Role = uRole.GetRoleByName("UGITSuperAdmin");
            if (Role != null)
            {
                var users = _applicationContext.UserManager.GetUsersByGroupID(Role.Id);
                var mail = new MailMessenger(_applicationContext);
                return mail.SendMail(string.Join(";", users.Select(x => x.Email).ToList()), subject, "", sb.ToString(), true);
            }

            return string.Empty;
        }
    }
}
