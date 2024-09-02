using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Web;
using System.Drawing;
using DevExpress.Web.Rendering;
using DevExpress.Utils;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.XtraReports.UI;
using uGovernIT.Report.Helpers;

namespace uGovernIT.Report.DxReport
{
    public partial class TaskSummary : System.Web.UI.Page
    {
        private bool isGetFilteredDataDone;
        private DataTable resultedTable;
        public string ajaxPageURL;
        public bool enablePrint;
        private string moduleName = string.Empty;
        int[] Ids = null;
        TicketStatus tstatus { get; set; }
        string docPath = string.Empty;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected override void OnInit(EventArgs e)
        {
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/AjaxHelper.aspx");
            if (Request["enablePrint"] != null)
            {
                enablePrint = true;
            }

            if (Request["moduleName"] != null)
            {
                moduleName = Request["moduleName"].Trim().ToUpper();
            }

            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }

        protected sealed override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (Context.Request.Form["__EVENTARGUMENT"] != null &&
                 Context.Request.Form["__EVENTARGUMENT"].EndsWith("__ClearFilter__"))
            {
                // Clear FilterExpression
                ViewState.Remove(Constants.FilterExpression);
            }
        }

        public DataTable getDataSource()
        {
            if (isGetFilteredDataDone)
            {
                return resultedTable;
            }
            else
            {
                return GetConfigTables();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            // get rowcount and  bind gridview to show data
            GenerateColumns();
            gridTaskSummary.DataBind();
            TaskSummary_Report taskSummaryreport = new TaskSummary_Report(getDataSource());
           
            base.OnPreRender(e);
        }

        /// <summary>
        /// Generates resulted table and bind table column with gridview
        /// </summary>
        /// <returns></returns>
        private DataTable GetConfigTables()
        {
            isGetFilteredDataDone = true;

            if (Request["Ids"] != null)
            {
                if (!string.IsNullOrEmpty(Request["Ids"]))
                {
                    Ids = Array.ConvertAll<string, int>(Request["Ids"].Split(','), int.Parse);
                }
            }
            
            if (Request["ProjectStatus"] != null)
            {
                tstatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Request["ProjectStatus"]);
            }
            TaskSummary_Scheduler obj = new TaskSummary_Scheduler();
            resultedTable = obj.GetProjectsTasksTable(_context,Ids, tstatus, moduleName);

            return resultedTable;
        }

        void AddColumn(string fieldName, string caption, HorizontalAlign horizontalAlign, int width, string format)
        {
            AddColumn(fieldName, caption, horizontalAlign, width, format, false, DefaultBoolean.True);
        }

        void AddColumn(string fieldName, string caption, HorizontalAlign horizontalAlign, int width, string format, DefaultBoolean allowHeaderFilter)
        {
            AddColumn(fieldName, caption, horizontalAlign, width, format, false, allowHeaderFilter);
        }

        void AddColumn(string fieldName, string caption, HorizontalAlign horizontalAlign, int width, string format, bool isGroupField, DefaultBoolean allowHeaderFilter)
        {
            GridViewDataTextColumn column = new GridViewDataTextColumn();

            column = new GridViewDataTextColumn();
            column.FieldName = fieldName;
            column.Caption = caption;
            column.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
            column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            column.HeaderStyle.HorizontalAlign = horizontalAlign;
            column.CellStyle.HorizontalAlign = horizontalAlign;
            column.PropertiesEdit.DisplayFormatString = format;
            if (width > 0)
            {
                column.Width = new Unit(width);
            }
            column.Settings.AllowHeaderFilter = allowHeaderFilter;
            gridTaskSummary.Columns.Add(column);
            if (isGroupField)
            {
                gridTaskSummary.GroupBy(gridTaskSummary.Columns[fieldName]);
            }
        }

