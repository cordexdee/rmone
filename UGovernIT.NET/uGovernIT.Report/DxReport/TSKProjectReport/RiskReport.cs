using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class RiskReport : DevExpress.XtraReports.UI.XtraReport
    {
        public RiskReport(DataTable dataTable)
        {
            InitializeComponent();

            foreach (DataRow drow in dataTable.Rows)
            {
                drow[DatabaseObjects.Columns.RiskProbability] = Convert.ToDouble(drow[DatabaseObjects.Columns.RiskProbability]) / 100;
            }
            
            xrTCRiskNum.Summary.Func = SummaryFunc.RecordNumber;
            xrTCRiskNum.Summary.FormatString = uGITFormatConstants.RecordNumberFormat;
            xrTCRiskNum.Summary.Running = SummaryRunning.Report;

            xrTCRiskTitle.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrTCMitigationPlan.DataBindings.Add("Text", null, DatabaseObjects.Columns.MitigationPlan);
            xrTCContingencyPlan.DataBindings.Add("Text", null, DatabaseObjects.Columns.ContingencyPlan);
            xrTCProbability.DataBindings.Add("Text", null, DatabaseObjects.Columns.RiskProbability, uGITFormatConstants.PrecentFormat);
            xrTCImpact.DataBindings.Add("Text", null, DatabaseObjects.Columns.IssueImpact);
            xrAssignedTo.DataBindings.Add("Text",null,DatabaseObjects.Columns.AssignedTo);
            this.DataSource = dataTable;
        }

    }
}
