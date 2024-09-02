using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;
using System.Web;
using uGovernIT.Util.Log;
using System.Threading.Tasks;
using uGovernIT.Util.Cache;
using uGovernIT.DAL;
using DevExpress.ClipboardSource.SpreadsheetML;
//using System.Threading.Tasks;
//using System.Windows.Forms;

namespace uGovernIT
{
    public class DashboardCacheData
    {
        public DashboardCacheData(Guid guid)
        {
            //listTaskCache = new List<TaskData>();
            this.collectionId = guid;
        }
        public Guid collectionId { get; set; }
        public Dictionary<string, DataTable> factTables;
        public DataTable factTableSettings;
        public List<string> DateViews;
        public string TenantID;
    }

    public class DashboardCache
    {
        private DashboardCache(Guid guid)
        {
            collectionId = guid;
        }

        public static Guid collectionId { get; set; }

        private static List<DashboardCacheData> listDashboardCache { get; set; }

        public static DashboardCacheData GetDashboardCacheInstance()
        {
            //if (SPContext.Current != null)
            //{
            //    return GetDashboardCacheInstance(SPContext.Current.Site.ID);
            //}
            //else
            //{
            return GetDashboardCacheInstance(collectionId);
            // }
        }

        public static DashboardCacheData GetDashboardCacheInstance(Guid id)
        {
            if (listDashboardCache == null)
                listDashboardCache = new List<DashboardCacheData>();

            collectionId = id;
            if (listDashboardCache != null && listDashboardCache.Exists(x => x != null && x.collectionId == id))
            {
                return listDashboardCache.Find(x => x.collectionId == id);
            }
            else
            {
                var dashboardCache = new DashboardCacheData(id);
                listDashboardCache.Add(dashboardCache);

                return dashboardCache;
            }
        }

        //private static Dictionary<string, DataTable> factTables;
        //private static DataTable factTableSettings;
        private static object lockObj = new object();
        private static object lObj = new object();

        public static void ClearCache()
        {
            GetDashboardCacheInstance().factTables = null;
            GetDashboardCacheInstance().factTableSettings = null;
        }

