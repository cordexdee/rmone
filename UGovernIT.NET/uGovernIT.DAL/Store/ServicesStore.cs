using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ServicesStore : StoreBase<Services>, IServicesStore
    {
        public ServicesStore(CustomDbContext context) : base(context)
        {

        }
    }
    interface IServicesStore : IStore<Services>
    {

    }
}
