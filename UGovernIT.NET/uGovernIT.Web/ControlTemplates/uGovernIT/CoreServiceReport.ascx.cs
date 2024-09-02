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
    public partial class CoreServiceReport :UserControl
    {
        public string Module { get; set; }
        public string Type { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        DataTable dtReport;

        private string companyLogo = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private ApplicationContext _context = null;
        private TicketManager _ticketManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private FieldConfigurationManager _fmanger = null;

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
                if (_fmanger == null)
                {
                    _fmanger = new FieldConfigurationManager(ApplicationContext);
                }
                return _fmanger;
            }
        }
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            companyLogo = ConfigurationVariableManager.GetValue(ConfigConstants.ReportLogo);
            //SPQuery query = new SPQuery();
            //query.Query = "<Where></Where>";
            //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, PublicTicketID);
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/><FieldRef Name='{3}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ModuleNameLookup, DatabaseObjects.Columns.CheckListTemplateLookup);
            // query.ViewFieldsOnly = true;
            //DataTable dtCRMProject = SPListHelper.GetDataTable(DatabaseObjects.Lists.CRMProject, query);
            //if (dtCRMProject != null && dtCRMProject.Rows.Count > 0)
            //{
            //}
        }

        protected override void OnPreRender(EventArgs e)
        {
            grdReport.DataBind();
            base.OnPreRender(e);
        }

        #endregion

        #region Custom Events

        private DataTable CreateTableSchema()
        {
            if (string.IsNullOrEmpty(Module))
                return null;

            DataTable dtResult = TicketManager.GetAllTickets(ModuleViewManager.GetByName(Module));

            DataView view = new DataView(dtResult);
            DataTable distinctCRMBU = view.ToTable(true, DatabaseObjects.Columns.DivisionLookup);

            #region Table Schema Definition

            DataTable coreServiceSchema = new DataTable();
            coreServiceSchema.Columns.Add(DatabaseObjects.Columns.Type, typeof(string));

            if (Type == "Detailed")
                coreServiceSchema.Columns.Add(DatabaseObjects.Columns.Estimator, typeof(string));

            List<string> lstColumns = new List<string>();
            foreach (DataRow dr in distinctCRMBU.Rows)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr[0])))
                {
                    coreServiceSchema.Columns.Add(new DataColumn() { ColumnName = string.Format("Count-{0}", Convert.ToString(dr[0])), DataType = typeof(double), Caption = Convert.ToString(dr[0]) });
                    coreServiceSchema.Columns.Add(new DataColumn() { ColumnName = string.Format("Amount-{0}", Convert.ToString(dr[0])), DataType = typeof(double), Caption = Convert.ToString(dr[0]) });
                    lstColumns.Add(string.Format("[Amount-{0}]", Convert.ToString(dr[0])));
                }
            }

            coreServiceSchema.Columns.Add("GrandTotal", typeof(double), string.Join(" + ", lstColumns.ToArray()));

            #endregion

            #region Load Data
            foreach (DataRow dr in dtResult.Rows)
            {
                string pType = string.Empty;
                if (Convert.ToString(dr[DatabaseObjects.Columns.TicketStatus]) == "Cancelled")
                {
                    pType = "Loss/Cancelled";
                }
                else if (Convert.ToString(dr[DatabaseObjects.Columns.TicketStatus]) == "Closed")
                {
                    pType = "Closed";
                }
                else if (Convert.ToInt16(dr[DatabaseObjects.Columns.StageStep]) > 6 && Convert.ToInt16(dr[DatabaseObjects.Columns.StageStep]) <= 9)
                {
                    pType = "Awarded";
                }
                else
                {
                    pType = "Estimating";
                }

                DataRow dataRow = coreServiceSchema.NewRow();
                dataRow[DatabaseObjects.Columns.Type] = pType;

                if (Type == "Detailed")
                {
                    //string[] estimators = uHelper.GetMultiLookupValue(Convert.ToString(dr[DatabaseObjects.Columns.Estimator]));
                    //dataRow[DatabaseObjects.Columns.Estimator] = string.Join(", ", estimators);
                    if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.Estimator])))
                        dataRow[DatabaseObjects.Columns.Estimator] = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.Estimator, Convert.ToString(dr[DatabaseObjects.Columns.Estimator]));
                    else
                        dataRow[DatabaseObjects.Columns.Estimator] = string.Empty;
                }
                dataRow["GrandTotal"] = 0;

                foreach (DataRow drBC in distinctCRMBU.Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(drBC[0])))
                    {

                        if (Convert.ToString(dr[DatabaseObjects.Columns.DivisionLookup]) == Convert.ToString(drBC[0]) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.ApproxContractValue])))
                        {
                            dataRow[string.Format("Amount-{0}", Convert.ToString(drBC[0]))] = dr[DatabaseObjects.Columns.ApproxContractValue];
                            dataRow[string.Format("Count-{0}", Convert.ToString(drBC[0]))] = 1;
                        }
                        else
                        {
                            dataRow[string.Format("Amount-{0}", Convert.ToString(drBC[0]))] = 0;
                            dataRow[string.Format("Count-{0}", Convert.ToString(drBC[0]))] = 0;
                        }

                    }
                }
                coreServiceSchema.Rows.Add(dataRow);
            }

            var rows = coreServiceSchema.AsEnumerable();
            var columns = coreServiceSchema.Columns.Cast<DataColumn>();

            string columnToGroup = Type == "Detailed" ? string.Format("{0},{1}", DatabaseObjects.Columns.Type, DatabaseObjects.Columns.Estimator) : DatabaseObjects.Columns.Type;
            DataColumn[] colToGroup = columns.Where(c => columnToGroup.Contains(c.ColumnName)).ToArray();
            var colsToSum = columns.Where(c => c.ColumnName.Contains("Amount") || c.ColumnName.Contains("Count"));
            var columnsToSum = new HashSet<DataColumn>(colsToSum);

            DataTable resultTable = coreServiceSchema.Clone(); // empty table, same schema

            if (colToGroup.Count() == 2)
            {
                foreach (var groupType in rows.GroupBy(r => r[colToGroup[0]]))
                {
                    foreach (var group in groupType.GroupBy(r => r[colToGroup[1]]))
                    {
                        DataRow row = resultTable.Rows.Add();
                        foreach (var col in columns)
                        {
                            if (columnsToSum.Contains(col))
                            {
                                double sum = group.Sum(r => r.Field<double?>(col) == null ? 0 : r.Field<double>(col));
                                row[col.ColumnName] = sum.ToString("N2");
                            }
                            else
                            {
                                if (col.ColumnName == colToGroup[0].ColumnName)
                                    row[col.ColumnName] = groupType.First()[col];
                                else if (col.ColumnName != "GrandTotal")
                                    row[col.ColumnName] = group.First()[col];
                            }
                        }
                    }
                }
            }
            else
            {

                foreach (var group in rows.GroupBy(r => r[colToGroup[0]]))
                {
                    DataRow row = resultTable.Rows.Add();
                    foreach (var col in columns)
                    {
                        if (columnsToSum.Contains(col))
                        {
                            double sum = group.Sum(r => r.Field<double?>(col) == null ? 0 : r.Field<double>(col));
                            row[col.ColumnName] = sum.ToString("N2");
                        }
                        else
                        {
                            if (col.ColumnName != "GrandTotal")
                                row[col.ColumnName] = group.First()[col];
                        }
                    }
                }

            }

            DataView dataView = resultTable.AsDataView();
            dataView.Sort = Type == "Summary" ? string.Format("{0} ASC", DatabaseObjects.Columns.Type) : string.Format("{0} ASC,{1} ASC", DatabaseObjects.Columns.Type, DatabaseObjects.Columns.Estimator);
            coreServiceSchema = dataView.ToTable();

            #endregion

            GenerateColumns(coreServiceSchema);

            return coreServiceSchema;
        }

        protected void GenerateColumns(DataTable dtReport)
        {

            if (grdReport.Columns.Count == 0)
            {

                if (dtReport != null && dtReport.Columns.Count > 0)
                {
                    foreach (DataColumn dc in dtReport.Columns)
                    {


                        GridViewDataTextColumn dataTextColumn = new GridViewDataTextColumn();
                        dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        if (dc.ColumnName == DatabaseObjects.Columns.Estimator || dc.ColumnName == DatabaseObjects.Columns.Type)
                        {
                            dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        }
                        else if (dc.ColumnName.Contains("Amount") || dc.ColumnName == "GrandTotal")
                        {
                            dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            dataTextColumn.PropertiesEdit.DisplayFormatString = "{0:c0}";
                        }

                        dataTextColumn.HeaderStyle.HorizontalAlign = dc.ColumnName == DatabaseObjects.Columns.Estimator ? HorizontalAlign.Left : HorizontalAlign.Center;
                        dataTextColumn.FieldName = dc.ColumnName;
                        dataTextColumn.Name = dc.ColumnName;

                        if (dc.ColumnName == "Type" && Type == "Detailed")
                            dataTextColumn.GroupIndex = 0;

                        if (dc.ColumnName.Contains("Count") || dc.ColumnName.Contains("Amount"))
                        {

                            dataTextColumn.Caption = dc.ColumnName.Contains("Amount") ? "Amount" : "Count";
                            dataTextColumn.Name = dc.ColumnName.Contains("Amount") ? "Amount" : "Count";


                            GridViewBandColumn bndColumn = grdReport.AllColumns.FirstOrDefault(m => m as GridViewBandColumn != null && m.Caption == dc.Caption) as GridViewBandColumn;
                            if (bndColumn == null)
                            {
                                bndColumn = new GridViewBandColumn();
                                bndColumn.Name = dc.Caption;
                                bndColumn.Caption = dc.Caption;
                                bndColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                bndColumn.HeaderStyle.Font.Bold = true;
                                grdReport.Columns.Add(bndColumn);
                            }
                            bndColumn.Columns.Add(dataTextColumn);
                        }
                        else
                        {
                            dataTextColumn.Caption = dc.Caption;
                            grdReport.Columns.Add(dataTextColumn);
                        }
                    }
                }
            }
        }

        #endregion

        #region Grid Events

        protected void grdReport_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void grdReport_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Estimator)
            {
                var values = Convert.ToString(e.CellValue);
                if (FieldConfigurationManager.GetFieldByFieldName(e.DataColumn.FieldName) != null)
                {
                    e.Cell.Text = FieldConfigurationManager.GetFieldConfigurationData(e.DataColumn.FieldName, values);                    
                }
            }
        }

        protected void grdReport_DataBinding(object sender, EventArgs e)
        {
            if (dtReport == null)
            {
                dtReport = CreateTableSchema();
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
            dtReport = CreateTableSchema();
            grdReport.DataSource = dtReport;

            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;

            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Executive Summary", 8F, "xls", null, qFormat);
            reportHelper.WriteXlsToResponse(Response, "ExecutiveSummary" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
            reportHelper.companyLogo = companyLogo;
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;

            dtReport = CreateTableSchema();
            grdReport.DataSource = dtReport;


            ReportQueryFormat qFormat = new ReportQueryFormat();
            qFormat.ShowDateInFooter = true;
            qFormat.ShowCompanyLogo = true;

            XtraReport report = reportHelper.GenerateReport(grdReport, (DataTable)grdReport.DataSource, "Executive Summary", 6.75F, "pdf", null, qFormat);
            reportHelper.WritePdfToResponse(Response, "ExecutiveSummary" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
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