        void GenerateColumns()
        {
            gridTaskSummary.Settings.ShowFilterRowMenu = true;
            gridTaskSummary.Settings.ShowHeaderFilterButton = true;
            gridTaskSummary.Settings.ShowFooter = true;
            gridTaskSummary.SettingsPopup.HeaderFilter.Height = 200;
            gridTaskSummary.Settings.ShowGroupPanel = false;
            gridTaskSummary.Settings.GroupFormat = "{1}";
            gridTaskSummary.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;

            gridTaskSummary.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            gridTaskSummary.Styles.AlternatingRow.BackColor = Color.FromArgb(234, 234, 234);
            gridTaskSummary.Styles.Row.BackColor = Color.White;
            gridTaskSummary.Styles.GroupRow.Font.Bold = true;

            if (gridTaskSummary.Columns.Count <= 0)
            {
                StringBuilder filterExpresions = new StringBuilder();
                AddColumn(DatabaseObjects.Columns.ItemOrder, "#", HorizontalAlign.Left, 20, "", DefaultBoolean.False);

               AddColumn("TicketId", "Ticket ID", HorizontalAlign.Left, 0, "", true, DefaultBoolean.False);
                filterExpresions.AppendFormat("{0},", "Ticket ID");

                AddColumn(DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Title, HorizontalAlign.Left, 0, "");
                filterExpresions.AppendFormat("{0},", DatabaseObjects.Columns.Title);

                AddColumn(DatabaseObjects.Columns.Status, DatabaseObjects.Columns.Status, HorizontalAlign.Center, 80, "");
                filterExpresions.AppendFormat("{0},", DatabaseObjects.Columns.Status);

                AddColumn(DatabaseObjects.Columns.PercentComplete, "% Comp.", HorizontalAlign.Center, 50, "{0:0.##}%");
                filterExpresions.AppendFormat("{0},", DatabaseObjects.Columns.PercentComplete);

                //AddColumn(DatabaseObjects.Columns.PredecessorsByOrder, "Pred.", HorizontalAlign.Center, 50, "");
                //filterExpresions.AppendFormat("{0},", DatabaseObjects.Columns.Predecessors);

                AddColumn(DatabaseObjects.Columns.AssignedTo, "Assigned To", HorizontalAlign.Center, 80, "");
                filterExpresions.AppendFormat("{0},", DatabaseObjects.Columns.AssignedTo);

                AddColumn(DatabaseObjects.Columns.StartDate, "Start Date", HorizontalAlign.Center, 80, "{0:MMM-d-yyyy}");
                filterExpresions.AppendFormat("{0},", DatabaseObjects.Columns.StartDate);

                AddColumn(DatabaseObjects.Columns.DueDate, "Due Date", HorizontalAlign.Center, 80, "{0:MMM-d-yyyy}");
                filterExpresions.AppendFormat("{0},", DatabaseObjects.Columns.DueDate);
            }
        }

        protected void gridTaskSummary_DataBinding(object sender, EventArgs e)
        {
            gridTaskSummary.DataSource = getDataSource();
        }

