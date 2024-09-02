using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates.RMONE;

namespace uGovernIT.Web
{
    public partial class ModuleStageTask : UserControl
    {
        public string TicketPublicID = string.Empty;
        public string ModuleName = string.Empty;
        public string ModuleStageId = string.Empty;
        public string Type = string.Empty;
        public string isModuleConstraint= string.Empty;
        public string ModuleStepId { get; set; }
        //public bool IsRequestFromSummaryOrTask { get; set; }

        public int ConstraintId = 0;
        int viewType = 0;

        private ApplicationContext _context = null;
        private ModuleViewManager _moduleViewManager = null;
        private TicketManager _ticketManager = null;
        private ModuleStageConstraintsManager _moduleStageConstraintsManager = null;
        private ConfigurationVariableManager _configurationVariableHelper = null;
        private LifeCycleStageManager _lifeCycleStageManager = null;
        private UserProfileManager _userProfileManager = null;
       

        //SPList splst;
        //SPListItem taskItem = null;
        DataRow projectItem = null;

        List<ModuleStageConstraints> taskItem = null;
        ModuleStageConstraints currerntModuleStageTask = null;

        protected string ajaxHelperURL = string.Empty;  

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }

        protected ModuleViewManager ModuleViewManager
        {

            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(ApplicationContext);
                }
                return _ticketManager;
            }
        }

        protected ModuleStageConstraintsManager ModuleStageConstraintsManager
        {
            get
            {
                if (_moduleStageConstraintsManager == null)

                {
                    _moduleStageConstraintsManager = new ModuleStageConstraintsManager(ApplicationContext);
                }
                return _moduleStageConstraintsManager;
            }

        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableHelper == null)

                {
                    _configurationVariableHelper = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configurationVariableHelper;
            }

        }

        protected LifeCycleStageManager LifeCycleStageManager
        {
            get
            {
                if (_lifeCycleStageManager == null)

                {
                    _lifeCycleStageManager = new LifeCycleStageManager(ApplicationContext);
                }
                return _lifeCycleStageManager;
            }

        }

        protected UserProfileManager UserProfileManager
        {
            get
            {
                if (_userProfileManager == null)
                {
                    _userProfileManager = new UserProfileManager(ApplicationContext);
                }
                return _userProfileManager;
            }
        }

        public UGITModuleConstraint ModuleTask = new UGITModuleConstraint();

        protected override void OnInit(EventArgs e)
        {
            //cross check for correct task. by ticketid
            # region changeEditView
            int.TryParse(Request["viewType"], out viewType);

            trAutoApprove.Visible = false;
            if (viewType != 1 && viewType != -1)
            {
                btSaveAndNotify.Visible = true;
            }

            if (viewType == 1)
            {
                txtTitle.Visible = false;
                peAssignedTo.Visible = false;
                dtcStartDate.Visible = false;
                dtcDueDate.Visible = false;
                txtEstimatedHours.Visible = false;
                txtItemOrder.Visible = false;
                btSaveMyTask.Visible = true;
                txtDescription.Visible = false;
                trComment.Visible = true;
                btSaveTask.Visible = true;
                btDelete.Visible = false;
                lbTitle.Visible = true;
                lbAssignedTo.Visible = true;
                lbStartDate.Visible = true;
                lbDueDate.Visible = true;
                lbEstimatedHours.Visible = true;
                btSaveTask.Visible = false;
                lbDescription.Visible = true;
                lblItemOrder.Visible = true;
            }
            else if (viewType == -1)
            {
                txtTitle.Visible = false;
                peAssignedTo.Visible = false;
                dtcStartDate.Visible = false;
                dtcDueDate.Visible = false;
                txtEstimatedHours.Visible = false;
                txtItemOrder.Visible = false;
                btSaveMyTask.Visible = false;
                txtDescription.Visible = false;
                trComment.Visible = true;
                btSaveTask.Visible = false;
                btDelete.Visible = false;
                lbTitle.Visible = true;
                lbAssignedTo.Visible = true;
                lbStartDate.Visible = true;
                lbDueDate.Visible = true;
                lbEstimatedHours.Visible = true;

                lbDescription.Visible = true;
                lblItemOrder.Visible = true;
                lbStage.Visible = true;
                ddlModuleStep.Visible = false;
                ddlStatus.Visible = false;
                lbStatus.Visible = true;
                btProposeNewDate.Visible = false;
                lbActualHours.Visible = true;
                txtActualHours.Visible = false;
                txtPctComplete.Visible = false;
                lbPctComplete.Visible = true;
            }
            #endregion

            // splst = SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleStageConstraints);
            //DataTable moduleStageConstraints = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStageConstraints, $"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
            //List<ModuleStageConstraints> moduleStageConstraintslist = ModuleStageConstraintsManager.Load();


            ajaxHelperURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
            if (this.ConstraintId > 0)
            {
                //Need to check 
                //taskItem = splst.GetItemById(Convert.ToInt32(this.ConstraintId));
                currerntModuleStageTask = ModuleStageConstraintsManager.LoadByID( ConstraintId); //moduleStageConstraintslist.FirstOrDefault(x => x.ID == ConstraintId);
                if (currerntModuleStageTask != null)
                {
                    taskItem = new List<ModuleStageConstraints>();
                    taskItem.Add(currerntModuleStageTask);
                }
            }
            else if (Type != null && Type.Equals(Constants.NewConstraint))
            {
                ModuleTask.ModuleName = ModuleName;
                ModuleTask.TicketPublicID = TicketPublicID;


                if (!IsPostBack)
                    BindModuleStep(ModuleName);

                if (Request["moduleStepId"] != null)
                {
                    ModuleStepId = Request["moduleStepId"];
                    if (!IsPostBack)
                        ddlModuleStep.SelectedIndex = ddlModuleStep.Items.IndexOf(ddlModuleStep.Items.FindByValue(ModuleStepId));
                }
                ModuleTask.ID = 0;
                ModuleTask.ConstraintType = DatabaseObjects.ContentType.ModuleTaskCT;
                FillFormData();
            }
            if (taskItem != null)
            {

                // ModuleTask = UGITModuleConstraint.LoadItem(Context.Current.Web, taskItem);
                ModuleTask = UGITModuleConstraint.LoadItem(ApplicationContext, taskItem);

                TicketPublicID = ModuleTask.TicketPublicID;
                ConstraintId = ModuleTask.ID;
                ModuleName = ModuleTask.ModuleName;

                this.ModuleStepId = ModuleTask.ModuleStep;
                // projectItem = Ticket.getCurrentTicket(this.ModuleName, TicketPublicID);
                var module = ModuleViewManager.LoadByName(ModuleName);
                var projectItem = TicketManager.GetByTicketID(module, TicketPublicID);

                if (!IsPostBack)
                    BindModuleStep(ModuleName);
            }

            #region Task
            if (ModuleTask.ID > 0)
            {
                if (ModuleTask != null)
                {
                    string approveProposedDateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=modulestagetaskapprovedate");
                    btApprove.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}', 'moduleName={1}&taskid={2}&ticketid={4}&action=1', '{1} Task: {3}', 70, 50, 0, '{5}');return false;", approveProposedDateUrl, ModuleName, ModuleTask.ID, UGITUtility.TruncateWithEllipsis(ModuleTask.Title, 70, "."), TicketPublicID, Uri.EscapeUriString(Request.Url.AbsolutePath)));
                    btReject.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}', 'moduleName={1}&taskid={2}&ticketid={4}&action=2', '{1} Task: {3}', 70, 50, 0, '{5}');return false;", approveProposedDateUrl, ModuleName, ModuleTask.ID, UGITUtility.TruncateWithEllipsis(ModuleTask.Title, 70, "."), TicketPublicID, Uri.EscapeUriString(Request.Url.AbsolutePath)));

                    string proposedDateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=moduletaskproposeadate");
                    btProposeNewDate.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}', 'moduleName={1}&taskid={2}&ticketid={4}&tasksource=modulestage', '{1} Task: {3}', 70, 50, 0, '{5}');return false;", proposedDateUrl, ModuleName, ModuleTask.ID, UGITUtility.TruncateWithEllipsis(ModuleTask.Title, 70, "."), TicketPublicID, Uri.EscapeUriString(Request.Url.AbsolutePath)));

                    if (viewType == 1)
                    {
                        btApprove.Visible = false;
                        btReject.Visible = false;
                        btProposeNewDate.Visible = false;

                        if (ModuleTask.ProposedDateStatus == UGITTaskProposalStatus.Pending_Date)
                        {
                            trProposedDueDate.Visible = true;
                            lbProposedDate.Visible = true;

                            if (ModuleTask.ProposedDate != null)
                                lbProposedDate.Text = string.Format("{0:MMM-dd-yyyy} <b style='color:red;'>(Approval Pending)</b>", ModuleTask.ProposedDate);
                            btProposeNewDate.Visible = false;
                        }
                        else
                        {
                            trProposedDueDate.Visible = false;
                            btProposeNewDate.Visible = true;
                        }
                    }
                    else if (ModuleTask.ProposedDateStatus == UGITTaskProposalStatus.Pending_Date)
                    {
                        if (ModuleTask.ProposedDate != null)
                            lbProposedDate.Text = string.Format("{0:MMM-dd-yyyy}", ModuleTask.ProposedDate);

                        trProposedDueDate.Visible = true;
                        lbProposedDate.Visible = true;
                        btApprove.Visible = true;
                        btReject.Visible = true;
                    }

                    txtTitle.Text = ModuleTask.Title;
                    lbTitle.Text = ModuleTask.Title;
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(ModuleTask.Status));
                    if (ddlStatus.SelectedIndex > 0)
                        lbStatus.Text = ddlStatus.SelectedItem.Text;

                    txtPctComplete.Text = ModuleTask.PercentageComplete.ToString();
                    lbPctComplete.Text = ModuleTask.PercentageComplete.ToString();

                    if (ModuleTask.AssignedTo != null && ModuleTask.AssignedTo.Length > 0)
                    {
                        if (ModuleTask.AssignedTo != null)
                        {
                            var userAssign = Convert.ToString(ModuleTask.AssignedTo);
                            if (userAssign != null)
                            {
                                peAssignedTo.SetValues(userAssign);
                                // peAssignedTo.UpdateEntities(UGITUtility.getUsersListFromCollection(ModuleTask.AssignedTo));
                                //lbAssignedTo.Text = UserProfileManager.CommaSeparatedIdsFrom(ModuleTask.AssignedTo, "; ");
                                var users = UserProfileManager.GetUserInfosById(ModuleTask.AssignedTo);
                                if (users != null && users.Count > 0)
                                {
                                    lbAssignedTo.Text = string.Join(Constants.UserInfoSeparator, users.Select(x => x.Name).OrderBy(x => x).ToList());
                                }
                            }

                        }
                    }

                    //dtcStartDate.SelectedDate = ModuleTask.StartDate;
                    dtcStartDate.Date = UGITUtility.StringToDateTime(ModuleTask.StartDate);
                    lbStartDate.Text = ModuleTask.StartDate.ToString("MMM-dd-yyyy");
                    //dtcDueDate.SelectedDate = ModuleTask.DueDate;
                    dtcDueDate.Date = UGITUtility.StringToDateTime(ModuleTask.DueDate);
                    lbDueDate.Text = ModuleTask.DueDate.ToString("MMM-dd-yyyy");
                    txtDescription.Text = ModuleTask.Description;
                    lbDescription.Text = ModuleTask.Description;

                    txtEstimatedHours.Text = ModuleTask.EstimatedHours.ToString();
                    lbEstimatedHours.Text = string.Format("{0}", ModuleTask.EstimatedHours);
                    txtActualHours.Text = ModuleTask.ActualHours.ToString();
                    lbActualHours.Text = string.Format("{0}", ModuleTask.ActualHours);

                    lblItemOrder.Text = Convert.ToString(ModuleTask.ItemOrder);
                    txtItemOrder.Text = Convert.ToString(ModuleTask.ItemOrder);

                    rComments.DataSource = ModuleTask.CommentHistory;
                    rComments.DataBind();

                    if (!IsPostBack)
                    {
                        if (ddlModuleStep.Items.FindByValue(ModuleTask.ModuleStep) == null)
                        {

                            //UGITModule currentModule = uGITCache.ModuleConfigCache.GetCachedModule(ApplicationContext, ModuleName);
                            var currentModule = ModuleViewManager.LoadByName(ModuleName);
                            // UGITModule currentModule = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, ModuleName);
                            if (currentModule != null)
                            {
                                LifeCycle currentLifeCycle = currentModule.List_LifeCycles.FirstOrDefault();
                                if (currentLifeCycle != null)
                                {
                                    List<LifeCycleStage> stages = currentLifeCycle.Stages;
                                    if (stages != null && stages.Count > 0)
                                    {
                                        LifeCycleStage taskStage = stages.FirstOrDefault(x => x.StageStep == UGITUtility.StringToInt(ModuleTask.ModuleStep));
                                        ddlModuleStep.Items.Insert(0, new ListItem(taskStage.Name, Convert.ToString(taskStage.StageStep)));
                                    }
                                }
                            }
                        }
                        ddlModuleStep.SelectedIndex = ddlModuleStep.Items.IndexOf(ddlModuleStep.Items.FindByValue(ModuleTask.ModuleStep));
                    }

                    if (ddlModuleStep.SelectedIndex > 0)
                        lbStage.Text = ddlModuleStep.SelectedItem.Text;

                    //completed on 
                    if (ModuleTask.Status == "Completed")
                    {
                        if (ModuleTask.CompletedBy != null)
                            lblCompletedOn.Text = string.Format("{0} by {1}", UGITUtility.GetDateStringInFormat(ModuleTask.CompletionDate, true), ModuleTask.CompletedBy);
                        else
                            lblCompletedOn.Text = UGITUtility.GetDateStringInFormat(ModuleTask.CompletionDate, true);

                        tdCompletedOn.Visible = true;
                    }
                }
            }
            #endregion

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPctComplete.Attributes.Add("onblur", "modifyStatusFromPctComplete()");
            ddlStatus.Attributes.Add("onchange", "modifyPctCompleteFromStatus()");
        }

        #region common
        protected void BtSaveTask_Click(object sender, EventArgs e)
        {
            ASPxButton btAction = (ASPxButton)sender;
            //Button btAction = (Button)sender;
            bool isError = false;

            if (isError || !Page.IsValid)
            {
                return;
            }
            if (isModuleConstraint == "1")
            {
                Session["fromCommentTask"] = "1";
            }
            else
            {
                Session["fromCommentTask"] = null;
            }
            //Creates uservaluecollection object from peoplepicker control
            string assignedUser = string.Empty;
            List<string> assignedUsers = new List<string>();
            //SPFieldUserValueCollection userMultiLookup = new SPFieldUserValueCollection();
            var userMultiLookup = new List<UserProfile>();
            if (trAssignedTo.Visible)
            {
                userMultiLookup = UserProfileManager.GetUserInfosById(peAssignedTo.GetValues());// check method GetUserValueCollection
                //for (int i = 0; i < peAssignedTo.Accounts.Count; i++)
                //{
                //    PickerEntity entity = (PickerEntity)peAssignedTo.Entities[i];
                //    if (entity != null && entity.Key != null)
                //    {
                //        SPUser user = UserProfile.GetUserByName(entity.Key, SPPrincipalType.User);
                //        if (user != null)
                //        {
                //            SPFieldUserValue userLookup = new SPFieldUserValue();
                //            userLookup.LookupId = user.ID;
                //            userMultiLookup.Add(userLookup);
                //        }
                //    }
                //}
            }

            string oldStatus = ModuleTask.Status;
            string status = ddlStatus.SelectedValue;
            double oldPctComplete = ModuleTask.PercentageComplete;

            #region Save Task
            ModuleTask.Title = txtTitle.Text.Trim();
            ModuleTask.Description = txtDescription.Text.Trim();
            string taskTitle = ModuleTask.Title;

            ModuleTask.ItemOrder = Convert.ToInt32(txtItemOrder.Text == "" ? "0" : txtItemOrder.Text);

            ModuleTask.Status = ddlStatus.SelectedValue;
            double pctComplete = 0;
            double.TryParse(txtPctComplete.Text.Trim(), out pctComplete);
            ModuleTask.PercentageComplete = pctComplete;

            if (ddlStatus.SelectedValue.ToLower() == Constants.Completed.ToLower() || pctComplete >= 100)
            {
                status = Constants.Completed;
                pctComplete = 100;
                // ModuleTask = UGITModuleConstraint.MarkStageTaskAsComplete(ModuleTask, SPContext.Current.Web);
                ModuleTask = UGITModuleConstraint.MarkStageTaskAsComplete(ModuleTask, ApplicationContext);

            }
            // if (dtcDueDate.SelectedDate >= DateTime.Now)
            if (dtcDueDate.Date >= DateTime.Now)
                ModuleTask.StartDate = DateTime.Now;
            else
                ModuleTask.StartDate = dtcDueDate.Date;
            //ModuleTask.StartDate = dtcDueDate.Date.AddDays(-1);
            ModuleTask.DueDate = dtcDueDate.Date;//check SelectedDate
                                                 //foreach (var item in userMultiLookup)
                                                 //{
            ModuleTask.AssignedTo = peAssignedTo.GetValues();
            // }

            //ModuleTask.AssignedTo = userMultiLookup.ToString();

            double estimatedHours = 0;
            double.TryParse(txtEstimatedHours.Text.Trim(), out estimatedHours);
            ModuleTask.EstimatedHours = estimatedHours;

            if (txtActualHours.Visible)
            {
                double actualHours = 0;
                double.TryParse(txtActualHours.Text.Trim(), out actualHours);
                ModuleTask.ActualHours = actualHours;
            }

            ModuleTask.ModuleStep = ddlModuleStep.SelectedValue; //UGITModuleConstraint.GetModuleStepIdFromStage(ddlModuleStep.SelectedValue);

            ModuleTask.LatestComment = txtComment.Text.Trim();
            ModuleTask.Changes = true;

            // UGITModuleConstraint.SaveTask(SPContext.Current.Web, ref ModuleTask, DatabaseObjects.Lists.ModuleStageConstraints);
            UGITModuleConstraint.SaveTask(ApplicationContext, ref ModuleTask, DatabaseObjects.Tables.ModuleStageConstraints);

            string message = string.Empty;
            bool flag = UGITModuleConstraint.GetPendingConstraintsStatus(TicketPublicID, Convert.ToInt32(ModuleTask.ModuleStep), ref message, ApplicationContext);
            // Do not approve if AutoApproveOnStageTasks set false at stage level
            //Ticket ticketObj = new Ticket(SPContext.Current.Web, UGITUtility.getModuleIdByTicketID(this.TicketPublicID));
            var ticketObj = new Ticket(ApplicationContext, uHelper.getModuleIdByTicketID(ApplicationContext, this.TicketPublicID));

            LifeCycleStage stage = new LifeCycleStage();
            if (ticketObj != null)
            {
                LifeCycle lifeCycle = ticketObj.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                if (lifeCycle != null)
                    stage = lifeCycle.Stages.FirstOrDefault(x => x.StageStep == UGITUtility.StringToInt(this.ModuleStageId));//x.ID 
            }
            if (flag && stage != null && stage.AutoApproveOnStageTasks)
            {
                UGITModuleConstraint taskConstraint = new UGITModuleConstraint();
                taskConstraint.AutoApproveTicket(ModuleTask, _context);//Add Method in UgitModuleconstraint
            }
            #endregion

            #region Update History
            // Update project history to log change
            //if (projectItem != null)
            //    projectItem = SPListHelper.GetSPListItem(projectItem.ParentList, projectItem[DatabaseObjects.Columns.]);

            if (projectItem != null)
            {
                string historyDesc = string.Empty;
                if (oldPctComplete != pctComplete)
                {
                    historyDesc = string.Format("Task [{0}]:", taskTitle);
                    historyDesc += string.Format(" {0}% => {1}%", oldPctComplete, pctComplete);
                }
                if (oldStatus != status)
                {
                    if (historyDesc == string.Empty)
                        historyDesc += string.Format("Task [{0}]:", taskTitle);
                    else
                        historyDesc += ",";
                    historyDesc += string.Format(" {0} => {1}", oldStatus, status);
                }

                if (historyDesc != string.Empty)
                {
                    uHelper.CreateHistory(ApplicationContext.CurrentUser, historyDesc, projectItem, false, ApplicationContext);
                    // projectItem.UpdateOverwriteVersion(); check comment for given method
                }
            }
            #endregion

            #region Send Notifications
            //Notify assigned used about the task but only when use click on Save & notify Button
            if (btAction.ID == "btSaveAndNotify" && userMultiLookup != null && userMultiLookup.Count > 0)
            {
                // List<UserProfile> users = UserProfile.LoadUsersByIds(userMultiLookup.Select(x => x.LookupId.ToString()).ToList());
                string UserId = "";
                if (userMultiLookup.Count > 0)
                {
                    for (int i = 0; i < userMultiLookup.Count; i++) 
                    {
                        if (i == 0)
                        {
                            UserId = userMultiLookup[i].Id;
                        }
                        else
                        {
                            UserId += "," + userMultiLookup[i].Id;
                        }
                    }                    
                }
                var users = UserProfileManager.GetUserInfosById(UserId.ToString());

                List<string> emails = new List<string>();
                StringBuilder mailToNames = new StringBuilder();

                foreach (var userProfile in users)
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

                    if (!string.IsNullOrEmpty(ModuleTask.TicketPublicID))
                    {
                        // SPListItem spItem = Ticket.getCurrentTicket(ModuleTask.ModuleName, ModuleTask.TicketPublicID);
                        var item = ModuleViewManager.LoadByName(ModuleName);
                        if (item != null)
                            taskToEmail.Add("ProjectTitle", Convert.ToString(item.Title));
                    }

                    taskToEmail.Add("Title", ModuleTask.Title);
                    taskToEmail.Add("ProjectID", ModuleTask.TicketPublicID);
                    taskToEmail.Add("Description", ModuleTask.Description);
                    taskToEmail.Add("StartDate", ModuleTask.StartDate.ToString());
                    taskToEmail.Add("DueDate", ModuleTask.DueDate.ToString());
                    taskToEmail.Add("EstimatedHours", ModuleTask.EstimatedHours.ToString());
                    Ticket ticketRequest = new Ticket(_context, ModuleName);
                    var tabvalue = ticketRequest.Module.List_FormTab.Where(m => m.TabName == "Tasks").Select(m => m.TabId).FirstOrDefault();
                    string url = string.Format("{0}?TicketId={1}&ModuleName={2}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath), ModuleTask.TicketPublicID, ModuleTask.ModuleName);
                    string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, _context.TenantID, tabvalue.ToString());

                    StringBuilder taskEmail = new StringBuilder();
                    taskEmail.AppendFormat("Hi {0}<br /><br />", mailToNames.ToString());
                    //  taskEmail.AppendFormat("A new task <b>\"{0}\"</b> has been assigned to you by {1}.<br>", ModuleTask.Title, SPContext.Current.Web.CurrentUser.Name);
                    taskEmail.AppendFormat("A new task <b>\"{0}\"</b> has been assigned to you by {1}.<br>", ModuleTask.Title, ApplicationContext.CurrentUser.Name);

                    taskEmail.Append(emailFooter);

                    MailMessenger mailMessage = new MailMessenger(ApplicationContext);

                    if (ConfigurationVariableManager.GetValueAsBool(ConfigConstants.KeepExitCriteriaNotifications))
                        //mailMessage.SendMail(string.Join(",", emails.ToArray()), "New Task Assigned: " + ModuleTask.Title, "", taskEmail.ToString(), true,  new string[] { }, true, saveToTicketId: ModuleTask.TicketPublicID); // Pass ticket id to save email , 
                        mailMessage.SendMail(string.Join(",", emails.ToArray()), "New Task Assigned: " + ModuleTask.Title, "", taskEmail.ToString(), true, new string[] { }, true); // Pass ticket id to save email , 
                    else
                        mailMessage.SendMail(string.Join(",", emails.ToArray()), "New Task Assigned: " + ModuleTask.Title, "", taskEmail.ToString(), true, new string[] { }, true);
                }
            }
            #endregion
            string cookieValue = UGITUtility.GetCookieValue(Request, ModuleName + "-TicketSelectedTabConst");
            if (!string.IsNullOrEmpty(cookieValue))
            {
                UGITUtility.CreateCookie(Response, ModuleName + "-TicketSelectedTab", cookieValue);
                UGITUtility.DeleteCookie(Request, Response, ModuleName + "-TicketSelectedTabConst");
            }
            // TaskCache.ReloadProjectTasks(Constants.ExitCriteria, ModuleTask.TicketPublicID);
            //Added below condition for redirecting to particular location if request is from Task details of Summary page
            //if (IsRequestFromSummaryOrTask == true)
            //{
            //    ProjectSummary summary = new ProjectSummary();
            //    summary.IsRequestFromSummaryOrTask= true;
            //    Session["IsRequestFromSummaryOrTask"] = true;
            //    Response.Write("<script type='text/javascript'> window.parent.location.reload(\"" + Request["source"].Trim() + "\")</script>");
            //}
            //else
            //{
            ProjectSummary summary = new ProjectSummary();
            //summary.IsRequestFromSummaryOrTask = false;
            //Session["IsRequestFromSummaryOrTask"] = false;
            uHelper.ClosePopUpAndEndResponse(Context, true);
            //}
            //END
        }

        protected void RComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HistoryEntry hEntry = (HistoryEntry)e.Item.DataItem;

                Literal lCommentOwner = (Literal)e.Item.FindControl("lCommentOwner");
                Literal lCommentDate = (Literal)e.Item.FindControl("lCommentDate");
                Literal lCommentDesc = (Literal)e.Item.FindControl("lCommentDesc");

                lCommentOwner.Text = hEntry.createdBy;
                lCommentDate.Text = hEntry.created;
                lCommentDesc.Text = hEntry.entry;
            }
        }

        protected void BtDelete_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region My task actions

        protected void BtSaveMyTask_Click(object sender, EventArgs e)
        {
            if (ModuleTask == null)
                return;

            string status = ddlStatus.SelectedValue;
            int pctComplete = 0;
            string oldStatus = ModuleTask.Status;
            double oldPctComplete = ModuleTask.PercentageComplete;
            string taskTitle = ModuleTask.Title;

            string latestComment = txtComment.Text.Trim();
            int.TryParse(txtPctComplete.Text.Trim(), out pctComplete);
            Dictionary<string, string> taskToEmail = new Dictionary<string, string>();

            if (ddlStatus.SelectedValue.ToLower() == Constants.Completed.ToLower() || pctComplete >= 100)
            {
                status = Constants.Completed;
                pctComplete = 100;
                ModuleTask = UGITModuleConstraint.MarkStageTaskAsComplete(ModuleTask, ApplicationContext);
            }

            #region Save Task

            ModuleTask.Status = status;
            ModuleTask.PercentageComplete = pctComplete;
            ModuleTask.LatestComment = latestComment;

            double hours = 0;
            double.TryParse(txtActualHours.Text.Trim(), out hours);
            ModuleTask.ActualHours = hours;
            ModuleTask.ModuleStep = ddlModuleStep.SelectedValue;
            ModuleTask.Changes = true;
            string message = string.Empty;
            UGITModuleConstraint.SaveTask(ApplicationContext,ref ModuleTask, DatabaseObjects.Tables.ModuleStageConstraints);
            bool flag = UGITModuleConstraint.GetPendingConstraintsStatus(TicketPublicID, Convert.ToInt32(ModuleTask.ModuleStep), ref message, ApplicationContext);
            // Do not approve if AutoApproveOnStageTasks set false at stage level
            Ticket ticketObj = new Ticket(ApplicationContext, uHelper.getModuleIdByTicketID(ApplicationContext, this.TicketPublicID));
            LifeCycleStage stage = new LifeCycleStage();
            if (ticketObj != null)
            {
                stage = ticketObj.GetTicketCurrentStage(projectItem); //lifeCycle.Stages.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(this.ModuleStageId));
            }
            if (flag && stage != null && stage.AutoApproveOnStageTasks)
            {
                UGITModuleConstraint taskConstraint = new UGITModuleConstraint();
                taskConstraint.AutoApproveTicket(ModuleTask,_context);
            }
            #endregion

            // Update project history to log change
            //if (projectItem != null)
            //    projectItem = SPListHelper.GetSPListItem(projectItem.ParentList, projectItem.ID); check commented code

            if (projectItem != null)
            {
                #region Update History
                string historyDesc = string.Empty;
                if (oldPctComplete != pctComplete)
                {
                    historyDesc = string.Format("Task [{0}]:", taskTitle);
                    historyDesc += string.Format(" {0}% => {1}%", oldPctComplete, pctComplete);
                }
                if (oldStatus != status)
                {
                    if (historyDesc == string.Empty)
                        historyDesc += string.Format("Task [{0}]:", taskTitle);
                    else
                        historyDesc += ",";
                    historyDesc += string.Format(" {0} => {1}", oldStatus, status);
                }

                if (historyDesc != string.Empty)
                {
                    uHelper.CreateHistory(ApplicationContext.CurrentUser, historyDesc, projectItem, false,ApplicationContext);
                    // projectItem.UpdateOverwriteVersion();
                }
                #endregion

                #region Send Notifications

                taskToEmail.Add("ProjectID", ModuleTask.TicketPublicID);
                taskToEmail.Add("Title", ModuleTask.Title);
                taskToEmail.Add("Description", ModuleTask.Description);
                taskToEmail.Add("StartDate", ModuleTask.StartDate.ToString());
                taskToEmail.Add("DueDate", ModuleTask.DueDate.ToString());
                taskToEmail.Add("EstimatedHours", ModuleTask.EstimatedHours.ToString());
                taskToEmail.Add("ActualHours", ModuleTask.ActualHours.ToString());
                taskToEmail.Add("Status", ModuleTask.Status.ToString());
                taskToEmail.Add("% Complete", ModuleTask.PercentageComplete.ToString());

                var projectManagers = UGITUtility.GetSPItemValue(projectItem, DatabaseObjects.Columns.TicketProjectManager);
                //  var userIDs = projectManagers.Where(x => x.LookupId > 0).Select(x => x.LookupId.ToString()).ToList();
                var userIDs = "";
                //Checks current user is a project manager members list. if yes then don't send mail to him
                // if (!userIDs.Contains(SPContext.Current.Web.CurrentUser.ID.ToString()))
                if (!userIDs.Contains(ApplicationContext.CurrentUser.ToString()))
                {
                    // List<UserProfile> users = UserProfile.LoadUsersByIds(userIDs);
                    var users = UserProfileManager.GetUserInfosById("");//userIDs
                    List<string> emails = new List<string>();
                    foreach (UserProfile userProfile in users)
                    {
                        if (userProfile != null && !string.IsNullOrEmpty(userProfile.Email))
                        {
                            emails.Add(userProfile.Email);
                        }
                    }

                    string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, "", true, false, _context.TenantID);
                    StringBuilder taskEmail = new StringBuilder();
                    bool taskCompleted = (pctComplete == 100);
                    taskEmail.AppendFormat("Task \"{0}\" has been {1} by {2}", taskTitle, taskCompleted ? "completed" : "updated", ApplicationContext.CurrentUser.Name);
                    taskEmail.Append(emailFooter);
                    string subject = string.Format("Task {0}: {1}", taskCompleted ? "Completed" : "Updated", taskTitle);

                    MailMessenger mailMessage = new MailMessenger(ApplicationContext);
                    if (_configurationVariableHelper.GetValueAsBool(ConfigConstants.KeepExitCriteriaNotifications))
                        mailMessage.SendMail(string.Join(",", emails.ToArray()), subject, "", taskEmail.ToString(), true, new string[] { }, true); // Pass ticket id to save email
                    else
                        mailMessage.SendMail(string.Join(",", emails.ToArray()), subject, "", taskEmail.ToString(), true, new string[] { }, true);
                }
                #endregion
            }
            //TaskCache.ReloadProjectTasks(Constants.ExitCriteria, this.TicketPublicID);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        #endregion

        #region helpers

        private void FillFormData()
        {
            txtTitle.Text = ModuleTask.Title;
            lbTitle.Text = ModuleTask.Title;
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(ModuleTask.Status));
        }

        void BindModuleStep(string selectedModule)
        {
            try
            {
                ddlModuleStep.ClearSelection();
                ddlModuleStep.Items.Clear();
                // var moduleStages = ModuleViewManager.GetDataTable(DatabaseObjects.Tables.ModuleStages);
                // SPList spListModuleStep = SPListHelper.GetSPList(DatabaseObjects.Tables.ModuleStages);
                //List<LifeCycleStage> spModuleStepList = LifeCycleStageManager.Load(x => x.ModuleNameLookup == ModuleName).OrderBy(x => x.StageStep).ToList();
                DataTable dtModuleStep = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup}='{ModuleName}' and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");//spModuleStepList.Items.GetDataTable(); need to sort

                //var modulestepList = LifeCycleStageManager.Load();
                //if (modulestepList != null && modulestepList.Count > 0)
                //{

                //}
                //DataRow[] rows = dtModuleStep.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, selectedModule));

                if (dtModuleStep.Rows.Count > 0)
                {
                    //DataTable datatableModuleStep = rows.CopyToDataTable();
                    dtModuleStep.DefaultView.Sort = DatabaseObjects.Columns.ModuleStep + " ASC";
                    dtModuleStep = dtModuleStep.DefaultView.ToTable();

                    int moduleStep = UGITUtility.StringToInt(ModuleStepId);
                    if (moduleStep == 0 && !string.IsNullOrEmpty(this.ModuleStageId))
                    {
                        //Check ModuleStageId

                        DataRow[] steps = dtModuleStep.Select(string.Format("{0}={1}", DatabaseObjects.Columns.StageStep, UGITUtility.StringToInt(this.ModuleStageId)));
                        if (steps != null && steps.Length > 0)
                            moduleStep = UGITUtility.StringToInt(steps[0][DatabaseObjects.Columns.ModuleStep]);
                    }

                    int rowCounter = 0;
                    foreach (DataRow row in dtModuleStep.Rows)
                    {
                        // Skip 1st & last step
                        rowCounter++;
                        if (rowCounter == 1 || rowCounter == dtModuleStep.Rows.Count)
                            continue;

                        int thisStep = UGITUtility.StringToInt(row[DatabaseObjects.Columns.ModuleStep]);
                        if (thisStep >= moduleStep)
                            //Add stagetitlename
                            ddlModuleStep.Items.Add(new ListItem(row[DatabaseObjects.Columns.StageTitle].ToString(), row[DatabaseObjects.Columns.ModuleStep].ToString()));
                    }
                }

                ddlModuleStep.SelectedIndex = 0;                
            }
            catch (Exception)
            {
            }
        }
        #endregion        
    }
}