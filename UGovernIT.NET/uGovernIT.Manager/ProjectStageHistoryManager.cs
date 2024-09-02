using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IProjectStageHistoryManager : IManagerBase<ProjectStageHistory>
    {
    }

    public class ProjectStageHistoryManager : ManagerBase<ProjectStageHistory>, IProjectStageHistoryManager
    {
        public ProjectStageHistoryManager(ApplicationContext context) : base(context)
        {
            store = new ProjectStageHistoryStore(this.dbContext);
        }        
    }
}
