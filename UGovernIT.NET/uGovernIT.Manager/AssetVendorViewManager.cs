using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class AssetVendorViewManager:ManagerBase<AssetVendor>, IAssetVendors
    {
        public AssetVendorViewManager(ApplicationContext context):base(context)
        {
            store = new AssetVendorStore(this.dbContext);
        }
    }
    public interface IAssetVendors : IManagerBase<AssetVendor>
    {

    }
}
