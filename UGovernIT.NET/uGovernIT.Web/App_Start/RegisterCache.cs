using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Helpers;

namespace uGovernIT.Web
{
    public static class RegisterCache
    {
        public static void Register()
        {
            CacheHelper<object>.AddCacheInstance();
            CacheHelper<PageConfiguration>.AddCacheInstance();
            CacheHelper<ConfigurationVariable>.AddCacheInstance();
            CacheHelper<UGITModule>.AddCacheInstance();
            CacheHelper<UserProfile>.AddCacheInstance();
            CacheHelper<FieldConfiguration>.AddCacheInstance();
            CacheHelper<Dashboard>.AddCacheInstance();
            CacheHelper<Services>.AddCacheInstance();
            CacheHelper<DashboardPanelView>.AddCacheInstance();
            CacheHelper<ResourceWorkItems>.AddCacheInstance();
            CacheHelper<RResourceAllocation>.AddCacheInstance();
            ApplicationContext _context = ApplicationContext.Create();
            ULog.Create(_context.TenantAccountId, _context.CurrentUser.Name);
            bool enableCache = UGITUtility.StringToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableLoadCache"]);
            if(enableCache)
                LoadCacheData();
        }

        public static void ClearCache(string tenantID)
        {
            StringBuilder cacheDetails = new StringBuilder();
            cacheDetails.Append($"Starting Cache Clear Object.. \n");
            CacheHelper<object>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear Page Configuration.. \n");
            CacheHelper<PageConfiguration>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear Configuration Variable.. \n");
            CacheHelper<ConfigurationVariable>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear UGITModule.. \n");
            CacheHelper<UGITModule>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear UserProfile.. \n");
            CacheHelper<UserProfile>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear Dashboard.. \n");
            CacheHelper<Dashboard>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear Services.. \n");
            CacheHelper<Services>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear DashboardPanelView.. \n");
            CacheHelper<DashboardPanelView>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear ResourceWorkItems.. \n");
            CacheHelper<ResourceWorkItems>.ClearWithRegion(tenantID);
            cacheDetails.Append($"Starting Cache Clear ResourceAllocation.. \n");
            CacheHelper<RResourceAllocation>.ClearWithRegion(tenantID);
            Util.Log.ULog.WriteLog(UGITUtility.ObjectToString(cacheDetails));
            ClearConfigCache(tenantID);
            ClearProfileCache(tenantID);
            ClearTicketCache(tenantID);
        }

        public static void ClearConfigCache(string tenantID)
        {
            StringBuilder cacheDetails = new StringBuilder();
            cacheDetails.Append($"Starting Cache Clear Field Configuration.. \n");
            CacheHelper<FieldConfiguration>.ClearWithRegion(tenantID);
            Util.Log.ULog.WriteLog(UGITUtility.ObjectToString(cacheDetails));
        }

        public static void ClearProfileCache(string tenantID)
        {
            StringBuilder cacheDetails = new StringBuilder();
            cacheDetails.Append($"Starting Cache Clear UserProfile.. \n");
            CacheHelper<UserProfile>.ClearWithRegion(tenantID);
            Util.Log.ULog.WriteLog(UGITUtility.ObjectToString(cacheDetails));
        }

        public static void ClearTicketCache(string tenantID)
        {
            StringBuilder cacheDetails = new StringBuilder();
            cacheDetails.Append($"Starting Cache Clear TicketCache.. \n");
            CacheHelper<object>.ClearWithRegion(tenantID);
            Util.Log.ULog.WriteLog(UGITUtility.ObjectToString(cacheDetails));
        }

