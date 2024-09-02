using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager.Reports;
using uGovernIT.Manager.Report.Entities;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;
namespace uGovernIT.Report.DxReport
{

    public class OnePagerReportHelper
    {
        #region Global Variable
        public string TicketId { get; set; }
        public string documnetPickerUrl { get; set; }
        public string DocName { get; set; }
        public string FolderGuid { get; set; }
        public string stageName { get; set; }
        public string SelectFolder { get; set; }
        public string PathValue { get; set; }
        ProjectCompactReportEntity entity;
        DataTable pmmList;
        DataTable pmmtaskList;
        DataTable pmmMonitorState;
        public DataTable sourcetable = new DataTable();
        public int[] PMMIds { get; set; }
        #endregion
        ApplicationContext context;
        UGITTaskManager uGITTaskManager;
        ProjectMonitorStateManager monitorStateManager;
        TicketManager ticketManager;
        UserProfileManager userProfileManager;
        ModuleViewManager ModuleManager;
        ConfigurationVariableManager _configurationVariableManager;
        public OnePagerReportHelper(ApplicationContext applicationContext)
        {
            context = applicationContext;
            _configurationVariableManager = new ConfigurationVariableManager(context);
            uGITTaskManager = new UGITTaskManager(context);
            monitorStateManager = new ProjectMonitorStateManager(context);
            ticketManager = new TicketManager(context);
            pmmList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            pmmtaskList = uGITTaskManager.GetDataTable();
            // pmmComment = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMComments, spWeb);
            //  pmmRisks = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMRisks, spWeb);
            // pmmIssue = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMIssues, spWeb);
            pmmMonitorState = monitorStateManager.GetDataTable();
            ModuleManager = new ModuleViewManager(context);
        }

        //public OnePagerReportHelper(SPWeb web)
        //{
        //    spWeb = web;
        //}

        public ProjectCompactReportEntity GetOnePagerReportEntity()
        {
            entity = new ProjectCompactReportEntity();
            GetProjectDetails();
            return entity;
        }

