using System;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class LandingPagesManager : ManagerBase<LandingPages>
    {
        UserProfileManager userProfileManager = null;
        ApplicationContext _context = null;
        public LandingPagesManager(ApplicationContext context) : base(context)
        {
             _context = context;
        }        

        public List<LandingPages> GetLandingPages()
        {
            return this.Load().Where(x => x.TenantID.Equals(_context.TenantID, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public List<LandingPages> GetRolesByType(RoleType roleType)
        {
            return this.GetLandingPages().Where(x => x.Name == roleType.ToString()).ToList();
        }
        public List<LandingPages> GetRolesByName(string roleName)
        {
            return this.GetLandingPages().Where(x => x.Name == roleName).ToList();
        }
        public LandingPages GetRoleByName(string roleName)
        {
            return this.GetLandingPages().FirstOrDefault(x => x.Name == roleName);
        }
        
        public LandingPages GetUserRoleById(string Id)
        {
            return this.GetLandingPages().Where(x => x.Id == Id.ToString()).FirstOrDefault();
        }
        public string GetLandingPageById(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
                return this.GetLandingPages().Where(x => x.Id == Id.ToString()).Select(x => x.LandingPage).FirstOrDefault();
            else
                return string.Empty;
        }


        public string LandingPagesfinal(UserProfile user, string frommail = null)
        {
            string landingPageUrl = string.Empty;
            userProfileManager = new UserProfileManager(_context);
            TenantManager tenantManager = new TenantManager(_context);
            if (user != null)
            {
                if (user.UserRoleId == null)
                {
                    if (uHelper.IsDataAllSet(_context))
                    {
                        //landingPageUrl = UGITUtility.GetAbsoluteURL("/Pages/UserHomePage");
                        landingPageUrl = UGITUtility.GetAbsoluteURL(uHelper.GetDefaultLandingPage(_context, user));
                    }
                    else
                        landingPageUrl = UGITUtility.GetAbsoluteURL("/SitePages/NewLoginWizard.aspx");
                }
                else
                {
                    if (tenantManager.IsTrialTenant(_context.TenantID) && (user.IsShowDefaultAdminPage == true) && userProfileManager.IsAdmin(user))
                    {
                        landingPageUrl = "~/Admin/NewAdminUI.aspx" + "?deft=true";
                        if (frommail == "frommail")
                        {
                            landingPageUrl = "Admin/NewAdminUI.aspx" + "?deft=true";
                        }
                    }
                    else if(userProfileManager.IsUGITSuperAdmin(user))
                    {                        
                        landingPageUrl = "SuperAdmin/SuperAdmin.aspx";
                    }
                    else
                    {
                        landingPageUrl = GetLandingPageById(user.UserRoleId);
                    }

                }
            }
            

            return landingPageUrl;
        }
    }
}
