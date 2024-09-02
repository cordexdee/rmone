using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class BackgroundProcessStatusManager : ManagerBase<BackgroundProcessStatus>, IBackgroundProcessStatusManager
    {
        public BackgroundProcessStatusManager(ApplicationContext context) : base(context)
        {
            store = new BackgroundProcessStatusStore(this.dbContext);
        }

    }

    public interface IBackgroundProcessStatusManager : IManagerBase<BackgroundProcessStatus>
    {

    }

}
