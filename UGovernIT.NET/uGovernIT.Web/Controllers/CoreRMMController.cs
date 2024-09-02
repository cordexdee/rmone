using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Web.Models;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using System.IO;
using uGovernIT.Util.Log;
using uGovernIT.Manager.Core;
using System.Data.SqlTypes;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/CoreRMM")]
    public class CoreRMMController : ApiController
    {
        private ApplicationContext _applicationContext;
        private ModuleViewManager ModuleManager;
        private TicketManager _TicketManager;
        private ConfigurationVariableManager _configVariableManager;
        string labelFormat = string.Empty;
        public CoreRMMController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            ModuleManager = new ModuleViewManager(_applicationContext);
            _TicketManager = new TicketManager(_applicationContext);
            _configVariableManager = new ConfigurationVariableManager(_applicationContext);
        }
        [HttpGet]
        [Route("GetRMMPanelCount")]
        public async Task<IHttpActionResult> GetRMMPanelCount(string ViewID)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(context);
            List<RMMCountPanelResponse> lstList = new List<RMMCountPanelResponse>();
            string userid = context.CurrentUser.Id;
            TicketManager ticketMgr = new TicketManager(context);
            ModuleViewManager moduleMgr = new ModuleViewManager(context);
            UGITModule cprModule = moduleMgr.LoadByName("CPR", true);
            UGITModule opmModule = moduleMgr.LoadByName("OPM", true);
            UGITModule cnsModule = moduleMgr.LoadByName("CNS", true);
            DataTable dtCPROpenTickets = new DataTable();
            DataTable dtCPRCloseTickets = new DataTable();
            DataTable dtOPMOpenTickets = new DataTable();
            DataTable dtCNSOpenTickets = new DataTable();

            try
            {
                if (cprModule != null)
                {
                    dtCPROpenTickets = ticketMgr.GetOpenTickets(cprModule);
                    dtCPRCloseTickets = ticketMgr.GetClosedTickets(cprModule);
                }
                if (opmModule != null)
                    dtOPMOpenTickets = ticketMgr.GetOpenTickets(opmModule);
                if (cnsModule != null)
                    dtCNSOpenTickets = ticketMgr.GetOpenTickets(cnsModule);

                var role = context.UserManager.GetUserRoles(userid).Select(x => x.Id).ToList();
                StringBuilder sbQuery = new StringBuilder();

                int viewID = UGITUtility.StringToInt(ViewID);
                DashboardPanelView View = objDashboardPanelViewManager.LoadViewByID(viewID, true);
                CommonDashboardsView commonViewObj = View.ViewProperty == null ? new CommonDashboardsView() : (View.ViewProperty as CommonDashboardsView);

                //TabViewManager tabViewManager = new TabViewManager(context);
                //List<TabView> tabViewrows = tabViewManager.GetTabsByViewName("");
                DashboardPanelProperty dpProperty = commonViewObj.Dashboards.FirstOrDefault(x => x.DashboardSubType == "LeftTicketCountBar");
                List<string> tabs = dpProperty.KPIList.Select(x => x.KpiName).ToList<string>();

                ModuleStatistics mod = new ModuleStatistics(context);
                Dictionary<string, int> stat = mod.LoadAllCount(tabs, userid);

                if (stat.ContainsKey("waitingonme"))
                {
                    RMMCountPanelResponse obj = GetResponseObjectFromStat(stat, dpProperty, "waitingonme");
                    lstList.Add(obj);
                }
                if (stat.ContainsKey("myopentickets"))
                {
                    RMMCountPanelResponse obj = GetResponseObjectFromStat(stat, dpProperty, "myopentickets");
                    lstList.Add(obj);
                }
                if (stat.ContainsKey("myproject"))
                {
                    RMMCountPanelResponse obj = GetResponseObjectFromStat(stat, dpProperty, "myproject");
                    lstList.Add(obj);
                }
                if (stat.ContainsKey("myopenopportunities"))
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "myopentickets", ModuleName = "OPM", Status = TicketStatus.Open, UserID = userid };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = mod.Load(statRequest, false);
                    string value = UGITUtility.ObjectToString(UGITUtility.StringToInt(statistics.ResultedData?.Rows.Count));
                    RMMCountPanelResponse obj1 = GetResponseObject(dpProperty, value, "myopenopportunities");
                    lstList.Add(obj1);
                }
                if (stat.ContainsKey("allopenproject"))
                {
                    RMMCountPanelResponse obj2 = GetResponseObjectFromStat(stat, dpProperty, "allopenproject");

                    lstList.Add(obj2);
                }
                if (stat.ContainsKey("allcloseproject"))
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allclosedtickets", ModuleName = "CPR", Status = TicketStatus.Closed, UserID = userid };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = mod.Load(statRequest, false);
                    string value = UGITUtility.ObjectToString(UGITUtility.StringToInt(statistics.ResultedData?.Rows.Count));
                    RMMCountPanelResponse obj1 = GetResponseObject(dpProperty, value, "allcloseproject");
                    obj1.IconUrl = "past_project.png";
                    lstList.Add(obj1);
                }
                if (stat.ContainsKey("futureopencpr"))
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allopentickets", ModuleName = "CPR", Status = TicketStatus.Open, UserID = userid };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = mod.Load(statRequest, false);
                    //string value = UGITUtility.ObjectToString(UGITUtility.StringToInt(statistics.ResultedData?.Rows.Count));
                    //RMMCountPanelResponse obj1 = GetResponseObject(dpProperty, value, "allopentickets");
                    DataTable openTicketsTable = new DataTable();
                    if (statistics != null)
                        openTicketsTable = statistics.ResultedData;
                    if (openTicketsTable != null)
                    {
                        DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0} > '{1}'", DatabaseObjects.Columns.EstimatedConstructionStart, DateTime.Now.ToString("MM/dd/yyyy")));
                        if (resolvedRows != null)
                        {
                            string value = UGITUtility.ObjectToString(UGITUtility.StringToInt(resolvedRows.Count()));
                            RMMCountPanelResponse obj1 = GetResponseObject(dpProperty, value, "futureopencpr");
                            obj1.IconUrl = "future_project.png";
                            lstList.Add(obj1);
                        }
                    }

                }
                if (stat.ContainsKey("currentopencpr"))
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allopentickets", ModuleName = "CPR", Status = TicketStatus.Open, UserID = userid };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = mod.Load(statRequest, false);
                    DataTable openTicketsTable = new DataTable();
                    if (statistics != null)
                        openTicketsTable = statistics.ResultedData;
                    if (openTicketsTable != null)
                    {
                        DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0}  <=  '{1}'", DatabaseObjects.Columns.EstimatedConstructionStart, DateTime.Now.ToString("MM/dd/yyyy")));
                        if (resolvedRows != null)
                        {
                            string value = UGITUtility.ObjectToString(UGITUtility.StringToInt(resolvedRows.Count()));
                            RMMCountPanelResponse obj1 = GetResponseObject(dpProperty, value, "currentopencpr");
                            obj1.IconUrl = "current_project.png";
                            lstList.Add(obj1);
                        }
                    }

                }
                if (stat.ContainsKey("totalresource"))
                {
                    //DataTable TotalResource = GetTableDataManager.GetTableDataUsingQuery($"TotalResources @TenantId='{context.TenantID}'");
                    //if (TotalResource != null && TotalResource.Rows.Count > 0)
                    //{
                    //    string value = UGITUtility.ObjectToString(UGITUtility.StringToInt(TotalResource.Rows[0][0]));
                    //    RMMCountPanelResponse obj1 = GetResponseObject(dpProperty, value, "totalresource");
                    //    obj1.IconUrl = "total_resource.png";
                    //    lstList.Add(obj1);
                    //}
                    // Above code is not needed becz we have already list of users.
                    UserProfileManager profileManager = new UserProfileManager(context);
                    string value = UGITUtility.ObjectToString(profileManager.GetUsersProfile().Where(x => x.Enabled == true).Count());
                    RMMCountPanelResponse obj1 = GetResponseObject(dpProperty, value, "totalresource");
                    obj1.IconUrl = "total_resource.png";
                    lstList.Add(obj1);

                }
                if (stat.ContainsKey("allopenopportunities"))
                {
                    RMMCountPanelResponse obj3 = GetResponseObject(dpProperty, UGITUtility.ObjectToString(dtOPMOpenTickets?.Rows.Count), "allopenopportunities");
                    obj3.IconUrl = "potential_project.png";
                    lstList.Add(obj3);
                }
                if (stat.ContainsKey("allopenservices"))
                {
                    RMMCountPanelResponse obj4 = GetResponseObject(dpProperty, UGITUtility.ObjectToString(dtCNSOpenTickets?.Rows.Count), "allopenservices");

                    lstList.Add(obj4);
                }
                if (stat.ContainsKey("recentwonopportunity"))
                {
                    DataRow[] recentwonopm = dtOPMOpenTickets.Select($"{DatabaseObjects.Columns.CRMOpportunityStatus}='Awarded'");
                    recentwonopm.OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.AwardedLossDate));
                    string value = UGITUtility.ObjectToString(recentwonopm.Count());
                    if (recentwonopm != null)
                    {
                        RMMCountPanelResponse obj5 = GetResponseObject(dpProperty, value, "recentwonopportunity");
                        lstList.Add(obj5);
                    }
                }
                if (stat.ContainsKey("recentlostopportunity"))
                {
                    DataRow[] recentlostopm = dtOPMOpenTickets.Select($"{DatabaseObjects.Columns.CRMOpportunityStatus}='Lost'");
                    recentlostopm.OrderByDescending(x => x.Field<DateTime>(DatabaseObjects.Columns.AwardedLossDate));
                    string value = UGITUtility.ObjectToString(recentlostopm.Count());
                    if (recentlostopm != null)
                    {
                        RMMCountPanelResponse obj6 = GetResponseObject(dpProperty, value, "recentlostopportunity");
                        lstList.Add(obj6);
                    }
                }
                if (stat.ContainsKey("openticketstoday"))
                {
                    List<UGITModule> smsmodules = ModuleManager.Load(x => x.ModuleType == ModuleType.SMS);
                    DataTable dttickets = new DataTable();
                    foreach (UGITModule module in smsmodules)
                    {
                        if (string.IsNullOrEmpty(module.ModuleTable))
                            continue;
                        DataTable dtmoduletickets = ticketMgr.GetTickets($"select * from {module.ModuleTable} where cast({DatabaseObjects.Columns.Created} as date) = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}' and {DatabaseObjects.Columns.Closed} <> 1");
                        if (dtmoduletickets != null && dtmoduletickets.Rows.Count > 0)
                            dttickets.Merge(dtmoduletickets);
                    }
                    if (dttickets != null)
                    {
                        RMMCountPanelResponse obj7 = GetResponseObject(dpProperty, UGITUtility.ObjectToString(dttickets.Rows.Count), "openticketstoday");
                        lstList.Add(obj7);
                    }

                }
                if (stat.ContainsKey("closeticketstoday"))
                {
                    List<UGITModule> smsmodules = ModuleManager.Load(x => x.ModuleType == ModuleType.SMS);
                    DataTable dttickets = new DataTable();
                    foreach (UGITModule module in smsmodules)
                    {
                        if (string.IsNullOrEmpty(module.ModuleTable))
                            continue;
                        DataTable dtmoduletickets = ticketMgr.GetTickets($"select * from {module.ModuleTable} where cast({DatabaseObjects.Columns.CloseDate} as date) = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}' and {DatabaseObjects.Columns.Closed} = 1");
                        if (dtmoduletickets != null && dtmoduletickets.Rows.Count > 0)
                            dttickets.Merge(dtmoduletickets);
                    }
                    if (dttickets != null)
                    {
                        RMMCountPanelResponse obj8 = GetResponseObject(dpProperty, UGITUtility.ObjectToString(dttickets.Rows.Count), "closeticketstoday");
                        lstList.Add(obj8);
                    }
                }
                if (stat.ContainsKey("nprtickets"))
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "nprtickets", ModuleName = "NPR", Status = TicketStatus.Open, UserID = userid };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = mod.Load(statRequest, false);
                    string value = UGITUtility.ObjectToString(UGITUtility.StringToInt(statistics.ResultedData?.Rows.Count));
                    RMMCountPanelResponse obj9 = GetResponseObject(dpProperty, value, "nprtickets");
                    lstList.Add(obj9);
                }
                if (stat.ContainsKey("resolvedtickets"))
                {
                    UGITModule nprmodule = ModuleManager.LoadByName(ModuleNames.TSR, true);

                    LifeCycle lifeCycle = nprmodule.List_LifeCycles.FirstOrDefault();
                    LifeCycleStage resolvedStage = null;
                    if (lifeCycle != null)
                        resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTitle == StageType.Resolved.ToString() || x.StageTypeChoice == StageType.Resolved.ToString());
                    // Added condition above, for CPR Under Construction/ Resolved tab
                    if (resolvedStage != null)
                    {
                        ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "myopentickets", ModuleName = ModuleNames.TSR, Status = TicketStatus.Open, UserID = userid };
                        ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                        statistics = mod.Load(statRequest, false);
                        DataTable openTicketsTable = new DataTable();
                        if (statistics != null)
                            openTicketsTable = statistics.ResultedData;
                        DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0} >= {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));
                        if (resolvedRows != null)
                        {
                            RMMCountPanelResponse obj9 = GetResponseObject(dpProperty, UGITUtility.ObjectToString(resolvedRows.Count()), "resolvedtickets");
                            lstList.Add(obj9);
                        }
                    }
                }

                if (tabs != null)
                {
                    lstList = lstList.Where(x => tabs.Contains(x.Name)).OrderBy(x => x.Order).ToList();
                }


                return Ok(lstList);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRMMPanelCount: " + ex);
                return InternalServerError();
            }
        }
        private static RMMCountPanelResponse GetResponseObject(DashboardPanelProperty dpProperty, string value, string kpiname)
        {
            RMMCountPanelResponse obj3 = new RMMCountPanelResponse();
            try
            {
                obj3.Name = kpiname;
                obj3.Value = value;
                obj3.Text = $"{UGITUtility.ObjectToString(dpProperty.KPIList.FirstOrDefault(x => x.KpiName == obj3.Name)?.KpiDisplayName)} <b>({obj3.Value})</b>";
                obj3.Status = kpiname;
                obj3.Order = UGITUtility.StringToInt(dpProperty.KPIList.FirstOrDefault(x => x.KpiName == obj3.Name)?.Order);
                obj3.HideIcon = UGITUtility.StringToBoolean(dpProperty.KPIList.FirstOrDefault(x => x.KpiName == obj3.Name)?.HideIcon);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetResponseObject: " + ex);
            }
            return obj3;
        }
        [Route("GetProjectPendingAllocation")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetProjectPendingAllocation()
        {
            await Task.FromResult(0);
            DataTable dtResult = GetTableDataManager.GetTableDataUsingQuery($"ProjectPendingAllocation @TenantId='{_applicationContext.TenantID}'");
            Dictionary<string, string> modulePaths = new Dictionary<string, string>();
            ModuleViewManager moduleMgr = new ModuleViewManager(_applicationContext);
            List<UGITModule> uGITModules = new List<UGITModule>();
            try
            {
                uGITModules = moduleMgr.Load(x => x.ModuleName == "CPR");
                foreach (var module in uGITModules)
                {
                    modulePaths.Add(module.ModuleName, module.StaticModulePagePath);
                }
                var projects = dtResult.Select().Select(x => new
                {
                    Id = Convert.ToInt32(x["Id"]),
                    TicketURL = modulePaths[uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(x[DatabaseObjects.Columns.TicketId]))],
                    TicketId = x[DatabaseObjects.Columns.TicketId],
                    Image = x[DatabaseObjects.Columns.IconBlob],
                    Name = x[DatabaseObjects.Columns.Title].ToString(),
                    CloseDate = x[DatabaseObjects.Columns.CloseDate] == DBNull.Value ? Convert.ToDateTime(x[DatabaseObjects.Columns.Modified]) : Convert.ToDateTime(x[DatabaseObjects.Columns.CloseDate])
                }).OrderByDescending(p => p.CloseDate).Take(5).ToList();

                return Json(projects);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetProjectPendingAllocation: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetUserOpenProject")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserOpenProject()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                UserProfileManager umanager = new UserProfileManager(_applicationContext);
                var role = umanager.GetUserRoles(currentUser.Id).Select(x => x.Id).ToList();
                var userid = currentUser.Id;
                DataTable dtResult = new DataTable();
                Dictionary<string, string> modulePaths = new Dictionary<string, string>();

                ModuleViewManager moduleMgr = new ModuleViewManager(_applicationContext);
                TicketManager ticketMgr = new TicketManager(_applicationContext);
                List<UGITModule> uGITModules = new List<UGITModule>();
                List<DataTable> result = new List<DataTable>();
                ModuleStatistics mod = new ModuleStatistics(_applicationContext);
                List<ModuleStatisticResponse> resultList = new List<ModuleStatisticResponse>();
                uGITModules = moduleMgr.Load(x => x.ModuleType == ModuleType.Project);
                //List<string> projectModules = new List<string>() { ModuleNames.CPR, ModuleNames.CNS, ModuleNames.OPM };
                //uGITModules = moduleMgr.Load(x => projectModules.Any(y => y == x.ModuleName)).ToList();
                //uGITModules = moduleMgr.Load(x => x.ModuleType == ModuleType.Project).ToList();
                foreach (var module in uGITModules)
                {
                    modulePaths.Add(module.ModuleName, module.StaticModulePagePath);
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "myopentickets", ModuleName = module.ModuleName, Status = TicketStatus.Open, UserID = currentUser.Id };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = mod.Load(statRequest, false);
                    resultList.Add(statistics);
                }
                foreach (var data in resultList)
                {
                    if (data.ResultedData != null)
                        dtResult.Merge(data.ResultedData);
                }
                var projects = dtResult.Select().Select(x => new
                {
                    Id = Convert.ToInt32(x["ID"]),
                    TicketURL = modulePaths[uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(x[DatabaseObjects.Columns.TicketId]))],
                    TicketId = x[DatabaseObjects.Columns.TicketId],
                    Image = x[DatabaseObjects.Columns.IconBlob],
                    Name = x[DatabaseObjects.Columns.Title].ToString(),
                    CloseDate = x[DatabaseObjects.Columns.CloseDate] == DBNull.Value ? Convert.ToDateTime(x[DatabaseObjects.Columns.Modified]) : Convert.ToDateTime(x[DatabaseObjects.Columns.CloseDate])
                }).OrderByDescending(p => p.CloseDate).ToList();

                return Json(projects);
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetUserOpenProject: " + ex);
            }
            return null;
        }
        [Route("GetUserProject")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserProject()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                UserProfileManager umanager = new UserProfileManager(_applicationContext);
                var role = umanager.GetUserRoles(currentUser.Id).Select(x => x.Id).ToList();
                var userid = currentUser.Id;
                DataTable dtResult = new DataTable();
                Dictionary<string, string> modulePaths = new Dictionary<string, string>();

                ModuleViewManager moduleMgr = new ModuleViewManager(_applicationContext);
                TicketManager ticketMgr = new TicketManager(_applicationContext);
                List<UGITModule> uGITModules = new List<UGITModule>();
                List<DataTable> result = new List<DataTable>();
                ModuleStatistics mod = new ModuleStatistics(_applicationContext);
                List<ModuleStatisticResponse> resultList = new List<ModuleStatisticResponse>();
                uGITModules = moduleMgr.Load(x => x.ModuleType == ModuleType.Project);
                foreach (var module in uGITModules)
                {
                    modulePaths.Add(module.ModuleName, module.StaticModulePagePath);
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "allclosedtickets", ModuleName = module.ModuleName, Status = TicketStatus.Closed, UserID = userid };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = mod.Load(statRequest, false);
                    resultList.Add(statistics);
                }
                foreach (var data in resultList)
                {
                    dtResult.Merge(data.ResultedData);
                }


                var projects = dtResult.Select().Select(x => new
                {
                    Id = Convert.ToInt32(x["Id"]),
                    TicketURL = modulePaths[uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(x[DatabaseObjects.Columns.TicketId]))],
                    TicketId = x[DatabaseObjects.Columns.TicketId],
                    Image = x[DatabaseObjects.Columns.IconBlob],
                    Name = x[DatabaseObjects.Columns.Title].ToString(),
                    CloseDate = x[DatabaseObjects.Columns.CloseDate] == DBNull.Value ? Convert.ToDateTime(x[DatabaseObjects.Columns.Modified]) : Convert.ToDateTime(x[DatabaseObjects.Columns.CloseDate])
                }).OrderByDescending(p => p.CloseDate).Take(5).ToList();

                return Json(projects);
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetUserProject: " + ex);
            }
            return null;
        }

        [Route("GetUserWelcomeMessage")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserWelcomeMessage(string ViewID)
        {
            await Task.FromResult(0);
            try
            {
                var userInfoMessage = new List<string>();
                var currentUser = _applicationContext.CurrentUser;
                UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                await Task.FromResult(currentUser.UserName);
                UserProfile user = _applicationContext.UserManager.GetUserById(currentUser.Id);

                DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(_applicationContext);
                int viewID = UGITUtility.StringToInt(ViewID);
                DashboardPanelView View = objDashboardPanelViewManager.LoadViewByID(viewID, true);
                CommonDashboardsView commonViewObj = View.ViewProperty == null ? new CommonDashboardsView() : (View.ViewProperty as CommonDashboardsView);
                DashboardPanelProperty dpProperty = commonViewObj.Dashboards.FirstOrDefault(x => x.DashboardSubType == "UserWelcomePanel");

                UGITTaskManager taskManager = new UGITTaskManager(_applicationContext);
                //List<string> projecttypemodules = ModuleManager.Load(x => x.ModuleType == ModuleType.Project).Select(x => x.ModuleName).ToList();
                List<string> projecttypemodules = new List<string>() { ModuleNames.CPR, ModuleNames.CNS, ModuleNames.OPM };
                List<UGITTask> lstCriticalTasks = taskManager.Load(x => !string.IsNullOrEmpty(x.AssignedTo) && x.AssignedTo.Contains(currentUser.Id) && projecttypemodules.Contains(x.ModuleNameLookup)).OrderByDescending(x => x.Modified).ToList();
                if (lstCriticalTasks != null)
                    lstCriticalTasks = lstCriticalTasks.Take(2).ToList();
                string welcomeMessage = $"Welcome<br>{currentUser.Name}<br>";
                string msg = string.Empty;
                string finalUserInfoMessage = string.Empty;
                bool HideMoreIcon = true;

                if (user.Picture.EndsWith("userNew.png"))
                    user.Picture = "/Content/Images/userimg.png";

                if (dpProperty.ShowProjectCountOnWelcomeScreen)
                {
                    DataTable dtResult = GetTableDataManager.GetTableDataUsingQuery($"GetMyProjectCount @TenantId='{_applicationContext.TenantID}',@UserId='{_applicationContext.CurrentUser.Id}'");
                    //DataTable dtResult = GetTableDataManager.GetTableDataUsingQuery($"GetUsersProjectCount @tenantid='{_applicationContext.TenantID}',@resource='{_applicationContext.CurrentUser.Id}', @includeClosedProjects=0");

                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        //HideMoreIcon = false;
                        msg = "You are allocated to ";
                        int projectsCount = Convert.ToInt32(dtResult.Rows.Count);
                        List<DataRow> titles = dtResult.Rows.Cast<System.Data.DataRow>().Take(2).ToList();
                        if (projectsCount > 1 || projectsCount == 0)
                            finalUserInfoMessage = $"{msg} <a>{projectsCount}</a> projects on <a>{titles[0][1]},{titles[1][1]}</a>";
                        else if (projectsCount == 1)
                            finalUserInfoMessage = $"{msg} <a>{projectsCount}</a> project on <a>{titles[0][1]}</a>";
                    }
                }
                else
                {
                    if (lstCriticalTasks != null && lstCriticalTasks.Count > 0)
                    {
                        HideMoreIcon = false;
                        msg = "Waiting on assigning resources<br> to ";
                        foreach (UGITTask task in lstCriticalTasks)
                        {
                            string paramlist = $"'{UGITUtility.ObjectToString(task.ID)}','{task.ModuleNameLookup}','{task.TicketId}','{task.Title}'";
                            userInfoMessage.Add($"<a class=\"moreIcon\" onclick=\"openTasklink({paramlist})\">{task.Title}</a>");
                        }
                        finalUserInfoMessage = msg + UGITUtility.ConvertListToString(userInfoMessage, Constants.Separator6);
                    }

                }

                if (lstCriticalTasks != null && lstCriticalTasks.Count > 0)
                    HideMoreIcon = false;

                var result = new
                {
                    Name = currentUser.UserName,
                    img = string.IsNullOrEmpty(user.Picture) ? "/Content/Images/userimg.png" : File.Exists(System.Web.HttpContext.Current.Server.MapPath(user.Picture)) ? user.Picture : "/Content/Images/userimg.png",
                    WelcomeMessage = welcomeMessage,
                    userInfoMessage = finalUserInfoMessage,
                    HideMoreIcon = HideMoreIcon
                };
                return Json(result);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserWelcomeMessage: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetAllOpenOpportunities")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllOpenOpportunities()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;

                ModuleViewManager moduleManger = new ModuleViewManager(_applicationContext);
                UGITModule module = moduleManger.LoadByName("OPM", true);
                if (module == null)
                    return Ok();
                TicketManager ticketManager = new TicketManager(_applicationContext);
                DataTable dtAllOpenOPM = ticketManager.GetOpenTickets(module);
                if (dtAllOpenOPM != null)
                {
                    var projects = dtAllOpenOPM.Select().Select(x => new
                    {
                        Id = Convert.ToInt32(x["Id"]),
                        TicketURL = module.StaticModulePagePath,
                        TicketId = x[DatabaseObjects.Columns.TicketId],
                        Image = x[DatabaseObjects.Columns.IconBlob],
                        Name = x["Title"].ToString(),
                        CloseDate = x[DatabaseObjects.Columns.Modified]
                    }).OrderByDescending(p => p.CloseDate).Take(4).ToList();

                    return Json(projects);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetAllOpenOpportunities: " + ex);
            }
            return null;
        }

        [Route("GetAllOpenProjects")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllOpenProjects()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                Dictionary<string, string> modulePaths = new Dictionary<string, string>();
                ModuleViewManager moduleManger = new ModuleViewManager(_applicationContext);
                List<UGITModule> listModule = moduleManger.Load(x => x.ModuleType == ModuleType.Project);
                TicketManager ticketManager = new TicketManager(_applicationContext);
                DataTable dtAllOpenProjects = new DataTable();
                foreach (UGITModule module in listModule)
                {
                    modulePaths.Add(module.ModuleName, module.StaticModulePagePath);
                    DataTable dtAllOpenmoduletickets = ticketManager.GetOpenTickets(module);
                    dtAllOpenProjects.Merge(dtAllOpenmoduletickets);
                }

                if (dtAllOpenProjects != null)
                {
                    var projects = dtAllOpenProjects.Select().Select(x => new
                    {
                        Id = Convert.ToInt32(x["Id"]),
                        TicketURL = modulePaths[uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(x[DatabaseObjects.Columns.TicketId]))],
                        TicketId = x[DatabaseObjects.Columns.TicketId],
                        Image = x[DatabaseObjects.Columns.IconBlob],
                        Name = x["Title"].ToString(),
                        CloseDate = x[DatabaseObjects.Columns.Modified]
                    }).OrderByDescending(p => p.CloseDate).Take(4).ToList();

                    return Json(projects);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetAllOpenProjects: " + ex);
            }
            return null;
        }

        [AllowAnonymous]
        [Route("GetUserChartData")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserChartData(string mode, string studio = null, string division = null)
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", currentUser.TenantID);
                arrParams.Add("Mode", mode);
                arrParams.Add("studio", studio);
                arrParams.Add("division", division);
                string procName = string.Empty;

                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("GetDivisionChartData", arrParams);
                if (!string.IsNullOrWhiteSpace(mode))
                {
                    var result = dtResultBillings.Select().Select(x => new
                    {
                        Name = x["Name"].ToString(),
                        Value = Convert.ToInt32(x["Value"]),
                    }).OrderBy(x => x.Name).ToList();

                    return Json(result);

                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserChartData: " + ex);
            }
            return Json("");
        }

        [AllowAnonymous]
        [Route("GetSectorChartData")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSectorChartData(string mode, string studio = null, string division = null)
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", currentUser.TenantID);
                arrParams.Add("Mode", mode);
                arrParams.Add("studio", studio);
                arrParams.Add("division", division);
                string procName = string.Empty;

                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("GetSectorChartData", arrParams);
                if (!string.IsNullOrWhiteSpace(mode))
                {
                    var result = dtResultBillings.Select().Select(x => new
                    {
                        Name = x["Name"].ToString(),
                        Value = Convert.ToInt32(x["Value"]),
                    }).OrderBy(x => x.Name).ToList();

                    return Json(result);

                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetSectorChartData: " + ex);
            }
            return Json("");
        }
        private string getFormat(string format)
        {
            if (format.ToLower() == "default")
            {
                labelFormat = "millions";
                return string.Empty;
            }

            if (format.ToLower() == "currency")
            {
                labelFormat = "currency";
                return labelFormat;
            }
            if (format == "Min to Days")
            {
                // process your code here
            }
            return "millions";
        }
        [Route("GetUserChartDetails")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUserChartDetails(string ViewID, string PanelId)
        {
            await Task.FromResult(0);
            //var messageCode = 0;
            var expressionCalc = new ExpressionCalc(_applicationContext);
            try
            {
                var message = string.Empty;
                int.TryParse(PanelId, out var panelId);
                int.TryParse(ViewID, out var dashboardViewId);
                var dashboardManager = new DashboardManager(_applicationContext);
                var dashboardPanelViewManager = new DashboardPanelViewManager(_applicationContext);
                var dashboardPanelView = dashboardPanelViewManager.LoadViewByID(dashboardViewId, true);
                //var isSideBar = dashboardPanelView?.ViewProperty is SideDashboardView;

                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                if (panelId > 0)
                {
                    var dashboard = dashboardManager.LoadPanelById(panelId, true);

                    if (dashboard != null)
                    {
                        var devxChartHelper = new DevxChartHelper(_applicationContext);
                        ChartSetting chartSetting = dashboard.panel as ChartSetting;
                        devxChartHelper.ChartTitle = dashboard.Title;
                        devxChartHelper.ChartSetting = chartSetting;
                        dynamic output = new ExpandoObject();
                        output.dashboardTitle = dashboard.Title;

                        output.aoData = devxChartHelper.GetChart(false, dashboard, dashboardViewId, panelId, 900, 900, false);
                        var IsCacheChart = chartSetting.IsCacheChart;

                        List<Series> series = new List<Series>();
                        foreach (var expresion in chartSetting.Expressions)
                        {
                            var dimensonTitle = expresion.Dimensions.FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(dimensonTitle) && chartSetting.Dimensions.Count > 0)
                            {
                                var dimenson = chartSetting.Dimensions.FirstOrDefault(x => x.Title == dimensonTitle);
                                series.Add(new Series()
                                {
                                    name = expresion.Title,
                                    valueField = expresion.FunctionExpression,
                                    argumentField = dimenson.SelectedField,
                                    type = expresion.ChartType.ToLower(),
                                    label = new { indentFromAxis = 10, staggeringSpacing = 20, visible = !expresion.HideLabel, format = new { type = getFormat(expresion.LabelFormat), precision = 1 }, overlappingBehavior = "rotate" },
                                    //axis = dimenson.Title
                                });
                            }
                        }
                        output.valueAxis = chartSetting.Dimensions.Select(x => new
                        {
                            name = x.Title,
                            position = "left",
                            tickInterval = 300,
                            inverted = false,
                            allowDecimals = false,
                            valueType = "numeric",
                            showZero = true,
                            grid = new { visible = true },
                            label = new { indentFromAxis = 10, staggeringSpacing = 20, visible = true, format = new { type = labelFormat, precision = 1 }, overlappingBehavior = "rotate" },
                        });
                        output.series = series;
                        output.textFormat = labelFormat;
                        output.legend = new { visible = !chartSetting.HideLegend, verticalAlignment = chartSetting.VerticalAlignment, horizontalAlignment = chartSetting.HorizontalAlignment };
                        output.setting = new { };
                        //output.series = chartSetting.Expressions.Select(x => new
                        //{
                        //    type = x.ChartType.ToLower(),
                        //    name = x.Title,
                        //    valueField = x.FunctionExpression,
                        //    argumentField = chartSetting.Dimensions.FirstOrDefault(p => p.Title == x.Dimensions[0]).SelectedField
                        //    axis = chartSetting.Dimensions.FirstOrDefault(p => p.Title == x.Dimensions[0]).Title
                        //    //color = x.LabelColor
                        //}); 
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(output);

                        return Ok(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex.Message, "GetUserChartDetails- panel id not found");
            }
            return null;
        }

        [HttpGet]
        [Route("GetUserHelpCards")]
        public async Task<IHttpActionResult> GetUserHelpCards(string ViewID)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UserProfileManager userManager = new UserProfileManager(context);
                DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(context);
                HelpCardManager helpCardManager = new HelpCardManager(context);
                HelpCardContentManager helpCardContentManager = new HelpCardContentManager(context);

                int viewID = UGITUtility.StringToInt(ViewID);
                DashboardPanelView View = objDashboardPanelViewManager.LoadViewByID(viewID, true);
                CommonDashboardsView commonViewObj = View.ViewProperty == null ? new CommonDashboardsView() : (View.ViewProperty as CommonDashboardsView);
                DashboardPanelProperty dpProperty = commonViewObj.Dashboards.FirstOrDefault(x => x.DashboardSubType == "HelpCardsPanel");
                List<HelpCardResponse> helpCardResponse = new List<HelpCardResponse>();

                List<string> tickets = UGITUtility.SplitString(dpProperty.HelpCards, new string[] { Constants.Separator6, Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //var listHelpCard = helpCardManager.Load(x => x.Deleted == false && tickets.Contains(x.TicketId)).Select(x => new { x.TicketId, x.Title, x.Description }).ToList();
                var listHelpCard = helpCardManager.Load(x => x.Deleted == false && tickets.Contains(x.TicketId)).ToList();

                if (listHelpCard.Count > 0)
                {
                    listHelpCard.ForEach(x =>
                    {
                        var user = userManager.LoadById(x.CreatedBy);
                        if (user != null)
                            x.CreatedBy = user.Name;
                    });
                    foreach (var article in listHelpCard)
                    {
                        var obj = new HelpCardResponse();
                        obj.ID = article.ID;
                        obj.CreatedBy = article.CreatedBy;
                        obj.Created = Convert.ToDateTime(article.Created).ToString("MMM-dd-yyyy hh:mm tt");
                        obj.HelpCardTitle = article.Title;
                        obj.HelpCardTicketId = article.TicketId;
                        obj.AgentLookUp = article.AgentLookUp;
                        obj.HelpCardCategory = article.Category;
                        obj.Description = article.Description;
                        var helpCardContent = helpCardContentManager.Load(x => x.TicketId == article.TicketId).FirstOrDefault();
                        if (helpCardContent != null)
                        {
                            obj.AgentContent = helpCardContent.AgentContent;
                            obj.HelpCardContent = helpCardContent.Content;
                        }
                        helpCardResponse.Add(obj);
                    }

                    if (helpCardResponse != null)
                    {
                        string jsonmodules = JsonConvert.SerializeObject(helpCardResponse);
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }
                }

                return Ok(helpCardResponse);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserHelpCards: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetCriticalTasks")]
        public async Task<IHttpActionResult> GetCriticalTasks()
        {
            await Task.FromResult(0);
            try
            {
                UGITTaskManager taskManager = new UGITTaskManager(_applicationContext);
                List<CriticalTaskResponse> lstResponseObjs = new List<CriticalTaskResponse>();
                //List<string> lstprojectmodules = ModuleManager.Load(x => x.ModuleType == ModuleType.Project).Select(x => x.ModuleName).ToList();
                List<string> lstprojectmodules = new List<string>() { ModuleNames.CPR, ModuleNames.CNS, ModuleNames.OPM };
                List<UGITTask> lstCriticalTasks = taskManager.Load(x => !string.IsNullOrEmpty(x.AssignedTo) && x.AssignedTo.Contains(_applicationContext.CurrentUser.Id) && lstprojectmodules.Contains(x.ModuleNameLookup)).OrderByDescending(x => x.Modified).ToList();
                //List<UGITTask> lstCriticalTasks = taskManager.Load(x => string.IsNullOrEmpty(x.AssignedTo) && x.IsCritical == true && lstprojectmodules.Contains(x.ModuleNameLookup)).OrderBy(x => x.Modified).ToList();
                foreach (UGITTask task in lstCriticalTasks)
                {
                    CriticalTaskResponse responseObj = new CriticalTaskResponse();
                    string modulename = task.ModuleNameLookup;
                    UGITModule module = ModuleManager.GetByName(modulename);
                    if (module == null)
                        continue;
                    DataRow currentticket = Ticket.GetCurrentTicket(_applicationContext, modulename, task.TicketId);
                    if (currentticket != null)
                    {
                        responseObj.ProjectTitle = UGITUtility.ObjectToString(currentticket[DatabaseObjects.Columns.ProjectID]) + ": "
                            + UGITUtility.ObjectToString(currentticket[DatabaseObjects.Columns.Title]) + ": " + task.TicketId;
                    }
                    responseObj.TicketId = task.TicketId;
                    responseObj.ID = task.ID;
                    responseObj.ModuleNameLookup = task.ModuleNameLookup;
                    responseObj.Title = task.Title;
                    lstResponseObjs.Add(responseObj);
                }
                return Ok(lstResponseObjs);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetCriticalTasks: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetDivisions")]
        public async Task<IHttpActionResult> GetDivisions(string OnlyEnabled = "0")
        {
            await Task.FromResult(0);
            try
            {
                StudioManager studioManager = new StudioManager(_applicationContext);
                CompanyDivisionManager divisionManager = new CompanyDivisionManager(_applicationContext);
                List<CompanyDivision> lstDivisions = null;
                if (OnlyEnabled == "1")
                    lstDivisions = divisionManager.Load(x => !x.Deleted);
                else
                    lstDivisions = divisionManager.Load();
                CompanyManager companyManager = new CompanyManager(_applicationContext);
                List<Company> lstcompanies = companyManager.Load();
                if (lstDivisions != null)
                    lstDivisions = lstDivisions.OrderBy(x => x.Title).ToList();
                if (lstcompanies.Count > 0)
                {
                    List<CompanyDivision> distinctDivisions = lstDivisions.Where(x => x.CompanyIdLookup == lstcompanies.First().ID).ToList();
                    return Ok(distinctDivisions);
                }

                return Ok(lstDivisions);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDivisions: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetStudios")]
        public async Task<IHttpActionResult> GetStudios(string division)
        {
            await Task.FromResult(0);
            try
            {
                StudioManager studioManager = new StudioManager(_applicationContext);
                bool EnableStudioDivisionHierarchy = UGITUtility.StringToBoolean(_configVariableManager.GetValue(ConfigConstants.EnableStudioDivisionHierarchy));

                List<Studio> selectedStudios = null;
                if (string.IsNullOrEmpty(division) || division == "0" || !EnableStudioDivisionHierarchy)
                    selectedStudios = studioManager.Load();
                else
                    selectedStudios = studioManager.Load(x => x.DivisionLookup == UGITUtility.StringToLong(division));
                if (selectedStudios != null)
                {
                    selectedStudios = selectedStudios.OrderBy(x => x.Title).ToList();
                    return Ok(selectedStudios);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetStudios: " + ex);
                return InternalServerError();
            }
        }

        public static RMMCountPanelResponse GetResponseObjectFromStat(Dictionary<string, int> stat, DashboardPanelProperty dpProperty, string kpiName)
        {
            RMMCountPanelResponse obj = new RMMCountPanelResponse();
            try
            {
                obj.Name = kpiName;
                obj.Value = UGITUtility.ObjectToString(UGITUtility.ObjectToString(stat[kpiName]));
                obj.Text = $"{UGITUtility.ObjectToString(dpProperty.KPIList.FirstOrDefault(x => x.KpiName == obj.Name)?.KpiDisplayName)}  <b>({obj.Value})</b>";
                obj.Status = kpiName;
                obj.Order = UGITUtility.StringToInt(dpProperty.KPIList.FirstOrDefault(x => x.KpiName == obj.Name)?.Order);
                obj.HideIcon = UGITUtility.StringToBoolean(dpProperty.KPIList.FirstOrDefault(x => x.KpiName == obj.Name)?.HideIcon);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetResponseObjectFromStat: " + ex);
            }
            return obj;
        }

        [Route("GetAllWaitingOnMe")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAllWaitingOnMe()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                DataTable dtResult = new DataTable();
                ModuleStatistics modStatistics = new ModuleStatistics(_applicationContext);
                Dictionary<string, string> modulePaths = new Dictionary<string, string>();
                {
                    List<UGITModule> uGITModules = new List<UGITModule>();
                    List<DataTable> result = new List<DataTable>();
                    List<ModuleStatisticResponse> resultList = new List<ModuleStatisticResponse>();
                    uGITModules = ModuleManager.Load(x => x.ModuleType != ModuleType.Governance && x.EnableModule == true);
                    //uGITModules = ModuleManager.LoadAllModule();
                    foreach (var mObj in uGITModules)
                    {
                        modulePaths.Add(mObj.ModuleName, mObj.StaticModulePagePath);
                        ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "waitingonme", ModuleName = mObj.ModuleName, Status = TicketStatus.Open, UserID = currentUser.Id };
                        ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                        statistics = modStatistics.Load(statRequest, false);
                        resultList.Add(statistics);
                        //dtResult.Merge(resultList[0].ResultedData);
                    }
                    foreach (var data in resultList)
                    {
                        try
                        {
                            if (data.ResultedData != null)
                                dtResult.Merge(data.ResultedData);
                        }
                        catch (Exception ex)
                        {
                            //ULog.WriteException(ex);
                            ULog.WriteException($"An Exception Occurred in GetAllWaitingOnMe: " + ex);
                        }

                    }
                }
                if (dtResult != null)
                {
                    var projects = dtResult.Select().Select(x => new
                    {
                        Id = Convert.ToInt32(x["Id"]),
                        TicketURL = modulePaths[uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(x[DatabaseObjects.Columns.TicketId]))],
                        TicketId = x[DatabaseObjects.Columns.TicketId],
                        Image = x[DatabaseObjects.Columns.IconBlob],
                        Name = x["Title"].ToString(),
                        CloseDate = x[DatabaseObjects.Columns.Modified]
                    }).OrderByDescending(p => p.CloseDate).Take(4).ToList();

                    return Json(projects);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetAllWaitingOnMe: " + ex);
            }
            return null;
        }

        [Route("GetNPRTickets")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetNPRTickets()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                DataTable dtResult = new DataTable();
                ModuleStatistics modStatistics = new ModuleStatistics(_applicationContext);
                UGITModule nprmodule = ModuleManager.LoadByName(ModuleNames.NPR, true);
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "nprtickets", ModuleName = ModuleNames.NPR, Status = TicketStatus.Open, UserID = currentUser.Id };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = modStatistics.Load(statRequest, false);
                    dtResult = statistics.ResultedData;
                }
                if (dtResult != null)
                {
                    var projects = dtResult.Select().Select(x => new
                    {
                        Id = Convert.ToInt32(x["Id"]),
                        TicketURL = nprmodule.StaticModulePagePath,
                        TicketId = x[DatabaseObjects.Columns.TicketId],
                        Image = x[DatabaseObjects.Columns.IconBlob],
                        Name = x["Title"].ToString(),
                        CloseDate = x[DatabaseObjects.Columns.Modified]
                    }).OrderByDescending(p => p.CloseDate).Take(4).ToList();

                    return Json(projects);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetAllWaitingOnMe: " + ex);
                //ULog.WriteException(ex);
            }
            return null;
        }

        [Route("GetResolvedTickets")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetResolvedTickets()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                ModuleStatistics modStatistics = new ModuleStatistics(_applicationContext);
                DataTable dtResult = new DataTable();
                UGITModule nprmodule = ModuleManager.LoadByName(ModuleNames.TSR, true);

                LifeCycle lifeCycle = nprmodule.List_LifeCycles.FirstOrDefault();
                LifeCycleStage resolvedStage = null;
                if (lifeCycle != null)
                    resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTitle == StageType.Resolved.ToString() || x.StageTypeChoice == StageType.Resolved.ToString());
                // Added condition above, for CPR Under Construction/ Resolved tab
                if (resolvedStage != null)
                {
                    ModuleStatisticRequest statRequest = new ModuleStatisticRequest() { CurrentTab = "myopentickets", ModuleName = ModuleNames.TSR, Status = TicketStatus.Open, UserID = currentUser.Id };
                    ModuleStatisticResponse statistics = new ModuleStatisticResponse();
                    statistics = modStatistics.Load(statRequest, false);
                    DataTable openTicketsTable = new DataTable();
                    if (statistics != null)
                        openTicketsTable = statistics.ResultedData;
                    if (openTicketsTable != null)
                    {
                        DataRow[] resolvedRows = openTicketsTable.Select(string.Format("{0} >= {1}", DatabaseObjects.Columns.StageStep, resolvedStage.StageStep));
                        if (resolvedRows != null && resolvedRows.Length > 0)
                        {
                            dtResult = resolvedRows.CopyToDataTable();
                        }
                    }
                }
                if (dtResult != null)
                {
                    var projects = dtResult.Select().Select(x => new
                    {
                        Id = Convert.ToInt32(x["Id"]),
                        TicketURL = nprmodule.StaticModulePagePath,   // modulePaths[uHelper.getModuleNameByTicketId(UGITUtility.ObjectToString(x[DatabaseObjects.Columns.TicketId]))],
                        TicketId = x[DatabaseObjects.Columns.TicketId],
                        Image = x[DatabaseObjects.Columns.IconBlob],
                        Name = x["Title"].ToString(),
                        CloseDate = x[DatabaseObjects.Columns.Modified]
                    }).OrderByDescending(p => p.CloseDate).Take(4).ToList();

                    return Json(projects);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetAllWaitingOnMe: " + ex);
            }
            return null;
        }

        [Route("GetRoleWiseAllocations")]
        public async Task<IHttpActionResult> GetRoleWiseAllocations()
        {
            try
            {
                await Task.FromResult(0);
                var currentUser = _applicationContext.CurrentUser;
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", currentUser.TenantID);
                DataTable dtroleAllocations = uGITDAL.ExecuteDataSetWithParameters("GetRoleWiseAllocations", arrParams);
                return Ok(dtroleAllocations);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRoleWiseAllocations: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetSalesTargetBySector")]
        public async Task<IHttpActionResult> GetSalesTargetBySector()
        {
            await Task.FromResult(0);
            ModuleViewManager _moduleViewManager = new ModuleViewManager(_applicationContext);
            TicketManager _ticketManager = new TicketManager(_applicationContext);
            UGITModule opmModuleObj = _moduleViewManager.LoadByName(ModuleNames.OPM);
            UGITModule cprModuleObj = _moduleViewManager.LoadByName(ModuleNames.CPR);
            DataTable dtOPMOpenTickets = _ticketManager.GetOpenTickets(opmModuleObj);
            DataTable dtCPRAwardedTickets = _ticketManager.GetTickets($"Select * from CRMProject where {DatabaseObjects.Columns.StageStep} <= 7 and {DatabaseObjects.Columns.ApproxContractValue} is not null && {DatabaseObjects.Columns.ApproxContractValue} !='' ");
            List<GaugeChartResponse> lstResponse = new List<GaugeChartResponse>();
            List<GaugeChartResponse> lstresult = new List<GaugeChartResponse>();

            try
            {
                dtOPMOpenTickets.Merge(dtCPRAwardedTickets);
                DataTable lstSectorwise = dtOPMOpenTickets.AsEnumerable()
                  .GroupBy(r => r.Field<string>("SectorChoice"))
                  .Select(g =>
                  {
                      var row = dtOPMOpenTickets.NewRow();
                      row["SectorChoice"] = g.Key;
                      row["ApproxContractValue"] = g.Sum(r => r.Field<double>("ApproxContractValue"));

                      return row;
                  }).CopyToDataTable();

                foreach (DataRow row in lstSectorwise.Rows)
                {
                    GaugeChartResponse newObj = new GaugeChartResponse()
                    {
                        Name = UGITUtility.ObjectToString(row["SectorChoice"]),
                        Value = UGITUtility.StringToDouble(row["ApproxContractValue"]),
                        DataType = "Sector"
                    };
                    lstResponse.Add(newObj);
                }

                List<GaugeChartResponse> lsttop = lstResponse.OrderByDescending(x => x.Value).Take(5).ToList();
                List<GaugeChartResponse> lstlast = lstResponse.OrderBy(y => y.Value).Take(lstResponse.Count - 5).ToList();

                lstresult.Add(new GaugeChartResponse()
                {
                    Name = "Others",
                    Value = lstlast.Sum(x => x.Value),
                    DataType = "Sector"
                });
                lstresult.AddRange(lsttop);
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetSalesTargetBySector: " + ex);
            }

            return Ok(lstresult.OrderByDescending(x => x.Value));
        }

        [Route("GetSalesTargetByOtherSector")]
        public async Task<IHttpActionResult> GetSalesTargetByOtherSector()
        {
            await Task.FromResult(0);
            ModuleViewManager _moduleViewManager = new ModuleViewManager(_applicationContext);
            TicketManager _ticketManager = new TicketManager(_applicationContext);
            UGITModule opmModuleObj = _moduleViewManager.LoadByName(ModuleNames.OPM);
            DataTable dtOPMOpenTickets = _ticketManager.GetOpenTickets(opmModuleObj);
            List<GaugeChartResponse> lstResponse = new List<GaugeChartResponse>();
            List<GaugeChartResponse> lstresult = new List<GaugeChartResponse>();

            try
            {
                DataTable lstSectorwise = dtOPMOpenTickets.AsEnumerable()
                  .GroupBy(r => r.Field<string>("SectorChoice"))
                  .Select(g =>
                  {
                      var row = dtOPMOpenTickets.NewRow();
                      row["SectorChoice"] = g.Key;
                      row["ApproxContractValue"] = g.Sum(r => r.Field<double>("ApproxContractValue"));

                      return row;
                  }).CopyToDataTable();

                foreach (DataRow row in lstSectorwise.Rows)
                {
                    GaugeChartResponse newObj = new GaugeChartResponse()
                    {
                        Name = UGITUtility.ObjectToString(row["SectorChoice"]),
                        Value = UGITUtility.StringToDouble(row["ApproxContractValue"]),
                        DataType = "Sector"
                    };
                    lstResponse.Add(newObj);
                }

                lstresult = lstResponse.OrderBy(y => y.Value).Take(lstResponse.Count - 5).ToList();

            }
            catch (Exception ex)
            {
                // ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetSalesTargetByOtherSector: " + ex);
            }

            return Ok(lstresult.OrderByDescending(x => x.Value));
        }

        [Route("GetSalesTargetByDivision")]
        public async Task<IHttpActionResult> GetSalesTargetByDivision(string division)
        {
            await Task.FromResult(0);
            ModuleViewManager _moduleViewManager = new ModuleViewManager(_applicationContext);
            TicketManager _ticketManager = new TicketManager(_applicationContext);
            UGITModule opmModuleObj = _moduleViewManager.LoadByName(ModuleNames.OPM);
            DataTable dtOPMOpenTickets = _ticketManager.GetOpenTickets(opmModuleObj);
            List<GaugeChartResponse> lstResponse = new List<GaugeChartResponse>();

            try
            {
                DataTable lstSectorwise = dtOPMOpenTickets.AsEnumerable()
                    .Where(x => x.Field<string>(DatabaseObjects.Columns.BCCISector) == division)
                  .GroupBy(r => r.Field<string>(DatabaseObjects.Columns.DivisionLookup))
                  .Select(g =>
                  {
                      var row = dtOPMOpenTickets.NewRow();
                      row[DatabaseObjects.Columns.DivisionLookup] = g.Key;
                      row["ApproxContractValue"] = g.Sum(r => r.Field<double>("ApproxContractValue"));

                      return row;
                  }).CopyToDataTable();

                foreach (DataRow row in lstSectorwise.Rows)
                {
                    GaugeChartResponse newObj = new GaugeChartResponse()
                    {
                        Name = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.DivisionLookup]),
                        Value = UGITUtility.StringToDouble(row["ApproxContractValue"]),
                        DataType = "Division"
                    };
                    lstResponse.Add(newObj);
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetSalesTargetByDivision: " + ex);
            }

            return Ok(lstResponse.OrderByDescending(x => x.Value).Take(5));
        }

        [Route("GetSalesByCommittedDivision")]
        public async Task<IHttpActionResult> GetSalesByDivision()
        {
            await Task.FromResult(0);

            ModuleViewManager _moduleViewManager = new ModuleViewManager(_applicationContext);
            TicketManager _ticketManager = new TicketManager(_applicationContext);
            UGITModule cprModuleObj = _moduleViewManager.LoadByName(ModuleNames.CPR);
            DataTable dtCPROpenTickets = _ticketManager.GetOpenTickets(cprModuleObj);
            List<GaugeChartResponse> lstResponse = new List<GaugeChartResponse>();

            List<GaugeChartResponse> lsttop = new List<GaugeChartResponse>();
            List<GaugeChartResponse> lstlast = new List<GaugeChartResponse>();

            List<GaugeChartResponse> lstresult = new List<GaugeChartResponse>();
            try
            {
                DataTable lstSectorwise = dtCPROpenTickets.AsEnumerable()
                                        .Where(x => x.Field<string>(DatabaseObjects.Columns.DivisionLookup) != string.Empty
                                        && x.Field<string>(DatabaseObjects.Columns.DivisionLookup) != null)
                  .GroupBy(r => r.Field<string>(DatabaseObjects.Columns.DivisionLookup))
                  .Select(g =>
                  {
                      var row = dtCPROpenTickets.NewRow();
                      row[DatabaseObjects.Columns.DivisionLookup] = g.Key;
                      row["ApproxContractValue"] = g.Sum(r => r.Field<double>("ApproxContractValue"));

                      return row;
                  }).CopyToDataTable();

                foreach (DataRow row in lstSectorwise.Rows)
                {
                    GaugeChartResponse newObj = new GaugeChartResponse()
                    {
                        Name = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.DivisionLookup]),
                        Value = UGITUtility.StringToDouble(row["ApproxContractValue"]),
                        DataType = "Division"
                    };
                    lstResponse.Add(newObj);
                }

                lsttop.AddRange(lstResponse.OrderByDescending(x => x.Value).Take(5));
                if (lstResponse.Count > 5)
                {
                    lstlast.AddRange(lstResponse.OrderBy(x => x.Value).Take(lstResponse.Count - 1));

                    lstresult.Add(new GaugeChartResponse()
                    {
                        Name = "Others",
                        Value = lstlast.Sum(x => x.Value),
                        DataType = "Division"
                    });
                }
                lstresult.AddRange(lsttop);
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetSalesByDivision: " + ex);
            }

            return Ok(lstresult.OrderByDescending(x => x.Value));
        }

        [Route("GetSalesByOtherDivision")]
        public async Task<IHttpActionResult> GetSalesByOtherDivision()
        {
            await Task.FromResult(0);
            ModuleViewManager _moduleViewManager = new ModuleViewManager(_applicationContext);
            TicketManager _ticketManager = new TicketManager(_applicationContext);
            UGITModule cprModuleObj = _moduleViewManager.LoadByName(ModuleNames.CPR);
            DataTable dtCPROpenTickets = _ticketManager.GetOpenTickets(cprModuleObj);
            List<GaugeChartResponse> lstResponse = new List<GaugeChartResponse>();

            try
            {
                DataTable lstSectorwise = dtCPROpenTickets.AsEnumerable()
                    .Where(x => x.Field<string>(DatabaseObjects.Columns.DivisionLookup) != string.Empty && x.Field<string>(DatabaseObjects.Columns.DivisionLookup) != null)
                  .GroupBy(r => r.Field<string>(DatabaseObjects.Columns.DivisionLookup))
                  .Select(g =>
                  {
                      var row = dtCPROpenTickets.NewRow();
                      row[DatabaseObjects.Columns.DivisionLookup] = g.Key;
                      row["ApproxContractValue"] = g.Sum(r => r.Field<double>("ApproxContractValue"));

                      return row;
                  }).CopyToDataTable();

                foreach (DataRow row in lstSectorwise.Rows)
                {
                    GaugeChartResponse newObj = new GaugeChartResponse()
                    {
                        Name = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.DivisionLookup]),
                        Value = UGITUtility.StringToDouble(row["ApproxContractValue"]),
                        DataType = "Division"
                    };
                    lstResponse.Add(newObj);
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetSalesByOtherDivision: " + ex);
            }

            return Ok(lstResponse.OrderBy(x => x.Value).Take(lstResponse.Count - 5));
        }

        [Route("GetCommittedSalesTargetBySector")]
        public async Task<IHttpActionResult> GetCommittedSalesTargetBySector(string division)
        {
            await Task.FromResult(0);
            ModuleViewManager _moduleViewManager = new ModuleViewManager(_applicationContext);
            TicketManager _ticketManager = new TicketManager(_applicationContext);
            UGITModule cprModuleObj = _moduleViewManager.LoadByName(ModuleNames.CPR);
            DataTable dtcprOpenTickets = _ticketManager.GetOpenTickets(cprModuleObj);
            List<GaugeChartResponse> lstResponse = new List<GaugeChartResponse>();

            try
            {
                DataTable lstSectorwise = dtcprOpenTickets.AsEnumerable()
                    .Where(x => x.Field<string>(DatabaseObjects.Columns.DivisionLookup) == division && x.Field<string>(DatabaseObjects.Columns.DivisionLookup) != string.Empty && x.Field<string>(DatabaseObjects.Columns.DivisionLookup) != null)
                  .GroupBy(r => r.Field<string>(DatabaseObjects.Columns.BCCISector))
                  .Select(g =>
                  {
                      var row = dtcprOpenTickets.NewRow();
                      row[DatabaseObjects.Columns.BCCISector] = g.Key;
                      row["ApproxContractValue"] = g.Sum(r => r.Field<double>("ApproxContractValue"));

                      return row;
                  }).CopyToDataTable();

                foreach (DataRow row in lstSectorwise.Rows)
                {
                    GaugeChartResponse newObj = new GaugeChartResponse()
                    {
                        Name = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.BCCISector]),
                        Value = UGITUtility.StringToDouble(row["ApproxContractValue"]),
                        DataType = "Sector"
                    };
                    lstResponse.Add(newObj);
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetCommittedSalesTargetBySector: " + ex);
            }

            return Ok(lstResponse.OrderByDescending(x => x.Value).Take(5));
        }


        [Route("GetTooltipData")]
        public async Task<IHttpActionResult> GetTooltipData(string datatype)
        {
            await Task.FromResult(0);
            List<ToolTipDataRecords> lstAllRecords = new List<ToolTipDataRecords>();
            List<ToolTipDataRecords> lstResponse = new List<ToolTipDataRecords>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {
                values.Add("@tenantID", _applicationContext.TenantID);
                if (datatype == "Division")
                {
                    DataTable dt = GetTableDataManager.GetData("DivisionSalesDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Division",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                if (datatype == "Sector")
                {
                    DataTable dt = GetTableDataManager.GetData("SectorSalesDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Sector",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }

                return Ok(lstAllRecords.OrderBy(x => x.Value));

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTooltipData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetRevenueTooltipData")]
        public async Task<IHttpActionResult> GetRevenueTooltipData(string datatype)
        {
            await Task.FromResult(0);
            List<ToolTipDataRecords> lstAllRecords = new List<ToolTipDataRecords>();
            List<ToolTipDataRecords> lstResponse = new List<ToolTipDataRecords>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {
                values.Add("@tenantID", _applicationContext.TenantID);
                if (datatype == "Division")
                {
                    DataTable dt = GetTableDataManager.GetData("DivisionRevenueDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Division",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                if (datatype == "Sector")
                {
                    DataTable dt = GetTableDataManager.GetData("SectorRevenueDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Sector",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }

                return Ok(lstAllRecords.OrderBy(x => x.Value));
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRevenueTooltipData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetChartSubData")]
        public async Task<IHttpActionResult> GetChartSubData(string type, string name)
        {
            await Task.FromResult(0);
            List<ToolTipDataRecords> lstAllRecords = new List<ToolTipDataRecords>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {
                values.Add("@tenantID", _applicationContext.TenantID);
                if (type == "division")
                {
                    values.Add("@Division", name);
                    DataTable dt = GetTableDataManager.GetData("SectorSalesDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Sector",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                if (type == "sector")
                {
                    values.Add("@Sector", name);
                    DataTable dt = GetTableDataManager.GetData("DivisionSalesDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Division",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                return Ok(lstAllRecords);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetChartSubData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetCommittedSubData")]
        public async Task<IHttpActionResult> GetCommittedSubData(string type, string name)
        {
            await Task.FromResult(0);
            List<ToolTipDataRecords> lstAllRecords = new List<ToolTipDataRecords>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {
                values.Add("@tenantID", _applicationContext.TenantID);
                if (type == "division")
                {
                    values.Add("@division", name);
                    DataTable dt = GetTableDataManager.GetData("SectorRevenueDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Sector",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                if (type == "sector")
                {
                    values.Add("@sector", name);
                    DataTable dt = GetTableDataManager.GetData("DivisionRevenueDetails", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstAllRecords.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                HotRevenue = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                Value = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Division",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                return Ok(lstAllRecords);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetCommittedSubData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetCommonChartData")]
        public async Task<IHttpActionResult> GetCommonChartData([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            List<ToolTipDataRecords> lstresponse = new List<ToolTipDataRecords>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {

                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);

                if (request.Base == "Division")
                {
                    if (request.Base == "Studio")
                    {
                        values.Add("@studio", request.Selection);
                    }
                    else
                        values.Add("@sector", request.Selection);
                    DataTable dt = GetTableDataManager.GetData("CommonDivisionChartData", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstresponse.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                Value = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                HotRevenue = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Division",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                else if (request.Base == "Sector" || (request.Base == "Studio" && !string.IsNullOrEmpty(request.Selection)))
                {
                    if (request.Base == "Studio")
                    {
                        values.Add("@studio", request.Selection);
                    }
                    else
                        values.Add("@division", request.Selection);
                    DataTable dt = GetTableDataManager.GetData("CommonSectorChartData", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstresponse.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                Value = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                HotRevenue = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Sector",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }
                else if (request.Base == "Studio")
                {
                    DataTable dt = GetTableDataManager.GetData("CommonStudioChartData", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstresponse.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                Value = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                HotRevenue = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Studio",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }

                if (request.Type == "Financial")
                    return Ok(lstresponse.OrderBy(x => x.Value));
                else
                    return Ok(lstresponse.OrderBy(x => x.ResourceCount));

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetCommonChartData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetStudioChartData")]
        public async Task<IHttpActionResult> GetStudioChartData(string filter)
        {
            await Task.FromResult(0);
            List<ToolTipDataRecords> lstresponse = new List<ToolTipDataRecords>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", filter);

                DataTable dt = GetTableDataManager.GetData("CommonStudioChartData", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new ToolTipDataRecords()
                        {
                            Name = UGITUtility.ObjectToString(row["Name"]),
                            ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                            HotProject = UGITUtility.StringToInt(row["hotproject"]),
                            Value = UGITUtility.StringToDouble(row["hotrevenue"]),
                            CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                            HotRevenue = UGITUtility.StringToDouble(row["committedrevenue"]),
                            PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                            DataType = "Division",
                            Utilization = UGITUtility.StringToDouble(row["Utilization"])
                        });
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetStudioChartData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetResourceChartDataNotUsed")]
        public async Task<IHttpActionResult> GetResourceChartDataNotUsed([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            List<ToolTipDataRecords> lstresponse = new List<ToolTipDataRecords>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);

                if (request.Base == "Division")
                {
                    string selection = string.Empty;
                    if (request.Base == "Studio")
                    {
                        values.Add("@studio", request.Selection);
                        selection = "Studio~" + request.Selection;
                    }
                    else
                    {
                        values.Add("@sector", request.Selection);
                        selection = "Sector~" + request.Selection;
                    }

                    DataTable dt = GetTableDataManager.GetData("DivisionChart_Resource", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {

                            lstresponse.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                Value = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                HotRevenue = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Division",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"]),
                                Selection = selection
                            });
                        }
                    }
                }
                else if (request.Base == "Sector" || (request.Base == "Studio" && !string.IsNullOrEmpty(request.Selection)))
                {
                    string selection = string.Empty;
                    if (request.Base == "Studio")
                    {
                        values.Add("@studio", request.Selection);
                        selection = "Studio~" + request.Selection;
                    }
                    else
                    {
                        values.Add("@division", request.Selection);
                        selection = "Division~" + request.Selection;
                    }
                    DataTable dt = GetTableDataManager.GetData("SectorChart_Resource", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstresponse.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                Value = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                HotRevenue = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Sector",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"]),
                                Selection = selection
                            });
                        }
                    }
                }
                else if (request.Base == "Studio")
                {
                    DataTable dt = GetTableDataManager.GetData("StudioChart_Resource", values);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            lstresponse.Add(new ToolTipDataRecords()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                                HotProject = UGITUtility.StringToInt(row["hotproject"]),
                                Value = UGITUtility.StringToDouble(row["hotrevenue"]),
                                CommittedProjects = UGITUtility.StringToInt(row["committedprojects"]),
                                HotRevenue = UGITUtility.StringToDouble(row["committedrevenue"]),
                                PastRevenue = UGITUtility.StringToDouble(row["pastrevenue"]),
                                DataType = "Studio",
                                Utilization = UGITUtility.StringToDouble(row["Utilization"])
                            });
                        }
                    }
                }

                if (request.Type == "Financial")
                    return Ok(lstresponse.OrderBy(x => x.Value));
                else
                    return Ok(lstresponse.OrderBy(x => x.ResourceCount));
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetResourceChartDataNotUsed: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetMonthWiseUtilizationChart")]
        public async Task<IHttpActionResult> GetMonthWiseUtilizationChart([FromUri] ResourceDetailChartRequest request)
        {
            await Task.FromResult(0);
            List<ResourceDetailChart> lstresponse = new List<ResourceDetailChart>();
            Dictionary<string, object> values = new Dictionary<string, object>();
            try
            {
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime aStartdate = UGITUtility.StringToDateTime(request.StartDate);
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate);
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else
                {
                    if (request.Period == "Now")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "3 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "6 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }

                List<string> selectionList = UGITUtility.ConvertStringToList(request.ParentSelection, Constants.Separator2);
                string selectionType = selectionList.First();
                if (selectionType == "Division")
                    values.Add("@division", selectionList.Last());
                else if (selectionType == "Sector")
                    values.Add("@sector", selectionList.Last());
                else if (selectionType == "Studio")
                    values.Add("@studio", selectionList.Last());

                if (request.Base == "Division")
                    values.Add("@division", request.ChildSelection);
                else if (request.Base == "Sector")
                    values.Add("@sector", request.ChildSelection);
                else if (request.Base == "Studio")
                    values.Add("@studio", request.ChildSelection);


                values.Add("@billable", request.Billable);
                DataTable dt = GetTableDataManager.GetData("MonthWiseUtilizationChart", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new ResourceDetailChart()
                        {
                            MonthStart = UGITUtility.ObjectToString(row["Months"]),
                            Utilization = UGITUtility.StringToInt(row["FTE"]),
                            Availability = Math.Abs(100 - UGITUtility.StringToInt(row["FTE"]))
                        });
                    }
                }


                return Ok(lstresponse);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetMonthWiseUtilizationChart: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetResourceRequired")]
        public async Task<IHttpActionResult> GetResourceRequired([FromUri] RecruitmentViewChartRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<RecruitmentViewResponse> lstResponse = new List<RecruitmentViewResponse>();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                if (!string.IsNullOrEmpty(request.Division)) 
                    values.Add("@division", UGITUtility.StringToLong(request.Division));
                if (!string.IsNullOrEmpty(request.Studio))
                    values.Add("@studio", UGITUtility.ObjectToString(request.Studio));
                if (!string.IsNullOrEmpty(request.Sector))
                    values.Add("@sector", UGITUtility.ObjectToString(request.Sector));
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime aStartdate = UGITUtility.StringToDateTime(request.StartDate);
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate);
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else
                {
                    if (request.Period == "Now")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "3 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "6 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }

                DataTable dt = GetTableDataManager.GetData("ResourceRequired", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RecruitmentViewResponse obj = new RecruitmentViewResponse()
                        {
                            RoleId = UGITUtility.ObjectToString(row["RoleId"]),
                            PctAllocation = UGITUtility.StringToDouble(row["PctAllocation"]),
                            ResourceRequired = UGITUtility.StringToInt(row["ResourceRequired"])
                        };
                        if (obj.ResourceRequired == 0)
                            continue;
                        lstResponse.Add(obj);
                    }
                }

                var data = lstResponse.OrderByDescending(x => x.ResourceRequired).Take(15);
                var maxResourceRequired = data.Count() > 0 ? data.Max(x => Math.Abs(x.ResourceRequired)) : 0;
                maxResourceRequired = maxResourceRequired < 5 ? 5 : maxResourceRequired;
                return Ok(new { maxResourceRequired = maxResourceRequired, data = data });
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetResourceRequired: " + ex);
            }
            return Ok();
        }

        [Route("GetChartData")]
        public async Task<IHttpActionResult> GetChartData([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<ChartResponse> lstresponse = new List<ChartResponse>();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                //values.Add("@Startdate", new DateTime(DateTime.Now.Year, 1, 1));
                //values.Add("@Endate", DateTime.Now.ToShortDateString());
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                string selection = Uri.UnescapeDataString(request.Selection ?? string.Empty);
                if (request.SelectionDataType == "Division")
                    values.Add("@division", UGITUtility.StringToLong(selection));
                if (request.SelectionDataType == "Studio")
                    values.Add("@studio", UGITUtility.StringToLong(selection));
                if (request.SelectionDataType == "Sector")
                    values.Add("@sector", UGITUtility.ObjectToString(selection));

                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime aStartdate = UGITUtility.StringToDateTime(request.StartDate);
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate);
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else
                {
                    if (request.Period == "Now")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "3 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "6 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }

                DataTable dt = GetTableDataManager.GetData("ChartData", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new ChartResponse()
                        {
                            FTE = UGITUtility.StringToDouble(row["FTE"]),
                            ResourceCount = UGITUtility.StringToInt(row["ResourceUser"]),
                            Amount = UGITUtility.StringToDouble(row["Amount"]),
                            Name = UGITUtility.ObjectToString(row["Name"]),
                            Title = UGITUtility.ObjectToString(row["ArgumentField"]),
                            DataType = request.Base
                        });
                    }
                }

                return Ok(lstresponse.OrderBy(x => x.Amount));
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetChartData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetProjectChartData")]
        public async Task<IHttpActionResult> GetProjectChartData([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<ChartResponse> lstresponse = new List<ChartResponse>();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                if (request.SelectionDataType == "Division")
                    values.Add("@division", UGITUtility.StringToLong(request.Selection));
                if (request.SelectionDataType == "Studio")
                    values.Add("@studio", UGITUtility.ObjectToString(request.Selection));
                if (request.SelectionDataType == "Sector")
                    values.Add("@sector", UGITUtility.ObjectToString(request.Selection));
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime aStartdate = UGITUtility.StringToDateTime(request.StartDate);
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate);
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else
                {
                    if (request.Period == "Now")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "3 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "6 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }

                DataTable dt = GetTableDataManager.GetData("ProjectChartData", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new ChartResponse()
                        {
                            Amount = UGITUtility.StringToDouble(row["Projects"]),
                            Name = UGITUtility.ObjectToString(row["Name"]),
                            Title = UGITUtility.ObjectToString(row["ArgumentField"]),
                            DataType = request.Base
                        });
                    }
                }
                if (request.Base == "Complexity")
                    return Ok(lstresponse.OrderBy(x => x.Name));
                else
                    return Ok(lstresponse.OrderBy(x => x.Amount));
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetProjectChartData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetResourceChartData")]
        public async Task<IHttpActionResult> GetResourceChartData([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<ChartResponse> lstresponse = new List<ChartResponse>();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime aStartdate = UGITUtility.StringToDateTime(request.StartDate);
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate);
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else
                {
                    if (request.Period == "Now")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "3 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "6 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                if (request.SelectionDataType == "Division")
                    values.Add("@division", UGITUtility.StringToLong(request.Selection));
                if (request.SelectionDataType == "Studio")
                    values.Add("@studio", UGITUtility.ObjectToString(request.Selection));
                if (request.SelectionDataType == "Sector")
                    values.Add("@sector", UGITUtility.ObjectToString(request.Selection));


                DataTable dt = GetTableDataManager.GetData("ResourceChartData", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new ChartResponse()
                        {
                            ResourceCount = UGITUtility.StringToInt(row["ResourceCount"]),
                            Name = UGITUtility.ObjectToString(row["Name"]),
                            Title = UGITUtility.ObjectToString(row["Title"]),
                            DataType = request.Base
                        });
                    }
                }

                return Ok(lstresponse.OrderBy(x => x.ResourceCount));
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetResourceChartData: " + ex);
                return InternalServerError();
            }
        }
        [Route("GetCardKpis")]
        public async Task<IHttpActionResult> GetCardKpis([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            CardKpisResponse responseData = new CardKpisResponse();
            List<CardKpis> lstresponse = new List<CardKpis>();
            Dictionary<string, object> values = new Dictionary<string, object>();

            try
            {
                DateTime aEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime aStartdate = UGITUtility.StringToDateTime(request.StartDate);
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate);
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
                else
                {
                    if (request.Period == "Now")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "3 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else if (request.Period == "6 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                    else
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                values.Add("@headtype", request.Type);
                string selection = Uri.UnescapeDataString(request.Selection??string.Empty);
                if (request.SelectionDataType == "Division")
                    values.Add("@division", UGITUtility.StringToLong(selection));
                if (request.SelectionDataType == "Studio")
                    values.Add("@studio", UGITUtility.StringToLong(selection));
                if (request.SelectionDataType == "Sector")
                    values.Add("@sector", UGITUtility.ObjectToString(selection));


                DataTable dt = GetTableDataManager.GetData("CardKpis", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new CardKpis()
                        {
                            HeadCount = UGITUtility.ObjectToString(row["HeadCount"]) ?? "-",
                            HeadName = UGITUtility.ObjectToString(row["HeadName"]),
                            HeadType = UGITUtility.ObjectToString(row["HeadType"])
                        });
                    }
                }
            } catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetCardKpis: " + ex);
            }
            responseData.Kpis = lstresponse;
            return Ok(responseData);
        }

        //Please don't use this action. Instead use NewOPMWizardController -> GetSimilarProjects
        [Obsolete]
        [HttpGet]
        [Route("SimilarProjects")]
        public async Task<IHttpActionResult> SimilarProjects(string ProjectID, string SearchData)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            try
            {
                int stageStep = 0;

                LifeCycleStageManager stageMgr = new LifeCycleStageManager(context);
                TicketManager ticketMgr = new TicketManager(context);

                //ProjectSimilarityMetricsManager projectSimilarityMetricsManager = new ProjectSimilarityMetricsManager(context);
                ProjectSimilarityConfigManager projectSimilarityConfigManager = new ProjectSimilarityConfigManager(context);
                string moduleName = uHelper.getModuleNameByTicketId(ProjectID);
                ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                UGITModule module = moduleViewManager.LoadByName(moduleName);

                DataRow drCurrentTicket = ticketMgr.GetByTicketID(module, ProjectID);

                LifeCycleStage lifeCycleStage = stageMgr.Load(x => x.StageTitle.EqualsIgnoreCase("Contract Award") && x.ModuleNameLookup == moduleName).FirstOrDefault();
                if (lifeCycleStage != null)
                    stageStep = lifeCycleStage.StageStep;

                List<ProjectSimilarityConfig> lstProjSimilarityConfig = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == moduleName && x.Deleted == false).ToList();

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                Dictionary<string, string> dictSearchColumns = new Dictionary<string, string>();
                StringBuilder sb = new StringBuilder();
                //sb.Append($"({DatabaseObjects.Columns.Closed}=1 or stageStep={stageStep}) ");
                sb.Append($"1=1");
                if (!string.IsNullOrEmpty(SearchData))
                {
                    dictSearchColumns = serializer.Deserialize<Dictionary<string, string>>(SearchData);
                    foreach (var item in dictSearchColumns)
                    {
                        if(String.IsNullOrEmpty(item.Value)) continue;
                        sb.Append($" and {item.Key}='{item.Value}'");
                    }
                }

                sb.Append($" and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

                DataTable dt = GetTableDataManager.GetTableData(module.ModuleTable, $"{Convert.ToString(sb)}", "*,'' as Score", null);
                foreach (DataRow item in dt.Rows)
                {
                    if (Convert.ToInt32(item["StageStep"]) == stageStep)
                        item["Title"] = $"{item["Title"]}*";

                    if (lstProjSimilarityConfig != null && lstProjSimilarityConfig.Count > 0)
                    {
                        double score = 0.0;
                        foreach (var dr in lstProjSimilarityConfig)
                        {
                            string column = Convert.ToString(dr.ColumnName);
                            //if (item.Fields.ContainsField(column))
                            if (UGITUtility.IfColumnExists(item, column))
                            {
                                if (Convert.ToString(dr.ColumnType) == "MatchValue")
                                {
                                    if (UGITUtility.ObjectToString(item[column]) == UGITUtility.ObjectToString(drCurrentTicket[column]))
                                    {
                                        score += Convert.ToDouble(dr.StageWeight);
                                    }
                                }
                                else if (Convert.ToString(dr.ColumnType) == "Ratio")
                                {
                                    double num1 = UGITUtility.StringToDouble(item[column]);
                                    double num2 = UGITUtility.StringToDouble(item[column]);
                                    double ratio = 0;
                                    if(num1 == 0 || num2 == 0)
                                    {
                                        ratio = 0;
                                    }
                                    else
                                    {
                                        if (num1 >= num2)
                                        {
                                            ratio = num2 / num1;
                                        }
                                        else if (num1 < num2)
                                        {
                                            ratio = num1 / num2;
                                        }
                                    }
                                    
                                    score += Convert.ToInt32(ratio * Convert.ToDouble(dr.StageWeight));

                                }
                            }
                        }
                        item["Score"] = score;
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    return Ok(dt);
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in SimilarProjects: " + ex);
            }
            return Ok();
        }

        //Please don't use this action. Instead use NewOPMWizardController -> GetSimilarProjects
        [Obsolete]
        [HttpGet]
        [Route("GetSimilarProjects")]
        public async Task<IHttpActionResult> GetSimilarProjects(string ProjectID)
        {
            await Task.FromResult(0);
            try
            {
                List<SimilarProjectResponse> lstResponse = new List<SimilarProjectResponse>();
                ProjectSimilarityConfigManager projectSimilarityConfigManager = new ProjectSimilarityConfigManager(_applicationContext);
                string moduleName = uHelper.getModuleNameByTicketId(ProjectID);
                UGITModule moduleObj = ModuleManager.LoadByName(moduleName);
                DataTable dtAllTickets = _TicketManager.GetOpenTickets(moduleObj);
                if (dtAllTickets != null && dtAllTickets.Rows.Count > 0)
                {
                    List<string> lstTicketIds = dtAllTickets.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)).ToList();

                    List<ProjectSimilarityConfig> lstProjSimilarityConfig = projectSimilarityConfigManager.Load(x => x.ModuleNameLookup == moduleName && x.Deleted == false).ToList();

                    StringBuilder spQuery = new StringBuilder();

                    spQuery.Append($"{DatabaseObjects.Columns.TicketId} in (");
                    foreach (string s in lstTicketIds)
                    {
                        spQuery.Append($"'{s}',");
                    }
                    spQuery.Remove(spQuery.Length - 1, 1);
                    spQuery.Append(")");

                    DataTable spListitemCollection = GetTableDataManager.GetTableData(moduleObj.ModuleTable, $"{spQuery.ToString()} and TenantID='{_applicationContext.TenantID}'");

                    DataTable prjSimilarityData = new DataTable();
                    string col = string.Empty;
                    if (!prjSimilarityData.Columns.Contains(DatabaseObjects.Columns.TicketId))
                    {
                        prjSimilarityData.Columns.Add(DatabaseObjects.Columns.TicketId);
                    }

                    foreach (DataRow item in spListitemCollection.Rows)
                    {
                        col = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
                        if (!prjSimilarityData.Columns.Contains(col))
                        {
                            prjSimilarityData.Columns.Add(col);
                        }
                    }

                    foreach (DataRow item in spListitemCollection.Rows)
                    {
                        object[] objs = new object[spListitemCollection.Rows.Count + 1];
                        objs[0] = UGITUtility.TruncateWithEllipsis(string.Format("{0}: {1}", item[DatabaseObjects.Columns.TicketId], item[DatabaseObjects.Columns.Title]), 50);
                        int ctr = 1;
                        foreach (DataRow secondaryItem in spListitemCollection.Rows)
                        {
                            if (lstProjSimilarityConfig != null && lstProjSimilarityConfig.Count > 0)
                            {
                                double score = 0.0;

                                foreach (var dr in lstProjSimilarityConfig)
                                {
                                    string column = Convert.ToString(dr.ColumnName);
                                    //if (item.Fields.ContainsField(column))
                                    if (UGITUtility.IfColumnExists(item, column))
                                    {
                                        //Match string value
                                        if (Convert.ToString(dr.ColumnType) == "MatchValue")
                                        {
                                            if (UGITUtility.ObjectToString(secondaryItem[column]) == UGITUtility.ObjectToString(item[column]))
                                            {
                                                score += Convert.ToDouble(dr.StageWeight);
                                            }
                                        }
                                        //Take ratio of numeric values
                                        else if (Convert.ToString(dr.ColumnType) == "Ratio")
                                        {
                                            double num1 = UGITUtility.StringToDouble(secondaryItem[column]);
                                            double num2 = UGITUtility.StringToDouble(item[column]);
                                            double ratio = 0;

                                            if (num1 == 0 || num2 == 0)
                                            {
                                                ratio = 0;
                                            }
                                            else
                                            {
                                                if (num1 >= num2)
                                                {
                                                    ratio = num2 / num1;
                                                }
                                                else if (num1 < num2)
                                                {
                                                    ratio = num1 / num2;
                                                }
                                            }

                                            score += Convert.ToInt32(ratio * Convert.ToDouble(dr.StageWeight));

                                        }
                                    }
                                }
                                // Do not calculate score for same project.
                                if (Convert.ToString(secondaryItem[DatabaseObjects.Columns.TicketId]) == Convert.ToString(item[DatabaseObjects.Columns.TicketId]))
                                    objs[ctr] = string.Empty;
                                else
                                    objs[ctr] = score;
                            }

                            ctr++;
                        }

                        prjSimilarityData.Rows.Add(objs);
                    }

                    if (prjSimilarityData != null)
                    {
                        prjSimilarityData = prjSimilarityData.AsEnumerable().OrderByDescending(x => UGITUtility.StringToInt(x.Field<string>(ProjectID))).Take(10).CopyToDataTable();
                        foreach (DataRow row in prjSimilarityData.Rows)
                        {
                            string ticketid = UGITUtility.ConvertStringToList(Convert.ToString(row[DatabaseObjects.Columns.TicketId]), Constants.Separator7).First();
                            DataRow similarProjectRow = dtAllTickets.Select($"{DatabaseObjects.Columns.TicketId}='{ticketid}'")[0];
                            SimilarProjectResponse response = new SimilarProjectResponse();
                            response.Title = UGITUtility.ObjectToString(similarProjectRow[DatabaseObjects.Columns.TicketId]);
                            response.Duration = UGITUtility.StringToInt(similarProjectRow[DatabaseObjects.Columns.Duration]);
                            response.Hrs = "15,000";
                            response.Resources = 23;
                            response.ERPJobIDNC = UGITUtility.ObjectToString(similarProjectRow[DatabaseObjects.Columns.ERPJobIDNC]);
                            lstResponse.Add(response);
                        }
                        return Ok(lstResponse);
                    }


                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetSimilarProjects: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetProjectList")]
        public async Task<IHttpActionResult> GetProjectList([FromUri] ResourceDetailChartRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<ChartResponse> lstresponse = new List<ChartResponse>();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);
                values.Add("@Startdate", new DateTime(DateTime.Now.Year, 1, 1));
                values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                List<string> selectionList = UGITUtility.ConvertStringToList(request.ParentSelection, Constants.Separator2);
                string selectionType = selectionList.First();
                if (selectionType == "Division")
                    values.Add("@division", selectionList.Last());
                else if (selectionType == "Sector")
                    values.Add("@sector", selectionList.Last());
                else if (selectionType == "Studio")
                    values.Add("@studio", selectionList.Last());

                if (request.Base == "Division")
                    values.Add("@division", request.ChildSelection);
                else if (request.Base == "Sector")
                    values.Add("@sector", request.ChildSelection);
                else if (request.Base == "Studio")
                    values.Add("@studio", request.ChildSelection);

                DataTable dt = GetTableDataManager.GetData("ProjectList", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Ok(dt);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetProjectList: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetExecutiveChartData")]
        public async Task<IHttpActionResult> GetExecutiveChartData([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<ExecutiveChartResponse> lstresponse = new List<ExecutiveChartResponse>();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                if (request.SelectionDataType == "Division")
                    values.Add("@division", UGITUtility.StringToLong(request.Selection));
                if (request.SelectionDataType == "Studio")
                    values.Add("@studio", UGITUtility.ObjectToString(request.Selection));
                if (request.SelectionDataType == "Sector")
                    values.Add("@sector", UGITUtility.ObjectToString(request.Selection));

                DateTime aEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    DateTime temp = UGITUtility.StringToDateTime(request.StartDate).Date;
                    DateTime aStartdate = new DateTime(temp.Year, temp.Month, 1);
                    DateTime aEnddate = UGITUtility.StringToDateTime(request.EndDate).Date;
                    values.Add("@Startdate", aStartdate.ToString("yyyy-MM-dd"));
                    values.Add("@Endate", aEnddate.ToString("yyyy-MM-dd"));
                }
                else
                {
                    if (request.Period == "Now")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else if (request.Period == "3 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else if (request.Period == "6 Month")
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                        values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd"));
                        values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                }

                DataTable dt = GetTableDataManager.GetData("ExecutiveChartData", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(row["Name"])))
                        {
                            lstresponse.Add(new ExecutiveChartResponse()
                            {
                                Name = UGITUtility.ObjectToString(row["Name"]),
                                Capacity = UGITUtility.StringToDouble(row["capacity"]),
                                Utilization = UGITUtility.StringToDouble(row["utilization"]),
                                Lookup = UGITUtility.ObjectToString(row["Lookup"]),
                                DataType = request.Base
                            });
                        }
                    }
                }

                return Ok(lstresponse.OrderBy(x => x.Capacity));
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetExecutiveChartData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetExecutiveViewCounts")]
        public async Task<IHttpActionResult> GetExecutiveViewCounts()
        {
            await Task.FromResult(0);
            ////old code
            //Dictionary<string, object> values = new Dictionary<string, object>();
            //values.Add("@TenantID", _applicationContext.TenantID);
            //values.Add("@UserID", _applicationContext.CurrentUser.Id);
            //DataTable dt = GetTableDataManager.GetData("ExecutiveViewCount", values);
            //if (dt != null && dt.Rows.Count > 0)
            //    return Ok(dt);
            ////old code
            try
            {
                List<ExecutiveViewCountModel> lstExecutiveViewCountResponse = new List<ExecutiveViewCountModel>();
                UGITModule CPRModule = ModuleManager.LoadByName(ModuleNames.CPR);
                DataTable cprTickets = _TicketManager.GetAllTickets(CPRModule);

                UGITModule OPMModule = ModuleManager.LoadByName(ModuleNames.OPM);
                DataTable opmTickets = _TicketManager.GetAllTickets(OPMModule);

                if (opmTickets != null && opmTickets.Rows.Count > 0)
                {
                    ExecutiveViewCountModel PendingAssignment = new ExecutiveViewCountModel();
                    PendingAssignment.KpiName = "PendingAssignment";
                    PendingAssignment.KpiLabel = "Pending<br>Assignment";
                    DataRow[] assignOPMTickets = opmTickets.Select($"StageStep = 2 and (OnHold is null or OnHold = 0)");
                    if (assignOPMTickets != null && assignOPMTickets.Count() > 0)
                    {
                        PendingAssignment.Count = assignOPMTickets.Count();
                    }
                    lstExecutiveViewCountResponse.Add(PendingAssignment);
                }

                if (cprTickets != null && cprTickets.Rows.Count > 0)
                {
                    ExecutiveViewCountModel ConstructionContract = new ExecutiveViewCountModel();
                    ConstructionContract.KpiName = "ConstructionContract";
                    ConstructionContract.KpiLabel = "Construction<br>Contract";
                    DataRow[] constructionTickets = cprTickets.Select($"StageStep=6 and (OnHold is null or OnHold = 0)");
                    if (constructionTickets != null && constructionTickets.Count() > 0)
                    {
                        ConstructionContract.Count = constructionTickets.Count();
                    }
                    lstExecutiveViewCountResponse.Add(ConstructionContract);

                    ExecutiveViewCountModel Closeout = new ExecutiveViewCountModel();
                    Closeout.KpiName = "Closeout";
                    Closeout.KpiLabel = "Closeout Due";
                    int optimalcloseout = UGITUtility.StringToInt(_configVariableManager.GetValue(ConfigConstants.CloseoutDueDays));
                    DataRow[] closeoutticketsone = cprTickets.AsEnumerable().Where(row =>row.Field<int>("StageStep") == 8 &&
                                                                                         (row.Field<DateTime?>("CloseoutDate")?.AddDays(-optimalcloseout) ?? DateTime.MinValue) <= DateTime.Now.Date
                                                                                         && row.Field<int?>(DatabaseObjects.Columns.OnHold) != 1).ToArray();
                    if (closeoutticketsone != null && closeoutticketsone.Count() > 0)
                    {
                        Closeout.Count = closeoutticketsone.Count();
                    }
                    lstExecutiveViewCountResponse.Add(Closeout);
                }

                return Ok(lstExecutiveViewCountResponse);
            }
            catch(Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetExecutiveViewCounts: " + ex);
            }
            return Ok("No Data Returned");
        }

        [Route("GetPMOChartData")]
        public async Task<IHttpActionResult> GetPMOChartData([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<ChartResponse> lstresponse = new List<ChartResponse>();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@Startdate", new DateTime(DateTime.Now.Year - 1, 1, 1));
                values.Add("@Endate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                if (request.SelectionDataType == "Division")
                    values.Add("@division", UGITUtility.StringToLong(request.Selection));
                if (request.SelectionDataType == "Studio")
                    values.Add("@studio", UGITUtility.ObjectToString(request.Selection));
                if (request.SelectionDataType == "Sector")
                    values.Add("@sector", UGITUtility.ObjectToString(request.Selection));
                //values.Add("@Startdate", string.Empty);
                //values.Add("@Endate", string.Empty);

                DataTable dt = GetTableDataManager.GetData("PMMChartData", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new ChartResponse()
                        {
                            Amount = UGITUtility.StringToDouble(row["Projects"]),
                            Name = UGITUtility.ObjectToString(row["Name"]),
                            Title = UGITUtility.ObjectToString(row["ArgumentField"]),
                            DataType = request.Base
                        });
                    }
                }

                return Ok(lstresponse.OrderBy(x => x.Amount));

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetPMOChartData: " + ex);
                return InternalServerError();
            }
        }

        [Route("GetPMOCardKpis")]
        public async Task<IHttpActionResult> GetPMOCardKpis([FromUri] ChartRequest request)
        {
            await Task.FromResult(0);
            CardKpisResponse responseData = new CardKpisResponse();
            List<CardKpis> lstresponse = new List<CardKpis>();
            Dictionary<string, object> values = new Dictionary<string, object>();

            try
            {
                DateTime aEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                if (request.Period == "Now")
                {
                    DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEndDate);
                }
                else if (request.Period == "3 Month")
                {
                    DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-3).Month, 1);
                    values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEndDate);
                }
                else if (request.Period == "6 Month")
                {
                    DateTime aStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-6).Month, 1);
                    values.Add("@Startdate", aStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    values.Add("@Endate", aEndDate);
                }
                else
                {
                    DateTime aStartDate = new DateTime(DateTime.Now.Year, 1, 1);
                    values.Add("@Startdate", aStartDate);
                    values.Add("@Endate", aEndDate);
                }
                values.Add("@tenantID", _applicationContext.TenantID);
                values.Add("@filter", request.Filter);
                values.Add("@base", request.Base);
                values.Add("@headtype", request.Type);
                if (request.SelectionDataType == "Division")
                    values.Add("@division", UGITUtility.StringToLong(request.Selection));
                if (request.SelectionDataType == "Studio")
                    values.Add("@studio", UGITUtility.ObjectToString(request.Selection));
                if (request.SelectionDataType == "Sector")
                    values.Add("@sector", UGITUtility.ObjectToString(request.Selection));


                DataTable dt = GetTableDataManager.GetData("PMOCardKpis", values);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        lstresponse.Add(new CardKpis()
                        {
                            HeadCount = UGITUtility.ObjectToString(row["HeadCount"]) ?? "-",
                            HeadName = UGITUtility.ObjectToString(row["HeadName"]),
                            HeadType = UGITUtility.ObjectToString(row["HeadType"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetPMOCardKpis: " + ex);
            }
            responseData.Kpis = lstresponse;
            return Ok(responseData);
        }

        [Route("GetFinancialChartDetails")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFinancialChartDetails(string filter, string type)
        {
          
            //var messageCode = 0;
            try
            {
                await Task.FromResult(0);
                
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("Filter", filter);
                arrParams.Add("Type", type);

                DataTable aoData = GetTableDataManager.GetData("FinancialDetails", arrParams);
                dynamic output = new ExpandoObject();
                output.dashboardTitle = "Financial Analytics";

                output.aoData = aoData;
                if (aoData != null && aoData.Rows.Count > 0)
                    return Ok(aoData);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex.Message, "GetUserChartDetails- panel id not found");
            }
            return null;
        }

        [Route("GetSectorStudioData")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSectorStudioData()
        {
            try
            {
                await Task.FromResult(0);
                string where = string.Format("{0}='{1}' AND {2} IN ('{3}','{4}') AND {5}='{6}' AND {7} = '{8}'", DatabaseObjects.Columns.TenantID, _applicationContext.TenantID, 
                    DatabaseObjects.Columns.FieldName, "StudioLookup", "SectorChoice", DatabaseObjects.Columns.TableName, DatabaseObjects.Tables.CRMProject, "DataType", DatabaseObjects.DataTypes.Choices);
                DataTable dtdata = GetTableDataManager.GetTableData(DatabaseObjects.Tables.FieldConfiguration, where, DatabaseObjects.Columns.FieldName + ", " + DatabaseObjects.Columns.Data, null);

                if (dtdata != null && dtdata.Rows.Count > 0)
                    return Ok(dtdata);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex.Message, "GetUserChartDetails- panel id not found");
            }
            return null;
        }
        [Route("GetSectorStudioDivisionData")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSectorStudioDivisionData(string dataRequiredFor, string parentDivision = "")
        {
            try
            {
                await Task.FromResult(0);
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("DataRequiredFor", dataRequiredFor);
                ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(_applicationContext);
                bool enableStudioDivisionHierarchy = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);
                DataTable dtResult = null;
                DataSet dsResult = GetTableDataManager.GetDataSet("SectorStudioDivisionData", arrParams);
                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (!string.IsNullOrEmpty(parentDivision) && parentDivision != "0" && enableStudioDivisionHierarchy && dataRequiredFor == "studio")
                    {
                        DataRow[] dr = dtResult.Select(DatabaseObjects.Columns.DivisionLookup + "='" + parentDivision + "'");
                        dtResult = dr.Length > 0 ? dr.CopyToDataTable() : null;
                    }
                }
                return Ok(dsResult);
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex.Message, "GetUserChartDetails- panel id not found");
            }
            return null;
        }

    }
    public class RMMCountPanelResponse
    {
        public string Text;
        public string Name;
        public string Value;
        public string IconUrl;
        public string Status;
        public int Order;
        public bool HideIcon;
    }
    public class CriticalTaskResponse
    {
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string ProjectTitle { get; set; }
        public string ModuleNameLookup { get; set; }
        public long ID { get; set; }
    }
    public class Series
    {
        public string type { get; set; }
        public string name { get; set; }
        public string valueField { get; set; }
        public string argumentField { get; set; }
        public string axis { get; set; }
        public object label { get; set; }
    }
    public class GaugeChartResponse
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public string DataType { get; set; }
    }
    public class ToolTipDetail
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ToolTipDataRecords
    {
        public string Name { get; set; }
        public int ResourceCount { get; set; }
        public int HotProject { get; set; }
        public double HotRevenue { get; set; }
        public int CommittedProjects { get; set; }
        public double Value { get; set; }
        public double PastRevenue { get; set; }
        public string DataType { get; set; }
        public double Utilization { get; set; }
        public string Selection { get; set; }
    }
    public class EstimatedResourceBillingChartResponse
    {
        public string Title { get; set; }
        public double Hours { get; set; }
        //public double Amount { get; set; }
    }
        public class EstimatedResourceBillingChartRequest
    {
        public string Period { get; set; }
        public string Filter { get; set; }
        //public string Base { get; set; }
        //public string Type { get; set; }
    }
        public class ChartRequest
    {
        public string Period { get; set; }
        public string Filter { get; set; }
        public string Base { get; set; }
        public string Type { get; set; }
        public string Selection { get; set; }
        public string SelectionDataType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class RecruitmentViewChartRequest
    {
        public string Period { get; set; }
        public string Filter { get; set; }
        public string Base { get; set; }
        public string Type { get; set; }
        public string Selection { get; set; }
        public string SelectionDataType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Studio { get; set; }
        public string Division { get; set; }
        public string Sector { get; set; }
    }

    public class ResourceDetailChart
    {
        public string MonthStart { get; set; }
        public int Utilization { get; set; }
        public int Availability { get; set; }
    }
    public class ResourceDetailChartRequest
    {
        public string Filter { get; set; }
        public string Base { get; set; }
        public string Type { get; set; }
        public string ParentSelection { get; set; }
        public string ChildSelection { get; set; }
        public string Billable { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Period { get; set; }
    }

    public class ChartResponse
    {
        public double FTE { get; set; }
        public int ResourceCount { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
    }

    public class RecruitmentViewResponse
    {
        public string RoleId { get; set; }
        public double PctAllocation { get; set; }
        public int ResourceRequired { get; set; }
    }
    public class CardKpisResponse
    {
        public List<CardKpis> Kpis { get; set; }
    }
    public class CardKpis
    {
        public string HeadCount { get; set; }
        public string HeadName { get; set; }
        public string HeadType { get; set; }
    }
    public class ExecutiveChartResponse
    {
        public double Utilization { get; set; }
        public double Capacity { get; set; }
        public string Name { get; set; }
        public string Lookup { get; set; }
        public string DataType { get; set; }
    }

    public class SimilarProjectResponse
    {//"Title": "Google HQ T1", "Duration": 92, "Resources": 12, "Hrs": "15,000"
        public string Title { get; set; }
        public int Duration { get; set; }
        public int Resources { get; set; }
        public string Hrs { get; set; }
        public string TicketId { get; set; }
        public double Score { get; set; }
        public string ERPJobIDNC { get; set; }
        public string ColorCode { get; set; }
    }

    public class ExecutiveViewCountModel
    {
        public string KpiName { get; set; }
        public int Count { get; set; }
        public string KpiLabel { get; set; }
    }
}
