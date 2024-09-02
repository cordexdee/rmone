using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class ModuleEscalationRuleManager:ManagerBase<ModuleEscalationRule>, IModuleEscalationRuleManager
    {
        public ModuleEscalationRuleManager(ApplicationContext context):base(context)
        {
            store = new ModuleEscalationRuleStore(this.dbContext);
        }
        public  DataTable LoadModuleEscalationRule()
        {
           return GetDataTable();
        }
        public ModuleEscalationRule AddUpdateSLARule(ModuleEscalationRule objModuleEscalationRule)
        {
            return (store as ModuleEscalationRuleStore).AddUpdateSLARule(objModuleEscalationRule);
        }


    }
    public interface IModuleEscalationRuleManager : IManagerBase<ModuleEscalationRule>
    {

    }
}
