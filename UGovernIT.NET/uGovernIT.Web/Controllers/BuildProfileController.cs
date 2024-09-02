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
using Newtonsoft.Json;
using uGovernIT.Manager.Managers;
using System;
using uGovernIT.Web.Helpers;

using ExportReport = uGovernIT.Helpers.ExportReport;using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Data.Entity;
using uGovernIT.Web.ControlTemplates.RMONE;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/buildprofile")]
    public class BuildProfileController : ApiController
    {

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IHttpActionResult> GetUsers(string projectID)
        {
            await Task.FromResult(0);
            if (!string.IsNullOrEmpty(projectID))
            {
                try
                {
                    ApplicationContext context = HttpContext.Current.GetManagerContext();
                    GlobalRoleManager roleManager = new GlobalRoleManager(context);
                    ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
                    List<ProjectEstimatedAllocation> projectAllocations = cRMProjectAllocationManager.Load(x => x.TicketId == projectID && x.Deleted != true);
                    List<AllocationTemplateModel> allocations = new List<AllocationTemplateModel>();
                    projectAllocations = projectAllocations?.Where(o => !string.IsNullOrWhiteSpace(context.UserManager.GetUserInfoById(o.AssignedTo).Name))?.GroupBy(x => x.AssignedTo).Select(o => o.First()).ToList();
                    foreach (ProjectEstimatedAllocation alloc in projectAllocations)
                    {
                        AllocationTemplateModel model = new AllocationTemplateModel();
                        model.AllocationEndDate = alloc.AllocationEndDate;
                        model.AllocationStartDate = alloc.AllocationStartDate;
                        model.AssignedTo = alloc.AssignedTo;
                        UserProfile user = context.UserManager.GetUserInfoById(alloc.AssignedTo);
                        if (user != null)
                            model.AssignedToName = user.Name;
                        model.PctAllocation = alloc.PctAllocation;
                        model.ID = alloc.ID;
                        model.Type = alloc.Type;
                        GlobalRole typeGroup = roleManager.Get(x => x.Id == alloc.Type);
                        if (typeGroup != null)
                            model.TypeName = typeGroup.Name;
                        model.Title = alloc.Title;
                        allocations.Add(model);


                    }
                    string jsonProjectAllocations = JsonConvert.SerializeObject(allocations);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);

                }
                catch (Exception ex)
                {
                    ULog.WriteException($"An Exception Occurred in GetUsers: " + ex);
                }
            }

            return Ok();
        }



        [HttpGet]
        [Route("GetUsersHistoryProfile")]
        public async Task<IHttpActionResult> GetUserHistoryProfile(string projectID, bool IncludeCurrentProject = false
            , string selectedUserId = "", string selectedCompany = "", string selectedRole = "", string selectedType = ""
            , string selectedComplexity = "", string fromDate = "", string toDate = "")
        {
            try
            {
                await Task.FromResult(0);
                if (!string.IsNullOrEmpty(projectID) && !string.IsNullOrEmpty(selectedUserId))
                {
                    ApplicationContext context = HttpContext.Current.GetManagerContext();
                    GlobalRoleManager roleManager = new GlobalRoleManager(context);
                    ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
                    TicketManager ticketManager = new TicketManager(context);
                    ModuleViewManager moduleManager = new ModuleViewManager(context);
                    FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
                    UserProfileManager ObjUserProfileManager = new UserProfileManager(context);
                    UserProfile userProfile = null;
                    string currentModule = string.Empty;
                    string projectModule = ModuleNames.CPR;
                    string moduleTablename = string.Empty;
                    List<UserHistory> userHistoryList = new List<UserHistory>();
                    List<ProjectEstimatedAllocation> projectAllocations = null;

                    currentModule = uHelper.getModuleNameByTicketId(projectID);
                    if (currentModule.EqualsIgnoreCase(ModuleNames.NPR))
                        projectModule = ModuleNames.PMM;
                    else if (currentModule.EqualsIgnoreCase(ModuleNames.OPM) || currentModule.EqualsIgnoreCase(ModuleNames.CPR))
                        projectModule = ModuleNames.CPR;

                    moduleTablename = moduleManager.GetModuleTableName(projectModule);

                    DataTable dtTickets = new DataTable();
                    string requiredColumns = $"{DatabaseObjects.Columns.TicketId},{DatabaseObjects.Columns.Title},{DatabaseObjects.Columns.Description},{DatabaseObjects.Columns.CRMCompanyLookup},{DatabaseObjects.Columns.TicketRequestTypeLookup},{DatabaseObjects.Columns.ApproxContractValue},{DatabaseObjects.Columns.PreconStartDate},{DatabaseObjects.Columns.EstimatedConstructionStart},{DatabaseObjects.Columns.EstimatedConstructionEnd},{DatabaseObjects.Columns.CRMProjectComplexity}";
                    if (IncludeCurrentProject == false)
                        dtTickets = ticketManager.GetTickets($"select {requiredColumns} from {moduleTablename} where ({DatabaseObjects.Columns.Closed} = 1) and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                    else
                        dtTickets = ticketManager.GetTickets($"select {requiredColumns} from {moduleTablename} where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");

                    var selectedCompanyList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(selectedCompany))
                    {
                        selectedCompanyList = selectedCompany.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                    }

                    var selectedProjectTypeList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(selectedType))
                    {
                        selectedProjectTypeList = selectedType.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                    }

                    var selectedRoleList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(selectedRole))
                    {
                        selectedRoleList = selectedRole.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                    }

                    var selectedComplexityList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(selectedComplexity))
                    {
                        selectedComplexityList = selectedComplexity.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                    }

                    foreach (var user in selectedUserId.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList())
                    {
                        UserHistory userHistory = new UserHistory();
                        userHistory.UserId = user;
                        userProfile = ObjUserProfileManager.GetUserById(Convert.ToString(user));
                        if (userProfile != null)
                        {
                            userHistory.UserName = userProfile.Name;
                            userHistory.key = userProfile.Name;
                            userHistory.Resume = userProfile.Resume;

                            userHistory.Picture = UGITUtility.GetDefualtProfilePictureIfEmpty(userProfile.Picture);
                            userHistory.UserDatailsControl = LoadUserDeatilControl(user);
                        }

                        projectAllocations = cRMProjectAllocationManager.Load(x => x.AssignedTo.EqualsIgnoreCase(user));
                        try
                        {
                            if (projectAllocations != null && projectAllocations.Count > 0)
                            {
                                projectAllocations = projectAllocations.GroupBy(o => new { o.TicketId, o.Type }).Select(o => o.First()).ToList();
                                var projectDetails = (from tkt in dtTickets.AsEnumerable()
                                                      join alloc in projectAllocations on
                                                      tkt.Field<string>(DatabaseObjects.Columns.TicketId) equals alloc.TicketId
                                                      select new { alloc.Type, alloc.Duration, tkt }).ToList();

                                if (projectDetails != null && projectDetails.Count() > 0)
                                {
                                    projectDetails.Reverse();
                                    foreach (var item in projectDetails)
                                    {
                                        UserHistoryDetail userHistoryDetail = new UserHistoryDetail();
                                        if (!string.IsNullOrEmpty(Convert.ToString(item.tkt[DatabaseObjects.Columns.CRMCompanyLookup])))
                                            userHistoryDetail.CompanyName = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.CRMCompanyLookup, Convert.ToString(item.tkt[DatabaseObjects.Columns.CRMCompanyLookup]));
                                        else
                                            userHistoryDetail.CompanyName = string.Empty;

                                        if (!string.IsNullOrWhiteSpace(selectedCompany) && !selectedCompanyList.Contains(userHistoryDetail.CompanyName))
                                        {
                                            continue;
                                        }

                                        if (!string.IsNullOrEmpty(Convert.ToString(item.tkt[DatabaseObjects.Columns.TicketRequestTypeLookup])))
                                            userHistoryDetail.ProjectType = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeLookup, Convert.ToString(item.tkt[DatabaseObjects.Columns.TicketRequestTypeLookup]));
                                        else
                                            userHistoryDetail.ProjectType = string.Empty;

                                        if (!string.IsNullOrWhiteSpace(selectedType) && !selectedProjectTypeList.Contains(userHistoryDetail.ProjectType))
                                        {
                                            continue;
                                        }

                                        userHistoryDetail.TicketId = Convert.ToString(item.tkt[DatabaseObjects.Columns.TicketId]);
                                        userHistoryDetail.ProjectTitle = Convert.ToString(item.tkt[DatabaseObjects.Columns.Title]);
                                        userHistoryDetail.Description = Convert.ToString(item.tkt[DatabaseObjects.Columns.Description]);
                                        userHistoryDetail.ProjectComplexityChoice = Convert.ToString(item.tkt[DatabaseObjects.Columns.CRMProjectComplexity]);

                                        if (!string.IsNullOrWhiteSpace(selectedComplexity) && !selectedComplexityList.Contains(userHistoryDetail.ProjectComplexityChoice))
                                        {
                                            continue;
                                        }

                                        GlobalRole typeGroup = roleManager.Get(x => x.Id == item.Type);
                                        if (typeGroup != null)
                                        {
                                            userHistoryDetail.RoleName = typeGroup.Name;
                                        }
                                        if (!string.IsNullOrWhiteSpace(selectedRole) && !selectedRoleList.Contains(userHistoryDetail.RoleName))
                                        {
                                            continue;
                                        }
                                        userHistoryDetail.Duration = item.Duration;
                                        userHistoryDetail.ContractValue = string.Format("{0:c}", item.tkt[DatabaseObjects.Columns.ApproxContractValue]); //Convert.ToString(item[DatabaseObjects.Columns.ApproxContractValue]);

                                        userHistoryDetail.PreconStartDate = (item.tkt[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value
                                            ? Convert.ToDateTime(item.tkt[DatabaseObjects.Columns.PreconStartDate]).ToString("MM/dd/yyyy")
                                            : item.tkt[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value
                                            ? Convert.ToDateTime(item.tkt[DatabaseObjects.Columns.EstimatedConstructionStart]).ToString("MM/dd/yyyy") :
                                            string.Empty);

                                        userHistoryDetail.PreconEndDate = (item.tkt[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value ? Convert.ToDateTime(item.tkt[DatabaseObjects.Columns.EstimatedConstructionEnd]).ToString("MM/dd/yyyy") : "");

                                        if (!string.IsNullOrWhiteSpace(userHistoryDetail.PreconStartDate)
                                            && !string.IsNullOrWhiteSpace(userHistoryDetail.PreconEndDate)
                                            && !string.IsNullOrWhiteSpace(fromDate)
                                            && !string.IsNullOrWhiteSpace(toDate))
                                        {
                                            DateTime preconStartDate = DateTime.Parse(userHistoryDetail.PreconStartDate);
                                            DateTime preconEndDate = DateTime.Parse(userHistoryDetail.PreconEndDate);
                                            DateTime fDate = DateTime.Parse(fromDate);
                                            DateTime tDate = DateTime.Parse(toDate);

                                            if (!(preconStartDate <= tDate && fDate <= preconEndDate))
                                            {
                                                continue;
                                            }

                                        }
                                        //open code

                                        string viewUrl = string.Empty;
                                        string title = string.Empty;
                                        string func = string.Empty;
                                        string ticketTitle = string.Empty;
                                        string ticketID = string.Empty;
                                        //string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
                                        string sourceURL = "/default.aspx";
                                        ticketID = userHistoryDetail.TicketId;
                                        ticketTitle = userHistoryDetail.ProjectTitle;
                                        string module = uHelper.getModuleNameByTicketId(ticketID);
                                        DataRow moduleDetail = null;
                                        if (!string.IsNullOrEmpty(module))
                                        {
                                            DataTable moduledt = UGITUtility.ObjectToData(moduleManager.LoadByName(module));
                                            if (moduledt.Rows.Count > 0)
                                                moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(moduleName);
                                        }

                                        if (moduleDetail != null)
                                        {
                                            viewUrl = string.Empty;
                                            if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                                            {
                                                viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                                            }


                                            if (!string.IsNullOrEmpty(ticketTitle))
                                            {
                                                ticketTitle = UGITUtility.TruncateWithEllipsis(ticketTitle, 100, string.Empty);
                                            }

                                            if (!string.IsNullOrEmpty(ticketID))
                                            {
                                                title = string.Format("{0}: ", ticketID);
                                            }
                                            title = string.Format("{0}{1}", title, ticketTitle);

                                        }

                                        userHistoryDetail.Url = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketID), title, sourceURL, 90, 90);

                                        userHistory.items.Add(userHistoryDetail);
                                    }
                                }
                                else
                                {
                                    UserHistoryDetail userHistoryDetail = new UserHistoryDetail();
                                    userHistoryDetail.IsEmpty = true;
                                    userHistory.items.Add(userHistoryDetail);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            //Util.Log.ULog.WriteException(ex);
                            ULog.WriteException($"An Exception Occurred in GetUserHistoryProfile: " + ex);
                        }
                        userHistoryList.Add(userHistory);
                    }


                    string jsonProjectAllocations = JsonConvert.SerializeObject(userHistoryList);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserHistoryProfile: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetUsersProfileForComparison")]
        public async Task<IHttpActionResult> GetUsersProfileForComparision(bool IncludeCurrentProject = false
            , string selectedUserId = "", string selectedCompany = "", string selectedRole = "", string selectedType = ""
            , string selectedComplexity = "", string fromDate = "", string toDate = "", string selectedModule = "")
        {
            await Task.FromResult(0);
            if (!string.IsNullOrEmpty(selectedUserId))
            {
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                GlobalRoleManager roleManager = new GlobalRoleManager(context);
                LocationManager locationManager = new LocationManager(context);
                DepartmentManager departmentManager = new DepartmentManager(context);
                CompanyDivisionManager companyDivisionManager = new CompanyDivisionManager(context);
                ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
                TicketManager ticketManager = new TicketManager(context);
                ModuleViewManager moduleManager = new ModuleViewManager(context);
                FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
                UserProfileManager ObjUserProfileManager = new UserProfileManager(context);
                UserProfile userProfile = null;
                List<UserCertificates> spUserCertificateList;
                UserCertificateManager userCertificateManager = new UserCertificateManager(context);
                spUserCertificateList = userCertificateManager.Load(x => x.TenantID == context.TenantID);
                List<UserHistory> userHistoryList = new List<UserHistory>();
                List<ProjectEstimatedAllocation> projectAllocations = null;
                
                DataTable dtTickets = ticketManager.GetAllProjectTickets();
                
                var selectedCompanyList = new List<string>();
                if (!string.IsNullOrWhiteSpace(selectedCompany))
                {
                    selectedCompanyList = selectedCompany.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                }

                var selectedProjectTypeList = new List<string>();
                if (!string.IsNullOrWhiteSpace(selectedType))
                {
                    selectedProjectTypeList = selectedType.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                }

                var selectedRoleList = new List<string>();
                if (!string.IsNullOrWhiteSpace(selectedRole))
                {
                    selectedRoleList = selectedRole.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                }

                var selectedComplexityList = new List<string>();
                if (!string.IsNullOrWhiteSpace(selectedComplexity))
                {
                    selectedComplexityList = selectedComplexity.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                }

                var selectedModuleList = new List<string>();
                if (!string.IsNullOrWhiteSpace(selectedModule))
                {
                    selectedModuleList = selectedModule.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                }

                List<UserProfile> userProfiles = ObjUserProfileManager.GetUsersProfile();
                List<Location> locations = locationManager.Load(x => x.TenantID == context.TenantID);
                List<Department> departments = departmentManager.Load(x => x.TenantID == context.TenantID);
                List<CompanyDivision> companyDivisions = companyDivisionManager.Load(x => x.TenantID == context.TenantID);
                List<GlobalRole> globalRoles = roleManager.Load(x => x.TenantID == context.TenantID);

                foreach (var user in selectedUserId.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList())
                {
                    UserHistory userHistory = new UserHistory();
                    userHistory.UserId = user;
                    userProfile = userProfiles.FirstOrDefault(x => x.Id == Convert.ToString(user));
                    if (userProfile != null)
                    {
                        userHistory.UserName = userProfile.Name;
                        userHistory.key = userProfile.Name;
                        userHistory.Resume = userProfile.Resume;
                        GlobalRole typeGroup = globalRoles.FirstOrDefault(x => x.Id == userProfile.GlobalRoleId);
                        userHistory.UserGlobalRole = typeGroup != null ? typeGroup.Name : "";
                        if (!string.IsNullOrWhiteSpace(userProfile.Location))
                        {
                            Location location = locations.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(userProfile.Location));
                            userHistory.Location = location != null ? location.Title : string.Empty;
                        }
                        if (!string.IsNullOrWhiteSpace(userProfile.Department))
                        {
                            Department department = departments.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(userProfile.Department));
                            if (department != null)
                            {
                                CompanyDivision companyDivision = companyDivisions.FirstOrDefault(x => x.ID == department.DivisionIdLookup.Value);
                                userHistory.Division = companyDivision != null ? companyDivision.Title : "";
                            }
                        }
                        userHistory.Picture = userProfile.Picture;
                        userHistory.UserDatailsControl = LoadUserExperienceTagsControl(user);
                    }
                    if (!string.IsNullOrWhiteSpace(userProfile.UserCertificateLookup))
                    {
                        List<string> certificateIds = userProfile.UserCertificateLookup.Split(',').ToList();
                        userHistory.Certificates = spUserCertificateList?.Where(o => certificateIds.Contains(o.ID.ToString()))
                            ?.Select(o => o.Title)?.ToList();
                    }

                    projectAllocations = cRMProjectAllocationManager.Load(x => x.AssignedTo.EqualsIgnoreCase(user));
                    try
                    {
                        if (projectAllocations != null && projectAllocations.Count > 0)
                        {
                            projectAllocations = projectAllocations.GroupBy(o => new { o.TicketId, o.Type }).Select(o => o.First()).ToList();
                            var projectDetails = (from tkt in dtTickets.AsEnumerable() 
                                                  join alloc in projectAllocations on
                                                  tkt.Field<string>(DatabaseObjects.Columns.TicketId) equals alloc.TicketId
                                                  select new { alloc.Type, alloc.Duration, alloc.SoftAllocation, tkt }
                                                  ).ToList();
                            if (projectDetails != null && projectDetails.Count() > 0)
                            {
                                userHistory.UserProjectCount = projectDetails.Count().ToString();
                                userHistory.UserSoftAllocPer = (projectDetails.Where(o => o.SoftAllocation).Count() / projectDetails.Count * 100).ToString();
                                userHistory.UserHardAllocPer = (projectDetails.Where(o => !o.SoftAllocation).Count() / projectDetails.Count * 100).ToString();
                                if (IncludeCurrentProject == false)
                                {
                                    projectDetails = projectDetails.Where(item => Convert.ToBoolean(item.tkt[DatabaseObjects.Columns.Closed]) == true).ToList();
                                }
                            }
                            if (projectDetails != null && projectDetails.Count() > 0)
                            {
                                projectDetails.Reverse();
                                int filteredProject = 0;
                                foreach (var item in projectDetails)
                                {
                                    UserHistoryDetail userHistoryDetail = new UserHistoryDetail();
                                    if (!string.IsNullOrEmpty(Convert.ToString(item.tkt[DatabaseObjects.Columns.CRMCompanyLookup])))
                                    {
                                        userHistoryDetail.DisplayCompanyName = UGITUtility.TruncateWithEllipsis(fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.CRMCompanyLookup, Convert.ToString(item.tkt[DatabaseObjects.Columns.CRMCompanyLookup])), 20, string.Empty);
                                        userHistoryDetail.CompanyName = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.CRMCompanyLookup, Convert.ToString(item.tkt[DatabaseObjects.Columns.CRMCompanyLookup]));
                                    }
                                    else
                                    {
                                        userHistoryDetail.DisplayCompanyName = string.Empty;
                                        userHistoryDetail.CompanyName = string.Empty;
                                    }
                                        

                                    GlobalRole typeGroup = globalRoles.FirstOrDefault(x => x.Id == item.Type);
                                    if (typeGroup != null)
                                    {
                                        userHistoryDetail.RoleName = typeGroup.Name;
                                        userHistory.Roles += userHistory.Roles != null && !userHistory.Roles.Contains(typeGroup.Name) ? typeGroup.Name + ", " : "";
                                    }

                                    if (!string.IsNullOrWhiteSpace(selectedCompany) && !selectedCompanyList.Contains(userHistoryDetail.CompanyName))
                                    {
                                        continue;
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(item.tkt[DatabaseObjects.Columns.TicketRequestTypeLookup])))
                                        userHistoryDetail.ProjectType = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeLookup, Convert.ToString(item.tkt[DatabaseObjects.Columns.TicketRequestTypeLookup]));
                                    else
                                        userHistoryDetail.ProjectType = string.Empty;

                                    if (!string.IsNullOrWhiteSpace(selectedType) && !selectedProjectTypeList.Contains(userHistoryDetail.ProjectType))
                                    {
                                        continue;
                                    }

                                    userHistoryDetail.TicketId = Convert.ToString(item.tkt[DatabaseObjects.Columns.TicketId]);
                                    userHistoryDetail.ProjectTitle = Convert.ToString(item.tkt[DatabaseObjects.Columns.Title]) +
                                        ":" + UGITUtility.TruncateWithEllipsis(Convert.ToString(item.tkt[DatabaseObjects.Columns.Title]), 50);
                                    userHistoryDetail.Description = Convert.ToString(item.tkt[DatabaseObjects.Columns.Description]);
                                    userHistoryDetail.ProjectComplexityChoice = Convert.ToString(item.tkt[DatabaseObjects.Columns.CRMProjectComplexity]);

                                    if (!string.IsNullOrWhiteSpace(selectedComplexity) && !selectedComplexityList.Contains(userHistoryDetail.ProjectComplexityChoice))
                                    {
                                        continue;
                                    }

                                    
                                    if (!string.IsNullOrWhiteSpace(selectedRole) && !selectedRoleList.Contains(userHistoryDetail.RoleName))
                                    {
                                        continue;
                                    }
                                    userHistoryDetail.Duration = item.Duration;

                                    userHistoryDetail.ContractValue = string.Format("{0:c}", item.tkt[DatabaseObjects.Columns.ApproxContractValue]); //Convert.ToString(item[DatabaseObjects.Columns.ApproxContractValue]);
                                   
                                    userHistoryDetail.PreconStartDate = (item.tkt[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value 
                                        ? Convert.ToDateTime(item.tkt[DatabaseObjects.Columns.PreconStartDate]).ToString("MM/dd/yyyy") 
                                        : item.tkt[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value
                                        ? Convert.ToDateTime(item.tkt[DatabaseObjects.Columns.EstimatedConstructionStart]).ToString("MM/dd/yyyy"):
                                        string.Empty);
                                    
                                    userHistoryDetail.PreconEndDate = (item.tkt[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value ? Convert.ToDateTime(item.tkt[DatabaseObjects.Columns.EstimatedConstructionEnd]).ToString("MM/dd/yyyy") : "");

                                    if (!string.IsNullOrWhiteSpace(userHistoryDetail.PreconStartDate)
                                        && !string.IsNullOrWhiteSpace(userHistoryDetail.PreconEndDate)
                                        && !string.IsNullOrWhiteSpace(fromDate)
                                        && !string.IsNullOrWhiteSpace(toDate))
                                    {
                                        DateTime preconStartDate = DateTime.Parse(userHistoryDetail.PreconStartDate);
                                        DateTime preconEndDate = DateTime.Parse(userHistoryDetail.PreconEndDate);
                                        DateTime fDate = DateTime.Parse(fromDate);
                                        DateTime tDate = DateTime.Parse(toDate);

                                        if (!(preconStartDate <= tDate && fDate <= preconEndDate))
                                        {
                                            continue;
                                        }

                                    }

                                    if (!string.IsNullOrWhiteSpace(selectedModule) && !selectedModuleList.Contains(userHistoryDetail.TicketId.Substring(0,3)))
                                    {
                                        continue;
                                    }
                                    //open code

                                    string viewUrl = string.Empty;
                                    string title = string.Empty;
                                    string func = string.Empty;
                                    string ticketTitle = string.Empty;
                                    string ticketID = string.Empty;
                                    //string sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
                                    string sourceURL = "/default.aspx";
                                    ticketID = userHistoryDetail.TicketId;
                                    ticketTitle = userHistoryDetail.ProjectTitle;
                                    string module = uHelper.getModuleNameByTicketId(ticketID);
                                    DataRow moduleDetail = null;
                                    if (!string.IsNullOrEmpty(module))
                                    {
                                        DataTable moduledt = UGITUtility.ObjectToData(moduleManager.LoadByName(module));
                                        if (moduledt.Rows.Count > 0)
                                            moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(moduleName);
                                    }

                                    if (moduleDetail != null)
                                    {
                                        viewUrl = string.Empty;
                                        if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                                        {
                                            viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                                        }


                                        if (!string.IsNullOrEmpty(ticketTitle))
                                        {
                                            ticketTitle = UGITUtility.TruncateWithEllipsis(ticketTitle, 100, string.Empty);
                                        }

                                        if (!string.IsNullOrEmpty(ticketID))
                                        {
                                            title = string.Format("{0}: ", ticketID);
                                        }
                                        title = string.Format("{0}{1}", title, ticketTitle);

                                    }

                                    userHistoryDetail.Url = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketID), title, sourceURL, 90, 90);

                                    userHistory.items.Add(userHistoryDetail);
                                    filteredProject++;
                                }
                                userHistory.UserFilteredProjectCount = filteredProject.ToString();
                            }
                            else
                            {
                                UserHistoryDetail userHistoryDetail = new UserHistoryDetail();
                                userHistoryDetail.IsEmpty = true;
                                userHistory.items.Add(userHistoryDetail);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Util.Log.ULog.WriteException(ex);
                        ULog.WriteException($"An Exception Occurred in GetUsersProfileForComparision: " + ex);
                    }
                    userHistoryList.Add(userHistory);
                }


                string jsonProjectAllocations = JsonConvert.SerializeObject(userHistoryList);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }

            return Ok();
        }

        [HttpPost]
        [Route("GetPdfUserHistory")]
        public async void GetPdfUserHistory([FromBody] HtmlStringUser htmlStringUser)
        {
            try
            {
                await Task.FromResult(0);
                ExportReport convert = new ExportReport();
                convert.ScriptsEnabled = true;
                convert.ShowFooter = true;
                convert.ShowHeader = true;
                int reportType = 0;
                //string reportTypeString = "pdf";
                string contentType = "Application/pdf";
                reportType = 1;
                //reportTypeString = "png";
                contentType = "image/png";

                convert.ReportType = reportType;
                htmlStringUser.html = string.Format(@"<html><head></head><body>{0}</body></html>", htmlStringUser.html);
                byte[] bytes = convert.GetReportFromHTML(htmlStringUser.html, "");

                var httpResponse = HttpContext.Current.Response;
                httpResponse.Clear();
                httpResponse.ClearContent();
                httpResponse.ClearHeaders();
                httpResponse.Buffer = true;
                httpResponse.ContentType = contentType;
                httpResponse.AddHeader("Content-Disposition", "attachment;filename=uooo");

                httpResponse.BinaryWrite(bytes);
                httpResponse.WriteFile("kooko");
                httpResponse.Write(bytes);
                //httpResponse.WriteFile();

                httpResponse.Flush();
                httpResponse.Close();
                httpResponse.End();

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetPdfUserHistory: " + ex);
            }

            //ApplicationContext context = HttpContext.Current.GetManagerContext();
            //string jsonProjectAllocations = JsonConvert.SerializeObject(htmlStringUser.html);
            //var response = this.Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(jsonProjectAllocations, Encoding.UTF8, "application/json");
            // return Ok();
        }

        [HttpPost]
        [Route("SavePdfToServer")]
        public void SavePdfToServer([FromBody] string htmlData)
        {
            try
            {
                string imageFilePath = string.Empty;
                string pdfFilePath = string.Empty;
                string outputPath = uHelper.GetTempFolderPath();
                imageFilePath = System.IO.Path.Combine(outputPath, "resume_image.png");
                pdfFilePath = System.IO.Path.Combine(outputPath, "UserResume.pdf");
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                using (FileStream fs = new FileStream(imageFilePath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(htmlData);
                        bw.Write(data);
                        bw.Close();
                    }
                }
                using (RichEditDocumentServer server = new RichEditDocumentServer())
                {
                    //Insert an image
                    DocumentImage docImage = server.Document.Images.Append(DocumentImageSource.FromFile(imageFilePath));

                    //Adjust the page width and height to the image's size
                    server.Document.Sections[0].Page.Width = docImage.Size.Width + server.Document.Sections[0].Margins.Right + server.Document.Sections[0].Margins.Left;
                    server.Document.Sections[0].Page.Height = docImage.Size.Height + server.Document.Sections[0].Margins.Top + server.Document.Sections[0].Margins.Bottom;

                    //Export the result to PDF
                    using (FileStream fs = new FileStream(pdfFilePath, FileMode.OpenOrCreate))
                    {
                        server.ExportToPdf(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                //Util.Log.ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in GetDashboardFilters: " + ex);
            }
        }

        public string LoadUserDeatilControl(string userId)
        {
            try
            {
                using (System.Web.UI.Page objPage = new System.Web.UI.Page())
                {
                    UserDetailsPanel userDetailsPanel = (UserDetailsPanel)objPage.LoadControl("~/ControlTemplates/UserDetailsPanel.ascx");
                    userDetailsPanel.Width = Unit.Pixel(900);
                    userDetailsPanel.Height = Unit.Pixel(160);
                    userDetailsPanel.UserId = userId;
                    objPage.Controls.Add(userDetailsPanel);
                    using (StringWriter sWriter = new StringWriter())
                    {
                        HttpContext.Current.Server.Execute(objPage, sWriter, false);
                        return sWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in LoadUserDeatilControl: " + ex);
                return null;
            }
        }

        public string LoadUserExperienceTagsControl(string userId)
        {
            try
            {
                using (System.Web.UI.Page objPage = new System.Web.UI.Page())
                {
                    AddUserExperienceTags addUserExperienceTags = (AddUserExperienceTags)objPage.LoadControl("~/ControlTemplates/RMONE/AddUserExperienceTags.ascx");
                    addUserExperienceTags.UserId = userId;
                    objPage.Controls.Add(addUserExperienceTags);
                    using (StringWriter sWriter = new StringWriter())
                    {
                        HttpContext.Current.Server.Execute(objPage, sWriter, false);
                        return sWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in LoadUserExperienceTagsControl: " + ex);
                return null;
            }
        }

        public class HtmlStringUser
            {

            public string html { get; set; }
        }
    }
}
