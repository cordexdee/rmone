using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public class TenantHelper
    {
        public static string GetTanantID()
        {
            var currentUser = HttpContext.Current.CurrentUser();

            if (currentUser != null)
                return currentUser.TenantID;
            else
                return string.Empty;
        }
    }

    public class PermissionHelper
    {
        ApplicationContext context = null;
        UserProfile user = null;

        public PermissionHelper()
        {
            context = HttpContext.Current.GetManagerContext();
            user = HttpContext.Current.CurrentUser();
        }

        public bool isAdmin()
        {
            var isAdmin = false;
            try
            {
                var manager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                isAdmin = manager.IsRole(RoleType.Admin, user.UserName);
            }
            catch (Exception)
            {

                throw;
            }
            return isAdmin;
        }

        public bool IsOffice365Subscription()
        {
            var isOffice365 = false;
            try
            {
                var moduleViewManager = new ModuleUserTypeManager(context);
                var tenant = moduleViewManager.GetTenantById(user.TenantID);
                isOffice365 = tenant.IsOffice365Subscription;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return isOffice365;
        }

        public static bool IsOnboardingUIRequest()
        {
            return UGITUtility.IsOnboardingUIRequest();
        }
        
        public static bool IsOnboardingUIRequestType()
        {
            return UGITUtility.IsOnboardingUIRequestType();
        }
        /*
        public UserRoles getUserRole(string roleId)
        {
            UserRolesManager ObjUserRolesManager = new UserRolesManager(HttpContext.Current.GetManagerContext());
            UserRoles userRole = new UserRoles();
            if (!string.IsNullOrEmpty(roleId))
            {
                userRole = ObjUserRolesManager.GetUserRoleById(roleId);
            }
            return userRole;
        }
        */
        public LandingPages getUserRole(string roleId)
        {
            LandingPagesManager ObjLandingPagesManager = new LandingPagesManager(HttpContext.Current.GetManagerContext());
            LandingPages landingPages = new LandingPages();
            if (!string.IsNullOrEmpty(roleId))
            {
                landingPages = ObjLandingPagesManager.GetUserRoleById(roleId);
            }
            return landingPages;
        }

        public UserProfile getCurrentUser()
        {
            var context = HttpContext.Current.GetManagerContext();
            UserProfile user = HttpContext.Current.CurrentUser();
            return user;
        }

        public string getCompanyLogo()
        {
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            var ImageUrl = objConfigurationVariableHelper.GetValue(ConfigConstants.HeaderLogo);

            return ImageUrl;
        }
    }
}