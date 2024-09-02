using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Util.Cache;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using DevExpress.Security.Resources;
using uGovernIT.Util.Log;
namespace uGovernIT
{
    public partial class BuildReport : System.Web.UI.Page
    {
        public string ReportName { get; set; }
        public string Name { get; set; }
        public string Mode { get; set; }
        public string ControlName { get; set; }
        public string RootFolder { get; set; }
        public string Prefix { get; set; }
        public string UserId;
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        UserProfileManager UserProfileManager = null;

        protected override void OnInit(EventArgs e)
        {
            UserProfileManager = new UserProfileManager(context);
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            //DevExpress.Utils.UrlAccessSecurityLevelSetting.SecurityLevel = DevExpress.Utils.UrlAccessSecurityLevel.Unrestricted;
            DevExpress.Security.Resources.AccessSettings.DataResources.TrySetRules(UrlAccessRule.Allow(), DirectoryAccessRule.Deny());
            ReportName = Request["reportName"];
            UserId = Request["userId"];
            if (!string.IsNullOrEmpty(UserId))
            {
                UserProfile userProfile = UserProfileManager.GetUserById(UserId);
                if (userProfile != null)
                {
                    context.SetCurrentUser(userProfile);
                    context.TenantID = userProfile.TenantID;
                    context.TenantAccountId = userProfile.AccountId;
                    Session["context"] = context;

                    ULog.Create(context.TenantAccountId, userProfile.Name);
                    ULog.WriteLog($"{context.TenantAccountId}|{userProfile.Name}: Run Report: " + ReportName);
                }
            }
           
            if (!string.IsNullOrEmpty(ReportName))
            {
                if (Request.QueryString.AllKeys.Contains("Filter"))
                    ControlName = ReportName + "_Filter";
                else
                    ControlName = ReportName + "_Viewer";

            }

            Control ctr = Page.LoadControl("~/Reports/DxReport/" + ReportName + "/" + ControlName + ".ascx");
            if (ctr != null)
            {
                DelegatePanel.Controls.Add(ctr);
            }
            base.OnInit(e);
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //Work and It will assign the values to label. 
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    //throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }

        }
    }
}