using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class GovernanceLinkCategoryStore : StoreBase<GovernanceLinkCategory>, IGovernanceLinkCategoryStore
    {
        public GovernanceLinkCategoryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IGovernanceLinkCategoryStore : IStore<GovernanceLinkCategory>
    {

    }
}
