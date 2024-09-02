using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace uGovernIT.DefaultConfig
{
    public interface IDefault
    {
        void UpdateRoles(ApplicationContext applicationContext, string AccountID, bool createSuperAdmin);
        void UpdateUsers(ApplicationContext applicationContext, string AccountID, AccountInfo accountInfo, bool createSuperAdmin);

        void UpdateMenuNavigations(ApplicationContext applicationContext);
        void UpdateTabView(ApplicationContext applicationContext);
        void UpdateFieldConfigData(ApplicationContext applicationContext);
        void InsertProjectLifeCycles(ApplicationContext applicationContext);
        List<UGITModule> GetUGITModule(ApplicationContext applicationContext);
        List<ModuleSeverity> GetModuleSeverity();
        List<ModuleImpact> GetModuleImpact();
        List<ModulePrioirty> GetModulePrioirty();
        List<ModuleColumn> GetModuleColumns(ApplicationContext applicationContext);
        List<ModulePriorityMap> GetModulePriorityMap();
        List<Module_StageType> GetModuleStageType(ApplicationContext applicationContext);
        List<ConfigurationVariable> GetConfigurationVariable(ApplicationContext applicationContext);
        List<State> GetStates(ApplicationContext applicationContext);
        List<CRMRelationshipType> GetCRMRelationshipTypes(ApplicationContext applicationContext);
        List<ModuleUserType> GetModuleUserType();
        List<BudgetCategory> GetBudgetCategories(ApplicationContext applicationContext);
        List<Location> GetLocation(ApplicationContext applicationContext);
        List<ModuleMonitorOption> GetModuleMonitorOptions(ApplicationContext applicationContext, List<ModuleMonitor> ModuleMonitorList);
        List<ModuleMonitor> GetModuleMonitors(ApplicationContext applicationContext);
        List<Company> GetCompany(ApplicationContext applicationContext);
        List<Department> GetDepartment(ApplicationContext applicationContext, List<Company> companyList);
        List<FunctionalArea> GetFunctionalAreas(ApplicationContext applicationContext, List<Department> departmentList);
        List<MailTokenColumnName> GetMailTokenColumnName(ApplicationContext applicationContext);
        List<MessageBoard> GetMessageBoard(ApplicationContext applicationContext);
        List<ClientAdminConfigurationList> GetClientAdminConfigurationLists(ApplicationContext applicationContext, List<ClientAdminCategory> ClientAdminCategoryList);
        List<ClientAdminCategory> GetClientAdminCategory(ApplicationContext applicationContext);
        List<GenericTicketStatus> GetGenericTicketStatus(ApplicationContext applicationContext);

        List<PageConfiguration> GetPages();
    }
}
