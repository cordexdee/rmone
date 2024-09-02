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
using uGovernIT.Report.Helpers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using ReportEntity = uGovernIT.Manager;

namespace uGovernIT.Report.DxReport
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
        public string delegateControl = UGITUtility.GetAbsoluteURL("BuildReport.aspx");

        public bool JobSummary { get; set; }

        private string companyLogo = null;

        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private FieldConfigurationManager _fieldConfigurationManager = null;
        private UserProfileManager _userProfileManager = null;
        private FieldConfiguration field = null;

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

            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                //ReportFilterURl = ApplicationContext.ReportUrl + delegateControl + "?reportName=CombinedLostJobReport&Filter=Filter&Module=" + Module + "&title=" + ReportTitle;
                ReportFilterURl = delegateControl + "?reportName=CombinedLostJobReport&Filter=Filter&Module=" + Module + "&title=" + ReportTitle;

                Response.Redirect(ReportFilterURl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //_context = Session["context"] as ApplicationContext;

            companyLogo = ConfigurationVariableManager.GetValue(ConfigConstants.CompanyLogo);//check logo code

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

            //if (!dtResult.Columns.Contains(DatabaseObjects.Columns.EstimateNo))
            //    dtResult.Columns.Add(DatabaseObjects.Columns.EstimateNo);

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

                if (!dtResult.Columns.Contains("ERPJobID"))
                    dtResult.Columns.Add("ERPJobID");

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
                    dtResult.Columns.Add(DatabaseObjects.Columns.Title);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ApproxContractValue))
                    dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(double));

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMowner))
                    dtResult.Columns.Add(DatabaseObjects.Columns.CRMowner);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.AwardedLossDate))
                    dtResult.Columns.Add(DatabaseObjects.Columns.AwardedLossDate, typeof(DateTime));


                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketComment))
                    dtResult.Columns.Add(DatabaseObjects.Columns.TicketComment);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMBusinessUnit))
                    dtResult.Columns.Add(DatabaseObjects.Columns.CRMBusinessUnit);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                    dtResult.Columns.Add(DatabaseObjects.Columns.TicketCreationDate, typeof(DateTime));

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Estimator))
                    dtResult.Columns.Add(DatabaseObjects.Columns.Estimator);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMProjectStatus))
                    dtResult.Columns.Add(DatabaseObjects.Columns.CRMProjectStatus);

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Studio))
                    dtResult.Columns.Add(DatabaseObjects.Columns.Studio);

                //DataTable drs = null;
                //string tableName = ModuleViewManager.GetModuleTableName(ModuleNames.OPM);

                //if (JobSummary)
                //{
                //    // DataTable dtOPM = TicketManager.GetAllTickets(uHelper.getModuleIdByModuleName("OPM"));
                //    //drs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.OPM));

                //    drs = GetTableDataManager.GetTableData(tableName, query, "ProjectId,TicketId,Title,ApproxContractValue,CRMownerUser,AwardedorLossDate,CloseDate,Reason,CRMBusinessUnitChoice,CreationDate,EstimatorUser,CRMOpportunityStatusChoice,StudioChoice", null);
                //}
                //else
                //{
                //    /*
                //    var dtOPM = TicketManager.GetClosedTickets(ModuleViewManager.GetByName(ModuleNames.OPM));//check query,taking time to fetch the data
                //    drs = dtOPM.Select($"{ DatabaseObjects.Columns.CRMOpportunityStatus} ='Lost'");//tenantid
                //    */
                //    drs = GetTableDataManager.GetTableData(tableName, $"{DatabaseObjects.Columns.Closed} = 1 and {DatabaseObjects.Columns.CRMOpportunityStatus} = 'Lost' and {query}", "ProjectId,TicketId,Title,ApproxContractValue,CRMownerUser,AwardedorLossDate,CloseDate,Reason,CRMBusinessUnitChoice,CreationDate,EstimatorUser,CRMOpportunityStatusChoice,StudioChoice", null);

                //}

                //if (drs != null && drs.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in drs.Rows)
                //    {
                //        DataRow newDR = dtResult.NewRow();
                //        //newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.TicketId];
                //        newDR[DatabaseObjects.Columns.CRMProjectID] = !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMProjectID])) ? dr[DatabaseObjects.Columns.CRMProjectID] : dr[DatabaseObjects.Columns.TicketId];
                //        newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                //        newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                //        if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMowner])))
                //            newDR[DatabaseObjects.Columns.CRMowner] = string.Join(", ", uHelper.GetMultiLookupValue(FieldConfigurationManager.GetFieldConfigurationData(field, Convert.ToString(dr[DatabaseObjects.Columns.CRMowner]))));
                //        else
                //            newDR[DatabaseObjects.Columns.CRMowner] = dr[DatabaseObjects.Columns.CRMowner];
                //        //newDR[DatabaseObjects.Columns.AwardedLossDate] = dr[DatabaseObjects.Columns.TicketCloseDate];
                //        newDR[DatabaseObjects.Columns.AwardedLossDate] = dr[DatabaseObjects.Columns.AwardedLossDate] != DBNull.Value ? dr[DatabaseObjects.Columns.AwardedLossDate] : dr[DatabaseObjects.Columns.TicketCloseDate];

                //        newDR[DatabaseObjects.Columns.TicketComment] = dr[DatabaseObjects.Columns.Reason];
                //        newDR[DatabaseObjects.Columns.CRMBusinessUnit] = dr[DatabaseObjects.Columns.CRMBusinessUnit];
                //        newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                //        //newDR[DatabaseObjects.Columns.Estimator] = dr[DatabaseObjects.Columns.Estimator];
                //        newDR[DatabaseObjects.Columns.Estimator] = string.Join(", ", uHelper.GetMultiLookupValue(FieldConfigurationManager.GetFieldConfigurationData(field, Convert.ToString(dr[DatabaseObjects.Columns.Estimator]))));
                //        newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMOpportunityStatus];
                //        newDR[DatabaseObjects.Columns.Studio] = Convert.ToString(dr[DatabaseObjects.Columns.Studio]);
                //        dtResult.Rows.Add(newDR);
                //    }
                //}

                ///*
                //DataTable dtCPR = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CPR));
                //DataTable dtCNS = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CNS));

                ////DataTable dtCPR = TicketManager.GetAllTickets(uHelper.getModuleIdByModuleName(ApplicationContext,ModuleNames.CPR));
                ////DataTable dtCNS = TicketManager.GetAllTickets(uHelper.getModuleIdByModuleName(ApplicationContext, ModuleNames.CNS));
                //if (dtCPR != null && dtCNS != null)
                //{
                //    dtCPR.Merge(dtCNS, false, MissingSchemaAction.Ignore);
                //}
                //else if (dtCNS != null)
                //{
                //    dtCPR = dtCNS;
                //}
                //*/

                //DataTable dtCPR = null;
                //tableName = ModuleViewManager.GetModuleTableName(ModuleNames.CPR);
                //if (JobSummary)
                //{
                //    dtCPR = GetTableDataManager.GetTableData(tableName, query, "ProjectId,TicketId,Title,ApproxContractValue,OwnerUser,AwardedorLossDate,CloseDate,Reason,CRMBusinessUnitChoice,CreationDate,EstimatorUser,CRMProjectStatusChoice,StudioChoice,Comment", null);
                //}
                //else
                //{
                //    dtCPR = GetTableDataManager.GetTableData(tableName, $"{DatabaseObjects.Columns.CRMProjectStatus} = 'Lost' and {query}", "ProjectId,TicketId,Title,ApproxContractValue,OwnerUser,AwardedorLossDate,CloseDate,Reason,CRMBusinessUnitChoice,CreationDate,EstimatorUser,CRMProjectStatusChoice,StudioChoice,Comment", null);
                //}

                //DataTable dtCNS = null;
                //tableName = ModuleViewManager.GetModuleTableName(ModuleNames.CNS);
                //if (JobSummary)
                //{
                //    dtCNS = GetTableDataManager.GetTableData(tableName, query, "ProjectId,TicketId,Title,ApproxContractValue,OwnerUser,AwardedorLossDate,CloseDate,Reason,CRMBusinessUnitChoice,CreationDate,EstimatorUser,CRMProjectStatusChoice,StudioChoice,Comment", null);
                //}
                //else
                //{
                //    dtCNS = GetTableDataManager.GetTableData(tableName, $"{DatabaseObjects.Columns.CRMProjectStatus} = 'Lost' and {query}", "ProjectId,TicketId,Title,ApproxContractValue,OwnerUser,AwardedorLossDate,CloseDate,Reason,CRMBusinessUnitChoice,CreationDate,EstimatorUser,CRMProjectStatusChoice,StudioChoice,Comment", null);
                //}

                //if (dtCPR != null && dtCPR.Rows.Count > 0 && dtCNS != null && dtCNS.Rows.Count > 0)
                //{
                //    dtCPR.Merge(dtCNS, false, MissingSchemaAction.Ignore);
                //}
                //else if (dtCNS != null)
                //{
                //    dtCPR = dtCNS;
                //}


                ////if (dtCPR != null && dtCPR.Rows.Count > 0)
                ////{
                ////    DataRow[] drsCPR = null;
                ////    if (JobSummary)
                ////        drsCPR = dtCPR.Select();
                ////    else
                ////        drsCPR = dtCPR.Select(string.Format("{0} ='Lost'", DatabaseObjects.Columns.CRMProjectStatus));//string

                //if (dtCPR != null && dtCPR.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dtCPR.Rows)
                //    {
                //        DataRow newDR = dtResult.NewRow();
                //        //newDR[DatabaseObjects.Columns.EstimateNo] = dr[DatabaseObjects.Columns.EstimateNo];
                //        newDR[DatabaseObjects.Columns.CRMProjectID] = !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.CRMProjectID])) ? dr[DatabaseObjects.Columns.CRMProjectID] : dr[DatabaseObjects.Columns.TicketId];
                //        newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                //        newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                //        if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.Estimator])))
                //            newDR[DatabaseObjects.Columns.CRMowner] = string.Join(", ", uHelper.GetMultiLookupValue(FieldConfigurationManager.GetFieldConfigurationData(field, Convert.ToString(dr[DatabaseObjects.Columns.Estimator])))); //dr[DatabaseObjects.Columns.Estimator];
                //        else
                //            newDR[DatabaseObjects.Columns.CRMowner] = dr[DatabaseObjects.Columns.Estimator];
                //        newDR[DatabaseObjects.Columns.AwardedLossDate] = dr[DatabaseObjects.Columns.AwardedLossDate];
                //        string value = Convert.ToString(dr[DatabaseObjects.Columns.TicketComment]);
                //        if (!string.IsNullOrEmpty(value))
                //        {
                //            string[] vals = value.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                //            if (vals != null && vals.Length > 0)
                //            {
                //                newDR[DatabaseObjects.Columns.TicketComment] = vals[vals.Length - 1];
                //            }
                //        }
                //        newDR[DatabaseObjects.Columns.CRMBusinessUnit] = dr[DatabaseObjects.Columns.CRMBusinessUnit];
                //        newDR[DatabaseObjects.Columns.TicketCreationDate] = dr[DatabaseObjects.Columns.TicketCreationDate];
                //        //newDR[DatabaseObjects.Columns.Estimator] = dr[DatabaseObjects.Columns.Estimator];
                //        newDR[DatabaseObjects.Columns.Estimator] = string.Join(", ", uHelper.GetMultiLookupValue(FieldConfigurationManager.GetFieldConfigurationData(field, Convert.ToString(dr[DatabaseObjects.Columns.Estimator]))));
                //        newDR[DatabaseObjects.Columns.CRMProjectStatus] = dr[DatabaseObjects.Columns.CRMProjectStatus];
                //        newDR[DatabaseObjects.Columns.Studio] = Convert.ToString(dr[DatabaseObjects.Columns.Studio]);
                //        dtResult.Rows.Add(newDR);
                //    }
                //}
                ////}

                ///*
                //if (!string.IsNullOrEmpty(Convert.ToString(Request["dtStart"])) && !string.IsNullOrEmpty(Convert.ToString(Request["dtEnd"])))
                //{
                //    DateTime dtStart;
                //    DateTime.TryParse(Convert.ToString(Request["dtStart"]), out dtStart);

                //    DateTime dtEnd;
                //    if (DateTime.TryParse(Convert.ToString(Request["dtEnd"]), out dtEnd))
                //    {
                //        // Changing Time 12AM to 11:59 PM to include all time value for a specific date     
                //        dtEnd = dtEnd.AddMinutes(1439);
                //    }

                //    DataRow[] drResults = dtResult.Select($"{DatabaseObjects.Columns.AwardedLossDate} >= #{dtStart}# and {DatabaseObjects.Columns.AwardedLossDate} <= #{dtEnd}#");
                //    if (drResults != null && drResults.Length > 0)
                //        dtResult = drResults.CopyToDataTable();
                //    else
                //        dtResult = null;
                //}
                //*/

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("@TenantID", _context.TenantID);
                values.Add("@StartDate", dtStart.ToShortDateString());
                values.Add("@Enddate", dtEnd.ToShortDateString());
                values.Add("@JobSummary", JobSummary);

                dtResult = DAL.uGITDAL.ExecuteDataSetWithParameters("Usp_GetRptCombinedReport", values);
                //DataView dv = new DataView(dtResult);
                //dv.RowFilter = query; // query
                //dtResult = dv.ToTable();
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

