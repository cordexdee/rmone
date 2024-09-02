
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
namespace uGovernIT.Manager.Helper
{
    public class BusinessStrategyDashboard
    {
        #region Properties
        ApplicationContext applicationContext;
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
        #endregion
        ProjectInitiativeViewManager projectInitiativeViewManager;
        public UGITTaskManager uGITTaskManager;
        public TicketManager ticketManager;
        public ModuleViewManager moduleViewManager;
        public BusinessStrategyManager businessStrategyManager;
        #region Methods
        public BusinessStrategyDashboard(ApplicationContext context)
        {
            applicationContext = context;
            projectInitiativeViewManager = new ProjectInitiativeViewManager(applicationContext);
            uGITTaskManager = new UGITTaskManager(applicationContext);
            ticketManager = new TicketManager(applicationContext);
            moduleViewManager = new ModuleViewManager(applicationContext);
            businessStrategyManager = new BusinessStrategyManager(applicationContext);
            dtBusinessStrategy = GetAllBusinessStrategy();
            dtInitiative = GetIntiative();
            dtIssues = GetIssues();
        }
        public DataTable GetIntiative()
        {
            DataTable dtProjectInitiative = projectInitiativeViewManager.GetDataTable();
            DataTable dtAllInit = new DataTable();
            dtAllInit.Columns.Add(DatabaseObjects.Columns.Title);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.ID);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.BusinessStrategyLookup);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.BusinessId);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.BusinessStrategyDescription);
            dtAllInit.Columns.Add(DatabaseObjects.Columns.InitiativeDescription);
            if (dtProjectInitiative != null && dtProjectInitiative.Rows.Count > 0)
            {
                foreach (DataRow item in dtProjectInitiative.Rows)
                {
                    DataRow addRow = dtAllInit.NewRow();
                    dtAllInit.Rows.Add(addRow);
                    addRow[DatabaseObjects.Columns.Title] = item[DatabaseObjects.Columns.Title];
                    addRow[DatabaseObjects.Columns.ID] = item[DatabaseObjects.Columns.ID];
                    addRow[DatabaseObjects.Columns.InitiativeDescription] = item[DatabaseObjects.Columns.ProjectNote];
                    string value = (Convert.ToString(item[DatabaseObjects.Columns.BusinessStrategyLookup]));
                    if (value != null)
                    {
                        DataRow bsRow = dtBusinessStrategy.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ID) == (value)).FirstOrDefault();
                        if (bsRow != null)
                        {
                            addRow[DatabaseObjects.Columns.BusinessStrategyLookup] = value;
                            addRow[DatabaseObjects.Columns.BusinessId] = value;
                            addRow[DatabaseObjects.Columns.BusinessStrategyDescription] = Convert.ToString(bsRow[DatabaseObjects.Columns.TicketDescription]);
                        }
                    }
                }
                DataTable resultdt = UpdateEmptyOrNull(dtAllInit, DatabaseObjects.Columns.BusinessStrategyLookup, DatabaseObjects.Columns.BusinessStrategyLookup);
                if (resultdt != null && resultdt.Rows.Count > 0)
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
                            dr[DatabaseObjects.Columns.BusinessStrategy] = updateColObj.Program;
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
                        default:
                            break;
                    }
                }

            }
        }
        public DataTable GetTasks()
        {
            DataTable dtTasks = null;
            // SPListItemCollection pmmTasks = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMTasks, spWeb);
            DataTable tasks = uGITTaskManager.GetDataTable();// pmmTasks.GetDataTable();
            if (tasks != null)
            {
                dtTasks = tasks;
            }
            return dtTasks;
        }
        public DataTable GetIssues()
        {
            //SPListItemCollection pmmIssues = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMIssues, spWeb);
            dtIssues = uGITTaskManager.GetDataTable($"{DatabaseObjects.Columns.UGITSubTaskType} = 'Issues'"); //pmmIssues.GetDataTable();

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
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><Neq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Neq></Where>", DatabaseObjects.Columns.TicketStatus, "Completion");
            DataTable dataTable = ticketManager.GetAllTickets(moduleViewManager.LoadByName("PMM"));
            DataRow[] pmmColl = null;
            if (dataTable != null && dataTable.Rows.Count > 0)
                pmmColl = dataTable.Select(string.Format("{0}<>'{1}'", DatabaseObjects.Columns.TicketStatus, "Completion"));
            DataTable pmmInitiative = projectInitiativeViewManager.GetDataTable(); //SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ProjectInitiative, spWeb);
            if (pmmColl != null)
            {
                resultentTable = pmmColl.CopyToDataTable();
                if (resultentTable == null) return resultentTable;
                if (resultentTable != null && !resultentTable.Columns.Contains(DatabaseObjects.Columns.BusinessStrategy))
                    resultentTable.Columns.Add(DatabaseObjects.Columns.BusinessStrategy);
            }

            if (resultentTable != null && pmmInitiative != null)
            {
                dtInitiative = pmmInitiative.Copy();
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

            resultentTable = Portfolio.LoadAll(applicationContext, filters);

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
            if (dtInitiative != null)
            {
                DataTable dtview = resultentTable.DefaultView.ToTable(true, DatabaseObjects.Columns.ProjectInitiativeLookup);
                foreach (DataRow dr in dtview.Rows)
                {
                    DataRow businessStrat = dtInitiative.AsEnumerable().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.Title]) == Convert.ToString(dr[DatabaseObjects.Columns.ProjectInitiativeLookup]));
                    if (businessStrat != null)
                    {
                        List<string> updateColl = new List<string>() { DatabaseObjects.Columns.BusinessId, DatabaseObjects.Columns.InitiativeId, DatabaseObjects.Columns.BusinessStrategy, DatabaseObjects.Columns.InitiativeDescription, DatabaseObjects.Columns.BusinessStrategyDescription };
                        updateColObj.Program = Convert.ToString(businessStrat[DatabaseObjects.Columns.BusinessStrategyLookup]);
                        updateColObj.BusinessStratId = Convert.ToString(businessStrat[DatabaseObjects.Columns.BusinessId]);
                        updateColObj.InitId = Convert.ToString(businessStrat[DatabaseObjects.Columns.Id]);
                        updateColObj.BSInitiative = Convert.ToString(businessStrat[DatabaseObjects.Columns.Title]);
                        updateColObj.BsDescription = Convert.ToString(businessStrat[DatabaseObjects.Columns.BusinessStrategyDescription]);
                        updateColObj.InitDescription = Convert.ToString(businessStrat[DatabaseObjects.Columns.InitiativeDescription]);
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

            DataRow dr = result.NewRow();
            result.Rows.Add(dr);
            if (data.Rows.Count == 1 && result.Columns.Contains(DatabaseObjects.Columns.TicketId))
                dr[DatabaseObjects.Columns.BusinessStrategyDescription] = data.Rows[0][DatabaseObjects.Columns.BusinessStrategyDescription];
            dr[DatabaseObjects.Columns.TicketId] = data.Rows[0][DatabaseObjects.Columns.TicketId];

            dr[DatabaseObjects.Columns.InitiativeDescription] = data.Rows[0][DatabaseObjects.Columns.InitiativeDescription];
            dr[DatabaseObjects.Columns.Title] = title;
            dr[DatabaseObjects.Columns.BusinessId] = data.Rows[0][DatabaseObjects.Columns.BusinessId];
            dr[DatabaseObjects.Columns.InitiativeId] = data.Rows[0][DatabaseObjects.Columns.InitiativeId];

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
            DataTable resultdt = businessStrategyManager.GetDataTable();
            if (resultdt != null && resultdt.Rows.Count > 0)
                dtBusinessStrategy = resultdt;
            // SPList spListBS = SPListHelper.GetSPList(DatabaseObjects.Lists.BusinessStrategy, spWeb);
            // SPQuery query = new SPQuery();
            // query.Query = string.Format("<Where></Where>");
            // query.ViewFields = string.Concat(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title),
            //  string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id),
            //  string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TicketDescription)
            //  );
            //  query.ViewFieldsOnly = true;
            //  SPListItemCollection pmmBSInitiative = spListBS.GetItems(query);
            //if (pmmBSInitiative != null)
            //{
            //    DataTable resultdt = pmmBSInitiative.GetDataTable();

            //    dtBusinessStrategy = resultdt;
            //}
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
        public string InitDescription { get; set; }
    }
    public class Portfolio
    {
        protected string Title { get; set; }
        protected string Priority { get; set; }
        protected string TicketStatus { get; set; }
        protected float BudgetAmount { get; set; }
        protected string Actual { get; set; }
        protected float CostToCompletion { get; set; }
        protected string TicketActualStartDate { get; set; }
        protected string TicketActualCompletionDate { get; set; }
        protected string ProjectStatus { get; set; }
        protected static DateTime yearStartDate = DateTime.Now;
        protected static DateTime yearEndDate = DateTime.Now;

        public static DataTable LoadAll(ApplicationContext applicationContext, List<Constants.ProjectType> projectType)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(applicationContext);
            DataTable result = CreateTable();
            UGITModule nprModule = moduleViewManager.LoadByName("NPR"); //uGITCache.ModuleConfigCache.GetCachedModule("NPR");
            UGITModule pmmModule = moduleViewManager.LoadByName("PMM");//uGITCache.ModuleConfigCache.GetCachedModule("PMM");

            bool queryNPR = false;
            bool queryPMM = false;

            //Builds query based on type
            List<string> requiredQuery = new List<string>();
            List<string> nprRequestQuery = new List<string>();

            if (projectType.Contains(Constants.ProjectType.All) || projectType.Contains(Constants.ProjectType.CurrentProjects) || projectType.Contains(Constants.ProjectType.CompletedProjects))
            {
                queryPMM = true;
            }

            if (projectType.Contains(Constants.ProjectType.All) || projectType.Contains(Constants.ProjectType.ApprovedNPRs) || projectType.Contains(Constants.ProjectType.OnHold) || projectType.Contains(Constants.ProjectType.PendingApprovalNPRs))
            {
                queryNPR = true;
            }

            //Add this query only when pending approval view is not added. if pending approval view is added then we are adding approved view inside it.
            if (projectType.Contains(Constants.ProjectType.ApprovedNPRs))
            {
                //Fetchs NPRs whose current stage is approved
                LifeCycleStage approvedStage = nprModule.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.Prop_ReadyToImport.HasValue && x.Prop_ReadyToImport.Value);
                nprRequestQuery.Add(string.Format("{0}={1}",
                DatabaseObjects.Columns.StageStep, approvedStage.StageStep));
            }

            if (projectType.Contains(Constants.ProjectType.PendingApprovalNPRs))
            {
                //Fetchs NPRs whose current stage is ITGReview and SCReview
                // First add all NPR steps before Approved stage
                LifeCycleStage approvedStage = nprModule.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.Prop_ReadyToImport.HasValue && x.Prop_ReadyToImport.Value);
                List<string> stepExps = new List<string>();
                string stepExp = string.Empty;
                List<LifeCycleStage> approvalStages = nprModule.List_LifeCycles.FirstOrDefault().Stages.Where(x => x.StageStep < approvedStage.StageStep).ToList();
                foreach (LifeCycleStage stage in approvalStages)
                {
                    stepExps.Add(string.Format("{0}={1}", DatabaseObjects.Columns.StageStep, stage.StageStep));
                }

                // Then exclude all projects on hold
                if (!projectType.Contains(Constants.ProjectType.OnHold))
                {
                    stepExp = "(" + string.Join(" OR ", stepExps) + ")";
                    nprRequestQuery.Add(string.Format("{2} And {0} <> '{1}'", DatabaseObjects.Columns.TicketStatus, Constants.OnHoldStatus, stepExp));
                }
                else
                {
                    stepExp = "(" + string.Join(" OR ", stepExps) + ")";
                    nprRequestQuery.Add(stepExp);
                }

            }

            if (projectType.Contains(Constants.ProjectType.OnHold))
            {
                //Shows Only onhold tickets
                nprRequestQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketStatus, Constants.OnHoldStatus));
            }

            string pmmQuery = string.Empty;
            List<string> pmmQueryStr = new List<string>();
            if (requiredQuery.Count > 0)
            {
                pmmQuery = "(" + string.Join(" OR ", requiredQuery) + ")";
                pmmQueryStr.Add(pmmQuery);
            }

            if (!(projectType.Contains(Constants.ProjectType.CompletedProjects) && projectType.Contains(Constants.ProjectType.CurrentProjects)))
            {
                if (projectType.Contains(Constants.ProjectType.CompletedProjects) && !projectType.Contains(Constants.ProjectType.CurrentProjects))
                    pmmQueryStr.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketClosed, "True"));
                else
                    pmmQueryStr.Add(string.Format("{0} <> '{1}' or {0} is null", DatabaseObjects.Columns.TicketClosed, "True"));
            }
            pmmQuery = "(" + string.Join(" And ", pmmQueryStr) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(pmmQueryStr, true));

            List<string> nprQueryStr = new List<string>();
            if (nprRequestQuery.Count > 0)
            {
                nprQueryStr.Add("(" + string.Join(" OR ", nprRequestQuery) + ")");
                //nprQueryStr.Add(uHelper.GenerateWhereQueryWithAndOr(nprRequestQuery, false));
            }
            nprQueryStr.Add(string.Format("( {0} <> '{1}' OR {2} is Null )", DatabaseObjects.Columns.TicketClosed, "True",DatabaseObjects.Columns.TicketPMMIdLookup));
            //nprQueryStr.Add(string.Format(" ( OR {0} is Null", DatabaseObjects.Columns.TicketPMMIdLookup+ " ) "));

            string nprQuery = string.Empty;
            nprQuery = "(" + string.Join(" And ", nprQueryStr) + ")"; //string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(nprQueryStr, true));

            if (queryNPR)
            {
                DataTable dataTable = GetTableDataManager.GetTableData(nprModule.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{applicationContext.TenantID}'");
                DataTable nprRData = null;
                DataRow[] nprItemCollection = null;
                if (dataTable != null && dataTable.Rows.Count > 0)
                    nprItemCollection = dataTable.Select(nprQuery);
                if (nprItemCollection != null && nprItemCollection.Length > 0)
                {
                    nprRData = nprItemCollection.CopyToDataTable();
                }
                if (nprRData != null && nprRData.Rows.Count > 0)
                {
                    result = GeneratePortfolioTable(nprModule, nprRData);
                }
            }

            if (queryPMM)
            {
                DataTable pmm = GetTableDataManager.GetTableData(pmmModule.ModuleTable, $"{DatabaseObjects.Columns.TenantID} = '{applicationContext.TenantID}'");
                DataRow[] pmmItemCollection = null;
                DataTable pmmRData = null;
                if (pmm != null && pmm.Rows.Count > 0)
                {
                    pmmItemCollection = pmm.Select(pmmQuery);
                }
                if (pmmItemCollection != null && pmmItemCollection.Length > 0)
                {
                    pmmRData = pmmItemCollection.CopyToDataTable();
                }
                if (pmmRData != null)
                {
                    DataTable table = GeneratePortfolioTable(pmmModule, pmmRData);
                    if (result == null || result.Rows.Count <= 0)
                        result = table;
                    else if (table != null && table.Rows.Count > 0)
                        result.Merge(table);
                }
            }

            return result;
        }

        private static DataTable GeneratePortfolioTable(UGITModule module, DataTable rawData)
        {
            DataTable result = CreateTable();
            if (rawData == null)
                return result;

            string absoluteURL = UGITUtility.GetAbsoluteURL(module.DetailPageUrl);
            rawData.DefaultView.Sort = DatabaseObjects.Columns.Title; // sort by title
            foreach (DataRowView rowView in rawData.DefaultView)
            {
                DataRow rRow = rowView.Row;
                DataRow newRow = result.NewRow();
                result.Rows.Add(newRow);

                newRow[DatabaseObjects.Columns.ModuleName] = module.ModuleName;

                foreach (DataColumn dc in result.Columns)
                {
                    object value = UGITUtility.GetSPItemValue(rRow, dc.ColumnName);
                    if (dc.DataType.FullName == "System.Single"
                                   || dc.DataType.FullName == "System.Double")
                    {
                        if (Convert.ToString(value) != string.Empty)
                            newRow[dc.ColumnName] = UGITUtility.StringToDouble(value);
                        else
                            newRow[dc.ColumnName] = 0;
                    }
                    else if (dc.DataType.FullName == "System.String" && !dc.ColumnName.ToLower().Contains("lookup"))
                    {
                        if (Convert.ToString(value) != string.Empty)
                            newRow[dc.ColumnName] = value.ToString();
                        else
                            newRow[dc.ColumnName] = string.Empty;
                    }
                    else if (dc.DataType.FullName == "System.DateTime")
                    {
                        if (rRow[dc.ColumnName] != null && Convert.ToString(rRow[dc.ColumnName]) != string.Empty)
                            newRow[dc.ColumnName] = Convert.ToString(rRow[dc.ColumnName]);
                        else
                            newRow[dc.ColumnName] = DBNull.Value;
                    }
                    else
                        newRow[dc.ColumnName] = UGITUtility.StripHTML(Convert.ToString(rRow[dc.ColumnName]));
                }

                float estimatedCost = 0;
                float.TryParse(Convert.ToString(rRow[DatabaseObjects.Columns.TicketTotalCost]), out estimatedCost);

                float actualCost = 0;
                float.TryParse(Convert.ToString(rRow[DatabaseObjects.Columns.ProjectCost]), out actualCost);
                newRow["Actual"] = actualCost;
                //newRow[DatabaseObjects.Columns.CostToCompletion] = estimatedCost;
                newRow[DatabaseObjects.Columns.BudgetAmount] = estimatedCost;
                newRow[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("{0:C}", 0);

                string ticketTitle = UGITUtility.TruncateWithEllipsis(newRow[DatabaseObjects.Columns.Title].ToString(), 100, string.Empty);
                string title = string.Empty;
                if (newRow[DatabaseObjects.Columns.TicketId] != null)
                {
                    title = string.Format("{0}: ", newRow[DatabaseObjects.Columns.TicketId]);
                }
                title = string.Format("{0}{1}", title, ticketTitle);
                title = uHelper.ReplaceInvalidCharsInURL(title);// # ' " cause issues!

                if (estimatedCost > 0)
                {
                    if (module.ModuleName == "PMM")
                    {
                        string url = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/ProjectManagement.aspx");
                        newRow[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("<a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','control=ProjectBudgetDetail&IsReadOnly=true&pmmid={1}&isdlg=1&isudlg=1','{3}', 90, 80);\">{2}</a>", url, rRow[DatabaseObjects.Columns.Id], String.Format("{0:C}", estimatedCost), title);
                    }
                    else
                    {
                        string url = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/newprojectmanagement.aspx");
                        newRow[DatabaseObjects.Columns.BudgetAmountWithLink] = string.Format("<a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','control=nprbudget&IsReadOnly=true&NPRID={1}','{3}', 90, 80);\">{2}</a>", url, rRow[DatabaseObjects.Columns.Id], String.Format("{0:C}", estimatedCost), title);
                    }
                }

                //added hidden span block to perform sorting correctly when we show data in gridview.
                newRow[DatabaseObjects.Columns.TitleLink] = string.Format("<span style='display:none;'>{2}</span><a href='javascript:void(0)' onclick=\"window.parent.UgitOpenPopupDialog('{0}','TicketId={1}','{3}', 90, 80);\">{2}</a>", absoluteURL, rRow[DatabaseObjects.Columns.TicketId], rRow[DatabaseObjects.Columns.Title], title);
                newRow[DatabaseObjects.Columns.ModuleName] = module.ModuleName;
                newRow[DatabaseObjects.Columns.Title] = rRow[DatabaseObjects.Columns.Title].ToString();
            }

            return result;
        }

        private static DataTable CreateTable()
        {
            DataTable result = new DataTable("ITGConstraints");
            result.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            result.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ModuleName, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketPriorityLookup, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketStatus, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.BudgetAmount, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.BudgetAmountWithLink, typeof(string));

            DataColumn dateColumn = result.Columns.Add(DatabaseObjects.Columns.TicketActualStartDate, typeof(DateTime));
            dateColumn.AllowDBNull = true;

            dateColumn = result.Columns.Add(DatabaseObjects.Columns.TicketActualCompletionDate, typeof(DateTime));
            dateColumn.AllowDBNull = true;
            result.Columns.Add("Actual", typeof(double));
            //result.Columns.Add(DatabaseObjects.Columns.CostToCompletion, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.BudgetDescription, typeof(string));
            //Measures
            result.Columns.Add(DatabaseObjects.Columns.TicketNoOfFTEs, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketNoOfFTEsNotes, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketNoOfConsultants, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketNoOfConsultantsNotes, typeof(string));
            //Constraints
            result.Columns.Add(DatabaseObjects.Columns.TicketTotalCost, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.TicketTotalCostsNotes, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketTotalStaffHeadcount, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketTotalStaffHeadcountNotes, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketTotalOnSiteConsultantHeadcount, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketTotalConsultantHeadcountNotes, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketTotalOffSiteConsultantHeadcount, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.TitleLink, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ProjectInitiativeLookup, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ProjectRank, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketRequestTypeCategory, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketRequestTypeLookup, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.TicketRequestTypeSubCategory, typeof(string));
            result.Columns.Add(DatabaseObjects.Columns.ProjectCost, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.TicketRiskScore, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.TicketPctComplete, typeof(double));
            result.Columns.Add(DatabaseObjects.Columns.TicketProjectManager);
            result.Columns.Add(DatabaseObjects.Columns.TicketDesiredCompletionDate, typeof(DateTime));
            return result;
        }
    }
}
