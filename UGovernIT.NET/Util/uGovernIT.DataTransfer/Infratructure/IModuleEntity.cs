using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.DataTransfer.Infratructure
{
    public interface IModuleEntity
    {
        void Importdata();
        void ImportConfiguration();
        void UpdateModuleColumns();
        /// <summary>
        /// Update form tab, form layout, rolewriteacess
        /// </summary>
        void UpdateFormLayoutAndAccess();
        /// <summary>
        /// Update user types, workflow, default value
        /// </summary>
        void UpdateWorkflow();
        void UpdateProjectLifecycles();
        /// <summary>
        /// Update request type, request type by location
        /// </summary>
        void UpdateRequestTypes();

        //Update Severity, impact, priority and priority mapping
        void UpdatePriorityMap();

        void UpdateTicketData();
        void UpdateTicketDataHistory();

        void UpdateModuleEmails();
        void UpdateTicketEmails();
        void UpdateTicketSLAs();

        void StageExitCriteria();

        void UpdateHelpCards();
        void UpdateWikis();
        

        void UpdateTicketTasks();
        void UpdateRelatedTickets();
        void ProjectMonitorState();
        void ProjectMonitorStateHistory();
        void Sprint();
        void ProjectReleases();
        void SprintTasks();
        
    
        
        void TicketHours();
        void PMMEvents();
        void NPRResources();
        void TicketCountTrends();
        void ModuleBudget();
        void ModuleBudgetActuals();
        void ModuleBudgetActualsHistory();
        void ModuleBudgetHistory();
        void ModuleMonthlyBudget();
        void ModuleMonthlyBudgetHistory();
        void BaseLineDetails();
        void SchedulerActions();
        void SchedulerActionArchives();
        
        void PMMComments();
        void PMMCommentsHistory();
        void ApplicationModules();
        void ApplicationRole();
        void ApplicationAccess();
        void TicketEvents();
        void UpdateGenericStausMapping();
    }
}
