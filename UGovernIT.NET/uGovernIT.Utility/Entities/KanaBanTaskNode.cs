using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities
{
    public class KanaBanTaskNode
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
        public bool isCritical { get; set; }
        public double EstimatedHours { get; set; }
        public double ActualHours { get; set; }
        public double EstimatedRemainingHours { get; set; }
        public double Duration { get; set; }
        public double Contribution { get; set; }
        public string Predecessors { get; set; }
        public long ParentTaskIDummy { get; set; }
        public int Level { get; set; }
        public bool HasItem { get; set; }


    }

    public class KanBanCategory
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

    }




}
