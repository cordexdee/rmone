using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using uGovernIT.Utility;
namespace uGovernIT.DxReport
{
    public partial class PlanReport : DevExpress.XtraReports.UI.XtraReport
    {
        public PlanReport(DataTable dataTable)
        {
            InitializeComponent();
            xrTCPlanNum.Summary.Func = SummaryFunc.RecordNumber;
            xrTCPlanNum.Summary.FormatString = uGITFormatConstants.RecordNumberFormat;
            xrTCPlanNum.Summary.Running = SummaryRunning.Report;

            xrTCPlanTitle.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrTCPlanProjectsNotes.DataBindings.Add("Text", null, DatabaseObjects.Columns.ProjectNote);
           // xrTCPlanCreatedOn.DataBindings.Add("Text", null, DatabaseObjects.Columns.Created, uGITFormatConstants.DateFormat);
            xrTCPlannedDate.DataBindings.Add("Text", null, DatabaseObjects.Columns.UGITEndDate, uGITFormatConstants.DateFormat);
            this.DataSource = dataTable;
        }

        private void xrTCPlannedDate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DateTime date = UGITUtility.StringToDateTime(xrTCPlannedDate.Text);
            if (date != DateTime.MinValue && date < DateTime.Today)
            {
                xrTCPlannedDate.ForeColor = Color.Red;
                xrTCPlannedDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else
            {
                xrTCPlannedDate.ForeColor = Color.Black;
                xrTCPlannedDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

    }
}
