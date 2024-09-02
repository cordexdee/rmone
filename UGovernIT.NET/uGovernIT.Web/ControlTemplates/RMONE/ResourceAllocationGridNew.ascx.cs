using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using uGovernIT.Helpers;
using System.Linq;
using DevExpress.Web;
using DevExPrinting = DevExpress.XtraPrinting;
using uGovernIT.Manager.Reports;
//using ReportEntity = uGovernIT.Manager.Report.Entities;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using System.Drawing;
using DevExpress.Web.Rendering;
using System.Globalization;
using System.Collections;
using System.Text;
using uGovernIT.Manager;
using System.Threading;
using DevExpress.XtraGrid;
using DevExpress.Data;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using System.Data.SqlClient;
using uGovernIT.DAL;
using DevExpress.CodeParser;
using uGovernIT.Web.Modules;
using uGovernIT.Util.Log;
using uGovernIT.Util.Cache;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using DevExpress.XtraReports.Native.Templates;
using uGovernIT.Web.ControlTemplates.RMM;
//using static DevExpress.Utils.MVVM.Internal.ILReader;
using System.Text.RegularExpressions;
using DevExpress.XtraScheduler.Native;
using System.Web.Mvc;
using DevExpress.Office.PInvoke;
using uGovernIT.Web.Models;
using uGovernIT.Web.ControlTemplates.RMONE;
using Microsoft.Owin;

namespace uGovernIT.Web
{
    public partial class ResourceAllocationGridNew : UserControl
    {
        public String TicketID { get; set; }
        protected ZoomLevel zoomLevel;
        DataTable ResultData = new DataTable();
        protected bool IsCustomCallBack = false;
        public bool HideTopBar { get; set; }
        public Unit Height { get; set; }
        public Unit Width { get; set; }
        //userall - selectedUsers - selectedYear - includeClosed
        public string UserAll { get; set; }
        public string SelectedUsers { get; set; }
        public string SelectedYear { get; set; }
        public string IncludeClosed { get; set; }
        public bool ShowCurrentUserDetailsOnly { get; set; }
        public bool HideAllocationType { get; set; }
        public string MultiAddUrl { get; set; }
        public bool ShowUserResume { get; set; }
        public const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";

        private DateTime dateFrom;
        private DateTime dateTo;
        public bool btnexport;

