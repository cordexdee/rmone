using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class TicketsWithWorkflowJob
    {
        public bool IsProcessActive;
        public int PctCompete;
        public string moduleName;
        public string TenantID { get; set; }
    }
    public class SyncTicketsWithWorkflowJob
    {
        private static List<TicketsWithWorkflowJob> syncFlows;

        public SyncTicketsWithWorkflowJob()
        {
            if (syncFlows == null)
                syncFlows = new List<TicketsWithWorkflowJob>();
        }

        public static bool ProcessState(string tenantID)
        {
            if (syncFlows == null)
                syncFlows = new List<TicketsWithWorkflowJob>();
            TicketsWithWorkflowJob syncJob = syncFlows.FirstOrDefault(x => x.TenantID.ToLower() == tenantID.ToLower());
            if (syncJob != null)
            {
                return syncJob.IsProcessActive;
            }
            return false;
        }

        public static int GetProgressPercentage(string tenantID)
        {
            if (syncFlows == null)
                syncFlows = new List<TicketsWithWorkflowJob>();

            TicketsWithWorkflowJob syncJob = syncFlows.FirstOrDefault(x => x.TenantID.ToLower() == tenantID.ToLower());
            if (syncJob != null)
            {
                return syncJob.PctCompete;
            }
            return 0;
        }

        private void SetProgressPercentage(string tenantID, int pct)
        {
            if (syncFlows == null)
                syncFlows = new List<TicketsWithWorkflowJob>();

            TicketsWithWorkflowJob syncJob = syncFlows.FirstOrDefault(x => x.TenantID.ToLower() == tenantID.ToLower());
            if (syncJob != null)
            {
                syncJob.PctCompete = pct;
            }
        }

        public void Execute(string tenantID, string moduleName)
        {
            if (syncFlows == null)
                syncFlows = new List<TicketsWithWorkflowJob>();

            TicketsWithWorkflowJob syncJob = syncFlows.FirstOrDefault(x => x.TenantID.ToLower() == tenantID.ToLower());
            if (syncJob == null)
            {
                syncJob = new TicketsWithWorkflowJob();
                syncJob.TenantID = tenantID;
                syncJob.moduleName = moduleName;
                syncJob.PctCompete = 0;
                syncFlows.Add(syncJob);
            }


            if (string.IsNullOrWhiteSpace(moduleName))
                return;

            if (syncJob.IsProcessActive)
                return;

            ApplicationContext _context = ApplicationContext.CreateContext(tenantID);
            ModuleViewManager moduleMgr = new ModuleViewManager(_context);
            moduleMgr.UpdateCache(moduleName);

            syncJob.IsProcessActive = true;

            Thread syncThread = new Thread(delegate ()
            {
                using (ApplicationContext context = ApplicationContext.CreateContext(tenantID))
                {
                    UpdateTicketsWorkflow(context, moduleName);

                    TicketsWithWorkflowJob job = syncFlows.FirstOrDefault(x => x.TenantID.ToLower() == tenantID.ToLower());
                    if (job != null)
                        job.IsProcessActive = false;
                }
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void UpdateTicketsWorkflow(ApplicationContext context, string sModuleName)
        {
            ULog.WriteLog(string.Format("Updating StageStep in {0} tickets using ModuleStepLookup", sModuleName));

            try
            {
                ModuleViewManager moduleMgr = new ModuleViewManager(context);
                Ticket ticket = new Ticket(context, sModuleName);
                UGITModule module = moduleMgr.GetByName(sModuleName);
                TicketManager ticketManager = new TicketManager(context);

                DataTable ticketData = ticketManager.GetAllTicketsWithOutCache(module);
                if (ticketData == null)
                    return;

                DataRowCollection ticketCollection = ticketData.Rows;
                long collectionCount = ticketCollection.Count;

                List<string> updatedTickets = new List<string>();

                //Find ticket collection which need to update
                for (int j = 0; j < collectionCount; j++)
                {
                    DataRow tItem = ticketCollection[j];
                    //tItem.SetModified();

                    List<LifeCycleStage> stages = ticket.GetCurrentLifeCyleStages(tItem);
                    LifeCycleStage currentStage = ticket.GetTicketCurrentStage(tItem);

                    if (currentStage == null)
                        continue;

                    bool ticketUpdated = false;
                    if (currentStage.ID != UGITUtility.StringToLong(tItem[DatabaseObjects.Columns.ModuleStepLookup]) || currentStage.StageStep != UGITUtility.StringToLong(tItem[DatabaseObjects.Columns.StageStep]))
                    {
                        tItem[DatabaseObjects.Columns.StageStep] = currentStage.StageStep;
                        tItem[DatabaseObjects.Columns.ModuleStepLookup] = currentStage.ID;
                        ticketUpdated = true;
                    }

                    if (Convert.ToString(tItem[DatabaseObjects.Columns.TicketStatus]) != currentStage.StageTitle)
                    {
                        tItem[DatabaseObjects.Columns.TicketStatus] = currentStage.StageTitle;
                        ticketUpdated = true;
                    }

                    if (Convert.ToString(tItem[DatabaseObjects.Columns.TicketStageActionUserTypes]) != currentStage.ActionUser)
                    {
                        tItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = currentStage.ActionUser;
                        tItem[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context, currentStage.ActionUser, tItem);
                        ticketUpdated = true;
                    }

                    if (tItem.Table.Columns.Contains(DatabaseObjects.Columns.DataEditors) && Convert.ToString(tItem[DatabaseObjects.Columns.DataEditors]) != currentStage.DataEditors)
                    {
                        tItem[DatabaseObjects.Columns.DataEditors] = currentStage.DataEditors;
                        ticketUpdated = true;
                    }

                    if (ticketUpdated)
                    {
                        if (currentStage.StageType == StageType.Closed.ToString())
                        {
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketClosed, tItem.Table))
                                tItem[DatabaseObjects.Columns.TicketClosed] = 1;
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, tItem.Table) && tItem[DatabaseObjects.Columns.TicketCloseDate] == null)
                                tItem[DatabaseObjects.Columns.TicketCloseDate] = tItem[DatabaseObjects.Columns.CurrentStageStartDate];
                         
                        }
                        else
                        {
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, tItem.Table))
                                tItem[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;
                            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketClosed, tItem.Table))
                                tItem[DatabaseObjects.Columns.TicketClosed] = 0;
                        }
                       
                     
                        ticketManager.Save(module, tItem);
                        
                        updatedTickets.Add(Convert.ToString(tItem[DatabaseObjects.Columns.TicketId]));
                    }

                    //Update Job Progress
                    if (collectionCount > 0)
                    {
                        int pct = (int)(UGITUtility.StringToDouble(j) / collectionCount * 100);
                        SetProgressPercentage(context.TenantID, pct);
                    }
                }

                // Reload updated tickets in cache
                //SPListHelper.ReloadTicketsInCache(updatedTickets, spWeb);
                ULog.WriteLog(string.Format("Finished updating {0} tickets with new workflow, {1} tickets updated", sModuleName, updatedTickets.Count));
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "Error Updating tickets with workflow changes");
            }
        }
    }
}
