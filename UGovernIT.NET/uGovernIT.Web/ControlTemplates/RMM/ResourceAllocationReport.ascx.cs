using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using DevExpress.Web;
using DevExPrinting = DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using System.Drawing;
using DevExpress.Web.Rendering;
using System.Globalization;
using System.Collections;
using System.Text;
using uGovernIT.Manager;
using DevExpress.XtraGrid;
using DevExpress.Data;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.DAL;


namespace uGovernIT.Web.ControlTemplates.RMM
{
    public partial class ResourceAllocationReport : System.Web.UI.UserControl
    {
        private DateTime dateFrom;
        private DateTime dateTo;
        private ResourceAllocationManager ResourceAllocationManager = null;
        private ResourceWorkItemsManager ResourceWorkItemsManager = null;
        private UserProfileManager ObjUserProfileManager = null;
        ConfigurationVariableManager ConfigVariableMGR = null;
        FieldConfigurationManager fieldConfigMgr = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ResourceProjectComplexityManager cpxManager = null;
        private UserProfile CurrentUser;
        private UserProfile user;
        protected bool enableDivision;

        private string selectedCategory = string.Empty;
        private string selectedManager = string.Empty;
        public string SelectedUser = string.Empty;
        private string selecteddepartment = string.Empty;
        private long selectedfunctionalare = -1;
        List<string> selectedTypes = new List<string>();
        protected bool isResourceAdmin = false;
        public bool HideAllocationType { get; set; }
        //DateTime firstOfThisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        private string allowAllocationForSelf;
        private bool allowAllocationViewAll;
        private bool viewself = false;
        protected List<UserProfile> userEditPermisionList = null;
        protected List<UserProfile> userProfiles = null;
        DataTable allocationData = null;
        bool isPercentageMode = true;

        ConfigurationVariable cvAllocationTimeLineColor = null;
        //FieldConfiguration fieldConfig = null;
        private bool DisablePlannedAllocation;
        List<string> lstEstimateColors = null;
        List<string> lstEstimateColorsAndFontColors = null;
        List<string> lstAssignColors = null;
        List<string> lstAssignColorsAndFontColors = null;
        private bool chkIncludeClosed = false;
        public bool btnexport;
        public bool isAdminResource;
        //private bool rbtnBar = true;
        private bool rbtnFTE = false;
        string hdnaspDepartment = "";
        string hdndisplayMode = "Monthly";
        string hdnYear = "";

        protected TableRow SummaryTextRow { get; set; }
        protected GridViewGroupRowTemplateContainer Container { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            enableDivision = ConfigVariableMGR.GetValueAsBool(ConfigConstants.EnableDivision);
            cvAllocationTimeLineColor = ConfigVariableMGR.LoadVaribale("AllocationTimeLineColor");
            if (cvAllocationTimeLineColor != null && !string.IsNullOrEmpty(cvAllocationTimeLineColor.KeyValue))
            {
                string[] color = cvAllocationTimeLineColor.KeyValue.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                if (color.Length > 1)
                {
                    lstEstimateColors = color[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    lstAssignColors = color[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                    lstEstimateColors = color[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            lstEstimateColorsAndFontColors = RMMSummaryHelper.SetFontColors(lstEstimateColors);
            lstAssignColorsAndFontColors = RMMSummaryHelper.SetFontColors(lstAssignColors);
            lblErrorDepartmentReqrd.Visible = false;
            if (!IsPostBack)
            {
                //set department to current user's department
                UserProfile currentUserProfile = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(CurrentUser.Id));
                if (currentUserProfile != null && UGITUtility.StringToLong(currentUserProfile.Department) > 0)
                {
                    ddlDepartment.SetValues(Convert.ToString(currentUserProfile.Department));
                    hdnDepartment.Value = currentUserProfile.Department.ToString();
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            ResourceAllocationManager = new ResourceAllocationManager(context);
            ObjUserProfileManager = new UserProfileManager(context);
            ConfigVariableMGR = new ConfigurationVariableManager(context);
            ResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            cpxManager = new ResourceProjectComplexityManager(context);
            fieldConfigMgr = new FieldConfigurationManager(context);
            isResourceAdmin = ObjUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || ObjUserProfileManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            DisablePlannedAllocation = ConfigVariableMGR.GetValueAsBool(ConfigConstants.DisablePlannedAllocation);
            if (!isResourceAdmin)
                userEditPermisionList = ObjUserProfileManager.LoadAuthorizedUsers(allowAllocationForSelf);
            allowAllocationForSelf = ConfigVariableMGR.GetValue(ConfigConstants.AllowAllocationForSelf);
            allowAllocationViewAll = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowAllocationViewAll);
            userProfiles = ObjUserProfileManager.GetUsersProfile();
            CurrentUser = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(HttpContext.Current.CurrentUser().Id)); //HttpContext.Current.CurrentUser();
            dtFromDate.Date = uHelper.FirstDayOfMonth(DateTime.Now);
            dtToDate.Date = uHelper.LastDayOfMonth(DateTime.Now.AddMonths(11));
        }
        private DataTable GetAllocationData_old()
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Name, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Project, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.AllocationEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PlannedEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PctEstimatedAllocation, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.PctPlannedAllocation, typeof(int));
            data.Columns.Add("AllocationType", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.WorkItemID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.AllocationID, typeof(int));
            data.Columns.Add("ProjectNameLink", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Closed, typeof(bool));
            data.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ModuleRelativePagePath, typeof(string));
            data.Columns.Add("ExtendedDate", typeof(string));
            data.Columns.Add("ExtendedDateAssign", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));
            DataTable dtlist = new DataTable();

            dtlist = ResourceAllocationManager.GetDatelist(hdndisplayMode, dateFrom, dateTo);
            data.Merge(dtlist);
            data.Clear();
            SelectedUser = "";
            selectedCategory = "";
            selectedManager = "0";
            selectedfunctionalare = 0;
            if (!string.IsNullOrEmpty(ddlDepartment.GetValues()))
            {
                selecteddepartment = ddlDepartment.GetValues();
            }
            else if (!string.IsNullOrEmpty(hdnDepartment.Value))
            {
                selecteddepartment = hdnDepartment.Value;
            }

            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                dateFrom = uHelper.FirstDayOfMonth(DateTime.Now);
                dateTo = dateFrom.AddMonths(12);
            }
            List<UserProfile> lstUProfile = new List<UserProfile>();

            bool limitedUsers = false;
            lstUProfile = ObjUserProfileManager.GetUsersProfile();

            DataTable dtResult = null;
            DataTable workitems = null;
            List<string> userIds = new List<string>();

