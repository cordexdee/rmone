using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class UGITLogStore : StoreBase<UGITLog>, IUGITLogStore
    {
        public UGITLogStore(CustomDbContext context) : base(context)
        {
        }

    }
    public interface IUGITLogStore : IStore<UGITLog>
    {
    }
}