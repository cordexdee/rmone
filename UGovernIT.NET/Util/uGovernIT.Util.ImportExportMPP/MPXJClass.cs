using net.sf.mpxj;
using net.sf.mpxj.mpp;
using net.sf.mpxj.mspdi;
using net.sf.mpxj.reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using uGovernIT.Utility;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;
using System.Globalization;
using uGovernIT.Utility.Entities;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;
using uGovernIT.Util.Log;

namespace uGovernIT.Util.ImportExportMPP
{
    public class MPXJClass
    {
        public Project GetMPXJResult(string strFileName, ConfigSetiingMpp setting, string uploadedFilePath)
        {
            Project objProject = new Project();
            ProjectReader reader = null;
            strFileName = strFileName.Replace("\"", "");
            if (strFileName.EndsWith(".mpp"))
                reader = new MPPReader();
            else
                reader = new MSPDIReader();

            string filePath = uploadedFilePath + strFileName;
            try
            {
                ProjectFile project = reader.read(filePath);
                TaskContainer taskcontainer = project.Tasks;
                ProjectCalendar clander = new ProjectCalendar(project);
                double workingMinutePerDay = clander.MinutesPerDay;
                objProject.taskList = new List<TaskList>();
                foreach (net.sf.mpxj.Task task in taskcontainer)
                {
                    ResourceAssignment dd = new ResourceAssignment(project, task);
                    if (task == null || task.ID.intValue() == 0)
                    {
                        continue;
                    }

                    TaskList obj = new TaskList();
                    obj.Name = task.Name;
                    obj.Start = Utiluhelper.GetDateFromJavaUtil(task.Start);
                    obj.Finish = Utiluhelper.GetDateFromJavaUtil(task.Finish); 
                    obj.Notes = task.Notes;
                    obj.Milestone = UGITUtility.StringToBoolean(task.Milestone);
                    obj.Duration = UGITUtility.StringToDouble(task.Duration.Duration);
                    obj.Id = task.ID.intValue();
                    obj.PercentageComplete = UGITUtility.StringToDouble(task.PercentageComplete);
                    obj.ResourceAssignments = task.ResourceAssignments.size();
                    obj.workingMinutePerDay = workingMinutePerDay;
                    if (task.ParentTask != null)
                        obj.ParentTask = Convert.ToString(task.ParentTask.ID);
                    if (!setting.importDatesOnly)
                    {
                        if (task.Predecessors != null && task.Predecessors.size() > 0)
                        {
                            obj.taskIds = new List<string>();
                            obj.taskDependencyList = new List<TaskDependency>();
                            foreach (Relation taskD in task.Predecessors.toArray())
                            {
                                obj.taskIds.Add(taskD.TargetTask.ID.ToString());
                                TaskDependency taskDep = new TaskDependency();
                                taskDep.Index = taskD.TargetTask.ID.intValue();
                                taskDep.ParenID = taskD.SourceTask.ID.intValue();
                                if (taskD.TargetTask != null)
                                    taskDep.ToID = taskD.TargetTask.ID.intValue();

                                if (taskD.SourceTask != null)
                                    taskDep.FromID = taskD.SourceTask.ID.intValue();

                                taskDep.Lag = Convert.ToDouble(taskD.Lag.Duration);
                                taskDep.LagType = (ProjectFormatUnit)Utiluhelper.GetProjectFormatUnit(taskD.TargetTask.Duration.Units.Name);
                                taskDep.Type = (ProjectTaskLinkType)taskD.TargetTask.Type.Value;
                                obj.taskDependencyList.Add(taskDep);
                            }
                        }
                    }
                    if (!setting.dontImportAssignee && task.ResourceAssignments.size() > 0)
                    {
                        java.util.Iterator assingmentIterator = task.ResourceAssignments.iterator();
                        string strAssignToPct = string.Empty;
                        while (assingmentIterator.hasNext())
                        {
                            ResourceAssignment assignt = assingmentIterator.next() as ResourceAssignment;
                            if (assignt.Resource == null)
                                continue;
                            string strMainAssignToPct = assignt.Resource.Name;
                            if (assignt.Units.doubleValue() > 0)
                            {
                                strMainAssignToPct = string.Format("{0}{1}{2}{3}", strMainAssignToPct, Constants.Separator1, assignt.Units.doubleValue(), Constants.Separator);
                            }
                            obj.strMainAssignToPct += strMainAssignToPct;
                        }
                    }
                    //Added 27 jan 2020
                    if (setting.calculateEstimatedHrs)
                    {
                        double strMainCalculateEstHrsToPct = Math.Round(obj.Duration * obj.HoursPerDay, 2);
                        obj.strmainCalculateEstHrsToPct = strMainCalculateEstHrsToPct;
                    
                    }
                    //

                    //Added status calculation

                    if (obj.PercentageComplete == 100)
                        obj.Status = Constants.Completed;
                    else if (obj.PercentageComplete > 0 && obj.PercentageComplete < 100)
                        obj.Status = Constants.InProgress;
                    else if (obj.PercentageComplete == 0)
                        obj.Status = Constants.NotStarted;
                    objProject.taskList.Add(obj);
                }
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            return objProject;
        }
        public void MPXJExportTask(Project project,string fileName,string fullFileName)
        {
            if (project != null)
            {
                List<Tuple<int, int, string>> resourceList = new List<Tuple<int, int, string>>();
                ProjectFile projectFile = new ProjectFile();
                ProjectCalendar clander = new ProjectCalendar(projectFile);
                double workingMinutePerDay = clander.MinutesPerDay * 60;
                net.sf.mpxj.Task task;
                project.datapropert = new List<Util.ImportExportMPP.DataPropert>();
                foreach (TaskList tasklist in project.taskList)
                {
                    task = projectFile.AddTask();
                    task.Name = tasklist.Name;
                    task.Start = Utiluhelper.GetJavaDateFromSystemDate(Convert.ToString(tasklist.Start));
                    task.ActualStart = Utiluhelper.GetJavaDateFromSystemDate(Convert.ToString(tasklist.ActualStart));
                    task.Duration = Duration.getInstance(tasklist.Duration, TimeUnit.DAYS);
                    task.Work = Duration.getInstance(tasklist.Work, TimeUnit.HOURS);
                    task.ActualWork = Duration.getInstance(tasklist.ActualWork, TimeUnit.HOURS);
                    task.RemainingWork = Duration.getInstance(tasklist.RemainingWork, TimeUnit.HOURS);
                    task.Notes = tasklist.Notes;
                    task.PercentageComplete = java.lang.Double.valueOf(Convert.ToDouble(tasklist.PercentComplete));
                    task.PercentageWorkComplete = java.lang.Double.valueOf(Convert.ToDouble(tasklist.PercentComplete));
                    task.Milestone = tasklist.Milestone;
                    task.OutlineLevel = java.lang.Integer.valueOf(tasklist.OutlineLevel);
                    task.Finish = Utiluhelper.GetJavaDateFromSystemDate(Convert.ToString(tasklist.Finish));                    
                    Util.ImportExportMPP.DataPropert dataPropert = new DataPropert();
                    dataPropert.PMMID = Convert.ToInt32(tasklist.Id);
                    dataPropert.PredecessorsID = string.Join(";",tasklist.taskIds);
                    dataPropert.TaskIndex = task.ID.intValue();
                    project.datapropert.Add(dataPropert);
                    if (tasklist.listAssignTo != null && tasklist.listAssignTo.Count > 0 )
                    {

                        foreach (var item in tasklist.listAssignTo)
                        {
                            Tuple<int, int, string> resource = resourceList.FirstOrDefault(x => x.Item3 != null && x.Item3.ToLower() == item.LoginName.ToLower());
                            Resource rResource = null;
                            if (resource != null)
                            {
                                rResource = projectFile.GetResourceByID(java.lang.Integer.valueOf(resource.Item1));
                            }
                            else
                            {
                                rResource = projectFile.AddResource();
                                rResource.Name = item.LoginName;
                                resource = new Tuple<int, int, string>(rResource.ID.intValue(), rResource.UniqueID.intValue(), rResource.Name);
                                resourceList.Add(resource);
                            }

                            ResourceAssignment rAssign = projectFile.NewResourceAssignment(task);
                            rAssign.Units = java.lang.Double.valueOf(Convert.ToDouble(item.Percentage));
                            rAssign.ResourceUniqueID = rResource.UniqueID;
                            rResource.AddResourceAssignment(rAssign);
                            task.AddResourceAssignment(rAssign);
                        }
                    }
                }
                if (project.datapropert != null && project.datapropert.Count > 0)
                {
                    foreach (DataPropert taskRow in project.datapropert)
                    {
                        if (Convert.ToString(taskRow.PredecessorsID) != string.Empty)
                        {
                            string[] predecessorsArray = Convert.ToString(taskRow.PredecessorsID).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            string msPreds = string.Empty;
                            foreach (string preds in predecessorsArray)
                            {
                                List<DataPropert> PredecessorsRow = project.datapropert.Where(x => x.PMMID == Convert.ToInt32(preds)).ToList();
                                if (PredecessorsRow.Count > 0)
                                {
                                    if (Array.IndexOf(predecessorsArray, preds) > 0)
                                    {
                                        msPreds += ",";
                                    }
                                    msPreds += PredecessorsRow[0].TaskIndex;
                                }
                            }
                            taskRow.PredecessorsID = msPreds;
                        }

                    }
                }
                if (project.datapropert != null && project.datapropert.Count > 0)
                {
                    foreach (DataPropert taskRow in project.datapropert)
                    {
                        try
                        {

                            task = projectFile.GetTaskByUniqueID(java.lang.Integer.valueOf(Convert.ToString(taskRow.TaskIndex)));
                            if (Convert.ToString(taskRow.PredecessorsID) != string.Empty)
                            {

                                string[] predecessorsArray = Convert.ToString(taskRow.PredecessorsID).Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                                string msPreds = string.Empty;
                                foreach (string preds in predecessorsArray)
                                {
                                    net.sf.mpxj.Task t = projectFile.GetTaskByUniqueID(java.lang.Integer.valueOf(preds.Trim()));
                                    task.AddPredecessor(t, RelationType.FINISH_START, t.Duration);

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error updating predecessors during MS Project export");
                        }

                    }
                }
                TaskContainer taskCon = projectFile.Tasks;
                if (taskCon.Size() >= 1)
                {
                    java.io.OutputStream outs = null;
                    try
                    {
                        //  string serverPath = Server.MapPath(fileName);
                        if (System.IO.File.Exists(fullFileName))
                        {
                            System.IO.File.Delete(fullFileName);
                        }
                        MSPDIWriter writer = new MSPDIWriter();
                        outs = new java.io.FileOutputStream(fullFileName);
                        writer.Write(projectFile, outs);
                        outs.close();
                    }
                    catch (System.Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                    finally
                    {
                        if (outs != null)
                            outs.close();
                    }
                }
            }
        }

    }
}
