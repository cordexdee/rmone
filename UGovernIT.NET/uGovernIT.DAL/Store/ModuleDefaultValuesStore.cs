using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class ModuleDefaultValuesStore : StoreBase<ModuleDefaultValue>, IModuleDefaultValueStore
    {
        public ModuleDefaultValuesStore(CustomDbContext context) : base(context)
        {

        }
        public List<ModuleDefaultValue> LoadByModule(string moduleName)
        {
            List<ModuleDefaultValue> moduleDefaultValues = this.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName)); 
            return moduleDefaultValues;
        }
        

    }
    public interface IModuleDefaultValueStore : IStore<ModuleDefaultValue>
    {
        List<ModuleDefaultValue> LoadByModule(string moduleName);
    }
}
