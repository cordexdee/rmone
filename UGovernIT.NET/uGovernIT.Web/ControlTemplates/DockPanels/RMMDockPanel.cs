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
    
    public class RMMDockPanel:DockPanel
    {
        public RMMDockPanelSetting rmmDockPanelSetting { get { return DockSetting as RMMDockPanelSetting; } set { DockSetting = value; } }
        uGovernITRMMUserControl control = null;
        protected override void OnInit(EventArgs e)
        {
            //this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";
            if (rmmDockPanelSetting != null)
            { 
                if (!string.IsNullOrEmpty(rmmDockPanelSetting.Title))
                    this.HeaderText = $"{rmmDockPanelSetting.Title} ({this.GetType().Name})"; //rmmDockPanelSetting.Title;
                else
                    this.HeaderText = this.GetType().Name; //"RMM";

                if (!string.IsNullOrEmpty(rmmDockPanelSetting.ModuleName))
                this.HeaderText += " - " + rmmDockPanelSetting.ModuleName;
            }
            
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (uGovernITRMMUserControl)page.LoadControl("~/ControlTemplates/RMM/uGovernITRMMUserControl.ascx");

            control.ID = "rmmFilter";
            control.HideActualTab = rmmDockPanelSetting.HideActualTab;
            control.HideAllocationTab = rmmDockPanelSetting.HideAllocationTab;
            control.HideAllocationTimelineTab = rmmDockPanelSetting.HideAllocationTimelineTab;
            control.HideResourcePlanningTab = rmmDockPanelSetting.HideResourcePlanningTab;
            control.HideResourceAvailabilityTab = rmmDockPanelSetting.HideResourceAvailabilityTab;
            control.HideResourceTab = rmmDockPanelSetting.HideResourcesTab;
            control.HideProjectComplexityTab = rmmDockPanelSetting.HideProjectComplexityTab;
            control.HideCapacityReportTab = rmmDockPanelSetting.HideCapacityReportTab;
            control.HideBillingAndMarginsTab = rmmDockPanelSetting.HideBillingAndMarginTab;
            control.HideExecutiveKPITab = rmmDockPanelSetting.HideExecutiveKPITab;
            control.HideResourceUtilizationIndexTab = rmmDockPanelSetting.HideResourceUtilizationIndexTab;
            control.HideManageAllocationTemplatesTab = rmmDockPanelSetting.HideManageAllocationTemplatesTab;
            control.HideBenchTab = rmmDockPanelSetting.HideBenchTab;

            control.ResourceAllocationOrder = rmmDockPanelSetting.ResourceAllocationOrder;
            control.ActualOrder = rmmDockPanelSetting.ActualOrder;
            control.ResourceOrder = rmmDockPanelSetting.ResourceOrder;
            control.ResourcePlaningOrder = rmmDockPanelSetting.ResourcePlaningOrder;
            control.ResourceAvailabilityOrder = rmmDockPanelSetting.ResourceAvailabilityOrder;
            control.AllocationTimelineOrder = rmmDockPanelSetting.AllocationTimelineOrder;
            control.ProjectComplexityOrder = rmmDockPanelSetting.ProjectComplexityOrder;
            control.CapacityReportOrder = rmmDockPanelSetting.CapacityReportOrder;
            control.BillingAndMarginOrder = rmmDockPanelSetting.BillingAndMarginOrder;
            control.ExecutiveKPIOrder = rmmDockPanelSetting.ExecutiveKPIOrder;
            control.ResourceUtilizationIndexOrder = rmmDockPanelSetting.ResourceUtilizationIndexOrder;
            control.ManageAllocationTemplatesTabOrder= rmmDockPanelSetting.ManageAllocationTemplatesOrder;
            control.BenchTabOrder = rmmDockPanelSetting.ManageBenchTabOrder;
            control.ID = "rmmFilter";

            return control;
        }
    }
}