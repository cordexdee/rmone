using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ResourceAllocationMonthlyStore : StoreBase<ResourceAllocationMonthly>, IResourceAllocationMonthlyStore
    {
        public ResourceAllocationMonthlyStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IResourceAllocationMonthlyStore : IStore<ResourceAllocationMonthly>
    {
    }
}
