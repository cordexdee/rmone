using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public class ModuleImpactStore : StoreBase<ModuleImpact>, IModuleImpactStore
    {
        public ModuleImpactStore(CustomDbContext context) : base(context)
        {
        }
        public List<ModuleImpact> LoadByModule(string moduleName)
        {
            return this.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName));
        }
    }
    public interface IModuleImpactStore : IStore<ModuleImpact>
    {
        List<ModuleImpact> LoadByModule(string moduleName);
    }

}
