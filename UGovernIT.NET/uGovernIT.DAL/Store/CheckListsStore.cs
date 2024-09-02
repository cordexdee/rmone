using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CheckListsStore : StoreBase<CheckLists>, ICheckListsStore
    {
        public CheckListsStore(CustomDbContext context) : base(context)
        {
        }

    }
    public interface ICheckListsStore : IStore<CheckLists>
    {
    }
}