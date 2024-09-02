using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class AllocationStore:StoreBase<Allocation>, IAllocation
    {
        public AllocationStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IAllocation : IStore<Allocation>
    {
    }
}
