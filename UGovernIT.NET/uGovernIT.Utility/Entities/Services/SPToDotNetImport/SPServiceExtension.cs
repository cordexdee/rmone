using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Utility
{
    public class SPServiceExtension:SPServices
    {
        public List<SPUserInfo> UserInfo { get; set; }
        public List<SPLookupValueServiceExtension> LookupValues { get; set; }
        public SPServiceExtension() { }
        public SPServiceExtension(SPServices service)
            : base()
        {
            Title = service.Title;
            Description = service.Description;

            Category = service.Category;

            CategoryId = service.CategoryId;

            Owners = service.Owners;

            ID = service.ID;

            CreateParentServiceRequest = service.CreateParentServiceRequest;

            OwnerApprovalRequired = service.OwnerApprovalRequired;

            SkipTaskCondition = service.SkipTaskCondition;

            SkipSectionCondition = service.SkipSectionCondition;

            Questions = service.Questions;

            Sections = service.Sections;

            Tasks = service.Tasks;

            QuestionsMapping = service.QuestionsMapping;

            ItemOrder = service.ItemOrder;

            Activated = service.Activated;

            CustomProperties = service.CustomProperties;

            AuthorizedToView = service.AuthorizedToView;

            NavigationUrl = service.NavigationUrl;
            QMapVariables = service.QMapVariables;

            IconUrl = service.IconUrl;

            ModuleId = service.ModuleId;
            ModuleName = service.ModuleName;
            LoadDefaultValue = service.LoadDefaultValue;
            ShowStageTransitionButtons = service.ShowStageTransitionButtons;
            HideSummary = service.HideSummary;
            HideThankYouScreen = service.HideThankYouScreen;
            ModuleStage = service.ModuleStage;

            ResolutionSLA = service.ResolutionSLA;
            SLADisabled = service.SLADisabled;
            StartResolutionSLAFromAssigned = service.StartResolutionSLAFromAssigned;
            AllowServiceTasksInBackground = service.AllowServiceTasksInBackground;

            AttachmentRequired = service.AttachmentRequired;
            AttachmentRequiredCondition = service.AttachmentRequiredCondition;
            AttachmentsInChildTickets = service.AttachmentsInChildTickets;
            NavigationUrl = service.NavigationUrl;
            CompletionMessage = service.CompletionMessage;
            LoadDefaultValue = service.LoadDefaultValue;

            EnableTaskReminder = service.EnableTaskReminder;
            Reminders = service.Reminders;
            SLAConfig = service.SLAConfig;
            UserInfo = new List<SPUserInfo>();
            LookupValues = new List<SPLookupValueServiceExtension>();

        }
    }
}