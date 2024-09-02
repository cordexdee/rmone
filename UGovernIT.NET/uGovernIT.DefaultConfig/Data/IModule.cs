using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig
{
    public interface IModule
    {
        UGITModule Module { get; }
        string ModuleName { get; }
        List<ModuleColumn> GetModuleColumns();
        List<ModuleFormTab> GetFormTabs();
        //List<ModuleImpact> GetImpact();

        List<ModuleStatusMapping> GetModuleStatusMapping();
        List<ModuleUserType> GetModuleUserType();
        List<ModuleDefaultValue> GetModuleDefaultValue();
        List<ModuleFormLayout> GetModuleFormLayout();
        List<ModuleTaskEmail> GetModuleTaskEmail();
        List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess();
        List<ModuleRequestType> GetModuleRequestType();
        List<LifeCycleStage> GetLifeCycleStage();
        List<ACRType> GetACRTypes();
        List<DRQRapidType> GetDRQRapidTypes();
        List<DRQSystemArea> GetDRQSystemAreas();

        // List<ModuleSeverity> GetModuleSeverity();
        // List<ModulePrioirty> GetModulePriority();
        // List<ModulePriorityMap> GetModulePriorityMap();
        void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping);
        List<TabView> UpdateTabView();
    }
}
