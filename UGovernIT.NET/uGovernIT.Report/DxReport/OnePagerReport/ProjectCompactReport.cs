using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class ProjectCompactReport : DevExpress.XtraReports.UI.XtraReport
    {
        ProjectCompactReportEntity entity;
        public ProjectCompactReport(ProjectCompactReportEntity pEntity)
        {
            InitializeComponent();
            entity = pEntity;
            DoBinding();
        }

        private void DoBinding()
        {
            #region project basic details
            if (entity.ProjectDetails != null && entity.ProjectDetails.Rows.Count > 0)
            {
                xrProject.DataBindings.Add("Text", entity.ProjectDetails, DatabaseObjects.Columns.Title);
                xrWeekEnding.DataBindings.Add("Text", entity.ProjectDetails, "WeekEnding");
                xrBusinessOwner.DataBindings.Add("Text", entity.ProjectDetails, DatabaseObjects.Columns.TicketBusinessManager);
                xrITOwner.DataBindings.Add("Text", entity.ProjectDetails, DatabaseObjects.Columns.TicketOwner);
                xrBusinessPM.DataBindings.Add("Text", entity.ProjectDetails, DatabaseObjects.Columns.TicketProjectManager);
                xrITPM.DataBindings.Add("Text", entity.ProjectDetails, DatabaseObjects.Columns.TicketITManager);

                xrProjectDes.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketDescription);
                xrWeeklySummary.DataBindings.Add("Text", null, DatabaseObjects.Columns.ProjectSummaryNote);
                xrlblTicketId.DataBindings.Add("Text", null, "TicketId");
                xrProject.DataBindings.Add("Bookmark", null, "Title");

                xrWeeklySummary.BeforePrint += xrWeeklySummary_BeforePrint;
                this.DataSource = entity.ProjectDetails;
            }
            #endregion

            

            ProjectCompact_Milestone_Subreport mileReport = new ProjectCompact_Milestone_Subreport(entity);
            xrSubreport1.ReportSource = mileReport;



            xrCurrentWeekAccom.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrCurrentWeekAccom.BeforePrint += xrCurrentWeekAccom_BeforePrint;
            xrNxtWeekPlannedAct.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrNxtWeekPlannedAct.BeforePrint += xrNxtWeekPlannedAct_BeforePrint;
           // DetailReport_Accomplishment.DataSource = entity.AccomPlanned;

            if (entity.PMMRisks != null && entity.PMMRisks.Rows.Count > 0)
            {
                xrTCRiskNum.Summary.Func = SummaryFunc.RecordNumber;
                xrTCRiskNum.Summary.FormatString = uGITFormatConstants.RecordNumberFormat;
                xrTCRiskNum.Summary.Running = SummaryRunning.Report;

                xrRisksIssue.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
                xrProbability.DataBindings.Add("Text", null, DatabaseObjects.Columns.RiskProbability, uGITFormatConstants.PrecentFormat);
                xrimpact.DataBindings.Add("Text", null, DatabaseObjects.Columns.IssueImpact);
                xrRiskMiti.DataBindings.Add("Text", null, DatabaseObjects.Columns.MitigationPlan);
                xrContingency.DataBindings.Add("Text", null, DatabaseObjects.Columns.ContingencyPlan);
                xrRiskAssignedTo.DataBindings.Add("Text", null, DatabaseObjects.Columns.AssignedTo);
                DetailReport_Risk.DataSource = entity.PMMRisks;
            }
            if (entity.PMMIssues != null && entity.PMMIssues.Rows.Count > 0)
            {

                xrTCIssuesNum.Summary.Func = SummaryFunc.RecordNumber;
                xrTCIssuesNum.Summary.FormatString = uGITFormatConstants.RecordNumberFormat;
                xrTCIssuesNum.Summary.Running = SummaryRunning.Report;

                xrIssue.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
                xrIssuePriority.DataBindings.Add("Text", null, DatabaseObjects.Columns.TaskPriority);
                xrIssueCreatedOn.DataBindings.Add("Text", null, DatabaseObjects.Columns.Created, uGITFormatConstants.DateFormat);
                xrIssueStatus.DataBindings.Add("Text", null, DatabaseObjects.Columns.Status);
                xrIssueAssignedTo.DataBindings.Add("Text", null, DatabaseObjects.Columns.AssignedTo);
                xrIssueDescription.DataBindings.Add("Text", null, DatabaseObjects.Columns.Body);
                xrIssueResolution.DataBindings.Add("Text", null, DatabaseObjects.Columns.UGITResolution);
                DetailReport_Issue.DataSource = entity.PMMIssues;
            }

            CompactReportLegendSubReport legendReport = new CompactReportLegendSubReport(entity);
            xrSubreport2.ReportSource = legendReport;
        }

        void xrNxtWeekPlannedAct_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel lbl = sender as XRLabel;
            lbl.Text = string.Empty;
            if (entity.ImediatePlanned != null && entity.ImediatePlanned.Rows.Count > 0)
            {

                List<string> lst = entity.ImediatePlanned.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Title])).ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    string currentItem = lst[i];
                    lst[i] = string.Format("{0}. {1}", i + 1, currentItem);
                }

                lbl.Text = string.Join("\n", lst);
            }
        }

        void xrCurrentWeekAccom_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            XRLabel lbl = sender as XRLabel;
            lbl.Text = string.Empty;
            if (entity.AccomPlanned != null && entity.AccomPlanned.Rows.Count > 0)
            {

                List<string> lst = entity.AccomPlanned.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Title])).ToList();
                for (int i = 0; i < lst.Count; i++)
                {
                    string currentItem = lst[i];
                    lst[i] = string.Format("{0}. {1}", i + 1, currentItem);
                }

                lbl.Text = string.Join("\n", lst);
            }
        }

        void xrWeeklySummary_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel lbl = sender as XRLabel;
            lbl.Text = UGITUtility.StripHTML(lbl.Text);
        }
    }
}
