using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using uGovernIT.Core;
using uGovernIT.Utility;
using System.Xml.Linq;
using uGovernIT.DAL;
using System.Web;
using System.Text.RegularExpressions;
using uGovernIT.Utility.Entities;
using uGovernIT.DAL.Store;
using uGovernIT.Util.Log;
using uGovernIT.Manager.Managers;
using uGovernIT.Manager.Core;
using System.Threading;
using System.Collections;
using uGovernIT.Util.ImportExportMPP;
using uGovernIT.Manager;
using System.Xml;
using uGovernIT.Manager.Helper;
using uGovernIT.Web.Helpers;
using System.Linq.Expressions;
using uGovernIT.Utility.Helpers;
namespace uGovernIT.Manager
{
    public class UGITTaskManager : ManagerBase<UGITTask>, IUGITTaskManager
    {
        ApplicationContext _context;
        ConfigurationVariableManager objConfigurationVariableHelper;
        ModuleViewManager ObjModuleViewManager;
        TicketManager ticketManager;
        ProjectStageHistoryManager prjStageHistoryMgr;
        //public bool IsStartDateChange { get; set; }
        //public bool IsEndDateChange { get; set; }

        //added 24 jan 2020
        public bool chkSaveTaskDate { get; set; }

        //
        public UGITTaskManager(ApplicationContext context) : base(context)
        {
            _context = context;
            store = new UGITTaskManagerStore(this.dbContext);
            objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
            ObjModuleViewManager = new ModuleViewManager(context);
            prjStageHistoryMgr = new ProjectStageHistoryManager(context);
        }
        public UGITTaskManager(ApplicationContext context, string listName, string lookupFieldName) : base(context)
        {
            _context = context;
            ListName = listName;
            LookupFieldName = lookupFieldName;
            store = new UGITTaskManagerStore(this.dbContext);
            objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
            ObjModuleViewManager = new ModuleViewManager(context);

        }

        public UGITTaskManager(ApplicationContext context, string requestPage = "") : base(context, requestPage)
        {

            // changes made for New Ui Admin  userInfo login and submit
            if (requestPage == "SelfRegistration")
            {
                _context = ApplicationContext.Create();
            }
            else
            {
                _context = context;
            }
            store = new UGITTaskManagerStore(this.dbContext);
            objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
            ObjModuleViewManager = new ModuleViewManager(_context);
        }

        public string ListName { get; set; }
        public string LookupFieldName { get; set; }
        public UGITTask Save(UGITTask task)
        {
            if (task.ID > 0)
                this.Update(task);
            else
                this.Insert(task);
            return task;
        }
        public bool Save(UGITTask dependency, ApplicationContext _appContext)
        {
            if (dependency == null)
                return false;
            DataTable dtmoduleTasks = GetTableDataManager.GetTableStructure(DatabaseObjects.Tables.ModuleTasks);
            //SPList ticketRelationships = SPListHelper.GetSPList(DatabaseObjects.Lists.TicketRelationship, spWeb);
            UGITTask relationship = null;
            DataRow taskRow = null;
            bool logStatusChangeEvent = false;
            bool logAssigneeChangeEvent = false;
            bool logTicketTaskCreated = false;
            long ID = 0;
            long.TryParse(Convert.ToString(dependency.ID), out ID);
            UGITTaskManager uGITTaskManager = new UGITTaskManager(_appContext);
            if (ID > 0)
            {

                taskRow = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.ModuleTasks, DatabaseObjects.Columns.ID, ID).FirstOrDefault();
                relationship = uGITTaskManager.LoadByID(ID);
                //relationship = SPListHelper.GetSPListItem(ticketRelationships, ID);
                string oldStatus = Convert.ToString(relationship.Status);
                //Convert.ToString(UGITUtility.GetSPItemValue(relationship, DatabaseObjects.Columns.Status));
                if (oldStatus == "Waiting" && oldStatus != dependency.Status)
                    dependency.TaskActualStartDate = DateTime.Now;

                if (dependency.Behaviour == "Task")
                {
                    if (!oldStatus.Equals(dependency.Status))
                        logStatusChangeEvent = true;

                    logAssigneeChangeEvent = GetAssignToDifference(dependency, relationship);
                }
            }
            else
            {
                relationship = new UGITTask();
                if (string.IsNullOrWhiteSpace(dependency.RelatedModule))
                    logStatusChangeEvent = true;
                else
                    logTicketTaskCreated = true;
            }

            relationship.Title = UGITUtility.TruncateWithEllipsis(UGITUtility.StripHTML(dependency.Title), 255); // Remove invalid HTML and truncate to 255 (SharePoint limit)
            if (UGITUtility.IsSPItemExist(dtmoduleTasks.NewRow(), DatabaseObjects.Columns.ErrorMsg))
            {
                relationship.ErrorMsg = dependency.ErrorMsg;
            }

            if (UGITUtility.IfColumnExists(dtmoduleTasks.NewRow(), DatabaseObjects.Columns.ModuleNameLookup))
                relationship.ModuleNameLookup = dependency.ModuleNameLookup;

            if (!string.IsNullOrWhiteSpace(dependency.RelatedModule))
            {
                relationship.RelatedModule = dependency.RelatedModule;
                relationship.Behaviour = "Ticket";
                dependency.Behaviour = "Ticket";
            }
            else
            {
                relationship.RelatedModule = null;
                relationship.Behaviour = "Task";
                dependency.Behaviour = "Task";
            }

            relationship.Predecessors = null;
            if (!string.IsNullOrWhiteSpace(dependency.Predecessors))
            {
                relationship.Predecessors = dependency.Predecessors;
            }

            relationship.AssignedTo = null;
            if (!string.IsNullOrWhiteSpace(dependency.AssignedTo))
            {
                relationship.AssignedTo = dependency.AssignedTo;
            }

            relationship.Description = dependency.Description;
            relationship.TicketId = dependency.TicketId;
            relationship.RelatedTicketID = dependency.RelatedTicketID;

            if (UGITUtility.IfColumnExists(dtmoduleTasks.NewRow(), DatabaseObjects.Columns.Comment))
            {
                if (dependency.Comment != null && dependency.Comment.Trim() != string.Empty && (!dependency.Comment.StartsWith(";#False<;#>")))
                {
                    UserProfile user = _context.CurrentUser;
                    string comment = UGITUtility.GetVersionString(user.UserName, dependency.Comment, taskRow, DatabaseObjects.Columns.Comment);
                    dependency.Comment = comment;
                    relationship.Comment = dependency.Comment;
                    dependency.CommentHistory = uHelper.GetHistory(taskRow, DatabaseObjects.Columns.Comment, false);
                    relationship.CommentHistory = dependency.CommentHistory;
                }
                if (!string.IsNullOrEmpty(dependency.Comment) && dependency.Comment.StartsWith(";#False<;#>"))
                {
                    var regex = new Regex(Regex.Escape(";#False<;#>"));
                    var replaceEmptyComment = regex.Replace(dependency.Comment, "", 1);
                    dependency.Comment = replaceEmptyComment;
                    relationship.Comment = dependency.Comment;
                }
            }

            relationship.ProposedDate = dependency.ProposedDate;
            if (UGITUtility.IfColumnExists(dtmoduleTasks.NewRow(), DatabaseObjects.Columns.UGITProposedDate))
            {
                if (dependency.ProposedDate == null || dependency.ProposedDate <= DateTime.MinValue)
                    relationship.ProposedDate = new DateTime(1800, 1, 1);
            }

            relationship.StartDate = DateTime.MinValue;
            if (dependency.IsStartDateChange)
            {
                relationship.StartDate = dependency.StartDate;
                if (dependency.StartDate != DateTime.MinValue && (dependency.StartDate == null || dependency.StartDate <= new DateTime(1800, 1, 1)))
                {
                    relationship.StartDate = DateTime.MinValue;
                }
            }

            //relationship.DueDate = DateTime.MinValue;
            relationship.DueDate = dependency.DueDate;
            if (dependency.IsEndDateChange)
            {
                relationship.DueDate = dependency.EndDate.HasValue ? dependency.EndDate.Value : DateTime.MinValue;
                if (dependency.EndDate.HasValue && dependency.EndDate != DateTime.MinValue && (dependency.EndDate == null || dependency.EndDate <= new DateTime(1800, 1, 1)))
                {
                    relationship.DueDate = DateTime.MinValue;
                }
            }

            //relationship.PercentComplete = dependency.PercentComplete / 100;
            relationship.PercentComplete = dependency.PercentComplete;
            relationship.EstimatedHours = dependency.EstimatedHours;
            relationship.Weight = dependency.Weight;
            relationship.ActualHours = dependency.ActualHours;
            relationship.ServiceApplicationAccessXml = dependency.ServiceApplicationAccessXml;
            relationship.Level = dependency.Level;
            relationship.ParentTaskID = dependency.ParentTaskID;
            relationship.ItemOrder = dependency.ItemOrder;

            relationship.CompletionDate = dependency.CompletionDate;
            relationship.CompletedBy = null;

            if (dependency.Status == Constants.Completed || dependency.Status == Constants.Cancelled)
            {
                if (dependency.CompletionDate != DateTime.MinValue && (dependency.CompletionDate == null || dependency.CompletionDate <= new DateTime(1800, 1, 1)))
                    relationship.CompletionDate = DateTime.Now;
                else
                    relationship.CompletionDate = DateTime.Now;

                if (!string.IsNullOrWhiteSpace(dependency.CompletedBy))
                    relationship.CompletedBy = dependency.CompletedBy;
                else
                    relationship.CompletedBy = _appContext.CurrentUser.Id;
            }

            if (dependency.Status == Constants.Completed)
            {
                //Add user inside user list and udpate predecessors 
                if (dependency.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower() && !string.IsNullOrWhiteSpace(dependency.NewUserName) && ID > 0)
                {
                    bool userAdded = string.IsNullOrWhiteSpace(AddNewUser(dependency)) ? false : true;

                    if (userAdded)
                    {
                        List<UGITTask> tasksList = new List<UGITTask>();
                        tasksList = LoadByProjectID(dependency.ParentInstance);
                        tasksList = tasksList.Where(x => x.ID != dependency.ID).ToList();
                        foreach (UGITTask item in tasksList)
                        {
                            if (item.Behaviour == "Task")
                            {
                                item.NewUserName = dependency.NewUserName;
                                item.UGITNewUserDisplayName = dependency.UGITNewUserDisplayName;
                                Save(item, _appContext);
                            }
                            else if (dependency.AutoFillRequestor)
                            {
                                UserProfileManager userProfileManager = new UserProfileManager(_appContext);
                                UserProfile user = userProfileManager.GetUserByUserName(dependency.NewUserName);
                                //SPUser user = UserProfile.GetUserByName(this.UGITNewUserName);
                                TicketManager ticketManager = new TicketManager(_appContext);
                                //GetTableDataManager.GetDataRow()
                                string moduleName = uHelper.getModuleNameByTicketId(item.ChildInstance);
                                DataRow lstItem = Ticket.GetCurrentTicket(_appContext, moduleName, item.ChildInstance);
                                //SPListItem lstItem = uHelper.GetTicket(item.ChildInstance);
                                if (lstItem != null && user != null && uHelper.IfColumnExists(DatabaseObjects.Columns.TicketRequestor, lstItem.Table))
                                {
                                    //SPFieldLookupValue requestor = new SPFieldLookupValue(user.ID, user.LoginName);
                                    if (user != null && string.IsNullOrEmpty(Convert.ToString(lstItem[DatabaseObjects.Columns.TicketRequestor])))
                                    {
                                        lstItem[DatabaseObjects.Columns.TicketRequestor] = user.Id;
                                        //bool allowUnsafeUpdates = spWeb.AllowUnsafeUpdates;
                                        //spWeb.AllowUnsafeUpdates = true;
                                        Save(item);
                                        //spWeb.AllowUnsafeUpdates = allowUnsafeUpdates;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (dependency.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower() && !string.IsNullOrEmpty(dependency.ServiceApplicationAccessXml) && !string.IsNullOrEmpty(Convert.ToString(relationship.Status)) && Convert.ToString(relationship.Status) != Constants.Completed)
                {
                    ApplicationHelperManager appHelperManager = new ApplicationHelperManager(_appContext);
                    appHelperManager.UpdateApplicationSpecificAccess(dependency);
                }
            }

            if (!string.IsNullOrEmpty(dependency.UserFieldXML))
                relationship.UserFieldXML = dependency.UserFieldXML;

            relationship.SubTaskType = dependency.SubTaskType;
            relationship.NewUserName = dependency.NewUserName;
            //relationship[DatabaseObjects.Columns.UGITNewUserDisplayName] = dependency.UGITNewUserDisplayName;
            relationship.Approver = dependency.Approver;
            relationship.Status = dependency.Status;
            relationship.ApprovalStatus = dependency.ApprovalStatus;
            //relationship[DatabaseObjects.Columns.AutoCreateUser] = dependency.AutoCreateUser;
            //relationship[DatabaseObjects.Columns.AutoFillRequestor] = dependency.AutoFillRequestor;
            //relationship[DatabaseObjects.Columns.ApprovalType] = dependency.ApprovalType;
            //relationship[DatabaseObjects.Columns.TaskActionUser] = dependency.TaskActionUser;
            //relationship[DatabaseObjects.Columns.ApprovedBy] = dependency.ApprovedBy;

            // Save attachments

            //if (dependency.AttachedFiles != null)
            //{
            //    List<string> deleteFiles = new List<string>();
            //    List<string> lstOfAttachment = UGITUtility.SplitString(Convert.ToString(relationship[DatabaseObjects.Columns.Attachments]), Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList();
            //    foreach (string attachment in lstOfAttachment)
            //    {
            //        if (dependency.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == attachment.ToLower()).Value == null)
            //            deleteFiles.Add(attachment);
            //    }

            //    foreach (string dFile in deleteFiles)
            //    {
            //        lstOfAttachment.Remove(dFile);
            //    }

            //    relationship[DatabaseObjects.Columns.Attachments] = string.Join(Constants.Separator6, lstOfAttachment);
            //}


            //if (dependency.AttachFiles != null && dependency.AttachFiles.Count > 0)
            //{
            //    foreach (string key in  dependency.AttachFiles.Keys)
            //    {
            //        List<string> lstOfAttac= UGITUtility.SplitString(Convert.ToString(relationship[DatabaseObjects.Columns.Attachments]), Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList();
            //        string existingFile = lstOfAttac.FirstOrDefault(x => x.ToLower() == key.ToLower());
            //        if (string.IsNullOrEmpty(existingFile))
            //            relationship.Attachments.Add(key, AttachFiles[key]);
            //    }
            //}

            relationship.OnHold = UGITUtility.StringToBoolean(dependency.OnHold);
            relationship.OnHoldReasonChoice = dependency.OnHoldReasonChoice;
            relationship.OnHoldStartDate = dependency.OnHoldStartDate.HasValue ? dependency.OnHoldStartDate.Value : DateTime.MinValue;
            if (dependency.OnHoldStartDate != DateTime.MinValue && dependency.OnHoldStartDate <= new DateTime(1800, 1, 1))
                relationship.OnHoldStartDate = DateTime.MinValue;

            relationship.OnHoldTillDate = dependency.OnHoldTillDate.HasValue ? dependency.OnHoldTillDate.Value : DateTime.MinValue;
            if (dependency.OnHoldTillDate != DateTime.MinValue && dependency.OnHoldTillDate <= new DateTime(1800, 1, 1))
                relationship.OnHoldTillDate = DateTime.MinValue;

            relationship.TaskActualStartDate = dependency.TaskActualStartDate.HasValue ? dependency.TaskActualStartDate.Value : DateTime.MinValue;
            if (dependency.TaskActualStartDate != DateTime.MinValue && dependency.TaskActualStartDate <= new DateTime(1800, 1, 1))
                relationship.TaskActualStartDate = DateTime.MinValue;

            relationship.TotalHoldDuration = dependency.TotalHoldDuration;
            relationship.SLADisabled = dependency.SLADisabled;
            relationship.NotificationDisabled = dependency.NotificationDisabled;
            //Save task
            //UGITTask saveTask = LoadItem(relationship, string.Empty);
            Save(relationship);
            dependency.ID = relationship.ID;
            dependency.Created = dependency.Created;// UGITUtility.StringToDateTime(relationship[DatabaseObjects.Columns.Created]);
            dependency.Modified = dependency.Modified; //UGITUtility.StringToDateTime(relationship[DatabaseObjects.Columns.Modified]);

            if (dependency.Status == Constants.Completed && dependency.SubTaskType == ServiceSubTaskType.AccountTask &&
                !string.IsNullOrEmpty(dependency.NewUserName) && !string.IsNullOrEmpty(dependency.UserFieldXML))
            {
                try
                {

                    //UpdateUserItem(oWeb);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "ERROR update user profile properties");
                }


                //UserProfile newUser = UserProfile.LoadUserByNameFromWeb(this.UGITNewUserName, spWeb);
                //UserProfileCache.UpdateUserInCache(newUser);
            }

            //Log event (TicketEvents)
            if (!string.IsNullOrWhiteSpace(dependency.TicketId) && dependency.ModuleNameLookup.Contains("SVC"))
            {
                if (logStatusChangeEvent)
                    UpdateEventLogForTask(_appContext, dependency.Status, dependency.TicketId, Convert.ToString(relationship.Title), Convert.ToString(dependency.ID));

                if (logAssigneeChangeEvent)
                    UpdateEventLogForTask(_appContext, Constants.TicketEventType.Assigned, dependency.ParentInstance, Convert.ToString(relationship.Title), Convert.ToString(dependency.ID), dependency.AssignedTo);

                if (logTicketTaskCreated)
                    UpdateEventLogForTask(_appContext, dependency.Status, dependency.TicketId, dependency.Title, dependency.ChildInstance);
            }

            return true;
        }
        //public bool Save(UGITTask dependency,ApplicationContext _appContext)
        //{
        //    if (dependency == null)
        //        return false;
        //    DataTable dtmoduleTasks= GetTableDataManager.GetTableStructure(DatabaseObjects.Tables.ModuleTasks);
        //    //SPList ticketRelationships = SPListHelper.GetSPList(DatabaseObjects.Lists.TicketRelationship, spWeb);
        //    DataRow relationship = null;
        //    bool logStatusChangeEvent = false;
        //    bool logAssigneeChangeEvent = false;
        //    bool logTicketTaskCreated = false;
        //    long ID = 0;
        //    long.TryParse(Convert.ToString(dependency.ID), out ID);

        //    if (ID > 0)
        //    {
        //        relationship= GetTableDataManager.GetDataRow(DatabaseObjects.Tables.ModuleTasks, DatabaseObjects.Columns.ID, ID).FirstOrDefault();
        //        //relationship = SPListHelper.GetSPListItem(ticketRelationships, ID);
        //        string oldStatus = Convert.ToString(UGITUtility.GetSPItemValue(relationship, DatabaseObjects.Columns.Status));
        //        if (oldStatus == "Waiting" && oldStatus != dependency.Status)
        //            dependency.TaskActualStartDate = DateTime.Now;

        //        if (dependency.Behaviour == "Task")
        //        {
        //            if (!oldStatus.Equals(dependency.Status))
        //                logStatusChangeEvent = true;

        //            logAssigneeChangeEvent = GetAssignToDifference(dependency, relationship);
        //        }
        //    }
        //    else
        //    {
        //        relationship = dtmoduleTasks.NewRow();
        //        if (string.IsNullOrWhiteSpace(dependency.RelatedModule))
        //            logStatusChangeEvent = true;
        //        else
        //            logTicketTaskCreated = true;
        //    }

        //    relationship[DatabaseObjects.Columns.Title] = UGITUtility.TruncateWithEllipsis(UGITUtility.StripHTML(dependency.Title), 255); // Remove invalid HTML and truncate to 255 (SharePoint limit)
        //    if (UGITUtility.IsSPItemExist(relationship, DatabaseObjects.Columns.ErrorMsg))
        //    {
        //        relationship[DatabaseObjects.Columns.ErrorMsg] = dependency.ErrorMsg;
        //        dependency.ErrorMsg = (String)relationship[DatabaseObjects.Columns.ErrorMsg];
        //    }

        //    if (UGITUtility.IfColumnExists(relationship, DatabaseObjects.Columns.ModuleNameLookup))
        //        relationship[DatabaseObjects.Columns.ModuleNameLookup] = dependency.ModuleNameLookup;

        //    if (!string.IsNullOrWhiteSpace(dependency.RelatedModule))
        //    {
        //        relationship[DatabaseObjects.Columns.RelatedModule] = dependency.RelatedModule;
        //        relationship[DatabaseObjects.Columns.TaskBehaviour] = "Ticket";
        //        dependency.Behaviour = "Ticket";
        //    }
        //    else
        //    {
        //        relationship[DatabaseObjects.Columns.RelatedModule] = null;
        //        relationship[DatabaseObjects.Columns.TaskBehaviour] = "Task";
        //        dependency.Behaviour = "Task";
        //    }

        //    relationship[DatabaseObjects.Columns.Predecessors] = null;
        //    if (!string.IsNullOrWhiteSpace(dependency.Predecessors))
        //    {
        //        relationship[DatabaseObjects.Columns.Predecessors] = dependency.Predecessors;
        //    }

        //    relationship[DatabaseObjects.Columns.AssignedTo] = null;
        //    if (!string.IsNullOrWhiteSpace(dependency.AssignedTo))
        //    {
        //        relationship[DatabaseObjects.Columns.AssignedTo] =dependency.AssignedTo;
        //    }

        //    relationship[DatabaseObjects.Columns.Description] = dependency.Description;
        //    relationship[DatabaseObjects.Columns.TicketId] = dependency.TicketId;
        //    relationship[DatabaseObjects.Columns.RelatedTicketID] = dependency.RelatedTicketID;

        //    if (UGITUtility.IfColumnExists(relationship, DatabaseObjects.Columns.Comment))
        //    {
        //        if (dependency.Comment != null && dependency.Comment.Trim() != string.Empty && (!dependency.Comment.StartsWith(";#False<;#>")))
        //        {
        //            UserProfile user = _context.CurrentUser;
        //            string comment = UGITUtility.GetVersionString(user.UserName, dependency.Comment, relationship, DatabaseObjects.Columns.Comment);
        //            dependency.Comment = comment;
        //            dependency.CommentHistory = uHelper.GetHistory(relationship, DatabaseObjects.Columns.Comment, false);
        //            //task.Comment = string.Empty;
        //        }
        //        if (!string.IsNullOrEmpty(dependency.Comment) && dependency.Comment.StartsWith(";#False<;#>"))
        //        {
        //            var regex = new Regex(Regex.Escape(";#False<;#>"));
        //            var replaceEmptyComment = regex.Replace(dependency.Comment, "", 1);
        //            dependency.Comment = replaceEmptyComment;
        //        }
        //    }

        //    relationship[DatabaseObjects.Columns.UGITProposedDate] = dependency.ProposedDate;
        //    if (UGITUtility.IfColumnExists(relationship, DatabaseObjects.Columns.UGITProposedDate))
        //    {
        //        if (dependency.ProposedDate==null || dependency.ProposedDate <= DateTime.MinValue)
        //            relationship[DatabaseObjects.Columns.UGITProposedDate] = new DateTime(1800, 1, 1);
        //    }

        //    relationship[DatabaseObjects.Columns.StartDate] = DateTime.MinValue;
        //    if (dependency.IsStartDateChange)
        //    {
        //        relationship[DatabaseObjects.Columns.StartDate] = dependency.StartDate;
        //        if (dependency.StartDate!= DateTime.MinValue && (dependency.StartDate==null || dependency.StartDate <= new DateTime(1800, 1, 1)))
        //        {
        //            relationship[DatabaseObjects.Columns.StartDate] = DateTime.MinValue;
        //        }
        //    }

        //    relationship[DatabaseObjects.Columns.DueDate] = DateTime.MinValue;
        //    if (dependency.IsEndDateChange)
        //    {
        //        relationship[DatabaseObjects.Columns.DueDate] = dependency.EndDate.HasValue?dependency.EndDate.Value:DateTime.MinValue;
        //        if (dependency.EndDate.HasValue && dependency.EndDate != DateTime.MinValue && (dependency.EndDate==null || dependency.DueDate <= new DateTime(1800, 1, 1)))
        //        {
        //            relationship[DatabaseObjects.Columns.DueDate] = DateTime.MinValue;
        //        }
        //    }

        //    relationship[DatabaseObjects.Columns.PercentComplete] = dependency.PercentComplete / 100;
        //    relationship[DatabaseObjects.Columns.TaskEstimatedHours] = dependency.EstimatedHours;
        //    relationship[DatabaseObjects.Columns.Weight] = dependency.Weight;
        //    relationship[DatabaseObjects.Columns.TaskActualHours] = dependency.ActualHours;
        //    relationship[DatabaseObjects.Columns.ServiceApplicationAccessXml] =dependency.ServiceApplicationAccessXml;
        //    relationship[DatabaseObjects.Columns.UGITLevel] = dependency.Level;
        //    relationship[DatabaseObjects.Columns.ParentTaskID] = dependency.ParentTaskID;
        //    relationship[DatabaseObjects.Columns.ItemOrder] = dependency.ItemOrder;

        //    relationship[DatabaseObjects.Columns.CompletionDate] = dependency.CompletionDate;
        //    relationship[DatabaseObjects.Columns.CompletedBy] = null;

        //    if (dependency.Status == Constants.Completed || dependency.Status == Constants.Cancelled)
        //    {
        //        if (dependency.CompletionDate != DateTime.MinValue && (dependency.CompletionDate==null || dependency.CompletionDate <= new DateTime(1800, 1, 1)))
        //            relationship[DatabaseObjects.Columns.CompletionDate] =DateTime.Now;
        //        else
        //            relationship[DatabaseObjects.Columns.CompletionDate] = DateTime.Now;

        //        if (!string.IsNullOrWhiteSpace(dependency.CompletedBy))
        //            relationship[DatabaseObjects.Columns.CompletedBy] =dependency.CompletedBy;
        //        else
        //            relationship[DatabaseObjects.Columns.CompletedBy] = _appContext.CurrentUser.Id;
        //    }

        //    if (dependency.Status == Constants.Completed)
        //    {
        //        //Add user inside user list and udpate predecessors 
        //        if (dependency.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower() && !string.IsNullOrWhiteSpace(dependency.NewUserName) && ID > 0)
        //        {
        //            bool userAdded = string.IsNullOrWhiteSpace(AddNewUser(dependency)) ? false : true;

        //            if (userAdded)
        //            {
        //                List<UGITTask> tasksList = new List<UGITTask>();
        //                tasksList = LoadByProjectID(dependency.ParentInstance);
        //                tasksList = tasksList.Where(x => x.ID != dependency.ID).ToList();
        //                foreach (UGITTask item in tasksList)
        //                {
        //                    if (item.Behaviour == "Task")
        //                    {
        //                        item.NewUserName = dependency.NewUserName;
        //                        item.UGITNewUserDisplayName = dependency.UGITNewUserDisplayName;
        //                        Save(item, _appContext);
        //                    }
        //                    else if (dependency.AutoFillRequestor)
        //                    {
        //                        UserProfileManager userProfileManager = new UserProfileManager(_appContext);
        //                        UserProfile user = userProfileManager.GetUserByUserName(dependency.NewUserName);
        //                        //SPUser user = UserProfile.GetUserByName(this.UGITNewUserName);
        //                        TicketManager ticketManager = new TicketManager(_appContext);
        //                        //GetTableDataManager.GetDataRow()
        //                        string moduleName = uHelper.getModuleNameByTicketId(item.ChildInstance);
        //                        DataRow lstItem = Ticket.GetCurrentTicket(_appContext, moduleName, item.ChildInstance);
        //                        //SPListItem lstItem = uHelper.GetTicket(item.ChildInstance);
        //                        if (lstItem != null && user != null && uHelper.IfColumnExists(DatabaseObjects.Columns.TicketRequestor, lstItem.Table))
        //                        {
        //                            //SPFieldLookupValue requestor = new SPFieldLookupValue(user.ID, user.LoginName);
        //                            if (user != null && string.IsNullOrEmpty(Convert.ToString(lstItem[DatabaseObjects.Columns.TicketRequestor])))
        //                            {
        //                                lstItem[DatabaseObjects.Columns.TicketRequestor] = user.Id;
        //                                //bool allowUnsafeUpdates = spWeb.AllowUnsafeUpdates;
        //                                //spWeb.AllowUnsafeUpdates = true;
        //                                Save(item);
        //                                //spWeb.AllowUnsafeUpdates = allowUnsafeUpdates;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if (dependency.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower() && !string.IsNullOrEmpty(dependency.ServiceApplicationAccessXml) && !string.IsNullOrEmpty(Convert.ToString(relationship[DatabaseObjects.Columns.Status])) && Convert.ToString(relationship[DatabaseObjects.Columns.Status]) != Constants.Completed)
        //        {
        //            ApplicationHelperManager appHelperManager = new ApplicationHelperManager(_appContext);
        //            appHelperManager.UpdateApplicationSpecificAccess(dependency);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(dependency.UserFieldXML))
        //        relationship[DatabaseObjects.Columns.UserFieldXML] = dependency.UserFieldXML;

        //    relationship[DatabaseObjects.Columns.UGITSubTaskType] = dependency.SubTaskType;
        //    relationship[DatabaseObjects.Columns.UGITNewUserName] = dependency.NewUserName;
        //    //relationship[DatabaseObjects.Columns.UGITNewUserDisplayName] = dependency.UGITNewUserDisplayName;
        //    relationship[DatabaseObjects.Columns.Approver] = dependency.Approver;
        //    relationship[DatabaseObjects.Columns.Status] = dependency.Status;
        //    relationship[DatabaseObjects.Columns.Approvalstatus] = dependency.ApprovalStatus;
        //    //relationship[DatabaseObjects.Columns.AutoCreateUser] = dependency.AutoCreateUser;
        //    //relationship[DatabaseObjects.Columns.AutoFillRequestor] = dependency.AutoFillRequestor;
        //    //relationship[DatabaseObjects.Columns.ApprovalType] = dependency.ApprovalType;
        //    //relationship[DatabaseObjects.Columns.TaskActionUser] = dependency.TaskActionUser;
        //    //relationship[DatabaseObjects.Columns.ApprovedBy] = dependency.ApprovedBy;

        //    // Save attachments

        //    if (dependency.AttachedFiles != null && relationship[DatabaseObjects.Columns.Attachments]!=null)
        //    {
        //        List<string> deleteFiles = new List<string>();
        //        List<string> lstOfAttachment=  UGITUtility.SplitString(Convert.ToString(relationship[DatabaseObjects.Columns.Attachments]), Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        foreach (string attachment in lstOfAttachment)
        //        {
        //            if (dependency.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == attachment.ToLower()).Value == null)
        //                deleteFiles.Add(attachment);
        //        }

        //        foreach (string dFile in deleteFiles)
        //        {
        //            lstOfAttachment.Remove(dFile);
        //        }

        //        relationship[DatabaseObjects.Columns.Attachments] = string.Join(Constants.Separator6, lstOfAttachment);
        //    }


        //    //if (dependency.AttachFiles != null && dependency.AttachFiles.Count > 0)
        //    //{
        //    //    foreach (string key in  dependency.AttachFiles.Keys)
        //    //    {
        //    //        List<string> lstOfAttac= UGITUtility.SplitString(Convert.ToString(relationship[DatabaseObjects.Columns.Attachments]), Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    //        string existingFile = lstOfAttac.FirstOrDefault(x => x.ToLower() == key.ToLower());
        //    //        if (string.IsNullOrEmpty(existingFile))
        //    //            relationship.Attachments.Add(key, AttachFiles[key]);
        //    //    }
        //    //}

        //    relationship[DatabaseObjects.Columns.TicketOnHold] = dependency.OnHold ? 1 : 0;
        //    relationship[DatabaseObjects.Columns.OnHoldReason] = dependency.OnHoldReasonChoice;
        //    relationship[DatabaseObjects.Columns.TicketOnHoldStartDate] = dependency.OnHoldStartDate.HasValue?dependency.OnHoldStartDate.Value:DateTime.MinValue;
        //    if (dependency.OnHoldStartDate != DateTime.MinValue && dependency.OnHoldStartDate <= new DateTime(1800, 1, 1))
        //        relationship[DatabaseObjects.Columns.TicketOnHoldStartDate] = DateTime.MinValue;

        //    relationship[DatabaseObjects.Columns.TicketOnHoldTillDate] = dependency.OnHoldTillDate.HasValue ? dependency.OnHoldTillDate.Value : DateTime.MinValue;
        //    if (dependency.OnHoldTillDate != DateTime.MinValue && dependency.OnHoldTillDate <= new DateTime(1800, 1, 1))
        //        relationship[DatabaseObjects.Columns.TicketOnHoldTillDate] = DateTime.MinValue;

        //    relationship[DatabaseObjects.Columns.TaskActualStartDate] = dependency.TaskActualStartDate.HasValue ? dependency.TaskActualStartDate.Value : DateTime.MinValue;
        //    if (dependency.TaskActualStartDate != DateTime.MinValue && dependency.TaskActualStartDate <= new DateTime(1800, 1, 1))
        //        relationship[DatabaseObjects.Columns.TaskActualStartDate] = DateTime.MinValue;

        //    relationship[DatabaseObjects.Columns.TicketTotalHoldDuration] = dependency.TotalHoldDuration;
        //    relationship[DatabaseObjects.Columns.SLADisabled] = dependency.SLADisabled;
        //    relationship[DatabaseObjects.Columns.NotificationDisabled] = dependency.NotificationDisabled;
        //    //Save task
        //    UGITTask saveTask = LoadItem(relationship, string.Empty);
        //    Save(saveTask);
        //    dependency.ID = saveTask.ID;
        //    dependency.Created = UGITUtility.StringToDateTime(relationship[DatabaseObjects.Columns.Created]);
        //    dependency.Modified = UGITUtility.StringToDateTime(relationship[DatabaseObjects.Columns.Modified]);

        //    if (dependency.Status == Constants.Completed && dependency.SubTaskType == ServiceSubTaskType.AccountTask &&
        //        !string.IsNullOrEmpty(dependency.NewUserName) && !string.IsNullOrEmpty(dependency.UserFieldXML))
        //    {
        //        try
        //        {

        //            //UpdateUserItem(oWeb);
        //        }
        //        catch (Exception ex)
        //        {
        //            ULog.WriteException(ex, "ERROR update user profile properties");
        //        }


        //        //UserProfile newUser = UserProfile.LoadUserByNameFromWeb(this.UGITNewUserName, spWeb);
        //        //UserProfileCache.UpdateUserInCache(newUser);
        //    }

        //    //Log event (TicketEvents)
        //    if (!string.IsNullOrWhiteSpace(dependency.ParentInstance) && dependency.ParentInstance.Contains("SVC"))
        //    {
        //        if (logStatusChangeEvent)
        //            UpdateEventLogForTask(_appContext, dependency.Status, dependency.ParentInstance, Convert.ToString(relationship[DatabaseObjects.Columns.Title]), Convert.ToString(dependency.ID));

        //        if (logAssigneeChangeEvent)
        //            UpdateEventLogForTask(_appContext, Constants.TicketEventType.Assigned, dependency.ParentInstance, Convert.ToString(relationship[DatabaseObjects.Columns.Title]), Convert.ToString(dependency.ID), dependency.AssignedTo);

        //        if (logTicketTaskCreated)
        //            UpdateEventLogForTask(_appContext, dependency.Status, dependency.ParentInstance, dependency.Title, dependency.ChildInstance);
        //    }

        //    return true;
        //} 
        public List<UGITTask> GetCriticalPathTask(List<UGITTask> tasks)
        {
            #region Old Calculation
            //List<UGITTask> criticalPathTasks = new List<UGITTask>();
            //var firsttask = tasks.FirstOrDefault();
            //criticalPathTasks.Add(firsttask);
            //var lasttask = tasks.LastOrDefault();
            //criticalPathTasks.AddRange(tasks.Where(x => x.ID != lasttask.ID && x.ID != firsttask.ID).ToList());
            //criticalPathTasks.Add(lasttask);

            //var notvalidCriticalTask = new List<UGITTask>();
            //foreach (var ctask in tasks)
            //{
            //    if (ctask.ParentTaskID == 0)
            //    {
            //        if (ctask.ChildCount > 0)
            //        {
            //            notvalidCriticalTask.Add(ctask);
            //            continue;
            //        }
            //    }

            //    if (ctask.PredecessorTasks != null)
            //    {
            //        var predTasks = ctask.PredecessorTasks;
            //        foreach (var predtask in predTasks)
            //        {
            //            string nextworkingDatenTime = uHelper.GetNextWorkingDateAndTime(ctask.DueDate, SPContext.Current.Web);
            //            string[] nextworkingStartNEndTime = uHelper.SplitString(nextworkingDatenTime, Constants.Separator);
            //            DateTime nextStartTime = Convert.ToDateTime(nextworkingStartNEndTime[0]);

            //            TimeSpan tsdiff = predtask.StartDate.Subtract(nextStartTime);
            //            if (tsdiff.Days > 0)
            //            {
            //                notvalidCriticalTask.Add(ctask);
            //            }
            //        }
            //    }
            //}

            //criticalPathTasks = criticalPathTasks.Except(notvalidCriticalTask).ToList();

            //var tasknopredecessors = tasks.Where(x => (x.PredecessorTasks == null || x.PredecessorTasks.Count == 0) &&
            //                                   (x.SuccessorTasks == null || x.SuccessorTasks.Count == 0) &&
            //                                   x.ID != firsttask.ID && x.ID != lasttask.ID).ToList();

            //criticalPathTasks = criticalPathTasks.Except(tasknopredecessors).ToList();
            #endregion

            #region Critical Path Task calculation based on estimated hours of each task

            List<UGITTask> roottasks = tasks.OrderBy(x => x.StartDate).Where(x => (x.PredecessorTasks == null || x.PredecessorTasks.Count == 0) && (x.ParentTaskID == 0)).ToList();
            List<Tuple<List<UGITTask>>> allPath = new List<Tuple<List<UGITTask>>>();
            foreach (UGITTask roottask in roottasks)
            {
                Tuple<List<UGITTask>> path = new Tuple<List<UGITTask>>(new List<UGITTask>());
                path.Item1.Add(roottask);
                allPath.Add(path);
                if (roottask.SuccessorTasks != null && roottask.SuccessorTasks.Count > 0)
                {
                    CalculateCriticalPath(allPath, path, roottask.SuccessorTasks);
                }
            }

            var data = tasks.Sum(x => x.EstimatedHours) > 0 ? (from a in allPath select new { tasks = a.Item1, sum = a.Item1.Sum(x => x.EstimatedHours) }) : (from a in allPath select new { tasks = a.Item1, sum = a.Item1.Sum(x => x.Duration) });
            List<UGITTask> criticalPathTasks = new List<UGITTask>();
            if (data != null)
            {
                var criticalPath = data.OrderByDescending(x => x.sum).FirstOrDefault();
                if (criticalPath != null)
                    criticalPathTasks = criticalPath.tasks;
            }
            #endregion

            return criticalPathTasks;
        }

        void CalculateCriticalPath(List<Tuple<List<UGITTask>>> allPath, Tuple<List<UGITTask>> path, List<UGITTask> successorTasks)
        {
            if (successorTasks != null)
            {
                List<UGITTask> pathTasks = new List<UGITTask>();
                pathTasks.AddRange(path.Item1);
                for (int i = 0; i < successorTasks.Count; i++)
                {
                    UGITTask task = successorTasks[i];
                    Tuple<List<UGITTask>> sPath = null;
                    if (i == 0)
                    {
                        sPath = path;
                        sPath.Item1.Add(task);
                    }
                    else
                    {
                        sPath = new Tuple<List<UGITTask>>(new List<UGITTask>());
                        sPath.Item1.AddRange(pathTasks);
                        sPath.Item1.Add(task);
                        allPath.Add(sPath);
                    }
                    CalculateCriticalPath(allPath, sPath, task.SuccessorTasks);
                }
            }
        }

        public void CopyTask(int taskid, string ticketPublicId, bool copyChild, string moduleName)
        {
            var pTasks = LoadByProjectID(moduleName, ticketPublicId);
            pTasks = MapRelationalObjects(pTasks);

            UGITTask ptask = pTasks.FirstOrDefault(x => x.ID == taskid);
            CopyTasks(ptask, pTasks, ptask.ParentTaskID, copyChild, moduleName, true, ticketPublicId);

            pTasks = MapRelationalObjects(pTasks);
            ReOrderTasks(ref pTasks);

            //UGITTaskHelper.ReloadProjectTasks(moduleName, ticketPublicId);
        }

        public void CopyTasks(UGITTask ctask, List<UGITTask> pTasks, long pid, bool copyChild, string moduleName, bool isfirst, string ticketPublicId)
        {
            UGITTask t = CopyTo(ctask, pTasks, pid, ticketPublicId);
            pTasks.Add(t);

            if (isfirst)
            {
                PlacedTaskAt(t, ctask, ref pTasks);
            }
            else
            {
                UGITTask ptask = pTasks.FirstOrDefault(x => x.ID == pid);
                PlacedTaskAt(t, ptask, ref pTasks);
            }

            pTasks = MapRelationalObjects(pTasks);
            ReOrderTasks(ref pTasks);
            SaveTasks(ref pTasks, moduleName, ticketPublicId);

            if (copyChild)
            {
                if (ctask.ChildCount > 0 && ctask.ChildTasks != null)
                {
                    foreach (var childtask in ctask.ChildTasks.Reverse<UGITTask>())
                    {
                        CopyTasks(childtask, pTasks, t.ID, copyChild, moduleName, !isfirst, ticketPublicId);
                    }
                }
            }
        }

        private static string GetTaskList(string moduleName)
        {
            switch (moduleName)
            {
                case ModuleNames.PMM:
                    return DatabaseObjects.Tables.PMMTasks;
                case ModuleNames.NPR:
                    return DatabaseObjects.Tables.NPRTasks;
                case ModuleNames.TSK:
                    return DatabaseObjects.Tables.TSKTasks;
                default:
                    return DatabaseObjects.Tables.ModuleTasks;
            }
        }

        private static string GetProjectLookupFieldName(string moduleName)
        {
            switch (moduleName)
            {
                case ModuleNames.PMM:
                    return DatabaseObjects.Columns.TicketPMMIdLookup;
                case ModuleNames.NPR:
                    return DatabaseObjects.Columns.TicketNPRIdLookup;
                case ModuleNames.TSK:
                    return DatabaseObjects.Columns.TSKIDLookup;
                default:
                    return DatabaseObjects.Columns.TicketId;
            }
        }

        private static string GetModuleName(string taskList)
        {
            switch (taskList)
            {
                case "PMMTasks":
                    return ModuleNames.PMM;
                case "NPRTasks":
                    return ModuleNames.NPR;
                case "TSKTasks":
                    return ModuleNames.TSK;
                default:
                    return string.Empty;
            }
        }

        public void IncIndent(string TicketPublicId, int tskTaskID)
        {
            string moduleName = uHelper.getModuleNameByTicketId(TicketPublicId);
            List<UGITTask> tasks = LoadByProjectID(moduleName, TicketPublicId);
            tasks = MapRelationalObjects(tasks);
            //Negative TaskId means that this task is newly added and we have received its itemOrder/position.
            if (tskTaskID < 0 && tasks.Count + 1 >= -tskTaskID) 
            { 
                tskTaskID = -tskTaskID;
                //Pick the taskId of newly added task using its itemOrder.
                tskTaskID = ((int)tasks[tskTaskID - 1].ID);
            }
            UGITTask childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);

            //remove predecessors of of task which trigger indent
            childTask.Predecessors = null;

            int childIndex = tasks.IndexOf(childTask);
            UGITTask parentTask = null;
            for (int i = childIndex - 1; i >= 0; i--)
            {
                // return because not child task exits and parent  id exits task cannot be further indented
                if (childTask.ParentTaskID == tasks[i].ID)
                    return;
                if (tasks[i].Level == childTask.Level)
                {
                    parentTask = tasks[i];
                    break;
                }
                else if (tasks[i].Level < childTask.Level)
                {
                    parentTask = null;
                    break;
                }
            }

            long parentTaskID = 0;
            if (parentTask != null)
            {
                parentTaskID = parentTask.ID;
            }

            AddChildTask(parentTaskID, tskTaskID, ref tasks);
            ReOrderTasks(ref tasks);
            PropagateTaskEffect(ref tasks, childTask);

            childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);
            //Not task is move from root then dettach it from milestone.
            if (childTask.ParentTaskID > 0)
            {
                childTask.IsMileStone = false;
                childTask.StageStep = 0;
            }
            SaveTasks(ref tasks, moduleName, TicketPublicId);

            //Calculate project related values
            DataRow ticketItem = Ticket.GetCurrentTicket(_context, moduleName, TicketPublicId);
            CalculateProjectStartEndDate(moduleName, tasks, ticketItem);
        }

        public void DecIndent(string TicketPublicId, int tskTaskID)
        {
            string moduleName = uHelper.getModuleNameByTicketId(TicketPublicId);
            List<UGITTask> tasks = LoadByProjectID(moduleName, TicketPublicId);
            tasks = MapRelationalObjects(tasks);

            UGITTask childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);


            if (childTask.Level > 0)
            {
                //remove predecessors of of task which trigger indent
                childTask.Predecessors = null;

                int childIndex = tasks.IndexOf(childTask);
                UGITTask parentTask = null;
                for (int i = childIndex - 1; i >= 0; i--)
                {
                    if (tasks[i].Level == childTask.Level - 1)
                    {
                        parentTask = tasks[i];
                        break;
                    }
                }

                if (parentTask != null)
                {
                    RemoveChildTask(parentTask.ID, tskTaskID, ref tasks);
                    ReOrderTasks(ref tasks);
                    PropagateTaskEffect(ref tasks, childTask);

                    childTask = tasks.FirstOrDefault(x => x.ID == tskTaskID);
                    //Not task is move from root then dettach it from milestone.
                    if (childTask.ParentTaskID > 0)
                    {
                        childTask.IsMileStone = false;
                        childTask.StageStep = 0;
                    }

                    SaveTasks(ref tasks, moduleName, TicketPublicId);

                    //Calculate project related values
                    DataRow ticketItem = Ticket.GetCurrentTicket(_context, moduleName, TicketPublicId);
                    CalculateProjectStartEndDate(moduleName, tasks, ticketItem);
                }
            }
        }

        public void UpdateTaskListItemOrder(string TicketPublicId, string dragRow, string targetRow, string firstAddInCategory = null)
        {
            if (string.IsNullOrEmpty(dragRow))
                return;

            List<long> selectedIds = new List<long>();
            selectedIds.Add(UGITUtility.StringToInt(dragRow));

            string moduleName = uHelper.getModuleNameByTicketId(TicketPublicId);
            List<UGITTask> tasks = LoadByProjectID(moduleName, TicketPublicId);
            tasks = MapRelationalObjects(tasks);

            int dragrowId = Convert.ToInt32(dragRow);
            int targetRowId = Convert.ToInt32(targetRow);
            List<UGITTask> dragTasks = tasks.Where(x => selectedIds.Contains(x.ID)).ToList();
            UGITTask targetTasks = tasks.Where(x => x.ID == targetRowId).FirstOrDefault();

            if (dragTasks == null || dragTasks.Count == 0)
                return;

            dragTasks = dragTasks.OrderBy(x => x.ItemOrder).ToList();

            int indexCounter = 0;

            foreach (UGITTask cTask in dragTasks)
            {
                UGITTask dragTaskSiblingTask = null;
                if (cTask != null && cTask.ParentTask != null && cTask.ParentTask.ChildTasks.Count > 0)
                    dragTaskSiblingTask = cTask.ParentTask.ChildTasks.FirstOrDefault(x => x.ID != cTask.ID);

                if (!string.IsNullOrEmpty(firstAddInCategory))
                {
                    AddChildTask(targetTasks.ID, cTask.ID, ref tasks);
                }
                else
                {
                    AddChildTask(targetTasks.ParentTaskID, cTask.ID, ref tasks);
                    List<UGITTask> temptaskList = null;
                    if (targetTasks.ParentTaskID > 0)
                    {
                        var childIndex = targetTasks.ParentTask.ChildTasks.IndexOf(cTask);
                        int childtargetIndex;
                        {
                            childtargetIndex = targetTasks.ParentTask.ChildTasks.IndexOf(targetTasks);
                        }
                        targetTasks.ParentTask.ChildTasks.RemoveAt(childIndex);
                        targetTasks.ParentTask.ChildTasks.Insert(childtargetIndex, cTask);
                        temptaskList = targetTasks.ParentTask.ChildTasks;
                    }
                    else
                    {
                        temptaskList = tasks.Where(x => x.ParentTaskID == 0).ToList();
                        temptaskList.Remove(cTask);
                        {
                            temptaskList.Insert(temptaskList.IndexOf(targetTasks), cTask);
                        }

                    }

                    for (int i = 0; i < temptaskList.Count; i++)
                    {
                        temptaskList[i].ItemOrder = i + 1;
                    }

                }





                ReOrderTasks(ref tasks);

                //call propogation when dragtask is moving into other summary task
                PropagateTaskEffect(_context, ref tasks, cTask, true, true);

                if (dragTaskSiblingTask != null)
                    PropagateTaskEffect(_context, ref tasks, dragTaskSiblingTask, true, true);

                indexCounter++;
            }

            SaveTasks(ref tasks, moduleName, TicketPublicId);
            DataRow ticketItem = Ticket.GetCurrentTicket(_context, moduleName, TicketPublicId);
            CalculateProjectStartEndDate(moduleName, tasks, ticketItem);
            Ticket ticket = new Ticket(_context, moduleName);
            ticket.CommitChanges(ticketItem);
        }

        public UGITTask GetTaskById(string id)
        {
            UGITTask task;

            if (string.IsNullOrEmpty(id))
                return null;

            task = store.Load(x => x.ID == Convert.ToInt32(id)).FirstOrDefault();

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref task);
            return task;
        }

        public static void GetDependentTasks(UGITTask currentTask, ref List<UGITTask> dependentTasks, bool skipChildren)
        {
            if (currentTask == null)
                return;

            if (dependentTasks.Contains(currentTask))
                return;

            dependentTasks.Add(currentTask);

            if (currentTask.ParentTask != null)
            {
                GetDependentTasks(currentTask.ParentTask, ref dependentTasks, true);
            }

            if (!skipChildren && currentTask.ChildCount > 0)
            {
                foreach (UGITTask tTask in currentTask.ChildTasks)
                {
                    GetDependentTasks(tTask, ref dependentTasks, false);
                }
            }

            if (currentTask.SuccessorTasks != null)
            {
                foreach (UGITTask tTask in currentTask.SuccessorTasks)
                {
                    GetDependentTasks(tTask, ref dependentTasks, false);
                }
            }
        }

        public void ReOrderTasks(ref List<UGITTask> tasks)
        {
            tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            List<UGITTask> rootTasks = tasks.Where(x => x.ParentTaskID == 0).ToList();
            int start = 1;
            if (rootTasks != null && rootTasks.Count > 0)
            {
                foreach (UGITTask tTask in rootTasks)
                {
                    tTask.ItemOrder = start;
                    tTask.Changes = true;
                    start += 1;
                    tTask.Level = 0;
                    ReOrderTasks(tTask, ref start, tTask.Level);
                }
            }
            UpdatePredecessors(ref tasks);
        }
        //BTS-22-000819:Code added to update the Predecessor numbers of existing tasks in case of any insertion or deletion of tasks.
        public void UpdatePredecessors(ref List<UGITTask> tasks)
        {
            tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            foreach (UGITTask task in tasks)
            {
                if (task.PredecessorTasks != null)
                task.Predecessors = string.Join(Constants.Separator6 + Constants.SpaceSeparator, task.PredecessorTasks.Select(x => x.ItemOrder.ToString()));
            }

        }

        private static void ReOrderTasks(UGITTask task, ref int start, int parentLevel)
        {
            if (task.ChildCount > 0 && task.ChildTasks.Count > 0)
            {
                task.ChildTasks = task.ChildTasks.OrderBy(x => x.ItemOrder).ToList();
                foreach (UGITTask tTask in task.ChildTasks)
                {
                    tTask.ItemOrder = start;
                    tTask.Changes = true;
                    start += 1;
                    tTask.Level = parentLevel + 1;
                    ReOrderTasks(tTask, ref start, tTask.Level);
                }
            }
        }

        /// <summary>
        /// Links reference task object based reference values
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static List<UGITTask> MapRelationalObjects(List<UGITTask> tasks)
        {
            if (tasks == null)
                return null;

            UGITTask tempTask = null;
            foreach (UGITTask task in tasks)
            {
                //Attaches ParentTask Object based of parenttaskid
                if (task.ParentTaskID > 0)
                {
                    tempTask = tasks.FirstOrDefault(x => x.ID == task.ParentTaskID);
                    if (tempTask != null)
                    {
                        task.ParentTask = tempTask;
                        tempTask = null;
                    }
                }

                string[] predecessorsList = UGITUtility.SplitString(task.Predecessors, Constants.Separator6);
                int predecessorsCount = predecessorsList.Count();
                //Attaches Predecessors task objects based of Predecessors collection
                if (task.Predecessors != null && predecessorsCount > 0)
                {
                    task.PredecessorTasks = new List<UGITTask>();
                    foreach (string lookup in predecessorsList)
                    {
                        tempTask = tasks.FirstOrDefault(x => x.ItemOrder == UGITUtility.StringToInt(lookup));//BTS-22-000819
                        if (tempTask != null)
                        {
                            task.PredecessorTasks.Add(tempTask);
                            tempTask = null;
                        }
                    }
                }

                //Attaches sucessors tasks of current task
                List<string> xPredecessors = new List<string>(predecessorsList);
                task.SuccessorTasks = tasks.Where(x => x.Predecessors != null && UGITUtility.ConvertStringToList(x.Predecessors, Constants.Separator6).Exists(y => y == task.ItemOrder.ToString())).ToList();//BTS-22-000819

                //Attaches Child task objects if child count is more then 0
                task.ChildTasks = new List<UGITTask>();
                if (task.ID > 0)
                    task.ChildTasks = tasks.Where(x => x.ParentTaskID == task.ID).ToList();
                if (task.ChildTasks != null)
                    task.ChildCount = task.ChildTasks.Count;

            }

            foreach (UGITTask task in tasks)
            {

                if (task.SuccessorTasks != null && task.SuccessorTasks.Count > 0)
                {
                    Action<UGITTask> recursive = null;
                    List<UGITTask> successorchildtasks = new List<UGITTask>();
                    foreach (var item in task.SuccessorTasks)
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
                    task.SuccessorTasks.AddRange(successorchildtasks);
                }
            }
            return tasks;
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
                    if (task.ParentTaskID > 0)
                    {
                        var parenttask = tasks.FirstOrDefault(z => z.ID == task.ParentTaskID);
                        if (parenttask != null)
                        {
                            parenttask.ChildTasks = tasks.FindAll(y => y.ParentTaskID == task.ParentTaskID);
                            parenttask.ChildCount = tasks.FindAll(y => y.ParentTaskID == task.ParentTaskID).Count;
                        }
                        task.ParentTask = tasks.FirstOrDefault(z => z.ID == task.ParentTaskID);
                        task.SuccessorTasks = tasks.Where(z => z.Predecessors != null && z.Predecessors.Split(',').Contains(Convert.ToString(task.ID))).ToList();

                        //Attaches Child task objects if child count is more then 0
                        task.ChildTasks = new List<UGITTask>();
                        if (task.ID > 0)
                            task.ChildTasks = tasks.Where(y => y.ParentTaskID == task.ID).ToList();
                        if (task.ChildTasks != null)
                            task.ChildCount = task.ChildTasks.Count;


                        if (task.SuccessorTasks != null && task.SuccessorTasks.Count > 0)
                        {
                            Action<UGITTask> recursive = null;
                            List<UGITTask> successorchildtasks = new List<UGITTask>();
                            foreach (var item in task.SuccessorTasks)
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
                            task.SuccessorTasks.AddRange(successorchildtasks);
                        }
                    }
                });

