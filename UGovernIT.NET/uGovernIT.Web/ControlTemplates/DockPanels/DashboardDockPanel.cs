using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;


namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class DashboardDockPanel : DockPanel
    {
        public DashboardDockPanelSetting dashboardDockPanelSetting { get { return DockSetting as DashboardDockPanelSetting; } set { DockSetting = value; } }
        uGovernITDashboardChartsUserControl control;
        protected override void OnInit(EventArgs e)
        {

            this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){  setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";
            if (dashboardDockPanelSetting != null)
            {
                if (dashboardDockPanelSetting != null && !string.IsNullOrEmpty(dashboardDockPanelSetting.Title))
                    this.HeaderText = dashboardDockPanelSetting.Title;
                else
                {
                    if (!string.IsNullOrEmpty(dashboardDockPanelSetting.ViewName))
                    {
                        if (Request.QueryString["Control"] != null && Convert.ToString(Request.QueryString["Control"]).EqualsIgnoreCase("pageeditorview"))
                        {
                            this.HeaderText = $"{dashboardDockPanelSetting.ViewName} ({this.GetType().Name})";  //dashboardDockPanelSetting.ViewName;
                        }
                        else
                        {
                            this.HeaderText = dashboardDockPanelSetting.ViewName;
                        }                        
                    }
                    else
                        this.HeaderText = this.GetType().Name;  //"Dashboard";
                }
            }
           
            base.OnInit(e);
        }


        public override Control LoadControl(Page page)
        {
            control = (uGovernITDashboardChartsUserControl)page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/uGovernITDashboardChartsUserControl.ascx");
            control.ID = "dashboardPanelDock";
            if (dashboardDockPanelSetting != null && !string.IsNullOrEmpty(dashboardDockPanelSetting.ViewName))
                control.ViewName = dashboardDockPanelSetting.ViewName;
            return control;
        }

    }
}