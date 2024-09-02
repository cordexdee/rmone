
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
using uGovernIT.Helpers;
//using uGovernIT.Report.Entities;
 

namespace uGovernIT.DxReport
{
    public class ReportAgentJobHelper
    {
        ApplicationContext _context;
        UserProfile user;
        ModuleViewManager objModuleViewManager=null;
        TicketManager objTicketManager = null;
        ScheduleActionsManager scheduleActionsManager = null;
        public ReportAgentJobHelper(ApplicationContext context)
        {
            _context = context;
            user = _context.CurrentUser;
            objModuleViewManager = new ModuleViewManager(_context);
            objTicketManager = new TicketManager(_context);
            scheduleActionsManager = new ScheduleActionsManager(_context);
        }
  
        public bool IsItemDelete { get; set; }

  
        #region Email
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="scheduleAction">The schedule action.</param>
        //public void SendEmail(SPListItem scheduleAction)
        //{
        //    StringBuilder body = new StringBuilder();
        //    string cc = string.Empty;
        //    string to = string.Empty;
        //    try
        //    {
        //        string logs = SendMail(scheduleAction);
        //        if (String.IsNullOrEmpty(logs))
        //        {
        //            CreateLogs(scheduleAction, AgentJobStatus.Success, logs);
        //            bool recurring = Convert.ToBoolean(scheduleAction[DatabaseObjects.Columns.Recurring]);
        //            if (recurring)
        //            {
        //                UpdateIsRecurrence(scheduleAction, null);
        //            }
        //            else
        //            {
        //                IsItemDelete = true;
        //            }
        //        }
        //        else
        //        {
        //            CreateLogs(scheduleAction, AgentJobStatus.Fail, logs);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IsItemDelete = false;
        //        CreateLogs(scheduleAction, AgentJobStatus.Fail, ex.ToString());
        //    }

        //}

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
            objScheduleActionsArchive.TicketId =   scheduleActionItem.TicketId;
            objScheduleActionsArchive.Recurring =  scheduleActionItem.Recurring;
            objScheduleActionsArchive.RecurringInterval = Convert.ToInt32(scheduleActionItem.RecurringInterval);
            if (scheduleActionItem.RecurringEndDate != null)
            {
                objScheduleActionsArchive.RecurringEndDate = Convert.ToDateTime(scheduleActionItem.RecurringEndDate);
            }
            else {
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
        private void UpdateIsRecurrence(SchedulerAction scheduleAction, object recurringEndDate)
        {
            ScheduleActionsManager objScheduleActionsManager = new ScheduleActionsManager(_context);
            SchedulerAction objScheduleAction = new SchedulerAction();
            DateTime recurringEnd = DateTime.MaxValue;
            if (recurringEndDate != null)
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

            //SPFieldLookupValue spFieldLookupModule = new SPFieldLookupValue(Convert.ToString(scheduleAction[DatabaseObjects.Columns.ModuleNameLookup]));
            string moduleName = scheduleAction.ModuleNameLookup;    //spFieldLookupModule != null ? spFieldLookupModule.LookupValue : string.Empty;
            string actionType = scheduleAction.ActionType;

            while (myStartTime < DateTime.Now)
            {
                if (actionType == "Reminder" && moduleName == "CMT")
                    myStartTime = myStartTime.AddMinutes(recurringIntervalMinutes); // Recurring interval is in calendar time!
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
                                DateTime nextWeekStartDay = uHelper.FirstDayOfWeek(_context,myStartTime.AddDays(7));
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
                            myStartTime = uHelper.GetWorkingEndDate(_context,myStartTime, recurringIntervalMinutes);//EscalationProcess.AddTimeInWorkingHours(scheduleAction.Web, myStartTime, recurringIntervalMinutes);
                        }
                    }
                }
                else // for escalations, etc
                {
                    // add recurring interval in business hours only
                    myStartTime = uHelper.GetWorkingEndDate(_context,myStartTime, recurringIntervalMinutes);//EscalationProcess.AddTimeInWorkingHours(scheduleAction.Web, myStartTime, recurringIntervalMinutes);
                }
            }

            if (recurringEnd < myStartTime)
            {
                IsItemDelete = true;
                return;
            }

