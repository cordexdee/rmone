using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Data;
using System.IO;
using uGovernIT.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Cache;


namespace uGovernIT.Web
{
    public partial class ConfigCache : UserControl
    {
        protected ApplicationContext _context; 
        protected void Page_Load(object sender, EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();
            if (!IsPostBack)
            {
                CacheDetail();
            }
        }
        protected void BtClearCache_Click(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Starting Clear Cache");
            RegisterCache.ClearCache(_context.TenantID);
            string message = "";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallNotify", "window.onload = function() { NotifyUser('" + message + "'); };", true);
        }

        /// <summary>
        /// This method is used to Refresh Configuration Cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefreshConfigCache_Click(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Starting Refresh of Configuration Cache");
            //Clear & reload all config cache
            RegisterCache.ClearConfigCache(_context.TenantID);
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Refresh of Configuration Cache Complete!\n Reloading  Configuration Cache");
            RegisterCache.ReloadAllConfigCache(_context);
            string message = "Configuration";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallNotify", "window.onload = function() { NotifyUser('" + message + "'); };", true);
        }

     
        /// <summary>
        /// This method is used to refresh ticket cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefreshTicketsCache_Click(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Starting Refresh of Ticket Cache");
            RegisterCache.ClearTicketCache(_context.TenantID);
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Refresh of Ticket Cache Complete!\n Reloading Ticket Cache");
            RegisterCache.ReloadTicketsCache(_context);
            RegisterCache.ReloadProfileCache(_context);

