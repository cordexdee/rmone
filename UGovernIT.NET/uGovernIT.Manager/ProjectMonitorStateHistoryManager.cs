using uGovernIT.DAL;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public interface IProjectMonitorStateHistoryManager:IManagerBase<ProjectMonitorStateHistory>
    {

    }

    public class ProjectMonitorStateHistoryManager:ManagerBase<ProjectMonitorStateHistory>, IProjectMonitorStateHistoryManager
    {
        public ProjectMonitorStateHistoryManager(ApplicationContext context):base(context)
        {
            store = new ProjectMonitorStateHistoryStore(this.dbContext);
        }
    }
}
