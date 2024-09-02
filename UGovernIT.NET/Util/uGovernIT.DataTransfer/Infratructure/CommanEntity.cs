using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DataTransfer.SharePointToDotNet;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.DataTransfer.Infratructure
{
    public class CommanEntity : ICommanEntity
    {
        protected ImportContext baseContext;
        public CommanEntity(ImportContext context)
        {
            this.baseContext = context;
        }

        public virtual void CreateDefaultEntries()
        {
           
        }

        public void ExcecuteFinalMethods()
        {
            UpdateServiceCatalogAndAgents();
            UpdateSurveyFeedback();
            UpdateRelatedTickets();
        }
        public void Excecute_SPSpecificlMethods()
        {
            UpdateMyModuleColumns();
            UpdateTasktemplates();
            UpdateTaskTemplateItems();
            ProjectSimilarityConfig();
        }

        public void Excecute()
        {
            UpdateTenantDefaultUser();
            UpdateLocation();
            UpdateDepartments();
            UpdateUserSkills();
            UpdateUsersAndRoles();
            UpdateModules();
            UpdateFunctionalAreas();
            CreateDefaultEntries();
            UpdateConfigurationVariables();
            UpdateBudgetsCategory();
            UpdateWikiCategories();
            UpdateStageExitCriteriaTemplates();
            UpdateDashboardAndQueries();
            //UpdateFileAttachments();
            UpdateQuickTickets();
            UpdateAssetVendors();
            UpdateAssetModels();
            UpdateFactTables();
            UpdateMessageBoard();
            UpdateACRTypes();
            UpdateDRQRapidTypes();
            UpdateDRQSystemAreas();
            UpdateEnvironment();
            UpdateSubLocation();
            //UpdateProjectLifecycles();
            UpdateProjectInitiative();
            UpdateProjectClass();
            UpdateProjectStandards();
            UpdateGlobalRoles();
            UpdateLandingPages();
            UpdateJobTitle();
            UpdateEmployeeTypes();
            UpdateuGovernITLogs();
            UpdateGovernanceConfiguration();
            UpdateModuleMonitors();
            UpdateModuleMonitorOptions();
            UpdateProjectComplexity();
            UpdateMailTokenColumnName();
            UpdateGenericStatus();
            UpdateLinkViews();
            UpdateTenantScheduler();
            
            UpdatePhrases();
            UpdateWidgets();
            UpdateDocuments();
            RankingCriteria();
            LeadCriteria();
            ChecklistTemplates();
            Studio();
            CRMRelationshipTypeLookup();
            State();
            ResourceWorkItems();
            TicketHours();
            ResourceTimeSheet();
            ResourceAllocation();
            ResourceAllocationMonthly();
            ProjectEstimatedAllocation();
            ProjectPlannedAllocation();
            ResourceUsageSummaryWeekWise();
            ResourceUsageSummaryMonthWise();
        }

        public virtual void UpdateBudgetsCategory()
        {
            
        }

        public virtual void UpdateConfigurationVariables()
        {
          
        }

        public virtual void UpdateDepartments()
        {
           
        }

        public virtual void UpdateFunctionalAreas()
        {
            
        }

        public virtual void UpdateLocation()
        {
          
        }

        public virtual void UpdateModules()
        {
            
        }

        public virtual void UpdateTenantDefaultUser()
        {
          

        }

        public virtual void UpdateUsersAndRoles()
        {
          
        }

        public virtual void UpdateWikiCategories()
        {

        }

        public virtual void UpdateStageExitCriteriaTemplates()
        {

        }

        public virtual void UpdateDashboardAndQueries()
        {

        }

        public virtual void UpdateFileAttachments()
        {

        }

        public virtual void UpdateQuickTickets()
        {

        }

        public virtual void UpdateAssetVendors()
        {

        }

        public virtual void UpdateAssetModels()
        {

        }

        public virtual void UpdateFactTables()
        {

        }

        public virtual void UpdateMessageBoard()
        {

        }

        public virtual void UpdateUserSkills()
        {

        }

        public virtual void UpdateServiceCatalogAndAgents()
        {

        }

        public virtual void UpdateACRTypes()
        {

        }

        public virtual void UpdateDRQRapidTypes()
        {

        }

        public virtual void UpdateDRQSystemAreas()
        {

        }

        public virtual void UpdateEnvironment()
        {

        }

        public virtual void UpdateSubLocation()
        {

        }

        //public virtual void UpdateProjectLifecycles()
        //{

        //}

        public virtual void UpdateProjectInitiative()
        {

        }

        public virtual void UpdateProjectClass()
        {

        }

        public virtual void UpdateProjectStandards()
        {

        }

        public virtual void UpdateGlobalRoles()
        {

        }

        public virtual void UpdateLandingPages()
        {

        }

        public virtual void UpdateJobTitle()
        {

        }

        public virtual void UpdateEmployeeTypes()
        {

        }

        public virtual void UpdateuGovernITLogs()
        {

        }

        public virtual void UpdateGovernanceConfiguration()
        {

        }

        public virtual void UpdateModuleMonitors()
        {

        }

        public virtual void UpdateModuleMonitorOptions()
        {

        }

        public virtual void UpdateProjectComplexity()
        {

        }

        public virtual void UpdateMailTokenColumnName()
        {

        }

        public virtual void UpdateGenericStatus()
        {

        }

        public virtual void UpdateLinkViews()
        {

        }

        public virtual void UpdateTenantScheduler()
        {

        }

        public virtual void UpdateSurveyFeedback()
        {

        }
        public virtual void UpdateRelatedTickets()
        {

        }

        public virtual void UpdatePhrases()
        {

        }

        public virtual void UpdateWidgets()
        {

        }

        public virtual void UpdateDocuments()
        {

        }
        public virtual void ChecklistTemplates()
        {
        }

        public virtual void LeadCriteria()
        {
        }

        public virtual void RankingCriteria()
        {
        }

        public virtual void Studio()
        {
        }
        public virtual void CRMRelationshipTypeLookup()
        {
        }
        
        public virtual void State()
        {
        }
        public virtual void ResourceWorkItems()
        {

        }
        public virtual void TicketHours()
        {

        }
        public virtual void ResourceTimeSheet()
        {

        }
        public virtual void ResourceAllocation()
        {

        }

        public virtual void ResourceUsageSummaryMonthWise()
        {

        }

        public virtual void ResourceUsageSummaryWeekWise()
        {

        }
        public virtual void ResourceAllocationMonthly()
        {

        }

        public virtual void ProjectEstimatedAllocation()
        {

        }

        public virtual void ProjectPlannedAllocation()
        {

        }

        public virtual void UpdateMyModuleColumns()
        {

        }
        public virtual void UpdateTasktemplates()
        {

        }
        public virtual void UpdateTaskTemplateItems()
        {

        }
        public virtual void ProjectSimilarityConfig()
        {

        }

    }

}
