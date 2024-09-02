using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.SprintTasks)]
    public class SprintTasks:DBBaseEntity
    {
        public long ID { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }
        public string Body { get; set; }
        public int? ChildCount { get; set; }
        public string Comment { get; set; }
        public int? Contribution { get; set; }
        public int? Duration { get; set; }
        //public bool? IsDeleted { get; set; }
        public bool? IsMilestone { get; set; }
        public int? ItemOrder { get; set; }
        public int? Level { get; set; }
        public int? ParentTask { get; set; }
        public double? PercentComplete { get; set; }
        public long PMMIdLookup { get; set; }
        public string Priority { get; set; }
        public DateTime? ProposedDate { get; set; }
        public string ProposedStatus { get; set; }
        public long? ReleaseLookup { get; set; }
        public bool? ShowOnProjectCalendar { get; set; }
        public long? SprintLookup { get; set; }
        public int? SprintOrder { get; set; }
        public int? StageStep { get; set; }
        public DateTime? StartDate { get; set; }
        public double? TaskActualHours { get; set; }
        public string TaskBehaviour { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public double? TaskEstimatedHours { get; set; }
        public string TaskGroup { get; set; }
        public string TaskStatus { get; set; }
        public string Title { get; set; }
        //public string TenantID { get; set; }
        [NotMapped]
        public string PMMTitle { get; set; }
        [NotMapped]
        public string SprintTitle { get; set; }
    }

}