            string message = "Ticket";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallNotify", "window.onload = function() { NotifyUser('" + message + "'); };", true);
        }


        /// <summary>
        /// This method is used to refresh profile cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefreshProfileCache_Click(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Starting Refresh of Profile Cache");
            RegisterCache.ClearProfileCache(_context.TenantID);
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Refresh of Profile Cache Complete!\n Reloading Profile Cache");
            RegisterCache.ReloadProfileCache(_context);
            string message = "Profile";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallNotify", "window.onload = function() { NotifyUser('" + message + "'); };", true);
        }


        /// <summary>
        /// This method is uesed to refresh entire cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefreshEntireCache_Click(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Starting Refresh of Entire Cache");
            RegisterCache.ClearCache(_context.TenantID);
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Refresh of Profile, Config, Tickets Complete!\n Reloading Profile, Config, Tickets  Cache");
            RegisterCache.ReloadAllConfigCache(_context);
            RegisterCache.ReloadProfileCache(_context);
            RegisterCache.ReloadTicketsCache(_context);
            string message = "Entire";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallNotify", "window.onload = function() { NotifyUser('" + message + "'); };", true);
        }

        protected void btnRebuildStatistics_Click(object sender, EventArgs e)
        {
            Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: rebuilding statistics");
            ModuleUserStatisticsManager statisticsManager = new ModuleUserStatisticsManager(_context);
            statisticsManager.RebuildModuleUserStatistics(_context);
            statisticsManager.RefreshCache();
            //CacheDetail();
        }

        protected void BtRebuildDashboardSummary_Click(object sender, EventArgs e)
        {
            //Log.AuditTrail(SPContext.Current.Web.CurrentUser, "rebuilding dashboard summary", Request.Url);
            //PopulateDashboard popDashboard = new PopulateDashboard(SPContext.Current.Web);
            //popDashboard.PopulateDashboardFromModules();
            //DashboardCache.RefreshDashboardCache(DatabaseObjects.Lists.DashboardSummary, SPContext.Current.Web);
            //CacheDetail();
        }

        private void CacheDetail()
        {
            //DataTable dd = new DataTable();
            //StringBuilder cacheDetail = new StringBuilder();

            //cacheDetail.Append("<div class='cache-item-container'><b class='cache-module-label'>Last Updated:</b></div>");

            //cacheDetail.AppendFormat("<div class='cache-item-container'><span class='cache-item-label'>Configuration:</span><span class='fleft'>{0}</span></div>",
            //                           uGITCache.GetuGITCacheDataInstance().lastConfigUpdate == DateTime.MinValue ? "n/a" : uGITCache.GetuGITCacheDataInstance().lastConfigUpdate.ToString());
            //cacheDetail.AppendFormat("<div class='cache-item-container'><span class='cache-item-label'>Statistics:</span><span class='fleft'>{0}</span></div>",
            //                            uGITCache.GetuGITCacheDataInstance().lastCacheUpdate == DateTime.MinValue ? "n/a" : uGITCache.GetuGITCacheDataInstance().lastCacheUpdate.ToString());
            //cacheDetail.AppendFormat("<div class='cache-item-container'><span class='cache-item-label'>SMS:</span><span class='fleft'>{0}</span></div>",
            //                            uGITCache.ModuleDataCache.GetModuleDataCacheInstance().lastCacheUpdate == DateTime.MinValue ? "n/a" : uGITCache.ModuleDataCache.GetModuleDataCacheInstance().lastCacheUpdate.ToString());

            //cacheDetail.Append("<div class='cache-item-container'><b class='cache-module-label'>Common Data:</b></div>");

            //string topTableDetailMsg = "<div class='cache-item-container'><span class='cache-item-label'>{2}:</span><span class='fleft'>{0:#,0} Rows ({1:#,0} KB)</span></div>";

            //double memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.ConfigurationVariable);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.ConfigurationVariable), memoryConsumed, "Configuration Variables");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.Modules);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.Modules), memoryConsumed, "Modules");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.ModuleColumns);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.ModuleColumns), memoryConsumed, "Module Columns");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.ModuleStages);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.ModuleStages), memoryConsumed, "Module Stages");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.ModuleUserTypes);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.ModuleUserTypes), memoryConsumed, "Module User Types");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.ModuleFormTab);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.ModuleFormTab), memoryConsumed, "Module Form Tabs");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.RequestRoleWriteAccess);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.RequestRoleWriteAccess), memoryConsumed, "Request Role Write Access");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.FormLayout);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.FormLayout), memoryConsumed, "Form Layout");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.RequestType);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.RequestType), memoryConsumed, "Request Types");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.TaskEmails);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.TaskEmails), memoryConsumed, "Task Emails");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.RequestPriority);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.RequestPriority), memoryConsumed, "Request Priority (Mapping)");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.StageType);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.StageType), memoryConsumed, "Stage Types");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.TicketPriority);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.TicketPriority), memoryConsumed, "Ticket Priority");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.TicketSeverity);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.TicketSeverity), memoryConsumed, "Ticket Severity");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.TicketImpact);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.TicketImpact), memoryConsumed, "Ticket Impact");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.TicketStatusMapping);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.TicketStatusMapping), memoryConsumed, "Ticket Status Mapping");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.SLARule);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.SLARule), memoryConsumed, "SLA Rules");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.GenericTicketStatus);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.GenericTicketStatus), memoryConsumed, "Generic Ticket Status");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.DashboardPanels);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.DashboardPanels), memoryConsumed, "Dashboard Panels");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.ModuleUserStatistics);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.ModuleUserStatistics), memoryConsumed, "Module User Statistics");

            //cacheDetail.AppendFormat("<div class='cache-item-container'><b class='cache-module-label'>Module: EDM</b></div>");
            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.DocumentInfoList);

            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.DocumentInfoList), memoryConsumed, "Document Information");
            //UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "EDM");
            //List<ModuleColumn> moduleColumnsList = moduleObj.List_ModuleColumns;
            //if (moduleColumnsList.Count > 0)
            //{
            //    moduleColumnsList = moduleColumnsList.Where(x => x.CustomProperties != null && x.CustomProperties.Contains("ShowInViewDropdDown")).ToList();
            //    if (moduleColumnsList != null && moduleColumnsList.Count > 0)
            //    {
            //        foreach (ModuleColumn mc in moduleColumnsList)
            //        {
            //            string colName = mc.FieldName;
            //            string listName = ConfigurationVariable.GetValue(Convert.ToString(mc.FieldName));
            //            if (!string.IsNullOrEmpty(listName))
            //            {
            //                memoryConsumed = uGITCache.GetCacheSize(listName);
            //                cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(listName), memoryConsumed, mc.FieldDisplayName);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    moduleColumnsList = null;
            //}

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.DocumentType);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.DocumentType), memoryConsumed, "Document Types");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.DocTypeInfo);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.DocTypeInfo), memoryConsumed, "Document Categories");

            //memoryConsumed = uGITCache.GetCacheSize(DatabaseObjects.Lists.PortalInfo);
            //cacheDetail.AppendFormat(topTableDetailMsg, uGITCache.GetDataCount(DatabaseObjects.Lists.PortalInfo), memoryConsumed, "Portal Information");

            //DataTable modules = uGITCache.GetModuleList(ModuleType.All); // SPList modules = SPListHelper.GetSPList(DatabaseObjects.Lists.Modules);
            //string moduleMsg = "<div class='cache-item-container'><span class='cache-item-label'>{2}:</span><span class='fleft'>{0:#,0} ({1:#,0} KB)</span></div>";
            //foreach (DataRow mRow in modules.Rows)
            //{
            //    string moduleName = string.Empty;
            //    try
            //    {
            //        // Only show data for enables modules
            //        if (!uHelper.StringToBoolean(mRow[DatabaseObjects.Columns.EnableModule]) ||
            //            !uHelper.StringToBoolean(mRow[DatabaseObjects.Columns.EnableCache]) ||
            //            mRow[DatabaseObjects.Columns.ModuleType] == null)
            //            continue;

            //        ModuleType mType = (ModuleType)Enum.Parse(typeof(ModuleType), Convert.ToString(mRow[DatabaseObjects.Columns.ModuleType]));
            //        string nType = "Tickets";
            //        if (moduleName == "CMDB")
            //            nType = "Assets";

            //        if (mType == ModuleType.SMS || moduleName == "CMDB")
            //        {
            //            int moduleId = Convert.ToInt32(mRow[DatabaseObjects.Columns.Id]);
            //            moduleName = Convert.ToString(mRow[DatabaseObjects.Columns.ModuleName]);

            //            cacheDetail.AppendFormat("<div class='cache-item-container'><b class='cache-module-label'>Module: {0}</b></div>", moduleName);
            //            memoryConsumed = uGITCache.ModuleDataCache.GetOpenTicketsSize(moduleId);
            //            cacheDetail.AppendFormat(moduleMsg, uGITCache.ModuleDataCache.GetOpenTicketsCount(moduleId), memoryConsumed, "Total Open " + nType);

            //            memoryConsumed = uGITCache.ModuleDataCache.GetCloseTicketsSize(moduleId);
            //            cacheDetail.AppendFormat(moduleMsg, uGITCache.ModuleDataCache.GetClosedTicketsCount(moduleId), memoryConsumed, "Total Closed " + nType);
            //        }
            //    }
            //    catch
            //    {
            //        cacheDetail.AppendFormat("Unable get cache detail of {0}", moduleName);
            //        Log.WriteLog(cacheDetail.ToString());
            //    }
            //}

            //cacheDetail.AppendFormat("<div class='cache-item-container'><b class='cache-module-label'>Module: {0}</b></div>", "TSK");
            //memoryConsumed = TaskCache.GetOpenedTaskSize();
            //cacheDetail.AppendFormat(moduleMsg, TaskCache.GetOpenedTasksCount(), memoryConsumed, "Total Active Tasks");

            //memoryConsumed = TaskCache.GetCompletedTaskSize();
            //cacheDetail.AppendFormat(moduleMsg, TaskCache.GetCompletedTasksCount(), memoryConsumed, "Total Completed Tasks");

            //double totalSize = uGITCache.GetTotalCacheSize();
            //cacheDetail.AppendFormat("<div class='cache-item-container' style='font-weight:bold;padding-top:5px;'>Total Cache Size: {0:#,0} KB</div>", totalSize);

            //cacheDetailPanel.Controls.Clear();
            //cacheDetailPanel.Controls.Add(new LiteralControl(cacheDetail.ToString()));
        }

        protected void BtnRearrangeTasks_Click(object sender, EventArgs e)
        {
            //Log.AuditTrail(SPContext.Current.Web.CurrentUser, "reorganized project tasks", Request.Url);
            //UGITTaskHelper.RearrangeAllTasks(SPContext.Current.Web);
            //TaskCache.ReloadAllTasks();
            //CacheDetail();
        }

        protected void btnUpdateWorkflowHistory_Click(object sender, EventArgs e)
        {
            //Log.AuditTrail(SPContext.Current.Web.CurrentUser, "recalculating module workflow history", Request.Url);
            //WorkflowHistoryHelper wHelper = new WorkflowHistoryHelper(SPContext.Current.Web);
            //wHelper.UpdateDuration();
            ////wHelper.UpdateDuration(Request["ticketID"]);

            //CacheDetail();
        }

        protected void btnRefreshTasksCache_Click(object sender, EventArgs e)
        {
            //Log.AuditTrail(SPContext.Current.Web.CurrentUser, "refresh tasks cache", Request.Url);
            //TaskCache.ReloadAllTasks();
            //CacheDetail();
        }

        protected void btnRefreshDocumentsCache_Click(object sender, EventArgs e)
        {
            //Log.AuditTrail(SPContext.Current.Web.CurrentUser, "refresh document cache", Request.Url);
            //DMCache.RefreshDocumentDataCache();
            //CacheDetail();
        }
    }
}
