using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.DAL.Store
{
    public class TenantSchedulerStore : StoreBase<TenantScheduler>, ITenantSchedulerStore
    {
        public TenantSchedulerStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ITenantSchedulerStore : IStore<TenantScheduler>
    {

    }
}