            return tasks;
        }

        /// <summary>
        /// Delete Allocations if User is removed from Task.
        /// </summary>
        public void DeleteAllocationOnTaskSave(string assignedTo, string workItemType, string workItem)
        {
            ResourceAllocationManager resourceAllocManager = new ResourceAllocationManager(_context);
            bool autoCreateRMMProjectAllocation = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
            if (autoCreateRMMProjectAllocation)
            {
                if (!string.IsNullOrEmpty(assignedTo))
                {
                    List<string> users = new List<string>();//.Select(x => x.LookupId).ToList();

                    if (assignedTo.Contains(Constants.Separator6))
                        users = assignedTo.Split(',').ToList();
                    else if (assignedTo.Contains(Constants.Separator))
                        users = assignedTo.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    else
                        users.Add(assignedTo);

                    ThreadStart tsDeleteProjectPlannedAllocation = delegate () { resourceAllocManager.DeletePlannedAllocationUnAssignedUser(string.Empty, users, workItemType, workItem, true); };
                    Thread thDeleteProjectPlannedAllocation = new Thread(tsDeleteProjectPlannedAllocation);
                    thDeleteProjectPlannedAllocation.Start();
                }

            }
        }

        private List<string> LoadFieldListFromDB(string moduleName)
        {
            List<string> fields = new List<string>();
            fields.Add(string.Format("<FieldRef Name='{0}' />", DatabaseObjects.Columns.Id));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Title));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Body));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.DueDate));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.StartDate));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.ParentTask));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Predecessors));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Status));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.PercentComplete));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE' Type='User' Mult='TRUE' />", DatabaseObjects.Columns.AssignedTo));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.ItemOrder));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.TaskEstimatedHours));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.TaskActualHours));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITChildCount));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITLevel));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITContribution));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITDuration));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.TaskBehaviour));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.IsCritical));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITAssignToPct));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.CompletionDate));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.CompletedBy));

            if (moduleName != "NPR")
            {
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITComment));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITProposedDate));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITProposedStatus));
            }

            if (moduleName == "PMM")
            {
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.IsMilestone));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.StageStep));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.ShowOnProjectCalendar));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.LinkedDocuments));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.TicketId));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.SprintLookup));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UGITComment));
            }

            if (moduleName == "PMM" || moduleName == "TSK" || IsModuleTasks(moduleName))
            {
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.EstimatedRemainingHours));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.TaskReminderDays));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.TaskReminderEnabled));
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.TaskRepeatInterval));
            }

            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", GetProjectLookupFieldName(moduleName)));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Attachments));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Editor));
            fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.Author));

            if (moduleName == "PMM" || moduleName == "TSK" || moduleName == "NPR" || IsModuleTasks(moduleName))
                fields.Add(string.Format("<FieldRef Name='{0}' Nullable='TRUE'/>", DatabaseObjects.Columns.UserSkillMultiLookup));

            return fields;
        }

        private string LoadFieldsFromDB(string moduleName)
        {
            List<string> expressions = LoadFieldListFromDB(moduleName);
            return string.Join("", expressions.ToArray());
        }

        public List<UGITTask> LoadByTemplateID(long refID)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            tasks = Load(x => x.ModuleNameLookup == Constants.Template && x.TicketId == Convert.ToString(refID));

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref tasks);
            return tasks;
        }

        public List<UGITTask> LoadByProjectID(string refPublicID, bool includeOpenTicketsOnly = false)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            tasks = store.Load(x => x.TicketId == refPublicID);
            #region Copy code from sharepoint
            List<UGITTask> ticketRelations = new List<UGITTask>();
            if (tasks.Count > 0)
            {
                foreach (UGITTask uGITTask in tasks)
                {
                    //ModuleInstanceDependency depObj = ModuleInstanceDependency.LoadRelation(currentTicket);
                    if (includeOpenTicketsOnly && uGITTask.SubTaskType == "Ticket" && uGITTask.ModuleNameLookup != null && !string.IsNullOrWhiteSpace(uGITTask.ModuleNameLookup))
                    {

                        DataRow tItem = Ticket.GetCurrentTicket(_context, uGITTask.ModuleNameLookup, uGITTask.TicketId);
                        if (!UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(tItem, DatabaseObjects.Columns.TicketClosed)))
                        {
                            ticketRelations.Add(uGITTask);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(uGITTask.ModuleNameLookup))
                            ticketRelations.Add(uGITTask);
                    }
                }

                UGITTask tempObj = null;
                foreach (UGITTask mInstance in ticketRelations)
                {
                    if (!string.IsNullOrEmpty(mInstance.Predecessors))
                    {
                        mInstance.PredecessorsObj = new List<UGITTask>();
                        List<string> lstOfPredecessor = UGITUtility.SplitString(mInstance.Predecessors, Constants.Separator6).ToList();
                        foreach (string predecessor in lstOfPredecessor)
                        {
                            tempObj = ticketRelations.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(predecessor));
                            if (tempObj != null)
                                mInstance.PredecessorsObj.Add(tempObj);
                        }
                    }
                }
            }
            #endregion
            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref ticketRelations);
            return ticketRelations;
        }

        public List<UGITTask> LoadTemplateTask(string modulename)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            tasks = store.Load(x => x.TicketId == modulename && x.ModuleNameLookup == Constants.Template).ToList();

            // Set min date to the Start and Due Dates which are null or
            /// </summary> has default SQl Min date.
            ManageMinStartDueDates(ref tasks);
            return tasks;
        }

        /// <summary>
        /// This function only works for PMM, NPR and TSK tasks
        /// <param name="spWeb"></param>
        /// <param name="moduleName"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public static List<UGITTask> LoadByProjectID(string moduleName, int refID)
        {
            List<UGITTask> tasks = new List<UGITTask>();

            //SPQuery rQuery = new SPQuery();
            //rQuery.IncludeAttachmentUrls = true;
            //rQuery.ViewFields = LoadFieldsFromDB(moduleName);
            //if (!IsModuleTasks(moduleName))
            //{

            //    rQuery.Query = string.Format(@"<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>
            //                               <OrderBy><FieldRef Name='{2}'  /></OrderBy>",
            //     GetProjectLookupFieldName(moduleName), refID, DatabaseObjects.Columns.ItemOrder);
            //}
            //else
            //{
            //    var moduleDetail = uHelper.GetModuleDetails(moduleName, spWeb);
            //    var ticketList = Convert.ToString(moduleDetail[DatabaseObjects.Columns.ModuleTicketTable]);
            //    var currentTicket = SPListHelper.GetSPListItem(ticketList, refID, spWeb);
            //    var publicTicketID = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
            //    rQuery.Query = string.Format(@"<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>
            //                               <OrderBy><FieldRef Name='{2}'  /></OrderBy>",
            //         GetProjectLookupFieldName(moduleName), publicTicketID, DatabaseObjects.Columns.ItemOrder);
            //}
            //SPList list = SPListHelper.GetSPList(GetTaskList(moduleName), spWeb);
            //if (list != null)
            //{
            //    SPListItemCollection result = list.GetItems(rQuery);
            //    foreach (SPListItem row in result)
            //    {
            //        tasks.Add(LoadItem(spWeb, row, moduleName));
            //    }
            //}
            return tasks;
        }

        /// <summary>
        /// This function only works for PMM, NPR and TSK tasks
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="moduleName"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public List<UGITTask> LoadByProjectID(string moduleName, string refPublicID)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            if (string.IsNullOrEmpty(moduleName))
                tasks = store.Load(x => x.TicketId == refPublicID).OrderBy(x => x.ItemOrder).ToList();
            else
                tasks = store.Load(x => x.TicketId == refPublicID && x.ModuleNameLookup == moduleName).OrderBy(x => x.ItemOrder).ToList();

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref tasks);
            return tasks;
        }

        public List<UGITTask> LoadByModule(string moduleName)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            tasks = Load(x => x.ModuleNameLookup.Equals(moduleName)).OrderBy(x => x.ItemOrder).ToList();

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref tasks);
            return tasks;
        }

        private UGITTask LoadItem(DataRow rItem, string moduleName)
        {
            return LoadItem(rItem, moduleName, GetProjectLookupFieldName(moduleName));
        }
        /// <summary>
        /// Load task object from datarow of task 
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="rItem"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        private UGITTask LoadItem(DataRow rItem, string moduleName, string lookFieldName)
        {
            UGITTask task = new UGITTask();
            long Id = 0;
            long.TryParse(UGITUtility.ObjectToString(rItem[DatabaseObjects.Columns.Id]), out Id);
            task.ID = Id;
            task.Title = Convert.ToString(rItem[DatabaseObjects.Columns.Title]);

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.Body) && Convert.ToString(rItem[DatabaseObjects.Columns.Body]) != string.Empty)
                task.Description = Convert.ToString(rItem[DatabaseObjects.Columns.Body]);

            task.StartDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.StartDate) && Convert.ToString(rItem[DatabaseObjects.Columns.StartDate]) != string.Empty)
                task.StartDate = UGITUtility.StringToDateTime(rItem[DatabaseObjects.Columns.StartDate]);

            task.DueDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.DueDate) && Convert.ToString(rItem[DatabaseObjects.Columns.DueDate]) != string.Empty)
                task.DueDate = UGITUtility.StringToDateTime(rItem[DatabaseObjects.Columns.DueDate]);

            task.ProposedDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITProposedDate) && Convert.ToString(rItem[DatabaseObjects.Columns.UGITProposedDate]) != string.Empty)
                task.ProposedDate = Convert.ToDateTime(rItem[DatabaseObjects.Columns.UGITProposedDate]);

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITProposedStatus) && Convert.ToString(rItem[DatabaseObjects.Columns.UGITProposedStatus]) != string.Empty)
                task.ProposedStatus = (UGITTaskProposalStatus)Enum.Parse(typeof(UGITTaskProposalStatus), Convert.ToString(rItem[DatabaseObjects.Columns.UGITProposedStatus]));

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.ParentTask) && Convert.ToString(rItem[DatabaseObjects.Columns.ParentTask]) != string.Empty)
            {
                int parentTask = 0;
                int.TryParse(Convert.ToString(rItem[DatabaseObjects.Columns.ParentTask]), out parentTask);
                task.ParentTaskID = parentTask;
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.StageStep))
            {
                int stageStep = 0;
                int.TryParse(Convert.ToString(rItem[DatabaseObjects.Columns.StageStep]), out stageStep);
                task.StageStep = stageStep;
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.IsMilestone))
            {
                task.IsMileStone = UGITUtility.StringToBoolean(rItem[DatabaseObjects.Columns.IsMilestone]);
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.Predecessors))
            {
                task.Predecessors = Convert.ToString(rItem[DatabaseObjects.Columns.Predecessors]);
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.Status))
                task.Status = Convert.ToString(rItem[DatabaseObjects.Columns.Status]);

            //if (uHelper.IsSPItemExist(rItem, DatabaseObjects.Columns.TaskPriority))
            //    task.Priority = Convert.ToString(rItem[DatabaseObjects.Columns.TaskPriority]);

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.PercentComplete))
                task.PercentComplete = Convert.ToDouble(rItem[DatabaseObjects.Columns.PercentComplete]) * 100;

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITComment))
                task.CommentHistory = uHelper.GetHistory(Convert.ToString(rItem[DatabaseObjects.Columns.UGITComment]), false);

            double estimatedHours = 0;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.TaskEstimatedHours))
                double.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(rItem, DatabaseObjects.Columns.TaskEstimatedHours)), out estimatedHours);
            task.EstimatedHours = estimatedHours;


            double actualHours = 0;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.TaskActualHours))
                double.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(rItem, DatabaseObjects.Columns.TaskActualHours)), out actualHours);
            task.ActualHours = actualHours;


            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.AssignedTo))
            {
                task.AssignedTo = Convert.ToString(rItem[DatabaseObjects.Columns.AssignedTo]);
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITLevel))
            {
                int level = 0;
                int.TryParse(Convert.ToString(rItem[DatabaseObjects.Columns.UGITLevel]), out level);
                task.Level = level;
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.ItemOrder))
            {
                int itemOrder = 0;
                int.TryParse(Convert.ToString(rItem[DatabaseObjects.Columns.ItemOrder]), out itemOrder);
                task.ItemOrder = itemOrder;
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITChildCount))
            {
                int childCount = 0;
                int.TryParse(Convert.ToString(rItem[DatabaseObjects.Columns.UGITChildCount]), out childCount);
                task.ChildCount = childCount;
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITDuration))
                task.Duration = Convert.ToDouble(rItem[DatabaseObjects.Columns.UGITDuration]);

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITContribution))
                task.Contribution = Convert.ToDouble(rItem[DatabaseObjects.Columns.UGITContribution]);


            if (UGITUtility.IsSPItemExist(rItem, lookFieldName))
            {
                if (IsModuleTasks(moduleName))
                {
                    task.TicketId = Convert.ToString(rItem[lookFieldName]);
                }
                else
                {
                    if (UGITUtility.IsSPItemExist(rItem, lookFieldName))
                    {
                        task.ProjectLookup = UGITUtility.StringToLong(rItem[lookFieldName]);
                    }
                }
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.TaskBehaviour))
                task.Behaviour = Convert.ToString(rItem[DatabaseObjects.Columns.TaskBehaviour]);

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.ShowOnProjectCalendar))
                task.ShowOnProjectCalendar = Convert.ToBoolean(rItem[DatabaseObjects.Columns.ShowOnProjectCalendar]);

            task.AttachedFiles = new Dictionary<string, string>();
            //List<>
            //foreach (string attachment in rItem.Attachments)
            //{
            //    task.AttachedFiles.Add(attachment, string.Format("{0}{1}", rItem.Attachments.UrlPrefix, attachment));
            //}
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.LinkedDocuments))
            {
                task.LinkedDocuments = Convert.ToString(rItem[DatabaseObjects.Columns.LinkedDocuments]);
            }
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.TicketId))
            {
                task.TicketId = Convert.ToString(rItem[DatabaseObjects.Columns.TicketId]);
            }
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UGITAssignToPct))
            {
                task.AssignToPct = Convert.ToString(rItem[DatabaseObjects.Columns.UGITAssignToPct]);
            }
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.IsCritical))
                task.IsCritical = Convert.ToBoolean(rItem[DatabaseObjects.Columns.IsCritical]);
            else
                task.IsCritical = false;

            double eRHHours = 0;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.EstimatedRemainingHours))
                double.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(rItem, DatabaseObjects.Columns.EstimatedRemainingHours)), out eRHHours);
            task.EstimatedRemainingHours = eRHHours;

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.TaskReminderDays))
            {
                task.ReminderDays = Convert.ToInt32(rItem[DatabaseObjects.Columns.TaskReminderDays]);
            }
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.TaskRepeatInterval))
            {
                task.RepeatInterval = Convert.ToInt32(rItem[DatabaseObjects.Columns.TaskRepeatInterval]);
            }
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.TaskReminderEnabled))
            {
                task.ReminderEnabled = Convert.ToBoolean(rItem[DatabaseObjects.Columns.TaskReminderEnabled]);
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.SprintLookup))
            {
                task.SprintLookup = UGITUtility.StringToLong(rItem[DatabaseObjects.Columns.SprintLookup]);
            }

            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.UserSkillMultiLookup) && Convert.ToString(rItem[DatabaseObjects.Columns.UserSkillMultiLookup]) != string.Empty)
            {
                task.UserSkillMultiLookup = Convert.ToString(rItem[DatabaseObjects.Columns.UserSkillMultiLookup]);
            }
            task.Created = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.Created) && Convert.ToString(rItem[DatabaseObjects.Columns.Created]) != string.Empty)
                task.Created = Convert.ToDateTime(rItem[DatabaseObjects.Columns.Created]);

            task.Modified = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.Modified) && Convert.ToString(rItem[DatabaseObjects.Columns.Modified]) != string.Empty)
                task.Modified = Convert.ToDateTime(rItem[DatabaseObjects.Columns.Modified]);


            task.ModifiedBy = string.Empty;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.Editor))
            {
                task.ModifiedBy = UGITUtility.ObjectToString(rItem[DatabaseObjects.Columns.Editor]);
            }

            task.Author = string.Empty;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.Author))
            {
                task.Author = UGITUtility.ObjectToString(rItem[DatabaseObjects.Columns.Author]);
            }

            task.CompletionDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.CompletionDate) && Convert.ToString(rItem[DatabaseObjects.Columns.CompletionDate]) != string.Empty)
                task.CompletionDate = UGITUtility.StringToDateTime(rItem[DatabaseObjects.Columns.CompletionDate]);
            if (task.CompletionDate == new DateTime(1900, 1, 1))
                task.CompletionDate = DateTime.MinValue;


            task.CompletedBy = null;
            if (UGITUtility.IsSPItemExist(rItem, DatabaseObjects.Columns.CompletedBy) && Convert.ToString(rItem[DatabaseObjects.Columns.CompletedBy]) != string.Empty)
            {
                task.CompletedBy = Convert.ToString(rItem[DatabaseObjects.Columns.CompletedBy]);
            }

            task.ModuleNameLookup = Convert.ToString(rItem[DatabaseObjects.Columns.ModuleNameLookup]);

            return task;
        }

        public UGITTask CopyItem(ModuleTasksHistory moduleTaskHistoryItem)
        {
            UGITTask task = new UGITTask();

            try
            {
                task.ID = moduleTaskHistoryItem.ID;

                task.Title = moduleTaskHistoryItem.Title;

                task.Description = Convert.ToString(moduleTaskHistoryItem.Description);

                task.StartDate = moduleTaskHistoryItem.StartDate;

                task.DueDate = moduleTaskHistoryItem.DueDate;

                task.ProposedDate = moduleTaskHistoryItem.ProposedDate;

                task.ProposedStatus = moduleTaskHistoryItem.ProposedStatus;

                task.ParentTaskID = moduleTaskHistoryItem.ParentTaskID;

                task.StageStep = moduleTaskHistoryItem.StageStep;

                task.IsMileStone = UGITUtility.StringToBoolean(moduleTaskHistoryItem.IsMileStone);

                task.Predecessors = moduleTaskHistoryItem.Predecessors;

                task.Status = Convert.ToString(moduleTaskHistoryItem.Status);

                task.PercentComplete = Convert.ToDouble(moduleTaskHistoryItem.PercentComplete) * 100;

                task.CommentHistory = moduleTaskHistoryItem.CommentHistory;

                task.EstimatedHours = moduleTaskHistoryItem.EstimatedHours;

                task.ActualHours = moduleTaskHistoryItem.ActualHours;

                task.AssignedTo = moduleTaskHistoryItem.AssignedTo;

                task.Level = moduleTaskHistoryItem.Level;

                task.ItemOrder = moduleTaskHistoryItem.ItemOrder;

                task.ChildCount = moduleTaskHistoryItem.ChildCount;

                task.Duration = moduleTaskHistoryItem.Duration;

                task.Contribution = moduleTaskHistoryItem.Contribution;

                task.TicketId = moduleTaskHistoryItem.TicketId;

                task.Behaviour = moduleTaskHistoryItem.Behaviour;

                task.ShowOnProjectCalendar = moduleTaskHistoryItem.ShowOnProjectCalendar;

                task.LinkedDocuments = moduleTaskHistoryItem.LinkedDocuments;

                task.TicketId = Convert.ToString(moduleTaskHistoryItem.TicketId);

                task.AssignToPct = Convert.ToString(moduleTaskHistoryItem.AssignToPct);

                task.IsCritical = Convert.ToBoolean(moduleTaskHistoryItem.IsCritical);

                task.EstimatedRemainingHours = moduleTaskHistoryItem.EstimatedRemainingHours;

                task.ReminderDays = moduleTaskHistoryItem.ReminderDays;

                task.RepeatInterval = moduleTaskHistoryItem.RepeatInterval;

                task.ReminderEnabled = moduleTaskHistoryItem.ReminderEnabled;

                task.SprintLookup = moduleTaskHistoryItem.SprintLookup;

                task.UserSkillMultiLookup = moduleTaskHistoryItem.UserSkillMultiLookup;

                task.Created = moduleTaskHistoryItem.Created;

                task.Modified = moduleTaskHistoryItem.Modified;

                task.ModifiedBy = moduleTaskHistoryItem.ModifiedBy;

                task.Author = moduleTaskHistoryItem.Author;

                task.CompletionDate = moduleTaskHistoryItem.CompletionDate;

                task.CompletedBy = moduleTaskHistoryItem.CompletedBy;

                return task;
            }

            catch (Exception)
            {

                throw;

            }
        }

        public void DeleteTask(string moduleName, ref List<UGITTask> tasks, UGITTask currentTask)
        {
            List<UGITTask> deletedTasks = new List<UGITTask>();
            //SPList taskList = SPListHelper.GetSPList(GetTaskList(moduleName), spWeb);
            if (tasks == null || currentTask == null || currentTask.ID <= 0)
            {
                return;
            }

            UGITTask parentTask = tasks.FirstOrDefault(x => x.ID == currentTask.ParentTaskID);
            GetDeletedTasks(tasks, ref deletedTasks, currentTask);
            AgentJobHelper agentJobHelper = new AgentJobHelper(_context);
            foreach (UGITTask task in deletedTasks)
            {
                agentJobHelper.DeleteTaskReminder(UGITUtility.ObjectToString(task.ID), ScheduleActionType.Reminder);
                bool result = Delete(task);   // uGITDAL.Delete<UGITTask>(task, DatabaseObjects.Tables.ModuleTasks);
                if (result)
                    tasks.Remove(task);
            }

            if (parentTask != null)
            {
                parentTask.ChildTasks.Remove(currentTask);
                parentTask.ChildCount = parentTask.ChildTasks.Count;
                parentTask.Changes = true;
                if (parentTask.ChildTasks.Count > 0)
                {
                    parentTask.StartDate = parentTask.ChildTasks.Min(x => x.StartDate);
                    parentTask.DueDate = parentTask.ChildTasks.Max(x => x.DueDate);
                    parentTask.Duration = CalculateTaskDuration(parentTask);
                    parentTask.EstimatedHours = Math.Round(parentTask.ChildTasks.Sum(x => x.EstimatedHours), 2);
                    parentTask.ActualHours = parentTask.ChildTasks.Sum(x => x.ActualHours);
                    parentTask.EstimatedRemainingHours = Math.Round(parentTask.ChildTasks.Sum(x => x.EstimatedRemainingHours), 2);

                    PropagateTaskEffect(ref tasks, parentTask);
                }
            }
            ReOrderTasks(ref tasks);
            SaveTasks(ref tasks, moduleName, currentTask.TicketId);
        }

        public void GetDeletedTasks(List<UGITTask> tasks, ref List<UGITTask> deletedTasks, UGITTask currentTask)
        {
            if (!deletedTasks.Exists(x => x.ID == currentTask.ID))
            {
                deletedTasks.Add(currentTask);
            }

            if (currentTask.ChildTasks != null && currentTask.ChildTasks.Count > 0)
            {
                foreach (UGITTask task in currentTask.ChildTasks)
                {
                    deletedTasks.Add(task);
                    GetDeletedTasks(tasks, ref deletedTasks, task);
                }
            }
        }

        public void DeleteTasks(string moduleName, List<UGITTask> tasks)
        {
            if (tasks.Count <= 0)
            {
                return;
            }

            foreach (UGITTask task in tasks)
            {
                bool result = Delete(task);    // uGITDAL.Delete<UGITTask>(task, DatabaseObjects.Tables.ModuleTasks);
            }
        }

        /// <summary>
        /// This function only works for PMM, NPR and TSK tasks
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="moduleName"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public void SaveTasks(ref List<UGITTask> tasks, string moduleName, string publicTicketId)
        {
            if (tasks == null || tasks.Count <= 0)
            {
                ULog.WriteLog("SaveTask: No task found");
                return;
            }

            DataTable taskList = UGITUtility.ToDataTable<UGITTask>(this.LoadByProjectID(moduleName, publicTicketId));
            DataRow[] taskCollection = taskList.Select();

            List<UGITTask> tTasks = tasks.Where(x => x.Changes).ToList();
            foreach (UGITTask task in tTasks)
            {
                DataRow taskItem = null;
                //if (task.ID > 0)
                //    //taskItem = taskCollection.GetItemById(task.ID);
                //else
                taskItem = taskList.Rows.Add();

                taskItem[DatabaseObjects.Columns.Title] = task.Title.Trim();
                taskItem[DatabaseObjects.Columns.UGITChildCount] = task.ChildCount;
                //
                task.TicketId = publicTicketId;

                // We are comparing StartDate and DueDate with "new DateTime(1800, 1, 1)" because 
                // 1. it is the Min DateTime value for SQL DateTime type field.
                // 2. we have updated the type to DateTime2 which supports DateTime.MinValue as it's default min value.

                if (taskList.Columns.Contains(DatabaseObjects.Columns.StartDate))
                {
                    if (task.StartDate != DateTime.MinValue && task.StartDate <= new DateTime(1800, 1, 1))
                        task.StartDate = DateTime.MinValue;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.DueDate))
                {
                    if (task.DueDate != DateTime.MinValue && task.DueDate <= new DateTime(1800, 1, 1))
                        task.DueDate = DateTime.MinValue;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.PercentComplete))
                    task.PercentComplete = task.PercentComplete;

                if (taskList.Columns.Contains(DatabaseObjects.Columns.Comment))
                {
                    if (task.Comment != null && task.Comment.Trim() != string.Empty && (!task.Comment.StartsWith(";#False<;#>")))
                    {
                        UserProfile user = _context.CurrentUser;
                        string comment = uHelper.GetVersionString(user.UserName, task.Comment, taskItem, DatabaseObjects.Columns.Comment);
                        task.Comment = comment;
                        task.CommentHistory = uHelper.GetHistory(taskItem, DatabaseObjects.Columns.Comment, false);
                        //task.Comment = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(task.Comment) && task.Comment.StartsWith(";#False<;#>"))
                    {
                        var regex = new Regex(Regex.Escape(";#False<;#>"));
                        var replaceEmptyComment = regex.Replace(task.Comment, "", 1);
                        task.Comment = replaceEmptyComment;
                    }
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITProposedDate))
                {
                    if (task.ProposedDate <= DateTime.MinValue)
                        task.ProposedDate = new DateTime(1800, 1, 1);
                }


                //if (task.AttachedFiles != null && task.AttachedFiles.Count != taskItem.Attachments.Count)
                //{
                //    List<string> deleteFiles = new List<string>();
                //    foreach (string attachment in taskItem.Attachments)
                //    {
                //        if (task.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == attachment.ToLower()).Value == null)
                //        {
                //            deleteFiles.Add(attachment);
                //        }
                //    }

                //    foreach (string dFile in deleteFiles)
                //    {
                //        taskItem.Attachments.DeleteNow(dFile);
                //    }
                //}

                if (task.AttachFiles != null && task.AttachFiles.Count > 0)
                {
                    //foreach (string key in task.AttachFiles.Keys)
                    //{
                    //    string existingFile = taskItem.Attachments.Cast<string>().FirstOrDefault(x => x.ToLower() == key.ToLower());
                    //    if (string.IsNullOrEmpty(existingFile))
                    //    {
                    //        taskItem.Attachments.Add(key, task.AttachFiles[key]);
                    //    }
                    //}
                }


                if (taskList.Columns.Contains(DatabaseObjects.Columns.CompletionDate))
                {
                    if (task.Status == "Completed")
                    {
                        if (task.CompletionDate <= DateTime.MinValue && task.CompletionDate <= new DateTime(1800, 1, 1))
                            task.CompletionDate = DateTime.Now;
                    }
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.CompletedBy))
                {
                    taskItem[DatabaseObjects.Columns.CompletedBy] = DBNull.Value;
                    if (task.Status == "Completed")
                    {
                        if (string.IsNullOrEmpty(task.CompletedBy))
                            task.CompletedBy = _context.CurrentUser.Id;
                    }
                }

                if (task.ID < 1)
                {
                    long i = Insert(task);
                }
                else
                {
                    bool i = Update(task);
                }
                task.Changes = false;

                // FOR PMM only, if config variable TrackProjectStageHistory is TRUE, AND due date changed for a stage-linked task, log the change
                if (task.ModuleNameLookup == "PMM" && task.StageStep > 0)
                {
                    ThreadStart threadStartMethodUpdateProjectStageHistory = delegate () { UpdateProjectStageHistory(task); };
                    Thread sThreadUpdateProjectStageHistory = new Thread(threadStartMethodUpdateProjectStageHistory);
                    sThreadUpdateProjectStageHistory.IsBackground = true;
                    sThreadUpdateProjectStageHistory.Start();
                }
            }

        }
        public void SaveTasks(ref List<UGITTask> tasks, string moduleName, string publicTicketId, bool IsBatchNewTaskCheck = false)
        {
            if (tasks == null || tasks.Count <= 0)
            {
                ULog.WriteLog("SaveTask: No task found");
                return;
            }
            //SPFieldLookupValue projectLookup = new SPFieldLookupValue();//ProjectLookup
            //if (!IsModuleTasks(moduleName))
            //{
            //    projectLookup = tasks[0].ProjectLookup;
            //    if (projectLookup == null)
            //    {
            //        Log.WriteLog("SaveTask: Project Lookup is null");
            //        return;
            //    }
            //}

            //SPQuery rQuery = new SPQuery();
            //rQuery.IncludeAttachmentUrls = true;
            //rQuery.ViewFields = UGITTaskHelper.LoadFieldsFromDB(moduleName);
            //if (!IsModuleTasks(moduleName))
            //{
            //    rQuery.Query = string.Format(@"<Where><Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></Where>
            //                                   <OrderBy><FieldRef Name='{2}'  /></OrderBy>",
            //                                   GetProjectLookupFieldName(moduleName), projectLookup, DatabaseObjects.Columns.ItemOrder);
            //}
            //else
            //{
            //    string ticketid = tasks[0].TicketId;
            //    rQuery.Query = string.Format(@"<Where><Eq><FieldRef Name='{0}'  /><Value Type='Text'>{1}</Value></Eq></Where>
            //                                   <OrderBy><FieldRef Name='{2}'  /></OrderBy>",
            //                                   GetProjectLookupFieldName(moduleName), ticketid, DatabaseObjects.Columns.ItemOrder);
            //}

            //SPList taskList = SPListHelper.GetSPList(GetTaskList(moduleName), spWeb);
            //SPListItemCollection taskCollection = taskList.GetItems(rQuery);

            DataTable taskList = UGITUtility.ToDataTable<UGITTask>(this.LoadByProjectID(moduleName, publicTicketId));
            DataRow[] taskCollection = taskList.Select();

            List<UGITTask> tTasks = tasks.Where(x => x.Changes).ToList();

            DateTime oldDueDate = DateTime.MinValue;
            DateTime newDueDate = DateTime.MinValue;

            foreach (UGITTask task in tTasks)
            {
                DataRow taskItem = null;
                //if (task.ID > 0)
                //    taskItem = taskCollection.GetItemById(task.ID);
                //taskItem = taskList.Rows.Add();
                //else
                //    taskItem = taskList.AddItem();
                taskItem = taskList.Rows.Add();

                taskItem[DatabaseObjects.Columns.Title] = task.Title.Trim();
                if (taskList.Columns.Contains(DatabaseObjects.Columns.Body))
                {
                    taskItem[DatabaseObjects.Columns.Body] = task.Description;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.StartDate))
                {
                    taskItem[DatabaseObjects.Columns.StartDate] = DBNull.Value;
                    if (task.StartDate != DateTime.MinValue)
                        taskItem[DatabaseObjects.Columns.StartDate] = task.StartDate;
                }

                newDueDate = task.DueDate;
                if (taskList.Columns.Contains(DatabaseObjects.Columns.DueDate))
                {
                    oldDueDate = UGITUtility.StringToDateTime(taskItem[DatabaseObjects.Columns.DueDate]);
                    taskItem[DatabaseObjects.Columns.DueDate] = DBNull.Value;
                    if (task.DueDate != DateTime.MinValue)
                        taskItem[DatabaseObjects.Columns.DueDate] = task.DueDate;
                }
                else
                    oldDueDate = DateTime.MinValue;

                if (taskList.Columns.Contains(DatabaseObjects.Columns.PercentComplete))
                {
                    taskItem[DatabaseObjects.Columns.PercentComplete] = task.PercentComplete / 100;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.Status))
                {
                    taskItem[DatabaseObjects.Columns.Status] = task.Status;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.AssignedTo))
                {
                    taskItem[DatabaseObjects.Columns.AssignedTo] = task.AssignedTo;
                }

                //if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITFollowers))
                //{
                //    taskItem[DatabaseObjects.Columns.UGITFollowers] = task.UGITFollowers;
                //}


                if (taskList.Columns.Contains(DatabaseObjects.Columns.Comment))
                {
                    if (task.Comment != null && task.Comment.Trim() != string.Empty && (!task.Comment.StartsWith(";#False<;#>")))
                    {
                        UserProfile user = _context.CurrentUser;
                        string comment = uHelper.GetVersionString(user.UserName, task.Comment, taskItem, DatabaseObjects.Columns.Comment);
                        task.Comment = comment;
                        task.CommentHistory = uHelper.GetHistory(taskItem, DatabaseObjects.Columns.Comment, false);
                        //task.Comment = string.Empty;
                    }
                    if (task.Comment != null && task.Comment.StartsWith(";#False<;#>"))
                    {
                        var regex = new Regex(Regex.Escape(";#False<;#>"));
                        var replaceEmptyComment = regex.Replace(task.Comment, "", 1);
                        task.Comment = replaceEmptyComment;
                    }
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.TaskEstimatedHours))
                {
                    taskItem[DatabaseObjects.Columns.TaskEstimatedHours] = task.EstimatedHours;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.TaskActualHours))
                {
                    taskItem[DatabaseObjects.Columns.TaskActualHours] = task.ActualHours;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.ItemOrder))
                {
                    taskItem[DatabaseObjects.Columns.ItemOrder] = task.ItemOrder;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITLevel))
                {
                    taskItem[DatabaseObjects.Columns.UGITLevel] = task.Level;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITDuration))
                {
                    taskItem[DatabaseObjects.Columns.UGITDuration] = task.Duration;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITContribution))
                {
                    taskItem[DatabaseObjects.Columns.UGITContribution] = task.Contribution;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITChildCount))
                {
                    taskItem[DatabaseObjects.Columns.UGITChildCount] = task.ChildCount;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                {
                    taskItem[DatabaseObjects.Columns.ModuleNameLookup] = uHelper.getModuleIdByModuleName(_context, task.ModuleNameLookup);
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.ParentTaskID))
                {
                    taskItem[DatabaseObjects.Columns.ParentTaskID] = task.ParentTaskID;
                }


                if (taskList.Columns.Contains(DatabaseObjects.Columns.Predecessors))
                {
                    taskItem[DatabaseObjects.Columns.Predecessors] = task.Predecessors;
                }

                // Note - this is the main project lookup field for non-module tasks, not ticket Id
                if (taskList.Columns.Contains(GetProjectLookupFieldName(moduleName)))
                {
                    taskItem[GetProjectLookupFieldName(moduleName)] = task.ProjectLookup;
                }

                // For module tasks ONLY, this points to parent ticket
                // For PMM, points to related ticket when task type is ticket
                if (taskList.Columns.Contains(DatabaseObjects.Columns.TicketId))
                {
                    taskItem[DatabaseObjects.Columns.TicketId] = task.TicketId;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITProposedDate))
                {
                    taskItem[DatabaseObjects.Columns.UGITProposedDate] = DBNull.Value;
                    if (task.ProposedDate != DateTime.MinValue)
                        taskItem[DatabaseObjects.Columns.UGITProposedDate] = task.ProposedDate;
                }
                //if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITProposedStatus))Need to check 
                //{
                //    taskItem[DatabaseObjects.Columns.UGITProposedStatus] = task.ProposedStatus.ToString();
                //}

                if (taskList.Columns.Contains(DatabaseObjects.Columns.IsMilestone))
                {
                    taskItem[DatabaseObjects.Columns.IsMilestone] = task.IsMileStone ? 1 : 0;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.StageStep))
                {
                    taskItem[DatabaseObjects.Columns.StageStep] = task.StageStep;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour))
                {
                    taskItem[DatabaseObjects.Columns.TaskBehaviour] = task.Behaviour;
                }

                //if (task.AttachedFiles != null && task.AttachedFiles.Count != taskItem.Attachments.Count)
                //{
                //    List<string> deleteFiles = new List<string>();
                //    foreach (string attachment in taskItem.Attachments)
                //    {
                //        if (task.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == attachment.ToLower()).Value == null)
                //        {
                //            deleteFiles.Add(attachment);
                //        }
                //    }

                //    foreach (string dFile in deleteFiles)
                //    {
                //        taskItem.Attachments.DeleteNow(dFile);
                //    }
                //}

                //if (task.AttachFiles != null && task.AttachFiles.Count > 0)
                //{
                //    foreach (string key in task.AttachFiles.Keys)
                //    {
                //        string existingFile = taskItem.Attachments.Cast<string>().FirstOrDefault(x => x.ToLower() == key.ToLower());
                //        if (string.IsNullOrEmpty(existingFile))
                //        {
                //            taskItem.Attachments.Add(key, task.AttachFiles[key]);
                //        }
                //    }
                //}

                //if (taskList.Columns.Contains(DatabaseObjects.Columns.AttachedDocuments))
                //{
                //    taskItem[DatabaseObjects.Columns.AttachedDocuments] = task.LinkedDocuments;
                //}

                if (taskList.Columns.Contains(DatabaseObjects.Columns.ShowOnProjectCalendar))
                {
                    taskItem[DatabaseObjects.Columns.ShowOnProjectCalendar] = task.ShowOnProjectCalendar;
                }

                //new for assignToPct.
                if (taskList.Columns.Contains(DatabaseObjects.Columns.UGITAssignToPct))
                {
                    taskItem[DatabaseObjects.Columns.UGITAssignToPct] = task.AssignToPct;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.SprintLookup))
                {
                    taskItem[DatabaseObjects.Columns.SprintLookup] = task.SprintLookup;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.IsCritical))
                {

                    taskItem[DatabaseObjects.Columns.IsCritical] = task.IsCritical;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.EstimatedRemainingHours))
                {
                    taskItem[DatabaseObjects.Columns.EstimatedRemainingHours] = task.EstimatedRemainingHours;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.TaskReminderDays))
                {
                    taskItem[DatabaseObjects.Columns.TaskReminderDays] = task.ReminderDays;
                }
                if (taskList.Columns.Contains(DatabaseObjects.Columns.TaskRepeatInterval))
                {
                    taskItem[DatabaseObjects.Columns.TaskRepeatInterval] = task.RepeatInterval;
                }
                if (taskList.Columns.Contains(DatabaseObjects.Columns.TaskReminderEnabled))
                {
                    taskItem[DatabaseObjects.Columns.TaskReminderEnabled] = task.ReminderEnabled;
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.UserSkillMultiLookup))
                {
                    taskItem[DatabaseObjects.Columns.UserSkillMultiLookup] = task.UserSkillMultiLookup;//checked value
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.CompletionDate))
                {
                    taskItem[DatabaseObjects.Columns.CompletionDate] = DBNull.Value;
                    if (task.Status == "Completed")
                    {
                        if (task.CompletionDate != DateTime.MinValue)
                            taskItem[DatabaseObjects.Columns.CompletionDate] = task.CompletionDate;
                        else
                            taskItem[DatabaseObjects.Columns.CompletionDate] = DateTime.Now;
                    }
                }

                if (taskList.Columns.Contains(DatabaseObjects.Columns.CompletedBy))
                {
                    taskItem[DatabaseObjects.Columns.CompletedBy] = DBNull.Value;
                    if (task.Status == "Completed")
                    {
                        if (string.IsNullOrEmpty(task.CompletedBy))
                            task.CompletedBy = _context.CurrentUser.Id;
                    }
                }

                if (task.ID < 1)
                {
                    long i = Insert(task);
                }
                else
                {
                    bool i = Update(task);
                }
                task.Changes = false;

                // FOR PMM only, if config variable TrackProjectStageHistory is TRUE, AND due date changed for a stage-linked task, log the change
                if (task.ModuleNameLookup == "PMM" && task.StageStep > 0)
                {
                    ThreadStart threadStartMethodUpdateProjectStageHistory = delegate () { UpdateProjectStageHistory(task); };
                    Thread sThreadUpdateProjectStageHistory = new Thread(threadStartMethodUpdateProjectStageHistory);
                    sThreadUpdateProjectStageHistory.IsBackground = true;
                    sThreadUpdateProjectStageHistory.Start();
                }
                if (!IsBatchNewTaskCheck)
                    task.Changes = false;
            }

        }


        public void UpdateProjectStageHistory(UGITTask task)
        {
            if (task == null || task.ProjectLookup < 0)
            {
                ULog.WriteLog("ERROR: Missing project id, cannot update project stage history!");
                return;
            }

            //if (oldDueDate == null || oldDueDate == DateTime.MinValue)
            if (task.DueDate == null || task.DueDate == DateTime.MinValue)
            {
                // Initial task creation, skip creating entry
                return;
            }

            if (task == null || task.DueDate == null || task.DueDate == DateTime.MinValue)
            {
                ULog.WriteLog("ERROR: Missing new due date, cannot update project stage history!");
                return;
            }

            ProjectStageHistory obj = new ProjectStageHistory();
            obj.Title = task.Title;
            obj.TaskID = Convert.ToString(task.ID);
            obj.TicketId = task.TicketId;
            obj.StageStep = task.StageStep;
            obj.StartDate = task.StartDate;
            obj.EndDate = task.DueDate;

            if (obj.ID > 0)
                prjStageHistoryMgr.Update(obj);
            else
                prjStageHistoryMgr.Insert(obj);
        }

        /// <summary>
        /// This function only works for PMM, NPR and TSK tasks
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="moduleName"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public void SaveTask(ref UGITTask task, string moduleName, string publicTicketId)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            task.Changes = true;
            tasks.Add(task);
            SaveTasks(ref tasks, moduleName, publicTicketId);
            task.ID = tasks[0].ID;
        }


        public DataTable LoadTasksTable(string moduleName, bool fromCache, string projectPublicID)
        {
            DataTable taskTable = null;
            taskTable = LoadTaskTable(moduleName, projectPublicID);
            if (taskTable == null || taskTable.Rows.Count == 0)
            {
                List<UGITTask> tasks = LoadByProjectID(moduleName, projectPublicID);
                taskTable = UGITUtility.ToDataTable<UGITTask>(tasks); //FillTaskTable(tasks);

                DataRow ticketItem = Ticket.GetCurrentTicket(_context, moduleName, projectPublicID);
                double projectDuration = UGITUtility.StringToDouble(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.TicketDuration));
                CalculateContribution(taskTable, projectDuration);
                if (taskTable != null)
                {
                    taskTable.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
                    taskTable = taskTable.DefaultView.ToTable();
                    //TaskCache.UpdatePredecessorsByOrder(taskTable);
                }
            }


            return taskTable;
        }
        public DataTable GetAllTasks(string moduleName)
        {
            DataTable tasks = null;
            List<UGITTask> lstTasks = LoadByModule(moduleName);
            tasks = UGITUtility.ToDataTable<UGITTask>(lstTasks);
            return tasks;
        }
        public DataTable LoadTasksTable(string moduleName)
        {
            List<UGITTask> tasks = LoadByModule(moduleName);
            // DataTable taskTable = FillTaskTable(tasks);
            DataTable taskTable = UGITUtility.ToDataTable<UGITTask>(tasks);
            return taskTable;
        }
        public DataTable GetAllTasksByProjectID(string moduleName, string projectPublicID)
        {
            DataRow projectItem = Ticket.GetCurrentTicket(_context, moduleName, projectPublicID);
            DataTable tasks = null;
            if (projectItem != null && !UGITUtility.StringToBoolean(projectItem[DatabaseObjects.Columns.TicketClosed]))
            {
                //  LoadTaskIfEmpty();
                tasks = GetAllTasks(moduleName, projectPublicID);

                //Sort by itemorder to get proper ordering
                if (tasks != null && tasks.Rows.Count > 0)
                {
                    tasks.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
                    tasks = tasks.DefaultView.ToTable();
                }
            }
            else
            {
                if (moduleName == Constants.PMMIssue)
                {
                    //UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, "PMMIssues", DatabaseObjects.Columns.TicketPMMIdLookup);
                    //List<UGITTask> issuesTasks = tHelper.LoadByProjectID(projectPublicID);
                    //tasks = UGITTaskHelper.FillTaskTable(issuesTasks);
                }
                else if (moduleName == Constants.ExitCriteria)
                {
                    //UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, DatabaseObjects.Lists.ModuleStageConstraints, DatabaseObjects.Columns.TicketId);
                    //List<UGITTask> criteriaTasks = tHelper.LoadByProjectID(projectPublicID);
                    //tasks = UGITTaskHelper.FillTaskTable(criteriaTasks);
                }
                if (moduleName == Constants.VendorRisks)
                {
                    //UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, DatabaseObjects.Lists.VendorRisks, DatabaseObjects.Columns.VendorMSALookup);
                    //List<UGITTask> issuesTasks = tHelper.LoadByProjectID(projectPublicID);
                    //tasks = UGITTaskHelper.FillTaskTable(issuesTasks);
                }
                else if (moduleName == Constants.VendorIssue)
                {
                    //UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, DatabaseObjects.Lists.VendorIssues, DatabaseObjects.Columns.VendorMSALookup);
                    //List<UGITTask> criteriaTasks = tHelper.LoadByProjectID(projectPublicID);
                    //tasks = UGITTaskHelper.FillTaskTable(criteriaTasks);
                }
                else if (moduleName == "PMM" || moduleName == "TSK" || moduleName == "TSK")
                {
                    // tasks = UGITTaskHelper.LoadTasksTable(SPContext.Current.Web, moduleName, false, projectPublicID);
                }
                //else if (UGITTaskHelper.IsModuleTasks(moduleName))
                //{
                //UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, "ModuleTasks", DatabaseObjects.Columns.TicketId);
                //List<UGITTask> moduleTasks = tHelper.LoadByProjectID(projectPublicID);
                //tasks = UGITTaskHelper.FillTaskTable(moduleTasks);
                //  }
            }

            //if (tasks == null)
            //    tasks = CreateSchema();

            return tasks;
        }
        public DataTable GetSummaryTasksByProjectID(string moduleName, string projectPublicID)
        {
            DataRow projectItem = Ticket.GetCurrentTicket(_context, moduleName, projectPublicID);
            DataTable summarytasks = null;
            DataTable tasks = null;
            //if (projectItem != null && !UGITUtility.StringToBoolean(projectItem[DatabaseObjects.Columns.TicketClosed]))
            //{
            //    tasks = GetAllTasks(moduleName, projectPublicID);
            //}
            //else
            //{
                tasks = LoadTasksTable(moduleName, false, projectPublicID);
            //}


            if (tasks != null && tasks.Rows.Count > 0)
            {
                DataRow[] rows = tasks.Select(string.Format("({0} ='{1}' And TicketId='{2}' AND ParentTaskID=0 AND {4}='') or {3} in('milestone','deliverable','receivable')", DatabaseObjects.Columns.ModuleNameLookup, moduleName, projectPublicID, DatabaseObjects.Columns.TaskBehaviour, DatabaseObjects.Columns.UGITSubTaskType));
                if (rows.Length > 0)
                {
                    summarytasks = rows.CopyToDataTable();
                }
            }

            return summarytasks;
        }
        public DataTable GetAllTasks(string moduleName, string projectPublicID)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            tasks = Load(x => x.ModuleNameLookup.Equals(moduleName) && x.TicketId.Equals(projectPublicID));

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref tasks);

            DataTable taskTable = UGITUtility.ToDataTable<UGITTask>(tasks);
            return taskTable;
        }

        public static string GetTaskAssignedToWithPntg(ApplicationContext context, string strUGITAssignToPct)
        {
            if (!string.IsNullOrEmpty(strUGITAssignToPct))
            {
                /*
                //List<UGITAssignTo> listAssignTo = UGITTaskHelper.GetUGITAssignPct(strUGITAssignToPct);
                string strAssignToPct = string.Empty;
                //foreach (var item in listAssignTo)
                //{
                //    if (!string.IsNullOrEmpty(strAssignToPct))
                //        strAssignToPct += Constants.UserInfoSeparator;

                //    if (item.Percentage != "100")
                //        strAssignToPct += item.UserName + "[" + item.Percentage + "%]";
                //    else
                //        strAssignToPct += item.UserName;

                //}
                */

                UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
                List<UGITAssignTo> listAssignTo = uGITTaskManager.GetUGITTaskPct(context, strUGITAssignToPct);


                string strAssignToPct = string.Empty;
                foreach (var item in listAssignTo)
                {
                    if (!string.IsNullOrEmpty(strAssignToPct))
                        strAssignToPct += Constants.UserInfoSeparator;

                    if (item.Percentage != "100")
                        strAssignToPct += item.UserName + "[" + item.Percentage + "%]";
                    else
                        strAssignToPct += item.UserName;

                }

                return strAssignToPct + ";";
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Calculates project's startdate, enddate, pctcomplete, duration and  DaysToComplete
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="tasks"></param>
        /// <param name="pItem"></param>
        public void CalculateProjectStartEndDate(string moduleName, List<UGITTask> tasks, DataRow pItem)
        {
            if (moduleName != "PMM" && moduleName != "TSK" && moduleName != "NPR" && !IsModuleTasks(moduleName))
                return;

            if (tasks == null || tasks.Count == 0)
            {
                // This happens if the last task has just been deleted, or user chose to delete all tasks
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedHours))
                    pItem[DatabaseObjects.Columns.EstimatedHours] = 0;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.ActualHour))
                    pItem[DatabaseObjects.Columns.ActualHour] = 0;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedRemainingHours))
                    pItem[DatabaseObjects.Columns.EstimatedRemainingHours] = 0;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketActualStartDate))
                    pItem[DatabaseObjects.Columns.TicketActualStartDate] = DBNull.Value;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketActualCompletionDate))
                    pItem[DatabaseObjects.Columns.TicketActualCompletionDate] = DBNull.Value;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketDuration))
                    pItem[DatabaseObjects.Columns.TicketDuration] = 0;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketPctComplete))
                    pItem[DatabaseObjects.Columns.TicketPctComplete] = 0;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.UGITDaysToComplete))
                    pItem[DatabaseObjects.Columns.UGITDaysToComplete] = 0;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.NextActivity))
                    pItem[DatabaseObjects.Columns.NextActivity] = string.Empty;
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.NextMilestone))
                    pItem[DatabaseObjects.Columns.NextMilestone] = string.Empty;
            }
            else
            {
                DateTime minDate = tasks.Min(x => x.StartDate);
                DateTime maxDate = tasks.Max(x => x.DueDate);

                List<UGITTask> rootTasks = tasks.Where(x => x.ParentTaskID == 0).ToList();
                List<UGITTask> leafTasks = tasks.Where(x => x.ChildCount == 0).ToList();

                //new line of code for calucate project actual/estimate/estimated remaining hours.
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedHours))    // unable to find column with name estimated hours
                {
                    double estimatedHours = leafTasks.Sum(x => x.EstimatedHours);
                    pItem[DatabaseObjects.Columns.EstimatedHours] = estimatedHours;

                }
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.ActualHour))
                {
                    double actualHours = leafTasks.Sum(x => x.ActualHours);
                    pItem[DatabaseObjects.Columns.ActualHour] = actualHours;
                }

                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedRemainingHours))
                {
                    double estimateRemainingHours = leafTasks.Sum(x => x.EstimatedRemainingHours);
                    pItem[DatabaseObjects.Columns.EstimatedRemainingHours] = estimateRemainingHours;
                }

                if (minDate != DateTime.MinValue && minDate != DateTime.MaxValue)
                    pItem[DatabaseObjects.Columns.TicketActualStartDate] = minDate;
                if (maxDate != DateTime.MinValue && maxDate != DateTime.MaxValue)
                    pItem[DatabaseObjects.Columns.TicketActualCompletionDate] = maxDate;

                //if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketDuration) &&
                //    minDate != DateTime.MinValue && minDate != DateTime.MaxValue &&
                //    maxDate != DateTime.MinValue && maxDate != DateTime.MaxValue)
                //    pItem[DatabaseObjects.Columns.TicketDuration] = UGITUtility.GetTotalWorkingDaysBetween(minDate, maxDate);

                //Calculates Pct Complete of project
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.TicketPctComplete))
                {

                    double pctComplete = 0;
                    double totalTaskEsimatedHr = rootTasks.Sum(x => x.EstimatedHours);
                    if (UGITUtility.StringToBoolean(objConfigurationVariableHelper.GetValue(ConfigConstants.ProjectCompletionUsingEstHours)) && totalTaskEsimatedHr > 0)
                    {
                        double completedTaskEstimatedHr = rootTasks.Sum(x => (x.EstimatedHours * x.PercentComplete) / 100);
                        pctComplete = (completedTaskEstimatedHr / totalTaskEsimatedHr);
                    }
                    else
                    {
                        double totalTaskDuration = rootTasks.Sum(x => x.Duration);
                        if (totalTaskDuration > 0)
                        {
                            double completedTaskDuration = rootTasks.Sum(x => (x.Duration * x.PercentComplete) / 100);
                            pctComplete = (completedTaskDuration / totalTaskDuration);
                        }
                    }

                    pctComplete = Math.Round(pctComplete, 4, MidpointRounding.AwayFromZero);
                    pItem[DatabaseObjects.Columns.TicketPctComplete] = pctComplete * 100;
                }

                //Calcuates totaldaystocomplete
                if (pItem.Table.Columns.Contains(DatabaseObjects.Columns.UGITDaysToComplete))
                {
                    double daysToComplete = rootTasks.Sum(x => x.PercentComplete == 100 ? 0 : (((100 - x.PercentComplete) * x.Duration) / 100));
                    pItem[DatabaseObjects.Columns.UGITDaysToComplete] = daysToComplete;
                }

                //new lines of code for update the nextactivity and nextmilestone..........
                if (moduleName == "PMM" || moduleName == "TSK")
                {
                    // for Activity task, get first incomplete 2nd level task under a stage-linked task (that has .IsMileStone set)
                    pItem[DatabaseObjects.Columns.NextActivity] = string.Empty;
                    List<UGITTask> secondLevelActivityTasks = tasks.Where(t => t.ParentTask != null && t.ParentTask.IsMileStone && t.Status != Constants.Completed && // t.TaskLevel == 1 && 
                                                                               t.Behaviour != Constants.TaskType.Milestone).OrderBy(t => t.ItemOrder).ToList();
                    if (secondLevelActivityTasks != null && secondLevelActivityTasks.Count > 0)
                    {
                        UGITTask activityTaskItem = secondLevelActivityTasks.FirstOrDefault();
                        if (activityTaskItem != null)
                            pItem[DatabaseObjects.Columns.NextActivity] = activityTaskItem.Title;
                    }
                    else
                    {
                        List<UGITTask> firstLevelActivityTasks = tasks.Where(t => t.Level == 0 && t.Status != Constants.Completed && !t.IsMileStone && t.Behaviour != Constants.TaskType.Milestone).OrderBy(t => t.ItemOrder).ToList();
                        if (firstLevelActivityTasks != null && firstLevelActivityTasks.Count > 0)
                        {
                            UGITTask activityTaskItem = firstLevelActivityTasks.FirstOrDefault();
                            if (activityTaskItem != null)
                                pItem[DatabaseObjects.Columns.NextActivity] = activityTaskItem.Title;
                        }
                    }

                    // for milestone task..
                    pItem[DatabaseObjects.Columns.NextMilestone] = string.Empty;
                    List<UGITTask> milestoneTasks = tasks.Where(t => t.Status != Constants.Completed && t.Behaviour == Constants.TaskType.Milestone).OrderBy(t => t.ItemOrder).ToList();
                    if (milestoneTasks != null && milestoneTasks.Count > 0)
                    {
                        UGITTask milestoneTaskItem = milestoneTasks.FirstOrDefault();
                        if (milestoneTaskItem != null)
                            pItem[DatabaseObjects.Columns.NextMilestone] = milestoneTaskItem.Title;
                    }
                }
            }

            //Auto Calculate Monitor State "On Time"
            if (moduleName == "PMM")
                AutoCalculateProjectMonitorStateOnTime(moduleName, tasks, pItem, _context);
        }

        /// <summary>
        /// Get Baseline task for PMM project only
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="moduleName"></param>
        /// <param name="projectPublicID"></param>
        /// <param name="totalDuration"></param>
        /// <param name="baselineNum"></param>
        /// <returns></returns>
        public List<UGITTask> LoadBaselineTableByProjectId(ApplicationContext context, string moduleName, string ticketId, double baselineNum)
        {
            ModuleTaskHistoryManager _moduleTaskHistoryManager = new ModuleTaskHistoryManager(context);
            if (moduleName != "PMM")
                return null;

            List<UGITTask> tasks = new List<UGITTask>();

            //SPList pmmTasksList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMTasksHistory);

            //SPQuery rQuery = new SPQuery();
            //rQuery.IncludeAttachmentUrls = true;
            //// StringBuilder fields = new StringBuilder();
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Description);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.DueDate);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.StartDate);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TicketPMMIdLookup);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ParentTask);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Predecessors);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Status);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.PercentComplete);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.AssignedTo);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ItemOrder);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TaskActualHours);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TaskEstimatedHours);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TaskBehaviour);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.IsMilestone);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.StageStep);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Attachments);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.UGITAssignToPct);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.SprintLookup);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.UGITProposedStatus);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.IsCritical);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.EstimatedRemainingHours);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.ActualHour);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TaskReminderDays);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TaskReminderEnabled);
            // fields.AppendFormat("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.TaskRepeatInterval);


            //    rQuery.ViewFields = fields.ToString();

            //    // rQuery.Query = string.Format("<Where><And><Or><Eq><FieldRef Name='{0}'  /><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq></Or><Eq><FieldRef Name='{2}' /><Value Type='Lookup'>{3}</Value></Eq></And></Where><OrderBy><FieldRef Name='{4}'  /></OrderBy>", DatabaseObjects.Columns.TicketPMMIdLookup, projectPublicID, DatabaseObjects.Columns.BaselineNum, baselineNum, DatabaseObjects.Columns.ItemOrder);
            //    XElement xmlWhere = new XElement("Where",
            //                            new XElement("And",
            //                                new XElement("Eq",
            //                                    new XElement("FieldRef", new XAttribute("Name", DatabaseObjects.Columns.TicketPMMIdLookup)),
            //                                    new XElement("Value", new XAttribute("Type", "Lookup"), projectPublicID)

            //                                ),
            //                                new XElement("Eq",
            //                                                new XElement("FieldRef", new XAttribute("Name", DatabaseObjects.Columns.BaselineNum)),
            //                                                new XElement("Value", new XAttribute("Type", "Lookup"), baselineNum)
            //                                )
            //                            )
            //                        );
            //    XElement xmlOrder = new XElement("OrderBy", new XElement("FieldRef", new XAttribute("Name", DatabaseObjects.Columns.ItemOrder)));

            //rQuery.Query = xmlWhere.ToString() + xmlOrder.ToString();

            //SPListItemCollection resultCollection = pmmTasksList.GetItems(rQuery);

            List<ModuleTasksHistory> moduleTaskHistory = _moduleTaskHistoryManager.Load($"{DatabaseObjects.Columns.TicketId}='{ticketId}' and {DatabaseObjects.Columns.BaselineId}={baselineNum}").OrderBy(x => x.ItemOrder).ToList();

            foreach (var item in moduleTaskHistory)
            {
                UGITTask taskList = new UGITTask();

                tasks.Add(CopyItem(item));
            }

            if (tasks != null && tasks.Count > 0)
            {
                tasks = MapRelationalObjects(tasks);

                CalculateDuration(ref tasks);

                ReOrderTasks(ref tasks);

                tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            }

            return tasks;
        }

        /// <summary>
        /// This function only works for PMM, NPR and TSK tasks
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="moduleName"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public List<UGITTask> ImportTasks(string moduleName, List<UGITTask> importTasks, bool calculateEstimatedHrs, string projectId)
        {
            foreach (UGITTask task in importTasks)
            {
                task.ModuleNameLookup = moduleName;
                task.Changes = true;
                task.ID = 0;
                task.Predecessors = null;
                task.ParentTaskID = 0;
                //task.Status = "Not Started";
                //task.PercentComplete = 0;
            }

            SaveTasks(ref importTasks, moduleName, projectId);

            //remanage parentid and predecessors

            foreach (UGITTask task in importTasks)
            {
                List<string> predecessors = new List<string>();
                if (task.ParentTask != null)
                {
                    task.ParentTaskID = task.ParentTask.ID;
                    task.Changes = true;
                }

                if (task.PredecessorTasks != null && task.PredecessorTasks.Count > 0)
                {
                    foreach (UGITTask predecessorTask in task.PredecessorTasks)
                    {
                        predecessors.Add(UGITUtility.ObjectToString(predecessorTask.ID));
                    }
                    task.Predecessors = string.Join(Constants.Separator6, predecessors);
                    task.Changes = true;
                }
            }

            ReManageTasks(ref importTasks, calculateEstimatedHrs);

            //Saves parentid and predecessor relation
            SaveTasks(ref importTasks, moduleName, projectId);

            return importTasks;
        }

        public List<UGITTask> ImportTasks(List<UGITTask> importTasks)
        {
            foreach (UGITTask task in importTasks)
            {
                task.Changes = true;
                task.ID = 0;
                task.Predecessors = null;
                task.ParentTaskID = 0;
            }

            SaveTasks(ref importTasks);
            //SPFieldLookupValueCollection predecessors = new SPFieldLookupValueCollection();
            //Added 24A jan 2020
            foreach (UGITTask task in importTasks)
            {

                if (task.ChildCount > 0 && task.ChildTasks != null)
                {
                    foreach (UGITTask childtask in task.ChildTasks)
                    {
                        childtask.ParentTaskID = task.ID;
                    }

                    task.Changes = true;
                }
            }
            //
            ////remanage parentid and predecessors
            //SPFieldLookupValueCollection predecessors = new SPFieldLookupValueCollection();
            //foreach (UGITTask task in importTasks)
            //{
            //    if (task.ParentTask != null)
            //    {
            //        task.ParentTaskID = task.ParentTask.ID;
            //        task.Changes = true;
            //    }

            //    if (task.PredecessorTasks != null && task.PredecessorTasks.Count > 0)
            //    {
            //        predecessors = new SPFieldLookupValueCollection();
            //        foreach (UGITTask predecessorTask in task.PredecessorTasks)
            //        {
            //            predecessors.Add(new SPFieldLookupValue(predecessorTask.ID, string.Empty));
            //        }
            //        task.Predecessors = predecessors;
            //        task.Changes = true;
            //    }
            //}

            //Saves parentid and predecessor relation
            //Added 24B jan 2020
            ReManageTasks(ref importTasks, false);
            //
            SaveTasks(ref importTasks);

            return importTasks;
        }

        public void SaveTasks(ref List<UGITTask> tasks)
        {
            if (tasks == null || tasks.Count <= 0)
            {
                ULog.WriteLog("Empty task list passed to SaveTasks()");
                return;
            }


            List<UGITTask> tTasks = tasks.Where(x => x.Changes).ToList();
            foreach (UGITTask task in tTasks)
            {

                DataRow taskItem = null;
                //if (task.ID > 0)
                //    taskItem = taskCollection.GetItemById(task.ID);
                //else
                //    taskItem = taskList.AddItem();

                task.PercentComplete = task.PercentComplete;

                if (task.Comment != null && task.Comment.Trim() != string.Empty)
                {
                    string comment = uHelper.GetVersionString(dbContext.CurrentUser.UserName, task.Comment, taskItem, DatabaseObjects.Columns.UGITComment);
                    task.Comment = comment;
                    //task.CommentHistory = uHelper.GetHistory(comment, false);
                }



                if (task.Status == "Completed")
                {
                    if (task.CompletedBy != null)
                        task.CompletedBy = task.CompletedBy;
                    else
                        task.CompletedBy = dbContext.CurrentUser.UserName;
                }

                if (task.ID < 1)
                {
                    long i = Insert(task);  // uGITDAL.InsertItem<UGITTask>(task);
                }
                else
                {
                    bool i = Update(task);  // uGITDAL.Update<UGITTask>(task, DatabaseObjects.Tables.ModuleTasks);
                }
                task.Changes = false;
            }
        }

        public void SaveTask(ref UGITTask task)
        {
            List<UGITTask> tasks = new List<UGITTask>();
            task.Changes = true;
            tasks.Add(task);
            SaveTasks(ref tasks);
            task.ID = tasks[0].ID;
        }

        public string LoadJson(DataTable pmmTasks, bool showOnlyTitle)
        {
            if (pmmTasks == null || pmmTasks.Rows.Count <= 0)
                return string.Empty;

            StringBuilder json = new StringBuilder();

            //query.IncludeAttachmentUrls = true;
            DataRow[] rows = pmmTasks.AsEnumerable().Where(p => p.Field<int>(DatabaseObjects.Columns.ParentTask) == 0).ToArray();
            json.Append("{");
            json.AppendFormat("'data':'Tasks'");
            json.AppendFormat(",'icon':'projectTasks'");
            json.AppendFormat(",'state':'open'");
            json.Append(",'attr':{'id':'0-0','parentId':'0','name':'TaskRoot','currentId':'0', 'rel':'TaskRoot'}");
            if (rows.Count() > 0)
            {
                json.Append(",'children':[");
                for (int i = 0; i < rows.Count(); i++)
                {
                    json.Append(CreateNode(rows[i], pmmTasks, i, showOnlyTitle));
                }
                json.Append("]");
            }
            json.Append("}");
            return json.ToString();
        }

        private string CreateNode(DataRow row, DataTable pmmTasks, int count, bool showOnlyTitle)
        {
            StringBuilder json = new StringBuilder();
            if (count != 0)
            {
                json.Append(",");
            }
            json.Append("{");
            string title = row[DatabaseObjects.Columns.Title] != null ? Convert.ToString(row[DatabaseObjects.Columns.Title]) : string.Empty;
            title = UGITUtility.ReplaceInvalidCharsInURL(title);
            DateTime dueDate = DateTime.MaxValue;
            if (!(row[DatabaseObjects.Columns.DueDate] is DBNull))
                Convert.ToDateTime(row[DatabaseObjects.Columns.DueDate]);
            string background = string.Empty;
            if (dueDate.Date < DateTime.Now.Date && Convert.ToString(row[DatabaseObjects.Columns.Status]) != Constants.Completed)
            {
                background = "style=background:#FFAAAA;";
            }

            string treeTitle = string.Empty;
            if (showOnlyTitle)
                treeTitle = string.Format("<span><b>{0}</b></span>", title);
            else
                treeTitle = string.Format("<span><b>{0}</b> - <b>{1:MMM-d-yyyy}</b> to <b {3}>{2:MMM-d-yyyy}</b></span>", title, row[DatabaseObjects.Columns.StartDate], row[DatabaseObjects.Columns.DueDate], background);

            json.AppendFormat("'data':'{0}'", treeTitle);
            string type = "Task";
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.TaskBehaviour) && Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]) != string.Empty)
            {
                json.AppendFormat(",'icon':'{0}'", Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]));
                type = Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]);
            }
            else
            {
                json.AppendFormat(",'icon':'{0}'", "Task");
            }
            json.AppendFormat(",'attr':{0}'id':'{4}-{3}','parentid':'{3}','title':'{5}', 'startdate':'{6:M/d/yyyy}', 'enddate':'{7:M/d/yyyy}' ,'currentid':'{4}','rel':'{2}'{1}", "{", "}", type, row[DatabaseObjects.Columns.ParentTask], row[DatabaseObjects.Columns.Id], title, row[DatabaseObjects.Columns.StartDate], row[DatabaseObjects.Columns.DueDate]);
            double id = -1;
            double.TryParse(row[DatabaseObjects.Columns.Id].ToString(), out id);
            DataRow[] rows = pmmTasks.AsEnumerable().Where(p => p.Field<int>(DatabaseObjects.Columns.ParentTask) == id).ToArray();
            if (rows.Count() > 0)
            {
                json.Append(",'children':[");
                for (int i = 0; i < rows.Count(); i++)
                {
                    json.Append(CreateNode(rows[i], pmmTasks, i, showOnlyTitle));
                }
                json.Append("]");
            }
            json.Append("}");
            return json.ToString();
        }

        //public static List<UGITAssignTo> GetUGITAssignPct(string strAssignToPct)
        //{
        //    List<UGITAssignTo> AssignToPctList = new List<UGITAssignTo>();
        //    if (!string.IsNullOrEmpty(strAssignToPct))
        //    {
        //        string[] assignPct = strAssignToPct.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (var item in assignPct)
        //        {
        //            string[] assignItem = item.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
        //            UGITAssignTo newAssignTo = new UGITAssignTo();
        //            newAssignTo.LoginName = Convert.ToString(assignItem[0]);
        //            UserProfile user = UserProfile.LoadByLoginName(newAssignTo.LoginName);
        //            newAssignTo.UserName = user != null ? user.Name : UserProfile.GetLoginNameWithoutClaim(newAssignTo.LoginName);
        //            if (assignItem.Length > 1)
        //                newAssignTo.Percentage = Convert.ToString(assignItem[1]);
        //            else
        //                newAssignTo.Percentage = "100";
        //            AssignToPctList.Add(newAssignTo);
        //        }
        //    }
        //    return AssignToPctList;
        //}

        #region Method to Update the records for current task in ScheduleActions List
        /// <summary>
        /// This method is used to Delete reminders from ScheduleActions List and then recreate them for current task.
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="svcRequestsListItem"></param>
        /// <param name="service"></param>
        /// <param name="web"></param>
        public static void UpdateScheduleActions(ApplicationContext context, List<UGITTask> taskList, bool needTaskReminder, Services service)
        {
            if (taskList == null || taskList.Count == 0 || service == null || string.IsNullOrEmpty(service.Reminders))
                return;
            ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(context);
            //List<SchedulerAction> scheduleActionsList = SPListHelper.GetSPList(DatabaseObjects.Lists.ScheduleActions, web);

            //if (scheduleActionsList == null)
            //    return;

            var taskReminders = taskList.Where(x => x.SubTaskType.ToLower() == "task").Select(z => z);
            if (taskReminders == null || taskReminders.Count() == 0)
                return;

            taskList = taskReminders.ToList();

            #region Delete all records for current task from ScheduledActions list

            var currentTaskIDs = taskList.Where(x => x.ID != 0).Select(x => x.ID).ToList();
            string query = $"{DatabaseObjects.Columns.TicketId} IN ('{string.Join("','", currentTaskIDs.ToArray())}')";
            List<SchedulerAction> scheduleActionsList = scheduleActionsManager.Load(query);
            scheduleActionsManager.Delete(scheduleActionsList);

            //string requiredQuery = string.Format("<In><FieldRef Name='{0}' /><Values>{1}</Values></In>", DatabaseObjects.Columns.TicketId, string.Join("", moduleInstDependencyList.Select(x => string.Format("<Value Type='Text'>{0}</Value>", x.ID))));
            //SPQuery sQuery = new SPQuery();
            //sQuery.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id);
            //sQuery.ViewFieldsOnly = true;
            //sQuery.Query = string.Format("<Where>{0}</Where>", requiredQuery);
            //SPListItemCollection resultCollection = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ScheduleActions, sQuery, web);

            //if (resultCollection != null && resultCollection.Count > 0)
            //{
            //    bool allowUnsafe = web.AllowUnsafeUpdates;
            //    web.AllowUnsafeUpdates = true;
            //    int count = resultCollection.Count;
            //    while (count > 0)
            //    {
            //        SPListItem item = resultCollection[0];
            //        if (item != null)
            //            item.Delete();
            //        count--;
            //    }
            //    web.AllowUnsafeUpdates = allowUnsafe;
            //}

            #endregion Delete all records for current task from ScheduledActions list

            if (!needTaskReminder)
                return;

            #region Add new record for current task into ScheduledActions list
            // DeSerialize task reminder xml
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(service.Reminders);
            TaskReminderProperties reminderProperties = new TaskReminderProperties();
            reminderProperties = (TaskReminderProperties)uHelper.DeSerializeAnObject(doc, reminderProperties);

            // Return if reminders are not set
            if (!reminderProperties.Reminder1 && !reminderProperties.Reminder2)
                return;

            foreach (UGITTask task in taskList)
            {
                if ((task.EndDate == null || task.EndDate == DateTime.MinValue) && task.DueDate != null && task.DueDate != DateTime.MinValue)
                    task.EndDate = task.DueDate;

                // Skip if TaskType is Ticket or EndDate i.e. DueDate is not available or if Task Status is OnHold/Canceled/Completed/Rejected
                if (task.EndDate == null || task.EndDate == DateTime.MinValue ||
                    (task.Status != Constants.InProgress && task.Status != Constants.Waiting && task.Status != Constants.Pending))
                    continue;

                // set mailTo email addresses i.e. the notification receiver and other email variables
                UserProfileManager userProfileManager = new UserProfileManager(context);
                string mailToAddresses = string.Empty;

                List<UserProfile> mailToUsersInfo = new List<UserProfile>();

                if (task.ApprovalStatus == TaskApprovalStatus.Pending)
                {
                    if (task.Approver != null)
                        mailToUsersInfo = userProfileManager.GetUserInfosById(task.Approver); // UserProfile.GetUserInfo(task.Approver, web);
                }
                else
                {
                    if (task.AssignedTo != null)
                        mailToUsersInfo = userProfileManager.GetUserInfosById(task.AssignedTo); //UserProfile.GetUserInfo(task.AssignedTo, web);
                }

                if (mailToUsersInfo.Count > 0)
                {
                    foreach (UserProfile mailToUser in mailToUsersInfo)
                    {
                        mailToAddresses = mailToUser.Email;

                        string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", task.ParentInstance, task.ID, "SVC");
                        string taskLink = "<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>" + "<a href='" + url + "'>" + task.Title + "</a>";

                        string emailSubject = string.Format("Service Task: {0}", task.ApprovalStatus == TaskApprovalStatus.Pending ? "Approval Needed" : "Assigned for Completion");
                        string emailBody = string.Empty;
                        string moduleName = uHelper.getModuleNameByTicketId(task.ParentInstance);
                        DataRow CurrentItem = Ticket.GetCurrentTicket(context, moduleName, task.ParentInstance);
                        string emailFooter = HttpUtility.HtmlDecode(uHelper.GetTicketDetailsForEmailFooter(context, CurrentItem, moduleName, true, false));
                        ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(context);
                        string greeting = configurationVariableManager.GetValue(ConfigConstants.Greeting);//uGITCache.GetConfigVariableValue("Greeting");
                        string signature = configurationVariableManager.GetValue(ConfigConstants.Signature);//uGITCache.GetConfigVariableValue("Signature");
                        emailBody = string.Format("The following task needs action:<br/> {0}", taskLink);
                        emailBody = string.Format(@"{0} <b>{1}</b><br/>
                                                {2}<br/><br/>
                                                {3}<br/><br/><br/>", greeting, mailToUser.UserName, emailBody, signature);

                        emailBody += emailFooter;


                        // Create a reminder if task reminder1 is checked
                        if (reminderProperties.Reminder1)
                        {
                            DateTime endDate = Convert.ToDateTime(task.EndDate);
                            DateTime reminderDate;
                            if (service.Use24x7Calendar)
                                reminderDate = reminderProperties.Reminder1Frequency == "After" ? endDate.AddMinutes(reminderProperties.Reminder1Duration) : endDate.AddMinutes(-reminderProperties.Reminder1Duration);
                            else
                                reminderDate = uHelper.GetWorkingEndDate(context, endDate, reminderProperties.Reminder1Frequency == "After" ? +reminderProperties.Reminder1Duration : -reminderProperties.Reminder1Duration);

                            if (reminderDate >= DateTime.Today) // Only set reminder if date not already past
                            {
                                SchedulerAction scheduleActionsListItems = new SchedulerAction();
                                scheduleActionsListItems.ActionType = ScheduleActionType.Reminder.ToString();
                                scheduleActionsListItems.TicketId = Convert.ToString(task.ID);
                                scheduleActionsListItems.Title = task.Title;
                                scheduleActionsListItems.StartTime = reminderDate;
                                scheduleActionsListItems.EmailIDTo = mailToAddresses;
                                scheduleActionsListItems.MailSubject = emailSubject;
                                scheduleActionsListItems.EmailBody = emailBody;
                                scheduleActionsListItems.Recurring = false;
                                scheduleActionsListItems.ListName = DatabaseObjects.Tables.ModuleTasks;
                                scheduleActionsManager.Update(scheduleActionsListItems);
                            }
                        }

                        // Create a reminder if task reminder2 is checked
                        if (reminderProperties.Reminder2)
                        {
                            DateTime endDate = Convert.ToDateTime(task.EndDate);
                            DateTime reminderDate;
                            if (service.Use24x7Calendar)
                                reminderDate = reminderProperties.Reminder2Frequency == "After" ? endDate.AddMinutes(reminderProperties.Reminder2Duration) : endDate.AddMinutes(-reminderProperties.Reminder2Duration);
                            else
                                reminderDate = uHelper.GetWorkingEndDate(context, endDate, reminderProperties.Reminder2Frequency == "After" ? +reminderProperties.Reminder2Duration : -reminderProperties.Reminder2Duration);

                            if (reminderDate >= DateTime.Today) // Only set reminder if date not already past
                            {
                                SchedulerAction scheduleActionsListItems = new SchedulerAction();
                                scheduleActionsListItems.ActionType = ScheduleActionType.Reminder.ToString();
                                scheduleActionsListItems.TicketId = Convert.ToString(task.ID);
                                scheduleActionsListItems.Title = task.Title;
                                scheduleActionsListItems.StartTime = reminderDate;
                                scheduleActionsListItems.EmailIDTo = mailToAddresses;
                                scheduleActionsListItems.MailSubject = emailSubject;
                                scheduleActionsListItems.EmailBody = emailBody;
                                scheduleActionsListItems.Recurring = false;
                                scheduleActionsListItems.ListName = DatabaseObjects.Tables.ModuleTasks;
                                scheduleActionsManager.Update(scheduleActionsListItems);
                            }
                        }
                    }
                }
            }
            #endregion Add new record for current task into ScheduledActions list
        }
        #endregion Method to Update the records for current task in ScheduleActions List

        public void UpdateProjectTask(DataRow selectedSprintItem)
        {
            List<UGITTask> ptasks;
            DataRow project = null;

            int numberOfTaskChanges = 0;
            int projectLookup = Convert.ToInt32(selectedSprintItem[DatabaseObjects.Columns.TicketPMMIdLookup]);
            ptasks = LoadByProjectID("PMM", projectLookup); //UGITTaskHelper.LoadByProjectID(_context, "PMM", projectLookup);
            UGITTask pTask = ptasks.FirstOrDefault(x => x.SprintLookup == Convert.ToInt32(selectedSprintItem[DatabaseObjects.Columns.Id]));
            ptasks = MapRelationalObjects(ptasks);

            if (pTask != null)
            {
                // SPFieldLookupValue projectLookup = new SPFieldLookupValue(Convert.ToString(selectedSprintItem[DatabaseObjects.Columns.TicketPMMIdLookup]));
                project = Ticket.GetCurrentTicket(_context, "PMM", Convert.ToString(projectLookup));
                // ptasks = UGITTask.MapRelationalObjects(ptasks);

                UpdateTaskToSprintItem(selectedSprintItem, pTask);

                //Marks all child task as complete
                PropagateTaskEffect(ref ptasks, pTask);

                //Calculates project's startdate, enddate, pctcomplete, duration,  DaysToComplete, nextactivity and nextmilestone.
                CalculateProjectStartEndDate("PMM", ptasks, project);

                //project.UpdateOverwriteVersion();
                Ticket tkt = new Ticket(_context, "PMM");
                tkt.CommitChanges(project);

                //Gets count of tasks which are effected in this action
                numberOfTaskChanges = ptasks.Where(x => x.Changes).Count();
                //SaveTasks(_context, ref ptasks, "PMM");
                SaveTasks(ref ptasks);

                //TaskCache.ReloadProjectTasks("PMM", projectLookup.LookupValue); // cache is not implemented so not using right now
            }


            //List<UGITTask> ptasks;
            //SPListItem project = null;

            //int numberOfTaskChanges = 0;
            //SPFieldLookupValue projectLookup = new SPFieldLookupValue(Convert.ToString(selectedSprintItem[DatabaseObjects.Columns.TicketPMMIdLookup]));
            //ptasks = UGITTaskHelper.LoadByProjectID(SPContext.Current.Web, "PMM", projectLookup.LookupValue);
            //UGITTask pTask = ptasks.FirstOrDefault(x => x.SprintLookup != null && x.SprintLookup.LookupId == Convert.ToInt32(selectedSprintItem[DatabaseObjects.Columns.Id]));
            //ptasks = UGITTask.MapRelationalObjects(ptasks);

            //if (pTask != null)
            //{
            //    // SPFieldLookupValue projectLookup = new SPFieldLookupValue(Convert.ToString(selectedSprintItem[DatabaseObjects.Columns.TicketPMMIdLookup]));
            //    project = Ticket.getCurrentTicket("PMM", projectLookup.LookupValue);
            //    // ptasks = UGITTask.MapRelationalObjects(ptasks);

            //    UpdateTaskToSprintItem(selectedSprintItem, pTask);

            //    //Marks all child task as complete
            //    UGITTask.PropagateTaskEffect(ref ptasks, pTask);

            //    //Calculates project's startdate, enddate, pctcomplete, duration,  DaysToComplete, nextactivity and nextmilestone.
            //    UGITTaskHelper.CalculateProjectStartEndDate("PMM", ptasks, project);
            //    project.UpdateOverwriteVersion();

            //    //Gets count of tasks which are effected in this action
            //    numberOfTaskChanges = ptasks.Where(x => x.Changes).Count();
            //    UGITTaskHelper.SaveTasks(SPContext.Current.Web, ref ptasks, "PMM");

            //    TaskCache.ReloadProjectTasks("PMM", projectLookup.LookupValue);
            //}
        }

        public void UpdateTaskToSprintItem(DataRow selectedSprintItem, UGITTask pTask)
        {
            pTask.StartDate = Convert.ToDateTime(selectedSprintItem[DatabaseObjects.Columns.UGITStartDate]);
            pTask.DueDate = Convert.ToDateTime(selectedSprintItem[DatabaseObjects.Columns.UGITEndDate]);
            pTask.PercentComplete = Convert.ToDouble(selectedSprintItem[DatabaseObjects.Columns.PercentComplete]);
            pTask.EstimatedHours = Math.Round(Convert.ToDouble(selectedSprintItem[DatabaseObjects.Columns.TaskEstimatedHours]), 2);
            pTask.EstimatedRemainingHours = Math.Round(Convert.ToDouble(selectedSprintItem[DatabaseObjects.Columns.RemainingHours]), 2);
            pTask.ActualHours = Math.Round((Convert.ToDouble(selectedSprintItem[DatabaseObjects.Columns.TaskEstimatedHours]) - Convert.ToDouble(selectedSprintItem[DatabaseObjects.Columns.RemainingHours])), 2);
            pTask.Changes = true;
        }
        public void UpdateTaskToSprintItem(Sprint selectedSprintItem, UGITTask pTask)
        {
            pTask.StartDate = UGITUtility.StringToDateTime(selectedSprintItem.StartDate);
            pTask.DueDate = UGITUtility.StringToDateTime(selectedSprintItem.EndDate);
            pTask.PercentComplete = UGITUtility.StringToDouble(selectedSprintItem.PercentComplete);
            pTask.EstimatedHours = Math.Round(UGITUtility.StringToDouble(selectedSprintItem.TaskEstimatedHours), 2);
            pTask.EstimatedRemainingHours = Math.Round(UGITUtility.StringToDouble(selectedSprintItem.RemainingHours), 2);
            pTask.ActualHours = Math.Round((UGITUtility.StringToDouble(selectedSprintItem.TaskEstimatedHours) - UGITUtility.StringToDouble(selectedSprintItem.RemainingHours)), 2);
            pTask.Changes = true;
        }
        public bool IsModuleTasks(string modulename)
        {
            if (modulename != ModuleNames.TSK &&
               modulename != ModuleNames.PMM &&
               modulename != ModuleNames.NPR &&
               modulename != ModuleNames.SVC &&
               modulename != "SVCConfig" &&
               modulename != Constants.PMMIssue &&
               modulename != Constants.ExitCriteria &&
               modulename != Constants.VendorIssue &&
               modulename != Constants.VendorRisks)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Auto Calculate project Monitor state "On Time"
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="ptasks"></param>
        /// <param name="project"></param>

        public float AutoCalculateProjectMonitorStateOnTime(DataRow project, ApplicationContext context)
        {
            string ticketId = UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketId]);
            string moduleName = uHelper.getModuleNameByTicketId(ticketId);
            List<UGITTask> ptasks = LoadByProjectID(moduleName, ticketId);
            ptasks = MapRelationalObjects(ptasks);
            float oldProjectScore = 0;
            float.TryParse(UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketProjectScore]), out oldProjectScore);
            AutoCalculateProjectMonitorStateOnTime(moduleName, ptasks, project, context);
            float projectScore = (float)UGITUtility.StringToDouble(project[DatabaseObjects.Columns.TicketProjectScore]);
            if (oldProjectScore != projectScore)
            {
                Ticket ticket = new Ticket(_context, moduleName, _context.CurrentUser);
                ticket.CommitChanges(project);
            }
            return projectScore;
        }

        private void AutoCalculateProjectMonitorStateOnTime(string moduleName, List<UGITTask> ptasks, DataRow project, ApplicationContext _context)
        {
            string ticketId = UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketId]);
            int projectID = UGITUtility.StringToInt(project[DatabaseObjects.Columns.ID]);

            if (moduleName == ModuleNames.PMM)
            {
                ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(_context);
                ModuleMonitor moduleMonitor = moduleMonitorManager.Load(x => x.TenantID == _context.TenantID && x.MonitorName.Equals("On Time") && x.ModuleNameLookup == moduleName).FirstOrDefault();
                long onTimeMonitorId = moduleMonitor.ID;
                ProjectMonitorStateManager projectMonitorStateManager = new ProjectMonitorStateManager(_context);
                ProjectMonitorState projectMonitorState = projectMonitorStateManager.Load(x => x.TenantID == _context.TenantID && x.TicketId == ticketId && x.ModuleMonitorNameLookup == onTimeMonitorId).FirstOrDefault();

                if (projectMonitorState != null)
                {
                    if (!UGITUtility.StringToBoolean(projectMonitorState.AutoCalculate))
                        return;

                    ModuleMonitorOptionManager moduleMonitorOptionManager = new ModuleMonitorOptionManager(_context);
                    List<ModuleMonitorOption> moduleMonitorOptions = moduleMonitorOptionManager.Load(x => x.TenantID == _context.TenantID && x.ModuleMonitorNameLookup == onTimeMonitorId);
                    double scheduleVariance = 0;

                    if (ptasks != null && ptasks.Count > 0)
                        scheduleVariance = CalculatePlannedCompletedPercentage(moduleName, ptasks, project);

                    ModuleMonitorOption moduleMonitorOption = null;
                    if (scheduleVariance < 5)
                        moduleMonitorOption = moduleMonitorOptions.FirstOrDefault(x => x.ModuleMonitorMultiplier == 100);
                    else if (scheduleVariance >= 5 && scheduleVariance <= 15)
                        moduleMonitorOption = moduleMonitorOptions.FirstOrDefault(x => x.ModuleMonitorMultiplier == 50);
                    else if (scheduleVariance > 15)
                        moduleMonitorOption = moduleMonitorOptions.FirstOrDefault(x => x.ModuleMonitorMultiplier == 0);

                    long oldMonitorValue = 0;
                    if (projectMonitorState.ModuleMonitorOptionIdLookup > 0)//monitorState[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] != null
                    {
                        oldMonitorValue = projectMonitorState.ModuleMonitorOptionIdLookup;
                    }
                    long newMonitorValue = moduleMonitorOption.ID;
                    if (oldMonitorValue != newMonitorValue)
                    {
                        projectMonitorState.ModuleMonitorOptionIdLookup = newMonitorValue;
                        projectMonitorStateManager.Update(projectMonitorState);
                        // Roll up to total project score
                        UpdateProjectScore(project, _context);
                    }
                }
            }
        }

        //update the score...
        public float UpdateProjectScore(DataRow project, ApplicationContext _context)
        {
            string ticketId = UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketId]);
            ProjectMonitorStateManager projectMonitorStateManager = new ProjectMonitorStateManager(_context);
            List<ProjectMonitorState> projectMonitorStates = projectMonitorStateManager.Load(x => x.TenantID == _context.TenantID && x.TicketId == ticketId);
            if (projectMonitorStates == null || projectMonitorStates.Count == 0)
            {
                ULog.WriteLog("ERROR: No monitor state values found!");
                return 0;
            }

            float overallScore = 0;
            float totalWeight = 0;
            float monitorWeightFloat = 0;
            float pmmriskscore = 0;
            bool updateRiskScore = false;
            long selectedOption = 0;

            ModuleMonitorOptionManager _moduleMonitorOptionManager = new ModuleMonitorOptionManager(_context);
            ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(_context);
            foreach (ProjectMonitorState _projectMonitorState in projectMonitorStates)
            {
                float.TryParse(UGITUtility.ObjectToString(_projectMonitorState.ProjectMonitorWeight), out monitorWeightFloat);
                totalWeight += monitorWeightFloat;
                selectedOption = _projectMonitorState.ModuleMonitorOptionIdLookup;

                ModuleMonitorOption moduleMonitorOption = _moduleMonitorOptionManager.LoadByID(selectedOption);
                float multiplierOS = moduleMonitorOption.ModuleMonitorMultiplier;
                float scoreOS = monitorWeightFloat * (multiplierOS / 100);
                overallScore += scoreOS;


                ModuleMonitor moduleMonitor = moduleMonitorManager.LoadByID(_projectMonitorState.ModuleMonitorNameLookup);
                if (moduleMonitor != null && moduleMonitor.MonitorName == "Risk Level")
                {
                    updateRiskScore = true;
                    pmmriskscore = scoreOS;
                }
            }

            overallScore = (float)Math.Round(overallScore * 100 / totalWeight, 0);
            project[DatabaseObjects.Columns.TicketProjectScore] = overallScore;
            if (updateRiskScore)
                project[DatabaseObjects.Columns.TicketRiskScore] = Math.Round(100 - pmmriskscore);

            return overallScore;
        }

        /// <summary>
        /// Calculates Planned project completion percentage
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="tasks"></param>
        /// <param name="pItem"></param>
        public double CalculatePlannedCompletedPercentage(string moduleName, List<UGITTask> tasks, DataRow pItem)
        {
            double completionVariance = 0.0;

            if (moduleName != "PMM" && moduleName != "TSK" && moduleName != "NPR")
                return completionVariance;

            if (tasks == null || tasks.Count == 0)
                return completionVariance;

            DateTime minDate = tasks.Min(x => x.StartDate);
            DateTime maxDate = tasks.Max(x => x.DueDate);

            List<UGITTask> rootTasks = tasks.Where(x => x.ParentTaskID == 0).ToList();
            List<UGITTask> leafTasks = tasks.Where(x => x.ChildCount == 0).ToList();

            double totalTaskEsimatedHr = leafTasks.Sum(x => x.EstimatedHours);
            double totalTaskDuration = rootTasks.Sum(x => x.Duration);
            double plannedPctComplete = 0;

            if (UGITUtility.StringToBoolean(objConfigurationVariableHelper.GetValue(ConfigConstants.ProjectCompletionUsingEstHours)) && totalTaskEsimatedHr > 0)
            {
                var taskstilltoday = leafTasks.Where(x => x.DueDate.Date <= DateTime.Now.Date);
                double plannedCompletedTaskEstimatedHr = taskstilltoday.Sum(x => x.EstimatedHours); // +(inProcessTaskWorkingdays * uHelper.GetWorkingHoursInADay(spWeb));
                plannedPctComplete = (plannedCompletedTaskEstimatedHr / totalTaskEsimatedHr);
            }
            else
            {
                var taskstilltoday = leafTasks.Where(x => x.DueDate.Date <= DateTime.Now.Date);
                double plannedCompletedTaskDuration = taskstilltoday.Sum(x => x.Duration); // +inProcessTaskWorkingdays;
                if (totalTaskDuration > 0)
                {
                    plannedPctComplete = (plannedCompletedTaskDuration / totalTaskDuration);
                }
            }

            completionVariance = (plannedPctComplete - Convert.ToDouble(Convert.IsDBNull(pItem[DatabaseObjects.Columns.TicketPctComplete]))) * 100;

            return completionVariance;
        }

        public void AutoCalculateStartEndDateOfChild(List<UGITTask> ptasks, UGITTask task)
        {
            UGITTask prevTask = null;

            foreach (var ct in task.ChildTasks.OrderBy(x => x.ItemOrder))
            {
                var childtask = ptasks.FirstOrDefault(x => x.ID == ct.ID);
                if (prevTask != null)
                {
                    //DateTime[] StartandEnddates = UGITUtility.GetNewEndDateForExistingDuration(childtask.StartDate, childtask.DueDate, prevTask.DueDate, true);
                    //childtask.StartDate = StartandEnddates[0];
                    //childtask.DueDate = StartandEnddates[1];
                    //childtask.Changes = true;

                }
                else
                {
                    //DateTime[] StartandEnddates = UGITUtility.GetNewEndDateForExistingDuration(childtask.StartDate, childtask.DueDate, task.StartDate, false);
                    //childtask.StartDate = StartandEnddates[0];
                    //childtask.DueDate = StartandEnddates[1];
                    //childtask.Changes = true;
                }
                //UGITTask.PropagateTaskEffect(ref ptasks, childtask);
                prevTask = childtask;
            }
        }

        public void AutoAdjustSchedules(ref List<UGITTask> tasks, string moduleName, string publicTicketId)
        {
            foreach (UGITTask ctask in tasks)
            {
                if (ctask.PredecessorTasks != null)
                {
                    List<UGITTask> predTasks = ctask.PredecessorTasks;
                    foreach (UGITTask predtask in predTasks)
                    {

                        if ((ctask.ParentTaskID == predtask.ParentTaskID) || (ctask.PredecessorTasks != null) ||
                            (ctask.ParentTask != null && ctask.ParentTaskID != predtask.ParentTaskID &&
                            (ctask.ParentTask.PredecessorTasks == null || ctask.ParentTask.PredecessorTasks.Count == 0)))
                        {
                            string nextworkingDateNTime = uHelper.GetNextWorkingDateAndTime(_context, predtask.DueDate);
                            string[] nextworkingStartNEndTime = UGITUtility.SplitString(nextworkingDateNTime, Constants.Separator);
                            DateTime nextStartTime = Convert.ToDateTime(nextworkingStartNEndTime[0]);

                            TimeSpan tsdiff = ctask.StartDate.Subtract(nextStartTime);
                            if (tsdiff.Days > 0)
                            {
                                DateTime[] dates = uHelper.GetNewEndDateForExistingDuration(_context, ctask.StartDate, ctask.DueDate, predtask.DueDate, true);

                                ctask.StartDate = dates[0];
                                ctask.DueDate = dates[1];
                                ctask.Changes = true;
                                PropagateTaskEffect(ref tasks, ctask);
                            }
                        }
                        else if (ctask.ParentTask != null)
                        {
                            DateTime[] dates = uHelper.GetNewEndDateForExistingDuration(_context, ctask.StartDate, ctask.DueDate, ctask.ParentTask.StartDate, false);
                            ctask.StartDate = dates[0];
                            ctask.DueDate = dates[1];
                            ctask.Changes = true;
                            PropagateTaskEffect(ref tasks, ctask);
                        }

                    }
                }
                else
                {
                    if (ctask.ParentTask != null)
                    {
                        //Added by mudassir 27 feb 2020
                        if (ctask.StartDate != DateTime.MinValue.Date && ctask.DueDate != DateTime.MaxValue.Date)
                        {
                            DateTime[] dates = uHelper.GetNewEndDateForExistingDuration(_context, ctask.StartDate, ctask.DueDate, ctask.ParentTask.StartDate, false);
                            ctask.StartDate = dates[0];
                            ctask.DueDate = dates[1];
                            ctask.Changes = true;
                            PropagateTaskEffect(ref tasks, ctask);
                        }
                        //

                    }
                }
            }

            SaveTasks(ref tasks, moduleName, publicTicketId);

        }

        internal void ClearCookies(System.Web.HttpRequest Request, System.Web.HttpResponse Response, string currentTicketPublicID)
        {
            var _closechildren = "closechildren" + currentTicketPublicID;
            var _openchildren = "openchildren" + currentTicketPublicID;
            var _selectedrowonpage = "selectedrowonpage" + currentTicketPublicID;
            //uHelper.DeleteCookie(Request, Response, _closechildren);
            //uHelper.DeleteCookie(Request, Response, _openchildren);
            //uHelper.DeleteCookie(Request, Response, _selectedrowonpage);
        }
        internal void MarkPMMTaskAsComplete(string TicketPublicId, string moduleName, DataRow ticketItem)
        {
            List<UGITTask> tasks = LoadByProjectID(moduleName, TicketPublicId);
            foreach (UGITTask task in tasks)
            {
                if (task.Status != Constants.Completed || task.PercentComplete != 100)
                {
                    task.Status = Constants.Completed;
                    task.PercentComplete = 100;
                    task.CompletionDate = DateTime.Now;
                    task.Changes = true;
                    //UGITTaskHelper.SaveTasks(ref tasks, moduleName);
                    //UGITTaskHelper.CalculateProjectStartEndDate(moduleName, tasks, ticketItem);
                    //ticketItem.UpdateOverwriteVersion();
                    //TaskCache.ReloadProjectTasks(moduleName, TicketPublicId);
                    //AgentJobHelper agentHelper = new AgentJobHelper(spWeb);
                    //agentHelper.DeleteTaskReminder(task.ID.ToString());
                }
            }
            SaveTasks(ref tasks, moduleName, TicketPublicId);
            CalculateProjectStartEndDate(moduleName, tasks, ticketItem);
            AgentJobHelper agentHelper = new AgentJobHelper(_context);
            agentHelper.DeleteTaskReminder(TicketPublicId);
        }

        /// <summary>
        /// Propogate changes to other task which are directly or indirectly related to current task
        /// </summary>
        /// <param name="tasks"></param>
        public void PropagateTaskEffect(ApplicationContext _context, ref List<UGITTask> tasks, UGITTask currentTask, bool IsPropagateTaskEffect, bool IsManageDateChanges)
        {
            //Propogate status change to child and parents
            //If status is completed then mark all child as complete
            //If parent of current task contains all completed task then mark it as complete otherwise mark according to following rules
            //{
            //  all childtasks = Not Started then parenttask = Not Started and %Complete =0
            //  any one childtask != Not Started or != Completed then parenttask = In process and %complete= according to total %complete
            //}
            if (IsPropagateTaskEffect)
                PropagateTaskStatusEffect(ref tasks, currentTask);

            // Find project startdate, enddate and workinghours in a day

            DateTime minDate = tasks.Min(x => x.StartDate);
            DateTime maxStartDate = tasks.Max(x => x.StartDate);

            // Compare minDate & maxStartDate to ignore the case of DateTime.MinValue if we have other tasks with start dates
            if (minDate == DateTime.MinValue && minDate != maxStartDate)
                minDate = tasks.Where(y => y.StartDate != DateTime.MinValue).Min(x => x.StartDate);

            DateTime maxDate = tasks.Max(x => x.DueDate);
            double projectDuration = uHelper.GetTotalWorkingDaysBetween(_context, minDate, maxDate);
            double workingHoursInADay = uHelper.GetWorkingHoursInADay(_context);

            //Manage Date according to child tasks
            if (IsManageDateChanges)
                ManageDateChanges(ref tasks, currentTask, workingHoursInADay);
        }

        /// <summary>
        /// Propogate changes to other task which are directly or indirectly related to current task
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="currentTask"></param>
        public void PropagateTaskEffect(ref List<UGITTask> tasks, UGITTask currentTask)
        {

            //Propogate status change to child and parents
            //If status is completed then mark all child as complete
            //If parent of current task contains all completed task then mark it as complete otherwise mark according to following rules
            //{
            //  all childtasks = Not Started then parenttask = Not Started and %Complete =0
            //  any one childtask != Not Started or != Completed then parenttask = In process and %complete= according to total %complete
            //}
            PropagateTaskStatusEffect(ref tasks, currentTask);


            //Finds project start and enddate and workinghours in a day.

            DateTime minDate = tasks.Min(x => x.StartDate);
            DateTime maxDate = tasks.Max(x => x.DueDate);
            //Added by mudassir 18 feb 2020
            //if (currentTask.StartDate != minDate)
            //{
            //    minDate = currentTask.StartDate;
            //}
            //else {
            //    minDate = currentTask.ParentTask.StartDate;

            //}


            //
            double projectDuration = uHelper.GetTotalWorkingDateBetween(_context, minDate, maxDate).Count;
            double workingHoursInADay = uHelper.GetWorkingHoursInADay(_context);

            //Manage Date according to child tasks
            ManageDateChanges(ref tasks, currentTask, workingHoursInADay);

        }

        /// <summary>
        /// Propogate status change to child and parents
        ///If status is completed then mark all child as complete
        ///If parent of current task contains all completed task then mark it as complete otherwise mark according to following rules
        ///{
        ///  all childtasks = Not Started then parenttask = Not Started and %Complete =0
        /// any one childtask != Not Started or != Completed then parenttask = In process and %complete= according to total %complete
        ///}
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="currentTask"></param>
        public void PropagateTaskStatusEffect(ref List<UGITTask> tasks, UGITTask currentTask)
        {
            //propogate status change to all child task
            PropagateStatusToChildTasks(currentTask);

            //propogate status change to parent tasks 
            PropagateStatusToParentTasks(ref tasks, currentTask);
        }

        /// <summary>
        /// Marks all child task as complete
        /// </summary>
        /// <param name="currentTask"></param>
        private void PropagateStatusToChildTasks(UGITTask currentTask)
        {
            if (currentTask.ChildTasks == null || currentTask.ChildTasks.Count <= 0)
            {
                return;
            }

            if (currentTask.Status == Constants.Completed)
            {
                foreach (UGITTask tTask in currentTask.ChildTasks)
                {
                    if (tTask.Status != Constants.Completed || tTask.PercentComplete != 100)
                    {
                        tTask.Status = Constants.Completed;
                        tTask.PercentComplete = 100;

                        tTask.Changes = true;
                    }
                    PropagateStatusToChildTasks(tTask);
                }
            }
            else if (currentTask.Status == Constants.NotStarted)
            {
                foreach (UGITTask tTask in currentTask.ChildTasks)
                {
                    if (tTask.Status != Constants.NotStarted || tTask.PercentComplete != 0)
                    {
                        tTask.Status = Constants.NotStarted;
                        tTask.PercentComplete = 0;
                        tTask.Changes = true;
                    }
                    PropagateStatusToChildTasks(tTask);
                }
            }
        }

        /// <summary>
        /// Marks parent task as complete if all parallel tasks of current are completed 
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="currentTask"></param>
        private void PropagateStatusToParentTasks(ref List<UGITTask> tasks, UGITTask currentTask)
        {
            UGITTask parentTask = tasks.FirstOrDefault(x => x.ID == currentTask.ParentTaskID);
            if (parentTask == null)
                return;
            if (parentTask.ID == 0 && currentTask.ParentTaskID == 0)
                return;

            //if (parentTask.ChildTasks.Where(x => x.Status == Constants.Completed).Count() == parentTask.ChildTasks.Count)
            if (parentTask.ChildTasks.Where(x => x.PercentComplete == 100.0).Count() == parentTask.ChildTasks.Count)
            {
                // 100 % - Complete
                parentTask.Status = Constants.Completed;
                parentTask.PercentComplete = 100;

                parentTask.Changes = true;
            }
            else if (parentTask.ChildTasks.Where(x => x.PercentComplete == 0.0).Count() == parentTask.ChildTasks.Count)
            {
                // 0 % - Not Started
                parentTask.Status = Constants.NotStarted;
                parentTask.PercentComplete = 0;
                parentTask.Changes = true;
            }
            else
            {
                // In Progress
                if (parentTask.Status == Constants.Completed || parentTask.Status == Constants.NotStarted)
                    parentTask.Status = Constants.InProgress;

                double pctComplete = 0;
                double totalTaskEsimatedHr = parentTask.ChildTasks.Sum(x => x.EstimatedHours);
                if (UGITUtility.StringToBoolean(objConfigurationVariableHelper.GetValue(ConfigConstants.ProjectCompletionUsingEstHours)) && totalTaskEsimatedHr > 0)
                {
                    double completedTaskEstimatedHr = parentTask.ChildTasks.Sum(x => (x.EstimatedHours * x.PercentComplete) / 100);
                    pctComplete = completedTaskEstimatedHr * 100 / totalTaskEsimatedHr;
                }
                else
                {
                    double totalTaskDuration = parentTask.ChildTasks.Sum(x => x.Duration);
                    if (totalTaskDuration > 0)
                    {
                        double completedTaskDuration = parentTask.ChildTasks.Sum(x => (x.Duration * x.PercentComplete) / 100);
                        pctComplete = completedTaskDuration * 100 / totalTaskDuration;
                    }
                }

                if (pctComplete > 99.9) // && pctComplete < 100)
                    parentTask.PercentComplete = 99.9; // Don't show 100% for In Progress tasks to account for child milestones
                else
                    parentTask.PercentComplete = Math.Round(pctComplete, 1, MidpointRounding.AwayFromZero);

                parentTask.Changes = true;
            }

            PropagateStatusToParentTasks(ref tasks, parentTask);
        }

        /// <summary>
        ///Manage dates of other task which directly or indirectly related to changed task;
        ///Following are the changes implemented due to change in date of current task
        ///1. Changes parent task dates so that all child tasks comes under it.
        ///2. Changes successor task date so that It start after dependent task
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="currentTask"></param>
        /// <param name="projectDuration"></param>
        /// <param name="workingHoursInADay"></param>
        private void ManageDateChanges(ref List<UGITTask> tasks, UGITTask currentTask, double workingHoursInADay)
        {
            currentTask.Duration = CalculateTaskDuration(currentTask);
            currentTask.Changes = true;

            List<UGITTask> taskStack = new List<UGITTask>();
            UpdateSuccessorAndParentDates(ref tasks, currentTask, workingHoursInADay, ref taskStack);
        }

        private void UpdateSuccessorAndParentDates(ref List<UGITTask> tasks, UGITTask currentTask, double workingHoursInADay, ref List<UGITTask> taskStack)
        {
            UpdateSuccessorDates(ref tasks, currentTask, workingHoursInADay, ref taskStack); // Must do this first!
            UpdateParentDates(ref tasks, currentTask, workingHoursInADay, ref taskStack);
        }

        public int CalculateTaskDuration(UGITTask task)
        {
            if (task.StartDate == task.DueDate && task.Behaviour == Constants.TaskType.Milestone)
                return 0;
            else
                return uHelper.GetTotalWorkingDateBetween(_context, task.StartDate, task.DueDate).Count;
        }

        public void CalculateDuration(ref List<UGITTask> tasks, bool updateParentDates = true)
        {
            List<UGITTask> childTasks = tasks.Where(x => x.ChildCount == 0).ToList();
            double workingHoursInADay = uHelper.GetWorkingHoursInADay(_context);
            foreach (UGITTask task in childTasks)
            {
                task.Duration = CalculateTaskDuration(task);
                task.Changes = true;

                if (updateParentDates)
                {
                    List<UGITTask> taskStack = new List<UGITTask>();
                    UpdateSuccessorAndParentDates(ref tasks, task, workingHoursInADay, ref taskStack);
                }
            }
        }


        /// <summary>
        ///Manage dates of other task which directly or indirectly related to changed task;
        ///Following are the changes implemented due to change in date of current task
        ///1. Changes successor task date so that It start after dependent task
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="currentTask"></param>
        /// <param name="projectDuration"></param>
        /// <param name="workingHoursInADay"></param>
        private void UpdateSuccessorDates(ref List<UGITTask> tasks, UGITTask currentTask, double workingHoursInADay, ref List<UGITTask> taskStack)
        {
            if (currentTask.SuccessorTasks == null || currentTask.SuccessorTasks.Count <= 0)
                return;

            // Add to stack so we can check for looping if we find it again
            if (!taskStack.Exists(x => x.ID == currentTask.ID))
                taskStack.Add(currentTask);

            foreach (UGITTask tTask in currentTask.SuccessorTasks)
            {
                //if successor start date is less than currenttask duedate then change successor date
                if (tTask.StartDate.Date <= currentTask.DueDate.Date)
                {
                    // Check for successor looping
                    if (taskStack.Exists(x => x.ID == tTask.ID))
                    {
                        // Found recursion, skip
                        // Log.WriteLog(string.Format("ManageSuccessorTasksDates: Recursion in {0} when updating successor task {1}: {2} ({3})",
                        //           tTask.ProjectLookup.LookupValue, tTask.ItemOrder, tTask.Title, tTask.ID),
                        // TraceSeverity.High, EventSeverity.Error);
                        continue;
                    }

                    // Recalculate start & end date based on working days for current duration & new start date
                    DateTime[] dates = uHelper.GetNewEndDateForExistingDuration(_context, tTask.StartDate, tTask.DueDate, currentTask.DueDate, true);
                    tTask.StartDate = dates[0];
                    tTask.DueDate = dates[1];

                    //Get new duration based on dates
                    tTask.Duration = uHelper.GetTotalWorkingDaysBetween(_context, dates[0], dates[1]);
                    tTask.Duration = CalculateTaskDuration(tTask);

                    //// Don't change estimated hours, this should be preserved even when dates move since they can be manually entered!
                    //double totalHours = workingDays * workingHoursInADay;
                    //tTask.EstimatedHours = totalHours;

                    tTask.Changes = true;

                    // Add to stack so we can check for looping if we find it again
                    taskStack.Add(tTask);

                    //Debug.WriteLine(string.Format("Updated successor task {0}: {1} ({2})", tTask.ItemOrder, tTask.Title, tTask.ID));

                    //UpdateSuccessorAndParentDates(ref tasks, tTask, workingHoursInADay, ref taskStack);
                }
            }
        }

        /// <summary>
        ///Manage dates of other task which directly or indirectly related to changed task;
        ///Following are the changes implemented due to change in date of current task
        ///1. Changes parent task dates so that all child tasks comes under it.
        ///NOTE: taskStack is used to prevent recursion in ManageSuccessorTasksDates, just pass-thru for this function
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="currentTask"></param>
        /// <param name="projectDuration"></param>
        /// <param name="workingHoursInADay"></param>
        /// <param name="taskStack"></param>
        private void UpdateParentDates(ref List<UGITTask> tasks, UGITTask currentTask, double workingHoursInADay, ref List<UGITTask> taskStack)
        {
            UGITTask parentTask = tasks.FirstOrDefault(x => x.ID == currentTask.ParentTaskID);
            if (parentTask == null || parentTask.ChildTasks == null || parentTask.ChildTasks.Count <= 0)
                return;

            if (!chkSaveTaskDate)
            {
                parentTask.StartDate = parentTask.ChildTasks.Min(x => x.StartDate);
                parentTask.DueDate = parentTask.ChildTasks.Max(x => x.DueDate);
            }

            if (!chkSaveTaskDate)
            {
                if (parentTask.ChildTasks.Min(x => x.StartDate) != DateTime.MinValue.Date)
                {
                    parentTask.StartDate = parentTask.ChildTasks.Min(x => x.StartDate);

                }
                if (parentTask.ChildTasks.Max(x => x.DueDate) != DateTime.MinValue.Date)
                {

                    parentTask.DueDate = parentTask.ChildTasks.Max(x => x.DueDate);
                }
            }

            //


            //
            parentTask.Duration = CalculateTaskDuration(parentTask);
            parentTask.EstimatedHours = Math.Round(parentTask.ChildTasks.Sum(x => x.EstimatedHours), 2);
            parentTask.ActualHours = parentTask.ChildTasks.Sum(x => x.ActualHours);
            parentTask.EstimatedRemainingHours = Math.Round(parentTask.ChildTasks.Sum(x => x.EstimatedRemainingHours), 2);
            parentTask.Changes = true;

            UpdateSuccessorAndParentDates(ref tasks, parentTask, workingHoursInADay, ref taskStack);
        }

        /// <summary>
        /// return right predecessors task from specified predecessor list
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="predecessors"></param>
        /// <returns></returns>
        public List<string> FilterPredecessors(List<UGITTask> tasks, List<string> predecessors, UGITTask currentTask)
        {
            List<string> lookups = new List<string>();
            if (tasks != null)
            {
            	//BTS-22-000819: ID changed to ItemOrder
                List<UGITTask> predecessorTasks = tasks.Where(x => predecessors.Contains(Convert.ToString(x.ItemOrder))).OrderBy(x => x.ParentTaskID).ToList();
                foreach (UGITTask task in predecessorTasks)
                {
                    //if (IsCorrectPredecessor(tasks, predecessorTasks, task, lookups)) ---> Commented as this sode is not doing anything.
                    //{
                        string lookup = Convert.ToString(task.ItemOrder);  // new SPFieldLookupValue(task.ID, task.Title);
                        lookups.Add(lookup);
                    //}
                }
            }
            return lookups;
        }

        /// <summary>
        /// Check currentPre task is a right predecessors or not
        /// Current parents and children and successors should not be the part of predecessors
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="predecessors"></param>
        /// <param name="currentPre"></param>
        /// <returns></returns>
        private bool IsCorrectPredecessor(List<UGITTask> tasks, List<UGITTask> predecessors, UGITTask currentPre, List<string> filteredPredecessors)
        {
            //List<UGITTask> dependents = GetParentsAndChildsOfCurrentTask(tasks, currentPre);
            //if (predecessors.Exists(x => dependents.Exists(y => y.ID == x.ID && y.ID != currentPre.ID) || (currentPre.SuccessorTasks != null && currentPre.SuccessorTasks.Exists(y => y.ID == x.ID))))
            //{
            //    if (filteredPredecessors.Exists(x => dependents.Exists(y => y.ID == x.LookupId) || (currentPre.SuccessorTasks != null && currentPre.SuccessorTasks.Exists(y => y.ID == x.LookupId))))
            //        return false;
            //}

            return true;
        }

        /// <summary>
        /// To Get All Successors
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="successors"></param>
        public void GetAllSuccessors(UGITTask currentTask, ref List<UGITTask> successors)
        {
            if (currentTask != null && currentTask.SuccessorTasks != null && currentTask.SuccessorTasks.Count > 0)
            {
                foreach (UGITTask tTask in currentTask.SuccessorTasks)
                {
                    if (!successors.Exists(x => x.ID == tTask.ID))
                    {
                        successors.Add(tTask);
                        GetAllSuccessors(tTask, ref successors);
                    }
                }
            }
        }

        public List<UGITAssignTo> GetUGITAssignPct(string strAssignToPct)
        {
            List<UGITAssignTo> AssignToPctList = new List<UGITAssignTo>();
            if (!string.IsNullOrEmpty(strAssignToPct))
            {
                string[] assignPct = strAssignToPct.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in assignPct)
                {
                    string[] assignItem = item.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                    UGITAssignTo newAssignTo = new UGITAssignTo();
                    UserProfile assignUser = dbContext.UserManager.GetUserInfoByIdOrName(Convert.ToString(assignItem[0]));
                    if (assignUser != null)
                    {
                        newAssignTo.ID = assignUser.Id;
                        newAssignTo.LoginName = assignUser.UserName;
                        newAssignTo.UserName = assignUser.Name;
                        if (assignItem.Length > 1)
                            newAssignTo.Percentage = Convert.ToString(assignItem[1]);
                        else
                            newAssignTo.Percentage = "100";

                        AssignToPctList.Add(newAssignTo);
                    }
                }
            }
            return AssignToPctList;
        }


        public List<UGITAssignTo> GetUGITTaskPct(ApplicationContext context, string strUGITAssignToPct)
        {
            List<UGITAssignTo> AssignToPctList = new List<UGITAssignTo>();
            if (!string.IsNullOrEmpty(strUGITAssignToPct))
            {
                string[] assignPct = strUGITAssignToPct.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in assignPct)
                {
                    string[] assignItem = item.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                    UGITAssignTo newAssignTo = new UGITAssignTo();
                    //newAssignTo.LoginName = Convert.ToString(assignItem[0]);
                    //UserProfile user = UserProfile.LoadByLoginName(newAssignTo.LoginName);
                    //newAssignTo.UserName = user != null ? user.Name : UserProfile.GetLoginNameWithoutClaim(newAssignTo.LoginName);
                    UserProfileManager userManager = new Manager.UserProfileManager(context);
                    UserProfile user = userManager.GetUserByUserName(Convert.ToString(assignItem[0]));
                    if (user != null)
                    {
                        newAssignTo.UserName = user.Name;
                        newAssignTo.LoginName = user.UserName;
                    }

                    if (assignItem.Length > 1)
                        newAssignTo.Percentage = Convert.ToString(assignItem[1]);
                    else
                        newAssignTo.Percentage = "100";
                    AssignToPctList.Add(newAssignTo);
                }
            }
            return AssignToPctList;
        }


        public List<UGITAssignTo> GetUGITAssignPctExport(string strAssignToPct)
        {
            List<UGITAssignTo> AssignToPctList = new List<UGITAssignTo>();
            if (!string.IsNullOrEmpty(strAssignToPct))
            {
                try
                {
                    string[] assignPct = strAssignToPct.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in assignPct)
                    {
                        string[] assignItem = item.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                        UGITAssignTo newAssignTo = new UGITAssignTo();
                        UserProfile user = dbContext.UserManager.GetUserById(assignItem[0]);
                        if (user != null)
                            newAssignTo.LoginName = Convert.ToString(assignItem[0]);
                        //UserProfile user = _context.UserManager.GetUserByUserName(newAssignTo.LoginName);
                        if (user != null)
                        {
                            newAssignTo.ID = user.Id;
                            newAssignTo.LoginName = user.UserName;
                            //newAssignTo.UserName = user != null ? user.Name : UserProfileManager.GetLoginNameWithoutClaim(newAssignTo.LoginName);
                            if (assignItem.Length > 1)
                            {

                                newAssignTo.Percentage = Convert.ToString(assignItem[1]);
                            }
                            else
                            {
                                newAssignTo.Percentage = "100";
                            }
                        }
                        AssignToPctList.Add(newAssignTo);
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            return AssignToPctList;
        }

        public void PlacedTaskAt(UGITTask currentTask, UGITTask sublingTask, ref List<UGITTask> tasks)
        {
            int itemOrder = sublingTask.ItemOrder;
            currentTask.ItemOrder = itemOrder + 1;
            foreach (UGITTask task in tasks)
            {
                if (currentTask.ID != task.ID && task.ItemOrder > itemOrder)
                {
                    task.ItemOrder += 1;
                }
            }
            tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
        }

        //Add Child task in current task
        public void AddChildTask(long parentTaskID, long childTaskID, ref List<UGITTask> tasks)
        {
            UGITTask childTask = tasks.FirstOrDefault(x => x.ID == childTaskID);
            UGITTask parentTask = tasks.FirstOrDefault(x => x.ID == parentTaskID);

            List<UGITTask> parentTasks = GetParentsOfCurrentTask(tasks, parentTask);
            parentTasks.Add(parentTask);

            //if parent and parents of parents containing childtask as predecessor then remove. because it create dependency loop.
            if (childTask != null)
            {
                if (!string.IsNullOrEmpty(childTask.Predecessors))
                {
                    if (childTask.Predecessors != null && UGITUtility.SplitString(childTask.Predecessors, Constants.Separator6).Count() > 0)
                    {
                        List<UGITTask> pExistAsPreds = childTask.PredecessorTasks.Where(x => parentTasks.Exists(y => y != null && y.ID == x.ID)).ToList();
                        if (pExistAsPreds.Count > 0)
                        {
                            foreach (UGITTask ut in pExistAsPreds)
                            {
                                ut.Changes = true;
                                //childTask.Predecessors.RemoveAll(x => x.LookupId == ut.ID);
                                childTask.PredecessorTasks.RemoveAll(x => x.ID == ut.ID);
                                ut.SuccessorTasks.RemoveAll(x => x.ID == childTask.ID);
                            }
                        }
                    }

                    //If childtask is ref as a predecessor in parent and parents of parent then remove it becuase it create dependency look.
                    List<UGITTask> cExistAsPreds = parentTasks.Where(x => x != null && x.Predecessors != null && x.Predecessors.Contains(Convert.ToString(childTask.ID))).ToList();
                    if (cExistAsPreds.Count > 0)
                    {
                        foreach (UGITTask ut in cExistAsPreds)
                        {
                            ut.Changes = true;
                            //ut.Predecessors.RemoveAll(y => y.LookupId == childTask.ID);
                            ut.PredecessorTasks.RemoveAll(x => x.ID == childTask.ID);
                            childTask.SuccessorTasks.RemoveAll(x => x.ID == ut.ID);
                        }
                    }
                }
            }
            if (childTask != null)
            {
                if (childTask.ParentTaskID > 0 && childTask.ParentTask != null)
                {
                    childTask.ParentTask.ChildTasks.Remove(childTask);
                    childTask.ParentTask.ChildCount = childTask.ParentTask.ChildTasks.Count;
                    childTask.ParentTask.Changes = true;
                }

                if (parentTask != null)
                {
                    if (parentTask.ChildTasks == null)
                        parentTask.ChildTasks = new List<UGITTask>();

                    if (!parentTask.ChildTasks.Exists(x => x.ID == childTask.ID))
                    {
                        childTask.ParentTaskID = parentTask.ID;
                        childTask.ParentTask = parentTask;
                        childTask.Level = parentTask.Level + 1;
                        childTask.Changes = true;
                        parentTask.ChildTasks.Add(childTask);
                        parentTask.ChildCount = parentTask.ChildTasks.Count;
                        parentTask.Changes = true;
                        parentTask.EstimatedHours = Math.Round(parentTask.ChildTasks.Sum(x => x.EstimatedHours), 2);
                        RelevelChildTasks(childTask, false);
                    }
                }
                else
                {
                    childTask.ParentTaskID = 0;
                    childTask.ParentTask = null;
                    childTask.Level = 0;
                    childTask.Changes = true;
                    RelevelChildTasks(childTask, false);
                }
            }
        }

        /// <summary>
        /// Remove Child task from current task
        /// </summary>
        /// <param name="childTaskID"></param>
        public void RemoveChildTask(long parentTaskID, int childTaskID, ref List<UGITTask> tasks)
        {
            UGITTask parentTask = tasks.FirstOrDefault(x => x.ID == parentTaskID);
            if (parentTask.ChildTasks == null)
            {
                parentTask.ChildCount = 0;
                return;
            }

            UGITTask task = parentTask.ChildTasks.FirstOrDefault(x => x.ID == childTaskID);
            if (task != null)
            {
                parentTask.ChildTasks.Remove(task);
                parentTask.ChildCount = parentTask.ChildTasks.Count;
                if (parentTask.ChildTasks.Count > 0)
                    parentTask.EstimatedHours = Math.Round(parentTask.ChildTasks.Sum(x => x.EstimatedHours), 2);

                task.Level = parentTask.Level;
                task.ParentTaskID = parentTask.ParentTaskID;

                AddChildTask(task.ParentTaskID, task.ID, ref tasks);
            }
        }

        /// <summary>
        /// Relevel all child tasks
        /// </summary>
        /// <param name="task"></param>
        /// <param name="reduceLevel"></param>
        private void RelevelChildTasks(UGITTask task, bool reduceLevel)
        {
            if (task.ChildCount > 0)
            {
                foreach (UGITTask tTask in task.ChildTasks)
                {
                    tTask.Level = task.Level + 1;
                    tTask.Changes = true;
                    RelevelChildTasks(tTask, reduceLevel);
                }
            }
        }

        /// <summary>
        /// Get all the parents means parent of parent as well
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="currentTask"></param>
        /// <returns></returns>
        public List<UGITTask> GetParentsOfCurrentTask(List<UGITTask> tasks, UGITTask currentTask)
        {
            List<UGITTask> tTasks = new List<UGITTask>();
            if (currentTask != null)
            {
                PullParentTaskOfCurrentTask(ref tTasks, currentTask);
            }
            return tTasks;
        }

        /// <summary>
        /// Pull all the parents means parent of parent as well
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="task"></param>
        private void PullParentTaskOfCurrentTask(ref List<UGITTask> tasks, UGITTask task)
        {
            if (task != null && task.ParentTask != null && !tasks.Contains(task.ParentTask))
            {
                tasks.Add(task.ParentTask);
                PullParentTaskOfCurrentTask(ref tasks, task.ParentTask);
            }
        }

        public string GetCopyOfTitle(List<UGITTask> pTasks, string p, UGITTask ptask)
        {
            if (pTasks.Exists(x => x.ParentTaskID == ptask.ParentTaskID && x.Level == ptask.Level && x.Title == p.Trim()))
            {
                string newtitle = "";
                if (Regex.IsMatch(p, @"\d+"))
                {
                    string titlesuffix = Regex.Match(p, @"\d+").Value;
                    string titlewithoutsuffix = p.Replace(titlesuffix, "");
                    if (Regex.IsMatch(titlesuffix, @"\d+"))
                    {
                        Match number = Regex.Match(titlesuffix, @"\d+");
                        newtitle = titlewithoutsuffix.Trim() + string.Format(" {0}", Convert.ToInt32(number.Value) + 1);
                    }
                    return GetCopyOfTitle(pTasks, newtitle, ptask);
                }
                else
                {
                    newtitle = p + string.Format(" {0}", 2);
                }
                return GetCopyOfTitle(pTasks, newtitle, ptask);
            }
            return p;
        }
        public void CalculateEstimatedHours(ref List<UGITTask> tasks)
        {
            CalculateEstimatedHours(ref tasks, false);
        }
        public void CalculateEstimatedHours(ref List<UGITTask> tasks, bool forUnassignedTasksOnly)
        {
            tasks = MapRelationalObjects(tasks);
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(dbContext);
            foreach (UGITTask task in tasks)
            {
                // For any task with zero estimated hours that is not a milestone
                if (task.EstimatedHours == 0 && task.Behaviour != Constants.TaskType.Milestone && task.ChildCount == 0)
                {
                    // Update estimated hours from duration IF forUnassignedTasksOnly is false OR no assignees
                    if (!forUnassignedTasksOnly || task.AssignedTo == null || UGITUtility.SplitString(task.AssignedTo, Constants.Separator6).Count() == 0)
                        task.EstimatedHours = Math.Round(task.Duration * workingHoursInADay, 2);
                }
            }

            CalculateEstimateHrsForSummaryTask(tasks);
        }

        private void CalculateEstimateHrsForSummaryTask(List<UGITTask> tasks)
        {
            foreach (UGITTask task in tasks.Reverse<UGITTask>())
            {
                if (task.ChildCount > 0)
                {
                    var childtasks = task.ChildTasks.Where(x => x.ChildCount == 0);
                    if (childtasks != null && childtasks.Count() > 0)
                    {
                        task.EstimatedHours = task.ChildTasks.Sum(x => x.EstimatedHours);
                        task.Duration = task.ChildTasks.Sum(x => x.Duration);
                        //task.StartDate = task.ChildTasks.Min(x => x.StartDate); commented 24B jan 2020
                        //task.DueDate = task.ChildTasks.Max(x => x.DueDate); commented 24B jan 2020
                    }
                }
            }
        }

        public void ReManageTasks(ref List<UGITTask> tasks)
        {
            ReManageTasks(ref tasks, true);
        }
        public void ReManageTasks(ref List<UGITTask> tasks, bool calculateEstimatedHrs)
        {
            if (calculateEstimatedHrs)
                CalculateEstimatedHours(ref tasks);
            tasks = MapRelationalObjects(tasks);

            if (tasks.Count > 0)
            {
                //Finds project start and enddate and workinghours in a day.
                CalculateDuration(ref tasks);
                ReOrderTasks(ref tasks);
                tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            }
        }

        public UGITTask CopyTo(UGITTask task, List<UGITTask> pTasks, long pid, string publicTicketId)
        {
            UGITTask ntask = new UGITTask(task.ModuleNameLookup);
            ntask.ProjectLookup = task.ProjectLookup;

            string ticketid = Convert.ToString(task.ProjectLookup);
            ntask.Title = GetCopyOfTitle(pTasks, task.Title + " - Copy", task);
            int itemOrder = pTasks.Count + 1;
            pTasks = null;
            ntask.ChildCount = task.ChildCount;
            ntask.ItemOrder = itemOrder;
            ntask.Level = task.Level;
            ntask.EstimatedRemainingHours = task.EstimatedRemainingHours;
            ntask.ParentTaskID = pid;

            ntask.EstimatedHours = task.EstimatedHours;
            ntask.ActualHours = task.ActualHours;

            ntask.AssignToPct = task.AssignToPct;

            ntask.Description = task.Description;
            ntask.Status = task.Status;
            ntask.PercentComplete = task.PercentComplete;
            ntask.StartDate = task.StartDate;
            ntask.DueDate = task.DueDate;

            ntask.AssignedTo = task.AssignedTo;

            ntask.Predecessors = task.Predecessors;

            ntask.Behaviour = task.Behaviour;
            ntask.Comment = task.Comment;
            ntask.Changes = task.Changes;
            ntask.TicketId = task.TicketId;
            ntask.ShowOnProjectCalendar = task.ShowOnProjectCalendar;

            ntask.AttachFiles = task.AttachFiles;

            ntask.LinkedDocuments = task.LinkedDocuments;

            ntask.IsCritical = task.IsCritical;

            ntask.UserSkillMultiLookup = task.UserSkillMultiLookup;

            SaveTask(ref ntask, task.ModuleNameLookup, publicTicketId);
            return ntask;
        }

        public void StartTasks(string publicID)
        {
            StartTasks(publicID, null, false);
        }

        public List<UGITTask> StartTasks(string publicID, string completedTaskID, bool isTask, UGITTask currentTask = null)
        {
            //UGITTaskManager taskManager = new UGITTaskManager(_context);

            //Load all dependency  based on publicid
            List<UGITTask> depncies = LoadByProjectID(publicID);
            if (depncies.Count == 0)
                return depncies;

            Dictionary<string, List<string>> moduleStartEndStages = new Dictionary<string, List<string>>();

            //Fetchs modules of which ticket is created and exist in dependency
            List<string> modules = depncies.Where(x => x.Behaviour == "Ticket").Select(x => x.RelatedModule).Distinct().ToList();

            //Load Module stage types
            LifeCycleManager lifecyleHelper = new Manager.LifeCycleManager(_context);
            LifeCycleStageManager objLifeCycleStageManager = new LifeCycleStageManager(_context);
            //SPListHelper.GetDataTable(DatabaseObjects.Lists.StageType, spWeb).Select();

            //Get Modules start and close stage
            foreach (string module in modules)
            {
                if (!string.IsNullOrEmpty(module))
                {
                    DataRow[] moduleStageTypes = UGITUtility.ToDataTable<LifeCycleStage>(lifecyleHelper.LoadLifeCycleByModule(module)[0].Stages).Select();

                    DataTable moduleStageTable = UGITUtility.ToDataTable<LifeCycleStage>(lifecyleHelper.LoadLifeCycleByModule(module)[0].Stages);  // uGITCache.LoadTable(DatabaseObjects.Lists.ModuleStages, mStageQuery, spWeb);
                    List<string> startEndStages = new List<string>();
                    DataRow[] stagesRow = moduleStageTable.Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                    startEndStages.Add(Convert.ToString(stagesRow[0][DatabaseObjects.Columns.StageTitle]));
                    DataRow stageType = moduleStageTypes.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageType) == StageType.Closed.ToString());
                    if (stageType != null && Convert.ToString(stageType[DatabaseObjects.Columns.StageTitle]) != string.Empty)
                    {
                        DataRow stage = stagesRow.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTitle) == stageType[DatabaseObjects.Columns.StageTitle].ToString());
                        if (stage != null)
                        {
                            startEndStages.Add(Convert.ToString(stage[DatabaseObjects.Columns.StageTitle]));
                        }
                        else
                        {
                            startEndStages.Add(string.Empty);
                        }
                    }
                    moduleStartEndStages.Add(module, startEndStages);
                }
            }

            //Get Status of all tasks and tickets
            Dictionary<long, string> tasksStatus = new Dictionary<long, string>();
            UGITTask instDep = new UGITTask();
            foreach (UGITTask instDep1 in depncies)
            {
                instDep = instDep1;
                if (instDep.Behaviour == "Ticket")
                {
                    //DataRow ticket = Ticket.GetCurrentTicket(_context, instDep.RelatedModule, instDep.ChildInstance);
                    DataRow ticket = Ticket.GetCurrentTicket(_context, instDep.RelatedModule, instDep.RelatedTicketID);
                    string childTicketInstance = !string.IsNullOrEmpty(instDep.ChildInstance) ? instDep.ChildInstance : instDep.RelatedTicketID;
                    bool isCloseTicket = false;
                    if (!isTask && completedTaskID != null && completedTaskID == childTicketInstance)
                    {

                        isCloseTicket = true;
                        //Save  status completed for ticket in dependency so that we don't need to check ticket again.
                        if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.TicketActualHours))
                        {
                            instDep.ActualHours = UGITUtility.StringToDouble(ticket[DatabaseObjects.Columns.TicketActualHours]);
                        }

                        instDep.PercentComplete = 100;
                        instDep.Status = Constants.Completed;
                        Save(instDep, _context);
                        //taskManager.SaveTask(ref instDep);
                    }

                    if (!isCloseTicket)
                    {
                        string lookup = objLifeCycleStageManager.Load(x => x.ModuleNameLookup == instDep.RelatedModule).FirstOrDefault(x => x.StageStep == Convert.ToInt32(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.StageStep))).StageTitle;
                        tasksStatus.Add(instDep.ID, lookup);
                    }
                    else
                    {
                        tasksStatus.Add(instDep.ID, moduleStartEndStages[instDep.RelatedModule][1]);
                    }
                }
                else
                {
                    if (instDep.PercentComplete == 100)
                        tasksStatus.Add(instDep.ID, Constants.Completed);
                    else
                        tasksStatus.Add(instDep.ID, instDep.Status);
                }
            }

            //Find tasks and tickets which need to be started
            List<UGITTask> needToStartTasks = new List<UGITTask>();
            foreach (int key in tasksStatus.Keys)
            {
                UGITTask instst = depncies.FirstOrDefault(x => x.ID == key);

                if (instst.Behaviour == "Ticket")
                {
                    KeyValuePair<string, List<string>> keyVal = moduleStartEndStages.FirstOrDefault(x => x.Key == instst.RelatedModule);
                    if (keyVal.Key != null && keyVal.Value[0] == tasksStatus[key])
                    {
                        needToStartTasks.Add(instst);
                    }
                }
                else
                {
                    if ((tasksStatus[key] == Constants.NotStarted || tasksStatus[key] == Constants.Waiting) && (string.IsNullOrEmpty(instst.ApprovalType) || instst.ApprovalType.ToLower() == TaskApprovalStatus.None || instst.ApprovalStatus.ToLower() != TaskApprovalStatus.Pending))
                    {
                        needToStartTasks.Add(instst);
                    }
                }
            }


            //Start All non predecessors tasks and tickets
            //UGITTask instDepTask = new UGITTask();
            foreach (UGITTask instDepTask1 in needToStartTasks)
            {
                //instDepTask = instDepTask1;
                bool isStart = true;

                if (!string.IsNullOrWhiteSpace(instDepTask1.Predecessors))
                {
                    bool isLStart = true;

                    foreach (UGITTask instst in instDepTask1.PredecessorsObj)
                    {
                        if (instst.Behaviour == "Ticket")
                        {
                            KeyValuePair<string, List<string>> keyVal = moduleStartEndStages.FirstOrDefault(x => x.Key == instst.RelatedModule);
                            if (keyVal.Key != null && keyVal.Value[1] != tasksStatus[instst.ID])
                            {
                                isLStart = false;
                            }
                        }
                        else if (tasksStatus[instst.ID] != Constants.Completed)
                        {
                            isLStart = false;
                        }
                    }

                    isStart = isLStart;
                }

                if (isStart)
                {
                    //Ticket case
                    if (instDepTask1.Behaviour == "Ticket")
                    {
                        Ticket tReq = new Ticket(_context, instDepTask1.RelatedModule);

                        //DataRow ticket = Ticket.GetCurrentTicket(_context, instDepTask.RelatedModule, instDepTask.ChildInstance);
                        DataRow ticket = Ticket.GetCurrentTicket(_context, instDepTask1.RelatedModule, instDepTask1.RelatedTicketID);
                        //UserProfile actionUser = uHelper.GetUser(_context, ticket, DatabaseObjects.Columns.Author);
                        string oldStatus = Convert.ToString(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.TaskStatus));
                        List<TicketColumnValue> formValues = new List<TicketColumnValue>();
                        uHelper.SetFormValues(ticket, formValues);
                        List<TicketColumnError> error = new List<TicketColumnError>();
                        bool valid = tReq.Validate(formValues, ticket, error, false, false, 1);
                        if (valid)
                            tReq.QuickClose(uHelper.getModuleIdByModuleName(_context, instDepTask1.RelatedModule), ticket, "0", performedActionName: Convert.ToString(TicketActionType.Created));
                        else if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketStatus, ticket.Table))
                        {
                            LifeCycleStage currentStage = tReq.GetTicketCurrentStage(ticket);
                            if (currentStage != null)
                                ticket[DatabaseObjects.Columns.TicketStatus] = currentStage.Name;
                        }
                        //tReq.QuickClose(uHelper.getModuleIdByModuleName(_context, instDepTask1.RelatedModule), ticket, string.Empty);
                        tReq.CommitChanges(ticket, string.Empty);
                        //Update change after updating ticket
                        tReq.UpdateTicketCache(ticket, instDepTask1.RelatedModule);
                        //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(instDepTask1.Module.LookupId, ticket);

                        //SPFieldLookupValue stageLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(ticket, DatabaseObjects.Columns.ModuleStepLookup)));
                        tasksStatus[instDepTask1.ID] = UGITUtility.ObjectToString(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.ModuleStepLookup));
                    }
                    //Task case
                    else
                    {
                        bool notifyAssignee = false;
                        bool disabledTaskNotification = false;
                        if (!string.IsNullOrWhiteSpace(instDepTask1.ApprovalType) && instDepTask1.ApprovalType.ToLower() != "none" && instDepTask1.ApprovalStatus == TaskApprovalStatus.NotStarted)
                        {
                            instDepTask1.ApprovalStatus = TaskApprovalStatus.Pending;
                            Save(instDepTask1, _context);

                            DataRow[] taskItems = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.ModuleTasks, DatabaseObjects.Columns.ID, instDepTask1.ID);
                            if (taskItems != null && taskItems.Length > 0)
                                SendEmailToApprover(taskItems[0], _context);
                        }
                        else
                        {
                            instDepTask1.Status = Constants.InProgress; //Constants.InProgress;  // Status changed for Tenant Application registration
                            instDepTask1.PercentComplete = 0;
                            instDepTask1.StartDate = DateTime.Now;
                            instDepTask1.IsStartDateChange = true;
                            Save(instDepTask1, _context);
                            tasksStatus[instDepTask1.ID] = Constants.InProgress; //Constants.InProgress;

                            ConfigurationVariableManager configManager = new ConfigurationVariableManager(_context);
                            notifyAssignee = configManager.GetValueAsBool(ConfigConstants.NotifyTaskAssignee);
                            disabledTaskNotification = instDepTask1.NotificationDisabled;
                        }

                        //Create user account if task type is create account task
                        if (instDepTask1.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower() && instDepTask1.AutoCreateUser)
                        {
                            string userDisplayName = "";
                            userDisplayName = AddNewUser(instDepTask1);
                            if (!string.IsNullOrWhiteSpace(userDisplayName))
                            {
                                tasksStatus[instDepTask1.ID] = Constants.Completed;

                                //Update login name of all access type task based on account task.
                                List<UGITTask> accessTasks = depncies.Where(x => x.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower() && x.Predecessors != null && UGITUtility.ConvertStringToList(x.Predecessors, Constants.Separator6).Exists(y => UGITUtility.StringToLong(y) == instDep.ID)).ToList();

                                foreach (UGITTask accessTsk in accessTasks)
                                {
                                    accessTsk.NewUserName = instDepTask1.NewUserName;
                                    accessTsk.UGITNewUserDisplayName = instDepTask1.UGITNewUserDisplayName;
                                    Save(accessTsk, _context);
                                    //accessTsk.Save(spWeb);
                                }
                            }
                        }

                        if (notifyAssignee && !disabledTaskNotification && instDepTask1.Status == Constants.InProgress)
                        {
                            SendEmailToAssignee(instDep, _context);
                        }
                    }

                }
            }

            //TaskCache.ReloadProjectTasks("SVC", publicID, spWeb);
            string moduleName = uHelper.getModuleNameByTicketId(publicID);

            // Initiate Updating the records for current task in ScheduleActions List
            List<UGITTask> scheduleReminders = new List<UGITTask>();

            if (needToStartTasks != null && needToStartTasks.Count > 0)
                scheduleReminders = needToStartTasks;

            if (currentTask != null)
                scheduleReminders.Add(currentTask);

            if (moduleName == ModuleNames.SVC && scheduleReminders != null && scheduleReminders.Count > 0 && publicID != null)
            {
                if (scheduleReminders.Count == 1 && scheduleReminders[0].SubTaskType == "Ticket")
                    return depncies;


                DataRow svcRequestsListItem = Ticket.GetCurrentTicket(_context, moduleName, publicID);

                long serviceLookup = UGITUtility.StringToLong(UGITUtility.GetSPItemValue(svcRequestsListItem, DatabaseObjects.Columns.ServiceTitleLookup));
                ServicesManager servicesManager = new ServicesManager(_context);
                Services service = null;

                if (serviceLookup > 0)
                    service = servicesManager.LoadByID(serviceLookup);

                try
                {
                    ThreadStart starter = delegate { UpdateScheduleActions(_context, scheduleReminders, UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(svcRequestsListItem, DatabaseObjects.Columns.EnableTaskReminder)), service); };
                    Thread thread = new Thread(starter);
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception error)
                {
                    ULog.WriteLog("Exception: ", error.Message);
                    Thread.ResetAbort();
                }
            }

            return depncies;
        }

        public void MoveSVCTicket(string parentTicketID, UGITTask task = null)
        {
            UGITTaskManager taskManager = new UGITTaskManager(_context);
            List<UGITTask> depncies = taskManager.LoadByProjectID(parentTicketID);
            if (depncies.Count == 0)
                return;
            DataRow svcTicket = null;
            if (task == null)
            {
                svcTicket = Ticket.GetCurrentTicket(_context, ModuleNames.SVC, parentTicketID);
                if (svcTicket == null)
                {
                    ULog.WriteLog("ERROR: Cannot find SVC ticket with ID " + parentTicketID);
                    return;
                }
            }
            else
            {
                DataTable dataTable = UGITUtility.ObjectToData(task);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    svcTicket = dataTable.Rows[0];
                }
            }

            bool dependencies = depncies.Exists(x => x.Status != Constants.Completed && x.Status != Constants.Cancelled);
            if (!dependencies)
            {
                UGITModule module = ObjModuleViewManager.LoadByName(ModuleNames.SVC);
                if (module != null)
                {
                    Ticket tReq = new Ticket(_context, ModuleNames.SVC);

                    //Updates IsAllTaskComplete field of svc ticket to true
                    //Save actual hours in service based on actual hours of completed ticket and task
                    svcTicket[DatabaseObjects.Columns.TicketActualHours] = depncies.Sum(x => x.ActualHours);

                    svcTicket[DatabaseObjects.Columns.IsAllTaskComplete] = 1;
                    UpdateSVCSummaryFields(depncies, svcTicket);

                    tReq.CommitChanges(svcTicket, donotUpdateEscalations: true);

                    //Load ticket again and move service ticket to next stage
                    LifeCycleStage currentStage = tReq.GetTicketCurrentStage(svcTicket);
                    if (currentStage != null && currentStage.StageTypeChoice == StageType.Assigned.ToString())
                    {
                        tReq.ApproveTicket(new List<TicketColumnError>(), svcTicket, false);
                        // Update cache after updating ticket
                        tReq.UpdateTicketCache(svcTicket, tReq.Module.ModuleName);
                    }
                }
            }

            else if (depncies.Where(x => x.Status == Constants.Completed).Count() > 0)
            {
                Ticket tReq = new Ticket(_context, ModuleNames.SVC);
                svcTicket[DatabaseObjects.Columns.TicketActualHours] = depncies.Where(x => x.Status == Constants.Completed).Sum(x => x.ActualHours);
                UpdateSVCSummaryFields(depncies, svcTicket);
                tReq.CommitChanges(svcTicket, donotUpdateEscalations: true);
                if (tReq.Module != null)
                    tReq.UpdateTicketCache(svcTicket, tReq.Module.ModuleName);
            }
        }

        public bool isRequestTaskCompleted(UGITTask task)
        {
            UGITTaskManager taskManager = new UGITTaskManager(_context);
            bool isRequestTaskCompleted = true;
            string ticketId = task.TicketId;
            List<UGITTask> allTaskOnSameStage = taskManager.LoadByProjectID(task.TicketId).Where(x => x.StageStep == task.StageStep).ToList();
            if (allTaskOnSameStage.Count > 0)
            {
                List<UGITTask> incompleteTaskOnSameStage = allTaskOnSameStage.Where(x => x.Status != "Completed" && x.ID != task.ID).ToList();
                if (incompleteTaskOnSameStage.Count > 0)
                {
                    isRequestTaskCompleted = false;
                }
                else
                {

                }
            }

            return isRequestTaskCompleted;

        }

        public void UpdateSVCTicket(DataRow svcItem, bool update)
        {
            string publicID = Convert.ToString(svcItem[DatabaseObjects.Columns.TicketId]);
            List<UGITTask> depncies = LoadByProjectID(publicID);
            //Load(x => x.RelatedTicketID.Equals(publicID));

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            //ManageMinStartDueDates(ref depncies);

            if (depncies.Count > 0 && depncies[0].TicketId.StartsWith("SVC"))
            {
                List<UGITTask> completedTasks = depncies.Where(x => x.Status == Constants.Completed).ToList();
                if (completedTasks.Count > 0)
                {
                    svcItem[DatabaseObjects.Columns.TicketActualHours] = completedTasks.Sum(x => x.ActualHours);
                    if (completedTasks.Count == depncies.Count)
                        svcItem[DatabaseObjects.Columns.IsAllTaskComplete] = 1;
                }

                svcItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = 0;
                // Need to discuss because below code is from SharePoint and in .NET we don't have supporting tables.
                List<UGITTask> lTasks = depncies.Where(x => !x.SLADisabled).ToList();
                if (lTasks.Count > 0)
                {
                    List<Tuple<string, DateTime, DateTime>> holdSlots = new List<Tuple<string, DateTime, DateTime>>();
                    foreach (UGITTask d in lTasks)
                    {
                        DateTime startDate = DateTime.MinValue;
                        if (d.TaskActualStartDate.HasValue)
                            startDate = d.TaskActualStartDate.Value;
                        else if (d.StartDate != DateTime.MinValue)
                            startDate = d.StartDate;

                        uHelper.GetHoldUnHoldSlots(_context, d.ID.ToString(), publicID, ref holdSlots);
                    }
                    svcItem[DatabaseObjects.Columns.TicketTotalHoldDuration] = uHelper.GetTotalDurationByTimeSlot(_context, holdSlots);
                }

                UpdateSVCSummaryFields(depncies, svcItem);
                if (update)
                {
                    Ticket sModuleTicket = new Ticket(_context, "SVC");
                    sModuleTicket.CommitChanges(svcItem, string.Empty, null, false, false, true);
                }
            }
        }

        public void UpdateSVCSummaryFields(List<UGITTask> tasks, DataRow moduleItem)
        {
            moduleItem[DatabaseObjects.Columns.TicketPctComplete] = 0;
            List<UGITTask> completedTasks = tasks.Where(x => x.PercentComplete == 100.0f).ToList();

            // Update overall % complete for SVC ticket based on task status
            if (completedTasks.Count > 0)
            {
                // If we have estimated hours for any of the tasks, then calculate overall % complete using estimated hours
                double pctComplete = 0;
                double totalHours = tasks.Sum(x => x.EstimatedHours);
                if (totalHours > 0)
                {
                    double completedHours = 0;
                    if (completedTasks.Count > 0)
                        completedHours = completedTasks.Sum(x => x.EstimatedHours);
                    pctComplete = (completedHours / totalHours);
                }
                else
                {
                    // If we DON'T have estimated hours for any tasks, then calculate overall % complete using task weights
                    double totalWeight = tasks.Sum(x => x.Weight);
                    if (totalWeight > 0)
                    {
                        double completedWeight = completedTasks.Sum(x => x.Weight);
                        pctComplete = (completedWeight / totalWeight);
                    }
                }

                // Cannot show 100% complete if any tasks are still incomplete
                if (pctComplete == 1)
                {
                    List<UGITTask> incompleteTasks = tasks.Where(x => x.PercentComplete != 100.0f).ToList();
                    if (incompleteTasks.Count > 0)
                    {
                        // Reduce % complete by 1% for every incomplete task
                        pctComplete = pctComplete - (UGITUtility.StringToDouble(incompleteTasks.Count) / 100);
                    }
                }
                moduleItem[DatabaseObjects.Columns.TicketPctComplete] = Math.Round(pctComplete, 3);
            }

            // Update TicketStageActionUsers to show list of people/groups we are waiting on
            UpdateActionUsers(moduleItem, tasks);
        }

        public string AddNewUser(UGITTask instDep)
        {
            string Status = "";

            //return is instance is not account task
            if ((instDep == null || instDep.SubTaskType.ToLower() != ServiceSubTaskType.AccountTask.ToLower()) || instDep.NewUserName == string.Empty)
            {
                return Status;
            }

            string fullUserName = instDep.NewUserName;
            UserProfileManager userManager = new Manager.UserProfileManager(_context);
            string loginUserName = userManager.FormatNewUserName(fullUserName);
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(_context);
            string defaultPassword = configManager.GetValue(ConfigConstants.DefaultPassword);

            UserProfile nUser = null;
            try
            {
                if (userManager.SaveNewUser(_context, loginUserName, defaultPassword, fullUserName, String.Empty, Enums.UserType.NewADUser))
                {
                    instDep.Status = Constants.Completed;
                    instDep.PercentComplete = 100;
                    instDep.Description = " User Created by System automatically with " + loginUserName + " username in AD";
                    instDep.ErrorMsg = "";
                    Status = fullUserName;

                    try
                    {
                        // Checks whether the specified logon name belongs to a valid user of the website, and if the logon name does not already exist, adds it to the website.
                        // nUser = spWeb.EnsureUser(loginUserName);
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, "EnsureUser error for new user " + loginUserName);
                    }

                    if (nUser != null)
                    {
                        //Add user in default user group
                        string memberGroupName = configManager.GetValue(ConfigConstants.DefaultGroup);
                        if (memberGroupName != string.Empty)
                        {
                            //UserProfile userGroup = UserProfile.GetGroupByName(memberGroupName);
                            //if (userGroup != null)
                            //{
                            //    userGroup.AddUser(nUser);
                            //    userGroup.Update();
                            //}
                        }

                        instDep.NewUserName = nUser.UserName;
                        //instDep.Save(spWeb);

                    }
                }
                else
                {
                    instDep.ErrorMsg += " Unable to create the user automatically with login name " + loginUserName + " as user already exists. Please create user manually and update the login name.";
                    Status = "";
                }
            }
            catch (Exception ex)
            {
                //Log.WriteException(ex, "Error creating user " + loginUserName);
                instDep.ErrorMsg += "Unable to create the user automatically with login name " + loginUserName + " as " + ex.Message;
                Status = "";
            }

            return Status;
        }

        //public void UpdateUserItem(UGITTask current,ApplicationContext _appContext)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(current.UserFieldXML);
        //    Dictionary<string, object> userprofileDic = UGITUtility.DeserializeDicObject(current.UserFieldXML);
        //    if (userprofileDic == null)
        //    {
        //        ULog.WriteLog("ERROR deserializing UserFieldXML when updating new user profile attributes");
        //        return;
        //    }

        //    UserProfileManager userProfileManager = new UserProfileManager(_appContext);
        //    UserProfile user = userProfileManager.GetUserByUserName(current.NewUserName);
        //    UserProfile userItem=  userProfileManager.GetUserById(user.Id);
        //    //SPList userInfoList = spWeb.SiteUserInfoList;
        //    //SPListItem userItem = SPListHelper.GetSPListItem(userInfoList.Title, user.ID, spWeb);
        //    object value;

        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.MobilePhone))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.MobilePhone, out value);
        //        userItem.MobilePhone = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.Name))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.Name, out value);
        //        userItem[DatabaseObjects.Columns.Name] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.JobTitle))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.JobTitle, out value);
        //        userItem[DatabaseObjects.Columns.JobTitle] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.DepartmentLookup))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.DepartmentLookup, out value);
        //        userItem[DatabaseObjects.Columns.DepartmentLookup] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.ResourceHourlyRate))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.ResourceHourlyRate, out value);
        //        userItem[DatabaseObjects.Columns.ResourceHourlyRate] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.IsConsultant))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.IsConsultant, out value);
        //        userItem[DatabaseObjects.Columns.IsConsultant] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.IsManager))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.IsManager, out value);
        //        userItem[DatabaseObjects.Columns.IsManager] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.IT))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.IT, out value);
        //        userItem[DatabaseObjects.Columns.IT] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.BudgetLookup))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.BudgetLookup, out value);
        //        userItem[DatabaseObjects.Columns.BudgetLookup] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.UserRoleLookup))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.UserRoleLookup, out value);
        //        userItem[DatabaseObjects.Columns.UserRoleLookup] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.Enabled))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.Enabled, out value);
        //        userItem[DatabaseObjects.Columns.Enabled] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.DeskLocation))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.DeskLocation, out value);
        //        userItem[DatabaseObjects.Columns.DeskLocation] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.NotificationEmail))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.NotificationEmail, out value);
        //        userItem[DatabaseObjects.Columns.NotificationEmail] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.DisableWorkflowNotifications))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.DisableWorkflowNotifications, out value);
        //        userItem[DatabaseObjects.Columns.DisableWorkflowNotifications] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.EmployeeID))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.EmployeeID, out value);
        //        userItem[DatabaseObjects.Columns.EmployeeID] = value;
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.UGITStartDate))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.UGITStartDate, out value);
        //        if (!string.IsNullOrEmpty(Convert.ToString(value)))
        //            userItem[DatabaseObjects.Columns.UGITStartDate] = Convert.ToDateTime(value);

        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.UGITEndDate))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.UGITEndDate, out value);
        //        if (!string.IsNullOrEmpty(Convert.ToString(value)))
        //            userItem[DatabaseObjects.Columns.UGITEndDate] = Convert.ToDateTime(value);
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.ManagerLookup))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.ManagerLookup, out value);
        //        if (!string.IsNullOrEmpty(Convert.ToString(value)))
        //            userItem[DatabaseObjects.Columns.ManagerLookup] = uHelper.GetLookupID(Convert.ToString(value));
        //    }
        //    if (userprofileDic.ContainsKey(DatabaseObjects.Columns.LocationLookup))
        //    {
        //        userprofileDic.TryGetValue(DatabaseObjects.Columns.LocationLookup, out value);
        //        if (!string.IsNullOrEmpty(Convert.ToString(value)))
        //            userItem[DatabaseObjects.Columns.LocationLookup] = uHelper.GetLookupID(Convert.ToString(value));
        //    }

        //    bool allowUnsafe = spWeb.AllowUnsafeUpdates;
        //    spWeb.AllowUnsafeUpdates = true;
        //    userItem.Update();
        //    spWeb.AllowUnsafeUpdates = allowUnsafe;
        //}

        public void UpdateSVCPRPORP(DataRow parentTicket, string requestPage = " ")
        {
            if (requestPage == "SelfRegistration")
            {
                _context = ApplicationContext.Create();

            }
            string parentTicketID = Convert.ToString(parentTicket[DatabaseObjects.Columns.TicketId]);
            string parentModuleName = uHelper.getModuleNameByTicketId(parentTicketID);

            List<UGITTask> tasks = LoadByProjectID(parentTicketID);
            List<UserProfile> assignedToColl = new List<UserProfile>();
            if (tasks != null && tasks.Count > 0)
            {
                List<UGITTask> lstTasks = tasks.Where(x => x.Behaviour == "Task" && x.AssignedTo != null).ToList();
                foreach (UGITTask item in lstTasks)
                {

                    if (item.AssignedTo != null)
                    {
                        UserProfileManager userManager = new UserProfileManager(_context);
                        assignedToColl = userManager.GetUserInfosById(item.AssignedTo);
                        foreach (UserProfile user in assignedToColl)
                        {
                            if (!assignedToColl.Exists(x => x.Id == user.Id))
                                assignedToColl.Add(user);
                        }
                    }
                }

                Ticket sModuleTicket = new Ticket(_context, parentModuleName);
                if (parentTicket != null && assignedToColl != null && assignedToColl.Count > 0)
                {
                    // Assign first assignee as PRP, and rest as ORP
                    UserProfile prpUser = assignedToColl.FirstOrDefault();
                    if (parentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
                    {
                        parentTicket[DatabaseObjects.Columns.TicketPRP] = prpUser.Id;
                        List<UserProfile> orpUsers = new List<UserProfile>();
                        assignedToColl.Remove(prpUser);

                        if (parentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketORP))
                        {
                            parentTicket[DatabaseObjects.Columns.TicketORP] = assignedToColl;
                        }

                        sModuleTicket.CommitChanges(parentTicket, null);
                    }
                }
            }
        }
        //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
        public void TaskOnHold(string comment, DateTime holdTill, string holdReason, UGITTask task, string moduleName = "", bool stopUnHoldSchedule = false)
        //public void TaskOnHold(string comment, DateTime holdTill, string holdReason, int ID, string moduleName = "", bool stopUnHoldSchedule = false)
        //
        {

            if (task.ID == 0)
                return;

            UGITTask relationship = LoadByID(task.ID);
            string holdComments = string.Format("On Hold Till {0}, Reason: {1}", UGITUtility.GetDateStringInFormat(holdTill, true), holdReason);

            if (!string.IsNullOrWhiteSpace(comment))
                holdComments = string.Format("{0}, Comment: {1}", holdComments, comment);

            holdComments = UGITUtility.WrapComment(holdComments, "hold");

            relationship.OnHold = UGITUtility.StringToBoolean("1");
            relationship.Status = Constants.OnHoldStatus;
            relationship.Comment = UGITUtility.GetVersionString(_context.CurrentUser.UserName, holdComments, relationship.Comment);
            if (holdReason == null || holdReason.Trim() == string.Empty)
                holdReason = "Other";

            relationship.OnHoldReasonChoice = holdReason;
            relationship.OnHoldStartDate = DateTime.Now;

            if (holdTill == DateTime.MinValue || holdTill == DateTime.MaxValue)
                holdTill = DateTime.Today.AddYears(1);

            relationship.OnHoldTillDate = holdTill;
            Update(relationship);

            // Hold sub ticket as well

            if (!string.IsNullOrEmpty(relationship.RelatedModule))
            {
                DataRow sTicket = Ticket.GetCurrentTicket(_context, relationship.RelatedModule, relationship.RelatedTicketID);
                Ticket tRequest = new Ticket(_context, relationship.RelatedModule);
                tRequest.HoldTicket(sTicket, relationship.RelatedModule, comment, holdTill, holdReason);
                tRequest.CommitChanges(sTicket);

            }

            // Create schedule action
            if (!stopUnHoldSchedule && moduleName == ModuleNames.SVC)
            {
                AgentJobHelper agentJob = new AgentJobHelper(_context);
                agentJob.ScheduleUnholdTask(relationship, moduleName);
            }
            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
            UpdateEventLogForTask(_context, Constants.TicketEventType.Hold, task.TicketId, task.Title, task.ID.ToString(), plannedEndDate: holdTill, comment: holdComments);

            //
            // Put on Hold SVC Ticket
            DoHoldUnHoldSVCInstance(_context, relationship.TicketId);
        }
        //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
        public void UpdateEventLogForTask(ApplicationContext _context, string status, string parentInstance, string title, string taskId, string effectedUsers = "", string eventReason = "", string comment = "", DateTime? plannedEndDate = null, bool bySystem = false, bool backgroundUpdate = true)
        {
            if(parentInstance != null)
            { 
                string moduleName = uHelper.getModuleNameByTicketId(parentInstance);
                int moduleId = uHelper.getModuleIdByModuleName(_context, moduleName);
                TicketEventManager logEventHelper = new TicketEventManager(_context, moduleName, parentInstance);
                Ticket ticket = new Ticket(_context, moduleId);
                //int userid = UGITUtility.StringToInt(_context.CurrentUser.Id);

                if (ticket != null)
                {
                    DataRow parentItem = Ticket.GetCurrentTicket(_context, moduleName, parentInstance);
                    //SPListItem parentItem = uHelper.GetTicket(parentInstance, _context);
                    LifeCycleStage lifeCycleStage = ticket.GetTicketCurrentStage(parentItem);
                    if (backgroundUpdate)
                        logEventHelper.LogEvent(status, lifeCycleStage, subTaskTitle: title, subTaskId: taskId, affectedUsers: effectedUsers, eventReason: eventReason, plannedEndDate: plannedEndDate, comment: comment, bySystem: bySystem);
                    else
                        logEventHelper.LogEventImmediate(_context, _context.CurrentUser.Id, status, lifeCycleStage, subTaskTitle: title, subTaskId: taskId, affectedUsers: effectedUsers, eventReason: eventReason, plannedEndDate: plannedEndDate, comment: comment, bySystem: bySystem);
                }
            }
        }

        /// <summary>
        /// This method is used to unHold the task
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="task"></param>
        /// <param name="holdExpired"></param>
        //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
        public void TaskUnHold(string comment, UGITTask task, bool holdExpired = false)
        {
            if (task.ID == 0)
                return;

            UGITTask relationship = LoadByID(task.ID);

            if (!string.IsNullOrEmpty(comment))
                relationship.Comment = UGITUtility.GetVersionString(_context.CurrentUser.UserName, UGITUtility.WrapComment(comment, holdExpired ? "HoldExpired" : "UnHold"), relationship.Comment);

            relationship.OnHoldTillDate = null;

            // calculate current hold of ticket
            double holdTime = 0;
            if (relationship.OnHoldStartDate.HasValue && relationship.OnHoldStartDate.Value != DateTime.MinValue)
            {
                holdTime = uHelper.GetWorkingMinutesBetweenDates(_context, (DateTime)relationship.OnHoldStartDate, DateTime.Now);
                relationship.OnHoldStartDate = null;
            }

            relationship.OnHoldReasonChoice = null;

            //Set total hold duration of ticket till now.
            relationship.TotalHoldDuration = UGITUtility.StringToDouble(relationship.TotalHoldDuration) + Math.Round(holdTime, 0);

            // Get all the predecessors of current task and check if any predecessor is not completed/cancelled
            // if yes then set the status current task to Waiting
            List<UGITTask> taskPredecessors = GetTaskPredecessors(relationship.ModuleNameLookup, relationship);
            bool incompletePredecessor = false;

            if (taskPredecessors != null && taskPredecessors.Count > 0)
                incompletePredecessor = taskPredecessors.Any(x => x.Status != Constants.Completed && x.Status != Constants.Cancelled);

            string svcTicketID = relationship.TicketId;
            DataRow svcITem = Ticket.GetCurrentTicket(_context, "SVC", svcTicketID);

            // Get currentStage and assignedStage for SVC Ticket life cycle
            Ticket tRequest = new Ticket(_context, "SVC");
            LifeCycle lifeCycle = tRequest.GetTicketLifeCycle(svcITem);
            LifeCycleStage currentStage = tRequest.GetTicketCurrentStage(svcITem);
            LifeCycleStage assignedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Assigned.ToString());

            double pctComplete = relationship.PercentComplete;

            // Set the status of current task to Waiting if below condition is true
            if (incompletePredecessor || (currentStage != null && assignedStage != null && currentStage.StageStep < assignedStage.StageStep))
            {
                relationship.Status = Constants.Waiting;
                relationship.PercentComplete = 0;
            }
            else if (pctComplete < 100)
            {
                relationship.Status = Constants.InProgress;
            }
            else
            {
                relationship.Status = Constants.Completed;
            }

            relationship.OnHold = UGITUtility.StringToBoolean("0");
            Update(relationship);

            // Hold sub ticket as well
            if (!string.IsNullOrEmpty(relationship.RelatedModule))
            {
                DataRow sTicket = Ticket.GetCurrentTicket(_context, relationship.RelatedModule, relationship.RelatedTicketID);
                Ticket ticketRequest = new Ticket(_context, relationship.RelatedModule);
                ticketRequest.UnHoldTicket(sTicket, relationship.RelatedModule, comment);
                ticketRequest.CommitChanges(sTicket);
            }

            // Add task event for when is being removed from hold
            // NOTE: Need to pass backgroundUpdate: false since we need the event data immediately after to calculate SVC ticket hold time
            string eventType = holdExpired ? "Hold Expired" : "Hold Removed";

            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
            UpdateEventLogForTask(_context, eventType, task.TicketId, task.Title, task.ID.ToString(), comment: comment, bySystem: holdExpired, backgroundUpdate: false);

            //Update SVC ticket 
            if (svcITem != null)
            {
                // Un-hold SVC Ticket if any of the task go off un-hold
                if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketOnHold, svcITem.Table) && UGITUtility.StringToBoolean(svcITem[DatabaseObjects.Columns.TicketOnHold]))
                    DoHoldUnHoldSVCInstance(_context, relationship.TicketId);
                else
                    UpdateSVCTicket(svcITem, true);
            }
        }

        public void HoldTasks(string moduleName, string ticketID, string comment, DateTime holdTill, string holdReason)
        {
            if (moduleName != "SVC")
                return;

            List<UGITTask> tasks = LoadByProjectID(ticketID);
            List<UGITTask> tasksToHold = tasks.Where(x => x.OnHold ||  // already on hold
                                                                          (string.IsNullOrEmpty(x.RelatedModule) && x.Status == Constants.InProgress) ||  // sub-tasks in progress 
                                                                          (!string.IsNullOrEmpty(x.RelatedModule) && x.Status != Constants.Waiting && x.Status != Constants.Completed) // sub-tickets in progress
                                                                          ).ToList();
            foreach (UGITTask task in tasksToHold)
            {
                //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
                TaskOnHold(comment, holdTill, holdReason, task, moduleName, true);
                //
                //TaskOnHold(comment, holdTill, holdReason, Convert.ToInt32(task.ID), moduleName, true);
            }
        }

        public void UnHoldTasks(string moduleName, string ticketID, string comment, bool holdExpired = false)
        {
            if (moduleName != "SVC")
                return;

            List<UGITTask> tasks = LoadByProjectID(ticketID);
            List<UGITTask> holdTasks = tasks.Where(x => x.OnHold).ToList();
            foreach (UGITTask task in holdTasks)
            {
                //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
                //TaskUnHold(comment, Convert.ToInt32(task.ID), holdExpired);
                TaskUnHold(comment, task, holdExpired);
                //
            }
        }

        public double GetTotalHoldTime(UGITTask task, bool inHours)
        {

            double totalHoldTime = task.TotalHoldDuration;

            //Add current hold duration of task as well if task is on hold
            if (task.OnHold && task.OnHoldStartDate.HasValue && task.OnHoldStartDate.Value != DateTime.MinValue)
            {
                double cHoldMinutes = uHelper.GetWorkingMinutesBetweenDates(_context, task.OnHoldStartDate.Value, DateTime.Now);
                totalHoldTime += cHoldMinutes;
            }

            if (inHours)
                return TimeSpan.FromMinutes(totalHoldTime).TotalHours;

            return totalHoldTime;
        }

        public string GetFormattedHoldTime(UGITTask task)
        {
            double totalHoldTime = GetTotalHoldTime(task, false);
            if (totalHoldTime == 0)
                return "0";

            TimeSpan timeSpan = TimeSpan.FromMinutes(totalHoldTime);
            if (totalHoldTime < 60)
                return timeSpan.ToString("m' mins'");
            else
            {
                int totalHours = (int)Math.Floor(timeSpan.TotalHours);
                int minutes = timeSpan.Minutes;
                return string.Format("{0} hrs, {1} mins", totalHours, minutes);
            }
        }
        //public DataTable GetAllTasksByProjectID(string moduleName, string projectPublicID)
        //{
        //    DataRow projectItem = Ticket.GetCurrentTicket(this.dbContext,moduleName, projectPublicID);
        //    DataTable tasks = null;
        //    if (projectItem != null && !UGITUtility.StringToBoolean(projectItem[DatabaseObjects.Columns.TicketClosed]))
        //    {
        //        //LoadTaskIfEmpty();
        //        tasks =UGITUtility.ToDataTable(this.Load(x=> x.ModuleName==moduleName && x.ProjectLookup==Convert.ToInt64(projectPublicID)));

        //        //Sort by itemorder to get proper ordering
        //        if (tasks != null && tasks.Rows.Count > 0)
        //        {
        //            tasks.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
        //            tasks = tasks.DefaultView.ToTable();
        //        }
        //    }
        //    else
        //    {
        //        UGITTaskManager tHelper = new UGITTaskManager(this.dbContext);//, "PMMIssues", DatabaseObjects.Columns.TicketPMMIdLookup);
        //        if (moduleName == Constants.PMMIssue)
        //        {

        //            List<UGITTask> issuesTasks = tHelper.LoadByProjectID(projectPublicID).Where(x=>x.ModuleName== "PMMIssues").ToList();
        //           // tasks = tHelper.FillTaskTable(issuesTasks);
        //        }
        //        //else if (moduleName == Constants.ExitCriteria)
        //        //{
        //        //    UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, DatabaseObjects.Tables.ModuleStageConstraints, DatabaseObjects.Columns.TicketId);
        //        //    List<UGITTask> criteriaTasks = tHelper.LoadByProjectID(projectPublicID);
        //        //    tasks = UGITTaskHelper.FillTaskTable(criteriaTasks);
        //        //}
        //        //if (moduleName == Constants.VendorRisks)
        //        //{
        //        //    UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, DatabaseObjects.Tables.VendorRisks, DatabaseObjects.Columns.VendorMSALookup);
        //        //    List<UGITTask> issuesTasks = tHelper.LoadByProjectID(projectPublicID);
        //        //    tasks = UGITTaskHelper.FillTaskTable(issuesTasks);
        //        //}
        //        //else if (moduleName == Constants.VendorIssue)
        //        //{
        //        //    UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, DatabaseObjects.Lists.VendorIssues, DatabaseObjects.Columns.VendorMSALookup);
        //        //    List<UGITTask> criteriaTasks = tHelper.LoadByProjectID(projectPublicID);
        //        //    tasks = UGITTaskHelper.FillTaskTable(criteriaTasks);
        //        //}
        //        //else if (moduleName == "PMM" || moduleName == "TSK" || moduleName == "TSK")
        //        //{
        //        //    tasks = UGITTaskHelper.LoadTasksTable(SPContext.Current.Web, moduleName, false, projectPublicID);
        //        //}
        //        //else if (UGITTaskHelper.IsModuleTasks(moduleName))
        //        //{
        //        //    UGITTaskHelper tHelper = new UGITTaskHelper(SPContext.Current.Web, "ModuleTasks", DatabaseObjects.Columns.TicketId);
        //        //    List<UGITTask> moduleTasks = tHelper.LoadByProjectID(projectPublicID);
        //        //    tasks = UGITTaskHelper.FillTaskTable(moduleTasks);
        //        //}
        //    }

        //    //if (tasks == null)
        //    //    tasks = CreateSchema();

        //    return tasks;
        //}

        public void UpdatePredecessorsByOrder(DataTable table)
        {
            if (table == null || table.Rows.Count <= 0)
            {
                return;
            }
            UpdatePredecessorsByOrder(table.Select());
        }

        public void UpdatePredecessorsByOrder(DataRow[] tableRows)
        {
            if (tableRows.Length <= 0)
            {
                return;
            }

            List<string> orderCal = null;
            string predecessors = string.Empty;
            string[] predecessorsID = new string[0];
            DataRow tRow = null;
            List<string> predecessorsOrder = new List<string>();
            try
            {
                foreach (DataRow taskRow in tableRows)
                {
                    orderCal = new List<string>();
                    predecessors = Convert.ToString(taskRow[DatabaseObjects.Columns.Predecessors]);
                    if (predecessors == null || predecessors.Trim() == string.Empty)
                    {
                        continue;
                    }

                    predecessorsOrder = new List<string>();
                    predecessorsID = predecessors.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string pID in predecessorsID)
                    {
                        DataRow[] rowColl = tableRows.CopyToDataTable().Select(string.Format("{0}={1}", DatabaseObjects.Columns.ItemOrder, pID));
                        if (rowColl != null && rowColl.Length > 0)
                            tRow = rowColl[0];
                        //tableRows.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.Id) == UGITUtility.StringToLong(pID));
                        if (tRow != null)
                        {
                            predecessorsOrder.Add(Convert.ToString(tRow[DatabaseObjects.Columns.ItemOrder]));
                        }
                    }
                    taskRow[DatabaseObjects.Columns.Predecessors] = string.Join("; ", predecessorsOrder.ToArray());
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public DataTable GetProjectsTasksTable(ApplicationContext applicationContext, int[] Ids, TicketStatus tstatus, string moduleName)
        {
            ticketManager = new TicketManager(applicationContext);
            ObjModuleViewManager = new ModuleViewManager(applicationContext);
            DataTable openProjects = new DataTable();
            //int moduleId = getModuleIdByModuleName(moduleName);
            UGITModule uGITModule = ObjModuleViewManager.GetByName(moduleName);

            DataTable allTasks = null;
            if (tstatus == TicketStatus.Open)
                allTasks = this.GetAllTasks(moduleName);
            else
                allTasks = this.LoadTasksTable(moduleName);

            switch (tstatus)
            {
                case TicketStatus.Open:
                    openProjects = ticketManager.GetOpenTickets(uGITModule); //uGITCache.ModuleDataCache.GetOpenTickets(moduleId, spWeb);
                    //allTasks =   TaskCache.GetAllTasks(moduleName,spWeb); // Returns all tasks of OPEN projects only
                    break;
                case TicketStatus.Closed:
                    openProjects = ticketManager.GetClosedTickets(uGITModule); //uGITCache.ModuleDataCache.GetClosedTickets(moduleId, spWeb);
                    //allTasks = UGITTaskHelper.LoadTasksTable(spWeb, moduleName, false, null);
                    break;
                default: // All projects
                    openProjects = ticketManager.GetAllTickets(uGITModule); //uGITCache.ModuleDataCache.GetAllTickets(moduleId, spWeb);
                    //allTasks = UGITTaskHelper.LoadTasksTable(spWeb, moduleName);
                    break;
            }



            if (allTasks == null || allTasks.Rows.Count <= 0 || openProjects == null || openProjects.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                openProjects = (from x in openProjects.AsEnumerable()
                                where (Ids.Contains(Convert.ToInt32(x.Field<long>(DatabaseObjects.Columns.ID))))
                                select x).CopyToDataTable();
            }

            var openTaskRows = (from t in allTasks.AsEnumerable()
                                join p in openProjects.AsEnumerable() on
                                    t.Field<string>(DatabaseObjects.Columns.TicketId) equals
                                    p.Field<string>(DatabaseObjects.Columns.TicketId)
                                select t).ToArray();

            DataTable openProjectTasks = null;

            if (openTaskRows.Length > 0)
            {
                openProjectTasks = openTaskRows.CopyToDataTable();

                //Update Predecessor by order field
                //  uGITTaskManager.UpdatePredecessorsByOrder(openProjectTasks);

                openProjectTasks.Columns.Add("TitleWithPctComplete");
                openProjectTasks.Columns.Add(DatabaseObjects.Columns.BehaviourIcon);
                int openProjectWithTasks = 0;

                foreach (DataRow row in openProjects.Rows)
                {
                    DataRow[] projectTasks = openProjectTasks.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, row[DatabaseObjects.Columns.TicketId]));
                    if (projectTasks.Length > 0)
                    {
                        openProjectWithTasks += 1;
                        foreach (DataRow pTask in projectTasks)
                        {
                            int pctComplete = 0;
                            int.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.TicketPctComplete)), out pctComplete);
                            pTask["TitleWithPctComplete"] = string.Format("{0} ({1}% complete)", row[DatabaseObjects.Columns.Title], pctComplete);
                            pTask[DatabaseObjects.Columns.BehaviourIcon] = uHelper.GetBehaviourIcon(Convert.ToString(pTask[DatabaseObjects.Columns.TaskBehaviour]));
                        }
                    }
                }
            }

            if (openProjectTasks == null || openProjectTasks.Rows.Count <= 0)
            {
                return null;
            }

            DataView view = openProjectTasks.DefaultView;
            view.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ItemOrder);

            return openProjectTasks.DefaultView.ToTable();
        }
        /// <summary>
        /// Update Parent SVC ticket & sibling SVC tasks on the basis of ticket status Update.
        /// </summary>
        /// <param name="parentID"></param>
        public void UpdateDependentSVCTasks(DataRow ticket)
        {
            string ticketId = Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]);
            bool ticketClosed = UGITUtility.StringToBoolean(ticket[DatabaseObjects.Columns.TicketClosed]);

            UGITTaskManager taskManager = new UGITTaskManager(_context);
            List<UGITTask> currentTicketDetails = taskManager.Load(x => x.RelatedTicketID == ticketId);

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref currentTicketDetails);

            foreach (UGITTask item in currentTicketDetails)
            {
                string parentTicketID = Convert.ToString(item.TicketId);
                if (parentTicketID.StartsWith("SVC"))
                {
                    // Update dependent tasks & tickets
                    if (ticketClosed)
                    {
                        StartTasks(parentTicketID, ticketId, false);
                        MoveSVCTicket(parentTicketID, item);
                    }

                    int dependencyId = Convert.ToInt32(item.ID);
                    var task = taskManager.LoadByID(dependencyId);
                    if (UGITUtility.IfColumnExists(ticket, DatabaseObjects.Columns.TicketTotalHoldDuration))
                        task.TotalHoldDuration = UGITUtility.StringToDouble(ticket[DatabaseObjects.Columns.TicketTotalHoldDuration]);
                    if (!ticketClosed)
                    {
                        task.Status = Convert.ToString(UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.TicketStatus));
                        task.PercentComplete = 1;
                    }
                    else
                    {
                        task.Status = Constants.Completed;
                        task.PercentComplete = 100;
                        task.CompletionDate = DateTime.Now;

                    }

                    //if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.SLADisabled, ticket.ParentList))
                    //    task.SLADisabled = UGITUtility.StringToBoolean(ticket[DatabaseObjects.Columns.SLADisabled]);

                    if (uHelper.IfColumnExists(DatabaseObjects.Columns.SLADisabled, ticket.Table))
                        task.SLADisabled = UGITUtility.StringToBoolean(ticket[DatabaseObjects.Columns.SLADisabled]);

                    if (uHelper.IfColumnExists(DatabaseObjects.Columns.NotificationDisabled, ticket.Table))
                        task.NotificationDisabled = UGITUtility.StringToBoolean(ticket[DatabaseObjects.Columns.NotificationDisabled]);

                    if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketOnHold, ticket.Table))
                        task.OnHold = UGITUtility.StringToBoolean(ticket[DatabaseObjects.Columns.TicketOnHold]);

                    if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldTillDate, ticket.Table))
                    {
                        DateTime? onholdTilldate = UGITUtility.StringToDateTime(ticket[DatabaseObjects.Columns.TicketOnHoldTillDate]);
                        task.OnHoldTillDate = onholdTilldate;
                        if (!onholdTilldate.HasValue || onholdTilldate == DateTime.MinValue || onholdTilldate == DateTime.MaxValue)
                            task.OnHoldTillDate = null;
                    }

                    Save(task);

                    // Update parent task for ticket is close
                    if (ticketClosed && task.ParentTaskID > 0)
                    {
                        var tasks = LoadByProjectID(task.TicketId);
                        tasks = MapRelationalObjects(tasks);  // MapRelationshipObjects(tasks);
                        //ModuleInstanceDependency.PropogateTaskEffect(ref tasks, task);
                        SaveAllTasks(tasks);
                    }
                }
            }
        }

        /// <summary>
        /// Save all task which are haves Changes True.
        /// </summary>
        /// <param name="tasks"></param>
        public void SaveAllTasks(List<UGITTask> tasks)
        {
            if (tasks != null && tasks.Count > 0)
            {
                tasks.ForEach(x =>
                {
                    Save(x);
                });

                //TaskCache.ReloadProjectTasks("SVC", tasks[0].ParentInstance, spWeb);
            }
        }
        public bool CheckOwnerIsAssignedToAll(string publicID)
        {
            bool isAssigned = true;
            List<UGITTask> depncies = this.Load(x => x.RelatedTicketID == publicID);
            if (depncies.Count > 0)
            {
                depncies = MapRelationalObjects(depncies);
                if (depncies.Exists(x => x.Behaviour == "Task" && x.ChildCount == 0 && (x.AssignedTo == null || UGITUtility.SplitString(x.AssignedTo, ",").Count() <= 0)))
                {
                    isAssigned = false;
                }
            }
            return isAssigned;
        }

        public void ImportAllTasksUnderPMM(PMMMode mode, string fromTicketId, string toTicketID)
        {
            ImportAllTasksUnderPMM(mode, fromTicketId, toTicketID, 0);
        }
        public void ImportAllTasksUnderPMM(PMMMode mode, string fromTicketId, string toTicketID, long nprTemplateID)
        {
            // Load project to get some datail like start and endDate
            DataRow project = Ticket.GetCurrentTicket(_context, ModuleNames.PMM, toTicketID);

            DateTime projectStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketActualStartDate] == DBNull.Value ? DateTime.MinValue : project[DatabaseObjects.Columns.TicketActualStartDate]);

            if (projectStartDate == DateTime.MinValue)
                projectStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketTargetStartDate] == DBNull.Value ? DateTime.MinValue : project[DatabaseObjects.Columns.TicketTargetStartDate]);
            if (projectStartDate == DateTime.MinValue)
                projectStartDate = DateTime.Now;

            // Fetch lifecycle of PMM project
            LifeCycleManager lifeCycleHelper = new LifeCycleManager(_context);
            LifeCycle lifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM)[0];
            List<UGITTask> lstTasks = new List<UGITTask>();
            List<LifeCycle> objLifeCycle = new List<LifeCycle>();
            long lifecyleid = 0;
            if (mode == PMMMode.Template)
            {
                long templateID = 0;

                if (!string.IsNullOrEmpty(fromTicketId))
                    long.TryParse(Convert.ToString(fromTicketId), out templateID);


                // Delete existing task and import task from selected template
                TaskTemplateManager taskTemplateHelper = new TaskTemplateManager(_context);
                TaskTemplate taskTemplate = taskTemplateHelper.LoadTemplateByID(templateID, toTicketID);
                lstTasks = taskTemplate.Tasks;
            }
            else if (mode == PMMMode.Scratch)
            {
                objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM);
                // Select the first LifeCycle from selected if exits otherwise load first lifecyle
                lifecyleid = UGITUtility.StringToInt(project[DatabaseObjects.Columns.ProjectLifeCycleLookup]);
                if (lifecyleid > 0)
                    lifeCycle = objLifeCycle.FirstOrDefault(x => x.ID == lifecyleid);
                else
                    lifeCycle = objLifeCycle.FirstOrDefault();

                if (lifeCycle != null && lifeCycle.Stages.Count > 0)
                {
                    for (var i = 0; i < lifeCycle.Stages.Count; i++)
                    {
                        UGITTask task = new UGITTask();
                        task.Title = lifeCycle.Stages[i].Name;
                        task.StartDate = DateTime.Now.Date;
                        task.DueDate = DateTime.Now.Date;
                        task.Status = "";
                        task.ItemOrder = i;
                        task.Duration = 1;
                        lstTasks.Add(task);
                    }
                }
                else
                {
                    //create default 5 tasks
                    for (var i = 1; i <= 5; i++)
                    {
                        UGITTask task = new UGITTask();
                        task.Title = $"Task {i}";
                        task.StartDate = DateTime.Now.Date;
                        task.DueDate = DateTime.Now.Date;
                        task.Status = "";
                        task.ItemOrder = i;
                        task.Duration = 1;
                        lstTasks.Add(task);
                    }
                }
            }
            else
            {
                if (nprTemplateID > 0)
                {
                    // Delete existing task and import task from selected template
                    TaskTemplateManager taskTemplateHelper = new TaskTemplateManager(_context);
                    TaskTemplate taskTemplate = taskTemplateHelper.LoadTemplateByID(nprTemplateID, toTicketID);
                    lstTasks = taskTemplate.Tasks;
                }
                else
                {
                    lstTasks = LoadByProjectID(fromTicketId);
                    if (lstTasks.Count == 0)
                    {
                        objLifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleNames.PMM);
                        // Select the first LifeCycle from selected if exits otherwise load first lifecyle
                        lifecyleid = UGITUtility.StringToInt(project[DatabaseObjects.Columns.ProjectLifeCycleLookup]);
                        if (lifecyleid > 0)
                            lifeCycle = objLifeCycle.FirstOrDefault(x => x.ID == lifecyleid);
                        else
                            lifeCycle = objLifeCycle.FirstOrDefault();

                        if (lifeCycle != null && lifeCycle.Stages.Count > 0)
                        {
                            for (var i = 0; i < lifeCycle.Stages.Count; i++)
                            {
                                UGITTask task = new UGITTask();
                                task.Title = lifeCycle.Stages[i].Name;
                                task.StartDate = DateTime.Now.Date;
                                task.DueDate = DateTime.Now.Date;
                                task.Status = "";
                                task.ItemOrder = i;
                                task.Duration = 1;
                                lstTasks.Add(task);
                            }
                        }
                    }
                }
            }

            // Update Start/Due Dates for all the tasks based on the Actual Start Date of project
            //bool NPRTaskdate = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.NPRTaskdate);
            //if (!NPRTaskdate)
            //{
            if (lstTasks != null && lstTasks.Count > 0)
                lstTasks = SetTaskDatesFromProjectStartDate(lifeCycle, projectStartDate, lstTasks);
            //}

            //not needed because already calling this method inside SetTaskDatesFromProjectStartDate()
            // Remanage all tasks i.e. estimate durations, update Successor & Parent Dates
            //ReManageTasks(ref lstTasks);

            if (lstTasks != null && lstTasks.Count > 0)
            {
                List<UGITTask> taskList = MapRelationalObjects(lstTasks);
                lstTasks = ImportTasks(ModuleNames.PMM, taskList, false, toTicketID);

                bool autoCreateRMMProjectAllocation = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
                if (autoCreateRMMProjectAllocation)
                {
                    ResourceAllocationManager allocationManager = new ResourceAllocationManager(_context);
                    ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate () { allocationManager.UpdateProjectPlannedAllocationByUser(taskList, "PMM", Convert.ToString(project[DatabaseObjects.Columns.TicketId]), false); };
                    Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                    sThreadUpdateProjectPlannedAllocation.IsBackground = true;
                    sThreadUpdateProjectPlannedAllocation.Start();

                }
            }

        }

        /// <summary>
        /// This method is used to manage min date for the StartDate & DueDate which is null or has default SQl Min date.
        /// </summary>
        /// <param name="tasks"></param>
        public void ManageMinStartDueDates(ref List<UGITTask> tasks)
        {
            if (tasks != null && tasks.Count > 0)
            {
                tasks.ForEach(x =>
                {
                    if (x.StartDate != DateTime.MinValue && (x.StartDate == null || x.StartDate <= new DateTime(1800, 1, 1)))
                        x.StartDate = DateTime.MinValue;
                    if (x.DueDate != DateTime.MinValue && (x.DueDate == null || x.DueDate <= new DateTime(1800, 1, 1)))
                        x.DueDate = DateTime.MinValue;
                });
            }
        }

        /// <summary>
        /// This method is used to manage min date for the StartDate & DueDate which is null or has default SQl Min date.
        /// </summary>
        /// <param name="task"></param>
        public void ManageMinStartDueDates(ref UGITTask task)
        {
            if (task != null)
            {
                if (task.StartDate != DateTime.MinValue && (task.StartDate == null || task.StartDate <= new DateTime(1800, 1, 1)))
                    task.StartDate = DateTime.MinValue;
                if (task.DueDate != DateTime.MinValue && (task.DueDate == null || task.DueDate <= new DateTime(1800, 1, 1)))
                    task.DueDate = DateTime.MinValue;
            }
        }

        /// <summary>
        /// This method is used to get all the predecessors of current task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="spWeb"></param>
        /// <returns></returns>
        public List<UGITTask> GetTaskPredecessors(string moduleName, UGITTask task)
        {
            List<UGITTask> tasksList = LoadByProjectID(moduleName, task.ParentInstance);

            if (tasksList == null)
                return tasksList;

            tasksList = tasksList.Where(x => x.ID == task.ID).Select(y => y.PredecessorsObj).FirstOrDefault();

            return tasksList;
        }

        /// <summary>
        /// This method is used to Put on Hold/Unhold the SVC Instance if it is needed
        /// </summary>
        public void DoHoldUnHoldSVCInstance(ApplicationContext context, string parentInstanceID, int holdTaskID = 0, bool propagateTicketCache = true, string latestComment = null)
        {
            if (string.IsNullOrEmpty(parentInstanceID))
                return;

            Ticket ticketRequest = new Ticket(_context, ModuleNames.SVC);

            List<UGITTask> instDependencies = LoadByProjectID(parentInstanceID);
            if (instDependencies == null || instDependencies.Count == 0)
                return;
            List<UGITTask> inprogressTask = instDependencies.Where(x => !x.OnHold && x.Status != Constants.Waiting && x.Status != Constants.Completed && x.Status != Constants.Cancelled).ToList();
            UGITTask onHoldTask = null;

            if (holdTaskID > 0)
                onHoldTask = instDependencies.Where(x => x.ID == holdTaskID).FirstOrDefault();
            else
                onHoldTask = instDependencies.Where(x => x.OnHold).OrderBy(x => x.OnHoldTillDate).FirstOrDefault(x => x.OnHoldTillDate != null && x.OnHoldTillDate != DateTime.MinValue);

            DataRow svcItem = Ticket.GetCurrentTicket(context, ModuleNames.SVC, parentInstanceID);

            if (svcItem == null)
                return;

            bool svcTicketIsOnHold = UGITUtility.StringToBoolean(svcItem[DatabaseObjects.Columns.TicketOnHold]);

            bool holdTicket = (!svcTicketIsOnHold && onHoldTask != null && inprogressTask != null && inprogressTask.Count == 0);


            bool unHoldTicket = (svcTicketIsOnHold && inprogressTask != null && inprogressTask.Count > 0);

            if (holdTicket)
            {
                DoHoldSVCInstance(ticketRequest, svcItem, onHoldTask, latestComment: latestComment);
            }
            else if (unHoldTicket)
            {
                svcItem[DatabaseObjects.Columns.TicketOnHold] = "0";
                string newStatus = UGITUtility.SplitString(svcItem[DatabaseObjects.Columns.ModuleStepLookup], Constants.Separator, 1);
                if (string.IsNullOrEmpty(newStatus))
                    newStatus = ticketRequest.GetTicketCurrentStage(svcItem).Name;

                if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketStatus, svcItem.Table))
                    svcItem[DatabaseObjects.Columns.TicketStatus] = newStatus;

                svcItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = DBNull.Value;
                svcItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = DBNull.Value;
                svcItem[DatabaseObjects.Columns.OnHoldReason] = null;
                svcItem[DatabaseObjects.Columns.TicketOnHold] = "0";

                UpdateSVCTicket(svcItem, true);
            }
        }

        /// <summary>
        /// This method is used the Put on Hold the SVC Instance
        /// </summary>
        /// <param name="ticketRequest"></param>
        /// <param name="svcItem"></param>
        /// <param name="dependencies"></param>
        /// <param name="onHoldTaskId"></param>
        /// <param name="holdReason"></param>
        /// <param name="comment"></param>
        /// <param name="propagateTicketsCache"></param>
        //public void DoHoldSVCInstance(Ticket ticketRequest, DataRow svcItem, List<UGITTask> dependencies, long onHoldTaskId, string holdReason, string comment, bool propagateTicketsCache = true)
        //{
        //    if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldStartDate, svcItem.Table))
        //    {
        //        svcItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = DateTime.Now.ToString();
        //    }

        //    // For ticket type task we do not have OnHoldTillDate stored in TicketRelationShip list
        //    UGITTask individualTask = dependencies.OrderBy(x => x.OnHoldTillDate).FirstOrDefault(x => x.OnHoldTillDate != null);
        //    string svccomment = string.Empty;
        //    string reason = string.Empty;

        //    if (individualTask.ID == onHoldTaskId)
        //    {
        //        svccomment = comment;
        //        reason = holdReason;
        //    }

        //    if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldTillDate, svcItem.Table))
        //    {
        //        DateTime? onholdTillDate = individualTask.OnHoldTillDate;

        //        if (!onholdTillDate.HasValue || onholdTillDate == DateTime.MinValue || onholdTillDate == DateTime.MaxValue)
        //            onholdTillDate = DateTime.Today.AddYears(1);

        //        svcItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = onholdTillDate;
        //    }

        //    svcItem[DatabaseObjects.Columns.TicketOnHold] = "1";
        //    svcItem[DatabaseObjects.Columns.Status] = Constants.OnHoldStatus;
        //    svccomment = string.IsNullOrEmpty(svccomment) ? individualTask.Comment : svccomment;
        //    reason = string.IsNullOrEmpty(reason) ? individualTask.OnHoldReasonChoice : reason;

        //    svcItem[DatabaseObjects.Columns.TicketComment] = svccomment;

        //    if (uHelper.IfColumnExists(DatabaseObjects.Columns.OnHoldReason, svcItem.Table))
        //        svcItem[DatabaseObjects.Columns.OnHoldReason] = reason;

        //    ticketRequest.CommitChanges(svcItem);

        //    //DataRow module = uGITCache.GetModuleDetails("SVC", svcItem.Web);

        //    //if (module != null && propagateTicketsCache)
        //    //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(Convert.ToInt32(module[DatabaseObjects.Columns.Id]), svcItem);
        //}
        public void DoHoldSVCInstance(Ticket ticketReq, DataRow svcItem, UGITTask individualTask, bool propagateTicketsCache = true, string latestComment = null)
        {

            svcItem[DatabaseObjects.Columns.TicketOnHold] = "1";
            svcItem[DatabaseObjects.Columns.TicketOnHoldStartDate] = DateTime.Now.ToString();

            DateTime? onholdTillDate = individualTask.OnHoldTillDate;
            if (!onholdTillDate.HasValue || onholdTillDate == DateTime.MinValue || onholdTillDate == DateTime.MaxValue)
                onholdTillDate = DateTime.Today.AddYears(1);
            svcItem[DatabaseObjects.Columns.TicketOnHoldTillDate] = onholdTillDate;
            svcItem[DatabaseObjects.Columns.OnHoldReason] = individualTask.OnHoldReasonChoice;
            svcItem[DatabaseObjects.Columns.TicketStatus] = Constants.OnHoldStatus;

            if (!string.IsNullOrWhiteSpace(latestComment))
                svcItem[DatabaseObjects.Columns.TicketComment] = UGITUtility.GetVersionString(_context.CurrentUser.Name, latestComment, svcItem, DatabaseObjects.Columns.TicketComment);

            ticketReq.CommitChanges(svcItem);
            ModuleViewManager moduleViewManager = new ModuleViewManager(_context);

            if (ticketReq.Module != null && propagateTicketsCache)
                ticketReq.UpdateTicketCache(svcItem, ticketReq.Module.ModuleName);

        }
        public void CallUpdateAllocationOnTaskSave(string assignedTo, UGITTask task, string workItemType, string workItem)
        {
            ResourceAllocationManager resourceAllocManager = new ResourceAllocationManager(_context);
            bool autoCreateRMMProjectAllocation = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
            if (autoCreateRMMProjectAllocation)
            {
                List<UGITTask> taskList = new List<UGITTask>();
                if (task != null && !string.IsNullOrEmpty(assignedTo))
                {
                    taskList.Add(task);
                    List<string> users = new List<string>();
                    users.AddRange(UGITUtility.ConvertStringToList(assignedTo, ";#"));
                    resourceAllocManager.UpdateProjectPlannedAllocation(string.Empty, taskList, users, workItemType, workItem, true);

                    //ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate () { resourceAllocManager.UpdateProjectPlannedAllocation(string.Empty, taskList, users, workItemType, workItem, true); };
                    //Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                    //sThreadUpdateProjectPlannedAllocation.Start();
                }

            }
        }

        /// <summary>
        /// This method is being used to update start/due dates of all tasks based on the project start date
        /// </summary>
        /// <param name="lifeCycle"></param>
        /// <param name="startDate"></param>
        /// <param name="taskTemplate"></param>
        /// <returns></returns>
        public List<UGITTask> SetTaskDatesFromProjectStartDate(LifeCycle lifeCycle, DateTime startDate, List<UGITTask> taskTemplate)
        {
            List<UGITTask> newTaskList = new List<UGITTask>();
            if (taskTemplate == null || taskTemplate.Count == 0)
                return newTaskList;

            ReManageTasks(ref taskTemplate, false);
            DateTime templateStartDate = taskTemplate.Min(x => x.StartDate);

            // If template dates are greater than start date, move them back to one year before start date else it will throw off all the math
            int yearDelta = 0;
            if (templateStartDate > startDate)
            {
                int origYear = templateStartDate.Year;
                int newYear = startDate.Year - 1;
                yearDelta = origYear - newYear;
                templateStartDate = new DateTime(newYear, templateStartDate.Month, templateStartDate.Day);
            }

            int deltaDayDiff = uHelper.GetTotalWorkingDaysBetween(_context, templateStartDate, startDate);

            foreach (UGITTask task in taskTemplate)
            {
                //If step is not exist then remove milestone from task
                if (lifeCycle != null && lifeCycle.Stages.Exists(x => x.StageStep == task.StageStep))
                {
                    task.IsMileStone = true;
                }
                else
                {
                    task.IsMileStone = false;
                    task.StageStep = 0;
                }

                DateTime taskStart = yearDelta > 0 ? task.StartDate.AddYears(-yearDelta) : task.StartDate;
                DateTime taskDue = yearDelta > 0 ? task.DueDate.AddYears(-yearDelta) : task.DueDate;

                DateTime[] startdates = uHelper.GetEndDateByWorkingDays(_context, taskStart, deltaDayDiff);
                DateTime[] endDates = uHelper.GetEndDateByWorkingDays(_context, taskDue, deltaDayDiff);

                // Second item contain new date with default difference
                task.StartDate = startdates[1];
                task.DueDate = endDates[1];
                newTaskList.Add(task);
            }

            newTaskList = newTaskList.OrderBy(x => x.ItemOrder).ToList();

            return newTaskList;
        }

        public DataTable GetAllTasksForQuery()
        {
            DataTable dtAllTasks = null;

            List<UGITTask> tasks = new List<UGITTask>();
            tasks = Load(x => !x.Deleted && !string.IsNullOrEmpty(x.TicketId)
                && (x.ModuleNameLookup.Equals(ModuleNames.PMM) || x.ModuleNameLookup.Equals(ModuleNames.NPR) || x.ModuleNameLookup.Equals(ModuleNames.TSK) || (x.ModuleNameLookup.Equals(ModuleNames.SVC) && x.Behaviour.ToLower() == "task")));

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref tasks);

            dtAllTasks = UGITUtility.ToDataTable<UGITTask>(tasks);
            return dtAllTasks;
        }

        public DataTable GetOpenTasksForQuery()
        {
            DataTable dtOpenTasks = null;

            List<UGITTask> tasks = new List<UGITTask>();
            tasks = Load(x => !x.Deleted && x.Status != "Completed" && !string.IsNullOrEmpty(x.TicketId)
                && (x.ModuleNameLookup.Equals(ModuleNames.PMM) || x.ModuleNameLookup.Equals(ModuleNames.NPR) || x.ModuleNameLookup.Equals(ModuleNames.TSK) || (x.ModuleNameLookup.Equals(ModuleNames.SVC) && x.Behaviour.ToLower() == "task")));

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref tasks);

            dtOpenTasks = UGITUtility.ToDataTable<UGITTask>(tasks);
            return dtOpenTasks;
        }

        public DataTable GetCompletedTasksForQuery()
        {
            DataTable dtOpenTasks = null;

            List<UGITTask> tasks = new List<UGITTask>();
            tasks = Load(x => !x.Deleted && x.Status == "Completed" && !string.IsNullOrEmpty(x.TicketId)
                && (x.ModuleNameLookup.Equals(ModuleNames.PMM) || x.ModuleNameLookup.Equals(ModuleNames.NPR) || x.ModuleNameLookup.Equals(ModuleNames.TSK) || (x.ModuleNameLookup.Equals(ModuleNames.SVC) && x.Behaviour.ToLower() == "task")));

            // Set min date to the Start and Due Dates which are null or has default SQl Min date.
            ManageMinStartDueDates(ref tasks);

            dtOpenTasks = UGITUtility.ToDataTable<UGITTask>(tasks);
            return dtOpenTasks;
        }

        public DataTable GetOpenedTasksByUser(string userID, bool excludeWaitingTasks, string[] excludeModules, bool includeCompleted = false)
        {
            DataTable openTasks = null;

            UserProfile usr = _context.UserManager.GetUserById(userID);
            List<string> expressions = new List<string>();
            if (usr != null)
            {
                expressions.Add(usr.Id);
                List<string> usrGroups = _context.UserManager.GetUserGroups(usr.Id);
                foreach (string g in usrGroups)
                {
                    expressions.Add(g);
                }

                #region Delegate Task For

                string[] delegateUserFor = UGITUtility.SplitString(usr.DelegateUserFor, Constants.Separator6);
                foreach (string uVal in delegateUserFor)
                {
                    UserProfile sndUserProfile = _context.UserManager.LoadById(uVal);
                    if (sndUserProfile != null && DateTime.Now.Date >= sndUserProfile.LeaveFromDate.Date && DateTime.Now.Date <= sndUserProfile.LeaveToDate)
                    {
                        expressions.Add(uVal);
                    }
                }

                expressions = expressions.Distinct().ToList();

                #endregion
            }


            string excludedModulesExp = string.Empty;
            if (excludeModules != null && excludeModules.Length > 0)
            {
                excludedModulesExp = string.Join(" and ", excludeModules.Select(x => string.Format("{0} <> '{1}'", DatabaseObjects.Columns.ModuleNameLookup, x)));
            }


            DataView view = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTasks, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'" + " and " + excludedModulesExp).AsDataView(); //GetDataTable(excludedModulesExp).AsDataView();  // GetTaskCacheInstance().Tasks.AsDataView();
            if (string.IsNullOrWhiteSpace(excludedModulesExp) && !includeCompleted)
                view.RowFilter = string.Format("(SubTaskType in ('ticket','task','') and {0} <> 'Completed' and {0} <> 'Cancelled' and {0} <> 'On Hold')", DatabaseObjects.Columns.Status);
            else if (!string.IsNullOrWhiteSpace(excludedModulesExp) && !includeCompleted)
                view.RowFilter = string.Format("(SubTaskType in ('ticket','task','') and {0} <> 'Completed' and {0} <> 'Cancelled' and {0} <> 'On Hold') and ({1})", DatabaseObjects.Columns.Status, excludedModulesExp);

            else if (string.IsNullOrWhiteSpace(excludedModulesExp) && includeCompleted)
                view.RowFilter = string.Format("(SubTaskType in ('ticket','task','') and {0} <> 'Cancelled' and {0} <> 'On Hold')", DatabaseObjects.Columns.Status);
            else if (!string.IsNullOrWhiteSpace(excludedModulesExp) && includeCompleted)
                view.RowFilter = string.Format("(SubTaskType in ('ticket','task','') and {0} <> 'Cancelled' and {0} <> 'On Hold') and ({1})", DatabaseObjects.Columns.Status, excludedModulesExp);

            DataRow[] rowApproved = uHelper.GetMultipleLookupMultiValueExistData(view.ToTable(), DatabaseObjects.Columns.Approver, expressions, Constants.Separator6);
            DataRow[] rowAssigned = uHelper.GetMultipleLookupMultiValueExistData(view.ToTable(), DatabaseObjects.Columns.AssignedTo, expressions, Constants.Separator6);
            if (rowAssigned.Length > 0)
            {
                openTasks = rowAssigned.CopyToDataTable();
                DataView dView = openTasks.DefaultView;
                dView.RowFilter = string.Format("{0} <> 'waiting'", DatabaseObjects.Columns.Status);
                openTasks = dView.ToTable();

            }

            if (rowApproved.Length > 0)
            {
                //rowApproved = rowApproved.Where(x => !x.IsNull(DatabaseObjects.Columns.ApprovalType) && x.Field<string>(DatabaseObjects.Columns.ApprovalType).ToLower() != ApprovalType.None &&
                //    !x.IsNull(DatabaseObjects.Columns.Approvalstatus) && x.Field<string>(DatabaseObjects.Columns.Approvalstatus).ToLower() == "pending").ToArray();
                //if (rowApproved.Length > 0)
                //{
                //    if (openTasks != null)
                //        openTasks.Merge(rowApproved.CopyToDataTable());
                //    else
                //        openTasks = rowApproved.CopyToDataTable();
                //}
            }
            if (openTasks != null)
                openTasks = openTasks.DefaultView.ToTable(true);

            return openTasks;
        }

        public List<UGITTask> GetOpenedTasksByUserList(string userID, bool excludeWaitingTasks, string[] excludeModules)
        {
            List<UGITTask> openTasks = null;
            List<string> expressions = new List<string>();
            UserProfile usr = _context.UserManager.GetUserById(userID);
            Expression<Func<UGITTask, bool>> exp = (t) => true;
            if (usr != null)
            {
                expressions.Add(usr.Id);
                List<string> usrGroups = _context.UserManager.GetUserGroups(usr.Id);
                foreach (string g in usrGroups)
                {
                    expressions.Add(g);
                }

                #region Delegate Task For

                string[] delegateUserFor = UGITUtility.SplitString(usr.DelegateUserFor, Constants.Separator6);
                foreach (string uVal in delegateUserFor)
                {
                    UserProfile sndUserProfile = _context.UserManager.LoadById(uVal);
                    if (sndUserProfile != null && DateTime.Now.Date >= sndUserProfile.LeaveFromDate.Date && DateTime.Now.Date <= sndUserProfile.LeaveToDate)
                    {
                        expressions.Add(uVal);
                    }
                }

                expressions = expressions.Distinct().ToList();

                #endregion
            }


            string excludedModulesExp = string.Empty;
            if (excludeModules != null && excludeModules.Length > 0)
            {
                excludedModulesExp = string.Join(" and ", excludeModules.Select(x => string.Format("{0} <> '{1}'", DatabaseObjects.Columns.ModuleNameLookup, x)));
            }


            var view = Load(excludedModulesExp);
            exp = x => x.SubTaskType == "ticket" || x.SubTaskType == "task" || x.SubTaskType == "";
            exp = exp.And(x => x.Status != "Completed" && x.Status != "Cancelled" && x.Status != "On Hold");

            openTasks = view.AsQueryable().Where<UGITTask>(exp).ToList();

            List<UGITTask> taskApproved = openTasks.Where(x => expressions.Contains(x.Approver)).ToList();
            List<UGITTask> taskAssigned = openTasks.Where(x => expressions.Contains(x.AssignedTo)).ToList();
            //DataRow[] rowApproved = uHelper.GetMultipleLookupMultiValueExistData(view.ToTable(), DatabaseObjects.Columns.Approver, expressions, Constants.Separator6);
            //DataRow[] rowAssigned = uHelper.GetMultipleLookupMultiValueExistData(view.ToTable(), DatabaseObjects.Columns.AssignedTo, expressions, Constants.Separator6);
            if (taskAssigned != null && taskAssigned.Count > 0)
            {
                List<UGITTask> nonwaitingtasks = taskAssigned.Where(x => x.Status != "Waiting").ToList();
                openTasks = nonwaitingtasks;
            }

            //if (rowApproved.Length > 0)
            //{
            //    //rowApproved = rowApproved.Where(x => !x.IsNull(DatabaseObjects.Columns.ApprovalType) && x.Field<string>(DatabaseObjects.Columns.ApprovalType).ToLower() != ApprovalType.None &&
            //    //    !x.IsNull(DatabaseObjects.Columns.Approvalstatus) && x.Field<string>(DatabaseObjects.Columns.Approvalstatus).ToLower() == "pending").ToArray();
            //    //if (rowApproved.Length > 0)
            //    //{
            //    //    if (openTasks != null)
            //    //        openTasks.Merge(rowApproved.CopyToDataTable());
            //    //    else
            //    //        openTasks = rowApproved.CopyToDataTable();
            //    //}
            //}
            //if (openTasks != null)
            //    openTasks = openTasks.DefaultView.ToTable(true);

            return openTasks;
        }
        private bool GetAssignToDifference(UGITTask current, DataRow relationship)
        {
            List<string> oldAssigneeColl = !string.IsNullOrWhiteSpace(Convert.ToString(relationship[DatabaseObjects.Columns.AssignedTo])) ? UGITUtility.SplitString(Convert.ToString(relationship[DatabaseObjects.Columns.AssignedTo]), Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            List<string> newAssigneeColl = !string.IsNullOrWhiteSpace(current.AssignedTo) ? UGITUtility.SplitString(current.AssignedTo, Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            List<string> difference = new List<string>();

            difference = oldAssigneeColl.Except(newAssigneeColl).ToList();
            if (difference.Count == 0)
                difference = newAssigneeColl.Except(oldAssigneeColl).ToList();

            if (difference != null && difference.Count > 0)
            {
                return true;
            }
            return false;
        }
        private bool GetAssignToDifference(UGITTask current, UGITTask relationship)
        {
            List<string> oldAssigneeColl = !string.IsNullOrWhiteSpace(Convert.ToString(relationship.AssignedTo)) ? UGITUtility.SplitString(Convert.ToString(relationship.AssignedTo), Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            List<string> newAssigneeColl = !string.IsNullOrWhiteSpace(current.AssignedTo) ? UGITUtility.SplitString(current.AssignedTo, Constants.Separator6, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            List<string> difference = new List<string>();

            difference = oldAssigneeColl.Except(newAssigneeColl).ToList();
            if (difference.Count == 0)
                difference = newAssigneeColl.Except(oldAssigneeColl).ToList();

            if (difference != null && difference.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool SendEmailToApprover(DataRow task, ApplicationContext applicationContext, List<UserProfile> newUsers = null)
        {
            if (_context != null)
                applicationContext = _context;
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);
            if (!configurationVariableManager.GetValueAsBool(ConfigConstants.SendEmail))
                return true;

            if (task == null)
                return false;
            string parentTicketId = Convert.ToString(task[DatabaseObjects.Columns.TicketId]);
            string taskTitle = Convert.ToString(task[DatabaseObjects.Columns.Title]);

            string emailTitle = string.Format("{1} Approval Required for Task: {0}", taskTitle, parentTicketId);
            string emailBody = "Your approval is required for the task";

            string greeting = configurationVariableManager.GetValue("Greeting");
            string signature = configurationVariableManager.GetValue("Signature");

            string emailBodyFull = string.Empty;
            string emails = string.Empty;
            string names = string.Empty;

            if (newUsers != null && newUsers.Count > 0)
            {
                emails = string.Join(Constants.UserInfoSeparator, newUsers.Where(x => x.Email != string.Empty).Select(x => x.Email));
                names = string.Join(Constants.UserInfoSeparator, newUsers.Where(x => x.Name != string.Empty).Select(x => x.Name));
            }
            else
            {
                UserProfileManager userProfileManager = new UserProfileManager(applicationContext);
                List<UsersEmail> emailUsers = userProfileManager.GetUsersEmail(task, new string[] { DatabaseObjects.Columns.Approver }, false);
                if (emailUsers != null)
                {
                    emails = string.Join(Constants.UserInfoSeparator, emailUsers.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email).ToList());
                    names = string.Join(Constants.UserInfoSeparator, emailUsers.Where(x => !string.IsNullOrWhiteSpace(x.UserName)).Select(x => x.UserName).ToList());
                }
            }

            if (!string.IsNullOrEmpty(emails))
            {
                Dictionary<string, string> taskToEmail = new Dictionary<string, string>();
                taskToEmail.Add("ProjectID", parentTicketId);
                //GetTableDataManager.GetDataRow()
                ModuleViewManager moduleViewManager = new ModuleViewManager(applicationContext);
                UGITModule uGITModule = moduleViewManager.GetByName(uHelper.getModuleNameByTicketId(parentTicketId));
                DataRow[] dataRows = GetTableDataManager.GetDataRow(uGITModule.ModuleTable, DatabaseObjects.Columns.TicketId, parentTicketId);
                DataRow parentTicket = null;
                if (dataRows != null && dataRows.Length > 0)
                    parentTicket = dataRows[0];
                //SPListItem parentTicket = uHelper.GetTicket(parentTicketId, task.Web);
                if (parentTicket != null)
                    taskToEmail.Add("ProjectTitle", Convert.ToString(parentTicket[DatabaseObjects.Columns.Title]));

                taskToEmail.Add("Title", Convert.ToString(task[DatabaseObjects.Columns.Title]));
                taskToEmail.Add("Description", Convert.ToString(task[DatabaseObjects.Columns.Description]));
                if (task[DatabaseObjects.Columns.StartDate] != null)
                    taskToEmail.Add("StartDate", UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(task[DatabaseObjects.Columns.StartDate]), false));
                if (task[DatabaseObjects.Columns.DueDate] != null)
                    taskToEmail.Add("DueDate", UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(task[DatabaseObjects.Columns.DueDate]), false));

                taskToEmail.Add("EstimatedHours", Convert.ToString(task[DatabaseObjects.Columns.TaskEstimatedHours]));
                taskToEmail.Add("ActualHours", Convert.ToString(task[DatabaseObjects.Columns.TaskActualHours]));
                taskToEmail.Add("Status", Convert.ToString(task[DatabaseObjects.Columns.Status]));
                taskToEmail.Add("% Complete", Convert.ToString(task[DatabaseObjects.Columns.PercentComplete]));
                taskToEmail.Add("IsService", "true");

                //Change last, first name to first, last name
                List<string> userNames = UGITUtility.ConvertStringToList(names, ";");
                for (int i = 0; i < userNames.Count; i++)
                {
                    userNames[i] = UGITUtility.ConvertToFirstMLast(userNames[i]);
                }

                string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", Convert.ToString(task[DatabaseObjects.Columns.TicketId]), Convert.ToString(task[DatabaseObjects.Columns.Id]), "SVC");
                string titleLink = "<a href='" + url + "'>" + taskTitle + "</a>";

                string moduleName = uGITModule.ModuleName;

                if (UGITUtility.ObjectToString(task[DatabaseObjects.Columns.UGITSubTaskType]) == ServiceSubTaskType.AccessTask.ToLower())
                {
                    string serviceAppAcessXml = UGITUtility.ObjectToString(task[DatabaseObjects.Columns.ServiceApplicationAccessXml]);
                    string value = !string.IsNullOrWhiteSpace(serviceAppAcessXml) ? HttpUtility.HtmlDecode(serviceAppAcessXml) : string.Empty;
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(value))
                            doc.LoadXml(value.Trim());
                        else
                            return false;
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, "Missing or invalid ServiceApplicationAccessXml");
                        return false;
                    }

                    List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
                    ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                    try
                    {
                        serviceMatrixData = uHelper.DeSerializeAnObject(doc, serviceMatrixData) as ServiceMatrixData;
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, "Invalid ServiceApplicationAccessXml");
                        return false;
                    }

                    if (serviceMatrixData != null)
                        serviceMatrixDataList.Add(serviceMatrixData);

                    string appAccessTaskDtls = uHelper.CreateApplicationAccessTable(serviceMatrixDataList, applicationContext);
                    //Apprve Reject action code
                    emailBody = string.Format("{0}: {1}", emailBody, titleLink);
                    //Get Approve/Reject action button in Email
                    emailBody = SetApproveRejectInEmail(emailBody, moduleName, task);
                    emailBodyFull = string.Format(@"{0} <b>{1}</b><br/><br/>
                                                {2}<br/><br/>
                                                {4}<br/><br/>
                                                {3}<br/>", greeting, string.Join(", ", userNames), emailBody, signature, appAccessTaskDtls);
                }
                else
                {

                    emailBody = string.Format("{0}: {1}", emailBody, titleLink);
                    //Get Approve/Reject action button in Email
                    emailBody = SetApproveRejectInEmail(emailBody, moduleName, task);
                    emailBodyFull = string.Format(@"{0} <b>{1}</b><br/><br/>
                                                {2}<br/><br/>
                                                {3}<br/>", greeting, string.Join(", ", userNames), emailBody, signature);
                }

                emailBodyFull += HttpUtility.HtmlDecode(UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, "", true, false,_context.TenantID));

                MailMessenger mail = new MailMessenger(applicationContext);
                if (configurationVariableManager.GetValueAsBool(ConfigConstants.KeepSVCTaskNotifications) && !string.IsNullOrWhiteSpace(moduleName) && moduleName == "SVC")
                    mail.SendMail(emails, emailTitle, "", emailBodyFull, true, new string[] { }, true, saveToTicketId: parentTicketId); // Pass ticket ID to save email
                else
                    mail.SendMail(emails, emailTitle, "", emailBodyFull, true, new string[] { }, true);
            }

            return true;
        }

        public bool SendEmailToAssignee(UGITTask task, ApplicationContext applicationContext)
        {
            if (_context != null)
                applicationContext = _context;
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(applicationContext);

            if (!configurationVariableManager.GetValueAsBool(ConfigConstants.SendEmail))
                return true;

            string emailTitle = string.Format("{0} {1}: {2}", task.ParentInstance, task.Status != Constants.Completed ? "New Task Assigned" : "Task Completed", task.Title);
            string emailBody = (task.Status != Constants.Completed ? "A new task has been assigned to you" : "Task Completed");

            string greeting = configurationVariableManager.GetValue("Greeting");
            string signature = configurationVariableManager.GetValue("Signature");

            string emailBodyFull = string.Empty;
            UserProfileManager userProfileManager = new UserProfileManager(applicationContext);
            UserProfileManager.UsersInfo emailUsers = userProfileManager.GetUserInfo(task.AssignedTo);
            if (emailUsers != null && !string.IsNullOrWhiteSpace(emailUsers.userEmails))
            {
                Dictionary<string, string> taskToEmail = new Dictionary<string, string>();
                taskToEmail.Add("ProjectID", task.ParentInstance);
                ModuleViewManager moduleViewManager = new ModuleViewManager(applicationContext);
                UGITModule uGITModule = moduleViewManager.GetByName(uHelper.getModuleNameByTicketId(task.ParentInstance));
                if (uGITModule != null)
                {
                    DataRow[] dataRows = GetTableDataManager.GetDataRow(uGITModule.ModuleTable, DatabaseObjects.Columns.TicketId, task.ParentInstance);
                    DataRow parentTicket = null;
                    if (dataRows != null && dataRows.Length > 0)
                        parentTicket = dataRows[0];

                    if (parentTicket != null)
                        taskToEmail.Add("ProjectTitle", Convert.ToString(parentTicket[DatabaseObjects.Columns.Title]));
                }
                taskToEmail.Add("Title", task.Title);
                taskToEmail.Add("Description", task.Description);
                if (task.StartDate != DateTime.MinValue)
                    taskToEmail.Add("StartDate", UGITUtility.GetDateStringInFormat(task.StartDate, false));
                if (task.EndDate.HasValue)
                    taskToEmail.Add("DueDate", UGITUtility.GetDateStringInFormat(task.EndDate.Value, false));

                taskToEmail.Add("EstimatedHours", task.EstimatedHours.ToString());
                taskToEmail.Add("ActualHours", task.ActualHours.ToString());
                taskToEmail.Add("Status", task.Status);
                taskToEmail.Add("% Complete", task.PercentComplete.ToString());
                taskToEmail.Add("IsService", "true");

                //Change last, first name to first, last name
                List<string> userNames = UGITUtility.ConvertStringToList(emailUsers.userNames, ";");
                for (int i = 0; i < userNames.Count; i++)
                {
                    userNames[i] = UGITUtility.ConvertToFirstMLast(userNames[i]);
                }

                string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", Convert.ToString(task.ParentInstance), task.ID, "SVC");
                string titleLink = " <a href='" + url + "'>" + task.Title + "</a>";
                if (task.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
                {
                    string value = !string.IsNullOrWhiteSpace(task.ServiceApplicationAccessXml) ? HttpUtility.HtmlDecode(task.ServiceApplicationAccessXml) : string.Empty;
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(value))
                            doc.LoadXml(value.Trim());
                        else
                            return false;
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, "Missing or invalid ServiceApplicationAccessXml");
                        return false;
                    }

                    List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
                    ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                    try
                    {
                        serviceMatrixData = uHelper.DeSerializeAnObject(doc, serviceMatrixData) as ServiceMatrixData;
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex, "Invalid ServiceApplicationAccessXml");
                        return false;
                    }

                    if (serviceMatrixData != null)
                        serviceMatrixDataList.Add(serviceMatrixData);

                    string appAccessTaskDtls = uHelper.CreateApplicationAccessTable(serviceMatrixDataList, applicationContext);
                    emailBodyFull = string.Format(@"{0} <b>{1}</b><br/><br/>
                                                {2}: {3}<br/><br/>
                                                {5}<br/><br/>
                                                {4}<br/>", greeting, string.Join(", ", userNames), emailBody, titleLink, signature, appAccessTaskDtls);
                }
                else
                {
                    emailBodyFull = string.Format(@"{0} <b>{1}</b><br/><br/>
                                                {2}: {3}<br/><br/>
                                                {4}<br/>", greeting, string.Join(", ", userNames), emailBody, titleLink, signature);
                }


                emailBodyFull += HttpUtility.HtmlDecode(UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, _context.TenantID));


                MailMessenger mail = new MailMessenger(applicationContext);
                string moduleName = uHelper.getModuleNameByTicketId(task.ParentInstance);
                if (configurationVariableManager.GetValueAsBool(ConfigConstants.KeepSVCTaskNotifications) && !string.IsNullOrWhiteSpace(moduleName) && moduleName == "SVC")
                    mail.SendMail(emailUsers.userEmails, emailTitle, "", emailBodyFull, true, new string[] { }, true, saveToTicketId: task.ParentInstance); // Pass ticket ID to save email
                else
                    mail.SendMail(emailUsers.userEmails, emailTitle, "", emailBodyFull, true, new string[] { }, true);
            }

            return true;
        }
        /// <summary>
        /// To set Approve/Reject Action button in Email body
        /// </summary>
        /// <returns></returns>
        private string SetApproveRejectInEmail(string emailBody, string moduleName, DataRow task)
        {
            //Approve Reject action code
            string imageUrlApprove = UGITUtility.GetAbsoluteURL("/_layouts/15/images/uGovernIT/ButtonImages/approve_btn.png");
            string lnkUrlApprove = string.Format("{0}?taskID={1}&control=approvereject&ModuleName={2}&UserAction=Approve&SVCTask=true&parentTaskID={3}", UGITUtility.GetAbsoluteURL("/SitePages/ApproveReject.aspx"), Convert.ToString(task[DatabaseObjects.Columns.Id]), moduleName, Convert.ToString(task[DatabaseObjects.Columns.TicketId]));
            emailBody = string.Format("{0}<br/><br/><a style='text-decoration: none' href='{2}'><img title='Approve' border='0' src='{1}'></a>&nbsp;", emailBody, imageUrlApprove, lnkUrlApprove);
            string imageUrlReject = UGITUtility.GetAbsoluteURL("/_layouts/15/images/uGovernIT/ButtonImages/reject_btn.png");
            string lnkUrlReject = string.Format("{0}?taskID={1}&control=approvereject&ModuleName={2}&UserAction=Reject&SVCTask=true&parentTaskID={3}", UGITUtility.GetAbsoluteURL("/SitePages/ApproveReject.aspx"), Convert.ToString(task[DatabaseObjects.Columns.Id]), moduleName, Convert.ToString(task[DatabaseObjects.Columns.TicketId]));
            emailBody = string.Format("{0}<a style='text-decoration: none;padding-left:5px;' href='{2}'><img title='Approve' border='0' src='{1}'></a>&nbsp;", emailBody, imageUrlReject, lnkUrlReject);
            return emailBody;
        }
        // Update TicketStageActionUsers to show list of people/groups we are waiting on
        public void UpdateActionUsers(DataRow svcTicket, List<UGITTask> tasks)
        {
            ConfigurationVariableManager configurationVariableManager = new ConfigurationVariableManager(_context);
            if (!configurationVariableManager.GetValueAsBool(ConfigConstants.UpdateSVCPRPORP))
                return;

            List<UGITTask> activeTasks = tasks.Where(x => x.PercentComplete < 100.0f && x.Status != "Waiting").ToList();
            List<string> assignees = GetAllAssignees(activeTasks);
            if (assignees != null && assignees.Count > 0)
            {
                string actionUsers = string.Empty;
                foreach (string assignee in assignees)
                {
                    if (!string.IsNullOrWhiteSpace(actionUsers))
                        actionUsers += Constants.Separator;
                    if (!string.IsNullOrWhiteSpace(assignee))
                        actionUsers += assignee;
                    else
                        actionUsers += assignee;
                }

                svcTicket[DatabaseObjects.Columns.TicketStageActionUsers] = actionUsers;
            }
        }
        public List<string> GetAllAssignees(List<UGITTask> tasks)
        {
            List<string> assignees = new List<string>();
            if (tasks != null && tasks.Count > 0)
            {
                List<UGITTask> lstTasks = tasks.Where(x => x.Behaviour.ToLower() == "task" && x.AssignedTo != null).ToList();
                if (lstTasks != null && lstTasks.Count > 0)
                {
                    foreach (UGITTask depcy in lstTasks)
                    {
                        if (!string.IsNullOrWhiteSpace(depcy.AssignedTo))
                        {
                            if (!assignees.Exists(x => x == depcy.AssignedTo))
                                assignees.Add(depcy.AssignedTo);
                        }
                    }
                }

                List<UGITTask> lstTickets = tasks.Where(x => x.Behaviour.ToLower() == "ticket").ToList();
                if (lstTickets != null && lstTickets.Count > 0)
                {
                    string prevModuleName = string.Empty;
                    List<string> viewFields = null;
                    foreach (UGITTask depcy in lstTickets)
                    {
                        string ticketId = depcy.RelatedTicketID;
                        string thisModuleName = depcy.RelatedModule;
                        if (thisModuleName != prevModuleName)
                        {
                            ModuleUserTypeManager moduleUserTypeManager = new ModuleUserTypeManager(_context);
                            List<ModuleUserType> moduleUserTypes = moduleUserTypeManager.Load(x => x.ModuleNameLookup.EqualsIgnoreCase(thisModuleName));
                            viewFields = moduleUserTypes.Select(x => x.FieldName).ToList();
                            viewFields.Add(DatabaseObjects.Columns.TicketId);
                            viewFields.Add(DatabaseObjects.Columns.TicketStageActionUserTypes);
                        }

                        DataRow ticket = Ticket.GetCurrentTicket(_context, thisModuleName, ticketId, viewFields);
                        if (ticket != null && uHelper.IfColumnExists(DatabaseObjects.Columns.TicketStageActionUserTypes, ticket.Table))
                        {

                            List<string> ticketAssignees = uHelper.GetActionUsersList(_context, ticket);
                            if (ticketAssignees != null && ticketAssignees.Count > 0)
                            {
                                foreach (string user in ticketAssignees)
                                {
                                    if (!assignees.Exists(x => x == user))
                                        assignees.Add(user);
                                }
                            }
                        }
                    }
                }
            }

            return assignees;
        }
        public DataTable LoadTaskTable(string moduleName, string refTicketId = "")
        {
            DataTable taskTable = null;
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", _context.TenantID);
            values.Add("@ModuleName", moduleName);
            values.Add("@TicketId", refTicketId);
            taskTable = uGITDAL.ExecuteDataSetWithParameters("usp_GetModuleTasks", values);
            return taskTable;
        }

        public string GetMinimumTaskStartDate(string ModuleName, string TicketId)
        {
            List<UGITTask> lstTasks = new List<UGITTask>();
            lstTasks = LoadByProjectID(TicketId);
            DateTime StartDate = lstTasks.Min(x => x.StartDate);
            return uHelper.GetDateStringInFormat(_context, StartDate, false);
        }

        public static void CalculateContribution(DataTable tasks, double projectDuration)
        {
            if (projectDuration == 0 || tasks == null || tasks.Rows.Count == 0 || !tasks.Columns.Contains(DatabaseObjects.Columns.UGITContribution))
                return;

            CalculateContribution(tasks.Select(), projectDuration);
        }

        public static void CalculateContribution(DataRow[] tasks, double projectDuration)
        {
            if (tasks.Length <= 0)
                return;

            foreach (DataRow task in tasks)
            {
                task[DatabaseObjects.Columns.UGITContribution] = 0;
                if (projectDuration > 0)
                    task[DatabaseObjects.Columns.UGITContribution] = Math.Round(Convert.ToDouble(task[DatabaseObjects.Columns.UGITDuration]) / projectDuration * 100, 0);
            }
        }
    }


    public interface IUGITTaskManager : IManagerBase<UGITTask> { }
}
