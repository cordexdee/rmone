using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using System.Linq;
using System.Text;
using DevExpress.XtraCharts;
using System.Globalization;
using ReportEntity = uGovernIT.Manager.Entities;
using DevExpress.XtraReports.UI;
using System.Drawing;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Helpers;
using uGovernIT.DAL;

namespace uGovernIT.DxReport
{
    public partial class GanttView_Viewer : UserControl
    {
        public string Module { get; set; }
        public string GanttType { get; set; }
        public string OpenProjectOnly { get; set; }
        public string UserSelectedColumns { get; set; }
        public string ColumnsSortOrder { get; set; }
        public string GroupBy { get; set; }
        private DataRow moduleRow;
        protected DataTable resultedTable;
        protected ZoomLevel zoomLevel;
        protected bool IsCustomCallBack = false;
        private DataRow[] moduleColumns;
        Ticket moduleRequest;
        private DataTable projectMonitorsStateTable;
        private DataTable moduleMonitorsTable;
        ModuleViewManager moduleViewManager;
        ModuleColumnManager moduleColumnManager;
        TicketManager ticketManager;
        ProjectMonitorStateManager projectMonitorStateManager;
        ModuleMonitorManager moduleMonitorManager;
        ModuleMonitorOptionManager moduleMonitorOptionManager;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        public string ReportFilterURl;
        private ConfigurationVariableManager _configurationVariableManager;

        public GanttView_Viewer()
        {
            moduleColumns = new DataRow[0];
        }

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            _configurationVariableManager = new ConfigurationVariableManager(_context);
            List<string> paramRequired = new List<string>();
            paramRequired.Add("GanttType");
            paramRequired.Add("GroupBy");
            paramRequired.Add("OpenProjectOnly");
            Module = Request["Module"];

            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = delegateControl + "?reportName=GanttView&Filter=Filter&Module=" + Module;
                Response.Redirect(ReportFilterURl);
            }
            GanttType = Request["GanttType"];
            GroupBy = Request["GroupBy"];
            OpenProjectOnly = Convert.ToString(Request["OpenProjectOnly"]);
            UserSelectedColumns = Convert.ToString(Request["UserSelectedColumns"]);
            ColumnsSortOrder = Convert.ToString(Request["ColumnsSortOrder"]);            
            ticketManager = new TicketManager(_context);
            moduleViewManager = new ModuleViewManager(_context);
            moduleColumnManager = new ModuleColumnManager(_context);
            projectMonitorStateManager = new ProjectMonitorStateManager(_context);
            moduleMonitorManager = new ModuleMonitorManager(_context);
            moduleMonitorOptionManager = new ModuleMonitorOptionManager(_context);
            grid.SettingsPager.AlwaysShowPager = false;
            grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            grid.SettingsPager.ShowDisabledButtons = true;
            grid.SettingsPager.ShowNumericButtons = true;
            grid.SettingsPager.ShowSeparators = true;
            grid.SettingsPager.ShowDefaultImages = true;
            grid.AutoGenerateColumns = false;
            grid.SettingsText.EmptyHeaders = "  ";
            grid.SettingsPager.PageSizeItemSettings.Visible = true;
            grid.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;
            grid.SettingsBehavior.AllowSelectByRowClick = true;
            grid.SettingsBehavior.EnableRowHotTrack = true;
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ugitlight1lightest";
            grid.Settings.GridLines = System.Web.UI.WebControls.GridLines.Horizontal;
            grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            grid.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            grid.SettingsBehavior.AllowDragDrop = false;
            grid.Settings.ShowHeaderFilterBlankItems = false;

            grid.SettingsPager.PageSize = 20;

            if (IsPostBack)
            {
                zoomLevel = (ZoomLevel)Enum.Parse(typeof(ZoomLevel), UGITUtility.GetCookieValue(Request, "ZoomLevel"));
                if (Request.Form["__CALLBACKPARAM"] != null && Request.Form["__CALLBACKPARAM"].Contains("CUSTOMCALLBACK"))
                {
                    IsCustomCallBack = true;
                    ZoomLevel zoom = zoomLevel;
                    if (Request.Form["__CALLBACKPARAM"].Contains("+"))
                    {
                        if (zoom == ZoomLevel.Yearly)
                        {
                            zoomLevel = ZoomLevel.HalfYearly;
                        }
                        if (zoom == ZoomLevel.HalfYearly)
                        {
                            zoomLevel = ZoomLevel.Quarterly;
                        }
                        else if (zoom == ZoomLevel.Quarterly)
                        {
                            zoomLevel = ZoomLevel.Monthly;
                        }
                        else if (zoom == ZoomLevel.Monthly)
                        {
                            zoomLevel = ZoomLevel.Weekly;
                        }
                    }
                    else if (Request.Form["__CALLBACKPARAM"].Contains("-"))
                    {
                        if (zoom == ZoomLevel.Weekly)
                        {
                            zoomLevel = ZoomLevel.Monthly;
                        }
                        else
                            if (zoom == ZoomLevel.Monthly)
                        {
                            zoomLevel = ZoomLevel.Quarterly;
                        }
                        else if (zoom == ZoomLevel.Quarterly)
                        {
                            zoomLevel = ZoomLevel.HalfYearly;
                        }
                        else if (zoom == ZoomLevel.HalfYearly)
                        {
                            zoomLevel = ZoomLevel.Yearly;
                        }

                    }

                    UGITUtility.CreateCookie(Response, "ZoomLevel", zoomLevel.ToString());

                }


            }
            else
            {
                zoomLevel = ZoomLevel.Quarterly;
                UGITUtility.CreateCookie(Response, "ZoomLevel", zoomLevel.ToString());
            }


            // Just in case module id was passed in instead of name try to parse to int
            long moduleId = 0;
            long.TryParse(this.Module, out moduleId);

            // Get module details
            /*
            moduleRow = moduleViewManager.GetDataTable().AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == this.Module);
            if (moduleRow != null)
                Module = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
            */

            if (moduleId > 0)
                Module = moduleViewManager.GetByID(moduleId).ModuleName;
            else
                Module = moduleViewManager.GetByName(this.Module).ModuleName;

            GetFilteredData();

            if (resultedTable == null || resultedTable.Rows.Count == 0)
            {
                grid.SettingsPager.Visible = false;
                grid.ExpandAll();
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsCustomCallBack)
            {
                GenerateColumns(zoomLevel);
                
            }
            grid.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;

            foreach (DataRow row in resultedTable.Rows)
            {
                foreach (DataColumn column in resultedTable.Columns)
                {
                    string data = Convert.ToString(row[column]);

                    if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                    {
                        row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? "Green" : (data.ToLower().Contains("class='yellowled monitoricon'") ? "Yellow" : (data.ToLower().Contains("class='redled monitoricon'") ? "Red" : "Green"));
                    }
                }
            }

