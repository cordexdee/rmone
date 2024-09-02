using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uGovernIT.Utility;
using System.Security.Claims;
using uGovernIT.Utility.Entities;
using System.Data.SqlClient;
using System.Data;
using uGovernIT.DAL.Infratructure;
using System.Configuration;
using uGovernIT.Util.Cache;


namespace uGovernIT.DAL.Store
{
    public class UserStore<TUser> : StoreBase<TUser>,
            IUserLoginStore<TUser>,
            IUserClaimStore<TUser>,
            IUserRoleStore<TUser>,
            IUserPasswordStore<TUser>,
            IUserSecurityStampStore<TUser>,
            IQueryableUserStore<TUser>,
            IUserEmailStore<TUser>,
            IUserPhoneNumberStore<TUser>,
            IUserTwoFactorStore<TUser, String>,
            IUserLockoutStore<TUser, String>,
            IUserStore<TUser, string>,
            IUserStore<TUser>,
            IIdentityValidator<TUser>

            where TUser : UserProfile
    {

        private Boolean disposed;
        private RoleStore<Role> roleStore;

        public UserStore(CustomDbContext context) : base(context)
        {
            this.disposed = false;
            roleStore = new RoleStore<Role>(context);
        }

        public async Task<IdentityResult> ValidateAsync(TUser user)
        {
            await Task.FromResult(0);
            var errors = new List<string>();

            //if (_userManager != null)
            //{
            //    //check username availability. and add a custom error message to the returned errors list.
            //    //    var existingAccount = await _userManager.FindByNameAsync(user.UserName);
            //    //    if (existingAccount != null && existingAccount.Id != user.Id)
            //    //        errors.Add("User name already in use ...");
            //    //
            //}

            //set the returned result (pass/fail) which can be read via the Identity Result returned from the CreateUserAsync
            //    return errors.Any()
            //        ? IdentityResult.Failed(errors.ToArray())
            //        : IdentityResult.Success;
            return IdentityResult.Success;
        }

        public IIdentityValidator<TUser> IdentityValidator
        {
            get
            {
                return this.IdentityValidator;
            }

        }


        public IQueryable<TUser> Users
        {
            get
            {
                return this.Load().AsQueryable();
            }
        }

        public async Task AddClaimAsync(TUser user, Claim claim)
        {
            long i = 0;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.Set<Claim>().Add(claim);
                ctx.SaveChanges();
            }
            await Task.FromResult(i);
        }

