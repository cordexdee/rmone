using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IModuleBudgetActualsHistoryStore : IStore<ModuleBudgetsActualHistory>
    {

    }

    public class ModuleBudgetActualsHistoryStore : StoreBase<ModuleBudgetsActualHistory>, IModuleBudgetActualsHistoryStore
    {
        public ModuleBudgetActualsHistoryStore(CustomDbContext context) : base(context)
        {

        }
    }

}
