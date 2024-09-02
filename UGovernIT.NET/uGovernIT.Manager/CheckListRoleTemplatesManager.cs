using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CheckListRoleTemplatesManager : ManagerBase<CheckListRoleTemplates>, ICheckListRoleTemplates
    {
        public CheckListRoleTemplatesManager(ApplicationContext context) : base(context)
        {
            store = new CheckListRoleTemplatesStore(this.dbContext);
        }
    }
    public interface ICheckListRoleTemplates : IManagerBase<CheckListRoleTemplates>
    {
    }
}