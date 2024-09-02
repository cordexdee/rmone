using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ResourceAllocationMonthly)]
    [JsonObject(MemberSerialization.OptOut)]
    public class ResourceAllocationMonthly : DBBaseEntity
    {
        public long ID { get; set; }
        public DateTime? MonthStartDate { get; set; }
        public double? PctAllocation { get; set; }
        public double? PctPlannedAllocation { get; set; }
        [Column(DatabaseObjects.Columns.Resource)]
        public string Resource { get; set; }
        public string ResourceSubWorkItem { get; set; }
        public string ResourceWorkItem { get; set; }
        public long ResourceWorkItemLookup { get; set; }
        public string ResourceWorkItemType { get; set; }
        public string Title { get; set; }
        public string GlobalRoleID { get; set; }

        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set;}
        public double? ActualPctAllocation { get; set; }

    }

}
