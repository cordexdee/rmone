using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public class BusinessStrategyDashboard
    {
        #region Properties

        public int WorkingHours { get { return 8; } }
        public int WorkingDayInMonth { get { return 22; } }
        public int RiskLevelRedThreshold { get { return 100; } }//if risklevel is 100%
        public int RiskLevelYellowThreshold { get { return 90; } }//if risklevel is 100%
        public int AmountLeftYellowThreshold { get { return 10; } }
        public int AmountLeftRedThreshold { get { return 5; } }
        public int MonthsLeftYellowThreshold { get { return 10; } }
        public int MonthsLeftRedThreshold { get { return 5; } }
        public int IssuesYellowThreshold { get { return 10; } }
        public int IssuesRedThreshold { get { return 25; } }
        //flipview properties
        public int PercentageOfNNumberOfBusinessStrategy { get { return 2; } }
        public int PercentageOfNNumberOfInitiative { get { return 2; } }
        public int PercentageOfNNumberOfProjects { get { return 2; } }

        //Budget variance
        public int RedCostVarianceThreshold { get { return 10; } }
        public double YellowCostVarianceThreshold { get { return 5; } }

        #endregion

        #region Variable

        DataTable resultentTable;
        DataTable dtInitiative;
        DataTable dtIssues;
        DataTable dtBusinessStrategy;
        UpdateColumns updateColObj = new UpdateColumns();
        ApplicationContext _Context;
        BusinessStrategyManager BsStrategyManager;
        #endregion

        #region Methods
        public BusinessStrategyDashboard(ApplicationContext context)
        {
            _Context = context;
            BsStrategyManager = new BusinessStrategyManager(context);
            dtBusinessStrategy = GetAllBusinessStrategy();
            dtInitiative = GetIntiative();
            dtIssues = GetIssues();
        }

        public DataTable GetIntiative()
        {
            ProjectInitiativeViewManager projectInitiativeManager = new ProjectInitiativeViewManager(_Context);

            DataRow[] pmmInitiative = projectInitiativeManager.GetDataTable().Select();
            DataTable dtAllInit = new DataTable();
            dtAllInit.Columns.Add(DatabaseObjects.Columns.Title);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.ID);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.BusinessStrategyLookup);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.BusinessId);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.BusinessStrategyDescription);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.InitiativeDescription);

            if (pmmInitiative != null)
            {
                foreach (DataRow item in pmmInitiative)
                {
                    DataRow addRow = dtAllInit.NewRow();
                    dtAllInit.Rows.Add(addRow);
                    addRow[DatabaseObjects.Columns.Title] = item[DatabaseObjects.Columns.Title];
                    addRow[DatabaseObjects.Columns.ID] = item[DatabaseObjects.Columns.ID];
                    addRow[DatabaseObjects.Columns.InitiativeDescription] = item[DatabaseObjects.Columns.ProjectNote];
                    string value = Convert.ToString(item[DatabaseObjects.Columns.BusinessStrategyLookup]);

                    DataRow bsRow = dtBusinessStrategy.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ID) == value).FirstOrDefault();
                    if (bsRow != null)
                    {
                        addRow[DatabaseObjects.Columns.BusinessStrategyLookup] = value;
                        addRow[DatabaseObjects.Columns.BusinessId] = Convert.ToString(value);
                        addRow[DatabaseObjects.Columns.BusinessStrategyDescription] = Convert.ToString(bsRow[DatabaseObjects.Columns.TicketDescription]);
                    }

                }
                //List<string> mappedBsIds = dtAllInit.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.BusinessId)).Select(x => x.Field<string>(DatabaseObjects.Columns.BusinessId)).ToList();
                //List<string> bsIds = dtBusinessStrategy.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Id])).ToList();
                //List<string> unmappedBsIds = bsIds.Except(mappedBsIds).Distinct().ToList();
                //foreach (string id in unmappedBsIds)
                //{
                //    DataRow addRow = dtAllInit.NewRow();
                //    DataRow getRow = dtBusinessStrategy.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.Id]) == id).FirstOrDefault();
                //    addRow[DatabaseObjects.Columns.BusinessStrategyLookup] = getRow[DatabaseObjects.Columns.Title];
                //    addRow[DatabaseObjects.Columns.BusinessId] = getRow[DatabaseObjects.Columns.Id];
                //    addRow[DatabaseObjects.Columns.BusinessStrategyDescription] = getRow[DatabaseObjects.Columns.TicketDescription];

                //    dtAllInit.Rows.Add(addRow);
                //}
                DataTable resultdt = UpdateEmptyOrNull(dtAllInit, DatabaseObjects.Columns.BusinessStrategyLookup, DatabaseObjects.Columns.BusinessStrategyLookup);
                dtInitiative = resultdt;
            }
            return dtInitiative;
        }

        public DataTable UpdateEmptyOrNull(DataTable dt, string filterOption, string updateColumn)
        {
            List<string> updatecolumn = new List<string>() { updateColumn };
            DataTable dtBSUnique = new DataTable();
            if (dt == null || dt.Rows.Count == 0)
                return dtBSUnique;

            dtBSUnique = dt.Clone();
            DataRow[] bsGroupingColl = dt.AsEnumerable().Where(x => x.IsNull(filterOption) || x.Field<string>(filterOption) == string.Empty).ToArray();
            if (bsGroupingColl != null && bsGroupingColl.Length > 0)
            {
                DataTable dtEmptyOrNull = bsGroupingColl.CopyToDataTable();
                updateColObj.Program = string.Empty;
                updateColObj.BusinessStratId = string.Empty;
                updateColObj.BSInitiative = string.Empty;
                updateColObj.InitId = string.Empty;
                dtEmptyOrNull.AsEnumerable().ToList().ForEach(x => UpdateRow(x, updateColObj, updatecolumn));

                dtBSUnique.Merge(dtEmptyOrNull);
            }
            bsGroupingColl = dt.AsEnumerable().Where(x => !x.IsNull(filterOption) && x.Field<string>(filterOption) != string.Empty).ToArray();
            if (bsGroupingColl != null && bsGroupingColl.Length > 0)
            {
                DataTable dtEmptyOrNull = bsGroupingColl.CopyToDataTable();
                //dtEmptyOrNull.AsEnumerable().ToList().ForEach(x => UpdateRow(x, string.Empty, updateColumn));

                dtBSUnique.Merge(dtEmptyOrNull);
            }
            return dtBSUnique;
        }

        protected void UpdateRow(DataRow dr, UpdateColumns businessStrategyValue, List<string> updateColumn)
        {
            if (businessStrategyValue != null)
            {
                foreach (string column in updateColumn)
                {
                    switch (column)
                    {

                        case "BusinessId":
                            dr[DatabaseObjects.Columns.BusinessId] = updateColObj.BusinessStratId;
                            break;
                        case "BusinessStrategy":
                            if (!string.IsNullOrEmpty(updateColObj.Program))
                            {
                                dr[DatabaseObjects.Columns.BusinessStrategy] = BsStrategyManager.LoadByID(Convert.ToInt64(updateColObj.Program)).Title;
                            }
                            break;
                        case "InitiativeId":
                            dr[DatabaseObjects.Columns.InitiativeId] = updateColObj.InitId;
                            break;
                        case "ProjectInitiativeLookup":
                            dr[DatabaseObjects.Columns.ProjectInitiativeLookup] = updateColObj.BSInitiative;
                            break;
                        case "BusinessStrategyDescription":
                            dr[DatabaseObjects.Columns.BusinessStrategyDescription] = updateColObj.BsDescription;
                            break;
                        case "InitiativeDescription":
                            dr[DatabaseObjects.Columns.InitiativeDescription] = updateColObj.InitDescription;
                            break;
                        case "BsStrategyTitle":
                            dr["BusinessStrategyTitle"] = updateColObj.BsStrategyTitle;
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        public DataTable GetTasks()
        {
            DataTable dtTasks = null;
            UGITTaskManager taskManager = new UGITTaskManager(_Context);
            DataRow[] tasks = taskManager.GetDataTable().Select(string.Format("{0} = 'PMM' and {1} = 'Task'", DatabaseObjects.Columns.ModuleNameLookup, DatabaseObjects.Columns.TaskBehaviour));
            if (tasks != null && tasks.Count() > 0)
            {
                dtTasks = tasks.CopyToDataTable();
            }
            return dtTasks;
        }

        public DataTable GetIssues()
        {
            UGITTaskManager taskManager = new UGITTaskManager(_Context);
            DataRow[] pmmIssues = taskManager.GetDataTable().Select(string.Format("{0} = 'Issue'", DatabaseObjects.Columns.UGITSubTaskType));
            if (pmmIssues != null && pmmIssues.Count() > 0)
                dtIssues = pmmIssues.CopyToDataTable();

            return dtIssues;
        }

        public DataTable CreateSchema()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(DatabaseObjects.Columns.TotalAmount, typeof(double));
            dt.Columns.Add(DatabaseObjects.Columns.AmountLeft, typeof(double));
            dt.Columns.Add(DatabaseObjects.Columns.AllMonth, typeof(double));
            dt.Columns.Add(DatabaseObjects.Columns.MonthLeft, typeof(double));
            dt.Columns.Add(DatabaseObjects.Columns.Issues, typeof(int));
            dt.Columns.Add(DatabaseObjects.Columns.RiskLevel);
            dt.Columns.Add(DatabaseObjects.Columns.BusinessStrategy);
            dt.Columns.Add(DatabaseObjects.Columns.ProjectInitiativeLookup);
            dt.Columns.Add(DatabaseObjects.Columns.Title);
            dt.Columns.Add(DatabaseObjects.Columns.TitleLink);
            dt.Columns.Add("AmountLeftR");
            dt.Columns.Add("AmountLeftY");
            dt.Columns.Add("AmountLeftG");
            dt.Columns.Add("MonthLeftR");
            dt.Columns.Add("MonthLeftY");
            dt.Columns.Add("MonthLeftG");
            dt.Columns.Add("IssuesR");
            dt.Columns.Add("IssuesY");
            dt.Columns.Add("IssuesG");
            dt.Columns.Add("RiskLevelR");
            dt.Columns.Add("RiskLevelY");
            dt.Columns.Add("RiskLevelG");
            dt.Columns.Add("TopNBS");
            dt.Columns.Add("TopNIni");
            dt.Columns.Add("TopNPro");
            dt.Columns.Add("LongestInstanceBS");
            dt.Columns.Add("LongestInstanceI");
            dt.Columns.Add("LongestInstanceProj");
            dt.Columns.Add("projcount");
            dt.Columns.Add("days");
            dt.Columns.Add("previousDate");
            dt.Columns.Add("nextDate");
            dt.Columns.Add(DatabaseObjects.Columns.TicketId);

            dt.Columns.Add(DatabaseObjects.Columns.BusinessStrategyDescription);
            dt.Columns.Add(DatabaseObjects.Columns.InitiativeDescription);
            dt.Columns.Add(DatabaseObjects.Columns.BusinessId);
            dt.Columns.Add(DatabaseObjects.Columns.InitiativeId);
            return dt;
        }

        public DataTable GetAllProjects()
        {
            DataRow[] pmmColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID}='{_Context.TenantID}'").Select(); // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMProjects, query, spWeb);

            ProjectInitiativeViewManager projectInitiativeMGR = new ProjectInitiativeViewManager(_Context);
            DataRow[] pmmInitiative = projectInitiativeMGR.GetDataTable().Select();  // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ProjectInitiative, spWeb);

            if (pmmColl != null)
            {
                resultentTable = pmmColl.CopyToDataTable();
                if (resultentTable == null) return resultentTable;
                if (resultentTable != null && !resultentTable.Columns.Contains(DatabaseObjects.Columns.BusinessStrategy))
                    resultentTable.Columns.Add(DatabaseObjects.Columns.BusinessStrategy);
            }

            if (resultentTable != null && pmmInitiative != null)
            {
                dtInitiative = pmmInitiative.CopyToDataTable();
                DataTable dtview = resultentTable.DefaultView.ToTable(true, DatabaseObjects.Columns.ProjectInitiativeLookup);
                foreach (DataRow dr in dtview.Rows)
                {
                    DataRow businessStrat = dtInitiative.AsEnumerable().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.Title]) == Convert.ToString(dr[DatabaseObjects.Columns.ProjectInitiativeLookup]));
                    //if (businessStrat != null)
                    //resultentTable.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.ProjectInitiativeLookup]) == Convert.ToString(dr[DatabaseObjects.Columns.ProjectInitiativeLookup])).ToList().ForEach(x => UpdateRow(x, Convert.ToString(businessStrat[DatabaseObjects.Columns.BusinessStrategyLookup]), DatabaseObjects.Columns.BusinessStrategy));
                }
            }
            return resultentTable;
        }

        public DataTable FilterById(List<string> filterIdPost)
        {
            List<Constants.ProjectType> filters = new List<Constants.ProjectType>();
            foreach (string filter in filterIdPost)
            {
                if (filter == Constants.ProjectType.CurrentProjects.ToString())
                {
                    filters.Add(Constants.ProjectType.CurrentProjects);
                    continue;
                }
                else if (filter == Constants.ProjectType.ApprovedNPRs.ToString())
                {
                    filters.Add(Constants.ProjectType.ApprovedNPRs);
                    continue;
                }
                else if (filter == Constants.ProjectType.PendingApprovalNPRs.ToString())
                {
                    filters.Add(Constants.ProjectType.PendingApprovalNPRs);
                    continue;
                }
            }

            resultentTable = ITG.Portfolio.LoadAll(_Context, filters);

            if (resultentTable == null) return resultentTable;
            if (!resultentTable.Columns.Contains(DatabaseObjects.Columns.BusinessStrategy))
                resultentTable.Columns.Add(DatabaseObjects.Columns.BusinessStrategy);
            if (!resultentTable.Columns.Contains(DatabaseObjects.Columns.BusinessId))
                resultentTable.Columns.Add(DatabaseObjects.Columns.BusinessId);
            if (!resultentTable.Columns.Contains(DatabaseObjects.Columns.InitiativeId))
                resultentTable.Columns.Add(DatabaseObjects.Columns.InitiativeId);
            if (!resultentTable.Columns.Contains(DatabaseObjects.Columns.BusinessStrategyDescription))
                resultentTable.Columns.Add(DatabaseObjects.Columns.BusinessStrategyDescription);
            if (!resultentTable.Columns.Contains(DatabaseObjects.Columns.InitiativeDescription))
                resultentTable.Columns.Add(DatabaseObjects.Columns.InitiativeDescription);
            if (!resultentTable.Columns.Contains("BusinessStrategyTitle"))
                resultentTable.Columns.Add("BusinessStrategyTitle");
            if (dtInitiative != null)
            {
                DataTable dtview = resultentTable.DefaultView.ToTable(true, DatabaseObjects.Columns.ProjectInitiativeLookup);
                foreach (DataRow dr in dtview.Rows)
                {
                    DataRow businessStrat = dtInitiative.AsEnumerable().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.ID]) == Convert.ToString(dr[DatabaseObjects.Columns.ProjectInitiativeLookup]));
                    
                    if (businessStrat != null)
                    {
                        List<string> updateColl = new List<string>() { DatabaseObjects.Columns.BusinessId,
                            DatabaseObjects.Columns.InitiativeId,
                            DatabaseObjects.Columns.BusinessStrategy,
                            DatabaseObjects.Columns.InitiativeDescription,
                            DatabaseObjects.Columns.BusinessStrategyDescription
                        };
                        updateColObj.Program = Convert.ToString(businessStrat[DatabaseObjects.Columns.BusinessStrategyLookup]);
                        updateColObj.BusinessStratId = Convert.ToString(businessStrat[DatabaseObjects.Columns.BusinessId]);
                        updateColObj.InitId = Convert.ToString(businessStrat[DatabaseObjects.Columns.Id]);
                        updateColObj.BSInitiative = Convert.ToString(businessStrat[DatabaseObjects.Columns.Title]);
                        updateColObj.BsDescription = Convert.ToString(businessStrat[DatabaseObjects.Columns.BusinessStrategyDescription]);
                        updateColObj.InitDescription = Convert.ToString(businessStrat[DatabaseObjects.Columns.InitiativeDescription]);
                        updateColObj.BsStrategyTitle = Convert.ToString(businessStrat[DatabaseObjects.Columns.Title]);
                        resultentTable.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.ProjectInitiativeLookup]) == Convert.ToString(dr[DatabaseObjects.Columns.ProjectInitiativeLookup])).ToList().ForEach(x => UpdateRow(x, updateColObj, updateColl));
                    }
                }
                //List<string> lstBusinessStrategy = resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.BusinessId)).Select(x => x.Field<string>(DatabaseObjects.Columns.BusinessId)).Distinct().ToList();
                //List<string> lstBusinessStrategyfromInit = dtInitiative.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.BusinessId)).Select(x => x.Field<string>(DatabaseObjects.Columns.BusinessId)).Distinct().ToList();
                //List<string> unmappedBusinessStrategy = lstBusinessStrategyfromInit.Except(lstBusinessStrategy).ToList();
                //if (unmappedBusinessStrategy != null && unmappedBusinessStrategy.Count > 0)
                //{
                //    foreach (string str in unmappedBusinessStrategy)
                //    {
                //        DataRow row = resultentTable.NewRow();
                //        DataRow currRow = dtInitiative.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.BusinessId) && x.Field<string>(DatabaseObjects.Columns.BusinessId) == str).FirstOrDefault();
                //        updateColObj.Program = Convert.ToString(currRow[DatabaseObjects.Columns.BusinessStrategyLookup]);
                //        updateColObj.BusinessStratId = str;
                //        updateColObj.InitId = Convert.ToString(currRow[DatabaseObjects.Columns.Id]);
                //        updateColObj.BSInitiative = Convert.ToString(currRow[DatabaseObjects.Columns.Title]);
                //        updateColObj.BsDescription = Convert.ToString(currRow[DatabaseObjects.Columns.BusinessStrategyDescription]);
                //        updateColObj.InitDescription = Convert.ToString(currRow[DatabaseObjects.Columns.InitiativeDescription]);
                //        List<string> updatecolumn = new List<string>() { DatabaseObjects.Columns.BusinessId, DatabaseObjects.Columns.BusinessStrategy, DatabaseObjects.Columns.ProjectInitiativeLookup,DatabaseObjects.Columns.InitiativeId,DatabaseObjects.Columns.InitiativeDescription,DatabaseObjects.Columns.BusinessStrategyDescription };
                //        UpdateRow(row, updateColObj, updatecolumn);
                //        resultentTable.Rows.Add(row);
                //    }
                //}
            }
            DataView defaultView = resultentTable.DefaultView;
            defaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.TicketActualCompletionDate);
            DataTable newdt = defaultView.ToTable(true);
            resultentTable = newdt;
            return resultentTable;
        }

        public DataTable GenerateData(DataTable result, DataTable data, bool flipview, string title = "")
        {
            if (data == null || data.Rows.Count == 0)
                return result;

            TicketManager ticketManager = new TicketManager(_Context);
            ModuleViewManager moduleManager = new ModuleViewManager(_Context);
            

            DataRow dr = result.NewRow();
            result.Rows.Add(dr);
            if (data.Rows.Count == 1 && result.Columns.Contains(DatabaseObjects.Columns.TicketId))
                dr[DatabaseObjects.Columns.BusinessStrategyDescription] = data.Rows[0][DatabaseObjects.Columns.BusinessStrategyDescription];
            dr[DatabaseObjects.Columns.TicketId] = data.Rows[0][DatabaseObjects.Columns.TicketId];

            dr[DatabaseObjects.Columns.InitiativeDescription] = data.Rows[0][DatabaseObjects.Columns.InitiativeDescription];
            dr[DatabaseObjects.Columns.Title] = title;
            dr[DatabaseObjects.Columns.BusinessId] = data.Rows[0][DatabaseObjects.Columns.BusinessId];
            dr[DatabaseObjects.Columns.InitiativeId] = data.Rows[0][DatabaseObjects.Columns.InitiativeId];

            if (string.IsNullOrWhiteSpace(Convert.ToString(dr[DatabaseObjects.Columns.InitiativeId])))
                dr[DatabaseObjects.Columns.InitiativeId] = string.Empty;

            if (string.IsNullOrWhiteSpace(Convert.ToString(dr[DatabaseObjects.Columns.BusinessId])))
                dr[DatabaseObjects.Columns.BusinessId] = string.Empty;

            if (data.Rows.Count == 0)
                dr[DatabaseObjects.Columns.Title] = data.Rows[0][DatabaseObjects.Columns.TicketId];

            double totalBudget = UGITUtility.StringToDouble(data.Compute(string.Format("sum({0})", DatabaseObjects.Columns.TicketTotalCost), string.Empty));
            double totalSpent = UGITUtility.StringToDouble(data.Compute(string.Format("sum({0})", DatabaseObjects.Columns.ProjectCost), string.Empty));
            dr[DatabaseObjects.Columns.TotalAmount] = totalBudget;
            dr[DatabaseObjects.Columns.AmountLeft] = totalBudget - totalSpent;

            dr["AllMonth"] = 0;
            dr["MonthLeft"] = 0;
            DataRow commondate = null;
            DateTime startdate = DateTime.MinValue;
            DateTime closeDate = DateTime.MinValue;

            DataView viewduration = data.DefaultView;
            DataTable forduration = viewduration.ToTable(true, DatabaseObjects.Columns.TicketActualStartDate, DatabaseObjects.Columns.TicketActualCompletionDate);
            if (forduration != null && forduration.Rows.Count > 0)
            {
                commondate = forduration.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualStartDate)).OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketActualStartDate)).FirstOrDefault();
                if (commondate != null)
                    startdate = Convert.ToDateTime(Convert.ToString(commondate[DatabaseObjects.Columns.TicketActualStartDate]));

                commondate = forduration.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate)).OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate)).FirstOrDefault();
                if (commondate != null)
                    closeDate = Convert.ToDateTime(Convert.ToString(commondate[DatabaseObjects.Columns.TicketActualCompletionDate]));
            }

            if (startdate == DateTime.MinValue)
                startdate = DateTime.Now;
            if (closeDate == DateTime.MinValue)
                closeDate = DateTime.Now;

            int month = (closeDate.Month - startdate.Month) + (12 * (closeDate.Year - startdate.Year));
            dr["AllMonth"] = month;

            int monthleft = (closeDate.Month - DateTime.Now.Month) + (12 * (closeDate.Year - DateTime.Now.Year));
            dr["MonthLeft"] = monthleft;

            double allTaskMonth = month;

            dr["Issues"] = 0;
            if (dtIssues != null && dtIssues.Rows.Count > 0)
            {
                DataRow[] issuesRows = (from a in dtIssues.AsEnumerable()
                                        join
                                        p in data.AsEnumerable() on
                                        a.Field<string>(DatabaseObjects.Columns.TicketId) equals p.Field<string>(DatabaseObjects.Columns.TicketId)
                                        select a).ToArray();

                dr["Issues"] = issuesRows.Length;
            }

            //For Risk Level
            dr["RiskLevel"] = "G";
            //if (data.Rows.Count > 1)
            //{
            double ticketscore = 0;
            if (!data.Columns.Contains("RiskBudgetData"))
                data.Columns.Add("RiskBudgetData", typeof(double), string.Format("[{0}]*[{1}]", DatabaseObjects.Columns.TicketRiskScore, DatabaseObjects.Columns.TicketTotalCost));

            double totalRisk = UGITUtility.StringToDouble(data.Compute("sum([RiskBudgetData])", string.Empty));
            if (totalRisk == 0)
                ticketscore = 0;
            else
                ticketscore = totalRisk / (data.Rows.Count * totalBudget);
            //double riskredthreshold = (ticketscore * 100);
            if (ticketscore >= RiskLevelRedThreshold)
                dr["RiskLevel"] = "R";
            else if (ticketscore >= RiskLevelYellowThreshold && ticketscore < RiskLevelRedThreshold)
                dr["RiskLevel"] = "Y";

            if (flipview)
            {
                #region Top N Percentage

                //Business Strategy Case
                dr["TopNBS"] = 0;
                var outresult = from r in data.AsEnumerable()
                                group r by r.Field<string>(DatabaseObjects.Columns.BusinessStrategy) into grp
                                select new
                                {
                                    commonkey = grp.Key,
                                    totalsum = grp.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketTotalCost)).Sum(x => x.Field<double>(DatabaseObjects.Columns.TicketTotalCost))
                                };
                if (outresult != null)
                {
                    outresult = outresult.OrderByDescending(x => x.totalsum);
                    double topntotal = 0;
                    if (outresult.Count() >= PercentageOfNNumberOfBusinessStrategy)
                        topntotal = outresult.Take(PercentageOfNNumberOfBusinessStrategy).Sum(x => x.totalsum);
                    else
                        topntotal = outresult.Take(PercentageOfNNumberOfBusinessStrategy).Sum(x => x.totalsum);

                    if (topntotal > 0)
                    {
                        double percentageOfTotal = (topntotal * 100) / totalBudget;
                        dr["TopNBS"] = Math.Round(percentageOfTotal, 1);
                    }
                }
                //Initiative Case
                dr["TopNIni"] = 0;
                outresult = from r in data.AsEnumerable()
                            group r by r.Field<string>(DatabaseObjects.Columns.ProjectInitiativeLookup) into grp
                            select new
                            {
                                commonkey = grp.Key,
                                totalsum = grp.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketTotalCost)).Sum(x => x.Field<double>(DatabaseObjects.Columns.TicketTotalCost))
                            };
                if (outresult != null)
                {
                    outresult = outresult.OrderByDescending(x => x.totalsum);
                    double topntotal = 0;
                    if (outresult.Count() >= PercentageOfNNumberOfBusinessStrategy)
                        topntotal = outresult.Take(PercentageOfNNumberOfBusinessStrategy).Sum(x => x.totalsum);
                    else
                        topntotal = outresult.Take(PercentageOfNNumberOfBusinessStrategy).Sum(x => x.totalsum);

                    if (topntotal > 0)
                    {
                        double percentageOfTotal = (topntotal * 100) / totalBudget;
                        dr["TopNIni"] = Math.Round(percentageOfTotal, 1);
                    }
                }

                //Project Case
                dr["TopNPro"] = 0;
                DataView view = data.DefaultView;
                view.Sort = string.Format("{0} desc", DatabaseObjects.Columns.TicketTotalCost);
                DataTable dtTopNProj = view.ToTable(false, new[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.TicketTotalCost });
                DataRow[] arrProj = null;
                if (dtTopNProj != null && dtTopNProj.Rows.Count >= PercentageOfNNumberOfProjects)
                    arrProj = dtTopNProj.AsEnumerable().Take(PercentageOfNNumberOfProjects).ToArray();
                else
                    arrProj = dtTopNProj.AsEnumerable().Take(dtTopNProj.Rows.Count).ToArray();
                if (arrProj != null && arrProj.Length > 0)
                {
                    double topncost = arrProj.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketTotalCost)).Sum(x => x.Field<double>(DatabaseObjects.Columns.TicketTotalCost));
                    if (topncost > 0)
                    {
                        double percentage = (topncost * 100) / totalBudget;
                        dr["TopNPro"] = Math.Round(percentage, 1);
                    }
                }

                #endregion

                #region Longest Instance

                //All Month
                dr["LongestInstanceBS"] = 0;
                //if (dtTasks != null && dtTasks.Rows.Count > 0)
                //{
                var groupedbyfilter = data.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.BusinessStrategy));
                DataTable dt = new DataTable();
                dt.Columns.Add(DatabaseObjects.Columns.BusinessStrategy);
                dt.Columns.Add(DatabaseObjects.Columns.UGITDuration);
                foreach (var key in groupedbyfilter)
                {
                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);
                    row[DatabaseObjects.Columns.BusinessStrategy] = key.Key;
                    DataRow[] rowColl = key.ToArray();
                    if (rowColl != null && rowColl.Length > 0)
                    {
                        DataTable taskcoll = rowColl.CopyToDataTable();
                        DataView longview = taskcoll.DefaultView;
                        taskcoll = longview.ToTable(true, DatabaseObjects.Columns.TicketActualStartDate, DatabaseObjects.Columns.TicketActualCompletionDate);

                        commondate = null;
                        startdate = DateTime.MinValue;
                        closeDate = DateTime.MinValue;
                        month = 0;
                        monthleft = 0;
                        if (taskcoll != null && taskcoll.Rows.Count > 0)
                        {
                            commondate = taskcoll.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualStartDate)).OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketActualStartDate)).FirstOrDefault();
                            if (commondate != null)
                                startdate = Convert.ToDateTime(Convert.ToString(commondate[DatabaseObjects.Columns.TicketActualStartDate]));

                            commondate = taskcoll.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate)).OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate)).FirstOrDefault();
                            if (commondate != null)
                                closeDate = Convert.ToDateTime(Convert.ToString(commondate[DatabaseObjects.Columns.TicketActualCompletionDate]));
                        }

                        if (startdate == DateTime.MinValue)
                            startdate = DateTime.Now;
                        if (closeDate == DateTime.MinValue)
                            closeDate = DateTime.Now;

                        month = (closeDate.Month - startdate.Month) + (12 * (closeDate.Year - startdate.Year));

                        row[DatabaseObjects.Columns.UGITDuration] = month;
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    DataRow topschedule = dt.AsEnumerable().OrderByDescending(x => Convert.ToDouble(x[DatabaseObjects.Columns.UGITDuration])).FirstOrDefault();
                    if (topschedule != null)
                    {
                        double topinstanceduration = Math.Round(Convert.ToDouble(topschedule[DatabaseObjects.Columns.UGITDuration]), 2);
                        //double months = topinstanceduration / (obj.WorkingHours * obj.WorkingDayInMonth);
                        double longestDuration = (topinstanceduration * 100) / allTaskMonth;
                        dr["LongestInstanceBS"] = longestDuration;
                    }
                }

                //Initiative Case
                dr["LongestInstanceI"] = 0;
                groupedbyfilter = data.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.ProjectInitiativeLookup));
                dt.Clear();
                foreach (var key in groupedbyfilter)
                {
                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);
                    row[DatabaseObjects.Columns.BusinessStrategy] = key.Key;
                    DataRow[] rowColl = key.ToArray();
                    if (rowColl != null && rowColl.Length > 0)
                    {
                        DataTable taskcoll = rowColl.CopyToDataTable();
                        row[DatabaseObjects.Columns.UGITDuration] = 0;

                        DataView longview = taskcoll.DefaultView;
                        taskcoll = longview.ToTable(true, DatabaseObjects.Columns.TicketActualStartDate, DatabaseObjects.Columns.TicketActualCompletionDate);

                        commondate = null;
                        startdate = DateTime.MinValue;
                        closeDate = DateTime.MinValue;
                        month = 0;
                        monthleft = 0;
                        if (taskcoll != null && taskcoll.Rows.Count > 0)
                        {
                            commondate = taskcoll.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualStartDate)).OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketActualStartDate)).FirstOrDefault();
                            if (commondate != null)
                                startdate = Convert.ToDateTime(Convert.ToString(commondate[DatabaseObjects.Columns.TicketActualStartDate]));

                            commondate = taskcoll.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate)).OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate)).FirstOrDefault();
                            if (commondate != null)
                                closeDate = Convert.ToDateTime(Convert.ToString(commondate[DatabaseObjects.Columns.TicketActualCompletionDate]));
                        }

                        if (startdate == DateTime.MinValue)
                            startdate = DateTime.Now;
                        if (closeDate == DateTime.MinValue)
                            closeDate = DateTime.Now;

                        month = (closeDate.Month - startdate.Month) + (12 * (closeDate.Year - startdate.Year));
                        row[DatabaseObjects.Columns.UGITDuration] = month;
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    DataRow topschedule = dt.AsEnumerable().OrderByDescending(x => Convert.ToDouble(x[DatabaseObjects.Columns.UGITDuration])).FirstOrDefault();
                    if (topschedule != null)
                    {
                        double topinstanceduration = Math.Round(Convert.ToDouble(topschedule[DatabaseObjects.Columns.UGITDuration]), 2);
                        //double months = topinstanceduration / (obj.WorkingHours * obj.WorkingDayInMonth);
                        double longestDuration = (topinstanceduration * 100) / allTaskMonth;
                        dr["LongestInstanceI"] = longestDuration;
                    }
                }

                //Project Case
                dr["LongestInstanceProj"] = 0;
                double projtotalduration = 0;

                DataView projView = data.DefaultView;
                DataTable protaskcoll = projView.ToTable(true, DatabaseObjects.Columns.TicketActualStartDate, DatabaseObjects.Columns.TicketActualCompletionDate);

                commondate = null;
                startdate = DateTime.MinValue;
                closeDate = DateTime.MinValue;
                month = 0;
                protaskcoll.Columns.Add(DatabaseObjects.Columns.UGITDuration);
                if (protaskcoll != null && protaskcoll.Rows.Count > 0)
                {
                    foreach (DataRow drproj in protaskcoll.Rows)
                    {
                        DateTime startproj = DateTime.MinValue;
                        DateTime closeproj = DateTime.MinValue;
                        startproj = Convert.ToDateTime(drproj[DatabaseObjects.Columns.TicketActualStartDate] is DBNull ? DateTime.MinValue : drproj[DatabaseObjects.Columns.TicketActualStartDate]);
                        closeproj = Convert.ToDateTime(drproj[DatabaseObjects.Columns.TicketActualCompletionDate] is DBNull ? DateTime.MinValue : drproj[DatabaseObjects.Columns.TicketActualCompletionDate]);
                        if (startproj == DateTime.MinValue)
                            startproj = DateTime.Now;
                        if (closeproj == DateTime.MinValue)
                            closeproj = DateTime.Now;

                        month = (closeproj.Month - startproj.Month) + (12 * (closeproj.Year - startproj.Year));
                        drproj[DatabaseObjects.Columns.UGITDuration] = month;

                    }
                    if (protaskcoll != null && protaskcoll.Rows.Count > 0)
                    {
                        DataRow longproj = protaskcoll.AsEnumerable().OrderByDescending(x => Convert.ToDouble(x[DatabaseObjects.Columns.UGITDuration])).FirstOrDefault();
                        if (longproj != null)
                        {
                            projtotalduration = Convert.ToDouble(longproj[DatabaseObjects.Columns.UGITDuration]);
                            double longestDuration = (projtotalduration * 100) / allTaskMonth;
                            dr["LongestInstanceProj"] = projtotalduration;
                        }
                    }
                }

                #endregion

                // Amount Left 
                int redcount = 0;
                int yellowcount = 0;
                int greencount = 0;

                foreach (DataRow row in data.Rows)
                {
                    double totalcost = 0;
                    double.TryParse(Convert.ToString(row[DatabaseObjects.Columns.TicketTotalCost]), out totalcost);

                    double spendcost = 0;
                    double.TryParse(Convert.ToString(row[DatabaseObjects.Columns.ProjectCost]), out spendcost);
                    double costvarience = ((totalcost - spendcost) * 100) / totalcost;
                    if (costvarience >= RedCostVarianceThreshold)
                        redcount++;
                    else if (costvarience >= YellowCostVarianceThreshold && costvarience < RedCostVarianceThreshold)
                        yellowcount++;
                    else
                        greencount++;
                }

                dr["AmountLeftR"] = redcount;
                dr["AmountLeftY"] = yellowcount;
                dr["AmountLeftG"] = greencount;


                //Month left
                redcount = 0;
                yellowcount = 0;
                greencount = 0;
                startdate = DateTime.MinValue;
                closeDate = DateTime.MinValue;

                if (data != null && data.Rows.Count > 0)
                {
                    foreach (DataRow drmonthleft in data.Rows)
                    {
                        startdate = Convert.ToDateTime(drmonthleft[DatabaseObjects.Columns.TicketActualStartDate] is DBNull ? DateTime.MinValue : drmonthleft[DatabaseObjects.Columns.TicketActualStartDate]);
                        closeDate = Convert.ToDateTime(drmonthleft[DatabaseObjects.Columns.TicketActualCompletionDate] is DBNull ? DateTime.MinValue : drmonthleft[DatabaseObjects.Columns.TicketActualCompletionDate]);

                        if (startdate == DateTime.MinValue)
                            startdate = DateTime.Now;
                        if (closeDate == DateTime.MinValue)
                            closeDate = DateTime.Now;

                        month = (closeDate.Month - startdate.Month) + (12 * (closeDate.Year - startdate.Year));
                        double percentageOfleftTask = 0;
                        int durationleft = (closeDate.Month - DateTime.Now.Month) + (12 * (closeDate.Year - DateTime.Now.Year));
                        if (month > 0)
                            percentageOfleftTask = (durationleft * 100) / month;

                        if (percentageOfleftTask >= RedCostVarianceThreshold)
                            redcount++;
                        else if (percentageOfleftTask >= YellowCostVarianceThreshold && percentageOfleftTask < RedCostVarianceThreshold)
                            yellowcount++;
                        else
                            greencount++;
                    }
                }

                dr["MonthLeftR"] = redcount;
                dr["MonthLeftY"] = yellowcount;
                dr["MonthLeftG"] = greencount;

                //Issues

                redcount = 0;
                yellowcount = 0;
                greencount = 0;

                if (dtIssues != null && dtIssues.Rows.Count > 0)
                {
                    DataRow[] issueColl = (from a in dtIssues.AsEnumerable()
                                           join
                                           p in data.AsEnumerable() on
                                           a.Field<string>(DatabaseObjects.Columns.TicketId) equals p.Field<string>(DatabaseObjects.Columns.TicketId)
                                           select a).ToArray();

                    var groupIssues = issueColl.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.TicketId));
                    foreach (var pmmKey in groupIssues)
                    {
                        DataRow[] issuearr = pmmKey.ToArray();
                        if (issuearr != null)
                        {
                            int issuecount = issuearr.Length;
                            if (issuecount >= IssuesRedThreshold)
                                redcount++;
                            else if (issuecount >= IssuesYellowThreshold && issuecount < IssuesRedThreshold)
                                yellowcount++;
                            else
                                greencount++;
                        }

                    }
                }
                dr["IssuesR"] = redcount;
                dr["IssuesY"] = yellowcount;
                dr["IssuesG"] = greencount;

                //Risk Level

                redcount = 0;
                yellowcount = 0;
                greencount = 0;
                ticketscore = 0;

                foreach (DataRow riskRow in data.Rows)
                {
                    totalRisk = UGITUtility.StringToDouble(riskRow["RiskBudgetData"]);
                    double ticketcost = 0;
                    double.TryParse(Convert.ToString(riskRow[DatabaseObjects.Columns.TicketTotalCost]), out ticketcost);
                    if (totalRisk == 0)
                        ticketscore = 0;
                    else
                        ticketscore = totalRisk / ticketcost;
                    ticketscore = Math.Round(UGITUtility.StringToDouble(riskRow[DatabaseObjects.Columns.TicketRiskScore]), 1);
                    //double riskredthreshold = (ticketscore * 100);
                    if (ticketscore >= RiskLevelRedThreshold)
                        redcount++;
                    else if (ticketscore >= RiskLevelYellowThreshold && ticketscore < RiskLevelRedThreshold)
                        yellowcount++;
                    else
                        greencount++;

                }
                dr["RiskLevelR"] = redcount;
                dr["RiskLevelY"] = yellowcount;
                dr["RiskLevelG"] = greencount;

            }
            return result;
        }

        public DataTable GetBusinessStrategyData(string[] filter, List<string> dataFilter, bool bsexist, bool dayplan = false, DateTime? previousDate = null, DateTime? nextDate = null)
        {
            DataTable dt = FilterById(dataFilter);
            DataRow[] drColl = null;

            if (!dayplan)
            {
                string bStartegy = string.Empty;
                string initiative = string.Empty;

                DataTable dtunique = null;
                if (filter == null)
                    filter = new string[] { "businessstrategy" };
                if (filter.Length > 0)
                {
                    string prefix = filter[0];
                    if (prefix == "businessstrategy")
                    {
                        drColl = dt.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId)).Select(x => x).ToArray();
                    }
                    else if (prefix == "businessinitiative")
                    {
                        string bscheck = filter[1];
                        bStartegy = bscheck.ToLower() == "unassigned" ? string.Empty : bscheck;
                        dtunique = UpdateEmptyOrNull(dt, DatabaseObjects.Columns.BusinessStrategy, DatabaseObjects.Columns.BusinessStrategy);
                        drColl = dtunique.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && x.Field<string>(DatabaseObjects.Columns.BusinessId) == bStartegy).Select(x => x).ToArray(); //dtunique.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.BusinessStrategy) == bStartegy).Select(x => x).ToArray();

                    }
                    else if (prefix == "project")
                    {
                        if (bsexist)
                        {
                            string bscheck = filter[1];
                            string inicheck = filter[2];
                            bStartegy = bscheck.ToLower() == "unassigned" ? string.Empty : bscheck;
                            initiative = inicheck.ToLower() == "unassigned" ? string.Empty : inicheck;
                            dtunique = UpdateEmptyOrNull(dt, DatabaseObjects.Columns.BusinessStrategy, DatabaseObjects.Columns.BusinessStrategy);
                            dtunique = UpdateEmptyOrNull(dtunique, DatabaseObjects.Columns.ProjectInitiativeLookup, DatabaseObjects.Columns.ProjectInitiativeLookup);
                            if (string.IsNullOrEmpty(bStartegy) && string.IsNullOrEmpty(initiative))
                                drColl = dtunique.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && (x[DatabaseObjects.Columns.BusinessId] is DBNull || x.Field<string>(DatabaseObjects.Columns.BusinessId) == bStartegy) && (x[DatabaseObjects.Columns.InitiativeId] is DBNull || x.Field<string>(DatabaseObjects.Columns.InitiativeId) == initiative)).Select(x => x).ToArray();
                            else
                                drColl = dtunique.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && x.Field<string>(DatabaseObjects.Columns.BusinessId) == bStartegy && x.Field<string>(DatabaseObjects.Columns.InitiativeId) == initiative).Select(x => x).ToArray();

                        }
                        else
                        {
                            initiative = filter[2].ToLower() == "unassigned" ? string.Empty : filter[2];
                            dtunique = UpdateEmptyOrNull(dt, DatabaseObjects.Columns.ProjectInitiativeLookup, DatabaseObjects.Columns.ProjectInitiativeLookup);
                            drColl = dtunique.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && x.Field<string>(DatabaseObjects.Columns.InitiativeId) == initiative).Select(x => x).ToArray();
                            if (drColl.Length == 0 && string.IsNullOrEmpty(initiative))
                                drColl = dtunique.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && (x[DatabaseObjects.Columns.InitiativeId] is DBNull || x.Field<string>(DatabaseObjects.Columns.InitiativeId) == initiative)).Select(x => x).ToArray();

                        }

                    }

                }
            }
            else
            {
                if (previousDate == null && nextDate == null)
                {
                    drColl = dt.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) < DateTime.Now.Date).ToArray();
                }
                else
                {
                    DateTime previous = Convert.ToDateTime(previousDate).Date;
                    DateTime next = Convert.ToDateTime(nextDate).Date;
                    if (previous == next)
                    {
                        DataTable clone = dt.Clone();

                        drColl = dt.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate)).ToArray();
                        if (drColl != null && drColl.Length > 0)
                            clone = drColl.CopyToDataTable();

                        drColl = resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) >= previous.Date).ToArray();
                        if (drColl != null && drColl.Length > 0)
                            clone.Merge(drColl.CopyToDataTable());

                        drColl = clone.AsEnumerable().Select(x => x).ToArray();
                        //drColl = dt.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) > previous).ToArray();
                    }
                    else
                    {
                        drColl = dt.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && !x.IsNull(DatabaseObjects.Columns.TicketActualCompletionDate) && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) >= previous && x.Field<DateTime>(DatabaseObjects.Columns.TicketActualCompletionDate) < next).ToArray();
                    }
                }
            }

            if (drColl != null && drColl.Length > 0)
            {
                dt = drColl.CopyToDataTable();
            }
            else
                dt.Clear();

            return dt;
        }

        public DataTable GetAllBusinessStrategy()
        {
            BusinessStrategyManager businessStrategyManager = new BusinessStrategyManager(_Context);

            DataRow[] pmmBSInitiative = businessStrategyManager.GetDataTable().Select();
            if (pmmBSInitiative != null && pmmBSInitiative.Count() > 0)
            {
                DataTable resultdt = pmmBSInitiative.CopyToDataTable();

                dtBusinessStrategy = resultdt;
            }
            return dtBusinessStrategy;
        }

        #endregion
    }

    public class UpdateColumns
    {

        public string Program { get; set; }
        public string BSInitiative { get; set; }
        public string BusinessStratId { get; set; }
        public string InitId { get; set; }
        public string BsDescription { get; set; }
        public string BsStrategyTitle { get; set; }
        public string InitDescription { get; set; }
    }
}
