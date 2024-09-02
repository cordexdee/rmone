
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager.Entities;
using uGovernIT.Manager.Reports;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ReportEntity = uGovernIT.Manager.Entities;
using DevExpress.XtraReports.UI;
using uGovernIT.Manager;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Web;
using uGovernIT.DxReport;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;

namespace uGovernIT.DxReport
{
    public partial class ProjectStatusReport_Viewer : UserControl
    {
        public int[] PMMIds { get; set; }
        public int projectYear { get; set; }

        public bool IsComponentCall { get; set; }
        public bool ShowAccomplishment { get; set; }
        public bool ShowPlan { get; set; }
        public bool ShowIssues { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowSummaryGanttChart { get; set; }
        public bool ShowAllTask { get; set; }
        public bool ShowOpenTaskOnly { get; set; }
        public bool ShowMilestone { get; set; }
        public bool ShowDeliverable { get; set; }
        public bool ShowReceivable { get; set; }
        public bool CalculateExpected { get; set; }
        public bool ShowProjectDescription { get; set; }
        public bool ShowBudgetSummary { get; set; }
        public bool ShowPlannedvsActualByCategory { get; set; }
        public bool ShowPlannedvsActualByBudgetItem { get; set; }
        public bool ShowPlannedvsActualByMonth { get; set; }
        public bool ShowProjectRoles { get; set; }
        public bool ShowResourceAllocation { get; set; }
        public bool ShowMonitorState { get; set; }
        public string TicketId { get; set; }
        public string documnetPickerUrl { get; set; }
        public string DocName { get; set; }
        public string FolderGuid { get; set; }
        public string stageName { get; set; }
        public string SelectFolder { get; set; }
        public string PathValue { get; set; }

        private ProjectReportHelper proRepHelper;
        TSKProjectReportEntity prEntity;
        public bool ShowRisks { get; set; }
        public string ShowSortOrder { get; set; }
        public bool ShowTrafficlight { get; set; }
        public bool ShowAllMilestone { get; set; }
        public bool ShowDecisionLog { get; set; }
        public string Module { get; set; }
        ApplicationContext _context = System.Web.HttpContext.Current.GetManagerContext();
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        public string ReportFilterURl;
        protected override void OnLoad(EventArgs e)
        {
            string eventArgument = Request.Params["__EVENTARGUMENT"];
            if (eventArgument == "saveToWindow=format:xls;" || eventArgument == "saveToDisk=format:xls;"
             || eventArgument == "saveToWindow=format:xlsx;" || eventArgument == "saveToDisk=format:xlsx;"
             || eventArgument == "saveToWindow=format:png;" || eventArgument == "saveToDisk=format:png;")
            {
                if (proRepHelper.sourcetable != null && proRepHelper.sourcetable.Rows.Count == 1)
                {
                    RptVwrProjectReport.Report.Bands[BandKind.PageHeader].Visible = false;
                }
            }
            base.OnLoad(e);
        }

        protected override void OnInit(EventArgs e)
        {

            Module = Request["Module"];
            List<string> paramRequired = new List<string>();
            paramRequired.Add("PMMIds");
            paramRequired.Add("projectYear");
            paramRequired.Add("Acc");
            paramRequired.Add("Plan");
            paramRequired.Add("Status");
            paramRequired.Add("Risk");
            paramRequired.Add("Issues");
            paramRequired.Add("DecisionLog");
            paramRequired.Add("SGC");
            paramRequired.Add("SAT");
            paramRequired.Add("SMS");
            paramRequired.Add("SKD");
            paramRequired.Add("SKR");
            paramRequired.Add("CalcExpected");
            paramRequired.Add("ProjectDesc");
            paramRequired.Add("BudgetSummary");
            paramRequired.Add("PlannedvsActualByCategory");
            paramRequired.Add("PlannedvsActualByBudgetItem");
            paramRequired.Add("PlannedvsActualByMonth");
            paramRequired.Add("ProjectRoles");
            paramRequired.Add("ResourceAllocation");
            paramRequired.Add("ProjectStatus");
            paramRequired.Add("SortOrder");
            paramRequired.Add("Trafficlight");
            paramRequired.Add("AllMilestone");
            paramRequired.Add("SOTO");
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = delegateControl + "?reportName=ProjectStatusReport&Filter=Filter&Module=" + Module + "&alltickets=" + UGITUtility.ObjectToString(Request["alltickets"]);
                Response.Redirect(ReportFilterURl);
            }
            hdnconfiguration.Set("RequestUrl", Request.Url.AbsolutePath);

            //    DataRow module = uGITCache.GetModuleDetails("PMM");

            prEntity = new TSKProjectReportEntity();
            prEntity.ShowAccomplishment = UGITUtility.StringToBoolean(Request["Acc"]);
            prEntity.ShowPlan = UGITUtility.StringToBoolean(Request["Plan"]);
            prEntity.ShowIssues = UGITUtility.StringToBoolean(Request["Issues"]);
            prEntity.ShowDecisionLog = UGITUtility.StringToBoolean(Request["DecisionLog"]);
            prEntity.ShowStatus = UGITUtility.StringToBoolean(Request["Status"]);
            prEntity.ShowSummaryGanttChart = UGITUtility.StringToBoolean(Request["SGC"]);
            prEntity.ShowAllTask = UGITUtility.StringToBoolean(Request["SAT"]);
            prEntity.ShowOpenTaskOnly = UGITUtility.StringToBoolean(Request["SOTO"]);
            prEntity.ShowMilestone = UGITUtility.StringToBoolean(Request["SMS"]);
            prEntity.ShowDeliverable = UGITUtility.StringToBoolean(Request["SKD"]);
            prEntity.ShowReceivable = UGITUtility.StringToBoolean(Request["SKR"]);
            prEntity.CalculateExpected = UGITUtility.StringToBoolean(Request["CalcExpected"]);
            prEntity.ShowProjectDescription = UGITUtility.StringToBoolean(Request["ProjectDesc"]);
            prEntity.ShowBudgetSummary = UGITUtility.StringToBoolean(Request["BudgetSummary"]);
            prEntity.ShowPlannedvsActualByCategory = UGITUtility.StringToBoolean(Request["PlannedvsActualByCategory"]);
            prEntity.ShowPlannedvsActualByBudgetItem = UGITUtility.StringToBoolean(Request["PlannedvsActualByBudgetItem"]);
            prEntity.ShowPlannedvsActualByMonth = UGITUtility.StringToBoolean(Request["PlannedvsActualByMonth"]);
            prEntity.ShowProjectRoles = UGITUtility.StringToBoolean(Request["ProjectRoles"]);
            prEntity.ShowResourceAllocation = UGITUtility.StringToBoolean(Request["ResourceAllocation"]);
            prEntity.ShowMonitorState = UGITUtility.StringToBoolean(Request["Status"]);
            prEntity.IsPMM = true;
            prEntity.ShowRisk = UGITUtility.StringToBoolean(Request["Risk"]);
            prEntity.SortOrder = Request["SortOrder"];
            prEntity.ShowTrafficlight = UGITUtility.StringToBoolean(Request["Trafficlight"]);
            prEntity.ShowAllMilestone = UGITUtility.StringToBoolean(Request["AllMilestone"]);
            proRepHelper = new ProjectReportHelper();
            if (Request["PMMIds"] != null)
            {
                if (!string.IsNullOrEmpty(Request["PMMIds"]))
                {
                    PMMIds = Array.ConvertAll<string, int>(Request["PMMIds"].Split(','), int.Parse);
                }
            }
            string ticketStatus = Request["ProjectStatus"];
            prEntity = proRepHelper.GetProjectReportEntity(_context, prEntity, PMMIds, ticketStatus);
            if (prEntity.Projects.Rows.Count > 1)
            {
                ASPxSplitter1.Panes[0].Collapsed = false;
                MultiProReport proReport = new MultiProReport(prEntity);
                RptVwrProjectReport.Report = proReport;
                //RptToolBar.Items.RemoveAt(20);
                //RptToolBar.Items.RemoveAt(19);
            }
            else
            {
                ASPxSplitter1.Panes[0].Collapsed = true;
                ProjectReport proReport = new ProjectReport(prEntity);
                RptVwrProjectReport.Report = proReport;

               
            }



            pnlReport.Visible = true;

            ///Document Folder picker related code.
            //DataTable dataTable = null;
            //TicketManager ticketManager = new TicketManager(_context);
            //ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
            //UGITModule module = moduleViewManager.LoadByName(ModuleNames.PMM, true);
            //if (ticketStatus == "Open")
            //{
            //    dataTable = ticketManager.GetOpenTickets(module);
            //}
            //else if (ticketStatus == "Closed")
            //{
            //    dataTable = ticketManager.GetClosedTickets(module);
            //}
            //else
            //    dataTable = ticketManager.GetAllTickets(module);

            ////DataTable dataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            //DataRow spListItem = null;
            //if (dataTable != null && dataTable.Rows.Count > 0)
            //{
            //    spListItem = dataTable.Rows[0];
            //    TicketId = Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]);
            //    DocName = TicketId.Replace("-", "_");
            //}

            //Ticket ticket = new Ticket(_context, "PMM");
            //var stage = ticket.GetTicketCurrentStage(spListItem);
            //stageName = stage.Name;
            // TreeViewEDM eDMTreeView = DMCache.LoadPortalTreeView(DocName);
            //if (eDMTreeView != null && eDMTreeView.FolderList != null && eDMTreeView.FolderList.Count > 0)
            //{
            //    FolderList folder = eDMTreeView.FolderList.FirstOrDefault(c => c.Name == stageName);
            //    if (folder != null)
            //    {
            //        FolderGuid = folder.folderGuid;
            //        CurrentUserSelectionInfo currentSelection = new CurrentUserSelectionInfo(DocName, FolderGuid, EDMViewType.ByFolder, false);
            //        if (currentSelection != null)
            //        {
            //            PathValue = currentSelection.pathValue;
            //            SelectFolder = currentSelection.selectedFolder;
            //        }
            //    }
            //}


            base.OnInit(e);
        }

        //private bool CheckIfPortalExists(string TicketId)
        //{
        //    SPWeb oWeb = SPContext.Current.Web;
        //    string docName = TicketId.Replace("-", "_");

        //    if (oWeb.Lists.TryGetList(docName.Trim()) == null)
        //        return false;
        //    else
        //        return true; 
        //}

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("Project_Status_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                RptVwrProjectReport.Report.ExportToPdf(path);

                e.Result = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=projectreport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string typeparam = (PMMIds.Length > 0 && PMMIds.Length > 1) ? "multiprojectreport" : "projectreport";
                string fileName = string.Format("Project_Status_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&type=" + typeparam + "&localpath=" + path + "&relativepath=" + uploadFileURL + "&DocName=" + DocName);
            }

        }
    }
}
