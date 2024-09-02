using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class ModuleWebPartDockPanel:DockPanel
    {
        public ModuleWebpartDockPanelSetting moduleDockPanelSetting { get { return DockSetting as ModuleWebpartDockPanelSetting; } set { DockSetting = value; } }
        uGovernITModuleWebpartUserControl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s, e) {  $('.dxpc-content').addClass('dxpcextra-content' );  $('.dxdpLite').addClass('dxdpLiteextra' ); setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000);}";
            if (moduleDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(moduleDockPanelSetting.Title))
                    this.HeaderText = moduleDockPanelSetting.Title;
                else
                    this.HeaderText = "ModuleWebpart";
                if (!string.IsNullOrEmpty(moduleDockPanelSetting.ModuleName))
                    this.HeaderText += " - " + moduleDockPanelSetting.ModuleName;
            }
        
            
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (uGovernITModuleWebpartUserControl)page.LoadControl("~/ControlTemplates/shared/uGovernITModuleWebpartUserControl.ascx");
            control.ID = "moduledock";
            control.currentModuleName = moduleDockPanelSetting.ModuleName;

            return control;
        }
    }
}