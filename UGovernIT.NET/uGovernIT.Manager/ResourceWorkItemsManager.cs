using DevExpress.XtraScheduler.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
    public class ResourceWorkItemsManager : ManagerBase<ResourceWorkItems>, IResourceWorkItemsManager
    {
        ResourceTimeSheetManager ObjResourceTimeSheetManager = null;
        ResourceAllocationManager ObjResourceAllocationManager = null;
        ApplicationContext _context = null;
        public ResourceWorkItemsManager(ApplicationContext context) : base(context)
        {
            store = new ResourceWorkItemsStore(this.dbContext);
            _context = context;

        }

        public ResourceWorkItems Save(ResourceWorkItems resourceWorkItems)
        {
            if (resourceWorkItems.ID > 0)
            {
                this.Update(resourceWorkItems);
                UpdateIntoCache(resourceWorkItems, false);
            }
            else
            {
                this.Insert(resourceWorkItems);
                UpdateIntoCache(resourceWorkItems, true);
            }
            return resourceWorkItems;
        }
        public override List<ResourceWorkItems> Load()
        {
            List<ResourceWorkItems> lstResourceWorkItems = new List<ResourceWorkItems>();
            string cacheName = DatabaseObjects.Tables.ResourceWorkItems + "_" + _context.TenantID;
            //lstResourceWorkItems = (List<ResourceWorkItems>)CacheHelper<object>.Get(cacheName, _context.TenantID);
            lstResourceWorkItems = (List<ResourceWorkItems>)CacheHelper<object>.Get(cacheName, _context.TenantID);

            if (lstResourceWorkItems == null || lstResourceWorkItems.Count == 0)
            {
                lstResourceWorkItems = store.Load();
                CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, lstResourceWorkItems);
                CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'"));

            }

            return lstResourceWorkItems;
        }
        public long SaveResourceWorkItems(ResourceWorkItems resourceWorkItems)
        {
            ObjResourceTimeSheetManager = new ResourceTimeSheetManager(_context);
            int message = 0;
            ResourceWorkItems wItem = null;
            if (resourceWorkItems.ID <= 0)
                wItem = LoadByWorkItem(resourceWorkItems.Resource.ToString(), resourceWorkItems.WorkItemType, resourceWorkItems.WorkItem, resourceWorkItems.SubWorkItem, resourceWorkItems.SubSubWorkItem, Convert.ToString(resourceWorkItems.StartDate), Convert.ToString(resourceWorkItems.EndDate));
            //When No Work Item exists in the WorkItem Table for the current Level1/2/3 + Resource combination

            if (wItem == null)
            {
                wItem = resourceWorkItems;
                this.Save(wItem);
                UpdateIntoCache(wItem, true);
                //Add an item to Resource Time sheet only if called from Actuals tab, else skip.
                if (wItem.StartDate != DateTime.MinValue)
                {
                    ResourceTimeSheet workSheetItem = new ResourceTimeSheet();
                    workSheetItem.WorkDate = wItem.StartDate;
                    workSheetItem.Resource = wItem.Resource;
                    workSheetItem.ResourceWorkItemLookup = wItem.ID;
                    workSheetItem.HoursTaken = 0;
                    ObjResourceTimeSheetManager.Save(workSheetItem);
                }
                message = 1;
            }
            //When we have an entry in the WorkItem list (With no timesheet entry)
            //When we want to delete a work item (we actually delete the time sheet entry)
            else
            {
                ResourceWorkItems item = this.LoadByID(wItem.ID);
                //string endDate = wItem.StartDate.AddDays(6).ToString("yyyy-MM-dd");
                //if (wItem.EndDate != DateTime.MinValue)
                //{
                //    endDate = wItem.EndDate.ToString("yyyy-MM-dd");
                //}
                List<string> requiredQuery = new List<string>();
                List<ResourceTimeSheet> resourceTimeSheetItems = ObjResourceTimeSheetManager.Load().Where(x => x.ResourceWorkItemLookup == wItem.ID && x.WorkDate >= wItem.StartDate && x.WorkDate <= wItem.EndDate).ToList();//
                message = 2;


                if (wItem.StartDate != DateTime.MinValue && resourceTimeSheetItems.Count() == 0)
                {
                    ObjResourceAllocationManager = new ResourceAllocationManager(_context);
                    //Check for an existing entry in Resource Allocation for the current week
                    bool resourceAllocationExists = ObjResourceAllocationManager.ResourceAllocationExistsOverlapping(wItem.ID, wItem.StartDate ?? DateTime.MinValue, wItem.StartDate ?? DateTime.MinValue.AddDays(6));
                    if (!resourceAllocationExists)
                    {
                        List<ResourceTimeSheet> workSheetList = ObjResourceTimeSheetManager.Load();// SPListHelper.GetSPList(DatabaseObjects.Lists.ResourceTimeSheet, spWeb);
                        ResourceTimeSheet workSheetItem = new ResourceTimeSheet();
                        workSheetItem.Title = string.Format("{0};#{1};#{2}", wItem.WorkItemType, wItem.WorkItem, wItem.SubWorkItem);
                        workSheetItem.WorkDate = wItem.StartDate;
                        workSheetItem.Resource = wItem.Resource;
                        workSheetItem.ResourceWorkItemLookup = wItem.ID;
                        workSheetItem.HoursTaken = 0;
                        ObjResourceTimeSheetManager.Save(workSheetItem);
                        message = 1;
                    }
                }
            }
            return message;
        }
        /// <summary>
        /// Load workitem by user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="status">0 for all workitem, 1 for workitem which is visible in time sheet, 3 for workitem not visible in time sheet</param>
        /// <returns></returns>
        public List<ResourceWorkItems> LoadByUser(string user, int status)
        {
            List<ResourceWorkItems> rWorkItems = new List<ResourceWorkItems>();
            int userId = -1;
            try
            {
                userId = int.Parse(user);
            }
            catch { userId = -1; }

            List<string> requiredQuery = new List<string>();
            if (userId > 0)
            {
                requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
            }
            else
            {
                requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
            }
            requiredQuery.Add(string.Format("{0}=1", DatabaseObjects.Columns.Deleted));

            if (status == 1)
            {
                requiredQuery.Add(string.Format("{0}=1", DatabaseObjects.Columns.Deleted));
            }
            else if (status == 2)
            {
                requiredQuery.Add(string.Format("{0}=1", DatabaseObjects.Columns.Deleted));
            }

            string rQuery = string.Join(" and", requiredQuery);
            DataTable workItemList = this.GetDataTable();// SPListHelper.GetSPList(DatabaseObjects.Tables.ResourceWorkItems);
            if (workItemList.Rows.Count > 0)
            {
                DataRow[] resultCollection = workItemList.Select(rQuery);
                if (resultCollection != null && resultCollection.Count() > 0)
                {
                    foreach (DataRow resultedItem in resultCollection)
                    {
                        ResourceWorkItems wItem = this.LoadResourceWorkItem(resultedItem);
                        rWorkItems.Add(wItem);
                    }
                }
            }
            return rWorkItems;
        }

        public ResourceWorkItems LoadByWorkItem(string user, string workItemTypeId, string workItemId, string subWorkItemId, string subSubWorkItemId = "", string StartDate = "", string EndDate = "")
        {
            ResourceWorkItems wItem = null;

            //This query will check for weather a workitem already exists or not
            List<string> workItemExistsQuery = new List<string>();
            if (!string.IsNullOrEmpty(user))
            {
                workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
            }
            workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItemType, workItemTypeId));
            if (string.IsNullOrEmpty(workItemId))
            {
                workItemExistsQuery.Add(string.Format("{0} is null", DatabaseObjects.Columns.WorkItem));
            }
            else
            {
                workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItem, workItemId));
            }
            if (!string.IsNullOrEmpty(subWorkItemId.Replace(Constants.Separator, string.Empty)))
            {
                workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.SubWorkItem, subWorkItemId));
            }
            else
            {
                if (workItemTypeId != Constants.RMMLevel1PMMProjectsType && workItemTypeId != Constants.RMMLevel1TSKProjectsType && !string.IsNullOrEmpty(subWorkItemId))
                    workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.SubWorkItem, subWorkItemId));

            }

            if (!string.IsNullOrEmpty(subSubWorkItemId))
            {
                workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.SubSubWorkItem, subSubWorkItemId));
            }

            if (!string.IsNullOrEmpty(StartDate))
            {
                workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.StartDate, StartDate.ToDateTime().ToShortDateString()));
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                workItemExistsQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.EndDate, EndDate.ToDateTime().ToShortDateString()));
            }

            string rQuery = string.Join(" and ", workItemExistsQuery);
            DataTable workItemList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}' and {DatabaseObjects.Columns.Deleted} = 0");
            if (workItemList.Rows.Count > 0)
            {
                DataRow[] resultCollection = workItemList.Select(rQuery);
                bool workItemNameOrIdDuplicate = false;
                int currentCollectionRow = 0;
                if (workItemTypeId == Constants.RMMLevel1PMMProjectsType || workItemTypeId == Constants.RMMLevel1TSKProjectsType)
                {
                    string subworkItemId = UGITUtility.SplitString(subWorkItemId, Constants.Separator, 0);
                    string subworkItemName = UGITUtility.SplitString(subWorkItemId, Constants.Separator, 1);
                    foreach (DataRow result in resultCollection)
                    {
                        string[] resultedWorkItem = UGITUtility.SplitString(Convert.ToString(result[DatabaseObjects.Columns.SubWorkItem]), Constants.Separator);
                        if ((resultedWorkItem.Count() == 2 && (resultedWorkItem[0] == subworkItemId)) //|| resultedWorkItem[1] == subworkItemName
                            || (resultedWorkItem.Count() == 1 && resultedWorkItem[0] == subworkItemId)
                            || (resultedWorkItem.Count() == 0 && subworkItemId == null))
                        {
                            workItemNameOrIdDuplicate = true;
                            break;
                        }
                        currentCollectionRow++;
                    }
                }
                if (resultCollection != null && resultCollection.Count() > 0
                    && (workItemNameOrIdDuplicate || (workItemTypeId != Constants.RMMLevel1PMMProjectsType && workItemTypeId != Constants.RMMLevel1TSKProjectsType)))
                {
                    if (workItemNameOrIdDuplicate)
                        wItem = LoadResourceWorkItem(resultCollection[currentCollectionRow]);
                    else
                        wItem = LoadResourceWorkItem(resultCollection[0]);
                }
            }
            return wItem;
        }

        public List<ResourceWorkItems> LoadWorkItemsById(string workItemTypeId, string workItemId, string subWorkItemId)
        {
            ////List<ResourceWorkItems> itemList = base.Load(x=>x.ID==Convert.ToInt64(workItemId) && x.WorkItemType==workItemTypeId && x.WorkItem==workItemId && x.SubWorkItem==subWorkItemId);

            ////List<ResourceWorkItems> itemList = base.Load(x => x.WorkItemType == workItemTypeId && x.WorkItem == workItemId && x.SubWorkItem == subWorkItemId);
            List<ResourceWorkItems> itemList = base.Load(x => x.WorkItemType == workItemTypeId && x.WorkItem == workItemId);

            return itemList;
        }

        public ResourceWorkItems LoadWorkItemCombinations(string resourceuser, string workItemType, string workItem, string subWorkItem)
        {
            ResourceWorkItems rItem = base.Load(x => x.Resource==resourceuser && x.WorkItemType == workItemType && x.WorkItem == workItem && x.SubWorkItem == subWorkItem)
                .OrderBy(x => x.Created).LastOrDefault();
            if (rItem == null)
            {
                rItem = new ResourceWorkItems();
                rItem.Resource = resourceuser;
                rItem.WorkItemType = workItemType;
                rItem.WorkItem = workItem;
                rItem.SubWorkItem = subWorkItem;
                rItem.Deleted = false;
            }
            this.Save(rItem);
            return rItem;
        }

        /// <summary>
        /// Load Work Items with the help of workitemtype, workitem and subworkitem, You may ignore workitem or subworkitem to query
        /// </summary>
        /// <param name="workItemTypeId"></param>
        /// <param name="workItemId"></param>
        /// <param name="subWorkItemId"></param>
        /// <param name="ignoreWorkItemId"></param>
        /// <param name="ignoreSubWorkItemId"></param>
        /// <returns></returns>
        public List<ResourceWorkItems> LoadWorkItemsById(string workItemTypeId, string workItemId, string subWorkItemId, bool ignoreWorkItemId, Boolean ignoreSubWorkItemId)
        {
            List<ResourceWorkItems> rWorkItems = new List<ResourceWorkItems>();
            List<string> requiredQuery = new List<string>();
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItemType, workItemTypeId));
            if (!ignoreWorkItemId)
            {
                if (string.IsNullOrEmpty(workItemId))
                {
                    requiredQuery.Add(string.Format("({0} is  null or {0} = '')", DatabaseObjects.Columns.WorkItem));
                }
                else
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItem, workItemId));
                }
            }

            if (!ignoreSubWorkItemId)
            {
                if (string.IsNullOrEmpty(subWorkItemId))
                {
                    requiredQuery.Add(string.Format("({0} is  null or {0} = '')", DatabaseObjects.Columns.SubWorkItem));
                }
                else
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.SubWorkItem, subWorkItemId));
                }
            }

            string rQuery = string.Join(" and ", requiredQuery);
            DataTable workItemList = this.GetDataTable(rQuery);
            if (workItemList.Rows.Count > 0)
            {
                DataRow[] resultCollection = workItemList.Select();
                if (resultCollection != null && resultCollection.Count() > 0)
                {
                    foreach (DataRow resultedItem in resultCollection)
                    {
                        ResourceWorkItems wItem = this.LoadResourceWorkItem(resultedItem);
                        if (wItem != null)
                            rWorkItems.Add(wItem);
                    }
                }
            }
            return rWorkItems;
        }


        public ResourceWorkItems LoadResourceWorkItem(DataRow item)
        {
            ResourceWorkItems wItem = null;
            try
            {
                wItem = this.Get(x => x.Resource == UGITUtility.SplitString(item[DatabaseObjects.Columns.Resource], Constants.Separator6, 0) && x.ID == UGITUtility.StringToLong(item[DatabaseObjects.Columns.ID]));
                if (wItem == null)
                {
                    return wItem;
                }
                
                wItem.ID = UGITUtility.StringToInt(item[DatabaseObjects.Columns.Id]);
                wItem.Deleted = UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.Deleted]);
                string workItemLevel1 = Convert.ToString(item[DatabaseObjects.Columns.WorkItemType]);
                wItem.WorkItemType = workItemLevel1;

                if (workItemLevel1 == Constants.RMMLevel1PMMProjectsType)
                    wItem.WorkItemType = this.dbContext.ConfigManager.GetValue("RMMTypeActiveProjects");
                else if (workItemLevel1 == Constants.RMMLevel1TSKProjectsType)
                    wItem.WorkItem = this.dbContext.ConfigManager.GetValue("RMMTypeTasklists");
                else if (workItemLevel1 == Constants.RMMLevel1NPRProjectsType)
                    wItem.SubWorkItem = this.dbContext.ConfigManager.GetValue("RMMTypeProjectsPendingApproval");
                else
                {
                    DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.ModuleName} = '{workItemLevel1}' and {DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'", $"{DatabaseObjects.Columns.ModuleTicketTable}", null);
                    if (dt != null && dt.Rows.Count > 0)
                        workItemLevel1 = Convert.ToString(dt.Rows[0][0]);
                }
                //BTS-22-000970: This line gives error in case of workItemLevel1 = "Time Off" so skip it if Ticket Id is not valid.
                if (UGITUtility.IsValidTicketID(Convert.ToString(item[DatabaseObjects.Columns.WorkItem])))
                { 
                    DataTable dtWorkItem = GetTableDataManager.GetTableData(workItemLevel1, $"{DatabaseObjects.Columns.TicketId} = '{Convert.ToString(item[DatabaseObjects.Columns.WorkItem])}'");
                    if (dtWorkItem != null && dtWorkItem.Rows.Count>0)
                        wItem.WorkItemType =Convert.ToString(dtWorkItem.Rows[0][DatabaseObjects.Columns.Title]);
                }
                wItem.WorkItem = Convert.ToString(item[DatabaseObjects.Columns.WorkItem]);
                wItem.SubWorkItem = Convert.ToString(item[DatabaseObjects.Columns.SubWorkItem]);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex, "Exception in LoadResourceWorkItem()");
            }
            return wItem;
        }

        public DataTable LoadRawTableByResource(string userID, int status)
        {
            List<string> usersID = new List<string>();
            usersID.Add(userID);
            return LoadRawTableByResource(usersID, status);
        }
        public DataTable LoadRawTableByResource(List<string> usersID, int status)
        {
            DataTable resultTable = null;

            List<ResourceWorkItems> rWorkItems = new List<ResourceWorkItems>();

            List<string> requiredQuery = new List<string>();
            if (usersID.Count > 0)
            {
                List<string> userExps = new List<string>();
                foreach (string userid in usersID)
                {
                    userExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, userid));
                }
                requiredQuery.Add($"({string.Join(" or ", userExps)})");
            }

            if (status == 1)
            {
                requiredQuery.Add(string.Format("{0} <> 'True'", DatabaseObjects.Columns.Deleted));
            }
            else if (status == 2)
            {
                requiredQuery.Add(string.Format("{0} = 'True'", DatabaseObjects.Columns.Deleted));
            }

            requiredQuery.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TenantID, this._context.TenantID));
            string rQuery = string.Join("  and ", requiredQuery);
            //resultTable = this.GetDataTable(rQuery);

            string cacheName = DatabaseObjects.Tables.ResourceWorkItems + "_" + _context.TenantID;

            resultTable = (DataTable)CacheHelper<object>.Get($"dt_{cacheName}", _context.TenantID);
            if (resultTable == null)
                resultTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, rQuery);

            DataView view = resultTable.AsDataView();
            view.RowFilter = rQuery;
            resultTable = view.ToTable();
            
            return resultTable;

        }

        public List<ResourceWorkItems> LoadRawTableByResourceList(List<string> usersID, int status)
        {
            List<ResourceWorkItems> rWorkItems = new List<ResourceWorkItems>();

            List<string> requiredQuery = new List<string>();
            if (usersID.Count > 0)
            {
                List<string> userExps = new List<string>();
                foreach (string userid in usersID)
                {
                    userExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, userid));
                }
                requiredQuery.Add(string.Join(" or ", userExps));
            }

            if (status == 1)
            {
                requiredQuery.Add(string.Format("{0} <> 1", DatabaseObjects.Columns.Deleted));
            }
            else if (status == 2)
            {
                requiredQuery.Add(string.Format("{0} = 1", DatabaseObjects.Columns.Deleted));
            }

            requiredQuery.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TenantID, this._context.TenantID));
            string rQuery = string.Join("  and ", requiredQuery);
            rWorkItems = this.Load(rQuery);
            return rWorkItems;
        }
        public static string FormatWorkItemType(string type)
        {
            string level1 = string.Empty;
            level1 = uHelper.GetModuleTitle(type);
            if (string.IsNullOrEmpty(level1))
                level1 = type;

            return level1;
        }

        public bool IsWorkItemTypeIsModule(string title)
        {
            ModuleViewManager moduleManager = new ModuleViewManager(_context);
            UGITModule module = moduleManager.Get(x => x.Title.ToLower() == title.ToLower());
            if (module != null)
                return true;

            return false;
        }
        public void RefreshCache()
        {
            Load();
        }
        public void UpdateIntoCache(ResourceWorkItems resourceWorkItems, bool flag, bool fromActualTime = false)
        {
            List<ResourceWorkItems> lstResourceWorkItems = new List<ResourceWorkItems>();
            string cacheName = DatabaseObjects.Tables.ResourceWorkItems + "_" + _context.TenantID;
            lstResourceWorkItems = (List<ResourceWorkItems>)CacheHelper<object>.Get(cacheName, _context.TenantID);
            if (lstResourceWorkItems == null)
            {
                ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
                lstResourceWorkItems = resourceWorkItemsManager.Load();
            }
           
            if(!flag)
            {
                var cacheitem = lstResourceWorkItems.FirstOrDefault(x => x.ID == resourceWorkItems.ID);
                cacheitem = resourceWorkItems;
                CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, lstResourceWorkItems);
                string requiredQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TenantID, this._context.TenantID);
                CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, requiredQuery));
            }
            else if (flag && fromActualTime)
            {
                ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
                lstResourceWorkItems = store.Load();
                CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, lstResourceWorkItems);
                string requiredQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TenantID, this._context.TenantID);
                CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, requiredQuery));
            }
            else
            {
                lstResourceWorkItems.Add(resourceWorkItems);
                CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, lstResourceWorkItems);
                string requiredQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TenantID, this._context.TenantID);
                CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, requiredQuery));
            }
        }
    }
    public interface IResourceWorkItemsManager : IManagerBase<ResourceWorkItems>
    {

    }
}
