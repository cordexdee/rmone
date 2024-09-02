
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using System.Linq.Expressions;
using uGovernIT.Utility.Helpers;
using uGovernIT.Util.Log;
using System.Threading;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Helpers;


namespace uGovernIT.Manager
{
    public class TicketHoursManager : ManagerBase<ActualHour>, ITicketHoursManager
    {
        ApplicationContext _context;
        ModuleViewManager ObjModuleViewManager = null;
        ConfigurationVariableManager ObjConfigurationVariableHelper = null;
        ResourceTimeSheetManager ObjResourceTimeSheetManager = null;
        ResourceWorkItemsManager ObjResourceWorkItemsManager = null;
        UGITTaskManager taskManager = null;
        ModuleViewManager moduleManager = null;
        public TicketHoursManager(ApplicationContext context) : base(context)
        {
            store = new TicketHoursStore(this.dbContext);
            _context = context;
            //DataTable list = GetDataTable();   // GetTableData(DatabaseObjects.Tables.TicketHours, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            ObjModuleViewManager = new ModuleViewManager(context);
            ObjConfigurationVariableHelper = new ConfigurationVariableManager(context);
            ObjResourceTimeSheetManager = new ResourceTimeSheetManager(context);
            ObjResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            taskManager = new UGITTaskManager(context);
            moduleManager = new ModuleViewManager(context);
        }
        public List<ActualHour> TicketHourList()
        {
            List<ActualHour> objModuleUserType = new List<ActualHour>();
            objModuleUserType = store.Load();
            return objModuleUserType;
        }
        public List<ActualHour> TicketHourList(string TicketID)
        {
            List<ActualHour> objModuleUserType = new List<ActualHour>();
            objModuleUserType = store.Load(x => x.TicketID == TicketID).ToList();
            return objModuleUserType;
        }
        public DataTable TicketHours()
        {
            DataTable aResultDT = UGITUtility.ToDataTable(TicketHourList());
            return aResultDT;
        }
        public DataTable TicketHours(string TicketID)
        {
            List<ActualHour> objModuleUserType = TicketHourList(TicketID);
            DataTable aResultDT = UGITUtility.ToDataTable(objModuleUserType);
            return aResultDT;
        }
        public ActualHour AddUpdate(ActualHour relation)
        {
            if (relation != null)
            {
                if (relation.ID > 0)
                    store.Update(relation);
                else
                    store.Insert(relation);
            }
            else
            {
                relation = null;
            }
            return relation;
        }

