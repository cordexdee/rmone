using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class AssetVendorStore:StoreBase<AssetVendor>, IAssetVendorStore
    {
        public AssetVendorStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IAssetVendorStore : IStore<AssetVendor>
    {

    }
}
