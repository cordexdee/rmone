using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Report.DxReport
{
    public class ProjectCompactReportEntity
    {
        public DataTable ProjectDetails { get; set; }
        public DataTable MileStone { get; set; }
        public DataTable RiskIssue { get; set; }
        public DataTable PMMComment { get; set; }
        public DataTable PMMRisks { get; set; }
        public DataTable PMMIssues { get; set; }
        public DataTable AccomPlanned { get; set; }
        public DataTable ProjectMonitorHealth { get; set; }
        public DataTable Tasks { get; set; }
        public DataTable ImediatePlanned { get; set; }
        public bool ShowAllMilestone { get; set; }
        public DataTable DecisionLog { get; set; }
        public bool ShowDecisionLog { get; set; }
    }
}
