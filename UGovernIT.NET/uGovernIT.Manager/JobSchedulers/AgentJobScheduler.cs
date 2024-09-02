using System;
using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.Util.Log;
using System.Threading.Tasks;

namespace uGovernIT.Manager.JobSchedulers
{
    public class AgentJobScheduler : IJobScheduler
    {
        public string Duration { get; set; }
        
        /// <summary>
        /// This method is used to run Scheduled Actions i.e. Scheduled Jobs 
        /// </summary>
        /// <param name="TenantID"></param>
        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext _context = ApplicationContext.CreateContext(TenantID);
            ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(_context);
            DateTime currentDateTime = DateTime.Now;

            // Load schedule actions
            List<SchedulerAction> scheduleJobColl = scheduleActionsManager.Load(x => x.StartTime <= currentDateTime);

            if (scheduleJobColl != null && scheduleJobColl.Count > 0)
            {
                ULog.WriteLog(string.Format("AgentJobScheduler: Found {0} jobs scheduled to run at this time", scheduleJobColl.Count));
                AgentJobHelper agentJobHelper = new AgentJobHelper(_context);
                int index = 0;
                int totalItem = scheduleJobColl.Count;
                List<string> ticketsToRefresh = new List<string>();
                
                while (index < totalItem)
                {
                    bool isDeleted = false;
                    SchedulerAction item = scheduleJobColl[index++];
                    ScheduleActionType ActionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), Convert.ToString(item.ActionType));

                    switch (ActionType)
                    {
                        case ScheduleActionType.EscalationEmail:
                            agentJobHelper.EscalationEMail(item);
                            break;
                        case ScheduleActionType.AutoStageMove:
                            agentJobHelper.AutoStageMove(item);
                            ticketsToRefresh.Add(Convert.ToString(item.TicketId));
                            break;
                        case ScheduleActionType.UnHoldTicket:
                            agentJobHelper.AutoUnholdTicket(item);
                            ticketsToRefresh.Add(Convert.ToString(item.TicketId));
                            break;
                        case ScheduleActionType.Reminder:
                            agentJobHelper.SendReminder(item);
                            break;
                        case ScheduleActionType.Query:
                            agentJobHelper.ExecuteQuery(item);
                            break;
                        case ScheduleActionType.Report:
                            agentJobHelper.SendReport(item);
                            break;
                        case ScheduleActionType.Alert:
                            agentJobHelper.SendAlert(item);
                            break;
                        case ScheduleActionType.ScheduledTicket:
                            agentJobHelper.CreateRecurringTickets(item, ticketsToRefresh);
                            break;
                        case ScheduleActionType.UnHoldTask:
                            agentJobHelper.AutoUnholdTask(item);
                            break;
                        default:
                            break;
                    }

                    if (agentJobHelper.IsItemDelete)
                        isDeleted = agentJobHelper.DeleteFromScheduleAction(item);

                    if (isDeleted)
                    {
                        index--;
                        totalItem--;
                    }
                }

                // If we made changes to any tickets in the loop above, add them to the list of tickets to refresh
                //if (ticketsToRefresh.Count > 0)
                //    SPListHelper.ReloadTicketsInCache(ticketsToRefresh, spWeb);

                //Call method to refresh cache for user profile
               // agentJobHelper.RefreshCacheService(spWeb);

                //for delete the ScheduleActionsArchive.
                agentJobHelper.ScheduleActionArchiveCleanup();
            }
        }
    }
}
