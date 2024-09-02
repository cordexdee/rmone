using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public class ModuleRequestTypeLocationStore : StoreBase<ModuleRequestTypeLocation>,IModuleRequestTypeLocationStore
    {
        public ModuleRequestTypeLocationStore(CustomDbContext context) : base(context)
        {
        }

        public List<ModuleRequestTypeLocation> LoadByModule(string moduleName)
        {
            return this.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName));
        }
    }
    public interface IModuleRequestTypeLocationStore: IStore<ModuleRequestTypeLocation>
    {
        List<ModuleRequestTypeLocation> LoadByModule(string moduleName);
    }
}