        protected void gridTaskSummary_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    GridViewTableDataCell editCell = (GridViewTableDataCell)cell;
                    if (((GridViewDataColumn)editCell.Column).FieldName == DatabaseObjects.Columns.Title)
                    {
                        DataRow currentRow = gridTaskSummary.GetDataRow(e.VisibleIndex);
                        int uGITLevel = Convert.ToInt32(currentRow[DatabaseObjects.Columns.UGITLevel]);
                        int uGITChildCount = Convert.ToInt32(currentRow[DatabaseObjects.Columns.UGITChildCount]);
                        string imgUrl = UGITUtility.GetImageUrlForReport( GetBehaviourIcon(Convert.ToString(currentRow[DatabaseObjects.Columns.TaskBehaviour])));

                        LiteralControl control = editCell.Controls[0] as LiteralControl;
                        string text = control.Text;
                        if (!string.IsNullOrEmpty(imgUrl))
                        {
                            string imghtml = "<table><tr><td style=\"width:16px\"><img src=\"" + imgUrl + "\"/></td><td>{0}</td></tr></table>";
                            if (uGITLevel > 0 && uGITChildCount > 0)
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "<div style=\"padding-left:" + (10 * uGITLevel) + "px\">" + string.Format(imghtml, string.Format("<b>{0}</b>", text)) + "</div>";
                            }
                            else if (uGITLevel > 0)
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "<div style=\"padding-left:" + (10 * uGITLevel) + "px\">" + string.Format(imghtml,  text) + "</div>";
                            }
                            else if (uGITChildCount > 0)
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = string.Format(imghtml, string.Format("<b>{0}</b>", text));
                            }
                        }
                        else
                        {
                            if (uGITLevel > 0 && uGITChildCount > 0)
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "<div style=\"padding-left:" + (10 * uGITLevel) + "px\"><b>" + text + "</b></div>";
                            }
                            else if (uGITLevel > 0)
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "<div style=\"padding-left:" + (10 * uGITLevel) + "px\">" + text + "</div>";
                            }
                            else if (uGITChildCount > 0)
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "<b>" + text + "</b>";
                            }
                        }
                        
                    }

                }
            }
        }

        private string GetBehaviourIcon(string behaviour)
        {
            if (string.IsNullOrEmpty(behaviour))
                return string.Empty;

            string fileName = string.Empty;
            switch (behaviour)
            {
                case Constants.TaskType.Milestone:
                    fileName = @"/Report/Content/images/milestone_icon.png";
                    break;
                case Constants.TaskType.Deliverable:
                    fileName = @"/Report/Content/images/document_down.png";
                    break;
                case Constants.TaskType.Receivable:
                    fileName = @"/Report/Content/images/document_up.png";
                    break;
                default:
                    break;
            }

            return fileName;
        }


        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            TaskSummary_Report taskSummaryreport = new TaskSummary_Report(getDataSource());
            string title = "Task Summary Report";
            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;
            uGovernIT.Manager.ReportGenerationHelper reportHelper = new Manager.ReportGenerationHelper();
            XtraReport report = reportHelper.GenerateReport(gridTaskSummary, getDataSource(), title, 8F, "xls", null, qFormat);
            reportHelper.WriteXlsToResponse(Response, "TaskSummaryReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            // uHelper.WriteXlsToResponse(Response, title + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString(), taskSummaryreport);
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            TaskSummary_Report taskSummaryreport = new TaskSummary_Report(getDataSource());
            string title = "Task Summary Report";
            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;
            uGovernIT.Manager.ReportGenerationHelper reportHelper = new Manager.ReportGenerationHelper();
            XtraReport report = reportHelper.GenerateReport(gridTaskSummary, getDataSource(), title, 8F, "pdf", null, qFormat);
            reportHelper.WritePdfToResponse(Response, "TaskSummaryReport" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            // uHelper.WritePdfToResponse(Response, title + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString(), taskSummaryreport);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            TaskSummary_Report taskSummaryreport = new TaskSummary_Report(getDataSource());
            string title = "Task Summary Report";
            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;
            uGovernIT.Manager.ReportGenerationHelper reportHelper = new Manager.ReportGenerationHelper();
            XtraReport report = reportHelper.GenerateReport(gridTaskSummary, getDataSource(),title, 8F, "xls", null, qFormat);
            reportHelper.WriteXlsToResponse(Response, "TaskSummaryReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            // uHelper.WritePdfToResponse(Response, title + ".pdf", System.Net.Mime.DispositionTypeNames.Inline.ToString(), taskSummaryreport);
        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("Task_Summary_Report_{0}", uHelper.GetCurrentTimestamp());
                //string uploadFileURL = string.Format("/Content/images/ugovernit/upload/{0}.pdf", fileName);
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/upload/{0}.pdf", fileName);
                string path = docPath = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                if (getDataSource() != null && getDataSource().Rows.Count > 0)
                {
                    TaskSummary_Report taskSummaryreport = new TaskSummary_Report(getDataSource());
                    taskSummaryreport.ExportToPdf(docPath);
                    
                    e.Result = UGITUtility.GetImageUrlForReport("/Layouts/uGovernIT/DelegateControl.aspx?control=ticketemail&type=tsksummary&localpath=" + path + "&relativepath=" + uploadFileURL);
                }
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string fileName = string.Format("Task_Summary_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/upload/{0}.pdf", fileName);
                //string uploadFileURL = string.Format("/Content/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                TaskSummary_Report taskSummaryreport = new TaskSummary_Report(getDataSource());
                taskSummaryreport.ExportToPdf(path);
                e.Result = UGITUtility.GetImageUrlForReport(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }

        protected void gridTaskSummary_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            //if(e.Column.FieldName == DatabaseObjects.Columns.AssignedTo)
            //{
            //    List<uGovernIT.Utility.Entities.UserProfile> userProfiles = HttpContext.Current.GetUserManager().GetUserInfosById(Convert.ToString(e.Value));
            //    if(userProfiles!=null && userProfiles.Count> 0)
            //    {
            //        e.DisplayText = string.Join(Constants.Separator6, userProfiles.Select(x => x.UserName));
            //    }
            //}
        }
    }
}
