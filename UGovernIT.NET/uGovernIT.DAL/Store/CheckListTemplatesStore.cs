using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class CheckListTemplatesStore : StoreBase<CheckListTemplates>, ICheckListTemplates
    {
        public CheckListTemplatesStore(CustomDbContext context) : base(context)
        {
        }
    }
    public interface ICheckListTemplates : IStore<CheckListTemplates>
    {
    }
}