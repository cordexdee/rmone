using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class CRMProjectAllocationStore : StoreBase<ProjectEstimatedAllocation>, ICRMProjectAllocationStore
    {
        public CRMProjectAllocationStore(CustomDbContext context) : base(context)
        {
        }

    }

    public interface ICRMProjectAllocationStore : IStore<ProjectEstimatedAllocation>
    {

    }
}
