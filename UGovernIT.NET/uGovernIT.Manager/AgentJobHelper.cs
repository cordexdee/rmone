
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using DevExpress.XtraReports.UI;
using DevExpress.Web;
using System.Text.RegularExpressions;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using System.Net.Http;
using uGovernIT.Helpers;
using uGovernIT.Web.Helpers;

namespace uGovernIT.Manager
{
    public class AgentJobHelper
    {
        ApplicationContext _context;
        UserProfile user;
        ModuleViewManager ObjModuleViewManager = null;
        TicketManager ObjTicketManager = null;
        ScheduleActionsManager scheduleActionsManager = null;
        ConfigurationVariableManager configVariableManager = null;

        public AgentJobHelper(ApplicationContext context)
        {
            _context = context;
            user = _context.CurrentUser;
            ObjModuleViewManager = new ModuleViewManager(_context);
            ObjTicketManager = new TicketManager(_context);
            scheduleActionsManager = new ScheduleActionsManager(_context);
            configVariableManager = new ConfigurationVariableManager(_context);
        }

        // Properties and Member
        public bool IsItemDelete { get; set; }

        #region Escalation Email
        /// <summary>
        /// Escalations the e mail.
        /// </summary>
        /// <param name="scheduleAction">The schedule action.</param>
        public void EscalationEMail(SchedulerAction scheduleAction)
        {
            // Check if escalations are enabled
            bool escalationsEnabled = configVariableManager.GetValueAsBool(ConfigConstants.EnableEscalations);
            if (!escalationsEnabled)
                return;
            ULog.WriteLog("Sending Escalation Mail :" + scheduleAction.TicketId +","+ scheduleAction.TenantID);
            // Check if the ticket is on hold or closed
            // We shouldn't need this since escalations are not generated for these tickets, but sanity check to prevent user frustration!
            string ticketId = scheduleAction.TicketId;
            string moduleName = uHelper.getModuleNameByTicketId(ticketId);
            DataRow ticketItem = Ticket.GetCurrentTicket(_context, moduleName, ticketId);
            if (ticketItem == null)
            {
                CreateLogs(scheduleAction, AgentJobStatus.Fail, "Ticket not found.");
                IsItemDelete = true;
                return;
            }
            if (UGITUtility.StringToBoolean(ticketItem[DatabaseObjects.Columns.TicketOnHold]) || UGITUtility.StringToBoolean(ticketItem[DatabaseObjects.Columns.TicketClosed]))
            {
                // Entry leftover in error, delete it!
                // Log.WriteLog(string.Format("Aborting escalation for closed or on-hold ticket {0}", ticketId), TraceSeverity.Medium, EventSeverity.Error);
                IsItemDelete = true;
                return;
            }
            // Log.WriteLog("EscalationEMail for ticket: " + ticketId);
            // All good so far, send the escalation email!
            string logs = SendMail(scheduleAction);
            if (string.IsNullOrEmpty(logs))
            {
                // Success! Log status
                CreateLogs(scheduleAction, AgentJobStatus.Success, logs);

                // Reschedule if recurring, else just delete
                if (scheduleAction.Recurring)
                {
                    UpdateIsRecurrence(scheduleAction, null);
                }
                else
                    IsItemDelete = true;
            }
            else
            {
                // Failed, log status
                CreateLogs(scheduleAction, AgentJobStatus.Fail, logs);
            }
        }
        #endregion

        #region Methods for Auto Stage Move
        // <summary>
        // Automatics the stage move.
        // </summary>
        // <param name="scheduleAction">The schedule action.</param>
        public void AutoStageMove(SchedulerAction scheduleAction)
        {
            try
            {
                string moduleName = scheduleAction.ModuleNameLookup;
                Ticket ticketRequest = new Ticket(_context, moduleName);
                string ticketID = scheduleAction.TicketId;

                DataRow currentTicket = Ticket.GetCurrentTicket(_context, moduleName, ticketID);
                if (currentTicket == null)
                {
                    ULog.WriteLog(string.Format("ERROR in AutoStageMove: ticket with ID {0} not found!", ticketID));
                    IsItemDelete = true;
                    return;
                }

                // Check whether the ticket is still on same stage where it is been loged.
                int currentstageId = Convert.ToInt32(currentTicket[DatabaseObjects.Columns.StageStep]);
                bool onHoldStatus = UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.TicketOnHold]);
                Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(scheduleAction.CustomProperties);

                if (customProperties.ContainsKey(CustomProperties.ScheduledTriggerStageid))
                {
                    int stageid = 0;
                    if (!int.TryParse(customProperties[CustomProperties.ScheduledTriggerStageid], out stageid))
                    {
                        CreateLogs(scheduleAction, AgentJobStatus.NoAction, "Stage id not available in custom properties of Schedule action.");
                        IsItemDelete = true;
                        return;
                    }
                    if (stageid != currentstageId)
                    {
                        CreateLogs(scheduleAction, AgentJobStatus.NoAction, "Ticket(" + scheduleAction.TicketId + ") is moved to another stage.");
                        IsItemDelete = true;
                        return;
                    }
                }
                else
                {
                    CreateLogs(scheduleAction, AgentJobStatus.NoAction, "Stage information is missing from custom property field.");
                    IsItemDelete = true;
                    return;
                }

                ULog.WriteLog("AutoStageMove for ticket: " + ticketID);

                List<TicketColumnError> cErrors = new List<TicketColumnError>();
                ticketRequest.ApproveTicket(cErrors, currentTicket, true, user, true);

                if (cErrors != null && cErrors.Count == 0)
                {
                    // Code to close child tickets as well
                    LifeCycleStage newCurrentStage = ticketRequest.GetTicketCurrentStage(currentTicket);

                    if (newCurrentStage != null && (newCurrentStage.StageTypeChoice == Convert.ToString(StageType.Resolved) || newCurrentStage.StageTypeChoice == Convert.ToString(StageType.Closed)) &&
                        (configVariableManager.GetValue(ConfigConstants.AutoCloseChildTickets) == "Always"))
                    {
                        string resolutionType = UGITUtility.GetSPItemValueAsString(currentTicket, DatabaseObjects.Columns.TicketResolutionType);
                        UserProfile prp = uHelper.GetUser(_context, currentTicket, DatabaseObjects.Columns.TicketPRP);

                        // Close all child tickets
                        TicketRelationshipHelper ticketHelper = new TicketRelationshipHelper(_context);
                        ticketHelper.CloseTickets(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), 2, string.Empty, user, resolutionType);    // type=2 means filter child ticket
                    }
                }

                string errmsg = string.Empty;
                if (cErrors != null && cErrors.Count > 0)
                    errmsg = string.Join("; ", cErrors.Select(x => string.Format("{0}: {1} ", x.InternalFieldName, x.Message)));

