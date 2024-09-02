using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class UGITLogManager : ManagerBase<UGITLog>, IUGITLog
    {
        public UGITLogManager(ApplicationContext context) : base(context)
        {
            store = new UGITLogStore(this.dbContext);
        }
    }
    public interface IUGITLog : IManagerBase<UGITLog>
    {
    }
}
