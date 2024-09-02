using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CheckListTaskTemplatesManager : ManagerBase<CheckListTaskTemplates>, ICheckListTaskTemplates
    {
        public CheckListTaskTemplatesManager(ApplicationContext context) : base(context)
        {
            store = new CheckListTaskTemplatesStore(this.dbContext);
        }
    }
    public interface ICheckListTaskTemplates : IManagerBase<CheckListTaskTemplates>
    {
    }
}