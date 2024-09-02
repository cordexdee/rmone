using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleFormLayoutStore : StoreBase<ModuleFormLayout>, IModuleFormLayoutStore
    {
        public ModuleFormLayoutStore(CustomDbContext context)
            : base(context)
        {
        }
        public List<ModuleFormLayout> LoadByModule(string moduleName)
        {
            List<ModuleFormLayout> objFormLayout = this.Load(string.Format("Where ModuleNameLookup = '{0}'", moduleName)); // moduleFormLayot.GetData().ToList();
            return objFormLayout;
        }

    }

    public interface IModuleFormLayoutStore : IStore<ModuleFormLayout>
    {
        List<ModuleFormLayout> LoadByModule(string moduleName);
    }
}