        /// <summary>
        /// Reload dashboard facttable
        /// </summary>
        /// <param name="applicationContext"></param>
        public static void ReloadFactTableSettings(ApplicationContext applicationContext)
        {
            GetDashboardCacheInstance().factTableSettings = null;
            DataTable dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardFactTables, $"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");
            lock (lockObj)
            {
                if (GetDashboardCacheInstance().factTableSettings == null)
                {
                    GetDashboardCacheInstance().factTableSettings = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardFactTables, $"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");
                }
            }
        }

        /// <summary>
        /// Refresh dashboard facttable if empty
        /// </summary>
        //public static DataTable GetfactTableSettings()
        //{
        //    return GetfactTableSettings();
        //}
        public static DataTable GetfactTableSettings(ApplicationContext context)
        {
            if (GetDashboardCacheInstance().factTableSettings != null)
                return GetDashboardCacheInstance().factTableSettings;
            GetDashboardCacheInstance().factTableSettings = null;
            lock (lockObj)
            {
                if (GetDashboardCacheInstance().factTableSettings == null)
                    ReloadFactTableSettings(context);
            }

            return GetDashboardCacheInstance().factTableSettings;
        }

        public static void AddFactTableIfNeeded(string factTableName)
        {
            if (GetDashboardCacheInstance().factTables == null)
            {
                lock (lockObj)
                {
                    if (GetDashboardCacheInstance().factTables == null)
                        GetDashboardCacheInstance().factTables = new Dictionary<string, DataTable>();
                }
            }

            if (GetDashboardCacheInstance().factTables.ContainsKey(factTableName)) return;

            lock (lockObj)
            {
                if (!GetDashboardCacheInstance().factTables.ContainsKey(factTableName))
                    GetDashboardCacheInstance().factTables.Add(factTableName, null);
            }
        }

        /// <summary>
        /// Get dashboard fact tables which are predefinied.
        /// </summary>
        /// <returns></returns>
        public static List<string> DashboardFactTables(ApplicationContext context = null)
        {
            if (context == null)
                context = HttpContext.Current.GetManagerContext();

            List<string> factTables = new List<string>();
            DashboardFactTableManager DashboardFactTableManager = new DashboardFactTableManager(context);
            var factTableSettings = DashboardFactTableManager.Load().Select(x => new { x.Title }).OrderBy(x => x.Title).ToList();

            if (factTableSettings != null)
            {
                foreach (var item in factTableSettings)
                {
                    factTables.Add(item.Title);
                }
            }

            factTables.Add("AllTickets");
            factTables.Add("AllSMSTickets");
            factTables.Add("AllOpenTickets");
            factTables.Add("AllOpenSMSTickets");
            factTables.Add("AllClosedTickets");
            factTables.Add("AllClosedSMSTickets");

            // Get all cached ticket tables!
            ModuleViewManager ModuleViewManager = new ModuleViewManager(context);
            //var modules = ModuleViewManager.Load().Where(x => x.EnableModule == true && x.EnableCache == true).Select(x => new { x.ModuleName }).OrderBy(x => x.ModuleName).ToList();

            // Removed EnableModule condition, as Crash occurs when Dashboard & Report Queries, of disabled modules are opened. eg. PMM related query opened in CRM, where PMM module is disabled.
            var modules = ModuleViewManager.Load().Where(x => x.EnableCache == true && x.EnableModule == true).Select(x => new { x.ModuleName }).OrderBy(x => x.ModuleName).ToList();

            if (modules != null)
            {
                foreach (var module in modules)
                {
                    factTables.Add(string.Format("{0}-AllTickets", module.ModuleName));
                    factTables.Add(string.Format("{0}-OpenTickets", module.ModuleName));
                    factTables.Add(string.Format("{0}-ClosedTickets", module.ModuleName));
                }
            }

            factTables.Add("Task-Open");
            factTables.Add("Task-Closed");
            factTables.Add("Task-All");
            factTables.Add("Assets");
            factTables.Add("SprintSummary");
            factTables = factTables.OrderBy(x => x).ToList();

            return factTables;
        }

        ///// <summary>
        ///// Get Facttable fields information
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <returns></returns>
        public static List<FactTableField> GetFactTableFields(ApplicationContext context, string tableName)
        {
            List<FactTableField> factTableInfo = new List<FactTableField>();
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            TicketManager ticketManager = new TicketManager(context);
            //Get all defined fact tables
            string factTable = DashboardFactTables(context).FirstOrDefault(x => x == tableName);
            if (factTable != null)
            {
                //Get Dashboard summary fields info
                if (factTable == "DashboardSummary")
                {
                    DataTable table = uGITDAL.GetTableStructure(DatabaseObjects.Tables.DashboardSummary);
                    factTableInfo = CreateFactTableFieldsData(context, table, tableName);
                }
                else if (factTable == "ResourceUsageSummaryWeekWise")
                {
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ID, "System.Integer"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.Title, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.WorkItemType, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.WorkItem, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.SubWorkItem, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.Resource, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ManagerLookup, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.FunctionalAreaTitleLookup, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.IsManager, "System.Boolean"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.IsIT, "System.Boolean"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.IsConsultant, "System.Boolean"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.WeekStartDate, "System.DateTime"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.PctAllocation, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.AllocationHour, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.PctActual, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ActualHour, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.WorkItemID, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.TenantID, "System.String"));
                }
                else if (factTable == "ResourceUsageSummaryMonthWise")
                {
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ID, "System.Integer"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.Title, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.WorkItemType, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.WorkItem, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.SubWorkItem, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.Resource, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ManagerLookup, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.FunctionalAreaTitleLookup, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.IsManager, "System.Boolean"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.IsIT, "System.Boolean"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.IsConsultant, "System.Boolean"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.MonthStartDate, "System.DateTime"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.PctAllocation, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.AllocationHour, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.PctActual, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ActualHour, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.WorkItemID, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.TenantID, "System.String"));
                }
                else if (factTable == "SprintSummary")
                {
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ID, "System.Integer"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.Title, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.TicketPMMIdLookup, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.UGITDescription, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.ItemOrder, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.UGITStartDate, "System.DateTime"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.UGITEndDate, "System.DateTime"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.RemainingHours, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.TaskEstimatedHours1, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.SprintLookup, "System.String"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.PercentComplete, "System.Double"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.Created, "System.DateTime"));
                    factTableInfo.Add(new FactTableField(tableName, DatabaseObjects.Columns.TenantID, "System.String"));
                }
                //Get open or closed ticket fields info only for sms modules
                else if (factTable.ToLower() == "allopensmstickets" || factTable.ToLower() == "allclosedsmstickets" || factTable.ToLower() == "allsmstickets")
                {
                    List<UGITModule> modules = moduleViewManager.Load(x => x.ModuleType == ModuleType.SMS && x.EnableModule);
                    factTableInfo = GetFactTableFieldsForAllModules(modules, context, factTable, tableName);
                }
                else if (factTable.ToLower() == "allopentickets" || factTable.ToLower() == "allclosedtickets" || factTable.ToLower() == "alltickets")
                {
                    List<UGITModule> modules = moduleViewManager.LoadAllModule().Where(x => x.EnableModule && x.EnableCache).ToList();
                    factTableInfo = GetFactTableFieldsForAllModules(modules, context, factTable, tableName);
                   
                }
                else
                {
                    DataRow dashboardSetting = null;
                    DataTable factTableSettings = GetfactTableSettings(context);
                    if (factTableSettings != null)
                    {
                        DataRow[] dashboardRows = factTableSettings.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, factTable));
                        if (dashboardRows.Length > 0)
                        {
                            dashboardSetting = dashboardRows[0];
                        }
                    }
                    if (dashboardSetting != null)
                    {
                        DataTable table = uGITDAL.GetTableStructure(factTable);
                        if (table!=null)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                factTableInfo.Add(new FactTableField(tableName, column.ColumnName, column.DataType.ToString()));
                            }
                        }
                        
                    }
                    else
                    {
                        string moduleName = tableName.Split('-')[0];
                        if (moduleName == null || moduleName.Trim() == string.Empty)
                            return factTableInfo;

                        if (moduleName == "Task")
                        {
                            DataTable table = uGITDAL.GetTableStructure(DatabaseObjects.Tables.ModuleTasks);
                            foreach (DataColumn column in table.Columns)
                            {
                                factTableInfo.Add(new FactTableField(tableName, column.ColumnName, column.DataType.ToString()));
                            }
                        }
                        else
                        {
                            DataTable listFields = null;
                            UGITModule module = null;
                            if (factTable == "Assets")
                                module = moduleViewManager.LoadByName(ModuleNames.CMDB);
                            else
                                module = moduleViewManager.LoadByName(moduleName);

                            if (module != null)
                            {
                                List<ModuleColumn> lstModuleColumn = module.List_ModuleColumns;
                                listFields = GetFactTable(context, factTable);
                                if (listFields == null || listFields.Rows.Count < 1)
                                {
                                    listFields = uGITDAL.GetTableStructure(module.ModuleTable);
                                }
                                if (lstModuleColumn != null && lstModuleColumn.Count > 0)
                                {
                                    foreach (DataColumn column in listFields.Columns)
                                    {
                                        if (lstModuleColumn.Exists(x => x != null && x.FieldName == column.ColumnName))
                                        {
                                            ModuleColumn moduleColumn = lstModuleColumn.Where(x => x.FieldName == column.ColumnName).FirstOrDefault();
                                            factTableInfo.Add(new FactTableField(tableName, column.ColumnName, QueryHelperManager.GetStandardDataType(Convert.ToString(column.DataType)), moduleColumn.FieldDisplayName));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return factTableInfo;
        }
        public static List<FactTableField> GetFactTableFieldsForAllModules(List<UGITModule> modules, ApplicationContext context, string factTable, string tableName)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            List<FactTableField> factTableFields = new List<FactTableField>();
            foreach (var item in modules)
            {
                UGITModule module = moduleViewManager.LoadByName(item.ModuleName);
                List<ModuleColumn> lstModuleColumn = module.List_ModuleColumns;
                DataTable listFields = GetFactTable(context, factTable);
                if (listFields == null || listFields.Rows.Count < 1)
                {
                    listFields = uGITDAL.GetTableStructure(module.ModuleTable);
                }
                if (lstModuleColumn != null && lstModuleColumn.Count > 0)
                {
                    foreach (DataColumn column in listFields.Columns)
                    {
                        if (lstModuleColumn.Exists(x => x != null && x.FieldName == column.ColumnName))
                        {
                            ModuleColumn moduleColumn = lstModuleColumn.Where(x => x.FieldName == column.ColumnName).FirstOrDefault();
                            factTableFields.Add(new FactTableField(tableName, column.ColumnName, QueryHelperManager.GetStandardDataType(Convert.ToString(column.DataType)), moduleColumn.FieldDisplayName));
                        }
                    }
                }
            }
            factTableFields = factTableFields.GroupBy(f => f.FieldName).Select(g => g.First()).ToList();
            factTableFields.Add(new FactTableField(tableName, DatabaseObjects.Columns.ModuleName, "System.String", DatabaseObjects.Columns.ModuleName));
            return factTableFields;
        }
        //public static DataTable GetCachedDashboardData(string factTableName)
        //{
        //    return GetCachedDashboardData(factTableName, SPContext.Current.Web);
        //}
        ///// <summary>
        ///// Get dashboard data from cache
        ///// </summary>
        ///// <param name="factTableName"></param>
        ///// <returns></returns>
        public static DataTable GetCachedDashboardData(ApplicationContext context, string factTableName)
        {
            //lock (lObj)
            //{
            DataRow setting = null;
            var isCacheTable = false;
            uint cacheAfter = 0;
            uint thresholdLimit = 0;
            var cacheMode = string.Empty;
            DateTime expiryDate = DateTime.MinValue;
            var gotNewData = false;
            DataTable dashboardData = null;
            string refreshMode = null;
            // Refresh dashboard setting empty
            //DataTable factTableSettings = GetfactTableSettings(context);
            DataTable factTableSettings = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardFactTables, $"TenantID='{context.TenantID}'");
            AddFactTableIfNeeded(factTableName);

            // Get all the settings from dashboard fact table
            if (factTableSettings != null && factTableSettings.Rows.Count > 0)
            {
                DataRow[] settings = factTableSettings.Select($"{DatabaseObjects.Columns.Title}='{factTableName}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (settings.Length > 0)
                {
                    setting = settings[0];
                    isCacheTable = UGITUtility.StringToBoolean(setting[DatabaseObjects.Columns.CacheTable]);
                    uint.TryParse(Convert.ToString(setting[DatabaseObjects.Columns.CacheAfter]), out cacheAfter);
                    uint.TryParse(Convert.ToString(setting[DatabaseObjects.Columns.CacheThreshold]), out thresholdLimit);
                    DateTime.TryParse(Convert.ToString(setting[DatabaseObjects.Columns.ExpiryDate]), out expiryDate);
                    cacheMode = Convert.ToString(setting[DatabaseObjects.Columns.CacheMode]);
                    refreshMode = Convert.ToString(setting[DatabaseObjects.Columns.RefreshMode]);
                }
            }

            // Checks if cache table enabled or not. if its enable then and data present in cache then pick data from cache
            if (isCacheTable && GetDashboardCacheInstance().factTables[factTableName] != null && GetDashboardCacheInstance().TenantID == context.TenantID)
            {
                dashboardData = GetDashboardCacheInstance().factTables[factTableName];
            }

            // if data in not exist then load data from database using GetFactTable method.
            if (dashboardData == null || dashboardData.Rows.Count == 0)
            {
                //lock (lockObj)
                //{
                dashboardData = GetFactTable(context, factTableName, refreshMode);
                gotNewData = true;
                //}
            }
            if (dashboardData != null)
                return dashboardData;
            DataTable dashboardFactTableList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardFactTables, $"tenantid='{context.TenantID}'");

            // If item in dashboard table exceed cache threshold then store data in cache otherwise make it null
            if (isCacheTable && dashboardFactTableList.Rows.Count > thresholdLimit && gotNewData)
                GetDashboardCacheInstance().factTables[factTableName] = dashboardData;

            // Check condition which enable to trigger event to recache data
            if (isCacheTable && cacheMode == "Scheduled" && cacheAfter > 0 && setting != null && dashboardFactTableList.Rows.Count > thresholdLimit && expiryDate <= DateTime.Now)
            {
                // Checks status of item in InProgress or not.
                //  Status of InProgress means cache is already being refreshed.
                //  If Not, then trigger the event to recache data board data by setting status to InProgress
                //  The status is checked in event receiver for DashboardFactTable, which refreshes fact table if status is set to InProgress
                DataRow item = dashboardFactTableList.Select($"{DatabaseObjects.Columns.ID}={Convert.ToInt32(setting[DatabaseObjects.Columns.Id])}")[0];

                if (item != null && UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.Status, false) != Constants.InProgress)
                {
                    //lock (lockObj)
                    //{
                    item = dashboardFactTableList.Select($"{DatabaseObjects.Columns.ID}={Convert.ToInt32(setting[DatabaseObjects.Columns.Id])}")[0];

                    if (UGITUtility.GetSPItemValueAsString(item, DatabaseObjects.Columns.Status, false) != Constants.InProgress)
                    {
                        item[DatabaseObjects.Columns.Status] = Constants.InProgress;

                        // Update in DB
                        Dictionary<string, object> values = new Dictionary<string, object>();
                        values.Add(DatabaseObjects.Columns.Status, Constants.InProgress);
                        GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.DashboardFactTables, Convert.ToInt32(item[DatabaseObjects.Columns.ID]), values);
                        //Task.Run(async () =>
                        //{
                        //    await Task.FromResult(0);
                        if (isCacheTable && cacheMode == "Scheduled" && cacheAfter > 0 && UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.Status).ToString() == Constants.InProgress)
                        {
                            RefreshDashboardCache(setting[DatabaseObjects.Columns.Title].ToString(), context);
                        }
                        //});
                    }
                    //}
                }

            }

            // Update lookup & user type field values
            if (dashboardData != null)
                return dashboardData;
            //return UpdateLookupAndUserFieldValues(context, dashboardData);
            else
                return null;
            //}
        }

        ///// <summary>
        ///// Set Dashboard data in cache
        ///// </summary>
        ///// <param name="factTableName"></param>
        ///// <param name="dashboardTable"></param>
        //public static void SetDashboardData(string factTableName, DataTable dashboardTable)
        //{
        //    if (GetDashboardCacheInstance().factTables == null || !GetDashboardCacheInstance().factTables.ContainsKey(factTableName))
        //    {
        //        return;
        //    }

        //    GetDashboardCacheInstance().factTables[factTableName] = dashboardTable;
        //}

        public static List<string> GetDateViewList()
        {
            if (GetDashboardCacheInstance().DateViews != null && GetDashboardCacheInstance().DateViews.Count > 0)
            {
                return GetDashboardCacheInstance().DateViews;
            }

            GetDashboardCacheInstance().DateViews = new List<string>();
            GetDashboardCacheInstance().DateViews.Add("Today");
            GetDashboardCacheInstance().DateViews.Add("Current Week");
            GetDashboardCacheInstance().DateViews.Add("Current Month");
            GetDashboardCacheInstance().DateViews.Add("Current Quarter");
            GetDashboardCacheInstance().DateViews.Add("Current Year");
            GetDashboardCacheInstance().DateViews.Add("Yesterday");
            GetDashboardCacheInstance().DateViews.Add("Last 24 Hours");
            GetDashboardCacheInstance().DateViews.Add("Last 7 Days");
            GetDashboardCacheInstance().DateViews.Add("Last 30 Days");
            GetDashboardCacheInstance().DateViews.Add("Last 3 Months");
            GetDashboardCacheInstance().DateViews.Add("Last 6 Months");
            GetDashboardCacheInstance().DateViews.Add("Last 12 Months");
            GetDashboardCacheInstance().DateViews.Add("Last Month");
            GetDashboardCacheInstance().DateViews.Add("Last Quarter");
            GetDashboardCacheInstance().DateViews.Add("Last Year");

            return GetDashboardCacheInstance().DateViews;
        }


        ///// <summary>
        ///// Refresh all dashboard cache
        ///// </summary>
        ///// <param name="spWeb"></param>
        public static void RefreshAllDashboardCache(ApplicationContext context, int Id, int CacheAfter)
        {
            DataTable dashboardData = null;
            if (dashboardData == null)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add(DatabaseObjects.Columns.Status, Constants.Completed);
                values.Add("Lastupdated", DateTime.Now.ToString());
                values.Add("ExpiryDate", DateTime.Now.AddMinutes(CacheAfter));
                GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.DashboardFactTables, Id, values);
            }

        }

        ///// <summary>
        ///// Refresh cache of specified facttable
        ///// </summary>
        ///// <param name="dashboardTable"></param>
        ///// <param name="spWeb"></param>
        public static void RefreshDashboardCache(string dashboardTable, ApplicationContext context)
        {
            DataRow[] itemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardFactTables, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select($"{DatabaseObjects.Columns.Title}='{dashboardTable}'");
            if (itemColl == null || itemColl.Count() == 0)
                return;
            DataRow dashboardItem = itemColl[0];
            RefreshDashboardCache(dashboardTable, dashboardItem, context);
        }

        ///// <summary>
        ///// recache the dashboard table
        ///// </summary>
        ///// <param name="dashboardTable"></param>
        ///// <param name="spWeb"></param>
        ///// <param name="refreshMode"></param>
        public static void RefreshDashboardCache(string dashboardTable, DataRow dashboardItem, ApplicationContext context)
        {
            if (GetDashboardCacheInstance().factTables == null)
            {
                lock (lockObj)
                {
                    if (GetDashboardCacheInstance().factTables == null)
                        GetDashboardCacheInstance().factTables = new Dictionary<string, DataTable>();
                }
            }

            if (!GetDashboardCacheInstance().factTables.ContainsKey(dashboardTable))
            {
                lock (lockObj)
                {
                    if (!GetDashboardCacheInstance().factTables.ContainsKey(dashboardTable))
                        GetDashboardCacheInstance().factTables.Add(dashboardTable, null);
                }
            }

            //Load facttable from database
            lock (lockObj)
            {
                uint cacheAfter = 0;
                string refreshMode = UGITUtility.GetSPItemValue(dashboardItem, DatabaseObjects.Columns.RefreshMode).ToString();
                uint.TryParse(UGITUtility.GetSPItemValue(dashboardItem, DatabaseObjects.Columns.CacheAfter).ToString(), out cacheAfter);
                GetDashboardCacheInstance().factTables[dashboardTable] = GetFactTable(context, dashboardTable, refreshMode);
                GetDashboardCacheInstance().TenantID = context.TenantID;
                dashboardItem[DatabaseObjects.Columns.ExpiryDate] = DateTime.Now.AddMinutes(cacheAfter);
                dashboardItem[DatabaseObjects.Columns.Status] = Constants.Completed;
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add(DatabaseObjects.Columns.Status, Constants.Completed);
                    values.Add("Lastupdated", DateTime.Now.ToString());
                    values.Add("ExpiryDate", DateTime.Now.AddMinutes(cacheAfter));
                    GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.DashboardFactTables, Convert.ToInt32(dashboardItem[DatabaseObjects.Columns.ID]), values);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "ERROR - Update failed for fact table: " + dashboardTable);
                }
                DashboardCache.ReloadFactTableSettings(context);
                ULog.WriteLog("Refreshed fact table: " + dashboardTable);

                //Load and refresh cached of those charts which are created by current facttable
                DashboardCache.RefreshCharts(dashboardTable, context);
            }
        }

        private static void RefreshCharts(string factTable, ApplicationContext context)
        {
            DashboardManager objDashboard = new DashboardManager(context);
            //Load and refresh cached of those charts which are created by current facttable
            //List<UDashboard> dashboards = UDashboard.LoadDashboardPanelsByType(DashboardType.Chart);
            List<Dashboard> chartDashboards = objDashboard.LoadDashboardPanelsByType(DashboardType.Chart, true, context.TenantID);
            chartDashboards = chartDashboards.Where(x => x.panel != null && ((ChartSetting)x.panel).FactTable == factTable).ToList();
            foreach (Dashboard dashboard in chartDashboards)
            {
                try
                {
                    ChartSetting chartSetting = (ChartSetting)dashboard.panel;
                    if (!chartSetting.IsCacheChart)
                    {
                        //Continue if cache setting is off
                        continue;
                    }

                    ChartCachedDataPoints chartCachedDatapoints = ChartCache.GetChartCache(chartSetting.ChartId.ToString());
                    if (chartCachedDatapoints != null)
                    {
                        //Expire cached chart 
                        chartCachedDatapoints.LastUpdated = DateTime.Now.AddMinutes(-(chartSetting.CacheSchedule + 1));
                    }

                    //Creates chart which will refresh the cache automatically.
                    ChartHelper cHelper = new ChartHelper(chartSetting, context);
                    cHelper.UseAjax = true;
                    cHelper.ChartTitle = dashboard.Title;
                    cHelper.CreateChart(true, "115", "100", false);

                    ULog.WriteLog("  Refreshed dashboard: " + dashboard.Title);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "  Error refreshing dashboard " + dashboard.Title);
                }
            }
        }

        //#region Helpers

        ///// <summary>
        ///// Get FactTable from database
        ///// </summary>
        ///// <param name="factTableName"></param>
        ///// <param name="spWeb"></param>
        ///// <returns></returns>
        private static DataTable GetFactTable(ApplicationContext context, string factTableName)
        {
            return GetFactTable(context, factTableName, null);
        }

        /// <summary>
        /// Get filter data board data from database
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="factTableName"></param>
        /// <param name="refreshMode"></param>
        /// <returns></returns>
        private static DataTable GetFactTable(ApplicationContext applicationContext, string factTableName, string refreshMode)
        {
            TicketManager ticketManager = new TicketManager(applicationContext);
            ModuleViewManager moduleViewManager = new ModuleViewManager(applicationContext);
            DataRow dashboardSetting = null;
            Dictionary<string, object> values = new Dictionary<string, object>();
            //DataTable factTableSettings = GetfactTableSettings(applicationContext);
            DataTable factTableSettings = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardFactTables, $"TenantID='{applicationContext.TenantID}'");
            if (factTableSettings != null && factTableSettings.Rows.Count > 0)
            {
                DataRow[] dashboardRows = factTableSettings.Select($"{DatabaseObjects.Columns.Title}='{factTableName}'");

                if (dashboardRows.Length > 0)
                    dashboardSetting = dashboardRows[0];
            }

            DataTable dashboardData = null;
            //StringBuilder sbViewFields = null;

            // Get Dashboard summary fields info
            #region DashboardSummary

            if (factTableName.Trim() == "DashboardSummary")
            {
                dashboardData = (DataTable)CacheHelper<object>.Get($"DashboardSummary", applicationContext.TenantID);
                if (dashboardData == null)
                {
                    values.Add("@TenantID", applicationContext.TenantID);
                    dashboardData = GetTableDataManager.GetData(DatabaseObjects.Tables.DashboardSummary, values);
                    CacheHelper<object>.AddOrUpdate($"DashboardSummary", applicationContext.TenantID, dashboardData);
                }

                //if (refreshMode == "ChangesOnly" && dashboardSetting != null && GetDashboardCacheInstance().factTables.ContainsKey(factTableName)
                //    && GetDashboardCacheInstance().factTables[factTableName] != null)
                //{
                //    dashboardData = GetDashboardCacheInstance().factTables[factTableName];
                //}
                //else
                //{
                //    values.Add("@TenantID", applicationContext.TenantID);
                //    dashboardData = GetTableDataManager.GetData(DatabaseObjects.Tables.DashboardSummary, values);
                //}
            }

            #endregion

            #region ResourceUsageSummaryWeekWise

            //Get Dashboard ResourceAllocationSummary fields info
            else if (factTableName.Trim() == "ResourceUsageSummaryWeekWise")
            {
                if (refreshMode == "ChangesOnly" && dashboardSetting != null &&
                    GetDashboardCacheInstance().factTables.ContainsKey(factTableName) &&
                    GetDashboardCacheInstance().factTables[factTableName] != null)
                {
                    dashboardData = GetDashboardCacheInstance().factTables[factTableName];

                }
                else
                {
                    values.Add("@TenantID", applicationContext.TenantID);
                    dashboardData = GetTableDataManager.GetData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, values);

                }
            }

            #endregion

            #region ResourceUsageSummaryMonthWise

            //Get Dashboard ResourceUsageSummaryMonthWise fields info
            else if (factTableName.Trim() == "ResourceUsageSummaryMonthWise")
            {
                if (refreshMode == "ChangesOnly" && dashboardSetting != null && GetDashboardCacheInstance().factTables.ContainsKey(factTableName)
                    && GetDashboardCacheInstance().factTables[factTableName] != null)
                {
                    dashboardData = GetDashboardCacheInstance().factTables[factTableName];
                }
                else
                {
                    values.Add("@TenantID", applicationContext.TenantID);
                    dashboardData = GetTableDataManager.GetData(DatabaseObjects.Tables.ResourceUsageSummaryMonthWise, values);
                }
            }

            #endregion

            #region SprintSummary

            else if (factTableName.Trim() == "SprintSummary")
            {
                if (refreshMode == "ChangesOnly" && dashboardSetting != null && GetDashboardCacheInstance().factTables.ContainsKey(factTableName)
                    && GetDashboardCacheInstance().factTables[factTableName] != null)
                {
                    dashboardData = GetDashboardCacheInstance().factTables[factTableName];
                }
                else
                {
                    values.Add("@TenantID", applicationContext.TenantID);
                    dashboardData = GetTableDataManager.GetData(DatabaseObjects.Tables.SprintSummary, values);
                }
            }

            #endregion

            #region Assets

            else if (factTableName == "Assets")
            {
                var assetModule = moduleViewManager.LoadByName("CMDB");

                if (assetModule != null)
                {
                    var dtAssets = ticketManager.GetAllTickets(assetModule);
                    if (dtAssets != null)
                    {
                        dashboardData = dtAssets;
                    }
                }
            }

            #endregion
            else if (factTableName.Trim() == "AllRevenues")
            {
                dashboardData = (DataTable)CacheHelper<object>.Get($"AllRevenues", applicationContext.TenantID);
                if (dashboardData == null)
                {
                    values.Add("@TenantID", applicationContext.TenantID);
                    dashboardData = GetTableDataManager.GetData("AllRevenues", values);
                    CacheHelper<object>.AddOrUpdate($"AllRevenues", applicationContext.TenantID, dashboardData);
                }
            }

            #region Database Lists

            else
            {
                if (dashboardSetting != null)
                {
                    int dashboardPanelInfo = 0;
                    if (UGITUtility.IsSPItemExist(dashboardSetting, "DashboardPanelInfo") &&
                        Int32.TryParse(Convert.ToString(dashboardSetting["DashboardPanelInfo"]), out dashboardPanelInfo))
                    {
                        QueryHelperManager queryHelper = new QueryHelperManager(applicationContext);
                        dashboardData = queryHelper.GetReportData(dashboardPanelInfo);
                    }
                    else
                    {
                        values.Add("@TenantID", applicationContext.TenantID);
                        DataTable factSPList = GetTableDataManager.GetData(factTableName, values);
                        if ((factSPList == null || factSPList.Rows.Count == 0) && uHelper.IsTableExist(factTableName))
                            factSPList = GetTableDataManager.GetTableData(factTableName, $"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");
                        dashboardData = factSPList;
                        if (factSPList != null)
                        {

                            if (refreshMode == "ChangesOnly" && dashboardSetting != null &&
                                GetDashboardCacheInstance().factTables.ContainsKey(factTableName) &&
                                GetDashboardCacheInstance().factTables[factTableName] != null)
                            {
                                dashboardData = GetDashboardCacheInstance().factTables[factTableName];
                                DateTime lastUpdated = Convert.ToDateTime(dashboardSetting[DatabaseObjects.Columns.Modified]);
                                string query = string.Format("{0}>='{1}' and {2}='{3}'", DatabaseObjects.Columns.Modified, lastUpdated, DatabaseObjects.Columns.TenantID, applicationContext.TenantID);
                                DataTable fTable = GetTableDataManager.GetTableData(factTableName, query);

                                if (fTable != null && fTable.Rows.Count > 0)
                                {
                                    foreach (DataRow row in fTable.Rows)
                                    {
                                        DataRow[] dashboardRows = dashboardData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, row[DatabaseObjects.Columns.Id]));
                                        DataRow needUpdate = null;

                                        if (dashboardRows.Length > 0)
                                        {
                                            needUpdate = dashboardRows[0];
                                        }
                                        else
                                        {
                                            needUpdate = dashboardData.NewRow();
                                        }

                                        needUpdate = row;
                                    }
                                }
                            }
                            //else
                            //{
                            //    //dashboardData = GetTableDataManager.GetTableData(factTableName, $"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");
                            //    dashboardData = GetTableDataManager.GetData(factTableName, values);
                            //}
                        }
                    }
                }
                else
                {
                    string prefix = factTableName.Split('-')[0];
                    if (prefix == null || prefix.Trim() == string.Empty)
                    {
                        return dashboardData;
                    }

                    if (prefix == "Task")
                    {
                        string type = factTableName.Split('-')[1];
                        if (type.ToLower() == "open")
                        {
                            DataTable openTask = GetOpenTasks(applicationContext);                 //TaskCache.GetOpenedTasks();
                            dashboardData = openTask;
                        }
                        else if (type.ToLower() == "closed")
                        {
                            DataTable closedTask = GetCompletedTasks(applicationContext);          //TaskCache.GetCompletedTasks();
                            dashboardData = closedTask;
                        }
                        else if (type.ToLower() == "all")
                        {
                            DataTable allTask = GetAllTasks(applicationContext);                   //TaskCache.GetAllTasks();
                            dashboardData = allTask;
                        }
                    }
                    else
                    {
                        UGITModule module = moduleViewManager.LoadByName(prefix);
                        if (module != null && factTableName.Contains("-"))
                        {
                            // added condition to fix Error when creating New Query from Dashboard & Queries (from Admin Page), when 'Table' radiobutton is selected.
                            string type = factTableName.Split('-')[1];
                            if (type.ToLower() == "opentickets")
                            {
                                DataTable openTickets = ticketManager.GetOpenTickets(module);
                                dashboardData = openTickets;
                            }
                            else if (type.ToLower() == "closedtickets")
                            {
                                DataTable closedTickets = ticketManager.GetClosedTickets(module);
                                dashboardData = closedTickets;
                            }
                            else if (type.ToLower() == "alltickets")
                            {
                                DataTable tickets = ticketManager.GetAllTickets(module);
                                dashboardData = tickets;
                            }
                        }
                        else if (prefix.ToLower() == "allopensmstickets")
                        {
                            DataTable data = null;
                            List<UGITModule> modules = moduleViewManager.Load(x => x.ModuleType == ModuleType.SMS && x.EnableModule);
                            if (modules != null && modules.Count > 0)
                            {
                                foreach (UGITModule row in modules)
                                {
                                    if (UGITUtility.StringToBoolean(row.EnableCache))
                                    {
                                        UGITModule ugitmodule = moduleViewManager.LoadByName(row.ModuleName);
                                        DataTable openTickets = ticketManager.GetOpenTickets(ugitmodule);
                                        if (openTickets != null && !string.IsNullOrEmpty(ugitmodule.ModuleName))
                                        {
                                            string query = string.Empty;
                                            LifeCycle lifeCycle = ugitmodule.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                                            LifeCycleStage resolvedStage = null;
                                            DataRow[] dr = new DataRow[0];
                                            if (lifeCycle != null)
                                                resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Resolved.ToString());
                                            // Exclude resolved tickets
                                            if (ugitmodule.WaitingOnMeExcludesResolved)
                                            {
                                                if (resolvedStage != null)
                                                    query = string.Format("{0} < {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep);
                                                DataView dv = new DataView(openTickets);

                                                dv.RowFilter = query; // query
                                                openTickets = dv.ToTable();
                                            }
                                            else
                                            {
                                                if (data == null)
                                                    data = openTickets;
                                                else
                                                    data.Merge(openTickets);
                                            }
                                        }
                                    }
                                }

                                dashboardData = data;
                            }
                        }
                        else if (prefix.ToLower() == "allopentickets")
                        {
                            DataTable data = null;
                            List<UGITModule> modules = moduleViewManager.Load(x => x.EnableModule);

                            if (modules != null && modules.Count > 0)
                            {
                                foreach (UGITModule row in modules)
                                {
                                    if (UGITUtility.StringToBoolean(row.EnableCache))
                                    {
                                        DataTable openTickets = ticketManager.GetOpenTickets(row);
                                        if (openTickets != null)
                                        {
                                            if (data == null)
                                                data = openTickets;
                                            else
                                                data.Merge(openTickets);
                                        }
                                    }
                                }

                                dashboardData = data;
                            }
                        }
                        else if (prefix.ToLower() == "allclosedtickets")
                        {
                            DataTable data = null;
                            List<UGITModule> modules = moduleViewManager.Load(x => x.EnableModule);
                            if (modules != null && modules.Count > 0)
                            {
                                foreach (UGITModule row in modules)
                                {
                                    if (UGITUtility.StringToBoolean(row.EnableCache))
                                    {
                                        DataTable closedTickets = ticketManager.GetClosedTickets(row);
                                        if (closedTickets != null)
                                        {
                                            if (data == null)
                                                data = closedTickets;
                                            else
                                                data.Merge(closedTickets);
                                        }
                                    }
                                }

                                dashboardData = data;
                            }
                        }
                        else if (prefix.ToLower() == "allclosedsmstickets")
                        {
                            DataTable data = null;
                            List<UGITModule> modules = moduleViewManager.Load(x => x.ModuleType == ModuleType.SMS && x.EnableModule).ToList();

                            if (modules != null && modules.Count > 0)
                            {
                                foreach (UGITModule row in modules)
                                {
                                    if (UGITUtility.StringToBoolean(row.EnableCache))
                                    {
                                        DataTable openTickets = ticketManager.GetClosedTickets(row);
                                        if (openTickets != null)
                                        {
                                            if (data == null)
                                                data = openTickets;
                                            else
                                                data.Merge(openTickets);
                                        }
                                    }
                                }
                                /*
                                dashboardData = data.AsEnumerable()
                            .Where(r => r.Field<string>("Status") != "Pending Assignment")
                            .CopyToDataTable();
                                */

                                var dr = data.AsEnumerable().Where(r => r.Field<string>("Status") != "Pending Assignment");
                                if (dr != null && dr.Count() > 0)
                                    dashboardData = dr.CopyToDataTable();
                                else
                                    data = new DataTable();

                                //dashboardData = data;
                            }
                        }
                        else if (prefix.ToLower() == "alltickets")
                        {
                            DataTable data = null;
                            List<UGITModule> modules = moduleViewManager.Load(x => x.EnableModule);

                            if (modules != null && modules.Count > 0)
                            {
                                foreach (UGITModule row in modules)
                                {
                                    if (UGITUtility.StringToBoolean(row.EnableCache))
                                    {
                                        DataTable openTickets = ticketManager.GetAllTickets(row);
                                        if (openTickets != null)
                                        {
                                            if (data == null)
                                                data = openTickets;
                                            else
                                                data.Merge(openTickets);
                                        }
                                    }
                                }

                                dashboardData = data;
                            }
                        }
                        else if (prefix.ToLower() == "allsmstickets")
                        {
                            DataTable data = null;
                            List<UGITModule> modules = moduleViewManager.Load(x => x.ModuleType == ModuleType.SMS && x.EnableModule);

                            if (modules != null && modules.Count > 0)
                            {
                                foreach (UGITModule row in modules)
                                {
                                    if (UGITUtility.StringToBoolean(row.EnableCache))
                                    {

                                        DataTable openTickets = ticketManager.GetAllTickets(row);
                                        if (openTickets != null)
                                        {
                                            if (data == null)
                                                data = openTickets;
                                            else
                                                data.Merge(openTickets);
                                        }
                                    }
                                }

                                dashboardData = data;
                            }
                        }
                        else
                        {
                            string modulename = moduleViewManager.GetModuleByTableName(factTableName);
                            module = moduleViewManager.LoadByName(modulename);
                            if (module != null)
                            {
                                dashboardData = ticketManager.GetAllTicketsByModuleName(module, applicationContext);
                            }
                            else
                            {
                                dashboardData = GetTableDataManager.GetTableData(factTableName, $"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");
                                dashboardData = UpdateLookupAndUserFieldValues(applicationContext, dashboardData);
                            }
                        }

                        if (prefix.ToLower() == "allopensmstickets" || prefix.ToLower() == "allopentickets" || prefix.ToLower() == "allclosedtickets" || prefix.ToLower() == "allclosedsmstickets" || prefix.ToLower() == "alltickets" || prefix.ToLower() == "allsmstickets")
                        {
                            //Add modulename column with expression to get module name from ticketid
                            //For all module, we have ticketid max length 13 but CMDB having length 14 so for CMDB it gets substirng of 4 character otherwise 3 character is fine.
                            //Expression don't have split or indexof function, thats why, this logic is put it here.
                            if (dashboardData != null && dashboardData.Columns.Contains(DatabaseObjects.Columns.TicketId) && !dashboardData.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                            {
                                dashboardData.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string), string.Format("substring([{0}], 1, len([{0}])-10)", DatabaseObjects.Columns.TicketId));
                            }
                        }
                    }
                }
            }

            #endregion

            // Filter data table to keep view fields only
            //if (dashboardData != null && sbViewFields != null && sbViewFields.Length > 0)
            //{
            //    var dv = new DataView(dashboardData);
            //    dashboardData = dv.ToTable(false, sbViewFields.ToString().Split(','));
            //}

            return dashboardData;
        }

        /// <summary>
        /// Method to set the datatype for LookUp and Choices type fields from FieldConfiguration table
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected static List<FactTableField> CreateFactTableFieldsData(ApplicationContext context, DataTable data, string tableName)
        {
            List<FactTableField> lstFactTableFields = new List<FactTableField>();

            if (data == null)
                return lstFactTableFields;

            FieldConfigurationManager fcManager = new FieldConfigurationManager(context);
            List<FieldConfiguration> fcFields = fcManager.Load();
            bool fcHasData = fcFields != null && fcFields.Count > 0;
            bool fieldExists = false;

            foreach (DataColumn column in data.Columns)
            {
                // check if fcFields contains the column and it's datatype is either 'Lookup' or 'Choices'
                fieldExists = fcHasData && fcFields.Any(x => x.FieldName.EqualsIgnoreCase(column.ColumnName) && (x.Datatype.ToLower() == "lookup" || x.Datatype.ToLower() == "choices"));

                if (fieldExists)
                    lstFactTableFields.Add(new FactTableField(tableName, column.ColumnName, "System.String"));
                else
                    lstFactTableFields.Add(new FactTableField(tableName, column.ColumnName, column.DataType.ToString()));

            }
            return lstFactTableFields;
        }

        /// <summary>
        /// This Method is used to get all tasks
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static DataTable GetAllTasks(ApplicationContext context)
        {
            DataTable dataTable = null;
            DataTable data = null;

            // Get tasks for NPR, PMM, TSK & SVC
            UGITTaskManager taskManager = new UGITTaskManager(context);
            data = taskManager.GetAllTasksForQuery();

            if (data != null)
            {
                dataTable = data;

                // Remove parentTask column as it a not mapped column of type UGITTask which causes problem while merging with ModuleStageConstraints and PMMIssues
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ParentTask, dataTable))
                    dataTable.Columns.Remove(DatabaseObjects.Columns.ParentTask);
            }

            // Get Module Stage Constraints tasks
            ModuleStageConstraintsManager constraintsManager = new ModuleStageConstraintsManager(context);
            data = constraintsManager.GetAllStageConstraints();
            if (data != null)
            {
                if (dataTable == null)
                    dataTable = data;
                else
                    dataTable.Merge(data);
            }

            // Get PMM issues
            UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
            List<UGITTask> lstTasks = uGITTaskManager.LoadByModule(ModuleNames.PMM);
            data = UGITUtility.ToDataTable<UGITTask>(lstTasks.Where(x => x.SubTaskType.ToLower() == "issues").ToList());
            //PMMIssueManager pmmIssueManager = new PMMIssueManager(context);
            //data = pmmIssueManager.GetAllPMMIssues();
            if (data != null)
            {
                if (dataTable == null)
                    dataTable = data;
                else
                    dataTable.Merge(data);
            }

            return dataTable;
        }

        /// <summary>
        /// This Method is used to get all open tasks
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static DataTable GetOpenTasks(ApplicationContext context)
        {
            DataTable dataTable = null;
            DataTable data = null;

            // Get tasks for NPR, PMM, TSK & SVC
            UGITTaskManager taskManager = new UGITTaskManager(context);
            data = taskManager.GetOpenTasksForQuery();

            if (data != null)
            {
                dataTable = data;

                // Remove parentTask column as it a not mapped column of type UGITTask which causes problem while merging with ModuleStageConstraints and PMMIssues
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ParentTask, dataTable))
                    dataTable.Columns.Remove(DatabaseObjects.Columns.ParentTask);
            }

            // Get Module Stage Constraints tasks
            ModuleStageConstraintsManager constraintsManager = new ModuleStageConstraintsManager(context);
            data = constraintsManager.GetOpenStageConstraints();
            if (data != null)
            {
                if (dataTable == null)
                    dataTable = data;
                else
                    dataTable.Merge(data);
            }

            // Get PMM issues
            //PMMIssueManager pmmIssueManager = new PMMIssueManager(context);
            //data = pmmIssueManager.GetOpenPMMIssues();
            UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
            List<UGITTask> lstTasks = uGITTaskManager.LoadByModule(ModuleNames.PMM);
            data = UGITUtility.ToDataTable<UGITTask>(lstTasks.Where(x => x.SubTaskType.ToLower() == "issues" && x.Status != Constants.Completed).ToList());

            if (data != null)
            {
                if (dataTable == null)
                    dataTable = data;
                else
                    dataTable.Merge(data);
            }

            return dataTable;
        }

        /// <summary>
        /// This Method is used to get all completed tasks
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static DataTable GetCompletedTasks(ApplicationContext context)
        {
            DataTable dataTable = null;
            DataTable data = null;

            // Get tasks for NPR, PMM, TSK & SVC
            UGITTaskManager taskManager = new UGITTaskManager(context);
            data = taskManager.GetCompletedTasksForQuery();

            if (data != null)
            {
                dataTable = data;

                // Remove parentTask column as it a not mapped column of type UGITTask which causes problem while merging with ModuleStageConstraints and PMMIssues
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ParentTask, dataTable))
                    dataTable.Columns.Remove(DatabaseObjects.Columns.ParentTask);
            }

            // Get Module Stage Constraints tasks
            ModuleStageConstraintsManager constraintsManager = new ModuleStageConstraintsManager(context);
            data = constraintsManager.GetCompletedStageConstraints();
            if (data != null)
            {
                if (dataTable == null)
                    dataTable = data;
                else
                    dataTable.Merge(data);
            }

            // Get PMM issues
            //PMMIssueManager pmmIssueManager = new PMMIssueManager(context);
            //data = pmmIssueManager.GetCompletedPMMIssues();
            UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
            List<UGITTask> lstTasks = uGITTaskManager.LoadByModule(ModuleNames.PMM);
            data = UGITUtility.ToDataTable<UGITTask>(lstTasks.Where(x => x.SubTaskType.ToLower() == "issues" && x.Status == Constants.Completed).ToList());

            if (data != null)
            {
                if (dataTable == null)
                    dataTable = data;
                else
                    dataTable.Merge(data);
            }

            return dataTable;
        }

        /// <summary>
        /// This method is used to update values of Lookup & User type fields 
        /// lookup values or user id are changed with their Title or Name fields
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dashboardData"></param>
        /// <returns></returns>
        public static DataTable UpdateLookupAndUserFieldValues(ApplicationContext context, DataTable dashboardData)
        {
            if (dashboardData == null)
                return null;

            FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
            FieldConfiguration field = null;
            List<UserProfile> userProfiles = null;

            // Get all lookup and user type fields from field configuration table
            DataTable dtLookupAndUserFields = fieldManager.GetFieldConfiguration_Lookup_UserField();

            // Create schema of input table
            DataTable dtResult = dashboardData.Clone();
            dtResult.Clear();

            // Get lookup & user type fields from result table 
            // Here we are eliminating the ModuleNameLookup as we don't need to convert ModuleNames into Module Title
            var targetedFieldNames = dtResult.Columns.Cast<DataColumn>()
                .Where(y => y.ColumnName != DatabaseObjects.Columns.ModuleNameLookup)
                .Select(x => x.ColumnName).ToList();

            // Get list of FieldConfiguration for Lookup and User types fields
            List<FieldConfiguration> lstFieldConfig = (from o in dtLookupAndUserFields.AsEnumerable()
                                                       where targetedFieldNames.Contains(o.Field<string>("FieldName"))
                                                       select
                                                           new FieldConfiguration
                                                           {
                                                               //ID = o.Field<long>("ID"),
                                                               FieldName = o.Field<string>("FieldName"),
                                                               ParentTableName = o.Field<string>("ParentTableName"),
                                                               ParentFieldName = o.Field<string>("ParentFieldName"),
                                                               Datatype = o.Field<string>("Datatype")
                                                           }).ToList();


            // Change datatype of Lookup & User types columns to string
            dtResult.Columns.Cast<DataColumn>().ToList().ForEach(y =>
            {
                if ((y.ColumnName.EndsWith("Lookup") || y.ColumnName.EndsWith("User")) || (y.DataType != typeof(DateTime) && y.DataType != typeof(double) && y.DataType != typeof(Int32) && y.DataType != typeof(Int64) &&
                    y.DataType != typeof(decimal)))
                {
                    y.DataType = typeof(string);
                }
            });

            // Copy data from input table
            dashboardData.Rows.Cast<DataRow>().ToList().ForEach(x => { dtResult.Rows.Add(x.ItemArray); });

            // Update values for all lookup and user type fields
            dtResult.Columns.Cast<DataColumn>().ToList().ForEach(x =>
            {
                field = lstFieldConfig.FirstOrDefault(z => z.FieldName == x.ColumnName);
                var listOfUniqueValues = new List<KeyValuePair<string, string>>();

                if (field != null)
                {
                    dtResult.Rows.Cast<DataRow>().ToList().ForEach(y =>
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(y[field.FieldName])))
                            {
                                if (userProfiles == null)
                                    userProfiles = new UserProfileManager(context).GetUsersProfileWithGroup(context.TenantID);

                                var codeAllReadyPresent = listOfUniqueValues.Find(a =>
                                    a.Key == Convert.ToString(y[field.FieldName]));

                                if (codeAllReadyPresent.Value != null)
                                {
                                    y[x.ColumnName] = codeAllReadyPresent.Value;
                                }
                                else
                                {
                                    var value = fieldManager.GetFieldConfigurationData(field,
                                        Convert.ToString(y[field.FieldName]), userProfiles);

                                    if (!string.IsNullOrWhiteSpace(value))
                                    {
                                        listOfUniqueValues.Add(
                                            new KeyValuePair<string, string>(
                                                Convert.ToString(y[field.FieldName]), value));
                                        try
                                        {
                                            y[x.ColumnName] = value;
                                        }
                                        catch
                                        {
                                            // ignored
                                        }
                                    }
                                    else
                                    {
                                        y[x.ColumnName] = "";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                    });
                }

                listOfUniqueValues.Clear();
            });
            return dtResult;
        }


    }

    public class FactTableField
    {
        public string FieldName { get; set; }
        public string FieldDisplayName { get; set; }
        public string DataType { get; set; }
        public string TableName { get; set; }
        public FactTableField(string tableName, string fieldName, string dataType)
        {
            TableName = tableName;
            FieldName = fieldName;
            DataType = dataType;
        }

        public FactTableField(string tableName, string fieldName, string dataType, string fieldDisplayName)
        {
            TableName = tableName;
            FieldName = fieldName;
            DataType = dataType;
            FieldDisplayName = fieldDisplayName;
        }
    }
}
