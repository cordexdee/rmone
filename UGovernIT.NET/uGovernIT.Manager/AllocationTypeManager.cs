using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Manager
{
    public class AllocationTypeManager
    {
        private static DataTable CreateTable()
        {
            DataTable resultedTable = new DataTable();
            resultedTable.Columns.Add("LevelTitle", typeof(string));
            resultedTable.Columns.Add("LevelName", typeof(string));
            return resultedTable;
        }

        public static DataTable LoadLevel1(ApplicationContext context)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            var enabledModules = moduleViewManager.Load(x => x.EnableModule == true).Select(x => x.ModuleName);
            DataTable resultedTable = CreateTable();
            DataTable categoryMappingList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.Deleted}='{false}'and {DatabaseObjects.Columns.ModuleNameLookup} in ('{string.Join("','", enabledModules)}')");
            if (categoryMappingList.Rows.Count > 0)
            {
                DataTable collectionTable = categoryMappingList.DefaultView.ToTable(true, DatabaseObjects.Columns.RequestCategory);
                List<UGITModule> listModules = moduleViewManager.Load(x => x.EnableRMMAllocation).OrderBy(y => y.Title).ToList();
                if (listModules != null && listModules.Count > 0)
                {
                    resultedTable = UGITUtility.ToDataTable<UGITModule>(listModules).DefaultView.ToTable(true, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleName);
                    resultedTable.Columns[DatabaseObjects.Columns.Title].ColumnName = "LevelTitle";
                    resultedTable.Columns[DatabaseObjects.Columns.ModuleName].ColumnName = "LevelName";
                }
                collectionTable.DefaultView.Sort = DatabaseObjects.Columns.RequestCategory + " DESC";
                collectionTable = collectionTable.DefaultView.ToTable();
                ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
                // Put "Other" category at the bottom of the list
                DataRow[] lastRow = collectionTable.Select(string.Format("{0} is not null and {0} <> '' and {0} = '{1}'", DatabaseObjects.Columns.RequestCategory, configurationVariableManager.GetValue("RMMTypeOther")));
                if (lastRow.Count() > 0)
                {
                    DataTable tempTable = collectionTable.Clone();
                    tempTable.ImportRow(lastRow[0]);
                    collectionTable.Rows.Remove(lastRow[0]);
                    collectionTable.ImportRow(tempTable.Rows[0]);
                }
                var disableRequestTypeAllocation = configurationVariableManager.GetValueAsBool("disableRequestTypeAllocation");
                if (!disableRequestTypeAllocation)
                {
                    if (collectionTable != null)
                    {
                        foreach (DataRow dr in collectionTable.Rows)
                        {
                            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.RequestCategory])))
                            {
                                resultedTable.Rows.Add(dr[DatabaseObjects.Columns.RequestCategory], dr[DatabaseObjects.Columns.RequestCategory]);
                            }
                        }
                    }
                }

            }
            return resultedTable;
        }

        public static DataTable LoadLevel2(ApplicationContext context, string level1, bool fromModule)
        {


            if (fromModule)
            {
                //if project then select project ids
                return LoadProjectLevel2(context, level1);
            }
            else
            {
                return LoadCategoryLevel2(context, level1);
            }
        }

        public static DataTable LoadLevel3(ApplicationContext context, string level1, string level2, string globalRoleId, bool isModule = false, bool isProjStandardWorkItemsEnabled = false)
        {
            if (isModule)
            {
                if (level1 == "CPR" || level1 == "OPM" || level1 == "CNS" || level1 == "PMM" || level1 == "TSK" || level1 == "NPR")
                {
                    if (!isProjStandardWorkItemsEnabled)
                        return LoadLevel2ProjectGroups(context, globalRoleId);
                    else
                        return LoadLevel2ProjectStdWorkItems(context);
                }

                //No need to fetch 3 level data for module.
                return null;
            }
            else
            {
                return LoadCategoryLevel3(context, level1, level2);
            }
        }

        public static DataTable LoadLevel4(ApplicationContext context, string level1, string secondLevelVal, bool isModule, bool isProjStandardWorkItemsEnabled = false)
        {
            if (isModule)
            {
                if (level1 == "CPR" || level1 == "OPM" || level1 == "CNS" || level1 == "PMM" || level1 == "TSK" || level1 == "NPR")
                {
                    ModuleViewManager moduleManager = new ModuleViewManager(context);
                    TicketManager ticketManager = new TicketManager(context);
                    UGITModule module = moduleManager.LoadByName(level1);
                    DataRow dr = ticketManager.GetByTicketID(module, secondLevelVal);
                    bool EnableStdWorkItems = false;
                    if (UGITUtility.IfColumnExists(dr, "EnableStdWorkItems"))
                        EnableStdWorkItems = Convert.ToBoolean(dr["EnableStdWorkItems"] == DBNull.Value ? false : dr["EnableStdWorkItems"]);

                    if (isProjStandardWorkItemsEnabled && EnableStdWorkItems)
                        return LoadLevel2ProjectStdWorkItems(context);
                }

                return null;
            }
            return null;
        }

        private static DataTable LoadCategoryLevel2(ApplicationContext context, string level1)
        {
            DataRow[] categoryMappingRows = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(DatabaseObjects.Columns.RequestCategory + " ='" + level1 + "'");
            if (categoryMappingRows.Count() > 0)
            {
                DataTable collectionTable = categoryMappingRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.Category, DatabaseObjects.Columns.ID,
                    DatabaseObjects.Columns.SubCategory, DatabaseObjects.Columns.RequestType);
                collectionTable.Columns[DatabaseObjects.Columns.Category].ColumnName = "LevelTitle";
                collectionTable.DefaultView.Sort = "LevelTitle ASC";
                return collectionTable.DefaultView.ToTable();

            }
            return null;
        }

        private static DataTable LoadCategoryLevel3(ApplicationContext context, string level1, string level2)
        {
            DataRow[] categoryMappingRows = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(
                DatabaseObjects.Columns.RequestCategory + " ='" + level1 + "' AND " + DatabaseObjects.Columns.Category + " = '" + level2 + "'");
            if (categoryMappingRows != null && categoryMappingRows.Count() > 0)
            {
                //DataTable collectionTable = categoryMappingRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketRequestType);
                DataTable collectionTable = categoryMappingRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.SubCategory);
                if (collectionTable == null || collectionTable.Rows.Count == 0 || string.IsNullOrEmpty(Convert.ToString(collectionTable.Rows[0][DatabaseObjects.Columns.SubCategory])))
                {
                    collectionTable = categoryMappingRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketRequestType);
                    collectionTable.Columns[DatabaseObjects.Columns.TicketRequestType].ColumnName = "LevelId";
                }
                else
                {
                    collectionTable.Columns[DatabaseObjects.Columns.SubCategory].ColumnName = "LevelId";
                }

                if (collectionTable != null)
                {
                    collectionTable.Columns.Add("LevelName", typeof(string));
                    foreach (DataRow row in collectionTable.Rows)
                    {
                        row["LevelName"] = row["LevelId"];
                    }
                }

                collectionTable.DefaultView.Sort = "LevelId ASC";
                return collectionTable.DefaultView.ToTable();
            }
            return null;
        }

        private static DataTable LoadLevel2ProjectGroups(ApplicationContext context, string globalRoleId)
        {

            DataTable dtGroups = new DataTable();
            dtGroups.Columns.Add("LevelId");
            dtGroups.Columns.Add("LevelName");

            GlobalRoleManager globalRoleManager = new GlobalRoleManager(context);
            List<GlobalRole> globalRoles = new List<GlobalRole>();
            if (!string.IsNullOrEmpty(globalRoleId))
                globalRoles = globalRoleManager.Load(x => x.Id == globalRoleId);
            else
                globalRoles = uHelper.GetGlobalRoles(context, false);

            foreach (GlobalRole oGroup in globalRoles)
            {
                DataRow dr = dtGroups.NewRow();
                dr["LevelId"] = oGroup.Id;
                dr["LevelName"] = oGroup.Name;
                dtGroups.Rows.Add(dr);

            }
            dtGroups.DefaultView.Sort = "LevelName ASC";
            return dtGroups;
        }
        private static DataTable LoadLevel2ProjectStdWorkItems(ApplicationContext context)
        {
            DataTable dtStdWorkItems = new DataTable();
            dtStdWorkItems.Columns.Add("LevelId");
            dtStdWorkItems.Columns.Add("LevelName");
            dtStdWorkItems.Columns.Add("LevelCode");
            dtStdWorkItems.Columns.Add("LevelDescription");
            dtStdWorkItems.Columns.Add("LevelItem");

            ProjectStandardWorkItemManager workItemMgr = new ProjectStandardWorkItemManager(context);
            List<ProjectStandardWorkItem> workItems = new List<ProjectStandardWorkItem>();
            workItems = workItemMgr.Load(x => x.Deleted == false).OrderBy(x => x.ItemOrder).ToList();

            foreach (ProjectStandardWorkItem item in workItems)
            {
                DataRow dr = dtStdWorkItems.NewRow();
                dr["LevelId"] = item.ID;
                dr["LevelName"] = item.Title;
                dr["LevelCode"] = item.Code;
                dr["LevelDescription"] = item.Description;
                dr["LevelItem"] = $"{item.Title}{Constants.Separator}{item.Code}";
                dtStdWorkItems.Rows.Add(dr);

            }
            //dtStdWorkItems.DefaultView.Sort = "LevelName ASC";
            return dtStdWorkItems;
        }


        /// <summary>
        /// Only from PMMLeveleItem2 and PMMProjectItem3
        /// </summary>
        /// <param name="level1"></param>
        /// <returns></returns>
        private static DataTable LoadProjectLevel2(ApplicationContext context, string level1)
        {
            //Get Open project only
            DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, level1));

            DataRow drModule = drModules != null && drModules.Length > 0 ? drModules[0] : null;

            string pmmClosedStageId = uHelper.GetModuleStageId(context, drModule, StageType.Closed);

            DataTable dt = new DataTable();
            dt = CacheHelper<object>.Get($"OpenTicket_{Convert.ToString(drModule[DatabaseObjects.Columns.ModuleName])}", context.TenantID) as DataTable;

            if (dt == null)
                dt = GetTableDataManager.GetTableData(Convert.ToString(drModule[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.Closed} != 1  AND {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            DataTable dataTable = null;
            if (dt.Rows.Count > 0)
            {
                DataRow[] dataRows = dt.Select();  //  dt.Select(string.Format("{0}<>{1} And {2}<>'True'", DatabaseObjects.Columns.ModuleStepLookup, pmmClosedStageId, DatabaseObjects.Columns.TicketClosed));
                if (dataRows != null && dataRows.Count() > 0)
                {
                    dataTable = dataRows.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).CopyToDataTable();
                }
                else
                {
                    dataTable = dt.Clone();
                    dataTable.Merge(dt);

                }
            }
            else
            {
                dataTable = dt;
            }
            if (dataTable != null && !dataTable.Columns.Contains(DatabaseObjects.Columns.TicketId) && dataTable.Rows.Count <= 0)
            {
                dataTable.Columns.Add(DatabaseObjects.Columns.TicketId);

            }
            else
            {
                //dataTable.Columns.Add(DatabaseObjects.Columns.TicketId);

                // dataTable.Columns[DatabaseObjects.Columns.TicketId].ColumnName = "LevelTitle";
                dataTable.Columns[DatabaseObjects.Columns.TicketId].ColumnName = "LevelTitle";

            }
            dataTable.DefaultView.Sort = "LevelTitle ASC";
            return dataTable = dataTable.DefaultView.ToTable();
        }

        /// <summary>
        /// Not in use
        /// </summary>
        /// <param name="level1Id"></param>
        /// <param name="level2Id"></param>
        /// <returns></returns>
        private static DataTable LoadProjectLevel3(ApplicationContext context, string level1Id, string level2Id)
        {
            DataTable projectTasks = null;
            string dataQuery = "";
            string RMMLevel1PMMProjects = uHelper.GetModuleTitle("PMM");
            string RMMLevel1TSKProjects = uHelper.GetModuleTitle("TSK");
            UGITTaskManager objTaskManager = new UGITTaskManager(context);

            if (level1Id.Equals(RMMLevel1PMMProjects, StringComparison.CurrentCultureIgnoreCase))
            {
                DataTable list = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMTasks, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                //dataQuery.ViewFields = string.Format("<FieldRef Name=\"{0}\" /><FieldRef Name='{1}' Nullable='TRUE' Type='Lookup'/><FieldRef Name='{2}' Nullable='TRUE' Type='Lookup'/>",
                //                         DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketPMMIdLookup, DatabaseObjects.Columns.TSKIDLookup);
                //dataQuery.Webs = "<Webs Scope=\"SiteCollection\" />";
                dataQuery = string.Format("{1}='{2}'", DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketPMMIdLookup, level2Id);
                DataTable dataTable = list.Select(dataQuery).OrderBy(x => x[DatabaseObjects.Columns.Title]).CopyToDataTable();
                if (dataTable.Columns.Count > 0)
                {
                    projectTasks = objTaskManager.GetAllTasksByProjectID("PMM", level2Id);
                }
            }
            else if (level1Id.Equals(RMMLevel1TSKProjects, StringComparison.CurrentCultureIgnoreCase))
            {
                //dataQuery = new SPSiteDataQuery();
                //Guid tskTasklistID = uGITCache.GetListID(DatabaseObjects.Lists.TSKTasks);
                //dataQuery.Lists = string.Format("<Lists Hidden='True'><List ID='{0}'/></Lists>", tskTasklistID);
                //dataQuery.ViewFields = string.Format("<FieldRef Name=\"{0}\" /><FieldRef Name='{1}' Nullable='TRUE' Type='Lookup'/>",
                //                         DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TSKIDLookup);
                //dataQuery.Webs = "<Webs Scope=\"SiteCollection\" />";
                //dataQuery.Query = string.Format(@"<Where><Eq><FieldRef Name='{1}'/><Value Type='Lookup'>{2}</Value></Eq></Where><OrderBy><FieldRef Name='{0}' /></OrderBy>",
                //                                DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TSKIDLookup, level2Id);
                //DataTable dataTable = SPContext.Current.Web.GetSiteData(dataQuery);
                //if (dataTable.Columns.Count > 0)
                //    projectTasks = TaskCache.GetAllTasksByProjectID("TSK", level2Id);
            }
            else
            {
                // Should not be here!!
                ULog.WriteLog("Invalid level1 passed to LoadProjectLevel3()");
            }

            if (projectTasks != null && projectTasks.Rows.Count > 0)
            {
                projectTasks.Columns[DatabaseObjects.Columns.Title].ColumnName = "LevelTitle";
                projectTasks.DefaultView.Sort = "LevelTitle ASC";
                return projectTasks;
            }

            return null;
        }

        public static bool IsMappingExist(string level1, string level2, string level3)
        {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            if (!string.IsNullOrEmpty(level2) && level2.Trim() != string.Empty)
            {
                List<string> requiredQuery = new List<string>();
                requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.RequestCategory, level1));
                if (!string.IsNullOrEmpty(level2) && level2.Trim() != string.Empty)
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.Category, level2));
                }
                if (!string.IsNullOrEmpty(level3) && level3.Trim() != string.Empty)
                {
                    requiredQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketRequestType, level3));
                }

                DataTable categoryMappingList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                if (categoryMappingList != null && categoryMappingList.Rows.Count > 0)
                {
                    // string Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(requiredQuery, requiredQuery.Count - 1, true));
                    DataRow[] resultedCollection = categoryMappingList.Select(string.Join(" and ", requiredQuery));
                    if (resultedCollection != null && resultedCollection.Count() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
