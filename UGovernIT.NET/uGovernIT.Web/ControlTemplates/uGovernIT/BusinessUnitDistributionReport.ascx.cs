﻿using DevExpress.Web;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using ReportEntity = uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class BusinessUnitDistributionReport : UserControl
    {
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

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
                dtResult.Columns.Add(DatabaseObjects.Columns.Title);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ApproxContractValue))
                dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(string));

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

            DataTable dtCOM = TicketManager.GetAllTickets(ModuleViewManager.GetByName("COM"));
            DataTable dtOPM = TicketManager.GetAllTickets(ModuleViewManager.GetByName("OPM"));
            dynamic companyTitle = "";
            dynamic companyId = "";
            DataRow drCompany = null;
            if (dtOPM != null && dtOPM.Rows.Count > 0)
            {
                foreach (DataRow dr in dtOPM.Rows)
                {
                    companyId = !String.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMCompanyLookup])) ? dr[DatabaseObjects.Columns.CRMCompanyLookup] : "";
                    drCompany = companyId != "" ?  dtCOM.Select($"{DatabaseObjects.Columns.TicketId} = '{companyId}'")[0] : null;
                    if (drCompany != null)
                        companyTitle = drCompany[DatabaseObjects.Columns.Title];
                    else
                        companyTitle = "";

                    DataRow newDR = dtResult.NewRow();

                    newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.TicketId];
                    newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                    newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                    newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyLookup];
                    newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                    newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                    newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMOpportunityStatus];
                    newDR[DatabaseObjects.Columns.Module] = "OPM";
                    newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                    newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                    dtResult.Rows.Add(newDR);
                }
            }

            DataTable dtLEM = TicketManager.GetAllTickets(ModuleViewManager.GetByName("LEM"));
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
                    newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyLookup];
                    newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                    newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                    newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.LeadStatus];
                    newDR[DatabaseObjects.Columns.Module] = "LEM";
                    newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                    newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                    dtResult.Rows.Add(newDR);
                }
            }

            DataTable dtCPR = TicketManager.GetAllTickets(ModuleViewManager.GetByName("CPR"));
            DataTable dtCNS = TicketManager.GetAllTickets(ModuleViewManager.GetByName("CNS"));
            if (dtCPR != null && dtCNS != null)
            {
                dtCPR.Merge(dtCNS,false,MissingSchemaAction.Ignore);//Need to ask about pctplannedprofit.
            }
            else if (dtCNS != null)
            {
                dtCPR = dtCNS;
            }

            if (dtCPR != null)
            {
                DataRow[] drsCPR = null;
                drsCPR = dtCPR.Select();

                if (drsCPR != null && drsCPR.Length > 0)
                {
                    foreach (DataRow dr in drsCPR)
                    {
                        companyId = !String.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMCompanyLookup])) ? dr[DatabaseObjects.Columns.CRMCompanyLookup] : "";
                        drCompany = companyId != "" ? dtCOM.Select($"{DatabaseObjects.Columns.ID} = {companyId}")[0] : null;
                        if (drCompany != null)
                            companyTitle = drCompany[DatabaseObjects.Columns.Title];
                        else
                            companyTitle = "";

                        DataRow newDR = dtResult.NewRow();
                        newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.EstimateNo];
                        newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                        newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                        newDR[DatabaseObjects.Columns.CRMCompanyLookup] = companyTitle; //dr[DatabaseObjects.Columns.CRMCompanyLookup];

                        newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                        newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                        newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMProjectStatus];
                        newDR[DatabaseObjects.Columns.Module] = uHelper.getModuleNameByTicketId(Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                        newDR[DatabaseObjects.Columns.StreetAddress1] = dr[DatabaseObjects.Columns.StreetAddress1];
                        newDR[DatabaseObjects.Columns.City] = dr[DatabaseObjects.Columns.City];
                        dtResult.Rows.Add(newDR);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Request["dtStart"])) && !string.IsNullOrEmpty(Convert.ToString(Request["dtEnd"])))
            {
                DateTime dtStart;
                DateTime.TryParse(Convert.ToString(Request["dtStart"]), out dtStart);

                DateTime dtEnd;
                if (DateTime.TryParse(Convert.ToString(Request["dtEnd"]), out dtEnd))
                {
                    // Changing Time 12AM to 11:59 PM to include all time value for a specific date     
                    dtEnd = dtEnd.AddMinutes(1439);
                }

                //DataRow[] drResults = dtResult.Select(string.Format("{0} <= '#" + dtEnd + "#' AND {0} >= '#" + dtStart + "#'", DatabaseObjects.Columns.TicketCreationDate));
                DataRow[] drResults = dtResult.Select($"{DatabaseObjects.Columns.TicketCreationDate} >= #{dtStart}# and {DatabaseObjects.Columns.TicketCreationDate} <= #{dtEnd}#");
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
            reportHelper.companyLogo = companyLogo;
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            if (dtReport == null)
            {
                dtReport = LoadData();
            }
            grdReport.DataSource = dtReport;
            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Business Unit Distribution Report", 8F, "xls", null, null);
            reportHelper.WriteXlsToResponse(Response, "BusinessUnitDistributionReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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

            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Business Unit Distribution Report", 6.75F, "pdf", null, null);
            reportHelper.WritePdfToResponse(Response, "BusinessUnitDistributionReport" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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