        public async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            long i = 0;
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                ctx.Set<UserLoginInfo>().Add(login);
                ctx.SaveChanges();
            }
            await Task.FromResult(i);
        }

        public async Task AddToRoleAsync(TUser user, string roleName)
        {
            long i = 0;
            Role role = await roleStore.FindByNameAsync(roleName);
            UserRole userRole = new UserRole() { RoleId = role.Id, Title = "", UserId = user.Id };
            StoreBase<UserRole> urStore = new StoreBase<UserRole>(context);
            userRole.TenantID = context.TenantID;
            urStore.Insert(userRole);
            await Task.FromResult(i);
        }

        public async Task RemoveFromRolesAsync(TUser user, string roleName)
        {
            long i = 0;
            Role role = await roleStore.FindByNameAsync(roleName);
            StoreBase<UserRole> urStore = new StoreBase<UserRole>(context);
            UserRole userRole = urStore.Get(x => x.RoleId == role.Id && x.UserId == user.Id);
            if (userRole != null)
                urStore.Delete(userRole);
            await Task.FromResult(i);
        }
        public async Task CreateAsync(TUser user)
        {
            user.TenantID = context.TenantID;
            this.Insert(user);

            CacheHelper<TUser>.AddOrUpdate(user.UserName, context.TenantID, user);
            CacheHelper<TUser>.AddOrUpdate(user.Email, context.TenantID, user);
            CacheHelper<TUser>.AddOrUpdate(user.Id, context.TenantID, user);
            await Task.FromResult(0);
        }

        public async Task DeleteAsync(TUser user)
        {
            this.Delete(user);

            CacheHelper<TUser>.Delete(user.UserName, context.TenantID);
            CacheHelper<TUser>.Delete(user.Email, context.TenantID);
            CacheHelper<TUser>.Delete(user.Id, context.TenantID);
            await Task.FromResult(0);
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.disposed = true;
            }
        }

        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            return await Task.Run(async () =>
            {
                TUser user = null;
                UserLogin userLogin;
                StoreBase<UserLogin> ulStore = new StoreBase<UserLogin>(context);
                userLogin = ulStore.Get(x => x.ProviderKey == login.ProviderKey);
                if (userLogin == null)
                    return null;

                user = await FindByIdAsync(userLogin.UserId);
                return user;
            });
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await Task.Run(() =>
            {
                TUser user = CacheHelper<TUser>.Get(email, context.TenantID);
                if (user == null)
                {
                    user = this.Get(x => x.Email == email);
                    CacheHelper<TUser>.AddOrUpdate(email, context.TenantID, user);
                }
                return user;
            });
        }
        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            return await Task.Run(() =>
            {
                List<Claim> claims = new List<Claim>();
                using (var sqlConnection = new SqlConnection(context.Database))
                {
                    SqlCommand cmd = new SqlCommand("select * from " + DatabaseObjects.Tables.AspNetUserClaims + " (nolock)", sqlConnection);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable aDt = new DataTable();
                    adp.Fill(aDt);
                    DataRow[] rows = aDt.Select(string.Format("{0} = '{1}'", "UserId", user.Id));
                    foreach (DataRow row in rows)
                    {
                        Claim d = new Claim(Convert.ToString(row["ClaimType"]), Convert.ToString(row["ClaimValue"]));
                        claims.Add(d);
                    }
                }
                return claims;
            });
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            return await Task.Run(() =>
            {
                TUser user = CacheHelper<TUser>.Get(userId, context.TenantID);
                if (user == null)
                {
                    user = this.Get(x => x.Id == userId);
                    CacheHelper<TUser>.AddOrUpdate(userId, context.TenantID, user);
                }
                return user;
            });
        }
        /// <summary>
        /// tenantAccountId is optional if passed then filter else use default code
        /// It is added to avoid conflict and keep similer call from web browser or api both
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="tenantAccountId"></param>
        /// <returns></returns>
        public async Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            string tenantAccountId = string.Empty;
            string authenticationType = string.Empty; // to check, if call is from API
            return await Task.Run(() =>
            {

                TUser user;
                if (ConfigurationManager.AppSettings["DefaultUser"] != null && Convert.ToString(ConfigurationManager.AppSettings["DefaultUser"]).Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                {
                    user = this.Get(x => x.UserName == userName);
                }
                else
                {
                    user = null;
                    authenticationType = UGITUtility.AuthenticationType;

                    // to check, if call is from API
                    if (string.IsNullOrEmpty(authenticationType))
                        user = CacheHelper<TUser>.Get(userName, context.TenantID);                    
                    
                    if (user == null)
                    {
                        if (string.IsNullOrEmpty(authenticationType))
                            user = this.Get(x => x.UserName == userName && x.TenantID == context.TenantID);

                        //To featch user based on user name
                        //It is conflicting with default keys in web.config
                        //Causing issue from api calling 
                        tenantAccountId = UGITUtility.TenantAccount;

                        if (user == null && !string.IsNullOrEmpty(tenantAccountId))
                        {
                            Tenant tenant = GetTenant(tenantAccountId);
                            if (tenant != null)
                                user = this.Get(x => x.UserName == userName && x.TenantID == Convert.ToString(tenant.TenantID));
                        }

                        // Allow user authentication when AccountId/Tenant not provided.
                        if (user == null && string.IsNullOrEmpty(tenantAccountId) && !string.IsNullOrEmpty(authenticationType))
                            user = this.Get(x => x.UserName == userName);

                        //CacheHelper<TUser>.AddOrUpdate(userName, context.TenantID, user);
                        if (user != null)
                        {
                            context.TenantID = user.TenantID;
                            CacheHelper<TUser>.AddOrUpdate(userName, context.TenantID, user);
                        }
                    }

                    // AuthenticationType not clearing after logging from API, then from UI, with different tenants
                    Utility.UGITUtility.AuthenticationType = null;
                }
                return user;
            });
        }

        public TUser FindOnlyByName(string userName)
        {
            TUser user;
            //user = this.Get(x => x.UserName == userName);
            user = this.Get(x => x.UserName == userName && x.TenantID == context.TenantID);
            return user;
        }

        public TUser FindOnlyByDisplayName(string displayName)
        {
            TUser user;
            //user = this.Get(x => x.Name == displayName);
            user = this.Get(x => x.Name == displayName && x.TenantID == context.TenantID);
            return user;
        }

        public async Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }


        public async Task<string> GetEmailAsync(TUser user)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return await Task.FromResult(user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc.Value : DateTimeOffset.MinValue);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return await Task.Run(() =>
            {
                List<UserLogin> userLoginList;
                List<UserLoginInfo> userlogininfolist;
                StoreBase<UserLogin> loginStore = new StoreBase<UserLogin>(context);
                userLoginList = loginStore.Load(x => x.UserId == user.Id).ToList();
                userlogininfolist = AutoMapper.Mapper.Map<List<UserLogin>, List<UserLoginInfo>>(userLoginList);
                return userlogininfolist;
            });
        }

        public async Task<string> GetPasswordHashAsync(TUser user)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<string> GetPhoneNumberAsync(TUser user)
        {
            return await Task.FromResult(user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return await Task.FromResult(user.PhoneNumberConfirmed);
        }
        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            return await Task.Run(() =>
            {
                List<string> roles = null;
                StoreBase<UserRole> urStore = new StoreBase<UserRole>(context);
                List<UserRole> uRoles = urStore.Load(x => x.UserId == user.Id);
                List<Role> allRoles = roleStore.Load();
                roles = allRoles.Where(x => uRoles.Exists(y => y.RoleId == x.Id)).Select(x => x.Name).ToList();
                return roles;
            });

        }
        public async Task<IList<Role>> GetRolesAsync(string user)
        {
            return await Task.Run(() =>
            {
                List<Role> roles = null;
                StoreBase<UserRole> urStore = new StoreBase<UserRole>(context);
                List<UserRole> uRoles = urStore.Load(x => x.UserId == user);
                List<Role> allRoles = roleStore.Load();
                roles = allRoles.Where(x => uRoles.Exists(y => y.RoleId == x.Id)).ToList();
                return roles;
            });

        }


        public async Task<string> GetSecurityStampAsync(TUser user)
        {
            return await Task.FromResult(user.SecurityStamp);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public async Task<bool> HasPasswordAsync(TUser user)
        {
            return await Task.FromResult(!String.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount += 1;
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            Role role = await roleStore.FindByNameAsync(roleName);
            if (role == null)
                return false;

            return await Task.Run(() =>
            {
                UserRole uRole = null;
                StoreBase<UserRole> urStore = new StoreBase<UserRole>(context);
                uRole = urStore.Get(x => x.RoleId == role.Id && x.UserId == user.Id);
                if (uRole != null)
                    return true;
                return false;
            });
        }

        public async Task<bool> RemoveClaimAsync(TUser user, Claim claim)
        {
            bool i;
            StoreBase<Claim> cStore = new StoreBase<Claim>(context);
            i = cStore.Delete(claim);
            return await Task.FromResult(i);
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            Role role = roleStore.Get(x => x.Name == roleName);
            if (role != null)
            {
                StoreBase<UserRole> urBase = new StoreBase<UserRole>(context);
                UserRole userRole = urBase.Get(x => x.RoleId == role.Id && x.UserId == user.Id);
                urBase.Delete(userRole);
            }
            await Task.FromResult(0);
        }

        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            StoreBase<UserLogin> loginStore = new StoreBase<UserLogin>(context);
            UserLogin userLogin = new UserLogin() { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey, UserId = user.Id };
            loginStore.Delete(userLogin);
            await Task.FromResult(0);
        }

        public async Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            await Task.FromResult(0);
        }

        public async Task SetEmailAsync(TUser user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.Email = email;
            await Task.FromResult(0);
        }

        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.EmailConfirmed = confirmed;
            await Task.FromResult(0);
        }

        public async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            await Task.FromResult(0);
        }

        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = lockoutEnd;
            await Task.FromResult(0);
        }

        public async Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            await Task.FromResult(0);

        }

        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumber = phoneNumber;
            await Task.FromResult(0);
        }

        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PhoneNumberConfirmed = confirmed;
            await Task.FromResult(0);
        }

        public async Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            await Task.FromResult(0);
        }

        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            await Task.FromResult(0);
        }

        public async Task UpdateAsync(TUser user)
        {
            this.Update(user);

            CacheHelper<TUser>.AddOrUpdate(user.UserName, context.TenantID, user);
            CacheHelper<TUser>.AddOrUpdate(user.Email, context.TenantID, user);
            CacheHelper<TUser>.AddOrUpdate(user.Id, context.TenantID, user);
            await Task.FromResult(0);
        }

        public async Task<List<TUser>> GetUsersByRole(string roleID)
        {
            List<TUser> userProfiles;
            StoreBase<UserRole> urStore = new StoreBase<UserRole>(context);
            List<UserRole> userRoles = urStore.Load(x => x.RoleId == roleID).ToList();
            userProfiles = Users.Where(x => userRoles.Any(y => y.UserId.Equals(x.Id))).ToList();
            return await Task.FromResult(userProfiles);
        }
        public async Task<List<UserRole>> GetUserRolesByRole(string roleID)
        {
            List<UserRole> userRoles;
            StoreBase<UserRole> urStore = new StoreBase<UserRole>(context);
            if(!string.IsNullOrEmpty(roleID))
                userRoles = urStore.Load(x => x.RoleId == roleID).ToList();
            else
                userRoles = urStore.Load().ToList();
            //userProfiles = Users.Where(x => userRoles.Any(y => y.UserId.Equals(x.Id))).ToList();
            return await Task.FromResult(userRoles);
        }

        Task IUserClaimStore<TUser, string>.RemoveClaimAsync(TUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

    }

}
