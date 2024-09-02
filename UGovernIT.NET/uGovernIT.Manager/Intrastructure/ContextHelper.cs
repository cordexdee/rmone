using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Utility.Entities;
using System.Data.SqlClient;

namespace uGovernIT.Manager
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
            List<string> tabList = new List<string>();
            SqlDataReader dr = DBConnection.ExecuteReader("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES (NOLOCK) WHERE TABLE_TYPE='BASE TABLE'");
            while (dr.Read())
            {
                tabList.Add(dr["TABLE_NAME"].ToString());
            }
            return tabList;
        }
    }
}