        /// <summary>
        /// Project data 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void GetProjectDetails()
        {
            #region Project block

            ///Expression for PMM and Other related Table.
            List<string> pmmqueryExpressions = new List<string>();
            List<string> commonexpressions = new List<string>();

            string qexpression = string.Empty;
            string queryexpression = string.Empty;
            string _query = string.Empty;

            if (PMMIds != null && PMMIds.Length > 0)
            {
                foreach (int PMMid in PMMIds)
                {
                    if (PMMid > 0)
                    {
                        UGITModule module = ModuleManager.LoadByName(ModuleNames.PMM);
                        DataRow dataRow = ticketManager.GetByID(module, PMMid);
                        pmmqueryExpressions.Add(string.Format("{0}={1}",
                                                            DatabaseObjects.Columns.ID, PMMid));
                        commonexpressions.Add(string.Format("{0}='{1}'",
                                                           DatabaseObjects.Columns.TicketId, dataRow[DatabaseObjects.Columns.TicketId]));
                    }
                }
                if (pmmqueryExpressions.Count > 0)
                {
                    qexpression = "(" + string.Join(" OR ", pmmqueryExpressions) + ")"; //uHelper.GenerateWhereQueryWithAndOr(pmmqueryExpressions, pmmqueryExpressions.Count - 1, false);
                    queryexpression = "(" + string.Join(" OR ", commonexpressions) + ")"; //uHelper.GenerateWhereQueryWithAndOr(commonexpressions, commonexpressions.Count - 1, false);
                }
            }

            _query = qexpression; //string.Format("<Where>{0}</Where>", qexpression);
            DataRow[] spListItemColl = pmmList.Select(_query);

            if (spListItemColl != null && spListItemColl.Count() > 0)
            {
                sourcetable = spListItemColl.CopyToDataTable();
                foreach (DataRow spItem in spListItemColl)
                {
                    int pmmid = Convert.ToInt32(spItem[DatabaseObjects.Columns.ID]);
                    string beneficiary = Convert.ToString(spItem[DatabaseObjects.Columns.TicketBeneficiaries]);
                    beneficiary = uHelper.FormatDepartment(context, beneficiary, true);
                    string projectSummary = string.Empty;

                    List<HistoryEntry> historyList = uHelper.GetHistory(spItem, DatabaseObjects.Columns.ProjectSummaryNote);
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

                DataTable convertedSourcetable = uHelper.ConvertTableLookupValues(context, sourcetable);
                DataTable dtProject = convertedSourcetable.Copy();
                dtProject.Columns.Add("WeekEnding", typeof(string));

                string weekEndingDate = UGITUtility.GetDateStringInFormat(DateTime.Now, false);
                dtProject.AsEnumerable().ToList().ForEach(row => row.SetField("WeekEnding", weekEndingDate));

                //Sort project by title
                DataView view = dtProject.DefaultView;
                view.Sort = string.Format("{0} asc", DatabaseObjects.Columns.Title);
                entity.ProjectDetails = view.ToTable(true);
            }

            #endregion

            #region All Tasks
            DataTable organizedTasks = null;

            if (entity.ProjectDetails.Rows.Count>0)
            {
                foreach (DataRow row in entity.ProjectDetails.Rows)
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
                organizedTasks = GetAllTasks(PMMIds, TicketStatus.All, "PMM");
            }

            uGITTaskManager.UpdatePredecessorsByOrder(organizedTasks);
            entity.Tasks = organizedTasks;
            #endregion

            #region Mile Stone

            DataTable summaryTasks = null;
            if (entity.ProjectDetails.Rows.Count >0)
            {
                foreach (DataRow row in entity.ProjectDetails.Rows)
                {
                    DataTable tasks = uGITTaskManager.GetSummaryTasksByProjectID("PMM", Convert.ToString(row[DatabaseObjects.Columns.TicketId]));
                    string query = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TaskBehaviour, "Milestone");
                    DataView dv = new DataView(tasks);

                    dv.RowFilter = query; // query
                    tasks = dv.ToTable();
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
                summaryTasks = GetSummaryTasks(PMMIds, TicketStatus.All, "PMM");
            }

            if (summaryTasks != null && summaryTasks.Rows.Count > 0)
            {
                if (_configurationVariableManager.GetValueAsBool(ConfigConstants.OnePagerMilestonesOnly))
                {
                    entity.ShowAllMilestone = true;
                    if (summaryTasks.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour))
                    {
                        DataRow[] summaryTaskRows = summaryTasks.Select(string.Format("{0} = 'milestone'", DatabaseObjects.Columns.TaskBehaviour));

                        // entity.MileStone = null;
                        if (summaryTaskRows.Length > 0)
                        {
                            DataTable sData = summaryTaskRows.CopyToDataTable();
                            // TaskCache.UpdatePredecessorsByOrder(summaryTaskRows);
                            summaryTasks = sData;
                        }
                        else
                        {
                            summaryTasks = null;
                        }

                    }
                }
                else
                {
                    entity.ShowAllMilestone = false;
                    //Those milestone are not part of summary which root task is completed.
                    //Only Show root task for it.
                    DataRow[] summaryTaskRows = summaryTasks.Select(string.Format("{0} = 'milestone'", DatabaseObjects.Columns.TaskBehaviour));
                    List<int> removedTasks = new List<int>();
                    foreach (DataRow sRow in summaryTaskRows)
                    {
                        if (UGITUtility.StringToInt(sRow[DatabaseObjects.Columns.ParentTask]) > 0)
                        {
                            int parentID = UGITUtility.StringToInt(sRow[DatabaseObjects.Columns.ParentTask]);
                            bool keepLooping = true;
                            do
                            {
                                keepLooping = false;
                                DataRow spRow = entity.Tasks.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.Id) == parentID);
                                if (spRow != null)
                                {
                                    parentID = UGITUtility.StringToInt(spRow[DatabaseObjects.Columns.ParentTask]);
                                    if (parentID > 0)
                                    {
                                        //keep looping to find out root task of milestone
                                        keepLooping = true;
                                    }
                                    else if (UGITUtility.StringToDouble(spRow[DatabaseObjects.Columns.PercentComplete]) >= 100)
                                    {
                                        removedTasks.Add(UGITUtility.StringToInt(sRow[DatabaseObjects.Columns.Id]));
                                    }
                                }
                            } while (keepLooping);
                        }
                    }

                    if (removedTasks.Count > 0)
                    {
                        summaryTaskRows = summaryTasks.AsEnumerable().Where(x => !removedTasks.Exists(y => y == x.Field<int>(DatabaseObjects.Columns.Id))).ToArray();
                        if (summaryTaskRows.Length > 0)
                            summaryTasks = summaryTaskRows.CopyToDataTable();
                        else
                            summaryTasks = new DataTable();
                    }
                }

            }

