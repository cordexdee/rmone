using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
   public  class RequestPriorityManager: ManagerBase<ModulePriorityMap>, IRequestPriorityManager
    {
        public RequestPriorityManager(ApplicationContext context):base(context)
        {
            store = new RequestPriorityStore(this.dbContext);
        }
    }
    public interface IRequestPriorityManager : IManagerBase<ModulePriorityMap>
    {

    }
}
