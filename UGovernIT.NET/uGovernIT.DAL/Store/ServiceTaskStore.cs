using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ServiceTaskStore : StoreBase<UGITTask>, IServiceTaskStore
    {
        public ServiceTaskStore(CustomDbContext context) : base(context)
        {
        }
    }
    interface IServiceTaskStore : IStore<UGITTask>
    {

    }
}
