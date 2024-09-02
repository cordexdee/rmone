using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CheckListTaskStatusStore : StoreBase<CheckListTaskStatus>, ICheckListTaskStatus
    {
        public CheckListTaskStatusStore(CustomDbContext context) : base(context)
        {
        }

    }
    public interface ICheckListTaskStatus : IStore<CheckListTaskStatus>
    {
    }
}