using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.DockPanels.Settings;
using uGovernIT.Web.ControlTemplates.Wiki;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class WikiArticlesDockPanel : DockPanel
    {
        public WikiArticlesDockPanelSetting homeDockPanelSetting { get { return DockSetting as WikiArticlesDockPanelSetting; } set { DockSetting = value; } }
        WikiArticlesCtrl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (homeDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(homeDockPanelSetting.Title))
                    this.HeaderText = homeDockPanelSetting.Title;
                else
                    this.HeaderText = "Wiki Articles";
            }
          
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (WikiArticlesCtrl)page.LoadControl("~/ControlTemplates/Wiki/WikiArticlesCtrl.ascx");
            control.ID = "wikiArticles";
            return control;
        }
    }
}
