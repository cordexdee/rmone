using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Utility
{
    public class ServiceExtension : Services
    {
        public List<UserProfile> UserInfo { get; set; }
        public List<LookupValueServiceExtension> LookupValues { get; set; }
        public ServiceExtension() { }
        public ServiceExtension(Services service)
            : base()
        {
            Title = service.Title;
            ServiceDescription = service.ServiceDescription;
            //ServiceCategory = service.Category;
            CategoryId = service.CategoryId;
            OwnerUser = service.OwnerUser;
            ID = service.ID;
            CreateParentServiceRequest = service.CreateParentServiceRequest;
            IncludeInDefaultData   = service.IncludeInDefaultData;
            OwnerApprovalRequired = service.OwnerApprovalRequired;
            SkipTaskCondition = service.SkipTaskCondition;
            SkipSectionCondition = service.SkipSectionCondition;
            Questions = service.Questions;
            Sections = service.Sections;
            Tasks = service.Tasks;
            QuestionsMapping = service.QuestionsMapping;
            ItemOrder = service.ItemOrder;
            IsActivated = service.IsActivated;
            CustomProperties = service.CustomProperties;
            AuthorizedToView = service.AuthorizedToView;
        
            NavigationUrl = service.NavigationUrl;
            QMapVariables = service.QMapVariables;

            ImageUrl = service.ImageUrl;

            ModuleId = service.ModuleId;
            ModuleNameLookup = service.ModuleNameLookup;
            LoadDefaultValue = service.LoadDefaultValue;
            ShowStageTransitionButtons = service.ShowStageTransitionButtons;
            HideSummary = service.HideSummary;
            HideThankYouScreen = service.HideThankYouScreen;
            ModuleStage = service.ModuleStage;
            ServiceType = service.ServiceType;
            ServiceCategoryType = service.ServiceCategoryType;
            SLADisabled = service.SLADisabled;
            ResolutionSLA = service.ResolutionSLA;
            EnableTaskReminder = service.EnableTaskReminder;
            Reminders = service.Reminders;
            StartResolutionSLAFromAssigned = service.StartResolutionSLAFromAssigned;
            SLAConfig = service.SLAConfig;
        }
    }
}
