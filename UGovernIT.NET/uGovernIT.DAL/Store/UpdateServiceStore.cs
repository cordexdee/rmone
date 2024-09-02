using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
     public class UpdateServiceStore : StoreBase<ServiceUpdates_Master>, IUpdateServiceStore
     {
        public UpdateServiceStore(CustomDbContext context) : base(context)
        {

        }
     }
    public interface IUpdateServiceStore : IStore<ServiceUpdates_Master>
    {

    }
}
