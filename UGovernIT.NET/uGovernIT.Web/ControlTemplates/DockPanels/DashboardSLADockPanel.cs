using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class DashboardSLADockPanel : DockPanel
    {
        public DashboardSLADockPanelSetting dashboardSLADockPanelSetting { get { return DockSetting as DashboardSLADockPanelSetting; } set { DockSetting = value; } }
        uGovernITDashboardSLAUserControl control;
        
        protected override void OnInit(EventArgs e)
        {
            this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){  setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";
            
            if (dashboardSLADockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(dashboardSLADockPanelSetting.Title))
                    this.HeaderText = dashboardSLADockPanelSetting.Title;
                else
                    this.HeaderText = "Dashboard SLA";
            }
            
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (uGovernITDashboardSLAUserControl)page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/uGovernITDashboardSLAUserControl.ascx");
            control.ID = "DashboardSLAControl";
           
            if (dashboardSLADockPanelSetting != null)
            {
                control.FilterView = dashboardSLADockPanelSetting.FilterView;
                control.IncludeOpen = dashboardSLADockPanelSetting.IncludeOpen;
                control.LegendSetting = dashboardSLADockPanelSetting.LegendSetting;
                control.Module = dashboardSLADockPanelSetting.Module;
                control.ShowSLAName = dashboardSLADockPanelSetting.ShowSLAName;
                control.SLAEnableModules = dashboardSLADockPanelSetting.SLAEnableModules;
                control.StringOfSelectedModule = dashboardSLADockPanelSetting.StringOfSelectedModule;
            }

            return control;
        }
    }
}