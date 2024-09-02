using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    class TicketByPRPAndStatus
    {
        public string Category { get; set; }
        public string PRP { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }

    class WeeklyTeamPerformance
    {
        public int Sequence { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }

    class NonPeakHoursPerformance
    {
        public int Sequence { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class ModuleStatistics
    {
        UserProfileManager UserManager;

        UserProfile User;

        ModuleUserStatisticsManager _moduleUserStatsManager;

        ApplicationContext _context;

        ModuleViewManager _moduleViewManager;

        TicketManager _ticketManager = null;
        ConfigurationVariableManager objConfigurationVariableHelper = null;
        FieldConfigurationManager _configFieldManager;

        public ModuleStatistics(ApplicationContext context)
        {
            _context = context;
            UserManager = context.UserManager;
            User = context.CurrentUser;
            _moduleUserStatsManager = new Manager.ModuleUserStatisticsManager(context);
            _moduleViewManager = new ModuleViewManager(context);
            _ticketManager = new TicketManager(context);
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
        }

        //public ModuleStatisticResponse Load(ModuleStatisticRequest request)
        //{
        //    if (request.Tabs == null)
        //        request.Tabs = new List<string>();

        //    //Add current tab if not exist
        //    if (request.CurrentTab != null && !request.Tabs.Contains(request.CurrentTab))
        //        request.Tabs.Add(request.CurrentTab);

        //    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
        //    statistics.ModuleName = request.ModuleName;

        //    ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
        //    UGITModule module = moduleViewManager.LoadByName(request.ModuleName);// uGITCache.ModuleConfigCache.GetCachedModule(request.SPWebObj, request.ModuleName);
        //                                                                         //Look ahead functionality
        //    if (module != null)
        //    {
        //        statistics.ModuleTitle = statistics.ModuleTitle;
        //        if (request.ModuleName == "WIKI")
        //        {
        //            statistics.ResultedData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.WikiArticles);
        //        }
        //        else
        //        {
        //            statistics = GetCustomFilterTabData(request);
        //        }
        //    }
        //    return statistics;
        //}

        public ModuleStatisticResponse Load(ModuleStatisticRequest request, bool includeClosed = false)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            if (request.Tabs == null)
                request.Tabs = new List<string>();

            //Add current tab if not exist
            if (request.CurrentTab != null && !request.Tabs.Contains(request.CurrentTab))
                request.Tabs.Add(request.CurrentTab);

            ModuleStatisticResponse statistics = new ModuleStatisticResponse();
            statistics.ModuleName = request.ModuleName;

            if (request.ModuleName == "WIKI")
            {
                values.Add("@TenantID", _context.TenantID);
                statistics.ResultedData = GetTableDataManager.GetData(DatabaseObjects.Tables.WikiArticles, values);
            }
            else if (request.ModuleName == "HLP")
            {
                statistics.ResultedData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.HelpCard, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                //BTS-23-001038: Help tickets are displayed under "Waiting On Me" tab, but their count was not getting added to the ticket count on tab's text.
                if (statistics.ResultedData != null)
                {
                    statistics.TabCounts = new Dictionary<string, int>();
                    statistics.TabCounts.Add(FilterTab.waitingonme, statistics.ResultedData.Rows.Count);
                }
            }
            else
            {
                statistics = GetCustomFilterTabData(request, includeClosed);
            }
            return statistics;
        }


        public ModuleStatisticResponse Load(ModuleStatisticRequest request, TicketStatus status, string userType)
        {
            if (!string.IsNullOrWhiteSpace(userType))
                userType = userType.ToLower();

            if (userType == "my" && status == TicketStatus.Open)
            {
                request.CurrentTab = CustomFilterTab.MyOpenTickets.ToString();
            }
            else if (userType == "mygroup" && status == TicketStatus.Open)
            {
                request.CurrentTab = CustomFilterTab.MyGroupTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Open)
            {
                request.CurrentTab = CustomFilterTab.OpenTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Closed)
            {
                request.CurrentTab = CustomFilterTab.CloseTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.WaitingOnMe)
            {
                request.CurrentTab = CustomFilterTab.WaitingOnMe.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Unassigned)
            {
                request.CurrentTab = CustomFilterTab.UnAssigned.ToString();
            }
            else if (userType == "all" && status == TicketStatus.All)
            {
                request.CurrentTab = CustomFilterTab.AllTickets.ToString();
            }
            else if (status == TicketStatus.Department)
            {
                request.CurrentTab = CustomFilterTab.MyDepartmentTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Approved)
            {
                request.CurrentTab = CustomFilterTab.ResolvedTickets.ToString();
            }
            else if (userType == "my" && status == TicketStatus.Closed)
            {
                request.CurrentTab = CustomFilterTab.MyCloseTickets.ToString();
            }
            return Load(request);
        }

        public List<ModuleStatisticResponse> LoadAll(ModuleStatisticRequest request, bool includeClosed = false)
        {
            List<ModuleStatisticResponse> statistics = new List<ModuleStatisticResponse>();

            if (request.Tabs == null)
                request.Tabs = new List<string>();

            //Add current tab if not exist
            if (request.CurrentTab != null && !request.Tabs.Contains(request.CurrentTab))
                request.Tabs.Add(request.CurrentTab);

            List<UGITModule> moduleRow = null;
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);

            if (request.ModuleType == ModuleType.All)
                moduleRow = moduleViewManager.LoadAllModule().Where(x => x.EnableModule).ToList();
            else
                moduleRow = moduleViewManager.LoadAllModule().Where(x => x.ModuleType == request.ModuleType && x.EnableModule).ToList();

            if (moduleRow != null && moduleRow.Count > 0)
            {
                List<string> hideTickets = new List<string>();
                foreach (UGITModule module in moduleRow)
                {
                    if (request.IsGlobalSearch && !UGITUtility.StringToBoolean(module.UseInGlobalSearch))
                        continue;

                    request.HideTickets = hideTickets;
                    request.ModuleName = module.ModuleName;
                    ModuleStatisticResponse stats = Load(request, includeClosed);
                    if (stats != null)
                    {
                        statistics.Add(stats);
                    }
                }
            }
            return statistics;
        }

        public List<ModuleStatisticResponse> LoadAll(ModuleStatisticRequest request, TicketStatus status, string userType)
        {
            if (!string.IsNullOrWhiteSpace(userType))
                userType = userType.ToLower();

            if (userType == "my" && status == TicketStatus.Open)
            {
                request.CurrentTab = CustomFilterTab.MyOpenTickets.ToString();
            }
            else if (userType == "mygroup" && status == TicketStatus.Open)
            {
                request.CurrentTab = CustomFilterTab.MyGroupTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Open)
            {
                request.CurrentTab = CustomFilterTab.OpenTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Closed)
            {
                request.CurrentTab = CustomFilterTab.CloseTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.WaitingOnMe)
            {
                request.CurrentTab = CustomFilterTab.WaitingOnMe.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Unassigned)
            {
                request.CurrentTab = CustomFilterTab.UnAssigned.ToString();
            }
            else if (userType == "all" && status == TicketStatus.All)
            {
                request.CurrentTab = CustomFilterTab.AllTickets.ToString();
            }
            else if (status == TicketStatus.Department)
            {
                request.CurrentTab = CustomFilterTab.MyDepartmentTickets.ToString();
            }
            else if (userType == "all" && status == TicketStatus.Approved)
            {
                request.CurrentTab = CustomFilterTab.ResolvedTickets.ToString();
            }
            else if (userType == "my" && status == TicketStatus.Closed)
            {
                request.CurrentTab = CustomFilterTab.MyCloseTickets.ToString();
            }
            return LoadAll(request);
        }

        //private ModuleStatisticResponse GetCustomFilterTabData(ModuleStatisticRequest request)
        //{
        //    //return if statistic is null or modulename is not exist
        //    if (request == null || request.ModuleName.Trim() == string.Empty)
        //    {
        //        return null;
        //    }

        //    // Get module detail
        //    UGITModule module = _moduleViewManager.LoadByName(request.ModuleName);

        //    if (module == null)
        //        return null;

        //    UserProfile currentUser = User;

        //    string userName = currentUser != null ? currentUser.Name : string.Empty;

        //    List<string> currentUserNdGroup = new List<string>();
        //    if (currentUser != null)
        //    {
        //        currentUserNdGroup.Add(request.UserID ?? currentUser.Id);

        //        // List<UserProfile> groups = currentUser.Roles();
        //        var roles = UserManager.GetUserRoles(request.UserID ?? currentUser.Id);
        //        foreach (var group in roles)
        //        {
        //            currentUserNdGroup.Add(group.Id);
        //        }
        //    }

        //    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
        //    statistics.TabCounts = new Dictionary<string, int>();
        //    if (request.Tabs != null && request.Tabs.Count > 0)
        //    {
        //        foreach (string tab in request.Tabs)
        //        {
        //            statistics.TabCounts.Add(tab, 0);
        //        }
        //    }
        //    statistics.CurrentTab = request.CurrentTab;
        //    statistics.CurrentTabCount = 0;
        //    statistics.ResultedData = null;
        //    statistics.ModuleName = request.ModuleName;

        //    // Gets data from cache
        //    DataTable openTicketsTable = null;
        //    DataTable closeTicketTable = null;
        //    //DataTable allTicketTable = null;
        //    DataTable moduleUserStatisticsListTable = null;

        //    //if (request.Tabs.Exists(x => x == FilterTab.alltickets /*"alltickets"*/)) //not needed only used in allopentickets tab but we can use opentickets table instead
        //    //{
        //    //    allTicketTable = _ticketManager.GetOpenTickets(module);    // uGITCache.ModuleDataCache.GetAllTickets(module.ID, request.SPWebObj);   //need to discuss:MK
        //    //}
        //    if (request.Tabs.Exists(x => x == FilterTab.allclosedtickets) || request.Tabs.Exists(x => x == "myclosed") || request.Tabs.Exists(x => x == "CloseTickets"))
        //    {
        //        closeTicketTable = _ticketManager.GetClosedTickets(module);
        //    }
        //    if (request.Tabs.Exists(x => x == FilterTab.myopentickets) || request.Tabs.Exists(x => x == FilterTab.mygrouptickets))
        //    {
        //        moduleUserStatisticsListTable = UGITUtility.ToDataTable<ModuleUserStatistic>(_moduleUserStatsManager.GetModuleUserStatistics());
        //    }

        //    openTicketsTable = _ticketManager.GetOpenTickets(module, request);

        //    statistics.ResultedData = openTicketsTable;
        //    #region UnAssigned
        //    //if (request.Tabs.Exists(x => x == FilterTab.unassigned))
        //    if (request.Tabs.Exists(x => x.Equals(FilterTab.unassigned, StringComparison.InvariantCultureIgnoreCase)))
        //    {
        //        DataTable unassingedData = _ticketManager.GetUnassignedTickets(request.ModuleName);
        //        // Tab tab = new Tab(request.Tabs.Find(x => x.Equals(FilterTab.unassigned, StringComparison.InvariantCultureIgnoreCase)), request.Tabs.Find(x => x.Equals(FilterTab.unassigned, StringComparison.InvariantCultureIgnoreCase)));
        //        Tab tab = new Tab("UnAssigned", "UnAssigned");
        //        if (unassingedData != null)
        //        {
        //            statistics.TabCounts[tab.Name] = unassingedData.Rows.Count;
        //        }

        //        if (request.CurrentTab == tab.Name)
        //        {
        //            statistics.CurrentTabCount = unassingedData.Rows.Count;
        //            statistics.ResultedData = unassingedData;
        //        }
        //    }
        //    #endregion

        //    #region Waiting On Me

        //    if (request.Tabs.Find(x => x == FilterTab.waitingonme) != null && openTicketsTable != null)
        //    {
        //        if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
        //        {

        //            if (!openTicketsTable.Columns.Contains("StageActionUser1"))
        //                openTicketsTable.Columns.Add("StageActionUser1");
        //            openTicketsTable.Columns["StageActionUser1"].Expression = string.Format("'{0}'", DatabaseObjects.Columns.TicketStageActionUsers);
        //            if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
        //            {
        //                DataRow[] dr = new DataRow[0];
        //                if (module.WaitingOnMeIncludesGroups)
        //                {
        //                    string query = string.Join(" or ", currentUserNdGroup.Select(x => string.Format("{0} like '%{2} {1}{2}%'", "StageActionUser1", x, Constants.Separator5)));
        //                    dr = openTicketsTable.Select(query);
        //                }
        //                else
        //                {
        //                    dr = openTicketsTable.Select(string.Format("{0} like '%{1}%'", DatabaseObjects.Columns.TicketStageActionUsers, request.UserID ?? currentUser.Id));
        //                }

        //                //Exclude SVC ticket having Stage of type Assigned.
        //                if (request.ModuleName == "SVC" && dr.Length > 0)
        //                {
        //                    LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
        //                    LifeCycleStage assignedStage = null;
        //                    if (lifeCycle != null)
        //                        assignedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageType == StageType.Assigned.ToString());
        //                    if (assignedStage != null)
        //                    {
        //                        dr = dr.CopyToDataTable().Select(string.Format("{0}<>{1}", DatabaseObjects.Columns.StageStep, assignedStage.StageStep));
        //                    }
        //                }

        //                if (dr.Length > 0)
        //                {
        //                    Tab tab = new Tab(FilterTab.waitingonme, FilterTab.waitingonme);  // request.Tabs.Find(x => x == FilterTab.waitingonme);
        //                    statistics.TabCounts[tab.Name] = dr.Length;

        //                    if (request.CurrentTab == tab.Name)
        //                    {
        //                        statistics.CurrentTabCount = dr.Length;
        //                        statistics.ResultedData = dr.CopyToDataTable();
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    #endregion

        //    #region Open Tickets
        //    if ((request.Tabs.Exists(x => x == FilterTab.allopentickets) || request.Tabs.Exists(x => x == "OpenTickets")) && openTicketsTable != null)
        //    {
        //        if (openTicketsTable != null)
        //        {
        //            Tab tab = new Tab(FilterTab.allopentickets, FilterTab.allopentickets); // request.Tabs.Find(x => x.Name == FilterTab.allopentickets);
        //            bool loadAllOpen = true;
        //            if (request.Tabs.Exists(x => x == FilterTab.allresolvedtickets))
        //            {
        //                LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault();
        //                LifeCycleStage resolvedStage = null;
        //                if (lifeCycle != null)
        //                    resolvedStage = lifeCycle.Stages.FirstOrDefault(x => (x.StageTitle == StageType.Resolved.ToString() || x.StageType == StageType.Resolved.ToString()));

        //                if (resolvedStage != null)
        //                {
        //                    loadAllOpen = false;
        //                    DataRow[] nonResolvedTickets = openTicketsTable.Select(string.Format("{0} < {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));
        //                    if (nonResolvedTickets.Length > 0)
        //                    {
        //                        statistics.TabCounts[tab.Name] = nonResolvedTickets.Length;
        //                        if (request.CurrentTab == tab.Name)
        //                        {
        //                            statistics.CurrentTabCount = nonResolvedTickets.Length;
        //                            statistics.ResultedData = nonResolvedTickets.CopyToDataTable();
        //                        }
        //                    }
        //                }
        //            }

        //            if (loadAllOpen)
        //            {
        //                statistics.TabCounts[tab.Name] = openTicketsTable.Rows.Count;
        //                if (request.CurrentTab == tab.Name)
        //                {
        //                    statistics.CurrentTabCount = openTicketsTable.Rows.Count;
        //                    statistics.ResultedData = openTicketsTable;
        //                }
        //                Tab tab2 = new Tab("OpenTickets", "OpenTickets");
        //                statistics.TabCounts[tab2.Name] = openTicketsTable.Rows.Count;

        //                if (request.CurrentTab == tab2.Name)
        //                {
        //                    statistics.CurrentTabCount = openTicketsTable.Rows.Count;
        //                    statistics.ResultedData = openTicketsTable;
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region My Open Tickets
        //    if (request.Tabs.Exists(x => x == FilterTab.myopentickets) && openTicketsTable != null)
        //    {
        //        //load from database if module envent receiver is not enabled which load data is module user statistic list
        //        Tab tab = new Tab(FilterTab.myopentickets, FilterTab.myopentickets); // request.Tabs.Find(x => x == FilterTab.myopentickets);
        //        if (!module.EnableEventReceivers)
        //        {
        //            ModuleStatisticResponse rs = GetMyOpenTicketsFromDatabase(request, module);
        //            if (rs != null)
        //            {
        //                statistics.TabCounts[tab.Name] = rs.CurrentTabCount;
        //                if (request.CurrentTab == tab.Name)
        //                {
        //                    statistics.CurrentTabCount = rs.CurrentTabCount;
        //                    if (rs.ResultedData != null)
        //                    {
        //                        statistics.ResultedData = rs.ResultedData;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (moduleUserStatisticsListTable != null)
        //            {
        //                string selectQuery = string.Format("{0} = '{1}' and {2} = '{3}' and {4} is not null and {4} <> ''", DatabaseObjects.Columns.ModuleName, module.ModuleName, DatabaseObjects.Columns.TicketUser, currentUserNdGroup.First(), DatabaseObjects.Columns.TicketId);
        //                DataRow[] myModuleUserStatisticsRows = moduleUserStatisticsListTable.Select(selectQuery);


        //                DataTable moduleUserStatisticsTable = new DataTable();
        //                if (myModuleUserStatisticsRows != null && myModuleUserStatisticsRows.Length >= 1)
        //                {
        //                    //removew hidden ticket from statistics
        //                    if (request.HideTickets != null && request.HideTickets.Count > 0)
        //                    {
        //                        // Note Requester renamed to Requestor, so handle both
        //                        DataRow[] hideUserStatisticsRows = myModuleUserStatisticsRows.Where(x => (x.Field<string>(DatabaseObjects.Columns.UserRole) == "Initiator" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requestor" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requester")
        //                                                                                            && request.HideTickets.Exists(y => y == x.Field<string>(DatabaseObjects.Columns.TicketId))).ToArray();

        //                        myModuleUserStatisticsRows = myModuleUserStatisticsRows.Except(hideUserStatisticsRows).ToArray();
        //                    }

        //                    if (myModuleUserStatisticsRows.Length > 0)
        //                    {
        //                        //Get unique ticket from statistics
        //                        DataTable myOpenTickets = myModuleUserStatisticsRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
        //                        var openTickets = from op in openTicketsTable.AsEnumerable()
        //                                          join st in myOpenTickets.AsEnumerable() on op.Field<string>(DatabaseObjects.Columns.TicketId) equals st.Field<string>(DatabaseObjects.Columns.TicketId)
        //                                          select op;
        //                        statistics.TabCounts[tab.Name] = openTickets.Count();

        //                        if (request.CurrentTab == tab.Name)
        //                        {
        //                            statistics.CurrentTabCount = myOpenTickets.Rows.Count;

        //                            if (openTickets.Count() > 0)
        //                            {
        //                                statistics.ResultedData = openTickets.CopyToDataTable();
        //                                statistics.TabCounts[tab.Name] = openTickets.Count();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region My Group tickets
        //    if (request.Tabs.Exists(x => x == FilterTab.mygrouptickets) && openTicketsTable != null)
        //    {
        //        if (!module.EnableEventReceivers)
        //        {
        //            ModuleStatisticResponse rs = GetMyGroupTicketsFromDatabase(request, module);
        //            if (rs != null)
        //            {
        //                Tab tab = new Tab(FilterTab.mygrouptickets, FilterTab.mygrouptickets);
        //                statistics.TabCounts[tab.Name] = rs.CurrentTabCount;
        //                if (request.CurrentTab == tab.Name)
        //                {
        //                    statistics.CurrentTabCount = rs.CurrentTabCount;
        //                    if (rs.ResultedData != null)
        //                    {
        //                        statistics.ResultedData = rs.ResultedData;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (moduleUserStatisticsListTable != null)
        //            {
        //                if (currentUserNdGroup.Count > 1)
        //                {
        //                    string selectQuery = string.Format("{0} = '{1}' and {2} in ({3})", DatabaseObjects.Columns.ModuleName, module.ModuleName, DatabaseObjects.Columns.TicketUser, string.Join(",", currentUserNdGroup.Skip(1).Select(x => string.Format("'{0}'", x)).ToArray()));

        //                    DataRow[] myModuleUserStatisticsRows = moduleUserStatisticsListTable.Select(selectQuery);

        //                    DataTable moduleUserStatisticsTable = new DataTable();
        //                    if (myModuleUserStatisticsRows != null && myModuleUserStatisticsRows.Length >= 1)
        //                    {
        //                        //removew hidden ticket from statistics
        //                        if (request.HideTickets != null && request.HideTickets.Count > 0)
        //                        {
        //                            // Note Requester renamed to Requestor, so handle both
        //                            DataRow[] hideUserStatisticsRows = myModuleUserStatisticsRows.Where(x => (x.Field<string>(DatabaseObjects.Columns.UserRole) == "Initiator" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requestor" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requester")
        //                                                                                                && request.HideTickets.Exists(y => y == x.Field<string>(DatabaseObjects.Columns.TicketId))).ToArray();

        //                            myModuleUserStatisticsRows = myModuleUserStatisticsRows.Except(hideUserStatisticsRows).ToArray();
        //                        }

        //                        if (myModuleUserStatisticsRows.Length > 0)
        //                        {
        //                            //Get unique ticket from statistics
        //                            DataTable myOpenTickets = myModuleUserStatisticsRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
        //                            Tab tab = new Tab(FilterTab.mygrouptickets, FilterTab.mygrouptickets);
        //                            statistics.TabCounts[tab.Name] = myOpenTickets.Rows.Count;

        //                            if (request.CurrentTab == tab.Name)
        //                            {
        //                                statistics.CurrentTabCount = myOpenTickets.Rows.Count;

        //                                var openTickets = from op in openTicketsTable.AsEnumerable()
        //                                                  join st in myOpenTickets.AsEnumerable() on op.Field<string>(DatabaseObjects.Columns.TicketId) equals st.Field<string>(DatabaseObjects.Columns.TicketId)
        //                                                  select op;

        //                                if (openTickets.Count() > 0)
        //                                {
        //                                    statistics.ResultedData = openTickets.CopyToDataTable();
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region My Department
        //    if (request.Tabs.Exists(x => x == FilterTab.departmentticket) && openTicketsTable != null)
        //    {
        //        DataRow[] departmentTicketRow = new DataRow[0];
        //        if (module.ModuleName == "NPR" || module.ModuleName == "PMM" || module.ModuleName == "TSK")
        //        {
        //            openTicketsTable.Columns.Add("TicketBeneficiaries1");
        //            openTicketsTable.Columns["TicketBeneficiaries1"].Expression = string.Format("'{0}'+{1}+'{0}'", Constants.Separator, DatabaseObjects.Columns.TicketBeneficiaries);
        //            if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketBeneficiaries))
        //            {
        //                departmentTicketRow = openTicketsTable.Select(string.Format("{0} like '%{2}{1}{2}%'", "TicketBeneficiaries1", currentUser.Department, Constants.Separator));
        //            }
        //            openTicketsTable.Columns.Remove("TicketBeneficiaries1");
        //        }
        //        else
        //        {
        //            if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.DepartmentLookup))
        //            {
        //                departmentTicketRow = openTicketsTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.DepartmentLookup, string.IsNullOrEmpty(currentUser.Department) ? "0" : currentUser.Department));
        //            }
        //        }

        //        if (departmentTicketRow.Length > 0)
        //        {
        //            Tab tab = new Tab("departmentticket", "departmentticket");    // request.Tabs.Find(x => x.Name == "departmentticket");
        //            statistics.TabCounts[tab.Name] = departmentTicketRow.Length;
        //            if (statistics.CurrentTab == tab.Name)
        //            {
        //                statistics.CurrentTabCount = departmentTicketRow.Length;
        //                statistics.ResultedData = departmentTicketRow.CopyToDataTable();
        //            }
        //        }
        //    }
        //    #endregion

        //    #region Resolved Tickets
        //    if (request.Tabs.Exists(x => x == FilterTab.allresolvedtickets) && openTicketsTable != null)
        //    {
        //        LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault();
        //        LifeCycleStage resolvedStage = null;
        //        if (lifeCycle != null)
        //            resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTitle == StageType.Resolved.ToString() || x.StageType == StageType.Resolved.ToString());
        //        // Added condition above, for CPR Under Construction/ Resolved tab

        //        if (resolvedStage != null)
        //        {
        //            DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));
        //            if (resolvedRows.Length > 0)
        //            {
        //                Tab tab = new Tab(FilterTab.allresolvedtickets, FilterTab.allresolvedtickets);  // request.Tabs.Find(x => x == FilterTab.allresolvedtickets );
        //                statistics.TabCounts[tab.Name] = resolvedRows.Length;

        //                if (request.CurrentTab == tab.Name || request.CurrentTab == tab.Text)
        //                {
        //                    statistics.CurrentTabCount = resolvedRows.Length;
        //                    statistics.ResultedData = resolvedRows.CopyToDataTable();
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    #region All Tickets
        //    if (request.Tabs.Exists(x => x.Equals(FilterTab.alltickets, StringComparison.InvariantCultureIgnoreCase) && openTicketsTable != null))
        //    {
        //        Tab tab = new Tab(FilterTab.alltickets, FilterTab.alltickets);  // request.Tabs.Find(x => x == FilterTab.alltickets);
        //        //Tab tab = new Tab("AllTickets", "AllTickets");  
        //        statistics.TabCounts[tab.Name] = openTicketsTable.Rows.Count;
        //        if (request.CurrentTab == tab.Name)
        //            if (request.CurrentTab.Equals(tab.Name, StringComparison.InvariantCultureIgnoreCase))
        //            {
        //                statistics.CurrentTabCount = openTicketsTable.Rows.Count;
        //                statistics.ResultedData = openTicketsTable;
        //            }
        //    }
        //    #endregion

        //    #region Close Tickets
        //    if ((request.Tabs.Exists(x => x == FilterTab.allclosedtickets) || request.Tabs.Exists(x => x == "CloseTickets")) && closeTicketTable != null)
        //    {
        //        Tab tab = new Tab(FilterTab.allclosedtickets, FilterTab.allclosedtickets); // request.Tabs.Find(x => x == FilterTab.allclosedtickets);
        //        statistics.TabCounts[tab.Name] = closeTicketTable.Rows.Count;

        //        if (request.CurrentTab == tab.Name)
        //        {
        //            statistics.CurrentTabCount = closeTicketTable.Rows.Count;
        //            statistics.ResultedData = closeTicketTable;
        //        }

        //        Tab tab2 = new Tab("CloseTickets", "CloseTickets");
        //        statistics.TabCounts[tab2.Name] = closeTicketTable.Rows.Count;

        //        if (request.CurrentTab == tab2.Name)
        //        {
        //            statistics.CurrentTabCount = closeTicketTable.Rows.Count;
        //            statistics.ResultedData = closeTicketTable;
        //        }
        //    }
        //    #endregion

        //    #region My Close Tickets
        //    if (request.Tabs.Exists(x => x == "myclosedtickets"))
        //    {
        //        ModuleStatisticResponse response = GetModuleStatisticsCalculatorForMyClosed(request, module);
        //        Tab tab = new Tab(FilterTab.myclosedtickets, FilterTab.myclosedtickets);
        //        statistics.TabCounts[tab.Name] = response.CurrentTabCount;
        //        if (request.CurrentTab == tab.Name)
        //        {
        //            statistics.CurrentTabCount = response.CurrentTabCount;
        //            statistics.ResultedData = response.ResultedData;
        //        }
        //    }
        //    #endregion

        //    return statistics;
        //}

        private ModuleStatisticResponse GetCustomFilterTabData(ModuleStatisticRequest request, bool includeClosed = false)
        {
            //return if statistic is null or modulename is not exist
            if (request == null || request.ModuleName.Trim() == string.Empty)
            {
                return null;
            }

            // Get module detail
            UGITModule module = _moduleViewManager.LoadByName(request.ModuleName, true);

            if (module == null)
                return null;

            UserProfile currentUser = User;

            string userName = currentUser != null ? currentUser.Name : string.Empty;

            List<string> currentUserNdGroup = new List<string>();
            if (currentUser != null)
            {
                currentUserNdGroup.Add(request.UserID ?? currentUser.Id);

                var roles = UserManager.GetUserRoles(request.UserID ?? currentUser.Id);
                foreach (var group in roles)
                {
                    currentUserNdGroup.Add(group.Id);
                }
            }

            ModuleStatisticResponse statistics = new ModuleStatisticResponse();
            statistics.TabCounts = new Dictionary<string, int>();
            if (request.Tabs != null && request.Tabs.Count > 0)
            {
                foreach (string tab in request.Tabs)
                {
                    statistics.TabCounts.Add(tab, 0);
                }
            }
            statistics.CurrentTab = request.CurrentTab;
            statistics.CurrentTabCount = 0;
            statistics.ResultedData = null;
            statistics.ModuleName = request.ModuleName;

            // Gets data from cache
            DataTable openTicketsTable = null;
            DataTable closeTicketTable = null;
            //DataTable allTicketTable = null;
            DataTable moduleUserStatisticsListTable = null;
            bool onHoldTabVisible = request.Tabs.Contains(FilterTab.OnHold);

            if (request.Tabs.Exists(x => x == FilterTab.allclosedtickets) || request.Tabs.Exists(x => x == "myclosed")  || request.Tabs.Exists(x => x == "CloseTickets"))
            {
                closeTicketTable = _ticketManager.GetClosedTickets(module);
            }
            if (request.Tabs.Exists(x => x == FilterTab.myopentickets) || request.Tabs.Exists(x => x == FilterTab.mygrouptickets))
            {
                if (!string.IsNullOrEmpty(request.ModuleName))
                {
                    moduleUserStatisticsListTable = (DataTable)CacheHelper<object>.Get($"ModuleUserStatistics{_context.TenantID}", _context.TenantID);
                    if (moduleUserStatisticsListTable == null)
                    {
                        moduleUserStatisticsListTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                        CacheHelper<object>.AddOrUpdate($"ModuleUserStatistics{_context.TenantID}", _context.TenantID, moduleUserStatisticsListTable);
                    }
                    else
                    {
                        DataView dv = new DataView(moduleUserStatisticsListTable);
                        dv.RowFilter = $"{DatabaseObjects.Columns.ModuleNameLookup} = '{request.ModuleName}'"; // query example = "id = 10"
                        moduleUserStatisticsListTable = dv.ToTable();
                    }
                }
                else
                {
                    //moduleUserStatisticsListTable = UGITUtility.ToDataTable<ModuleUserStatistic>(_moduleUserStatsManager.GetModuleUserStatistics());
                    moduleUserStatisticsListTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");

                }
            }

            openTicketsTable = _ticketManager.GetOpenTickets(module, request);

            //statistics.ResultedData = openTicketsTable;
            #region UnAssigned
            if (request.Tabs.Exists(x => x.Equals(FilterTab.unassigned, StringComparison.InvariantCultureIgnoreCase)))
            {
                DataTable unassingedData = _ticketManager.GetUnassignedTickets(request.ModuleName);

                if (unassingedData.Rows.Count > 0 && onHoldTabVisible && openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                {
                    DataRow[] dr = unassingedData.AsEnumerable().Where(x => x.IsNull(DatabaseObjects.Columns.TicketOnHold)
                                        || string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.TicketOnHold]))
                                        || Convert.ToInt32(x[DatabaseObjects.Columns.TicketOnHold]) == 0).ToArray();

                    if (dr != null && dr.Length > 0)
                        unassingedData = dr.CopyToDataTable();
                }

                Tab tab = new Tab(FilterTab.unassigned, FilterTab.unassigned);
                if (unassingedData != null)
                {
                    statistics.TabCounts[tab.Name] = unassingedData.Rows.Count;
                }

                if (request.CurrentTab == tab.Name)
                {
                    statistics.CurrentTabCount = unassingedData.Rows.Count;
                    statistics.ResultedData = unassingedData;
                }
            }
            #endregion

            #region Waiting On Me

            if (request.Tabs.Find(x => x == FilterTab.waitingonme) != null && openTicketsTable != null)
            {
                if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
                {

                    if (!openTicketsTable.Columns.Contains("StageActionUser1"))
                        openTicketsTable.Columns.Add("StageActionUser1");
                    openTicketsTable.Columns["StageActionUser1"].Expression = string.Format("'{0}'", DatabaseObjects.Columns.TicketStageActionUsers);
                    if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
                    {
                        DataRow[] dr = new DataRow[0];
                        if (module.WaitingOnMeIncludesGroups)
                        {
                            string query = string.Join(" or ", currentUserNdGroup.Select(x => string.Format("{0} like '%{1}%'", DatabaseObjects.Columns.TicketStageActionUsers, x, Constants.Separator5)));
                            dr = openTicketsTable.Select(query);
                        }
                        else
                        {
                            dr = openTicketsTable.Select(string.Format("{0} like '%{1}%'", DatabaseObjects.Columns.TicketStageActionUsers, request.UserID ?? currentUser.Id));
                        }

                        LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                        LifeCycleStage resolvedStage = null;

                        if (lifeCycle != null)
                            resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Resolved.ToString());

                        // Exclude resolved tickets
                        if (module.WaitingOnMeExcludesResolved && resolvedStage != null && dr.Length > 0)
                            dr = dr.CopyToDataTable().Select(string.Format("{0} < {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));

                        if (dr != null)
                        {
                            //Take out configuration based condition
                            if (onHoldTabVisible && openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketOnHold)
                                && !objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.MyQueueShowOnHold))
                            {
                                dr = dr.AsEnumerable().Where(x => x.IsNull(DatabaseObjects.Columns.TicketOnHold) ||
                                                                  string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.TicketOnHold])) ||
                                                                  Convert.ToInt32(x[DatabaseObjects.Columns.TicketOnHold]) == 0).ToArray();
                            }

                            Tab tab = new Tab(FilterTab.waitingonme, FilterTab.waitingonme);  // request.Tabs.Find(x => x == FilterTab.waitingonme);
                            statistics.TabCounts[tab.Name] = dr.Length;

                            if (request.CurrentTab == tab.Name)
                            {
                                statistics.CurrentTabCount = dr.Length;
                                if (dr.Count() > 0)
                                    statistics.ResultedData = dr.CopyToDataTable();
                                else
                                    statistics.ResultedData = null;
                            }
                        }
                    }
                }

            }
            #endregion

            #region Open Tickets
            if ((request.Tabs.Exists(x => x == FilterTab.allopentickets) || request.Tabs.Exists(x => x == "OpenTickets")) && openTicketsTable != null)
            {
                if (openTicketsTable != null)
                {
                    Tab tab = new Tab(FilterTab.allopentickets, FilterTab.allopentickets); // request.Tabs.Find(x => x.Name == FilterTab.allopentickets);
                    bool loadAllOpen = true;
                    if (request.Tabs.Exists(x => x == FilterTab.allresolvedtickets))
                    {
                        LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault();
                        LifeCycleStage resolvedStage = null;
                        if (lifeCycle != null)
                            resolvedStage = lifeCycle.Stages.FirstOrDefault(x => (x.StageTitle == StageType.Resolved.ToString() || x.StageTypeChoice == StageType.Resolved.ToString()));

                        if (resolvedStage != null)
                        {
                            loadAllOpen = false;
                            DataRow[] nonResolvedTickets = openTicketsTable.Select(string.Format("{0} < {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));
                            if (nonResolvedTickets != null && nonResolvedTickets.Length > 0)
                            {
                                if (onHoldTabVisible && !objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.OpenTicketsShowOnHold)
                                    && nonResolvedTickets.CopyToDataTable().Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                                {
                                    nonResolvedTickets = nonResolvedTickets.Where(x => x.IsNull(DatabaseObjects.Columns.TicketOnHold)
                                        || string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.TicketOnHold]))
                                        || Convert.ToInt32(x[DatabaseObjects.Columns.TicketOnHold]) == 0).ToArray();
                                }

                                if (nonResolvedTickets != null && nonResolvedTickets.Length > 0)
                                {
                                    statistics.TabCounts[tab.Name] = nonResolvedTickets.Length;
                                    if (request.CurrentTab == tab.Name)
                                    {
                                        statistics.CurrentTabCount = nonResolvedTickets.Length;
                                        statistics.ResultedData = nonResolvedTickets.CopyToDataTable();
                                    }
                                }
                                else
                                {
                                    statistics.TabCounts[tab.Name] = 0;
                                    if (request.CurrentTab == tab.Name)
                                    {
                                        statistics.CurrentTabCount = 0;
                                        statistics.ResultedData = null;
                                    }
                                }
                            }
                        }
                    }

                    if (loadAllOpen)
                    {
                        DataTable outputTable = openTicketsTable;

                        // Exclude On-Hold tickets if On-Hold tab visible AND not configured to show all tickets
                        DataRow[] drOpen = null;
                        if (onHoldTabVisible && !objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.OpenTicketsShowOnHold) &&
                            openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                        {
                            drOpen = openTicketsTable.AsEnumerable().Where(x => x.IsNull(DatabaseObjects.Columns.TicketOnHold) ||
                                                                           string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.TicketOnHold])) ||
                                                                           Convert.ToInt32(x[DatabaseObjects.Columns.TicketOnHold]) == 0).ToArray();
                            if (drOpen != null && drOpen.Length > 0)
                                outputTable = drOpen.CopyToDataTable();
                        }


                        statistics.TabCounts[tab.Name] = outputTable.Rows.Count;
                        if (request.CurrentTab == tab.Name)
                        {
                            statistics.CurrentTabCount = outputTable.Rows.Count;
                            statistics.ResultedData = outputTable;
                        }
                        Tab tab2 = new Tab("OpenTickets", "OpenTickets");
                        statistics.TabCounts[tab2.Name] = outputTable.Rows.Count;

                        if (request.CurrentTab == tab2.Name)
                        {
                            statistics.CurrentTabCount = outputTable.Rows.Count;
                            statistics.ResultedData = outputTable;
                        }
                    }
                }
            }
            #endregion

            #region My Open Tickets
            if (request.Tabs.Exists(x => x == FilterTab.myopentickets) && openTicketsTable != null)
            {
                //load from database if module envent receiver is not enabled which load data is module user statistic list
                Tab tab = new Tab(FilterTab.myopentickets, FilterTab.myopentickets);
                if (!module.EnableEventReceivers)
                {
                    ModuleStatisticResponse rs = GetMyOpenTicketsFromDatabase(request, module);
                    if (rs != null)
                    {
                        // Exclude On-Hold tickets if OnHold tab visible AND not configured to show anyway
                        if (onHoldTabVisible && !objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.MyTicketsShowOnHold) &&
                            rs.ResultedData.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                        {
                            DataRow[] drColl = null;
                            drColl = rs.ResultedData.AsEnumerable().Where(x => x.IsNull(DatabaseObjects.Columns.TicketOnHold) ||
                                                                               string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.TicketOnHold])) ||
                                                                               Convert.ToInt32(x[DatabaseObjects.Columns.TicketOnHold]) == 0).ToArray();
                            if (drColl != null && drColl.Length > 0)
                            {
                                rs.ResultedData = drColl.CopyToDataTable();
                                rs.CurrentTabCount = drColl.Length;
                            }
                        }

                        statistics.TabCounts[tab.Name] = rs.CurrentTabCount;
                        if (request.CurrentTab == tab.Name)
                        {
                            statistics.CurrentTabCount = rs.CurrentTabCount;
                            if (rs.ResultedData != null)
                            {
                                statistics.ResultedData = rs.ResultedData;
                            }
                        }
                    }
                }
                else
                {
                    if (moduleUserStatisticsListTable != null)
                    {
                        var userGroup = "";
                        for (int i = 0; i < currentUserNdGroup.Count; i++)
                        {
                            if (i > 0)
                                userGroup += ",";

                            userGroup += $"'{currentUserNdGroup[i]}'";
                        }

                        //string selectQuery = string.Format("{0} = '{1}' and {2} = '{3}' and {4} is not null and {4} <> ''", DatabaseObjects.Columns.ModuleName, module.ModuleName, DatabaseObjects.Columns.TicketUser, currentUserNdGroup.First(), DatabaseObjects.Columns.TicketId);
                        string selectQuery = string.Format("{0} = '{1}' and {2} IN ({3}) and {4} is not null and {4} <> ''", DatabaseObjects.Columns.ModuleNameLookup, module.ModuleName, DatabaseObjects.Columns.TicketUser, userGroup, DatabaseObjects.Columns.TicketId);

                        DataRow[] myModuleUserStatisticsRows = moduleUserStatisticsListTable.Select(selectQuery);

                        DataTable moduleUserStatisticsTable = new DataTable();
                        if (myModuleUserStatisticsRows != null && myModuleUserStatisticsRows.Length >= 1)
                        {
                            //removew hidden ticket from statistics
                            if (request.HideTickets != null && request.HideTickets.Count > 0)
                            {
                                // Note Requester renamed to Requestor, so handle both
                                DataRow[] hideUserStatisticsRows = myModuleUserStatisticsRows.Where(x => (x.Field<string>(DatabaseObjects.Columns.UserRole) == "Initiator" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requestor" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requester")
                                                                                                    && request.HideTickets.Exists(y => y == x.Field<string>(DatabaseObjects.Columns.TicketId))).ToArray();

                                myModuleUserStatisticsRows = myModuleUserStatisticsRows.Except(hideUserStatisticsRows).ToArray();
                            }

                            if (myModuleUserStatisticsRows.Length > 0)
                            {
                                //Get unique ticket from statistics
                                DataTable myOpenTickets = myModuleUserStatisticsRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
                                var openTickets = from op in openTicketsTable.AsEnumerable()
                                                  join st in myOpenTickets.AsEnumerable() on op.Field<string>(DatabaseObjects.Columns.TicketId) equals st.Field<string>(DatabaseObjects.Columns.TicketId)
                                                  select op;
                                statistics.TabCounts[tab.Name] = openTickets.Count();

                                if (request.CurrentTab == tab.Name)
                                {
                                    statistics.CurrentTabCount = myOpenTickets.Rows.Count;

                                    if (openTickets.Count() > 0)
                                    {
                                        // Exclude On-Hold tickets if OnHold tab visible AND not configured to show anyway
                                        DataRow[] dropen = null;
                                        if (onHoldTabVisible && !objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.MyTicketsShowOnHold) &&
                                            openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                                        {
                                            dropen = openTickets.AsEnumerable().Where(x => x.IsNull(DatabaseObjects.Columns.TicketOnHold) ||
                                                                                          string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.TicketOnHold])) ||
                                                                                          Convert.ToInt32(x[DatabaseObjects.Columns.TicketOnHold]) == 0).ToArray();
                                            if (dropen != null && dropen.Length > 0)
                                            {
                                                openTickets = dropen.AsEnumerable();
                                            }
                                            else
                                            {
                                                openTickets = null;
                                            }
                                        }

                                        statistics.ResultedData = openTickets != null ? openTickets.CopyToDataTable() : null;
                                        statistics.TabCounts[tab.Name] = openTickets != null ? openTickets.Count() : 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (request.CurrentTab == tab.Name)
                            {
                                statistics.CurrentTabCount = 0;

                                statistics.ResultedData = null;
                                statistics.TabCounts[tab.Name] = 0;

                            }
                        }
                    }
                }
            }
            #endregion

            #region My Group tickets
            if (request.Tabs.Exists(x => x == FilterTab.mygrouptickets) && openTicketsTable != null)
            {
                if (!module.EnableEventReceivers)
                {
                    ModuleStatisticResponse rs = GetMyGroupTicketsFromDatabase(request, module);
                    if (rs != null)
                    {
                        Tab tab = new Tab(FilterTab.mygrouptickets, FilterTab.mygrouptickets);
                        statistics.TabCounts[tab.Name] = rs.CurrentTabCount;
                        if (request.CurrentTab == tab.Name)
                        {
                            statistics.CurrentTabCount = rs.CurrentTabCount;
                            if (rs.ResultedData != null)
                            {
                                statistics.ResultedData = rs.ResultedData;
                            }
                        }
                    }
                }
                else
                {
                    if (moduleUserStatisticsListTable != null)
                    {
                        if (currentUserNdGroup.Count > 1)
                        {
                            string selectQuery = string.Format("{0} = '{1}' and {2} in ({3})", DatabaseObjects.Columns.ModuleNameLookup, module.ModuleName, DatabaseObjects.Columns.TicketUser, string.Join(",", currentUserNdGroup.Skip(1).Select(x => string.Format("'{0}'", x)).ToArray()));

                            DataRow[] myModuleUserStatisticsRows = moduleUserStatisticsListTable.Select(selectQuery);

                            DataTable moduleUserStatisticsTable = new DataTable();
                            if (myModuleUserStatisticsRows != null && myModuleUserStatisticsRows.Length >= 1)
                            {
                                //removew hidden ticket from statistics
                                if (request.HideTickets != null && request.HideTickets.Count > 0)
                                {
                                    // Note Requester renamed to Requestor, so handle both
                                    DataRow[] hideUserStatisticsRows = myModuleUserStatisticsRows.Where(x => (x.Field<string>(DatabaseObjects.Columns.UserRole) == "Initiator" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requestor" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requester")
                                                                                                        && request.HideTickets.Exists(y => y == x.Field<string>(DatabaseObjects.Columns.TicketId))).ToArray();

                                    myModuleUserStatisticsRows = myModuleUserStatisticsRows.Except(hideUserStatisticsRows).ToArray();
                                }

                                if (myModuleUserStatisticsRows.Length > 0)
                                {
                                    //Get unique ticket from statistics
                                    DataTable myOpenTickets = myModuleUserStatisticsRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
                                    Tab tab = new Tab(FilterTab.mygrouptickets, FilterTab.mygrouptickets);
                                    statistics.TabCounts[tab.Name] = myOpenTickets.Rows.Count;

                                    if (request.CurrentTab == tab.Name)
                                    {
                                        statistics.CurrentTabCount = myOpenTickets.Rows.Count;

                                        var openTickets = from op in openTicketsTable.AsEnumerable()
                                                          join st in myOpenTickets.AsEnumerable() on op.Field<string>(DatabaseObjects.Columns.TicketId) equals st.Field<string>(DatabaseObjects.Columns.TicketId)
                                                          select op;

                                        if (openTickets.Count() > 0)
                                        {
                                            statistics.ResultedData = openTickets.CopyToDataTable();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region My Department
            if (request.Tabs.Exists(x => x == FilterTab.departmentticket) && openTicketsTable != null)
            {
                DataRow[] departmentTicketRow = new DataRow[0];
                if (module.ModuleName == "NPR" || module.ModuleName == "PMM" || module.ModuleName == "TSK")
                {
                    openTicketsTable.Columns.Add("TicketBeneficiaries1");
                    //openTicketsTable.Columns["TicketBeneficiaries1"].Expression = string.Format("'{0}'+{1}+'{0}'", Constants.Separator, DatabaseObjects.Columns.TicketBeneficiaries);
                    if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketBeneficiaries))
                    {
                        openTicketsTable.Columns["TicketBeneficiaries1"].Expression = string.Format("'{0}'+{1}+'{0}'", Constants.Separator, DatabaseObjects.Columns.TicketBeneficiaries);
                        departmentTicketRow = openTicketsTable.Select(string.Format("{0} like '%{2}{1}{2}%'", "TicketBeneficiaries1", currentUser.Department, Constants.Separator));
                    }
                    openTicketsTable.Columns.Remove("TicketBeneficiaries1");
                }
                else
                {
                    if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.DepartmentLookup))
                    {
                        departmentTicketRow = openTicketsTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.DepartmentLookup+"$Id", string.IsNullOrEmpty(currentUser.Department) ? "0" : currentUser.Department));
                        if (departmentTicketRow != null && departmentTicketRow.Count() > 0)
                        {
                            Tab tab = new Tab("departmentticket", "departmentticket");    // request.Tabs.Find(x => x.Name == "departmentticket");
                            statistics.TabCounts[tab.Name] = departmentTicketRow.Length;
                            statistics.ResultedData = departmentTicketRow.CopyToDataTable();

                            if (statistics.CurrentTab == tab.Name)
                                statistics.CurrentTabCount = departmentTicketRow.Length;
                        }
                        else
                            statistics.ResultedData = null;
                    }
                }

                
            }
            #endregion

            #region Resolved Tickets
            if (request.Tabs.Exists(x => x == FilterTab.allresolvedtickets) && openTicketsTable != null)
            {
                LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault();
                LifeCycleStage resolvedStage = null;
                if (lifeCycle != null)
                    resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTitle == StageType.Resolved.ToString() || x.StageTypeChoice == StageType.Resolved.ToString());
                // Added condition above, for CPR Under Construction/ Resolved tab

                if (resolvedStage != null)
                {
                    DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0} >= {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));
                    if (resolvedRows != null)
                    {
                        if (onHoldTabVisible && openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketOnHold) 
                            && !objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.ResolvedTicketsShowOnHold))
                        {
                            resolvedRows = resolvedRows.AsEnumerable().Where(x => x.IsNull(DatabaseObjects.Columns.TicketOnHold) ||
                                                                                          string.IsNullOrEmpty(Convert.ToString(x[DatabaseObjects.Columns.TicketOnHold])) ||
                                                                                          Convert.ToInt32(x[DatabaseObjects.Columns.TicketOnHold]) == 0).ToArray();
                        }
                        if (resolvedRows != null && resolvedRows.Count() > 0)
                        {
                            Tab tab = new Tab(FilterTab.allresolvedtickets, FilterTab.allresolvedtickets);  // request.Tabs.Find(x => x == FilterTab.allresolvedtickets );
                            statistics.TabCounts[tab.Name] = resolvedRows.Length;

                            if (request.CurrentTab == tab.Name || request.CurrentTab == tab.Text)
                            {
                                statistics.CurrentTabCount = resolvedRows.Length;
                                if (resolvedRows.Count() > 0)
                                    statistics.ResultedData = resolvedRows.CopyToDataTable();
                                else
                                    statistics.ResultedData = null;
                            }
                        }
                    }
                }
            }
            #endregion

            #region All Tickets
            if (request.Tabs.Exists(x => x.Equals(FilterTab.alltickets, StringComparison.InvariantCultureIgnoreCase) && openTicketsTable != null))
            {
                Tab tab = new Tab(FilterTab.alltickets, FilterTab.alltickets);  // request.Tabs.Find(x => x == FilterTab.alltickets);
                //Tab tab = new Tab("AllTickets", "AllTickets");  
                statistics.TabCounts[tab.Name] = openTicketsTable.Rows.Count;
                int allOpenticketCount = openTicketsTable.Rows.Count;

                if (includeClosed == true)
                {
                    closeTicketTable = _ticketManager.GetClosedTickets(module);
                    if (closeTicketTable != null && closeTicketTable.Rows.Count > 0)
                    {
                        statistics.TabCounts[tab.Name] = allOpenticketCount + closeTicketTable.Rows.Count;
                    }
                }

                if (request.CurrentTab.Equals(tab.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    statistics.CurrentTabCount = openTicketsTable.Rows.Count;
                    statistics.ResultedData = openTicketsTable;
                    if (includeClosed && closeTicketTable != null && closeTicketTable.Rows.Count > 0)
                    {
                        statistics.ResultedData.Merge(closeTicketTable);
                    }
                }
            }
            #endregion

            #region Close Tickets

            if (request.Tabs.Exists(x => x == FilterTab.allclosedtickets))
            {
                Tab tab = new Tab(FilterTab.allclosedtickets, FilterTab.allclosedtickets); // request.Tabs.Find(x => x == FilterTab.allclosedtickets);
                statistics.TabCounts[tab.Name] = closeTicketTable?.Rows?.Count > 0 ? closeTicketTable.Rows.Count : 0;

                if (request.CurrentTab == tab.Name)
                {
                    statistics.CurrentTabCount = closeTicketTable?.Rows?.Count > 0 ? closeTicketTable.Rows.Count : 0;
                    statistics.ResultedData = closeTicketTable;
                }
            }
            if (request.Tabs.Exists(x => x == "CloseTickets"))
            {
                Tab tab2 = new Tab("CloseTickets", "CloseTickets");
                statistics.TabCounts[tab2.Name] = closeTicketTable?.Rows?.Count > 0 ? closeTicketTable.Rows.Count : 0;

                if (request.CurrentTab == tab2.Name)
                {
                    statistics.CurrentTabCount = closeTicketTable?.Rows?.Count > 0 ? closeTicketTable.Rows.Count : 0;
                    statistics.ResultedData = closeTicketTable;
                }
            }
            
            #endregion

            #region My Close Tickets
            if (request.Tabs.Exists(x => x == "myclosedtickets"))
            {
                ModuleStatisticResponse response = GetModuleStatisticsCalculatorForMyClosed(request, module);
                Tab tab = new Tab(FilterTab.myclosedtickets, FilterTab.myclosedtickets);
                statistics.TabCounts[tab.Name] = response.CurrentTabCount;
                if (request.CurrentTab == tab.Name)
                {
                    statistics.CurrentTabCount = response.CurrentTabCount;
                    statistics.ResultedData = response.ResultedData;
                }
                if (closeTicketTable != null && closeTicketTable.Rows.Count > 0)
                {
                    if (openTicketsTable == null || openTicketsTable.Rows.Count == 0)
                        statistics.ResultedData = closeTicketTable;
                }
            }
            #endregion

            #region OnHold Tickets
            if (request.Tabs.Contains(FilterTab.OnHold) && openTicketsTable != null && openTicketsTable.Rows.Count > 0)
            {
                DataRow[] dr = new DataRow[0];
                if (openTicketsTable.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                {
                    dr = openTicketsTable.Select(string.Format("{0}=1", DatabaseObjects.Columns.TicketOnHold));
                    if (dr != null)
                    {
                        statistics.TabCounts[FilterTab.OnHold] = dr.Length;

                        if (request.CurrentTab == FilterTab.OnHold)
                        {
                            statistics.CurrentTabCount = dr.Length;
                            if (dr.Length > 0)
                                statistics.ResultedData = dr.CopyToDataTable();
                            else
                                statistics.ResultedData = null;
                        }
                    }
                }
            }
            #endregion

            return statistics;
        }

        private ModuleStatisticResponse GetModuleStatisticsCalculatorForMyClosed(ModuleStatisticRequest request, UGITModule module)
        {
            UserProfile currentUser = User;
            //return if statistic is null or modulename is not exist
            if (request == null || request.ModuleName.Trim() == string.Empty)
                return null;

            ModuleStatisticResponse statistics = new ModuleStatisticResponse();

            DataTable filterTable = null;

            // set the closed ticket count and table.
            List<string> expressions = new List<string>();
            filterTable = _ticketManager.GetClosedTickets(module);
            if (filterTable != null && filterTable.Rows.Count > 0)
            {
                if (filterTable.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    filterTable.DefaultView.Sort = string.Format("{0} desc", DatabaseObjects.Columns.TicketCreationDate);
                    filterTable = filterTable.DefaultView.ToTable();
                }
                else if (filterTable.Columns.Contains(DatabaseObjects.Columns.Created))
                {
                    filterTable.DefaultView.Sort = string.Format("{0} desc", DatabaseObjects.Columns.Created);
                    filterTable = filterTable.DefaultView.ToTable();
                }
            }

            if (filterTable != null)
            {
                string query = "";
                DataRow[] dr = new DataRow[0];
                if (filterTable.Columns.Contains(DatabaseObjects.Columns.TicketRequestor))
                {
                    query = string.Format("{0} like '%{1}%'", DatabaseObjects.Columns.TicketRequestor, request.UserID ?? currentUser.Id);
                    dr = filterTable.Select(query);
                }

                if (dr != null)
                {
                    if (dr.Count() > 0)
                        filterTable = dr.CopyToDataTable();
                    else
                        filterTable = null;
                    statistics.ResultedData = filterTable;
                }

                statistics.CurrentTab = FilterTab.myclosedtickets; //new Tab(FilterTab.myclosedtickets, FilterTab.myclosedtickets);
                statistics.CurrentTabCount = dr.Length;
            }
            return statistics;
        }

        public Dictionary<string, int> LoadAllCount(List<string> tabs, string userid)
        {
            List<UGITModule> listModule = _moduleViewManager.LoadAllModule(); //uGITCache.ModuleConfigCache.LoadAllModules().Select("ModuleType = '" + ModuleType.SMS + "'");
            List<UGITModule> moduleTable = new List<UGITModule>();
            if (listModule.Count() > 0)
            {
                moduleTable = listModule.Where(x => x.ModuleType == ModuleType.SMS && x.EnableModule == true).ToList();
            }
            DataTable moduleUserStatisticsListTable = (DataTable)CacheHelper<object>.Get($"ModuleUserStatistics{_context.TenantID}", _context.TenantID);
            if (moduleUserStatisticsListTable == null)
            {
                moduleUserStatisticsListTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                CacheHelper<object>.AddOrUpdate($"ModuleUserStatistics{_context.TenantID}", _context.TenantID, moduleUserStatisticsListTable);
            }
            ModuleStatisticResponse moduleStatResult;
            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            // mRequest.UserID = User.Id;
            mRequest.Tabs = tabs;
            mRequest.CurrentTab = FilterTab.waitingonme;// new Tab(FilterTab.waitingonme, FilterTab.waitingonme);
            Dictionary<string, int> statistics = new Dictionary<string, int>();
            foreach (string s in tabs)
            {
                statistics.Add(s, 0);
            }
            if (statistics.ContainsKey(FilterTab.myproject))
            {
                List<UGITModule> moduleTableProjectType = new List<UGITModule>();
                moduleTableProjectType = listModule.Where(x => x.ModuleType == ModuleType.Project && x.EnableModule == true).ToList();
                //DataTable moduleUserStatisticsListTable =GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
                foreach (UGITModule module in moduleTableProjectType)
                {
                    //DataTable moduleUserStatisticsListTable = UGITUtility.ToDataTable<ModuleUserStatistic>(_moduleUserStatsManager.GetModuleUserStatistics());
                    if (moduleUserStatisticsListTable != null && moduleUserStatisticsListTable.Rows.Count > 0 && module != null) // added && ugitModule != null, throwing exception if ugitModule is null
                    {
                        DataRow[] myProjectRows = moduleUserStatisticsListTable.Select(DatabaseObjects.Columns.ModuleNameLookup + "='" + module.ModuleName + "'");
                        if (myProjectRows.Length >= 1)
                        {
                            string selectQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.UserName, userid);

                            myProjectRows = myProjectRows.CopyToDataTable().Select(selectQuery);
                            DataTable dt = new DataTable();
                            if (myProjectRows.Length > 0)
                                dt = myProjectRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
                            if (dt.Rows.Count > 0)
                                statistics[FilterTab.myproject] = statistics[FilterTab.myproject] + dt.Rows.Count;  ///statistics.Projects += dt.Rows.Count;
                        }
                    }
                }
            }
            if (statistics.ContainsKey("allopenproject"))
            {
                DataTable mytable = new DataTable();
                List<UGITModule> moduleTableProjectType = new List<UGITModule>();
                moduleTableProjectType = listModule.Where(x => x.ModuleType == ModuleType.Project && x.EnableModule == true).ToList();
                foreach (UGITModule module in moduleTableProjectType)
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allopenproject", ModuleName = module.ModuleName, Status = TicketStatus.Open, UserID = userid };
                    ModuleStatisticResponse statisticsResponse = new ModuleStatisticResponse();
                    statisticsResponse = Load(statRequest, false);
                    if (statisticsResponse.ResultedData != null && statisticsResponse.ResultedData.Rows.Count > 0)
                        mytable.Merge(statisticsResponse.ResultedData, false, MissingSchemaAction.Ignore);
                }
                statistics["allopenproject"] = UGITUtility.StringToInt(mytable?.Rows.Count);
            }
            if (statistics.ContainsKey("myopenopportunities"))
            {
                UGITModule opmModuleObj = listModule.FirstOrDefault(x => x.ModuleName == "OPM");
                if (moduleUserStatisticsListTable != null && moduleUserStatisticsListTable.Rows.Count > 0 && opmModuleObj != null) // added && ugitModule != null, throwing exception if ugitModule is null
                {
                    DataRow[] myOPMRows = moduleUserStatisticsListTable.Select(DatabaseObjects.Columns.ModuleNameLookup + "='" + opmModuleObj.ModuleName + "'");
                    if (myOPMRows.Length >= 1)
                    {
                        string selectQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.UserName, userid);

                        myOPMRows = myOPMRows.CopyToDataTable().Select(selectQuery);
                        DataTable dt = new DataTable();
                        if (myOPMRows.Length > 0)
                            dt = myOPMRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
                        if (dt.Rows.Count > 0)
                            statistics["myopenopportunities"] = dt.Rows.Count;  ///statistics.Projects += dt.Rows.Count;
                    }
                }
            }
            if (statistics.ContainsKey("nprtickets"))
            {
                UGITModule nprModuleObj = listModule.FirstOrDefault(x => x.ModuleName == "NPR");
                if (moduleUserStatisticsListTable != null && moduleUserStatisticsListTable.Rows.Count > 0 && nprModuleObj != null) // added && ugitModule != null, throwing exception if ugitModule is null
                {
                    DataRow[] myOPMRows = moduleUserStatisticsListTable.Select(DatabaseObjects.Columns.ModuleNameLookup + "='" + nprModuleObj.ModuleName + "'");
                    if (myOPMRows.Length >= 1)
                    {
                        string selectQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.UserName, userid);

                        myOPMRows = myOPMRows.CopyToDataTable().Select(selectQuery);
                        DataTable dt = new DataTable();
                        if (myOPMRows.Length > 0)
                            dt = myOPMRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
                        if (dt.Rows.Count > 0)
                            statistics["nprtickets"] = dt.Rows.Count;  ///statistics.Projects += dt.Rows.Count;
                    }
                }
            }
            if (statistics.ContainsKey("allopenopportunities"))
            {
                UGITModule opmModuleObj = listModule.FirstOrDefault(x => x.ModuleName == "OPM");
                DataTable dtOPMOpenTickets = _ticketManager.GetOpenTickets(opmModuleObj);
                if (dtOPMOpenTickets != null)
                    statistics["allopenopportunities"] = UGITUtility.StringToInt(dtOPMOpenTickets.Rows.Count);
            }
            if (statistics.ContainsKey("allopenservices"))
            {
                UGITModule cnsModuleObj = listModule.FirstOrDefault(x => x.ModuleName == "CNS");
                DataTable dtCNSOpenTickets = _ticketManager.GetOpenTickets(cnsModuleObj);
                if (dtCNSOpenTickets != null)
                    statistics["allopenservices"] = UGITUtility.StringToInt(dtCNSOpenTickets.Rows.Count);
            }
            if (statistics.ContainsKey("recentwonopportunity"))
            {
                UGITModule opmModuleObj = listModule.FirstOrDefault(x => x.ModuleName == "OPM");
                DataTable dtOPMOpenTickets = _ticketManager.GetOpenTickets(opmModuleObj);
                if (dtOPMOpenTickets != null)
                {

                }
            }
            if (statistics.ContainsKey("recentlostopportunity"))
            {
                UGITModule opmModuleObj = listModule.FirstOrDefault(x => x.ModuleName == "OPM");
                DataTable dtOPMOpenTickets = _ticketManager.GetOpenTickets(opmModuleObj);
                if (dtOPMOpenTickets != null)
                {

                }
            }
            if (moduleTable != null && moduleTable.Count > 0)
            {
                foreach (UGITModule module in moduleTable)
                {
                    mRequest.ModuleName = module.ModuleName;
                    moduleStatResult = Load(mRequest);
                    foreach (KeyValuePair<string, int> item in moduleStatResult.TabCounts)
                    {
                        if (statistics.ContainsKey(item.Key))
                        {
                            statistics[item.Key] = statistics[item.Key] + item.Value;
                        }
                    }
                }
            }
            if (tabs.Exists(x => x == "mytask"))
            {
                UGITTaskManager taskManager = new UGITTaskManager(_context);
                DataTable dt = taskManager.GetOpenedTasksByUser(_context.CurrentUser.Id, true, new string[] { "NPR", "SVCConfig", "Template" });
                if (dt != null && dt.Rows.Count > 0)
                    statistics["mytask"] = dt.Rows.Count;
            }
            return statistics;
        }

        public Dictionary<string, int> GetMyOpenTicketAsRoles(UserProfile user, List<string> authorizedModulesNames)
        {
            Dictionary<string, int> roles = new Dictionary<string, int>();
            if (authorizedModulesNames == null || authorizedModulesNames.Count == 0)
                return roles;

            string userName = user.Name.Replace("'", "''");
            string selectQuery = string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketUser, userName);
            if (authorizedModulesNames != null)
            {
                selectQuery = string.Format("{0} = '{1}' And ({2})", DatabaseObjects.Columns.TicketUser, user.Id,
               string.Join(" Or ", authorizedModulesNames.Select(x => string.Format("{0} ='{1}'", DatabaseObjects.Columns.ModuleNameLookup, x))));
            }

            string[] columnnames = new string[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ModuleNameLookup, DatabaseObjects.Columns.UserRole, DatabaseObjects.Columns.TicketUser };
            //DataTable moduleUserStatisticsTable = UGITUtility.ToDataTable<ModuleUserStatistic>(_moduleUserStatsManager.GetModuleUserStatistics()).DefaultView.ToTable(true, columnnames);  //uGITCache.GetDataTable(DatabaseObjects.Tables.ModuleUserStatistics);
            DataTable moduleUserStatisticsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").DefaultView.ToTable(true, columnnames);

            if (moduleUserStatisticsTable == null || moduleUserStatisticsTable.Rows.Count == 0)
                return roles;

            List<string> hideTickets = new List<string>();
            DataRow[] cuSVCSubTickets = GetSVCSubTicketsForHide();
            if (cuSVCSubTickets != null && cuSVCSubTickets.Length > 0)
            {
                hideTickets = cuSVCSubTickets.Select(x => x.Field<string>(DatabaseObjects.Columns.ChildTicketId)).ToList();
            }

            DataRow[] statisticsRows = moduleUserStatisticsTable.Select(selectQuery);
            if (hideTickets != null && hideTickets.Count > 0 && statisticsRows.Length > 0)
            {
                // Note Requester renamed to Requestor, so handle both
                DataRow[] hideUserStatisticsRows = statisticsRows.Where(x => (x.Field<string>(DatabaseObjects.Columns.UserRole) == "Initiator" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requestor" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requester")
                                                                        && hideTickets.Exists(y => y == x.Field<string>(DatabaseObjects.Columns.TicketId))).ToArray();

                statisticsRows = statisticsRows.Except(hideUserStatisticsRows).ToArray();
            }

            if (statisticsRows.Length == 0)
                return roles;


            DataTable myStatistics = statisticsRows.Distinct().CopyToDataTable();
            var groupedData = from b in myStatistics.AsEnumerable()
                              group b by b.Field<string>(DatabaseObjects.Columns.UserRole) into g
                              let count = g.Count()
                              select new
                              {
                                  Role = g.Key,
                                  Count = count,
                              };

            foreach (var x in groupedData)
            {
                roles.Add(x.Role.ToString(), x.Count);
            }

            return roles;
        }

        /// <summary>
        /// Get service sub tickets where current user is as initiator or requestor
        /// these tickek will be hidden from current user
        /// </summary>
        /// <returns></returns>
        /// 
        //private static DataRow[] GetSVCSubTicketsForHide()
        //{
        //    //DataTable moduleUserStatisticsListTable = uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleUserStatistics);
        //    //DataRow svcModule = uGITCache.GetModuleDetails("SVC");
        //    //return GetSVCSubTicketsForHide(moduleUserStatisticsListTable, svcModule, SPContext.Current.Web);
        //}
        //private static DataRow[] GetSVCSubTicketsForHide()
        //{
        //    DataTable moduleUserStatisticsListTable = (SPContext.Current != null) ?
        //                                                    uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleUserStatistics) :
        //                                                    SPListHelper.GetDataTable(DatabaseObjects.Lists.ModuleUserStatistics, spWeb);
        //    DataRow svcModule = uHelper.GetModuleDetails("SVC", spWeb);
        //    return GetSVCSubTicketsForHide(moduleUserStatisticsListTable, svcModule, spWeb);
        //}
        private DataRow[] GetSVCSubTicketsForHide(/*DataTable moduleUserStatisticsListTable, DataRow svcModule*/)
        {
            List<string> svcRows = new List<string>();
            DataRow[] cuSVCSubTickets = new DataRow[0];
            //try
            //{
            //    if (moduleUserStatisticsListTable != null && moduleUserStatisticsListTable.Rows.Count > 0)
            //    {
            //        if (svcModule != null)
            //        {
            //            // Handle names like Peter O'Toole
            //            string userName = spWeb.CurrentUser != null ? spWeb.CurrentUser.Name.Replace("'", "''") : string.Empty;

            //            svcRows = moduleUserStatisticsListTable.Select(string.Format("{0}='{1}' and {2}='{3}' and ({4}='Initiator' or {4}='Requestor' or {4}='Requester')", // Note Requester renamed to Requestor, so handle both
            //                DatabaseObjects.Columns.ModuleId, svcModule[DatabaseObjects.Columns.Id], DatabaseObjects.Columns.TicketUser, userName,
            //                DatabaseObjects.Columns.UserRole)).Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).Distinct().ToList();

            //            DataTable ticketRelationShips = SPListHelper.GetDataTable(DatabaseObjects.Lists.TicketRelationship, spWeb);
            //            if (ticketRelationShips != null && ticketRelationShips.Rows.Count > 0)
            //            {
            //                cuSVCSubTickets = ticketRelationShips.AsEnumerable().Join(svcRows, x => x.Field<string>(DatabaseObjects.Columns.ParentTicketId), x => x, (x, y) => x).ToArray();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteException(ex, "GetSVCSubTicketsForHide");
            //    cuSVCSubTickets = new DataRow[0];
            //}
            return cuSVCSubTickets;
        }

        public DataTable GetMyOpenTicketDataByRole(UserProfile user, string role)
        {
            DataTable result = null;
            DataTable moduleUserStatisticsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            //DataTable moduleUserStatisticsTable = UGITUtility.ToDataTable<ModuleUserStatistic>(_moduleUserStatsManager.GetModuleUserStatistics()); // uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleUserStatistics);
            if (moduleUserStatisticsTable == null || moduleUserStatisticsTable.Rows.Count == 0)
                return result;

            string userName = user.Name.Replace("'", "''");
            role = role.Replace("'", "''");
            string selectQuery = string.Format("{0} = '{1}' and {2}='{3}'", DatabaseObjects.Columns.TicketUser, user.Id, DatabaseObjects.Columns.UserRole, role);

            DataRow[] statisticsRows = moduleUserStatisticsTable.Select(selectQuery);
            if (statisticsRows.Length == 0)
                return result;

            List<string> hideTickets = new List<string>();
            DataRow[] cuSVCSubTickets = GetSVCSubTicketsForHide();
            if (cuSVCSubTickets != null && cuSVCSubTickets.Length > 0)
            {
                hideTickets = cuSVCSubTickets.Select(x => x.Field<string>(DatabaseObjects.Columns.ChildTicketId)).ToList();
            }

            if (hideTickets != null && hideTickets.Count > 0 && statisticsRows.Length > 0)
            {
                // Note Requester renamed to Requestor, so handle both
                DataRow[] hideUserStatisticsRows = statisticsRows.Where(x => (x.Field<string>(DatabaseObjects.Columns.UserRole) == "Initiator" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requestor" || x.Field<string>(DatabaseObjects.Columns.UserRole) == "Requester")
                     && hideTickets.Exists(y => y == x.Field<string>(DatabaseObjects.Columns.TicketId))).ToArray();

                statisticsRows = statisticsRows.Except(hideUserStatisticsRows).ToArray();
            }

            // **  Added code to show selected columns in Home Page Grid
            ModuleColumnManager columnManager = new ModuleColumnManager(_context);
            List<string> cols = columnManager.Load(x => x.CategoryName.EqualsIgnoreCase("MyHomeTab") && x.IsDisplay == true).Select(x => x.FieldName).ToList();

            DataTable openTickets = new DataTable();
            DataRow[] mymodules = moduleUserStatisticsTable.DefaultView.ToTable(true, DatabaseObjects.Columns.ModuleNameLookup).Select();
            foreach (DataRow item in mymodules)
            {
                try
                {
                    DataTable moduleOpentickets = _ticketManager.GetOpenTickets(_moduleViewManager.LoadByName(Convert.ToString(item[0])));
                    //openTickets.Merge(moduleOpentickets);

                    if (moduleOpentickets == null || moduleOpentickets.Rows.Count <= 0)
                        continue;

                    if (openTickets.Rows.Count <= 0)
                    {
                        openTickets = moduleOpentickets;

                        // **  Added code to show selected columns in Home Page Grid
                        if (cols.Count > 0)
                        {
                            foreach (var col in cols)
                            {
                                if (!UGITUtility.IfColumnExists(col, openTickets))
                                {
                                    openTickets.Columns.Add(col, typeof(string));
                                }
                            }
                        }
                    }
                    else
                    {
                        openTickets.Merge(moduleOpentickets, false, MissingSchemaAction.Ignore);
                    }
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex);
                }
            }
            DataTable myOpenTickets = statisticsRows.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.TicketId);
            //DataTable openTickets = uGITCache.ModuleDataCache.GetOpenTicketFromList();

            var myOpenTicketData = from op in openTickets.AsEnumerable()
                                   join st in myOpenTickets.AsEnumerable() on op.Field<string>(DatabaseObjects.Columns.TicketId) equals st.Field<string>(DatabaseObjects.Columns.TicketId)
                                   select op;

            if (myOpenTicketData.Count() > 0)
            {
                result = myOpenTicketData.CopyToDataTable();
            }
            return result;
        }

        private ModuleStatisticResponse GetMyOpenTicketsFromDatabase(ModuleStatisticRequest request, UGITModule module)
        {
            if (module == null)
                return null;

            List<string> userFilteredList = new List<string>();
            ModuleStatisticResponse response = new ModuleStatisticResponse();
            foreach (ModuleUserType userType in module.List_ModuleUserTypes)
            {
                userFilteredList.Add(string.Format("{0} = '{1}'", userType.ColumnName, request.UserID));
            }
            string whereQuery = string.Join(" Or ", userFilteredList.ToArray());
            DataRow[] tableAsRoles = _ticketManager.GetOpenTickets(module).Select(whereQuery); //SPListHelper.GetDataTable(module.ModuleTicketTable, query1, request.SPWebObj);
            string tab = FilterTab.myopentickets; //new Tab("myopentickets", "myopentickets"); // request.Tabs.Find(x => x.Name == "myopentickets");
            response.CurrentTab = tab;
            response.ModuleName = request.ModuleName;
            if (tableAsRoles != null && tableAsRoles.Count() > 0)
            {
                response.ResultedData = tableAsRoles.CopyToDataTable();
                response.CurrentTabCount = tableAsRoles.Count();
            }
            return response;
        }

        private ModuleStatisticResponse GetMyGroupTicketsFromDatabase(ModuleStatisticRequest request, UGITModule module)
        {
            if (module == null)
                return null;

            List<string> userMyGroupFilteredList = new List<string>();
            ModuleStatisticResponse response = new ModuleStatisticResponse();
            foreach (ModuleUserType userType in module.List_ModuleUserTypes)
            {
                userMyGroupFilteredList.Add(string.Format("{0} = {1}", userType.FieldName, request.UserID));
            }

            //SPQuery query1 = new SPQuery();
            //query1.ViewFields = uGITCache.ModuleConfigCache.GetCachedModuleViewFields(request.SPWebObj, module.ModuleName);
            //query1.ViewFieldsOnly = true;
            //query1.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(userMyGroupFilteredList, userMyGroupFilteredList.Count - 1, false));
            //response.CurrentTab = CustomFilterTab.MyGroupTickets;
            //response.ModuleName = request.ModuleName;

            //SPList moduleTickets = SPListHelper.GetSPList(module.ModuleTicketTable, request.SPWebObj);
            //if (moduleTickets != null)
            //{
            //    DataTable tableAsRoles = moduleTickets.GetItems(query1).GetDataTable();
            //    if (tableAsRoles != null)
            //    {
            //        response.ResultedData = tableAsRoles;
            //        response.CurrentTabCount = tableAsRoles.Rows.Count;
            //    }
            //}
            return response;
        }

        public DataTable GetTicketsCountByPRP(string moduleName, bool groupByCategory, List<string> status, DateTime from, DateTime to, bool isModuleSort, bool includeTechnician, string selectedITManagers)
        {
            //   ModuleViewManager moduleViewManager = new ModuleViewManager(this._context);
            try
            {
                DataTable data = new DataTable();
                data.Columns.Add(DatabaseObjects.Columns.Category);
                data.Columns.Add(DatabaseObjects.Columns.PRP);
                data.Columns.Add(DatabaseObjects.Columns.Status);
                data.Columns.Add(DatabaseObjects.Columns.ModuleName);
                data.Columns.Add("Count", typeof(int));
                List<string> nonOnHoldStages = status.Where(x => x.ToLower() != "onhold").ToList();
                if (!string.IsNullOrEmpty(moduleName))
                {
                    string[] moduleNames = moduleName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    DataTable openTickets = new DataTable();
                    foreach (string strModule in moduleNames)
                    {
                        List<TicketByPRPAndStatus> resultedData = new List<TicketByPRPAndStatus>();
                        UGITModule module = _moduleViewManager.LoadByName(strModule);
                        openTickets = _ticketManager.GetOpenTickets(module);
                        LifeCycleStage closedStage = module.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());
                        DataTable closeTickets = _ticketManager.GetClosedTickets(module); //uGITCache.ModuleDataCache.GetClosedTickets(module.ID, spWeb);
                        if (openTickets == null && closeTickets == null)
                            continue;

                        string ticketCreationDateColumn = DatabaseObjects.Columns.Created;
                        if (openTickets != null && openTickets.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                            ticketCreationDateColumn = DatabaseObjects.Columns.TicketCreationDate;

                        List<string> queryExps = new List<string>();
                        if (from != DateTime.MinValue)
                            queryExps.Add(string.Format("{0}>#{1}#", ticketCreationDateColumn, from.ToString("MM/dd/yyyy")));
                        if (to != DateTime.MinValue)
                            queryExps.Add(string.Format("{0}< #{1}#", ticketCreationDateColumn, to.AddDays(1).ToString("MM/dd/yyyy")));

                        DataRow[] filteredTickets = new DataRow[0];
                        List<string> aQueryExps = new List<string>();
                        foreach (string statusName in nonOnHoldStages)
                        {
                            if (strModule == "CMDB")
                                aQueryExps.Add(string.Format("{0} ='{1}'", DatabaseObjects.Columns.TicketStatus, statusName));
                            else
                                aQueryExps.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Status, statusName));
                        }
                        // queryExps.Add(string.Format("{0} <>'1'", DatabaseObjects.Columns.TicketOnHold));

                        List<string> combineQueryExps = new List<string>();
                        if (aQueryExps.Count > 0)
                        {
                            if (strModule != "CMDB") // Added condition to check if module is CMDB, then don't add Hold related condition in Query Expression.
                            {
                                string nonHoldOnly = string.Format("({0} ='0' OR {0} is null)", DatabaseObjects.Columns.TicketOnHold);
                                combineQueryExps.Add(string.Format("(({0}) AND {1})", string.Join(" OR ", aQueryExps), nonHoldOnly));
                            }
                            combineQueryExps.AddRange(queryExps);
                        }

                        if (openTickets != null && openTickets.Rows.Count > 0)
                            filteredTickets = openTickets.Select(string.Join(" AND ", combineQueryExps));

                        DataTable dt = new DataTable();
                        if (includeTechnician)
                        {
                            if (filteredTickets.Length > 0)
                                dt = filteredTickets.CopyToDataTable();
                            if (dt.Rows.Count > 0)
                            {
                                DataTable tempTable = dt.Copy();
                                for (int x = 0; x < dt.Rows.Count; x++)
                                {
                                    string orps = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(dt.Rows[x][DatabaseObjects.Columns.TicketORP]));
                                    string[] arrORP = orps.Split(';');

                                    for (int i = 0; i < (arrORP.Length); i++)
                                    {
                                        DataRow dr = tempTable.NewRow();
                                        dr.ItemArray = dt.Rows[x].ItemArray;
                                        dr[DatabaseObjects.Columns.TicketPRP] = arrORP[i].Trim();
                                        tempTable.Rows.Add(dr);
                                        tempTable.AcceptChanges();
                                    }
                                }
                                filteredTickets = tempTable.Select();
                            }
                        }

                        if (groupByCategory && strModule != "SVC")
                        {
                            var byGroups = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory], prp = x[DatabaseObjects.Columns.TicketPRP], status = x[DatabaseObjects.Columns.ModuleStepLookup] })
                               .Select(x => new TicketByPRPAndStatus() { Category = Convert.ToString(x.Key.cateroy), PRP = Convert.ToString(x.Key.prp), Status = Convert.ToString(x.Key.status), Count = x.Count() }).ToList();
                            resultedData.AddRange(byGroups);
                        }
                        else
                        {
                            if (strModule != "CMDB")
                            {
                                var byGroups = filteredTickets.GroupBy(x => new { prp = _context.UserManager.GetUserById(Convert.ToString(x[DatabaseObjects.Columns.TicketPRP])) != null ? _context.UserManager.GetUserById(Convert.ToString(x[DatabaseObjects.Columns.TicketPRP])).Name : "", status = x[DatabaseObjects.Columns.Status] })
                                    .Select(x => new TicketByPRPAndStatus() { PRP = Convert.ToString(x.Key.prp), Status = Convert.ToString(x.Key.status), Count = x.Count() }).ToList();
                                resultedData.AddRange(byGroups);
                            }
                        }

                        if (status.FirstOrDefault(x => x.ToLower() == "onhold") != null && openTickets.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                        {
                            DataRow[] onHoldTickets = new DataRow[0];
                            List<string> onHoldQueryExps = new List<string>();
                            onHoldQueryExps.Add(string.Format("{0}='1'", DatabaseObjects.Columns.TicketOnHold));
                            onHoldQueryExps.AddRange(queryExps);
                            onHoldTickets = openTickets.Select(string.Join(" AND ", onHoldQueryExps));

                            if (groupByCategory && strModule != "SVC")
                            {
                                var byGroups = onHoldTickets.AsEnumerable().GroupBy(x => new { category = x[DatabaseObjects.Columns.TicketRequestTypeCategory], prp = _context.UserManager.GetUserById(Convert.ToString(x[DatabaseObjects.Columns.TicketPRP])).Name })
                                    .Select(x => new TicketByPRPAndStatus() { Category = Convert.ToString(x.Key.category), PRP = Convert.ToString(x.Key.prp), Status = "OnHold", Count = x.Count() }).ToList();
                                resultedData.AddRange(byGroups);
                            }
                            else
                            {
                                var byGroups = onHoldTickets.GroupBy(x => new { prp = _context.UserManager.GetDisplayNameFromUserId(Convert.ToString(x[DatabaseObjects.Columns.TicketPRP])), status = x[DatabaseObjects.Columns.Status] })
                                                          .Select(x => new TicketByPRPAndStatus() { PRP = Convert.ToString(x.Key.prp), Status = Convert.ToString(x.Key.status), Count = x.Count() }).ToList();
                                resultedData.AddRange(byGroups);
                            }
                        }

                        if (closeTickets != null && status.Contains("Closed") && closeTickets.Columns.Contains(DatabaseObjects.Columns.TicketCloseDate))
                        {
                            List<string> closeQueryExps = new List<string>();
                            if (from != DateTime.MinValue)
                                closeQueryExps.Add(string.Format("{0}>#{1}#", DatabaseObjects.Columns.TicketCloseDate, from.ToString("MM/dd/yyyy")));
                            if (to != DateTime.MinValue)
                                closeQueryExps.Add(string.Format("{0}< #{1}#", DatabaseObjects.Columns.TicketCloseDate, to.AddDays(1).ToString("MM/dd/yyyy")));

                            filteredTickets = closeTickets.Select(string.Join(" AND ", closeQueryExps));

                            if (groupByCategory && strModule != "SVC")
                            {
                                var byGroups = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory], prp = x[DatabaseObjects.Columns.TicketPRP], status = x[DatabaseObjects.Columns.Status] })
                                   .Select(x => new TicketByPRPAndStatus() { Category = Convert.ToString(x.Key.cateroy), PRP = Convert.ToString(x.Key.prp), Status = Convert.ToString(x.Key.status), Count = x.Count() }).ToList();
                                resultedData.AddRange(byGroups);
                            }
                            else
                            {
                                var byGroups = filteredTickets.GroupBy(x => new { prp = _context.UserManager.GetDisplayNameFromUserId(Convert.ToString(x[DatabaseObjects.Columns.TicketResolvedBy])), status = x[DatabaseObjects.Columns.Status] })
                                    .Select(x => new TicketByPRPAndStatus() { PRP = Convert.ToString(x.Key.prp), Status = Convert.ToString(x.Key.status), Count = x.Count() }).ToList();
                                resultedData.AddRange(byGroups);
                            }
                        }

                        foreach (var item in resultedData)
                        {
                            DataRow rRow = data.NewRow();
                            rRow[DatabaseObjects.Columns.PRP] = item.PRP;
                            if (string.IsNullOrWhiteSpace(item.PRP))
                                rRow[DatabaseObjects.Columns.PRP] = "(none)";
                            rRow[DatabaseObjects.Columns.Category] = item.Category;
                            if (string.IsNullOrWhiteSpace(item.Category))
                                rRow[DatabaseObjects.Columns.Category] = "None";
                            rRow[DatabaseObjects.Columns.Status] = item.Status;
                            if (item.Status.ToLower() == "onhold")
                                rRow[DatabaseObjects.Columns.Status] = "On Hold";
                            rRow[DatabaseObjects.Columns.ModuleName] = strModule;
                            rRow["Count"] = item.Count;

                            data.Rows.Add(rRow);
                        }
                    }

                    if (data == null || data.Rows.Count == 0)
                        return null;

                    if (!string.IsNullOrEmpty(selectedITManagers) && selectedITManagers.ToLower() != "all")
                    {
                        string[] arrManagers = selectedITManagers.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        List<UserProfile> lstUsers = new List<UserProfile>();
                        foreach (string manager in arrManagers)
                        {
                            string managerId = manager;
                            if (!string.IsNullOrEmpty(managerId))
                            {
                                lstUsers.AddRange(UserManager.GetUserByManager(managerId));
                            }
                        }

                        // Altering PRP Names to avoid conflict of apostrophe(')
                        string strUsers = string.Join("','", lstUsers.Select(x => x.Name).ToList());
                        DataRow[] dr = data.Select(string.Format("{0} IN ('{1}') AND Status not IN ('{2}') ", DatabaseObjects.Columns.PRP, strUsers, "0"));
                        if (dr.Length > 0)
                        {
                            data = dr.CopyToDataTable();

                        }
                        else
                            data = null;
                    }
                }

                if (data != null && data.Rows.Count > 0)
                {
                    if (isModuleSort && data.Columns.Contains(DatabaseObjects.Columns.ModuleName))
                    {
                        data.DefaultView.Sort = string.Format("{0} ASC,{1} ASC", DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.PRP);
                        data = data.DefaultView.ToTable();
                    }
                }

                return data;

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }
            return null;
           
        }

        public DataTable GetSummaryByTechnicianDrillDownData(string moduleName, string category, List<string> status, DateTime from, DateTime to, bool includeTechnician, string selectedITManagers)
        {
            DataTable data = new DataTable();
            DataRow[] filteredTickets = new DataRow[0];
            DataRow[] closedTickets = new DataRow[0];
            DataRow[] onHoldTickets = new DataRow[0];
            List<string> nonOnHoldStages = status.Where(x => x.ToLower().Replace(" ", string.Empty) != "onhold" && x.ToLower() != "closed").ToList();
            if (!string.IsNullOrEmpty(moduleName))
            {
                string[] moduleNames = moduleName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                DataTable openTickets = new DataTable();
                foreach (string strModule in moduleNames)
                {
                    List<TicketByPRPAndStatus> resultedData = new List<TicketByPRPAndStatus>();
                    UGITModule module = _moduleViewManager.LoadByName(strModule);  //uGITCache.ModuleConfigCache.GetCachedModule(spWeb, strModule);
                    openTickets = _ticketManager.GetOpenTickets(module);//uGITCache.ModuleDataCache.GetOpenTickets(module.ID, spWeb);

                    LifeCycleStage closedStage = module.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());

                    DataTable closeTickets = _ticketManager.GetClosedTickets(module); //uGITCache.ModuleDataCache.GetClosedTickets(module.ID, spWeb);

                    if (openTickets == null && closeTickets == null)
                        continue;

                    DataTable dt = new DataTable();

                    //closedStage.StageType
                    if (closedStage != null && status.Contains(closedStage.Name) && closeTickets != null && closeTickets.Columns.Contains(DatabaseObjects.Columns.TicketCloseDate))
                    {
                        List<string> closeQueryExps = new List<string>();
                        if (from != DateTime.MinValue)
                            closeQueryExps.Add(string.Format("{0}>#{1}#", DatabaseObjects.Columns.TicketCloseDate, from.ToString("MM/dd/yyyy")));
                        if (to != DateTime.MinValue)
                            closeQueryExps.Add(string.Format("{0}< #{1}#", DatabaseObjects.Columns.TicketCloseDate, to.AddDays(1).ToString("MM/dd/yyyy")));

                        closedTickets = closeTickets.Select(string.Join(" AND ", closeQueryExps));
                        // For closed tickets, use TicketResolvedBy instead of TicketPRP
                        if (closedTickets.Length > 0)
                        {
                            foreach (DataRow dr in closedTickets)
                            {
                                dr[DatabaseObjects.Columns.TicketPRP] = dr[DatabaseObjects.Columns.TicketResolvedBy];
                            }
                        }
                    }
                    string ticketCreationDateColumn = DatabaseObjects.Columns.Created;
                    if (openTickets != null && openTickets.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                        ticketCreationDateColumn = DatabaseObjects.Columns.TicketCreationDate;

                    List<string> queryExps = new List<string>();
                    if (from != DateTime.MinValue)
                        queryExps.Add(string.Format("{0}>#{1}#", ticketCreationDateColumn, from.ToString("MM/dd/yyyy")));
                    if (to != DateTime.MinValue)
                        queryExps.Add(string.Format("{0}< #{1}#", ticketCreationDateColumn, to.AddDays(1).ToString("MM/dd/yyyy")));

                    List<string> aQueryExps = new List<string>();
                    if (status.FirstOrDefault(x => x.ToLower().Replace(" ", string.Empty) == "onhold") != null && openTickets.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                    {

                        List<string> onHoldQueryExps = new List<string>();
                        onHoldQueryExps.Add(string.Format("{0}='1'", DatabaseObjects.Columns.TicketOnHold));
                        onHoldQueryExps.AddRange(queryExps);
                        onHoldTickets = openTickets.Select(string.Join(" AND ", onHoldQueryExps));
                    }
                    if (nonOnHoldStages.Count > 0)
                    {
                        foreach (string statusName in nonOnHoldStages)
                        {
                            if (strModule == "CMDB")
                                aQueryExps.Add(string.Format("{0} ='{1}'", DatabaseObjects.Columns.TicketStatus, statusName));
                            else
                                aQueryExps.Add(string.Format("{0} ='{1}'", DatabaseObjects.Columns.Status, statusName));
                        }
                        List<string> combineQueryExps = new List<string>();
                        //  queryExps.Add(string.Format("{0} <>'1'", DatabaseObjects.Columns.TicketOnHold));
                        if (aQueryExps.Count > 0)
                        {
                            string nonHoldOnly = string.Format("({0} ='0' OR {0} is null)", DatabaseObjects.Columns.TicketOnHold);
                            combineQueryExps.Add(string.Format("(({0}) AND {1})", string.Join(" OR ", aQueryExps), nonHoldOnly));
                            combineQueryExps.AddRange(queryExps);
                        }
                        filteredTickets = openTickets.Select(string.Join(" AND ", combineQueryExps));
                        // Also get ORP assignments if option selected - NOT NEEDED FOR CLOSED TICKETS!
                        if (includeTechnician)
                        {
                            if (filteredTickets.Length > 0)
                                dt = filteredTickets.CopyToDataTable();
                            if (dt.Rows.Count > 0)
                            {
                                DataTable tempTable = dt.Copy();
                                for (int x = 0; x < dt.Rows.Count; x++)
                                {
                                    string orps = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(dt.Rows[x][DatabaseObjects.Columns.TicketORP]));
                                    string[] arrORP = orps.Split(';');
                                    for (int i = 0; i < (arrORP.Length); i++)
                                    {
                                        DataRow dr = tempTable.NewRow();
                                        dr.ItemArray = dt.Rows[x].ItemArray;
                                        dr[DatabaseObjects.Columns.TicketPRP] = arrORP[i].Trim();
                                        tempTable.Rows.Add(dr);
                                        tempTable.AcceptChanges();
                                    }
                                }
                                filteredTickets = tempTable.Select();
                            }
                        }
                    }


                    if (filteredTickets.Length > 0)
                    {
                        if (data != null && data.Rows.Count > 0)
                            data.Merge(filteredTickets.CopyToDataTable());
                        else
                            data = filteredTickets.CopyToDataTable();
                    }
                    if (onHoldTickets.Length > 0)
                    {
                        if (data != null && data.Rows.Count > 0)
                            data.Merge(onHoldTickets.CopyToDataTable());
                        else
                            data = onHoldTickets.CopyToDataTable();
                    }
                    if (closedTickets.Length > 0)
                    {
                        if (data != null && data.Rows.Count > 0)
                            data.Merge(closedTickets.CopyToDataTable());
                        else
                            data = closedTickets.CopyToDataTable();
                    }
                }

                if (!string.IsNullOrWhiteSpace(category))
                {
                    if (category.Contains(',') || category.Contains("'"))
                    {
                        string[] selectedCategories = category.Split(new string[] { "','", "," }, StringSplitOptions.RemoveEmptyEntries);
                        category = string.Join("','", selectedCategories.Select(x => x.Replace("'", "''")));
                    }
                    DataRow[] dr = new DataRow[0];
                    dr = data.Select(string.Format("{0} ='{1}'", DatabaseObjects.Columns.TicketRequestTypeCategory, category));
                    if (dr.Length > 0)
                        data = dr.CopyToDataTable();
                }

                // If a technician's name was passed in, filter on that
                if (data != null && data.Rows.Count > 0 && !string.IsNullOrEmpty(selectedITManagers) && selectedITManagers.ToLower() != "all")
                {
                    DataRow[] dr = new DataRow[0];
                    if (selectedITManagers.Contains("none"))
                    {
                        dr = data.Select(string.Format("({0} = '' OR {0} is null)", DatabaseObjects.Columns.TicketPRP));
                    }
                    else
                    {
                        _configFieldManager = new FieldConfigurationManager(_context);
                        var Id = _configFieldManager.GetFieldConfigurationIdByName(DatabaseObjects.Columns.TicketPRP, selectedITManagers);

                        dr = data.Select(string.Format("{0} IN ({1})", DatabaseObjects.Columns.TicketPRP, Id));

                        //dr = data.Select(string.Format("{0} IN ('{1}')", DatabaseObjects.Columns.TicketPRP, selectedITManagers));
                    }
                    if (dr.Length > 0)
                        data = dr.CopyToDataTable();
                    else
                        //data = data;
                        data = null;
                }
            }
            return data;
        }

        public DataTable GetWeeklyTeamPrfCount(string moduleName, DateTime from, DateTime to, string category)
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Category);
            data.Columns.Add(DatabaseObjects.Columns.Status);
            data.Columns.Add(DatabaseObjects.Columns.ModuleName);
            //data.Columns.Add(DatabaseObjects.Columns.Priority);
            data.Columns.Add(DatabaseObjects.Columns.TicketPriorityLookup);
            data.Columns.Add("Count", typeof(int));
            data.Columns.Add("Sequence", typeof(int));
            List<WeeklyTeamPerformance> resultedData = new List<WeeklyTeamPerformance>();
            UGITModule module = _moduleViewManager.LoadByName(moduleName); //uGITCache.ModuleConfigCache.GetCachedModule(spWeb, moduleName);
            DataTable openTickets = _ticketManager.GetAllTickets(module); //uGITCache.ModuleDataCache.GetAllTickets(module.ID, spWeb);

            DataRow[] filteredTickets = new DataRow[0];

            string ticketCreationDateColumn = DatabaseObjects.Columns.Created;
            if (openTickets.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                ticketCreationDateColumn = DatabaseObjects.Columns.TicketCreationDate;

            List<string> queryExps = new List<string>();
            if (from != DateTime.MinValue)
                queryExps.Add(string.Format("{0}>#{1}#", ticketCreationDateColumn, from.ToString("MM/dd/yyyy")));
            if (to != DateTime.MinValue)
                queryExps.Add(string.Format("{0}< #{1}#", ticketCreationDateColumn, to.AddDays(1).ToString("MM/dd/yyyy")));

            filteredTickets = openTickets.Select(string.Join(" AND ", queryExps));

            var byGroups = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory], priority = x[DatabaseObjects.Columns.TicketPriorityLookup] })
                          .Select(x => new WeeklyTeamPerformance() { Sequence = 1, Status = "Created", Category = Convert.ToString(x.Key.cateroy), Priority = Convert.ToString(x.Key.priority), Count = x.Count() }).ToList();
            resultedData.AddRange(byGroups);

            LifeCycleStage closedStage = module.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());

            DataTable closeTickets = _ticketManager.GetClosedTickets(module); //uGITCache.ModuleDataCache.GetClosedTickets(module.ID, spWeb);
            if (closeTickets != null && closeTickets.Columns.Contains(DatabaseObjects.Columns.TicketCloseDate))
            {
                List<string> closeQueryExps = new List<string>();
                if (from != DateTime.MinValue)
                    closeQueryExps.Add(string.Format("{0}>#{1}#", DatabaseObjects.Columns.TicketCloseDate, from.ToString("MM/dd/yyyy")));
                if (to != DateTime.MinValue)
                    closeQueryExps.Add(string.Format("{0}< #{1}#", DatabaseObjects.Columns.TicketCloseDate, to.AddDays(1).ToString("MM/dd/yyyy")));

                filteredTickets = closeTickets.Select(string.Join(" AND ", closeQueryExps));

                var byGroupsClosed = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory], priority = x[DatabaseObjects.Columns.TicketPriorityLookup] })
                         .Select(x => new WeeklyTeamPerformance() { Sequence = 2, Status = "Closed", Category = Convert.ToString(x.Key.cateroy), Priority = Convert.ToString(x.Key.priority), Count = x.Count() }).ToList();

                resultedData.AddRange(byGroupsClosed);
            }

            foreach (var item in resultedData)
            {
                DataRow rRow = data.NewRow();
                rRow["Sequence"] = item.Sequence;
                //rRow[DatabaseObjects.Columns.Priority] = item.Priority;
                rRow[DatabaseObjects.Columns.TicketPriorityLookup] = item.Priority;
                if (string.IsNullOrWhiteSpace(item.Priority))
                    //rRow[DatabaseObjects.Columns.Priority] = "None";
                    rRow[DatabaseObjects.Columns.TicketPriorityLookup] = "None";

                rRow[DatabaseObjects.Columns.Category] = item.Category;
                if (string.IsNullOrWhiteSpace(item.Category))
                    rRow[DatabaseObjects.Columns.Category] = "None";
                rRow[DatabaseObjects.Columns.Status] = item.Status;
                rRow[DatabaseObjects.Columns.ModuleName] = moduleName;
                rRow["Count"] = item.Count;
                data.Rows.Add(rRow);
            }
            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                category = category.Replace("; ", "','");
                DataRow[] dr = data.Select(string.Format("{0} IN ('{1}')", DatabaseObjects.Columns.Category, category));
                if (dr.Length > 0)
                    data = dr.CopyToDataTable();
                else
                    data = null;
            }
            if (data != null)
            {
                data.DefaultView.Sort = string.Format("{0} ASC", "Sequence");
                data = data.DefaultView.ToTable();
            }

            //if (data != null && data.Rows.Count > 0)
            //{
            //    data = uHelper.ConvertTableLookupValues(_context, data);
            //    data.Columns[DatabaseObjects.Columns.TicketPriorityLookup].ColumnName = DatabaseObjects.Columns.Priority;
            //}
            return data;
        }

        public DataTable GetNonPeakHoursCount(string moduleName, DateTime date, int nonPeakHrWndw, string workingWindowStartTime, string workingWindowEndTime)
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Status);
            data.Columns.Add("Count", typeof(int));
            data.Columns.Add("Sequence", typeof(int));
            string startDayWorkingHours = uHelper.GetWorkingHourOfWorkingDate(_context, date);
            string[] WorkingdateTime = startDayWorkingHours.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
            DateTime WorkingStartDateTime = Convert.ToDateTime(workingWindowStartTime);
            DateTime WorkingEndDateTime = Convert.ToDateTime(workingWindowEndTime);
            WorkingStartDateTime = new DateTime(date.Year, date.Month, date.Day, WorkingStartDateTime.Hour, WorkingStartDateTime.Minute, WorkingStartDateTime.Second);
            WorkingEndDateTime = new DateTime(date.Year, date.Month, date.Day, WorkingEndDateTime.Hour, WorkingEndDateTime.Minute, WorkingEndDateTime.Second);
            DateTime nonPeakHourBeforeStartTime = WorkingStartDateTime.Add(new TimeSpan(-nonPeakHrWndw, 0, 0));
            DateTime nonPeakHourBeforeEndTime = WorkingStartDateTime;

            DateTime nonPeakHourAfterStartTime = WorkingEndDateTime;
            DateTime nonPeakHourAfterEndTime = WorkingEndDateTime.Add(new TimeSpan(nonPeakHrWndw, 0, 0));

            List<NonPeakHoursPerformance> resultedData = new List<NonPeakHoursPerformance>();
            UGITModule module = _moduleViewManager.LoadByName(moduleName); //uGITCache.ModuleConfigCache.GetCachedModule(moduleName);
            DataTable openTickets = _ticketManager.GetOpenTickets(module); //uGITCache.ModuleDataCache.GetOpenTickets(module.ID);
            DataTable closeTickets = _ticketManager.GetClosedTickets(module);//uGITCache.ModuleDataCache.GetClosedTickets(module.ID);
            LifeCycleStage closedStage = module.List_LifeCycles.FirstOrDefault().Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());


            DataRow[] filteredTickets = new DataRow[0];

            #region CreatedTicketsBeforePeakHrs
            string ticketCreationDateColumn = DatabaseObjects.Columns.Created;
            if (openTickets.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                ticketCreationDateColumn = DatabaseObjects.Columns.TicketCreationDate;

            List<string> queryExps = new List<string>();
            if (nonPeakHourBeforeStartTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}>#{1}#", ticketCreationDateColumn, nonPeakHourBeforeStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
            if (nonPeakHourBeforeEndTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}<#{1}#", ticketCreationDateColumn, nonPeakHourBeforeEndTime.ToString("MM/dd/yyyy hh:mm:ss tt")));

            filteredTickets = openTickets.Select(string.Join(" AND ", queryExps));
            string statusTcktsCrtdBfrPeakHrs = "Tickets Created Between " + nonPeakHourBeforeStartTime.ToString("h:mm") + " - " + nonPeakHourBeforeEndTime.ToString("h:mm tt") + "";
            var byGroups = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory] })
                          .Select(x => new NonPeakHoursPerformance() { Sequence = 1, Status = statusTcktsCrtdBfrPeakHrs, Count = x.Count() }).ToList();
            if (byGroups.Count == 0)
                byGroups.Add(new NonPeakHoursPerformance() { Sequence = 1, Status = statusTcktsCrtdBfrPeakHrs, Count = 0 });
            resultedData.AddRange(byGroups);
            #endregion

            #region ClosedTicketsBeforePeakHrs
            if (closeTickets != null && closeTickets.Columns.Contains(DatabaseObjects.Columns.TicketCloseDate))
            {
                List<string> closeQueryExps = new List<string>();
                if (nonPeakHourBeforeStartTime != DateTime.MinValue)
                    closeQueryExps.Add(string.Format("{0}>#{1}#", DatabaseObjects.Columns.TicketCloseDate, nonPeakHourBeforeStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
                if (nonPeakHourBeforeEndTime != DateTime.MinValue)
                    closeQueryExps.Add(string.Format("{0}<#{1}#", DatabaseObjects.Columns.TicketCloseDate, nonPeakHourBeforeEndTime.ToString("MM/dd/yyyy hh:mm:ss tt")));

                filteredTickets = closeTickets.Select(string.Join(" AND ", closeQueryExps));
                string statusTcktsClsdBfrPeakHrs = "Tickets Closed Between " + nonPeakHourBeforeStartTime.ToString("h:mm") + " - " + nonPeakHourBeforeEndTime.ToString("h:mm tt") + "";
                var byGroupsClosed = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory] })
                         .Select(x => new NonPeakHoursPerformance() { Sequence = 2, Status = statusTcktsClsdBfrPeakHrs, Count = x.Count() }).ToList();
                if (byGroupsClosed.Count == 0)
                    byGroupsClosed.Add(new NonPeakHoursPerformance() { Sequence = 2, Status = statusTcktsClsdBfrPeakHrs, Count = 0 });
                resultedData.AddRange(byGroupsClosed);
            }
            #endregion

            #region UpdatedTicketsBeforePeakHrs


            queryExps = new List<string>();
            if (nonPeakHourBeforeStartTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}<#{1}#", ticketCreationDateColumn, nonPeakHourBeforeStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
            if (nonPeakHourBeforeStartTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}>#{1}#", DatabaseObjects.Columns.Modified, nonPeakHourBeforeStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
            if (nonPeakHourBeforeEndTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}<#{1}#", DatabaseObjects.Columns.Modified, nonPeakHourBeforeEndTime.ToString("MM/dd/yyyy hh:mm:ss tt")));

            filteredTickets = openTickets.Select(string.Join(" AND ", queryExps));

            string statusTcktsUpdtdBfrPeakHrs = "Tickets Updated Between " + nonPeakHourBeforeStartTime.ToString("h:mm") + " - " + nonPeakHourBeforeEndTime.ToString("h:mm tt") + "";
            byGroups = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory] })
                          .Select(x => new NonPeakHoursPerformance() { Sequence = 3, Status = statusTcktsUpdtdBfrPeakHrs, Count = x.Count() }).ToList();
            if (byGroups.Count == 0)
                byGroups.Add(new NonPeakHoursPerformance() { Sequence = 3, Status = statusTcktsUpdtdBfrPeakHrs, Count = 0 });
            resultedData.AddRange(byGroups);
            #endregion

            #region CreatedTicketsAfterPeakHrs


            queryExps = new List<string>();
            if (nonPeakHourAfterStartTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}>#{1}#", ticketCreationDateColumn, nonPeakHourAfterStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
            if (nonPeakHourAfterEndTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}<#{1}#", ticketCreationDateColumn, nonPeakHourAfterEndTime.ToString("MM/dd/yyyy hh:mm:ss tt")));

            filteredTickets = openTickets.Select(string.Join(" AND ", queryExps));
            string statusTcktsCrtdAftrPeakHrs = "Tickets Created Between " + nonPeakHourAfterStartTime.ToString("h:mm") + " - " + nonPeakHourAfterEndTime.ToString("h:mm tt") + "";
            byGroups = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory] })
                           .Select(x => new NonPeakHoursPerformance() { Sequence = 4, Status = statusTcktsCrtdAftrPeakHrs, Count = x.Count() }).ToList();
            if (byGroups.Count == 0)
                byGroups.Add(new NonPeakHoursPerformance() { Sequence = 4, Status = statusTcktsCrtdAftrPeakHrs, Count = 0 });
            resultedData.AddRange(byGroups);
            #endregion

            #region ClosedTicketsBeforePeakHrs
            if (closeTickets != null && closeTickets.Columns.Contains(DatabaseObjects.Columns.TicketCloseDate))
            {
                List<string> closeQueryExps = new List<string>();
                if (nonPeakHourAfterStartTime != DateTime.MinValue)
                    closeQueryExps.Add(string.Format("{0}>#{1}#", DatabaseObjects.Columns.TicketCloseDate, nonPeakHourAfterStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
                if (nonPeakHourAfterEndTime != DateTime.MinValue)
                    closeQueryExps.Add(string.Format("{0}<#{1}#", DatabaseObjects.Columns.TicketCloseDate, nonPeakHourAfterEndTime.ToString("MM/dd/yyyy hh:mm:ss tt")));

                filteredTickets = closeTickets.Select(string.Join(" AND ", closeQueryExps));
                string statusTcktsClsdAftrPeakHrs = "Tickets Closed Between " + nonPeakHourAfterStartTime.ToString("h:mm") + " - " + nonPeakHourAfterEndTime.ToString("h:mm tt") + "";
                var byGroupsClosed = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory] })
                         .Select(x => new NonPeakHoursPerformance() { Sequence = 5, Status = statusTcktsClsdAftrPeakHrs, Count = x.Count() }).ToList();
                if (byGroups.Count == 0)
                    byGroups.Add(new NonPeakHoursPerformance() { Sequence = 5, Status = statusTcktsClsdAftrPeakHrs, Count = 0 });
                resultedData.AddRange(byGroupsClosed);
            }
            #endregion

            #region UpdatedTicketsAfterPeakHrs


            queryExps = new List<string>();
            if (nonPeakHourBeforeStartTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}<#{1}#", ticketCreationDateColumn, nonPeakHourAfterStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
            if (nonPeakHourAfterStartTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}>#{1}#", DatabaseObjects.Columns.Modified, nonPeakHourAfterStartTime.ToString("MM/dd/yyyy hh:mm:ss tt")));
            if (nonPeakHourAfterEndTime != DateTime.MinValue)
                queryExps.Add(string.Format("{0}<#{1}#", DatabaseObjects.Columns.Modified, nonPeakHourAfterEndTime.ToString("MM/dd/yyyy hh:mm:ss tt")));

            filteredTickets = openTickets.Select(string.Join(" AND ", queryExps));
            string statusTcktsUpdtAftrPeakHrs = "Tickets Updated Between " + nonPeakHourAfterStartTime.ToString("h:mm") + " - " + nonPeakHourAfterEndTime.ToString("h:mm tt") + "";
            byGroups = filteredTickets.GroupBy(x => new { cateroy = x[DatabaseObjects.Columns.TicketRequestTypeCategory] })
                         .Select(x => new NonPeakHoursPerformance() { Sequence = 6, Status = statusTcktsUpdtAftrPeakHrs, Count = x.Count() }).ToList();
            if (byGroups.Count == 0)
                byGroups.Add(new NonPeakHoursPerformance() { Sequence = 6, Status = statusTcktsUpdtAftrPeakHrs, Count = 0 });
            resultedData.AddRange(byGroups);
            #endregion

            foreach (var item in resultedData)
            {
                DataRow rRow = data.NewRow();
                rRow[DatabaseObjects.Columns.Status] = item.Status;
                rRow["Count"] = item.Count;
                rRow["Sequence"] = item.Sequence;
                data.Rows.Add(rRow);
            }
            data.DefaultView.Sort = string.Format("{0} ASC", "Sequence");

            data = data.DefaultView.ToTable();
            return data;

        }

    }
}
