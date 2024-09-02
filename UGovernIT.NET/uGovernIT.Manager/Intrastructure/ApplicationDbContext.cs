using System;
using System.Configuration;
using System.Data;
using System.Linq;
using uGovernIT.DAL;
using uGovernIT.DAL.Infratructure;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public interface IApplicationContext
    {
        UserProfile CurrentUser { get; }
        UserProfileManager UserManager { get; }
    }

    public class ApplicationContext : CustomDbContext, IDisposable, IApplicationContext
    {
        const string DBCONNECTION = "cnn";
        const string DBCONNECTIONCOMMON = "tenantcnn";

        public string TID { get; set; }
        public UserProfileManager UserManager { get; private set; }
        public ConfigurationVariableManager ConfigManager { get; set; }
        public IPageConfigurationManager PageManager { get; set; }
        public string SiteUrl { get; set; }
        //public string ReportUrl { get; set; }

        private ApplicationContext(string database, string commonDatabase) : base(database, commonDatabase)
        {

        }     

        public void SetCurrentUser(UserProfile user)
        {
            this.CurrentUser = user;
            if (user != null)
            {
                this.TenantID = user.TenantID;
                GetTenantAccountId(user.TenantID);
            }
        }

        public void GetTenantAccountId(string tenantId)
        {
            var tenant = uGITDAL.GetTenantList().Where(x => x.TenantID.ToString().Equals(TenantID,StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            this.TenantAccountId = tenant.AccountID;
            this.CurrentUser.AccountId = tenant.AccountID;
        }

        public void SetUserManager(UserProfileManager manager)
        {
            this.UserManager = manager;
        }

        public void SetConfigManager(ConfigurationVariableManager configManager)
        {
            this.ConfigManager = configManager;
        }

        public void SetPageManager(IPageConfigurationManager pageManager)
        {
            this.PageManager = pageManager;
        }

        public static ApplicationContext CreateContext(string TenantID, string UserName = "")
        {
            string dbUrl = string.Empty;
            string dbUrlCommon = string.Empty;

            dbUrl = Convert.ToString(ConfigurationManager.ConnectionStrings[DBCONNECTION]);
            dbUrlCommon = Convert.ToString(ConfigurationManager.ConnectionStrings[DBCONNECTIONCOMMON]);

            Guid tenantGUID;
            Guid.TryParse(TenantID, out tenantGUID);

            Tenant tenant = uGITDAL.GetTenantList().FirstOrDefault(x => x.TenantID.Equals(tenantGUID));
            ApplicationContext ctx = new ApplicationContext(dbUrl, dbUrlCommon);

            ctx.TenantID = TenantID;
            //ctx.AccountId = tenant.AccountID;
            ctx.SetUserManager(new UserProfileManager(ctx));

            //changed tenant.Tenantname to tenant.AccountId as tenant is username of tenant Administrator + AccountId 
            if (tenant != null)
            {
                if (string.IsNullOrEmpty(UserName))
                    ctx.SetCurrentUser(ctx.UserManager.GetUserByUserName("Administrator_" + tenant.AccountID));
                else
                    ctx.SetCurrentUser(ctx.UserManager.GetUserByUserName(UserName));
            }
            ctx.SetConfigManager(new ConfigurationVariableManager(ctx));
            ctx.SetPageManager(new PageConfigurationManager(ctx));
            ctx.SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];
            //ctx.ReportUrl = ConfigurationManager.AppSettings["ReportUrl"];
            return ctx;
        }

        public static ApplicationContext Create()
        {
            string dbUrl = string.Empty;
            string dbUrlCommon = string.Empty;

            dbUrl = Convert.ToString(ConfigurationManager.ConnectionStrings[DBCONNECTION]);
            dbUrlCommon = Convert.ToString(ConfigurationManager.ConnectionStrings[DBCONNECTIONCOMMON]);

            ApplicationContext ctx = new ApplicationContext(dbUrl, dbUrlCommon);
            ctx.SetUserManager(new UserProfileManager(ctx));

            if (ConfigurationManager.AppSettings["DefaultUser"] != null)
                ctx.SetCurrentUser(ctx.UserManager.GetUserByUserName(ConfigurationManager.AppSettings["DefaultUser"]));

            ctx.SetConfigManager(new ConfigurationVariableManager(ctx));
            ctx.SetPageManager(new PageConfigurationManager(ctx));
            ctx.SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];
            //ctx.ReportUrl = ConfigurationManager.AppSettings["ReportUrl"];

            return ctx;
        }


        public static ApplicationContext CreateContext(string dbConnectionString, string dbCommanConnectionString, string accountID)
        {
          
            ApplicationContext ctx = new ApplicationContext(dbConnectionString, dbCommanConnectionString);
            ManagerBase<Tenant> tenantMgr = new ManagerBase<Tenant>(ctx);
            Tenant tenant = tenantMgr.GetTenant(accountID);

            if (tenant != null)
                ctx.TenantID = tenant.TenantID.ToString();

            ctx.TenantAccountId = accountID;
            ctx.SetUserManager(new UserProfileManager(ctx));
            ctx.SetConfigManager(new ConfigurationVariableManager(ctx));
            ctx.SetPageManager(new PageConfigurationManager(ctx));
            return ctx;
        }
    }
}
