using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class LandingPageDockPanel : DockPanel
    {
        public LandingPageDockPanelSetting superAdminDockPanelSettings { get { return DockSetting as LandingPageDockPanelSetting; } set { DockSetting = value; } }
        //UserProfile currentUser;

        //UserProfileManager userManager;



        //TenantInfoSuperAdmin _tenantInfoSuperAdmin = null;
        Control control;
        //TrialUser trialUser = null;
        //TenantInfoSuperAdmin tenantInfoSuperAdmin = null;
        //Purchase purchase = null;

        protected override void OnInit(EventArgs e)
        {
            this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){  setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";

            

            if (superAdminDockPanelSettings != null)
            {
                if (!string.IsNullOrEmpty(superAdminDockPanelSettings.Title))
                    this.HeaderText = superAdminDockPanelSettings.Title;
                else
                    this.HeaderText = "Landing Page";
            }

            
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            var isSuperAdmin = context.UserManager.IsUGITSuperAdmin(context.CurrentUser);

            if (isSuperAdmin)
            {

            }
            else if (Request.Path == "/Pages/Purchase")
            {
                control = (Purchase)page.LoadControl("~/ControlTemplates/Purchase.ascx");
                control.ID = "Purchase";
            }
            else
            {
                control = (TrialUser)page.LoadControl("~/ControlTemplates/Admin/TrialUser.ascx");
                control.ID = "Trail User";
            }

            return control;
        }
    }
}