
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager.Reports;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
namespace uGovernIT.DxReport
{
    public partial class ProjectReport_Viewer : UserControl
    {
        public int[] PMMIds { get; set; }
        public int projectYear { get; set; }

        public bool IsComponentCall { get; set; }
        public bool ShowProjectName { get; set; }
        public bool ShowPriority { get; set; }
        public bool ShowProStatus { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowTargetDate { get; set; }
        public bool ShowProjectManagers { get; set; }
        public bool ShowProgress { get; set; }
        public bool ShowProjectType { get; set; }
        public bool ShowPercentComplete { get; set; }
        public bool ShowLatestOnly { get; set; }
        public bool ShowPlainText { get; set; }

        public bool ShowAccomplishment { get; set; }
        public bool ShowPlan { get; set; }
        public bool ShowIssues { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowProjectDescription { get; set; }
        public bool ShowProjectRoles { get; set; }
        public bool ShowMonitorState { get; set; }

        public string TicketId { get; set; }
        public string documnetPickerUrl { get; set; }
        public string DocName { get; set; }
        public string FolderGuid { get; set; }
        public string stageName { get; set; }
        public string SelectFolder { get; set; }
        public string PathValue { get; set; }
        public string Module { get; set; }
        private PMMProjectReportHelper proRepHelper;
        TSKProjectReportEntity prEntity;
        public string ShowSortOrder { get; set; }
        public bool ShowRisks { get; set; }
        public bool ShowTrafficlight { get; set; }
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
            ShowProjectName = UGITUtility.StringToBoolean(Request["ProjectName"]);
            ShowPriority = UGITUtility.StringToBoolean(Request["Priority"]);
            ShowStatus = UGITUtility.StringToBoolean(Request["Status"]);
            ShowProStatus = UGITUtility.StringToBoolean(Request["ProStatus"]);
            ShowDescription = UGITUtility.StringToBoolean(Request["Description"]);
            ShowTargetDate = UGITUtility.StringToBoolean(Request["TargetDate"]);
            ShowProjectManagers = UGITUtility.StringToBoolean(Request["ProjectManager"]);
            ShowProgress = UGITUtility.StringToBoolean(Request["Progress"]);
            ShowProjectType = UGITUtility.StringToBoolean(Request["ProjectType"]);
            ShowPercentComplete = UGITUtility.StringToBoolean(Request["PercentageComp"]);
            ShowLatestOnly = UGITUtility.StringToBoolean(Request["LatestOnly"]);
            ShowPlainText = UGITUtility.StringToBoolean(Request["PlainText"]);
            ShowAccomplishment = UGITUtility.StringToBoolean(Request["Acc"]);
            ShowPlan = UGITUtility.StringToBoolean(Request["Plan"]);
            ShowIssues = UGITUtility.StringToBoolean(Request["Issues"]);
            // ShowProjectDescription= UGITUtility.StringToBoolean(Request["Issues"]);
            // ShowProjectRoles = UGITUtility.StringToBoolean(Request[""]);
            ShowMonitorState = UGITUtility.StringToBoolean(Request["Monitors"]);
            ShowSortOrder = Request["SortOrder"];
            ShowRisks = UGITUtility.StringToBoolean(Request["Risk"]);
            ShowTrafficlight = UGITUtility.StringToBoolean(Request["Trafficlight"]);
            Module = Request["Module"];
            string selectedProjectStatus = Request["SelectedProjectStatus"];


            if (Request["PMMIds"] != null)
            {
                if (!string.IsNullOrEmpty(Request["PMMIds"]))
                {
                    PMMIds = Array.ConvertAll<string, int>(Request["PMMIds"].Split(','), int.Parse);
                }
            }
            List<string> paramRequired = new List<string>();
            paramRequired.Add("ProjectName");
            paramRequired.Add("Priority");
            paramRequired.Add("Status");
            paramRequired.Add("Description");
            paramRequired.Add("TargetDate");
            paramRequired.Add("ProjectManager");
            paramRequired.Add("Progress");
            paramRequired.Add("ProjectType");
            paramRequired.Add("PercentageComp");
            paramRequired.Add("LatestOnly");
            paramRequired.Add("PlainText");
            paramRequired.Add("Acc");
            paramRequired.Add("Plan");
            paramRequired.Add("ProStatus");
            paramRequired.Add("Issues");
            paramRequired.Add("Risk");
            paramRequired.Add("Monitors");
            paramRequired.Add("ProjectStatus");
            paramRequired.Add("AccDesc");
            paramRequired.Add("PlanDesc");
            paramRequired.Add("IssDesc");
            paramRequired.Add("RiskDesc");
            paramRequired.Add("PMMIds");
            paramRequired.Add("SortOrder");
            paramRequired.Add("Trafficlight");

            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = delegateControl + "?reportName=ProjectReport&Filter=Filter&Module=" + Module + "&alltickets=" + UGITUtility.ObjectToString(Request["alltickets"]);
                Response.Redirect(ReportFilterURl);
            }
            hdnconfiguration.Set("RequestUrl", Request.Url.AbsolutePath);

            // DataRow module = uGITCache.GetModuleDetails("PMM");

            prEntity = new TSKProjectReportEntity();
            prEntity.ShowProjectName = ShowProjectName;
            prEntity.ShowPriority = ShowPriority;
            prEntity.ShowStatus = ShowStatus;
            prEntity.ShowProStatus = ShowProStatus;
            prEntity.ShowDescription = ShowDescription;
            prEntity.ShowTargetDate = ShowTargetDate;
            prEntity.ShowProjectManagers = ShowProjectManagers;
            prEntity.ShowProgress = ShowProgress;
            prEntity.ShowProjectType = ShowProjectType;
            prEntity.ShowPercentComplete = ShowPercentComplete;
            prEntity.ShowLatestOnly = ShowLatestOnly;
            prEntity.ShowPlainText = ShowPlainText;

            prEntity.ShowAccomplishment = ShowAccomplishment;
            prEntity.ShowPlan = ShowPlan;
            prEntity.ShowIssues = ShowIssues;
            prEntity.ShowProjectDescription = ShowProjectDescription;
            prEntity.ShowProjectRoles = ShowProjectRoles;
            prEntity.ShowMonitorState = ShowMonitorState;
            prEntity.IsPMM = true;
            prEntity.SortOrder = ShowSortOrder;
            prEntity.ShowRisk = ShowRisks;
            prEntity.ShowTrafficlight = ShowTrafficlight;
            proRepHelper = new PMMProjectReportHelper();
            //proRepHelper.SummaryReport = true;
            prEntity = proRepHelper.GetProjectReportEntity(_context, prEntity, PMMIds, Module, selectedProjectStatus);

            //ProjectSummaryReport proSummaryReport = new ProjectSummaryReport(prEntity);
            if (prEntity.Projects != null)
            {
                foreach (DataRow item in prEntity.Projects.Rows)
                {
                    item.SetField(prEntity.Projects.Columns[DatabaseObjects.Columns.TicketProjectManager], UGITUtility.RemoveIDsFromLookupString(Convert.ToString(item[DatabaseObjects.Columns.TicketProjectManager])));
                }
            }
            XtraReport r = new XtraReport();
            DynamicReportBuilderHelper rbh = new DynamicReportBuilderHelper();
            rbh.GenerateReport(r, prEntity);
            RptVwrProjectReport.Report = r;
            //ASPxSplitter1.Panes[0].Collapsed = true;
            pnlReport.Visible = true;

            string projectTable = DatabaseObjects.Tables.PMMProjects;
            if(!string.IsNullOrEmpty(Module))
            {
                ModuleViewManager modulemanager = new ModuleViewManager(_context);
                projectTable = modulemanager.GetModuleTableName(Module);
            }

            ///Document Folder picker related code.
            DataTable spItemCollection = GetTableDataManager.GetTableData(projectTable, $"TenantID='{_context.TenantID}'");
            DataRow spListItem = null;
            if (spItemCollection != null && spItemCollection.Rows.Count > 0)
            {
                spListItem = spItemCollection.Rows[0];
            }
            if (spListItem != null)
            {
                TicketId = Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]);
                DocName = TicketId.Replace("-", "_");
            }

            Ticket ticket = new Ticket(_context, Module);
            var stage = ticket.GetTicketCurrentStage(spListItem);
            if (stage != null)
            {
                stageName = stage.Name;
            }
            //TreeViewEDM eDMTreeView = DMCache.LoadPortalTreeView(DocName);
            //if (eDMTreeView != null && eDMTreeView.FolderList != null && eDMTreeView.FolderList.Count > 0)
            //{
            //    FolderList folder = eDMTreeView.FolderList.FirstOrDefault(c => c.Name == string.Format("{0}-{1}", stage.ID, stageName));
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
                string fileName = string.Format("Project_Summary_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                RptVwrProjectReport.Report.ExportToPdf(path);

                e.Result = UGITUtility.GetAbsoluteURL("layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=projectsummaryreport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string typeparam = "multiprojectreport";
                string fileName = string.Format("Project_Summary_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&type=" + typeparam + "&localpath=" + path + "&relativepath=" + uploadFileURL + "&DocName=" + DocName + "&folderid=" + FolderGuid);
            }

        }
    }
}
