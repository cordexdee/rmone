using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
  public  class ModuleUserTypeStore : StoreBase<ModuleUserType>, IModuleUserTypeStore
    {
        public ModuleUserTypeStore(CustomDbContext context) : base(context)
        {
        }
        public List<ModuleUserType> LoadByModule(string moduleName)
        {
            return  this.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName));  // moduleUserType.GetData().ToList();
         }
  
        
    }
    public interface IModuleUserTypeStore : IStore<ModuleUserType>
    {
        List<ModuleUserType> LoadByModule(string moduleName);
      //List<ModuleUserType> GetConfigModuleUserTypeData();
     
    }
}
