using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CheckListTasksManager : ManagerBase<CheckListTasks>, ICheckListTasks
    {
        public CheckListTasksManager(ApplicationContext context) : base(context)
        {
            store = new CheckListTasksStore(this.dbContext);
        }
    }
    public interface ICheckListTasks : IManagerBase<CheckListTasks>
    {
    }
}