using DevExpress.Office.Utils;
using DevExpress.Xpo.DB.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Managers
{
    public interface ITicketManager : ITicketStore
    {
        //Added the viewfield to return only specific columns 
        //Anand
        DataTable GetAllTickets(UGITModule module, List<string> viewFields = null);
        DataTable GetTableSchemaDetail(string tableName, string moduleName);
        DataTable GetColumnDetail(string tableName, string moduleName);
    }

    public class TicketManager : TicketStore, ITicketManager
    {
        private ApplicationContext applicationContext;
        private TicketStore ticketStore;

        public TicketManager(ApplicationContext context) : base(context)
        {
            applicationContext = context;
            ticketStore = new TicketStore(context);
        }
        //This method finds the FieldName in ModuleColumns and update module cache
        public void ApplyChangesToTicketCache(string FieldName)
        {
            UGITModule module = null;
            ModuleViewManager moduleViewManager = new ModuleViewManager(applicationContext);
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(applicationContext);
            List<ModuleColumn> moduleColumns  = moduleColumnManager.Load(x => x.FieldName == FieldName);
            foreach (ModuleColumn col in moduleColumns)
            {
                module = moduleViewManager.LoadByName(col.CategoryName, true);
                if (module != null)
                    UpdateTicketCache(module);
                module = null;
            }
        }

        public DataTable GetTicketTableBasedOnTicketId(string moduleName, string ticketId)
        {
            var where = string.Empty;
            
            if (!string.IsNullOrEmpty(ticketId))
                where = "TicketId = '" + ticketId + "'";

            var moduleViewManager = new ModuleViewManager(applicationContext);
            var module = moduleViewManager.LoadByName(moduleName);
            DataRow[] ticketCollection = this.GetAllTickets(module).Select(where);
            if (ticketCollection != null && ticketCollection.Count() > 0)
            {
                return this.GetAllTickets(module).Select(where).CopyToDataTable();
            }
            else
            {
                return new DataTable();
            }

        }
        public DataRow GetByTicketIdFromCache(string pModuleName, string pTicketId)
        {
            DataRow dtResult = null;
            TicketManager ticketMGR = new TicketManager(this.applicationContext);
            ModuleViewManager moduleMGR = new ModuleViewManager(this.applicationContext);
            UGITModule companyModule = moduleMGR.LoadByName(pModuleName);
            DataTable dtAllTickets = ticketMGR.GetAllTickets(companyModule);
            DataRow[] _ticketrows = dtAllTickets.Select($"TicketId = '{pTicketId}'");
            if (_ticketrows != null && _ticketrows.Count() > 0)
            {
                return _ticketrows[0];
            }
            return dtResult;
        }
        public DataTable GetUnassignedTickets(string moduleName)
        {
            var moduleViewManager = new ModuleViewManager(applicationContext);

            var module = moduleViewManager.LoadByName(moduleName,true);
            var openTickets = this.GetOpenTickets(module);

            if (openTickets == null)
                return new DataTable();

            DataTable unAssignedTickets = openTickets.Copy(); //openTickets.Clone();

            if (moduleName == ModuleNames.PMM)
                return unAssignedTickets;

            //Get the module stage sequence for Assigned ticket.
            if (module.List_LifeCycles.Count > 0 && module.List_LifeCycles != null)
            {
                LifeCycleStage assignedStage = module.List_LifeCycles.First(x => x.ID == 0).Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Assigned.ToString());

                if (assignedStage != null &&
                    openTickets.Columns.Contains(DatabaseObjects.Columns.StageStep))
                {
                    if (moduleName != "CMDB")
                    {
                        DataRow[] selectedRows = openTickets.Select($"{DatabaseObjects.Columns.StageStep} < {assignedStage.StageStep}"); //string.Format("{0} < {1}", DatabaseObjects.Columns.StageStep, assignedStage.StageStep)
                        if (selectedRows.Length > 0)
                        {
                            unAssignedTickets = selectedRows.CopyToDataTable();
                        }
                        else
                        {
                            unAssignedTickets.Clear();
                        }
                    }
                }
            }

            return unAssignedTickets;
        }

        /*
        public DataTable GetAllTickets(UGITModule module)
        {
            DataTable allTickets = null;
            var tempTable = ticketStore.GetOpenTickets(module);

            if (tempTable != null)
                allTickets = tempTable.Copy();

            var closedTickets = ticketStore.GetClosedTickets(module);

            if (closedTickets != null)
            {
                if (allTickets == null)
                    allTickets = closedTickets.Copy();
                else
                {
                    var drNew = allTickets.NewRow();

                    foreach (DataRow drClose in closedTickets.Rows)
                    {
                        foreach (DataColumn dc in closedTickets.Columns)
                        {
                            allTickets.Merge(closedTickets);

                            if (allTickets.Columns.Contains(dc.ColumnName))
                                drNew[dc.ColumnName] = drClose[dc.ColumnName];
                        }
                    }
                }

            }

            return allTickets;
        }
        */

        //Added the viewfield to return only specific columns 
        //Anand
        public DataTable GetAllTickets(UGITModule module, List<string> viewFields = null)
        {
            DataTable allTickets = null;

            DataTable tempTable = GetOpenTickets(module, null, viewFields);
            if (tempTable != null)
                allTickets = tempTable.Copy();


            DataTable closedTickets = GetClosedTickets(module, viewFields);
            if (closedTickets != null)
            {
                if (allTickets == null)
                    allTickets = closedTickets.Copy();
                else
                    allTickets.Merge(closedTickets);
            }
            return allTickets;
        }

        public DataTable GetTableSchemaDetail(string tableName, string moduleName)
        {
            var where = "select * from " + tableName + " (nolock)";

            if (!string.IsNullOrEmpty(moduleName))
                where += " Where TABLE_NAME='" + moduleName + "'";
            if(string.IsNullOrEmpty(moduleName))
                where= "select top(0) * from " + tableName + " (nolock)";
            return ticketStore.GetTickets(where);
        }

        public DataTable GetColumnDetail(string tableName, string moduleName)
        {
            var where = "select COLUMN_NAME from " + tableName;

            if (!string.IsNullOrEmpty(moduleName))
                where += " Where TABLE_NAME='" + moduleName + "'";

            return ticketStore.GetTickets(where);
        }

        public void RefreshCache()
        {
            ModuleViewManager viewManager = new ModuleViewManager(applicationContext);
            List<UGITModule> lstModules = viewManager.LoadAllModule();
            if (lstModules == null || lstModules.Count == 0)
                return;
            lstModules = lstModules.Where(x => x.EnableModule && x.EnableCache && !string.IsNullOrEmpty(x.ModuleTable)).ToList();
            foreach (UGITModule module in lstModules)
            {
                // Below two functions get ticket from cache and if not available then these functions get tickets from DB and add them to Cache
                GetOpenTickets(module);
                GetClosedTickets(module);
                GetAllTicketsByModuleName(module, applicationContext);
            }
            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(applicationContext);
            DataTable modulecolumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID} = '{applicationContext.TenantID}'");
            CacheHelper<object>.AddOrUpdate($"ModuleColumns{applicationContext.TenantID}", applicationContext.TenantID, modulecolumns);
            CacheHelper<object>.AddOrUpdate($"ModuleColumnslistview{applicationContext.TenantID}", applicationContext.TenantID, moduleColumnManager.Load());
            DataTable ModuleUserStatistics = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{applicationContext.TenantID}'");
            CacheHelper<object>.AddOrUpdate($"ModuleUserStatistics{applicationContext.TenantID}", applicationContext.TenantID, ModuleUserStatistics);

        }

        public DataTable GetAllTicketsByModuleName(string module)
        {
            ModuleViewManager moduleManager = new ModuleViewManager(applicationContext);
            UGITModule moduleObj = moduleManager.LoadByName(module);
            if (moduleObj != null)
                return GetAllTicketsByModuleName(moduleObj, applicationContext);
            else
                return null;
        }

        public DataTable GetAllTicketsByModuleName(UGITModule module, ApplicationContext context)
        {
            DataTable allTickets = null;
            string AllTicketscache = $"AllTicket_{module.ModuleName}";
            if (CacheHelper<object>.IsExists(AllTicketscache, context.TenantID))
            {
                allTickets = CacheHelper<object>.Get(AllTicketscache, context.TenantID) as DataTable;
            }
            else
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                values.Add("@ModuleName", module.ModuleName);
                allTickets = uGITDAL.ExecuteDataSetWithParameters("USP_GetModuleTableData", values);
                CacheHelper<object>.AddOrUpdate(AllTicketscache, context.TenantID, allTickets);
            }

            //DataTable OpenTickets = null;
            //DataTable closedTickets = null;
            //string AllTicketscache = $"AllTicket_{module.ModuleName}";
            //string OpenTicketcache = $"OpenTicket_{module.ModuleName}";
            //string ClosedTicketcache = $"ClosedTicket_{module.ModuleName}";
            //if (CacheHelper<object>.IsExists(AllTicketscache, context.TenantID))
            //{
            //    allTickets = CacheHelper<object>.Get(AllTicketscache, context.TenantID) as DataTable;
            //}
            //if (allTickets == null && !string.IsNullOrEmpty(module.ModuleTable))
            //{
            //    if (CacheHelper<object>.IsExists(OpenTicketcache, context.TenantID) && CacheHelper<object>.IsExists(ClosedTicketcache, context.TenantID))
            //    {
            //        OpenTickets = (DataTable)CacheHelper<object>.Get($"OpenTicket_{module.ModuleName}", context.TenantID);
            //        closedTickets = (DataTable)CacheHelper<object>.Get($"ClosedTicket_{module.ModuleName}", context.TenantID);
            //        if (closedTickets != null && OpenTickets != null)
            //        {
            //            allTickets = OpenTickets;
            //            allTickets.Merge(closedTickets);
            //        }
            //    }
            //}
            //else
            //{
            //Dictionary<string, object> values = new Dictionary<string, object>();
            //values.Add("@TenantID", context.TenantID);
            //values.Add("@ModuleName", module.ModuleName);
            //allTickets = uGITDAL.ExecuteDataSetWithParameters("USP_GetModuleTableData", values);
            //CacheHelper<object>.AddOrUpdate(AllTicketscache, context.TenantID, allTickets);
            //}

            return allTickets;
        }

        public DataTable GetAllOpenTicketsBasedOnModuleName(string moduleName)
        {
            DataTable dtResult = new DataTable();
            ModuleViewManager moduleManagerObj = new ModuleViewManager(applicationContext);
            UGITModule module = moduleManagerObj.LoadByName(moduleName);
            dtResult = GetOpenTickets(module);
            return dtResult;
        }

        public void RefreshCacheModuleWise(UGITModule module)
        {
            //CacheHelper<object>.ClearWithRegion(applicationContext.TenantID);
            CacheHelper<object>.Delete($"OpenTicket_{module.ModuleName}", applicationContext.TenantID);
            CacheHelper<object>.Delete($"ClosedTicket_{module.ModuleName}", applicationContext.TenantID);
            CacheHelper<object>.Delete($"AllTicket_{module.ModuleName}", applicationContext.TenantID);
            GetOpenTickets(module);
            GetClosedTickets(module);
            GetAllTicketsByModuleName(module, applicationContext);
        }
        public static DataRow GetCurrentTicketbyId(ApplicationContext context, string TicketId, string moduleName)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            string moduletable = moduleViewManager.GetModuleTableName(moduleName);
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            values.Add("@IsClosed", "");
            values.Add("@TicketId", TicketId);
            DataTable collection = uGITDAL.ExecuteDataSetWithParameters($"usp_Get{moduletable}", values);

            if (collection != null && collection.Rows.Count > 0)
                return collection.Select()[0];
            else
                return null;
        }

        public DataRow GetDataRowByTicketId(string TicketId)
        {
            DataRow dtResult = null;
            try
            {
                string moduleName = uHelper.getModuleNameByTicketId(TicketId);
                ModuleViewManager moduleViewManager = new ModuleViewManager(applicationContext);
                UGITModule module = moduleViewManager.LoadByName(moduleName);

                dtResult = GetByTicketID(module, TicketId);
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return dtResult;
        }

        public DataTable GetAllProjectTickets(bool includeClosed = true)
        {
            DataTable dtResult = null;
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@tenantID", applicationContext.TenantID);
                values.Add("@includeClosed", includeClosed);
                if (dtResult == null || dtResult.Rows.Count == 0)
                    dtResult = GetTableDataManager.GetData("GetAllProjectTickets ", values);
                else
                    return dtResult;
                return dtResult;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return dtResult;
        }

        //public DataTable GetRefreshedProjectData(string moduleName)
        //{
        //    DataTable dtResult = null;
        //    try
        //    {
        //        Dictionary<string, object> values = new Dictionary<string, object>();
        //        values.Add("@tenantID", applicationContext.TenantID);
        //        values.Add("@mode", "View");
        //        values.Add("@module", moduleName);
        //        dtResult = GetTableDataManager.GetDataFromSP("usp_fillPageSummaryData_31Aug2023_Working", values);

        //        return dtResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        ULog.WriteException(ex);
        //    }
        //    return dtResult;
        //}


        public DataTable GetProjectDetailsByCurrentUser()
        {
            DataTable dtResult = null;
            try
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantId", applicationContext.TenantID);
                values.Add("@UserId", applicationContext.CurrentUser.Id);
                dtResult = GetTableDataManager.GetData("UserAllProjectDetails", values);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return dtResult;
        }
        //public DataTable UpdateRefreshedProjectData(DataTable dt, string moduleName)
        //{
        //    DataTable dtResult = null;
        //    try
        //    {
        //        Dictionary<string, object> values = new Dictionary<string, object>();
        //        values.Add("@tenantID", applicationContext.TenantID);
        //        values.Add("@mode", "Update");
        //        values.Add("@module", moduleName);
        //        //values.Add("@Refreshed", dt);
        //        //dtResult = GetTableDataManager.GetDataFromSP("usp_fillPageSummaryData_31Aug2023_Working", values);

        //        //return dtResult;
        //        //var tenantId = new SqlParameter("tenantID", SqlDbType.VarChar);
        //        //tenantId.Value = applicationContext.TenantID;

        //        //var mode = new SqlParameter("mode", SqlDbType.VarChar);
        //        //mode.Value = "Update";

        //        //var refreshed = new SqlParameter("Refreshed", SqlDbType.Structured);
        //        //refreshed.Value = dt;
        //        //refreshed.TypeName = "dbo.RefreshedProjects";

        //        dtResult = GetTableDataManager.ExecuteSPTableValuedParameter("usp_fillPageSummaryData_31Aug2023_Working", dt, values);
        //    }
        //    catch (Exception ex)
        //    {
        //        ULog.WriteException(ex);
        //    }
        //    return dtResult;
        //}

        
    }
}
