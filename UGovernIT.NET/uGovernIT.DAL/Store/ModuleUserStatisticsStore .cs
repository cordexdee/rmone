using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using uGovernIT.Utility;
using System.Data.SqlClient;
using System.Linq.Expressions;
using uGovernIT.DAL;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace uGovernIT.DAL.Store
{
    public class ModuleUserStatisticsStore : StoreBase<ModuleUserStatistic>, IModuleUserStatisticsStore
    {
        public ModuleUserStatisticsStore(CustomDbContext context) : base(context)
        {
        }
   

        public List<ModuleUserStatistic> GetModuleUserStatistics()
        {
            List<ModuleUserStatistic> listModuleUserStatistics = (List<ModuleUserStatistic>)CacheHelper<object>.Get(CacheKey.USERSTATISTICS, context.TenantID);
            if (listModuleUserStatistics == null)
            {
                listModuleUserStatistics = this.Load();
                if (listModuleUserStatistics != null)
                {
                    CacheHelper<object>.AddOrUpdate(CacheKey.USERSTATISTICS, context.TenantID, listModuleUserStatistics);
                }
            }
            return listModuleUserStatistics;
        }

        public void AddUpdateCache(ModuleUserStatistic obj)
        {
            List<ModuleUserStatistic> listModuleUserStatistics = (List<ModuleUserStatistic>)CacheHelper<object>.Get(CacheKey.USERSTATISTICS, context.TenantID);
            if (listModuleUserStatistics != null)
            {
                if (obj.ID > 0)
                {
                    ModuleUserStatistic muStats = listModuleUserStatistics.Find(x => x.ID == obj.ID);
                    if (muStats != null)
                    {
                        muStats = obj;
                    }
                    else
                    {
                        listModuleUserStatistics.Add(obj);
                        CacheHelper<object>.AddOrUpdate(CacheKey.USERSTATISTICS, context.TenantID, listModuleUserStatistics);
                    }
                }
            }
        }
        public  void DeleteObjectFromCache(ModuleUserStatistic obj)
        {
            List<ModuleUserStatistic> listModuleUserStatistics = (List<ModuleUserStatistic>)CacheHelper<object>.Get(CacheKey.USERSTATISTICS, context.TenantID);
            if (listModuleUserStatistics != null)
            {
                if (obj.ID > 0)
                {
                    ModuleUserStatistic muStats = listModuleUserStatistics.Single(x => x.ID == obj.ID);
                    if (muStats!=null)
                    {
                        listModuleUserStatistics.Remove(muStats);
                    }
                }
            }
        }

        public void RefreshCache()
        {
            ThreadStart threadStart = delegate () {
                CacheHelper<object>.AddOrUpdate($"ModuleUserStatistics{context.TenantID}", context.TenantID, GetDataTable($" where {DatabaseObjects.Columns.TenantID}='{context.TenantID}'"));
            };
            Thread thread = new Thread(threadStart);
            thread.IsBackground = true;
            thread.Start();
        }

        public void ADDUpdateModuleUserStatistics(string moduleName, DataTable moduleUserStatisticsList, List<string> modulesRoles, DataRow currentTicketItem)
        {
            try
            {
                // get the current ticket close status
                bool ticketClosed = UGITUtility.StringToBoolean(currentTicketItem[DatabaseObjects.Columns.Closed]);

                // get the current ticket rows from ModuleUserStatistics table.
                DataRow[] myModuleUserStatisticsRows = moduleUserStatisticsList.Select();

                //Delete User Statistic first against specified ticket
                List<ModuleUserStatistic> moduleUserStatisticsCollection = new List<ModuleUserStatistic>();
                foreach (DataRow row in myModuleUserStatisticsRows)
                {
                    ModuleUserStatistic objModuleUserStatistics = new ModuleUserStatistic();
                    objModuleUserStatistics.ID = Convert.ToInt32(row[DatabaseObjects.Columns.ID]);
                    objModuleUserStatistics.IsActionUser = UGITUtility.StringToBoolean(Convert.ToString(row[DatabaseObjects.Columns.IsActionUser]));
                    objModuleUserStatistics.ModuleNameLookup = Convert.ToString(row[DatabaseObjects.Columns.ModuleNameLookup]);
                    objModuleUserStatistics.TicketId = Convert.ToString(row[DatabaseObjects.Columns.TicketId]);
                    objModuleUserStatistics.UserName = Convert.ToString(row[DatabaseObjects.Columns.TicketUser]);
                    moduleUserStatisticsCollection.Add(objModuleUserStatistics);
                    this.Delete(objModuleUserStatistics);
                }

                // if the ticket is not in the last stage then update statistics else delete it from statistics table.
                if (!ticketClosed)
                {
                    // Check for each role & add/update ModuleUserStatistics.
                    string[] actionUserTypes = UGITUtility.SplitString(Convert.ToString(currentTicketItem[DatabaseObjects.Columns.TicketStageActionUserTypes]), Constants.Separator);
                    foreach (string role in modulesRoles)
                    {
                        var UserRole = role;
                        if (role == DatabaseObjects.Columns.Owner)
                        {
                            if (UGITUtility.IsSPItemExist(currentTicketItem, role))
                                UserRole = role;
                            else if (UGITUtility.IsSPItemExist(currentTicketItem, DatabaseObjects.Columns.OwnerUser))
                                UserRole = DatabaseObjects.Columns.OwnerUser;
                        }

                        // Get all the users assigned to this role on the ticket
                        if (UGITUtility.IsSPItemExist(currentTicketItem, UserRole))
                        {
                            string usersAssignedOnRoles = Convert.ToString(currentTicketItem[UserRole]);
                            string[] userCollection = UGITUtility.SplitString(usersAssignedOnRoles, Constants.Separator6);
                            for (int i = 0; i < userCollection.Count(); i++)
                            {
                                ////Create Statistic for ticket
                                ModuleUserStatistic moduleUserStatisticsItem = new ModuleUserStatistic();
                                moduleUserStatisticsItem.TicketId = Convert.ToString(currentTicketItem[DatabaseObjects.Columns.TicketId]);
                                moduleUserStatisticsItem.ModuleNameLookup = moduleName;
                                moduleUserStatisticsItem.UserRole = role;
                                moduleUserStatisticsItem.UserName = Convert.ToString(userCollection[i]);
                                if (actionUserTypes.Contains(Convert.ToString(role)))
                                    moduleUserStatisticsItem.IsActionUser = true;
                                else
                                    moduleUserStatisticsItem.IsActionUser = false;
                               
                                this.Insert(moduleUserStatisticsItem);
                                AddUpdateCache(moduleUserStatisticsItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        public static void RebuildModuleUserStatistics()
        {
            //SPWeb spWeb = SPContext.Current.Web;

            //// Delete whole list
            //SPListHelper.DeleteAllSPListItems(spWeb, DatabaseObjects.Lists.ModuleUserStatistics);

            //// Load all modules.
            //DataTable modules = uGITCache.GetModuleList(ModuleType.All);
            //if (modules == null || modules.Rows.Count == 0)
            //    return; // nothing to do!

            //try
            //{
            //    // load the ModuleUserStatistics table.
            //    SPList moduleUserStatisticsList = spWeb.Lists[DatabaseObjects.Lists.ModuleUserStatistics];

            //    foreach (DataRow dr in modules.Rows)
            //    {
            //        // Only update statistics for modules that have event receivers enabled
            //        bool enableEventReceiver = uHelper.StringToBoolean(dr[DatabaseObjects.Columns.EnableEventReceivers]);
            //        if (!enableEventReceiver)
            //            continue;

            //        // get the module detail.
            //        DataTable moduleDetail = modules.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, Convert.ToInt32(dr[DatabaseObjects.Columns.Id]))).CopyToDataTable();
            //        if (moduleDetail == null || moduleDetail.Rows.Count == 0)
            //            continue;

            //        // ID of module
            //        int moduleId = int.Parse(Convert.ToString(moduleDetail.Rows[0][DatabaseObjects.Columns.Id]));

            //        // get the Roles belongs from current ticket's module.
            //        SPList modulesRoleList = spWeb.Lists[DatabaseObjects.Lists.ModuleUserTypes];
            //        SPQuery query1 = new SPQuery();
            //        query1.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ModuleNameLookup, moduleId);
            //        SPListItemCollection modulesRoles = modulesRoleList.GetItems(query1);

            //        //load the all the tickets of this module.
            //        // gets the ticket table name of the module.
            //        SPList ticketList = null;
            //        string ticketTable = Convert.ToString(moduleDetail.Rows[0][DatabaseObjects.Columns.ModuleTicketTable]);
            //        if (!string.IsNullOrEmpty(ticketTable))
            //            ticketList = spWeb.Lists[ticketTable];

            //        if (ticketList != null && ticketList.ItemCount >= 1)
            //        {
            //            // get all open tickets.
            //            SPQuery openTicketQuery = new SPQuery();
            //            if (ticketList.Fields.ContainsField(DatabaseObjects.Columns.TicketClosed))
            //                openTicketQuery.Query = string.Format("<Where><Neq><FieldRef Name='{0}' /><Value Type='Boolean'>1</Value></Neq></Where>",
            //                                                            DatabaseObjects.Columns.TicketClosed);
            //            else
            //                openTicketQuery.Query = "<Where></Where>";

            //            SPListItemCollection openTickets = ticketList.GetItems(openTicketQuery);

            //            // create statistics for each ticket
            //            foreach (SPListItem ticketItem in openTickets)
            //            {
            //                ADDUpdateModuleUserStatistics(spWeb, moduleId, moduleUserStatisticsList, modulesRoles, ticketItem);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteException(ex, "RebuildModuleUserStatistics failed");
            //}

            //uGITCache.ReloadModuleUserStatistics();

            //return;
        }
    }
    public interface IModuleUserStatisticsStore : IStore<ModuleUserStatistic>
    {
        List<ModuleUserStatistic> GetModuleUserStatistics();
        void AddUpdateCache(ModuleUserStatistic obj);
        void DeleteObjectFromCache(ModuleUserStatistic obj);
        void RefreshCache();
    }
}