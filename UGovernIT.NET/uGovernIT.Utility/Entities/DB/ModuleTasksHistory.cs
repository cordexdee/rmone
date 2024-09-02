using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.ModuleTasksHistory)]
    public class ModuleTasksHistory : DBBaseEntity
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
        [NotMapped]
        public bool AutoFillRequestor { get; set; }
        public string Body { get; set; }

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
        public DateTime ProposedDate { get; set; } = new DateTime(1800, 1, 1);
        [NotMapped]
        public UGITTaskProposalStatus ProposedStatus { get; set; }
        public bool ShowOnProjectCalendar { get; set; }
        public bool IsCritical { get; set; }
        public string LinkedDocuments { get; set; }
        public string TicketId { get; set; }
        public long SprintLookup { get; set; }

        //new property for add skill in task
        public string UserSkillMultiLookup { get; set; }
        public DateTime CompletionDate { get; set; } = System.DateTime.Now;
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
        //[NotMapped]
        //public DateTime Created { get; set; } = System.DateTime.Now;
        //[NotMapped]
        //public string ModifiedByUser { get; set; }
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
        public string ApproverUser { get; set; }
        public string ApprovalStatus { get; set; }
        [NotMapped]
        public string ApprovalType { get; set; }
        [NotMapped]
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

        public string IssueImpact { get; set; }
        public string Resolution { get; set; }
        public DateTime ResolutionDate { get; set; } = new DateTime(1800, 1, 1);
        public string ContingencyPlan { get; set; }
        public string MitigationPlan { get; set; }
        public int RiskProbability { get; set; }
        //public bool IsDeleted { get; set; }
        public int BaselineId { get; set; }
        public DateTime BaselineDate { get; set; }

        public ModuleTasksHistory(string moduleName)
            : base()
        {
            ModuleNameLookup = moduleName;
        }

        public ModuleTasksHistory()
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

