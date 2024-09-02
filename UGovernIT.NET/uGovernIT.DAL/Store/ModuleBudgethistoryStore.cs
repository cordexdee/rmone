using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{

    public class ModuleBudgetHistoryStore : StoreBase<ModuleBudgetHistory>, IModuleBudgetHistoryStore
    {
        public ModuleBudgetHistoryStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface IModuleBudgetHistoryStore : IStore<ModuleBudgetHistory>
    {

    }

}
