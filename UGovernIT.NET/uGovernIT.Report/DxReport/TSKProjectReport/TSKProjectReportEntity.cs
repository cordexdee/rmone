using DevExpress.XtraPrinting;
using System;
using System.Data;
using System.Drawing;

namespace uGovernIT.Report.DxReport
{
    public class TSKProjectReportEntity
    {
        public string TicketId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectScore { get; set; }
        public string ProjectPhase { get; set; }
        public string PhaseCompletion { get; set; }
        public string OverallStatus { get; set; }
        public string CompanyLogo { get; set; }
        public int LogoHeight { get; set; }
        public int LogoWidth { get; set; }

        public string CriticalIssues { get; set; }
        public string WithinScope { get; set; }
        public string OnBudget { get; set; }
        public string OnTime { get; set; }
        public string RiskLevel { get; set; }

        public DataTable Projects { get; set; }
        public DataTable MonitorState { get; set; }
        public DataTable Accomplishment { get; set; }
        public DataTable ImmediatePlans { get; set; }
        public DataTable Risks { get; set; }
        public DataTable Issues { get; set; }
        public DataTable Tasks { get; set; }
        public DataTable OpenTasks { get; set; }
        public DataTable SummaryTasks { get; set; }
        public DataTable PlannedvsActualsbyCategory { get; set; }
        public DataSet PlannedMonthlyBudget { get; set; }
        public DataSet ActualMonthlyBudget { get; set; }
        public DataTable BudgetAllocation { get; set; }
        public DataTable ExecutiveHistory { get; set; }
        public DataTable PMMRisk { get; set; }
        public string SortOrder { get; set; }
        public bool ShowTrafficlight { get; set; }
        public bool ShowAllMilestone { get; set; }
        #region "Project Summary Report Field"
        [Reportable(LenFactor = 3, AlternateName = "Project Name", TextAlignment = TextAlignment.TopLeft)]
        public bool ShowProjectName { get; set; }

        [Reportable(LenFactor = 2, AlternateName = "Priority", TextAlignment = TextAlignment.TopCenter)]
        public bool ShowPriority { get; set; }

        [Reportable(LenFactor = 2, AlternateName = "Progress", TextAlignment = TextAlignment.TopCenter)]
        public bool ShowProgress { get; set; }

        [Reportable(LenFactor = 4, AlternateName = "Description", TextAlignment = TextAlignment.TopLeft)]
        public bool ShowDescription { get; set; }

        [Reportable(LenFactor = 2, AlternateName = "Target Date", TextAlignment = TextAlignment.TopCenter)]
        public bool ShowTargetDate { get; set; }

        [Reportable(LenFactor = 2, AlternateName = "Project Manager(s)", TextAlignment = TextAlignment.TopCenter)]
        public bool ShowProjectManagers { get; set; }


        [Reportable(LenFactor = 2, AlternateName = "Project Type", TextAlignment = TextAlignment.TopCenter)]
        public bool ShowProjectType { get; set; }

        [Reportable(LenFactor = 1, AlternateName = "% Comp", TextAlignment = TextAlignment.TopCenter)]
        public bool ShowPercentComplete { get; set; }

        [Reportable(LenFactor = 4, AlternateName = "Status", TextAlignment = TextAlignment.TopLeft)]
        public bool ShowProStatus { get; set; }

        public bool ShowLatestOnly { get; set; }
        public bool ShowPlainText { get; set; }

        #endregion

        [Reportable(LenFactor = 3, AlternateName = "Accomplishments", TextAlignment = TextAlignment.TopLeft)]
        public bool ShowAccomplishment { get; set; }

        public bool ShowAccomplishmentDesc { get; set; }

        [Reportable(LenFactor = 4, AlternateName = "Planned Items", TextAlignment = TextAlignment.MiddleLeft)]
        public bool ShowPlan { get; set; }

        public bool ShowPlanDesc { get; set; }


        public bool ShowRiskDesc { get; set; }

        [Reportable(LenFactor = 3, AlternateName = "Risks", TextAlignment = TextAlignment.MiddleLeft)]
        public bool ShowRisk { get; set; }

        [Reportable(LenFactor = 3, AlternateName = "Issues", TextAlignment = TextAlignment.MiddleLeft)]
        public bool ShowIssues { get; set; }

        public bool ShowIssuesDesc { get; set; }

        [Reportable(LenFactor = 4, AlternateName = "MonitorStates", TextAlignment = TextAlignment.MiddleCenter)]
        public bool ShowMonitorState { get; set; }

        public bool ShowStatus { get; set; }


        public bool ShowSummaryGanttChart { get; set; }
        public bool ShowAllTaskGanttChart { get; set; }
        public bool ShowAllTask { get; set; }
        public bool ShowOpenTaskOnly { get; set; }
        public bool ShowMilestone { get; set; }
        public bool ShowDeliverable { get; set; }
        public bool ShowReceivable { get; set; }
        public bool CalculateExpected { get; set; }
        public bool ShowProjectDescription { get; set; }
        public bool ShowBudgetSummary { get; set; }
        public bool ShowPlannedvsActualByCategory { get; set; }
        public bool ShowPlannedvsActualByBudgetItem { get; set; }
        public bool ShowPlannedvsActualByMonth { get; set; }
        public bool ShowProjectRoles { get; set; }
        public bool ShowResourceAllocation { get; set; }
         
        public bool IsPMM { get; set; }
        public Image CompanyLogoBitmap { get; set; }

        public DataTable DecisionLog { get; set; }
        public bool ShowDecisionLog { get; set; }
    }
    public class ResourceAllocation
    {
        public string Legend { get; set; }
        public double Jan { get; set; }
        public double Feb { get; set; }
        public double Mar { get; set; }
        public double Apr { get; set; }
        public double May { get; set; }
        public double Jun { get; set; }
        public double Jul { get; set; }
        public double Aug { get; set; }
        public double Sep { get; set; }
        public double Oct { get; set; }
        public double Nov { get; set; }
        public double Dec { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class Reportable : Attribute
    {
        private string altName;
        public string AlternateName
        {
            get { return this.altName; }
            set { this.altName = value; }
        }
        private int lenFactor;
        public int LenFactor
        {
            get { return this.lenFactor; }
            set { this.lenFactor = value; }
        }

        private TextAlignment textAlignment;
        public TextAlignment TextAlignment
        {
            get { return this.textAlignment; }
            set { this.textAlignment = value; }
        }
        private string fName;
        public string FName
        {
            get { return this.fName; }
            set { this.fName = value; }
        }
    }

}
