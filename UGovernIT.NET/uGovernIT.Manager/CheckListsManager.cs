using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CheckListsManager : ManagerBase<CheckLists>, ICheckLists
    {
        public CheckListsManager(ApplicationContext context) : base(context)
        {
            store = new CheckListsStore(this.dbContext);
        }
    }
    public interface ICheckLists : IManagerBase<CheckLists>
    {
    }
}
