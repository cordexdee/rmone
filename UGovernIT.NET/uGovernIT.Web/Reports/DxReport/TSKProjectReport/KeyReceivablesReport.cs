using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace uGovernIT.DxReport
{
    public partial class KeyReceivablesReport : DevExpress.XtraReports.UI.XtraReport
    {
        public KeyReceivablesReport(DataTable dataTable)
        {
            InitializeComponent();

            xrTCKRSrNo.Summary.Func = SummaryFunc.RecordNumber;
            xrTCKRSrNo.Summary.FormatString = "{0:#}";
            xrTCKRSrNo.Summary.Running = SummaryRunning.Report;

            xrTCKRTitle.DataBindings.Add("Text", null, "Title");
            xrTCKRAssignedTo.DataBindings.Add("Text", null, "AssignedTo");
            xrTCKRTargetDate.DataBindings.Add("Text", null, "DueDate", "{0:MMM-dd-yyyy}");
            xrTCKRStatus.DataBindings.Add("Text", null, "Status");

            this.DataSource = dataTable;
        }

        private void xrTCKRTargetDate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToDateTime(xrTCKRTargetDate.Text) < DateTime.Now && !(string.Compare(xrTCKRStatus.Text.Trim(), "Completed", true) == 0))
            {
                xrTCKRTargetDate.ForeColor = Color.Red;
                xrTCKRTargetDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

    }
}
