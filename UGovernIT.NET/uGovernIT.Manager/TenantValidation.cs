using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
   public class TenantValidation
    {
        private TenantManager _tenantManager = null;
        private ApplicationContext _context = null;
        private ConfigurationVariableManager _configurationVariableManager = null;

        
        bool isTrialTenant = false;
       
        UserProfileManager userManager;

        public TenantValidation(ApplicationContext context)
        {
            _context = context;
            //userManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

        }

        //protected ApplicationContext ApplicationContext
        //{
        //    get
        //    {
        //        if (_context == null)
        //        {
        //            _context = HttpContext.Current.GetManagerContext();
        //        }
        //        return _context;
        //    }
        //}

        protected TenantManager TenantManager
        {
            get
            {
                if (_tenantManager == null)
                {
                    _tenantManager = new TenantManager(_context);
                }
                return _tenantManager;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(_context);
                }
                return _configurationVariableManager;
            }

        }

        //UserProfileManager UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public bool IsUserLimitExceed()
        {
            isTrialTenant = TenantManager.IsTrialTenant(_context.TenantID);

            userManager = new UserProfileManager(_context);

            if (isTrialTenant)
            {                
                var userCount = _context.UserManager.GetUserCountForTenant(_context.TenantID);

                var masterTenant = TenantManager.GetTenant(ConfigHelper.DefaultTenant.ToLower());

                if (masterTenant != null)
                {
                    var numberOfFreeUserAccounts = uGITDAL.GetSingleValue(DatabaseObjects.Tables.ConfigurationVariable,
                         $"{DatabaseObjects.Columns.TenantID}='{masterTenant.TenantID}'and {DatabaseObjects.Columns.KeyName}='{ConfigConstants.NumberOfFreeUserAccounts}'", DatabaseObjects.Columns.KeyValue);

                    if (!string.IsNullOrEmpty(Convert.ToString(numberOfFreeUserAccounts)))
                    {
                        //isLimitExceed = count > Convert.ToInt64(numberOfFreeUserAccounts) ? true : false;
                        return userCount >= Convert.ToInt64(numberOfFreeUserAccounts);
                    }
                }
            }

            return false;
        }

        public bool IsNewTenantCreationLimitExceded()
        {
            TenantManager tenantManager = new TenantManager(ApplicationContext.Create());

            var masterTenant = TenantManager.GetTenant(ConfigHelper.DefaultTenant.ToLower());
            int NoOfTenantsCreatedToday = tenantManager.GetTenantList().Where(x => x.Created.Year == DateTime.Today.Year && x.Created.Month == DateTime.Today.Month && x.Created.Day == DateTime.Today.Day).Count(); 
            if (masterTenant != null)
            {
                var NewTenantCreationPerDay = uGITDAL.GetSingleValue(DatabaseObjects.Tables.ConfigurationVariable, $"{DatabaseObjects.Columns.TenantID}='{masterTenant.TenantID}'and {DatabaseObjects.Columns.KeyName}='{ConfigConstants.NewTenantCreationPerDay}'", DatabaseObjects.Columns.KeyValue);
                if (!string.IsNullOrEmpty(Convert.ToString(NewTenantCreationPerDay)))
                {
                    return NoOfTenantsCreatedToday >= Convert.ToInt64(NewTenantCreationPerDay);
                }
            }

            return false;
        }
    }
}
