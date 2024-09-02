using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class SubContractorStore : StoreBase<SubContractor>, ISubContractorStore
    {
        public SubContractorStore(CustomDbContext context) : base(context)
        {
        }
    }

    public interface ISubContractorStore : IStore<SubContractor>
    {

    }
}
