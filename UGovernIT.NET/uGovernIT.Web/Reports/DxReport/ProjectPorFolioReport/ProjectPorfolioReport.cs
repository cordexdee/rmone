using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using uGovernIT.Manager.Entities;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Reports
{
    public partial class ProjectPorfolioReport : DevExpress.XtraReports.UI.XtraReport
    {
        private int year;
        private string yeartype = string.Empty;

        public ProjectPorfolioReport(DataTable portfolioReport, int nyear, string yearType, bool isPercentage)
        {
            year = nyear;
            yeartype = yearType;
            InitializeComponent();

            xrTClProjectID.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketId);
            xrTClProject.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrTClResources.DataBindings.Add("Text", null, "Resources");
            xrTClPriority.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketPriorityLookup);
            xrTCIStatus.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketStatus);
            xrTaCIBudget.DataBindings.Add("Text", null, DatabaseObjects.Columns.BudgetAmount, "{0:C}");

            xrTClActual.DataBindings.Add("Text", null, "Actual", "{0:C}");

            xrTCIStartDate.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketActualStartDate, uGITFormatConstants.DateFormat);
            xrTEndDate.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketActualCompletionDate, uGITFormatConstants.DateFormat);

            xrLblTitle.PrintOnPage += xrLblTitle_PrintOnPage;

            //subtotal lbls update::start
            //budget
            xrTotallbl.Summary.Running = SummaryRunning.Report;
            xrTotallbl.Summary.Func = SummaryFunc.Sum;
            xrTotallbl.Summary.FormatString = "{0:C}";
            xrTotallbl.DataBindings.Add("Text", null, DatabaseObjects.Columns.BudgetAmount, "{0:C}");

            //actual
            xrActaulSTLbl.Summary.Running = SummaryRunning.Report;
            xrActaulSTLbl.Summary.Func = SummaryFunc.Sum;
            xrActaulSTLbl.Summary.FormatString = "{0:C}";
            xrActaulSTLbl.DataBindings.Add("Text", null, "Actual", "{0:C}");

            //Q1
            xrQ1STLbl.Summary.Running = SummaryRunning.Report;
            xrQ1STLbl.Summary.Func = SummaryFunc.Sum;

            //Q2
            xrQ2STLb.Summary.Running = SummaryRunning.Report;
            xrQ2STLb.Summary.Func = SummaryFunc.Sum;

            //Q3
            xrQ3STLbl.Summary.Running = SummaryRunning.Report;
            xrQ3STLbl.Summary.Func = SummaryFunc.Sum;

            //Q4
            xrQ4STLbl.Summary.Running = SummaryRunning.Report;
            xrQ4STLbl.Summary.Func = SummaryFunc.Sum;

            if (isPercentage == true)
            {
                xrTClResourceDemandQ1.DataBindings.Add("Text", null, "Q1", "{0:F1}%");
                xrTClResourceDemandQ2.DataBindings.Add("Text", null, "Q2", "{0:F1}%");
                xrTClResourceDemandQ3.DataBindings.Add("Text", null, "Q3", "{0:F1}%");
                xrTClResourceDemandQ4.DataBindings.Add("Text", null, "Q4", "{0:F1}%");

                xrQ1STLbl.DataBindings.Add("Text", null, "Q1", "{0:F1}%");
                xrQ1STLbl.Summary.FormatString = "{0:F1}%";

                xrQ2STLb.DataBindings.Add("Text", null, "Q2", "{0:F1}%");
                xrQ2STLb.Summary.FormatString = "{0:F1}%";

                xrQ3STLbl.DataBindings.Add("Text", null, "Q3", "{0:F1}%");
                xrQ3STLbl.Summary.FormatString = "{0:F1}%";

                xrQ4STLbl.DataBindings.Add("Text", null, "Q4", "{0:F1}%");
                xrQ4STLbl.Summary.FormatString = "{0:F1}%";
            }
            else
            {
                xrTClResourceDemandQ1.DataBindings.Add("Text", null, "Q1", "{0:F1}");
                xrTClResourceDemandQ2.DataBindings.Add("Text", null, "Q2", "{0:F1}");
                xrTClResourceDemandQ3.DataBindings.Add("Text", null, "Q3", "{0:F1}");
                xrTClResourceDemandQ4.DataBindings.Add("Text", null, "Q4", "{0:F1}");

                xrQ1STLbl.DataBindings.Add("Text", null, "Q1", "{0:F1}");
                xrQ1STLbl.Summary.FormatString = "{0:F1}";

                xrQ2STLb.DataBindings.Add("Text", null, "Q2", "{0:F1}");
                xrQ2STLb.Summary.FormatString = "{0:F1}";

                xrQ3STLbl.DataBindings.Add("Text", null, "Q3", "{0:F1}");
                xrQ3STLbl.Summary.FormatString = "{0:F1}";

                xrQ4STLbl.DataBindings.Add("Text", null, "Q4", "{0:F1}");
                xrQ4STLbl.Summary.FormatString = "{0:F1}";
            }

            this.Report.DataSource = portfolioReport;
        }

        void xrLblTitle_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (yeartype == "Fiscal Year")
                xrLblTitle.Text = string.Format("Project Portfolio Report ({0} {1})", yeartype, year);
            else
                xrLblTitle.Text = string.Format("Project Portfolio Report ({0})", year);
        }

        //global for all
        private string DoStringFormate(double sbtotal, string strformate)
        {
            return String.Format(strformate, sbtotal);
        }
    }
}