            if (summaryTasks != null && summaryTasks.Rows.Count > 0)
            {
                uGITTaskManager.UpdatePredecessorsByOrder(summaryTasks);
                entity.MileStone = summaryTasks;
            }

            #endregion

            #region Bind Accomplishments

            List<string> accqueryExpressions = new List<string>();
            string accompleshmentQuery = string.Empty;
            if (!string.IsNullOrEmpty(queryexpression))
            {
                accqueryExpressions.Add(queryexpression);
            }
            accqueryExpressions.Add(string.Format("{0}='{1}'",
                                                   DatabaseObjects.Columns.ProjectNoteType, "Accomplishments"));
            accqueryExpressions.Add(string.Format("{0}<>{1}",DatabaseObjects.Columns.Deleted, "True"));

            if (accqueryExpressions.Count > 0)
            {
                accompleshmentQuery = "(" + string.Join(" And ", accqueryExpressions) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(accqueryExpressions, accqueryExpressions.Count - 1, true));
            }

            DataRow[] accomplishments =  GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMComments, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(accompleshmentQuery);
            if (accomplishments != null && accomplishments.Length > 0)
            {
                //entity.AccomPlanned = accomplishments.CopyToDataTable();
                DataView dataView = accomplishments.CopyToDataTable().DefaultView;
                dataView.Sort = string.Format("{0} DESC", DatabaseObjects.Columns.AccomplishmentDate);
                entity.AccomPlanned = dataView.ToTable();
            }
                

            if (entity.AccomPlanned != null && entity.AccomPlanned.Rows.Count > 0)
                entity.AccomPlanned.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
            #endregion

            #region Bind Immediate Plans

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

            if (planqueryExpressions.Count() > 0)
                planQuery = "(" + string.Join(" And ", planqueryExpressions) + ")";
            //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(planqueryExpressions, planqueryExpressions.Count - 1, true));
            DataTable dtImmediateplans = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMComments, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            DataRow[] Immediateplans = null;
            if (dtImmediateplans != null && dtImmediateplans.Rows.Count > 0)
                Immediateplans = dtImmediateplans.Select(planQuery);
            
            
            if (Immediateplans != null && Immediateplans.Length > 0)
                entity.ImediatePlanned = Immediateplans.CopyToDataTable();
            if (entity.ImediatePlanned != null && entity.ImediatePlanned.Rows.Count > 0)
                entity.ImediatePlanned.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
            #endregion

            #region Bind Issues
            string issueQuery = string.Empty;
            List<string> issuesExpressions = new List<string>();
            if (!string.IsNullOrEmpty(queryexpression))
            {
                issuesExpressions.Add(queryexpression);
            }
            issuesExpressions.Add(string.Format("{0}<>'{1}'",DatabaseObjects.Columns.Deleted, "True"));
            if (issuesExpressions.Count() > 0)

                issueQuery = "(" + string.Join(" And ", issuesExpressions) + ")";
            //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(issuesExpressions, issuesExpressions.Count - 1, true));
            DataRow[] issues = null;
            if (pmmtaskList.Rows!=null && pmmtaskList.Rows.Count>0)

            issues = pmmtaskList.Select(issueQuery);
            if (issues != null && issues.Count() > 0)
            {
                issues = issues.Where(x =>!x.IsNull(DatabaseObjects.Columns.UGITSubTaskType) && x.Field<string>(DatabaseObjects.Columns.UGITSubTaskType).EqualsIgnoreCase("issue")).ToArray();
                if (issues != null && issues.Length > 0)
                    entity.PMMIssues =uHelper.ConvertTableLookupValues(context, issues.CopyToDataTable());
                else
                    entity.PMMIssues = null;
            }
                
            if (entity.PMMIssues != null && entity.PMMIssues.Rows.Count > 0)
                entity.PMMIssues.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));

            #endregion

            #region Bind Risks
            issueQuery = string.Empty;
            issuesExpressions = new List<string>();
            if (!string.IsNullOrEmpty(queryexpression))
            {
                issuesExpressions.Add(queryexpression);
            }
            issuesExpressions.Add(string.Format("{0}<>'{1}'",DatabaseObjects.Columns.Deleted, "True"));
            if (issuesExpressions.Count() > 0)

                issueQuery = "(" + string.Join(" And ", issuesExpressions) + ")";
            //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(issuesExpressions, issuesExpressions.Count - 1, true));
            DataRow[] risks = null;
            if (pmmtaskList!=null && pmmtaskList.Rows.Count>0)

            risks = pmmtaskList.Select(issueQuery);
            if (risks != null && risks.Count() > 0)
            {
                risks = risks.Where(x => !x.IsNull(DatabaseObjects.Columns.UGITSubTaskType) && x.Field<string>(DatabaseObjects.Columns.UGITSubTaskType).EqualsIgnoreCase("risk")).ToArray();
                if (risks != null && risks.Length > 0)
                    entity.PMMRisks = uHelper.ConvertTableLookupValues(context, risks.CopyToDataTable());
                else
                    entity.PMMRisks = null;
            }

            if (entity.PMMRisks != null && entity.PMMRisks.Rows.Count > 0)
            {
                entity.PMMRisks.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
                entity.PMMRisks.Rows.Cast<DataRow>().AsEnumerable().ToList().ForEach(x => x.SetField(DatabaseObjects.Columns.RiskProbability, GetCalculatedValue(x[DatabaseObjects.Columns.RiskProbability])));

            }

            #endregion

            #region monitor state
            ModuleMonitorOptionManager moduleMonitorOptionManager = new ModuleMonitorOptionManager(context);
            var projectMonitorState = pmmMonitorState; //SPListHelper.GetSPList(DatabaseObjects.Lists.ProjectMonitorState, spWeb);
            string queryPMMSpecificMonitors = string.Empty;
            queryPMMSpecificMonitors = queryexpression;
            var projectMonitorOptions = moduleMonitorOptionManager.Load(x=>x.TenantID==context.TenantID);
            DataRow[] monitors = pmmMonitorState.Select(queryPMMSpecificMonitors);
            ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(context);
            List<ModuleMonitor> moduleMonitors= moduleMonitorManager.Load(x => x.TenantID == context.TenantID); 
            DataTable monitorState = null;
            if (monitors!=null && monitors.Count()>0)
                 monitorState = monitors.CopyToDataTable();
            if (monitorState != null && monitorState.Rows.Count>0)
            {
                DataColumn dataColumn = new DataColumn(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup, typeof(string));
                DataColumn dataColumn1 = new DataColumn(DatabaseObjects.Columns.ModuleMonitorName, typeof(string));
                monitorState.Columns.Add(dataColumn);
                monitorState.Columns.Add(dataColumn1);
                foreach (DataRow dr in monitorState.Rows)
                {
                    ModuleMonitor moduleMonitor = moduleMonitors.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.ModuleMonitorNameLookup]));
                    ModuleMonitorOption moduleMonitorOption = projectMonitorOptions.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(dr[DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                    if (moduleMonitorOption != null)
                    {
                        dr[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = UGITUtility.GetImageUrlForReport(uHelper.GetHealthIndicatorImageUrl(Convert.ToString(moduleMonitorOption.ModuleMonitorOptionLEDClass)));
                        dr[DatabaseObjects.Columns.ModuleMonitorName] = moduleMonitor.MonitorName;
                    }
                }


                entity.ProjectMonitorHealth = monitorState;
            }
            #endregion

            #region Decision Log
            if (entity.ShowDecisionLog)
                GetDecisionLog(context, entity, queryexpression);
            #endregion
        }

        private double GetCalculatedValue(object oldPer)
        {
            double per = 0F;
            double.TryParse(Convert.ToString(oldPer), out per);
            if (per > 0)
                per = per / 100;
            return per;
        }

        /// <summary>
        /// Gets the summary tasks.
        /// </summary>
        /// <param name="Ids">The ids.</param>
        /// <param name="ticketStatus">The ticket status.</param>
        /// <param name="module">The module.</param>
        /// <returns>DataTable.</returns>
        private DataTable GetSummaryTasks(int[] Ids, TicketStatus ticketStatus, string module)
        {
            DataTable dt = new DataTable();
            DataRow[] dataRowCollection = null;
            dt = uGITTaskManager.GetProjectsTasksTable(context, Ids, ticketStatus, module);
            dataRowCollection = dt.Select(string.Format("ParentTask=0 or {0} in ('milestone','deliverable','receivable')",
                                        DatabaseObjects.Columns.TaskBehaviour));
            if (dataRowCollection != null && dataRowCollection.Count() >0)
                dt = dataRowCollection.CopyToDataTable();
            return dt;
        }

        private void UpdateAssignedTo(DataRow row, string columnName)
        {
            userProfileManager = new UserProfileManager(context);
            string[] userList = UGITUtility.SplitString(Convert.ToString(row[columnName]), ",", StringSplitOptions.RemoveEmptyEntries);
            if (userList.Length > 0)
            {
                List<string> users = new List<string>();
                foreach (string userName in userList)
                {
                    UserProfile user = userProfileManager.GetUserByUserName(userName);
                    if (user != null)
                        users.Add(user.Name);
                }

                if (users.Count > 0)
                    row[columnName] = String.Join("; ", users);
            }
        }

        //private DataRow UpdateAssignedTo(DataRow row, string columnName)
        //{
        //    userProfileManager = new UserProfileManager(context);
        //    string[] userList = UGITUtility.SplitString(Convert.ToString(row[columnName]), ",", StringSplitOptions.RemoveEmptyEntries);
        //    if (userList.Length > 0)
        //    {
        //        List<string> users = new List<string>();
        //        foreach (string userName in userList)
        //        {
        //            UserProfile user = userProfileManager.GetUserByUserName(userName);
        //            if (user != null)
        //                users.Add(user.Name);
        //        }

        //        if (users.Count > 0)
        //            row[columnName] = String.Join("; ", users);
        //    }
        //    return row;
        //}

        private void UpdateProjectDescription(DataRow row)
        {
            //row["ProjectDescription"] = item[DatabaseObjects.Columns.TicketDescription];
        }

        private void UpdateNoteContent(DataRow row)
        {
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.ProjectNote))
                row[DatabaseObjects.Columns.ProjectNote] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.ProjectNote]));

            if (row.Table.Columns.Contains(DatabaseObjects.Columns.MitigationPlan))
                row[DatabaseObjects.Columns.MitigationPlan] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.MitigationPlan]));

            if (row.Table.Columns.Contains(DatabaseObjects.Columns.ContingencyPlan))
                row[DatabaseObjects.Columns.ContingencyPlan] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.ContingencyPlan]));

            if (row.Table.Columns.Contains(DatabaseObjects.Columns.Body))
                row[DatabaseObjects.Columns.Body] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.Body]));

            if (row.Table.Columns.Contains(DatabaseObjects.Columns.UGITResolution))
                row[DatabaseObjects.Columns.UGITResolution] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.UGITResolution]));

            if (row.Table.Columns.Contains(DatabaseObjects.Columns.AssignedTo))
                row[DatabaseObjects.Columns.AssignedTo] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[DatabaseObjects.Columns.AssignedTo]));
        }

        /// <summary>
        /// Gets all tasks by project identifier.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>DataTable.</returns>
        private DataTable GetAllTasks(int[] Ids, TicketStatus ticketStatus, string module)
        {
            DataTable dt = new DataTable();

            dt = uGITTaskManager.GetProjectsTasksTable(context, Ids, ticketStatus, module);
            return dt;
        }
        public void GetDecisionLog(ApplicationContext context, ProjectCompactReportEntity prEntity, string queryexpression)
        {
            DecisionLogManager decisionLogManager = new DecisionLogManager(context);
            //prEntity.Projects
            DataTable decisionLogList = decisionLogManager.GetDataTable();
            if (decisionLogList == null)
                return;
            DataTable decisionLogTable = null;
            string decisionLogQuery = string.Empty;
            if (prEntity.ProjectDetails == null || prEntity.ProjectDetails.Rows.Count == 0)
                return;

            List<string> lstPmmTicketIds = new List<string>();
            lstPmmTicketIds = prEntity.ProjectDetails.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).Distinct().ToList();
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

            // decisionLogQuery.ViewFields = string.Format("<FieldRef Name='{0}'/>,<FieldRef Name='{1}'/>,<FieldRef Name='{2}'/>,<FieldRef Name='{3}'/>,<FieldRef Name='{4}'/>,<FieldRef Name='{5}'/>,<FieldRef Name='{6}'/>,<FieldRef Name='{7}'/><FieldRef Name='{8}'/>", DatabaseObjects.Columns.ReleaseDate, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.UGITAssignedTo, DatabaseObjects.Columns.DecisionMaker, DatabaseObjects.Columns.UGITDescription, DatabaseObjects.Columns.DecisionStatus, DatabaseObjects.Columns.Deleted, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketId);
            // decisionLogQuery.ViewFieldsOnly = true;

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
        private void UpdateColumnContent(DataRow row, string columnName)
        {
            string value = Convert.ToString(row[columnName]);
            if (!string.IsNullOrEmpty(value))
                row[columnName] = UGITUtility.RemoveIDsFromLookupString(value);
        }
    }
}
