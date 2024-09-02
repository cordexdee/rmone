using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using uGovernIT.DAL.Store;
using uGovernIT.Manager.Managers;
namespace uGovernIT.Manager
{
    public class AssetsManger:ManagerBase<Assests>, IAssetsManger
    {
        AssetIncidentRelationsManager assetIncidentRelationsMgr = null;
        ApplicationContext _context = null;
        public AssetsManger(ApplicationContext context):base(context)
        {
            _context = context;
            store = new AssestsStore(this.dbContext);
            assetIncidentRelationsMgr = new AssetIncidentRelationsManager(context);
        }
        public bool CreateRelationWithIncident(string assetId, string ticketId)
        {
            AssetIncidentRelations assetIncidentRelations = new AssetIncidentRelations();
            if (!assetIncidentRelationsMgr.IsRelationExist(assetId, ticketId))
            {
                string assetTicketID = GetAssetTicketId(assetId);
                if (!string.IsNullOrEmpty(assetTicketID))
                {
                    assetIncidentRelations.TicketId = ticketId;
                    assetIncidentRelations.ParentTicketId = assetTicketID;
                    assetIncidentRelations.AssetTagNumLookup = assetId;
                    assetIncidentRelationsMgr.Insert(assetIncidentRelations);
                    return true;
                }
            }
            
            return false;
        }
        public string assetName(string ids)
        {
            List<string> idss = UGITUtility.ConvertStringToList(ids,Constants.Separator6);
            string Name = "";
            List<Assests> assetList = new List<Assests>();
            idss.ForEach(x => assetList.Add(this.LoadByID(Convert.ToInt64(x))));
            if (assetList.Count > 0)
            {
                Name = string.Join<string>(Constants.Separator6, assetList.Select(x=>x.Title).ToArray());
            }
            else
            {
                Name = "";
            }
            return Name;
           
        }
        public string GetAssetTicketId(string assetId)
        {
            string ticketId = string.Empty;
            Assests listColl = LoadByID(UGITUtility.StringToLong(assetId));
            if (listColl != null)
                ticketId = Convert.ToString(listColl.TicketId);

            return ticketId;
        }

        public Assests GetAssetTicket(string assetTagOrId)
        {
            Assests item = null;
            List<Assests> listColl = Load(x => x.TenantID == _context.TenantID && (x.AssetTagNum == assetTagOrId || x.TicketId == assetTagOrId));
            if (listColl != null && listColl.Count > 0)
                item = listColl[0];

            return item;
        }

        public bool CreateAssetHistory(int assetId, string childTicketId)
        {
            //Get Asset/Ticket item
            DataRow[] dataRows= GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Assets, DatabaseObjects.Columns.ID, assetId);
            if (dataRows == null || dataRows.Length == 0)
                return false;
            DataRow asset = dataRows[0];
            string moduleName = uHelper.getModuleNameByTicketId(childTicketId);
            DataRow ticket= Ticket.GetCurrentTicket(_context, moduleName, childTicketId);
            return CreateAssetHistory(asset, ticket);
        }
        internal bool CreateAssetHistory(string assetId, DataRow ticket)
        {
            DataRow[] dataRows = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Assets, DatabaseObjects.Columns.ID, assetId);
            if (dataRows == null || dataRows.Length == 0)
                return false;
            DataRow asset = dataRows[0];
            return CreateAssetHistory(asset, ticket);
        }
        internal bool CreateAssetHistory(DataRow asset, DataRow ticket)
        {
            string ticketDescription = Constants.TicketRelationDescriptionCreate + " " + Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]);
            string assetDescription = Constants.AssetRelationDescriptionCreate + " " + Convert.ToString(asset[DatabaseObjects.Columns.TicketId]) + " " + "(" + Convert.ToString(asset[DatabaseObjects.Columns.AssetName]) + ")";
            uHelper.CreateHistory(_context.CurrentUser, ticketDescription, asset,_context);
            uHelper.CreateHistory(_context.CurrentUser, assetDescription, ticket, _context);
            return true;
        }

        public List<Assests> GetAssetTicket()
        {
            List<Assests> listColl = Load(x => x.TenantID == _context.TenantID);
            return listColl;
        }

    }
    public interface IAssetsManger : IManagerBase<Assests>
    {
    }
}
