using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CheckListTaskTemplatesStore : StoreBase<CheckListTaskTemplates>, ICheckListTaskTemplates
    {
        public CheckListTaskTemplatesStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface ICheckListTaskTemplates : IStore<CheckListTaskTemplates>
    {
    }
}