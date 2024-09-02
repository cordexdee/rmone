using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace uGovernIT.Report.DxReport
{
    public partial class KeyDeliverables : DevExpress.XtraReports.UI.XtraReport
    {
        public KeyDeliverables(DataTable dataTable)
        {
            InitializeComponent();

            xrTCKDSrNo.Summary.Func = SummaryFunc.RecordNumber;
            xrTCKDSrNo.Summary.FormatString = "{0:#}";
            xrTCKDSrNo.Summary.Running = SummaryRunning.Report;

            xrTCKDTitle.DataBindings.Add("Text", null, "Title");
            xrTCKDAssignedTo.DataBindings.Add("Text", null, "AssignedTo");
            xrTCKDTargetDate.DataBindings.Add("Text", null, "DueDate", "{0:MMM-dd-yyyy}");
            xrTCKDStatus.DataBindings.Add("Text", null, "Status");
            this.DataSource = dataTable;
        }

        private void xrTCKDTargetDate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToDateTime(xrTCKDTargetDate.Text) < DateTime.Now && !(string.Compare(xrTCKDStatus.Text.Trim(), "Completed", true) == 0))
            {
                xrTCKDTargetDate.ForeColor = Color.Red;
                xrTCKDTargetDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }


    }
}
