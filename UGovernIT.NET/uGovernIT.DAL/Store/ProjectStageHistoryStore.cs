using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public interface IProjectStageHistoryStore : IStore<ProjectStageHistory>
    {
    }

    public class ProjectStageHistoryStore : StoreBase<ProjectStageHistory>, IProjectStageHistoryStore
    {
        public ProjectStageHistoryStore(CustomDbContext context) : base(context)
        {
        }
    }
}
