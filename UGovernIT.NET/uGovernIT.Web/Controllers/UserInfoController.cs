using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;

namespace uGovernIT.Web.Controllers
{
    [RoutePrefix("api/userinfo")]
    public class UserInfoController : ApiController
    {
        private ApplicationContext _applicationContext = null;
        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        UserRoleManager UserRoleMgr = null;
        GlobalRoleManager rolesManager = null;
        public UserInfoController()
        {
            _applicationContext = HttpContext.Current.GetManagerContext();
            UserRoleMgr = new UserRoleManager(_applicationContext);
            rolesManager = new GlobalRoleManager(_applicationContext);
        }

        [HttpPost]
        [Route("CreateUsers")]

        public async Task<IHttpActionResult> CreateUsers(UserInfoRequestModel userInfoRequestModel)
        {
            await Task.FromResult(0);
            CreateUsersResponse createUsersResponse = new CreateUsersResponse();

            TenantValidation tenantValidation = new TenantValidation(_applicationContext);

            //bool IsUsersCreated = false;
          
            //bool IsUserLimitExceed = false;
            // Deserializes userInfoRequestModel object the get list of userprofiles

            if (userInfoRequestModel != null)
            {
                if (userInfoRequestModel.UserData.Count != 0)
                {
                    var sb = new StringBuilder();

                    foreach (var item in userInfoRequestModel.UserData)
                    {
                        if (tenantValidation.IsUserLimitExceed())
                        {
                            createUsersResponse.IsUserLimitExceed = true;
                            sb.Append(item.username);
                            sb.Append(",");

                        }
                        else
                        {

                            UserProfile user = new UserProfile();
                            Users userNAme = new Users();
                            user.UserName = item.email;
                            user.Name = item.username;
                            userNAme.userName = item.username;
                            user.LoginName = item.username;
                            user.Email = item.email;
                            user.TenantID = _applicationContext.TenantID;
                            if (!string.IsNullOrEmpty(item.role))
                            {
                                var Role = rolesManager.Load().FirstOrDefault(x => x.Name == item.role);
                                //var Role = umanager.GetUserRoleByGroupName(item.role);

                                if (Role == null)
                                {
                                    if (!string.IsNullOrEmpty(item.role))
                                    {
                                        GlobalRole globalRole = new GlobalRole();
                                        globalRole.Name = item.role;
                                        //globalRole.Description = memoDescription.Text;
                                        globalRole.Id = UGITUtility.ObjectToString(Guid.NewGuid());
                                        rolesManager.Insert(globalRole);
                                        user.GlobalRoleId = globalRole.Id;
                                    }
                                }
                                if (Role != null && Role.Id != string.Empty)
                                {
                                    //user.UserRoleId = Role.Id;
                                   // user.isRole = true;
                                    user.GlobalRoleId = Role.Id;
                                    //user.isRole = true;

                                }
                            }
                            // user.= item.role;
                            var password = umanager.GeneratePassword();

                            IdentityResult result = umanager.Create(user, password.Trim());

                            if (result.Succeeded)
                            {
                                //IsUsersCreated = true;
                                createUsersResponse.IsUsersCreated = true;
                                string encryUserId = uGovernITCrypto.Encrypt(user.Id, "ugitpass");
                                string encrTenAccId = uGovernITCrypto.Encrypt(_applicationContext.TenantID, "ugitpass");
                                string encrword = uGovernITCrypto.Encrypt(password, "ugitpass");
                                SendEmailToNewApplicaitonRequester(user.Email, _applicationContext.TenantAccountId, user.UserName, password, encryUserId, encrTenAccId, encrword, user.Name);
                            }
                            else
                            {
                                createUsersResponse.IsUsersCreated = false;
                                sb.Append(item.username);
                                sb.Append(",");
                                //createUsersResponse.userList.Add(userNAme);
                            }
                        }
                        createUsersResponse.userName = sb.ToString();

                    }

                }
            }


            //return Json(new { success = true, message = "Order updated successfully" }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            string jsonmodules = JsonConvert.SerializeObject(createUsersResponse);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonmodules, System.Text.Encoding.UTF8, "application/json");
            return ResponseMessage(response);

            ////uHelper.ClosePopUpAndEndResponse(_applicationContext, true);
            //await Task.FromResult(0);
            //return Ok();
        }

        public class CreateUsersResponse
        {
            public bool IsUsersCreated { get; set; }
            public bool IsUserLimitExceed { get; set; }
            public string userName { get; set; }

        }

        public class Users
        {
            public string userName { get; set; }
        }

        private void SendEmailToNewApplicaitonRequester(string emailID, string accountID, string userName, string password, string encryUserId = null, string encrTenAccId = null, string encrword = null, string name = null)
        {
            string subject = "Your registration is successful!";

            //string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}Account/Login.aspx";
            string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/UserVerfication?id={encryUserId}&acc={encrTenAccId}&di={encrword}";
            string userCredentials = $"<br> Account id ={accountID} <br> User Name={userName} <br> Password={password}";
            string CurrentUserName = _applicationContext.CurrentUser.Name;
            string htmlBody = @"<html>
                                    <head></head>
                                    <body>
                                          <p> Dear " + name + @": <br><br> You are set up as a user of Service Prime. <br><br>  You can create IT requests using a simple portal that is now set up for you. <br><br> This email contains your details: <strong> " + userCredentials + @" </strong>
                                              <br><br><br>
                                              Please <a href=" + SiteUrl + @">click here </a> to activate your account. 
                                         </p>
                                          <p>
                                            <br><br> Thanks & Regards <br> " + CurrentUserName + @"<br>Service Prime.
                                         </p>
                                    </body>
                                </html>";

            var mail = new MailMessenger(HttpContext.Current.GetManagerContext());

            var response = mail.SendMail(emailID, subject, "", htmlBody, true);

        }


        #region Roles

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IHttpActionResult> GetRoles()
        {
            await Task.FromResult(0);
            try
            {
                List<GlobalRole> roles = rolesManager.Load();

                List<UserRole> userRoles = new List<UserRole>();
                var sb = new StringBuilder();
                foreach (var role in roles)
                {
                    var userrole = new UserRole();
                    userrole.value = role.Name;
                    userRoles.Add(userrole);
                }

                if (roles != null)
                {
                    string jsonmodules = JsonConvert.SerializeObject(userRoles);
                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jsonmodules, Encoding.UTF8, "application/json");
                    return ResponseMessage(response);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetRoles: " + ex);
                return InternalServerError();
            }
        }

        public class  UserRole
        {
            public string value { get; set; }
        }

        #endregion
    }
}
