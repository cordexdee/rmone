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
    public class OrganizationManager:ManagerBase<Organization>, IOrganizationManager
    {
        public OrganizationManager(ApplicationContext context):base(context)
        {
            store = new OrganizationStore(this.dbContext);
        }
    }
    public interface IOrganizationManager : IManagerBase<Organization>
    {

    }
}
