using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModulePriorityMapStore : StoreBase<ModulePriorityMap>, IModulePriorityMapStore
    {
        public ModulePriorityMapStore(CustomDbContext context) : base(context)
        {
        }
        public List<ModulePriorityMap> LoadByModule(string moduleName)
        {
            List<ModulePriorityMap> objModulePriorityMap = new List<ModulePriorityMap>();
            objModulePriorityMap = this.Load(x => x.ModuleNameLookup == moduleName && x.Deleted != true, includeExpressions: new List<Expression<Func<ModulePriorityMap, object>>>() { x => x.ModulePrioirty, x => x.ModuleSeverity, x => x.ModuleImpact });
            return objModulePriorityMap;
        }
    }
    public interface IModulePriorityMapStore : IStore<ModulePriorityMap>
    {
        List<ModulePriorityMap> LoadByModule(string moduleName);
    }
}