            scheduleAction.StartTime = myStartTime;
            scheduleAction.RecurringInterval =Convert.ToInt32(recurringIntervalMinutes);
            ScheduleActionType ActionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType),scheduleAction.ActionType);
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
                    scheduleAction.RecurringInterval = uHelper.GetRecurringIntervalInMinutes(_context,repeatInterval, UGITUtility.StringToDateTime(scheduleAction.StartTime));
                }
            }
            if (ActionType != ScheduleActionType.Query && ActionType != ScheduleActionType.Report && ActionType != ScheduleActionType.Reminder)
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
            ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(_context);
            string errMessage = string.Empty;
            try
            {
                ///Get subject.
                string subject = scheduleAction.MailSubject;

                ///Get Email To id
                string to = scheduleAction.EmailIDTo;

                ///Get Email CC id
                string cc = scheduleAction.EmailIDCC;

                ///Get Email Body content from Schedule action.
                StringBuilder body = new StringBuilder();
                body.Append(Convert.ToString(scheduleAction.EmailBody));
                body.Append("<br />");
                body.Append("<br />");
                body.Append(objConfigurationVariableManager.GetValue(ConfigConstants.Signature));

                //if (scheduleAction.ActionType == "Reminder")
                //{
                //    if (scheduleAction.TicketId != string.Empty)
                //    {
                //        string ticketId = scheduleAction.TicketId;
                //        string moduleName = uHelper.getModuleNameByTicketId(ticketId);
                //        DataRow currentticket = Ticket.getCurrentTicket(_context,moduleName, ticketId);
                //        string ticketDetailFooter = uHelper.GetTicketDetailsForEmailFooter(_context,currentticket, moduleName, true, false);
                //        body.Append(ticketDetailFooter);
                //    }
                //}

                MailMessenger mail = new MailMessenger(_context);
                errMessage = mail.SendMail(to, subject, cc, body.ToString(), true, attachments);
            }
            catch (Exception ex)
            {
                errMessage = ex.ToString();
            }

            return errMessage;
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
                string serialized = scheduleAction.ActionTypeData;
                Dictionary<string, object> formdict = UGITUtility.DeserializeDicObject(serialized);
                
                TypeOfReport reportType = (TypeOfReport)Enum.Parse(typeof(TypeOfReport), Convert.ToString(formdict["Report"]));
                string attachedFormat = scheduleAction.AttachmentFormat;
                if (!formdict.ContainsKey(DatabaseObjects.Columns.Title))
                    formdict.Add(DatabaseObjects.Columns.Title, scheduleAction.Title);

                Type reportSchedularType = typeof(IReportScheduler);
                if (reportSchedularType != null)
                {
                    Type mytype = reportSchedularType.Assembly.GetTypes().FirstOrDefault(x => x.Name == Convert.ToString(reportType+ "_Scheduler") && x.GetInterfaces().Contains(typeof(IReportScheduler)));
                    if (mytype != null)
                    {
                        IReportScheduler classObj = Activator.CreateInstance(mytype) as IReportScheduler;
                        fileName =  classObj.GetReport(_context, formdict, attachedFormat);
                    }
                }

               logMessage = SendMail(scheduleAction, new string[] { fileName });
            }
            catch (Exception ex)
            {
                logMessage = ex.ToString();
            }

            if (String.IsNullOrEmpty(logMessage))
            {
                CreateLogs(scheduleAction, AgentJobStatus.Success, logMessage);
                bool recurring = scheduleAction.Recurring;
                if (recurring)
                {
                   UpdateIsRecurrence(scheduleAction, null);
                }
                else
                {
                    IsItemDelete = true;
                }
            }
            else
            {
                CreateLogs(scheduleAction, AgentJobStatus.Fail, logMessage);
            }
        }

        /// <summary>
        /// This method is used to generate report based on the ScheduleAction and return report path
        /// </summary>
        /// <param name="scheduleActionID"></param>
        /// <returns></returns>
        public string GetReport(long scheduleActionID)
        {
            string fileName = string.Empty;
            try
            {
                if (scheduleActionID == 0)
                    return fileName;

                ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(_context);
                SchedulerAction scheduleAction = scheduleActionsManager.LoadByID(scheduleActionID);
                if (string.IsNullOrEmpty(UGITUtility.ObjectToString(scheduleAction)))
                    return fileName;
                string serialized = scheduleAction.ActionTypeData;

                if (string.IsNullOrEmpty(serialized))
                    return fileName;

                Dictionary<string, object> formdict = UGITUtility.DeserializeDicObject(serialized);

                TypeOfReport reportType = (TypeOfReport)Enum.Parse(typeof(TypeOfReport), Convert.ToString(formdict["Report"]));
                string attachedFormat = scheduleAction.AttachmentFormat;
                if (!formdict.ContainsKey(DatabaseObjects.Columns.Title))
                    formdict.Add(DatabaseObjects.Columns.Title, scheduleAction.Title);

                Type reportSchedularType = typeof(IReportScheduler);
                if (reportSchedularType != null)
                {
                    Type mytype = reportSchedularType.Assembly.GetTypes().FirstOrDefault(x => x.Name == Convert.ToString(reportType + "_Scheduler") && x.GetInterfaces().Contains(typeof(IReportScheduler)));
                    if (mytype != null)
                    {
                        IReportScheduler classObj = Activator.CreateInstance(mytype) as IReportScheduler;
                        fileName = classObj.GetReport(_context, formdict, attachedFormat);
                    }
                }

                return fileName;
            }
            catch (Exception ex)
            {
                ULog.WriteLog(ex.ToString());
                return fileName;
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

        #region Schedule Action Type Alert
        //public void SendAlert(DataRow scheduleAction)
        //{
        //    int queryID = Convert.ToInt32(scheduleAction[DatabaseObjects.Columns.TicketId]);
        //    string title = Convert.ToString(scheduleAction[DatabaseObjects.Columns.Title]);
        //    List<string> wherevalue = new List<string>();
        //    DataTable dataTable = new DataTable();
        //    string serialized = Convert.ToString(scheduleAction[DatabaseObjects.Columns.CustomProperties]);
        //    if (string.IsNullOrEmpty(serialized))
        //        serialized = Convert.ToString(scheduleAction[DatabaseObjects.Columns.ActionTypeData]);

        //    Dictionary<string, object> customProperties = UGITUtility.DeserializeDicObject(serialized);

        //    if (customProperties.ContainsKey(ReportScheduleConstant.Where))
        //    {
        //        string where = Convert.ToString(customProperties[ReportScheduleConstant.Where]);
        //        wherevalue = UGITUtility.ConvertStringToList(where, ",");
        //    }

        //    UDashboard dashboard = UDashboard.(queryID, false);
        //    DashboardQuery dashboardQuery = (DashboardQuery)dashboard.panel;

        //    DataTable dtTotals = new DataTable();
        //    bool isParameterize = dashboardQuery.QueryInfo.WhereClauses.Exists(w => w.ParameterName != string.Empty);
        //    if (isParameterize)
        //    {
        //        string whereFilter = string.Join(",", wherevalue.ToArray());

        //        dataTable = QueryHelper.GetReportData(dashboardQuery.QueryTable, dashboardQuery.QueryInfo, whereFilter, ref dtTotals, _spWeb, true);
        //    }
        //    else
        //    {
        //        dataTable = QueryHelper.GetReportData(dashboardQuery.QueryTable, dashboardQuery.QueryInfo, string.Empty, ref dtTotals, _spWeb, true);
        //    }
        //    if (dataTable == null || dataTable.Rows.Count == 0) return;

        //    var valueInData = Convert.ToInt32(dataTable.Rows[0][0]);
        //    var value = 0;
        //    bool sendAlert = false;

        //    var alertConditionstr = Convert.ToString(scheduleAction[DatabaseObjects.Columns.AlertCondition]);
        //    Regex regexCondition = new Regex(RegularExpressionConstant.ConditionExpress);
        //    if (regexCondition.IsMatch(alertConditionstr))
        //    {
        //        #region Value from Condition
        //        Regex regexValue = new Regex(RegularExpressionConstant.ValueExpress);
        //        if (regexValue.IsMatch(alertConditionstr))
        //        {
        //            value = Convert.ToInt32(regexValue.Match(alertConditionstr).Value);
        //            //alertCondition.Value = value;
        //        }
        //        #endregion

        //        #region operator from Condition
        //        Regex regexOperator = new Regex(RegularExpressionConstant.OperatorTypeExpress, RegexOptions.IgnoreCase);
        //        if (regexOperator.IsMatch(alertConditionstr))
        //        {
        //            var operatorType = regexOperator.Match(alertConditionstr).Value;
        //            switch (operatorType)
        //            {
        //                case "==":
        //                    sendAlert = (valueInData == value);
        //                    break;
        //                case ">=":
        //                    sendAlert = (valueInData >= value);
        //                    break;
        //                case "<=":
        //                    sendAlert = (valueInData <= value);
        //                    break;
        //                case ">":
        //                    sendAlert = (valueInData > value);
        //                    break;
        //                case "<":
        //                    sendAlert = (valueInData < value);
        //                    break;
        //                default:
        //                    break;
        //            }

        //        }
        //        #endregion

        //    }

        //    if (sendAlert)
        //    {
        //        string errMsg = SendMail(scheduleAction);
        //        if (string.IsNullOrEmpty(errMsg))
        //        {
        //          //  CreateLogs(scheduleAction, AgentJobStatus.Success, errMsg);
        //            IsItemDelete = true;
        //        }
        //        else
        //        {
        //           // CreateLogs(scheduleAction, AgentJobStatus.Fail, errMsg);
        //        }
        //    }

        //}
        #endregion
    }
}
        #endregion
