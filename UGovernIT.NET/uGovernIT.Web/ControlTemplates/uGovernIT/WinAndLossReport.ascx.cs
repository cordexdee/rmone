using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using DevExpress.XtraReports.UI;
using System.Web;
using ReportEntity = uGovernIT.Manager;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class WinAndLossReport : UserControl
    {
        DataTable dtReport;
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public bool JobSummary { get; set; }
        private string companyLogo;
        public FieldConfiguration field = null;

        protected ApplicationContext ApplicationContext = HttpContext.Current.GetManagerContext();

        protected ConfigurationVariableManager ConfigurationVariableManager;

        protected FieldConfigurationManager fieldConfigurationManager;

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {

            ConfigurationVariableManager = new ConfigurationVariableManager(ApplicationContext);
            fieldConfigurationManager = new FieldConfigurationManager(ApplicationContext);
            companyLogo = ConfigurationVariableManager.GetValue(ConfigConstants.ReportLogo);
            // JobSummary = uHelper.StringToBoolean(Request["JobSummary"]);

            //grdReport.Columns[DatabaseObjects.Columns.CRMProjectStatus].Visible = JobSummary;
            //grdReport.Columns[DatabaseObjects.Columns.TicketCreationDate].Visible = JobSummary;
            //grdReport.Columns[DatabaseObjects.Columns.Estimator].Visible = JobSummary;
            //if (!JobSummary)
            //    (grdReport.Columns[DatabaseObjects.Columns.Estimator] as GridViewDataColumn).UnGroup();
            //grdReport.Columns[DatabaseObjects.Columns.TicketComment].Visible = !JobSummary;




        }

        protected override void OnPreRender(EventArgs e)
        {
            grdWinLossReport.DataBind();
            base.OnPreRender(e);
        }

        #endregion

        #region Custom Events

        private DataTable LoadData()
        {
            DataTable dtResult = new DataTable();

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketId))
                dtResult.Columns.Add(DatabaseObjects.Columns.TicketId);

                dtResult.Columns.Add(DatabaseObjects.Columns.Title);

                dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(double));

                dtResult.Columns.Add(DatabaseObjects.Columns.CRMOpportunityStatus);

                dtResult.Columns.Add(DatabaseObjects.Columns.AwardedLossDate, typeof(DateTime));

                dtResult.Columns.Add(DatabaseObjects.Columns.Reason);

                dtResult.Columns.Add(DatabaseObjects.Columns.MarketSector);

                dtResult.Columns.Add("CMFirm");

                dtResult.Columns.Add("CMContact");

                dtResult.Columns.Add("ArchitectFirm");

                dtResult.Columns.Add("ArchitectContact");

                dtResult.Columns.Add("DeveloperEndUserFirm");

                dtResult.Columns.Add("DeveloperEndUserContact");

                dtResult.Columns.Add("BrokerFirm");

                dtResult.Columns.Add("BrokerContact");



            //get all realtedcompanys.
            //DataTable dtRelatedCompanies = SPListHelper.GetDataTable(DatabaseObjects.Lists.RelatedCompanies);
            string query = string.Format("{0} = {1} ", 1, 1);
            DataTable dtRelatedCompanies = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RelatedCompanies, $"{query} and TenantID='{ApplicationContext.TenantID}'"); //spList.GetItems(spQuery);


            //DataTable dtOPM = uGITCache.ModuleDataCache.GetAllTickets(uHelper.getModuleIdByModuleName("OPM"));
            TicketManager ticketManager = new TicketManager(ApplicationContext);
            ModuleViewManager moduleViewManager = new ModuleViewManager(ApplicationContext);
            CRMRelationshipTypeManager crmrelationshiptypemanager = new CRMRelationshipTypeManager(ApplicationContext);
            UGITModule module = moduleViewManager.LoadByName("OPM");
            DataTable dtOPM = ticketManager.GetAllTickets(module);

            List<CRMRelationshipType> crmrelationshiptype = null;

            if (dtOPM != null && dtOPM.Rows.Count > 0)
            {
                DataRow[] drOPM = dtOPM.Select(string.Format("{0} = '{1}' OR {0} = '{2}' OR {0} = '{3}' ", DatabaseObjects.Columns.CRMOpportunityStatus, "Awarded", "Declined", "Lost"));
                if (drOPM != null && drOPM.Length > 0)
                {
                    foreach (DataRow dr in drOPM)
                    {
                        DataRow newDR = dtResult.NewRow();
                        newDR[DatabaseObjects.Columns.TicketId] = dr[DatabaseObjects.Columns.TicketId];
                        newDR[DatabaseObjects.Columns.Title] = dr[DatabaseObjects.Columns.Title];
                        newDR[DatabaseObjects.Columns.ApproxContractValue] = dr[DatabaseObjects.Columns.ApproxContractValue];
                        newDR[DatabaseObjects.Columns.CRMOpportunityStatus] = dr[DatabaseObjects.Columns.CRMOpportunityStatus]; //string.Join(", ", uHelper.GetMultiLookupValue(Convert.ToString(dr[DatabaseObjects.Columns.TicketOwner])));
                        newDR[DatabaseObjects.Columns.AwardedLossDate] = dr[DatabaseObjects.Columns.AwardedLossDate];
                        newDR[DatabaseObjects.Columns.Reason] = dr[DatabaseObjects.Columns.Reason];
                        newDR[DatabaseObjects.Columns.MarketSector] = dr[DatabaseObjects.Columns.MarketSector];

                        if (dtRelatedCompanies != null && dtRelatedCompanies.Rows.Count > 0)
                        {
                            //CRMRelationshipType crmrelationshiptype = new CRMRelationshipType();
                            //CRMRelationshipTypeManager crmrelationshiptypemanager = new CRMRelationshipTypeManager(ApplicationContext);

                            //List<CRMRelationshipType>  crmrelationshiptype = crmrelationshiptypemanager.Load($"{ DatabaseObjects.Columns.Title} like '%Construction Manager%'")    ;
                            crmrelationshiptype = crmrelationshiptypemanager.Load($"{ DatabaseObjects.Columns.Title} like '%Construction Manager%'");
                            DataRow[] drRelatedCompanies = dtRelatedCompanies.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, Convert.ToString(dr[DatabaseObjects.Columns.TicketId])));

                            if (drRelatedCompanies != null && drRelatedCompanies.Length > 0)
                            {
                                DataRow[] rowCMFirm = null;
                                if (crmrelationshiptype != null)
                                {
                                     rowCMFirm = drRelatedCompanies.CopyToDataTable().Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.RelationshipTypeLookup, crmrelationshiptype[0].ID));
                                }

                                if (rowCMFirm != null && rowCMFirm.Length > 0)
                                {
                                    string strCMFirm = string.Empty;
                                    string strCMContact = string.Empty;

                                    foreach (DataRow item in rowCMFirm)
                                    {
                                        if (!string.IsNullOrEmpty(strCMFirm))
                                            strCMFirm += ", ";

                                        if (!string.IsNullOrEmpty(strCMContact))
                                            strCMContact += ", ";

                                        strCMFirm += GetCompanyName(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyLookup])); //Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                                        strCMContact += GetContactName(Convert.ToString(item[DatabaseObjects.Columns.ContactLookup])); //Convert.ToString(item[DatabaseObjects.Columns.ContactLookup]);
                                    }
                                    newDR["CMFirm"] = strCMFirm;
                                    newDR["CMContact"] = strCMContact;
                                }

                                crmrelationshiptype = crmrelationshiptypemanager.Load($"{ DatabaseObjects.Columns.Title} like '%Architect%'");
                                DataRow[] rowArchitectFirm = null;
                                if (crmrelationshiptype != null )
                                {
                                     rowArchitectFirm = drRelatedCompanies.CopyToDataTable().Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.RelationshipTypeLookup, crmrelationshiptype[0].ID));
                                }
                                

                                if (rowArchitectFirm != null && rowArchitectFirm.Length > 0)
                                {
                                    string strArchitectFirm = string.Empty;
                                    string strArchitectContact = string.Empty;

                                    foreach (DataRow item in rowArchitectFirm)
                                    {
                                        if (!string.IsNullOrEmpty(strArchitectFirm))
                                            strArchitectFirm += ", ";

                                        if (!string.IsNullOrEmpty(strArchitectContact))
                                            strArchitectContact += ", ";

                                        //strArchitectFirm += string.Join(", ", uHelper.GetMultiLookupValue(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyTitleLookup])));
                                        //strArchitectContact += string.Join(", ", uHelper.GetMultiLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ContactLookup])));

                                        strArchitectFirm += GetCompanyName(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyLookup])); //string.Join(", ", uHelper.GetMultiLookupValue(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyTitleLookup])));
                                        strArchitectContact += GetContactName(Convert.ToString(item[DatabaseObjects.Columns.ContactLookup])); //string.Join(", ", uHelper.GetMultiLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ContactLookup])));
                                    }
                                    newDR["ArchitectFirm"] = strArchitectFirm;
                                    newDR["ArchitectContact"] = strArchitectContact;
                                }


                                crmrelationshiptype = crmrelationshiptypemanager.Load($"{ DatabaseObjects.Columns.Title} like '%Developer%'");
                                DataRow[] rowDeveloperEndUserFirm = null;
                                if (crmrelationshiptype != null)
                                {
                                    rowDeveloperEndUserFirm = drRelatedCompanies.CopyToDataTable().Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.RelationshipTypeLookup, crmrelationshiptype[0].ID));
                                }
                                if (rowDeveloperEndUserFirm != null && rowDeveloperEndUserFirm.Length > 0)
                                {
                                    string strDeveloperEndUserFirm = string.Empty;
                                    string strDeveloperEndUserContact = string.Empty;

                                    foreach (DataRow item in rowDeveloperEndUserFirm)
                                    {
                                        if (!string.IsNullOrEmpty(strDeveloperEndUserFirm))
                                            strDeveloperEndUserFirm += ", ";

                                        if (!string.IsNullOrEmpty(strDeveloperEndUserContact))
                                            strDeveloperEndUserContact += ", ";

                                        strDeveloperEndUserFirm += GetCompanyName(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyLookup])); //Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                                        strDeveloperEndUserContact += GetContactName(Convert.ToString(item[DatabaseObjects.Columns.ContactLookup])); //Convert.ToString(item[DatabaseObjects.Columns.ContactLookup]);
                                    }
                                    newDR["DeveloperEndUserFirm"] = strDeveloperEndUserFirm;
                                    newDR["DeveloperEndUserContact"] = strDeveloperEndUserContact;
                                }

                                crmrelationshiptype = crmrelationshiptypemanager.Load($"{ DatabaseObjects.Columns.Title} like '%Broker%'");
                                DataRow[] rowBrokerFirm = null;
                                if (crmrelationshiptype != null)
                                {
                                     rowBrokerFirm = drRelatedCompanies.CopyToDataTable().Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.RelationshipTypeLookup, crmrelationshiptype[0].ID));
                                }
                                if (rowBrokerFirm != null && rowBrokerFirm.Length > 0)
                                {
                                    string strBrokerFirm = string.Empty;
                                    string strBrokerContact = string.Empty;

                                    foreach (DataRow item in rowBrokerFirm)
                                    {
                                        if (!string.IsNullOrEmpty(strBrokerFirm))
                                            strBrokerFirm += ", ";

                                        if (!string.IsNullOrEmpty(strBrokerContact))
                                            strBrokerContact += ", ";

                                        strBrokerFirm += GetCompanyName(Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyLookup])); //Convert.ToString(item[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                                        strBrokerContact += GetContactName(Convert.ToString(item[DatabaseObjects.Columns.ContactLookup])); //Convert.ToString(item[DatabaseObjects.Columns.ContactLookup]);
                                    }
                                    newDR["BrokerFirm"] = strBrokerFirm;
                                    newDR["BrokerContact"] = strBrokerContact;
                                }

                            }
                        }

                        dtResult.Rows.Add(newDR);
                    }
                }
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Request["dtStart"])) && !string.IsNullOrEmpty(Convert.ToString(Request["dtEnd"])))
            {
                DateTime dtStart;
                DateTime.TryParse(Convert.ToString(Request["dtStart"]), out dtStart);
                string f = Convert.ToString(Request["dtStart"]);
                
                DateTime dtEnd;
                if (DateTime.TryParse(Convert.ToString(Request["dtEnd"]), out dtEnd))
                {
                    //Changing Time 12AM to 11:59 PM to include all time value for a specific date

                   dtEnd = dtEnd.AddMinutes(1439);
                }

               // DataRow[] drResults = dtResult.Select(string.Format("{0} <= '#" + dtStart + "#' AND {0} >= '#" + dtEnd + "#'", DatabaseObjects.Columns.AwardedLossDate));
                DataRow[] drResults = dtResult.Select($"{DatabaseObjects.Columns.AwardedLossDate} >= #{dtStart}# and {DatabaseObjects.Columns.AwardedLossDate} <= #{dtEnd}#");

                if (drResults != null && drResults.Length > 0)
                    dtResult = drResults.CopyToDataTable();
                else
                    dtResult = null;
            }


            DataView dv = new DataView(dtResult);
            if(dtResult != null)
            {
                dv.Sort = $"{DatabaseObjects.Columns.CRMOpportunityStatus}, AwardedorLossDate,Title ASC";
                dtResult = dv.ToTable();
            }
           
            return dtResult;
        }

        private string GetCompanyName(string CompanyLookup)
        {
            if (string.IsNullOrEmpty(CompanyLookup))
                return string.Empty;

            return fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.CRMCompanyLookup, CompanyLookup);
        }

        private string GetContactName(string ContactLookup)
        {
            if (string.IsNullOrEmpty(ContactLookup))
                return string.Empty;

            return fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.ContactLookup, ContactLookup);
        }
               
        #endregion

        #region Grid Events

        protected void grdWinLossReport_DataBinding(object sender, EventArgs e)
        {
            if (dtReport == null)
            {
                dtReport = LoadData();
                grdWinLossReport.DataSource = dtReport;
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
            grdWinLossReport.DataSource = dtReport;

            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;

            XtraReport report = reportHelper.GenerateReport(grdWinLossReport, (DataTable)grdWinLossReport.DataSource, "OPM Wins and Losses Report", 8F, "xls", null, qFormat);
            reportHelper.WriteXlsToResponse(Response, "OPMWinsAndLossesReport" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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
            grdWinLossReport.DataSource = dtReport;

            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;

            XtraReport report = reportHelper.GenerateReport(grdWinLossReport, (DataTable)grdWinLossReport.DataSource, "OPM Wins and Losses Report", 6.75F, "pdf", null, qFormat);
            reportHelper.WritePdfToResponse(Response, "OPMWinsAndLossesReport" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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

    }
}