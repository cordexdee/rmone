
using DevExpress.XtraReports.UI;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class AccompolishmentReport : XtraReport
    {
        public AccompolishmentReport(DataTable dataTable)
        {
            InitializeComponent();

            xrTCAccNum.Summary.Func = SummaryFunc.RecordNumber;
            xrTCAccNum.Summary.FormatString = uGITFormatConstants.RecordNumberFormat;
            xrTCAccNum.Summary.Running = SummaryRunning.Report;

            xrTCTitle.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrTCNote.DataBindings.Add("Text", null, DatabaseObjects.Columns.ProjectNote);
            xrTCCompletedOn.DataBindings.Add("Text", null, DatabaseObjects.Columns.AccomplishmentDate, uGITFormatConstants.DateFormat);
            this.DataSource = dataTable;
        }

    }
}
