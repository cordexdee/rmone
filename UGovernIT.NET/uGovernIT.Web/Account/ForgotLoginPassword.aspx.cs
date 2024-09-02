using DevExpress.ExpressApp.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Helpers;
using Constants = uGovernIT.Utility.Constants;

namespace uGovernIT.Web.Account
{
    public partial class ForgotLoginPassword : System.Web.UI.Page
    {
        //IdentityResult result;
        UserProfileManager umanager;
        string aspnetUserName = string.Empty;
        ApplicationContext context = null;
        public string AssemblyVersion = string.Empty;
        public string DefaultTenant { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            context = HttpContext.Current.GetManagerContext();
            if (ConfigurationManager.AppSettings["DefaultTenant"] != null)
            {
                DefaultTenant = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            }
            if (DefaultTenant.Equals("uGovernIT"))
            {
                logoImage.Visible = true;
                leftImage.Visible = true;
            }
        }

        protected void ForgetPasswordAction(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            var accountId = accountIdTextBox.Text;
            var userName = userNameTextBox.Text;
            DataTable TenantDetails = GetTableDataManager.GetTenantDataUsingQueries($"select * from {DatabaseObjects.Tables.Tenant} where  AccountId = '{accountId}'");
            if (TenantDetails == null || TenantDetails.Rows.Count == 0)
            {
                errorMessage = Constants.UserAccountMessage.IsTenantDeleted;
                lblMesgForAccount.Text = errorMessage;
                Util.Log.ULog.WriteLog($"errMessage: {errorMessage}");
                lblMesgForUserName.Text = string.Empty;
                lblMessage.Text = string.Empty;
            }
            else if (TenantDetails != null && TenantDetails.Rows.Count > 0)
            {
                string Name = string.Empty;
                string tenantId = TenantDetails.Rows[0]["TenantId"].ToString();
                string Query = $"Select * from AspNetUsers  where UserName = '{userName}' and enabled = 1 and TenantID = '{tenantId}'";
                DataTable userData = GetTableDataManager.ExecuteQuery(Query);
                if (userData != null && userData.Rows.Count > 0)
                {
                    Name = userData.Rows[0][DatabaseObjects.Columns.Name].ToString();
                    aspnetUserName = userData.Rows[0][DatabaseObjects.Columns.UserName].ToString();
                }
                if (!accountId.Equals(TenantDetails.Rows[0]["AccountID"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    errorMessage = Constants.UserAccountMessage.IsAccountIdExist;
                    lblMesgForAccount.Text = errorMessage;
                    Util.Log.ULog.WriteLog($"errMessage: {errorMessage}");
                    lblMesgForUserName.Text = string.Empty;
                    lblMessage.Text = string.Empty;
                }
                else if (!userName.EqualsIgnoreCase(aspnetUserName))
                {
                    errorMessage = Constants.UserAccountMessage.IsUseNameExist;
                    lblMesgForUserName.Text = errorMessage;
                    Util.Log.ULog.WriteLog($"errMessage: {errorMessage}");
                    lblMesgForAccount.Text = string.Empty;
                    lblMessage.Text = string.Empty;
                }
                else
                {
                    var _context = ApplicationContext.CreateContext(tenantId); 
                    EmailHelper emailHelper = new EmailHelper(_context);
                    var userId = userData.Rows[0][DatabaseObjects.Columns.ID].ToString();
                    ConfigurationVariableManager ConfigurationVariableManager = new ConfigurationVariableManager(_context);
                    string supportEmailId = ConfigurationVariableManager.GetValue(ConfigConstants.SupportEmail);
                    string tenantName = TenantDetails.Rows[0]["TenantName"].ToString();
                    var userEmail = userData.Rows[0][DatabaseObjects.Columns.EmailID].ToString();
                    if (uHelper.IsValidEmail(userName))
                        userEmail += "," + userName;
                    accountIdTextBox.Value = string.Empty;
                    userNameTextBox.Value = string.Empty;
                    emailHelper.SendForgetPasswordMailToUser(QueryString.Encode(userId + Constants.Separator + tenantId), accountId, Name, userName, userEmail, supportEmailId, tenantName);
                    lblMessage.Text = "An e-mail will be sent to reset your password.  Please check your inbox or spam folder.";
                    submitButton.Visible = false;
                    userNameTextBox.Visible = false;
                    accountIdTextBox.Visible = false;
                    lblMesgForUserName.Text = string.Empty;
                    lblMesgForAccount.Text = string.Empty;
                }
            }
        }

        protected void SendMailToUserAboutForgetPassword(string tenantId,string accountId, string Name,string userName, string password, string userEmail)
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
            sb.AppendFormat("If you did not personally request this change, please contact support. at support@servicerime.com");
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
            var mail = new MailMessenger(HttpContext.Current.GetManagerContext());
             var response = mail.SendMail(userEmail, subject, "", sb.ToString(), true);


        }
    }
}