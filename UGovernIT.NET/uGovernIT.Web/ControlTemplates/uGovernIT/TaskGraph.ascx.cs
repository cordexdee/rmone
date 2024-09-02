using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class TaskGraph : UserControl
    {
        public string ticketID { get; set; }
        int overdueTaskCnt = 0;
        int pendingTaskCnt = 0;
        int completedTaskCnt = 0;

        protected int pctOverdue { get; set; }
        protected int pctPending { get; set; }
        protected int pctCompleted { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleStageConstraintsManager moduleStageConstraintsManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {          
            moduleStageConstraintsManager = new ModuleStageConstraintsManager(context);

            var lstConstraints = moduleStageConstraintsManager.Load(x => x.TicketId == ticketID).Select(x => new { x.ID, x.TaskDueDate, x.TaskStatus }).ToList();

            if(lstConstraints != null && lstConstraints.Count() > 0)
            {
                overdueTaskCnt = lstConstraints.Where(x => x.TaskDueDate < DateTime.Now && x.TaskStatus != "Completed").Count();
                pendingTaskCnt = lstConstraints.Where(x => x.TaskStatus != "Completed").Count();
                completedTaskCnt = lstConstraints.Where(x => x.TaskStatus == "Completed").Count();
            }

            lblOverdueTasks.Text = overdueTaskCnt.ToString();          
            lblPendingTasks.Text = pendingTaskCnt.ToString();           
            lblCompletedTasks.Text = completedTaskCnt.ToString();

            int max = overdueTaskCnt;
            if (pendingTaskCnt > max)
                max = pendingTaskCnt;
            if (completedTaskCnt > max)
                max = completedTaskCnt;

            if (max == 0)
                max = 1;

            pctOverdue = (overdueTaskCnt * 100) / max;
            pctPending = (pendingTaskCnt * 100) / max;
            pctCompleted= (completedTaskCnt * 100) / max;         
        }        
    }
}
