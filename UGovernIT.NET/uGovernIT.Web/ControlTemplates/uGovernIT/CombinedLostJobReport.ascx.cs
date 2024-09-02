using DevExpress.Web;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using ReportEntity = uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class CombinedLostJobReport :UserControl
    {
        DataTable dtReport;      
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        public bool JobSummary { get; set; }

        private string companyLogo = string.Empty;

        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        
       
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
            companyLogo= ConfigurationVariableManager.GetValue(ConfigConstants.ReportLogo);//check logo code

            JobSummary = UGITUtility.StringToBoolean(Request["JobSummary"]);

            grdReport.Columns[DatabaseObjects.Columns.CRMProjectStatus].Visible = JobSummary;
            grdReport.Columns[DatabaseObjects.Columns.TicketCreationDate].Visible = JobSummary;
            grdReport.Columns[DatabaseObjects.Columns.Estimator].Visible = JobSummary;
            if(!JobSummary)
            (grdReport.Columns[DatabaseObjects.Columns.Estimator] as GridViewDataColumn).UnGroup();
            grdReport.Columns[DatabaseObjects.Columns.TicketComment].Visible = !JobSummary;


           

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
                dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue,typeof(double));

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                dtResult.Columns.Add(DatabaseObjects.Columns.TicketOwner);

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.AwardedLossDate))
                dtResult.Columns.Add(DatabaseObjects.Columns.AwardedLossDate,typeof(DateTime));

           
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

             
        

            

            DataRow[] drs = null;
            if (JobSummary)
            {
                // DataTable dtOPM = TicketManager.GetAllTickets(uHelper.getModuleIdByModuleName("OPM"));
               var  dtOPM= TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.OPM));
                drs = dtOPM.Select();
            }
            else
            {
                var dtOPM = TicketManager.GetClosedTickets(ModuleViewManager.GetByName(ModuleNames.OPM));
                drs = dtOPM.Select($"{ DatabaseObjects.Columns.CRMOpportunityStatus} ='Lost'");
            }

            if (drs != null && drs.Length > 0)            
            {
                foreach (DataRow dr in drs)
                {
                    DataRow newDR = dtResult.NewRow();
                    newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.TicketId];
                    newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                    newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                    newDR[DatabaseObjects.Columns.TicketOwner] =string.Join(", ", uHelper.GetMultiLookupValue(Convert.ToString(dr[DatabaseObjects.Columns.TicketOwner])));
                    newDR[DatabaseObjects.Columns.AwardedLossDate] = dr[DatabaseObjects.Columns.TicketCloseDate];
                    newDR[DatabaseObjects.Columns.TicketComment] = dr[DatabaseObjects.Columns.Reason];
                    newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                    newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                    newDR[DatabaseObjects.Columns.Estimator] = dr[DatabaseObjects.Columns.Estimator];
                    newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMOpportunityStatus];
                    dtResult.Rows.Add(newDR);
                }
            }

            DataTable dtCPR = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CPR));
            DataTable dtCNS = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CNS));

            //DataTable dtCPR = TicketManager.GetAllTickets(uHelper.getModuleIdByModuleName(ApplicationContext,ModuleNames.CPR));
            //DataTable dtCNS = TicketManager.GetAllTickets(uHelper.getModuleIdByModuleName(ApplicationContext, ModuleNames.CNS));
            if (dtCPR != null && dtCNS != null)
            {
                dtCPR.Merge(dtCNS);
            }
            else if (dtCNS != null)
            {
                dtCPR = dtCNS;
            }


            if (dtCPR != null)
            {
                DataRow[] drsCPR = null;
                if (JobSummary)
                    drsCPR = dtCPR.Select();
                else
                    drsCPR = dtCPR.Select(string.Format("{0} ='Lost'", DatabaseObjects.Columns.CRMProjectStatus));
                
                if (drsCPR != null && drsCPR.Length > 0)
                {
                    foreach (DataRow dr in drsCPR)
                    {
                        DataRow newDR = dtResult.NewRow();
                        newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.EstimateNo];
                        newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                        newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                        newDR[DatabaseObjects.Columns.TicketOwner] = string.Join(", ", uHelper.GetMultiLookupValue(Convert.ToString(dr[DatabaseObjects.Columns.Estimator]))); //dr[DatabaseObjects.Columns.Estimator];
                        newDR[DatabaseObjects.Columns.AwardedLossDate] = dr[DatabaseObjects.Columns.AwardedLossDate];
                        string value = Convert.ToString(dr[DatabaseObjects.Columns.TicketComment]);
                        if (!string.IsNullOrEmpty(value))
                        {
                            string[] vals = value.Split(new string[]{ ";#" },StringSplitOptions.RemoveEmptyEntries);
                            if (vals != null && vals.Length > 0)
                            {
                                newDR[DatabaseObjects.Columns.TicketComment] = vals[vals.Length - 1];
                            }
                        }
                        newDR[DatabaseObjects.Columns.DivisionLookup] = dr[DatabaseObjects.Columns.DivisionLookup];
                        newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                        newDR[DatabaseObjects.Columns.Estimator] = dr[DatabaseObjects.Columns.Estimator];
                        newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMProjectStatus];
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

                DataRow[] drResults = dtResult.Select(string.Format("{0} <= '#" + dtEnd + "#' AND {0} >= '#" + dtStart + "#'", DatabaseObjects.Columns.AwardedLossDate));
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
            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Combined Lost Job Report", 8F, "xls", null, null);
            reportHelper.WriteXlsToResponse(Response, "CombinedLostJobReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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

            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Combined Lost Job Report", 6.75F, "pdf", null, null);
            reportHelper.WritePdfToResponse(Response, "CombinedLostJobReport" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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