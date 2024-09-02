using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.DataTransfer.Infratructure
{
    public interface ICommanEntity
    {
        void UpdateTenantDefaultUser();
        void UpdateLocation();
        void Excecute();
        void UpdateFunctionalAreas();
        void UpdateDepartments();
        void UpdateUsersAndRoles();
        void UpdateModules();
        void UpdateBudgetsCategory();
        void UpdateConfigurationVariables();
        void CreateDefaultEntries();
        void UpdateWikiCategories();
        void UpdateStageExitCriteriaTemplates();
        void UpdateDashboardAndQueries();
        void UpdateFileAttachments();
        void UpdateQuickTickets();
        void UpdateAssetVendors();
        void UpdateAssetModels();
        void UpdateFactTables();
        void UpdateMessageBoard();
        void UpdateUserSkills();
        void UpdateServiceCatalogAndAgents();
        void UpdateMyModuleColumns();
        void UpdateACRTypes();
        void UpdateDRQRapidTypes();
        void UpdateDRQSystemAreas();
        void UpdateEnvironment();
        void UpdateSubLocation();
        //void UpdateProjectLifecycles();
        void UpdateProjectInitiative();
        void UpdateProjectClass();
        void UpdateProjectStandards();
        void UpdateGlobalRoles();
        void UpdateLandingPages();
        void UpdateJobTitle();
        void UpdateEmployeeTypes();
        void UpdateuGovernITLogs();
        void UpdateGovernanceConfiguration();
        void UpdateModuleMonitors();
        void UpdateModuleMonitorOptions();
        void UpdateProjectComplexity();
        void UpdateMailTokenColumnName();
        void UpdateGenericStatus();
        void UpdateLinkViews();
        void UpdateTenantScheduler();
        void UpdateSurveyFeedback();
        void UpdatePhrases();
        void UpdateWidgets();
        void UpdateDocuments();
        void ResourceWorkItems();
        void TicketHours();
        void ResourceTimeSheet();
        void ResourceAllocation();
        void ResourceUsageSummaryMonthWise();
        void ResourceUsageSummaryWeekWise();

        void ResourceAllocationMonthly();
        void ProjectEstimatedAllocation();
        void ProjectPlannedAllocation();
        void UpdateRelatedTickets();

        void UpdateTasktemplates();
        void UpdateTaskTemplateItems();
        void ProjectSimilarityConfig();
    }
}
