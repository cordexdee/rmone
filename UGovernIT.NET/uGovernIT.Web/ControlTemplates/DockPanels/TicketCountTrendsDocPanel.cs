using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class TicketCountTrendsDocPanel: DockPanel
    {
        public TicketCountTrendsDocPanelSetting docPanelSetting { get { return DockSetting as TicketCountTrendsDocPanelSetting; } set { DockSetting = value; } }
        public DailyTicketCountTrendsControl control;

        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (docPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(docPanelSetting.Title))
                    this.HeaderText = docPanelSetting.Title;
                else
                    this.HeaderText = "Ticket Count Trends";
            }

            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/DailyTicketCountTrendsControl.ascx") as DailyTicketCountTrendsControl;
            control.ID = "DailyTicketCountTrends";
            if (docPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(docPanelSetting.Title))
                    control.ContentTitle = docPanelSetting.Title;

                if (!string.IsNullOrEmpty(docPanelSetting.DefaultModule))
                    control.DefaultModule = docPanelSetting.DefaultModule;

                if (!string.IsNullOrEmpty(docPanelSetting.StringOfSelectedModule))
                    control.StringOfSelectedModule = docPanelSetting.StringOfSelectedModule;
            }

            return control;
        }
    }
}