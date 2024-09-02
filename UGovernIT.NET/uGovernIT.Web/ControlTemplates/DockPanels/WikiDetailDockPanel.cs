using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.DockPanels.Settings;
using uGovernIT.Web.ControlTemplates.Wiki;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class WikiDetailDockPanel : DockPanel
    {
        public WikiDetailDockPanelSetting docPanelSetting { get { return DockSetting as WikiDetailDockPanelSetting; } set { DockSetting = value; } }
        WikiDetailCtrl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (docPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(docPanelSetting.Title))
                    this.HeaderText = docPanelSetting.Title;
                else
                    this.HeaderText = "Wiki Detail";
            }
           
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (WikiDetailCtrl)page.LoadControl("~/ControlTemplates/Wiki/WikiDetailCtrl.ascx");
            control.ID = "wikiDetail";
            return control;
        }
    }
}
