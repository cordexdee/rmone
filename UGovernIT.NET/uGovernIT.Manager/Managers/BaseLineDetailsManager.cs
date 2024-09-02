using uGovernIT.DAL;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class BaseLineDetailsManager:ManagerBase<BaseLineDetails>, IBaseLineDetailsManager
    {
        public BaseLineDetailsManager(ApplicationContext context) : base(context)
        {
            store = new BaseLineDetailsStore(this.dbContext);
        }
    }

    public interface IBaseLineDetailsManager : IManagerBase<BaseLineDetails>
    {

    }
}
