using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Web.ControlTemplates.Shared;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class ReportCardDockPanel : DockPanel
    {

        public ReportDockPanelsetting taskDockPanelSetting { get { return DockSetting as ReportDockPanelsetting; } set { DockSetting = value; } }
        ReportCardView control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (taskDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(taskDockPanelSetting.Title))
                    this.HeaderText = taskDockPanelSetting.Title;
                else
                    this.HeaderText = "Report Cards";
            }

            base.OnInit(e);

        }

        public override Control LoadControl(Page page)
        {
            control = (ReportCardView)page.LoadControl("~/ControlTemplates/Shared/ReportCardView.ascx");
            control.ID = "ReportCards";
            return control;
        }
    }
}