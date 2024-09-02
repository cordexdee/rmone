using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleRequestTypeStore : StoreBase<ModuleRequestType>
    {
        public ModuleRequestTypeStore(CustomDbContext context) : base(context)
        {
        }
        public List<ModuleRequestType> LoadByModule(string moduleName)
        {
            List<ModuleRequestType> objRequestType = new List<ModuleRequestType>();
            objRequestType = this.Load(x => x.ModuleNameLookup == moduleName, includeExpressions: new List<System.Linq.Expressions.Expression<Func<ModuleRequestType, object>>>() { x => x.FunctionalArea });
            return objRequestType;
        }
    }
    public interface IModuleRequestTypeStore : IStore<ModuleRequestType>
    {
        List<ModuleRequestType> LoadByModule(string moduleName);
    }
}
