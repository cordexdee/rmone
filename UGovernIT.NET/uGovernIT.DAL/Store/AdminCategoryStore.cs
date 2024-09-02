using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class AdminCategoryStore:StoreBase<ClientAdminCategory>, IAdminCategoryStore
    {
        public AdminCategoryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IAdminCategoryStore:IStore<ClientAdminCategory>
    {

    }
}
