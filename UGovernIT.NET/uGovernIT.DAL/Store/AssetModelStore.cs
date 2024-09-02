using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class AssetModelStore:StoreBase<AssetModel>, IAssetModelStore
    {
        public AssetModelStore(CustomDbContext context):base(context)
        {


        }
    }
    public interface IAssetModelStore : IStore<AssetModel>
    {

    }
}
