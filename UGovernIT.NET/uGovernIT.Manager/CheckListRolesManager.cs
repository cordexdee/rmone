using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CheckListRolesManager : ManagerBase<CheckListRoles>, ICheckListRoles
    {
        public CheckListRolesManager(ApplicationContext context) : base(context)
        {
            store = new CheckListRolesStore(this.dbContext);
        }
    }
    public interface ICheckListRoles : IManagerBase<CheckListRoles>
    {
    }
}