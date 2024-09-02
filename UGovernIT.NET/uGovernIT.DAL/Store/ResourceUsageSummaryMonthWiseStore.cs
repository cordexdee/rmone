using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ResourceUsageSummaryMonthWiseStore:StoreBase<ResourceUsageSummaryMonthWise>, IResourceUsageSummaryMonthWiseStore
    {
        public ResourceUsageSummaryMonthWiseStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IResourceUsageSummaryMonthWiseStore : IStore<ResourceUsageSummaryMonthWise>
    {
    }
}