            FillGantViewdata();
            XtraReport report = reportHelper.GenerateReport(grid, resultedTable, $"{Module} Gantt View", 8F, "xls");
            reportHelper.WriteXlsToResponse(Response, $"{Module}GanttView.xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {

            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            foreach (DataRow row in resultedTable.Rows)
            {
                foreach (DataColumn column in resultedTable.Columns)
                {
                    string data = Convert.ToString(row[column]);

                    if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                    {
                        row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : GetImage("GreenLED")));
                    }
                }
            }

            FillGantViewdata();
            XtraReport report = reportHelper.GenerateReport(grid, resultedTable, $"{Module} Gantt View", 6.75F);
            reportHelper.WritePdfToResponse(Response, $"{Module}GanttView.pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

            //gridExporter.WritePdfToResponse(title);
        }

        void reportHelper_CustomizeColumn(object source, ControlCustomizationEventArgs e)
        {

        }

        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(((XRShape)sender).Report.GetCurrentColumnValue("Discontinued")) == true)
                ((XRShape)sender).FillColor = Color.Yellow;
            else
                ((XRShape)sender).FillColor = Color.White;
        }

        void reportHelper_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e)
        {
            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Title"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Title").ColumnWidth *= 2;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Rank"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Rank").ColumnWidth = 50;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Priority"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Priority").ColumnWidth = 50;
        }
        #endregion

        #region GridView

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = grid.GetDataRow(e.VisibleIndex);

            string moduleName = string.Empty;
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
            {
                moduleName = Convert.ToString(currentRow[DatabaseObjects.Columns.ModuleNameLookup]);
            }
            if (string.IsNullOrEmpty(moduleName))
            {
                string ticketId = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                moduleName = ticketId.Substring(0, ticketId.IndexOf("-"));
            }

            int ganttStartIndex = grid.Columns.Count - grid.AllColumns.Where(m => m as GridViewBandColumn != null).ToList().Count;

            DateTime dtStart;
            DateTime dtEnd;

            DateTime.TryParse(Convert.ToString(currentRow[DatabaseObjects.Columns.TicketActualStartDate]), out dtStart);
            DateTime.TryParse(Convert.ToString(currentRow[DatabaseObjects.Columns.TicketActualCompletionDate]), out dtEnd);

            DateTime minDate = new DateTime(1800, 1, 1);
            if (dtStart == DateTime.MinValue || dtEnd == DateTime.MinValue || dtStart == minDate || dtEnd == minDate)
            {
                return;
            }
            DateTime? pctCompleteDate = null;
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketPctComplete))
            {
                int days = (int)((UGITUtility.StringToDouble(currentRow[DatabaseObjects.Columns.TicketPctComplete])/100) * (dtEnd - dtStart).Days);
                pctCompleteDate = dtStart.AddDays(days);
            }
            DataColumn ModuleMonitorOptionLEDClassLookup = new DataColumn(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup, typeof(string));
            DataColumn ModuleMonitorName = new DataColumn(DatabaseObjects.Columns.ModuleMonitorName, typeof(string));
            DataColumn ModuleMonitorOptionNameLookup = new DataColumn(DatabaseObjects.Columns.ModuleMonitorOptionNameLookup, typeof(string));

            if (moduleName == ModuleNames.PMM)
            {
                if (projectMonitorsStateTable == null)
                    return;

                if (!projectMonitorsStateTable.Columns.Contains(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup))
                {
                    projectMonitorsStateTable.Columns.Add(ModuleMonitorOptionLEDClassLookup);
                }
                if (!projectMonitorsStateTable.Columns.Contains(DatabaseObjects.Columns.ModuleMonitorName))
                {
                    projectMonitorsStateTable.Columns.Add(ModuleMonitorName);
                }
                if (!projectMonitorsStateTable.Columns.Contains(DatabaseObjects.Columns.ModuleMonitorOptionNameLookup))
                {
                    projectMonitorsStateTable.Columns.Add(ModuleMonitorOptionNameLookup);
                } 
            }
            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    GridViewTableDataCell editCell = (GridViewTableDataCell)cell;
                    string fieldName = ((GridViewDataColumn)editCell.Column).FieldName;
                    //ModuleMonitor moduleMonitor;
                    int cellIndex = ((GridViewDataColumn)editCell.Column).Index; 
                    if (Module == "PMM")
                    {
                        if (((GridViewDataColumn)editCell.Column).Name.StartsWith("projecthealth"))
                        {
                            string ticketID = Convert.ToString(e.KeyValue);
                            //// code transfered to custom grid modification
                            //moduleMonitor = new ModuleMonitor();
                            //moduleMonitor = moduleMonitorManager.Load(x => x.MonitorName.Equals(fieldName)).FirstOrDefault();
                            //if (projectMonitorsStateTable != null && projectMonitorsStateTable.Rows.Count > 0)
                            //{
                            //    DataRow[] projectMonitorState = projectMonitorsStateTable.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.ModuleMonitorNameLookup, moduleMonitor.ID));
                            //    if (projectMonitorState.Length > 0)
                            //    {
                            //        ModuleMonitorOption moduleMonitorOption = moduleMonitorOptionManager.LoadByID(Convert.ToInt64((projectMonitorState[0][DatabaseObjects.Columns.ModuleMonitorOptionIdLookup])));
                            //        projectMonitorState[0][DatabaseObjects.Columns.ModuleMonitorName] = moduleMonitor.MonitorName;
                            //        projectMonitorState[0][DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = moduleMonitorOption.ModuleMonitorOptionLEDClass;
                            //        projectMonitorState[0][DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = moduleMonitorOption.ModuleMonitorOptionName;
                            //        //e.Row.Cells[cellIndex].Text = UGITUtility.GetMonitorsGraphic(projectMonitorState[0]);
                            //    }
                            //}
                        }
                    }
                    if (fieldName == DatabaseObjects.Columns.TicketPriorityLookup)
                    {
                        if (Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketPriorityLookup)) == "High")
                        {
                            e.Row.Cells[cellIndex].Style.Add(HtmlTextWriterStyle.Color, "#900000");
                            e.Row.Cells[cellIndex].Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                    }

                    if (fieldName == DatabaseObjects.Columns.TicketStatus || fieldName == DatabaseObjects.Columns.ModuleStepLookup)
                    {
                        if (!string.IsNullOrEmpty(moduleName))
                        {
                            bool onHold = false;
                            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                                bool.TryParse(Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketOnHold)), out onHold);

                            LifeCycleStage currentStage = null;
                            LifeCycle defaultLifeCycle = moduleRequest.GetTicketLifeCycle(currentRow);
                            double totalWeight = defaultLifeCycle.Stages.Sum(x => x.StageWeight);
                            currentStage = moduleRequest.GetTicketCurrentStage(currentRow);
                            if (currentStage != null)
                            {
                                double tillStageWeight = defaultLifeCycle.Stages.Where(x => x.StageStep < currentStage.StageStep).Sum(x => x.StageWeight);
                                int pctComplete = (int)(tillStageWeight / totalWeight * 100);
                                string status = string.Empty;
                                if (fieldName == DatabaseObjects.Columns.TicketStatus && currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketStatus))
                                    status = Convert.ToString(currentRow[DatabaseObjects.Columns.TicketStatus]); // TicketStatus field passed in
                                else
                                    status = currentStage.Name; // ModuleStepLookup field passed in

                                // Show short stage title if configured AND if we don't have an artificial status like "Returned"
                                if (!string.IsNullOrEmpty(currentStage.ShortStageTitle) && currentStage.Name.ToLower() == status.ToLower())
                                    status = currentStage.ShortStageTitle;
                                e.Row.Cells[cellIndex].Text = status; // UGITUtility.GetProgressBar(defaultLifeCycle, currentStage, currentRow, fieldName, onHold, false);
                            }
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketAge)
                    {
                        bool ageColorByTargetCompletion = _configurationVariableManager.GetValueAsBool(ConfigConstants.AgeColorByTargetCompletion);
                        int ticketAge = UGITUtility.StringToInt(e.GetValue(DatabaseObjects.Columns.TicketAge));
                        e.Row.Cells[cellIndex].Text = UGITUtility.GetAgeBar(currentRow, ageColorByTargetCompletion, ticketAge);
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketPctComplete)
                    {
                        double pctComplete;
                        if (double.TryParse(Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketPctComplete)), out pctComplete))
                        {
                            // Display 0-1 value as % with 1 or two decimals (if needed)
                            //pctComplete *= 100;
                            if (pctComplete > 99.9 && pctComplete < 100)
                                e.Row.Cells[cellIndex].Text = "99.9%"; // Don't show 100% unless all the way done!
                            else
                                e.Row.Cells[cellIndex].Text = string.Format("{0:0.#}%", pctComplete);
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketDueIn)
                    {
                        if (Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketDesiredCompletionDate)) != string.Empty)
                        {
                            e.Row.Cells[cellIndex].Text = UGITUtility.GetDueIn((DateTime)(e.GetValue(DatabaseObjects.Columns.TicketDesiredCompletionDate)));
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketStageActionUsers && Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketStageActionUsers)) != string.Empty)
                    {
                        if (_context.ConfigManager.GetValueAsBool("OnlyShowFirstActionUser"))
                            e.Row.Cells[cellIndex].Text = UGITUtility.SplitString(e.GetValue(DatabaseObjects.Columns.TicketStageActionUsers), ";")[0];
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketComment ||
                             fieldName == DatabaseObjects.Columns.TicketResolutionComments ||
                             fieldName == DatabaseObjects.Columns.ProjectSummaryNote ||
                             fieldName == DatabaseObjects.Columns.History)
                    {
                        string rawData = Convert.ToString(e.GetValue(fieldName));

                        // Gets ALL entries
                        // gvRow.Cells[i].Text = uHelper.GetFormattedHistoryString(rawData, true);

                        // Just get the latest entry
                        string[] versionsDelim = { Constants.SeparatorForVersions };
                        string[] versions = rawData.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);

                        if (versions != null && versions.Length != 0)
                        {
                            string latestEntry = versions[versions.Length - 1];

                            // Assume <version1>$;#$<version2>$;#$<version3>
                            string[] versionDelim = { Constants.Separator };
                            string[] versionData = latestEntry.Split(versionDelim, StringSplitOptions.None);

                            if (versionData.GetLength(0) == 3)
                            {
                                // Assume <userID>;#<timestamp>;#<text>
                                string createdBy = versionData[0];
                                DateTime createdDate;
                                string created = string.Empty;
                                if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (DateTime.TryParse(versionData[1].Substring(Constants.UTCPrefix.Length), out createdDate))
                                        created = createdDate.ToLocalTime().ToString("MMM-d-yyyy");
                                }
                                else
                                {
                                    if (DateTime.TryParse(versionData[1], out createdDate))
                                        created = createdDate.ToString("MMM-d-yyyy");
                                }
                                string entry = versionData[2];
                                if (fieldName == DatabaseObjects.Columns.TicketResolutionComments)
                                    e.Row.Cells[cellIndex].Text = entry;
                                else
                                    e.Row.Cells[cellIndex].Text = string.Format("<b>{0}</b>: {1}", created, entry);
                            }
                            else
                            {
                                e.Row.Cells[cellIndex].Text = latestEntry;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(((GridViewDataColumn)editCell.Column).Name) && !((GridViewDataColumn)editCell.Column).Name.StartsWith("projecthealth"))
                    {

                        string header = ((GridViewDataColumn)editCell.Column).ParentBand.Name;
                        int yr = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1] : header);
                        int month = 1;
                        int day = 1;
                        if (zoomLevel == ZoomLevel.Weekly)
                        {
                            month = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0] : "1");
                            day = UGITUtility.StringToInt(((GridViewDataColumn)editCell.Column).Name);

                            DateTime dtGrid = new DateTime(yr, month, day);
                            if ((dtGrid >= dtStart && dtGrid <= dtEnd) || (((dtStart - dtGrid).Days < 7 && (dtStart - dtGrid).Days > 0) && dtGrid <= dtEnd))
                            {
                                int bandcolumncount = 0;

                                for (int i = ganttStartIndex + 1; ((GridViewDataColumn)editCell.Column).ParentBand.Index >= i; i++)
                                {
                                    bandcolumncount += ((GridViewBandColumn)grid.Columns[i - 1]).Columns.Count;
                                }

                                int index = cellIndex + ganttStartIndex + bandcolumncount;
                                string html = string.Empty;
                                if (pctCompleteDate != null && (dtStart - dtGrid).Days < 7 && dtGrid <= pctCompleteDate)
                                {
                                    int width = ((dtStart - dtGrid).Days * 100) / 7;
                                    if (width < 100 && width > 0)
                                    {
                                        if (pctCompleteDate != dtStart)
                                        {
                                            html = " <div style='float:right;width:" + width + "%;font-weight: lighter;background-color: darkgray;height: 12px;'> <div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: 100%;margin-top: 4px;'></div></div>";
                                        }
                                        else
                                        {
                                            html = " <div style='float:right;width:" + width + "%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                        }
                                    }
                                    else
                                    {
                                        html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'> <div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: 100%;margin-top: 4px;'></div></div>";
                                    }
                                }
                                else if (dtGrid >= dtStart && dtGrid <= dtEnd)
                                {

                                    if (pctCompleteDate != null && pctCompleteDate != dtStart)
                                    {
                                        int width = ((pctCompleteDate.Value - dtGrid).Days * 100) / 7;
                                        html = " <div style='float:right;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'> <div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: " + width + "%;margin-top: 4px;'></div></div>";
                                    }
                                    else
                                    {
                                        html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                    }
                                }
                                if (e.Row.Cells.Count > index)
                                    e.Row.Cells[index].Text = html;

                            }
                        }
                        else
                        {

                            string html = string.Empty;
                            month = UGITUtility.StringToInt(((GridViewDataColumn)editCell.Column).Name);
                            int period = zoomLevel == ZoomLevel.Yearly ? 12 : (zoomLevel == ZoomLevel.HalfYearly ? 6 : (zoomLevel == ZoomLevel.Quarterly ? 3 : 1));

                            DateTime dtGrid = new DateTime(yr, month, day);
                            DateTime dtPreGrid = dtGrid.AddMonths(-period);
                            DateTime dtPostGrid = dtGrid.AddMonths(+period);

                            if (dtGrid >= dtStart && dtGrid <= dtEnd && dtPostGrid <= dtEnd)
                            {
                                int index = cellIndex + ganttStartIndex + (((GridViewDataColumn)editCell.Column).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)grid.Columns[ganttStartIndex]).Columns.Count;
                                if (dtGrid >= dtStart && dtGrid <= pctCompleteDate && pctCompleteDate != dtStart)
                                {
                                    html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'><div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: 100%;margin-top: 4px;'></div></div>";
                                }
                                else
                                {
                                    html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                }
                                if (e.Row.Cells.Count > index)
                                    e.Row.Cells[index].Text = html;


                            }
                            else if (dtStart >= dtPreGrid && dtStart <= dtPostGrid && dtPostGrid <= dtEnd)
                            {
                                int index = cellIndex + ganttStartIndex + (((GridViewDataColumn)editCell.Column).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)grid.Columns[ganttStartIndex]).Columns.Count;
                                int width = (dtPostGrid - dtStart).Days * 100 / (period * 2 * 30);
                                if (dtStart >= dtPreGrid && dtPreGrid <= pctCompleteDate && pctCompleteDate != dtStart)
                                {
                                    html = " <div style='float:right;width:" + width + "%;font-weight: lighter;background-color: darkgray;height: 12px;'><div style='float: right;background-color: #008000;height: 4px;width: 100%;margin-top: 4px;'></div></div>";
                                }
                                else
                                {
                                    html = " <div style='float:right;width:" + width + "%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                }
                                if (e.Row.Cells.Count > index)
                                    e.Row.Cells[index].Text = html;
                            }
                            else if (dtEnd >= dtGrid && dtEnd <= dtPostGrid)
                            {
                                int index = cellIndex + ganttStartIndex + (((GridViewDataColumn)editCell.Column).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)grid.Columns[ganttStartIndex]).Columns.Count;
                                int leftMargin = dtStart > dtGrid ? ((dtStart - dtGrid).Days * 100) / (period * 30) : 0;
                                int width = (dtEnd - dtPreGrid).Days * 100 / (period * 2 * 30) - leftMargin;

                                if (dtGrid <= pctCompleteDate && pctCompleteDate != dtStart)
                                {
                                    int widthPctComplete =  (pctCompleteDate.Value - dtPreGrid).Days * 100 /(period * 2 * 30);
                                    html = " <div style='margin-left:" + leftMargin + "%;float:left;width:" + width + "%;font-weight: lighter;background-color: darkgray;height: 12px;'><div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: " + widthPctComplete + "%;margin-top: 4px;'></div></div>";
                                }
                                else
                                {
                                    html = " <div style='margin-left:" + leftMargin + "%;float:left;width:" + width + "%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                }

                                if (e.Row.Cells.Count > index)
                                    e.Row.Cells[index].Text = html;
                            }
                        }
                    }
                }
            }
        }

        protected void grid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            bool needToHandleMultiValue = false;

            if (moduleColumns.Length == 0 && !string.IsNullOrEmpty(Module))
                moduleColumns = moduleColumnManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, Module));
            if (moduleColumns.Length > 0)
            {
                DataRow moduleColumn = moduleColumns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.FieldName) == e.Column.FieldName).FirstOrDefault();
                if (moduleColumn != null && moduleColumn[DatabaseObjects.Columns.ColumnType] != null && (Convert.ToString(moduleColumn[DatabaseObjects.Columns.ColumnType]).ToLower() == "multiuser" || Convert.ToString(moduleColumn[DatabaseObjects.Columns.ColumnType]).ToLower() == "multilookup"))
                    needToHandleMultiValue = true;
            }

            if (needToHandleMultiValue)
            {
                List<FilterValue> temp = new List<FilterValue>(); // List of filter value objects
                List<string> values = new List<string>(); // List in string format used to keep out duplicates
                foreach (FilterValue fValue in e.Values)
                {
                    if (fValue.Value.Contains(";"))
                    {
                        // Found multiple semi-colon separated values
                        string[] vals = fValue.Value.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string val in vals)
                        {
                            // Add to filter list only if not already in it
                            string trimmedVal = val.Trim();
                            if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                            {
                                temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                                values.Add(trimmedVal);
                            }
                        }
                    }
                    else
                    {
                        // Single value, add to list if not already in it
                        string trimmedVal = fValue.Value.Trim();
                        if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                        {
                            temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                            values.Add(trimmedVal);
                        }
                    }
                }

                // Add to filter list in order
                temp = temp.OrderBy(o => o.Value).ToList();
                e.Values.Clear();
                foreach (FilterValue fVal in temp)
                {
                    e.Values.Add(fVal);
                }
            }

            string fieldName = e.Column.FieldName;

            DataRow[] drs = moduleMonitorsTable != null ? moduleMonitorsTable.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.MonitorName, fieldName)) : null;
            if (drs != null && drs.Length == 0)
            {
                FilterValue fvBlanks = new FilterValue("(Blanks)", "Blanks", string.Format("not ([{0}] is not null and [{0}] <> '')", e.Column.FieldName)); //not ([{0}] is not null and [{0}] <> '')
                FilterValue fvNonBlanks = new FilterValue("(Non Blanks)", "Non Blanks", string.Format("[{0}] is not null and [{0}] <> ''", e.Column.FieldName));

                e.Values.Insert(0, fvNonBlanks);
                e.Values.Insert(0, fvBlanks);
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (resultedTable == null)
            {
                GetFilteredData();
            }

            if (resultedTable != null && resultedTable.Rows.Count > 0)
            {
                grid.DataSource = resultedTable;
            }
            else
            {
                grid.DataSource = null;
            }
        }

        #endregion

        #region Helper Methods

        private void FillGantViewdata()
        {
            List<GridViewColumn> gvDataColumn = grid.AllColumns.Where(m => m as GridViewBandColumn == null).ToList();
            List<GridViewColumn> gvBandColumn = grid.AllColumns.Where(m => m as GridViewBandColumn != null).ToList();

            if (gvBandColumn.Count == 0)
                return;

            for (int i = 0; i < grid.VisibleRowCount; i++)
            {

                DataRow currentRow = grid.GetDataRow(i);

                if (currentRow == null)
                    continue;

                DateTime dtStart = DateTime.MinValue;
                DateTime dtEnd = DateTime.MinValue;
                DateTime? pctCompleteDate = null;
                if (currentRow != null)
                {
                    DateTime.TryParse(Convert.ToString(currentRow[DatabaseObjects.Columns.TicketActualStartDate]), out dtStart);
                    DateTime.TryParse(Convert.ToString(currentRow[DatabaseObjects.Columns.TicketActualCompletionDate]), out dtEnd);

                    if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.TicketPctComplete) && !(currentRow[DatabaseObjects.Columns.TicketPctComplete] is DBNull) && dtEnd != DateTime.MinValue && dtStart != DateTime.MinValue)
                    {
                        int days = (int)(Convert.ToDouble(currentRow[DatabaseObjects.Columns.TicketPctComplete]) * (dtEnd - dtStart).Days);
                        pctCompleteDate = dtStart.AddDays(days);
                    }
                }

                for (int j = 0; j < gvDataColumn.Count; j++)
                {
                    #region GanttView
                    int ganttStartIndex = grid.Columns.Count - gvBandColumn.Count;

                    if (!string.IsNullOrEmpty(((GridViewDataColumn)gvDataColumn[j]).Name) &&
                        !((GridViewDataColumn)gvDataColumn[j]).Name.StartsWith("projecthealth"))
                    {
                        if (dtStart != DateTime.MinValue && dtEnd != DateTime.MinValue)
                        {
                            string header = ((GridViewDataColumn)gvDataColumn[j]).ParentBand.Name;
                            int yr = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1] : header);
                            int month = 1;
                            int day = 1;


                            if (zoomLevel == ZoomLevel.Weekly)
                            {

                                month = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0] : "1");
                                day = UGITUtility.StringToInt(((GridViewDataColumn)gvDataColumn[j]).Name);

                                DateTime dtGrid = new DateTime(yr, month, day);
                                if (dtGrid >= dtStart && dtGrid <= dtEnd)
                                {
                                    int bandcolumncount = 0;

                                    for (int k = ganttStartIndex + 1; ((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index >= k; k++)
                                    {
                                        bandcolumncount += ((GridViewBandColumn)grid.Columns[k - 1]).Columns.Count;
                                    }

                                    int index = j + ganttStartIndex + bandcolumncount;

                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "████████████████";
                                }
                            }
                            else
                            {
                                month = UGITUtility.StringToInt(((GridViewDataColumn)gvDataColumn[j]).Name);
                                int period = zoomLevel == ZoomLevel.Yearly ? 12 : (zoomLevel == ZoomLevel.HalfYearly ? 6 : (zoomLevel == ZoomLevel.Quarterly ? 3 : 1));

                                DateTime dtGrid = new DateTime(yr, month, day);
                                DateTime dtPreGrid = dtGrid.AddMonths(-period);
                                DateTime dtPostGrid = dtGrid.AddMonths(+period);

                                if (dtGrid >= dtStart && dtGrid <= dtEnd && dtPostGrid <= dtEnd)
                                {
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)grid.Columns[ganttStartIndex]).Columns.Count;

                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "████████████████";

                                }
                                else if (dtStart >= dtPreGrid && dtStart <= dtPostGrid && dtPostGrid <= dtEnd)
                                {
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)grid.Columns[ganttStartIndex]).Columns.Count;
                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "███████";

                                }
                                else if (dtEnd >= dtGrid && dtEnd <= dtPostGrid)
                                {
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)grid.Columns[ganttStartIndex]).Columns.Count;
                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "██████";
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        private Unit GetWidthByZoomLevel(ZoomLevel zoom)
        {
            Unit width = Unit.Pixel(30);
            switch (zoom)
            {
                case ZoomLevel.Yearly:
                    width = Unit.Pixel(90);
                    break;
                case ZoomLevel.HalfYearly:
                    width = Unit.Pixel(90);
                    break;
                case ZoomLevel.Quarterly:
                    width = Unit.Pixel(45);
                    break;
                case ZoomLevel.Monthly:
                    width = Unit.Pixel(30);
                    break;
                case ZoomLevel.Weekly:
                    width = Unit.Pixel(30);
                    break;
                default:
                    width = Unit.Pixel(30);
                    break;
            }
            return width;
        }

        private DataTable GetFilteredData()
        {
            GetBaseFilteredData();

            if (grid.Columns.Count == 0 && resultedTable != null && resultedTable.Rows.Count > 0)
            {
                GenerateColumns(zoomLevel);
            }
            return resultedTable;
        }

        /// <summary>
        /// Apply base filtered 
        /// </summary>
        private void GetBaseFilteredData()
        {

            LoadFilteredTickets();
            CustomGridModifications();

        }

        /// <summary>
        /// Use this for adding "Calculated fields" in the Filter tickets table
        /// </summary>
        private void CustomGridModifications()
        {
            if (resultedTable == null)
                return;

            #region Calculated Columns
            // Since we dont have/want a Database column in all the Ticket tables for Ticket Age since its a calculated field and would
            // only make the table heavy we add it directly to the DataTable that we plan to bind to the DataGrid

            // Only for single module page types
            if (!resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketAge)
                && resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
            {
                //DataTable columns = moduleColumnManager.GetDataTable();
                DataTable columns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.CategoryName}='{Module}'");
                
                // Check if we have a column by TicketAge in the ModulesColumns table for the current module
                DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == Module
                    && x.Field<bool>(DatabaseObjects.Columns.IsDisplay) == true
                    && x.Field<string>(DatabaseObjects.Columns.FieldName) == DatabaseObjects.Columns.TicketAge).OrderBy(x => x.Field<int>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                if ((selectedColumns != null && selectedColumns.Count() > 0) || Module == string.Empty)
                    resultedTable.Columns.Add(DatabaseObjects.Columns.TicketAge, typeof(int));
            }
            if (!resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketDueIn)
                && resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketDesiredCompletionDate))
            {
                //DataTable columns = moduleColumnManager.GetDataTable();
                DataTable columns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.CategoryName}='{Module}'");
                
                // Check if we have a column by TicketDueIn in the ModulesColumns table for the current module
                DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == Module
                    && x.Field<bool>(DatabaseObjects.Columns.IsDisplay) == true
                    && x.Field<string>(DatabaseObjects.Columns.FieldName) == DatabaseObjects.Columns.TicketDueIn).OrderBy(x => x.Field<int>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                if ((selectedColumns != null && selectedColumns.Count() > 0) || Module == string.Empty)
                    resultedTable.Columns.Add(DatabaseObjects.Columns.TicketDueIn, typeof(int));
            }
            if (!resultedTable.Columns.Contains(DatabaseObjects.Columns.SelfAssign))
            {
                //DataTable columns = moduleColumnManager.GetDataTable();
                DataTable columns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.CategoryName}='{Module}'");

                // Check if we have a column by SelfAssign in the ModulesColumns table for the current module
                DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == Module
                    && x.Field<bool>(DatabaseObjects.Columns.IsDisplay) == true
                    && x.Field<string>(DatabaseObjects.Columns.FieldName) == DatabaseObjects.Columns.SelfAssign).OrderBy(x => x.Field<int>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                if (selectedColumns != null && selectedColumns.Count() > 0)
                    resultedTable.Columns.Add(DatabaseObjects.Columns.SelfAssign, typeof(string));
            }

            if (moduleRow == null)
            {
                int moduleId = 0;
                int.TryParse(this.Module, out moduleId);
                //moduleRow = moduleViewManager.GetDataTable().AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == this.Module || x.Field<int>(DatabaseObjects.Columns.Id) == moduleId);
                moduleRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and ({DatabaseObjects.Columns.ModuleName}='{this.Module}' or {DatabaseObjects.Columns.Id}={moduleId})").Select()[0];
            }
            //Use this to add column for monitor in case of PMM 
            if (Module.Trim().ToLower() == "pmm" && moduleRow != null)
            {
                //add monitor columns
                DataColumn ModuleMonitorOptionLEDClassLookup = new DataColumn(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup, typeof(string));
                DataColumn ModuleMonitorName = new DataColumn(DatabaseObjects.Columns.ModuleMonitorName, typeof(string));
                DataColumn ModuleMonitorOptionNameLookup = new DataColumn(DatabaseObjects.Columns.ModuleMonitorOptionNameLookup, typeof(string));


                if (moduleMonitorsTable == null)
                {
                    //Load All Monitor in case of pmm projects

                    projectMonitorsStateTable = projectMonitorStateManager.GetDataTable(); 

                    moduleMonitorsTable = moduleMonitorManager.GetDataTable();
                    DataRow[] moduleMonitorsTableColl = moduleMonitorsTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, Module));
                    if (moduleMonitorsTableColl != null && moduleMonitorsTableColl.Length > 0)
                        moduleMonitorsTable = moduleMonitorsTableColl.CopyToDataTable();
                }
                if (moduleMonitorsTable != null && moduleMonitorsTable.Rows.Count > 0)
                {

                    if (!projectMonitorsStateTable.Columns.Contains(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup))
                    {
                        projectMonitorsStateTable.Columns.Add(ModuleMonitorOptionLEDClassLookup);
                    }
                    if (!projectMonitorsStateTable.Columns.Contains(DatabaseObjects.Columns.ModuleMonitorName))
                    {
                        projectMonitorsStateTable.Columns.Add(ModuleMonitorName);
                    }
                    if (!projectMonitorsStateTable.Columns.Contains(DatabaseObjects.Columns.ModuleMonitorOptionNameLookup))
                    {
                        projectMonitorsStateTable.Columns.Add(ModuleMonitorOptionNameLookup);
                    }
                    foreach (DataRow monitor in moduleMonitorsTable.Rows)
                    {
                        string colName = string.Empty;
                        if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.MonitorName) && !resultedTable.Columns.Contains(UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString()))
                        {
                            colName = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString();
                            resultedTable.Columns.Add(colName);
                        }

                        if (resultedTable.Rows != null && resultedTable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in resultedTable.Rows)
                            {
                                //ModuleMonitor moduleMonitor = moduleMonitorManager.Load(x => x.MonitorName == colName).FirstOrDefault();
                                if (projectMonitorsStateTable != null && projectMonitorsStateTable.Rows.Count > 0)
                                {
                                    DataRow drData = projectMonitorsStateTable.Select(string.Format("{0} = '{1}' and {2} = '{3}'", DatabaseObjects.Columns.TicketId, Convert.ToString(dr[DatabaseObjects.Columns.TicketId]), DatabaseObjects.Columns.ModuleMonitorNameLookup, UGITUtility.ObjectToString(monitor["ID"]))).FirstOrDefault();
                                    if (drData != null)
                                    {
                                        ModuleMonitorOption moduleMonitorOption = moduleMonitorOptionManager.LoadByID(Convert.ToInt64(drData[DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                                        //dr[colName] = Convert.ToString(moduleMonitorOption.ModuleMonitorOptionLEDClass).Replace("LED", string.Empty);
                                        drData[DatabaseObjects.Columns.ModuleMonitorName] = monitor["MonitorName"];
                                        drData[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = moduleMonitorOption.ModuleMonitorOptionLEDClass;
                                        drData[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = moduleMonitorOption.ModuleMonitorOptionName;
                                        dr[colName] = UGITUtility.GetMonitorsGraphic(drData);
                                    }
                                    else
                                    {
                                        dr[colName] = @"<div><span style='margin: 0 8px 0 8px;' class='GreenLED monitoricon'><span style='display:none;' class='info'>Critical Issues - No major issues</span></span></div>";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Use this to fill default values in the newly added columns,
            if (moduleColumns.Length == 0)
            {
                if (!string.IsNullOrEmpty(Module))
                {
                    //moduleColumns = moduleColumnManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.CategoryName, Module));
                    if (string.IsNullOrEmpty(UserSelectedColumns))
                        moduleColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.CategoryName}='{Module}'").Select();
                    else
                        moduleColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.CategoryName}='{Module}' and {DatabaseObjects.Columns.ID} in ({UserSelectedColumns})").Select();
                }
            }
            if (moduleColumns.Length > 0)
            {
                //DataRow[] multiuserColumns = moduleColumns.CopyToDataTable().Select(string.Format("{0} = 'MultiUser' or {0} = 'MultiLookup'", DatabaseObjects.Columns.ColumnType));
                DataRow[] multiuserColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.CategoryName}='{Module}' and {DatabaseObjects.Columns.ColumnType} in ('MultiUser','MultiLookup')").Select();
                foreach (DataRow row in resultedTable.Rows)
                {
                    if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketDueIn) && Convert.ToString(row[DatabaseObjects.Columns.TicketDesiredCompletionDate]) != string.Empty)
                        row[DatabaseObjects.Columns.TicketDueIn] = (UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.TicketDesiredCompletionDate]) - DateTime.Now).Days;

                    if (moduleColumns.Length > 0)
                    {
                        foreach (DataRow moduleColumn in multiuserColumns)
                        {
                            if (resultedTable.Columns.Contains(Convert.ToString(moduleColumn[DatabaseObjects.Columns.FieldName])))
                                row[Convert.ToString(moduleColumn[DatabaseObjects.Columns.FieldName])] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[Convert.ToString(moduleColumn[DatabaseObjects.Columns.FieldName])]));
                        }
                    }
                }
            }
            else if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
            {
                // Needed because in dashboard case we don't use ModuleColumns, yet need to handle multi-user values in Owner field
                foreach (DataRow row in resultedTable.Rows)
                {
                    row[DatabaseObjects.Columns.TicketOwner] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[DatabaseObjects.Columns.TicketOwner]));
                }
            }

            if (Module.Trim().ToLower() == "cmdb" && resultedTable != null && !resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketId))
            {
                resultedTable.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string), string.Format("[{0}]", DatabaseObjects.Columns.AssetTagNum));
            }
            #endregion
        }

        private void LoadFilteredTickets()
        {
            
            DataTable dtResult = new DataTable();
            if (OpenProjectOnly.EqualsIgnoreCase("true"))
            {
                dtResult = ticketManager.GetOpenTickets(moduleViewManager.GetByName(Module));
            }
            else if(OpenProjectOnly.EqualsIgnoreCase("false"))
            {
                dtResult = ticketManager.GetClosedTickets(moduleViewManager.GetByName(Module));
            }
            else if (OpenProjectOnly.EqualsIgnoreCase("all"))
            {
                dtResult = ticketManager.GetAllTickets(moduleViewManager.GetByName(Module));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
                dtResult.Columns[DatabaseObjects.Columns.ModuleNameLookup].Expression = "'" + Module + "'";
            }
            /*
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.Format("{0}<>1", DatabaseObjects.Columns.Deleted);
            dt = dv.ToTable();
            */
            DataView dv = dtResult.DefaultView;
            dv.RowFilter = $"{DatabaseObjects.Columns.TicketActualStartDate} IS NOT NULL AND {DatabaseObjects.Columns.TicketActualCompletionDate} IS NOT NULL";
            dtResult = dv.ToTable();

            dtResult.DefaultView.Sort = $"{ColumnsSortOrder}";
            resultedTable = dtResult.DefaultView.ToTable();
        }

        /// <summary>
        /// Bind datacolumn to gridview
        /// </summary>
        private void GenerateColumns(ZoomLevel? zoom)
        {
            grid.Columns.Clear();

            if (grid.Columns.Count == 0)
            {
                DataRow[] selectedColumns = new DataRow[0];

                List<string> mandatoryColumns = new List<string>();

                //selectedColumns = moduleColumns.Where(x => x.Field<string>(DatabaseObjects.Columns.DisplayForReport) == "True").ToArray();
                selectedColumns = moduleColumns.Where(x => x.Field<bool>(DatabaseObjects.Columns.DisplayForReport) == true).ToArray();
                // Add TicketId and Title if no column selected.
                if (selectedColumns == null || selectedColumns.Length == 0)
                {
                    mandatoryColumns.Add(DatabaseObjects.Columns.TicketId);
                    mandatoryColumns.Add(DatabaseObjects.Columns.Title);
                }

                // To include grouping column
                if (GroupBy == "1")
                    mandatoryColumns.Add(DatabaseObjects.Columns.TicketPriorityLookup);
                else if (GroupBy == "2")
                    mandatoryColumns.Add(DatabaseObjects.Columns.TicketRequestTypeLookup);
                else if (GroupBy == "3")
                    mandatoryColumns.Add(DatabaseObjects.Columns.ProjectInitiativeLookup);
                else
                {
                    imgCollapse.Visible = false;
                    imgExpand.Visible = false;
                }


                if (mandatoryColumns != null && mandatoryColumns.Count > 0)
                {
                    DataRow dr = moduleColumns.FirstOrDefault(x => mandatoryColumns.Contains(x.Field<string>(DatabaseObjects.Columns.FieldName)));
                    if (dr != null)
                    {
                        dr[DatabaseObjects.Columns.FieldSequence] = -1;
                    }
                }

                //selectedColumns = moduleColumns.Where(x => x.Field<string>(DatabaseObjects.Columns.DisplayForReport) == "True" || mandatoryColumns.Contains(x.Field<string>(DatabaseObjects.Columns.FieldName))).OrderBy(x => Convert.ToDouble(x.Field<object>(DatabaseObjects.Columns.FieldSequence))).ToArray();
                selectedColumns = moduleColumns.Where(x => x.Field<bool>(DatabaseObjects.Columns.DisplayForReport) == true || mandatoryColumns.Contains(x.Field<string>(DatabaseObjects.Columns.FieldName))).OrderBy(x => Convert.ToDouble(x.Field<object>(DatabaseObjects.Columns.FieldSequence))).ToArray();

                if (mandatoryColumns != null && mandatoryColumns.Count > 0)
                {
                    List<string> requiredColumns = new List<string>();
                    requiredColumns.AddRange(mandatoryColumns);
                        //mandatoryColumns;
                    foreach (string item in mandatoryColumns)
                    {
                        DataRow dr = selectedColumns.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.FieldName) == item);
                        if (dr != null)
                        {
                            requiredColumns.Remove(item);
                        }
                    }

                    DataTable dtMandatoryColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}' and {DatabaseObjects.Columns.CategoryName}='{Module}' and {DatabaseObjects.Columns.FieldName} in ('{ string.Join("','", requiredColumns)}')");
                    DataTable dtSelectedColumns = selectedColumns.CopyToDataTable();
                    dtSelectedColumns.Merge(dtMandatoryColumns);
                    selectedColumns = dtSelectedColumns.Select();
                }

                Dictionary<string, string> customProperties = new Dictionary<string, string>();
                StringBuilder filterDataFields = new StringBuilder();
                moduleRequest = new Ticket(_context, Module);

                #region "Generate columns for Modules"
                foreach (DataRow moduleColumn in selectedColumns)
                {
                    //1. check for column exist is resultedtable or not
                    //2. Check if closed tickets are being shown then only specified column will be shown.
                    string fieldColumn = Convert.ToString(moduleColumn["FieldName"]);
                    if (resultedTable != null && (resultedTable.Columns.Contains(fieldColumn) || fieldColumn.ToLower() == "projectmonitors"))
                    {
                        Type dataType = typeof(string);
                        if (resultedTable.Columns.Contains(fieldColumn))
                            dataType = resultedTable.Columns[fieldColumn].DataType;


                        GridViewDataTextColumn colId = null;
                        GridViewDataDateColumn dateTimeColumn = null;

                        if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketStatus.ToLower())
                        {
                            colId = new GridViewDataTextColumn();
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = Convert.ToString(moduleColumn["FieldName"]);
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketStatus);
                            //colId.FixedStyle = GridViewColumnFixedStyle.Left;
                            grid.Columns.Add(colId);
                        }
                        else if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketAge.ToLower())
                        {
                            colId = new GridViewDataTextColumn();
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = Convert.ToString(moduleColumn["FieldName"]);
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketAge);
                            //colId.FixedStyle = GridViewColumnFixedStyle.Left;
                            grid.Columns.Add(colId);
                        }
                        else if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketPctComplete.ToLower())
                        {
                            colId = new GridViewDataTextColumn();
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = Convert.ToString(moduleColumn["FieldName"]);
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            colId.PropertiesEdit.DisplayFormatString = "{0:0.#%}";
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketPctComplete);
                            //colId.FixedStyle = GridViewColumnFixedStyle.Left;
                            grid.Columns.Add(colId);
                        }
                        else if (dataType == typeof(DateTime))
                        {
                            dateTimeColumn = new GridViewDataDateColumn();
                            dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            dateTimeColumn.FieldName = Convert.ToString(moduleColumn["FieldName"]);
                            dateTimeColumn.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                            dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                            dateTimeColumn.HeaderStyle.Font.Bold = true;
                            dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                            dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            dateTimeColumn.Width = GetColumnWidth("DateTime");
                            //colId.FixedStyle = GridViewColumnFixedStyle.Left;
                            grid.Columns.Add(dateTimeColumn);
                        }


                        else if (fieldColumn.ToLower() == "projectmonitors" && Module.Trim().ToLower() == "pmm")
                        {
                            string headerText = string.Empty;
                            if (moduleMonitorsTable != null && moduleMonitorsTable.Rows.Count > 0)
                            {
                                foreach (DataRow monitor in moduleMonitorsTable.Rows)
                                {
                                    if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.Title))
                                    {
                                        colId = new GridViewDataTextColumn();
                                        colId.PropertiesTextEdit.EncodeHtml = false;
                                        colId.FieldName = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString();
                                        colId.Name = string.Format("projecthealth{0}", moduleMonitorsTable.Rows.IndexOf(monitor));
                                        colId.Caption = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.Title).ToString(); // "Issues Scope $$$$ OnTime Risk";  // 5 monitors: Issues, Scope, Budget, Time, Risk
                                        colId.Width = GetColumnWidth("ProjectHealth");
                                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                        colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                                        //colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                                        //colId.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Width = GetColumnWidth("ProjectMonitors");
                                        colId.HeaderStyle.Font.Bold = true;
                                        grid.Columns.Add(colId);
                                    }
                                }
                            }
                        }
                        else
                        {
                            colId = new GridViewDataTextColumn();
                            colId.FieldName = Convert.ToString(moduleColumn["FieldName"]);

                            string dollorCol = (fieldColumn.ToLower().EndsWith("lookup") || fieldColumn.ToLower().EndsWith("user")) ? string.Format("{0}$", fieldColumn) : fieldColumn;
                            if (dollorCol.EndsWith("$") && resultedTable.Rows.Count > 0 && UGITUtility.IfColumnExists(resultedTable.Rows[0], dollorCol))
                                colId.FieldName = dollorCol;

                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                            {
                                colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                            }

                            if (fieldColumn == DatabaseObjects.Columns.TicketPriorityLookup && GroupBy == "1")
                                colId.GroupIndex = 0;
                            else if (fieldColumn == DatabaseObjects.Columns.TicketRequestTypeLookup && GroupBy == "2")
                                colId.GroupIndex = 0;
                            else if (fieldColumn == DatabaseObjects.Columns.ProjectInitiativeLookup && GroupBy == "3")
                            {
                                colId.Caption = string.IsNullOrEmpty(_context.ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name)) ? Convert.ToString(moduleColumn["FieldDisplayName"]) : _context.ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name);
                                colId.GroupIndex = 0;
                            }

                            if (fieldColumn.ToLower() == DatabaseObjects.Columns.Title.ToLower())
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                            }
                            else if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketId);
                            }
                            else
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketPriority.ToLower() ||
                                fieldColumn.ToLower() == DatabaseObjects.Columns.TicketPriorityLookup.ToLower())
                                {
                                    colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketPriorityLookup);

                                }
                                else if (fieldColumn.ToLower() == DatabaseObjects.Columns.FunctionalAreaLookup.ToLower())
                                {
                                    colId.Width = GetColumnWidth(DatabaseObjects.Columns.FunctionalAreaLookup);
                                }
                            }

                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                            if (dataType == typeof(double) || dataType == typeof(float) || dataType == typeof(decimal))
                            {
                                colId.PropertiesEdit.DisplayFormatString = "#.##";
                            }
                            //colId.FixedStyle = GridViewColumnFixedStyle.Left;

                            colId.HeaderStyle.Font.Bold = true;
                            grid.Columns.Add(colId);
                        }
                    }

                }
                #endregion

                if (zoom != null)
                {
                    if (!resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketActualCompletionDate) || !resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketActualStartDate))
                    {
                        return;
                    }

                    DateTime dbminValue = new DateTime(1800, 1, 1);
                    var compDate = resultedTable.AsEnumerable().Where(m => !m.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) 
                                                                            && m.Field<object>(DatabaseObjects.Columns.TicketActualCompletionDate) != DBNull.Value
                                                                            && m.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) != DateTime.MinValue 
                                                                            && m.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) != dbminValue);
                    DateTime dtMax = compDate.Count() == 0 ? DateTime.MaxValue : compDate.Max(m => m.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate));

                    var stDate = resultedTable.AsEnumerable().Where(m => !m.IsNull(DatabaseObjects.Columns.TicketActualStartDate)
                                                                            && m.Field<object>(DatabaseObjects.Columns.TicketActualStartDate) != DBNull.Value
                                                                            && m.Field<DateTime>(DatabaseObjects.Columns.TicketActualStartDate) != DateTime.MinValue
                                                                            && m.Field<DateTime>(DatabaseObjects.Columns.TicketActualStartDate) != dbminValue);

                    DateTime dtMin = stDate.Count() == 0 ? DateTime.MinValue : stDate.Min(m => m.Field<DateTime>(DatabaseObjects.Columns.TicketActualStartDate));

                    if (dtMax != DateTime.MaxValue & dtMin != DateTime.MinValue)
                    {
                        switch (zoom)
                        {
                            case ZoomLevel.Monthly:

                                for (DateTime startDate = dtMin; startDate < dtMax.AddMonths(6); startDate = startDate.AddMonths(6))
                                {
                                    GridViewBandColumn col = new GridViewBandColumn();
                                    int monthBar = startDate.Month > 6 ? 12 : 6;
                                    col.Name = (monthBar == 6 ? 1 : 7) + " " + startDate.Year.ToString();

                                    col.Caption = (monthBar == 6 ? "January" : "July") + " " + startDate.Year.ToString();

                                    col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                                    col.HeaderStyle.Font.Bold = true;

                                    for (int i = monthBar > 6 ? 7 : 1; i <= monthBar; i++)
                                    {
                                        GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                        subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                        subcol.Name = i.ToString();
                                        subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3);
                                        subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                        subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                        subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                        subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                        subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        //subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.HeaderStyle.Font.Bold = true;
                                        col.Columns.Add(subcol);

                                        string colName = subcol.Name + "-" + col.Name;
                                        if (!resultedTable.Columns.Contains(colName))
                                            resultedTable.Columns.Add(colName);
                                    }
                                    grid.Columns.Add(col);

                                }
                                break;
                            case ZoomLevel.Quarterly:

                                for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                                {
                                    GridViewBandColumn col = new GridViewBandColumn();
                                    col.Caption = yrs.ToString();
                                    col.Name = yrs.ToString();
                                    col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    col.HeaderStyle.Font.Bold = true;

                                    for (int i = 0; i < 12; i += 3)
                                    {
                                        GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                        subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                        subcol.Name = (i + 1).ToString();
                                        subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1).Substring(0, 3);
                                        subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                        subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                        subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                        subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                        subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        //subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.HeaderStyle.Font.Bold = true;
                                        string colName = subcol.Name + "-" + col.Name;
                                        if (!resultedTable.Columns.Contains(colName))
                                            resultedTable.Columns.Add(colName);
                                        col.Columns.Add(subcol);
                                    }
                                    grid.Columns.Add(col);
                                }
                                break;
                            case ZoomLevel.HalfYearly:
                                for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                                {
                                    GridViewBandColumn col = new GridViewBandColumn();
                                    col.Caption = yrs.ToString();
                                    col.Name = yrs.ToString();
                                    col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                    col.HeaderStyle.Font.Bold = true;

                                    for (int i = 0; i < 12; i += 6)
                                    {
                                        GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                        subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                        subcol.Name = (i + 1).ToString();
                                        subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1).Substring(0, 3);
                                        subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                        subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                        subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                        subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                        subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        //subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.HeaderStyle.Font.Bold = true;
                                        string colName = subcol.Name + "-" + col.Name;
                                        if (!resultedTable.Columns.Contains(colName))
                                            resultedTable.Columns.Add(colName);
                                        col.Columns.Add(subcol);
                                    }
                                    grid.Columns.Add(col);
                                }
                                break;
                            case ZoomLevel.Yearly:
                                for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                                {
                                    GridViewBandColumn col = new GridViewBandColumn();
                                    col.Caption = yrs.ToString();
                                    col.Name = yrs.ToString();
                                    col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                    col.HeaderStyle.Font.Bold = true;

                                    //for (int i = 0; i < 12; i += 12)
                                    //{
                                    GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                    subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                    subcol.Name = (1).ToString();
                                    subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1).Substring(0, 3);
                                    subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                    subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                    subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                    subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                    //subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.HeaderStyle.Font.Bold = true;
                                    string colName = subcol.Name + "-" + col.Name;
                                    if (!resultedTable.Columns.Contains(colName))
                                        resultedTable.Columns.Add(colName);
                                    col.Columns.Add(subcol);
                                    // }
                                    grid.Columns.Add(col);
                                }
                                break;
                            case ZoomLevel.Weekly:
                                for (DateTime startDate = dtMin.AddDays(-1 * (dtMin.Day - 1)); startDate < dtMax; startDate = startDate.AddMonths(1))
                                {
                                    GridViewBandColumn col = new GridViewBandColumn();

                                    col.Name = startDate.Month.ToString() + " " + startDate.Year.ToString();

                                    col.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month) + " " + startDate.Year.ToString();
                                    col.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                    col.HeaderStyle.Font.Bold = true;
                                    DateTime dt = new DateTime(startDate.Year, startDate.Month, 1);
                                    for (DateTime ctrDate = dt; ctrDate.Day < DateTime.DaysInMonth(startDate.Year, startDate.Month); ctrDate = ctrDate.AddDays(1))
                                    {
                                        if (ctrDate.DayOfWeek == DayOfWeek.Monday)
                                        {
                                            GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                            subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                            subcol.Name = (ctrDate.Day).ToString();
                                            subcol.Caption = (ctrDate.Day).ToString();
                                            subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                            subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                            subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                            subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                            subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                            subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                            //subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                            subcol.HeaderStyle.Font.Bold = true;
                                            string colName = subcol.Name + "-" + col.Name;
                                            if (!resultedTable.Columns.Contains(colName))
                                                resultedTable.Columns.Add(colName);
                                            col.Columns.Add(subcol);
                                        }
                                    }
                                    grid.Columns.Add(col);
                                }
                                break;
                        }

                    }
                }
                
            }
        }

        private Unit GetColumnWidth(string columnName)
        {
            Unit width = new Unit("30px");

            if (columnName == DatabaseObjects.Columns.TicketPriority || columnName == DatabaseObjects.Columns.TicketPriorityLookup)
            {
                width = new Unit("85px");
            }
            else if (columnName == DatabaseObjects.Columns.TicketId)
            {
                width = new Unit("105px");
            }
            else if (columnName == "DateTime")
            {
                width = new Unit("105px");
            }
            else if (columnName == DatabaseObjects.Columns.TicketStatus)
            {
                width = new Unit("140px");
            }
            else if (columnName == DatabaseObjects.Columns.TicketAge || columnName == "ProjectMonitors")
            {
                width = new Unit("50px");
            }
            else if (columnName == "ProjectHealth" || columnName == "SelfAssign" || columnName == DatabaseObjects.Columns.ProjectRank)
            {
                width = new Unit("30px");
            }
            else if (columnName == DatabaseObjects.Columns.FunctionalAreaLookup)
            {
                width = new Unit("120px");
            }
            else if (columnName == DatabaseObjects.Columns.TicketPctComplete)
            {
                width = new Unit("75px");
            }
            // else use default width from above

            return width;
        }

        private string GetImage(string className)
        {
            string fileName = string.Empty;
            switch (className)
            {
                case "GreenLED":
                    fileName = "Green"; //UGITUtility.GetImageUrlForReport("/Content/Images/LED_Green.png");
                    break;
                case "YellowLED":
                    fileName = "Yellow"; // UGITUtility.GetImageUrlForReport("/Content/Images/LED_Yellow.png");
                    break;
                case "RedLED":
                    fileName = "Red"; // UGITUtility.GetImageUrlForReport("/Content/Images/uGovernIT/LED_Red.png");
                    break;
                default:
                    break;
            }

            return fileName;
        }

        #endregion

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context,false);
        }

        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            /*
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            FieldConfiguration field = fieldManager.GetFieldByFieldName(e.Column.FieldName);
            if(field != null)
            {
                if(field.Datatype == "Lookup")
                {
                    e.DisplayText = fieldManager.GetFieldConfigurationData(field.FieldName, Convert.ToString(e.Value));
                }
                if(field.Datatype == "UserField")
                {
                    List<uGovernIT.Utility.Entities.UserProfile> userProfiles = HttpContext.Current.GetUserManager().GetUserInfosById(Convert.ToString(e.Value));
                    if (userProfiles != null && userProfiles.Count > 0)
                    {
                        e.DisplayText = string.Join(Constants.Separator6, userProfiles.Select(x => x.UserName));
                    }
                }
            }
            */
        }
    }
}

