using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DevExpress.Data;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity;
using System.Data;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/NewWizard")]
    public class NewWizardController : ApiController
    {
        private ApplicationContext _applicationContext;
        private ModuleViewManager ModuleManager;
        public NewWizardController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            ModuleManager = new ModuleViewManager(_applicationContext);
        }

        [HttpGet]
        [Route("GetDepartments")]
        public HttpResponseMessage GetDepartments()
        {
            try
            {
                DepartmentManager departmentManagerObj = new DepartmentManager(_applicationContext);
                List<Department> lstDepartments = departmentManagerObj.Load();
                if (lstDepartments != null)
                {
                    lstDepartments = lstDepartments.GroupBy(x => x.Title).Select(y => y.First()).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, lstDepartments.OrderBy(x => x.Title));
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDepartments: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("BatchUpdateDepartments")]
        public HttpResponseMessage BatchUpdateDepartments(List<CustomDepartmentResponse> changes)
        {
            DepartmentManager departmentManagerObj = new DepartmentManager(_applicationContext);
            CompanyManager companyManager = new CompanyManager(_applicationContext);
            Department departmentObj = new Department();

            try
            {
                if (changes == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No Changes Applied");

                foreach (var change in changes)
                {
                    long keyID = UGITUtility.StringToLong(change.key);
                    if (keyID > 0)
                        departmentObj = departmentManagerObj.LoadByID(keyID);
                    if (change.type == "remove")
                    {
                        if (departmentObj != null)
                            departmentManagerObj.Delete(departmentObj);
                    }
                    if (change.type == "update")
                    {
                        string jsonchanges = Convert.ToString(change.data);
                        JsonConvert.PopulateObject(jsonchanges, departmentObj);

                        departmentManagerObj.Update(departmentObj);
                    }
                    if (change.type == "insert")
                    {
                        Department newDepartment = new Department();
                        string jsonchanges = Convert.ToString(change.data);
                        JsonConvert.PopulateObject(jsonchanges, newDepartment);
                        Company defaultCompany = companyManager.Load().FirstOrDefault();
                        long companyID = 0;
                        if (defaultCompany == null)
                        {
                            Company newCompany = new Company();
                            newCompany.Title = _applicationContext.TenantAccountId;
                            newCompany.Description = _applicationContext.TenantAccountId + ", Auto Created Company";
                            companyManager.Insert(newCompany);

                            companyID = newCompany.ID;
                        }
                        else
                            companyID = defaultCompany.ID;

                        newDepartment.CompanyIdLookup = companyID;
                        departmentManagerObj.Insert(newDepartment);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, changes);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in BatchUpdateDepartments: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetRoles")]
        public HttpResponseMessage GetRoles()
        {
            try
            {
                GlobalRoleManager roleManager = new GlobalRoleManager(_applicationContext);
                List<GlobalRole> lstRoles = roleManager.Load();
                if (lstRoles != null)
                    return Request.CreateResponse(HttpStatusCode.OK, lstRoles.OrderBy(x => x.Name));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRoles: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("BatchUpdateRoles")]
        public HttpResponseMessage BatchUpdateRoles(List<CustomRolesResponse> changes)
        {
            try
            {
                GlobalRoleManager roleManager = new GlobalRoleManager(_applicationContext);
                GlobalRole roleObj = new GlobalRole();
                if (changes == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No Changes Applied");
                foreach (var change in changes)
                {
                    string roleid = UGITUtility.ObjectToString(change.key);
                    if (!string.IsNullOrEmpty(roleid))
                        roleObj = roleManager.LoadById(roleid);
                    if (change.type == "remove")
                    {
                        if (roleObj != null)
                            roleManager.Delete(roleObj);
                    }
                    if (change.type == "update")
                    {
                        if (roleObj != null)
                        {
                            string jsonchanges = Convert.ToString(change.data);
                            JsonConvert.PopulateObject(jsonchanges, roleObj);

                            roleManager.Update(roleObj);
                        }
                    }
                    if (change.type == "insert")
                    {
                        GlobalRole newRole = new GlobalRole();
                        string jsonchanges = Convert.ToString(change.data);
                        JsonConvert.PopulateObject(jsonchanges, newRole);
                        roleManager.Insert(newRole);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in BatchUpdateRoles: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetJobTitles")]
        public HttpResponseMessage GetJobTitles()
        {
            try
            {
                JobTitleManager jobTitleManagerObj = new JobTitleManager(_applicationContext);
                List<JobTitle> lstJobTitles = jobTitleManagerObj.Load();
                DepartmentManager departmentMGR = new DepartmentManager(_applicationContext);
                GlobalRoleManager roleMGR = new GlobalRoleManager(_applicationContext);

                foreach (JobTitle jobtitle in lstJobTitles)
                {
                    Department departmentObj = departmentMGR.LoadByID(UGITUtility.StringToLong(jobtitle.DepartmentId));
                    if (departmentObj != null)
                    {
                        jobtitle.DepartmentName = departmentObj.Title;
                        jobtitle.DepartmentDescription = departmentObj.DepartmentDescription;
                    }
                    GlobalRole roleObj = roleMGR.LoadById(jobtitle.RoleId);
                    if (roleObj != null)
                        jobtitle.RoleName = roleObj.Name;
                }
                if (lstJobTitles != null)
                    return Request.CreateResponse(HttpStatusCode.OK, lstJobTitles.OrderBy(x => x.Title));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetJobTitles: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        [Route("BatchUpdateJobTitles")]
        public HttpResponseMessage BatchUpdateJobTitles(List<CustomJobTitleResponse> changes)
        {
            try
            {
                JobTitleManager jobTitleManagerObj = new JobTitleManager(_applicationContext);
                JobTitle jobtitleObj = new JobTitle();
                if (changes == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No Changes Applied");
                foreach (var change in changes)
                {
                    long jobtitleId = UGITUtility.StringToLong(change.key);
                    if (jobtitleId > 0)
                        jobtitleObj = jobTitleManagerObj.LoadByID(jobtitleId);
                    if (change.type == "remove")
                    {
                        if (jobtitleObj != null)
                            jobTitleManagerObj.Delete(jobtitleObj);
                    }
                    if (change.type == "update")
                    {
                        if (jobtitleObj != null)
                        {
                            string jsonchanges = Convert.ToString(change.data);
                            JsonConvert.PopulateObject(jsonchanges, jobtitleObj);

                            jobTitleManagerObj.Update(jobtitleObj);
                        }
                    }
                    if (change.type == "insert")
                    {
                        JobTitle newJobTitle = new JobTitle();
                        string jsonchanges = Convert.ToString(change.data);
                        JsonConvert.PopulateObject(jsonchanges, newJobTitle);
                        jobTitleManagerObj.Insert(newJobTitle);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, changes);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in BatchUpdateJobTitles: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetUserProfiles")]
        public HttpResponseMessage GetUserProfiles()
        {
            try
            {
                UserProfileManager userProfileManager = new UserProfileManager(_applicationContext);
                JobTitleManager jobTitleMGR = new JobTitleManager(_applicationContext);
                List<UserProfile> lstUseprofile = userProfileManager.GetEnabledUsers().Where(x => !x.UserName.EqualsIgnoreCase("SuperAdmin")).ToList();

                DepartmentManager departmentMGR = new DepartmentManager(_applicationContext);
                foreach (UserProfile item in lstUseprofile)
                {
                    Department departmentObj = departmentMGR.LoadByID(UGITUtility.StringToLong(item.Department));
                    if (departmentObj != null)
                        item.DepartmentName = departmentObj.Title;
                    JobTitle jobTitleObj = jobTitleMGR.LoadByID(UGITUtility.StringToLong(item.JobTitleLookup));
                    if (jobTitleObj != null)
                        item.JobProfile = jobTitleObj.Title;
                }
                if (lstUseprofile != null)
                    return Request.CreateResponse(HttpStatusCode.OK, lstUseprofile.OrderBy(x => x.Name));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserProfiles: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        [Route("GetManagerUserProfiles")]
        public HttpResponseMessage GetManagerUserProfiles()
        {
            try
            {
                UserProfileManager userProfileManager = new UserProfileManager(_applicationContext);
                List<UserProfile> lstUseprofile = userProfileManager.GetEnabledUsers().Where(x => !x.UserName.EqualsIgnoreCase("SuperAdmin")).ToList();
                if (lstUseprofile != null)
                    return Request.CreateResponse(HttpStatusCode.OK, lstUseprofile.OrderBy(x => x.Name));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetManagerUserProfiles: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        [Route("BatchUpdateUsers")]
        public HttpResponseMessage BatchUpdateUsers(List<CustomUsersResponse> changes)
        {
            try
            {
                UserProfileManager umanager = _applicationContext.UserManager;
                UserProfile userObj = new UserProfile();
                if (changes == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No Changes Applied");
                foreach (var change in changes)
                {
                    string userid = UGITUtility.ObjectToString(change.key);
                    if (!string.IsNullOrEmpty(userid))
                        userObj = umanager.LoadById(userid);
                    if (change.type == "update")
                    {
                        if (userObj != null)
                        {
                            string jsonchanges = Convert.ToString(change.data);
                            JsonConvert.PopulateObject(jsonchanges, userObj);

                            JobTitleManager jobTitleManager = new JobTitleManager(HttpContext.Current.GetManagerContext());
                            JobTitle jobTitleObj = jobTitleManager.LoadByID(userObj.JobTitleLookup);
                            userObj.GlobalRoleId = null;
                            if (jobTitleObj != null)
                            {
                                userObj.GlobalRoleId = jobTitleObj.RoleId;
                                userObj.JobProfile = jobTitleObj.Title;
                            }

                            IdentityResult result = umanager.Update(userObj);
                        }
                    }
                    if (change.type == "insert")
                    {
                        UserProfile newUserProfile = new UserProfile();
                        string jsonchanges = Convert.ToString(change.data);
                        JsonConvert.PopulateObject(jsonchanges, newUserProfile);
                        newUserProfile.UserName = newUserProfile.UserName.Replace(" ", "");
                        newUserProfile.NotificationEmail = newUserProfile.Email;
                        newUserProfile.UGITStartDate = DateTime.Now;
                        if (newUserProfile.JobTitleLookup > 0)
                        {
                            JobTitleManager jobTitleManager = new JobTitleManager(HttpContext.Current.GetManagerContext());
                            JobTitle jobTitleObj = jobTitleManager.LoadByID(newUserProfile.JobTitleLookup);
                            newUserProfile.GlobalRoleId = null;
                            if (jobTitleObj != null)
                                newUserProfile.GlobalRoleId = jobTitleObj.RoleId;
                        }

                        newUserProfile.isRole = false;
                        newUserProfile.Enabled = true;
                        string password = umanager.GeneratePassword();
                        newUserProfile.TenantID = _applicationContext.TenantID;

                        IdentityResult result = umanager.Create(newUserProfile, password);
                        //if(result.Succeeded)
                        //    return Request.CreateResponse(HttpStatusCode.OK, changes);
                        //else
                        //    return Request.CreateResponse(HttpStatusCode.BadRequest, changes);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, changes);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in BatchUpdateUsers: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetProjects")]
        public HttpResponseMessage GetProjects()
        {
            try
            {
                UserProfileManager userProfileManager = new UserProfileManager(_applicationContext);
                TicketManager ticketMgr = new TicketManager(_applicationContext);
                ModuleViewManager moduleMgr = new ModuleViewManager(_applicationContext);
                List<UGITModule> lstprojectmodules = moduleMgr.Load(x => x.ModuleType == ModuleType.Project);
                DataTable dtAllProjects = new DataTable();
                //    List<string> viewColumns = new List<string>() { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ID,
                //DatabaseObjects.Columns.Description, DatabaseObjects.Columns.TicketProjectManager};
                List<ProjectClass> lstProjects = new List<ProjectClass>();
                foreach (UGITModule item in lstprojectmodules)
                {
                    List<string> viewColumns = new List<string>() { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ID,
            DatabaseObjects.Columns.Description, DatabaseObjects.Columns.TicketProjectManager};
                    if (item.ModuleName == ModuleNames.CPR || item.ModuleName == ModuleNames.OPM || item.ModuleName == ModuleNames.CNS)
                    {
                        viewColumns.Add(DatabaseObjects.Columns.EstimatedConstructionStart);
                        viewColumns.Add(DatabaseObjects.Columns.EstimatedConstructionEnd);
                    }
                    if (item.ModuleName == ModuleNames.PMM || item.ModuleName == ModuleNames.NPR)
                    {
                        viewColumns.Add(DatabaseObjects.Columns.TicketActualStartDate);
                        viewColumns.Add(DatabaseObjects.Columns.TicketDesiredCompletionDate);
                    }
                    DataTable tickets = ticketMgr.GetAllTickets(item, viewColumns);
                    if (tickets != null && tickets.Rows.Count > 0)
                    {
                        tickets.Columns.Add("ModuleType");
                        tickets.Columns["ModuleType"].DefaultValue = item.ModuleName;
                        foreach (DataRow row in tickets.Rows)
                        {
                            ProjectClass projectClassObj = new ProjectClass();
                            projectClassObj.ProjectName = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Title]);
                            projectClassObj.ProjectManagerUser = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketProjectManager]);
                            projectClassObj.Description = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.Description]);
                            projectClassObj.TicketId = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketId]);
                            projectClassObj.ModuleType = uHelper.getModuleNameByTicketId(projectClassObj.TicketId);

                            if (item.ModuleName == ModuleNames.CPR || item.ModuleName == ModuleNames.OPM || item.ModuleName == ModuleNames.CNS)
                            {
                                projectClassObj.StartDate = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                projectClassObj.EndDate = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                            }
                            if (item.ModuleName == ModuleNames.PMM)
                            {
                                projectClassObj.StartDate = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketActualStartDate]);
                                projectClassObj.EndDate = UGITUtility.ObjectToString(row[DatabaseObjects.Columns.TicketDesiredCompletionDate]);
                            }
                            lstProjects.Add(projectClassObj);
                        }
                    }
                }
                if (lstProjects != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, lstProjects.OrderBy(x => x.ProjectName));
                }
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetProjects: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("BatchUpdateProjects")]
        public HttpResponseMessage BatchUpdateProjects(List<CustomProjectResponse> changes)
        {
            try
            {
                UserProfileManager userProfileManager = new UserProfileManager(_applicationContext);
                UserProfileManager umanager = _applicationContext.UserManager;
                TicketManager ticketMgr = new TicketManager(_applicationContext);
                Ticket ticketObj;
                if (changes == null)
                    return Request.CreateResponse(HttpStatusCode.OK, "No Changes Applied");
                foreach (var change in changes)
                {
                    ProjectClass projectObj = new ProjectClass();
                    string jsonString = UGITUtility.ObjectToString(change.data);
                    JsonConvert.PopulateObject(jsonString, projectObj);
                    if (change.type == "update")
                    {
                        string moduleType = uHelper.getModuleNameByTicketId(change.key);
                        if (moduleType == ModuleNames.CPR || moduleType == ModuleNames.CNS || moduleType == ModuleNames.OPM || moduleType == ModuleNames.PMM)
                        {
                            UGITModule cprmodule = ModuleManager.LoadByName(moduleType);
                            DataTable ticketRow = ticketMgr.GetCachedModuleTableSchema(cprmodule);

                            DataRow cprRow = ticketMgr.GetByTicketID(cprmodule, change.key);
                            ticketObj = new Ticket(_applicationContext, moduleType);
                            if (!string.IsNullOrEmpty(projectObj.ProjectName))
                                cprRow[DatabaseObjects.Columns.Title] = projectObj.ProjectName;
                            if (!string.IsNullOrEmpty(projectObj.ProjectManagerUser))
                                cprRow[DatabaseObjects.Columns.TicketProjectManager] = projectObj.ProjectManagerUser;
                            if (!string.IsNullOrEmpty(projectObj.Description))
                                cprRow[DatabaseObjects.Columns.Description] = projectObj.Description;
                            if (!string.IsNullOrEmpty(projectObj.StartDate))
                            {
                                if (moduleType == ModuleNames.PMM)
                                {
                                    cprRow[DatabaseObjects.Columns.TicketActualStartDate] = UGITUtility.StringToDateTime(projectObj.StartDate);
                                }
                                else
                                {
                                    cprRow[DatabaseObjects.Columns.EstimatedConstructionStart] = UGITUtility.StringToDateTime(projectObj.StartDate);
                                    cprRow[DatabaseObjects.Columns.PreconStartDate] = UGITUtility.StringToDateTime(projectObj.StartDate);
                                }
                            }
                            if (!string.IsNullOrEmpty(projectObj.EndDate))
                            {
                                if (moduleType == ModuleNames.PMM)
                                {
                                    cprRow[DatabaseObjects.Columns.TicketDesiredCompletionDate] = UGITUtility.StringToDateTime(projectObj.EndDate);
                                }
                                else
                                { cprRow[DatabaseObjects.Columns.EstimatedConstructionEnd] = UGITUtility.StringToDateTime(projectObj.EndDate); }
                            }
                            cprRow[DatabaseObjects.Columns.Closed] = false;
                            ticketObj.CommitChanges(cprRow);
                        }
                    }
                    if (change.type == "insert")
                    {
                        //string jsonString = UGITUtility.ObjectToString(change.data);
                        //ProjectClass newProject = new ProjectClass();
                        //JsonConvert.PopulateObject(jsonString, newProject);
                        string moduleType = projectObj.ModuleType;
                        if (moduleType == ModuleNames.CPR || moduleType == ModuleNames.CNS || moduleType == ModuleNames.OPM || moduleType == ModuleNames.PMM)
                        {
                            UGITModule cprmodule = ModuleManager.LoadByName(moduleType);
                            DataTable ticketRow = ticketMgr.GetDatabaseTableSchema(cprmodule.ModuleTable);

                            DataRow cprRow = ticketRow.NewRow();
                            ticketObj = new Ticket(_applicationContext, moduleType);
                            cprRow[DatabaseObjects.Columns.Title] = projectObj.ProjectName;
                            cprRow[DatabaseObjects.Columns.TicketProjectManager] = projectObj.ProjectManagerUser;
                            cprRow[DatabaseObjects.Columns.Description] = projectObj.Description;
                            if (moduleType != ModuleNames.PMM)
                            {
                                cprRow[DatabaseObjects.Columns.EstimatedConstructionStart] = UGITUtility.StringToDateTime(projectObj.StartDate);
                                cprRow[DatabaseObjects.Columns.PreconStartDate] = UGITUtility.StringToDateTime(projectObj.StartDate);
                                cprRow[DatabaseObjects.Columns.EstimatedConstructionEnd] = UGITUtility.StringToDateTime(projectObj.EndDate);
                            }
                            else
                            {
                                cprRow[DatabaseObjects.Columns.TicketActualStartDate] = UGITUtility.StringToDateTime(projectObj.StartDate);
                                cprRow[DatabaseObjects.Columns.TicketDesiredCompletionDate] = UGITUtility.StringToDateTime(projectObj.EndDate);
                            }
                            cprRow[DatabaseObjects.Columns.Closed] = false;
                            ticketObj.Create(cprRow, _applicationContext.CurrentUser);
                            string resultmsg = ticketObj.CommitChanges(cprRow);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, changes);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in BatchUpdateProjects: " + ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("IsDepartmentDuplicate")]
        public async Task<IHttpActionResult> IsDepartmentDuplicate(string departmentName, string ID)
        {
            await Task.FromResult(0);
            try
            {
                bool result = true;
                DepartmentManager departmentMGR = new DepartmentManager(_applicationContext);
                Department departmentObj = departmentMGR.Load(x => x.Title == departmentName).FirstOrDefault();
                if (departmentObj != null)
                {
                    if (UGITUtility.StringToLong(ID) != departmentObj.ID)
                        result = false;
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in IsDepartmentDuplicate: " + ex);
                return InternalServerError();
            }
        }
    }

    public class CustomDepartmentResponse
    {
        public object data { get; set; }
        public string key { get; set; }
        public string type { get; set; }
    }
    
    public class CustomRolesResponse
    {
        public object data { get; set; }
        public string key { get; set; }
        public string type { get; set; }
    }
    public class CustomJobTitleResponse
    {
        public object data { get; set; }
        public string key { get; set; }
        public string type { get; set; }
    }
    public class CustomUsersResponse
    {
        public object data { get; set; }
        public string key { get; set; }
        public string type { get; set; }
    }

    public class ProjectClass
    {
        public long ID { get; set; }
        public string TicketId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectManagerUser { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ModuleType { get; set; }
    }

    public class CustomProjectResponse
    {
        public object data { get; set; }
        public string key { get; set; }
        public string type { get; set; }
    }
}
