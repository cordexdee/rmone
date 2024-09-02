using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
//using ReportEntity = uGovernIT.Manager.Report.Entities;
using ReportEntity = uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class ProjectPlanReport : UserControl
    {
        private string ids = string.Empty;
        UGITModule moduleDetail = new UGITModule();
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager objConfigurationVariableHelper = null;
        DataTable dtResults = new DataTable();
        TicketManager ticketManager;
        XtraReport report;
        UGITTaskManager TaskManager;
        ModuleViewManager moduleViewManager;
        FieldConfigurationManager fieldManager = null;

        private string[] Months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        protected void Page_Load(object sender, EventArgs e)
        {
            ticketManager = new TicketManager(context);
            fieldManager = new FieldConfigurationManager(context);

            report = new XtraReport();
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            ids = Convert.ToString(Request["ids"]);
            
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";

            if (string.IsNullOrEmpty(hndYear.Value))
            {
                hndYear.Value = DateTime.Now.Year.ToString();
            }

            lblSelectedYear.Text = hndYear.Value;
            grid.DataSource = GetData();
            grid.DataBind();

        }

        private DataTable GetData(bool dataForReport = false)
        {
            if (string.IsNullOrEmpty(ids))
                return null;

            //DataTable dtResults = UGITUtility.ModuleDataCache.GetOpenTickets(uHelper.getModuleIdByModuleName("PMM"));
            moduleViewManager = new ModuleViewManager(context);
            moduleDetail = moduleViewManager.LoadByName("PMM");
            dtResults = ticketManager.GetOpenTickets(moduleDetail);
            DataTable dtProjPortFolio = new DataTable();
            DataRow[] dRows = dtResults.Select(string.Format("{0} in ('{1}')", DatabaseObjects.Columns.TicketId, ids.Replace(",", "','")));
            if (!dtProjPortFolio.Columns.Contains(DatabaseObjects.Columns.TicketId))
            {
                dtProjPortFolio.Columns.Add(DatabaseObjects.Columns.TicketId);
            }

            if (!dtProjPortFolio.Columns.Contains(DatabaseObjects.Columns.Title))
            {
                dtProjPortFolio.Columns.Add(DatabaseObjects.Columns.Title);
            }

            if (!dtProjPortFolio.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
            {
                dtProjPortFolio.Columns.Add(DatabaseObjects.Columns.TicketPriorityLookup);
            }

            if (!dtProjPortFolio.Columns.Contains(DatabaseObjects.Columns.TicketStatus))
            {
                dtProjPortFolio.Columns.Add(DatabaseObjects.Columns.TicketStatus);
            }

            if (!dtProjPortFolio.Columns.Contains(DatabaseObjects.Columns.TicketPctComplete))
            {
                dtProjPortFolio.Columns.Add(DatabaseObjects.Columns.TicketPctComplete);
            }

            foreach (string month in Months)
                if (!dtProjPortFolio.Columns.Contains(month))
                {
                    dtProjPortFolio.Columns.Add(month);
                }


            if (dRows != null)
            {
                int year = UGITUtility.StringToInt(hndYear.Value);
                TaskManager = new UGITTaskManager(context);
                string status;
                string PlannedTasks, InProgressTasks, CompletedTasks;
                
                foreach (DataRow dr in dRows)
                {
                    //DataTable dtTasks = TaskCache.GetAllTasksByProjectID("PMM", Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                    DataTable dtTasks = TaskManager.GetAllTasksByProjectID("PMM", Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));

                    object[] monthData = new object[12];
                    for (int i = 0; i < Months.Length; i++)
                    {
                        DateTime startDate = new DateTime(year, i + 1, 1);
                        DateTime endDate = new DateTime(year, i + 1, DateTime.DaysInMonth(year, i + 1));
                        DataRow[] drs = dtTasks.Select(string.Format("ParentTaskID = 0 And ((StartDate >= '{0}' And DueDate <= '{1}') OR  (StartDate <= '{0}' And DueDate >= '{0}' And DueDate <= '{1}'))", startDate, endDate));
                        status = "Planned";
                        PlannedTasks = "";
                        InProgressTasks = "";
                        CompletedTasks = "";
                        if (drs != null && drs.Length > 0)
                        {
                            List<string> lstTasks = new List<string>();
                            foreach (DataRow drTask in drs)
                            {
                                if (Convert.ToString(drTask[DatabaseObjects.Columns.Status]) == "In Progress")
                                {
                                    status = Convert.ToString(drTask[DatabaseObjects.Columns.Status]);
                                    InProgressTasks = InProgressTasks + ", " + Convert.ToString(drTask[DatabaseObjects.Columns.Title]);
                                }
                                else if (Convert.ToString(drTask[DatabaseObjects.Columns.Status]) == "Completed")
                                {
                                    status = Convert.ToString(drTask[DatabaseObjects.Columns.Status]);
                                    CompletedTasks = CompletedTasks + ", " + Convert.ToString(drTask[DatabaseObjects.Columns.Title]);
                                }
                                else
                                { 
                                    PlannedTasks = PlannedTasks + ", " + Convert.ToString(drTask[DatabaseObjects.Columns.Title]);
                                }
                                //lstTasks.Add(string.Format("{0};#{1}", status, Convert.ToString(drTask[DatabaseObjects.Columns.Title])));
                            }
                            if(dataForReport)
                            {
                                if (!string.IsNullOrEmpty(PlannedTasks))
                                {
                                    PlannedTasks = PlannedTasks.Substring(2);
                                    lstTasks.Add(string.Format("Planned;#{0}", PlannedTasks));
                                }
                                if (!string.IsNullOrEmpty(InProgressTasks))
                                {
                                    InProgressTasks = InProgressTasks.Substring(2);
                                    lstTasks.Add(string.Format("In Progress;#{0}", InProgressTasks));
                                }
                                if (!string.IsNullOrEmpty(CompletedTasks))
                                {
                                    CompletedTasks = CompletedTasks.Substring(2);
                                    lstTasks.Add(string.Format("Completed;#{0}", CompletedTasks));
                                }
                                monthData[i] = string.Join(Constants.Separator1, lstTasks.ToArray());
                            }
                            else 
                            { 
                                if (!string.IsNullOrEmpty(PlannedTasks)) {
                                    PlannedTasks = PlannedTasks.Substring(2);
                                    lstTasks.Add(string.Format("<div class = 'bg-Planned'>{0}</div>", PlannedTasks));
                                }
                                if (!string.IsNullOrEmpty(InProgressTasks)) {
                                    InProgressTasks = InProgressTasks.Substring(2);
                                    lstTasks.Add(string.Format("<div class = 'bg-InProgress'>{0}</div>", InProgressTasks));
                                }
                                if (!string.IsNullOrEmpty(CompletedTasks)) {
                                    CompletedTasks = CompletedTasks.Substring(2);
                                    lstTasks.Add(string.Format("<div class = 'bg-Completed'>{0}</div>", CompletedTasks));
                                }

                                monthData[i] = string.Join("", lstTasks.ToArray());
                            }
                        }

                    }

                    int pct = (int)(UGITUtility.StringToDouble(Convert.ToString(dr[DatabaseObjects.Columns.TicketPctComplete])));
                    //dtProjPortFolio.Rows.Add(new object[] { dr[DatabaseObjects.Columns.TicketId], dr[DatabaseObjects.Columns.Title], dr[DatabaseObjects.Columns.TicketPriorityLookup], dr[DatabaseObjects.Columns.TicketStatus], pct, monthData[0], monthData[1], monthData[2], monthData[3], monthData[4], monthData[5], monthData[6], monthData[7], monthData[8], monthData[9], monthData[10], monthData[11] });
                    dtProjPortFolio.Rows.Add(new object[] { dr[DatabaseObjects.Columns.TicketId], dr[DatabaseObjects.Columns.Title], fieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketPriorityLookup, Convert.ToString(dr[DatabaseObjects.Columns.TicketPriorityLookup])), dr[DatabaseObjects.Columns.TicketStatus], pct, monthData[0], monthData[1], monthData[2], monthData[3], monthData[4], monthData[5], monthData[6], monthData[7], monthData[8], monthData[9], monthData[10], monthData[11] });
                    
                }
            }
            return dtProjPortFolio;
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void grid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.KeyValue == null)
                return;

            string value = Convert.ToString(e.CellValue);
            if (!string.IsNullOrEmpty(value) && value.Contains(Constants.Separator))
            {
                string[] vals = value.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                if (vals.Length > 1)
                {
                    e.Cell.Text = vals[1];
                    //e.Cell.Text= "<div class='bg-danger'>Phase1</div><div class='bg-success'>Phase2</div><div class='bg-warning'>Phase3</div>";
                    //string colorCode = "#FFFFFF";
                    //if (vals[0] == "Completed")
                    //    colorCode = "#C6EFCE";
                    //else if (vals[0] == "In Progress")
                    //    colorCode = "#FFCC99";
                    //else
                    //    colorCode = "#B4C6E7";
                    //e.Cell.RowSpan = 3;
                    
                    //e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(colorCode);

                }
            }
        }

        protected void previousYear_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void nextYear_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            ReportQueryFormat qryFormat = new ReportQueryFormat();
            qryFormat.AdditionalInfo = string.Empty;
            qryFormat.Header = "Project Plan";
            qryFormat.Footer = string.Empty;
            qryFormat.ShowCompanyLogo = true;
            qryFormat.AdditionalFooterInfo = string.Empty;
            qryFormat.ShowDateInFooter = true;
            Dictionary<string, string> legend = new Dictionary<string, string>();
            legend.Add("Planned", "#5B84CF");
            legend.Add("Completed", "#57BE6A");
            legend.Add("In Progress", "#FD9227");
            qryFormat.Legend = legend;

            string url = objConfigurationVariableHelper.GetValue(Constants.HomePage);
            XtraReport report = reportHelper.GenerateReport(grid, GetData(true), qryFormat.Header, 8F, "xls", null, qryFormat);
            reportHelper.homePageUrl = url;
            reportHelper.WriteXlsToResponse(Response, qryFormat.Header + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            ReportQueryFormat qryFormat = new ReportQueryFormat();
            qryFormat.AdditionalInfo = string.Empty;
            qryFormat.Header = "Project Plan";
            qryFormat.Footer = string.Empty;
            qryFormat.ShowCompanyLogo = true;
            qryFormat.AdditionalFooterInfo = string.Empty;
            qryFormat.ShowDateInFooter = true;
            Dictionary<string, string> legend = new Dictionary<string, string>();
            legend.Add("Planned", "#B4C6E7");
            legend.Add("Completed", "#C6EFCE");
            legend.Add("In Progress", "#FFCC99");
            qryFormat.Legend = legend;

            XtraReport report = reportHelper.GenerateReport(grid, GetData(true), qryFormat.Header, 6.75F, "pdf", null, qryFormat);
            string url = objConfigurationVariableHelper.GetValue(Constants.HomePage);
            reportHelper.homePageUrl = url;
            reportHelper.WritePdfToResponse(Response, qryFormat.Header + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

        }


        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();

                XtraReport report = reportHelper.GenerateReport(grid, GetData(true), "Project Plan", 6.75F);

                string fileName = string.Format("Project_Plan_{0}", UGITUtility.GetCurrentTimestamp());
                string uploadFileURL = string.Format("/Layouts/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ticketemail&type=queryReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }
    }
    }

    