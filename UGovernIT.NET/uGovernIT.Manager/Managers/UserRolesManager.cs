using System;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL.Store;
using Microsoft.AspNet.Identity;
using uGovernIT.Util.Cache;
using System.Runtime.Remoting.Contexts;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class UserRoleManager : ManagerBase<Role>
    {
        private UserProfileManager _userProfileManager = null;
        private RoleManager<Role> _roleManager = null;
        private ApplicationContext _context = null;

        public UserRoleManager(ApplicationContext context) : base(context)
        {
            _userProfileManager = new UserProfileManager(context);
            _roleManager = new RoleManager<Role>(new RoleStore<Role>(context));
            _context = context;
        }

        public void CreateUserRole(Role roles)
        {
            if (!this.GetRoleList().Exists(x => x.Name == roles.Name.ToString() && x.TenantID == _context.TenantID))
            {
                _roleManager.Create(roles);
                UpdateRolesInCache();
            }
        }

        /// <summary>
        /// Method for Add Or Update Roles
        /// </summary>
        /// <param name="roles">Object Of Role</param>
        public void AddOrUpdate(Role roles)
        {
            if (!this.GetRoleList().Exists(x => x.Name == roles.Name.ToString() && x.TenantID == _context.TenantID))
            {
                IdentityResult result = _roleManager.Create(roles);
                // below code commented beceause above method is already creating roles and users, below code throwing exception so commented below code
                //if (result.Succeeded)
                //{
                    //UserProfile user = new UserProfile() { Id = roles.Id, Name = roles.Title, UserName = roles.Name, isRole = true, TenantID = roles.TenantID };
                    //result = _userProfileManager.Create(user);
                //}
            }
            else
            {
                IdentityResult result = _roleManager.Update(roles);

                if (result.Succeeded)
                {
                    UserProfile user = new UserProfile() { Id = roles.Id, Name = roles.Title, UserName = roles.Name, isRole = true, TenantID = roles.TenantID, CreatedBy = roles.CreatedBy };
                    _userProfileManager.Update(user);
                }
            }

        }

        public List<Role> GetRoleList()
        {
            //return this.Load().Where(x => x.TenantID.Equals(_context.TenantID, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return this.Load(x => x.TenantID == _context.TenantID).ToList();
        }

        public List<Role> GetRolesByType(RoleType roleType)
        {
            return this.GetRoleList().Where(x => x.Name == roleType.ToString()).ToList();
        }

        public List<Role> GetRolesByName(string roleName)
        {
            //return this.GetRoleList().Where(x => x.Name == roleName).ToList();
            return this.GetRoleList().Where(x => x.Name == roleName || x.Title == roleName).ToList();
        }

        public Role GetRoleByName(string roleName)
        {
            return this.GetRoleList().FirstOrDefault(x => x.Name == roleName || x.Title == roleName);
        }

        public List<Role> GetUserRoleByGroup(RoleType Name)
        {
            return this.GetRoleList().Where(x => x.Name == Name.ToString()).ToList();
        }

        public List<Role> GetUserRoleByGroup(string Name)
        {
            return this.GetRoleList().Where(x => x.Name == Name.ToString()).ToList();
        }

        public void UpdateUserRole(Role role)
        {
            if (this.GetRoleList().Exists(x => x.Id == role.Id && x.TenantID == _context.TenantID))
            {
                _roleManager.Update(role);
                UpdateRolesInCache();
            }
        }

        public void UpdateRolesInCache()
        {
            var roleCollection = this.GetRoleList();
            string cacheName = "Lookup_" + DatabaseObjects.Tables.AspNetRoles + "_" + _context.TenantID;
            if (roleCollection != null || roleCollection.Count > 0)
            {
                CacheHelper<object>.AddOrUpdate(cacheName, _context.TenantID, UGITUtility.ToDataTable<Role>(roleCollection));
            }
        }
    }
}
