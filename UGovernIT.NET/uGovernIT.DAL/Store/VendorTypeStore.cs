using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class VendorTypeStore: StoreBase<VendorType>, IVendorTypeStore
    {
        public VendorTypeStore(CustomDbContext context) : base(context)
        {


        }
    }
    public interface IVendorTypeStore : IStore<VendorType>
    {

    }
}
