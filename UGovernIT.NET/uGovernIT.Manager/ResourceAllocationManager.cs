using DevExpress.CodeParser;
using DevExpress.Office.Utils;
using DevExpress.Utils.Extensions;
using MailKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Util;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Helpers;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager.RMM;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Helpers;

namespace uGovernIT.Manager
{
    public class ResourceAllocationManager : ManagerBase<RResourceAllocation>, IResourceAllocationManager
    {
        ResourceWorkItemsManager workitemManager = null;
        ApplicationContext _context = null;
        ResourceAllocationMonthlyManager ObjResourceAllocationMonthlyManager = null;
        ConfigurationVariableManager ObjConfigurationVariableManager = null;
        private static bool IsProcessActive = false;
        private static bool IsSummaryComplete = false;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public ResourceAllocationManager(ApplicationContext context) : base(context)
        {

            store = new ResourceAllocationStore(this.dbContext);
            _context = context;
            workitemManager = new ResourceWorkItemsManager(_context);
            ObjResourceAllocationMonthlyManager = new ResourceAllocationMonthlyManager(_context);
            ObjConfigurationVariableManager = new ConfigurationVariableManager(_context);
        }

        public bool ProcessState()
        {
            return IsProcessActive;
        }
        public void RefreshCache()
        {
            Load();
        }
        public bool SummaryComplete()
        {
            return IsSummaryComplete;
        }
        public override List<RResourceAllocation> Load()
        {
            string cacheName = DatabaseObjects.Tables.ResourceAllocation + "_" + _context.TenantID;
            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable dt = new DataTable();
            List<RResourceAllocation> lstResourceAllocation = new List<RResourceAllocation>();
            ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
            dt = (DataTable)CacheHelper<object>.Get($"dt_{cacheName}", _context.TenantID);
            lstResourceAllocation = (List<RResourceAllocation>)CacheHelper<object>.Get(cacheName, _context.TenantID);
            if (dt == null || lstResourceAllocation == null)
            {
                values.Add("@TenantID", _context.TenantID);
                dt = GetTableDataManager.GetData("ResourceAllocation2", values);
                CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dt);

                lstResourceAllocation = base.Load();

                List<ResourceWorkItems> lstAllWorkitems = resourceWorkItemsManager.Load();

                if (lstResourceAllocation != null)
                {
                    foreach (RResourceAllocation item in lstResourceAllocation)
                    {
                        List<ResourceWorkItems> resourceWorkItemLst = lstAllWorkitems.Where(y => y.ID == item.ResourceWorkItemLookup).ToList();
                        if (resourceWorkItemLst != null && resourceWorkItemLst.Count > 0)
                            item.ResourceWorkItems = resourceWorkItemLst[0];
                    }
                    CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, lstResourceAllocation);
                }
            }
            return lstResourceAllocation;
        }
        public string Save(RResourceAllocation resourceAllocation, bool isPlanned = false, bool deleteDuplicateAllocation = false)
        {
            try
            {
                _semaphore.WaitAsync();
                {
                    if (!resourceAllocation.AllocationStartDate.HasValue || resourceAllocation.AllocationStartDate == DateTime.MinValue || resourceAllocation.AllocationStartDate == DateTime.MaxValue)
                        return "Invalid Allocation Start Date";
                    if (!resourceAllocation.AllocationEndDate.HasValue || resourceAllocation.AllocationEndDate == DateTime.MinValue || resourceAllocation.AllocationEndDate == DateTime.MaxValue)
                        return "Invalid Allocation End Date";

                    DateTime updatedRowTime = DateTime.Now;
                    if (resourceAllocation.ResourceWorkItems != null && resourceAllocation.ResourceWorkItems.WorkItemType != string.Empty
                        && (resourceAllocation.ResourceWorkItems.WorkItem != string.Empty || resourceAllocation.ResourceWorkItems.SubWorkItem != string.Empty))
                    {
                        ResourceWorkItems rWorkItem = workitemManager.LoadWorkItemCombinations(resourceAllocation.Resource,
                            resourceAllocation.ResourceWorkItems.WorkItemType, resourceAllocation.ResourceWorkItems.WorkItem, resourceAllocation.ResourceWorkItems.SubWorkItem);

                        if (resourceAllocation?.ResourceWorkItems?.ID > 0 && resourceAllocation?.ResourceWorkItems?.ID != rWorkItem?.ID)
                        {
                            RMMSummaryHelper.DeleteMonthlyDistributions(_context, resourceAllocation.ResourceWorkItems.ID, resourceAllocation.ID);
                            RMMSummaryHelper.DeleteMonthlyAndWeeklySummaryDistributions(_context, resourceAllocation.ResourceWorkItems.ID, resourceAllocation.ID);
                        }

                        resourceAllocation.ResourceWorkItemLookup = rWorkItem.ID;
                        resourceAllocation.ResourceWorkItems.ID = rWorkItem.ID;
                        resourceAllocation.ResourceWorkItems.Resource = rWorkItem.Resource;

                        List<RResourceAllocation> resourceAllocations = this.Load(x => x.ResourceWorkItemLookup == resourceAllocation.ResourceWorkItemLookup
                                                                                        && x.ID != resourceAllocation.ID).ToList();
                        if (resourceAllocations.Count > 0 && !isPlanned)
                        {
                            if (deleteDuplicateAllocation)
                            {
                                //while (resourceAllocations.Count > 0)
                                //{
                                //    this.Delete(resourceAllocations[0]);
                                //    resourceAllocations.RemoveAt(0);
                                //}
                            }
                            else
                            {
                                // Check to make sure the new (or edited) allocation doesn't have a overlapping date range with any other entry
                                foreach (RResourceAllocation item in resourceAllocations)
                                {
                                    DateTime allocationStartDate = (DateTime)item.AllocationStartDate;
                                    DateTime allocationEndDate = (DateTime)item.AllocationEndDate;

                                    //Overlaps between 2 AllocationRanges
                                    //(StartA <= EndB) And (EndA >= StartB)
                                    //If any issue check startdate and EndDate
                                    if (resourceAllocation.Resource != "00000000-0000-0000-0000-000000000000" && resourceAllocation.AllocationStartDate <= allocationEndDate && resourceAllocation.AllocationEndDate >= allocationStartDate)
                                    {
                                        //Check if the previous duplicate entry was deleted, if yes un-delete it so that user can edit.
                                        if (UGITUtility.StringToBoolean(item.Deleted))
                                        {
                                            resourceAllocation.ID = item.ID;
                                            resourceAllocation.Deleted = false;
                                            break;
                                        }

                                        // Found overlapping date range for same work item, return error without saving allocation
                                        return Constants.ErrorMsgRMMOverlappingDates;
                                    }
                                }
                            }
                        }

                        if (isPlanned)
                        {
                            resourceAllocation.PctPlannedAllocation = resourceAllocation.PctPlannedAllocation;
                            if (resourceAllocation.PlannedStartDate == DateTime.MinValue)
                                resourceAllocation.PlannedStartDate = null;
                            if (resourceAllocation.PlannedEndDate == DateTime.MinValue)
                                resourceAllocation.PlannedEndDate = null;
                            //if (resourceAllocation.PlannedStartDate != DateTime.MinValue)
                            //    resourceAllocation.PlannedStartDate = resourceAllocation.PlannedStartDate;
                            //if (resourceAllocation.PlannedEndDate != DateTime.MinValue)
                            //    resourceAllocation.PlannedEndDate = resourceAllocation.PlannedEndDate;
                            if (string.IsNullOrWhiteSpace(Convert.ToString(resourceAllocation.EstStartDate)))
                            {
                                if (resourceAllocation.PlannedStartDate != DateTime.MinValue)
                                    resourceAllocation.AllocationStartDate = resourceAllocation.PlannedStartDate;
                                if (resourceAllocation.PlannedEndDate != DateTime.MinValue)
                                    resourceAllocation.AllocationEndDate = resourceAllocation.PlannedEndDate;
                            }
                            else
                            {
                                resourceAllocation.PctAllocation = resourceAllocation.PctEstimatedAllocation;
                                if (resourceAllocation.EstStartDate != DateTime.MinValue)
                                    resourceAllocation.AllocationStartDate = resourceAllocation.EstStartDate;
                                if (resourceAllocation.EstEndDate != DateTime.MinValue)
                                    resourceAllocation.AllocationEndDate = resourceAllocation.EstEndDate;
                            }
                        }
                        else
                        {
                            if (resourceAllocation.PctAllocation == 0)
                            {
                                resourceAllocation.EstStartDate = null;
                                resourceAllocation.EstEndDate = null;
                            }
                            else
                            {
                                resourceAllocation.EstStartDate = resourceAllocation.AllocationStartDate;
                                resourceAllocation.EstEndDate = resourceAllocation.AllocationEndDate;
                            }
                            resourceAllocation.EstStartDate = resourceAllocation.AllocationStartDate;
                            resourceAllocation.EstEndDate = resourceAllocation.AllocationEndDate;
                            resourceAllocation.PctEstimatedAllocation = resourceAllocation.PctAllocation;
                        }

                        //Set Normal allocation dates as whatever is minimum start date or maximum end date
                        if (resourceAllocation.EstStartDate != null && resourceAllocation.PlannedStartDate != null && resourceAllocation.EstStartDate != DateTime.MinValue && resourceAllocation.PlannedStartDate != DateTime.MinValue)
                            resourceAllocation.AllocationStartDate = resourceAllocation.EstStartDate > resourceAllocation.PlannedStartDate ? resourceAllocation.PlannedStartDate : resourceAllocation.EstStartDate;
                        if (resourceAllocation.PlannedEndDate != null && resourceAllocation.EstEndDate != null && resourceAllocation.PlannedEndDate != DateTime.MinValue && resourceAllocation.EstEndDate != DateTime.MinValue)
                            resourceAllocation.AllocationEndDate = resourceAllocation.EstEndDate < resourceAllocation.PlannedEndDate ? resourceAllocation.PlannedEndDate : resourceAllocation.EstEndDate;


                        //// No issues so far, go ahead and save

                        if (resourceAllocation.ID > 0)
                        {
                            try
                            {
                                RResourceAllocation ifObjPersists = this.LoadByID(resourceAllocation.ID);
                                if (ifObjPersists != null)
                                    this.Update(resourceAllocation);
                            }
                            catch (Exception e)
                            {
                                ULog.WriteException("Problem while Update:" + resourceAllocation.TicketID + e.ToString());
                            }

                        }
                        else
                        {
                            try
                            {
                                this.Insert(resourceAllocation);
                            }
                            catch (Exception e)
                            {
                                ULog.WriteException("Problem while Insert :" + resourceAllocation.TicketID + e.ToString());
                            }
                        }


                        try
                        {
                            UpdateIntoCache(resourceAllocation, true);
                        }
                        catch (Exception e)
                        {
                            ULog.WriteException("Problem while updating cache:" + resourceAllocation.TicketID + e.ToString());
                        }

                        try
                        {
                            if (resourceAllocation.PctAllocation == 0 && !isPlanned)
                            {
                                RMMSummaryHelper.CleanAllocation(_context, resourceAllocation.ResourceWorkItems, cleanEstimated: true, rAllocation: resourceAllocation, rAllocationMonthly: null);// this.WorkItem
                            }
                        }
                        catch (Exception e)
                        {
                            ULog.WriteException("Problem while CleanAllocation:" + resourceAllocation.TicketID + e.ToString());
                        }

                    }
                }
            }
            catch (Exception e)
            {
                ULog.WriteException("Save(RResourceAllocation_semaphore Lock:" + e.ToString());
            }
            finally
            {
                _semaphore.Release();
            }
            return string.Empty;
        }
        public override List<RResourceAllocation> Load(Expression<Func<RResourceAllocation, bool>> where, Expression<Func<RResourceAllocation, object>> order = null, int skip = 0, int take = 0, List<Expression<Func<RResourceAllocation, object>>> includeExpressions = null)
        {
            ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
            return base.Load(where, order, skip, take, includeExpressions);
        }
        public override RResourceAllocation Get(Expression<Func<RResourceAllocation, bool>> where, Expression<Func<RResourceAllocation, RResourceAllocation>> order = null, List<Expression<Func<RResourceAllocation, object>>> includeExpressions = null)
        {
            ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
            RResourceAllocation item = base.Get(where, order, includeExpressions);
            item.ResourceWorkItems = resourceWorkItemsManager.Get(y => y.ID == item.ResourceWorkItemLookup);
            return item;
        }
        public override RResourceAllocation Get(object ID)
        {
            ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
            RResourceAllocation item = base.Get(ID);
            if (item != null)
                item.ResourceWorkItems = resourceWorkItemsManager.Get(y => y.ID == item.ResourceWorkItemLookup);
            return item;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tense">0 for past data, 1 for current data, 2 for currnet and future, 3 for future only, 4 for all</param>
        /// <returns></returns>
        public double AllocationPercentage(string userID, int tense, bool IncludeClosed = false)
        {
            DataTable table = LoadRawTableByResource(userID, tense);
            if (table != null)
            {
                if (!IncludeClosed)
                {
                    if (!UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketId, table))
                        table.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));

                    //workitemManager.LoadByID
                    //ResourceWorkItems resourceWorkItems = null;
                    //List<ResourceWorkItems> lstResourceWorkItems = new List<ResourceWorkItems>();
                    //string cacheName = DatabaseObjects.Tables.ResourceWorkItems + "_" + _context.TenantID;
                    //lstResourceWorkItems = (List<ResourceWorkItems>)CacheHelper<object>.Get(cacheName, _context.TenantID);
                    //if (lstResourceWorkItems == null)
                    //    lstResourceWorkItems = workitemManager.Load();
                    //for (int i = 0; i < table.Rows.Count; i++)
                    //{
                    //    if (lstResourceWorkItems == null)
                    //        resourceWorkItems = workitemManager.LoadByID(Convert.ToInt64(table.Rows[i][DatabaseObjects.Columns.ResourceWorkItemLookup]));
                    //    else
                    //    {
                    //        resourceWorkItems = lstResourceWorkItems.FirstOrDefault(x => x.ID == Convert.ToInt64(table.Rows[i][DatabaseObjects.Columns.ResourceWorkItemLookup]));
                    //        table.Rows[i][DatabaseObjects.Columns.TicketId] = resourceWorkItems.WorkItem;
                    //    }

                    //}

                    // Filter by Open Tickets.
                    List<string> LstClosedTicketIds = new List<string>();
                    //get closed ticket instead of open ticket and then filter all except closed ticket
                    DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(_context);
                    if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
                    {
                        LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
                    }
                    DataRow[] dr = table.AsEnumerable().Where(x => !LstClosedTicketIds.Any(y => x.Field<string>(DatabaseObjects.Columns.TicketId).EqualsIgnoreCase(y))).ToArray();
                    if (dr != null && dr.Count() > 0)
                        table = dr.CopyToDataTable();
                    else
                        table.Rows.Clear();
                }

                if (table.Rows.Count > 0)
                {
                    if (table.Columns.Contains(DatabaseObjects.Columns.AllocationStartDate) && table.Columns.Contains(DatabaseObjects.Columns.AllocationEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PctAllocation))
                    {
                        if (tense == 1)
                        {
                            // since we got list of current allocations, total current allocation is just sum of PctAllocation column
                            return table.AsEnumerable().Sum(x => UGITUtility.StringToDouble(x[DatabaseObjects.Columns.PctAllocation]));
                        }
                        else
                        {
                            DateTime minDate = table.Rows.OfType<DataRow>().Select(k => UGITUtility.StringToDateTime(k[DatabaseObjects.Columns.AllocationStartDate])).Min();
                            DateTime maxDate = table.Rows.OfType<DataRow>().Select(k => UGITUtility.StringToDateTime(k[DatabaseObjects.Columns.AllocationEndDate])).Max();
                            int totalNoOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, minDate, maxDate);
                            double alloc = 0;
                            foreach (DataRow dr in table.Rows)
                            {
                                int noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.AllocationStartDate]), UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.AllocationEndDate]));
                                alloc += ((noOfDays * UGITUtility.StringToDouble(dr[DatabaseObjects.Columns.PctAllocation])) / 100);
                            }
                            return Math.Round(alloc * 100 / totalNoOfWorkingDays, 2);
                        }
                    }
                }
            }
            return 0;
        }
        public int AllocationPercentage(string userID, DateTime startDate, DateTime endDate)
        {
            return AllocationPercentage(userID, startDate, endDate, true);
        }
        /// <summary>
        /// Total Allocation calculation on the base of userid, and date range.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="tense"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int AllocationPercentage(string userID, DateTime startDate, DateTime endDate, bool minOneDay)
        {
            DataTable table = LoadRawTableByResource(userID, 4); // Since filtering on dates, get all allocations (tense = 4)

            if (table != null && table.Rows.Count > 0)
            {
                DateTime minDate = startDate;
                DateTime maxDate = endDate;
                int totalNoOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, minDate, maxDate, minOneDay);
                double alloc = 0;
                int workingHrs = uHelper.GetWorkingHoursInADay(_context);
                if (table.Columns.Contains(DatabaseObjects.Columns.AllocationStartDate) && table.Columns.Contains(DatabaseObjects.Columns.AllocationEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PctAllocation))
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        if (UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.WorkItemType]) == ModuleNames.OPM
                            || UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.WorkItemType]) == ModuleNames.NPR)
                        {
                            if (UGITUtility.StringToBoolean(dr[DatabaseObjects.Columns.Closed]) == true)
                                continue;
                        }
                        if (UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.AllocationStartDate]) == DateTime.MinValue ||
                            UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.AllocationEndDate]) == DateTime.MinValue)
                            continue;

                        DateTime allocatedstdt = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.AllocationStartDate]),
                               allocatedenddt = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.AllocationEndDate]);
                        double pctAlloc = UGITUtility.StringToDouble(dr[DatabaseObjects.Columns.PctAllocation]);

                        if (allocatedstdt <= startDate && allocatedenddt <= endDate && allocatedenddt > startDate)
                        {
                            int noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, startDate, allocatedenddt, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt >= startDate && allocatedenddt <= endDate)
                        {
                            int noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, allocatedstdt, allocatedenddt, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt >= startDate && allocatedenddt >= endDate && allocatedenddt > startDate)
                        {
                            int noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, allocatedstdt, endDate, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt <= startDate && allocatedenddt >= endDate)
                        {
                            int noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, startDate, endDate, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else
                        {
                            alloc += 0;
                        }
                    }
                }

                if (totalNoOfWorkingDays > 0)
                    return UGITUtility.StringToInt((alloc * 100) / totalNoOfWorkingDays);
            }

            return 0;
        }

        private static DataTable CreateTable()
        {
            DataTable result = new DataTable("Allocation");
            result.Columns.Add(DatabaseObjects.Columns.Id, typeof(long));
            result.Columns.Add(DatabaseObjects.Columns.WorkItemType, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.WorkItem, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.WorkItemLink, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.PctAllocation, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.ResourceId, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            result.Columns.Add("WorkItemID", typeof(long));
            result.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ShowEditButton, typeof(bool));
            result.Columns.Add(DatabaseObjects.Columns.ShowPartialEdit, typeof(bool));
            result.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.PlannedStartDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.PlannedEndDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.PctEstimatedAllocation, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.EstStartDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.EstEndDate, typeof(DateTime));
            result.Columns.Add(DatabaseObjects.Columns.Closed, typeof(bool));
            result.Columns.Add("CMICLink", typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ERPJobID, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.SoftAllocation, typeof(string));

            result.Columns.Add(DatabaseObjects.Columns.IsAllocInPrecon, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.IsAllocInConst, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.IsAllocInCloseOut, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.IsStartDateBeforePrecon, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.IsStartDateBetweenPreconAndConst, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.IsStartDateBetweenConstAndCloseOut, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.OnHold, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.Status, typeof(string));
            return result;
        }

        /// <summary>
        /// Convert list of resource allcoations to datatable
        /// </summary>
        /// <param name="allocations"></param>
        /// <returns></returns>
        public DataTable ConvertObjectToDataTable(List<RResourceAllocation> allocations)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            DataTable result = CreateTable();
            string RMMLevel1PMMProjects = uHelper.GetModuleTitle("PMM");
            string RMMLevel1NPRProjects = uHelper.GetModuleTitle("NPR");
            string RMMLevel1TSKProjects = uHelper.GetModuleTitle("TSK");

            //Get Close stage id of NPR and PMMs
            DataRow[] genericStages = new DataRow[0];
            DataTable gStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.StageType, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            if (gStages != null)
            {
                genericStages = gStages.Select();
            }
            DataRow[] nprStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, "NPR"));
            string nprClosedStageID = uHelper.GetModuleStageId(nprStages, genericStages, StageType.Closed);
            DataRow[] pmmStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, "PMM"));
            string pmmClosedStageID = uHelper.GetModuleStageId(pmmStages, genericStages, StageType.Closed);
            DataRow[] tskStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, "TSK"));
            string tskClosedStageID = uHelper.GetModuleStageId(pmmStages, genericStages, StageType.Closed);

            List<string> queryExps = new List<string>();
            List<string> tempAllocations = new List<string>();
            UGITModule nprModuleRow = null;
            DataTable nprData = null;
            tempAllocations = allocations.Where(x => x.ResourceWorkItems.WorkItemType == RMMLevel1NPRProjects).Select(x => x.ResourceWorkItems.WorkItem).Distinct().ToList();

            List<string> queryList = new List<string>();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }
                nprModuleRow = moduleViewManager.GetByName("NPR");
                string nprQuery = string.Join(" and", queryList);// uHelper.GenerateWhereQueryWithAndOr(queryExps, queryExps.Count - 1, false));
                DataTable listID = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRRequest, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                nprData = listID.Select(nprQuery).CopyToDataTable();
                queryExps = new List<string>();
                queryList = new List<string>();
            }

            UGITModule tskModuleRow = null;
            DataTable tskData = null;
            tempAllocations = allocations.Where(x => x.ResourceWorkItems.WorkItemType == RMMLevel1TSKProjects).Select(x => x.ResourceWorkItems.WorkItem).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                tskModuleRow = moduleViewManager.GetByName("TSK");
                string tskQuery = string.Join(" and", queryList);
                DataTable listTSK = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                tskData = listTSK.Select(tskQuery).CopyToDataTable();

                queryExps = new List<string>();
                queryList = new List<string>();
            }

            UGITModule pmmModuleRow = null;
            DataTable pmmData = null;
            tempAllocations = allocations.Where(x => x.ResourceWorkItems.WorkItemType == RMMLevel1PMMProjects).Select(x => x.ResourceWorkItems.WorkItem).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                pmmModuleRow = moduleViewManager.GetByName("PMM");
                string pmmQuery = string.Join(" and", queryExps);
                DataTable listPMM = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                pmmData = listPMM.Select(pmmQuery).CopyToDataTable();

                queryExps = null;
            }

            foreach (RResourceAllocation rAllocation in allocations)
            {
                if (rAllocation.ResourceWorkItems != null && rAllocation.ResourceWorkItems.WorkItemType.Trim() != string.Empty)
                {
                    DataRow row = result.NewRow();
                    row[DatabaseObjects.Columns.ShowEditButton] = true;
                    row[DatabaseObjects.Columns.ShowPartialEdit] = false;
                    row[DatabaseObjects.Columns.Id] = rAllocation.ID;
                    row[DatabaseObjects.Columns.WorkItemType] = rAllocation.ResourceWorkItems.WorkItemType;
                    row[DatabaseObjects.Columns.WorkItem] = rAllocation.ResourceWorkItems.WorkItem;
                    row[DatabaseObjects.Columns.WorkItemLink] = rAllocation.ResourceWorkItems.SubWorkItem;

                    if (rAllocation.ResourceWorkItems.WorkItemType == RMMLevel1NPRProjects && nprData != null && nprData.Rows.Count > 0)
                    {
                        DataRow[] items = nprData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, rAllocation.ResourceWorkItems.WorkItem));
                        if (items.Length > 0)
                        {
                            row[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                            row[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(nprModuleRow,
                                    Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, rAllocation.ResourceWorkItems.WorkItem);

                            //If project is closed then use cann't edit the entry
                            if (UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.ModuleStepLookup).ToString() != string.Empty)
                            {
                                string moduleStepLookup = Convert.ToString(items[0][DatabaseObjects.Columns.ModuleStepLookup]);
                                if (moduleStepLookup.ToString() == nprClosedStageID)
                                {
                                    row[DatabaseObjects.Columns.ShowPartialEdit] = true;
                                    row[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", row[DatabaseObjects.Columns.Title]);
                                }
                            }
                        }
                    }
                    else if (rAllocation.ResourceWorkItems.WorkItemType == RMMLevel1PMMProjects && pmmData != null && pmmData.Rows.Count > 0)
                    {
                        DataRow[] items = pmmData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, rAllocation.ResourceWorkItems.WorkItem));
                        if (items.Length > 0)
                        {
                            row[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                            row[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(pmmModuleRow,
                                Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, rAllocation.ResourceWorkItems.WorkItem);

                            //If project is closed then use cann't edit the entry
                            if (UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.ModuleStepLookup).ToString() != string.Empty)
                            {
                                string moduleStepLookup = Convert.ToString(items[0][DatabaseObjects.Columns.ModuleStepLookup]);
                                if (moduleStepLookup.ToString() == pmmClosedStageID)
                                {
                                    row[DatabaseObjects.Columns.ShowPartialEdit] = true;
                                    row[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", row[DatabaseObjects.Columns.Title]);
                                }
                            }
                        }
                    }
                    else if (rAllocation.ResourceWorkItems.WorkItemType == RMMLevel1TSKProjects && tskData != null && tskData.Rows.Count > 0)
                    {
                        DataRow[] items = tskData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, rAllocation.ResourceWorkItems.WorkItem));
                        if (items.Length > 0)
                        {
                            row[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                            row[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(tskModuleRow,
                                Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, rAllocation.ResourceWorkItems.WorkItem);

                            //If project is closed then use cann't edit the entry
                            if (UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.ModuleStepLookup).ToString() != string.Empty)
                            {
                                string moduleStepLookup = Convert.ToString(items[0][DatabaseObjects.Columns.ModuleStepLookup]);
                                if (moduleStepLookup.ToString() == tskClosedStageID)
                                {
                                    row[DatabaseObjects.Columns.ShowPartialEdit] = true;
                                    row[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", row[DatabaseObjects.Columns.Title]);
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow moduleTaskModuleRow = null;
                        DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, rAllocation.ResourceWorkItems.WorkItemType));
                        if (drModules != null && drModules.Length > 0)
                            moduleTaskModuleRow = drModules[0];
                        if (moduleTaskModuleRow != null)
                        {
                            string moduleTaskQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, row[DatabaseObjects.Columns.WorkItem]);
                            DataTable listTicket = GetTableDataManager.GetTableData(Convert.ToString(moduleTaskModuleRow[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                            DataTable moduleTaskData = listTicket.Select(moduleTaskQuery).CopyToDataTable();

                            if (moduleTaskData.Rows.Count > 0)
                            {
                                row[DatabaseObjects.Columns.Title] = Convert.ToString(moduleTaskData.Rows[0][DatabaseObjects.Columns.Title]);
                                row[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(tskModuleRow,
                                    Convert.ToString(moduleTaskData.Rows[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(moduleTaskData.Rows[0][DatabaseObjects.Columns.Title])).NavigateUrl, rAllocation.ResourceWorkItems.WorkItem);

                                //If project is closed then use cann't edit the entry
                                if (UGITUtility.GetSPItemValue(moduleTaskData.Rows[0], DatabaseObjects.Columns.ModuleStepLookup).ToString() != string.Empty)
                                {
                                    string moduleStepLookup = Convert.ToString(moduleTaskData.Rows[0][DatabaseObjects.Columns.ModuleStepLookup]);
                                    if (moduleStepLookup.ToString() == tskClosedStageID)
                                    {
                                        row[DatabaseObjects.Columns.ShowPartialEdit] = true;
                                        row[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", row[DatabaseObjects.Columns.Title]);
                                    }
                                }
                            }
                        }
                    }

                    string subWorkItem = string.Empty;
                    if (rAllocation.ResourceWorkItems.SubWorkItem != null && rAllocation.ResourceWorkItems.SubWorkItem.Trim() != string.Empty)
                        subWorkItem = rAllocation.ResourceWorkItems.SubWorkItem.Trim();

                    row[DatabaseObjects.Columns.SubWorkItem] = subWorkItem;
                    row[DatabaseObjects.Columns.PctAllocation] = Math.Round(UGITUtility.StringToDouble(rAllocation.PctAllocation), 2);
                    row[DatabaseObjects.Columns.AllocationStartDate] = rAllocation.AllocationStartDate;
                    row[DatabaseObjects.Columns.AllocationEndDate] = rAllocation.AllocationEndDate;
                    row[DatabaseObjects.Columns.ResourceId] = rAllocation.Resource;
                    row[DatabaseObjects.Columns.Resource] = rAllocation.Resource;
                    row[DatabaseObjects.Columns.PctPlannedAllocation] = Math.Round(UGITUtility.StringToDouble(rAllocation.PctPlannedAllocation), 2);
                    row["WorkItemID"] = rAllocation.ResourceWorkItems.ID;
                    result.Rows.Add(row);
                }
            }
            return result;
        }

        public DataTable LoadTableByWorkItem(string workItemTypeId, string workItemId, string subWorkItemId, int tense)
        {

            return LoadTableByWorkItem(workItemTypeId, workItemId, subWorkItemId, tense, false, false);
        }

        public ResourceWorkItems LoadByWorkItem(string user, string workItemTypeId, string workItemId, int subWorkItemId)
        {
            Expression<Func<ResourceWorkItems, bool>> exp = (t) => true;
            if (!string.IsNullOrEmpty(user))
                exp = exp.And(x => x.Resource == user);

            if (!string.IsNullOrEmpty(workItemTypeId))
                exp = exp.And(x => x.WorkItemType == workItemTypeId);

            if (!string.IsNullOrEmpty(workItemId))
                exp = exp.And(x => x.WorkItem == workItemId);

            ResourceWorkItemsManager workitemManager = new ResourceWorkItemsManager(_context);
            List<ResourceWorkItems> workitems = workitemManager.Load(exp);
            return workitems.FirstOrDefault();
        }
        public List<RResourceAllocation> LoadByWorkItem(string workItemTypeId, string workItemId, string subWorkItemId, int tense, bool ignoreWorkItemId, bool ignoreSubWorkItemId)
        {
            ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
            List<ResourceWorkItems> resourceWorkItems = resourceWorkItemsManager.LoadWorkItemsById(workItemTypeId, workItemId, subWorkItemId, ignoreWorkItemId, ignoreSubWorkItemId);
            List<string> query = new List<string>();
            List<string> queryOr = new List<string>();

            for (int i = 0; i < resourceWorkItems.Count; i++)
            {
                if (resourceWorkItems[i] != null)
                {
                    query.Add(Convert.ToString(resourceWorkItems[i].ID));

                    // spliting IN condition with the help of OR
                    if ((i > 0 && i % 300 == 0) || i == resourceWorkItems.Count - 1)
                    {
                        //queryOr.Add(string.Format("<In><FieldRef Name=\"ResourceWorkItemLookup\" /><Values>{0}</Values></In>", string.Join("", query.ToArray())));

                        queryOr.Add(string.Format("{0} IN ( {1})", DatabaseObjects.Columns.ResourceWorkItemLookup, string.Join(",", query.ToArray())));
                        query.Clear();
                    }
                }
            }

            DataTable rAllocation = this.GetDataTable();
            string rQuery = "";
            if (resourceWorkItems.Count == 0)
            {
                return new List<RResourceAllocation>();
            }

            DateTime today = DateTime.Now.Date;
            List<string> requiredQuery = new List<string>();
            if (queryOr.Count > 0)
                requiredQuery.Add(" ( " + string.Join(" or ", queryOr) + " ) ");

            switch (tense)
            {
                case 0:
                    requiredQuery.Add(string.Format("{0}<{1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                    break;
                case 1:
                    requiredQuery.Add(string.Format("{0}<={1}", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                    requiredQuery.Add(string.Format("{0}>={1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                    break;
                case 2:
                    requiredQuery.Add(string.Format("{0}>={1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                    break;
                case 3:
                    requiredQuery.Add(string.Format("{0}>{1}", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                    break;
                default: break;
            }

            rQuery = string.Join(" and ", requiredQuery);
            DataRow[] collections = null;
            collections = rAllocation.Select(rQuery);
            List<RResourceAllocation> allocations = new List<RResourceAllocation>();
            if (collections != null)
            {
                foreach (DataRow item in collections)
                {
                    RResourceAllocation allocation = new RResourceAllocation();
                    allocation = this.Get(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
                    if (allocation != null && !allocation.Deleted)
                        allocations.Add(allocation);
                }
            }

            return allocations;
        }

        public DataTable LoadTableByWorkItem(string workItemTypeId, string workItemId, string subWorkItemId, int tense, bool ignoreWorkItemId, bool ignoreSubWorkItemId)
        {
            List<RResourceAllocation> allocations = this.LoadByWorkItem(workItemTypeId, workItemId, subWorkItemId, tense, ignoreWorkItemId, ignoreSubWorkItemId).ToList();
            return ConvertObjectToDataTable(allocations);
        }

        private ResourceAllocation LoadAllocationByResourceWorkItemId(string resourceWorkItemId)
        {
            ResourceAllocation resourceAllocation = null;



            return resourceAllocation;
        }
        public List<RResourceAllocation> LoadByResource(string user, int tense)
        {
            List<RResourceAllocation> allocations = new List<RResourceAllocation>();
            DataTable rAllocation = this.GetDataTable();//(DatabaseObjects.Lists.ResourceAllocation);
            try
            {
                string rQuery = "";
                string userId = user;

                DateTime today = DateTime.UtcNow.Date;
                List<string> requiredQuery = new List<string>();
                requiredQuery.Add(string.Format("{0}=0", DatabaseObjects.Columns.Deleted));
                if (!string.IsNullOrEmpty(userId))
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
                }
                else
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
                }

                switch (tense)
                {
                    case 0:
                        requiredQuery.Add(string.Format("{0}<{1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                        break;
                    case 1:
                        requiredQuery.Add(string.Format("{0}<={1}", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                        requiredQuery.Add(string.Format("{0}>={1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                        break;
                    case 2:
                        requiredQuery.Add(string.Format("{0}>={1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                        break;
                    case 3:
                        requiredQuery.Add(string.Format("{0}>{1}", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                        break;
                    default: break;
                }

                rQuery = string.Join(" and ", requiredQuery);
                DataRow[] resultCollection = rAllocation.Select(rQuery);
                if (resultCollection != null)
                {
                    foreach (DataRow item in resultCollection)
                    {
                        RResourceAllocation allocation = new RResourceAllocation();
                        allocation = this.LoadByID(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
                        allocations.Add(allocation);
                    }
                }
            }
            catch { }
            return allocations;
        }

        public List<RResourceAllocation> LoadByResource(string user, DateTime startDate, DateTime endDate)
        {
            List<RResourceAllocation> allocations = new List<RResourceAllocation>();
            DataTable rAllocation = this.GetDataTable();// (DatabaseObjects.Lists.ResourceAllocation, spWeb);
            try
            {
                string rQuery;
                string userId = user;

                DateTime today = DateTime.UtcNow.Date;
                List<string> requiredQuery = new List<string>();
                requiredQuery.Add(string.Format("{0}=0", DatabaseObjects.Columns.Deleted));
                if (!string.IsNullOrEmpty(userId))
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
                }
                else
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
                }

                requiredQuery.Add(string.Format("{0}<={1}", DatabaseObjects.Columns.AllocationEndDate, startDate.ToString("yyyy-MM-dd")));
                requiredQuery.Add(string.Format("{0}=>{1}", DatabaseObjects.Columns.AllocationStartDate, endDate.ToString("yyyy-MM-dd")));

                rQuery = string.Join(" and ", requiredQuery);
                DataRow[] resultCollection = rAllocation.Select(rQuery);
                if (resultCollection != null)
                {
                    foreach (DataRow item in resultCollection)
                    {
                        RResourceAllocation allocation = new RResourceAllocation();
                        allocation = this.LoadByID(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
                        allocations.Add(allocation);
                    }
                }
            }
            catch { }
            return allocations;
        }

        public List<RResourceAllocation> LoadByResource(string user, DateTime startDate, DateTime endDate, ResourceAllocationType allocationType, bool IncludeClosed = false)
        {
            List<RResourceAllocation> allocations = new List<RResourceAllocation>();


            Expression<Func<RResourceAllocation, bool>> exp = (t) => true;
            string userId = user;
            DateTime today = DateTime.UtcNow.Date;
            List<string> requiredQuery = new List<string>();
            exp = x => !x.Deleted;
            if (!string.IsNullOrWhiteSpace(user))
                exp = exp.And(x => x.Resource == user);


            List<string> qryOrDates = new List<string>();
            List<string> qryDates = new List<string>();
            if (allocationType == ResourceAllocationType.Planned)
            {
                exp = exp.And(x => x.PlannedStartDate.HasValue && x.PlannedEndDate.HasValue &&
                      (x.PlannedEndDate.Value.Date <= startDate.Date && x.PlannedStartDate.Value.Date >= endDate.Date) ||
                      (x.PlannedEndDate.Value.Date >= startDate.Date && x.PlannedStartDate.Value.Date <= endDate.Date));
            }
            else if (allocationType == ResourceAllocationType.Estimated)
            {
                exp = exp.And(x => x.EstStartDate.HasValue && x.EstEndDate.HasValue &&
                      (x.EstEndDate.Value.Date <= startDate.Date && x.EstStartDate.Value.Date >= endDate.Date) ||
                      (x.EstEndDate.Value.Date >= startDate.Date && x.EstStartDate.Value.Date <= endDate.Date));
            }
            else
            {
                Expression<Func<RResourceAllocation, bool>> exp1 = (t) => true;

                exp1 = exp1.And(x => x.AllocationStartDate.HasValue && x.AllocationEndDate.HasValue &&
                     (x.AllocationEndDate.Value.Date <= startDate.Date && x.AllocationStartDate.Value.Date >= endDate.Date) ||
                     (x.AllocationEndDate.Value.Date >= startDate.Date && x.AllocationStartDate.Value.Date <= endDate.Date));

                exp1 = exp1.Or(x => x.PlannedStartDate.HasValue && x.PlannedEndDate.HasValue &&
                     (x.PlannedEndDate.Value.Date <= startDate.Date && x.PlannedStartDate.Value.Date >= endDate.Date) ||
                     (x.PlannedEndDate.Value.Date >= startDate.Date && x.PlannedStartDate.Value.Date <= endDate.Date));
                exp = exp.And(exp1);
            }

            allocations = this.Load(exp);

            var workItems = workitemManager.Load();


            allocations.ForEach(x =>
            {
                x.ResourceWorkItems = new ResourceWorkItems();
                x.ResourceWorkItems.WorkItem = workItems.Where(y => y.ID == x.ResourceWorkItemLookup)?.FirstOrDefault()?.WorkItem;
                //x.IsAllocInPrecon = 
            });


            if (!IncludeClosed)
            {
                // Filter by Open Tickets.
                List<string> LstClosedTicketIds = new List<string>();
                //get closed ticket instead of open ticket and then filter all except closed ticket
                DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(_context);
                if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
                {
                    LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
                }

                allocations = allocations.Where(x => !LstClosedTicketIds.Any(y => x.ResourceWorkItems.WorkItem.EqualsIgnoreCase(y))).ToList();
            }

            //DataTable dt = new DataTable();
            //Dictionary<string, object> values = new Dictionary<string, object>();
            //values.Add("@allocations", allocations);

            //dt = uGITDAL.ExecuteDataSetWithParameters("usp_Getdatelist", values);

            var query = from a in DatabaseObjects.Tables.CRMProject
                        select a;

            return allocations;
        }

        // tense: 0 for past data, 1 for current data, 2 for currnet and future, 3 for future only, 4 for all
        public List<RResourceAllocation> LoadByResource(List<int> usersID, int tense)
        {
            List<RResourceAllocation> allocations = new List<RResourceAllocation>();
            DataTable rAllocation = this.GetDataTable();// (DatabaseObjects.Lists.ResourceAllocation);
            try
            {
                string rQuery = "";
                DateTime today = DateTime.UtcNow.Date;
                List<string> requiredQuery = new List<string>();
                requiredQuery.Add(string.Format("{0}=0", DatabaseObjects.Columns.Deleted));
                List<string> userExprs = new List<string>();
                foreach (int urs in usersID)
                {
                    userExprs.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, urs));
                }
                requiredQuery.Add(" ( " + string.Join(" or ", userExprs) + " ) ");

                switch (tense)
                {
                    case 0:
                        requiredQuery.Add(string.Format("{0}<{1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                        break;
                    case 1:
                        requiredQuery.Add(string.Format("{0}<={1}", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                        requiredQuery.Add(string.Format("{0}>={1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                        break;
                    case 2:
                        requiredQuery.Add(string.Format("{0}>={1}", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                        break;
                    case 3:
                        requiredQuery.Add(string.Format("{0}>{1}", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                        break;
                    default: break;
                }

                rQuery = string.Join(" and ", requiredQuery);
                DataRow[] resultCollection = rAllocation.Select(rQuery);
                if (resultCollection != null)
                {
                    foreach (DataRow item in resultCollection)
                    {
                        RResourceAllocation allocation = new RResourceAllocation();
                        allocation = this.LoadByID(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
                        allocations.Add(allocation);
                    }
                }
            }
            catch { }
            return allocations;
        }
        private RResourceAllocation LoadObj(DataRow rItem)
        {
            RResourceAllocation allocation = new RResourceAllocation();
            try
            {

                allocation.ResourceWorkItems = workitemManager.LoadByID(UGITUtility.StringToInt(UGITUtility.SplitString(rItem[DatabaseObjects.Columns.ResourceWorkItemLookup], Constants.Separator, 0)));
                allocation.PctAllocation = Math.Round(UGITUtility.StringToDouble(rItem[DatabaseObjects.Columns.PctAllocation]), 2);
                allocation.AllocationStartDate = rItem[DatabaseObjects.Columns.AllocationStartDate] != null ? (DateTime)rItem[DatabaseObjects.Columns.AllocationStartDate] : DateTime.MinValue;
                allocation.AllocationEndDate = rItem[DatabaseObjects.Columns.AllocationEndDate] != null ? (DateTime)rItem[DatabaseObjects.Columns.AllocationEndDate] : DateTime.MinValue;
                allocation.ID = Convert.ToInt64(rItem[DatabaseObjects.Columns.ID]);
                allocation.Resource = UGITUtility.SplitString(rItem[DatabaseObjects.Columns.Resource], Constants.Separator, 0);
                //allocation.ResourceName = uHelper.SplitString(rItem[DatabaseObjects.Columns.Resource], Constants.Separator, 1);
                allocation.PctPlannedAllocation = Math.Round(UGITUtility.StringToDouble(rItem[DatabaseObjects.Columns.PctPlannedAllocation]), 2);
                allocation.PlannedStartDate = rItem[DatabaseObjects.Columns.PlannedStartDate] != DBNull.Value ? Convert.ToDateTime(rItem[DatabaseObjects.Columns.PlannedStartDate]) : DateTime.MinValue;
                allocation.PlannedEndDate = rItem[DatabaseObjects.Columns.PlannedEndDate] != DBNull.Value ? Convert.ToDateTime(rItem[DatabaseObjects.Columns.PlannedEndDate]) : DateTime.MinValue;
                if (allocation.ResourceWorkItems != null)
                    allocation.ResourceWorkItemLookup = allocation.ResourceWorkItems.ID;

                allocation.Deleted = Convert.ToBoolean(rItem[DatabaseObjects.Columns.Deleted]);
            }
            catch (Exception ex) { ULog.WriteException(ex); }
            return allocation;
        }



        /// <summary>
        /// Function checks if the given workItemId has an entry for the given date range.
        /// </summary>
        /// <param name="workItemId">Work Item Id for a particular resource</param>
        /// <param name="startDate">Range start date</param>
        /// <param name="endDate">Range end date</param>
        /// <returns>Exists or not</returns>
        public bool ResourceAllocationExistsOverlapping(long workItemId, DateTime startDate, DateTime endDate)
        {
            DataTable rAllocation = this.GetDataTable();// (DatabaseObjects.Lists.ResourceAllocation, spWeb);

            List<string> requiredQuery = new List<string>();
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.ResourceWorkItemLookup, workItemId));
            requiredQuery.Add(string.Format("{0}=false", DatabaseObjects.Columns.Deleted));
            string Query = string.Join(" and ", requiredQuery);
            DataRow[] resourceAllocations = rAllocation.Select(Query);
            if (resourceAllocations.Count() > 0)
            {
                foreach (DataRow resourceAllocation in resourceAllocations)
                {
                    DateTime allocationStartDate = (DateTime)resourceAllocation[DatabaseObjects.Columns.AllocationStartDate];
                    DateTime allocationEndDate = (DateTime)resourceAllocation[DatabaseObjects.Columns.AllocationEndDate];

                    //Overlaps between 2 AllocationRanges
                    //(StartA <= EndB) And (EndA >= StartB)
                    if (startDate <= allocationEndDate && endDate >= allocationStartDate)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public DataTable LoadRawTableByResource(string userID, int tense)
        {
            List<string> usersID = new List<string>();
            usersID.Add(userID);
            return LoadRawTableByResource(usersID, tense);
        }
        public DataTable GetDatelist(string Mode, DateTime Fromdate, DateTime Todate)
        {
            DataTable dt = new DataTable();
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@_Fromdate", Convert.ToDateTime(Fromdate));
            values.Add("@_Todate", Convert.ToDateTime(Todate));
            values.Add("@Mode", Mode);
            dt = uGITDAL.ExecuteDataSetWithParameters("usp_Getdatelist", values);
            return dt;
        }
        public DataTable GetDatelistAllocation(string Mode, DateTime Fromdate, DateTime Todate)
        {
            DataTable dt = new DataTable();
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@_Fromdate", Convert.ToDateTime(Fromdate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
            values.Add("@_Todate", Convert.ToDateTime(Todate).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
            values.Add("@Mode", Mode);
            dt = uGITDAL.ExecuteDataSetWithParameters("usp_GetdatelistAllocation", values);
            return dt;
        }
        public DataTable LoadRawTableByResource(List<string> usersID, int tense, DateTime? sDate = null, DateTime? eDate = null, string TicketID = null, bool useCache = true)
        {
            DataTable resultTable = null;
            string query = "";
            DataTable dt = new DataTable();
            string cacheName = DatabaseObjects.Tables.ResourceAllocation + "_" + _context.TenantID;
            Dictionary<string, object> values = new Dictionary<string, object>();
            List<string> requiredQuery = new List<string>();
            //requiredQuery.Add(string.Format("{0}=false", DatabaseObjects.Columns.Deleted));
            if (usersID != null && usersID.Count > 0)
            {
                List<string> userExps = new List<string>();
                foreach (string userid in usersID)
                {
                    userExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.ResourceId, userid));
                }
                requiredQuery.Add(" ( " + string.Join(" or ", userExps) + " ) ");
            }

            DateTime today = DateTime.Now;
            switch (tense)
            {
                case 0: // Past
                    requiredQuery.Add(string.Format("{0}<'{1}'", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                    break;
                case 1: // Present
                    requiredQuery.Add(string.Format("{0}<='{1}'", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                    requiredQuery.Add(string.Format("{0}>='{1}'", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                    break;
                case 2: // Present & Future
                    requiredQuery.Add(string.Format("{0}>='{1}'", DatabaseObjects.Columns.AllocationEndDate, today.ToString("yyyy-MM-dd")));
                    break;
                case 3: // Future
                    requiredQuery.Add(string.Format("{0}>'{1}'", DatabaseObjects.Columns.AllocationStartDate, today.ToString("yyyy-MM-dd")));
                    break;
                case 4://All
                    if (sDate.HasValue && eDate.HasValue && Convert.ToDateTime(sDate.Value) != DateTime.MinValue && Convert.ToDateTime(eDate.Value) != DateTime.MinValue)
                    {
                        List<string> rmmAllocationQuery = new List<string>();
                        rmmAllocationQuery.Add(string.Format(" ( {0} is not null or {1} is not null ) ", DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate));
                        List<string> lstPlanned = new List<string>();
                        lstPlanned.Add(string.Format("{0}<='{1}'", DatabaseObjects.Columns.PlannedStartDate, Convert.ToDateTime(eDate.Value).ToString("yyyy-MM-dd")));
                        lstPlanned.Add(string.Format("{0}>='{1}'", DatabaseObjects.Columns.PlannedEndDate, Convert.ToDateTime(sDate.Value).ToString("yyyy-MM-dd")));
                        rmmAllocationQuery.Add("(" + string.Join(" and ", lstPlanned) + ")");
                        List<string> asPerPlannedDate = new List<string>();
                        asPerPlannedDate.Add("(" + string.Join(" and ", rmmAllocationQuery) + ")");
                        rmmAllocationQuery.Clear();
                        rmmAllocationQuery.Add(string.Format("( {0} is not null or {1} is not null )", DatabaseObjects.Columns.AllocationStartDate, DatabaseObjects.Columns.AllocationEndDate));
                        lstPlanned.Clear();
                        lstPlanned.Add(string.Format("{0} <= '{1}'", DatabaseObjects.Columns.AllocationStartDate, Convert.ToDateTime(eDate.Value).ToString("yyyy-MM-dd")));
                        lstPlanned.Add(string.Format("{0} >= '{1}'", DatabaseObjects.Columns.AllocationEndDate, Convert.ToDateTime(sDate.Value).ToString("yyyy-MM-dd")));
                        rmmAllocationQuery.Add("(" + string.Join(" and ", lstPlanned) + ")");
                        asPerPlannedDate.Add("(" + string.Join(" and ", rmmAllocationQuery) + ")");
                        requiredQuery.Add("(" + string.Join(" or ", asPerPlannedDate) + ")");
                    }
                    break;
                case 5: //All years Data for new gantt in project team based on ticket id
                    requiredQuery.Add(string.Format("{0} = {1}", DatabaseObjects.Columns.WorkItem, TicketID));
                    break;
                default: break;
            }

            requiredQuery.Add(string.Format("({0} <> 'True')", DatabaseObjects.Columns.Deleted));
            query = string.Join(" and ", requiredQuery);
            dt = (DataTable)CacheHelper<object>.Get($"dt_{cacheName}", _context.TenantID);
            if (dt == null || !useCache)
            {
                values.Add("@TenantID", _context.TenantID);
                dt = GetTableDataManager.GetData("ResourceAllocation2", values);
                CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dt);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    DataView view = dt.AsDataView();
                    view.RowFilter = query;
                    resultTable = view.ToTable();
                }
                catch (Exception ex) { ULog.WriteException(ex); }

            }
            return resultTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tense">0 for past data, 1 for current data, 2 for currnet and future, 3 for future only, 4 for all</param>
        /// <returns></returns>
        public DataTable LoadWorkAllocationByDate(List<string> usersID, int tense, DateTime? sDate = null, DateTime? eDate = null)
        {
            DataTable resultTable = CreateTable();

            string RMMLevel1PMMProjects = uHelper.GetModuleTitle("PMM");
            string RMMLevel1NPRProjects = uHelper.GetModuleTitle("NPR");
            string RMMLevel1TSKProjects = uHelper.GetModuleTitle("TSK");
            DataTable alloctions = null;
            //Loads allocations and workitem data for specified users
            alloctions = this.LoadRawTableByResource(usersID, tense, sDate: sDate, eDate: eDate);
            DataTable workitems = workitemManager.LoadRawTableByResource(usersID, 1);

            //returns if there is not allocation and workitem
            if ((alloctions != null && alloctions.Rows.Count <= 0) || (workitems != null && workitems.Rows.Count <= 0))
            {
                return resultTable;
            }

            //DataRow workitemRow = null;
            string lookupVal = "";
            //Iterates on each allocation
            if (alloctions != null)
            {
                resultTable = alloctions;

            }
            //returns if there is not allocation exist
            if (resultTable.Rows.Count < 0)
            {
                return resultTable;
            }

            List<string> queryExps = new List<string>();
            List<string> tempAllocations = new List<string>();
            UGITModule nprModuleRow = null;
            DataTable nprData = null;
            ModuleViewManager moduleManager = new ModuleViewManager(_context);
            TicketManager ticketManager = new TicketManager(_context);
            //if NPR work item is exist then loads NPR tickets and NPR module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType) == RMMLevel1NPRProjects || x.Field<string>(DatabaseObjects.Columns.WorkItemType) == ModuleNames.NPR)
                                                                                .Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }
                nprModuleRow = moduleManager.GetByName("NPR");
                string nprQuery = string.Join(" or ", queryExps);

                DataTable list = ticketManager.GetAllTickets(nprModuleRow);
                DataRow[] nprmodulerowCollection = list.Select(nprQuery);
                if (nprmodulerowCollection != null && nprmodulerowCollection.Count() > 0)
                    nprData = list.Select(nprQuery).CopyToDataTable();

                queryExps = new List<string>();
            }

            UGITModule tskModuleRow = null;
            DataTable tskData = null;
            //if TSK work item is exist then loads TSK tickets and TSK module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType) == RMMLevel1TSKProjects || x.Field<string>(DatabaseObjects.Columns.WorkItemType) == ModuleNames.TSK)
                                                                                .Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                tskModuleRow = moduleManager.GetByName("TSK");
                string tskQuery = string.Join(" or ", queryExps);
                DataTable listID = ticketManager.GetAllTickets(tskModuleRow);
                tskData = listID.Select(tskQuery).CopyToDataTable();

                queryExps = new List<string>();
            }

            UGITModule pmmModuleRow = null;
            DataTable pmmData = null;
            //if PMM work item is exist then loads PMM tickets and PMM module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType) == RMMLevel1PMMProjects || x.Field<string>(DatabaseObjects.Columns.WorkItemType) == ModuleNames.PMM)
                                                                                .Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                pmmModuleRow = moduleManager.GetByName("PMM");
                string pmmQuery = string.Join(" or ", queryExps);
                DataTable listID = ticketManager.GetAllTickets(pmmModuleRow);
                pmmData = listID.Select(pmmQuery).CopyToDataTable();

                queryExps = null;
            }

            /* Skipped condition, to show titles for CRM related projects also.
            //Returns if there is no npr , pmm, tsk ticket exist
            if (!((nprData != null && nprData.Rows.Count > 0) || (tskData != null && tskData.Rows.Count > 0) || (pmmData != null && pmmData.Rows.Count > 0)))
            {
                return resultTable;
            }
            */
            List<string> lstworkitems = new List<string>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable _listID = new DataTable();
            DataView dv = null;
            if (resultTable != null && resultTable.Rows.Count > 0)
            {
                lstworkitems.AddRange(resultTable.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)));
                string tickets = UGITUtility.ConvertListToString(lstworkitems, ","); //"'" + UGITUtility.ConvertListToString(lstworkitems, "','") + "'";
                values.Add("@TenantID", _context.TenantID);
                values.Add("@tickets", tickets);
                _listID = GetTableDataManager.GetData("AllWorkitems", values);
            }
            foreach (DataRow itemRow in resultTable.Rows)
            {
                //Runs in case of NPR type of workitem
                if ((Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == RMMLevel1NPRProjects || Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == ModuleNames.NPR) && nprData != null && nprData.Rows.Count > 0)
                {
                    DataRow[] items = nprData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        string title = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        string ticketid = Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(nprModuleRow,
                                ticketid, true, title).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);

                        lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                        if (!string.IsNullOrEmpty(lookupVal))
                        {
                            itemRow["SubWorkItem"] = lookupVal;
                        }

                        //If project is closed then user cann't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow[DatabaseObjects.Columns.ShowPartialEdit] = true;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:#9A2A2A;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else if ((Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == RMMLevel1PMMProjects || Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == ModuleNames.PMM) && pmmData != null && pmmData.Rows.Count > 0)
                {
                    DataRow[] items = pmmData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(pmmModuleRow,
                            Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);

                        lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                        if (!string.IsNullOrEmpty(lookupVal))
                        {
                            itemRow["SubWorkItem"] = lookupVal;
                        }
                        //If project is closed then user cann't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow[DatabaseObjects.Columns.ShowPartialEdit] = true;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='#9A2A2A;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else if ((Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == RMMLevel1TSKProjects || Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == ModuleNames.TSK) && tskData != null && tskData.Rows.Count > 0)
                {
                    DataRow[] items = tskData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(tskModuleRow,
                            Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);
                        lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                        if (!string.IsNullOrEmpty(lookupVal))
                        {
                            itemRow["SubWorkItem"] = lookupVal;
                        }
                        //If project is closed then user cann't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow[DatabaseObjects.Columns.ShowPartialEdit] = true;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:#9A2A2A;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else
                {
                    DataTable listID = new DataTable();
                    string WorkItemType = Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]);

                    UGITModule _uGITModule = moduleManager.LoadByName(WorkItemType);
                    if (_uGITModule != null)
                    {
                        string moduleTaskQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]);
                        if (_listID != null && _listID.Rows.Count > 0)
                        {
                            dv = _listID.AsDataView();
                            dv.RowFilter = moduleTaskQuery; // query
                            listID = dv.ToTable();
                            DataRow[] moduleTaskData = listID.Select();
                            if (moduleTaskData != null && moduleTaskData.Count() > 0)
                            {
                                itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(moduleTaskData[0][DatabaseObjects.Columns.Title]);

                                //If project is closed then user cann't edit the entry
                                if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(moduleTaskData[0], DatabaseObjects.Columns.TicketClosed)))
                                {
                                    itemRow[DatabaseObjects.Columns.Closed] = true;
                                    itemRow[DatabaseObjects.Columns.ShowPartialEdit] = true;
                                    itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:#9A2A2A;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                                }

                                string projectTitle = UGITUtility.ObjectToString(itemRow[DatabaseObjects.Columns.Title]);
                                string ticketid = Convert.ToString(moduleTaskData[0][DatabaseObjects.Columns.TicketId]);
                                itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(_uGITModule, ticketid, true, projectTitle).NavigateUrl, projectTitle);
                                itemRow[DatabaseObjects.Columns.ERPJobID] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(_uGITModule, ticketid, true, projectTitle).NavigateUrl,
                                     UGITUtility.ObjectToString(itemRow[DatabaseObjects.Columns.ERPJobID]));
                                lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                                if (!string.IsNullOrEmpty(lookupVal))
                                {
                                    itemRow["SubWorkItem"] = lookupVal;
                                }
                            }
                        }
                    }
                    else
                    {
                        itemRow[DatabaseObjects.Columns.Title] = UGITUtility.ObjectToString(itemRow[DatabaseObjects.Columns.WorkItemLink]);
                    }
                }
            }
            return resultTable;
        }

        public void UpdateProjectPlannedAllocation(string spWebUrl, List<UGITTask> tasks, List<string> users, string workItemType, string workItem, bool IsTask)
        {

            if (users == null || users.Count == 0)
                return;

            List<AllocationItem> tempAllocations = new List<AllocationItem>();

            ResourceAllocationManager allocManager = new ResourceAllocationManager(_context);
            ResourceWorkItemsManager workitemManager = new ResourceWorkItemsManager(_context);
            ResourceAllocationMonthlyManager allocMManager = new ResourceAllocationMonthlyManager(_context);
            UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
            UserProfileManager userProfileManager = new UserProfileManager(_context);

            try // Need try-catch to prevent crash taking down the web server!
            {
                users = users.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

                DateTime minStartDate = tasks.Min(x => x.StartDate);
                DateTime maxEndDate = tasks.Max(x => x.DueDate);

                List<RResourceAllocation> allAllocataions = allocManager.LoadByWorkItem(workItemType, workItem, string.Empty, 4, false, true);
                List<RResourceAllocation> uAllocations = null;
                RResourceAllocation rAllocation = null;
                tempAllocations = new List<AllocationItem>();
                //Get working in a day
                int workingHrsInDay = uHelper.GetWorkingHoursInADay(_context);

                // project alloction 
                ProjectAllocationManager pAllocationManager = new ProjectAllocationManager(_context);

                List<ProjectAllocation> tAllocations = null;
                List<ProjectAllocation> tempAllocations1 = null;



                foreach (string user in users)
                {
                    try
                    {
                        tAllocations = new List<ProjectAllocation>();
                        tempAllocations1 = new List<ProjectAllocation>();

                        uAllocations = allAllocataions.Where(x => x.Resource == user).ToList();

                        //need this code for delete multiple entries In ResourceAllocation Table 
                        if (uAllocations != null && uAllocations.Count > 0)
                            rAllocation = uAllocations.FirstOrDefault();
                        else
                            rAllocation = null;

                        if (uAllocations.Count > 1)
                        {
                            uAllocations.RemoveAt(0);
                            allocManager.Delete(uAllocations);
                        }

                        List<ProjectAllocation> projectAllocationsCollection = pAllocationManager.GetItemList(workItem, user);
                        //List<UGITTask> userTasks = tasks.Where(x => x.AssignedTo != null && x.AssignedTo.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList().Exists(y => y == user)).ToList();
                        List<UGITTask> userTasks = tasks.Where(x => x.AssignedTo != null && x.AssignedTo.Contains(user)).ToList();
                        List<UGITAssignTo> listAssignToPct = new List<UGITAssignTo>();
                        double totalPercentage = 0;
                        double totalUserTaskHrs = 0;
                        UserProfile userProfile = userProfileManager.GetUserById(user);

                        if (userProfile == null && !string.IsNullOrEmpty(userProfile.Id))
                            continue;

                        foreach (UGITTask t in userTasks)
                        {
                            totalUserTaskHrs = 0;
                            listAssignToPct = uGITTaskManager.GetUGITAssignPct(t.AssignToPct);
                            totalPercentage = listAssignToPct.Sum(x => UGITUtility.StringToDouble(x.Percentage));
                            UGITAssignTo ugitAssignToItem = listAssignToPct.FirstOrDefault(x => x.LoginName == userProfile.UserName);
                            if (ugitAssignToItem != null && totalPercentage > 0)
                                totalUserTaskHrs = UGITUtility.StringToDouble(ugitAssignToItem.Percentage) * t.EstimatedHours / totalPercentage;
                            if (t.Duration == 0)
                                t.Duration = uGITTaskManager.CalculateTaskDuration(t);
                            //don't include 0 hours entries
                            if (totalUserTaskHrs <= 0)
                                continue;

                            //changed to projectAllocationsCollection from tAllocations because tAllocation will always be 0 here
                            ProjectAllocation pAlloc = tAllocations.FirstOrDefault(x => t.StartDate.Date <= x.AllocationEndDate && t.DueDate.Date >= x.AllocationStartDate);
                            if (pAlloc != null)
                            {
                                pAlloc.AllocationHour += totalUserTaskHrs;
                                if (t.StartDate <= pAlloc.AllocationStartDate)
                                    pAlloc.AllocationStartDate = t.StartDate;
                                if (t.DueDate >= pAlloc.AllocationEndDate)
                                    pAlloc.AllocationEndDate = t.DueDate;
                                pAlloc.TotalWorkingHour += t.Duration * workingHrsInDay;
                            }
                            else
                            {
                                tAllocations.Add(new ProjectAllocation()
                                {
                                    AllocationStartDate = t.StartDate,
                                    AllocationEndDate = t.DueDate,
                                    AllocationHour = totalUserTaskHrs,
                                    TotalWorkingHour = t.Duration * workingHrsInDay
                                });
                            }
                        }

                        // added for project allocations
                        //Check overlapping dates and merge if any
                        tAllocations = tAllocations.OrderByDescending(x => x.AllocationStartDate).ThenBy(x => x.AllocationEndDate).ToList();
                        foreach (ProjectAllocation t in tAllocations)
                        {
                            ProjectAllocation pAlloc = tempAllocations1.FirstOrDefault(x => t.AllocationStartDate.Date <= x.AllocationEndDate.Date && t.AllocationEndDate.Date >= x.AllocationStartDate);
                            if (pAlloc != null)
                            {
                                pAlloc.AllocationHour += t.AllocationHour;
                                if (t.AllocationStartDate <= pAlloc.AllocationStartDate)
                                    pAlloc.AllocationStartDate = t.AllocationStartDate;
                                if (t.AllocationEndDate >= pAlloc.AllocationEndDate)
                                    pAlloc.AllocationEndDate = t.AllocationEndDate;
                                pAlloc.TotalWorkingHour += t.TotalWorkingHour;
                            }
                            else
                            {
                                tempAllocations1.Add(new ProjectAllocation()
                                {
                                    AllocationStartDate = t.AllocationStartDate,
                                    AllocationEndDate = t.AllocationEndDate,
                                    AllocationHour = t.AllocationHour,
                                    TotalWorkingHour = t.TotalWorkingHour
                                });
                            }
                        }
                        tAllocations = tempAllocations1;

                        //if no user task found then delete his allocations
                        if (userTasks.Count == 0 || !tAllocations.Exists(x => x.AllocationHour > 0))
                        {
                            pAllocationManager.Delete(projectAllocationsCollection);
                            RMMSummaryHelper.CleanAllocation(_context, workItemType, workItem, string.Empty, user, cleanPlanned: true);
                            continue;
                        }

                        // add or update projectAllocations tables
                        for (int i = 0; i < tAllocations.Count; i++)
                        {
                            ProjectAllocation item = null;
                            ProjectAllocation al = tAllocations[i];
                            Dictionary<string, object> values = new Dictionary<string, object>();
                            values.Clear();

                            if (projectAllocationsCollection.Count > i)
                            {
                                item = pAllocationManager.Get(projectAllocationsCollection[i].ID);
                                if (item != null)
                                    projectAllocationsCollection.Remove(item);
                            }

                            if (item == null)
                                item = new ProjectAllocation();

                            item.TicketID = workItem;
                            item.Resource = user;
                            item.AllocationStartDate = al.AllocationStartDate;
                            item.AllocationEndDate = al.AllocationEndDate;
                            item.AllocationHour = al.AllocationHour;
                            item.PctAllocation = 0;
                            if (al.TotalWorkingHour > 0)
                                item.PctAllocation = (al.AllocationHour * 100) / al.TotalWorkingHour;

                            pAllocationManager.Save(item);
                        }

                        if (projectAllocationsCollection.Count > tAllocations.Count)
                        {
                            pAllocationManager.Delete(projectAllocationsCollection);
                        }

                        //total allocation
                        if (rAllocation == null)
                            rAllocation = new RResourceAllocation(user);

                        if (rAllocation.ResourceWorkItems == null)
                            rAllocation.ResourceWorkItems = new ResourceWorkItems(user);
                        if (rAllocation.ResourceWorkItems.ID == 0)
                        {
                            rAllocation.ResourceWorkItems.WorkItemType = workItemType;
                            rAllocation.ResourceWorkItems.WorkItem = workItem;
                            rAllocation.ResourceWorkItems.SubWorkItem = string.Empty;
                        }

                        rAllocation.PlannedStartDate = tAllocations.Min(x => x.AllocationStartDate);
                        rAllocation.PlannedEndDate = tAllocations.Max(x => x.AllocationEndDate);

                        double pctAlloc = Math.Round((tAllocations.Sum(x => x.AllocationHour) * 100) / tAllocations.Sum(x => x.TotalWorkingHour), 2);
                        rAllocation.PctPlannedAllocation = pctAlloc;
                        allocManager.Save(rAllocation, true);

                        //ThreadStart threadStartMethod = delegate () {
                        RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, rAllocation.ResourceWorkItemLookup, true);
                        //};
                        //Thread sThread = new Thread(threadStartMethod);
                        //sThread.IsBackground = true;
                        //sThread.Start();
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, string.Format("ERROR Updating project allocation of User:{0} for workitem: {1}", user, workItem));
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "ERROR Updating project planned allocation");
            }
        }

        public void DeleteProjectPlannedAllocationUnAssignedUsers(string spWebUrl, List<UGITTask> tasks, List<string> users, string workItemType, string workItem, bool IsTask)
        {
            if (users == null || users.Count == 0)
                return;

            try
            {
                ResourceAllocationManager allocManager = new ResourceAllocationManager(_context);
                ResourceWorkItemsManager workitemManager = new ResourceWorkItemsManager(_context);
                ResourceAllocationMonthlyManager allocMManager = new ResourceAllocationMonthlyManager(_context);
                ProjectAllocationManager pManager = new ProjectAllocationManager(_context);
                UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
                UserProfileManager userProfileManager = new UserProfileManager(_context);

                List<RResourceAllocation> allAllocataions = allocManager.LoadByWorkItem(workItemType, workItem, string.Empty, 4, false, true);

                List<ResourceWorkItems> resourceWorkItems = workitemManager.LoadWorkItemsById(workItemType, workItem, string.Empty);
                List<ResourceAllocationMonthly> resourceAllocationMonthlies = allocMManager.Load(x => x.ResourceWorkItem == workItem);
                List<ProjectAllocation> projectAllocations = pManager.GetItemList(workItem);

                List<string> usersTobeRemoved = allAllocataions.Select(x => x.Resource).Except(users.Select(y => y)).ToList();
                allocManager.Delete(allAllocataions.Where(x => usersTobeRemoved.Contains(x.Resource)).ToList());

                usersTobeRemoved = resourceAllocationMonthlies.Select(x => x.Resource).Except(users.Select(y => y)).ToList();
                allocMManager.Delete(resourceAllocationMonthlies.Where(x => usersTobeRemoved.Contains(x.Resource)).ToList());

                usersTobeRemoved = resourceWorkItems.Select(x => x.Resource).Except(users.Select(y => y)).ToList();
                usersTobeRemoved = usersTobeRemoved.Where(x => !string.IsNullOrEmpty(x)).ToList();
                workitemManager.Delete(resourceWorkItems.Where(x => usersTobeRemoved.Contains(x.Resource)).ToList());

                usersTobeRemoved = projectAllocations.Select(x => x.Resource).Except(users.Select(y => y)).ToList();
                pManager.Delete(projectAllocations.Where(x => usersTobeRemoved.Contains(x.Resource)).ToList());
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "ERROR Deleting Un-Assigned Users from project planned allocation");
            }
        }

        public void DeletePlannedAllocationUnAssignedUser(string spWebUrl, List<string> users, string workItemType, string workItem, bool IsTask)
        {
            if (users == null || users.Count == 0)
                return;

            try
            {
                ResourceAllocationManager allocManager = new ResourceAllocationManager(_context);
                ResourceWorkItemsManager workitemManager = new ResourceWorkItemsManager(_context);
                ResourceAllocationMonthlyManager allocMManager = new ResourceAllocationMonthlyManager(_context);
                ProjectAllocationManager pManager = new ProjectAllocationManager(_context);
                UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
                UserProfileManager userProfileManager = new UserProfileManager(_context);

                List<RResourceAllocation> allAllocataions = allocManager.LoadByWorkItem(workItemType, workItem, string.Empty, 4, false, true);
                List<ResourceWorkItems> resourceWorkItems = workitemManager.LoadWorkItemsById(workItemType, workItem, string.Empty);
                List<ResourceAllocationMonthly> resourceAllocationMonthlies = allocMManager.Load(x => x.ResourceWorkItem == workItem);
                List<ProjectAllocation> projectAllocations = pManager.GetItemList(workItem);
                List<UGITTask> usertasks = null;
                foreach (var user in users)
                {
                    usertasks = uGITTaskManager.Load(x => x.TicketId == workItem && x.AssignedTo.Contains(user));
                    if (usertasks.Count == 0)
                    {
                        allocManager.Delete(allAllocataions.Where(x => x.Resource.EqualsIgnoreCase(user)).ToList());
                        allocMManager.Delete(resourceAllocationMonthlies.Where(x => x.Resource.EqualsIgnoreCase(user)).ToList());
                        workitemManager.Delete(resourceWorkItems.Where(x => x.Resource.EqualsIgnoreCase(user)).ToList());
                        pManager.Delete(projectAllocations.Where(x => x.Resource.EqualsIgnoreCase(user)).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "ERROR Deleting Un-Assigned User from project planned allocation");
            }
        }

        public List<UGITTask> LoadNPRResourceList(string projectPublicId)
        {
            UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
            List<UGITTask> nprTasks = new List<UGITTask>();
            DataTable resourceList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            string rQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, projectPublicId);
            DataRow[] nprResources = resourceList.Select(rQuery);

            if (nprResources != null && nprResources.Count() > 0)
            {
                foreach (DataRow spItem in nprResources)
                {
                    UGITTask tsk = new UGITTask();

                    tsk.ID = UGITUtility.StringToInt(nprResources[0][DatabaseObjects.Columns.Id]);
                    tsk.Title = Convert.ToString(nprResources[0][DatabaseObjects.Columns.UserSkillLookup]);
                    tsk.StartDate = UGITUtility.StringToDateTime(spItem[DatabaseObjects.Columns.AllocationStartDate]);
                    tsk.DueDate = UGITUtility.StringToDateTime(spItem[DatabaseObjects.Columns.AllocationEndDate]);
                    tsk.EstimatedHours = UGITUtility.StringToInt(spItem[DatabaseObjects.Columns.EstimatedHours]);
                    tsk.PercentComplete = UGITUtility.StringToDouble(spItem[DatabaseObjects.Columns.TicketNoOfFTEs]);

                    List<string> userLookups = Convert.ToString(spItem[DatabaseObjects.Columns.RequestedResources]).Split(',').ToList();
                    if (userLookups != null)
                        tsk.AssignedTo = Convert.ToString(spItem[DatabaseObjects.Columns.RequestedResources]);

                    nprTasks.Add(tsk);
                }
            }
            return nprTasks;
        }

        //new methods related to update resoruce allocation
        #region allocation modification
        public List<ResourceWorkItems> LoadWorkItemsByIdAndUser(string workItemTypeId, string workItemId, int userId)
        {
            List<ResourceWorkItems> rWorkItems = new List<ResourceWorkItems>();
            List<string> requiredQuery = new List<string>();
            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItemType, workItemTypeId));

            if (userId == 0)
            {
                requiredQuery.Add(string.Format("{0} is null", DatabaseObjects.Columns.Resource));
            }
            else
            {
                requiredQuery.Add(string.Format("{0} is not null", DatabaseObjects.Columns.Resource));
            }

            requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.WorkItem, workItemId));

            string rQuery = string.Join(" and ", requiredQuery);
            DataTable workItemList = workitemManager.GetDataTable();// GetTableDataManager.Get.GetSPList(DatabaseObjects.Tables.ResourceWorkItems, spWeb);
            if (workItemList.Rows.Count > 0)
            {
                DataRow[] resultCollection = workItemList.Select(rQuery);
                if (resultCollection != null && resultCollection.Count() > 0)
                {
                    foreach (DataRow resultedItem in resultCollection)
                    {
                        ResourceWorkItems wItem = workitemManager.LoadResourceWorkItem(resultedItem);
                        rWorkItems.Add(wItem);
                    }
                }
            }
            return rWorkItems;
        }
        #endregion

        public void UpdateProjectPlannedAllocationByUser(List<UGITTask> tasks, string moduleName, string projectPublicId, bool IsTask)
        {
            List<string> userIdList = new List<string>();
            foreach (UGITTask item in tasks)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(item.AssignedTo)))
                {
                    if (item.AssignedTo.Contains(Constants.Separator6))
                        userIdList.AddRange(item.AssignedTo.Split(',').ToList());
                    else if (item.AssignedTo.Contains(Constants.Separator))
                        userIdList.AddRange(item.AssignedTo.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList());
                    else
                        userIdList.Add(item.AssignedTo);
                }
            }

            userIdList = userIdList.Distinct().ToList();
            //update Resource Allocation while Import task (project file .mpp) and from template.
            ThreadStart threadStartMethod = delegate () { UpdateProjectPlannedAllocation(string.Empty, tasks, userIdList, moduleName, projectPublicId, IsTask); };
            Thread sThread = new Thread(threadStartMethod);
            sThread.IsBackground = true;
            sThread.Start();
        }

        public void DeleteProjectPlannedAllocationByUnAssignedUsers(List<UGITTask> tasks, string moduleName, string projectPublicId, bool IsTask)
        {
            List<string> userIdList = new List<string>();
            foreach (UGITTask item in tasks)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(item.AssignedTo)))
                {
                    if (item.AssignedTo.Contains(Constants.Separator6))
                        userIdList = item.AssignedTo.Split(',').ToList();
                    else if (item.AssignedTo.Contains(Constants.Separator))
                        userIdList = item.AssignedTo.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    else
                        userIdList.Add(item.AssignedTo);
                }
            }

            userIdList = userIdList.Distinct().ToList();
            ThreadStart threadStartMethod = delegate () { DeleteProjectPlannedAllocationUnAssignedUsers(string.Empty, tasks, userIdList, moduleName, projectPublicId, IsTask); };
            Thread sThread = new Thread(threadStartMethod);
            sThread.IsBackground = true;
            sThread.Start();
        }

        public void UpdateTaskAllocation(List<UGITTask> tasks, UGITTask task, List<string> users, string workItemType, string workItem)
        {
            ///to avoid Summry Task Allocation entry
            if (task.ChildCount > 0)
            {
                return;
            }
            List<RResourceAllocation> allAllocations = this.LoadByWorkItem(workItemType, workItem, string.Format("{0};#{1}", task.ID, task.Title), 4, false, true);

            if (allAllocations != null && allAllocations.Count > 0)
            {
                foreach (RResourceAllocation rAlloc in allAllocations)
                {
                    if (!users.Contains(rAlloc.Resource))
                    {
                        if (string.Format("{0};#{1}", task.ID, task.Title) == rAlloc.ResourceWorkItems.SubWorkItem)
                        {
                            this.Delete(rAlloc);
                            continue;
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(rAlloc.ResourceWorkItems.SubWorkItem))
                        {
                            this.Delete(rAlloc);
                            continue;
                        }
                    }

                    if (users.Count > 0 && !string.IsNullOrEmpty(rAlloc.Resource) && string.Format("{0};#{1}", task.ID, task.Title) == rAlloc.ResourceWorkItems.SubWorkItem)
                    {
                        this.Delete(rAlloc);
                    }
                }
            }

            int workingHrsInDay = uHelper.GetWorkingHoursInADay(_context);

            List<RResourceAllocation> allocations = this.LoadByWorkItem(workItemType, workItem, string.Format("{0};#{1}", task.ID, task.Title), 4, false, false);
            foreach (string user in users)
            {
                RResourceAllocation rAllocation = allocations.FirstOrDefault(x => x.Resource == user);

                if (rAllocation == null)
                {
                    rAllocation = new RResourceAllocation();
                    rAllocation.ResourceWorkItems = workitemManager.Get(x => x.Resource == user);
                    rAllocation.ResourceWorkItems.WorkItemType = workItemType;
                    rAllocation.ResourceWorkItems.WorkItem = workItem;
                }

                rAllocation.ResourceWorkItems.SubWorkItem = string.Format("{0};#{1}", task.ID, task.Title);
                UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
                List<UGITAssignTo> listAssignToPct = uGITTaskManager.GetUGITAssignPct(task.AssignToPct);
                UserProfileManager userProfileManager = new UserProfileManager(_context);
                UserProfile spUser = userProfileManager.GetUserById(user);
                UGITAssignTo assignTo = null;
                if (spUser == null)
                    continue;

                assignTo = listAssignToPct.FirstOrDefault(x => x.LoginName == spUser.UserName);


                if (assignTo != null && !string.IsNullOrEmpty(assignTo.Percentage))
                {
                    if (rAllocation.PctAllocation != int.Parse(assignTo.Percentage))
                        rAllocation.PctAllocation = int.Parse(assignTo.Percentage);
                }

                if (task.StartDate != null && task.StartDate != DateTime.MinValue)
                {
                    if (rAllocation.AllocationStartDate != task.StartDate)
                        rAllocation.AllocationStartDate = task.StartDate;
                }

                if (task.DueDate != null && task.DueDate != DateTime.MinValue)
                {
                    if (rAllocation.AllocationEndDate != task.DueDate)
                        rAllocation.AllocationEndDate = task.DueDate;
                }

                rAllocation.Resource = user;
                //if (spUser != null)
                //    rAllocation.ResourceName = spUser.LoginName;



                this.Save(rAllocation);

                if (rAllocation.ResourceWorkItemLookup > 0)
                {
                    string webUrl = HttpContext.Current.Request.Url.ToString();
                    //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                    ThreadStart threadStartMethod = delegate ()
                    {
                        RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, rAllocation.ResourceWorkItemLookup);
                    };
                    Thread sThread = new Thread(threadStartMethod);
                    sThread.IsBackground = true;
                    sThread.Start();
                }
            }

            if (users.Count == 0)
            {
                RResourceAllocation rAllocation = null;
                if (allocations != null && allAllocations.Count > 0)
                    rAllocation = allocations.FirstOrDefault(x => x.ResourceWorkItems.WorkItem == workItem && x.ResourceWorkItems.SubWorkItem.Contains(task.Title));

                if (rAllocation == null)
                {
                    rAllocation = new RResourceAllocation();

                    List<ResourceWorkItems> wItems = workitemManager.LoadWorkItemsById(workItemType, workItem, string.Format("{0};#{1}", task.ID, task.Title));

                    if (wItems != null && wItems.Count > 0)
                    {
                        rAllocation.ResourceWorkItems = wItems[0];
                    }
                    else
                    {

                        rAllocation.ResourceWorkItems = new ResourceWorkItems("");
                        rAllocation.ResourceWorkItems.WorkItemType = workItemType;
                        rAllocation.ResourceWorkItems.WorkItem = workItem;
                    }
                }

                rAllocation.ResourceWorkItems.SubWorkItem = string.Format("{0};#{1}", task.ID, task.Title);

                if (task.StartDate != null && task.StartDate != DateTime.MinValue)
                {
                    rAllocation.AllocationStartDate = task.StartDate;
                }

                if (task.DueDate != null && task.DueDate != DateTime.MinValue)
                {
                    rAllocation.AllocationEndDate = task.DueDate;
                }

                rAllocation.Resource = "";
                // rAllocation.ResourceName = string.Empty;

                int taskWorkingDay = uHelper.GetTotalWorkingDaysBetween(_context, task.StartDate, task.DueDate);
                int totalWorkingHrs = (taskWorkingDay * workingHrsInDay);
                int pctAlloc = totalWorkingHrs > 0 ? (int)(task.EstimatedHours * 100) / totalWorkingHrs : 0;
                if (rAllocation.PctAllocation != pctAlloc && pctAlloc > 0)
                {
                    rAllocation.PctAllocation = pctAlloc;
                    string webUrl = HttpContext.Current.Request.Url.ToString();

                    //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                    ThreadStart threadStartMethod = delegate ()
                    {
                        RMMSummaryHelper.UpdateRMMMonthDistributionForUnassigned(_context, rAllocation);
                    }; //rAllocation.WorkItemID
                    Thread sThread = new Thread(threadStartMethod);
                    sThread.IsBackground = true;
                    sThread.Start();
                }
            }
        }

        public void UpdateTaskAllocationWithoutThread(List<UGITTask> tasks, UGITTask task, List<string> users, string workItemType, string workItem)
        {
            List<RResourceAllocation> allAllocations = this.LoadByWorkItem(workItemType, workItem, string.Format("{0};#{1}", task.ID, task.Title), 4, false, true);

            if (allAllocations != null && allAllocations.Count > 0)
            {
                foreach (RResourceAllocation rAlloc in allAllocations)
                {
                    if (!users.Contains(rAlloc.Resource))
                    {
                        if (string.Format("{0};#{1}", task.ID, task.Title) == rAlloc.ResourceWorkItems.SubWorkItem)
                        {
                            this.Delete(rAlloc);
                            continue;
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(rAlloc.ResourceWorkItems.SubWorkItem))
                        {
                            this.Delete(rAlloc);
                            continue;
                        }
                    }

                    if (users.Count > 0 && string.IsNullOrEmpty(rAlloc.Resource) && string.Format("{0};#{1}", task.ID, task.Title) == rAlloc.ResourceWorkItems.SubWorkItem)
                    {
                        this.Delete(rAlloc);
                    }
                }
            }

            int workingHrsInDay = uHelper.GetWorkingHoursInADay(_context);

            List<RResourceAllocation> allocations = this.LoadByWorkItem(workItemType, workItem, string.Format("{0};#{1}", task.ID, task.Title), 4, false, false);
            foreach (string user in users)
            {
                RResourceAllocation rAllocation = allocations.FirstOrDefault(x => x.Resource == user);
                List<UGITTask> userTasks = tasks.Where(x => x.ChildCount == 0 && x.AssignedTo != null && x.AssignedTo.Split(',').ToList().Exists(y => y == user)).ToList();

                if (rAllocation == null)
                {
                    rAllocation = new RResourceAllocation();
                    rAllocation.ResourceWorkItems = workitemManager.Get(x => x.SubWorkItem == user);
                    rAllocation.ResourceWorkItems.WorkItemType = workItemType;
                    rAllocation.ResourceWorkItems.WorkItem = workItem;
                }

                rAllocation.ResourceWorkItems.SubWorkItem = string.Format("{0};#{1}", task.ID, task.Title);
                UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
                List<UGITAssignTo> listAssignToPct = uGITTaskManager.GetUGITAssignPct(task.AssignToPct);
                UserProfileManager userProfileManager = new UserProfileManager(_context);
                UserProfile spUser = userProfileManager.GetUserById(users[0]);
                UGITAssignTo assignTo = null;
                if (spUser == null)
                    continue;

                assignTo = listAssignToPct.FirstOrDefault(x => x.LoginName == spUser.UserName);


                if (assignTo != null && !string.IsNullOrEmpty(assignTo.Percentage))
                {
                    if (rAllocation.PctAllocation != int.Parse(assignTo.Percentage))
                        rAllocation.PctAllocation = int.Parse(assignTo.Percentage);
                }

                if (task.StartDate != null && task.StartDate != DateTime.MinValue)
                {
                    if (rAllocation.AllocationStartDate != task.StartDate)
                        rAllocation.AllocationStartDate = task.StartDate;
                }

                if (task.DueDate != null && task.DueDate != DateTime.MinValue)
                {
                    if (rAllocation.AllocationEndDate != task.DueDate)
                        rAllocation.AllocationEndDate = task.DueDate;
                }

                rAllocation.Resource = user;
                //if (spUser != null)
                //    rAllocation.ResourceName = spUser.UserName;

                double totalUserProjectHrs = 0;
                foreach (UGITTask t in userTasks)
                {
                    double totalPercentage = 0;
                    // bool isCheckPercentage = false;

                    List<UGITAssignTo> lstAssignToPct = uGITTaskManager.GetUGITAssignPct(t.AssignToPct);

                    if (lstAssignToPct.Count > 0)
                        totalPercentage = lstAssignToPct.Sum(x => UGITUtility.StringToDouble(x.Percentage));

                    foreach (UGITAssignTo ugitAssignToItem in lstAssignToPct)
                    {

                        if (ugitAssignToItem.LoginName == spUser.UserName && totalPercentage > 0)
                        {
                            totalUserProjectHrs += (UGITUtility.StringToDouble(ugitAssignToItem.Percentage) * t.EstimatedHours / (totalPercentage * userTasks.Count));
                        }
                    }
                }

                double totalProjectLeafTaskHrs = 0;
                totalProjectLeafTaskHrs = tasks.Where(x => x.ChildCount == 0).Sum(x => x.EstimatedHours);
                if (totalProjectLeafTaskHrs > 0)
                {
                    rAllocation.PctPlannedAllocation = Convert.ToInt32(totalUserProjectHrs * 100 / totalProjectLeafTaskHrs);
                }

                this.Save(rAllocation);
                if (rAllocation.ResourceWorkItemLookup > 0)
                {
                    RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, rAllocation.ResourceWorkItemLookup);
                }
            }

            if (users.Count == 0)
            {
                RResourceAllocation rAllocation = null;
                if (allocations != null && allAllocations.Count > 0)
                    rAllocation = allocations.FirstOrDefault(x => x.ResourceWorkItems.WorkItem == workItem && x.ResourceWorkItems.SubWorkItem.Contains(task.Title));

                if (rAllocation == null)
                {
                    rAllocation = new RResourceAllocation();
                    List<ResourceWorkItems> wItems = workitemManager.LoadWorkItemsById(workItemType, workItem, string.Format("{0};#{1}", task.ID, task.Title));

                    if (wItems != null && wItems.Count > 0)
                    {
                        rAllocation.ResourceWorkItems = wItems[0];
                    }
                    else
                    {

                        rAllocation.ResourceWorkItems = new ResourceWorkItems("");
                        rAllocation.ResourceWorkItems.WorkItemType = workItemType;
                        rAllocation.ResourceWorkItems.WorkItem = workItem;
                    }
                }

                rAllocation.ResourceWorkItems.SubWorkItem = string.Format("{0};#{1}", task.ID, task.Title);

                if (task.StartDate != null && task.StartDate != DateTime.MinValue)
                {
                    rAllocation.AllocationStartDate = task.StartDate;
                }

                if (task.DueDate != null && task.DueDate != DateTime.MinValue)
                {
                    rAllocation.AllocationEndDate = task.DueDate;
                }

                rAllocation.Resource = "";
                // rAllocation.ResourceName = string.Empty;

                int taskWorkingDay = uHelper.GetTotalWorkingDaysBetween(_context, task.StartDate, task.DueDate);
                int totalWorkingHrs = (taskWorkingDay * workingHrsInDay);
                int pctAlloc = totalWorkingHrs > 0 ? (int)(task.EstimatedHours * 100) / totalWorkingHrs : 0;
                if (rAllocation.PctAllocation != pctAlloc && pctAlloc > 0)
                {
                    rAllocation.PctAllocation = pctAlloc;
                    string webUrl = HttpContext.Current.Request.Url.ToString();// spWeb.Url;
                    RMMSummaryHelper.UpdateRMMMonthDistributionForUnassigned(_context, rAllocation);
                }
            }
        }

        public void UpdateProjectPlannedAllocationWithoutThread(List<UGITTask> tasks, List<string> users, string workItemType, string workItem, List<string> existingUser)
        {

            List<RResourceAllocation> allocations = this.LoadByWorkItem(workItemType, workItem, null, 4, false, true);
            Dictionary<string, RResourceAllocation> updatedRA = new Dictionary<string, RResourceAllocation>();
            users = users.Distinct().ToList();

            DataRow projectItem = Ticket.GetCurrentTicket(_context, workItemType, workItem);

            DateTime projectStartDate = DateTime.MinValue;
            DateTime projectEndDate = DateTime.MinValue;

            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualStartDate) && projectItem[DatabaseObjects.Columns.TicketActualStartDate] != DBNull.Value)
                projectStartDate = UGITUtility.StringToDateTime(projectItem[DatabaseObjects.Columns.TicketActualStartDate]);

            if (UGITUtility.IsSPItemExist(projectItem, DatabaseObjects.Columns.TicketActualCompletionDate) && projectItem[DatabaseObjects.Columns.TicketActualCompletionDate] != DBNull.Value)
                projectEndDate = UGITUtility.StringToDateTime(projectItem[DatabaseObjects.Columns.TicketActualCompletionDate]);

            int workingHrsInDay = uHelper.GetWorkingHoursInADay(_context);
            UGITTaskManager uGITTaskManagerStore = new UGITTaskManager(_context);
            UserProfileManager userProfileManager = new UserProfileManager(_context);
            if (existingUser != null)
            {
                #region existing assign users

                List<string> recalculateUser = new List<string>();
                foreach (string useritem in existingUser)
                {
                    if (!users.Contains(useritem))
                    {
                        recalculateUser.Add(useritem);
                    }
                }

                foreach (string user in recalculateUser)
                {
                    List<UGITTask> userTasks = tasks.Where(x => x.AssignedTo != null && x.AssignedTo.Split(',').ToList().Exists(y => y == user)).ToList();

                    DateTime allocationStartDate = (userTasks == null || userTasks.Count < 1) ? DateTime.Now : userTasks.Min(x => x.StartDate);
                    DateTime allocationEndDate = (userTasks == null || userTasks.Count < 1) ? DateTime.Now : userTasks.Max(x => x.DueDate);

                    double totalPorjectWorkingHrs = 0;
                    double totalUserProjectHrs = 0;
                    double pctAllocation = 0;
                    double totalUserProjectUtilizationHrs = 0;
                    foreach (UGITTask task in userTasks)
                    {
                        double totalPercentage = 0;
                        // bool isCheckPercentage = false;

                        List<UGITAssignTo> listAssignToPct = uGITTaskManagerStore.GetUGITAssignPct(task.AssignToPct);

                        if (listAssignToPct.Count > 0)
                            totalPercentage = listAssignToPct.Sum(x => UGITUtility.StringToDouble(x.Percentage));

                        foreach (UGITAssignTo ugitAssignToItem in listAssignToPct)
                        {
                            UserProfile spUser = userProfileManager.GetUserById(user);
                            if (spUser != null && ugitAssignToItem.LoginName == spUser.UserName && totalPercentage > 0)
                            {
                                DateTime sDate = task.StartDate;
                                DateTime eDate = task.DueDate;

                                int taskWorkingDaysPctAllocation = uHelper.GetTotalWorkingDaysBetween(_context, sDate, eDate);
                                totalUserProjectHrs += (UGITUtility.StringToDouble(ugitAssignToItem.Percentage) * taskWorkingDaysPctAllocation * workingHrsInDay) / totalPercentage;

                                totalUserProjectUtilizationHrs += UGITUtility.StringToDouble(ugitAssignToItem.Percentage) * task.EstimatedHours / totalPercentage;
                            }
                        }
                    }

                    if (userTasks != null && userTasks.Count > 0)
                    {
                        DateTime pusDate = userTasks.Min(x => x.StartDate);
                        DateTime pueDate = userTasks.Max(x => x.DueDate);

                        int taskWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, pusDate, pueDate);
                        totalPorjectWorkingHrs = taskWorkingDays * workingHrsInDay;
                    }


                    RResourceAllocation allocation = allocations.FirstOrDefault(x => x.Resource == user);
                    if (allocation == null)
                    {
                        allocation = new RResourceAllocation();
                        allocation.ResourceWorkItems = workitemManager.Get(x => x.Resource == user);
                        allocation.ResourceWorkItems.WorkItemType = workItemType;
                        allocation.ResourceWorkItems.WorkItem = workItem;
                        allocation.ResourceWorkItems.SubWorkItem = string.Empty;
                        allocation.Resource = user;
                    }

                    // calculate the project allocation for user.
                    pctAllocation = (totalUserProjectHrs * 100) / totalPorjectWorkingHrs;
                    allocation.PctAllocation = (int)pctAllocation;

                    allocation.AllocationStartDate = allocationStartDate;
                    allocation.AllocationEndDate = allocationEndDate;

                    //calculate the user utilization.
                    double totalProjectLeafTaskHrs = 0;
                    totalProjectLeafTaskHrs = tasks.Where(x => x.ChildCount == 0).Sum(x => x.EstimatedHours);
                    if (totalProjectLeafTaskHrs > 0)
                    {
                        allocation.PctPlannedAllocation = Convert.ToInt32(totalUserProjectUtilizationHrs * 100 / totalProjectLeafTaskHrs);
                    }

                    if (allocation.PctAllocation == 0 && allocation.PctPlannedAllocation == 0)
                        this.Delete(allocation);
                    else
                    {
                        this.Save(allocation);
                        updatedRA.Add(string.Format("updated{0}", user), allocation);

                        RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, allocation.ResourceWorkItemLookup);
                    }
                }
                #endregion
            }

            #region assign users
            foreach (string user in users)
            {
                List<UGITTask> userTasks = tasks.Where(x => x.AssignedTo != null && x.AssignedTo.Split(',').ToList().Exists(y => y == user)).ToList();

                DateTime allocationStartDate = (userTasks == null || userTasks.Count < 1) ? DateTime.Now : userTasks.Min(x => x.StartDate);
                DateTime allocationEndDate = (userTasks == null || userTasks.Count < 1) ? DateTime.Now : userTasks.Max(x => x.DueDate);

                double totalPorjectWorkingHrs = 0;
                double totalUserProjectHrs = 0;
                double pctAllocation = 0;
                double totalUserProjectUtilizationHrs = 0;
                foreach (UGITTask task in userTasks)
                {
                    double totalPercentage = 0;
                    // bool isCheckPercentage = false;
                    List<UGITAssignTo> listAssignToPct = uGITTaskManagerStore.GetUGITAssignPct(task.AssignToPct);

                    if (listAssignToPct.Count > 0)
                        totalPercentage = listAssignToPct.Sum(x => UGITUtility.StringToDouble(x.Percentage));

                    foreach (UGITAssignTo ugitAssignToItem in listAssignToPct)
                    {
                        UserProfile spUser = userProfileManager.GetUserById(user);
                        if (spUser != null && ugitAssignToItem.LoginName == spUser.UserName && totalPercentage > 0)
                        {
                            DateTime sDate = task.StartDate;
                            DateTime eDate = task.DueDate;

                            int taskWorkingDaysPctAllocation = uHelper.GetTotalWorkingDaysBetween(_context, sDate, eDate);
                            totalUserProjectHrs += (UGITUtility.StringToDouble(ugitAssignToItem.Percentage) * taskWorkingDaysPctAllocation * workingHrsInDay) / totalPercentage;

                            totalUserProjectUtilizationHrs += UGITUtility.StringToDouble(ugitAssignToItem.Percentage) * task.EstimatedHours / totalPercentage;
                        }
                    }
                }

                if (userTasks != null && userTasks.Count > 0)
                {
                    DateTime pusDate = userTasks.Min(x => x.StartDate);
                    DateTime pueDate = userTasks.Max(x => x.DueDate);

                    int taskWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, pusDate, pueDate);
                    totalPorjectWorkingHrs = taskWorkingDays * workingHrsInDay;
                }

                RResourceAllocation allocation = allocations.FirstOrDefault(x => x.Resource == user);
                if (allocation == null)
                {
                    allocation = new RResourceAllocation();
                    allocation.ResourceWorkItems = workitemManager.Get(x => x.Resource == user);
                    allocation.ResourceWorkItems.WorkItemType = workItemType;
                    allocation.ResourceWorkItems.WorkItem = workItem;
                    allocation.ResourceWorkItems.SubWorkItem = string.Empty;
                    allocation.Resource = user;
                }

                // calculate the project allocation for user.
                pctAllocation = (totalUserProjectHrs * 100) / totalPorjectWorkingHrs;
                allocation.PctAllocation = (int)pctAllocation;

                allocation.AllocationStartDate = allocationStartDate;
                allocation.AllocationEndDate = allocationEndDate;

                //calculate the user utilization.
                double totalProjectLeafTaskHrs = 0;
                totalProjectLeafTaskHrs = tasks.Where(x => x.ChildCount == 0).Sum(x => x.EstimatedHours);
                if (totalProjectLeafTaskHrs > 0)
                {
                    allocation.PctPlannedAllocation = Convert.ToInt32(totalUserProjectUtilizationHrs * 100 / totalProjectLeafTaskHrs);
                }
                this.Save(allocation);
                updatedRA.Add(string.Format("updated{0}", user), allocation);
                RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, allocation.ResourceWorkItemLookup);
            }
            #endregion

            #region Unassigned Tasks
            RResourceAllocation rAllocation = null;
            rAllocation = new RResourceAllocation();
            List<ResourceWorkItems> wItems = workitemManager.LoadWorkItemsById(workItemType, workItem, string.Empty);
            ResourceWorkItems newItems = wItems.FirstOrDefault(x => x.WorkItem == workItem && !string.IsNullOrWhiteSpace(x.Resource));

            List<UGITTask> UnAssignuserTasks = tasks.Where(x => x.AssignToPct == null || x.AssignToPct == string.Empty).ToList();
            //min and max date of unassign task
            DateTime allocationUnAssignStartDate = (UnAssignuserTasks == null || UnAssignuserTasks.Count < 1) ? DateTime.Now : UnAssignuserTasks.Min(x => x.StartDate);
            DateTime allocationUnAssignEndDate = (UnAssignuserTasks == null || UnAssignuserTasks.Count < 1) ? DateTime.Now : UnAssignuserTasks.Max(x => x.DueDate);

            if (newItems != null)
            {
                rAllocation.ResourceWorkItems = newItems;
            }
            else
            {
                rAllocation.ResourceWorkItems = new ResourceWorkItems("");
                rAllocation.ResourceWorkItems.WorkItemType = workItemType;
                rAllocation.ResourceWorkItems.WorkItem = workItem;
            }


            rAllocation.AllocationStartDate = allocationUnAssignStartDate;
            rAllocation.AllocationEndDate = allocationUnAssignEndDate;

            rAllocation.Resource = "";
            //rAllocation.ResourceName = string.Empty;

            int projectWorkingDay = uHelper.GetTotalWorkingDaysBetween(_context, projectStartDate, projectEndDate);
            int totalWorkingHrs = (projectWorkingDay * workingHrsInDay);
            // int pctAlloc = totalWorkingHrs > 0 ? (int)(tasks.Where(x => x.ChildCount == 0 && (x.AssignToPct == null || x.AssignToPct == string.Empty)).Sum(x => x.EstimatedHours) * 100) / totalWorkingHrs : 0;
            int tempalloc = (int)(tasks.Where(x => x.ChildCount == 0 && (x.AssignToPct == null || x.AssignToPct == string.Empty)).Sum(x => x.EstimatedHours) * 100);
            int pctAlloc = totalWorkingHrs > 0 ? tempalloc / totalWorkingHrs : 0;

            if (rAllocation.PctAllocation != pctAlloc)
            {
                rAllocation.PctAllocation = pctAlloc;
                RMMSummaryHelper.UpdateRMMMonthDistributionForUnassigned(_context, rAllocation);
            }
            #endregion
        }


        public void DeleteProjectAllocations(string webUrl, string workItemType, string workItem, string subWorkItem)
        {

            List<ResourceWorkItems> resources = workitemManager.LoadWorkItemsById(workItemType, workItem, subWorkItem);
            foreach (ResourceWorkItems userWorkItem in resources)
            {
                string rallocationQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.ResourceWorkItemLookup, userWorkItem.ID);

                DataTable dtJResourceAllocation = this.GetDataTable().Select(rallocationQuery).CopyToDataTable();
                if (dtJResourceAllocation != null && dtJResourceAllocation.Rows.Count > 0)
                {
                    List<long> Ids = new List<long>();
                    foreach (DataRow dr in dtJResourceAllocation.Rows)
                    {
                        Ids.Add(UGITUtility.StringToLong(dr[DatabaseObjects.Columns.Id]));
                    }

                    RMMSummaryHelper.BatchDeleteListItems(_context, Ids, DatabaseObjects.Tables.ResourceAllocation);
                }
            }

        }
        public static void RefreshAllTicketsAllocations(ApplicationContext context)
        {
            if (IsProcessActive)
                return;

            List<string> errorTicketIds = new List<string>();
            string currentid = string.Empty;
            try
            {
                IsProcessActive = true;

                GlobalRoleManager roleManager = new GlobalRoleManager(context);
                ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
                string configModuleName = configManager.GetValue(ConfigConstants.RefreshAllocationForModule);
                List<GlobalRole> gRoles = roleManager.Load();
                ProjectAllocationManager projectAllocationManager = new ProjectAllocationManager(context);
                ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
                List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
                List<ProjectEstimatedAllocation> plannedAllocation = CRMProjAllocManager.Load(x => x.TicketId.StartsWith(configModuleName) && !string.IsNullOrEmpty(x.TicketId) && !string.IsNullOrEmpty(x.AssignedTo));
                var plannedlookups = plannedAllocation.ToLookup(x => x.TicketId);
                List<ProjectEstimatedAllocation> projectAllocations = CRMProjAllocManager.Load(x => x.TicketId.StartsWith(configModuleName) && !string.IsNullOrEmpty(x.TicketId) && !string.IsNullOrEmpty(x.AssignedTo));
                var lookups = projectAllocations.ToLookup(x => x.TicketId);

                foreach (var project in lookups)
                {
                    var pAllocations = project.ToList();
                    currentid = project.Key;
                    foreach (var allocation in pAllocations)
                    {
                        string roleName = string.Empty;
                        GlobalRole uRole = gRoles.FirstOrDefault(x => x.Id == allocation.Type);
                        if (uRole != null)
                            roleName = uRole.Name;
                        lstUserWithPercetage.Add(new UserWithPercentage() { EndDate = allocation.AllocationEndDate ?? DateTime.MinValue, StartDate = allocation.AllocationStartDate ?? DateTime.MinValue, Percentage = allocation.PctAllocation, UserId = allocation.AssignedTo, RoleTitle = roleName });
                    }
                    var taskManager = new UGITTaskManager(context);
                    List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(project.Key), project.Key);
                    List<string> lstUsers = pAllocations.Select(a => a.AssignedTo).ToList();
                    var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                    if (res == null || res.Count == 0)
                    {
                        ResourceAllocationManager.CPRResourceAllocationRefresh(context, uHelper.getModuleNameByTicketId(project.Key), project.Key, lstUserWithPercetage, null);
                    }
                    lstUserWithPercetage.Clear();
                }

                foreach (var project in plannedlookups)
                {
                    var pAllocations = project.ToList();
                    currentid = project.Key;
                    foreach (var allocation in pAllocations)
                    {
                        string roleName = string.Empty;
                        GlobalRole uRole = gRoles.FirstOrDefault(x => x.Id == allocation.Type);
                        if (uRole != null)
                            roleName = uRole.Name;
                        lstUserWithPercetage.Add(new UserWithPercentage() { EndDate = allocation.AllocationEndDate ?? DateTime.MinValue, StartDate = allocation.AllocationStartDate ?? DateTime.MinValue, Percentage = allocation.PctAllocation, UserId = allocation.AssignedTo, RoleTitle = roleName });
                    }
                    var taskManager = new UGITTaskManager(context);
                    List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(project.Key), project.Key);
                    List<string> lstUsers = pAllocations.Select(a => a.AssignedTo).ToList();
                    var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                    if (res == null || res.Count == 0)
                    {
                        ResourceAllocationManager.CPRResourceAllocationRefresh(context, uHelper.getModuleNameByTicketId(project.Key), project.Key, lstUserWithPercetage, null);
                    }
                    lstUserWithPercetage.Clear();
                }

                IsProcessActive = false;
            }
            catch (Exception ex)
            {
                errorTicketIds.Add(currentid);
                ULog.WriteException(ex);
            }
            finally
            {
                IsProcessActive = false;
                ULog.WriteLog("allocation for these ticket ids not run with error: " + UGITUtility.ConvertListToString(errorTicketIds, Constants.Separator6));
            }
        }
        #region CPR
        public static void CPRResourceAllocation(ApplicationContext context, string moduleName, string projectId, List<UserWithPercentage> lstUserWithPercetage)
        {
            List<string> lstOldNewUsers = new List<string>();
            ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(projectId), projectId, lstUserWithPercetage, lstOldNewUsers);
        }

        /// <summary>
        /// Method to create Resoruce Allocation while we save the Resoruce in Details tab of CPR.
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="moduleName"></param>
        /// <param name="project"></param>
        public static void CPRResourceAllocation(ApplicationContext context, string moduleName, string projectId, List<UserWithPercentage> lstUserWithPercetage, List<string> lstOldNewUsers, bool updateProjectGroups = true)
        {
            try
            {
                if (updateProjectGroups)
                {
                    ProjectEstimatedAllocationManager crmManager = new ProjectEstimatedAllocationManager(context);
                    crmManager.UpdateProjectGroups(moduleName, projectId);
                }
                ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
                List<RResourceAllocation> allocations = allocationManager.LoadByWorkItem(moduleName, projectId, null, 4, false, true);

                if (lstUserWithPercetage != null && lstUserWithPercetage.Count > 0)
                {
                    SaveAndDistributeAllocation(context, moduleName, projectId, allocations, lstUserWithPercetage, lstOldNewUsers);
                }
                ULog.WriteLog("Process Ended: " + projectId);
            }

            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }

        }
        public static void RefreshAllocation(ApplicationContext context, string moduleName, string projectId, List<UserWithPercentage> lstUserWithPercetage,
            List<string> lstOldNewUsers, ref ErrorCount errorCount, List<ProjectEstimatedAllocation> projectEstimatedAllocations = null,
            List<ResourceWorkItems> resourceWorkItems = null)
        {
            try
            {
                #region Insert and update resource allocation from project estmated allocation
                ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
                ResourceWorkItemsManager resourceWorkitemMGR = new ResourceWorkItemsManager(context);

                List<string> lstDistinctUserId = lstUserWithPercetage.Select(x => x.UserId).Distinct().ToList();
                List<RResourceAllocation> rResourceAllocations = new List<RResourceAllocation>();
                RResourceAllocation allocation = null;
                UserWithPercentage userWithPercentage = null;
                for (int i = 0; i < lstUserWithPercetage.Count; i++)
                {
                    userWithPercentage = lstUserWithPercetage[i];
                    allocation = new RResourceAllocation();
                    allocation.ResourceWorkItems = new ResourceWorkItems(userWithPercentage.UserId);
                    allocation.ResourceWorkItems.WorkItemType = moduleName;
                    allocation.ResourceWorkItems.WorkItem = projectId;
                    allocation.ResourceWorkItems.SubWorkItem = userWithPercentage.RoleTitle;
                    allocation.TicketID = projectId;
                    allocation.Resource = userWithPercentage.UserId;

                    allocation.PctAllocation = userWithPercentage.Percentage;
                    allocation.AllocationStartDate = userWithPercentage.StartDate;
                    allocation.AllocationEndDate = userWithPercentage.EndDate;
                    allocation.ProjectEstimatedAllocationId = userWithPercentage.ProjectEstiAllocId;
                    allocation.RoleId = userWithPercentage.RoleId;
                    allocation.EstStartDate = allocation.AllocationStartDate;
                    allocation.EstEndDate = allocation.AllocationEndDate;
                    allocation.PctEstimatedAllocation = allocation.PctAllocation;
                    allocation.SoftAllocation = userWithPercentage.SoftAllocation;
                    allocation.NonChargeable = userWithPercentage.NonChargeable;
                    //To update resourceworkitemlookup into project allocation table 
                    if (resourceWorkItems != null && allocation.ResourceWorkItems != null && !string.IsNullOrWhiteSpace(allocation.ResourceWorkItems.WorkItemType) && (!string.IsNullOrWhiteSpace(allocation.ResourceWorkItems.WorkItem) || !string.IsNullOrWhiteSpace(allocation.ResourceWorkItems.SubWorkItem)))
                    {
                        string workItemType = allocation.ResourceWorkItems.WorkItemType;
                        List<ResourceWorkItems> rWorkItems = null;
                        string subWorkItem = Convert.ToString(allocation.ResourceWorkItems.SubWorkItem);
                        if (!string.IsNullOrWhiteSpace(subWorkItem))
                            rWorkItems = resourceWorkItems.Where(x => (x.WorkItemType == allocation.ResourceWorkItems.WorkItemType || x.WorkItemType.Contains(allocation.ResourceWorkItems.WorkItemType))
                                                && x.WorkItem == allocation.ResourceWorkItems.WorkItem && x.SubWorkItem.EqualsIgnoreCase(subWorkItem)).ToList();
                        //In some work item records subworkitem is null, the below line of code will take care of it
                        if (rWorkItems == null || rWorkItems.Count == 0)
                            rWorkItems = resourceWorkItems.Where(x => (x.WorkItemType == allocation.ResourceWorkItems.WorkItemType || x.WorkItemType.Contains(allocation.ResourceWorkItems.WorkItemType))
                                                && x.WorkItem == allocation.ResourceWorkItems.WorkItem).ToList();

                        ResourceWorkItems rWorkItem = null;
                        if (rWorkItems != null && rWorkItems.Count > 0)
                        {
                            //changed from firstordefault to lastordefault based on created date, so that it will pick latest update workitem
                            rWorkItem = rWorkItems.OrderBy(x => x.Created).LastOrDefault(x => x.Resource == allocation.Resource || x.Resource == "0");

                            allocation.ResourceWorkItems.ID = rWorkItem.ID;
                            allocation.ResourceWorkItemLookup = rWorkItem.ID;
                        }
                        else
                        {
                            resourceWorkitemMGR.Insert(allocation.ResourceWorkItems);
                            allocation.ResourceWorkItemLookup = allocation.ResourceWorkItems.ID;
                        }

                    }


                    rResourceAllocations.Add(allocation);

                }
                #endregion

                //Insert bulk records into resource allocation
                if (rResourceAllocations != null && rResourceAllocations.Count > 0)
                {
                    bool success = allocationManager.InsertItems(rResourceAllocations);
                    if (!success)
                    {
                        errorCount.numError = rResourceAllocations.Count;
                        ULog.WriteLog("Error during resource allocation insertion");
                    }
                    else
                        errorCount.numInsertedResAllocations = rResourceAllocations.Count;
                }
            }

            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }

        }
        private static void SaveAndDistributeAllocation(ApplicationContext _context, string moduleName, string publicProjectId, List<RResourceAllocation> allocations, List<UserWithPercentage> lstUserWithPercetage, List<string> lstOldNewUsers = null)
        {
            try
            {
                ResourceAllocationManager allocationManager = new ResourceAllocationManager(_context);
                ModuleUserStatisticsManager moduleUserStatisticsManager = new ModuleUserStatisticsManager(_context);
                TicketManager ticketManager = new TicketManager(_context);
                List<string> lstDistinctUserId = lstUserWithPercetage.Select(x => x.UserId).Distinct().ToList();
                //Older users for which we need to update distribution
                List<string> oldUsers = new List<string>();
                if (lstOldNewUsers != null)
                    oldUsers = lstOldNewUsers.Where(x => !lstDistinctUserId.Contains(x)).ToList();

                DataRow row = ticketManager.GetByTicketIdFromCache(moduleName, publicProjectId);

                RResourceAllocation userAllocations = null;
                List<RResourceAllocation> rResourceAllocations = new List<RResourceAllocation>();

                foreach (UserWithPercentage user in lstUserWithPercetage)
                {
                    // Get allocation already exist or need to create 
                    userAllocations = allocations.FirstOrDefault(x => x.ProjectEstimatedAllocationId == user.ProjectEstiAllocId);

                    RResourceAllocation allocation = new RResourceAllocation();
                    if (userAllocations != null && userAllocations.ID > 0)
                    {
                        allocation = userAllocations;
                    }
                    else
                    {
                        allocation.ResourceWorkItems = new ResourceWorkItems(user.UserId);
                        allocation.ResourceWorkItems.WorkItemType = moduleName;
                        allocation.ResourceWorkItems.WorkItem = publicProjectId;
                        //allocation.ResourceWorkItems.SubWorkItem = user.Title;
                    }
                    allocation.TicketID = publicProjectId;
                    allocation.Resource = user.UserId;
                    allocation.PctAllocation = user.Percentage;
                    allocation.AllocationStartDate = user.StartDate;
                    allocation.AllocationEndDate = user.EndDate;
                    allocation.ProjectEstimatedAllocationId = user.ProjectEstiAllocId;
                    allocation.RoleId = user.RoleId;
                    allocation.SoftAllocation = user.SoftAllocation;
                    allocation.ResourceWorkItems.Resource = allocation.Resource;
                    allocation.ResourceWorkItems.SubWorkItem = user.RoleTitle;
                    allocation.ResourceWorkItems.Title = Convert.ToString(row[DatabaseObjects.Columns.Title]);
                    rResourceAllocations.Add(allocation);
                    allocationManager.Save(allocation);
                }

                //Update resource summary tables
                if (rResourceAllocations != null && rResourceAllocations.Count > 0)
                {
                    for (int i = 0; i < rResourceAllocations.Count; i++)
                    {
                        RResourceAllocation ralloc = rResourceAllocations[i];
                        RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, ralloc.ResourceWorkItemLookup);
                    }
                }
                moduleUserStatisticsManager.RefreshCache();
                //Update for all resources

            }

            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }

        public static void CPRResourceAllocationRefresh(ApplicationContext context, string moduleName, string projectId, List<UserWithPercentage> lstUserWithPercetage, List<string> lstOldNewUsers)
        {
            try
            {
                ProjectEstimatedAllocationManager crmManager = new ProjectEstimatedAllocationManager(context);
                crmManager.UpdateProjectGroups(moduleName, projectId);

                ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
                List<RResourceAllocation> allocations = allocationManager.LoadByWorkItem(moduleName, projectId, null, 4, false, true);

                if (lstUserWithPercetage != null && lstUserWithPercetage.Count > 0)
                {
                    SaveAndDistributeAllocationRefresh(context, moduleName, projectId, allocations, lstUserWithPercetage, lstOldNewUsers);
                }
            }

            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }

        }

        private static void SaveAndDistributeAllocationRefresh(ApplicationContext _context, string moduleName, string publicProjectId, List<RResourceAllocation> allocations, List<UserWithPercentage> lstUserWithPercetage, List<string> lstOldNewUsers = null)
        {
            try
            {
                ResourceAllocationManager allocationManager = new ResourceAllocationManager(_context);

                List<string> lstDistinctUserId = lstUserWithPercetage.Select(x => x.UserId).Distinct().ToList();
                //Older users for which we need to update distribution
                List<string> oldUsers = new List<string>();
                if (lstOldNewUsers != null)
                    oldUsers = lstOldNewUsers.Where(x => !lstDistinctUserId.Contains(x)).ToList();

                List<RResourceAllocation> userAllocations = new List<RResourceAllocation>();
                foreach (string user in lstDistinctUserId)
                {
                    userAllocations = allocations.Where(x => x.Resource == user).ToList();

                    //remove allocation if that are more than one 
                    RResourceAllocation allocation = userAllocations.FirstOrDefault();
                    if (userAllocations.Count > 1)
                    {
                        userAllocations.RemoveAt(0);
                        foreach (RResourceAllocation r in userAllocations)
                            allocationManager.Delete(r);
                    }


                    double workingDays = 0;
                    double totalWorkingDays = 0;
                    double allocationPercentage = 0;

                    List<UserWithPercentage> userAllocs = lstUserWithPercetage.Where(x => x.UserId == user.ToString()).ToList();
                    foreach (UserWithPercentage uPct in userAllocs)
                    {
                        workingDays += (uPct.Percentage * uHelper.GetTotalWorkingDaysBetween(_context, uPct.StartDate, uPct.EndDate)) / 100;
                        totalWorkingDays += uHelper.GetTotalWorkingDaysBetween(_context, uPct.StartDate, uPct.EndDate);
                    }
                    if (totalWorkingDays > 0)
                        allocationPercentage = (workingDays * 100) / totalWorkingDays;
                    DateTime startDate = userAllocs.Min(x => x.StartDate);
                    DateTime endDate = userAllocs.Max(x => x.EndDate);
                    string roleTile = userAllocs.FirstOrDefault().RoleTitle;

                    if (allocation == null)
                    {
                        allocation = new RResourceAllocation();
                        allocation.ResourceWorkItems = new ResourceWorkItems(user.ToString());
                        allocation.ResourceWorkItems.WorkItemType = moduleName;
                        allocation.ResourceWorkItems.WorkItem = publicProjectId;
                        allocation.ResourceWorkItems.SubWorkItem = null;
                        allocation.Resource = user.ToString();
                    }

                    if (allocation.ResourceWorkItems == null)
                        allocation.ResourceWorkItems = new ResourceWorkItems(user.ToString());
                    allocation.ResourceWorkItems.WorkItem = publicProjectId;
                    allocation.ResourceWorkItems.WorkItemType = moduleName;

                    if (allocation.ResourceWorkItems.WorkItem != null)
                        allocation.ResourceWorkItems.SubWorkItem = roleTile;

                    allocation.PctAllocation = allocationPercentage;
                    allocation.AllocationStartDate = startDate;
                    allocation.AllocationEndDate = endDate;

                    allocationManager.Save(allocation);

                    RMMSummaryHelper.DistributeAllocationByMonth(_context, allocation.ResourceWorkItemLookup);
                }

                //Clean up user allocation who is deleted from allocation
                foreach (string oldUser in oldUsers)
                {
                    userAllocations = allocations.Where(x => x.Resource == oldUser).ToList();
                    if (userAllocations.Count <= 0)
                        continue;

                    ResourceWorkItems workItem = userAllocations.FirstOrDefault().ResourceWorkItems;
                    if (workItem != null)
                        RMMSummaryHelper.CleanAllocation(_context, workItem, cleanEstimated: true);
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }
        #endregion

        //new method for increase the performance..
        public int AllocationPercentageWithProjectType(DataTable rawTable, string userID, int tense, DateTime startDate, DateTime endDate, bool minOneDay, List<string> type, DataTable projectDatatable, bool IsRunScenario, int viewId, bool IsAssignedAllocation = false, List<ResourceWorkItems> resourceWorkItemList = null)
        {
            UserProfileManager userManager = new UserProfileManager(_context);
            DataTable dttempnewProjectitems = new DataTable();
            dttempnewProjectitems.Columns.Add("TicketTitle", typeof(string));
            dttempnewProjectitems.Columns.Add("TicketId", typeof(string));
            UserProfile user = userManager.LoadById(userID);
            DataTable table = null;
            if (rawTable != null && rawTable.Rows.Count > 0)
            {
                DataRow[] rows = rawTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user.Id));

                if (rows != null && rows.Length > 0)
                    table = rows.CopyToDataTable();
            }

            //   DataTable table = rawTable;

            if (table != null && table.Rows.Count > 0)
            {
                DateTime minDate = startDate; //table.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["AllocationStartDate"])).Min();
                DateTime maxDate = endDate; //table.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["AllocationEndDate"])).Max();
                int totalNoOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, minDate, maxDate, minOneDay);
                double alloc = 0;
                int workingHrs = uHelper.GetWorkingHoursInADay(_context);
                //if (table.Columns.Contains(DatabaseObjects.Columns.AllocationStartDate) && table.Columns.Contains(DatabaseObjects.Columns.AllocationEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PctAllocation) && table.Columns.Contains(DatabaseObjects.Columns.PctPlannedAllocation))
                if (table.Columns.Contains(DatabaseObjects.Columns.EstStartDate) && table.Columns.Contains(DatabaseObjects.Columns.EstEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PlannedStartDate) && table.Columns.Contains(DatabaseObjects.Columns.PlannedEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PctEstimatedAllocation) && table.Columns.Contains(DatabaseObjects.Columns.PctPlannedAllocation))
                {
                    ResourceWorkItems workItem = null;
                    DateTime allocatedstdt;
                    DateTime allocatedenddt;
                    double pctAlloc;
                    int noOfDays;
                    long workitemId = 0;
                    string moduleName = string.Empty;
                    string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>",
                                                       DatabaseObjects.Columns.Id, DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem);
                    foreach (DataRow dr in table.Rows)
                    {
                        //block for filter by type.
                        #region filter by type
                        if (type != null && type.Count > 0 && !type.Contains("ALL"))
                        {
                            LookupValue workitemlookup = new LookupValue(Convert.ToInt64(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]), Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]));

                            if (workitemlookup != null)
                            {
                                if (workitemId != Convert.ToInt64(workitemlookup.ID))
                                {
                                    workitemId = Convert.ToInt64(workitemlookup.ID);
                                    if (resourceWorkItemList == null)
                                    {
                                        workItem = workitemManager.LoadByID(workitemId);  // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ResourceWorkItems, query);
                                        if (workItem != null)
                                            moduleName = Convert.ToString(workItem.WorkItemType);
                                    }
                                    else
                                    {
                                        workItem = resourceWorkItemList.FirstOrDefault(x => x.ID == workitemId);  // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ResourceWorkItems, query);
                                        if (workItem != null)
                                            moduleName = Convert.ToString(workItem.WorkItemType);
                                    }
                                }

                                if (!type.Contains(moduleName))
                                    continue;
                            }
                        }
                        #endregion

                        //DateTime allocatedstdt;
                        //DateTime allocatedenddt;
                        pctAlloc = 0;
                        if (IsAssignedAllocation)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedEndDate])))
                                continue;

                            pctAlloc = UGITUtility.StringToDouble(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                            allocatedstdt = Convert.ToDateTime(dr[DatabaseObjects.Columns.PlannedStartDate]);
                            allocatedenddt = Convert.ToDateTime(dr[DatabaseObjects.Columns.PlannedEndDate]);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                continue;

                            pctAlloc = UGITUtility.StringToDouble(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                            allocatedstdt = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate]);
                            allocatedenddt = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate]);
                        }

                        if (allocatedstdt <= startDate && allocatedenddt <= endDate && allocatedenddt > startDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, startDate, allocatedenddt, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt >= startDate && allocatedenddt <= endDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, allocatedstdt, allocatedenddt, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt >= startDate && allocatedenddt >= endDate && allocatedenddt > startDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, allocatedstdt, endDate, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt <= startDate && allocatedenddt >= endDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, startDate, endDate, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else
                        {
                            alloc += 0;
                        }
                    }
                }

                if (totalNoOfWorkingDays > 0)
                    return Convert.ToInt32(Math.Ceiling((alloc * 100) / totalNoOfWorkingDays));
            }

            return 0;
        }

        public int ProjectAllocationPercentage(string userID, int tense, DateTime startDate, DateTime endDate, string projectId)
        {
            return ProjectAllocationPercentage(userID, tense, startDate, endDate, projectId, true);
        }

        public int ProjectAllocationPercentage(string userID, int tense, DateTime startDate, DateTime endDate, string projectId, bool minOneDay, List<ResourceWorkItems> lstResourceWorkItem = null)
        {
            List<long> workItemid = new List<long>();
            if (lstResourceWorkItem == null || lstResourceWorkItem.Count == 0)
                lstResourceWorkItem = workitemManager.LoadWorkItemsById(uHelper.getModuleNameByTicketId(projectId), projectId, null, false, true);
            foreach (ResourceWorkItems rworkItem in lstResourceWorkItem)
            {
                workItemid.Add(rworkItem.ID);
            }
            DataTable table = LoadRawTableByProjectResource(userID, tense, workItemid);
            //DataTable table = LoadRawTableByProjectResource(userID, tense);
            if (table != null && table.Rows.Count > 0)
            {
                DateTime minDate = startDate;
                DateTime maxDate = endDate;
                DateTime allocatedstdt;
                DateTime allocatedenddt;
                int totalNoOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(_context, minDate, maxDate, minOneDay);
                int noOfDays;
                double alloc = 0;
                double pctAlloc;
                int workingHrs = uHelper.GetWorkingHoursInADay(_context);
                // if (table.Columns.Contains(DatabaseObjects.Columns.AllocationStartDate) && table.Columns.Contains(DatabaseObjects.Columns.AllocationEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PctAllocation))
                if (table.Columns.Contains(DatabaseObjects.Columns.EstStartDate) && table.Columns.Contains(DatabaseObjects.Columns.EstEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PctEstimatedAllocation))
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        //DateTime allocatedstdt = Convert.ToDateTime(dr[DatabaseObjects.Columns.AllocationStartDate]),
                        //       allocatedenddt = Convert.ToDateTime(dr[DatabaseObjects.Columns.AllocationEndDate]);
                        //double pctAlloc = uHelper.StringToDouble(dr[DatabaseObjects.Columns.PctAllocation]);

                        allocatedstdt = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate]);
                        allocatedenddt = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate]);
                        pctAlloc = UGITUtility.StringToDouble(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);

                        if (allocatedstdt <= startDate && allocatedenddt <= endDate && allocatedenddt > startDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, startDate, allocatedenddt, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt >= startDate && allocatedenddt <= endDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, allocatedstdt, allocatedenddt, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt >= startDate && allocatedenddt >= endDate && allocatedenddt > startDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, allocatedstdt, endDate, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else if (allocatedstdt <= startDate && allocatedenddt >= endDate)
                        {
                            noOfDays = uHelper.GetTotalWorkingDaysBetween(_context, startDate, endDate, minOneDay);
                            alloc += ((noOfDays * pctAlloc) / 100);
                        }
                        else
                        {
                            alloc += 0;
                        }
                    }
                }
                if (totalNoOfWorkingDays > 0)
                    return Convert.ToInt16(Math.Ceiling((alloc * 100) / totalNoOfWorkingDays));
            }

            return 0;
        }

        public DataTable LoadRawTableAllResource(DateTime? sDate = null, DateTime? eDate = null)
        {
            List<RResourceAllocation> allocations = LoadByResource(null, sDate.Value, eDate.Value, ResourceAllocationType.Allocation);
            return UGITUtility.ToDataTable<RResourceAllocation>(allocations);
        }

        public List<RResourceAllocation> LoadByResource(string user, DateTime startDate, DateTime endDate, ResourceAllocationType allocationType)
        {
            List<RResourceAllocation> allocations = new List<RResourceAllocation>();
            Expression<Func<RResourceAllocation, bool>> exp = (t) => true;
            string userId = user;
            DateTime today = DateTime.UtcNow.Date;
            List<string> requiredQuery = new List<string>();
            exp = x => !x.Deleted;
            if (!string.IsNullOrWhiteSpace(user))
                exp = exp.And(x => x.Resource == user);


            List<string> qryOrDates = new List<string>();
            List<string> qryDates = new List<string>();
            if (allocationType == ResourceAllocationType.Planned)
            {
                exp = exp.And(x => x.PlannedStartDate.HasValue && x.PlannedEndDate.HasValue &&
                      (x.PlannedEndDate.Value.Date <= startDate.Date && x.PlannedStartDate.Value.Date >= endDate.Date) ||
                      (x.PlannedEndDate.Value.Date >= startDate.Date && x.PlannedStartDate.Value.Date <= endDate.Date));
            }
            else if (allocationType == ResourceAllocationType.Estimated)
            {
                exp = exp.And(x => x.EstStartDate.HasValue && x.EstEndDate.HasValue &&
                      (x.EstEndDate.Value.Date <= startDate.Date && x.EstStartDate.Value.Date >= endDate.Date) ||
                      (x.EstEndDate.Value.Date >= startDate.Date && x.EstStartDate.Value.Date <= endDate.Date));
            }
            else
            {
                Expression<Func<RResourceAllocation, bool>> exp1 = (t) => true;

                exp1 = exp1.And(x => x.AllocationStartDate.HasValue && x.AllocationEndDate.HasValue &&
                     (x.AllocationEndDate.Value.Date <= startDate.Date && x.AllocationStartDate.Value.Date >= endDate.Date) ||
                     (x.AllocationEndDate.Value.Date >= startDate.Date && x.AllocationStartDate.Value.Date <= endDate.Date));

                exp1 = exp1.Or(x => x.PlannedStartDate.HasValue && x.PlannedEndDate.HasValue &&
                     (x.PlannedEndDate.Value.Date <= startDate.Date && x.PlannedStartDate.Value.Date >= endDate.Date) ||
                     (x.PlannedEndDate.Value.Date >= startDate.Date && x.PlannedStartDate.Value.Date <= endDate.Date));
                exp = exp.And(exp1);
            }

            allocations = this.Load(exp);


            return allocations;
        }

        public DataTable LoadRawTableByProjectResource(string usersID, int tense, List<long> workItemId)
        {

            DataTable resourceAllocationDt = GetDataTable($"Resource='{usersID}' and ResourceWorkItemLookup IN ({string.Join(",", workItemId)})");
            return resourceAllocationDt;
        }

        public int CountAllocationPercentageWithProjectType(DataTable rawTable, string userID, int tense, DateTime startDate, DateTime endDate, bool minOneDay, List<string> type, DataTable projectDatatable, bool IsRunScenario, int viewId, bool IsAssignedAllocation = false, List<ResourceWorkItems> workItemList = null)
        {
            DataTable dttempnewProjectitems = new DataTable();
            dttempnewProjectitems.Columns.Add("TicketTitle", typeof(string));
            dttempnewProjectitems.Columns.Add("TicketId", typeof(string));
            UserProfileManager userManager = new UserProfileManager(_context);
            UserProfile user = userManager.LoadById(userID);
            DataTable table = null;
            if (rawTable != null && rawTable.Rows.Count > 0)
            {
                DataRow[] rows = rawTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, userID));
                if (rows != null && rows.Length > 0)
                    table = rows.CopyToDataTable();
            }

            if (table != null && table.Rows.Count > 0)
            {
                DateTime minDate = startDate;
                DateTime maxDate = endDate;
                int Count = 0;

                if (table.Columns.Contains(DatabaseObjects.Columns.EstStartDate) && table.Columns.Contains(DatabaseObjects.Columns.EstEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PlannedStartDate) && table.Columns.Contains(DatabaseObjects.Columns.PlannedEndDate) && table.Columns.Contains(DatabaseObjects.Columns.PctEstimatedAllocation) && table.Columns.Contains(DatabaseObjects.Columns.PctPlannedAllocation))
                {
                    long workitemId = 0;
                    string moduleName = string.Empty;
                    string viewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>",
                                                        DatabaseObjects.Columns.Id, DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem);
                    foreach (DataRow dr in table.Rows)
                    {
                        //block for filter by type.
                        #region filter by type
                        if (type != null && type.Count > 0 && !type.Contains("ALL"))
                        {
                            long workitemlookup = Convert.ToInt64(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]);


                            if (workitemId != workitemlookup)
                            {
                                workitemId = workitemlookup;
                                ResourceWorkItems workItem = null;
                                if (workItemList == null)
                                {
                                    //SPQuery query = new SPQuery();
                                    //query.Query = "<Where><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + workitemId.ToString() + "</Value></Eq></Where>";
                                    //query.ViewFields = viewFields;
                                    //query.ViewFieldsOnly = true;
                                    workItemList = workitemManager.Load().Where(x => x.WorkItem == Convert.ToString(workitemId)).ToList();   // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ResourceWorkItems, query);
                                }
                                var items = workItemList.Cast<ResourceWorkItems>().Where(x => x.ID == workitemId).ToList();
                                //SPListHelper.GetSPListItem(workItemList, workitemId, viewFields, true);
                                if (items == null || items.Count == 0)
                                    continue;
                                workItem = items[0];

                                moduleName = Convert.ToString(workItem.WorkItemType);
                            }

                            if (!type.Contains(moduleName))
                                continue;

                        }
                        #endregion

                        DateTime allocatedstdt;
                        DateTime allocatedenddt;

                        double pctAlloc = 0;
                        if (IsAssignedAllocation)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedEndDate])))
                                continue;

                            pctAlloc = UGITUtility.StringToDouble(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                            allocatedstdt = Convert.ToDateTime(dr[DatabaseObjects.Columns.PlannedStartDate]);
                            allocatedenddt = Convert.ToDateTime(dr[DatabaseObjects.Columns.PlannedEndDate]);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                continue;

                            pctAlloc = UGITUtility.StringToDouble(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                            allocatedstdt = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate]);
                            allocatedenddt = Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate]);
                        }

                        if (pctAlloc > 0)
                        {
                            if (allocatedstdt > endDate && allocatedenddt > endDate)
                            {
                            }
                            else if (startDate >= allocatedstdt && startDate <= allocatedenddt && endDate <= allocatedenddt)
                                Count++;
                            else if (startDate >= allocatedstdt && endDate >= allocatedenddt && startDate <= allocatedenddt)
                                Count++;
                            else if (startDate <= allocatedstdt && endDate >= allocatedenddt)
                                Count++;
                            else if (startDate <= allocatedstdt && endDate <= allocatedenddt && endDate <= allocatedenddt)
                                Count++;
                            else
                            {
                            }
                        }
                    }
                }

                return Count;
            }

            return 0;
        }

        public DataTable LoadDatatableByResource(string user, DateTime startDate, DateTime endDate, ResourceAllocationType allocationType, DataTable ticketData, bool IncludeClosed = false, string RoleId = "")
        {
            ModuleViewManager moduleManager = new ModuleViewManager(_context);
            TicketManager ticketManager = new TicketManager(_context);
            DataTable resultTable = CreateTable();
            string userId = user;

            //Loads allocations and workitem data for specified users
            List<RResourceAllocation> alloctions = LoadByResource(user, startDate, endDate, allocationType, IncludeClosed);
            DataTable workitems = workitemManager.LoadRawTableByResource(userId, 1);

            if (!string.IsNullOrEmpty(RoleId))
            {
                alloctions = alloctions.Where(x => x.RoleId.EqualsIgnoreCase(RoleId)).ToList();
            }

            //returns if there is not allocation and workitem
            if (alloctions.Count <= 0 || workitems.Rows.Count <= 0)
            {
                return resultTable;
            }

            DataRow workitemRow = null;
            GlobalRoleManager roleManager = new GlobalRoleManager(_context);
            //Iterates on each allocation
            foreach (RResourceAllocation allocationRow in alloctions)
            {
                //if (allocationType == ResourceAllocationType.Estimated && allocationRow.PctEstimatedAllocation == 0)
                //    continue;

                //if (allocationType == ResourceAllocationType.Planned && allocationRow.PctAllocation == 0)
                //    continue;
                DateTime allocationStartDate = UGITUtility.StringToDateTime(allocationRow.AllocationStartDate);
                DateTime allocationEndDate = UGITUtility.StringToDateTime(allocationRow.AllocationEndDate);
                DataRow rRow = resultTable.NewRow();
                rRow[DatabaseObjects.Columns.ResourceId] = allocationRow.Resource;
                rRow[DatabaseObjects.Columns.Resource] = _context.UserManager.GetDisplayNameFromUserId(allocationRow.Resource);
                resultTable.Rows.Add(rRow);

                //Gets Workitem row from workitems table
                DataRow[] workitemrowtemp = workitems.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, allocationRow.ResourceWorkItemLookup));
                if (workitemrowtemp != null && workitemrowtemp.Length > 0)
                    workitemRow = workitemrowtemp[0];

                if (workitemRow != null)
                {
                    rRow[DatabaseObjects.Columns.WorkItemType] = ResourceWorkItemsManager.FormatWorkItemType(Convert.ToString(workitemRow[DatabaseObjects.Columns.WorkItemType]));
                    rRow[DatabaseObjects.Columns.WorkItem] = workitemRow[DatabaseObjects.Columns.WorkItem];
                    rRow[DatabaseObjects.Columns.SubWorkItem] = workitemRow[DatabaseObjects.Columns.SubWorkItem];
                    rRow[DatabaseObjects.Columns.WorkItemLink] = workitemRow[DatabaseObjects.Columns.WorkItem];
                    rRow["WorkItemID"] = workitemRow[DatabaseObjects.Columns.Id];

                }

                rRow[DatabaseObjects.Columns.Id] = allocationRow.ID;
                rRow[DatabaseObjects.Columns.PctAllocation] = Math.Round(UGITUtility.StringToDouble(allocationRow.PctAllocation), 0);
                rRow[DatabaseObjects.Columns.AllocationStartDate] = allocationStartDate;
                rRow[DatabaseObjects.Columns.AllocationEndDate] = allocationEndDate;
                rRow[DatabaseObjects.Columns.ShowEditButton] = true;
                rRow[DatabaseObjects.Columns.ShowPartialEdit] = false;

                rRow[DatabaseObjects.Columns.PctPlannedAllocation] = Math.Round(UGITUtility.StringToDouble(allocationRow.PctPlannedAllocation), 2);
                rRow[DatabaseObjects.Columns.PlannedStartDate] = Convert.ToDateTime(allocationRow.PlannedStartDate);
                rRow[DatabaseObjects.Columns.PlannedEndDate] = Convert.ToDateTime(allocationRow.PlannedEndDate);

                rRow[DatabaseObjects.Columns.PctEstimatedAllocation] = Math.Round(UGITUtility.StringToDouble(allocationRow.PctEstimatedAllocation), 2);
                rRow[DatabaseObjects.Columns.EstStartDate] = Convert.ToDateTime(allocationRow.EstStartDate);
                rRow[DatabaseObjects.Columns.EstEndDate] = Convert.ToDateTime(allocationRow.EstEndDate);
                rRow[DatabaseObjects.Columns.SoftAllocation] = Convert.ToString(allocationRow.SoftAllocation);
                //if (Convert.ToString(allocationRow.SoftAllocation) == "True")
                //{
                //    rRow[DatabaseObjects.Columns.SoftAllocation] = "Soft";
                //}
                //else
                //{
                //    rRow[DatabaseObjects.Columns.SoftAllocation] = "Hard";
                //}

                var projectData = ticketData?.AsEnumerable()?.FirstOrDefault(x => x?.Field<string>(DatabaseObjects.Columns.TicketId) == allocationRow.TicketID) ?? null;

                if (projectData != null)
                {
                    DateTime preconStartDate = projectData[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value
                        ? projectData.Field<DateTime>(DatabaseObjects.Columns.PreconStartDate) : DateTime.MinValue;
                    DateTime preconEndDate = projectData[DatabaseObjects.Columns.PreconEndDate] != DBNull.Value
                        ? projectData.Field<DateTime>(DatabaseObjects.Columns.PreconEndDate) : DateTime.MinValue;
                    DateTime constStartDate = projectData[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value
                        ? projectData.Field<DateTime>(DatabaseObjects.Columns.EstimatedConstructionStart) : DateTime.MinValue;
                    DateTime constEndDate = projectData[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value
                        ? projectData.Field<DateTime>(DatabaseObjects.Columns.EstimatedConstructionEnd) : DateTime.MinValue;
                    DateTime closeoutStartDate = projectData[DatabaseObjects.Columns.CloseoutStartDate] != DBNull.Value
                        ? projectData.Field<DateTime>(DatabaseObjects.Columns.CloseoutStartDate) : DateTime.MinValue;
                    DateTime closeoutEndDate = projectData[DatabaseObjects.Columns.CloseoutDate] != DBNull.Value
                        ? projectData.Field<DateTime>(DatabaseObjects.Columns.CloseoutDate) : DateTime.MinValue;

                    rRow[DatabaseObjects.Columns.IsAllocInPrecon] = uHelper.IsDateRangeOverlapping(
                        allocationStartDate,
                        allocationEndDate,
                        preconStartDate, preconEndDate);

                    rRow[DatabaseObjects.Columns.IsAllocInConst] = uHelper.IsDateRangeOverlapping(
                        allocationStartDate,
                        allocationEndDate,
                        constStartDate, constEndDate);

                    rRow[DatabaseObjects.Columns.IsAllocInCloseOut] = uHelper.IsDateRangeOverlapping(
                        allocationStartDate,
                        allocationEndDate,
                        closeoutStartDate, closeoutEndDate);

                    rRow[DatabaseObjects.Columns.IsStartDateBeforePrecon] = allocationStartDate != DateTime.MinValue && preconStartDate != DateTime.MinValue
                        ? allocationStartDate < preconStartDate : false;

                    rRow[DatabaseObjects.Columns.IsStartDateBetweenPreconAndConst] = allocationStartDate != DateTime.MinValue && preconEndDate != DateTime.MinValue && constStartDate != DateTime.MinValue
                        ? allocationStartDate > preconEndDate && allocationStartDate < constStartDate : false;

                    rRow[DatabaseObjects.Columns.IsStartDateBetweenConstAndCloseOut] = allocationStartDate != DateTime.MinValue && constEndDate != DateTime.MinValue && closeoutStartDate != DateTime.MinValue
                        ? allocationStartDate > constEndDate && allocationStartDate < closeoutStartDate : false;
                }
                else
                {
                    rRow[DatabaseObjects.Columns.IsAllocInPrecon] = Convert.ToBoolean(allocationRow.IsAllocInPrecon);
                    rRow[DatabaseObjects.Columns.IsAllocInConst] = Convert.ToBoolean(allocationRow.IsAllocInConst);
                    rRow[DatabaseObjects.Columns.IsAllocInCloseOut] = Convert.ToBoolean(allocationRow.IsAllocInCloseOut);
                    rRow[DatabaseObjects.Columns.IsStartDateBeforePrecon] = Convert.ToBoolean(allocationRow.IsStartDateBeforePrecon);
                    rRow[DatabaseObjects.Columns.IsStartDateBetweenPreconAndConst] = Convert.ToBoolean(allocationRow.IsStartDateBetweenPreconAndConst);
                    rRow[DatabaseObjects.Columns.IsStartDateBetweenConstAndCloseOut] = Convert.ToBoolean(allocationRow.IsStartDateBetweenConstAndCloseOut);
                }
            }

            //returns if there is not allocation exist
            if (resultTable.Rows.Count < 0)
            {
                return resultTable;
            }


            List<string> queryExps = new List<string>();
            List<string> tempAllocations = new List<string>();
            UGITModule nprModuleRow = null;
            DataTable nprData = null;

            //if NPR work item is exist then loads NPR tickets and NPR module detail
            string rmmLevel1NPRProjects = ObjConfigurationVariableManager.GetValue("RMMTypeProjectsPendingApproval");
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType) == rmmLevel1NPRProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, tID));
                }
                nprModuleRow = moduleManager.LoadByName(ModuleNames.NPR);

                DataRow[] nprDataCollection = ticketManager.GetAllTickets(nprModuleRow).Select(string.Join("And", queryExps));
                nprData = nprDataCollection.CopyToDataTable();

                queryExps = new List<string>();
            }

            UGITModule tskModuleRow = null;
            DataTable tskData = null;
            //if TSK work item is exist then loads TSK tickets and TSK module detail
            string rmmLevel1TSKProjects = ObjConfigurationVariableManager.GetValue("RMMTypeTasklists");
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType) == rmmLevel1TSKProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                tskModuleRow = moduleManager.LoadByName(ModuleNames.TSK);

                DataTable nprTable = ticketManager.GetAllTickets(tskModuleRow);
                if (nprTable != null)
                {
                    DataRow[] nprDataCollection = nprTable.Select(string.Join(" Or ", queryExps));
                    if (nprDataCollection != null && nprDataCollection.Count() > 0)
                        tskData = nprDataCollection.CopyToDataTable();
                }
                queryExps = new List<string>();
            }

            UGITModule pmmModuleRow = null;
            DataTable pmmData = null;
            //if PMM work item is exist then loads PMM tickets and PMM module detail
            string rmmLevel1PMMProjects = ObjConfigurationVariableManager.GetValue("RMMTypeActiveProjects");
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItemType) == rmmLevel1PMMProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                pmmModuleRow = moduleManager.LoadByName(ModuleNames.PMM);

                DataTable pmmDataTable = ticketManager.GetAllTickets(pmmModuleRow);
                if (pmmDataTable != null)
                {
                    DataRow[] pmmDataCollection = pmmDataTable.Select(string.Join(" Or ", queryExps));
                    if (pmmDataCollection != null && pmmDataCollection.Count() > 0)
                        pmmData = pmmDataCollection.CopyToDataTable();
                }
                queryExps = null;
            }

            DataRow[] items = new DataRow[0];
            DataRow ticketrow = null;
            UGITModule module = null;
            foreach (DataRow itemRow in resultTable.Rows)
            {
                items = new DataRow[0];

                //Runs in case of NPR type of workitem
                if (Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == rmmLevel1NPRProjects && nprData != null && nprData.Rows.Count > 0)
                {
                    items = nprData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>",
                                                                        uHelper.GetHyperLinkControlForTicketID(nprModuleRow, Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl,
                                                                        UGITUtility.TruncateWithEllipsis(Convert.ToString(itemRow[DatabaseObjects.Columns.Title]), 40));

                        itemRow["SubWorkItem"] = Convert.ToString(itemRow["SubWorkItem"]);

                        //If project is closed then user cann't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow[DatabaseObjects.Columns.ShowPartialEdit] = true;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else if (Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == rmmLevel1TSKProjects && pmmData != null && pmmData.Rows.Count > 0)
                {
                    items = pmmData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>",
                                                    uHelper.GetHyperLinkControlForTicketID(pmmModuleRow, Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl,
                                                    UGITUtility.TruncateWithEllipsis(Convert.ToString(itemRow[DatabaseObjects.Columns.Title]), 40));

                        itemRow["SubWorkItem"] = Convert.ToString(itemRow["SubWorkItem"]);

                        //If project is closed then user cann't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow[DatabaseObjects.Columns.ShowPartialEdit] = true;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else if (Convert.ToString(itemRow[DatabaseObjects.Columns.WorkItemType]) == rmmLevel1PMMProjects && tskData != null && tskData.Rows.Count > 0)
                {
                    items = tskData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>",
                                                    uHelper.GetHyperLinkControlForTicketID(tskModuleRow, Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl,
                                                    UGITUtility.TruncateWithEllipsis(Convert.ToString(itemRow[DatabaseObjects.Columns.Title]), 40));

                        itemRow["SubWorkItem"] = Convert.ToString(itemRow["SubWorkItem"]);

                        //If project is closed then user cann't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow[DatabaseObjects.Columns.ShowPartialEdit] = true;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else
                {
                    string ticketid = UGITUtility.ObjectToString(itemRow[DatabaseObjects.Columns.WorkItem]);
                    string modulename = uHelper.getModuleNameByTicketId(ticketid);
                    if (!string.IsNullOrEmpty(modulename))
                    {
                        module = moduleManager.GetByName(modulename);
                        if (module != null)
                        {
                            ticketrow = ticketManager.GetByTicketID(module, ticketid);
                            if (ticketrow != null)
                            {
                                if (UGITUtility.StringToBoolean(ticketrow[DatabaseObjects.Columns.Closed]) == true)
                                {
                                    itemRow[DatabaseObjects.Columns.Title] = $"<b style='color:red;'>(Closed)</b> {Convert.ToString(ticketrow[DatabaseObjects.Columns.Title])}";
                                    itemRow[DatabaseObjects.Columns.Closed] = true;

                                    //itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                    //        Convert.ToString(ticketrow[DatabaseObjects.Columns.TicketId]), true, Convert.ToString(itemRow[DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);

                                    itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                        Convert.ToString(ticketrow[DatabaseObjects.Columns.TicketId]), true, Convert.ToString(itemRow[DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.Title]);

                                    if (UGITUtility.IfColumnExists(ticketrow, DatabaseObjects.Columns.ERPJobID) && ticketrow[DatabaseObjects.Columns.ERPJobID] != DBNull.Value)
                                    {
                                        //itemRow[DatabaseObjects.Columns.ERPJobID] = Convert.ToString(ticketrow[DatabaseObjects.Columns.ERPJobID]);

                                        itemRow[DatabaseObjects.Columns.ERPJobID] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                            Convert.ToString(ticketrow[DatabaseObjects.Columns.TicketId]), true, Convert.ToString(itemRow[DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.ERPJobID]);
                                    }
                                }
                                else
                                {
                                    itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(ticketrow[DatabaseObjects.Columns.Title]);
                                    if (UGITUtility.IfColumnExists(ticketrow, DatabaseObjects.Columns.ProjectID) && ticketrow[DatabaseObjects.Columns.ProjectID] != DBNull.Value)
                                    {
                                        //itemRow[DatabaseObjects.Columns.WorkItemLink] = $"{ticketrow[DatabaseObjects.Columns.ProjectID]} ({itemRow[DatabaseObjects.Columns.WorkItem]})";

                                        //itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                        //    Convert.ToString(ticketrow[DatabaseObjects.Columns.TicketId]), true, Convert.ToString(ticketrow[DatabaseObjects.Columns.Title])).NavigateUrl, $"{ticketrow[DatabaseObjects.Columns.ProjectID]} ({itemRow[DatabaseObjects.Columns.WorkItem]})");

                                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                            Convert.ToString(ticketrow[DatabaseObjects.Columns.TicketId]), true, Convert.ToString(ticketrow[DatabaseObjects.Columns.Title])).NavigateUrl, $"{ticketrow[DatabaseObjects.Columns.Title]}");
                                    }
                                    else
                                    {
                                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                            Convert.ToString(ticketrow[DatabaseObjects.Columns.TicketId]), true, Convert.ToString(ticketrow[DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);
                                    }

                                    if (UGITUtility.IfColumnExists(ticketrow, DatabaseObjects.Columns.ERPJobID) && ticketrow[DatabaseObjects.Columns.ERPJobID] != DBNull.Value)
                                    {
                                        //itemRow[DatabaseObjects.Columns.ERPJobID] = Convert.ToString(ticketrow[DatabaseObjects.Columns.ERPJobID]);

                                        itemRow[DatabaseObjects.Columns.ERPJobID] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                            Convert.ToString(ticketrow[DatabaseObjects.Columns.TicketId]), true, Convert.ToString(ticketrow[DatabaseObjects.Columns.Title])).NavigateUrl, $"{ticketrow[DatabaseObjects.Columns.ERPJobID]}");
                                    }
                                }
                            }
                        }
                    }
                }

            }

            return resultTable;
        }

        public void DeleteProjectEstimatedAllocation(AllocationDeleteModel model)
        {
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(_context);
            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(_context);
            List<ProjectEstimatedAllocation> allocations = CRMProjAllocManager.Load(x => x.TicketId == model.TicketID);
            ProjectEstimatedAllocation spListItem = allocations.FirstOrDefault(x => x.ID == model.ID);
            if (spListItem == null)
                return;
            long projEstmatedAllocId = spListItem.ID;
            List<string> historyDesc = new List<string>();
            string moduleName = uHelper.getModuleNameByTicketId(model.TicketID);
            List<RResourceAllocation> rmmAllocations = resourceAllocationManager.LoadByWorkItem(moduleName, model.TicketID, null, 4, false, true);

            string roleName = model.RoleName;
            string userName = model.UserName;
            historyDesc.Add(string.Format("Allocation removed for user: {0} - {1} {2}% {3}-{4}", userName, roleName, spListItem.PctAllocation, String.Format("{0:MM/dd/yyyy}", spListItem.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", spListItem.AllocationEndDate)));
            RResourceAllocation userRMMAllocation = rmmAllocations.FirstOrDefault(x => x.ProjectEstimatedAllocationId == UGITUtility.ObjectToString(projEstmatedAllocId));
            historyDesc.Add(string.Format("Allocation removed for user: {0} - {1} {2}% {3}-{4}", userName, roleName, spListItem.PctAllocation, String.Format("{0:MM/dd/yyyy}", spListItem.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", spListItem.AllocationEndDate)));
            List<ProjectEstimatedAllocation> userAllocations = allocations.Where(x => x.AssignedTo == model.UserID && x.ID != spListItem.ID).ToList();
            ProjectEstimatedAllocation ifObjPersists = CRMProjAllocManager.LoadByID(spListItem.ID);
            if (userAllocations.Count > 0)
            {
                if (userRMMAllocation != null)
                {
                    if (ifObjPersists != null)
                    {
                        resourceAllocationManager.Delete(userRMMAllocation);
                        resourceAllocationManager.UpdateIntoCache(userRMMAllocation, false);
                    }
                }

                if (ifObjPersists != null)
                    CRMProjAllocManager.Delete(spListItem);

                List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
                foreach (ProjectEstimatedAllocation alloc in userAllocations)
                {
                    lstUserWithPercetage.Add(new UserWithPercentage() { EndDate = alloc.AllocationEndDate ?? DateTime.MinValue, StartDate = alloc.AllocationStartDate ?? DateTime.MinValue, Percentage = alloc.PctAllocation, UserId = alloc.AssignedTo, RoleTitle = roleName, ProjectEstiAllocId = UGITUtility.ObjectToString(alloc.ID), RoleId = alloc.Type });
                }

                var taskManager = new UGITTaskManager(_context);
                List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(model.TicketID), model.TicketID);
                var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.ToLower().Contains(model.UserID.ToLower())).ToList();
                // Only create allocation enties if user is not in schedule
                if (res == null || res.Count == 0)
                {

                    ResourceAllocationManager.CPRResourceAllocation(_context, uHelper.getModuleNameByTicketId(model.TicketID), model.TicketID, lstUserWithPercetage);
                    ResourceAllocationManager.UpdateHistory(_context, historyDesc, model.TicketID);
                    historyDesc.ForEach(e => { ULog.WriteLog("PT >> " + _context.CurrentUser.Name + e); });
                    //Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                    //sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                    //sThreadStartMethodUpdateCPRProjectAllocation.Start();
                }
            }
            else
            {
                if (ifObjPersists != null)
                    CRMProjAllocManager.Delete(spListItem);
                if (userRMMAllocation != null)
                {
                    ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate ()
                    {
                        ProjectEstimatedAllocationManager CRMProjAllocMgr = new ProjectEstimatedAllocationManager(_context);
                        CRMProjAllocMgr.UpdateProjectGroups(moduleName, model.TicketID);
                        ResourceAllocationManager.UpdateHistory(_context, historyDesc, model.TicketID);
                        historyDesc.ForEach(e => { ULog.WriteLog("PT >> " + _context.CurrentUser.Name + e); });
                    };
                    Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                    sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                    sThreadStartMethodUpdateCPRProjectAllocation.Start();
                }
            }
            if (userRMMAllocation != null)
                RMMSummaryHelper.CleanAllocation(_context, userRMMAllocation?.ResourceWorkItems, true);
            InsertUserProjectExperience(model, spListItem);
        }

        private static void InsertUserProjectExperience(AllocationDeleteModel model, ProjectEstimatedAllocation spListItem)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", spListItem.TenantID);
            values.Add("@UserId", model.UserID);
            values.Add("@TicketId", model.TicketID);
            values.Add("@Tags", model.Tags);


            DataTable dt = uGITDAL.ExecuteDataSetWithParameters("usp_deleteUserProjectExperience", values);
        }

        public static void UpdateHistory(ApplicationContext context, List<string> historyDesc, string ProjectID, int? noOfAllocationChanges = null, bool? isUnfilledRoleCompleted = null)
        {
            if (historyDesc.Count > 0 || noOfAllocationChanges.HasValue || isUnfilledRoleCompleted.HasValue)
            {
                string moduleName = uHelper.getModuleNameByTicketId(ProjectID);

                Ticket TicketRequest = new Ticket(context, moduleName);
                DataRow dr = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(ProjectID), ProjectID);
                foreach (var item in historyDesc)
                {
                    uHelper.CreateHistory(context.CurrentUser, item, dr, context);
                }
                if ((noOfAllocationChanges.HasValue || isUnfilledRoleCompleted.HasValue) && moduleName == ModuleNames.CPR)
                {
                    UpdateAllocationStatistics(context, dr, noOfAllocationChanges, isUnfilledRoleCompleted);
                }
                TicketRequest.CommitChanges(dr);
            }
        }

        public static void UpdateAllocationStatistics(ApplicationContext context, DataRow dr, int? noOfAllocationChanges, bool? isUnfilledRoleCompleted)
        {
            // NumResourceAllocationChanges
            if (noOfAllocationChanges.HasValue)
            {
                var numResourceAllocationChanges = UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.NumResourceAllocationChanges]);
                if (!string.IsNullOrEmpty(numResourceAllocationChanges))
                {
                    var value = Convert.ToInt32(numResourceAllocationChanges);
                    dr[DatabaseObjects.Columns.NumResourceAllocationChanges] = value + noOfAllocationChanges.Value;
                }
                else
                {
                    dr[DatabaseObjects.Columns.NumResourceAllocationChanges] = noOfAllocationChanges.Value;
                }
            }

            // PerTimeTakenToFillUnfilledRoles
            if (isUnfilledRoleCompleted.HasValue && isUnfilledRoleCompleted.Value)
            {
                if (dr[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value && dr[DatabaseObjects.Columns.CloseoutDate] != DBNull.Value)
                {
                    int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, Convert.ToDateTime(dr[DatabaseObjects.Columns.PreconStartDate]), Convert.ToDateTime(dr[DatabaseObjects.Columns.CloseoutDate]));
                    int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                    int noOfWorkingDaysCurrent = uHelper.GetTotalWorkingDaysBetween(context, Convert.ToDateTime(dr[DatabaseObjects.Columns.Created]), DateTime.Now);
                    double projectDurationInWeeks = Convert.ToDouble(uHelper.GetWeeksFromDays(context, noOfWorkingDaysCurrent));

                    dr[DatabaseObjects.Columns.PerTimeTakenToFillUnfilledRoles] = $"{Math.Round((projectDurationInWeeks / noOfWeeks) * 100, MidpointRounding.AwayFromZero)}%";
                }
                else
                {
                    dr[DatabaseObjects.Columns.PerTimeTakenToFillUnfilledRoles] = "-";
                }
            }
            else
            {
                dr[DatabaseObjects.Columns.PerTimeTakenToFillUnfilledRoles] = "-";
            }
        }

        public static void UpdateHistory(ApplicationContext context, string historyDesc, string ProjectID)
        {
            string moduleName = uHelper.getModuleNameByTicketId(ProjectID);
            Ticket TicketRequest = new Ticket(context, moduleName);
            DataRow dr = Ticket.GetCurrentTicket(context, moduleName, ProjectID);
            if (dr != null)
            {
                uHelper.CreateHistory(context.CurrentUser, historyDesc, dr, context);
                TicketRequest.CommitChanges(dr);
            }
        }

        /// <summary>
        /// Returns Open Tickets/Items related resource allocations.
        /// </summary>   

        public List<RResourceAllocation> LoadOpenItems(DateTime StartDate, DateTime EndDate, string filerOnUser = "")
        {
            //List<string> LstOpenTicketIds = new List<string>();
            //DataTable dtOpenTickets = RMMSummaryHelper.GetOpenTickstIds(this.dbContext);
            //if (dtOpenTickets != null && dtOpenTickets.Rows.Count > 0)
            //{
            //    LstOpenTicketIds.AddRange(dtOpenTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
            //}

            List<RResourceAllocation> resourceAllocation = new List<RResourceAllocation>();
            DateTime sDate = StartDate;
            DataTable dtResourceWorkitems = RMMSummaryHelper.GetOpenworkitems(dbContext, false);
            //List<ResourceWorkItems> resourceWorkItems = resourceWorkItemsManager.Load(x => !x.Deleted && (LstOpenTicketIds.Any(y => x.WorkItem.EqualsIgnoreCase(y)) || !UGITUtility.IsValidTicketID(x.WorkItem)));
            List<long> filteredWorkItems = dtResourceWorkitems.AsEnumerable().Select(x => x.Field<long>(DatabaseObjects.Columns.ID)).ToList();
            if (!string.IsNullOrEmpty(filerOnUser))
            {
                resourceAllocation = this.Load(x => x.AllocationEndDate.Value >= sDate
                && (x.AllocationStartDate.Value <= EndDate)
                && filteredWorkItems.Contains(x.ResourceWorkItemLookup) && x.Resource.EqualsIgnoreCase(filerOnUser));
            }
            else
            {
                resourceAllocation = this.Load(x => x.AllocationEndDate.Value >= sDate
               && (x.AllocationStartDate.Value <= EndDate)
               && filteredWorkItems.Contains(x.ResourceWorkItemLookup));
            }

            return resourceAllocation;
        }

        public void UpdateProjectGroups(string moduleName, string ticketID)
        {
            ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(_context);
            List<ResourceWorkItems> resourceWorkItems = resourceWorkItemsManager.Load(x => x.WorkItem == ticketID);

            List<RResourceAllocation> allocations = this.Load(x => resourceWorkItems.Any(y => y.ID == x.ResourceWorkItemLookup));
            //Get distinct roles
            var alloc = (from availAlloc in allocations
                         join workItem in resourceWorkItems on availAlloc.ResourceWorkItemLookup equals workItem.ID
                         where workItem.WorkItem == ticketID
                         select availAlloc
                       );

            var userByGlobalRoles = alloc.ToLookup(x => x.RoleId);

            TicketManager ticketManager = new TicketManager(_context);
            DataRow row = Ticket.GetCurrentTicket(_context, moduleName, ticketID);
            if (row == null)
                return;

            Ticket ticket = new Ticket(_context, moduleName);
            GlobalRoleManager roleManager = new GlobalRoleManager(_context);
            GlobalRole uRole;
            foreach (var item in roleManager.Load(x => x.FieldName != null).Select(x => x.FieldName).ToList())
            {
                if (row.Table.Columns.Contains(item))
                {
                    row[item] = DBNull.Value;
                }
            }

            foreach (var g in userByGlobalRoles)
            {
                string roleFieldName = string.Empty;
                uRole = roleManager.LoadById(g.Key);
                if (uRole != null)
                    roleFieldName = uRole.FieldName;

                if (!string.IsNullOrWhiteSpace(roleFieldName) && row.Table.Columns.Contains(roleFieldName))
                {
                    row[roleFieldName] = string.Join(",", g.ToList().Select(x => x.Resource).Distinct());
                }
            }

            row[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(row[DatabaseObjects.Columns.TicketStageActionUserTypes]), row);
            ticket.CommitChanges(row);

        }

        public void MigrateResourceAllocation()
        {
            if (IsProcessActive)
                return;

            try
            {
                IsProcessActive = true;

                List<UserWithPercentage> lstUserWithPercetage = null;
                GlobalRoleManager roleManager = new GlobalRoleManager(_context);
                #region MyRegion
                List<ResourceWorkItems> resourceWorkItems = workitemManager.Load();
                List<RResourceAllocation> resourceAllocations = this.Load();
                //We assuming that all allocation with with valid ticket id is project allocation
                #endregion
                GlobalRole uRole = null;
                List<RResourceAllocation> rResourceAllocations = (from ra in resourceAllocations
                                                                  join rw in resourceWorkItems
                                                                  on ra.ResourceWorkItemLookup equals rw.ID
                                                                  where UGITUtility.IsValidTicketID(rw.WorkItem)
                                                                  select ra).ToList();

                // Delete project related allocation
                ULog.WriteLog("Delete project allocation started from resource allocation");
                this.Delete(rResourceAllocations);
                ULog.WriteLog("Delete project allocation completed from resource allocation");
                //Update resource allocation object after delteing 
                resourceAllocations = this.Load();

                ProjectEstimatedAllocationManager projectEstimatedAllocationManager = new ProjectEstimatedAllocationManager(_context);
                List<ProjectEstimatedAllocation> projEstAlloc = projectEstimatedAllocationManager.Load();
                int numLoadedEstAllocations = 0;
                int numSkippedEstAllocations = 0;
                int numNewWorkItems = 0;
                int numInsertedResAllocations = 0;
                int numErrors = 0;
                if (projEstAlloc != null)
                    numLoadedEstAllocations = projEstAlloc.Count;
                //Goroup by work item
                var proEstAlloc = projEstAlloc.GroupBy(x => x.TicketId).OrderBy(x => x.Key);
                List<ProjectEstimatedAllocation> projectEstimatedAllocations = null;
                int i = 0;
                foreach (var estAlloc in proEstAlloc)
                {
                    ULog.WriteLog("Process Started: " + estAlloc.Key);
                    projectEstimatedAllocations = estAlloc.OrderBy(x => x.ID).ToList();
                    List<ResourceWorkItems> workitems = null;
                    ResourceWorkItems workitem = null;
                    lstUserWithPercetage = new List<UserWithPercentage>();
                    foreach (ProjectEstimatedAllocation palloc in projectEstimatedAllocations)
                    {
                        string roleName = string.Empty;
                        uRole = roleManager.Get(x => x.Id == palloc.Type);
                        if (uRole != null)
                            roleName = uRole.Name;
                        else
                            roleName = string.Empty;


                        #region check work item is valid or not 
                        workitems = resourceWorkItems.Where(x => x.Resource.EqualsIgnoreCase(palloc.AssignedTo) && x.WorkItem == palloc.TicketId
                           && x.SubWorkItem.EqualsIgnoreCase(roleName)).ToList();
                        if (workitems == null || workitems.Count == 0)
                        {
                            workitem = new ResourceWorkItems();
                            workitem.Resource = palloc.AssignedTo;
                            workitem.SubWorkItem = roleName;
                            workitem.WorkItem = palloc.TicketId;
                            workitem.WorkItemType = uHelper.getModuleNameByTicketId(palloc.TicketId);
                            resourceWorkItems.Add(workitem);
                        }


                        #endregion

                        #region Skip allocation if one of start & end date is null
                        if (!palloc.AllocationStartDate.HasValue || !palloc.AllocationEndDate.HasValue)
                        {
                            numSkippedEstAllocations++;
                            continue;
                        }
                        #endregion

                        lstUserWithPercetage.Add(new UserWithPercentage()
                        {
                            EndDate = palloc.AllocationEndDate ?? DateTime.MinValue,
                            StartDate = palloc.AllocationStartDate ?? DateTime.MinValue,
                            Percentage = palloc.PctAllocation,
                            UserId = palloc.AssignedTo,
                            RoleTitle = roleName,
                            ProjectEstiAllocId = UGITUtility.ObjectToString(palloc.ID),
                            RoleId = palloc.Type,
                            SoftAllocation = palloc.SoftAllocation,
                            NonChargeable = palloc.NonChargeable
                        });

                    }

                    List<ResourceWorkItems> newResourceWorkItems = resourceWorkItems.Where(x => x.ID < 1).ToList();
                    if (newResourceWorkItems != null && newResourceWorkItems.Count > 0)
                    {
                        bool success = workitemManager.InsertItems(newResourceWorkItems);
                        if (!success)
                        {
                            numSkippedEstAllocations += lstUserWithPercetage.Count;
                            ULog.WriteLog("Issue during new work item creation and skip related allocation");
                            continue;
                        }

                        numNewWorkItems += newResourceWorkItems.Count;
                    }

                    //It will take care resource allocation 
                    ErrorCount errorCount = new ErrorCount();
                    ResourceAllocationManager.RefreshAllocation(_context, uHelper.getModuleNameByTicketId(estAlloc.Key), estAlloc.Key, lstUserWithPercetage, new List<string>(), ref errorCount, projectEstimatedAllocations: projectEstimatedAllocations, resourceWorkItems: resourceWorkItems);
                    i++;
                    if (errorCount.numError > 0)
                        numErrors += errorCount.numError;
                    if (errorCount.numInsertedResAllocations > 0)
                        numInsertedResAllocations += errorCount.numInsertedResAllocations;

                    ULog.WriteLog("Process Ended: " + estAlloc.Key);
                }

                //Below code used to show Statistic log
                string logMsg = string.Format("New Work Items: {0} \n Loaded Estimated Allocation: {1} \n Skipped Resource Allocation: {2} \n Insert Resource Allocation: {3} \n Resource Allocations Errors: {4}", numNewWorkItems, numLoadedEstAllocations, numSkippedEstAllocations, numInsertedResAllocations, numErrors);

                ULog.WriteLog(logMsg);
                IsProcessActive = false;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            finally
            {
                IsProcessActive = false;
            }
        }

        private long GetResourceWorkItemLookup(string assignedTo, string ticketId, string roleName)
        {
            List<ResourceWorkItems> workitems = workitemManager.Load(x => x.Resource.EqualsIgnoreCase(assignedTo) && x.WorkItem == ticketId).ToList();
            if (workitems != null && workitems.Count == 1)
                return workitems.FirstOrDefault().ID;
            else
            {
                workitems = workitemManager.Load(x => x.Resource.EqualsIgnoreCase(assignedTo) && x.WorkItem == ticketId && x.SubWorkItem.EqualsIgnoreCase(roleName)).ToList();
                if (workitems != null && workitems.Count > 1)
                {
                    return workitems.FirstOrDefault().ID;
                }
                else
                {
                    ResourceWorkItems workitem = new ResourceWorkItems();
                    workitem.Resource = assignedTo;
                    workitem.SubWorkItem = roleName;
                    workitem.WorkItem = ticketId;
                    workitem.WorkItemType = uHelper.getModuleNameByTicketId(ticketId);

                    workitemManager.Insert(workitem);
                    return workitem.ID;
                }
            }
        }
        public void UpdateResourceSummary()
        {
            try
            {
                if (IsSummaryComplete)
                    return;

                IsSummaryComplete = true;

                #region Block to update resource summary

                List<RResourceAllocation> rResourceAllocations = this.Load();
                ULog.WriteLog("List Of Resource Allocation Loaded:  " + rResourceAllocations.Count);
                #region Delete data from all summary table for current tenant for whom the update script is being executed
                ResourceAllocationMonthlyManager resourceAllocationMonthlyManager = new ResourceAllocationMonthlyManager(_context);
                ResourceUsageSummaryMonthWiseManager resourceUsageSummaryMonthWiseManager = new ResourceUsageSummaryMonthWiseManager(_context);
                ResourceUsageSummaryWeekWiseManager resourceUsageSummaryWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(_context);
                resourceAllocationMonthlyManager.Delete(resourceAllocationMonthlyManager.Load());
                resourceUsageSummaryMonthWiseManager.Delete(resourceUsageSummaryMonthWiseManager.Load());
                resourceUsageSummaryWeekWiseManager.Delete(resourceUsageSummaryWeekWiseManager.Load());
                #endregion
                RResourceAllocation rAlloc = null;

                for (int i = 0; i < rResourceAllocations.Count; i++)
                {
                    rAlloc = rResourceAllocations[i];
                    ULog.WriteLog("Current allocation Id: " + rAlloc.ID);
                    RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, rAlloc.ResourceWorkItemLookup, rResourceAlloc: rResourceAllocations);
                    ULog.WriteLog(string.Format("Process Work Item : {0}", Convert.ToString(rAlloc.ResourceWorkItemLookup)));
                }

                #endregion

                IsSummaryComplete = false;
            }
            catch (Exception ex)
            {
                ULog.WriteLog("Update Resource Summary Failed: " + ex.Message);
            }
            finally
            {
                IsSummaryComplete = false;
            }
        }
        public void UpdateIntoCache(RResourceAllocation allocation, bool flag)
        {
            List<RResourceAllocation> allocations = new List<RResourceAllocation>();
            string cacheName = DatabaseObjects.Tables.ResourceAllocation + "_" + _context.TenantID;
            allocations = (List<RResourceAllocation>)CacheHelper<object>.Get(cacheName, _context.TenantID);
            if (allocations == null)
            {
                ResourceAllocationManager objmgr = new ResourceAllocationManager(_context);
                allocations = objmgr.Load();
                flag = false;
            }
            if (flag)
            {
                allocations.Add(allocation);
                Dictionary<string, object> values = new Dictionary<string, object>();
                CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, allocations);
                DataTable dt = (DataTable)CacheHelper<object>.Get($"dt_{cacheName}", _context.TenantID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select($"Id = {allocation.ID}");
                    if (dr.Count() > 0)
                    {
                        // delete old record and add new.
                        DataView dv = new DataView(dt);
                        dv.RowFilter = string.Format("{0} <> {1}", DatabaseObjects.Columns.Id, allocation.ID); // query
                        dt = dv.ToTable();
                        values.Add("@TenantID", _context.TenantID);
                        values.Add("@Id", allocation.ID);
                        DataTable dtAllocations = GetTableDataManager.GetData("ResourceAllocation2", values);
                        dt.Merge(dtAllocations);
                        CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dt);
                    }
                    else
                    {
                        // Add new record to cache.
                        values.Add("@TenantID", _context.TenantID);
                        values.Add("@Id", allocation.ID);
                        DataTable dtAllocations = GetTableDataManager.GetData("ResourceAllocation2", values);
                        dt.Merge(dtAllocations);
                        CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dt);
                    }
                }
                else
                {
                    // Update cache with all records.
                    values.Add("@TenantID", _context.TenantID);
                    DataTable dtAllocations = GetTableDataManager.GetData("ResourceAllocation2", values);
                    CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dtAllocations);
                }
            }
            else
            {
                //allocations = base.Load();
                CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, allocations);
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", _context.TenantID);
                DataTable dtAllocations = GetTableDataManager.GetData("ResourceAllocation2", values);
                CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dtAllocations);
            }
        }
        public void UpdateDeletedAllocationCache(List<long> lstAllocationIds)
        {
            string cacheName = DatabaseObjects.Tables.ResourceAllocation + "_" + _context.TenantID;
            ResourceAllocationManager objmgr = new ResourceAllocationManager(_context);

            try
            {
                if (lstAllocationIds != null && lstAllocationIds.Count > 0)
                {
                    List<RResourceAllocation> lstAllocations = CacheHelper<object>.Get(cacheName, _context.TenantID) as List<RResourceAllocation>;
                    if (lstAllocations != null && lstAllocations.Count > 0)
                    {
                        lstAllocations = lstAllocations.Where(allocation => !lstAllocationIds.Contains(allocation.ID)).ToList();
                        CacheHelper<object>.AddOrUpdate(cacheName, lstAllocations);
                    }
                    else
                    {
                        lstAllocations = objmgr.Load();
                        CacheHelper<object>.AddOrUpdate(cacheName, lstAllocations);
                    }

                    DataTable dtAllocations = CacheHelper<object>.Get($"dt_{cacheName}", _context.TenantID) as DataTable;
                    if (dtAllocations != null && dtAllocations.Rows.Count > 0)
                    {
                        // Remove rows directly from DataTable using LINQ
                        dtAllocations.Rows.Cast<DataRow>()
                            .Where(row => lstAllocationIds.Contains(Convert.ToInt64(row["ID"])))
                            .ToList()
                            .ForEach(rowToDelete => dtAllocations.Rows.Remove(rowToDelete));
                        CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dtAllocations);
                    }
                    else
                    {
                        Dictionary<string, object> values = new Dictionary<string, object>();
                        values.Add("@TenantID", _context.TenantID);
                        DataTable dtNewAllocations = GetTableDataManager.GetData("ResourceAllocation2", values);
                        CacheHelper<object>.AddOrUpdate($"dt_{cacheName}", _context.TenantID, dtNewAllocations);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public DataTable FindPotentialAllocationsByRole(ApplicationContext context, PotentialAllocationsRequest requestModel)
        {
            DataSet dsAllocations = null;
            DataTable dtUnfilledAllocations = null;
            Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@StartDate", requestModel.StartDate},
                    { "@EndDate", requestModel.EndDate},
                    { "@UserRole", requestModel.RoleId },
                    { "@TenantID", context.TenantID },
                    { "@SoftAllocation", requestModel.IncludeSoftAllocation }
                };
            dsAllocations = GetTableDataManager.GetDataSet("UnfilledAllocationsList", values);
            if (dsAllocations?.Tables.Count > 0 && dsAllocations.Tables[0]?.Rows.Count > 0)
            {
                string prevTitle = "", prevCmic = "", prevType = "";
                
                dtUnfilledAllocations = dsAllocations.Tables[0];
                dtUnfilledAllocations.Columns.Add(new DataColumn("HasDuplicateRecord"));
                foreach (var item in dtUnfilledAllocations.AsEnumerable())
                {
                    string title = item.Field<string>(DatabaseObjects.Columns.Title);
                    string cmic = item.Field<string>(DatabaseObjects.Columns.ERPJobID);
                    string type = item.Field<string>("TypeName");
                    if (prevTitle == title && prevCmic == cmic && prevType == type)
                    {
                        item["HasDuplicateRecord"] = 1;
                    }
                    else
                    {
                        item["HasDuplicateRecord"] = 0;
                    }
                    prevTitle = title;
                    prevCmic = cmic;
                    prevType = type;
                }                //dtUnfilledAllocations.Columns.Add(new DataColumn("isSummary"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("UserAlloc"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("Fit"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("UserTags"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("ProjectTags"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("UserCertifications"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("ProjectCertifications"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("TicketURL"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("ProjectLink"));
                //dtUnfilledAllocations.Columns.Add(new DataColumn("Conflicts"));


                //dtUnfilledAllocations = GetUnfilledAllocationDetailsWithSummary(context, dtUnfilledAllocations);
            }
            
            return dtUnfilledAllocations;
        }
             
        public PotentialAllocationsResponse GetPotentialAllocationsList(ApplicationContext context, PotentialAllocationsRequest requestModel)
        {
            PotentialAllocationsResponse potentialAllocationsResponse = new PotentialAllocationsResponse();
            try
            {
                DataSet dsAllocations = null;
                DataTable dtUnfilledAllocations = null;
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "@StartDate", requestModel.StartDate},
                    { "@EndDate", requestModel.EndDate},
                    { "@UserRole", requestModel.RoleId },
                    { "@UserID", requestModel.UserId },
                    { "@TenantID", context.TenantID }
                };
                dsAllocations = GetTableDataManager.GetDataSet("PotentialAllocationsList", values);
                List<ResourceWeekWiseAvailabilityResponse> lstAvailabilityResponses = null;
                ResourceUsageSummaryWeekWiseManager resourceUsageSummaryWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
                double overLappingFactor = uHelper.GetAllocationOverbookingFactor(context);
                if (dsAllocations?.Tables.Count > 0 && dsAllocations.Tables[0]?.Rows.Count > 0 && dsAllocations.Tables[1] != null && dsAllocations.Tables[3] != null)
                {
                    dtUnfilledAllocations = dsAllocations.Tables[0];
                    //Fetch user's allocations
                    List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWise = new List<ResourceUsageSummaryWeekWise>();
                    resourceUsageSummaryWeekWise = UGITUtility.MapToList<ResourceUsageSummaryWeekWise>(dsAllocations.Tables[1]);

                    List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWiseNonSelected = new List<ResourceUsageSummaryWeekWise>();
                    resourceUsageSummaryWeekWiseNonSelected = UGITUtility.MapToList<ResourceUsageSummaryWeekWise>(dsAllocations.Tables[1]);

                    lstAvailabilityResponses = CalculateResourceAvailibility(requestModel.UserId, dtUnfilledAllocations, resourceUsageSummaryWeekWise, resourceUsageSummaryWeekWiseNonSelected, requestModel.IncludeSoftAllocation, false, overLappingFactor);
                    if (requestModel.Allocations != null && dsAllocations.Tables.Count > 1 && dsAllocations.Tables[2].Rows.Count > 0)
                    {
                        List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWiseUnfilled = null;
                        resourceUsageSummaryWeekWiseUnfilled = UGITUtility.MapToList<ResourceUsageSummaryWeekWise>(dsAllocations.Tables[2]);
                        //Add the selected allocations to user's allocation list.
                        foreach (ProjectAllocationModel pAlloc in requestModel.Allocations)
                        {
                            resourceUsageSummaryWeekWise.AddRange(resourceUsageSummaryWeekWiseUnfilled.Where(x => x.ActualEndDate.Value == pAlloc.AllocationEndDate
                            && x.ActualStartDate.Value == pAlloc.AllocationStartDate &&
                            x.ActualPctAllocation == pAlloc.PctAllocation && x.SubWorkItem == pAlloc.TypeName && x.WorkItem == pAlloc.ProjectID).GroupBy(x => x.WeekStartDate).Select(x => x.First(z => !resourceUsageSummaryWeekWise.Select(y => y.ID).Contains(z.ID))));
                            if (dtUnfilledAllocations.AsEnumerable().Any(x => x.Field<long>(DatabaseObjects.Columns.ID) == pAlloc.ID && x.Field<bool>("IsSelected") == false))
                            {
                                dtUnfilledAllocations.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ID) == pAlloc.ID && x.Field<bool>("IsSelected") == false)["IsSelected"] = true;
                            }
                        }


                        if (resourceUsageSummaryWeekWiseUnfilled != null)
                        {
                            //Set user's Id for Resource only to mimick that this unfilled allocation has been assgined to the user.
                            resourceUsageSummaryWeekWise.ForEach(alloc => alloc.Resource = requestModel.UserId);
                            //Send this new allocations list to calculates Post Alloc%.
                            lstAvailabilityResponses = CalculateResourceAvailibility(requestModel.UserId, dtUnfilledAllocations, resourceUsageSummaryWeekWise, resourceUsageSummaryWeekWiseNonSelected, requestModel.IncludeSoftAllocation, true, overLappingFactor);
                        }
                    }
                }
                DataTable dtUnfilledAllocationWithSummary = GetUnfilledAllocationDetailsWithSummary(context,dtUnfilledAllocations);
                potentialAllocationsResponse.dtUnfilledAllocations = dtUnfilledAllocationWithSummary;
                potentialAllocationsResponse.dtDefaultDivisionRole = dsAllocations.Tables[3];
                potentialAllocationsResponse.lstResourceWeekWiseAvailability = lstAvailabilityResponses;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return potentialAllocationsResponse;
        }

        public DataTable GetUnfilledAllocationDetailsWithSummary(ApplicationContext context,DataTable dtUnfilledAllocation)
        {
            DataTable dtWithSummary = null;
            if(dtUnfilledAllocation != null)
            {
                dtWithSummary = dtUnfilledAllocation.Clone();
                DataTable dtOnlySummary = dtUnfilledAllocation.Clone();
                foreach (DataRow row in dtUnfilledAllocation.Rows)
                {
                    DataTable dt = null;
                    bool isExistAlready = false;
                    try
                    {
                        if (row["ERPJobID"].ToString() != "")
                        {
                            dt = dtUnfilledAllocation.AsEnumerable().Where(x => x.Field<string>("Title") == row["Title"].ToString() && x.Field<string>("ProjectId") == row["ProjectId"].ToString() && x.Field<string>("ERPJobID") == row["ERPJobID"].ToString() && x.Field<string>("TypeName") == row["TypeName"].ToString()).CopyToDataTable();
                            if (dtWithSummary.Rows.Count > 0)
                            {
                                isExistAlready = dtWithSummary.AsEnumerable().Where(x => x.Field<string>("Title") == row["Title"].ToString() && x.Field<string>("ProjectId") == row["ProjectId"].ToString() && x.Field<string>("ERPJobID") == row["ERPJobID"].ToString() && x.Field<string>("TypeName") == row["TypeName"].ToString()).Any();
                            }
                        }
                        else{
                            dt = dtUnfilledAllocation.AsEnumerable().Where(x => x.Field<string>("Title") == row["Title"].ToString() && x.Field<string>("ProjectId") == row["ProjectId"].ToString() && x.Field<string>("TypeName") == row["TypeName"].ToString()).CopyToDataTable();
                            if (dtWithSummary.Rows.Count > 0)
                                isExistAlready = dtWithSummary.AsEnumerable().Where(x => x.Field<string>("Title") == row["Title"].ToString() && x.Field<string>("ProjectId") == row["ProjectId"].ToString() && x.Field<string>("TypeName") == row["TypeName"].ToString()).Any();
                        }
                        if (dt.Rows.Count > 1)
                        {
                            if (isExistAlready == false)
                            {
                                DataRow dr = dtOnlySummary.NewRow();
                                dr["Title"] = dt.Rows[0]["Title"].ToString();
                                dr["ERPJobID"] = dt.Rows[0]["ERPJobID"].ToString();
                                if (dt.Rows[0]["AllocationStartDate"] != null)
                                    dr["AllocationStartDate"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["AllocationStartDate"])).Min();
                                if (dt.Rows[0]["AllocationEndDate"] != null)
                                    dr["AllocationEndDate"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["AllocationEndDate"])).Max();
                                dr["isSummary"] = 1;
                                dr["PctAllocation"] = GetPctAllocation(dt);
                                dr["UserAlloc"] = dt.Rows[0]["UserAlloc"].ToString();
                                dr["ResourceWorkItemLookup"] = dt.Rows[0]["ResourceWorkItemLookup"].ToString();
                                dr["ProjectId"] = dt.Rows[0]["ProjectId"].ToString();
                                dr["DivisionId"] = dt.Rows[0]["DivisionId"].ToString();
                                dr["DepartmentId"] = dt.Rows[0]["DepartmentId"].ToString();
                                dr["Department"] = dt.Rows[0]["Department"].ToString();
                                dr["DepartmentDivision"] = dt.Rows[0]["DepartmentDivision"].ToString();

                                dr["ModuleName"] = dt.Rows[0]["ModuleName"].ToString();
                                if (dt.Rows[0]["PreconStartDate"].ToString() != "")
                                    dr["PreconStartDate"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["PreconStartDate"])).Min();
                                if (dt.Rows[0]["PreconEndDate"].ToString() != "")
                                    dr["PreconEndDate"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["PreconEndDate"])).Max();
                                if (dt.Rows[0]["EstimatedConstructionStart"].ToString() != "")
                                    dr["EstimatedConstructionStart"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["EstimatedConstructionStart"])).Min();
                                if (dt.Rows[0]["EstimatedConstructionEnd"].ToString() != "")
                                    dr["EstimatedConstructionEnd"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["EstimatedConstructionEnd"])).Max();
                                if (dt.Rows[0]["CloseoutStartDate"].ToString() != "")
                                    dr["CloseoutStartDate"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["CloseoutStartDate"])).Min();
                                if (dt.Rows[0]["CloseoutDate"].ToString() != "")
                                    dr["CloseoutDate"] = dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["CloseoutDate"])).Max(); 
                                dr["Division"] = dt.Rows[0]["Division"].ToString();
                                dr["TypeName"] = dt.Rows[0]["TypeName"].ToString();
                                dr["Length"] = dt.Compute("sum(Length)", string.Empty);//uHelper.GetWeeksFromDays(context, Convert.ToInt32((dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["AllocationEndDate"])).Max() - dt.Rows.OfType<DataRow>().Select(k => Convert.ToDateTime(k["AllocationStartDate"])).Min()).TotalDays));
                                dr["ProjectEstimatedAllocationId"] = dt.Rows[0]["ProjectEstimatedAllocationId"].ToString();
                                dr["SoftAllocation"] = dt.Rows[0]["SoftAllocation"].ToString();
                                dr["NonChargeable"] = dt.Rows[0]["NonChargeable"].ToString();
                                dr["Fit"] = dt.Rows[0]["Fit"].ToString();
                                dr["UserTags"] = dt.Rows[0]["UserTags"].ToString();
                                dr["Conflicts"] = dt.Rows[0]["Conflicts"].ToString();
                                dr["ProjectTags"] = dt.Rows[0]["ProjectTags"].ToString();
                                dr["FunctionalName"] = dt.Rows[0]["FunctionalName"].ToString();
                                dr["ProjectLink"] = dt.Rows[0]["ProjectLink"].ToString();
                                dr["TicketUrl"] = dt.Rows[0]["TicketUrl"].ToString();
                                dr["UserCertifications"] = dt.Rows[0]["UserCertifications"].ToString();
                                dr["ProjectCertifications"] = dt.Rows[0]["ProjectCertifications"].ToString();
                                dtOnlySummary.Rows.Add(dr);
                            }
                        }
                        dtWithSummary = dtOnlySummary;
                        if (isExistAlready == false)
                        {
                            foreach (DataRow dr1 in dt.Rows)
                            {
                                if (dt.Rows.Count == 1)
                                {
                                    dr1["IsSummary"] = 2;
                                }
                                else
                                {
                                    dr1["IsSummary"] = 0;
                                }
                                dtWithSummary.Rows.Add(dr1.ItemArray);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

            }
            return dtWithSummary;
        }

        public double GetPctAllocation(DataTable data)
        {
            double totalDays = 0;
            double totalPercentage = 0;
            foreach (DataRow dr in data.Rows)
            {
                DateTime allocStartDate = dr.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate);
                DateTime allocEndDate = dr.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate);
                double daysDiff = (allocEndDate - allocStartDate).TotalDays;
                double pctAlloc = dr.Field<double>(DatabaseObjects.Columns.PctAllocation);
                totalPercentage += pctAlloc * daysDiff;
            }
            totalDays = (data.AsEnumerable().Select(o => o.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate)).Max()
                - data.AsEnumerable().Select(o => o.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate)).Min()).TotalDays;
            return totalDays > 0 ? Math.Round(totalPercentage / totalDays, 1) : 0.0;
        }

        public List<ResourceWeekWiseAvailabilityResponse> CalculateResourceAvailibility(string userId, DataTable dtUnfilledAllocations,
            List<ResourceUsageSummaryWeekWise> lstUserAllocations, List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeekWiseNonSelected, bool includeSoftAllocations, bool calculatePostAllocPct, double overLappingFactor)
        {
            string colName;
            List<ProjectTag> matchingTags;
            List<ProjectTag> matchingCertifications;
            List<ResourceWeekWiseAvailabilityResponse> lstResourceWeekWiseAvailabilityResponse = new List<ResourceWeekWiseAvailabilityResponse>();
            DateTime allocStartdate;
            DateTime allocEnddate;
            try
            {
                ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(_context);
                ResourceUsageSummaryWeekWiseManager resourceUsageSummaryWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(_context);
                UserProjectExperienceManager objUserProjectExperienceManager = new UserProjectExperienceManager(_context);
                UserCertificateManager userCertificateManager = new UserCertificateManager(_context);

                List<UserProjectExperience> userProjectExperiences = new List<UserProjectExperience>();
                List<UserCertificates> userCertification = new List<UserCertificates>();
                List<ResourceWeekWiseAvailabilityResponse> lstRescWeekWiseAvailability = null;
                List<AllocationData> lstAllocationDates = new List<AllocationData>();
                List<string> lstUserId = new List<string>();
                List<Tuple<DateTime, DateTime>> tupleAllocationsDate = null;
                int fit;
                lstUserId.Add(userId);

                if (!includeSoftAllocations)
                    lstUserAllocations = lstUserAllocations.Where(x => !x.SoftAllocation).ToList();

                if (!includeSoftAllocations)
                    resourceUsageSummaryWeekWiseNonSelected = resourceUsageSummaryWeekWiseNonSelected.Where(x => !x.SoftAllocation).ToList();

                if (calculatePostAllocPct)
                    colName = "Availability";
                else
                {
                    colName = "UserAlloc";
                    dtUnfilledAllocations.Columns.Add(new DataColumn("Fit"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("UserTags"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("Conflicts"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("ProjectTags"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("ProjectLink"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("TicketURL"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("UserCertifications"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("ProjectCertifications"));
                    dtUnfilledAllocations.Columns.Add(new DataColumn("isSummary"));
                    userProjectExperiences = objUserProjectExperienceManager.Load(x => x.UserId == userId);
                    userCertification = userCertificateManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == _context.TenantID);
                }
                dtUnfilledAllocations.Columns.Add(new DataColumn(colName));

                foreach (DataRow drAlloc in dtUnfilledAllocations.Rows)
                {
                    tupleAllocationsDate = null;
                    lstAllocationDates.Clear();
                    bool IsSelectedRow = drAlloc.Field<bool>("IsSelected");
                    allocStartdate = UGITUtility.StringToDateTime(drAlloc[DatabaseObjects.Columns.AllocationStartDate]);
                    allocEnddate = UGITUtility.StringToDateTime(drAlloc[DatabaseObjects.Columns.AllocationEndDate]);
                    tupleAllocationsDate = CRMProjAllocManager.SplitAllocationDateAcrossPhases(UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID]),
                        allocStartdate, allocEnddate);
                    tupleAllocationsDate.ForEach(x =>
                    {
                        lstAllocationDates.Add(new AllocationData() { StartDate = x.Item1, EndDate = x.Item2, RequiredPctAllocation = UGITUtility.StringToDouble(drAlloc[DatabaseObjects.Columns.PctAllocation]) });
                    });
                    List<ResourceUsageSummaryWeekWise> resourceUsageSummaryWeeks = null;
                    if (IsSelectedRow)
                    {
                        List<long> tempDat = lstUserAllocations.Where(x => x.WorkItem == UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID])
                            && x.ActualStartDate.Value == allocStartdate
                            && x.ActualEndDate.Value == allocEnddate
                            && x.SubWorkItem == UGITUtility.ObjectToString(drAlloc["TypeName"])
                            && x.ActualPctAllocation == UGITUtility.StringToDouble(UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.PctAllocation])))?.GroupBy(x => x.WeekStartDate)?.Select(x => x.First().ID)?.ToList() ?? null;
                        resourceUsageSummaryWeeks = lstUserAllocations.Where(x => !tempDat?.Contains(x.ID) ?? true)?.ToList() ?? null;


                        //resourceUsageSummaryWeeks = lstUserAllocations.Where(x => !(x.WorkItem == UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID])
                        //    && x.ActualStartDate.Value == allocStartdate
                        //    && x.ActualEndDate.Value == allocEnddate
                        //    && x.SubWorkItem == UGITUtility.ObjectToString(drAlloc["TypeName"])
                        //    && x.ActualPctAllocation == UGITUtility.StringToDouble(UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.PctAllocation]))))?.ToList() ?? null;

                    }
                    else {
                        resourceUsageSummaryWeeks = resourceUsageSummaryWeekWiseNonSelected.Where(x => !(x.WorkItem == UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID])
                            && x.ActualStartDate.Value == allocStartdate
                            && x.ActualEndDate.Value == allocEnddate
                            && x.SubWorkItem == UGITUtility.ObjectToString(drAlloc["TypeName"])
                            && x.ActualPctAllocation == UGITUtility.StringToDouble(UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.PctAllocation]))))?.ToList() ?? null;
                    }
                    lstRescWeekWiseAvailability = uHelper.GetWeekWiseAveragePctAllocation(resourceUsageSummaryWeeks, lstAllocationDates, lstUserId);
                    if (lstRescWeekWiseAvailability == null || lstRescWeekWiseAvailability.Count == 0)
                    {
                        ResourceWeekWiseAvailabilityResponse resourceWeekWiseAvailability = new ResourceWeekWiseAvailabilityResponse();
                        resourceWeekWiseAvailability.UserId = userId;
                        resourceWeekWiseAvailability.AverageUtilization = 0;
                        resourceWeekWiseAvailability.AvailabilityType = Availability.FullyAvailable;
                        resourceWeekWiseAvailability.WeekWiseAllocations = null;
                        lstRescWeekWiseAvailability.Add(resourceWeekWiseAvailability);
                    }
                    lstRescWeekWiseAvailability[0].ID = UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ID]);
                    lstResourceWeekWiseAvailabilityResponse.Add(lstRescWeekWiseAvailability[0]);
                    double searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(_context, allocStartdate, UGITUtility.StringToDateTime(drAlloc[DatabaseObjects.Columns.AllocationEndDate]));
                    drAlloc[colName] = calculatePostAllocPct 
                        ? Math.Round(lstRescWeekWiseAvailability[0].PostAverageUtilization) <= 0 
                            ? UGITUtility.StringToDouble(UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.PctAllocation]))
                            :Math.Round(lstRescWeekWiseAvailability[0].PostAverageUtilization) + overLappingFactor
                        : Math.Round(lstRescWeekWiseAvailability[0].AverageUtilization);
                    drAlloc["Conflicts"] = lstRescWeekWiseAvailability[0].WeekWiseAllocations != null ? lstRescWeekWiseAvailability[0].WeekWiseAllocations.Where(x => !x.IsAvailable).Count() : 0;
                    if (!calculatePostAllocPct)
                    {
                        fit = 0;
                        matchingTags = null;
                        matchingCertifications = null;
                        List<ProjectTag> projectTags = objUserProjectExperienceManager.GetProjectExperienceTags(UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID]), true);
                        if (projectTags != null && projectTags.Count > 0)
                        {
                            matchingTags = projectTags.Where(x => x.Type == TagType.Experience && userProjectExperiences.Any(y => y.TagLookup == UGITUtility.StringToInt(x.TagId))).ToList();
                            matchingCertifications = projectTags.Where(x => x.Type == TagType.Certificate && userCertification.Any(y => y.ID == UGITUtility.StringToInt(x.TagId))).ToList();
                        }
                        drAlloc[DatabaseObjects.Columns.AllocationStartDate] = uHelper.GetDateStringInFormat(_context, allocStartdate, false);
                        drAlloc[DatabaseObjects.Columns.AllocationEndDate] = uHelper.GetDateStringInFormat(_context, allocEnddate, false);
                        drAlloc[DatabaseObjects.Columns.AllocationEndDate] = uHelper.GetDateStringInFormat(_context, allocEnddate, false);
                        fit = (matchingTags != null ? matchingTags.Count : 0) + (matchingCertifications != null ? matchingCertifications.Count : 0);
                        drAlloc["Fit"] = string.Format("{0}/{1}", fit, projectTags != null ? projectTags.Count : 0);
                        drAlloc["UserTags"] = string.Join(Constants.Separator6, userProjectExperiences.Select(x => x.TagLookup).ToArray());
                        drAlloc["ProjectTags"] = projectTags != null ? UGITUtility.ConvertListToString(projectTags.Where(x => x.Type == TagType.Experience).Select(x => x.TagId).ToList(), Constants.Separator6) : "";
                        drAlloc["UserCertifications"] = string.Join(Constants.Separator6, userCertification.Select(x => x.ID).ToArray());
                        drAlloc["ProjectCertifications"] = projectTags != null ? UGITUtility.ConvertListToString(projectTags.Where(x => x.Type == TagType.Certificate).Select(x => x.TagId).ToList(), Constants.Separator6) : "";

                        string ticketPath = $"event.stopPropagation();openTicketDialog('/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=ugitmodulewebpart&isreadonly=true&Module={UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ModuleName])}','TicketId={UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID])}','{UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID])}:{UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.Title])}','95','90', 0, ''); return false;";
                        drAlloc["TicketURL"] = ticketPath;
                        string path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" +
                            UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ProjectID]) + "&module=" + UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ModuleName]);
                        drAlloc["ProjectLink"] = string.Format("event.stopPropagation(); javascript:window.parent.UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path,
                            string.Format("moduleName={0}&ConfirmBeforeClose=true", UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.ModuleName])), UGITUtility.ObjectToString(drAlloc[DatabaseObjects.Columns.Title]), "");
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return lstResourceWeekWiseAvailabilityResponse;
        }

        protected List<ResourceUsageSummaryWeekWise> CreateWeekWiseAllcations(DateTime allocStartDate, DateTime allocEndDate, double pctAlloc, string userId)
        {
            DateTime tempStartDate = allocStartDate;
            //Create weeksummary 
            List<ResourceUsageSummaryWeekWise> lstWeekWiseAllocations = new List<ResourceUsageSummaryWeekWise>();
            while (tempStartDate < allocEndDate)
            {
                //Gets startdate of the week
                DateTime weekStartDate = tempStartDate.Date.AddDays(-(int)tempStartDate.DayOfWeek + 1);
                ResourceUsageSummaryWeekWise weekWiseSummaryRow = new ResourceUsageSummaryWeekWise();
                weekWiseSummaryRow.Resource = userId;
                weekWiseSummaryRow.PctAllocation = pctAlloc;
                weekWiseSummaryRow.WeekStartDate = weekStartDate;
                lstWeekWiseAllocations.Add(weekWiseSummaryRow);
            }
            return lstWeekWiseAllocations;
        }
        public class ErrorCount
        {
            public int numInsertedResAllocations { get; set; }
            public int numError { get; set; }
        }
    }
    public interface IResourceAllocationManager : IManagerBase<RResourceAllocation>
    {

    }
}
