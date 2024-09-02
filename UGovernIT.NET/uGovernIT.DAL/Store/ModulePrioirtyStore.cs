using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public class ModulePrioirtyStore : StoreBase<ModulePrioirty>,IModulePrioirtyStore
    {
        public ModulePrioirtyStore(CustomDbContext context) : base(context)
        {
        }
        public List<ModulePrioirty> LoadByModule(string moduleName)
        {
            List<ModulePrioirty> objModulePrioirty = new List<ModulePrioirty>();
                objModulePrioirty = this.Load(string.Format("Where ModuleNameLookup='{0}'",moduleName)); // modulePrioirty.GetData().ToList();
            if (objModulePrioirty != null)
                objModulePrioirty = objModulePrioirty.OrderBy(x => x.Title).ToList();
            return objModulePrioirty;
            
        }
    }
    public interface IModulePrioirtyStore : IStore<ModulePrioirty>
    {
        List<ModulePrioirty> LoadByModule(string moduleName);
    }
}

