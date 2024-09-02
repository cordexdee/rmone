
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Helpers;

namespace uGovernIT.Manager
{
    /// <summary>
    /// The UserManager provides for manipulation of all user instances.
    /// </summary>
    /// <remarks>
    /// In this case we are wrapping the provided UserManager and providing a way for it to be
    /// initialized with an EF backed store. The store provides the lower level mechanism
    /// by which operations on the manager are carried out.
    /// </remarks>
    public class UserProfileManager : UserManager<UserProfile>
    {
        ApplicationContext context = null;
        UserRoleManager uRole = null;
        List<Role> roleCollection = null;

        public UserProfileManager(ApplicationContext _context)
             : base(new UserStore<UserProfile>(_context))
        {
            context = _context;
        }

        public ClaimsIdentity GenerateUserIdentity(UserProfile user)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = this.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            ////claim code start - This code is better way to store superadmin Id we will use it  during security optimaztion
            //if(user.ParentUserId != null)
            //{
            //    Claim claim = new Claim("IsFromSuperAdmin", "True");
            //    userIdentity.AddClaim(claim);
            //}
            //claim  code end 
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserProfile user, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await this.CreateIdentityAsync(user, authenticationType);
            // Add custom user claims here
            userIdentity.AddClaims(new[]
            {
               new Claim(ClaimTypes.GivenName, user.Name),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Sid, user.Id)
            });

