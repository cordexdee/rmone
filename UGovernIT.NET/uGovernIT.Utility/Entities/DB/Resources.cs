using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Resources)]
    public class Resources
    {
        public long ID { get; set; }
        public string _ResourceType { get; set; }
        public DateTime? AllocationEndDate { get; set; }
        public DateTime? AllocationStartDate { get; set; }
        public string BudgetDescription { get; set; }
        public string BudgetType { get; set; }
        public int? EstimatedHours { get; set; }
        public string ModuleNameLookup { get; set; }
        public int? NoOfFTEs { get; set; }
        public string RequestedResources { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public long UserSkillLookup { get; set; }
        public string TenantID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string CreatedByUser { get; set; }
        public string ModifiedByUser { get; set; }
        public bool? Deleted { get; set; }
        public string Attachments { get; set; }
    }

}
