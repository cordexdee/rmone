using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility.DockPanels;
using System.Web.UI;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class DashboardReportDockPanel : DockPanel
    {
        public DashboardReportPanelSetting ticketDockPanelSetting { get { return DockSetting as DashboardReportPanelSetting; } set { DockSetting = value; } }
        DashboardReportUserControl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=null && s.mainElement.children[0].children[0].children[0]!=undefined && s.mainElement.children[0].children[0].children[0]!=''){s.mainElement.children[0].children[0].children[0].style.height='';}}}}}, 100);}";
            if (ticketDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(ticketDockPanelSetting.Title))
                    this.HeaderText = $"{ticketDockPanelSetting.Title} ({this.GetType().Name})"; //ticketDockPanelSetting.Title;
                else
                    this.HeaderText = this.GetType().Name;   //"DashboardReport";
            }
            if (NeedData)
            {
                control = (DashboardReportUserControl)Page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/DashboardReportUserControl.ascx");
                //customFilteredTickets.ID = "messageboard";
            }

            if (control != null)
                this.Controls.Add(control);
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (DashboardReportUserControl)page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/DashboardReportUserControl.ascx");
            control.ID = "dashboardReportUserControl";
            return control;
        }
    }
}