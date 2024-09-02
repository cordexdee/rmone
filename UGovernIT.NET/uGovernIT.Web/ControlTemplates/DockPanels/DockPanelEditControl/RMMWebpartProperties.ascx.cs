using System;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Web.ControlTemplates.DockPanels;

namespace uGovernIT.Web
{
    public partial class RMMWebpartProperties : UserControl
    {       
        public RMMDockPanelSetting rmmDockPanelSetting { get; set; }

        protected override void OnInit(EventArgs e)
        {
            chkTitle.Checked = rmmDockPanelSetting.ShowTitle;
            txtTitle.Text = rmmDockPanelSetting.Title;

            chkAllocationTab.Checked = !rmmDockPanelSetting.HideAllocationTab;
            chkActualTab.Checked = !rmmDockPanelSetting.HideActualTab;
            chkResourceTab.Checked = !rmmDockPanelSetting.HideResourcesTab;
            chkResourcePlanningTab.Checked = !rmmDockPanelSetting.HideResourcePlanningTab;
            chkResourceAvailabilityTab.Checked = !rmmDockPanelSetting.HideResourceAvailabilityTab;
            chkAllocationTimelineTab.Checked = !rmmDockPanelSetting.HideAllocationTimelineTab;
            chkProjectComplexityTab.Checked = !rmmDockPanelSetting.HideProjectComplexityTab;
            chkCapacityReportTab.Checked = !rmmDockPanelSetting.HideCapacityReportTab;
            chkBillingAndMarginTab.Checked = !rmmDockPanelSetting.HideBillingAndMarginTab;
            chkExecutiveKPITab.Checked = !rmmDockPanelSetting.HideExecutiveKPITab;
            chkResourceUtilizationIndexTab.Checked = !rmmDockPanelSetting.HideResourceUtilizationIndexTab;
            chkManageAllocationTemplatesTab.Checked = !rmmDockPanelSetting.HideManageAllocationTemplatesTab;
            chkBenchab.Checked = !rmmDockPanelSetting.HideBenchTab;

            //chkFinancialViewTab.Checked = !rmmDockPanelSetting.HideFinancialViewTab;


            ddlAllocationTabOrder.SelectedIndex = ddlAllocationTabOrder.Items.IndexOfValue(rmmDockPanelSetting.ResourceAllocationOrder.ToString());
            ddlActualTabOrder.SelectedIndex = ddlActualTabOrder.Items.IndexOfValue(rmmDockPanelSetting.ActualOrder.ToString());
            ddlResourceTabOrder.SelectedIndex = ddlResourceTabOrder.Items.IndexOfValue(rmmDockPanelSetting.ResourceOrder.ToString());
            ddlResourcePlanningTabOrder.SelectedIndex = ddlResourcePlanningTabOrder.Items.IndexOfValue(rmmDockPanelSetting.ResourcePlaningOrder.ToString());
            ddlResourceAvailabilityTabOrder.SelectedIndex = ddlResourceAvailabilityTabOrder.Items.IndexOfValue(rmmDockPanelSetting.ResourceAvailabilityOrder.ToString());
            ddlAllocationTimeline.SelectedIndex = ddlAllocationTimeline.Items.IndexOfValue(rmmDockPanelSetting.AllocationTimelineOrder.ToString());
            ddlProjectComplexity.SelectedIndex = ddlProjectComplexity.Items.IndexOfValue(rmmDockPanelSetting.ProjectComplexityOrder.ToString());
            ddlCapacityReport.SelectedIndex = ddlCapacityReport.Items.IndexOfValue(rmmDockPanelSetting.CapacityReportOrder.ToString());
            ddlBillingAndMargins.SelectedIndex = ddlBillingAndMargins.Items.IndexOfValue(rmmDockPanelSetting.BillingAndMarginOrder.ToString());
            ddlExecutiveKPI.SelectedIndex = ddlExecutiveKPI.Items.IndexOfValue(rmmDockPanelSetting.ExecutiveKPIOrder.ToString());
            ddlResourceUtilizationIndex.SelectedIndex = ddlResourceUtilizationIndex.Items.IndexOfValue(rmmDockPanelSetting.ResourceUtilizationIndexOrder.ToString());
            ddlManageAllocationTemplatesTabOrder.SelectedIndex = ddlManageAllocationTemplatesTabOrder.Items.IndexOfValue(rmmDockPanelSetting.ManageAllocationTemplatesOrder.ToString());

            ddlBenchTabOrder.SelectedIndex = ddlBenchTabOrder.Items.IndexOfValue(rmmDockPanelSetting.ManageBenchTabOrder.ToString());
            //ddlFinancialView.SelectedIndex = ddlFinancialView.Items.IndexOfValue(rmmDockPanelSetting.FinancialViewOrder.ToString());

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rmmDockPanelSetting.ShowTitle = chkTitle.Checked;
            rmmDockPanelSetting.Title = txtTitle.Text.Trim();

            rmmDockPanelSetting.HideAllocationTab = !chkAllocationTab.Checked;
            rmmDockPanelSetting.HideActualTab = !chkActualTab.Checked;
            rmmDockPanelSetting.HideResourcesTab = !chkResourceTab.Checked;
            rmmDockPanelSetting.HideResourcePlanningTab = !chkResourcePlanningTab.Checked;
            rmmDockPanelSetting.HideResourceAvailabilityTab = !chkResourceAvailabilityTab.Checked;
            rmmDockPanelSetting.HideAllocationTimelineTab = !chkAllocationTimelineTab.Checked;
            rmmDockPanelSetting.HideProjectComplexityTab = !chkProjectComplexityTab.Checked;
            rmmDockPanelSetting.HideCapacityReportTab = !chkCapacityReportTab.Checked;
            rmmDockPanelSetting.HideBillingAndMarginTab = !chkCapacityReportTab.Checked;
            rmmDockPanelSetting.HideExecutiveKPITab = !chkExecutiveKPITab.Checked;
            rmmDockPanelSetting.HideResourceUtilizationIndexTab = !chkResourceUtilizationIndexTab.Checked;
            //rmmDockPanelSetting.HideFinancialViewTab = !chkFinancialViewTab.Checked;
            rmmDockPanelSetting.HideManageAllocationTemplatesTab = !chkManageAllocationTemplatesTab.Checked;
            rmmDockPanelSetting.HideBenchTab = !chkBenchab.Checked;

            rmmDockPanelSetting.ResourceAllocationOrder = UGITUtility.StringToInt(ddlAllocationTabOrder.Value);
            rmmDockPanelSetting.ActualOrder = UGITUtility.StringToInt(ddlActualTabOrder.Value);
            rmmDockPanelSetting.ResourceOrder = UGITUtility.StringToInt(ddlResourceTabOrder.Value);
            rmmDockPanelSetting.ResourcePlaningOrder = UGITUtility.StringToInt(ddlResourcePlanningTabOrder.Value);
            rmmDockPanelSetting.ResourceAvailabilityOrder = UGITUtility.StringToInt(ddlResourceAvailabilityTabOrder.Value);
            rmmDockPanelSetting.AllocationTimelineOrder = UGITUtility.StringToInt(ddlAllocationTimeline.Value);
            rmmDockPanelSetting.ProjectComplexityOrder = UGITUtility.StringToInt(ddlProjectComplexity.Value);
            rmmDockPanelSetting.CapacityReportOrder = UGITUtility.StringToInt(ddlCapacityReport.Value);
            rmmDockPanelSetting.BillingAndMarginOrder = UGITUtility.StringToInt(ddlBillingAndMargins.Value);
            rmmDockPanelSetting.ExecutiveKPIOrder = UGITUtility.StringToInt(ddlExecutiveKPI.Value);
            rmmDockPanelSetting.ResourceUtilizationIndexOrder = UGITUtility.StringToInt(ddlResourceUtilizationIndex.Value);
            rmmDockPanelSetting.ManageAllocationTemplatesOrder = UGITUtility.StringToInt(ddlManageAllocationTemplatesTabOrder.Value);
            rmmDockPanelSetting.ManageBenchTabOrder = UGITUtility.StringToInt(ddlBenchTabOrder.Value);
        }

