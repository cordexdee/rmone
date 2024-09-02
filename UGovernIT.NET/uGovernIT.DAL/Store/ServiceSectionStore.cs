using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ServiceSectionStore : StoreBase<ServiceSection>, IServiceSectionStore
    {
        public ServiceSectionStore(CustomDbContext context) : base(context)
        {
        }
    }
    interface IServiceSectionStore : IStore<ServiceSection>
    {

    }
}
