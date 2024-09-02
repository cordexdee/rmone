using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public interface IModuleMonitorOptionHistoryStore : IStore<ModuleMonitorOptionHistory>
    {

    }

    public class ModuleMonitorOptionHistoryStore:StoreBase<ModuleMonitorOptionHistory>, IModuleMonitorOptionHistoryStore
    {
        public ModuleMonitorOptionHistoryStore(CustomDbContext context) :base(context)
        {

        }
    }
}
