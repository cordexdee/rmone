using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{

    public interface IModuleBudgetActualsHistoryManager : IManagerBase<ModuleBudgetsActualHistory>
    {

    }

    public class ModuleBudgetActualsHistoryManager:ManagerBase<ModuleBudgetsActualHistory>, IModuleBudgetActualsHistoryManager
    {
        public ModuleBudgetActualsHistoryManager(ApplicationContext context) : base(context)
        {
            store = new ModuleBudgetActualsHistoryStore(this.dbContext);
        }
    }
}
