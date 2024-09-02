using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DataTransfer.SharePointToDotNet;
using uGovernIT.Util.Log;

namespace uGovernIT.DataTransfer.Infratructure
{
    public class ModuleEntity : IModuleEntity
    {
        protected ImportContext bContext;
        protected string moduleName;
        public ModuleEntity(ImportContext context, string moduleName)
        {
            this.bContext = context;
            this.moduleName = moduleName;
        }

        public void ImportConfiguration()
        {
            UpdateModuleColumns();
            UpdateRequestTypes();
            UpdateWorkflow();
            UpdateProjectLifecycles();
            UpdateFormLayoutAndAccess();
            UpdatePriorityMap();
            UpdateModuleEmails();
            UpdateTicketEmails();
            UpdateTicketSLAs();
            StageExitCriteria();
            UpdateHelpCards();
            UpdateWikis();
            UpdateGenericStausMapping();
        }
        
        public void Importdata()
        {
            UpdateTicketData();
            UpdateTicketArchiveData();
            UpdateTicketDataHistory();
            UpdateTicketTasks();
            UpdateRelatedTickets();
            ProjectMonitorState();
            ProjectMonitorStateHistory();
            Sprint();
            ProjectReleases();
            SprintTasks();
            PMMEvents();
            NPRResources();
            TicketCountTrends();
            ModuleBudget();
            ModuleBudgetActuals();
            ModuleBudgetActualsHistory();
            ModuleBudgetHistory();
            ModuleMonthlyBudget();
            ModuleMonthlyBudgetHistory();
            BaseLineDetails();
            SchedulerActions();
            SchedulerActionArchives();
            PMMComments();
            PMMCommentsHistory();
            AssetIncidentRelation();
            ApplicationServer();
            ApplicationModules();
            ApplicationRole();
            ApplicationAccess();
            TicketEvents();
            
        }

        public virtual void UpdateFormLayoutAndAccess()
        {
          
        }

        public virtual void UpdateModuleColumns()
        {
         
        }
        public virtual void UpdateGenericStausMapping()
        {
         
        }
        
        public virtual void UpdatePriorityMap()
        {
           
        }

        public virtual void UpdateRequestTypes()
        {
          
        }

        public virtual void UpdateTicketData()
        {
          
        }

        public virtual void UpdateTicketArchiveData()
        {

        }

        public virtual void UpdateModuleEmails()
        {
       
        }
        public virtual void UpdateTicketEmails()
        {

        }
        
        public virtual void UpdateTicketSLAs()
        {
           
        }

        public virtual void UpdateWorkflow()
        {
           
        }
        public virtual void UpdateProjectLifecycles()
        {

        }

        public virtual void UpdateTicketDataHistory()
        {
           
        }

        public virtual void UpdateTicketTasks()
        {

        }

        public virtual void UpdateRelatedTickets()
        {

        }

        public virtual void ProjectMonitorState()
        {

        }

        public virtual void ProjectMonitorStateHistory()
        {

        }

        public virtual void Sprint()
        {

        }

        public virtual void ProjectReleases()
        {

        }

        public virtual void SprintTasks()
        {

        }
        
        public virtual void TicketHours()
        {

        }

        public virtual void PMMEvents()
        {

        }

        public virtual void NPRResources()
        {

        }

        public virtual void TicketCountTrends()
        {

        }

        public virtual void ModuleBudget()
        {

        }

        public virtual void ModuleBudgetActuals()
        {

        }

        public virtual void ModuleBudgetActualsHistory()
        {

        }

        public virtual void ModuleBudgetHistory()
        {

        }

        public virtual void ModuleMonthlyBudget()
        {

        }

        public virtual void ModuleMonthlyBudgetHistory()
        {

        }


        public virtual void BaseLineDetails()
        {

        }

        public virtual void SchedulerActions()
        {

        }

        public virtual void SchedulerActionArchives()
        {

        }


        public virtual void PMMComments()
        {

        }

        public virtual void PMMCommentsHistory()
        {

        }

        public virtual void StageExitCriteria()
        {

        }

        public virtual void UpdateHelpCards()
        {
            
        }

        public virtual void UpdateWikis()
        {
            
        }
        public virtual void AssetIncidentRelation()
        {

        }
        public virtual void ApplicationServer()
        {

        }
        public virtual void ApplicationModules()
        {

        }

        public virtual void ApplicationRole()
        {

        }
        public virtual void ApplicationAccess()
        {

        }
        public virtual void TicketEvents()
        {

        }
        
    }

}
