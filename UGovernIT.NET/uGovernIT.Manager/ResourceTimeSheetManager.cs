using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Helper;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class ResourceTimeSheetManager : ManagerBase<ResourceTimeSheet>, IResourceTimeSheetManager
    {
        UserProfileManager ObjUserProfileManager = null;
        ResourceWorkItemsManager resourceWorkItemsManager = null;
        ResourceAllocationManager resourceAllocationManager = null;
        UGITTaskManager uGITTaskManager = null;
        ModuleViewManager ObjModuleViewManager =null;
        ConfigurationVariableManager configurationVariableMgr = null;
        ApplicationContext _context = null;

        List<string> previousSubWorkItem = new List<string>();

        public ResourceTimeSheetManager(ApplicationContext context) : base(context)
        {
            store = new ResourceTimeSheetStore(this.dbContext);
            ObjUserProfileManager = new UserProfileManager(this.dbContext);
            resourceWorkItemsManager = new ResourceWorkItemsManager(this.dbContext);
            resourceAllocationManager = new ResourceAllocationManager(this.dbContext);
            uGITTaskManager = new UGITTaskManager(this.dbContext);
            ObjModuleViewManager = new ModuleViewManager(this.dbContext);
            configurationVariableMgr = new ConfigurationVariableManager(this.dbContext);
            _context = context;
        }

        public DataTable Load(string user, DateTime startDate, DateTime endDate)
        {
            List<ResourceTimeSheet> list = new List<ResourceTimeSheet>();
            List<string> requiredQuery = new List<string>();
            if (!string.IsNullOrEmpty(user))
            {
                list = this.Load().Where(x => x.Resource == user && x.WorkDate >= startDate && x.WorkDate <= endDate).ToList();
            }
            return UGITUtility.ToDataTable(list);
        }
        public List<ResourceTimeSheet> LoadList(string user, DateTime startDate, DateTime endDate)
        {
            List<ResourceTimeSheet> list = new List<ResourceTimeSheet>();
            List<string> requiredQuery = new List<string>();
            if (!string.IsNullOrEmpty(user) && startDate != null && endDate != null)
            {
                list = this.Load().Where(x => x.Resource == user && Convert.ToDateTime(x.WorkDate).Date >= startDate.Date && Convert.ToDateTime(x.WorkDate) <= endDate.Date).ToList();
            }
            return list;
        }
        public ResourceTimeSheet LoadSheetItem(string user, DateTime startDate, long workItemId)
        {
            ResourceTimeSheet item = null;
            List<ResourceTimeSheet> list = new List<ResourceTimeSheet>();
            List<string> requiredQuery = new List<string>();
            if (!string.IsNullOrEmpty(user))
            {
                list = this.Load().Where(x => x.Resource == user && x.WorkDate == startDate && x.ResourceWorkItemLookup == workItemId).ToList();
                if (list.Count() > 0)
                    item = list[0];
            }
            return item;
        }

        public List<ResourceTimeSheet> LoadActualByWorkItem(long workItemID)
        {
            List<ResourceTimeSheet> timeSheet = this.Load().Where(x => x.ResourceWorkItemLookup == workItemID).ToList();
            return timeSheet;
        }


        private static ResourceTimeSheet LoadObject(DataRow item)
        {
            ResourceTimeSheet workItem = new ResourceTimeSheet(UGITUtility.SplitString(item[DatabaseObjects.Columns.Resource].ToString(), Constants.Separator, 0), (DateTime)item[DatabaseObjects.Columns.WorkDate]);
            workItem.HoursTaken = (int)item[DatabaseObjects.Columns.HoursTaken];
            workItem.ResourceWorkItemLookup = Convert.ToInt32(UGITUtility.SplitString(item[DatabaseObjects.Columns.ResourceWorkItemLookup].ToString(), Constants.Separator, 0));
            workItem.ID = (int)item[DatabaseObjects.Columns.Id];
            return workItem;
        }
        //public int Save()
        //{
        //    return Save(SPContext.Current.Web);
        //}
        //public int Save(SPWeb spWeb)
        //{
        //    int message = 0;
        //    SPList workSheetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ResourceTimeSheet, spWeb);
        //    SPListItem item = null;
        //    ResourceTimeSheet work = ResourceTimeSheet.LoadSheetItem(spWeb, this.ResourceId.ToString(), this.workDate, this.WorkItem.WorkItemId);

        //    if (work != null)
        //    {
        //        item = SPListHelper.GetSPListItem(workSheetList, work.WorkId);
        //        item[DatabaseObjects.Columns.HoursTaken] = this.Hours;
        //        item.Update();
        //    }
        //    else if (work == null && this.Hours > 0)
        //    {
        //        item = workSheetList.Items.Add();
        //        item[DatabaseObjects.Columns.Title] = string.Format("{0};#{1};#{2}", this.WorkItem.Level1, this.WorkItem.Level2, this.WorkItem.Level3);
        //        item[DatabaseObjects.Columns.WorkDate] = this.workDate.ToString("yyyy-MM-dd");
        //        item[DatabaseObjects.Columns.Resource] = this.ResourceId;
        //        item[DatabaseObjects.Columns.ResourceWorkItemLookup] = this.WorkItem.WorkItemId;
        //        item[DatabaseObjects.Columns.HoursTaken] = this.Hours;
        //        item.Update();


        //    }

        //    return message;
        //}

        /// <summary>
        /// Load WorkSheet of specified user. It loads worksheet of week basis. week start from specified startdate
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public DataTable LoadWorkSheetByDate(string userID, DateTime startDate)
        {
            string RMMLevel1PMMProjects = uHelper.GetModuleTitle("PMM");
            string RMMLevel1NPRProjects = uHelper.GetModuleTitle("NPR");
            string RMMLevel1TSKProjects = uHelper.GetModuleTitle("TSK");

            TicketHoursManager ticketHourHelper =new TicketHoursManager(_context);
            //Creates timesheet table
            DataTable resultTable = CreateTableSchema();
            //Loads workitems and timesheet and allocation data in datatable
            DataTable workItems = resourceWorkItemsManager.LoadRawTableByResource(userID, 1);   // UGITUtility.ToDataTable(resourceWorkItemsManager.Load().Where(x=>x.Resource==userID && !Convert.ToBoolean(x.Deleted)).ToList());
            DataTable timeSheetTable = UGITUtility.ToDataTable(this.LoadList(userID.ToString(), startDate, startDate.AddDays(6)));
            DataTable allocations = resourceAllocationManager.LoadRawTableByResource(userID, 4);

            //Create WorkDate1 Column which store workdate value as datetime
            //Because workdate come in string format which is useless to query on date
            if (timeSheetTable != null && timeSheetTable.Columns.Contains(DatabaseObjects.Columns.WorkDate))
            {
                timeSheetTable.Columns.Add("WorkDate1", typeof(DateTime));
                timeSheetTable.Columns["WorkDate1"].Expression = DatabaseObjects.Columns.WorkDate;
            }

            if (allocations != null && allocations.Rows.Count > 0)
            {
                DataRow[] rows = allocations.AsEnumerable().Where(x => !string.IsNullOrEmpty(Convert.ToString(x.Field<DateTime?>(DatabaseObjects.Columns.AllocationStartDate))) && !string.IsNullOrEmpty(Convert.ToString(x.Field<DateTime?>(DatabaseObjects.Columns.AllocationEndDate)))).ToArray();
                if (rows.Length > 0)
                    allocations = rows.CopyToDataTable();
                else
                    allocations = allocations.Clone();
            }

            //Create StartDate Column which store AllocationStartDate value as datetime
            //Because AllocationStartDate come in string format which is useless to query on date
            if (allocations != null && allocations.Columns.Contains(DatabaseObjects.Columns.AllocationStartDate))
            {
                allocations.Columns.Add("StartDate", typeof(DateTime));
                allocations.Columns["StartDate"].Expression = DatabaseObjects.Columns.AllocationStartDate;
            }

            //Create EndDate Column which store AllocationEndDate value as datetime
            //Because AllocationEndDate come in string format which is useless to query on date
            if (allocations != null && allocations.Columns.Contains(DatabaseObjects.Columns.AllocationEndDate))
            {
                allocations.Columns.Add("EndDate", typeof(DateTime));
                allocations.Columns["EndDate"].Expression = DatabaseObjects.Columns.AllocationEndDate;
            }

            //if there is not work item then return right away. workitem must be exist if there is any allocation
            if (workItems == null || workItems.Rows.Count <= 0)
            {
                return resultTable;
            }

            //Overlaps between AllocationRange and CurrentWeek
            //(StartA <= EndB) And (EndA >= StartB)
            string query = string.Format("(#{0}#<= {3} AND #{1}# >= {2})",
                startDate.ToString("MM/dd/yyyy"), startDate.AddDays(6).ToString("MM/dd/yyyy"), "StartDate", "EndDate");

            //Loads current week existing allocation of user
            DataRow[] allocationsForCurrentWeek = new DataRow[0];
            if (allocations != null && allocations.Rows.Count > 0)
            {
                allocationsForCurrentWeek = allocations.Select(query);
            }

            DataRow allocationRow = null;
            DataRow[] timesheetRows = new DataRow[0];
            DataRow[] timesheetAllRows = (timeSheetTable != null ? timeSheetTable.Select() : null);

            DataRow sheetRow = null;
            DateTime tempDate = startDate;
            string moduleQuery = string.Format("'{0}'", DatabaseObjects.Columns.ModuleName);
           var moduleList = ObjModuleViewManager.LoadAllModules().AsEnumerable().Select(x => Convert.ToString(x.Field<string>(DatabaseObjects.Columns.ModuleName))).ToList();
            // List<string> moduleList = uGITCache.GetModuleList(ModuleType.All, spWeb).AsEnumerable().Select(x => Convert.ToString(x.Field<string>(DatabaseObjects.Columns.ModuleName))).ToList();

            List<string> previousWorkItem = new List<string>();

            #region Iterates on workitems   
            foreach (DataRow workItem in workItems.Rows)
            {
                //get task details by Id....
                string[] arr = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                DataRow taskItem = null;
                string workItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                string subWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);
                string subSubWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubSubWorkItem]);
                string uniqueWorkItem = $"{workItem[DatabaseObjects.Columns.WorkItem]};#{workItem[DatabaseObjects.Columns.Resource]};#{workItem[DatabaseObjects.Columns.StartDate]}";

                if (workItemType == "Current Projects (PMM)" || workItemType == "Project Management Module (PMM)" || workItemType.Contains("(PMM)"))
                    workItemType = "PMM";

                if (workItemType == "Opportunity Management (OPM)" || workItemType.Contains("(OPM)"))
                    workItemType = ModuleNames.OPM;

                if (workItemType == "Project Management (CPR)" || workItemType.Contains("(CPR)"))
                    workItemType = ModuleNames.CPR;

                if (workItemType == "Service Projects (CNS)" || workItemType.Contains("(CNS)"))
                    workItemType = ModuleNames.CNS;

                if (arr.Length == 2)
                {
                    if (UGITUtility.IsNumber(arr[0], out long result))
                    {
                        if (workItemType == "TSK")
                            taskItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKTasks, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(arr[0])))[0];
                        else if (workItemType == "PMM")
                            taskItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMTasks, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select((string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(arr[0]))))[0];
                        else if (uGITTaskManager.IsModuleTasks(workItemType))
                            taskItem = uGITTaskManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, Convert.ToInt32(arr[0])))[0]; 
                    }
                }

                //Show total hours time sheet entries in case of standard workitem or actual hours by user is enable
                bool showTotalHoursTimeSheetRow = workItemType != "SVC" && (ticketHourHelper.IsStdWorkItemEnable(_context, workItemType, startDate) || ticketHourHelper.IsActualHoursByUserEnable(_context, workItemType));

                //loads if allocation is exist for current workitem
                //allocationRow = allocationsForCurrentWeek.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == string.Format("{0};#{0}", workItem[DatabaseObjects.Columns.Id]));
                long workItemID = UGITUtility.StringToLong(workItem[DatabaseObjects.Columns.Id]);

                allocationRow = allocationsForCurrentWeek.FirstOrDefault(x => x.Field<long?>(DatabaseObjects.Columns.ResourceWorkItemLookup) == workItemID);

                if (allocationRow == null)
                {
                    //Loads if any timesheet entry is exist in timesheet list against current workitem
                    if (timesheetAllRows != null && timesheetAllRows.Length > 0)
                    {
                        timesheetRows = timesheetAllRows.Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == Convert.ToString(workItemID)).ToArray();
                    }
                    DataRow tRow = null;

                    DataTable subTasks = resultTable.Clone();
                    if (timesheetRows.Count() > 0 && moduleList.Exists(x => (workItemType).ToLower() == x.ToLower()))
                        CreateTaskLevelTimeSheet(_context, userID, startDate, workItem, subTasks);

                    //If timesheet entries is exist then loads current workitem in weektimesheet datatable
                    if ((timesheetRows.Count() > 0 || showTotalHoursTimeSheetRow) && !previousWorkItem.Contains(uniqueWorkItem))
                    {
                        tRow = resultTable.NewRow();
                        tRow["WorkItemID"] = workItem[DatabaseObjects.Columns.Id];
                        tRow["Type"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                        tRow["WorkItem"] = workItem[DatabaseObjects.Columns.WorkItem];
                        tRow[DatabaseObjects.Columns.WorkItemLink] = workItem[DatabaseObjects.Columns.WorkItem];
                        tRow["SubWorkItem"] = workItem[DatabaseObjects.Columns.SubWorkItem];
                //        tRow["SubSubWorkItem"] = workItem[DatabaseObjects.Columns.SubSubWorkItem];
                        tRow["SubWorkItemLink"] = workItem[DatabaseObjects.Columns.SubWorkItem];
                        tRow["ShowEditButtons"] = true;
                        tRow["ShowDeleteButton"] = true;

                        if (Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]) == "SVC")
                        {
                            tRow["ShowEditButtons"] = false;
                        }
                        //if any of the entry for workitem inside request type have module for which actualHorus by user enable then disable editing for that workitem
                        //else if (IsActualHoursPresent_NonModuleWorkItems(spWeb, requestTypeData, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]), Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]), Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem])))
                        //{
                        //    tRow["ShowEditButtons"] = false;
                        //    tRow["ShowDeleteButton"] = false;
                        //}
                        else if (showTotalHoursTimeSheetRow)
                        {
                            tRow["ShowEditButtons"] = false;
                            tRow["ShowDeleteButton"] = false;
                            tRow["SubWorkItem"] = "Total";
                            tRow["SubWorkItemLink"] = "Total";
                        }

                        if (taskItem == null)
                            tRow["EstimatedRemainingHours"] = 0;
                        else
                            tRow["EstimatedRemainingHours"] = taskItem[DatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : taskItem[DatabaseObjects.Columns.EstimatedRemainingHours];

                        //resultTable.Rows.Add(tRow);
                        tempDate = startDate;
                        for (int i = 1; i <= 7; i++)
                        {
                            tRow["WeekDay" + i] = 0;
                            sheetRow = timesheetRows.FirstOrDefault(x => x.Field<DateTime>("WorkDate1").Date == tempDate.Date);
                            if (subTasks.Rows.Count > 0)
                            {
                                tRow["WeekDay" + i] = subTasks.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItem) == Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem])).Sum(x => x.Field<double>("WeekDay" + i));
                            }
                            else
                            {
                                if (sheetRow != null)
                                    tRow["WeekDay" + i] = Convert.ToDouble(sheetRow[DatabaseObjects.Columns.HoursTaken]);
                            }

                            if (sheetRow != null)
                            {                                
                                tRow["Comment" + i] = Convert.ToString(sheetRow[DatabaseObjects.Columns.WorkDescription]);
                                tRow["ID" + i] = Convert.ToInt32(sheetRow[DatabaseObjects.Columns.Id]);
                            }

                            tempDate = tempDate.AddDays(1);
                        }
                        tRow["ListName"] = DatabaseObjects.Tables.ResourceTimeSheet;
                    }
                    else
                    {
                        DataRow dr = resultTable.Select($"{DatabaseObjects.Columns.WorkItem}='{workItem[DatabaseObjects.Columns.WorkItem]}' and {DatabaseObjects.Columns.SubWorkItem}='Total'").FirstOrDefault();
                        if (dr != null)
                        {
                            tempDate = startDate;
                            double hours = 0;
                            for (int i = 1; i <= 7; i++)
                            {
                                if (subTasks.Rows.Count > 0)
                                {
                                    hours = Convert.ToDouble(dr["WeekDay" + i]);
                                    dr["WeekDay" + i] = hours + subTasks.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.WorkItem) == Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem])).Sum(x => x.Field<double>("WeekDay" + i));
                                    
                                }
                                hours = 0;
                                tempDate = tempDate.AddDays(1);
                            }
                        }
                    }

                    if (!previousWorkItem.Contains(uniqueWorkItem) && showTotalHoursTimeSheetRow)
                        previousWorkItem.Add(uniqueWorkItem);

                    // Moved below code up, to fix  (BTS-21-000564: Cant add PMM work item to time sheet); Sum of Hours of Days of SubWorkItem is not adding to Total (in Sub header)
                    /*
                    DataTable subTasks = resultTable.Clone();
                    if (timesheetRows.Count() > 0 && moduleList.Exists(x => (workItemType).ToLower() == x.ToLower()))
                        CreateTaskLevelTimeSheet(_context, userID, startDate, workItem, subTasks);
                    */

                    //add main timesheet entries if its normal case otherwise subtasks timesheet must be present.
                    if (tRow != null && (!showTotalHoursTimeSheetRow || subTasks.Rows.Count > 0))
                        resultTable.Rows.Add(tRow);

                    if (subTasks.Rows.Count > 0)
                    {
                        if (tRow != null)
                        {
                            tRow[DatabaseObjects.Columns.EstimatedRemainingHours] = (double)subTasks.Compute(string.Format("sum({0})", DatabaseObjects.Columns.EstimatedRemainingHours), "");
                        }
                        resultTable.Merge(subTasks);
                    }

                }
                else
                {
                    //If allocation is exist then loads current workitem in weektimesheet datatable
                    DataRow tRow = resultTable.NewRow();
                    tRow["WorkItemID"] = workItem[DatabaseObjects.Columns.Id];
                    tRow["Type"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                    tRow["WorkItem"] = workItem[DatabaseObjects.Columns.WorkItem];
                    tRow[DatabaseObjects.Columns.WorkItemLink] = workItem[DatabaseObjects.Columns.WorkItem];
                    tRow["SubWorkItem"] = workItem[DatabaseObjects.Columns.SubWorkItem];
                    tRow["SubWorkItemLink"] = workItem[DatabaseObjects.Columns.SubWorkItem];
                    tRow["ShowEditButtons"] = true;
                    tRow["ShowDeleteButton"] = true;

                    bool isActualHourByUser = ticketHourHelper.IsActualHoursByUserEnable(_context, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]));

                    if (Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]) == "SVC")
                    {
                        tRow["ShowEditButtons"] = false;
                        tRow["ShowDeleteButton"] = false;
                    }
                    //if any of the entry for workitem inside request type have module for which actualHorus by user enable then disable editing for that workitem
                    //else if (IsActualHoursPresent_NonModuleWorkItems(spWeb, requestTypeData, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]), Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]), Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem])))
                    //{
                    //    tRow["ShowEditButtons"] = false;
                    //    tRow["ShowDeleteButton"] = false;
                    //}
                    else if (ticketHourHelper.IsStdWorkItemEnable(_context, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]), startDate) || isActualHourByUser)
                    {
                        tRow["ShowEditButtons"] = false;

                        if (string.IsNullOrEmpty(subWorkItem))
                        {
                            tRow["SubWorkItem"] = "Total";
                            tRow["SubWorkItemLink"] = "Total";
                        }
                    }

                    // Non project allocation to make hyperlink
                    if (!UGITUtility.StringToBoolean(tRow["ShowEditButtons"]))
                    {
                        string url = string.Format("{0}?control=timesheetentrydetail&workitemtype={1}&workitem={2}&subworkitem={3}&StartDate={4}&UserId={5}", UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx"), tRow["TypeName"], tRow["WorkItem"], tRow["OriginalSubWorkItem"], startDate, userID);
                        string title = string.Format("Tickets for Work Item: {0}", uHelper.ReplaceInvalidCharsInURL(Convert.ToString(tRow["WorkItem"])));
                        string prefix = "javascript:window.parent.parent.UgitOpenPopupDialog(";
                        string navigationUrl = string.Format(prefix + "\"{0}\",\"\",\"{1}\",\"80\",\"90\")", url, title);

                        tRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", navigationUrl, tRow["WorkItem"]);

                        if (!string.IsNullOrEmpty(subWorkItem))
                            tRow["SubWorkItemLink"] = string.Format("<a href='{0}'>{1}</a>", navigationUrl, tRow["SubWorkItem"]);
                    }

                    if (taskItem == null)
                        tRow["EstimatedRemainingHours"] = 0;
                    else
                        tRow["EstimatedRemainingHours"] = taskItem[DatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : taskItem[DatabaseObjects.Columns.EstimatedRemainingHours];

                   // resultTable.Rows.Add(tRow);
                    if (timesheetAllRows != null && timesheetAllRows.Length > 0)
                    {
                        timesheetRows = timesheetAllRows.Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == Convert.ToString(workItemID)).ToArray();
                    }
                    tempDate = startDate;
                    for (int i = 1; i <= 7; i++)
                    {
                        tRow["WeekDay" + i] = 0;
                        sheetRow = timesheetRows.FirstOrDefault(x => x.Field<DateTime>("WorkDate1").Date == tempDate.Date);
                        if (sheetRow != null)
                        {
                            tRow["WeekDay" + i] = Convert.ToDouble(sheetRow[DatabaseObjects.Columns.HoursTaken]);
                            tRow["Comment" + i] = Convert.ToString(sheetRow[DatabaseObjects.Columns.WorkDescription]);
                            tRow["ID" + i] = Convert.ToInt32(sheetRow[DatabaseObjects.Columns.Id]);
                        }

                        tempDate = tempDate.AddDays(1);
                    }
                    tRow["ListName"] = DatabaseObjects.Tables.ResourceTimeSheet;

                    DataTable subTasks = resultTable.Clone();
                    if (moduleList.Exists(x => Convert.ToString(workItemType).ToLower() == x.ToLower()))
                        CreateTaskLevelTimeSheet(_context, userID, startDate, workItem, subTasks);

                    //add main timesheet entries if its normal case otherwise subtasks timesheet must be present.
                    if (tRow != null && (!showTotalHoursTimeSheetRow || subTasks.Rows.Count > 0))
                        resultTable.Rows.Add(tRow);
                    if (subTasks.Rows.Count > 0)
                    {
                        if (tRow != null)
                        {
                            tRow[DatabaseObjects.Columns.EstimatedRemainingHours] = (double)subTasks.Compute(string.Format("sum({0})", DatabaseObjects.Columns.EstimatedRemainingHours), "");

                            // fill record for OriginalSubWorkItem from it's subtasks
                            tRow["OriginalSubWorkItem"] = string.Join(Constants.Separator, subTasks.AsEnumerable().Select(x => x.Field<string>("OriginalSubWorkItem")).ToList());
                        }
                        resultTable.Merge(subTasks);
                    }


                }
            }
            #endregion

            //Returns when not entry in resulttable
            if (resultTable == null || resultTable.Rows.Count <= 0)
            {
                return resultTable;
            }

            List<string> queryExps = new List<string>();
            List<string> tempAllocations = new List<string>();
            UGITModule nprModuleRow = null;
            DataTable nprData = null;
            ModuleViewManager moduleViewManager = new ModuleViewManager(this.dbContext);

            //if NPR work item is exist then loads NPR tickets and NPR module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>("Type") == RMMLevel1NPRProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }
                nprModuleRow = moduleViewManager.GetByName("NPR");
                string nprQuery = string.Join(" or ", queryExps);
                DataTable listID = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRRequest, $"TenantID='{_context.TenantID}'");
                nprData = listID.Select(nprQuery).CopyToDataTable();
                queryExps = new List<string>();
            }

            UGITModule tskModuleRow = null;
            DataTable tskData = null;
            //if TSK work item is exist then loads TSK tickets and TSK module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>("Type") == RMMLevel1TSKProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                tskModuleRow = moduleViewManager.GetByName("TSK");
                string tskQuery = string.Join(" or ", queryExps);
                DataTable listID = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, $"TenantID='{_context.TenantID}'");
                tskData = listID.Select(tskQuery).CopyToDataTable();

                queryExps = new List<string>();
            }

            UGITModule pmmModuleRow = null;
            DataTable pmmData = new DataTable();
            //if PMM work item is exist then loads PMM tickets and PMM module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>("Type") == RMMLevel1PMMProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, tID));
                }

                pmmModuleRow = moduleViewManager.GetByName("PMM");
                string pmmQuery = string.Join(" or ", queryExps);
                DataTable listID = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                pmmData = listID.Select(pmmQuery).CopyToDataTable();

                queryExps = null;
            }

            foreach (DataRow itemRow in resultTable.Rows)
            {
                if (Convert.ToString(itemRow["Type"]) == RMMLevel1NPRProjects && nprData != null && nprData.Rows.Count > 0)
                {
                    DataRow[] items = nprData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(nprModuleRow,
                                Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);

                        string lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                        if (!string.IsNullOrEmpty(lookupVal))
                        {
                            itemRow["SubWorkItem"] = lookupVal;

                            DataRow spTSKItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.NPRTasks, DatabaseObjects.Columns.ID, lookupVal)[0];
                            if (spTSKItem != null)
                            {
                                string url = string.Format("{0}?projectID={2}&taskID={1}&moduleName={3}", UGITUtility.GetAbsoluteURL("/_layouts/ugovernit/EditTask.aspx"), lookupVal, Convert.ToString(items[0][DatabaseObjects.Columns.Id]), "NPR");
                                string prefix = "javascript:window.parent.parent.UgitOpenPopupDialog(";
                                string navigationUrl = string.Format(prefix + "\"{0}\",\"EditTask\",\"\",\"800px\",\"90\")", url, lookupVal);
                                itemRow["SubWorkItemLink"] = string.Format("<a href='{0}'>{1}</a>", navigationUrl, lookupVal);
                            }
                            else
                            {
                                itemRow["SubWorkItemLink"] = string.Format("{0}", lookupVal);
                            }
                        }

                        //If project is closed then user cann't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow["ShowEditButtons"] = false;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else if (Convert.ToString(itemRow["Type"]) == RMMLevel1PMMProjects && pmmData != null && pmmData.Rows.Count > 0)
                {
                    DataRow[] items = pmmData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(pmmModuleRow,
                            Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);

                        string lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                        if (!configurationVariableMgr.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems))
                        {
                            if (!string.IsNullOrEmpty(lookupVal))
                            {
                                itemRow["SubWorkItem"] = lookupVal;
                                DataRow spTSKItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.ModuleTasks, DatabaseObjects.Columns.ID, lookupVal)[0];
                                if (spTSKItem != null)
                                {
                                    string url = string.Format("{0}?projectID={2}&taskID={1}&moduleName={3}", UGITUtility.GetAbsoluteURL("/_layouts/ugovernit/EditTask.aspx"), lookupVal, Convert.ToString(items[0][DatabaseObjects.Columns.Id]), "PMM");
                                    string prefix = "javascript:window.parent.parent.UgitOpenPopupDialog(";
                                    string navigationUrl = string.Format(prefix + "\"{0}\",\"EditTask\",\"\",\"800px\",\"90\")", url, lookupVal);
                                    itemRow["SubWorkItemLink"] = string.Format("<a href='{0}'>{1}</a>", navigationUrl, lookupVal);
                                }
                                else
                                {
                                    itemRow["SubWorkItemLink"] = string.Format("{0}", lookupVal);
                                }
                            }
                        }
                        //If project is closed then user cann't edit the entry

                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow["ShowEditButtons"] = false;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else if (Convert.ToString(itemRow["Type"]) == RMMLevel1TSKProjects && tskData != null && tskData.Rows.Count > 0)
                {
                    DataRow[] items = tskData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(tskModuleRow,
                            Convert.ToString(items[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(items[0][DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);

                        string lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                        if (!string.IsNullOrEmpty(lookupVal))
                        {
                            itemRow["SubWorkItem"] = lookupVal;
                            DataRow spTSKItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.TSKTasks, DatabaseObjects.Columns.ID, lookupVal)[0];
                            if (spTSKItem != null)
                            {
                                string url = string.Format("{0}?projectID={2}&taskID={1}&moduleName={3}", UGITUtility.GetAbsoluteURL("/_layouts/ugovernit/EditTask.aspx"), lookupVal, Convert.ToString(items[0][DatabaseObjects.Columns.Id]), "TSK");
                                string prefix = "javascript:window.parent.parent.UgitOpenPopupDialog(";
                                string navigationUrl = string.Format(prefix + "\"{0}\",\"EditTask\",\"\",\"800px\",\"90\")", url, lookupVal);
                                itemRow["SubWorkItemLink"] = string.Format("<a href='{0}'>{1}</a>", navigationUrl, lookupVal);
                            }
                            else
                            {
                                itemRow["SubWorkItemLink"] = string.Format("{0}", lookupVal);
                            }
                        }

                        // If project is closed then user can't edit the entry
                        if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(items[0], DatabaseObjects.Columns.TicketClosed)))
                        {
                            itemRow["ShowEditButtons"] = false;
                            itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                        }
                    }
                }
                else
                {
                    DataRow moduleTaskModuleRow = null;
                    //UGITModule itemModules = ObjModuleViewManager.LoadByName(Convert.ToString(itemRow["Type"]));
                    DataTable drModules = ObjModuleViewManager.GetDataTable($"ModuleName = '{Convert.ToString(itemRow["Type"])}'");  // GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Modules, DatabaseObjects.Columns.Title, Convert.ToString(itemRow["Type"]));
                    if (drModules != null && drModules.Rows.Count==0)
                    {
                        drModules = ObjModuleViewManager.GetDataTable($"{DatabaseObjects.Columns.Title} = '{Convert.ToString(itemRow["Type"])}'");
                    }
                    if (drModules != null && drModules.Rows.Count > 0)
                    {
                        moduleTaskModuleRow = drModules.Rows[0];
                        itemRow["Type"] = moduleTaskModuleRow[DatabaseObjects.Columns.Title];
                    }
                    if (moduleTaskModuleRow != null)
                    {
                        string moduleTaskQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]);
                        DataTable listID = GetTableDataManager.GetTableData(Convert.ToString(moduleTaskModuleRow[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                        DataTable moduleTaskData = listID.Select(moduleTaskQuery).CopyToDataTable();

                        if (moduleTaskData.Rows.Count > 0)
                        {
                            itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(moduleTaskData.Rows[0][DatabaseObjects.Columns.Title]);
                            itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("<a href='{0}'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(moduleTaskModuleRow,
                                Convert.ToString(moduleTaskData.Rows[0][DatabaseObjects.Columns.TicketId]), true, Convert.ToString(moduleTaskData.Rows[0][DatabaseObjects.Columns.Title])).NavigateUrl, itemRow[DatabaseObjects.Columns.WorkItem]);

                            string lookupVal = Convert.ToString(itemRow["SubWorkItem"]);
                            if (!string.IsNullOrEmpty(lookupVal))
                            {
                                itemRow["SubWorkItem"] = lookupVal;
                            }

                            //If project is closed then user cann't edit the entry
                            if (UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(moduleTaskData.Rows[0], DatabaseObjects.Columns.TicketClosed)))
                            {
                                itemRow["ShowEditButtons"] = false;
                                itemRow[DatabaseObjects.Columns.Title] = string.Format("<b style='color:red;'>(Closed)</b> {0}", itemRow[DatabaseObjects.Columns.Title]);
                            }
                        }
                    }
                }
            }

            return resultTable;
        }

        private DataTable CreateTableSchema()
        {
            DataTable timeSheet = new DataTable();

            timeSheet.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            timeSheet.Columns.Add("WorkItemID", typeof(int));
            timeSheet.Columns.Add("Type", typeof(string));
            timeSheet.Columns.Add("TypeName", typeof(string));
            timeSheet.Columns.Add("WorkItem", typeof(string));
            timeSheet.Columns.Add("SubWorkItem", typeof(string));
            timeSheet.Columns.Add("SubSubWorkItem", typeof(string));
            timeSheet.Columns.Add("OriginalSubWorkItem", typeof(string));
            timeSheet.Columns.Add("WorkItemPublicId", typeof(string));
            timeSheet.Columns.Add("WorkItemLink", typeof(string));
            timeSheet.Columns.Add("WeekDay1", typeof(double));
            timeSheet.Columns.Add("WeekDay2", typeof(double));
            timeSheet.Columns.Add("WeekDay3", typeof(double));
            timeSheet.Columns.Add("WeekDay4", typeof(double));
            timeSheet.Columns.Add("WeekDay5", typeof(double));
            timeSheet.Columns.Add("WeekDay6", typeof(double));
            timeSheet.Columns.Add("WeekDay7", typeof(double));
            timeSheet.Columns.Add("PRPHours", typeof(double));
            timeSheet.Columns.Add("ShowEditButtons", typeof(bool));
            timeSheet.Columns.Add("ShowDeleteButton", typeof(bool));
            timeSheet.Columns.Add("EstimatedRemainingHours", typeof(double));
            timeSheet.Columns.Add("SubWorkItemLink", typeof(string));
            timeSheet.Columns.Add("SubSubWorkItemLink", typeof(string));
            timeSheet.Columns.Add("SubWorkItemTitle", typeof(string));
            timeSheet.Columns.Add("ItemOrder", typeof(int)); timeSheet.Columns.Add("Comment1", typeof(string));
            timeSheet.Columns.Add("Comment2", typeof(string));
            timeSheet.Columns.Add("Comment3", typeof(string));
            timeSheet.Columns.Add("Comment4", typeof(string));
            timeSheet.Columns.Add("Comment5", typeof(string));
            timeSheet.Columns.Add("Comment6", typeof(string));
            timeSheet.Columns.Add("Comment7", typeof(string));
            timeSheet.Columns.Add("ID1", typeof(int));
            timeSheet.Columns.Add("ID2", typeof(int));
            timeSheet.Columns.Add("ID3", typeof(int));
            timeSheet.Columns.Add("ID4", typeof(int));
            timeSheet.Columns.Add("ID5", typeof(int));
            timeSheet.Columns.Add("ID6", typeof(int));
            timeSheet.Columns.Add("ID7", typeof(int));
            timeSheet.Columns.Add("ListName", typeof(string));
            return timeSheet;
        }

        public DataTable LoadRawTableByResource(string user, DateTime startDate, DateTime endDate)
        {
            DataTable resultedTable = null;

            List<string> requiredQuery = new List<string>();
            if (!string.IsNullOrEmpty(user))
            {
                requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Resource, user));
            }
            else
            {
                requiredQuery.Add(string.Format("{0}='{1}", DatabaseObjects.Columns.Resource, user));
            }

            requiredQuery.Add(string.Format("{0}>='{1}'", DatabaseObjects.Columns.WorkDate, startDate));
            requiredQuery.Add(string.Format("{0}<='{1}'", DatabaseObjects.Columns.WorkDate, endDate));

            try
            {
                string sQuery = string.Join(" and ", requiredQuery);
                DataTable listTable = this.GetDataTable();
                DataRow[] rows = listTable.Select(sQuery);
                if (rows != null && rows.Count() > 0)
                    resultedTable = rows.CopyToDataTable();
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }

            return resultedTable;
        }
        public ResourceTimeSheet Save(ResourceTimeSheet resourceTimeSheet)
        {
            if (resourceTimeSheet.ID > 0)
                this.Update(resourceTimeSheet);
            else
                this.Insert(resourceTimeSheet);
            return resourceTimeSheet;
        }
        public void UpdateWorkingHours(ResourceWorkItems workItem, string userID, DateTime workDate, double hours, bool overrideHours, bool updateDistribution = true)
        {
            UserProfile user = ObjUserProfileManager.GetUserById(userID);

            //Gets workitems of selected users
            ResourceWorkItems workItemInfo = workItem;
            if (workItem.ID == 0)
                workItemInfo = resourceWorkItemsManager.LoadByWorkItem(userID.ToString(), workItem.WorkItemType, workItem.WorkItem, workItem.SubWorkItem);
            else
                workItemInfo = resourceWorkItemsManager.LoadByID(workItem.ID);

            //Create work item if work item is not found to entry actual hours againt it.
            if (workItemInfo == null)
            {
                workItemInfo = new ResourceWorkItems(userID);
                workItemInfo.WorkItemType = workItem.WorkItemType;
                workItemInfo.WorkItem = workItem.WorkItem;
                workItemInfo.SubWorkItem = workItem.SubWorkItem;
                resourceWorkItemsManager.Insert(workItemInfo);
            }

            ResourceTimeSheet workSheetItem = null;
            if (!overrideHours)
                workSheetItem = this.LoadSheetItem(userID.ToString(), workDate, workItemInfo.ID);

            if (workSheetItem == null)
                workSheetItem = new ResourceTimeSheet(userID, workDate);

            //Only accept 24hr in a day
            if (hours > 24)
                hours = 24;
            if (workSheetItem.HoursTaken + hours >= 0)
                workSheetItem.HoursTaken = Convert.ToInt32(workSheetItem.HoursTaken + hours);
            else
                workSheetItem.HoursTaken = 0;

            if (overrideHours)
                workSheetItem.HoursTaken = Convert.ToInt32(hours);

            workSheetItem.ResourceWorkItemLookup = workItemInfo.ID;
            workSheetItem.ResourceWorkItem = workItemInfo;
            this.Save(workSheetItem);

            //Start Distribution logic to distribute hours in monthly and weekly list
            string webUrl = HttpContext.Current.Request.Url.ToString();
            List<long> workdItems = new List<long>();
            workdItems.Add(workItemInfo.ID);

            if (updateDistribution)
            {
                //Start Thread to update rmm summary list for current sheet entries
                ThreadStart threadStartMethod = delegate ()
                {
                    //RMMSummaryHelper.UpdateActualInRMMSummary(workdItems, userID, webUrl, workDate);
                };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
        }

        #region create task level timesheet

        private bool CreateTaskLevelTimeSheet(ApplicationContext context, string userId, DateTime startDate, DataRow workItem, DataTable resultTable)
        {
            TicketHoursManager tHelper = new TicketHoursManager(_context);
            //exclude svc from here and non module
            string workItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
            string subWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);

            if (workItemType == "Current Projects (PMM)" || workItemType == "Project Management Module (PMM)" || workItemType.Contains("(PMM)"))
                workItemType = "PMM";

            if (workItemType == "Opportunity Management (OPM)" || workItemType.Contains("(OPM)"))
                workItemType = ModuleNames.OPM;

            if (workItemType == "Project Management (CPR)" || workItemType.Contains("(CPR)"))
                workItemType = ModuleNames.CPR;

            if (workItemType == "Service Projects (CNS)" || workItemType.Contains("(CNS)"))
                workItemType = ModuleNames.CNS;

            UGITModule module = ObjModuleViewManager.GetByName(workItemType); //uGITCache.ModuleConfigCache.GetCachedModule(spWeb, Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]));
            if (module == null || module.ModuleName == "SVC")
                return false;

            //Create Standard work items tasks if EnableProjStdWorkItems configuration is enable

            if (tHelper.IsStdWorkItemEnable(context, module.ModuleName, startDate))
                return CreateProjStdWorkItemTasksTimeSheet(context, userId, startDate, workItem, resultTable);

            //only create task level timesheet if actual hours by user is enable at module level. 
            if (!tHelper.IsActualHoursByUserEnable(context, module.ModuleName))
                return false;

            // Get all project tasks
            List<UGITTask> allTasks = uGITTaskManager.LoadByProjectID(Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]), Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]));
            if (allTasks == null || allTasks.Count == 0)
                return true;

            DateTime endDate = startDate.AddDays(6).Date;

            List<UGITTask> weekTasks = allTasks.Where(x => x.ChildCount == 0 && x.AssignedTo != null && x.AssignedTo.Contains(userId) &&
                                                      startDate.Date <= x.DueDate && endDate >= x.StartDate.Date).OrderBy(x => x.ItemOrder).ToList();

            //Get data as week range from ticket hours
            DataTable ticketHours = tHelper.GetWorkHours(Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]),
                                                         Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]),
                                                         userId, 0, startDate, startDate.AddDays(6));

            // In case if actual hours reported for some task in current week irrespective thier end date
            string moduleName = uHelper.getModuleNameByTicketId(Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]));
            if (ticketHours != null && ticketHours.Rows.Count > 0)
            {
                List<UGITTask> reportedTasks = null;

                // reported task ids reported in curret selected week range
                List<string> taskids = ticketHours.AsEnumerable().Where(y => !y.IsNull(DatabaseObjects.Columns.TaskID)).Select(x => x.Field<string>(DatabaseObjects.Columns.TaskID)).Distinct().ToList();
                if (taskids != null && taskids.Count > 0)
                    reportedTasks = allTasks.Where(x => taskids.Contains(Convert.ToString(x.ID))).ToList();

                // Merge assigned tasks with tasks that have reported hours
                if (weekTasks == null || weekTasks.Count == 0)
                    weekTasks = reportedTasks;
                else if (reportedTasks != null && reportedTasks.Count > 0)
                    weekTasks = reportedTasks.Union(weekTasks).ToList();
            }

            if (weekTasks == null || weekTasks.Count == 0)
                return true;

            DataRow sheetRow = null;
            DateTime tempDate = startDate;
            foreach (UGITTask task in weekTasks)
            {
                DataRow tRow = resultTable.NewRow();
                tRow["WorkItemID"] = 0;
                tRow["Type"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                tRow["TypeName"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                tRow["WorkItem"] = workItem[DatabaseObjects.Columns.WorkItem];
                tRow[DatabaseObjects.Columns.WorkItemLink] = workItem[DatabaseObjects.Columns.WorkItem];
                tRow["SubWorkItem"] = string.Format("{0}{1}{2}", task.ID, Constants.Separator, task.Title);
                tRow["OriginalSubWorkItem"] = tRow["SubWorkItem"];
                tRow["SubWorkItemLink"] = task.Title;
                tRow["SubWorkItemTitle"] = task.Title;
                tRow["ShowEditButtons"] = true;
                tRow["ShowDeleteButton"] = true;
                tRow["EstimatedRemainingHours"] = task.EstimatedRemainingHours;
                resultTable.Rows.Add(tRow);

                tempDate = startDate;
                for (int i = 1; i <= 7; i++)
                {
                    tRow["WeekDay" + i] = 0;
                    if (ticketHours != null)
                        sheetRow = ticketHours.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TaskID) == task.ID.ToString() && x.Field<DateTime>(DatabaseObjects.Columns.WorkDate).Date == tempDate.Date);
                    if (sheetRow != null)
                    {
                        tRow["WeekDay" + i] = Convert.ToDouble(sheetRow[DatabaseObjects.Columns.HoursTaken]);
                        tRow["Comment" + i] = Convert.ToString(sheetRow[DatabaseObjects.Columns.TicketComment]);
                        tRow["ID" + i] = Convert.ToInt32(sheetRow[DatabaseObjects.Columns.Id]);
                    }

                    tempDate = tempDate.AddDays(1);
                }
                tRow["ListName"] = DatabaseObjects.Tables.TicketHours;
            }
            return true;
        }

        private  bool CreateProjStdWorkItemTasksTimeSheet(ApplicationContext context, string userId, DateTime startDate, DataRow workItem, DataTable resultTable)
        {
            TicketHoursManager tHelper = new TicketHoursManager(_context);

            string workItemType = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
            string subWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);

            if (workItemType == "Current Projects (PMM)" || workItemType == "Project Management Module (PMM)" || workItemType.Contains("(PMM)"))
                workItemType = "PMM";

            if (workItemType == "Opportunity Management (OPM)" || workItemType.Contains("(OPM)"))
                workItemType = ModuleNames.OPM;

            if (workItemType == "Project Management (CPR)" || workItemType.Contains("(CPR)"))
                workItemType = ModuleNames.CPR;

            if (workItemType == "Service Projects (CNS)" || workItemType.Contains("(CNS)"))
                workItemType = ModuleNames.CNS;

            UGITModule module = ObjModuleViewManager.GetByName(workItemType); 

            //only create stand work item for project.
            if (module == null || (module.ModuleName != "PMM" && module.ModuleName != "CPR" && module.ModuleName != "CNS" && module.ModuleName != "OPM"))
                return false;

            //ProjectStandardWorkItemManager standardworkitemManager = new ProjectStandardWorkItemManager(context);
            //DataTable items = standardworkitemManager.GetDataTable();
            string query = string.Empty;

            string uniqueWorkItem = $"{workItem[DatabaseObjects.Columns.WorkItem]};#{workItem[DatabaseObjects.Columns.SubWorkItem]}";

            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(workItem[DatabaseObjects.Columns.SubSubWorkItem])))
            {
                string[] arrSubWorkItems = UGITUtility.SplitString(workItem[DatabaseObjects.Columns.SubSubWorkItem], Constants.Separator);
                //arrSubWorkItems.ElementAtOrDefault(2)
                if (arrSubWorkItems.ElementAtOrDefault(0) != null)
                    query = $"{DatabaseObjects.Columns.Title} = '{arrSubWorkItems[0]}'";
                if (arrSubWorkItems.ElementAtOrDefault(1) != null)
                    query = query + $" and Code = '{arrSubWorkItems[1]}'";
                //if (arrSubWorkItems.ElementAtOrDefault(2) != null)
                //    query = query + $" and {DatabaseObjects.Columns.Description} = '{arrSubWorkItems[2]}'";

                query = query + $" and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.Deleted} = 'False'";
            }
            //else
            //{
            //    query = $" {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.Deleted} = 'False'";
            //}

            DateTime tempDate = startDate;
            DataTable ticketHours = tHelper.GetWorkHours(workItemType,
                                                             Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]),
                                                             userId, 0, startDate, startDate.AddDays(6), subWorkItem);
            DataRow sheetRow = null;


            if (!string.IsNullOrEmpty(query))
            {
                //DataTable items = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectStandardWorkItems, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.Deleted} = 'False'");
                DataTable items = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectStandardWorkItems, query);

                if (items.Rows.Count <= 0)
                    return false;
                DataRow[] workitems = items.Select(); // replaced by items;


                foreach (var task in workitems)
                {
                    DataRow tRow = resultTable.NewRow();
                    tRow["WorkItemID"] = 0;
                    tRow["Type"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                    tRow["TypeName"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                    tRow["WorkItem"] = workItem[DatabaseObjects.Columns.WorkItem];
                    tRow[DatabaseObjects.Columns.WorkItemLink] = workItem[DatabaseObjects.Columns.WorkItem];
                    tRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];
                    tRow[DatabaseObjects.Columns.SubSubWorkItem] = string.Format("{0}{1}{2}{1}{3}{1}{4}", task[DatabaseObjects.Columns.ID], Constants.Separator, task[DatabaseObjects.Columns.Title], task["Code"], task[DatabaseObjects.Columns.Description]);
                    //tRow[DatabaseObjects.Columns.SubSubWorkItemLink] = string.Format("{0} {1}", task["Code"], task[DatabaseObjects.Columns.Title]);
                    tRow[DatabaseObjects.Columns.SubSubWorkItemLink] = string.Format("{0} : {1}", task["Code"], task[DatabaseObjects.Columns.Description]);
                    tRow["OriginalSubWorkItem"] = tRow["SubWorkItem"];
                    //tRow["SubWorkItemLink"] = string.Format("{0}", task[DatabaseObjects.Columns.Title]);
                    //tRow["SubWorkItemLink"] = string.Format("{0} {1}", task["Code"], task[DatabaseObjects.Columns.Title]);

                    if (!previousSubWorkItem.Contains(uniqueWorkItem))
                    {
                        tRow["SubWorkItemLink"] = workItem[DatabaseObjects.Columns.SubWorkItem];
                        previousSubWorkItem.Add(uniqueWorkItem);
                    }

                    tRow["SubWorkItemTitle"] = string.Format("{0}. {1}", task[DatabaseObjects.Columns.ItemOrder], task[DatabaseObjects.Columns.Title]);
                    tRow["ShowEditButtons"] = true;
                    tRow["ShowDeleteButton"] = true;
                    tRow["EstimatedRemainingHours"] = 0;
                    tRow[DatabaseObjects.Columns.ItemOrder] = UGITUtility.StringToInt(task[DatabaseObjects.Columns.ItemOrder]);
                    resultTable.Rows.Add(tRow);

                    tempDate = startDate;
                    for (int i = 1; i <= 7; i++)
                    {
                        tRow["WeekDay" + i] = 0;
                        if (ticketHours != null)
                            sheetRow = ticketHours.AsEnumerable().FirstOrDefault(x => UGITUtility.StringToBoolean(x.Field<string>(DatabaseObjects.Columns.StandardWorkItem)) && x.Field<DateTime>(DatabaseObjects.Columns.WorkDate).Date == tempDate.Date && x.Field<string>(DatabaseObjects.Columns.TaskID) == Convert.ToString(task[DatabaseObjects.Columns.ID]) && x.Field<string>(DatabaseObjects.Columns.SubWorkItem) == subWorkItem);       //!x.IsNull(DatabaseObjects.Columns.StandardWorkItem) &&
                        if (sheetRow != null)
                        {
                            tRow["WeekDay" + i] = Convert.ToDouble(sheetRow[DatabaseObjects.Columns.HoursTaken]);
                            tRow["Comment" + i] = Convert.ToString(sheetRow[DatabaseObjects.Columns.TicketComment]);
                            tRow["ID" + i] = Convert.ToInt32(sheetRow[DatabaseObjects.Columns.Id]);
                        }
                        tempDate = tempDate.AddDays(1);
                    }
                    tRow["ListName"] = DatabaseObjects.Tables.TicketHours;
                } 
            }
            else
            {
                DataRow tRow = resultTable.NewRow();
                tRow["WorkItemID"] = 0;
                tRow["Type"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                tRow["TypeName"] = Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]);
                tRow["WorkItem"] = workItem[DatabaseObjects.Columns.WorkItem];
                tRow[DatabaseObjects.Columns.WorkItemLink] = workItem[DatabaseObjects.Columns.WorkItem];
                tRow[DatabaseObjects.Columns.SubWorkItem] = workItem[DatabaseObjects.Columns.SubWorkItem];
                tRow[DatabaseObjects.Columns.SubSubWorkItem] = string.Empty;
                tRow["OriginalSubWorkItem"] = tRow["SubWorkItem"];
                //tRow["SubWorkItemLink"] = string.Format("{0}", task[DatabaseObjects.Columns.Title]);
                //tRow["SubWorkItemLink"] = string.Format("{0} {1}", task["Code"], task[DatabaseObjects.Columns.Title]);
                tRow["SubWorkItemLink"] = workItem[DatabaseObjects.Columns.SubWorkItem];
                tRow["SubWorkItemTitle"] = string.Empty; //string.Format("{0}. {1}", task[DatabaseObjects.Columns.ItemOrder], task[DatabaseObjects.Columns.Title]);
                tRow["ShowEditButtons"] = true;
                tRow["ShowDeleteButton"] = true;
                tRow["EstimatedRemainingHours"] = 0;
                tRow[DatabaseObjects.Columns.ItemOrder] = 0; // UGITUtility.StringToInt(task[DatabaseObjects.Columns.ItemOrder]);
                resultTable.Rows.Add(tRow);

                tempDate = startDate;
                for (int i = 1; i <= 7; i++)
                {
                    tRow["WeekDay" + i] = 0;
                    if (ticketHours != null)
                        sheetRow = ticketHours.AsEnumerable().FirstOrDefault(x => UGITUtility.StringToBoolean(x.Field<string>(DatabaseObjects.Columns.StandardWorkItem)) && x.Field<DateTime>(DatabaseObjects.Columns.WorkDate).Date == tempDate.Date && x.Field<string>(DatabaseObjects.Columns.TaskID) == "0" && x.Field<string>(DatabaseObjects.Columns.SubWorkItem) == subWorkItem);       //!x.IsNull(DatabaseObjects.Columns.StandardWorkItem) &&
                    if (sheetRow != null)
                    {
                        tRow["WeekDay" + i] = Convert.ToDouble(sheetRow[DatabaseObjects.Columns.HoursTaken]);
                        tRow["Comment" + i] = Convert.ToString(sheetRow[DatabaseObjects.Columns.TicketComment]);
                        tRow["ID" + i] = Convert.ToInt32(sheetRow[DatabaseObjects.Columns.Id]);
                    }
                    tempDate = tempDate.AddDays(1);
                }
                tRow["ListName"] = DatabaseObjects.Tables.TicketHours;
            }
            return true;
        }


        public void UpdateTimesheet(List<Tuple<string, DateTime>> datesWithUsers, string rmmCategory, List<string> workItems, string moduleName = null, string ticketID = null)
        {
            workItems = workItems.Distinct().ToList();
            foreach (string workItem in workItems)
            {
                UpdateTimesheet(datesWithUsers, rmmCategory, workItem, string.Empty, moduleName, ticketID);
            }
        }

        public void UpdateTimesheet(List<Tuple<string, DateTime>> datesWithUsers, string rmmCategory, string workItem, string subWorkItem, string moduleName = null, string ticketID = null)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                moduleName = rmmCategory;

            if (string.IsNullOrWhiteSpace(ticketID))
                ticketID = workItem;

            datesWithUsers = datesWithUsers.Distinct().ToList();
            TicketHoursManager tHelper = new TicketHoursManager(_context);
            var lookup = datesWithUsers.ToLookup(x => x.Item1);
            foreach (var l in lookup)
            {
                //Keep week start with resource id to run actual hours distribution once for conbination
                List<Tuple<long, DateTime>> distributionWeeks = new List<Tuple<long, DateTime>>();

                string userID = l.Key;

                ResourceWorkItems workItemObj = resourceWorkItemsManager.LoadByWorkItem(userID.ToString(), rmmCategory, workItem, subWorkItem);
                if (workItemObj == null)
                {
                    workItemObj = new ResourceWorkItems(userID);
                    workItemObj.WorkItemType = rmmCategory;
                    workItemObj.WorkItem = workItem;
                    workItemObj.SubWorkItem = subWorkItem;
                    resourceWorkItemsManager.Insert(workItemObj);
                    //workItemObj.(spWeb);
                }
                ResourceTimeSheetManager timeSheetManager = new ResourceTimeSheetManager(_context);
                List<ResourceTimeSheet> timesheetList = timeSheetManager.LoadActualByWorkItem(workItemObj.ID);

                List<DateTime> weekStartDates = new List<DateTime>();

                foreach (Tuple<string, DateTime> item in l.ToList())
                {
                    List<int> updatedWorkItems = new List<int>();
                    List<ActualHour> data = tHelper.GetWorkHoursCollection(moduleName, workItem, userID, 0, startDate: item.Item2, endDate: item.Item2);
                    double hours = 0;
                    if (data != null)
                        hours = data.Sum(x => x.HoursTaken);
                    //hours = (double)data.Compute(string.Format("sum({0})", DatabaseObjects.Columns.HoursTaken), string.Empty);

                    ResourceTimeSheet sheetItem = null;
                    if (timesheetList != null)
                    {
                        List<ResourceTimeSheet> sItems = timesheetList.Where(x => x.WorkDate == item.Item2.Date).ToList();
                        sheetItem = sItems.FirstOrDefault();

                        if (sItems.Count > 1)
                        {   //Delete extra entries from timesheet if any
                            //system need to keep only single entry
                            sItems.RemoveAt(0);
                            //SPList timeSheetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ResourceTimeSheet, spWeb);
                            //SPListHelper.DeleteBatch(spWeb, timeSheetList, sItems.Select(x => x.WorkId).ToList());
                            timeSheetManager.Delete(sItems);
                        }
                    }

                    if (sheetItem == null)
                    {
                        sheetItem = new ResourceTimeSheet(userID, item.Item2);
                        sheetItem.ResourceWorkItem = workItemObj;
                        sheetItem.ResourceWorkItemLookup = workItemObj.ID;
                    }

                    sheetItem.HoursTaken = Convert.ToInt32(hours);
                    if (sheetItem.ID > 0)
                        timeSheetManager.Update(sheetItem);
                    else
                        timeSheetManager.Insert(sheetItem);
                    //sheetItem.Save(spWeb);

                    distributionWeeks.Add(new Tuple<long, DateTime>(workItemObj.ID, uHelper.GetWeekStartDate(item.Item2)));
                }

                distributionWeeks = distributionWeeks.Distinct().ToList();
                foreach (var dItem in distributionWeeks)
                {
                    RMMSummaryHelper.UpdateActualInRMMSummary(_context, new List<long>() { dItem.Item1 }, userID, dItem.Item2);
                }
            }
        }
        #endregion
    }
    public interface IResourceTimeSheetManager : IManagerBase<ResourceTimeSheet>
    {

    }
}
