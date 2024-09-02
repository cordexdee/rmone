using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ResourceAllocationStore : StoreBase<RResourceAllocation>, IResourceAllocationStore
    {
        public ResourceAllocationStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IResourceAllocationStore : IStore<RResourceAllocation>
    {
    }
}
