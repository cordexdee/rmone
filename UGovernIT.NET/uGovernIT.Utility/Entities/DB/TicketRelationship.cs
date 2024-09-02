using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class TicketRelationship:DBBaseEntity
    {
        public long ID { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalType { get; set; }
        public string ApprovedBy { get; set; }
        public string Approver { get; set; }
        public string AssignedTo { get; set; }
        public bool AutoCreateUser { get; set; }
        public string Body { get; set; } //nvarchar(MAX)
        public string ChildId { get; set; }
        public string Comment { get; set; }
        public string CompletedBy { get; set; }
        public string CompletionDate { get; set; }  //datetime
        public string DueDate { get; set; }// datetime
        public bool EnableApproval { get; set; }
        public string ErrorMsg { get; set; }
        //public bool IsDeleted { get; set; }
        public int ItemOrder { get; set; }
        public int Level { get; set; } //int
        public string ModuleNameLookup { get; set; }
        public string NewUserName { get; set; }
        public string ParentId { get; set; }
        public int ParentTask { get; set; }
        public int PercentComplete { get; set; }
        public string Predecessors { get; set; }
        public string Priority { get; set; }
        public string ServiceApplicationAccessXml { get; set; }
        public int StageWeight { get; set; }
        public string StartDate { get; set; }//  datetime
        public string Status { get; set; }
        public string SubTaskType { get; set; }
        public string TaskActionUser { get; set; }
        public double TaskActualHours { get; set; } //int
        public double TaskEstimatedHours { get; set; }//  int
        public string TaskGroup { get; set; }
        public string TaskType { get; set; }




    }
}
