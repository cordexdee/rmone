using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Reports;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Report.DxReport.ApplicationReport
{
    public partial class ApplicationReport_Viewer : System.Web.UI.UserControl
    {
        public string[] UserIds { get; set; }
        public int[] ApplicationIds { get; set; }
        public string[] Categories { get; set; }
        public bool isUserType { get; set; }
        public bool isDetails { get; set; }

        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        UserProfileManager userProfileManager = null;
        ApplicationModuleManager applicationModuleManager = null;
        ApplicationRoleManager applicationRoleManager = null;

        DataTable appTable = null;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (Request["UserIds"] != null)
            {
                if (!string.IsNullOrEmpty(Request["UserIds"]))
                {
                    UserIds = Request["UserIds"].Split(',');
                    UserIds = UserIds;
                }
            }
            if (Request["ApplicationIds"] != null)
            {
                if (!string.IsNullOrEmpty(Request["ApplicationIds"]))
                {
                   ApplicationIds = Array.ConvertAll<string, int>(Request["ApplicationIds"].Split(','), int.Parse);
                }
            }
            if (Request["Categories"] != null)
            {
                if (!string.IsNullOrEmpty(Request["Categories"]))
                {
                    Categories = Request["Categories"].Split(',');
                }
            }
            if (Request["isUserType"] != null)
            {
                isUserType = UGITUtility.StringToBoolean(Request["isUserType"]);
            }
            if (Request["isDetails"] != null)
            {
                isDetails = UGITUtility.StringToBoolean(Request["isDetails"]);
            }

            userProfileManager = new UserProfileManager(_context);
            applicationModuleManager = new ApplicationModuleManager(_context);
            applicationRoleManager = new ApplicationRoleManager(_context);

            string query = string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID);
            appTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, query);
            DataTable dtApplicationReport = CreateTable();
            ApplicationReportR appReport = new ApplicationReportR(dtApplicationReport, isDetails, isUserType);
            RptVwrApplicationReport.Report = appReport;
        }

        private DataTable CreateTable()
        {
            DataTable dtApplicationReport = new DataTable();

            dtApplicationReport.Columns.Add(new DataColumn("CategoryName"));
            dtApplicationReport.Columns.Add(new DataColumn("ApplicationId"));
            dtApplicationReport.Columns.Add(new DataColumn("ApplicationName"));
            dtApplicationReport.Columns.Add(new DataColumn("UserName"));
            dtApplicationReport.Columns.Add(new DataColumn("Module"));
            dtApplicationReport.Columns.Add(new DataColumn("UserId"));

            if (isUserType && UserIds != null && UserIds.Length > 0 && !string.IsNullOrEmpty(UserIds[0]))
                GetDataBasedOnUserIds(ref dtApplicationReport);
            else
                GetDataBasedOnApplicationIds(ref dtApplicationReport);

            dtApplicationReport.AcceptChanges();
            return dtApplicationReport;
        }

        private void GetDataBasedOnApplicationIds(ref DataTable dtApplicationReport)
        {
            ApplicationModule appModule = null;
            ApplicationRole appRole = null;

            DataTable spListItemColl = null;
            DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"TenantID='{_context.TenantID}'");
            if (dtApplications == null || dtApplications.Rows.Count == 0)
                return;

            foreach (int id in ApplicationIds)
            {
                DataRow[] drApps = dtApplications.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Id, id));

                if (drApps.Length > 0)
                {
                    DataRow dr = drApps[0];
                    string query = string.Format("{0}='{1}' and {2}={3}",DatabaseObjects.Columns.TenantID, _context.TenantID, DatabaseObjects.Columns.APPTitleLookup,UGITUtility.StringToInt(dr[DatabaseObjects.Columns.ID]));
                    spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship, query);

                    if (spListItemColl != null && spListItemColl.Rows.Count > 0)
                    {
                        #region Get modules and roles to avoid multiple hits to database in foreach loop
                        List<ApplicationModule> applicationModules = new List<ApplicationModule>();
                        List<ApplicationRole> applicationRoles = new List<ApplicationRole>();
                        List<long> lstOfModules = spListItemColl.DefaultView.ToTable(true, DatabaseObjects.Columns.ApplicationModulesLookup).AsEnumerable().Select(x => x.Field<long>(DatabaseObjects.Columns.ApplicationModulesLookup)).ToList();
                        List<long> lstOfRoles = spListItemColl.DefaultView.ToTable(true, DatabaseObjects.Columns.ApplicationRoleLookup).AsEnumerable().Select(x => x.Field<long>(DatabaseObjects.Columns.ApplicationRoleLookup)).ToList();

                        if (lstOfModules != null)
                            applicationModules = applicationModuleManager.Load(x => x.TenantID == _context.TenantID && lstOfModules.Any(y => y == x.ID));
                        if (lstOfRoles != null)
                            applicationRoles = applicationRoleManager.Load(x => x.TenantID == _context.TenantID && lstOfRoles.Any(y => y == x.ID));
                        appModule = null;
                        appRole = null;

                        #endregion

                        DataView dataView = spListItemColl.DefaultView;
                        dataView.Sort = string.Format("{0},{1},{2} asc", DatabaseObjects.Columns.ApplicationModulesLookup, DatabaseObjects.Columns.ApplicationRoleLookup, DatabaseObjects.Columns.ApplicationRoleAssign);
                        spListItemColl = dataView.ToTable();
                        UserProfile applRoleAssignee = null;
                        foreach (DataRow item in spListItemColl.Rows)
                        {
                            DataRow drNew;
                            DataRow[] drUsers = null;
                            applRoleAssignee = userProfileManager.GetUserById( Convert.ToString(item[DatabaseObjects.Columns.ApplicationRoleAssign]));

                            if (dtApplicationReport.Rows.Count > 0 && applRoleAssignee != null)
                            {
                                drUsers = dtApplicationReport.Select(string.Format("UserId='{0}' AND ApplicationId='{1}'", applRoleAssignee.Id, UGITUtility.StringToInt(dr[DatabaseObjects.Columns.ID])));
                            }

                            bool newRow = false;
                            if (drUsers != null && drUsers.Length > 0)
                                drNew = drUsers[0];
                            else
                            {
                                drNew = dtApplicationReport.NewRow();
                                newRow = true;
                            }

                            drNew["CategoryName"] = Convert.ToString(dr[DatabaseObjects.Columns.CategoryNameChoice]);
                            drNew["ApplicationId"] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.ID]);
                            drNew["ApplicationName"] = dr[DatabaseObjects.Columns.Title];
                            if (applRoleAssignee != null)
                            {
                                drNew["UserName"] = applRoleAssignee.Name;
                                drNew["UserId"] = applRoleAssignee.Id;
                            }
                            drNew["Module"] = "N/A";

                            appModule = applicationModules.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(item[DatabaseObjects.Columns.ApplicationModulesLookup]));
                            appRole = applicationRoles.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(item[DatabaseObjects.Columns.ApplicationRoleLookup]));
                            if (appModule != null && appModule.ID > 0)
                            {
                                if (string.IsNullOrEmpty(Convert.ToString(drNew["Module"])) || Convert.ToString(drNew["Module"]) == "N/A")
                                    drNew["Module"] = string.Format("{0} ({1})", appModule.Title, appRole.Title);
                                else
                                    drNew["Module"] = string.Format("{0}, {1} ({2})", drNew["Module"], appModule.Title, appRole.Title);
                            }

                            if (newRow)
                                dtApplicationReport.Rows.Add(drNew);
                        }
                    }
                }
            }

            if (dtApplicationReport != null && dtApplicationReport.Rows.Count > 0)
            {
                dtApplicationReport.DefaultView.Sort = "CategoryName,ApplicationName,UserName";
                dtApplicationReport = dtApplicationReport.DefaultView.ToTable();
            }
        }

        private void GetDataBasedOnUserIds(ref DataTable dtApplicationReport)
        {
            ApplicationModule applModule = null;
            ApplicationRole applRole = null;
            DataTable spListColl = null;
            foreach (string id in UserIds)
            {
                string query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ApplicationRoleAssign,id,DatabaseObjects.Columns.TenantID,_context.TenantID);

                spListColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplModuleRoleRelationship, query);
                if (spListColl != null && spListColl.Rows.Count > 0)
                {
                    #region Get modules and roles to avoid multiple hits to database in foreach loop
                    List<ApplicationModule> applicationModules = new List<ApplicationModule>();
                    List<ApplicationRole> applicationRoles = new List<ApplicationRole>();
                    List<long> lstOfModules = spListColl.DefaultView.ToTable(true, DatabaseObjects.Columns.ApplicationModulesLookup).AsEnumerable().Select(x => x.Field<long>(DatabaseObjects.Columns.ApplicationModulesLookup)).ToList();
                    List<long> lstOfRoles = spListColl.DefaultView.ToTable(true, DatabaseObjects.Columns.ApplicationRoleLookup).AsEnumerable().Select(x => x.Field<long>(DatabaseObjects.Columns.ApplicationRoleLookup)).ToList();
                    //ApplicationModuleManager applicationModuleManager = new ApplicationModuleManager(_context);
                    //ApplicationRoleManager applicationRoleManager = new ApplicationRoleManager(_context);
                    if (lstOfModules != null)
                        applicationModules = applicationModuleManager.Load(x => x.TenantID == _context.TenantID && lstOfModules.Any(y => y == x.ID));
                    if (lstOfRoles != null)
                        applicationRoles = applicationRoleManager.Load(x => x.TenantID == _context.TenantID && lstOfRoles.Any(y => y == x.ID));
                    applModule = null;
                    applRole = null;
                    #endregion

                    #region Apply sorting 
                    DataView dataView = spListColl.DefaultView;
                    dataView.Sort = string.Format("{0},{1},{2} asc", DatabaseObjects.Columns.APPTitleLookup,DatabaseObjects.Columns.ApplicationModulesLookup, DatabaseObjects.Columns.ApplicationRoleLookup);
                    spListColl = dataView.ToTable();
                    #endregion

                    long applicationTitle = 0;
                    UserProfile userProfile = null;
                    foreach (DataRow item in spListColl.Rows)
                    {
                        DataRow drNew = null;
                        DataRow[] drUsers = null;
                        applicationTitle = UGITUtility.StringToLong(Convert.ToString(item[DatabaseObjects.Columns.APPTitleLookup]));
                        string applRoleAssignee = Convert.ToString(item[DatabaseObjects.Columns.ApplicationRoleAssign]);

                        if (dtApplicationReport.Rows.Count > 0 && !string.IsNullOrEmpty(applRoleAssignee))
                        {
                            drUsers = dtApplicationReport.Select(string.Format("ApplicationId='{0}' AND UserId='{1}'", applicationTitle, applRoleAssignee));
                        }

                        bool newRow = false;
                        if (drUsers != null && drUsers.Length > 0)
                            drNew = drUsers[0];
                        else
                        {
                            drNew = dtApplicationReport.NewRow();
                            newRow = true;
                        }

                        DataRow appRow= appTable.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ID) == applicationTitle);
                        
                        drNew["ApplicationId"] = applicationTitle;
                        if(appRow!=null)
                        drNew["ApplicationName"] =UGITUtility.ObjectToString(appRow[DatabaseObjects.Columns.Title]);

                        userProfile = userProfileManager.LoadById(applRoleAssignee);
                        if (userProfile != null)
                        {
                            drNew["UserName"] = userProfile.Name;
                            drNew["UserId"] = userProfile.Id;
                        }

                        applModule = applicationModules.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(item[DatabaseObjects.Columns.ApplicationModulesLookup]));
                        applRole = applicationRoles.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(item[DatabaseObjects.Columns.ApplicationRoleLookup]));
                        if (applModule != null)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(drNew["Module"])) || Convert.ToString(drNew["Module"]) == "N/A")
                            {
                                drNew["Module"] = string.Format("{0} ({1})", applModule.Title, applRole.Title);
                            }
                            else
                            {
                                if (Convert.ToString(drNew["Module"]).Contains(applModule.Title))
                                {
                                    string module = Convert.ToString(drNew["Module"]);
                                    int index = module.IndexOf(applModule.Title);
                                    int lastIndex = module.IndexOf(")", index);
                                    module = module.Insert(lastIndex, string.Format(", {0}", applRole.Title));
                                    drNew["Module"] = module;
                                }
                                else
                                {
                                    drNew["Module"] = string.Format("{0}, {1} ({2})", drNew["Module"], applModule.Title, applRole.Title);
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(drNew["Module"])))
                            {
                                drNew["Module"] = string.Format("{0} ({1})", UGITUtility.ObjectToString(appRow[DatabaseObjects.Columns.Title]), applRole.Title);
                            }
                            else
                            {
                                drNew["Module"] = string.Format("{0}, {1} ({2})", drNew["Module"], UGITUtility.ObjectToString(appRow[DatabaseObjects.Columns.Title]), applRole.Title);
                            }
                        }

                        if (newRow)
                            dtApplicationReport.Rows.Add(drNew);
                    }
                }

                if (dtApplicationReport != null && dtApplicationReport.Rows.Count > 0)
                {
                    dtApplicationReport.DefaultView.Sort = "UserName,ApplicationName,Module";
                    dtApplicationReport = dtApplicationReport.DefaultView.ToTable();
                }
            }
        }
    }
}