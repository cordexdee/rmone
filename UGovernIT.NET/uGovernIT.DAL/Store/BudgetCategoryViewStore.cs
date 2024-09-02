using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class BudgetCategoryViewStore : StoreBase<BudgetCategory>, IBudgetCategoryViewStore
    {
        public BudgetCategoryViewStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IBudgetCategoryViewStore : IStore<BudgetCategory>
    {

    }
}
