using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public class SprintSummaryStore:StoreBase<SprintSummary>, ISprintSummaryStore
    {
        public SprintSummaryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ISprintSummaryStore : IStore<SprintSummary>
    {

    }
}
