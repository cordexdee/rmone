using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class OrganizationStore : StoreBase<Organization>, IOrganizationStore
    {
        public OrganizationStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IOrganizationStore : IStore<Organization>
    {
    }
}
