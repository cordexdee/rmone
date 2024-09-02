using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ModuleTasks)]
    [JsonObject(MemberSerialization.OptOut)]
    public class UGITTask : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public double PercentComplete { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }
        public string Comment { get; set; }
        public int ItemOrder { get; set; }
        public long ParentTaskID { get; set; }
        public int Weight { get; set; }
        [NotMapped]
        public bool AutoCreateUser { get; set; }

        public string body { get; set; }
        [NotMapped]
        public bool AutoFillRequestor { get; set; }

        public string SubTaskType { get; set; } = string.Empty;
                                                            
        public string RequestTypeCategory { get; set; }
        [NotMapped]
        public UGITTask ParentTask { get; set; }
        public string Predecessors { get; set; }
        [NotMapped]
        public List<UGITTask> PredecessorTasks { get; set; }
        [NotMapped]
        public List<UGITTask> SuccessorTasks { get; set; }
        public string Priority { get; set; }
        public double Contribution { get; set; }
        public int ChildCount { get; set; }
        [NotMapped]
        public List<UGITTask> ChildTasks { get; set; }
        public int Level { get; set; }
        public double Duration { get; set; }
        public double ActualHours { get; set; }
        public double EstimatedHours { get; set; }
        [NotMapped]
        public List<HistoryEntry> CommentHistory { get; set; }
        [NotMapped]
        public long ProjectLookup { get; set; }
        
        [NotMapped]
        public bool Changes { get; set; }
        public string ModuleNameLookup { get; set; }
        [NotMapped]
        public string Module { get; set; }
        [NotMapped]
        public SPFieldLookupValue SPModule { get; set; }
        public DateTime ProposedDate { get; set; } = new DateTime(1800, 1, 1);
        [NotMapped]
        public UGITTaskProposalStatus ProposedStatus { get; set; }
        public bool ShowOnProjectCalendar { get; set; }
        public bool IsCritical { get; set; }
        public string LinkedDocuments { get; set; }
        public string TicketId { get; set; }
        public long SprintLookup { get; set; }
        [NotMapped]
        public string PickUserFrom { get; set; }
        //new property for add skill in task
        public string UserSkillMultiLookup { get; set; }
        
        public DateTime CompletionDate { get; set; }
        public string CompletedBy { get; set; }


        /// <summary>
        /// Property work for root task only
        /// </summary>
        public bool IsMileStone { get; set; } // Indicates (for top-level tasks only) if linked to stage
        public int StageStep { get; set; }
        public string Behaviour { get; set; } // Milestone, Deliverable, Receivable (default is Action)
        public string AssignToPct { get; set; }
        public double EstimatedRemainingHours { get; set; }
        public int ReminderDays { get; set; }
        public bool ReminderEnabled { get; set; }
        public int RepeatInterval { get; set; }
        [NotMapped]
        public string Cal_TaskIcon
        {
            get
            {
                if (!string.IsNullOrEmpty(Behaviour))
                {
                    switch (Behaviour)
                    {
                        case Constants.TaskType.Milestone:
                            return "/Content/images/uGovernIT/milestone_icon.png";
                        case Constants.TaskType.Deliverable:
                            return "/Content/images/uGovernIT/document_down.png";
                        case Constants.TaskType.Receivable:
                            return "/Content/images/uGovernIT/document_up.png";
                    }
                }

                return "/Content/images/ittask.png";
            }
        }
        [NotMapped]
        public Dictionary<string, string> AttachedFiles { get; set; }
        [NotMapped]
        public Dictionary<string, byte[]> AttachFiles { get; set; }
        
        [NotMapped]
        public string Author { get; set; }
        public string RelatedModule { get; set; }
        public string RelatedTicketID { get; set; }
        [NotMapped]
        public string ChildInstance { get; set; }
        [NotMapped]
        public string ParentInstance { get; set; }       
        
        public string NewUserName { get; set; }
        [NotMapped]
        public string UGITNewUserDisplayName { get; set; }
        [NotMapped]
        public string ErrorMsg { get; set; }
        [NotMapped]
        public bool IsStartDateChange { get; set; }
        [NotMapped]
        public bool IsEndDateChange { get; set; }
        [NotMapped]
        public DateTime? EndDate { get; set; }
        public bool EnableApproval { get; set; }  
        [Column(DatabaseObjects.Columns.Approver)]
        public string Approver { get; set; }
        public string ApprovalStatus { get; set; }
        [NotMapped]
        public string ApprovalType { get; set; }
        
        public string ServiceApplicationAccessXml { get; set; }
        [NotMapped]
        public List<UGITTask> PredecessorsObj { get; set; }
        public double TotalHoldDuration { get; set; }
        
        public DateTime? OnHoldStartDate { get; set; }
        public bool OnHold { get; set; }
        
        public DateTime? OnHoldTillDate { get; set; }
        public string OnHoldReasonChoice { get; set; }
        
        public DateTime? TaskActualStartDate { get; set; }
        public bool SLADisabled { get; set; }
        public bool NotificationDisabled { get; set; }        

        public string IssueImpact { get; set; }
        public string Resolution { get; set; }
        public DateTime ResolutionDate { get; set; } = new DateTime(1800, 1, 1);
        public string ContingencyPlan { get; set; }
        public string MitigationPlan { get; set; }
        public int RiskProbability { get; set; }
        //public bool IsDeleted { get; set; }
        [NotMapped]
        public string PredecessorsByOrder { get; set; }
        public string QuestionID { get; set; }
        public string QuestionProperties { get; set; }
        [NotMapped]
        public string UserFieldXML { get; set; }
        [NotMapped]
        public string TaskActionUser { get; set; }
        [NotMapped]
        [Column(DatabaseObjects.Columns.ApprovedBy)]
        public string ApprovedBy { get; set; }
        [NotMapped]
        public string Service { get; set; }
        [NotMapped]
        public int TaskLevel { get; set; }
        [NotMapped]
        public string UGITSubTaskType { get; set; }
        [NotMapped]
        public bool UseADAuthentication { get; set; }
        [NotMapped]
        public string RequestCategory { get; set; }
        [NotMapped]
        public string Approvalstatus { get; set; }
        [NotMapped]
        public List<SPFieldLookupValue> SPPredecessor { get; set; }
        public UGITTask(string moduleName)
            : base()
        {
            ModuleNameLookup = moduleName;
        }

        public UGITTask()
        {
            ModuleNameLookup = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            Status = Constants.NotStarted;
            Comment = string.Empty;
            ProposedStatus = UGITTaskProposalStatus.Not_Proposed;
            AttachFiles = new Dictionary<string, byte[]>();
            AttachedFiles = new Dictionary<string, string>();

        }
    }
}
