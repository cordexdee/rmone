using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleFormTabStore : StoreBase<ModuleFormTab>, IModuleFormTabStore
    {
        public ModuleFormTabStore(CustomDbContext context) : base(context)
        {
        }

        public List<ModuleFormTab> LoadByModule(string moduleName)
        {
            List<ModuleFormTab> objModuleFormTab = new List<ModuleFormTab>();
            objModuleFormTab = this.Load(string.Format("Where ModuleNameLookup='{0}'",moduleName));// moduleFormTab.GetData().ToList();
            return objModuleFormTab;
        }
    }

    public interface IModuleFormTabStore : IStore<ModuleFormTab>
    {
        List<ModuleFormTab> LoadByModule(string moduleName);


    }
}
