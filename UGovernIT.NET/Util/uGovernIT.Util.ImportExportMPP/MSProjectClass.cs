using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.Util.ImportExportMPP
{

    class MSProjectClass
    {
        public Project ImportTask(string strFileName,ConfigSetiingMpp setting)
        {
            Project objProject = null;
            //// create the new Instance of "MS-Project" application.
            //Microsoft.Office.Interop.MSProject.ApplicationClass projApp = new Microsoft.Office.Interop.MSProject.ApplicationClass();
            //if (projApp != null)
            //{
            //    if (projApp.FileOpenEx(strFileName, true, Microsoft.Office.Interop.MSProject.PjMergeType.pjDoNotMerge, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, "MSProject.mpp", Type.Missing, Microsoft.Office.Interop.MSProject.PjPoolOpen.pjDoNotOpenPool, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing))
            //    {
            //        if (projApp.Projects.Count >= 1)
            //        {
            //            // Get the active project
            //            Microsoft.Office.Interop.MSProject.Project proj = projApp.ActiveProject;
            //            if (proj.Tasks.Count >= 1)
            //            {
            //                objProject = new Project();
            //                objProject.taskList = new List<TaskList>();
            //                foreach (Microsoft.Office.Interop.MSProject.Task task in proj.Tasks)
            //                {
            //                    if (task == null || task.ID == 0)
            //                    {
            //                        continue;
            //                    }
                               
            //                    objProject.HoursPerDay = proj.HoursPerDay;
            //                    TaskList obj = new TaskList();
            //                    obj.Name = task.Name;
            //                    obj.Notes = task.Notes;
            //                    obj.Milestone = UGITUtility.StringToBoolean(task.Milestone);
            //                    //obj.Start = Utiluhelper.GetDateFromJavaUtil(task.Start);
            //                    //obj.Finish = Utiluhelper.GetDateFromJavaUtil(task.Finish);
            //                    obj.ResourceNames = task.ResourceNames;
            //                    obj.Duration = UGITUtility.StringToDouble(task.Duration);
            //                    obj.ParentTask = task.OutlineParent.ID.ToString();
            //                    if (task.PredecessorTasks.Count > 0)
            //                    {
            //                        obj.taskIds = new List<string>();
            //                        obj.taskDependencyList = new List<TaskDependency>();
            //                        foreach (Microsoft.Office.Interop.MSProject.Task preTask in task.PredecessorTasks)
            //                        {
            //                            obj.taskIds.Add(preTask.ID.ToString());
            //                        }
            //                        List<ProjectTaskDependency> taskDeps = new List<ProjectTaskDependency>();
            //                        foreach (Microsoft.Office.Interop.MSProject.TaskDependency taskD in task.TaskDependencies)
            //                        {
            //                            TaskDependency taskDep = new TaskDependency();
            //                            taskDep.Index = taskD.Index;
            //                            taskDep.ParenID = taskD.Parent.ID;
            //                            if (taskD.To != null)
            //                                taskDep.ToID = taskD.To.ID;

            //                            if (taskD.From != null)
            //                                taskDep.FromID = taskD.From.ID;

            //                            taskDep.Lag = Convert.ToDouble(taskD.Lag);
            //                            taskDep.LagType = (ProjectFormatUnit)(taskD.LagType);
            //                            taskDep.Type = (ProjectTaskLinkType)taskD.Type;
            //                            obj.taskDependencyList.Add(taskDep);
            //                        }
            //                    }
            //                    obj.Id = task.ID;
            //                    objProject.taskList.Add(obj);
            //                }
            //                return objProject;
            //            }
            //        }
            //    }
            //}
            return objProject;
        }
        public void MSExportTask(Project pro, string fileName, string fullFileName)
        {
            if (pro != null)
            {
                //// kill the process if any instance is already open.
                //FindAndKillProcess("WINPROJ");
                //// Create the new instance of MS-Project application.
                //Microsoft.Office.Interop.MSProject.ApplicationClass projectApp = new Microsoft.Office.Interop.MSProject.ApplicationClass();
                //// create a new file in Ms-Proj application.
                //projectApp.FileNew(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //// create an active project in file.
                //Microsoft.Office.Interop.MSProject.Project project = projectApp.ActiveProject;
                //Microsoft.Office.Interop.MSProject.Task task;
                //double workingMinutePerDay = project.HoursPerDay * 60;
                //pro.datapropert = new List<Util.ImportExportMPP.DataPropert>();
                //foreach (TaskList tasklist in pro.taskList)
                //{
                //    task = project.Tasks.Add(tasklist.Name, Type.Missing);
                //    task.Start = Convert.ToDateTime(tasklist.Start);
                //    task.Work = tasklist.Duration;
                //    task.Notes = Convert.ToString(tasklist.Notes);
                //    task.ResourceNames = tasklist.ResourceNames;
                //    task.PercentComplete = tasklist.PercentComplete;
                //    task.Milestone = tasklist.Milestone;
                //    task.OutlineLevel = Convert.ToInt16(Convert.ToInt16(tasklist.OutlineLevel));
                //    task.Finish = Convert.ToDateTime(tasklist.Finish);
                //    task.Work = tasklist.Work * 60;
                //    Util.ImportExportMPP.DataPropert dataPropert = new DataPropert();
                //    dataPropert.PMMID = Convert.ToInt32(tasklist.Id);
                //    dataPropert.PredecessorsID = string.Join(",", tasklist.taskIds);
                //    dataPropert.TaskIndex = tasklist.Id;
                //    pro.datapropert.Add(dataPropert);
                //}
                //if (pro.datapropert != null && pro.datapropert.Count > 0)
                //{
                //    foreach (DataPropert taskRow in pro.datapropert)
                //    {
                //        if (Convert.ToString(taskRow.PredecessorsID) != string.Empty)
                //        {
                //            string[] predecessorsArray = Convert.ToString(taskRow.PredecessorsID).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                //            string msPreds = string.Empty;
                //            foreach (string preds in predecessorsArray)
                //            {
                //                List<DataPropert> PredecessorsRow = pro.datapropert.Where(x => x.PMMID.Equals(preds.Trim())).ToList();
                //                if (PredecessorsRow.Count > 0)
                //                {
                //                    if (Array.IndexOf(predecessorsArray, preds) > 0)
                //                    {
                //                        msPreds += ",";
                //                    }
                //                    msPreds += PredecessorsRow[0].TaskIndex;
                //                }
                //            }
                //            taskRow.PredecessorsID = msPreds;
                //        }

                //    }
                //}
                //if (pro.datapropert != null && pro.datapropert.Count > 0)
                //{
                //    foreach (DataPropert taskRow in pro.datapropert)
                //    {
                //        try
                //        {
                //            task = project.Tasks.get_UniqueID(Convert.ToInt32(taskRow.TaskIndex));
                //            if (Convert.ToString(taskRow.PredecessorsID) != string.Empty)
                //            {
                //                string[] predecessorsArray = Convert.ToString(taskRow.PredecessorsID).Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                //                string msPreds = string.Empty;
                //                foreach (string preds in predecessorsArray)
                //                {
                //                    Microsoft.Office.Interop.MSProject.Task parentTask = project.Tasks.get_UniqueID(Convert.ToInt32(preds.Trim()));
                //                    task.LinkPredecessors(parentTask, Microsoft.Office.Interop.MSProject.PjTaskLinkType.pjFinishToStart, Type.Missing);
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            //Log.WriteException(ex, "Error updating predecessors during MS Project export");
                //        }
                //    }
                //}
                //if (project.Tasks.Count >= 1)
                //{
                //    try
                //    {
                //        // string serverPath = Server.MapPath(fileName);
                //        if (System.IO.File.Exists(fullFileName))
                //        {
                //            System.IO.File.Delete(fullFileName);
                //        }

                //        projectApp.FileSaveAs(fullFileName, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //        projectApp.DocClose();
                //    }
                //    catch (System.Exception ex)
                //    {
                //        //Util.Log.ULog.WriteException(ex);
                //    }

                //}
            }


        }
        private DateTime GetDateFromJavaUtil(java.util.Date strdate)
        {
            java.util.Date d = strdate;
            string javadate = new java.text.SimpleDateFormat("MM/dd/yyyy").format(d);
            DateTime dtime = DateTime.MinValue;

            dtime = DateTime.Parse(javadate);
            return dtime;
        }
        private bool FindAndKillProcess(string name)
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (System.Diagnostics.Process clsProcess in System.Diagnostics.Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes by using the StartsWith Method,
                //this prevents us from incluing the .EXE for the process we're looking for.
                //. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running
                if (clsProcess.ProcessName.StartsWith(name))
                {
                    //since we found the proccess we now need to use the
                    //Kill Method to kill the process. Remember, if you have
                    //the process running more than once, say IE open 4
                    //times the loop thr way it is now will close all 4,
                    //if you want it to just close the first one it finds
                    //then add a return; after the Kill
                    clsProcess.Kill();

                    //process killed, return true
                    return true;
                }
            }

            //process not found, return false
            return false;
        }
    }
}
