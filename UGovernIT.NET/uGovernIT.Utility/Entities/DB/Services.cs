using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Services)]
    public class Services : DBBaseEntity
    {
        public string ServiceType { get; set; }
        public string Title { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceCategoryType { get; set; }
        public long? CategoryId { get; set; }
        [Column(DatabaseObjects.Columns.Owner)]
        public string OwnerUser { get; set; }
        public long ID { get; set; }
        public bool CreateParentServiceRequest { get; set; }
        public bool IncludeInDefaultData { get; set; }
        public bool OwnerApprovalRequired { get; set; }
        public bool AllowServiceTasksInBackground { get; set; }
        public string CompletionMessage { get; set; }
        public bool Use24x7Calendar { get; set; }
        public List<ServiceTaskCondition> SkipTaskCondition = new List<ServiceTaskCondition>();
        public List<ServiceSectionCondition> SkipSectionCondition = new List<ServiceSectionCondition>();
        [NotMapped]
        public List<ServiceQuestion> Questions { get; set; }
        [NotMapped]
        public List<ServiceSection> Sections { get; set; }
        [NotMapped]
        public List<UGITTask> Tasks { get; set; }
        [NotMapped]
        public List<ServiceQuestionMapping> QuestionsMapping { get; set; }
        public int ItemOrder { get; set; }
        [NotMapped]
        public int CategoryItemOrder { get; set; }

        public bool IsActivated { get; set; }
        [NotMapped]
        public Dictionary<string, string> CustomProperties { get; set; }
        public string AuthorizedToView { get; set; }
        public string NavigationUrl { get; set; }
        public string NavigationType { get; set; }
        //public bool IsDeleted { get; set; }
        public string AttachmentRequired { get; set; }
        //private ServiceSectionCondition _AttachmentRequiredCondition;
        public string ImageUrl { get; set; }

        public Boolean HideSummary { get; set; }
        public Boolean HideThankYouScreen { get; set; }
        [NotMapped]
        public long ModuleId { get; set; }
        public string ModuleNameLookup { get; set; }

        [NotMapped]
        public string serviceUrl { get; set; }
        public string ModuleStage { get; set; }
        public Boolean ShowStageTransitionButtons { get; set; }
        public string ConditionalLogic { get; set; }
        public string SectionConditionalLogic { get; set; }
        public string QuestionMapVariables { get; set; }
        [NotMapped]
        public List<QuestionMapVariable> QMapVariables { get; set; }

        public bool EnableTaskReminder { get; set; }
        public string Reminders { get; set; }

        private ServiceSectionCondition _AttachmentRequiredCondition;
        public Boolean AttachmentsInChildTickets { get; set; }
        public Boolean LoadDefaultValue { get; set; }
        public bool SLADisabled { get; set; }
        public double ResolutionSLA { get; set; }
        public bool StartResolutionSLAFromAssigned { get; set; }
        public string SLAConfiguration { get; set; }

        public string DataEditor { get; set; }

        [NotMapped]
        public SLAConfiguration SLAConfig { get; set; }

        /// <summary>
        /// if attachment is conditionally mandatory then condition need to get and set using this property
        /// </summary>
        [NotMapped]
        public ServiceSectionCondition AttachmentRequiredCondition
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
    }
}
