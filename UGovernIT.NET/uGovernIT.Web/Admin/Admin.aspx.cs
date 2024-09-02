using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Web;
using uGovernIT.Utility;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;

namespace uGovernIT.Web
{
    public partial class Admin : UPage
    {
        UserProfile user;

        UserProfileManager userManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = HttpContext.Current.CurrentUser();

            userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            bool isAdmin = userManager.IsAdmin(user) ||  userManager.IsUGITSuperAdmin(user);

            if (isAdmin)
            {
                AdminCatalog adc = LoadControl("~/ControlTemplates/Admin/AdminCatalog.ascx") as AdminCatalog;

                controlload.Controls.Add(adc);

                Page.Title = Constants.PageTitle.Admin;
            }
            else
            {
                spNotAuthorizedUser.Visible = true;
            }
            if(!IsPostBack)
                Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{user.Name}: Visited Page: Admin.aspx");
        }
    }
}