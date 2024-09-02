using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager;
using System.Linq;

namespace uGovernIT.Web
{
    public static class ContextHelper
    {
        public static UserProfile CurrentUser(this HttpContext context)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                return null;
            }

            if (context.Items.Contains("CurrentUser"))
            {
                return context.Items["CurrentUser"] as UserProfile;
            }
            else
            {
                var manager = context.GetOwinContext().Get<UserProfileManager>();

                if (manager == null)
                    return null;

                var user = Task.Run(async () => { return await manager.FindByIdAsync(context.User.Identity.GetUserId()); });

                Task.WaitAny(user);

                UserProfile profile = user.Result;
                context.Items.Add("CurrentUser", profile);

                return profile;
            }
        }
        public static ApplicationContext AppContext {
            get {
                return HttpContext.Current.GetOwinContext().Get<ApplicationContext>();
            }
        }
        public static ApplicationContext GetManagerContext(this HttpContext context)
        {
            return HttpContext.Current.GetOwinContext().Get<ApplicationContext>();
        }

        public static UserProfileManager GetUserManager(this HttpContext context)
        {
            return HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
        }

        public static UserRoleManager GetRoleManager(this HttpContext context)
        {
            return HttpContext.Current.GetOwinContext().Get<UserRoleManager>();
        }

        public static List<string> TableList(this HttpContext context)
        {
            var tabList = new List<string>();
            //var dr = DBConnection.ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES (NOLOCK) WHERE TABLE_TYPE='BASE TABLE'");
             
            var dr = DBConnection.ExecuteReader("exec GetAllTableList");
            while (dr.Read())
            {
                tabList.Add(dr["TABLE_NAME"].ToString());
            }
            return tabList;
        }

        public static UserProfile FindByName(this UserManager<UserProfile, string> manager, string userName, string TenantId)
        {
            UserProfile profile = manager.Users.FirstOrDefault(x => x.UserName.EqualsIgnoreCase(userName) && x.TenantID.EqualsIgnoreCase(TenantId));
            return profile;
        }

        public static UserProfile FindByEmail(this UserManager<UserProfile, string> manager, string email, string TenantId)
        {
            UserProfile profile = manager.Users.FirstOrDefault(x => x.Email.EqualsIgnoreCase(email) || x.UserName.EqualsIgnoreCase(email) && x.TenantID.EqualsIgnoreCase(TenantId));
            return profile;
        }

        public static UserProfile FindByEmailIgnoreCurrentUser(this UserManager<UserProfile, string> manager, string email, string TenantId,string userId)
        {
            UserProfile profile = manager.Users.FirstOrDefault(x => x.Email.EqualsIgnoreCase(email) && x.TenantID.EqualsIgnoreCase(TenantId) && x.Id != userId);
            return profile;
        }
    }
}