        public static void ReloadAllConfigCache(ApplicationContext _context)
        {
            StringBuilder cacheDetails = new StringBuilder();
            // Reload Resourceworkitem Cache
            cacheDetails.Append($"Resourceworkitem Cache Clear Process Started \n");
            ResourceWorkItemsManager objResourceAvailbility = new ResourceWorkItemsManager(_context);
            objResourceAvailbility.RefreshCache();
            cacheDetails.Append($"Reload Resourceworkitem Cache Completed.. \n");

            cacheDetails.Append($"ResourceAllocation Cache Clear Process Started \n");
            ResourceAllocationManager objmgr = new ResourceAllocationManager(_context);
            objmgr.RefreshCache();
            cacheDetails.Append($"Reload ResourceAllocation Cache Completed.. \n");



            // Reload PageConfiguration Cache
            cacheDetails.Append($"PageConfigurationManager Cache Clear Process Started \n");
            PageConfigurationManager configurationManager = new PageConfigurationManager(_context);
            configurationManager.RefreshCache();
            cacheDetails.Append($"Reload Page Configuration Cache Completed.. \n");

            // Reload Configuration variable Cache
            cacheDetails.Append($"ConfigurationVariableManager Cache Clear Process Started \n");
            ConfigurationVariableManager cVManager = new ConfigurationVariableManager(_context);
            cVManager.RefreshCache();
            cacheDetails.Append($"Reload Configuration Variable cache Completed.. \n");

            // Reload Module Cache
            cacheDetails.Append($"ModuleViewManager Cache Clear Process Started \n");
            ModuleViewManager viewManager = new ModuleViewManager(_context);
            viewManager.RefreshCache();
            cacheDetails.Append($"Reload Module Cache Completed.. \n");

            // Reload Field Configuration Cache
            cacheDetails.Append($"FieldConfigurationManager Cache Clear Process Started \n");
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_context);
            fieldConfigurationManager.RefreshCache();
            cacheDetails.Append($"Reload Field configuration Cache Completed.. \n");

            // Reload Dashboard Cache
            cacheDetails.Append($"DashboardManager Cache Clear Process Started \n");
            DashboardManager dashboardManager = new DashboardManager(_context);
            dashboardManager.RefreshCache();
            cacheDetails.Append($"Reload Dashboard Cache Completed.. \n");

            // Reload Service Cache
            cacheDetails.Append($"ServicesManager Cache Clear Process Started \n");
            ServicesManager servicesManager = new ServicesManager(_context);
            servicesManager.RefreshCache();
            cacheDetails.Append($"Reload Service Cache Completed.. \n");

            // Reload Dashboard Panel View Cache
            cacheDetails.Append($"DashboardPanelViewManager Cache Clear Process Started \n");
            DashboardPanelViewManager dPanelViewManager = new DashboardPanelViewManager(_context);
            dPanelViewManager.RefreshCache();
            cacheDetails.Append($"Reload Dashboard Panel Cache Completed.. \n");
            Util.Log.ULog.WriteLog(UGITUtility.ObjectToString(cacheDetails));
            // Reload Factatble Cache
            cacheDetails.Append($"DashboardManager Cache Clear Process Started \n");
            DashboardFactTableManager ObjDashboard = new DashboardFactTableManager(_context);
            ObjDashboard.RefreshCache();
            //Update cache updated date
            CacheStatisticHelper.UpdateStat(CacheStatisticConstants.CONFIGURATIONCACHEUPDATEON, _context.TenantID, DateTime.UtcNow);
        }

        public static void ReloadTicketsCache(ApplicationContext _context)
        {
            TicketManager ticketManager = new TicketManager(_context);
            ticketManager.RefreshCache();
            //Update cache updated date
            CacheStatisticHelper.UpdateStat(CacheStatisticConstants.TICKETCACHEUPDATEON, _context.TenantID, DateTime.UtcNow);
        }

        public static void ReloadProfileCache(ApplicationContext _context)
        {
            UserProfileManager profileManager = new UserProfileManager(_context);
            profileManager.RefreshCache();
            //Update cache updated date
            CacheStatisticHelper.UpdateStat(CacheStatisticConstants.PROFILECACHEUPDATEON, _context.TenantID, DateTime.UtcNow);
        }

