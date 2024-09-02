
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using uGovernIT.Manager.Report.Entities;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;

namespace uGovernIT.Report.DxReport
{
    public class ProjectReportHelper
    {
        public DataTable sourcetable;
        ApplicationContext context;
        public ProjectReportHelper()
        {
        }

        public TSKProjectReportEntity GetProjectReportEntity(ApplicationContext context, TSKProjectReportEntity prEntity, int[] PMMIds)
        {
            TicketManager ticketManager = new TicketManager(context);
            UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
            // Get company logo to show on report header
            Tuple<string, Image> cLogo = uHelper.GetCompanyLogo(context);
            if (cLogo != null)
            {
                prEntity.CompanyLogo = cLogo.Item1;
                prEntity.CompanyLogoBitmap = cLogo.Item2;
                if (cLogo.Item2 != null)
                {
                    prEntity.LogoHeight = cLogo.Item2.Height;
                    prEntity.LogoWidth = cLogo.Item2.Width;
                }
            }

            ///Expression for PMM and Other related Table.
            List<string> pmmqueryExpressions = new List<string>();
            List<string> commonexpressions = new List<string>();
            List<string> pmmQueryOr = new List<string>();
            List<string> commonQueryOr = new List<string>();
            string qexpression = string.Empty;
            string queryexpression = string.Empty;
            string _query = string.Empty;
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule module = moduleViewManager.LoadByName(ModuleNames.PMM);
            DataTable dtallProject= GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, context.TenantID));
            DataRow drRow = null;
            if (PMMIds != null && PMMIds.Length > 0)
            {
                for (int i = 0; i < PMMIds.Length; i++)
                {
                    double pmmid = UGITUtility.StringToDouble(PMMIds[i]);
                    if (pmmid == 0)
                        continue;
                    pmmqueryExpressions.Add(UGITUtility.ObjectToString(pmmid));
                    drRow = dtallProject.AsEnumerable().FirstOrDefault(x => x.Field<Int64>(DatabaseObjects.Columns.ID) == pmmid);
                    if (drRow != null)
                    commonexpressions.Add(string.Format("'{0}'", UGITUtility.ObjectToString(drRow[DatabaseObjects.Columns.TicketId])));
                    // spliting IN condition with the help of OR
                    if ((i > 0 && i % 300 == 0) || i == PMMIds.Length - 1)
                    {
                        pmmQueryOr.Add(string.Format("{0} IN ({1})",DatabaseObjects.Columns.ID, string.Join(",", pmmqueryExpressions.ToArray())));
                        commonQueryOr.Add(string.Format("{0} IN ({1})", DatabaseObjects.Columns.TicketId, string.Join(",", commonexpressions.ToArray())));
                        pmmqueryExpressions.Clear();
                        commonexpressions.Clear();
                    }
                }

                if(pmmQueryOr.Count>0)
                    qexpression = "(" + string.Join(" OR ", pmmQueryOr) + ")";

                if(commonQueryOr.Count>0)
                    queryexpression = "(" + string.Join(" OR ", commonQueryOr) + ")";

                pmmqueryExpressions.Add(qexpression);
                commonexpressions.Add(queryexpression);

            }

            _query = qexpression; //string.Format("<Where>{0}</Where>", qexpression);
            DataRow[] spListItemColl = dtallProject.Select(_query);

            if (spListItemColl != null && spListItemColl.Length > 0)
            {
                sourcetable = spListItemColl.CopyToDataTable();
                foreach (DataRow item in spListItemColl)
                {
                    int pmmid = Convert.ToInt32(item[DatabaseObjects.Columns.ID]);
                    string beneficiary = Convert.ToString(item[DatabaseObjects.Columns.TicketBeneficiaries]);
                    beneficiary = uHelper.FormatDepartment(context, beneficiary, true);
                    string projectSummary = string.Empty;

                    List<HistoryEntry> historyList = uHelper.GetHistory(item, DatabaseObjects.Columns.ProjectSummaryNote);
                    if (historyList != null && historyList.Count > 0)
                    {
                        projectSummary = historyList.First().entry.Replace("<br>", "\r\n");
                    }

                    sourcetable.AsEnumerable().Where(dr => dr.Field<long>(DatabaseObjects.Columns.ID) == pmmid).ToList()
                    .ForEach(row =>
                    {
                        row.SetField(DatabaseObjects.Columns.ProjectSummaryNote, projectSummary);
                        row.SetField(DatabaseObjects.Columns.TicketBeneficiaries, beneficiary);
                    });
                }
                sourcetable = uHelper.ConvertTableLookupValues(context, sourcetable);
                DataTable dtProject = sourcetable.Clone();
                dtProject.Columns.Add("CompanyLogo", typeof(string));
                dtProject.Columns.Add("OverallProjectScoreColor", typeof(string));
                DataSet dsPlanned = new DataSet("PlannedMonthlyBudget");
                DataSet dsActuals = new DataSet("ActualsMonthlyBudget");
                foreach (DataRow dr in sourcetable.Rows)
                {
                    double score = 0;
                    Double.TryParse(Convert.ToString(dr[DatabaseObjects.Columns.TicketProjectScore]), out score);
                    ///Overall project score Color for Project Report Status Component..
                    string oapScoreImageUrl = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl(context, score));
                    List<object> listobject = dr.ItemArray.ToList();
                    if (cLogo != null)
                        listobject.Add(cLogo.Item1);
                    else
                        listobject.Add(string.Empty);
                    listobject.Add(oapScoreImageUrl);
                    dtProject.Rows.Add(listobject.ToArray());
                    int pmmid = Convert.ToInt32(dr[DatabaseObjects.Columns.Id]);

                    #region Planned vs Actual monthly Report
                    if (prEntity.ShowPlannedvsActualByMonth)
                    {

                        dsPlanned.Tables.Add(GetPlannedDistribution(context, pmmid));
                        dsActuals.Tables.Add(GetActualsDistribution(context, pmmid));
                        prEntity.PlannedMonthlyBudget = dsPlanned;
                        prEntity.ActualMonthlyBudget = dsActuals;
                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(prEntity.SortOrder) && dtProject != null && dtProject.Rows.Count > 0)
                {
                    List<string> lstOfSortingColumn = prEntity.SortOrder.Split(',').ToList();

                    DataView dtView = new DataView(dtProject);
                    DataTable dt = new DataTable();
                    List<string> sortColumn = new List<string>();
                    foreach (string column in lstOfSortingColumn)
                    {
                        if (dtProject.Columns.Contains(column))
                            sortColumn.Add(column);
                    }
                    if (sortColumn.Count > 0)
                        dtView.Sort = string.Join(",", sortColumn);

                    dt = dtView.ToTable();
                    dtProject = dt;

                }
                prEntity.Projects = dtProject;
            }

            #region Bind Accomplishments
            if (prEntity.ShowAccomplishment)
            {
                List<string> accqueryExpressions = new List<string>();
                string accompleshmentQuery = string.Empty;
                if (!string.IsNullOrEmpty(queryexpression))
                {
                    accqueryExpressions.Add(queryexpression);
                }
                accqueryExpressions.Add(string.Format("{0}='{1}'",
                                                       DatabaseObjects.Columns.ProjectNoteType, "Accomplishments"));
                accqueryExpressions.Add(string.Format("{0}<>'{1}'",
                                                       DatabaseObjects.Columns.Deleted, "True"));
                if (accqueryExpressions.Count > 0)
                {
                    accompleshmentQuery = "(" + string.Join(" And ", accqueryExpressions) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(accqueryExpressions, accqueryExpressions.Count - 1, true));
                }

