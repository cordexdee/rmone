using DevExpress.Web;
using DevExpress.XtraReports.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using ReportEntity = uGovernIT.Manager;
namespace uGovernIT.DxReport

{
    public partial class StudioSpecific_Viewer : UserControl
    {
        public string SelectedModule { get; set; }
        public string SelectedType { get; set; }
        public string SortType { get; set; }
        public string IsModuleSort { get; set; }
        public string StrDateFrom { get; set; }
        public string StrDateTo { get; set; }
        public string Module { get; set; }
        public string ReportTitle { get; set; }
        public string Studios { get; set; }
        public string ReportFilterURl;
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");

        DataTable dtReport;
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private string companyLogo = string.Empty;
        private string ERPJobIDName = string.Empty;
        private CompanyDivisionManager _companyDivisionManager = null;
        private StudioManager _studioManager = null;

        protected StudioManager StudioManager
        {
            get
            {
                if (_studioManager == null)
                    _studioManager = new StudioManager(HttpContext.Current.GetManagerContext());
                return _studioManager;
            }
        }
        protected CompanyDivisionManager CompanyDivisionManager
        {
            get
            {
                if (_companyDivisionManager == null)
                {
                    _companyDivisionManager = new CompanyDivisionManager(HttpContext.Current.GetManagerContext());
                }
                return _companyDivisionManager;
            }
        }
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

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            List<string> paramRequired = new List<string>();

            paramRequired.Add("showReport");
            ReportTitle = Request["title"];
            Studios = Request["studios"];
            ERPJobIDName = ConfigurationVariableManager.GetValue(ConfigConstants.ERPJobIDName);
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = delegateControl + "?reportName=StudioSpecific&Filter=Filter&Module=" + Module + "&title=" + ReportTitle;
                Response.Redirect(ReportFilterURl);
            }

