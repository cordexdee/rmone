using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public interface IProjectMonitorStateHistoryStore : IStore<ProjectMonitorStateHistory>
    {

    }
    public class ProjectMonitorStateHistoryStore:StoreBase<ProjectMonitorStateHistory>, IProjectMonitorStateHistoryStore
    {
        public ProjectMonitorStateHistoryStore(CustomDbContext context) :base(context)

        {

        }
    }
}
