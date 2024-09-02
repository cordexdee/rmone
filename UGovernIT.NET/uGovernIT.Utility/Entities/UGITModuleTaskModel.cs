using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    public class UGITModuleTaskModel
    {
        public long ID { get; set; }
        public int ItemOrder { get; set; }
        public string Title { get; set; }
        public long ParentTaskID { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public string AssignToPct { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public double PercentComplete { get; set; }
        public int ChildCount { get; set; }
        public string Behaviour { get; set; } // Milestone, Deliverable, Receivable (default is Action)
        [NotMapped]
        public bool isCritical { get; set; }
        public double EstimatedHours { get; set; }
        public double ActualHours { get; set; }
        public double EstimatedRemainingHours { get; set; }
        public double Duration { get; set; }
        public double Contribution { get; set; }
        public string Predecessors { get; set; }
        public int StageStep { get; set; }
        public bool IsNextNewRowSet { get; set; }
        public string Description { get; set; }
        public string Sprint { get; set; }

    }
}