            GenerateColumns();
        }

        private void GenerateColumns()
        {
            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = "Module"; 
            colId.Caption = "Module";
            //colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = "DivisionTitle";
            colId.Caption = "Division";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.GroupIndex = 0;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = "StudioTitle";
            colId.Caption = "Studio";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.GroupIndex = 1;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.EstimateNo;
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
            colId.Width = 250;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.CRMCompanyLookup;
            colId.Caption = "Company";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
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
            colId.FieldName = "TicketCreationDate";
            colId.Caption = "Created On";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            colId.Visible = false;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.StreetAddress1;
            colId.Caption = "Address";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grdReport.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.FieldName = DatabaseObjects.Columns.City;
            colId.Caption = DatabaseObjects.Columns.City;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            grdReport.Columns.Add(colId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            companyLogo = ConfigurationVariableManager.GetValue(ConfigConstants.ReportLogo);
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

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.EstimateNo))
                dtResult.Columns.Add(DatabaseObjects.Columns.EstimateNo);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ERPJobIDNC))
                dtResult.Columns.Add(DatabaseObjects.Columns.ERPJobIDNC);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
                dtResult.Columns.Add(DatabaseObjects.Columns.Title);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ApproxContractValue))
                dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(double));

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                dtResult.Columns.Add(DatabaseObjects.Columns.DivisionLookup);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                dtResult.Columns.Add(DatabaseObjects.Columns.TicketCreationDate, typeof(string));

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMProjectStatus))
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMProjectStatus);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Module))
                dtResult.Columns.Add(DatabaseObjects.Columns.Module);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMCompanyLookup))
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMCompanyLookup, typeof(string));

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StreetAddress1))
                dtResult.Columns.Add(DatabaseObjects.Columns.StreetAddress1);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.City))
                dtResult.Columns.Add(DatabaseObjects.Columns.City);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StudioLookup))
                dtResult.Columns.Add(DatabaseObjects.Columns.StudioLookup);

            if (!dtResult.Columns.Contains("StudioTitle"))
                dtResult.Columns.Add("StudioTitle");
            if (!dtResult.Columns.Contains("DivisionTitle"))
                dtResult.Columns.Add("DivisionTitle");
            List<Studio> lstStudios = StudioManager.Load();
            List<CompanyDivision> lstDivisions = CompanyDivisionManager.Load();

            DataTable dtCOM = TicketManager.GetAllTickets(ModuleViewManager.GetByName("COM"));
            DataTable dtOPM = TicketManager.GetOpenTickets(ModuleViewManager.GetByName("OPM"));
            dynamic companyTitle = "";
            dynamic companyId = "";
            DataRow drCompany = null;
            DataRow[] arrCompanies = null;
            if (dtOPM != null && dtOPM.Rows.Count > 0)
            {
                foreach (DataRow dr in dtOPM.Rows)
                {
                    companyId = !String.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMCompanyLookup])) ? dr[DatabaseObjects.Columns.CRMCompanyLookup] : "";
                    
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        arrCompanies = dtCOM.Select($"{DatabaseObjects.Columns.TicketId} = '{companyId}'");
                        if (arrCompanies != null && arrCompanies.Length > 0)
                            companyTitle = arrCompanies[0][DatabaseObjects.Columns.Title];
                        else
                            companyTitle = "";
                    }
                    
                    DataRow newDR = dtResult.NewRow();
                    if (lstDivisions != null && lstDivisions.Count > 0)
                        newDR["DivisionTitle"] = lstDivisions.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.DivisionLookup]))?.Title;

                    if (lstStudios != null && lstStudios.Count > 0)
                        newDR["StudioTitle"] = lstStudios.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.StudioLookup]))?.Title;

                    newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.ERPJobID]; //dr[DatabaseObjects.Columns.TicketId];
                    newDR[DatabaseObjects.Columns.ERPJobIDNC] = dr[DatabaseObjects.Columns.ERPJobIDNC];
                    newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                    newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                    newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyTitleLookup];
                    newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                    newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                    newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMOpportunityStatus];
                    newDR[DatabaseObjects.Columns.Module] = "OPM";
                    newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                    newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                    newDR[DatabaseObjects.Columns.StudioLookup] = Convert.ToString(dr[DatabaseObjects.Columns.StudioLookup]);
                    dtResult.Rows.Add(newDR);
                }
            }

            DataTable dtLEM = TicketManager.GetOpenTickets(ModuleViewManager.GetByName("LEM"));
            if (dtLEM != null && dtLEM.Rows.Count > 0)
            {
                foreach (DataRow dr in dtLEM.Rows)
                {
                    companyId = !String.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMCompanyLookup])) ? dr[DatabaseObjects.Columns.CRMCompanyLookup] : "";
                    drCompany = companyId != "" ? dtCOM.Select($"{DatabaseObjects.Columns.TicketId} = '{companyId}'")[0] : null;
                    if (drCompany != null)
                        companyTitle = drCompany[DatabaseObjects.Columns.Title];
                    else
                        companyTitle = "";

                    DataRow newDR = dtResult.NewRow();
                    if (lstDivisions != null && lstDivisions.Count > 0)
                        newDR["DivisionTitle"] = lstDivisions.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.DivisionLookup]))?.Title;

                    newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.TicketId];
                    newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                    newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                    newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyTitleLookup];
                    newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                    newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                    newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.LeadStatus];
                    newDR[DatabaseObjects.Columns.Module] = "LEM";
                    newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                    newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                    newDR[DatabaseObjects.Columns.StudioLookup] = string.Empty;
                    dtResult.Rows.Add(newDR);
                }
            }

            DataTable dtCPR = TicketManager.GetOpenTickets(ModuleViewManager.GetByName("CPR"));

            if (dtCPR != null)
            {
                DataRow[] drsCPR = null;
                drsCPR = dtCPR.Select();

                if (drsCPR != null && drsCPR.Length > 0)
                {
                    foreach (DataRow dr in drsCPR)
                    {
                        companyId = !String.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMCompanyLookup])) ? dr[DatabaseObjects.Columns.CRMCompanyLookup] : "";
                        DataRow[] drCollection = dtCOM.Select($"{DatabaseObjects.Columns.TicketId} = '{companyId}'");
                        if(drCollection == null || drCollection.Length == 0)
                        {
                            continue;
                        }
                        else
                            drCompany = drCollection[0];
                        
                        companyTitle = UGITUtility.ObjectToString(drCompany[DatabaseObjects.Columns.Title]);

                        DataRow newDR = dtResult.NewRow();
                        if (lstDivisions != null && lstDivisions.Count > 0)
                            newDR["DivisionTitle"] = lstDivisions.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.DivisionLookup]))?.Title;

                        if (lstStudios != null && lstStudios.Count > 0)
                            newDR["StudioLookup"] = lstStudios.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.StudioLookup]))?.Title;

                        newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.ERPJobID]; //dr[DatabaseObjects.Columns.EstimateNo];
                        newDR[DatabaseObjects.Columns.ERPJobIDNC] = dr[DatabaseObjects.Columns.ERPJobIDNC];
                        newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                        newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                        newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyTitleLookup];

                        newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                        newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                        newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMProjectStatus];
                        newDR[DatabaseObjects.Columns.Module] = uHelper.getModuleNameByTicketId(Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                        newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                        newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                        newDR[DatabaseObjects.Columns.StudioLookup] = Convert.ToString(dr[DatabaseObjects.Columns.StudioLookup]);
                        dtResult.Rows.Add(newDR);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Studios))
            {
                DataRow[] drResults = null;

                Studios = Studios.Replace("None", string.Empty);
                drResults = dtResult.Select($"{DatabaseObjects.Columns.StudioLookup} in ({string.Join(",", Studios.Split(',').Select(x => string.Format("'{0}'", x)).ToList())})");

                if (drResults != null && drResults.Length > 0)
                    dtResult = drResults.CopyToDataTable();
                else
                    dtResult = null;
            }

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

            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Studio Specific Report", 8F, "xls", null, qFormat);
            reportHelper.WriteXlsToResponse(Response, "StudioSpecificReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            reportHelper.companyLogo = uHelper.GetReportLogoURL(ApplicationContext);
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

            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Studio Specific Report", 6.75F, "pdf", null, qFormat);
            reportHelper.WritePdfToResponse(Response, "StudioSpecificReport" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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
        }
    }
}