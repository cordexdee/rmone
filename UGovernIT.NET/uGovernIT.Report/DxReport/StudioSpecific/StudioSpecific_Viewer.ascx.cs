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
using uGovernIT.Report.Helpers;
using uGovernIT.Utility;
using ReportEntity = uGovernIT.Manager;
namespace uGovernIT.Report.DxReport

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
        public string delegateControl = UGITUtility.GetAbsoluteURL("BuildReport.aspx");

        DataTable dtReport;
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private string companyLogo = string.Empty;

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
            //paramRequired.Add("dtStart");
            //paramRequired.Add("dtEnd");
            //Module = Request["Module"];
            //paramRequired.Add("Module");
            paramRequired.Add("showReport");
            ReportTitle = Request["title"];
            Studios = Request["studios"];

            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                //ReportFilterURl = ApplicationContext.ReportUrl + delegateControl + "?reportName=StudioSpecific&Filter=Filter&Module=" + Module + "&title=" + ReportTitle;
                ReportFilterURl = delegateControl + "?reportName=StudioSpecific&Filter=Filter&Module=" + Module + "&title=" + ReportTitle;
                Response.Redirect(ReportFilterURl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            //_context = Session["context"] as ApplicationContext;
            companyLogo = ConfigurationVariableManager.GetValue(ConfigConstants.CompanyLogo);
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

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
                dtResult.Columns.Add(DatabaseObjects.Columns.Title);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ApproxContractValue))
                dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(double));

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMBusinessUnit))
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMBusinessUnit);

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

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Studio))
                dtResult.Columns.Add(DatabaseObjects.Columns.Studio);

            DataTable dtCOM = TicketManager.GetAllTickets(ModuleViewManager.GetByName("COM"));
            //DataTable dtOPM = TicketManager.GetAllTickets(ModuleViewManager.GetByName("OPM"));
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
                    //drCompany = companyId != "" ? dtCOM.Select($"{DatabaseObjects.Columns.TicketId} = '{companyId}'")[0] : null;
                    //if (drCompany != null)
                    //    companyTitle = drCompany[DatabaseObjects.Columns.Title];
                    //else
                    //    companyTitle = "";
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        arrCompanies = dtCOM.Select($"{DatabaseObjects.Columns.TicketId} = '{companyId}'");
                        if (arrCompanies != null && arrCompanies.Length > 0)
                            companyTitle = arrCompanies[0][DatabaseObjects.Columns.Title];
                        else
                            companyTitle = "";
                    }
                    
                    DataRow newDR = dtResult.NewRow();
                    newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.CRMProjectID]; //dr[DatabaseObjects.Columns.TicketId];
                    newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                    newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                    newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyTitleLookup];
                    newDR[DatabaseObjects.Columns.CRMBusinessUnit] = dr[DatabaseObjects.Columns.CRMBusinessUnit];
                    newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                    newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMOpportunityStatus];
                    newDR[DatabaseObjects.Columns.Module] = "OPM";
                    newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                    newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                    newDR[DatabaseObjects.Columns.Studio] = Convert.ToString(dr[DatabaseObjects.Columns.Studio]);
                    dtResult.Rows.Add(newDR);
                }
            }

            //DataTable dtLEM = TicketManager.GetAllTickets(ModuleViewManager.GetByName("LEM"));
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
                    newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.TicketId];
                    newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                    newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                    newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyTitleLookup];
                    newDR[DatabaseObjects.Columns.CRMBusinessUnit] = dr[DatabaseObjects.Columns.CRMBusinessUnit];
                    newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                    newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.LeadStatus];
                    newDR[DatabaseObjects.Columns.Module] = "LEM";
                    newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                    newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                    newDR[DatabaseObjects.Columns.Studio] = string.Empty;
                    dtResult.Rows.Add(newDR);
                }
            }

            //DataTable dtCPR = TicketManager.GetAllTickets(ModuleViewManager.GetByName("CPR"));
            DataTable dtCPR = TicketManager.GetOpenTickets(ModuleViewManager.GetByName("CPR"));

            // Commented as CNS records not required. Mail Ref. SPR #184 BCCI Issue: Business Unit Report Bug and Mod ESCALATION
            /*
            DataTable dtCNS = TicketManager.GetAllTickets(ModuleViewManager.GetByName("CNS"));
            if (dtCPR != null && dtCNS != null)
            {
                dtCPR.Merge(dtCNS, false, MissingSchemaAction.Ignore);//Need to ask about pctplannedprofit.
            }
            else if (dtCNS != null)
            {
                dtCPR = dtCNS;
            }
            */

            if (dtCPR != null)
            {
                DataRow[] drsCPR = null;
                drsCPR = dtCPR.Select();

                if (drsCPR != null && drsCPR.Length > 0)
                {
                    foreach (DataRow dr in drsCPR)
                    {
                        companyId = !String.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMCompanyLookup])) ? dr[DatabaseObjects.Columns.CRMCompanyLookup] : "";
                        drCompany = companyId != "" ? dtCOM.Select($"{DatabaseObjects.Columns.TicketId} = '{companyId}'")[0] : null;
                        if (drCompany != null)
                            companyTitle = drCompany[DatabaseObjects.Columns.Title];
                        else
                            companyTitle = "";

                        DataRow newDR = dtResult.NewRow();
                        newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.CRMProjectID]; //dr[DatabaseObjects.Columns.EstimateNo];
                        newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                        newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                        newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyTitleLookup];

                        newDR[DatabaseObjects.Columns.CRMBusinessUnit] = dr[DatabaseObjects.Columns.CRMBusinessUnit];
                        newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                        newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMProjectStatus];
                        newDR[DatabaseObjects.Columns.Module] = uHelper.getModuleNameByTicketId(Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                        newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                        newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                        newDR[DatabaseObjects.Columns.Studio] = Convert.ToString(dr[DatabaseObjects.Columns.Studio]);
                        dtResult.Rows.Add(newDR);
                    }
                }
            }

            // Commented below code, as per client's request to remove date filters.
            /*
            //if (!string.IsNullOrEmpty(Convert.ToString(Request["dtStart"])) && !string.IsNullOrEmpty(Convert.ToString(Request["dtEnd"])))
            if (!string.IsNullOrEmpty(Convert.ToString(Request["dtStart"])) || !string.IsNullOrEmpty(Convert.ToString(Request["dtEnd"])) || !string.IsNullOrEmpty(Studios))
            {
                DateTime dtStart = DateTime.MinValue;
                DateTime.TryParse(Convert.ToString(Request["dtStart"]), out dtStart);

                DateTime dtEnd = DateTime.MaxValue;
                if (DateTime.TryParse(Convert.ToString(Request["dtEnd"]), out dtEnd))
                {
                    // Changing Time 12AM to 11:59 PM to include all time value for a specific date     
                    dtEnd = dtEnd.AddMinutes(1439);
                }

                if (dtEnd == DateTime.MinValue)
                    dtEnd = DateTime.MaxValue;

                //DataRow[] testdrResults = dtResult.Select(string.Format("{0} <= #" + dtEnd + "# AND {0} >= #" + dtStart + "#", DatabaseObjects.Columns.TicketCreationDate));
                DataRow[] drResults = null;

                if (!string.IsNullOrEmpty(Studios))
                {
                    Studios = Studios.Replace("None", string.Empty);
                    //drResults = dtResult.Select($"{DatabaseObjects.Columns.TicketCreationDate} >= #{dtStart}# and {DatabaseObjects.Columns.TicketCreationDate} <= #{dtEnd}# and {DatabaseObjects.Columns.Studio} in ({string.Join(",", Studios.Split(',').Select(x => string.Format("'{0}'", x)).ToList())})");
                    drResults = dtResult.Select($"{DatabaseObjects.Columns.Studio} in ({string.Join(",", Studios.Split(',').Select(x => string.Format("'{0}'", x)).ToList())})").Where(x => (Convert.ToDateTime(x[DatabaseObjects.Columns.TicketCreationDate]) >= dtStart && Convert.ToDateTime(x[DatabaseObjects.Columns.TicketCreationDate]) <= dtEnd)).ToArray();
                }
                else
                {
                    //drResults = dtResult.Select($"{DatabaseObjects.Columns.TicketCreationDate} >= #{dtStart}# and {DatabaseObjects.Columns.TicketCreationDate} <= #{dtEnd}#");
                    drResults = dtResult.Select().Where(x => (Convert.ToDateTime(x[DatabaseObjects.Columns.TicketCreationDate]) >= dtStart && Convert.ToDateTime(x[DatabaseObjects.Columns.TicketCreationDate]) <= dtEnd)).ToArray();
                }

                if (drResults != null && drResults.Length > 0)
                    dtResult = drResults.CopyToDataTable();
                else
                    dtResult = null;
            }
            */

            if (!string.IsNullOrEmpty(Studios))
            {
                DataRow[] drResults = null;

                Studios = Studios.Replace("None", string.Empty);
                drResults = dtResult.Select($"{DatabaseObjects.Columns.Studio} in ({string.Join(",", Studios.Split(',').Select(x => string.Format("'{0}'", x)).ToList())})");

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