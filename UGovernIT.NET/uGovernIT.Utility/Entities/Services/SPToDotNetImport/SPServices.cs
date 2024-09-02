using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility;

namespace uGovernIT.Utility
{
    public class SPServices
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public List<SPFieldUserValue> Owners { get; set; }
        public int ID { get; set; }
        public bool CreateParentServiceRequest { get; set; }
        public bool OwnerApprovalRequired { get; set; }
        public bool AllowServiceTasksInBackground { get; set; }
        public List<SPServiceTaskCondition> SkipTaskCondition;
        public List<SPServiceSectionCondition> SkipSectionCondition;
        public List<SPServiceQuestion> Questions { get; set; }
        public List<SPServiceSection> Sections { get; set; }
        public List<ServiceTask> Tasks { get; set; }
        public List<SPServiceQuestionMapping> QuestionsMapping { get; set; }
        public int ItemOrder { get; set; }
        public bool Activated { get; set; }
        public Dictionary<string, string> CustomProperties { get; set; }
        public List<SPFieldUserValue> AuthorizedToView { get; set; }
        public string NavigationUrl { get; set; }
        public bool IsDeleted { get; set; }
        public string AttachmentRequired { get; set; }
        private SPServiceSectionCondition _AttachmentRequiredCondition;
        public string IconUrl { get; set; }

        public Boolean HideSummary { get; set; }
        public Boolean HideThankYouScreen { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<SPFieldLookupValue> ModuleStage { get; set; }
        public Boolean ShowStageTransitionButtons { get; set; }
        public Boolean LoadDefaultValue { get; set; }
        public Boolean AttachmentsInChildTickets { get; set; }
        public bool SLADisabled { get; set; }
        public double ResolutionSLA { get; set; }
        public SPSLAConfiguration SLAConfig { get; set; }
        public bool StartResolutionSLAFromAssigned { get; set; }
        public string CompletionMessage { get; set; }
        public bool Use24x7Calendar { get; set; }
        /// <summary>
        /// It will keep old and new question id mapping 
        /// </summary>
        private Dictionary<int, int> importSrvQuestionList;
        /// <summary>
        /// if attachment is conditionally mandatory then condition need to get and set using this property
        /// </summary>
        public SPServiceSectionCondition AttachmentRequiredCondition
        {
            get
            {
                if (AttachmentRequired != null && AttachmentRequired.ToLower() == "conditional")
                {
                    return _AttachmentRequiredCondition;
                }
                else
                    return null;
            }
            set
            {
                if (AttachmentRequired != null && AttachmentRequired.ToLower() == "conditional")
                {
                    _AttachmentRequiredCondition = value;
                }
                else
                    _AttachmentRequiredCondition = null;

            }
        }

        /// <summary>
        /// If Attachment is mandatory then this property will be true
        /// </summary>
        public bool IsAttachmentRequired
        {
            get
            {
                if (AttachmentRequired != null && (AttachmentRequired.ToLower() != "always" || AttachmentRequired.ToLower() != "conditional"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Question Map Vairable use to store some value based on condition which then may be mapped with field of task and ticket.
        /// </summary>
        public List<SPQuestionMapVariable> QMapVariables { get; set; }

        public bool EnableTaskReminder { get; set; }
        public string Reminders { get; set; }

        public SPServices()
        {
            Category = string.Empty;
            Description = string.Empty;
            CreateParentServiceRequest = true;
            AttachmentsInChildTickets = false;
            SkipTaskCondition = new List<SPServiceTaskCondition>();
            SkipSectionCondition = new List<SPServiceSectionCondition>();
            QMapVariables = new List<SPQuestionMapVariable>();
            AttachmentRequired = string.Empty;
            IconUrl = string.Empty;
            LoadDefaultValue = false;
            Use24x7Calendar = false;
            importSrvQuestionList = new Dictionary<int, int>();
        }
    }
}