using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{    
    public class HomeDockPanel:DockPanel
    {
        public HomeDockPanelSetting homeDockPanelSetting { get { return DockSetting as HomeDockPanelSetting; } set { DockSetting = value; } }
        uGovernITHomeUserControl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (homeDockPanelSetting != null)
            { 
                if (!string.IsNullOrEmpty(homeDockPanelSetting.Title))
                {
                    if (Request.QueryString["Control"] != null && Convert.ToString(Request.QueryString["Control"]).EqualsIgnoreCase("pageeditorview"))
                    {
                        this.HeaderText = $"{homeDockPanelSetting.Title} ({this.GetType().Name})"; //homeDockPanelSetting.Title;
                    }
                    else
                    {
                        this.HeaderText = homeDockPanelSetting.Title;
                    }
                }                    
                else
                    this.HeaderText = this.GetType().Name; //"Home User Panel";        
            }
           
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (uGovernITHomeUserControl)page.LoadControl("~/ControlTemplates/shared/uGovernITHomeUserControl.ascx");
            control.ID = "homeUser";
            if (homeDockPanelSetting != null)
            {
                control.WelcomeDesc = homeDockPanelSetting.WelcomeDesc;
                control.EnableNewButton = homeDockPanelSetting.EnableNewButton;
                control.WelcomeHeading = homeDockPanelSetting.WelcomeHeading;
                control.UpdateWaitingOnMeTab = homeDockPanelSetting.UpdateWaitingOnMeTab;
                control.UpdateMyTickets = homeDockPanelSetting.UpdateMyTickets;
                control.UpdateMyTasks = homeDockPanelSetting.UpdateMyTasks;
                control.UpdateMyProject = homeDockPanelSetting.UpdateMyProject;
                control.UpdateMyPendingApprovals = homeDockPanelSetting.UpdateMyPendingApprovals;
                control.UpdateMyDepartmentTickets = homeDockPanelSetting.UpdateMyDepartmentTickets;
                control.UpdateMyClosedTickets = homeDockPanelSetting.UpdateMyClosedTickets;
                control.ShowServiceCatalog = homeDockPanelSetting.ShowServiceCatalog;
                control.ServiceCatalogOrder = homeDockPanelSetting.ServiceCatalogOrder;
                control.NoOfPreviewTickets = homeDockPanelSetting.NoOfPreviewTickets;
                control.MyTicketPanelOrder = homeDockPanelSetting.MyTicketPanelOrder;
                control.ModulePanelOrder = homeDockPanelSetting.ModulePanelOrder;
                control.HideWaitingOnMeTab = homeDockPanelSetting.HideWaitingOnMeTab;
                control.HideSMSModules = homeDockPanelSetting.HideSMSModules;
                control.HideMyTickets = homeDockPanelSetting.HideMyTickets;
                control.HideMyTasks = homeDockPanelSetting.HideMyTasks;
                control.HideMyProject = homeDockPanelSetting.HideMyProject;
                control.HideMyPendingApprovals = homeDockPanelSetting.HideMyPendingApprovals;
                control.HideMyDepartment = homeDockPanelSetting.HideMyDepartmentTickets;
                control.HideMyClosedTickets = homeDockPanelSetting.HideMyClosedTickets;
                control.HideGovernanceModules = homeDockPanelSetting.HideGovernanceModules;
                control.ShowServiceIcons = homeDockPanelSetting.ShowIcons;
                control.ServiceCatalogViewMode = homeDockPanelSetting.ServiceViewType;
                control.IconSize = homeDockPanelSetting.IconSize;
                control.ShowCompactRows = homeDockPanelSetting.ShowCompactRows;
                control.ShowBandedRows = homeDockPanelSetting.ShowBandedRows;
                control.ViewName = homeDockPanelSetting.Name;
            }
            return control;
        }
    }
}