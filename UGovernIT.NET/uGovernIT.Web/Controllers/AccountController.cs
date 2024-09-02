using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApplication3.Models;
using uGovernIT.Web.Models;
using uGovernIT.Web.Results;
using uGovernIT.Web.Providers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Linq;
using System.Data;
using uGovernIT.DAL;
using System.IO;
using uGovernIT.Utility.Entities;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager.Helper;
using uGovernIT.DMS.Amazon;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.Util.Log;
using System.Net;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private UserProfileManager _userManager;
        private ApplicationContext _applicationContext;

        public AccountController()
        {
            //_applicationContext = ApplicationContext.Create();
            _applicationContext = HttpContext.Current.GetManagerContext();
        }

        public AccountController(UserProfileManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public UserProfileManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<UserProfileManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            try
            {
                ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
                return new UserInfoViewModel
                {
                    Email = User.Identity.GetUserName(),
                    HasRegistered = externalLogin == null,
                    LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
                };
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserInfo: " + ex);
            }
            return null;
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            try
            {
                UserProfile user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                if (user == null)
                {
                    return null;
                }

                List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

                List<UserLoginInfo> userLogins = UserManager.GetLogins(user.Id).ToList();

                foreach (UserLoginInfo linkedAccount in userLogins)
                {
                    logins.Add(new UserLoginInfoViewModel
                    {
                        LoginProvider = linkedAccount.LoginProvider,
                        ProviderKey = linkedAccount.ProviderKey
                    });
                }

                if (user.PasswordHash != null)
                {
                    logins.Add(new UserLoginInfoViewModel
                    {
                        LoginProvider = LocalLoginProvider,
                        ProviderKey = user.UserName,
                    });
                }

                return new ManageInfoViewModel
                {
                    LocalLoginProvider = LocalLoginProvider,
                    Email = user.UserName,
                    Logins = logins,
                    ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
                };
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetManageInfo: " + ex);
            }
            return null;
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            try
            {
                if (error != null)
                {
                    return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
                }

                if (!User.Identity.IsAuthenticated)
                {
                    return new ChallengeResult(provider, this);
                }

                ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

                if (externalLogin == null)
                {
                    return InternalServerError();
                }

                if (externalLogin.LoginProvider != provider)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    return new ChallengeResult(provider, this);
                }

                UserProfile user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                    externalLogin.ProviderKey));

                bool hasRegistered = user != null;

                if (hasRegistered)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    ClaimsIdentity oAuthIdentity = await UserManager.GenerateUserIdentityAsync(user,
                       OAuthDefaults.AuthenticationType);
                    ClaimsIdentity cookieIdentity = await UserManager.GenerateUserIdentityAsync(user,
                        CookieAuthenticationDefaults.AuthenticationType);

                    AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                    Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
                }
                else
                {
                    IEnumerable<Claim> claims = externalLogin.GetClaims();
                    ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                    Authentication.SignIn(identity);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetExternalLogin: " + ex);
                return InternalServerError();
            }

        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            try
            {
                IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
                List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

                string state;

                if (generateState)
                {
                    const int strengthInBits = 256;
                    state = RandomOAuthStateGenerator.Generate(strengthInBits);
                }
                else
                {
                    state = null;
                }

                foreach (AuthenticationDescription description in descriptions)
                {
                    ExternalLoginViewModel login = new ExternalLoginViewModel
                    {
                        Name = description.Caption,
                        Url = Url.Route("ExternalLogin", new
                        {
                            provider = description.AuthenticationType,
                            response_type = "token",
                            client_id = Startup.PublicClientId,
                            redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                            state = state
                        }),
                        State = state
                    };
                    logins.Add(login);
                }
                return logins;
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetExternalLogins: " + ex);
                return null;
            }
        }

        [AllowAnonymous]
        [Route("GenerateRandomPassword")]
        [HttpPost]
        public string GeneratePassword()
        {
            return UserManager.GeneratePassword();
        }

        [AllowAnonymous]
        [Route("Requestordata")]
        [HttpPost]
        public async Task<IHttpActionResult> GetRequestorInfo(RequestorModel requestorInfo)
        {
            try
            {
                await Task.FromResult(0);
                requestorInfo.UserID = Uri.UnescapeDataString(requestorInfo.UserID);
                // Get user location
                string locationID = string.Empty;
                string desklocation = string.Empty;
                bool isUserVIP = false;
                UserProfileManager UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                UserProfile user = UserManager.GetUserById(requestorInfo.UserID);
                if (user != null)
                {
                    locationID = Convert.ToString(user.Location);
                    desklocation = user.DeskLocation; ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                    if (user != null && UserManager.CheckUserIsInGroup(objConfigurationVariableHelper.GetValue(ConfigConstants.VIPGroup), user))
                    {
                        isUserVIP = true;
                    }
                }

                requestorInfo = new Models.RequestorModel();
                requestorInfo.UserID = user.Id;
                requestorInfo.UserLocationID = user.Location;
                requestorInfo.UserDeskLoaction = user.DeskLocation;
                requestorInfo.IsVIP = isUserVIP;
                requestorInfo.ManagerID = user.ManagerID;
                FieldConfigurationManager fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                if (!string.IsNullOrEmpty(user.ManagerID))
                {
                    requestorInfo.Manager = UserManager.GetUserById(user.ManagerID).Name.ToString();
                }
                if (!string.IsNullOrEmpty(requestorInfo.UserLocationID))
                {
                    requestorInfo.Location = fmanger.GetFieldConfigurationData("LocationLookup", requestorInfo.UserLocationID);
                }
                requestorInfo.Name = user.Name;
                requestorInfo.Department = user.Department;
                requestorInfo.FunctionalAreaID = Convert.ToString(user.FunctionalArea);
                if (!string.IsNullOrEmpty(requestorInfo.FunctionalAreaID))
                {
                    requestorInfo.FunctionalAreaName = fmanger.GetFieldConfigurationData("FunctionalAreaLookup", requestorInfo.FunctionalAreaID);
                }
                return Ok(requestorInfo);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRequestorInfo: " + ex);
                return null;
            }
        }

        [AllowAnonymous]
        [Route("deleteimage")]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteImages(TicketModel ticket)
        {
            try
            {
                await Task.FromResult(0);
                if (ticket != null)
                {
                    DataTable dt = GetTableDataManager.GetTableData(ticket.moduleName, DatabaseObjects.Columns.ID + "=" + ticket.id);
                    if (dt != null)
                    {
                        string path = HttpContext.Current.Server.MapPath("~/Content/images/") + ticket.image;
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            dt.Rows[0][DatabaseObjects.Columns.Attachments] = Convert.ToString(dt.Rows[0][DatabaseObjects.Columns.Attachments]).Replace(ticket.image, "");
                        }
                        TicketDal.SaveTicket(dt.Rows[0], ticket.moduleName, false);
                    }
                }
                return Ok();
            }
            catch(Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRequestorInfo: " + ex);
                return Ok("Error");
            }

        }

        [AllowAnonymous]
        [Route("findissuetypes")]
        [HttpPost]
        public async Task<IHttpActionResult> GetIssueTypes(ChoiceTypeModel choice)
        {
            try
            {
                await Task.FromResult(0);
                if (string.IsNullOrEmpty(choice.moduleName))
                    return Ok();
                List<string> lstIssues = new List<string>();
                choice.moduleName = Uri.UnescapeDataString(choice.moduleName);
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                UGITModule module = moduleViewManager.LoadByName(choice.moduleName);
                if (module == null)
                    return Ok();
                ModuleRequestType currentRequestType = module.List_RequestTypes.FirstOrDefault(x => x.ID == choice.requesttypeid);

                if (currentRequestType != null && !string.IsNullOrWhiteSpace(currentRequestType.IssueTypeOptions))
                {
                    lstIssues = UGITUtility.SplitString(currentRequestType.IssueTypeOptions, new string[] { Utility.Constants.Separator, Utility.Constants.NewLineSeperator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    return Ok(lstIssues.JsonSerialize());
                }
                else
                {
                    //DataTable lstrequesttype = GetTableDataManager.GetTableData(module.ModuleTable);
                    ////SPFieldMultiChoice choiceField = lstrequesttype.Columns.GetFieldByInternalName(DatabaseObjects.Columns.UGITIssueType) as SPFieldMultiChoice;
                    //if (lstrequesttype != null && lstrequesttype.Rows.Count > 0)
                    //{
                    //    // lstIssues = lstrequesttype.AsEnumerable().Select(row => row.Field<string>(DatabaseObjects.Columns.UGITIssueType)).ToList();
                    //    lstIssues = lstrequesttype.Rows.OfType<DataRow>().Select(dr => dr.Field<string>(DatabaseObjects.Columns.IssueTypeOptions)).ToList();
                    //    //choiceField.Choices.Cast<string>().ToList();
                    //}
                    if (lstIssues == null || lstIssues.Count == 0)
                    {
                        FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                        FieldConfiguration field = fieldManager.GetFieldByFieldName(choice.fieldName);
                        lstIssues = UGITUtility.ConvertStringToList(field.Data, Utility.Constants.Separator);
                    }
                    return Ok(lstIssues.JsonSerialize());
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetIssueTypes: " + ex);
                return null;
            }
        }

        [AllowAnonymous]
        [Route("findresolutiontypes")]
        [HttpPost]
        public async Task<IHttpActionResult> GetResolutionTypes(ChoiceTypeModel choice)
        {
            try
            {
                await Task.FromResult(0);
                if (string.IsNullOrEmpty(choice.moduleName))
                    return Ok();
                List<string> lstIssues = new List<string>();
                choice.moduleName = Uri.UnescapeDataString(choice.moduleName);
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                UGITModule module = moduleViewManager.LoadByName(choice.moduleName);
                if (module == null)
                    return Ok();
                ModuleRequestType currentRequestType = module.List_RequestTypes.FirstOrDefault(x => x.ID == choice.requesttypeid);
                if (currentRequestType != null && !string.IsNullOrWhiteSpace(currentRequestType.ResolutionTypes))
                {
                    lstIssues = UGITUtility.SplitString(currentRequestType.ResolutionTypes, new string[] { Utility.Constants.Separator, Utility.Constants.NewLineSeperator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    return Ok(lstIssues.JsonSerialize());
                }
                else
                {
                    //DataTable lstrequesttype = GetTableDataManager.GetTableData(module.ModuleTable);
                    ////SPFieldMultiChoice choiceField = lstrequesttype.Fields.GetFieldByInternalName(DatabaseObjects.Columns.TicketResolutionType) as SPFieldMultiChoice;
                    //if (lstrequesttype != null && lstrequesttype.Rows.Count > 0)
                    //{
                    //    lstIssues = lstrequesttype.Rows.OfType<DataRow>().Select(dr => dr.Field<string>(DatabaseObjects.Columns.ResolutionTypes)).ToList();
                    //}
                    if (lstIssues == null || lstIssues.Count == 0)
                    {

                        FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                        FieldConfiguration field = fieldManager.GetFieldByFieldName(choice.fieldName);
                        lstIssues = UGITUtility.ConvertStringToList(field.Data, Utility.Constants.Separator);

                    }
                    return Ok(lstIssues.JsonSerialize());
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetResolutionTypes: " + ex);
                return null;
            }
        }


        [AllowAnonymous]
        [Route("GetAllDetails")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllDetails(TicketModel ticket)
        {
            try
            {
                await Task.FromResult(0);
                string strReturnValue = string.Empty;
                string currentTicketPublicID = ticket.id;
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                AssetIncidentRelationsManager assetIncidentRelationsMgr = new AssetIncidentRelationsManager(context);
                TicketManager ticketManager = new TicketManager(context);

                //Get data from ModuleWorkflowHistory for this ticket.
                DataTable moduleWorkflowItemsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleWorkflowHistory,
                    $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' AND {DatabaseObjects.Columns.TicketId} = '{currentTicketPublicID}'");
                if (moduleWorkflowItemsTable == null)
                    return Ok(strReturnValue);

                string where = $"{DatabaseObjects.Columns.ParentTicketId} = '{currentTicketPublicID}' AND {DatabaseObjects.Columns.Deleted} = 0";
                //Get AssetIncidentRelations where this ticket is Parent ticket.
                List<AssetIncidentRelations> assetIncidentRelations = assetIncidentRelationsMgr.Load(where);
                DataRow currentTicket = ticketManager.GetByTicketIdFromCache(ticket.moduleName, currentTicketPublicID);

                DataTable dtRelatedTicketsDetails = currentTicket.Table.Clone();
                DataRow drTicket = null;
                Dictionary<string, DateTime> lstasset = new Dictionary<string, DateTime>();

                foreach (AssetIncidentRelations relation in assetIncidentRelations)
                {
                    drTicket = ticketManager.GetDataRowByTicketId(relation.TicketId);
                    //dtRelatedTicketsDetails.Rows.Add(drTicket);

                    string rdatetime = UGITUtility.getDateStringInFormat(Convert.ToDateTime(drTicket[DatabaseObjects.Columns.CreationDate]), false);
                    string rticket = Convert.ToString(drTicket[DatabaseObjects.Columns.TicketId]);
                    string url = "";// UGITUtility.GetAbsoluteURL(Convert.ToString(drTicket[DatabaseObjects.Columns.ModuleRelativePagePath]));
                    string title = Uri.EscapeDataString(Convert.ToString(drTicket[DatabaseObjects.Columns.Title]));
                    string ModuleName = uHelper.getModuleNameByTicketId(rticket);
                    string description = Uri.EscapeDataString(Convert.ToString(drTicket[DatabaseObjects.Columns.TicketDescription]));
                    string rticketwith = string.Format("{0}^{1}^{2}^{3}^{4}^{5}", rticket, url, title, ModuleName, rdatetime, UGITUtility.TruncateWithEllipsis(description, 100));
                    lstasset.Add(rticketwith, Convert.ToDateTime(drTicket[DatabaseObjects.Columns.CreationDate]));
                }

                int maxstepno = 0;
                DataTable distinctsteps = moduleWorkflowItemsTable.DefaultView.ToTable(true, DatabaseObjects.Columns.StageStep);
                if (distinctsteps.Rows.Count > 0)
                    maxstepno = UGITUtility.StringToInt(distinctsteps.Compute(string.Format("Max({0})", DatabaseObjects.Columns.StageStep), string.Empty));
                maxstepno += 1;

                DateTime firstStageStartDate = Convert.ToDateTime(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.Created));
                DateTime previousdate = DateTime.MinValue;
                List<string[]> mapstagelst = new List<string[]>();
                List<DateTime> enddates = new List<DateTime>();
                Dictionary<int, List<string>> stageMaps = new Dictionary<int, List<string>>();

                if (moduleWorkflowItemsTable != null)
                {
                    int i = 1;
                    foreach (DataRow hRow in moduleWorkflowItemsTable.Rows)
                    {
                        int step = Convert.ToInt32(UGITUtility.GetSPItemValue(hRow, DatabaseObjects.Columns.StageStep));
                        DateTime sEndDate = DateTime.MinValue;
                        if (hRow[DatabaseObjects.Columns.StageEndDate] == DBNull.Value)
                            sEndDate = DateTime.Now;
                        else
                            sEndDate = Convert.ToDateTime(UGITUtility.GetSPItemValue(hRow, DatabaseObjects.Columns.StageEndDate));

                        foreach (var rrelatedincident in lstasset)
                        {
                            string currentmap = rrelatedincident.Key;
                            DateTime rrelatedinci = rrelatedincident.Value;
                            double differenceofdays = 0.0;
                            double noOfDays = 0.0;

                            if (rrelatedinci < firstStageStartDate && rrelatedinci > previousdate)
                            {
                                currentmap = string.Format("{0}^{1}^{2}", currentmap, differenceofdays, noOfDays);

                                if (stageMaps.ContainsKey(0))
                                    stageMaps[0].Add(currentmap);
                                else
                                    stageMaps.Add(0, new List<string>() { currentmap });
                            }
                            else if (rrelatedinci <= sEndDate && rrelatedinci > firstStageStartDate && rrelatedinci > previousdate)
                            {
                                if (previousdate == DateTime.MinValue)
                                {
                                    differenceofdays = (sEndDate - firstStageStartDate).TotalDays;
                                    noOfDays = (rrelatedinci - firstStageStartDate).TotalDays;
                                    currentmap = string.Format("{0}^{1}^{2}", currentmap, differenceofdays, noOfDays);
                                }
                                else
                                {
                                    differenceofdays = (sEndDate - previousdate).TotalDays;
                                    noOfDays = (rrelatedinci - previousdate).TotalDays;
                                    currentmap = string.Format("{0}^{1}^{2}", currentmap, differenceofdays, noOfDays);
                                }


                                if (stageMaps.ContainsKey(step))
                                    stageMaps[step].Add(currentmap);
                                else
                                    stageMaps.Add(step, new List<string>() { currentmap });

                            }
                            else if (rrelatedinci > previousdate && rrelatedinci <= sEndDate)
                            {

                                differenceofdays = (sEndDate - previousdate).TotalDays;
                                noOfDays = (rrelatedinci - previousdate).TotalDays;
                                currentmap = string.Format("{0}^{1}^{2}", currentmap, differenceofdays, noOfDays);
                                if (stageMaps.ContainsKey(step))
                                    stageMaps[step].Add(currentmap);
                                else
                                    stageMaps.Add(step, new List<string>() { currentmap });
                            }
                        }

                        previousdate = sEndDate;
                        if (i == moduleWorkflowItemsTable.Rows.Count)
                            enddates.Add(sEndDate);

                        i++;
                    }
                    DateTime maxEndDate = enddates.Last();
                    foreach (var rrelatedincident in lstasset)
                    {
                        string currentmap = rrelatedincident.Key;
                        DateTime rrelatedinci = rrelatedincident.Value;
                        if (rrelatedinci > maxEndDate)
                        {
                            if (stageMaps.ContainsKey(maxstepno))
                                stageMaps[maxstepno].Add(currentmap);
                            else
                                stageMaps.Add(maxstepno, new List<string>() { currentmap });
                        }
                    }
                }

                List<string> exp = new List<string>();
                StringBuilder expBuild;
                foreach (int key in stageMaps.Keys)
                {
                    expBuild = new StringBuilder();
                    expBuild.Append("{");
                    expBuild.AppendFormat("\"key\":\"{0}\",\"mapt\":", key);
                    expBuild.Append("[");
                    foreach (string value in stageMaps[key])
                    {
                        if (stageMaps[key].IndexOf(value) != 0)
                            expBuild.Append(",");

                        expBuild.Append("{");
                        expBuild.AppendFormat("\"ticketid\":\"{0}\"", value);
                        expBuild.Append("}");
                    }
                    expBuild.Append("]}");
                    exp.Add(expBuild.ToString());

                }
                strReturnValue = "[" + string.Join(",", exp.ToArray()) + "]";

                return Ok(strReturnValue);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetAllDetails: " + ex);
                return null;
            }
        }

        [AllowAnonymous]
        [Route("StageApproveValue")]
        [HttpPost]
        public async Task<IHttpActionResult> StageApproveValue(TicketModel tmodel)
        {
            try
            {
                await Task.FromResult(0);
                string strReturnValue = string.Empty;
                string message = string.Empty;
                bool isAllTaskComplete = UGITModuleConstraint.GetPendingConstraintsStatus(tmodel.id, tmodel.currentStep, ref message, HttpContext.Current.GetManagerContext());
                if (isAllTaskComplete)
                {
                    strReturnValue = Utility.Constants.Completed;
                }
                else
                {
                    strReturnValue = Utility.Constants.Pending;
                    strReturnValue = strReturnValue + Utility.Constants.Separator + message;
                }
                return Ok(strReturnValue);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in StageApproveValue: " + ex);
                return null;
            }
        }
        [AllowAnonymous]
        [Route("GetTicketHoldReason")]
        [HttpPost]
        public async Task<IHttpActionResult> GetTicketHoldReason(CommonModel comonModel)
        {
            try
            {
                string result = string.Empty;
                await Task.FromResult(0);
                DataRow item = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), comonModel.name, comonModel.id);
                if (item == null || !UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.TicketOnHold]))
                    return Ok(result);

                string reason = Convert.ToString(item[DatabaseObjects.Columns.OnHoldReasonChoice]);
                DateTime holdStartDate = DateTime.Now;
                if (item.Table.Columns.Contains(DatabaseObjects.Columns.TicketOnHoldStartDate))
                    holdStartDate = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.TicketOnHoldStartDate]);

                DateTime holdTillDate = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.TicketOnHoldTillDate]);
                string holdTillDateStr = string.Empty;
                if (holdTillDate != DateTime.MinValue)
                {
                    if (holdTillDate.Year == DateTime.Now.Year)
                        holdTillDateStr = holdTillDate.ToString("MMM-d hh:mm tt");
                    else
                        holdTillDateStr = UGITUtility.GetDateStringInFormat(holdTillDate, true);
                }

                List<HistoryEntry> comments = uHelper.GetHistory(Convert.ToString(item[DatabaseObjects.Columns.TicketComment]), true);

                string comment = string.Empty;
                if (comments.Count > 0)
                {
                    HistoryEntry entry = comments.FirstOrDefault(x => !x.IsPrivate && x.entry.Contains("[Hold]"));
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.entry))
                    {
                        string[] dtComent = UGITUtility.SplitString(entry.entry, new string[] { "Comment:" });
                        if (dtComent.Length == 2)
                            comment = dtComent[1];
                    }
                }
                result = string.Format("{{ \"holdreason\":\"{0}\", \"holdtilldate\":\"{1}\",\"holdstartdate\":\"{2}\",\"holdcomment\":\"{3}\"}}", reason, holdTillDateStr, UGITUtility.GetDateStringInFormat(holdStartDate, true), comment);
                return Ok(result);

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTicketHoldReason: " + ex);
                return null;
            }
        }
        [AllowAnonymous]
        [Route("GetTaskHoldReason")]
        [HttpPost]
        public async Task<IHttpActionResult> GetTaskHoldReason(CommonModel comonModel)
        {
            try
            {
                await Task.FromResult(0);
                string result = string.Empty;
                int taskIDV = UGITUtility.StringToInt(comonModel.id);
                if (comonModel.name != "SVC" || taskIDV <= 0)
                    return Ok(result);

                UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
                UGITTask task = taskManager.LoadByID(taskIDV);
                if (task == null || !task.OnHold)
                    return Ok(result);


                string holdTillDateStr = string.Empty;
                if (task.OnHoldTillDate.HasValue && task.OnHoldTillDate.Value != DateTime.MinValue)
                {
                    if (task.OnHoldTillDate.Value.Year == DateTime.Now.Year)
                        holdTillDateStr = task.OnHoldTillDate.Value.ToString("MMM-d hh:mm tt");
                    else
                        holdTillDateStr = UGITUtility.GetDateStringInFormat(task.OnHoldTillDate.Value, true);
                }

                List<HistoryEntry> comments = uHelper.GetHistory(task.Comment); //task.CommentList;
                string comment = string.Empty;
                if (comments.Count > 0)
                {
                    HistoryEntry entry = comments.FirstOrDefault(x => !x.IsPrivate && x.entry.Contains("[Hold]"));
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.entry))
                    {
                        string[] dtComent = UGITUtility.SplitString(entry.entry, new string[] { "Comment:" });
                        if (dtComent.Length == 2)
                            comment = dtComent[1];
                    }
                }
                result = "{" + string.Format("\"holdreason\":\"{0}\", \"holdtilldate\":\"{1}\",\"holdstartdate\":\"{2}\",\"holdcomment\":\"{3}\"", task.OnHoldReasonChoice, holdTillDateStr, UGITUtility.GetDateStringInFormat(task.OnHoldStartDate.Value, true), comment) + "}";
                //"{" + string.Format("\"messageCode\":{0},\"message\":\"{1}\",\"kpisInfo\":{2}", messageCode, message, kpiText) + "}";
                return Ok(result);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetTaskHoldReason: " + ex);
                return null;
            }
        }
        [AllowAnonymous]
        [Route("getDoughnutOnlyConfiguration")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDoughnutOnlyConfiguration(DashbaordPanelKPIFilter dashboardPanelKPIFilter)
        {
            await Task.FromResult(0);
            var messageCode = 0;
            var json = string.Empty;
            var chartType = string.Empty;
            var PanelViewType = string.Empty;

            var applicationContext = HttpContext.Current.GetManagerContext();
            var expressionCalc = new ExpressionCalc(applicationContext);


            try
            {
                var message = string.Empty;
                int.TryParse(dashboardPanelKPIFilter.panelID, out var panelId);
                //string kpiText = string.Empty;
                var sbKpiText = new StringBuilder();

                int.TryParse(dashboardPanelKPIFilter.viewID, out var dashboardViewId);

                var dashboardManager = new DashboardManager(applicationContext);
                var dashboardPanelViewManager = new DashboardPanelViewManager(applicationContext);

                var dashboardPanelView = dashboardPanelViewManager.LoadViewByID(dashboardViewId);
                var isSideBar = dashboardPanelView?.ViewProperty is SideDashboardView;
                if (panelId > 0)
                {
                    var kpiIds = UGITUtility.ConvertStringToList(dashboardPanelKPIFilter.kpiIDs, new string[] { "," });
                    if (kpiIds.Any())
                    {
                        var dashboard = dashboardManager.LoadPanelById(panelId);
                        if (dashboard != null)
                        {
                            if (dashboard.DashboardType == DashboardType.Panel && dashboard.panel is PanelSetting panel)
                            {
                                //ExpressionCalc expCalc = null;
                                var calcResults = new Dictionary<Guid, double>();

                                // dynamic expCalcClone = null;
                                chartType = panel.ChartType;
                                PanelViewType = panel.PanelViewType;
                                string hAlign = string.IsNullOrWhiteSpace(panel.HorizontalAlignment) ? "center" : panel.HorizontalAlignment;
                                string vAlign = string.IsNullOrWhiteSpace(panel.VerticalAlignment) ? "bottom" : panel.VerticalAlignment;
                                string textFormat = string.IsNullOrWhiteSpace(panel.TextFormat) ? "custom" : panel.TextFormat;
                                string legendvisible = string.IsNullOrWhiteSpace(panel.Legendvisible) ? "false" : panel.Legendvisible;
                                string centreTitle = string.IsNullOrWhiteSpace(panel.CentreTitle) ? "BCCI" : panel.CentreTitle;
                                //string centreImageUrl = string.IsNullOrWhiteSpace(panel.CentreImageUrl) ? System.Web.VirtualPathUtility.ToAbsolute("~/Content/Images/No_Image.svg") : panel.CentreImageUrl;
                                json = "{" + string.Format("\"messageCode\":{0},\"message\":\"{1}\",\"chartType\":\"{2}\",\"panelViewType\":\"{3}\",\"hAlign\":\"{4}\",\"vAlign\":\"{5}\",\"textFormat\":\"{6}\",\"Legendvisible\":\"{7}\",\"centreTitle\":\"{8}\" ", messageCode, message, chartType, PanelViewType, hAlign, vAlign, textFormat, legendvisible, centreTitle) + "}";
                            }
                            else
                            {
                                messageCode = 0;
                                message = "No panel exist";
                            }
                        }
                        else
                        {
                            messageCode = 0;
                            message = "No panel exist";
                        }
                    }
                    else
                    {
                        messageCode = 0;
                        message = "No panel exist";
                    }
                }
                else
                {
                    messageCode = 0;
                    message = "No panel exist";
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
            return Ok(json);

        }
        [AllowAnonymous]
        [Route("GetDashbaordPanelKPI")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDashbaordPanelKPI(DashbaordPanelKPIFilter dashboardPanelKPIFilter)
        {
            await Task.FromResult(0);
            // Do lot of work here           

            var messageCode = 0;
            var json = string.Empty;
            var chartType = string.Empty;
            var IsChartHide = 0;
            var PanelViewType = string.Empty;
            var dashboardTitle = string.Empty;

            var applicationContext = HttpContext.Current.GetManagerContext();
            var expressionCalc = new ExpressionCalc(applicationContext);

            try
            {
                var message = string.Empty;
                int.TryParse(dashboardPanelKPIFilter.panelID, out var panelId);
                //string kpiText = string.Empty;
                var sbKpiText = new StringBuilder();

                int.TryParse(dashboardPanelKPIFilter.viewID, out var dashboardViewId);

                var dashboardManager = new DashboardManager(applicationContext);
                var dashboardPanelViewManager = new DashboardPanelViewManager(applicationContext);

                var dashboardPanelView = dashboardPanelViewManager.LoadViewByID(dashboardViewId,true);
                var isSideBar = dashboardPanelView?.ViewProperty is SideDashboardView;

                if (panelId > 0)
                {
                    var kpiIds = UGITUtility.ConvertStringToList(dashboardPanelKPIFilter.kpiIDs, new string[] { "," });

                    if (kpiIds.Any())
                    {
                        var dashboard = dashboardManager.LoadPanelById(panelId,true);
                        if (dashboard != null)
                        {
                            dashboardTitle = dashboard.Title;

                            if (dashboard.DashboardType == DashboardType.Panel && dashboard.panel is PanelSetting panel)
                            {
                                ExpressionCalc expCalc = null;
                                var calcResults = new Dictionary<Guid, double>();

                                dynamic expCalcClone = null;
                                chartType = panel.ChartType;
                                IsChartHide = panel.ChartHide;
                                PanelViewType = panel.PanelViewType;
                                //var isSameTable = panel.Expressions.Select(a => a.DashboardTable).Distinct().Count() == 1;
                                bool isSameTable = false;

                                foreach (var kpi in panel.Expressions)
                                {
                                    if (kpi.IsHide)
                                        continue;
                                    if (kpiIds.Exists(x => x == kpi.LinkID.ToString()))
                                    {
                                        if (expCalcClone != null && expCalcClone.FactTableName == kpi.DashboardTable)
                                            isSameTable = true;
                                        else
                                            isSameTable = false;
                                        if (isSameTable && expCalcClone != null)
                                        {
                                            expCalc = expCalcClone;
                                        }
                                        else
                                        {
                                            expCalc = new ExpressionCalc(kpi.DashboardTable, applicationContext);
                                            expCalcClone = expCalc;
                                        }
                                    }

                                    if (expCalc != null)
                                    {
                                        var gFilter = DashboardHelper.GetGlobalFilter(applicationContext, dashboardPanelKPIFilter.globalFilter, dashboardPanelView, expCalc.FactTable);
                                        if (kpi.Filter != "" && kpi.Filter.Contains("[Closed] <> '1'"))
                                        {
                                            kpi.Filter = ("[Closed] <> 'True'");
                                        }
                                        if (kpi.Filter != "" && kpi.Filter.Contains("[Closed] = '1'"))
                                        {
                                            kpi.Filter = ("[Closed] = 'True'");
                                        }
                                        if (kpi.Filter != "" && kpi.Filter.Contains("[Closed] = '0'"))
                                        {
                                            kpi.Filter = ("[Closed] = 'False'");
                                        }
                                        if (kpi.Filter != "" && kpi.Filter.Contains("[Closed] <> '0'"))
                                        {
                                            kpi.Filter = ("[Closed] <> 'False'");
                                        }

                                        double result = expCalc.Aggragate(kpi.AggragateFun, kpi.AggragateOf, kpi.Filter, expCalc.GetDateRangeClause(kpi.DateFilterStartField, kpi.DateFilterDefaultView), kpi.IsPct, gFilter);
                                        calcResults.Add(kpi.LinkID, result);
                                    }
                                }
                                sbKpiText.Append("[");
                                foreach (var kpiLink in panel.Expressions)
                                {
                                    if (kpiLink.ExpressionFormat.Trim() == string.Empty) continue;

                                    if (panel.Expressions.IndexOf(kpiLink) != 0)
                                    {
                                        sbKpiText.Append(",");
                                    }

                                    sbKpiText.Append("{");

                                    // Use expression like {$exp$:C0} to format KPI data 
                                    var kpiString = string.Empty;
                                    if (!string.IsNullOrEmpty(kpiLink.ExpressionFormat))
                                    {
                                        kpiString = kpiLink.ExpressionFormat.Contains("{$exp$")
                                            ? kpiLink.ExpressionFormat.Replace("$exp$", "0")
                                                .Replace(" ", "&nbsp;")
                                            : kpiLink.ExpressionFormat.Replace("$exp$", "{0}")
                                                .Replace(" ", "&nbsp;");

                                        if (calcResults.Keys.Contains(kpiLink.LinkID))
                                            kpiString = string.Format(kpiString, calcResults[kpiLink.LinkID]);
                                        kpiString = expressionCalc.EvaluateEvalFunt(kpiString);
                                    }
                                    //if (kpiii.ShowBar)
                                    //{

                                    var barConditions = kpiLink.Conditions.OrderByDescending(x => x.Score).ToList();
                                    var color = kpiLink.BarDefaultColor;

                                    foreach (var barCondition in barConditions)
                                    {
                                        if (expressionCalc.EvalLogicalExp(
                                            $"{calcResults[kpiLink.LinkID]} {barCondition.Operator} {barCondition.Score}"))
                                        {
                                            color = barCondition.Color;
                                        }
                                    }

                                    var barMaxWidth = 100;
                                    if (isSideBar)
                                        barMaxWidth = 60;

                                    double maxLimit = 0;

                                    if (panel.StopAutoScale)
                                        maxLimit = kpiLink.MaxLimit;
                                    else
                                    {
                                        //For Percentage max limit must be 100;
                                        if (kpiLink.BarUnit != null && (kpiLink.BarUnit.Trim().ToLower() == "percentage" || kpiLink.BarUnit.Trim().ToLower() == "%"))
                                        {
                                            maxLimit = 100;
                                        }
                                        else
                                        {
                                            var sameUnitDashboards = panel.Expressions.Where(x => x.BarUnit == kpiLink.BarUnit).ToList();
                                            var selectedResults = (from s in sameUnitDashboards
                                                                   join c in calcResults
                                                                       on s.LinkID equals c.Key
                                                                   select c);

                                            var keyValuePairs = selectedResults.ToList();
                                            if (keyValuePairs.Any())
                                            {
                                                maxLimit = keyValuePairs.Max(x => x.Value);
                                            }
                                        }
                                    }
                                    if (kpiLink.ShowBar)
                                    {
                                        if (calcResults.Keys.Contains(kpiLink.LinkID))
                                            kpiString = UGITUtility.GetProgressBar(kpiString, color, maxLimit, calcResults[kpiLink.LinkID], barMaxWidth);
                                    }
                                    if (calcResults.Keys.Contains(kpiLink.LinkID))
                                    {
                                        var progressPct = Math.Round((calcResults[kpiLink.LinkID] / maxLimit) * 100, 2);

                                        sbKpiText.AppendFormat("\"KpiID\":\"{0}\",\"kpiTitle\":\"{1}\",\"linkExist\":\"{2}\", \"ProgressPct\":\"{3}\" ", kpiLink.LinkID, kpiString, false, progressPct);

                                    }
                                    sbKpiText.Append("}");
                                }

                                sbKpiText.Append("]");

                                messageCode = 2;
                                message = "Success";
                            }
                        }
                        else
                        {
                            messageCode = 0;
                            message = "No panel exist";
                        }
                    }
                    else
                    {
                        messageCode = 1;
                        message = "There is no dirty KPI";
                    }
                }
                else
                {
                    messageCode = 0;
                    message = "No panel exist";
                }

                json = "{" + string.Format("\"messageCode\":{0},\"message\":\"{1}\",\"kpisInfo\":{2},\"dashboardTitle\":\"{3}\",\"chartType\":\"{4}\",\"panelViewType\":\"{5}\",\"panelID\":\"{6}\",\"ChartHide\":\"{7}\" ", messageCode, message, sbKpiText.ToString(), dashboardTitle, chartType, PanelViewType, panelId, IsChartHide) + "}";

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDashbaordPanelKPI: " + ex);
            }
            return Ok(json);
        }
        [Route("GetUserProject")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserProject()
        {
            await Task.FromResult(0);
            try
            {
                var currentUser = _applicationContext.CurrentUser;
                UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                var role = umanager.GetUserRoles(currentUser.Id).Select(x => x.Id).ToList();
                Dictionary<string, object> arrParams = new Dictionary<string, object>();
                arrParams.Add("TenantId", currentUser.TenantID);
                arrParams.Add("IsClosed", '1');
                DataTable dtResultBillings = uGITDAL.ExecuteDataSetWithParameters("usp_GetCRMProject", arrParams);
                StringBuilder sbQuery = new StringBuilder();
                //string ownerColumn = DatabaseObjects.Columns.Owner;
                var userid = currentUser.Id;

                //(moduleTable == DatabaseObjects.Tables.CRMContact || moduleTable == DatabaseObjects.Tables.CRMCompany || moduleTable == DatabaseObjects.Tables.Opportunity)
                //ownerColumn = DatabaseObjects.Columns.OwnerUser;
                //else if (moduleTable == DatabaseObjects.Tables.Lead || moduleTable == DatabaseObjects.Tables.CRMProject)
                //    ownerColumn = DatabaseObjects.Columns.Owner;

                sbQuery.Append($"{DatabaseObjects.Columns.Owner} like '%{userid}%' OR ");
                sbQuery.Append($"{DatabaseObjects.Columns.Estimator} like '%{userid}%' OR ");
                sbQuery.Append($"ActionUserTypes like '%{userid}%' OR ");
                sbQuery.Append($"ActionUsers like '%{userid}%' OR ");
                sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUserTypes} like '%{userid}%' OR ");
                sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUsers} like '%{userid}%' OR ");
                //sbQuery.Append($"ProjectExecutive like '%{userid}%' OR ");
                //sbQuery.Append($"ProjectManager like '%{userid}%' OR ");
                //sbQuery.Append($"Superintendent like '%{userid}%' OR ");
                sbQuery.Append($"{DatabaseObjects.Columns.TicketInitiator} like '%{userid}%' OR ");
                sbQuery.Append($"{DatabaseObjects.Columns.Closed} =1 OR ");

                foreach (var item in role)
                {
                    sbQuery.Append($"{DatabaseObjects.Columns.Owner} like '%{item}%' OR ");
                    sbQuery.Append($"{DatabaseObjects.Columns.Estimator} like '%{item}%' OR ");
                    sbQuery.Append($"ActionUserTypes like '%{item}%' OR ");
                    sbQuery.Append($"ActionUsers like '%{item}%' OR ");
                    sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUserTypes} like '%{item}%' OR ");
                    sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUsers} like '%{item}%' OR ");
                    //sbQuery.Append($"ProjectExecutive like '%{item}%' OR ");
                    //sbQuery.Append($"ProjectManager like '%{item}%' OR ");
                    //sbQuery.Append($"Superintendent like '%{item}%' OR ");
                    sbQuery.Append($"{DatabaseObjects.Columns.TicketInitiator} like '%{item}%' OR ");
                }

                string query = !string.IsNullOrEmpty(Convert.ToString(sbQuery)) ? Convert.ToString(sbQuery) : string.Empty;
                query = query.Remove(query.LastIndexOf("OR"));
                query = query + "";
                var projects = dtResultBillings.Select(query).Select(x => new
                {
                    Id = Convert.ToInt32(x["Id"]),
                    Image = "",
                    Name = x["Title"].ToString(),
                    CloseDate = x["CloseDate"] == DBNull.Value ? Convert.ToDateTime(x["Modified"]) : Convert.ToDateTime(x["CloseDate"])
                }).OrderByDescending(p => p.CloseDate).Take(5).ToList();

                return Json(projects);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserProject: " + ex);
            }
            return null;
        }
        [AllowAnonymous]
        [Route("GetDashboardChart")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDashboardChart(DashboardChartFilter dashboardChartFilter)
        {
            await Task.FromResult(0);
            string resultedJson = string.Empty;
            int messageCode = 2;
            string message = string.Empty;
            try
            {
                message = string.Empty;
                int panelId = Convert.ToInt32(dashboardChartFilter.panelID);
                DashboardManager objDashboardManager = new DashboardManager(HttpContext.Current.GetManagerContext());
                Dashboard dashboard = objDashboardManager.LoadPanelById(panelId);
                dashboard.PanelWidth = 240;
                dashboard.PanelHeight = 150;

                int dashboardViewID = 0;
                int.TryParse(dashboardChartFilter.viewID, out dashboardViewID);
                DashboardPanelViewManager objDVM = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
                DashboardPanelView view = objDVM.LoadByID(dashboardViewID);


                ChartSetting panel = (ChartSetting)dashboard.panel;
                ChartHelper chartH = new ChartHelper(panel, HttpContext.Current.GetManagerContext());

                int dashboardViewType = 0;
                int.TryParse(dashboardChartFilter.viewType, out dashboardViewType);
                chartH.ViewType = dashboardViewType;
                chartH.UseAjax = true;
                chartH.GlobalFilter = DevxChartHelper.GetGlobalFilter(HttpContext.Current.GetManagerContext(), dashboardChartFilter.globalFilter, view);
                chartH.LocalFilter = dashboardChartFilter.localFilter;
                int dimensionIndex = 0;
                int.TryParse(dashboardChartFilter.drillDown, out dimensionIndex);
                chartH.DrillDownFilter = dimensionIndex;
                chartH.DatapointFilter = dashboardChartFilter.datapointFilter;

                chartH.ChartTitle = "$Date$";

                int dashboardWidth = 0;
                int.TryParse(dashboardChartFilter.width, out dashboardWidth);
                if (dashboardWidth <= 0)
                {
                    dashboardWidth = dashboard.PanelWidth;
                }
                int dashboardHeight = 0;
                int.TryParse(dashboardChartFilter.height, out dashboardHeight);
                if (dashboardHeight <= 0)
                {
                    dashboardHeight = dashboard.PanelHeight;
                }

                bool isSideBar = UGITUtility.StringToBoolean(dashboardChartFilter.sidebar);

                //Check click is coming from where
                //1= user clicked on datapoint, 2=user clicked on localfilter, 3=user click on drilldown
                int WhoTriggeredEvent = 0;
                int.TryParse(dashboardChartFilter.whoTriggered, out WhoTriggeredEvent);

                string imageMapId = Guid.NewGuid().ToString();
                Chart cPChart = null;
                string imageMap = string.Empty;

                bool isZoom = UGITUtility.StringToBoolean(dashboardChartFilter.zoom);

                //From cache if exist
                //chartH.ChartTitle = panel.ContainerTitle;

                chartH.WhoTriggered = WhoTriggeredEvent;
                int aheight = dashboardHeight - 25;
                if (view == null || view.ViewProperty is IndivisibleDashboardsView || isZoom)
                {
                    aheight = dashboardHeight - 60;
                }
                cPChart = chartH.CreateChart(true, (dashboardWidth - 10).ToString(), aheight.ToString(), isSideBar);
                cPChart.ImageStorageMode = ImageStorageMode.UseImageLocation;

                string tempPath = uHelper.GetTempFolderPath();
                string fileName = Guid.NewGuid() + "chart.png";
                tempPath = tempPath + "/" + fileName;
                try
                {
                    cPChart.SaveImage(tempPath, ChartImageFormat.Png);
                }
                catch (Exception ex)
                {
                    //if any exception occur then send messagecode 1 with message
                    messageCode = 1;
                    message = "View Not Found";
                    ULog.WriteException($"An Exception Occurred in GetDashboardChart: " + ex);
                }

                imageMap = HttpContext.Current.Server.UrlEncode(cPChart.GetHtmlImageMap(imageMapId));

                //if no data point ploted on chart then send messagecode 1 with message  
                //&& x.Points.FirstOrDefault(y => y.YValues.Length > 0 && y.YValues[0] > 0) != null
                if (cPChart.Series.Count <= 0 || cPChart.Series.Where(x => x.Points.Count > 0).Count() <= 0)
                {
                    messageCode = 1;
                    message = "No Data";
                }

                string chartUrl = string.Format("/content/images/ugovernittemp/{0}", fileName);
                string showDetailQuery = string.Empty;
                if (chartH.ShowDetail)
                {
                    messageCode = 3;
                    message = "Show Datapoint detail";
                }

                resultedJson = "{" + string.Format("\"messageCode\":{0},\"message\":\"{1}\",\"chartURL\":\"{2}\",\"imageMapID\":\"{3}\",\"imageMap\":\"{4}\",\"chartTitle\":\"{7}\",\"showDetail\":\"{5}\"{6}", messageCode, message, chartUrl, imageMapId, imageMap, chartH.ShowDetail, showDetailQuery, chartH.ChartTitle) + "}";
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }

            return Ok(resultedJson);
        }


        [AllowAnonymous]
        [Route("GetDashboardFilters")]
        [AcceptVerbs("GET", "POST")]
        public async Task<IHttpActionResult> GetDashboardFilters(DashboardFilters dashfilter)
        {
            try
            {
                await Task.FromResult(0);
                var applicationContext = HttpContext.Current.GetManagerContext();
                var filteredData = new StringBuilder();

                int dashboardViewID = 0;
                int.TryParse(dashfilter.viewID, out dashboardViewID);

                var dashboardPanelViewManager = new DashboardPanelViewManager(applicationContext);
                var dashboardPanelView = dashboardPanelViewManager.LoadViewByID(dashboardViewID, true);

                var viewfilters = new List<DashboardFilterProperty>();
                if (dashboardPanelView != null)
                {
                    if (dashboardPanelView.ViewProperty is SuperDashboardsView)
                        viewfilters = ((SuperDashboardsView)dashboardPanelView.ViewProperty).GlobalFilers;
                    else if (dashboardPanelView.ViewProperty is IndivisibleDashboardsView)
                        viewfilters = ((IndivisibleDashboardsView)dashboardPanelView.ViewProperty).GlobalFilers;
                }

                Dictionary<string, string> filters = DevxChartHelper.GetGlobalFilterList(applicationContext, dashfilter.globalFilter, dashboardPanelView);



                var resultJson = new List<string>();
                var expressions = filters.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Value).ToList();

                string filterQuery = string.Join(" and ", expressions.ToArray());

                for (int i = 0; i < viewfilters.Count; i++)
                {
                    if (filters.FirstOrDefault(x => !string.IsNullOrEmpty(x.Value) && x.Key == viewfilters[i].ID.ToString()).Key == null)
                    {
                        var type = "String";
                        DataTable filteredTable = DevxChartHelper.GetDatatableForGlobalFilter(applicationContext, viewfilters[i], ref type, filterQuery);

                        if (filteredTable != null)
                        {
                            List<string> values = filteredTable.AsEnumerable().Select(x => x.Field<string>("Value")).ToList();

                            string valCollection = string.Join(",", values.ToArray()).Replace("\\", "\\\\");

                            resultJson.Add(string.Format("{0}\"Key\":\"{3}\",\"Values\":\"{2}\"{1}", "{", "}", valCollection, viewfilters[i].ID));
                        }
                    }
                }

                if (resultJson.Count > 0)
                    filteredData.AppendFormat("[{0}]", string.Join(",", resultJson.ToArray()));

                return Ok(filteredData.ToString());
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDashboardFilters: " + ex);
                return null;
            }
        }

        [AllowAnonymous]
        [Route("DeleteDocument")]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteDocument(CommonModel dashfilter)
        {
            DMSManagerService dmsManagerService = new DMSManagerService(HttpContext.Current.GetManagerContext());
            DocumentManager documentManager = new DocumentManager(HttpContext.Current.GetManagerContext());
            bool result = false;
            string status = string.Empty;
            await Task.FromResult(0);
            try
            {
                if (!Guid.TryParse(dashfilter.id, out var newGuid))
                {
                    DMSDocument dmsDocument = dmsManagerService.GetFileListByFileId(dashfilter.id).FirstOrDefault();

                    result = dmsManagerService.DeleteDocumentByfileId(Convert.ToInt32(dashfilter.id));
                }
                else
                {

                    //dmsManagerService.DeleteDocuments(dashfilter.id);

                    Utility.Entities.Document documents = documentManager.Load(x => x.FileID == dashfilter.id).FirstOrDefault();
                    if (documents != null)
                        result = documentManager.Delete(documents);
                }

                if (result)
                    status = "deleted";
                return Ok(status);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in DeleteDocument: " + ex);
                return null;
            }
        }
        [AllowAnonymous]
        [Route("ValidateRuleExpression")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidateRuleExpression(CommonModel expression)
        {
            await Task.FromResult(0);
            string message = string.Empty;
            double expValue = 0;
            expression.name = Uri.UnescapeDataString(expression.name);
            string expressionCopy = expression.name.Replace("\\", @"\\");
            ExpressionCalc expressionCalc = new ExpressionCalc(HttpContext.Current.GetManagerContext());
            expressionCopy = expressionCalc.GetParsedDateExpression(expressionCopy);
            NCalc.Expression expEval = new NCalc.Expression(expressionCopy);
            int messageCode = 0;
            try
            {
                if (!expEval.HasErrors())
                {
                    message = "Formula is valid!";
                    expValue = Convert.ToDouble(expEval.Evaluate());
                    messageCode = 1;
                }
                else
                {
                    message = expEval.Error;
                }

            }
            catch (Exception ex)
            {
                message = "Formula not Valid!:  " + ex.Message;
            }
            return Ok(("{\"messagecode\":" + messageCode + ", \"message\":\"" + message + "\"}").ToString());
        }
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var user = new UserProfile() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in Register: " + ex);
            }

            //return Ok();
            //var manager = Context.GetOwinContext().GetUserManager<UserProfileManager>();
            //var authmanager = Context.GetOwinContext().Get<AuthenticationServiceManager>();
            //var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            //var user = new UserProfile() { UserName = model.Email, Email = model.Email };
            ////IdentityResult result = manager.Create(user, model.Password);
            //IdentityResult result = await manager.CreateAsync(user, model.Password);
            //if (result.Succeeded)
            //{
            //    //if (ddlUserType.SelectedIndex == 0)
            //    //{
            //    //    ContextType contextType = IsConnectedToDomain() ? ContextType.Domain : ContextType.Machine;
            //    //    PrincipalContext principalContext = new PrincipalContext(contextType);
            //    //    UserPrincipal up = UserPrincipal.FindByIdentity(principalContext, System.DirectoryServices.AccountManagement.IdentityType.SamAccountName, user.Email);
            //    //    if (up == null)
            //    //    {
            //    //        UserPrincipal userPrincipal = new UserPrincipal(principalContext, user.Email, "Mayank@123", true);
            //    //        userPrincipal.Name = user.Email;
            //    //        userPrincipal.DisplayName = user.Email;
            //    //        if (contextType != ContextType.Machine)
            //    //        {
            //    //            userPrincipal.UserPrincipalName = user.Email;
            //    //            if (!string.IsNullOrEmpty(user.Email))
            //    //                userPrincipal.EmailAddress = user.Email;
            //    //        }
            //    //        userPrincipal.PasswordNeverExpires = true;
            //    //        userPrincipal.Enabled = true;
            //    //        userPrincipal.Save();
            //    //        manager.AddClaim(user.Id, new Claim("AuthProvider", "Windows"));
            //    //    }
            //}
            //else
            //{
            //    manager.AddClaim(user.Id, new Claim("AuthProvider", "Forms"));
            //    //Session["UserProfile"] = user;
            //}
            return Ok();
        }


        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var info = await Authentication.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return InternalServerError();
                }

                var user = new UserProfile() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in RegisterExternal: " + ex);
                return InternalServerError();
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        [AllowAnonymous]
        [Route("ResetAllUserPasswords")]
        [HttpPost]
        public string ResetAllUserPasswords()
        {
            try
            {
                bool ResetPasswords = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ResetPasswords"]);

                if (ResetPasswords == false)
                    return "Error: Failed to reset some or all User Passwords.";

                var context = HttpContext.Current.GetManagerContext();
                UserProfileManager UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                List<UserProfile> userProfile = UserManager.GetUsersProfile().Where(x => x.Enabled == true).ToList();
                MailMessenger mail = new MailMessenger(context);
                IdentityResult result;

                string SkipUsersForResetPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SkipUsersForResetPassword"]);

                foreach (var user in userProfile)
                {
                    if (!string.IsNullOrEmpty(SkipUsersForResetPassword) && SkipUsersForResetPassword.Contains(user.UserName))
                        continue;

                    string NewPassword = GeneratePassword();
                    string passwordToken = UserManager.GeneratePasswordResetToken(user.Id);
                    result = UserManager.ResetPassword(user.Id, passwordToken, NewPassword);

                    if (result.Succeeded)
                    {
                        ULog.WriteLog($"User: {user.Name}\tUser Name: {user.UserName}\tPassword: {NewPassword}\tStatus: Success");

                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            mail.SendMail(user.Email, "Your New Credentials", "", $"Hello,<br />Your password to access COREM has been reset.<br /> Please use the following link, to login:<br /><a href='https://corem.bcciconst.com'>https://corem.bcciconst.com</a><br /><br /><b>Account Id:</b> {context.TenantAccountId}<br /><b>User Name:</b> {user.UserName}<br /><b>Password:</b> {NewPassword}<br /><br />Thank you,<br /><br />COREM Support Team", true, new string[] { }, true, false);
                        }
                    }
                    else
                    {
                        ULog.WriteLog($"User: {user.Name}\tUser Name: {user.UserName}\tPassword: {NewPassword}\tStatus: Fail");
                    }
                }
            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in ResetAllUserPasswords: " + ex);
                return "Error: Failed to reset some or all User Passwords.";
            }

            return "Success: Passwords reset for All users.";
        }

        [AllowAnonymous]
        [Route("SendTestEmail")]
        [HttpPost]
        public string SendTestEmail()
        {
            try
            {
                bool ResetPasswords = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ResetPasswords"]);

                if (ResetPasswords == false)
                    return "Error: Failed to run test email";

                string email = HttpContext.Current.Request["email"];
                var context = HttpContext.Current.GetManagerContext();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    MailMessenger mail = new MailMessenger(context);
                    mail.SendMail(email, "Test Email!", "", $"Hello User, <br> this is test mail. ", true, new string[] { }, false, false);
                }

            }
            catch (Exception ex)
            {
                //ULog.WriteException(ex);
                ULog.WriteException($"An Exception Occurred in SendTestEmail: " + ex);
                return "Error: Failed to send test email.";
            }

            return "Success: sent test email.";
        }

        [AllowAnonymous]
        [Route("GetDashboardAdvisor")]
        [HttpPost]
        public async Task<IHttpActionResult> GetDashboardAdvisor(MessageBoard messageBoard)
        {
            await Task.FromResult(0);
            List<MessageBoard> messageSuccess = new List<MessageBoard>();
            List<MessageBoard> MessageBoardList = new List<MessageBoard>();
            var applicationContext = HttpContext.Current.GetManagerContext();
            var resultJson = new List<string>();
            try
            {

                MessageBoardList = (List<MessageBoard>)CacheHelper<object>.Get($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID);
                if (MessageBoardList == null)
                {
                    MessageBoardManager objMessageBoardManager = new MessageBoardManager(applicationContext);
                    MessageBoardList = objMessageBoardManager.Load();
                    CacheHelper<object>.AddOrUpdate($"MessageBoard", HttpContext.Current.GetManagerContext().TenantID, MessageBoardList);
                }
                if (UGITUtility.ObjectToString(messageBoard.MessageType) == "Critical")
                {
                    messageSuccess = MessageBoardList.Where(x => x.MessageType == messageBoard.MessageType && (x.Expires >= DateTime.Now || x.Expires == null)).ToList();
                }
                if (UGITUtility.ObjectToString(messageBoard.MessageType) == "Ok")
                {
                    messageSuccess = MessageBoardList.Where(x => (x.MessageType == messageBoard.MessageType && (x.Expires >= DateTime.Now || x.Expires == null)) || (x.MessageType == "Information" && (x.Expires >= DateTime.Now || x.Expires == null))).ToList();
                }
                if (UGITUtility.ObjectToString(messageBoard.MessageType) == "Warning")
                {
                    messageSuccess = MessageBoardList.Where(x => x.MessageType == messageBoard.MessageType && (x.Expires >= DateTime.Now || x.Expires == null)).ToList();
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(messageSuccess);
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDashboardFilters: " + ex);
                return null;
            }
        }
        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion


        ///Service related agent

        [HttpPost]
        [Route("ServiceRelatedAgent")]
        [AllowAnonymous]
        public IHttpActionResult ServiceRelatedAgent(ServiceAgent serviceAgent)
        {
            //var tenantID = "c345e784-aa08-420f-b11f-2753bbebfdd5";

            string moduleTypeTable = string.Empty;
            string prpUser = string.Empty;
            string title = string.Empty;
            string columnName = string.Empty;
            string error = string.Empty;
            string message = string.Empty;

            //bool AvailableAsset = false;
            bool ticketClosed = false;

            DataTable NewMatch = new DataTable();
            DataTable moduleTaskList = new DataTable();
            DataTable groupOfOpenTicketPRP = new DataTable();
            DataTable groupOfClosedTicketPRP = new DataTable();
            DataRow childTicket;

            // Workflow workflow = new Workflow();

            Ticket TicketRequest = null;
            //ApplicationContext context = ApplicationContext.Create();//need to set for respective tenant
            //ApplicationContext context = HttpContext.Current.GetManagerContext();

            UGITTaskManager TaskManager = new UGITTaskManager(_applicationContext);

            AvailablePRPAndAssignTo AvailablePRPAndAssignTo = new AvailablePRPAndAssignTo(_applicationContext);

            AssetModelViewManager AssetModelViewManager = new AssetModelViewManager(_applicationContext);

            AssetsManger AssetsManger = new AssetsManger(_applicationContext);

            var _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            ServiceAgentsManager serviceAgentsManager = new ServiceAgentsManager(_applicationContext);

            try
            {
                UserProfile userProfile = _userProfileManager.LoadById(_applicationContext.CurrentUser.Id);
                //UserProfile userProfile = _userProfileManager.LoadById("48b7d2da-1af0-40b7-aee7-031c0adae6da");

                //var multiTaskID = serviceAgent.Workflow.TaskId.Split(',');

                var ticketRelatedTask = TaskManager.LoadByProjectID("SVC", Convert.ToString(serviceAgent.Workflow.TicketId));

                //serviceAgentsManager.SetPRP(ticketRelatedTask);
                // foreach (var multiTaskId in multiTaskID)

                {
                    var moduleTasksOrTicketData = ticketRelatedTask.Where(x => x.Title.StartsWith(serviceAgent.Workflow.TaskTitle)).FirstOrDefault();

                    if (moduleTasksOrTicketData != null)
                    {

                        moduleTypeTable = moduleTasksOrTicketData.Behaviour.ToLower() == "task" ? DatabaseObjects.Tables.ModuleTasks : "SVCRequests";

                        title = moduleTasksOrTicketData.Title;

                        columnName = moduleTypeTable == DatabaseObjects.Tables.ModuleTasks ? DatabaseObjects.Columns.AssignedTo : DatabaseObjects.Columns.PRP;

                        //if (GetTableDataManager.IsLookaheadTicketExists(moduleTable, context.TenantID, serviceAgent.Workflow.Title, "",true))
                        {

                            if (moduleTasksOrTicketData.Behaviour.ToLower() == "task")
                            {
                                groupOfOpenTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, _applicationContext.TenantID, title, columnName, "Waiting");

                                groupOfClosedTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, _applicationContext.TenantID, title, columnName, "Completed");
                            }
                            else
                            {
                                groupOfOpenTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, _applicationContext.TenantID, title, columnName, 0, true);

                                groupOfClosedTicketPRP = GetTableDataManager.autoSetPRP(moduleTypeTable, _applicationContext.TenantID, title, columnName, 1, true);
                            }
                            columnName = moduleTasksOrTicketData.Behaviour.ToLower() == "task" ? DatabaseObjects.Columns.AssignedTo : DatabaseObjects.Columns.PRP;

                            prpUser = AvailablePRPAndAssignTo.GetPRPOrAssignTo(groupOfOpenTicketPRP, groupOfClosedTicketPRP, columnName);

                            var listOfTask = ticketRelatedTask.Where(x => x.ID == Convert.ToInt64(moduleTasksOrTicketData.ID)).ToList();

                            AssetData assetData = serviceAgentsManager.UserQuestionSummary(serviceAgent.Workflow.TicketId);

                            AssetModel availableAsset = AssetModelViewManager.Get($"where {DatabaseObjects.Columns.ModelName} like '%{assetData.AssetModel}%' and {DatabaseObjects.Columns.TenantID}='{_applicationContext.TenantID}'");

                            //assetModel check
                            if (availableAsset != null)
                            {

                                var assetTicket = AssetsManger.Get($"where {DatabaseObjects.Columns.AssetModelLookup}={availableAsset.ID} and {DatabaseObjects.Columns.StageStep}=2 and {DatabaseObjects.Columns.TenantID}='{_applicationContext.TenantID}'");

                                if (assetTicket != null)
                                {
                                    //assetTicket.CurrentUser = "48b7d2da-1af0-40b7-aee7-031c0adae6da";//Need to assign current user which is created
                                    assetTicket.CurrentUser = serviceAgent.Workflow.TaskTitle.Split(':').Last();//Need to assign current user which is created

                                    var assignValue = AssetsManger.Update(assetTicket);

                                    var ticketData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Assets, $"{DatabaseObjects.Columns.TicketId}='{assetTicket.TicketId}'");

                                    TicketRelationshipHelper tRelation = new TicketRelationshipHelper(_applicationContext, moduleTasksOrTicketData.RelatedTicketID, assetTicket.TicketId);

                                    int rowEffected = tRelation.CreateRelation(_applicationContext);

                                    TicketRequest = new Ticket(_applicationContext, "CMDB");


                                    //UserProfile userProfile = _userProfileManager.LoadById("48b7d2da-1af0-40b7-aee7-031c0adae6da");
                                    //Need to add current user



                                    error = TicketRequest.Approve(userProfile, ticketData.Rows[0]);

                                    if (string.IsNullOrEmpty(error))
                                    {
                                        error = TicketRequest.CommitChanges(ticketData.Rows[0]);
                                    }
                                    //add user in  asset


                                    // childTicket = GetTableDataManager.GetTableData(serviceAgent.Workflow.ChildModuleName, $"{DatabaseObjects.Columns.TicketId}='{moduleTasksOrTicketData.RelatedTicketID}'").Select().FirstOrDefault();

                                    //TicketRequest = new Ticket(_applicationContext, serviceAgent.Workflow.ChildModuleName);
                                    //TicketRequest.CloseTicket(childTicket[0], uHelper.GetCommentString(userProfile, $"Add purchase order as asset is not available.", childTicket[0], DatabaseObjects.Columns.TicketComment, false));

                                    //error = TicketRequest.CommitChanges(childTicket);
                                    //if (string.IsNullOrEmpty(error))
                                    //{

                                    //    moduleTasksOrTicketData.Status = "Completed";

                                    //    ticketClosed = true;
                                    //}
                                    //else
                                    //{
                                    //    //Display error message
                                    //}


                                }

                                else
                                {
                                    message = "Asset is not available in the Inventry";
                                    //return;
                                }

                                childTicket = GetTableDataManager.GetTableData(serviceAgent.Workflow.ChildModuleName, $"{DatabaseObjects.Columns.TicketId}='{moduleTasksOrTicketData.RelatedTicketID}'").Select().FirstOrDefault();

                                TicketRequest = new Ticket(_applicationContext, serviceAgent.Workflow.ChildModuleName);

                                TicketRequest.CloseTicket(childTicket, uHelper.GetCommentString(userProfile, message, childTicket, DatabaseObjects.Columns.TicketComment, false));

                                error = TicketRequest.CommitChanges(childTicket);

                                if (string.IsNullOrEmpty(error))
                                {

                                    moduleTasksOrTicketData.Status = "Completed";

                                    ticketClosed = true;
                                }
                                else
                                {
                                    //Display error message
                                }

                                if (listOfTask != null)
                                {
                                    foreach (UGITTask item in listOfTask)
                                    {
                                        item.AssignedTo = prpUser;

                                        if (ticketClosed)

                                            item.Status = "Completed";

                                        item.Changes = true;
                                    }
                                }
                                TaskManager.SaveTasks(ref listOfTask, "SVC", serviceAgent.Workflow.TicketId);

                            }

                            else
                            {
                                //remove same code
                                TicketRequest = new Ticket(_applicationContext, "TSR");

                                childTicket = GetTableDataManager.GetTableData(serviceAgent.Workflow.ChildModuleName, $"{DatabaseObjects.Columns.TicketId}='{moduleTasksOrTicketData.RelatedTicketID}'").Select().FirstOrDefault();

                                childTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(userProfile, "test comment", childTicket, DatabaseObjects.Columns.TicketComment, false);

                                TicketRequest.CommitChanges(childTicket);
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetDashboardFilters: " + ex);
                return InternalServerError();
            }
            return Ok();
        }
    }
}
