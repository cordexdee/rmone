using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web
{
    public partial class SuperAdmin : UPage
    {
        UserProfile currentUser;
        UserProfileManager userManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = HttpContext.Current.CurrentUser();
            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var isSuperAdmin = userManager.IsUGITSuperAdmin(currentUser);
            if (isSuperAdmin)
            {
                TenantInfoSuperAdmin tenantInfoSuperAdmin = (TenantInfoSuperAdmin)Page.LoadControl("~/ControlTemplates/Admin/TenantInfoSuperAdmin.ascx");
                controlload.Controls.Add(tenantInfoSuperAdmin);
                Page.Title = Constants.PageTitle.SuperAdmin;
            }
            else
            {
                unAuthorizedUser.Visible = true;
            }
            if(!IsPostBack)
                Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{currentUser.Name}: Visited Page: SuperAdmin.aspx");
        }
    }
}