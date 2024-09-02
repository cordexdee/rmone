using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IModuleMonthlyBudgetHistoryStore : IStore<ModuleMonthlyBudgetHistory>
    {

    }
    public class ModuleMonthlyBudgetHistoryStore : StoreBase<ModuleMonthlyBudgetHistory>, IModuleMonthlyBudgetHistoryStore
    {
        public ModuleMonthlyBudgetHistoryStore(CustomDbContext context) : base(context)
        {

        }
    }
}
