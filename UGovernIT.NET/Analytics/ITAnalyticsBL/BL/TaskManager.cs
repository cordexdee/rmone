using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITAnalyticsBL.DB;

namespace ITAnalyticsBL.BL
{
    public class TaskManager
    {
        public static  List<Task> GetTasksByUserName(string userName)
        {
            UserContext userContext = new UserContext();
            //List<Task> userTasks = (from user in userContext.UserRoles join role in userContext.roles on user.RoleId equals role.Id join roletask in userContext.RoleTasks on role.RoleID equals roletask.RoleID join task in userContext.Tasks on roletask.TaskID equals task.TaskID
            //                        where user.UserName.Equals(userName) select task).ToList();
            //return userTasks;
            return new List<Task>();
        }

        public static List<Task> GetTasksByRoleName(string roleName)
        {
            //UserContext userContext = new UserContext();
            //List<Task> roleTask = (from m in userContext.roles
            //                       join t in userContext.RoleTasks on m.RoleID equals t.RoleID
            //                       join p in userContext.Tasks on t.TaskID equals p.TaskID
            //                       where m.Name == roleName
            //                       select p).ToList();
            //return roleTask;

            return new List<Task>();

        }

        public static List<TaskGroup> GetAllTaskGroup()
        {
            UserContext userContext = new UserContext();
            return userContext.TaskGroups.ToList();
        }

        public static List<Task> GetTaskByRoleID(long roleID)
        {
            UserContext userContext = new UserContext();
            return (from rt in userContext.RoleTasks
                    join t in userContext.Tasks on rt.TaskID equals t.TaskID
                    where rt.RoleID == roleID
                    select t).ToList();
        }

    }
}
