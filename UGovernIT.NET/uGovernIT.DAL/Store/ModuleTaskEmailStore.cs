using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public class ModuleTaskEmailStore : StoreBase<ModuleTaskEmail>, IModuleTaskEmailStore
    {
        public ModuleTaskEmailStore(CustomDbContext context) : base(context)
        {
        }
        public List<ModuleTaskEmail> LoadByModule(string moduleName)
        {
            List<ModuleTaskEmail> objModuleTaskEmail = new List<ModuleTaskEmail>();
            objModuleTaskEmail = this.Load(string.Format(" Where ModuleNameLookup='{0}'", moduleName)); 
            return objModuleTaskEmail;
        }
    }
    public interface IModuleTaskEmailStore : IStore<ModuleTaskEmail>
    {
        List<ModuleTaskEmail> LoadByModule(string moduleName);
    }
}