        public void CreateOrUpdateOrDeleteActualHours(string action, ActualHourHelper hourHelper, string ModuleName, bool isTask = false)
        {
            string rmmCategory = ModuleName;
            string workItem = hourHelper.WorkItem;

            List<string> workItemInvolved = new List<string>();
            ModuleViewManager moduleManager = new ModuleViewManager(_context);

            //Update user working hours inside resource timesheet if setting is enabled            
            if (!string.IsNullOrEmpty(ModuleName) && !isTask)
            {
                UGITModule uModule = moduleManager.LoadByName(ModuleName);

                if (uModule != null && uModule.ActualHoursByUser)
                {
                    workItem = null;

                    if (hourHelper != null && hourHelper.RequestTypeLookup > 0)
                    {
                        ModuleRequestType requestType = uModule.List_RequestTypes.FirstOrDefault(x => x.ID == hourHelper.RequestTypeLookup);

                        if (requestType != null)
                        {
                            rmmCategory = requestType.Category;
                            workItem = requestType.Category;
                            hourHelper.WorkItem = workItem;
                        }
                    }
                }
            }

            List<Tuple<string, DateTime>> updatedItems = new List<Tuple<string, DateTime>>();
            if (action == "Create")
            {
                #region InsertNewValues 
                if (hourHelper.TimeSpent > 0 || !string.IsNullOrEmpty(hourHelper.ResolutionDescription))
                {
                    double workHours = UGITUtility.StringToDouble(hourHelper.TimeSpent);
                    DateTime workDate = UGITUtility.StringToDateTime(hourHelper.WorkDate);

                    List<ActualHour> collection = GetWorkHoursCollection(ModuleName, hourHelper.TicketId, _context.CurrentUser.Id, hourHelper.TaskId, workDate.Date, workDate.Date);
                    ActualHour ticketHoursItem = new ActualHour();

                    if (collection.Count > 0)
                    {
                        workHours += UGITUtility.StringToDouble(collection.FirstOrDefault().HoursTaken);
                        //ticketHoursItem = collection.FirstOrDefault();
                        string oldWorkItem = Convert.ToString(ticketHoursItem.WorkItem);

                        if (!string.IsNullOrWhiteSpace(oldWorkItem))
                            workItemInvolved.Add(oldWorkItem);
                    }

                    ticketHoursItem.TicketID = hourHelper.TicketId;
                    ticketHoursItem.WorkItem = hourHelper.WorkItem;
                    ticketHoursItem.StageStep = hourHelper.TicketStageStep;
                    ticketHoursItem.ModuleNameLookup = ModuleName;

                    if (isTask)
                        ticketHoursItem.TaskID = hourHelper.TaskId;

                    if (workHours > 24)
                        workHours = 24;


                    ticketHoursItem.HoursTaken = workHours;
                    ticketHoursItem.Comment = hourHelper.ResolutionDescription;
                    ticketHoursItem.Resource = _context.CurrentUser.Id;
                    ticketHoursItem.WorkDate = workDate;
                    ticketHoursItem.TenantID = _context.TenantID;

                    updatedItems.Add(new Tuple<string, DateTime>(ticketHoursItem.Resource, workDate));

                    //Update week & month start date from WorkDate
                    if (workDate != DateTime.MinValue && workDate != DateTime.MaxValue)
                    {
                        DateTime weekSDate = uHelper.GetWeekStartDate(workDate);//week start day will be monday
                        ticketHoursItem.WeekStartDate = weekSDate.Date;
                        ticketHoursItem.MonthStartDate = uHelper.GetMonthStartDate(workDate).Date;
                    }

                    TicketHoursManager ticketHourManager = new TicketHoursManager(_context);

                    if (hourHelper.ItemId <= 0)
                        ticketHourManager.Insert(ticketHoursItem);
                    else
                        ticketHourManager.Update(ticketHoursItem);
                }
                #endregion
            }
            else if (action == "Update")
            {
                #region UpdateOldValue

                int id = hourHelper.ItemId;
                ActualHour item = LoadByID(id);

                if (item != null)
                {
                    string oldWorkItem = Convert.ToString(item.WorkItem);

                    if (!string.IsNullOrWhiteSpace(oldWorkItem))
                        workItemInvolved.Add(oldWorkItem);

                    DateTime workDate = hourHelper.WorkDate;
                    DateTime oldWorkDate = item.WorkDate;
                    double oldHours = item.HoursTaken;
                    double workHours = hourHelper.TimeSpent;

                    if (workHours > 24)
                        workHours = 24;

                    item.WorkItem = hourHelper.WorkItem;
                    item.TenantID = _context.TenantID;

                    //Update only comment
                    if (workDate.Date == oldWorkDate.Date && workHours == oldHours)
                    {
                        //Week start day will be monday
                        DateTime weekSDate = uHelper.GetWeekStartDate(workDate);
                        item.WeekStartDate = weekSDate.Date;
                        item.MonthStartDate = uHelper.GetMonthStartDate(workDate).Date;
                        item.Comment = hourHelper.ResolutionDescription;
                        Update(item);
                    }
                    //update hours on same date
                    else if (workDate.Date == oldWorkDate.Date)
                    {
                        //Week start day will be monday
                        DateTime weekSDate = uHelper.GetWeekStartDate(workDate);
                        item.WeekStartDate = weekSDate.Date;
                        item.MonthStartDate = uHelper.GetMonthStartDate(workDate).Date;
                        item.HoursTaken = workHours;
                        item.Comment = hourHelper.ResolutionDescription;
                        Update(item);

                        if (!string.IsNullOrEmpty(item.Resource))
                            updatedItems.Add(new Tuple<string, DateTime>(item.Resource, workDate));
                    }
                    //change date value
                    else
                    {
                        List<ActualHour> collection = GetWorkHoursCollection(ModuleName, hourHelper.TicketId, _context.CurrentUser.Id, hourHelper.TaskId, workDate.Date, workDate.Date);

                        if (collection.Count > 0)
                        {
                            //Old entry and update exiting entry 
                            Delete(item);
                            oldHours = collection.FirstOrDefault().HoursTaken;
                            workHours += oldHours;

                            if (workHours > 24)
                                workHours = 24;

                            item = collection[0];
                        }

                        item.HoursTaken = workHours;
                        item.Comment = hourHelper.ResolutionDescription;
                        item.WorkDate = workDate;

                        //Update week & month start date from WorkDate
                        if (workDate != DateTime.MinValue && workDate != DateTime.MaxValue)
                        {
                            DateTime weekSDate = uHelper.GetWeekStartDate(workDate);//Week start day will be monday
                            item.WeekStartDate = weekSDate.Date;
                            item.MonthStartDate = uHelper.GetMonthStartDate(workDate).Date;
                        }

                        Update(item);

                        string userLookup = item.Resource;

                        if (!string.IsNullOrEmpty(userLookup))
                        {
                            updatedItems.Add(new Tuple<string, DateTime>(userLookup, workDate.Date));
                            updatedItems.Add(new Tuple<string, DateTime>(userLookup, oldWorkDate.Date));
                        }
                    }

                    if (oldWorkItem != hourHelper.WorkItem)
                    {
                        List<ActualHour> collection = this.GetWorkHoursCollectionByTicket(ModuleName, hourHelper.TicketId, _context.CurrentUser.Id, 0);
                        foreach (ActualHour cItem in collection)
                        {
                            try
                            {
                                oldWorkItem = Convert.ToString(cItem.WorkItem);

                                if (string.IsNullOrWhiteSpace(oldWorkItem) || hourHelper.WorkItem == oldWorkItem)
                                {
                                    //don't need to update, becuase workitem is same
                                    continue;
                                }

                                workItemInvolved.Add(oldWorkItem);
                                cItem.WorkItem = hourHelper.WorkItem;
                                string userLookup = cItem.Resource;

                                if (!string.IsNullOrEmpty(userLookup))
                                    updatedItems.Add(new Tuple<string, DateTime>(userLookup, cItem.WorkDate));

                                Update(cItem);
                            }
                            catch (Exception ex)
                            {
                                //It may happened, if any one also updating same item on same time.
                                ULog.WriteException(ex);
                            }
                        }
                    }
                }
                #endregion
            }
            else if (action == "Delete")
            {
                #region DeleteValues

                int id = hourHelper.ItemId;
                ActualHour item = LoadByID(id);

                if (item != null)
                {
                    string userLookup = item.Resource;

                    if (!string.IsNullOrEmpty(userLookup))
                        updatedItems.Add(new Tuple<string, DateTime>(userLookup, item.WorkDate));

                    Delete(item);
                }
                #endregion
            }

            //workitem must be present to update timesheet data
            if (!string.IsNullOrWhiteSpace(workItem))
            {
                //Update timeshee against all involved workitems 
                //Generally its one but when request type change then mulitple workitems may come.
                workItemInvolved.Add(workItem);
                workItemInvolved = workItemInvolved.Distinct().ToList();
                ResourceTimeSheetManager timesheetManager = new ResourceTimeSheetManager(_context);
                ThreadStart threadStartMethod = delegate () { timesheetManager.UpdateTimesheet(updatedItems, rmmCategory, workItemInvolved, ModuleName, hourHelper.TicketId); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }

        }

        public List<ActualHour> GetWorkHoursCollection(string moduleName, string workItem, string userID, long taskID, DateTime? startDate = null, DateTime? endDate = null, string subWorkItem = "")
        {
            StringBuilder expression = new StringBuilder();
            expression.Append(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
            expression.Append(string.Format("and {0}='{1}'", DatabaseObjects.Columns.WorkItem, workItem));

            if (!string.IsNullOrEmpty(userID))
                expression.Append(string.Format("and {0}='{1}'", DatabaseObjects.Columns.Resource, userID));

            if (taskID > 0)
                expression.Append(string.Format(" and {0}='{1}'", DatabaseObjects.Columns.TaskID, taskID));

            if (startDate.HasValue && startDate.Value != DateTime.MinValue)
                expression.Append(string.Format(" and CONVERT(DATE, {0})>=CONVERT(DATE, '{1}')", DatabaseObjects.Columns.WorkDate, startDate));

            if (endDate.HasValue && endDate.Value != DateTime.MinValue)
                expression.Append(string.Format(" and CONVERT(DATE, {0})<=CONVERT(DATE, '{1}')", DatabaseObjects.Columns.WorkDate, endDate));

            if (!string.IsNullOrEmpty(subWorkItem))
                expression.Append(string.Format(" and {0}='{1}'", DatabaseObjects.Columns.SubWorkItem, subWorkItem));

            return Load(expression.ToString());
        }

        public List<ActualHour> GetWorkHoursCollectionByTicket(string moduleName, string ticketID, string userID, long taskID, DateTime? startDate = null, DateTime? endDate = null)
        {
            StringBuilder expression = new StringBuilder();
            expression.Append(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
            expression.Append(string.Format("and {0}='{1}'", "TicketID", ticketID));

            if (!string.IsNullOrEmpty(userID))
                expression.Append(string.Format("and {0}='{1}'", DatabaseObjects.Columns.Resource, userID));

            if (taskID > 0)
                expression.Append(string.Format("and {0}='{1}'", DatabaseObjects.Columns.TaskID, taskID));

            if (startDate.HasValue && startDate.Value != DateTime.MinValue)
                expression.Append(string.Format("and CONVERT(DATE, {0})>=CONVERT(DATE, '{1}')", DatabaseObjects.Columns.WorkDate, startDate));

            if (endDate.HasValue && endDate.Value != DateTime.MinValue)
                expression.Append(string.Format("and CONVERT(DATE, {0})<=CONVERT(DATE, '{1}')", DatabaseObjects.Columns.WorkDate, endDate));

            return Load(expression.ToString());
        }

        private static double GetTimesheetActulHours(double hours)
        {
            if (hours <= 0)
                return 0;
            int wholeHours = (int)((hours * 60) / 60);
            int remMin = (int)((hours * 60) % 60);
            if (remMin > 45)
            {
                remMin = 60;
            }
            else if (remMin > 30)
            {
                remMin = 45;
            }
            else if (remMin > 15)
            {
                remMin = 30;
            }
            else if (remMin > 0)
            {
                remMin = 15;
            }

            double actualHours = Math.Round((double)(((double)(wholeHours * 60) + remMin) / 60), 2);
            return actualHours;
        }

        public bool IsStdWorkItemEnable(ApplicationContext context, string module, DateTime requiredDate)
        {
            if (context == null || string.IsNullOrEmpty(module))
                return false;

            bool enable = false;
            bool configEnableStd = ObjConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems);
            //configEnableStd = true;// now it is hardcoded but further have to change when its Admin panel is done (Stdworkitems)
            DateTime enableProjStdWorkItemsAfterDate = UGITUtility.StringToDateTime(ObjConfigurationVariableHelper.GetValue(ConfigConstants.EnableProjStdWorkItemsAfterDate));

            //enable std workitem after some date which is specified in configuration otherwise some as showing earlier.
            //show std workitem when not date specified
            if ((module == "PMM" || module == "CPR" || module == "CNS" || module == "OPM") && configEnableStd && (enableProjStdWorkItemsAfterDate == DateTime.MinValue || requiredDate >= enableProjStdWorkItemsAfterDate.Date))
                enable = true;

            return enable;
        }

        public bool IsActualHoursByUserEnable(ApplicationContext context, string module)
        {
            if (context == null || string.IsNullOrEmpty(module))
                return false;

            UGITModule ugitModule = ObjModuleViewManager.GetByName(module);
            return IsActualHoursByUserEnable(ugitModule);
        }

        public bool IsActualHoursByUserEnable(UGITModule module)
        {
            return (module != null && module.ActualHoursByUser);
        }

        public DataTable GetWorkHours(string moduleName, string workItem, string userID, int taskID = 0, DateTime? startDate = null, DateTime? endDate = null, string subWorkItem = "")
        {
            DataTable temp = null;
            List<ActualHour> collection = GetWorkHoursCollection(moduleName, workItem, userID, taskID, startDate, endDate, subWorkItem);
            if (collection != null && collection.Count() > 0)
            {
                temp = UGITUtility.ToDataTable<ActualHour>(collection);

            }
            return temp;
        }

        //public DataRow[] GetWorkHoursCollection(string moduleName, string workItem, string userID, int taskID = 0, DateTime? startDate = null, DateTime? endDate = null)
        //{
        //    List<string> expressions = new List<string>();
        //    expressions.Add(string.Format(" {0}= '{1}'", DatabaseObjects.Columns.ModuleName, moduleName));

        //    expressions.Add(string.Format(" {0}= '{1}' ", DatabaseObjects.Columns.WorkItem, workItem));

        //    if (!string.IsNullOrEmpty(userID))
        //        expressions.Add(string.Format(" {0}= '{1}'", DatabaseObjects.Columns.Resource, userID));

        //    if ((taskID > 0))
        //        expressions.Add(string.Format(" {0}= {1} ", DatabaseObjects.Columns.TaskID, taskID));

        //    string query = string.Empty;
        //    if (expressions.Count > 0)
        //    {
        //        query = string.Join("AND ", expressions);
        //    }

        //    return GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketHours).Select(query);
        //}


        public List<long> CreateResourceTimesheetsEntries(ApplicationContext context, string selectedUserID, DateTime weekStartDate, List<WorkItemHours> weekWorkSheet)
        {
            List<long> changedWorkItemsID = new List<long>();

            ResourceTimeSheetManager ObjResourceTimeSheetManager = new ResourceTimeSheetManager(context);
            string workItemQuery = string.Format("{1}='{0}'", selectedUserID, DatabaseObjects.Columns.Resource);
            
            DataTable workItemCollections = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"{workItemQuery} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            StringBuilder queryString = new StringBuilder();
            queryString.AppendFormat("{0}>=#{1}#", DatabaseObjects.Columns.WorkDate, weekStartDate);
            queryString.AppendFormat(" and {0}<=#{1}#", DatabaseObjects.Columns.WorkDate, weekStartDate.AddDays(6));
            queryString.AppendFormat(" and {0}=#{1}#", DatabaseObjects.Columns.Resource, selectedUserID);
            string query = queryString.ToString();
            List<ResourceTimeSheet> timesheetCollections = ObjResourceTimeSheetManager.Load(x => x.WorkDate >= weekStartDate && x.WorkDate <= weekStartDate.AddDays(6) && x.Resource == selectedUserID).ToList();
            //Starts loop for workitemhours which contains hours for each day for each editable workItem
            foreach (WorkItemHours work in weekWorkSheet)
            {
                DataRow[] wdataRows = workItemCollections.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, work.WorkItemID));
                if (wdataRows != null && wdataRows.Count() > 0)
                {
                    DataRow wItem = wdataRows[0];
                    //Continues loop if workitem id is 0 or its not the work item of selected user
                    if (wItem == null)
                    {
                        continue;
                    }
                    int existingEntryID = 0;
                    //Gets existing timesheet of current work item
                    List<ResourceTimeSheet> workItems = new List<ResourceTimeSheet>();
                    if (timesheetCollections != null && timesheetCollections.Count > 0)
                    {
                        workItems = timesheetCollections.Where(y => y.ResourceWorkItemLookup == work.WorkItemID).ToList();
                    }
                    //Starts loop to save hours for week. so loop for each day
                    for (int i = 0; i < 7; i++)
                    {
                        //-1 means no change in the existing hours
                        if (work.GetHoursOfDay(weekStartDate.AddDays(i).DayOfWeek) < 0)
                        {
                            continue;
                        }
                        //Gets existing timesheet id for selected  user, selected workItem, selected date
                        existingEntryID = 0;
                        if (workItems.Count() > 0)
                        {
                            ResourceTimeSheet workItemActual = workItems.FirstOrDefault(x => Convert.ToDateTime(x.WorkDate).Date == weekStartDate.Date.AddDays(i));
                            if (workItemActual != null)
                                int.TryParse(Convert.ToString(workItemActual.ID), out existingEntryID);
                        }
                        ResourceTimeSheet spListItem = timesheetCollections.FirstOrDefault(z => z.ID == existingEntryID);
                        if (spListItem != null)
                        {
                            //If existingEntryID is exist then get timesheet item and update hours
                            spListItem.HoursTaken = Convert.ToInt32(work.GetHoursOfDay(weekStartDate.AddDays(i).DayOfWeek));
                            ObjResourceTimeSheetManager.Save(spListItem);
                            //keeps updated workitem id
                            if (wItem != null)
                                changedWorkItemsID.Add(Convert.ToInt32(wItem[DatabaseObjects.Columns.ID]));
                        }
                        else
                        {
                            //If existingEntryID is not exist then ceate new timesheet entry and update hours
                            //Gets selected date hours and checks it is greater the 0 or not.
                            //If it is greater then 0 then only create timesheet entry and save hours
                            double hour = work.GetHoursOfDay(weekStartDate.AddDays(i).DayOfWeek);
                            if (hour > 0)
                            {
                                int wholeHours = (int)((hour * 60) / 60);
                                int remMin = (int)((hour * 60) % 60);
                                if (remMin > 45)
                                {
                                    remMin = 60;
                                }
                                else if (remMin > 30)
                                {
                                    remMin = 45;
                                }
                                else if (remMin > 15)
                                {
                                    remMin = 30;
                                }
                                else if (remMin > 0)
                                {
                                    remMin = 15;
                                }

                                double actualHours = Math.Round((double)(((double)(wholeHours * 60) + remMin) / 60), 2);
                                spListItem = new ResourceTimeSheet();
                                ResourceWorkItems rWorkItem = ObjResourceWorkItemsManager.LoadResourceWorkItem(wItem);
                                spListItem.Title = string.Format("{0};#{1};#{2}", rWorkItem.WorkItemType, rWorkItem.WorkItem, rWorkItem.SubWorkItem);
                                spListItem.ResourceWorkItemLookup = work.WorkItemID;
                                spListItem.WorkDate = weekStartDate.Date.AddDays(i);
                                spListItem.HoursTaken = Convert.ToInt32(actualHours);
                                spListItem.Resource = selectedUserID;
                                ObjResourceTimeSheetManager.Save(spListItem);
                                //keeps updated workitem id
                                changedWorkItemsID.Add(Convert.ToInt32(wItem[DatabaseObjects.Columns.ID]));
                            }
                        }
                    }
                }
            }

            return changedWorkItemsID;
        }

