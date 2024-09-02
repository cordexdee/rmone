using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;


namespace uGovernIT.Manager
{
    public class UpdateServicesMasterManager : ManagerBase<ServiceUpdates_Master>, IUpdateServicesMasterManager
    {
        public UpdateServicesMasterManager(ApplicationContext context) : base(context)
        {
            store = new UpdateServiceStore(this.dbContext);
        }
    }

    public interface IUpdateServicesMasterManager : IManagerBase<ServiceUpdates_Master>
    {

    }
}
