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
using static uGovernIT.Web.ModuleResourceAddEdit;
using DevExpress.Utils.Extensions;
using DevExpress.Office.Utils;

namespace uGovernIT.Web
{
    public partial class CustomProjectTeamGantt : UserControl
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
        public const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";
        
        private DateTime dateFrom;
        private DateTime dateTo;
        public bool btnexport;

        private string selectedCategory = string.Empty;
        private string selectedManager = string.Empty;
        public string SelectedUser = string.Empty;
        private string selecteddepartment = string.Empty;
        //private long selectedfunctionalare = -1;
        List<string> selectedTypes = new List<string>();
        //private string ControlName;
        public string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}";
        public string editAllocationUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=addworkitem&Type=ResourceAllocation&WorkItemType=Time Off");
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
        private ProjectEstimatedAllocationManager ProjectEstimatedAllocationMGR = null;
        //ResourceProjectComplexityManager cpxManager = null;
        //FieldConfigurationManager fieldConfigMgr = null;
        //FieldConfiguration fieldConfig = null;
        //ContactManager ContactManagerMGR = null;

        protected bool IsMobileView
        {
            get
            {
                return UGITUtility.IsMobileWithLandscapeView(Request);
            }
        }

        public int MonthColWidth = 70;
        public int ScrollMonth = 1;
        public bool isEditAllowed = false; 

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            ConfigVariableMGR = new ConfigurationVariableManager(context);
            ObjUserProfileManager = new UserProfileManager(context);
            ResourceAllocationManager = new ResourceAllocationManager(context);
            ResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            ProjectEstimatedAllocationMGR = new ProjectEstimatedAllocationManager(context);
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

            isEditAllowed = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowEditInGantt);
            MultiAddUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "addmultiallocation", "Add Multiple Allocations", SelectedUser, "", IncludeClosed));

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
            //if (ConfigVariableMGR.GetValue(ConfigConstants.AllowAllocationForSelf).EqualsIgnoreCase("View"))
            //    viewself = true;

            int fullScreenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "screenWidth"));
            if(fullScreenWidth > 0)
            {
                int remainingwidthforcols = fullScreenWidth - 380;
                MonthColWidth = UGITUtility.StringToInt( remainingwidthforcols / 14);
            }

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

            #region cookies

            if (!IsPostBack)
            {
                chkIncludeClosed.Checked = true;
                //string filterBarPercentageFTE = UGITUtility.GetCookieValue(Request, "filterbarpercentagefte");

                //if (filterBarPercentageFTE == "fte")
                //    rbtnFTE.Checked = true;
                //else 
                //    rbtnPercentage.Checked = true;

                //string closedfilter = UGITUtility.GetCookieValue(Request, "IncludeClosedAT");
                //if (closedfilter == "true")
                //{
                //    chkIncludeClosed.Checked = true;
                //}
                //else
                //{
                //    chkIncludeClosed.Checked = false;
                //}

            }

            //gvPreview.Templates.GroupRowContent = new GantttGroupRowContentTemplateNew(userEditPermisionList, isResourceAdmin, rbtnPercentage.Checked, chkIncludeClosed.Checked, selectedCategory, MonthColWidth);
            #endregion

            gvPreview.SettingsPager.AlwaysShowPager = false;
            gvPreview.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

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

        protected void gvPreview_DataBinding(object sender, EventArgs e)
        {
            if (allocationData == null)
            {
                try
                {
                    allocationData = GetAllocationData();
                    DateTime minStartDate = allocationData.AsEnumerable().Min(x => x.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate));
                    ScrollMonth = minStartDate.Month;
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
                if (!string.IsNullOrWhiteSpace(value)) { 
                    Estimatecolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                    foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
                }
            }

            if (lstAssignColorsAndFontColors != null && lstAssignColorsAndFontColors.Count > 0)
            {
                string value = Convert.ToString(lstAssignColorsAndFontColors.FirstOrDefault(x => x.Contains(moduleName)));
                Assigncolor = UGITUtility.SplitString(value, Constants.Separator, 1);
                foreColor = UGITUtility.SplitString(value, Constants.Separator, 2);
            }
            if (e.DataColumn.FieldName != DatabaseObjects.Columns.Resource && e.DataColumn.FieldName != "AllocationType" && e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationStartDate && e.DataColumn.FieldName != DatabaseObjects.Columns.AllocationEndDate && e.DataColumn.FieldName != DatabaseObjects.Columns.Title)
            {
                int defaultBarH = 12;
                if (DisablePlannedAllocation)
                    defaultBarH = 24;
                string html;
                DateTime plndD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.PlannedStartDate));
                DateTime estD = UGITUtility.StringToDateTime(e.GetValue(DatabaseObjects.Columns.AllocationStartDate));
                string type = UGITUtility.ObjectToString(e.GetValue("ProjectNameLink"));

                

                string tickId = string.Empty;
                string name = string.Empty;
                DateTime oPreconStart = DateTime.MinValue;
                DateTime oPreconEnd = DateTime.MinValue;
                DateTime oConstStart = DateTime.MinValue;
                DateTime oConstEnd = DateTime.MinValue;
                DateTime oCloseout = DateTime.MinValue;

                DateTime aStartDate = DateTime.MinValue;
                DateTime aEndDate = DateTime.MinValue;
                DateTime aPreconStart = DateTime.MinValue;
                DateTime aPreconEnd = DateTime.MinValue;
                DateTime aConstStart = DateTime.MinValue;
                DateTime aConstEnd = DateTime.MinValue;
                DateTime aCloseout = DateTime.MinValue;
                string id = string.Empty;
                string allocId = string.Empty;
                bool aSoftAllocation = true;
                string project = string.Empty;
                string subworkitem = String.Empty;
                if (e.VisibleIndex >= 0)
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
                    id = row[DatabaseObjects.Columns.Id].ToString();
                    name = !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Name])) ? UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Name]).Replace("'", "`") : string.Empty;
                    allocId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.AllocationID]);
                    subworkitem = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.SubWorkItem]);
                }

                DateTime dateF = Convert.ToDateTime(dateFrom);
                DateTime dateT = Convert.ToDateTime(dateTo);
                for (DateTime dt = dateF; dateT > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    html = "";
                    DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                    string overLappingColumnName = "OverlappingAllocations" + dt.ToString("MMM-dd-yy") + "E";
                    string resourceWorkItemColumnName = "ResourceWorkItemLookup" + dt.ToString("MMM-dd-yy") + "E";
                    string startAndEndDateColumnName = "StartAndEndDates" + dt.ToString("MMM-dd-yy") + "E";
                    string softAllocationColumnName = "SoftAllocation" + dt.ToString("MMM-dd-yy") + "E";
                    string allocationIDColumnName = dt.ToString("MMM-dd-yy") + "E_AllocationID";
                    if (e.DataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                    {
                        string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));
                        
                        string[] pctAllocations = new string[5];
                        string[] tempStartEndDate = new string[5];
                        string[] resourceWorkItems = new string[5];
                        string[] softAllocations = new string[5];
                        string[] allocationIDs = new string[5];

                        if (UGITUtility.IfColumnExists(row, overLappingColumnName))
                        {
                            pctAllocations = row[overLappingColumnName].ToString().Split(',');
                        }

                        if (UGITUtility.IfColumnExists(row, startAndEndDateColumnName))
                        {
                            tempStartEndDate = row[startAndEndDateColumnName].ToString().Split(',');
                        }

                        if (UGITUtility.IfColumnExists(row, resourceWorkItemColumnName))
                        {
                            resourceWorkItems = row[resourceWorkItemColumnName].ToString().Split(',');
                        }

                        if (UGITUtility.IfColumnExists(row, softAllocationColumnName))
                        {
                            softAllocations = row[softAllocationColumnName].ToString().Split(',');
                        }

                        if (UGITUtility.IfColumnExists(row, allocationIDColumnName))
                        {
                            allocationIDs = row[allocationIDColumnName].ToString().Split(',');
                        }

                        List<Tuple<DateTime, DateTime, string, string, string, string>> lstStartAndEndDates = new List<Tuple<DateTime, DateTime, string, string, string, string>>();
                        int index = 0;
                        if (UGITUtility.IfColumnExists(row, DatabaseObjects.Columns.Id) && row[DatabaseObjects.Columns.Id].ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            lstStartAndEndDates.Add(new Tuple<DateTime, DateTime, string, string, string, string>(aStartDate, aEndDate, estAlloc, UGITUtility.ObjectToString(row[DatabaseObjects.Columns.AllocationID]), aSoftAllocation.ToString(), UGITUtility.ObjectToString(allocationIDs.FirstOrDefault(x=>!string.IsNullOrEmpty(x)))));
                        }
                        else
                        {
                            foreach (string str in tempStartEndDate)
                            {
                                if (!string.IsNullOrWhiteSpace(str))
                                {
                                    string[] startEndDate = str.Split('#');
                                    lstStartAndEndDates.Add(new Tuple<DateTime, DateTime, string, string, string, string>(UGITUtility.StringToDateTime(startEndDate[0])
                                        , UGITUtility.StringToDateTime(startEndDate[1])
                                        , pctAllocations[index], resourceWorkItems[index], softAllocations[index], allocationIDs[index]));
                                }
                                index++;
                            }
                        }
                        if (lstStartAndEndDates != null && lstStartAndEndDates.Count > 0)
                        {
                            lstStartAndEndDates = lstStartAndEndDates.OrderBy(x => x.Item1).ToList();

                            int counter = 0;
                            int tempWidthAlloc = 0;
                            foreach (Tuple<DateTime, DateTime, string, string, string, string> str in lstStartAndEndDates)
                            {
                                estAlloc = str.Item3;
                                allocId = str.Item6;
                                if (!string.IsNullOrWhiteSpace(str.Item5))
                                {
                                    aSoftAllocation = UGITUtility.StringToBoolean(str.Item5);
                                }
                                string roundleftbordercls = string.Empty;
                                string roundrightbordercls = string.Empty;

                                aStartDate = str.Item1;
                                aEndDate = str.Item2;

                                string backgroundColor = string.Empty;
                                if (aStartDate == DateTime.MinValue || aEndDate == DateTime.MinValue)
                                    return;
                                // classed to set round corners on bar end points
                                if (hdndisplayMode.Value != "Weekly")
                                {

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

                                    //if (IsFirstMondayOfYear(dt) && aStartDate <= dt)
                                    //{
                                    //    roundleftbordercls = "RoundLeftSideCorner";
                                    //}
                                    if (aStartDate >= dt && aStartDate.AddDays(-6) < dt)
                                    {
                                        roundleftbordercls = "RoundLeftSideCorner";
                                    }
                                    if (aEndDate >= dt && aEndDate <= dt.AddDays(6))
                                    {
                                        roundrightbordercls = "RoundRightSideCorner";
                                    }
                                }

                                backgroundColor = uHelper.GetGanttCellBackGroundColor(context,aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt, dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), aSoftAllocation);

                                if (dt.Month == aConstStart.Month && dt.Month == oPreconEnd.Month && aConstStart.Year == dt.Year && oPreconEnd.Year == dt.Year)
                                {
                                    if (aEndDate <= oPreconEnd)
                                    {
                                        backgroundColor = "preconbgcolor";
                                    }
                                    else if (aConstStart >= oConstStart && aConstEnd <= oConstEnd)
                                    {
                                        backgroundColor = "constbgcolor";
                                    }
                                }

                                string cell = e.Cell.ClientID;
                                string startDateString = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(aStartDate), false);
                                string endDateString = UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(aEndDate), false);

                                string onClickEvent = isEditAllowed ? $"onclick='javascript:event.cancelBubble=true; openworkitem(\"{allocId}\", \"{name}\",\"{subworkitem}\", \"{aPreconStart}\",\"{aPreconEnd}\",\"{aConstStart}\", \"{aConstEnd}\",\"{aConstEnd.AddDays(1)}\", \"{aCloseout}\")' " : string.Empty;


                                if (!string.IsNullOrEmpty(estAlloc) && estAlloc != "0")
                                {
                                    if (moduleName != null && moduleName.Equals("Time Off"))
                                    {
                                        //backgroundColor = "ptobgcolor";
                                        html = GeneratePtoCard(dt, id, defaultBarH, type);
                                        e.Cell.CssClass += " ptoAlignmentClass";
                                    }
                                    else
                                    {
                                        estAlloc = estAlloc + "% <br>";

                                        int NoOfDays = 0;
                                        int remainingDays = 0;
                                        int widthEstAlloc = 100;
                                        string leftMargin = "0";
                                        int daysInMonth = System.DateTime.DaysInMonth(dt.Year, dt.Month);
                                        if (hdndisplayMode.Value != "Weekly")
                                        {
                                            if ((aStartDate.Month == dt.Month && aStartDate.Year == dt.Year) || (aEndDate.Month == dt.Month && aEndDate.Year == dt.Year))
                                            {
                                                if (aStartDate.Month == dt.Month && aStartDate.Year == dt.Year)
                                                {
                                                    NoOfDays = aStartDate.Day;
                                                    remainingDays = daysInMonth - NoOfDays;
                                                    widthEstAlloc = ((remainingDays <= 0 ? 2 : remainingDays) * 100) / daysInMonth;

                                                    roundleftbordercls = "RoundLeftSideCorner";
                                                    backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, aStartDate, dt.AddMonths(1).AddDays(-1));
                                                }
                                                if (aEndDate.Month == dt.Month && aEndDate.Year == dt.Year)
                                                {
                                                    NoOfDays = aEndDate.Day;
                                                    //remainingDays = 30 - NoOfDays;
                                                    if (aStartDate.Month == dt.Month && aStartDate.Year == dt.Year)
                                                    {
                                                        NoOfDays = aEndDate.Day - aStartDate.Day;
                                                        leftMargin = Convert.ToString((aStartDate.Day * 100 / daysInMonth) - tempWidthAlloc);
                                                        backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, aStartDate, aEndDate);
                                                    }
                                                    else
                                                    {
                                                        backgroundColor = uHelper.GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt, aEndDate);
                                                    }
                                                    widthEstAlloc = (NoOfDays * 100) / daysInMonth;
                                                    roundrightbordercls = "RoundRightSideCorner";
                                                    //roundleftbordercls = "";
                                                }
                                            }
                                            else if (dt.Month == aConstStart.Month && dt.Month == oPreconEnd.Month)
                                            {
                                                if (aEndDate <= oPreconEnd)
                                                {
                                                    NoOfDays = aEndDate.Day;
                                                    //remainingDays = 30 - NoOfDays;
                                                    widthEstAlloc = (NoOfDays * 100) / daysInMonth;
                                                }
                                                else if (aConstStart >= oConstStart && aConstEnd <= oConstEnd)
                                                {
                                                    if (aConstStart.Year != 1 || oConstStart.Year != 1 || aConstEnd.Year != 1 && oConstEnd.Year != 1)
                                                    {
                                                        if (aConstStart >= oConstStart)
                                                        {
                                                            NoOfDays = aConstStart.Day;
                                                            remainingDays = daysInMonth - NoOfDays;
                                                            widthEstAlloc = (remainingDays * 100) / daysInMonth;
                                                        }
                                                        else if (aConstEnd <= oConstEnd)
                                                        {
                                                            NoOfDays = aConstEnd.Day;
                                                            //remainingDays = 30 - NoOfDays;
                                                            widthEstAlloc = (NoOfDays * 100) / daysInMonth;
                                                        }
                                                    }
                                                }
                                            }

                                            if (aSoftAllocation)
                                                backgroundColor = "softallocationbgcolor";

                                        }
                                        else {
                                            if (lstStartAndEndDates.Count > 1)
                                            {
                                                widthEstAlloc = 100 / lstStartAndEndDates.Count;
                                            }
                                        }
                                        if (widthEstAlloc >= 0)
                                        {
                                            if (widthEstAlloc != 100)
                                            {
                                                if (widthEstAlloc > 50)
                                                {
                                                    widthEstAlloc -= 8;
                                                }
                                                else if (widthEstAlloc >= 18 && widthEstAlloc <= 50)
                                                {
                                                    widthEstAlloc -= 3;
                                                }
                                                else if (widthEstAlloc < 13)
                                                {
                                                    widthEstAlloc = 10;
                                                }
                                                tempWidthAlloc += widthEstAlloc + UGITUtility.StringToInt(leftMargin);
                                            }
                                            if (Convert.ToInt32(widthEstAlloc) >= 35)
                                            {

                                                html += $"<div id='gCell_{cell}{counter}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' ";

                                                if (moduleName != "CPR")
                                                {
                                                    html += $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{str.Item3}%\")' onmouseout='hideTooltip(this)' ";
                                                }

                                                html+=$"{onClickEvent}" +
                                                    $"style='width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;margin-left:{leftMargin}%;cursor: pointer;'>{estAlloc}</div>";

                                            }
                                            else
                                            {

                                                html += $"<div id='gCell_{cell}{counter}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' ";

                                                if (moduleName != "CPR")
                                                {
                                                   html+= $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{str.Item3}%\")' onmouseout='hideTooltip(this)' ";
                                                }
                                                   html+= $"{onClickEvent}" +
                                                    $"style='width:{widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;margin-left:{leftMargin}%;cursor: pointer;'></div>";

                                            }
                                        }
                                        else
                                        {
                                            html += $"<div id='gCell_{cell}{counter}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor}' ";

                                            if (moduleName != "CPR")
                                            {
                                                html += $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{str.Item3}%\")' onmouseout='hideTooltip(this)' ";
                                            }
                                            html += $"{onClickEvent}" +
                                            $"style='float:left;width:100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;cursor: pointer;'>{estAlloc}</div>";
                                        }
                                    }
                                }
                                
                                counter++;
                            }
                        }
                        else
                        {
                            html = isEditAllowed ? $" <div id='gCell_{e.Cell.ClientID}' runat='server' " +
                                $"onclick='javascript:event.cancelBubble=true; " + //BTS-24-001501: Issues with Gantt Edit:
                                $"style='float:left;width:100%;padding-left:{defaultBarH}px;height: {defaultBarH}px;'></div>"
                                : $" <div id='gCell_{e.Cell.ClientID}' runat='server' style='float:left;width:100%;padding-left:{defaultBarH}px;height: {defaultBarH}px;'></div>";
                        }
                        e.Cell.Text = html;
                        //sets alternate color to columns
                        if (e.DataColumn.VisibleIndex % 2 == 0)
                            e.Cell.BackColor = Color.WhiteSmoke;
                    }


                }
            }

        }

        public string GeneratePtoCard(DateTime currentDate, string userId, int defaultBarH, string workItem)
        {
            string html = string.Empty;
            string marginLeft = string.Empty;
            DateTime startDate = currentDate;
            DateTime endDate = DateTime.MinValue;
            Tuple<int,int> addDays = new Tuple<int, int>(10, 20);
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
            else{
                int lastDayOfMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                endDate = new DateTime(startDate.Year, startDate.Month, lastDayOfMonth);
            }
            DataTable dt = ResourceAllocationManager.LoadRawTableByResource(userId.Split(',').ToList(), 4, dateFrom, dateTo);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] monthlyData = dt.Select($"{DatabaseObjects.Columns.AllocationStartDate}<='{endDate}' and {DatabaseObjects.Columns.AllocationEndDate} >= '{startDate}'" +
                                            $"and {DatabaseObjects.Columns.WorkItemType}='Time Off' and {DatabaseObjects.Columns.WorkItemLink}='{workItem}'");

                if (monthlyData.Count() > 0)
                {
                    foreach (DataRow data in monthlyData)
                    {
                        string taskId = data.Field<long>(DatabaseObjects.Columns.Id).ToString();
                        string resourceName = data.Field<string>(DatabaseObjects.Columns.Resource).ToString().Replace("'", "`");
                        DateTime fromDate = data.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate);
                        DateTime toDate = data.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate);
                        if ((fromDate >= startDate && fromDate <= startDate.AddDays(addDays.Item1)) || fromDate < startDate)
                        {
                            marginLeft = "0%";
                        }
                        else if (fromDate > startDate.AddDays(addDays.Item1) && fromDate <= startDate.AddDays(addDays.Item2))
                        {
                            marginLeft = "30%";
                        }
                        else
                        {
                            marginLeft = "60%";
                        }
                        if (fromDate == toDate)
                        {
                            html += $"<a href=\"#\" onclick=\"OpenTimeOffAllocation('{taskId}', '{resourceName}')\"  >";
                            html += $"<div class='RoundLeftSideCorner RoundRightSideCorner ptobgcolor jqtooltip' title = '{fromDate.ToString("dd MMM, yyyy")}' style='width:40%;margin-bottom:5%;color:white;margin-left:{marginLeft};height:{defaultBarH}px;padding-top:5px;font-weight:500;'>1</div>";
                            html += "</a>";
                        }
                        else
                        {
                            double dayDiff = 0;
                            dayDiff = GetOverlappingDays(startDate,endDate, fromDate, toDate);  
                            dayDiff += 1;
                            html += $"<a href=\"#\" onclick=\"OpenTimeOffAllocation('{taskId}', '{resourceName}')\"  >";
                            html += $"<div class='RoundLeftSideCorner RoundRightSideCorner ptobgcolor jqtooltip' title = '{fromDate.ToString("dd MMM, yyyy")} to {toDate.ToString("dd MMM, yyyy")}' style='width:40%;margin-bottom:5%;color:white;margin-left:{marginLeft};height:{defaultBarH}px;padding-top:5px;font-weight:500;'>{dayDiff}</div>";
                            html += "</a>";
                        }
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
                string resourceuser = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Resource]);
                if (!string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.AllocationEndDate])))
                    absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&ticketId={4}&tabname=allocationtimeline&isRedirectFromCardView=true",
                        "CustomResourceAllocation", userid, UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.AllocationStartDate])).ToShortDateString(), UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.AllocationEndDate])).ToShortDateString(), Convert.ToString(row[DatabaseObjects.Columns.TicketId])));
                else
                    absoluteUrlEdit = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/DelegateControl.aspx?control={0}&ID={1}&startDate={2}&endDate={3}&ticketId={4}&workItemType={5}&subWorkItem={6}&monthlyAllocationEdit=false&tabname=allocationtimeline&isRedirectFromCardView=true",
                        "CustomResourceAllocation", userid, UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.PlannedStartDate])).ToShortDateString(), UGITUtility.StringToDateTime(Convert.ToString(row[DatabaseObjects.Columns.PlannedEndDate])).ToShortDateString(), Convert.ToString(row[DatabaseObjects.Columns.TicketId]), Convert.ToString(row[DatabaseObjects.Columns.ModuleName]).Trim(), Convert.ToString(row[DatabaseObjects.Columns.SubWorkItem]).Trim()));
                string func = string.Format("openResourceAllocationDialog('{0}','{1}','{2}')", absoluteUrlEdit, "Resource Utilization", Server.UrlEncode(Request.Url.AbsolutePath));
                string aHref = UGITUtility.GetHrefFromATagString(Convert.ToString(row["ProjectNameLink"]));

                if (!IsMobileView)
                {
                    if (string.IsNullOrEmpty(TicketID))
                    {
                        if (!string.IsNullOrEmpty(cmicno))
                            e.Row.Cells[1].Text = $"<div onmouseover='ShowEditImage(this)' onmouseout='HideEditImage(this)' style='float:right; width:380px;'>" +
                                    $"<div class='glabel1' onClick='{aHref}'><div>{companyname} {erpLabel}</div> <div class='cmicnoLabel'>&nbsp;{cmicno}</div></div>" +
                                    $"<div class='glabel2'>{row["ProjectNameLink"]}</div>" +
                                    $"</div>";
                        else
                            e.Row.Cells[1].Text = $"<div onmouseover='ShowEditImage(this)' onmouseout='HideEditImage(this)' style='float:right; width:380px;'>" +
                                    $"<div class='glabel1' onClick='{aHref}'>{companyname}</div>" +
                                    $"<div class='glabel2'>{row["ProjectNameLink"]}</div>" +
                                    $"</div>";
                    }
                    else
                    {
                        //string str = userid + "," + resourceuser;
                        string imagePath = userProfiles.Find(x => x.Id == row.Field<string>("Id"))?.Picture ?? "/Content/Images/userNew.png";
                        
                        string anchorTag = userid != "00000000-0000-0000-0000-000000000000"
                            ? $"<img src=\"{imagePath}\"/><a href='#' onclick='openResourceTimeSheet(\"{userid}\", \"{resourceuser}\");' style='font-weight: bold; color: black;'>{resourceuser}&nbsp;</a>"
                            : String.Empty;
                        e.Row.Cells[0].Text = $"<div style='float:left;' ><div class='glabel1'>({subworkitem})</div><div class='glabel2'>{anchorTag}</div> </div>";
                    } 
                }
                else
                {
                    string resourceUserTag = resourceuser;
                    var resourceUserObj = userProfiles.FirstOrDefault(u => u.Id == userid); 
                    if(resourceUserObj != null && !resourceUserObj.Enabled)
                    {
                        resourceUserTag = $"<span style='color:red'>{resourceuser}</span>";
                    }
                    //string anchorTag = userid != "00000000-0000-0000-0000-000000000000" 
                    //    ? $"<a href='#' onclick='openResourceTimeSheet(\"{userid}\", \"{resourceuser}\");' style='font-weight: bold; color: black;'>{resourceUserTag}&nbsp;</a>"
                    //    :String.Empty;
                    //e.Row.Cells[0].Text = $"<div style='float:right; width:380px;' ><div class='glabel1'>({subworkitem})</div><div class='glabel2'>{anchorTag}</div> </div>";
                    if (string.IsNullOrEmpty(TicketID))
                    {
                        if (!string.IsNullOrEmpty(cmicno))
                            e.Row.Cells[1].Text = $"<div onmouseover='ShowEditImage(this)' onmouseout='HideEditImage(this)' style='float:right; width:160px;text-align:right;' >" +
                                    $"<div class='glabel1' onClick='{aHref}'><div>{companyname} {erpLabel}</div> <div class='cmicnoLabel'>&nbsp;{cmicno}</div></div>" +
                                    $"<div class='glabel2'>{row["ProjectNameLink"]}</div>" +
                                    $"</div>";
                        else
                            e.Row.Cells[1].Text = $"<div onmouseover='ShowEditImage(this)' onmouseout='HideEditImage(this)' style='float:right; width:160px;text-align:right;' >" +
                                    $"<div class='glabel1' onClick='{aHref}'>{companyname}</div>" +
                                    $"<div class='glabel2'>{row["ProjectNameLink"]}</div>" +
                                    $"</div>";
                    }
                    else
                    {
                        //string str = userid + "," + resourceuser;
                        string anchorTag = userid != "00000000-0000-0000-0000-000000000000"
                            ? $"<a href='#' onclick='openResourceTimeSheet(\"{userid}\", \"{resourceuser}\");' style='font-weight: bold; color: black;'>{resourceuser}&nbsp;</a>"
                            : String.Empty;
                        e.Row.Cells[0].Text = $"<div style='float:right; width:160px;text-align:right;' ><div class='glabel2'>{anchorTag}</div><div class='glabel1'>({subworkitem})</div></div>";
                    }
                }
            }
        }
        private DataTable LoadAllocationMonthlyView()
        {
            ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            return allocationMonthlyManager.LoadAllocationMonthlyView(Convert.ToDateTime(dateFrom),
                Convert.ToDateTime(dateTo), chkIncludeClosed.Checked);

        }

        private DataTable LoadAllocationWeeklySummaryView()
        {
            try
            {
                DateTime dtFrom = dateFrom;
                DateTime dtTo = dateTo;

                //dtFrom = new DateTime(dateFrom.Year, 01, 01);
                //dtTo = new DateTime(dateFrom.Year, 12, 31);

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
            DataTable dtlist = new DataTable();
            if (hdndisplayMode.Value == "Weekly")
            {
                string selectedMonth = UGITUtility.ObjectToString(hdnMonth["selectedDate"]);

                string dateFormat = "MMM-dd-yy'E'";

                DateTime parsedDate = DateTime.ParseExact(selectedMonth, dateFormat, null);
                DateTime oneMonthBefore = parsedDate.AddMonths(-1);
                DateTime oneMonthAfter = parsedDate.AddMonths(1);

                dateFrom = new DateTime(oneMonthBefore.Year, oneMonthBefore.Month, 1);
                dateTo = new DateTime(oneMonthAfter.Year, oneMonthAfter.Month, DateTime.DaysInMonth(oneMonthAfter.Year, oneMonthAfter.Month));

            }
            else
            {

                int selectedYear = UGITUtility.StringToInt(hndYear.Value);
                dateFrom = new DateTime(selectedYear - 2, 1, 1);
                dateTo = new DateTime(selectedYear + 3, 1, 1);

            }
            dtlist = ResourceAllocationManager.GetDatelist(hdndisplayMode.Value, dateFrom, dateTo);
            data.Merge(dtlist);
            data.Clear();

            //bool containsModules = false;
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
            List<UserProfile> lstUProfile = new List<UserProfile>();

            bool limitedUsers = false;
            //bool isTicketData = false;

            lstUProfile = ObjUserProfileManager.GetUsersProfile();


            if (!string.IsNullOrEmpty(SelectedUser))
            {
                lstUProfile.Clear();
                UserProfile user = Context.GetUserManager().GetUserById(SelectedUser);
                if (user != null)
                    lstUProfile.Add(user);
                limitedUsers = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "SelectedUser")))
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

            // temp code
            //limitedUsers = true;
            //SelectedUsers = "3ed90305-64bd-437b-aa56-935ebd0481d0";
            if (limitedUsers)
            {
                if (hdndisplayMode.Value == "Weekly" && !string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "SelectedUser")))
                {
                    SelectedUsers = UGITUtility.GetCookieValue(Request, "SelectedUser");
                }
                if (!string.IsNullOrEmpty(SelectedUsers) && SelectedUsers != "null")
                {
                    userIds = UGITUtility.ConvertStringToList(SelectedUsers, Constants.Separator6);
                    //List<UserProfile> lstSelectedUsers = ObjUserProfileManager.GetUserInfosById(SelectedUsers);
                    //userIds = lstSelectedUsers.Select(x => x.Id).ToList();
                }
                else
                {
                    userIds = lstUProfile.Select(x => x.Id).ToList();
                }
                dtResult = ResourceAllocationManager.LoadRawTableByResource(userIds, 5, dateFrom, dateTo, TicketID);
                workitems = ResourceWorkItemsManager.LoadRawTableByResource(userIds, 4);

            }
            else
            {
                dtResult = ResourceAllocationManager.LoadRawTableByResource(null, 5, dateFrom, dateTo, TicketID, true);
                workitems = RMMSummaryHelper.GetOpenworkitems(context, chkIncludeClosed.Checked);
            }

            // Retrieve the minimum and maximum dates
            if (dtResult != null && hdndisplayMode.Value != "Weekly" && dtResult.Rows.Count > 0)
            {
                DateTime minDate = dtResult.AsEnumerable()
                    .Min(row => row.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate));
                dateFrom = new DateTime(minDate.Year, 1, 1);

                DateTime maxDate = dtResult.AsEnumerable()
                    .Max(row => row.Field<DateTime>(DatabaseObjects.Columns.AllocationEndDate));
                dateTo = new DateTime(maxDate.Year, 12, 31);

            }

            if (!string.IsNullOrEmpty(TicketID))
            {

                DataRow[] workitemRows = workitems.Select($"{DatabaseObjects.Columns.WorkItem} = {TicketID}");
                workitems = workitemRows!= null && workitemRows.Count() > 0 ? workitemRows.CopyToDataTable() : null;
            }


            if (dtResult == null)
                return data;
            bool IsPhaseSummaryView = UGITUtility.StringToBoolean(Request["IsPhaseSummaryView"]);

            var allocationGroupData = dtResult.AsEnumerable()
            .GroupBy(row => new
            {
                Id = row.Field<string>("ResourceId"),
                SubWorkItem = row.Field<string>("SubWorkItem")
                // Add other grouping columns here
            })
            .Select(group => new
            {
                ResourceId = group.Key.Id,
                SubWorkItem = group.Key.SubWorkItem,
                // Add other aggregated properties here
                AllocationID = string.Join(",", group.Select(row => row.Field<long?>("ID") + ":" + UGITUtility.ChangeDateFormat(row.Field<DateTime?>("AllocationStartDate")??DateTime.MinValue,"MM/dd/yyyy") + ":" + UGITUtility.ChangeDateFormat(row.Field<DateTime?>("AllocationEndDate")??DateTime.MinValue, "MM/dd/yyyy"))),
                ResourceUser = group.First().Field<string>("ResourceUser"),
                WorkItemType = group.First().Field<string>("WorkItemType"),
                WorkItemLink = group.First().Field<string>("WorkItemLink"),
                WorkItem = group.First().Field<string>("WorkItem"),
                AllocationStartDate =  group.Min(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue),
                AllocationEndDate = group.Max(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue),
                PctAllocation = group.Average(row => row.Field<double>("PctAllocation")),
                WorkItemID = string.Join(",", group.Select(row => row.Field<long>("WorkItemID")).Distinct()),
                //Title = group.First().Field<string>("Title"),
                ShowEditButton = false, // group.Select(row => row.Field<bool?>("ShowEditButton")).FirstOrDefault() ?? false,
                ShowPartialEdit = false, //group.Select(row => row.Field<bool?>("ShowPartialEdit")).FirstOrDefault() ?? false,
                PlannedStartDate = group.Min(row => row.Field<DateTime?>("PlannedStartDate") ?? DateTime.MinValue),
                PlannedEndDate = group.Max(row => row.Field<DateTime?>("PlannedEndDate") ?? DateTime.MinValue),
                PctPlannedAllocation = group.Average(row => row.Field<double?>("PctPlannedAllocation") ?? 0),
                EstStartDate = group.Min(row => row.Field<DateTime?>("EstStartDate") ?? DateTime.MinValue),
                EstEndDate = group.Max(row => row.Field<DateTime?>("EstEndDate") ?? DateTime.MinValue),
                PctEstimatedAllocation = group.Average(row => row.Field<double?>("PctEstimatedAllocation") ?? 0),
                SoftAllocation = group.First().Field<bool>("SoftAllocation")
                //ResourceWorkItemLookup = string.Join(",", group.Select(row => row.Field<long?>("ResourceWorkItemLookup")).FirstOrDefault()??0),
            }).Where(o => o.ResourceId != "00000000-0000-0000-0000-000000000000").ToArray();

            var unAssignedAllocationData = dtResult.AsEnumerable()
            .Select(row => new
            {
                ResourceId = row.Field<string>("ResourceId"),
                SubWorkItem = row.Field<string>("SubWorkItem"),
                // Add other aggregated properties here
                AllocationID = UGITUtility.ObjectToString(row.Field<long>("ID") + ":" + UGITUtility.ChangeDateFormat(row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue, "MM/dd/yyyy") + ":" + UGITUtility.ChangeDateFormat(row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue, "MM/dd/yyyy")),
                ResourceUser = row.Field<string>("ResourceUser"),
                WorkItemType = row.Field<string>("WorkItemType"),
                WorkItemLink = row.Field<string>("WorkItemLink"),
                WorkItem = row.Field<string>("WorkItem"),
                AllocationStartDate = row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue,
                AllocationEndDate = row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue,
                PctAllocation = row.Field<double>("PctAllocation"),
                WorkItemID = row.Field<long>("WorkItemID").ToString(),
                //Title = group.First().Field<string>("Title"),
                ShowEditButton = false, 
                ShowPartialEdit = false, 
                PlannedStartDate =row.Field<DateTime?>("PlannedStartDate") ?? DateTime.MinValue,
                PlannedEndDate = row.Field<DateTime?>("PlannedEndDate") ?? DateTime.MinValue,
                PctPlannedAllocation = row.Field<double?>("PctPlannedAllocation") ?? 0,
                EstStartDate = row.Field<DateTime?>("EstStartDate") ?? DateTime.MinValue,
                EstEndDate = row.Field<DateTime?>("EstEndDate") ?? DateTime.MinValue,
                PctEstimatedAllocation = row.Field<double?>("PctEstimatedAllocation") ?? 0,
                SoftAllocation = row.Field<bool>("SoftAllocation")
            }).Where(o => o.ResourceId == "00000000-0000-0000-0000-000000000000").OrderBy(x => x.SubWorkItem).ToArray();
            
            if (unAssignedAllocationData != null && unAssignedAllocationData.Length > 0)
            {
                allocationGroupData = allocationGroupData.Concat(unAssignedAllocationData).ToArray();
            }

            DataTable dtAllocationMonthly = null;
            DataTable dtAllocationWeekly = null;
            //List<ProjectEstimatedAllocation> dtEstimatedTable = null;

            if (hdndisplayMode.Value == "Weekly")
            {
                dtAllocationWeekly = LoadAllocationWeeklySummaryView();
            }
            else
            {
                dtAllocationMonthly = LoadAllocationMonthlyView();
            }


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
                string userid = UGITUtility.ObjectToString(dr.ResourceId);
                if (string.IsNullOrEmpty(userid))
                    continue;

                userDetails = userProfiles.FirstOrDefault(x => x.Id.EqualsIgnoreCase(userid));

                //Now allowing disabled users to be shown - BTS-23-001396: Show disabled users in red when showing project allocations
                //if (userDetails != null && !userDetails.Enabled)
                //{
                //    continue;
                //}

                if (string.IsNullOrEmpty(UGITUtility.ObjectToString(dr.WorkItemID)))
                    continue;

                string workitemid = Convert.ToString(dr.WorkItemID);
                List<string> lstWorkItemIds = UGITUtility.ConvertStringToList(dr.WorkItemID, Constants.Separator6);
                DataRow newRow = data.NewRow();

                newRow[DatabaseObjects.Columns.Id] = userid;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;
                if (btnexport)
                {
                    newRow[DatabaseObjects.Columns.Resource] = userDetails != null ? userDetails.Name : "";
                }
                else
                {
                    newRow[DatabaseObjects.Columns.Resource] = UGITUtility.ObjectToString(dr.ResourceUser);
                }
                newRow[DatabaseObjects.Columns.Name] = userDetails != null ? userDetails.Name : "";
                newRow[DatabaseObjects.Columns.SoftAllocation] = UGITUtility.StringToBoolean(dr.SoftAllocation);
                newRow[DatabaseObjects.Columns.SubWorkItem] = UGITUtility.ObjectToString(dr.SubWorkItem);
                newRow[DatabaseObjects.Columns.AllocationID] = UGITUtility.ObjectToString(dr.AllocationID);

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
                                dataRow = ticketManager.GetDataRowByTicketId(workItem); // dtAllModuleTickets.AsEnumerable().FirstOrDefault(row => row.Field<string>("TicketId") == workItem);
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
                                    //newRow[DatabaseObjects.Columns.CRMCompanyLookup] = UGITUtility.ObjectToString(dataRow["CRMCompanyTitleLookup"]);
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
                                newRow[DatabaseObjects.Columns.WorkItemID] = UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.ResourceId);

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

                                    List<string> lstworkitems = UGITUtility.ConvertStringToList(dr.WorkItemID, Constants.Separator6);
                                    if (hdndisplayMode.Value == "Weekly" && dtWeeklyLookups != null)
                                    {
                                        foreach (string s in lstworkitems)
                                        {
                                            DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(s)].ToArray();
                                            if (dttemp != null && dttemp.Length > 0)
                                            {
                                                DataRow[] dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.ActualStartDate) == dr.AllocationStartDate
                                                                                        && x.Field<DateTime>(DatabaseObjects.Columns.ActualEndDate) == dr.AllocationEndDate
                                                                                        && x.Field<double>(DatabaseObjects.Columns.ActualPctAllocation) == dr.PctAllocation).ToArray();
                                                if (dtActualTemp == null || dtActualTemp.Length == 0)
                                                    dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<long>(DatabaseObjects.Columns.WorkItemID) == UGITUtility.StringToLong(s)).ToArray();

                                                if (dtActualTemp != null && dtActualTemp.Length > 0)
                                                    ViewTypeAllocation(data, newRow, dtActualTemp);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            foreach (string s in lstworkitems)
                                            {
                                                DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                                
                                                if (dttemp != null && dttemp.Length > 0)
                                                {
                                                    DataRow[] dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.ActualStartDate) == dr.AllocationStartDate
                                                                                        && x.Field<DateTime>(DatabaseObjects.Columns.ActualEndDate) == dr.AllocationEndDate
                                                                                        && x.Field<double>(DatabaseObjects.Columns.ActualPctAllocation) == dr.PctAllocation).ToArray();
                                                    if (dtActualTemp == null || dtActualTemp.Length == 0)
                                                        dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == s).ToArray();

                                                    if (dtActualTemp !=null && dtActualTemp.Length > 0)
                                                        ViewTypeAllocation(data, newRow, dtActualTemp);
                                                }
                                            }
                                        }
                                    }

                                    data.Rows.Add(newRow);

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
                                        DataRow[] dttemp = dtWeeklyLookups[UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])].ToArray();
                                        if (dttemp != null && dttemp.Length > 0)
                                        {
                                            DataRow[] dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.ActualStartDate) == dr.AllocationStartDate
                                                                                        && x.Field<DateTime>(DatabaseObjects.Columns.ActualEndDate) == dr.AllocationEndDate
                                                                                        && x.Field<double>(DatabaseObjects.Columns.ActualPctAllocation) == dr.PctAllocation).ToArray();
                                            if (dtActualTemp == null || dtActualTemp.Length == 0)
                                                dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<long>(DatabaseObjects.Columns.WorkItemID) == UGITUtility.StringToLong(drWorkItem[DatabaseObjects.Columns.Id])).ToArray();

                                            if (dtActualTemp != null && dtActualTemp.Length > 0)
                                                ViewTypeAllocation(data, newRow, dtActualTemp);
                                        }
                                    }
                                    else
                                    {
                                        if (dtAllocLookups != null)
                                        {
                                            List<string> lstworkitems = UGITUtility.ConvertStringToList(dr.WorkItemID, Constants.Separator6);
                                            foreach (string s in lstworkitems)
                                            {
                                                DataRow[] dttemp = dtAllocLookups[UGITUtility.ObjectToString(s)].ToArray();
                                                
                                                if (dttemp != null && dttemp.Length > 0)
                                                {
                                                    DataRow[] dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<DateTime>(DatabaseObjects.Columns.ActualStartDate) == dr.AllocationStartDate
                                                                                        && x.Field<DateTime>(DatabaseObjects.Columns.ActualEndDate) == dr.AllocationEndDate
                                                                                        && x.Field<double>(DatabaseObjects.Columns.ActualPctAllocation) == dr.PctAllocation).ToArray();
                                                    
                                                    if(dtActualTemp == null || dtActualTemp.Length == 0)
                                                        dtActualTemp = dttemp.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ResourceWorkItemLookup) == s).ToArray();

                                                    if (dtActualTemp != null && dtActualTemp.Length > 0)
                                                        ViewTypeAllocation(data, newRow, dtActualTemp);

                                                }
                                            }
                                        }
                                    }
                                    data.Rows.Add(newRow);
                                }
                            }
                        }
                    }

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

            foreach (DataRow rowitem in dttemp)
            {
                string monthlyPct = UGITUtility.ObjectToString(rowitem[DatabaseObjects.Columns.PctAllocation]);
                List<long> allocationID = uHelper.SplitAllocationIdIntoTuple(UGITUtility.ObjectToString(newRow[DatabaseObjects.Columns.AllocationID]), Convert.ToDateTime(rowitem[DatabaseObjects.Columns.ActualStartDate]), Convert.ToDateTime(rowitem[DatabaseObjects.Columns.ActualEndDate]));
                
                if (hdndisplayMode.Value == "Weekly")
                {
                    if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        if (!data.Columns.Contains("OverlappingAllocations" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"))
                        {
                            data.Columns.Add("OverlappingAllocations" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                        }
                        newRow["OverlappingAllocations" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] += Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2).ToString() + ",";

                        if (!data.Columns.Contains("StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"))
                        {
                            data.Columns.Add("StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                        }
                        if (!newRow["StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"].ToString()
                           .Contains(rowitem[DatabaseObjects.Columns.ActualStartDate].ToString() + "#"
                           + rowitem["ActualEndDate"].ToString()))
                        {
                            newRow["StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] += rowitem[DatabaseObjects.Columns.ActualStartDate].ToString() + "#"
                            + rowitem["ActualEndDate"].ToString() + ",";
                        }
                        if (!data.Columns.Contains("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"))
                        {
                            data.Columns.Add("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                        }
                        newRow["ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] += rowitem[DatabaseObjects.Columns.WorkItemID].ToString() + ",";

                        if (!data.Columns.Contains("SoftAllocation" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"))
                        {
                            data.Columns.Add("SoftAllocation" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                        }
                        newRow["SoftAllocation" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] += rowitem[DatabaseObjects.Columns.SoftAllocation].ToString() + ",";


                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);

                        if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"))
                            data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct", typeof(string));
                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"] += monthlyPct + Constants.Separator6;

                        if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_AllocationID"))
                            data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_AllocationID", typeof(string));
                        newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]).ToString("MMM-dd-yy") + "E_AllocationID"] += string.Join(",", allocationID) + Constants.Separator6;
                    }

                }
                else
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        DateTime AllocationStartDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate]);
                        DateTime AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                        DateTime allocationEndDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate]);
                        
                        if (Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) >= AllocationMonthStartDate && Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]) <= allocationEndDate)
                        {
                            if (data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                            {
                                if (!data.Columns.Contains("OverlappingAllocations" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                                {
                                    data.Columns.Add("OverlappingAllocations" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                                }
                                newRow["OverlappingAllocations" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] += Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2).ToString() + ",";
                                
                                if (!data.Columns.Contains("StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                                {
                                    data.Columns.Add("StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                                }

                                if (!newRow["StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"].ToString().Contains(rowitem[DatabaseObjects.Columns.ActualStartDate].ToString() + "#"
                                    + rowitem["ActualEndDate"].ToString()))
                                {
                                    newRow["StartAndEndDates" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] += rowitem[DatabaseObjects.Columns.ActualStartDate].ToString() + "#"
                                + rowitem["ActualEndDate"].ToString() + ",";
                                }

                                if (!data.Columns.Contains("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                                {
                                    data.Columns.Add("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                                }
                                newRow["ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] += rowitem[DatabaseObjects.Columns.ResourceWorkItemLookup].ToString() + ",";

                                if (!data.Columns.Contains("SoftAllocation" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                                {
                                    data.Columns.Add("SoftAllocation" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                                }
                                newRow["SoftAllocation" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] += rowitem[DatabaseObjects.Columns.SoftAllocation].ToString() + ",";

                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"] = Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);

                                if (!data.Columns.Contains(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"))
                                    data.Columns.Add(Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct", typeof(string));
                                newRow[Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E_MonthlyPct"] += monthlyPct + Constants.Separator6;

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

            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Resource;
            colId.Caption = DatabaseObjects.Columns.Resource;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.Width = new Unit("200px");
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.HeaderStyle.Font.Bold = true;
            //colId.GroupIndex = 0;
            colId.Visible = false;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
            colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            //colId.FixedStyle = GridViewColumnFixedStyle.Left;
            gvPreview.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Name;
            colId.Caption = DatabaseObjects.Columns.Name;
            colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.Visible = false;
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
            colId.Width = new Unit(IsMobileView ? "160px": "400px");
            colId.ExportWidth = 400;
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
            string currentDate = string.Empty;

            //if (hdndisplayMode.Value == "Weekly")
            //{
            //    dateFrom = new DateTime(dateFrom.Year, 01, 01);
            //    dateTo = new DateTime(dateFrom.Year, 12, 31);
            //}
            if (dateFrom.Year != DateTime.Now.Year && dateFrom.Year < DateTime.Now.Year && dateTo.Year >= DateTime.Now.Year)
            {
                UGITUtility.CreateCookie(Response, "ScrollPosition", (DateTime.Now.Year - dateFrom.Year).ToString());
            }
            else 
            {
                UGITUtility.DeleteCookie(Request, Response, "ScrollPosition");
            }

            for (DateTime dt = dateFrom; dateTo > dt; dt = dt.AddDays(GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
            {
                if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                {
                    bdCol.HeaderTemplate = new MonthUpGridViewBandColumnProjGantt(bdCol, hdndisplayMode.Value);
                    gvPreview.Columns.Add(bdCol);
                    bdCol = new GridViewBandColumn();
                }

                if (dt.ToString("yyyy") != currentDate)
                {
                    bdCol.Caption = dt.ToString("yyyy");
                    bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    bdCol.HeaderStyle.Font.Bold = true;
                    bdCol.HeaderStyle.CssClass = "current-scroll";
                    bdCol.HeaderTemplate = new MonthUpGridViewBandColumnProjGantt(bdCol, hdndisplayMode.Value);
                    currentDate = dt.ToString("yyyy");
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

                ColIdData.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                ColIdData.HeaderTemplate = new MonthDownHeaderTemplateProjGantt(ColIdData, hdndisplayMode.Value);

                if (hdndisplayMode.Value == "Monthly")
                {
                    ColIdData.Width = new Unit(IsMobileView ? "70px" : MonthColWidth + "px");
                    ColIdData.ExportWidth = 38;
                }

                if (hdndisplayMode.Value == "Weekly")
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

            if (Height != null && Height.Value > 0)
            {
                gvPreview.Settings.VerticalScrollableHeight = Convert.ToInt32(Height.Value - 260);
                gvPreview.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
        }

        private void CreateGridSummaryColumn(DevExpress.Web.ASPxGridView gvPreview, string column)
        {
            ASPxSummaryItem summary = new ASPxSummaryItem(column, DevExpress.Data.SummaryItemType.Sum);
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
                ULog.WriteException("Method UpdateRMMAllocationSummary Called Inside Thread In Event gvPreview_BatchUpdate on Page CustomProjectTeamGantt.ascx");
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

                    if (item.FieldName != DatabaseObjects.Columns.Resource && item.FieldName != DatabaseObjects.Columns.Id 
                        && item.FieldName != DatabaseObjects.Columns.Title && item.FieldName != "AllocationType" 
                        && item.FieldName != DatabaseObjects.Columns.AllocationStartDate 
                        && item.FieldName != DatabaseObjects.Columns.AllocationEndDate)
                    {
                        DataRow row = ((System.Data.DataRowView)e.Row).Row;
                        if (((DevExpress.Web.ASPxSummaryItemBase)(e.Item)).Tag == "")
                        {
                            if (UGITUtility.IfColumnExists(((DevExpress.Web.ASPxSummaryItemBase)e.Item).FieldName + "_MonthlyPct", row.Table))
                            {
                                string rowvalue = UGITUtility.ObjectToString(row[((DevExpress.Web.ASPxSummaryItemBase)e.Item).FieldName + "_MonthlyPct"]);
                                string SoftAllocationstring = UGITUtility.ObjectToString(row["SoftAllocation"+((DevExpress.Web.ASPxSummaryItemBase)e.Item).FieldName]);
                                List<string> lstSoftAllocation = UGITUtility.ConvertStringToList(SoftAllocationstring, Constants.Separator6);
                                List<string> lstStrings = UGITUtility.ConvertStringToList(rowvalue, Constants.Separator6);
                                double FTE = 0;
                                for (int i = 0; i < lstStrings.Count; i++)
                                {
                                    bool softallocation = UGITUtility.StringToBoolean(lstSoftAllocation[i]);
                                    if (softallocation)
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

                        //List<UserProfile> lstUProfile;

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
                gvPreview.ExpandAll();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, ex.Message);
            }
        }
    }

    public class GantttGroupRowContentTemplateNew : ITemplate
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

        public GantttGroupRowContentTemplateNew(List<UserProfile> userEditPermisionList, bool isResourceAdmin, bool rbtnPercentage, 
            bool IncludeClosedProjects, string selectedCategory, int monthColWidth)
        {
            MonthColWidth = monthColWidth;
            permisionlist = userEditPermisionList;
            isAdminResource = isResourceAdmin;
            isPercentageMode = rbtnPercentage;
            this.IncludeClosedProjects = IncludeClosedProjects;
            this.lstselectedCategory = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries).ToList();
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

            if (column.Caption == "")
            {
                string strCell = string.Empty;
                
                user = UserManager.GetUserInfoByIdOrName(Container.GroupText);

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

                if(!string.IsNullOrEmpty(user.JobProfile))
                    strCell = string.Format("<div style='width:400px;padding-left:3px;padding-bottom:10px' class='maintitle'><div class='nameLabel'>{0}</div> {2} <div class='roleLabel'>({1})</div></div>", user.Name, user.JobProfile,appendIcons);
                else
                    strCell = string.Format("<div style='width:400px;padding-left:3px;padding-bottom:10px' class='maintitle'><div class='nameLabel'>{0}</div> {2}</div>", user.Name, user.JobProfile, appendIcons);

                cell.Text = strCell;
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
                double pct = UGITUtility.StringToDouble(summarytext);
                //100% or more - over allocated, 90% to 100% is near Max, 30% to 80% is available, < 30% is under allocated
                int colorcode = 2;
                if (pct >= 100)
                    colorcode = 6;
                else if (pct >= 90 && pct < 100)
                    colorcode = 5;
                else if (pct >= 30 && pct < 90)
                    colorcode = 4;
                else
                    colorcode = 3;

                var childrowscount = Grid.GetChildRowCount(Container.VisibleIndex);
                int projectCount = 0;
                for (int i = 0; i < childrowscount; i++)
                {
                    DataRowView datarow = Grid.GetChildRow(Container.VisibleIndex, i) as DataRowView;
                    if (datarow != null)
                    {
                        GridViewDataColumn col = column as GridViewDataColumn;
                        DataRow row = datarow.Row;
                        if (UGITUtility.IfColumnExists(row, col.FieldName))
                        {
                            double rowpct = UGITUtility.StringToDouble(row[col.FieldName]);
                            if (rowpct > 0)
                                projectCount++;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(summarytext))
                {

                    if (projectCount > 0)
                        text = $"<div style='width:{MonthColWidth}px;' class='headerColorBox custom-task-color-{colorcode}' >{summarytext}% <br>{projectCount} Proj.</div>";
                    else
                    {
                        string value = string.Format("{0:0.00}", Math.Round(UGITUtility.StringToDouble(summarytext) / 100, 2));
                        text = $"<div  style='width:{MonthColWidth}px;' class='headerColorBox custom-task-color-{colorcode}' >{value}</div>";
                    }
                }
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

    class GanttTitleHeaderTemplate : ITemplate
    {
        string _selectedUser = string.Empty;
        public GanttTitleHeaderTemplate(string user)
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

    public class MonthDownHeaderTemplateProjGantt : ITemplate
    {
        GridViewDataTextColumn colID = null;
        string _mode = null;
        public MonthDownHeaderTemplateProjGantt(GridViewDataTextColumn coID, string mode)
        {
            this.colID = coID;
            _mode = mode;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            HyperLink hnkButton = new HyperLink();
            hnkButton.Text = this.colID.Caption;
            if (_mode == "Monthly")
                hnkButton.CssClass = "hand-cursor";
            else
                hnkButton.CssClass = "pointer";
            container.Controls.Add(hnkButton);

            string func = string.Format("ClickOnDrillDown(this,'{0}','{1}')", colID.FieldName, colID.Caption);
            hnkButton.Attributes.Add("onclick", func);

        }

        #endregion
    }

    public class MonthUpGridViewBandColumnProjGantt : ITemplate
    {
        GridViewBandColumn colBDC = null;
        string _mode = null;
        public MonthUpGridViewBandColumnProjGantt(GridViewBandColumn coID, string mode)
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
            if (_mode == "Weekly")
            {
                hnkBDButton.CssClass = "hand-cursor";

                //container.Controls.Add(hnkBDButton);
                string func = string.Format("ClickOnDrillUP(this,'{0}')", colBDC.Caption);
                //hnkBDButton.Attributes.Add("onclick", func);
                HContainer.Attributes.Add("onclick", func);
            }
            else
                HContainer.Attributes.Add("class", "pointer");
            HContainer.Controls.Add(hnkBDButton);
            container.Controls.Add(HContainer);

        }

        #endregion
    }
}
