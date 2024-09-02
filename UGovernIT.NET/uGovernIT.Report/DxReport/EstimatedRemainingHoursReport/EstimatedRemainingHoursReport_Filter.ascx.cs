using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;

namespace uGovernIT.Report.DxReport
{
    public partial class EstimatedRemainingHoursReport_Filter : UserControl
    {
        public string TicketPublicId { get; set; }
        public int TicketId { get; set; }
        protected StringBuilder urlBuilder = new StringBuilder();
        protected bool printReport = false;
        protected string editTaskFormUrl = string.Empty;
        protected string sprintTaskUrl = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UGITTaskManager taskManager = null;
        ReportGenerationHelper reportHelper = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["alltickets"] != null)
            {
                List<string> lstOfpublicids = Request["alltickets"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                if (lstOfpublicids.Count > 0)
                    TicketPublicId = lstOfpublicids.FirstOrDefault();
            }
            reportHelper = new ReportGenerationHelper();
            taskManager = new UGITTaskManager(context);
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/_layouts/15/uGovernIT/edittask.aspx");
            sprintTaskUrl = UGITUtility.GetAbsoluteURL("/_layouts/15/uGovernIT/ProjectManagement.aspx?control=pmmsprints");

            gvEstimatedRemaningHoursReport.DataSource = GetProjectTasks();
             gvEstimatedRemaningHoursReport.DataBind();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(System.Web.HttpContext.Current, false);
        }

        public DataTable GetProjectTasks()
        {
            DataTable taskTable = new DataTable();
            taskTable = taskManager.LoadTasksTable(ModuleNames.PMM,true,TicketPublicId);

            //added new column.
            
            taskTable.Columns.Add("ActualVariance", typeof(double));
            taskTable.Columns.Add("Projected", typeof(double));
            taskTable.Columns.Add("ProjectedEstimate", typeof(double));
            taskTable.Columns.Add("PMMComment", typeof(string));

            taskTable.Columns["ActualVariance"].Expression = "Convert(ActualHours,System.Int64) -Convert(EstimatedHours,System.Int64)";
            taskTable.Columns["Projected"].Expression = "Convert(ActualHours,System.Int64)+Convert(EstimatedRemainingHours,System.Int64)";
            taskTable.Columns["ProjectedEstimate"].Expression = "Convert(Projected,System.Int64)-Convert(EstimatedHours,System.Int64)";

           
            taskTable.AsEnumerable().
                Where(X => !string.IsNullOrEmpty(X.Field<string>(DatabaseObjects.Columns.Comment))).ToList().
                ForEach(x =>
                {
                    List<HistoryEntry> commentHistoryList = uHelper.GetHistory(x.Field<string>(DatabaseObjects.Columns.UGITComment), false);
                    string comment = string.Format("{0} : {1}", commentHistoryList[0].entry, commentHistoryList[0].created);
                    x.SetField<string>("PMMComment", comment);
                });
         
            return taskTable;

        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            ERHProjectTasks report = new ERHProjectTasks(GetProjectTasks());
            string title = lblReportTitle.Text;
           ReportGenerationHelper.WriteXlsToResponse(Response, title + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString(), report);
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ERHProjectTasks report = new ERHProjectTasks(GetProjectTasks());
            string title = lblReportTitle.Text;
            ReportGenerationHelper.WritePdfToResponse(Response, title + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString(), report);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ERHProjectTasks report = new ERHProjectTasks(GetProjectTasks());
            string title = lblReportTitle.Text;
            ReportGenerationHelper.WritePdfToResponse(Response, title + ".pdf", System.Net.Mime.DispositionTypeNames.Inline.ToString(), report);
        }

        protected void gvEstimatedRemaningHoursReport_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                
                int titleIndex = gvEstimatedRemaningHoursReport.Columns["Title"].Index;
                int level = Convert.ToInt32(gvEstimatedRemaningHoursReport.GetRowValues(e.VisibleIndex, DatabaseObjects.Columns.UGITLevel));
                if (level != 0 && e.Row.Cells.Count > 1)
                {
                        e.Row.Cells[titleIndex].Style.Add("padding", string.Format("0 0 0 {0}px", 25 * level));
                }

                DataRow currentRow = gvEstimatedRemaningHoursReport.GetDataRow(e.VisibleIndex);

                if (currentRow != null)
                {
                    e.Row.Attributes.Add("level", Convert.ToString(currentRow[DatabaseObjects.Columns.UGITLevel]));
                    e.Row.Attributes.Add("childcount", Convert.ToString(currentRow[DatabaseObjects.Columns.UGITChildCount]));
                    e.Row.Attributes.Add("parenttask", Convert.ToString(currentRow[DatabaseObjects.Columns.ParentTask]));
                    e.Row.Attributes.Add("task", Convert.ToString(currentRow[DatabaseObjects.Columns.Id]));
                }
            }
        }

        protected string GetTitleHtmlData()
        {
            StringBuilder data = new StringBuilder();

            int childCount = UGITUtility.StringToInt(Eval(DatabaseObjects.Columns.UGITChildCount));
            bool isMileStone = UGITUtility.StringToBoolean(Eval(DatabaseObjects.Columns.IsMilestone));
            string taskBehaviour = Convert.ToString(Eval(DatabaseObjects.Columns.TaskBehaviour));

            data.Append("<div class=\"task-title\" style=\"float:left;");
           // data.Append("<div style=\"float:left;");
            if (childCount > 0)
                data.Append("font-weight:bold;");
            data.Append("\" >");

            string cssChanges = "color:#000;";
            if (childCount > 0)
            {
                data.AppendFormat("<img style='float:left;padding-right:2px;' src='/_layouts/15/images/uGovernIT/minimise.gif' colexpand='true' onclick=\"event.cancelBubble=true; CloseChildren('{0}' , '{1}', this)\" />", Eval(DatabaseObjects.Columns.ItemOrder), Eval("ID"));
            }
            if (Eval("Title") != null)
            {
                string[] arr = Convert.ToString(Eval("Title")).Replace('(', ' ').Replace(')', ' ').Split(new string[] { "Sprint:" }, StringSplitOptions.RemoveEmptyEntries);
                data.AppendFormat("<a href='javascript:void(0);' onclick='editTask({1},{3})'><span style='padding-top:1px;{2}'>{0}</span></a> ", arr[0].Trim(), Eval("ID"), cssChanges, Eval("ItemOrder"));
                if (arr.Length > 1)
                {
                    data.AppendFormat("<a href='javascript:void(0);' onclick='openSprintTask(\"{0}\")'><span style='padding-top:1px;{1}'>(Sprint:{0})</span></a>", arr[1].Trim(), cssChanges);
                }
            }
          
            data.Append("</div>");

            return data.ToString();
        }

        protected override void OnPreRender(EventArgs e)
        {
            urlBuilder.Append(UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery));
            gvEstimatedRemaningHoursReport.DataBind();
            base.OnPreRender(e);
        }
    }
}
