using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public  class LocationStore: StoreBase<Location>, ILocationStore
    {
        public LocationStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ILocationStore : IStore<Location>
    {

    }
}
