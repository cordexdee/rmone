using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.DockPanels.Settings;


namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class ITSMDockPanel : DockPanel
    {
        public ITSMDockPanelSetting homeDockPanelSetting { get { return DockSetting as ITSMDockPanelSetting; } set { DockSetting = value; } }
        ITSMCtrl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){   $('.dxpc-content').addClass('dxpcextra-content' );  if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (homeDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(homeDockPanelSetting.Title))
                    this.HeaderText = homeDockPanelSetting.Title;
                else
                    this.HeaderText = "ITSM";
            }
            
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (ITSMCtrl)page.LoadControl("~/ControlTemplates/uGovernIT/ITSMCtrl.ascx");
            control.ID = "ITSM";
          
            return control;
        }
    }
}
