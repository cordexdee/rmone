using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using Newtonsoft.Json;
using System.Text;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/rmmcard")]
    public class RMMCardController : ApiController
    {
        private ApplicationContext _applicationContext = null;
        bool isResourceAdmin = false;
        UserProfileManager ObjUserProfileManager = null;
        ConfigurationVariableManager ObjConfigurationVariableManager = null;
        private bool allowAllocationForSelf;
        private List<ResourceCard> lstresources = null;
        private AsistantsResponse asistantsResponse = null;
        //for load assistant
        List<UserProfile> selectedUsersList = new List<UserProfile>();
        string hdnParentOf = string.Empty;
        string hdnChildOf = string.Empty;
        ResourceAllocationManager allocManager = null;
        ResourceTimeSheetSignOffManager resourceTimeSheetSignOffManager = null;
        List<ResourceTimeSheetSignOff> resourceTimeSheetSignOffs = null;
        public RMMCardController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            ObjConfigurationVariableManager = new ConfigurationVariableManager(_applicationContext);
            ObjUserProfileManager = new UserProfileManager(_applicationContext);
            allocManager = new ResourceAllocationManager(_applicationContext);
            resourceTimeSheetSignOffManager = new ResourceTimeSheetSignOffManager(_applicationContext);

            lstresources = new List<ResourceCard>();
            resourceTimeSheetSignOffs = resourceTimeSheetSignOffManager.Load(x => x.Deleted == false).ToList();
        }

        [HttpGet]
        [Route("GetResourceManager")]
        public async Task<IHttpActionResult> GetResourceManager()
        {
            await Task.FromResult(0);
            try
            {
                UserProfile currentUser = _applicationContext.CurrentUser;
                isResourceAdmin = ObjUserProfileManager.IsUGITSuperAdmin(currentUser) || ObjUserProfileManager.IsResourceAdmin(currentUser);
                allowAllocationForSelf = ObjConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowAllocationForSelf);
                if (isResourceAdmin)
                {
                    List<UserProfile> userCollection = ObjUserProfileManager.GetUsersProfile().OrderBy(x => x.Name).ToList();
                    if (userCollection != null)
                    {
                        foreach (UserProfile user in userCollection)
                        {
                            //cmbResourceManager.Items.Add(new ListEditItem(user.Name, user.Id.ToString()));
                            lstresources.Add(new ResourceCard()
                            {
                                Id = user.Id.ToString(),
                                Name = user.Name
                            });
                        }
                    }
                }
                else
                {
                    List<UserProfile> userCollection = new List<UserProfile>();
                    userCollection = ObjUserProfileManager.LoadAuthorizedUsers(allowAllocationForSelf || currentUser.IsManager); // ObjUserProfileManager.GetUserByManager(HttpContext.Current.CurrentUser().Id);
                    userCollection = userCollection.OrderBy(x => x.Name).ToList();

                    if (userCollection != null)
                    {
                        foreach (UserProfile user in userCollection)
                        {
                            lstresources.Add(new ResourceCard()
                            {
                                Id = user.Id.ToString(),
                                Name = user.Name
                            });
                        }
                    }
                }
                if (lstresources == null)
                    return NotFound();

                string jsonResource = JsonConvert.SerializeObject(lstresources);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonResource, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetResourceManager: " + ex);
                return InternalServerError();
            }


        }

        [HttpGet]
        [Route("GetLoadAsistants")]
        public async Task<IHttpActionResult> GetLoadAsistants(string hdnChildOf, string hdnParentOf, string Year)
        {
            await Task.FromResult(0);
            try
            {
                this.hdnChildOf = hdnChildOf;
                this.hdnParentOf = hdnParentOf;
                LoadAsistantsAndAllocation(Year);
                string jsonAsistantsResponse = JsonConvert.SerializeObject(asistantsResponse);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonAsistantsResponse, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetLoadAsistants: " + ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("GetManager")]
        public async Task<IHttpActionResult> GetManager(string hdnChildOf, string hdnParentOf, string Year)
        {
            await Task.FromResult(0);
            try
            {
                this.hdnChildOf = hdnChildOf;
                this.hdnParentOf = hdnParentOf;
                UserProfile manager = new UserProfile();
                AssitantsResource assitantsResource = new AssitantsResource();
                DateTime startDate = new DateTime(UGITUtility.StringToInt(Year), 1, 1);
                DateTime endDate = new DateTime(UGITUtility.StringToInt(Year), 12, 31);
                //if (hdnParentOf == string.Empty && hdnChildOf == string.Empty)
                if (string.IsNullOrEmpty(hdnParentOf) && string.IsNullOrEmpty(hdnChildOf))
                {
                    UserProfile userInfo = ObjUserProfileManager.LoadById(_applicationContext.CurrentUser.Id);
                    if (userInfo != null)
                    {
                        manager = userInfo;
                    }
                }
                else if (hdnParentOf != string.Empty && hdnParentOf != null)
                {
                    string userId = hdnParentOf;
                    UserProfile userInfo = ObjUserProfileManager.GetUserById(userId);

                    if (userInfo != null)
                    {

                        if (!string.IsNullOrWhiteSpace(userInfo.ManagerID))
                        {
                            UserProfile managerInfo = ObjUserProfileManager.LoadById(userInfo.ManagerID);
                            if (managerInfo != null)
                            {
                                manager = managerInfo;
                            }
                        }
                    }
                }
                else if (hdnChildOf != string.Empty && hdnChildOf != null)
                {
                    string userId = hdnChildOf;
                    UserProfile userInfo = ObjUserProfileManager.GetUserById(userId);
                    if (userInfo != null)
                    {
                        manager = userInfo;

                    }
                }

                if (manager != null)
                {
                    List<UsersEmail> userInfo = new List<UsersEmail>();
                    List<string> toolTip = userInfo.Select(x => x.userToolTip).ToList();
                    string sourceUrl = "";
                    string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&UpdateUser=1&RMMCardView=1", manager.Id));
                    string userName = Convert.ToString(manager.Name);

                    string usrLinkUr = string.Format("<div class='user-manager-name'>{5}</div> <div class='user-edit-icon'> <img title='{4}' src='/Content/images/edit-pencil.png' class='jqtooltip' onclick='javascript:event.cancelBubble=true;window.parent.UgitOpenPopupDialog(\"{2}\",\"\", \"User Details: {1}\", \"615px\",\"90\", false,\"{3}\")'/></div>{0}",
                                                    false ? string.Format("&nbsp;<img class='movedown-icon' onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' style='width:20px;' src='/Content/images/downarrow_new.png' alt='Down' title='Move down'/>", manager.Id) : "&nbsp;&nbsp;&nbsp;",
                                                    userName.Replace("'", string.Empty), userLinkUrl, sourceUrl, string.Join(" ", toolTip).Replace("'", string.Empty), userName);

                    string managerlink = string.Empty;
                    if (!string.IsNullOrEmpty(manager.ManagerID))
                    {
                        managerlink = $"<div class='users-manager'><img src='/content/images/uparrow_new.png' style='width:20px;' title='Move up' onclick='event.cancelBubble=true;MoveDown(\"{manager.ManagerID}\")' /></div>";
                    }

                    UserProfile currentUser = _applicationContext.CurrentUser;
                    if (!ObjUserProfileManager.IsAdmin(currentUser) && !ObjUserProfileManager.IsResourceAdmin(currentUser))
                    {
                        usrLinkUr = string.Format("<div class='user-manager-name'>{2}</div> <div class='user-edit-icon-disabled'> <img title='{1}' src='/Content/images/edit-pencil.png' class='jqtooltip' style='display:none'/></div>{0}",
                                                   false ? string.Format("&nbsp;<img class='movedown-icon' onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' src='/Content/images/downarrow_new.png' style='width:20px;' alt='Down' title='Move down'/>", manager.Id) : "&nbsp;&nbsp;&nbsp;",
                                                    string.Join(" ", toolTip).Replace("'", string.Empty), userName);

                    }

                    //ResourceProjectComplexityManager cpxManager = new ResourceProjectComplexityManager(_applicationContext);
                    //var rComplixities = cpxManager.Load().ToLookup(x => x.UserId);
                    //int projectcount = rComplixities[manager.Id].Sum(x => x.Count);
                    int projectcount = 0;
                    //System.Data.DataTable dt = GetTableDataManager.GetTableDataUsingQuery($"GetUsersProjectCount @resource='{manager.Id}',@TenantId='{_applicationContext.TenantID}',@includeClosedProjects=0");
                    System.Data.DataTable dt = GetTableDataManager.GetTableDataUsingQuery($"GetUsersAllocationCount @resource='{manager.Id}',@TenantId='{_applicationContext.TenantID}',@startdate='{startDate}',@enddate='{endDate}',@includeClosedProjects=0");
                    if (dt != null && dt.Rows.Count > 0)
                        projectcount = Convert.ToInt32(dt.Rows[0][0]);

                    int pendingApprovalCount = resourceTimeSheetSignOffs.Where(x => x.Resource == manager.Id && x.SignOffStatus == Constants.PendingApproval).Count();

                    assitantsResource = new AssitantsResource()
                    {
                        ID = manager.Id,
                        Title = manager.Name,
                        JobTitle = manager.JobProfile,
                        //BudgetLookup = Convert.ToString(budgetCategory),
                        //ResourceHourlyRate = user.HourlyRate,
                        //DepartmentLookup = Convert.ToString(user.Department),
                        // LocationLookup = Convert.ToString(user.Location),
                        //Allocation = "",
                        // IsAssistantExist = isAssistantExist,
                        AllocationBar = CreateAllocationBar(manager.Id, Year),
                        UsrEditLinkUrl = usrLinkUr,
                        ManagerLinkUrl = managerlink,
                        ProjectCount = projectcount,
                        // Consultant = Convert.ToString(consultantText).TrimStart(),
                        AssitantCount = ObjUserProfileManager.GetUserByManager(manager.Id) != null ? ObjUserProfileManager.GetUserByManager(manager.Id).Where(x => x.Enabled).Count() : 0,
                        imageUrl = !string.IsNullOrEmpty(manager.Picture) && System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(manager.Picture)) ? manager.Picture : "/Content/Images/userNew.png",
                        PendingApprovalCount = pendingApprovalCount
                    };
                }
                string jsonAssitantsResource = JsonConvert.SerializeObject(assitantsResource);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(jsonAssitantsResource, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetManager: " + ex);
                return InternalServerError();
            }
        }

        private void LoadAsistantsAndAllocation(string year)
        {

            asistantsResponse = new AsistantsResponse();
            //ResourceColumnsSetting();
            //moveUp.Visible = false;
            try
            {
                if (hdnParentOf == string.Empty && hdnChildOf == string.Empty)
                {
                    UserProfile userInfo = ObjUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);
                    selectedUsersList.Clear();
                    if (userInfo != null)
                    {
                        selectedUsersList.Add(userInfo);
                        // manager = userInfo;
                        // moveUp.Visible = false;
                        // CreateResourceTable(HttpContext.Current.CurrentUser().Id);
                    }
                }
                else if (hdnParentOf != string.Empty && hdnParentOf != null)
                {
                    string userId = hdnParentOf;
                    UserProfile userInfo = ObjUserProfileManager.GetUserById(userId);

                    if (userInfo != null)
                    {
                        if (string.IsNullOrEmpty(userInfo.ManagerID) || userInfo.Id == userInfo.ManagerID || (!isResourceAdmin && userInfo.ManagerID == _applicationContext.CurrentUser.Id))
                        {
                            // moveUp.Visible = false;
                        }
                        else
                        {
                            UserProfile currentuserManager = ObjUserProfileManager.LoadById(userInfo.ManagerID);
                            if (currentuserManager != null && !string.IsNullOrWhiteSpace(currentuserManager.ManagerID))
                            {
                                // moveUp.Visible = true;
                                // moveUp.Attributes.Add("onclick", string.Format("MoveUp(\"{0}\")", userInfo.ManagerID));
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(userInfo.ManagerID))
                        {
                            UserProfile managerInfo = ObjUserProfileManager.LoadById(userInfo.ManagerID);
                            if (managerInfo != null)
                            {
                                selectedUsersList.Clear();
                                selectedUsersList.Add(managerInfo);
                                //manager = managerInfo;
                                CreateResourceTable(userInfo.ManagerID, year);
                            }
                        }
                    }
                }
                else if (hdnChildOf != string.Empty && hdnChildOf != null)
                {
                    // moveUp.Attributes.Add("onclick", string.Format("MoveUp(\"{0}\")", hdnChildOf.Value));
                    string userId = hdnChildOf;
                    UserProfile userInfo = ObjUserProfileManager.GetUserById(userId);

                    if (userInfo != null)
                    {

                        if ((!isResourceAdmin && userInfo.Id == _applicationContext.CurrentUser.Id) || string.IsNullOrEmpty(userInfo.ManagerID) || userInfo.Id == userInfo.ManagerID)
                        {
                            //moveUp.Visible = false;
                        }
                        else
                        {
                            // moveUp.Visible = true;
                            // moveUp.Attributes.Add("onclick", string.Format("MoveUp(\"{0}\")", hdnChildOf.Value));
                        }

                        selectedUsersList.Clear();
                        selectedUsersList.Add(userInfo);
                        //manager = userInfo;
                        CreateResourceTable(userId, year);
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in LoadAsistantsAndAllocation: " + ex);
            }
            //Set selected resource id in cockies
            //if (manager != null && !string.IsNullOrEmpty(manager.Id))
            //{
            //    UGITUtility.CreateCookie(Response, resourceFromCookies, manager.Id.ToString());
            //}
        }

        private void CreateResourceTable(string userId, string year)
        {
            try
            {
                List<UserProfile> userProfiles = ObjUserProfileManager.GetUserByManager(userId).Where(x => x.Enabled).ToList();
                DateTime startDate = new DateTime(UGITUtility.StringToInt(year), 1, 1);
                DateTime endDate = new DateTime(UGITUtility.StringToInt(year), 12, 31);

                int pendingApprovalCount = 0;

                if (userProfiles != null && userProfiles.Count > 0)
                {
                    List<UserProfile> userInfoList = ObjUserProfileManager.GetUsersProfile();
                    foreach (UserProfile user in userProfiles)
                    {
                        StringBuilder consultantText = new StringBuilder();

                        if (user.IsIT)
                            consultantText.Append("IT");

                        if (user.IsConsultant)
                            consultantText.Append(" Consultant");
                        else
                            consultantText.Append(" Staff");

                        if (user.IsManager)
                            consultantText.Append(" (Manager)");

                        string budgetCategory = Convert.ToString(Convert.ToInt32(user.BudgetCategory) > 0 ? user.BudgetCategory : 0);

                        bool isAssistantExist = false;
                        //code for input checkbox
                        //bool isSelected = false;

                        //Check whether assistant of user exist or not
                        List<UserProfile> userCollection = userInfoList.Where(x => x.ManagerID == user.Id).ToList();
                        if (userCollection != null && userCollection.Count > 0)
                            isAssistantExist = true;

                        pendingApprovalCount = resourceTimeSheetSignOffs.Where(x => x.Resource == user.Id && x.SignOffStatus == Constants.PendingApproval).Count();

                        List<UsersEmail> userInfo = new List<UsersEmail>();
                        List<string> toolTip = userInfo.Select(x => x.userToolTip).ToList();
                        string sourceUrl = "";
                        string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&RMMCardView=1", user.Id));
                        string userName = Convert.ToString(user.Name);
                        //string usrLinkUr = string.Format("<button title='{4}' class='jqtooltip' type='button' onclick ='javascript:event.cancelBubble=true;window.parent.UgitOpenPopupDialog(\"{2}\",\"\", \"User Details: {1}\", \"615px\",\"90\", false,\"{3}\")'>{5}</button>{0}",
                        //                                isAssistantExist ? string.Format("&nbsp;<img onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' src='/Content/images/downarrow.png' alt='Down' title='Move down'/>", user.Id) : "&nbsp;&nbsp;&nbsp;",
                        //                                userName.Replace("'", string.Empty), userLinkUrl, sourceUrl, string.Join(" ", toolTip).Replace("'", string.Empty), userName);

                        string usrLinkUr = string.Format("<div class='cardResource-edit-icon'> <img title='{4}' src='/Content/images/edit-pencil.png' class='jqtooltip' onclick='javascript:event.cancelBubble=true;window.parent.UgitOpenPopupDialog(\"{2}\",\"\", \"User Details: {1}\", \"615px\",\"90\", false,\"{3}\")'/>{0}</div>",
                                                        isAssistantExist ? string.Format("&nbsp;<img class='movedown-icon' onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' src='/Content/images/downarrow_new.png' style='width:20px;' alt='Down' title='Move down'/>", user.Id) : "&nbsp;&nbsp;&nbsp;",
                                                        userName.Replace("'", string.Empty), userLinkUrl, sourceUrl, string.Join(" ", toolTip).Replace("'", string.Empty), userName);

                        UserProfile currentUser = _applicationContext.CurrentUser;
                        if (!ObjUserProfileManager.IsAdmin(currentUser) && !ObjUserProfileManager.IsResourceAdmin(currentUser))
                        {
                            usrLinkUr = string.Format("<div class='cardResource-edit-icon-disabled'> <img title='{1}' src='/Content/images/edit-pencil.png' class='jqtooltip' style='display:none'/>{0}</div>",
                                                        isAssistantExist ? string.Format("&nbsp;<img class='movedown-icon disable-state' onclick='event.cancelBubble=true;MoveDown(\"{0}\")' align='absmiddle' src='/Content/images/downarrow_new.png' style='width:20px;' alt='Down' title='Move down'/>", user.Id) : "&nbsp;&nbsp;&nbsp;",
                                                         string.Join(" ", toolTip).Replace("'", string.Empty), userName); ;
                        }
                        string allocationBar = CreateAllocationBar(user.Id, year);

                        //ResourceProjectComplexityManager cpxManager = new ResourceProjectComplexityManager(_applicationContext);
                        //var rComplixities = cpxManager.Load().ToLookup(x => x.UserId);
                        //int projectcount = rComplixities[user.Id].Sum(x => x.Count);

                        int projectcount = 0;
                        System.Data.DataTable dt = GetTableDataManager.GetTableDataUsingQuery($"GetUsersAllocationCount @resource='{user.Id}',@TenantId='{_applicationContext.TenantID}', @startdate='{startDate}',@enddate='{endDate}',@includeClosedProjects=0");
                        if (dt != null && dt.Rows.Count > 0)
                            projectcount = Convert.ToInt32(dt.Rows[0][0]);

                        if (user != null && (user.ManagerID == null || user.Id != ObjUserProfileManager.LoadById(user.Id).ManagerID))
                        {
                            asistantsResponse.lstAssitantsResource.Add(new AssitantsResource()
                            {
                                ID = user.Id,
                                Title = user.Name,
                                JobTitle = user.JobProfile,
                                BudgetLookup = Convert.ToString(budgetCategory),
                                ResourceHourlyRate = user.HourlyRate,
                                DepartmentLookup = Convert.ToString(user.Department),
                                LocationLookup = Convert.ToString(user.Location),
                                Allocation = "",
                                IsAssistantExist = isAssistantExist,
                                AllocationBar = allocationBar,
                                UsrEditLinkUrl = usrLinkUr,
                                Consultant = Convert.ToString(consultantText).TrimStart(),
                                //imageUrl = string.IsNullOrEmpty(user.Picture) ? "Content/images/download.png" : user.Picture,
                                imageUrl = !string.IsNullOrEmpty(user.Picture) && System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(user.Picture)) ? user.Picture : "/Content/Images/userNew.png",
                                ProjectCount = projectcount,
                                PendingApprovalCount = pendingApprovalCount
                            });
                            if (asistantsResponse.lstAssitantsResource != null && asistantsResponse.lstAssitantsResource.Count > 0)
                                asistantsResponse.lstAssitantsResource = asistantsResponse.lstAssitantsResource.OrderBy(x => x.Title).ToList();

                            //result.Rows.Add(user.Id, user.Name, Convert.ToString(budgetCategory), user.HourlyRate, Convert.ToString(user.Department), Convert.ToString(user.Location), "", isAssistantExist, Convert.ToString(consultantText).TrimStart());
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateResourceTable: " + ex);
            }
        }

        private string CreateAllocationBar(string userId, string year)
        {
            StringBuilder bar = new StringBuilder();
            try
            {
                DateTime startDate = new DateTime(UGITUtility.StringToInt(year), 1, 1);
                DateTime endDate = new DateTime(UGITUtility.StringToInt(year), 12, 31);
                double pctAllocation = allocManager.AllocationPercentage(userId, startDate, endDate, false);
                double percentage = 0;
                string progressBarClass = "progressbar";
                string empltyProgressBarClass = "rmmEmpty-ProgressBar emptyProgressBar";
                percentage = (long)Math.Round(pctAllocation);
                if (percentage > 100)
                {
                    progressBarClass = "progressbarhold";
                    bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:8px;z-index:1; color:#FFF;'>Avg Util: {2}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:100%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
                }
                else
                {
                    bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:100%;text-align:center;top:8px;z-index:1;'>Avg Util: {2}%</strong><div class='{0}' style='float:left; width:100%;'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in CreateAllocationBar: " + ex);
            }
            return bar.ToString();
        }

    }
}