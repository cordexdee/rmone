using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.DAL;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Manager.RMM.ViewModel;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using uGovernIT.Util.Log;
using uGovernIT.Manager.RMM;
using uGovernIT.Utility.Entities;
using DevExpress.Web.ASPxGantt;
using System.Globalization;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/Bench")]
    public class BenchController : ApiController
    {
        private ApplicationContext _applicationContext;
        private ModuleViewManager ModuleManager;
        private TicketManager _TicketManager;
        private ConfigurationVariableManager _configVariableManager;
        string labelFormat = string.Empty;
        private FunctionRoleManager _functionRoleManager;
        private FunctionRoleMappingManager _functionRoleMappingManager;
        public BenchController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            ModuleManager = new ModuleViewManager(_applicationContext);
            _TicketManager = new TicketManager(_applicationContext);
            _configVariableManager = new ConfigurationVariableManager(_applicationContext);
            _functionRoleManager = new FunctionRoleManager(_applicationContext);
            _functionRoleMappingManager = new FunctionRoleMappingManager(_applicationContext);
        }
        [HttpPost]
        [Route("FindPotentialAllocationsForResource")]
        public async Task<IHttpActionResult> FindPotentialAllocationsForResource(PotentialAllocationsRequest requestModel)
        {
            await Task.FromResult(0);
            ResourceAllocationManager objResourceAllocationManager = new ResourceAllocationManager(_applicationContext);
            try
            {
                PotentialAllocationsResponse responsModel = objResourceAllocationManager.GetPotentialAllocationsList(_applicationContext, requestModel);
                string jsonAllocation = JsonConvert.SerializeObject(responsModel);
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
        [Route("FindPotentialAllocationsByRole")]
        public async Task<IHttpActionResult> FindPotentialAllocationsByRole(PotentialAllocationsRequest requestModel)
        {
            await Task.FromResult(0);
            ResourceAllocationManager objResourceAllocationManager = new ResourceAllocationManager(_applicationContext);
            try
            {
                DataTable responsModel = objResourceAllocationManager.FindPotentialAllocationsByRole(_applicationContext, requestModel);
                List<UserProfile> userProfiles = _applicationContext.UserManager.GetUsersProfile();
                string jsonAllocation = JsonConvert.SerializeObject(new { UnfilledAllocations = responsModel, UserProfiles = userProfiles });
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

        [HttpGet]
        [Route("GetFunctionRoleMapping")]
        public async Task<IHttpActionResult> GetFunctionRoleMapping()
        {
            await Task.FromResult(0);
            List<FunctionRoleMapping> lstFunctionRoleMapping = new List<FunctionRoleMapping>();
            lstFunctionRoleMapping = _functionRoleMappingManager.LoadFunctionRoleMapping();
            if (lstFunctionRoleMapping != null && lstFunctionRoleMapping.Count > 0)
            {
                return Ok(lstFunctionRoleMapping.OrderBy(x => x.FunctionName));
            }
            return Ok("No Data Found");
        }
        [HttpGet]
        [Route("DeleteFunctionRoleMapping")]
        public async Task<IHttpActionResult> DeleteFunctionRoleMapping(string functionRoleId, string FunctionId, string RoleId)
        {
            await Task.FromResult(0);
            FunctionRoleMapping functionRoleMappingObj = _functionRoleMappingManager.LoadByID(UGITUtility.StringToLong(functionRoleId));
            _functionRoleMappingManager.Delete(functionRoleMappingObj);

            return Ok();
        }

        [HttpGet]
        [Route("GetFunctonRoles")]
        public async Task<IHttpActionResult> GetFunctonRoles()
        {
            await Task.FromResult(0);
            List<FunctionRole> lstFunctionRoles = new List<FunctionRole>();
            lstFunctionRoles = _functionRoleManager.Load();
            if (lstFunctionRoles != null && lstFunctionRoles.Count > 0) { return Ok(lstFunctionRoles); }
            else { return Ok("No Data Found"); }
        }

        [HttpGet]
        [Route("GetBenchChartData")]
        public async Task<IHttpActionResult> GetBenchChartData([FromUri] BenchChartRequest request)
        {
            await Task.FromResult(0);
            List<BenchChartResponse> lstResponse = new List<BenchChartResponse>();
            Dictionary<string, object> RUValues = new Dictionary<string, object>();
            RUValues.Add("@TenantID", HttpContext.Current.GetManagerContext().TenantID);
            RUValues.Add("@IncludeAllResources", UGITUtility.StringToBoolean(request.IncludeAllResources));
            RUValues.Add("@IncludeClosedProject", UGITUtility.StringToBoolean(request.IncludeClosedProject));
            RUValues.Add("@DisplayMode", UGITUtility.ObjectToString(request.DisplayMode));  //FTE, COUNT, PERCENT, AVAILABILITY
            RUValues.Add("@Department", UGITUtility.ObjectToString(request.Department));
            RUValues.Add("@StartDate", UGITUtility.GetFirstDayOfPreviousMonth().ToString("MM-dd-yyyy"));
            RUValues.Add("@EndDate", UGITUtility.GetLastDayOfNextMonth().ToString("MM-dd-yyyy"));
            RUValues.Add("@ResourceManager", UGITUtility.ObjectToString(request.ResourceManager));
            RUValues.Add("@AllocationType", UGITUtility.ObjectToString(request.AllocationType));
            RUValues.Add("@LevelName", UGITUtility.ObjectToString(request.LevelName));
            RUValues.Add("@GlobalRoleId", UGITUtility.ObjectToString(request.GlobalRoleId));
            RUValues.Add("@Mode", UGITUtility.ObjectToString(request.Mode));
            RUValues.Add("@Function", UGITUtility.ObjectToString(request.Function));
            DataTable dtResult = GetTableDataManager.GetData("BenchReportChartData", RUValues);
            if (dtResult != null)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    BenchChartResponse obj = new BenchChartResponse();
                    obj.Month = UGITUtility.ObjectToString(row["MName"]);
                    obj.Utilization = Math.Round(UGITUtility.StringToDouble(row["PctAllocation"]), 1);
                    lstResponse.Add(obj);
                }
            }
            return Ok(lstResponse);
        }

        [HttpGet]
        [Route("GetBenchAndOverAllocatedResources")]
        public async Task<IHttpActionResult> GetBenchAndOverAllocatedResources([FromUri] FindResourceRequest request)
        {
            await Task.FromResult(0);
            //all required managers initialization
            ApplicationContext context = HttpContext.Current.GetManagerContext();
            ResourceUsageSummaryWeekWiseManager weekwiseManager = new ResourceUsageSummaryWeekWiseManager(context);
            UserProfileManager userManager = new UserProfileManager(context);
            ResourceProjectComplexityManager complexityManager = new ResourceProjectComplexityManager(context);
            ResourceAllocationMonthlyManager monthlyManager = new ResourceAllocationMonthlyManager(context);
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            JobTitleManager jobTitleManager = new JobTitleManager(context);
            TicketManager ticketManagerr = new TicketManager(context);
            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
            List<FindResourceResponse> lstResponse = new List<FindResourceResponse>();
            GlobalRoleManager roleManager = new GlobalRoleManager(context);
            List<GlobalRole> globalRoles = roleManager.Load(x => x.TenantID == context.TenantID);
            DepartmentManager departmentManager = new DepartmentManager(context);
            FunctionRoleMappingManager functionRoleMappingManager = new FunctionRoleMappingManager(context);
            try
            {
                #region load allocation from resource allocation
                List<RResourceAllocation> lstAllOverLappingAlloc = new List<RResourceAllocation>();
                lstAllOverLappingAlloc = resourceAllocationManager.LoadOpenItems(request.AllocationStartDate.Date, request.AllocationEndDate.Date);
                #endregion
                //FunctionRole functionRole;
                List<string> selectedUsers = new List<string>();
                //int projectComplexity = 0;
                //loading current cpr ticket
                string moduleName = uHelper.getModuleNameByTicketId(request.ProjectID);
                UGITModule module = moduleManager.LoadByName(moduleName);

                List<UserProfile> lstUProfile = context.UserManager.GetUsersProfile().Where(x => x.Enabled == true).ToList();

                if ((request.Complexity || request.ProjectVolume || request.ProjectCount))
                {

                    List<UserProfile> lstComplexityUsers = new List<UserProfile>();
                    List<SummaryResourceProjectComplexity> complexityAboveCurrentCRP = complexityManager.Load();
                    if (complexityAboveCurrentCRP != null && complexityAboveCurrentCRP.Count > 0)
                        lstComplexityUsers = lstUProfile.FindAll(x => complexityAboveCurrentCRP.Any(y => y.UserId == x.Id)).ToList();

                    //filtering only those users which have complexity saved in summary_resourceprojectcoomplexity table bcz project count and revenue count depends on summary table only
                    if (request.Complexity)
                    {
                        selectedUsers = lstComplexityUsers.Select(x => x.Id).Distinct().ToList();
                    }
                }
                if (!string.IsNullOrEmpty(request.Type))
                {
                    lstUProfile = lstUProfile.Where(o => o.GlobalRoleId == request.Type).ToList();
                }
                if (request.departments > 0)
                {
                    // filter based on department selected.
                    lstUProfile = lstUProfile.Where(o => o.Department == request.departments.ToString()).ToList();
                }
                if (!string.IsNullOrEmpty(request.FunctionId))
                {
                    List<FunctionRoleMapping> lstFunctionRoleMapping = functionRoleMappingManager.LoadFunctionRoleMappingById(UGITUtility.StringToLong(request.FunctionId));
                    lstUProfile = lstUProfile.Where(x => lstFunctionRoleMapping.Any(y => y.RoleId == x.GlobalRoleId)).ToList();
                }
                if (!request.Complexity)
                {
                    selectedUsers = lstUProfile.Select(x => x.Id).Distinct().ToList();
                }

                List<JobTitle> jobTitles = new List<JobTitle>();
                if (!string.IsNullOrEmpty(request.JobTitles))
                {
                    List<string> lstjobtitles = UGITUtility.ConvertStringToList(request.JobTitles, Constants.Separator);
                    jobTitles = jobTitleManager.Load(x => lstjobtitles.Contains(x.Title));
                }

                if (jobTitles.Count > 0)
                {
                    // filter based on job title.
                    List<string> lstjobtitles = jobTitles.Select(x => Convert.ToString(x.ID)).ToList();
                    lstUProfile = lstUProfile.FindAll(x => lstjobtitles.Contains(Convert.ToString(x.JobTitleLookup)));
                }
                if (!string.IsNullOrEmpty(request.DivisionId) && UGITUtility.StringToInt(request.DivisionId) != 0)
                {
                    List<string> lstDepartments = departmentManager.Load(x => request.DivisionId.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).Contains(UGITUtility.ObjectToString(x.DivisionIdLookup))
                    && !x.Deleted).Select(x => UGITUtility.ObjectToString(x.ID)).ToList();
                    lstUProfile = lstUProfile.FindAll(x => lstDepartments.Contains(UGITUtility.ObjectToString(x.Department)));
                }
                selectedUsers = lstUProfile.Select(x => x.Id).ToList() ?? null;

                DataTable dtTicketIds = new DataTable();
                if (request.Customer == true && request.Sector == true)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{request.CompanyLookup}' and {DatabaseObjects.Columns.BCCISector}='{request.SectorName}'", DatabaseObjects.Columns.TicketId, null);
                }
                else if (request.Customer == true && request.Sector == false)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.CRMCompanyLookup}='{request.CompanyLookup}'", DatabaseObjects.Columns.TicketId, null);
                }
                else if (request.Customer == false && request.Sector == true)
                {
                    dtTicketIds = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.BCCISector}='{request.SectorName}'", DatabaseObjects.Columns.TicketId, null);
                }

                if (dtTicketIds != null && dtTicketIds.Rows.Count > 0)
                {
                    List<string> LstOpenTicketIds = new List<string>();
                    LstOpenTicketIds.AddRange(dtTicketIds.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.TicketId)));

                    if (selectedUsers != null && selectedUsers.Count > 0)
                    {
                        List<string> users = lstAllOverLappingAlloc.Where(x => LstOpenTicketIds.Any(y => x.TicketID.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                        selectedUsers = selectedUsers.Intersect(users).ToList();
                    }
                    else
                    {
                        selectedUsers = lstAllOverLappingAlloc.Where(x => LstOpenTicketIds.Any(y => x.TicketID.Equals(y))).Select(z => z.Resource).Distinct().ToList();
                    }
                }


                UserProfile profile = null;
                JobTitle jobTitle = null;
                List<RResourceAllocation> lstUserAllocation = null;
                List<SummaryResourceProjectComplexity> userComplexities = new List<SummaryResourceProjectComplexity>();

                int noOfWorkingHoursPerDay = uHelper.GetWorkingHoursInADay(context);
                int noOfWorkingDaysPerWeek = uHelper.GetWorkingDaysInWeeks(context, 1);
                double searchPeriodDays = uHelper.GetTotalWorkingDaysBetween(context, request.AllocationStartDate.Date, request.AllocationEndDate.Date);
                //int noOfWeeks = uHelper.GetWeeksFromDays(_context, noOfWorkingDays);

                //selectedUsers = selectedUsers.Where(x => x == "f4d20c1f-9915-41c4-b344-8991e24801dc").ToList();
                //code to create response object only for those users who have complexity saved
                //calculat allocation based on following logic
                // if we have 3 weeks from start 4th to 15th then
                // (w1*4 + w2*7 + w3*4) / (4+7+4)
                //selectedUsers =new List<string>() { selectedUsers.FirstOrDefault(x => x.EqualsIgnoreCase("44380d17-c887-488c-856b-31753e4197b7")) };
                UserProjectExperienceManager userProjectExperienceMGR = new UserProjectExperienceManager(context);

                List<ProjectTag> projectTags = null;
                List<ProjectTag> certificates = null;
                if (uHelper.IsExperienceTagAllowGroupFilter(context) && request.SelectedTags != null && request.SelectedTags.Count() > 0)
                {
                    projectTags = request.SelectedTags.Where(x => x.IsMandatory && x.Type == TagType.Experience).ToList();
                    certificates = request.SelectedTags.Where(x => x.IsMandatory && x.Type == TagType.Certificate).ToList();
                }

                foreach (string uProfile in selectedUsers)
                {
                    profile = lstUProfile.FirstOrDefault(x => x.Id == uProfile);

                    if (profile == null)
                        continue;

                    FindResourceResponse response = new FindResourceResponse();
                    response.AssignedTo = profile.Id;
                    response.AssignedToName = profile.Name;
                    response.GroupID = profile.GlobalRoleId;   // request.GroupID;
                    response.JobTitle = profile.JobProfile;
                    response.UserImagePath = profile.Picture;

                    GlobalRole typeGroup = globalRoles.FirstOrDefault(x => x.Id == profile.GlobalRoleId);
                    if (typeGroup != null)
                    {
                        response.RoleName = typeGroup.Name;
                    }

                    if (certificates != null && certificates.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(profile.UserCertificateLookup))
                        {
                            continue;
                        }
                        else
                        {
                            bool resourceSelected = true;
                            List<string> userCertificates = profile.UserCertificateLookup.Split(',').ToList();
                            certificates.ForEach(x => {
                                if (userCertificates.Contains(x.TagId))
                                {
                                    if (response.ResourceTags == null)
                                    {
                                        response.ResourceTags = new List<ResourceTag>();
                                    }
                                    response.ResourceTags.Add(new ResourceTag { TagId = x.TagId, TagCount = "Y", Type = TagType.Certificate });
                                }
                                else
                                {
                                    resourceSelected = false;
                                }
                            });
                            if (!resourceSelected)
                            {
                                continue;
                            }
                        }
                    }

                    if (projectTags != null && projectTags.Count() > 0)
                    {
                        List<UserProjectExperience> userProjectExperiences = userProjectExperienceMGR.Load(x => !string.IsNullOrWhiteSpace(x.ProjectID) && x.UserId == profile.Id);
                        bool resourceSelected = true;
                        projectTags.ForEach(x =>
                        {
                            int userTagCount = userProjectExperiences.Where(y => y.TagLookup.ToString() == x.TagId).Count();
                            if (userTagCount >= x.MinValue)
                            {
                                if (response.ResourceTags == null)
                                {
                                    response.ResourceTags = new List<ResourceTag>();
                                }
                                response.ResourceTags.Add(new ResourceTag { TagId = x.TagId, TagCount = userTagCount.ToString(), Type = TagType.Experience });
                            }
                            else
                            {
                                resourceSelected = false;
                            }

                        });
                        if (!resourceSelected)
                        {
                            continue;
                        }
                    }
                    //selecting monthlly allocation of given user only
                    //assigning average pct allocation on response object
                    double? totalPercentAllocatedInRange = 0;
                    double? hardPercentAllocatedInRange = 0;
                    double? softPercentAllocatedInRange = 0;
                    //User related allocation
                    lstUserAllocation = lstAllOverLappingAlloc.Where(x => x.Resource == profile.Id && x.ProjectEstimatedAllocationId != null).OrderBy(x => x.AllocationStartDate).ToList();
                    if (lstUserAllocation != null && lstUserAllocation.Count > 0)
                    {
                        List<DateTime> lstOfStartDate = null;
                        List<DateTime> lstOfEndDate = null;
                        double? totalAllocOverlapDays = 0;
                        double? hardAllocOverlapDays = 0;
                        double? softAllocOverlapDays = 0;

                        foreach (RResourceAllocation ralloc in lstUserAllocation)
                        {
                            //To get overlapping days (Max start date and Min end date)
                            lstOfStartDate = new List<DateTime>() { request.AllocationStartDate, ralloc.AllocationStartDate.Value };
                            lstOfEndDate = new List<DateTime>() { request.AllocationEndDate, ralloc.AllocationEndDate.Value };
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
                            if (ralloc.SoftAllocation)
                                softAllocOverlapDays += workingDays * pctAlloc;
                            else
                                hardAllocOverlapDays += workingDays * pctAlloc;
                        }

                        //allocation in given date range 
                        totalPercentAllocatedInRange = (totalAllocOverlapDays / searchPeriodDays) * 100;
                        softPercentAllocatedInRange = (softAllocOverlapDays / searchPeriodDays) * 100;
                        hardPercentAllocatedInRange = (hardAllocOverlapDays / searchPeriodDays) * 100;
                    }


                    response.TotalPctAllocation = 0;
                    response.PctAllocation = 0;
                    response.SoftPctAllocation = 0;
                    if (totalPercentAllocatedInRange > 0)
                        response.TotalPctAllocation = Math.Round(totalPercentAllocatedInRange.Value, 1);
                    if (softPercentAllocatedInRange > 0)
                        response.SoftPctAllocation = Math.Round(softPercentAllocatedInRange.Value, 1);
                    if (totalPercentAllocatedInRange > 0)
                        response.PctAllocation = Math.Round(hardPercentAllocatedInRange.Value, 1);

                    //BTS-22-000946: Changed the tile color as per the BTS
                    if (response.TotalPctAllocation < 80)
                        response.AllocationRange = 0;
                    else if (response.TotalPctAllocation >= 80 && response.TotalPctAllocation <= 99)
                        response.AllocationRange = 1;
                    else if (response.TotalPctAllocation >= 100 && response.TotalPctAllocation <= 110)
                        response.AllocationRange = 2;
                    else if (response.TotalPctAllocation > 110)
                        response.AllocationRange = 3;

                    //list of complexity to count highest complexity, totalreveneu and project count for each user
                    //List<SummaryResourceProjectComplexity> userComplexities = new List<SummaryResourceProjectComplexity>();
                    userComplexities = complexityManager.Load(x => x.UserId == profile.Id);

                    //capacity code only needed if complexity exits for that user
                    if (userComplexities != null && userComplexities.Count > 0)
                    {
                        response.HighestComplexity = userComplexities.Max(x => x.Complexity);
                        //find project count
                        //JobTitle jobTitle = null;
                        if (request.ProjectCount)
                        {
                            response.ProjectCount = userComplexities.Sum(x => x.Count);
                            jobTitle = jobTitleManager.LoadByID(profile.JobTitleLookup);
                            response.TotalVolumeRange = 0;
                            if (jobTitle != null)
                            {
                                if ((jobTitle.LowProjectCapacity <= 0 && jobTitle.HighProjectCapacity <= 0) || response.ProjectCount <= jobTitle.LowProjectCapacity)
                                    response.projectCountRange = 0;
                                else if (response.ProjectCount > jobTitle.HighProjectCapacity)
                                    response.projectCountRange = 2;
                                else
                                    response.projectCountRange = 1;
                            }
                        }

                        if (request.ProjectVolume)
                        {
                            double cost = 0;
                            cost = userComplexities.Sum(x => x.HighProjectCapacity);

                            response.TotalVolume = UGITUtility.FormatNumber(cost, "currency");
                            response.TotalVolumeRange = 0;
                            jobTitle = jobTitleManager.LoadByID(profile.JobTitleLookup);
                            if (jobTitle != null)
                            {
                                if ((jobTitle.LowRevenueCapacity <= 0 && jobTitle.HighRevenueCapacity <= 0) || cost <= jobTitle.LowRevenueCapacity)
                                    response.TotalVolumeRange = 0;
                                else if (cost > jobTitle.HighRevenueCapacity)
                                    response.TotalVolumeRange = 2;
                                else
                                    response.TotalVolumeRange = 1;
                            }
                        }
                    }
                    lstResponse.Add(response);
                }

                List<FindResourceResponse> lstResponseAvailability = new List<FindResourceResponse>();
                if (request.isAllocationView == false && request.ResourceAvailability != ResourceAvailabilityType.AllResource)
                {

                    foreach (FindResourceResponse responseAvailability in lstResponse)
                    {
                        if (responseAvailability.PctAllocation <= 100)
                        {
                            lstResponseAvailability.Add(responseAvailability);
                        }
                    }
                    lstResponse = lstResponseAvailability;
                }


                //set ordering in last in descending order and asc for pct allocation
                if (request.ResourceAvailability == ResourceAvailabilityType.FullyAvailable)
                    lstResponse = lstResponse.Where(x => x.PctAllocation == 0).OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();
                else if (request.ResourceAvailability == ResourceAvailabilityType.PartiallyAvailable)
                    lstResponse = lstResponse.Where(x => x.PctAllocation >= 0).OrderBy(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();
                else
                    lstResponse = lstResponse.OrderByDescending(x => x.PctAllocation).ThenBy(x => x.AssignedToName).ToList();


            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return Ok(lstResponse.Take(5));
        }

        [HttpGet]
        [Route("GetBenchAnalyticstData")]
        public async Task<IHttpActionResult> GetBenchAnalyticstData([FromUri] BenchChartRequest request)
        {
            await Task.FromResult(0);
            List<BenchAnalyticsResponse> lstResponse = new List<BenchAnalyticsResponse>();
            Dictionary<string, object> RUValues = new Dictionary<string, object>();
            RUValues.Add("@TenantID", _applicationContext.TenantID);
            RUValues.Add("@IncludeAllResources", UGITUtility.ObjectToString(request.IncludeAllResources));
            RUValues.Add("@IncludeClosedProject", UGITUtility.ObjectToString(request.IncludeClosedProject));
            RUValues.Add("@DisplayMode", "FTE" ?? string.Empty);  //FTE, COUNT, PERCENT, AVAILABILITY
            RUValues.Add("@Department", UGITUtility.ObjectToString(request.Department) ?? string.Empty);
            RUValues.Add("@StartDate", Convert.ToDateTime(request.StartDate).ToString("MM-dd-yyyy"));
            RUValues.Add("@EndDate", Convert.ToDateTime(request.EndDate).ToString("MM-dd-yyyy"));
            RUValues.Add("@ResourceManager", "0" ?? string.Empty);
            RUValues.Add("@AllocationType", UGITUtility.ObjectToString(request.AllocationType) ?? string.Empty);
            RUValues.Add("@LevelName", UGITUtility.ObjectToString(request.LevelName) ?? string.Empty);
            RUValues.Add("@GlobalRoleId", UGITUtility.ObjectToString(request.GlobalRoleId) ?? string.Empty);
            RUValues.Add("@Mode", UGITUtility.ObjectToString(request.Mode));
            //RUValues.Add("@ShowAvgColumns", "1");
            RUValues.Add("@Function", request.Function);

            DataSet dsData = GetTableDataManager.GetDataSet("ResourceUtlizationFooter_AllocationHrs", RUValues);
            if (dsData != null && dsData.Tables.Count > 0)
            {
                if (request.DisplayMode == "Monthly")
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        string monthNameAbbreviated = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i);
                        string monthAbbreviatedFirst3Letters = monthNameAbbreviated.Substring(0, 3);
                        BenchAnalyticsResponse obj = new BenchAnalyticsResponse();
                        DateTime AllocationMonthStartDate = new DateTime(Convert.ToDateTime(DateTime.Now).Year, i, 1);
                        if (dsData.Tables[1] != null && dsData.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtTCResult = dsData.Tables[1];  //total capacity table
                            DataTable dtADResult = dsData.Tables[0];   //AllocatedDemand table
                            DataTable dtUAResult = dsData.Tables[2];   //unfilled allocation table
                            obj.Month = monthAbbreviatedFirst3Letters;
                            obj.TotalCapacity = Math.Round(UGITUtility.StringToDouble(dtTCResult.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]), 1);
                            obj.AllocatedDemand = Math.Round(UGITUtility.StringToDouble(dtADResult.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]), 1);
                            obj.UnfilledDemand = Math.Round(UGITUtility.StringToDouble(dtUAResult.Rows[0][AllocationMonthStartDate.ToString("MMM-dd-yy")]), 1);
                            lstResponse.Add(obj);

                        }
                    }
                }
                else
                {

                    DateTime startDate = Convert.ToDateTime(request.StartDate);
                    DateTime endDate = Convert.ToDateTime(request.EndDate);

                    DateTime currentDate = startDate;
                    while (currentDate <= endDate)
                    {
                        DateTime weekStartDate = UGITUtility.GetFirstMondayOfWeek(currentDate);   // currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)DayOfWeek.Monday);
                        DateTime weekEndDate = weekStartDate.AddDays(6); // Get the end of the week

                        BenchAnalyticsResponse obj = new BenchAnalyticsResponse();
                        if (dsData.Tables[1] != null && dsData.Tables[1].Rows.Count > 0)
                        {
                            if (dsData.Tables[3].Columns.Contains(weekStartDate.ToString("MMM-dd-yy")))
                            {
                                DataTable dtTCResult = dsData.Tables[1];  //total capacity table
                                DataTable dtADResult = dsData.Tables[0];   //AllocatedDemand table
                                DataTable dtUAResult = dsData.Tables[2];   //unfilled allocation table
                                obj.Month = weekStartDate.ToString("MMM-dd");
                                obj.TotalCapacity = Math.Round(UGITUtility.StringToDouble(dtTCResult.Rows[0][weekStartDate.ToString("MMM-dd-yy")]), 1);
                                obj.AllocatedDemand = Math.Round(UGITUtility.StringToDouble(dtADResult.Rows[0][weekStartDate.ToString("MMM-dd-yy")]), 1);
                                obj.UnfilledDemand = Math.Round(UGITUtility.StringToDouble(dtUAResult.Rows[0][weekStartDate.ToString("MMM-dd-yy")]), 1);
                                lstResponse.Add(obj);
                            }
                        }
                        // Move to the next week
                        currentDate = weekEndDate.AddDays(1);
                    }
                }
            }
            return Ok(lstResponse);
        }
    }

    public class BenchChartRequest
    {
        public bool IncludeAllResources { get; set; }
        public bool IncludeClosedProject { get; set; }
        public string DisplayMode { get; set; }
        public string Department { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ResourceManager { get; set; }
        public string AllocationType { get; set; }
        public string LevelName { get; set; }
        public string GlobalRoleId { get; set; }
        public string Mode { get; set; }
        public string Function { get; set; }
    }

    public class BenchChartResponse
    {
        public string Month { get; set; }
        public double Utilization { get; set; }

    }
    public class BenchAnalyticsResponse
    {
        public string Month { get; set; }
        public double TotalCapacity { get; set; }
        public double AllocatedDemand { get; set; }
        public double UnfilledDemand { get; set; }
    }
}