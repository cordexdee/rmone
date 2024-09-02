using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using uGovernIT.Manager.Entities;
using System.Data;
using System.Linq;

namespace uGovernIT.DxReport
{
    public partial class MultiProReport : DevExpress.XtraReports.UI.XtraReport
    {
        TSKProjectReportEntity _prEntity = null;
        public MultiProReport(TSKProjectReportEntity prEntity)
        {
            InitializeComponent();
            _prEntity = prEntity;
            xrlblTicket.DataBindings.Add("Text", null, "TicketId");
            Report.DataSource = prEntity.Projects;

        }

        private void xrProjectSubReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (xrlblTicket.Text == "TicketId")
            {
                return;
            }
            TSKProjectReportEntity entity = new TSKProjectReportEntity();
            string ticketId = xrlblTicket.Text;
            DataTable dt = new DataTable();
            DataRow[] drs = null;

            drs = _prEntity.Projects.Select("TicketId='" + ticketId + "'");
            if (drs.Length > 0) entity.Projects = drs.CopyToDataTable();

            if (_prEntity.MonitorState != null)
            {
                dt = _prEntity.MonitorState.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.MonitorState = drs.CopyToDataTable();
            }

            if (_prEntity.Accomplishment != null)
            {
                dt = _prEntity.Accomplishment.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.Accomplishment = drs.CopyToDataTable();
            }

            if (_prEntity.ImmediatePlans != null)
            {
                dt = _prEntity.ImmediatePlans.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.ImmediatePlans = drs.CopyToDataTable();
            }

            if (_prEntity.Issues != null && _prEntity.Issues.Rows.Count > 0)
            {
                dt = _prEntity.Issues.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.Issues = drs.CopyToDataTable();
            }

            if (_prEntity.Tasks != null)
            {
                dt = _prEntity.Tasks.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.Tasks = drs.CopyToDataTable();
            }

            //if (_prEntity.OpenTasks != null)
            //{
            //    dt = _prEntity.OpenTasks.Copy();
            //    drs = dt.Select("ProjectID='" + ticketId + "'");
            //    if (drs.Length > 0) entity.OpenTasks = drs.CopyToDataTable();
            //}

            if (_prEntity.SummaryTasks != null)
            {
                dt = _prEntity.SummaryTasks.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.SummaryTasks = drs.CopyToDataTable();
            }

            if (_prEntity.PlannedvsActualsbyCategory != null)
            {
                dt = _prEntity.PlannedvsActualsbyCategory.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.PlannedvsActualsbyCategory = drs.CopyToDataTable();
            }

            if (_prEntity.PlannedMonthlyBudget != null)
            {
                DataSet ds = _prEntity.PlannedMonthlyBudget.Copy();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Contains(ticketId))
                    {
                        DataSet _ds = new DataSet();
                        _ds.Tables.Add(ds.Tables[ticketId].Copy());
                        entity.PlannedMonthlyBudget = _ds;
                    }
                }
            }

            if (_prEntity.ActualMonthlyBudget != null)
            {
                DataSet ds = _prEntity.ActualMonthlyBudget.Copy();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Contains(ticketId))
                    {
                        DataSet _ds = new DataSet();
                        _ds.Tables.Add(ds.Tables[ticketId].Copy());
                        entity.ActualMonthlyBudget = _ds;
                    }
                }
            }

            if (_prEntity.BudgetAllocation != null)
            {
                dt = _prEntity.BudgetAllocation.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.BudgetAllocation = drs.CopyToDataTable();
            }

            if (_prEntity.Risks != null)
            {
                dt = _prEntity.Risks.Copy();
                drs = dt.Select("TicketId='" + ticketId + "'");
                if (drs.Length > 0) entity.Risks = drs.CopyToDataTable();
            }

            //if (_prEntity.DecisionLog != null) 
            //{
            //    dt = _prEntity.DecisionLog.Copy();
            //    drs = dt.Select("TicketId='" + ticketId + "'");
            //    if (drs.Length > 0) entity.DecisionLog = drs.CopyToDataTable();
            //}
            entity.ShowAccomplishment = _prEntity.ShowAccomplishment;
            entity.ShowPlan = _prEntity.ShowPlan;
            entity.ShowIssues = _prEntity.ShowIssues;
            entity.ShowStatus = _prEntity.ShowStatus;
            entity.ShowSummaryGanttChart = _prEntity.ShowSummaryGanttChart;
            entity.ShowAllTask = _prEntity.ShowAllTask;
           // entity.ShowOpenTaskOnly = _prEntity.ShowOpenTaskOnly;
            entity.ShowMilestone = _prEntity.ShowMilestone;
            entity.ShowDeliverable = _prEntity.ShowDeliverable;
            entity.ShowReceivable = _prEntity.ShowReceivable;
            entity.CalculateExpected = _prEntity.CalculateExpected;
            entity.ShowProjectDescription = _prEntity.ShowProjectDescription;
            entity.ShowBudgetSummary = _prEntity.ShowBudgetSummary;
            entity.ShowPlannedvsActualByCategory = _prEntity.ShowPlannedvsActualByCategory;
            entity.ShowPlannedvsActualByBudgetItem = _prEntity.ShowPlannedvsActualByBudgetItem;
            entity.ShowPlannedvsActualByMonth = _prEntity.ShowPlannedvsActualByMonth;
            entity.ShowProjectRoles = _prEntity.ShowProjectRoles;
            entity.ShowResourceAllocation = _prEntity.ShowResourceAllocation;
            entity.ShowMonitorState = _prEntity.ShowMonitorState;
            entity.LogoHeight = _prEntity.LogoHeight;
            entity.LogoWidth = _prEntity.LogoWidth;
            entity.ShowRisk = _prEntity.ShowRisk;
            entity.ShowTrafficlight = _prEntity.ShowTrafficlight;
          //  entity.ShowDecisionLog = _prEntity.ShowDecisionLog;
            ProjectReport pr = new ProjectReport(entity);
            xrProjectSubReport.ReportSource = pr;
        }
    }
}
