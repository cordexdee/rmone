using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Helpers;
using uGovernIT.Web.Models;
using uGovernIT.Manager.RMM;
using System.Web.Script.Serialization;
using uGovernIT.Util.ImportExportMPP;
using uGovernIT.Util.Cache;
using DevExpress.ExpressApp;
using DevExpress.Web.Internal.XmlProcessor;
using System.Web.UI.DataVisualization.Charting;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/module")]
    public class ModuleController : ApiController
    {
        public ModuleViewManager _moduleManager = null;
        public TicketManager _ticketManager = null;
        public ApplicationContext _context = null;
        public ModuleController()
        {
            _moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            _ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            _context = HttpContext.Current.GetManagerContext();
        }
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }
        [HttpGet]
        [Route("GetRequestTypeDependent")]
        public async Task<IHttpActionResult> GetRequestTypeDependent(string moduleName, long requestTypeID, long locationID, string requestor)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ModuleViewManager moduleMgr = new ModuleViewManager(context);
                UGITModule module = moduleMgr.GetByName(moduleName);
                RequesetTypeDependentModel rtDep = RequestTypeHelper.GetRequestType(context, module, requestTypeID, locationID, requestor);
                await Task.FromResult(0);
                return Ok(rtDep);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRequestTypeDependent: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetPriorityMappings")]
        public async Task<IHttpActionResult> GetPriorityMappings(string moduleName)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ModuleViewManager moduleMgr = new ModuleViewManager(context);
                UGITModule module = moduleMgr.GetByName(moduleName);
                await Task.FromResult(0);
                return Ok(module.List_PriorityMaps);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetPriorityMappings: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GenericEmailContent")]
        public async Task<IHttpActionResult> GenericEmailContent(string moduleName)
        {
            await Task.FromResult(0);
            return Ok("");
        }
        [HttpGet]
        [Route("LoadRequestTypeByLocation")]
        public async Task<IHttpActionResult> LoadRequestTypeByLocation(string moduleName)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                List<string> locationStarting = new List<string>();
                RequestTypeByLocationManager rTypeLoc = new RequestTypeByLocationManager(context);
                List<ModuleRequestTypeLocation> rTypeLocList = rTypeLoc.GetRequestTypeLocationData(moduleName);
                foreach (ModuleRequestTypeLocation rtl in rTypeLocList)
                {
                    if (rtl.RequestType != null && rtl.Location != null && rtl.Owner != null)
                        locationStarting.Add(string.Format("\"{2}-{3}\":{0}\"owners\":\"{4}\",\"prpgroup\":\"{5}\",\"orpgroup\":\"{6}\"{1}", "{", "}",
                            rtl.RequestTypeLookup, rtl.LocationLookup, context.UserManager.ConcatenateValues(context.UserManager.uesrListByMultipleID(rtl.Owner, ";"), ";"), context.UserManager.GetValue(context.UserManager.GetUserById(rtl.PRPGroup)), context.UserManager.ConcatenateValues(context.UserManager.uesrListByMultipleID(rtl.ORP, ";"), ";")));
                }
                string s = "{" + string.Join(",", locationStarting.ToArray()) + "}";
                await Task.FromResult(0);
                return Ok(s);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in LoadRequestTypeByLocation: " + ex);
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("UpdateScore")]
        public async Task<IHttpActionResult> UpdateScore(ProjectScoreModel projectScoreModel)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext _context = HttpContext.Current.GetManagerContext();
                ProjectMonitorStateManager monitorStateManager = new ProjectMonitorStateManager(_context);

                MonitorState monitorstate = new MonitorState();
                string monitorColor = string.Empty;
                long pmmId = projectScoreModel.pmmid;
                long monitorId = projectScoreModel.monitorId;

                List<ProjectMonitorState> projectMonitorStates = monitorStateManager.Load(x => x.TenantID == _context.TenantID && x.PMMIdLookup == pmmId);
                float overallScore = 0;
                float totalWeight = 0;
                float monitorWeightFloat = 0;
                float pmmriskscore = 0;
                bool updateRiskScore = false;
                ModuleMonitorOptionManager moduleMonitorOptionManager = new ModuleMonitorOptionManager(_context);
                ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(_context);
                ModuleMonitorOption moduleMonitorOption = null;
                ModuleMonitor moduleMonitor = null;
                foreach (ProjectMonitorState _projectMonitorState in projectMonitorStates)
                {
                    monitorWeightFloat = 0;
                    if (monitorId == _projectMonitorState.ID)
                    {
                        _projectMonitorState.ModuleMonitorOptionIdLookup = UGITUtility.StringToLong(projectScoreModel.monitorOptionName);
                        _projectMonitorState.ProjectMonitorWeight = UGITUtility.StringToInt(projectScoreModel.monitorWeight);
                        _projectMonitorState.ProjectMonitorNotes = projectScoreModel.ProjectMonitorNotes;
                        //Update in Project monitor state
                        monitorStateManager.Update(_projectMonitorState);

                        moduleMonitorOption = moduleMonitorOptionManager.LoadByID(_projectMonitorState.ModuleMonitorOptionIdLookup);
                        moduleMonitor = moduleMonitorManager.LoadByID(_projectMonitorState.ModuleMonitorNameLookup);
                        monitorstate.LEDClass = moduleMonitorOption.ModuleMonitorOptionLEDClass;
                        monitorstate.Id = monitorId;
                        monitorstate.SelectedOption = moduleMonitorOption.ModuleMonitorOptionName;
                        monitorstate.Weight = UGITUtility.StringToInt(_projectMonitorState.ProjectMonitorWeight);
                        monitorstate.ModuleMonitorName = moduleMonitor.MonitorName;
                        float multiplier = 0;
                        float.TryParse(Convert.ToString(moduleMonitorOption.ModuleMonitorMultiplier), out multiplier);
                        float.TryParse(UGITUtility.ObjectToString(projectScoreModel.monitorWeight), out monitorWeightFloat);
                        float score = monitorWeightFloat * (multiplier / 100);
                        monitorstate.MonitorStateScore = score;
                        if (moduleMonitor != null && moduleMonitor.MonitorName == "Risk Level")
                        {
                            updateRiskScore = true;
                            pmmriskscore = score;
                        }
                    }
                    else
                    {
                        moduleMonitorOption = moduleMonitorOptionManager.LoadByID(_projectMonitorState.ModuleMonitorOptionIdLookup);
                        float.TryParse(Convert.ToString(_projectMonitorState.ProjectMonitorWeight), out monitorWeightFloat);
                    }

                    if (moduleMonitorOption != null)
                    {
                        totalWeight += monitorWeightFloat;
                        float multiplier = 0;
                        float.TryParse(Convert.ToString(moduleMonitorOption.ModuleMonitorMultiplier), out multiplier);
                        overallScore += monitorWeightFloat * (multiplier / 100);
                    }
                }

                Ticket ticket = new Ticket(_context, ModuleNames.PMM, _context.CurrentUser);
                TicketManager ticketManager = new TicketManager(_context);
                ModuleViewManager moduleViewManager = new ModuleViewManager(_context);
                UGITModule module = moduleViewManager.LoadByName(ModuleNames.PMM);
                DataRow pmm = ticketManager.GetByID(module, projectScoreModel.pmmid);

                pmm[DatabaseObjects.Columns.TicketProjectScore] = Math.Round(overallScore * 100 / totalWeight, 0);
                if (updateRiskScore)
                    pmm[DatabaseObjects.Columns.TicketRiskScore] = Math.Round(100 - pmmriskscore);

                monitorColor = overallScore.ToString();
                ticket.CommitChanges(pmm);
                monitorstate.Overallscore = overallScore;
                return Ok(monitorstate);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateScore: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("CheckedAutoUpdate")]
        public async Task<IHttpActionResult> CheckedAutoUpdate(ProjectScoreModelAutoCalculate projectScoreModelAutoCalculate)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext _context = HttpContext.Current.GetManagerContext();
                ModuleBudgetManager moduleBudgetManager = new ModuleBudgetManager(_context);
                ProjectMonitorStateManager monitorStateManager = new ProjectMonitorStateManager(_context);
                ModuleMonitorOptionManager monitorOptionManager = new ModuleMonitorOptionManager(_context);
                bool ischecked = projectScoreModelAutoCalculate.IsChecked;
                string monitorName = projectScoreModelAutoCalculate.MonitorName;
                string moduleName = projectScoreModelAutoCalculate.ModuleNameLookup;
                int monitorStateId = projectScoreModelAutoCalculate.MonitorStateId;
                int pmmId = projectScoreModelAutoCalculate.PmmId;
                string ticketId = projectScoreModelAutoCalculate.TicketId;
                ProjectMonitorState projectMonitorState = monitorStateManager.LoadByID(monitorStateId);
                projectMonitorState.AutoCalculate = ischecked;
                monitorStateManager.Update(projectMonitorState);

                float overallScore = 0;
                TicketManager ticketManager = new TicketManager(_context);
                ModuleViewManager moduleManager = new ModuleViewManager(_context);
                UGITModule module = moduleManager.LoadByName(moduleName);
                DataRow pmmitem = ticketManager.GetByID(module, pmmId);
                UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
                if (ischecked)
                {
                    if (monitorName == "On Time")
                        overallScore = uGITTaskManager.AutoCalculateProjectMonitorStateOnTime(pmmitem, _context);
                    else if (monitorName == "On Budget")
                        overallScore = moduleBudgetManager.AutoCalculateBudgetMonitorState(ticketId, moduleName);
                }
                else
                {
                    float.TryParse(UGITUtility.ObjectToString(pmmitem[DatabaseObjects.Columns.TicketProjectScore]), out overallScore);
                }


                MonitorState monitorState = new MonitorState();
                ModuleMonitorOption moduleMonitorOption = monitorOptionManager.LoadByID(projectMonitorState.ModuleMonitorOptionIdLookup);
                monitorState.Id = projectMonitorState.ID;
                monitorState.LEDClass = moduleMonitorOption.ModuleMonitorOptionLEDClass;
                monitorState.Weight = Convert.ToInt32(projectMonitorState.ProjectMonitorWeight);
                monitorState.SelectedOption = moduleMonitorOption.ModuleMonitorOptionName;
                monitorState.SelectedOptionId = moduleMonitorOption.ID;

                float multiplier = 0;
                float.TryParse(Convert.ToString(moduleMonitorOption.ModuleMonitorMultiplier), out multiplier);
                float score = monitorState.Weight * (multiplier / 100);
                monitorState.MonitorStateScore = score;
                monitorState.Overallscore = overallScore;

                return Ok(monitorState);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CheckedAutoUpdate: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("RefreshProjectSummary")]
        public async Task<IHttpActionResult> RefreshProjectSummary()
        {
            await Task.FromResult(0);
            try
            {
                ProjectEstimatedAllocationManager projectManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
                ThreadStart threadMethod = delegate ()
                {
                    projectManager.RefreshProjectComplexity();
                };
                Thread sThread = new Thread(threadMethod);
                sThread.IsBackground = true;
                sThread.Start();
                return Ok(true);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CheckedAutoUpdate: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("CheckComplexityRefreshInProcess")]
        public async Task<IHttpActionResult> CheckComplexityRefreshInProcess()
        {
            await Task.FromResult(0);
            ProjectEstimatedAllocationManager projectManager = new ProjectEstimatedAllocationManager(HttpContext.Current.GetManagerContext());
            return Ok(projectManager.ProcessState());
        }
        [HttpGet]
        [Route("GetTaskData")]
        public async Task<IHttpActionResult> GetTaskData(string TicketID, int BaseLineID)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UserProfileManager userManager = new UserProfileManager(context);
                UGITTaskManager taskManager = new UGITTaskManager(context);
                ModuleTaskHistoryManager _moduleTaskHistoryManager = new ModuleTaskHistoryManager(context);
                List<UGITTask> tasks = new List<UGITTask>();
                string moduleName = uHelper.getModuleNameByTicketId(TicketID);
                double projectDuration = UGITUtility.StringToDouble(Ticket.GetCurrentTicket(context, moduleName, TicketID)[DatabaseObjects.Columns.Duration]);
                //base line for Task tab 
                if (BaseLineID > 0)
                {
                    List<ModuleTasksHistory> moduleTaskHistory = _moduleTaskHistoryManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketID}' and {DatabaseObjects.Columns.BaselineId}={BaseLineID}").OrderBy(x => x.ItemOrder).ToList();

                    foreach (var item in moduleTaskHistory)
                    {
                        tasks.Add(taskManager.CopyItem(item));
                    }
                }
                else
                    tasks = taskManager.Load(x => x.TicketId == TicketID && x.ModuleNameLookup == moduleName && string.IsNullOrEmpty(x.SubTaskType));
                // Manage min date for Start and Due Dates
                taskManager.ManageMinStartDueDates(ref tasks);
                //tasks = UGITTaskManager.MapRelationalObjects(tasks);
                if (projectDuration == 0)
                    projectDuration = tasks.Sum(x => x.Duration);
                List<UGITTask> CriticalPathTasks = taskManager.GetCriticalPathTask(tasks);
                //taskManager.ReManageTasks(ref tasks); commented 27 jan 2020
                List<UGITModuleTaskModel> tasklist = new List<UGITModuleTaskModel>();
                List<UGITAssignTo> listAssignToPct = null;
                foreach (UGITTask task in tasks)
                {
                    UGITModuleTaskModel model = new UGITModuleTaskModel();
                    model.ID = task.ID;
                    model.AssignedTo = task.AssignedTo;
                    listAssignToPct = taskManager.GetUGITAssignPct(task.AssignToPct);
                    if (listAssignToPct != null && listAssignToPct.Count > 0)
                        model.AssignToPct = string.Join(Constants.Separator, listAssignToPct.Select(x => string.Join(Constants.Separator1, new string[] { x.ID, x.Percentage, x.UserName })));
                    model.Title = task.Title;
                    model.Status = task.Status;
                    model.StartDate = task.StartDate;
                    model.DueDate = task.DueDate;
                    model.PercentComplete = Math.Ceiling((task.PercentComplete) * 100);
                    model.ParentTaskID = task.ParentTaskID;
                    model.ItemOrder = task.ItemOrder;
                    model.ChildCount = task.ChildCount;
                    model.EstimatedHours = task.EstimatedHours;
                    model.EstimatedRemainingHours = task.EstimatedRemainingHours;
                    model.ActualHours = task.ActualHours;
                    model.Duration = task.Duration;
                    model.Contribution = Math.Round(Convert.ToDouble(task.Duration) / projectDuration * 100, 0);
                    model.StageStep = task.StageStep;
                    model.Description = task.Description;
                    if (task.SprintLookup > 0)
                    {
                        object p = GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.Sprint, DatabaseObjects.Columns.Title, Convert.ToString(task.SprintLookup), context.TenantID);
                        model.Sprint = UGITUtility.ObjectToString(p);
                        string title = string.Format("{0} (Sprint: {1})", task.Title, UGITUtility.ObjectToString(p));
                        if (title.Length > 150)
                            title = title.Substring(0, 147) + "...";
                        model.Title = title;
                    }
                    model.Behaviour = task.Behaviour;
                    if (!string.IsNullOrEmpty(task.Predecessors))
                    {
                        //Changed by Inderjeet Kaur on 15/10/2022
                        //Display the ordered list of Predecessors as it is. Fetching pred Ids is not required, ItemOrder of Predecessors is being used.
                        string[] preds = task.Predecessors.Split(',');
                        model.Predecessors = String.Join(Constants.Separator6 + Constants.SpaceSeparator,
                        task.Predecessors.Split(',').Select(c => Convert.ToInt32(c.Trim())).OrderBy(i => i));
                    }
                    if (CriticalPathTasks.Any(x => x.ID == task.ID))
                        task.IsCritical = true;
                    model.isCritical = task.IsCritical;
                    tasklist.Add(model);
                }
                if (tasklist != null)
                {
                    tasklist = tasklist.OrderBy(x => x.ItemOrder).ToList();
                    return Ok(tasklist);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTaskData: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetSummaryData")]
        public async Task<IHttpActionResult> GetSummaryData(string TicketID, int BaseLineID)
        {

            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                string moduleName = uHelper.getModuleNameByTicketId(TicketID);
                DataRow ticketItem = Ticket.GetCurrentTicket(context, moduleName, TicketID);
                SummaryHelper summaryHelper = new SummaryHelper();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                ModuleTaskHistoryManager _moduleTaskHistoryManager = new ModuleTaskHistoryManager(context);
                List<UGITTask> tasks = new List<UGITTask>();
                //base line for Task tab 
                if (BaseLineID > 0)
                {
                    List<ModuleTasksHistory> moduleTaskHistory = _moduleTaskHistoryManager.Load($"{DatabaseObjects.Columns.TicketId}='{TicketID}' and {DatabaseObjects.Columns.BaselineId}={BaseLineID}").OrderBy(x => x.ItemOrder).ToList();

                    foreach (var item in moduleTaskHistory)
                    {
                        tasks.Add(taskManager.CopyItem(item));
                    }
                }
                else
                {

                    tasks = taskManager.Load(x => x.TicketId == TicketID && x.ModuleNameLookup == moduleName && x.SubTaskType != "Risk" && x.SubTaskType != "Issue");
                }
                // Manage min date for Start and Due Dates
                taskManager.ManageMinStartDueDates(ref tasks);

                double totalEstimatedHour = 0;
                double ActualHours = 0;
                double EstimatedHours = 0;
                double Duration = 0;
                var ProjectCompletion = 0.0;
                DateTime? StartDate = null;
                DateTime? DueDate = null;
                if (tasks != null && tasks.Count > 0)
                {
                    totalEstimatedHour = tasks.Sum(x => x.EstimatedRemainingHours);
                    ActualHours = tasks.Sum(x => x.ActualHours);
                    EstimatedHours = tasks.Sum(x => x.EstimatedHours);
                    Duration = tasks.Sum(x => x.Duration);
                    ProjectCompletion = UGITUtility.StringToDouble(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketPctComplete));
                    if (ProjectCompletion > 99.9 && ProjectCompletion < 100)
                        ProjectCompletion = 99.9;
                    else
                        ProjectCompletion = Math.Round(ProjectCompletion, 1, MidpointRounding.AwayFromZero);
                    //((tasks.Sum(x => x.PercentComplete) / (tasks.Count() * 100)) * 100).ToString("#,0.##");
                    StartDate = tasks.Min(x => x.StartDate);
                    DueDate = tasks.Max(x => x.DueDate);
                }

                var SummaryData = new
                {

                    EstimatedRemainingHours = summaryHelper.GetEstimatedRemainingHoursSummaryText(totalEstimatedHour.ToString()),
                    EstimatedHours = summaryHelper.GetEstimatedHoursSummaryText(EstimatedHours.ToString()),
                    ActualHours = summaryHelper.GetActualHoursSummaryText(ActualHours.ToString()),
                    DueDate = (DueDate != null ? DueDate.Value.ToString("MMM-dd-yyyy") : string.Empty),
                    StartDate = (StartDate != null ? StartDate?.ToString("MMM-dd-yyyy") : string.Empty),
                    //StartDate = summaryHelper.GetStartDateSummaryText(ticketItem),
                    ProjectCompletion = $"Project is {ProjectCompletion}% complete",
                    Duration = $"{Duration} days"
                };
                return Ok(SummaryData);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetSummaryData: " + ex);
                return InternalServerError();
            }

        }

        //Added 20 jan 2020
        [HttpGet]
        [Route("GetTaskDataByUser")]
        public async Task<IHttpActionResult> GetTaskDataByUser(string TicketID)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UserProfileManager userManager = new UserProfileManager(context);
                UGITTaskManager taskManager = new UGITTaskManager(context);
                string moduleName = uHelper.getModuleNameByTicketId(TicketID);
                List<UGITTask> tasks = taskManager.Load(x => x.TicketId == TicketID && x.AssignedTo.Contains(Convert.ToString(context.CurrentUser.Id)) && x.ModuleNameLookup == moduleName);

                List<UGITTask> CriticalPathTasks = taskManager.GetCriticalPathTask(tasks);
                List<UGITModuleTaskModel> tasklist = new List<UGITModuleTaskModel>();
                foreach (UGITTask task in tasks)
                {
                    List<string> strUserName = new List<string>();
                    char[] delimiters = new[] { ',', '#', ';' };  // List of delimiters
                    var splittedArray = task.AssignedTo.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string value in splittedArray)
                    {
                        strUserName.Add(uHelper.GetUserNameBasedOnId(context, value));
                    }
                    UGITModuleTaskModel model = new UGITModuleTaskModel();
                    model.ID = task.ID;
                    model.AssignedTo = task.AssignedTo;
                    model.AssignedToName = String.Join(",", strUserName.ToArray());
                    model.Title = task.Title;
                    model.Status = task.Status;
                    model.StartDate = task.StartDate;
                    model.DueDate = task.DueDate;
                    model.PercentComplete = task.PercentComplete;
                    model.ParentTaskID = task.ParentTaskID;
                    model.ItemOrder = task.ItemOrder;
                    model.ChildCount = task.ChildCount;
                    model.EstimatedHours = task.EstimatedHours;
                    model.EstimatedRemainingHours = task.EstimatedRemainingHours;
                    model.ActualHours = task.ActualHours;
                    model.Duration = task.Duration;
                    if (!string.IsNullOrEmpty(task.Predecessors))
                    {
                        string[] preds = task.Predecessors.Split(',');
                        List<string> newPredecessors = new List<string>();
                        foreach (string pred in preds.OrderBy(x => x))
                        {
                            UGITTask predTask = taskManager.Load(x => x.TicketId == TicketID && x.ID == UGITUtility.StringToInt(pred)).FirstOrDefault();
                            if (predTask != null)
                            {
                                newPredecessors.Add(Convert.ToString(predTask.ItemOrder));
                            }
                        }
                        model.Predecessors = string.Join(Constants.Separator6 + Constants.SpaceSeparator, newPredecessors);
                    }
                    model.Contribution = task.Contribution;
                    if (CriticalPathTasks.Any(x => x.ID == task.ID))
                    {
                        task.IsCritical = true;
                    }
                    model.isCritical = task.IsCritical;
                    tasklist.Add(model);
                }
                if (tasklist != null)
                {
                    var resultList = tasklist.OrderBy(x => x.ItemOrder);
                    string usersJson = JsonConvert.SerializeObject(resultList);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(usersJson, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTaskDataByUser: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateTaskData(string key, string values)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                UGITTask task = taskManager.GetTaskById(UGITUtility.ObjectToString(key));

                JsonConvert.PopulateObject(values, task);

                Validate(task);
                //if (!ModelState.IsValid)
                //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.GetFullErrorMessage());

                taskManager.Save(task);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateTaskData: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public HttpResponseMessage InsertTaskData(string TicketID, string values)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                UGITTask newTask = new UGITTask();
                JsonConvert.PopulateObject(values, newTask);
                newTask.TicketId = UGITUtility.ObjectToString(TicketID);
                newTask.ModuleNameLookup = uHelper.getModuleNameByTicketId(TicketID);
                Validate(newTask);
                taskManager.SaveTask(ref newTask, newTask.ModuleNameLookup, newTask.TicketId);
                //taskManager.Insert(newTask);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in InsertTaskData: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpDelete]
        public void DeleteTaskData(string key)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                UGITTask task = taskManager.GetTaskById(key);
                List<UGITTask> subtasks = taskManager.Load(x => x.ParentTaskID == Convert.ToInt32(key));

                foreach (UGITTask subtask in subtasks)
                {
                    List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == subtask.ID);
                    if (getChildTask != null && getChildTask.Count > 0)
                    {
                        taskManager.Delete(subtask);
                        if (subtask != null)
                            deleteChildTask(subtask);
                    }
                    else
                        taskManager.Delete(subtask);
                }
                if (task != null)
                    taskManager.Delete(task);

                void deleteChildTask(UGITTask subtask)
                {
                    List<UGITTask> SubChildTasks = taskManager.Load(x => x.ParentTaskID == subtask.ID);

                    foreach (UGITTask SubChildTask in SubChildTasks)
                    {
                        List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == SubChildTask.ID);
                        if (getChildTask != null && getChildTask.Count > 0)
                        {
                            taskManager.Delete(SubChildTask);
                            if (SubChildTask != null)
                                deleteChildTask(SubChildTask);
                        }
                        else
                            taskManager.Delete(SubChildTask);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteTaskData: " + ex);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateTask(FormDataCollection form)
        {
            try
            {
                if (form == null)
                    return Request.CreateResponse(HttpStatusCode.OK);

                var key = UGITUtility.StringToLong(form.Get("key"));
                var values = form.Get("values");
                var projectid = UGITUtility.ObjectToString(form.Get("TicketId"));
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                UGITTask task = taskManager.GetTaskById(UGITUtility.ObjectToString(key));

                if (task == null)
                {
                    task = new UGITTask();
                    task.TicketId = projectid;
                    task.ModuleNameLookup = uHelper.getModuleNameByTicketId(projectid);
                    // set dates to default dates if DueDate or Startdate not set for new task
                    if (!values.Contains(DatabaseObjects.Columns.DueDate))
                    {
                        task.DueDate = DateTime.Now.Date;
                        task.StartDate = DateTime.Now.Date;
                    }
                    if (!values.Contains(DatabaseObjects.Columns.StartDate))
                    {
                        task.StartDate = DateTime.Now.Date;
                    }
                }

                JsonConvert.PopulateObject(values, task);
                Validate(task);
                //if (task.PercentComplete > 0 && !string.IsNullOrEmpty(task.AssignedTo))
                //    task.AssignToPct = task.AssignedTo + Constants.Separator1 + "0";

                string oldAssignedTo = task.AssignedTo;
                List<UGITAssignTo> listAssignToPct = taskManager.GetUGITAssignPct(task.AssignToPct);
                task.AssignedTo = string.Empty;
                if (listAssignToPct != null && listAssignToPct.Count > 0)
                {
                    task.AssignedTo = string.Join(Constants.Separator, listAssignToPct.Select(x => x.ID));
                }

                //Load Task list before save for itemorder 
                List<UGITTask> tasklist = taskManager.Load(x => x.TicketId == task.TicketId && x.SubTaskType != "Risk" && x.SubTaskType != "Issue");

                //Convert Order id's to task ids for predessors
                string[] predessorsList = UGITUtility.SplitString(task.Predecessors, Constants.Separator6);
                List<string> predessorIds = new List<string>();
                foreach (string p in predessorsList)
                {
                    if (tasklist.Exists(x => x.ItemOrder == UGITUtility.StringToInt(p)))
                    {
                        UGITTask pretask = tasklist.First(x => x.ItemOrder == UGITUtility.StringToInt(p));
                        if (pretask != null) predessorIds.Add(UGITUtility.ObjectToString(pretask.ID));
                    }
                }
                //task.Predecessors = UGITUtility.ConvertListToString(predessorIds, Constants.Separator6);


                if (task.ItemOrder <= 0 && tasklist != null && tasklist.Count > 0)
                    task.ItemOrder = tasklist.Max(x => x.ItemOrder) + 1;
                // Donot Save task if task is NewTask, two tasks are default tasks
                if (!string.IsNullOrEmpty(task.Title) && task.Title != "(NewTask)")
                    taskManager.Save(task);

                tasklist = taskManager.Load(x => x.TicketId == task.TicketId && x.SubTaskType != "Risk" && x.SubTaskType != "Issue");
                taskManager.ReManageTasks(ref tasklist);
                taskManager.ReOrderTasks(ref tasklist);
                taskManager.SaveAllTasks(tasklist);
                taskManager.CallUpdateAllocationOnTaskSave(task.AssignedTo, task, task.ModuleNameLookup, task.TicketId);

                // Calculates project's startdate, enddate, pctcomplete, duration,  DaysToComplete, nextactivity and nextmilestone.
                if (!string.IsNullOrEmpty(task.TicketId))
                {
                    DataRow project = Ticket.GetCurrentTicket(context, task.ModuleNameLookup, task.TicketId);
                    taskManager.CalculateProjectStartEndDate(task.ModuleNameLookup, tasklist, project);

                    if (project != null)
                    {
                        //project.AcceptChanges();
                        Ticket ticketRequest = new Ticket(context, task.ModuleNameLookup);
                        ticketRequest.CommitChanges(project);
                    }
                }

                if (!oldAssignedTo.EqualsIgnoreCase(task.AssignedTo) && !string.IsNullOrEmpty(oldAssignedTo))
                    taskManager.DeleteAllocationOnTaskSave(oldAssignedTo, task.ModuleNameLookup, task.TicketId);

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateTask: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("InsertTask")]
        [HttpPost]
        public HttpResponseMessage InsertTask(FormDataCollection form)
        {
            try
            {
                var values = form.Get("values");
                var ticketid = form.Get("TicketId");
                string moduleName = uHelper.getModuleNameByTicketId(ticketid);
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                List<UGITTask> tasks = taskManager.LoadByProjectID(moduleName, ticketid);
                UGITTask newTask = new UGITTask();
                JsonConvert.PopulateObject(values, newTask);
                newTask.TicketId = UGITUtility.ObjectToString(ticketid);
                newTask.ModuleNameLookup = uHelper.getModuleNameByTicketId(ticketid);
                //DataRow dr = Ticket.GetCurrentTicket(context, moduleName, ticketid);
                //newTask.StageStep = (int)dr[DatabaseObjects.Columns.StageStep];
                int maxItemOrder = 0;
                Validate(newTask);
                if (newTask.ParentTaskID != 0)
                {
                    List<UGITTask> childTask = tasks.Where(x => x.ParentTaskID == newTask.ParentTaskID).ToList();
                    UGITTask ptask = taskManager.LoadByID(newTask.ParentTaskID);
                    ptask.ChildCount = ptask.ChildCount + 1;
                    if (childTask != null && childTask.Count > 0)
                        maxItemOrder = childTask.Max(x => x.ItemOrder);
                    else
                    {
                        if (ptask != null)
                            maxItemOrder = ptask.ItemOrder;
                    }
                }
                //Added by Inderjeet Kaur on 11 Oct '22. Negative newTask.ID means that a new task has to be created below this ID.
                //Find out itemOrder on the basis of its child (if exists), and create a task at this position.
                else if (newTask.ID < 0 && newTask.ParentTaskID == 0)
                {
                    UGITTask prevTask = taskManager.LoadByID(-newTask.ID);
                    List<UGITTask> childTask = tasks.Where(x => x.ParentTaskID == prevTask.ID).ToList();
                    if (childTask != null && childTask.Count > 0)
                        maxItemOrder = childTask.Max(x => x.ItemOrder) - 1;
                    else
                    {
                        maxItemOrder = prevTask.ItemOrder - 1;
                    }
                    newTask.ID = 0;
                }
                else
                {
                    if (tasks != null && tasks.Count > 0)
                        maxItemOrder = tasks.Max(x => x.ItemOrder);
                }
                newTask.ItemOrder = maxItemOrder + 1;
                //if (newTask.PercentComplete > 0 && !string.IsNullOrEmpty(newTask.AssignedTo))
                //    newTask.AssignToPct = newTask.AssignedTo + Constants.Separator1 + newTask.PercentComplete;

                List<UGITAssignTo> listAssignToPct = taskManager.GetUGITAssignPct(newTask.AssignToPct);
                newTask.AssignedTo = string.Empty;
                if (listAssignToPct != null && listAssignToPct.Count > 0)
                {
                    newTask.AssignedTo = string.Join(Constants.Separator, listAssignToPct.Select(x => x.ID));
                }
                // Donot Save task if task is NewTask or NewChildTask, two tasks are default tasks
                if (!string.IsNullOrEmpty(newTask.Title) && newTask.Title != "(NewTask)")
                    taskManager.SaveTask(ref newTask, newTask.ModuleNameLookup, newTask.TicketId);
                List<UGITTask> tasklist = taskManager.Load(x => x.TicketId == ticketid && x.SubTaskType != "Risk" && x.SubTaskType != "Issue");

                taskManager.ReManageTasks(ref tasklist);
                taskManager.ReOrderTasks(ref tasklist);
                taskManager.SaveAllTasks(tasklist);
                taskManager.CallUpdateAllocationOnTaskSave(newTask.AssignedTo, newTask, newTask.ModuleNameLookup, newTask.TicketId);
                //List<UGITTask> returntasklist = taskManager.Load(x => x.TicketId == ticketid);
                //var responseobj = new { taskid = newTask.ParentTaskID, data = returntasklist };
                //return Request.CreateResponse(HttpStatusCode.Created, responseobj);
                UGITTask task = tasklist.FirstOrDefault(x => x.ID == newTask.ID);
                List<string> returnids = new List<string>();
                UGITUtility.GetParentTaskID(task, returnids);
                return Request.CreateResponse(returnids);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in InsertTask: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        public void DeleteTask(FormDataCollection form)
        {
            try
            {
                var key = Convert.ToInt32(form.Get("key"));
                var ticketid = form.Get("TicketId");
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                UGITTask task = taskManager.GetTaskById(UGITUtility.ObjectToString(key));
                List<UGITTask> subtasks = taskManager.Load(x => x.ParentTaskID == Convert.ToInt32(form.Get("key")));

                foreach (UGITTask subtask in subtasks)
                {
                    List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == subtask.ID);
                    if (getChildTask != null && getChildTask.Count > 0)
                    {
                        taskManager.Delete(subtask);
                        if (subtask != null)
                            deleteChildTask(subtask);
                    }
                    else
                        taskManager.Delete(subtask);
                }
                if (task != null)
                    taskManager.Delete(task);

                void deleteChildTask(UGITTask subtask)
                {
                    List<UGITTask> SubChildTasks = taskManager.Load(x => x.ParentTaskID == subtask.ID);

                    foreach (UGITTask SubChildTask in SubChildTasks)
                    {
                        List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == SubChildTask.ID);
                        if (getChildTask != null && getChildTask.Count > 0)
                        {
                            taskManager.Delete(SubChildTask);
                            if (SubChildTask != null)
                                deleteChildTask(SubChildTask);
                        }
                        else
                            taskManager.Delete(SubChildTask);
                    }
                }
                List<UGITTask> tasks = taskManager.Load(x => x.TicketId == ticketid && x.SubTaskType != "Risk" && x.SubTaskType != "Issue");
                taskManager.ReManageTasks(ref tasks);
                taskManager.ReOrderTasks(ref tasks);
                taskManager.SaveAllTasks(tasks);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteTask: " + ex);
            }
        }

        [HttpDelete]
        [Route("DeleteHomePageTask")]
        public void DeleteHomePageTask(FormDataCollection form)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                var key = Convert.ToInt32(form.Get("key"));
                string ticketid = UGITUtility.ObjectToString(form.Get("TaskKeys[TicketId]"));
                string mode = UGITUtility.ObjectToString(form.Get("TaskKeys[mode]"));
                long taskid = UGITUtility.StringToLong(form.Get("TaskKeys[TaskId]"));
                if (mode == "ModuleStageConstraints")
                {
                    ModuleStageConstraintsManager constraintManager = new ModuleStageConstraintsManager(context);
                    ModuleStageConstraints constraint = null;
                    if (taskid > 0)
                    {
                        constraint = constraintManager.LoadByID(taskid);
                        if (constraint != null)
                        {
                            constraint.Deleted = true;
                            constraintManager.Update(constraint);
                        }
                    }
                }
                if (mode == "ModuleTasks")
                {
                    UGITTaskManager taskManager = new UGITTaskManager(context);
                    UGITTask task = taskManager.GetTaskById(UGITUtility.ObjectToString(taskid));
                    List<UGITTask> subtasks = taskManager.Load(x => x.ParentTaskID == taskid);

                    foreach (UGITTask subtask in subtasks)
                    {
                        List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == subtask.ID);
                        if (getChildTask != null && getChildTask.Count > 0)
                        {
                            taskManager.Delete(subtask);
                            if (subtask != null)
                                deleteChildTask(subtask);
                        }
                        else
                            taskManager.Delete(subtask);
                    }
                    if (task != null)
                        taskManager.Delete(task);

                    void deleteChildTask(UGITTask subtask)
                    {
                        List<UGITTask> SubChildTasks = taskManager.Load(x => x.ParentTaskID == subtask.ID);

                        foreach (UGITTask SubChildTask in SubChildTasks)
                        {
                            List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == SubChildTask.ID);
                            if (getChildTask != null && getChildTask.Count > 0)
                            {
                                taskManager.Delete(SubChildTask);
                                if (SubChildTask != null)
                                    deleteChildTask(SubChildTask);
                            }
                            else
                                taskManager.Delete(SubChildTask);
                        }
                    }
                    List<UGITTask> tasks = taskManager.Load(x => x.TicketId == ticketid);
                    taskManager.ReManageTasks(ref tasks);
                    taskManager.ReOrderTasks(ref tasks);
                    taskManager.SaveAllTasks(tasks);
                }
                if (mode == "CRMActivities")
                {
                    CRMActivitiesManager activitiesManager = new CRMActivitiesManager(context);
                    CRMActivities activity = null;
                    if (taskid > 0)
                    {
                        activity = activitiesManager.LoadByID(taskid);
                        if (activity != null)
                        {
                            //activity.IsDeleted = true;
                            activity.Deleted = true;
                            activitiesManager.Update(activity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteHomePageTask: " + ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage DragAndDrop(string TicketPublicId, string toKey, string fromKey, string firstAddInCategory = null)
        {
            try
            {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            UGITTaskManager taskManager = new UGITTaskManager(context);
            taskManager.UpdateTaskListItemOrder(TicketPublicId, toKey, fromKey, firstAddInCategory);
            return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DragAndDrop: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        [Route("MarkTaskAsInProgress")]
        public HttpResponseMessage MarkTaskAsInProgress(TaskListAction taskTaskComplete) {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                string TicketPublicId = taskTaskComplete.TicketPublicId;
                ModuleStageConstraintsManager constraintManager = new ModuleStageConstraintsManager(context);
                List<ModuleStageConstraints> constraintTasks = constraintManager.Load(x => x.TicketId == TicketPublicId);
                string taskconstraintId = taskTaskComplete.TaskKeys.FirstOrDefault();
                if (constraintTasks != null && constraintTasks.Count > 0)
                {
                    ModuleStageConstraints constraintTask = constraintTasks.First(x => x.ID == UGITUtility.StringToLong(taskconstraintId));
                    if (constraintTask != null)
                        UGITModuleConstraint.MarkStageTaskAsInProgress(UGITUtility.StringToLong(taskconstraintId), context);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in MarkTaskAsInProgress: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("MarkTaskAsComplete")]
        public HttpResponseMessage MarkTaskAsComplete(TaskListAction taskTaskComplete)
        {
            try
            {
                string TicketPublicId = taskTaskComplete.TicketPublicId;

                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
                bool RunApproveService = false;
                long taskID = 0;
                bool taskOnStagedifferentfromTicket = false;
                string ModuleName = uHelper.getModuleNameByTicketId(TicketPublicId);
                List<UGITTask> tasks = uGITTaskManager.LoadByProjectID(ModuleName, TicketPublicId);
                tasks = UGITTaskManager.MapRelationalObjects(tasks);

                if (taskTaskComplete.TaskType == "ModuleTasks")
                {
                    foreach (string taskkey in taskTaskComplete.TaskKeys)
                    {
                        taskID = UGITUtility.StringToLong(taskkey);
                        UGITTask task = tasks.FirstOrDefault(x => x.ID == taskID);
                        if (task != null)
                        {
                            string oldStatus = task.Status;
                            double oldPctComplete = task.PercentComplete;

                            if (oldStatus != Constants.Completed || oldPctComplete != 100)
                            {
                                if (ModuleName == Constants.ExitCriteria)
                                {
                                    RunApproveService = true;
                                    RunApproveService = uGITTaskManager.isRequestTaskCompleted(task);
                                    DataRow dr = Ticket.GetCurrentTicket(context, ModuleName, TicketPublicId);
                                    int ticketStage = (int)dr[DatabaseObjects.Columns.StageStep];
                                    if (ticketStage != task.StageStep)
                                    {
                                        taskOnStagedifferentfromTicket = true;
                                    }
                                }

                                if (!taskOnStagedifferentfromTicket)
                                {
                                    task.Status = Constants.Completed;
                                    task.PercentComplete = 100;
                                    //completed on..
                                    task.CompletionDate = DateTime.Now;
                                    task.Changes = true;

                                    uGITTaskManager.PropagateTaskStatusEffect(ref tasks, task);
                                    uGITTaskManager.SaveTasks(ref tasks, ModuleName, TicketPublicId);
                                    DataRow ticketItem = null;
                                    DataTable moduleTicketList = null;
                                    ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                                    TicketManager ticketManager = new TicketManager(context);
                                    UGITModule moduleObj = moduleViewManager.LoadByName(ModuleName);
                                    if (moduleObj != null)
                                    {
                                        moduleTicketList = ticketManager.GetAllTickets(moduleObj);
                                        ticketItem = moduleTicketList.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, TicketPublicId))[0];
                                        //Calculate project related values
                                        uGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasks, ticketItem);
                                    };

                                    string historyDesc = string.Empty;
                                    if (oldPctComplete != task.PercentComplete)
                                    {
                                        historyDesc = string.Format("Task [{0}]:", task.Title);
                                        historyDesc += string.Format(" {0}% => {1}%", oldPctComplete, task.PercentComplete);
                                    }
                                    if (oldStatus != task.Status)
                                    {
                                        if (historyDesc == string.Empty)
                                            historyDesc += string.Format("Task [{0}]:", task.Title);
                                        else
                                            historyDesc += ",";
                                        historyDesc += string.Format(" {0} => {1}", oldStatus, task.Status);
                                    }
                                    int updatedTasks = tasks.Where(x => x.Changes).Count();
                                }
                            }
                        }

                    }
                }
                if (taskTaskComplete.TaskType == "ModuleStageConstraints")
                {
                    ModuleStageConstraintsManager constraintManager = new ModuleStageConstraintsManager(context);
                    List<ModuleStageConstraints> constraintTasks = constraintManager.Load(x => x.TicketId == TicketPublicId);
                    string taskconstraintId = taskTaskComplete.TaskKeys.FirstOrDefault();
                    if (constraintTasks != null && constraintTasks.Count > 0)
                    {
                        ModuleStageConstraints constraintTask = constraintTasks.First(x => x.ID == UGITUtility.StringToLong(taskconstraintId));
                        if (constraintTask != null)
                            UGITModuleConstraint.MarkStageTaskAsComplete(UGITUtility.StringToLong(taskconstraintId), context);
                    }

                }
                if (taskTaskComplete.TaskType == "CRMActivities")
                {
                    CRMActivitiesManager activitiesManager = new CRMActivitiesManager(context);
                    CRMActivities activity = null;
                    foreach (string taskkey in taskTaskComplete.TaskKeys)
                    {
                        activity = activitiesManager.LoadByID(UGITUtility.StringToLong(taskkey));
                        if (activity != null)
                        {
                            activity.ActivityStatus = Constants.Completed;
                            activitiesManager.Update(activity);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in MarkTaskAsComplete: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("DuplicateTask")]
        public HttpResponseMessage DuplicateTask(DuplicateTaskData duplicateTaskData)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
                int taskid = 0;
                string ticketId = duplicateTaskData.ticketId;
                bool copyChild = duplicateTaskData.copyChild;
                string moduleName = uHelper.getModuleNameByTicketId(ticketId);
                foreach (string taskkey in duplicateTaskData.TaskKeys)
                {
                    taskid = UGITUtility.StringToInt(taskkey);
                    uGITTaskManager.CopyTask(taskid, ticketId, copyChild, moduleName);
                }
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DuplicateTask: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("AutoAdjustSchedule")]
        public HttpResponseMessage AutoAdjustSchedule(string ticketID)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
                string ModuleName = uHelper.getModuleNameByTicketId(ticketID);
                List<UGITTask> tasks = uGITTaskManager.LoadByProjectID(ModuleName, ticketID);
                tasks = UGITTaskManager.MapRelationalObjects(tasks);
                uGITTaskManager.AutoAdjustSchedules(ref tasks, ModuleName, ticketID);

                DataRow project = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, ticketID);
                uGITTaskManager.CalculateProjectStartEndDate(ModuleName, tasks, project);
                Ticket ticket = new Ticket(HttpContext.Current.GetManagerContext(), ModuleName);
                ticket.CommitChanges(project, string.Empty);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in AutoAdjustSchedule: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("ImportTasksMPP")]
        public HttpResponseMessage ImportTasksMPP(string ticketID, string filename, bool importDatesOnly = false, bool dontImportAssignee = false, bool calculateEstHrs = false)
        {
            try
            {
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectManager projectManager = new ProjectManager(context);
            UGITTaskManager taskManager = new UGITTaskManager(context);
            string ModuleName = uHelper.getModuleNameByTicketId(ticketID);

            projectManager.ImportTask_MSProject(filename, ticketID, importDatesOnly, dontImportAssignee, calculateEstHrs);

            return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ImportTasksMPP: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        [Route("ExportTasksMPP")]
        public async Task<IHttpActionResult> ExportTasksMPP(string ticketID)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ConfigurationVariableManager ObjConfigVariableHelper = new ConfigurationVariableManager(context);
                UGITTaskManager taskManager = new UGITTaskManager(context);
                UserProfileManager UserManager = new UserProfileManager(context);
                string serverLocation = uHelper.GetTempFolderPath();
                string fileName = "ProjectTasks_" + ticketID + ".xml";
                string fullFileName = serverLocation + fileName;
                string moduleName = uHelper.getModuleNameByTicketId(ticketID);
                Project project = new Project();
                DataTable tasks = taskManager.LoadTasksTable(moduleName, false, ticketID);
                if (tasks != null && tasks.Rows.Count > 0)
                {
                    DataRow[] arrColl = tasks.AsEnumerable().OrderBy(x => UGITUtility.StringToInt(x["ItemOrder"])).ToArray();
                    if (arrColl != null && arrColl.Length > 0)
                        tasks = arrColl.CopyToDataTable();
                }
                ConfigSetiingMpp configSettingMpp = new ConfigSetiingMpp();
                configSettingMpp.EnableExportImport = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.EnableProjectExportImport));
                configSettingMpp.UseMSProject = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.MSProjectImportExportEnabled));
                project.datapropert = new List<Util.ImportExportMPP.DataPropert>();
                Util.ImportExportMPP.TaskList task;
                if (tasks != null && tasks.Rows.Count > 0)
                {

                    project.taskList = new List<Util.ImportExportMPP.TaskList>();
                    foreach (DataRow taskRow in tasks.Rows)
                    {
                        task = new Util.ImportExportMPP.TaskList();
                        //Task task = project.AddTask();
                        string title = UGITUtility.StripHTML(Convert.ToString(taskRow[DatabaseObjects.Columns.Title])); // UGITUtility.StripHTML(Convert.ToString(taskRow[DatabaseObjects.Columns.Title]));
                        task.Name = title;
                        task.Start = UGITUtility.StringToDateTime((taskRow[DatabaseObjects.Columns.StartDate]));
                        task.ActualStart = UGITUtility.StringToDateTime((taskRow[DatabaseObjects.Columns.StartDate]));
                        double duration = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(taskRow[DatabaseObjects.Columns.UGITDuration])))
                            duration = UGITUtility.StringToDouble((taskRow[DatabaseObjects.Columns.UGITDuration]));

                        // task.Duration = Duration.getInstance(work, TimeUnit.DAYS);
                        task.Duration = duration;
                        double work = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(taskRow[DatabaseObjects.Columns.TaskEstimatedHours])))
                            work = UGITUtility.StringToDouble((taskRow[DatabaseObjects.Columns.TaskEstimatedHours]));
                        if (work > 0)
                            task.Work = work;

                        double actualWork = UGITUtility.StringToDouble(taskRow[DatabaseObjects.Columns.TaskActualHours]);
                        task.ActualWork = actualWork; //Duration.getInstance(actualWork, TimeUnit.HOURS);
                        double remainingWork = UGITUtility.StringToDouble(taskRow[DatabaseObjects.Columns.EstimatedRemainingHours]);
                        task.RemainingWork = remainingWork; //Duration.getInstance(remainingWork, TimeUnit.HOURS);
                        task.Notes = Convert.ToString(taskRow[DatabaseObjects.Columns.Description]);
                        if (tasks.Columns.Contains(DatabaseObjects.Columns.PercentComplete))
                        {
                            task.PercentageComplete = (Convert.ToDouble(taskRow[DatabaseObjects.Columns.PercentComplete]));
                            task.PercentComplete = (Convert.ToDouble(taskRow[DatabaseObjects.Columns.PercentComplete]));
                            task.PercentageWorkComplete = Convert.ToDouble(taskRow[DatabaseObjects.Columns.PercentComplete]);
                        }
                        task.Status = Convert.ToString(taskRow[DatabaseObjects.Columns.Status]);
                        if (tasks.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour) && Convert.ToString(taskRow[DatabaseObjects.Columns.TaskBehaviour]) == Constants.TaskType.Milestone)
                            task.Milestone = true;
                        else
                            task.Milestone = false;

                        task.OutlineLevel = (Convert.ToString(Convert.ToInt32(taskRow[DatabaseObjects.Columns.UGITLevel]) + 1));
                        task.Finish = UGITUtility.StringToDateTime((taskRow[DatabaseObjects.Columns.DueDate]));
                        //Util.ImportExportMPP.DataPropert dataPropert = new DataPropert();
                        //dataPropert.PMMID = Convert.ToInt32(taskRow[DatabaseObjects.Columns.Id]);
                        //dataPropert.PredecessorsID =Convert.ToString(UGITUtility.GetSPItemValue(taskRow, DatabaseObjects.Columns.Predecessors));
                        //dataPropert.TaskIndex = task.Id;
                        task.taskIds = UGITUtility.ConvertStringToList(Convert.ToString(UGITUtility.GetSPItemValue(taskRow, DatabaseObjects.Columns.Predecessors)), ",");
                        task.Id = Convert.ToInt32(taskRow[DatabaseObjects.Columns.Id]);
                        // task.ParentTask = Convert.ToString(taskRow[DatabaseObjects.Columns.ParentTaskID]);
                        //project.datapropert.Add(dataPropert);

                        //new line for code for Export/Import task.
                        if (UGITUtility.IsSPItemExist(taskRow, DatabaseObjects.Columns.UGITAssignToPct))
                        {
                            List<UGITAssignTo> listAssignTo = taskManager.GetUGITAssignPctExport(Convert.ToString(taskRow[DatabaseObjects.Columns.UGITAssignToPct]));
                            string strAssignToPct = string.Empty;
                            task.listAssignTo = listAssignTo;
                            foreach (var item in listAssignTo)
                            {
                                if (!string.IsNullOrEmpty(item.LoginName))
                                {
                                    UserProfile user = UserManager.GetUserByUserName(item.LoginName); //.GetUserByName(item.LoginName, SPPrincipalType.User);
                                    if (user != null)
                                    {
                                        if (!string.IsNullOrEmpty(strAssignToPct))
                                            strAssignToPct += Constants.UserInfoSeparator;
                                        strAssignToPct += user.UserName + "[" + item.Percentage + "%]";
                                    }
                                }
                            }
                            task.ResourceNames = strAssignToPct.Replace(";", ",");
                        }
                        project.taskList.Add(task);
                    }
                    ExportManagerMpp exportManagerMpp = new ExportManagerMpp(configSettingMpp, project, fileName, fullFileName);
                    exportManagerMpp.ExportTask();
                    if (System.IO.File.Exists(fullFileName))
                    {
                        string filePath = String.Format("Content\\IMAGES\\ugovernit\\upload\\{0}", fileName);
                        string fullFilePath = UGITUtility.GetAbsoluteURL(filePath);
                        //string usersJson = JsonConvert.SerializeObject(fullFilePath);
                        string usersJson = JsonConvert.SerializeObject(new { fullFilePath, fileName });
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(usersJson, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ExportTasksMPP: " + ex);
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("DecreaseIndent")]
        public HttpResponseMessage DecreaseIndent(TaskListAction taskListAction)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                int taskID = 0;
                foreach (string taskkey in taskListAction.TaskKeys)
                {
                    taskID = UGITUtility.StringToInt(taskkey);
                    if (taskID > 0)
                        taskManager.DecIndent(taskListAction.TicketPublicId, taskID);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DecreaseIndent: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        [Route("IncreaseIndent")]
        public HttpResponseMessage IncreaseIndent(TaskListAction taskListAction)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                int taskID = 0;
                foreach (string taskkey in taskListAction.TaskKeys)
                {
                    taskID = UGITUtility.StringToInt(taskkey);
                    //Send all the taskIds, even if they are negative. Negative id indicates the itemOrder of the newly added task.
                    taskManager.IncIndent(taskListAction.TicketPublicId, taskID);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in IncreaseIndent: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpDelete]
        [Route("DeleteSelectedTask")]
        public HttpResponseMessage DeleteSelectedTask(TaskListAction taskListAction)
        {
            try
            {
                string TicketPublicId = taskListAction.TicketPublicId;
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                long taskID = 0;
                foreach (string taskkey in taskListAction.TaskKeys)
                {
                    taskID = UGITUtility.StringToLong(taskkey);
                    UGITTask task = taskManager.LoadByID(taskID);
                    if (task == null)
                        continue;
                    List<UGITTask> subtasks = taskManager.Load(x => x.ParentTaskID == taskID);

                    foreach (UGITTask subtask in subtasks)
                    {
                        List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == subtask.ID);
                        if (getChildTask != null && getChildTask.Count > 0)
                        {
                            taskManager.Delete(subtask);
                            if (subtask != null)
                                deleteChildTask(subtask);
                        }
                        else
                            taskManager.Delete(subtask);
                    }
                    if (task != null)
                        taskManager.Delete(task);

                    void deleteChildTask(UGITTask subtask)
                    {
                        List<UGITTask> SubChildTasks = taskManager.Load(x => x.ParentTaskID == subtask.ID);

                        foreach (UGITTask SubChildTask in SubChildTasks)
                        {
                            List<UGITTask> getChildTask = taskManager.Load(x => x.ParentTaskID == SubChildTask.ID);
                            if (getChildTask != null && getChildTask.Count > 0)
                            {
                                taskManager.Delete(SubChildTask);
                                if (SubChildTask != null)
                                    deleteChildTask(SubChildTask);
                            }
                            else
                                taskManager.Delete(SubChildTask);
                        }
                    }
                }
                List<UGITTask> tasks = taskManager.Load(x => x.TicketId == TicketPublicId);
                taskManager.ReManageTasks(ref tasks);
                taskManager.ReOrderTasks(ref tasks);
                taskManager.SaveAllTasks(tasks);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteSelectedTask: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpDelete]
        [Route("DeleteAllTask")]
        public HttpResponseMessage DeleteAllTask(string ticketID)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager taskManager = new UGITTaskManager(context);
                string moduleName = uHelper.getModuleNameByTicketId(ticketID);
                List<UGITTask> projectTasks = taskManager.LoadByProjectID(ticketID);
                if (projectTasks != null && projectTasks.Count > 0)
                {
                    taskManager.Delete(projectTasks);
                    projectTasks = taskManager.LoadByProjectID(moduleName, ticketID);
                    DataRow ticketItem = Ticket.GetCurrentTicket(context, moduleName, ticketID);
                    taskManager.CalculateProjectStartEndDate(moduleName, projectTasks, ticketItem);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteAllTask: " + ex);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        [HttpGet]
        [Route("getmoduleshowdetail")]
        public async Task<IHttpActionResult> getmoduleshowdetail(string TicketId)
        {
            try
            {
                await Task.FromResult(0);
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                string modulename = uHelper.getModuleNameByTicketId(TicketId);
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                UGITModule module = moduleManager.GetByName(modulename);

                TicketManager ticketManager = new TicketManager(context);
                DataRow ticketrow = ticketManager.GetByTicketID(module, TicketId);

                if (ticketrow != null)
                {
                    PageURLResponse dataResponse = new PageURLResponse()
                    {
                        TicketTitle = UGITUtility.ObjectToString(ticketrow[DatabaseObjects.Columns.Title]),
                        ModuleURL = module.StaticModulePagePath
                    };
                    string returnData = JsonConvert.SerializeObject(dataResponse);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(returnData, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in getmoduleshowdetail: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetChoiceFieldOptions")]
        public async Task<IHttpActionResult> GetChoiceFieldOptions(string FieldName, string TableName)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
                FieldConfiguration fieldConfig = fieldManager.GetFieldByFieldName(FieldName, TableName);

                if (fieldConfig != null)
                {
                    //List<string> fieldOptions = UGITUtility.SplitString(fieldConfig.Data, Constants.Separator).ToList();
                    //fieldOptions = fieldOptions.OrderBy(x => x).ToList();
                    string jsonCTypes = JsonConvert.SerializeObject(fieldConfig.Data);

                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonCTypes, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetChoiceFieldOptions: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetChoiceTypeFields")]
        public async Task<IHttpActionResult> GetChoiceTypeFields(string SearchValue)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
                List<FieldConfiguration> lstFieldConfig = fieldManager.Load(x => x.Datatype == DatabaseObjects.DataTypes.Choices && x.TableName == SearchValue);
                lstFieldConfig = lstFieldConfig.DistinctBy(x => x.FieldName).ToList();
                if (!string.IsNullOrEmpty(SearchValue) && SearchValue != "null" && SearchValue != "undefined")
                    lstFieldConfig = lstFieldConfig.Where(x => x.TableName.IndexOf(SearchValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

                if (lstFieldConfig != null)
                {
                    string jsonCTypes = JsonConvert.SerializeObject(lstFieldConfig.OrderBy(x => x.FieldName));

                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonCTypes, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetChoiceTypeFields: " + ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("GetChoiceTypeTable")]
        public async Task<IHttpActionResult> GetChoiceTypeTable(string SearchValue)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);

                List<FieldConfiguration> lstFieldConfig = fieldManager.Load(x => x.Datatype == DatabaseObjects.DataTypes.Choices && x.TableName != null);
                lstFieldConfig = lstFieldConfig.DistinctBy(x => x.TableName).ToList();
                if (!string.IsNullOrEmpty(SearchValue) && SearchValue != "null" && SearchValue != "undefined")
                    lstFieldConfig = lstFieldConfig.Where(x => x.TableName.IndexOf(SearchValue, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

                if (lstFieldConfig != null)
                {
                    string jsonCTypes = JsonConvert.SerializeObject(lstFieldConfig.OrderBy(x => x.TableName));

                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonCTypes, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetChoiceTypeTable: " + ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("UpdateChoiceFieldOptions")]
        public async Task<IHttpActionResult> UpdateChoiceFieldOptions(string TableName, string FieldName, string values)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
                FieldConfiguration field = fieldManager.GetFieldByFieldName(FieldName, TableName);

                if (field != null)
                {
                    field.Data = values.Replace(Constants.Separator2, Constants.Separator);
                    fieldManager.Update(field);

                    CacheHelper<FieldConfiguration>.AddOrUpdate(string.Format("{0}_{1}", field.FieldName, field.TableName), context.TenantID, field);
                    //var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    //response.Content = new StringContent(field.Data, Encoding.UTF8, "application/json");
                    //return ResponseMessage(response);
                }
                return Ok(field.Data);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateChoiceFieldOptions: " + ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("GetDeletedStudioData")]
        public async Task<IHttpActionResult> GetDeletedStudioData()
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                StudioManager studioManagerObj = new StudioManager(context);
                List<Studio> lstStudios = studioManagerObj.Load(x => x.Deleted).OrderBy(x => x.FieldDisplayName).ToList();

                return Ok(lstStudios);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDeletedStudioData: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetStudioData")]
        public async Task<IHttpActionResult> GetStudioData([FromUri] string showArchived)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                StudioManager studioManagerObj = new StudioManager(context);
                List<Studio> lstStudios = studioManagerObj.Load(x => x.Deleted == UGITUtility.StringToBoolean(showArchived)).OrderBy(x => x.FieldDisplayName).ToList();

                return Ok(lstStudios);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetStudioData: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetDivsionData")]
        public async Task<IHttpActionResult> GetDivsionData()
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                CompanyDivisionManager divisionManagerObj = new CompanyDivisionManager(context);
                List<CompanyDivision> lstStudios = divisionManagerObj.Load();

                return Ok(lstStudios);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDivsionData: " + ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("AddUpdateStudio")]
        public async Task<IHttpActionResult> AddUpdateStudio([FromBody] StudioRequest request)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                StudioManager studioManagerObj = new StudioManager(context);
                CompanyDivisionManager objCompanyDivisionManager = new CompanyDivisionManager(context);
                ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(context);
                bool enableStudioDivisionHierarchy = objConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);
                if (request != null)
                {
                    string errormsg = string.Empty;

                    if (UGITUtility.StringToLong(request.ID) > 0)
                    {
                        Studio oldStudioObj = studioManagerObj.LoadByID(UGITUtility.StringToLong(request.ID));
                        oldStudioObj.DivisionLookup = UGITUtility.StringToLong(request.DivisionLookup) > 0 ? UGITUtility.StringToLong(request.DivisionLookup) : oldStudioObj.DivisionLookup;
                        oldStudioObj.Title = !string.IsNullOrEmpty(request.Title) ? request.Title : oldStudioObj.Title;
                        oldStudioObj.Description = !string.IsNullOrEmpty(request.Description) ? request.Description : oldStudioObj.Description;

                        if (request.Title != null || request.DivisionLookup != null)
                            errormsg = ValidateStudioTitle(oldStudioObj, studioManagerObj, enableStudioDivisionHierarchy);

                        if (enableStudioDivisionHierarchy)
                            oldStudioObj.FieldDisplayName = string.Format("{0} > {1}", objCompanyDivisionManager.LoadByID(UGITUtility.StringToLong(oldStudioObj.DivisionLookup)).Title,
                                oldStudioObj.Title);
                        else
                            oldStudioObj.FieldDisplayName = oldStudioObj.Title;

                        if (!string.IsNullOrEmpty(errormsg))
                            return Ok(errormsg);
                        studioManagerObj.Update(oldStudioObj);
                    }
                    else
                    {
                        if (enableStudioDivisionHierarchy && string.IsNullOrEmpty(request.DivisionLookup))
                        {
                            errormsg = "DivisionRequired";
                            return Ok(errormsg);
                        }
                        Studio newStudioObj = new Studio()
                        {
                            ID = UGITUtility.StringToLong(request.ID),
                            DivisionLookup = UGITUtility.StringToLong(request.DivisionLookup),
                            Title = request.Title,
                            Description = request.Description
                        };
                        errormsg = ValidateStudioTitle(newStudioObj, studioManagerObj, enableStudioDivisionHierarchy);

                        if (enableStudioDivisionHierarchy)
                            newStudioObj.FieldDisplayName = string.Format("{0} > {1}", objCompanyDivisionManager.LoadByID(UGITUtility.StringToLong(request.DivisionLookup)).Title,
                                request.Title);
                        else
                            newStudioObj.FieldDisplayName = request.Title;

                        if (!string.IsNullOrEmpty(errormsg))
                            return Ok(errormsg);
                        studioManagerObj.Insert(newStudioObj);
                    }
                }
                List<Studio> lstStudios = studioManagerObj.Load(x => !x.Deleted).OrderBy(x => x.FieldDisplayName).ToList();

                return Ok(lstStudios);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in AddUpdateStudio: " + ex);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("DeleteStudio")]
        public async Task<IHttpActionResult> DeleteStudio(FormDataCollection form)
        {
            await Task.FromResult(0);
            try
            {
                var key = UGITUtility.StringToLong(form.Get("key"));
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                StudioManager studioManagerObj = new StudioManager(context);
                if (key > 0)
                {
                    string errormsg = string.Empty;
                    Studio oldStudioObj = studioManagerObj.LoadByID(key);
                    oldStudioObj.Deleted = !oldStudioObj.Deleted;
                    studioManagerObj.Update(oldStudioObj);
                }
                List<Studio> lstStudios = studioManagerObj.Load(x => !x.Deleted).OrderBy(x => x.FieldDisplayName).ToList();

                return Ok(lstStudios);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteStudio: " + ex);
                return InternalServerError();
            }
        }

        public string ValidateStudioTitle(Studio requestobj, StudioManager studiomanagerObj, bool enableStudioDivisionHierarchy)
        {
            string result = string.Empty;
            try
            {
                //check duplicate values
                if (!string.IsNullOrEmpty(requestobj.Title))
                {
                    List<Studio> lstDuplicateStudios;
                    if (enableStudioDivisionHierarchy)
                    {
                        lstDuplicateStudios = studiomanagerObj.Load(x => x.Title == requestobj.Title &&
                        x.DivisionLookup == UGITUtility.StringToLong(requestobj.DivisionLookup));
                    }
                    else
                        lstDuplicateStudios = studiomanagerObj.Load(x => x.Title == requestobj.Title);

                    if (lstDuplicateStudios != null && lstDuplicateStudios.Count > 0)
                        result = "Duplicate";
                }
                else
                    result = "EmptyTitle";
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ValidateStudioTitle: " + ex);
            }
            return result;
        }

        [HttpPost]
        [Route("OnPredecessorChange")]
        public async Task<IHttpActionResult> OnPredecessorChange(int taskid, string projectId, string moduleName)
        {
            StringBuilder sbjson = new StringBuilder();
            try
            {
                await Task.FromResult(0);
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UGITTaskManager ugitTaskManagerObj = new UGITTaskManager(context);
                List<UGITTask> _HideTasksInPred = new List<UGITTask>();
                List<UGITTask> tasks = ugitTaskManagerObj.LoadByProjectID(moduleName, projectId);
                tasks = UGITTaskManager.MapRelationalObjects(tasks);
                int maxItemOrder = tasks.Max(x => x.ItemOrder);
                var currentTask = tasks.FirstOrDefault(x => x.ID == taskid);

                if (currentTask != null)
                {
                    UGITTaskManager.GetDependentTasks(currentTask, ref _HideTasksInPred, false);
                    foreach (UGITTask depTask in _HideTasksInPred)
                    {

                    }
                    if (_HideTasksInPred != null && _HideTasksInPred.Count > 0)
                    {
                        sbjson.Append("{");
                        sbjson.Append("\"messagecode\":\"2\",");
                        sbjson.Append("\"message\":\"success\",");
                        sbjson.Append("\"maxitemorder\":\"" + maxItemOrder + "\",");
                        sbjson.Append("\"value\":");
                        sbjson.Append("\"" + string.Join(",", _HideTasksInPred.Select(x => x.ItemOrder).ToArray()) + "\"");
                        sbjson.Append("}");
                    }

                }
                return Ok(sbjson.ToString());
            }
            catch(Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in OnPredecessorChange: " + ex);
                return Ok("{\"messagecode\":\"0\",\"message\":\"error\"}");
            }
        }

        [HttpGet]
        [Route("GetLeads")]
        public async Task<IHttpActionResult> GetLeads()
        {
            await Task.FromResult(0);
            try
            {
                UGITModule moduleData = _moduleManager.LoadByName(ModuleNames.LEM, true);
                List<LeadResponse> lstResponse = new List<LeadResponse>();
                DataTable dtResult = _ticketManager.GetOpenTickets(moduleData);
                if (dtResult != null)
                {
                    foreach (DataRow row in dtResult.Rows)
                    {
                        LeadResponse item = new LeadResponse();
                        item.ID = UGITUtility.StringToLong(row[DatabaseObjects.Columns.ID]);
                        item.Title = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                        item.LeadStatus = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.LeadStatus]);
                        item.Status = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Status]);
                        item.TicketId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                        lstResponse.Add(item);
                    }
                }
                return Ok(lstResponse);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetLeads: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetOpportunities")]
        public async Task<IHttpActionResult> GetOpportunities()
        {
            await Task.FromResult(0);
            try
            {
                UGITModule moduleData = _moduleManager.LoadByName(ModuleNames.OPM, true);
                List<OpportunityResponse> lstResponse = new List<OpportunityResponse>();
                DataTable dtResult = _ticketManager.GetOpenTickets(moduleData);
                DataRow[] drCollection = dtResult.Select();  //dtResult.Select($"{DatabaseObjects.Columns.ExternalID} IS NOT NULL AND {DatabaseObjects.Columns.ExternalID} <> ''");
                if (drCollection != null && drCollection.Length > 0)
                {
                    string prefixcolumn = uHelper.getAltTicketIdField(_context, ModuleNames.OPM);
                    foreach (DataRow row in drCollection)
                    {
                        OpportunityResponse item = new OpportunityResponse();
                        item.ID = UGITUtility.StringToLong(row[DatabaseObjects.Columns.ID]);
                        if (UGITUtility.IfColumnExists(row, prefixcolumn))
                            item.prefixTitle = Convert.ToString(row[prefixcolumn]);
                        item.Title = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                        item.Status = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Status]);
                        item.ERPJobIDNC = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ERPJobIDNC]);
                        item.TicketId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                        item.OpportunityTargetChoice = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.OpportunityTargetChoice]);
                        item.SummaryIcon = $"<div><a title='{item.Title}' width='30px' height='30px' onclick='event.stopPropagation(); javascript:openProjectSummaryPage(this)' TicketId='{item.TicketId}' ticketTitle='{item.Title}'>{uHelper.GenerateSummaryIcon(_context, row, item.TicketId)}</a></div>";
                        lstResponse.Add(item);
                    }
                }
                return Ok(lstResponse.OrderBy(x => x.Title));
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetOpportunities: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetTemplateAllocations")]
        public async Task<IHttpActionResult> GetTemplateAllocations()
        {
                await Task.FromResult(0);
            try
            {
                List<RMMResponse> objResponse = new List<RMMResponse>();
                objResponse.Add(new RMMResponse() { Role = "Estimator", PreconPct = 10, ConstPct = 20, CloseoutPct = 0 });
                objResponse.Add(new RMMResponse() { Role = "Accountant", PreconPct = 0, ConstPct = 80, CloseoutPct = 0 });
                objResponse.Add(new RMMResponse() { Role = "Project Manager", PreconPct = 10, ConstPct = 60, CloseoutPct = 0 });
                objResponse.Add(new RMMResponse() { Role = "Director", PreconPct = 10, ConstPct = 40, CloseoutPct = 0 });
                objResponse.Add(new RMMResponse() { Role = "Program Manager", PreconPct = 0, ConstPct = 50, CloseoutPct = 10 });
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTemplateAllocations: " + ex);
                return InternalServerError();
            }
        }
    }

    public class RMMResponse
    {
        public string Role { get; set; }
        public double PreconPct { get; set; }

        public double ConstPct { get; set; }
        public double CloseoutPct { get; set; }
    }

    public class LeadResponse
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string LeadStatus { get; set; }
        public string Status { get; set; }
        public string TicketId { get; set; }
    }

    public class OpportunityResponse
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string TicketId { get; set; }
        public string Status { get; set; }
        public string ERPJobIDNC { get; set; }
        public string SummaryIcon { get; set; }
        public string OpportunityTargetChoice { get; set; }
        public string prefixTitle { get; set; }

    }

    public class PageURLResponse
    {
        public string ModuleURL { get; set; }
        public string TicketTitle { get; set; }
    }

    public class StudioRequest
    {
        public string ID { get; set; }
        public string DivisionLookup { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public string FieldDisplayName { get; set; }
    }
}
