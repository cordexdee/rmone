using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Collections.Generic;
using System.Drawing.Printing;
using DevExpress.XtraPrinting;
using System.Text;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class TicketSummary_Report : DevExpress.XtraReports.UI.XtraReport
    {

        public TicketSummary_Report(string reportType, DataTable dtOpenTickets, DataTable dtDateWiseData, Dictionary<string, string> dicStatus, List<ChartEntity> chartUrl, DataTable dtRequestsSummary)
        {
          
            InitializeComponent();
            xrLabelHeader.Text = reportType + " Tickets Report";
            CreateDynamicTables(dtOpenTickets, dtDateWiseData, reportType);
            if (dtOpenTickets != null && dtOpenTickets.Rows.Count > 0)
            {
                Report.DataSource = dtOpenTickets;
            }
            else
            {
                xrTable2.Visible = false;
                xrTable1.Visible = false;
            }
            BindStatus(dicStatus);

            if (chartUrl != null && chartUrl.Count > 0)
            {
                foreach (ChartEntity chartEntityObj in chartUrl)
                {
                    List<ChartEntity> lst = new List<ChartEntity>();
                    lst.Add(chartEntityObj);
                    ChartSubReport chartReport = new ChartSubReport(lst);
                    XRSubreport xrSubreport1 = new XRSubreport();
                    xrSubreport1.LocationF = new PointF(chartEntityObj.Left, chartEntityObj.Top + 284);
                    List<int> rows = chartEntityObj.Row;
                    xrSubreport1.SizeF = new System.Drawing.SizeF(chartEntityObj.Width, chartEntityObj.Height);
                    xrSubreport1.ReportSource = chartReport;

                    ReportHeader1.Controls.Add(xrSubreport1);
                }
            }
            if (dtRequestsSummary != null && dtRequestsSummary.Rows.Count > 0)
            {
                RequestSummaryReport requestSummaryReport = new RequestSummaryReport(dtRequestsSummary);
                xrSubreport2.ReportSource = requestSummaryReport;
            }
        }

        private void CreateDynamicTables(DataTable dtOpenTickets, DataTable dtDateWiseData, string reportType)
        {
            if (dtOpenTickets != null && dtOpenTickets.Rows.Count > 0)
            {
                xrTCAge.BeforePrint += xrTCAge_BeforePrint;
                xrTCAge.DataBindings.Add("Text", null, "Age");
                xrTCCategory.DataBindings.Add("Text", null, "RequestTypeCategory");
                xrTCPriority.DataBindings.Add("Text", null, "PriorityLookup");
                xrTCProgress.DataBindings.Add("Text", null, "Status");
                xrTCRequestor.DataBindings.Add("Text", null, "RequestorUser");
                xrTCTargetDate.BeforePrint += XrTCTargetDate_BeforePrint;
                xrTCTargetDate.DataBindings.Add("Text", null, "DesiredCompletionDate", "{0:MMM-dd-yyyy}");
                xrTCTicketId.DataBindings.Add("Text", null, "TicketId");
                xrTCTicketId.DataBindings.Add("NavigateUrl", null, "TicketDialogUrl");
                xrTCTitle.DataBindings.Add("Text", null, "Title");
                xrTCDepartment.DataBindings.Add("Text", null, "DepartmentLookup");
                xrTCLocation.DataBindings.Add("Text", null, "LocationLookup");
                xrTCWaitingOn.DataBindings.Add("Text", null, "StageActionUsersUser");
                if (dtOpenTickets.Columns["ModuleName"] != null)
                {
                    grpTickets.GroupFields.Add(new GroupField("ModuleName"));
                }
            }

            xrTable3.Visible = true;

            if (dtDateWiseData != null && dtDateWiseData.Rows.Count >= 1)
            {
                xrTCCurrentDate.Text = Convert.ToString(dtDateWiseData.Rows[0]["SelectedDate"]);
                xrTCCurrentIssue.Text = Convert.ToString(dtDateWiseData.Rows[0]["OldestIssue"]);
                xrTCCurrentAvgDays.Text = Convert.ToString(dtDateWiseData.Rows[0]["AvgDaysOpen"]);
                xrTCCurrentMaxDays.Text = Convert.ToString(dtDateWiseData.Rows[0]["MaxDaysOpen"]);
                xrTCCurrentOpenTickets.Text = Convert.ToString(dtDateWiseData.Rows[0]["OpenTicketsCount"]);
                xrTCCurrentClosed.Text = Convert.ToString(dtDateWiseData.Rows[0]["ClosedTicketsCount"]);
                xrTCQCCurrent.Text = Convert.ToString(dtDateWiseData.Rows[0]["QuickClosedTicketsCount"]);
            }

            if (dtDateWiseData != null && dtDateWiseData.Rows.Count >= 2)
            {
                xrTCPreviousDate.Text = Convert.ToString(dtDateWiseData.Rows[1]["SelectedDate"]);
                xrTCPreviousIssue.Text = Convert.ToString(dtDateWiseData.Rows[1]["OldestIssue"]);
                xrTCPreviousAvgDays.Text = Convert.ToString(dtDateWiseData.Rows[1]["AvgDaysOpen"]);
                xrTCPreviousMaxDays.Text = Convert.ToString(dtDateWiseData.Rows[1]["MaxDaysOpen"]);
                xrTCPreviousOpenTickets.Text = Convert.ToString(dtDateWiseData.Rows[1]["OpenTicketsCount"]);
                xrTCPreviousClosed.Text = Convert.ToString(dtDateWiseData.Rows[1]["ClosedTicketsCount"]);
                xrTCQCPrevious.Text = Convert.ToString(dtDateWiseData.Rows[1]["QuickClosedTicketsCount"]);
            }

            if (dtDateWiseData != null && dtDateWiseData.Rows.Count >= 3)
            {
                xrTCVariance.Text = Convert.ToString(dtDateWiseData.Rows[2]["SelectedDate"]);
                xrTCVarianceIssue.Text = Convert.ToString(dtDateWiseData.Rows[2]["OldestIssue"]);
                xrTCVarianceAvgDays.Text = Convert.ToString(dtDateWiseData.Rows[2]["AvgDaysOpen"]);
                xrTCVarianceMaxDays.Text = Convert.ToString(dtDateWiseData.Rows[2]["MaxDaysOpen"]);
                xrTCVarianceOpenTickets.Text = Convert.ToString(dtDateWiseData.Rows[2]["OpenTicketsCount"]);
                xrTCVarianceClosed.Text = Convert.ToString(dtDateWiseData.Rows[2]["ClosedTicketsCount"]);
                xrTCVarianceQC.Text = Convert.ToString(dtDateWiseData.Rows[2]["QuickClosedTicketsCount"]);
            }
        }

         void XrTCTargetDate_BeforePrint(object sender, PrintEventArgs e)
        {
            if (xrTCTargetDate.Text != null && xrTCTargetDate.Text != string.Empty)
            {
                xrTCTargetDate.Text = Convert.ToDateTime(xrTCTargetDate.Text).ToString("MMM-dd-yyyy");

            }
        }

        void xrTCAge_BeforePrint(object sender, PrintEventArgs e)
        {
            if(xrTCAge.Text!=null && xrTCAge.Text!=string.Empty)
            GetAgeBar(Convert.ToInt32(xrTCAge.Text));
        }


        private void ProgressBar_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            e.DisplayText = xrTCProgress.Text;
        }


        public void GetAgeBar(int age)
        {
            if (age > 0)
            {
                xrTCAge.BackColor = Color.FromArgb(245, 201, 217);
            }
            else
            {
                xrTCAge.BackColor = Color.FromArgb(201, 245, 242);
            }

        }

        private void BindStatus(Dictionary<string, string> dicStatus)
        {
            List<string> keys = new List<string>(dicStatus.Keys);
            List<string> values = new List<string>(dicStatus.Values);
            xrLblStatus1.Text = keys[0];
            xrLblStatus2.Text = keys[1];
            xrLblStatus3.Text = keys[2];
            xrLblStatus4.Text = keys[3];
            xrLblStatusCount1.Text = values[0];
            xrLblStatusCount2.Text = values[1];
            xrLblStatusCount3.Text = values[2];
            xrLblStatusCount4.Text = values[3];
        }
    }
}
