using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class AdminConfigurationListManager:ManagerBase<ClientAdminConfigurationList>, IAdminConfigurationListManager
    {
        public AdminConfigurationListManager(ApplicationContext context):base(context)
        {
            store = new AdminconfigurationListStore(this.dbContext);
        }
    }
    public interface IAdminConfigurationListManager : IManagerBase<ClientAdminConfigurationList>
    {

    }
}