        public List<long> CreateTaskHoursEntries(ApplicationContext context, string selectedUserID, DateTime weekStartDate, List<WorkItemHours> weekWorkSheet)
        {
            bool enableProjStdTasks = ObjConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems);

            List<long> changedWorkItemsID = new List<long>();
            List<WorkItemHours> resourceLevelWorkItems = new List<WorkItemHours>();
            Dictionary<string, object> values = new Dictionary<string, object>();

            //group project tasks to loop in one iteration
            var lookup = weekWorkSheet.ToLookup(x => x.WorkItem);
            foreach (var wHours in lookup)
            {
                List<WorkItemHours> taskSheetHours = wHours.ToList();
                string moduleName = taskSheetHours.First().WorkItemType;
                if (moduleName == "Current Projects (PMM)" || moduleName == "Project Management Module (PMM)")
                    moduleName = "PMM";
                if (moduleName.Contains("(CPR)"))
                    moduleName = ModuleNames.CPR;

                if (moduleName.Contains("(CNS)"))
                    moduleName = ModuleNames.CNS;

                if (moduleName.Contains("(OPM)"))
                    moduleName = ModuleNames.OPM;

                string workItem = taskSheetHours.First().WorkItem;

                //keep enableprojstd tasks true if module name is pmm otherwise keep it false                
                enableProjStdTasks = IsStdWorkItemEnable(context, moduleName, weekStartDate);

                List<UGITTask> tasks = new List<UGITTask>();
                //required to fetch project tasks when 
                if (!enableProjStdTasks)
                    tasks = taskManager.LoadByProjectID(workItem);

                DataTable ticketHours = null;
                DataRow ticketHourRow = null;


                //TicketHoursManager ticketHourHelper = new TicketHoursManager(context);
                //ticketHours = ticketHourHelper.GetWorkHours(moduleName, workItem, selectedUserID, startDate: weekStartDate, endDate: weekStartDate.AddDays(6));
                ticketHours = GetWorkHours(moduleName, workItem, selectedUserID, startDate: weekStartDate, endDate: weekStartDate.AddDays(6));

                //DataTable ticketHours = ticketHourHelper.GetWorkHours(moduleName, workItem, selectedUserID, startDate: weekStartDate, endDate: weekStartDate.AddDays(6));
                foreach (WorkItemHours sheetItem in taskSheetHours)
                {
                    //string SubWorkItem = sheetItem.SubWorkItem; 
                    string SubSubWorkItem = sheetItem.SubSubWorkItem; //sheetItem.SubWorkItem;
                    if (string.IsNullOrEmpty(SubSubWorkItem))
                        SubSubWorkItem = "0";
                    //continue;

                    string dataseprator = ";#";
                    string[] entries = UGITUtility.SplitString(SubSubWorkItem, dataseprator);
                    string subWorkItemId = SubSubWorkItem != "0" ? entries[0] : "0";
                    long taskId = Convert.ToInt32(subWorkItemId);
                    string subWorkItemTitle = SubSubWorkItem != "0" ? entries[1] : sheetItem.SubWorkItem;

                    int existingEntryID = 0;
                    for (int i = 0; i < 7; i++)
                    {
                        //exclude -ve hours from iteration if any
                        if (sheetItem.GetHoursOfDay(weekStartDate.AddDays(i).DayOfWeek) < 0)
                            continue;

                        //Gets existing ticket hours id for selected  user, selected workItem, selected date
                        existingEntryID = 0;
                        if (ticketHours != null)
                        {
                            // have uncomment after updating table of ticket helper
                            ticketHourRow = ticketHours.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TaskID) == UGITUtility.ObjectToString(taskId) && x.Field<DateTime>(DatabaseObjects.Columns.WorkDate).Date == weekStartDate.Date.AddDays(i) && UGITUtility.StringToBoolean(x.Field<string>(DatabaseObjects.Columns.StandardWorkItem)) == enableProjStdTasks && x.Field<string>(DatabaseObjects.Columns.WorkItem) == sheetItem.WorkItem && x.Field<string>(DatabaseObjects.Columns.SubWorkItem) == sheetItem.SubWorkItem);

                            if (ticketHourRow != null)
                                int.TryParse(Convert.ToString(ticketHourRow[DatabaseObjects.Columns.Id]), out existingEntryID);
                        }


                        ActualHour uhItem = null;
                        if (existingEntryID > 0)
                            uhItem = LoadByID(existingEntryID);

                        double hours = GetTimesheetActulHours(sheetItem.GetHoursOfDay(weekStartDate.AddDays(i).DayOfWeek));

                        if (hours <= 0)
                            continue;

                        //if (uhItem == null)
                        //    uhItem = ticketHourHelper.AddNew();
                        DateTime workDate = weekStartDate.AddDays(i).Date;
                        if (uhItem == null)
                        {
                            //values.Clear();
                            uhItem = new ActualHour();
                            uhItem.TicketID = workItem;
                            uhItem.WorkItem = workItem;
                            uhItem.SubWorkItem = sheetItem.SubWorkItem;
                            uhItem.StageStep = 0;
                            uhItem.ModuleNameLookup = moduleName;
                            uhItem.TaskID = taskId;
                            //uhItem.t = subWorkItemTitle;
                            uhItem.HoursTaken = hours;
                            //uhItem.Comment = string.Empty;
                            uhItem.Resource = selectedUserID;
                            uhItem.WorkDate = workDate;
                            uhItem.StandardWorkItem = enableProjStdTasks;
                            uhItem.TenantID = context.TenantID;

                            //Update week & month start date from WorkDate 
                            if (workDate != DateTime.MinValue && workDate != DateTime.MaxValue)
                            {
                                DateTime weekSDate = uHelper.GetWeekStartDate(workDate);//Week start day will be monday
                                uhItem.WeekStartDate = weekSDate.Date;
                                uhItem.MonthStartDate = uHelper.GetMonthStartDate(workDate).Date;
                            }
                            Insert(uhItem);
                            //GetTableDataManager.AddItem<int>(DatabaseObjects.Tables.TicketHours, values);
                        }

                        else
                        {
                            values.Clear();
                            uhItem.TicketID = workItem;
                            uhItem.WorkItem = workItem;
                            uhItem.SubWorkItem = sheetItem.SubWorkItem;
                            uhItem.StageStep = 0;
                            uhItem.HoursTaken = hours;
                            //uhItem.Comment = string.Empty; // Commented, as existing comments are disappearing, when Timesheet values are edited & Saved.
                            uhItem.Resource = selectedUserID;
                            uhItem.WorkDate = workDate;
                            uhItem.StandardWorkItem = enableProjStdTasks;
                            uhItem.TenantID = context.TenantID;

                            //Update week & month start date from WorkDate 
                            if (workDate != DateTime.MinValue && workDate != DateTime.MaxValue)
                            {
                                DateTime weekSDate = uHelper.GetWeekStartDate(workDate);//Week start day will be monday
                                uhItem.WeekStartDate = weekSDate.Date;
                                uhItem.MonthStartDate = uHelper.GetMonthStartDate(workDate).Date;
                            }
                            Update(uhItem);
                            //GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.TicketHours, UGITUtility.StringToLong(uhItem.ID), values);

                        }

                    }
                    // have to uncomment
                    if (!enableProjStdTasks && tasks.Count > 0)
                    {
                        UGITTask task = tasks.FirstOrDefault(x => x.ID == taskId);
                        //ticketHourHelper.UpdateTaskActualHours(moduleName, workItem, task);
                        UpdateTaskActualHours(moduleName, workItem, task);
                    }
                }