                DataRow[] accomplishments = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMComments, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(accompleshmentQuery);
                if (accomplishments != null && accomplishments.Length > 0)
                {
                    //prEntity.Accomplishment = accomplishments.CopyToDataTable();
                    DataView dataView = accomplishments.CopyToDataTable().DefaultView;
                    dataView.Sort = string.Format("{0} DESC", DatabaseObjects.Columns.AccomplishmentDate);
                    prEntity.Accomplishment = dataView.ToTable();
                }
            }
            #endregion

            #region Bind Immediate Plans
            if (prEntity.ShowPlan)
            {
                List<string> planqueryExpressions = new List<string>();
                string planQuery = string.Empty;
                if (!string.IsNullOrEmpty(queryexpression))
                {
                    planqueryExpressions.Add(queryexpression);
                }
                planqueryExpressions.Add(string.Format("{0}='{1}'",
                                                        DatabaseObjects.Columns.ProjectNoteType, "Immediate Plans"));
                planqueryExpressions.Add(string.Format("{0}<>'{1}'",
                                                      DatabaseObjects.Columns.Deleted, "True"));

                if (planqueryExpressions.Count > 0)
                {
                    planQuery = "(" + string.Join(" And ", planqueryExpressions) + ")";  //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(planqueryExpressions, planqueryExpressions.Count - 1, true));
                }

                DataTable dtImmediatePlans = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMComments, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");  //changed to pmmcomments from moduletasks bcz pmmcomment column not availabe in moduletask table
                if (dtImmediatePlans != null && dtImmediatePlans.Rows.Count > 0)
                {
                    DataRow[] Immediateplans = dtImmediatePlans.Select(planQuery);
                    if (Immediateplans != null && Immediateplans.Count() > 0)
                        prEntity.ImmediatePlans = Immediateplans.CopyToDataTable();
                }
            }
            #endregion

            #region Bind Issues
            if (prEntity.ShowIssues)
            {
                string issueQuery = string.Empty;
                List<string> issuesExpressions = new List<string>();
                if (!string.IsNullOrEmpty(queryexpression))
                {
                    issuesExpressions.Add(queryexpression);
                }
                issuesExpressions.Add(string.Format("{0}<>'{1}'",
                                                     DatabaseObjects.Columns.Deleted, "True"));
                if (issuesExpressions.Count > 0)
                {
                    issueQuery = "(" + string.Join(" And ", issuesExpressions) + ")"; 
                }


                DataRow[] issues = uGITTaskManager.GetDataTable(issueQuery).Select(); 
                if (issues != null && issues.Count() > 0)
                {
                    issues = issues.Where(x => !x.IsNull(DatabaseObjects.Columns.UGITSubTaskType) && x.Field<string>(DatabaseObjects.Columns.UGITSubTaskType).EqualsIgnoreCase("issue")).ToArray();
                    if (issues != null && issues.Length > 0)
                        prEntity.Issues = issues.CopyToDataTable();
                    else
                        prEntity.Issues = null;
                }
                prEntity.Issues= uHelper.ConvertTableLookupValues(context, prEntity.Issues);
                //if (prEntity.Issues != null && prEntity.Issues.Rows.Count > 0 && uHelper.IfColumnExists(DatabaseObjects.Columns.AssignedTo, prEntity.Issues))
                //{
                //    prEntity.Issues.AsEnumerable().ToList().ForEach(x => UpdateColumnContent(x, DatabaseObjects.Columns.AssignedTo));
                //}
            }
            #endregion

            #region Bind Risks
            if (prEntity.ShowRisk)
            {
                string issueQuery = string.Empty;
                List<string> issuesExpressions = new List<string>();
                if (!string.IsNullOrEmpty(queryexpression))
                {
                    issuesExpressions.Add(queryexpression);
                }
                issuesExpressions.Add(string.Format("{0}<>'{1}'",
                                                     DatabaseObjects.Columns.Deleted, "True"));
                if (issuesExpressions.Count > 0)
                {
                    issueQuery = "(" + string.Join(" And ", issuesExpressions) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(issuesExpressions, issuesExpressions.Count - 1, true));
                }


                DataRow[] risks = uGITTaskManager.GetDataTable(issueQuery).Select(); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMRisks).Select(issueQuery);
                if (risks != null && risks.Count() > 0)
                {
                    risks = risks.Where(x => !x.IsNull(DatabaseObjects.Columns.UGITSubTaskType) && x.Field<string>(DatabaseObjects.Columns.UGITSubTaskType).EqualsIgnoreCase("risk")).ToArray();
                    if (risks != null && risks.Length > 0)
                        prEntity.Risks = uHelper.ConvertTableLookupValues(context, risks.CopyToDataTable());
                    else
                        prEntity.Risks = null;
                }

                if (prEntity.Risks != null && prEntity.Risks.Rows.Count > 0 && uHelper.IfColumnExists(DatabaseObjects.Columns.AssignedTo, prEntity.Risks))
                {
                    prEntity.Risks.AsEnumerable().ToList().ForEach(x => UpdateColumnContent(x, DatabaseObjects.Columns.AssignedTo));
                }
            }
            #endregion

            #region monitor state
            if (prEntity.ShowMonitorState)
            {
                ProjectMonitorStateManager projectMonitorStateManager = new ProjectMonitorStateManager(context);
                List<ProjectMonitorState> monitorStates=  projectMonitorStateManager.Load(x => x.TenantID == context.TenantID);
                var projectMonitorState = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                ModuleMonitorOptionManager moduleMonitorOptionManager = new ModuleMonitorOptionManager(context);
                List<ModuleMonitorOption> moduleMonitorOptions= moduleMonitorOptionManager.Load(x => x.TenantID == context.TenantID);
                ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(context);
                var moduleMonitors = moduleMonitorManager.Load(x => x.TenantID == context.TenantID);
                var projectMonitorOptions = moduleMonitorOptions;
                string queryPMMSpecificMonitors = string.Empty;
                queryPMMSpecificMonitors = queryexpression; //string.Format("<Where>{0}</Where>", queryexpression);
                DataRow[] monitors = UGITUtility.ToDataTable<ProjectMonitorState>(monitorStates).Select(queryPMMSpecificMonitors);
                DataTable monitorState = null;
                if (monitors != null && monitors.Count() > 0)
                {
                    monitorState = monitors.CopyToDataTable();
                }
                if (monitorState != null && monitorState.Rows.Count > 0)
                {
                    DataColumn dataColumn = new DataColumn(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup, typeof(string));
                    DataColumn dataColumn1 = new DataColumn(DatabaseObjects.Columns.ModuleMonitorName, typeof(string));
                    monitorState.Columns.Add(dataColumn);
                    monitorState.Columns.Add(dataColumn1);
                    foreach (DataRow dr in monitorState.Rows)
                    {
                        ModuleMonitor moduleMonitor = moduleMonitors.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.ModuleMonitorNameLookup]));
                        ModuleMonitorOption moduleMonitorOption = projectMonitorOptions.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                        dr[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl(Convert.ToString(moduleMonitorOption.ModuleMonitorOptionLEDClass)));
                        dr[DatabaseObjects.Columns.ModuleMonitorName] = moduleMonitor.MonitorName;
                        dr[DatabaseObjects.Columns.ModuleMonitorOptionName] = moduleMonitorOption.ModuleMonitorOptionName;
                    }
                    prEntity.MonitorState = monitorState;
                }
            }
            #endregion

            #region Planned vs Actual by Category  (ShowPlannedvsActualByCategory || ShowPlannedvsActualByBudgetItem || ShowBudgetSummary)

            if (prEntity.ShowPlannedvsActualByCategory || prEntity.ShowPlannedvsActualByBudgetItem || prEntity.ShowBudgetSummary)
            {
                prEntity.PlannedvsActualsbyCategory = LoadBudgetSummaryReport(context, queryexpression);
            }
            #endregion

            #region All Tasks
            DataTable organizedTasks = null;

            if (prEntity.Projects.Rows.Count > 0)
            {
                foreach (DataRow row in prEntity.Projects.Rows)
                {
                    DataTable tasks;
                    tasks = uGITTaskManager.GetAllTasksByProjectID("PMM", Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                    if (tasks != null && tasks.Rows.Count > 0)
                    {

                        if (organizedTasks != null && organizedTasks.Rows.Count > 0)
                        {
                            organizedTasks.Merge(tasks);
                        }
                        else
                        {
                            organizedTasks = tasks.Copy();
                        }
                    }
                }
            }
            else
            {
                organizedTasks = GetAllTasks(context, PMMIds, TicketStatus.All, "PMM");
            }

            uGITTaskManager.UpdatePredecessorsByOrder(organizedTasks);
            prEntity.Tasks = organizedTasks;
            if (prEntity.ShowOpenTaskOnly)
            {
                if (organizedTasks != null && organizedTasks.Columns.Contains(DatabaseObjects.Columns.Status))
                {
                    DataRow[] rRows = organizedTasks.Select(string.Format("{0} <> 'Completed'", DatabaseObjects.Columns.Status));
                    prEntity.OpenTasks = null;
                    if (rRows.Length > 0)
                    {
                        DataTable sData = rRows.CopyToDataTable(); ;
                        uGITTaskManager.UpdatePredecessorsByOrder(sData);
                        prEntity.OpenTasks = sData;
                    }
                }
            }
            #endregion

            #region Summary Task (ShowMilestone || ShowSummaryGanttChart)
            if (prEntity.ShowMilestone || prEntity.ShowSummaryGanttChart)
            {
                DataTable summaryTasks = null;
                if (prEntity.Projects.Rows.Count > 0)
                {
                    foreach (DataRow row in prEntity.Projects.Rows)
                    {
                        DataTable tasks = uGITTaskManager.GetSummaryTasksByProjectID("PMM", Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                        if (tasks != null && tasks.Rows.Count > 0)
                        {
                            if (summaryTasks != null && summaryTasks.Rows.Count > 0)
                                summaryTasks.Merge(tasks);
                            else
                                summaryTasks = tasks.Copy();
                        }
                    }
                }
                else
                {
                    summaryTasks = GetSummaryTasks(context, PMMIds, TicketStatus.All, "PMM");
                }

                if (prEntity.ShowAllMilestone)
                {
                    if (summaryTasks != null && summaryTasks.Rows.Count > 0)
                    {
                        //Those milestone are not part of summary which root task is completed.
                        //Only Show root task for it.
                        if (summaryTasks.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour))
                        {
                            DataRow[] summaryTaskRows = summaryTasks.Select(string.Format("{0} = 'milestone'", DatabaseObjects.Columns.TaskBehaviour));
                            //DataRow[] summaryTaskRows = summaryTasks.Select(string.Format("{0} <> 'Issue' and {0} <> 'Risk'", "SubTaskType"));
                            if (summaryTaskRows.Length > 0)
                            {
                                DataTable sData = summaryTaskRows.CopyToDataTable();
                                summaryTasks = sData;
                            }
                            else
                                summaryTasks = null;
                        }
                    }
                }

                if (summaryTasks != null && summaryTasks.Rows.Count > 0)
                {
                    uGITTaskManager.UpdatePredecessorsByOrder(summaryTasks);
                    prEntity.SummaryTasks = summaryTasks;
                }
            }
            #endregion

            #region Resource Allocation
            if (prEntity.ShowResourceAllocation)
            {
                DataTable budgetReportTable = GetSourceData(context, queryexpression);
                prEntity.BudgetAllocation = budgetReportTable;
            }
            #endregion

            #region Decision Log
            if (prEntity.ShowDecisionLog)
                GetDecisionLog(context, prEntity, queryexpression);
            #endregion

            if (prEntity.Projects != null && prEntity.Projects.Rows.Count > 0)
            {
                List<HistoryEntry> AllHistoryList = new List<HistoryEntry>();
                DataTable dtExecutiveHistory = new DataTable();
                dtExecutiveHistory.Columns.Add(DatabaseObjects.Columns.TicketId);
                dtExecutiveHistory.Columns.Add("Data");
                dtExecutiveHistory.Columns.Add("CreatedByUser");
                dtExecutiveHistory.Columns.Add("Created");

                foreach (DataRow dr in prEntity.Projects.Rows)
                {
                    DataRow pmmItem = Ticket.GetCurrentTicket(context, "PMM", Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                    if (pmmItem != null)
                    {
                        List<HistoryEntry> historyList = uHelper.GetHistory(pmmItem, DatabaseObjects.Columns.ProjectSummaryNote);
                        if (historyList != null)
                        {
                            foreach (HistoryEntry hE in historyList)
                            {
                                dtExecutiveHistory.Rows.Add(new object[] { Convert.ToString(dr[DatabaseObjects.Columns.TicketId]), hE.entry, hE.createdBy, hE.created });
                            }

                        }
                    }
                }
                prEntity.ExecutiveHistory = dtExecutiveHistory;
            }

            return prEntity;
        }


        #region Planned vs Actual by Category
        private DataTable CreateBudgetTable()
        {
            DataTable projectBudgetTable = new DataTable();
            projectBudgetTable.Columns.Add("TicketId", typeof(string));
            projectBudgetTable.Columns["TicketId"].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add("BudgetCategory", typeof(string));
            projectBudgetTable.Columns["BudgetCategory"].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add("BudgetSubCategory", typeof(string));
            projectBudgetTable.Columns["BudgetSubCategory"].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add("CategoryGLCode", typeof(string));
            projectBudgetTable.Columns["CategoryGLCode"].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add("SubCategoryGLCode", typeof(string));
            projectBudgetTable.Columns["SubCategoryGLCode"].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add("BudgetItem", typeof(string));
            projectBudgetTable.Columns["BudgetItem"].DefaultValue = string.Empty;
            projectBudgetTable.Columns.Add("Planned", typeof(double));
            projectBudgetTable.Columns["Planned"].DefaultValue = 0.0;
            projectBudgetTable.Columns.Add("BaseLine", typeof(double));
            projectBudgetTable.Columns["BaseLine"].DefaultValue = 0.0;
            projectBudgetTable.Columns.Add("Actual", typeof(double));
            projectBudgetTable.Columns["Actual"].DefaultValue = 0.0;

            return projectBudgetTable;
        }

        private DataTable LoadBudgetSummaryReport(ApplicationContext context, string expression)
        {
            DataTable projectBudgetSummarizeTable = null;
            ModuleBudgetManager moduleBudgetManager = new ModuleBudgetManager(context);
            BudgetActualsManager budgetActualsManager = new BudgetActualsManager(context);
            //DataTable projectBudgetTable = CreateBudgetTable();
            DataTable PMMPlanBudgetList = moduleBudgetManager.GetDataTable();
            DataTable PMMBudgetActualList = budgetActualsManager.GetDataTable();
            DataTable PMMBaseLineList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetHistory, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            //Get the project item.
            string query = string.Empty;
            query = expression;
            List<BudgetByCategory> BudgetByCategoryList = new List<BudgetByCategory>();
            List<BudgetByCategory> BudgetByCategoryListPlanned = new List<BudgetByCategory>();
            List<BudgetByCategory> BudgetByCategoryListActual = new List<BudgetByCategory>();
            if (PMMPlanBudgetList.Rows.Count > 0 && PMMBudgetActualList.Rows.Count > 0)
            {
                DataRow[] PMMPlanBudgetFilter = PMMPlanBudgetList.Select(query);
                DataTable PMMBaseLineTable = new DataTable(); //PMMBaseLineList.Select(query).CopyToDataTable();                

                if (PMMPlanBudgetFilter != null && PMMPlanBudgetFilter.Count() > 0)
                {
                    DataTable PMMPlanBudgetTable = PMMPlanBudgetFilter.CopyToDataTable();
                    DataRow[] PMMBudgetActualFilter = PMMBudgetActualList.Select(query);
                    BudgetByCategoryListPlanned = (from planned in PMMPlanBudgetTable.AsEnumerable()
                                                   group planned by planned.Field<string>(DatabaseObjects.Columns.BudgetItem) into g
                                                   select new BudgetByCategory
                                                   {
                                                       TicketId = g.Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).FirstOrDefault(),
                                                       BudgetSubCategory = g.Select(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup)).FirstOrDefault(),
                                                       BudgetItem = g.Key,
                                                       Planned = g.Sum(x => Convert.ToDouble(x.Field<object>(DatabaseObjects.Columns.BudgetAmount)))
                                                   }).ToList();

                    if (PMMBudgetActualFilter != null && PMMBudgetActualFilter.Count() > 0)
                    {

                        DataTable PMMBudgetActualTable = PMMBudgetActualFilter.CopyToDataTable();
                        BudgetByCategoryListActual = (from actual in PMMBudgetActualTable.AsEnumerable()
                                                      group actual by actual.Field<string>(DatabaseObjects.Columns.ModuleBudgetLookup) into g
                                                      select new BudgetByCategory
                                                      {
                                                          TicketId = g.Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).FirstOrDefault(),
                                                          BudgetItem = g.Key,
                                                          Actual = g.Sum(x => Convert.ToDouble(x.Field<object>(DatabaseObjects.Columns.BudgetAmount)))
                                                      }).ToList();

                        BudgetByCategoryList = (from planned in BudgetByCategoryListPlanned
                                                join actual in BudgetByCategoryListActual
                                                on planned.BudgetItem equals
                                                   actual.BudgetItem into gj
                                                from _actual in gj.DefaultIfEmpty()
                                                select new BudgetByCategory
                                                {
                                                    TicketId = planned.TicketId,
                                                    BudgetSubCategory = planned.BudgetSubCategory,
                                                    BudgetItem = planned.BudgetItem,
                                                    Planned = planned.Planned,
                                                    Actual = (_actual == null ? 0 : _actual.Actual),
                                                    BaseLine = 0.0
                                                }).ToList();
                    }
                    else
                    {
                        BudgetByCategoryList = (from planned in PMMPlanBudgetTable.AsEnumerable()
                                                select new BudgetByCategory
                                                {
                                                    TicketId = planned.Field<string>(DatabaseObjects.Columns.TicketId),
                                                    BudgetSubCategory = planned.Field<string>(DatabaseObjects.Columns.BudgetCategoryLookup),
                                                    BudgetItem = planned.Field<string>(DatabaseObjects.Columns.BudgetItem),
                                                    Planned = planned.Field<double>(DatabaseObjects.Columns.BudgetAmount),
                                                    Actual = 0.0,
                                                    BaseLine = 0.0
                                                }).ToList();
                    }
                }
            }
            //Get all the BaseLine budget.
            DataTable PMMBudgetHistoryTable = null;
            string queryBLDetails = string.Empty;
            queryBLDetails = expression;//string.Format("<Where>{0}</Where>", expression);
            DataTable dtPMMBaseLine = GetTableDataManager.GetTableData(DatabaseObjects.Tables.BaseLineDetails, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            if (dtPMMBaseLine.Rows.Count > 0)
            {
                DataRow[] spItemColl = dtPMMBaseLine.Select(queryBLDetails);
                DataTable dt = null;
                if (spItemColl != null && spItemColl.Count() > 0)
                {
                    dt = spItemColl.CopyToDataTable();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    //dt.DefaultView.Sort = string.Format("{0} ASC, {1} ASC", DatabaseObjects.Columns.TicketPMMIdLookup, DatabaseObjects.Columns.BaselineNum);
                    if (!dt.Columns.Contains(DatabaseObjects.Columns.BaselineNum))
                        dt.Columns.Add(DatabaseObjects.Columns.BaselineNum, typeof(double));
                    dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.BaselineNum);
                    if (!dt.Columns.Contains(DatabaseObjects.Columns.TicketPMMIdLookup))
                        dt.Columns.Add(DatabaseObjects.Columns.TicketPMMIdLookup);
                    DataTable dtticket = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.TicketPMMIdLookup);
                    if (sourcetable != null)
                    {
                        foreach (DataRow dr in sourcetable.Rows)
                        {
                            string ticket = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);
                            int pmmid = Convert.ToInt32(dr[DatabaseObjects.Columns.ID]);
                            DataRow _dr = dt.AsEnumerable().Where(x=>x[DatabaseObjects.Columns.BaselineNum]!=DBNull.Value).OrderBy(x => x.Field<string>(DatabaseObjects.Columns.TicketPMMIdLookup))
                                                           .ThenBy(x =>x.Field<double>(DatabaseObjects.Columns.BaselineNum))
                                                           .FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketPMMIdLookup) == ticket);

                            int baselineNum = 0;
                            if (_dr != null && _dr[DatabaseObjects.Columns.BaselineNum] != null && Convert.ToInt32(_dr[DatabaseObjects.Columns.BaselineNum]) > 0)
                            {
                                baselineNum = Convert.ToInt32(_dr[DatabaseObjects.Columns.BaselineNum]);
                            }

                            List<string> qExpression = new List<string>();
                            qExpression.Add(string.Format("{0}={1}",
                                                            DatabaseObjects.Columns.TicketPMMIdLookup, pmmid));
                            qExpression.Add(string.Format("{0}={1}",
                                                            DatabaseObjects.Columns.BaselineId, baselineNum));

                            string queryForBaseLine = string.Empty;
                            if (qExpression.Count > 0)
                            {
                                queryForBaseLine = "(" + string.Join(" And ", qExpression) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(qExpression, qExpression.Count - 1, true));
                            }

                            if (PMMBaseLineList!=null && PMMBaseLineList.Rows.Count > 0)
                            {
                                DataRow[] drBaseLine = PMMBaseLineList.Select(queryForBaseLine);

                                if (drBaseLine != null && drBaseLine.Length > 0)
                                    PMMBudgetHistoryTable = PMMBaseLineList.Select(queryForBaseLine).CopyToDataTable();

                                // Import all the Planed budget in ProjectBudgetTable.

                                if (PMMBudgetHistoryTable != null)
                                {
                                    foreach (DataRow drPMMBudgetPlannedRow in PMMBudgetHistoryTable.Rows)
                                    {
                                        BudgetByCategoryList.Add(new BudgetByCategory
                                        {
                                            TicketId = Convert.ToString(drPMMBudgetPlannedRow[DatabaseObjects.Columns.TicketPMMIdLookup]),
                                            BudgetSubCategory = Convert.ToString(drPMMBudgetPlannedRow[DatabaseObjects.Columns.BudgetCategoryLookup]),
                                            BudgetItem = Convert.ToString(drPMMBudgetPlannedRow[DatabaseObjects.Columns.BudgetItem]),
                                            Actual = 0.0,
                                            BaseLine = Convert.ToDouble(drPMMBudgetPlannedRow[DatabaseObjects.Columns.BudgetAmount]),
                                            Planned = 0.0
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Save the changes of ProjectBudgetTable
            foreach (BudgetByCategory item in BudgetByCategoryList)
            {
                BudgetCategoryViewManager budgetCategoryViewManager = new BudgetCategoryViewManager(context);
                BudgetCategory budgetCategory = budgetCategoryViewManager.Load(x => x.BudgetSubCategory.Equals(item.BudgetSubCategory)).FirstOrDefault();
                if (budgetCategory != null)
                {
                    // SPListItem spItem = spItemcoll[0];
                    item.BudgetCategory = budgetCategory.BudgetCategoryName;
                    item.CategoryGLCode = budgetCategory.BudgetAcronym;
                    item.SubCategoryGLCode = budgetCategory.BudgetCOA;
                }
            }

            var summaryList = (from item in BudgetByCategoryList
                               group item by item.TicketId into g
                               select new
                               {
                                   Ticket = g.Key,
                                   Category = (from cat in g
                                               group cat by cat.BudgetCategory into _bcat
                                               select new
                                               {
                                                   _BudgetCategory = _bcat.Key,
                                                   CategoryGLCode = _bcat.Select(x => x.CategoryGLCode).FirstOrDefault(),
                                                   SubCategory = (from subcat in _bcat
                                                                  group subcat by subcat.BudgetSubCategory into item
                                                                  select new
                                                                  {
                                                                      BudgetSubCategory = item.Key,
                                                                      SubCategoryGLCode = item.Select(x => x.SubCategoryGLCode).FirstOrDefault(),
                                                                      BudgetItem = (from m in item
                                                                                    group m by m.BudgetItem into p
                                                                                    select new
                                                                                    {

                                                                                        BudgetItem = p.Key,
                                                                                        PlannedAmt = p.Sum(x => x.Planned),
                                                                                        BaseLine = p.Sum(x => x.BaseLine),
                                                                                        ActualAmt = p.Sum(x => x.Actual)
                                                                                    })
                                                                  })
                                               })
                               }).ToList();


            projectBudgetSummarizeTable = CreateBudgetTable();

            foreach (var summary in summaryList)
            {
                foreach (var cat in summary.Category)
                {

                    foreach (var subcat in cat.SubCategory)
                    {

                        foreach (var item in subcat.BudgetItem)
                        {
                            DataRow dr = projectBudgetSummarizeTable.NewRow();
                            dr[DatabaseObjects.Columns.TicketId] = summary.Ticket;
                            dr[DatabaseObjects.Columns.BudgetCategory] = cat._BudgetCategory;
                            dr[DatabaseObjects.Columns.BudgetSubCategory] = subcat.BudgetSubCategory;
                            dr[DatabaseObjects.Columns.BudgetItem] = item.BudgetItem;
                            dr["CategoryGLCode"] = cat.CategoryGLCode;
                            dr["SubCategoryGLCode"] = subcat.SubCategoryGLCode;
                            dr["Planned"] = item.PlannedAmt;
                            dr["BaseLine"] = item.BaseLine;
                            dr["Actual"] = item.ActualAmt;
                            projectBudgetSummarizeTable.Rows.Add(dr);
                        }
                    }
                }
            }
            projectBudgetSummarizeTable.AcceptChanges();

            return projectBudgetSummarizeTable;
        }
        #endregion

        #region  Planned vs Actual monthly Report
        private DataTable GetPlannedDistribution(ApplicationContext context, int pmmId)
        {
            ModuleBudgetManager moduleBudgetManager = new ModuleBudgetManager(context);
            DataTable pmmBudget = moduleBudgetManager.GetDataTable(); //SPListHelper.GetSPList(DatabaseObjects.Lists.PMMBudget, oWeb);
            DataRow pmmProjectItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, pmmId))[0];

            string budgetQuery = string.Empty;
            budgetQuery = string.Format("{0}='{1}'",
                                                           DatabaseObjects.Columns.TicketId, Convert.ToString(pmmProjectItem[DatabaseObjects.Columns.TicketId]));

            // Generate format table
            DateTime ReportStartDate = UGITUtility.StringToDateTime(pmmProjectItem[DatabaseObjects.Columns.TicketActualStartDate]);
            DateTime ReportEndDate = UGITUtility.StringToDateTime(pmmProjectItem[DatabaseObjects.Columns.TicketActualCompletionDate]);

            DataTable budgetTempTable = new DataTable(UGITUtility.ObjectToString(pmmProjectItem[DatabaseObjects.Columns.TicketId]));
            //budgetTempTable.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            budgetTempTable.Columns.Add(DatabaseObjects.Columns.BudgetCategory, typeof(string));

            // loop through start to end date.
            DateTime tempStartDateTime = ReportStartDate;
            while (tempStartDateTime <= ReportEndDate)
            {
                budgetTempTable.Columns.Add(tempStartDateTime.ToString("MMM") + "'" + tempStartDateTime.ToString("yy"), typeof(double));
                tempStartDateTime = tempStartDateTime.AddMonths(1);
            }
            budgetTempTable.Columns.Add("Total", typeof(double));

            DataRow[] budgetCollection = pmmBudget.Select(budgetQuery);

            if (budgetCollection.Count() > 0)
            {
                // Sort the table on subcategory in ascending order.
                DataTable pmmBudgetTable = budgetCollection.CopyToDataTable();
                pmmBudgetTable.DefaultView.Sort = DatabaseObjects.Columns.BudgetCategoryLookup + " ASC";
                pmmBudgetTable = pmmBudgetTable.DefaultView.ToTable();

                //string oldCategory = string.Empty;
                string newCategory = string.Empty;

                foreach (DataRow budgetRow in pmmBudgetTable.Rows)
                {
                    newCategory = UGITUtility.ObjectToString(budgetRow[DatabaseObjects.Columns.BudgetCategoryLookup]);
                    string ticketId = UGITUtility.ObjectToString(budgetRow[DatabaseObjects.Columns.TicketId]);
                    DateTime startDate = UGITUtility.StringToDateTime( budgetRow[DatabaseObjects.Columns.AllocationStartDate]);
                    DateTime endDate = UGITUtility.StringToDateTime( budgetRow[DatabaseObjects.Columns.AllocationEndDate]);
                    double totalAmount = UGITUtility.StringToDouble(budgetRow[DatabaseObjects.Columns.BudgetAmount]);

                    // Distribute the amount within specified dates and get the result in month and amount format.
                    Dictionary<DateTime, double> budgetDistributions = uHelper.DistributeAmount(context, startDate, endDate, totalAmount);
                    // Update with old distribution
                    foreach (DateTime key in budgetDistributions.Keys)
                    {
                        string colName = key.ToString("MMM") + "'" + key.ToString("yy");

                        if (budgetTempTable.Columns.Contains(colName))
                        {
                            double keyVal = budgetDistributions[key];
                            DataRow oldItem = null;

                            oldItem = budgetTempTable.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategory) == newCategory);
                            if (oldItem != null)
                            {
                                if (!DBNull.Value.Equals(oldItem[colName]))
                                    oldItem[colName] = Convert.ToDouble(oldItem[colName]) + keyVal;
                                else
                                    oldItem[colName] = keyVal;

                                if (!DBNull.Value.Equals(oldItem["Total"]))
                                    oldItem["Total"] = Convert.ToDouble(oldItem["Total"]) + keyVal;
                                else
                                    oldItem["Total"] = keyVal;

                                budgetTempTable.AcceptChanges();
                            }
                            else
                            {
                                DataRow newCategoryRow = GetBudgetRow(budgetTempTable, ReportStartDate, ReportEndDate);
                                //newCategoryRow[DatabaseObjects.Columns.TicketId] = ticketId;
                                newCategoryRow[DatabaseObjects.Columns.BudgetCategory] = newCategory;
                                newCategoryRow[colName] = keyVal;
                                newCategoryRow["Total"] = keyVal;
                                budgetTempTable.Rows.Add(newCategoryRow);
                            }
                        }
                    }
                }
            }
            return budgetTempTable;
        }

        private DataTable GetActualsDistribution(ApplicationContext context, int pmmid)
        {
            //SPList pmmBudget = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMBudget);
            DataRow pmmProjectItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, pmmid))[0];
            string expression = string.Format("{0}='{1}'",
                                                           DatabaseObjects.Columns.TicketId, Convert.ToString(pmmProjectItem[DatabaseObjects.Columns.TicketId]));

            string actualQuery = string.Empty; //new SPQuery();
            actualQuery = expression;

            // Generate format table
            DateTime ReportStartDate = UGITUtility.StringToDateTime(pmmProjectItem[DatabaseObjects.Columns.TicketActualStartDate]);
            DateTime ReportEndDate = UGITUtility.StringToDateTime(pmmProjectItem[DatabaseObjects.Columns.TicketActualCompletionDate]);

            DataTable actualTempTable = new DataTable(UGITUtility.ObjectToString(pmmProjectItem[DatabaseObjects.Columns.TicketId]));
            actualTempTable.Columns.Add(DatabaseObjects.Columns.BudgetCategory, typeof(string));

            // loop through start to end date.
            DateTime tempStartDateTime = ReportStartDate;
            while (tempStartDateTime <= ReportEndDate)
            {
                actualTempTable.Columns.Add(tempStartDateTime.ToString("MMM") + "'" + tempStartDateTime.ToString("yy"), typeof(double));
                tempStartDateTime = tempStartDateTime.AddMonths(1);
            }
            actualTempTable.Columns.Add("Total", typeof(double));
            ModuleBudgetManager moduleBudgetManager = new ModuleBudgetManager(context);
            BudgetActualsManager budgetActualsManager = new BudgetActualsManager(context);
            DataTable pmmActuals = budgetActualsManager.GetDataTable();
            DataTable pmmBudget = moduleBudgetManager.GetDataTable();

            DataRow[] actualCollection = pmmActuals.Select(actualQuery);

            DataRow actualTotalRow = GetBudgetRow(actualTempTable, ReportStartDate, ReportEndDate);
            actualTotalRow[DatabaseObjects.Columns.BudgetCategory] = "Total Actual";

            double actualGrandTotal = 0;

            if (actualCollection.Count() > 0)
            {
                foreach (DataRow actualItem in actualCollection)
                {
                    int budgetID = 0;
                    int.TryParse(UGITUtility.SplitString(actualItem[DatabaseObjects.Columns.ModuleBudgetLookup], Constants.Separator, 0), out budgetID);

                    if (budgetID > 0)
                    {
                        string budgetQuery = string.Empty;
                        List<string> WhereList = new List<string>();
                        if (!string.IsNullOrEmpty(expression))
                        {
                            WhereList.Add(expression);
                        }
                        WhereList.Add(string.Format("{0}={1}", DatabaseObjects.Columns.ID, budgetID));
                        if (WhereList.Count > 0)
                        {
                            budgetQuery = "(" + string.Join(" And ", WhereList) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(WhereList, WhereList.Count - 1, true));
                        }

                        DataRow[] budgetColl = pmmBudget.Select(budgetQuery);

                        string newCategory = string.Empty;

                        if (budgetColl.Count() > 0)
                        {
                            DataTable budgetTable = budgetColl.CopyToDataTable();
                            newCategory = UGITUtility.ObjectToString(budgetTable.Rows[0][DatabaseObjects.Columns.BudgetCategoryLookup]);
                            //string ticketId = Convert.ToString(budgetTable.Rows[0][DatabaseObjects.Columns.TicketPMMIdLookup]);

                            DateTime startDate = UGITUtility.StringToDateTime( actualItem[DatabaseObjects.Columns.AllocationStartDate]);
                            DateTime endDate = UGITUtility.StringToDateTime( actualItem[DatabaseObjects.Columns.AllocationEndDate]);
                            double totalAmount = UGITUtility.StringToDouble(actualItem[DatabaseObjects.Columns.BudgetAmount]);

                            // Distribute the amount within specified dates and get the result in month and amount format.
                            Dictionary<DateTime, double> oldDistributions = uHelper.DistributeAmount(context, startDate, endDate, totalAmount);

                            // Update with old distribution
                            foreach (DateTime key in oldDistributions.Keys)
                            {
                                string colName = key.ToString("MMM") + "'" + key.ToString("yy");

                                if (actualTempTable.Columns.Contains(colName))
                                {
                                    double keyVal = oldDistributions[key];
                                    DataRow oldItem = null;

                                    oldItem = actualTempTable.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.BudgetCategory) == newCategory);
                                    if (oldItem != null)
                                    {
                                        if (!DBNull.Value.Equals(oldItem[colName]))
                                            oldItem[colName] = UGITUtility.StringToDouble(oldItem[colName]) + keyVal;
                                        else
                                            oldItem[colName] = keyVal;

                                        if (!DBNull.Value.Equals(oldItem["Total"]))
                                            oldItem["Total"] = UGITUtility.StringToDouble(oldItem["Total"]) + keyVal;
                                        else
                                            oldItem["Total"] = keyVal;

                                        actualTempTable.AcceptChanges();
                                    }
                                    else
                                    {
                                        DataRow newCategoryRow = GetBudgetRow(actualTempTable, ReportStartDate, ReportEndDate);
                                        //newCategoryRow[DatabaseObjects.Columns.TicketId] = ticketId;
                                        newCategoryRow[DatabaseObjects.Columns.BudgetCategory] = newCategory;
                                        newCategoryRow[colName] = keyVal;
                                        newCategoryRow["Total"] = keyVal;
                                        actualTempTable.Rows.Add(newCategoryRow);
                                    }

                                    actualGrandTotal += keyVal;
                                }
                            }
                        }
                    }
                }
            }

            //actualTotalRow["Total"] = actualGrandTotal;
            //actualTempTable.Rows.Add(actualTotalRow);
            actualTempTable.AcceptChanges();

            return actualTempTable;
        }

        private DataRow GetBudgetRow(DataTable budgetTempTable, DateTime startDate, DateTime endDate)
        {
            // Generate format table
            DateTime tempDate = startDate;

            DataRow defaultRow = budgetTempTable.NewRow();

            while (tempDate <= endDate)
            {
                string colName = tempDate.ToString("MMM") + "'" + tempDate.ToString("yy");
                defaultRow[colName] = "0";
                tempDate = tempDate.AddMonths(1);
            }
            defaultRow["Total"] = 0;
            return defaultRow;
        }
        #endregion

        #region Resource Allocation Budget Chart

        /// <summary>
        /// Gets the source data.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>DataTable.</returns>
        private DataTable GetSourceData(ApplicationContext context, string expression)
        {
            ModuleMonthlyBudgetManager moduleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(context);
            // Load PMM Monthly Budget List.
            DataTable PMMMonthlyBudgetList = moduleMonthlyBudgetManager.GetDataTable(); //SPListHelper.GetSPList(DatabaseObjects.Lists.PMMMonthlyBudget, oWeb);
            string ticketId = string.Empty;

            // Find the resource data from pmm monthly list.
            string query = string.Empty;
            List<string> queryList = new List<string>();
            if (!string.IsNullOrEmpty(expression))
            {
                queryList.Add(expression);
            }
            queryList.Add(string.Format("{0}<>'{1}'", DatabaseObjects.Columns.BudgetType, "0"));
            if (queryList.Count > 0)
            {
                query = "(" + string.Join(" And ", queryList) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(queryList, queryList.Count - 1, true));
            }

            DataRow[] items = PMMMonthlyBudgetList.Select(query);

            DataTable projectPlanTable = null;

            if (items != null && items.Count() >= 1)
            {
                ticketId = UGITUtility.SplitString(items[0][DatabaseObjects.Columns.TicketId].ToString(), Constants.Separator, 1);

                if (PMMMonthlyBudgetList != null && ticketId != string.Empty)
                {
                    DataTable PMMMonthlyBudgetTable = items.CopyToDataTable();

                    // Set the Start date & end date of the report
                    DataView dv = new DataView(PMMMonthlyBudgetTable);
                    dv.Sort = DatabaseObjects.Columns.AllocationStartDate;

                    var ReportStartDate = Convert.ToDateTime(dv.ToTable().Rows[0][DatabaseObjects.Columns.AllocationStartDate].ToString());
                    var ReportEndDate = Convert.ToDateTime(dv.ToTable().Rows[dv.ToTable().Rows.Count - 1][DatabaseObjects.Columns.AllocationStartDate].ToString());

                    // Get the data group by the "Month" and then by "Category".
                    if (PMMMonthlyBudgetTable != null)
                    {
                        var summaryList =
                                   (from row in PMMMonthlyBudgetTable.AsEnumerable()
                                    where row.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId.ToString() &&
                                          row.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate) >= ReportStartDate &&
                                          row.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate) <= ReportEndDate
                                    group row by row.Field<string>(DatabaseObjects.Columns.TicketId) into ticket
                                    select new
                                    {
                                        Ticketid = ticket.Key,
                                        MonthDetails = (from t in ticket
                                                        group t by t.Field<DateTime>(DatabaseObjects.Columns.AllocationStartDate).ToString("MMM-yy") into g
                                                        select new
                                                        {
                                                            month = g.Key,
                                                            categories = (from m in g
                                                                          group m by m.Field<string>(DatabaseObjects.Columns.BudgetType.ToUpper()) into p
                                                                          select new
                                                                          {
                                                                              category = p.Key,
                                                                              categorySum = p.Sum(x => x.Field<double>(DatabaseObjects.Columns.BudgetAmount))
                                                                          }
                                                            ).ToList()

                                                        }).ToList()
                                    }).ToList();

                        projectPlanTable = CreateReportTable();

                        foreach (var obj in summaryList)
                        {
                            foreach (var obj1 in obj.MonthDetails)
                            {
                                foreach (var obj2 in obj1.categories)
                                {
                                    if (obj2.category != null)
                                    {
                                        DataRow dr = projectPlanTable.NewRow();
                                        dr["TicketId"] = obj.Ticketid;
                                        dr["Month"] = Convert.ToDateTime("15-" + obj1.month);
                                        if (obj2.category.ToUpper() == "1")
                                            dr["Category"] = "Staff";
                                        else if (obj2.category.ToUpper() == "2")
                                            dr["Category"] = "On-Site Consultants";
                                        else if (obj2.category.ToUpper() == "3")
                                            dr["Category"] = "Off-Site Consultants";

                                        dr["CategorySum"] = obj2.categorySum;
                                        projectPlanTable.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return projectPlanTable;
        }

        private DataTable CreateReportTable()
        {
            DataTable projectPlan = new DataTable();
            projectPlan.Columns.Add("TicketId", typeof(string));
            projectPlan.Columns.Add("Month", typeof(DateTime));
            projectPlan.Columns.Add("Category", typeof(string));
            projectPlan.Columns.Add("CategorySum", typeof(double));
            return projectPlan;
        }

        #endregion
        private void UpdateColumnContent(DataRow row, string columnName)
        {
            string value = Convert.ToString(row[columnName]);
            if (!string.IsNullOrEmpty(value))
                row[columnName] = UGITUtility.RemoveIDsFromLookupString(value);
        }
        public TSKProjectReportEntity GetTSKProjectsEntity(ApplicationContext _context, TSKProjectReportEntity prEntity, int[] tskIds)
        {
            UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
            // Get company logo to show on report header
            System.Drawing.Image img = null;
            context = _context;
            // Get custom company logo if configured
            string companyLogo = context.ConfigManager.GetValue(ConfigConstants.CompanyLogo);
            //if (!string.IsNullOrEmpty(companyLogo))
            //{
            //    //if (!companyLogo.Contains(oWeb.Url))
            //    //{
            //    //    companyLogo = oWeb.Url + companyLogo;
            //    //}
            //    prEntity.CompanyLogo = companyLogo;
            //    string fileName = companyLogo.Substring(companyLogo.LastIndexOf("/"));
            //    string filePath = uHelper.GetUploadFolderPath();
            //    try
            //    {
            //        img = Bitmap.FromFile(filePath + fileName);
            //    }
            //    catch (Exception ex)
            //    {
            //        ULog.WriteException(ex, "Cannot load Company Logo file");
            //        img = null;
            //    }
            //}

            //// If custom logo not configured OR cannot load custom logo, then default to standard uGovernIT logo
            //if (img == null)
            //{
            //    companyLogo = "/content/images/uGovernIT_Logo.png";
            //    prEntity.CompanyLogo = companyLogo;
            //    string fileName = companyLogo.Substring(companyLogo.LastIndexOf("/"));
            //    string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid(); //Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath("TEMPLATE\\IMAGES\\uGovernIT\\", SPUtility.CompatibilityLevel15);
            //    try
            //    {
            //        img = Bitmap.FromFile(filePath + fileName);
            //    }
            //    catch (Exception ex)
            //    {
            //        ULog.WriteException(ex, "Cannot load Company Logo file");
            //        img = null;
            //    }
            //}

            //if (img != null)
            //{
            //    prEntity.LogoHeight = img.Height;
            //    prEntity.LogoWidth = img.Width;
            //}

            //List<string> tskqueryExpressions = new List<string>();
            //List<string> commonexpressions = new List<string>();

            DataTable TSKProjects = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, $"TenantID='{_context.TenantID}'"); //SPListHelper.GetSPList(DatabaseObjects.Lists.TSKProjects, oWeb).Items.GetDataTable();
            if (TSKProjects != null && TSKProjects.Rows.Count > 0)
            {

                var result = from t in TSKProjects.AsEnumerable()
                             where tskIds.Contains(Convert.ToInt32(t.Field<long>(DatabaseObjects.Columns.ID)))
                             select t;
                if (result != null)
                {
                    TSKProjects = result.CopyToDataTable();
                }
                DataTable dt = TSKProjects.Copy();
                foreach (DataRow row in TSKProjects.Rows)
                {
                    int tskid = Convert.ToInt32(row[DatabaseObjects.Columns.ID]);
                    string beneficiary = Convert.ToString(row[DatabaseObjects.Columns.TicketBeneficiaries]);
                    beneficiary = uHelper.FormatDepartment(context, beneficiary, true);

                    dt.AsEnumerable().Where(dr => Convert.ToInt32(dr.Field<long>(DatabaseObjects.Columns.ID)) == tskid).ToList()
                   .ForEach(r =>
                   {
                       r.SetField(DatabaseObjects.Columns.TicketBeneficiaries, beneficiary);
                   });
                }

                DataTable dtask = dt.Clone();
                dtask.Columns.Add("CompanyLogo", typeof(string));
                foreach (DataRow dr in dt.Rows)
                {
                    List<object> listobject = dr.ItemArray.ToList();
                    listobject.Add(companyLogo);
                    dtask.Rows.Add(listobject.ToArray());

                }

                prEntity.Projects = dtask;
            }

            #region All Tasks (ShowAllTask || ShowDeliverable || ShowReceivable)
            if (prEntity.ShowAllTask || prEntity.ShowDeliverable || prEntity.ShowReceivable)
            {
                DataTable organizedTasks = null;
                if (prEntity.Projects.Rows.Count > 0)
                {
                    foreach (DataRow row in prEntity.Projects.Rows)
                    {
                        DataTable tasks = uGITTaskManager.GetAllTasksByProjectID("TSK", Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                        if (tasks != null && tasks.Rows.Count > 0)
                        {
                            if (organizedTasks != null && organizedTasks.Rows.Count > 0)
                            {
                                organizedTasks.Merge(tasks);
                            }
                            else
                            {
                                organizedTasks = tasks.Copy();
                            }
                        }
                    }
                }
                else
                {
                    organizedTasks = GetAllTasks(_context, tskIds, TicketStatus.All, "TSK");
                }

                //  TaskCache.UpdatePredecessorsByOrder(organizedTasks);
                prEntity.Tasks = organizedTasks;
            }
            #endregion

            #region Summary Task (ShowMilestone || ShowSummaryGanttChart)
            if (prEntity.ShowMilestone || prEntity.ShowSummaryGanttChart)
            {
                DataTable summaryTasks = null;
                if (prEntity.Projects.Rows.Count > 0)
                {
                    foreach (DataRow row in prEntity.Projects.Rows)
                    {
                        DataTable tasks = uGITTaskManager.GetSummaryTasksByProjectID("TSK", Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                        if (tasks != null && tasks.Rows.Count > 0)
                        {
                            if (summaryTasks != null && summaryTasks.Rows.Count > 0)
                            {
                                summaryTasks.Merge(tasks);
                            }
                            else
                            {
                                summaryTasks = tasks;
                            }
                        }
                    }
                }
                else
                {
                    summaryTasks = GetSummaryTasks(context, tskIds, TicketStatus.All, "TSK");
                }

                //  TaskCache.UpdatePredecessorsByOrder(summaryTasks);
                prEntity.SummaryTasks = summaryTasks;
            }
            #endregion

            return prEntity;
        }

        /// <summary>
        /// Gets all tasks by project identifier.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>DataTable.</returns>
        private DataTable GetAllTasks(ApplicationContext context, int[] Ids, TicketStatus ticketStatus, string module)
        {
            DataTable dt = new DataTable();
            TaskSummary_Scheduler taskSummary_Scheduler = new TaskSummary_Scheduler();
            dt = taskSummary_Scheduler.GetProjectsTasksTable(context, Ids, ticketStatus, module);
            return dt;
        }

        /// <summary>
        /// Gets the summary tasks.
        /// </summary>
        /// <param name="Ids">The ids.</param>
        /// <param name="ticketStatus">The ticket status.</param>
        /// <param name="module">The module.</param>
        /// <returns>DataTable.</returns>
        private DataTable GetSummaryTasks(ApplicationContext context, int[] Ids, TicketStatus ticketStatus, string module)
        {
            DataTable dt = new DataTable();
            TaskSummary_Scheduler taskSummary_Scheduler = new TaskSummary_Scheduler();
            dt = taskSummary_Scheduler.GetProjectsTasksTable(context, Ids, ticketStatus, module);
            dt = dt.Select(string.Format("ParentTask=0 or {0} in ('milestone','deliverable','receivable')",
                                        DatabaseObjects.Columns.TaskBehaviour)).CopyToDataTable();
            return dt;
        }

        public void GetDecisionLog(ApplicationContext context, TSKProjectReportEntity prEntity, string queryexpression)
        {
            DecisionLogManager decisionLogManager = new DecisionLogManager(context);
            //prEntity.Projects
            DataTable decisionLogList = decisionLogManager.GetDataTable();
            if (decisionLogList == null)
                return;
            DataTable decisionLogTable = null;
            string decisionLogQuery = string.Empty;
            if (prEntity.Projects == null || prEntity.Projects.Rows.Count == 0)
                return;

            List<string> lstPmmTicketIds = new List<string>();
            lstPmmTicketIds = prEntity.Projects.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).Distinct().ToList();
            List<string> issuesExpressions = new List<string>();
            List<string> commonQueryExpression = new List<string>();
            lstPmmTicketIds.ForEach(x => commonQueryExpression.Add(string.Format("{0}='{1}'",
                                                           DatabaseObjects.Columns.TicketId, x)));

            if (commonQueryExpression.Count > 0)
            {
                queryexpression = "(" + string.Join(" OR ", commonQueryExpression) + ")"; //uHelper.GenerateWhereQueryWithAndOr(commonQueryExpression, commonQueryExpression.Count - 1, false);
                issuesExpressions.Add(queryexpression);
            }

            issuesExpressions.Add(string.Format("{0}<>'{1}'",
                                                 DatabaseObjects.Columns.Deleted, "True"));
            if (issuesExpressions.Count > 0)
            {
                decisionLogQuery = "(" + string.Join(" And ", issuesExpressions) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(issuesExpressions, issuesExpressions.Count - 1, true));
            }

            DataRow[] itemColl = decisionLogList.Select(decisionLogQuery);
            if (itemColl != null && itemColl.Count() > 0)
            {
                DataView dataView = itemColl.CopyToDataTable().DefaultView;
                dataView.Sort = string.Format("{0} DESC", DatabaseObjects.Columns.ReleaseDate);
                decisionLogTable = dataView.ToTable();
            }
            prEntity.DecisionLog = decisionLogTable;
            if (prEntity.DecisionLog != null && prEntity.DecisionLog.Rows.Count > 0 && uHelper.IfColumnExists(DatabaseObjects.Columns.UGITAssignedTo, prEntity.DecisionLog))
            {
                prEntity.DecisionLog.AsEnumerable().ToList().ForEach(x => UpdateColumnContent(x, DatabaseObjects.Columns.UGITAssignedTo));
            }
        }

    }

    public class BudgetByCategory
    {
        public string TicketId { get; set; }
        public string BudgetCategory { get; set; }
        public string BudgetSubCategory { get; set; }
        public string CategoryGLCode { get; set; }
        public string SubCategoryGLCode { get; set; }
        public string BudgetItem { get; set; }
        public double Planned { get; set; }
        public double Actual { get; set; }
        public double BaseLine { get; set; }

    }
}
