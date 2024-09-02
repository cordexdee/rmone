using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Web.Models;
using Constants = uGovernIT.Utility.Constants;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/timesheet")]
    public class TimesheetController : ApiController
    {
        private ApplicationContext _applicationContext;
        private ModuleViewManager _moduleViewManager = null;
        private TicketManager _ticketManager = null;
        private UserProfileManager _profileManager = null;
        ResourceTimeSheetSignOffManager resourceTimeSheetSignOffManager = null;

        public TimesheetController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            _moduleViewManager = new ModuleViewManager(_applicationContext);
            _ticketManager = new TicketManager(_applicationContext);
            _profileManager = new UserProfileManager(_applicationContext);
            resourceTimeSheetSignOffManager = new ResourceTimeSheetSignOffManager(_applicationContext);
        }

        [HttpGet]
        [Authorize]
        [Route("GetTimesheet/user-name/{user_name}/start-date/{start_date}/end-date/{end_date}")]
        public async Task<IHttpActionResult> GetTimesheet(string user_name, string start_date, string end_date)
        {
            await Task.FromResult(0);
            try
            {
                var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

                var User = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(user_name));
                var IsAuthorisedUser = UserManager.IsAdmin(User) || (_applicationContext.CurrentUser.UserName.EqualsIgnoreCase(user_name)) || (_applicationContext.CurrentUser.Id.EqualsIgnoreCase(User.ManagerID));

                if (!IsAuthorisedUser)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "User not authorized to view timesheet" });

                //TicketHoursManager thManager = new TicketHoursManager(_applicationContext);
                //ProjectStandardWorkItemManager projectStandardWorkItemManager = new ProjectStandardWorkItemManager(_applicationContext);
                //List<ProjectStandardWorkItem> projectStandardWorkItems = projectStandardWorkItemManager.Load();
                //ProjectStandardWorkItem standardWorkItem = null;

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;

                
                if (User == null)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "invalid UseName" });

                if (!DateTime.TryParse(start_date, out startDate))
                {
                    return Ok(new ErrorResponse { error_code = "1001", error_category = "parameter", error_message = "invalid start date" });
                }

                if (!DateTime.TryParse(end_date, out endDate))
                {
                    return Ok(new ErrorResponse { error_code = "1002", error_category = "parameter", error_message = "invalid end date" });
                }

                if (startDate > endDate)
                {
                    return Ok(new ErrorResponse { error_code = "1003", error_category = "parameter", error_message = "end date cannot be earlier than start date" });
                }

                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("UserId", User.Id);
                arrParams.Add("StartDate", startDate);
                arrParams.Add("EndDate", endDate);
                arrParams.Add("WorkItem", string.Empty);
                DataTable dtTimesheet = uGITDAL.ExecuteDataSetWithParameters("GetTimesheet", arrParams);

                ResourceTimesheetModel resourceTimesheet = new ResourceTimesheetModel();
                resourceTimesheet.userName = user_name;
                resourceTimesheet.timesheet = new List<Timesheet>();
                List<Timesheet> timesheets = new List<Timesheet>();
                foreach (DataRow item in dtTimesheet.Rows)
                {
                    Timesheet timesheet = new Timesheet();
                    timesheet.date = Convert.ToString(item["WorkDate"]);
                    timesheet.day = Convert.ToString(item["Day"]);
                    timesheet.hours = Convert.ToString(item["HoursTaken"]); 
                    timesheet.jobCode = Convert.ToString(item["Code"]);
                    timesheet.note = Convert.ToString(item["Comment"]); 
                    timesheet.workItem = Convert.ToString(item["WorkItem"]); 
                    timesheet.role = Convert.ToString(item["SubWorkItem"]); 
                    timesheet.type = Convert.ToString(item["WorkItemType"]); 
                    timesheet.Title = Convert.ToString(item["Title"]);
                    timesheet.status = "";

                    timesheets.Add(timesheet);
                }


                /*
                List<ActualHour> actualHours = thManager.Load(x => x.Resource.EqualsIgnoreCase(User.Id) && x.WorkDate >= startDate && x.WorkDate <= endDate && x.Deleted == false).OrderBy(x => x.WorkDate).ToList();

                ResourceTimesheetModel resourceTimesheet = new ResourceTimesheetModel();
                resourceTimesheet.userName = user_name;
                resourceTimesheet.timesheet = new List<Timesheet>();
                List<Timesheet> timesheets = new List<Timesheet>();

                foreach (var item in actualHours)
                {
                    Timesheet timesheet = new Timesheet();
                    timesheet.date = item.WorkDate.ToShortDateString();
                    timesheet.day = item.WorkDate.DayOfWeek.ToString();
                    timesheet.hours = Convert.ToString(item.HoursTaken);

                    standardWorkItem = projectStandardWorkItems.FirstOrDefault(x => x.ID == item.TaskID);
                    if (standardWorkItem != null)
                        timesheet.jobCode = standardWorkItem.Code;
                    else
                        timesheet.jobCode = string.Empty;

                    timesheet.note = item.Comment;
                    timesheet.workItem = item.WorkItem;
                    timesheet.role = item.SubWorkItem;
                    timesheet.type = "";
                    timesheet.status = "";

                    timesheets.Add(timesheet);
                }

                
                foreach (var workdate in actualHours.Select(x => x.WorkDate).Distinct().ToList())
                {
                    Timesheet timesheet = new Timesheet();
                    timesheet.date = workdate.ToShortDateString();
                    timesheet.day = workdate.DayOfWeek.ToString();
                    timesheet.entries = new List<Entry>();
                    List<Entry> entries = new List<Entry>();
                    var data = actualHours.Where(x => x.WorkDate == workdate).ToList();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            Entry entry = new Entry();
                            entry.hours = Convert.ToString(item.HoursTaken);

                            standardWorkItem = projectStandardWorkItems.FirstOrDefault(x => x.ID == item.TaskID);
                            if (standardWorkItem != null)
                                entry.jobCode = standardWorkItem.Code;
                            else
                                entry.jobCode = string.Empty;

                            entry.note = item.Comment;
                            entry.workItem = item.WorkItem;
                            entry.role = item.SubWorkItem;
                            entry.type = "";
                            entry.status = "";

                            entries.Add(entry);
                        }
                        timesheet.entries.AddRange(entries);
                    }
                    timesheets.Add(timesheet);
                }
                */

                resourceTimesheet.timesheet.AddRange(timesheets);
                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = resourceTimesheet });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTimesheet: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetWeeksTimesheet/user-name/{user_name}/workitem/{work_item}/date/{date}")]
        public async Task<IHttpActionResult> GetWeeksTimesheet(string user_name, string work_item, string date)
        {
            await Task.FromResult(0);
            try
            {
                var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

                var User = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(user_name));
                var IsAuthorisedUser = UserManager.IsAdmin(User) || (_applicationContext.CurrentUser.UserName.EqualsIgnoreCase(user_name)) || (_applicationContext.CurrentUser.Id.EqualsIgnoreCase(User.ManagerID));

                if (!IsAuthorisedUser)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "User not authorized to view timesheet" });

                //TicketHoursManager thManager = new TicketHoursManager(_applicationContext);

                if (User == null)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "invalid UseName" });

                ResourceTimesheetModel resourceTimesheet = GetWeeksTimesheetDetails(User, work_item, date, "");

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = resourceTimesheet });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWeeksTimesheet: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("GetWeeksTimesheet/user-name/{user_name}/workitem/{work_item}/date/{date}/subworkitem/{subwork_item}")]
        public async Task<IHttpActionResult> GetWeeksTimesheet(string user_name, string work_item, string date, string subwork_item = "")
        {
            try
            {
                await Task.FromResult(0);
                var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

                var User = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(user_name));
                var IsAuthorisedUser = UserManager.IsAdmin(User) || (_applicationContext.CurrentUser.UserName.EqualsIgnoreCase(user_name)) || (_applicationContext.CurrentUser.Id.EqualsIgnoreCase(User.ManagerID));

                if (!IsAuthorisedUser)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "User not authorized to view timesheet" });

                //TicketHoursManager thManager = new TicketHoursManager(_applicationContext);

                if (User == null)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "invalid UseName" });

                ResourceTimesheetModel resourceTimesheet = GetWeeksTimesheetDetails(User, work_item, date, subwork_item);
                
                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = resourceTimesheet });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWeeksTimesheet: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        private ResourceTimesheetModel GetWeeksTimesheetDetails(UserProfile user, string work_item, string date, string subwork_item = "")
        {
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("UserId", user.Id);
                arrParams.Add("Date", date);
                arrParams.Add("WorkItem", work_item);
                arrParams.Add("SubWorkItem", subwork_item);
                DataTable dtTimesheet = uGITDAL.ExecuteDataSetWithParameters("GetWeeksTimesheet", arrParams);

                ResourceTimeSheetSignOff obj = resourceTimeSheetSignOffManager.Load(x => x.Resource.EqualsIgnoreCase(user.Id) && x.StartDate == Convert.ToDateTime(date) && x.Deleted == false).FirstOrDefault();

                ResourceTimesheetModel resourceTimesheet = new ResourceTimesheetModel();
                resourceTimesheet.userName = user.UserName;

                if (obj != null)
                    resourceTimesheet.SignOffStatus = obj.SignOffStatus;
                else
                    resourceTimesheet.SignOffStatus = Constants.TimeEntry;

                resourceTimesheet.timesheet = new List<Timesheet>();
                List<Timesheet> timesheets = new List<Timesheet>();
                foreach (DataRow item in dtTimesheet.Rows)
                {
                    Timesheet timesheet = new Timesheet();
                    timesheet.date = Convert.ToString(item["WorkDate"]);
                    timesheet.day = Convert.ToString(item["Day"]);
                    timesheet.hours = Convert.ToString(item["HoursTaken"]);
                    timesheet.jobCode = Convert.ToString(item["Code"]);
                    timesheet.note = Convert.ToString(item["Comment"]);
                    timesheet.workItem = Convert.ToString(item["WorkItem"]);
                    timesheet.role = Convert.ToString(item["SubWorkItem"]);
                    timesheet.type = Convert.ToString(item["WorkItemType"]);
                    timesheet.Title = Convert.ToString(item["Title"]);
                    timesheet.status = "";

                    timesheets.Add(timesheet);
                }

                resourceTimesheet.timesheet.AddRange(timesheets);

                return resourceTimesheet;

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetWeeksTimesheetDetails: " + ex);
                return null;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetConsolidatedTimesheet/user-name/{user_name}/start-date/{start_date}/end-date/{end_date}")]
        public async Task<IHttpActionResult> GetConsolidatedTimesheet(string user_name, string start_date, string end_date)
        {
            await Task.FromResult(0);
            try
            {
                var UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

                var User = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(user_name));
                var IsAuthorisedUser = UserManager.IsAdmin(User) || (_applicationContext.CurrentUser.UserName.EqualsIgnoreCase(user_name)) || (_applicationContext.CurrentUser.Id.EqualsIgnoreCase(User.ManagerID));

                if (!IsAuthorisedUser)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "User not authorized to view timesheet" });

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;


                if (User == null)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "invalid UseName" });

                if (!DateTime.TryParse(start_date, out startDate))
                {
                    return Ok(new ErrorResponse { error_code = "1001", error_category = "parameter", error_message = "invalid start date" });
                }

                if (!DateTime.TryParse(end_date, out endDate))
                {
                    return Ok(new ErrorResponse { error_code = "1002", error_category = "parameter", error_message = "invalid end date" });
                }

                if (startDate > endDate)
                {
                    return Ok(new ErrorResponse { error_code = "1003", error_category = "parameter", error_message = "end date cannot be earlier than start date" });
                }

                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("UserId", User.Id);
                arrParams.Add("StartDate", startDate);
                arrParams.Add("EndDate", endDate);
                arrParams.Add("WorkItem", string.Empty);
                DataTable dtTimesheet = uGITDAL.ExecuteDataSetWithParameters("GetConsolidatedTimesheet", arrParams);
                ResourceTimeSheetSignOff obj = resourceTimeSheetSignOffManager.Load(x => x.Resource.EqualsIgnoreCase(User.Id) && x.StartDate == Convert.ToDateTime(startDate) && x.Deleted == false).FirstOrDefault();

                ResourceTimesheetModel resourceTimesheet = new ResourceTimesheetModel();
                resourceTimesheet.userName = user_name;

                if (obj != null)
                    resourceTimesheet.SignOffStatus = obj.SignOffStatus;
                else
                    resourceTimesheet.SignOffStatus = Constants.TimeEntry;

                resourceTimesheet.timesheet = new List<Timesheet>();
                List<Timesheet> timesheets = new List<Timesheet>();
                foreach (DataRow item in dtTimesheet.Rows)
                {
                    Timesheet timesheet = new Timesheet();
                    timesheet.date = Convert.ToString(item["WorkDate"]);
                    //timesheet.day = Convert.ToString(item["Day"]);
                    timesheet.hours = Convert.ToString(item["TotalHours"]);
                    timesheet.Title = Convert.ToString(item["Title"]);
                    timesheet.jobCode = Convert.ToString(item["Code"]);
                    //timesheet.note = Convert.ToString(item["Comment"]);
                    timesheet.workItem = Convert.ToString(item["WorkItem"]);
                    timesheet.role = Convert.ToString(item["SubWorkItem"]);
                    timesheet.type = Convert.ToString(item["WorkItemType"]);
                    timesheet.status = "";

                    timesheets.Add(timesheet);
                }

                resourceTimesheet.timesheet.AddRange(timesheets);
                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = resourceTimesheet });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetOwinContext: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("Add")]
        public async Task<IHttpActionResult> Add([FromBody] ResourceTimesheetModel obj)
        {
            await Task.FromResult(0);
            if (obj == null)
            {
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { "No input data." } });
            }

            ResourceTimeSheetManager rtsManager = new ResourceTimeSheetManager(_applicationContext);
            ResourceWorkItemsManager rwiManager = new ResourceWorkItemsManager(_applicationContext);
            TicketHoursManager thManager = new TicketHoursManager(_applicationContext);
            ProjectStandardWorkItemManager projStdWorkItemMgr = new ProjectStandardWorkItemManager(_applicationContext);
            List<string> invalidInputRecords = new List<string>();
            ActualHour actualHour = new ActualHour();
            ResourceWorkItems resourceWorkItems = new ResourceWorkItems();
            ResourceTimeSheet rTimesheet = new ResourceTimeSheet();

            try
            {
                ResourceTimesheetModel resourceTimesheet = obj;
                var User = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(resourceTimesheet.userName));
                if (User == null)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "invalid UseName" });

                var projStdWorkItems = projStdWorkItemMgr.Load(x => x.Deleted == false).Select(x => new { x.ID, Title = x.Title, Code = x.Code }).ToList();

                long projStdWorkItemsId = 0;
                string jobCode = string.Empty;

                foreach (var timesheet in obj.timesheet)
                {
                    //foreach (var item in timesheet.entries)
                    //{
                        var projStd = projStdWorkItems.FirstOrDefault(x => x.Code == timesheet.jobCode);

                        if (projStd != null)
                        {
                            projStdWorkItemsId = projStd.ID;
                            jobCode = $"{projStd.Title};#{projStd.Code}";
                        }
                        else
                        {
                            projStdWorkItemsId = 0;
                            jobCode = string.Empty;
                        }

                        actualHour = thManager.Load(x => x.Resource.EqualsIgnoreCase(User.Id) && x.WorkItem == timesheet.workItem && x.SubWorkItem.EqualsIgnoreCase(timesheet.role) && x.WorkDate == Convert.ToDateTime(timesheet.date) && x.TaskID == projStdWorkItemsId && x.StandardWorkItem == true && x.Deleted == false).FirstOrDefault();
                        if (actualHour != null && actualHour.ID > 0)
                        {
                            actualHour.HoursTaken = Convert.ToDouble(timesheet.hours);
                            actualHour.Comment = timesheet.note;
                            thManager.Update(actualHour);
                        }
                        else
                        {
                            //invalidInputRecords.Add($"User: {User.UserName}, WorkItem: {item.workItem}, SubWorkItem: {item.role}, WorkDate: {timesheet.date}, Jobcode: {item.jobCode}\n");
                            actualHour = new ActualHour();
                            actualHour.TicketID = timesheet.workItem;
                            actualHour.StageStep = 0;
                            actualHour.ModuleNameLookup = uHelper.getModuleNameByTicketId(timesheet.workItem);
                            actualHour.Resource = User.Id;
                            actualHour.WorkItem = timesheet.workItem;
                            actualHour.SubWorkItem = timesheet.role;
                            actualHour.WorkDate = Convert.ToDateTime(timesheet.date);
                            actualHour.TaskID = projStdWorkItemsId;
                            actualHour.HoursTaken = Convert.ToDouble(timesheet.hours);
                            actualHour.Comment = timesheet.note;
                            actualHour.StandardWorkItem = true;
                            actualHour.Deleted = false;
                            //Update week & month start date from WorkDate 
                            if (actualHour.WorkDate != DateTime.MinValue && actualHour.WorkDate != DateTime.MaxValue)
                            {
                                DateTime weekSDate = uHelper.GetWeekStartDate(actualHour.WorkDate);//Week start day will be monday
                                actualHour.WeekStartDate = weekSDate.Date;
                                actualHour.MonthStartDate = uHelper.GetMonthStartDate(actualHour.WorkDate).Date;
                            }
                            thManager.Insert(actualHour);

                            resourceWorkItems = rwiManager.Load(x => x.Resource.EqualsIgnoreCase(User.Id) && x.WorkItem == timesheet.workItem && x.SubWorkItem.EqualsIgnoreCase(timesheet.role) && x.StartDate == actualHour.WeekStartDate && x.EndDate == actualHour.WeekStartDate.AddDays(6) && x.SubSubWorkItem == jobCode && x.Deleted == false).FirstOrDefault();
                            if (resourceWorkItems == null)
                            {
                                resourceWorkItems = new ResourceWorkItems();
                                resourceWorkItems.Resource = User.Id;
                                resourceWorkItems.WorkItem = timesheet.workItem;
                                resourceWorkItems.WorkItemType = uHelper.getModuleNameByTicketId(timesheet.workItem);
                                resourceWorkItems.SubWorkItem = timesheet.role;
                                resourceWorkItems.SubSubWorkItem = jobCode;
                                resourceWorkItems.StartDate = actualHour.WeekStartDate;
                                resourceWorkItems.EndDate = actualHour.WeekStartDate.AddDays(6);

                                rwiManager.Insert(resourceWorkItems);

                                rTimesheet = new ResourceTimeSheet();
                                rTimesheet.ResourceWorkItemLookup = resourceWorkItems.ID;
                                rTimesheet.HoursTaken = 0;
                                rTimesheet.Resource = User.Id;
                                rTimesheet.WorkDate = Convert.ToDateTime(timesheet.date);
                                rtsManager.Insert(rTimesheet);
                            }
                        }
                    //}
                }
                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" } });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in Add: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Update")]
        public async Task<IHttpActionResult> Update([FromBody] ResourceTimesheetModel obj)
        {
            await Task.FromResult(0);
            if (obj == null)
            {
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { "No input data." } });
            }

            TicketHoursManager thManager = new TicketHoursManager(_applicationContext);
            ProjectStandardWorkItemManager projStdWorkItemMgr = new ProjectStandardWorkItemManager(_applicationContext);
            ResourceWorkItemsManager rwiManager = new ResourceWorkItemsManager(_applicationContext);
            ResourceTimeSheetManager rtsManager = new ResourceTimeSheetManager(_applicationContext);

            List<string> invalidInputRecords = new List<string>();
            ActualHour actualHour = new ActualHour();
            try
            {
                ResourceTimesheetModel resourceTimesheet = obj;
                var User = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(resourceTimesheet.userName));
                if (User == null)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "invalid UseName" });

                var projStdWorkItems = projStdWorkItemMgr.Load(x => x.Deleted == false).Select(x => new { x.ID, Title = x.Title, Code = x.Code }).ToList();

                long projStdWorkItemsId = 0;

                foreach (var timesheet in obj.timesheet)
                {
                    //foreach (var item in timesheet.entries)
                    //{
                        var projStd = projStdWorkItems.FirstOrDefault(x => x.Code == timesheet.jobCode);

                        if (projStd != null)
                            projStdWorkItemsId = projStd.ID;
                        else
                            projStdWorkItemsId = 0;

                        actualHour = thManager.Load(x => x.Resource.EqualsIgnoreCase(User.Id) && x.WorkItem == timesheet.workItem && x.SubWorkItem.EqualsIgnoreCase(timesheet.role) && x.WorkDate == Convert.ToDateTime(timesheet.date) && x.TaskID == projStdWorkItemsId && x.StandardWorkItem == true && x.Deleted == false).FirstOrDefault();
                        if (actualHour != null && actualHour.ID > 0)
                        {
                            actualHour.HoursTaken = Convert.ToDouble(timesheet.hours);
                            actualHour.Comment = timesheet.note;
                            thManager.Update(actualHour);
                        }
                        else
                        {
                            ResourceWorkItems resourceWorkItems = rwiManager.Load(x => x.WorkItem == timesheet.workItem && x.WorkItemType == timesheet.type && x.Resource.EqualsIgnoreCase(User.Id)).FirstOrDefault();
                            if (resourceWorkItems != null)
                            {
                                ResourceTimeSheet resourceTimeSheet = rtsManager.Load(x => x.ResourceWorkItemLookup == resourceWorkItems.ID && x.WorkDate == Convert.ToDateTime(timesheet.date) && x.Resource.EqualsIgnoreCase(User.Id)).FirstOrDefault();
                                if (resourceTimeSheet != null)
                                {
                                    resourceTimeSheet.HoursTaken = Convert.ToInt32(timesheet.hours);
                                    resourceTimeSheet.WorkDescription = timesheet.note;
                                    rtsManager.Update(resourceTimeSheet);
                                }
                                else
                                {
                                    resourceTimeSheet = new ResourceTimeSheet();
                                    resourceTimeSheet.ResourceWorkItemLookup = resourceWorkItems.ID;
                                    resourceTimeSheet.WorkDate = Convert.ToDateTime(timesheet.date);
                                    resourceTimeSheet.Resource = User.Id;
                                    resourceTimeSheet.HoursTaken = Convert.ToInt32(timesheet.hours);
                                    resourceTimeSheet.WorkDescription = timesheet.note;
                                    rtsManager.Insert(resourceTimeSheet);
                                }
                            }    

                            //invalidInputRecords.Add($"User: {User.UserName}, WorkItem: {timesheet.workItem}, SubWorkItem: {timesheet.role}, WorkDate: {timesheet.date}, Jobcode: {timesheet.jobCode}\n");
                        }
                    //} 
                }

                if (invalidInputRecords.Count > 0)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "input data", error_message = "invalid records", errors = invalidInputRecords });
                else
                    return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" } });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in Update: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("AddWorkItem")]
        public async Task<IHttpActionResult> AddWorkItem([FromBody] WorkItems obj)
        {
            await Task.FromResult(0);
            if (obj == null)
            {
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { "No input data." } });
            }

            try
            {

                WorkItems workItems = obj;

                var User = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(workItems.userName));
                if (User == null)
                    return Ok(new ErrorResponse { error_code = "1000", error_category = "parameter", error_message = "invalid UseName" });

                ProjectStandardWorkItemManager projStdWorkItemMgr = new ProjectStandardWorkItemManager(_applicationContext);
                ResourceWorkItemsManager ObjWorkItemManager = new ResourceWorkItemsManager(_applicationContext);
                ResourceWorkItems workItem = null;
                List<string> errMsg = new List<string>();
                long messageType = 0, projStdWorkItemsId = 0;
                string jobCode = string.Empty;

                var projStdWorkItems = projStdWorkItemMgr.Load(x => x.Deleted == false).Select(x => new { x.ID, Title = x.Title, Code = x.Code }).ToList();

                foreach (var item in workItems.workitems)
                {
                    var projStd = projStdWorkItems.FirstOrDefault(x => x.Code == item.jobCode);

                    if (projStd != null)
                    {
                        projStdWorkItemsId = projStd.ID;
                        jobCode = $"{projStd.Title};#{projStd.Code}";
                    }
                    else
                    {
                        projStdWorkItemsId = 0;
                        jobCode = string.Empty;
                    }

                    workItem = new ResourceWorkItems();

                    workItem.Resource = User.Id;
                    workItem.WorkItem = item.workItem;
                    workItem.WorkItemType = uHelper.getModuleNameByTicketId(item.workItem);
                    workItem.SubWorkItem = item.role;
                    workItem.SubSubWorkItem = jobCode;

                    workItem.StartDate = uHelper.GetWeekStartDate(Convert.ToDateTime(item.startDate));  

                    workItem.EndDate = Convert.ToDateTime(workItem.StartDate).AddDays(6);

                    /*
                    workItem.StartDate = Convert.ToDateTime(item.startDate); //StartDate;
                    workItem.EndDate = Convert.ToDateTime(item.startDate).AddDays(6);
                    */

                    /*
                     DateTime wkStartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                     DateTime wkEndDate = wkStartDate.AddDays(7).AddSeconds(-1);
                     */

                    messageType = ObjWorkItemManager.SaveResourceWorkItems(workItem);
                    if (messageType == 2)
                        errMsg.Add($"WorkItem {item.workItem}; {workItem.SubWorkItem}; {workItem.SubSubWorkItem } already exists\n");
                }

                if (errMsg.Count > 0)
                    return Ok(new CommonTicketResponse { Status = false, ErrorMessages = errMsg });

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" } });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in AddWorkItem: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("AllWorkItems/{WorkItemType}")]
        public async Task<IHttpActionResult> AllWorkItems(string WorkItemType)
        {
            await Task.FromResult(0);
            try
            {
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("WorkItemType", WorkItemType);
                arrParams.Add("WorkItemCategory", "all");
                DataTable dtResult = uGITDAL.ExecuteDataSetWithParameters("GetProjectWorkItems", arrParams);
                if (dtResult.Rows.Count > 0)
                {
                    return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = dtResult });
                }
                else
                {
                    dtResult = AllocationTypeManager.LoadLevel2(_applicationContext, WorkItemType, false);
                    return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = dtResult });
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in AllWorkItems: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("CurrentWorkItems/{WorkItemType}")]
        public async Task<IHttpActionResult> CurrentWorkItems(string WorkItemType)
        {
            await Task.FromResult(0);
            try
            {
                //var loggedinUser = _profileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(_applicationContext.CurrentUser.Id));
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("WorkItemType", WorkItemType);
                arrParams.Add("WorkItemCategory", "current");
                DataTable dtResult = uGITDAL.ExecuteDataSetWithParameters("GetProjectWorkItems", arrParams);
                if (dtResult.Rows.Count > 0)
                {
                    return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = dtResult });
                }
                else
                {
                    dtResult = AllocationTypeManager.LoadLevel2(_applicationContext, WorkItemType, false);
                    return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = dtResult });
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CurrentWorkItems: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Employees")]
        public async Task<IHttpActionResult> Employees()
        {
            await Task.FromResult(0);
            try
            {
                var loggedinUser = _profileManager.GetUsersProfile();
                var Users = _profileManager.GetUsersProfile().Select(x => new { x.UserName, x.Name, x.Picture, x.IsManager, x.Email, x.Id }).OrderBy(x => x.Name).ToList();
                return Ok(new EmployeesResponse { Status = true, ErrorMessages = new List<string>() { "" }, Employees = Users });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in Employees: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetSubordinates")]
        public async Task<IHttpActionResult> GetSubordinates()
        {
            await Task.FromResult(0);
            try
            {
                var loggedinUser = _profileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(_applicationContext.CurrentUser.Id));
                if (loggedinUser != null && !loggedinUser.IsManager)
                {
                    return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { "Loggedin User is not a Manager" }, Employees = null });
                }
                var Users = _profileManager.GetUsersProfile().Where(x => x.ManagerID.EqualsIgnoreCase(loggedinUser.Id)).Select(x => new { x.UserName, x.Name, x.Picture, x.IsManager, x.Email, x.Id }).OrderBy(x => x.Name).ToList();

                return Ok(new EmployeesResponse { Status = true, ErrorMessages = new List<string>() { "" }, Employees = Users });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetSubordinates: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Login")]
        public async Task<IHttpActionResult> Login()
        {
            await Task.FromResult(0);
            try
            {
                var loggedinUser = _profileManager.GetUsersProfile().FirstOrDefault(x => x.Id.EqualsIgnoreCase(_applicationContext.CurrentUser.Id));
                if (loggedinUser == null)
                {
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username or password" }, User = null });
                }

                if (loggedinUser != null && loggedinUser.Enabled == false)
                {
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Inactive User" }, User = null });
                }
                var user = new { loggedinUser.Id, loggedinUser.UserName, loggedinUser.Name, loggedinUser.IsManager, loggedinUser.Email, loggedinUser.Picture };
                return Ok(new UserResponse { Status = true, ErrorMessages = new List<string>() { "" }, User = user });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in Login: " + ex);
                return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserProfile")]
        public async Task<IHttpActionResult> GetUserProfile(string userName)
        {
            await Task.FromResult(0);
            try
            {
                if (String.IsNullOrEmpty(userName))
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Username required" }, User = null });

                UserProfile UserInfo = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(userName));
                if (UserInfo == null)
                {
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username or password" }, User = null });
                }

                if (UserInfo != null && UserInfo.Enabled == false)
                {
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Inactive User" }, User = null });
                }
                var user = new { UserInfo.Id, UserInfo.UserName, UserInfo.Name, UserInfo.IsManager, UserInfo.Email, UserInfo.Picture, UserInfo.NotificationEmail,
                    UserInfo.PhoneNumber, UserInfo.EmployeeId, UserInfo.EmployeeType,
                    UserInfo.HourlyRate,
                    UserInfo.MobilePhone,
                    UserInfo.IsIT,
                    UserInfo.IsConsultant,
                    UserInfo.Enabled,
                    UserInfo.DeskLocation,
                    UserInfo.EnablePasswordExpiration,
                    UserInfo.PasswordExpiryDate,
                    UserInfo.DisableWorkflowNotifications,
                    UserInfo.LeaveFromDate,
                    UserInfo.LeaveToDate,
                    UserInfo.EnableOutofOffice,
                    UserInfo.WorkingHoursStart,
                    UserInfo.WorkingHoursEnd,
                    UserInfo.Resume
                };
                return Ok(new UserResponse { Status = true, ErrorMessages = new List<string>() { "" }, User = user });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserProfile: " + ex);
                return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("UpdateUserProfile")]
        public async Task<IHttpActionResult> UpdateUserProfile([FromBody] UserProfile obj)
        {
            await Task.FromResult(0);
            try
            {
                if (obj == null)
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Invalid User details" }, User = null });

                UserProfile UserInfo = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(obj.UserName));
                if (UserInfo == null)
                {
                    return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username" }, User = null });
                }

                if (!string.IsNullOrEmpty(obj.Name)) UserInfo.Name = obj.Name;
                if (obj.IsManager != UserInfo.IsManager) UserInfo.IsManager = obj.IsManager;
                if (!string.IsNullOrEmpty(obj.Email)) UserInfo.Email = obj.Email;
                if (!string.IsNullOrEmpty(obj.NotificationEmail)) UserInfo.NotificationEmail = obj.NotificationEmail;
                if (!string.IsNullOrEmpty(obj.PhoneNumber)) UserInfo.PhoneNumber = obj.PhoneNumber;
                if (!string.IsNullOrEmpty(obj.EmployeeId)) UserInfo.EmployeeId = obj.EmployeeId;
                if (!string.IsNullOrEmpty(obj.EmployeeType)) UserInfo.EmployeeType = obj.EmployeeType;
                if (obj.HourlyRate != UserInfo.HourlyRate) UserInfo.HourlyRate = obj.HourlyRate;
                if (obj.IsIT != UserInfo.IsIT) UserInfo.IsIT = obj.IsIT;
                if (obj.IsConsultant != UserInfo.IsConsultant) UserInfo.IsConsultant = obj.IsConsultant;
                if (obj.Enabled != UserInfo.Enabled) UserInfo.Enabled = obj.Enabled;
                if (!string.IsNullOrEmpty(obj.DeskLocation)) UserInfo.DeskLocation = obj.DeskLocation;
                if (obj.EnablePasswordExpiration != UserInfo.EnablePasswordExpiration) UserInfo.EnablePasswordExpiration = obj.EnablePasswordExpiration;
                if (obj.PasswordExpiryDate != UserInfo.PasswordExpiryDate) UserInfo.PasswordExpiryDate = obj.PasswordExpiryDate;
                if (obj.DisableWorkflowNotifications != UserInfo.DisableWorkflowNotifications) UserInfo.DisableWorkflowNotifications = obj.DisableWorkflowNotifications;
                if (obj.LeaveFromDate != UserInfo.LeaveFromDate) UserInfo.LeaveFromDate = obj.LeaveFromDate;
                if (obj.LeaveToDate != UserInfo.LeaveToDate) UserInfo.LeaveToDate = obj.LeaveToDate;
                if (obj.EnableOutofOffice != UserInfo.EnableOutofOffice) UserInfo.EnableOutofOffice = obj.EnableOutofOffice;
                if (obj.WorkingHoursStart != UserInfo.WorkingHoursStart) UserInfo.WorkingHoursStart = obj.WorkingHoursStart;
                if (obj.WorkingHoursEnd != UserInfo.WorkingHoursEnd) UserInfo.WorkingHoursEnd = obj.WorkingHoursEnd;

                IdentityResult result = _profileManager.Update(UserInfo);
                if (result.Succeeded)
                {
                    _profileManager.UpdateIntoCache(UserInfo);
                    return Ok(new UserResponse { Status = true, ErrorMessages = new List<string>() { "" }, User = "User details successfully updated." });
                }

                return Ok(new UserResponse { Status = true, ErrorMessages = new List<string>() { "" } });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in UpdateUserProfile: " + ex);
                return Ok(new UserResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("StandardWorkItems")]
        public async Task<IHttpActionResult> StandardWorkItems()
        {
            await Task.FromResult(0);
            try
            {
                ProjectStandardWorkItemManager projStdWorkItemMgr = new ProjectStandardWorkItemManager(_applicationContext);
                var projStdWorkItems = projStdWorkItemMgr.Load(x => x.Deleted == false).Select(x => new { ItemOrder = x.ItemOrder, Title = x.Title, Code = x.Code, Description = x.Description, StandardWorkItem = $"{x.Title} | {x.Code} | {x.Description}" }).OrderBy(y => y.ItemOrder).ToList();

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = projStdWorkItems });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in StandardWorkItems: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("WorkItemTypes")]
        public async Task<IHttpActionResult> WorkItemTypes()
        {
            await Task.FromResult(0);
            try
            {
                DataTable resultedTable = AllocationTypeManager.LoadLevel1(_applicationContext);
                List<WorkItemTypes> workItemTypes = new List<WorkItemTypes>();

                if (resultedTable != null)
                {
                    foreach (DataRow row in resultedTable.Rows)
                    {
                        WorkItemTypes workItemType = new WorkItemTypes();
                        workItemType.Title = Convert.ToString(row["LevelTitle"]);
                        workItemType.Name = Convert.ToString(row["LevelName"]);

                        workItemTypes.Add(workItemType);
                    }
                }

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = workItemTypes });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in WorkItemTypes: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("GetSubWorkItems")]
        public async Task<IHttpActionResult> GetSubWorkItems([FromBody] SubWorkItemModel obj)
        {
            await Task.FromResult(0);
            try
            {
                bool IsModule = obj != null ? Convert.ToBoolean(obj.isModule) : true;                
                DataTable resultedTable = AllocationTypeManager.LoadLevel3(_applicationContext, obj != null ? obj.type : "CPR" , obj != null ? obj.workItem : "", "", IsModule, false);
                List<WorkItemTypes> workItemTypes = new List<WorkItemTypes>();

                if (resultedTable != null)
                {
                    foreach (DataRow row in resultedTable.Rows)
                    {
                        WorkItemTypes workItemType = new WorkItemTypes();
                        workItemType.Title = Convert.ToString(row["LevelId"]);
                        workItemType.Name = Convert.ToString(row["LevelName"]);

                        workItemTypes.Add(workItemType);
                    }
                }

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = workItemTypes });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetSubWorkItems: " + ex);
                return Ok(new EmployeesResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword([FromBody] CredentialsModel obj)
        {
            await Task.FromResult(0);
            try
            {
                if (obj == null)
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid input" } });

                UserProfileManager UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                IdentityResult result;

                string errorMessage = string.Empty, message = string.Empty;
                string aspnetUserName = string.Empty;

                DataTable TenantDetails = GetTableDataManager.GetTenantDataUsingQueries($"select * from {DatabaseObjects.Tables.Tenant} where  AccountId = '{obj.AccountID}'");

                if (TenantDetails == null || TenantDetails.Rows.Count == 0)
                {
                    errorMessage = Constants.UserAccountMessage.IsTenantDeleted;
                    Util.Log.ULog.WriteLog($"errMessage: {errorMessage}");
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { errorMessage } });
                }
                else if (TenantDetails != null && TenantDetails.Rows.Count > 0)
                {
                    // user = umanager.GetUserById(AccountID);
                    string Name = string.Empty;
                    string tenantId = TenantDetails.Rows[0]["TenantId"].ToString();
                    string Query = $"Select * from AspNetUsers  where UserName = '{obj.UserName}' and enabled = 1 and TenantID = '{tenantId}'";
                    DataTable userData = GetTableDataManager.ExecuteQuery(Query);

                    if (userData != null && userData.Rows.Count > 0)
                    {
                        Name = userData.Rows[0][DatabaseObjects.Columns.Name].ToString();
                        //Enabled = userData.Rows[0]["Enabled"].ToString();
                        aspnetUserName = userData.Rows[0][DatabaseObjects.Columns.UserName].ToString();
                    }

                    string account = TenantDetails.Rows[0]["AccountID"].ToString();
                    if (obj.AccountID != TenantDetails.Rows[0]["AccountID"].ToString())
                    {
                        errorMessage = Constants.UserAccountMessage.IsAccountIdExist;
                        Util.Log.ULog.WriteLog($"errMessage: {errorMessage}");
                        return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { errorMessage } });
                    }

                    else if (obj.UserName != aspnetUserName)
                    {
                        errorMessage = Constants.UserAccountMessage.IsUseNameExist;
                        Util.Log.ULog.WriteLog($"errMessage: {errorMessage}");
                        return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { errorMessage } });
                    }

                    else
                    {
                        // valid user
                        EmailHelper emailHelper = new EmailHelper(_applicationContext);
                        var userId = userData.Rows[0][DatabaseObjects.Columns.ID].ToString();
                        string newPassword = UserManager.GeneratePassword();

                        string passwordToken = UserManager.GeneratePasswordResetToken(userId);
                        result = UserManager.ResetPassword(userId, passwordToken, newPassword);
                        //UserManager.RefreshCache();

                        var tenantContext = ApplicationContext.CreateContext(account);
                        UserProfileManager umanagerTenant = new UserProfileManager(tenantContext);
                        UserProfile user = umanagerTenant.LoadById(userId);
                        umanagerTenant.UpdateIntoCache(user);

                        var userEmail = Convert.ToString(userData.Rows[0][DatabaseObjects.Columns.EmailID]);
                        if (result.Succeeded)
                        {
                            emailHelper.SendMailToUserAboutForgetPassword(tenantId, obj.AccountID, Name, obj.UserName, newPassword, userEmail);
                            message = "An email will be sent to you shortly with new password details. If you don't receive an email, please try again and make " +
                                "sure you enter the correct details associated with your Service Prime account.";
                        }
                    }
                }
                return Ok(new ActionResponse { Status = true, ErrorMessages = new List<string>() { errorMessage }, Message = message });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ForgotPassword: " + ex);
                return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword([FromBody] ChangePasswordModel obj)
        {
            await Task.FromResult(0);
            try
            {
                if (obj == null)
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid input" } });

                UserProfileManager UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                IdentityResult result;

                string errorMessage = string.Empty, message = string.Empty;
                bool status = true;
                var user = UserManager.FindByName(obj.UserName);
                string passwordToken = UserManager.GeneratePasswordResetToken(user.Id);
                result = UserManager.ResetPassword(user.Id, passwordToken, obj.NewPassword);

                if (result.Succeeded)
                {
                    message = "Password changed successfully";
                    status = true;
                }
                else
                {
                    errorMessage = "Error in changing Password";
                    status = false;
                }

                return Ok(new ActionResponse { Status = status, ErrorMessages = new List<string>() { errorMessage }, Message = message });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ChangePassword: " + ex);
                return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }


        [Route("UploadImage")]
        public async Task<HttpResponseMessage> UploadImage()
        {
            await Task.FromResult(0);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                UserProfileManager UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

                var httpRequest = HttpContext.Current.Request;
                string UserName = httpRequest.Form["UserName"];
                var user = UserManager.FindByName(UserName);

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 3; //Size = 1 MB

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 3 MB.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            string path = "/content/ProfileImages/";
                            if (!string.IsNullOrEmpty(user.Picture))
                            {
                                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(Path.GetDirectoryName(path)));
                                if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(user.Picture)) && !user.Picture.Equals("/Content/Images/userNew.png"))
                                {
                                    File.Delete(System.Web.HttpContext.Current.Server.MapPath(user.Picture));
                                }
                            }

                            path = $"{path}{Guid.NewGuid().ToString()}{extension}";
                            user.Picture = path;

                            IdentityResult result = _profileManager.Update(user);
                            if (result.Succeeded)
                            {
                                var filePath = HttpContext.Current.Server.MapPath(path);
                                //Userimage myfolder name where i want to save my image
                                postedFile.SaveAs(filePath);
                            }          
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error in Updating User details");
                            }
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                dict.Add("error", ex.Message);
                ULog.WriteException($"An Exception Occurred in UploadImage: " + ex);
                //Util.Log.ULog.WriteLog($"error Message: {ex.Message}");
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("TimeSheetSignOffList")]
        public async Task<IHttpActionResult> TimeSheetSignOffList([FromBody] TimeSheetSignOff obj)
        {
            await Task.FromResult(0);
            try
            {
                UserProfile UserInfo = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(obj != null ? obj.Resource : ""));

                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", _applicationContext.TenantID);
                arrParams.Add("ManagerId", _applicationContext.CurrentUser.Id);
                arrParams.Add("StartDate", obj != null ? obj.StartDate : string.Empty);
                arrParams.Add("Resource", UserInfo != null ? UserInfo.Id : string.Empty);
                arrParams.Add("SignOffStatus", obj != null ? obj.SignOffStatus : string.Empty);
                DataTable dtResult = uGITDAL.ExecuteDataSetWithParameters("GetTimeSheetSignOffList", arrParams);

                return Ok(new CommonTicketResponse { Status = true, ErrorMessages = new List<string>() { "" }, Data = dtResult });
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in TimeSheetSignOffList: " + ex);
                return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("SendTimeSheetForApproval")]
        public async Task<IHttpActionResult> SendTimeSheetForApproval([FromBody] TimeSheetSignOff obj)
        {
            await Task.FromResult(0);
            try
            {
                if (obj == null)
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid Input" }, Message = null });

                UserProfile currentSelectedUser = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(obj.Resource));
                if (currentSelectedUser == null)
                {
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username" }, Message = null });
                }

                try
                {
                    DateTime signOffWeekStartDate = UGITUtility.StringToDateTime(obj.StartDate);
                    DateTime signOffWeekEndDate = signOffWeekStartDate.EndOfWeek(DayOfWeek.Sunday);

                    ResourceTimeSheetSignOff resourceTimeSheetSignOff = null;
                    resourceTimeSheetSignOff = resourceTimeSheetSignOffManager.Load(x => x.Resource.EqualsIgnoreCase(obj.Resource) && x.StartDate == signOffWeekStartDate && x.Deleted == false).FirstOrDefault();
                    if (resourceTimeSheetSignOff != null && resourceTimeSheetSignOff.ID > 0 && resourceTimeSheetSignOff.SignOffStatus == Constants.PendingApproval)
                        return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { }, Message = "Timesheet already sent for Approval." });
                    else
                        resourceTimeSheetSignOff = new ResourceTimeSheetSignOff();

                    resourceTimeSheetSignOff.Title = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd");
                    resourceTimeSheetSignOff.StartDate = signOffWeekStartDate;
                    resourceTimeSheetSignOff.EndDate = signOffWeekEndDate;
                    resourceTimeSheetSignOff.Resource = currentSelectedUser.Id;
                    resourceTimeSheetSignOff.History = _applicationContext.CurrentUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetPendingApprovalStatus;
                    resourceTimeSheetSignOff.SignOffStatus = Constants.PendingApproval;

                    resourceTimeSheetSignOffManager.AddOrUpdate(resourceTimeSheetSignOff);

                    SendEmail(_applicationContext.CurrentUser.Name, _applicationContext.CurrentUser.Email, Constants.TimesheetPendingApprovalStatus, signOffWeekStartDate, currentSelectedUser, obj.Comments);

                    return Ok(new ActionResponse { Status = true, ErrorMessages = new List<string>() { }, Message = "Timesheet sent for Approval." });

                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteLog($"Error in SendTimeSheetForApproval API: {ex.Message}");
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { }, Message = "Error in Sending Timesheet for Approval. " + ex.Message });
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in SendTimeSheetForApproval: " + ex);
                return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("ApproveTimeSheet")]
        public async Task<IHttpActionResult> ApproveTimeSheet([FromBody] TimeSheetSignOff obj)
        {
            await Task.FromResult(0);
            try
            {
                if (obj == null)
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid Input" }, Message = null });

                UserProfile currentSelectedUser = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(obj.Resource));
                if (currentSelectedUser == null)
                {
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username" }, Message = null });
                }

                try
                {
                    DateTime signOffWeekStartDate = UGITUtility.StringToDateTime(obj.StartDate);
                    DateTime signOffWeekEndDate = signOffWeekStartDate.EndOfWeek(DayOfWeek.Sunday);

                    ResourceTimeSheetSignOff resourceTimeSheetSignOff = new ResourceTimeSheetSignOff();
                    resourceTimeSheetSignOff = resourceTimeSheetSignOffManager.Load(x => x.StartDate == signOffWeekStartDate && x.EndDate == x.EndDate && x.Resource.EqualsIgnoreCase(currentSelectedUser.Id)).FirstOrDefault();
                    if (resourceTimeSheetSignOff != null && resourceTimeSheetSignOff.SignOffStatus == Constants.Approved)
                        return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { }, Message = "Timesheet already Approved." });

                    /*
                    resourceTimeSheetSignOff.Title = currentSelectedUser.Name + Constants.Separator7 + Constants.SpaceSeparator + signOffWeekStartDate.ToString("yyyy-MM-dd") + Constants.SpaceSeparator + Constants.DashSeparator + Constants.SpaceSeparator + signOffWeekEndDate.ToString("yyyy-MM-dd");
                    resourceTimeSheetSignOff.StartDate = signOffWeekStartDate;
                    resourceTimeSheetSignOff.EndDate = signOffWeekEndDate;
                    resourceTimeSheetSignOff.Resource = currentSelectedUser.Id;
                    resourceTimeSheetSignOff.History = _applicationContext.CurrentUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetPendingApprovalStatus;
                    resourceTimeSheetSignOff.SignOffStatus = Constants.TimesheetApprovedStatus;
                    */

                    resourceTimeSheetSignOff.History += _applicationContext.CurrentUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimesheetApprovedStatus;
                    resourceTimeSheetSignOff.SignOffStatus = Constants.Approved;

                    resourceTimeSheetSignOffManager.AddOrUpdate(resourceTimeSheetSignOff);

                    SendEmail(currentSelectedUser.Name, currentSelectedUser.Email, Constants.TimesheetApprovedStatus, signOffWeekStartDate, currentSelectedUser, obj.Comments);

                    return Ok(new ActionResponse { Status = true, ErrorMessages = new List<string>() { }, Message = "Timesheet Approved." });

                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteLog($"Error in ApproveTimeSheet API: {ex.Message}");
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { }, Message = "Error in Timesheet Approval." + ex.Message });
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ApproveTimeSheet: " + ex);
                return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("ReturnTimeSheet")]
        public async Task<IHttpActionResult> ReturnTimeSheet([FromBody] TimeSheetSignOff obj)
        {
            await Task.FromResult(0);
            try
            {
                if (obj == null)
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid Input" }, Message = null });

                UserProfile currentSelectedUser = _profileManager.GetUsersProfile().FirstOrDefault(x => x.UserName.EqualsIgnoreCase(obj.Resource));
                if (currentSelectedUser == null)
                {
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { "Invalid username" }, Message = null });
                }

                try
                {
                    DateTime signOffWeekStartDate = UGITUtility.StringToDateTime(obj.StartDate);
                    DateTime signOffWeekEndDate = signOffWeekStartDate.EndOfWeek(DayOfWeek.Sunday);

                    ResourceTimeSheetSignOff resourceTimeSheetSignOff = new ResourceTimeSheetSignOff();
                    resourceTimeSheetSignOff = resourceTimeSheetSignOffManager.Load(x => x.StartDate == signOffWeekStartDate && x.EndDate == x.EndDate && x.Resource.EqualsIgnoreCase(currentSelectedUser.Id)).FirstOrDefault();

                    if (resourceTimeSheetSignOff != null && resourceTimeSheetSignOff.SignOffStatus == Constants.Returned)
                        return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { }, Message = "Timesheet already Rejected." });

                    resourceTimeSheetSignOff.History += _applicationContext.CurrentUser.Name + Constants.Separator + Constants.UTCPrefix + DateTime.UtcNow + Constants.Separator + Constants.TimeSheetReturnStatus + Constants.Separator + obj.Comments;
                    resourceTimeSheetSignOff.SignOffStatus = Constants.Returned;

                    resourceTimeSheetSignOffManager.AddOrUpdate(resourceTimeSheetSignOff);

                    SendEmail(currentSelectedUser.Name, currentSelectedUser.Email, Constants.TimeSheetReturnStatus, signOffWeekStartDate, currentSelectedUser, obj.Comments);

                    return Ok(new ActionResponse { Status = true, ErrorMessages = new List<string>() { }, Message = "Timesheet Returned." });
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteLog($"Error in ReturnTimeSheet API: {ex.Message}");
                    return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { }, Message = "Error in Returning Timesheet." + ex.Message });
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in ReturnTimeSheet: " + ex);
                return Ok(new ActionResponse { Status = false, ErrorMessages = new List<string>() { ex.Message } });
            }
        }

        private void SendEmail(string infromTo, string mailTo, string statusText, DateTime weekStartDate, UserProfile currentSelectedUser, string Comments)
        {
            try
            {
                string timeSheetAbsoluteUrl = string.Empty;
                UGITModule ugitModule = _moduleViewManager.GetByName(ModuleNames.RMM);

                if (ugitModule != null)
                    timeSheetAbsoluteUrl = ugitModule.StaticModulePagePath;

                DateTime weekEndDate = Extensions.EndOfWeek(weekStartDate, DayOfWeek.Sunday);
                timeSheetAbsoluteUrl = string.Format("{0}?TabN=Actuals&startDate={1}&UId={2}", timeSheetAbsoluteUrl, weekStartDate, currentSelectedUser.Id);

                string weekStartDateString = weekStartDate.ToString("MMM dd, yyyy");
                string weekEndDateString = weekEndDate.ToString("MMM dd, yyyy");

                string subject = string.Format("Timesheet for {0}: {1} {2}", currentSelectedUser.Name, weekStartDateString, statusText);
                StringBuilder bodyText = new StringBuilder();
                bodyText.AppendFormat("<div>");
                bodyText.AppendFormat("<span>Hi <b>{0}</b>,</span>", infromTo);
                bodyText.AppendFormat("<span><br /><br /><b>{0}'s</b> timesheet for the week of <b>{1}</b> to <b>{2}</b> has been <b>{3}</b>.</span>",
                                      currentSelectedUser.Name, weekStartDateString, weekEndDateString, statusText);

                if (statusText == Constants.TimeSheetReturnStatus)
                    bodyText.AppendFormat("<span><br /><br /><b>Manager Comment:</b> {0}</span>", Comments);

                bodyText.AppendFormat("<span><br /><br />Please <a href=\"{0}\">click here</a> to view the timesheet.</span>",
                                       UGITUtility.GetAbsoluteURL(timeSheetAbsoluteUrl));
                bodyText.AppendFormat("</div>");
                MailMessenger mail = new MailMessenger(_applicationContext);

                mail.SendMail(mailTo, subject, string.Empty, bodyText.ToString(), true);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in SendEmail: " + ex);
            }
        }

    }

    public class WorkItemTypes
    {
        public string Title { get; set; }
        public string Name { get; set; }
    }
}