                //find project level workitem to update rollup timesheet data
                WorkItemHours weekEntryItem = GetTotalHoursForWeek(context, selectedUserID, moduleName, workItem, weekStartDate);
                resourceLevelWorkItems.Add(weekEntryItem);
            }

            //Update project level timesheet entry by rolling up task level hours
            resourceLevelWorkItems = resourceLevelWorkItems.Where(x => x.WorkItemID > 0).ToList();
            changedWorkItemsID = CreateResourceTimesheetsEntries(context, selectedUserID, weekStartDate, resourceLevelWorkItems);
            return changedWorkItemsID;

        }

        private WorkItemHours GetTotalHoursForWeek(ApplicationContext context, string selectedUserID, string moduleName, string workItem, DateTime weekStartDate)
        {
            string PMMModule = string.Empty;

            if (moduleName == "PMM" || moduleName == "Current Projects (PMM)")
                PMMModule = "PMM";
            else
                PMMModule = moduleName;

            ResourceWorkItems rWorkItem = ObjResourceWorkItemsManager.LoadByWorkItem(selectedUserID, PMMModule, workItem, "", "", Convert.ToString(weekStartDate), Convert.ToString(weekStartDate.AddDays(6)));

            WorkItemHours resourceLevelWorkItem = new WorkItemHours();

            if (rWorkItem != null)
                resourceLevelWorkItem.WorkItemID = (int)rWorkItem.ID;

            resourceLevelWorkItem.Mon = resourceLevelWorkItem.Tue = resourceLevelWorkItem.Wed =
                       resourceLevelWorkItem.Thu = resourceLevelWorkItem.Fri = resourceLevelWorkItem.Sat = resourceLevelWorkItem.Sun = 0;
            //here module name required as PMM
            if (moduleName == "PMM" || moduleName == "Current Projects (PMM)")
                moduleName = "PMM";
            DataTable ticketHours = GetWorkHours(moduleName, workItem, selectedUserID, startDate: weekStartDate, endDate: weekStartDate.AddDays(6));
            bool enableStdWorkItem = IsStdWorkItemEnable(context, moduleName, weekStartDate);
            for (int i = 0; i < 7; i++)
            {
                DateTime wDate = weekStartDate.AddDays(i);
                double hours = 0;
                if (ticketHours != null && ticketHours.Rows.Count > 0)
                {
                    if (enableStdWorkItem)
                        hours = ticketHours.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.WorkDate).Date == wDate.Date && UGITUtility.StringToBoolean(x.Field<string>(DatabaseObjects.Columns.StandardWorkItem))).Sum(x => Convert.ToInt32(x.Field<string>(DatabaseObjects.Columns.HoursTaken)));
                    else
                        hours = ticketHours.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.WorkDate).Date == wDate.Date && UGITUtility.StringToBoolean(x.Field<string>(DatabaseObjects.Columns.StandardWorkItem)) != false).Sum(x => Convert.ToInt32(x.Field<double>(DatabaseObjects.Columns.HoursTaken)));

                }
                resourceLevelWorkItem.SetHoursOfDay(wDate.DayOfWeek, hours);
            }

            return resourceLevelWorkItem;
        }


        // Delete week entries for a subtask of a workitem
        public void DeleteWeekEntries(ApplicationContext context, string moduleName, string workItem, string userID, int taskID, DateTime weekStartDate)
        {
            DeleteWeekEntries(context, moduleName, workItem, userID, new List<int>() { (taskID) }, weekStartDate);
        }

        // Delete week entries for all the subtasks of a workitem
        public void DeleteWeekEntries(ApplicationContext context, string moduleName, string workItem, string userID, List<int> taskIDs, DateTime weekStartDate)
        {
            //bool unsafeUpdate = _spWeb.AllowUnsafeUpdates;
            //_spWeb.AllowUnsafeUpdates = true;
            string PMMmoduleName = string.Empty;
            TicketHoursManager tHelper = new TicketHoursManager(context);
            if (moduleName == "Current Projects (PMM)" || moduleName == "PMM")
                PMMmoduleName = "PMM";
            else
                PMMmoduleName = moduleName;

            if (moduleName == "Opportunity Management (OPM)")
                PMMmoduleName = ModuleNames.OPM;

            if (moduleName == "Project Management (CPR)")
                PMMmoduleName = ModuleNames.CPR;

            if (moduleName == "Service Projects (CNS)")
                PMMmoduleName = ModuleNames.CNS;

            bool enableProjStdTasks = IsStdWorkItemEnable(context, PMMmoduleName, weekStartDate);
            foreach (int taskID in taskIDs)
            {
                if (taskID == 0)
                    continue;
                int i = 0;
                List<ActualHour> collection = GetWorkHoursCollection(PMMmoduleName, workItem, userID, taskID, weekStartDate, weekStartDate.AddDays(6));
                for (i = 0; i < collection.Count(); i++)
                {
                    ActualHour item = collection[i];
                    int value = Convert.ToInt32(item.ID);
                    item.HoursTaken = 0;
                    Delete(item);
                }

                if (!enableProjStdTasks)
                {
                    List<UGITTask> taskList = taskManager.LoadByProjectID(workItem);
                    UGITTask task = taskList.FirstOrDefault(x => x.ID == taskID);
                    if (task != null)
                        UpdateTaskActualHours(moduleName, workItem, task);
                }
            }

            //Rollup hours into timesheet
            WorkItemHours weekEntry = tHelper.GetTotalHoursForWeek(context, userID, moduleName, workItem, weekStartDate);
            List<long> changedWorkItemsID = tHelper.CreateResourceTimesheetsEntries(context, userID, weekStartDate, new List<WorkItemHours>() { weekEntry });

            if (changedWorkItemsID.Count > 0)
            {
                //Start Thread to update rmm summary list for current sheet entries
                ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateActualInRMMSummary(context, changedWorkItemsID, userID, weekStartDate); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }
        }



        public void UpdateTaskActualHours(string moduleName, string workItem, UGITTask task)
        {
            UGITModule module = moduleManager.LoadByName(moduleName);

            if (module != null && module.ActualHoursByUser)
            {
                DataTable data = GetWorkHours(moduleName, workItem, " ", Convert.ToInt32(task.ID));
                double hours = 0;
                if (data != null && data.Rows.Count > 0)
                {
                    hours = (double)data.Compute(string.Format("sum({0})", DatabaseObjects.Columns.HoursTaken), string.Empty);
                }

                task.ActualHours = hours;
                task.EstimatedRemainingHours = task.EstimatedHours - task.ActualHours;
                if (task.EstimatedRemainingHours <= 0)
                    task.EstimatedRemainingHours = 0;

                taskManager.SaveTask(ref task);
            }
        }

        public DataTable GetActualHoursList(string query, string columns)
        {
            DataTable dt = uGITDAL.GetTable(DatabaseObjects.Tables.TicketHours, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}' and {DatabaseObjects.Columns.TicketId} in ({query})", columns);
            return dt;
        }
    }
    public interface ITicketHoursManager : IManagerBase<ActualHour>
    {
        ActualHour AddUpdate(ActualHour relation);
        List<ActualHour> TicketHourList();
        List<ActualHour> TicketHourList(string TicketID);
        DataTable TicketHours();
        DataTable TicketHours(string TicketID);
    }
}

