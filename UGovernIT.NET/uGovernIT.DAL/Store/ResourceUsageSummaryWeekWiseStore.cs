using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ResourceUsageSummaryWeekWiseStore : StoreBase<ResourceUsageSummaryWeekWise>, IResourceUsageSummaryWeekWiseStore
    {
        public ResourceUsageSummaryWeekWiseStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IResourceUsageSummaryWeekWiseStore : IStore<ResourceUsageSummaryWeekWise>
    {
    }
}
