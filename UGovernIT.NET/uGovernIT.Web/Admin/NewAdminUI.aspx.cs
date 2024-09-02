using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.GlobalPage;
using uGovernIT.Web.ControlTemplates.PMM;
namespace uGovernIT.Web
{
    public partial class NewAdminUI : UPage
    {
        UserProfile user;
        UserProfileManager userManager;



        protected void Page_Load(object sender, EventArgs e)
        {

           
                user = HttpContext.Current.CurrentUser();

                userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

                bool isAdmin = userManager.IsAdmin(user) || userManager.IsUGITSuperAdmin(user) || userManager.IsUGITSuperAdmin(user);

                if (isAdmin)
                {
                    if (Request.QueryString.AllKeys.Contains("deft"))
                    {
                        defaultadmin defaultadmin = LoadControl("~/ControlTemplates/defaultadmin.ascx") as defaultadmin;
                        bool.TryParse(Request["shwD"], out defaultadmin.showDefaultAdminPageOnButtonClick);
                        controlLoad.Controls.Add(defaultadmin);
                    
                    }
                    else
                    {
                        AdministerCore adminCore = LoadControl("~/ControlTemplates/Admin/AdministerCore.ascx") as AdministerCore;
                         controlLoad.Controls.Add(adminCore);
                    }
                    
                    
                    


                    Page.Title = Constants.PageTitle.Admin;
                }
                else
                {
                    notAuthorized.Visible = true;
                }
                Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext().TenantAccountId}|{user.UserName}: Visited Page: NewAdminUI.aspx");
        }
    }
}