            // Add custom user claims here
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(UserProfile user)
        {
            Task.FromResult(0);
            var userIdentity = GenerateUserIdentity(user);
            userIdentity.AddClaims(new[]
            {
               new Claim(ClaimTypes.GivenName, user.Name),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Sid, user.Id)
            });
            return Task.FromResult(userIdentity);
        }

        public bool CheckPassword(UserProfile user)
        {
            return this.HasPassword(user.Id);
        }

        public UserProfileManager Create(IdentityFactoryOptions<UserProfileManager> options, IOwinContext context)

        {
            var manager = new UserProfileManager(context.Get<ApplicationContext>());
            // Configure validation logic for usernames

            manager.UserValidator = new CustomUserValidator<UserProfile>(manager);

            //manager.UserValidator = new Microsoft.AspNet.Identity.UserValidator<UserProfile>(manager)
            //{
            //    AllowOnlyAlphanumericUserNames = false,
            //    RequireUniqueEmail = false,
            //};

            // Configure validation logic for passwords
            manager.PasswordValidator = new Microsoft.AspNet.Identity.PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});
            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});



            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = System.TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<UserProfile>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public UserProfile GetUserByUserName(string userName)
        {
            userName = Regex.Replace(userName, @"\s", "");
            UserProfile user = this.FindByName(userName);
            return user;
        }

        public UserProfile GetUserOnlyByUserName(string userName)
        {
            UserStore<UserProfile> userStore = new UserStore<UserProfile>(context);
            UserProfile user = userStore.FindOnlyByName(userName);
            // UserProfile user = GetUsersProfileWithGroup().FirstOrDefault(x => x.UserName == userName || x.Email == userName);
            return user;
        }

        public UserProfile GetUserByBothUserNameandDisplayName(string userName)
        {
            UserStore<UserProfile> userStore = new UserStore<UserProfile>(context);
            UserProfile user = userStore.FindOnlyByName(userName);
            if (user == null)
                user = userStore.FindOnlyByDisplayName(userName);
            return user;
        }

        public bool IsResourceAdmin(UserProfile user)
        {
            if (user == null || user.Id == null)
                return false;
            //return this.IsInRole(user.Id, RoleType.ResourceAdmin.ToString());

            if (context.UserManager.IsUGITSuperAdmin(user))
                return true;

            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            string adminGroup = configManager.GetValue(ConfigConstants.ResourceAdminGroup);

            if (!string.IsNullOrEmpty(adminGroup))
                return CheckUserIsInResourceGroup(adminGroup, user);
            else
                return this.IsInRole(user.Id, RoleType.ResourceAdmin.ToString());
        }

        /// <summary>
        /// Checks if user is part of the group(s) passed in
        /// </summary>
        /// <param name="groupNames">One or more groups separated by ";#"</param>
        /// <param name="user">User whose membership to check</param>
        /// <returns></returns>
        public bool CheckUserIsInResourceGroup(string groupNames, UserProfile user)
        {
            if (string.IsNullOrEmpty(groupNames) || user == null)
                return false;

            string[] groups = UGITUtility.SplitString(groupNames, uGovernIT.Utility.Constants.Separator);
            if (groups != null && groups.Count() > 0)
                return IsUserinGroups(groupNames, user.UserName, uGovernIT.Utility.Constants.Separator);

            return false;
        }

        /// <summary>
        /// Check if user belongs to any group mentioned in ConfigVariable 'AdminGroupName'.
        /// </summary>
        public bool IsinAdminGroup(UserProfile user)
        {
            if (user == null || user.Id == null)
                return false;

            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            string adminGroup = configManager.GetValue(ConfigConstants.AdminGroup);

            if (!string.IsNullOrEmpty(adminGroup))
                return IsUserinGroups(adminGroup, user.UserName, uGovernIT.Utility.Constants.Separator);
            else
                return false;
        }

        public bool IsAdmin(UserProfile user)
        {
            if (user == null || user.Id == null)
                return false;
            return this.IsInRole(user.Id, RoleType.Admin.ToString());
        }

        public bool IsUGITSuperAdmin(UserProfile user)
        {
            if (user == null || user.Id == null)
                return false;
            return this.IsInRole(user.Id, RoleType.UGITSuperAdmin.ToString());
        }

        public List<UserProfile> RMMUserList()
        {
            return this.Load(x => this.IsInRole(x.Id, RoleType.Admin.ToString()));
        }

        public bool IsTicketAdmin(UserProfile user)
        {
            if (user == null || user.Id == null)
                return false;
            if (IsinAdminGroup(user))
                return true;

            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            string ticketadminGroup = configManager.GetValue(ConfigConstants.TicketAdminGroup);
            if(!string.IsNullOrEmpty(ticketadminGroup) && CheckUserIsInResourceGroup(ticketadminGroup, user))
                return true;
            else
                return this.IsInRole(user.Id, RoleType.TicketAdmin.ToString());
            }

        public bool CheckUserIsInGroup(string groupName, UserProfile user)
        {
            bool result = false;
            if (user == null || user.Id == null)
                return false;
            UserRoleManager userRoleManager = new UserRoleManager(context);
            Role role = userRoleManager.GetRoleByName(groupName);
            result = this.IsInRole(user.Id, groupName);
            if (!result && role != null)
            {
                return this.IsInRole(user.Id, role.Name);
            }
            return result;
        }

        public void AddUserRole(UserProfile user, string role)
        {
            var uUsers = Task.Run(async () => { await (this.Store as UserStore<UserProfile>).AddToRoleAsync(user, role); });
            Task.WaitAll(uUsers);
        }

        public void DeleteUserRole(UserProfile user, string role)
        {
            var uUsers = Task.Run(async () => { await (this.Store as UserStore<UserProfile>).RemoveFromRoleAsync(user, role); });
            Task.WaitAll(uUsers);
        }

        public List<string> GetUserGroups(string userIDs)
        {
            List<string> roleList = new List<string>();
            userIDs.Split(',').ToList().ForEach(x =>
            {
                List<Role> roles = this.GetUserRoles(x);
                if (roles != null && roles.Count > 0)
                {
                    roles.ForEach(y =>
                    {
                        string id = y.Id;
                        if (!roleList.Exists(z => z == id))
                            roleList.Add(y.Id);
                    });
                }
            });
            return roleList;
        }

        public string NameFromUser(string uid)
        {
            return CommaSeparatedNamesFrom(uid);
        }

        public string GetDisplayNameFromUserId(string uid)
        {
            if (string.IsNullOrEmpty(uid))
                return string.Empty;

            UserProfileManager umanager = new UserProfileManager(context);
            List<UserProfile> lstUserProfiles = GetUsersProfile();
            UserProfile user = lstUserProfiles.FirstOrDefault(x => x.Id == uid);

            if (user != null)
            {
                return user.Name;
            }
            return string.Empty;
        }

        public List<KeyValuePair<string, string>> GetUserNames(List<string> userIds, string tenantId)
        {
            var userCollection = new List<KeyValuePair<string, string>>();
            if (userIds == null || !userIds.Any())
                return userCollection;

            var userProfiles = this.Users;
            if (userProfiles == null)
            {
                var userProfileManager = new UserProfileManager(context);
                userProfiles = userProfileManager.Users;
            }

            userCollection = userProfiles
                .Where(x => x.TenantID.EqualsTo(tenantId)
                            && userIds.Any(u => u.EqualsTo(x.Id)))
                .Select(x => new KeyValuePair<string, string>(x.Id, x.Name))
                .ToList();

            return userCollection;
        }

        public string CommaSeparatedNamesFrom(string userString)
        {
            if (string.IsNullOrEmpty(userString))
                return string.Empty;

            List<UserProfile> userCollection = GetUsersProfileWithGroup();  //.GetAllSiteUsers();

            string[] userList = UGITUtility.SplitString(userString, Utility.Constants.Separator6);
            string commaSepratedNames = string.Empty;
            if (userList != null)
            {
                foreach (string s in userList)
                {
                    if (userCollection.Any(x => x.Id == s))
                    {
                        UserProfile u = userCollection.Where(x => x.Id == s).FirstOrDefault();
                        commaSepratedNames = commaSepratedNames + ',' + u.Name;
                    }
                }
            }

            if (!string.IsNullOrEmpty(commaSepratedNames))
                commaSepratedNames = commaSepratedNames.Remove(0, 1);
            return commaSepratedNames;
        }

        public bool IsUserPresentInModuleUserTypes(UserProfile userToBeSearched, DataRow item, string moduleName)
        {
            //Get All Module Usertypes
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            DataTable ModulUserTypesTable = moduleViewManager.LoadModuleListByName(moduleName, DatabaseObjects.Tables.ModuleUserTypes);
            //Query to filter the UserTypes for the given moduleName
            string query = string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            //UserTypse for the given moduleName
            DataRow[] moduleUserTypes = ModulUserTypesTable.Select(query);

            foreach (DataRow moduleUserType in moduleUserTypes)
            {
                if (IsUserPresentInField(userToBeSearched, item, Convert.ToString(moduleUserType[DatabaseObjects.Columns.ColumnName])))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// It tooks ;# sperated fieldnames in which we need to check user.
        /// </summary>
        /// <param name="userToBeSearched"></param>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool IsUserPresentInField(UserProfile user, DataRow item, string fieldName, bool includeMyDelegates = false)
        {
            if (user == null || string.IsNullOrEmpty(fieldName))
                return false;
            string[] fieldTypes = UGITUtility.SplitString(fieldName, Utility.Constants.Separator);
            List<UserProfile> loggedInUsers = new List<UserProfile>();
            loggedInUsers.Add(user);
            if (includeMyDelegates)
            {

                if (!string.IsNullOrEmpty(user.DelegateUserFor))
                {
                    List<string> delegateUserString = UGITUtility.ConvertStringToList(user.DelegateUserFor, Utility.Constants.Separator6);
                    foreach (string uVal in delegateUserString)
                    {
                        UserProfile sndUserProfile = GetUsersProfile().Where(x => x.Id == uVal).FirstOrDefault();
                        try
                        {
                            if (sndUserProfile != null && DateTime.Now.Date >= sndUserProfile.LeaveFromDate && DateTime.Now.Date <= sndUserProfile.LeaveToDate)
                            {
                                loggedInUsers.Add(sndUserProfile);
                            }

                        }
                        catch (Exception ex)
                        {
                            Util.Log.ULog.WriteException(ex);
                        }
                    }
                }
            }
            if (loggedInUsers.Count == 0)
                return false;
            foreach (string fieldType in fieldTypes)
            {
                try
                {
                    if (item != null && UGITUtility.IfColumnExists(fieldType.Trim(), item.Table) && item[fieldType.Trim()] != null)
                    {
                        string[] users = UGITUtility.SplitString(item[fieldType.Trim()], ",");
                        if (users != null)
                        {
                            foreach (string u in users)
                            {
                                UserProfile userprofile = GetUserById(u);
                                if (userprofile != null)
                                {
                                    if (loggedInUsers.Exists(x => x.Id == u))
                                        return true;
                                    if (userprofile.isRole)
                                    {
                                        foreach (UserProfile loginUser in loggedInUsers)
                                        {
                                            List<string> userids = new List<string>() { u };
                                            return IsUserinGroups(loginUser.Id, userids);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (UserProfile loginUser in loggedInUsers)
                                    {
                                        if (CheckIfUserGroup(fieldType.Trim(), loginUser.Id))
                                            return true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (UserProfile loginUser in loggedInUsers)
                        {
                            if (CheckUserIsInGroup(fieldType.Trim(), loginUser))
                                return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }

            return false;
        }
        public bool IsUserPresentInUserCollection(UserProfile userToBeSearched, string userCommaSeperated)
        {
            if (userToBeSearched == null || string.IsNullOrWhiteSpace(userCommaSeperated))
                return false;

            List<string> userAndGroups = new List<string>();
            userAndGroups.Add(userToBeSearched.Id);
            List<string> lstUserIds = UGITUtility.SplitString(userCommaSeperated, Utility.Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<Role> lstOfGroups = GetUserGroupById(userToBeSearched.Id);
            foreach (Role group in lstOfGroups)
            {
                userAndGroups.Add(group.Id);
            }

            return (lstUserIds.Exists(x => userAndGroups.Contains(x)));
        }
        public List<UserProfile> GetUserInfosById(string userIds)
        {
            List<UserProfile> users = new List<UserProfile>();

            if (string.IsNullOrEmpty(userIds))
                return users;

            string separator = Utility.Constants.Separator;

            if (userIds.Contains(Utility.Constants.Separator))
                separator = Utility.Constants.Separator;
            else if (userIds.Contains(Utility.Constants.Separator6))
                separator = Utility.Constants.Separator6;

            List<string> lstUserIds = UGITUtility.SplitString(userIds, separator, StringSplitOptions.RemoveEmptyEntries).ToList();
            UserProfile user = null;
            foreach (string userId in lstUserIds)
            {
                user = GetUserById(userId);
                if (user != null)
                    users.Add(user);
            }
            return users;
        }

        public List<Role> GetUserGroupById(string userIds)
        {
            List<Role> users = new List<Role>();
            GetUserGroups().Where(x => x.Id == userIds).ToList();
            List<string> lstUserIds = UGITUtility.SplitString(userIds, Utility.Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string userId in lstUserIds)
            {
                Role user = GetUserGroups().Where(x => x.Id == userId).SingleOrDefault();
                if (user != null)
                    users.Add(user);
            }
            return users;
        }

        public List<UserProfile> GetUserInfosById(string[] userIds)
        {
            List<UserProfile> users = new List<UserProfile>();
            List<string> lstUserIds = userIds.ToList();
            foreach (string userId in lstUserIds)
            {
                users.Add(GetUserInfoById(userId));
            }
            return users;

        }

        public UserProfile GetUserInfoById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var userInfo = LoadWithGroup(x => x.Id == userId, take: 1).FirstOrDefault();
            if (userInfo == null)
            {
                userInfo = new UserProfile();
            }
            return userInfo;
        }

        /// <summary>
        /// Get User Information By Name or Id (Guid)
        /// </summary>
        public UserProfile GetUserInfoByIdOrName(string user)
        {
            if (string.IsNullOrEmpty(user))
                return null;

            Guid usr = Guid.Empty;
            Guid.TryParse(user, out usr);

            if (usr != Guid.Empty)
            {
                var userInfo = LoadWithGroup(x => x.Id == user, take: 1).FirstOrDefault();
                if (userInfo == null)
                {
                    userInfo = new UserProfile();
                }
                return userInfo;
            }
            else
            {
                var userInfo = LoadWithGroup(x => x.UserName.Equals(user, StringComparison.InvariantCultureIgnoreCase) || x.Name.Equals(user, StringComparison.InvariantCultureIgnoreCase), take: 1).FirstOrDefault();
                if (userInfo == null)
                {
                    userInfo = new UserProfile();
                }
                return userInfo;
            }
        }

        public UserProfile GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;
            UserProfile user = LoadWithGroup(x => x.Id == userId, take: 1).SingleOrDefault();
            if (user == null)
                user = GetUserInfoByIdOrName(userId);
            return user;
        }

        public UserProfile GetUserProfile(string userId) // For Report to run on QA site, as it is not getting logged in TenantId
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            UserProfile user = ((UserStore<UserProfile>)this.Store).Load(x => x.Id == userId).SingleOrDefault(); //GetUsersProfileWithGroup().SingleOrDefault(x => x.Id == userId);            
            return user;
        }


        public bool isCurrentUser(string id)
        {
            return GetUserByUserName(HttpContext.Current.User.Identity.Name.ToString()).Id == id;
        }

        public void DeleteFromGroupById(string userId, string role)
        {

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(role))
            {
                UserProfileManager umanager = new UserProfileManager(context);
                IdentityResult i = umanager.RemoveFromRole(userId, role.Replace(" ", ""));
            }


        }

        /// <summary>
        /// Returns User's Name, by UserId (Guid)
        /// </summary>
        public string GetUserNameById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            UserProfile user = LoadWithGroup(x => x.Id == userId, take: 1).SingleOrDefault();
            return user != null ? user.Name : "";
        }

        public void AddInGroupById(string userId, string role)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(role))
            {
                UserProfileManager umanager = new UserProfileManager(context);
                string[] roleList = role.Split(',');
                foreach (string roleName in roleList)
                    if (!string.IsNullOrEmpty(roleName) && !string.IsNullOrEmpty(userId))
                    {
                        if (!umanager.IsInRole(userId, roleName))
                        {
                            umanager.AddToRole(userId, roleName);
                        }
                    }
            }
        }

        public void DeleteUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return;
            UserProfileManager umanager = new UserProfileManager(context);
            UserProfile uprofile = umanager.FindById(userId);
            if (uprofile != null)
            {
                IdentityResult i = umanager.Delete(uprofile);
            }
        }

        public string DisableUserById(string userId, bool updateRequired = true)
        {
            if (string.IsNullOrEmpty(userId))
                return "";
            UserProfileManager umanager = new UserProfileManager(context);
            if(updateRequired) //update Enabled and UGITEndDate here.
            { 
                UserProfile uprofile = umanager.FindById(userId);
                uprofile.Enabled = false;
                uprofile.UGITEndDate = DateTime.Now;
                IdentityResult i = umanager.Update(uprofile);
            }
            //else update will get executed in the caller page.

            List<string> existingRoles = umanager.GetRoles(userId).ToList();
            existingRoles.ForEach(x => { umanager.DeleteFromGroupById(userId, x); });
            return string.Join(uGovernIT.Utility.Constants.UserInfoSeparator, existingRoles);
        }

        public void EnableUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return;
            UserProfileManager umanager = new UserProfileManager(context);
            UserProfile uprofile = umanager.FindById(userId);
            uprofile.Enabled = true;
            uprofile.UGITEndDate = new DateTime(8900, 12, 31);
            IdentityResult i = umanager.Update(uprofile);
        }

        public string CommaSeparatedAccountsFrom(List<UserProfile> userCollection, string separator)
        {
            if (userCollection == null || userCollection.Count == 0)
                return string.Empty;

            string userList = string.Empty;
            foreach (UserProfile user in userCollection)
            {
                if (user != null && user.UserName != null)
                {
                    if (userList != string.Empty)
                        userList += separator;
                    userList += user.UserName;
                }
            }
            return userList;
        }

        public string CommaSeparatedNamesFrom(List<string> userids, string separator, List<UserProfile> userProfiles = null)
        {
            if (userids == null || userids.Count == 0)
                return string.Empty;

            string names = string.Empty;

            List<UserProfile> userCollection = userProfiles != null
                ? userProfiles.Where(x => userids.Contains(x.Id) && !string.IsNullOrEmpty(x.UserName)).ToList()
                : GetUsersProfileWithGroup().Where(x => userids.Contains(x.Id) && !string.IsNullOrEmpty(x.UserName)).ToList();

            if (userCollection != null && userCollection.Count > 0)
                names = string.Join(separator, userCollection.Select(x => x.Name).ToArray()).Replace(",", ", ");

            //List<Role> groupCollection = GetUserGroups().Where(x => userids.Contains(x.Id)).ToList();
            //if (groupCollection != null && groupCollection.Count > 0)
            //{
            //    if (!string.IsNullOrEmpty(names))
            //        names = $"{names}, ";

            //    names += string.Join(separator, groupCollection.Select(x => x.Title).ToArray()).Replace(",", ", ");
            //}

            return names;
        }

        public string CommaSeparatedNamesFrom(DataRow item, List<string> userString, string separator)
        {
            if (userString == null || userString.Count == 0)
                return string.Empty;

            List<string> userList = new List<string>();

            foreach (string u in userString)
            {
                List<string> userType = UGITUtility.ConvertStringToList(u, separator);
                var filteredUserType = userType.Where(x => !string.IsNullOrWhiteSpace(x) && UGITUtility.IsSPItemExist(item, x)).ToList();
                if (filteredUserType.Count == 0)
                {
                    //UserProfile userProfile = GetUserByUserName(u);
                    // Above line commented as, a OPM ticket opened first time, is crashing, after refreshing entire Cache, due to setting of TenantId of context to Default TenantId, instead of logged in TenantId.

                    UserProfile userProfile = GetUserByBothUserNameandDisplayName(u);
                    if (userProfile != null && !string.IsNullOrWhiteSpace(userProfile.Name))
                        userList.Add(userProfile.Name);
                }
                //foreach (string utype in userType)
                //{
                //    if (!string.IsNullOrWhiteSpace(utype))
                //    {
                //        if (UGITUtility.IsSPItemExist(item, utype))
                //        {
                //            string id = Convert.ToString(item[utype]);
                //            if (!string.IsNullOrWhiteSpace(id))
                //            {                                
                //                List<UserProfile> users = GetUsersProfileWithGroup(context.TenantID, id).ToList();
                //                if (users != null)
                //                    userList.AddRange(users.Select(x => x.Name));
                //                else
                //                {
                //                    string[] ids = id.Split(',');
                //                    userList.AddRange(ids);
                //                }
                //            }
                //        }
                //    }
                //}

                foreach (string utype in filteredUserType)
                {
                    string id = Convert.ToString(item[utype]);
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        List<UserProfile> users = GetUsersProfileWithGroup(context.TenantID, id).ToList();
                        if (users != null)
                            userList.AddRange(users.Select(x => x.Name));
                        else
                        {
                            string[] ids = id.Split(',');
                            userList.AddRange(ids);
                        }
                    }
                }
            }
            return string.Join(uGovernIT.Utility.Constants.UserInfoSeparator, userList.Distinct().ToArray());
        }

        public string CommaSeparatedGroupNamesFrom(List<string> groupids, string separator)
        {
            if (groupids == null || groupids.Count == 0)
                return string.Empty;

            //List<UserProfile> userCollection = GetUsersProfileWithGroup().Where(x => groupids.Contains(x.Id) && !string.IsNullOrEmpty(x.UserName)).ToList();
            //if (userCollection != null && userCollection.Count > 0)
            //    return string.Join(separator, userCollection.Select(x => x.Name).ToArray()).Replace(",", ", ");

            List<Role> groupCollection = GetUserGroups().Where(x => groupids.Contains(x.Id)).ToList();
            if (groupCollection != null && groupCollection.Count > 0)
                return string.Join(separator, groupCollection.Select(x => x.Title).ToArray()).Replace(",", ", ");

            return string.Empty;
        }

        public string CommaSeparatedIdsFrom(string searchText, string separator)
        {
            if (string.IsNullOrEmpty(searchText))
                return string.Empty;

            string names = string.Empty;

            if (!searchText.Contains("bcci")) //Added this condition since, bcci word exists for all users in UserName column & Search function is craching.
            {
                List<UserProfile> userCollection = GetUsersProfileWithGroup().Where(x => (x.Id.ToLower().Contains(searchText.ToLower()) || x.UserName.ToLower().Contains(searchText.ToLower()) || x.Name.ToLower().Contains(searchText.ToLower())) && !string.IsNullOrEmpty(x.UserName)).ToList();
                if (userCollection != null && userCollection.Count > 0)
                    names = string.Join(separator, userCollection.Select(x => "'" + x.Id + "'").ToArray()).Replace(",", ", ").Replace("#", "");
            }

            List<Role> groupCollection = GetUserGroups().Where(x => (x.Id.ToLower().Contains(searchText.ToLower()) || x.Title.ToLower().Contains(searchText.ToLower()) || x.Name.ToLower().Contains(searchText.ToLower()))).ToList();
            if (groupCollection != null && groupCollection.Count > 0)
            {
                if (!string.IsNullOrEmpty(names))
                    names = $"{names}, ";

                names += string.Join(separator, groupCollection.Select(x => "'" + x.Id + "'").ToArray()).Replace(",", ", ").Replace("#", "");

                return names;
            }

            if (!string.IsNullOrEmpty(names))
                return names;
            else
                return "''";
        }

        public List<UserProfile> GetUsersProfile()
        {
            string cacheName = "UserProfile_" + context.TenantID;
            List<UserProfile> profiles = new List<UserProfile>();
            profiles = (List<UserProfile>)CacheHelper<object>.Get(cacheName, context.TenantID);
            if (profiles == null || profiles.Count==1)
            {
                UserProfileManager umanager = new UserProfileManager(context);
                UserStore<UserProfile> stores = (UserStore<UserProfile>)this.Store;
                profiles = stores.Load(x => !x.isRole && x.TenantID == context.TenantID && x.Name != "Super Admin");

                profiles.ForEach(x => {
                    if (string.IsNullOrEmpty(UGITUtility.ObjectToString(x.Picture)))
                    {
                        x.Picture = "/Content/Images/RMONE/blankImg.png";
                    }
                });
                profiles = (List<UserProfile>)CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, profiles);
                return profiles;
            }
            profiles = profiles.Where(x => !x.isRole && x.TenantID == context.TenantID ).ToList();
            return profiles;
        }

        public List<UserProfile> GetEnabledUsers()
        {
            UserProfileManager umanager = new UserProfileManager(context);
            UserStore<UserProfile> stores = (UserStore<UserProfile>)this.Store;
            return stores.Load(x => !x.isRole && x.TenantID == context.TenantID && x.Enabled);
        }

        public bool CheckUserIsGroup(string id)
        {
            UserProfileManager umanager = new UserProfileManager(context);
            List<UserProfile> userCollection = umanager.LoadWithGroup(x => x.isRole && x.Id == id).ToList();
            if (userCollection != null)
                return userCollection.Count > 0;
            else
                return false;
        }

        public List<UserProfile> GetUsersProfileWithGroup()
        {
            return GetUsersProfileWithGroup(null);
        }

        public List<UserProfile> GetUsersProfileWithGroup(string tenantId)
        {
            //var userManager = new UserProfileManager(context);
            //var userCollection = !string.IsNullOrEmpty(tenantId) ? userManager.Users.Where(x => x.TenantID == tenantId).ToList() : userManager.Users.ToList();
            //var userCollection = !string.IsNullOrEmpty(tenantId) ? userManager.Users.Where(x => x.TenantID.Equals(tenantId, StringComparison.CurrentCultureIgnoreCase)).ToList() : userManager.Users.ToList();

            var userCollection = LoadWithGroup();
            return userCollection;
        }

        public List<UserProfile> GetUsersProfileWithGroup(string tenantId, string id)
        {
            string[] ids = id.Split(',');
            //var userManager = new UserProfileManager(context);

            ////var userCollection = !string.IsNullOrEmpty(tenantId) ? userManager.Users.Where(x => x.TenantID == tenantId).ToList() : userManager.Users.ToList();
            //var userCollection = !string.IsNullOrEmpty(tenantId) ? userManager.Users.Where(x => ids.Contains(x.Id) && x.TenantID.Equals(tenantId, StringComparison.CurrentCultureIgnoreCase)).ToList() : userManager.Users.ToList();

            var userCollection = !string.IsNullOrEmpty(tenantId)
                ? LoadWithGroup(x => ids.Contains(x.Id))
                : LoadWithGroup();

            return userCollection;
        }

        public List<Role> GetUserGroups()
        {
            roleCollection = new List<Role>();
            try
            {
                uRole = new UserRoleManager(context);
                string cacheName = "Lookup_" + DatabaseObjects.Tables.AspNetRoles + "_" + context.TenantID;
                DataTable dt = CacheHelper<object>.Get(cacheName, context.TenantID) as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    roleCollection = uRole.GetRoleList();
                    CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, UGITUtility.ToDataTable<Role>(roleCollection));
                }
                else
                {
                    roleCollection = UGITUtility.ConvertDataTable<Role>(dt);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException($"An Exception Occurred in GetUserGroups: " + ex);
            }
            return roleCollection;
        }

        public DataTable GetCustomizeTable(List<UserProfile> users, string sourceUrl)
        {
            DataTable data = UGITUtility.ToDataTable<UserProfile>(users);
            data.Columns[DatabaseObjects.Columns.BudgetCategory].ColumnName = "BudgetCategoryAlternate";
            data.Columns[DatabaseObjects.Columns.FunctionalArea].ColumnName = "FunctionalAreaAlternate";
            data.Columns[DatabaseObjects.Columns.ManagerID].ColumnName = "ManagerAlternate";
            data.Columns[DatabaseObjects.Columns.Skills].ColumnName = DatabaseObjects.Columns.UserSkill;
            data.Columns[DatabaseObjects.Columns.IsIT].ColumnName = "IT";
            data.Columns.Add(DatabaseObjects.Columns.ManagerLink);
            data.Columns.Add(DatabaseObjects.Columns.TitleLink);
            data.Columns.Add(DatabaseObjects.Columns.Skills);
            data.Columns.Add(DatabaseObjects.Columns.BudgetCategory);
            data.Columns.Add(DatabaseObjects.Columns.FunctionalArea);
            data.Columns.Add(DatabaseObjects.Columns.Manager);
            data.Columns.Add(DatabaseObjects.Columns.IsIT);
            foreach (DataRow row in data.Rows)
            {


                if (!string.IsNullOrEmpty(row["ManagerAlternate"].ToString()))
                {

                    var managervalue = GetUserById(row["ManagerAlternate"].ToString());
                    string managerLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&UpdateUser=1", managervalue.Id));
                    row[DatabaseObjects.Columns.ManagerLink] = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false, \"{2}\")'>{3}</a>",
                                                managerLinkUrl, Convert.ToString(managervalue.Name).Replace("'", string.Empty), Uri.EscapeDataString(sourceUrl), Convert.ToString(managervalue.Name));
                    row[DatabaseObjects.Columns.Manager] = managervalue.Name;
                }

                string userLinkUrl = UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?uID={0}&UpdateUser=1", row[DatabaseObjects.Columns.Id]));
                row[DatabaseObjects.Columns.TitleLink] = string.Format("<a href='javascript:window.parent.UgitOpenPopupDialog(\"{0}\",\"\", \"User Details: {1}\", \"600px\",\"90\", false,\"{2}\")'>{3}</a>",
                                                userLinkUrl, Convert.ToString(row[DatabaseObjects.Columns.Name]).Replace("'", string.Empty), Uri.EscapeDataString(sourceUrl), Convert.ToString(row[DatabaseObjects.Columns.Name]));

                if (row["FunctionalAreaAlternate"] != DBNull.Value)
                {
                    //var innerValue = (LookupValue)row["FunctionalAreaAlternate"];
                    //row[DatabaseObjects.Columns.FunctionalArea] = innerValue.Value;
                }

                if (!string.IsNullOrEmpty(row["Location"].ToString()))
                {
                    DataTable managervalue = GetTableDataManager.GetTableData("Location", "id=" + row["Location"].ToString());
                    row[DatabaseObjects.Columns.LocationName] = managervalue.Rows.Count > 0 ? managervalue.Rows[0]["Title"].ToString() : "";
                }
                if (!string.IsNullOrEmpty(row["Department"].ToString()))
                {
                    string[] val = row["Department"].ToString().Split(new string[] { ";#" }, StringSplitOptions.None);
                    FieldLookupValue lookupVals = new FieldLookupValue(Convert.ToInt32(val[0]), "Title", "Department");
                    row["Department"] = lookupVals.Value;

                }
                if (row["BudgetCategoryAlternate"] != DBNull.Value)
                {
                    var budgetCategory = row["BudgetCategoryAlternate"];
                    row[DatabaseObjects.Columns.BudgetCategory] = budgetCategory;
                }

                if (row[DatabaseObjects.Columns.UserName] != DBNull.Value)
                {
                    row[DatabaseObjects.Columns.UserName] = row[DatabaseObjects.Columns.UserName].ToString();
                }

                row[DatabaseObjects.Columns.IsIT] = UGITUtility.StringToBoolean(row["IT"]) ? "Yes" : "No";

            }
            return data;
        }

        public bool IsRole(string userID)
        {
            if (userID != null)
            {
                return GetUsersProfileWithGroup().SingleOrDefault(x => x.Id == userID) != null;

            }
            else { return false; }
        }

        public bool IsRole(RoleType roleType, string username)
        {
            var returnValue = false;

            if (username != null)
            {
                var user = this.GetUserByUserName(username);

                if (user == null)
                    return returnValue;

                var userRole = new UserRoleManager(context);

                var roles = userRole.GetRolesByType(roleType);
                if (roles == null)
                    return returnValue;

                var userProfileManager = new UserProfileManager(context);
                var userRoles = userProfileManager.GetRoles(user.Id).ToList();

                returnValue = userRoles.Exists(x => roles.Exists(y => y.Name == x));
            }

            return returnValue;
        }

        public bool IsRole(string roleName, string username)
        {
            if (username != null)
            {
                UserProfile u = this.Load(x => x.UserName == username, take: 1).FirstOrDefault();
                if (u == null)
                    return false;

                UserRoleManager uRole = new UserRoleManager(context);

                List<Role> roles = uRole.GetRolesByName(roleName);

                UserProfileManager pManager = new UserProfileManager(context);
                List<string> userRoles = pManager.GetRoles(u.Id).ToList();
                return userRoles.Exists(x => roles.Exists(y => y.Name == x));

            }
            else { return false; }
        }

        /// <summary>
        /// Check if user exists in one of the comma seperated UserGroups.
        /// </summary>        
        public bool IsUserinGroups(string groupNames, string username, string commaSeperator = ",")
        {
            if (username != null)
            {
                UserProfile u = this.Load(x => x.UserName == username, take: 1).FirstOrDefault();
                if (u == null)
                    return false;

                UserRoleManager uRole = new UserRoleManager(context);

                List<Role> roles = new List<Role>();

                foreach (var item in groupNames.Split(new string[] { commaSeperator }, StringSplitOptions.RemoveEmptyEntries))
                {
                    roles.AddRange(uRole.GetRolesByName(item));
                }

                UserProfileManager pManager = new UserProfileManager(context);
                List<string> userRoles = pManager.GetRoles(u.Id).ToList();
                return userRoles.Exists(x => roles.Exists(y => y.Name == x));

            }
            else { return false; }
        }

        /// <summary>
        /// Check if user exists in one of the comma seperated UserGroupsId, UserId.
        /// </summary>        
        public bool IsUserinGroups(string userId, List<string> groupId)
        {
            if (userId != null)
            {
                UserProfile u = this.Load(x => x.Id.EqualsIgnoreCase(userId), take: 1).FirstOrDefault();
                if (u == null)
                    return false;

                UserRoleManager uRole = new UserRoleManager(context);

                List<Role> roles = new List<Role>();

                foreach (var item in groupId)
                {
                    roles.AddRange(uRole.GetRoleList().Where(x => x.Id.EqualsIgnoreCase(item)));
                }

                UserProfileManager pManager = new UserProfileManager(context);
                List<string> userRoles = pManager.GetRoles(u.Id).ToList();
                return userRoles.Exists(x => roles.Exists(y => y.Name == x));

            }
            else { return false; }
        }

        public List<UserProfile> GetFilteredUsers(string roleId, bool? isIT, bool? isConsultant, bool? isManager)
        {
            List<UserProfile> uProfiles = null;
            try
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    var uUsers = Task.Run(async () => { return await (this.Store as UserStore<UserProfile>).GetUsersByRole(roleId); });
                    Task.WaitAll(uUsers);
                    uProfiles = uUsers.Result;
                    uProfiles = uProfiles.Where(x => (isIT == null || x.IsIT == isIT) && (isConsultant == null || x.IsConsultant == isConsultant) && (isManager == null || x.IsManager == isManager)).ToList();
                }
                else
                {
                    uProfiles = this.Load(x => (isIT == null || x.IsIT == isIT) && (isConsultant == null || x.IsConsultant == isConsultant) && (isManager == null || x.IsManager == isManager)).ToList();
                }
                return uProfiles;

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return uProfiles = new List<UserProfile>();
            }
        }
        public List<UserRole> GetUserRoleListByRole(string roleId)
        {
            List<UserRole> userRoles = null;
            try
            {
                var uUsers = Task.Run(async () => { return await (this.Store as UserStore<UserProfile>).GetUserRolesByRole(roleId); });
                Task.WaitAll(uUsers);
                userRoles = uUsers.Result;
                return userRoles;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return userRoles = new List<UserRole>();
            }
        }

        public bool CheckIfUserGroup(string userID, string groupId)
        {
            bool status = false;
            try
            {
                UserProfile spUser = GetUserById(userID);
                if (spUser != null)
                {
                    UserProfileManager pManager = new UserProfileManager(context);
                    UserRoleManager roleManager = new UserRoleManager(context);
                    Role role = roleManager.Get(x => x.Id == groupId);
                    if (role == null)
                        return false;

                    status = pManager.IsInRole(userID, groupId);
                }

            }
            catch
            {
                return status;
            }
            return status;


        }

        public bool CheckUserInGroup(string userID, string groupId)
        {
            bool status = false;
            try
            {
                UserProfile spUser = GetUserById(userID);
                if (spUser != null)
                {
                    UserProfileManager pManager = new UserProfileManager(context);
                    UserRoleManager roleManager = new UserRoleManager(context);
                    Role role = roleManager.Get(x => x.Id == groupId);
                    if (role == null)
                        return false;

                    //status = pManager.IsInRole(userID, groupId);
                    status = pManager.IsInRole(userID, role.Name);
                }

            }
            catch
            {
                return status;
            }
            return status;


        }

        public bool IsUserExistInList(UserProfile user, List<string> usersAndGroupsName)
        {
            if (usersAndGroupsName == null || usersAndGroupsName.Count == 0)
                return false;

            bool exist = false;

            foreach (string ud in usersAndGroupsName)
            {
                if (user.Id == ud)
                    exist = true;

            }

            if (usersAndGroupsName.FirstOrDefault(x => x.ToLower() == user.UserName.ToLower()) != null)
            {
                exist = true;
            }
            else
            {

            }
            return exist;
        }

        public bool IsUserAuthorizedToViewModule(UserProfile user, string moduleName)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule ugitModule = moduleViewManager.LoadByName(moduleName);
            DataRow moduleRow = UGITUtility.ObjectToData(ugitModule).Rows[0];
            return IsUserAuthorizedToViewModule(user, moduleRow);
        }

        public bool IsUserAuthorizedToViewModule(UserProfile user, DataRow moduleRow)
        {
            if (moduleRow == null)
                return false; // Should never happen!

            // If Super-Admin, then always authorized!
            if (IsUGITSuperAdmin(user))
                return true;

            // If module is disabled, then no one is authorized!
            if (!UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.EnableModule]))
                return false;

            string authorizedToView = Convert.ToString(moduleRow[DatabaseObjects.Columns.AuthorizedToView]);
            if (string.IsNullOrEmpty(authorizedToView))
                return true;    // If column is empty, that means EVERYONE is authorized!

            try
            {
                string[] users = UGITUtility.SplitString(authorizedToView, Utility.Constants.Separator6);
                if (users.Contains(user.Id))
                {
                    return true;
                }
                else
                {
                    // In case column value had group names
                    foreach (string groupName in users)
                    {
                        if (CheckUserIsInGroup(groupName, user))
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return false;
        }

        public bool IsUserAuthorizedToViewModule(UserProfile user, UGITModule module)
        {
            if (user == null || module == null)
                return false; // Should never happen!!

            List<string> authorizeToViewList = UGITUtility.ConvertStringToList(module.AuthorizedToView, Utility.Constants.Separator6);

            if (module.AuthorizedToView == null || authorizeToViewList.Count == 0)
                return true;

            if (authorizeToViewList.Any(x => x == user.Id))
            {
                return true;
            }
            //else
            //{
            //    IEnumerable<string> groupIDs = authorizeToViewList.Where(x => x.IsGroup).Select(x => x.ID);
            //    foreach (int groupID in groupIDs)
            //    {
            //        if (UserProfile.CheckUserIsInGroup(groupID, user))
            //            return true;
            //    }
            //}
            return false;
        }


        public List<UserProfile> uesrListByMultipleID(string userIDs, string separator)
        {
            string values = string.Empty;
            if (separator == null)
                separator = Utility.Constants.Separator;
            if (string.IsNullOrWhiteSpace(userIDs))
                return new List<UserProfile>();

            List<string> LookUps = UGITUtility.SplitString(userIDs, separator).ToList();
            List<UserProfile> allUsers = new List<UserProfile>();
            if (LookUps != null && LookUps.Count > 0)
            {
                foreach (string uid in LookUps)
                {
                    UserProfile u = GetUserById(uid);
                    if (u != null) { allUsers.Add(u); }

                }

            }
            return allUsers;
        }

        public string ConcatenateValues(List<UserProfile> lookups, string separator)
        {

            string values = string.Empty;
            if (separator == null)
                separator = Utility.Constants.Separator;
            if (lookups != null && lookups.Count > 0)
            {
                values = string.Join(separator, lookups.Select(x => x.Name).ToArray());
            }
            return values;
        }

        public string GetValue(UserProfile user)
        {
            if (user != null)
                return user.Name;
            return string.Empty;
        }

        public UserProfile LoadById(string userId)
        {
            UserProfile userProfile = new UserProfile();
            if (!string.IsNullOrEmpty(userId))
            {
                userProfile = this.Load(x => x.Id == userId).FirstOrDefault();
            }
            return userProfile;
        }

        public List<UserProfile> GetUserByManager(string managerId, bool includeDisabled = false)
        {
            List<UserProfile> userProfile = new List<UserProfile>();
            if (!string.IsNullOrEmpty(managerId))
            {
                if(includeDisabled)
                    userProfile = this.Load(x => x.ManagerID == managerId);
                else
                    userProfile = this.Load(x => x.ManagerID == managerId && x.Enabled == true);
            }
            return userProfile;
        }

        public UserProfile GetUserInfo(DataRow item, string[] fieldNames, bool needTooltip)
        {
            UserProfile usersInfo = GetUserById(Convert.ToString(item[DatabaseObjects.Columns.TicketStageActionUsers]));

            //AddUsersFromItemField(ref usersInfo, item, fieldNames, needTooltip, true);
            return usersInfo;
        }

        public List<UsersEmail> GetUsersEmail(DataRow item, string[] fieldNames, bool needTooltip)
        {
            List<UsersEmail> usersInfo = new List<UsersEmail>();
            AddUsersFromItemField(ref usersInfo, item, fieldNames, needTooltip, true);
            return usersInfo;
        }

        public UserProfile GetRequestorUserInfo(DataRow item, string[] values)
        {
            UserProfile usersInfo = GetUserById(Convert.ToString(item[DatabaseObjects.Columns.TicketRequestor]));

            return usersInfo;
        }
        public UserProfile GetUserInfo(DataRow item, string[] values)
        {
            UserProfile usersInfo = GetUserById(Convert.ToString(item[DatabaseObjects.Columns.TicketStageActionUsers]));

            return usersInfo;
        }
        public void AddUsersFromItemField(ref List<UsersEmail> usersInfo, DataRow item, string[] fields, bool needTooltip, bool checkDisableWorkflowNotifications)
        {
            try
            {
                string Email = string.Empty;
                if (UGITUtility.IsSPItemExist(item, fields))
                {
                    foreach (string fieldName in fields)
                    {
                        if (item.Table.Columns.Contains(fieldName))
                        {
                            // If we find the field in the ticket, get user info from field value
                            string[] usersColumn = Convert.ToString(item[fieldName]).Split(',');
                            if (usersColumn.Count() > 0)
                            {
                                List<UserProfile> userList = GetUserInfosById(string.Join(",", usersColumn));
                                foreach (UserProfile user in userList)
                                {
                                    UsersEmail ue = new UsersEmail();
                                    ue.ID = user.Id;
                                    ue.Email = user.Email;
                                    ue.NotificationEmail = user.NotificationEmail;
                                    ue.UserName = user.Name;
                                    usersInfo.Add(ue);
                                }
                                AddUsersFromFieldUserValue(ref usersInfo, true, checkDisableWorkflowNotifications);
                            }
                            else
                            {
                                List<UserProfile> users = GetUserInfosById(Convert.ToString(item[fieldName]));
                                if (users != null)
                                {
                                    foreach (UserProfile user in users)
                                    {
                                        UserProfile uProfile = user;
                                        UsersEmail ue = new UsersEmail();
                                        ue.ID = uProfile.Id;
                                        ue.Email = uProfile.Email;
                                        ue.NotificationEmail = uProfile.NotificationEmail;
                                        ue.UserName = uProfile.Name;
                                        usersInfo.Add(ue);
                                    }
                                    AddUsersFromFieldUserValue(ref usersInfo, needTooltip, checkDisableWorkflowNotifications);
                                }
                            }
                        }
                        else
                        {
                            // If fieldname is not a field, check if its a group name                            
                            string groupName = fieldName;
                            UserProfile user = null;
                            List<UserProfile> lstGroupUsers = GetUserProfilesByGroupName(groupName);
                            foreach (UserProfile userProfileItem in lstGroupUsers)
                            {
                                user = GetUserById(userProfileItem.Id);

                                if (user != null && (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.NotificationEmail)))
                                {
                                    UsersEmail ue = new UsersEmail();
                                    ue.ID = user.Id;
                                    ue.Email = user.Email;
                                    ue.NotificationEmail = user.NotificationEmail;
                                    ue.UserName = user.Name;
                                    usersInfo.Add(ue);

                                    AddUsersFromFieldUserValue(ref usersInfo, true, checkDisableWorkflowNotifications);
                                }
                            }


                        }
                    }
                }
                else
                {
                    // If fieldname is not a field, check if its a group name
                    foreach (string fieldName in fields)
                    {
                        string groupName = fieldName;
                        UserProfile user = null;
                        List<UserProfile> lstGroupUsers = GetUserProfilesByGroupName(groupName);
                        if (lstGroupUsers == null || lstGroupUsers.Count == 0)
                            continue;

                        foreach (UserProfile userProfileItem in lstGroupUsers)
                        {
                            user = GetUserById(userProfileItem.Id);

                            if (user != null && (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.NotificationEmail)))
                            {
                                UsersEmail ue = new UsersEmail();
                                ue.ID = user.Id;
                                ue.Email = user.Email;
                                ue.NotificationEmail = user.NotificationEmail;
                                ue.UserName = user.Name;
                                usersInfo.Add(ue);

                                AddUsersFromFieldUserValue(ref usersInfo, true, checkDisableWorkflowNotifications);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        //if checkDisableWorkflowNotifications true then email notification not sent those user who have DisableWorkflowNotifications column value true.  
        public void AddUsersFromFieldUserValue(ref List<UsersEmail> usersInfo, bool needTooltip, bool checkDisableWorkflowNotifications)
        {
            if (usersInfo == null)
            {
                return; // Nothing can do!
            }
            foreach (UsersEmail uem in usersInfo)
            {
                if (!string.IsNullOrWhiteSpace(uem.ID) && !IsRole(uem.ID))
                {

                    // This is a valid single user, get most current info from SPUser object (user.User)
                    //UGITUtility.AppendWithSeparator(ref userinfo, usersInfo.Id, Utility.Constants.Separator6);
                    // usersInfo.userIds.Add(user.User.ID.ToString());

                    string Email = string.Empty;
                    UserProfile userProfile = GetUserById(uem.ID);
                    //get delegate user profile 

                    UserProfile delegateUserProfile = null;
                    string delegateProfileEmail = string.Empty;

                    if (userProfile != null)
                    {
                        if (Convert.ToBoolean(userProfile.EnableOutofOffice) && userProfile.DelegateUserOnLeave != null && userProfile.LeaveFromDate <= DateTime.Now.Date && userProfile.LeaveToDate >= DateTime.Now.Date)
                            delegateUserProfile = GetUserById(userProfile.DelegateUserOnLeave);

                        //if checkDisableWorkflowNotifications true then email notification not sent those user who have DisableWorkflowNotifications column value true.  
                        if (!checkDisableWorkflowNotifications || userProfile.DisableWorkflowNotifications == false)
                        {
                            Email = !string.IsNullOrEmpty(userProfile.NotificationEmail) ? userProfile.NotificationEmail : userProfile.Email;
                            string ee = uem.Email;
                            UGITUtility.AppendWithSeparator(ref ee, Email, Utility.Constants.UserInfoSeparator);
                            if (!string.IsNullOrEmpty(ee))
                            {
                                uem.Email = ee;
                            }
                        }

                        if (needTooltip)
                        {
                            List<string> info = new List<string>();

                            // Add comma AND newline after each line since newlines only work in IE
                            if (!string.IsNullOrEmpty(userProfile.Name))
                            {
                                if (!string.IsNullOrEmpty(userProfile.JobProfile))
                                    info.Add(string.Format("<b>{0}</b>, {1}", userProfile.Name, userProfile.JobProfile));
                                else
                                    info.Add(string.Format("<b>{0}</b>", userProfile.Name));
                            }
                            info.Add(string.Format("<b>Login:</b> {0}", userProfile.UserName));
                            if (!string.IsNullOrEmpty(userProfile.Department))
                                info.Add(string.Format("<b>Department:</b> {0}", userProfile.Department));
                            if (!string.IsNullOrEmpty(userProfile.Location))
                                info.Add(string.Format("<b>Location:</b> {0}", userProfile.Location));
                            if (!string.IsNullOrEmpty(userProfile.MobilePhone))
                                info.Add(string.Format("<b>Phone:</b> {0}", userProfile.MobilePhone));
                            if (userProfile.Manager1 != null)
                                info.Add(string.Format("<b>Manager:</b> {0}", userProfile.Manager1));

                            if (string.IsNullOrEmpty(uem.userToolTip))
                                uem.userToolTip = string.Join("\n", info.ToArray());
                            else
                                uem.userToolTip = string.Format("{0}\n\n{1}", uem.userToolTip, string.Join("\n", info.ToArray()));
                        }
                    }

                    if (delegateUserProfile != null)
                    {
                        //add delegate users id
                        string uusernames = uem.UserName;

                        UGITUtility.AppendWithSeparator(ref uusernames, delegateUserProfile.Name, Utility.Constants.UserInfoSeparator);
                        string uuid = uem.ID;
                        uem.ID = uuid + "," + delegateUserProfile.Id.ToString();
                        uem.UserName = uusernames;

                        //add delegate users Email & name
                        delegateProfileEmail = !string.IsNullOrEmpty(delegateUserProfile.NotificationEmail) ? delegateUserProfile.NotificationEmail : delegateUserProfile.Email;
                        string udemail = uem.Email;
                        UGITUtility.AppendWithSeparator(ref udemail, delegateProfileEmail, Utility.Constants.UserInfoSeparator);
                        uem.Email = udemail;
                        if (needTooltip)
                        {
                            List<string> info = new List<string>();

                            // Add comma AND newline after each line since newlines only work in IE
                            if (!string.IsNullOrEmpty(delegateUserProfile.Name))
                            {
                                if (!string.IsNullOrEmpty(delegateUserProfile.JobProfile))
                                    info.Add(string.Format("<b>{0}</b>, {1}", delegateUserProfile.Name, delegateUserProfile.JobProfile));
                                else
                                    info.Add(string.Format("<b>{0}</b>", delegateUserProfile.Name));
                            }
                            info.Add(string.Format("<b>Login:</b> {0}", delegateUserProfile.UserName));
                            if (!string.IsNullOrEmpty(delegateUserProfile.Department))
                                info.Add(string.Format("<b>Department:</b> {0}", delegateUserProfile.Department));
                            if (!string.IsNullOrEmpty(delegateUserProfile.Location))
                                info.Add(string.Format("<b>Location:</b> {0}", delegateUserProfile.Location));
                            if (!string.IsNullOrEmpty(userProfile.MobilePhone))
                                info.Add(string.Format("<b>Phone:</b> {0}", delegateUserProfile.MobilePhone));
                            if (delegateUserProfile.ManagerID != null)
                                info.Add(string.Format("<b>Manager:</b> {0}", GetUserById(delegateUserProfile.Id).Name));

                            if (string.IsNullOrEmpty(uem.userToolTip))
                                uem.userToolTip = string.Join("\n", info.ToArray());
                            else
                                uem.userToolTip = string.Format("{0}\n\n{1}", uem.userToolTip, string.Join("\n", info.ToArray()));
                        }
                    }
                }
                else
                {
                    // Not a current valid user, so check if its a group
                    List<UserProfile> userList = GetUsersByGroupID(uem.ID);

                    //SPGroup group = GetGroupByID(user.LookupId);
                    //if (userList == null)
                    //{
                    //    if (!usersInfo.userIds.Contains(user.LookupId.ToString()))
                    //    {
                    //        // Was valid user value, but user no longer valid (deleted, de-activated, etc)
                    //        // So just user the info that was in the object
                    //        uHelper.AppendWithSeparator(ref usersInfo.userNames, user.LookupValue, Constants.UserInfoSeparator);
                    //        usersInfo.userIds.Add(user.LookupId.ToString());
                    //        //uHelper.AppendWithSeparator(ref usersInfo.userIds, user.LookupId.ToString(), Constants.UserInfoSeparator);
                    //    }
                    //}
                    //else if (!usersInfo.userNames.Contains(group.Name))
                    if (userList != null && userList.Count > 0)
                    {
                        // This is a valid single group name, add it to the list if not already there
                        //uHelper.AppendWithSeparator(ref usersInfo.userNames, group.Name, Constants.UserInfoSeparator);
                        foreach (UserProfile u in userList)
                        {
                            if (!uem.ID.Contains(u.Id.ToString()))
                            {
                                UserProfile userP = u;
                                string grpMemberEmail = string.Empty;

                                //if user contain delegate user get that profile for Email and Notification Email iteration
                                UserProfile delegateUserProfile = null;
                                if (userP.DelegateUserOnLeave != null)
                                    delegateUserProfile = LoadById(userP.DelegateUserOnLeave);

                                string grpDelegateMemberEmail = string.Empty;

                                if (userP != null)
                                {
                                    grpMemberEmail = !string.IsNullOrEmpty(userP.NotificationEmail) ? userP.NotificationEmail : userP.Email;
                                }


                                if (delegateUserProfile != null)
                                {
                                    if (delegateUserProfile.Email != null)
                                        grpDelegateMemberEmail = !string.IsNullOrEmpty(delegateUserProfile.NotificationEmail) ? delegateUserProfile.NotificationEmail : delegateUserProfile.Email;
                                }

                                //if checkDisableWorkflowNotifications true then email notification not sent those user who are present in group but DisableWorkflowNotifications column value true.  
                                if (checkDisableWorkflowNotifications)
                                {

                                    if (userP != null && !userP.DisableWorkflowNotifications)
                                    {
                                        string uuMail = uem.Email;
                                        UGITUtility.AppendWithSeparator(ref uuMail, grpMemberEmail, Utility.Constants.UserInfoSeparator);
                                        uem.Email = uuMail;
                                        string uuid = uem.ID;
                                        uem.ID = uuid + "," + userP.Id;
                                    }

                                    //add delegate  email notification for delegate user
                                    if (delegateUserProfile != null)
                                    {
                                        string uEm = uem.Email;

                                        UGITUtility.AppendWithSeparator(ref uEm, grpDelegateMemberEmail, Utility.Constants.UserInfoSeparator);
                                        uem.Email = uEm;
                                        uem.ID = uem.ID + "," + delegateUserProfile.Id.ToString();
                                        //usersInfo.userIds.Add();
                                    }
                                }
                                else
                                {
                                    string usermail = uem.Email;
                                    if (u != null)
                                    {

                                        UGITUtility.AppendWithSeparator(ref usermail, grpMemberEmail, Utility.Constants.UserInfoSeparator);
                                        uem.ID = uem.ID + "," + u.Id.ToString();
                                        //uHelper.AppendWithSeparator(ref usersInfo.userIds, groupMember.ID.ToString(), Constants.UserInfoSeparator);
                                    }

                                    if (delegateUserProfile != null)
                                    {
                                        usermail = uem.Email;
                                        UGITUtility.AppendWithSeparator(ref usermail, grpDelegateMemberEmail, Utility.Constants.UserInfoSeparator);
                                        uem.ID = uem.ID + "," + delegateUserProfile.Id;
                                        //usersInfo.userIds.Add(delegateUserProfile.ID.ToString());
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        public bool IsActionUser(DataRow currentTicket, UserProfile user)
        {
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes))
            {
                string[] currentStageActionUserTypes = UGITUtility.SplitString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes], Utility.Constants.Separator);
                foreach (string currentStageActionUserType1 in currentStageActionUserTypes)
                {
                    if (IsUserPresentInField(user, currentTicket, currentStageActionUserType1, true))
                        return true;
                }
            }

            return false;
        }

        //public static bool IsDataEditor(DataRow currentTicket)
        //{
        //    return IsDataEditor(currentTicket, SPContext.Current.Web.CurrentUser);
        //}

        public bool IsDataEditor(DataRow currentTicket, UserProfile user)
        {
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.DataEditors))
            {
                string[] dataEditors = UGITUtility.SplitString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.DataEditors), Utility.Constants.Separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string dataEditor in dataEditors)
                {
                    if (IsUserPresentInField(user, currentTicket, dataEditor, true))
                        return true;
                }
            }

            return false;
        }

        public List<UserProfile> GetUsersByGroupID(string groupID)
        {
            var uUsers = Task.Run(async () => { return await (this.Store as UserStore<UserProfile>).GetUsersByRole(groupID); });
            Task.WaitAll(uUsers);
            List<UserProfile> userList = uUsers.Result;
            return userList;
        }

        public List<UserProfile> FillITManagers()
        {
            List<UserProfile> profiles = this.Load(x => x.IsIT && x.IsManager && x.Enabled).ToList(); //uGITCache.UserProfileCache.GetEnabledUsers(SPContext.Current.Web);
            return profiles;
        }

        public List<Role> GetUserRoles(string userID)
        {
            try
            {
                var uUsers = Task.Run(async () => { return await (this.Store as UserStore<UserProfile>).GetRolesAsync(userID); });
                Task.WaitAll(uUsers);
                List<Role> list = (uUsers.Result).ToList();
                return list;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
                return null;
            }
        }

        /// <summary>
        /// This will format the new user name which according in configuration
        /// </summary>
        /// <param name="newUserName"></param>
        /// <returns></returns>
        public string FormatNewUserName(string newUserName)
        {
            if (string.IsNullOrWhiteSpace(newUserName))
                return string.Empty;

            var userNames = newUserName.Split(new char[] { ' ' });
            string valFirstName = "";
            string valLastName = "";
            char valFirstInitial = 'a';
            char valLastInitial = 'a';

            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            string userNameFormatToken = configManager.GetValue(ConfigConstants.UserAccountCreationFormatToken);
            //Set default format is configuration is set.
            if (string.IsNullOrWhiteSpace(userNameFormatToken))
                userNameFormatToken = uGovernIT.Utility.Constants.NewUserNameFormat;

            MatchCollection matchedTokens = Regex.Matches(userNameFormatToken, "\\[\\$(.+?)\\$\\]", RegexOptions.IgnoreCase);
            if (userNames.Length > 1)
            {
                valFirstName = userNames.First();
                valLastName = userNames.Last();
                valFirstInitial = valFirstName[0];
                valLastInitial = valLastName[0];

                foreach (Match token in matchedTokens)
                {
                    // This should not happen unless there is a mapping error, but check to prevent crashes
                    if (string.IsNullOrEmpty(userNameFormatToken))
                        break; // Nothing more to parse
                    string tokenVal = token.ToString().Replace("[$", string.Empty).Replace("$]", string.Empty).ToLower();
                    switch (tokenVal)
                    {
                        case "firstname":
                            userNameFormatToken = userNameFormatToken.Replace(token.ToString(), valFirstName);
                            break;
                        case "lastname":
                            userNameFormatToken = userNameFormatToken.Replace(token.ToString(), valLastName);
                            break;
                        case "firstinitial":
                            userNameFormatToken = userNameFormatToken.Replace(token.ToString(), valFirstInitial.ToString());
                            break;
                        case "lastinitial":
                            userNameFormatToken = userNameFormatToken.Replace(token.ToString(), valLastInitial.ToString());
                            break;
                        case "dot":
                            userNameFormatToken = userNameFormatToken.Replace(token.ToString(), ".");
                            break;
                        case "none":
                            userNameFormatToken = userNameFormatToken.Replace(token.ToString(), "");
                            break;
                        default:
                            userNameFormatToken = userNameFormatToken.Replace(token.ToString(), "");
                            break;
                    }
                }
            }
            else if (userNames.Length == 1)
            {
                userNameFormatToken = userNames[0];
            }

            return userNameFormatToken;
        }

        public List<UserProfile> GetUsersProfileByTenant(string tenantId)
        {
            UserProfileManager umanager = new UserProfileManager(context);
            List<UserProfile> userCollection = umanager.Users.Where(x => !x.isRole && x.TenantID.Equals(tenantId, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return userCollection;
        }

        public long GetUserCountForTenant(string tenantId)
        {
            UserProfileManager umanager = new UserProfileManager(context);
            long userCount = umanager.Users.ToList().Where(x => x.TenantID.EqualsIgnoreCase(tenantId) && x.isRole == false).LongCount();
            return userCount;
        }

        /*
        public UserRoles GetUserRoleById(string userID)
        {
            UserRolesManager uRole = new UserRolesManager(context);

            UserRoles userRoles = uRole.GetUserRoleById(userID);
            return userRoles;
        }
        */

        public LandingPages GetUserRoleById(string userID)
        {
            LandingPagesManager uRole = new LandingPagesManager(context);

            LandingPages userRoles = uRole.GetUserRoleById(userID);
            return userRoles;
        }

        public string GetUserOrGroupName(List<string> userId)
        {
            UserProfile user;
            StringBuilder userName = new StringBuilder();

            foreach (string entity in userId)
            {
                user = GetUserById(entity);

                if (user == null)
                    continue;

                if (user != null && user.isRole == true) // for User Groups
                {
                    List<UserProfile> lstGroupUsers = GetUsersByGroupID(entity);

                    foreach (UserProfile userProfileItem in lstGroupUsers)
                    {
                        user = GetUserById(userProfileItem.Id);

                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            if (userName.Length != 0)
                                userName.Append(";");
                            userName.Append(user.Name);
                        }
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(user.Name))
                    {
                        if (userName.Length != 0)
                            userName.Append(";");
                        userName.Append(user.Name);
                    }
                }
            }
            return Convert.ToString(userName);
        }

        /// <summary>
        /// Get list of all users that the current user is authorized to see RMM allocations and actuals for
        /// due to being their manager, functional area owner, or department owner
        /// </summary>
        /// <param name="users"></param>
        public List<UserProfile> LoadAuthorizedUsers(bool includeCurrentUser)
        {
            List<UserProfile> users = new List<UserProfile>();

            // Get current user and all users reporting to current user directly or indirectly based on manager relationship
            LoadUserWorkingUnder(context.CurrentUser.Id, ref users, includeCurrentUser);

            // Get all users that belong to any functional area(s) user is manager of
            LoadFunctionalAreaUsers(context.CurrentUser.Id, ref users);

            // Get all users that belong to any department(s) user is manager of
            LoadDepartmentUsers(context.CurrentUser.Id, ref users);

            // Sort list by name
            if (users.Count > 0)
                users.Sort((x, y) => x.Name.CompareTo(y.Name));

            return users;
        }

        public List<UserProfile> LoadAuthorizedUsers(string allowAllocationForSelf)
        {
            List<UserProfile> users = new List<UserProfile>();

            if (allowAllocationForSelf.EqualsIgnoreCase("true"))
            {
                // Get current user and all users reporting to current user directly or indirectly based on manager relationship
                LoadUserWorkingUnder(context.CurrentUser.Id, ref users, Convert.ToBoolean(allowAllocationForSelf));

                // Get all users that belong to any functional area(s) user is manager of
                LoadFunctionalAreaUsers(context.CurrentUser.Id, ref users);

                // Get all users that belong to any department(s) user is manager of
                LoadDepartmentUsers(context.CurrentUser.Id, ref users);
            }
            else if(allowAllocationForSelf.EqualsIgnoreCase("Edit"))
            {
                if (users == null)
                    users = new List<UserProfile>();

                if (users.Count <= 0)
                {
                    UserProfile user = LoadById(context.CurrentUser.Id);
                    if (user != null)
                        users.Add(user);
                }
            }
            else
            {
                if (users.Count <= 0)
                {
                    UserProfile user = LoadById(context.CurrentUser.Id);
                    if (user != null)
                        users.Add(user);
                }
            }
            // Sort list by name
            if (users.Count > 0)
                users.Sort((x, y) => x.Name.CompareTo(y.Name));

            return users;
        }

        /// <summary>
        /// Loads all user working under specified user. It loads all level of users recursively. 
        /// Load From Cache
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="users"></param>
        public void LoadUserWorkingUnder(string userId, ref List<UserProfile> users, bool includeCurrentUser = true)
        {
            if (users == null)
                users = new List<UserProfile>();

            if (users.Count <= 0 && includeCurrentUser)
            {
                UserProfile user = LoadById(userId);
                if (user != null)
                    users.Add(user);
            }

            List<UserProfile> myUsers = GetUserByManager(userId); // LoadUserProfilesByManagerId(userId, SPContext.Current.Web);
            foreach (UserProfile myUser in myUsers)
            {
                if (!users.Exists(x => x.Id == myUser.Id))
                {
                    users.Add(myUser);
                    LoadUserWorkingUnder(myUser.Id, ref users);
                }
                else
                    continue;
            }
        }

        /// <summary>
        /// Loads list of users in all functional areas owned by managerId
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="users"></param>
        public void LoadFunctionalAreaUsers(string managerId, ref List<UserProfile> users)
        {
            FunctionalAreasManager functionalAreasMGR = new FunctionalAreasManager(context);

            // Get list of functional areas managed by user
            List<FunctionalArea> functionalAreas = functionalAreasMGR.LoadFunctionalAreas();
            List<FunctionalArea> managerFunctionalAreas = functionalAreas.Where(x => x.Owner != null || x.Owner.Contains(managerId)).ToList();

            // If user not manager of any functional areas, return
            if (managerFunctionalAreas == null || managerFunctionalAreas.Count == 0)
                return;

            // Get list of all users in functional areas
            List<UserProfile> profiles = GetUsersProfile();  // uGITCache.UserProfileCache.GetEnabledUsers(SPContext.Current.Web);
            foreach (FunctionalArea functionalArea in managerFunctionalAreas)
            {
                List<UserProfile> sProfiles = new List<UserProfile>();
                if (profiles != null)
                    sProfiles = profiles.Where(x => x.FunctionalArea != null && x.FunctionalArea == functionalArea.ID).ToList();

                foreach (UserProfile up in sProfiles)
                {
                    UserInfo userInfo = new UserInfo(up.Id, up.Name);
                    if (!users.Exists(x => x.Id == up.Id))
                        users.Add(new UserProfile(userInfo));
                }
            }
        }

        /// <summary>
        /// Loads list of users in all departments managed by managerId
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="users"></param>
        public void LoadDepartmentUsers(string managerId, ref List<UserProfile> users)
        {
            ConfigurationVariableManager configMGR = new ConfigurationVariableManager(context);
            bool enableDivision = configMGR.GetValueAsBool(ConfigConstants.EnableDivision);

            // Get list of departments managed by user
            DepartmentManager departmentManager = new DepartmentManager(context);
            List<Department> lstDepartment = departmentManager.GetDepartmentData(); // uGITCache.LoadDepartments(SPContext.Current.Web);
            List<Department> managerDepartments = lstDepartment.Where(x => x.Manager == managerId).ToList();

            // If user not manager of any departments, return
            if (managerDepartments == null || managerDepartments.Count == 0)
                return;

            // Get list of all users in functional areas
            List<UserProfile> profiles = GetUsersProfile();   // uGITCache.UserProfileCache.GetEnabledUsers(SPContext.Current.Web);
            foreach (Department department in managerDepartments)
            {
                List<UserProfile> sProfiles = new List<UserProfile>();
                if (profiles != null)
                    sProfiles = profiles.Where(x => x.Department == Convert.ToString(department.ID)).ToList();

                foreach (UserProfile up in sProfiles)
                {
                    UserInfo userInfo = new UserInfo(up.Id, up.Name);
                    if (!users.Exists(x => x.Id == up.Id))
                        users.Add(new UserProfile(userInfo));
                }

                // get division (division manager) based on current user and current user department and configuration variable enabledivision. 
                if (enableDivision)
                {
                    if (department.DivisionLookup != null && !string.IsNullOrEmpty(department.DivisionLookup.Value))
                    {
                        if (department.CompanyLookup != null && !string.IsNullOrEmpty(department.CompanyLookup.Value))
                        {
                            CompanyManager companyManager = new CompanyManager(context);
                            List<Company> lstcompany = companyManager.LoadAllHierarchy();   // uGITCache.LoadCompanies(SPContext.Current.Web);
                            Company company = lstcompany.Where(x => x.ID == Convert.ToInt64(department.CompanyLookup.Value)).FirstOrDefault();

                            if (company != null && company.CompanyDivisions != null && company.CompanyDivisions.Count > 0)
                            {
                                CompanyDivision companyDivision = company.CompanyDivisions.Where(x => x.ID == Convert.ToInt64(department.DivisionLookup.ID)).FirstOrDefault();

                                if (companyDivision != null && !string.IsNullOrEmpty(companyDivision.Manager))
                                {
                                    UserInfo userInfo = new UserInfo(companyDivision.Manager, companyDivision.Manager);
                                    if (!users.Exists(x => x.Id == companyDivision.Manager))
                                        users.Add(new UserProfile(userInfo));
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual List<UserProfile> Load(Expression<Func<UserProfile, bool>> where, Expression<Func<UserProfile, object>> order = null, int skip = 0, int take = 0, List<Expression<Func<UserProfile, object>>> includeExpressions = null)
        {
            where = where.And(x => !x.isRole && x.TenantID == context.TenantID);
            return ((UserStore<UserProfile>)this.Store).Load(where, order, skip, take, includeExpressions);
        }

        public virtual List<UserProfile> LoadWithGroup(Expression<Func<UserProfile, bool>> where, Expression<Func<UserProfile, object>> order = null, int skip = 0, int take = 0, List<Expression<Func<UserProfile, object>>> includeExpressions = null)
        {
            where = where.And(x => x.TenantID == context.TenantID);
            return ((UserStore<UserProfile>)this.Store).Load(where, order, skip, take, includeExpressions);
        }

        public virtual List<UserProfile> LoadWithGroup()
        {
            return ((UserStore<UserProfile>)this.Store).Load(x => x.TenantID == context.TenantID);
        }

        public virtual List<UserProfile> LoadWithoutGroup()
        {
            return ((UserStore<UserProfile>)this.Store).Load(x => x.TenantID == context.TenantID && !x.isRole);
        }

        /// <summary>
        /// Returns EmailIds of selected Users and groups by Comma seperated Ids.
        /// </summary>        
        public StringBuilder GetUserEmailId(string SelectedUsers)
        {
            UserProfile user = null;
            StringBuilder userEmailTo = new StringBuilder();
            List<string> userId = UGITUtility.ConvertStringToList(SelectedUsers, ",");
            foreach (string entity in userId)
            {
                user = GetUserById(entity);
                if (user == null)
                    continue;

                if (user != null && user.isRole == true) // for User Groups
                {
                    List<UserProfile> lstGroupUsers = GetUsersByGroupID(entity);

                    foreach (UserProfile userProfileItem in lstGroupUsers)
                    {
                        user = GetUserById(userProfileItem.Id);

                        if (user != null && !string.IsNullOrEmpty(user.Email))
                        {
                            if (userEmailTo.Length != 0)
                                userEmailTo.Append(";");
                            userEmailTo.Append(user.Email);
                        }
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        if (userEmailTo.Length != 0)
                            userEmailTo.Append(";");
                        userEmailTo.Append(user.Email);
                    }
                }
            }
            return userEmailTo;
        }

        public StringBuilder GetUserEmailIdByGroupOrUserName(string SelectedUsers, string separator = ";")
        {
            UserProfile user = null;
            StringBuilder userEmailTo = new StringBuilder();
            List<string> userId = UGITUtility.ConvertStringToList(SelectedUsers, separator);
            foreach (string entity in userId)
            {
                user = GetUserByUserName(entity);
                if (user == null)
                    continue;

                if (user != null && user.isRole == true) // for User Groups
                {

                    List<UserProfile> lstGroupUsers = GetUserProfilesByGroupName(entity);

                    if (lstGroupUsers != null)
                    {
                        foreach (UserProfile userProfileItem in lstGroupUsers)
                        {
                            user = GetUserById(userProfileItem.Id);

                            if (user != null && !string.IsNullOrEmpty(user.Email))
                            {
                                if (userEmailTo.Length != 0)
                                    userEmailTo.Append(";");
                                userEmailTo.Append(user.Email);
                            }
                        }
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        if (userEmailTo.Length != 0)
                            userEmailTo.Append(";");
                        userEmailTo.Append(user.Email);
                    }
                }
            }
            return userEmailTo;
        }



        public Role GetUserRoleByGroupName(string groupName)
        {
            UserRoleManager uRole = new UserRoleManager(context);

            Role userRoles = uRole.GetRoleByName(groupName);
            if (userRoles == null)
                userRoles = uRole.GetRolesByName(groupName).FirstOrDefault();
            return userRoles;
        }

        public List<UserProfile> GetUserProfilesByGroupName(string groupName)
        {

            UserRoleManager uRole = new UserRoleManager(context);
            List<UserProfile> userLst = null;
            List<Role> userlist = uRole.GetRolesByName(groupName);
            if (userlist != null && userlist.Count > 0)
            {
                userLst = GetUsersByGroupID(string.Join(",", userlist.Select(x => x.Id)));
            }
            return userLst;
        }
        public List<UserProfile> GetRolesAsUserProfile(List<Role> roles)
        {
            //UserRoleManager uRole = new UserRoleManager(context);
            List<UserProfile> userLst = null;
            //List<Role> userRolelist = uRole.GetRoleList();
            if (roles != null && roles.Count > 0)
            {
                foreach (Role role in roles)
                    userLst.Add(GetUserProfileFromRole(role));
            }

            if (userLst != null && userLst.Count > 0)
                userLst = userLst.Distinct().ToList();
            return userLst;
        }
        public List<UserProfile> GetUsersByGlobalRoleID(string globalRoleID)
        {
            List<UserProfile> userList = this.Load(x => x.GlobalRoleId == globalRoleID);
            return userList;
        }


        public string GeneratePassword()
        {
            string Passwd = Guid.NewGuid().ToString().Substring(0, 10).ToString();
            for (int i = 0; i < Passwd.Length; i++)
            {
                if (char.IsLetter(Passwd[i]))
                {
                    Passwd = Passwd.Replace(Passwd[i], char.ToUpper(Passwd[i]));
                    break;
                }
            }
            // Regular expression to check Auto Generated password, for alteast 1 small, 1 capital letter, 1 digit, 1 special character.
            String PasswdRegex = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%-]).{6,10})"; ///@"^\d{5}$";
            if (Regex.IsMatch(Passwd, PasswdRegex))
            {
                return Passwd;
            }
            else
            {
                return GeneratePassword();
            }
        }


        public Dictionary<string, string> resetPasswordForDefaultAdmin(string UserId, UserProfileManager umanager)
        {
            Dictionary<string, string> resultOfResetPassword = new Dictionary<string, string>();
            IdentityResult result;
            string newPassword = GeneratePassword();
            string passwordToken = umanager.GeneratePasswordResetToken(UserId);
            result = umanager.ResetPassword(UserId, passwordToken, newPassword);
            if (result.Succeeded)
            {
                resultOfResetPassword.Add("isPasswortReset", "True");
                resultOfResetPassword.Add("NewPassword", newPassword);

            }

            return resultOfResetPassword;
        }





        //Used to avoid ValidateAsync method for other identity method other then create
        public override async Task<IdentityResult> UpdateAsync(UserProfile user)
        {
            //ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //var result = await UserValidator.ValidateAsync(user).WithCurrentCulture();
            //if (!result.Succeeded)
            //{
            //    return result;
            //}
            await Store.UpdateAsync(user);
            return IdentityResult.Success;
        }

        public void RefreshCache()
        {
            UserStore<UserProfile> stores = (UserStore<UserProfile>)this.Store;
            List<UserProfile> lstUserProfile = stores.Load(x => !x.isRole && x.TenantID == context.TenantID);
            string cacheName = "UserProfile_" + context.TenantID;
            if (lstUserProfile == null || lstUserProfile.Count == 0)
                return;
            lstUserProfile.ForEach(x => {
                if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(x.Picture)))
                {
                    x.Picture = "/Content/Images/RMONE/blankImg.png";
                }
            });
            foreach (UserProfile uProfile in lstUserProfile)
            {
                CacheHelper<UserProfile>.AddOrUpdate(uProfile.UserName, context.TenantID, uProfile);
                CacheHelper<UserProfile>.AddOrUpdate(uProfile.Email, context.TenantID, uProfile);
                CacheHelper<UserProfile>.AddOrUpdate(uProfile.Id, context.TenantID, uProfile);
            }
            CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, lstUserProfile);
            CacheHelper<object>.AddOrUpdate($"AspNetUsers{context.TenantID}", context.TenantID, GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetUsers, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'"));


        }

        /// <summary>
        /// Returns Names of selected Users and groups by Comma seperated Ids.
        /// </summary>        
        public string GetUserNamesById(string SelectedUsers, string Seperator = ",")
        {
            UserProfile user = null;
            Role role = null;
            StringBuilder userNames = new StringBuilder();
            UserRoleManager roleManager = new UserRoleManager(context);
            List<string> userId = UGITUtility.ConvertStringToList(SelectedUsers, Seperator);
            foreach (string entity in userId)
            {
                user = GetUserById(entity);

                if (user == null) // for User Groups
                {
                    role = roleManager.Get(x => x.Id == entity);

                    if (role != null)
                    {
                        if (userNames.Length != 0)
                            userNames.Append(";");
                        userNames.Append(role.Title);
                    }
                }
                else // for Users
                {
                    if (!string.IsNullOrEmpty(user.Name))
                    {
                        if (userNames.Length != 0)
                            userNames.Append(";");
                        userNames.Append(user.Name);
                    }
                }
            }
            return Convert.ToString(userNames);
        }
        public UserProfile GetUserProfileFromRole(Role role)
        {
            UserProfile userProfile = new UserProfile();
            if (role != null)
            {
                userProfile.Id = role.Id;
                userProfile.UserName = role.Name;
                userProfile.Name = role.Title;
            }
            return userProfile;
        }
        public void UpdateIntoCache(UserProfile user)
        {
            //Update into chache
            //DataTable dt;
            List<UserProfile> _user = new List<UserProfile>();
            _user.Add(user);
            string cacheName = "UserProfile_" + context.TenantID;
            //var userProfiles = Load(x => x.Enabled);
            //dt = UGITUtility.ToDataTable<UserProfile>(_user);
            CacheHelper<object>.AddOrUpdate($"{cacheName}", UGITUtility.ToDataTable<UserProfile>(_user));
            CacheHelper<object>.AddOrUpdate(cacheName, context.TenantID, _user);
        }
        public List<UserProfile> GetUserNames(string UsrName)
        {
            List<UserProfile> users = new List<UserProfile>();
            UserStore<UserProfile> stores = (UserStore<UserProfile>)this.Store;
            users = stores.Load(x => !x.isRole && x.UserName == UsrName && x.Enabled);
            return users;
        }
        public UsersInfo GetUserInfo(string users)
        {
            if (users == null)
                return null;

            string separator = Utility.Constants.Separator;

            if (users.Contains(Utility.Constants.Separator))
                separator = Utility.Constants.Separator;
            else if (users.Contains(Utility.Constants.Separator6))
                separator = Utility.Constants.Separator6;
            List<string> lstUserIds = UGITUtility.SplitString(users, separator, StringSplitOptions.RemoveEmptyEntries).ToList();
            UsersInfo usersInfo = new UsersInfo();
            List<UserProfile> userProfiles = new List<UserProfile>();
            List<string> emails = new List<string>();
            UserProfile userProfile = null;
            foreach (string uLookup in lstUserIds)
            {
                userProfile = GetUserById(uLookup);
                if (userProfile != null)
                {
                    userProfiles.Add(userProfile);
                    if (!string.IsNullOrWhiteSpace(userProfile.NotificationEmail))
                        emails.Add(userProfile.NotificationEmail);
                    else
                        emails.Add(userProfile.Email);
                }
                else
                {
                    userProfile = GetUserProfile(uLookup);
                    //SPGroup group = UserProfile.GetGroupByID(uLookup.LookupId, spWeb);
                    if (userProfile != null)
                    {
                        List<UserProfile> groupUsers = GetUsersByGroupID(userProfile.Id);
                        //List<UserProfile> groupUsers = UserProfile.GetGroupUsers(uLookup.LookupId, spWeb);
                        if (groupUsers != null && groupUsers.Count > 0)
                        {
                            userProfiles.AddRange(groupUsers);
                            foreach (UserProfile pf in groupUsers)
                            {
                                if (!string.IsNullOrWhiteSpace(pf.NotificationEmail))
                                    emails.Add(pf.NotificationEmail);
                                else
                                    emails.Add(pf.Email);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(userProfile.NotificationEmail))
                            emails.Add(userProfile.NotificationEmail);
                        else
                            emails.Add(userProfile.Email);
                    }
                }
            }

            userProfiles = userProfiles.Distinct().ToList();

            usersInfo.userEmails = string.Join(uGovernIT.Utility.Constants.UserInfoSeparator, emails.Where(x => !string.IsNullOrWhiteSpace(x)));
            usersInfo.userNames = string.Join(uGovernIT.Utility.Constants.UserInfoSeparator, userProfiles.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => x.Name));
            usersInfo.userIds = new ArrayList();
            usersInfo.userIds.AddRange(userProfiles.Select(x => x.Id).Distinct().ToArray());

            return usersInfo;
        }

        public bool SaveNewUser(ApplicationContext context, string puser, string defaultPassword, string displayName, string email, Enums.UserType userType)
        {
            UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            bool newUserCreated = false;

            // Generate randow password if blank passed in
            if (string.IsNullOrWhiteSpace(defaultPassword))
                defaultPassword = context.UserManager.GeneratePassword();

            if (userType == Enums.UserType.NewADUser)
            {
                var user = new UserProfile() { UserName = puser, Email = "", isRole = false };
                IdentityResult result = umanager.Create(user, defaultPassword);
                if (result.Succeeded)
                {
                    umanager.AddClaim(user.Id, new Claim("AuthProvider", "Windows"));
                }

            }
            return newUserCreated;

        }

        public bool SaveNewUser(ApplicationContext context, string username)
        {
            bool UserCreated = false;
            UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var configurationVariableHelper = new ConfigurationVariableManager(context);
            string password = configurationVariableHelper.GetValue("DefaultPassword");
            // Generate randow password if blank passed in
            if (string.IsNullOrWhiteSpace(password))
                password = context.UserManager.GeneratePassword();
            UserProfile userCreated = new UserProfile() { UserName = username, Email = username, Enabled = true, Name = username, TenantID = context.TenantID };
            IdentityResult result = umanager.Create(userCreated, password);
            if (result.Succeeded == true)
            {
                UserCreated = true;
            }
            return UserCreated;
        }
        public class UsersInfo
        {
            public string userNames;
            public string userEmails;
            public ArrayList userIds;
            public string userToolTip;
            public UsersInfo()
            {
                userNames = string.Empty;
                userEmails = string.Empty;
                userIds = new ArrayList();
                userToolTip = string.Empty;
            }
        }

    }
}