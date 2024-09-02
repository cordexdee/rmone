using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace uGovernIT.Report.DxReport
{
    public partial class RequestSummaryReport : DevExpress.XtraReports.UI.XtraReport
    {

        public RequestSummaryReport(DataTable dtRequestSummary)
        {
            InitializeComponent();
            xrTCRequestTitle.DataBindings.Add("Text", null, "ReuestTitle");
            xrTCRequestCount.DataBindings.Add("Text", null, "RequestsCount");
            Report.DataSource = dtRequestSummary;
        }

       
    }
}
