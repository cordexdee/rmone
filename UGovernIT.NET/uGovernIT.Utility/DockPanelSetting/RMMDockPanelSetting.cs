using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Utility.DockPanels
{
    public class RMMDockPanelSetting : DockPanelSetting
    {
        public string ModuleName { get; set; }

        public bool HideAllocationTab { get; set; }
        public bool HideActualTab { get; set; }
        public bool HideResourcesTab { get; set; }
        public bool HideResourcePlanningTab { get; set; }
        public bool HideResourceAvailabilityTab { get; set; }
        public bool HideAllocationTimelineTab { get; set; }
        public bool HideProjectComplexityTab { get; set; }
        public bool HideCapacityReportTab { get; set; }
        public bool HideBillingAndMarginTab { get; set; }
        public bool HideExecutiveKPITab { get; set; }
        public bool HideResourceUtilizationIndexTab { get; set; }
        public bool HideFinancialViewTab { get; set; }
        public bool HideManageAllocationTemplatesTab { get; set; }
        public bool HideBenchTab { get; set; }

        public int ResourceAllocationOrder { get; set; }
        public int ActualOrder { get; set; }
        public int ResourceOrder { get; set; }
        public int ResourcePlaningOrder { get; set; }
        public int ResourceAvailabilityOrder { get; set; }
        public int AllocationTimelineOrder { get; set; }
        public int ProjectComplexityOrder { get; set; }
        public int CapacityReportOrder { get; set; }
        public int BillingAndMarginOrder { get; set; }
        public int ExecutiveKPIOrder { get; set; }
        public int ResourceUtilizationIndexOrder { get; set; }
        public int FinancialViewOrder { get; set; }
        public int ManageAllocationTemplatesOrder { get; set; }
        public int ManageBenchTabOrder { get; set; }

    }
}