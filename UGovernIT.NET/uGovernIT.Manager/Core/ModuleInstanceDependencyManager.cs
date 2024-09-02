using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class ModuleInstanceDependencyManager
    {
        /// <summary>
        /// Get Dependent Task to set Prodcessor list.
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="dependentTasks"></param>
        /// <param name="skipChildren"></param>
        public static void GetDependentTasks(UGITTask currentTask, ref List<UGITTask> dependentTasks, bool skipChildren)
        {
            if (currentTask == null)
                return;

            if (dependentTasks.Contains(currentTask))
                return;

            dependentTasks.Add(currentTask);

            if (currentTask.ParentTaskID > 0)
            {
                GetDependentTasks(currentTask.PTask, ref dependentTasks, true);
            }

            if (!skipChildren && currentTask.ChildCount > 0)
            {
                foreach (UGITTask tTask in currentTask.ChildTasks)
                {
                    GetDependentTasks(tTask, ref dependentTasks, false);
                }
            }

            if (currentTask.SuccessorsObj != null)
            {
                foreach (UGITTask tTask in currentTask.SuccessorsObj)
                {
                    GetDependentTasks(tTask, ref dependentTasks, false);
                }
            }
        }


        /// <summary>
        /// Map relationship (adding childtasks in ChildTasks object and add parenttask in PTask Obj).
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static List<UGITTask> MapRelationshipObjects(List<UGITTask> tasks)
        {
            tasks.ForEach(
                task =>
                {
                    if (task.ParentTask > 0)
                    {
                        var parenttask = tasks.FirstOrDefault(z => z.ID == task.ParentTask);
                        if (parenttask != null)
                        {
                            parenttask.ChildTasks = tasks.FindAll(y => y.ParentTask == task.ParentTask);
                            parenttask.ChildCount = tasks.FindAll(y => y.ParentTask == task.ParentTask).Count;
                        }
                        task.PTask = tasks.FirstOrDefault(z => z.ID == task.ParentTask);
                        task.SuccessorsObj = tasks.Where(z => z.PredecessorsObj != null && z.PredecessorsObj.Exists(y => y.ID == task.ID)).ToList();

                        //Attaches Child task objects if child count is more then 0
                        task.ChildTasks = new List<UGITTask>();
                        if (task.ID > 0)
                            task.ChildTasks = tasks.Where(y => y.ParentTask == task.ID).ToList();
                        if (task.ChildTasks != null)
                            task.ChildCount = task.ChildTasks.Count;


                        if (task.SuccessorsObj != null && task.SuccessorsObj.Count > 0)
                        {
                            Action<UGITTask> recursive = null;
                            List<UGITTask> successorchildtasks = new List<UGITTask>();
                            foreach (var item in task.SuccessorsObj)
                            {
                                recursive = (t) =>
                                {
                                    if (t.ChildTasks != null && t.ChildTasks.Count > 0)
                                    {
                                        t.ChildTasks.ForEach(x =>
                                        {
                                            successorchildtasks.Add(x);
                                            recursive(x);

                                        });
                                    }
                                };
                                recursive(item);
                            }
                            task.SuccessorsObj.AddRange(successorchildtasks);
                        }
                    }
                });

            return tasks;
        }
    }
}
