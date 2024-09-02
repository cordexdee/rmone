
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
 using System.Threading.Tasks;
 

namespace uGovernIT.DxReport
{
    public class ReportAgentJobScheduler : IJobScheduler
    {
       
        public ReportAgentJobScheduler()
        {
              
        }

        //public string Duration
        //{
        //    get
        //    {
        //        return Cron.MinuteInterval(2);
        //    }
        //}

        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext _context = ApplicationContext.Create();
            ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(_context);
            ScheduleActionsArchiveManager scheduleActionsArchiveManager = new ScheduleActionsArchiveManager(_context);
            DateTime currentDateTime = DateTime.Now;
            List<SchedulerAction> dtScheduleJobs = scheduleActionsManager.Load(x => x.StartTime <= currentDateTime && x.ActionType == ScheduleActionType.Report.ToString());

            if (dtScheduleJobs != null && dtScheduleJobs.Count() > 0)
            {
                ReportAgentJobHelper agentJobHelper = new ReportAgentJobHelper(_context);
             //   int index = 0;
               // bool isExecute = true;
                int totalItem = dtScheduleJobs.Count();
                List<string> ticketsToRefresh = new List<string>();
                foreach (SchedulerAction item in dtScheduleJobs)
                {
                    agentJobHelper.SendReport(item);
                }
                //while (isExecute)
                //{
                //    bool isDeleted = false;
                //    SchedulerAction item = dtScheduleJobs[index++];
                //    agentJobHelper.SendReport(item);

                //    if (agentJobHelper.IsItemDelete)
                //        isDeleted = agentJobHelper.DeleteSPListItem(item);

                //    if (isDeleted)
                //        index--;

                //    isExecute = ((dtScheduleJobs.Count() <= index) || (dtScheduleJobs.Count() == 0)) ? false : true;
                //}

                scheduleActionsArchiveManager.ScheduleActionArchiveCleanup();
            }
        }
    }
}
