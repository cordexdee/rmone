using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using DevExpress.Web;
using System.Drawing;
using DevExpress.XtraReports.UI;
using DevExPrinting = DevExpress.XtraPrinting;
using System.Text;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using ReportEntity = uGovernIT.Manager;
using uGovernIT.Manager.Managers;

namespace uGovernIT.ControlTemplates.uGovernIT
{
    public partial class ResourceERH : UserControl
    {
        #region Member Variables
        protected int year = System.DateTime.Now.Year;
        private DateTime dateFrom;
        private DateTime dateTo;
        private string manager;
        private string managerName;
        private List<string> columnList = new List<string>();
        protected bool printReport = false; 
        string title = string.Empty;
        protected StringBuilder urlBuilder = new StringBuilder();
        static DataTable dttempTable;
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            loadPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblReportTitle.Text = "Estimated Remaining Hours: " + managerName;
            lblReportDateRange.Text = "<b>Report From: </b>" + UGITUtility.getDateStringInFormat(dateFrom, false) + "<b> To </b>" + UGITUtility.getDateStringInFormat( dateTo, false);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(System.Web.HttpContext.Current, false);
        }

        protected void gvEstimatedRemaningHoursReport_DataBinding(object sender, EventArgs e)
        {
            gvEstimatedRemaningHoursReport.DataSource = LoadWorkSheetByDate(manager, dateFrom, dateTo);
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            DataTable ResultData = LoadWorkSheetByDate(manager, dateFrom, dateTo);
            XtraReport report = reportHelper.GenerateReport(gvEstimatedRemaningHoursReport, ResultData, lblReportTitle.Text, 8F, "xls");
            reportHelper.WriteXlsToResponse(Response, lblReportTitle.Text + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            DataTable ResultData = LoadWorkSheetByDate(manager, dateFrom, dateTo);
            XtraReport report = reportHelper.GenerateReport(gvEstimatedRemaningHoursReport, ResultData, lblReportTitle.Text, 6.75F);
            reportHelper.WritePdfToResponse(Response, lblReportTitle.Text + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DevExPrinting.PdfExportOptions option = new DevExPrinting.PdfExportOptions();
            option.ShowPrintDialogOnOpen = true;
            gridExporter.WritePdfToResponse(lblReportTitle.Text, option);
        }
        private void loadPage()
        {

            if (string.IsNullOrEmpty(Request["DateTo"]) ||
                !DateTime.TryParse(Convert.ToString(Request["DateTo"]), out dateTo))
            {
                dateTo = DateTime.Now.Date;
            }

            if (string.IsNullOrEmpty(Request["DateFrom"]) ||
                !DateTime.TryParse(Convert.ToString(Request["DateFrom"]), out dateFrom))
            {
                dateFrom = dateTo.AddDays(-1); 
            }

            if (!string.IsNullOrEmpty(Request["Manager"]))
            {
                manager = Convert.ToString(Request["Manager"]);
            }
            if (!string.IsNullOrEmpty(Request["ManagerName"]))
            {
                managerName = Convert.ToString(Request["ManagerName"]);
            }

            gvEstimatedRemaningHoursReport.Settings.ShowFilterRowMenu = true;
            gvEstimatedRemaningHoursReport.Settings.ShowHeaderFilterButton = true;
            gvEstimatedRemaningHoursReport.SettingsPopup.HeaderFilter.Height = 200;
            gvEstimatedRemaningHoursReport.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            gvEstimatedRemaningHoursReport.Styles.AlternatingRow.BackColor = Color.FromArgb(245, 245, 245);
            gvEstimatedRemaningHoursReport.Styles.Row.BackColor = Color.White;
            gvEstimatedRemaningHoursReport.Styles.GroupRow.Font.Bold = true;
            gvEstimatedRemaningHoursReport.Settings.GridLines = GridLines.None;
        }

        void reportHelper_CustomizeColumnsCollection(object source, ReportEntity.ColumnsCreationEventArgs e)
        {
            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Title"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Title").ColumnWidth *= 2;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Rank"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Rank").ColumnWidth = 50;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Priority"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Priority").ColumnWidth = 50;
        }
        protected override void OnPreRender(EventArgs e)
        {
            urlBuilder.Append(UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery));
            gvEstimatedRemaningHoursReport.DataBind();
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // If request is for export the report in given form like pdf,excel etc.
            if (Request["ExportReport"] != null)
            {
                string exportType = Request["exportType"];

                if (exportType == "excel")
                {
                    // Filter && Copy the budget table in temporary table.
                    DataTable excelTable = LoadWorkSheetByDate(manager, dateFrom, dateTo);

                    // Convert the data in csv format.
                    string csvData = UGITUtility.ConvertTableToCSV(excelTable);
                    string attachment = string.Format("attachment; filename={0}.csv", "Export");
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "text/csv";
                    Response.Write(csvData.ToString());
                    Response.Flush();
                    Response.End();
                }
                // In case of print is called from report
                else if (exportType == "print")
                {
                    btnClose.Visible = false;
                    exportPanel.Visible = false;
                    printReport = true;
                }
            }

            base.Render(writer);
        }
        #region fillData
        public static DataTable LoadWorkSheetByDate(string userID, DateTime startDate, DateTime endDate)
        {
            
             //Creates timesheet table
             DataTable resultTable = CreateTableSchema();
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            //Loads workitems and timesheet and allocation data in datatable
            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
            ResourceTimeSheetManager resourceTimeSheetManager = new ResourceTimeSheetManager(context);
            ResourceWorkItemsManager resourceWorkItemsManager = new ResourceWorkItemsManager(context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            TicketManager ticketManager = new TicketManager(context);

            DataTable workItems = resourceWorkItemsManager.LoadRawTableByResource(Convert.ToString(userID), 1);
              
            DataTable timeSheetTable = resourceTimeSheetManager.LoadRawTableByResource(Convert.ToString(userID) ,startDate, endDate);
            DataTable allocations = resourceAllocationManager.LoadRawTableByResource(Convert.ToString(userID), 4);

            //Create StartDate Column which store AllocationStartDate value as datetime
            //Because AllocationStartDate come in string format which is useless to query on date
            if (allocations != null && allocations.Columns.Contains(DatabaseObjects.Columns.AllocationStartDate))
            {
                allocations.Columns.Add("StartDate", typeof(DateTime));
                allocations.Columns["StartDate"].Expression = DatabaseObjects.Columns.AllocationStartDate;
            }

            //Create EndDate Column which store AllocationEndDate value as datetime
            //Because AllocationEndDate come in string format which is useless to query on date
            if (allocations != null && allocations.Columns.Contains(DatabaseObjects.Columns.AllocationEndDate))
            {
                allocations.Columns.Add("EndDate", typeof(DateTime));
                allocations.Columns["EndDate"].Expression = DatabaseObjects.Columns.AllocationEndDate;
            }

            //if there is not work item then return right away. workitem must be exist if there is any allocation
            if (workItems == null || workItems.Rows.Count <= 0)
            {
                return resultTable;
            }

            //Overlaps between AllocationRange
            string query = string.Format("('{0}'<= {3} AND '{1}' >= {2})",
                startDate.ToString("MM/dd/yyyy"), endDate.ToString("MM/dd/yyyy"), "StartDate", "EndDate");
            DataTable dtAllocationForCurrentWeek = new DataTable();
            //Loads current week existing allocation of user
            DataRow[] allocationsForCurrentWeek = new DataRow[0];
            if (allocations != null && allocations.Rows.Count > 0)
            {
                allocationsForCurrentWeek = allocations.Select(query);
            }

            DataRow allocationRow = null;
            DataRow[] timesheetRows = new DataRow[0];
            DataRow[] timesheetAllRows = (timeSheetTable != null ? timeSheetTable.Select() : null);

            DateTime tempDate = startDate;
            //Iterates on workitems 
            foreach (DataRow workItem in workItems.Rows)
            {
                //get task details by Id....
                string[] arr = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                DataRow sptaskList = null;
                DataRow spProjectList = null;
                if (arr.Length == 2)
                {
                    if (Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]) == "TSK")
                    {
                        sptaskList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKTasks, Convert.ToInt32(arr[0])).Select()[0];
                    }
                    else
                    {
                        sptaskList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMTasks, Convert.ToInt32(arr[0])).Select()[0];
                    }
                }
                else
                { 
                    spProjectList = Ticket.GetCurrentTicket(context,Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]),Convert.ToString(workItem[DatabaseObjects.Columns.WorkItem]));
                }

                //foreach(var item in allocationsForCurrentWeek)
                //{
                //    var _resourceWorkItemLookup = item.Field<long>(DatabaseObjects.Columns.ResourceWorkItemLookup);
                //    if(_resourceWorkItemLookup == Convert.ToInt64(workItem[DatabaseObjects.Columns.Id]))
                //    {
                //        allocationRow = item;
                //    }
                   
                //}
                //loads if allocation is exist for current workitem
                allocationRow = allocationsForCurrentWeek.FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ResourceWorkItemLookup) == Convert.ToInt64(workItem[DatabaseObjects.Columns.Id]));





                if (allocationRow == null)
                {
                    //Loads if any timesheet entry is exist in timesheet list against current workitem
                    if (timesheetAllRows != null && timesheetAllRows.Length > 0)
                    {
                        timesheetRows = timesheetAllRows.Where(x => x.Field<Int64>(DatabaseObjects.Columns.ResourceWorkItemLookup) ==Convert.ToInt64( workItem[DatabaseObjects.Columns.Id])).ToArray();
                    }



                    //If timesheet entries is exist then loads current workitem in weektimesheet datatable
                    if (timesheetRows.Length > 0)
                    {
                        string subWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);
                        if (!string.IsNullOrWhiteSpace(subWorkItem) && subWorkItem.IndexOf(";#") != -1)
                        {
                            subWorkItem = Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]);
                          
                        }
                     
                        DataRow tRow = resultTable.NewRow();
                      
                        tRow["WorkItemID"] = workItem[DatabaseObjects.Columns.Id];
                        tRow["Type"] = ResourceWorkItemsManager.FormatWorkItemType(Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]));
                        tRow["WorkItem"] = workItem[DatabaseObjects.Columns.WorkItem];
                        tRow[DatabaseObjects.Columns.WorkItemLink] = workItem[DatabaseObjects.Columns.WorkItem];
                        if (!string.IsNullOrWhiteSpace(subWorkItem))
                        {
                            tRow["SubWorkItem"] = subWorkItem;
                            tRow["SubWorkItemLink"] = subWorkItem;
                        }

                        if (sptaskList != null)
                        {
                            tRow["EstimatedRemainingHours"] = sptaskList[DatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : sptaskList[DatabaseObjects.Columns.EstimatedRemainingHours];
                            tRow["EstimatedHours"] = sptaskList[DatabaseObjects.Columns.TaskEstimatedHours] == null ? 0 : sptaskList[DatabaseObjects.Columns.TaskEstimatedHours];
                            tRow["ActualHours"] = sptaskList[DatabaseObjects.Columns.TaskActualHours] == null ? 0 : sptaskList[DatabaseObjects.Columns.TaskActualHours];
                            List<HistoryEntry> commentHistoryList = uHelper.GetHistory(Convert.ToString(sptaskList[DatabaseObjects.Columns.UGITComment]), false);
                            if (commentHistoryList != null && commentHistoryList.Count > 0)
                            {
                                string comment = string.Format("{0} : {1}", commentHistoryList[0].entry, commentHistoryList[0].created);
                                tRow["UGITComment"] = comment;
                            }
                            else
                                tRow["UGITComment"] = "";

                        }
                        else if (spProjectList != null && Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]) != "NPR")
                        {
                            tRow["EstimatedRemainingHours"] = spProjectList[DatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : spProjectList[DatabaseObjects.Columns.EstimatedRemainingHours];
                            tRow["EstimatedHours"] = spProjectList[DatabaseObjects.Columns.EstimatedHours] == null ? 0 : spProjectList[DatabaseObjects.Columns.EstimatedHours];
                            tRow["ActualHours"] = spProjectList[DatabaseObjects.Columns.ActualHour] == null ? 0 : spProjectList[DatabaseObjects.Columns.ActualHour];
                            tRow["UGITComment"] = "";
                        }
                        else
                        {
                            tRow["EstimatedRemainingHours"] = 0;
                            tRow["EstimatedHours"] = 0;
                            tRow["ActualHours"] = 0;
                            tRow["UGITComment"] = "";
                        }

                        resultTable.Rows.Add(tRow);
                    }
                }
                else
                {
                   

                    //SPFieldLookupValue lookupVal = new SPFieldLookupValue(Convert.ToString(workItem[DatabaseObjects.Columns.SubWorkItem]));

                    //If allocation is exist then loads current workitem in weektimesheet datatable
                    DataRow tRow = resultTable.NewRow();
                    tRow["WorkItemID"] = workItem[DatabaseObjects.Columns.Id];
                    tRow["Type"] = ResourceWorkItemsManager.FormatWorkItemType(Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]));
                    tRow["WorkItem"] = workItem[DatabaseObjects.Columns.WorkItem];
                    tRow[DatabaseObjects.Columns.WorkItemLink] = workItem[DatabaseObjects.Columns.WorkItem];
                    //if (lookupVal.LookupId > 0)
                    //{
                    //    tRow["SubWorkItem"] = lookupVal.LookupValue;
                    //    tRow["SubWorkItemLink"] = lookupVal.LookupValue;
                    //}

                    if (sptaskList != null)
                    {
                        tRow["EstimatedRemainingHours"] = sptaskList[DatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : sptaskList[DatabaseObjects.Columns.EstimatedRemainingHours];
                        tRow["EstimatedHours"] = sptaskList[DatabaseObjects.Columns.TaskEstimatedHours] == null ? 0 : sptaskList[DatabaseObjects.Columns.TaskEstimatedHours];
                        tRow["ActualHours"] = sptaskList[DatabaseObjects.Columns.TaskActualHours] == null ? 0 : sptaskList[DatabaseObjects.Columns.TaskActualHours];
                        List<HistoryEntry> commentHistoryList = uHelper.GetHistory(Convert.ToString(sptaskList[DatabaseObjects.Columns.UGITComment]), false);
                        if (commentHistoryList != null && commentHistoryList.Count > 0)
                        {
                            string comment = string.Format("{0} : {1}", commentHistoryList[0].entry, commentHistoryList[0].created);
                            tRow["UGITComment"] = comment;
                        }
                        else
                            tRow["UGITComment"] = "";

                    }
                    else if (spProjectList != null && Convert.ToString(workItem[DatabaseObjects.Columns.WorkItemType]) != "NPR")
                    {
                        tRow["EstimatedRemainingHours"] = spProjectList[DatabaseObjects.Columns.EstimatedRemainingHours] == null ? 0 : spProjectList[DatabaseObjects.Columns.EstimatedRemainingHours];
                        tRow["EstimatedHours"] = spProjectList[DatabaseObjects.Columns.EstimatedHours] == null ? 0 : spProjectList[DatabaseObjects.Columns.EstimatedHours];
                        tRow["ActualHours"] = spProjectList[DatabaseObjects.Columns.ActualHour] == null ? 0 : spProjectList[DatabaseObjects.Columns.ActualHour];
                        tRow["UGITComment"] = "";
                    }
                    else
                    {
                        tRow["EstimatedRemainingHours"] = 0;
                        tRow["EstimatedHours"] = 0;
                        tRow["ActualHours"] = 0;
                        tRow["UGITComment"] = "";
                    }

                    resultTable.Rows.Add(tRow);
                    if (timesheetAllRows != null && timesheetAllRows.Length > 0)
                    {
                        timesheetRows = timesheetAllRows.Where(x => x.Field<long>(DatabaseObjects.Columns.ResourceWorkItemLookup).ToString() == string.Format("{0};#{0}", workItem[DatabaseObjects.Columns.Id])).ToArray();
                    }
                }
            }


            //Returns when not entry in resulttable
            if (resultTable == null || resultTable.Rows.Count <= 0)
            {
                return resultTable;
            }

            //Get Close stage id of NPR and PMM and TSK
            DataRow[] genericStages = new DataRow[0];
            DataTable gStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.StageType);
            if (gStages != null)
            {
                genericStages = gStages.Select();
            }

            string RMMLevel1PMMProjects = uHelper.GetModuleTitle("PMM");
            string RMMLevel1NPRProjects = uHelper.GetModuleTitle("NPR");
            string RMMLevel1TSKProjects = uHelper.GetModuleTitle("TSK");
           // $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' AND '{DatabaseObjects.Columns.ModuleNameLookup}' = 'NPR'";
            DataRow[] nprStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' AND '{DatabaseObjects.Columns.ModuleNameLookup}' = 'NPR'").Select();
            string nprClosedStageID = uHelper.GetModuleStageId(nprStages, genericStages, StageType.Closed);
            DataRow[] pmmStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' AND '{DatabaseObjects.Columns.ModuleNameLookup}' = 'PMM'").Select();
            string pmmClosedStageID = uHelper.GetModuleStageId(pmmStages, genericStages, StageType.Closed);
            DataRow[] tskStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' AND '{DatabaseObjects.Columns.ModuleNameLookup}' = 'NPR'").Select();
            string tskClosedStageID = uHelper.GetModuleStageId(pmmStages, genericStages, StageType.Closed);

            List<string> queryExps = new List<string>();
            List<string> tempAllocations = new List<string>();
            //DataRow nprModuleRow = null;
            DataTable nprData = null;

            //if NPR work item is exist then loads NPR tickets and NPR module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>("Type") == RMMLevel1NPRProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    queryExps.Add(string.Format("{0}={1}",DatabaseObjects.Columns.TicketId, tID));
                    //queryExps.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.TicketId, tID));
                }

                //nprModuleRow = moduleViewManager.LoadByName("NPR"); //uGITCache.GetModuleDetails("NPR");
                //SPSiteDataQuery nprQuery = new SPSiteDataQuery();
                //string nprQuery = "";

                //nprQuery.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>",
                //    DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleStepLookup, DatabaseObjects.Columns.TicketId);
                //nprQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(queryExps, queryExps.Count - 1, false));
                //Guid listID = uGITCache.GetListID(DatabaseObjects.Tables.NPRRequest);
                //nprQuery.Lists = string.Format("<Lists Hidden='True'><List ID='{0}'/></Lists>", listID);
                //nprData = SPContext.Current.Web.GetSiteData(nprQuery);

                var nprModuleRow = moduleViewManager.LoadByName("NPR");
                nprData = ticketManager.GetAllTickets(nprModuleRow);
                DataView dv = new DataView();
                string[] selectedColumns = { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleStepLookup, DatabaseObjects.Columns.TicketId };
                nprData = new DataView(nprData).ToTable(false, selectedColumns);

                queryExps = new List<string>();
            }

            //DataRow tskModuleRow = null;
            DataTable tskData = null;
            //if TSK work item is exist then loads TSK tickets and TSK module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>("Type") == RMMLevel1TSKProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    //queryExps.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.TicketId, tID));
                    queryExps.Add(string.Format("{0}={1}", DatabaseObjects.Columns.TicketId, tID));
                }

                //tskModuleRow = uGITCache.GetModuleDetails("TSK");
                //SPSiteDataQuery tskQuery = new SPSiteDataQuery();
                //tskQuery.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>",
                //    DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleStepLookup, DatabaseObjects.Columns.TicketId);
                //tskQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(queryExps, queryExps.Count - 1, false));
                //Guid listID = uGITCache.GetListID(DatabaseObjects.Lists.TSKProjects);
                //tskQuery.Lists = string.Format("<Lists Hidden='True'><List ID='{0}'/></Lists>", listID);
                //tskData = SPontext.Current.Web.GetSiteData(tskQuery);

                var tskModuleRow = moduleViewManager.LoadByName("TSK");
                nprData = ticketManager.GetAllTickets(tskModuleRow);

                queryExps = new List<string>();
            }

            DataTable pmmData = null;
            //if PMM work item is exist then loads PMM tickets and PMM module detail
            tempAllocations = resultTable.AsEnumerable().Where(x => x.Field<string>("Type") == RMMLevel1PMMProjects).Select(x => x.Field<string>(DatabaseObjects.Columns.WorkItem)).Distinct().ToList();
            if (tempAllocations.Count > 0)
            {
                foreach (string tID in tempAllocations)
                {
                    //queryExps.Add(string.Format("<Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq>", DatabaseObjects.Columns.TicketId, tID));
                    queryExps.Add(string.Format("{0}={1}", DatabaseObjects.Columns.TicketId, tID));
                }

                //pmmModuleRow = uGITCache.GetModuleDetails("PMM");
                //SPSiteDataQuery pmmQuery = new SPSiteDataQuery();
                //pmmQuery.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>",
                //   DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleStepLookup, DatabaseObjects.Columns.TicketId);
                //pmmQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(queryExps, queryExps.Count - 1, false));
                //Guid listID = uGITCache.GetListID(DatabaseObjects.Lists.PMMProjects);
                //pmmQuery.Lists = string.Format("<Lists Hidden='True'><List ID='{0}'/></Lists>", listID);
                //pmmData = SPContext.Current.Web.GetSiteData(pmmQuery);

                var pmmModuleRow = moduleViewManager.LoadByName("PMM");
                nprData = ticketManager.GetAllTickets(pmmModuleRow);


                queryExps = null;
            }

            //added new column.
            resultTable.Columns.Add("ActualVariance", typeof(double));
            resultTable.Columns.Add("Projected", typeof(double));
            resultTable.Columns.Add("ProjectedEstimate", typeof(double));

            resultTable.Columns["ActualVariance"].Expression = "ActualHours-EstimatedHours";
            resultTable.Columns["Projected"].Expression = "ActualHours+EstimatedRemainingHours";
            resultTable.Columns["ProjectedEstimate"].Expression = "Projected-EstimatedHours";

            //Returns if there is no npr , pmm, tsk ticket exist
            if (!((nprData != null && nprData.Rows.Count > 0) || (tskData != null && tskData.Rows.Count > 0) || (pmmData != null && pmmData.Rows.Count > 0)))
            {
                if (resultTable != null && resultTable.Rows.Count > 0)
                {
                    DataRow[] itemRows = resultTable.Select("EstimatedRemainingHours <> 0 AND ActualHours <> 0 AND EstimatedHours <> 0");
                    //resultTable.Clear();
                    if (itemRows != null && itemRows.Length > 0)
                        resultTable = itemRows.CopyToDataTable();
                    else
                        resultTable = null;
                }

                if ( dttempTable !=null && dttempTable.Rows.Count > 0)
                dttempTable.Clear();
                dttempTable = resultTable;

                return resultTable;
            }

            DataRow[] items = new DataRow[0];
            foreach (DataRow itemRow in resultTable.Rows)
            {
                items = new DataRow[0];

                if (Convert.ToString(itemRow["Type"]) == RMMLevel1NPRProjects && nprData != null && nprData.Rows.Count > 0)
                {
                    items = nprData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("{0}<br>{1}", itemRow[DatabaseObjects.Columns.WorkItem], itemRow[DatabaseObjects.Columns.Title]);
                    }
                }
                else if (Convert.ToString(itemRow["Type"]) == RMMLevel1PMMProjects && pmmData != null && pmmData.Rows.Count > 0)
                {
                    items = pmmData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                       itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("{0}<br>{1}", itemRow[DatabaseObjects.Columns.WorkItem], itemRow[DatabaseObjects.Columns.Title]);
                    }
                }
                else if (Convert.ToString(itemRow["Type"]) == RMMLevel1TSKProjects && tskData != null && tskData.Rows.Count > 0)
                {
                    items = tskData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, itemRow[DatabaseObjects.Columns.WorkItem]));
                    if (items.Length > 0)
                    {
                        itemRow[DatabaseObjects.Columns.Title] = Convert.ToString(items[0][DatabaseObjects.Columns.Title]);
                        itemRow[DatabaseObjects.Columns.WorkItemLink] = string.Format("{0}<br>{1}", itemRow[DatabaseObjects.Columns.WorkItem], itemRow[DatabaseObjects.Columns.Title]);
                    }
                }
            }

            if (resultTable != null && resultTable.Rows.Count > 0)
            {
                DataRow[] itemRows = resultTable.Select("EstimatedRemainingHours <> 0 AND ActualHours <> 0 AND EstimatedHours <> 0");
                //resultTable.Clear();
                if (itemRows != null && itemRows.Length > 0)
                    resultTable = itemRows.CopyToDataTable();
                else
                    resultTable = null;
            }
            if( dttempTable !=null && dttempTable.Rows.Count>0) 
            dttempTable.Clear();
            dttempTable = resultTable;

            return resultTable;
        }

        private static DataTable CreateTableSchema()
        {
            DataTable timeSheet = new DataTable();

            timeSheet.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            timeSheet.Columns.Add("WorkItemID", typeof(int));
            timeSheet.Columns.Add("Type", typeof(string));
            timeSheet.Columns.Add("WorkItem", typeof(string));
            timeSheet.Columns.Add("SubWorkItem", typeof(string));
            timeSheet.Columns.Add("WorkItemPublicId", typeof(string));
            timeSheet.Columns.Add("WorkItemLink", typeof(string));
            timeSheet.Columns.Add("EstimatedRemainingHours", typeof(double));
            timeSheet.Columns.Add("SubWorkItemLink", typeof(string));
            timeSheet.Columns.Add("EstimatedHours", typeof(double));
            timeSheet.Columns.Add("ActualHours", typeof(double));
            timeSheet.Columns.Add("UGITComment", typeof(string));
            return timeSheet;
        }
        #endregion

        protected void gvEstimatedRemaningHoursReport_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            foreach (GridViewDataColumn c in grid.Columns)
            {
                if (dttempTable !=null && dttempTable.Rows.Count > 0)
                {
                    DataRow[] rows = dttempTable.Select("SubWorkItemLink <> ''");
                    if ((rows == null || rows.Length == 0) && (c.FieldName.ToString()) == "SubWorkItemLink")
                    {
                            c.Visible = false;
                    }
                }
            }
        }
    }
}
