using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Helpers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class ReSyncAllProjectAllocationsJob : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
           
            //ResourceAllocationManager.RefreshAllTicketsAllocations(context);
            
        }

        private void ReSyncAllProjectAllocations(ApplicationContext context)
        {

            try
            {

                RMMSummaryHelper.DeleteAllAllocations(context, "");
               // for NPR allocations
                NPRResourcesManager npr = new NPRResourcesManager(context);
                DataTable dtNPR = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRRequest, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (dtNPR != null)
                {
                    foreach (DataRow dr in dtNPR.Rows)
                    {
                        string ticketId = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);
                        string pmmLookUpId = Convert.ToString(dr[DatabaseObjects.Columns.TicketPMMIdLookup]);
                        bool isClosed = UGITUtility.StringToBoolean(dr[DatabaseObjects.Columns.TicketClosed]);
                        if (!string.IsNullOrEmpty(ticketId) && string.IsNullOrEmpty(pmmLookUpId) && !isClosed)
                            RecreateAllocationByTicketID("NPR", ticketId, context);
                        else
                            RMMSummaryHelper.DeleteAllocationsByTasks(context,ticketId,true);
                    }
                }

              //for PMM allocations
               DataTable dtPMM = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (dtPMM != null)
                {
                    foreach (DataRow dr in dtPMM.Rows)
                    {
                        if (dr[DatabaseObjects.Columns.TicketId] != null)
                        {
                            RecreateAllocationByTicketID("PMM", Convert.ToString(dr[DatabaseObjects.Columns.TicketId]), context);
                        }
                    }
                }

               // for TSK allocations
               DataTable dtTSK = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (dtTSK != null)
                {
                    foreach (DataRow dr in dtTSK.Rows)
                    {
                        if (dr[DatabaseObjects.Columns.TicketId] != null)
                        {
                            RecreateAllocationByTicketID("TSK", Convert.ToString(dr[DatabaseObjects.Columns.TicketId]), context);
                        }
                    }
                }

              // moduleTask allocation..
                DataTable dtModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTasks, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (dtModules != null)
                {
                    DataView dtmoduleview = new DataView(dtModules);
                    DataTable distinctValues = dtmoduleview.ToTable(true, DatabaseObjects.Columns.TicketId);

                    foreach (DataRow dr in distinctValues.Rows)
                    {
                        if (dr[DatabaseObjects.Columns.TicketId] != null)
                        {
                            RecreateAllocationByTicketID(uHelper.getModuleNameByTicketId(Convert.ToString(dr[DatabaseObjects.Columns.TicketId])), Convert.ToString(dr[DatabaseObjects.Columns.TicketId]),context);
                        }
                    }
                }

                DataTable workItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceWorkItems, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                DataTable allocationItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceAllocation, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                DataTable actualItems = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ResourceTimeSheet,$"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                if (workItems != null && workItems.Rows.Count > 0)
                {
                    RMMSummaryHelper.UpdateResourceUsageSummary(context, workItems, allocationItems, actualItems);
                }
                RMMSummaryHelper.DistributeAllocationByMonth(context, updatePlannedOnly: true);
            }

            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

        }


        private static void RecreateAllocationByTicketID(string Module, string ProjectPublicID, ApplicationContext context)
        {
            UGITTaskManager uGITTask = new UGITTaskManager(context);
            ResourceAllocationManager allocationManager = new ResourceAllocationManager(context);
            List<UGITTask> tasks = uGITTask.LoadByProjectID(Module, ProjectPublicID);
            bool checkIsTask = true;

            if (Module == "NPR" && (tasks == null || tasks.Count < 1))
            {
                tasks = allocationManager.LoadNPRResourceList(ProjectPublicID);
                checkIsTask = false;
            }

            allocationManager.UpdateProjectPlannedAllocationByUser(tasks, Module, ProjectPublicID, checkIsTask);

        }
       
    }
}
