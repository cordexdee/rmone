using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;
using System.Data;

namespace uGovernIT.Manager
{
   public class RequestTypeManager:ManagerBase<ModuleRequestType>, IRequestTypeManager
    {
        ApplicationContext _context;
        public RequestTypeManager(ApplicationContext context):base(context)
        {
            _context = context;
            store = new RequestTypeStore(this.dbContext);
        }

        public List<ModuleRequestType> GetConfigModuleRequestTypeData(string moduleName)
        {
            return store.Load(string.Format("Where ModuleNameLookup = '{0}'", moduleName));  
        }
    }
    public interface IRequestTypeManager : IManagerBase<ModuleRequestType>
    {
        List<ModuleRequestType> GetConfigModuleRequestTypeData(string moduleName);
    }
}
