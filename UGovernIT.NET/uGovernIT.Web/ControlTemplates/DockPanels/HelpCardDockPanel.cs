using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Web.ControlTemplates.DockPanels.Settings;
using uGovernIT.Web.ControlTemplates;
using System.Web.UI;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class HelpCardDockPanel : DockPanel
    {
        public HelpCardDockPanelSettings homeDockPanelSetting { get { return DockSetting as HelpCardDockPanelSettings; } set { DockSetting = value; } }
        HelpCardCtrl ctrl = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (homeDockPanelSetting != null)
            {

                if (!string.IsNullOrEmpty(homeDockPanelSetting.Title))
                    this.HeaderText = homeDockPanelSetting.Title;
                else
                    this.HeaderText = "Help Cards";
                //if (!string.IsNullOrEmpty(homeDockPanelSetting.Title))
                //    this.HeaderText = $"{homeDockPanelSetting.Title} ({this.GetType().Name})"; //homeDockPanelSetting.Title;
                //else
                //    this.HeaderText = this.GetType().Name; //"Wiki Articles";
            }
            //else
            //{
            //    this.HeaderText = this.GetType().Name;
            //}
            //if (NeedData)
            //{
            //    ctrl = (HelpCardCtrl)Page.LoadControl("~/ControlTemplates/HelpCard/HelpCardCtrl.ascx");
            //    ctrl.ID = "wikiArticles";
            //    if (homeDockPanelSetting != null)
            //    {

            //    }
            //}

            //if (ctrl != null)
            //    this.Controls.Add(ctrl);
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            ctrl = (HelpCardCtrl)page.LoadControl("~/ControlTemplates/HelpCard/HelpCardCtrl.ascx");
            ctrl.ID = "helpCard";
            return ctrl;
        }
    }
}