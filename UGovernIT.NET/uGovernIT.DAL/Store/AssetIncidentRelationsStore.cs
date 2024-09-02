using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class AssetIncidentRelationsStore: StoreBase<AssetIncidentRelations>, IAssetIncidentRelationsStore
    {
        public AssetIncidentRelationsStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IAssetIncidentRelationsStore : IStore<AssetIncidentRelations>
    {
    }
}
