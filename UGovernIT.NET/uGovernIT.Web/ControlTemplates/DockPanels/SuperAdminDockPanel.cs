using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class SuperAdminDockPanel: DockPanel
    {
        public SuperAdminDockPanelSetting superAdminDockPanelSetting { get { return DockSetting as SuperAdminDockPanelSetting; } set { DockSetting = value; } }
        TenantInfoSuperAdmin tenantInfoSuperAdmin = null;
        CurrentTenant currentTenant = null;
        protected override void OnInit(EventArgs e)
        {
            this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){  setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";
                                   
            if (superAdminDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(superAdminDockPanelSetting.Title))
                    this.HeaderText = superAdminDockPanelSetting.Title;
                else
                    this.HeaderText = "Super Admin";
            }

            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            if (Convert.ToString(page.Request.QueryString["ct"]) == "1")
            {
                currentTenant = (CurrentTenant)page.LoadControl("~/ControlTemplates/Admin/CurrentTenant.ascx");
                currentTenant.ID = "CurrentTenant";
                return currentTenant;
            }
            else
            {
                tenantInfoSuperAdmin = (TenantInfoSuperAdmin)page.LoadControl("~/ControlTemplates/Admin/TenantInfoSuperAdmin.ascx");
                tenantInfoSuperAdmin.ID = "SuperAdmin";
                return tenantInfoSuperAdmin;
            }

            
        }
    }
}