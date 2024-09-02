using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IModuleMonthlyBudgetHistoryManager : IManagerBase<ModuleMonthlyBudgetHistory>
    {


    }

    public class ModuleMonthlyBudgetHistoryManager : ManagerBase<ModuleMonthlyBudgetHistory>, IModuleMonthlyBudgetHistoryManager
    {
        public ModuleMonthlyBudgetHistoryManager(ApplicationContext context) : base(context)
        {
            store = new ModuleMonthlyBudgetHistoryStore(this.dbContext);
        }
    }
}