                CreateLogs(scheduleAction, AgentJobStatus.Success, errmsg);
                IsItemDelete = true;
            }
            catch (Exception ex)
            {
                IsItemDelete = false;
                CreateLogs(scheduleAction, AgentJobStatus.Fail, ex.ToString());
            }
        }

        //<summary>
        // Schedules the automatic stage.
        // </summary>
        //<param name = "currentTicket" > The current ticket.</param>
        // <param name = "customProperty" > The custom property.</param>
        // <param name = "moduleid" > The moduleid.</param>
        public void ScheduleAutoStage(DataRow currentTicket, string customProperty, string moduleName)
        {
            Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(customProperty, Constants.Separator, false, true);

            if (customProperties.ContainsKey(CustomProperties.ScheduledAutoStage) && (customProperties.ContainsKey(CustomProperties.TicketAutoCloseAfter) || customProperties.ContainsKey(CustomProperties.ScheduledTriggerFieldName)))
            {
                bool autostage = UGITUtility.StringToBoolean(customProperties[CustomProperties.ScheduledAutoStage]);
                if (autostage)
                {
                    int ticketAutoCloseAfter = 0;
                    if (customProperties.ContainsKey(CustomProperties.TicketAutoCloseAfter))
                        ticketAutoCloseAfter = UGITUtility.StringToInt(customProperties[CustomProperties.TicketAutoCloseAfter]);

                    string startTimeField = DatabaseObjects.Columns.CurrentStageStartDate;
                    if (customProperties.ContainsKey(CustomProperties.ScheduledTriggerFieldName))
                        startTimeField = Convert.ToString(customProperties[CustomProperties.ScheduledTriggerFieldName]);



                    if (UGITUtility.IsSPItemExist(currentTicket, startTimeField))
                    {
                        DateTime startTime = Convert.ToDateTime(currentTicket[startTimeField]);
                        startTime = startTime.AddDays(ticketAutoCloseAfter);
                        //SPFieldLookupValue spLookupValue = new SPFieldLookupValue(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]));
                        //int stageId = spLookupValue.LookupId;
                        int stageId = Convert.ToInt32(currentTicket[DatabaseObjects.Columns.StageStep]);
                        ScheduleAutoStage(startTime, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), moduleName, stageId);
                    }
                }
            }
        }

        // <summary>
        // Schedules the automatic stage.
        // </summary>
        // <param name = "startTime" > The start time.</param>
        // <param name = "ticketId" > The ticket identifier.</param>
        // <param name = "moduleid" > The moduleid.</param>
        // <param name = "stageId" > The stage identifier.</param>
        private void ScheduleAutoStage(DateTime startTime, string ticketId, string moduleid, int stageId)
        {
            Dictionary<string, object> scheduleDic = new Dictionary<string, object>();

            scheduleDic.Add(DatabaseObjects.Columns.StartTime, startTime);
            scheduleDic.Add(DatabaseObjects.Columns.Title, ticketId.ToString() + " " + UGITUtility.getDateStringInFormat(startTime, true));
            scheduleDic.Add(DatabaseObjects.Columns.TicketId, ticketId);
            scheduleDic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.AutoStageMove);
            scheduleDic.Add(DatabaseObjects.Columns.ModuleNameLookup, moduleid);
            scheduleDic.Add(DatabaseObjects.Columns.Recurring, false);

            StringBuilder customProperties = new StringBuilder();
            customProperties.Append(string.Format("{0}={1}", CustomProperties.ScheduledTriggerStageid, stageId));
            scheduleDic.Add(DatabaseObjects.Columns.CustomProperties, customProperties.ToString());

            CreateSchedule(scheduleDic);
        }
        #endregion Methods for Auto Stage Move

        #region Unhold Ticket
        /// <summary>
        /// Automatics the unhold ticket.
        /// </summary>
        /// <param name="scheduleAction">The schedule action.</param>
        public void AutoUnholdTicket(SchedulerAction scheduleAction)
        {
            string moduleNameLookup;
            try
            {
                moduleNameLookup = scheduleAction.ModuleNameLookup;
                Ticket ticketRequest = new Ticket(_context, moduleNameLookup);
                string ticketTableName = ticketRequest.Module.ModuleTable;
                string ticketID = scheduleAction.TicketId;
                DataRow currentTicket = ObjTicketManager.GetTicketTableBasedOnTicketId(moduleNameLookup, ticketID).Rows[0];

                if (currentTicket == null)
                {
                    // Ticket not found, maybe it was deleted
                    ULog.WriteLog(string.Format("ERROR in AutoUnholdTicket: ticket with ID {0} not found!", ticketID));
                    IsItemDelete = true;
                    return;
                }

                bool onHoldStatus = UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.TicketOnHold]);
                if (!onHoldStatus)
                {
                    // Not on hold, nothing to do
                    CreateLogs(scheduleAction, AgentJobStatus.NoAction, "Ticket(" + scheduleAction.TicketId + ") is already in unhold mode.");
                    IsItemDelete = true;
                    return;
                }
                else
                {
                    string comment = string.Format("{0} re-opened due to expiration of hold time", UGITUtility.moduleTypeName(ticketRequest.Module.ModuleName));
                    string customPropertiesList = Convert.ToString(scheduleAction.CustomProperties);
                    Dictionary<string, string> customProperties = UGITUtility.GetCustomProperties(customPropertiesList, Constants.Separator, false, true);
                    bool closeTicketOnHoldExpiration = false;

                    if (customProperties.ContainsKey(CustomProperties.CloseTicketOnHoldExpiration))
                    {
                        closeTicketOnHoldExpiration = UGITUtility.StringToBoolean(customProperties[CustomProperties.CloseTicketOnHoldExpiration]);
                        if (closeTicketOnHoldExpiration)
                            comment = string.Format("{0} closed due to expiration of hold time", UGITUtility.moduleTypeName(ticketRequest.Module.ModuleName));
                    }

                    ticketRequest.UnHoldTicket(currentTicket, moduleNameLookup, comment, true, closeTicketOnHoldExpiration);
                    ticketRequest.CommitChanges(currentTicket);
                    //  UpdateSPListItem(currentTicket);
                    ticketRequest.SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleNameLookup, UGITUtility.WrapCommentForEmail(comment, "HoldExpired"), string.Empty);
                    CreateLogs(scheduleAction, AgentJobStatus.Success, string.Empty);
                }

                IsItemDelete = true;
            }
            catch (Exception ex)
            {
                IsItemDelete = false;
                CreateLogs(scheduleAction, AgentJobStatus.Fail, ex.ToString());
            }
        }

        public void ScheduleUnholdTask(UGITTask currentTicket, string moduleid, bool isEnableDeleteOnHold = false)
        {
            bool isOnHold = UGITUtility.StringToBoolean(currentTicket.OnHold);
            if (isOnHold)
            {
                DateTime holdDate = Convert.ToDateTime(currentTicket.OnHoldTillDate);
                Dictionary<string, object> scheduleDic = new Dictionary<string, object>();
                scheduleDic.Add(DatabaseObjects.Columns.StartTime, holdDate);
                scheduleDic.Add(DatabaseObjects.Columns.Title, string.Format("{0}({1}) {2}", currentTicket.Title, Convert.ToString(currentTicket.TicketId), UGITUtility.GetDateStringInFormat(holdDate, true)));
                scheduleDic.Add(DatabaseObjects.Columns.TicketId, Convert.ToString(currentTicket.ID));
                scheduleDic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.UnHoldTask);
                scheduleDic.Add(DatabaseObjects.Columns.ModuleNameLookup, moduleid);
                scheduleDic.Add(DatabaseObjects.Columns.Recurring, false);
                scheduleDic.Add(DatabaseObjects.Columns.ListName, DatabaseObjects.Tables.ModuleTasks);
                StringBuilder customProperties = new StringBuilder();
                customProperties.Append(string.Format("{0}={1}", CustomProperties.CloseTicketOnHoldExpiration, isEnableDeleteOnHold));
                scheduleDic.Add(DatabaseObjects.Columns.CustomProperties, customProperties.ToString());

                CreateSchedule(scheduleDic);
            }
        }

        /// <summary>
        /// Schedules the unhold ticket.
        /// </summary>
        /// <param name="currentTicket">The current ticket.</param>
        /// <param name="moduleid">The moduleid.</param>
        public void ScheduleUnholdTicket(DataRow currentTicket, string moduleid, bool isEnableDeleteOnHold = false)
        {
            bool isOnHold = UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.TicketOnHold]);
            if (isOnHold)
            {
                DateTime holdDate = Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.TicketOnHoldTillDate]);
                Dictionary<string, object> scheduleDic = new Dictionary<string, object>();
                scheduleDic.Add(DatabaseObjects.Columns.StartTime, holdDate);
                scheduleDic.Add(DatabaseObjects.Columns.Title, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]) + " " + UGITUtility.getDateStringInFormat(holdDate, true));
                scheduleDic.Add(DatabaseObjects.Columns.TicketId, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
                scheduleDic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.UnHoldTicket);
                scheduleDic.Add(DatabaseObjects.Columns.ModuleNameLookup, moduleid);
                scheduleDic.Add(DatabaseObjects.Columns.Recurring, false);

                StringBuilder customProperties = new StringBuilder();
                customProperties.Append(string.Format("{0}={1}", CustomProperties.CloseTicketOnHoldExpiration, isEnableDeleteOnHold));
                scheduleDic.Add(DatabaseObjects.Columns.CustomProperties, customProperties.ToString());
                CreateSchedule(scheduleDic);
            }
        }

        /// <summary>
        /// Cancels the un hold ticket.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        public void CancelUnHoldTicket(string ticketId)
        {
            List<SchedulerAction> itemColl = GetScheduleActionItemByTicketId(ticketId, ScheduleActionType.UnHoldTicket); 
            if (itemColl != null && itemColl.Count > 0)
            {
                scheduleActionsManager.Delete(itemColl);
            }
        }
        #endregion

        #region Send Reminder
        public void SendReminder(SchedulerAction scheduleAction)
        {
            try
            {
                string listName = scheduleAction.ListName;
                if (!string.IsNullOrEmpty(listName))
                {
                    long ticketId = UGITUtility.StringToInt(scheduleAction.TicketId);
                    if (ticketId > 0) // If its a non-zero, then it points to a task (not ticket)
                    {
                        string condition = $"{DatabaseObjects.Columns.ID}={ticketId} and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'";
                        DataRow task = GetTableDataManager.GetTableData(listName, condition).Select().First();

                        if (UGITUtility.IsSPItemExist(task, DatabaseObjects.Columns.Status))
                        {
                            string status = Convert.ToString(task[DatabaseObjects.Columns.Status]);

                            // If the task is a child task of SVC Ticket
                            bool isSVCTask = UGITUtility.IsSPItemExist(task, DatabaseObjects.Columns.TicketId) && uHelper.getModuleNameByTicketId(Convert.ToString(task[DatabaseObjects.Columns.TicketId])) == ModuleNames.SVC
                                && UGITUtility.IsSPItemExist(task, DatabaseObjects.Columns.UGITSubTaskType) && Convert.ToString(task[DatabaseObjects.Columns.UGITSubTaskType]).ToLower() == "task" && listName == DatabaseObjects.Tables.ModuleTasks;

                            // Check task status before sending notification
                            if (isSVCTask)
                            {
                                if (status == Constants.Completed)
                                {
                                    IsItemDelete = true;
                                    ULog.WriteLog(string.Format("JobScheduler: Cancelled reminder for {0} {1} since task is already completed", DatabaseObjects.Tables.ModuleTasks, ticketId));
                                    return;
                                }
                                else if (status == Constants.Waiting)
                                {
                                    // For waiting tasks, cancel current reminder but still need to schedule further recurring reminders if configured
                                    ULog.WriteLog(string.Format("JobScheduler: Cancelled reminder for {0} {1} since task is in Waiting status", DatabaseObjects.Tables.ModuleTasks, ticketId));
                                    if (scheduleAction.Recurring)
                                    {
                                        DateTime dRecurringEndDate = UGITUtility.StringToDateTime(scheduleAction.RecurringEndDate);
                                        UpdateIsRecurrence(scheduleAction, dRecurringEndDate);
                                    }
                                    return;
                                }
                                else if (status != Constants.InProgress && status != Constants.NotStarted && status != Constants.Pending)
                                {
                                    IsItemDelete = true;
                                    ULog.WriteLog(string.Format("JobScheduler: Cancelled reminder for {0} {1} since task is closed or on-hold", DatabaseObjects.Tables.ModuleTasks, ticketId));
                                    return;
                                }
                            }
                            if (Convert.ToString(task[DatabaseObjects.Columns.Status]) == Constants.Completed)
                            {
                                IsItemDelete = true;
                                ULog.WriteLog(string.Format("JobScheduler: Cancelled reminder for {0} {1} since task is already completed", listName, ticketId));
                                return;
                            }
                        }

                        if (UGITUtility.IsSPItemExist(task, DatabaseObjects.Columns.ReportInstanceStatus))
                        {
                            string instanceStatus = Convert.ToString(task[DatabaseObjects.Columns.ReportInstanceStatus]);
                            if (instanceStatus != "Pending" && instanceStatus != "Past Due")
                            {
                                IsItemDelete = true;
                                ULog.WriteLog(string.Format("JobScheduler: Cancelled reminder for {0} {1} since its not due any more", listName, ticketId));
                                return;
                            }
                        }
                    }
                }

                DateTime recurringEndDate = UGITUtility.StringToDateTime(scheduleAction.RecurringEndDate);
                if (recurringEndDate != DateTime.MinValue && recurringEndDate < DateTime.MinValue)
                {
                    IsItemDelete = true;
                    return;
                }

                ULog.WriteLog("SendReminder for ticket: " + scheduleAction.TicketId);

                string logs = SendMail(scheduleAction);
                if (string.IsNullOrEmpty(logs))
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Success, logs);

                    if (scheduleAction.Recurring)
                        UpdateIsRecurrence(scheduleAction, recurringEndDate);
                    else
                        IsItemDelete = true;
                }
                else
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Fail, logs);
                }
            }
            catch (Exception ex)
            {
                IsItemDelete = false;
                CreateLogs(scheduleAction, AgentJobStatus.Fail, ex.ToString());
            }
        }

        /// <summary>
        /// Automatics the unhold task.
        /// </summary>
        /// <param name="scheduleAction">The schedule action.</param>
        public void AutoUnholdTask(SchedulerAction scheduleAction)
        {
            try
            {
                string moduleName = scheduleAction.ModuleNameLookup;
                string taskTableName = scheduleAction.ListName;

                if (string.IsNullOrEmpty(taskTableName) || taskTableName.ToLower() != DatabaseObjects.Tables.ModuleTasks.ToLower())
                    return;

                long taskId = UGITUtility.StringToInt(scheduleAction.TicketId);
                if (taskId == 0) // If its a non-zero, then it points to a task (not ticket)
                    return;

                UGITTaskManager TaskManager = new UGITTaskManager(_context);
                UGITTask task = TaskManager.LoadByID(taskId);

                if (task == null)
                {
                    // Task not found, maybe it was deleted
                    ULog.WriteLog(string.Format("ERROR in AutoUnholdTask: Task with ID {0} not found!", taskId));
                    IsItemDelete = true;
                    return;
                }

                if (!task.OnHold)
                {
                    // Not on hold, nothing to do
                    CreateLogs(scheduleAction, AgentJobStatus.NoAction, "ERROR in AutoUnholdTask: Task(" + taskId + ") is not on hold!");
                    IsItemDelete = true;
                    return;
                }

                string comment = string.Format("Task re-opened due to expiration of hold time");
                TaskManager.TaskUnHold(comment, task, holdExpired: true);

                // Notification for task assigned when task reopen due to expriration of hold time.
                if (moduleName == ModuleNames.SVC && !string.IsNullOrEmpty(task.AssignedTo))
                {
                    List<string> emails = new List<string>();
                    StringBuilder mailToNames = new StringBuilder();
                    UserProfileManager UserManager = new UserProfileManager(_context);
                    List<UserProfile> users = UserManager.GetUserInfosById(task.AssignedTo);

                    foreach (UserProfile userProfile in users)
                    {
                        if (userProfile != null && !string.IsNullOrEmpty(userProfile.Email))
                        {
                            emails.Add(userProfile.Email);
                            if (mailToNames.Length != 0)
                                mailToNames.Append(", ");
                            mailToNames.Append(userProfile.Name);
                        }
                    }

                    if (emails.Count > 0)
                    {
                        Dictionary<string, string> taskToEmail = new Dictionary<string, string>();
                        taskToEmail.Add(DatabaseObjects.Columns.ProjectID, task.TicketId); //taskid
                        taskToEmail.Add(DatabaseObjects.Columns.ProjectTitle, scheduleAction.Title);
                        taskToEmail.Add(DatabaseObjects.Columns.Title, task.Title);
                        taskToEmail.Add(DatabaseObjects.Columns.LinkDescription, task.Description);
                        taskToEmail.Add(DatabaseObjects.Columns.StartDate, task.StartDate.ToString());
                        taskToEmail.Add(DatabaseObjects.Columns.DueDate, task.EndDate.ToString());
                        taskToEmail.Add(DatabaseObjects.Columns.EstimatedHours, task.EstimatedHours.ToString());
                        taskToEmail.Add("IsService", "true");

                        string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", task.TicketId, task.ID, ModuleNames.SVC);
                        string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, _context.TenantID);
                        string greeting = configVariableManager.GetValue("Greeting");
                        string signature = configVariableManager.GetValue("Signature");

                        StringBuilder taskEmail = new StringBuilder();
                        taskEmail.AppendFormat("{0} {1}<br /><br />", greeting, mailToNames.ToString());
                        taskEmail.AppendFormat("Task <b>\"{0}\"</b> re-opened due to expiration of hold time.<br>", task.Title);
                        taskEmail.Append("<br /><br />" + signature + "<br />");
                        taskEmail.Append(emailFooter);

                        string emailSubject = string.Format("Task {0} re-opened due to expiration of hold time.", task.Title);

                        MailMessenger mailMessage = new MailMessenger(_context);
                        if (configVariableManager.GetValueAsBool(ConfigConstants.KeepSVCTaskNotifications))
                            mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { }, saveToTicketId: task.ParentInstance); // Pass ticketID to save email
                        else
                            mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { });
                        mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { }, true);
                    }
                }

                ULog.WriteLog(comment);
                CreateLogs(scheduleAction, AgentJobStatus.Success, string.Empty);
                IsItemDelete = true;
            }
            catch (Exception ex)
            {
                IsItemDelete = false;
                CreateLogs(scheduleAction, AgentJobStatus.Fail, ex.ToString());
            }
        }

        /// <summary>
        /// Updates the schedule action email.
        /// </summary>
        /// <param name="currentTicket">The current ticket.</param>
        /// <param name="moduleid">The moduleid.</param>
        public void UpdateDRQScheduleActionEmail(DataRow currentTicket, int moduleid)
        {
            string ticketId = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);

            if (!UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.IsUserNotificationRequired]) ||
                !UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.AutoSend]) ||
                UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.TicketClosed]))
            {
                List<SchedulerAction> itemcoll = GetScheduleActionItemByTicketId(ticketId, ScheduleActionType.Reminder);
                ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(_context);
                while (itemcoll != null && itemcoll.Count > 0)
                {
                    SchedulerAction sa= itemcoll[0];
                    scheduleActionsManager.Delete(itemcoll[0]);
                }
                return;
            }


            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(DatabaseObjects.Columns.Title, string.Format("{0} {1}", ticketId, Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.TicketToBeSentByDate])));
            dic.Add(DatabaseObjects.Columns.StartTime, currentTicket[DatabaseObjects.Columns.TicketToBeSentByDate]);
            dic.Add(DatabaseObjects.Columns.TicketId, ticketId);
            dic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.Reminder);
            dic.Add(DatabaseObjects.Columns.EmailIDTo, currentTicket[DatabaseObjects.Columns.TicketRequestor]);
            dic.Add(DatabaseObjects.Columns.MailSubject, configVariableManager.GetValue(ConfigConstants.DRQNotificationSubject));

            dic.Add(DatabaseObjects.Columns.Recurring, false);

            StringBuilder emailBody = new StringBuilder();
            emailBody.Append(Convert.ToString(currentTicket[DatabaseObjects.Columns.NotificationText]));
            string Footer = uHelper.GetTicketDetailsForEmailFooter(_context, currentTicket, uHelper.getModuleNameByTicketId(ticketId), true, false);
            emailBody.Append(Footer);

            dic.Add(DatabaseObjects.Columns.EmailBody, emailBody.ToString());
            dic.Add(DatabaseObjects.Columns.ModuleNameLookup, moduleid);

            CreateSchedule(dic);
        }

        public void UpdateCMTScheduleActionReminder(DataRow currentTicket, int moduleid)
        {
            string ticketId = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
            string emailTo = getEmailIdFromUserInfo(Convert.ToString(currentTicket[DatabaseObjects.Columns.ReminderTo]), false);
            if (string.IsNullOrEmpty(emailTo) || currentTicket[DatabaseObjects.Columns.ReminderDate] == null ||
                UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.TicketClosed]))
            {
                List<SchedulerAction> itemcoll = GetScheduleActionItemByTicketId(ticketId, ScheduleActionType.Reminder);
                ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(_context);
                while (itemcoll != null && itemcoll.Count > 0)
                {
                    SchedulerAction sa = itemcoll[0];
                    scheduleActionsManager.Delete(itemcoll[0]);
                }
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(DatabaseObjects.Columns.Title, string.Format("{0} {1}", ticketId, Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.ReminderDate])));
            dic.Add(DatabaseObjects.Columns.StartTime, currentTicket[DatabaseObjects.Columns.ReminderDate]);
            dic.Add(DatabaseObjects.Columns.TicketId, ticketId);
            dic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.Reminder);
            dic.Add(DatabaseObjects.Columns.EmailIDTo, emailTo);

            ///to get subject from configurationvariable table.
            string subject = configVariableManager.GetValue(ConfigConstants.CMTReminderSubject);
            EscalationProcess escalationProcess = new EscalationProcess(_context);
            if (string.IsNullOrEmpty(subject))
            {
                subject = string.Format("Reminder for contract {0}: {1}",
                                        ticketId, Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]));
            }
            else
            {

                string[] tokens = uHelper.GetMyTokens(subject);
                subject = escalationProcess.ReplaceMyTokensWithTicketValues(currentTicket, subject, tokens);
            }

            dic.Add(DatabaseObjects.Columns.MailSubject, subject);

            ///Recurring Interval 
            dic.Add(DatabaseObjects.Columns.Recurring, true);
            int interval = uHelper.GetRecurringIntervalInMinutes(_context, Convert.ToString(currentTicket[DatabaseObjects.Columns.RepeatInterval]),
                                                Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.ReminderDate]));
            dic.Add(DatabaseObjects.Columns.RecurringInterval, interval);

            dic.Add(DatabaseObjects.Columns.EmailBody, currentTicket[DatabaseObjects.Columns.ReminderBody]);
            dic.Add(DatabaseObjects.Columns.ModuleNameLookup, moduleid);
            dic.Add(DatabaseObjects.Columns.RecurringEndDate, currentTicket[DatabaseObjects.Columns.ContractExpirationDate]);
            dic.Add(DatabaseObjects.Columns.CustomProperties, string.Format("{0}={1}", CustomProperties.ScheduledRepeatInterval,
                                                              Convert.ToString(currentTicket[DatabaseObjects.Columns.RepeatInterval])));
            CreateSchedule(dic);
        }
       
        #endregion

        #region Execute Query
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="scheduleAction">The schedule action.</param>
        public void ExecuteQuery(SchedulerAction scheduleAction)
        {
            try
            {
                long queryID = UGITUtility.StringToInt(scheduleAction.TicketId);
                if (queryID == 0) // If its a non-zero, then it points to a Query
                    return;

                string title = scheduleAction.Title;
                //To get Schedule action type 
                ScheduleActionType actionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), Convert.ToString(scheduleAction.ActionType));
                List<string> whereValue = new List<string>();
                DataTable dataTable = new DataTable();
                string serialized = string.Empty;

                if (!string.IsNullOrEmpty(scheduleAction.ActionTypeData))
                    serialized = scheduleAction.ActionTypeData;
                else if (string.IsNullOrEmpty(serialized) && !string.IsNullOrEmpty(scheduleAction.CustomProperties))
                    serialized = scheduleAction.CustomProperties;

                List<string> strCustomPropOther = new List<string>();
                //if we provide file name in case of ftp.
                if (!string.IsNullOrEmpty(Convert.ToString(scheduleAction.CustomProperties)))
                {
                    strCustomPropOther = Convert.ToString(scheduleAction.CustomProperties).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<string> strFileName = strCustomPropOther[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (strFileName.Count > 1 && !string.IsNullOrEmpty(strFileName[1]))
                        title = UGITUtility.ReplaceInvalidCharsInFolderName(strFileName[1]);
                }

                Dictionary<string, object> reportOptions = UGITUtility.DeserializeDicObject(serialized);

                if (reportOptions != null && reportOptions.ContainsKey(ReportScheduleConstant.Where))
                {
                    string where = Convert.ToString(reportOptions[ReportScheduleConstant.Where]);
                    whereValue = UGITUtility.ConvertStringToList(where, ",");
                }

                // Load the dashboard
                DashboardManager dManager = new DashboardManager(_context);
                Dashboard uDashboard = null;

                if (queryID > 0)
                    uDashboard = dManager.LoadPanelById(queryID, false);

                if (uDashboard == null)
                    return;

                DashboardQuery dashboardQuery = uDashboard.panel as DashboardQuery;
                QueryHelperManager queryHelper = new QueryHelperManager(_context);
                DataTable dtTotals = new DataTable();

                bool isParameterize = dashboardQuery.QueryInfo.WhereClauses.Exists(w => w.ParameterName != string.Empty);
                if (isParameterize)
                {
                    string whereFilter = string.Join(",", whereValue.ToArray());
                    dataTable = queryHelper.GetReportData(dashboardQuery.QueryInfo, whereFilter, ref dtTotals, false);
                }
                else
                {
                    dataTable = queryHelper.GetReportData(dashboardQuery.QueryInfo, string.Empty, ref dtTotals, false);
                }

                if (dataTable == null || dataTable.Rows.Count == 0)
                    return;

                string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid();
                string fileName = string.Empty;

                string attachFormat = reportOptions.ContainsKey(ReportScheduleConstant.AttachmentFormat) ? Convert.ToString(reportOptions[ReportScheduleConstant.AttachmentFormat]) : string.Empty;

                if (string.IsNullOrEmpty(attachFormat))
                    attachFormat = scheduleAction.AttachmentFormat;
                if (string.IsNullOrEmpty(attachFormat))
                    attachFormat = "pdf";

                ASPxGridView gridView = new ASPxGridView();
                QueryPreviewHelperManager queryPreviewHelper = new QueryPreviewHelperManager(queryID, gridView, _context);
                gridView = queryPreviewHelper.InitializeGrid();
                queryPreviewHelper.query = dashboardQuery;
                queryPreviewHelper.ResultData = dataTable;
                queryPreviewHelper.zoomLevel = ZoomLevel.Quarterly;
                gridView = queryPreviewHelper.BindGrid(dataTable);

                // Generate Report
                XtraReport report = GenerateReport(queryPreviewHelper, gridView, title, attachFormat, 8F);

                // Export file
                string errMsg = string.Empty;
                if (attachFormat == "csv" && actionType == ScheduleActionType.Query)
                {
                    fileName = title + ".csv";

                    // Replace CSS/HTML tags in PMM Project Health column with plain-text
                    if (dashboardQuery.QueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
                    {
                        if (dashboardQuery.QueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects)
                                           .Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth))
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (DataColumn column in dataTable.Columns)
                                {
                                    string data = Convert.ToString(row[column]);

                                    if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                                    {
                                        row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? "Green" : (data.ToLower().Contains("class='yellowled monitoricon'") ? "Yellow" : (data.ToLower().Contains("class='redled monitoricon'") ? "Red" : string.Empty));
                                    }
                                }
                            }
                        }
                    }

                    // Remove unselected columns from result set based on query
                    if (dashboardQuery != null && dashboardQuery.QueryInfo != null && dashboardQuery.QueryInfo.Tables != null && dataTable != null && dataTable.Rows.Count > 0)
                    {
                        List<string> removeUnselectedCols = new List<string>();
                        List<string> selectedCols = new List<string>();
                        dashboardQuery.QueryInfo.Tables.ToList().ForEach(x =>
                        {
                            selectedCols.AddRange(x.Columns.Where(y => y.Selected).Select(y => y.DisplayName).ToList());
                        });

                        List<string> allColumns = dataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                        removeUnselectedCols = allColumns.Except(selectedCols).ToList();
                        if (removeUnselectedCols.Count > 0)
                        {
                            removeUnselectedCols.ForEach(x =>
                            {
                                dataTable.Columns.Remove(x);
                            });
                        }
                    }

                    string csvOutput = UGITUtility.ConvertTableToCSV(dataTable);
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    File.WriteAllText(Path.Combine(filePath, fileName), csvOutput);
                    filePath = Path.Combine(filePath, fileName);
                }
                else
                    filePath = ExportFiles(report, attachFormat, filePath, title);

                if (Convert.ToString(scheduleAction.FileLocation).ToLower() == "ftp")
                {
                    ULog.WriteLog("Uploading scheduled query to FTP location ...");
                    FTPConfiguration ftpconfig = new FTPConfiguration();

                    //code for ftp goes here...
                    List<string> strFtpurl = strCustomPropOther[1].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> strftpCredential = strCustomPropOther[2].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (strFtpurl.Count > 1 && !string.IsNullOrEmpty(strFtpurl[1]))
                        ftpconfig.FtpBaseUrl = strFtpurl[1];

                    if (strftpCredential.Count > 1 && !string.IsNullOrEmpty(strftpCredential[1]))
                        ftpconfig.FtpCredential = strftpCredential[1];

                    List<string> lstfilePath = new List<string>();
                    lstfilePath.Add(filePath);

                    FTPHelper ftpHelp = new FTPHelper(_context, ftpconfig);
                    errMsg = ftpHelp.UploadFiles(lstfilePath);
                }
                else
                {
                    string[] attachments = { filePath };
                    errMsg = SendMail(scheduleAction, attachments);
                }
                if (string.IsNullOrEmpty(errMsg))
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Success, errMsg);

                    if (scheduleAction.Recurring)
                    {
                        UpdateIsRecurrence(scheduleAction, scheduleAction.RecurringEndDate);
                    }
                    else
                    {
                        IsItemDelete = true;
                    }
                }
                else
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Fail, errMsg);
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "AgentJobHelper >> ExecuteQuery (scheduleAction) ");
            }
        }

        private XtraReport GenerateReport(QueryPreviewHelperManager queryHelper, ASPxGridView gridView, string title, string attachFormat, float fontSize)
        {
            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            if (queryHelper.query.QueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
            {
                if (queryHelper.query.QueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects)
                                   .Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth))
                {
                    queryHelper.ResultData = UpdateDataTableForExcelOrPdf(queryHelper.ResultData, attachFormat);
                }
            }
            queryHelper.FillGantViewdata();
            return reportHelper.GenerateReport(gridView, queryHelper.ResultData, title, fontSize, attachFormat);
        }

        DataTable UpdateDataTableForExcelOrPdf(DataTable dt, string fileformat)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    string data = Convert.ToString(row[column]);

                    if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                    {
                        if (fileformat == "pdf")
                            row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : GetImage("GreenLED")));
                        else if (fileformat == "xls")
                            row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? "Green" : (data.ToLower().Contains("class='yellowled monitoricon'") ? "Yellow" : (data.ToLower().Contains("class='redled monitoricon'") ? "Red" : "Green"));
                    }
                }
            }
            return dt;
        }

        private object GetImage(string className)
        {
            string fileName = string.Empty;
            switch (className)
            {
                case "GreenLED":
                    fileName = _context.SiteUrl + @"/Content/images/LED_Green.png";
                    break;
                case "YellowLED":
                    fileName = _context.SiteUrl + @"/Content/images/LED_Yellow.png";
                    break;
                case "RedLED":
                    fileName = _context.SiteUrl + @"/Content/images/LED_Red.png";
                    break;
                default:
                    break;
            }
            return fileName;
        }

        private void reportHelper_CustomizeColumn(object source, ControlCustomizationEventArgs e)
        {

        }

        private void reportHelper_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e)
        {
            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Title"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Title").ColumnWidth *= 2;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Rank"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Rank").ColumnWidth = 50;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Priority"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Priority").ColumnWidth = 50;
        }

        /// <summary>
        /// Exports the files.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="attachFormat">The attach format.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="title">The title.</param>
        /// <returns>System.String.</returns>
        private string ExportFiles(XtraReport report, string attachFormat, string filePath, string title)
        {
            string fileName = string.Empty;
            if (attachFormat == "xls")
            {
                fileName = title + ".xls";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                report.ExportToXls(Path.Combine(filePath, fileName));
            }
            else if (attachFormat == "pdf")
            {
                fileName = title + ".pdf";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                report.ExportToPdf(Path.Combine(filePath, fileName));
            }
            else if (attachFormat == "csv")
            {
                fileName = title + ".csv";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                report.ExportToCsv(Path.Combine(filePath, fileName));
            }

            return Path.Combine(filePath, fileName);
        }

        #endregion

        #region Common Function
        private void CreateLogs(SchedulerAction scheduleActionItem, AgentJobStatus jobStatus, string logMessage)
        {
            ScheduleActionsArchiveManager objScheduleActionsArchiveManager = new ScheduleActionsArchiveManager(_context);
            SchedulerActionArchive objScheduleActionsArchive = new SchedulerActionArchive();
            objScheduleActionsArchive.Title = scheduleActionItem.Title;
            objScheduleActionsArchive.StartTime = UGITUtility.StringToDateTime(scheduleActionItem.StartTime);
            objScheduleActionsArchive.ActionType = scheduleActionItem.ActionType;
            objScheduleActionsArchive.EmailIDTo = scheduleActionItem.EmailIDTo;
            objScheduleActionsArchive.EmailIDCC = scheduleActionItem.EmailIDCC;
            objScheduleActionsArchive.MailSubject = scheduleActionItem.MailSubject;
            objScheduleActionsArchive.TicketId = scheduleActionItem.TicketId;
            objScheduleActionsArchive.Recurring = scheduleActionItem.Recurring;
            objScheduleActionsArchive.RecurringInterval = Convert.ToInt32(scheduleActionItem.RecurringInterval);
            if (scheduleActionItem.RecurringEndDate != null)
            {
                objScheduleActionsArchive.RecurringEndDate = Convert.ToDateTime(scheduleActionItem.RecurringEndDate);
            }
            else
            {
                objScheduleActionsArchive.RecurringEndDate = null;
            }
            objScheduleActionsArchive.CustomProperties = scheduleActionItem.CustomProperties;
            objScheduleActionsArchive.EmailBody = scheduleActionItem.EmailBody;
            objScheduleActionsArchive.ModuleNameLookup = scheduleActionItem.ModuleNameLookup;
            objScheduleActionsArchive.AlertCondition = scheduleActionItem.AlertCondition;
            objScheduleActionsArchive.ActionTypeData = scheduleActionItem.ActionTypeData;
            objScheduleActionsArchive.AttachmentFormat = scheduleActionItem.AttachmentFormat;
            objScheduleActionsArchive.Log = logMessage;
            objScheduleActionsArchive.AgentJobStatus = Convert.ToString(jobStatus);

            // Log errors in SharePoint logs as well!
            if (jobStatus == AgentJobStatus.Fail)
            {
                // uGovernIT.Helpers.Log.WriteLog(logMessage, TraceSeverity.Unexpected, EventSeverity.None);
            }
            objScheduleActionsArchiveManager.Insert(objScheduleActionsArchive);
            //UpdateSPListItem(scheduleActionArchiveItem);

        }

        private void UpdateSPListItem(DataRow spListItem)
        {
            // bool unsafeUpdate = _spWeb.AllowUnsafeUpdates;
            //_spWeb.AllowUnsafeUpdates = true;
            // spListItem.UpdateOverwriteVersion(); // Use UpdateOverwriteVersion() to avoid creating unnecessary versions in tickets
            // _spWeb.AllowUnsafeUpdates = unsafeUpdate;

        }

        public bool DeleteFromScheduleAction(SchedulerAction scheduleActionItem)
        {
            bool value = false;
            if (scheduleActionItem != null)
            {
                value = scheduleActionsManager.Delete(scheduleActionItem);

            }
            return value;
        }
        public long CreateSchedule(Dictionary<string, object> dicObject)
        {
            ScheduleActionsManager objScheduleActionsManager = new ScheduleActionsManager(_context);
            if (dicObject == null) { return 0; }
            SchedulerAction scheduleAction = null;

            if (dicObject.ContainsKey(DatabaseObjects.Columns.TicketId) && dicObject.ContainsKey(DatabaseObjects.Columns.ActionType))
            {
                ScheduleActionType actionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), Convert.ToString(dicObject[DatabaseObjects.Columns.ActionType]));

                string ticketId = Convert.ToString(dicObject[DatabaseObjects.Columns.TicketId]);
                string customproperty = dicObject.ContainsKey(DatabaseObjects.Columns.CustomProperties) ? Convert.ToString(dicObject[DatabaseObjects.Columns.CustomProperties]) : "";
                string moduleId = dicObject.ContainsKey(DatabaseObjects.Columns.ModuleNameLookup) ? Convert.ToString(dicObject[DatabaseObjects.Columns.ModuleNameLookup]) : "";
                string listName = dicObject.ContainsKey(DatabaseObjects.Columns.ListName) ? Convert.ToString(dicObject[DatabaseObjects.Columns.ListName]) : "";

                bool itemExists = false;
                // DataRow[] itemcoll = GetScheduleActionItemByTicketId(ticketId, actionType);
                List<SchedulerAction> itemcoll = objScheduleActionsManager.Load(x => x.TicketId == ticketId && x.ActionType == actionType.ToString());
                if (itemcoll != null && itemcoll.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(customproperty) && actionType == ScheduleActionType.EscalationEmail)
                    {
                        foreach (SchedulerAction spItem in itemcoll)
                        {
                            Dictionary<string, string> cproperty = UGITUtility.GetCustomProperties(spItem.CustomProperties);
                            Dictionary<string, string> dic_cproperty = UGITUtility.GetCustomProperties(customproperty);
                            string dbExtendedKey = string.Empty;
                            if (cproperty.ContainsKey(CustomProperties.ExtendedKey))
                                dbExtendedKey = cproperty[CustomProperties.ExtendedKey];

                            string extendedKey = string.Empty;
                            if (dic_cproperty.ContainsKey(CustomProperties.ExtendedKey))
                                extendedKey = dic_cproperty[CustomProperties.ExtendedKey];

                            if (cproperty.ContainsKey(CustomProperties.EscalationRuleId) && dic_cproperty.ContainsKey(CustomProperties.EscalationRuleId))
                            {
                                if (string.Compare(cproperty[CustomProperties.EscalationRuleId], dic_cproperty[CustomProperties.EscalationRuleId], true) == 0 && dbExtendedKey == extendedKey)
                                {
                                    scheduleAction = spItem;
                                    itemExists = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (SchedulerAction spItem in itemcoll)
                        {
                            // Need to do this check since for VND Deliverables & tasks, we store DB ID in TicketId column which is not unique across modules
                            // Need to check both module AND list name, since we don't always store both
                            //SPFieldLookupValue moduleLookup = new SPFieldLookupValue(Convert.ToString(spItem[DatabaseObjects.Columns.ModuleNameLookup]));
                            string moduleLookup = Convert.ToString(spItem.ModuleNameLookup);
                            string itemModuleId = string.Empty;
                            string moduleName = string.Empty;
                            if (moduleLookup != null)
                            {
                                // itemModuleId = moduleLookup.LookupId.ToString();
                                moduleName = moduleLookup;
                            }

                            Dictionary<string, string> cproperty = UGITUtility.GetCustomProperties(spItem.CustomProperties);
                            Dictionary<string, string> dic_cproperty = UGITUtility.GetCustomProperties(customproperty);
                            string dbExtendedKey = string.Empty;
                            if (cproperty.ContainsKey(CustomProperties.ExtendedKey))
                                dbExtendedKey = cproperty[CustomProperties.ExtendedKey];

                            string extendedKey = string.Empty;
                            if (dic_cproperty.ContainsKey(CustomProperties.ExtendedKey))
                                extendedKey = dic_cproperty[CustomProperties.ExtendedKey];

                            // Match if ticket ID starts with module name (TSR-... for regular tickets) OR listname matches (for tasks & deliverables
                            if (moduleName == moduleId && dbExtendedKey == extendedKey && (uHelper.getModuleNameByTicketId(ticketId) == moduleName || spItem.ListName == listName))
                            {
                                scheduleAction = spItem;
                                itemExists = true;
                                break;
                            }
                        }
                    }
                }

                if (!itemExists)
                {
                    // SPList ScheduleActionList = _spWeb.Lists[DatabaseObjects.Lists.ScheduleActions];
                    scheduleAction = new SchedulerAction();
                }
            }
            else
            {
                // SPList ScheduleActionList = _spWeb.Lists[DatabaseObjects.Lists.ScheduleActions];
                scheduleAction = new SchedulerAction();
            }

            scheduleAction.Title = dicObject.ContainsKey(DatabaseObjects.Columns.Title) ? Convert.ToString(dicObject[DatabaseObjects.Columns.Title]) : "";

            if (dicObject.ContainsKey(DatabaseObjects.Columns.StartTime) && !string.IsNullOrWhiteSpace(Convert.ToString(dicObject[DatabaseObjects.Columns.StartTime])))
                scheduleAction.StartTime = Convert.ToDateTime(dicObject[DatabaseObjects.Columns.StartTime]);
            else
                scheduleAction.StartTime = null;

            scheduleAction.ActionType = dicObject.ContainsKey(DatabaseObjects.Columns.ActionType) ? Convert.ToString(dicObject[DatabaseObjects.Columns.ActionType]) : null;
            scheduleAction.EmailIDTo = dicObject.ContainsKey(DatabaseObjects.Columns.EmailIDTo) ? Convert.ToString(dicObject[DatabaseObjects.Columns.EmailIDTo]) : "";
            scheduleAction.EmailIDCC = dicObject.ContainsKey(DatabaseObjects.Columns.EmailIDCC) ? Convert.ToString(dicObject[DatabaseObjects.Columns.EmailIDCC]) : "";
            scheduleAction.MailSubject = dicObject.ContainsKey(DatabaseObjects.Columns.MailSubject) ? Convert.ToString(dicObject[DatabaseObjects.Columns.MailSubject]) : "";
            scheduleAction.TicketId = dicObject.ContainsKey(DatabaseObjects.Columns.TicketId) ? Convert.ToString(dicObject[DatabaseObjects.Columns.TicketId]) : "";
            bool isRecurring = dicObject.ContainsKey(DatabaseObjects.Columns.Recurring) ? UGITUtility.StringToBoolean(dicObject[DatabaseObjects.Columns.Recurring]) : false;
            if (isRecurring)
            {
                scheduleAction.Recurring = true;
                scheduleAction.RecurringInterval = dicObject.ContainsKey(DatabaseObjects.Columns.RecurringInterval) ? Convert.ToInt32(dicObject[DatabaseObjects.Columns.RecurringInterval]) : 0;

                if (dicObject.ContainsKey(DatabaseObjects.Columns.RecurringEndDate) && !string.IsNullOrWhiteSpace(Convert.ToString(dicObject[DatabaseObjects.Columns.RecurringEndDate])))
                    scheduleAction.RecurringEndDate = Convert.ToDateTime(dicObject[DatabaseObjects.Columns.RecurringEndDate]);
                else
                    scheduleAction.RecurringEndDate = null;
            }
            else
            {
                scheduleAction.Recurring = false;
                scheduleAction.RecurringInterval = 0;
                scheduleAction.RecurringEndDate = null;
            }
            // scheduleAction.RecurringInterval = dicObject.ContainsKey(DatabaseObjects.Columns.RecurringInterval) ? Convert.ToInt32(dicObject[DatabaseObjects.Columns.RecurringInterval]) : 0;

            if (dicObject.ContainsKey(DatabaseObjects.Columns.RecurringEndDate) && !string.IsNullOrWhiteSpace(Convert.ToString(dicObject[DatabaseObjects.Columns.RecurringEndDate])))
                scheduleAction.RecurringEndDate = Convert.ToDateTime(dicObject[DatabaseObjects.Columns.RecurringEndDate]);
            else
                scheduleAction.RecurringEndDate = null;

            scheduleAction.CustomProperties = dicObject.ContainsKey(DatabaseObjects.Columns.CustomProperties) ? Convert.ToString(dicObject[DatabaseObjects.Columns.CustomProperties]) : "";

            scheduleAction.ActionTypeData = dicObject.ContainsKey(DatabaseObjects.Columns.ActionTypeData) ? Convert.ToString(dicObject[DatabaseObjects.Columns.ActionTypeData]) : "";

            scheduleAction.EmailBody = dicObject.ContainsKey(DatabaseObjects.Columns.EmailBody) ? Convert.ToString(dicObject[DatabaseObjects.Columns.EmailBody]) : "";

            if (dicObject.ContainsKey(DatabaseObjects.Columns.ModuleNameLookup))
                scheduleAction.ModuleNameLookup = Convert.ToString(dicObject[DatabaseObjects.Columns.ModuleNameLookup]);
            else
                scheduleAction.ModuleNameLookup = null;

            scheduleAction.ListName = dicObject.ContainsKey(DatabaseObjects.Columns.ListName) ? Convert.ToString(dicObject[DatabaseObjects.Columns.ListName]) : null;
            scheduleAction.AttachmentFormat = dicObject.ContainsKey(DatabaseObjects.Columns.AttachmentFormat) ? Convert.ToString(dicObject[DatabaseObjects.Columns.AttachmentFormat]) : null;
            //UpdateSPListItem(scheduleAction);
            if (scheduleAction.ID > 0)
                objScheduleActionsManager.Update(scheduleAction);

            objScheduleActionsManager.Insert(scheduleAction);
            return scheduleAction.ID;
        }
        public List<SchedulerAction> GetScheduleActionItemByTicketId(string TicketId, ScheduleActionType actionType)
        {
            ScheduleActionsManager objScheduleActionsManager = new ScheduleActionsManager(_context);
            List<SchedulerAction> itemcoll = objScheduleActionsManager.Load(x => x.TicketId == TicketId && x.ActionType == actionType.ToString());
            return itemcoll;
        }
        private string getEmailIdFromUserInfo(string userInfo, bool canDelete)
        {
            string email = string.Empty;
            if (!string.IsNullOrEmpty(userInfo))
            {
                UserProfileManager userProfileManager = new UserProfileManager(_context);
                UserProfile spUserValuecc = userProfileManager.LoadById(userInfo);
                if (spUserValuecc == null)
                    IsItemDelete = canDelete;
                else
                    email = uHelper.GetUserEmail(spUserValuecc, _context);
            }

            return email;
        }

        private void UpdateIsRecurrence(SchedulerAction scheduleAction, object recurringEndDate)
        {
            ScheduleActionsManager objScheduleActionsManager = new ScheduleActionsManager(_context);
            SchedulerAction objScheduleAction = new SchedulerAction();
            DateTime recurringEnd = DateTime.MaxValue;
            if (recurringEndDate != null && Convert.ToDateTime(recurringEndDate) != DateTime.MinValue)
            {
                recurringEnd = Convert.ToDateTime(recurringEndDate);
                if (recurringEnd < DateTime.Now)
                {
                    IsItemDelete = true;
                    return;
                }
            }

            DateTime myStartTime = UGITUtility.StringToDateTime(scheduleAction.StartTime);

            // In case start time was way in the past and we are playing catch up, 
            // keep incrementing by the recurring interval till we catch up
            double recurringIntervalMinutes = 0;
            double.TryParse(Convert.ToString(scheduleAction.RecurringInterval), out recurringIntervalMinutes);

            string moduleName = scheduleAction.ModuleNameLookup;
            string actionType = scheduleAction.ActionType;

            while (myStartTime < DateTime.Now)
            {
                if (actionType == "Reminder" && moduleName == "CMT")
                {
                    myStartTime = myStartTime.AddMinutes(recurringIntervalMinutes); // Recurring interval is in calendar time!
                }
                else if (actionType == "Query")
                {
                    string serialized = Convert.ToString(scheduleAction.CustomProperties);
                    Dictionary<string, object> tempdic = UGITUtility.DeserializeDicObject(serialized);

                    if (tempdic.ContainsKey(ReportScheduleConstant.CustomRecurrence)) // Custom recurrance case
                    {
                        List<string> pValues = Convert.ToString(tempdic[ReportScheduleConstant.CustomRecurrence]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        int startDay = (int)myStartTime.DayOfWeek;
                        int nextDay = 7;
                        double nxtDay = 0;

                        if (pValues.Exists(x => Convert.ToInt16(x) > startDay))
                        {
                            nextDay = Convert.ToInt16(pValues.FirstOrDefault(x => Convert.ToInt16(x) > startDay));
                            nxtDay = myStartTime.AddDays(nextDay - startDay).Date.Subtract(myStartTime.Date).TotalDays;
                        }
                        else
                        {
                            if (pValues.Exists(x => Convert.ToInt16(x) >= 0))
                            {
                                nextDay = Convert.ToInt16(pValues.FirstOrDefault(x => Convert.ToInt16(x) >= 0));
                                DateTime nextWeekStartDay = uHelper.FirstDayOfWeek(_context, myStartTime.AddDays(7));
                                nxtDay = nextWeekStartDay.AddDays(nextDay).Date.Subtract(myStartTime.Date).TotalDays;
                            }
                        }

                        if (myStartTime.AddDays(nxtDay) <= recurringEnd)
                        {
                            recurringIntervalMinutes = 0;
                            myStartTime = myStartTime.AddDays(nxtDay);
                        }
                    }
                    else // Normal recurrence
                    {
                        if (recurringIntervalMinutes <= 0)
                        {
                            ULog.WriteLog("ERROR: Invalid Recurring interval!");
                            IsItemDelete = true;
                            return;
                        }

                        else if (tempdic.ContainsKey(ReportScheduleConstant.BusinessHours) && UGITUtility.StringToBoolean(tempdic[ReportScheduleConstant.BusinessHours]) == false)
                        {
                            // add exact recurring interval ignore business hours 
                            int workingHoursInADay = uHelper.GetWorkingHoursInADay(_context);
                            if (recurringIntervalMinutes % (workingHoursInADay * 60) == 0)
                            {
                                double daysToAdd = Convert.ToDouble(recurringIntervalMinutes / (workingHoursInADay * 60));
                                myStartTime = myStartTime.AddDays(daysToAdd);
                            }
                            else if (recurringIntervalMinutes % 60 == 0)
                            {
                                double hrsToAdd = Convert.ToDouble(recurringIntervalMinutes / (60));
                                myStartTime = myStartTime.AddHours(hrsToAdd);
                            }
                            else
                                myStartTime = myStartTime.AddMinutes(recurringIntervalMinutes);
                        }
                        else
                        {
                            // add recurring interval in business hours only when Business Hours is true in Custom Property or is not set
                            //It will run in Business Hours by Default
                            myStartTime = uHelper.GetWorkingEndDate(_context, myStartTime, recurringIntervalMinutes);
                        }
                    }
                }
                else // for escalations, etc
                {
                    // add recurring interval in business hours only
                    myStartTime = uHelper.GetWorkingEndDate(_context, myStartTime, recurringIntervalMinutes);
                }
            }

            if (recurringEnd < myStartTime)
            {
                IsItemDelete = true;
                return;
            }

            scheduleAction.StartTime = myStartTime;
            scheduleAction.RecurringInterval = Convert.ToInt32(recurringIntervalMinutes);
            ScheduleActionType ActionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), scheduleAction.ActionType);

            if (ActionType == ScheduleActionType.Reminder)
            {
                Dictionary<string, string> customproper = UGITUtility.GetCustomProperties(scheduleAction.CustomProperties);
                if (!customproper.ContainsKey(CustomProperties.ScheduledRepeatInterval))
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Fail, "ScheduledRepeatInterval not available");
                }
                else
                {
                    string repeatInterval = customproper[CustomProperties.ScheduledRepeatInterval];
                    scheduleAction.RecurringInterval = uHelper.GetRecurringIntervalInMinutes(_context, repeatInterval, UGITUtility.StringToDateTime(scheduleAction.StartTime));
                }
            }
            else if (ActionType != ScheduleActionType.Query && ActionType != ScheduleActionType.Report && ActionType != ScheduleActionType.Reminder)
            {
                scheduleAction.Title = string.Format("{0} {1}", scheduleAction.TicketId, UGITUtility.StringToDateTime(scheduleAction.StartTime).ToString());
            }

            if (scheduleAction.ID > 0)
                objScheduleActionsManager.Update(scheduleAction);
            else
                objScheduleActionsManager.Insert(scheduleAction);

            IsItemDelete = false;
        }

        private string SendMail(SchedulerAction scheduleAction)
        {
            string[] attachments = { };
            return SendMail(scheduleAction, attachments);
        }
        private string SendMail(SchedulerAction scheduleAction, string[] attachments)
        {
            string errMessage = string.Empty;
            try
            {
                // Get subject.
                string subject = scheduleAction.MailSubject;

                // Get Email To id
                string to = scheduleAction.EmailIDTo;

                // Get Email CC id
                string cc = scheduleAction.EmailIDCC;

                // Get Email Body content from Schedule action.
                StringBuilder body = new StringBuilder();
                body.Append(Convert.ToString(scheduleAction.EmailBody));
                body.Append("<br />");
                body.Append("<br />");
                body.Append(configVariableManager.GetValue(ConfigConstants.Signature));

                if (scheduleAction.ActionType == "Reminder")
                {
                    if (scheduleAction.TicketId != string.Empty)
                    {
                        string ticketId = scheduleAction.TicketId;
                        string moduleName = uHelper.getModuleNameByTicketId(ticketId);
                        DataRow currentticket = Ticket.GetCurrentTicket(_context, moduleName, ticketId);
                        string ticketDetailFooter = uHelper.GetTicketDetailsForEmailFooter(_context, currentticket, moduleName, true, false);
                        body.Append(ticketDetailFooter);
                    }
                }

                MailMessenger mail = new MailMessenger(_context);
                errMessage = mail.SendMail(to, subject, cc, body.ToString(), true, attachments);
            }
            catch (Exception ex)
            {
                errMessage = ex.ToString();
            }

            return errMessage;
        }

        public void DeleteTaskReminder(string ticketId)
        {
            SchedulerAction scheduleactions = scheduleActionsManager.Get(x => x.TicketId == ticketId);
            if (scheduleactions != null)
                scheduleActionsManager.Delete(scheduleactions);
        }
        public void DeleteTaskReminder(string ticketId, ScheduleActionType actionType)
        {
            SchedulerAction scheduleactions = scheduleActionsManager.Get(x => x.TicketId == ticketId && x.ActionType == actionType.ToString());
            if (scheduleactions != null)
                scheduleActionsManager.Delete(scheduleactions);

        }
        #endregion

        #region "Execute Report (Ticket Summary Report, Task Summary, Project Report)"
        /// <summary>
        /// Sends the report.
        /// </summary>
        /// <param name="scheduleAction">The schedule action.</param>
        public void SendReport(SchedulerAction scheduleAction)
        {
            string logMessage = string.Empty;
            string fileName = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_context.SiteUrl);
                    // HTTP GET
                    var postTask = client.GetAsync("api/reportapi/GetReportPath?scheduledActionID=" + scheduleAction.ID+"&TenantId="+_context.TenantID);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var path = result.Content.ReadAsStringAsync();
                        path.Wait();
                        fileName = path.Result;
                    }
                }

                if (!string.IsNullOrEmpty(fileName))
                {
                    logMessage = SendMail(scheduleAction, new string[] { fileName });
                }
                else {
                    logMessage = "No report found.";
                }
                
            }
            catch (Exception ex)
            {
                logMessage = ex.ToString();
            }

            if (string.IsNullOrEmpty(logMessage))
            {
                CreateLogs(scheduleAction, AgentJobStatus.Success, logMessage);

                if (scheduleAction.Recurring)
                    UpdateIsRecurrence(scheduleAction, null);
                else
                    IsItemDelete = true;
            }
            else
            {
                CreateLogs(scheduleAction, AgentJobStatus.Fail, logMessage);
            }
        }

        /// <summary>
        /// Project Report Export into Pdf, Excel files and return filePath
        /// </summary>
        /// <param name="formobj"></param>
        /// <returns></returns>
        //private string GetProjectReport(Dictionary<string, object> formobj, string attachFormat)
        //{
        //    string filePath = Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
        //    string fileName = string.Empty;
        //    ProjectReportEntity proEntity = new ProjectReportEntity();

        //    proEntity.ShowAccomplishment = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowAccomplishment]);
        //    proEntity.ShowPlan = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlan]);
        //    proEntity.ShowIssues = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowIssues]);
        //    proEntity.ShowStatus = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowStatus]);
        //    proEntity.ShowSummaryGanttChart = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowSummaryGanttChart]);
        //    proEntity.ShowAllTask = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowAllTask]);
        //    proEntity.ShowMilestone = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowMilestone]);
        //    proEntity.ShowDeliverable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowDeliverable]);
        //    proEntity.ShowReceivable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowReceivable]);
        //    proEntity.CalculateExpected = Convert.ToBoolean(formobj[ReportScheduleConstant.CalculateExpected]);
        //    proEntity.ShowProjectDescription = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectDescription]);
        //    proEntity.ShowBudgetSummary = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowBudgetSummary]);
        //    proEntity.ShowPlannedvsActualByCategory = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlannedvsActualByCategory]);
        //    proEntity.ShowPlannedvsActualByBudgetItem = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlannedvsActualByBudgetItem]);
        //    proEntity.ShowPlannedvsActualByMonth = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlannedvsActualByMonth]);
        //    proEntity.ShowProjectRoles = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectRoles]);
        //    proEntity.ShowResourceAllocation = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowResourceAllocation]);
        //    proEntity.ShowMonitorState = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowMonitorState]);

        //    if (string.IsNullOrEmpty(attachFormat))
        //        attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

        //    string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);

        //    int[] PMMids = Array.ConvertAll<string, int>(Convert.ToString(formobj[ReportScheduleConstant.ids]).Split(','), int.Parse);
        //    ProjectReportHelper prHelper = new ProjectReportHelper(_spWeb);
        //    proEntity = prHelper.GetProjectReportEntity(proEntity, PMMids);

        //    MultiProReport multiproject = new MultiProReport(proEntity);

        //    return ExportFiles(multiproject, attachFormat, filePath, title);
        //}

        /// <summary>
        /// Gets the TSK summary report.
        /// </summary>
        /// <param name="formobj">The formobj.</param>
        /// <returns>System.String.</returns>
        //private string GetTSKSummaryReport(Dictionary<string, object> formobj, string attachFormat)
        //{
        //    string filePath = Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
        //    string fileName = string.Empty;

        //    int[] Ids = Array.ConvertAll<string, int>(Convert.ToString(formobj[ReportScheduleConstant.Projects]).Split(','), int.Parse);
        //    TicketStatus tstatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Convert.ToString(formobj[ReportScheduleConstant.TicketStatus]));
        //    string moduleName = Convert.ToString(formobj[ReportScheduleConstant.Module]);

        //    if (string.IsNullOrEmpty(attachFormat))
        //        attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

        //    string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
        //    DataTable dataSource = uHelper.GetProjectsTasksTable(Ids, tstatus, moduleName, _spWeb);

        //    TaskSummaryReport report = new TaskSummaryReport(dataSource);

        //    return ExportFiles(report, attachFormat, filePath, title);
        //}

        /// <summary>
        /// Generates the TSK project status report.
        /// </summary>
        /// <param name="formobj">The formobj.</param>
        /// <returns>System.String.</returns>
        //private string GenerateTSKProjectStatusReport(Dictionary<string, object> formobj, string attachFormat)
        //{
        //    string filePath = Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
        //    string fileName = string.Empty;
        //    ProjectReportEntity proEntity = new ProjectReportEntity();
        //    proEntity.ShowStatus = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowStatus]);
        //    proEntity.ShowProjectRoles = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectRoles]);
        //    proEntity.ShowProjectDescription = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectDescription]);
        //    proEntity.ShowReceivable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowReceivable]);
        //    proEntity.ShowDeliverable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowDeliverable]);
        //    proEntity.ShowMilestone = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowMilestone]);
        //    proEntity.ShowAllTask = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowAllTask]);
        //    proEntity.ShowSummaryGanttChart = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowSummaryGanttChart]);

        //    if (string.IsNullOrEmpty(attachFormat))
        //        attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);
        //    string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);

        //    int[] PMMids = Array.ConvertAll<string, int>(Convert.ToString(formobj[ReportScheduleConstant.ids]).Split(','), int.Parse);
        //    ProjectReportHelper prHelper = new ProjectReportHelper(_spWeb);
        //    proEntity = prHelper.GetTSKProjectsEntity(proEntity, PMMids);

        //    MultiProReport multiproject = new MultiProReport(proEntity);

        //    return ExportFiles(multiproject, attachFormat, filePath, title);
        //}

        /// <summary>
        /// Gets the ticket summary report.
        /// </summary>
        /// <param name="formobj">The formobj.</param>
        /// <returns>System.String.</returns>
        //private string GetTicketSummaryReport(Dictionary<string, object> formobj, string attachFormat)
        //{
        //    string filePath = uHelper.GetTempFolderPath() + "/" + Guid.NewGuid(); //Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
        //    string module = Convert.ToString(formobj[ReportScheduleConstant.Module]);
        //    string sortbyModule = Convert.ToString(formobj[ReportScheduleConstant.SortByModule]);
        //    TicketStatus tstatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Convert.ToString(formobj[ReportScheduleConstant.TicketStatus]));
        //    string sortby = Convert.ToString(formobj[ReportScheduleConstant.SortBy]);
        //    string fromdate = string.Empty, todate = string.Empty;
        //    if (!string.IsNullOrEmpty(Convert.ToString(formobj[ReportScheduleConstant.FromDate])))
        //    {
        //        fromdate = Convert.ToString(formobj[ReportScheduleConstant.FromDate]);
        //    }
        //    if (!string.IsNullOrEmpty(Convert.ToString(formobj[ReportScheduleConstant.ToDate])))
        //    {
        //        todate = Convert.ToString(formobj[ReportScheduleConstant.ToDate]);
        //    }
        //    UserProfile currentUser = _context.CurrentUser;//_spWeb.Users.GetByID(Convert.ToInt32(formobj[ReportScheduleConstant.ToDate]));
        //    TicketSummaryReportHelper tsrHelper = new TicketSummaryReportHelper(module, sortby, sortbyModule, tstatus, fromdate, todate, _context, currentUser);

        //    XtraReport report = tsrHelper.GetTicketSummaryReport();
        //    if (string.IsNullOrEmpty(attachFormat))
        //        attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

        //    string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);

        //    return ExportFiles(report, attachFormat, filePath, title);
        //}
        //private string GetNonPeakHourReport(Dictionary<string, object> formobj, string attachFormat)
        //{
        //    string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
        //    string filePath = Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
        //    DateTime DateFrom = new DateTime();
        //    if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
        //    {
        //        string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
        //        Dictionary<string, DateTime> dic = getReportScheduleDates(dateRange);
        //        if (dic.Count > 0)
        //            DateFrom = dic["DateFrom"];
        //    }
        //    string moduleName = formobj.ContainsKey(ReportScheduleConstant.Module) ? Convert.ToString(formobj[ReportScheduleConstant.Module]) : string.Empty;

        //    int nonPeakHourWindow = formobj.ContainsKey(ReportScheduleConstant.NonPeakHourWindow) ? uHelper.StringToInt(formobj[ReportScheduleConstant.NonPeakHourWindow]) : 0;
        //    if (nonPeakHourWindow == 0)
        //        nonPeakHourWindow = uHelper.StringToInt(uGITCache.GetConfigVariableValue("NonPeakHourWindow", _spWeb));

        //    string workingWindowStartTime = formobj.ContainsKey(ReportScheduleConstant.WorkingWindowStartTime) ? Convert.ToString(formobj[ReportScheduleConstant.WorkingWindowStartTime]) : string.Empty;
        //    string workingWindowEndTime = formobj.ContainsKey(ReportScheduleConstant.WorkingWindowEndTime) ? Convert.ToString(formobj[ReportScheduleConstant.WorkingWindowEndTime]) : string.Empty;


        //    if (string.IsNullOrEmpty(workingWindowStartTime))
        //        workingWindowStartTime = Convert.ToString(uGITCache.GetConfigVariableValue("WorkdayStartTime", _spWeb));


        //    if (string.IsNullOrEmpty(workingWindowEndTime))
        //        workingWindowEndTime = Convert.ToString(uGITCache.GetConfigVariableValue("WorkdayEndTime", _spWeb));


        //    DataTable data = ModuleStatistics.GetNonPeakHoursCount(_spWeb, moduleName, DateFrom, nonPeakHourWindow, workingWindowStartTime, workingWindowEndTime);
        //    HelpDeskPrfReportEntity entity = new HelpDeskPrfReportEntity();
        //    entity.Data = data;

        //    if (DateFrom != DateTime.MinValue)
        //        entity.StartDate = uHelper.GetDateStringInFormat(DateFrom, false);

        //    HelpDeskPrfReport report = new HelpDeskPrfReport(entity);
        //    XtraReport reportNonPeakHours = report;
        //    return ExportFiles(reportNonPeakHours, attachFormat, filePath, title);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formobj"></param>
        /// <param name="attachFormat"></param>
        /// <returns></returns>
        //private string GetWeeklyTeamReport(Dictionary<string, object> formobj, string attachFormat)
        //{
        //    string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
        //    string filePath = Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
        //    string category = formobj.ContainsKey(ReportScheduleConstant.Category) ? Convert.ToString(formobj[ReportScheduleConstant.Category]) : string.Empty;

        //    DateTime DateFrom = new DateTime();
        //    DateTime DateTo = new DateTime();
        //    if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
        //    {
        //        string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
        //        Dictionary<string, DateTime> dic = getReportScheduleDates(dateRange);
        //        if (dic.Count > 0)
        //        {
        //            DateFrom = dic["DateFrom"];
        //            DateTo = dic["DateTo"];
        //        }
        //    }

        //    if (string.IsNullOrEmpty(attachFormat))
        //        attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

        //    string moduleName = formobj.ContainsKey(ReportScheduleConstant.Module) ? Convert.ToString(formobj[ReportScheduleConstant.Module]) : string.Empty;

        //    DataTable data = new DataTable();

        //    data = ModuleStatistics.GetWeeklyTeamPrfCount(_spWeb, moduleName, DateFrom, DateTo, category);
        //    WeeklyTeamPrfReportEntity entity = new WeeklyTeamPrfReportEntity();
        //    entity.Data = data;

        //    if (DateFrom != DateTime.MinValue)
        //        entity.StartDate = uHelper.GetDateStringInFormat(DateFrom, false);
        //    if (DateTo != DateTime.MinValue)
        //        entity.EndDate = uHelper.GetDateStringInFormat(DateTo, false);
        //    WeeklyTeamPerformanceReport reportWeekly = new WeeklyTeamPerformanceReport(entity);
        //    XtraReport report = reportWeekly;

        //    return ExportFiles(report, attachFormat, filePath, title);
        //}

        /// <summary>
        /// Gets the ticket summary report.
        /// </summary>
        /// <param name="formobj">The formobj.</param>
        /// <returns>System.String.</returns>
        //private string GetSummaryByTechnicianReport(Dictionary<string, object> formobj, string attachFormat)
        //{
        //    string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
        //    string filePath = Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
        //    if (string.IsNullOrEmpty(attachFormat))
        //        attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

        //    string moduleName = formobj.ContainsKey(ReportScheduleConstant.Module) ? Convert.ToString(formobj[ReportScheduleConstant.Module]) : string.Empty;
        //    if (moduleName.ToLower() == "all")
        //    {
        //        DataTable dtModules = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules, _spWeb);
        //        if (dtModules != null && dtModules.Rows.Count != 0)
        //        {
        //            var modules = dtModules.AsEnumerable()
        //                              .Where(x => !x.IsNull(DatabaseObjects.Columns.ShowTicketSummary)
        //                                        && x.Field<string>(DatabaseObjects.Columns.ShowTicketSummary).Equals("1")
        //                                        && !x.IsNull(DatabaseObjects.Columns.EnableModule)
        //                                        && x.Field<string>(DatabaseObjects.Columns.EnableModule).Equals("1"));

        //            moduleName = string.Join(",", modules.Select(x => x.Field<string>(DatabaseObjects.Columns.ModuleName)).ToList());
        //        }
        //    }

        //    bool groupByCategory = formobj.ContainsKey(ReportScheduleConstant.GroupByCategory) ? uHelper.StringToBoolean(formobj[ReportScheduleConstant.GroupByCategory]) : false;
        //    bool includeORP = formobj.ContainsKey(ReportScheduleConstant.IncludeORP) ? uHelper.StringToBoolean(formobj[ReportScheduleConstant.IncludeORP]) : false;
        //    bool sortByModule = formobj.ContainsKey(ReportScheduleConstant.SortByModule) ? uHelper.StringToBoolean(formobj[ReportScheduleConstant.SortByModule]) : false;
        //    string includeCounts = formobj.ContainsKey(ReportScheduleConstant.IncludeCounts) ? Convert.ToString(formobj[ReportScheduleConstant.IncludeCounts]) : string.Empty;
        //    if (includeCounts.ToLower() == "all")
        //    {
        //        includeCounts = string.Empty;
        //        string[] moduleNames = moduleName.Split(',');
        //        foreach (string strmodule in moduleNames)
        //        {
        //            UGITModule module = uGITCache.ModuleConfigCache.GetCachedModule(_spWeb, strmodule);
        //            if (module != null)
        //            {
        //                LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault();
        //                if (lifeCycle != null)
        //                {
        //                    List<LifeCycleStage> stages = lifeCycle.Stages.Where(x => x.StageType == StageType.Assigned.ToString() || x.StageType == StageType.Closed.ToString()).ToList();
        //                    foreach (string item in stages.Select(x => x.Name))
        //                    {
        //                        if (!includeCounts.Contains(item))
        //                        {
        //                            if (!string.IsNullOrEmpty(includeCounts))
        //                                includeCounts += "," + item;
        //                            else
        //                                includeCounts = item;
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(includeCounts))
        //            includeCounts += ",OnHold";
        //        else
        //            includeCounts += "OnHold";
        //    }

        //    string itManagers = formobj.ContainsKey(ReportScheduleConstant.ITManagers) ? Convert.ToString(formobj[ReportScheduleConstant.ITManagers]) : string.Empty;
        //    DateTime DateFrom = new DateTime();
        //    DateTime DateTo = new DateTime();
        //    if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
        //    {
        //        string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
        //        Dictionary<string, DateTime> dic = getReportScheduleDates(dateRange);
        //        if(dic.Count>0)
        //        {
        //            DateFrom = dic["DateFrom"];
        //            DateTo = dic["DateTo"];
        //        }
        //    }
        //    DataTable data = ModuleStatistics.GetTicketsCountByPRP(_spWeb, moduleName, groupByCategory, uHelper.ConvertStringToList(includeCounts, ","), DateFrom, DateTo, sortByModule, includeORP, itManagers);
        //    TicketSummaryByPRPEntity entity = new TicketSummaryByPRPEntity();
        //    entity.Data = data;
        //    entity.GroupByCategory = groupByCategory;
        //    if (DateFrom != DateTime.MinValue)
        //        entity.StartDate = uHelper.GetDateStringInFormat(DateFrom, false);
        //    if (DateTo != DateTime.MinValue)
        //        entity.EndDate = uHelper.GetDateStringInFormat(DateTo, false);
        //    TicketSummaryByPRPReport reports = new TicketSummaryByPRPReport(entity);
        //    XtraReport report = reports;
        //    return ExportFiles(report, attachFormat, filePath, title);
        //}
        //private Dictionary<string,DateTime> getReportScheduleDates(string dateRange)
        //{
        //    Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
        //    DateTime DateFrom = new DateTime();
        //    DateTime DateTo = new DateTime();
        //    string[] arrDateRange = dateRange.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
        //    if (arrDateRange.Length > 0)
        //    {
        //        string unit = arrDateRange[1];
        //        string[] arrDates = arrDateRange[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.None);
        //        if (arrDates.Length > 0)
        //        {
        //            int fromDays = 0;
        //            int toDays = 0;
        //            switch (unit)
        //            {
        //                case "Days":
        //                    {
        //                        fromDays = uHelper.StringToInt(arrDates[0]);
        //                        toDays = uHelper.StringToInt(arrDates[1]);
        //                        DateFrom = DateTime.Now.AddDays(uHelper.StringToInt(arrDates[0]));
        //                        DateTo = DateTime.Now.AddDays(uHelper.StringToInt(arrDates[1]));
        //                    }
        //                    break;
        //                case "Weeks":
        //                    {
        //                        fromDays = (uHelper.StringToInt(arrDates[0]) * 7);
        //                        toDays = (uHelper.StringToInt(arrDates[1]) * 7);
        //                        DateFrom = DateTime.Now.AddDays(fromDays);
        //                        DateTo = DateTime.Now.AddDays(toDays);
        //                    }
        //                    break;
        //                case "Months":
        //                    {
        //                        fromDays = (uHelper.StringToInt(arrDates[0]));
        //                        toDays = (uHelper.StringToInt(arrDates[1]));
        //                        DateFrom = DateTime.Now.AddMonths(fromDays);
        //                        DateTo = DateTime.Now.AddMonths(toDays);
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    dic.Add("DateFrom", DateFrom);
        //    dic.Add("DateTo", DateTo);
        //    return dic;
        //}
        #endregion

        #region Schedule Action Type Alert
        public void SendAlert(SchedulerAction scheduleAction)
        {
            try
            {
                int queryID = UGITUtility.StringToInt(scheduleAction.TicketId);
                List<string> wherevalue = new List<string>();
                DataTable dataTable = new DataTable();

                string serialized = scheduleAction.CustomProperties;
                if (string.IsNullOrEmpty(serialized))
                    serialized = scheduleAction.ActionTypeData;

                Dictionary<string, object> customProperties = null;

                if (!string.IsNullOrEmpty(serialized))
                    customProperties = UGITUtility.DeserializeDicObject(serialized);

                if (customProperties != null && customProperties.ContainsKey(ReportScheduleConstant.Where))
                {
                    string where = Convert.ToString(customProperties[ReportScheduleConstant.Where]);
                    wherevalue = UGITUtility.ConvertStringToList(where, ",");
                }

                // Load the dashboard 
                DashboardManager dManager = new DashboardManager(_context);
                Dashboard uDashboard = null;

                if (queryID > 0)
                    uDashboard = dManager.LoadPanelById(queryID, false);

                if (uDashboard == null)
                    return;

                DashboardQuery dashboardQuery = uDashboard.panel as DashboardQuery;
                QueryHelperManager queryHelper = new QueryHelperManager(_context);

                DataTable dtTotals = new DataTable();
                bool isParameterize = dashboardQuery.QueryInfo.WhereClauses.Exists(w => w.ParameterName != string.Empty);

                if (isParameterize)
                {
                    string whereFilter = string.Join(",", wherevalue.ToArray());
                    dataTable = queryHelper.GetReportData(dashboardQuery.QueryInfo, whereFilter, ref dtTotals, false);
                }
                else
                {
                    dataTable = queryHelper.GetReportData(dashboardQuery.QueryInfo, string.Empty, ref dtTotals, false);
                }

                if (dataTable == null || dataTable.Rows.Count == 0)
                    return;

                var valueInData = Convert.ToInt32(dataTable.Rows[0][0]);
                var value = 0;
                bool sendAlert = false;

                var alertConditionstr = scheduleAction.AlertCondition;
                Regex regexCondition = new Regex(RegularExpressionConstant.ConditionExpress);
                if (regexCondition.IsMatch(alertConditionstr))
                {
                    // Value from condition
                    Regex regexValue = new Regex(RegularExpressionConstant.ValueExpress);
                    if (regexValue.IsMatch(alertConditionstr))
                    {
                        value = Convert.ToInt32(regexValue.Match(alertConditionstr).Value);
                    }

                    // Operator from condition
                    Regex regexOperator = new Regex(RegularExpressionConstant.OperatorTypeExpress, RegexOptions.IgnoreCase);
                    if (regexOperator.IsMatch(alertConditionstr))
                    {
                        var operatorType = regexOperator.Match(alertConditionstr).Value;
                        switch (operatorType)
                        {
                            case "==":
                                sendAlert = (valueInData == value);
                                break;
                            case ">=":
                                sendAlert = (valueInData >= value);
                                break;
                            case "<=":
                                sendAlert = (valueInData <= value);
                                break;
                            case ">":
                                sendAlert = (valueInData > value);
                                break;
                            case "<":
                                sendAlert = (valueInData < value);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (sendAlert)
                {
                    string errMsg = SendMail(scheduleAction);
                    if (string.IsNullOrEmpty(errMsg))
                    {
                        CreateLogs(scheduleAction, AgentJobStatus.Success, errMsg);
                        IsItemDelete = true;
                    }
                    else
                    {
                        CreateLogs(scheduleAction, AgentJobStatus.Fail, errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                IsItemDelete = false;
                CreateLogs(scheduleAction, AgentJobStatus.Fail, ex.ToString());
            }
        }
        #endregion

        public void ScheduleActionArchiveCleanup()
        {
            ScheduleActionsArchiveManager objScheduleActionsArchiveManager = new ScheduleActionsArchiveManager(_context);
            string configurationdays = _context.ConfigManager.GetValue(ConfigConstants.ScheduleActionArchiveExpiration);

            int days = 90;
            if (!string.IsNullOrEmpty(configurationdays))
                days = int.Parse(configurationdays);
            DateTime currentDateTime = DateTime.Now;
            currentDateTime = currentDateTime.AddDays(-days);
            List<SchedulerActionArchive> listScheduleActionsArchive = objScheduleActionsArchiveManager.Load(x => x.StartTime <= currentDateTime);
            if (listScheduleActionsArchive != null && listScheduleActionsArchive.Count > 0)
            {
                //foreach (SchedulerActionArchive objScheduleActionsArchive in listScheduleActionsArchive)
                //{
                objScheduleActionsArchiveManager.Delete(listScheduleActionsArchive);
                // }
            }
        }

        #region Refresh User Profile Cache and Update entries for out of office dates expiers

        //public void RefreshCacheService(SPWeb spWeb)
        //{
        //    // Get list of users whose OutOfOffice date has expired
        //    SPList refreshProfile = spWeb.SiteUserInfoList;
        //    SPQuery query = new SPQuery();
        //    query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}'/><Value Type='Boolean'>1</Value></Eq><Lt><FieldRef Name='{1}'/><Value Type='DateTime' IncludeTimeValue='FALSE'><Today/></Value></Lt></And></Where>", DatabaseObjects.Columns.EnableOutofOffice, DatabaseObjects.Columns.LeaveToDate);
        //    SPListItemCollection coll = refreshProfile.GetItems(query);
        //    StringBuilder userIds = new StringBuilder();
        //    List<UserIdCollection> lstUserIdColl = new List<UserIdCollection>();

        //    // Reset OutOfOffice for these users
        //    if (coll != null && coll.Count > 0)
        //    {
        //        UserIdCollection objUserIds = null;
        //        foreach (SPListItem item in coll)
        //        {
        //            objUserIds = new UserIdCollection();
        //            int id = 0;
        //            int.TryParse(Convert.ToString(item[DatabaseObjects.Columns.Id]), out id);

        //            objUserIds.Id = id;
        //            lstUserIdColl.Add(objUserIds);

        //            item[DatabaseObjects.Columns.EnableOutofOffice] = false;
        //            item[DatabaseObjects.Columns.LeaveToDate] = null;
        //            item[DatabaseObjects.Columns.LeaveFromDate] = null;
        //            item[DatabaseObjects.Columns.DelegateUserOnLeave] = null;
        //            item.SystemUpdate();
        //        }

        //        // Force user profile cache reload on next access
        //        SPListHelper.SetSPWebProperty(Constants.RefreshUserCache, "true", spWeb);
        //    }
        //}
        #endregion

        #region Recurring Tickets
        /// <summary>
        /// Methods to automate the creation of recurring tickets
        /// </summary>
        /// <param name="scheduleAction"></param>
        /// <param name="ticketsToRefresh"></param>
        public void CreateRecurringTickets(SchedulerAction scheduleAction, List<string> ticketsToRefresh)
        {
            try
            {
                if (scheduleAction == null || string.IsNullOrEmpty(scheduleAction.ModuleNameLookup) || string.IsNullOrEmpty(scheduleAction.ActionTypeData))
                    return;

                string actionTypeData = scheduleAction.ActionTypeData;
                Dictionary<string, object> templateDic = UGITUtility.DeserializeDicObject(actionTypeData);

                if (templateDic == null || !templateDic.ContainsKey(RecurringTicketScheduleConstant.TemplateId))
                {
                    ULog.WriteLog("Error creating scheduled recurring ticket - template not configured");
                    return;
                }

                int templateId = UGITUtility.StringToInt(templateDic[RecurringTicketScheduleConstant.TemplateId]);
                if (templateId == 0)
                {
                    ULog.WriteLog("Error creating scheduled recurring ticket - template ID not set");
                    return;
                }

                // Get Ticket Template
                TicketTemplateManager templateManager = new TicketTemplateManager(_context);
                TicketTemplate template = templateManager.LoadByID(templateId);
                if (template == null)
                {
                    ULog.WriteLog("Error creating scheduled recurring ticket - template not found");
                    return;
                }

                string fieldvalues = template.FieldValues;
                if (string.IsNullOrEmpty(fieldvalues))
                {
                    ULog.WriteLog("Error creating scheduled recurring ticket - field values not set");
                    return;
                }

                string[] values = fieldvalues.Split(new string[] { Constants.Separator3 }, StringSplitOptions.RemoveEmptyEntries);
                string value = values.FirstOrDefault(x => x.StartsWith(DatabaseObjects.Columns.Title));
                if (string.IsNullOrEmpty(value))
                    return;

                string[] attributes = value.Split(new string[] { Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries);
                if (attributes.Length == 3 && string.IsNullOrEmpty(attributes[2]))
                    return;

                // Get module and ticket table schema
                UGITModule module = ObjModuleViewManager.GetByName(scheduleAction.ModuleNameLookup);
                DataTable ticketTable = ObjTicketManager.GetTableSchemaDetail(module.ModuleTable, string.Empty);
                if (ticketTable == null)
                    return;

                DataRow newticket = ticketTable.NewRow();
                Ticket ticketRequest = new Ticket(_context, scheduleAction.ModuleNameLookup, user);

                // Get field values from template
                List<TicketColumnValue> formValues = new List<TicketColumnValue>();
                GetFieldValuesFromTemplate(formValues, values, scheduleAction, module, ticketTable);

                // Set item values
                ticketRequest.SetItemValues(newticket, formValues, true, false, user.Id);

                // Create new ticket
                ticketRequest.Create(newticket, user);
                ticketRequest.QuickClose(module.ID, newticket, string.Empty);
                string error = ticketRequest.CommitChanges(newticket, donotUpdateEscalations: true, stopUpdateDependencies: true);

                if (!string.IsNullOrEmpty(error))
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Fail, error);
                    return;
                }

                ULog.WriteLog("Created scheduled recurring ticket: " + Convert.ToString(newticket[DatabaseObjects.Columns.Title]));

                // Add ticket Id to refresh list
                ticketsToRefresh.Add(Convert.ToString(newticket[DatabaseObjects.Columns.TicketId]));

                // Send notification
                string logs = SendEmailNotification(scheduleAction, newticket);
                if (string.IsNullOrEmpty(logs))
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Success, logs);

                    if (scheduleAction.Recurring)
                        UpdateIsRecurrence(scheduleAction, null);
                    else
                        IsItemDelete = true;
                }
                else
                {
                    CreateLogs(scheduleAction, AgentJobStatus.Fail, logs);
                }
            }
            catch (Exception ex)
            {
                IsItemDelete = false;
                CreateLogs(scheduleAction, AgentJobStatus.Fail, ex.ToString());
            }
        }

        public void GetFieldValuesFromTemplate(List<TicketColumnValue> formValues, string[] values, SchedulerAction scheduleItem, UGITModule module, DataTable ticketTable)
        {
            List<ModuleFormLayout> collFormLayoutItems = module.List_FormLayout.Where(x => x.TabId == 0 && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();

            foreach (ModuleFormLayout layout in collFormLayoutItems)
            {
                bool foundFieldValue = false;
                string fieldDisplayText = layout.FieldDisplayName;
                string fieldInternalName = layout.FieldName;
                TicketColumnValue cValue = new TicketColumnValue();

                if (UGITUtility.IfColumnExists(fieldInternalName, ticketTable))
                {
                    string value = values.FirstOrDefault(x => x.StartsWith(fieldInternalName));
                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] attributes = value.Split(new string[] { Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries);

                        if (attributes.Length == 3)
                        {
                            foundFieldValue = true;
                            cValue.DisplayName = fieldDisplayText;
                            cValue.InternalFieldName = fieldInternalName;
                            cValue.TabNumber = 0;
                            cValue.Value = attributes[2];
                        }

                        if (foundFieldValue)
                            formValues.Add(cValue);
                    }
                }
            }
        }

        private string SendEmailNotification(SchedulerAction scheduleAction, DataRow newTicket)
        {
            string[] attachments = { };
            string errMessage = string.Empty;
            try
            {
                // Get subject.
                string subject = scheduleAction.MailSubject;

                // Get Email To id
                string emailTo = scheduleAction.EmailIDTo;

                if (string.IsNullOrEmpty(emailTo))
                    return string.Empty;

                // Get Email CC id
                string emailCC = scheduleAction.EmailIDCC;

                // Get Email Body content from Schedule action.
                StringBuilder body = new StringBuilder();
                body.Append(Convert.ToString(scheduleAction.EmailBody));
                if (Convert.ToString(newTicket[DatabaseObjects.Columns.TicketId]) != string.Empty)
                {
                    string ticketId = Convert.ToString(newTicket[DatabaseObjects.Columns.TicketId]);
                    string moduleName = uHelper.getModuleNameByTicketId(ticketId);
                    string ticketDetailFooter = uHelper.GetTicketDetailsForEmailFooter(_context, newTicket, moduleName, true, false);
                    body.Append(ticketDetailFooter);
                }
                body.Append("<br />");
                body.Append("<br />");
                body.Append(configVariableManager.GetValue(ConfigConstants.Signature));
                MailMessenger mail = new MailMessenger(_context);
                errMessage = mail.SendMail(emailTo, subject, emailCC, body.ToString(), true, attachments);
            }
            catch (Exception ex)
            {
                errMessage = ex.ToString();
            }

            return errMessage;
        }
        #endregion

        public void UpdateContractReminder(DataRow currentTicket, UGITModule module)
        {
            // Create or update entry in message board if needed
            if(module!=null && !string.IsNullOrWhiteSpace(module.ModuleTable))
            UpdateContractMessageBoardReminder(currentTicket,module);

            // Schedule contract expiration reminder email
            if ((module.ModuleName == ModuleNames.VND || module.ModuleName == ModuleNames.VSW) && !configVariableManager.GetValueAsBool(ConfigConstants.EnableVNDContractExpireEmail))
                return;

            string ticketId = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
            string emailTo = getEmailIdFromUserInfo(Convert.ToString(currentTicket[DatabaseObjects.Columns.ReminderTo]), false);

            //if reminder to not set get from ticket owner
            if (string.IsNullOrWhiteSpace(emailTo) && UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.TicketOwner))
                emailTo = getEmailIdFromUserInfo(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketOwner]), false);

            if (string.IsNullOrEmpty(emailTo) || currentTicket[DatabaseObjects.Columns.ReminderDate] == null ||
                UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.TicketClosed]))
            {
                List<SchedulerAction> itemcoll = GetScheduleActionItemByTicketId(ticketId, ScheduleActionType.Reminder);
                
                while (itemcoll != null && itemcoll.Count > 0)
                {
                    SchedulerAction schedulerAction = itemcoll[0];
                    scheduleActionsManager.Delete(schedulerAction);
                }
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(DatabaseObjects.Columns.Title, string.Format("{0} {1}", ticketId, Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.ReminderDate])));
            dic.Add(DatabaseObjects.Columns.StartTime, currentTicket[DatabaseObjects.Columns.ReminderDate]);
            dic.Add(DatabaseObjects.Columns.TicketId, ticketId);
            dic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.Reminder);
            dic.Add(DatabaseObjects.Columns.EmailIDTo, emailTo);

            // Email Subject
            string subject = configVariableManager.GetValue(ConfigConstants.ReminderSubject);
            EscalationProcess escalationProcess = new EscalationProcess(_context);
            if (!string.IsNullOrEmpty(subject))
            {
                subject = escalationProcess.ReplaceMyTokensWithTicketValues(currentTicket, subject);
            }
            else
            {
                if (module.ModuleName == ModuleNames.CMT)
                    subject = string.Format("ACTION REQUIRED: Contract Expiration - {0} ({1})", Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]), ticketId);
            }

            dic.Add(DatabaseObjects.Columns.MailSubject, subject);

            // Email Body
            string emailBody = Convert.ToString(currentTicket[DatabaseObjects.Columns.ReminderBody]);
            if (string.IsNullOrWhiteSpace(emailBody))
                emailBody = configVariableManager.GetValue(ConfigConstants.ContractExpirationReminderBody);
            if (!string.IsNullOrWhiteSpace(emailBody))
            {
                emailBody = escalationProcess.ReplaceMyTokensWithTicketValues(currentTicket, emailBody);
            }
            else
            {
                if (module.ModuleName == ModuleNames.CMT)
                    emailBody = string.Format("ACTION REQUIRED: Contract {0} ({1}) Expires on {2}",
                                              Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]), ticketId, UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.ContractExpirationDate])));
            }

            dic.Add(DatabaseObjects.Columns.EmailBody, emailBody);
            if (module != null)
                dic.Add(DatabaseObjects.Columns.ModuleNameLookup, module.ModuleId);

            // Recurring Interval 
            string repeatInterval = Convert.ToString(currentTicket[DatabaseObjects.Columns.RepeatInterval]).Trim();
            if (!string.IsNullOrWhiteSpace(repeatInterval) && repeatInterval != "None")
            {
                dic.Add(DatabaseObjects.Columns.Recurring, true);
                int interval = uHelper.GetRecurringIntervalInMinutes(_context, repeatInterval, Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.ReminderDate]));
                dic.Add(DatabaseObjects.Columns.RecurringInterval, interval);
                if (module != null && module.ModuleName == ModuleNames.CMT)
                    dic.Add(DatabaseObjects.Columns.RecurringEndDate, currentTicket[DatabaseObjects.Columns.ContractExpirationDate]);
                else
                    dic.Add(DatabaseObjects.Columns.RecurringEndDate, currentTicket[DatabaseObjects.Columns.EffectiveEndDate]);

                dic.Add(DatabaseObjects.Columns.CustomProperties, string.Format("{0}={1}", CustomProperties.ScheduledRepeatInterval,
                                                                  Convert.ToString(currentTicket[DatabaseObjects.Columns.RepeatInterval])));
            }

            CreateSchedule(dic);
        }
        public void UpdateContractMessageBoardReminder(DataRow item,UGITModule uGITModule)
        {
            string currentList = uGITModule.ModuleTable;
            string ticketId = Convert.ToString(item[DatabaseObjects.Columns.TicketId]);
            string title = Convert.ToString(item[DatabaseObjects.Columns.Title]);
            DateTime expirationDate = currentList.Equals(DatabaseObjects.Tables.Contracts) ? UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.ContractExpirationDate]) : UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.EffectiveEndDate]);
            DateTime reminderDate = UGITUtility.StringToDateTime(item[DatabaseObjects.Columns.ReminderDate]);
            bool reminderNeeded = !UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.TicketClosed]) && reminderDate != DateTime.MinValue && reminderDate <= DateTime.Today && expirationDate >= DateTime.Today; // check if reminder needed for this contract

            string authorized = null;
            if (!string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.ReminderTo])))
                authorized =UGITUtility.ObjectToString(item[DatabaseObjects.Columns.ReminderTo]);
            else
                authorized = UGITUtility.ObjectToString(item[DatabaseObjects.Columns.TicketOwner]);

            if (authorized == null)
                reminderNeeded = false; // No one to show to!

            MessageBoardManager messageBoardManager = new MessageBoardManager(_context);
            List<MessageBoard> messageListCol = messageBoardManager.Load(x => x.TenantID == _context.TenantID && x.TicketId == ticketId);
            MessageBoard messageListItem = null;
            if (messageListCol.Count > 0)
            {
                if (!reminderNeeded)
                {
                    try
                    {
                        messageBoardManager.Delete(messageListCol[0]);
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteLog(string.Format("{0}-{1}", "Error deleting expired item from Message Board", ex.Message));
                    }
                    return;
                }

                messageListItem = messageListCol[0];
            }
            else if (reminderNeeded)
                messageListItem = new MessageBoard(); 
            else // Didn't find existing entry in message board AND don't need to create one
                return;

            messageListItem.Title = title;

            string body = string.Empty;
            if (currentList.Equals(DatabaseObjects.Tables.Contracts))
                body = string.Format("Contract {0}: {1} will expire on {2}", ticketId, title, UGITUtility.GetDateStringInFormat(expirationDate, false));
            else if (currentList.Equals(DatabaseObjects.Tables.VendorMSA))
                body = string.Format("MSA {0}: {1} will expire on {2}", ticketId, title, UGITUtility.GetDateStringInFormat(expirationDate, false));
            else if (currentList.Equals(DatabaseObjects.Tables.VendorSOW))
                body = string.Format("SOW {0}: {1} will expire on {2}", ticketId, title, UGITUtility.GetDateStringInFormat(expirationDate, false));

            messageListItem.Body = body;
            messageListItem.MessageType = Constants.MessageTypeValues.Reminder;
            messageListItem.Expires = expirationDate;
            messageListItem.AuthorizedToView = authorized;
            messageListItem.TicketId = ticketId;

            string moduleName = uHelper.getModuleNameByTicketId(ticketId);
            if (uGITModule != null)
                messageListItem.NavigationUrl = string.Format("{0}?TicketId={1}&ModuleName={2}", uGITModule.ModuleRelativePagePath, ticketId, moduleName);

            try
            {
                messageBoardManager.Insert(messageListItem);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "ERROR Updating contract reminder for " + ticketId);
            }
        }
    }
}
