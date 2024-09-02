using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class UGITModuleConstraint
    {
        public bool Changes { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
        public string ConstraintType { get; set; }
        public string DocumentPath { get; set; }
        public int DocumentID { get; set; }
        public string TicketPublicID { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public double PercentageComplete { get; set; }
        public string AssignedTo { get; set; }
        public List<UserLookupValue> CreatedByUser { get; set; }
        public List<UserLookupValue> ModifiedByUser { get; set; }
        public string LatestComment { get; set; }
        public string Priority { get; set; }
        public double Duration { get; set; }
        public double ActualHours { get; set; }
        public double EstimatedHours { get; set; }
        public List<HistoryEntry> CommentHistory { get; set; }
        public string ModuleName { get; set; }
        public DateTime? ProposedDate { get; set; }
        public UGITTaskProposalStatus ProposedDateStatus { get; set; }
        public bool AutoApprove { get; set; }
        public string FormulaValue { get; set; }
        public string ModuleStage { get; set; }
        public string ModuleStep { get; set; }
        public DateTime CompletionDate { get; set; }
        public int itemOrder { get; set; }
        public int ItemOrder { get; set; }
        public string CompletedBy { get; set; }
        public string UserRoleType { get; set; }
        public string ProposedStatus { get; set; }
        public string DateExpresion { get; set; }
        public static DataRow ModuleStageConstraints { get; private set; }


        public UGITModuleConstraint()
        {
            ModuleName = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            Status = Constants.NotStarted;
            LatestComment = string.Empty;
            ProposedDateStatus = UGITTaskProposalStatus.Not_Proposed;
            ModuleStep = string.Empty;
        }

        /// <summary>
        /// this will delete the Constraint
        /// </summary>
        /// <param name="sWeb"></param>
        public void DeleteConstraint()
        {
            //SPList splst = SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleStageConstraints);
            //SPListItem taskItem = null;
            //taskItem = splst.GetItemById(Convert.ToInt16(this.ID));
            //if (taskItem != null)
            //    taskItem.Delete();
        }


        /// <summary>
        /// This will load the task Object.
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="rItem"></param>
        /// <returns></returns>
        public static ModuleStageConstraints LoadItem(ModuleStageConstraintTemplates rItem)
        {
            ModuleStageConstraints task = new ModuleStageConstraints();
            task.ID = rItem.ID;
            task.Title = rItem.Title;
            if (rItem.Body != string.Empty)
                task.Body = UGITUtility.StripHTML(rItem.Body);
            task.StartDate = null;
            if (Convert.ToString(rItem.StartDate) != string.Empty)
                task.StartDate = Convert.ToDateTime(rItem.StartDate);
            task.TaskDueDate = null;
            if (Convert.ToString(rItem.TaskDueDate) != string.Empty)
                task.TaskDueDate = Convert.ToDateTime(rItem.TaskDueDate);
            task.ProposedDate = null;
            if (Convert.ToString(rItem.ProposedDate) != string.Empty)
                task.ProposedDate = Convert.ToDateTime(rItem.ProposedDate);

            if (Convert.ToString(rItem.ProposedStatus) != string.Empty)
                task.ProposedStatus = rItem.ProposedStatus;

            if (Convert.ToString(rItem.DateExpression) != string.Empty)
                task.DateExpression = Convert.ToString(rItem.DateExpression);

            task.TicketId = rItem.TicketId;
            task.ModuleNameLookup = rItem.ModuleNameLookup;
            task.TaskStatus = rItem.TaskStatus;
            task.ModuleStep = rItem.ModuleStep;
            task.PercentComplete = rItem.PercentComplete;
            task.Comment = rItem.Comment;
            double estimatedHours = 0;
            double.TryParse(Convert.ToString(rItem.EstimatedHours), out estimatedHours);
            task.TaskEstimatedHours = estimatedHours;
            double actualHours = 0;
            double.TryParse(Convert.ToString(rItem.TaskActualHours), out actualHours);
            task.TaskActualHours = actualHours;
            task.AssignedTo = rItem.AssignedTo;
            return task;
        }

        public static UGITModuleConstraint LoadItem(ApplicationContext context, List<ModuleStageConstraints> rItem)
        {

            UGITModuleConstraint task = new UGITModuleConstraint();
            foreach (var item in rItem)
            {
                task.ID = Convert.ToInt32(item.ID);
                task.Title = Convert.ToString(item.Title);

                //task.ItemOrder = Convert.ToString(item.ItemOrder);

                if(!string.IsNullOrEmpty(item.Body))
                task.Description = UGITUtility.StripHTML(Convert.ToString(item.Body));

                task.StartDate = DateTime.MinValue;
                task.StartDate = Convert.ToDateTime(item.StartDate);

                task.DueDate = DateTime.MinValue;
                task.DueDate = Convert.ToDateTime(item.TaskDueDate);

                task.ProposedDate = DateTime.MinValue;
                task.ProposedDate = Convert.ToDateTime(item.ProposedDate);

                if(!string.IsNullOrEmpty(Convert.ToString(item.ProposedStatus)))
                task.ProposedDateStatus = (UGITTaskProposalStatus)Enum.Parse(typeof(UGITTaskProposalStatus), Convert.ToString(item.ProposedStatus));


                task.TicketPublicID = Convert.ToString(item.TicketId);
                if (UGITUtility.IsSPItemExist(item.ModuleNameLookup))
                {
                    string[] delim = { Constants.Separator };
                    task.ModuleName = Convert.ToString(item.ModuleNameLookup).Split(delim, StringSplitOptions.None)[0];
                }

                if (UGITUtility.IsSPItemExist(item.TaskStatus))
                    task.Status = Convert.ToString(item.TaskStatus);

                task.ModuleStep = Convert.ToString(item.ModuleStep);
                task.PercentageComplete = Convert.ToDouble(item.PercentComplete); //Convert.ToDouble(item.PercentComplete) * 100; // modified for Lead mgmt.Tasks tab, Edit popup

                if (item != null && !string.IsNullOrEmpty(item.Comment))
                task.CommentHistory = uHelper.GetHistory(Convert.ToString(item.Comment), false);

                double estimatedHours = 0;
                //double.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(Convert.ToString(item.TaskEstimatedHours))), out estimatedHours);//rItem, DatabaseObjects.Columns.TaskEstimatedHours
                double.TryParse(Convert.ToString(item.TaskEstimatedHours), out estimatedHours);
                task.EstimatedHours = estimatedHours;


                double actualHours = 0;
                //double.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(Convert.ToString(item.TaskActualHours))), out actualHours);
                double.TryParse(Convert.ToString(item.TaskActualHours), out actualHours);
                task.ActualHours = actualHours;

                var userLookups = UGITUtility.ConvertStringToList(Convert.ToString(item.AssignedTo), ",");

                //SPFieldUserValueCollection userLookups = new SPFieldUserValueCollection(spWeb, Convert.ToString(rItem[DatabaseObjects.Columns.AssignedTo]));
                if (userLookups != null)
                    task.AssignedTo = userLookups.ToString();

                task.AssignedTo = item.AssignedTo;
                DateTime completionDate = UGITUtility.StringToDateTime(item.CompletionDate);
                if (completionDate != DateTime.MinValue)
                    task.CompletionDate = completionDate;

                task.ItemOrder = item.ItemOrder;

                //task.DateExpresion = Convert.ToString(rItem[DatabaseObjects.Columns.DateExpression]);//Missing column from ModuleStageConstraints


                //task.UserRoleType = Convert.ToString(rItem[DatabaseObjects.Columns.UserRoleType]);

                //task.CompletedBy = new SPFieldUserValue(spWeb, Convert.ToString(rItem[DatabaseObjects.Columns.CompletedBy]));
            }
            return task;

        }

        /// <summary>
        /// this will fetch the step id based on modulestage id
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns></returns>
        public static string GetModuleStepIdFromStage(ApplicationContext context, string stageId, string modulename)
        {
            if (string.IsNullOrEmpty(stageId) || stageId == "0")
                return stageId;
            string moduleStep = string.Empty;
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            UGITModule uGITModule = moduleViewManager.LoadByName(modulename);
            LifeCycle lifeCycle = uGITModule.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
            if (lifeCycle != null && lifeCycle.Stages.Count() > 0)
                moduleStep = Convert.ToString(lifeCycle.Stages.Where(x => x.StageStep == Convert.ToInt32(stageId)).FirstOrDefault().StageStep);


            //DataTable moduleStages = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleStep} = '{stageId}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            //DataRow moduleStageItem = null;

                //if (moduleStages != null && moduleStages.Rows.Count > 0)
                //{
                //    //moduleStageItem = moduleStages.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleStep, stageId))[0]; //SPListHelper.GetItemByID(moduleStages.Items, Convert.ToInt32(stageId));
                //    moduleStageItem = moduleStages.Select()[0];
                //    if (moduleStageItem != null && UGITUtility.IsSPItemExist(moduleStageItem, DatabaseObjects.Columns.ModuleStep))
                //    {
                //        moduleStep = Convert.ToString(moduleStageItem[DatabaseObjects.Columns.ModuleStep]);
                //    }
                //}
            return moduleStep;
        }

        /// <summary>
        /// this will load the task object by ticket and task id
        /// </summary>
        /// <param name="web"></param>
        /// <param name="ticketId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>

        public static UGITModuleConstraint LoadTask(string ticketId, int taskId)
        {
            //SPListItem item = null;
            //item = UGITModuleConstraint.GetTaskItem(ticketId, taskId);
            //UGITModuleConstraint mtask = null;
            ////if (item != null)
            ////{
            ////    mtask = UGITModuleConstraint.LoadItem(web, item);
            ////}
            //return mtask;
            throw new Exception();
        }

        /// <summary>
        /// This will mark the tasks complete which are associatedwith this ticket.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="moduleStep"></param>

        public static void MarkAllTaskComplete(string ticketId, int moduleStep, ApplicationContext context)
        {
            ModuleStageConstraintsManager constraintsManager = new ModuleStageConstraintsManager(context);
            List<ModuleStageConstraints> stageConstraints = constraintsManager.Load(x => x.TicketId==ticketId && x.ModuleStep==moduleStep);
            foreach (ModuleStageConstraints item in stageConstraints)
            {
                item.TaskStatus = Constants.Completed;
                item.PercentComplete = 1;
                item.CompletionDate = DateTime.Now;
                item.CompletedBy = context.CurrentUser.Id;
            }
            constraintsManager.UpdateItems(stageConstraints);
        }

        public static UGITModuleConstraint MarkStageTaskAsComplete(long taskID, ApplicationContext context)
        {
            UGITModuleConstraint constraint = null;
            UGITModuleConstraint moduleTask = new UGITModuleConstraint();
            ModuleStageConstraintsManager objModuleStageConstraintsManager = new ModuleStageConstraintsManager(context);
            ModuleStageConstraints taskItem = null;
            List<ModuleStageConstraints> moduleTaskItem = null;
            if (taskID > 0)
            {
                taskItem = objModuleStageConstraintsManager.LoadByID(taskID);
                moduleTaskItem = new List<ModuleStageConstraints>();
                moduleTaskItem.Add(taskItem);
                moduleTask = LoadItem(context, moduleTaskItem);
                constraint = MarkStageTaskAsComplete(moduleTask, context);
                SaveTask(context, ref constraint, DatabaseObjects.Tables.ModuleStageConstraints);
            }
            return constraint;
        }
        public static UGITModuleConstraint MarkStageTaskAsInProgress(long taskID, ApplicationContext context)
        {
            UGITModuleConstraint constraint = null;
            UGITModuleConstraint moduleTask = new UGITModuleConstraint();
            ModuleStageConstraintsManager objModuleStageConstraintsManager = new ModuleStageConstraintsManager(context);
            ModuleStageConstraints taskItem = null;
            List<ModuleStageConstraints> moduleTaskItem = null;
            if (taskID > 0)
            {
                taskItem = objModuleStageConstraintsManager.LoadByID(taskID);
                moduleTaskItem = new List<ModuleStageConstraints>();
                moduleTaskItem.Add(taskItem);
                moduleTask = LoadItem(context, moduleTaskItem);
                constraint = MarkStageTaskAsInProgress(moduleTask, context);
                SaveTask(context, ref constraint, DatabaseObjects.Tables.ModuleStageConstraints);
            }
            return constraint;
        }

        public static UGITModuleConstraint MarkStageTaskAsComplete(UGITModuleConstraint constraint, ApplicationContext context)
        {
            if (constraint != null)
            {
                constraint.Status = Constants.Completed;
                constraint.PercentageComplete = 100;
                //completed on..
                constraint.CompletionDate = DateTime.Now;
                constraint.Changes = true;
                constraint.CompletedBy = context.CurrentUser.Id;
                // SPFieldUserValue completedBy = new SPFieldUserValue();
                //completedBy.LookupId = context.CurrentUser.Id;
                //constraint.CompletedBy = completedBy;
            }
            return constraint;
        }

        public static UGITModuleConstraint MarkStageTaskAsInProgress(UGITModuleConstraint constraint, ApplicationContext context)
        {
            if (constraint != null)
            {
                constraint.Status = Constants.InProgress;
                constraint.PercentageComplete = 0;
            }
            return constraint;
        }

        /// <summary>
        /// This will get status of task, if they are pending and generates a message for it.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="moduleStep"></param>
        /// <param name="message"></param>
        /// <returns></returns>

        public static bool GetPendingConstraintsStatus(string ticketId, int moduleStep, ref string message, ApplicationContext context)
        {
            StringBuilder tasks = new StringBuilder();
            bool isTicketReady = false;   // temp MK need to change true to false
            int counter = 0;
            //SPQuery query = new SPQuery();
            /*
            DataTable splstConditions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStageConstraints, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            //query.Query = String.Format("<Where><And><Eq><FieldRef Name='{1}'/><Value Type='Text'>{0}</Value></Eq> <Eq><FieldRef Name='{2}' /><Value Type='Number'>{3}</Value></Eq></And></Where>", ticketId, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ModuleStep, moduleStep);
            string query = string.Format("{1}='{0}' and {2}={3}", ticketId, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Module_Step, moduleStep);
            DataRow[] constraintItems = splstConditions.Select(query);
            */

            string query = string.Format("{1}='{0}' and {2}={3} and {4} = '{5}' and {6} = 0", ticketId, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Module_Step, moduleStep, DatabaseObjects.Columns.TenantID, context.TenantID, DatabaseObjects.Columns.Deleted);
            DataTable splstConditions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStageConstraints, query);
            DataRow[] constraintItems = splstConditions.Select();

            if (constraintItems != null && constraintItems.Count() > 0)
            {
                foreach (DataRow item in constraintItems)
                {
                    if (!item[DatabaseObjects.Columns.UGITTaskStatus].Equals(Constants.Completed))
                    {
                        tasks.Append(item[DatabaseObjects.Columns.Title]);
                        tasks.Append(" ,: ");
                        counter++;
                    }
                }
            }
            if (counter > 0)
            {
                string finalMessage = Convert.ToString(tasks).Substring(0, Convert.ToString(tasks).LastIndexOf(" ,:"));
                isTicketReady = false;
                message = finalMessage;
            }
            else
            {
                isTicketReady = true;
                message = string.Empty;
            }
            return isTicketReady;
        }

        /// <summary>
        /// This will get the task as splistitem
        /// </summary>
        /// <param name="ticketID"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public static void GetTaskItem(string ticketID, int taskId)
        {
            //SPListItem item = null;
            //SPQuery query = new SPQuery();
            //SPList splstConditions = SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleStageConstraints);
            //query.Query = String.Format("<Where><Eq><FieldRef Name='{1}'/><Value Type='Text'>{0}</Value></Eq></Where>", ticketID, DatabaseObjects.Columns.TicketId);

            //if (splstConditions != null)
            //{
            //    item = splstConditions.GetItemById(taskId);
            //}
            //return item;
            throw new Exception();
        }

        /// <summary>
        /// This will save the task
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="task"></param>
        /// <param name="isNewTask"></param>

        /// <summary>
        /// Save Task, edit task based on taskID
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="task"></param>
        /// <param name="listName"></param>
        public static void SaveTask(ApplicationContext context, ref UGITModuleConstraint task, string listName, bool IsFromTemplate = false)
        {
            ModuleStageConstraints taskItem = null;
            if (task == null)
            {
                // if (task == null)
                // {
                //    // Log.WriteLog("SaveTask: No task found");
                return;
                // }
            }
            ModuleViewManager ModuleManager = new ModuleViewManager(context);
            ModuleStageConstraintsManager moduleStageConstraintsManager = new ModuleStageConstraintsManager(context);
            TicketManager TicketManager = new TicketManager(context);

            var moduleStageConstraintsList = moduleStageConstraintsManager.Load($"{DatabaseObjects.Columns.ModuleNameLookup}='{task.ModuleName}'");
            var taskID = task.ID;
            if (taskID > 0)

                taskItem = moduleStageConstraintsList.FirstOrDefault(x => x.ID == taskID);
            else
                taskItem = new ModuleStageConstraints();


            taskItem.Title = task.Title.Trim();
            taskItem.Body = task.Description;
            taskItem.StartDate = task.StartDate;
            taskItem.TaskDueDate = task.DueDate;
            taskItem.PercentComplete = Convert.ToInt32(task.PercentageComplete); //Convert.ToInt32(task.PercentageComplete / 100); // modified for LEM -> Tasks tab
            if (taskItem.PercentComplete == 100) 
            {
                taskItem.CompletionDate = task.CompletionDate;
                taskItem.CompletedBy = task.CompletedBy;
            }

            taskItem.TaskStatus = task.Status;
            taskItem.ItemOrder = task.ItemOrder;
            taskItem.ModuleStep = Convert.ToInt32(!string.IsNullOrEmpty(task.ModuleStep) ? task.ModuleStep : "1");

            var moduleDetails = ModuleManager.GetByName(task.ModuleName);
            if (moduleDetails != null)
                taskItem.ModuleNameLookup = Convert.ToString(moduleDetails.ModuleName);

            taskItem.TicketId = task.TicketPublicID;
           // var moduleStageContentType = taskItem.ContentType[DatabaseObjects.ContentType.ModuleTaskCT];
            //taskItem.ContentType = moduleStageContentType.Name;
            //taskItem.ContentTypeId = moduleStageContentType.Id;
            taskItem.AssignedTo = task.AssignedTo;

            if (task.LatestComment != null && task.LatestComment.Trim() != string.Empty)
            {
                string comment = uHelper.GetVersionString(context.CurrentUser.Id, task.LatestComment, UGITUtility.ObjectToData(taskItem).Select()[0], DatabaseObjects.Columns.TicketComment);
                taskItem.Comment = comment;
                task.CommentHistory = uHelper.GetHistory(comment, false);
            }

            taskItem.TaskEstimatedHours = task.EstimatedHours;
            taskItem.TaskActualHours = task.ActualHours;
            taskItem.ProposedDate = null;
            if (task.ProposedDate != DateTime.MinValue)
                taskItem.ProposedDate = task.ProposedDate;
            taskItem.ProposedStatus = task.ProposedDateStatus.ToString();

            //taskItem[DatabaseObjects.Columns.DateExpression] = task.DateExpresion; Add later
            // taskItem[DatabaseObjects.Columns.UserRoleType] = task.UserRoleType;

            if (listName == DatabaseObjects.Tables.ModuleStageConstraints && IsFromTemplate)
            {
                var taskPublicID = task.TicketPublicID;
                ModuleStageConstraints plstItem = moduleStageConstraintsManager.Load($"{DatabaseObjects.Columns.ID}={taskPublicID}")[0];
                string[] dateExp = !string.IsNullOrEmpty(task.DateExpresion) ? task.DateExpresion.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries) : null;
                if (dateExp != null)
                {
                    string[] dateDaysExp = dateExp == null || dateExp.Length == 0 ? null : dateExp[1].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);

                    if (UGITUtility.IsSPItemExist(UGITUtility.ObjectToData(plstItem).Select()[0], dateExp[0]))
                    {
                        var test = UGITUtility.ObjectToData(plstItem).Select()[0];
                        if (test[dateExp[0]] != null)
                        {
                            if (dateDaysExp[0] == "+")
                            {
                                DateTime dueDate = Convert.ToDateTime(test[dateExp[0]]).AddDays(UGITUtility.StringToInt(dateDaysExp[1]));
                                taskItem.StartDate = dueDate > DateTime.Now ? DateTime.Now : dueDate.Date;
                                taskItem.TaskDueDate = Convert.ToDateTime(test[dateExp[0]]).AddDays(UGITUtility.StringToInt(dateDaysExp[1]));
                            }
                            else
                                taskItem.TaskDueDate = Convert.ToDateTime(test[dateExp[0]]).AddDays(-UGITUtility.StringToInt(dateDaysExp[1]));
                        }
                    }
                }
                //    if (UGITUtility.IsSPItemExist(plstItem, task.UserRoleType))

                //    {
                //    taskItem.AssignedTo = plstItem[task.UserRoleType];

                //    SPList lstCRMProjectAllocation = SPListHelper.GetSPList(DatabaseObjects.Lists.CRMProjectAllocation, spWeb);

                //    if (lstCRMProjectAllocation != null)
                //    {
                //        SPQuery query = new SPQuery();
                //        query.Query = String.Format("<Where><And><Eq><FieldRef Name='{1}'/><Value Type='Text'>{0}</Value></Eq> <Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></And></Where>", task.TicketPublicID, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Type, task.UserRoleType);

                //        SPListItemCollection spColCRMProjectAllocation = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.CRMProjectAllocation, query, spWeb);
                //        try
                //        {
                //            if (spColCRMProjectAllocation != null && spColCRMProjectAllocation.Count == 1)
                //            {
                //                taskItem[DatabaseObjects.Columns.AssignedTo] = spColCRMProjectAllocation[0][DatabaseObjects.Columns.UGITAssignedTo];
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            Log.WriteLog(string.Format("CRMProjectAllocation Exception: {0}", ex.Message));
                //        }
                //    }
                //}
                // }
            }
            // Update completed on & by
            //if (uHelper.IfColumnExists(DatabaseObjects.Columns.CompletionDate, taskItem, true)) // Column doesn't exist for template tasks
            //{
            //    if (task.Status == "Completed")
            //    {
            //        if (taskItem.CompletionDate == null)
            //            taskItem.CompletionDate = DateTime.Now;
            //    }
            //    else
            //        taskItem.CompletionDate = null;
            //}

            //if (uHelper.IfColumnExists(DatabaseObjects.Columns.CompletedBy, taskItem, true)) // Column doesn't exist for template tasks taskItem.ParentList
            //{
            //    if (task.Status == "Completed" && task.CompletedBy != null)
            //        taskItem.CompletedBy = task.CompletedBy;
            //    else
            //        taskItem.CompletedBy = null;
            //}

            if (task.ID > 0)
                moduleStageConstraintsManager.Update(taskItem);
            else
                moduleStageConstraintsManager.Insert(taskItem);
            task.ID = Convert.ToInt32(taskItem.ID);                                                  
            task.Changes = false;
        }


        /// <summary>
        /// this helps in moving the ticket to next stage after ticket is marked complete.
        /// </summary>
        public string AutoApproveTicket(UGITModuleConstraint task, ApplicationContext context)
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            string errorMessage = string.Empty;
            DataRow[] moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup}='{task.ModuleName}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();

            DataRow currentStageRow = null;
            foreach (DataRow row in moduleStagesRow)
            {
                if (Convert.ToString(row[DatabaseObjects.Columns.ModuleStep]).Equals(task.ModuleStep))
                {
                    currentStageRow = row;
                    break;
                }
            }

            string ticketList = null;
            ticketList = GetRelatedTicketListFromModule(task.ModuleName);
            if (ticketList != null)
            {
                Ticket ticketObj = new Ticket(context, task.ModuleName);
                DataRow ticketItem = Ticket.GetCurrentTicket(context, task.ModuleName, task.TicketPublicID);
                {
                    UGITModule module = moduleViewManager.GetByName(task.ModuleName);
                    if (module != null)
                    {
                        int moduleId = Convert.ToInt32(module.ModuleId);
                        if (moduleId > 0 && ticketItem != null)
                        {
                            LifeCycleStage currentStage = ticketObj.GetTicketCurrentStage(ticketItem);
                            if (currentStage == null)
                                return string.Empty;

                            bool stageHasMandatoryFields = ticketObj.CheckMandatoryFieldForStage(currentStage, ticketItem);

                            if (!stageHasMandatoryFields)
                                errorMessage = ticketObj.Approve(context.CurrentUser, ticketItem);
                            else
                                errorMessage = "Please fill mandatory fields to approve the stage";

                            if (errorMessage.Equals(string.Empty))
                            {
                                ticketObj.AssignModuleSpecificDefaults(ticketItem);
                                ticketObj.SetTicketPriority(ticketItem, task.ModuleName);
                                // Update Ticket.

                                errorMessage = ticketObj.CommitChanges(ticketItem);

                                if (string.IsNullOrEmpty(errorMessage))
                                {
                                    //Send Email notification.
                                    ticketObj.SendEmailToActionUsers(Convert.ToString(ticketItem[DatabaseObjects.Columns.ModuleStepLookup]), ticketItem, module.ModuleName);
                                }
                            }                            
                        }
                    }
                }
            }

            return errorMessage;
        }

        /// <summary>
        /// This will fetch the modulestepid from moduleStage.
        /// </summary>
        /// <param name="moduleStageId"></param>
        /// <returns></returns>
        public string GetModuleStepFromModuleStageId(string moduleStageId)
        {
            string moduleStep = string.Empty;
            //SPList moduleStages = SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleStages);
            //SPListItem moduleStageItem = null;
            //if (moduleStages != null)
            //{
            //    moduleStageItem = moduleStages.GetItemById(Convert.ToInt32(moduleStageId));
            //    if (moduleStageItem != null && uHelper.IsSPItemExist(moduleStageItem, DatabaseObjects.Columns.ModuleStep))
            //    {
            //        moduleStep = Convert.ToString(moduleStageItem[DatabaseObjects.Columns.ModuleStep]);

            //    }
            //}
            return moduleStep;
        }

        public static string GetRelatedTicketListFromModule(string module)
        {

            string list = string.Empty;
            if (module.Equals("NPR"))
            {
                list = DatabaseObjects.Tables.NPRTicket;
            }
            else if (module.Equals("PRS"))
            {
                list = DatabaseObjects.Tables.PRSTicket;
            }
            else if (module.Equals("TSR"))
            {
                list = DatabaseObjects.Tables.TSRTicket;
            }
            else if (module.Equals("ACR"))
            {
                list = DatabaseObjects.Tables.ACRTicket;
            }
            else if (module.Equals("DRQ"))
            {
                list = DatabaseObjects.Tables.DRQTicket;
            }
            else if (module.Equals("BTS"))
            {
                list = DatabaseObjects.Tables.BTSTicket;
            }
            else if (module.Equals("INC"))
            {
                list = DatabaseObjects.Tables.INCTicket;
            }
            else if (module.Equals("SVC"))
            {
                list = DatabaseObjects.Tables.SVCRequests;
            }
            return list;
        }

        public static List<uGovernIT.FactTableField> GetColumnNamesWithDataType(ApplicationContext context, string listName)
        {
            List<uGovernIT.FactTableField> queryTableFields = new List<uGovernIT.FactTableField>();
            DataTable inputList = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            if (inputList != null)
            {
                FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
                foreach (DataColumn field in inputList.Columns)
                {
                    if (field != null)
                    {
                        FieldConfiguration fieldInfo = fieldConfigurationManager.GetFieldByFieldName(UGITUtility.ObjectToString(field.ColumnName));
                        if (fieldInfo != null && fieldInfo.Datatype.Equals("UserField"))
                        {
                            queryTableFields.Add(new uGovernIT.FactTableField(listName, field.ColumnName, "user", field.ColumnName));
                        }
                        if (fieldInfo != null && fieldInfo.Datatype.Equals("Lookup"))
                        {
                            queryTableFields.Add(new uGovernIT.FactTableField(listName, field.ColumnName, "string", field.ColumnName));
                        }
                        else
                        {
                            if (Convert.ToString(field.DataType).ToLower().Contains("bit"))
                            {
                                queryTableFields.Add(new uGovernIT.FactTableField(listName, field.ColumnName, "boolean", field.ColumnName));
                            }
                            else
                            {
                                queryTableFields.Add(new uGovernIT.FactTableField(listName, field.ColumnName, GetStandardDataType(Convert.ToString(field.DataType)).ToLower(), field.ColumnName));
                            }
                        }
                    }
                }

                queryTableFields.Sort((x, y) => x.FieldDisplayName.CompareTo(y.FieldDisplayName));
            }
            return queryTableFields;
        }

        public static string GetStandardDataType(string spDataType)
        {
            string standardType = "";
            spDataType = spDataType.Replace("System.", "");
            switch (spDataType)
            {
                case "DateTime":
                    standardType = "DateTime";
                    break;
                case "Lookup":
                    standardType = "String";
                    break;
                case "Currency":
                    standardType = "Currency";
                    break;
                case "Text":
                    standardType = "String";
                    break;
                case "Note":
                    standardType = "String";
                    break;
                case "Boolean":
                    standardType = "Boolean";
                    break;
                case "Choice":
                    standardType = "String";
                    break;
                case "Counter":
                    standardType = "Integer";
                    break;
                case "User":
                    standardType = "User";
                    break;
                case "Number":
                    standardType = "Double";
                    break;
                case "Int64":
                case "Int32":
                    standardType = "Int";
                    break;
                default:
                    standardType = "String";
                    break;
            }
            return standardType;
        }

        #region dynamic task creation and config

        /// <summary>
        /// This will add default task
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="task"></param>
        /// <param name="listName"></param>
        public static void AddNewModuleStageTaskToTicket(ref UGITModuleConstraint task, string listName)
        {
            //task.ID = 0; // ID changed to add new task in the constraintlist and copy from templatelist.
            //SaveTask(spWeb, ref  task, listName);
            //TaskCache.ReloadProjectTasks(Constants.ExitCriteria, task.TicketPublicID);
        }

        /// <summary>
        /// This will set start date, due date and Status of active task.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="spWeb"></param>
        public static void ConfigureCurrentModuleStageTask(ApplicationContext Context, DataRow ticket)  //SPListItem ticket, SPWeb spWeb
        {
            string moduleStageId = string.Empty;
            string ticketPublicId = string.Empty;
            string moduleStep = string.Empty;
            if (UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.TicketId))
            {
                ticketPublicId = Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]);
            }
            if (UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.StageStep))
            {
                moduleStageId = Convert.ToString(ticket[DatabaseObjects.Columns.StageStep]);
                //moduleStep = GetModuleStepIdFromStage(Convert.ToString(UGITUtility.SplitString(Convert.ToString(moduleStageId), Constants.Separator, 0)));
            }

            if (!moduleStageId.Equals(string.Empty) && !ticketPublicId.Equals(string.Empty))
            {
                ConfigureCurrentModuleStageTask(Context, Convert.ToInt32(moduleStageId), ticketPublicId);
            }
        }


        /// <summary>
        /// This will set start date, due date and Status of active task.
        /// </summary>
        /// <param name="currentModuleStep"></param>
        /// <param name="ticketPublicId"></param>
        /// <param name="spWeb"></param>
        public static void ConfigureCurrentModuleStageTask(ApplicationContext context, int currentModuleStep, string ticketPublicId)
        {
            ModuleStageConstraintsManager objModuleStageConstraintsManager = new ModuleStageConstraintsManager(context);
            bool isDateChanged = false;
            bool isStatusChanged = false;
            //SPQuery query = new SPQuery();
            List<ModuleStageConstraints> constraintItems = objModuleStageConstraintsManager.Load(x => x.TicketId.Equals(ticketPublicId) && x.ModuleStep.Equals(currentModuleStep));
            // DataTable splstConditions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStageConstraints);
            //query.Query = String.Format("<Where><And><Eq><FieldRef Name='{1}'/><Value Type='Text'>{0}</Value></Eq> <Eq><FieldRef Name='{2}' /><Value Type='Number'>{3}</Value></Eq></And></Where>", ticketPublicId, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ModuleStep, currentModuleStep);
            // string query = string.Format("{1}='{0}' and {2}={3}", ticketPublicId, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Module_Step, currentModuleStep);
            // DataRow[] constraintItems = splstConditions.Select(query);
            if (constraintItems != null && constraintItems.Count() > 0)
            {
                foreach (ModuleStageConstraints item in constraintItems)
                {
                    isDateChanged = false;
                    isStatusChanged = false;

                    //if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.StartDate) && UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TaskDueDate))
                    //{
                    item.StartDate = DateTime.Now;
                    DateTime dueDate = new DateTime();
                    dueDate = DateTime.Now;
                    //if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.TaskEstimatedHours))
                    //{
                    int hours = Convert.ToInt32(item.TaskEstimatedHours);
                    DateTime[] dates = uHelper.GetEndDateByHours(context, hours, DateTime.Now);
                    if (dates != null)
                        dueDate = dates[1];
                    item.TaskDueDate = dueDate;
                    //}
                    isDateChanged = true;

                    //if (UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.UGITTaskStatus))
                    //{
                    if (Convert.ToString(item.TaskStatus) == Constants.NotStarted)
                    {
                        item.TaskStatus = Constants.InProgress;
                        isStatusChanged = true;
                    }
                    // }
                    if (isDateChanged || isStatusChanged)
                    {
                        objModuleStageConstraintsManager.Update(item);
                        // item.UpdateOverwriteVersion();
                    }
                    // }
                }
                //TaskCache.ReloadProjectTasks(Constants.ExitCriteria, ticketPublicId);
            }
        }

        /// <summary>
        /// This will copy the template task in ticket module stage tasks for respective stages.
        /// </summary>
        /// <param name="ticketPublicId"></param>
        /// <param name="moduleName"></param>
        /// <param name="spWeb"></param>
        public static void CreateModuleStageTasksInTicket(ApplicationContext context, string ticketPublicId, string moduleName, string workFlowSkipStages = "")
        {
            try
            {
                ModuleStageConstraintTemplatesManager objModuleStageConstraintTemplatesManager = new ModuleStageConstraintTemplatesManager(context);
                ModuleStageConstraintsManager objModuleStageConstraintsManager = new ModuleStageConstraintsManager(context);
                List<ModuleStageConstraintTemplates> lstStageConstraint = objModuleStageConstraintTemplatesManager.Load(x => x.TenantID == context.TenantID && x.ModuleNameLookup.Equals(moduleName) && x.Deleted == false);
                if (lstStageConstraint != null && lstStageConstraint.Count() > 0)
                {
                    List<string> lstOfSkipStages = null;
                    if (!string.IsNullOrEmpty(workFlowSkipStages))
                        lstOfSkipStages = workFlowSkipStages.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    ModuleStageConstraints moduleTask = null;

                    DataRow plstItem = Ticket.GetCurrentTicket(context, moduleName, ticketPublicId);
                    foreach (ModuleStageConstraintTemplates item in lstStageConstraint)
                    {
                        moduleTask = LoadItem(item);
                        moduleTask.TicketId = ticketPublicId;
                        moduleTask.ID = 0;
                        if (lstOfSkipStages != null && lstOfSkipStages.Contains(UGITUtility.ObjectToString(moduleTask.ModuleStep)))
                            continue;
                        string[] dateExp = !string.IsNullOrEmpty(item.DateExpression) ? item.DateExpression.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries) : null;
                        if (dateExp != null)
                        {
                            string[] dateDaysExp = dateExp == null || dateExp.Length == 0 ? null : dateExp[1].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);

                            if (UGITUtility.IsSPItemExist(plstItem, dateExp[0]))
                            {
                                if (plstItem[dateExp[0]] != null)
                                {
                                    if (dateDaysExp[0] == "+")
                                    {
                                        DateTime dueDate = Convert.ToDateTime(plstItem[dateExp[0]]).AddDays(UGITUtility.StringToInt(dateDaysExp[1]));
                                        moduleTask.StartDate = dueDate > DateTime.Now ? DateTime.Now : dueDate.Date;
                                        moduleTask.TaskDueDate = Convert.ToDateTime(plstItem[dateExp[0]]).AddDays(UGITUtility.StringToInt(dateDaysExp[1]));
                                    }
                                    else
                                        moduleTask.TaskDueDate = Convert.ToDateTime(plstItem[dateExp[0]]).AddDays(-UGITUtility.StringToInt(dateDaysExp[1]));
                                }
                            }
                        }

                        objModuleStageConstraintsManager.Insert(moduleTask);
                    }
                }
            }catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
        }
        #endregion
    }
}
