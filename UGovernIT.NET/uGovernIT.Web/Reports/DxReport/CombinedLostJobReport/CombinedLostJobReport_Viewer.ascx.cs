using DevExpress.Web;
using DevExpress.Web.Rendering;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Helpers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using ReportEntity = uGovernIT.Manager;

namespace uGovernIT.DxReport
{
    public partial class CombinedLostJobReport_Viewer : UserControl
    {
        DataTable dtReport;
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string Module { get; set; }
        public string SelectedModule { get; set; }
        public string SelectedType { get; set; }
        public string ReportTitle { get; set; }
        public string ReportFilterURl;
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");

        public bool JobSummary { get; set; }

        private string companyLogo = null;

        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private FieldConfigurationManager _fieldConfigurationManager = null;
        private UserProfileManager _userProfileManager = null;
        private FieldConfiguration field = null;
        private string ERPJobIDName = string.Empty;
        
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();

                }
                return _context;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(ApplicationContext);

                }
                return _configurationVariableManager;
            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(ApplicationContext);
                }
                return _ticketManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fieldConfigurationManager == null)
                {
                    _fieldConfigurationManager = new FieldConfigurationManager(ApplicationContext);
                }
                return _fieldConfigurationManager;
            }
        }

        protected UserProfileManager UserProfileManager
        {
            get
            {
                if (_userProfileManager == null)
                {
                    _userProfileManager = new UserProfileManager(ApplicationContext);
                }
                return _userProfileManager;
            }
        }
        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            List<string> paramRequired = new List<string>();
            paramRequired.Add("SelectedModule");
            paramRequired.Add("SelectedType");
            Module = Request["Module"];
            ReportTitle = Request["title"];
            ERPJobIDName = ConfigurationVariableManager.GetValue(ConfigConstants.ERPJobIDName);

            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = delegateControl + "?reportName=CombinedLostJobReport&Filter=Filter&Module=" + Module + "&title=" + ReportTitle;

                Response.Redirect(ReportFilterURl);
            }

            GenerateColumns();
        }

        private void GenerateColumns()
        {
            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.DivisionLookup;
            colId.Caption = "Division";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.GroupIndex = 0;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.ERPJobID;
            colId.Caption = ERPJobIDName;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = 150;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.ERPJobIDNC;
            colId.Caption = "CMIC NCO #";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = 150;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.Title;
            colId.Caption = "Name";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = 300;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.OwnerUser;
            colId.Caption = "Owner";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = 150;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = "AwardedorLossDate";
            colId.Caption = "Loss Date";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = 90;
            colId.PropertiesTextEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.ApproxContractValue;
            colId.Caption = "Approx Contract Value";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = 100;
            colId.PropertiesTextEdit.DisplayFormatString = "{0:c}";
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.Reason;
            colId.Caption = "Loss Reason";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Width = 250;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.CreationDate;
            colId.Caption = "Created On";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.PropertiesTextEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            colId.Width = 90;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.CRMProjectStatus;
            colId.Caption = "Status";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.Estimator;
            colId.Caption = "Estimator";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.StudioLookup;
            colId.Caption = "Studio";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.GroupIndex = 1;
            grdReport.Columns.Add(colId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //_context = Session["context"] as ApplicationContext;

            companyLogo = ConfigurationVariableManager.GetValue(ConfigConstants.ReportLogo);//check logo code

            JobSummary = UGITUtility.StringToBoolean(Request["JobSummary"]);

            grdReport.Columns[DatabaseObjects.Columns.CRMProjectStatus].Visible = JobSummary;
            grdReport.Columns[DatabaseObjects.Columns.TicketCreationDate].Visible = JobSummary;
            grdReport.Columns[DatabaseObjects.Columns.Estimator].Visible = JobSummary;
            if (!JobSummary)
                (grdReport.Columns[DatabaseObjects.Columns.Estimator] as GridViewDataColumn).UnGroup();
            grdReport.Columns[DatabaseObjects.Columns.Reason].Visible = !JobSummary;

            field = FieldConfigurationManager.GetFieldByFieldName(DatabaseObjects.Columns.Estimator, string.Empty);
        }

        protected override void OnPreRender(EventArgs e)
        {
            grdReport.DataBind();
            base.OnPreRender(e);
        }

        #endregion

        #region Custom Events

        private DataTable LoadData()
        {
            DataTable dtResult = new DataTable();

            try
            {
                string query = string.Empty;
                DateTime dtStart = new DateTime();
                if (!string.IsNullOrEmpty(Convert.ToString(Request["dtStart"])))
                {
                    DateTime.TryParse(Convert.ToString(Request["dtStart"]), out dtStart);

                    query = query + $"{DatabaseObjects.Columns.AwardedLossDate} >= '{dtStart}'";
                }
                else
                {
                    dtStart = DateTime.MinValue.AddYears(1753);
                }

                DateTime dtEnd = new DateTime();
                if (!string.IsNullOrEmpty(Convert.ToString(Request["dtEnd"])))
                {
                    if (DateTime.TryParse(Convert.ToString(Request["dtEnd"]), out dtEnd))
                    {
                        // Changing Time 12AM to 11:59 PM to include all time value for a specific date     
                        dtEnd = dtEnd.AddMinutes(1439);
                    }

                    if (!string.IsNullOrEmpty(query))
                        query = query + $" and {DatabaseObjects.Columns.AwardedLossDate} <= '{dtEnd}'";
                    else
                        query = query + $" {DatabaseObjects.Columns.AwardedLossDate} <= '{dtEnd}'";
                }
                else
                {
                    dtEnd = DateTime.Today.AddYears(500);
                }

                if (!string.IsNullOrEmpty(query))
                    query = query + $" and {DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'";
                else
                    query = $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'";

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMProjectID))
                    dtResult.Columns.Add(DatabaseObjects.Columns.CRMProjectID);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ERPJobID))
                    dtResult.Columns.Add(DatabaseObjects.Columns.ERPJobID);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ERPJobIDNC))
                    dtResult.Columns.Add(DatabaseObjects.Columns.ERPJobIDNC);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
                    dtResult.Columns.Add(DatabaseObjects.Columns.Title);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ApproxContractValue))
                    dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(double));

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.OwnerUser))
                    dtResult.Columns.Add(DatabaseObjects.Columns.OwnerUser);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.AwardedLossDate))
                    dtResult.Columns.Add(DatabaseObjects.Columns.AwardedLossDate, typeof(DateTime));


                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketComment))
                    dtResult.Columns.Add(DatabaseObjects.Columns.TicketComment);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    dtResult.Columns.Add(DatabaseObjects.Columns.DivisionLookup);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                    dtResult.Columns.Add(DatabaseObjects.Columns.TicketCreationDate, typeof(DateTime));

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Estimator))
                    dtResult.Columns.Add(DatabaseObjects.Columns.Estimator);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMProjectStatus))
                    dtResult.Columns.Add(DatabaseObjects.Columns.CRMProjectStatus);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StudioLookup))
                    dtResult.Columns.Add(DatabaseObjects.Columns.StudioLookup);

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("@TenantID", _context.TenantID);
                values.Add("@StartDate", dtStart.ToShortDateString());
                values.Add("@Enddate", dtEnd.ToShortDateString());
                values.Add("@JobSummary", JobSummary);

                dtResult = DAL.uGITDAL.ExecuteDataSetWithParameters("Usp_GetRptCombinedReport", values);

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error in Combined Lost Job Report.");
            }
            
            //foreach (DataRow row in dtResult.Rows)
            //{
            //    foreach (DataColumn dc in dtResult.Columns)
            //    {
            //        string val = Convert.ToString(row[dc]);
            //        if (!string.IsNullOrEmpty(val) && val.Contains(Constants.Separator))
            //        {
            //            row[dc] = UGITUtility.RemoveIDsFromLookupString(val);
            //        }

            //        if (FieldConfigurationManager.GetFieldByFieldName(dc.ColumnName) != null)
            //        {
            //            string value = FieldConfigurationManager.GetFieldConfigurationData(dc.ColumnName, val);//Need to remove
            //            row[dc] = value;
            //        }
            //    }
            //}// need to check it for condition @chetan

            return dtResult;
           
        }

        #endregion

        #region Grid Events

        protected void grdReport_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
           
        }

        protected void grdReport_DataBinding(object sender, EventArgs e)
        {
            if (dtReport == null)
            {
                dtReport = LoadData();
                grdReport.DataSource = dtReport;
            }
        }

        #endregion

        #region Export Events

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            //reportHelper.companyLogo = companyLogo;
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            if (dtReport == null)
            {
                dtReport = LoadData();
            }
            grdReport.DataSource = dtReport;

            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            //qFormat.ShowCompanyLogo = true;

            if (JobSummary)
            {
                XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Combined Job Report", 8F, "xls", null, qFormat);
                reportHelper.WriteXlsToResponse(Response, "CombinedJobReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            }
            else
            {
                XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Combined Lost Job Report", 8F, "xls", null, qFormat);
                reportHelper.WriteXlsToResponse(Response, "CombinedLostJobReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            }
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            reportHelper.companyLogo = companyLogo;
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            if (dtReport == null)
            {
                dtReport = LoadData();
            }
            grdReport.DataSource = dtReport;

            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;

            if (JobSummary)
            {
                XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Combined Job Report", 6.75F, "pdf", null, qFormat);
                reportHelper.WritePdfToResponse(Response, "CombinedJobReport" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            }
            else
            {
                XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Combined Lost Job Report", 6.75F, "pdf", null, qFormat);
                reportHelper.WritePdfToResponse(Response, "CombinedLostJobReport" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            }
        }

        void reportHelper_CustomizeColumn(object source, ReportEntity.ControlCustomizationEventArgs e)
        {

        }

        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        void reportHelper_CustomizeColumnsCollection(object source, ReportEntity.ColumnsCreationEventArgs e)
        {
            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Project"))
            {
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Project").ColumnWidth *= 2;
            }
        }

        #endregion

        protected void grdReport_Load(object sender, EventArgs e)
        {
            //ASPxGridView grid = sender as ASPxGridView;
            //(grid.Columns["ApproxContractValue"] as GridViewDataTextColumn).PropertiesTextEdit.DisplayFormatString = "{0:C}";

        }
    }
}

