using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility.DockPanels;
using System.Web.UI;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class ApplicationUpTimeDashBoardDockPanel:DockPanel
    {
        //public DashboardDockPanelSetting dashboardDockPanelSetting { get { return DockSetting as DashboardDockPanelSetting; } set { DockSetting = value; } }
        ApplicationUpTimeDashBoardUserControl control;
        public Applicationuptimesetting moduleDockPanelSetting { get { return DockSetting as Applicationuptimesetting; } set { DockSetting = value; } }
        
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s, e) {  $('.dxpc-content').addClass('dxpcextra-content' );  $('.dxdpLite').addClass('dxdpLiteextra' ); setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (moduleDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(moduleDockPanelSetting.Title))
                    this.HeaderText = moduleDockPanelSetting.Title;
                else
                    this.HeaderText = "ApplicationUpTimeDashBoard";
                if (!string.IsNullOrEmpty(moduleDockPanelSetting.ModuleName))
                    this.HeaderText += " - " + moduleDockPanelSetting.ModuleName;
            }


            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/ApplicationUpTimeDashBoardUserControl.ascx") as ApplicationUpTimeDashBoardUserControl;
            control.ID= "ApplicationUpTimeDashBoardUserControl";
             
            return control;
        }
    }
}