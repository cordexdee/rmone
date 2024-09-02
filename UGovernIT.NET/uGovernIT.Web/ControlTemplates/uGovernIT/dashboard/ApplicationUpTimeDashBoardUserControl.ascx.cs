using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.Web;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.XtraPivotGrid.Data;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public class OutageHoursRpt
    {
        public int Sequence { get; set; }
        public string TicketRequestType { get; set; }
        public DateTime Months { get; set; }
        public double OutageHours { get; set; }
        public string currentModuleName;
    }
    class FieldValueTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            PivotGridFieldValueTemplateContainer c = (PivotGridFieldValueTemplateContainer)container;
            PivotGridFieldValueHtmlCell cell = c.CreateFieldValue();
            PivotFieldValueItem valueItem = c.ValueItem;
            PivotFieldValueEventArgs helperArgs = new PivotFieldValueEventArgs(valueItem);
            PivotGridField[] fields = helperArgs.GetHigherLevelFields();
            List<object> fieldValues = new List<object>();
            foreach (PivotGridField field in fields)
            {
                object currentValue = helperArgs.GetHigherLevelFieldValue(field);
                if (currentValue != null)
                    fieldValues.Add(currentValue);
            }

            cell.Controls.AddAt(cell.Controls.IndexOf(cell.TextControl), new MyAppUpTimeLink(c.Text, fieldValues));
            cell.Controls.Remove(cell.TextControl);
            c.Controls.Add(cell);
        }
    }
    class CellTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            PivotGridCellTemplateContainer c = container as PivotGridCellTemplateContainer;
            List<object> fieldValues = new List<object>();
            if (c.ColumnField != null)
                fieldValues.Add(c.GetFieldValue(c.ColumnField));
            if (c.RowField != null)
                fieldValues.Add(c.GetFieldValue(c.RowField));
            int upTimePercentage = UGITUtility.StringToInt(c.Text.Replace("%", ""));
            if (upTimePercentage < 100 && upTimePercentage != 0)
                c.Controls.Add(new MyAppUpTimeLink(c.Text, fieldValues));
            else
            {
                Label lblValue = new Label();
                lblValue.Text = c.Text;
                c.Controls.Add(lblValue);
            }

        }
    }
    public class MyAppUpTimeLink : HyperLink
    {
        public MyAppUpTimeLink(string text, List<object> values) : base()
        {
            Text = text;
            Style.Add("text-decoration", "underline");
            string valuesString = string.Empty;
            foreach (object value in values)
            {

                if (string.IsNullOrEmpty(valuesString))
                    valuesString = value.ToString();
                else
                    valuesString += "_" + value.ToString();
            }


            ID = "anchor_" + valuesString.Trim().Replace(" ", "");
            CssClass = valuesString.Trim().Replace(" ", "");
            Attributes.Add("href", string.Format("javascript:showOutageDetails('{0}','{1}')", valuesString, CssClass));
            ToolTip = valuesString;
        }
    }

    public partial class ApplicationUpTimeDashBoardUserControl : UserControl
    {
        protected string currentYear = DateTime.Now.Year.ToString();
        private DateTime yearStartDate = DateTime.Now;
        private DateTime yearEndDate = DateTime.Now;
        public string delegateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx");
        public Dictionary<string, bool> dictRequestTypes = null;
        ApplicationContext _Context;
        ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            _Context = HttpContext.Current.GetManagerContext();
            lblSelectedYear.Text = DateTime.Now.Date.Year.ToString();
            //if (Request["Module"] != null)
            //    currentModuleName = Convert.ToString(Request["Module"]);
            CreateGrid();
            GetData();
        }

        private DataTable BindData(string moduleName)
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.TicketRequestType);
            data.Columns.Add("Month", typeof(DateTime));
            data.Columns.Add(DatabaseObjects.Columns.OutageHours, typeof(int));
            data.Columns.Add("TotalMonthUpTime", typeof(int));

            UGITModule module = moduleManager.GetByName(moduleName);  // uGITCache.ModuleConfigCache.GetCachedModule(spWeb, moduleName);
            
            DataTable openTickets = ticketManager.GetAllTickets(module);    // uGITCache.ModuleDataCache.GetAllTickets(module.ID, spWeb);
            DataRow[] filteredTickets = new DataRow[0];
            List<string> lstRequestTypes = new List<string>();
            List<OutageHoursRpt> resultedData = new List<OutageHoursRpt>();

            #region If INC have tickets
            if (openTickets != null)
            {
                string incidentOccurrenceDateColumn = DatabaseObjects.Columns.Created;
                if (openTickets.Columns.Contains(DatabaseObjects.Columns.OccurrenceDate))
                    incidentOccurrenceDateColumn = DatabaseObjects.Columns.OccurrenceDate;

                List<string> queryExps = new List<string>();
                if (yearStartDate != DateTime.MinValue)
                    queryExps.Add(string.Format("{0}>#{1}#", incidentOccurrenceDateColumn, yearStartDate.ToString("MM/dd/yyyy")));
                if (yearEndDate != DateTime.MinValue)
                    queryExps.Add(string.Format("{0}< #{1}#", incidentOccurrenceDateColumn, yearEndDate.AddDays(1).ToString("MM/dd/yyyy")));

                filteredTickets = openTickets.Select(string.Join(" AND ", queryExps));


                var byGroups = filteredTickets.GroupBy(x => new { Month = UGITUtility.StringToDateTime(x[DatabaseObjects.Columns.OccurrenceDate]), TicketRequestType = x[DatabaseObjects.Columns.TicketRequestTypeLookup] })
                    .Select(group => new OutageHoursRpt
                    {
                        Months = group.Key.Month,
                        TicketRequestType = Convert.ToString(group.Key.TicketRequestType),
                        OutageHours = group.Sum(a => UGITUtility.StringToDouble(a[DatabaseObjects.Columns.OutageHours]))
                    }).ToList();

                resultedData.AddRange(byGroups);
                lstRequestTypes = resultedData.Select(x => x.TicketRequestType).Distinct().ToList();
            }
            #endregion

            List<string> lstAllModuleRequestType = GetModuleRequestType();
            if (lstRequestTypes == null || lstRequestTypes.Count == 0)
                lstRequestTypes = lstAllModuleRequestType;
            else
            {
                foreach (string reqType in lstAllModuleRequestType)
                {
                    if (!lstRequestTypes.Any(x => x.Equals(reqType)))
                        lstRequestTypes.Add(reqType);
                }
            }

            foreach (string item in lstRequestTypes)
            {
                int workingHoursInDay = GetWorkingHours(item);
                for (int i = 0; i < 12; i++)
                {
                    DataRow rRow = data.NewRow();
                    rRow[DatabaseObjects.Columns.TicketRequestType] = item;
                    if (string.IsNullOrWhiteSpace(item))
                        rRow[DatabaseObjects.Columns.TicketRequestType] = "None";
                    string monthName = new DateTime(yearStartDate.Year, yearStartDate.AddMonths(i).Month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
                    rRow["Month"] = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture);// yearStartDate.AddMonths(i);
                    int totalNoOfDaysInMnth = DateTime.DaysInMonth(yearStartDate.Year, yearStartDate.AddMonths(i).Month);
                    int totalWorkingDaysInMnth = totalNoOfDaysInMnth;

                    // Check if working hours in a day are set according to Use24x7Calendar, if not then reset totalWorkingDaysInMnth according to working days
                    if (workingHoursInDay != 24)
                        totalWorkingDaysInMnth = uHelper.GetTotalWorkingDaysBetween(_Context, yearStartDate.AddMonths(i), yearStartDate.AddMonths(i).AddDays(totalNoOfDaysInMnth));

                    double outageHoursPerMonth = resultedData.Where(x => x.TicketRequestType == item && x.Months.Month == yearStartDate.AddMonths(i).Month).Sum(x => x.OutageHours);
                    double totalUpTimePerMonth = totalWorkingDaysInMnth * workingHoursInDay;
                    double upTimePercentageInMonth = ((totalUpTimePerMonth - outageHoursPerMonth) / totalUpTimePerMonth) * 100;
                    rRow[DatabaseObjects.Columns.OutageHours] = upTimePercentageInMonth;
                    data.Rows.Add(rRow);
                }
            }

            return data;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // GetData();
        }

        private void GetData()
        {
            if (!string.IsNullOrEmpty(hdnCurrentYear.Value))
                currentYear = hdnCurrentYear.Value;
            else
                currentYear = Convert.ToString(DateTime.Now.Year);
            lblSelectedYear.Text = currentYear;
            yearStartDate = new DateTime(UGITUtility.StringToInt(currentYear), 1, 1);
            yearEndDate = new DateTime(UGITUtility.StringToInt(currentYear), 12, 31);
            grid.DataSource = BindData("INC");
        }

        private void CreateGrid()
        {
            PivotGridField field = null;
            field = new PivotGridField(DatabaseObjects.Columns.TicketRequestType, DevExpress.XtraPivotGrid.PivotArea.RowArea);
            field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            field.Caption = "Application/Area";
            field.MinWidth = 100;
            field.Width = 100;
            field.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.True;
            field.Options.AllowFilterBySummary = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowSortBySummary = DevExpress.Utils.DefaultBoolean.False;
            field.Options.ShowGrandTotal = false;

            field.Options.ShowTotals = true;
            field.Options.ShowInPrefilter = false;
            field.ValueStyle.Font.Bold = true;
            grid.Fields.Add(field);

            field = new PivotGridField("Month", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
            field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            field.Caption = "";

            field.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            field.SortMode = DevExpress.XtraPivotGrid.PivotSortMode.None;
            field.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowFilterBySummary = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowSortBySummary = DevExpress.Utils.DefaultBoolean.False;
            field.Options.ShowGrandTotal = false;
            field.Options.ShowTotals = true;
            field.Options.ShowInPrefilter = false;
            field.GroupInterval = DevExpress.XtraPivotGrid.PivotGroupInterval.DateMonth;
            field.CellFormat.FormatString = "MMM";
            field.HeaderStyle.Font.Bold = true;
            field.ValueStyle.Font.Bold = true;
            field.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            field.ValueFormat.FormatString = "MMM";
            field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
            grid.Fields.Add(field);

            field = new PivotGridField(DatabaseObjects.Columns.OutageHours, DevExpress.XtraPivotGrid.PivotArea.DataArea);
            field.Caption = "Uptime";
            field.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowFilterBySummary = DevExpress.Utils.DefaultBoolean.False;
            field.Options.AllowSortBySummary = DevExpress.Utils.DefaultBoolean.False;
            field.Options.ShowGrandTotal = false;
            field.Options.ShowTotals = true;
            field.Options.ShowInPrefilter = false;
            field.ValueFormat.FormatString = "p2";
            field.CellFormat.FormatString = "p2";

            grid.OptionsView.ShowColumnHeaders = false;

            field.ValueStyle.HorizontalAlign = HorizontalAlign.Center;
            field.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            grid.Fields.Add(field);
            field.Options.AllowFilterBySummary = DevExpress.Utils.DefaultBoolean.False;
            field.Options.HideEmptyVariationItems = true;
            field.Options.ShowInPrefilter = false;

            grid.CellTemplate = new CellTemplate();
            grid.CustomCellValue += Grid_CustomCellValue;
            grid.OptionsData.FilterByVisibleFieldsOnly = true;
            grid.OptionsView.ShowColumnHeaders = false;
            grid.OptionsView.ShowFilterHeaders = false;
            grid.OptionsView.ShowDataHeaders = false;
            grid.OptionsView.ShowContextMenus = true;
            grid.OptionsPager.RowsPerPage = 15;
        }
        private void Grid_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            ASPxPivotGrid pvotGrid = sender as ASPxPivotGrid;
            if (e.DataField != null && e.DataField.Area == DevExpress.XtraPivotGrid.PivotArea.DataArea && e.DataField.FieldName == DatabaseObjects.Columns.OutageHours)
            {
                object cellVal = 0F;
                e.Value = Convert.ToString(e.Value) + "%";
            }
        }
        protected void aspxCallBackYearFilter_Callback1(object sender, CallbackEventArgsBase e)
        {
            GetData();
        }

        private List<string> GetModuleRequestType()
        {
            List<string> requestTypes = new List<string>();
            dictRequestTypes = new Dictionary<string, bool>();
            UGITModule module = moduleManager.LoadByName(ModuleNames.INC); // uGITCache.ModuleConfigCache.GetCachedModule(spWeb, "INC");
            if (module != null && module.List_RequestTypes != null && module.List_RequestTypes.Count > 0)
            {
                foreach (ModuleRequestType requestType in module.List_RequestTypes.Distinct())
                {
                    if (!requestType.Deleted)
                    {
                        requestTypes.Add(requestType.RequestType);
                        if (!dictRequestTypes.ContainsKey(requestType.RequestType))
                        {
                            dictRequestTypes.Add(requestType.RequestType, requestType.Use24x7Calendar);
                        }
                        
                        
                    } 
                }
            }

            return requestTypes;
        }

        #region Method to Get Working hours in a Day based on the Use24x7Calenadar field
        /// <summary>
        /// Method to Get Working hours in a Day based on the Use24x7Calenadar field
        /// </summary>
        /// <param name="requestTypeName"></param>
        /// <param name="spWeb"></param>
        /// <returns></returns>
        private int GetWorkingHours(string requestTypeName)
        {
            int workingHoursInADay = 0;

            if (dictRequestTypes == null || dictRequestTypes.Count == 0 || string.IsNullOrEmpty(requestTypeName))
            {
                workingHoursInADay = uHelper.GetWorkingHoursInADay(_Context, false);
            }
            else
            {
                bool use24x7Calendar = dictRequestTypes.Where(x => x.Key == requestTypeName).Select(y => y.Value).FirstOrDefault();

                if (use24x7Calendar)
                    workingHoursInADay = 24;
                else
                    workingHoursInADay = uHelper.GetWorkingHoursInADay(_Context, false);
            }

            return workingHoursInADay;
        }
        #endregion Method to Get Working hours in a Day based on the Use24x7Calenadar field
    }
}
