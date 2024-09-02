using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class ServiceDockPanel : DockPanel
    {
        public ServiceDockPanelSetting serviceDockPanelSetting { get { return DockSetting as ServiceDockPanelSetting; } set { DockSetting = value; } }
        uGovernITHomeUserControl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (serviceDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(serviceDockPanelSetting.Title))
                    this.HeaderText = serviceDockPanelSetting.Title;
                else
                    this.HeaderText = "Home Service Panel";
            }
            
            base.OnInit(e);
        }
        public override Control LoadControl(Page page)
        {
            control = (uGovernITHomeUserControl)page.LoadControl("~/ControlTemplates/shared/uGovernITHomeUserControl.ascx");
            control.ID = "servicedoc";
            if (serviceDockPanelSetting != null)
            {
                control.WelcomeDesc = string.Empty;
                control.EnableNewButton = false;
                control.WelcomeHeading = string.Empty;
                control.UpdateWaitingOnMeTab = string.Empty;
                control.UpdateMyTickets = string.Empty;
                control.UpdateMyTasks = string.Empty;
                control.UpdateMyProject = string.Empty;
                control.UpdateMyPendingApprovals = string.Empty;
                control.UpdateMyDepartmentTickets = string.Empty;
                control.UpdateMyClosedTickets = string.Empty;
                control.ShowServiceCatalog = serviceDockPanelSetting.ShowServiceCatalog;
                control.ServiceCatalogOrder = 0;
                control.NoOfPreviewTickets = 0;
                control.MyTicketPanelOrder = 0;
                control.ModulePanelOrder = 0;
                control.HideWaitingOnMeTab = true;
                control.HideSMSModules = true;
                control.HideMyTickets = true;
                control.HideMyTasks = true;
                control.HideMyProject = true;
                control.HideMyPendingApprovals = true;
                control.HideMyDepartment = true;
                control.HideMyClosedTickets = true;
                control.HideGovernanceModules = true;
                control.ShowServiceIcons = serviceDockPanelSetting.ShowIcons;
                control.ServiceCatalogViewMode = serviceDockPanelSetting.ServiceViewType;
                control.IconSize = serviceDockPanelSetting.IconSize;
                control.IsServiceDocPanel = true;
                control.ShowPanel = serviceDockPanelSetting.ShowPanel;
                control.PanelId = serviceDockPanelSetting.PanelId;
            }

            return control;
        }

    }
}