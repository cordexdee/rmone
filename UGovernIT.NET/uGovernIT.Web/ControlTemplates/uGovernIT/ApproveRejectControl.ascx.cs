using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Data;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ApproveRejectControl : UserControl
    {
        protected string headertext = string.Empty;
        protected string footertext = string.Empty;
        UGITTask moduleInstDepny;
        protected string control;
        protected bool svcCall;
        protected string moduleName;
        protected long taskID;
        protected string parentTaskID;
        protected List<UGITTask> taskList;
        protected UGITTask taskItem;
        protected string projectPublicID;
        DataRow project = null;
        //SPList projectList;
        protected string projectID;
        protected string parentTitle;
        ConfigurationVariableManager configVariableManager;
        ApplicationContext AppContext;
        UGITTaskManager TaskManagerObj;
        protected override void OnInit(EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            TaskManagerObj = new UGITTaskManager(AppContext);
            configVariableManager = new ConfigurationVariableManager(AppContext);
            headertext = configVariableManager.GetValue("HomePageHeader");
            footertext = configVariableManager.GetValue("PageFooter");

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!configVariableManager.GetValueAsBool(ConfigConstants.EnableMobileRequest))
                aRefOK.Visible = false;

            string control = Request["control"];
            bool isSVCTaskApprovel = UGITUtility.StringToBoolean(Request["SVCTask"]);

            if (control == "approvereject" && isSVCTaskApprovel)
            {
                AcceptRejectSVCTask();
            }
            else if (control == "approvereject")
            {
                ApproveRejectAction();
            }
        }

        private void ApproveRejectAction()
        {
            if (Request["TicketId"] != null && Request["UserAction"] != null && Request["ModuleName"] != null)
            {
                string moduleName = Request["ModuleName"].Trim();
                string ticketId = Request["TicketId"].Trim();
                string listName = Request["List"] != null ? Request["List"].Trim() : string.Empty;
                
                string listid = string.Format("&List={0}", listName);
                DataRow listitem = Ticket.GetCurrentTicket(AppContext, moduleName, ticketId);
                Ticket objticket = new Ticket(AppContext, moduleName);
                Boolean isAllowed = Ticket.IsActionUser(AppContext, listitem, AppContext.CurrentUser);
                int emailStageStep = UGITUtility.StringToInt(Request["cStage"]);

                LifeCycle lifeCycle = objticket.GetTicketLifeCycle(listitem);
                LifeCycleStage currentLifeCycleStage = objticket.GetTicketCurrentStage(listitem, lifeCycle);

                string userAction = Request["UserAction"].ToLower();

                if (isAllowed && (currentLifeCycleStage.StageStep == emailStageStep || Request["cStage"] == null))
                {
                    if (userAction == "approve")
                    {
                        objticket.Approve(AppContext.CurrentUser, listitem, false);
                        objticket.AssignModuleSpecificDefaults(listitem);
                        objticket.SetTicketPriority(listitem, moduleName);

                        objticket.CommitChanges(listitem, string.Empty, Request.Url);
                        
                        LifeCycleStage newLifeCycleStage = objticket.GetTicketCurrentStage(listitem);

                        bool sendNotification = true;
                        if (currentLifeCycleStage == newLifeCycleStage && newLifeCycleStage.StageAllApprovalsRequired)
                            sendNotification = false;
                        if (sendNotification)
                            objticket.SendEmailToActionUsers(Convert.ToString(newLifeCycleStage.ID), listitem, UGITUtility.ObjectToString(objticket.Module.ID));

                        //This will ensure start date and end date are updated if they have previous dates from templates
                        UGITModuleConstraint.ConfigureCurrentModuleStageTask(AppContext, listitem);
                        //Update Cache
                        //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(objticket.Module.ID, listitem);

                        string url = string.Empty;
                        if (string.IsNullOrEmpty(listName))
                            url = string.Format("{0}?TicketId={1}&ModuleName={2}&isudlg=1" + listid, Constants.HomePage, ticketId, moduleName);
                        else
                            url = string.Format(UGITUtility.GetAbsoluteURL(Constants.MobileTicketEdit + "TicketId={0}&List={1}"), ticketId, listName);

                        lblHeader.Text = "Approval Complete";
                        spanSuccessMessage.InnerHtml = string.Format("<b>Thank You!</b><br><br>{0} <b>[{1}: {2}]</b> has been successfully approved!<br><br>You can now close this page.<br>",
                                                                     UGITUtility.moduleTypeName(moduleName),
                                                                     ticketId,
                                                                     UGITUtility.ObjectToString(listitem[DatabaseObjects.Columns.Title]));
                        aRefOK.HRef = UGITUtility.GetAbsoluteURL(url);
                    }
                    else if (userAction == "reject")
                    {
                        string url = string.Empty;
                        if (string.IsNullOrEmpty(Request["List"]))
                            url = string.Format("{0}?TicketId={1}&ModuleName={2}&UserAction=Reject&isudlg=1" + listid, UGITUtility.GetAbsoluteURL(Constants.HomePage), ticketId, moduleName);
                        else
                            url = string.Format(UGITUtility.GetAbsoluteURL(Constants.MobileTicketEdit + "TicketId={0}&List={1}&Actiontype=Reject"), ticketId, listName);

                        Response.Redirect(url);
                    }
                    else if (userAction == "return")
                    {
                        string url = string.Empty;
                        if (string.IsNullOrEmpty(Request["List"]))
                            url = string.Format("{0}?TicketId={1}&ModuleName={2}&UserAction=Return&isudlg=1" + listid, UGITUtility.GetAbsoluteURL(Constants.HomePage), ticketId, moduleName);
                        else
                            url = string.Format(UGITUtility.GetAbsoluteURL(Constants.MobileTicketEdit + "TicketId={0}&List={1}&Actiontype=Return"), ticketId, listName);

                        Response.Redirect(url);
                    }
                }
                else
                {
                    string url = string.Empty;
                    if (string.IsNullOrEmpty(Request["List"]))
                        url = string.Format("{0}?TicketId={1}&ModuleName={2}" + listid, Constants.HomePage, ticketId, moduleName);
                    else
                        url = string.Format(UGITUtility.GetAbsoluteURL(Constants.MobileTicketEdit + "TicketId={0}&List={1}&Actiontype=Return"), ticketId, listName);

                    if (userAction == "approve")
                        lblHeader.Text = "Approval Unsuccessful";
                    else if (userAction == "reject")
                        lblHeader.Text = "Rejection Unsuccessful";
                    else if (userAction == "return")
                        lblHeader.Text = "Return Unsuccessful";

                    string moduleTypeName = UGITUtility.moduleTypeName(moduleName);
                    spanSuccessMessage.InnerHtml = string.Format("{0} <b>[{1}: {2}]</b> could not be {3}:<br><br>",
                                                                 moduleTypeName,
                                                                 ticketId,
                                                                 UGITUtility.ObjectToString(listitem[DatabaseObjects.Columns.Title]),
                                                                 userAction == "approve" ? "approved" : userAction + "ed");

                    if (!isAllowed)
                    {
                        spanSuccessMessage.InnerHtml += string.Format("You are not authorized to {1} this {0} at this stage.<br>",
                                                                      moduleTypeName.ToLower(),
                                                                      userAction);
                    }
                    else
                    {
                        spanSuccessMessage.InnerHtml += string.Format("This {0} is no longer in the same stage - it may already have been approved/rejected.<br>",
                                                                      moduleTypeName.ToLower());
                    }
                    aRefOK.HRef = UGITUtility.GetAbsoluteURL(url);
                }
            }
            else
            {
                spanSuccessMessage.InnerText = string.Format("Malicious input");
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {

        }

        protected void AcceptRejectSVCTask()
        {
            if (Request["taskID"] != null && Request["UserAction"] != null && Request["ModuleName"] != null)
            {
                long.TryParse(Request["taskID"], out taskID);
                parentTaskID= Request["parentTaskID"];
                moduleName = Request["moduleName"];
               
                //taskList = SPListHelper.GetSPList(DatabaseObjects.Lists.TicketRelationship);
                taskItem = TaskManagerObj.LoadByID(taskID); // SPListHelper.GetSPListItem(taskList, taskID);
              
                moduleInstDepny = TaskManagerObj.LoadByID(taskID);
            }

            if (moduleInstDepny != null)
            {
                Ticket objticket = new Ticket(AppContext, moduleName);
                //projectList = SPListHelper.GetSPList(DatabaseObjects.Lists.SVCRequests);
                if (!string.IsNullOrEmpty(projectID))
                {
                    project = Ticket.GetCurrentTicket(AppContext, moduleName, Request["parentTaskID"]);
                    projectID = UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketId]);
                }
                //else
                //{
                //    project = SPListHelper.GetSPListItem(projectList, projectID);
                //}

                projectPublicID = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.TicketId));
                parentTitle = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));
                string userAction = Request["UserAction"].ToLower();
                if (userAction == "approve")
                {
                    UserProfile currentUser = AppContext.CurrentUser;
                    string currentUserId = UGITUtility.ObjectToString(currentUser.Id);

                    //List<Role> currUserGroups = AppContext.UserManager.group
                    List<string> currUserGroupIDs = new List<string>();
                    currUserGroupIDs = AppContext.UserManager.GetUserGroups(currentUser.Id);
                    //foreach (SPGroup group in _spWeb.CurrentUser.Groups)
                    //{
                    //    currUserGroupIDs.Add(group.ID.ToString());
                    //}

                    bool userExist = false;
                    List<string> taskActionUsers = new List<string>();
                    bool isTicketAdmin = AppContext.UserManager.IsTicketAdmin(AppContext.CurrentUser);
                    if (!string.IsNullOrEmpty(moduleInstDepny.TaskActionUser))
                    {
                        taskActionUsers = UGITUtility.SplitString(moduleInstDepny.TaskActionUser, Constants.Separator).ToList();
                        userExist = taskActionUsers.Exists(x => x == currentUserId || currUserGroupIDs.Contains(x));

                        if (!userExist && !isTicketAdmin)
                            return;
                    }

                    List<UserProfile> oldApprovers = AppContext.UserManager.GetUserInfosById( moduleInstDepny.Approver);

                    //bool approveTask = false;
                    if (!string.IsNullOrEmpty(moduleInstDepny.ApprovalType) && moduleInstDepny.ApprovalType.ToLower() != ApprovalType.None)
                    {
                        if (string.IsNullOrEmpty(moduleInstDepny.TaskActionUser))
                        {
                            //approveTask = true;
                            moduleInstDepny.ApprovalStatus = "Approved";
                        }
                        else
                        {
                            if (taskActionUsers.Count > 0)
                            {
                                if (userExist)
                                {
                                    //approveTask = true;

                                    // Remove approver if present in list of approvers
                                    taskActionUsers = taskActionUsers.Where(x => x != currentUserId && !currUserGroupIDs.Contains(x)).ToList();

                                    moduleInstDepny.TaskActionUser = String.Join(Constants.Separator, taskActionUsers);
                                    moduleInstDepny.ApprovalStatus = string.IsNullOrEmpty(moduleInstDepny.TaskActionUser) ? "Approved" : "Pending";
                                }
                                else if (isTicketAdmin)
                                {
                                    //approveTask = true;
                                    moduleInstDepny.TaskActionUser = string.Empty;
                                    moduleInstDepny.ApprovalStatus = "Approved";
                                }
                            }
                        }
                    }

                    //if (moduleInstDepny.ApprovedBy == null)
                    //    moduleInstDepny.ApprovedBy = new SPFieldUserValueCollection();
                    if (currentUser != null && (userExist || isTicketAdmin) && !moduleInstDepny.ApprovedBy.Contains(currentUser.Id))
                        moduleInstDepny.ApprovedBy = (currentUser.Id);
                    moduleInstDepny.Status = Constants.InProgress;

                    TaskManagerObj.Save(moduleInstDepny);
                    //if (approveTask)
                    //    moduleInstDepny.UpdateEventLogForTask(_spWeb, Constants.TicketEventType.Approved, moduleInstDepny.ParentInstance, moduleInstDepny.Title, moduleInstDepny.ID.ToString(), new SPFieldUserValueCollection() { currentUser });


                    taskItem = TaskManagerObj.LoadByID(moduleInstDepny.ID); // SPListHelper.GetSPListItem(taskList, moduleInstDepny.ID);

                    if (taskItem != null)
                    {
                        {
                            //SPFieldUserValueCollection assignedUsers = moduleInstDepny.AssignedTo; //uHelper.GetFieldUserValueCollection(peAssignedTo.ResolvedEntities);
                            if (!string.IsNullOrEmpty(moduleInstDepny.AssignedTo))
                            {
                                List<UserProfile> users = AppContext.UserManager.GetUserInfosById(moduleInstDepny.AssignedTo);
                                List<string> emails = new List<string>();
                                StringBuilder mailToNames = new StringBuilder();

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
                                    taskToEmail.Add(DatabaseObjects.Columns.ProjectID, projectPublicID);
                                    taskToEmail.Add(DatabaseObjects.Columns.ProjectTitle, Convert.ToString(project[DatabaseObjects.Columns.Title]));
                                    taskToEmail.Add(DatabaseObjects.Columns.Title, moduleInstDepny.Title);
                                    taskToEmail.Add(DatabaseObjects.Columns.LinkDescription, moduleInstDepny.Description);
                                    taskToEmail.Add(DatabaseObjects.Columns.StartDate, moduleInstDepny.StartDate.ToString());
                                    taskToEmail.Add(DatabaseObjects.Columns.DueDate, moduleInstDepny.EndDate.ToString());
                                    taskToEmail.Add(DatabaseObjects.Columns.EstimatedHours, moduleInstDepny.EstimatedHours.ToString());
                                    taskToEmail.Add("IsService", "true");

                                    string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", projectPublicID, moduleInstDepny.ID, moduleName);
                                    string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, AppContext.TenantID);

                                    StringBuilder taskEmail = new StringBuilder();
                                    string greeting = configVariableManager.GetValue("Greeting");
                                    string signature = configVariableManager.GetValue("Signature");

                                    taskEmail.AppendFormat("{0} {1}<br /><br />", greeting, mailToNames.ToString());
                                    taskEmail.AppendFormat("A new task <b>\"{0}\"</b> has been assigned to you by {1}.<br>", moduleInstDepny.Title, currentUser.Name);
                                    taskEmail.Append("<br /><br />" + signature + "<br />");
                                    taskEmail.Append(emailFooter);
                                    string emailSubject = moduleInstDepny.ParentInstance + " New Task Assigned: " + moduleInstDepny.Title;
                                    string attachUrl = string.Empty;
                                    if (UGITUtility.StringToBoolean(configVariableManager.GetValue(ConfigConstants.AttachTaskCalendarEntry)))
                                    {
                                        //Location where you want to save the vCalendar file
                                        attachUrl =
                                           uHelper.GetUploadFolderPath() + @"\" + moduleInstDepny.Title + ".vcs";

                                        //Create task
                                        using (StreamWriter sw = new StreamWriter(attachUrl))
                                        {
                                            sw.Write(CreateTask(Convert.ToDateTime(moduleInstDepny.StartDate), Convert.ToDateTime(moduleInstDepny.EndDate), emailSubject, taskEmail.ToString()));
                                        }
                                    }
                                    MailMessenger mailMessage = new MailMessenger(AppContext);
                                    if (configVariableManager.GetValueAsBool(ConfigConstants.KeepSVCTaskNotifications) && !string.IsNullOrWhiteSpace(moduleName) && moduleName == "SVC")
                                        mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl },saveToTicketId: moduleInstDepny.ParentInstance);
                                    else
                                        mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl });
                                }
                            }
                        }
                    }

                    //TaskCache.ReloadProjectTasks(moduleName, projectPublicID);
                    TaskManagerObj.StartTasks( projectPublicID);
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else if (userAction == "reject")
                {
                    commentsRejectPopup.ShowOnPageLoad = true;
                }

            }
        }
        static string CreateTask(DateTime start, DateTime end, string sub, string msgBody)
        {
            StringBuilder sbvCalendar = new StringBuilder();

            //Header
            sbvCalendar.Append("METHOD: REQUEST");
            sbvCalendar.Append("\n");
            sbvCalendar.Append("BEGIN:VCALENDAR");
            sbvCalendar.Append("\n");
            sbvCalendar.Append("PRODID:-//Microsoft Corporation//Outlook ");
            sbvCalendar.Append("\n");
            sbvCalendar.Append("VERSION:2.0");
            sbvCalendar.Append("\n");
            sbvCalendar.Append("BEGIN:VEVENT");
            sbvCalendar.Append("\n");

            //DTSTART 
            sbvCalendar.Append("DTSTART:");
            string hour = start.Hour.ToString();
            if (hour.Length < 2) { hour = "0" + hour; }

            string min = start.Minute.ToString();
            if (min.Length < 2) { min = "0" + min; }

            string sec = start.Second.ToString();
            if (sec.Length < 2) { sec = "0" + sec; }

            string mon = start.Month.ToString();
            if (mon.Length < 2) { mon = "0" + mon; }

            string day = start.Day.ToString();
            if (day.Length < 2) { day = "0" + day; }

            sbvCalendar.Append(start.Year.ToString() + mon + day
                                   + "T" + hour + min + sec);
            sbvCalendar.Append("\n");

            //DTEND
            sbvCalendar.Append("DTEND:");
            hour = end.Hour.ToString();
            if (hour.Length < 2) { hour = "0" + hour; }

            min = end.Minute.ToString();
            if (min.Length < 2) { min = "0" + min; }

            sec = end.Second.ToString();
            if (sec.Length < 2) { sec = "0" + sec; }

            mon = end.Month.ToString();
            if (mon.Length < 2) { mon = "0" + mon; }

            day = end.Day.ToString();
            if (day.Length < 2) { day = "0" + day; }

            sbvCalendar.Append(end.Year.ToString() + mon +
                         day + "T" + hour + min + sec);
            sbvCalendar.Append("\n");

            //Location
            sbvCalendar.Append("LOCATION;ENCODING=QUOTED-PRINTABLE: "
                                                     + String.Empty);
            sbvCalendar.Append("\n");

            //Message body
            //sbvCalendar.Append("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:"
            //                                                + msgBody);
            sbvCalendar.AppendFormat("X-ALT-DESC;FMTTYPE=text/html:{0}", msgBody);
            sbvCalendar.Append("\n");

            //Subject
            sbvCalendar.Append("SUMMARY;ENCODING=QUOTED-PRINTABLE:"
                                                            + sub);
            sbvCalendar.Append("\n");

            //Priority
            sbvCalendar.Append("PRIORITY:3");
            sbvCalendar.Append("\n");
            sbvCalendar.Append("END:VEVENT");
            sbvCalendar.Append("\n");
            sbvCalendar.Append("END:VCALENDAR");
            sbvCalendar.Append("\n");

            return sbvCalendar.ToString();
        }

        protected void aspxRejectbutton_Click(object sender, EventArgs e)
        {
            if (moduleInstDepny != null)
            {
                commentsRejectPopup.ShowOnPageLoad = false;

                moduleInstDepny.ApprovalStatus = "Rejected";

                string message = string.Empty;
                message = string.Format("Rejected Task [{0}] which was in {1} state", moduleInstDepny.Title, moduleInstDepny.Status);
                uHelper.CreateHistory(AppContext.CurrentUser, message, project, AppContext);
                //Log.AuditTrail(string.Format("User {0} {1} from ticket {2} [{3}]", _spWeb.CurrentUser.Name, message, projectPublicID, parentTitle));

                UserProfile currentApprover = AppContext.CurrentUser; // new SPFieldUserValue(_spWeb, _spWeb.CurrentUser.ID, _spWeb.CurrentUser.LoginName);

                moduleInstDepny.Approver = null;
                moduleInstDepny.TaskActionUser = string.Empty;
                moduleInstDepny.Status = Constants.Cancelled;
                moduleInstDepny.PercentComplete = 100;

                moduleInstDepny.Comment = "[Reject Reason]: " + txtTaskComment.Text.Trim();

                TaskManagerObj.Save(moduleInstDepny);

                TaskManagerObj.UpdateEventLogForTask(AppContext, Constants.TicketEventType.Rejected, moduleInstDepny.ParentInstance, moduleInstDepny.Title, moduleInstDepny.ID.ToString());

                //TaskCache.ReloadProjectTasks("SVC", moduleInstDepny.ParentInstance);
                TaskManagerObj.StartTasks(moduleInstDepny.ParentInstance);
                TaskManagerObj.MoveSVCTicket(moduleInstDepny.ParentInstance);

                // Send Reject notification to SVC ticket requestor if configured
                //if (uGITCache.GetConfigVariableValueAsBool(ConfigConstants.SVCTaskCancelNotifyRequestor) && project != null && project[DatabaseObjects.Columns.TicketRequestor] != null)
                if (project != null && (project[DatabaseObjects.Columns.TicketRequestor] != null || project[DatabaseObjects.Columns.TicketOwner] != null))
                {
                    Ticket tReq = new Ticket(AppContext, "SVC");
                    if (tReq != null)
                    {
                        string subject = string.Format("Task Rejected: {0}", moduleInstDepny.Title);
                        string body = string.Format("Task: <b>[{0}]</b> of service request <b>[{1}: {2}]</b> has been rejected by <b>{3}</b> <br/><br/> <b>Reason: </b> {4}", moduleInstDepny.Title, UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.TicketId), UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title), AppContext.CurrentUser.Name, txtTaskComment.Text.Trim());
                        string[] fieldlist = { DatabaseObjects.Columns.TicketRequestor, DatabaseObjects.Columns.Owner };
                        List<UsersEmail> emailUsers = AppContext.UserManager.GetUsersEmail(project, fieldlist, false);
                        if (configVariableManager.GetValueAsBool(ConfigConstants.KeepSVCTaskNotifications) && !string.IsNullOrWhiteSpace(moduleName) && moduleName == "SVC")
                            tReq.SendEmailToEmailList(project, subject, body, emailUsers);
                        else
                            tReq.SendEmailToEmailList(project, subject, body, emailUsers);
                    }
                }

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
    }
}

