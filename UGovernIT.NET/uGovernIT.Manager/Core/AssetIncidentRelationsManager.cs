using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
namespace uGovernIT.Manager
{
    public class AssetIncidentRelationsManager : ManagerBase<AssetIncidentRelations>, IAssetIncidentRelationsManager
    {
        public AssetIncidentRelationsManager(ApplicationContext context) : base(context)
        {
            store = new AssetIncidentRelationsStore(this.dbContext);
        }

        public bool IsRelationExist(string assetId, string ticketId)
        {
            bool exist = false;
            List<AssetIncidentRelations> rcollection = this.Load(x =>x.TenantID==this.dbContext.TenantID && ((x.AssetTagNumLookup == assetId && x.TicketId == ticketId) || (x.AssetTagNumLookup == ticketId && x.TicketId == assetId)));
            if (rcollection != null && rcollection.Count > 0)
                exist = true;

            return exist;
        }
    }

    public interface IAssetIncidentRelationsManager : IManagerBase<AssetIncidentRelations>
    {

    }
}
