using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
   public class TenantStore: StoreBase<Tenant>, ITenantStore
    {
        public TenantStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface ITenantStore : IStore<Tenant>
    {

    }
}
