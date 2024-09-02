using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CheckListRoleTemplatesStore : StoreBase<CheckListRoleTemplates>, ICheckListRoleTemplatesStore
    {
        public CheckListRoleTemplatesStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface ICheckListRoleTemplatesStore : IStore<CheckListRoleTemplates>
    {
    }
}
