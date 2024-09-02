using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig
{
    public interface IAssetModule : IModule
    {
        List<AssetVendor> GetAssetVendors();
        List<AssetModel> GetAssetModels();
    }
}
