using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CheckListTasksStore : StoreBase<CheckListTasks>, ICheckListTasks
    {
        public CheckListTasksStore(CustomDbContext context) : base(context)
        {
        }

    }
    public interface ICheckListTasks : IStore<CheckListTasks>
    {
    }
}