using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
//using uGovernIT.Manager.Reports;
using uGovernIT.Utility;
using uGovernIT.Manager.Helper;
using uGovernIT.Manager.Reports;


namespace uGovernIT.Report.DxReport.ProjectPorFolioReport
{
    public partial class ProjectPorFolioReport_Viewer : UserControl
    {
        public string TicketId { get; set; }
        public string documnetPickerUrl { get; set; }
        public string DocName { get; set; }
        public string FolderGuid { get; set; }
        public string stageName { get; set; }
        public string SelectFolder { get; set; }
        public string PathValue { get; set; }
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {

            bool isPercentage = false;
            isPercentage = UGITUtility.StringToBoolean(Request["isPercentage"]);
            string yearType = Convert.ToString(Request["yeartype"]);
            int year = UGITUtility.StringToInt(Request["year"]);
            List<Constants.ProjectType> type = new List<Constants.ProjectType>();
            type.Add(Constants.ProjectType.All);

            DataTable budgetData = ITG.Portfolio.LoadAll(_context, type);
            List<string> ticketIds = UGITUtility.ConvertStringToList(Request["ticketids"], ",");

            if (budgetData == null || ticketIds == null || ticketIds.Count == 0)
                return;


            DataRow[] selectedRows = (from a in budgetData.AsEnumerable()
                                      join
b in ticketIds on a.Field<string>(DatabaseObjects.Columns.TicketId) equals b
                                      select a).ToArray();

            if (selectedRows.Length == 0)
                return;

            budgetData = selectedRows.CopyToDataTable();

            ITG.Portfolio.BindResourceAllocationData(_context, budgetData, yearType, Request["allocationtype"], year, UGITUtility.StringToBoolean(Request["isApprovedProjectRequests"]), UGITUtility.StringToBoolean(Request["isPendingApproval"]));

            if (budgetData != null)
            {

                budgetData.Columns.Add("Resources", typeof(String));
                foreach (DataRow row in budgetData.Rows)
                {
                    DataTable dtfinal = new DataTable();
                    string assignTo = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.ModuleName])) && !string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.TicketId])) && !string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.TicketActualStartDate])) && !string.IsNullOrEmpty(Convert.ToString(row[DatabaseObjects.Columns.TicketActualCompletionDate])))
                    {
                        DataTable dtResources = ITG.Portfolio.GetAllocationData(_context, Convert.ToString(row[DatabaseObjects.Columns.ModuleName]), Convert.ToString(row[DatabaseObjects.Columns.TicketId]), Convert.ToDateTime(row[DatabaseObjects.Columns.TicketActualStartDate]), Convert.ToDateTime(row[DatabaseObjects.Columns.TicketActualCompletionDate]));
                        if (dtResources != null && dtResources.Rows.Count > 0)
                        {
                            DataView dv = dtResources.DefaultView;
                            dv.Sort = DatabaseObjects.Columns.PctPlannedAllocation + " DESC";
                            DataTable sortedResourceDT = dv.ToTable();
                            dtfinal = sortedResourceDT.AsEnumerable().Take(5).CopyToDataTable();
                            foreach (DataRow rowfinal in dtfinal.Rows)
                            {
                                if (!string.IsNullOrEmpty(assignTo))
                                    assignTo = assignTo + "; ";
                                assignTo += rowfinal[DatabaseObjects.Columns.Resource];
                            }
                        }
                    }
                    row["Resources"] = assignTo;
                }
            }

            RptVwrProjectReport.AutoSize = false;
            ProjectPorfolioReport report = new ProjectPorfolioReport(budgetData, year, yearType, isPercentage);
            RptVwrProjectReport.Report = report;


            ///Document Folder picker related code.
            //var spItemCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Tables.PMMProjects, _context);
           DataTable spItemCollection= GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            DataRow spListItem = null;
            if (spItemCollection != null && spItemCollection.Rows.Count > 0)
            {
                //spListItem = spItemCollection[0];
                spListItem = spItemCollection.Rows[0];
            }
            if (spListItem != null)
            {
                TicketId = Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]);
                DocName = TicketId.Replace("-", "_");
            }

            //Ticket ticket = new Ticket(_context, "PMM");
            //var stage = ticket.GetTicketCurrentStage(spListItem);
            //stageName = stage.Name;
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

        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("Project_Portfolio_Report_{0}_{1}", Request["year"], uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                RptVwrProjectReport.Report.ExportToPdf(path);

                e.Result = UGITUtility.GetAbsoluteURL("_layouts/15/ugovernit/DelegateControl.aspx?control=ticketemail&type=projectsummaryreport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string typeparam = "multiprojectreport";
                string fileName = string.Format("Project_Portfolio_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&type=" + typeparam + "&localpath=" + path + "&relativepath=" + uploadFileURL + "&DocName=" + DocName + "&folderid=" + FolderGuid);
            }

        }
    }
}