using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ServiceCategoryStore : StoreBase<ServiceCategory>, IServiceCategoryStore
    {
        public ServiceCategoryStore(CustomDbContext context) : base(context)
        {
        }
    }
    interface IServiceCategoryStore: IStore<ServiceCategory>
    {

    }
}
