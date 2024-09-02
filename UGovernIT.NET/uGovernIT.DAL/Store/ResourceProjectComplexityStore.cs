using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ResourceProjectComplexityStore : StoreBase<SummaryResourceProjectComplexity>, IResourceProjectComplexityStore
    {
        public ResourceProjectComplexityStore(CustomDbContext context) : base(context)
        {

        }
    }
    interface IResourceProjectComplexityStore : IStore<SummaryResourceProjectComplexity>
    {

    }
}
