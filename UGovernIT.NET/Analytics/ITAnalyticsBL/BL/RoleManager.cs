using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;
using ITAnalyticsBL.LoginManager;
using Microsoft.AspNet.Identity;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;

namespace ITAnalyticsBL.BL
{
    public class MyRoleManager
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserProfileManager UserManager;
        private readonly ModelDB modelDB;
        private readonly ApplicationDbContext mainContext;
        public MyRoleManager(RoleManager<Role> _roleManager, UserProfileManager _UserManager, ModelDB _modelDB, ApplicationDbContext applicationDbContext)
        {
            roleManager = _roleManager;
            UserManager = _UserManager;
            modelDB = _modelDB;
            mainContext = applicationDbContext;
        }
        /// <summary>
        /// Adds User's Role
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        /// 
        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {            
            //List<Role> roles = (from m in roleManager.Roles.ToList()
            //                    join t in roleNames on m.Name equals t
            //                    select m).ToList();
            
            //foreach (Role myRole in roles)
            //{
            //    string[] users = usernames.Except(mainContext.UserRoles.Where(x => x.RoleId == myRole.Id).Select(x => x.UserName).AsEnumerable()).ToArray();
            //    foreach (string user in usernames)
            //    {
            //        UserRole usrRole = new UserRole();
            //        usrRole.UserName = user;
            //        usrRole.RoleId = myRole.Id;
            //    }
            //}
            //mainContext.SaveChanges();
           
        }

        public void CreateRole(string roleName)
        {
             Role newRole = new Role();
            newRole.Name = roleName;
            roleManager.Create(newRole);
           
        }

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
         
            Role role = roleManager.Roles.ToList().FirstOrDefault(x => x.Name == roleName);
            if (role != null)
            {
                roleManager.Delete(role);
            }
            return false;
        }

        public  string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
           
            //UserRole uRole = mainContext.UserRoles.FirstOrDefault(x => x.UserName == usernameToMatch && x.Role.Name == roleName);
            List<string> usrList = new List<string>();
            //if (uRole != null)
            //{
            //    usrList = mainContext.UserRoles.Where(x => x.Role.Name == uRole.Role.Name).Select(x => x.UserName).ToList();
            //}
            return usrList.ToArray();
        }

        public  string[] GetAllRoles()
        {
           
            return roleManager.Roles.Select(x => x.Name).ToArray();
        }

        public string[] GetRolesForUser(string username)
        {
            return null;
            //return roleManager.Roles.Where(x => x.UserRoles.FirstOrDefault(y => y.UserName == username) != null).Select(x => x.Name).ToArray();
        }

        public  string[] GetUsersInRole(string roleName)
        {           
            return roleManager.Roles.Where(x => x.Name == roleName).Select(x => x.Name).ToArray();
        }

        public  bool IsUserInRole(string username, string roleName)
        {
           
            return mainContext.UserRoles.FirstOrDefault(x => x.UserName == username && x.Role.Name == username) != null;
        }

        public  void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            List<Role> roleList = (from m in roleManager.Roles.ToList()
                                   join r in roleNames on m.Name equals r
                                   select m).ToList();
            List<UserRole> usrRoles = (from m in mainContext.UserRoles
                                       join t in roleList on m.RoleId equals t.Id
                                       select m).ToList();

            foreach (UserRole uRole in usrRoles)
            {
                mainContext.UserRoles.Remove(uRole);
            }
            mainContext.SaveChanges();
        }

        public  bool RoleExists(string roleName)
        {
            
            return roleManager.Roles.ToList().FirstOrDefault(x => x.Name == roleName) != null;
        }

        public void EditRole(string newRoleName, string oldRoleName)
        {
            Role role = roleManager.Roles.ToList().FirstOrDefault(x => x.Name == oldRoleName);
            if (role != null && roleManager.Roles.ToList().FirstOrDefault(x=>x.Name == newRoleName && x.RoleID != role.RoleID) == null)
            {
                role.Name = newRoleName;
                roleManager.Update(role);
            }
            
        }

        public static Role GetRoleByUserName(string userName,ApplicationDbContext context=null)
        {
            Role role = null;
            UserRole uRole = null;
            if (context != null)
                uRole = context.UserRoles.FirstOrDefault(x => x.UserName == userName);
           
            if (uRole != null)
            {
                if (context != null)
                    role = context.roles.FirstOrDefault(x=>x.Id== uRole.RoleId);
            }
            return role;
        }

    }
}
