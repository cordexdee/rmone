using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.PMMIssues)]
    public class PMMIssues:DBBaseEntity
    {
        public long ID { get; set; }
        public string AssignedToUser { get; set; }
        public string Body { get; set; }
        public int? ChildCount { get; set; }
        public string Comment { get; set; }
        public double? Contribution { get; set; }
        public DateTime? DueDate { get; set; }
        public double? Duration { get; set; }
        //public bool? IsDeleted { get; set; }
        public string IssueImpact { get; set; }
        public int? ItemOrder { get; set; }
        public int? Level { get; set; }
        public int? ParentTask { get; set; }
        public double? PercentComplete { get; set; }
        public long PMMIdLookup { get; set; }
        public string Predecessors { get; set; }
        public string Priority { get; set; }
        public DateTime? ProposedDate { get; set; }
        public string ProposedStatus { get; set; }
        public string Resolution { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string Status { get; set; }
        public double? TaskActualHours { get; set; }
        public double? TaskEstimatedHours { get; set; }
        public string TaskGroup { get; set; }
        public string Title { get; set; }
        //public string TenantID { get; set; }
    }

}
