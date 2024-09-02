using DevExpress.Web;
using DevExpress.Web.Rendering;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class GroupScorecard : UserControl
    {
        public List<GroupScoreCardData> GroupScoreCard { get; set; }
        public List<TicketFlowByGroup> TicketFlowByGroupList { get; set; }
        public List<WeeklyRollingAverage> WeeklyRollingAverageList { get; set; }
        public List<PredictedBacklog> PredictedBacklogs { get; set; }
        public List<PredictedBacklog> GroupsUnsolvedList { get; set; }
        public List<UnSolvedTicket> OldestUnsolvedTickets { get; set; }

        public bool ShowIndividual { get; set; }
        public int NumberofDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFilterActive { get; set; }
        public string Module { get; set; }
        public string Status { get; set; }
        protected string drilDownData = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=problemreportdrildowndata");
        protected string url = "/layouts/uGovernit/DelegateControl.aspx?";

        protected string GrpScoreCardurl = "/layouts/uGovernit/DelegateControl.aspx?Control=groupscorecarddrilldowndata";
        protected string scoreCardurl = "/layouts/uGovernit/DelegateControl.aspx?Control=";
        public TypeOfReport ReportType { get; set; }
        private string ticketgroup = string.Empty;
        protected string drillDownUrl = UGITUtility.GetAbsoluteURL("/layouts/uGovernit/delegatecontrol.aspx?control=");
        public DataTable dtDashboardSummary = null;

        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string ticketAssignee;
        public string moduleName = string.Empty;
        int moduleid;
        public string WeeklyAverage { get; set; }
        public string Category { get; set; }
        private List<string> smsModules = new List<string>();

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        FieldConfigurationManager configFieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {

            //DataTable dtModule = uGITCache.GetModuleList(ModuleType.SMS);
            //smsModules = dtModule.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.ModuleName)).ToList();

            List<UGITModule> UgitModule = moduleViewManager.Load($"{DatabaseObjects.Columns.ModuleType}='{((int)ModuleType.SMS)}'").ToList();
            smsModules = UgitModule.Select(x => x.ModuleName).ToList();

            // For excluding NPR and PMM
            if (smsModules.Contains("NPR"))
                smsModules.Remove("NPR");
            if (smsModules.Contains("PMM"))
                smsModules.Remove("PMM");


            pnlControls.Width = Width;
            pnlControls.Height = Height;
            switch (ReportType)
            {
                case TypeOfReport.ScoreCard:
                    deStartdt.Date = DateTime.Now.AddMonths(-1);
                    deEnddt.Date = DateTime.Now;
                    pnlHeaderFilter.Visible = IsFilterActive;
                    if (IsFilterActive)
                    {
                        if (StartDate != DateTime.MinValue)
                            deStartdt.Date = StartDate;
                        if (EndDate != DateTime.MinValue)
                            deEnddt.Date = EndDate;
                    }

                    gvScoreBoard.Visible = true;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = false;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.TicketFlow:

                    deStartDateTicketFlow.Date = DateTime.Now.AddDays(-8);
                    deEndDateTicketFlow.Date = DateTime.Now;
                    pnlHeaderTicketFlow.Visible = IsFilterActive;
                    if (IsFilterActive)
                    {
                        if (StartDate != DateTime.MinValue)
                            deStartDateTicketFlow.Date = StartDate;
                        if (EndDate != DateTime.MinValue)
                            deEndDateTicketFlow.Date = EndDate;

                    }

                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = true;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = false;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.WeeklyRollingAverage:

                    pnlLastWeekvs12WeekHeader.Visible = IsFilterActive;
                    txtLastWeekvs12Week.Text = "12";
                    if (IsFilterActive)
                    {
                        if (!string.IsNullOrEmpty(WeeklyAverage))
                            txtLastWeekvs12Week.Text = WeeklyAverage;
                    }


                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = true;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = false;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.PredictBacklog:
                    txtPredictedBackLogFilter.Text = "12";
                    pnlPredictedBacklogHeader.Visible = IsFilterActive;
                    if (IsFilterActive)
                    {
                        if (!string.IsNullOrEmpty(WeeklyAverage))
                            txtPredictedBackLogFilter.Text = WeeklyAverage;
                    }

                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = true;
                    wccTicketsCreatedByWeek.Visible = false;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.TicketCreatedByWeek:
                    txtTicketCreatedWeekTrend.Text = "12";
                    pnlTicketCreatedByWeekChartFilter.Visible = IsFilterActive;
                    if (IsFilterActive)
                    {
                        if (!string.IsNullOrEmpty(WeeklyAverage))
                            txtTicketCreatedWeekTrend.Text = WeeklyAverage;
                    }
                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = true;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.GroupUnsolvedTickets:

                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = false;
                    gvGrpUnsolvedTickets.Visible = true;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.OldestUnsolved:

                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = false;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = true;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.TSRTrendChart:
                    GenerateDataTSRTrendReport(12);
                    pnlTicketCreatedByWeekChartFilter.Visible = IsFilterActive;
                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = true;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = false;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.ProblemReport:
                    GenerateDataProblemReport();
                    gvScoreBoard.Visible = false;
                    gvTicketFlow.Visible = false;
                    gvLastWeekvs12Week.Visible = false;
                    gvPredictedBacklog.Visible = false;
                    wccTicketsCreatedByWeek.Visible = false;
                    gvGrpUnsolvedTickets.Visible = false;
                    gvOldestUnsolved.Visible = false;
                    wccProblemReport.Visible = true;
                    wccRequestReport.Visible = false;
                    break;
                case TypeOfReport.AgentPerformance:
                    {


                        wccAgentPerformance.Visible = true;
                        gvScoreBoard.Visible = false;
                        gvTicketFlow.Visible = false;
                        gvLastWeekvs12Week.Visible = false;
                        gvPredictedBacklog.Visible = false;
                        wccTicketsCreatedByWeek.Visible = false;
                        gvGrpUnsolvedTickets.Visible = false;
                        gvOldestUnsolved.Visible = false;
                        wccProblemReport.Visible = false;
                        wccRequestReport.Visible = false;
                    }
                    break;
                //case TypeOfReport.ProblemReport:
                //    GenerateDataProblemReport();
                //    gvScoreBoard.Visible = false;
                //    gvTicketFlow.Visible = false;
                //    gvLastWeekvs12Week.Visible = false;
                //    gvPredictedBacklog.Visible = false;
                //    wccTicketsCreatedByWeek.Visible = false;
                //    gvGrpUnsolvedTickets.Visible = false;
                //    gvOldestUnsolved.Visible = false;
                //    wccProblemReport.Visible = true;
                //    wccRequestReport.Visible = false;
                //    break;
                //case TypeOfReport.RequestReport:
                //    GenerateDataRequestReport();
                //    gvScoreBoard.Visible = false;
                //    gvTicketFlow.Visible = false;
                //    gvLastWeekvs12Week.Visible = false;
                //    gvPredictedBacklog.Visible = false;
                //    wccTicketsCreatedByWeek.Visible = false;
                //    gvGrpUnsolvedTickets.Visible = false;
                //    gvOldestUnsolved.Visible = false;
                //    wccProblemReport.Visible = false;
                //    wccRequestReport.Visible = true;
                //    break;
                default:
                    break;
            }
            base.OnInit(e);
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            switch (ReportType)
            {
                case TypeOfReport.ScoreCard:
                    GenerateData(deStartdt.Date, deEnddt.Date);
                    gvScoreBoard.DataSource = GroupScoreCard;
                    gvScoreBoard.DataBind();
                    break;
                case TypeOfReport.TicketFlow:
                    GenerateDataTicketFlow(deStartDateTicketFlow.Date, deEndDateTicketFlow.Date);
                    gvTicketFlow.DataSource = TicketFlowByGroupList;
                    gvTicketFlow.DataBind();
                    break;
                case TypeOfReport.WeeklyRollingAverage:
                    {
                        int weeks = 0;
                        if (!int.TryParse(txtLastWeekvs12Week.Text.Trim(), out weeks))
                        {
                            weeks = 12;
                            txtLastWeekvs12Week.Text = weeks.ToString();
                        }
                        GenerateDataWeeklyRollingAverage(weeks);
                        gvLastWeekvs12Week.DataSource = WeeklyRollingAverageList;
                        gvLastWeekvs12Week.DataBind();
                    }
                    break;
                case TypeOfReport.PredictBacklog:
                    {
                        int weeks = 0;
                        if (!int.TryParse(txtPredictedBackLogFilter.Text.Trim(), out weeks))
                        {
                            weeks = 12;
                            txtPredictedBackLogFilter.Text = weeks.ToString();
                        }

                        GenerateDataPredictedBacklog(weeks);
                        gvPredictedBacklog.DataSource = PredictedBacklogs;
                        gvPredictedBacklog.DataBind();
                    }
                    break;
                case TypeOfReport.TicketCreatedByWeek:
                    {
                        int weeks = 0;
                        if (!int.TryParse(txtTicketCreatedWeekTrend.Text.Trim(), out weeks))
                        {
                            weeks = 12;
                            txtTicketCreatedWeekTrend.Text = weeks.ToString();
                        }
                        GenerateDataTicketsCreatedByWeek(weeks);

                    }
                    break;
                case TypeOfReport.GroupUnsolvedTickets:
                    GenerateDataUnsolvedTickets();
                    gvGrpUnsolvedTickets.DataSource = GroupsUnsolvedList;
                    gvGrpUnsolvedTickets.DataBind();
                    break;
                case TypeOfReport.OldestUnsolved:
                    GenerateDataOldestUnsolvedTickets();
                    gvOldestUnsolved.DataSource = OldestUnsolvedTickets;
                    gvOldestUnsolved.DataBind();
                    break;
                case TypeOfReport.ProblemReport:
                    break;
                case TypeOfReport.AgentPerformance:
                    GenerateDataAgentPerformance(10, 1);
                    break;
                default:
                    break;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //if (externalFilterInitiated)
            //{
            //    if (ReportType == TypeOfReport.ScoreCard)
            //    {
            //        gvScoreBoard.DataSource = GroupScoreCard;
            //        gvScoreBoard.DataBind();
            //    }
            //    else if (ReportType == TypeOfReport.TicketFlow)
            //    {
            //        gvTicketFlow.DataSource = TicketFlowByGroupList;
            //        gvTicketFlow.DataBind();
            //    }
            //    else if (ReportType == TypeOfReport.WeeklyRollingAverage)
            //    {
            //        gvLastWeekvs12Week.DataSource = WeeklyRollingAverageList;
            //        gvLastWeekvs12Week.DataBind();
            //    }
            //    else if (ReportType == TypeOfReport.PredictBacklog)
            //    {
            //        gvPredictedBacklog.DataSource = PredictedBacklogs;
            //        gvPredictedBacklog.DataBind();
            //    }
            //}
            base.OnPreRender(e);
        }

        public void GenerateData(DateTime dtstart, DateTime dtend)
        {
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}' and {DatabaseObjects.Columns.FunctionalAreaLookup} is not null");
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            LoadData();
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            GroupScoreCard = (from ds in dtDashboardSummary.AsEnumerable()
                              where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                              && (ds.Field<DateTime>(DatabaseObjects.Columns.Modified) >= dtstart &&
                              ds.Field<DateTime>(DatabaseObjects.Columns.Modified) <= dtend)
                              group ds by ds.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup + "$Id") into g
                              select new GroupScoreCardData
                              {
                                  TicketGroupId = g.Key,
                                  Reopen = g.Average(x => x.Field<int?>(DatabaseObjects.Columns.ReopenCount)),
                                  SolvedTickets = g.Where(x => x.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed").Count(),
                                  UnsolvedTickets = g.Where(x => x.Field<string>(DatabaseObjects.Columns.TicketStatus) != "Closed").Count()
                              }).ToList<GroupScoreCardData>();

            GroupScoreCard.ForEach(x =>
                {
                    x.TicketGroup =  configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.FunctionalAreaLookup, Convert.ToString(x.TicketGroupId));
                }
            );

        }

        public void GenerateDataTicketFlow(DateTime dtstart, DateTime dtend)
        {
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            LoadData();
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            TicketFlowByGroupList = (from ds in dtDashboardSummary.AsEnumerable()
                                     where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                     && (ds.Field<DateTime>(DatabaseObjects.Columns.Modified) >= dtstart &&
                                     ds.Field<DateTime>(DatabaseObjects.Columns.Modified) <= dtend)
                                     group ds by ds.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup + "$Id") into g
                                     select new TicketFlowByGroup
                                     {
                                         TicketGroupId = g.Key,
                                         Created = g.Where(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate) >= dtstart).Count(),
                                         Solved = g.Where(x => x.Field<string>(DatabaseObjects.Columns.TicketResolutionType) != null &&
                                             x.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed").Count(),
                                         Backlog = dtDashboardSummary.AsEnumerable().Where(x => x.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup + "$Id") == g.Key &&
                                             x.Field<string>(DatabaseObjects.Columns.TicketStatus) != "Closed").Count()
                                     }).ToList<TicketFlowByGroup>();

            if (TicketFlowByGroupList != null && TicketFlowByGroupList.Count > 0)
            {
                TicketFlowByGroupList.ForEach(x =>
                {
                    //x.Update(y =>
                    //{
                    //    y.SolvedPct = y.Solved == 0 || y.Created == 0 ? 0.0 : (y.Solved / y.Created);
                    //    y.BacklogImpact = y.Solved - y.Created;
                    //});
                    {
                        x.SolvedPct = x.Solved == 0 || x.Created == 0 ? 0.0 : (x.Solved / x.Created);
                        x.BacklogImpact = x.Solved - x.Created;
                        x.TicketGroup =configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.FunctionalAreaLookup, Convert.ToString(x.TicketGroupId));
                    }
                });
            }
        }

        public void GenerateDataWeeklyRollingAverage(int weeks)
        {

            LoadData();

            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            WeeklyRollingAverageList = (from ds in dtDashboardSummary.AsEnumerable()
                                        where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                        && ds.Field<DateTime>(DatabaseObjects.Columns.Modified) >= DateTime.Now.AddDays(-7)
                                        group ds by ds.Field<string>(DatabaseObjects.Columns.TicketPRP) into g
                                        select new WeeklyRollingAverage
                                        {
                                            TicketAssignee = string.IsNullOrEmpty(g.Key) ? string.Empty : g.Key,
                                            Solved = g.Where(x => x.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed").Count(),
                                            WeeklyAverage = Convert.ToDouble(string.Format("{0:0.00}", (dtDashboardSummary.AsEnumerable()
                                                           .Where(x => x.Field<string>(DatabaseObjects.Columns.TicketPRP) == g.Key &&
                                                                       x.Field<DateTime>(DatabaseObjects.Columns.Modified) >= DateTime.Now.AddDays(-(weeks * 7))).Count()) / (double)weeks))
                                        }).OrderBy(x => x.TicketAssignee).ToList<WeeklyRollingAverage>();


            if (WeeklyRollingAverageList != null && WeeklyRollingAverageList.Count > 0)
            {
                WeeklyRollingAverageList.ForEach(x =>
                {
                    //x.Update(y =>
                    //{
                    x.Performance = x.WeeklyAverage > 0.0 ? (x.Solved - x.WeeklyAverage) / x.WeeklyAverage : 0.0;
                    //});
                });
            }
        }

        public void GenerateDataPredictedBacklog(int weeks)
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            PredictedBacklogs = (from ds in dtDashboardSummary.AsEnumerable()
                                 where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                 group ds by ds.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup + "$Id") into g
                                 select new PredictedBacklog
                                 {
                                     TicketGroupId = g.Key,
                                     UnsolvedTicket = g.Where(x => x.Field<string>(DatabaseObjects.Columns.TicketStatus) != "Closed").Count(),
                                     SolvedWeeklyAvg = Math.Round((Convert.ToDouble(g.Where(x => x.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup + "$Id") == g.Key &&
                                          x.Field<string>(DatabaseObjects.Columns.TicketStatus) == "Closed" &&
                                                       x.Field<DateTime>(DatabaseObjects.Columns.Modified) >= DateTime.Now.AddDays(-(weeks * 7))).Count()) / (double)weeks), 2)


                                 }).ToList<PredictedBacklog>();

            if (PredictedBacklogs != null && PredictedBacklogs.Count > 0)
            {
                PredictedBacklogs.ForEach(x =>
                {
                    //x.Update(y =>
                    //{
                    //    y.Backlog = y.UnsolvedTicket == 0 || y.SolvedWeeklyAvg == 0 ? 0.0 : (y.UnsolvedTicket / y.SolvedWeeklyAvg);
                    //});
                    x.Backlog = x.UnsolvedTicket == 0 || x.SolvedWeeklyAvg == 0 ? 0.0 : (x.UnsolvedTicket / x.SolvedWeeklyAvg);
                    x.TicketGroup =  configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.FunctionalAreaLookup, Convert.ToString(x.TicketGroupId));
                });
            }
        }

        public void GenerateDataTicketsCreatedByWeek(int weeks)
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            List<TicketsByWeek> TicketsByWeekList = new List<TicketsByWeek>();

            TicketsByWeekList = (from ds in dtDashboardSummary.AsEnumerable()
                                 where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                 && ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate) >= DateTime.Now.AddDays(-7 * weeks)
                                 orderby ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate)
                                 group ds by new
                                 {
                                     WeekNumber = GetISOWeek(ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate)),
                                     WeekDate = FirstDayOfWeek(ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate))
                                 } into g
                                 select new TicketsByWeek
                                 {
                                     Week = g.Key.WeekDate.ToString("MMM/dd/yyyy"),
                                     TicketCount = g.Count()
                                 }).ToList<TicketsByWeek>();

            wccTicketsCreatedByWeek.Height = Unit.Pixel(400);
            wccTicketsCreatedByWeek.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            //wccTicketsCreatedByWeek.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            //wccTicketsCreatedByWeek.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;

            Series linchartseries = new Series("# Tickets", ViewType.Line);
            linchartseries.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            linchartseries.Label.TextOrientation = TextOrientation.Horizontal;
            PointSeriesLabel pntslabel = linchartseries.Label as PointSeriesLabel;
            pntslabel.Angle = 90;
            pntslabel.TextPattern = "{V:n2}";
            linchartseries.CrosshairLabelPattern = "{S}: {V:n2}";
            wccTicketsCreatedByWeek.Series.Add(linchartseries);

            XYDiagram diagram = wccTicketsCreatedByWeek.Diagram as XYDiagram;
            diagram.AxisX.Title.Text = "# Tickets";
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisX.Title.Text = "Week (Mon-Sun)/Year (Ticket Creates)";
            diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisX.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisX.Label.Angle = 300;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Week;
            diagram.AxisX.WholeRange.MinValue = DateTime.Now.AddDays(-7 * weeks);
            diagram.AxisX.WholeRange.MaxValue = DateTime.Now;

            SeriesPointCollection pointcollection = linchartseries.Points;
            foreach (TicketsByWeek item in TicketsByWeekList)
            {
                pointcollection.Add(new SeriesPoint(item.Week, item.TicketCount));
            }
        }

        public void GenerateDataUnsolvedTickets()
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            GroupsUnsolvedList = (from ds in dtDashboardSummary.AsEnumerable()
                                  where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                  group ds by ds.Field<long?>(DatabaseObjects.Columns.FunctionalAreaLookup + "$Id") into g
                                  select new PredictedBacklog
                                  {
                                      TicketGroupId = g.Key,
                                      UnsolvedTicket = g.Where(x => x.Field<string>(DatabaseObjects.Columns.TicketStatus) != "Closed").Count(),
                                  }).ToList<PredictedBacklog>();

            if (GroupsUnsolvedList != null && GroupsUnsolvedList.Count > 0)
            {
                GroupsUnsolvedList.ForEach(x =>
                {
                    x.TicketGroup = configFieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.FunctionalAreaLookup, Convert.ToString(x.TicketGroupId));
                });
            }
        }

        public void GenerateDataOldestUnsolvedTickets()
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            OldestUnsolvedTickets = (from ds in dtDashboardSummary.AsEnumerable()
                                     where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                     && ds.Field<string>(DatabaseObjects.Columns.TicketStatus) != "Closed"
                                     select new UnSolvedTicket
                                     {
                                         TicketId = ds.Field<string>(DatabaseObjects.Columns.TicketId),
                                         Title = ds.Field<string>(DatabaseObjects.Columns.Title),
                                         TicketAssignee = ds.Field<string>(DatabaseObjects.Columns.TicketPRP),
                                         TicketStatus = ds.Field<string>(DatabaseObjects.Columns.TicketStatus),
                                         TicketAge = (DateTime.Now - ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate)).Days
                                     }).OrderByDescending(x => x.TicketAge).ToList();
        }

        public void GenerateDataProblemReport()
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            if (!string.IsNullOrWhiteSpace(Category))
            {
                dtDashboardSummary.Columns.Add("joinExpression", typeof(string), string.Format("[{0}]+'**'+[{1}]", DatabaseObjects.Columns.Category, DatabaseObjects.Columns.SubCategory));
                List<string> categorylst = UGITUtility.ConvertStringToList(Category, ",");

                var filteredData = (from ds in dtDashboardSummary.AsEnumerable()
                                    where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                    join dtc in categorylst on ds.Field<string>("joinExpression") equals dtc
                                    select ds).ToArray();

                if (filteredData.Length > 0)
                    dtDashboardSummary = filteredData.CopyToDataTable();
                else
                    dtDashboardSummary = dtDashboardSummary.Clone();
            }

            List<ProblemReport> ProblemReports = new List<ProblemReport>();

            List<string> queryExp = new List<string>();
            if (!string.IsNullOrEmpty(Module))
                queryExp.Add(string.Format(" {0} = '{1}'", DatabaseObjects.Columns.ModuleNameLookup, Module));

            if (Status == "Open")
                queryExp.Add(string.Format(" {0} <> 'Closed'", DatabaseObjects.Columns.TicketStatus));
            else if (Status == "Closed")
                queryExp.Add(string.Format(" {0} = 'Closed'", DatabaseObjects.Columns.TicketStatus));

            DataRow[] rowopen = dtDashboardSummary.Select(string.Join(" And ", queryExp));
            if (rowopen != null && rowopen.Length > 0)
                dtDashboardSummary = rowopen.CopyToDataTable();


            ProblemReports = (from ds in dtDashboardSummary.AsEnumerable()
                              where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup))
                                    && !string.IsNullOrEmpty(ds.Field<string>(DatabaseObjects.Columns.SubCategory))
                              group ds by new
                              {
                                  Category = ds.Field<string>(DatabaseObjects.Columns.Category),
                                  SubCategory = ds.Field<string>(DatabaseObjects.Columns.SubCategory)
                              } into g
                              select new ProblemReport
                              {
                                  Category = g.Key.Category,
                                  SubCategory = g.Key.SubCategory,
                                  TicketCount = g.Count()
                              }).OrderByDescending(x => x.TicketCount).ToList<ProblemReport>();


            if (ProblemReports.Count == 0)
            {
                showemptymessage.Visible = true;
                return;
            }

            wccProblemReport.Width = Unit.Pixel((int)Width.Value - 10);
            wccProblemReport.Height = Unit.Pixel((int)Height.Value - 10);
            wccProblemReport.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            wccProblemReport.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            wccProblemReport.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            wccProblemReport.Legend.Direction = LegendDirection.LeftToRight;
            wccProblemReport.Legend.HorizontalIndent = 10;

            foreach (ProblemReport item in ProblemReports)
            {
                var seriesName = item.Category + (string.IsNullOrEmpty(item.SubCategory) ? "" : " > " + item.SubCategory);
                Series series = new Series(seriesName, ViewType.Bar);
                wccProblemReport.Series.Add(series);
                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                SeriesPointCollection pointcollection = series.Points;
                pointcollection.Add(new SeriesPoint(seriesName, item.TicketCount));

                SideBySideBarSeriesLabel sidebysidebarSeriesLbl = series.Label as SideBySideBarSeriesLabel;
                sidebysidebarSeriesLbl.LineVisibility = DevExpress.Utils.DefaultBoolean.False;
                sidebysidebarSeriesLbl.Position = BarSeriesLabelPosition.Top;
                sidebysidebarSeriesLbl.TextPattern = "{V:#}";

                SideBySideBarSeriesView sidebysidebarSeriesView = series.View as SideBySideBarSeriesView;
                sidebysidebarSeriesView.BarWidth = 13;
                sidebysidebarSeriesView.BarDistance = 40;
                //sidebysidebarSeriesView.BarDistance = 20;
            }

            XYDiagram diagram = wccProblemReport.Diagram as XYDiagram;
            diagram.AxisY.Title.Text = "# Tickets";
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisY.VisibleInPanesSerializable = "-1";
            diagram.AxisY.Interlaced = true;
            diagram.AxisY.NumericScaleOptions.AutoGrid = true;
            diagram.AxisY.NumericScaleOptions.GridSpacing = 10;

            diagram.AxisX.VisibleInPanesSerializable = "-1";
            diagram.AxisX.Label.Visible = false;
            diagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.False;
            diagram.AxisX.VisualRange.AutoSideMargins = false;
            diagram.AxisX.VisualRange.SideMarginsValue = 1.0;
        }


        private void GenerateDataTSRTrendReport(int weeks)
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            List<TicketsByWeek> TicketsByWeekList = new List<TicketsByWeek>();

            var TSRTrendChartData = (from ds in dtDashboardSummary.AsEnumerable()
                                     where ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate) >= DateTime.Now.AddDays(-7 * weeks)
                                     orderby ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate)
                                     group ds by new
                                     {
                                         Category = ds.Field<string>(DatabaseObjects.Columns.Category),
                                         SubCategory = ds.Field<string>(DatabaseObjects.Columns.SubCategory)

                                     } into g
                                     select new
                                     {
                                         Category = g.Key.Category + "_" + g.Key.Category,
                                         Data = (from cat in g
                                                 group cat by new
                                                 {
                                                     WeekNumber = GetISOWeek(cat.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate)),
                                                     WeekDate = FirstDayOfWeek(cat.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate))
                                                 } into g1
                                                 select new
                                                 {
                                                     Week = g1.Key.WeekDate.ToString("MMM/dd/yyyy"),
                                                     TicketCount = g1.Count()
                                                 })
                                     }).ToList();

            wccTicketsCreatedByWeek.Height = Unit.Pixel(400);
            wccTicketsCreatedByWeek.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            wccTicketsCreatedByWeek.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            wccTicketsCreatedByWeek.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            wccTicketsCreatedByWeek.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            wccTicketsCreatedByWeek.Legend.Direction = LegendDirection.LeftToRight;
            wccTicketsCreatedByWeek.Legend.HorizontalIndent = 10;


            foreach (var item in TSRTrendChartData)
            {
                Series linchartseries = new Series(item.Category, ViewType.Line);
                linchartseries.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                linchartseries.Label.TextOrientation = TextOrientation.Horizontal;

                PointSeriesLabel pntslabel = linchartseries.Label as PointSeriesLabel;
                wccTicketsCreatedByWeek.CrosshairOptions.ShowArgumentLabels = true;
                linchartseries.CrosshairLabelPattern = "{S}: {V:#0.##}";
                pntslabel.Angle = 90;
                pntslabel.TextPattern = "{V:#0.##}";
                wccTicketsCreatedByWeek.Series.Add(linchartseries);
                SeriesPointCollection pointcollection = linchartseries.Points;

                foreach (var item1 in item.Data)
                {
                    pointcollection.Add(new SeriesPoint(item1.Week, item1.TicketCount));
                }
            }

            XYDiagram diagram = wccTicketsCreatedByWeek.Diagram as XYDiagram;
            diagram.AxisX.Title.Text = "# Tickets";
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisX.Title.Text = "Week (Mon-Sun)/Year (Ticket Creates)";
            diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisX.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisX.Label.Angle = 300;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Week;
            diagram.AxisX.WholeRange.MinValue = DateTime.Now.AddDays(-7 * weeks);
            diagram.AxisX.WholeRange.MaxValue = DateTime.Now;

        }

        private void GenerateDataRequestReport()
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            List<RequestReport> RequestReports = new List<RequestReport>();
            RequestReports = (from ds in dtDashboardSummary.AsEnumerable()
                              where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup)) && !string.IsNullOrEmpty(ds.Field<string>(DatabaseObjects.Columns.Category))
                              group ds by new
                              {
                                  Category = ds.Field<string>(DatabaseObjects.Columns.Category),
                                  SubCategory = ds.Field<string>(DatabaseObjects.Columns.SubCategory)
                              } into g
                              select new RequestReport
                              {
                                  Category = g.Key.Category,
                                  SubCategory = g.Key.SubCategory,
                                  TicketCount = g.Count()
                              }).OrderByDescending(x => x.TicketCount).ToList<RequestReport>();

            wccRequestReport.Width = Unit.Pixel((int)Width.Value - 10);
            wccRequestReport.Height = Unit.Pixel((int)Height.Value - 10);
            wccRequestReport.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            wccRequestReport.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            wccRequestReport.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            wccRequestReport.Legend.Direction = LegendDirection.LeftToRight;
            wccRequestReport.Legend.HorizontalIndent = 10;

            foreach (RequestReport item in RequestReports)
            {
                var seriesName = item.Category + (string.IsNullOrEmpty(item.SubCategory) ? "" : " > " + item.SubCategory);
                Series series = new Series(seriesName, ViewType.Bar);
                wccRequestReport.Series.Add(series);
                series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;

                SeriesPointCollection pointcollection = series.Points;
                pointcollection.Add(new SeriesPoint(seriesName, item.TicketCount));

                SideBySideBarSeriesLabel sidebysidebarSeriesLbl = series.Label as SideBySideBarSeriesLabel;
                sidebysidebarSeriesLbl.LineVisibility = DevExpress.Utils.DefaultBoolean.False;
                sidebysidebarSeriesLbl.Position = BarSeriesLabelPosition.Top;
                sidebysidebarSeriesLbl.TextPattern = "{V:#}";

                SideBySideBarSeriesView sidebysidebarSeriesView = series.View as SideBySideBarSeriesView;
                sidebysidebarSeriesView.BarWidth = 13;
                sidebysidebarSeriesView.BarDistance = 40;
            }

            XYDiagram diagram = wccRequestReport.Diagram as XYDiagram;
            diagram.AxisY.Title.Text = "# Tickets";
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisY.VisibleInPanesSerializable = "-1";
            diagram.AxisY.Interlaced = true;
            diagram.AxisY.NumericScaleOptions.AutoGrid = true;
            diagram.AxisY.NumericScaleOptions.GridSpacing = 10;

            diagram.AxisX.VisibleInPanesSerializable = "-1";
            diagram.AxisX.Label.Visible = false;
            diagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.False;
            diagram.AxisX.VisualRange.AutoSideMargins = false;
            diagram.AxisX.VisualRange.SideMarginsValue = 1.0;
        }

        public DateTime FirstDayOfWeek(DateTime date)
        {
            var candidateDate = date;
            while (candidateDate.DayOfWeek != DayOfWeek.Monday)
            {
                candidateDate = candidateDate.AddDays(-1);
            }
            return candidateDate;
        }

        private int GetISOWeek(DateTime d)
        {
            return System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }



        protected void gvTicketFlow_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "SolvedPct")
            {
                if (Convert.ToDouble(e.CellValue) > 1.0)
                {
                    e.Cell.ForeColor = ColorTranslator.FromHtml("#3FB33F");
                }
                else if (Convert.ToDouble(e.CellValue) < 1.0)
                {
                    e.Cell.ForeColor = ColorTranslator.FromHtml("#BB2727");
                }
            }

            if (e.DataColumn.FieldName == "BacklogImpact")
            {
                e.Cell.ForeColor = Color.White;
                if (Convert.ToDouble(e.CellValue) > 0.0)
                {
                    e.Cell.BackColor = ColorTranslator.FromHtml("#BB2727");
                }
                else if (Convert.ToDouble(e.CellValue) <= 0.0)
                {
                    e.Cell.BackColor = ColorTranslator.FromHtml("#3FB33F");
                }
            }
        }

        protected void gvLastWeekvs12Week_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Performance")
            {
                if (Convert.ToDouble(e.CellValue) > 0.0)
                {
                    e.Cell.BackColor = ColorTranslator.FromHtml("#3FB33F");
                    e.Cell.ForeColor = Color.White;
                }
                else if (Convert.ToDouble(e.CellValue) < 0.0)
                {
                    e.Cell.BackColor = ColorTranslator.FromHtml("#BB2727");
                    e.Cell.ForeColor = Color.White;
                }
            }
            if (e.DataColumn.FieldName == "TicketAssignee")
            {
                ticketAssignee = Convert.ToString(e.CellValue);
            }
            if (e.DataColumn.FieldName == "Solved")
            {
                int weeks = 0;
                if (!int.TryParse(txtLastWeekvs12Week.Text.Trim(), out weeks))
                {
                    weeks = 12;
                    txtLastWeekvs12Week.Text = weeks.ToString();
                }

                string title = string.Format("{0}: # Solved Tickets", ticketAssignee);
                string newUrl = string.Format("{0}&Control=weeklydrilldata&type=solve&assigneePRP={1}&weeks={2}", url, ticketAssignee, weeks);
                e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0,\"{3}\");'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(newUrl), title, Uri.EscapeUriString(Request.Url.AbsolutePath));

            }
            if (e.DataColumn.FieldName == "WeeklyAverage")
            {
                int weeks = 0;
                if (!int.TryParse(txtLastWeekvs12Week.Text.Trim(), out weeks))
                {
                    weeks = 12;
                    txtLastWeekvs12Week.Text = weeks.ToString();
                }
                string newUrl = string.Format("{0}&Control=weeklydrilldata&type=weekly&assigneePRP={1}&weeks={2}", url, ticketAssignee, weeks);
                e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0,\"{3}\");'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(newUrl), string.Format("{0}: Weekly Average", ticketAssignee), Uri.EscapeUriString(Request.Url.AbsolutePath));
            }
        }

        protected void gvScoreBoard_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "TicketGroup")
            {
                ticketgroup = Convert.ToString(e.CellValue);
            }
            else if (e.DataColumn.FieldName == "SolvedTickets")
            {
                string url = string.Format("{0}&startdate={1}&enddate={2}&functionalarea={3}&status={4}", GrpScoreCardurl, Convert.ToString(deStartdt.Date), Convert.ToString(deEnddt.Date), ticketgroup, "Closed");
                if (UGITUtility.StringToInt(e.CellValue) > 0)
                    e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0);'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(url), "Group By: " + ticketgroup + "");
            }
            else if (e.DataColumn.FieldName == "UnsolvedTickets")
            {
                string url = string.Format("{0}&startdate={1}&enddate={2}&functionalarea={3}&status={4}", GrpScoreCardurl, Convert.ToString(deStartdt.Date), Convert.ToString(deEnddt.Date), ticketgroup, "NotClosed");
                if (UGITUtility.StringToInt(e.CellValue) > 0)
                    e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0);'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(url), "Group By: " + ticketgroup + "");
            }
        }
        private string ticketGroupPredictedBacklog = String.Empty;
        protected void gvPredictedBacklog_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Backlog")
            {
                if (Convert.ToDouble(e.CellValue) > 2.0)
                {
                    e.Cell.BackColor = ColorTranslator.FromHtml("#BB2727");
                    e.Cell.ForeColor = Color.White;
                }
            }
            else if (e.DataColumn.FieldName == "TicketGroup")
            {
                ticketGroupPredictedBacklog = Convert.ToString(e.CellValue);
            }
            else if (e.DataColumn.FieldName == "UnsolvedTicket")
            {
                string url = string.Format("{0}{1}&weeks={2}&functionalarea={3}&status={4}", scoreCardurl, "predictedbacklogdrilldowndata", txtPredictedBackLogFilter.Text, ticketGroupPredictedBacklog, "Unsolved");
                if (UGITUtility.StringToInt(e.CellValue) > 0)
                    e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0);'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(url), "Group By: " + ticketGroupPredictedBacklog + "");
            }
            else if (e.DataColumn.FieldName == "SolvedWeeklyAvg")
            {
                string url = string.Format("{0}{1}&weeks={2}&functionalarea={3}&status={4}", scoreCardurl, "predictedbacklogdrilldowndata", txtPredictedBackLogFilter.Text, ticketGroupPredictedBacklog, "UnsolvedPerWeek");
                if (UGITUtility.StringToInt(e.CellValue) > 0)
                    e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0);'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(url), "Group By: " + ticketGroupPredictedBacklog + "");

            }
        }

        protected void gvPredictedBacklog_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                PredictedBacklog objPredictedBacklog = gvPredictedBacklog.GetRow(e.VisibleIndex) as PredictedBacklog;
                if (objPredictedBacklog != null)
                {
                    object cell = e.Row.Cells[3];
                    if (cell is GridViewTableDataCell)
                    {
                        GridViewTableDataCell editCell = (GridViewTableDataCell)cell;
                        if (((GridViewDataColumn)editCell.Column).FieldName == "Backlog")
                        {
                            if (objPredictedBacklog.Backlog == 0.0)
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "--";
                            }
                        }
                    }

                }
            }
        }

        private void GenerateDataAgentPerformance(int numberOfAgent, int lastWeeks)
        {
            LoadData();
            //DataTable dtDashboardSummary = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");
            if (dtDashboardSummary == null || dtDashboardSummary.Rows.Count == 0)
                return;

            List<AgentPerformanceStat> topAgents = new List<AgentPerformanceStat>();

            DataRow[] filteredData = dtDashboardSummary.Select(string.Format("{0} is not null and {1} is not null and {1} <> '' ", DatabaseObjects.Columns.TicketCreationDate, DatabaseObjects.Columns.TicketPRP));

            topAgents = (from ds in filteredData
                         where smsModules.Contains(ds.Field<string>(DatabaseObjects.Columns.ModuleNameLookup)) && ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate) >= DateTime.Now.AddDays(-7 * lastWeeks)
                         orderby ds.Field<DateTime>(DatabaseObjects.Columns.TicketCreationDate)
                         group ds by new
                         {
                             PRP = ds.Field<string>(DatabaseObjects.Columns.TicketPRP)
                         } into g
                         select new AgentPerformanceStat
                         {
                             PRP = g.Key.PRP,
                             TicketCount = g.Count()
                         }).ToList<AgentPerformanceStat>();

            wccAgentPerformance.Series.Clear();

            topAgents = topAgents.OrderBy(x => x.TicketCount).Take(numberOfAgent).ToList();

            wccAgentPerformance.Width = new Unit(Width.Value - 30, UnitType.Pixel);
            wccAgentPerformance.Height = new Unit(Height.Value - 30, UnitType.Pixel);

            wccAgentPerformance.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            //wccTicketsCreatedByWeek.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            //wccTicketsCreatedByWeek.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;

            Series barchartseries = new Series("# Tickets", ViewType.StackedBar);

            barchartseries.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            barchartseries.Label.TextOrientation = TextOrientation.Horizontal;

            //PointSeriesLabel pntslabel = linchartseries.Label as PointSeriesLabel;
            //pntslabel.Angle = 90;
            //pntslabel.TextPattern = "{V:n2}";
            //linchartseries.CrosshairLabelPattern = "{S}: {V:n2}";
            wccAgentPerformance.Series.Add(barchartseries);

            XYDiagram diagram = wccAgentPerformance.Diagram as XYDiagram;
            diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.False;
            diagram.AxisX.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisX.Label.Angle = 300;

            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Font = new Font(FontFamily.GenericSansSerif, 9);
            diagram.AxisY.Title.Text = "# Solved Tickets";
            diagram.AxisY.NumericScaleOptions.AutoGrid = false;
            diagram.AxisY.NumericScaleOptions.GridSpacing = 1;
            diagram.Rotated = true;


            SeriesPointCollection pointcollection = barchartseries.Points;
            foreach (AgentPerformanceStat item in topAgents)
            {
                pointcollection.Add(new SeriesPoint(item.PRP, item.TicketCount));
            }
        }
        protected void gvOldestUnsolved_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketId)
            {
                GenerateUrl((DevExpress.Web.ASPxGridView)sender, e, DatabaseObjects.Columns.TicketId);
            }
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Title)
            {
                GenerateUrl((DevExpress.Web.ASPxGridView)sender, e, DatabaseObjects.Columns.Title);
            }
        }

        private void GenerateUrl(DevExpress.Web.ASPxGridView sender, ASPxGridViewTableDataCellEventArgs e, string columnname)
        {
            UnSolvedTicket obj = (UnSolvedTicket)sender.GetRow(e.VisibleIndex);
            string ticketUrl = string.Empty;
            if (obj != null && columnname == DatabaseObjects.Columns.TicketId)
            {
                moduleid = uHelper.getModuleIdByTicketID(context, Convert.ToString(e.CellValue));
                moduleName = uHelper.getModuleNameByTicketId(Convert.ToString(e.CellValue));
                //ticketUrl = Ticket.GenerateTicketURL(moduleid, Convert.ToString(e.CellValue), SPContext.Current.Web);
                ticketUrl = Ticket.GenerateTicketURL(context, moduleName, Convert.ToString(e.CellValue));
                e.Cell.Text = string.Format("<a href='javascript:' onclick='OpenTicket(\"{0}\",\"{1}\",\"{2}\")'>{0}</a>", e.CellValue, obj.Title, ticketUrl);
            }
            else if (obj != null && columnname == DatabaseObjects.Columns.Title)
            {
                moduleid = uHelper.getModuleIdByTicketID(context, obj.TicketId);
                moduleName = uHelper.getModuleNameByTicketId(Convert.ToString(e.CellValue));
                //ticketUrl = Ticket.GenerateTicketURL(moduleid, obj.TicketId, SPContext.Current.Web);
                ticketUrl = Ticket.GenerateTicketURL(context, moduleName, obj.TicketId);
                e.Cell.Text = string.Format("<a href='javascript:' onclick='OpenTicket(\"{0}\",\"{1}\",\"{2}\")'>{1}</a>", obj.TicketId, e.CellValue, ticketUrl);
            }
        }

        protected void gvGrpUnsolvedTickets_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            DevExpress.Web.ASPxGridView grid = sender as DevExpress.Web.ASPxGridView;

            if (grid == null)
            {
                e.Cell.Text = Convert.ToString(e.CellValue);
                return;
            }

            if (e.DataColumn.FieldName == "UnsolvedTicket")
            {
                PredictedBacklog obj = (PredictedBacklog)grid.GetRow(e.VisibleIndex);
                if (obj != null)
                {
                    string newUrl = string.Format("{0}Control=groupunsolvedTicket&group={1}", url, obj.TicketGroupId);
                    e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0,\"{3}\");'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(newUrl), obj.TicketGroup, Uri.EscapeUriString(Request.Url.AbsolutePath));
                }
                else { e.Cell.Text = Convert.ToString(e.CellValue); }
            }
            else if (e.DataColumn.FieldName == "TicketGroup")
            {
                PredictedBacklog obj = (PredictedBacklog)grid.GetRow(e.VisibleIndex);
                if (obj != null)
                {
                    string newUrl = string.Format("{0}Control=groupunsolvedTicket&group={1}", url, obj.TicketGroupId);
                    e.Cell.Text = string.Format("<a href='javascript:' onclick='window.parent.UgitOpenPopupDialog(\"{1}\",\"\", \"{2}\", \"80\", \"90\", 0,\"{3}\");'>{0}</a>", e.CellValue, UGITUtility.GetAbsoluteURL(newUrl), obj.TicketGroup, Uri.EscapeUriString(Request.Url.AbsolutePath));
                }
                else { e.Cell.Text = Convert.ToString(e.CellValue); }
            }
        }

        private void LoadData()
        {
            dtDashboardSummary = DashboardCache.GetCachedDashboardData(HttpContext.Current.GetManagerContext(), DatabaseObjects.Tables.DashboardSummary);

            if (dtDashboardSummary != null)
            {
                dtDashboardSummary.AcceptChanges();
            }
        }
    }




    public class GroupScoreCardData
    {
        public string TicketGroup { get; set; }
        public long? TicketGroupId { get; set; }
        public double? Reopen { get; set; }
        public int SolvedTickets { get; set; }
        public int UnsolvedTickets { get; set; }
    }

    public class TicketFlowByGroup
    {
        public string TicketGroup { get; set; }
        public long? TicketGroupId { get; set; }
        public double? Created { get; set; }
        public double? Solved { get; set; }
        public double? SolvedPct { get; set; }
        public double? BacklogImpact { get; set; }
        public double? Backlog { get; set; }
    }

    public class WeeklyRollingAverage
    {
        public string TicketAssignee { get; set; }
        public double? Solved { get; set; }
        public double? WeeklyAverage { get; set; }
        public double? Performance { get; set; }
    }

    public class PredictedBacklog
    {
        public string TicketGroup { get; set; }
        public long? TicketGroupId { get; set; }
        public double? UnsolvedTicket { get; set; }
        public double? SolvedWeeklyAvg { get; set; }
        public double? Backlog { get; set; }
    }

    public class TicketsByWeek
    {
        public DateTime FirstDateOfWeek { get; set; }
        public string Week { get; set; }
        public int TicketCount { get; set; }
    }

    public class ProblemReport
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int TicketCount { get; set; }
    }

    public class RequestReport
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int TicketCount { get; set; }
    }

    public class UnSolvedTicket
    {
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string TicketAssignee { get; set; }
        public string TicketStatus { get; set; }
        public int TicketAge { get; set; }
    }

    public class TSRTrendChart
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Week { get; set; }
        public int TicketCount { get; set; }
    }

    public class AgentPerformanceStat
    {
        public string PRP { get; set; }
        public int TicketCount { get; set; }
    }

    public enum TypeOfReport
    {
        ScoreCard,
        TicketFlow,
        WeeklyRollingAverage,
        TicketCreatedByWeek,
        PredictBacklog,
        GroupUnsolvedTickets,
        OldestUnsolved,
        TSRTrendChart,
        ProblemReport,
        RequestReport,
        AgentPerformance
    }
}