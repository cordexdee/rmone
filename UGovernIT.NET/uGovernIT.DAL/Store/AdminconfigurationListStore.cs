using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class AdminconfigurationListStore:StoreBase<ClientAdminConfigurationList>, IAdminconfigurationListStore
    {
        public AdminconfigurationListStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IAdminconfigurationListStore : IStore<ClientAdminConfigurationList>
    {

    }
}
