using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Web;
using System.Linq;
using System.IO;
using System.Reflection;
using uGovernIT.Manager;
using System.Xml;
using uGovernIT.Utility;
using Microsoft.Office;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.ImportExportMPP;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ImportExportTasks : UserControl
    {
        public string TicketId { get; set; }
        public string PublicTicketID { get; set; }
        public string moduleName { get; set; }
        private string lookupColumnName = string.Empty;
        private string parentList = string.Empty;
        string serverLocation = uHelper.GetTempFolderPath();
        public string ErrorMessage { get; set; }
        private bool msExportImport = false;
        UserProfile User;
        UserProfileManager UserManager;
        UGITTaskManager TaskManager;
        ConfigurationVariableManager ObjConfigVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        TicketManager ObjTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            User = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            try
            {
                if (UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.EnableProjectExportImport)))
                {
                    // Check the MSProject assembly is exist on client machine or not.
                    Assembly myAssembly = null;
                    msExportImport = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.MSProjectImportExportEnabled));
                    if (msExportImport)
                    {
                        // Check the MSProject assembly is exist on client machine or not.
                        // myAssembly = Assembly.Load("Microsoft.Office.Interop.MSProject, Version=14.0.0.0, Culture=Neutral, PublicKeyToken=71e9bce111e9429c");
                        if (myAssembly == null)
                            return;
                    }

                    string workItemType = string.Empty;
                    //if (moduleName == "NPR")
                    //{
                    //    lookupColumnName = DatabaseObjects.Columns.TicketNPRIdLookup;
                    //    parentList = DatabaseObjects.Tables.NPRTasks;
                    //    workItemType = Constants.RMMLevel1NPRProjectsType;
                    //}
                    //else if (moduleName == "PMM")
                    //{
                    //    lookupColumnName = DatabaseObjects.Columns.TicketPMMIdLookup;
                    //    parentList = DatabaseObjects.Tables.PMMTasks;
                    //    workItemType = Constants.RMMLevel1PMMProjectsType;
                    //}
                    //else if (moduleName == "TSK")
                    //{
                    //    lookupColumnName = DatabaseObjects.Columns.TSKIDLookup;
                    //    parentList = DatabaseObjects.Tables.TSKTasks;
                    //    workItemType = uHelper.GetModuleTitle(moduleName);
                    //}
                    lookupColumnName = DatabaseObjects.Columns.TicketId;
                    parentList = DatabaseObjects.Tables.ModuleTasks;
                    workItemType = moduleName;
                    if (TaskManager.IsModuleTasks(moduleName))
                    {
                        lookupColumnName = DatabaseObjects.Columns.TicketId;
                        parentList = DatabaseObjects.Tables.ModuleTasks;
                        workItemType = moduleName;
                    }
                }
                else { importExportButtonPnl.Visible = false; }
            }
            catch (Exception ex)
            {
                importExportButtonPnl.Visible = false;
                ULog.WriteException(ex);
            }
        }

        /// <summary>
        /// Functionality : This function calls the "ImportTaskList" method with Elevated privileges.
        /// </summary>
        /// <returns> void </returns>
        protected void ImportTasks(object sender, EventArgs e)
        {
            //Log.AuditTrail(SPContext.Current.Web.CurrentUser, string.Format("Importing from MS Project into project {0}", PublicTicketID), Request.Url);
            ImportTaskList();
            //SPSecurity.CodeToRunElevated elevatedGetSitesAndGroups = new SPSecurity.CodeToRunElevated(ImportTaskList);
            //SPSecurity.RunWithElevatedPrivileges(elevatedGetSitesAndGroups);
        }

        private void ImportTaskList()
        {
            if (!fileUpload.HasFile || fileUpload.PostedFile.ToString() == string.Empty)
                return; // No file chosen

            if (!fileUpload.FileName.EndsWith(".mpp", StringComparison.CurrentCultureIgnoreCase) && !fileUpload.FileName.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase))
            {
                ErrorMessage = "Invalid Format: please upload only .mpp or .xml file";
                return;
            }

            try
            {
                DataTable taskTable = null;
                string sourceFile = serverLocation + fileUpload.FileName;
                fileUpload.PostedFile.SaveAs(sourceFile);

                //if (msExportImport)
                taskTable = GetMPXJResult(sourceFile, chkImportDates.Checked, chkDontImportAssignee.Checked);
                //else
                //    taskTable = GetMPXJResult(sourceFile, chkImportDates.Checked, chkDontImportAssignee.Checked);

                if (taskTable != null)
                {
                    if (taskTable.Rows.Count >= 1)
                    {
                        UploadTask(taskTable, chkImportDates.Checked);
                        if (moduleName == "TSK" || moduleName == "PMM" || moduleName == "NPR" || TaskManager.IsModuleTasks(moduleName))
                        {
                            //TaskCache.ReloadProjectTasks(moduleName, PublicTicketID);

                            //User Allocation in RMM in TaskImport...
                            List<UGITTask> tasklist = TaskManager.LoadByProjectID(moduleName, PublicTicketID);
                            //ResourceAllocation.UpdateProjectPlannedAllocationByUser(tasklist, moduleName, PublicTicketID, true);
                        }
                    }
                }

                try
                {
                    // Open the file
                    FileStream fs = File.Open(sourceFile, FileMode.Open, FileAccess.Write, FileShare.None);

                    //close it and delete it immediately. 
                    fs.Close(); File.Delete(sourceFile);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "ERROR deleting imported project file");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error importing project file";
                ULog.WriteException(ex, ErrorMessage);
            }
        }

        private DataTable ImportTask(string strFileName, bool importDatesOnly, bool dontImportAssignee)
        {
            int rowId = 0;
            DataTable dtTarget = null;
            ArrayList tasks = new ArrayList();
            // Find and kill the process if  "MS-Project"process is already open.
            //FindAndKillProcess("WINPROJ");
            ConfigSetiingMpp configSettingMpp = new ConfigSetiingMpp();
            configSettingMpp.EnableExportImport = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.EnableProjectExportImport));
            configSettingMpp.UseMSProject = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.MSProjectImportExportEnabled));
            ImportManagerMpp managerMpp = new ImportManagerMpp(configSettingMpp, strFileName);
            Project projApp = managerMpp.importtask();

            if (projApp != null)
            {
                dtTarget = CreateTargetTable();
                double workingMinutePerDay = projApp.HoursPerDay * 60;
                foreach (Util.ImportExportMPP.TaskList task in projApp.taskList)
                {
                    if (task == null || task.Id == 0)
                    {
                        continue;
                    }

                    DataRow taskRow = dtTarget.NewRow();
                    //Copy the task value from tasks object t datatable.
                    taskRow["RowId"] = ++rowId;
                    if (TaskManager.IsModuleTasks(moduleName))
                    {

                        taskRow[DatabaseObjects.Columns.ModuleName] = uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName);
                    }

                    taskRow[DatabaseObjects.Columns.TicketNPRIdLookup] = TicketId;

                    taskRow[DatabaseObjects.Columns.Title] = null;
                    if (task.Name != null)
                        taskRow[DatabaseObjects.Columns.Title] = task.Name.Trim();
                    taskRow[DatabaseObjects.Columns.StartDate] = DateTime.Parse(task.Start.ToString());
                    taskRow[DatabaseObjects.Columns.DueDate] = DateTime.Parse(task.Finish.ToString());
                    try
                    {
                        taskRow[DatabaseObjects.Columns.Description] = task.Notes;
                    }
                    catch (Exception ex)
                    {
                        // Invalid characters in description
                        ULog.WriteException(ex, "Invalid task description for [" + task.Name + "]");
                    }

                    taskRow["DBId"] = 0;

                    taskRow[DatabaseObjects.Columns.ParentTask] = task.ParentTask;

                    taskRow[DatabaseObjects.Columns.Predecessors] = 0;
                    if (!importDatesOnly)
                    {
                        if (task.taskIds != null)
                            taskRow[DatabaseObjects.Columns.Predecessors] = string.Join(";", task.taskIds.ToArray());
                        if (task.taskDeps != null)
                        {
                            //taskRow["PredecessorExp"] = uHelper.SerializeObject(task.taskDeps).OuterXml;
                        }
                    }

                    double pctComplete = 0;
                    if (Double.TryParse(Convert.ToString(task.PercentComplete), out pctComplete))
                        taskRow[DatabaseObjects.Columns.PercentComplete] = pctComplete / 100;

                    bool isMilestone = UGITUtility.StringToBoolean(task.Milestone);
                    if (isMilestone)
                        taskRow[DatabaseObjects.Columns.TaskBehaviour] = Constants.TaskType.Milestone;

                    // Use estimated work in MSP for estimated hours unless no resources or milestone
                    // estimateHours is required for userallocation using task.work we getting worng hours.
                    taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = 0;
                    int totalDurationHours = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), Convert.ToDateTime(task.Start), Convert.ToDateTime(task.Finish)) * uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext());
                    if (!string.IsNullOrEmpty(task.ResourceNames) && !isMilestone)
                        taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = totalDurationHours;

                    taskRow[DatabaseObjects.Columns.UGITDuration] = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(task.Duration)))
                        taskRow[DatabaseObjects.Columns.UGITDuration] = Math.Round(Convert.ToDouble(task.Duration) / workingMinutePerDay, 2);

                    taskRow[DatabaseObjects.Columns.ItemOrder] = rowId;

                    // Find and set assigned resources
                    taskRow[DatabaseObjects.Columns.AssignedTo] = string.Empty;
                    if (!dontImportAssignee && !string.IsNullOrEmpty(task.ResourceNames))
                    {
                        try
                        {
                            //string strMainAssignToPct = task.ResourceNames.Trim().Replace(",","").Replace("[", Constants.Separator1).Replace("%]", Constants.Separator);
                            string[] assingToPct = task.ResourceNames.Trim().Split(',');
                            string strAssignToPct = string.Empty;
                            List<UserProfile> userCollection = new List<UserProfile>();
                            double estimateHours = 0.0;
                            foreach (string itemUser in assingToPct)
                            {
                                string strMainAssignToPct = itemUser.Trim().Replace("[", Constants.Separator1).Replace("%]", Constants.Separator);
                                List<UGITAssignTo> listAssignTo = TaskManager.GetUGITAssignPct(strMainAssignToPct);

                                foreach (UGITAssignTo item in listAssignTo)
                                {
                                    estimateHours += (Convert.ToDouble(item.Percentage) / 100D) * totalDurationHours;
                                    UserProfile user = UserManager.GetUserByUserName(item.LoginName);  //.GetUserByName(item.LoginName, SPPrincipalType.User);
                                    if (user != null)
                                    {
                                        if (!string.IsNullOrEmpty(strAssignToPct))
                                            strAssignToPct += Constants.Separator;
                                        strAssignToPct += user.UserName + Constants.Separator1 + item.Percentage;

                                        UserProfile userFValue = UserManager.GetUserById(user.Id); //new SPFieldUserValue(SPContext.Current.Web, user.ID, user.LoginName);
                                        if (userFValue != null)
                                            userCollection.Add(userFValue);
                                    }
                                }
                            }

                            taskRow[DatabaseObjects.Columns.UGITAssignToPct] = strAssignToPct;
                            taskRow[DatabaseObjects.Columns.AssignedTo] = userCollection;

                            // Use estimated work in MSP for estimated hours unless no resources or milestone
                            // estimateHours is required for userallocation using task.work we getting worng hours.
                            taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = 0;
                            if (!isMilestone)
                            {
                                taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = estimateHours;
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error finding user: " + task.ResourceNames.Trim());
                        }
                    }

                    dtTarget.Rows.Add(taskRow);
                    tasks.Add(task);
                }
                dtTarget.AcceptChanges();
                //}
                // }

                //projApp.FileCloseEx(Microsoft.Office.Interop.MSProject.PjSaveType.pjDoNotSave, Type.Missing, Type.Missing);
                //}
                //else
                //{
                //    string retVal = "The MS Project file " + strFileName + " could not be opened.";
                //}
            }

            //Stop the process that is opened above.
            FindAndKillProcess("WINPROJ");
            return dtTarget;
        }

        private DataTable GetMPXJResult(string strFileName, bool importDatesOnly, bool dontImportAssignee)
        {
            int rowId = 0;
            DataTable dtTarget = null;
            ArrayList tasks = new ArrayList();
            ConfigSetiingMpp configSettingMpp = new ConfigSetiingMpp();
            configSettingMpp.EnableExportImport = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.EnableProjectExportImport));
            configSettingMpp.UseMSProject = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.MSProjectImportExportEnabled));
            configSettingMpp.importDatesOnly = importDatesOnly;
            configSettingMpp.dontImportAssignee = dontImportAssignee;
            ImportManagerMpp managerMpp = new ImportManagerMpp(configSettingMpp, strFileName);
            Project projApp = managerMpp.importtask();

            // Create table schema
            dtTarget = CreateTargetTable();
            if (projApp != null)
            {
                foreach (Util.ImportExportMPP.TaskList task in projApp.taskList)
                {
                    DataRow taskRow = dtTarget.NewRow();
                    //Copy the task value from tasks object t datatable.
                    taskRow["RowId"] = ++rowId;
                    if (TaskManager.IsModuleTasks(moduleName))
                    {
                        taskRow[DatabaseObjects.Columns.ModuleName] = uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName);
                    }

                    taskRow[DatabaseObjects.Columns.TicketNPRIdLookup] = TicketId;
                    taskRow[DatabaseObjects.Columns.Title] = string.Empty;
                    if (task.Name != null)
                        taskRow[DatabaseObjects.Columns.Title] = task.Name.Trim();

                    DateTime startdate = DateTime.MinValue;
                    DateTime finishdate = DateTime.MinValue;
                    if (task.Start != null)
                        startdate = task.Start;
                    if (task.Finish != null)
                        finishdate = task.Finish;

                    taskRow[DatabaseObjects.Columns.StartDate] = startdate;
                    taskRow[DatabaseObjects.Columns.DueDate] = finishdate;

                    try
                    {
                        taskRow[DatabaseObjects.Columns.Description] = task.Notes;
                    }
                    catch (Exception ex)
                    {
                        // Invalid characters in description
                        ULog.WriteException(ex, "Invalid task description for [" + task.Name + "]");
                    }

                    taskRow["DBId"] = 0;
                    taskRow[DatabaseObjects.Columns.ParentTask] = task.ParentTask != null ? Convert.ToString(task.ParentTask) : "0";
                    taskRow[DatabaseObjects.Columns.Predecessors] = 0;
                    if (!importDatesOnly)
                    {
                        if (task.taskIds != null)
                            taskRow[DatabaseObjects.Columns.Predecessors] = string.Join(";", task.taskIds.ToArray());
                        if (task.taskDependencyList != null)
                        {
                            taskRow["PredecessorExp"] = uHelper.SerializeObject(task.taskDependencyList).OuterXml;
                        }
                    }

                    double pctComplete = 0;
                    if (Double.TryParse(Convert.ToString(task.PercentageComplete), out pctComplete))
                        taskRow[DatabaseObjects.Columns.PercentComplete] = pctComplete / 100;

                    bool isMilestone = UGITUtility.StringToBoolean(task.Milestone);
                    if (isMilestone)
                        taskRow[DatabaseObjects.Columns.TaskBehaviour] = Constants.TaskType.Milestone;

                    // Use estimated work in MSP for estimated hours unless no resources or milestone
                    // estimateHours is required for userallocation using task.work we getting worng hours.
                    taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = 0;
                    int totalDurationHours = uHelper.GetTotalWorkingDaysBetween(HttpContext.Current.GetManagerContext(), startdate, finishdate) * uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext());
                    if (task.ResourceAssignments == 0 && !isMilestone)
                        taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = totalDurationHours;

                    taskRow[DatabaseObjects.Columns.UGITDuration] = 0;


                    if (!string.IsNullOrEmpty(Convert.ToString(task.Duration)))
                    {
                        taskRow[DatabaseObjects.Columns.UGITDuration] = Math.Round(Convert.ToDouble(task.Duration) / task.workingMinutePerDay, 2);
                    }

                    taskRow[DatabaseObjects.Columns.ItemOrder] = rowId;

                    // Find and set assigned resources
                    taskRow[DatabaseObjects.Columns.AssignedTo] = string.Empty;
                    if (!dontImportAssignee && task.ResourceAssignments > 0)
                    {
                        try
                        {
                            double estimateHours = 0.0;
                            string strAssignToPct = string.Empty;
                            List<UserProfile> userCollection = new List<UserProfile>();
                            List<UGITAssignTo> listAssignTo = TaskManager.GetUGITAssignPct(task.strMainAssignToPct);
                            foreach (UGITAssignTo item in listAssignTo)
                            {
                                estimateHours += (Convert.ToDouble(item.Percentage) / 100D) * totalDurationHours;
                                UserProfile user = UserManager.GetUserByUserName(item.LoginName); //.GetUserByName(item.LoginName, SPPrincipalType.User);
                                if (user == null)
                                {
                                    //try to get it using display name
                                    string displayName = item.LoginName.Split('\\').Last();
                                    user = UserManager.GetUserByUserName(displayName); // UserProfile.GetUserByName(displayName, SPPrincipalType.User);
                                }

                                if (user != null)
                                {
                                    if (!string.IsNullOrEmpty(strAssignToPct))
                                        strAssignToPct += Constants.Separator;
                                    strAssignToPct += user.Id + Constants.Separator1 + item.Percentage;

                                    UserProfile userFValue = UserManager.GetUserByUserName(user.UserName); // new SPFieldUserValue(SPContext.Current.Web, user.ID, user.UserName);
                                    if (userFValue != null)
                                        userCollection.Add(userFValue);
                                }
                            }
                            // }

                            taskRow[DatabaseObjects.Columns.UGITAssignToPct] = strAssignToPct;
                            taskRow[DatabaseObjects.Columns.AssignedTo] = string.Join(";", userCollection.Select(x => x.Id).ToList());

                            // Use estimated work in MSP for estimated hours unless no resources or milestone
                            // estimateHours is required for userallocation using task.work we getting worng hours.
                            taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = 0;
                            if (!isMilestone)
                            {
                                taskRow[DatabaseObjects.Columns.TaskEstimatedHours] = estimateHours;
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error finding user: " + task.ResourceNames.Trim());
                        }
                    }

                    dtTarget.Rows.Add(taskRow);
                    tasks.Add(task);
                    // }
                    dtTarget.AcceptChanges();

                }
            }
            return dtTarget;
        }

        /// <summary>
        /// Functinality : This function uploads task from table to list.
        /// Date: 06/29/2017
        /// Author: Manish Kumar
        /// </summary>
        /// <returns> DataTable </returns>
        private void UploadTask(DataTable taskTable, bool importDateOnly)
        {
            DataTable taskList = UGITUtility.ToDataTable<UGITTask>(TaskManager.LoadByProjectID(moduleName, TicketId)); // SPContext.Current.Web.Lists[parentList];
            //SPQuery rQuery = new SPQuery();


            // get all the tasks for a ticketId from the list.
            List<UGITTask> projectTasks = TaskManager.LoadByProjectID(moduleName, TicketId);
            if (projectTasks != null && projectTasks.Count > 0)
            {
                TaskManager.DeleteTasks(moduleName, projectTasks);
            }

            // Add all the tasks from table to list.
            Dictionary<int, string> parentTaskWithAssignment = new Dictionary<int, string>();
            //taskTable.AsEnumerable().Where(x=>x.Field<int>(DatabaseObjects.Columns.ParentTask) == 
            foreach (DataRow taskRow in taskTable.Rows)
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(taskRow[DatabaseObjects.Columns.Title])))
                {
                    //project file possibly due to blank lines
                    taskRow["DBId"] = "0";
                    continue;
                }

                // DataRow taskItem = taskList.NewRow(); //.AddItem();
                UGITTask taskItem = new UGITTask();
                if (TaskManager.IsModuleTasks(moduleName))
                    taskItem.TicketId = PublicTicketID;
                else
                    taskItem.TicketId = TicketId;

                taskItem.ModuleNameLookup = moduleName;
                taskItem.Title = Convert.ToString(taskRow[DatabaseObjects.Columns.Title]);

                DateTime startDate = (DateTime)taskRow[DatabaseObjects.Columns.StartDate];
                DateTime dueDate = (DateTime)taskRow[DatabaseObjects.Columns.StartDate];

                taskItem.StartDate = new DateTime(1800, 1, 1);
                taskItem.DueDate = new DateTime(1800, 1, 1);

                if (startDate != DateTime.MinValue)
                    taskItem.StartDate = Convert.ToDateTime(taskRow[DatabaseObjects.Columns.StartDate]);
                if (dueDate != DateTime.MinValue)
                    taskItem.DueDate = Convert.ToDateTime(taskRow[DatabaseObjects.Columns.DueDate]);

                //if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ModuleNameLookup, taskList))
                //    taskItem.ModuleName = Convert.ToString(taskRow[DatabaseObjects.Columns.ModuleName]);

                int parentId = 0;
                int.TryParse(Convert.ToString(taskRow[DatabaseObjects.Columns.ParentTask]), out parentId);
                if (parentId > 0)
                {
                    var parentItem = taskTable.AsEnumerable().FirstOrDefault(x => x.Field<int>("RowId") == parentId);
                    if (parentItem != null)
                    {
                        int parentDBId = UGITUtility.StringToInt(parentItem["DBID"]);
                        if (parentDBId == 0) // Shdn't be zero for valid projects, but check just in case
                        {
                            ErrorMessage = "ERROR importing project file possibly due to blank lines";
                            Util.Log.ULog.WriteLog(ErrorMessage);
                            return;
                        }
                        if (taskList != null && taskList.Rows.Count > 0)
                        {

                            //DataRow parentTaskItem = taskList.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.ID, Convert.ToString(parentDBId)))[0]; //.GetItemById(parentDBId);
                            //if (parentTaskItem != null)
                            //{
                            //parentTaskItem[DatabaseObjects.Columns.UGITAssignToPct] = string.Empty;
                            //parentTaskItem[DatabaseObjects.Columns.AssignedTo] = string.Empty;
                            //}
                        }

                        //parentTaskItem.Update();
                    }
                }
                if (parentId > 0 && Convert.ToString(taskRow[DatabaseObjects.Columns.AssignedTo]) == string.Empty)
                {

                    string assigntopct = GetSummaryAssignToPctValue(taskRow, parentId, taskTable);
                    if (assigntopct != string.Empty)
                    {
                        taskItem.AssignToPct = assigntopct;
                    }
                }
                else
                {
                    List<UserProfile> assignees = UserManager.GetUserInfosById(Convert.ToString(taskRow[DatabaseObjects.Columns.AssignedTo])); // new (SPContext.Current.Web, Convert.ToString(taskRow[DatabaseObjects.Columns.AssignedTo]));
                    taskItem.AssignedTo = Convert.ToString(taskRow[DatabaseObjects.Columns.AssignedTo]); // assignees;
                    taskItem.AssignToPct = Convert.ToString(taskRow[DatabaseObjects.Columns.UGITAssignToPct]);
                }

                taskItem.ParentTaskID = Convert.ToInt32(taskRow[DatabaseObjects.Columns.ParentTask]);
                taskItem.Description = Convert.ToString(taskRow[DatabaseObjects.Columns.Description]);
                taskItem.Duration = Convert.ToDouble(taskRow[DatabaseObjects.Columns.UGITDuration]);

                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TaskEstimatedHours, taskList))
                    taskItem.EstimatedHours = Convert.ToDouble(taskRow[DatabaseObjects.Columns.TaskEstimatedHours]);

                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TaskActualHours, taskList))
                    taskItem.ActualHours = 0;

                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.EstimatedRemainingHours, taskList))
                    taskItem.EstimatedRemainingHours = Convert.ToDouble(taskRow[DatabaseObjects.Columns.TaskEstimatedHours]);

                int taskOrder = 0;
                int.TryParse(Convert.ToString(taskRow[DatabaseObjects.Columns.ItemOrder]), out taskOrder);
                taskItem.ItemOrder = taskOrder;

                // Update status field based on % complete
                double pctComplete = UGITUtility.StringToDouble(taskRow[DatabaseObjects.Columns.PercentComplete]);
                taskItem.PercentComplete = pctComplete;
                if (pctComplete >= 1.0)
                    taskItem.Status = Constants.Completed;
                else if (pctComplete > 0)
                    taskItem.Status = Constants.InProgress;
                else
                    taskItem.Status = Constants.NotStarted;

                taskItem.Behaviour = Convert.ToString(taskRow[DatabaseObjects.Columns.TaskBehaviour]);
                TaskManager.SaveTask(ref taskItem, moduleName, PublicTicketID);
                taskRow["DBId"] = taskItem.ID;

            }
            //foreach (DataRow taskRow in taskTable.Rows)
            //{
            //    if (Convert.ToString(taskRow[DatabaseObjects.Columns.Predecessors]) != string.Empty)
            //    {
            //        List<string> predecessors = UGITUtility.ConvertStringToList(Convert.ToString(taskRow[DatabaseObjects.Columns.Predecessors]), ";");
            //        List<string> newPreTaskIds = new List<string>();
            //        foreach (string preTaskID in predecessors)
            //        {
            //            DataRow[] PredecessorsRow = taskTable.Select(string.Format("RowId={0}", preTaskID));
            //            if (PredecessorsRow.Length > 0)
            //            {
            //                newPreTaskIds.Add(Convert.ToString(PredecessorsRow[0]["DBId"]));
            //            }
            //        }
            //        taskRow[DatabaseObjects.Columns.Predecessors] = string.Join(";", newPreTaskIds.ToArray());
            //        //taskItem.Predecessors= string.Join(";", newPreTaskIds.ToArray());
            //        //change index to with taskID
            //        if (Convert.ToString(taskRow["PredecessorExp"]) != string.Empty)
            //        {
            //            List<TaskDependency> taskDeps = new List<TaskDependency>();
            //            XmlDocument xmlDoc = new XmlDocument();
            //            xmlDoc.LoadXml(Convert.ToString(taskRow["PredecessorExp"]));
            //            taskDeps = (List<TaskDependency>)uHelper.DeSerializeAnObject(xmlDoc, taskDeps);
            //            foreach (TaskDependency dp in taskDeps)
            //            {
            //                DataRow[] predecessorsRow = taskTable.Select(string.Format("RowId={0}", dp.ParenID));
            //                if (predecessorsRow.Length > 0)
            //                    dp.ParenID = Convert.ToInt32(predecessorsRow[0]["DBId"]);

            //                predecessorsRow = taskTable.Select(string.Format("RowId={0}", dp.FromID));
            //                if (predecessorsRow.Length > 0)
            //                    dp.FromID = Convert.ToInt32(predecessorsRow[0]["DBId"]);

            //                predecessorsRow = taskTable.Select(string.Format("RowId={0}", dp.ToID));
            //                if (predecessorsRow.Length > 0)
            //                    dp.ToID = Convert.ToInt32(predecessorsRow[0]["DBId"]);
            //            }
            //            taskRow["PredecessorExp"] = uHelper.SerializeObject(taskDeps).OuterXml;
            //        }
            //    }

            //    if (Convert.ToString(taskRow[DatabaseObjects.Columns.ParentTask]) != string.Empty)
            //    {
            //        DataRow[] parentRows = taskTable.Select(string.Format("RowId={0}", taskRow[DatabaseObjects.Columns.ParentTask]));
            //        if (parentRows.Length > 0)
            //        {
            //            taskRow[DatabaseObjects.Columns.ParentTask] = parentRows[0]["DBId"];
                        
            //        }
            //    }
            //}

            //update Predecessors id, ParentTask in temp table
            foreach (DataRow taskRow in taskTable.Rows)
            {
                if (Convert.ToString(taskRow[DatabaseObjects.Columns.Predecessors]) != string.Empty)
                {
                    List<string> predecessors = UGITUtility.ConvertStringToList(Convert.ToString(taskRow[DatabaseObjects.Columns.Predecessors]), ";");
                    List<string> newPreTaskIds = new List<string>();
                    foreach (string preTaskID in predecessors)
                    {
                        DataRow[] PredecessorsRow = taskTable.Select(string.Format("RowId={0}", preTaskID));
                        if (PredecessorsRow.Length > 0)
                        {
                            newPreTaskIds.Add(Convert.ToString(PredecessorsRow[0]["DBId"]));
                        }
                    }
                    taskRow[DatabaseObjects.Columns.Predecessors] = string.Join(";", newPreTaskIds.ToArray());

                    //change index to with taskID
                    if (Convert.ToString(taskRow["PredecessorExp"]) != string.Empty)
                    {
                        List<TaskDependency> taskDeps = new List<TaskDependency>();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(Convert.ToString(taskRow["PredecessorExp"]));
                        taskDeps = (List<TaskDependency>)uHelper.DeSerializeAnObject(xmlDoc, taskDeps);
                        foreach (TaskDependency dp in taskDeps)
                        {
                            DataRow[] predecessorsRow = taskTable.Select(string.Format("RowId={0}", dp.ParenID));
                            if (predecessorsRow.Length > 0)
                                dp.ParenID = Convert.ToInt32(predecessorsRow[0]["DBId"]);

                            predecessorsRow = taskTable.Select(string.Format("RowId={0}", dp.FromID));
                            if (predecessorsRow.Length > 0)
                                dp.FromID = Convert.ToInt32(predecessorsRow[0]["DBId"]);

                            predecessorsRow = taskTable.Select(string.Format("RowId={0}", dp.ToID));
                            if (predecessorsRow.Length > 0)
                                dp.ToID = Convert.ToInt32(predecessorsRow[0]["DBId"]);
                        }
                        taskRow["PredecessorExp"] = uHelper.SerializeObject(taskDeps).OuterXml;
                    }
                }

                if (Convert.ToString(taskRow[DatabaseObjects.Columns.ParentTask]) != string.Empty)
                {
                    DataRow[] parentRows = taskTable.Select(string.Format("RowId={0}", taskRow[DatabaseObjects.Columns.ParentTask]));
                    if (parentRows.Length > 0)
                    {
                        taskRow[DatabaseObjects.Columns.ParentTask] = parentRows[0]["DBId"];
                    }
                }
            }

            //update Predecessors in database
            //DataRow[] newTaskCollection = taskList.Select(); // taskList.GetItems(rQuery);
            List<UGITTask> newTaskCollection= TaskManager.LoadByProjectID(moduleName, TicketId);
            foreach (DataRow taskRow in taskTable.Rows)
            {
                try
                {
                    UGITTask item= newTaskCollection.AsEnumerable().FirstOrDefault(x => x.ID == Convert.ToInt32(taskRow["DBId"])); // SPListHelper.GetItemByID(newTaskCollection, Convert.ToInt32(taskRow["DBId"]));
                   // DataRow item = newTaskCollection.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.ID) == Convert.ToInt32(taskRow["DBId"])); // SPListHelper.GetItemByID(newTaskCollection, Convert.ToInt32(taskRow["DBId"]));
                    if (item != null)
                    {
                        int parentTaskId = Convert.ToInt32(taskRow[DatabaseObjects.Columns.ParentTask]);
                        item.ParentTaskID = parentTaskId;

                        List<string> predecessors = UGITUtility.ConvertStringToList(Convert.ToString(taskRow[DatabaseObjects.Columns.Predecessors]), ";");
                        if (predecessors.Count > 0)
                        {
                            List<string> lookups = predecessors;
                            //SPFieldLookupValueCollection lookups = new SPFieldLookupValueCollection();
                            //foreach (string preTask in predecessors)
                            //{
                            //    SPFieldLookupValue lk = new SPFieldLookupValue();
                            //    lk.LookupId = Convert.ToInt32(preTask);
                            //    lookups.Add(lk);
                            //}
                            item.Predecessors = string.Join(",", lookups);

                            List<TaskDependency> taskDeps = new List<TaskDependency>();
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(Convert.ToString(taskRow["PredecessorExp"]));
                            taskDeps = (List<TaskDependency>)uHelper.DeSerializeAnObject(xmlDoc, taskDeps);

                            //if task contains complex predecessor then remove predecessor from task and save import information as comment
                            bool hasComplexDep = false;
                            foreach (TaskDependency tskDp in taskDeps)
                            {
                                //if (tskDp.ToID == Convert.ToInt32(item.ID) && (tskDp.Lag != 0 || tskDp.Type != ProjectTaskLinkType.pjFinishToStart))
                                //{
                                //    hasComplexDep = true;
                                //    break;
                                //}
                            }

                            if (hasComplexDep)
                            {
                                item.Predecessors = null;
                                item.Comment = UGITUtility.GetVersionString(User.UserName, Convert.ToString(taskRow[DatabaseObjects.Columns.UGITComment]), DatabaseObjects.Columns.UGITComment);
                            }
                        }
                        TaskManager.Update(item);


                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Error updating predecessors during MS Project import");
                }
            }

            //Creats tasks object list to calculates some properties like, level, duration, contribution, childcount
            List<UGITTask> tasks = TaskManager.LoadByProjectID(moduleName, PublicTicketID);
            if (tasks != null && tasks.Count > 0)
            {
                tasks = UGITTaskManager.MapRelationalObjects(tasks);

                // Recalculate duration based on system working hours
                TaskManager.CalculateDuration(ref tasks, false);

                // Recalculate estimated hours from duration only IF zero and for unassigned tasks only!
                if (ObjConfigVariableHelper.GetValueAsBool(ConfigConstants.AllowZeroHoursOnImport) == false)
                    TaskManager.CalculateEstimatedHours(ref tasks, true);

                TaskManager.ReOrderTasks(ref tasks);
                tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            }

            TaskManager.SaveTasks(ref tasks, moduleName, PublicTicketID);


            //update project properties which are related to tasks
            DataRow ticketItem = ObjTicketManager.GetTicketTableBasedOnTicketId(moduleName, TicketId).Rows[0];
            TaskManager.CalculateProjectStartEndDate(moduleName, tasks, ticketItem);
            //ticketItem.UpdateOverwriteVersion();
        }

        private string GetSummaryAssignToValue(DataRow taskRow, int parentId, DataTable taskTable)
        {
            var parentTask = taskTable.AsEnumerable().FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.ParentTask) == parentId);
            if (Convert.ToInt32(parentTask[DatabaseObjects.Columns.ParentTask]) > 0 && Convert.ToString(parentTask[DatabaseObjects.Columns.UGITAssignedTo]) == string.Empty)
            {
                int.TryParse(Convert.ToString(parentTask[DatabaseObjects.Columns.ParentTask]), out parentId);
                return GetSummaryAssignToValue(parentTask, parentId, taskTable);
            }
            else
            {
                return Convert.ToString(parentTask[DatabaseObjects.Columns.UGITAssignedTo]);
            }
        }

        private string GetSummaryAssignToPctValue(DataRow taskRow, int parentId, DataTable taskTable)
        {
            //double pId = Convert.ToDouble(parentId);
            var parentTask = taskTable.AsEnumerable().FirstOrDefault(x => x.Field<int>("RowId") == parentId);
            if (Convert.ToInt32(parentTask[DatabaseObjects.Columns.ParentTask]) > 0 && Convert.ToString(parentTask[DatabaseObjects.Columns.UGITAssignToPct]) == string.Empty)
            {
                int.TryParse(Convert.ToString(parentTask[DatabaseObjects.Columns.ParentTask]), out parentId);
                return GetSummaryAssignToPctValue(parentTask, parentId, taskTable);
            }
            else
            {
                return Convert.ToString(parentTask[DatabaseObjects.Columns.UGITAssignToPct]);
            }
        }

        /// <summary>
        /// Functinality : This function calls the "ExportList" method with Evalutaed privileges.
        /// </summary>
        /// <returns> void </returns>
        protected void ExportTasks(object sender, EventArgs e)
        {
            //Log.AuditTrail(SPContext.Current.Web.CurrentUser, string.Format("exporting tasks from project {0}", PublicTicketID), Request.Url);

            try
            {
                //SPSecurity.CodeToRunElevated elevatedGetSitesAndGroups;
                //if (msExportImport)
                //   // ExportList(); //elevatedGetSitesAndGroups = new SPSecurity.CodeToRunElevated(ExportList);
                //else
                MPXJExportTask(); //elevatedGetSitesAndGroups = new SPSecurity.CodeToRunElevated(MPXJExportTask);

                //SPSecurity.RunWithElevatedPrivileges(elevatedGetSitesAndGroups);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error exporting MS Project file";
                ULog.WriteException(ex, ErrorMessage);
                return;
            }
        }
        //private void ExportList()
        //{
        //    string fileName = "ProjectTasks_" + PublicTicketID + ".mpp";
        //    string fullFileName = serverLocation + fileName;

        //    DataTable tasks = TaskManager.LoadTasksTable(moduleName, false, PublicTicketID);

        //    if (tasks != null && tasks.Rows.Count > 0)
        //    {
        //        // kill the process if any instance is already open.
        //        FindAndKillProcess("WINPROJ");

        //        // Create the new instance of MS-Project application.
        //        Microsoft.Office.Interop.MSProject.ApplicationClass projectApp = new Microsoft.Office.Interop.MSProject.ApplicationClass();

        //        // create a new file in Ms-Proj application.
        //        projectApp.FileNew(Type.Missing, Type.Missing, Type.Missing, Type.Missing);

        //        // create an active project in file.
        //        Microsoft.Office.Interop.MSProject.Project project = projectApp.ActiveProject;
        //        Microsoft.Office.Interop.MSProject.Task task;

        //        double workingMinutePerDay = project.HoursPerDay * 60;

        //        DataTable tasksTable = new DataTable();
        //        tasksTable.Columns.Add("PMMID", typeof(int));
        //        tasksTable.Columns.Add(DatabaseObjects.Columns.PredecessorsID);
        //        tasksTable.Columns.Add("TaskIndex", typeof(int));

        //        // store all the tasks in task object
        //        foreach (DataRow taskRow in tasks.Rows)
        //        {
        //            string title = UGITUtility.StripHTML(Convert.ToString(taskRow[DatabaseObjects.Columns.Title]));
        //            task = project.Tasks.Add(title, Type.Missing);



        //            task.Start = Convert.ToDateTime(taskRow[DatabaseObjects.Columns.StartDate]);
        //            //  task.Finish = Convert.ToDateTime(taskRow[DatabaseObjects.Columns.DueDate]);

        //            //comment duration block of code becasue in project file is autocalculated if we set duration here is change the finish date task. 
        //            //double duration = 0;
        //            //if (!string.IsNullOrEmpty(Convert.ToString(taskRow[DatabaseObjects.Columns.UGITDuration])))
        //            //    duration = Convert.ToDouble(Convert.ToString(taskRow[DatabaseObjects.Columns.UGITDuration])) * workingMinutePerDay;
        //            //task.Duration = duration;

        //            double work = 0;
        //            if (!string.IsNullOrEmpty(Convert.ToString(taskRow[DatabaseObjects.Columns.TaskEstimatedHours])))
        //                work = Convert.ToDouble(Convert.ToString(taskRow[DatabaseObjects.Columns.TaskEstimatedHours])) * 60;
        //            task.Work = work;

        //            task.Notes = Convert.ToString(taskRow[DatabaseObjects.Columns.Description]);

        //            //new line for code for Export/Import task.
        //            if (UGITUtility.IsSPItemExist(taskRow, DatabaseObjects.Columns.UGITAssignToPct))
        //            {
        //                List<UGITAssignTo> listAssignTo = TaskManager.GetUGITAssignPct(Convert.ToString(taskRow[DatabaseObjects.Columns.UGITAssignToPct]));
        //                string strAssignToPct = string.Empty;
        //                foreach (var item in listAssignTo)
        //                {
        //                    UserProfile user = UserManager.GetUserByUserName(item.LoginName); //.GetUserByName(item.LoginName, SPPrincipalType.User);
        //                    if (user != null)
        //                    {
        //                        if (!string.IsNullOrEmpty(strAssignToPct))
        //                            strAssignToPct += Constants.UserInfoSeparator;
        //                        strAssignToPct += user.UserName + "[" + item.Percentage + "%]";
        //                    }
        //                }
        //                task.ResourceNames = strAssignToPct.Replace(";", ",");
        //            }

        //            if (tasks.Columns.Contains(DatabaseObjects.Columns.PercentComplete))
        //            {
        //                task.PercentComplete = Convert.ToString(taskRow[DatabaseObjects.Columns.PercentComplete]);
        //            }

        //            if (tasks.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour) && Convert.ToString(taskRow[DatabaseObjects.Columns.TaskBehaviour]) == Constants.TaskType.Milestone)
        //                task.Milestone = true;
        //            else
        //                task.Milestone = false;

        //            task.OutlineLevel = Convert.ToInt16(Convert.ToInt16(taskRow[DatabaseObjects.Columns.UGITLevel]) + 1);
        //            task.Finish = Convert.ToDateTime(taskRow[DatabaseObjects.Columns.DueDate]);
        //            DataRow row = tasksTable.NewRow();
        //            row["PMMID"] = Convert.ToInt32(taskRow[DatabaseObjects.Columns.Id]);
        //            row[DatabaseObjects.Columns.PredecessorsID] = UGITUtility.GetSPItemValue(taskRow, DatabaseObjects.Columns.PredecessorsID);
        //            row["TaskIndex"] = task.Index;
        //            tasksTable.Rows.Add(row);
        //        }

        //        if (tasksTable != null && tasksTable.Rows.Count > 0)
        //        {
        //            foreach (DataRow taskRow in tasksTable.Rows)
        //            {
        //                if (Convert.ToString(taskRow[DatabaseObjects.Columns.PredecessorsID]) != string.Empty)
        //                {
        //                    string[] predecessorsArray = Convert.ToString(taskRow[DatabaseObjects.Columns.PredecessorsID]).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        //                    string msPreds = string.Empty;
        //                    foreach (string preds in predecessorsArray)
        //                    {
        //                        DataRow[] PredecessorsRow = tasksTable.Select(string.Format("PMMID={0}", preds.Trim()));
        //                        if (PredecessorsRow.Length > 0)
        //                        {
        //                            if (Array.IndexOf(predecessorsArray, preds) > 0)
        //                            {
        //                                msPreds += ",";
        //                            }
        //                            msPreds += PredecessorsRow[0]["TaskIndex"];
        //                        }
        //                    }
        //                    taskRow[DatabaseObjects.Columns.PredecessorsID] = msPreds;
        //                }

        //            }
        //        }

        //        if (tasksTable != null && tasksTable.Rows.Count > 0)
        //        {
        //            foreach (DataRow taskRow in tasksTable.Rows)
        //            {
        //                try
        //                {
        //                    task = project.Tasks.get_UniqueID(Convert.ToInt32(taskRow["TaskIndex"]));
        //                    if (Convert.ToString(taskRow[DatabaseObjects.Columns.PredecessorsID]) != string.Empty)
        //                    {
        //                        string[] predecessorsArray = Convert.ToString(taskRow[DatabaseObjects.Columns.PredecessorsID]).Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
        //                        string msPreds = string.Empty;
        //                        foreach (string preds in predecessorsArray)
        //                        {
        //                            Microsoft.Office.Interop.MSProject.Task parentTask = project.Tasks.get_UniqueID(Convert.ToInt32(preds.Trim()));
        //                            task.LinkPredecessors(parentTask, Microsoft.Office.Interop.MSProject.PjTaskLinkType.pjFinishToStart, Type.Missing);
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    //Log.WriteException(ex, "Error updating predecessors during MS Project export");
        //                }
        //            }
        //        }

        //        if (project.Tasks.Count >= 1)
        //        {
        //            try
        //            {
        //                // string serverPath = Server.MapPath(fileName);
        //                if (System.IO.File.Exists(fullFileName))
        //                {
        //                    System.IO.File.Delete(fullFileName);
        //                }

        //                projectApp.FileSaveAs(fullFileName, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //                projectApp.DocClose();

        //                if (System.IO.File.Exists(fullFileName))
        //                {
        //                    DownloadFile(fullFileName, fileName);
        //                }
        //            }
        //            catch (System.Exception ex)
        //            {
        //                Util.Log.ULog.WriteException(ex);
        //            }

        //        }

        //        // stop the process if it is open.
        //        FindAndKillProcess("WINPROJ");
        //    }
        //}

        private void MPXJExportTask()
        {
            string fileName = "ProjectTasks_" + PublicTicketID + ".xml";
            string fullFileName = serverLocation + fileName;
            Project project = new Project();
            DataTable tasks = TaskManager.LoadTasksTable(moduleName, false, PublicTicketID);
            if (tasks != null && tasks.Rows.Count > 0)
            {
                DataRow[] arrColl = tasks.AsEnumerable().OrderBy(x =>UGITUtility.StringToInt(x["ItemOrder"])).ToArray();
                if (arrColl != null && arrColl.Length > 0)
                    tasks = arrColl.CopyToDataTable();
            }
            ConfigSetiingMpp configSettingMpp = new ConfigSetiingMpp();
            configSettingMpp.EnableExportImport = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.EnableProjectExportImport));
            configSettingMpp.UseMSProject = UGITUtility.StringToBoolean(ObjConfigVariableHelper.GetValue(DatabaseObjects.Columns.MSProjectImportExportEnabled));
            project.datapropert = new List<Util.ImportExportMPP.DataPropert>();
            Util.ImportExportMPP.TaskList task;
            if (tasks != null && tasks.Rows.Count > 0)
            {

                project.taskList = new List<Util.ImportExportMPP.TaskList>();
                foreach (DataRow taskRow in tasks.Rows)
                {
                    task = new Util.ImportExportMPP.TaskList();
                    //Task task = project.AddTask();
                    string title = UGITUtility.StripHTML(Convert.ToString(taskRow[DatabaseObjects.Columns.Title])); // UGITUtility.StripHTML(Convert.ToString(taskRow[DatabaseObjects.Columns.Title]));
                    task.Name = title;
                    task.Start = UGITUtility.StringToDateTime((taskRow[DatabaseObjects.Columns.StartDate]));
                    task.ActualStart = UGITUtility.StringToDateTime((taskRow[DatabaseObjects.Columns.StartDate]));
                    double duration = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(taskRow[DatabaseObjects.Columns.UGITDuration])))
                        duration = UGITUtility.StringToDouble((taskRow[DatabaseObjects.Columns.UGITDuration]));

                    // task.Duration = Duration.getInstance(work, TimeUnit.DAYS);
                    task.Duration = duration;
                    double work = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(taskRow[DatabaseObjects.Columns.TaskEstimatedHours])))
                        work = UGITUtility.StringToDouble((taskRow[DatabaseObjects.Columns.TaskEstimatedHours]));
                    if (work > 0)
                        task.Work = work;

                    double actualWork = UGITUtility.StringToDouble(taskRow[DatabaseObjects.Columns.TaskActualHours]);
                    task.ActualWork = actualWork; //Duration.getInstance(actualWork, TimeUnit.HOURS);
                    double remainingWork = UGITUtility.StringToDouble(taskRow[DatabaseObjects.Columns.EstimatedRemainingHours]);
                    task.RemainingWork = remainingWork; //Duration.getInstance(remainingWork, TimeUnit.HOURS);
                    task.Notes = Convert.ToString(taskRow[DatabaseObjects.Columns.Description]);
                    if (tasks.Columns.Contains(DatabaseObjects.Columns.PercentComplete))
                    {
                        task.PercentageComplete = (Convert.ToDouble(taskRow[DatabaseObjects.Columns.PercentComplete]));
                        task.PercentageWorkComplete = Convert.ToDouble(taskRow[DatabaseObjects.Columns.PercentComplete]);
                    }

                    if (tasks.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour) && Convert.ToString(taskRow[DatabaseObjects.Columns.TaskBehaviour]) == Constants.TaskType.Milestone)
                        task.Milestone = true;
                    else
                        task.Milestone = false;

                    task.OutlineLevel = (Convert.ToString(Convert.ToInt32(taskRow[DatabaseObjects.Columns.UGITLevel]) + 1));
                    task.Finish = UGITUtility.StringToDateTime((taskRow[DatabaseObjects.Columns.DueDate]));
                    //Util.ImportExportMPP.DataPropert dataPropert = new DataPropert();
                    //dataPropert.PMMID = Convert.ToInt32(taskRow[DatabaseObjects.Columns.Id]);
                    //dataPropert.PredecessorsID =Convert.ToString(UGITUtility.GetSPItemValue(taskRow, DatabaseObjects.Columns.Predecessors));
                    //dataPropert.TaskIndex = task.Id;
                    task.taskIds = UGITUtility.ConvertStringToList(Convert.ToString(UGITUtility.GetSPItemValue(taskRow, DatabaseObjects.Columns.Predecessors)), ",");
                    task.Id = Convert.ToInt32(taskRow[DatabaseObjects.Columns.Id]);
                   // task.ParentTask = Convert.ToString(taskRow[DatabaseObjects.Columns.ParentTaskID]);
                    //project.datapropert.Add(dataPropert);

                    //new line for code for Export/Import task.
                    if (UGITUtility.IsSPItemExist(taskRow, DatabaseObjects.Columns.UGITAssignToPct))
                    {
                        List<UGITAssignTo> listAssignTo = TaskManager.GetUGITAssignPctExport(Convert.ToString(taskRow[DatabaseObjects.Columns.UGITAssignToPct]));
                        string strAssignToPct = string.Empty;
                        task.listAssignTo = listAssignTo;
                        foreach (var item in listAssignTo)
                        {
                            if (!string.IsNullOrEmpty(item.LoginName))
                            {
                                UserProfile user = UserManager.GetUserByUserName(item.LoginName); //.GetUserByName(item.LoginName, SPPrincipalType.User);
                                if (user != null)
                                {
                                    if (!string.IsNullOrEmpty(strAssignToPct))
                                        strAssignToPct += Constants.UserInfoSeparator;
                                    strAssignToPct += user.UserName + "[" + item.Percentage + "%]";
                                }
                            }
                        }
                        task.ResourceNames = strAssignToPct.Replace(";", ",");
                    }
                    project.taskList.Add(task);
                }
                ExportManagerMpp exportManagerMpp = new ExportManagerMpp(configSettingMpp, project, fileName, fullFileName);
                exportManagerMpp.ExportTask();
                if (System.IO.File.Exists(fullFileName))
                {
                    DownloadFile(fullFileName, fileName);
                    Response.End();
                }
            }

        }
        /// <summary>
        /// Functinality : This function creates a table that is required for storing the Project tasks
        /// </summary>
        /// <returns> DataTable </returns>
        private DataTable CreateTargetTable()
        {
            DataTable targetTable = new DataTable();
            DataColumn RowId = new DataColumn("RowId", typeof(int));
            targetTable.Columns.Add(RowId);
            DataColumn TicketId = new DataColumn(DatabaseObjects.Columns.TicketNPRIdLookup, typeof(string));
            targetTable.Columns.Add(TicketId);
            DataColumn TaskName = new DataColumn(DatabaseObjects.Columns.Title, typeof(string));
            targetTable.Columns.Add(TaskName);
            DataColumn StartDate = new DataColumn(DatabaseObjects.Columns.StartDate, typeof(DateTime));
            targetTable.Columns.Add(StartDate);
            DataColumn DueDate = new DataColumn(DatabaseObjects.Columns.DueDate, typeof(DateTime));
            targetTable.Columns.Add(DueDate);
            DataColumn AssignedTo = new DataColumn(DatabaseObjects.Columns.AssignedTo, typeof(string));
            targetTable.Columns.Add(AssignedTo);
            DataColumn PercentComplete = new DataColumn(DatabaseObjects.Columns.PercentComplete, typeof(double));
            targetTable.Columns.Add(PercentComplete);
            DataColumn ParentTask = new DataColumn(DatabaseObjects.Columns.ParentTask, typeof(string));
            targetTable.Columns.Add(ParentTask);
            DataColumn Description = new DataColumn(DatabaseObjects.Columns.Description, typeof(string));
            targetTable.Columns.Add(Description);
            DataColumn Predecessors = new DataColumn(DatabaseObjects.Columns.Predecessors, typeof(string));
            targetTable.Columns.Add(Predecessors);
            targetTable.Columns.Add("PredecessorExp", typeof(string));
            targetTable.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            targetTable.Columns.Add("DBID", typeof(int));

            //new line for Export/import task list.
            DataColumn AssignToPct = new DataColumn(DatabaseObjects.Columns.UGITAssignToPct, typeof(string));
            targetTable.Columns.Add(AssignToPct);

            DataColumn taskEstimatedHours = new DataColumn(DatabaseObjects.Columns.TaskEstimatedHours, typeof(double));
            targetTable.Columns.Add(taskEstimatedHours);
            DataColumn duration = new DataColumn(DatabaseObjects.Columns.UGITDuration, typeof(double));
            targetTable.Columns.Add(duration);
            DataColumn taskType = new DataColumn(DatabaseObjects.Columns.TaskBehaviour, typeof(string));
            taskType.DefaultValue = string.Empty;
            targetTable.Columns.Add(taskType);
            DataColumn comment = new DataColumn(DatabaseObjects.Columns.UGITComment, typeof(string));
            comment.DefaultValue = string.Empty;
            targetTable.Columns.Add(comment);

            DataColumn moduleName = new DataColumn(DatabaseObjects.Columns.ModuleName, typeof(string));
            moduleName.DefaultValue = string.Empty;
            targetTable.Columns.Add(moduleName);

            return targetTable;
        }

        private void DownloadFile(string downloadFilePath, string fileName)
        {
            try
            {
                // Create an instance of WebClient
                WebClient client = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.ContentType = "Application/msproject";
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                byte[] data = client.DownloadData(downloadFilePath);
                response.BinaryWrite(data);
                // response.End();

            }
            catch (System.Exception ex)
            {
                //string errorVal = "Unable to download the file" + ex.Message;
                //Response.Write(errorVal);

                Util.Log.ULog.WriteException(ex);
            }
        }

        //private double DateDiff(System.DateTime startDate, System.DateTime endDate)
        //{
        //    double diff = 0;
        //    System.TimeSpan TS = new System.TimeSpan(startDate.Ticks - endDate.Ticks);
        //    diff = Convert.ToDouble(TS.TotalDays);
        //    return diff;
        //}

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