        public static List<CacheStatisticTypeDetail> GetCacheDetail(ApplicationContext context)
        {
            string tenantID = context.TenantID;
            List<CacheStatisticTypeDetail> data = new List<CacheStatisticTypeDetail>();
            var csd = new Dictionary<string, string>();
            CacheStatisticTypeDetail detail = new CacheStatisticTypeDetail();
            detail.CacheType = CacheStatisticConstants.CONFIGURATIONCACHEUPDATEON;
            detail.Title = "Cached Module(s)";
            CacheStatistic typeStat = CacheStatisticHelper.GetStat(CacheStatisticConstants.CONFIGURATIONCACHEUPDATEON, tenantID);
            if (typeStat != null && typeStat.NewValue != null)
                detail.UpdatedOn = Convert.ToDateTime(typeStat.NewValue);
            detail.Detail = new List<Dictionary<string, string>>();
            csd.Add("Total records", CacheHelper<UGITModule>.GetCount(tenantID).ToString());
            detail.Detail.Add(csd);
            data.Add(detail);
            csd = new Dictionary<string, string>();

            detail = new CacheStatisticTypeDetail();
            detail.CacheType = CacheStatisticConstants.PROFILECACHEUPDATEON;
            detail.Title = "User Profile";
            typeStat = CacheStatisticHelper.GetStat(CacheStatisticConstants.PROFILECACHEUPDATEON, tenantID);
            if (typeStat != null && typeStat.NewValue != null)
                detail.UpdatedOn = Convert.ToDateTime(typeStat.NewValue);
            csd.Add("Total records", CacheHelper<UserProfile>.GetCount(tenantID).ToString());
            detail.Detail = new List<Dictionary<string, string>>();
            detail.Detail.Add(csd);
            data.Add(detail);

            detail = new CacheStatisticTypeDetail();
            detail.CacheType = CacheStatisticConstants.TICKETCACHEUPDATEON;
            detail.Title = "Modules";
            typeStat = CacheStatisticHelper.GetStat(CacheStatisticConstants.TICKETCACHEUPDATEON, tenantID);
            if (typeStat != null && typeStat.NewValue != null)
                detail.UpdatedOn = Convert.ToDateTime(typeStat.NewValue);
            detail.Detail = new List<Dictionary<string, string>>();

            ModuleViewManager manager = new ModuleViewManager(context);
            List<UGITModule> modules = manager.LoadAllModule();
            modules = modules.Where(x => x.EnableModule && x.EnableCache && !string.IsNullOrEmpty(x.ModuleTable)).OrderBy(x => x.ModuleName).ToList();
            TicketManager ticketManager = new TicketManager(context);
            foreach (UGITModule m in modules)
            {
                DataTable mOData = CacheHelper<object>.Get($"OpenTicket_{m.ModuleName}", context.TenantID) as DataTable;
                DataTable mCData = CacheHelper<object>.Get($"ClosedTicket_{m.ModuleName}", context.TenantID) as DataTable;

                long openCount = 0;
                long closeCount = 0;
                if (mOData != null)
                    openCount = mOData.Rows.Count;
                if (mCData != null)
                    closeCount = mCData.Rows.Count;

                csd = new Dictionary<string, string>();
                csd.Add("Module", m.ModuleName);
                csd.Add("Open Items", openCount.ToString());
                csd.Add("Closed Items", closeCount.ToString());
                detail.Detail.Add(csd);
            }
            //detail.Detail.Add(new CacheStatisticDetail() { Title = "Enable Users", Value = CacheHelper<UserProfile>.GetCount(tenantID).ToString() });
            data.Add(detail);

            return data;
        }

        public static void TryLoadCacheWhileLogin(ApplicationContext context)
        {
            CacheStatistic configCache = CacheStatisticHelper.GetStat(CacheStatisticConstants.CONFIGURATIONCACHEUPDATEON, context.TenantID);
            CacheStatistic ticketCache = CacheStatisticHelper.GetStat(CacheStatisticConstants.TICKETCACHEUPDATEON, context.TenantID);

            if (configCache == null || ticketCache == null)
            {
                Task.Run(async () =>
                {
                    await Task.FromResult(0);

                    if (configCache == null)
                    {
                        ClearConfigCache(context.TenantID);
                        ReloadAllConfigCache(context);
                    }

                    if (ticketCache == null)
                    {
                        ClearTicketCache(context.TenantID);
                        ReloadTicketsCache(context);
                    }
                });
            }
            ReloadClientProfileStatus(context);
        }
        public static void ReloadCacheTenantWise(ApplicationContext context)
        {
            ClearCache(context.TenantID);
            ReloadAllConfigCache(context);
            ReloadProfileCache(context);
            ReloadTicketsCache(context);
            GetCacheDetail(context);
            ReloadClientProfileStatus(context);
        }
        public static void ReloadClientProfileStatus(ApplicationContext context)
        {
            DataTable clientProfileStatus  = GetTableDataManager.GetTableDataUsingQuery($"ClientProfileStatus @TenantId='{context.TenantID}'");
            CacheHelper<object>.AddOrUpdate($"clientProfileStatus_{context.TenantID}", context.TenantID, clientProfileStatus);
        }
        public static void LoadCacheData()
        {
            try
            {
                ULog.WriteLog("Load Cache Data- LoadCacheData()");
                TenantManager tenantMgr = new TenantManager(ApplicationContext.Create());
                List<Tenant> tList = tenantMgr.GetTenantList();
                tList = tList.Where(x => x.Deleted != true).ToList();
                ApplicationContext context = null;
                foreach (Tenant tenant in tList)
                {
                    context = ApplicationContext.CreateContext(tenant.TenantID.ToString());
                    RegisterCache.ReloadCacheTenantWise(context);
                    ULog.WriteLog("Load Cache Data- " + context.TenantAccountId);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteLog("Failed To Load Cache Data-" + ex.Message);
            }
        }
    }
}