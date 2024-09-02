using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class HomeCardDockPanel : DockPanel
    {
        public HomeCardPanelSetting homeCardPanelSetting { get { return DockSetting as HomeCardPanelSetting; } set { DockSetting = value; } }
        
        HomeCardView control;
        protected override void OnInit(EventArgs e)
        {

            this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){  setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";
            if (homeCardPanelSetting != null)
            {
                if (homeCardPanelSetting != null && !string.IsNullOrEmpty(homeCardPanelSetting.Title))
                    this.HeaderText = homeCardPanelSetting.Title;
                else
                    this.HeaderText = "Task list";
                if (!string.IsNullOrEmpty(homeCardPanelSetting.ContentTitle))
                    this.HeaderText = homeCardPanelSetting.ContentTitle;
            }
            
            
               

            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (HomeCardView)page.LoadControl("~/ControlTemplates/uGovernIT/HomeCardView.ascx");
            control.ID = "dashboardSLADockPanel";

            return control;
        }
    }
}