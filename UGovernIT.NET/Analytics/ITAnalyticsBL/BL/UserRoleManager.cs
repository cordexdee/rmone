using ITAnalyticsBL.DB;
using ITAnalyticsBL.LoginManager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITAnalyticsBL.BL
{     
    public class UserRoleManager : RoleManager<Role>
    {
        public UserRoleManager(IRoleStore<Role, string> store) : base(store)
        {
        }
        public static UserRoleManager Create(IdentityFactoryOptions<UserRoleManager> options, IOwinContext context)
        {
            var roleStore = new RoleStore<Role>(context.Get<ApplicationDbContext>());
            return new UserRoleManager(roleStore);
        }
        public bool IsRoleExist(string Name,long companyId)
        {
            return this.Roles.Where(x => x.Name.ToString().ToLower() == Name.ToLower() && x.CompanyID == companyId).Count() > 0;
        }
        
    }
}
