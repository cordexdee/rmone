using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class IssuesReport : DevExpress.XtraReports.UI.XtraReport
    {
        public IssuesReport(DataTable dataTable)
        {
            InitializeComponent();

            xrTCIssuesNum.Summary.Func = SummaryFunc.RecordNumber;
            xrTCIssuesNum.Summary.FormatString = uGITFormatConstants.RecordNumberFormat;
            xrTCIssuesNum.Summary.Running = SummaryRunning.Report;

            xrTCIssues.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrTCICreatedOn.DataBindings.Add("Text", null, DatabaseObjects.Columns.Created, uGITFormatConstants.DateFormat);
            xrTCIDueDate.DataBindings.Add("Text", null, DatabaseObjects.Columns.DueDate, uGITFormatConstants.DateFormat);
            xrTCIssueAssignedTo.DataBindings.Add("Text", null, DatabaseObjects.Columns.AssignedTo);
            xrTCIStatus.DataBindings.Add("Text", null, DatabaseObjects.Columns.Status);
            xrTCPriority.DataBindings.Add("Text", null, DatabaseObjects.Columns.TaskPriority);
            xrResolution.DataBindings.Add("Text", null, DatabaseObjects.Columns.UGITResolution);
            //xrTCImpact.DataBindings.Add("Text", null, DatabaseObjects.Columns.IssueImpact);

            this.DataSource = dataTable;
        }

    }
}
