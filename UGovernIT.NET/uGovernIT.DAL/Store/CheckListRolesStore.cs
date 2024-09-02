using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CheckListRolesStore : StoreBase<CheckListRoles>, ICheckListRolesStore
    {
        public CheckListRolesStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface ICheckListRolesStore : IStore<CheckListRoles>
    {
    }
}