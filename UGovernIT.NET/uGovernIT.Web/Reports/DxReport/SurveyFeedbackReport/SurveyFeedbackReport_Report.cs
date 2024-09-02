using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class SurveyFeedbackReport_Report : DevExpress.XtraReports.UI.XtraReport
    {
        public List<string> ratingnColumns;
        public string surveytype;
        public string surveyis;
         
        public SurveyFeedbackReport_Report(DataTable reporttable, string type, string survey)
        {
            if (reporttable == null || reporttable.Rows.Count == 0)
                return;
            InitializeComponent();
            surveytype = type;
            surveyis = survey;

            xrLabelReportHeader.Text = surveyis; 
            
            List<string> nonRatingColumns = new List<string>() { "Ticket ID", "Title", "Average Rating", "Submitted By", "Submit Date", "PRP", "Owner", "Category", "ID", "ModuleName" };
            ratingnColumns = reporttable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Except(nonRatingColumns).ToList();

            float titleWidth = 688;


            if (ratingnColumns.Count > 0)
            {
                xrTableHeadRating1.Text = ratingnColumns[0];
                xrTableRating1.DataBindings.Add("Text", null, ratingnColumns[0]);


                xrTableFooterRating1.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating1.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating1.Summary.FormatString = "{0:F2}";
                xrTableFooterRating1.DataBindings.Add("Text", null, ratingnColumns[0], "{0:F2}");

                titleWidth -= 86;
            }
            if (ratingnColumns.Count > 1)
            {
                xrTableHeadRating2.Text = ratingnColumns[1];
                xrTableRating2.DataBindings.Add("Text", null, ratingnColumns[1]);

                xrTableFooterRating2.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating2.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating2.Summary.FormatString = "{0:F2}";
                xrTableFooterRating2.DataBindings.Add("Text", null, ratingnColumns[1], "{0:F2}");


                titleWidth -= 86;
            }
            if (ratingnColumns.Count > 2)
            {
                xrTableHeadRating3.Text = ratingnColumns[2];
                xrTableRating3.DataBindings.Add("Text", null, ratingnColumns[2]);

                xrTableFooterRating3.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating3.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating3.Summary.FormatString = "{0:F2}";
                xrTableFooterRating3.DataBindings.Add("Text", null, ratingnColumns[2], "{0:F2}");
                titleWidth -= 86;
            }
            if (ratingnColumns.Count > 3)
            {
                xrTableHeadRating4.Text = ratingnColumns[3];
                xrTableRating4.DataBindings.Add("Text", null, ratingnColumns[3]);

                xrTableFooterRating4.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating4.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating4.Summary.FormatString = "{0:F2}";
                xrTableFooterRating4.DataBindings.Add("Text", null, ratingnColumns[3], "{0:F2}");

                titleWidth -= 86;
            }

            if (ratingnColumns.Count > 4)
            {
                xrTableHeadRating5.Text = ratingnColumns[4];
                xrTableRating5.DataBindings.Add("Text", null, ratingnColumns[4]);

                xrTableFooterRating5.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating5.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating5.Summary.FormatString = "{0:F2}";
                xrTableFooterRating5.DataBindings.Add("Text", null, ratingnColumns[4], "{0:F2}");

                titleWidth -= 86;
            }
            if (ratingnColumns.Count > 5)
            {
                xrTableHeadRating6.Text = ratingnColumns[5];
                xrTableRating6.DataBindings.Add("Text", null, ratingnColumns[5]);

                xrTableFooterRating6.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating6.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating6.Summary.FormatString = "{0:F2}";
                xrTableFooterRating6.DataBindings.Add("Text", null, ratingnColumns[5], "{0:F2}");

                titleWidth -= 86;
            }
            if (ratingnColumns.Count > 6)
            {
                xrTableHeadRating7.Text = ratingnColumns[6];
                xrTableRating7.DataBindings.Add("Text", null, ratingnColumns[6]);

                xrTableFooterRating7.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating7.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating7.Summary.FormatString = "{0:F2}";
                xrTableFooterRating7.DataBindings.Add("Text", null, ratingnColumns[6], "{0:F2}");

                titleWidth -= 86;
            }
            if (ratingnColumns.Count > 7)
            {
                xrTableHeadRating8.Text = ratingnColumns[7];
                xrTableRating8.DataBindings.Add("Text", null, ratingnColumns[7]);

                xrTableFooterRating8.Summary.Running = SummaryRunning.Report;
                xrTableFooterRating8.Summary.Func = SummaryFunc.Avg;
                xrTableFooterRating8.Summary.FormatString = "{0:F2}";
                xrTableFooterRating8.DataBindings.Add("Text", null, ratingnColumns[7], "{0:F2}");

                titleWidth -= 86;
            }



            if (titleWidth < 73)
            {
                titleWidth = 73.51f;
            }
            float increasewidthby = 0.0f;
            if (ratingnColumns.Count > 0)
            {
                increasewidthby = titleWidth / (8 + ratingnColumns.Count);
            }


            //float controlwidth = xrTableTitle.WidthF;
            //xrTableTitle.WidthF = controlwidth + increasewidthby;
            //xrTableHeadTitle.WidthF = controlwidth + increasewidthby;


            //controlwidth = xrTableTicketId.WidthF;
            //xrTableHeadTicketid.WidthF = controlwidth + increasewidthby;
            //xrTableTicketId.WidthF = controlwidth + increasewidthby;


            //xrTableFooterAverage.WidthF = xrTableTitle.WidthF + xrTableTicketId.WidthF;

            //controlwidth = xrTableHeadRating1.WidthF;
            //xrTableHeadRating1.WidthF = controlwidth + increasewidthby;
            //xrTableRating1.WidthF = controlwidth + increasewidthby;
            //xrTableFooterRating1.WidthF = controlwidth + increasewidthby;


            //controlwidth = xrTableAverageRating.WidthF;
            //xrTableAverageRating.WidthF = controlwidth + increasewidthby;
            //xrTableHeadAverageRating.WidthF = controlwidth + increasewidthby;
            //xrTableFooterAvgRating.WidthF = controlwidth + increasewidthby;

            //controlwidth = xrTableSubmittedby.WidthF;
            //xrTableSubmittedby.WidthF = controlwidth + increasewidthby;
            //xrTableHeadSubmittedBy.WidthF = controlwidth + increasewidthby;

            //controlwidth = xrTableSubmitDate.WidthF;
            //xrTableSubmitDate.WidthF = controlwidth + increasewidthby;
            //xrTableHeadSubmitdate.WidthF = controlwidth + increasewidthby;


            //controlwidth = xrTablePRP.WidthF;
            //xrTablePRP.WidthF = controlwidth + increasewidthby;
            //xrTableHeadPRP.WidthF = controlwidth + increasewidthby;

            //controlwidth = xrTableOwner.WidthF;
            //xrTableOwner.WidthF = controlwidth + increasewidthby;
            //xrTableHeadOwner.WidthF = controlwidth + increasewidthby;

            //controlwidth = xrTableCategory.WidthF;
            //xrTableCategory.WidthF = controlwidth + increasewidthby;
            //xrTableHeadCategory.WidthF = controlwidth + increasewidthby;


            //xrTableCell1.WidthF = xrTableHeadSubmittedBy.WidthF + xrTableSubmitDate.WidthF + xrTablePRP.WidthF + xrTableOwner.WidthF + xrTableCategory.WidthF;
            xrTableFooterAvgRating.Summary.Running = SummaryRunning.Report;
            xrTableFooterAvgRating.Summary.Func = SummaryFunc.Avg;
            xrTableFooterAvgRating.Summary.FormatString = "{0:F2}";
            xrTableFooterAvgRating.DataBindings.Add("Text", null, "Average Rating");

            //xrTableFooterAverage.Summary.Running = SummaryRunning.Report;
            //xrTableFooterAverage.Summary.Func = SummaryFunc.Avg;
            //xrTableFooterAverage.Summary.FormatString = "Average {0:F2}";
            //xrTableFooterAverage.DataBindings.Add("Text", null, "Average Rating", "{0:F2}");

            xrTableTicketId.DataBindings.Add("Text", null, "Ticket ID");
            xrTableTitle.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrTableAverageRating.DataBindings.Add("Text", null, "Average Rating");

          


            xrTableSubmittedby.DataBindings.Add("Text", null, "Submitted By");
            xrTableSubmitDate.DataBindings.Add("Text", null, "Submit Date", uGITFormatConstants.DateFormat);
            xrTablePRP.DataBindings.Add("Text", null, "PRP");
            xrTableOwner.DataBindings.Add("Text", null, DatabaseObjects.Columns.Owner);
            xrTableCategory.DataBindings.Add("Text", null, DatabaseObjects.Columns.Category);


            this.Report.DataSource = reporttable;
        }

        private void xrTableRow3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = sender as XRTableRow;
            xrTable2.SuspendLayout();
            if (ratingnColumns.Count > 0)
            {
                for (int i = ratingnColumns.Count + 1; i <= 8; i++)
                {
                    row.Cells.Remove(row.Cells[string.Format("xrTableHeadRating{0}", i)]);
                }
            }
            xrTable2.PerformLayout();
        }

        private void xrTable1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTable tbl = sender as XRTable;
            tbl.SuspendLayout();
            foreach (XRTableRow row in tbl.Rows)
            {
                for (int i = ratingnColumns.Count + 1; i <= 8; i++)
                {
                    XRTableCellCollection cellcoll = row.Cells;
                    XRControl ctr = (XRControl)cellcoll[string.Format("xrTableRating{0}", i)];
                    if (ctr != null)
                        row.Cells.Remove(row.Cells[string.Format("xrTableRating{0}", i)]);
                }
            }
            tbl.PerformLayout();
        }

        private void xrTable3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTable tbl = sender as XRTable;
            tbl.SuspendLayout();
            foreach (XRTableRow row in tbl.Rows)
            {
                for (int i = ratingnColumns.Count + 1; i <= 8; i++)
                {
                    XRTableCellCollection cellcoll = row.Cells;
                    XRControl ctr = (XRControl)cellcoll[string.Format("xrTableFooterRating{0}", i)];
                    if (ctr != null)
                        row.Cells.Remove(row.Cells[string.Format("xrTableFooterRating{0}", i)]);
                }
            }
            tbl.PerformLayout();
        }

        private void xrLabelReportHeader_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            string prefix = surveytype == "Generic" ? "Generic:" : "Module:";


            xrLabelReportHeader.Text = string.Format("{0}",  surveyis);
        }
    }
}
