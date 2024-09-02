using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise)]
    public class ResourceUsageSummaryMonthWise: DBBaseEntity
    {
        public long ID { get; set; }
        public double? ActualHour { get; set; }
        public double? AllocationHour { get; set; }
        public string FunctionalAreaTitleLookup { get; set; }
        public bool? IsConsultant { get; set; }
        public bool? IsIT { get; set; }
        public bool? IsManager { get; set; }
        public string ManagerLookup { get; set; }
        public string ManagerName { get; set; }
        public DateTime? MonthStartDate { get; set; }
        public double? PctActual { get; set; }
        public double? PctAllocation { get; set; }
        public double? PctPlannedAllocation { get; set; }
        public double? PlannedAllocationHour { get; set; }
        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }
        [Column(DatabaseObjects.Columns.ResourceName)]
        public string ResourceName { get; set; }
        public string SubWorkItem { get; set; }
        public string WorkItem { get; set; }
        public long? WorkItemID { get; set; }
        public string WorkItemType { get; set; }
        public string Title { get; set; }
        public string GlobalRoleID { get; set; }
        public bool SoftAllocation { get; set; }
        public string FunctionalArea { get; set; }
        public string ERPJobID { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public double? ActualPctAllocation { get; set; }

    }
}
