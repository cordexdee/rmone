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
using System.Drawing;
using uGovernIT.Manager;
using DevExpress.Data;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using DevExpress.XtraScheduler.Native;
using DevExpress.ExpressApp;
using DevExpress.XtraGrid;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class ProjectGantt : System.Web.UI.UserControl
    {
        public String TicketID { get; set; }
        protected ZoomLevel zoomLevel;
        protected bool IsCustomCallBack = false;
        public Unit Height { get; set; }
        public Unit Width { get; set; }
        public string UserAll { get; set; }
        public string SelectedUsers { get; set; }
        public string SelectedYear { get; set; }
        public string IncludeClosed { get; set; }
        public bool HideAllocationType { get; set; }
        public string MultiAddUrl { get; set; }
        public const string addMultiAllocationUrl = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&Type={2}&SelectedUsersList={3}&IncludeClosed={4}";

        private DateTime dateFrom;
        private DateTime dateTo;
        public bool btnexport;

        private string selectedCategory = string.Empty;
        public string SelectedUser = string.Empty;
        public string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Type={2}";
        public string editUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=addworkitem&Type=ResourceAllocation&WorkItemType=Time Off");
        ConfigurationVariableManager ConfigVariableMGR = null;
        ConfigurationVariable cvAllocationTimeLineColor;   // = ConfigurationVariable.Load("AllocationTimeLineColor");
        List<string> lstEstimateColors = null;
        List<string> lstEstimateColorsAndFontColors = null;
        List<string> lstAssignColors = null;
        List<string> lstAssignColorsAndFontColors = null;
        public bool enableStudioDivisionHierarchy;

        protected bool isResourceAdmin = false;

        protected List<UserProfile> userProfiles = null;
        DataTable allocationData = null;
        private bool DisablePlannedAllocation;
        protected bool enableDivision;
        private string erpLabel = string.Empty;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private UserProfileManager ObjUserProfileManager = null;
        private ResourceAllocationManager ResourceAllocationManager = null;
        private ResourceWorkItemsManager ResourceWorkItemsManager = null;
        public string deptCtrlId;
        private int MonthColWidth = 45;
        protected bool reCreateGridColumns;
        public string DivisionLabel { get; set; }
        

        protected override void OnInit(EventArgs e)
        {
            ConfigVariableMGR = new ConfigurationVariableManager(context);
            ObjUserProfileManager = new UserProfileManager(context);
            ResourceAllocationManager = new ResourceAllocationManager(context);
            ResourceWorkItemsManager = new ResourceWorkItemsManager(context);
            cvAllocationTimeLineColor = ConfigVariableMGR.LoadVaribale("AllocationTimeLineColor");
            DisablePlannedAllocation = ConfigVariableMGR.GetValueAsBool(ConfigConstants.DisablePlannedAllocation);
            userProfiles = ObjUserProfileManager.GetUsersProfile();
            enableDivision = ConfigVariableMGR.GetValueAsBool(ConfigConstants.EnableDivision);
            erpLabel = ConfigVariableMGR.GetValue(ConfigConstants.ERPIDLABEL);
            DivisionLabel = ConfigVariableMGR.GetValue(ConfigConstants.DivisionLabel);
            MultiAddUrl = UGITUtility.GetAbsoluteURL(string.Format(addMultiAllocationUrl, "addmultiallocation", "Add Multiple Allocations", SelectedUser, "", IncludeClosed));
            enableStudioDivisionHierarchy = ConfigVariableMGR.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);

            if (string.IsNullOrEmpty(Request["DateTo"]) && string.IsNullOrEmpty(Request["DateFrom"]))
            {
                string syear = Request[hndYear.UniqueID];
                if (string.IsNullOrEmpty(syear))
                {
                    hndYear.Value = DateTime.Now.Year.ToString();
                    syear = hndYear.Value;
                    SelectedYear = syear;
                }
                //lblSelectedYear.Text = syear;
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

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (pnlResourceUtil.Controls.Count < 1)
            {
                ResourceAvailability resourceUtilization = Page.LoadControl("~/ControlTemplates/RMM/ResourceAvailability.ascx") as ResourceAvailability;
                resourceUtilization.CalledFromDirectorView = true;
                if (hdnDataFilters.Contains("Division"))
                    resourceUtilization.selectedDivision = hdnDataFilters.Get("Division").ToString();
                 
                pnlResourceUtil.Controls.Add(resourceUtilization);
                Control deptControl = (Control)pnlResourceUtil.FindControlRecursive("department");
                deptCtrlId = deptControl.ClientID;
            }
            //Set visibility of the 2 views within the page.
            string activeView = UGITUtility.GetCookieValue(Request, "activeview");
            if (activeView != "ResourceUtil")
            {
                pnlResourceUtil.CssClass += " d-none";
                divGanttgvPreview.Attributes.Add("class", "");
            }
            else
            {
                pnlResourceUtil.CssClass = pnlResourceUtil.CssClass.Replace("d-none", "");
                divGanttgvPreview.Attributes.Add("class", "d-none");
            }
            reCreateGridColumns = false;
            if (!IsPostBack)
            {
                reCreateGridColumns = true;
                string closedfilter = UGITUtility.GetCookieValue(Request, "IncludeClosedAT");
                if (closedfilter == "true")
                {
                    chkIncludeClosed.Checked = true;
                }
                else
                {
                    chkIncludeClosed.Checked = false;
                }
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

        protected void gvPreview_DataBinding(object sender, EventArgs e)
        {
            if (allocationData == null)
            {
                try
                {
                    allocationData = GetAllocationData();  //GetAllocationData();
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, ex.Message);
                }
                if (hdnZoomClicked.Value == "YES")
                {
                    hdnZoomClicked.Value = "NO";
                }
            }
            gvPreview.DataSource = allocationData;
                PrepareAllocationGrid();
        }

        protected void gvPreview_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            string text = e.Cell.Text;

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
                e.DataColumn.FieldName != DatabaseObjects.Columns.Title && e.DataColumn.FieldName != "ResourceAllocationCount")
            {
                int defaultBarH = 12;
                if (DisablePlannedAllocation)
                    defaultBarH = 24;
                string html;
                string type = UGITUtility.ObjectToString(e.GetValue("ProjectNameLink"));

                string tickId = string.Empty;
                string allStartDates = string.Empty;
                string allEndDates = string.Empty;

                string allStartDatesList = string.Empty;
                string allEndDatesList = string.Empty;

                DateTime aStartDate = DateTime.MinValue;
                DateTime aEndDate = DateTime.MinValue;
                DateTime aPreconStart = DateTime.MinValue;
                DateTime aPreconEnd = DateTime.MinValue;
                DateTime aConstStart = DateTime.MinValue;
                DateTime aConstEnd = DateTime.MinValue;
                DateTime aCloseoutStart = DateTime.MinValue;
                DateTime aCloseoutEnd = DateTime.MinValue;
                string workitemIds = string.Empty;
                string id = string.Empty;
                string name = string.Empty;
                bool aSoftAllocation = true;
                string project = string.Empty;
                string allocId = string.Empty;
                string subworkitem = String.Empty;
                string workitemid = string.Empty;
                List<DateTime> lstStartDates = new List<DateTime>();
                List<DateTime> lstEndDates = new List<DateTime>();

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
                    aCloseoutStart = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.CloseoutStartDate]);
                    aCloseoutEnd = UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.CloseoutDate]);
                    aSoftAllocation = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.SoftAllocation]);
                    project = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                    //name = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Name]);
                    //id = row[DatabaseObjects.Columns.Id].ToString();
                    if (aCloseoutEnd == DateTime.MinValue)
                        aCloseoutEnd = aCloseoutStart != DateTime.MinValue ? aCloseoutStart.AddWorkingDays(uHelper.getCloseoutperiod(context)) : DateTime.MinValue;
                    allStartDates = UGITUtility.ObjectToString(row["AllStartDates"]);
                    allEndDates = UGITUtility.ObjectToString(row["allEndDates"]);

                    lstStartDates.Add(aStartDate); lstEndDates.Add(aEndDate);
                    lstStartDates.Add(aPreconStart); lstEndDates.Add(aPreconEnd);
                    lstStartDates.Add(aConstStart); lstEndDates.Add(aConstEnd);
                    lstStartDates.Add(aCloseoutStart); lstEndDates.Add(aConstEnd);

                }
                //if (tickId != "CPR-23-000744")  //debugging code
                //    return;
                if (string.IsNullOrEmpty(allStartDates))
                {
                    allStartDates = $"{aPreconStart},{aConstStart},{aCloseoutStart}";
                    allEndDates = $"{aPreconEnd},{aConstEnd},{aCloseoutEnd.AddDays(1)}";
                }
                //else
                //{
                //    allStartDates = $"{allStartDates},{aPreconStart},{aConstStart},{aCloseoutStart}";
                //    allEndDates = $"{allEndDates},{aPreconEnd},{aConstEnd},{aCloseoutEnd.AddDays(1)}";
                //}
                string phaseStartDates = $"{aPreconStart},{aConstStart},{aCloseoutStart}";
                string phaseEndDates = $"{aPreconEnd},{aConstEnd},{aCloseoutEnd}";

                List<DateTime> startdates = null;
                List<DateTime> enddates = null;
                List<(DateTime StartDate, DateTime EndDate)> datePairs = null;
                if (!string.IsNullOrEmpty(allStartDates))
                {
                    startdates = UGITUtility.ConvertStringToDateList(allStartDates);
                    enddates = UGITUtility.ConvertStringToDateList(allEndDates);
                    datePairs = startdates.Zip(enddates, (start, end) => (start, end)).ToList();
                    //Sort the date pairs based on the start dates
                    datePairs.Sort((pair1, pair2) => pair1.StartDate.CompareTo(pair2.StartDate));
                }

                List<DateTime> phStartdates = null;
                List<DateTime> phEnddates = null;
                List<(DateTime StartDate, DateTime EndDate)> phaseDatePairs = null;
                if (!string.IsNullOrEmpty(phaseStartDates))
                {
                    phStartdates = UGITUtility.ConvertStringToDateList(phaseStartDates);
                    phEnddates = UGITUtility.ConvertStringToDateList(phaseEndDates);
                    phaseDatePairs = phStartdates.Zip(phEnddates, (start, end) => (start, end)).ToList();
                    //Sort the date pairs based on the start dates
                    //datePairs.Sort((pair1, pair2) => pair1.StartDate.CompareTo(pair2.StartDate));
                }

                DateTime dateF = Convert.ToDateTime(dateFrom);
                DateTime dateT = Convert.ToDateTime(dateTo);
                for (DateTime dt = dateF; dateT > dt; dt = dt.AddDays(uHelper.GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
                {
                    html = "";
                    string resourceWorkItemColumnName = "ResourceWorkItemLookup" + dt.ToString("MMM-dd-yy") + "E";
                    string resourceWorkItems = null;
                    if (e.DataColumn.FieldName == dt.ToString("MMM-dd-yy") + "E")
                    {
                        DataRow row = gvPreview.GetDataRow(e.VisibleIndex);
                        string estAlloc = Convert.ToString(e.GetValue(dt.ToString("MMM-dd-yy") + "E"));

                        string workItemId = workitemid;
                        string roundleftbordercls = string.Empty;
                        string roundrightbordercls = string.Empty;
                        string backgroundColor = string.Empty;
                        if (aStartDate == DateTime.MinValue || aEndDate == DateTime.MinValue)
                        {
                            aStartDate = aPreconStart;
                            aEndDate = aCloseoutEnd;
                        }
                        //classed to set round corners on bar end points
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

                        backgroundColor = GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt, dt.AddDays(uHelper.GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1), false,
                                                                        aCloseoutStart, aCloseoutEnd);

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

                            html = GenerateGanttCell(foreColor, Estimatecolor, defaultBarH, name, aPreconStart, aPreconEnd, aConstStart, aConstEnd, allocId, startdates, enddates, dt,
                                ref estAlloc, ref roundleftbordercls, ref roundrightbordercls, ref backgroundColor, cell, startDateString, endDateString, resourceWorkItems, datePairs, subworkitem, aCloseoutStart, aCloseoutEnd, phaseDatePairs);
                            //html = GenerateGanttCell(foreColor, Estimatecolor, defaultBarH, name, aPreconStart, aPreconEnd, aConstStart, aConstEnd, allocId, startdates, enddates, dt,
                            //    ref estAlloc, ref roundleftbordercls, ref roundrightbordercls, ref backgroundColor, cell, startDateString, endDateString, resourceWorkItems, datePairs, subworkitem, aCloseoutStart, aCloseoutEnd);
                            html = $"<div class='flex-container'>{html}</div>";
                        }
                        else
                        {
                            if (backgroundColor == "nobgcolor")
                            {
                                html = $" <div id='gCell_{cell}' runat='server'  " +
                                    $"style='float:left;width:100%;padding-left:{defaultBarH}px;height: {defaultBarH}px;'></div>";
                            }
                            else
                            {
                                html = GenerateGanttCell(foreColor, Estimatecolor, defaultBarH, name, aPreconStart, aPreconEnd, aConstStart, aConstEnd, allocId, startdates, enddates, dt,
                                    ref estAlloc, ref roundleftbordercls, ref roundrightbordercls, ref backgroundColor, cell, startDateString, endDateString, resourceWorkItems, datePairs, subworkitem, aCloseoutStart, aCloseoutEnd, phaseDatePairs);
                                html = $"<div class='flex-container'>{html}</div>";

                            }

                        }
                        e.Cell.Text = html;
                        //sets alternate color to columns
                        if (e.DataColumn.VisibleIndex % 2 == 0)
                            e.Cell.BackColor = Color.WhiteSmoke;
                        return;
                    }


                }
            }

        }
        public class DateAndType
        {
            public DateTime ProjectDate;
            public string DateType;
        }

        public static string GetGanttCellBackGroundColor(ApplicationContext context, DateTime aPreconStart, DateTime aPreconEnd, DateTime aConstStart,
DateTime aConstEnd, DateTime dt, DateTime? dtEnd = null, bool aSoftAllocation = false, DateTime aCloseoutStart = default(DateTime), DateTime aCloseoutEnd = default(DateTime))
        {
            string backgroundColor = string.Empty;
            if (uHelper.IsCPRModuleEnabled(context))
            {
                dtEnd = dtEnd == null ? new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month)) : dtEnd;
                //classes to set color based on dates
                if ((dt >= aPreconStart && dt <= aPreconEnd) && (dtEnd >= aConstStart && dtEnd <= aConstEnd))
                    backgroundColor = "preconbgcolor-constbgcolor";
                else if ((dt >= aConstStart && dt <= aConstEnd) && (aConstEnd != DateTime.MinValue && dtEnd > aConstEnd))
                    backgroundColor = "constbgcolor-closeoutbgcolor";
                else if (dt >= aPreconStart && dt <= aPreconEnd)
                    backgroundColor = "preconbgcolor";
                else if (dt >= aConstStart && dt <= aConstEnd)
                    backgroundColor = "constbgcolor";
                //use this condition if closeout dates are provided
                else if (aCloseoutStart != DateTime.MinValue && dt >= aCloseoutStart && dt <= aCloseoutEnd)
                    backgroundColor = "closeoutbgcolor";
                //use this condition if closeout dates are NOT provided. 
                //else if (aConstEnd != DateTime.MinValue && dt > aConstEnd)
                //    backgroundColor = "closeoutbgcolor";
                else if ((dt.Month == aPreconStart.Month && dt.Year == aPreconStart.Year) || (dt.Month == aPreconEnd.Month && dt.Year == aPreconEnd.Year))
                    backgroundColor = "preconbgcolor";
                else if ((dt.Month == aConstStart.Month && dt.Year == aConstStart.Year) || (dt.Month == aConstEnd.Month && dt.Year == aConstEnd.Year))
                    backgroundColor = "constbgcolor";
                else if (dt.Month == aConstEnd.Month && dt.Year == aConstEnd.Year)
                    backgroundColor = "constbgcolor";
                else if (dt.Month == aConstEnd.Month && dt.Year == aConstEnd.Year)
                    backgroundColor = "constbgcolor";
                else
                    backgroundColor = "nobgcolor"; // if allocation does not falls in any stage consider it as precon stage
            }
            else
                backgroundColor = "itsmassignedbgcolor";
            return backgroundColor;
        }

        private string GenerateGanttCell(string foreColor, string Estimatecolor, int defaultBarH, string name, DateTime aPreconStart, DateTime aPreconEnd, DateTime aConstStart, DateTime aConstEnd,
   string allocId, List<DateTime> startdates, List<DateTime> enddates, DateTime dt, ref string estAlloc, ref string roundleftbordercls,
   ref string roundrightbordercls, ref string backgroundColor, string cell, string startDateString, string endDateString, string workItemId,
   List<(DateTime StartDate, DateTime EndDate)> datePairs, string subworkitem, DateTime aCloseoutStart, DateTime aCloseoutEnd,
   List<(DateTime StartDate, DateTime EndDate)> phaseDatePairs)
        {
            string html = string.Empty;
            if (!string.IsNullOrEmpty(estAlloc) && estAlloc != "0")
                estAlloc = estAlloc + "hr <br>";
            int NoOfDays = 0;
            int remainingDays = 0;
            string widthEstAlloc = "100";
            DateTime phaseEndDate, phaseStartDate;
            DateTime iEndDate, iStartDate;
            bool isLeftRound = false, isRightRound = false;
            if (hdndisplayMode.Value != "Weekly")
            {

                foreach (var item in phaseDatePairs)
                {
                    startDateString = uHelper.GetDateStringInFormat(context, item.StartDate, false);
                    endDateString = uHelper.GetDateStringInFormat(context, item.EndDate, false);
                    phaseStartDate = item.StartDate;
                    phaseEndDate = item.EndDate; 
                    if (phaseStartDate == aPreconStart)
                        backgroundColor = "preconbgcolor";
                    else if (phaseStartDate == aConstStart)
                        backgroundColor = "constbgcolor";
                    else if (phaseStartDate == aCloseoutStart)
                        backgroundColor = "closeoutbgcolor";

                    //Mark the phases if the current column date dt matches with any phase.
                    if (phaseStartDate.Month == dt.Month && phaseStartDate.Year == dt.Year
                        && phaseEndDate.Month == dt.Month && phaseEndDate.Year == dt.Year)
                    {
                        remainingDays = phaseEndDate.Day - dt.Day; //item.EndDate.Day - item.StartDate.Day;
                        widthEstAlloc = Convert.ToString((remainingDays * 100) / 30);
                        roundleftbordercls = "RoundLeftSideCorner";
                        roundrightbordercls = "RoundRightSideCorner";
                        isLeftRound = isRightRound = true;
                        html += GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls,
                            roundrightbordercls, backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart,
                            aPreconEnd, aConstStart, aConstEnd, aCloseoutStart, aCloseoutEnd);
                    }
                    else
                    {
                        if (phaseEndDate.Month == dt.Month && phaseEndDate.Year == dt.Year)
                        //else if (item.EndDate.Month == dt.Month && item.EndDate.Year == dt.Year)
                        {
                            NoOfDays = phaseEndDate.Day;
                            widthEstAlloc = Convert.ToString((NoOfDays * 100) / 30);
                            roundrightbordercls = "RoundRightSideCorner";
                            isRightRound = true;
                            html += GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls,
                                roundrightbordercls, backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart,
                                aPreconEnd, aConstStart, aConstEnd, aCloseoutStart, aCloseoutEnd);
                        }
                        //else if (item.StartDate.Month == dt.Month && item.StartDate.Year == dt.Year)
                        if (phaseStartDate.Month == dt.Month && phaseStartDate.Year == dt.Year)
                        {
                            NoOfDays = phaseStartDate.Day;
                            remainingDays = 30 - NoOfDays;
                            widthEstAlloc = Convert.ToString((remainingDays * 100) / 30);
                            roundleftbordercls = "RoundLeftSideCorner";
                            isLeftRound = true;
                            html += GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls,
                                roundrightbordercls, backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart,
                                aPreconEnd, aConstStart, aConstEnd, aCloseoutStart, aCloseoutEnd);
                        }
                        else
                        {
                            //if whole month falls in a phase
                            if (phaseStartDate <= dt && uHelper.LastDayOfMonth(dt) <= phaseEndDate)
                            //                            if (string.IsNullOrEmpty(html) && backgroundColor != "nobgcolor")
                            {
                                var matchingPair = phaseDatePairs.FirstOrDefault(pair => dt >= pair.StartDate && dt <= pair.EndDate);
                                startDateString = uHelper.GetDateStringInFormat(context, matchingPair.StartDate, false);
                                endDateString = uHelper.GetDateStringInFormat(context, matchingPair.EndDate, false);
                                html = GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls, roundrightbordercls,
                                    backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart, aPreconEnd, aConstStart,
                                    aConstEnd, aCloseoutStart, aCloseoutEnd);
                                return html;
                            }
                        }
                    }
                    roundrightbordercls = "";
                    roundleftbordercls = "";
                }


                backgroundColor = GetGanttCellBackGroundColor(context, aPreconStart, aPreconEnd, aConstStart, aConstEnd, dt, dt.AddDays(uHelper.GetDaysForDisplayMode(hdndisplayMode.Value, dt)).AddDays(-1));
                if (backgroundColor.Contains("pre"))
                {
                    phaseStartDate = aPreconStart;
                    phaseEndDate = aPreconEnd;
                }
                else if (backgroundColor.Contains("const"))
                {
                    phaseStartDate = aConstStart;
                    phaseEndDate = aConstEnd;
                }
                else if (backgroundColor.Contains("close"))
                {
                    phaseStartDate = aCloseoutStart;
                    phaseEndDate = aCloseoutEnd;
                }
                else
                {
                    phaseStartDate = DateTime.MinValue;
                    phaseEndDate = DateTime.MinValue;
                }

                //For each allocation pair, check if it lies before precon or after closeout phase. Show it with gray. 
                foreach (var item in datePairs)
                {
                    startDateString = uHelper.GetDateStringInFormat(context, item.StartDate, false);
                    endDateString = uHelper.GetDateStringInFormat(context, item.EndDate, false);
                    iStartDate = item.StartDate;
                    iEndDate = item.EndDate;
                    if (item.StartDate < aPreconStart || aCloseoutEnd < item.EndDate)
                    {
                        backgroundColor = "nobgcolor";
                        if (aCloseoutEnd < item.EndDate)
                            iStartDate = aCloseoutEnd.AddDays(1);

                        if (item.StartDate < aPreconStart)
                            iEndDate = aPreconStart.AddDays(-1);


                        if ((iStartDate.Month == dt.Month && iStartDate.Year == dt.Year)
                        && (iEndDate.Month == dt.Month && iEndDate.Year == dt.Year))
                        {
                            remainingDays = iEndDate.Day - iStartDate.Day; //phaseEndDate.Day - dt.Day ;item.EndDate.Day - item.StartDate.Day
                            widthEstAlloc = Convert.ToString((remainingDays * 100) / 30);
                            roundleftbordercls = isLeftRound ? "" : "RoundLeftSideCorner";
                            roundrightbordercls = isRightRound ? "" : "RoundRightSideCorner";
                            isLeftRound = !isLeftRound;
                            isRightRound = !isRightRound;
                            html += GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls,
                                roundrightbordercls, backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart,
                                aPreconEnd, aConstStart, aConstEnd, aCloseoutStart, aCloseoutEnd);

                        }
                        else if (iEndDate.Month == dt.Month && iEndDate.Year == dt.Year && !isRightRound)
                        {
                            NoOfDays = iEndDate.Day;
                            widthEstAlloc = Convert.ToString((NoOfDays * 100) / 30);
                            roundrightbordercls = isRightRound ? "" : "RoundRightSideCorner";
                            isRightRound = !isRightRound;
                            html += GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls,
                                roundrightbordercls, backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart,
                                aPreconEnd, aConstStart, aConstEnd, aCloseoutStart, aCloseoutEnd);
                        }
                        else if (iStartDate.Month == dt.Month && iStartDate.Year == dt.Year && !isLeftRound)
                        {
                            NoOfDays = iStartDate.Day;
                            remainingDays = 30 - NoOfDays;
                            widthEstAlloc = Convert.ToString((remainingDays * 100) / 30);
                            roundleftbordercls = isLeftRound ? "" : "RoundLeftSideCorner";
                            isLeftRound = !isLeftRound;
                            html += GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls,
                                roundrightbordercls, backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart,
                                aPreconEnd, aConstStart, aConstEnd, aCloseoutStart, aCloseoutEnd);
                        }

                    }
                }

            }

            if (string.IsNullOrEmpty(html))
            {
                var matchingPair = datePairs.FirstOrDefault(pair => dt >= pair.StartDate && dt <= pair.EndDate);
                startDateString = uHelper.GetDateStringInFormat(context, matchingPair.StartDate, false);
                endDateString = uHelper.GetDateStringInFormat(context, matchingPair.EndDate, false);
                html = GetCellDivBasedOnEstAlloc(foreColor, Estimatecolor, defaultBarH, name, allocId, estAlloc, roundleftbordercls, roundrightbordercls,
                    backgroundColor, cell, startDateString, endDateString, widthEstAlloc, "", subworkitem, aPreconStart, aPreconEnd, aConstStart,
                    aConstEnd, aCloseoutStart, aCloseoutEnd);
            }
            return html;
        }

        private string GetCellDivBasedOnEstAlloc(string foreColor, string Estimatecolor, int defaultBarH, string name, string allocId, string estAlloc,
            string roundleftbordercls, string roundrightbordercls, string backgroundColor, string cell, string startDateString, string endDateString,
            string widthEstAlloc, string workItemId,
            string subworkitem, DateTime aPreconStart, DateTime aPreconEnd, DateTime aConstStart, DateTime aConstEnd, DateTime aCloseoutStart, DateTime aCloseoutEnd)
        {
            string html;
            string margin_left = "";
            bool isEditAllowed = ConfigVariableMGR.GetValueAsBool(ConfigConstants.AllowEditInGantt);
            if (!string.IsNullOrEmpty(widthEstAlloc))
            {
                if (!string.IsNullOrEmpty(roundleftbordercls) && !string.IsNullOrEmpty(roundrightbordercls))
                    margin_left = "margin-left: 1px";
                    ///margin_left = string.Format("margin-left: {0}%;", (UGITUtility.StringToDateTime(startDateString).Day * 100 / 30));
                    //The above code is not working here when there are areas to be marked on gantt before or after the current one in the same month.
                if (Convert.ToInt32(widthEstAlloc) >= 35)
                {

                    html = $"<div id='gCell_{cell}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor} overLappingDiv pt-1' " +
                        $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{estAlloc.Replace(" <br>", string.Empty)}\",\"{subworkitem}\",\"{backgroundColor}\")' onmouseout='hideTooltip(this)' " +
                        $"style='flex:0 0 {widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;{margin_left}'>{estAlloc}</div>";

                }
                else
                {

                    html = $"<div id='gCell_{cell}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor} overLappingDiv pt-1' " +
                        $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{estAlloc.Replace(" <br>", string.Empty)}\",\"{subworkitem}\",\"{backgroundColor}\")' onmouseout='hideTooltip(this)' " +
                        $"style='flex:0 0 {widthEstAlloc}%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;{margin_left}'></div>";

                }
            }
            else
            {
                html = $"<div id='gCell_{cell}' runat='server' class='{roundleftbordercls} {roundrightbordercls} {backgroundColor} overLappingDiv pt-1' " +
                    $"onmouseover='showTooltip(this,\"{startDateString}\", \"{endDateString}\",\"{name}\",\"{estAlloc.Replace(" <br>", string.Empty)}\",\"{subworkitem}\",\"{backgroundColor}\")' onmouseout='hideTooltip(this)' " +
                    $"style='flex:0 0 100%;color:{foreColor};background-color: {Estimatecolor};height: {defaultBarH}px;padding-top:4px;{margin_left}'>{estAlloc}</div>";
            }

            return html;
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
                //string userid = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Id]);
                string cmicno = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ERPJobID]);
                string resourceAllocationCount = UGITUtility.ObjectToString(row["ResourceAllocationCount"]);
                string companyname = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.CRMCompanyLookup]);
                if (!string.IsNullOrEmpty(companyname) && companyname.Length > 32)
                    companyname = string.Format("{0}..", companyname.Substring(0, 30));
                string title = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                string ticketID = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                string aHref = UGITUtility.GetHrefFromATagString(Convert.ToString(row["ProjectNameLink"]));
                double approxContractValue = UGITUtility.StringToDouble(row[DatabaseObjects.Columns.ApproxContractValue]);
                string onHoldCss = row[DatabaseObjects.Columns.OnHold].ToString() == "1" ? "glabel2red" : "glabel2";
                string cmicnoLabel = row[DatabaseObjects.Columns.OnHold].ToString() == "1" ? "cmicnoLabelred" : "cmicnoLabel";
                string companyLabel= row[DatabaseObjects.Columns.OnHold].ToString() == "1" ? "cmicnoLabelred" : "";
                if (string.IsNullOrEmpty(ticketID))
                {
                    if (!string.IsNullOrEmpty(cmicno))
                        e.Row.Cells[1].Text = $"<div  style='float:left;' >" +
                                $"<div class='glabelleftalign' onClick='{aHref}'><div class='{companyLabel}'>{companyname} {erpLabel}</div> <div class='{cmicnoLabel}'>{cmicno}</div></div>" +
                                $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                                $"</div>";
                    else
                        e.Row.Cells[1].Text = $"<div style='float:left;' >" +
                            $"<div class='glabel1' onClick='{aHref}'><div class='{companyLabel}'>{companyname}</div></div>" +
                            $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                            $"</div>";
                }

                if (!string.IsNullOrEmpty(cmicno))
                    e.Row.Cells[1].Text = $"<div  style='float:left;' >" +
                            $"<div class='glabelleftalign' onClick='{aHref}'><div class='{companyLabel}'>{companyname} {erpLabel}</div> <div class='{cmicnoLabel}'>{cmicno}</div></div>" +
                            $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                            $"</div>";
                else
                    e.Row.Cells[1].Text = $"<div style='float:left;' >" +
                            $"<div class='glabel1' onClick='{aHref}'><div class='{companyLabel}'>{companyname}</div></div>" +
                            $"<div class='{onHoldCss}'>{row["ProjectNameLink"]}</div>" +
                            $"</div>";

                if(((DevExpress.Web.GridViewDataColumn)((DevExpress.Web.Rendering.GridViewTableBaseCell)e.Row.Cells[0]).Column).FieldName== UGITUtility.ObjectToString(DatabaseObjects.Columns.ShortName))
                {
                    string moduleShortName = UGITUtility.TruncateWithEllipsis(Convert.ToString(row[DatabaseObjects.Columns.ShortName]), 10);
                    e.Row.Cells[0].Text = $"<span title=\"{Convert.ToString(row[DatabaseObjects.Columns.ShortName])}\">{moduleShortName}</span>";
                }
                if (!string.IsNullOrEmpty(resourceAllocationCount))
                {
                    string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
                    string ModuleName = uHelper.getModuleNameByTicketId(ticketID);
                    string path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + ticketID + "&module=" + ModuleName;
                    string func = string.Format("event.stopPropagation(); javascript:UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path, string.Format("moduleName={0}&ConfirmBeforeClose=true", ModuleName), title, sourceURL);
                    e.Row.Cells[2].Attributes.Add("onClick", func);
                    e.Row.Cells[2].Attributes.Add("style", "cursor: pointer;");

                    string[] values = UGITUtility.SplitString(resourceAllocationCount, Constants.Separator);
                    int totalAllocation = int.Parse(values[0]);
                    int unAllocatedResource = int.Parse(values[1]);
                    int filledAllocation = totalAllocation - unAllocatedResource;
                    if (filledAllocation > 0)
                    {
                        if (filledAllocation == totalAllocation)
                        {
                            e.Row.Cells[2].Text = string.Format("<span class='resourceAllocationBlue{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, "");
                        }
                        else
                        {
                            e.Row.Cells[2].Text = string.Format("<span class='resourceAllocationRed{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, "");
                        }
                    }
                    else
                    {
                        e.Row.Cells[2].Text = string.Format("<span class='resourceAllocationRed{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, "");
                    }
                }
                e.Row.Cells[3].Text = uHelper.FormatNumber(approxContractValue, "currencywithoutdecimal");
            }
        }
        private DataTable LoadAllocationMonthlyView()
        {
            ResourceAllocationMonthlyManager allocationMonthlyManager = new ResourceAllocationMonthlyManager(context);
            return allocationMonthlyManager.LoadAllocationMonthlyView(Convert.ToDateTime(dateFrom),
                Convert.ToDateTime(dateTo), false);

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

        private void ViewTypeAllocation(DataTable data, DataRow newRow, DataRow[] dttemp, bool Assigned = true)
        {
            string workitemcolumn = hdndisplayMode.Value == "Weekly" ? DatabaseObjects.Columns.WorkItemID : DatabaseObjects.Columns.ResourceWorkItemLookup;
            DateTime AllocationStartDate;
            DateTime AllocationMonthStartDate;
            DateTime allocationEndDate;
            DateTime rowStartDate = DateTime.MinValue;
            foreach (DataRow rowitem in dttemp)
            {
                string workitem = UGITUtility.ObjectToString(rowitem[workitemcolumn]);
                string monthlyPct = UGITUtility.ObjectToString(rowitem[DatabaseObjects.Columns.PctAllocation]);
                if (hdndisplayMode.Value == "Weekly")
                {
                    rowStartDate = UGITUtility.StringToDateTime(rowitem[DatabaseObjects.Columns.WeekStartDate]);
                    AllocationStartDate = UGITUtility.StringToDateTime(rowitem[DatabaseObjects.Columns.ActualStartDate]);
                    AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                    allocationEndDate = UGITUtility.StringToDateTime(rowitem[DatabaseObjects.Columns.ActualEndDate]);
                    if (data.Columns.Contains(rowStartDate.ToString("MMM-dd-yy") + "E") && !String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        newRow[rowStartDate.ToString("MMM-dd-yy") + "E"] =
                            Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);

                        if (!data.Columns.Contains(rowStartDate.ToString("MMM-dd-yy") + "_WorkItem"))
                            data.Columns.Add(rowStartDate.ToString("MMM-dd-yy") + "_WorkItem", typeof(string));
                        newRow[rowStartDate.ToString("MMM-dd-yy") + "_WorkItem"] = workitem;
                        if (!data.Columns.Contains("ResourceWorkItemLookup" + rowStartDate.ToString("MMM-dd-yy") + "E"))
                        {
                            data.Columns.Add("ResourceWorkItemLookup" + rowStartDate.ToString("MMM-dd-yy") + "E", typeof(string));
                        }
                        newRow["ResourceWorkItemLookup" + rowStartDate.ToString("MMM-dd-yy") + "E"]
                            += AllocationStartDate + Constants.Separator6 + allocationEndDate + Constants.Separator6 + workitem + ";#";

                        if (!data.Columns.Contains(rowStartDate.ToString("MMM-dd-yy") + "E_MonthlyPct"))
                            data.Columns.Add(rowStartDate.ToString("MMM-dd-yy") + "E_MonthlyPct", typeof(string));
                        newRow[rowStartDate.ToString("MMM-dd-yy") + "E_MonthlyPct"] = monthlyPct;
                    }


                }
                else
                {
                    rowStartDate = UGITUtility.StringToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]);
                    if (!String.IsNullOrEmpty(Convert.ToString(newRow[DatabaseObjects.Columns.AllocationStartDate])))
                    {
                        AllocationStartDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationStartDate]);
                        AllocationMonthStartDate = new DateTime(AllocationStartDate.Year, AllocationStartDate.Month, 1);
                        allocationEndDate = UGITUtility.StringToDateTime(newRow[DatabaseObjects.Columns.AllocationEndDate]);

                        if (rowStartDate >= AllocationMonthStartDate && rowStartDate <= allocationEndDate)
                        {
                            if (data.Columns.Contains(rowStartDate.ToString("MMM-dd-yy") + "E"))
                            {
                                newRow[rowStartDate.ToString("MMM-dd-yy") + "E"] = UGITUtility.StringToDouble(newRow[rowStartDate.ToString("MMM-dd-yy") + "E"]) +
                                    Math.Round(UGITUtility.StringToDouble(rowitem[DatabaseObjects.Columns.ActualPctAllocation]), 2);
                                if (!data.Columns.Contains(rowStartDate.ToString("MMM-dd-yy") + "_WorkItem"))
                                    data.Columns.Add(rowStartDate.ToString("MMM-dd-yy") + "_WorkItem", typeof(string));
                                newRow[rowStartDate.ToString("MMM-dd-yy") + "_WorkItem"] += Constants.Separator6 + workitem;

                                if (!data.Columns.Contains("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E"))
                                {
                                    data.Columns.Add("ResourceWorkItemLookup" + Convert.ToDateTime(rowitem[DatabaseObjects.Columns.MonthStartDate]).ToString("MMM-dd-yy") + "E", typeof(string));
                                }
                                newRow["ResourceWorkItemLookup" + rowStartDate.ToString("MMM-dd-yy") + "E"]
                                    += AllocationStartDate + Constants.Separator6 + allocationEndDate + Constants.Separator6 + workitem + ";#";

                                if (!data.Columns.Contains(rowStartDate.ToString("MMM-dd-yy") + "E_MonthlyPct"))
                                    data.Columns.Add(rowStartDate.ToString("MMM-dd-yy") + "E_MonthlyPct", typeof(string));
                                newRow[rowStartDate.ToString("MMM-dd-yy") + "E_MonthlyPct"] += Constants.Separator6 + monthlyPct;
                            }
                        }
                    }


                }

            }

        }

        private void PrepareAllocationGrid()
        {
            if (reCreateGridColumns)
                gvPreview.Columns.Clear();
            else
                gvPreview.Columns.RemoveAt(gvPreview.Columns.Count - 1);
            int fullScreenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "screenWidth"));
            int noOfColumns = 14;
            if (hdndisplayMode.Value == "Weekly")
                noOfColumns = 16;
            if (fullScreenWidth > 0)
            {
                int remainingwidthforcols = fullScreenWidth - 370;
                MonthColWidth = UGITUtility.StringToInt(remainingwidthforcols / noOfColumns);
            }

            gvPreview.SettingsPager.AlwaysShowPager = false;
            gvPreview.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

            string selectedPieSlice = string.Empty;
            if (hdnDataFilters.Contains("PieSlice"))
            {
                selectedPieSlice = UGITUtility.ObjectToString(hdnDataFilters.Get("PieSlice"));
            }
            if (reCreateGridColumns)
            {
                GridViewDataTextColumn colId = new GridViewDataTextColumn();

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.TicketId;
                colId.Caption = DatabaseObjects.Columns.TicketId;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Width = new Unit("10px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.HeaderStyle.Font.Bold = true;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.Visible = false;
                colId.FixedStyle = GridViewColumnFixedStyle.Left;
                gvPreview.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.ShortName;
                colId.Caption = "Type";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Width = new Unit("70px");
                colId.FixedStyle = GridViewColumnFixedStyle.Left;
                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                //colId.SortOrder = ColumnSortOrder.Ascending;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                gvPreview.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Title;
                colId.HeaderCaptionTemplate = new GridTitleHeaderTemplate(selectedPieSlice);
                colId.Caption = "Projects";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Width = new Unit("280px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.HeaderStyle.Font.Bold = true;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.FixedStyle = GridViewColumnFixedStyle.Left;
                //colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                //colId.SortOrder = ColumnSortOrder.Ascending;
                //            colId.Settings.SortMode = ColumnSortMode.Custom;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                gvPreview.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = "ResourceAllocationCount";
                colId.Caption = "Alloc";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Width = new Unit("52px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.FixedStyle = GridViewColumnFixedStyle.Left;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                gvPreview.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.ApproxContractValue;
                colId.Caption = "Contr. Val";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Width = new Unit("72px");
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.FixedStyle = GridViewColumnFixedStyle.Left;
                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                colId.Settings.SortMode = ColumnSortMode.Custom;
                gvPreview.Columns.Add(colId);
            }
            GridViewBandColumn bdCol = new GridViewBandColumn();
            string currentDate = string.Empty;


            for (DateTime dt = dateFrom; dateTo > dt; dt = dt.AddDays(uHelper.GetDaysForDisplayMode(hdndisplayMode.Value, dt)))
            {
                if (dt.ToString("yyyy") != currentDate && !string.IsNullOrEmpty(currentDate))
                {
                    bdCol.Name = dt.ToString("MMM-dd-yy") + "E";
                    bdCol.HeaderTemplate = new MonthUpGridViewBandColumn(bdCol, hdndisplayMode.Value);
                    gvPreview.Columns.Add(bdCol);
                    bdCol = new GridViewBandColumn();
                }

                if (dt.ToString("yyyy") != currentDate)
                {
                    bdCol.Caption = dt.ToString("yyyy");
                    bdCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    bdCol.HeaderStyle.Font.Bold = true;
                    bdCol.HeaderTemplate = new MonthUpGridViewBandColumn(bdCol, hdndisplayMode.Value);
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
                if (hdndisplayMode.Value == "Monthly")
                    ColIdData.HeaderTemplate = new MonthDownHeaderTemplate(ColIdData);

                ColIdData.Width = new Unit(MonthColWidth + "px");
                ColIdData.ExportWidth = 38;

                bdCol.Columns.Add(ColIdData);
            }
            gvPreview.Columns.Add(bdCol);

            if (Height != null && Height.Value > 0)
            {
                gvPreview.Settings.VerticalScrollableHeight = Convert.ToInt32(Height.Value - 260);
                gvPreview.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            }
        }

        protected void gvPreview_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.ApproxContractValue)
            {
                float val1 = UGITUtility.StringToFloat(e.Value1);
                float val2 = UGITUtility.StringToFloat(e.Value2);
                e.Handled = true;
                e.Result = val1.CompareTo(val2);
            }
            if (e.Column.FieldName == "ResourceAllocationCount")
            {
                string alloc1 = UGITUtility.ObjectToString(e.GetRow1Value("ResourceAllocationCount"));
                string alloc2 = UGITUtility.ObjectToString(e.GetRow2Value("ResourceAllocationCount"));
                string[] values1 = UGITUtility.SplitString(alloc1, Constants.Separator);
                string[] values2 = UGITUtility.SplitString(alloc2, Constants.Separator);
                int unAllocatedResource1 = int.Parse(values1[1]);
                int unAllocatedResource2 = int.Parse(values2[1]);
                //Sorting is based on unfilled allocations.
                e.Handled = true;
                e.Result = unAllocatedResource1.CompareTo(unAllocatedResource2);
            }
            if (e.Column.FieldName == DatabaseObjects.Columns.Title)
            {
                object name1 = e.GetRow1Value(DatabaseObjects.Columns.Title);
                object name2 = e.GetRow2Value(DatabaseObjects.Columns.Title);
                e.Handled = true;
                e.Result = System.Collections.Comparer.Default.Compare(name1, name2);
            }

        }

        protected void gvPreview_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                // Bind the updated data to the grid
                reCreateGridColumns = true;
                gvPreview.DataBind();
                gvPreview.ExpandAll();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, ex.Message);
            }
        }

        public static List<Tuple<DateTime, DateTime, string>> ConvertStringToTupleList(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            //throw new ArgumentException("Input string is null or empty.");

            List<string> tupleStrings = UGITUtility.ConvertStringToList(input, Constants.Separator);
            var tupleList = new List<Tuple<DateTime, DateTime, string>>();

            foreach (string tupleString in tupleStrings)
            {
                string[] dateParts = tupleString.Split(',');
                if (dateParts.Length != 3)
                    throw new ArgumentException("Invalid date format in the input string.");

                DateTime date1 = UGITUtility.StringToDateTime(dateParts[0]), date2 = UGITUtility.StringToDateTime(dateParts[1]);
                tupleList.Add(Tuple.Create(UGITUtility.StringToDateTime(date1), UGITUtility.StringToDateTime(date2), dateParts[2]));
            }

            return tupleList;
        }

        private DataTable GetAllocationData()
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.ID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Name, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            data.Columns.Add("ProjectTitle", typeof(string));
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
            data.Columns.Add(DatabaseObjects.Columns.ERPJobID, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ContactLookup, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.CRMCompanyLookup, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.PreconStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.PreconEndDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionStart, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionEnd, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.CloseoutStartDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.CloseoutDate, typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.SoftAllocation, typeof(bool));
            data.Columns.Add("NCO", typeof(string));
            data.Columns.Add("AllStartDates", typeof(string));
            data.Columns.Add("AllEndDates", typeof(string));
            data.Columns.Add("ResourceAllocationCount", typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.OnHold, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ShortName, typeof(string));

            DataTable dtlist = new DataTable();
            if (hdndisplayMode.Value == "Weekly")
            {
                string selectedMonth = UGITUtility.ObjectToString(Request["selectedDate"]);

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
                dateFrom = new DateTime(selectedYear, 1, 1);
                dateTo = new DateTime(selectedYear + 1, 1, 1);

            }
            dtlist = ResourceAllocationManager.GetDatelist(hdndisplayMode.Value, dateFrom, dateTo);
            data.Merge(dtlist);
            data.Clear();

            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue)
            {
                if (string.IsNullOrEmpty(hndYear.Value))
                {
                    //lblSelectedYear.Text = DateTime.Now.Year.ToString();
                    //hndYear.Value = lblSelectedYear.Text;
                    hndYear.Value = DateTime.Now.Year.ToString();
                }
                dateFrom = new DateTime(Convert.ToInt16(hndYear.Value), 1, 1);
                dateTo = dateFrom.AddMonths(12);
            }

            List<UserProfile> lstUProfile = new List<UserProfile>();

            bool limitedUsers = false;

            lstUProfile = ObjUserProfileManager.GetUsersProfile();


            if (!string.IsNullOrEmpty(SelectedUser) && Request["RequestFromProjectAllocation"] == null)
            {
                limitedUsers = true;
            }

            if (!string.IsNullOrEmpty(TicketID))
            {
                limitedUsers = false;
            }

            DataTable dtResult = null;
            DataTable workitems = null;
            List<string> userIds = new List<string>();

            //// temp code
            //limitedUsers = true;
            //SelectedUsers = "bd583418-4326-499c-b72d-3d1091add2be";
            if (limitedUsers)
            {

                if (!string.IsNullOrEmpty(SelectedUsers) && SelectedUsers != "null")
                {
                    userIds = UGITUtility.ConvertStringToList(SelectedUsers, Constants.Separator6);
                    userIds.Distinct();
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
                workitems = RMMSummaryHelper.GetOpenworkitems(context, false);
            }


            if (!string.IsNullOrEmpty(TicketID))
            {
                DataRow[] ticketRows = dtResult.Select("TicketID = " + TicketID);
                dtResult = ticketRows.CopyToDataTable();

                DataRow[] workitemRows = workitems.Select($"{DatabaseObjects.Columns.WorkItem} = {TicketID}");
                workitems = workitemRows.CopyToDataTable();
            }

            //// Filter by Open Tickets.
            //if (!chkIncludeClosed.Checked)
            //{
            //    List<string> LstClosedTicketIds = new List<string>();
            //    //get closed ticket instead of open ticket and then filter all except closed ticket
            //    DataTable dtClosedTickets = RMMSummaryHelper.GetClosedTicketIds(context);
            //    if (dtClosedTickets != null && dtClosedTickets.Rows.Count > 0)
            //    {
            //        LstClosedTicketIds.AddRange(dtClosedTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));
            //    }
            //    if (dtResult != null && dtResult.Rows.Count > 0)
            //    {
            //        DataRow[] drow = dtResult.AsEnumerable().Where(x => !LstClosedTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId), StringComparer.OrdinalIgnoreCase)).ToArray();
            //        if (drow != null && drow.Length > 0)
            //            dtResult = drow.CopyToDataTable();
            //        else
            //            dtResult.Rows.Clear();
            //    }
            //}
            string sector = string.Empty;
            string selectedPieSlice = string.Empty;

            int division = 0;
            int studio = 0;
            if (hdnDataFilters.Contains("Sector"))
                sector = UGITUtility.ObjectToString(hdnDataFilters.Get("Sector"));
            if (hdnDataFilters.Contains("Division"))
                division = UGITUtility.StringToInt(hdnDataFilters.Get("Division"));
            if (hdnDataFilters.Contains("Studio"))
                studio = UGITUtility.StringToInt(hdnDataFilters.Get("Studio"));
            if (hdnDataFilters.Contains("PieSlice"))
            {
                selectedPieSlice = UGITUtility.ObjectToString(hdnDataFilters.Get("PieSlice"));
                //gvPreview.Columns[1].Caption = selectedPieSlice;
            }
            //Load Tickets object before loop to improve perfromance
            TicketManager objTicketManager = new TicketManager(context);
            DataTable dtAllModuleTickets = objTicketManager.GetAllProjectTickets();
            DataTable dtFilteredModuleTickets = null;
            DataSet dsFilteredTickets = RMMSummaryHelper.GetDirectorViewData(context, sector, division, studio, "", "", chkIncludeClosed.Checked);

            if (dsFilteredTickets == null || dsFilteredTickets.Tables.Count < 2 || dsFilteredTickets.Tables[1].Rows.Count == 0)
                return data;

            //DataTable dtProjects = dsFilteredTickets.Tables[1];
            //if (!chkIncludeClosed.Checked)
            //{
            //    DataRow[] rows = dtProjects.Select($"{DatabaseObjects.Columns.Closed} <> 'True'");
            //    if (rows != null && rows.Length > 0)
            //        dtProjects = rows.CopyToDataTable();
            //}

                var results = from table1 in dtAllModuleTickets.AsEnumerable()
                          join table2 in dsFilteredTickets.Tables[1].AsEnumerable() on table1["TicketId"] equals table2["TicketId"]
                          select table1;

            var filteredTickets = from table1 in dtAllModuleTickets.AsEnumerable()
                                  join table2 in dsFilteredTickets.Tables[1].AsEnumerable() on table1["TicketId"] equals table2["TicketId"]
                                  select new { table1, table2 };

            

            dtFilteredModuleTickets = results.CopyToDataTable();
            if (dtResult == null)
                return data;

            var allocationGroupData = dtResult.AsEnumerable()
                //.Where(x => x.Field<string>("ResourceId") == "f6150d04-0ccd-4417-94df-52c29f0e9d74")   //testing code
                .GroupBy(row => new
                {
                    Id = row.Field<string>("WorkItem"),
                })
                .Select(group => new AllocationData()
                {
                    ResourceId = group.First().Field<string>("SubWorkItem"),
                    WorkItem = group.Key.Id,
                    SubWorkItem = group.First().Field<string>("WorkItemLink"),
                    // Add other aggregated properties here
                    AllocationID = UGITUtility.ObjectToString(group.First().Field<long>("ID")), //string.Join(",", group.Select(row => row.Field<long?>("ID")).Select(id => id.HasValue ? id.Value.ToString() : "0")),
                    ResourceUser = group.First().Field<string>("ResourceUser"),
                    WorkItemType = group.First().Field<string>("WorkItemType"),
                    WorkItemLink = group.First().Field<string>("WorkItemLink"),
                    AllocationStartDate = group.Min(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue),
                    AllocationEndDate = group.Max(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue),
                    PctAllocation = group.Average(row => row.Field<double>("PctAllocation")),
                    WorkItemID = string.Join(",", group.Select(row => row.Field<long>("WorkItemID"))),
                    //Title = group.First().Field<string>("Title"),
                    ShowEditButton = false, // group.Select(row => row.Field<bool?>("ShowEditButton")).FirstOrDefault() ?? false,
                    ShowPartialEdit = false, //group.Select(row => row.Field<bool?>("ShowPartialEdit")).FirstOrDefault() ?? false,
                    PlannedStartDate = group.Min(row => row.Field<DateTime?>("PlannedStartDate") ?? DateTime.MinValue),
                    PlannedEndDate = group.Max(row => row.Field<DateTime?>("PlannedEndDate") ?? DateTime.MinValue),
                    PctPlannedAllocation = group.Average(row => row.Field<double?>("PctPlannedAllocation") ?? 0),
                    EstStartDate = group.Min(row => row.Field<DateTime?>("EstStartDate") ?? DateTime.MinValue),
                    EstEndDate = group.Max(row => row.Field<DateTime?>("EstEndDate") ?? DateTime.MinValue),
                    PctEstimatedAllocation = group.Average(row => row.Field<double?>("PctEstimatedAllocation") ?? 0),
                    SoftAllocation = UGITUtility.ObjectToString(group.First().Field<bool>("SoftAllocation")),
                    AllStartDates = string.Join(",", group.Select(row => row.Field<DateTime?>("AllocationStartDate") ?? DateTime.MinValue)),
                    AllEndDates = string.Join(",", group.Select(row => row.Field<DateTime?>("AllocationEndDate") ?? DateTime.MinValue)),
                    OnHold = group.All(o => o[DatabaseObjects.Columns.OnHold].ToString() == "1") ? "1" : "0"
                }).ToArray();

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

            TicketManager ticketManager = new TicketManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            UGITModule module = null;
            string moduleName = string.Empty;
            string ticketID = string.Empty;
            UserProfile userDetails = null;

            foreach (var dr in filteredTickets)
            {

                if (!string.IsNullOrEmpty(selectedPieSlice) && UGITUtility.ObjectToString(dr.table2[DatabaseObjects.Columns.Status]) != selectedPieSlice)
                {
                    continue;
                }
                DataRow newRow = data.NewRow();
                newRow[DatabaseObjects.Columns.TicketId] = ticketID = UGITUtility.ObjectToString(dr.table2[DatabaseObjects.Columns.TicketId]);
                string title = UGITUtility.TruncateWithEllipsis(Convert.ToString(dr.table1[DatabaseObjects.Columns.Title]), 50).Trim();
                newRow[DatabaseObjects.Columns.Title] = title;
                newRow[DatabaseObjects.Columns.Project] = string.Format("{0}: {1}", ticketID, title);
                newRow["ProjectTitle"] = title;
                newRow[DatabaseObjects.Columns.ItemOrder] = 1;
                newRow["ResourceAllocationCount"] = UGITUtility.ObjectToString(dr.table2["ResourceAllocationCount"]);
                newRow[DatabaseObjects.Columns.ApproxContractValue] = dr.table2[DatabaseObjects.Columns.ApproxContractValue];

                if (UGITUtility.IsValidTicketID(ticketID))
                    moduleName = uHelper.getModuleNameByTicketId(ticketID);

                if (!string.IsNullOrEmpty(moduleName))
                    module = moduleManager.GetByName(moduleName);
                if (module == null)
                    continue;

                DataRow dataRow = null;
                newRow[DatabaseObjects.Columns.ShortName] = module.ShortName;
                dataRow = dtFilteredModuleTickets.AsEnumerable().FirstOrDefault(row => row.Field<string>("TicketId") == ticketID);
                if (tempTicketCollection.ContainsKey(ticketID))
                    dataRow = tempTicketCollection[ticketID];
                else
                {
                    dataRow = dtFilteredModuleTickets.AsEnumerable().FirstOrDefault(row => row.Field<string>("TicketId") == ticketID);
                    if (dataRow != null)
                    {
                        tempTicketCollection.Add(ticketID, dataRow);
                        //newRow["ResourceAllocationCount"] = dataRow["ResourceAllocationCount"];
                    }
                }
                if (dataRow != null)
                {
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
                    newRow[DatabaseObjects.Columns.CloseoutStartDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.CloseoutStartDate]);
                    newRow[DatabaseObjects.Columns.CloseoutDate] = UGITUtility.StringToDateTime(dataRow[DatabaseObjects.Columns.CloseoutDate]);                    
                }

                List<AllocationData> lstAllocations = allocationGroupData.Where(x => x.WorkItem == ticketID).ToList();
                string onHoldCss = lstAllocations.Count > 0 ? lstAllocations[0].OnHold.ToString() == "1" ? "red" : "black" : "black";
                newRow["ProjectNameLink"] = string.Format("<a href='{0}' style='color:" + onHoldCss + ";font-weight:800px !important;'>{2}</a>", uHelper.GetHyperLinkControlForTicketID(module,
                ticketID, true, title).NavigateUrl, ticketID, UGITUtility.TruncateWithEllipsis(title, 38));

                newRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                newRow[DatabaseObjects.Columns.ModuleRelativePagePath] = module.DetailPageUrl;

                if (lstAllocations.Count == 0)
                {
                    //Code reverted. Projects without the phase dates need not be exclued from Gantt, as discussed for BTS-24-001561.
                    data.Rows.Add(newRow);
                    continue;
                }
                if (btnexport)
                {
                    newRow[DatabaseObjects.Columns.Resource] = userDetails.Name;
                }
                else
                {
                    newRow[DatabaseObjects.Columns.Resource] = Convert.ToString(lstAllocations[0].ResourceUser);
                }
                newRow[DatabaseObjects.Columns.Name] = newRow[DatabaseObjects.Columns.Resource];
                newRow[DatabaseObjects.Columns.SoftAllocation] = UGITUtility.StringToBoolean(lstAllocations[0].SoftAllocation);
                newRow[DatabaseObjects.Columns.SubWorkItem] = UGITUtility.ObjectToString(lstAllocations[0].SubWorkItem);
                newRow["AllStartDates"] = UGITUtility.ObjectToString(lstAllocations[0].AllStartDates);
                newRow["AllEndDates"] = UGITUtility.ObjectToString(lstAllocations[0].AllEndDates);
                newRow[DatabaseObjects.Columns.OnHold] = lstAllocations[0].OnHold;
                DataRow drWorkItem = null;
                List<string> lstWorkItemIds = UGITUtility.ConvertStringToList(lstAllocations[0].WorkItemID, Constants.Separator6);
                if (workitems != null && workitems.Rows.Count > 0)
                {
                    DataRow[] filterworkitemrow = workitems.AsEnumerable().Where(x => lstWorkItemIds.Contains(UGITUtility.ObjectToString(x.Field<long>("ID")))).ToArray();
                    if (filterworkitemrow != null && filterworkitemrow.Length > 0)
                        drWorkItem = filterworkitemrow[0];
                }
                if (drWorkItem == null)
                    continue;

                string workItem = Convert.ToString(drWorkItem[DatabaseObjects.Columns.WorkItem]);
                string[] arrayModule = selectedCategory.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

                if (!string.IsNullOrEmpty(moduleName))
                {
                    if (true) // can be used for pie chart filter
                    {

                        //check for active modules.
                        if (!UGITUtility.StringToBoolean(module.EnableRMMAllocation))
                            continue;


                        if (dataRow != null)
                        {
                            if (string.IsNullOrEmpty(ticketID))
                            {
                                newRow[DatabaseObjects.Columns.Title] = title;// title;
                                newRow[DatabaseObjects.Columns.Project] = title;
                            }
                            newRow[DatabaseObjects.Columns.TicketId] = workItem;
                            newRow[DatabaseObjects.Columns.WorkItemID] = lstAllocations[0].WorkItemID; // UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(dr.WorkItemID);

                            //condition for add new column for breakup gantt chart...
                            string plannedStartDate = Convert.ToString(lstAllocations[0].PlannedStartDate);
                            string plannedEndDate = Convert.ToString(lstAllocations[0].PlannedEndDate);

                            string estStartDate = Convert.ToString(lstAllocations[0].EstStartDate);
                            string estEndDate = Convert.ToString(lstAllocations[0].EstEndDate);
                            string expression = $"{DatabaseObjects.Columns.TicketId}='{workItem}'";
                            if (data != null && data.Rows.Count > 0)
                            {
                                DataRow[] row = data.Select(expression);

                                if (!string.IsNullOrEmpty(plannedStartDate) && !string.IsNullOrEmpty(plannedEndDate))
                                {
                                    newRow[DatabaseObjects.Columns.PlannedStartDate] = lstAllocations[0].PlannedStartDate;
                                    newRow[DatabaseObjects.Columns.PlannedEndDate] = lstAllocations[0].PlannedEndDate;
                                    newRow["ExtendedDateAssign"] = lstAllocations[0].PlannedStartDate + Constants.Separator1 + lstAllocations[0].PlannedEndDate;

                                    newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(lstAllocations[0].PctPlannedAllocation);
                                }

                                if (!string.IsNullOrEmpty(estStartDate) && !string.IsNullOrEmpty(estEndDate) && estEndDate != "01-01-1753 00:00:00")
                                {
                                    newRow[DatabaseObjects.Columns.AllocationStartDate] = lstAllocations[0].EstStartDate;
                                    newRow[DatabaseObjects.Columns.AllocationEndDate] = lstAllocations[0].EstEndDate;
                                    newRow["ExtendedDate"] = lstAllocations[0].EstStartDate + Constants.Separator1 + lstAllocations[0].EstEndDate;

                                    newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(lstAllocations[0].PctEstimatedAllocation);
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
                                                ViewTypeAllocation(data, newRow, dttemp);
                                        }
                                    }
                                }

                                data.Rows.Add(newRow);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(plannedStartDate) && !string.IsNullOrEmpty(plannedEndDate))
                                {
                                    newRow[DatabaseObjects.Columns.PlannedStartDate] = lstAllocations[0].PlannedStartDate;
                                    newRow[DatabaseObjects.Columns.PlannedEndDate] = lstAllocations[0].PlannedEndDate;
                                    newRow["ExtendedDateAssign"] = lstAllocations[0].PlannedStartDate + Constants.Separator1 + lstAllocations[0].PlannedEndDate;
                                    newRow[DatabaseObjects.Columns.PctPlannedAllocation] = UGITUtility.StringToInt(lstAllocations[0].PctPlannedAllocation);
                                }

                                if (!string.IsNullOrEmpty(estStartDate) && !string.IsNullOrEmpty(estEndDate))
                                {
                                    newRow[DatabaseObjects.Columns.AllocationStartDate] = lstAllocations[0].EstStartDate;
                                    newRow[DatabaseObjects.Columns.AllocationEndDate] = lstAllocations[0].EstEndDate;
                                    newRow["ExtendedDate"] = lstAllocations[0].EstStartDate + Constants.Separator1 + lstAllocations[0].EstEndDate;
                                    newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(lstAllocations[0].PctEstimatedAllocation);
                                }
                                string onHoldlinkCss = lstAllocations.Count > 0 ? lstAllocations[0].OnHold.ToString() == "1" ? "red" : "black" : "black";
                                newRow["ProjectNameLink"] = string.Format("<a href='{0}' title='{2}' style='color:"+ onHoldlinkCss + ";font-weight:800px !important;'>{1}</a>", uHelper.GetHyperLinkControlForTicketID(module,
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
                        string expression = string.Format("{0}= '{1}' AND {2}='{3}'", DatabaseObjects.Columns.TicketId, workItem, DatabaseObjects.Columns.SubWorkItem, Convert.ToString(drWorkItem[DatabaseObjects.Columns.SubWorkItem]));
                        DataRow[] row = data.Select(expression);

                        if (row != null && row.Count() > 0)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstEndDate)))
                            {
                                row[0]["ExtendedDate"] = Convert.ToString(row[0]["ExtendedDate"]) + Constants.Separator + lstAllocations[0].EstStartDate + Constants.Separator1 + lstAllocations[0].EstEndDate;

                                if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationStartDate])) && !string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstStartDate)) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationStartDate]) > Convert.ToDateTime(lstAllocations[0].EstStartDate)))
                                    row[0][DatabaseObjects.Columns.AllocationStartDate] = lstAllocations[0].EstStartDate;

                                if (!string.IsNullOrEmpty(Convert.ToString(row[0][DatabaseObjects.Columns.AllocationEndDate])) && !string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstEndDate)) && (Convert.ToDateTime(row[0][DatabaseObjects.Columns.AllocationEndDate]) < Convert.ToDateTime(lstAllocations[0].EstEndDate)))
                                    row[0][DatabaseObjects.Columns.AllocationEndDate] = lstAllocations[0].EstEndDate;
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
                            title = UGITUtility.TruncateWithEllipsis(workItem, 50);
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
                            newRow[DatabaseObjects.Columns.WorkItemID] = lstAllocations[0].WorkItemID;  // UGITUtility.StringToLong(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(lstAllocations[0].WorkItemID);

                            if (!string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstEndDate)))
                            {
                                newRow[DatabaseObjects.Columns.AllocationStartDate] = lstAllocations[0].EstStartDate;
                                newRow[DatabaseObjects.Columns.AllocationEndDate] = lstAllocations[0].EstEndDate;

                                newRow["ExtendedDate"] = lstAllocations[0].EstStartDate + Constants.Separator1 + lstAllocations[0].EstEndDate;
                                newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(lstAllocations[0].PctEstimatedAllocation);
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
                        title = UGITUtility.TruncateWithEllipsis(workItem, 50);
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
                        newRow[DatabaseObjects.Columns.WorkItemID] = lstAllocations[0].WorkItemID;  // UGITUtility.StringToInt(Convert.ToString(drWorkItem[DatabaseObjects.Columns.Id])) + Constants.Separator + Convert.ToString(lstAllocations[0].WorkItemID);
                        newRow["ProjectNameLink"] = workItem;

                        if (!string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstStartDate)) && !string.IsNullOrEmpty(Convert.ToString(lstAllocations[0].EstEndDate)))
                        {
                            newRow[DatabaseObjects.Columns.AllocationStartDate] = lstAllocations[0].EstStartDate;
                            newRow[DatabaseObjects.Columns.AllocationEndDate] = lstAllocations[0].EstEndDate;

                            newRow["ExtendedDate"] = lstAllocations[0].EstStartDate + Constants.Separator1 + lstAllocations[0].EstEndDate;
                            newRow[DatabaseObjects.Columns.PctEstimatedAllocation] = UGITUtility.StringToInt(lstAllocations[0].PctEstimatedAllocation);
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

            //}
            #endregion
            return data; // data.DefaultView.ToTable();
        }


        protected void chkIncludeClosed_CheckedChanged1(object sender, EventArgs e)
        {
            //if (chkIncludeClosed.Checked)
            //    UGITUtility.CreateCookie(Response, "IncludeClosedAT", "true");
            //else
            //    UGITUtility.CreateCookie(Response, "IncludeClosedAT", "");

            //allocationData = GetAllocationData();
            //gvPreview.DataBind();
        }

        protected void chkIncludeClosed_ValueChanged(object sender, EventArgs e)
        {
            //if (chkIncludeClosed.Checked)
            //    UGITUtility.CreateCookie(Response, "IncludeClosedAT", "true");
            //else
            //    UGITUtility.CreateCookie(Response, "IncludeClosedAT", "");

            //allocationData = GetAllocationData();
            //gvPreview.DataBind();
        }
    }

    class GridTitleHeaderTemplate : ITemplate
    {
        string _title = string.Empty;
        public GridTitleHeaderTemplate(string title)
        {
            if (string.IsNullOrEmpty(title))
                title = "Projects";
            _title = title;
        }
        public void InstantiateIn(Control container)
        {
            HtmlGenericControl div1 = new HtmlGenericControl("DIV");
            div1.InnerText = _title;
            container.Controls.Add(div1);
        }
    }
    public class MonthDownHeaderTemplate : ITemplate
    {
        GridViewDataTextColumn colID = null;
        public MonthDownHeaderTemplate(GridViewDataTextColumn coID)
        {
            this.colID = coID;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            HyperLink hnkButton = new HyperLink();
            hnkButton.Text = this.colID.Caption;
            hnkButton.CssClass = "hand-cursor";
            container.Controls.Add(hnkButton);

            string func = string.Format("ClickOnDrillDown_PG(this,'{0}','{1}')", colID.FieldName, colID.Caption);
            hnkButton.Attributes.Add("onclick", func);

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
            hnkBDButton.CssClass = "hand-cursor";
            //container.Controls.Add(hnkBDButton);
            string func = string.Format("ClickOnDrillUP_PG(this,'{0}')", colBDC.Caption);
            hnkBDButton.Attributes.Add("onclick", func);

            if (_mode == "Monthly")
            {
                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-right:7px;cursor:pointer;\" src=\"/content/images/back-arrowBlue.png\" onclick=\"ClickOnPrevious_PG()\" class=\"resource-img-gantt\"  />"));
                HContainer.Controls.Add(hnkBDButton);
                HContainer.Controls.Add(new LiteralControl("<image style=\"padding-left:7px;cursor:pointer;\" src=\"/content/images/next-arrowBlue.png\" onclick=\"ClickOnNext_PG()\" class=\"resource-img-gantt\"  />"));
                container.Controls.Add(HContainer);
            }
            else
            {
                HContainer.Controls.Add(hnkBDButton);
                container.Controls.Add(HContainer);
            }
        }

        #endregion
    }

}
