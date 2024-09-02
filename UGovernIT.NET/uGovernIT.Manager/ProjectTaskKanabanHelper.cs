using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Manager
{
    public class ProjectTaskKanabanHelper
    {
        public ApplicationContext applicationContext = null;
        public ProjectTaskKanabanHelper(ApplicationContext context)
        {
            applicationContext = context;
        }


        public List<KanaBanTaskNode> getAllTaskOfTicket(string TicketID)
        {
            UserProfileManager userManager = new UserProfileManager(applicationContext);
            UGITTaskManager taskManager = new UGITTaskManager(applicationContext);
            //ModuleTaskHistoryManager _moduleTaskHistoryManager = new ModuleTaskHistoryManager(applicationContext);
            List<UGITTask> tasks = new List<UGITTask>();

            string moduleName = uHelper.getModuleNameByTicketId(TicketID);
            tasks = taskManager.Load(x => x.TicketId == TicketID && x.ModuleNameLookup == moduleName).ToList();
           
            // Manage min date for Start and Due Dates
            taskManager.ManageMinStartDueDates(ref tasks);

            //tasks = UGITTaskManager.MapRelationalObjects(tasks);

            List<UGITTask> CriticalPathTasks = taskManager.GetCriticalPathTask(tasks);
            //taskManager.ReManageTasks(ref tasks); commented 27 jan 2020
            List<KanaBanTaskNode> tasklist = new List<KanaBanTaskNode>();
            UGITTask predTask = null;
            List<UGITAssignTo> listAssignToPct = null;
            foreach (UGITTask task in tasks)
            {
                KanaBanTaskNode model = new KanaBanTaskNode();                
                model.ID = task.ID;
                model.AssignedTo = task.AssignedTo;
                listAssignToPct = taskManager.GetUGITAssignPct(task.AssignToPct);
                if (listAssignToPct != null && listAssignToPct.Count > 0)
                {
                    model.AssignToPct = string.Join(Constants.Separator, listAssignToPct.Select(x => string.Join(Constants.Separator1, new string[] { x.ID, x.Percentage, x.UserName })));
                }
                model.AssignedToName = uHelper.GetUserNameBasedOnId(applicationContext, task.AssignedTo);
                model.Title = task.Title;
                model.Status = task.Status;
                model.StartDate = task.StartDate;
                model.DueDate = task.DueDate;
                model.PercentComplete = task.PercentComplete;
                model.ParentTaskID = task.ParentTaskID;
                model.ItemOrder = task.ItemOrder;
                model.ChildCount = task.ChildCount;
                model.EstimatedHours = task.EstimatedHours;
                model.EstimatedRemainingHours = task.EstimatedRemainingHours;
                model.ActualHours = task.ActualHours;
                model.Duration = task.Duration;
                model.Level = task.Level;
                if(task.Level == 1)
                {
                    model.ParentTaskIDummy = 0;
                }
                else
                {
                    model.ParentTaskIDummy = task.ParentTaskID;
                }

                if(task.ChildCount == 0)
                {
                    model.HasItem = false;
                }
                else
                {
                    model.HasItem = true;
                }

                if (!string.IsNullOrEmpty(task.Predecessors))
                {
                    string[] preds = task.Predecessors.Split(',');
                    List<string> newPredecessors = new List<string>();

                    foreach (string pred in preds.OrderBy(x => x))
                    {
                        //UGITTask predTask = taskManager.Load(x => x.TicketId == TicketID && x.ID == UGITUtility.StringToInt(pred)).FirstOrDefault();
                        predTask = tasks.Where(x => x.ID == Convert.ToInt32(pred)).FirstOrDefault();
                        if (predTask != null)
                        {
                            newPredecessors.Add(Convert.ToString(predTask.ItemOrder));
                        }
                    }
                    model.Predecessors = string.Join(Constants.Separator6 + Constants.SpaceSeparator, newPredecessors);
                }
                model.Contribution = task.Contribution;
                if (CriticalPathTasks.Any(x => x.ID == task.ID))
                {
                    task.IsCritical = true;
                }
                model.isCritical = task.IsCritical;

                tasklist.Add(model);
            }

            return tasklist;


        }

    }
}
