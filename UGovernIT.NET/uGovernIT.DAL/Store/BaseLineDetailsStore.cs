using uGovernIT.Utility;

namespace uGovernIT.DAL
{
    public class BaseLineDetailsStore:StoreBase<BaseLineDetails>, IBaseLineDetailsStore
    {
        public BaseLineDetailsStore(CustomDbContext context) : base(context)
        {
           
        }

    }
    public interface IBaseLineDetailsStore : IStore<BaseLineDetails>
    {

    }
}
