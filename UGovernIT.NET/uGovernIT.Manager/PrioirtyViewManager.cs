using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class PrioirtyViewManager:ManagerBase<ModulePrioirty>, IPrioirtyViewManager
    {
        public PrioirtyViewManager(ApplicationContext context):base(context)
        {
            store = new ModulePrioirtyStore(this.dbContext);
        }
        public List<ModulePrioirty> LoadByModule(string moduleName)
        {
            return (store as ModulePrioirtyStore).LoadByModule(moduleName);
        }
        public  ModulePrioirty GetElevatedPriority(List<ModulePrioirty> priorities)
        {
            if (priorities == null || priorities.Count == 0)
                return null;
            ModulePrioirty priority = priorities.FirstOrDefault(x => x.IsVIP);
            //if (priority == null)
            //    priority = priorities.OrderBy(x => x.ItemOrder).Last();
            return priority;
        }
    }
    public interface IPrioirtyViewManager : IManagerBase<ModulePrioirty>
    {
        ModulePrioirty GetElevatedPriority(List<ModulePrioirty> priorities);
         List<ModulePrioirty> LoadByModule(string moduleName);
    }
}