        private string selectedCategory = string.Empty;
        private string selectedManager = string.Empty;
        public string SelectedUser = string.Empty;
        private string selecteddepartment = string.Empty;
        public bool RequestFromProjectAllocation = false;
        public bool ShowDateInfo = true;
        public string classNameSt = string.Empty;
        public string classNameEd = string.Empty;
        public string ganttProjSD = string.Empty;
        public string ganttProjED = string.Empty;
        public string ganttProjReqAloc = string.Empty;
        int LeadIndex = 0;
        //private long selectedfunctionalare = -1;
        //List<string> selectedTypes = new List<string>();
        //private string ControlName;
        public string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}";
        public string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=addworkitem&Type=ResourceAllocation&WorkItemType=Time Off");
        public string OpenUserResumeUrl = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}&refreshpage=0";
        //private DataTable dataTable = null;
        ConfigurationVariableManager ConfigVariableMGR = null;
        ConfigurationVariable cvAllocationTimeLineColor;   // = ConfigurationVariable.Load("AllocationTimeLineColor");
        List<string> lstEstimateColors = null;
        List<string> lstEstimateColorsAndFontColors = null;
        List<string> lstAssignColors = null;
        List<string> lstAssignColorsAndFontColors = null;

        protected bool isResourceAdmin = false;
        //private bool allowAllocationForSelf;
        private string allowAllocationForSelf;
        private bool allowAllocationViewAll;
        //private bool viewself = false;
        DataTable dtFilterTypeData;

        protected List<UserProfile> userEditPermisionList = null;
        protected List<UserProfile> userProfiles = null;
        //private List<int> states = null;
        DataTable allocationData = null;
        bool stopToRegerateColumns = false;
        private bool DisablePlannedAllocation;
        protected bool enableDivision;
        private string erpLabel = string.Empty;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private UserProfileManager ObjUserProfileManager = null;
        private UserProfile CurrentUser;
        //private UserProfile user;
        private ResourceAllocationManager ResourceAllocationManager = null;
        private ResourceWorkItemsManager ResourceWorkItemsManager = null;
        //ResourceProjectComplexityManager cpxManager = null;
        //FieldConfigurationManager fieldConfigMgr = null;
        //FieldConfiguration fieldConfig = null;
        //ContactManager ContactManagerMGR = null;
        
        private int MonthColWidth = 60;
        bool useGanttDayFormat = true;

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            ConfigVariableMGR = new ConfigurationVariableManager(context);
            ObjUserProfileManager = new UserProfileManager(context);
            ResourceAllocationManager = new ResourceAllocationManager(context);
            ResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            //cpxManager = new ResourceProjectComplexityManager(context);
            //fieldConfigMgr = new FieldConfigurationManager(context);
            //ContactManagerMGR = new ContactManager(context);
            cvAllocationTimeLineColor = ConfigVariableMGR.LoadVaribale("AllocationTimeLineColor");
            DisablePlannedAllocation = ConfigVariableMGR.GetValueAsBool(ConfigConstants.DisablePlannedAllocation);
            //CurrentUser = HttpContext.Current.CurrentUser();
            userProfiles = ObjUserProfileManager.GetUsersProfile();
            CurrentUser = HttpContext.Current.CurrentUser();// userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(HttpContext.Current.CurrentUser().Id)); 
            enableDivision = ConfigVariableMGR.GetValueAsBool(ConfigConstants.EnableDivision);
            erpLabel = ConfigVariableMGR.GetValue(ConfigConstants.ERPIDLABEL);
            OpenUserResumeUrl = UGITUtility.GetAbsoluteURL(string.Format(OpenUserResumeUrl, "openuserresume", "User Resume", "ResourceAllocation"));
            MultiAddUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "addmultiallocation", "Add Multiple Allocations", SelectedUser, "", IncludeClosed));
            useGanttDayFormat = uHelper.GetGanttFormat(context) == GanttFormat.Days ? true : false;
            if (string.IsNullOrEmpty(Request["DateTo"]) && string.IsNullOrEmpty(Request["DateFrom"]))
            {
                string syear = Request[hndYear.UniqueID];
                if (string.IsNullOrEmpty(syear))
                {
                    hndYear.Value = DateTime.Now.Year.ToString();
                    syear = hndYear.Value;
                }
                lblSelectedYear.Text = syear;
                dateFrom = new DateTime(UGITUtility.StringToInt(syear), 1, 1);
                if (hdndisplayMode.Value == "Weekly")
                    dateTo = new DateTime(UGITUtility.StringToInt(syear), 12, 31);
                else
                    dateTo = dateFrom.AddMonths(12);
            }

            if (!IsPostBack)
            {
                string defaultDisplayMode = ConfigVariableMGR.GetValue(ConfigConstants.DefaultRMMDisplayMode);
                if (!string.IsNullOrEmpty(defaultDisplayMode))
                    hdndisplayMode.Value = defaultDisplayMode;
            }

            gvPreview.EnableRowsCache = false;
            gvPreview.SettingsBehavior.AutoExpandAllGroups = true;

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

            //int ogScreenWidth = 0;
            //ogScreenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "originalScreenWidth"));

            //if (ogScreenWidth == 0)
            //{
            //    int screenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "screenWidth"));
            //    SetCookie("originalScreenWidth", Convert.ToString(screenWidth));
            //}

            base.OnInit(e);

        }

        private void EditPermisionList()
        {
            isResourceAdmin = ObjUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || ObjUserProfileManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            //allowAllocationForSelf = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
            allowAllocationForSelf = ConfigVariableMGR.GetValue(ConfigConstants.AllowAllocationForSelf);
            allowAllocationViewAll = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowAllocationViewAll);

            if (!isResourceAdmin)
                userEditPermisionList = ObjUserProfileManager.LoadAuthorizedUsers(allowAllocationForSelf);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            absoluteUrlView = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "combinedmultiallocationjs", "Add Allocation", "ResourceAllocation"));

            if (!string.IsNullOrEmpty(Request["SelectedCategory"]))
                selectedCategory = UGITUtility.StripHTML(Request["SelectedCategory"]);

            List<string> users = UGITUtility.GetGanttViewExpandedUsers(Request);
            if (users == null)
                Response.Cookies.Add(new HttpCookie(Constants.Cookie.GanttViewExpandedUsers) { Value = "all", Path = "/" });

            users = UGITUtility.GetGanttViewCollapsedUsers(Request);
            if (users == null)
                Response.Cookies.Add(new HttpCookie(Constants.Cookie.GanttViewCollapsedUsers) { Value = "", Path = "/" });
            if (string.IsNullOrWhiteSpace(Request["showuserresume"]))
            {
                ShowUserResume = Convert.ToBoolean(Request["showuserresume"]);
            }
            if (ShowUserResume && !string.IsNullOrWhiteSpace(SelectedUser))
            {
                UserResume userResume = (UserResume)Page.LoadControl("~/CONTROLTEMPLATES/RMONE/UserResume.ascx");
                if (userResume != null)
                {
                    userResume.UserID = SelectedUser;
                    UserResumePH.Controls.Add(userResume);
                }
            }
            if (!string.IsNullOrWhiteSpace(Request["ShowDateInfo"]))
            {
                ShowDateInfo = Convert.ToBoolean(Request["ShowDateInfo"]);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!IsPostBack)
            {
                gvPreview.DataBind();
            }

            gvPreview.ExpandAll();
            base.OnPreRender(e);
        }

        #endregion

        #region Control Events
        protected void previousYear_Click(object sender, ImageClickEventArgs e)
        {
            int sYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(hndYear.Value))
            {
                sYear = UGITUtility.StringToInt(hndYear.Value);
                sYear = sYear - 1;
                hndYear.Value = sYear.ToString();
            }

            lblSelectedYear.Text = hndYear.Value;
            dateFrom = new DateTime(sYear, 1, 1);
            dateTo = dateFrom.AddMonths(12);
            allocationData = null;
            gvPreview.DataBind();
        }

        protected void nextYear_Click(object sender, ImageClickEventArgs e)
        {
            int sYear = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(hndYear.Value))
            {
                sYear = UGITUtility.StringToInt(hndYear.Value);
                sYear = sYear + 1;
                hndYear.Value = sYear.ToString();
            }

            lblSelectedYear.Text = hndYear.Value;
            dateFrom = new DateTime(sYear, 1, 1);
            dateTo = dateFrom.AddMonths(12);

            allocationData = null;
            gvPreview.DataBind();
        }

        protected void gvPreview_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName == "EXPANDROW")
            {
                List<string> collapsedUsers = UGITUtility.GetGanttViewCollapsedUsers(Request);

                string userId = ((DevExpress.Web.ASPxGridView)sender).GetRowValues(Convert.ToInt32(e.Args[0]), "Id").ToString();
                List<string> users = UGITUtility.GetGanttViewExpandedUsers(Request);
                if (users == null)
                {
                    Response.Cookies.Add(new HttpCookie(Constants.Cookie.GanttViewExpandedUsers) { Value = userId, Path = "/" });
                }
                else
                {
                    if (collapsedUsers != null)
                    {
                        if (collapsedUsers.Contains(userId))
                        {
                            collapsedUsers.Remove(userId);
                            Response.Cookies[Constants.Cookie.GanttViewCollapsedUsers].Value = string.Join(",", collapsedUsers);
                        }
                    }
                    if (users.Count == 1 && users[0] == "all")
                        users.Clear();
                    if (!users.Contains(userId))
                        users.Add(userId);
                    Response.Cookies[Constants.Cookie.GanttViewExpandedUsers].Value = string.Join(",", users);
                }
                if (collapsedUsers != null && collapsedUsers.Count == 1 && collapsedUsers[0] == "all")
                    Response.Cookies[Constants.Cookie.GanttViewCollapsedUsers].Value = "";
            }
            else if (e.CallbackName == "COLLAPSEROW")
            {
                List<string> expandedUsers = UGITUtility.GetGanttViewExpandedUsers(Request);

                string userId = ((DevExpress.Web.ASPxGridView)sender).GetRowValues(Convert.ToInt32(e.Args[0]), "Id").ToString();
                List<string> users = UGITUtility.GetGanttViewCollapsedUsers(Request);
                if (users == null)
                {
                    Response.Cookies.Add(new HttpCookie(Constants.Cookie.GanttViewCollapsedUsers) { Value = userId, Path = "/" });
                }
                else
                {
                    if (expandedUsers != null)
                    {
                        if (expandedUsers.Contains(userId))
                        {
                            expandedUsers.Remove(userId);
                            Response.Cookies[Constants.Cookie.GanttViewExpandedUsers].Value = string.Join(",", expandedUsers);
                        }
                    }
                    if (users.Count == 1 && users[0] == "all")
                        users.Clear();
                    if (!users.Contains(userId))
                        users.Add(userId);
                    Response.Cookies[Constants.Cookie.GanttViewCollapsedUsers].Value = string.Join(",", users);
                }
                if (expandedUsers != null && expandedUsers.Count == 1 && expandedUsers[0] == "all")
                    Response.Cookies[Constants.Cookie.GanttViewExpandedUsers].Value = "";
            }
            else if (e.CallbackName == "REFRESH")
            {
                Response.Cookies[Constants.Cookie.GanttViewCollapsedUsers].Value = "";
                Response.Cookies[Constants.Cookie.GanttViewExpandedUsers].Value = "all";
            }
            else if (e.CallbackName == "COLLAPSEALL")
            {
                Response.Cookies[Constants.Cookie.GanttViewCollapsedUsers].Value = "all";
                Response.Cookies[Constants.Cookie.GanttViewExpandedUsers].Value = "";
            }
            else if (e.CallbackName == "EXPANDALL")
            {
                Response.Cookies[Constants.Cookie.GanttViewExpandedUsers].Value = "all";
                Response.Cookies[Constants.Cookie.GanttViewCollapsedUsers].Value = "";
            }
        }

        protected void gvPreview_DataBinding(object sender, EventArgs e)
        {
            if (allocationData == null)
            {
                try
                {
                    allocationData = GetAllocationData();
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, ex.Message);
                }

                if (!stopToRegerateColumns)
                    PrepareAllocationGrid();
            }
            gvPreview.DataSource = allocationData;

        }


        protected void rbtnPercentage_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "percentage");
        }

        protected void rbtnFTE_CheckedChanged(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "fte");
        }


        #endregion

        #region Custom Methods

        protected void FillDropDownTypeAndColorGrid()
        {
            DataTable dtTypeData = AllocationTypeManager.LoadLevel1(context);

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
            if (dtTypeData != null)
            {
                for (int i = dtTypeData.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtTypeData.Rows[i]["LevelTitle"] == DBNull.Value)
                    {
                        dtTypeData.Rows[i].Delete();
                    }
                }
                dtTypeData.AcceptChanges();

                if (!dtTypeData.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                    dtTypeData.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));

                if (!dtTypeData.Columns.Contains("EstimatedColor"))
                    dtTypeData.Columns.Add("EstimatedColor", typeof(string));

                if (!dtTypeData.Columns.Contains("AssignedColor"))
                    dtTypeData.Columns.Add("AssignedColor", typeof(string));

                if (!dtTypeData.Columns.Contains("ColumnType"))
                    dtTypeData.Columns.Add("ColumnType", typeof(string));
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                foreach (DataRow rowitem in dtTypeData.Rows)
                {

                    DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, string.Format("{0}='{1}' and {2}='{3}' ", DatabaseObjects.Columns.Title, Convert.ToString(rowitem["LevelTitle"]), DatabaseObjects.Columns.TenantID, context.TenantID)).Select();
                    if (drModules != null && drModules.Length > 0)
                    {
                        rowitem[DatabaseObjects.Columns.ModuleName] = Convert.ToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                        rowitem["ColumnType"] = "Module";

                    }
                    else
                    {
                        rowitem[DatabaseObjects.Columns.ModuleName] = rowitem["LevelTitle"];
                        rowitem["ColumnType"] = "NonModule";
                    }

                    if (lstEstimateColors != null && lstEstimateColors.Count > 0)
                    {
                        string value = Convert.ToString(lstEstimateColors.FirstOrDefault(x => x.Contains(Convert.ToString(rowitem[DatabaseObjects.Columns.ModuleName]))));
                        rowitem["EstimatedColor"] = UGITUtility.SplitString(value, ";#", 1);
                    }

                    if (lstAssignColors != null && lstAssignColors.Count > 0)
                    {
                        string value = Convert.ToString(lstAssignColors.FirstOrDefault(x => x.Contains(Convert.ToString(rowitem[DatabaseObjects.Columns.ModuleName]))));
                        rowitem["AssignedColor"] = UGITUtility.SplitString(value, ";#", 1);
                    }
                }

                //grdColor.DataSource = dtTypeData;
                //grdColor.DataBind();

                dtFilterTypeData = dtTypeData;
            }
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

        #endregion

        protected string GetColor(object container)
        {
            return Convert.ToString(container);
        }
        protected void grdColor_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "AssignedColor")
            {
                if (Convert.ToString(e.GetValue("ColumnType")) == "NonModule")
                {
                    e.Cell.Text = "";
                }
            }
        }
        protected void gvPreview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        { 
            
            string foreColor = "#000000";
            string Estimatecolor = "#24b6fe";
            string Assigncolor = "#24b6fe";
            string moduleName = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ModuleName));
            if (lstEstimateColorsAndFontColors != null && lstEstimateColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstEstimateColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Estimatecolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                    foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
                }
            }

            if (lstAssignColorsAndFontColors != null && lstAssignColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstAssignColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                Assigncolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                //foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
            }
            if (e.DataColumn.FieldName != DatabaseObjects.Columns.Resource && e.DataColumn.FieldName != "AllocationType" && 
                e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate && 
                e.DataColumn.FieldName != DatabaseObjects.Columns.Title)
            {
                int defaultBarH = 12;
                if (DisablePlannedAllocation)
                    defaultBarH = 24;
                string html;
                DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                string type = UGITUtility.ObjectToString(e.GetValue("ProjectNameLink"));

                string tickId = string.Empty;
                string allStartDates = string.Empty;
                string allEndDates = string.Empty;

                DateTime aStartDate = DateTime.MinValue;
                DateTime aEndDate = DateTime.MinValue;
                DateTime aPreconStart = DateTime.MinValue;
                DateTime aPreconEnd = DateTime.MinValue;
                DateTime aConstStart = DateTime.MinValue;
                DateTime aConstEnd = DateTime.MinValue;
                DateTime aCloseout = DateTime.MinValue;
                string workitemIds = string.Empty;
                string id = string.Empty;
                string name = string.Empty;
                bool aSoftAllocation = true;
                string project = string.Empty;
                string allocId = string.Empty;
                string subworkitem = String.Empty;
                string workitemid = string.Empty;
                string chanceOfSuccess = string.Empty;
                
                if (e.VisibleIndex > 0)
                {
                    DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                    tickId = row[DatabaseObjects.Columns.TicketId].ToString();
                    aStartDate = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.AllocationStartDate]);
                    aEndDate = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.AllocationEndDate]);
                    aPreconStart = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.PreconStartDate]);
                    aPreconEnd = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.PreconEndDate]);
                    aConstStart = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    aConstEnd = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    aCloseout = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.CloseoutDate]);
                    aSoftAllocation = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.SoftAllocation]);
                    project = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                    name = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Name]);
                    id = row[DatabaseObjects.Columns.Id].ToString();
                    allocId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.AllocationID]);
                    workitemIds = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.WorkItemID]);
                    subworkitem = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.SubWorkItem]);
                    chanceOfSuccess = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ChanceOfSuccess]);

                    allStartDates = UGITUtility.ObjectToString(row["AllStartDates"]);
                    allEndDates = UGITUtility.ObjectToString(row["allEndDates"]);

                }
                if (allStartDates != null && allEndDates != null)
                {
                    List<DateTime> startdates = UGITUtility.ConvertStringToDateList(allStartDates);
                    List<DateTime> enddates = UGITUtility.ConvertStringToDateList(allEndDates);
                    // Combine start and end dates into a single collection of date pairs
                    List<(DateTime StartDate, DateTime EndDate)> datePairs = startdates.Zip(enddates, (start, end) => (start, end)).ToList();
                    // Sort the date pairs based on the start dates
                    datePairs.Sort((pair1, pair2) => pair1.StartDate.CompareTo(pair2.StartDate));

                    //if (tickId != "OPM-22-001948")  //debugging code
                    //    return;
                    DateTime dateF = Convert.ToDateTime(dateFrom);
                    DateTime dateT = hdndisplayMode.Value == "Weekly" ? Convert.ToDateTime(dateTo).AddDays(1) : Convert.ToDateTime(dateTo);

                    for (DateTime dt = dateF; dateT > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                    {
                        html = "";
                        string resourceWorkItemColumnName = "ResourceWorkItemLookup" + dt.ToString("MMM-dd-yy") + "E";
                        string resourceWorkItems = null;
                        string allocationIDColumnName = dt.ToString("MMM-dd-yy") + "E_AllocationID";
                        if (e.DataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                        {
                            DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                            string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                            string allocationIDs = string.Empty;
                            if (UGITUtility.IfColumnExists(row, dt.ToString("MMM-dd-yy") + "_WorkItem"))
                            {
                                workitemid = UGITUtility.ObjectToString(row[dt.ToString("MMM-dd-yy") + "_WorkItem"]);
                            }
                            if (UGITUtility.IfColumnExists(row, resourceWorkItemColumnName))
                            {
                                resourceWorkItems = UGITUtility.ObjectToString(row[resourceWorkItemColumnName]);
                            }
                            if (UGITUtility.IfColumnExists(row, allocationIDColumnName))
                            {
                                allocationIDs = UGITUtility.ObjectToString(row[allocationIDColumnName]);
                            }
                            string workItemId = workitemid;
                            string roundleftbordercls = string.Empty;
                            string roundrightbordercls = string.Empty;
                            string backgroundColor = string.Empty;
                            if (aStartDate == DateTime.MinValue || aEndDate == DateTime.MinValue)
                                return;
                            // classed to set round corners on bar end points
                            if (hdndisplayMode.Value != "Weekly")
                            {
                                if (aStartDate.Year < dt.Year && dt.Month == 1)
                                {
                                    roundleftbordercls = "RoundLeftSideCorner";
                                }
                                if (aEndDate.Year > dt.Year && dt.Month == 12)
                                {
                                    roundrightbordercls = "RoundRightSideCorner";
                                }
                                if (aStartDate.Month == dt.Month && aStartDate.Year == dt.Year)
                                {
                                    roundleftbordercls = "RoundLeftSideCorner";
                                }
                                if (aEndDate.Month == dt.Month && aEndDate.Year == dt.Year)
                                {
                                    roundrightbordercls = "RoundRightSideCorner";
                                }
                            }
                            else if (hdndisplayMode.Value == "Weekly")
                            {

                                if (aStartDate >= dt && aStartDate.AddDays(-6) < dt)
                                {
                                    roundleftbordercls = "RoundLeftSideCorner";
                                }
                                if (aEndDate >= dt && aEndDate <= dt.AddDays(6))
                                {
                                    roundrightbordercls = "RoundRightSideCorner";
                                }
                            }

                            backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt);

                            if (dt.Month == aConstStart.Month && dt.Month == aPreconEnd.Month)
                            {
                                if (aEndDate <= aPreconEnd)
                                {
                                    backgroundColor = "preconbgcolor";
                                }
                            }

                            string cell = e.Cell.ClientID;
                            //string tooltipString = uHelper.GetTooltipString(context, datePairs);
                            string startDateString = uHelper.GetDateStringInFormat(context, aStartDate, false);
                            string endDateString = uHelper.GetDateStringInFormat(context, aEndDate, false);
                            if (!string.IsNullOrEmpty(estAlloc) && estAlloc != "0")
                            {
                                if (aSoftAllocation)
                                    backgroundColor = "softallocationbgcolor";
                                if (moduleName != null && (moduleName.Equals("Time Off") || moduleName.Equals("Other")))
                                {
                                    //backgroundColor = "ptobgcolor";
                                    html = GeneratePtoCard(dt, id, defaultBarH, type, cell);
                                    e.Cell.CssClass += " ptoAlignmentClass";
                                }
                                else
                                {
                                    html = GenerateGanttCell(moduleName, foreColor, Estimatecolor, defaultBarH, name, aPreconStart, aPreconEnd, aConstStart, aConstEnd, allocationIDs, startdates, enddates, dt,
                                        ref estAlloc, ref roundleftbordercls, ref roundrightbordercls, ref backgroundColor, cell, startDateString, endDateString, resourceWorkItems, datePairs, subworkitem, aCloseout, chanceOfSuccess);
                                    html = $"<div>{html}</div>";
                                }
                            }
                            else
                            {
                                html = $" <div id='gCell_{cell}' runat='server' class='{roundleftbordercls} {roundrightbordercls}' " +
                                    $"style='float:left;width:100%;padding-left:{defaultBarH}px;height: {defaultBarH}px;'></div>";
                            }
                            e.Cell.Text = html;
                            //sets alternate color to columns
                            if (e.DataColumn.VisibleIndex % 2 == 0)
                                e.Cell.BackColor = Color.WhiteSmoke;
                        }


                    }
                }
            }

        }

        private string GenerateGanttCell(string moduleName,string foreColor, string Estimatecolor, int defaultBarH, string name, DateTime aPreconStart, DateTime aPreconEnd, DateTime aConstStart, DateTime aConstEnd,
           string allocId, List<DateTime> startdates, List<DateTime> enddates, DateTime dt, ref string estAlloc, ref string roundleftbordercls,
           ref string roundrightbordercls, ref string backgroundColor, string cell, string startDateString, string endDateString, string workItemId,
           List<(DateTime StartDate, DateTime EndDate)> datePairs,
           string subworkitem, DateTime aCloseout, string chanceOfSuccess)
        {
            string html = string.Empty;
            estAlloc = estAlloc + "% <br>";

            int NoOfDays = 0;
            int remainingDays = 0;
            int widthEstAlloc = 100;
            int index = 0;
            string leftMargin = "0";
            int daysInMonth = System.DateTime.DaysInMonth(dt.Year, dt.Month);
            List<string> lstAllocationIds = UGITUtility.ConvertStringToList(allocId, Constants.Separator6);
            List<Tuple<DateTime, DateTime, string, string, string>> dateworkitempair = ConvertStringToTupleList(workItemId);
            if (hdndisplayMode.Value != "Weekly")
            {
                int tempWidthAlloc = 0;
                foreach (var item in datePairs)
                {
                    bool renderHtml = false;
                    leftMargin = "0";
                    allocId = lstAllocationIds.FirstOrDefault();
                    Tuple<DateTime, DateTime, string, string, string> workitemlookup = dateworkitempair.FirstOrDefault(x => x.Item1 == item.StartDate && x.Item2 == item.EndDate);
                    startDateString = uHelper.GetDateStringInFormat(context, item.StartDate, false);
                    endDateString = uHelper.GetDateStringInFormat(context, item.EndDate, false);
                    bool aSoftAllocation = UGITUtility.StringToBoolean(workitemlookup?.Item4);
                    string tempChanceOfSuccess = aSoftAllocation ? chanceOfSuccess : "";

                    if (!string.IsNullOrWhiteSpace(workitemlookup?.Item5))
                    {
                        estAlloc = workitemlookup?.Item5 + "% <br>";
                    }
                    if (aSoftAllocation)
                        backgroundColor = "softallocationbgcolor";
                    if ((item.StartDate.Month == dt.Month && item.StartDate.Year == dt.Year)
                    && (item.EndDate.Month == dt.Month && item.EndDate.Year == dt.Year))
                    {
                        remainingDays = item.EndDate.Day - item.StartDate.Day;
                        widthEstAlloc = ((remainingDays + 1) * 100) / daysInMonth;
                        leftMargin = Convert.ToString(((item.StartDate.Day - 1) * 100 / daysInMonth) - tempWidthAlloc);
                        roundleftbordercls = "RoundLeftSideCorner";
                        roundrightbordercls = "RoundRightSideCorner";
                        backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, item.StartDate, aSoftAllocation: aSoftAllocation);
                        renderHtml = true;
                    }
                    else if (item.EndDate.Month == dt.Month && item.EndDate.Year == dt.Year)
                    {
                        NoOfDays = item.EndDate.Day;
                        widthEstAlloc = (NoOfDays * 100) / daysInMonth;
                        roundleftbordercls = "RoundRightSideCorner";
                        roundrightbordercls = "";
                        backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt, item.EndDate, aSoftAllocation: aSoftAllocation);
                        renderHtml = true;
                    }
                    else if (item.StartDate.Month == dt.Month && item.StartDate.Year == dt.Year)
                    {
                        NoOfDays = item.StartDate.Day;
                        remainingDays = daysInMonth - NoOfDays;
                        widthEstAlloc = (remainingDays * 100) / daysInMonth;
                        roundleftbordercls = "RoundLeftSideCorner";
                        roundrightbordercls = "";
                        backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, item.StartDate, aSoftAllocation: aSoftAllocation);
                        renderHtml = true;
                    }
                    index++;
                    if (widthEstAlloc >= 0 && renderHtml)
                    {
                        tempWidthAlloc += widthEstAlloc + UGITUtility.StringToInt(leftMargin);

                        //if (widthEstAlloc != 100)
                        //{
                        //    if (widthEstAlloc > 12)
                        //    {
                        //        widthEstAlloc -= 2;
                        //    }
                        //    //else if (widthEstAlloc >= 10 && widthEstAlloc <= 50)
                        //    //{
                        //    //    widthEstAlloc -= 2;
                        //    //}
                        //    else if (widthEstAlloc <= 4)
                        //    {
                        //        widthEstAlloc = 4;
                        //    }
                        //}
                        html += GetCellDivBasedOnEstAlloc(moduleName, foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls,
                            roundrightbordercls, backgroundColor, cell + index, startDateString, endDateString, Convert.ToString(widthEstAlloc), workitemlookup?.Item3, subworkitem, aPreconStart,
                            aPreconEnd, aConstStart, aConstEnd, aCloseout, tempChanceOfSuccess, leftMargin);
                    }
                }
            }

            if (string.IsNullOrEmpty(html))
            {
                dateworkitempair = dateworkitempair.OrderBy(x => x.Item1).ToList();
                int tempWidthAlloc = 0;
                foreach (var workitemlookup in dateworkitempair)
                {
                    widthEstAlloc = 100;
                    roundrightbordercls = "";
                    roundleftbordercls = "";
                    leftMargin = "0";
                    var matchingPair = datePairs.FirstOrDefault(pair => workitemlookup.Item1 == pair.StartDate && workitemlookup.Item2 == pair.EndDate);
                    startDateString = uHelper.GetDateStringInFormat(context, matchingPair.StartDate, false);
                    endDateString = uHelper.GetDateStringInFormat(context, matchingPair.EndDate, false);
                    if (dateworkitempair.Count > 1)
                    {
                        widthEstAlloc = 0;
                    }
                    if (!string.IsNullOrWhiteSpace(workitemlookup?.Item5))
                    {
                        estAlloc = workitemlookup?.Item5 + "% <br>";
                    }
                    if (matchingPair.StartDate >= dt && matchingPair.StartDate <= dt.AddDays(6)
                        && matchingPair.EndDate >= dt && matchingPair.EndDate <= dt.AddDays(6)) 
                    {
                        roundleftbordercls = "RoundLeftSideCorner";
                        roundrightbordercls = "RoundRightSideCorner";
                        remainingDays = (matchingPair.EndDate - matchingPair.StartDate).Days;
                        leftMargin = Convert.ToString(((matchingPair.StartDate - dt).Days * 100 / 7) - tempWidthAlloc);
                        widthEstAlloc = ((remainingDays + 1) * 100) / 7;
                    }
                    else if (matchingPair.StartDate >= dt && matchingPair.StartDate <= dt.AddDays(6))
                    {
                        NoOfDays = 7 - (int)matchingPair.StartDate.DayOfWeek;
                        widthEstAlloc = ((NoOfDays+1) * 100) / 7;
                        roundleftbordercls = "RoundLeftSideCorner";
                        roundrightbordercls = "";
                        leftMargin = "0";
                    }
                    else if (matchingPair.EndDate >= dt && matchingPair.EndDate <= dt.AddDays(6))
                    {
                        NoOfDays = (int)matchingPair.EndDate.DayOfWeek == 0 ? 7 : (int)matchingPair.EndDate.DayOfWeek;
                        widthEstAlloc = (NoOfDays * 100) / 7;
                        roundrightbordercls = "RoundRightSideCorner";
                        roundleftbordercls = "";
                        leftMargin = "0";
                    }
                    if (widthEstAlloc >= 0)
                    {
                        if (widthEstAlloc != 100)
                        {
                            tempWidthAlloc += widthEstAlloc + UGITUtility.StringToInt(leftMargin);
                        }
                    }

                    bool aSoftAllocation = UGITUtility.StringToBoolean(workitemlookup?.Item4);
                    backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt, dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), aSoftAllocation);


                    html += GetCellDivBasedOnEstAlloc(moduleName,foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls, roundrightbordercls,
                        backgroundColor, cell + index, startDateString, endDateString, Convert.ToString(widthEstAlloc), workitemlookup.Item3, subworkitem, aPreconStart, aPreconEnd, aConstStart,
                        aConstEnd, aCloseout, chanceOfSuccess, leftMargin);
                    index++;
                }
            }
            return html;
        }

        private string GetCellDivBasedOnEstAlloc(string moduleName,string foreColor, string Estimatecolor, int defaultBarH, string name, string allocId, string estAlloc, 
            string roundleftbordercls, string roundrightbordercls, string backgroundColor, string cell, string startDateString, string endDateString, 
            string widthEstAlloc, string workItemId, string subworkitem, DateTime aPreconStart, DateTime aPreconEnd, DateTime aConstStart, DateTime aConstEnd, 
            DateTime aCloseout, string chanceOfSuccess, string leftMargin)
        {
            string html;
            string margin_left = "";
            bool isEditAllowed = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowEditInGantt);
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Replace("'", "`");
            }

            string onClickEvent = isEditAllowed ? $"onclick='javascript:event.cancelBubble=true; openworkitem(\"{UGITUtility.StringToInt(workItemId)}\", \"{name}\",\"{subworkitem}\", \"{aPreconStart}\",\"{aPreconEnd}\",\"{aConstStart}\", \"{aConstEnd}\",\"{aConstEnd.AddDays(1)}\", \"{aCloseout}\",\"{startDateString}\", \"{endDateString}\")'" : string.Empty;
            string cursorStyle = isEditAllowed ? "cursor:pointer;" : string.Empty;
            if (!string.IsNullOrEmpty(widthEstAlloc) && UGITUtility.StringToInt(widthEstAlloc) >= 0)
            {
                //if (!string.IsNullOrEmpty(roundleftbordercls) && !string.IsNullOrEmpty(roundrightbordercls))
                //    margin_left = string.Format("margin-left: {0}%;", (UGITUtility.StringToDateTime(startDateString).Day * 100 / 30));
                if (Convert.ToInt32(widthEstAlloc) >= 35)
                {
                    html = $"<div id='gCell_{cell}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor} overLappingDiv pt-1' ";

                    if (moduleName == "OPM")
                    {
                        html = html + $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{estAlloc.Replace(" <br>", string.Empty)}\",\"{subworkitem}\", \"{chanceOfSuccess}\")' onmouseout='hideTooltip(this)' ";
                    }

                    DateTime startDate = UGITUtility.StringToDateTime(startDateString);
                    DateTime endDate = UGITUtility.StringToDateTime(endDateString);

                    if (startDate < DateTime.Now.Date && endDate < DateTime.Now.Date)
                    {
                        html = html + $"onmouseover='showTooltipdoubleclick(this,\"Double click to change past allocations\")' onmouseout='hideTooltip(this)' ";
                        onClickEvent = isEditAllowed ? $"ondblclick='javascript:event.cancelBubble=true; openworkitem(\"{UGITUtility.StringToInt(workItemId)}\", \"{name}\",\"{subworkitem}\", \"{aPreconStart}\",\"{aPreconEnd}\",\"{aConstStart}\", \"{aConstEnd}\",\"{aConstEnd.AddDays(1)}\", \"{aCloseout}\",\"{startDateString}\", \"{endDateString}\")'" : string.Empty;
                    }

                    html = html + $"{onClickEvent}" +
                    $"style='width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;margin-left:{leftMargin}%;{cursorStyle}'>{estAlloc}</div>";

                }
                else
                {

                    html = $"<div id='gCell_{cell}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor} overLappingDiv pt-1' ";
                    if (moduleName == "OPM")
                    {
                        html = html + $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{estAlloc.Replace(" <br>", string.Empty)}\",\"{subworkitem}\", \"{chanceOfSuccess}\")' onmouseout='hideTooltip(this)' ";
                    }

                    DateTime startDate = UGITUtility.StringToDateTime(startDateString);
                    DateTime endDate = UGITUtility.StringToDateTime(endDateString);

                    if (startDate < DateTime.Now.Date && endDate < DateTime.Now.Date)
                    {
                        html = html + $"onmouseover='showTooltipdoubleclick(this,\"Double click to change past allocations\")' onmouseout='hideTooltip(this)' ";
                        onClickEvent = isEditAllowed ? $"ondblclick='javascript:event.cancelBubble=true; openworkitem(\"{UGITUtility.StringToInt(workItemId)}\", \"{name}\",\"{subworkitem}\", \"{aPreconStart}\",\"{aPreconEnd}\",\"{aConstStart}\", \"{aConstEnd}\",\"{aConstEnd.AddDays(1)}\", \"{aCloseout}\",\"{startDateString}\", \"{endDateString}\")'" : string.Empty;
                    }

                    html = html + $"{onClickEvent}";
                    html += $"style='width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;margin-left:{leftMargin}%;{cursorStyle}'></div>";
                }
            }
            else
            {
                html = $"<div id='gCell_{cell}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor} overLappingDiv pt-1' ";

                if (moduleName == "OPM")
                {
                   html = html+ $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{estAlloc.Replace(" <br>", string.Empty)}\",\"{subworkitem}\", \"{chanceOfSuccess}\")' onmouseout='hideTooltip(this)' ";
                }

                DateTime startDate = UGITUtility.StringToDateTime(startDateString);
                DateTime endDate = UGITUtility.StringToDateTime(endDateString);

                if (startDate < DateTime.Now.Date && endDate < DateTime.Now.Date)
                {
                    html = html + $"onmouseover='showTooltipdoubleclick(this,\"Double click to change past allocations\")' onmouseout='hideTooltip(this)' ";
                    onClickEvent = isEditAllowed ? $"ondblclick='javascript:event.cancelBubble=true; openworkitem(\"{UGITUtility.StringToInt(workItemId)}\", \"{name}\",\"{subworkitem}\", \"{aPreconStart}\",\"{aPreconEnd}\",\"{aConstStart}\", \"{aConstEnd}\",\"{aConstEnd.AddDays(1)}\", \"{aCloseout}\",\"{startDateString}\", \"{endDateString}\")'" : string.Empty;
                }

                html = html+  $"{onClickEvent}" +
                    $"style='width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;{margin_left}'>{estAlloc}</div>";
            }

            return html;
        }

        public static List<Tuple<DateTime, DateTime, string, string, string>> ConvertStringToTupleList(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input string is null or empty.");

            List<string> tupleStrings = UGITUtility.ConvertStringToList(input, Constants.Separator);
            var tupleList = new List<Tuple<DateTime, DateTime, string, string, string>>();

            foreach (string tupleString in tupleStrings)
            {
                string[] dateParts = tupleString.Split(',');
                if (dateParts.Length != 5)
                    throw new ArgumentException("Invalid date format in the input string.");

                DateTime date1 = UGITUtility.StringToDateTime(dateParts[0]), date2 = UGITUtility.StringToDateTime(dateParts[1]);
                tupleList.Add(Tuple.Create(UGITUtility.StringToDateTime(date1), UGITUtility.StringToDateTime(date2), dateParts[2], dateParts[3], dateParts[4]));
            }

            return tupleList;
        }
        public string GetVerticalLine(int percentage)
        {
            return $"left: {percentage}%; transform: translateX(-{percentage}%);position: relative;border-left: 1px solid black;height: 100%;";
        }
        
        public string GeneratePtoCard(DateTime currentDate, string userId, int defaultBarH, string workItem, string cell)
        {
            string html = string.Empty;
            string marginLeft = string.Empty;
            DateTime startDate = currentDate;
            DateTime endDate = DateTime.MinValue;
            string leftMargin = string.Empty;
            int widthEstAlloc = 100;
            string roundleftbordercls = "";
            string roundrightbordercls = "";
            int totalWorkingDays = 0;
            int remainingDays = 0;
            int NoOfDays = 0;
            Tuple<int, int> addDays = new Tuple<int, int>(10, 20);
            if (hdndisplayMode.Value == "Weekly")
            {
                endDate = currentDate.AddDays(6);
                addDays = new Tuple<int, int>(2, 4);
            }
            else if (hdndisplayMode.Value == "Quarterly")
            {
                endDate = startDate.AddMonths(3);
                int firstInterval = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                int secondInterval = firstInterval + DateTime.DaysInMonth(startDate.Year, startDate.Month + 1);
                addDays = new Tuple<int, int>(firstInterval, secondInterval);
            }
            else
            {
                int lastDayOfMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                endDate = new DateTime(startDate.Year, startDate.Month, lastDayOfMonth);
            }
            int daysInMonth = System.DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            DataTable dt = ResourceAllocationManager.LoadRawTableByResource(userId.Split(',').ToList(), 4, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] monthlyData = dt.Select($"{DatabaseObjects.Columns.AllocationStartDate}<='{endDate}' and {DatabaseObjects.Columns.AllocationEndDate} >= '{startDate}'" +
                                            $"and ({DatabaseObjects.Columns.WorkItemType}='Time Off' or {DatabaseObjects.Columns.WorkItemType}='Other') and {DatabaseObjects.Columns.WorkItemLink}='{workItem}'");

                if (monthlyData.Count() > 0)
                {
                    int tempWidthAlloc = 0;
                    foreach (DataRow data in monthlyData)
                    {
                        widthEstAlloc = 100;
                        roundrightbordercls = "";
                        roundleftbordercls = "";
                        leftMargin = "0";
                        string pctAllocation = data.Field<double>(DatabaseObjects.Columns.PctAllocation).ToString() + "%";
                        string taskId = data.Field<long>(DatabaseObjects.Columns.Id).ToString();
                        string resourceName = data.Field<string>(DatabaseObjects.Columns.Resource).ToString().Replace("'", "`");
                        DateTime fromDate = data.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate);
                        DateTime toDate = data.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate);
                        if (hdndisplayMode.Value == "Weekly")
                        {

                            if (fromDate >= startDate && fromDate.AddDays(-6) <= startDate
                            && toDate >= startDate && toDate <= endDate)
                            {
                                roundleftbordercls = "RoundLeftSideCorner";
                                roundrightbordercls = "RoundRightSideCorner";
                                remainingDays = (toDate - fromDate).Days;
                                leftMargin = Convert.ToString(((fromDate - startDate).Days * 100 / 7) - tempWidthAlloc);
                                widthEstAlloc = ((remainingDays + 1) * 100) / 7;
                            }
                            else if (fromDate >= startDate && fromDate.AddDays(-6) <= startDate)
                            {
                                NoOfDays = (endDate - fromDate).Days + 1; ;
                                widthEstAlloc = ((NoOfDays) * 100) / 7;
                                roundleftbordercls = "RoundLeftSideCorner";
                                roundrightbordercls = "";
                            }
                            else if (toDate >= startDate && toDate <= endDate)
                            {
                                NoOfDays = (toDate - startDate).Days + 1;
                                widthEstAlloc = (NoOfDays * 100) / 7;
                                roundrightbordercls = "RoundRightSideCorner";
                                roundleftbordercls = "";
                            }
                        }
                        else
                        {
                            if ((fromDate.Month == startDate.Month && fromDate.Year == startDate.Year)
                                && (toDate.Month == startDate.Month && toDate.Year == startDate.Year))
                            {
                                remainingDays = toDate.Day - fromDate.Day;
                                widthEstAlloc = ((remainingDays + 1) * 100) / daysInMonth;
                                leftMargin = Convert.ToString(((fromDate.Day - 1) * 100 / daysInMonth) - tempWidthAlloc);
                                roundleftbordercls = "RoundLeftSideCorner";
                                roundrightbordercls = "RoundRightSideCorner";
                            }
                            else if (toDate.Month == startDate.Month && toDate.Year == startDate.Year)
                            {
                                NoOfDays = toDate.Day;
                                widthEstAlloc = (NoOfDays * 100) / daysInMonth;
                                roundleftbordercls = "RoundRightSideCorner";
                                roundrightbordercls = "";
                            }
                            else if (fromDate.Month == startDate.Month && fromDate.Year == startDate.Year)
                            {
                                NoOfDays = fromDate.Day;
                                remainingDays = daysInMonth - NoOfDays;
                                widthEstAlloc = (remainingDays * 100) / daysInMonth;
                                roundleftbordercls = "RoundLeftSideCorner";
                                roundrightbordercls = "";
                            }
                        }

                        if (widthEstAlloc >= 0)
                        {
                            if (widthEstAlloc != 100)
                            {
                                tempWidthAlloc += widthEstAlloc + UGITUtility.StringToInt(leftMargin);
                            }
                        }

                        int dayDiff = uHelper.GetTotalWorkingDaysBetween(context, fromDate, toDate);
                        //dayDiff = GetOverlappingDays(startDate, endDate, fromDate, toDate);
                        //dayDiff += 1;
                        string outputData = widthEstAlloc > 25 ? useGanttDayFormat ? dayDiff.ToString() : pctAllocation : "";
                        html += $"<a href=\"#\" onclick=\"OpenTimeOffAllocation('{taskId}', '{resourceName}')\"  " +
                                $">";
                        html += $"<div id='gCell1_{taskId}_{cell}' class='{roundleftbordercls} {roundrightbordercls} ptobgcolor'  style='width:{widthEstAlloc}%;margin-bottom:5%;color:white;margin-left:{leftMargin}%;height:{defaultBarH}px;padding-top:5px;font-weight:500;'" +
                            $" onmouseover='showTooltipPTO(this,\"{fromDate.ToString("dd MMM, yyyy")}\",\"{toDate.ToString("dd MMM, yyyy")}\")' onmouseout='hideTooltip(this)'>{outputData}</div>";
                        html += "</a>";
                    }
                }
            }
            return html;
        }
        public static double GetOverlappingDays(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
        {
            DateTime maxStart = firstStart > secondStart ? firstStart : secondStart;
            DateTime minEnd = firstEnd < secondEnd ? firstEnd : secondEnd;
            TimeSpan interval = minEnd - maxStart;
            double returnValue = interval > TimeSpan.FromSeconds(0) ? interval.TotalDays : 0;
            return returnValue;
        }
        public static bool IsFirstMondayOfYear(DateTime date)
        {
            int year = date.Year;
            DateTime firstDayOfYear = new DateTime(year, 1, 1);
            int daysToMonday = ((int)DayOfWeek.Monday - (int)firstDayOfYear.DayOfWeek + 7) % 7;
            DateTime firstMondayOfYear = firstDayOfYear.AddDays(daysToMonday);

            return date == firstMondayOfYear;
        }
        public static bool IsDateInFirstWeekOfYear(DateTime date)
        {
            // Get the first day of the year
            DateTime firstDayOfYear = new DateTime(date.Year, 1, 1);

            // Get the day of the week for the first day of the year
            int firstDayOfWeek = (int)firstDayOfYear.DayOfWeek;

            // Calculate the date of the first week's starting day
            DateTime firstWeekStart = firstDayOfYear.AddDays(7 - firstDayOfWeek);

            // Check if the date is in the first week of the year
            return date >= firstWeekStart && date < firstWeekStart.AddDays(7);
        }
        public static int GetIso8601WeekOfYear(DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        protected void gvPreview_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.Data)
            //{
            //    DataRow currentRow = gvPreview.GetDataRow(e.VisibleIndex);

            //    if (!isResourceAdmin)
            //    {
            //        string userId = UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.Id]);
            //        if (userEditPermisionList != null && userEditPermisionList.Count > 0 && !userEditPermisionList.Exists(x => x.Id == userId))
            //            e.Row.Attributes.Add("onclick", "event.cancelBubble = true");
            //    }
            //}
        }

        protected void gvPreview_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == GridViewRowType.Data)
            {
                DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                string absoluteUrlEdit = string.Empty;
                string userid = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Id]);
                string cmicno = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ERPJobID]);
                string companyname = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.CRMCompanyLookup]);
                string subworkitem = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.SubWorkItem]);
                string onHoldCss = row[DatabaseObjects.Columns.OnHold].ToString() == "1" ? "glabel2red" : "glabel2";
                string cmicnoLabel = row[DatabaseObjects.Columns.OnHold].ToString() == "1" ? "cmicnoLabelred" : "cmicnoLabel";
                string ProjectLeadUser = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ProjectLeadUser]) != null ? UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ProjectLeadUser]) : "";
                string LeadEstimatorUser = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.LeadEstimatorUser]) != null ? UGITUtility.ObjectToString(row[DatabaseObjects.Columns.LeadEstimatorUser]) : "";
                string ProjectManagerUser = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketProjectManager]) != null ? UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketProjectManager]) : "";
                if (!string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.AllocationEndDate])))
                    absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&ticketId={4}&tabname=allocationtimeline&isRedirectFromCardView=true",
                        "CustomResourceAllocation", userid, UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.AllocationStartDate])).ToShortDateString(), UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.AllocationEndDate])).ToShortDateString(), Convert.ToString(row[DatabaseObjects.Columns.TicketId])));
                else
                    absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&ticketId={4}&workItemType={5}&subWorkItem={6}&monthlyAllocationEdit=false&tabname=allocationtimeline&isRedirectFromCardView=true",
                        "CustomResourceAllocation", userid, UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.PlannedStartDate])).ToShortDateString(), UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.PlannedEndDate])).ToShortDateString(), Convert.ToString(row[DatabaseObjects.Columns.TicketId]), Convert.ToString(row[DatabaseObjects.Columns.ModuleName]).Trim(), Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]).Trim()));
                string func = string.Format("openResourceAllocationDialog('{0}','{1}','{2}')", absoluteUrlEdit, "Resource Utilization", Server.UrlEncode(Request.Url.AbsolutePath));
                string aHref = UGITUtility.GetHrefFromATagString(Convert.ToString(row["ProjectNameLink"]));

                string cell = e.Row.ClientID + LeadIndex;
                if (string.IsNullOrEmpty(TicketID))
                {
                    if (!string.IsNullOrEmpty(cmicno))
                        e.Row.Cells[1].Text = $"<div id='LeadCell_{cell}' runat='server' onmouseover='ShowLeadTooltip(this,\"{ProjectLeadUser}\",\"{LeadEstimatorUser}\",\"{ProjectManagerUser}\")' onmouseout='HideEditImage(this)' style='float:right; width:380px;' >" +
                                $"<div class='glabel1' onClick='{aHref}'><div>{companyname} {erpLabel}</div> <div class='{cmicnoLabel}'>&nbsp;{cmicno}</div></div>" +
                                $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                                $"</div>";
                    else
                        e.Row.Cells[1].Text = $"<div id='LeadCell_{cell}' runat='server' onmouseover='ShowLeadTooltip(this,\"{ProjectLeadUser}\",\"{LeadEstimatorUser}\",\"{ProjectManagerUser}\")' onmouseout='HideEditImage(this)' style='float:right; width:380px;' >" +
                                $"<div class='glabel1' onClick='{aHref}'>{companyname}</div>" +
                                $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                                $"</div>";
                }
                else
                {
                    e.Row.Cells[1].Text = $"<div style='float:right; width:380px;' ><div class='glabel1'>{subworkitem}</div></div>";
                }

                if (!string.IsNullOrEmpty(cmicno))
                    e.Row.Cells[1].Text = $"<div id='LeadCell_{cell}' runat='server' onmouseover='ShowLeadTooltip(this,\"{ProjectLeadUser}\",\"{LeadEstimatorUser}\",\"{ProjectManagerUser}\")' onmouseout='HideEditImage(this)' style='float:right; width:300px;' >" +
                            $"<div class='glabel1' onClick='{aHref}'><div>{companyname} {erpLabel}</div> <div class='{cmicnoLabel}'>&nbsp;{cmicno}</div></div>" +
                            $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                            $"</div>";
                else
                    e.Row.Cells[1].Text = $"<div id='LeadCell_{cell}' runat='server' onmouseover='ShowLeadTooltip(this,\"{ProjectLeadUser}\",\"{LeadEstimatorUser}\",\"{ProjectManagerUser}\")' onmouseout='HideEditImage(this)' style='float:right; width:300px;' >" +
                            $"<div class='glabel1' onClick='{aHref}'>{companyname}</div>" +
                            $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                            $"</div>";

                LeadIndex++;

            }
        }
        private DataTable LoadAllocationMonthlyView()
        {
            ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            return allocationMonthlyManager.LoadAllocationMonthlyView(Convert.ToDateTime(dateFrom),
                Convert.ToDateTime(dateTo), true);

        }

        private DataTable LoadAllocationWeeklySummaryView()
        {
            try
            {
                DateTime dtFrom = dateFrom;
                DateTime dtTo = dateTo;

                string commQuery = string.Empty;
                ResourceUsageSummaryWeekWiseManager allocationWeekWiseManager = new ResourceUsageSummaryWeekWiseManager(context);
                commQuery = string.Format("{0} >= '{1}' AND {0} <= '{2}'", DatabaseObjects.Columns.WeekStartDate, Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo));

                DataTable dtAllocationWeekWise = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceUsageSummaryWeekWise, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'" + "AND " + commQuery);
                return dtAllocationWeekWise;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return null;
        }
        private DataTable GetAllocationData()
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
            data.Columns.Add(DatabaseObjects.Columns.AllocationID, typeof(string));
            data.Columns.Add("ProjectNameLink", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Closed, typeof(bool));
            data.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ModuleRelativePagePath, typeof(string));
            data.Columns.Add("ExtendedDate", typeof(string));
            data.Columns.Add("ExtendedDateAssign", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.SubWorkItem, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ERPJobID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ContactLookup, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.CRMCompanyLookup, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.PreconStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PreconEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionStart, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionEnd, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.CloseoutDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.SoftAllocation, typeof(bool));
            data.Columns.Add("NCO", typeof(string));
            data.Columns.Add("AllStartDates", typeof(string));
            data.Columns.Add("AllEndDates", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.OnHold, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ChanceOfSuccess, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ProjectLeadUser, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.LeadEstimatorUser, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.TicketProjectManager, typeof(string));
            DataTable dtlist = new DataTable();

            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                if (string.IsNullOrEmpty(hndYear.Value))
                {
                    lblSelectedYear.Text = DateTime.Now.Year.ToString();
                    hndYear.Value = lblSelectedYear.Text;
                }
                dateFrom = new DateTime(Convert.ToInt16(hndYear.Value), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }

            if (hdndisplayMode.Value == "Weekly")
            {
                string selectedMonth = UGITUtility.ObjectToString(Request["selectedDate"]);
                if (string.IsNullOrWhiteSpace(selectedMonth) || !IsPostBack)
                {
                    selectedMonth = DateTime.Now.ToString("MMM-dd-yy");
                    SetCookie("selectedDate", selectedMonth);
                }

                string dateFormat = "MMM-dd-yy";

                DateTime parsedDate = DateTime.ParseExact(selectedMonth, dateFormat, null);
                DateTime oneMonthBefore = parsedDate;
                DateTime oneMonthAfter = parsedDate.AddMonths(2);

                dateFrom = new DateTime(oneMonthBefore.Year, oneMonthBefore.Month, 1);
                dateTo = new DateTime(oneMonthAfter.Year, oneMonthAfter.Month, DateTime.DaysInMonth(oneMonthAfter.Year, oneMonthAfter.Month));

            }
            else
            {

                int selectedYear = UGITUtility.StringToInt(hndYear.Value);
                dateFrom = new DateTime(selectedYear, 1, 1);
                dateTo = new DateTime(selectedYear + 1, 1, 1);

            }
            dtlist = ResourceAllocationManager.GetDatelist(hdndisplayMode.Value, dateFrom, dateTo);
            data.Merge(dtlist);
            data.Clear();

            //bool containsModules = false;

            List<UserProfile> lstUProfile = new List<UserProfile>();

            bool limitedUsers = false;
            //bool isTicketData = false;

            lstUProfile = ObjUserProfileManager.GetUsersProfile();


            if (!string.IsNullOrEmpty(SelectedUser) && Request["RequestFromProjectAllocation"] == null)
            {
                limitedUsers = true;
                SelectedUsers = SelectedUser;
            }
            else
            {
                if (Request["SelectedUsers"] != null)
                {
                    SelectedUsers = Request["SelectedUsers"].ToString();
                    limitedUsers = true;
                    UGITUtility.CreateCookie(Response, "SelectedUser", Request["SelectedUsers"].ToString());
                }
                else if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "SelectedUser")))
                {
                    SelectedUsers = UGITUtility.GetCookieValue(Request, "SelectedUser");
                    limitedUsers = true;
                }
            }

            if (!string.IsNullOrEmpty(TicketID))
            {
                limitedUsers = false;
                //isTicketData = true;
            }

            DataTable dtResult = null;
            DataTable workitems = null;
            List<string> userIds = new List<string>();

            //// temp code
            //limitedUsers = true;
            //SelectedUsers = "f6150d04-0ccd-4417-94df-52c29f0e9d74"; //budke
            if (limitedUsers)
            {

                if ((!string.IsNullOrEmpty(SelectedUsers) && SelectedUsers != "null") || !string.IsNullOrEmpty(UGITUtility.ObjectToString(UGITUtility.GetCookieValue(Request, "SelectedUser"))))
                {
                    if (string.IsNullOrEmpty(SelectedUsers))
                        SelectedUsers = UGITUtility.GetCookieValue(Request, "SelectedUser");
                    userIds = UGITUtility.ConvertStringToList(SelectedUsers, Constants.Separator6);
                    userIds.Distinct();
                    //List<UserProfile> lstSelectedUsers = ObjUserProfileManager.GetUserInfosById(SelectedUsers);
                    //userIds = lstSelectedUsers.Select(x => x.Id).ToList();
                }
                else
                {
                    userIds = lstUProfile.Select(x => x.Id).ToList();
                }
                dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 4, dateFrom, dateTo);
                workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 4);

            }
            else
            {
                dtResult = ResourceAllocationManager.LoadRawTableByResource(null, 4, dateFrom, dateTo);
                workitems = RMMSummaryHelper.GetOpenworkitems(context, chkIncludeClosed.Checked);
            }


            if (!string.IsNullOrEmpty(TicketID))
            {
                DataRow[] ticketRows = dtResult.Select("TicketID = " + TicketID);
                dtResult = ticketRows.CopyToDataTable();

                DataRow[] workitemRows = workitems.Select($"{DatabaseObjects.Columns.WorkItem} = {TicketID}");
                workitems = workitemRows.CopyToDataTable();
            }

            //Load Tickets object before loop to improve perfromance
            TicketManager objTicketManager = new TicketManager(context);
            DataTable dtAllModuleTickets = objTicketManager.GetAllProjectTickets();

            if (dtAllModuleTickets?.Rows?.Count > 0)
            {
                DataRow[] dr = dtAllModuleTickets.AsEnumerable().Where(x => !(x.Field<string>(DatabaseObjects.Columns.Status) == "Cancelled" || (x.Field<string>(DatabaseObjects.Columns.TicketId).StartsWith("OPM") && x.Field<bool>(DatabaseObjects.Columns.Closed)))).ToArray();
                if (dr != null && dr.Length > 0)
                    dtAllModuleTickets = dr.CopyToDataTable();
                else
                    dtAllModuleTickets.Rows.Clear();
            }

            bool includeCloseProject = !string.IsNullOrWhiteSpace(Request["includeClosed"])
                && UGITUtility.StringToBoolean(UGITUtility.ObjectToString(Request["includeClosed"])) ? true : false;

            if (!includeCloseProject)
            {
                // Filter by Open Tickets.
                List<string> LstOpenTicketIds = dtAllModuleTickets?.AsEnumerable()?.Where(x => x.Field<bool>(DatabaseObjects.Columns.Closed) == false)?.Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId))?.ToList() ?? new List<string>();
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataRow[] dr = dtResult.AsEnumerable().Where(x => LstOpenTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId), StringComparer.OrdinalIgnoreCase)).ToArray();
                    if (dr != null && dr.Length > 0)
                        dtResult = dr.CopyToDataTable();
                    else
                        dtResult.Rows.Clear();
                }
            }

            if (dtResult == null)
                return data;

            var allocationGroupData = dtResult.AsEnumerable()
                //.Where(x => x.Field<string>("ResourceId") == "f6150d04-0ccd-4417-94df-52c29f0e9d74")   //testing code
                .GroupBy(row => new
                {
                    Id = row.Field<string>("WorkItem"),
                    ResourceId = row.Field<string>("ResourceId"),
                    SubWorkItem = row.Field<string>("SubWorkItem")
                })
                .Select(group => new AllocationData()
                {
                    ResourceId = group.Key.ResourceId,
                    WorkItem = group.Key.Id,
                    SubWorkItem = group.Key.SubWorkItem,
                    // Add other aggregated properties here
                    AllocationID = string.Join(",", group.Select(row => row.Field<long?>("ID") + ":" + UGITUtility.ChangeDateFormat(row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue, "MM/dd/yyyy") + ":" + UGITUtility.ChangeDateFormat(row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue, "MM/dd/yyyy"))),
                    ResourceUser = group.First().Field<string>("ResourceUser"),
                    WorkItemType = group.First().Field<string>("WorkItemType"),
                    WorkItemLink = group.First().Field<string>("WorkItemLink"),
                    AllocationStartDate = group.Min(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue),
                    AllocationEndDate = group.Max(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue),
                    PctAllocation = group.Average(row => row.Field<double>("PctAllocation")),
                    WorkItemID = string.Join(",", group.Select(row => row.Field<long>("WorkItemID")).Distinct().ToList()),
                    //Title = group.First().Field<string>("Title"),
                    ShowEditButton = false, // group.Select(row => row.Field<bool?>("ShowEditButton")).FirstOrDefault() ?? false,
                    ShowPartialEdit = false, //group.Select(row => row.Field<bool?>("ShowPartialEdit")).FirstOrDefault() ?? false,
                    PlannedStartDate = group.Min(row => row.Field<DateTime?>("PlannedStartDate") ?? DateTime.MinValue),
                    PlannedEndDate = group.Max(row => row.Field<DateTime?>("PlannedEndDate") ?? DateTime.MinValue),
                    PctPlannedAllocation = group.Average(row => row.Field<double?>("PctPlannedAllocation") ?? 0),
                    EstStartDate = group.Min(row => row.Field<DateTime?>("EstStartDate") ?? DateTime.MinValue),
                    EstEndDate = group.Max(row => row.Field<DateTime?>("EstEndDate") ?? DateTime.MinValue),
                    PctEstimatedAllocation = group.Average(row => row.Field<double?>("PctEstimatedAllocation") ?? 0),
                    SoftAllocation = group.First().Field<bool>("SoftAllocation").ToString(),
                    AllStartDates = string.Join(",", group.Select(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue)),
                    AllEndDates = string.Join(",", group.Select(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue)),
                    OnHold = group.All(o => o[DatabaseObjects.Columns.OnHold].ToString() == "1") ? "1" : "0",
                    ChanceOfSuccess = group.First().Field<string>(DatabaseObjects.Columns.ChanceOfSuccess),
                    ProjectLeadUser = group.First().Field<string>(DatabaseObjects.Columns.ProjectLeadUser),
                    LeadEstimatorUser = group.First().Field<string>(DatabaseObjects.Columns.LeadEstimatorUser),
                    TicketProjectManager = group.First().Field<string>(DatabaseObjects.Columns.TicketProjectManager),
                }).ToArray();

            var allocatedUsers = allocationGroupData.Select(x => x.ResourceId).Distinct();
            var unAllocatedUsers = userIds.Where(x => !allocatedUsers.Contains(x));
            List<UserProfile> unAllocatedUsersData = null;
            if (unAllocatedUsers?.Count() > 0)
            {
                unAllocatedUsersData = userProfiles.Where(x => unAllocatedUsers.Contains(x.Id)).ToList();
            }
           
            DataTable dtAllocationMonthly = null;
            DataTable dtAllocationWeekly = null;

            if (hdndisplayMode.Value == "Weekly")
            {
                dtAllocationWeekly = LoadAllocationWeeklySummaryView();
            }
            else
                dtAllocationMonthly = LoadAllocationMonthlyView();


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

            foreach (var dr in allocationGroupData)
            {
                string userid = Convert.ToString(dr.ResourceId);
                if (string.IsNullOrEmpty(userid))
                    continue;

                userDetails = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(userid));

                if (userDetails == null || !userDetails.Enabled)
                    continue;


                if (string.IsNullOrEmpty(Convert.ToString(dr.WorkItemID)))
                    continue;

                List<string> lstWorkItemIds = UGITUtility.ConvertStringToList(dr.WorkItemID, Constants.Separator6);
                DataRow newRow = data.NewRow();
                newRow[DatabaseObjects.Columns.Id] = userid;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;
                if (btnexport)
                {
                    newRow[DatabaseObjects.Columns.Resource] = userDetails.Name;
                }
                else
                {
                    newRow[DatabaseObjects.Columns.Resource] = Convert.ToString(dr.ResourceUser);
                }

                newRow[DatabaseObjects.Columns.Name] = userDetails.Name;
                newRow[DatabaseObjects.Columns.SoftAllocation] = UGITUtility.StringToBoolean(dr.SoftAllocation);
                newRow[DatabaseObjects.Columns.SubWorkItem] = UGITUtility.ObjectToString(dr.SubWorkItem);
                newRow["AllStartDates"] = UGITUtility.ObjectToString(dr.AllStartDates);
                newRow["AllEndDates"] = UGITUtility.ObjectToString(dr.AllEndDates);
                newRow[DatabaseObjects.Columns.OnHold] = dr.OnHold;
                newRow[DatabaseObjects.Columns.AllocationID] = dr.AllocationID;
                newRow[DatabaseObjects.Columns.ChanceOfSuccess] = uHelper.GetFormattedChanceOfSuccess(dr.ChanceOfSuccess);
                if (!string.IsNullOrEmpty(Convert.ToString(dr.ProjectLeadUser)))
                {
                    newRow[DatabaseObjects.Columns.ProjectLeadUser] = dr.ProjectLeadUser;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dr.LeadEstimatorUser)))
                {
                    newRow[DatabaseObjects.Columns.LeadEstimatorUser] = dr.LeadEstimatorUser;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dr.TicketProjectManager)))
                {
                    newRow[DatabaseObjects.Columns.TicketProjectManager] = dr.TicketProjectManager;
                }

                DataRow drWorkItem = null;
                if (workitems != null && workitems.Rows.Count > 0)
                {
                    DataRow[] filterworkitemrow = workitems.AsEnumerable().Where(x => lstWorkItemIds.Contains(UGITUtility.ObjectToString(x.Field<long>("ID")))).ToArray();
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
                                dataRow = dtAllModuleTickets.AsEnumerable().FirstOrDefault(row => row.Field<string>("TicketId") == workItem);
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
                                    newRow[DatabaseObjects.Columns.ERPJobID] = UGITUtility.ObjectToString(dataRow[DatabaseObjects.Columns.ERPJobID]);
                                    newRow[DatabaseObjects.Columns.CRMCompanyLookup] = UGITUtility.ObjectToString(dataRow["CRMCompanyTitleLookup"]);
                                    if (UGITUtility.StringToBoolean(dataRow["Closed"]))
                                    {
                                        newRow[DatabaseObjects.Columns.Closed] = true;
                                    }
                                    newRow[DatabaseObjects.Columns.PreconStartDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.PreconStartDate]);
                                    newRow[DatabaseObjects.Columns.PreconEndDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.PreconEndDate]);
                                    newRow[DatabaseObjects.Columns.EstimatedConstructionStart] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                    newRow[DatabaseObjects.Columns.EstimatedConstructionEnd] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                                    newRow[DatabaseObjects.Columns.CloseoutDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.CloseoutDate]);                                    
                                    //newRow["NCO"] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.PreconStartDate]);

                                }
                                else
                                {
                                    newRow[DatabaseObjects.Columns.Title] = title;// title;
                                    newRow[DatabaseObjects.Columns.Project] = title;
                                }
                                newRow[DatabaseObjects.Columns.TicketId] = workItem;
                                newRow[DatabaseObjects.Columns.WorkItemID] = dr.WorkItemID; // UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.WorkItemID);

                                //condition for add new column for breakup gantt chart...
                                string plannedStartDate = Convert.ToString(dr.PlannedStartDate);
                                string plannedEndDate = Convert.ToString(dr.PlannedEndDate);

                                string estStartDate = Convert.ToString(dr.EstStartDate);
                                string estEndDate = Convert.ToString(dr.EstEndDate);
                                string expression = $"{DatabaseObjects.Columns.TicketId}='{workItem}' AND {DatabaseObjects.Columns.Id}='{userid}'";
                                if (data != null && data.Rows.Count > 0)
                                {
                                    DataRow[] row = data.Select(expression);

                                    if (!string.IsNullOrEmpty(plannedStartDate) && !string.IsNullOrEmpty(plannedEndDate))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr.PlannedStartDate;
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr.PlannedEndDate;
                                        newRow["ExtendedDateAssign"] = dr.PlannedStartDate + Constants.Separator1 + dr.PlannedEndDate;

                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr.PctPlannedAllocation);
                                    }

                                    if (!string.IsNullOrEmpty(estStartDate) && !string.IsNullOrEmpty(estEndDate) && estEndDate != "01-01-1753 00:00:00")
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;
                                        newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;

                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}' style='color:black;font-weight:800px !important;'>{2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 40));

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;


                                    if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();

                                            if (dttemp != null && dttemp.Length > 0)
                                                ViewTypeAllocation(data, newRow, dttemp);
                                        }
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            foreach (string s in lstWorkItemIds)
                                            {
                                                DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                                if (dttemp != null && dttemp.Length > 0)
                                                    ViewTypeAllocation(data, newRow, dttemp);
                                            }
                                        }
                                    }

                                    data.Rows.Add(newRow);
                                    //}
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(plannedStartDate) && !string.IsNullOrEmpty(plannedEndDate))
                                    {
                                        newRow[DatabaseObjects.Columns.PlannedStartDate] = dr.PlannedStartDate;
                                        newRow[DatabaseObjects.Columns.PlannedEndDate] = dr.PlannedEndDate;
                                        newRow["ExtendedDateAssign"] = dr.PlannedStartDate + Constants.Separator1 + dr.PlannedEndDate;
                                        newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(dr.PctPlannedAllocation);
                                    }

                                    if (!string.IsNullOrEmpty(estStartDate) && !string.IsNullOrEmpty(estEndDate))
                                    {
                                        newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                        newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;
                                        newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;
                                        newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                                    }

                                    newRow["ProjectNameLink"] = string.Format("<a href='{0}' title='{2}' style='color:black;font-weight:800px !important;'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                                           workItem, true, title).NavigateUrl, UGITUtility.TruncateWithEllipsis(title, 40), title ?? string.Empty);

                                    newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                                    newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;

                                    if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                                ViewTypeAllocation(data, newRow, dttemp);

                                        }
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            foreach (string s in lstWorkItemIds)
                                            {
                                                DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                                if (dttemp != null && dttemp.Length > 0)
                                                {
                                                    ViewTypeAllocation(data, newRow, dttemp);
                                                }
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
                        //This condition set to remove PTO types but core only use PTO type other then module type so return for all non module types
                        if (ConfigVariableMGR.GetValueAsBool(ConfigConstants.HidePTOonGantt))
                            continue;

                        if (arrayModule.Length > 0 && !arrayModule.Contains(Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItemType])))
                            continue;

                        if (data != null && data.Rows.Count > 0)
                        {
                            string expression = string.Format("{0}= '{1}' AND {2}='{3}' AND {4}='{5}'", DatabaseObjects.Columns.TicketId, workItem, DatabaseObjects.Columns.Id, userid, DatabaseObjects.Columns.SubWorkItem, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                            DataRow[] row = data.Select(expression);

                            if (row != null && row.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)))
                                {
                                    row[0]["ExtendedDate"] = Convert.ToString(row[0]["ExtendedDate"]) + Constants.Separator + dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(dr.EstStartDate)))
                                        row[0][DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;

                                    if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationEndDate])) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(dr.EstEndDate)))
                                        row[0][DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;
                                }

                                if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                {
                                    foreach (string s in lstWorkItemIds)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, row[0], dttemp);
                                    }
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();

                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                ViewTypeAllocation(data, row[0], dttemp, false);
                                            }
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
                                newRow[DatabaseObjects.Columns.WorkItemID] = dr.WorkItemID;  // UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.WorkItemID);

                                if (!string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)))
                                {
                                    newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                    newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;

                                    newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;
                                    newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                                }

                                if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                {
                                    foreach (string s in lstWorkItemIds)
                                    {
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                            ViewTypeAllocation(data, newRow, dttemp);
                                    }
                                }
                                else
                                {
                                    if (dtAllocLookups != null)
                                    {
                                        foreach (string s in lstWorkItemIds)
                                        {
                                            DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();

                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                ViewTypeAllocation(data, newRow, dttemp, false);
                                            }
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
                            newRow[DatabaseObjects.Columns.WorkItemID] = dr.WorkItemID;  // UGITUtility.StringToInt(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.WorkItemID);
                            newRow["ProjectNameLink"] = workItem;

                            if (!string.IsNullOrEmpty(Convert.ToString(dr.EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(dr.EstEndDate)))
                            {
                                newRow[DatabaseObjects.Columns.AllocationStartDate] = dr.EstStartDate;
                                newRow[DatabaseObjects.Columns.AllocationEndDate] = dr.EstEndDate;

                                newRow["ExtendedDate"] = dr.EstStartDate + Constants.Separator1 + dr.EstEndDate;
                                newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(dr.PctEstimatedAllocation);
                            }                            
                            if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                            {
                                foreach (string s in lstWorkItemIds)
                                {
                                    DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                    if (dttemp != null && dttemp.Length > 0)
                                        ViewTypeAllocation(data, newRow, dttemp);
                                }
                            }
                            else
                            {
                                if (dtAllocLookups != null)
                                {
                                    foreach (string s in lstWorkItemIds)
                                    {
                                        DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            ViewTypeAllocation(data, newRow, dttemp, false);

                                        }
                                    }
                                }
                            }

                            data.Rows.Add(newRow);
                        }
                    }
                }
                else
                {
                    newRow[DatabaseObjects.Columns.WorkItemID] = dr.WorkItemID;
                    data.Rows.Add(newRow);
                }
            }

            if (unAllocatedUsersData?.Count() > 0)
            {
                foreach (UserProfile user in unAllocatedUsersData)
                {
                    DataRow newRow = data.NewRow();
                    bool rowHasData = false;
                    newRow[DatabaseObjects.Columns.Id] = user.Id;
                    newRow[DatabaseObjects.Columns.ItemOrder] = 1;
                    newRow[DatabaseObjects.Columns.Resource] = user.Name;
                    newRow[DatabaseObjects.Columns.Name] = user.Name;
                    newRow[DatabaseObjects.Columns.WorkItemID] = user.Id;

                    for (DateTime dt = dateFrom; dateTo >= dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                    {
                        if ((user.UGITStartDate <= dt && dt < user.UGITEndDate))
                        {
                            if (data.Columns.Contains(dt.ToString("MMM-dd-yy") + "E"))
                            {
                                newRow[dt.ToString("MMM-dd-yy") + "E"] = "";
                                rowHasData = true;
                            }
                        }

                    }
                    if (rowHasData)
                        data.Rows.Add(newRow);
                }
            }
            #endregion
            data.DefaultView.Sort = string.Format("{0} ASC ,{1} ASC, {2} ASC", DatabaseObjects.Columns.Name, DatabaseObjects.Columns.Closed, DatabaseObjects.Columns.Project);
            return data;
        }

        private void ViewTypeAllocation(DataTable data, DataRow newRow, DataRow[] dttemp, bool Assigned = true)
        {
            //double yearquaAllocE = 0;
            //double yearquaAllocA = 0;

            //double halfyearquaAllocE1 = 0;
            //double halfyearquaAllocE2 = 0;
            //double halfyearquaAllocA1 = 0;
            //double halfyearquaAllocA2 = 0;

            //double quaterquaAllocE1 = 0;
            //double quaterquaAllocE2 = 0;
            //double quaterquaAllocE3 = 0;
            //double quaterquaAllocE4 = 0;
            //double quaterquaAllocA1 = 0;
            //double quaterquaAllocA2 = 0;
            //double quaterquaAllocA3 = 0;
            //double quaterquaAllocA4 = 0;

            string workitemcolumn = hdndisplayMode.Value == "Weekly" ? DatabaseObjects.Columns.WorkItemID : DatabaseObjects.Columns.ResourceWorkItemLookup;
            foreach (DataRow rowitem in dttemp)
            {
                string workitem = UGITUtility.ObjectToString(rowitem[workitemcolumn]);
                string monthlyPct = UGITUtility.ObjectToString(rowitem[DatabaseObjects.Columns.PctAllocation]);
                string softAlloc = UGITUtility.ObjectToString(rowitem[DatabaseObjects.Columns.SoftAllocation]);
                string actualMonthlyPct = UGITUtility.ObjectToString(rowitem[DatabaseObjects.Columns.ActualPctAllocation]);
                DateTime AllocationStartDate = UGITUtility.StringToDateTime(rowitem[DatabaseObjects.Columns.ActualStartDate]);
                DateTime AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                DateTime allocationEndDate = UGITUtility.StringToDateTime(rowitem[DatabaseObjects.Columns.ActualEndDate]);

                if (hdndisplayMode.Value == "Weekly")
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        List<long> allocationID = uHelper.SplitAllocationIdIntoTuple(UGITUtility.ObjectToString(newRow[DatabaseObjects.Columns.AllocationID]), Convert.ToDateTime(rowitem[DatabaseObjects.Columns.ActualStartDate]), Convert.ToDateTime(rowitem[DatabaseObjects.Columns.ActualEndDate]));

                        if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                        {
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] =
                                Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);

                            if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "_WorkItem"))
                                data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "_WorkItem", typeof(string));
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "_WorkItem"] = workitem;
                            if (!data.Columns.Contains("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"))
                            {
                                data.Columns.Add("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                            }
                            newRow["ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"]
                                += AllocationStartDate + Constants.Separator6 + allocationEndDate + Constants.Separator6 + workitem + Constants.Separator6 + softAlloc + Constants.Separator6 + actualMonthlyPct + ";#";

                            if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"))
                                data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct", typeof(string));
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"] += monthlyPct + Constants.Separator6;

                            if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_SoftAllocation"))
                                data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_SoftAllocation", typeof(string));
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_SoftAllocation"] += softAlloc + Constants.Separator6;

                            if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_AllocationID"))
                                data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_AllocationID", typeof(string));
                            newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_AllocationID"] += string.Join(",", allocationID) + Constants.Separator6;
                        }
                    }
                    
                }
                else
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        string datename = UGITUtility.ObjectToString(rowitem[DatabaseObjects.Columns.MonthStartDate]);
                        List<long> allocationID = uHelper.SplitAllocationIdIntoTuple(UGITUtility.ObjectToString(newRow[DatabaseObjects.Columns.AllocationID]), Convert.ToDateTime(rowitem[DatabaseObjects.Columns.ActualStartDate]), Convert.ToDateTime(rowitem[DatabaseObjects.Columns.ActualEndDate]));

                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) >= AllocationMonthStartDate && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) <= allocationEndDate)
                        {
                            if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                            {
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] =
                                    Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);
                                if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "_WorkItem"))
                                    data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "_WorkItem", typeof(string));
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "_WorkItem"] = workitem;

                                if (!data.Columns.Contains("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                                {
                                    data.Columns.Add("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                                }
                                newRow["ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] 
                                    += AllocationStartDate + Constants.Separator6 + allocationEndDate + Constants.Separator6 + workitem + Constants.Separator6 + softAlloc + Constants.Separator6 + actualMonthlyPct + ";#";

                                if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"))
                                    data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct", typeof(string));
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"] += monthlyPct + Constants.Separator6;

                                if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_SoftAllocation"))
                                    data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_SoftAllocation", typeof(string));
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_SoftAllocation"] += softAlloc + Constants.Separator6;

                                if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_AllocationID"))
                                    data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_AllocationID", typeof(string));
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_AllocationID"] += string.Join(",", allocationID) + Constants.Separator6;

                            }
                        }
                    }
                    

                }

            }

        }

        private void PrepareAllocationGrid()
        {
            gvPreview.Columns.Clear();
            gvPreview.GroupSummary.Clear();
            gvPreview.TotalSummary.Clear();
           
            int noOfColumns = 13;
            if (hdndisplayMode.Value == "Weekly")
            {
                for (DateTime dt = dateFrom; dateTo.AddDays(1) > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    if (dateTo == dt)
                    {
                        noOfColumns = 14;
                    }
                }
            }
            else {
                noOfColumns = 12;
            }
            int fullScreenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "screenWidth")) * 92 / 100;
            int minusWidth = fullScreenWidth * 23 / 100;

            if (fullScreenWidth > 0)
            {
                int remainingwidthforcols = fullScreenWidth - minusWidth;
                MonthColWidth = UGITUtility.StringToInt(remainingwidthforcols / noOfColumns);
            }

            gvPreview.Templates.GroupRowContent = new GridGroupRowContentTemplateNew(userEditPermisionList, isResourceAdmin, rbtnPercentage.Checked, chkIncludeClosed.Checked, selectedCategory, MonthColWidth, fullScreenWidth);
            gvPreview.SettingsPager.AlwaysShowPager = false;
            gvPreview.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
           

            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Resource;
            colId.Caption = DatabaseObjects.Columns.Resource;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.Cursor = "pointer";
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            //colId.Width = new Unit("200px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.GroupIndex = 0;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Name;
            colId.Caption = DatabaseObjects.Columns.Name;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.Visible = false;
            colId.HeaderStyle.Cursor = "pointer";
            //colId.SortOrder = ColumnSortOrder.Ascending;
            colId.Settings.SortMode = ColumnSortMode.Custom;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Id;
            colId.Caption = DatabaseObjects.Columns.Id;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("300px");
            colId.HeaderStyle.Cursor = "pointer";
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            colId.Visible = false;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Title;
            colId.HeaderCaptionTemplate = new GridTitleHeaderTemplate(UGITUtility.ObjectToString(Request["SelectedUser"]));
            colId.Caption = "";
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit(minusWidth.ToString() + "px");
            colId.HeaderStyle.Cursor = "pointer";
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            colId.FixedStyle = GridViewColumnFixedStyle.Left;
            gvPreview.Columns.Add(colId);

            gvPreview.TotalSummary.Add(SummaryItemType.Custom, DatabaseObjects.Columns.Title);

            var IsShowTotalCapicityFTE = ConfigVariableMGR.GetValueAsBool(ConfigConstants.ShowTotalCapicityFTE);
            if (IsShowTotalCapicityFTE)
            {
                ASPxSummaryItem item = new ASPxSummaryItem(DatabaseObjects.Columns.Title, SummaryItemType.Custom);
                item.Tag = "ResourceItem";

                gvPreview.TotalSummary.Add(item);
            }

            GridViewBandColumn bdCol = new GridViewBandColumn();
            bdCol.HeaderStyle.Cursor = "pointer";
            string currentDate = string.Empty;

            DateTime dateT = hdndisplayMode.Value == "Weekly" ? Convert.ToDateTime(dateTo).AddDays(1) : Convert.ToDateTime(dateTo);

            for (DateTime dt = dateFrom; dateT > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
            {
                if (hdndisplayMode.Value == "Weekly")
                {
                    if (dt.ToString("MMM-yy") != currentDate && !string.IsNullOrEmpty(currentDate))
                    {
                        bdCol.HeaderStyle.Cursor = "pointer";
                        gvPreview.Columns.Add(bdCol);
                        bdCol = new GridViewBandColumn();
                    }

                    if (dt.ToString("MMM-yy") != currentDate)
                    {
                        bdCol.Caption = dt.ToString("MMM-yy");
                        bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        bdCol.HeaderStyle.Font.Bold = true;
                        bdCol.HeaderStyle.Cursor = "pointer";
                        bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol, hdndisplayMode.Value);
                        currentDate = dt.ToString("MMM-yy");
                    }
                }
                else
                {
                    if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                    {
                        bdCol.HeaderStyle.Cursor = "pointer";
                        gvPreview.Columns.Add(bdCol);
                        bdCol = new GridViewBandColumn();
                    }

                    if (dt.ToString("yyyy") != currentDate)
                    {
                        bdCol.Caption = dt.ToString("yyyy");
                        SetCookie("year", dt.ToString("yyyy"));
                        bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        bdCol.HeaderStyle.Font.Bold = true;
                        bdCol.HeaderStyle.Cursor = "pointer";
                        bdCol.HeaderTemplate = new CommandGridViewBandColumn(bdCol, hdndisplayMode.Value);
                        currentDate = dt.ToString("yyyy");
                    }
                }

                GridViewDataTextColumn ColIdData = new GridViewDataTextColumn();
                if (hdndisplayMode.Value == "Weekly")
                {
                    if (dt.DayOfWeek != DayOfWeek.Monday)
                        continue;
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("dd-MMM");
                }
                else if (hdndisplayMode.Value == "Quarterly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode.Value == "HalfYearly")
                {
                    ColIdData.FieldName = dt.ToString("MMM-dd-yy") + "E";
                    ColIdData.Caption = dt.ToString("MMM");
                }
                else if (hdndisplayMode.Value == "Yearly")
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
                ColIdData.HeaderStyle.Cursor = "pointer";

                ColIdData.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.HeaderTemplate = new CommandColumnHeaderTemplateNew(ColIdData);

                ColIdData.Width = new Unit(MonthColWidth + "px");
                ColIdData.ExportWidth = 38;


                CreateGridSummaryColumn(gvPreview, dt.ToString("MMM-dd-yy") + "E");

                bdCol.HeaderStyle.Cursor = "pointer";
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
            bdCol.HeaderStyle.Cursor = "pointer";
            gvPreview.Columns.Add(bdCol);

            if (Height != null && Height.Value > 0)
            {
                gvPreview.Settings.VerticalScrollableHeight = Convert.ToInt32(Height.Value - 260);
                gvPreview.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
        }

        private void CreateGridSummaryColumn(DevExpress.Web.ASPxGridView gvPreview, string column)
        {
            ASPxSummaryItem summary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Custom);
            summary.ShowInGroupFooterColumn = column;
            summary.DisplayFormat = "{0}";
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

        protected void gvPreview_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(HttpContext.Current.GetManagerContext());
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
            ResourceWorkItemsManager workItemsManager = new ResourceWorkItemsManager(HttpContext.Current.GetManagerContext());
            ProjectEstimatedAllocationManager CRMProjectAllocationManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
            ProjectEstimatedAllocation cRMProjectAllocation = new ProjectEstimatedAllocation();

            List<long> workItemsId = new List<long>();
            int workindHrsInDay = uHelper.GetWorkingHoursInADay(context);
            RResourceAllocation resAllocation = null;
            long workItemID = 0;
            long allocationID = 0;
            string[] ids = new string[0];

            foreach (var args in e.UpdateValues)
            {
                ids = (Convert.ToString(args.Keys[0])).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Length != 2)
                {
                    //Log.WriteLog("gvPreview_BatchUpdate: gridview is not correctly updated");
                    continue;
                }
                workItemID = UGITUtility.StringToLong(ids[0]);
                allocationID = UGITUtility.StringToLong(ids[1]);

                //don't need go further
                if (workItemID == 0 || allocationID == 0)
                    continue;

                resAllocation = allocationManager.Get(allocationID);

                if (resAllocation.ResourceWorkItemLookup > 0)
                {
                    resAllocation.ResourceWorkItems = workItemsManager.Get(resAllocation.ResourceWorkItemLookup);
                }

                #region CPR Integration
                if (resAllocation.ResourceWorkItems != null)
                {
                    if (resAllocation.ResourceWorkItems.WorkItemType == "CPR" || resAllocation.ResourceWorkItems.WorkItemType == "OPM" || resAllocation.ResourceWorkItems.WorkItemType == "CNS")
                    {
                        string query = string.Format(" where TicketId = '{0}' AND AssignedTo = '{1}' AND AllocationStartDate = '{2}' AND AllocationEndDate = '{3}'", resAllocation.ResourceWorkItems.WorkItem, resAllocation.Resource, resAllocation.AllocationStartDate, resAllocation.AllocationEndDate);

                        cRMProjectAllocation = CRMProjectAllocationManager.Get(query);

                    }
                }
                #endregion
                double totalWorkingHrs = 0;
                double allocatedHrs = 0;

                ////fetch monthly allocations
                List<ResourceAllocationMonthly> monthAllocDBData = allocationMonthlyManager.Load(x => x.ResourceWorkItemLookup == workItemID).ToList();

                //update or create monthly allocations
                double pctAllocation = 0;
                for (DateTime dt = dateFrom; dateTo >= dt;)
                {
                    pctAllocation = UGITUtility.StringToDouble(Convert.ToString(args.NewValues[dt.ToString("MMM-dd-yy") + "E"]));
                    ResourceAllocationMonthly dRow = null;
                    if (monthAllocDBData != null && monthAllocDBData.Count > 0)
                    {
                        dRow = monthAllocDBData.FirstOrDefault(x => x.MonthStartDate == dt.Date);  // monthAllocDBData.AsEnumerable().FirstOrDefault(x => x.Field<DateTime>(DatabaseObjects.Columns.MonthStartDate).Date == dt.Date);

                        //If allocation is save then don't need to update it
                        if (dRow != null && pctAllocation == UGITUtility.StringToDouble(dRow.PctAllocation))
                        {
                            dt = dt.AddMonths(1);
                            continue;
                        }
                    }

                    if (dRow == null)
                    {
                        //don't add new entry if pct allocation is zero against it.
                        if (pctAllocation == 0)
                        {
                            dt = dt.AddMonths(1);
                            continue;
                        }

                        dRow = new ResourceAllocationMonthly();
                        dRow.MonthStartDate = dt;
                        dRow.ResourceWorkItemLookup = workItemID;
                        dRow.Resource = resAllocation.Resource;    //.ResourceId.ToString();
                        dRow.PctAllocation = pctAllocation;
                        allocationMonthlyManager.Save(dRow);
                        monthAllocDBData.Add(dRow);
                    }
                    else
                    {
                        dRow.PctAllocation = pctAllocation;
                        allocationMonthlyManager.Save(dRow);
                        dt = dt.AddMonths(1);
                    }
                }

                #region Update Allocation based on monthly entries
                //Calculate total working hours and actual hours allocation based on month wise data
                pctAllocation = 0;
                foreach (ResourceAllocationMonthly sRow in monthAllocDBData)
                {
                    pctAllocation = UGITUtility.StringToDouble(sRow.PctAllocation);
                    if (pctAllocation == 0)
                        continue;

                    DateTime sDate = UGITUtility.StringToDateTime(sRow.MonthStartDate);
                    DateTime eDate = new DateTime(sDate.Year, sDate.Month, DateTime.DaysInMonth(sDate.Year, sDate.Month));
                    int workingDaysInMonth = uHelper.GetTotalWorkingDaysBetween(context, sDate.Date, eDate.Date);
                    int totalHrsInMonth = workindHrsInDay * workingDaysInMonth;
                    totalWorkingHrs += totalHrsInMonth;
                    allocatedHrs += (pctAllocation * totalHrsInMonth) / 100;
                }


                List<ResourceAllocationMonthly> dRows = monthAllocDBData.Where(x => x.PctAllocation != null && x.PctAllocation > 0).ToList();
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;
                if (dRows.Count() > 0)
                {
                    startDate = dRows.Min(x => x.MonthStartDate).GetValueOrDefault();
                    endDate = dRows.Max(x => x.MonthStartDate).GetValueOrDefault();
                }

                //keep enddate and startdate equal to year start date if allocation don't have any value in months.
                if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
                {
                    startDate = dateFrom; endDate = dateFrom;
                }
                endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));


                if (resAllocation.AllocationStartDate.Value.Year == startDate.Year && resAllocation.AllocationStartDate.Value.Month == startDate.Month)
                    startDate = resAllocation.AllocationStartDate.Value;
                if (resAllocation.AllocationEndDate.Value.Year == endDate.Year && resAllocation.AllocationEndDate.Value.Month == endDate.Month)
                    endDate = resAllocation.AllocationEndDate.Value;

                resAllocation.AllocationStartDate = startDate;
                resAllocation.AllocationEndDate = endDate;
                resAllocation.PctEstimatedAllocation = resAllocation.PctAllocation = 0;
                if (totalWorkingHrs > 0)
                {
                    resAllocation.PctEstimatedAllocation = resAllocation.PctAllocation = Convert.ToInt32((allocatedHrs * 100) / totalWorkingHrs);
                }

                allocationManager.Save(resAllocation, false, true);

                #endregion
                #region CPR Integration
                if (resAllocation.ResourceWorkItems != null && cRMProjectAllocation != null)
                {
                    if (resAllocation.ResourceWorkItems.WorkItemType == "CPR" || resAllocation.ResourceWorkItems.WorkItemType == "OPM" || resAllocation.ResourceWorkItems.WorkItemType == "CNS")
                    {
                        // cRMProjectAllocation.TicketId = Convert.ToString(cbLevel2.Value);
                        cRMProjectAllocation.AllocationStartDate = resAllocation.AllocationStartDate;
                        cRMProjectAllocation.AllocationEndDate = resAllocation.AllocationEndDate;
                        cRMProjectAllocation.AssignedTo = resAllocation.Resource;
                        cRMProjectAllocation.PctAllocation = (double)resAllocation.PctAllocation;
                        //cRMProjectAllocation.Type = Convert.ToString(ddlUserType.Value);
                        //cRMProjectAllocation.Title = txtSubProject.Text;
                        CRMProjectAllocationManager.Update(cRMProjectAllocation);
                    };
                }
                #endregion


                //Delete useless entries
                if (monthAllocDBData != null)
                {
                    //List<ResourceAllocationMonthly> wasteRows = monthAllocDBData.Where(x => (x.PctPlannedAllocation.HasValue || x.PctPlannedAllocation == 0) && (x.PctAllocation.HasValue || x.PctAllocation == 0)).ToList();        //Select("ID > 0 and (PctPlannedAllocation is null or PctPlannedAllocation=0) and (PctAllocation is null or PctAllocation=0)");
                    List<ResourceAllocationMonthly> wasteRows = monthAllocDBData.Where(x => (x.PctPlannedAllocation == null || x.PctPlannedAllocation == 0) && (x.PctAllocation == null || x.PctAllocation == 0)).ToList();//Select("ID > 0 and (PctPlannedAllocation is null or PctPlannedAllocation=0) and (PctAllocation is null or PctAllocation=0)");

                    foreach (ResourceAllocationMonthly dRow in wasteRows)
                    {
                        //spItem = SPListHelper.GetItemByID(monthlyAllocCollection, uHelper.StringToInt(dRow[DatabaseObjects.Columns.Id]));
                        if (dRow != null)
                        {
                            allocationMonthlyManager.Delete(dRow);
                        }
                    }
                }

                //only update resource summary weekly and monthly if pct allocation is non zero.
                //in zero case, saveallocation firing update summary for estimation
                if (resAllocation.ResourceWorkItemLookup > 0 && resAllocation.PctAllocation > 0)
                {
                    workItemsId.Add(resAllocation.ResourceWorkItemLookup);
                }
            }

            if (workItemsId.Count > 0)
            {
                //Start Thread to update rmm summary list w.r.t current workitem
                ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
                ULog.WriteException("Method UpdateRMMAllocationSummary Called Inside Thread In Event gvPreview_BatchUpdate on Page ResourceAllocationGridNew.ascx");
                ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMAllocationSummary(applicationContext, workItemsId); };
                Thread sThread = new Thread(threadStartMethod);
                sThread.IsBackground = true;
                sThread.Start();
            }

            allocationData = null;
            stopToRegerateColumns = true;
            gvPreview.DataBind();

            e.Handled = true;
            gvPreview.CancelEdit();
        }

        protected void chkPercentage_CheckedChanged(object sender, EventArgs e)
        {

            allocationData = null;
            gvPreview.DataBind();
        }

        protected void btnZoomIn_Click(object sender, EventArgs e)
        {
            allocationData = null;
            gvPreview.DataBind();
        }

        protected void btnZoomOut_Click(object sender, EventArgs e)
        {

            allocationData = null;
            gvPreview.DataBind();
        }

        // Variables that store summary values.  
        DateTime dtstartDate;
        DateTime dtEndDate;
        double ResourceFTE;
        double ResourceTotalFTE;
        double pctSum;
        string datePattern = @"^[A-Za-z]{3}-\d{2}-\d{2}[A-Za-z]$";
        protected void gvPreview_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            #region group summary
            if (e.IsGroupSummary)
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    dtstartDate = DateTime.MinValue;
                    dtEndDate = DateTime.MinValue;
                    pctSum = 0;
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

                    if (Regex.IsMatch(item.FieldName, datePattern))
                    {
                        bool softAllocationValue = UGITUtility.StringToBoolean(e.GetValue("SoftAllocation"));
                        if (!softAllocationValue)
                            pctSum += UGITUtility.StringToDouble(e.FieldValue);
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

                    if (Regex.IsMatch(item.FieldName, datePattern))
                    {
                        e.TotalValue = pctSum;
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

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id
                        && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType"
                        && item.FieldName != DatabaseObjects.Columns.AllocationStartDate
                        && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {
                        DataRow row = ((System.Data.DataRowView)e.Row).Row;
                        bool isPTOAlloc = UGITUtility.IfColumnExists(row, DatabaseObjects.Columns.ModuleName)
                            && (row[DatabaseObjects.Columns.ModuleName].ToString() == "Time Off" || row[DatabaseObjects.Columns.ModuleName].ToString() == "Other") ? true : false;
                        if (((DevExpress.Web.ASPxSummaryItemBase)e.Item).Tag == "")
                        {
                            if (UGITUtility.IfColumnExists(((DevExpress.Web.ASPxSummaryItemBase)e.Item).FieldName + "_MonthlyPct", row.Table))
                            {
                                string rowvalue = UGITUtility.ObjectToString(row[((DevExpress.Web.ASPxSummaryItemBase)e.Item).FieldName + "_MonthlyPct"]);
                                string SoftAllocationstring = UGITUtility.ObjectToString(row[((DevExpress.Web.ASPxSummaryItemBase)e.Item).FieldName + "_SoftAllocation"]);
                                List<string> lstSoftAllocation = UGITUtility.ConvertStringToList(SoftAllocationstring, Constants.Separator6);
                                List<string> lstStrings = UGITUtility.ConvertStringToList(rowvalue, Constants.Separator6);
                                double FTE = 0;
                                for(int i = 0; i < lstStrings.Count; i++)
                                {
                                    bool softallocation = UGITUtility.StringToBoolean(lstSoftAllocation[i]);
                                    if (softallocation || isPTOAlloc)
                                        continue;
                                    FTE += UGITUtility.StringToDouble(lstStrings[i]);
                                }

                                ResourceFTE += UGITUtility.StringToDouble(FTE);
                            }
                        }
                            
                    }

                }
                // Finalization.  
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    DevExpress.Web.ASPxSummaryItem item = ((DevExpress.Web.ASPxSummaryItem)e.Item);

                    if (item.FieldName == DatabaseObjects.Columns.Title)
                    {
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "ResourceItem")
                            e.TotalValue = "Total Capacity";
                        else
                            e.TotalValue = "Allocated Demand";
                    }

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType" && item.FieldName != DatabaseObjects.Columns.AllocationStartDate && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {

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

        protected void gvPreview_ClientLayout(object sender, ASPxClientLayoutArgs e)
        {
            if (e.LayoutMode == DevExpress.Web.ClientLayoutMode.Saving)
            {
                UGITUtility.CreateCookie(Response, "AccountGrid", e.LayoutData);
            }
            if (e.LayoutMode == DevExpress.Web.ClientLayoutMode.Loading)
            {
                e.LayoutData = UGITUtility.GetCookieValue(Request, "AccountGrid");
            }
        }


        protected void gvPreview_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.Resource)
            {
                object name1 = e.GetRow1Value(DatabaseObjects.Columns.Name);
                object name2 = e.GetRow2Value(DatabaseObjects.Columns.Name);
                e.Handled = true;
                e.Result = System.Collections.Comparer.Default.Compare(name1, name2);
            }
        }

        protected void chkIncludeClosed_CheckedChanged(object sender, EventArgs e)
        {
        }


        protected void bZoomIn_Click(object sender, EventArgs e)
        {
            allocationData = null;
            gvPreview.DataBind();
        }
        protected void bZoomOut_Click(object sender, EventArgs e)
        {
            allocationData = null;
            gvPreview.DataBind();
        }

        protected void cp_Callback(object sender, CallbackEventArgsBase e)
        {
            if (rbtnPercentage.Checked)
            {
                UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "percentage");
            }
            if (rbtnFTE.Checked)
            {
                UGITUtility.CreateCookie(Response, "filterbarpercentagefte", "fte");
            }
        }

        protected void gvPreview_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                // Retrieve updated data from the data source
                DataTable data = GetAllocationData();
                PrepareAllocationGrid();
                // Bind the updated data to the grid
                gvPreview.DataSource = data;
                gvPreview.DataBind();

                //gvPreview.ExpandAll();

                List<string> collapsedUsers = UGITUtility.GetGanttViewCollapsedUsers(Request);
                List<string> expandedUsers = UGITUtility.GetGanttViewExpandedUsers(Request);

                if (collapsedUsers != null && collapsedUsers.Count == 0
                    && expandedUsers != null && expandedUsers.Count == 0)
                {
                    gvPreview.ExpandAll();
                    return;
                }
                if (collapsedUsers != null && collapsedUsers.Count == 1 && collapsedUsers[0] == "all"
                    && expandedUsers != null && expandedUsers.Count == 0)
                {
                    gvPreview.CollapseAll();
                    return;
                }
                if (expandedUsers != null && expandedUsers.Count == 1 && expandedUsers[0] == "all"
                    && collapsedUsers != null && collapsedUsers.Count == 0)
                {
                    gvPreview.ExpandAll();
                    return;
                }

                if (UGITUtility.IsGridCollapsed(Request))
                {
                    gvPreview.CollapseAll();
                    if (expandedUsers != null && expandedUsers.Count > 0)
                    {

                        foreach (string user in expandedUsers)
                        {
                            int rowCount = gvPreview.VisibleRowCount;

                            for (int i = 0; i < rowCount; i++)
                            {
                                if (user == gvPreview.GetDataRow(i)["Id"].ToString())
                                {
                                    gvPreview.ExpandRow(i);
                                    break;
                                }
                            }
                        }
                    } }
                else
                {
                    gvPreview.ExpandAll();

                    if (collapsedUsers != null && collapsedUsers.Count > 0)
                    {
                        foreach (string user in collapsedUsers)
                        {
                            int rowCount = gvPreview.VisibleRowCount;

                            for (int i = 0; i < rowCount; i++)
                            {
                                if (user == gvPreview.GetDataRow(i)["Id"].ToString())
                                {
                                    gvPreview.CollapseRow(i);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, ex.Message);
            }
        }

        private void SetCookie(string Name, string Value)
        {
            UGITUtility.CreateCookie(Response, Name, Value);
        }
    }

    public class GridGroupRowContentTemplateNew : ITemplate
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
        List<string> lstselectedCategory = null;
        int ScreenWidth = 0;

        public GridGroupRowContentTemplateNew(List<UserProfile> userEditPermisionList, bool isResourceAdmin, bool rbtnPercentage,
            bool IncludeClosedProjects, string selectedCategory, int monthColWidth, int fullScreenWidth = 0)
        {
            MonthColWidth = monthColWidth;
            permisionlist = userEditPermisionList;
            isAdminResource = isResourceAdmin;
            isPercentageMode = rbtnPercentage;
            this.IncludeClosedProjects = IncludeClosedProjects;
            this.lstselectedCategory = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.ScreenWidth = fullScreenWidth;
        }

        protected GridViewGroupRowTemplateContainer Container { get; set; }
        protected DevExpress.Web.ASPxGridView Grid { get { return Container.Grid; } }

        protected Table MainTable { get; set; }
        protected TableRow GroupTextRow { get; set; }
        protected TableRow SummaryTextRow { get; set; }
        private int MonthColWidth { get; set; }
        protected int IndentCount { get { return Grid.GroupCount - GroupLevel - 1; } }
        protected int GroupLevel { get { return Grid.DataBoundProxy.GetRowLevel(Container.VisibleIndex); } }
        protected UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        protected ConfigurationVariableManager ConfigVarManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        //ResourceProjectComplexityManager cpxManager = new ResourceProjectComplexityManager(HttpContext.Current.GetManagerContext());
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
            int fullScreenWidth = ScreenWidth;
            int minusWidth = fullScreenWidth == 0 ? 320 : fullScreenWidth * 23 / 100;
            if (column.Caption == "")
            {
                string strCell = string.Empty;

                user = UserManager.GetUserInfoByIdOrName(Container.GroupText);
                string imageURL = string.IsNullOrWhiteSpace(user.Picture) ? "/Content/Images/userNew.png" : user.Picture;
                isAdminResource = UserManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || UserManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
                string allowAllocationForSelf = ConfigVarManager.GetValue(ConfigConstants.AllowAllocationForSelf);

                if (!isAdminResource)
                {
                    permisionlist = UserManager.LoadAuthorizedUsers(allowAllocationForSelf);
                }

                string appendIcons = "";

                if (isAdminResource)
                {
                    if (user != null)
                    {
                        appendIcons = string.Format("<image style=\"margin-left: 7px; width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue-new.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenAddAllocationPopup('" + user.Id + "', '" + user.Name.Replace("'", "`") + "')\"  />");
                        // appendIcons = appendIcons + $"<img src='/content/images/uparrow_new.png' class='imgCollapse' height='20px' width='28px'></img>";
                    }
                }
                else
                {
                    if (user != null)
                    {
                        if (permisionlist != null && permisionlist.Exists(x => x.Id == user.Id))
                            appendIcons = string.Format("<image style=\"margin-left: 7px;width: 20px ;cursor:pointer;\" src=\"/content/images/plus-blue-new.png\" title='New Allocation' onclick=\"javascript:event.cancelBubble=true; OpenAddAllocationPopup('" + user.Id + "', '" + user.Name.Replace("'", "`") + "')\"  />");
                    }
                }

                if (!string.IsNullOrEmpty(user.JobProfile))
                    strCell = string.Format("<div style='width:{5}px;padding-left:3px;padding-bottom:10px' class='maintitle'><img src='{4}' class='user-image' /><a href='#' onclick='OpenUserResume(\"{3}\")'><div class='nameLabel'>{0}</div></a> {2} <div class='roleLabel'>({1})</div></div>", user.Name, user.JobProfile, appendIcons, user.Id, imageURL, minusWidth);
                else
                    strCell = string.Format("<div style='width:{5}px;padding-left:3px;padding-bottom:10px' class='maintitle'><img src='{4}' class='user-image' /><a href='#' onclick='OpenUserResume(\"{3}\")'><div class='nameLabel'>{0}</div></a> {2}</div>", user.Name, user.JobProfile, appendIcons, user.Id, imageURL, minusWidth);

                cell.Text = strCell;
                return;
            }

            //var dataColumn = column as GridViewDataColumn;
            var dataColumn = column as GridViewDataTextColumn;
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
                string includeNonProjectAllocation = ConfigVarManager.GetValue(ConfigConstants.IncludeNonProjectTime);
                string includeSoftAllocation = ConfigVarManager.GetValue(ConfigConstants.IncludeSoftAllocations);
                
                var childrowscount = Grid.GetChildRowCount(Container.VisibleIndex);
                int projectCount = 0;
                double pctAllocation = 0;
                List<string> lstTicketIds = new List<string>();
                for (int i = 0; i < childrowscount; i++)
                {
                    DataRowView datarow = Grid.GetChildRow(Container.VisibleIndex, i) as DataRowView;
                    if (datarow != null)
                    {
                        GridViewDataTextColumn col = column as GridViewDataTextColumn;
                        DataRow row = datarow.Row;
                        bool isSoftAlloc = false;
                        bool isPTOAlloc = UGITUtility.IfColumnExists(row, DatabaseObjects.Columns.ModuleName)
                            && (row[DatabaseObjects.Columns.ModuleName].ToString() == "Time Off" || row[DatabaseObjects.Columns.ModuleName].ToString() == "Other") ? true : false;
                        List<string> softAllocValues = null;
                        List<string> pctAllocValues = null;
                        if (UGITUtility.IfColumnExists(row, col.FieldName + "_SoftAllocation"))
                        {
                            softAllocValues = row[col.FieldName + "_SoftAllocation"].ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        }
                        if (UGITUtility.IfColumnExists(row, col.FieldName + "_MonthlyPct"))
                        {
                            pctAllocValues = row[col.FieldName + "_MonthlyPct"].ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        }
                        if (softAllocValues != null && softAllocValues.Count() > 0)
                        {
                            isSoftAlloc = !softAllocValues.Any(x => UGITUtility.StringToBoolean(x) == false);
                            if (UGITUtility.IfColumnExists(row, col.FieldName))
                            {
                                double rowpct = UGITUtility.StringToDouble(row[col.FieldName]);
                                if (rowpct > 0)
                                {
                                    if (includeNonProjectAllocation == "True" && isPTOAlloc)
                                    {
                                        if(!lstTicketIds.Contains(row[DatabaseObjects.Columns.TicketId]) && !isPTOAlloc)
                                        {
                                            projectCount++;
                                            lstTicketIds.Add(UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]));
                                        }
                                        else if (isPTOAlloc)
                                            projectCount++;
                                    }
                                    else if (includeSoftAllocation == "True" && isSoftAlloc)
                                    {
                                        projectCount++;
                                    }
                                    else if (!isSoftAlloc && !isPTOAlloc)
                                    {
                                        if (!lstTicketIds.Contains(row[DatabaseObjects.Columns.TicketId]))
                                        {
                                            projectCount++;
                                            lstTicketIds.Add(UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]));
                                        }
                                    }
                                }
                            }
                            int index = 0;
                            foreach (var x in softAllocValues)
                            {
                                double monthlypct = UGITUtility.StringToDouble(pctAllocValues[index].ToString());
                                isSoftAlloc = UGITUtility.StringToBoolean(x);
                                if (UGITUtility.IfColumnExists(row, col.FieldName + "_MonthlyPct"))
                                {
                                    if (monthlypct > 0)
                                    {
                                        if (includeNonProjectAllocation == "True" && isPTOAlloc)
                                        {
                                            pctAllocation += monthlypct;
                                        }
                                        else if (includeSoftAllocation == "True" && isSoftAlloc)
                                        {
                                            pctAllocation += monthlypct;
                                        }
                                        else if (!isSoftAlloc && !isPTOAlloc)
                                        {
                                            pctAllocation += monthlypct;
                                        }
                                    }
                                }
                                index++;
                            }
                        }
                    }
                }

                //100% or more - over allocated, 90% to 100% is near Max, 30% to 80% is available, < 30% is under allocated
                int colorcode = 2;
                if (pctAllocation > 120)
                {
                    colorcode = 7;
                }
                else if (pctAllocation >= 75 && pctAllocation < 120)
                {
                    colorcode = 4;
                }
                else if (pctAllocation >= 40 && pctAllocation < 75)
                {
                    colorcode = 8;
                }
                else if (pctAllocation >= 0 && pctAllocation < 40)
                {
                    colorcode = 9;
                }
                else
                {
                    colorcode = 3;
                }
                //if (pctAllocation >= 100)
                //    colorcode = 6;
                //else if (pctAllocation >= 90 && pctAllocation < 100)
                //    colorcode = 5;
                //else if (pctAllocation >= 30 && pctAllocation < 90)
                //    colorcode = 4;
                //else
                //    colorcode = 3;

                text = $"<div style='width:{MonthColWidth}px;' class='headerColorBox custom-task-color-{colorcode}' >{pctAllocation}% <br>{projectCount} Proj.</div>";
            }
            else
            {
                text = GetGroupSummaryText(summaryItems, column);
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
        protected List<ASPxSummaryItem> FindSummaryItems(GridViewDataTextColumn column)
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

    class GridTitleHeaderTemplate : ITemplate
    {
        string _selectedUser = string.Empty;
        public GridTitleHeaderTemplate(string user)
        {
            _selectedUser = user;
        }
        public void InstantiateIn(Control container)
        {
            string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&SelectedUsersList={3}&Type={2}";
            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, "multiallocationjs", "Add New Allocation", "ResourceAllocation",
                _selectedUser, false));
            HtmlGenericControl div1 = new HtmlGenericControl("DIV");
            div1.Style.Add("float", "left");
            div1.Style.Add("width", "50%");
            ASPxButton link = new ASPxButton();
            GridViewHeaderTemplateContainer gridContainer = (GridViewHeaderTemplateContainer)container;
            link.ID = "aAddItem";
            link.Text = string.Format("New Allocation ");
            link.Width = new Unit("140px");
            link.CssClass = "btn btn-sm db-quickTicket svcDashboard_addTicketBtn";
            link.ImageUrl = "/Content/images/plus-blue-new.png";
            link.Font.Size = new FontUnit("14px");
            link.AutoPostBack = false;
            string windowTitle = "Add Multiple Allocations";
            link.ClientSideEvents.Click = "function(s, e){ window.parent.UgitOpenPopupDialog(\"" + url + "\", '' , \"" + windowTitle + "\", '600px', '700px', 0,'') }";
            //link.ClientSideEvents.Click = string.Format("function(s, e){ window.parent.UgitOpenPopupDialog('{0}','','{1}','680px','500px',0); }", url, "Add Multiple Allocations");
            //div1.Controls.Add(link);

            HtmlGenericControl div2 = new HtmlGenericControl("DIV");
            div2.Style.Add("padding-left", "10px");
            div2.Style.Add("float", "left");
            div2.Style.Add("width", "50%");
            ASPxButton addMultiAllocation = new ASPxButton();
            addMultiAllocation.ID = "btnAddMultiAllocation";
            addMultiAllocation.Text = " Multi Allocation";
            addMultiAllocation.Width = new Unit("140px");
            addMultiAllocation.CssClass = "btn btn-sm db-quickTicket svcDashboard_addTicketBtn";
            addMultiAllocation.ImageUrl = "/Content/images/plus-blue-new.png";
            addMultiAllocation.AutoPostBack = false;
            addMultiAllocation.ClientSideEvents.Click = "btnAddMultiAllocation_Click";
            addMultiAllocation.Font.Size = new FontUnit("14px");
            //div2.Controls.Add(addMultiAllocation);
            container.Controls.Add(div1);
            container.Controls.Add(div2);
        }
    }


    public class CommandColumnHeaderTemplateNew : ITemplate
    {
        GridViewDataTextColumn colID = null;
        public CommandColumnHeaderTemplateNew(GridViewDataTextColumn coID)
        {
            this.colID = coID;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            HyperLink hnkButton = new HyperLink();
            hnkButton.Text = this.colID.Caption;
            container.Controls.Add(hnkButton);
            //if (!string.IsNullOrEmpty(HttpContext.Current.Request["pGroupName"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["ticketId"]))
            ////if (HttpContext.Current.Request["rbtnProject"] == "checked")
            ////{
            string func = string.Format("ClickOnDrillDown(this,'{0}','{1}')", colID.FieldName.TrimEnd(colID.FieldName.Last()), colID.Caption);
            hnkButton.Attributes.Add("onclick", func);
            //}
        }

        #endregion
    }
    public class MonthDownHeaderTemplate : ITemplate
    {
        GridViewDataTextColumn colID = null;
        string _mode = null;
        public MonthDownHeaderTemplate(GridViewDataTextColumn coID, string mode)
        {
            this.colID = coID;
            this._mode = mode;
            _mode = mode;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            HyperLink hnkButton = new HyperLink();
            hnkButton.Text = this.colID.Caption;

            if (this._mode == "Monthly")
            {
                string func = string.Format("ClickOnDrillDown(this,'{0}','{1}')", colID.FieldName, colID.Caption);
                hnkButton.Attributes.Add("onclick", func);
                hnkButton.CssClass = "hand-cursor";
                container.Controls.Add(hnkButton);
            }
            else
            {
                container.Controls.Add(hnkButton);
            }
        }

        #endregion
    }

    public class MonthUpGridViewBandColumn : ITemplate
    {
        GridViewBandColumn colBDC = null;
        string _mode = null;
        public MonthUpGridViewBandColumn(GridViewBandColumn coID, string mode)
        {
            this.colBDC = coID;
            this._mode = mode;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            HtmlGenericControl HContainer = new HtmlGenericControl("Div");
            HyperLink hnkBDButton = new HyperLink();
            hnkBDButton.Style.Add("vertical-align", "top");
            hnkBDButton.Text = this.colBDC.Caption;
            //container.Controls.Add(hnkBDButton);
            string func = string.Format("ClickOnDrillUP(this,'{0}')", colBDC.Caption);
            hnkBDButton.Attributes.Add("onclick", func);

            if (_mode == "Monthly")
            {
                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-right:7px;cursor:pointer;\" src=\"/content/images/back-arrowBlue.png\" onclick=\"ClickOnPrevious()\" class=\"resource-img-gantt\"  />"));
                hnkBDButton.CssClass = "";
                HContainer.Controls.Add(hnkBDButton);
                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-left:7px;cursor:pointer;\" src=\"/content/images/next-arrowBlue.png\" onclick=\"ClickOnNext()\" class=\"resource-img-gantt\"  />"));
                container.Controls.Add(HContainer);
            }
            else
            {
                hnkBDButton.CssClass = "hand-cursor";
                HContainer.Controls.Add(hnkBDButton);
                container.Controls.Add(HContainer);
            }
        }

        #endregion
    }

    public class CommandGridViewBandColumn : ITemplate
    {
        GridViewBandColumn colBDC = null;
        string _mode = null;
        public CommandGridViewBandColumn(GridViewBandColumn coID, string mode)
        {
            this.colBDC = coID;
            this._mode = mode;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            HtmlGenericControl HContainer = new HtmlGenericControl("Div");
            HyperLink hnkBDButton = new HyperLink();
            hnkBDButton.Style.Add("vertical-align", "top");
            hnkBDButton.Text = this.colBDC.Caption;
            //container.Controls.Add(hnkBDButton);
            string func = string.Format("ClickOnDrillUP(this,'{0}')", colBDC.Caption);
            hnkBDButton.Attributes.Add("onclick", func);

            HContainer.Controls.Add(new LiteralControl("<image style=\"padding-right:7px;\" src=\"/content/images/back-arrowBlue.png\" onclick=\"" + string.Format("ClickOnPrevious(this,'{0}','{1}')", colBDC.Caption, _mode) + "\" class=\"resource-img-gantt\"  />"));
            HContainer.Controls.Add(hnkBDButton);
            HContainer.Controls.Add(new LiteralControl("<image style=\"padding-left:7px;\" src=\"/content/images/next-arrowBlue.png\" onclick=\"" + string.Format("ClickOnNext(this,'{0}', '{1}')", colBDC.Caption, _mode) + "\" class=\"resource-img-gantt\"  />"));
            container.Controls.Add(HContainer);
        }

        #endregion
    }

    public class AllocationData
    {
        public string ResourceId { get; set; }
        public string WorkItem { get; set; }
        public string SubWorkItem { get; set; }
        public string AllocationID { get; set; }
        public string ResourceUser { get; set; }
        public string WorkItemType { get; set; }
        public string WorkItemLink { get; set; }
        public DateTime? AllocationStartDate { get; set; }
        public DateTime? AllocationEndDate { get; set; }
        public double? PctAllocation { get; set; }
        public string WorkItemID { get; set; }
        public bool ShowEditButton { get; set; }
        public bool ShowPartialEdit { get; set; }
        public DateTime? PlannedStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public double? PctPlannedAllocation { get; set; }
        public DateTime? EstStartDate { get; set; }
        public DateTime? EstEndDate { get; set; }
        public double? PctEstimatedAllocation { get; set; }
        public string SoftAllocation { get; set; }
        public string AllStartDates { get; set; }
        public string AllEndDates { get; set; }
        public string OnHold { get; set; }
        public string ChanceOfSuccess { get; set; }
        // Add other properties here
        public string ProjectLeadUser { get; set; }
        public string LeadEstimatorUser { get; set; }
        public string TicketProjectManager { get; set; }


        // Constructors
        public AllocationData() { }

        public AllocationData(
            string resourceId,
            string workItem,
            string subWorkItem,
            string allocationID,
            string resourceUser,
            string workItemType,
            string workItemLink,
            DateTime allocationStartDate,
            DateTime allocationEndDate,
            double pctAllocation,
            string workItemID,
            bool showEditButton,
            bool showPartialEdit,
            DateTime plannedStartDate,
            DateTime plannedEndDate,
            double pctPlannedAllocation,
            DateTime estStartDate,
            DateTime estEndDate,
            double pctEstimatedAllocation,
            string softAllocation,
            string allStartDate,
            string allEndDate,
            string onHold,
            string projectLeadUser,
            string leadEstimatorUser,
            string ticketProjectManager
        /* Add other parameters if any */
        )
        {
            ResourceId = resourceId;
            WorkItem = workItem;
            SubWorkItem = subWorkItem;
            AllocationID = allocationID;
            ResourceUser = resourceUser;
            WorkItemType = workItemType;
            WorkItemLink = workItemLink;
            AllocationStartDate = allocationStartDate;
            AllocationEndDate = allocationEndDate;
            PctAllocation = pctAllocation;
            WorkItemID = workItemID;
            ShowEditButton = showEditButton;
            ShowPartialEdit = showPartialEdit;
            PlannedStartDate = plannedStartDate;
            PlannedEndDate = plannedEndDate;
            PctPlannedAllocation = pctPlannedAllocation;
            EstStartDate = estStartDate;
            EstEndDate = estEndDate;
            PctEstimatedAllocation = pctEstimatedAllocation;
            SoftAllocation = softAllocation;
            AllStartDates = allStartDate;
            AllEndDates = allEndDate;
            OnHold = onHold;
            ProjectLeadUser = projectLeadUser;
            LeadEstimatorUser = leadEstimatorUser;
            TicketProjectManager = ticketProjectManager;
            // Initialize other properties using parameters if any
        }
    }
}
