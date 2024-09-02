using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CheckListTaskStatusManager : ManagerBase<CheckListTaskStatus>, ICheckListTaskStatus
    {
        public CheckListTaskStatusManager(ApplicationContext context) : base(context)
        {
            store = new CheckListTaskStatusStore(this.dbContext);
        }
    }
    public interface ICheckListTaskStatus : IManagerBase<CheckListTaskStatus>
    {
    }
}