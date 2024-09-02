using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class CheckListTemplatesManager : ManagerBase<CheckListTemplates>, ICheckListTemplates
    {
        public CheckListTemplatesManager(ApplicationContext context) : base(context)
        {
            store = new CheckListTemplatesStore(this.dbContext);
        }
    }
    public interface ICheckListTemplates : IManagerBase<CheckListTemplates>
    {
    }
}