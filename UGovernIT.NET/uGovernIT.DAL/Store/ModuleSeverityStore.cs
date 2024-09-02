using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public class ModuleSeverityStore : StoreBase<ModuleSeverity>, ISeverityStore
    {
        public ModuleSeverityStore(CustomDbContext context) : base(context)
        {
        }
        public List<ModuleSeverity> LoadByModule(string moduleName)
        {
            List<ModuleSeverity> objModuleSeverity = new List<ModuleSeverity>();
            objModuleSeverity = this.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName)); // moduleSeverity.GetData().ToList();
            return objModuleSeverity;
        }
    }
    public interface ISeverityStore : IStore<ModuleSeverity>
    {
        List<ModuleSeverity> LoadByModule(string moduleName);
    }
}
