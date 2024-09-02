using DevExpress.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Web
{
    public partial class defaultadmin : System.Web.UI.UserControl
    {
        protected string newTask = UGITUtility.GetAbsoluteURL("/Pages/RMM") + "?fromMail&mnu";
        public ApplicationContext _context = null;
        public ConfigurationVariableManager ObjConfigVariable = null;
        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        public string TenantName = string.Empty;
        public bool showDefaultAdminPageOnButtonClick = false;
        public string AssemblyVersion = string.Empty;
        public defaultadmin()
        {
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _context = HttpContext.Current.GetManagerContext();
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            ObjConfigVariable = new ConfigurationVariableManager(_context);
            string tenantName = ObjConfigVariable.GetValue("AccountName");
            TenantName = !string.IsNullOrEmpty(tenantName) ? tenantName : _context.TenantAccountId;
            UserProfile user = new UserProfile();
            user = HttpContext.Current.CurrentUser();
            if (user != null && user.IsShowDefaultAdminPage == false)
            {
                skiplogin.Visible = false;
            }
            if (user != null && user.IsShowDefaultAdminPage == false && !showDefaultAdminPageOnButtonClick)
            {
                Response.Redirect("~/Pages/UserHomePage");

            }
        }



        protected void skiplogin_CheckedChanged(object sender, EventArgs e)
        {
            UserProfile user = new UserProfile();
            user = HttpContext.Current.CurrentUser();
            if (user != null)
            {
                user.IsShowDefaultAdminPage = false;
                var result = umanager.Update(user);

            }

            Response.Redirect("~/Pages/UserHomePage");
        }
    }
}