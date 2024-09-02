using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using uGovernIT.DefaultConfig;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web.Helpers
{
    public class SyncTicketWithWorkflowJob
    {
        private static bool IsProcessActive;
        private static int pctComplete;

        private ModuleViewManager _moduleViewManager = null;
        private LifeCycleStageManager _lifeCycleStageManager = null;
        private ApplicationContext _context = null;
        private ITicketManager ticketManager;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if(_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected LifeCycleStageManager LifeCycleStageManager
        {
            get
            {
                if (_lifeCycleStageManager == null)
                {
                    _lifeCycleStageManager = new LifeCycleStageManager(ApplicationContext);

                }
                return _lifeCycleStageManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        public static bool ProcessState()
        {
            return IsProcessActive;
        }

        public static int GetProgressPercentage()
        {
            return pctComplete;
        }
        private void SetProgressPercentage(int pct)
        {
            pctComplete = pct;
        }

        public void Execute(ApplicationContext applicationContext, string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                return;

            if (IsProcessActive)
                return;

            IsProcessActive = true;
            UpdateTicketsWorkflow(applicationContext, moduleName);
            IsProcessActive = false;
        }

        private void UpdateTicketsWorkflow(ApplicationContext applicationContext, string moduleName)
        {
            DataTable modules = null;
            //DataRow ticketsToUpdate = null;

            Log.WriteLog(string.Format("Updating StageStep in {0} tickets using ModuleStepLookup", moduleName));

            try
            {
                if (!string.IsNullOrWhiteSpace(moduleName))
                    modules = GetTableDataManager.GetTableData($"{DatabaseObjects.Tables.Modules}", $"{DatabaseObjects.Columns.ModuleName}='{moduleName}' and {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");

                if (modules == null || modules.Rows.Count < 0)
                {
                    Log.WriteLog("ERROR: Invalid # of matching rows in Modules list!");
                    return;
                }

                DataRow mRow = modules.Rows[0];
                if (string.IsNullOrWhiteSpace(Convert.ToString(mRow[DatabaseObjects.Columns.ModuleTicketTable])))
                {
                    Log.WriteLog("ERROR: Ticket table not found!");
                    return;
                }

                int moduleId = UGITUtility.StringToInt(mRow[DatabaseObjects.Columns.Id]);

                DataTable stages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages,$"{DatabaseObjects.Columns.ModuleNameLookup}='{mRow[DatabaseObjects.Columns.ModuleName]}' and {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");

                //get all tickets from tansaction table
                var mTicketList = GetTableDataManager.GetTableData(Convert.ToString(mRow[DatabaseObjects.Columns.ModuleTicketTable]),$"{DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'");

                //SPList mTicketList = SPListHelper.GetSPList(Convert.ToString(mRow[DatabaseObjects.Columns.ModuleTicketTable]), spWeb);

                if (mTicketList.Rows.Count == 0 || !uHelper.IfColumnExists(DatabaseObjects.Columns.ModuleStepLookup, mTicketList) || !uHelper.IfColumnExists(DatabaseObjects.Columns.StageStep, mTicketList))
                    return;

                ticketManager = new TicketManager(applicationContext);
                _moduleViewManager = new ModuleViewManager(applicationContext);
                UGITModule uGITModule=   _moduleViewManager.LoadByID(UGITUtility.StringToLong(moduleId));
                List<string> updatedTickets = new List<string>();
                int j = 0;
                foreach (DataRow ticket in mTicketList.Rows)
                {
                    
                    //DataRow stateItem = null;
                    //Continue if no stage step set 
                    if (!UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.StageStep))
                        continue;
                    //fetch id from stage.
                    
                    DataRow stateItem = stages.Select(string.Format("{0} = '{1}' and {2} = {3}", DatabaseObjects.Columns.ModuleNameLookup, moduleName,DatabaseObjects.Columns.Id, ticket[DatabaseObjects.Columns.ModuleStepLookup])).FirstOrDefault();
                    if (stateItem == null)
                        continue;

                    bool ticketUpdated = false;

                    if (UGITUtility.StringToInt(ticket[DatabaseObjects.Columns.StageStep]) != UGITUtility.StringToInt(stateItem[DatabaseObjects.Columns.ModuleStep]))
                    {
                        ticket[DatabaseObjects.Columns.StageStep] = stateItem[DatabaseObjects.Columns.ModuleStep];
                        ticketUpdated = true;
                    }

                    if (Convert.ToString(ticket[DatabaseObjects.Columns.TicketStatus]) != Convert.ToString(stateItem[DatabaseObjects.Columns.StageTitle]))
                    {
                        ticket[DatabaseObjects.Columns.TicketStatus] = Convert.ToString(stateItem[DatabaseObjects.Columns.StageTitle]);
                        ticketUpdated = true;
                    }

                    string actionUserTypes = Convert.ToString(stateItem[DatabaseObjects.Columns.ActionUser]);
                    if (!string.IsNullOrEmpty(actionUserTypes) && actionUserTypes.IndexOf(Constants.Separator10) != -1)
                        actionUserTypes = actionUserTypes.Replace(Constants.Separator10, Constants.Separator);
                            
                    if (Convert.ToString(ticket[DatabaseObjects.Columns.TicketStageActionUserTypes]) != actionUserTypes)
                    {
                        ticket[DatabaseObjects.Columns.TicketStageActionUserTypes] = actionUserTypes;

                        ticket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(applicationContext,actionUserTypes, ticket);
                        ticketUpdated = true;
                    }
                    
                    
                    string dataEditors = Convert.ToString(stateItem[DatabaseObjects.Columns.DataEditors]);
                    if (!string.IsNullOrEmpty(dataEditors) && dataEditors.IndexOf(Constants.Separator10) != -1)
                        dataEditors = dataEditors.Replace(Constants.Separator10, Constants.Separator);
                    if (UGITUtility.IsSPItemExist(ticket,DatabaseObjects.Columns.DataEditor) && Convert.ToString(ticket[DatabaseObjects.Columns.DataEditor]) != dataEditors)
                    {
                        ticket[DatabaseObjects.Columns.DataEditor] = dataEditors;
                        ticketUpdated = true;
                    }

                    //Transaction tables having both column DataEditor and DataEditors, We need to remove DataEditors from each and every table and need to use DataEditor
                    if (UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.DataEditors) && Convert.ToString(ticket[DatabaseObjects.Columns.DataEditors]) != dataEditors)
                    {
                        ticket[DatabaseObjects.Columns.DataEditors] = dataEditors;
                        ticketUpdated = true;
                    }

                    if (ticketUpdated)
                    {
                        if (Convert.ToString(stateItem[DatabaseObjects.Columns.StageType]) == StageType.Closed.ToString())
                        {
                            //if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketClosed, tItem.ParentList))
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketClosed, mTicketList))
                                ticket[DatabaseObjects.Columns.TicketClosed] = 1;

                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, mTicketList) && ticket[DatabaseObjects.Columns.TicketCloseDate] == null)
                                ticket[DatabaseObjects.Columns.TicketCloseDate] = ticket[DatabaseObjects.Columns.CurrentStageStartDate];
                        }
                        else
                        {
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, mTicketList))
                                ticket[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;

                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketClosed, mTicketList))
                                ticket[DatabaseObjects.Columns.TicketClosed] = 0;
                        }

                        ticketManager.Save(uGITModule, ticket);
                        ++j;
                        updatedTickets.Add(Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]));
                    }

                    //Update Job Progress
                    if (mTicketList.Rows.Count > 0)
                    {
                        int pct = (int)(UGITUtility.StringToDouble(j) / mTicketList.Rows.Count * 100);
                        SetProgressPercentage(pct);
                    }
                }
                // Update ModuleStages in configuration cache
                
                _moduleViewManager.UpdateCache(moduleName);
                Log.WriteLog(string.Format("Finished updating {0} tickets with new workflow, {1} tickets updated", moduleName, updatedTickets.Count));
            }

            catch (Exception ex)
            {
                Log.WriteException(ex, "Error Updating tickets with workflow changes");
            }
        }
    }
}