            if (!isResourceAdmin && !allowAllocationViewAll)
            {

                if (userEditPermisionList != null && userEditPermisionList.Count > 0)
                {
                    userIds.AddRange(userEditPermisionList.Select(x => x.Id));

                    if (!userIds.Contains(CurrentUser.Id))
                        userIds.Add(CurrentUser.Id);

                    userIds = userIds.Distinct().ToList();
                }

                if (viewself)
                {
                    if (!userIds.Contains(CurrentUser.Id))
                        userIds.Add(CurrentUser.Id);
                }

                if (userIds != null && userIds.Count > 0)
                {
                    dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                    workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 1);
                }
            }
            else
            {
                if (limitedUsers)
                {
                    userIds = lstUProfile.Select(x => x.Id).ToList();

                    dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                    workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 4);

                }
                else
                {
                    dtResult = ResourceAllocationManager.LoadRawTableByResource(null, 4, dateFrom, dateTo);
                    workitems = RMMSummaryHelper.GetOpenworkitems(context, false); //workitems = RMMSummaryHelper.GetOpenworkitems(context, chkIncludeClosed.Checked);
                }
            }

            //filter data based on closed check
            if (!false) //if (!chkIncludeClosed.Checked)
            {
                // Filter by Open Tickets.
                List<string> LstClosedTicketIds = new List<string>();
                //get closed ticket instead of open ticket and then filter all except closed ticket
                DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(context);
                if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
                {
                    LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
                }
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow[] dr = dtResult.AsEnumerable().Where(x => !LstClosedTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId), StringComparer.OrdinalIgnoreCase)).ToArray();
                    if (dr != null && dr.Length > 0)
                        dtResult = dr.CopyToDataTable();
                    else
                        dtResult.Rows.Clear();
                }
            }

            DataTable dtAllocationMonthly = null;
            DataTable dtAllocationWeekly = null;

            if (hdndisplayMode == "Weekly") //if (hdndisplayMode == "Weekly")
            {
                dtAllocationWeekly = LoadAllocationWeeklySummaryView();
            }
            else
                dtAllocationMonthly = LoadAllocationMonthlyView();

            if (dtResult == null)
                return data;

            ILookup<object, DataRow> dtAllocLookups = null;
            ILookup<object, DataRow> dtWeeklyLookups = null;
            //Grouping on AllocationMonthly datatable based on ResourceWorkItemLookup
            if (dtAllocationMonthly != null && dtAllocationMonthly.Rows.Count > 0)
                dtAllocLookups = dtAllocationMonthly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.ResourceWorkItemLookup]);
            //Grouping on AllocationWeekly datatable based on WorkItemID
            if (dtAllocationWeekly != null && dtAllocationWeekly.Rows.Count > 0)
                dtWeeklyLookups = dtAllocationWeekly.AsEnumerable().ToLookup(x => x[DatabaseObjects.Columns.WorkItemID]);

            Dictionary<string, DataRow> tempTicketCollection = new Dictionary<string, DataRow>();
            #region data creating
            UserProfile userDetails = null;

            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = null;
            DataTable dtResouceWiseTotals = data.Clone();
            DataRow newRow = null;
            DataRow newTotalRow = null;
            bool isTotalRowNew = false;
            DataRow[] drArray = null;
            //GetUsersProfile()
            List<string> lstSelectedDepartment = UGITUtility.ConvertStringToList(selecteddepartment, Constants.Separator6);
            List<string> departmentTempUserIds = lstUProfile.Where(a => lstSelectedDepartment.Exists(d => d == a.Department)).Select(x => x.Id).ToList();
            List<string> functionalTempUserIds = lstUProfile.Where(a => a.FunctionalArea != null && a.FunctionalArea == selectedfunctionalare).Select(x => x.Id).ToList();
            List<string> managerTempUserIds = lstUProfile.Select(x => x.Id).ToList();
            foreach (DataRow dr in dtResult.Rows)
            {
                string userid = Convert.ToString(dr[DatabaseObjects.Columns.ResourceId]);
                if (string.IsNullOrEmpty(userid))
                    continue;

                userDetails = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(userid));

                if (userDetails == null || !userDetails.Enabled)
                    continue;

                //filter...
                if (!string.IsNullOrEmpty(selecteddepartment))
                {
                    if (departmentTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (selectedfunctionalare > 0)
                {
                    if (functionalTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (!string.IsNullOrEmpty(selectedManager))
                {
                    if (managerTempUserIds.IndexOf(userid) == -1)
                        continue;
                }

                if (string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup])))
                    continue;

                newRow = data.NewRow();
                newRow[DatabaseObjects.Columns.Id] = userid;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;

                //Initial value
                isTotalRowNew = false;

                //Find any existing Totals row or create a new one.
                drArray = dtResouceWiseTotals.Select(string.Format("{0} = '{1}'",DatabaseObjects.Columns.Id, userid));
                if (drArray.Length > 0)
                {
                    newTotalRow = drArray[0];
                }
                else
                {
                    isTotalRowNew = true;
                    newTotalRow = dtResouceWiseTotals.NewRow();
                    newTotalRow[DatabaseObjects.Columns.Id] = userid;
                    newTotalRow[DatabaseObjects.Columns.ItemOrder] = 1;
                    newTotalRow[DatabaseObjects.Columns.Title] = "Total";
                    newTotalRow[DatabaseObjects.Columns.Project] = "Total";
                    newTotalRow[DatabaseObjects.Columns.ModuleName] = "GroupTotalRow";
                }

                if (btnexport)
                {
                    newRow[DatabaseObjects.Columns.Resource] = userDetails.Name;
                }
                else
                {
                    newRow[DatabaseObjects.Columns.Resource] = Convert.ToString(dr[DatabaseObjects.Columns.Resource]);
                }
                newRow[DatabaseObjects.Columns.Name] = userDetails.Name;

                newTotalRow[DatabaseObjects.Columns.Resource] = newRow[DatabaseObjects.Columns.Resource];
                newTotalRow[DatabaseObjects.Columns.Name] = userDetails.Name;

                DataRow drWorkItem = null;
                if (workitems != null && workitems.Rows.Count > 0)
                {
                    string workitemid = Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]);
                    DataRow[] filterworkitemrow = workitems.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, UGITUtility.GetLookupID(Convert.ToString(dr[DatabaseObjects.Columns.ResourceWorkItemLookup]))));
                    if (filterworkitemrow != null && filterworkitemrow.Length > 0)
                        drWorkItem = filterworkitemrow[0];
                }

                if (drWorkItem != null && drWorkItem[DatabaseObjects.Columns.WorkItem] != null)
                {
                    string workItem = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItem]);
                    string[] arrayModule = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    string moduleName = string.Empty;
                    if (UGITUtility.IsValidTicketID(workItem))
                        moduleName = uHelper.getModuleNameByTicketId(workItem);

                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        if (arrayModule.Contains(moduleName) || arrayModule.Length == 0)
                        {

                            module = moduleManager.GetByName(moduleName);
                            if (module == null)
                                continue;

                            //check for active modules.
                            if (!UGITUtility.StringToBoolean(module.EnableRMMAllocation))
                                continue;
                            DataRow dataRow = null;
                            if (tempTicketCollection.ContainsKey(workItem))
                                dataRow = tempTicketCollection[workItem];
                            else
                            {
                                dataRow = ticketManager.GetByTicketID(module, workItem, viewFields: new List<string>() { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Closed });
                                if (dataRow != null)
                                {
                                    tempTicketCollection.Add(workItem, dataRow);
                                }
                            }


                            if (dataRow != null)
                            {
                                string ticketID = workItem;
                                string title = UGITUtility.TruncateWithEllipsis(Convert.ToString(dataRow[DatabaseObjects.Columns.Title]), 50);
                                if (!string.IsNullOrEmpty(ticketID))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0}: {1}", ticketID, title);
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0}: {1}", ticketID, title);
                                    if (UGITUtility.StringToBoolean(dataRow["Closed"]))
                                    {
                                        newRow[DatabaseObjects.Columns.Closed] = true;
                                    }
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = title;// title;
                                    newRow[DatabaseObjects.Columns.Project] = title;
                                }
                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);

                                //condition for add new column for breakup gantt chart...
                                if (data != null && data.Rows.Count > 0)
                                {
                                    string expression = $"{DatabaseObjects.Columns.TicketId}='{workItem}' AND {DatabaseObjects.Columns.Id}='{userid}'";
                                    DataRow[] row = data.Select(expression);

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr[DatabaseObjects.Columns.PlannedStartDate];
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow["ExtendedDateAssign"] = dr[DatabaseObjects.Columns.PlannedStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.PlannedEndDate];

                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])) && Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate]) != "01-01-1753 00:00:00")
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];

                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}'>{1}: {2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40));

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;


                                    if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)//if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, newRow, dttemp, newTotalRow);
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                                ViewTypeAllocation(data, newRow, dttemp, newTotalRow);
                                        }
                                    }

                                    data.Rows.Add(newRow);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.PlannedEndDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr[DatabaseObjects.Columns.PlannedStartDate];
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow["ExtendedDateAssign"] = dr[DatabaseObjects.Columns.PlannedStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.PlannedEndDate];
                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctPlannedAllocation]);
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}'>{1}: {2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40));

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;

                                    if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, newRow, dttemp, newTotalRow);
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                ViewTypeAllocation(data, newRow, dttemp, newTotalRow);
                                            }
                                        }
                                    }
                                    data.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (arrayModule.Length > 0 && !arrayModule.Contains(Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType])))
                            continue;

                        if (data != null && data.Rows.Count > 0)
                        {
                            string expression = string.Format("{0}= '{1}' AND {2}='{3}' AND {4}='{5}'", DatabaseObjects.Columns.TicketId, workItem, DatabaseObjects.Columns.Id, userid, DatabaseObjects.Columns.SubWorkItem, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            DataRow[] row = data.Select(expression);

                            if (row != null && row.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                {
                                    row[0]["ExtendedDate"] = Convert.ToString(row[0]["ExtendedDate"]) + Constants.Separator + dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(dr[DatabaseObjects.Columns.EstStartDate])))
                                        row[0][DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationEndDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(dr[DatabaseObjects.Columns.EstEndDate])))
                                        row[0][DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];
                                }
                                if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(data, row[0], dttemp, newTotalRow);
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(data, row[0], dttemp, newTotalRow, false);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                                if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                    newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                                }

                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow["ProjectNameLink"] = workItem;
                                newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                                newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);

                                if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                                {
                                    newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                    newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];

                                    newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                    newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                                }

                                if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(data, newRow, dttemp, newTotalRow);
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();

                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(data, newRow, dttemp, newTotalRow, false);
                                        }
                                    }
                                }
                                data.Rows.Add(newRow);
                            }
                        }
                        else
                        {
                            string title = UGITUtility.TruncateWithEllipsis(workItem, 50);
                            if (string.IsNullOrEmpty(Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem])))
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title);
                            }
                            else
                            {
                                newRow[DatabaseObjects.Columns.Title] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));// title;
                                newRow[DatabaseObjects.Columns.Project] = string.Format("{0} > {1} > {2}", Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]), title, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            }

                            newRow[DatabaseObjects.Columns.TicketId] = workItem;
                            newRow[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType]);
                            newRow[DatabaseObjects.Columns.SubWorkItem] = Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]);
                            newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToInt(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr[DatabaseObjects.Columns.Id]);
                            newRow["ProjectNameLink"] = workItem;

                            if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.EstEndDate])))
                            {
                                newRow[DatabaseObjects.Columns.AllocationStartDate] = dr[DatabaseObjects.Columns.EstStartDate];
                                newRow[DatabaseObjects.Columns.AllocationEndDate] = dr[DatabaseObjects.Columns.EstEndDate];

                                newRow["ExtendedDate"] = dr[DatabaseObjects.Columns.EstStartDate] + Constants.Separator1 + dr[DatabaseObjects.Columns.EstEndDate];
                                newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr[DatabaseObjects.Columns.PctEstimatedAllocation]);
                            }

                            if (hdndisplayMode == "Weekly" && dtWeeklyLookups != null)
                            {
                                DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                if (dttemp != null && dttemp.Length > 0)
                                    ViewTypeAllocation(data, newRow, dttemp, newTotalRow);
                            }
                            else
                            {
                                if (dtAllocLookups != null)
                                {
                                    DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                    {
                                        ViewTypeAllocation(data, newRow, dttemp, newTotalRow, false);

                                    }
                                }
                            }

                            data.Rows.Add(newRow);
                        }
                    }

                    if(!string.IsNullOrEmpty(newRow[DatabaseObjects.Columns.AllocationStartDate].ToString()) && (string.IsNullOrEmpty(newTotalRow[DatabaseObjects.Columns.AllocationStartDate].ToString()) || 
                        Convert.ToDateTime(newTotalRow[DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    { 
                        newTotalRow[DatabaseObjects.Columns.AllocationStartDate] = newRow[DatabaseObjects.Columns.AllocationStartDate]; //Capture minimum start date
                    }
                    if (!string.IsNullOrEmpty(newRow[DatabaseObjects.Columns.AllocationEndDate].ToString()) && (string.IsNullOrEmpty(newTotalRow[DatabaseObjects.Columns.AllocationEndDate].ToString()) ||
                        Convert.ToDateTime(newTotalRow[DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate])))
                    {
                        newTotalRow[DatabaseObjects.Columns.AllocationEndDate] = newRow[DatabaseObjects.Columns.AllocationEndDate];
                    }
                    //newTotalRow[DatabaseObjects.Columns.PlannedStartDate] = newRow[DatabaseObjects.Columns.PlannedStartDate];
                    //newTotalRow[DatabaseObjects.Columns.PlannedEndDate] = newRow[DatabaseObjects.Columns.PlannedEndDate];

                    if (isTotalRowNew)
                        dtResouceWiseTotals.Rows.Add(newTotalRow);
                    else
                    {
                        drArray = data.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Id, userid));
                        if (drArray.Length > 1)
                        {
                            newTotalRow[DatabaseObjects.Columns.ItemOrder] = drArray.Length;
                        }
                    }

                }

            }
            #endregion
            //data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC", DatabaseObjects.Columns.Name, DatabaseObjects.Columns.Project);
            //data = data.DefaultView.ToTable();
            DataView dvTotals = new DataView(dtResouceWiseTotals); 
            dvTotals.RowFilter = DatabaseObjects.Columns.ItemOrder + " > 1";
            if(dtResouceWiseTotals != null)
                data.Merge(dvTotals.ToTable());
            data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC, {2} ASC", DatabaseObjects.Columns.Name, DatabaseObjects.Columns.Closed, DatabaseObjects.Columns.Project);
            return data;
        }

        private DataTable GetAllocationData()
        {
            SelectedUser = "";
            selectedCategory = "";
            selectedManager = "0";
            selectedfunctionalare = 0;
            if (!string.IsNullOrEmpty(ddlDepartment.GetValues()))
            {
                selecteddepartment = ddlDepartment.GetValues();
            }
            else if (!string.IsNullOrEmpty(hdnDepartment.Value))
            {
                selecteddepartment = hdnDepartment.Value;
            }

            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                dateFrom = uHelper.FirstDayOfMonth(DateTime.Now);
                dateTo = dateFrom.AddMonths(12);
            }
            DataTable data = null;
            data = RMMSummaryHelper.GetAllocationData(context, dateFrom, dateTo, hdndisplayMode, selectedCategory, selecteddepartment, SelectedUser, selectedManager,
                selectedfunctionalare, viewself, hdnYear, CurrentUser, chkIncludeClosed, btnexport, true);
            return data;
        }

        private DataTable LoadAllocationMonthlyView()
        {
            try
            {
                string ModuleNames = "CPR,OPM,CNS,PMM,NPR";
                DataTable dtAllocationMonthWise = null;
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                values.Add("@ModuleNames", ModuleNames);
                values.Add("@Fromdate", Convert.ToDateTime(dateFrom));
                values.Add("@Todate", Convert.ToDateTime(dateTo));
                values.Add("@Isclosed", chkIncludeClosed);
                dtAllocationMonthWise = uGITDAL.ExecuteDataSetWithParameters("usp_GetAllocationdata", values);

                //Below code commented due to performance
                //ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
                //List<ResourceAllocationMonthly> allocMonthly = new List<ResourceAllocationMonthly>();
                //if (chkIncludeClosed.Checked == true)
                //    allocMonthly = allocationMonthlyManager.Load(x => x.MonthStartDate.Value.Date >= Convert.ToDateTime(dateFrom).Date && x.MonthStartDate.Value.Date <= Convert.ToDateTime(dateTo));
                //else
                //    allocMonthly = allocationMonthlyManager.LoadOpenItems(dateFrom, dateTo);
                //dtAllocationMonthWise = UGITUtility.ToDataTable<ResourceAllocationMonthly>(allocMonthly);
                return dtAllocationMonthWise;
            }
            catch (Exception)
            { }
            return null;
        }
        private DataTable LoadAllocationWeeklySummaryView()
        {
            try
            {
                DateTime dtFrom = dateFrom;
                DateTime dtTo = dateTo;

                dtFrom = new DateTime(dateFrom.Year, DateTime.Today.Month, dateFrom.Day);
                dtTo = new DateTime(dateFrom.Year, DateTime.Today.Month, DateTime.DaysInMonth(dateFrom.Year, DateTime.Today.Month));

                string commQuery = string.Empty;
                ResourceUsageSummaryWeekWiseManager allocationWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
                commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.WeekStartDate, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo));

                DataTable dtAllocationWeekWise = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'" + "AND " + commQuery);
                return dtAllocationWeekWise;
            }
            catch (Exception)
            { }
            return null;
        }

        private void ViewTypeAllocation(DataTable data, DataRow newRow, DataRow[] dttemp, DataRow newTotalRow, bool Assigned = true)
        {
            double yearquaAllocE = 0;
            double yearquaAllocA = 0;

            double halfyearquaAllocE1 = 0;
            double halfyearquaAllocE2 = 0;
            double halfyearquaAllocA1 = 0;
            double halfyearquaAllocA2 = 0;

            double quaterquaAllocE1 = 0;
            double quaterquaAllocE2 = 0;
            double quaterquaAllocE3 = 0;
            double quaterquaAllocE4 = 0;
            double quaterquaAllocA1 = 0;
            double quaterquaAllocA2 = 0;
            double quaterquaAllocA3 = 0;
            double quaterquaAllocA4 = 0;

            foreach (DataRow rowitem in dttemp)
            {
                if (hdndisplayMode == "Yearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hdnYear))
                    {
                        yearquaAllocE += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 12;
                        yearquaAllocA += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 12;
                    }

                    DateTime yearColumn = new DateTime(UGITUtility.StringToInt(hdnYear), 1, 1);
                    if (data.Columns.Contains((yearColumn).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(yearColumn).ToString("MMM-dd-yy") + "E"] = Math.Round(yearquaAllocE, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((yearColumn).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(yearColumn).ToString("MMM-dd-yy") + "A"] = Math.Round(yearquaAllocA, 2);
                        }
                    }
                }
                else if (hdndisplayMode == "HalfYearly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hdnYear))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            halfyearquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 6;
                            halfyearquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 6;
                        }

                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6)
                        {
                            halfyearquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 6;
                            halfyearquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 6;
                        }
                    }

                    DateTime halfyearColumn1 = new DateTime(UGITUtility.StringToInt(hdnYear), 1, 1);
                    DateTime halfyearColumn2 = new DateTime(UGITUtility.StringToInt(hdnYear), 7, 1);
                    if (data.Columns.Contains((halfyearColumn1).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(halfyearColumn1).ToString("MMM-dd-yy") + "E"] = Math.Round(halfyearquaAllocE1, 2);
                    }

                    if (data.Columns.Contains((halfyearColumn2).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(halfyearColumn2).ToString("MMM-dd-yy") + "E"] = Math.Round(halfyearquaAllocE2, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((halfyearColumn1).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(halfyearColumn1).ToString("MMM-dd-yy") + "A"] = Math.Round(halfyearquaAllocA1, 2);
                        }

                        if (data.Columns.Contains((halfyearColumn2).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(halfyearColumn2).ToString("MMM-dd-yy") + "A"] = Math.Round(halfyearquaAllocA2, 2);
                        }
                    }

                }
                else if (hdndisplayMode == "Quarterly")
                {
                    if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Year == UGITUtility.StringToInt(hdnYear))
                    {
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 4)
                        {
                            quaterquaAllocE1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA1 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 3 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 7)
                        {
                            quaterquaAllocE2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA2 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month > 6 && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).Month < 10)
                        {
                            quaterquaAllocE3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA3 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                        else
                        {
                            quaterquaAllocE4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]) / 3;
                            quaterquaAllocA4 += UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]) / 3;
                        }
                    }

                    DateTime quaterColumn1 = new DateTime(UGITUtility.StringToInt(hdnYear), 1, 1);
                    DateTime quaterColumn2 = new DateTime(UGITUtility.StringToInt(hdnYear), 4, 1);
                    DateTime quaterColumn3 = new DateTime(UGITUtility.StringToInt(hdnYear), 7, 1);
                    DateTime quaterColumn4 = new DateTime(UGITUtility.StringToInt(hdnYear), 10, 1);

                    if (data.Columns.Contains((quaterColumn1).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn1).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE1, 2);
                    }

                    if (data.Columns.Contains((quaterColumn2).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn2).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE2, 2);
                    }

                    if (data.Columns.Contains((quaterColumn3).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn3).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE3, 2);
                    }

                    if (data.Columns.Contains((quaterColumn4).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[(quaterColumn4).ToString("MMM-dd-yy") + "E"] = Math.Round(quaterquaAllocE4, 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains((quaterColumn1).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn1).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA1, 2);
                        }

                        if (data.Columns.Contains((quaterColumn2).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn2).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA2, 2);
                        }

                        if (data.Columns.Contains((quaterColumn3).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn3).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA3, 2);
                        }

                        if (data.Columns.Contains((quaterColumn4).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[(quaterColumn4).ToString("MMM-dd-yy") + "A"] = Math.Round(quaterquaAllocA4, 2);
                        }
                    }

                }
                else if (hdndisplayMode == "Weekly")
                {
                    if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                    }

                    if (Assigned)
                    {
                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "A"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]), 2);
                        }
                    }
                }
                else
                {
                    DateTime rowitemMonthStartDate = Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]);
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        DateTime AllocationStartDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate]);
                        DateTime AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                        DateTime allocationEndDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate]);
                        if (rowitemMonthStartDate >= AllocationMonthStartDate && rowitemMonthStartDate <= allocationEndDate)
                        {
                            if (data.Columns.Contains(rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"))
                            {
                                newRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                                newTotalRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"] = UGITUtility.StringToDouble(newTotalRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "E"]) + Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctAllocation]), 2);
                            }
                        }
                    }
                    if (Assigned)
                    {
                        if (data.Columns.Contains(rowitemMonthStartDate.ToString("MMM-dd-yy") + "A") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.PlannedStartDate])))
                        {
                            newRow[rowitemMonthStartDate.ToString("MMM-dd-yy") + "A"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.PctPlannedAllocation]), 2);
                        }
                    }

                }
            }

        }

        protected void gvPreview_DataBinding(object sender, EventArgs e)
        {
            string dept = hdnaspDepartment; //hdnaspDepartment.Value; 
            if (dept.EqualsIgnoreCase("undefined"))
            {
                dept = string.Empty;
            }

            //LoadDdlResourceManager(dept, ddlResourceManager.SelectedItem?.Value);

            if (allocationData == null)
            {
                allocationData = GetAllocationData();

                //if (!stopToRegerateColumns)
                //    PrepareAllocationGrid();
            }
            //PrepareAllocationGrid();
            gvPreview.DataSource = allocationData;
        }

        // Variables that store summary values.  
        DateTime dtstartDate;
        DateTime dtEndDate;
        double ResourceFTE;
        double ResourceTotalFTE;

        protected void gvPreview_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            #region group summary
            if (e.IsGroupSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    dtstartDate = DateTime.MinValue;
                    dtEndDate = DateTime.MinValue;

                }

                // Calculation. 
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == "AllocationStartDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("AllocationStartDate"))))
                    {
                        if (dtstartDate == DateTime.MinValue)
                            dtstartDate = Convert.ToDateTime(e.FieldValue);
                        else if (Convert.ToDateTime(e.GetValue("AllocationStartDate")) < dtstartDate)
                        {
                            dtstartDate = Convert.ToDateTime(e.FieldValue);
                        }
                    }

                    if (item.FieldName == "AllocationStartDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("PlannedStartDate"))))
                    {
                        if (dtstartDate == DateTime.MinValue)
                            dtstartDate = Convert.ToDateTime(e.GetValue("PlannedStartDate"));
                        else if (Convert.ToDateTime(e.GetValue("PlannedStartDate")) < dtstartDate)
                        {
                            dtstartDate = Convert.ToDateTime(e.GetValue("PlannedStartDate"));
                        }
                    }


                    if (item.FieldName == "AllocationEndDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("AllocationEndDate"))))
                    {
                        if (dtEndDate == DateTime.MinValue)
                            dtEndDate = Convert.ToDateTime(e.FieldValue);
                        else if (Convert.ToDateTime(e.GetValue("AllocationEndDate")) > dtEndDate)
                        {
                            dtEndDate = Convert.ToDateTime(e.FieldValue);
                        }
                    }

                    if (item.FieldName == "AllocationEndDate" && !string.IsNullOrEmpty(Convert.ToString(e.GetValue("PlannedEndDate"))))
                    {
                        if (dtEndDate == DateTime.MinValue)
                            dtEndDate = Convert.ToDateTime(e.GetValue("PlannedEndDate"));
                        else if (Convert.ToDateTime(e.GetValue("PlannedEndDate")) > dtEndDate)
                        {
                            dtEndDate = Convert.ToDateTime(e.GetValue("PlannedEndDate"));
                        }
                    }
                }
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {

                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == "AllocationStartDate")
                    {
                        if (dtstartDate != DateTime.MinValue)
                            e.TotalValue = dtstartDate.ToString("MMM-dd-yyyy");
                        else
                            e.TotalValue = "";

                    }
                    if (item.FieldName == "AllocationEndDate")
                    {
                        if (dtEndDate != DateTime.MinValue)
                            e.TotalValue = dtEndDate.ToString("MMM-dd-yyyy");
                        else
                            e.TotalValue = "";
                    }
                }
            }
            #endregion

            #region total summary
            if (e.IsTotalSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    ResourceFTE = 0.0;
                    ResourceTotalFTE = 0.0;
                }

                // Calculation. 
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType" && item.FieldName != DatabaseObjects.Columns.AllocationStartDate && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "" && ((System.Data.DataRowView)e.Row).Row.ItemArray[18].ToString() != "GroupTotalRow")
                            ResourceFTE += UGITUtility.StringToDouble(Convert.ToString(e.FieldValue));
                    }

                }
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == DatabaseObjects.Columns.Title)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceItem")
                            e.TotalValue = "Total Capacity (FTE)";
                        else
                            e.TotalValue = "Allocated Demand (FTE)";
                    }

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType" && item.FieldName != DatabaseObjects.Columns.AllocationStartDate && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {

                        List<UserProfile> lstUProfile;
                        lstUProfile = ObjUserProfileManager.GetUsersProfile();  /// uGITCache.UserProfileCache.GetAllUsers(SPContext.Current.Web);


                        foreach (UserProfile userProfile in lstUProfile)
                        {
                            if (!userProfile.Enabled)
                                continue;

                            //filter code.. for dropdowns.
                            //if (divFilter.Visible)
                            //{
                            //    if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "filterdeptRA")))
                            //    {
                            //        if (userProfile.Department != Convert.ToString(UGITUtility.GetCookieValue(Request, "filterdeptRA")))
                            //            continue;
                            //    }

                            //}

                            if (userProfile.UGITStartDate < UGITUtility.StringToDateTime(item.FieldName.Remove(item.FieldName.Length - 1)) && userProfile.UGITEndDate > UGITUtility.StringToDateTime(item.FieldName.Remove(item.FieldName.Length - 1)))
                                ResourceTotalFTE++;

                        }

                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "TFTE")
                        {
                            e.TotalValue = Math.Round(ResourceTotalFTE, 2);
                        }
                        else
                        {
                            e.TotalValue = Math.Round(ResourceFTE / 100, 2);
                        }
                    }
                }
            }
            #endregion
        }

        private int GetDaysForDisplayMode(string dMode, DateTime dt)
        {
            int days = 30;
            switch (dMode)
            {
                case "Daily":
                    days = 1;
                    break;
                case "Weekly":
                    {
                        if (dt.ToString("ddd") == "Mon")
                            days = 7;
                        else if (dt.ToString("ddd") == "Tue")
                            days = 6;
                        else if (dt.ToString("ddd") == "Wed")
                            days = 5;
                        else if (dt.ToString("ddd") == "Thu")
                            days = 4;
                        else if (dt.ToString("ddd") == "Fri")
                            days = 3;
                        else if (dt.ToString("ddd") == "Sat")
                            days = 2;
                        else if (dt.ToString("ddd") == "Sun")
                            days = 1;

                        break;
                    }
                case "Monthly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(1)) - dt).Days;
                    break;
                case "Quarterly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(3)) - dt).Days;
                    break;
                case "HalfYearly":
                    days = (uHelper.FirstDayOfMonth(dt.AddMonths(6)) - dt).Days;
                    break;
                case "Yearly":
                    days = 365;
                    break;
                default:
                    break;
            }
            return days;
        }
        private void PrepareAllocationGrid()
        {
            //gvPreview.Columns.Clear();
            //gvPreview.GroupSummary.Clear();
            //gvPreview.TotalSummary.Clear();

            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Resource;
            colId.Caption = DatabaseObjects.Columns.Resource;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("200px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.GroupIndex = 0;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Name;
            colId.Caption = DatabaseObjects.Columns.Name;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.Visible = false;
            colId.Settings.SortMode = ColumnSortMode.Custom;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Id;
            colId.Caption = DatabaseObjects.Columns.Id;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("30px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            colId.Visible = false;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Title;
            //colId.Caption = DatabaseObjects.Columns.Title;
            colId.Caption = "Work Item";
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("180px");
            colId.ExportWidth = 400;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            gvPreview.Columns.Add(colId);

            gvPreview.TotalSummary.Add(SummaryItemType.Custom, DatabaseObjects.Columns.Title);

            var IsShowTotalCapicityFTE = ConfigVariableMGR.GetValueAsBool(ConfigConstants.ShowTotalCapicityFTE);
            if (IsShowTotalCapicityFTE)
            {
                ASPxSummaryItem item = new ASPxSummaryItem(DatabaseObjects.Columns.Title, SummaryItemType.Custom);
                item.Tag = "ResourceItem";

                gvPreview.TotalSummary.Add(item);
            }

            if (!HideAllocationType)
            {
                colId = new GridViewDataTextColumn();
                colId.FieldName = "AllocationType";
                colId.Caption = "Allocation Type";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                colId.Width = new Unit("150px");
                gvPreview.Columns.Add(colId);
            }

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.AllocationStartDate;
            colId.Caption = "Start Date";
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Width = new Unit("100px");
            colId.ExportWidth = 90;
            colId.GroupFooterCellStyle.Font.Bold = true;
            gvPreview.Columns.Add(colId);

            CreateGridSummaryColumn(gvPreview, DatabaseObjects.Columns.AllocationStartDate);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.AllocationEndDate;
            colId.Caption = "End Date";
            colId.UnboundType = DevExpress.Data.UnboundColumnType.String;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.GroupFooterCellStyle.Font.Bold = true;
            colId.Width = new Unit("100px");
            colId.ExportWidth = 90;
            gvPreview.Columns.Add(colId);

            CreateGridSummaryColumn(gvPreview, DatabaseObjects.Columns.AllocationEndDate);

            GridViewBandColumn bdCol = new GridViewBandColumn();
            string currentDate = string.Empty;

            if (hdndisplayMode == "Weekly")
            {
                dateFrom = new DateTime(dateFrom.Year, DateTime.Today.Month, dateFrom.Day);
                dateTo = new DateTime(dateFrom.Year, DateTime.Today.Month, DateTime.DaysInMonth(dateFrom.Year, DateTime.Today.Month));
            }
            for (DateTime dt = dateFrom; dateTo > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode, dt)))
            {
                if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                {
                    gvPreview.Columns.Add(bdCol);
                    bdCol = new GridViewBandColumn();
                }

                if (dt.ToString("yyyy") != currentDate)
                {
                    bdCol.Caption = dt.ToString("yyyy");
                    bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    bdCol.HeaderStyle.Font.Bold = true;
                    currentDate = dt.ToString("yyyy");
                }

                GridViewDataSpinEditColumn ColIdData = new GridViewDataSpinEditColumn();
                if (hdndisplayMode == "Weekly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("dd-MMM");
                }
                else if (hdndisplayMode == "Quarterly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode == "HalfYearly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode == "Yearly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                ColIdData.CellStyle.CssClass = "timeline-td";
                ColIdData.UnboundType = DevExpress.Data.UnboundColumnType.String;
                ColIdData.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                ColIdData.HeaderStyle.Font.Bold = true;

                ColIdData.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.PropertiesSpinEdit.NumberType = SpinEditNumberType.Float;
                ColIdData.PropertiesSpinEdit.MinValue = 0;
                ColIdData.PropertiesSpinEdit.MaxValue = 999;
                ColIdData.PropertiesSpinEdit.SpinButtons.ShowIncrementButtons = false;

                if (hdndisplayMode == "Monthly")
                {
                    ColIdData.Width = new Unit("60px");
                    ColIdData.ExportWidth = 38;
                }

                if (hdndisplayMode == "Weekly")
                {
                    ColIdData.Width = new Unit("90px");
                    ColIdData.ExportWidth = 60;
                }

                CreateGridSummaryColumn(gvPreview, dt.ToString("MMM-dd-yy") + "E");

                bdCol.Columns.Add(ColIdData);

                ASPxSummaryItem itemCFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy") + "E", SummaryItemType.Custom);
                itemCFTE.DisplayFormat = "N2";
                gvPreview.TotalSummary.Add(itemCFTE);

                if (IsShowTotalCapicityFTE)
                {
                    ASPxSummaryItem itemTFTE = new ASPxSummaryItem(dt.ToString("MMM-dd-yy") + "E", SummaryItemType.Custom);
                    itemTFTE.Tag = "TFTE";
                    itemTFTE.DisplayFormat = "N2";
                    gvPreview.TotalSummary.Add(itemTFTE);
                }
            }

            gvPreview.Columns.Add(bdCol);

        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlDepartment.GetValues()) && string.IsNullOrEmpty(ddlDepartment.dropBox.Text) && hdnDepartment.Value == "")
            {
                lblErrorDepartmentReqrd.Visible = true;
                return;
            }
            ///Set default dates in case of no selection
            if (dtFromDate.Date == DateTime.MinValue || dtFromDate.Date == null)
                dtFromDate.Date = uHelper.FirstDayOfMonth(DateTime.Now);

            if (dtToDate.Date == DateTime.MinValue || dtToDate == null)
                dtToDate.Date = uHelper.LastDayOfMonth(dtFromDate.Date.AddMonths(12));
            //Assign the dates
            dateFrom = dtFromDate.Date;
            dateTo = dtToDate.Date;

            string pageHeaderInfo;
            string deptDetailInfo;
            string deptShortInfo;

            if (ddlDepartment.dropBox.Text.Contains(",") || ddlDepartment.dropBox.Text == "<Various>")
            {
                deptShortInfo = "Various Departments";
                deptDetailInfo = RMMSummaryHelper.GetSelectedDepartmentsInfo(context, hdnDepartment.Value, enableDivision);
            }
            else if (ddlDepartment.dropBox.Text == "All")
            {
                deptShortInfo = "All Departments";
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            else
            {
                deptShortInfo = ddlDepartment.dropBox.Text;
                deptDetailInfo = ddlDepartment.dropBox.Text;
            }
            pageHeaderInfo = string.Format("Resource Allocation: {0}; {1} to {2}", deptShortInfo, uHelper.GetDateStringInFormat(context, dateFrom, false), uHelper.GetDateStringInFormat(context, dateTo, false));
            string reportFooterInfo = string.Format("\nSelection Criteria\n   {0}: {1}\n   {2}: {3} to {4}", "Departments", deptDetailInfo, "Date Range", uHelper.GetDateStringInFormat(context, dateFrom, false), uHelper.GetDateStringInFormat(context, dateTo, false));

            //If date is greater than 1 then change To date to 1st of next month to show data for whole month because it is Monthly view.
            if (dateTo.Day > 1)
                dateTo = uHelper.FirstDayOfMonth(dtToDate.Date.AddMonths(1));
            dateFrom = uHelper.FirstDayOfMonth(dtFromDate.Date);

            PrepareAllocationGrid();
            gvPreview.Columns["AllocationType"].Visible = false; 
            gridExporter.Landscape = true;
            gridExporter.PaperKind = System.Drawing.Printing.PaperKind.A4;
            gridExporter.LeftMargin = 1;
            gridExporter.RightMargin = 1;
            gridExporter.TopMargin = -1;
            gridExporter.BottomMargin = -1;

            gridExporter.PageHeader.Font.Size = 11;
            gridExporter.PageHeader.Font.Name = "Arial";
            gridExporter.PageHeader.Center = pageHeaderInfo;

            gridExporter.PageFooter.Center = "Page [Page # of Pages #]";
            gridExporter.PageFooter.Left = "[Date Printed]";
            gridExporter.ReportFooter = reportFooterInfo;
            gvPreview.DataBind();
            gvPreview.Templates.GroupRowContent = new GridGroupRowContentTemplate(userEditPermisionList, isResourceAdmin, isPercentageMode, chkIncludeClosed);
            gridExporter.WritePdfToResponse("Resource Allocation");
        }

        protected void gridExporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Header)
                return;
            GridViewDataColumn dataColumn = e.Column as GridViewDataColumn;
            string Estimatecolor = "#24b6fe";
            string moduleName = "";
            rbtnFTE = false;
            List<SummaryResourceProjectComplexity> rComplexity = new List<SummaryResourceProjectComplexity>();
            if (e.RowType == GridViewRowType.Group)
            {
                user = ObjUserProfileManager.GetUserInfoByIdOrName(e.Text);

                if (string.IsNullOrEmpty(selectedCategory))
                    rComplexity = cpxManager.Load(x => x.UserId.EqualsIgnoreCase(user.Id));
                else
                    rComplexity = cpxManager.Load(x => x.UserId.EqualsIgnoreCase(user.Id) && selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList().Any(y => x.ModuleNameLookup.Contains(y)));

                //e.Text = fieldConfigMgr.GetFieldConfigurationData(fieldConfig, e.Text);

                if (rComplexity.Count() > 0)
                {
                    if (chkIncludeClosed == false) //(chkIncludeClosed.Value == false)
                        e.Text = string.Format("{2}\t# Active: {0}/$ Active: {1}", rComplexity.Sum(x => x.Count), UGITUtility.FormatNumber(rComplexity.Sum(x => x.HighProjectCapacity), "currency"), e.Text);
                    else
                        e.Text = string.Format("{2}\t# Lifetime: {0}/$ Lifetime: {1}", rComplexity.Sum(x => x.AllCount), UGITUtility.FormatNumber(rComplexity.Sum(x => x.AllHighProjectCapacity), "currency"), e.Text);
                }

                e.BrickStyle.Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                e.BrickStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4A90E2");
                e.BrickStyle.Sides = DevExPrinting.BorderSide.Bottom;
                e.BrickStyle.BorderColor = System.Drawing.ColorTranslator.FromHtml("#d9dae0");//Grey border lines
                e.BrickStyle.BackColor = System.Drawing.Color.White;
            }

            if (e.RowType == GridViewRowType.Data && dataColumn != null)
            {
                e.BrickStyle.Sides = DevExPrinting.BorderSide.Bottom | DevExPrinting.BorderSide.Top;
                e.BrickStyle.BorderColor = System.Drawing.Color.White;
                e.BrickStyle.BorderWidth = 0;
                e.BrickStyle.BorderStyle = DevExpress.XtraPrinting.BrickBorderStyle.Inset;
                moduleName = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleName));
                if (moduleName == "GroupTotalRow")
                {
                    e.BrickStyle.BackColor = System.Drawing.Color.AliceBlue;
                    e.BrickStyle.BorderColor = System.Drawing.Color.White;
                    e.BrickStyle.BorderWidth = 6;
                }
                if (dataColumn.FieldName == "AllocationStartDate" || dataColumn.FieldName == "AllocationEndDate")
                {
                    if (string.IsNullOrEmpty(e.Text.ToString()))
                        e.Text = "01/01/1900";
                    e.Text = string.Format("{0:MMM dd, yyyy}", Convert.ToDateTime(e.Text));
                }
                if (dataColumn.FieldName != DatabaseObjects.Columns.Resource && dataColumn.FieldName != "AllocationType" && dataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && dataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate && dataColumn.FieldName != DatabaseObjects.Columns.Title)
                {
                    DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                    DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
					string foreColor = "#000000";
                    for (DateTime dt = dateFrom; dateTo > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode, dt)))
                    {
                        if (rbtnFTE) //(rbtnFTE.Checked || rbtnBar.Checked)
                        {

                            if (dataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                            {
                                string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                                string AssignAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A"));

                                string html = string.Empty;
                                if (!string.IsNullOrEmpty(estAlloc))
                                {
                                    // html = estAlloc + "% <br>";
                                    html = string.Format("{0:0.00}", Math.Round(UGITUtility.StringToDouble(estAlloc) / 100, 2));
                                }

                                if (!string.IsNullOrEmpty(AssignAlloc) & plndD != DateTime.MinValue)
                                {
                                    // html += AssignAlloc + "%";
                                    html += string.Format("{0:0.00}", Math.Round(UGITUtility.StringToDouble(AssignAlloc) / 100, 2));
                                }

                                e.Text = html;

                                //if (!string.IsNullOrEmpty(estAlloc) && !estAlloc.Equals("0"))
                                //{
                                //    e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(Estimatecolor);
                                //}
                            }
                        }
                        else
                        {
                            //DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                            //DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                            string estAlloc;
                            if (dataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                            {
                                object estAllocValue = e.GetValue(dt.ToString("MMM-dd-yy") + "E");
                                if (estAllocValue != System.DBNull.Value)
                                    estAlloc  = Math.Ceiling(Convert.ToDecimal(e.GetValue(dt.ToString("MMM-dd-yy") + "E"))).ToString();
                                else
                                    estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                                string AssignAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "A"));

                                string html = string.Empty;
                                if (!string.IsNullOrEmpty(estAlloc) && !estAlloc.Equals("0"))
                                {
                                    html = $"{estAlloc}%";
                                }

                                if (!string.IsNullOrEmpty(AssignAlloc) & plndD != DateTime.MinValue)
                                {
                                    html += $"{AssignAlloc}%";
                                }

                                e.Text = html;
                                //create bars with module specific colours
                                if (moduleName != "GroupTotalRow")
                                {
                                    if (lstEstimateColorsAndFontColors != null)
                                    {
                                        string value = Convert.ToString(lstEstimateColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                                        if (!string.IsNullOrWhiteSpace(value))
                                        {
                                            Estimatecolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                                            foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
                                        }
                                    }
                                    //if (moduleName == "Time Off" || moduleName == "CNS")
                                    //    e.BrickStyle.ForeColor = System.Drawing.Color.White;
                                    e.BrickStyle.ForeColor = ColorTranslator.FromHtml(foreColor);
                                    if (!string.IsNullOrEmpty(estAlloc) && !estAlloc.Equals("0"))
                                    {
                                        e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(Estimatecolor);
                                        e.BrickStyle.BorderColor = System.Drawing.Color.White;
                                        e.BrickStyle.BorderWidth = 6;
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(e.Text) && !e.Text.Contains("%"))
                                {
                                    e.Text = $"{e.Text}%";
                                }
                            }
                        }

                    }
                }
            }
        }

        private void CreateGridSummaryColumn(DevExpress.Web.ASPxGridView gvPreview, string column)
        {
            ASPxSummaryItem summary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Sum);
            summary.ShowInGroupFooterColumn = column;
            summary.DisplayFormat = "{0}"; // "{0:n0}";
            gvPreview.GroupSummary.Add(summary);

            if (column == DatabaseObjects.Columns.AllocationStartDate)
            {
                ASPxSummaryItem summaryStartDate = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
                summaryStartDate.ShowInGroupFooterColumn = column;
                summaryStartDate.DisplayFormat = "{0:MMM-dd-yyyy}";
                gvPreview.GroupSummary.Add(summaryStartDate);
            }
            if (column == DatabaseObjects.Columns.AllocationEndDate)
            {
                ASPxSummaryItem summaryEndDate = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
                summaryEndDate.ShowInGroupFooterColumn = column;
                summaryEndDate.DisplayFormat = "{0:MMM-dd-yyyy}";
                gvPreview.GroupSummary.Add(summaryEndDate);
            }

        }
        public class GridGroupRowContentTemplate : ITemplate
        {
            const string
                MainTableCssClassName = "summaryTable rmmSummary-table",
                VisibleColumnCssClassName = "gridVisibleColumn",
                SummaryTextContainerCssClassName = "summaryTextContainer",
                SummaryCellCssClassNameFormat = "summaryCell_{0}",
                GroupTextFormat = "{0}: {1}";

            List<UserProfile> permisionlist = null;
            bool isAdminResource = false;
            bool isPercentageMode = false;
            bool IncludeClosedProjects = false;

            public GridGroupRowContentTemplate(List<UserProfile> userEditPermisionList, bool isResourceAdmin, bool rbtnPercentage, bool IncludeClosedProjects)
            {
                permisionlist = userEditPermisionList;
                isAdminResource = isResourceAdmin;
                isPercentageMode = rbtnPercentage;
                this.IncludeClosedProjects = IncludeClosedProjects;
            }

            protected GridViewGroupRowTemplateContainer Container { get; set; }
            protected DevExpress.Web.ASPxGridView Grid { get { return Container.Grid; } }

            protected Table MainTable { get; set; }
            protected TableRow GroupTextRow { get; set; }
            protected TableRow SummaryTextRow { get; set; }

            protected int IndentCount { get { return Grid.GroupCount - GroupLevel - 1; } }
            protected int GroupLevel { get { return Grid.DataBoundProxy.GetRowLevel(Container.VisibleIndex); } }
            protected UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            protected ConfigurationVariableManager ConfigVarManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            ResourceProjectComplexityManager cpxManager = new ResourceProjectComplexityManager(HttpContext.Current.GetManagerContext());
            List<SummaryResourceProjectComplexity> rComplexity = new List<SummaryResourceProjectComplexity>();
            UserProfile user = null;
            protected List<GridViewColumn> VisibleColumns
            {
                get
                {
                    List<GridViewColumn> lstCols = new List<GridViewColumn>();
                    foreach (GridViewColumn item in Grid.AllColumns)
                    {
                        if (item.Visible && (item as GridViewBandColumn) == null)
                        {
                            lstCols.Add(item);
                        }
                    }

                    return lstCols.Except(Grid.GetGroupedColumns()).ToList();

                    // return Grid.VisibleColumns.Except(Grid.GetGroupedColumns()).ToList(); 
                }
            }

            public void InstantiateIn(Control container)
            {

                Container = (GridViewGroupRowTemplateContainer)container;
                CreateGroupRowTable();
                Container.Controls.Add(MainTable);

                ApplyStyles();
            }

            protected void CreateGroupRowTable()
            {
                MainTable = new Table();

                GroupTextRow = CreateRow("Group");
                SummaryTextRow = CreateRow("Summary");

                CreateGroupTextCell();
                CreateIndentCells();
                foreach (var column in VisibleColumns)
                    CreateSummaryTextCell(column);
            }

            protected void CreateGroupTextCell()
            {
                var cell = CreateCell(GroupTextRow);
                cell.Text = "";
                cell.ColumnSpan = VisibleColumns.Count + IndentCount;
            }

            protected void CreateSummaryTextCell(GridViewColumn column)
            {
                var cell = CreateCell(SummaryTextRow);

                //if (column.Caption == "Title")
                if (column.Caption == "Work Item")
                {
                    string strCell = string.Empty;
                    //UserProfile user = UserManager.GetUserInfoById(Container.GroupText);
                    //user = UserManager.GetUserInfoById(Container.GroupText);
                    user = UserManager.GetUserInfoByIdOrName(Container.GroupText);

                    isAdminResource = UserManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || UserManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
                    //bool allowAllocationForSelf = ConfigVarManager.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
                    string allowAllocationForSelf = ConfigVarManager.GetValue(ConfigConstants.AllowAllocationForSelf);

                    if (!isAdminResource)
                    {
                        permisionlist = UserManager.LoadAuthorizedUsers(allowAllocationForSelf);
                    }

                    string appendIcons = "";

                    //if (isAdminResource)
                    //{

                    //    if (user != null)
                    //    {
                    //        appendIcons = string.Format("<image style=\"padding-right:7px; width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenAddAllocationPopup('" + user.Id + "', '" + user.Name + "')\"  />");
                    //    }
                    //}
                    //else
                    //{
                    //    if (user != null)
                    //    {
                    //        if (permisionlist != null && permisionlist.Exists(x => x.Id == user.Id))
                    //            appendIcons = string.Format("<image style=\"padding-right:7px;width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenAddAllocationPopup('" + user.Id + "', '" + user.Name + "')\"  />");
                    //    }
                    //}

                    strCell = string.Format("{0} {1}", user.Name, appendIcons);

                    cell.Text = strCell;
                    return;
                }
                else if (column.Caption == "Allocation Type")
                {
                    string strCell = string.Empty;
                    UserProfile user = UserManager.GetUserInfoByIdOrName(Container.GroupText);
                    if (user != null)
                    {
                        //ResourceProjectComplexityManager cpxManager = new ResourceProjectComplexityManager(HttpContext.Current.GetManagerContext());
                        //List<SummaryResourceProjectComplexity> rComplexity = cpxManager.Load(x => x.UserId == user.Id);
                        rComplexity = cpxManager.Load(x => x.UserId == user.Id);
                        if (rComplexity.Count() > 0)
                        {
                            if (IncludeClosedProjects == false)
                                strCell = string.Format("# Active: {0}/$ Active: {1}", rComplexity.Sum(x => x.Count), UGITUtility.FormatNumber(rComplexity.Sum(x => x.HighProjectCapacity), "currency"));
                            else
                                strCell = string.Format("# Lifetime: {0}/$ Lifetime: {1}", rComplexity.Sum(x => x.AllCount), UGITUtility.FormatNumber(rComplexity.Sum(x => x.AllHighProjectCapacity), "currency"));
                        }
                        cell.Text = strCell;
                    }
                    return;
                }

                var dataColumn = column as GridViewDataColumn;
                if (dataColumn == null)
                    return;


                var summaryItems = FindSummaryItems(dataColumn);
                if (summaryItems.Count == 0)
                    return;

                var div = new WebControl(HtmlTextWriterTag.Div) { CssClass = SummaryTextContainerCssClassName };
                cell.Controls.Add(div);

                var text = string.Empty;
                if (dataColumn.FieldName != DatabaseObjects.Columns.Title && dataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && dataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                {
                    var summarytext = GetGroupSummaryText(summaryItems, column);
                    if (!string.IsNullOrEmpty(summarytext))
                    {

                        if (isPercentageMode)
                            text = summarytext + "%";
                        else
                        {
                            text = string.Format("{0:0.00}", Math.Round(UGITUtility.StringToDouble(summarytext) / 100, 2));
                        }
                    }
                }
                else
                {
                    text = GetGroupSummaryText(summaryItems, column);//.Replace("<br />", "");
                }
                div.Controls.Add(new LiteralControl(text));
            }

            protected string GetGroupSummaryText(List<ASPxSummaryItem> items, GridViewColumn column)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    var summaryValue = Grid.GetGroupSummaryValue(Container.VisibleIndex, item);



                    sb.Append(item.GetGroupRowDisplayText(column, summaryValue));
                }
                return sb.ToString();
            }

            protected void ApplyStyles()
            {
                MainTable.CssClass = MainTableCssClassName;
                VisibleColumns[0].HeaderStyle.CssClass = VisibleColumnCssClassName;

                var startIndex = GroupLevel + 1;
                for (var i = 0; i < SummaryTextRow.Cells.Count; i++)
                    SummaryTextRow.Cells[i].CssClass = string.Format(SummaryCellCssClassNameFormat, i + startIndex);
            }

            protected void CreateIndentCells()
            {
                for (var i = 0; i < IndentCount; i++)
                    CreateCell(SummaryTextRow);
            }
            protected List<ASPxSummaryItem> FindSummaryItems(GridViewDataColumn column)
            {
                return Grid.GroupSummary.Where(i => i.FieldName == column.FieldName).ToList();
            }
            protected TableRow CreateRow(string rowtype)
            {
                var row = new TableRow();
                if (rowtype == "Summary")
                    row.CssClass = "SummaryHeaderAdjustment";
                MainTable.Rows.Add(row);
                return row;
            }
            protected TableCell CreateCell(TableRow row)
            {
                var cell = new TableCell();
                row.Cells.Add(cell);
                return cell;
            }
        }





    }
}