using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.Manager
{
    
    public class TenantSchedulerManager : ManagerBase<TenantScheduler>, ITenantSchedulerManager
    {
        public TenantSchedulerManager(ApplicationContext context) : base(context)
        {
            store = new TenantSchedulerStore(this.dbContext);
        }
    }
    public interface ITenantSchedulerManager : IManagerBase<TenantScheduler>
    {

    }
}
