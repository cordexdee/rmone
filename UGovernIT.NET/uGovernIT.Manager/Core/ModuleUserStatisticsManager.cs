using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Web;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public class ModuleUserStatisticsManager : ManagerBase<ModuleUserStatistic>, IModuleUserStatisticsManager
    {
        ModuleUserStatisticsStore mUserStore;

        public ModuleUserStatisticsManager(ApplicationContext context) : base(context)
        {
            mUserStore = new ModuleUserStatisticsStore(this.dbContext);
            store = new ModuleUserStatisticsStore(this.dbContext);
        }
        private static Object obj = new Object();

        public List<ModuleUserStatistic> GetModuleUserStatistics()
        {
            return mUserStore.GetModuleUserStatistics();
        }

        public void ADDUpdateModuleUserStatistics(DataRow currentTicketItem, string moduleName)
        {
            if (currentTicketItem == null)
                return;

            lock (obj)
            {
                UserProfile user = dbContext.CurrentUser;

                // get the current module item detail the ticket belogns from.  // load the modules.
                ModuleViewManager moduleViewManager = new ModuleViewManager(dbContext);
                UGITModule module = moduleViewManager.LoadByName(moduleName); 

                // Get current ticket table name.
                string ticketTableName = module.ModuleTable;
                if (module == null)
                    return;

                // if Event Receivers are not enabled for this module then return
                bool enableEventReceiver = UGITUtility.StringToBoolean(module.EnableEventReceivers);
                if (!enableEventReceiver)
                    return;
                
                DataTable moduleUserStatisticsListTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TicketId}='{currentTicketItem[DatabaseObjects.Columns.TicketId]}' and {DatabaseObjects.Columns.TenantID}='{dbContext.TenantID}'");

                // get the Roles belongs from current ticket's module.
                if (module.List_ModuleUserTypes != null)
                {
                    List<string> userFilteredList = new List<string>();
                    foreach (ModuleUserType userType in module.List_ModuleUserTypes)
                    {
                        userFilteredList.Add(userType.ColumnName);
                    }
                    mUserStore.ADDUpdateModuleUserStatistics(module.ModuleName, moduleUserStatisticsListTable, userFilteredList, currentTicketItem);
                }
            }
        }

        public void RebuildModuleUserStatistics(ApplicationContext context)
        {
            ModuleViewManager mMgr = new ModuleViewManager(context);
            List<UGITModule> modules = mMgr.Load(x => x.EnableModule && x.EnableEventReceivers);
            // Load all modules.
            if (modules == null || modules.Count() == 0)
                return; // nothing to do!
            try
            {
                // load the ModuleUserStatistics table.
                DataTable moduleUserStatisticsList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                TicketManager ticketManager = new TicketManager(context);
                foreach (UGITModule dr in modules)
                {
                    // Only update statistics for modules that have event receivers enabled
                    bool enableEventReceiver = dr.EnableEventReceivers;
                    if (!enableEventReceiver)
                        continue;
                    int moduleId = UGITUtility.StringToInt(dr.ID);
                    // get the Roles belongs from current ticket's module.
                    string prms = string.Format("{0} ='{1}' and {2} ='{3}'",DatabaseObjects.Columns.TenantID, context.TenantID,  DatabaseObjects.Columns.ModuleNameLookup, dr.ModuleName);
                    //DataTable modulesRoles = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserTypes, prms);
                    DataTable ticketList = null;
                    string ticketTable = Convert.ToString(dr.ModuleTable);
                    if (!string.IsNullOrEmpty(ticketTable))
                        ticketList = GetTableDataManager.GetTableData(ticketTable, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' AND {DatabaseObjects.Columns.Closed} != 'True' ");

                    if (ticketList != null && ticketList.Rows.Count >= 1)
                    {
                        //DataTable openTickets = ticketList.Select( ticketManager.GetOpenTickets(dr);
                        //List<string> userFilteredList = new List<string>();
                        //foreach (ModuleUserType userType in dr.List_ModuleUserTypes)
                        //{
                        //    userFilteredList.Add(userType.UserTypes);
                        //}
                        // create statistics for each ticket
                        foreach (DataRow ticketItem in ticketList.Rows)
                        {
                            ADDUpdateModuleUserStatistics(ticketItem, dr.ModuleName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "RebuildModuleUserStatistics failed");
            }
            //uGITCache.ReloadModuleUserStatistics();
            return;
        }

        public void RefreshCache()
        {
            mUserStore.RefreshCache();
        }
    }
    public interface IModuleUserStatisticsManager : IManagerBase<ModuleUserStatistic>
    {
        void ADDUpdateModuleUserStatistics(DataRow currentTicketItem, string moduleName);
        List<ModuleUserStatistic> GetModuleUserStatistics();
    }
}
