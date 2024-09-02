using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.RMM;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Web.Models;
using System.Linq.Expressions;
using uGovernIT.Utility.Helpers;
using uGovernIT.DAL;
using System.Data.SqlClient;
using System.Globalization;
using uGovernIT.Manager.Managers;
using System.IO;
using uGovernIT.Web.ControlTemplates.RMM;
using DevExpress.CodeParser;
using DevExpress.XtraSpreadsheet.DocumentFormats.Xlsb;
using static uGovernIT.Web.ModuleResourceAddEdit;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.Utils.Extensions;
using uGovernIT.Web.ControlTemplates.Admin.ListForm;
using System.EnterpriseServices.Internal;
using DevExpress.ExpressApp;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/rmmapi")]
    public class RMMAPIController : ApiController
    {
        ApplicationContext _context;
        ResourceAllocationManager resourceAllocationMGR;
        ProjectEstimatedAllocationManager projectEstimatedAllocationMGR;
        ModuleViewManager _moduleManager;
        public RMMAPIController()
        {
            _context = HttpContext.Current.GetManagerContext();
            resourceAllocationMGR = new ResourceAllocationManager(_context);
            projectEstimatedAllocationMGR = new ProjectEstimatedAllocationManager(_context);
            _moduleManager = new ModuleViewManager(_context);
        }
        //protected override void Initialize(HttpControllerContext controllerContext)
        //{
        //    base.Initialize(controllerContext);
        //}
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("SaveTimeSheet")]
        public async Task<IHttpActionResult> SaveTimeSheet(ResourceTimeSheetAPIModel dataVar)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
            int messageCode = 0;
            string message = string.Empty;
            try
            {
                List<long> changedWorkItemsID = new List<long>();
                bool isValid = false;
                bool linkActualFromRMMActual = UGITUtility.StringToBoolean(configurationVariableManager.GetValue("LinkActualFromRMMActual"));
                string selectedUserID = dataVar.userID;
                UserProfileManager objUserManager = new UserProfileManager(context);
                //Get User info of selected user
                UserProfile userInfo = objUserManager.GetUserById(selectedUserID);
                //Checks selected user is current user, if he is then allow save time sheet
                if (dataVar.currentUserID == selectedUserID)
                {
                    isValid = true;
                }
                else if (objUserManager.IsUGITSuperAdmin(objUserManager.GetUserById(dataVar.currentUserID)) ||
                        (userInfo != null && !string.IsNullOrEmpty(userInfo.ManagerID) && userInfo.ManagerID == dataVar.currentUserID))
                {
                    //Checks whether logged in user is super admin or manager of selected user 
                    //if he then allow him to save time sheet of selected user
                    isValid = true;
                }

                //If isValid false then set message code to 0 and authorize message
                if (!isValid)
                {
                    messageCode = 0;
                    message = "You are not authorized to edit actual hours of selected person";
                }

                //Convert startdate to datetime object
                DateTime weekStartDate = DateTime.MinValue;
                DateTime.TryParse(dataVar.startDate, out weekStartDate);
                if (isValid && weekStartDate == DateTime.MinValue)
                {
                    messageCode = 0;
                    message = "Invalid week date";
                    isValid = false;
                }

                if (isValid)
                {
                    //Deserializes timesheetdata object the get list of workItemHours
                    List<WorkItemHours> weekWorkSheet = new List<WorkItemHours>();
                    System.Xml.Serialization.XmlSerializer xSerialize1 = new System.Xml.Serialization.XmlSerializer(weekWorkSheet.GetType());
                    System.IO.StringReader sReader = new System.IO.StringReader(dataVar.timeSheetData);
                    weekWorkSheet = (List<WorkItemHours>)xSerialize1.Deserialize(sReader);

                    #region new updated one
                    List<WorkItemHours> resourceLevelSheets = weekWorkSheet.Where(x => x.WorkItemID > 0).ToList();
                    List<WorkItemHours> taskLevelSheets = weekWorkSheet.Where(x => x.WorkItemID <= 0).ToList();

                    TicketHoursManager tHelper = new TicketHoursManager(context);

                    //Update resource level timesheet entries
                    if (resourceLevelSheets.Count > 0)
                    {
                        List<long> updatedWorkItemIDs = tHelper.CreateResourceTimesheetsEntries(context, selectedUserID, weekStartDate, resourceLevelSheets);
                        if (updatedWorkItemIDs != null && updatedWorkItemIDs.Count > 0)
                        {
                            changedWorkItemsID.AddRange(updatedWorkItemIDs);
                        }
                    }

                    //Update task level timesheet entry
                    //Here we will update horus in tickethours list and then update it into resource timesheet
                    if (taskLevelSheets.Count > 0)
                    {
                        List<long> updatedWorkItemIDs = tHelper.CreateTaskHoursEntries(context, selectedUserID, weekStartDate, taskLevelSheets);
                        if (updatedWorkItemIDs != null || updatedWorkItemIDs.Count > 0)
                        {
                            changedWorkItemsID.AddRange(updatedWorkItemIDs);
                        }
                    }

                    #endregion
                    
                    //Removes Current Web in AllowUnsafeUpdates mode                  
                    changedWorkItemsID = changedWorkItemsID.Distinct().ToList();
                    if (changedWorkItemsID.Count > 0)
                    {
                        string resourceID = selectedUserID;
                        string webUrl = HttpContext.Current.Request.Url.ToString();
                        //Start Thread to update rmm summary list for current sheet entries

                        ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateActualInRMMSummary(context, changedWorkItemsID, resourceID, weekStartDate); };
                        Thread sThread = new Thread(threadStartMethod);
                        sThread.IsBackground = true;
                        sThread.Start();
                    }
                    //Sets messagecode and message
                    messageCode = 1;
                    message = "Successfully Saved";
                }
            }
            catch (Exception ex)
            {
                messageCode = 0;
                message = "An Exception Occurred in SaveTimeSheet";
                ULog.WriteException(ex);
            }
            string content = "{\"messagecode\":" + messageCode + ", \"message\":\"" + message + "\"}";
           
            return Ok(content);
        }

        [HttpPost]
        [Route("SaveAllocationAsTemplateNew")]
        public async Task<IHttpActionResult> SaveAllocationAsTemplateNew(TemplateModel model)
        {
            await Task.FromResult(0);
            try
            {
                ProjectAllocationTemplateManager templateManager = new ProjectAllocationTemplateManager(_context);
                ProjectAllocationTemplate dbTemplate = templateManager.Load(x => x.Name == model.TemplateName).FirstOrDefault();
                if (dbTemplate != null)
                    return BadRequest("Template name already exist");
                ProjectAllocationTemplate newTemplate = new ProjectAllocationTemplate();
                newTemplate.Name = model.TemplateName;
                newTemplate.ModuleName = model.ModuleName;
                newTemplate.TicketID = model.TicketID;
                newTemplate.TicketStartDate = model.StartDate;
                newTemplate.TicketEndDate = model.EndDate;
                newTemplate.Duration = model.Duration * 7;
                newTemplate.Template = model.Templates;
                long id = templateManager.Insert(newTemplate);
                if (id > 0)
                    return Ok();
                else
                    return BadRequest("An Exception Occurred in SaveAllocationAsTemplateNew.");
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in SaveAllocationAsTemplateNew: " + ex);
            }
            return Ok("An Exception Occurred in SaveAllocationAsTemplateNew");

        }

        [HttpPost]
        [Route("SaveAllocationAsTemplate")]
        public async Task<IHttpActionResult> SaveAllocationAsTemplate(VMSaveAllocationAsTemplate template)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                long retId = template.ID;
                ProjectAllocationTemplateManager templateManager = new ProjectAllocationTemplateManager(context);
                //ProjectAllocationTemplate dbTemplate = templateManager.Get(x => x.Name == template.Name);
                ProjectAllocationTemplate dbTemplate = templateManager.Load(x => x.Name == template.Name).FirstOrDefault();
                if (dbTemplate != null && !template.SaveOnExiting)
                    return BadRequest("Template name already exist");

                ProjectEstimatedAllocationManager crmAllocationMgr = new ProjectEstimatedAllocationManager(context);
                List<ProjectEstimatedAllocation> crmAllocations = crmAllocationMgr.Load(x => x.TicketId == template.TicketID);
                if ((crmAllocations == null || crmAllocations.Count == 0) && string.IsNullOrWhiteSpace(template.Allocations))
                    return BadRequest("No allocation exists to be saved as a Template.");

                DataRow ticketRow = Ticket.GetCurrentTicket(context, template.ModuleName, template.TicketID);
                if (ticketRow == null && string.IsNullOrWhiteSpace(template.Allocations))
                    return BadRequest("No Project found of which template is saved.");

                if (crmAllocations != null && crmAllocations.Count > 0 && string.IsNullOrWhiteSpace(template.Allocations))
                {
                    crmAllocations = crmAllocationMgr.SplitProjectAllocation(template.TicketID);
                }

                if (template.SaveOnExiting)
                {
                    ProjectAllocationTemplate dbExistingTemplate = templateManager.Get(x => x.ID == template.ID);
                    dbExistingTemplate.Name = template.Name;

                    dbExistingTemplate.TicketStartDate = template.StartDate;
                    dbExistingTemplate.TicketEndDate = template.EndDate;
                    dbExistingTemplate.PreconStartDate = template.PreconStartDate;
                    dbExistingTemplate.PreconEndDate = template.PreconEndDate;
                    dbExistingTemplate.ConstStartDate = template.ConstStartDate;
                    dbExistingTemplate.ConstEndDate = template.ConstEndDate;
                    dbExistingTemplate.CloseOutStartDate = template.CloseOutStartDate;
                    dbExistingTemplate.CloseOutEndDate = template.CloseOutEndDate;
                    dbExistingTemplate.Duration = template.Duration * 7;
                    if (dbExistingTemplate.CloseOutStartDate == DateTime.MinValue && dbExistingTemplate.ConstEndDate != DateTime.MinValue)
                    {
                        dbExistingTemplate.CloseOutStartDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, dbExistingTemplate.ConstEndDate.Value));
                    }
                    if (dbExistingTemplate.CloseOutEndDate == DateTime.MinValue && dbExistingTemplate.CloseOutStartDate != DateTime.MinValue)
                    {
                        dbExistingTemplate.CloseOutEndDate = dbExistingTemplate.CloseOutStartDate.Value.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                    }
                    dbExistingTemplate.Template = !string.IsNullOrWhiteSpace(template.Allocations) ? template.Allocations : JsonConvert.SerializeObject(crmAllocations);
                    templateManager.Update(dbExistingTemplate);
                }
                else
                {
                    dbTemplate = new ProjectAllocationTemplate();
                    dbTemplate.Name = template.Name;
                    dbTemplate.ModuleName = template.ModuleName;
                    dbTemplate.TicketID = template.TicketID;
                    dbTemplate.TicketStartDate = template.StartDate;
                    dbTemplate.TicketEndDate = template.EndDate;
                    dbTemplate.PreconStartDate = template.PreconStartDate;
                    dbTemplate.PreconEndDate = template.PreconEndDate;
                    dbTemplate.ConstStartDate = template.ConstStartDate;
                    dbTemplate.ConstEndDate = template.ConstEndDate;
                    dbTemplate.CloseOutStartDate = template.CloseOutStartDate;
                    dbTemplate.CloseOutEndDate = template.CloseOutEndDate;
                    dbTemplate.Duration = template.Duration * 7;
                    if (dbTemplate.CloseOutStartDate == DateTime.MinValue && dbTemplate.ConstEndDate != DateTime.MinValue)
                    {
                        dbTemplate.CloseOutStartDate = UGITUtility.StringToDateTime(uHelper.GetNextWorkingDateAndTime(_context, dbTemplate.ConstEndDate.Value));
                    }
                    if (dbTemplate.CloseOutEndDate == DateTime.MinValue && dbTemplate.CloseOutStartDate != DateTime.MinValue)
                    {
                        dbTemplate.CloseOutEndDate = dbTemplate.CloseOutStartDate.Value.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                    }

                    dbTemplate.Template = !string.IsNullOrWhiteSpace(template.Allocations) ? template.Allocations : JsonConvert.SerializeObject(crmAllocations);
                    templateManager.Insert(dbTemplate);
                    retId = dbTemplate.ID;
                }

                dbTemplate.Template = !string.IsNullOrWhiteSpace(template.Allocations) ? template.Allocations : JsonConvert.SerializeObject(crmAllocations);
                templateManager.Insert(dbTemplate);
                retId = dbTemplate.ID;
                return Ok(retId);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in SaveAllocationAsTemplate: " + ex);
                return Ok("An Exception Occurred in SaveAllocationAsTemplate");
            }
        }

        [Route("GetTemplateAllocations")]
        public async Task<IHttpActionResult> GetTemplateAllocations(long id, string projectID, string StartDate, string EndDate)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ProjectEstimatedAllocationManager crmManager = new ProjectEstimatedAllocationManager(context);
                UserProfileManager userManager = new UserProfileManager(context);
                if (id > 0)
                {
                    ProjectAllocationTemplate templateAllocation = ProjectEstimatedAllocationManager.GetTemplateAllocations(context, id);
                    if (templateAllocation != null)
                    {
                        templateAllocation = crmManager.NormaliseTemplate(templateAllocation, projectID, Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(templateAllocation.Template, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTemplateAllocations: " + ex);
            }
            return Ok("An Exception Occurred in GetTemplateAllocations");
            
        }

        [Route("GetTemplateDetails")]
        public async Task<IHttpActionResult> GetTemplateDetails(long id)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                List<GlobalRole> roles = uHelper.GetGlobalRoles(context, false);
                if (id > 0)
                {
                    ProjectAllocationTemplate templateAllocation = ProjectEstimatedAllocationManager.GetTemplateAllocations(context, id);
                    if (templateAllocation != null)
                    {
                        var retValue = new { roles = roles, templateData = templateAllocation };
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(JsonConvert.SerializeObject(retValue), Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTemplateDetails: " + ex);
            }
            return Ok("An Exception Occurred in GetTemplateDetails");
        }

        [HttpPost]
        [Route("UpdateFromTemplateAllocation")]
        public async Task<IHttpActionResult> UpdateFromTemplateAllocation(AllocationTemplateRequestModel model)
        {
            await Task.FromResult(0);
            try
            {
                ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(_context);
                
                //fetch existing allocations
                List<ProjectEstimatedAllocation> projectAllocations = cRMProjectAllocationManager.Load(x => x.TicketId == model.Allocations.First().ProjectID && x.Deleted != true
                                                                            && x.AssignedTo != UGITUtility.ObjectToString(Guid.Empty));
                List<AllocationTemplateModel> validAllocations = model.Allocations.FindAll(x => !string.IsNullOrEmpty(x.AssignedTo));
                foreach (AllocationTemplateModel allocation in validAllocations)
                {
                    if (allocation.AllocationStartDate > allocation.AllocationEndDate)
                    {
                        return Ok("DateNotValid:" + allocation.ID);
                    }

                    var userallocations = validAllocations.Where(x => x.AssignedTo == allocation.AssignedTo && x.AssignedTo != UGITUtility.ObjectToString(Guid.Empty)
                    && x.ID != allocation.ID);
                    if (userallocations != null && userallocations.Count() > 0)
                    {
                        var duplicateallocations = userallocations.Where(x =>
                        (allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationStartDate
                        || allocation.AllocationStartDate <= x.AllocationEndDate && allocation.AllocationEndDate >= x.AllocationEndDate
                        || allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationEndDate
                        || allocation.AllocationStartDate >= x.AllocationStartDate && allocation.AllocationEndDate <= x.AllocationEndDate)
                        && allocation.Type == x.Type);

                        if (duplicateallocations != null && duplicateallocations.Count() > 0)
                        {
                            return Ok("OverlappingAllocation:" + allocation.ID);
                        }
                    }
                    // if DeleteExistingAllocations is false then check for the overlapping allocations with existing project allocations.
                    if (!model.DeleteExistingAllocations)
                    {
                        var existingAlloc = projectAllocations.Where(x => x.AssignedTo == allocation.AssignedTo);
                        if (existingAlloc?.Count() > 0)
                        {
                            var duplicateallocations = existingAlloc.Where(x =>
                            (allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationStartDate
                            || allocation.AllocationStartDate <= x.AllocationEndDate && allocation.AllocationEndDate >= x.AllocationEndDate
                            || allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationEndDate
                            || allocation.AllocationStartDate >= x.AllocationStartDate && allocation.AllocationEndDate <= x.AllocationEndDate)
                            && allocation.Type == x.Type);

                            if (duplicateallocations != null && duplicateallocations.Count() > 0)
                            {
                                return Ok("OverlappingAllocation:" + allocation.ID);
                            }
                        }
                    }
                }
                cRMProjectAllocationManager.ImportAllocation(_context, model.Allocations, model.ProjectStartDate, model.ProjectEndDate, model.DeleteExistingAllocations);

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateFromTemplateAllocation: " + ex);
            }
            return Ok("An Exception Occurred in UpdateFromTemplateAllocation");

        }

        [HttpGet]
        [Route("GetProjectTemplates")]
        public async Task<IHttpActionResult> GetProjectTemplates()
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ProjectAllocationTemplateManager projectAllocationTemplateMGR = new ProjectAllocationTemplateManager(context);
                var projectAllocationTemplates = projectAllocationTemplateMGR.Load().Select(x => new { x.Name, x.ID, x.TicketID });
                if (projectAllocationTemplates != null)
                {
                    string jsonTemplates = JsonConvert.SerializeObject(projectAllocationTemplates);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonTemplates, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetProjectTemplates: " + ex);
            }
            return Ok("An Exception Occurred in GetProjectTemplates");

        }


        [Route("ShowProjectTeam")]
        public async Task<IHttpActionResult> ShowProjectTeam(AllocationTemplateResourceModel request)
        {
            await Task.FromResult(0);
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
                UserProfileManager objUserManager = new UserProfileManager(context);
                UserProfile userInfo = null;
                ProjectAllocationTemplateManager projectAllocationTemplateMGR = new ProjectAllocationTemplateManager(context);
                var projectAllocationTemplate = projectAllocationTemplateMGR.Load(x => x.ID == Convert.ToInt64(request.TemplateID)).Select(x => x.Template).FirstOrDefault();
                if (projectAllocationTemplate != null)
                {
                    List<AllocationTemplateModel> lstTemplates = Newtonsoft.Json.JsonConvert.DeserializeObject(projectAllocationTemplate, typeof(List<AllocationTemplateModel>)) as List<AllocationTemplateModel>;
                    List<AllocationTemplateModel> Allocations = request.Allocations;
                    List<RResourceAllocation> lstUserAllocation = null;
                    List<RResourceAllocation> lstAllOverLappingAlloc = new List<RResourceAllocation>();

                    int noOfWorkingHoursPerDay = uHelper.GetWorkingHoursInADay(context);
                    int noOfWorkingDaysPerWeek = uHelper.GetWorkingDaysInWeeks(context, 1);
                    double searchPeriodDays = 1;

                    foreach (var item in Allocations)
                    {
                        var user = lstTemplates.Where(y => y.ID == item.ID).Select(y => y.AssignedTo).FirstOrDefault();
                        if (user != null)
                        {
                            userInfo = objUserManager.GetUserById(user);
                            item.AssignedTo = user;
                            item.AssignedToName = userInfo.Name;
                            if (userInfo.Enabled)
                                item.IsResourceDisabled = false;
                            else
                                item.IsResourceDisabled = true;

                            searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(context, item.AllocationStartDate.Value, item.AllocationEndDate.Value);

                            double? totalPercetAllocatedInRange = 0;
                            lstAllOverLappingAlloc = resourceAllocationManager.LoadOpenItems(item.AllocationStartDate.Value, item.AllocationEndDate.Value, user);

                            lstUserAllocation = lstAllOverLappingAlloc.Where(x => x.Resource == user).OrderBy(x => x.AllocationStartDate).ToList();
                            if (lstUserAllocation != null && lstUserAllocation.Count > 0)
                            {
                                List<DateTime> lstOfStartDate = null;
                                List<DateTime> lstOfEndDate = null;
                                double? totalAllocOverlapDays = 0;
                                foreach (RResourceAllocation ralloc in lstUserAllocation)
                                {
                                    //To get overlapping days (Max start date and Min end date)
                                    lstOfStartDate = new List<DateTime>() { item.AllocationStartDate.Value, ralloc.AllocationStartDate.Value };
                                    lstOfEndDate = new List<DateTime>() { item.AllocationEndDate.Value, ralloc.AllocationEndDate.Value };
                                    DateTime maxStartDate = lstOfStartDate.Max();
                                    DateTime minEndDate = lstOfEndDate.Min();
                                    //Overlap days based on max start date and min end date
                                    double workingDays = uHelper.GetTotalWorkingDaysBetween(context, maxStartDate, minEndDate);
                                    double? pctAlloc = 0;
                                    if (ralloc.PctAllocation.HasValue)
                                        pctAlloc = ralloc.PctAllocation / 100;
                                    //Get allocation overlap percentage
                                    //double? allocOverlapPct = (workingDays / searchPeriodDays) * pctAlloc;
                                    // If more than one allocation user has 
                                    totalAllocOverlapDays += workingDays * pctAlloc;

                                }

                                //allocation in given date range 
                                totalPercetAllocatedInRange = (totalAllocOverlapDays / searchPeriodDays) * 100;
                            }

                            if (Math.Round(totalPercetAllocatedInRange.Value, 1) >= 100)
                                item.IsResourceOverAllocated = true;
                        }
                        else
                        {
                            item.AssignedTo = Guid.Empty.ToString();
                            item.AssignedToName = string.Empty;
                            item.IsResourceDisabled = false;
                        }
                    }

                    string jsonTemplates = JsonConvert.SerializeObject(Allocations);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonTemplates, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ShowProjectTeam: " + ex);
            }
            return Ok("An Exception Occurred in ShowProjectTeam");


        }

        [HttpGet]
        [Route("FindResourceBasedOnGroup")]
        public async Task<IHttpActionResult> FindResourceBasedOnGroup([FromUri]FindResourceRequest requestModel)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectEstimatedAllocationManager crmProjectAllocManager = new ProjectEstimatedAllocationManager(context);
            try
            {
                List<FindResourceResponse> allocations = crmProjectAllocManager.FindResourceBasedOnGroupNew(context, requestModel);
                string jsonAllocation = JsonConvert.SerializeObject(allocations);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonAllocation, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }
        }

        [HttpPost]
        [Route("FindTeams")]
        public async Task<IHttpActionResult> FindTeams([FromBody] FindTeamsRequest request)
        {
            await Task.FromResult(0);
            try
            {
                List<FindTeamsResponse> allocations = projectEstimatedAllocationMGR.FindTeams(_context, request);
                string jsonAllocation = JsonConvert.SerializeObject(allocations);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonAllocation, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }
        }

        [HttpPost]
        [Route("FindResourceBasedOnGroupNew")]
        public async Task<IHttpActionResult> FindResourceBasedOnGroupNew([FromBody] FindResourceRequest requestModel)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectEstimatedAllocationManager crmProjectAllocManager = new ProjectEstimatedAllocationManager(context);
            try
            {
                List<FindResourceResponse> allocations = crmProjectAllocManager.FindResourceBasedOnGroupNew(context, requestModel);
                string jsonAllocation = JsonConvert.SerializeObject(allocations);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonAllocation, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }
        }

        [Route("SelectMostAvailableResource")]
        public async Task<IHttpActionResult> SelectMostAvailableResource(FindResourceRequestModel requestModel)
        {
            await Task.FromResult(0);

            if (requestModel == null)
                return Ok();

            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectEstimatedAllocationManager crmProjectAllocManager = new ProjectEstimatedAllocationManager(context);
            //Thread.Sleep(5000);

            List<MostAvailableResourceResponse> allocations = crmProjectAllocManager.SelectMostAvailableResource(context, requestModel.TemplateAllocations);
            string jsonAllocation = JsonConvert.SerializeObject(allocations);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonAllocation, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpPost]
        [Route("DeleteTemplateAllocation")]
        public async Task<IHttpActionResult> DeleteTemplateAllocation(DeleteTemplateAllocationRequestModel requestModel)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectAllocationTemplateManager projectAllocationTemplateMGR = new ProjectAllocationTemplateManager(context);
            ProjectEstimatedAllocationManager crmProjectAllocManager = new ProjectEstimatedAllocationManager(context);
            crmProjectAllocManager.DeleteAllocationTemplate(context, requestModel);

            List<ProjectAllocationTemplate> allAllocTemplates = projectAllocationTemplateMGR.Load();
            string jsonallAllocTemplates = JsonConvert.SerializeObject(allAllocTemplates);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonallAllocTemplates, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetGroupsOrResource")]
        public async Task<IHttpActionResult> GetGroupsOrResource()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            List<GlobalRole> roles = uHelper.GetGlobalRoles(context, false);
            if (roles != null)
            {

                string jsonAllocation = JsonConvert.SerializeObject(roles.OrderBy(x => x.Name));
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonAllocation, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetUserProfilesData")]
        public async Task<IHttpActionResult> GetUserProfilesData()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            string jsonAllocation = JsonConvert.SerializeObject(context.UserManager.GetUsersProfile());
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonAllocation, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("IsResourceBelongsToAdminGroup")]
        public async Task<IHttpActionResult> IsResourceBelongsToAdminGroup(string resourceID)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            bool result = false;
            List<Role> userRoles = context.UserManager.GetUserRoles(resourceID);
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            ConfigurationVariable adminGroup = configManager.LoadVaribale(ConfigConstants.AdminGroup);
            if (adminGroup != null)
            {
                string[] adminGroups = UGITUtility.SplitString(adminGroup.KeyValue, Constants.Separator5);
                foreach (string s in adminGroups)
                {
                    if (userRoles.Exists(x => x.Title == s))
                    {
                        result = true;
                        break;
                    }
                }
            }
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("[{IsAdminGroup: " + result + "}]", Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetProjectAllocations")]
        public async Task<IHttpActionResult> GetProjectAllocations(string projectID)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
            UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(context);
            string moduleName = uHelper.getModuleNameByTicketId(projectID);
            DataRow currentTicket = Ticket.GetCurrentTicket(context, moduleName, projectID);
            
            //GetGroupsOrResource
            List<GlobalRole> roles = uHelper.GetGlobalRoles(context, false);

            //GetUserProfilesData
            List<UserProfile> userProfiles = context.UserManager.GetUsersProfile();
            
            //fetch allocations
            List<ProjectEstimatedAllocation> projectAllocations = cRMProjectAllocationManager.Load(x => x.TicketId == projectID && x.Deleted != true);

            //fetch user project experience tags
            List<UserProjectExperience> projectExperienceTags = userProjectExperienceMGR.Load(x => x.ProjectID == projectID);

            List<AllocationTemplateModel> allocations = new List<AllocationTemplateModel>();
            foreach (ProjectEstimatedAllocation alloc in projectAllocations)
            {
                AllocationTemplateModel model = new AllocationTemplateModel();
                var user = userProfiles.FirstOrDefault(o => o.Id == alloc.AssignedTo);
                model.AllocationEndDate = alloc.AllocationEndDate;
                model.AllocationStartDate = alloc.AllocationStartDate;
                model.TotalWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, alloc.AllocationStartDate.Value, alloc.AllocationEndDate.Value);
                model.AssignedTo = alloc.AssignedTo;
                model.IsLocked = alloc.IsLocked;
                model.UserImageUrl = user?.Picture ?? string.Empty;
                model.AssignedToName = user?.Name ?? string.Empty;
                model.IsResourceDisabled = !user?.Enabled ?? false;
                model.PctAllocation = Math.Ceiling(alloc.PctAllocation);
                model.ID = alloc.ID;
                model.Type = alloc.Type;
                model.TypeName = roles.FirstOrDefault(o => o.Id == alloc.Type)?.Name ?? string.Empty;
                model.Title = alloc.Title;
                model.SoftAllocation = alloc.SoftAllocation;
                model.NonChargeable = alloc.NonChargeable;
                model.ProjectID = alloc.TicketId;
                if(currentTicket != null)
                {
                    if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconStartDate) &&
                        UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconEndDate))
                    {
                        DateTime preconStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconStartDate]);
                        DateTime preconEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconEndDate]);
                        if (preconEnd != DateTime.MinValue && preconStart != DateTime.MinValue)
                        {
                            if (alloc.AllocationStartDate <= preconEnd && alloc.AllocationEndDate >= preconStart)
                            {
                                model.IsInPreconStage = true;
                            }
                        }
                    }
                    if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionStart) &&
                        UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionEnd))
                    {
                        DateTime constStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        DateTime constEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        if (constStart != DateTime.MinValue && constEnd != DateTime.MinValue)
                        {
                            if (alloc.AllocationStartDate <= constEnd && alloc.AllocationEndDate >= constStart)
                            {
                                model.IsInConstStage = true;
                            }
                        }
                    }
                    if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutStartDate) &&
                        UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutDate))
                    {
                        DateTime closesout = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutDate]);
                        DateTime closeoutStartEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]);
                        if (closesout == DateTime.MinValue && closeoutStartEnd != DateTime.MinValue)
                        {
                            closesout = closeoutStartEnd.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                        }
                        if (closesout != DateTime.MinValue && closeoutStartEnd != DateTime.MinValue)
                        {
                            //closeoutStartEnd = closeoutStartEnd.AddDays(1);
                            if (alloc.AllocationStartDate <= closesout && alloc.AllocationEndDate >= closeoutStartEnd)
                            {
                                model.IsInCloseoutStage = true;
                            }
                        }
                    }
                }

                model.UserImageUrl = user?.Picture;
                if (projectExperienceTags != null && projectExperienceTags.Count > 0)
                {
                    List<UserProjectExperience> userProjectExperience = projectExperienceTags.Where(x => x.UserId == alloc.AssignedTo).ToList();
                    model.Tags = userProjectExperience?.Count > 0 
                        ? userProjectExperience.Select(x => x.TagLookup.ToString()).Aggregate((x, y) => x + "," + y)
                        :string.Empty;
                }
                
                //On Uncomment below code make sure SP executes only once
                //Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("@TenantID", alloc.TenantID);
                //values.Add("@UserId", alloc.AssignedTo);
                //values.Add("@ProjectID", alloc.TicketId);

                //DataTable dt = uGITDAL.ExecuteDataSetWithParameters("usp_getUserExperienceTag", values);

                //if (dt.Rows.Count > 0)
                //{
                //    string Tag = Convert.ToString(dt.Rows[0]["TagLookup"]);
                //    if (!string.IsNullOrEmpty(Tag))
                //    {
                //        model.Tags = Tag;
                //    }
                //}

                allocations.Add(model);
            }

            
            TeamTabDataModel responseModel = new TeamTabDataModel()
            {
                Allocations = allocations,
                Roles = roles.OrderBy(x => x.Name).ToList(),
                UserProfiles = userProfiles
            };

            string jsonProjectAllocations = JsonConvert.SerializeObject(responseModel);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
            return ResponseMessage(response);

        }

        [HttpGet]
        [Route("GetNormaliseProjectAllocations")]
        public async Task<IHttpActionResult> GetNormaliseProjectAllocations(string baseProjectID, string projectID)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
            string moduleName = uHelper.getModuleNameByTicketId(baseProjectID);

            UserProfileManager userProfileManager = new UserProfileManager(context);
            List<UserProfile> userProfiles = userProfileManager.GetUsersProfile();

            //GetGroupsOrResource
            List<GlobalRole> roles = uHelper.GetGlobalRoles(context, false);

            //fetch allocations
            List<ProjectEstimatedAllocation> projectAllocations = cRMProjectAllocationManager.NormaliseProjectAllocation(baseProjectID, projectID);

            List<AllocationTemplateModel> allocations = new List<AllocationTemplateModel>();
            foreach (ProjectEstimatedAllocation alloc in projectAllocations)
            {
                AllocationTemplateModel model = new AllocationTemplateModel();
                model.AllocationEndDate = alloc.AllocationEndDate;
                model.AllocationStartDate = alloc.AllocationStartDate;
                model.AssignedTo = alloc.AssignedTo;
                model.AssignedToName = userProfiles.Find(x => x.Id == alloc.AssignedTo)?.Name ?? string.Empty;
                model.IsLocked = alloc.IsLocked;
                model.PctAllocation = Math.Ceiling(alloc.PctAllocation);
                model.ID = alloc.ID;
                model.Type = alloc.Type;
                model.TypeName = roles.FirstOrDefault(o => o.Id == alloc.Type)?.Name ?? string.Empty;
                model.Title = alloc.Title;
                model.SoftAllocation = alloc.SoftAllocation;
                model.NonChargeable = alloc.NonChargeable;
                model.ProjectID = alloc.TicketId;
                allocations.Add(model);
            }


            TeamTabDataModel responseModel = new TeamTabDataModel()
            {
                Allocations = allocations,
                Roles = roles.OrderBy(x => x.Name).ToList(),
            };

            string jsonProjectAllocations = JsonConvert.SerializeObject(responseModel);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
            return ResponseMessage(response);

        }

        [HttpGet]
        [Route("GetUserProjectDetails")]
        public async Task<IHttpActionResult> GetUserProjectDetails(string projectType)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
            List<ProjectAllocationDetail> projectAllocationDetails = new List<ProjectAllocationDetail>();
            //GetGroupsOrResource
            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable dt = new DataTable();
            values.Add("@TenantId", context.TenantID);
            values.Add("@UserId", context.CurrentUser.Id);
            values.Add("@ProjectType", projectType);
            values.Add("@IsManager", context.CurrentUser.IsManager);
            DataTable userTicketData = GetTableDataManager.GetData("UserProjectDetails", values);
            if (userTicketData != null)
            {
                foreach (DataRow dr in userTicketData.Rows)
                {
                    //GetUserProfilesData
                    List<UserProfile> userProfiles = context.UserManager.GetUsersProfile();
                    List<ProjectEstimatedAllocation> projectAllocations = cRMProjectAllocationManager.Load(x => x.TicketId == dr[DatabaseObjects.Columns.TicketId].ToString() && x.Deleted != true);
                    //fetch allocations
                    List<AllocationTemplateModel> allocations = new List<AllocationTemplateModel>();
                    ProjectAllocationDetail projectAllocationDetail = new ProjectAllocationDetail();
                    DateTime preconStart, preconEnd, constStart, constEnd, closeoutStart, closeoutEnd;
                    preconStart = preconEnd = constStart = constEnd = closeoutStart = closeoutEnd = DateTime.MinValue;
                    string title = UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.Title]);
                    projectAllocationDetail.ProjectTitle = UGITUtility.TruncateWithEllipsis(title,75) + ";" 
                        + title;
                    projectAllocationDetail.ProjectID = dr[DatabaseObjects.Columns.TicketId].ToString();

                    UGITModule moduleObj = _moduleManager.LoadByName(uHelper.getModuleNameByTicketId(projectAllocationDetail.ProjectID));
                    projectAllocationDetail.TitleLink = $"<a href='{uHelper.GetHyperLinkControlForTicketID(moduleObj, projectAllocationDetail.ProjectID, true, title).NavigateUrl}' style='color:black;font-weight:800px !important;'>{title}</a>";

                    if (UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.PreconStartDate) &&
                        UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.PreconEndDate))
                    {
                        preconStart = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.PreconStartDate]);
                        preconEnd = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.PreconEndDate]);
                        projectAllocationDetail.PreconStartDate = preconStart != DateTime.MinValue 
                            ? UGITUtility.GetDateStringInFormat(Convert.ToString(preconStart), false) 
                            : "<span class='rcorners2'>12, feb 2022</span>";
                        projectAllocationDetail.PreconEndDate = preconEnd != DateTime.MinValue 
                            ? UGITUtility.GetDateStringInFormat(Convert.ToString(preconEnd), false)
                            : "<span class='rcorners2'>12, feb 2022</span>";
                    }
                    if (UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.EstimatedConstructionStart) &&
                        UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.EstimatedConstructionEnd))
                    {
                        constStart = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.EstimatedConstructionStart]);
                        constEnd = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                        projectAllocationDetail.ConstStartDate = constStart != DateTime.MinValue 
                            ? UGITUtility.GetDateStringInFormat(Convert.ToString(constStart), false)
                            : "<span class='rcorners2'>12, feb 2022</span>";
                        projectAllocationDetail.ConstEndDate = constEnd != DateTime.MinValue 
                            ? UGITUtility.GetDateStringInFormat(Convert.ToString(constEnd), false)
                            : "<span class='rcorners2'>12, feb 2022</span>";
                    }
                    if (UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.CloseoutStartDate) &&
                        UGITUtility.IfColumnExists(dr, DatabaseObjects.Columns.CloseoutDate))
                    {
                        closeoutStart = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.CloseoutStartDate]);
                        closeoutEnd = UGITUtility.StringToDateTime(dr[DatabaseObjects.Columns.CloseoutDate]);
                        if (closeoutEnd == DateTime.MinValue && closeoutStart != DateTime.MinValue)
                        {
                            closeoutEnd = closeoutStart.AddWorkingDays(uHelper.getCloseoutperiod(_context));
                        }
                        projectAllocationDetail.CloseOutStartDate = closeoutStart != DateTime.MinValue 
                            ? UGITUtility.GetDateStringInFormat(Convert.ToString(closeoutStart), false)
                            : "</span><span class='rcorners2'>12, feb 2022</span>";
                        projectAllocationDetail.CloseOutEndDate = closeoutEnd == DateTime.MinValue && closeoutStart != DateTime.MinValue
                            ? UGITUtility.GetDateStringInFormat(Convert.ToString(closeoutStart.AddWorkingDays(uHelper.getCloseoutperiod(_context))), false)
                            : closeoutEnd == DateTime.MinValue 
                            ? "</span><span class='rcorners2'>12, feb 2022</span>"
                            : UGITUtility.GetDateStringInFormat(Convert.ToString(closeoutEnd), false);
                    }
                    if (projectAllocations == null)
                    {
                        continue;
                    }
                    int filledAllocationCount = 0;
                    foreach (ProjectEstimatedAllocation alloc in projectAllocations)
                    {
                        
                        AllocationTemplateModel model = new AllocationTemplateModel();
                        model.AllocationEndDate = alloc.AllocationEndDate;
                        model.AllocationStartDate = alloc.AllocationStartDate;
                        model.AssignedTo = alloc.AssignedTo;
                        UserProfile user = context.UserManager.GetUserInfoById(alloc.AssignedTo);
                        if (user != null)
                        {
                            model.AssignedToName = user.Name;
                            filledAllocationCount++;
                        }
                        model.PctAllocation = alloc.PctAllocation;
                        model.ID = alloc.ID;
                        model.Type = alloc.Type;
                        GlobalRole typeGroup = roleManager.Get(x => x.Id == alloc.Type);
                        if (typeGroup != null)
                            model.TypeName = typeGroup.Name;
                        model.Title = alloc.Title;
                        model.SoftAllocation = alloc.SoftAllocation;
                        if (preconEnd != DateTime.MinValue && preconStart != DateTime.MinValue)
                        {
                            if (alloc.AllocationStartDate <= preconEnd && alloc.AllocationEndDate >= preconStart)
                            {
                                model.IsInPreconStage = true;
                            }
                        }

                        if (constStart != DateTime.MinValue && constEnd != DateTime.MinValue)
                        {
                            if (alloc.AllocationStartDate <= constEnd && alloc.AllocationEndDate >= constStart)
                            {
                                model.IsInConstStage = true;
                            }
                        }

                        if (closeoutEnd != DateTime.MinValue && closeoutStart != DateTime.MinValue)
                        {
                            if (alloc.AllocationStartDate <= closeoutEnd && alloc.AllocationEndDate >= closeoutStart)
                            {
                                model.IsInCloseoutStage = true;
                            }
                        }

                        if (userProfiles != null && userProfiles.Count > 0)
                        {
                            model.UserImageUrl = userProfiles.FirstOrDefault(x => x.Id == alloc.AssignedTo)?.Picture;
                            if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(model.UserImageUrl)))
                            {
                                model.UserImageUrl = "/Content/Images/RMONE/blankImg.png";
                            }
                        }
                        allocations.Add(model);
                    }
                    projectAllocationDetail.TotalAllocationCount = allocations.Count.ToString();
                    projectAllocationDetail.FilledAllocationCount = allocations.Where(o => !string.IsNullOrWhiteSpace(o.AssignedToName)).Count().ToString();
                    projectAllocationDetail.Allocations = allocations;
                    projectAllocationDetails.Add(projectAllocationDetail);
                }
            }
            string jsonProjectAllocations = JsonConvert.SerializeObject(projectAllocationDetails);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
            return ResponseMessage(response);

        }
        [HttpGet]
        [Route("GetCertificationsList")]
        public async Task<IHttpActionResult> GetCertificationsList()
        {
            try
            {
                await Task.FromResult(0);
                List<UserCertificates> certificates = new List<UserCertificates>();
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                UserCertificateManager userCertificateManager = new UserCertificateManager(context);
                certificates = userCertificateManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == context.TenantID);
                if (certificates != null && certificates.Count > 0)
                    certificates = certificates.OrderBy(x => x.Title).ToList();
                string jsonCertificates = JsonConvert.SerializeObject(certificates);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonCertificates, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }
        }

                [HttpGet]
        [Route("GetExperiencedTagList")]
        public async Task<IHttpActionResult> GetProjectTagList(string tagMultiLookup)
        {
            try
            {
                List<ExperiencedTag> experiencedTags = new List<ExperiencedTag>();
                await Task.FromResult(0);
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                ExperiencedTagManager experiencedTagManager = new ExperiencedTagManager(context);
                if (!string.IsNullOrEmpty(tagMultiLookup))
                {
                    string check = "All";
                    if (tagMultiLookup.Equals(check))
                    {
                        experiencedTags = experiencedTagManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == context.TenantID);
                    }
                    else
                    {
                        experiencedTags = experiencedTagManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == context.TenantID && tagMultiLookup.Contains(Convert.ToString(x.ID)));
                    }
                }
                else
                {
                    //experiencedTags = experiencedTagManager.Load(x => !string.IsNullOrEmpty(x.Title) && !x.Deleted && x.TenantID == context.TenantID);
                    experiencedTags = new List<ExperiencedTag>();
                }
                if (experiencedTags != null && experiencedTags.Count > 0)
                    experiencedTags = experiencedTags.OrderBy(x => x.Title).ToList();
                string jsonUsers = JsonConvert.SerializeObject(experiencedTags);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonUsers, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }


        }

        [HttpGet]
        [Route("GetUserExperienceTagList")]
        public async Task<IHttpActionResult> GetUserExperienceTagList(string userId)
        {
            await Task.FromResult(0);
            try
            {
                UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(_context);
                UserProfileManager userProfileMGR = new UserProfileManager(_context);
                List<UserProjectExperience> userTags = userProjectExperienceMGR.Load(x => x.UserId == userId);
                
                // Tag assigned to project.
                List<UserProjectExperience> userProjectExperiences = userTags.Where(x => !string.IsNullOrWhiteSpace(x.ProjectID)).ToList();
                
                // Tag assigned to user profile.
                List<UserProjectExperience> userExperiencesTags = userTags.Where(x => string.IsNullOrWhiteSpace(x.ProjectID)).ToList();

                List<string> certificateData = null;
                var groupUserTagData = userProjectExperiences.GroupBy(x => new
                {
                    x.TagLookup
                }).Select(group => new
                {
                    group.Key.TagLookup,
                    TagCount = group.Count(),
                });

                UserProfile userProfile = userProfileMGR.LoadById(userId);
                if (userProfile != null && !string.IsNullOrWhiteSpace(userProfile.UserCertificateLookup))
                {
                    certificateData = userProfile.UserCertificateLookup.Split(',').ToList();
                }
                string jsonProjectTags = JsonConvert.SerializeObject(new { Certification = certificateData, UserProjectExperiencTags = groupUserTagData, UserExperiencesTags = userExperiencesTags });
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonProjectTags, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }
        }

        [HttpGet]
        [Route("GetProjectExperienceTagList")]
        public async Task<IHttpActionResult> GetProjectExperienceTagList(string projectId)
        {
            await Task.FromResult(0);
            try
            {
                UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(_context);
                string jsonProjectTags = JsonConvert.SerializeObject(userProjectExperienceMGR.GetProjectExperienceTags(projectId, true));
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonProjectTags, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }
        }

        [HttpPost]
        [Route("AddProjectExperienceTagList")]
        public async Task<IHttpActionResult> AddProjectExperienceTagList(ProjectExperienceModel model)
        {
            await Task.FromResult(0);
            
            ModuleViewManager _moduleViewManager = new ModuleViewManager(_context);
            TicketManager _ticketManager = new TicketManager(_context);
            
            List<string> tagLookup = model?.ProjectTags?.Where(x => x.Type == TagType.Experience)?.Select(x => x.TagId)?.ToList() ?? null;
            UserProjectExperienceManager userProjectExperienceManager = new UserProjectExperienceManager(_context);
            userProjectExperienceManager.UpdateUserProjectTagExperience(tagLookup, model.ProjectId);

            // Update to module table.
            string modulename = uHelper.getModuleNameByTicketId(model.ProjectId);
            UGITModule opmModuleObj = _moduleViewManager.GetByName(modulename);
            DataRow row = _ticketManager.GetByTicketID(opmModuleObj, model.ProjectId);
            if (UGITUtility.IfColumnExists(row, DatabaseObjects.Columns.TagMultiLookup))
            {
                row[DatabaseObjects.Columns.TagMultiLookup] = JsonConvert.SerializeObject(model.ProjectTags);
                Ticket TicketRequest = new Ticket(_context, modulename);
                TicketRequest.CommitChanges(row, "", donotUpdateEscalations: false);
            }
            
            return Ok();
        }

        [HttpPost]
        [Route("AddUserExperienceTagList")]
        public async Task<IHttpActionResult> AddUserExperienceTagList(string tagIds, string userId, TagType tagType)
        {
            await Task.FromResult(0);
            try
            {
                UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(_context);
                UserProfileManager userProfileMGR = new UserProfileManager(_context);
                List<UserProfile> userProfiles = userProfileMGR.GetUsersProfile();
                UserProfile userProfile = userProfiles.FirstOrDefault(o => o.Id == userId);
                if (tagType == TagType.Experience)
                {
                    List<string> tagLookup = !string.IsNullOrWhiteSpace(tagIds) ? tagIds.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList() : null;
                    userProjectExperienceMGR.UpdateUserProjectTagExperience(tagLookup, "", userId);
                }
                else
                {
                    userProfile.UserCertificateLookup = tagIds;
                    await userProfileMGR.UpdateAsync(userProfile);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return Ok();
            }
        }

        [HttpPost]
        [Route("UpdateBatchCRMAllocations")]
        public async Task<IHttpActionResult> UpdateBatchCRMAllocations(AllocationListModel model)
        {
            //If any changes are made in this method, check and make the relevant code changes in UpdateCRMAllocation method as well.
            try
            {
                await uHelper._semaphore.WaitAsync();
                {
                    ApplicationContext context = HttpContext.Current.GetManagerContext();
                    UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(context);
                    ModuleViewManager _moduleViewManager = new ModuleViewManager(context);
                    TicketManager _ticketManager = new TicketManager(context);
                    GlobalRoleManager roleManager = new GlobalRoleManager(context);
                    ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
                    UserProfileManager objUserProfileManager = new UserProfileManager(context);
                    
                    List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
                    List<string> historyDesc = new List<string>();
                    string projectID = string.Empty;
                    UserProfile resource = null;

                    if (model != null && model.Allocations != null)
                    {
                        model.Allocations.ForEach(
                           x =>
                           {
                               if (string.IsNullOrEmpty(x.AssignedTo))
                               {
                                   x.AssignedTo = Guid.Empty.ToString();
                                   x.AssignedToName = "Unassigned";
                               }
                           });
                        List<ProjectEstimatedAllocation> oldAllocations = CRMProjAllocManager.Load(x => x.TicketId == model.ProjectID);
                        List<string> oldAllocatedUsers = oldAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                        List<ProjectAllocationModel> validAllocations = model.Allocations.FindAll(x => !string.IsNullOrEmpty(x.AssignedTo));

                        if (oldAllocations != null && oldAllocations.Count > 0 && validAllocations != null && validAllocations.Count > 0)
                        {
                            if (!UGITUtility.StringToBoolean(model.OverrideAllocations))
                            {
                                List<ProjectEstimatedAllocation> newAllocationFromDb = oldAllocations.Where(o => !validAllocations.Select(x => x.ID).Contains(o.ID)).ToList();
                                if (newAllocationFromDb != null && newAllocationFromDb.Count > 0)
                                {
                                    newAllocationFromDb.ForEach(o =>
                                    {
                                        validAllocations.Add(new ProjectAllocationModel
                                        {
                                            AllocationStartDate = o.AllocationStartDate.Value,
                                            AllocationEndDate = o.AllocationEndDate.Value,
                                            AssignedTo = o.AssignedTo,
                                            PctAllocation = UGITUtility.StringToFloat(o.PctAllocation.ToString()),
                                            Type = o.Type,
                                            Title = o.Title,
                                            ProjectID = o.TicketId,
                                            SoftAllocation = o.SoftAllocation.ToString(),
                                            NonChargeable = o.NonChargeable.ToString(),
                                            IsLocked = o.IsLocked.ToString(),
                                            ID = o.ID
                                        });
                                    });
                                }
                            }
                            else
                            {
                                //delete existing allocations if override allocations is true
                                foreach(ProjectEstimatedAllocation spListItem in oldAllocations)
                                {
                                    long projEstmatedAllocId = spListItem.ID;
                                    ProjectEstimatedAllocationManager CRMProjAllocMgr = new ProjectEstimatedAllocationManager(_context);
                                    ResourceAllocationManager _resourceAllocationManager = new ResourceAllocationManager(_context);
                                    RResourceAllocation userRMMAllocation = _resourceAllocationManager.Get(x => x.ProjectEstimatedAllocationId == UGITUtility.ObjectToString(projEstmatedAllocId));
                                    CRMProjAllocManager.Delete(spListItem);
                                    string moduleName = uHelper.getModuleNameByTicketId(spListItem.TicketId);
                                    CRMProjAllocMgr.UpdateProjectGroups(moduleName, spListItem.TicketId);
                                    if (userRMMAllocation != null)
                                    {
                                        ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate ()
                                        {
                                            CRMProjAllocMgr.UpdateProjectGroups(moduleName, spListItem.TicketId);
                                            ResourceAllocationManager.UpdateHistory(_context, historyDesc, spListItem.TicketId);
                                            RMMSummaryHelper.CleanAllocation(_context, userRMMAllocation?.ResourceWorkItems, true);
                                            //historyDesc.ForEach(e => { ULog.WriteLog("PT >> " + _context.CurrentUser.Name + e); });
                                        };
                                        Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                                        sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                                        sThreadStartMethodUpdateCPRProjectAllocation.Start();
                                    }
                                }
                            }
                        }

                        if (validAllocations != null && validAllocations.Count <= 0)
                        {
                            return Ok("BlankAllocation");
                        }
                        List<string> newAllocatedUsers = validAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                        List<string> deAllocatedUsers = new List<string>();
                        foreach (ProjectAllocationModel allocation in validAllocations)
                        {
                            if (allocation.AllocationStartDate > allocation.AllocationEndDate)
                            {
                                return Ok("DateNotValid:" + allocation.ID);
                            }

                            var userallocations = validAllocations.Where(x => x.AssignedTo == allocation.AssignedTo && x.AssignedTo != UGITUtility.ObjectToString(Guid.Empty)
                            && x.ID != allocation.ID);
                            if (userallocations != null && userallocations.Count() > 0)
                            {
                                var duplicateallocations = userallocations.Where(x =>
                                (allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationStartDate
                                || allocation.AllocationStartDate <= x.AllocationEndDate && allocation.AllocationEndDate >= x.AllocationEndDate
                                || allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationEndDate
                                || allocation.AllocationStartDate >= x.AllocationStartDate && allocation.AllocationEndDate <= x.AllocationEndDate)
                                && allocation.Type == x.Type);

                                if (duplicateallocations != null && duplicateallocations.Count() > 0)
                                {
                                    return Ok("OverlappingAllocation:" + (!string.IsNullOrWhiteSpace(model.LastEditedRow)
                                        && duplicateallocations.Any(x => x.ID == UGITUtility.StringToLong(model.LastEditedRow))
                                        ? model.LastEditedRow : allocation.ID.ToString()));
                                }
                            }
                            resource = objUserProfileManager.LoadById(allocation.AssignedTo);
                            if (resource != null && (allocation.AllocationStartDate < resource.UGITStartDate || allocation.AllocationEndDate > resource.UGITEndDate))
                            {
                                return Ok(string.Format("AllocationOutofbounds~{0}~{1}~{2}~{3}", allocation.ID, resource.UGITStartDate.ToShortDateString(), resource.UGITEndDate.ToShortDateString(), resource.Name));
                            }
                        }

                        int? noOfAllocationChanges = null; 
                        bool? isUnfilledRoleCompleted = null;
                        bool isAnyUnfilledRoleFilled = false;
                        bool isAnyNewUnfilledRole= false;
                        List<string> recordToAddOrUpdate = new List<string>();
                        foreach (ProjectAllocationModel allocation in validAllocations)
                        {
                            ProjectEstimatedAllocation crmAllocation = new ProjectEstimatedAllocation();
                            crmAllocation.AllocationStartDate = allocation.AllocationStartDate;
                            crmAllocation.AllocationEndDate = allocation.AllocationEndDate;

                            int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, crmAllocation.AllocationStartDate.Value, crmAllocation.AllocationEndDate.Value);
                            int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                            crmAllocation.AssignedTo = allocation.AssignedTo;
                            crmAllocation.PctAllocation = allocation.PctAllocation;
                            crmAllocation.Type = allocation.Type;
                            crmAllocation.Duration = noOfWeeks;
                            crmAllocation.Title = allocation.Title;
                            crmAllocation.TicketId = model.ProjectID;
                            crmAllocation.SoftAllocation = UGITUtility.StringToBoolean(allocation.SoftAllocation);
                            crmAllocation.NonChargeable = UGITUtility.StringToBoolean(allocation.NonChargeable);
                            crmAllocation.IsLocked = UGITUtility.StringToBoolean(allocation.IsLocked);
                            crmAllocation.ID = allocation.ID;
                            if (crmAllocation.ID > 0)
                            {
                                var alloc = oldAllocations.FindAll(x => x.ID == allocation.ID).FirstOrDefault();
                                if (alloc != null)
                                {
                                    if (alloc.AssignedTo != allocation.AssignedTo || alloc.Type != allocation.Type || alloc.PctAllocation != allocation.PctAllocation || alloc.AllocationStartDate != allocation.AllocationStartDate || alloc.AllocationEndDate != allocation.AllocationEndDate 
                                        || alloc.SoftAllocation != UGITUtility.StringToBoolean(allocation.SoftAllocation) || alloc.NonChargeable != UGITUtility.StringToBoolean(allocation.NonChargeable) || alloc.IsLocked != UGITUtility.StringToBoolean(allocation.IsLocked))
                                    {
                                        ProjectEstimatedAllocation ifObjPersists = CRMProjAllocManager.LoadByID(crmAllocation.ID);
                                        if (ifObjPersists != null)
                                        {
                                            crmAllocation.Created = ifObjPersists.Created;
                                            crmAllocation.CreatedBy = ifObjPersists.CreatedBy;
                                            CRMProjAllocManager.Update(crmAllocation);
                                        }
                                        recordToAddOrUpdate.Add(UGITUtility.ObjectToString(crmAllocation.ID));
                                        string userName = context.UserManager.GetUserNameById(alloc.AssignedTo);
                                        string userRole = string.Empty;
                                        var role = roleManager.Get(x => x.Id == alloc.Type);
                                        if (role != null)
                                            userRole = role.Name;

                                        historyDesc.Add(string.Format("Updated allocation from user: {0} - {1} {2}% {3}-{4}  to  {5} - {6} {7}% {8}-{9}", userName, userRole, alloc.PctAllocation, String.Format("{0:MM/dd/yyyy}", alloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", alloc.AllocationEndDate),
                                                                                                                                    allocation.AssignedToName, allocation.TypeName, allocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", allocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", allocation.AllocationEndDate)));
                                    }

                                    if (alloc.AssignedTo != allocation.AssignedTo && alloc.AssignedTo != Guid.Empty.ToString())
                                    {
                                        noOfAllocationChanges = noOfAllocationChanges.HasValue ? noOfAllocationChanges.Value + 1 : 1;
                                    }
                                    if (alloc.AssignedTo != allocation.AssignedTo && alloc.AssignedTo == Guid.Empty.ToString())
                                    {
                                        isAnyUnfilledRoleFilled = true;
                                    }
                                }
                            }
                            else
                            {
                                crmAllocation.ID = 0;
                                CRMProjAllocManager.Insert(crmAllocation);
                                recordToAddOrUpdate.Add(UGITUtility.ObjectToString(crmAllocation.ID));
                                historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", allocation.AssignedToName, allocation.TypeName, allocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", allocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", allocation.AllocationEndDate)));

                                if (crmAllocation.AssignedTo == Guid.Empty.ToString())
                                {
                                    isAnyNewUnfilledRole = true;
                                }
                            }
                            projectID = allocation.ProjectID;
                            string roleName = string.Empty;

                            GlobalRole uRole = roleManager.Get(x => x.Id == allocation.Type);
                            if (uRole != null)
                                roleName = uRole.Name;

                            lstUserWithPercetage.Add(
                                new UserWithPercentage()
                                {
                                    EndDate = crmAllocation.AllocationEndDate ?? DateTime.MinValue,
                                    StartDate = crmAllocation.AllocationStartDate ?? DateTime.MinValue,
                                    Percentage = crmAllocation.PctAllocation,
                                    UserId = crmAllocation.AssignedTo,
                                    RoleTitle = roleName,
                                    ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID),
                                    RoleId = crmAllocation.Type,
                                    SoftAllocation = crmAllocation.SoftAllocation,
                                    NonChargeable = crmAllocation.NonChargeable,
                                });
                        }

                        if (!model.UpdateAllProjectAllocations) {
                            lstUserWithPercetage = lstUserWithPercetage.Where(x => recordToAddOrUpdate.Contains(x.ProjectEstiAllocId)).ToList();
                        }

                        if ((!isAnyNewUnfilledRole || isAnyUnfilledRoleFilled) && !validAllocations.Any(alloc => alloc.AssignedTo == Guid.Empty.ToString()))
                        {
                            isUnfilledRoleCompleted = true;
                        } 
                        else if(validAllocations.Any(alloc => alloc.AssignedTo == Guid.Empty.ToString()))
                        {
                            isUnfilledRoleCompleted = false;
                        }

                        if (model.IsAllocationSplitted)
                        {
                            ULog.WriteLog("Split Phases Button Clicked >> " + context.CurrentUser.Name);
                        }

                        ThreadStart threadUpdateUtilizationAndExperienceData = delegate ()
                        {
                            List<string> experienceTags = model?.Allocations[0]?.Tags?.Split(',')?.ToList() ?? null;
                            userProjectExperienceMGR.UpdateUserProjectTagExperience(experienceTags, model.ProjectID);
                        };
                        Thread sThread = new Thread(threadUpdateUtilizationAndExperienceData);
                        sThread.IsBackground = true;
                        sThread.Start();
                        
                        //to-do
                        //Code commented to close code review comment for BTS-23-001325 on 9 Jan 2024.
                        //This code finds out project tasks (PMM/Module tasks) which are assigned to the user and then does not allow allocations to be made for the user,
                        //if any tasks are assigned to the user.
                        //Since this method is currently being used only from Gantt, this code piece is not required as of now.
                        //var taskManager = new UGITTaskManager(context);
                        //List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(model.ProjectID), model.ProjectID);
                        //List<string> lstUsers = model.Allocations.Select(a => a.AssignedTo).ToList();
                        //var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                        // Only create allocation enties if user is not in schedule
                        newAllocatedUsers = newAllocatedUsers.Union(oldAllocatedUsers).ToList();
                        try
                        {
                            if (model.UseThreading)
                            {
                                ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate ()
                                {
                                    ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(model.ProjectID), model.ProjectID, lstUserWithPercetage, newAllocatedUsers);
                                    ResourceAllocationManager.UpdateHistory(context, historyDesc, model.ProjectID, noOfAllocationChanges, isUnfilledRoleCompleted);
                                    historyDesc.ForEach(o =>
                                    {
                                        ULog.WriteLog("PT >> " + context.CurrentUser.Name + o);
                                    });

                                };
                                Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                                sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                                sThreadStartMethodUpdateCPRProjectAllocation.Start();
                            }
                            else {
                                ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(model.ProjectID), model.ProjectID, lstUserWithPercetage, newAllocatedUsers);
                                ResourceAllocationManager.UpdateHistory(context, historyDesc, model.ProjectID, noOfAllocationChanges, isUnfilledRoleCompleted);
                                historyDesc.ForEach(o =>
                                {
                                    ULog.WriteLog("PT >> " + context.CurrentUser.Name + o);
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                        if (model.NeedReturnData)
                        {
                            #region ReturnData
                            List<ProjectEstimatedAllocation> projectAllocations = CRMProjAllocManager.Load(x => x.TicketId == model.ProjectID && x.Deleted != true);
                            List<AllocationTemplateModel> allocations = new List<AllocationTemplateModel>();
                            foreach (ProjectEstimatedAllocation alloc in projectAllocations)
                            {
                                AllocationTemplateModel resultObj = new AllocationTemplateModel();
                                resultObj.AllocationEndDate = alloc.AllocationEndDate;
                                resultObj.AllocationStartDate = alloc.AllocationStartDate;
                                resultObj.AssignedTo = alloc.AssignedTo;
                                UserProfile user = context.UserManager.GetUserInfoById(alloc.AssignedTo);
                                if (user != null)
                                    resultObj.AssignedToName = user.Name;
                                resultObj.UserImageUrl = resultObj.AssignedToName == "" ? string.Empty : user?.Picture ?? string.Empty;
                                resultObj.PctAllocation = alloc.PctAllocation;
                                resultObj.ID = alloc.ID;
                                resultObj.Type = alloc.Type;
                                resultObj.SoftAllocation = alloc.SoftAllocation;
                                resultObj.IsLocked = alloc.IsLocked;
                                resultObj.Tags = model.Allocations[0].Tags;
                                resultObj.NonChargeable = alloc.NonChargeable;
                                resultObj.TotalWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, alloc.AllocationStartDate.Value, alloc.AllocationEndDate.Value);
                                resultObj.IsResourceDisabled = !user?.Enabled ?? false;
                                GlobalRole typeGroup = roleManager.Get(x => x.Id == alloc.Type);
                                if (typeGroup != null)
                                    resultObj.TypeName = typeGroup.Name;
                                resultObj.Title = alloc.Title;
                                allocations.Add(resultObj);
                            }

                            Dictionary<string, DateTime> datesUpdates = new Dictionary<string, DateTime>();
                            if (!string.IsNullOrEmpty(model.PreConStart))
                                datesUpdates.Add(DatabaseObjects.Columns.PreconStartDate, DateTime.Parse(model.PreConStart));
                            if (!string.IsNullOrEmpty(model.PreConEnd))
                                datesUpdates.Add(DatabaseObjects.Columns.PreconEndDate, DateTime.Parse(model.PreConEnd));
                            if (!string.IsNullOrEmpty(model.ConstStart))
                                datesUpdates.Add(DatabaseObjects.Columns.EstimatedConstructionStart, DateTime.Parse(model.ConstStart));
                            if (!string.IsNullOrEmpty(model.ConstEnd))
                                datesUpdates.Add(DatabaseObjects.Columns.EstimatedConstructionEnd, DateTime.Parse(model.ConstEnd));
                            if (datesUpdates.Count > 0)
                            {
                                string modulename = uHelper.getModuleNameByTicketId(model.ProjectID);
                                UGITModule opmModuleObj = _moduleViewManager.GetByName(modulename);
                                DataRow row = _ticketManager.GetByTicketID(opmModuleObj, model.ProjectID);

                                foreach (KeyValuePair<string, DateTime> s in datesUpdates)
                                    row[s.Key] = s.Value;

                                Ticket TicketRequest = new Ticket(context, modulename);
                                TicketRequest.CommitChanges(row, "", donotUpdateEscalations: false);
                            }


                            string jsonProjectAllocations = JsonConvert.SerializeObject(allocations);
                            var response = this.Request.CreateResponse(HttpStatusCode.OK);
                            response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
                            return ResponseMessage(response);
                            #endregion
                        }
                        else {
                            return Ok();
                        }
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                ULog.WriteException("UpdateBatchCRMAllocations_semaphore Lock:" + e.ToString());
                return Ok("UpdateBatchCRMAllocations_semaphore Lock:" + e.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
        }


        [HttpPost]
        [Route("UpdateAllocationsAndPhaseDates")]
        public async Task<IHttpActionResult> UpdateAllocationsAndPhaseDates(AllocationDatesModel model)
        {
            try
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                int noOfDays = model.Duration;

                if (model.TimeFrame == TimeFrame.Weeks)
                {
                    noOfDays = uHelper.GetWorkingDaysInWeeks(context, model.Duration < 0 ? -model.Duration : model.Duration);
                    noOfDays = model.Duration < 0 ? -noOfDays : noOfDays;
                }
                else if (model.TimeFrame == TimeFrame.Months)
                {
                    noOfDays = uHelper.GetTotalWorkingDaysBetween(context, DateTime.Now, DateTime.Now.AddMonths(model.Duration < 0 ? -model.Duration : model.Duration));
                    noOfDays = model.Duration < 0 ? -noOfDays : noOfDays;
                }

                if (model != null && model.Allocations != null)
                {
                    foreach (var allocation in model.Allocations?.Where(x => !x.IsLocked))
                    {
                        if (model.UType == UpdateType.PastAndFuture || model.Duration < 0)
                        {
                            allocation.AllocationStartDate = uHelper.GetEndDateByWorkingDays(context, allocation.AllocationStartDate.Value, noOfDays)[1];
                            allocation.AllocationEndDate = uHelper.GetEndDateByWorkingDays(context, allocation.AllocationEndDate.Value, noOfDays)[1];
                        }
                        else
                        {
                            if (allocation.AllocationStartDate >= DateTime.Now)
                            {
                                allocation.AllocationStartDate = uHelper.GetEndDateByWorkingDays(context, allocation.AllocationStartDate.Value, noOfDays)[1];
                            }
                            if (allocation.AllocationEndDate >= DateTime.Now)
                            {
                                allocation.AllocationEndDate = uHelper.GetEndDateByWorkingDays(context, allocation.AllocationEndDate.Value, noOfDays)[1];
                            }
                        }
                    }
                    if (model.UpdatePhaseDates)
                    {
                        model.PreconStartDate = !string.IsNullOrWhiteSpace(model.PreconStartDate) 
                            ? model.UType == UpdateType.PastAndFuture || model.Duration < 0 || DateTime.ParseExact(model.PreconStartDate, "MM/dd/yyyy", CultureInfo.CurrentCulture) >= DateTime.Now
                                ? uHelper.GetEndDateByWorkingDays(context, DateTime.ParseExact(model.PreconStartDate, "MM/dd/yyyy", CultureInfo.CurrentCulture), noOfDays)[1].ToString("MM/dd/yyyy") 
                                : model.PreconStartDate
                            : "";
                        model.PreconEndDate = !string.IsNullOrWhiteSpace(model.PreconEndDate)
                            ? model.UType == UpdateType.PastAndFuture || model.Duration < 0 || DateTime.ParseExact(model.PreconEndDate, "MM/dd/yyyy", CultureInfo.CurrentCulture) >= DateTime.Now
                                ? uHelper.GetEndDateByWorkingDays(context, DateTime.ParseExact(model.PreconEndDate, "MM/dd/yyyy", CultureInfo.CurrentCulture), noOfDays)[1].ToString("MM/dd/yyyy")
                                : model.PreconEndDate
                            : "";
                        model.ConstStartDate = !string.IsNullOrWhiteSpace(model.ConstStartDate)
                            ? model.UType == UpdateType.PastAndFuture || model.Duration < 0 || DateTime.ParseExact(model.ConstStartDate, "MM/dd/yyyy", CultureInfo.CurrentCulture) >= DateTime.Now
                                ? uHelper.GetEndDateByWorkingDays(context, DateTime.ParseExact(model.ConstStartDate, "MM/dd/yyyy", CultureInfo.CurrentCulture), noOfDays)[1].ToString("MM/dd/yyyy")
                                : model.ConstStartDate
                            : "";
                        model.ConstEndDate = !string.IsNullOrWhiteSpace(model.ConstEndDate)
                            ? model.UType == UpdateType.PastAndFuture || model.Duration < 0 || DateTime.ParseExact(model.ConstEndDate, "MM/dd/yyyy", CultureInfo.CurrentCulture) >= DateTime.Now
                                ? uHelper.GetEndDateByWorkingDays(context, DateTime.ParseExact(model.ConstEndDate, "MM/dd/yyyy", CultureInfo.CurrentCulture), noOfDays)[1].ToString("MM/dd/yyyy")
                                : model.ConstEndDate
                            : "";
                        model.CloseOutStartDate = !string.IsNullOrWhiteSpace(model.CloseOutStartDate)
                            ? model.UType == UpdateType.PastAndFuture || model.Duration < 0 || DateTime.ParseExact(model.CloseOutStartDate, "MM/dd/yyyy", CultureInfo.CurrentCulture) >= DateTime.Now
                                ? uHelper.GetEndDateByWorkingDays(context, DateTime.ParseExact(model.CloseOutStartDate, "MM/dd/yyyy", CultureInfo.CurrentCulture), noOfDays)[1].ToString("MM/dd/yyyy")
                                : model.CloseOutStartDate
                            : "";
                        model.CloseOutEndDate = !string.IsNullOrWhiteSpace(model.CloseOutEndDate)
                            ? model.UType == UpdateType.PastAndFuture || model.Duration < 0 || DateTime.ParseExact(model.CloseOutEndDate, "MM/dd/yyyy", CultureInfo.CurrentCulture) >= DateTime.Now
                                ? uHelper.GetEndDateByWorkingDays(context, DateTime.ParseExact(model.CloseOutEndDate, "MM/dd/yyyy", CultureInfo.CurrentCulture), noOfDays)[1].ToString("MM/dd/yyyy")
                                : model.CloseOutEndDate
                            : "";
                    }
                }

                return Ok(model);
            }
            catch (Exception e)
            {
                ULog.WriteException("UpdateAllocationsAndPhaseDates: " + e.ToString());
                return Ok("UpdateAllocationsAndPhaseDates: " + e.ToString());
            }
        }
        [HttpPost]
        [Route("UpdateCRMAllocation")]
        public async Task<IHttpActionResult> UpdateCRMAllocation(AllocationListModel model)
        {
            await Task.FromResult(0);
            //If any changes are made in this method, check and make the relevant code changes in UpdateBatchCRMAllocations method as well.
            try
            {
                await uHelper._semaphore.WaitAsync();
                {
                    ApplicationContext context = HttpContext.Current.GetManagerContext();
                    ModuleViewManager _moduleViewManager = new ModuleViewManager(context);
                    GlobalRoleManager roleManager = new GlobalRoleManager(context);
                    ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
                    UserProfileManager objUserProfileManager = new UserProfileManager(context);

                    List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
                    List<string> historyDesc = new List<string>();
                    string projectID = string.Empty;
                    UserProfile resource = null;

                    if (model != null && model.Allocations != null)
                    {
                        model.Allocations.ForEach(
                           x =>
                           {
                               if (string.IsNullOrEmpty(x.AssignedTo))
                               {
                                   x.AssignedTo = Guid.Empty.ToString();
                                   x.AssignedToName = "Unassigned";
                               }
                           });
                        List<ProjectEstimatedAllocation> oldAllocations = CRMProjAllocManager.Load(x => x.TicketId == model.ProjectID);
                        List<string> oldAllocatedUsers = oldAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                        ProjectAllocationModel allocation = model.Allocations.FindAll(x => !string.IsNullOrEmpty(x.AssignedTo))[0];
                        if (allocation.AllocationStartDate < DateTime.Now && allocation.AllocationEndDate >= DateTime.Now && allocation.isChangeStartDate == true)
                        {
                            allocation.AllocationStartDate = DateTime.Now;
                        }
                        List<ProjectEstimatedAllocation> newAllocationFromDb = null;
                        if (allocation == null)
                        {
                            return Ok("BlankAllocation");
                        }

                        if (oldAllocations != null && oldAllocations.Count > 0 && allocation != null)
                        {
                            newAllocationFromDb = oldAllocations.Where(o => o.ID != allocation.ID).ToList();
                        }

                        List<string> newAllocatedUsers = new List<string>();
                        newAllocatedUsers.Add(allocation.AssignedTo);
                        List<string> deAllocatedUsers = new List<string>();
                            if (newAllocationFromDb != null)
                            {
                                var userallocations = newAllocationFromDb.Where(x => x.AssignedTo == allocation.AssignedTo && x.AssignedTo != UGITUtility.ObjectToString(Guid.Empty)
                                    && x.ID != allocation.ID);
                                if (userallocations != null && userallocations.Count() > 0)
                                {
                                    var duplicateallocations = userallocations.Where(x =>
                                    (allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationStartDate
                                    || allocation.AllocationStartDate <= x.AllocationEndDate && allocation.AllocationEndDate >= x.AllocationEndDate
                                    || allocation.AllocationStartDate <= x.AllocationStartDate && allocation.AllocationEndDate >= x.AllocationEndDate
                                    || allocation.AllocationStartDate >= x.AllocationStartDate && allocation.AllocationEndDate <= x.AllocationEndDate)
                                    && allocation.Type == x.Type);

                                    if (duplicateallocations != null && duplicateallocations.Count() > 0)
                                    {
                                        return Ok("OverlappingAllocation:" + (!string.IsNullOrWhiteSpace(model.LastEditedRow)
                                            && duplicateallocations.Any(x => x.ID == UGITUtility.StringToLong(model.LastEditedRow))
                                            ? model.LastEditedRow : allocation.ID.ToString()));
                                    }
                                }
                            }
                            resource = objUserProfileManager.LoadById(allocation.AssignedTo);
                            if (resource != null && (allocation.AllocationStartDate < resource.UGITStartDate || allocation.AllocationEndDate > resource.UGITEndDate))
                            {
                                return Ok(string.Format("AllocationOutofbounds~{0}~{1}~{2}~{3}", allocation.ID, resource.UGITStartDate.ToShortDateString(), resource.UGITEndDate.ToShortDateString(), resource.Name));
                            }
                        if (resource !=null && string.IsNullOrEmpty(allocation.AssignedToName))
                            allocation.AssignedToName = resource.Name;

                        ULog.WriteLog(string.Format("Gantt Edit Allocation >> Called From {5} > Current User: {0}, Allocation Details:- AssignedTo: {6}({1}), Start Date: {2}, End Date: {3}, %PCT: {4}", 
                            context.CurrentUser.Name, allocation.AssignedTo, allocation.AllocationStartDate, allocation.AllocationEndDate, allocation.PctAllocation, model.CalledFrom, allocation.AssignedToName));

                        ProjectEstimatedAllocation crmAllocation = new ProjectEstimatedAllocation();
                            crmAllocation.AllocationStartDate = allocation.AllocationStartDate;
                            crmAllocation.AllocationEndDate = allocation.AllocationEndDate;

                            int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, crmAllocation.AllocationStartDate.Value, crmAllocation.AllocationEndDate.Value);
                            int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);


                            crmAllocation.AssignedTo = allocation.AssignedTo;
                            crmAllocation.PctAllocation = allocation.PctAllocation;
                            crmAllocation.Type = allocation.Type;
                            crmAllocation.Duration = noOfWeeks;
                            crmAllocation.Title = allocation.Title;
                            crmAllocation.TicketId = model.ProjectID;
                            crmAllocation.SoftAllocation = UGITUtility.StringToBoolean(allocation.SoftAllocation);
                            crmAllocation.NonChargeable = UGITUtility.StringToBoolean(allocation.NonChargeable);
                            crmAllocation.IsLocked = UGITUtility.StringToBoolean(allocation.IsLocked);
                            crmAllocation.ID = allocation.ID;
                            if (crmAllocation.ID > 0)
                            {
                                ProjectEstimatedAllocation ifObjPersists = CRMProjAllocManager.LoadByID(crmAllocation.ID);
                                if (ifObjPersists != null)
                                {
                                    CRMProjAllocManager.Update(crmAllocation);
                                }

                                var alloc = oldAllocations.FindAll(x => x.ID == allocation.ID).FirstOrDefault();
                                if (alloc != null)
                                {
                                    if (alloc.AssignedTo != allocation.AssignedTo || alloc.Type != allocation.Type || alloc.PctAllocation != allocation.PctAllocation || alloc.AllocationStartDate != allocation.AllocationStartDate || alloc.AllocationEndDate != allocation.AllocationEndDate)
                                    {
                                        string userName = context.UserManager.GetUserNameById(alloc.AssignedTo);
                                        string userRole = string.Empty;
                                        var role = roleManager.Get(x => x.Id == alloc.Type);
                                        if (role != null)
                                            userRole = role.Name;

                                        historyDesc.Add(string.Format("Updated allocation for user: {0} - {1} {2}% {3}-{4}  to  {5} - {6} {7}% {8}-{9}", userName, userRole, alloc.PctAllocation, String.Format("{0:MM/dd/yyyy}", alloc.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", alloc.AllocationEndDate),
                                                                                                                                    allocation.AssignedToName, allocation.TypeName, allocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", allocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", allocation.AllocationEndDate)));
                                    }
                                }
                            }
                            else
                            {
                                crmAllocation.ID = 0;
                                CRMProjAllocManager.Insert(crmAllocation);

                                historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", allocation.AssignedToName, allocation.TypeName, allocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", allocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", allocation.AllocationEndDate)));
                            }
                            projectID = allocation.ProjectID;

                            lstUserWithPercetage.Add(
                                new UserWithPercentage()
                                {
                                    EndDate = crmAllocation.AllocationEndDate ?? DateTime.MinValue,
                                    StartDate = crmAllocation.AllocationStartDate ?? DateTime.MinValue,
                                    Percentage = crmAllocation.PctAllocation,
                                    UserId = crmAllocation.AssignedTo,
                                    RoleTitle = allocation.TypeName,
                                    ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID),
                                    RoleId = crmAllocation.Type,
                                    SoftAllocation = crmAllocation.SoftAllocation,
                                });


                        //to-do
                        //Code commented to close code review comment for BTS-23-001325 on 9 Jan 2024.
                        //This code finds out project tasks (PMM/Module tasks) which are assigned to the user and then does not allow allocations to be made for the user,
                        //if any tasks are assigned to the user.
                        //Since this method is currently being used only from Gantt, this code piece is not required as of now.
                        //var taskManager = new UGITTaskManager(context);
                        //List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(model.ProjectID), model.ProjectID);
                        //List<string> lstUsers = model.Allocations.Select(a => a.AssignedTo).ToList();
                        //var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                        //// Only create allocation enties if user is not in schedule
                        //newAllocatedUsers = newAllocatedUsers.Union(oldAllocatedUsers).ToList();
                        try
                        {
                            //if (res == null || res.Count == 0)
                            //{
                                ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate ()
                                {
                                    ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(model.ProjectID), model.ProjectID, lstUserWithPercetage, newAllocatedUsers, false);
                                    ResourceAllocationManager.UpdateHistory(context, historyDesc, model.ProjectID);
                                    historyDesc.ForEach(o =>
                                    {
                                        ULog.WriteLog("Gantt Edit Allocation >> " + model.CalledFrom  + ": " + context.CurrentUser.Name + " " + o);
                                    });

                                };
                                Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                                sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                                sThreadStartMethodUpdateCPRProjectAllocation.Start();
                            //}
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                        
                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                ULog.WriteException("UpdateCRMAllocation_semaphore Lock:" + e.ToString());
                return Ok("UpdateCRMAllocation_semaphore Lock:" + e.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
        }

        [HttpPost]
        [Route("UpdateBatchCRMAllocationsForTemplate")]
        public async Task<IHttpActionResult> UpdateBatchCRMAllocationsForTemplate(AllocationListModel model)
        {
            try
            {
                await uHelper._semaphore.WaitAsync();
                {
                    ApplicationContext context = HttpContext.Current.GetManagerContext();
                    UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(context);
                    ModuleViewManager _moduleViewManager = new ModuleViewManager(context);
                    TicketManager _ticketManager = new TicketManager(context);
                    GlobalRoleManager roleManager = new GlobalRoleManager(context);
                    ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
                    UserProfileManager objUserProfileManager = new UserProfileManager(context);
                    List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
                    List<string> historyDesc = new List<string>();
                    string projectID = model.ProjectID;

                    if (model != null && model.Allocations != null)
                    {
                        model.Allocations.ForEach(
                           x =>
                           {
                               x.AssignedTo = Guid.Empty.ToString();
                               x.AssignedToName = "Unassigned";
                           });
                        List<ProjectEstimatedAllocation> oldAllocations = CRMProjAllocManager.Load(x => x.TicketId == model.ProjectID);
                        List<string> oldAllocatedUsers = oldAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                        List<ProjectAllocationModel> validAllocations = model.Allocations.FindAll(x => !string.IsNullOrEmpty(x.AssignedTo));

                        if (oldAllocations != null && oldAllocations.Count > 0 && validAllocations != null && validAllocations.Count > 0)
                        {
                            RMMSummaryHelper.DeleteAllCRMAllocations(context, projectID);
                        }

                        if (validAllocations != null && validAllocations.Count <= 0)
                        {
                            return Ok("BlankAllocation");
                        }
                        List<string> newAllocatedUsers = validAllocations.Select(x => x.AssignedTo).Distinct().ToList();
                        foreach (ProjectAllocationModel allocation in validAllocations)
                        {
                            if (allocation.AllocationStartDate > allocation.AllocationEndDate)
                            {
                                return Ok("DateNotValid:" + allocation.ID);
                            }

                            ProjectEstimatedAllocation crmAllocation = new ProjectEstimatedAllocation();
                            crmAllocation.AllocationStartDate = allocation.AllocationStartDate;
                            crmAllocation.AllocationEndDate = allocation.AllocationEndDate;

                            int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, crmAllocation.AllocationStartDate.Value, crmAllocation.AllocationEndDate.Value);
                            int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                            crmAllocation.AssignedTo = allocation.AssignedTo;
                            crmAllocation.PctAllocation = allocation.PctAllocation;
                            crmAllocation.Type = allocation.Type;
                            crmAllocation.Duration = noOfWeeks;
                            crmAllocation.Title = allocation.Title;
                            crmAllocation.TicketId = model.ProjectID;
                            crmAllocation.SoftAllocation = UGITUtility.StringToBoolean(allocation.SoftAllocation);
                            crmAllocation.NonChargeable = UGITUtility.StringToBoolean(allocation.NonChargeable);
                            crmAllocation.IsLocked = UGITUtility.StringToBoolean(allocation.IsLocked);
                            crmAllocation.ID = allocation.ID;

                            crmAllocation.ID = 0;
                            CRMProjAllocManager.Insert(crmAllocation);

                            historyDesc.Add(string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", allocation.AssignedToName, allocation.TypeName, allocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", allocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", allocation.AllocationEndDate)));
                            projectID = allocation.ProjectID;
                            string roleName = string.Empty;

                            GlobalRole uRole = roleManager.Get(x => x.Id == allocation.Type);
                            if (uRole != null)
                                roleName = uRole.Name;

                            lstUserWithPercetage.Add(
                                new UserWithPercentage()
                                {
                                    EndDate = crmAllocation.AllocationEndDate ?? DateTime.MinValue,
                                    StartDate = crmAllocation.AllocationStartDate ?? DateTime.MinValue,
                                    Percentage = crmAllocation.PctAllocation,
                                    UserId = crmAllocation.AssignedTo,
                                    RoleTitle = roleName,
                                    ProjectEstiAllocId = UGITUtility.ObjectToString(crmAllocation.ID),
                                    RoleId = crmAllocation.Type,
                                    SoftAllocation = crmAllocation.SoftAllocation,
                                });
                        }

                        //to-do
                        var taskManager = new UGITTaskManager(context);
                        List<UGITTask> ptasks = taskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(model.ProjectID), model.ProjectID);
                        List<string> lstUsers = model.Allocations.Select(a => a.AssignedTo).ToList();
                        var res = ptasks.Where(x => x.AssignedTo != null && x.AssignedTo.Where(y => lstUsers != null && lstUsers.Contains(y.ToString())).Count() > 0).ToList();
                        newAllocatedUsers = newAllocatedUsers.Union(oldAllocatedUsers).ToList();
                        // Only create allocation enties if user is not in schedule
                        try
                        {
                            if (res == null || res.Count == 0)
                            {
                                ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate ()
                                {
                                    ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(model.ProjectID), model.ProjectID, lstUserWithPercetage, newAllocatedUsers);
                                    ResourceAllocationManager.UpdateHistory(context, historyDesc, model.ProjectID);
                                    historyDesc.ForEach(o =>
                                    {
                                        ULog.WriteLog("Allocation Template >> " + context.CurrentUser.Name + o);
                                    });

                                };
                                Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                                sThreadStartMethodUpdateCPRProjectAllocation.IsBackground = true;
                                sThreadStartMethodUpdateCPRProjectAllocation.Start();
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }

                    }
                    return Ok();
                }
            }
            catch (Exception e)
            {
                ULog.WriteException("UpdateBatchCRMAllocations_semaphore Lock:" + e.ToString());
                return Ok("UpdateBatchCRMAllocations_semaphore Lock:" + e.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
        }


        [HttpPost]
        [Route("DeleteCRMAllocation")]
        public async Task<IHttpActionResult> DeleteCRMAllocation(AllocationDeleteModel model)
        {
            await Task.FromResult(0);
            try
            {
                await uHelper._semaphore.WaitAsync();
                {
                    ApplicationContext context = HttpContext.Current.GetManagerContext();
                    ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);

                    resourceAllocationManager.DeleteProjectEstimatedAllocation(model);

                    return Ok();
                }
            }
            catch (Exception e)
            {
                ULog.WriteException("DeleteAllocations_semaphore Lock:" + e.ToString());
                return Ok("DeleteAllocations_semaphore Lock:" + e.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
        }

        [HttpGet]
        [Route("GetGroupTitles")]
        public async Task<IHttpActionResult> GetGroupTitles(string GroupID, string DivisionID)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            //JobTitleManager jobTitleManager = new JobTitleManager(context);
            //long department = UGITUtility.StringToInt(DepartmentID);
            //List<string> jobTitles = new List<string>();
            //if (department > 0)
            //    jobTitles = jobTitleManager.Load(x => x.RoleId == GroupID && x.DepartmentId == department).Select(x => x.Title).Distinct().ToList();
            //else
            //    jobTitles = jobTitleManager.Load(x => x.RoleId == GroupID).Select(x => x.Title).Distinct().ToList();

            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable dt = new DataTable();

            values.Add("@TenantID", context.TenantID);
            values.Add("@RoleId", GroupID);
            if(!DivisionID.Contains(Constants.Separator6) && UGITUtility.StringToInt(DivisionID) == 0)
                values.Add("@Dept", DivisionID);
            else
            {
                DepartmentManager departmentManager = new DepartmentManager(context);
                List<string> lstDepartments = departmentManager.Load(x => DivisionID.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).Contains(UGITUtility.ObjectToString(x.DivisionIdLookup))
                && !x.Deleted).Select(x => UGITUtility.ObjectToString(x.ID)).ToList();
                values.Add("@Dept", string.Join(Constants.Separator6, lstDepartments));
            }

            DataTable dtJobtitles = GetTableDataManager.GetData("JobtitlebyDept", values);
            //List<JobTitle> jobTitles = UGITUtility.ConvertCustomDataTable<JobTitle>(GetTableDataManager.GetData("JobtitlebyDept", values)); 
            List<string> jobTitles = dtJobtitles.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Title])).Distinct().ToList();
            string jsonProjectAllocations = JsonConvert.SerializeObject(jobTitles.OrderBy(x=>x));
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }
        [HttpGet]
        [Route("GetDepartmentsForDivision")]
        public async Task<IHttpActionResult> GetDepartmentsForDivision(string DivisionId)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            DepartmentManager departmentManager = new DepartmentManager(context);
            List<Department> lstDepartments = departmentManager.Load(x => x.DivisionIdLookup == UGITUtility.StringToLong(DivisionId)).OrderBy(x => x.Title).ToList();
            string jsonDepartments = JsonConvert.SerializeObject(lstDepartments);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonDepartments, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetDepartments")]
        public async Task<IHttpActionResult> GetDepartments(string GroupID)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            //JobTitleManager jobtitlemanager = new JobTitleManager(context);
            //List<JobTitle> jobTitles = jobtitlemanager.Load(x => x.RoleId == GroupID);
            Dictionary<string, object> values = new Dictionary<string, object>();
            DataTable dt = new DataTable();
            values.Add("@TenantID", context.TenantID);
            values.Add("@RoleId", GroupID);
            List<JobTitle> jobTitles = UGITUtility.ConvertCustomDataTable<JobTitle>(GetTableDataManager.GetData("JobtitlebyDept", values)); 
            List<long?> jobtitledepartments = jobTitles.Select(x => x.DepartmentId).Distinct().ToList();

            DepartmentManager departmentManager = new DepartmentManager(context);
            //List<Department> lstDepartments = new List<Department>();
            List<Department> lsstDepartments = departmentManager.Load(x => jobtitledepartments.Contains(x.ID) && x.Deleted != true).OrderBy(x => x.Title).ToList();

            CompanyManager objCompanyManager = new CompanyManager(context);
            List<Company> companies = objCompanyManager.LoadAllHierarchy();


            bool showCompany = companies.Count > 1;
            bool showDivision = context.ConfigManager.GetValueAsBool(ConfigConstants.EnableDivision);

            foreach (var department in lsstDepartments)
            {
                List<string> s = new List<string>();
                if (showCompany && department.CompanyIdLookup.HasValue)
                {
                    var cmp = companies.FirstOrDefault(x => x.ID == department.CompanyIdLookup);
                    if (cmp != null)
                        s.Add(cmp.Title);
                }

                if (showDivision && department.DivisionIdLookup.HasValue)
                {
                    List<CompanyDivision> divisions = new List<CompanyDivision>();
                    companies.ForEach(x => divisions.AddRange(x.CompanyDivisions));
                    var division = divisions.FirstOrDefault(x => x.ID == department.DivisionIdLookup);
                    if (division != null)
                        s.Add(division.Title);
                }

                s.Add(department.Title);

                department.Title = string.Join(" > ", s);
            }

            lsstDepartments = lsstDepartments.OrderBy(x => x.Title).ToList();
            lsstDepartments.Insert(0, new Department() { ID = 0, Title = "All Departments" });

            string jsonDepartments = JsonConvert.SerializeObject(lsstDepartments);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonDepartments, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IHttpActionResult> GetUserList(bool skipDisabled = false)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            UserProfileManager userManager = new UserProfileManager(context);
            List<UserProfile> userprofiles = userManager.GetUsersProfile();
            if (userprofiles != null && userprofiles.Count > 0)
            {
                if (skipDisabled)
                    userprofiles = userprofiles.Where(x => x.Enabled == true).ToList();
                userprofiles = userprofiles.OrderBy(x => x.Name).ToList();
            }
            string jsonUsers = JsonConvert.SerializeObject(userprofiles);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonUsers, Encoding.UTF8, "application/json");
            return ResponseMessage(response);

        }


        [HttpGet]
        [Route("GetUsersList")]
        public async Task<IHttpActionResult> GetUsersList(string assignedTo)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            UserProfileManager userProfile = new UserProfileManager(context);

            String[] arr = assignedTo.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 1)
            {
                assignedTo = arr[arr.Length - 1].Trim();
            }

            List<String> profiles = userProfile.GetUsersProfile().Where(x => (x.Email.ToLower().Contains(assignedTo.ToLower()) || x.Name.ToLower().Contains(assignedTo.ToLower()) || x.UserName.ToLower().Contains(assignedTo.ToLower()))).Select(x => x.Name).ToList();

            if (profiles != null && profiles.Count > 0)
            {
                string usersJson = JsonConvert.SerializeObject(profiles);

                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(usersJson, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            return Ok();
        }

        [HttpPost]
        [Route("UpdateTemplateName")]
        public async Task<IHttpActionResult> UpdateTemplateName(UpdateTemplateModel model)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectAllocationTemplateManager projectAllocationTemplateMGR = new ProjectAllocationTemplateManager(context);
            if (!string.IsNullOrEmpty(model.ID))
            {
                ProjectAllocationTemplate allocTemplate = projectAllocationTemplateMGR.LoadByID(UGITUtility.StringToInt(model.ID));
                if (allocTemplate != null)
                {
                    allocTemplate.Name = model.Name;
                    bool result = projectAllocationTemplateMGR.Update(allocTemplate);
                    if (result)
                    {
                        List<ProjectAllocationTemplate> allAllocTemplates = projectAllocationTemplateMGR.Load();
                        string jsonallAllocTemplates = JsonConvert.SerializeObject(allAllocTemplates);
                        var response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jsonallAllocTemplates, Encoding.UTF8, "application/json");
                        return ResponseMessage(response);
                    }
                    else
                        return Ok("Fail");
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("UpdateResourceAllocation")]
        public async Task<IHttpActionResult> UpdateResourceAllocation()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ULog.WriteLog("Copy Project Estimated Allocation into Resource Allocation started!");
            ResourceAllocationManager allocationmanager = new ResourceAllocationManager(context);
            allocationmanager.MigrateResourceAllocation();

            return Ok(true);
        }

        [HttpPost]
        [Route("CheckUpdateAlloctionInProcess")]
        public async Task<IHttpActionResult> CheckUpdateAlloctionInProcess()
        {
            await Task.FromResult(0);
            ResourceAllocationManager resourceManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
            return Ok(resourceManager.ProcessState());
        }

        [HttpPost]
        [Route("UpdateResourceSummary")]
        public async Task<IHttpActionResult> UpdateResourceSummary()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ULog.WriteLog("Refresh Resource Summary Started! ");
            ResourceAllocationManager allocationmanager = new ResourceAllocationManager(context);
            allocationmanager.UpdateResourceSummary();

            return Ok(true);
        }

        [HttpPost]
        [Route("CheckUpdateSummaryInProcess")]
        public async Task<IHttpActionResult> CheckUpdateSummaryInProcess()
        {
            await Task.FromResult(0);
            ResourceAllocationManager resourceManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
            return Ok(resourceManager.SummaryComplete());
        }

        [HttpPost]
        [Route("CopyPreviousWeekTimeSheet")]
        public async Task<IHttpActionResult> CopyPreviousWeekTimeSheet(CopyTimesheetModel model)
        {
            await Task.FromResult(0);

            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ResourceAllocationManager resourceAllocMGR = new ResourceAllocationManager(context);
            ResourceTimeSheetManager timesheetMGR = new ResourceTimeSheetManager(context);
            ResourceWorkItemsManager workItemsMGR = new ResourceWorkItemsManager(context);

            string selectedUserID = model.userID;
            DateTime weekStartDate = UGITUtility.StringToDateTime(model.startDate);

            if (string.IsNullOrEmpty(selectedUserID))
                return Ok("{\"status\":\"error\", \"message\":\"User is not selected.\"}");

            if (weekStartDate == DateTime.MinValue || weekStartDate == DateTime.MaxValue)
                return Ok("{\"status\":\"error\", \"message\":\"Week date is selected.\"}");

            //Get previous week workitems
            DateTime preWStartDate = weekStartDate.AddDays(-7);
            DateTime preWEndDate = weekStartDate.AddDays(-1);
            List<RResourceAllocation> preWeekAllocs = resourceAllocMGR.LoadByResource(model.userID, preWStartDate, preWEndDate, ResourceAllocationType.Allocation);
            DataTable preWeekTimesheet = timesheetMGR.LoadRawTableByResource(model.userID, preWStartDate, preWEndDate);
            List<long> preWeekWorkItems = preWeekAllocs.Select(x => x.ResourceWorkItemLookup).ToList();
            List<long> preTimeSheetWItems = new List<long>();
            if (preWeekTimesheet != null)
            {
                preTimeSheetWItems = preWeekTimesheet.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.ResourceWorkItemLookup)).Select(x => Convert.ToInt64(x[DatabaseObjects.Columns.ResourceWorkItemLookup])).ToList();
                preWeekWorkItems.AddRange(preTimeSheetWItems.Select(x => x));
            }

            if (preWeekWorkItems.Count == 0)
                return Ok("{\"status\":\"nochange\", \"message\":\"No new work items found!\"}");

            //Get current week workitems
            DateTime currentWStartDate = weekStartDate;
            DateTime currentWEndDate = weekStartDate.AddDays(6);
            List<RResourceAllocation> currentWeekAllocs = resourceAllocMGR.LoadByResource(model.userID, currentWStartDate, currentWEndDate, ResourceAllocationType.Allocation);
            DataTable currentWeekTimesheet = timesheetMGR.LoadRawTableByResource(model.userID, currentWStartDate, currentWEndDate);
            List<long> currentWeekWorkItems = currentWeekAllocs.Select(x => x.ResourceWorkItemLookup).ToList();
            List<long> currentTimeSheetWItems = new List<long>();
            if (currentWeekTimesheet != null)
            {
                currentTimeSheetWItems = currentWeekTimesheet.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.ResourceWorkItemLookup)).Select(x => Convert.ToInt64(x[DatabaseObjects.Columns.ResourceWorkItemLookup])).ToList();
                currentWeekWorkItems.AddRange(currentTimeSheetWItems.Select(x => x));
            }

            List<long> workItems = preWeekWorkItems.Except(currentWeekWorkItems).ToList();
            if (workItems.Count == 0)
                return Ok("{\"status\":\"nochange\", \"message\":\"No new work items found!\"}");

            //SPList workSheetList = SPListHelper.GetSPList(DatabaseObjects.Lists.ResourceTimeSheet, SPContext.Current.Web);
            foreach (int workItemID in workItems)
            {
                ResourceWorkItems workItemObject = workItemsMGR.LoadByID(workItemID);      // ResourceWorkItem.LoadById(SPContext.Current.Web, workItemID);

                ResourceTimeSheet item = new ResourceTimeSheet();   // workSheetList.AddItem();
                item.Title = string.Format("{0};#{1};#{2}", workItemObject.WorkItemType, workItemObject.WorkItem, workItemObject.SubWorkItem);
                item.WorkDate = weekStartDate;
                item.Resource = selectedUserID;
                item.ResourceWorkItemLookup = workItemID;
                item.HoursTaken = 0;
                timesheetMGR.Insert(item);
            }

            return  Ok("{\"status\":\"done\"}");
        }

        [HttpPost]
        [Route("AddTicketHours")]
        public async Task<IHttpActionResult> AddTicketHours(ActualHourModel model)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            Ticket TicketRequest = null;
            TicketHoursManager thManager = new TicketHoursManager(context);
            if (string.IsNullOrEmpty(model.ActualHourID))
            {
                if (!string.IsNullOrEmpty(model.TicketID))
                {
                    string TicketId = model.TicketID;
                    TicketRequest = new Ticket(context, uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), TicketId.Split('-')[0].ToString()));
                    DataRow currentTicket = Ticket.GetCurrentTicket(context, uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), TicketId.Split('-')[0].ToString()), TicketId);
                    int currentStep = Convert.ToInt32(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.StageStep));
                    if (UGITUtility.StringToDouble(model.Hours) > 0 && currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketResolutionComments))
                    {
                        DataTable spTicketHours = thManager.GetDataTable();
                        Utility.ActualHour ticketHour = new Utility.ActualHour();
                        ticketHour.TicketID = TicketId;
                        ticketHour.StageStep = currentStep;
                        //Allow user to enter max 24 hours in a day
                        double workHours = UGITUtility.StringToDouble(model.Hours);
                        if (workHours > 24)
                            workHours = 24;
                        DateTime workDate = UGITUtility.StringToDateTime(model.StartDate);
                        ticketHour.HoursTaken = workHours;
                        ticketHour.Comment = model.Description;
                        ticketHour.Resource = context.CurrentUser.Id;
                        ticketHour.WorkDate = workDate;
                        ticketHour.TenantID = context.TenantID;
                        thManager.AddUpdate(ticketHour);
                        //Get Total actual hours for current stage
                        string query = string.Format("{0}='{1}' and {2}={3}", DatabaseObjects.Columns.TicketId, TicketId, DatabaseObjects.Columns.StageStep, currentStep);
                        DataRow[] spColl = spTicketHours.Select(query);
                        //double totalActualHours = 0;
                        if (spColl != null && spColl.Count() > 0)
                        {
                            DataTable dt = spColl.CopyToDataTable();
                            //totalActualHours = UGITUtility.StringToDouble(dt.Compute("SUM(" + DatabaseObjects.Columns.HoursTaken + ")", string.Empty));
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(model.Description)))
                        {
                            currentTicket[DatabaseObjects.Columns.TicketResolutionComments] = uHelper.GetVersionString(context.CurrentUser.Id, Convert.ToString(model.Description), currentTicket, DatabaseObjects.Columns.TicketResolutionComments);
                        }
                        TicketRequest.CommitChanges(currentTicket, "", donotUpdateEscalations: false);
                        ////pcTicketHours.ShowOnPageLoad = false;
                        //// Notify requestor or action user of new comment if configured in Modules list for this module
                        //if (TicketRequest.Module.ActionUserNotificationOnComment || chkNotifyCommentRequestor.Checked || TicketRequest.Module.InitiatorNotificationOnComment)
                        //{
                        //    string ticketId = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
                        //    string mailBody = string.Format("{0} added the following comment to this ticket: <br/><br/>{1}", user.Name, txtResolutionDescription.Text.Trim());
                        //    string subject = string.Format("New Comment added to ticket: {0}", ticketId);
                        //    if (TicketRequest.Module.ActionUserNotificationOnComment)
                        //        TicketRequest.SendEmailToActionUsers(currentTicket, subject, mailBody);
                        //    //if (chkNotifyCommentRequestor.Checked)
                        //        //TicketRequest.SendEmailToRequestor(saveTicket, subject, mailBody);
                        //       // if (TicketRequest.Module.InitiatorNotificationOnComment) ;
                        //    //TicketRequest.SendEmailToInitiator(saveTicket, subject, mailBody);
                        //}

                        ConfigurationVariableManager cvHelper = new ConfigurationVariableManager(context);                    //Update user working hours inside resource timesheet if setting is enabled
                        if (UGITUtility.StringToBoolean(cvHelper.GetValue(ConfigConstants.CopyTicketActualsToRMM)))
                        {
                            int requestTypeLookup = Convert.ToInt16(currentTicket[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                            if (requestTypeLookup > 0)
                            {
                                ModuleRequestType requestType = TicketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.ID == requestTypeLookup);
                                if (requestType != null)
                                {
                                    //ResourceWorkItem workItem = new Helpers.ResourceWorkItem(SPContext.Current.Web.CurrentUser.ID);
                                    //workItem.Level1 = requestType.RMMCategory;
                                    //workItem.Level2 = requestType.Category;

                                    //ResourceTimeSheet.UpdateWorkingHours(SPContext.Current.Web, workItem, SPContext.Current.Web.CurrentUser.ID, workDate, workHours, false);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Utility.ActualHour ticketHour = thManager.TicketHourList().Where(x => x.ID == Convert.ToInt32(model.ActualHourID)).FirstOrDefault();
                double workHours = UGITUtility.StringToDouble(model.Hours);
                if (workHours > 24)
                    workHours = 24;
                DateTime workDate = UGITUtility.StringToDateTime(model.StartDate);
                ticketHour.HoursTaken = workHours;
                ticketHour.Comment = model.Description;
                ticketHour.Resource = context.CurrentUser.Id;
                ticketHour.WorkDate = workDate;
                ticketHour.TenantID = context.TenantID;
                thManager.AddUpdate(ticketHour);
            }   

            return Ok("{\"status\":\"done\"}");
        }

        [HttpGet]
        [Route("GetPMMLifeCycles")]
        public async Task<IHttpActionResult> GetPMMLifeCycles()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            LifeCycleManager lifeCycleManager = new LifeCycleManager(context);
            List<LifeCycle> lstlifeCycles = lifeCycleManager.LoadLifeCycleByModule(ModuleNames.PMM).OrderBy(x=>x.Name).ToList();
            if(lstlifeCycles != null)
            {
                string jsonLifeCycles = JsonConvert.SerializeObject(lstlifeCycles);

                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonLifeCycles, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetUsersInfo")]
        public async Task<IHttpActionResult> GetUsersInfo()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            var users = context.UserManager.LoadWithGroup(x=>x.Enabled).Select(x => new { id=x.Id, displayValue=x.Name });
            return Ok(users);
        }

        [HttpGet]
        [Route("GetBillingAndMargins")]
        public async Task<IHttpActionResult> GetBillingAndMargins(string Mode,DateTime StartDate, DateTime EndDate, bool CPR, bool OPM, bool CNS,
            bool Pipeline, bool Current, bool Closed, bool Billable, bool Overhead)
        {
            await Task.FromResult(0);

            ApplicationContext context = HttpContext.Current.GetManagerContext();
            try
            {
                string ticketfilter = string.Empty;
                List<string> selectedModules = new List<string>();
                if (CPR)
                    selectedModules.Add("CPR");
                if (OPM)
                    selectedModules.Add("OPM");
                if (CNS)
                    selectedModules.Add("CNS");
                if (Pipeline)
                    ticketfilter = "P";
                if (Current)
                    ticketfilter = ticketfilter + "C";
                if (Closed)
                    ticketfilter = ticketfilter + "R";
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", context.TenantID);
                //arrParams.Add("StartDate", new DateTime(DateTime.Now.Year, 1, 1));
                //arrParams.Add("EndDate", new DateTime(DateTime.Now.Year, 12, 31));
                arrParams.Add("StartDate", StartDate);
                arrParams.Add("EndDate", EndDate);
                arrParams.Add("modulenames", UGITUtility.ConvertListToString(selectedModules, Constants.Separator6));
                arrParams.Add("Mode", ticketfilter);
                arrParams.Add("Billable", Billable);
                arrParams.Add("Overhead", Overhead);

                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("GetBillingsAndMargins", arrParams);
                List<BillingAndMarginsResponse> lstBillings = new List<BillingAndMarginsResponse>();
                if (dtResultBillings != null && dtResultBillings.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResultBillings.Rows)
                    {
                        BillingAndMarginsResponse item = new BillingAndMarginsResponse();
                        item.Month = UGITUtility.ObjectToString(row["StartMonth"]);
                        //item.MonthOrder = UGITUtility.StringToInt(row["MonthOrder"]);
                        item.TotalBillingLaborRate = UGITUtility.ObjectToString(row["TotalBillingLaborRate"]);
                        item.TotalEmployeeCostRate = UGITUtility.ObjectToString(row["TotalEmployeeCostRate"]);
                        item.BilledResources = UGITUtility.StringToInt(row["BilledResources"]);
                        item.UnbilledResources = UGITUtility.StringToInt(row["UnbilledResources"]);
                        item.GrossMargin = UGITUtility.ObjectToString(row["GrossMargin"]);
                        item.MissedRevenues = UGITUtility.ObjectToString(row["MissedRevenues"]);
                        item.BilledWorkMonth = UGITUtility.ObjectToString(row["BilledWorkMonth"]);
                        item.UnBilledWorkMonth = UGITUtility.ObjectToString(row["UnBilledWorkMonth"]);
                        item.Utilization = UGITUtility.ObjectToString(row["Utilization"]);
                        lstBillings.Add(item);
                    }
                }

                if (lstBillings != null && lstBillings.Count > 0)
                    return Ok(lstBillings);
                else
                {
                    lstBillings.Add(new BillingAndMarginsResponse() { Month = DateTime.Now.ToString("MMM"), BilledResources = 0, GrossMargin = "$0", MissedRevenues = "$0", MonthOrder = 1, TotalBillingLaborRate = "$0", TotalCapacity = "$0", TotalProjects = 10, TotalEmployeeCostRate = "$0", UnbilledResources = 0 });
                    return Ok(lstBillings);
                }
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
           

            return Ok();
        }

        [HttpGet]
        [Route("GetMissedRevenues")]
        public async Task<IHttpActionResult> GetMissedRevenues(string Mode, DateTime StartDate, DateTime EndDate)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", context.TenantID);
                arrParams.Add("StartDate", new DateTime(DateTime.Now.Year, 1, 1));
                arrParams.Add("EndDate", new DateTime(DateTime.Now.Year, 12, 31));

                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("usp_GetTotalMissedRevenue", arrParams);
                List<MissedRevenueResponse> lstMissedRevenues = new List<MissedRevenueResponse>();
                if(dtResultBillings != null && dtResultBillings.Rows.Count > 0)
                {
                    foreach(DataRow row in dtResultBillings.Rows)
                    {
                        MissedRevenueResponse item = new MissedRevenueResponse();
                        item.MonthName = UGITUtility.ObjectToString(row["MonthName"]);
                        int monthNumber = 0;
                        monthNumber = DateTime.ParseExact(item.MonthName.ToUpper(), "MMM", CultureInfo.CurrentCulture).Month;
                        item.MonthOrder = monthNumber;
                        item.ResourceNotBilled = UGITUtility.StringToInt(row["ResourceNotBilled"]);
                        item.TotalMissedBilling = UGITUtility.ObjectToString(row["TotalMissedBilling"]);
                        item.TotalMissedCost = UGITUtility.ObjectToString(row["TotalMissedCost"]);
                        item.GrossMargin = UGITUtility.ObjectToString(row["GrossMargin"]);

                        lstMissedRevenues.Add(item);
                    }
                }

                if (lstMissedRevenues.Count > 0)
                {
                    lstMissedRevenues = lstMissedRevenues.OrderBy(x => x.MonthOrder).ToList();
                    return Ok(lstMissedRevenues);
                }
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetBillingDetails")]
        public async Task<IHttpActionResult> GetBillingDetails(string Mode, DateTime StartDate, DateTime EndDate, bool CPR, bool OPM, bool CNS,
            bool Pipeline, bool Current, bool Closed)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            try
            {
                string ticketfilter = string.Empty;
                List<string> selectedModules = new List<string>();
                if (CPR)
                    selectedModules.Add("CPR");
                if (OPM)
                    selectedModules.Add("OPM");
                if (CNS)
                    selectedModules.Add("CNS");
                if (Pipeline)
                    ticketfilter = "P";
                if (Current)
                    ticketfilter = ticketfilter + "C";
                if (Closed)
                    ticketfilter = ticketfilter + "R";
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", context.TenantID);
                arrParams.Add("StartDate", StartDate);
                arrParams.Add("EndDate", EndDate);
                arrParams.Add("modulenames", UGITUtility.ConvertListToString(selectedModules, Constants.Separator6));
                arrParams.Add("Mode", ticketfilter);
                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("usp_GetBillingDetails", arrParams);
                List<MissedRevenueResponse> lstMissedRevenues = new List<MissedRevenueResponse>();
                if (dtResultBillings != null && dtResultBillings.Rows.Count > 0)
                {
                    return Ok(dtResultBillings);
                }
                
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetMissedRevenueDetails")]
        public async Task<IHttpActionResult> GetMissedRevenueDetails(string Mode, DateTime StartDate, DateTime EndDate)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", context.TenantID);
                arrParams.Add("StartDate", StartDate);
                arrParams.Add("EndDate", EndDate);

                DataTable dtResultMissedRevenue = uGITDAL.ExecuteDataSetWithParameters("usp_GetMissedRevenueDetail", arrParams);
                List<MissedRevenueResponse> lstMissedRevenues = new List<MissedRevenueResponse>();
                if (dtResultMissedRevenue != null && dtResultMissedRevenue.Rows.Count > 0)
                {
                    return Ok(dtResultMissedRevenue);
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetExecutiveKpi")]
        public async Task<IHttpActionResult> GetExecutiveKpi(string Year, string Category)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", context.TenantID);
                arrParams.Add("Year", Year);
                arrParams.Add("Category", Category);

                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("usp_ExecutiveKpi", arrParams);
                
                List<ExecutiveKpi> lstExecutiveKpi = new List<ExecutiveKpi>();
                if (dtResultBillings != null && dtResultBillings.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResultBillings.Rows)
                    {
                        ExecutiveKpi item = new ExecutiveKpi();
                        if (Category.EqualsIgnoreCase("JobTitle"))
                            item.Category = UGITUtility.ObjectToString(row["JobTitle"]);
                        else if (Category.EqualsIgnoreCase("Role"))
                            item.Category = UGITUtility.ObjectToString(row["Role"]);
                        else if (Category.EqualsIgnoreCase("Division"))
                            item.Category = UGITUtility.ObjectToString(row["Division"]);
                        else if (Category.EqualsIgnoreCase("Sector"))
                            item.Category = UGITUtility.ObjectToString(row["Sector"]);
                        else if (Category.EqualsIgnoreCase("Studio"))
                            item.Category = UGITUtility.ObjectToString(row["Studio"]);
                        else if (Category.EqualsIgnoreCase("ProjectView"))
                        {
                            item.Category = UGITUtility.ObjectToString(row["TicketID"]);
                            item.TicketTitle = UGITUtility.ObjectToString(row["TicketTitle"]);
                            item.ProjectId = UGITUtility.ObjectToString(row["ProjectId"]);

                            item.ResourceHours = UGITUtility.ObjectToString(row["ResourceHours"]);
                            item.ResourceBillings = UGITUtility.ObjectToString(row["ResourceBillings"]);
                            item.ResourceCosts = UGITUtility.ObjectToString(row["ResourceCosts"]);
                            item.ResourceMargins = UGITUtility.ObjectToString(row["ResourceMargins"]);
                            item.UtilizationRate = UGITUtility.ObjectToString(row["UtilizationRate"]);
                        }

                        item.Margins = UGITUtility.ObjectToString(row["Margins"]);
                        item.ProjectedMargins = UGITUtility.ObjectToString(row["ProjectedMargins"]);
                        item.ProjectMargins = UGITUtility.ObjectToString(row["ProjectMargins"]);
                        item.EffectiveUtilization = UGITUtility.ObjectToString(row["EffectiveUtilization"]);
                        item.CommittedUtilization = UGITUtility.ObjectToString(row["CommittedUtilization"]);
                        item.PipelineUtilization = UGITUtility.ObjectToString(row["PipelineUtilization"]);

                        item.RevenuesRealized = UGITUtility.ObjectToString(row["RevenuesRealized"]);
                        item.RevenuesLost = UGITUtility.ObjectToString(row["RevenuesLost"]);
                        item.CommittedRevenues = UGITUtility.ObjectToString(row["CommittedRevenues"]);

                        item.PipelineRevenues = UGITUtility.ObjectToString(row["PipelineRevenues"]);

                        item.MarginsRealized = UGITUtility.ObjectToString(row["MarginsRealized"]);
                        item.MarginsLost = UGITUtility.ObjectToString(row["MarginsLost"]);
                        item.CommittedMargins = UGITUtility.ObjectToString(row["CommittedMargins"]);

                        lstExecutiveKpi.Add(item);
                    }
                }

                if (lstExecutiveKpi.Count > 0)
                {
                    lstExecutiveKpi = lstExecutiveKpi.OrderBy(x => x.Category).ToList();
                    return Ok(lstExecutiveKpi);
                }
                
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("GetPDFData")]

        public async Task<IHttpActionResult> GetPDFData([FromBody] string objPDFData)
        {
            await Task.FromResult(0);
            string resMsg = "";
            try
            {
                if (objPDFData != null && !string.IsNullOrEmpty(objPDFData))
                {
                    var pdfBinary = Convert.FromBase64String(objPDFData);

                    if (pdfBinary != null)
                    {
                        var dir = HttpContext.Current.Server.MapPath("~/ResourceUtilization");

                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        var fileName = dir + "\\ResourceUtilization-" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".pdf";

                        // save file in floder to send email
                        using (var fs = new FileStream(fileName, FileMode.Create))
                        using (var writer = new BinaryWriter(fs))
                        {
                            writer.Write(pdfBinary, 0, pdfBinary.Length);
                            writer.Close();
                        }

                        //send email
                        ApplicationContext context = HttpContext.Current.GetManagerContext();
                        string emailTo = context.CurrentUser.Email;
                        string subject = "ResourceUtilization pdf ";
                        String emailCC = "";
                        string body = "";
                        string[] attachments = new string[] { fileName };
                        MailMessenger mail = new MailMessenger(context);
                        resMsg = mail.SendMail(emailTo, subject, emailCC, body.ToString(), false, attachments, false);

                        //delete file post email sent
                        File.Delete(fileName);
                    }
                }
                return Ok(new { message = "Email sent successfully" });
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
                return Ok(new { message ="Failed to send the email" });
            }
        }

        [HttpGet]
        [Route("GetResourceUtilizationIndex")]
        public async Task<IHttpActionResult> GetResourceUtilizationIndex(string Year, string Category, string Type)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo newCulture = new CultureInfo(currentCulture.Name);
            newCulture.NumberFormat.CurrencyNegativePattern = 1;
            Thread.CurrentThread.CurrentCulture = newCulture;

            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", context.TenantID);
                arrParams.Add("Year", Year);
                arrParams.Add("Category", Category);
                arrParams.Add("Type", Type);

                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("usp_GetResourceUtilizationIndex", arrParams);

                List<uGovernIT.Manager.RMM.ViewModel.ResourceUtilizationIndex> lstResourceUtilizationIndex = new List<uGovernIT.Manager.RMM.ViewModel.ResourceUtilizationIndex>();
                if (dtResultBillings != null && dtResultBillings.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResultBillings.Rows)
                    {
                        uGovernIT.Manager.RMM.ViewModel.ResourceUtilizationIndex item = new uGovernIT.Manager.RMM.ViewModel.ResourceUtilizationIndex();
                        item.Category = UGITUtility.ObjectToString(row["Category"]);
                        item.DataType = UGITUtility.ObjectToString(row["DataType"]);
                        item.KPI = UGITUtility.ObjectToString(row["KPI"]);
                        item.Jan = FormatNumbers(row["Jan"], item.DataType);
                        item.Feb = FormatNumbers(row["Feb"], item.DataType);
                        item.Mar = FormatNumbers(row["Mar"], item.DataType);
                        item.Apr = FormatNumbers(row["Apr"], item.DataType);
                        item.May = FormatNumbers(row["May"], item.DataType);
                        item.Jun = FormatNumbers(row["Jun"], item.DataType);
                        item.Jul = FormatNumbers(row["Jul"], item.DataType);
                        item.Aug = FormatNumbers(row["Aug"], item.DataType);
                        item.Sep = FormatNumbers(row["Sep"], item.DataType);
                        item.Oct = FormatNumbers(row["Oct"], item.DataType);
                        item.Nov = FormatNumbers(row["Nov"], item.DataType);
                        item.Dec = FormatNumbers(row["Dec"], item.DataType);

                        lstResourceUtilizationIndex.Add(item);
                    }
                }

                if (lstResourceUtilizationIndex.Count > 0)
                {                    
                    return Ok(lstResourceUtilizationIndex);
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return Ok();
        }

        private string FormatNumbers(object value, string dataType)
        {
            if (value != DBNull.Value)
            {
                if (dataType == "percentage")
                    return $"{value}%";
                if (dataType == "currency")
                    return string.Format("{0:C0}", Convert.ToDecimal(value));
                if (dataType == "numberwithoutdecimal")
                    return string.Format("{0:N0}", Convert.ToDecimal(value));
            }

            return UGITUtility.ObjectToString(value);
        }

        [HttpGet]
        [Route("GetBillingAndMarginsYTD")]
        public async Task<IHttpActionResult> GetBillingAndMarginsYTD(string Mode, DateTime StartDate, DateTime EndDate, bool CPR, bool OPM, bool CNS,
            bool Pipeline, bool Current, bool Closed, bool Billable, bool Overhead)
        {
            await Task.FromResult(0);

            ApplicationContext context = HttpContext.Current.GetManagerContext();
            try
            {
                string ticketfilter = string.Empty;
                List<string> selectedModules = new List<string>();
                if (CPR)
                    selectedModules.Add("CPR");
                if (OPM)
                    selectedModules.Add("OPM");
                if (CNS)
                    selectedModules.Add("CNS");
                if (Pipeline)
                    ticketfilter = "P";
                if (Current)
                    ticketfilter = ticketfilter + "C";
                if (Closed)
                    ticketfilter = ticketfilter + "R";
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", context.TenantID);
                //arrParams.Add("StartDate", new DateTime(DateTime.Now.Year, 1, 1));
                //arrParams.Add("EndDate", new DateTime(DateTime.Now.Year, 12, 31));
                arrParams.Add("StartDate", StartDate);
                arrParams.Add("EndDate", EndDate);
                arrParams.Add("modulenames", UGITUtility.ConvertListToString(selectedModules, Constants.Separator6));
                arrParams.Add("Mode", ticketfilter);


                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("GetBillingsAndMarginsYTD", arrParams);
                List<BillingAndMarginsResponse> lstBillings = new List<BillingAndMarginsResponse>();
                if (dtResultBillings != null && dtResultBillings.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResultBillings.Rows)
                    {
                        BillingAndMarginsResponse item = new BillingAndMarginsResponse();
                        //item.Month = UGITUtility.ObjectToString(row["StartMonth"]);
                        //item.MonthOrder = UGITUtility.StringToInt(row["MonthOrder"]);
                        item.TotalBillingLaborRate = UGITUtility.ObjectToString(row["TotalBillingLaborRate"]);
                        item.TotalEmployeeCostRate = UGITUtility.ObjectToString(row["TotalEmployeeCostRate"]);
                        item.BilledResources = UGITUtility.StringToInt(row["BilledResources"]);
                        item.UnbilledResources = UGITUtility.StringToInt(row["UnbilledResources"]);
                        item.GrossMargin = UGITUtility.ObjectToString(row["GrossMargin"]);
                        item.MissedRevenues = UGITUtility.ObjectToString(row["MissedRevenues"]);
                        lstBillings.Add(item);
                    }
                }

                if (lstBillings != null && lstBillings.Count > 0)
                    return Ok(lstBillings);
                else
                {
                    lstBillings.Add(new BillingAndMarginsResponse() { Month =  DateTime.Now.ToString("MMM"), BilledResources = 0, GrossMargin = "$0", MissedRevenues = "$0", MonthOrder = 1, TotalBillingLaborRate = "$0", TotalCapacity = "$0", TotalProjects = 10, TotalEmployeeCostRate="$0", UnbilledResources=0 });
                    return Ok(lstBillings);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }


            return Ok();
        }

        [HttpGet]
        [Route("GetBilledWorkMonth")]
        public async Task<IHttpActionResult> GetBilledWorkMonth()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@tenantID", context.TenantID);
            
            DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("GetBilledWorkMonth", arrParams);
            if (dtResultBillings != null && dtResultBillings.Rows.Count > 0)
            {
                return Ok(dtResultBillings);
            }
            return Ok();
        }


        [HttpGet]
        [Route("GetOtherAllocationDetails")]
        public async Task<IHttpActionResult> GetOtherAllocationDetails(string AllocationType = "", string ResultType = "records", bool ShowByUsersDivision = false)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            //DateTime wkStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime wkStartDate = uHelper.GetWeekStartDate(DateTime.Today);
            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@StartDate", wkStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            arrParams.Add("@AllocationType", AllocationType);  // parameter can be conflicts or unfilled
            arrParams.Add("@ResultType", ResultType);  // parameter can be records, count
            arrParams.Add("@ShowByUsersDivision", ShowByUsersDivision);
            arrParams.Add("@TenantID", context.TenantID);
            arrParams.Add("@UserId", context.CurrentUser.Id);

            DataTable dtResult = uGITDAL.ExecuteDataSetWithParameters("GetOtherAllocationDetails", arrParams);
            return Ok(dtResult);
        }

        [HttpGet]
        [Route("GetAllocationConflicts")]
        public async Task<IHttpActionResult> GetAllocationConflicts(bool ShowByUsersDivision = false)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            //DateTime wkStartDate = UGITUtility.StringToDateTime(DateTime.Today);
            DateTime wkStartDate = uHelper.GetWeekStartDate(DateTime.Today);
            DateTime wkEndDate = wkStartDate.EndOfWeek(DayOfWeek.Sunday);

            int totalAllocConflicts = 0, totalAllocConflictsThisWeek = 0, totalAllocConflictsin3Weeks = 0;

            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@StartDate", wkStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            arrParams.Add("@EndDate", wkEndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            arrParams.Add("@ShowByUsersDivision", ShowByUsersDivision);
            arrParams.Add("@TenantID", context.TenantID);
            arrParams.Add("@UserId", context.CurrentUser.Id);
            DataTable dtResult = uGITDAL.ExecuteDataSetWithParameters("GetAllocationConflicts", arrParams);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                totalAllocConflicts = Convert.ToInt32(dtResult.Rows[0]["TotalAllocConflicts"]);
                totalAllocConflictsThisWeek = Convert.ToInt32(dtResult.Rows[0]["TotalAllocConflictsThisWeek"]);
                totalAllocConflictsin3Weeks = Convert.ToInt32(dtResult.Rows[0]["TotalAllocConflictsin3Weeks"]);
            }

            List<AllocationsResponse> lstAlloc = new List<AllocationsResponse>();
            AllocationsResponse alloc = new AllocationsResponse() { totalAlloc = totalAllocConflicts, totalAllocThisWeek = totalAllocConflictsThisWeek, totalAllocin3Weeks = totalAllocConflictsin3Weeks };
            lstAlloc.Add(alloc);
            string jsonmodules = JsonConvert.SerializeObject(lstAlloc);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetAllocationConflictProjects")]
        public async Task<IHttpActionResult> GetAllocationConflictProjects()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            //DateTime wkStartDate = UGITUtility.StringToDateTime(DateTime.Today);
            DateTime wkStartDate = uHelper.GetWeekStartDate(DateTime.Today);

            Dictionary<string, object> arrParams = new Dictionary<string, object>();
            arrParams.Add("@StartDate", wkStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            arrParams.Add("@TenantID", context.TenantID);

            DataTable dtResult = uGITDAL.ExecuteDataSetWithParameters("GetAllocationConflictProjects", arrParams);
            return Ok(dtResult);
        }


        [HttpGet]
        [Route("GetUnfilledAllocations")]
        public async Task<IHttpActionResult> GetUnfilledAllocations()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            //DateTime wkStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            //DateTime wkEndDate = wkStartDate.AddDays(7).AddSeconds(-1);

            //DateTime wkStartDate = UGITUtility.StringToDateTime(DateTime.Today);
            DateTime wkStartDate = uHelper.GetWeekStartDate(DateTime.Today);
            DateTime wkEndDate = wkStartDate.EndOfWeek(DayOfWeek.Sunday);

            ProjectEstimatedAllocationManager crmAllocationMgr = new ProjectEstimatedAllocationManager(context);
            List<ProjectEstimatedAllocation> projectAllocations = crmAllocationMgr.Load(x => x.AllocationStartDate <= wkStartDate && x.AllocationEndDate >= wkStartDate && x.AssignedTo == Guid.Empty.ToString() && x.Deleted != true);

            int totalUnfilledAlloc = projectAllocations.Count();
            int totalUnfilledAllocThisWeek = projectAllocations.Where(x => x.AllocationStartDate >= wkStartDate && x.AllocationStartDate <= wkEndDate).Count();
            int totalUnfilledAllocin3Weeks = projectAllocations.Where(x => x.AllocationStartDate >= wkStartDate && x.AllocationStartDate <= wkStartDate.AddDays(7)).Count();

            List<AllocationsResponse> lstAlloc = new List<AllocationsResponse>();
            AllocationsResponse alloc = new AllocationsResponse() { totalAlloc = totalUnfilledAlloc, totalAllocThisWeek = totalUnfilledAllocThisWeek, totalAllocin3Weeks = totalUnfilledAllocin3Weeks };
            lstAlloc.Add(alloc);
            string jsonmodules = JsonConvert.SerializeObject(lstAlloc);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetUnfilledProjectAllocations")]
        public async Task<IHttpActionResult> GetUnfilledProjectAllocations()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            //DateTime wkStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            //DateTime wkEndDate = wkStartDate.AddDays(7).AddSeconds(-1);

            //DateTime wkStartDate = UGITUtility.StringToDateTime(DateTime.Today);
            DateTime wkStartDate = uHelper.GetWeekStartDate(DateTime.Today);
            DateTime wkEndDate = wkStartDate.EndOfWeek(DayOfWeek.Sunday);

            ProjectEstimatedAllocationManager crmAllocationMgr = new ProjectEstimatedAllocationManager(context);
            List<ProjectEstimatedAllocation> projectAllocations = crmAllocationMgr.Load(x => x.AllocationStartDate <= wkStartDate && x.AllocationEndDate >= wkStartDate && x.AssignedTo == Guid.Empty.ToString() && x.Deleted != true);

            int totalUnfilledAlloc = projectAllocations.Count();
            int totalUnfilledAllocThisWeek = projectAllocations.Where(x => x.AllocationStartDate >= wkStartDate && x.AllocationStartDate <= wkEndDate).Count();
            int totalUnfilledAllocin3Weeks = projectAllocations.Where(x => x.AllocationStartDate >= wkStartDate && x.AllocationStartDate <= wkStartDate.AddDays(7)).Count();

            List<AllocationsResponse> lstAlloc = new List<AllocationsResponse>();
            AllocationsResponse alloc = new AllocationsResponse() { totalAlloc = totalUnfilledAlloc, totalAllocThisWeek = totalUnfilledAllocThisWeek, totalAllocin3Weeks = totalUnfilledAllocin3Weeks };
            lstAlloc.Add(alloc);
            string jsonmodules = JsonConvert.SerializeObject(lstAlloc);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetUnfilledPipelineAllocations")]
        public async Task<IHttpActionResult> GetUnfilledPipelineAllocations()
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            //DateTime wkStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            //DateTime wkEndDate = wkStartDate.AddDays(7).AddSeconds(-1);

            DateTime wkStartDate = uHelper.GetWeekStartDate(DateTime.Today);
            DateTime wkEndDate = wkStartDate.EndOfWeek(DayOfWeek.Sunday);

            ProjectEstimatedAllocationManager crmAllocationMgr = new ProjectEstimatedAllocationManager(context);
            List<ProjectEstimatedAllocation> projectAllocations = crmAllocationMgr.Load(x => x.AllocationStartDate <= wkStartDate && x.AllocationEndDate >= wkStartDate && x.AssignedTo == Guid.Empty.ToString() && x.Deleted != true);

            int totalUnfilledAlloc = projectAllocations.Count();
            int totalUnfilledAllocThisWeek = projectAllocations.Where(x => x.AllocationStartDate >= wkStartDate && x.AllocationStartDate <= wkEndDate).Count();
            int totalUnfilledAllocin3Weeks = projectAllocations.Where(x => x.AllocationStartDate >= wkStartDate && x.AllocationStartDate <= wkStartDate.AddDays(7)).Count();

            List<AllocationsResponse> lstAlloc = new List<AllocationsResponse>();
            AllocationsResponse alloc = new AllocationsResponse() { totalAlloc = totalUnfilledAlloc, totalAllocThisWeek = totalUnfilledAllocThisWeek, totalAllocin3Weeks = totalUnfilledAllocin3Weeks };
            lstAlloc.Add(alloc);
            string jsonmodules = JsonConvert.SerializeObject(lstAlloc);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("AllocateUnfilledResource")]
        public async Task<IHttpActionResult> AllocateUnfilledResource(string userid, string  id, string projectid)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
            GlobalRoleManager roleManager = new GlobalRoleManager(context);

            List<ProjectEstimatedAllocation> crmAllocation = new List<ProjectEstimatedAllocation>();
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            ProjectEstimatedAllocation projectEstimatedAllocation = CRMProjAllocManager.LoadByID(UGITUtility.StringToLong(id));
            if (projectEstimatedAllocation != null)
            {
                projectEstimatedAllocation.AssignedTo = userid;
                CRMProjAllocManager.Update(projectEstimatedAllocation);
            }

            crmAllocation = CRMProjAllocManager.Load(x=> x.TicketId == projectid && x.Deleted == false).ToList();
            crmAllocation.ForEach(x =>
            {
                string roleName = string.Empty;
                GlobalRole uRole = roleManager.Get(y => y.Id == x.Type);
                if (uRole != null)
                    roleName = uRole.Name;

                lstUserWithPercetage.Add(new UserWithPercentage() { EndDate = x.AllocationEndDate ?? DateTime.MinValue, StartDate = x.AllocationStartDate ?? DateTime.MinValue, Percentage = x.PctAllocation, UserId = x.AssignedTo, RoleTitle = roleName, ProjectEstiAllocId = UGITUtility.ObjectToString(x.ID), RoleId = x.Type });
            });

            ThreadStart ts = delegate ()
            {
                ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(projectid), projectid, lstUserWithPercetage, null);
            };
            Thread th = new Thread(ts);
            th.IsBackground = true;
            th.Start();

            string jsonmodules = JsonConvert.SerializeObject("");
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("AllocateConflictingResource")]
        public async Task<IHttpActionResult> AllocateConflictingResource(string userid, string id, string projectid,string PTOid, string opt)
        {
            await Task.FromResult(0);
            
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ProjectEstimatedAllocationManager CRMProjAllocManager = new ProjectEstimatedAllocationManager(context);
            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
            GlobalRoleManager roleManager = new GlobalRoleManager(context);

            List<ProjectEstimatedAllocation> crmAllocation = new List<ProjectEstimatedAllocation>();
            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            RResourceAllocation oldRA = resourceAllocationManager.LoadByID(UGITUtility.StringToLong(id));
            ProjectEstimatedAllocation projectEstimatedAllocation = new ProjectEstimatedAllocation();
            if (oldRA != null && oldRA.ProjectEstimatedAllocationId != null)
                projectEstimatedAllocation = CRMProjAllocManager.LoadByID(UGITUtility.StringToLong(oldRA.ProjectEstimatedAllocationId));

            RResourceAllocation ptoRA = resourceAllocationManager.LoadByID(UGITUtility.StringToLong(PTOid));

            if (opt.EqualsIgnoreCase("option1"))
            {
                if (projectEstimatedAllocation != null)
                {
                    //Allocation 1
                    int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, oldRA.AllocationStartDate.Value, Convert.ToDateTime(ptoRA.AllocationStartDate).AddDays(-1));
                    int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                    projectEstimatedAllocation.AllocationEndDate = Convert.ToDateTime(ptoRA.AllocationStartDate).AddDays(-1);
                    projectEstimatedAllocation.Duration = noOfWeeks;
                    CRMProjAllocManager.Update(projectEstimatedAllocation);

                    //Allocation 2
                    noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, ptoRA.AllocationStartDate.Value, oldRA.AllocationEndDate.Value);
                    noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                    ProjectEstimatedAllocation peNewResource = new ProjectEstimatedAllocation();
                    peNewResource.AssignedTo = userid;
                    peNewResource.TicketId = projectid;
                    peNewResource.Duration = noOfWeeks;
                    peNewResource.AllocationStartDate = ptoRA.AllocationStartDate.Value;
                    peNewResource.AllocationEndDate = oldRA.AllocationEndDate.Value;
                    peNewResource.PctAllocation = projectEstimatedAllocation.PctAllocation;
                    peNewResource.Type = projectEstimatedAllocation.Type;
                    peNewResource.Deleted = false;
                    CRMProjAllocManager.Insert(peNewResource);
                }
            }
            else if (opt.EqualsIgnoreCase("option2"))
            {
                //Allocation 1
                int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, oldRA.AllocationStartDate.Value, Convert.ToDateTime(ptoRA.AllocationStartDate).AddDays(-1));
                int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                projectEstimatedAllocation.AllocationEndDate = Convert.ToDateTime(ptoRA.AllocationStartDate).AddDays(-1);
                CRMProjAllocManager.Update(projectEstimatedAllocation);

                //Allocation 2
                noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, ptoRA.AllocationStartDate.Value, ptoRA.AllocationEndDate.Value);
                noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                ProjectEstimatedAllocation peNewResource = new ProjectEstimatedAllocation();
                peNewResource.AssignedTo = userid;
                peNewResource.TicketId = projectid;
                peNewResource.Duration = noOfWeeks;
                peNewResource.AllocationStartDate = ptoRA.AllocationStartDate.Value;
                peNewResource.AllocationEndDate = ptoRA.AllocationEndDate.Value;
                peNewResource.PctAllocation = projectEstimatedAllocation.PctAllocation;
                peNewResource.Type = projectEstimatedAllocation.Type;
                peNewResource.Deleted = false;
                CRMProjAllocManager.Insert(peNewResource);

                //Allocation 3
                noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, Convert.ToDateTime(ptoRA.AllocationStartDate).AddDays(1), oldRA.AllocationEndDate.Value);
                noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);

                if (oldRA.AllocationEndDate.Value > ptoRA.AllocationEndDate)
                {
                    peNewResource = new ProjectEstimatedAllocation();
                    peNewResource.AssignedTo = projectEstimatedAllocation.AssignedTo;
                    peNewResource.TicketId = projectid;
                    peNewResource.Duration = noOfWeeks;
                    peNewResource.AllocationStartDate = ptoRA.AllocationStartDate.Value;
                    peNewResource.AllocationEndDate = ptoRA.AllocationEndDate.Value;
                    peNewResource.PctAllocation = projectEstimatedAllocation.PctAllocation;
                    peNewResource.Type = projectEstimatedAllocation.Type;
                    peNewResource.Deleted = false;
                    CRMProjAllocManager.Insert(peNewResource);
                }
            }

            ptoRA.Attachments = $"replaced";
            resourceAllocationManager.Update(ptoRA);

            crmAllocation = CRMProjAllocManager.Load(x => x.TicketId == projectid && x.Deleted == false).ToList();
            crmAllocation.ForEach(x =>
            {
                string roleName = string.Empty;
                GlobalRole uRole = roleManager.Get(y => y.Id == x.Type);
                if (uRole != null)
                    roleName = uRole.Name;

                lstUserWithPercetage.Add(new UserWithPercentage() { EndDate = x.AllocationEndDate ?? DateTime.MinValue, StartDate = x.AllocationStartDate ?? DateTime.MinValue, Percentage = x.PctAllocation, UserId = x.AssignedTo, RoleTitle = roleName, ProjectEstiAllocId = UGITUtility.ObjectToString(x.ID), RoleId = x.Type });
            });

            ThreadStart ts = delegate ()
            {
                ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(projectid), projectid, lstUserWithPercetage, null);
            };
            Thread th = new Thread(ts);
            th.IsBackground = true;
            th.Start();
            
            string jsonmodules = JsonConvert.SerializeObject("");
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("GetResourceAllocations")]
        public async Task<IHttpActionResult> GetResourceAllocations(string userid, string selecteduserids, string selectedyear, bool chkIncludeClosed, string userall)
        {
            await Task.FromResult(0);
            DateTime startDateRange;
            DateTime endDateRange;
            try
            {
                if (!string.IsNullOrWhiteSpace(selectedyear))
                {
                    startDateRange = new DateTime(UGITUtility.StringToInt(selectedyear), 1, 1);
                    endDateRange = new DateTime(UGITUtility.StringToInt(selectedyear), 12, 31);
                }
                else
                {
                    startDateRange = new DateTime(UGITUtility.StringToInt(DateTime.Now.Year), 1, 1);
                    endDateRange = new DateTime(UGITUtility.StringToInt(DateTime.Now.Year), 12, 31);
                }
                DataTable allocations = null;
                ResourceAllocationManager allocManager = new ResourceAllocationManager(HttpContext.Current.GetManagerContext());
                if (!string.IsNullOrEmpty(selecteduserids) && selecteduserids != "null")
                {
                    List<string> userIds = UGITUtility.ConvertStringToList(selecteduserids, Constants.Separator6);
                    allocations = allocManager.LoadWorkAllocationByDate(userIds, 4, sDate: startDateRange, eDate: endDateRange);
                }
                else if(!string.IsNullOrEmpty(userall) && userall != "null")
                {
                    List<string> userIds = UGITUtility.ConvertStringToList(userall, Constants.Separator6);
                    allocations = allocManager.LoadWorkAllocationByDate(userIds, 4, sDate: startDateRange, eDate: endDateRange);
                }
                else
                {
                    allocations = allocManager.LoadWorkAllocationByDate(new List<string>() { userid }, 4, sDate: startDateRange, eDate: endDateRange);
                }

                if (allocations != null && allocations.Rows.Count > 0)
                {
                    allocations.DefaultView.Sort = string.Format("{0} asc, {1} asc, {2} asc, {3} asc", DatabaseObjects.Columns.Resource, DatabaseObjects.Columns.Closed, DatabaseObjects.Columns.WorkItemType, DatabaseObjects.Columns.WorkItem);
                }

                if (allocations != null && allocations.Rows.Count > 0 && chkIncludeClosed == false)
                {
                    DataView view = allocations.AsDataView();
                    view.RowFilter = $"{DatabaseObjects.Columns.Closed} = 'False' OR {DatabaseObjects.Columns.Closed} is NULL";
                    allocations = view.ToTable();
                }

                DataColumn parentCol = new DataColumn("ParentId", typeof(int));
                allocations.Columns.Add(parentCol);
                DataColumn childCol = new DataColumn("ChildId", typeof(int));
                allocations.Columns.Add(childCol);
                DataColumn colorCol = new DataColumn("Color", typeof(string));
                allocations.Columns.Add(colorCol);

                Dictionary<string, DataRow> dict = allocations.AsEnumerable()
                    .GroupBy(x => x.Field<string>("ResourceId"), y => y)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                int counter = 1;
                foreach (DataRow row in dict.Values)
                {
                    DataRow dr = allocations.NewRow();
                    string ResourceId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.ResourceId]);
                    int parentId = counter;
                    dr["Id"] = -Convert.ToInt64(row["Id"]);
                    dr["ChildId"] = parentId;
                    dr["ParentId"] = 0;
                    dr["ResourceId"] = ResourceId;
                    dr["ResourceUser"] = UGITUtility.ObjectToString(row["ResourceUser"]);
                    dr["SubWorkItem"] = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.SubWorkItem]);
                    dr["Title"] = UGITUtility.ObjectToString(row["ResourceUser"]);
                    dr["WorkItemLink"] = UGITUtility.ObjectToString(row["WorkItemLink"]);
                    dr["Color"] = 1;
                    
                    DataRow[] childRows = allocations.Select($"ResourceId = '{ResourceId}'");
                    if (childRows != null && childRows.Count() > 0)
                    {
                        dr[DatabaseObjects.Columns.AllocationStartDate] = childRows.AsEnumerable()
                                            .Select(cols => cols.Field<DateTime>("AllocationStartDate")).OrderBy(p => p.Ticks).FirstOrDefault();
                        dr[DatabaseObjects.Columns.AllocationEndDate] = childRows.AsEnumerable().Select(cols => cols.Field<DateTime>("AllocationEndDate"))
                            .OrderByDescending(p => p.Ticks).FirstOrDefault();
                        foreach (DataRow childRow in childRows)
                        {
                            childRow["ChildId"] = ++counter;
                            childRow["ParentId"] = parentId;
                            childRow["Color"] = 0;  // default color is blue

                            double pct = UGITUtility.StringToDouble(childRow[DatabaseObjects.Columns.PctAllocation]);
                            //100% or more - over allocated, 90% to 100% is near Max, 30% to 80% is available, < 30% is under allocated
                            if(pct >= 100)
                                childRow["Color"] = 6;
                            else if(pct >= 90 && pct < 100)
                                childRow["Color"] = 5;
                            else if(pct >= 30 && pct < 90)
                                childRow["Color"] = 4;
                            else
                                childRow["Color"] = 3;

                            if (UGITUtility.ObjectToString(childRow["WorkItem"]) == "PTO")
                                childRow["Color"] = 2;
                            
                        }
                    }

                    allocations.Rows.Add(dr);
                    ++counter;
                }

                return Ok(allocations);
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }

            return Ok("Unsuccessful");
        }

        public async Task<IHttpActionResult> GetddlLevel2Data(string SelectedItem, string TennantID)
        {
            await Task.FromResult(0);
            DataTable dtResult = new DataTable();
            try
            {
                if (SelectedItem != null && SelectedItem != "--Select--")
                {
                    ConfigurationVariableManager configVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                    DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'").Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, SelectedItem));
                    bool fromModule = drModules != null && drModules.Length > 0;
                    string moduleName = string.Empty;
                    if (fromModule)
                    {
                        moduleName = UGITUtility.ObjectToString(drModules[0][DatabaseObjects.Columns.ModuleName]);
                    }
                    DataTable resultedTable = AllocationTypeManager.LoadLevel2(HttpContext.Current.GetManagerContext(), SelectedItem, fromModule);
                    DataView dvresultedTable = resultedTable.DefaultView;

                    if (resultedTable != null)
                    {
                        bool ShowERPJobID = configVariableManager.GetValueAsBool(ConfigConstants.ShowERPJobID);
                        string ERPJobIDName = configVariableManager.GetValue(ConfigConstants.ERPJobIDName);
                        if (fromModule)
                        {
                            if (ShowERPJobID)
                            {
                                
                                    dtResult = dvresultedTable.ToTable("Selected", true, "LevelTitle", DatabaseObjects.Columns.ERPJobIDNC, DatabaseObjects.Columns.ERPJobID, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketStatus);
                                    dtResult.Columns["ERPJobIDNC"].ColumnName = "CMIC NCO #";
                                    dtResult.Columns["ERPJobID"].ColumnName = ERPJobIDName;
                                
                            }
                            else
                            {
                                dtResult = dvresultedTable.ToTable("Selected", true, "LevelTitle", DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketStatus);
                            }
                            dtResult.Columns["LevelTitle"].ColumnName = "ID";
                        }
                        else
                        {
                            DataTable destTable = new DataTable();

                            // Add columns to the destination table
                            destTable.Columns.Add("ID", typeof(long));
                            destTable.Columns.Add("LevelTitle", typeof(string));
                            destTable.Columns.Add("LevelText", typeof(string));
                            destTable.Columns.Add("RequestType", typeof(string));
                            destTable.Columns.Add("SubCategory", typeof(string));
                            DataTable dt = dvresultedTable.ToTable(true, "LevelTitle", "ID", "SubCategory", "RequestType");

                            foreach (DataRow sourceRow in dt.Rows)
                            {
                                DataRow destRow = destTable.NewRow();

                                // Copy the first three columns as-is
                                destRow["ID"] = UGITUtility.StringToLong(sourceRow["LevelTitle"]);
                                destRow["LevelTitle"] = UGITUtility.ObjectToString( sourceRow["LevelTitle"]);
                                destRow["SubCategory"] = UGITUtility.ObjectToString( sourceRow["SubCategory"]);
                                destRow["RequestType"] = UGITUtility.ObjectToString( sourceRow["RequestType"]);

                                // Concatenate the LevelTitle and LevelText columns into the new CombinedText column
                                destRow["LevelText"] = string.Format("{0} > {1}", sourceRow["LevelTitle"], sourceRow["RequestType"]);

                                // Add the new row to the destination table
                                destTable.Rows.Add(destRow);
                            }
                            return Ok(destTable);
                        }
                    }

                }
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return Ok(dtResult);
        }

        [HttpGet]
        [Route("Level2SelectionChanged")]

        public async Task<IHttpActionResult> Level2SelectionChanged(string ModuleName, string SelectedItem)
        {
            await Task.FromResult(0);
            lblDates objlblDates = new lblDates();
            if (!string.IsNullOrEmpty(ModuleName))
            {
                var context = HttpContext.Current.GetManagerContext();
                TicketManager objTicketManager = new TicketManager(context);
                Ticket ticketRequest = new Ticket(context, ModuleName);
                string ticketID = SelectedItem;
                //if (string.IsNullOrEmpty(ticketID))
                //    ticketID = WorkItem;
                if (!string.IsNullOrEmpty(ticketID) && UGITUtility.IsValidTicketID(ticketID))
                {
                    DataRow currentTicket = objTicketManager.GetTicketTableBasedOnTicketId(ModuleName, ticketID).Rows[0];
                    if (currentTicket != null)
                    {
                        string preconStart = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.PreconStartDate]), false);
                        string preconEnd = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.PreconEndDate]), false);
                        string constStart = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]), false);
                        string constEnd = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]), false);
                        
                        string closeoutStart = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]), false);
                        string closeout;
                        //If CloseoutStartDate is blank, then no need to add days to CloseoutStartDate to create the CloseoutEndDate.
                        if (string.IsNullOrEmpty(closeoutStart))
                            closeout = UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket["CloseOutDate"]), false);
                        else
                            closeout = string.IsNullOrWhiteSpace(UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket["CloseOutDate"]), false))
                                ? UGITUtility.GetDateStringInFormat(UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]).AddWorkingDays(uHelper.getCloseoutperiod(_context)).ToString(), false)
                                : UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket["CloseOutDate"]), false);

                        objlblDates.PreconStartDate = preconStart;
                        objlblDates.PreconEndDate = preconEnd;
                        objlblDates.ConstStartDate = constStart;
                        objlblDates.ConstEndDate = constEnd;
                        objlblDates.CloseoutDate = closeout;
                        objlblDates.CloseoutStartDate = closeoutStart;

                        if (!string.IsNullOrEmpty(preconStart) || !string.IsNullOrEmpty(preconEnd))
                            objlblDates.Precon = $"<b> {preconStart} to {preconEnd}</b>";
                        else
                            objlblDates.Precon = "<p>&nbsp;</p>";

                        if (!string.IsNullOrEmpty(constStart) || !string.IsNullOrEmpty(constEnd))
                            objlblDates.Const = $"<b> {constStart} to {constEnd}</b>";
                        else
                            objlblDates.Const = "<p>&nbsp;</p>";

                        if (!string.IsNullOrEmpty(closeoutStart) || !string.IsNullOrEmpty(closeout))
                            objlblDates.Closeout = $"<b> {closeoutStart} to {closeout}</b>";
                        else
                            objlblDates.Closeout = "<p>&nbsp;</p>";
                    }
                }
            }
            return Ok(objlblDates);
        }
        
        [HttpPost]
        [Route("SaveAllocationAs")]
        public async Task<IHttpActionResult> SaveAllocationAs(List<MultiAllocations> lstMultiAllocations)
        {
            try
            {
                await uHelper._semaphore.WaitAsync();
                {
                    try
                    {
                        ResourceAllocationManager objAllocationManager = new ResourceAllocationManager(_context);
                        ResourceWorkItemsManager ObjWorkItemsManager = new ResourceWorkItemsManager(_context);
                        ProjectEstimatedAllocationManager ObjEstimatedAllocationManager = new ProjectEstimatedAllocationManager(_context);
                        UserProjectExperienceManager ObjUserProjectExperienceManager = new UserProjectExperienceManager(_context);

                        foreach (MultiAllocations allocation in lstMultiAllocations)
                        {
                            // First internally check for any overlapping allocation.
                            List<MultiAllocations> newAllocations = lstMultiAllocations.Where(x => (allocation.StartDate <= x.StartDate && allocation.EndDate >= x.StartDate
                                || allocation.StartDate <= x.EndDate && allocation.EndDate >= x.EndDate
                                || allocation.StartDate <= x.StartDate && allocation.EndDate >= x.EndDate
                                || allocation.StartDate >= x.StartDate && allocation.EndDate <= x.EndDate)
                                && allocation.Role == x.Role && x.UserID == allocation.UserID
                                && allocation.WorkItem == x.WorkItem && x.key != allocation.key).ToList();
                            if (newAllocations != null && newAllocations.Count > 0)
                            {
                                return Ok($"Overlapping Allocations~{allocation.key}");
                            }

                            // For non-project allocation we need to check with ResourceAllocation table.
                            List<RResourceAllocation> existingAllocation = objAllocationManager.Load(x =>
                            (allocation.StartDate <= x.AllocationStartDate && allocation.EndDate >= x.AllocationStartDate
                                || allocation.StartDate <= x.AllocationEndDate && allocation.EndDate >= x.AllocationEndDate
                                || allocation.StartDate <= x.AllocationStartDate && allocation.EndDate >= x.AllocationEndDate
                                || allocation.StartDate >= x.AllocationStartDate && allocation.EndDate <= x.AllocationEndDate)
                                && allocation.Role == x.RoleId && x.Resource == allocation.UserID && allocation.WorkItem == x.TicketID);
                            if (existingAllocation != null && existingAllocation.Count > 0)
                            {
                                return Ok($"Overlapping Allocations~{allocation.key}");
                            }

                            // As we make entry in ResourceAllocation table in threading so some time we not get the entry from ResourceAllocation
                            // but present in ProjectEstimatedAllocation table. So added a below code to check in ProjectEstimatedAllocation table as well.
                            List<ProjectEstimatedAllocation> existingEstAllocation = ObjEstimatedAllocationManager.Load(x =>
                            (allocation.StartDate <= x.AllocationStartDate && allocation.EndDate >= x.AllocationStartDate
                                || allocation.StartDate <= x.AllocationEndDate && allocation.EndDate >= x.AllocationEndDate
                                || allocation.StartDate <= x.AllocationStartDate && allocation.EndDate >= x.AllocationEndDate
                                || allocation.StartDate >= x.AllocationStartDate && allocation.EndDate <= x.AllocationEndDate)
                                && allocation.Role == x.Type && x.AssignedTo == allocation.UserID && allocation.WorkItem == x.TicketId);
                            if (existingEstAllocation != null && existingEstAllocation.Count > 0)
                            {
                                return Ok($"Overlapping Allocations~{allocation.key}");
                            }
                        }
                        UserProfile user = null;
                        foreach (MultiAllocations allocation in lstMultiAllocations)
                        {
                            RResourceAllocation rAllocation = new RResourceAllocation();
                            rAllocation.ResourceWorkItems = new ResourceWorkItems(allocation.UserID);
                            rAllocation.ResourceWorkItems.WorkItemType = lstMultiAllocations[0].ModuleName;
                            rAllocation.ResourceWorkItems.WorkItem = Convert.ToString(allocation.WorkItem);
                            rAllocation.TicketID = UGITUtility.ObjectToString(allocation.WorkItem);
                            rAllocation.ResourceWorkItems.SubWorkItem = allocation.RoleName;
                            rAllocation.RoleId = allocation.Role;

                            rAllocation.ResourceWorkItems.StartDate = allocation.StartDate;
                            rAllocation.ResourceWorkItems.EndDate = allocation.EndDate;

                            //ObjWorkItemsManager.Insert(rAllocation.ResourceWorkItems);
                            //rAllocation.ResourceWorkItemLookup = rAllocation.ResourceWorkItems.ID;
                            rAllocation.PctAllocation = allocation.PctAllocation;
                            rAllocation.PctEstimatedAllocation = allocation.PctAllocation;
                            rAllocation.SoftAllocation = allocation.SoftAllocation;
                            rAllocation.NonChargeable = allocation.NonChargeable;

                            rAllocation.AllocationStartDate = allocation.StartDate;
                            rAllocation.AllocationEndDate = allocation.EndDate;
                            rAllocation.Resource = allocation.UserID;

                            user = _context.UserManager.LoadById(rAllocation.Resource);
                            if (user != null && (allocation.StartDate < user.UGITStartDate || allocation.EndDate > user.UGITEndDate))
                            {
                                return Ok($"Allocation Out of bounds~{allocation.key}~{user.UGITStartDate.ToShortDateString()}~{user.UGITEndDate.ToShortDateString()}~{user.Name}");
                            }

                            //Save Same Entry in Project Estimated Allocation if allocation is at project level
                            string projectid = rAllocation.ResourceWorkItems.WorkItem;
                            string workitemtype = null;
                            if (UGITUtility.IsValidTicketID(projectid))
                                workitemtype = uHelper.getModuleNameByTicketId(projectid);
                            uHelper.UpdateEstimatedAllocationTable(objAllocationManager, ObjEstimatedAllocationManager, rAllocation, workitemtype);

                            string allocationsavemsg = objAllocationManager.Save(rAllocation);                    
                            
                            string historyDesc = string.Format("Created new allocation for user: {0} - {1} {2}% {3}-{4}", user.Name, rAllocation.ResourceWorkItems.SubWorkItem, rAllocation.PctAllocation, String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationStartDate), String.Format("{0:MM/dd/yyyy}", rAllocation.AllocationEndDate));
                            ResourceAllocationManager.UpdateHistory(_context, historyDesc, rAllocation.TicketID);
                            ULog.WriteLog("MV >> " + _context.CurrentUser.Name + historyDesc);
                            user = null;
                            if (rAllocation.ResourceWorkItemLookup > 0)
                            {
                                string webUrl = HttpContext.Current.Request.Url.ToString();
                                //ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
                                long workItemID = rAllocation.ResourceWorkItemLookup;
                                //Start Thread to update rmm summary list and resourceallocation monthly w.r.t current workitem
                                ThreadStart threadStartMethod = delegate ()
                                {
                                    RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(_context, workItemID);
                                    // Add entry in Project Complexity table
                                    ObjEstimatedAllocationManager.UpdateProjectGroups(uHelper.getModuleNameByTicketId(rAllocation.TicketID), rAllocation.TicketID);
                                };
                                Thread sThread = new Thread(threadStartMethod);
                                sThread.IsBackground = true;
                                sThread.Start();
                            }

                        }

                        if (!string.IsNullOrWhiteSpace(lstMultiAllocations[0].WorkItem) && UGITUtility.IsValidTicketID(lstMultiAllocations[0].WorkItem))
                        {
                            List<string> tagLookup = ObjUserProjectExperienceManager.GetProjectExperienceTags(lstMultiAllocations[0].WorkItem, false)?.Select(x => x.TagId)?.ToList() ?? null;
                            ObjUserProjectExperienceManager.UpdateUserProjectTagExperience(tagLookup, lstMultiAllocations[0].WorkItem);
                        }

                        return Ok("Record updated successfully");
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException("SaveAllocationAs_semaphore Lock:" + ex.ToString());
                        return Ok("Error occured");
                    }
                }
            }
            catch (Exception e)
            {
                ULog.WriteException("SaveAllocationAs_semaphore Lock:" + e.ToString());
                return Ok("SaveAllocationAs_semaphore Lock:" + e.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
        }


        [HttpGet]
        [Route("GetAllocationByWorkitem")]
        public async Task<IHttpActionResult> GetAllocationByWorkitem(string AllocationID)
        {
            await Task.FromResult(0);
            RResourceAllocation resourceAllocationObj = resourceAllocationMGR.LoadByID(UGITUtility.StringToLong(AllocationID));   //.Load(x=>x.ResourceWorkItemLookup == UGITUtility.StringToLong(AllocationID)).FirstOrDefault();
            if (resourceAllocationObj != null)
            {
                ProjectEstimatedAllocation projectEstimatedAllocationObj = projectEstimatedAllocationMGR.LoadByID(UGITUtility.StringToLong(resourceAllocationObj.ProjectEstimatedAllocationId));
                if (projectEstimatedAllocationObj != null)
                {
                    return Ok(projectEstimatedAllocationObj);
                }
                else
                    return Ok("Estimated Allocaiton Not Found");
            }
            else
                return Ok("Workitem Not Found");;
        }

        [Route("GetAllocationByWorkitemID")]
        public async Task<IHttpActionResult> GetAllocationByWorkitemID(string WorkitemID)
        {
            await Task.FromResult(0);
            List<RResourceAllocation> resourceAllocationObj = resourceAllocationMGR.Load(x => x.ResourceWorkItemLookup == UGITUtility.StringToLong(WorkitemID));   
            if (resourceAllocationObj != null)
            {
                List<string> estimatedIDs = resourceAllocationObj.Select(x => x.ProjectEstimatedAllocationId).ToList();
                List<ProjectEstimatedAllocation> projectEstimatedAllocationLst = projectEstimatedAllocationMGR.Load(x=> estimatedIDs.Contains(UGITUtility.ObjectToString(x.ID))).OrderBy(a => a.AllocationStartDate).ToList();
                if (projectEstimatedAllocationLst != null)
                {
                    return Ok(projectEstimatedAllocationLst);
                }
                else
                    return Ok("Estimated Allocaiton Not Found");
            }
            else
                return Ok("Workitem Not Found"); ;
        }

        [HttpGet]
        [Route("GetAllocationByID")]
        public async Task<IHttpActionResult> GetAllocationByID(string AllocationID)
        {
            await Task.FromResult(0);
            RResourceAllocation resourceAllocationObj = resourceAllocationMGR.LoadByID(UGITUtility.StringToLong(AllocationID));
            if (resourceAllocationObj != null)
            {
                ProjectEstimatedAllocation projectEstimatedAllocationObj = projectEstimatedAllocationMGR.LoadByID(UGITUtility.StringToLong(resourceAllocationObj.ProjectEstimatedAllocationId));
                if (projectEstimatedAllocationObj != null)
                {
                    return Ok(projectEstimatedAllocationObj);
                }
                else
                    return Ok("Estimated Allocaiton Not Found");
            }
            else
                return Ok("Workitem Not Found"); ;
        }

        [HttpGet]
        [Route("GetCorruptedAllocations")]
        public async Task<IHttpActionResult> GetCorruptedAllocations(string tabname, bool IncludedClosed)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            DataSet result = RMMSummaryHelper.GetCorruptAllocations(context, tabname, IncludedClosed);
            if (result != null && result.Tables.Count > 0)
            {
                return Ok(result);
            }
            return Ok();
        }

        [HttpGet]
        [Route("AddLogInfo")]
        public async Task<IHttpActionResult> AddLogInfo(string message)
        {
            await Task.FromResult(0);
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ULog.WriteLog(context.CurrentUser.Name + " >> " + message);
            return Ok();
        }
        [HttpPost]
        [Route("DeleteAllocation_New")]
        public async Task<IHttpActionResult> DeleteAllocation_New(List<AllocationDeleteModel> lstModel)
        {
            await Task.FromResult(0);
            try
            {
                await uHelper._semaphore.WaitAsync();
                {
                     await projectEstimatedAllocationMGR.DeleteEstimatedAllocationUsingSP(lstModel);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                ULog.WriteException("DeleteAllocations_New :" + e.ToString());
                return Ok("DeleteAllocations_New :" + e.ToString());
            }
            finally
            {
                uHelper._semaphore.Release();
            }
        }


    }

    public class AllocationsResponse
    {
        public int totalAlloc { get; set; }
        public int totalAllocThisWeek { get; set; }
        public int totalAllocin3Weeks { get; set; }
    }

    public class TeamTabDataModel
    {
        public List<AllocationTemplateModel> Allocations { get; set; }
        public List<GlobalRole> Roles { get; set; }
        public List<UserProfile> UserProfiles { get; set; }
        public List<string> JobTitles { get; set; }
    }
    public class ProjectAllocationDetail
    {
        public List<AllocationTemplateModel> Allocations { get; set; }
        public string PreconStartDate { get; set; }
        public string PreconEndDate { get; set; }
        public string ConstStartDate { get; set; }
        public string ConstEndDate { get; set; }
        public string CloseOutStartDate { get; set; }
        public string CloseOutEndDate { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectID { get; set; }
        public string FilledAllocationCount { get; set; }
        public string TotalAllocationCount { get; set; }
        public string TitleLink { get; set; }
    }

    public class AllocationDatesModel
    {
        public List<AllocationTemplateModel> Allocations { get; set; }
        public TimeFrame TimeFrame { get; set; }
        public int Duration { get; set; }
        public bool UpdatePhaseDates { get; set; }
        public UpdateType UType { get; set; }
        public string PreconStartDate { get; set; }
        public string PreconEndDate { get; set; }
        public string ConstStartDate { get; set; }
        public string ConstEndDate { get; set; }
        public string CloseOutStartDate { get; set; }
        public string CloseOutEndDate { get; set; }
    }



    public class lblDates
    {
        public string Precon { get; set; }
        public string Const { get; set; }
        public string Closeout { get; set; }
        public string PreconStartDate { get; set; }
        public string PreconEndDate { get; set; }
        public string ConstStartDate { get; set; }
        public string ConstEndDate { get; set; }
        public string CloseoutStartDate { get; set; }
        public string CloseoutDate { get; set; }

    }

    public class MultiAllocations
    {
        public int key { get; set; }
        public string WorkItemType { get; set; }
        public string WorkItem { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public double? PctAllocation { get; set; }
        public bool SoftAllocation { get; set; }
        public string ModuleName { get; set; }
        public string UserID { get; set; }
        public bool NonChargeable { get; set; }
    }

    public class TemplateModel
    {
        public string TemplateName { get; set; }
        public string TicketID { get; set; }
        public string ModuleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public String Templates {get; set;}
    }
}
