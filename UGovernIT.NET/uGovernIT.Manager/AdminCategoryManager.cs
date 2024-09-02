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
    public class AdminCategoryManager:ManagerBase<ClientAdminCategory>,IAdminCategoryManager
    {
        public AdminCategoryManager(ApplicationContext context):base(context)
        {
            store = new AdminCategoryStore(this.dbContext);
        }
    }
    public interface IAdminCategoryManager : IManagerBase<ClientAdminCategory>
    {

    }
}