        public void CopyFromWebpart( RMMDockPanelSetting webpart)
        {
            webpart = rmmDockPanelSetting;
        }

        public void CopyFromControl(RMMDockPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text.Trim();

            webpart.HideAllocationTab = !chkAllocationTab.Checked;
            webpart.HideActualTab = !chkActualTab.Checked;
            webpart.HideResourcesTab = !chkResourceTab.Checked;
            webpart.HideResourcePlanningTab = !chkResourcePlanningTab.Checked;
            webpart.HideResourceAvailabilityTab = !chkResourceAvailabilityTab.Checked;
            webpart.HideAllocationTimelineTab = !chkAllocationTimelineTab.Checked;
            webpart.HideProjectComplexityTab = !chkProjectComplexityTab.Checked;
            webpart.HideCapacityReportTab = !chkCapacityReportTab.Checked;
            webpart.HideBillingAndMarginTab = !chkBillingAndMarginTab.Checked;
            webpart.HideExecutiveKPITab = !chkExecutiveKPITab.Checked;
            webpart.HideResourceUtilizationIndexTab = !chkResourceUtilizationIndexTab.Checked;
            //webpart.HideFinancialViewTab = !chkFinancialViewTab.Checked;
            webpart.HideManageAllocationTemplatesTab = !chkManageAllocationTemplatesTab.Checked;
            webpart.HideBenchTab = !chkBenchab.Checked;
            webpart.ResourceAllocationOrder = UGITUtility.StringToInt(ddlAllocationTabOrder.Value);
            webpart.ActualOrder = UGITUtility.StringToInt(ddlActualTabOrder.Value);
            webpart.ResourceOrder = UGITUtility.StringToInt(ddlResourceTabOrder.Value);
            webpart.ResourcePlaningOrder = UGITUtility.StringToInt(ddlResourcePlanningTabOrder.Value);
            webpart.ResourceAvailabilityOrder = UGITUtility.StringToInt(ddlResourceAvailabilityTabOrder.Value);
            webpart.AllocationTimelineOrder = UGITUtility.StringToInt(ddlAllocationTimeline.Value);
            webpart.ProjectComplexityOrder = UGITUtility.StringToInt(ddlProjectComplexity.Value);
            webpart.CapacityReportOrder = UGITUtility.StringToInt(ddlCapacityReport.Value);
            webpart.BillingAndMarginOrder = UGITUtility.StringToInt(ddlBillingAndMargins.Value);
            webpart.ExecutiveKPIOrder = UGITUtility.StringToInt(ddlExecutiveKPI.Value);
            webpart.ResourceUtilizationIndexOrder = UGITUtility.StringToInt(ddlResourceUtilizationIndex.Value);
            //webpart.FinancialViewOrder = UGITUtility.StringToInt(ddlFinancialView.Value);
            webpart.ManageAllocationTemplatesOrder = UGITUtility.StringToInt(ddlManageAllocationTemplatesTabOrder.Value);
            webpart.ManageBenchTabOrder = UGITUtility.StringToInt(ddlBenchTabOrder.Value);
            rmmDockPanelSetting = webpart;
        }
    }
}
