using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager.JobSchedulers
{

    public class WarmUpEntireCache : IJobScheduler
    {

        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            ClearWarmUpEntireCache(context);
            
        }

        private void ClearWarmUpEntireCache(ApplicationContext _context)
        {
            if (_context.TenantID != "")
            {
                CacheHelper<object>.ClearWithRegion(_context.TenantID);
                CacheHelper<PageConfiguration>.ClearWithRegion(_context.TenantID);
                CacheHelper<ConfigurationVariable>.ClearWithRegion(_context.TenantID);
                CacheHelper<UGITModule>.ClearWithRegion(_context.TenantID);
                CacheHelper<UserProfile>.ClearWithRegion(_context.TenantID);
                CacheHelper<FieldConfiguration>.ClearWithRegion(_context.TenantID);
                CacheHelper<Dashboard>.ClearWithRegion(_context.TenantID);
                CacheHelper<Services>.ClearWithRegion(_context.TenantID);
                CacheHelper<DashboardPanelView>.ClearWithRegion(_context.TenantID);

                // Reload PageConfiguration Cache
                PageConfigurationManager configurationManager = new PageConfigurationManager(_context);
                configurationManager.RefreshCache();

                // Reload Configuration variable Cache
                ConfigurationVariableManager cVManager = new ConfigurationVariableManager(_context);
                cVManager.RefreshCache();

                // Reload Module Cache
                ModuleViewManager viewManager = new ModuleViewManager(_context);
                viewManager.RefreshCache();

                // Reload Field Configuration Cache
                FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_context);
                fieldConfigurationManager.RefreshCache();

                // Reload Dashboard Cache
                DashboardManager dashboardManager = new DashboardManager(_context);
                dashboardManager.RefreshCache();

                // Reload Service Cache
                ServicesManager servicesManager = new ServicesManager(_context);
                servicesManager.RefreshCache();

                // Reload Dashboard Panel View Cache
                DashboardPanelViewManager dPanelViewManager = new DashboardPanelViewManager(_context);
                dPanelViewManager.RefreshCache();

                //Update cache updated date
                CacheStatisticHelper.UpdateStat(CacheStatisticConstants.CONFIGURATIONCACHEUPDATEON, _context.TenantID, DateTime.UtcNow);

                UserProfileManager profileManager = new UserProfileManager(_context);
                profileManager.RefreshCache();

                //Update cache updated date
                CacheStatisticHelper.UpdateStat(CacheStatisticConstants.PROFILECACHEUPDATEON, _context.TenantID, DateTime.UtcNow);

                TicketManager ticketManager = new TicketManager(_context);
                ticketManager.RefreshCache();

                //Update cache updated date
                CacheStatisticHelper.UpdateStat(CacheStatisticConstants.TICKETCACHEUPDATEON, _context.TenantID, DateTime.UtcNow);
            }
        }


    }
}
