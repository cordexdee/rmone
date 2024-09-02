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
   public class AssetModelViewManager:ManagerBase<AssetModel>, IAssetModelViewManager
   {
        public AssetModelViewManager(ApplicationContext context):base(context)
        {
            store = new AssetModelStore(this.dbContext);
        }

    }
    public interface IAssetModelViewManager : IManagerBase<AssetModel>
    {

    }
}
