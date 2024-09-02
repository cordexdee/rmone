using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.DMS.Amazon;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;
using uGovernIT.Web.Controllers;
using uGovernIT.Web.ControlTemplates.Shared;

namespace uGovernIT.Web
{

    public partial class EditTask : UserControl
    {
        public string ModuleName { get; set; }
        public string UploadPageUrl { get; set; }
        public string documnetPickerUrl { get; set; }
        public string ticketPickerUrl { get; set; }
        public string selectedStage { get; set; }
        public string checkSprint = string.Empty;
        public string jsonSkillData = string.Empty;
        public string ResourceSelectFilter = string.Empty;
        public string attachfolderUrl;
        public string taskName = string.Empty;
        public string DocumentManagementUrl { get; set; }
        public string Urlstatic { get; set; }
        public string FolderName { get; set; }
        public string DocumentName { get; set; }
        public string Iframe { get; set; }
        public bool IsTabActive { get; set; }

        public bool RepeatableTask
        {
            get
            {
                if (Request["RepeatableTask"] != null && Request["RepeatableTask"] == "true")
                {
                    return true;
                }
                else
                    return false;
            }

        }

        public int childTaskCount;
        public int baselineTaskThreshold;
        public int taskID = 0;
        private int parentTaskID = 0;
        private int viewType = 0;


        private const string absoluteUrlView = "/Layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&TicketId={2}&docName={3}&ControlId={4}";
        private string newParam = "listpickerDocuments";
        private string formTitle = "Picker List";
        private string parentTitle = string.Empty;
        private string downLoadUrl = string.Empty;
        private string TenantID = string.Empty;
        private bool isRowCount;

        private bool isDuplicate
        {
            get
            {
                if (Request["actiontype"] != null && Request["actiontype"] == "duplicateTask")
                {

                    return true;
                }
                return false;

            }
        }
        private bool copyChild
        {
            get
            {
                if (Request["copychild"] != null && Request["copychild"] == "true")
                {
                    return true;
                }
                return false;
            }
        }
        private bool reminderEable;
        private bool keepActualHourMandatory = false;
        private bool isPreviousTask = false;

        private DocumentManagementController _documentManagement = null;
        private DMSManagerService _dmsManagerService = null;
        StringBuilder linkFile = new StringBuilder();


        //DataTable projectList = null;
        //DataTable taskList = null;
        //DataRow taskItem = null;
        DataRow project = null;

        protected string projectID = string.Empty;
        protected string taskType = string.Empty;
        protected string taskSubType = string.Empty;
        protected string ajaxHelperURL = string.Empty;
        protected string projectPublicID = string.Empty;
        protected string ajaxHelper = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/ajaxhelper.aspx");
        protected string addRMMAllocationUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/ProjectManagement.aspx?control=projectresourceupdate");
        protected string url = string.Empty;
        protected string updateReviewersUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=updatereviewer");
        protected string selectedFolder { get; set; }
        protected string editTaskFormUrl = string.Empty;
        protected bool linkActualFromRMMActual;
        protected bool autoCreateRMMProjectAllocation;
        protected DocumentManagementController DocumentManagementController
        {
            get
            {
                if (_documentManagement == null)
                {
                    _documentManagement = new DocumentManagementController();
                }
                return _documentManagement;
            }
        }

        protected DMSManagerService DMSManagerService
        {
            get
            {
                if (_dmsManagerService == null)
                {
                    _dmsManagerService = new DMSManagerService(HttpContext.Current.GetManagerContext());
                }
                return _dmsManagerService;
            }
        }

        List<UGITTask> ptasks;
        List<UGITAssignTo> assignToList = new List<UGITAssignTo>();

        Services service;
        UGITTask moduleInstDepny;
        TaskPredecessorsControl tpControl;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        LifeCycleManager lcHelper = null;
        LifeCycleStageManager lifeCycleStageManager = null;
        ConfigurationVariableManager ConfigVariableHelper = null;
        TicketManager ObjTicketManager = null;
        ModuleViewManager ObjModuleViewManager = null;
        ResourceAllocationManager allocationManager = null;
        //AccountController accountControllerManager = new AccountController();


        DateTime pStartDate;
        DateTime pEndDate;

        UserProfile User;
        UserProfileManager UserManager;
        UGITTaskManager TaskManager;
        LifeCycleManager lifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
        LookupValueBoxEdit dropDownBox;
        protected Ticket ticketRequest;
        UGITModule ugitModule;
        private bool allowHold;
        bool isHold = false;
        protected LifeCycleStage ticketCurrentStage = null;

        #region Gobal Events

        protected override void OnInit(EventArgs e)
        {
            ConfigVariableHelper = new ConfigurationVariableManager(context);
            ObjTicketManager = new TicketManager(context);
            ObjModuleViewManager = new ModuleViewManager(context);
            allocationManager = new ResourceAllocationManager(context);
            lcHelper = new LifeCycleManager(context);
            lifeCycleStageManager = new LifeCycleStageManager(context);

            TenantID = context.TenantID; //Convert.ToString(Session["TenantID"]);
            dropDownBox = new LookupValueBoxEdit();
            dropDownBox.ID = "ddRequestType";
            dropDownBox.CssClass = "rmmLookup-valueBoxEdit";
            dropDownBox.isRequestType = true;
            dropDownBox.FieldName = DatabaseObjects.Columns.TicketRequestTypeLookup;
            dropDownBox.gridView.SettingsBehavior.AllowSelectByRowClick = true;
            dropDownBox.gridView.ClientSideEvents.SelectionChanged = "requestTypeSelectionChanged";
            dropDownBox.gridView.Width = Unit.Percentage(100);
            dropDownBox.gridView.EnableCallBacks = true;

            User = Context.CurrentUser(); // HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            TaskManager = new UGITTaskManager(context);
            //right now there is only two task account and task so need to bind it.
            //BindSubTaskType();

            reminderEable = ConfigVariableHelper.GetValueAsBool(ConfigConstants.TaskReminderDefaultEnable);
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?control=edittask");
            attachfolderUrl = UGITUtility.GetAbsoluteURL(string.Format("Layouts/uGovernIT/delegatecontrol.aspx?control=DocumentControl&isreadonly=False"));
            //Basic variables required for edit or creating item
            linkActualFromRMMActual = ConfigVariableHelper.GetValueAsBool(ConfigConstants.LinkActualFromRMMActual);
            autoCreateRMMProjectAllocation = ConfigVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
            ajaxHelperURL = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/ajaxhelper.aspx");

            if (string.IsNullOrEmpty(ModuleName))
                ModuleName = string.IsNullOrEmpty(Request["ModuleName"]) ? Request["Module"] : Request["ModuleName"];

            taskType = Request["taskType"];
            projectID = projectPublicID = string.IsNullOrEmpty(Request["projectID"]) ? Request["ticketId"] : Request["projectID"];
            int.TryParse(Request["taskID"], out taskID);
            int.TryParse(Request["parentTaskID"], out parentTaskID);
            int.TryParse(Request["viewType"], out viewType);
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);

            if (!string.IsNullOrEmpty(ModuleName))
                ugitModule = ObjModuleViewManager.LoadByName(ModuleName);

            LifeCycle projectLifeCycle = null;
            keepActualHourMandatory = ConfigVariableHelper.GetValueAsBool(string.Format("{0}Task_MakeActualHoursMandatory", ModuleName));

            tpControl = (TaskPredecessorsControl)Page.LoadControl(@"~/ControlTemplates/uGovernIT/Task/TaskPredecessorsControl.ascx");
            tpControl.ModuleName = ModuleName;
            tpControl.TaskID = taskID;
            tpControl.ProjectID = projectPublicID;

            aspxdtOnHoldDate.MinDate = DateTime.Now.Date.AddDays(1);
            aspxdtOnHoldDate.Date = DateTime.Now.AddDays(1);
            aspxdtOnHoldDate.EditFormat = EditFormat.DateTime;
            aspxdtOnHoldDate.TimeSectionProperties.Visible = true;

            ticketRequest = new Ticket(context, ModuleName);
            BindDDLCount();

            // Test this
            //taskList = UGITUtility.ToDataTable<UGITTask>(TaskManager.LoadByProjectID(ModuleName, projectPublicID));

            //Default settings for all type tasks
            #region TSK Default Setting
            if (ModuleName == "TSK")
            {
                if (viewType != 1)
                    btSaveAndNotify.Visible = true;

                if (!string.IsNullOrEmpty(projectID))
                    project = Ticket.GetCurrentTicket(context, ModuleName, projectID);

                parentTitle = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));

                trAssignedToNew.Visible = false;
                trTitle.Visible = true;
                trStatus.Visible = true;
                trAssignedTo.Visible = true;
                trPredecessors.Visible = true;
                editPredecessor.Visible = true;

                //trPctComplete.Visible = true;
                tdPctCompletelbl.Visible = true;
                tdPctComplete.Visible = true;

                trNote.Visible = true;
                //  trTaskType.Visible = true;
                trComment.Visible = true;
                trTaskBehaviour.Visible = true;
                trAttachment.Visible = true;
                trCritical.Visible = true;
                trTaskReminderDays.Visible = true;

                tdlblStartDate.Visible = true;
                tdStartDate.Visible = true;
                tdlblDueDate.Visible = true;
                tdDueDate.Visible = true;
                tdlblActualHours.Visible = true;
                tdActualHours.Visible = true;
                tdlblEstimatedHours.Visible = true;
                tdEstimatedHours.Visible = true;
                trHours.Visible = true;
                // tdDates.ColSpan = 0;

                trSkill.Visible = true;
            }
            #endregion

            #region Module Task Default Setting
            if (TaskManager.IsModuleTasks(ModuleName))
            {
                if (viewType != 1)
                    btSaveAndNotify.Visible = true;

                if (!string.IsNullOrEmpty(projectID))
                    project = Ticket.GetCurrentTicket(context, ModuleName, projectID);

                parentTitle = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));

                trAssignedToNew.Visible = false;
                trTitle.Visible = true;
                trStatus.Visible = true;
                trAssignedTo.Visible = true;
                trPredecessors.Visible = true;
                editPredecessor.Visible = true;
                //trPctComplete.Visible = true;
                tdPctCompletelbl.Visible = true;
                tdPctComplete.Visible = true;

                trNote.Visible = true;
                //  trTaskType.Visible = true;
                trComment.Visible = true;
                trTaskBehaviour.Visible = true;
                trAttachment.Visible = true;
                trCritical.Visible = true;
                trTaskReminderDays.Visible = true;

                tdlblStartDate.Visible = true;
                tdStartDate.Visible = true;
                tdlblDueDate.Visible = true;
                tdDueDate.Visible = true;
                tdlblActualHours.Visible = true;
                tdActualHours.Visible = true;
                tdlblEstimatedHours.Visible = true;
                tdEstimatedHours.Visible = true;
                trHours.Visible = true;
                //tdDates.ColSpan = 0;

                trSkill.Visible = true;
            }
            #endregion

            #region PMM Default Setting
            else if (ModuleName == "PMM")
            {
                if (viewType != 1)
                    btSaveAndNotify.Visible = true;

                int.TryParse(ConfigVariableHelper.GetValue("CreateBaselineTaskThreshold"), out baselineTaskThreshold);

                if (!string.IsNullOrEmpty(projectID))
                    project = Ticket.GetCurrentTicket(context, ModuleName, projectID);

                //   Ticket ticketRequest = new Ticket(SPContext.Current.Web, ModuleName);
                if (project[DatabaseObjects.Columns.ProjectLifeCycleLookup] != DBNull.Value)
                    projectLifeCycle = lcHelper.LoadByID(Convert.ToInt64(project[DatabaseObjects.Columns.ProjectLifeCycleLookup]));

                parentTitle = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));

                trAssignedToNew.Visible = false;
                trTitle.Visible = true;
                trStatus.Visible = true;
                trAssignedTo.Visible = true;
                trPredecessors.Visible = true;
                editPredecessor.Visible = true;
                // trPctComplete.Visible = true;
                tdPctCompletelbl.Visible = true;
                tdPctComplete.Visible = true;

                trNote.Visible = true;
                // trTaskType.Visible = true;
                trComment.Visible = true;
                trTaskBehaviour.Visible = true;
                trAttachment.Visible = true;
                trSprints.Visible = true;
                trCritical.Visible = true;
                // showonprojectcalendar
                trShowonProjectCalendar.Visible = true;
                trTaskReminderDays.Visible = true;

                tdlblStartDate.Visible = true;
                tdStartDate.Visible = true;
                tdlblDueDate.Visible = true;
                tdDueDate.Visible = true;
                tdlblActualHours.Visible = true;
                tdActualHours.Visible = true;
                tdlblEstimatedHours.Visible = true;
                tdEstimatedHours.Visible = true;
                trHours.Visible = true;
                // tdDates.ColSpan = 0;

                // hide milestone option for old projects
                if (projectLifeCycle != null && projectLifeCycle.ID != 0)
                {
                    trMilestone.Visible = true;
                    ddlProjectStages_Init(ddlProjectStages, new EventArgs());
                }

                if (isDuplicate && parentTaskID > 0 && projectLifeCycle != null && projectLifeCycle.ID != 0)
                {
                    trMilestone.Visible = true;
                    ddlProjectStages_Init(ddlProjectStages, new EventArgs());
                }

                ddlSprints_Init(ddlSprints, new EventArgs());
                trSkill.Visible = true;
            }
            #endregion

            #region NPR Default Setting
            else if (ModuleName == "NPR")
            {
                if (!string.IsNullOrEmpty(projectID))
                    project = Ticket.GetCurrentTicket(context, ModuleName, projectID);

                parentTitle = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));

                trTaskBehaviour.Visible = true;
                trTitle.Visible = true;
                trPredecessors.Visible = true;
                editPredecessor.Visible = true;
                trNote.Visible = true;
                //  trTaskType.Visible = true;
                trAssignedTo.Visible = true;
                fsMiscellaneous.Visible = true;

                tdlblStartDate.Visible = true;
                tdStartDate.Visible = true;
                tdlblDueDate.Visible = true;
                tdDueDate.Visible = true;

                //tdDates.ColSpan = 0;

                trSkill.Visible = true;
                trHours.Visible = true;
                tdEstimatedHours.Visible = true;
                tdlblEstimatedHours.Visible = true;
            }
            #endregion

            #region SVC Config Default Setting
            else if (ModuleName == "SVCConfig")
            {
                if (taskID == 0)
                    aspxMoreActions.ClientVisible = false;
                ServicesManager serviceManager = new ServicesManager(context);
                //service = serviceManager.LoadByServiceID(Convert.ToInt32(projectID), false, false, true); commented

                //Added by mudassir 28 feb 2020
                service = serviceManager.LoadByServiceID(Convert.ToInt32(projectID), true, true, true);
                //
                BindModuleStep("SVC");
                parentTitle = service.Title;
                trTitle.Visible = true;
                trPredecessors.Visible = true;
                editPredecessor.Visible = true;
                trWeight.Visible = true;
                tdDates.Visible = false;
                fsActualHours.Visible = false;

                if (taskType.ToLower() == ServiceSubTaskType.Task.ToLower() || taskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower() || taskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                {
                    //trModuleStep.Visible = true; Commented by mudassir 2 march 2020
                    trType.Visible = true;
                    trNote.Visible = true;
                    //trTaskType.Visible = true;
                    trAssignedToNew.Visible = true;
                    trAssignedTo.Visible = false;
                    tdlblEstimatedHours.Visible = true;
                    tdEstimatedHours.Visible = true;
                    imgEstimatedHrs.Visible = false;
                    trHours.Visible = true;
                    trApprovalRequired.Visible = true;
                    //editDescription.Visible = false;
                    editPredecessor.Visible = true;
                    txtDescription.Visible = true;
                    lbDescription.Visible = false;
                    tdApplicationAccess.Style.Add("Display", "none");
                    tdAccountTask.Style.Add("Display", "none");
                    trDisableSLA.Visible = true;

                    // test below commented code lines
                    trDisableNotification.Visible = true;

                    if (taskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                    {
                        tdAccountTask.Style.Remove("Display");
                        chkAutoUserCreation.Visible = true;
                        chkAutoFillRequestor.Visible = true;
                        ddlTypes.SelectedIndex = 1;
                    }
                    else if (taskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
                    {
                        tdApplicationAccess.Style.Remove("Display");
                        FillApplicationType(ddlApplicationAccess);
                        ddlApplicationAccess_SelectedIndexChanged(ddlApplicationAccess, null);
                        trApplicationAccess.Visible = true;
                        GridLookupApplicationQuestion.GridView.Style.Add("width", "168px");
                        ddlTypes.SelectedIndex = 2;
                    }
                    fsMiscellaneous.Visible = false;
                }
                else
                {
                    // Test below 3 lines
                    editPredecessor.Visible = true;
                    //trModuleStep.Visible = true;
                    //fsScheduling.Visible = true;
                    if (service.CreateParentServiceRequest)
                        trDisableSLA.Visible = true;

                    trSvcConfigSelectModules.Visible = true;
                }

                FillModuleDropDown(ddlModuleDetail);
                tpControl.ServiceTasks = service.Tasks;
                tpControl.PredecessorMode = PredecessorType.ServiceTask;
                pheditcontrol.Controls.Add(tpControl);

                trSkill.Visible = false;
                dueDateAutoCal.Visible = imgDueDateAutoCalculater.Visible = false;
                peAssignedTo.SelectionSet = "User,SPGroup";
            }
            #endregion

            #region SVC Request Setting
            else if (ModuleName == "SVC")
            {
                ServiceTaskManager serviceTaskManager = new ServiceTaskManager(context);
                //BindModuleStep("SVC");

                if (!string.IsNullOrEmpty(projectID))
                    project = Ticket.GetCurrentTicket(context, ModuleName, projectID);

                projectLifeCycle = ticketRequest.GetTicketLifeCycle(project);
                ticketCurrentStage = ticketRequest.GetTicketCurrentStage(project, projectLifeCycle);

                ServicesManager serviceManager = new ServicesManager(context);
                service = serviceManager.LoadByServiceID(UGITUtility.StringToLong(project["ServiceLookUp"]), true, true, true);

                parentTitle = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));
                tpControl.moduleDepncies = serviceTaskManager.LoadByServiceID(Convert.ToString(projectPublicID));

                if (UserManager.IsTicketAdmin(User) || UserManager.IsUGITSuperAdmin(User))
                    lnkDelete.Visible = true;

                //tdlblDueDate.Attributes.Add("style", "padding-left:0");
                trPredecessors.Visible = true;
                editPredecessor.Visible = true;
                trWeight.Visible = true;

                if (taskType.ToLower() == "task")
                {
                    if (viewType != 1)
                        btSaveAndNotify.Visible = true;
                    else
                        btSaveAndNotifyMyTask.Visible = true;

                    trDisableSLA.Visible = true;
                    trNote.Visible = true;
                    trStatus.Visible = true;
                    trTitle.Visible = true;

                    tdPctCompletelbl.Visible = true;
                    tdPctComplete.Visible = true;

                    trAssignedTo.Visible = false;
                    trAssignedToNew.Visible = true;
                    trComment.Visible = true;

                    txtEstimatedHours.Visible = true;
                    lbEstimatedHours.Visible = false;
                    txtActualHours.Visible = true;
                    lbActualHours.Visible = false;

                    tdlblDueDate.Visible = true;
                    tdDueDate.Visible = true;
                    tdlblActualHours.Visible = true;
                    tdActualHours.Visible = true;
                    tdlblEstimatedHours.Visible = true;
                    tdEstimatedHours.Visible = true;
                    imgEstimatedHrs.Visible = false;
                    trHours.Visible = true;
                    trAttachment.Visible = true;
                }
                else
                {
                    trSvcSelectModules.Visible = false;
                }

                if (trAssignedTo.Visible == false && trAssignedToNew.Visible == false && tdlblStartDate.Visible == false
                    && trProposedDueDate.Visible == false && trHours.Visible == false)
                {
                    fsScheduling.Visible = false;
                }
                else
                {
                    fsScheduling.Visible = true;
                }

                FillModuleDropDown(ddlModuleDetail);

                tpControl.ProjectID = projectPublicID;
                tpControl.PredecessorMode = PredecessorType.ModuleDependency;
                pheditcontrol.Controls.Add(tpControl);

                trSkill.Visible = false;
                dueDateAutoCal.Visible = imgDueDateAutoCalculater.Visible = false;
                peAssignedTo.SelectionSet = "User,SPGroup";
            }
            #endregion

            if (viewType == 1)
            {
                txtTitle.Visible = false;
                dtcStartDate.Visible = false;
                dtcDueDate.Visible = false;
                txtEstimatedHours.Visible = false;
                txtDescription.Visible = false;
                txtWeight.Visible = false;

                trComment.Visible = true;
                btSaveTask.Visible = false;
                btSaveMyTask.Visible = true;
                lnkDelete.Visible = false;
                lnkCancel.Visible = false;
                lbTitle.Visible = true;
                lbStartDate.Visible = true;
                lbDueDate.Visible = true;
                lbEstimatedHours.Visible = true;
                lbDescription.Visible = true;
                lbWeight.Visible = true;
                trSprints.Visible = false;
                trTaskBehaviour.Visible = false;
                trAttachment.Visible = true;
                trApprovalRequired.Visible = false;
                trParent.Visible = true;

                //Hide waiting status from my task edit view
                if (ModuleName == "SVC")
                {
                    ListItem statusOption = ddlStatus.Items.FindByText("Waiting");
                    if (statusOption != null)
                    {
                        statusOption.Enabled = false;
                    }
                }
            }

            //Load specified task. it works in edit mode
            if (taskID > 0 || (isDuplicate && taskID == 0 && parentTaskID > 0))
            {
                pAuditInformataion.Visible = true;

                #region Task
                if (ModuleName == "PMM" || ModuleName == "NPR" || ModuleName == "TSK" || TaskManager.IsModuleTasks(ModuleName))
                {
                    if (ModuleName == "PMM" || ModuleName == "TSK" || TaskManager.IsModuleTasks(ModuleName))
                        tdERH.Visible = tdlblERH.Visible = true;

                    //show delete button in edit task.
                    lnkDelete.Visible = true;

                    ptasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                    UGITTask pTask;

                    if (taskID > 0)
                    {
                        pTask = ptasks.FirstOrDefault(x => x.ID == taskID);
                        fsActualHours.Visible = ShowActualHours(ModuleName);
                    }
                    else
                    {
                        pTask = ptasks.FirstOrDefault(x => x.ID == parentTaskID);
                    }

                    if (pTask != null)
                    {
                        if (pTask.ChildCount > 0)
                        {
                            trAssignedTo.Visible = false;
                            dtcStartDate.Enabled = false;
                            dtcDueDate.Enabled = false;
                            txtEstimatedHours.Enabled = false;
                            txtEstimatedRemainingHours.Enabled = false;
                            txtActualHours.Enabled = false;
                            imgAssignee.Visible = false;
                            dueDateAutoCal.Visible = imgDueDateAutoCalculater.Visible = false;
                            imgRemainingHRS.Visible = false;
                            imgEstimatedHrs.Visible = false;
                            cbSkill.Enabled = false;
                            btnSkillAutoCalculate.Enabled = false;                            
                        }
                        else
                        {
                            trAssignedTo.Visible = true;
                            dtcStartDate.Enabled = true;
                            dtcDueDate.Enabled = true;
                            txtEstimatedHours.Enabled = true;
                            txtEstimatedRemainingHours.Enabled = true;
                            txtActualHours.Enabled = true;
                            imgAssignee.Visible = true;
                            dueDateAutoCal.Visible = imgDueDateAutoCalculater.Visible = true;
                            tdDueDate.Attributes.Add("style", "width:95%");
                            imgRemainingHRS.Visible = true;
                            imgEstimatedHrs.Visible = true;
                            cbSkill.Enabled = true;
                            btnSkillAutoCalculate.Enabled = true;                            
                        }
                        if (imgEstimatedHrs.Visible)
                        {
                            txtEstimatedHourswrap.Attributes.Add("style", "width:95%; display:inline-block;");
                        }
                        else
                        {
                            txtEstimatedHourswrap.Attributes.Add("style", "width:100%");
                        }

                        //Set related ticket link functionality
                        if (pTask.Behaviour == "Ticket")
                        {
                            trTicketReadOnly.Style.Add("display", "");
                            trTitle.Style.Add("display", "none");
                            trTicket.Style.Add("display", "none");

                            if (!string.IsNullOrEmpty(pTask.RelatedTicketID))
                            {
                                lblTicket.Text = lblTicketReadOnly.Text = pTask.Title;
                                string url = Ticket.GenerateTicketURL(context, pTask.RelatedTicketID);
                                lblTicket.Attributes.Add("TicketUrl", url);
                                lblTicketReadOnly.Attributes.Add("TicketUrl", url);
                                hdnTicketId.Value = pTask.Title;
                            }
                        }
                        else
                        {
                            trTitle.Style.Add("display", "");
                            trTicket.Style.Add("display", "none");
                            trTicketReadOnly.Style.Add("display", "none");
                        }

                        //set assigntopct into hidden field to get on page for show confirmation popup.
                        hdnStrAssignToPct.Value = pTask.AssignToPct;

                        if (ModuleName == "PMM" || ModuleName == "TSK" || TaskManager.IsModuleTasks(ModuleName))
                        {
                            string approveProposedDateUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=taskpropsedapprval");

                            btApprove.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}', 'moduleName={1}&taskid={2}&ticketid={4}&action=1', '{1} Task: {3}', 70, 50, 0, '{5}');return false;", approveProposedDateUrl, ModuleName, taskID, UGITUtility.TruncateWithEllipsis(pTask.Title, 70, "."), projectPublicID, Uri.EscapeUriString(Request.Url.AbsolutePath)));
                            btReject.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}', 'moduleName={1}&taskid={2}&ticketid={4}&action=2', '{1} Task: {3}', 70, 50, 0, '{5}');return false;", approveProposedDateUrl, ModuleName, taskID, UGITUtility.TruncateWithEllipsis(pTask.Title, 70, "."), projectPublicID, Uri.EscapeUriString(Request.Url.AbsolutePath)));

                            string proposedDateUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=taskproposeadate");
                            btProposeNewDate.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}', 'moduleName={1}&taskid={2}&ticketid={4}', '{1} Task: {3}', 70, 50, 0, '{5}');return false;", proposedDateUrl, ModuleName, taskID, UGITUtility.TruncateWithEllipsis(pTask.Title, 70, "."), projectPublicID, Uri.EscapeUriString(Request.Url.AbsolutePath)));

                            if (viewType == 1)
                            {
                                btApprove.Visible = false;
                                btReject.Visible = false;
                                btProposeNewDate.Visible = false;

                                if (pTask.ProposedStatus == UGITTaskProposalStatus.Pending_Date)
                                {
                                    trProposedDueDate.Visible = true;
                                    lbProposedDate.Visible = true;

                                    if (pTask.ProposedDate != null)
                                        lbProposedDate.Text = string.Format("{0:MMM-dd-yyyy} <b style='color:red;'>(Approval Pending)</b>", pTask.ProposedDate);
                                    btProposeNewDate.Visible = false;
                                }
                                else if (pTask.ProposedStatus == UGITTaskProposalStatus.Pending_AssignTo)
                                {
                                    trAssignedTo.Visible = false;
                                    peAssignedTo.Visible = false;
                                    trAssignedToNew.Visible = true;
                                    lbAssignedTo.Visible = true;

                                    // test this label value with multiple assigned to if allowed
                                    List<string> peopleDisplayName = new List<string>();
                                    List<UserProfile> lstAssignedToUsers = UserManager.GetUserInfosById(pTask.AssignedTo);

                                    if (lstAssignedToUsers != null && lstAssignedToUsers.Count > 0)
                                    {
                                        foreach (UserProfile userV in lstAssignedToUsers)
                                        {
                                            if (userV != null)
                                                peopleDisplayName.Add(userV.UserName);
                                        }
                                    }

                                    lbAssignedTo.Text = string.Join("; ", peopleDisplayName.ToArray());

                                    lbMessageForPendingApproval.Text = "Waiting for manager approval.";

                                    lbStatus.Visible = true;
                                    ddlStatus.Visible = false;
                                    lbDueDate.Visible = true;
                                    dtcDueDate.Visible = false;
                                    lbPctComplete.Visible = true;
                                    txtPctComplete.Visible = false;

                                    btSaveMyTask.Visible = false;
                                    btSaveAndNotify.Visible = false;
                                    btSaveAndNotifyMyTask.Visible = false;
                                }
                                else
                                {
                                    trProposedDueDate.Visible = false;
                                    btProposeNewDate.Visible = true;
                                }
                            }
                            else if (pTask.ProposedStatus == UGITTaskProposalStatus.Pending_Date)
                            {
                                if (pTask.ProposedDate != null)
                                    lbProposedDate.Text = string.Format("{0:MMM-dd-yyyy}", pTask.ProposedDate);

                                trProposedDueDate.Visible = true;
                                lbProposedDate.Visible = true;
                                btApprove.Visible = true;
                                btReject.Visible = true;
                            }
                            else if (pTask.ProposedStatus == UGITTaskProposalStatus.Pending_AssignTo)
                            {
                                trAssignedTo.Visible = false;
                                peAssignedTo.Visible = false;
                                trAssignedToNew.Visible = true;
                                lbAssignedTo.Visible = true;
                                lbStatus.Visible = true;
                                ddlStatus.Visible = false;
                                lbDueDate.Visible = true;
                                dtcDueDate.Visible = false;
                                lbPctComplete.Visible = true;
                                txtPctComplete.Visible = false;

                                // set AssignedTo users on label
                                List<string> peopleDisplayName = new List<string>();
                                List<UserProfile> lstAssignedToUsers = UserManager.GetUserInfosById(pTask.AssignedTo);

                                if (lstAssignedToUsers != null && lstAssignedToUsers.Count > 0)
                                {
                                    foreach (UserProfile userV in lstAssignedToUsers)
                                    {
                                        if (userV != null)
                                            peopleDisplayName.Add(userV.UserName);
                                    }
                                }

                                lbAssignedTo.Text = string.Join("; ", peopleDisplayName.ToArray());

                                //fetch project managers from project
                                List<UserProfile> lstProjectManagers = UserManager.GetUserInfosById(UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketProjectManager]));
                                bool userIsManager = lstProjectManagers.Exists(x => x.Id == context.CurrentUser.Id);

                                //SPListItem taskListItem = null;
                                //if (pTask.ModuleName == "PMM")
                                //{
                                //    taskListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMTasks, pTask.ID);
                                //}
                                //else if (pTask.ModuleName == "TSK")
                                //{
                                //    taskListItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.TSKTasks, pTask.ID);
                                //}

                                //SPFieldUserValue author = new SPFieldUserValue(_spWeb, Convert.ToString(taskListItem[DatabaseObjects.Columns.Author]));
                                //string userName = author.LookupValue;
                                //lbMessageForPendingApproval.Text = string.Format("Tentative assignment by {0}", userName);
                                lbMessageForPendingApproval.Text = "";

                                if (userIsManager)
                                {
                                    assignToContainer.Visible = btnApprove.Visible = true;
                                    btAssignToClear.Visible = true;
                                }
                            }

                            if (ModuleName == "PMM" && !IsPostBack)
                            {
                                pAttachmentContainer.Visible = false;
                                pAddDocuments.Style.Add("display", "block");
                                if (pTask.StageStep > 0 && pTask.StageStep <= ddlProjectStages.Items.Count)
                                {
                                    selectedStage = ddlProjectStages.Items[ddlProjectStages.Items.IndexOf(ddlProjectStages.Items.FindByValue(pTask.StageStep.ToString()))].Text;
                                }
                                else if (pTask.ParentTaskID > 0)
                                {
                                    UGITTask task = ptasks.FirstOrDefault(c => c.ID == pTask.ParentTaskID);
                                    if (task != null && task.StageStep > 0)
                                    {
                                        selectedStage = ddlProjectStages.Items[ddlProjectStages.Items.IndexOf(ddlProjectStages.Items.FindByValue(task.StageStep.ToString()))].Text;
                                    }
                                }

                                if (!string.IsNullOrEmpty(selectedStage))
                                {
                                    string[] arr = selectedStage.Split('-');

                                    if (arr != null && arr.Length == 2)
                                        selectedStage = arr[0].Trim() + "-" + arr[1].Trim();
                                }

                                CheckIfPortalExists(selectedStage);

                                ddlSprints_Init(ddlSprints, new EventArgs());

                                if (pTask.SprintLookup > 0)
                                {
                                    cbSprints.Checked = true;
                                    //sprintsDiv.Style.Add("display", "");
                                    ddlSprints.SelectedIndex = ddlSprints.Items.IndexOf(ddlSprints.Items.FindByValue(Convert.ToString(pTask.SprintLookup)));
                                }
                            }
                        }

                        // hide milestone option for old projects
                        trMilestone.Visible = false;
                        if (ModuleName == "PMM" && projectLifeCycle != null && projectLifeCycle.ID != 0 && pTask.ParentTaskID == 0 && viewType != 1)
                        {
                            trMilestone.Visible = true;
                            ddlProjectStages_Init(ddlProjectStages, new EventArgs());

                            if (pTask.IsMileStone)
                            {
                                cbMilestone.Checked = true;
                                milestoneStageDiv.Style.Add(HtmlTextWriterStyle.Display, "block");
                                ddlProjectStages.SelectedIndex = ddlProjectStages.Items.IndexOf(ddlProjectStages.Items.FindByValue(pTask.StageStep.ToString()));
                            }
                        }

                        bool isParent = false;
                        if (pTask.ChildCount > 0)
                        {
                            isParent = true;
                            trSprints.Visible = false;
                        }

                        txtTitle.Text = pTask.Title;
                        lbTitle.Text = pTask.Title;
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(pTask.Status));
                        txtPctComplete.Text = pTask.PercentComplete.ToString();
                        lbPctComplete.Text = pTask.PercentComplete.ToString();
                        chkCritical.Checked = pTask.IsCritical;

                        if (!string.IsNullOrEmpty(pTask.Attachments))
                        {
                            if (UploadAndLinkDocuments.Visible == true)
                            {
                                //get fileName from DMSDocument
                                List<DMSDocument> attachedFileList = DMSManagerService.GetFileListByFileId(pTask.Attachments);

                                foreach (var file in attachedFileList)
                                {
                                    linkFile = linkFile.Append($"<a id='file_{file.FileId}' style='cursor: pointer;' onclick='window.downloadSelectedFile({file.FileId})'>{file.FileName}</a><img src='/content/images/close-red.png' id='img_{file.FileId}' class='cancelUploadedFiles' onclick='window.deleteLinkedDocument(\"" + file.FileId + "\")'></img><br/>");
                                }

                                bindMultipleLink.InnerHtml = Convert.ToString(linkFile);
                            }
                            else if (taskFileUploadControl.Visible == true)
                            {
                                taskFileUploadControl.SetValue(pTask.Attachments);
                            }
                        }

                        // test below code line
                        //ugitupAttachments.SetValue(pTask.Attachments);

                        chkShowonProjectCalendar.Checked = pTask.ShowOnProjectCalendar;
                        childTaskCount = pTask.ChildCount;

                        GetPredecessors(ptasks, pTask);
                        tpControl.ProjectID = pTask.TicketId;     // UGITTaskHelper.IsModuleTasks(ModuleName) ? pTask.TicketId : pTask.ProjectLookup.LookupValue;
                        tpControl.PredecessorMode = PredecessorType.Task;

                        if (pTask.Predecessors != null && UGITUtility.SplitString(pTask.Predecessors, Constants.Separator6).Count() > 0)
                            tpControl.SelectedPredecessorsId = new List<string>(UGITUtility.SplitString(pTask.Predecessors, Constants.Separator6));   //.Select(x => x.LookupId).ToList();
                        else
                            tpControl.SelectedPredecessorsId = new List<string>();

                        if (pTask.PredecessorTasks != null && pTask.PredecessorTasks.Count > 0)
                            tpControl.SelectedPredecessorsText = string.Join(";", pTask.PredecessorTasks.Select(x => x.ItemOrder + " - " + x.Title).ToArray()) + ";";

                        pheditcontrol.Controls.Add(tpControl);

                        //new lines for modification of skill code..
                        List<UserSkill> listofuserskill = new List<UserSkill>();
                        if (string.IsNullOrEmpty(Request[hdnEditSkill.UniqueID]))
                        {
                            if (pTask.UserSkillMultiLookup != null)
                            {
                                //foreach (SPFieldLookupValue lVal in pTask.Skills)
                                //{
                                //    listofuserskill.Add(new UserSkill() { id = lVal.LookupId, name = lVal.LookupValue });
                                //}
                                //listofuserskill.Add(new UserSkill() { id = pTask.UserSkillMultiLookup, name = pTask.UserSkillMultiLookup });
                                jsonSkillData = Newtonsoft.Json.JsonConvert.SerializeObject(listofuserskill);
                                cbSkill.SetValues(pTask.UserSkillMultiLookup);
                            }
                            else
                            {
                                jsonSkillData = "[]";
                            }
                        }
                        else
                        {
                            // test this jsonSkillData
                            jsonSkillData = Request[hdnEditSkill.UniqueID];
                            cbSkill.SetValues(Request[hdnEditSkill.UniqueID]);
                        }

                        dtcStartDate.Date = pTask.StartDate;
                        hdnStartDate.Value = pTask.StartDate.ToString("MM/dd/yyyy");
                        lbStartDate.Text = pTask.StartDate.ToString("MMM-dd-yyyy");
                        dtcDueDate.Date = pTask.DueDate;
                        lbDueDate.Text = pTask.DueDate.ToString("MMM-dd-yyyy");
                        hdnDueDate.Value = pTask.DueDate.ToString("MM/dd/yyyy");
                        txtDescription.Text = Server.HtmlDecode(pTask.Description);
                        lbDescription.Text = UGITUtility.FindAndConvertToAnchorTag(Server.HtmlDecode(pTask.Description));

                        txtEstimatedHours.Text = pTask.EstimatedHours.ToString();
                        lbEstimatedHours.Text = string.Format("{0}", pTask.EstimatedHours);
                        txtActualHours.Text = pTask.ActualHours.ToString();
                        lbActualHours.Text = string.Format("{0}", pTask.ActualHours);
                        lbStatus.Text = pTask.Status;
                        rblTaskBehaviour.SelectedIndex = rblTaskBehaviour.Items.IndexOf(rblTaskBehaviour.Items.FindByValue(pTask.Behaviour));
                        if (rblTaskBehaviour.SelectedIndex == -1)
                            rblTaskBehaviour.SelectedIndex = 0;
                        txtEstimatedRemainingHours.Text = Convert.ToString(Convert.ToInt32(pTask.EstimatedHours) - Convert.ToInt32(pTask.ActualHours));// Convert.ToString(pTask.EstimatedRemainingHours);
                        lblEstimatedRemainingHours.Text = string.Format("{0}", pTask.EstimatedRemainingHours);
                        //new set for completedon on level....
                        if (pTask.CompletedBy != null && pTask.PercentComplete == 100)
                        {
                            UserProfile cUser = UserManager.GetUserInfoById(pTask.CompletedBy);
                            string cUserName = cUser != null ? cUser.Name : string.Empty;

                            if (!string.IsNullOrEmpty(cUserName))
                                lblCompletedBy.Text = string.Format("Completed: <br><b>{0}</b> by <b>{1}</b>", UGITUtility.getDateStringInFormat(pTask.CompletionDate, true), cUserName);
                        }
                        else if (pTask.CompletionDate != DateTime.MinValue && pTask.PercentComplete == 100)
                        {
                            lblCompletedBy.Text = string.Format("Completed On: <br><b>{0}</b>", UGITUtility.getDateStringInFormat(pTask.CompletionDate, true));
                        }

                        // new line of code show task reminder days.
                        chkTaskReminderEnable.Checked = pTask.ReminderEnabled;
                        if (!chkTaskReminderEnable.Checked)
                        {
                            chkTaskReminderEnable.Checked = false;
                            chkTaskReminderEnable_CheckedChanged(null, null);
                        }
                        if (pTask.ReminderEnabled)
                        {
                            if (pTask.ReminderDays == 0 || pTask.ReminderDays % 7 != 0)
                            {
                                ddlCount.SelectedValue = Convert.ToString(pTask.ReminderDays).Replace('-', ' ').Trim();
                                ddlTimeInterval.SelectedValue = "Days";
                            }
                            else
                            {
                                ddlCount.SelectedValue = Convert.ToString(pTask.ReminderDays / 7).Replace('-', ' ').Trim();
                                ddlTimeInterval.SelectedValue = "Weeks";
                            }

                            if (pTask.RepeatInterval == 0 || pTask.RepeatInterval % 7 != 0)
                            {
                                ddlRepeatCount.SelectedValue = Convert.ToString(pTask.RepeatInterval).Replace('-', ' ').Trim();
                                ddlInetrvalInWeeknDays.SelectedValue = "Days";
                            }
                            else
                            {
                                ddlRepeatCount.SelectedValue = Convert.ToString(pTask.RepeatInterval / 7).Replace('-', ' ').Trim();
                                ddlInetrvalInWeeknDays.SelectedValue = "Weeks";
                            }

                            lblTimeInterval.Text = ddlTimeInterval.SelectedValue;
                            lblCount.Text = ddlCount.SelectedValue;

                            if (Convert.ToString(pTask.ReminderDays).Contains('-'))
                            {
                                ddlIntervalType.SelectedValue = "-";
                                lblIntervalType.Text = "Before";
                            }
                            else
                            {
                                ddlIntervalType.SelectedValue = "+";
                                lblIntervalType.Text = "After";
                            }
                        }

                        rComments.DataSource = pTask.CommentHistory;
                        rComments.DataBind();

                        //  Add attachment is commented because Upload files in Portal is integrated
                        //ShowAttachmentList(pTask, true);
                        //if (!string.IsNullOrEmpty(pTask.LinkedDocuments))
                        //{
                        //    BindDocumentsGrid(pTask.LinkedDocuments);
                        //}

                        //If task is parent task then make startdate, duedate, estimated hours readonly. 
                        //Because these are dependent on child tasks.
                        if (isParent)
                        {
                            dtcStartDate.Visible = false;
                            dtcDueDate.Visible = false;
                            txtEstimatedHours.Visible = false;
                            lbStartDate.Visible = true;
                            lbDueDate.Visible = true;
                            lbEstimatedHours.Visible = true;
                            txtActualHours.Visible = false;
                            lbActualHours.Visible = true;
                            txtEstimatedRemainingHours.Visible = false;
                            lblEstimatedRemainingHours.Visible = true;
                        }

                        UserProfile user = null;
                        string userName = string.Empty;

                        user = UserManager.GetUserInfoById(pTask.CreatedBy);
                        userName = user != null ? user.Name : string.Empty;
                        lbCreatedInfo.Text = string.Format("Created: {0} by {1}", UGITUtility.getDateStringInFormat(pTask.Created, true), userName);

                        user = UserManager.GetUserInfoById(pTask.ModifiedBy);
                        userName = user != null ? user.Name : userName;
                        lbModifiedInfo.Text = string.Format("Modified: {0} by {1}", UGITUtility.getDateStringInFormat(pTask.Modified, true), userName);
                    }

                    //new line of code here........
                    int rowCounts = 0;
                    if (Request[hdnReptRowcount.UniqueID] == "0" || Request[hdnReptRowcount.UniqueID] == "" || Request[hdnReptRowcount.UniqueID] == null)
                    {
                        rowCounts = 0;
                    }
                    else
                    {
                        rowCounts = Convert.ToInt32(Request[hdnReptRowcount.UniqueID]);
                    }

                    string strAssingToPct = pTask != null ? pTask.AssignToPct : null;

                    if (!string.IsNullOrEmpty(strAssingToPct))
                    {
                        if (!IsPostBack)
                        {
                            assignToList = TaskManager.GetUGITAssignPct(strAssingToPct);
                            rowCounts = assignToList.Count;
                        }
                    }

                    if (rowCounts == 0)
                    {
                        assignToList.Add(new UGITAssignTo("", "", ""));
                        isRowCount = true;
                    }
                    else
                    {
                        if (assignToList.Count == 0)
                        {
                            for (int i = 0; i < Convert.ToInt32(rowCounts); i++)
                            {
                                assignToList.Add(new UGITAssignTo("", "", ""));
                            }
                        }
                        isRowCount = false;
                    }

                    rAssignToPct.DataSource = assignToList;
                    rAssignToPct.DataBind();

                    hdnReptRowcount.Value = Convert.ToString(rAssignToPct.Items.Count);

                    //new line of code for sprint.
                    if (pTask != null && pTask.SprintLookup > 0)
                    {
                        dtcDueDate.Visible = false;
                        lbDueDate.Visible = true;
                        dtcStartDate.Visible = false;
                        lbStartDate.Visible = true;
                        txtEstimatedHours.Visible = false;
                        lbEstimatedHours.Visible = true;
                        txtActualHours.Visible = false;
                        lbActualHours.Visible = true;
                        txtEstimatedRemainingHours.Visible = false;
                        lblEstimatedRemainingHours.Visible = true;
                        txtPctComplete.Visible = false;
                        lbPctComplete.Visible = true;
                    }

                    //Add ticket actual hours grid to ender ticket hours
                    TicketActualHoursView ticketHoursViews = Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/TicketActualHoursView.ascx") as TicketActualHoursView;
                    ticketHoursViews.ModuleName = ModuleName;
                    ticketHoursViews.TicketID = projectPublicID;
                    ticketHoursViews.WorkItem = projectPublicID;
                    ticketHoursViews.TaskID = taskID;
                    ticketHoursViews.pTask = pTask;
                    ticketHoursViews.AfterSave += TicketHoursViews_AfterSave;

                    if (project != null)
                        ticketHoursViews.TicketStageStep = UGITUtility.StringToInt(project[DatabaseObjects.Columns.StageStep]);

                    panelActualHours.Controls.Add(ticketHoursViews);
                }
                #endregion

                #region SVC Config
                else if (ModuleName == "SVCConfig")
                {
                    UserProfile user = null;
                    string UserName = string.Empty;

                    GridLookupApplicationQuestion.GridView.Width = GridLookupApplicationQuestion.Width;
                    GridLookupApplicationQuestion.GridView.Style.Add("width", "168px");

                    btDeleteMappingTask.Visible = true;
                    //ServiceTask serviceItem = ServiceTask.LoadByID(taskID);

                    ptasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                    UGITTask serviceItem;

                    if (taskID > 0)
                        serviceItem = ptasks.FirstOrDefault(x => x.ID == taskID);
                    else
                        serviceItem = ptasks.FirstOrDefault(x => x.ID == parentTaskID);
                    //Added by mudassir 28 feb 2020
                    ServiceTaskManager serviceTaskManager = new ServiceTaskManager(context);

                    //service.Tasks = serviceTaskManager.LoadByServiceID(Convert.ToString(service.ID));
                    ServiceQuestionMappingManager serviceQuestionMappingManager = new ServiceQuestionMappingManager(context);
                    service.QuestionsMapping = serviceQuestionMappingManager.GetByServiceID(service.ID);

                    ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(context);
                    service.Questions = serviceQuestionManager.GetByServiceID(service.ID, false);



                    //serviceItem = serviceTaskManager.LoadByServiceID(Convert.ToString(taskID));
                    //ServiceQuestionMappingManager objServiceQuestionMappingManager = new ServiceQuestionMappingManager(context);
                    //List<ServiceQuestionMapping> ServiceQuestionMapping = objServiceQuestionMappingManager.Load(x => x.ServiceTaskID == taskID && x.ServiceQuestionID != null).ToList();

                    ServiceQuestionMapping ServiceQuestionMapping = service.QuestionsMapping.FirstOrDefault(x => x.ServiceTaskID == taskID && x.ServiceQuestionID != null);

                    //ServiceQuestionMappingManager serviceQuestionMappingManager = ServiceQuestionMapping.FirstOrDefault(X => X.ServiceTaskID == taskID);
                    //ServiceQuestion serviceQuestion = new ServiceQuestion();
                    //List<ServiceQuestion> serviceQuestion = serviceTaskManager.Load(x => x.ID == ServiceQuestionMapping[0].ServiceQuestionID ).ToList();


                    //ServiceQuestion serviceQuestion = service.Questions.FirstOrDefault(x => x.ID == ServiceQuestionMapping.ServiceQuestionID);

                    //List< ServiceQuestionMappingManager> listserviceQuestionMappingManager = service.QuestionsMapping.FirstOrDefault(x => x.ServiceTaskID == taskID);
                    //ServiceQuestionMappingManager objServiceQuestionManager = new ServiceQuestionMappingManager(context);
                    //List<ServiceQuestionMappingManager> ServiceQuestionMapping = objServiceQuestionManager.Load(x => x.ServiceID == service.ID);
                    //List<ServiceQuestion> ServiceQuestion = objServiceQuestionManager.Load(x => x.ServiceID == service.ID).ToList();

                    //
                    txtTitle.Text = serviceItem.Title;
                    tdApplicationAccess.Style.Add("Display", "none");
                    tdAccountTask.Style.Add("Display", "none");

                    //Test this
                    //ddlModuleStep.SelectedValue = Convert.ToString(serviceItem.StageStep);

                    if (serviceItem.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                    {
                        chkAccountTask.Checked = true;
                        taskType = ServiceSubTaskType.AccountTask;
                        ddlTypes.SelectedIndex = 1;

                        chkAutoUserCreation.Visible = true;
                        chkAutoFillRequestor.Visible = true;

                        if (serviceItem.AutoCreateUser)
                            chkAutoUserCreation.Checked = true;

                        if (serviceItem.AutoFillRequestor)
                            chkAutoFillRequestor.Checked = true;

                        tdAccountTask.Style.Remove("Display");
                    }
                    else if (serviceItem.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
                    {
                        taskType = ServiceSubTaskType.AccessTask;
                        FillApplicationType(ddlApplicationAccess);
                        ddlApplicationAccess.SelectedIndex = ddlApplicationAccess.Items.IndexOf(ddlApplicationAccess.Items.FindByValue(serviceItem.QuestionID));

                        ddlApplicationAccess_SelectedIndexChanged(ddlApplicationAccess, null);
                        ddlTypes.SelectedIndex = 2;
                        GridLookupApplicationQuestion.GridView.Selection.SelectRowByKey(serviceItem.QuestionProperties);
                        trApplicationAccess.Visible = true;

                        if (ddlApplicationAccess.Items.Count > 1)
                            tdApplicationAccess.Style.Remove("Display");
                    }

                    if (!string.IsNullOrEmpty(serviceItem.Predecessors))               // (!= null && serviceItem.Predecessors.Count > 0)
                    {
                        tpControl.SelectAllPredecessor(serviceItem.Predecessors);
                    }

                    if (taskType.ToLower() == ServiceSubTaskType.Task.ToLower() || taskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower() || taskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                    {
                        txtDescription.Text = Server.HtmlDecode(serviceItem.Description);

                        // test below two code lines
                        //lbDescription.Text = uHelper.FindAndConvertToAnchorTag(Server.HtmlDecode(serviceItem.Description));
                        //peAssignedTo.SetValues(Convert.ToString(serviceItem.AssignedTo));
                    }
                    else // is a ticket
                    {
                        // fix below code, these controls aren't avialable in existing code
                        //if (!string.IsNullOrEmpty(serviceItem.RelatedModule))
                        //    ddlModuleDetail.SelectedIndex = ddlModuleDetail.Items.IndexOf(ddlModuleDetail.Items.FindByValue(serviceItem.Module.LookupId.ToString()));

                        //string module = string.Empty;
                        //if (string.IsNullOrWhiteSpace(Request[ddlModuleDetail.UniqueID]))
                        //    module = ddlModuleDetail.SelectedItem.Value;
                        //else
                        //    module = Request[ddlModuleDetail.UniqueID];
                        //DdlModuleDetail_SelectedIndexChanged(ddlRequestTypeSubCategory, new EventArgs());
                        //int moduleID = 0;
                        //int.TryParse(module, out moduleID);
                        //if (serviceItem.RequestCategory != null)
                        //{
                        //    string selectedRequestType = Convert.ToString(serviceItem.RequestCategory.LookupId);

                        //    DataRow moduleRow = uGITCache.GetModuleDetails(moduleID);
                        //    string strmoduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
                        //    DataRow[] selectedRTS = uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.ModuleNameLookup, strmoduleName);
                        //    if (selectedRTS.Length > 0)
                        //    {
                        //        DataRow dr = selectedRTS.FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.Id).ToString() == selectedRequestType);
                        //        if (dr != null)
                        //        {
                        //            string selectedCategory = Convert.ToString(dr[DatabaseObjects.Columns.Category]);
                        //            if (dr.Table.Columns.Contains(DatabaseObjects.Columns.RequestTypeSubCategory) && !string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.RequestTypeSubCategory])))
                        //                selectedCategory = selectedCategory + ";#;" + dr[DatabaseObjects.Columns.RequestTypeSubCategory];

                        //            if (ddlRequestTypeSubCategory.Items.Count > 0)
                        //                ddlRequestTypeSubCategory.SelectedIndex = ddlRequestTypeSubCategory.Items.IndexOf(ddlRequestTypeSubCategory.Items.FindByValue(selectedCategory));

                        //            ddlRequestTypeSubCategory_SelectedIndexChanged(null, null);

                        //            if (serviceItem.RequestCategory != null && ddlTicketRequestType.Items.Count > 0)
                        //                ddlTicketRequestType.SelectedIndex = ddlTicketRequestType.Items.IndexOf(ddlTicketRequestType.Items.FindByValue(serviceItem.RequestCategory.LookupId.ToString()));
                        //        }
                        //    }
                        //}
                    }

                    txtWeight.Text = serviceItem.Weight.ToString();
                    txtEstimatedHours.Text = lbEstimatedHours.Text = serviceItem.EstimatedHours.ToString();

                    if (!string.IsNullOrEmpty(peAssignedTo.GetValues()))
                    {
                        serviceItem.AssignedTo = peAssignedTo.GetValues();
                    }
                    peAssignedTo.SetValues(serviceItem.AssignedTo);
                    //txtDescription.Text = serviceItem.Description.ToString();

                    // test below six code lines
                    UGITModule selectedmodule = ObjModuleViewManager.LoadByName(serviceItem.RelatedModule);
                    if (selectedmodule != null)
                    {
                        ddlModuleDetail.SelectedIndex = ddlModuleDetail.Items.IndexOf(ddlModuleDetail.Items.FindByValue(selectedmodule.ModuleName));
                        //dropDownBox.ModuleName = selectedmodule.ModuleName;
                    }
                    //tdRequestType.Controls.Add(dropDownBox);
                    dropDownBox.SetValues(serviceItem.RequestTypeCategory);
                    DdlModuleDetail_SelectedIndexChanged(ddlRequestTypeSubCategory, new EventArgs());
                    #region bind request type
                    if (!string.IsNullOrWhiteSpace(serviceItem.RequestTypeCategory))
                    {
                        string selectedRequestType = serviceItem.RequestTypeCategory;
                        RequestTypeManager requestTypeManager = new RequestTypeManager(context);

                        List<ModuleRequestType> selectedRTS = requestTypeManager.Load(x => x.ModuleNameLookup.EqualsIgnoreCase(serviceItem.RelatedModule));
                        //DataRow[] selectedRTS = uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.ModuleNameLookup, strmoduleName);
                        if (selectedRTS != null && selectedRTS.Count > 0)
                        {
                            ModuleRequestType dr = selectedRTS.FirstOrDefault(x => UGITUtility.ObjectToString(x.ID) == selectedRequestType);
                            if (dr != null)
                            {
                                string selectedCategory = dr.Category;
                                if (string.IsNullOrWhiteSpace(dr.SubCategory))
                                    selectedCategory = selectedCategory + ";#;" + dr.SubCategory;

                                if (ddlRequestTypeSubCategory.Items.Count > 0)
                                    ddlRequestTypeSubCategory.SelectedIndex = ddlRequestTypeSubCategory.Items.IndexOf(ddlRequestTypeSubCategory.Items.FindByValue(selectedCategory));

                                ddlRequestTypeSubCategory_SelectedIndexChanged(null, null);

                                if (!string.IsNullOrWhiteSpace(serviceItem.RequestTypeCategory) && ddlTicketRequestType.Items.Count > 0)
                                    ddlTicketRequestType.SelectedIndex = ddlTicketRequestType.Items.IndexOf(ddlTicketRequestType.Items.FindByValue(serviceItem.RequestTypeCategory));
                            }
                        }
                    }
                    #endregion

                    user = UserManager.GetUserInfoById(serviceItem.CreatedBy);
                    UserName = user != null ? user.Name : string.Empty;
                    lbCreatedInfo.Text = string.Format("Created at {0} by {1}", UGITUtility.getDateStringInFormat(serviceItem.Created, true), UserName);

                    user = UserManager.GetUserInfoById(serviceItem.ModifiedBy);
                    UserName = user != null ? (!string.IsNullOrEmpty(user.Name) ? user.Name : UserName) : UserName;
                    lbModifiedInfo.Text = string.Format("Modified at {0} by {1}", UGITUtility.getDateStringInFormat(serviceItem.Modified, true), UserName);

                    chkApprovalRequired.Checked = serviceItem.EnableApproval;
                    if (chkApprovalRequired.Checked)
                    {
                        fsApproval.Visible = true;
                        peApprover.Visible = true;
                        peApprover.SetValues(serviceItem.Approver);
                    }
                    chkDisableSLA.Checked = serviceItem.SLADisabled;
                    chkDisableNotification.Checked = serviceItem.NotificationDisabled;
                }
                #endregion
                #region SVC Request
                else if (ModuleName == "SVC")
                {
                    ptasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                    moduleInstDepny = TaskManager.LoadByID(taskID);
                    aspxHoldTask.Visible = true;

                    if (moduleInstDepny != null)
                    {
                        if (ugitModule != null && project != null && uHelper.IfColumnExists(DatabaseObjects.Columns.StageStep, project.Table))
                        {
                            allowHold = UGITUtility.StringToInt(project[DatabaseObjects.Columns.StageStep]) <= ugitModule.ModuleHoldMaxStage
                                && moduleInstDepny.Status != Constants.Waiting && moduleInstDepny.Status != Constants.Completed && moduleInstDepny.Status != Constants.Cancelled;
                        }

                        txtTitle.Text = moduleInstDepny.Title;
                        lbTitle.Text = moduleInstDepny.Title;

                        // Test- it is needed in SVCConfig only.
                        //ddlModuleStep.SelectedValue = Convert.ToString(moduleInstDepny.StageStep);
                        //Added by mudassir 2 march 2020
                        chkDisableNotification.Checked = moduleInstDepny.NotificationDisabled;
                        //
                        if (moduleInstDepny.Status != "Waiting")
                        {
                            ListItem statusOption = ddlStatus.Items.FindByText("Waiting");
                            if (statusOption != null)
                                statusOption.Enabled = false;
                        }

                        if (moduleInstDepny.Status != Constants.Completed && moduleInstDepny.Status != Constants.Cancelled)
                            lnkCancel.Visible = true;

                        if (moduleInstDepny.Status == Constants.Cancelled)
                            lnkUncancelTask.Visible = true;

                        #region Approval

                        bool isApprover = false;
                        fsApproval.Visible = true;

                        //if (!string.IsNullOrEmpty(peApprover.GetValues()))
                        //    moduleInstDepny.Approver = peApprover.GetValues();

                        //peApprover.SetValues(moduleInstDepny.Approver);

                        List<string> approvalNames = new List<string>();
                        List<string> pendingNames = new List<string>();

                        if (moduleInstDepny.Approver != null && UserManager.GetUserInfosById(moduleInstDepny.Approver).Count > 0)
                        {
                            List<UserProfile> approvers = UserManager.GetUserInfosById(moduleInstDepny.Approver);
                            foreach (UserProfile userA in approvers)
                            {
                                if (userA != null)
                                    approvalNames.Add(userA.UserName);
                            }
                            lbApprover.Text = string.Join("; ", approvers.Select(x => x.Name).ToList());
                        }

                        if (string.IsNullOrWhiteSpace(moduleInstDepny.ApprovalStatus) || moduleInstDepny.ApprovalStatus.ToLower() == TaskApprovalStatus.NotStarted)
                        {
                            trApproval.Visible = true;
                            peApprover.Visible = true;
                            if ((isApprover || UserManager.IsTicketAdmin(User) || UserManager.IsActionUser(project, User)))
                            {
                                UserManager.IsActionUser(project, User);
                                if (!string.IsNullOrEmpty(peApprover.GetValues()))
                                    trbtnApproveReject.Visible = true;
                                else
                                {
                                    lnkbtnAssignApprover.Visible = true;
                                    assignApprover.Visible = true;
                                }
                            }
                        }
                        else if (moduleInstDepny.ApprovalStatus.ToLower() == TaskApprovalStatus.Approved && approvalNames != null && approvalNames.Count > 0)
                        {
                            trApproval.Visible = false;
                            trApprovedBy.Visible = true;
                            lblApproverName.Text = "Approved By " + string.Join(", ", approvalNames.ToArray());
                        }
                        else if (moduleInstDepny.ApprovalStatus.ToLower() == TaskApprovalStatus.Rejected && approvalNames != null && approvalNames.Count > 0)
                        {
                            trApproval.Visible = false;
                            trApprovedBy.Visible = true;
                            lblApproverName.Text = "Rejected By " + string.Join(", ", approvalNames.ToArray());
                        }
                        else if (moduleInstDepny.ApprovalStatus.ToLower() == TaskApprovalStatus.Pending)
                        {
                            //if (moduleInstDepny.TaskActionUser != null && moduleInstDepny.TaskActionUser != string.Empty)
                            //{
                            //    SPFieldUserValueCollection pendingApprovers = new SPFieldUserValueCollection();
                            //    string[] taskActionUsers = UGITUtility.SplitString(moduleInstDepny.TaskActionUser, Constants.Separator);
                            //    if (taskActionUsers.Length > 0)
                            //    {
                            //        foreach (var item in taskActionUsers)
                            //        {
                            //            SPUser actionUser = UserProfile.GetUserById(UGITUtility.StringToInt(item));
                            //            if (actionUser != null)
                            //            {
                            //                SPFieldUserValue uv = new SPFieldUserValue(SPContext.Current.Web, actionUser.ID, actionUser.Name);
                            //                pendingApprovers.Add(uv);

                            //                pendingNames.Add(actionUser.Name);
                            //                if (SPContext.Current.Web.CurrentUser.ID == actionUser.ID)
                            //                    isApprover = true;
                            //            }
                            //            else
                            //            {
                            //                SPGroup group = UserProfile.GetGroupByID(UGITUtility.StringToInt(item));
                            //                if (group != null)
                            //                {
                            //                    SPFieldUserValue uv = new SPFieldUserValue(SPContext.Current.Web, group.ID, group.Name);
                            //                    pendingApprovers.Add(uv);

                            //                    pendingNames.Add(group.Name);
                            //                    if (UserProfile.CheckUserIsInGroup(group.Name, SPContext.Current.Web.CurrentUser))
                            //                        isApprover = true;
                            //                }
                            //            }
                            //        }

                            //        peApprover.UpdateEntities(UGITUtility.getUsersListFromCollection(pendingApprovers));

                            //        lbApprover.Text = string.Join("; ", pendingApprovers.Select(x => x.LookupValue));
                            //    }

                            //    if ((!string.IsNullOrEmpty(moduleInstDepny.ApprovalType) && moduleInstDepny.ApprovalType.ToLower() == ApprovalType.All))
                            //    {
                            //        if (pendingNames != null && approvalNames.Count > 0)
                            //        {
                            //            trApprovedBy.Visible = true;
                            //            lblApproverName.Text = string.Format("Approved By: {0}", string.Join(", ", approvalNames.ToArray()));
                            //        }
                            //    }
                            //}

                            if (moduleInstDepny.ApprovalStatus.ToLower() == TaskApprovalStatus.Pending &&
                                (isApprover || UserManager.IsTicketAdmin(User) || UserManager.IsActionUser(project, User)))
                            {
                                if (!string.IsNullOrEmpty(peApprover.GetValues()))
                                    trbtnApproveReject.Visible = true;
                                else
                                {
                                    lnkbtnAssignApprover.Visible = true;
                                    assignApprover.Visible = true;
                                }
                                peApprover.Visible = true;
                                lbApprover.Visible = false;
                                trApproval.Visible = true;
                            }
                            else
                            {
                                trApproval.Visible = false;
                                peApprover.Visible = false;
                                lbApprover.Visible = true;
                            }
                        }

                        if (moduleInstDepny.Status.ToLower() != Constants.Waiting.ToLower() && moduleInstDepny.Status.ToLower() != Constants.Completed.ToLower() &&
                              (isApprover || UserManager.IsTicketAdmin(User) || UserManager.IsActionUser(project, User) /*|| UGITUtility.IsUserPresentInUserCollection(currentUser, moduleInstDepny.AssignedTo) */))
                        {
                            trApproval.Visible = true;
                            peApprover.Visible = true;
                            lbApprover.Visible = false;
                            List<UserProfile> approvedBy = UserManager.GetUserInfosById(moduleInstDepny.Approver);
                            List<UserProfile> approvers = UserManager.GetUserInfosById(moduleInstDepny.Approver);
                            List<UserProfile> pendingApprovrs = new List<UserProfile>();
                            if (approvedBy != null && approvedBy.Count > 0 && approvers != null)
                            {
                                foreach (UserProfile approver in approvers)
                                {
                                    if (approvers == null || !approvedBy.Exists(x => x.Id == approver.Id))
                                    {
                                        pendingApprovrs.Add(approver);
                                    }
                                }

                                if (pendingApprovrs.Count == 0)
                                    peApprover.SetValues(string.Empty);
                            }

                            lnkbtnAssignApprover.Visible = true;
                            assignApprover.Visible = true;
                        }

                        #endregion

                        if (moduleInstDepny.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask)
                        {
                            fsAccountTask.Visible = true;
                            trUserName.Visible = true;

                            lblErrorMsg.Text = moduleInstDepny.ErrorMsg;
                            UserProfile user = UserManager.GetUserInfoById(moduleInstDepny.NewUserName);
                            if (user != null && !string.IsNullOrEmpty(user.Id))
                            {
                                lbUserName.Text = string.Format("User Account created: <b>{0} ({1})</b>", user.Name, user.UserName);
                                //pplUserName.SetValues(user.Id);
                                txtUsername.Text = user.Name;
                            }
                            else
                            {
                                lbUserName.Text = string.Format("User Account: <b>{0}</b>", moduleInstDepny.NewUserName);
                                if (lblErrorMsg.Text == "")
                                    lbUserName.Visible = false;
                                    lblErrorMsg.Text = "Please create domain user and update the user account below";
                            }

                            txtUsername.Visible = true;//pplUserName.Visible = true;
                            //trbtnApproveReject.Visible -- this is not required because we need to show textbox to add user only.
                            if (moduleInstDepny.Status == "Completed")
                                txtUsername.Visible = false; //pplUserName.Visible = false;

                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(moduleInstDepny.NewUserName))
                            {
                                UserProfile user = UserManager.GetUserInfoById(moduleInstDepny.NewUserName);
                                if (user != null)
                                {
                                    fsAccountTask.Visible = true;
                                    trUserName.Visible = true;
                                    lbUserName.Text = string.Format("User Account : <b>{0} ({1})</b>", user.Name, user.UserName);
                                }
                                txtUsername.Visible = false; //pplUserName.Visible = false;
                            }
                        }

                        txtDescription.Text = Server.HtmlDecode(moduleInstDepny.Description);
                        //If the text is null or blank, the index goes -ve and throws and error.
                        if (!string.IsNullOrWhiteSpace(txtDescription.Text))
                            txtDescription.Rows = txtDescription.Text.Split('\n').Length + 2;

                        lbDescription.Text = UGITUtility.FindAndConvertToAnchorTag(Server.HtmlDecode(moduleInstDepny.Description));
                        txtPctComplete.Text = moduleInstDepny.PercentComplete.ToString();
                        lbPctComplete.Text = Convert.ToString(moduleInstDepny.PercentComplete);
                        lbStatus.Text = Convert.ToString(moduleInstDepny.Status);

                        if (moduleInstDepny.Predecessors != null && UGITUtility.ConvertStringToList(moduleInstDepny.Predecessors, Constants.Separator6).Count > 0)
                        {
                            tpControl.SelectAllPredecessor(moduleInstDepny.Predecessors);
                        }

                        if (moduleInstDepny.Status.ToLower() == Constants.Completed.ToLower() || moduleInstDepny.Status.ToLower() == Constants.Cancelled.ToLower())
                        {
                            tdCompletedOn.Visible = true;
                            if (moduleInstDepny.CompletedBy != null && UGITUtility.ConvertStringToList(moduleInstDepny.CompletedBy, Constants.Separator6).Count > 0)
                            {
                                UserProfile user = UserManager.GetUserInfoById(moduleInstDepny.CompletedBy);
                                lblCompletedBy.Text = string.Format("Completed:<br> <b>{0}</b> by <b>{1}</b>", UGITUtility.getDateStringInFormat(moduleInstDepny.CompletionDate, true), user.UserName);
                            }
                        }

                        if (moduleInstDepny.TaskActualStartDate.HasValue && moduleInstDepny.TaskActualStartDate.Value != DateTime.MinValue)
                        {
                            startDateNew.Visible = true;
                            lblStartDateNew.Text = string.Format("Start Date:<br> <b>{0}</b>", UGITUtility.GetDateStringInFormat(moduleInstDepny.TaskActualStartDate.Value, true));
                        }

                        if (moduleInstDepny.Behaviour == "Ticket")
                        {
                            trSvcSelectModules.Visible = false;
                        }
                        else
                        {
                            ListItem itemCancelled = new ListItem(moduleInstDepny.Status, moduleInstDepny.Status);
                            if (moduleInstDepny.Status.ToLower() == "cancelled" && !ddlStatus.Items.Contains(itemCancelled))
                                ddlStatus.Items.Add(itemCancelled);

                            if (moduleInstDepny.OnHold && moduleInstDepny.Status.ToLower() == "on hold")
                            {
                                ListItem itemOnHold = new ListItem(moduleInstDepny.Status, moduleInstDepny.Status);
                                if (!ddlStatus.Items.Contains(itemOnHold))
                                    ddlStatus.Items.Add(itemOnHold);
                            }

                            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(moduleInstDepny.Status));

                            List<string> peopleNames = new List<string>();
                            List<string> peopleDisplayName = new List<string>();
                            if (moduleInstDepny.AssignedTo != null && UserManager.GetUserInfosById(moduleInstDepny.AssignedTo).Count > 0)
                            {
                                foreach (UserProfile userV in UserManager.GetUserInfosById(moduleInstDepny.AssignedTo))
                                {
                                    if (userV != null)
                                    {
                                        peopleDisplayName.Add(userV.UserName);
                                        peopleNames.Add(userV.UserName);
                                    }
                                }
                                if (!string.IsNullOrEmpty(peAssignedTo.GetValues()))
                                {
                                    moduleInstDepny.AssignedTo = peAssignedTo.GetValues();
                                }
                                peAssignedTo.SetValues(moduleInstDepny.AssignedTo);
                            }

                            lbAssignedTo.Text = string.Join("; ", peopleDisplayName.ToArray());
                            moduleInstDepny.CommentHistory = uHelper.GetHistory(moduleInstDepny.Comment);
                            rComments.DataSource = moduleInstDepny.CommentHistory;
                            rComments.DataBind();
                        }

                        txtWeight.Text = moduleInstDepny.Weight.ToString();
                        lbWeight.Text = moduleInstDepny.Weight.ToString();

                        txtEstimatedHours.Text = moduleInstDepny.EstimatedHours.ToString();
                        lbEstimatedHours.Text = moduleInstDepny.EstimatedHours.ToString();

                        // For SVC tasks, estimated hours are pre-configured so make it readonly always!
                        txtEstimatedHours.Visible = false;
                        lbEstimatedHours.Visible = true;

                        txtActualHours.Text = moduleInstDepny.ActualHours.ToString();
                        lbActualHours.Text = moduleInstDepny.ActualHours.ToString();

                        //if (moduleInstDepny.EndDate.HasValue)
                        //{
                        //    dtcDueDate.Date = moduleInstDepny.EndDate.Value;
                        //    lbDueDate.Text = Convert.ToString(moduleInstDepny.EndDate.Value);
                        //}

                        if (moduleInstDepny.DueDate != null && moduleInstDepny.DueDate != DateTime.MinValue)
                        {
                            dtcDueDate.Date = moduleInstDepny.DueDate;
                            lbDueDate.Text = string.Format("{0:MM/dd/yyyy}", moduleInstDepny.DueDate);
                        }

                        bool isReadOnly = false;

                        if (moduleInstDepny.Status.ToLower() == "waiting" || (!UserManager.IsTicketAdmin(User) && !IsCurrentUserAssignedToTask(moduleInstDepny) && !UserManager.IsUserPresentInField(User, project, DatabaseObjects.Columns.TicketOwner)))
                        {
                            MakeReadOnly();
                            isReadOnly = true;
                        }


                        isHold = moduleInstDepny.OnHold;

                        if (moduleInstDepny.OnHold)
                        {
                            aspxHoldTask.Visible = false;
                            if (!isReadOnly)
                                aspxUnholdTask.Visible = true;
                        }
                        else // Not on hold
                        {
                            aspxUnholdTask.Visible = false;
                            if (!isReadOnly)
                                aspxHoldTask.Visible = true;
                        }

                        ShowAttachmentList(moduleInstDepny, !isReadOnly);

                        // Make Status and % Complete read only
                        if (moduleInstDepny.Status == Constants.Cancelled)
                        {
                            ddlStatus.Visible = false;
                            lbStatus.Visible = true;
                            txtPctComplete.Visible = false;
                            lbPctComplete.Visible = true;
                        }
                    }

                    if (moduleInstDepny != null && (moduleInstDepny.Behaviour == "Task" || string.IsNullOrEmpty(moduleInstDepny.Behaviour)))
                    {
                        string AccessAdmin = moduleInstDepny.AssignedTo != null ? moduleInstDepny.AssignedTo.ToString() : "";
                        chkDisableSLA.Checked = moduleInstDepny.SLADisabled;
                        if (!string.IsNullOrEmpty(moduleInstDepny.ServiceApplicationAccessXml))
                        {
                            XmlDocument doc = new XmlDocument();
                            string applicationAccessXml = HttpContext.Current.Server.HtmlDecode(moduleInstDepny.ServiceApplicationAccessXml);
                            doc.LoadXml(applicationAccessXml);
                            XmlElement root = doc.DocumentElement;
                            XmlNode nodeModuleRoleRelationList = root.SelectSingleNode("//ModuleRoleRelationList");

                            if (nodeModuleRoleRelationList != null)
                            {
                                //RemoveAccessList removeAccessList = new RemoveAccessList();
                                //RemoveUserAccess removeUserAccess = (RemoveUserAccess)Page.LoadControl("~/_controltemplates/15/uGovernIT/RemoveUserAccess.ascx");
                                //removeAccessList = (RemoveAccessList)uHelper.DeSerializeAnObject(doc, removeAccessList);
                                //removeUserAccess.RemoveAccessList = removeAccessList;
                                //if (removeAccessList != null && removeAccessList.SelectionType == "removeallaccess")
                                //{
                                //    removeUserAccess.GetRemoveAllAccessControl();
                                //    removeUserAccess.IsShowRemoveAllAccessControl = true;
                                //}

                                //    removeUserAccess.IsReadOnly = true;

                                //    pnlRemoveUserAccess.Controls.Add(removeUserAccess);
                                //    trRemoveUser.Visible = true;
                                //    hdnControl.Value = "removeuseraccess";
                            }
                            else
                            {
                                ServiceMatrix serviceMatrix = (ServiceMatrix)Page.LoadControl("~/controltemplates/uGovernIT/Services/ServiceMatrix.ascx");
                                List<ServiceMatrixData> lstServiceMatrixData = new List<ServiceMatrixData>();
                                ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                                serviceMatrixData = (ServiceMatrixData)uHelper.DeSerializeAnObject(doc, serviceMatrixData);
                                lstServiceMatrixData.Add(serviceMatrixData);
                                serviceMatrix.DisableAllCheckBox = true;
                                serviceMatrix.ShowBasedOnAccessAdmin = true;
                                serviceMatrix.AccessAdmin = AccessAdmin;

                                if (!string.IsNullOrEmpty(moduleInstDepny.NewUserName))
                                {
                                    UserProfile newuser = UserManager.GetUserProfile(moduleInstDepny.NewUserName);
                                    if (newuser != null)
                                        lstServiceMatrixData[0].RoleAssignee = moduleInstDepny.NewUserName;
                                }

                                serviceMatrix.IsNoteEnabled = false;
                                serviceMatrix.ShowAccessDescription = true;
                                serviceMatrix.ParentControl = "Task";

                                serviceMatrix.IsReadOnly = true;
                                pnlServiceMatrix.Controls.Add(serviceMatrix);

                                // Update App Modules and App Roles for the user in ServiceMatrixdata
                                lstServiceMatrixData = RefreshAppModulesAndRoles(lstServiceMatrixData, serviceMatrix);

                                serviceMatrix.LoadOnState(lstServiceMatrixData);
                                trServiceMatrix.Visible = true;
                            }
                        }
                        else
                        {
                            trRemoveUser.Visible = false;
                            trServiceMatrix.Visible = false;
                        }

                        UserProfile user = null;
                        string userName = string.Empty;

                        user = UserManager.GetUserInfoById(moduleInstDepny.CreatedBy);
                        userName = user != null ? user.Name : string.Empty;
                        lbCreatedInfo.Text = string.Format("Created at {0} by {1}", UGITUtility.getDateStringInFormat(moduleInstDepny.Created, true), userName);

                        user = UserManager.GetUserInfoById(moduleInstDepny.ModifiedBy);
                        userName = user != null ? user.Name : userName;
                        lbModifiedInfo.Text = string.Format("Modified at {0} by {1}", UGITUtility.getDateStringInFormat(moduleInstDepny.Modified, true), userName);

                        List<UGITTask> depncies = TaskManager.LoadByProjectID(projectPublicID);
                        var dependencies = depncies.Where(x => x.ParentTaskID == taskID);

                        if (dependencies != null && dependencies.Count() > 0)
                        {
                            trAssignedTo.Visible = false;
                            dtcStartDate.Enabled = false;
                            dtcDueDate.Enabled = false;
                            txtEstimatedHours.Enabled = false;
                            txtEstimatedRemainingHours.Enabled = false;
                            txtActualHours.Enabled = false;
                            imgAssignee.Visible = false;
                            dueDateAutoCal.Visible = imgDueDateAutoCalculater.Visible = false;
                            imgRemainingHRS.Visible = false;
                            imgEstimatedHrs.Visible = false;
                        }

                        if (moduleInstDepny.OnHold || moduleInstDepny.TotalHoldDuration > 0)
                        {
                            trHoldDuration.Visible = true;
                            lbHoldDuration.Text = TaskManager.GetFormattedHoldTime(moduleInstDepny);
                        }

                        if (moduleInstDepny.OnHold)
                        {
                            taskHoldBlock.Visible = true;
                            ddlStatus.Visible = false;
                            lbStatus.Visible = true;
                            lbStatus.Text = "On-Hold";
                            lbStatus.ForeColor = System.Drawing.Color.Red;
                            lbHoldReason.Text = string.Format("Reason: {0}", moduleInstDepny.OnHoldReasonChoice);
                            txtPctComplete.Visible = false;
                            lbPctComplete.Visible = true;

                            if (moduleInstDepny.OnHoldTillDate.HasValue)
                            {
                                lbHoldTill.Text = string.Format("Hold Till: {0}", UGITUtility.GetDateStringInFormat(moduleInstDepny.OnHoldTillDate.Value, true));
                                aspxdtOnHoldDate.Date = moduleInstDepny.OnHoldTillDate.Value;
                                aspxOnHoldReason.SelectedIndex = aspxOnHoldReason.Items.IndexOf(aspxOnHoldReason.Items.FindByValue(moduleInstDepny.OnHoldReasonChoice));
                            }
                        }

                        // Actual hours by User block for SVC
                        ptasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);

                        // ShowActualHours check if projectstdDisable is false then show actual hour section
                        ServicesManager objServicesManager = new ServicesManager(context);
                        bool showActualHours = taskID > 0 && ShowActualHours(ticketRequest.Module.ModuleName);
                        if (showActualHours)
                        {
                            //Add ticket actual hours grid to ender ticket hours
                            TicketActualHoursView ticketHoursViews = Page.LoadControl("~/CONTROLTEMPLATES/Admin/ListForm/TicketActualHoursView.ascx") as TicketActualHoursView;
                            ticketHoursViews.ModuleName = ModuleName;
                            ticketHoursViews.TicketID = projectPublicID;
                            ticketHoursViews.TaskID = taskID;
                            //BTS-23-001076: Service Title is now sent for WorkItem instead of TicketId, to make it same as the Sharepoint code.
                            //This is done to stop incorrect addition of HoursTaken in TicketHoursManager > CreateOrUpdateOrDeleteActualHours method.
                            Services service = objServicesManager.LoadByID(Convert.ToInt64(project[DatabaseObjects.Columns.ServiceTitleLookup]));
                            ticketHoursViews.WorkItem = service.Title;
                            ticketHoursViews.SVCTask = moduleInstDepny;
                            ticketHoursViews.AfterSave += TicketHoursViews_AfterSave;

                            if (project != null)
                                ticketHoursViews.TicketStageStep = UGITUtility.StringToInt(project[DatabaseObjects.Columns.StageStep]);

                            if (!UserManager.IsActionUser(project, User) && !UserManager.IsTicketAdmin(User) && !IsCurrentUserAssignedToTask(moduleInstDepny))
                                ticketHoursViews.HideActions = true;

                            panelActualHours.Controls.Add(ticketHoursViews);
                        }
                    }
                }
                #endregion

                if (txtDescription.Visible)
                {
                    lbDescription.Visible = true;
                }

                lbPredecessor.Text = tpControl.SelectedPredecessorsText;
                lbPredecessor.Visible = true;
                editPredecessor.Visible = true;
                dvpheditcontrol.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            //Load item, works for new item
            else
            {
                #region Tasks
                if (ModuleName == "PMM" || ModuleName == "TSK" || ModuleName == "NPR" || TaskManager.IsModuleTasks(ModuleName))
                {
                    if (reminderEable)
                    {
                        chkTaskReminderEnable.Checked = reminderEable;
                        chkTaskReminderEnable_CheckedChanged(null, null);
                    }
                    ptasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                    tpControl.ProjectID = projectPublicID;
                    tpControl.SelectedPredecessorsId = new List<string>();
                    tpControl.SelectedPredecessorsText = "";
                    pheditcontrol.Controls.Add(tpControl);

                    // test below code line, it was in old code
                    //tdRequestType.Controls.Add(dropDownBox); //add request type control dynamically for new mode

                    var rowCounts = Request[hdnReptRowcount.UniqueID];

                    if (rowCounts == "0" || rowCounts == "" || rowCounts == null)
                    {
                        assignToList.Add(new UGITAssignTo("", "", ""));
                        isRowCount = true;
                    }
                    else
                    {
                        for (int i = 0; i < Convert.ToInt32(rowCounts); i++)
                        {
                            assignToList.Add(new UGITAssignTo("", "", ""));
                        }
                        isRowCount = false;
                    }

                    rAssignToPct.DataSource = assignToList;
                    rAssignToPct.DataBind();

                    //new lines for modification of skill code..
                    if (string.IsNullOrEmpty(Request[hdnEditSkill.UniqueID]))
                    {
                        jsonSkillData = "[]";
                    }
                    else
                    {
                        jsonSkillData = Request[hdnEditSkill.UniqueID];
                    }

                    lbActualHours.Text = "0";
                }

                if (ModuleName == "PMM")
                {
                    CheckIfPortalExists(string.Empty);
                }

                #endregion
                #region SVC Config
                else if (ModuleName == "SVCConfig")
                {
                    if (!string.IsNullOrWhiteSpace(taskType) && taskType.EqualsIgnoreCase(Constants.TaskType.Ticket))
                        //tdRequestType.Controls.Add(dropDownBox);
                        txtWeight.Text = "1";
                }
                #endregion
                #region SVC Request
                else if (ModuleName == "SVC")
                {
                    ptasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                    ListItem statusOption = ddlStatus.Items.FindByText("Waiting");
                    if (statusOption != null)
                        statusOption.Enabled = false;
                }
                #endregion
            }

            if (parentTaskID > 0 && taskType == "mytask")
            {
                trAssignedToNew.Visible = true;
                trAssignedTo.Visible = false;
                peAssignedTo.Visible = true;
                lbAssignedTo.Visible = false;
                peAssignedTo.SetValues(User.Id);
                peAssignedTo.isMulti = false;
                trComment.Visible = false;
                trSprints.Visible = false;
                trTaskBehaviour.Visible = false;
                btSaveAndNotify.Visible = false;
                btSaveAndNotifyMyTask.Visible = false;
                dtcStartDate.Date = DateTime.Today;
                dtcDueDate.Date = DateTime.Today;
                txtEstimatedHours.Text = "8";
                trShowonProjectCalendar.Visible = false;
                //trCritical.Visible = true;

                tdActualHours.Visible = false;
                tdlblActualHours.Visible = false;
            }

            if (RepeatableTask)
            {
                trRepeatableTask.Visible = true;
                trMilestone.Visible = false;
                trSprints.Visible = false;
                trComment.Visible = false;
                dtcStartDate.Date = DateTime.Today;
                dtcDueDate.Date = DateTime.Today;
            }

            if (!IsPostBack)
            {
                //Log.AuditTrail(_spWeb.CurrentUser,
                //               taskID == 0 ? string.Format("creating new task on {0} {1} [{2}]", uHelper.moduleTypeName(ModuleName), projectPublicID, parentTitle) :
                //                             string.Format("opened task ID {0}: [{1}] on {2} {3} [{4}]", taskID, txtTitle.Text, uHelper.moduleTypeName(ModuleName), projectPublicID, parentTitle),
                //               Request.Url);
            }

            //new line of code for skill with user grid..
            pStartDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.TicketActualStartDate) && project[DatabaseObjects.Columns.TicketActualStartDate] != DBNull.Value)
                pStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketActualStartDate]);

            pEndDate = DateTime.MinValue;
            if (UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.TicketActualCompletionDate) && project[DatabaseObjects.Columns.TicketActualCompletionDate] != DBNull.Value)
                pEndDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketActualCompletionDate]);

            PrepareAllocationGrid();
            if (!string.IsNullOrEmpty(projectPublicID))
            {
                int width = 90;
                int height = 90;
                anchrParentTicketId.Attributes.Add("href", "javascript:");
                anchrParentTicketId.InnerText = projectPublicID + " (" + parentTitle + ")";
                string url = Ticket.GenerateTicketURL(context, projectPublicID);
                string sourceURL = Server.UrlEncode(Request.Url.AbsolutePath);
                string title = string.Format("{0}: {1}", projectPublicID, parentTitle);
                anchrParentTicketId.Attributes.Add("onclick", string.Format("openTicketDialog('{0}', '', '{1}', '{3}', '{4}', 1,'{2}');", url, title, sourceURL, width, height));
            }

            ResourceSelectFilter = ConfigVariableHelper.GetValue(ConfigConstants.ResourceSelectFilter);

            base.OnInit(e);
        }

        void BindModuleStep(string selectedModule)
        {
            try
            {
                ddlModuleStep.ClearSelection();
                ddlModuleStep.Items.Clear();
                List<LifeCycle> spListModuleStep = lifeCycleHelper.LoadLifeCycleByModule(selectedModule);
                List<LifeCycleStage> rows = spListModuleStep[0].Stages.Where(x => x.ModuleNameLookup.Equals(selectedModule, StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(x => x.StageStep).ToList();
                rows.Select(x => x.ID);
                if (rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        string title = $"{row.StageStep}" + " - " + $"{row.StageTitle}";
                        // ddlModuleStep.Items.Add(new ListItem(title, Convert.ToString(row.ID)));
                        ddlModuleStep.Items.Add(new ListItem(title, Convert.ToString(row.StageStep)));
                    }
                }
                ddlModuleStep.Items.Insert(0, new ListItem("(None)", "0"));
                ddlModuleStep.SelectedIndex = 0;
            }
            catch (Exception)
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UploadAndLinkDocuments uploadAndLinkDocuments = new UploadAndLinkDocuments();

            btnRejectApp.Attributes.Add("onclick", "callRejectRequest(); return false;");

            Guid newFrameId = Guid.NewGuid();

            DocumentManagementUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=DocumentControl&ticketId={0}&module={2}",
                                                         projectID, newFrameId, ModuleName));
            //FolderName = Convert.ToString(Request["folderName"]);

            if (string.IsNullOrEmpty(ModuleName))

                ModuleName = string.IsNullOrEmpty(Request["ModuleName"]) ? Request["Module"] : Request["ModuleName"];

            projectID = projectPublicID = string.IsNullOrEmpty(Request["PublicTicketID"]) ? Request["ticketId"] : Request["PublicTicketID"];
            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(Request["projectID"])))
                projectID = projectPublicID = UGITUtility.ObjectToString(Request["projectID"]);
            FolderName = Convert.ToString(Request["folderName"]);

            IsTabActive = Convert.ToBoolean(Request["isTabActive"]);

            var acct = fileAttchmentId.Value.ToString();



            //   BindSubTaskType();
            //if (!string.IsNullOrEmpty(ddlModuleDetail.GetText()))
            //{
            //    dropDownBox.ModuleName = uHelper.getModuleNameByModuleId(context, Convert.ToInt32(ddlModuleDetail.GetValues()));
            //}

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            txtPctComplete.Attributes.Add("onblur", "modifyStatusFromPctComplete()");
            ddlStatus.Attributes.Add("onchange", "modifyPctCompleteFromStatus()");

            UGITModule moduleDetails = ObjModuleViewManager.LoadByName(ModuleName);
            if (moduleDetails != null && !string.IsNullOrEmpty(projectPublicID))
            {
                if (ModuleName == "PMM")
                {
                    documnetPickerUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, projectPublicID, projectPublicID.Replace("-", "_"), pnlRelatedDocuments.ClientID));
                    UploadPageUrl = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.uploadDocument);
                }
                ticketPickerUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&TicketId={2}&Type={3}&ControlId={4}", "listpicker", "Picker List", projectPublicID, "PMMRelateTicket", lblTicketReadOnly.ClientID));
            }

            // commented below & moved in pnlAssignToPct_Callback, 

            //new line of code here for assign ....
            //if (!IsPostBack)
            //{
            //    assignToList.Clear();
            //    foreach (RepeaterItem item in rAssignToPct.Items)
            //    {
            //        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            //        {
            //            var peopleEditor = (UserValueBox)item.FindControl("peAssignedToPct_" + item.ItemIndex);
            //            var textboxPct = (TextBox)item.FindControl("txtPercentage");
            //            UGITAssignTo assignToItem = new UGITAssignTo();

            //            if (peopleEditor.GetValuesAsList() != null && peopleEditor.GetValuesAsList().Count > 0)
            //            {
            //                UserValueBox entity = (UserValueBox)peopleEditor;
            //                if (entity != null && entity != null)
            //                {
            //                    UserProfile user = UserManager.GetUserById(peopleEditor.GetValuesAsList()[0]);
            //                    if (user != null)
            //                    {
            //                        assignToItem.LoginName = user.UserName;
            //                        assignToItem.UserName = user.Name;
            //                        assignToItem.Percentage = textboxPct.Text;
            //                    }

            //                    assignToList.Add(assignToItem);
            //                }
            //                btnAssignToPct.Enabled = true;
            //            }
            //            else
            //            {
            //                if (!assignToList.Exists(x => x.UserName == string.Empty))
            //                    assignToList.Add(new UGITAssignTo("", "", ""));
            //            }
            //        }
            //    }
            //}

            if (ModuleName == "PMM")
            {
                if (cbSprints.Checked)
                    checkSprint = "checked";
                else
                    checkSprint = "notchecked";

                if (checkSprint == "checked")
                {
                    dtcDueDate.Visible = false;
                    lbDueDate.Visible = true;
                    dtcStartDate.Visible = false;
                    lbStartDate.Visible = true;
                    txtEstimatedHours.Visible = false;
                    lbEstimatedHours.Visible = true;
                    txtActualHours.Visible = false;
                    lbActualHours.Visible = true;
                    txtEstimatedRemainingHours.Visible = false;
                    lblEstimatedRemainingHours.Visible = true;
                    txtPctComplete.Visible = false;
                    lbPctComplete.Visible = true;
                    //listPredecessors.Enabled = false;
                    sprintsDiv.Visible = true;
                }
                else
                {
                    dtcDueDate.Visible = true;
                    lbDueDate.Visible = false;
                    dtcStartDate.Visible = true;
                    lbStartDate.Visible = false;
                    txtEstimatedRemainingHours.Visible = true;
                    lblEstimatedRemainingHours.Visible = false;
                    txtPctComplete.Visible = true;
                    lbPctComplete.Visible = false;
                    //listPredecessors.Enabled = true;
                    sprintsDiv.Visible = false;
                    txtActualHours.Visible = true;
                    lbActualHours.Visible = false;

                    txtEstimatedHours.Visible = true;
                    lbEstimatedHours.Visible = false;
                    if (!IsPostBack && (ModuleName == "PMM") && ConfigVariableHelper.GetValueAsBool(ConfigConstants.PreventEstimatedHoursEdit))
                    {
                        if (!string.IsNullOrEmpty(txtEstimatedHours.Text) && txtEstimatedHours.Text != "0")
                        {
                            txtEstimatedHours.Visible = false;
                            lbEstimatedHours.Visible = true;
                        }
                    }

                    if (ModuleName == "PMM" && ticketRequest.Module.ActualHoursByUser)
                    {
                        txtActualHours.Visible = false;
                        lbActualHours.Visible = true;
                    }
                }
            }

            if (!IsPostBack && (ModuleName == "TSK") && ConfigVariableHelper.GetValueAsBool(ConfigConstants.PreventEstimatedHoursEdit))
            {
                if (!string.IsNullOrEmpty(txtEstimatedHours.Text) && txtEstimatedHours.Text != "0")
                {
                    txtEstimatedHours.Visible = false;
                    lbEstimatedHours.Visible = true;
                }
            }

            if (ModuleName == "TSK")
            {
                txtActualHours.Visible = true;
                lbActualHours.Visible = false;

                if (ticketRequest.Module.ActualHoursByUser)
                {
                    txtActualHours.Visible = false;
                    lbActualHours.Visible = true;
                }
            }

            //new condition which is use to show the task reminder.
            if (chkTaskReminderEnable.Checked)
            {
                ddlCount.Visible = true;
                ddlIntervalType.Visible = true;
                ddlTimeInterval.Visible = true;
                lblDueDateText.Visible = true;
                lblCount.Visible = false;
                lblIntervalType.Visible = false;
                lblTimeInterval.Visible = false;
                dvRepeatInterval.Visible = true;
            }
            else
            {
                ddlCount.Visible = false;
                ddlIntervalType.Visible = false;
                ddlTimeInterval.Visible = false;
                lblDueDateText.Visible = false;
                lblCount.Visible = false;
                lblIntervalType.Visible = false;
                lblTimeInterval.Visible = false;
                dvRepeatInterval.Visible = false;
            }

            //Make Actual hours mandatory based on config variable. 
            if (keepActualHourMandatory && txtActualHours.Visible)
            {
                mlblActualHours.Visible = true;
                rfvActualHours.Visible = true;
            }

            if (!IsPostBack)
            {
                if (UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.AutoAdjustAllocations) && Convert.ToBoolean(project[DatabaseObjects.Columns.AutoAdjustAllocations]) == true && trAssignedTo.Visible == true && tdAutoAdjustAllocation.Visible == true)
                    chAutoAdjustAllocation.Checked = true;
                else
                    chAutoAdjustAllocation.Checked = false;
            }

            rAssignToPct.DataSource = assignToList;
            GridLookupApplicationQuestion.GridView.Width = GridLookupApplicationQuestion.Width;

            if (lnkDelete.Visible)
                ASPxPopupActionMenu.Items.FindByName("DeleteTask").Visible = true;

            if (lnkCancel.Visible)
                ASPxPopupActionMenu.Items.FindByName("CancelTask").Visible = true;

            if (btDeleteMappingTask.Visible)
                ASPxPopupActionMenu.Items.FindByName("DeleteMappingTask").Visible = true;

            if (aspxHoldTask.Visible && ConfigVariableHelper.GetValueAsBool(ConfigConstants.EnableSVCTaskHold) && allowHold)
                ASPxPopupActionMenu.Items.FindByName("Hold").Visible = true;

            if (aspxUnholdTask.Visible && ConfigVariableHelper.GetValueAsBool(ConfigConstants.EnableSVCTaskHold))
            {
                ASPxPopupActionMenu.Items.FindByName("UnHold").Visible = true;
                ASPxPopupActionMenu.Items.FindByName("EditHold").Visible = true;
            }

            if (ASPxPopupActionMenu.Items.GetVisibleItemCount() > 0)
                aspxMoreActions.Visible = true;

            if (ModuleName == "SVC" && ticketRequest.Module.ActualHoursByUser)
            {
                txtActualHours.Visible = false;
                lbActualHours.Visible = true;
            }

            if (lnkUncancelTask.Visible)
                ASPxPopupActionMenu.Items.FindByName("UncancelTask").Visible = true;
        }

        protected bool isRequestTaskCompleted(UGITTask task)
        {
            bool isRequestTaskCompleted = true;
            string ticketId = task.TicketId;
            List<UGITTask> allTaskOnSameStage = TaskManager.LoadByProjectID(task.TicketId).Where(x => x.StageStep == task.StageStep).ToList();
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

        #region My task actions
        protected void BtSaveMyTask_Click(object sender, EventArgs e)
        {
            string userLoginName = string.Empty;

            Control btAction = (Control)sender;
            string strAssignToPct = string.Empty;
            bool taskCompleted = false;
            string status = (moduleInstDepny != null && moduleInstDepny.OnHold) ? Constants.OnHoldStatus : ddlStatus.SelectedValue;
            int pctComplete = 0;
            string oldStatus = string.Empty;
            double oldPctComplete = 0;
            string taskTitle = string.Empty;
            string latestComment = txtComment.Text.Trim();
            int.TryParse(txtPctComplete.Text.Trim(), out pctComplete);
            Dictionary<string, string> taskToEmail = new Dictionary<string, string>();

            if (moduleInstDepny != null && !moduleInstDepny.OnHold && ddlStatus.SelectedValue.ToLower() == Constants.Completed.ToLower() || pctComplete >= 100)
            {
                status = Constants.Completed;
                pctComplete = 100;
                taskCompleted = true;
            }

            #region Project Tasks
            if (ModuleName == "TSK" || ModuleName == "PMM" || ModuleName == "NPR" || TaskManager.IsModuleTasks(ModuleName))
            {
                UGITTask task = null;
                if (ptasks != null)
                {
                    ptasks = UGITTaskManager.MapRelationalObjects(ptasks);
                    task = ptasks.FirstOrDefault(x => x.ID == taskID);
                }

                if (task == null)
                    return;

                oldStatus = task.Status;
                oldPctComplete = task.PercentComplete;

                task.Status = status;
                task.PercentComplete = pctComplete;

                task.Comment = latestComment;

                if (chkCritical.Visible)
                    task.IsCritical = chkCritical.Checked;

                if (!ticketRequest.Module.ActualHoursByUser && (ModuleName == "TSK" || ModuleName == "PMM" || ModuleName == "SVC" || TaskManager.IsModuleTasks(ModuleName)))
                {
                    double hours = 0;
                    double.TryParse(txtActualHours.Text.Trim(), out hours);
                    task.ActualHours = hours;
                }

                List<string> involvedUsers = new List<string>();
                List<string> userMultiLookup = new List<string>();

                foreach (RepeaterItem item in rAssignToPct.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var peopleEditor = (UserValueBox)item.FindControl("peAssignedToPct_" + item.ItemIndex);
                        TextBox textboxPct = (TextBox)item.FindControl("txtPercentage");

                        if (peopleEditor.GetValuesAsList() != null && peopleEditor.GetValuesAsList().Count > 0)
                        {
                            UserProfile user = UserManager.GetUserById(peopleEditor.GetValuesAsList()[0]);

                            if (user != null)
                            {
                                string strPercentage = textboxPct.Text;
                                if (string.IsNullOrEmpty(strPercentage))
                                    strPercentage = "100";
                                if (!string.IsNullOrEmpty(strAssignToPct))
                                    strAssignToPct += Constants.Separator;

                                strAssignToPct += user.Id + Constants.Separator1 + strPercentage;
                                userMultiLookup.Add(user.Id);
                                involvedUsers.Add(user.Id);
                            }
                        }
                    }
                }
                task.AssignedTo = string.Join(Constants.Separator6, userMultiLookup);
                task.AssignToPct = strAssignToPct;

                if (task.AttachedFiles != null && task.AttachedFiles.Count > 0)
                {
                    List<string> deleteFiles = UGITUtility.ConvertStringToList(txtDeleteFiles.Text, new string[] { "~" });
                    foreach (string dFile in deleteFiles)
                    {
                        if (task.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == dFile.ToLower()).Value != null)
                        {
                            task.AttachedFiles.Remove(dFile);
                        }
                    }
                }

                task.AttachFiles = GetAttachedFiles();
                task.LinkedDocuments = hdnDocIds.Value;

                task.Changes = true;
                TaskManager.SaveTask(ref task, ModuleName, projectPublicID);
                TaskManager.PropagateTaskStatusEffect(ref ptasks, task);
                TaskManager.SaveTasks(ref ptasks, ModuleName, projectPublicID);

                //Stage move on milestone task
                if (ModuleName == "PMM")
                {
                    LifeCycle lifeCycle = ticketRequest.GetTicketLifeCycle(project);
                    if (lifeCycle.ID != 0)
                    {
                        List<string> messages = new List<string>();
                        ticketRequest.ProjectStageMove(project, ptasks, true, ref messages);
                    }
                }

                //Creates task information which need to send in mail
                taskTitle = task.Title;
                taskToEmail.Add("ProjectID", Convert.ToString(project[DatabaseObjects.Columns.TicketId]));
                taskToEmail.Add("ProjectTitle", Convert.ToString(project[DatabaseObjects.Columns.Title]));
                taskToEmail.Add("Title", task.Title);
                taskToEmail.Add("Description", task.Description);
                taskToEmail.Add("StartDate", task.StartDate.ToString());
                taskToEmail.Add("DueDate", task.DueDate.ToString());
                taskToEmail.Add("EstimatedHours", task.EstimatedHours.ToString());
                taskToEmail.Add("ActualHours", task.ActualHours.ToString());
                taskToEmail.Add("Status", task.Status.ToString());
                taskToEmail.Add("% Complete", task.PercentComplete.ToString());
            }
            #endregion

            #region SVC
            //Save task status for service tasks
            else if (ModuleName == "SVC")
            {
                oldStatus = moduleInstDepny.Status;
                oldPctComplete = moduleInstDepny.PercentComplete;

                moduleInstDepny.Status = status;
                moduleInstDepny.PercentComplete = pctComplete;
                moduleInstDepny.Comment = latestComment;

                double hours = 0;
                double.TryParse(txtActualHours.Text.Trim(), out hours);
                moduleInstDepny.ActualHours = hours;

                if (moduleInstDepny.Status == "Completed" || moduleInstDepny.PercentComplete >= 100)
                {
                    moduleInstDepny.Status = "Completed";
                    moduleInstDepny.PercentComplete = 100;
                }

                moduleInstDepny.AssignedTo = peAssignedTo.GetValues();

                //if (trServiceMatrix.Visible == true && moduleInstDepny.UGITSubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
                //{
                //    ServiceMatrix serviceMatrix = pnlServiceMatrix.Controls[0] as ServiceMatrix;
                //    if (serviceMatrix.IsReadOnly)
                //    {
                //        serviceMatrix.SaveState();
                //        List<ServiceMatrixData> serviceMatricsData = serviceMatrix.GetSavedState();

                //        if (serviceMatricsData != null && serviceMatricsData.Count > 0)
                //        {
                //            XmlDocument doc = uHelper.SerializeObject(serviceMatricsData[0]);
                //            if (doc != null)
                //                moduleInstDepny.ServiceApplicationAccessXml = Server.HtmlEncode(doc.OuterXml);

                //            if (ddlStatus.SelectedValue == Constants.Completed)
                //            {
                //                int userID = 0;
                //                if (!string.IsNullOrEmpty(moduleInstDepny.UGITNewUserName) && string.IsNullOrEmpty(serviceMatricsData[0].RoleAssignee))
                //                {
                //                    SPUser user = UserProfile.GetUserByName(moduleInstDepny.UGITNewUserName);
                //                    if (user != null)
                //                        serviceMatricsData[0].RoleAssignee = Convert.ToString(user.ID);
                //                }
                //                // For Application Role Assignee
                //                if (!string.IsNullOrEmpty(serviceMatricsData[0].RoleAssignee))
                //                    userID = uHelper.StringToInt(serviceMatricsData[0].RoleAssignee);
                //                if (userID > 0)
                //                    ApplicationHelper.UpdateApplicationSpecificAccess(_spWeb, userID, serviceMatricsData[0], true);
                //            }
                //        }

                //    }
                //}

                if (!string.IsNullOrEmpty(hdnControl.Value) && hdnControl.Value == "removeuseraccess")
                {
                    // Need to fix required changes in below function
                    //UpdateUserAccess();
                }

                //if (moduleInstDepny.Status == "Completed" && moduleInstDepny.UGITSubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                //{
                //    bool userAdded = false;
                //    SPFieldUserValueCollection userValueCollection = pplUserName.GetUserValueCollection();
                //    if (userValueCollection.Count > 0)
                //    {
                //        userLoginName = userValueCollection[0].LookupValue;
                //        userAdded = UserProfile.SaveNewUserInUserList(SPContext.Current.Web, userLoginName, moduleInstDepny.UGITNewUserDisplayName);
                //        if (userAdded)
                //            moduleInstDepny.UGITNewUserName = userLoginName;

                //    }

                //    List<ModuleInstanceDependency> tasksList = new List<ModuleInstanceDependency>();
                //    tasksList = ModuleInstanceDependency.LoadByPublicID(_spWeb, moduleInstDepny.ParentInstance);
                //    List<ModuleInstanceDependency> dependentTasks = tasksList.Where(c => c.Predecessors != null && c.Predecessors.Exists(y => y.LookupId == moduleInstDepny.ID)).ToList();
                //    foreach (ModuleInstanceDependency dependentTask in dependentTasks)
                //    {
                //        if (dependentTask.UGITSubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
                //        {
                //            dependentTask.UGITNewUserName = moduleInstDepny.UGITNewUserName;
                //            SPUser pUser = UserProfile.GetUserByName(moduleInstDepny.UGITNewUserName);
                //            if (pUser != null)
                //                dependentTask.Title = string.Format("{0} for {1}", dependentTask.Title.Split(new string[] { " for " }, StringSplitOptions.RemoveEmptyEntries)[0], pUser.Name);

                //            dependentTask.Save(_spWeb);
                //        }
                //    }
                //}

                // Get attachments
                if (moduleInstDepny.AttachedFiles != null && moduleInstDepny.AttachedFiles.Count > 0)
                {
                    List<string> deleteFiles = UGITUtility.ConvertStringToList(txtDeleteFiles.Text, new string[] { "~" });
                    foreach (string dFile in deleteFiles)
                    {
                        if (moduleInstDepny.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == dFile.ToLower()).Value != null)
                            moduleInstDepny.AttachedFiles.Remove(dFile);
                    }
                }
                moduleInstDepny.AttachFiles = GetAttachedFiles();

                TaskManager.SaveTask(ref moduleInstDepny, ModuleName, projectPublicID);
                // test- need to discuss about calling the propagateTaskEffect function to set the status of other tasks.

                //Creates task information which need to send in mail
                taskTitle = moduleInstDepny.Title;
                taskToEmail.Add(DatabaseObjects.Columns.ProjectID, Convert.ToString(project[DatabaseObjects.Columns.TicketId]));
                taskToEmail.Add(DatabaseObjects.Columns.Title, moduleInstDepny.Title);
                taskToEmail.Add(DatabaseObjects.Columns.LinkDescription, moduleInstDepny.Description);
                taskToEmail.Add(DatabaseObjects.Columns.StartDate, moduleInstDepny.StartDate.ToString());
                taskToEmail.Add(DatabaseObjects.Columns.DueDate, moduleInstDepny.EndDate.ToString());
                taskToEmail.Add(DatabaseObjects.Columns.EstimatedHours, moduleInstDepny.EstimatedHours.ToString());
                taskToEmail.Add(DatabaseObjects.Columns.Percent_Complete, moduleInstDepny.PercentComplete.ToString());

                #region Log History
                //Update project history to log change
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
                    uHelper.CreateHistory(context.CurrentUser, historyDesc, project, false, context);
                }
                #endregion
            }
            #endregion

            #region Notifications
            // Send mail on task update
            if (btAction.ID == "btSaveAndNotifyMyTask")
            {
                //test
                //ModuleInstanceDependency.SendEmailToAssignee(moduleInstDepny, _spWeb);
            }

            bool notifyProjectManager = ConfigVariableHelper.GetValueAsBool("NotifyProjectManager");

            if (notifyProjectManager)
            {
                //fetch project managers from project
                List<UserProfile> lstProjectManagers = UserManager.GetUserInfosById(UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketProjectManager]));

                //Checks current user is a project manager members list. if yes then don't send mail to him
                bool userIsManager = lstProjectManagers.Exists(x => x.Id == context.CurrentUser.Id);
                if (!userIsManager)
                {
                    List<string> emails = new List<string>();
                    foreach (UserProfile userProfile in lstProjectManagers)
                    {
                        if (userProfile != null && !string.IsNullOrEmpty(userProfile.Email))
                            emails.Add(userProfile.Email);
                    }

                    string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, "", true, false, context.TenantID);
                    StringBuilder taskEmail = new StringBuilder();
                    string greeting = ConfigVariableHelper.GetValue("Greeting");
                    string signature = ConfigVariableHelper.GetValue("Signature");
                    taskEmail.AppendFormat("{0} {1}<br /><br />", greeting, string.Join(",", emails.ToArray()));
                    taskEmail.AppendFormat("Task \"{0}\" has been {1} by {2}", taskTitle, taskCompleted ? "completed" : "updated", context.CurrentUser.Name);
                    taskEmail.Append("<br /><br />" + signature + "<br />");
                    taskEmail.Append(emailFooter);
                    string subject = string.Format("{2} Task {0}: {1}", taskCompleted ? "Completed" : "Updated", taskTitle, projectPublicID);
                    MailMessenger mailMessage = new MailMessenger(context);
                    mailMessage.SendMail(string.Join(",", emails.ToArray()), subject, "", taskEmail.ToString(), true, new string[] { }, true);
                }
            }
            #endregion

            //When task completed then run moduledependency starttask to start tasks which are dependent on this task
            if (ModuleName == ModuleNames.SVC && moduleInstDepny != null)
            {
                if (taskCompleted)
                {
                    TaskManager.StartTasks(moduleInstDepny.TicketId, UGITUtility.ObjectToString(moduleInstDepny.ID), true);
                    TaskManager.MoveSVCTicket(moduleInstDepny.TicketId);
                }
                else
                {
                    DataRow svcTicket = Ticket.GetCurrentTicket(context, ModuleNames.SVC, moduleInstDepny.ParentInstance);
                    if (svcTicket != null)
                        TaskManager.UpdateSVCTicket(svcTicket, true);
                }
            }

            // Test
            if (ModuleName == ModuleNames.SVC && moduleInstDepny != null && moduleInstDepny.SubTaskType.ToLower() == "task")
            {
                // Check if SVC Instance needed to be put on Hold/UnHold
                if (oldStatus != status)
                    TaskManager.DoHoldUnHoldSVCInstance(context, moduleInstDepny.TicketId);

                // Initiate Updating the records for current task in ScheduleActions List
                ThreadStart starter = delegate { InitiateScheduleActionsInBackground(moduleInstDepny.ID, UGITUtility.StringToBoolean(UGITUtility.GetSPItemValueAsString(project, DatabaseObjects.Columns.EnableTaskReminder)), service); };
                Thread thread = new Thread(starter);
                thread.IsBackground = true;
                thread.Start();
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        #endregion

        protected void BtSaveTask_Click(object sender, EventArgs e)
        {
            string userLoginName = string.Empty;
            bool returnBit = false;
            dtcDateError.Visible = false;
            dtcRecurrEndDateError.Visible = false;
            if (RepeatableTask && dtcStartDate.Date == DateTime.MinValue)
            {
                dtcDateError.Text = "Please enter the Start Date. ";
                dtcDateError.Visible = true;
                returnBit = true;
            }
            if (RepeatableTask && dtcRecurrEndDate.Date == DateTime.MinValue)
            {
                dtcRecurrEndDateError.Text = "Please enter the Recurring End Date.";
                dtcRecurrEndDateError.Visible = true;
                returnBit = true;
            }

            if (RepeatableTask && chkRepeatEnable.Checked && (recurringcount.Value == string.Empty || dtcRecurrEndDate.Date == DateTime.MinValue))
                returnBit = true;

            if (returnBit)
                return;
            if (!cvUser.IsValid)
                return;

            string strAssignToPct = string.Empty;
            //new variable to get total percentage
            double dblAssignPercentage = 0;

            Control btAction = (Control)sender;
            bool isError = false;
            // Validate dates, but not for SVC since start date is not visible in SVC

            if (dtcStartDate.Date > dtcDueDate.Date)
            {
                if (ModuleName == "SVC")
                    dtcStartDate.Date = dtcDueDate.Date;
                else
                {
                    isError = true;
                    dtcDateError.Text = "Due Date cannot be before Start Date";
                    dtcDateError.Visible = true;
                }
            }
            if (RepeatableTask && dtcStartDate.Date > dtcRecurrEndDate.Date)
            {
                isError = true;
                dtcRecurrEndDateError.Text = "Recurring End Date cannot be before Start Date";
                dtcRecurrEndDateError.Visible = true;                
            }

            
            if (isError || !Page.IsValid)
                return;

            //new object to get the existing users.
            List<string> existingUsers = new List<string>();

            //Creates uservaluecollection object from peoplepicker control
            List<string> involvedUsers = new List<string>();
            string assignedUser = string.Empty;
            List<string> assignedUsers = new List<string>();
            List<string> userMultiLookup = new List<string>();

            if (ModuleName == "PMM" || ModuleName == "TSK" || ModuleName == "NPR" || TaskManager.IsModuleTasks(ModuleName))
            {
                foreach (RepeaterItem item in rAssignToPct.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var peopleEditor = (UserValueBox)item.FindControl("peAssignedToPct_" + item.ItemIndex);
                        TextBox textboxPct = (TextBox)item.FindControl("txtPercentage");

                        if (peopleEditor.GetValuesAsList() != null && peopleEditor.GetValuesAsList().Count > 0)
                        {

                            UserProfile user = UserManager.GetUserById(peopleEditor.GetValuesAsList()[0]);

                            if (user != null)
                            {
                                string strPercentage = textboxPct.Text;

                                if (string.IsNullOrEmpty(strPercentage))
                                    strPercentage = "100";

                                if (!string.IsNullOrEmpty(strAssignToPct))
                                    strAssignToPct += Constants.Separator;

                                //strAssignToPct += user.UserName + Constants.Separator1 + strPercentage;
                                strAssignToPct += user.Id + Constants.Separator1 + strPercentage;
                                dblAssignPercentage += Convert.ToDouble(strPercentage);

                                userMultiLookup.Add(user.Id);
                                involvedUsers.Add(user.Id);
                            }
                        }
                    }
                }

                if (parentTaskID > 0 && taskType == "mytask")
                {
                    UserProfile user = UserManager.GetUserById(peAssignedTo.GetValuesAsList()[0]);
                    if (user != null)
                    {
                        userMultiLookup.Add(user.Id);
                        involvedUsers.Add(user.Id);
                        strAssignToPct = user.Id + Constants.Separator1 + "100";
                    }
                }
            }
            else
            {
                userMultiLookup = peAssignedTo.GetValuesAsList();

                if (userMultiLookup != null && userMultiLookup.Count > 0)
                    involvedUsers = userMultiLookup;
            }

            // Create a list of predecessors
            List<string> predecessors = new List<string>();
            string predecessorIDs = string.Empty;

            foreach (var item in tpControl.SelectedPredecessorsId)
            {
                string preLookup = item;
                predecessors.Add(preLookup);
            }

            #region Task
            if (ModuleName == "PMM" || ModuleName == "NPR" || ModuleName == "TSK" || TaskManager.IsModuleTasks(ModuleName))
            {
                int numberOfTaskChanges = 0;
                bool isNewTask = false;
                ptasks = UGITTaskManager.MapRelationalObjects(ptasks);

                UGITTask task = null;
                if (!isDuplicate && taskID > 0)
                {
                    task = ptasks.FirstOrDefault(x => x.ID == taskID);

                    if (task.ChildCount > 0)
                    {
                        if (dtcStartDate.Date.ToString("MM/dd/yyyy") != hdnStartDate.Value)
                        {
                            dtcStartDate.Date = Convert.ToDateTime(hdnStartDate.Value);
                        }

                        if (dtcDueDate.Date.ToString("MM/dd/yyyy") != hdnDueDate.Value)
                        {
                            dtcDueDate.Date = Convert.ToDateTime(hdnDueDate.Value);
                        }
                    }

                    if (task.AssignedTo != null)
                        existingUsers = new List<string>(UGITUtility.SplitString(task.AssignedTo, Constants.Separator6));
                }
                else
                {
                    isNewTask = true;
                    task = new UGITTask(ModuleName);
                    task.TicketId = projectPublicID;
                    task.ParentTaskID = 0;
                    task.ParentTask = null;
                    task.Level = 0;
                    task.ItemOrder = ptasks.Count + 1;
                }

                // Assign the skill value
                //task.Skills = multiLookups;
                task.UserSkillMultiLookup = cbSkill.GetValues();

                //new condition for apply resource assignments to child task(s).
                if (task.AssignToPct == null)
                    task.AssignToPct = string.Empty;

                if (task.ChildCount > 0 && task.AssignToPct != strAssignToPct && (btAction.ID != "btnOK" && btAction.ID != "btnNo"))
                {
                    pcAssignTo.ShowOnPageLoad = true;
                    return;
                }

                // Get estimated hours from text field if not connected to sprint AND visible
                double estimatedHours = task.EstimatedHours;

                if (task.SprintLookup == 0 && txtEstimatedHours.Visible)
                {
                    if (string.IsNullOrEmpty(txtEstimatedHours.Text))
                    {
                        if (dblAssignPercentage > 0)
                            task.EstimatedHours = Math.Round(((dblAssignPercentage * (uHelper.GetTotalWorkingDaysBetween(context, dtcStartDate.Date, dtcDueDate.Date) * 8)) / 100), 2);
                        else
                            task.EstimatedHours = uHelper.GetTotalWorkingDaysBetween(context, dtcStartDate.Date, dtcDueDate.Date) * 8;
                    }
                    else
                    {
                        double.TryParse(txtEstimatedHours.Text.Trim(), out estimatedHours);
                        task.EstimatedHours = estimatedHours;
                    }
                }

                // Get actual hours from text field if not connected to sprint AND visible
                double actualHours = task.ActualHours;

                if (task.SprintLookup == 0 && txtActualHours.Visible)
                {
                    double.TryParse(txtActualHours.Text.Trim(), out actualHours);
                    task.ActualHours = actualHours;
                }

                // If ERH field visible, get value from there
                if (task.SprintLookup == 0 && txtEstimatedRemainingHours.Visible)
                {
                    double estimatedRemainingHours = 0;
                    double.TryParse(txtEstimatedRemainingHours.Text.Trim(), out estimatedRemainingHours);
                    task.EstimatedRemainingHours = estimatedRemainingHours;
                }
                else
                {
                    // Else (for new tasks) calculate it
                    task.EstimatedRemainingHours = estimatedHours - actualHours;
                }

                task.IsMileStone = false;
                task.StageStep = 0;
                if (UGITUtility.StringToInt(ddlProjectStages.SelectedValue) > 0)
                {
                    task.StageStep = UGITUtility.StringToInt(ddlProjectStages.SelectedValue);
                }
                if (task.ParentTaskID == 0 && ddlProjectStages.SelectedValue != string.Empty)
                {
                    task.IsMileStone = cbMilestone.Checked;
                    int selectedStage = 0;
                    if (task.IsMileStone && int.TryParse(ddlProjectStages.SelectedValue, out selectedStage))
                    {
                        task.StageStep = selectedStage;
                        List<UGITTask> sameStageTasks = ptasks.Where(x => task.ID != x.ID && x.StageStep == selectedStage).ToList();
                        foreach (UGITTask sTask in sameStageTasks)
                        {
                            sTask.StageStep = 0;
                            sTask.IsMileStone = false;
                            sTask.Changes = true;
                        }
                    }
                }
                if (cbSprints.Checked == true)
                {
                    // Need to be implemented
                    long lookup = UGITUtility.StringToLong(ddlSprints.SelectedValue);
                    if (lookup > 0)
                    {
                        List<UGITTask> sameSprintTasks = ptasks.Where(c => task.ID != c.ID && c.SprintLookup == lookup).ToList();
                        foreach (UGITTask sTask in sameSprintTasks)
                        {
                            sTask.SprintLookup = 0;
                            sTask.Changes = true;
                        }
                        task.SprintLookup = lookup;
                    }
                }
                else
                {
                    task.SprintLookup = 0;
                }

                task.Title = txtTitle.Text.Trim();
                task.Description = txtDescription.Text.Trim();

                task.Status = ddlStatus.SelectedValue;
                double pctComplete = 0;
                double.TryParse(txtPctComplete.Text.Trim(), out pctComplete);
                task.PercentComplete = pctComplete;

                if (task.Status.ToLower() == Constants.Completed.ToLower())
                    task.PercentComplete = 100;

                if (dtcStartDate.Date != DateTime.MinValue)
                    task.StartDate = dtcStartDate.Date;

                if (dtcDueDate.Date != DateTime.MinValue)
                    task.DueDate = dtcDueDate.Date;

                if (dtcStartDate.Date == DateTime.MinValue || dtcDueDate.Date == DateTime.MinValue)
                    task.StartDate = task.DueDate = DateTime.MinValue;

                if (userMultiLookup != null && userMultiLookup.Count > 0)
                    task.AssignedTo = string.Join(Constants.Separator6, userMultiLookup);

                predecessors = TaskManager.FilterPredecessors(ptasks, predecessors, task);

                //if (task.SprintLookup != null)
                //    task.Predecessors = null;
                //else
                //    task.Predecessors = predecessors;
                // Need to test below code for predecessors as it was found only in .NET code
                UGITTask taskPre = null;
                bool tasksuccessorFlag = false;
                bool TaskWithSamePre = false;

                if (predecessors.Count > 0)
                {
                    foreach (var item in predecessors)
                    {
                        taskPre = ptasks.FirstOrDefault(x => x.ItemOrder == Convert.ToInt64(item));

                        if (!string.IsNullOrEmpty(taskPre.Predecessors))
                        {
                            foreach (var itemTask in taskPre.Predecessors.Split(','))
                            {

                                if (!string.IsNullOrEmpty(taskPre.Predecessors) && task.ItemOrder == Convert.ToInt64(itemTask))
                                {
                                    tasksuccessorFlag = true;
                                    break;
                                }
                                else if (!string.IsNullOrEmpty(itemTask) && task.ItemOrder == Convert.ToInt64(itemTask))
                                {
                                    TaskWithSamePre = true;
                                    break;
                                }
                            }
                        }

                    }

                }
                if (tasksuccessorFlag)
                {
                    lblPredecessorError.Text = $"Predecessor can not be successor to each other. Please select another task as pre-decessor. {taskPre.ItemOrder}-{taskPre.Title}";
                    lblPredecessorError.Visible = true;
                    return;
                }
                else if (TaskWithSamePre)
                {
                    lblPredecessorError.Text = $"Can not assign same task as predecessor= {taskPre.ItemOrder}-{taskPre.Title}";
                    lblPredecessorError.Visible = true;
                    return;
                }
                else
                {
                    task.Predecessors = string.Join(Constants.Separator6, predecessors);
                }

                if (rblTaskBehaviour.SelectedValue != "")
                    task.Behaviour = rblTaskBehaviour.SelectedValue;
                else
                    task.Behaviour = "Task";

                task.Comment = txtComment.Text.Trim();
                task.Changes = true;

                if (TaskManager.IsModuleTasks(ModuleName))
                {
                    task.TicketId = projectPublicID;
                }
                else if (!string.IsNullOrEmpty(task.Behaviour) && task.Behaviour.ToLower() == "ticket")
                {
                    task.RelatedTicketID = UGITUtility.SplitString(hdnTicketId.Value.Trim(), ":", 0);
                    task.Title = hdnTicketId.Value.Trim();
                }
                else
                {
                    task.RelatedTicketID = string.Empty;
                }

                task.ShowOnProjectCalendar = chkShowonProjectCalendar.Checked;

                if (task.AttachedFiles != null && task.AttachedFiles.Count > 0)
                {
                    List<string> deleteFiles = UGITUtility.ConvertStringToList(txtDeleteFiles.Text, new string[] { "~" });
                    foreach (string dFile in deleteFiles)
                    {
                        if (task.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == dFile.ToLower()).Value != null)
                        {
                            task.AttachedFiles.Remove(dFile);
                        }
                    }
                }

                task.AttachFiles = GetAttachedFiles();
                task.LinkedDocuments = hdnDocIds.Value;
                task.IsCritical = chkCritical.Checked;
                //var linkedfiles=UploadAndLinkDocuments.FindControl("fileAttchmentId");
                //check previous save files
                //if(task.Attachments)
                if(UploadAndLinkDocuments.Visible == true)
                { 
                    var currentLink = Convert.ToString(((HiddenField)UploadAndLinkDocuments.FindControl("fileAttchmentId")).Value);

                    if (!string.IsNullOrEmpty(task.Attachments) && !string.IsNullOrEmpty(currentLink))
                    {
                        var attachment = task.Attachments + "," + string.Join(",", currentLink);
                        task.Attachments = attachment;
                    }
                    else
                        task.Attachments = Convert.ToString(((HiddenField)UploadAndLinkDocuments.FindControl("fileAttchmentId")).Value);
                }
                else if (!string.IsNullOrEmpty(taskFileUploadControl.GetValue()))
                {
                    task.Attachments = taskFileUploadControl.GetValue();
                }

                if (chkTaskReminderEnable.Checked)
                {
                    task.ReminderEnabled = true;

                    if (ddlTimeInterval.SelectedValue == "Days")
                        task.ReminderDays = Convert.ToInt32(ddlIntervalType.SelectedValue + ddlCount.SelectedValue);
                    else
                        task.ReminderDays = Convert.ToInt32(ddlIntervalType.SelectedValue + ddlCount.SelectedValue) * 7;

                    if (ddlInetrvalInWeeknDays.SelectedValue == "Days")
                        task.RepeatInterval = Convert.ToInt32(ddlRepeatCount.SelectedValue);
                    else
                        task.RepeatInterval = Convert.ToInt32(ddlRepeatCount.SelectedValue) * 7;
                }
                else
                {
                    task.ReminderEnabled = false;
                }

                // new lines for calulate the task date when resource % allocate changes.....
                if (UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.AutoAdjustAllocations) && chAutoAdjustAllocation.Checked && taskID > 0 && task.AssignToPct != strAssignToPct && !string.IsNullOrEmpty(task.AssignToPct) && !string.IsNullOrEmpty(strAssignToPct))
                {
                    double factor = 0;
                    factor = task.EstimatedHours;
                    if (factor == 0)
                    {
                        int workingDays = uHelper.GetTotalWorkingDaysBetween(context, task.StartDate, task.DueDate);
                        factor = workingDays * 8;
                    }
                    double calculateHours = 0;
                    double tempvalue = 0;
                    List<UGITAssignTo> listAssignToPct = TaskManager.GetUGITAssignPct(strAssignToPct);
                    List<UGITAssignTo> listAssignToPctTimeFactor = TaskManager.GetUGITAssignPct(task.AssignToPct);
                    double totalPercentage = listAssignToPctTimeFactor.Sum(x => Convert.ToDouble(x.Percentage));
                    double timefactor = (totalPercentage * factor) / 100;

                    foreach (UGITAssignTo item in listAssignToPct)
                    {
                        double tempvalue1 = (Convert.ToDouble(item.Percentage)) / (100 * timefactor);
                        tempvalue += tempvalue1;

                    }
                    calculateHours = Math.Round((1 / tempvalue), 2);
                    int calculateDays = Convert.ToInt32(calculateHours / 8);
                    DateTime[] Taskdates = uHelper.GetEndDateByWorkingDays(context, task.StartDate, calculateDays);
                    task.DueDate = Taskdates[1];
                }

                if (trAssignedTo.Visible == true && tdAutoAdjustAllocation.Visible == true)
                {
                    if (chAutoAdjustAllocation.Checked)
                        project[DatabaseObjects.Columns.AutoAdjustAllocations] = true;
                    else
                        project[DatabaseObjects.Columns.AutoAdjustAllocations] = false;
                }

                //new line of code for rmm...
                task.AssignToPct = strAssignToPct;

                if (btAction.ID == "btnOK" && userMultiLookup != null && userMultiLookup.Count > 0)
                {
                    foreach (UGITTask taskItem in task.ChildTasks)
                    {
                        taskItem.AssignedTo = string.Join(Constants.Separator6, userMultiLookup);
                        taskItem.AssignToPct = strAssignToPct;
                        taskItem.Changes = true;
                    }
                }

                if (taskType == "mytask")
                {
                    DataRow currentTicket = Ticket.GetCurrentTicket(context, ModuleName, projectPublicID);
                    List<UserProfile> assignTo = UserManager.GetUserInfosById(userMultiLookup.ToString());

                    if (!UserManager.IsActionUser(currentTicket, User))
                    {
                        task.ProposedStatus = UGITTaskProposalStatus.Pending_AssignTo;
                        List<UserProfile> users = UserManager.GetUserInfosById(userMultiLookup.ToString());
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
                            taskToEmail.Add(DatabaseObjects.Columns.Title, task.Title);
                            taskToEmail.Add(DatabaseObjects.Columns.LinkDescription, task.Description);
                            taskToEmail.Add(DatabaseObjects.Columns.StartDate, task.StartDate.ToString());
                            taskToEmail.Add(DatabaseObjects.Columns.DueDate, task.DueDate.ToString());
                            taskToEmail.Add(DatabaseObjects.Columns.EstimatedHours, task.EstimatedHours.ToString());

                            string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", projectPublicID, task.ID, ModuleName);
                            string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, context.TenantID);
                            string subject = string.Format("{0} {1}: {2}", projectPublicID, task.Status != Constants.Completed ? "New Task Assigned" : "Task Completed", task.Title);
                            StringBuilder taskEmail = new StringBuilder();
                            string greeting = ConfigVariableHelper.GetValue("Greeting");
                            string signature = ConfigVariableHelper.GetValue("Signature");

                            taskEmail.AppendFormat("{0} {1}<br /><br />", greeting, mailToNames.ToString());

                            if (task.Status != Constants.Completed)
                                taskEmail.AppendFormat("A new task <b>\"{0}\"</b> has been assigned to you by {1}.<br>", task.Title, User.Name);
                            else
                                taskEmail.AppendFormat("Task <b>\"{0}\"</b> has been completed by {1}.<br>", task.Title, User.Name);

                            taskEmail.Append("<br /><br />" + signature + "<br />");
                            taskEmail.Append(emailFooter);

                            MailMessenger mailMessage = new MailMessenger(context);
                            if (!string.IsNullOrWhiteSpace(ModuleName) && ModuleName == "SVC")
                                mailMessage.SendMail(string.Join(",", emails.ToArray()), subject, "", taskEmail.ToString(), true, new string[] { }, true, saveToTicketId: moduleInstDepny.ParentInstance); // Pass ticketID to save email
                            else
                                mailMessage.SendMail(string.Join(",", emails.ToArray()), subject, "", taskEmail.ToString(), true, new string[] { }, true);
                        }
                    }
                }

                //new line of code for task with sprint.
                if (cbSprints.Checked)
                {
                    // Need to be implemented
                    long lookup = UGITUtility.StringToLong(ddlSprints.SelectedValue);

                    if (lookup > 0)
                    {
                        SprintManager sprintManager = new SprintManager(context);
                        Sprint selectedData = sprintManager.LoadByID(lookup);
                        if (selectedData != null)
                        {
                            TaskManager.UpdateTaskToSprintItem(selectedData, task);
                        }
                    }
                }
                bool stageMoved = false;

                // RepeatTask Implementation.
                DateTime RepeatendDate = task.StartDate;
                string[] weekends = UGITUtility.SplitString(ConfigVariableHelper.GetValue(ConfigConstants.WeekendDays), ",");
                double estimateHours = 0;
                DateTime RecurringTaskStartDate = task.StartDate;
                DateTime startDate = RecurringTaskStartDate;

                if (RepeatableTask)
                {
                    RepeatendDate = dtcRecurrEndDate.Date;
                    estimateHours = task.EstimatedHours;
                }

                #region Repeatable Task
                int i = 0;
                do
                {
                    if (isNewTask)
                    {
                        if (i > 0)
                        {
                            if (!string.IsNullOrEmpty(recurringcount.Value))
                            {
                                int number = Convert.ToInt32(recurringcount.Value);
                                string RepeatInterval = ddlRecurringInterval.SelectedValue;

                                if (RepeatInterval == "Weeks")
                                {
                                    startDate = RecurringTaskStartDate.AddDays(7 * number * i);
                                }
                                else if (RepeatInterval == "Months")
                                {
                                    startDate = RecurringTaskStartDate.AddMonths(number * i);
                                }
                                else if (RepeatInterval == "Years")
                                {
                                    startDate = RecurringTaskStartDate.AddYears(number * i);
                                }
                                else
                                {
                                    startDate = startDate.AddDays(number);
                                }

                                if (startDate > RepeatendDate)
                                {
                                    break;
                                }
                            }

                        }

                        DateTime[] startnDueDate = uHelper.GetNewEndDateForExistingDuration(context, dtcStartDate.Date, dtcDueDate.Date, startDate, false);

                        task.StartDate = startDate = startnDueDate[0];
                        task.DueDate = startnDueDate[1];
                        task.ID = 0;
                        task.ParentTaskID = 0;
                        task.ParentTask = null;
                        task.Level = 0;
                        task.ItemOrder = ptasks.Count + 1;

                        TaskManager.SaveTask(ref task, ModuleName, projectPublicID);
                        task.TenantID = TenantID;
                        ptasks.Add(task);
                        ptasks = UGITTaskManager.MapRelationalObjects(ptasks);

                        if (parentTaskID > 0)
                        {
                            UGITTask parentTask = ptasks.FirstOrDefault(x => x.ID == parentTaskID);

                            //New task will be the subling of selected task. 
                            long parentID = 0;

                            if (parentTask != null && parentTask.ParentTask != null)
                                parentID = parentTask.ParentTask.ID;

                            if (parentTask != null)
                                TaskManager.PlacedTaskAt(task, parentTask, ref ptasks);

                            TaskManager.AddChildTask(parentID, task.ID, ref ptasks);
                        }

                        // New line of code for child task in case of sub task.
                        if (Request["SubTask"] == "true" && !string.IsNullOrEmpty(Request["parentTaskID"]))
                        {
                            UGITTask parentTask = ptasks.FirstOrDefault(x => x.ID == Convert.ToInt32(Request["parentTaskID"]));
                            TaskManager.AddChildTask(parentTask.ID, task.ID, ref ptasks);
                        }

                        TaskManager.ReOrderTasks(ref ptasks);
                    }

                    // Propagates date and/or status changes to child and parent tasks
                    TaskManager.PropagateTaskEffect(context, ref ptasks, task, true, true);

                    // Need to check below 3 lines of code, copied from existing .NEt code
                    // Calculate Start end if Summary Task Date Change.
                    //if (task.ChildCount > 0)
                    //{
                    //    TaskManager.AutoCalculateStartEndDateOfChild(ptasks, task);
                    //}

                    // Calculates project's startdate, enddate, pctcomplete, duration,  DaysToComplete, nextactivity and nextmilestone.
                    TaskManager.CalculateProjectStartEndDate(ModuleName, ptasks, project);

                    if (project != null)
                    {
                        project.AcceptChanges();
                        project.SetModified(); //Duplicate ticket is created when task under schedule tab is saved, in SVC.
                        ticketRequest.CommitChanges(project);
                    }
                    //project.UpdateOverwriteVersion();

                    // Gets count of tasks which are effected in this action
                    numberOfTaskChanges = ptasks.Where(x => x.Changes).Count();
                    TaskManager.SaveTasks(ref ptasks, ModuleName, projectPublicID);

                    // Notify assigned used about the task but only when use click on Save & notify Button
                    if (btAction.ID == "btSaveAndNotify" && userMultiLookup != null && userMultiLookup.Count > 0)
                    {
                        List<UserProfile> users = UserManager.GetUserInfosById(userMultiLookup.ToString());
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

                        //Send mail once for recurring task as well.
                        //value of i would be increased in recurring task that's why sending mail in first iteration in case of recurring
                        if (emails.Count > 0 && (!RepeatableTask || i == 0))
                        {
                            Dictionary<string, string> taskToEmail = new Dictionary<string, string>();
                            taskToEmail.Add(DatabaseObjects.Columns.ProjectID, projectPublicID);
                            taskToEmail.Add(DatabaseObjects.Columns.ProjectTitle, Convert.ToString(project[DatabaseObjects.Columns.Title]));
                            taskToEmail.Add(DatabaseObjects.Columns.Title, task.Title);
                            taskToEmail.Add(DatabaseObjects.Columns.LinkDescription, task.Description);
                            taskToEmail.Add(DatabaseObjects.Columns.StartDate, task.StartDate.ToString());
                            taskToEmail.Add(DatabaseObjects.Columns.DueDate, task.DueDate.ToString());
                            taskToEmail.Add(DatabaseObjects.Columns.EstimatedHours, task.EstimatedHours.ToString());

                            string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", projectPublicID, task.ID, ModuleName);
                            string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false,context.TenantID);

                            StringBuilder taskEmail = new StringBuilder();
                            string greeting = ConfigVariableHelper.GetValue("Greeting");
                            string signature = ConfigVariableHelper.GetValue("Signature");

                            taskEmail.AppendFormat("{0} {1}<br /><br /> ", greeting, mailToNames.ToString());

                            string subjectPrefix = "New Task Assigned";
                            if (task.Status != Constants.Completed)
                            {
                                if (RepeatableTask)
                                {
                                    taskEmail.AppendFormat("A new recurring task <b>\"{0}\"</b> has been assigned to you by {1}.<br>", task.Title, User.Name);
                                    subjectPrefix = "New Recurring Task Assigned";
                                }
                                else
                                    taskEmail.AppendFormat("A new task <b>\"{0}\"</b> has been assigned to you by {1}.<br>", task.Title, User.Name);
                            }
                            else
                            {
                                subjectPrefix = "Task Completed";
                                taskEmail.AppendFormat("Task <b>\"{0}\"</b> has been completed by {1}.<br>", task.Title, User.Name);
                            }

                            taskEmail.Append("<br /><br />" + signature + "<br />");
                            taskEmail.Append(emailFooter);

                            string emailSubject = string.Format("{0} {1}: {2}", projectPublicID, subjectPrefix, task.Title);
                            string attachUrl = string.Empty;

                            if (UGITUtility.StringToBoolean(ConfigVariableHelper.GetValue(ConfigConstants.AttachTaskCalendarEntry)))
                            {
                                //Location where you want to save the vCalendar file
                                attachUrl = uHelper.GetUploadFolderPath() + @"\" + task.Title + ".vcs";

                                //Create task
                                using (StreamWriter sw = new StreamWriter(attachUrl))
                                {
                                    sw.Write(CreateTask(task.StartDate, task.DueDate, emailSubject, taskEmail.ToString()));
                                }
                            }
                            MailMessenger mailMessage = new MailMessenger(context);
                            mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl });
                        }
                    }

                    // NOT NEEDED - causing unnecessary baseline creation!
                    //Create baseline if user changing status of more than two task.
                    //if (moduleName == "PMM" && numberOfTaskChanges > 2 && (ddlStatus.SelectedValue.ToLower() == Constants.Completed.ToLower() || pctComplete >= 100) && hdnActionType.Value == "BaselineAndSave" && baselineTaskThreshold > 0)
                    //{
                    //    hdnActionType.Value = string.Empty;
                    //    PMMBaseline baseline = new PMMBaseline(projectID, DateTime.Now);
                    //    baseline.BaselineComment = string.Format("Changed status of {0} tasks as Completed", childTaskCount);
                    //    baseline.CreateBaseline(project);
                    //}

                    // Move PMM stage if a linked task has changed status
                    if (ModuleName == "PMM")
                    {
                        LifeCycle lifeCycle = ticketRequest.GetTicketLifeCycle(project);
                        if (lifeCycle.ID != 0)
                        {
                            List<string> messages = new List<string>();
                            stageMoved = ticketRequest.ProjectStageMove(project, ptasks, true, ref messages);
                        }
                    }

                    i = i + 1;

                } while (RepeatableTask);

                #endregion

                // TaskCache isn't implemented yet
                // NOTE: This reload must be AFTER the above stage move block in case project goes from open to closed or vice-versa
                // TaskCache.ReloadProjectTasks(ModuleName, projectPublicID);

                // Update resource utilization pct of resource
                if (ModuleName == "PMM" || ModuleName == "TSK" || ModuleName == "NPR" || TaskManager.IsModuleTasks(ModuleName))
                {
                    autoCreateRMMProjectAllocation = ConfigVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);

                    if (autoCreateRMMProjectAllocation)
                    {
                        string webUrl = HttpContext.Current.Request.Url.ToString();

                        List<string> userListToUpdateAllocations = new List<string>();
                        userListToUpdateAllocations.AddRange(involvedUsers);
                        userListToUpdateAllocations.AddRange(existingUsers);

                        ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate () { allocationManager.UpdateProjectPlannedAllocation(webUrl, ptasks, userListToUpdateAllocations, ModuleName, projectPublicID, true); };
                        Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                        sThreadUpdateProjectPlannedAllocation.IsBackground = true;
                        sThreadUpdateProjectPlannedAllocation.Start();
                    }
                }

                // Adding task info to cache so that we can focus on this task in task grid when popup is closed
                AddTaskInfoToContextCache();

                // checks to refresh frame or whole parent window.
                if (ModuleName == "PMM" && stageMoved)
                {
                    string cookieValue = UGITUtility.GetCookieValue(Request, ModuleName + "-TicketSelectedTabConst");

                    if (!string.IsNullOrEmpty(cookieValue))
                    {
                        UGITUtility.CreateCookie(Response, ModuleName + "-TicketSelectedTab", cookieValue);
                        UGITUtility.DeleteCookie(Request, Response, ModuleName + "-TicketSelectedTabConst");
                    }

                    uHelper.ClosePopUpAndEndResponse(Context, true, "/refreshpage/");
                }
                else
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
            #endregion

            #region SVCConfig
            else if (ModuleName == "SVCConfig")
            {
                bool isNewTask = false;
                UGITTask task = null;
                if (taskID > 0)
                {
                    task = service.Tasks.FirstOrDefault(x => x.ID == taskID);
                }
                else
                {
                    task = new UGITTask();
                    //Added by mudassir 28 feb 2020 commented
                    task.ModuleNameLookup = ModuleName;
                    service.Tasks.Add(task);
                    isNewTask = true;
                }

                task.Title = txtTitle.Text.Trim();
                task.Description = txtDescription.Text.Trim();

                // Need to be implemented
                if (trDisableNotification.Visible)
                    task.NotificationDisabled = chkDisableNotification.Checked;


                if (!string.IsNullOrWhiteSpace(ddlModuleDetail.SelectedValue))
                    task.RelatedModule = ddlModuleDetail.SelectedValue;

                // Need to be implemented
                int requestTypeID = 0;
                int.TryParse(ddlTicketRequestType.SelectedValue, out requestTypeID);
                task.RequestTypeCategory = string.Empty;
                if (requestTypeID > 0)
                    task.RequestTypeCategory = ddlTicketRequestType.SelectedValue;


                //if (dropDownBox.GetValues() != null)
                //{
                //    task.RequestTypeCategory = dropDownBox.GetValues();

                //}

                task.Predecessors = string.Join(Constants.Separator6, predecessors);

                // Need to be checked for projectID and fixed if needed

                task.AssignedTo = string.Join(Constants.Separator6, userMultiLookup);
                task.SLADisabled = chkDisableSLA.Checked;
                if (trDisableNotification.Visible)
                    task.NotificationDisabled = chkDisableNotification.Checked;
                if (!service.CreateParentServiceRequest)
                    task.SLADisabled = false;

                if (isNewTask)
                {
                    task.ItemOrder = service.Tasks.Max(x => x.ItemOrder) + 1;
                    task.ParentTaskID = 0;
                    task.ParentTask = null;
                }

                if (parentTaskID > 0)
                {
                    UGITTask sTask = service.Tasks.FirstOrDefault(x => x.ID == parentTaskID);
                    task.ParentTask = sTask.ParentTask;
                    task.Level = sTask.Level;
                    task.ItemOrder = sTask.ItemOrder;
                }

                int weight = 1;
                int.TryParse(txtWeight.Text.Trim(), out weight);
                task.Weight = weight;

                double estimatedHours = 0;
                double.TryParse(txtEstimatedHours.Text.Trim(), out estimatedHours);
                task.EstimatedHours = estimatedHours;

                if (task.SubTaskType != null && task.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                {
                    // If this was an account task and is not anymore then 
                    // delete the mapping that was created to save to UGITNewUserName
                    ServiceQuestionMappingManager quesMappingMngr = new ServiceQuestionMappingManager(context);
                    List<ServiceQuestionMapping> taskQuesMapping = quesMappingMngr.GetByTaskID(task.ID);

                    if (taskQuesMapping != null && taskQuesMapping.Count > 0)
                    {
                        ServiceQuestionMapping newUserNameMapping = taskQuesMapping.FirstOrDefault(x => x.ColumnName.ToLower() == "ugitnewusername");
                        if (newUserNameMapping != null)
                        {
                            List<ServiceQuestionMapping> lstnewUserMapping = new List<ServiceQuestionMapping>();
                            lstnewUserMapping.Add(newUserNameMapping);
                            quesMappingMngr.Delete(lstnewUserMapping);
                        }
                    }
                }

                task.SubTaskType = ServiceSubTaskType.Task;

                if (ddlTypes.SelectedIndex == 1)
                    task.SubTaskType = ServiceSubTaskType.AccountTask;
                else if (ddlTypes.SelectedIndex == 2)
                    task.SubTaskType = ServiceSubTaskType.AccessTask;

                // Need to implement QuestionProperties
                task.QuestionID = ddlApplicationAccess.SelectedValue;
                task.QuestionProperties = Convert.ToString(GridLookupApplicationQuestion.Value);
                bool applicationTaskAlreadyExist = false;

                if (!string.IsNullOrEmpty(task.QuestionID) && !string.IsNullOrEmpty(task.QuestionProperties))
                    applicationTaskAlreadyExist = service.Tasks.Exists(x => x.QuestionID == task.QuestionID && x.QuestionProperties == task.QuestionProperties && x.ID != 0 && x.ID != task.ID);

                task.AutoCreateUser = chkAutoUserCreation.Checked;
                task.AutoFillRequestor = chkAutoFillRequestor.Checked;
                task.EnableApproval = chkApprovalRequired.Checked;
                task.Approver = peApprover.GetValues();

                if (!applicationTaskAlreadyExist)
                {
                    TaskManager.SaveTask(ref task, ModuleName, projectPublicID);
                    List<UGITTask> orderedTasks = OrderServiceTasks(service.Tasks);
                    TaskManager.SaveTasks(ref orderedTasks, ModuleName, projectPublicID);
                    Context.Cache.Add(string.Format("SVCConfigTask-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

                    string editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TaskList");
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Task for same Question and Application already exists!!');", true);
                }
            }

            #endregion

            #region  SVC Request
            else if (ModuleName == "SVC")
            {
                string oldStatus = Constants.NotStarted;
                double oldPctComplete = 0;
                string status = (moduleInstDepny != null && moduleInstDepny.OnHold) ? Constants.OnHoldStatus : ddlStatus.SelectedValue;
                int pctComplete = 0;
                bool? oldSLADisabled = null;
                if (moduleInstDepny != null)
                    oldSLADisabled = moduleInstDepny.SLADisabled;

                if (moduleInstDepny == null && taskType == "ticket" && ddlTickets.Items.Count <= 0)
                {
                    lbTicketMessage.Text = "Please map ticket";
                    lbTicketMessage.Visible = true;
                    return;
                }

                Ticket tReq = new Ticket(context, "SVC");
                LifeCycleStage ticketCurrentStage = tReq.GetTicketCurrentStage(project);

                bool startDependency = false;
                bool newTask = false;

                if (moduleInstDepny == null)
                {
                    newTask = true;
                    startDependency = true;
                    moduleInstDepny = new UGITTask();
                    moduleInstDepny.ParentInstance = projectPublicID;
                    moduleInstDepny.ModuleNameLookup = ModuleName;
                    moduleInstDepny.TicketId = projectPublicID;

                    if (taskType == "ticket")
                    {
                        moduleInstDepny.ModuleNameLookup = ddlModuleDetail.SelectedValue;
                        moduleInstDepny.ChildInstance = ddlTickets.SelectedValue;
                    }
                    if (taskType == "task")
                    {
                        moduleInstDepny.Behaviour = "Task";
                    }
                    moduleInstDepny.ItemOrder = 1;

                    if (ptasks != null && ptasks.Count > 0)
                        moduleInstDepny.ItemOrder = ptasks.Max(x => x.ItemOrder) + 1;
                }
                else
                {
                    oldStatus = moduleInstDepny.Status;
                    oldPctComplete = moduleInstDepny.PercentComplete;
                }

                bool isPredecessorChange = false;

                if (taskType == "ticket")
                {
                    moduleInstDepny.Title = Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));
                }
                else
                {
                    if (ddlStatus.SelectedValue == Constants.InProgress && moduleInstDepny.Status != ddlStatus.SelectedValue)
                    {
                        moduleInstDepny.IsStartDateChange = true;
                        moduleInstDepny.StartDate = DateTime.Now;
                    }

                    if (tdDueDate.Visible && dtcDueDate.Date != DateTime.MinValue)
                    {
                        moduleInstDepny.IsEndDateChange = true;
                        moduleInstDepny.EndDate = dtcDueDate.Date;
                    }

                    int.TryParse(txtPctComplete.Text.Trim(), out pctComplete);
                    if (pctComplete > 100)
                        pctComplete = 100;

                    moduleInstDepny.PercentComplete = pctComplete;
                    moduleInstDepny.Status = ddlStatus.SelectedValue;

                    moduleInstDepny.AssignedTo = string.Join(Constants.Separator6, userMultiLookup);
                    moduleInstDepny.Title = txtTitle.Text.Trim();
                    moduleInstDepny.Description = txtDescription.Text.Trim();
                    //do not execute in case of new task
                    if (!newTask)
                    {
                        List<string> oldPredecessor = new List<string>();

                        if (moduleInstDepny.Predecessors != null)
                            oldPredecessor = moduleInstDepny.Predecessors.Split(',').ToList();

                        isPredecessorChange = predecessors.Except(oldPredecessor).ToList().Count > 0;
                    }

                    moduleInstDepny.Predecessors = string.Join(Constants.Separator6, predecessors);

                    if (trDisableSLA.Visible)
                        moduleInstDepny.SLADisabled = chkDisableSLA.Checked;

                    if (txtEstimatedHours.Visible)
                    {
                        double estimatedHours = 0;
                        double.TryParse(txtEstimatedHours.Text.Trim(), out estimatedHours);
                        moduleInstDepny.EstimatedHours = estimatedHours;
                    }

                    if (txtActualHours.Visible)
                    {
                        double actualHours = 0;
                        double.TryParse(txtActualHours.Text.Trim(), out actualHours);
                        moduleInstDepny.ActualHours = actualHours;
                    }
                    //Added by mudassir 2 march 2020
                    if (trDisableNotification.Visible)
                        moduleInstDepny.NotificationDisabled = chkDisableNotification.Checked;
                    //

                    //If task status is completed and user did fill actual hours then actualhousr will be same as of estimated hour
                    if (ddlStatus.SelectedValue == Constants.Completed || pctComplete >= 100)
                    {
                        startDependency = true;
                        if (moduleInstDepny.ActualHours <= 0)
                        {
                            moduleInstDepny.ActualHours = moduleInstDepny.EstimatedHours;
                        }
                        pctComplete = 100;
                    }
                    if (trServiceMatrix.Visible == true && (moduleInstDepny.SubTaskType.ToLower() == "applicationaccessrequest" || moduleInstDepny.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower()))
                    {
                        ServiceMatrix serviceMatrix = pnlServiceMatrix.Controls[0] as ServiceMatrix;
                        if (serviceMatrix.IsReadOnly)
                        {
                            serviceMatrix.SaveState();
                            List<ServiceMatrixData> serviceMatricsData = serviceMatrix.GetSavedState();

                            if (serviceMatricsData != null && serviceMatricsData.Count > 0)
                            {
                                XmlDocument doc = uHelper.SerializeObject(serviceMatricsData[0]);
                                if (doc != null)
                                    moduleInstDepny.ServiceApplicationAccessXml = Server.HtmlEncode(doc.OuterXml);

                                if (ddlStatus.SelectedValue == Constants.Completed)
                                {
                                    string userID = string.Empty;
                                    if (!string.IsNullOrEmpty(moduleInstDepny.NewUserName) && string.IsNullOrEmpty(serviceMatricsData[0].RoleAssignee))
                                    {
                                        UserProfile user = UserManager.GetUserByUserName(moduleInstDepny.NewUserName);
                                        if (user != null)
                                            serviceMatricsData[0].RoleAssignee = Convert.ToString(user.Id);
                                    }
                                    // For Application Role Assignee
                                    if (!string.IsNullOrEmpty(serviceMatricsData[0].RoleAssignee))
                                        userID = serviceMatricsData[0].RoleAssignee;

                                    if (!string.IsNullOrEmpty(userID))
                                    {
                                        ApplicationHelperManager appHelperManager = new ApplicationHelperManager(HttpContext.Current.GetManagerContext());
                                        appHelperManager.UpdateApplicationSpecificAccess(userID, serviceMatricsData[0]);
                                    }
                                }
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(hdnControl.Value) && hdnControl.Value == "removeuseraccess")
                    {
                        UpdateUserAccess();
                    }

                    //set if approver is changed
                    if (!string.IsNullOrWhiteSpace(moduleInstDepny.ApprovalType) && moduleInstDepny.ApprovalType.ToLower() != ApprovalType.None && peApprover.Visible)
                    {
                        List<string> oldApprovers = moduleInstDepny.Approver.Split(',').ToList();
                        List<string> newApprovers = peApprover.GetValues().Split(',').ToList();

                        moduleInstDepny.Approver = peApprover.GetValues();

                        List<string> newUsers = new List<string>();
                        if (newApprovers != null && newApprovers.Count > 0)
                        {
                        }
                        else
                        {
                            // Approvers taken out, change approval status
                            if (!string.IsNullOrEmpty(moduleInstDepny.Approver))
                                moduleInstDepny.ApprovalStatus = TaskApprovalStatus.Approved;
                            else
                                moduleInstDepny.ApprovalStatus = TaskApprovalStatus.None;
                            if (moduleInstDepny.Status == Constants.Waiting)
                                moduleInstDepny.Status = Constants.InProgress;
                        }

                        //add new user in action user list
                        if (newUsers.Count > 0)
                        {
                            if (moduleInstDepny.ApprovalStatus != TaskApprovalStatus.Pending)
                                moduleInstDepny.ApprovalStatus = TaskApprovalStatus.Pending;
                            if (moduleInstDepny.Approver == null)
                                moduleInstDepny.Approver = string.Empty;
                        }
                    }
                }

                // Get and save weight
                int weight = 1;
                int.TryParse(txtWeight.Text.Trim(), out weight);
                moduleInstDepny.Weight = weight;

                moduleInstDepny.Comment = txtComment.Text.Trim();
                if (moduleInstDepny.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                {
                    //if (!string.IsNullOrEmpty(pplUserName.GetValues()))
                    //{
                    //    moduleInstDepny.NewUserName = pplUserName.GetValues();
                    //}
                    //pplUserName.SetValues(moduleInstDepny.NewUserName);
                    //if (moduleInstDepny.AutoFillRequestor && project != null)
                    //{
                    //    UserProfile user = UserManager.GetUserInfoById(moduleInstDepny.NewUserName);
                    //    if (user != null)
                    //    {
                    //        if (string.IsNullOrEmpty(Convert.ToString(project[DatabaseObjects.Columns.TicketRequestor])))
                    //        {
                    //            project[DatabaseObjects.Columns.TicketRequestor] = user.Id;
                    //        }
                    //    }
                    //}
                }

                if (oldStatus != moduleInstDepny.Status && moduleInstDepny.Status == "Completed" && moduleInstDepny.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
                {
                    bool userAdded = false;
                    if (!string.IsNullOrEmpty(txtUsername.Text.Trim()))
                    {
                        userAdded = UserManager.SaveNewUser(context, txtUsername.Text.Trim());
                        UserProfile user = UserManager.GetUserInfoByIdOrName(txtUsername.Text.Trim());
                        if (userAdded && user != null)
                            //moduleInstDepny.NewUserName = pplUserName.GetValues();
                            moduleInstDepny.NewUserName = user.Id;
                    }
                    ptasks = TaskManager.LoadByProjectID(moduleInstDepny.ParentInstance);
                    List<UGITTask> dependentTasks = ptasks.Where(c => c.Predecessors != null && c.Predecessors.Contains(Convert.ToString(moduleInstDepny.ID))).ToList();
                    foreach (UGITTask dependentTask in dependentTasks)
                    {
                        if (dependentTask.UGITSubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
                        {
                            dependentTask.NewUserName = moduleInstDepny.NewUserName;
                            UserProfile pUser = UserManager.GetUserInfoById(moduleInstDepny.NewUserName);
                            if (pUser != null)
                                dependentTask.Title = string.Format("{0} for {1}", dependentTask.Title.Split(new string[] { " for " }, StringSplitOptions.RemoveEmptyEntries)[0], pUser.Name);

                            TaskManager.Save(dependentTask, context);
                        }
                    }
                }

                // Get attachments
                if (moduleInstDepny.AttachedFiles != null && moduleInstDepny.AttachedFiles.Count > 0)
                {
                    List<string> deleteFiles = UGITUtility.ConvertStringToList(txtDeleteFiles.Text, new string[] { "~" });
                    foreach (string dFile in deleteFiles)
                    {
                        if (moduleInstDepny.AttachedFiles.FirstOrDefault(x => x.Key.ToLower() == dFile.ToLower()).Value != null)
                            moduleInstDepny.AttachedFiles.Remove(dFile);
                    }
                }
                moduleInstDepny.AttachFiles = GetAttachedFiles();


                // Put new task in waiting status if its predecessor tasks are not completed
                if ((newTask || isPredecessorChange) && predecessors != null && predecessors.Count > 0)
                {
                    List<UGITTask> predecessorTasks = ptasks.Where(x => predecessors.Exists(y => y == Convert.ToString(x.ID))).ToList();
                    if (predecessorTasks != null && predecessorTasks.Count > 0 && predecessorTasks.Exists(x => x.Status != Constants.Completed && x.Status != Constants.Cancelled))
                    {
                        moduleInstDepny.Status = Constants.Waiting;
                        moduleInstDepny.PercentComplete = 0;
                    }
                }

                TaskManager.Save(moduleInstDepny, context);

                if (moduleInstDepny.Behaviour == "Task")
                {
                    #region Log History
                    //Update project history to log change
                    string historyDesc = string.Empty;
                    if (oldPctComplete != pctComplete)
                    {
                        historyDesc += string.Format("Task [{0}]:", moduleInstDepny.Title);
                        historyDesc += string.Format(" {0}% => {1}%", oldPctComplete, pctComplete);
                    }
                    if (oldStatus != status)
                    {
                        if (historyDesc == string.Empty)
                            historyDesc += string.Format("Task [{0}]:", moduleInstDepny.Title);
                        else
                            historyDesc += ";";
                        historyDesc += string.Format(" {0} => {1}", oldStatus, status);
                    }

                    if (oldSLADisabled.HasValue && oldSLADisabled != moduleInstDepny.SLADisabled)
                    {
                        if (historyDesc == string.Empty)
                            historyDesc += string.Format("Task [{0}]:", moduleInstDepny.Title);
                        else
                            historyDesc += ";";

                        historyDesc += string.Format(" SLA Disabled: {0} => {1}", oldSLADisabled, moduleInstDepny.SLADisabled);
                    }

                    if (historyDesc != string.Empty)
                    {
                        uHelper.CreateHistory(User, historyDesc, project, false, context);
                        //project.UpdateOverwriteVersion();
                    }
                    #endregion

                    //Update Total hold duration if task sla disable is checked or unchecked
                    if (oldSLADisabled != moduleInstDepny.SLADisabled)
                        TaskManager.UpdateSVCTicket(project, true);

                    ///update dependent tasks
                    var tasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                    tasks = UGITTaskManager.MapRelationshipObjects(tasks);

                    ///to get mapped relation data of module Instance dependency
                    moduleInstDepny = tasks.FirstOrDefault(x => x.ID == moduleInstDepny.ID);
                    if (moduleInstDepny != null)
                    {
                        if (moduleInstDepny.ParentTask != null || moduleInstDepny.ChildCount > 0)
                            TaskManager.PropagateTaskStatusEffect(ref tasks, moduleInstDepny);
                        TaskManager.SaveAllTasks(tasks);
                    }

                    // task cache isn't implemented yet
                    //TaskCache.ReloadProjectTasks("SVC", moduleInstDepny.ParentInstance);
                }

                if (startDependency && ticketCurrentStage != null && ticketCurrentStage.StageTypeChoice == StageType.Assigned.ToString())
                {
                    //start child tasks if svc is in assigned stage
                    TaskManager.StartTasks(moduleInstDepny.TicketId, moduleInstDepny.ID.ToString(), moduleInstDepny.Behaviour == "Task");
                    TaskManager.MoveSVCTicket(moduleInstDepny.TicketId);

                    // Need to reload parent SVC ticket after update above to avoid conflict
                    project = Ticket.GetCurrentTicket(context, ModuleName, projectPublicID);
                }
                else
                {
                    TaskManager.UpdateSVCTicket(project, false);
                }

                if (ConfigVariableHelper.GetValueAsBool(ConfigConstants.UpdateSVCPRPORP))
                    TaskManager.UpdateSVCPRPORP(project);

                // Notify assigned used about the task but only when use click on Save & notify Button
                if (btAction.ID == "btSaveAndNotify" && userMultiLookup != null && userMultiLookup.Count > 0)
                {
                    List<UserProfile> users = UserManager.GetUserInfosById(userMultiLookup[0].ToString());
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

                        string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", projectPublicID, moduleInstDepny.ID, ModuleName);
                        string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, context.TenantID);
                        string greeting = ConfigVariableHelper.GetValue("Greeting");
                        string signature = ConfigVariableHelper.GetValue("Signature");
                        StringBuilder taskEmail = new StringBuilder();
                        taskEmail.AppendFormat("{0} {1}<br /><br />", greeting, mailToNames.ToString());

                        if (moduleInstDepny.Status != Constants.Completed)
                            taskEmail.AppendFormat("A new task <b>\"{0}\"</b> has been assigned to you by {1}.<br>", moduleInstDepny.Title, User.Name);
                        else
                            taskEmail.AppendFormat("Task <b>\"{0}\"</b> was completed by {1}.<br>", moduleInstDepny.Title, User.Name);

                        if (moduleInstDepny.SubTaskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
                        {
                            string value = HttpUtility.HtmlDecode(moduleInstDepny.ServiceApplicationAccessXml);
                            XmlDocument doc = new XmlDocument();
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(value))
                                    doc.LoadXml(value.Trim());
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex, "Missing or invalid ServiceApplicationAccessXml");
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
                            }

                            if (serviceMatrixData != null)
                            {
                                serviceMatrixDataList.Add(serviceMatrixData);
                                string appAccessTaskDtls = uHelper.CreateApplicationAccessTable(serviceMatrixDataList, context);
                                taskEmail.Append("<br />" + appAccessTaskDtls + "<br />");
                            }
                        }
                        taskEmail.Append("<br /><br />" + signature + "<br />");
                        taskEmail.Append(emailFooter);

                        string emailSubject = string.Format("{0} {1}: {2}", moduleInstDepny.TicketId, moduleInstDepny.Status != Constants.Completed ? "New Task Assigned" : "Task Completed", moduleInstDepny.Title);
                        string attachUrl = string.Empty;
                        if (UGITUtility.StringToBoolean(ConfigVariableHelper.GetValue(ConfigConstants.AttachTaskCalendarEntry)))
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

                        MailMessenger mailMessage = new MailMessenger(context);
                        if (ConfigVariableHelper.GetValueAsBool(ConfigConstants.KeepSVCTaskNotifications) && !string.IsNullOrWhiteSpace(ModuleName) && ModuleName == "SVC")
                            mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl });
                        //mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl }, saveToTicketId: moduleInstDepny.ParentInstance); // Pass ticketID to save email
                        else
                            mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl });
                    }
                }

                if (Request["ParentFramePopupID"] != null)
                {
                    var refreshParentID = Request["ParentFramePopupID"];
                    if (!string.IsNullOrWhiteSpace(refreshParentID))
                        UGITUtility.CreateCookie(Response, "refreshParent", refreshParentID);
                }

                // Need to be implemented
                if (ModuleName == ModuleNames.SVC && moduleInstDepny != null && !string.IsNullOrEmpty(moduleInstDepny.Behaviour) && moduleInstDepny.Behaviour.ToLower() == "task")
                {
                    // Check if SVC Instance needed to be put on Hold/UnHold
                    if (oldStatus != status)
                        TaskManager.DoHoldUnHoldSVCInstance(context, moduleInstDepny.TicketId);

                    // Initiate Updating the records for current task in ScheduleActions List
                    ThreadStart starter = delegate { InitiateScheduleActionsInBackground(moduleInstDepny.ID, UGITUtility.StringToBoolean(UGITUtility.GetSPItemValueAsString(project, DatabaseObjects.Columns.EnableTaskReminder)), service); };
                    Thread thread = new Thread(starter);
                    thread.IsBackground = true;
                    thread.Start();
                }

                // Adding task info to cache so that we can focus on this task in task grid when popup is closed
                AddTaskInfoToContextCache();

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            #endregion
        }
        #endregion

        protected void BtCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        #region SVC Config methods
        private void FillModuleDropDown(DropDownList dropDown)
        {
            #region SP copy code 
            dropDown.Items.Clear();

            #region Get modules
            ObjModuleViewManager = new ModuleViewManager(context);
            List<UGITModule> ModuleList = ObjModuleViewManager.Load(x => x.EnableModule && x.ModuleName != ModuleNames.SVC && (x.ModuleType == ModuleType.SMS || x.ModuleName == ModuleNames.NPR || UGITUtility.StringToBoolean(x.EnableModuleAgent))).OrderBy(x => x.ModuleName).ToList();
            if (ModuleList != null && ModuleList.Count > 0)
            {
                foreach (UGITModule uGITModule in ModuleList)
                {
                    dropDown.Items.Add(new ListItem(uGITModule.Title, uGITModule.ModuleName));
                }
            }

            dropDown.Items.Insert(0, new ListItem("--Select Type--", ""));
            #endregion

            #endregion
        }
        protected void DdlModuleDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = string.Empty;
            if (string.IsNullOrWhiteSpace(Request[ddlModuleDetail.UniqueID]))
            {
                module = ddlModuleDetail.SelectedItem.Value;
            }
            else
            {
                module = Request[ddlModuleDetail.UniqueID];
            }
            ddlTicketRequestType.Items.Clear();
            if (!string.IsNullOrWhiteSpace(module))
            {
                int moduleID = 0;
                int.TryParse(module, out moduleID);
                ObjModuleViewManager = new ModuleViewManager(context);
                UGITModule moduleRow = ObjModuleViewManager.LoadByName(module);
                DataRow mRow = UGITUtility.ObjectToData(moduleRow).Rows[0];
                //DataRow moduleRow = uGITCache.GetModuleDetails(moduleID);
                ddlRequestTypeSubCategory.Items.Clear();
                DropDownList temp = uHelper.GetCategoryWithSubCategoriesDropDown(context, mRow);

                if (temp.Items.Count > 0)
                    ddlRequestTypeSubCategory.Items.AddRange(temp.Items.OfType<ListItem>().ToArray());

                if (ddlRequestTypeSubCategory.Items.Count > 0)
                    ddlRequestTypeSubCategory.SelectedIndex = 0;

                ddlTicketRequestType.Items.Clear();
                ddlTicketRequestType.Items.Insert(0, new ListItem("--Choose Later--", ""));
                DropDownList requestType = uHelper.GetRequestTypesWithCategoriesDropDown(context, mRow);
                if (requestType.Items.Count > 0)
                    ddlTicketRequestType.Items.AddRange(requestType.Items.OfType<ListItem>().ToArray());

                if (ddlTicketRequestType.Items.Count > 0)
                    ddlTicketRequestType.SelectedIndex = 0;
            }
        }
        private void DevexListBox_ValueChanged(object sender, EventArgs e)
        {
            //if (ddlModuleDetail.devexListBox.Value != null)
            //{
            //    int selectedModule = Convert.ToInt32(ddlModuleDetail.devexListBox.Value);

            //    //ddRequestType.ModuleName = ObjModuleViewManager.LoadByID(selectedModule).ModuleName;
            //    //ddRequestType.DataBind();
            //}
        }
        #endregion

        public void BindDDLCount()
        {
            ddlCount.Items.Clear();
            ddlRepeatCount.Items.Clear();
            for (int i = 0; i < 32; i++)
            {
                ddlCount.Items.Add(i.ToString());
                ddlRepeatCount.Items.Add(i.ToString());
            }
        }

        protected void ddlProjectStages_Init(object sender, EventArgs e)
        {
            if (ddlProjectStages.Items.Count > 0)
                return;

            if (ModuleName != "PMM" || project == null || !UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.ProjectLifeCycleLookup))
                return;

            long lifeCycleLookup = Convert.ToInt64(project[DatabaseObjects.Columns.ProjectLifeCycleLookup]);
            if (lifeCycleLookup <= 0)
                return;
            //BTS-21-000464
            List<LifeCycleStage> stages = lifeCycleStageManager.Load(x => x.LifeCycleName == lifeCycleLookup).OrderBy(x => x.StageStep).ToList();

            if (stages != null)
            {
                stages.ForEach(x => { ddlProjectStages.Items.Add(new ListItem($"{x.StageStep} - {x.Name}", $"{x.StageStep}")); });
            }
            ddlProjectStages.Items.Insert(0, new ListItem("(None)", "0"));
            ddlProjectStages.SelectedIndex = 0;
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
                string strComments = uHelper.FindAndConvertToAnchorTag(Server.HtmlDecode(hEntry.entry));
                lCommentDesc.Text = hEntry.entry.Contains("\n") ? "<br/>" + strComments : strComments;
            }
        }

        protected void ddlSprints_Init(object sender, EventArgs e)
        {
            if (ddlSprints.Items.Count > 0)
                return;

            if (ModuleName != ModuleNames.PMM || project == null)
                return;

            if (project != null)
            {
                long pmmIdLookup = UGITUtility.StringToLong(project[DatabaseObjects.Columns.ID]);
                SprintManager sprintManager = new SprintManager(context);
                List<Sprint> sprints = sprintManager.Load(x => x.PMMIdLookup == pmmIdLookup && x.TenantID == context.TenantID);
                if (sprints != null)
                {
                    ddlSprints.DataSource = sprints;
                    ddlSprints.DataTextField = DatabaseObjects.Columns.Title;
                    ddlSprints.DataValueField = DatabaseObjects.Columns.ID;
                    ddlSprints.DataBind();
                }
            }
        }

        public ListBox GetPredecessors(List<UGITTask> tasks, UGITTask currentTask)
        {
            tasks = UGITTaskManager.MapRelationalObjects(tasks);

            //hide all parents and childs and successor of current task to avoid cycling problem
            List<UGITTask> hideTasksInPred = new List<UGITTask>();
            if (currentTask == null)
            {
                // For new task, just exclude all parents up the chain
                currentTask = tasks.FirstOrDefault(x => x.ID == parentTaskID);
                if (currentTask != null && currentTask.ParentTask != null)
                    UGITTaskManager.GetDependentTasks(currentTask.ParentTask, ref hideTasksInPred, true);
            }
            else
            {
                UGITTaskManager.GetDependentTasks(currentTask, ref hideTasksInPred, false);
            }

            //Gets remaining task which may need to choose as predecessor
            List<UGITTask> remainingTasks = tasks.Except(hideTasksInPred).OrderBy(x => x.ItemOrder).ToList();
            tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            ListBox predecessorsList = new ListBox();
            foreach (UGITTask task in remainingTasks)
            {
                string displayString = task.ItemOrder + " - " + task.Title; //UGITUtility.StripHTML(task.Title);
                ListItem pred = new ListItem();
                pred.Value = Convert.ToString(task.ID);
                pred.Text = displayString;
                pred.Attributes.Add("title", displayString);
                if (task.StartDate != DateTime.MinValue && task.DueDate != DateTime.MinValue)
                {
                    pred.Attributes.Add(DatabaseObjects.Columns.StartDate, task.DueDate.Date.ToString());
                }
                predecessorsList.Items.Add(pred);
            }

            return predecessorsList;
        }

        public void ScheduleTaskReminder(DataRow currentTicket, UGITTask task, List<string> userMultiLookup)
        {
            List<UserProfile> users = UserManager.GetUserInfosById(string.Join(Constants.Separator6, userMultiLookup));
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

            Dictionary<string, string> taskReminderToEmail = new Dictionary<string, string>();
            string projectTitle = Convert.ToString(project[DatabaseObjects.Columns.Title]);
            taskReminderToEmail.Add(DatabaseObjects.Columns.ProjectID, projectPublicID);
            taskReminderToEmail.Add(DatabaseObjects.Columns.ProjectTitle, projectTitle);
            taskReminderToEmail.Add(DatabaseObjects.Columns.Title, task.Title);
            taskReminderToEmail.Add(DatabaseObjects.Columns.LinkDescription, task.Description);
            taskReminderToEmail.Add(DatabaseObjects.Columns.StartDate, task.StartDate.ToString());
            taskReminderToEmail.Add(DatabaseObjects.Columns.DueDate, task.DueDate.ToString());
            taskReminderToEmail.Add(DatabaseObjects.Columns.EstimatedHours, task.EstimatedHours.ToString());

            string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", projectPublicID, task.ID, ModuleName);

            string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskReminderToEmail, url, true, false, context.TenantID);
            string greeting = ConfigVariableHelper.GetValue("Greeting");
            string signature = ConfigVariableHelper.GetValue("Signature");
            StringBuilder taskReminderEmail = new StringBuilder();

            taskReminderEmail.AppendFormat("{0} {1}<br /><br />", greeting, mailToNames.ToString());
            taskReminderEmail.Append("The Task ");
            taskReminderEmail.AppendFormat("<b>\"{0}\"</b> ", task.Title);
            taskReminderEmail.AppendFormat("from project <b>{0}</b> ", projectTitle);
            if (task.DueDate > DateTime.Now)
                taskReminderEmail.AppendFormat("is due on {0}.<br>", (task.DueDate).ToString("MMM-d-yyyy"));
            else if (task.DueDate < DateTime.Now)
                taskReminderEmail.AppendFormat("is overdue! It was due on <b>{0}</b>.<br>", (task.DueDate).ToString("MMM-d-yyyy"));
            taskReminderEmail.Append("<br /><br />" + signature + "<br />");
            taskReminderEmail.Append(emailFooter);

            string emailTo = string.Empty;
            emailTo = string.Join("; ", emails.ToArray());
            if (string.IsNullOrEmpty(emailTo))
            {
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(DatabaseObjects.Columns.Title, string.Format("{0} {1}", Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]),
                                                   Convert.ToDateTime(task.DueDate)));

            //check the start date of send notification...
            DateTime dtStartTime = new DateTime();
            dtStartTime = task.DueDate.AddDays(task.ReminderDays);
            if (dtStartTime < DateTime.Now)
                dic.Add(DatabaseObjects.Columns.StartTime, DateTime.Now);
            else
                dic.Add(DatabaseObjects.Columns.StartTime, dtStartTime);
            //task.TaskReminderDays;



            //dic.Add(DatabaseObjects.Columns.TicketId, currentTicket[DatabaseObjects.Columns.Id]);
            dic.Add(DatabaseObjects.Columns.TicketId, task.ID);
            dic.Add(DatabaseObjects.Columns.ActionType, ScheduleActionType.Reminder);
            dic.Add(DatabaseObjects.Columns.EmailIDTo, emailTo);
            string subject = string.Format("{0} Task {1} Due Date Reminder.", projectPublicID, task.Title);
            dic.Add(DatabaseObjects.Columns.MailSubject, subject);


            dic.Add(DatabaseObjects.Columns.Recurring, false);
            dic.Add(DatabaseObjects.Columns.RecurringInterval, 0);
            if (task.RepeatInterval > 0)
            {
                dic[DatabaseObjects.Columns.Recurring] = true;
                DateTime newDateTime = new DateTime();
                newDateTime = Convert.ToDateTime(dic[DatabaseObjects.Columns.StartTime]).AddDays(task.RepeatInterval);
                TimeSpan outResult = newDateTime.Subtract(Convert.ToDateTime(dic[DatabaseObjects.Columns.StartTime]));
                int recurringInterval = (int)outResult.TotalMinutes;
                dic[DatabaseObjects.Columns.RecurringInterval] = recurringInterval;
            }

            dic.Add(DatabaseObjects.Columns.EmailBody, Convert.ToString(taskReminderEmail));

            dic.Add(DatabaseObjects.Columns.ModuleNameLookup, ModuleName);
            dic.Add(DatabaseObjects.Columns.RecurringEndDate, DateTime.MaxValue);
            // dic.Add(DatabaseObjects.Columns.ListName, UGITTaskHelper.GetTaskList(task.ModuleName));

            AgentJobHelper agentHelper = new AgentJobHelper(context);
            agentHelper.CreateSchedule(dic);
        }

        private Dictionary<string, byte[]> GetAttachedFiles()
        {

            Dictionary<string, byte[]> attachFiles = new Dictionary<string, byte[]>();
            //Save attachment in temp area
            if (Request.Files.Count > 0)
            {
                string fileName = string.Empty;
                string exptension = string.Empty;
                string suffix = string.Empty;
                string fullName = string.Empty;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i].ContentLength > 0 && attachFiles.FirstOrDefault(x => x.Key.ToLower() == Request.Files[i].FileName.ToLower()).Key == null)
                    {
                        fileName = Path.GetFileNameWithoutExtension(Request.Files[i].FileName);
                        exptension = Path.GetExtension(Request.Files[i].FileName);
                        fullName = string.Format("{0}{1}", fileName, exptension);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(Request.Files[i].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Request.Files[i].ContentLength);
                        }
                        attachFiles.Add(Request.Files[i].FileName, fileData);
                    }
                }
            }

            return attachFiles;
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

        protected void btnAssignToPct_Click(object sender, EventArgs e)
        {
            if (!isRowCount && !assignToList.Exists(x => x.UserName == string.Empty))
                assignToList.Add(new UGITAssignTo("", "", ""));

            rAssignToPct.DataSource = assignToList;
            rAssignToPct.DataBind();
            hdnReptRowcount.Value = Convert.ToString(rAssignToPct.Items.Count);
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            if (ModuleName == ModuleNames.SVC)
            {
                if (moduleInstDepny != null)
                {
                    //moduleInstDepny.Delete();
                    string message = string.Empty;
                    if (moduleInstDepny.Behaviour == "Ticket")
                    {
                        message = string.Format("DELETED sub-ticket {0} ", moduleInstDepny.RelatedTicketID);
                    }
                    else
                    {
                        message = string.Format("DELETED Task [{0}] which was in {1} state", moduleInstDepny.Title, moduleInstDepny.Status);
                    }
                    UGITTask uGITTask = moduleInstDepny;
                    TaskManager.Delete(moduleInstDepny);
                    uHelper.CreateHistory(User, message, project, context);

                    if (uGITTask.Behaviour == "Task")
                    {
                        //List<UGITTask> tasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                        //tasks = UGITTaskManager.MapRelationalObjects(tasks);
                        //UGITTask task = tasks.FirstOrDefault(x => x.ID == taskID);
                        //TaskManager.DeleteTask(ModuleName, ref tasks, task);
                        //TaskCache.ReloadProjectTasks("SVC", moduleInstDepny.ParentInstance);
                        LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(project);
                        if (currentStage != null && currentStage.StageTypeChoice == StageType.Assigned.ToString())
                        {
                            TaskManager.StartTasks(uGITTask.TicketId);
                            TaskManager.MoveSVCTicket(uGITTask.TicketId);
                        }

                        // Check if SVC Instance needed to be put on Hold/UnHold
                        TaskManager.DoHoldUnHoldSVCInstance(context, moduleInstDepny.TicketId);
                    }
                }
            }
            else
            {
                if (taskID > 0 && !string.IsNullOrEmpty(ModuleName))
                {
                    List<UGITTask> tasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
                    tasks = UGITTaskManager.MapRelationalObjects(tasks);

                    UGITTask task = tasks.FirstOrDefault(x => x.ID == taskID);
                    if (task != null)
                    {
                        List<UGITTask> deletedTasks = new List<UGITTask>();
                        TaskManager.GetDeletedTasks(tasks, ref deletedTasks, task);

                        TaskManager.DeleteTask(ModuleName, ref tasks, task);

                        //Clean update user allocation if actualhours by user enable
                        if (ugitModule != null && ugitModule.ActualHoursByUser)
                        {
                            List<string> existingUsers = new List<string>();
                            //if (task.AssignedTo != null)
                            if (!string.IsNullOrEmpty(task.AssignedTo))
                            {
                                existingUsers = UGITUtility.SplitString(task.AssignedTo, ",").ToList();
                            }

                            existingUsers = existingUsers.Distinct().ToList();
                            string webUrl = HttpContext.Current.Request.Url.ToString();
                            ResourceAllocationManager resourceAllocationManager = new ResourceAllocationManager(context);
                            ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate ()
                            {

                                resourceAllocationManager.UpdateProjectPlannedAllocation(webUrl, tasks, existingUsers, ModuleName, projectPublicID, true);
                            };
                            Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                            sThreadUpdateProjectPlannedAllocation.IsBackground = true;
                            sThreadUpdateProjectPlannedAllocation.Start();
                        }


                        //TaskCache.ReloadProjectTasks(ModuleName, projectPublicID);
                        TaskManager.CalculateProjectStartEndDate(ModuleName, tasks, project);
                        Ticket tReq = new Ticket(context, ModuleName);
                        tReq.CommitChanges(project);
                    }
                }
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }


        //Added by mudassir 3 feb 2020
        protected void btDeleteMappingTask_Click(object sender, EventArgs e)
        {
            if (service != null && service.Tasks != null && service.Tasks.Count > 0)
            {
                List<UGITTask> tasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);

                UGITTask task = tasks.FirstOrDefault(x => x.ID == taskID);

                if (task != null)
                {
                    //TaskManager.DeleteTask(ModuleName, ref tasks, task);
                    List<ServiceQuestionMapping> questionMappings = service.QuestionsMapping.Where(x => x.ServiceTaskID == task.ID).ToList();

                    ServiceQuestionMappingManager SQMM = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
                    SQMM.DeleteTasks(ModuleName, questionMappings);

                    TaskManager.DeleteTask(ModuleName, ref tasks, task);

                    Context.Cache.Add(string.Format("SVCConfigTask-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    uHelper.ClosePopUpAndEndResponse(Context, true, Request.Url.AbsolutePath);
                }
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            if (ModuleName == "SVC" && moduleInstDepny != null && moduleInstDepny.Behaviour == "Task")
            {
                // If task is On-Hold then first we should remove hold then cancel the task
                if (moduleInstDepny.OnHold || moduleInstDepny.Status == Constants.OnHoldStatus)
                {
                    RemoveHold(false);
                    moduleInstDepny = TaskManager.LoadByID(moduleInstDepny.ID);
                }

                string message = string.Empty;
                message = string.Format("CANCELLED Task [{0}] which was {1}", moduleInstDepny.Title, moduleInstDepny.Status);
                uHelper.CreateHistory(User, message, project, context);
                //Log.AuditTrail(string.Format("User {0} {1} from ticket {2} [{3}]", _spWeb.CurrentUser.Name, message, projectPublicID, parentTitle));

                moduleInstDepny.PercentComplete = 100;
                moduleInstDepny.Status = Constants.Cancelled;
                moduleInstDepny.CompletedBy = User.Id;
                moduleInstDepny.CompletionDate = DateTime.Now;
                TaskManager.Save(moduleInstDepny, context);

                ///update dependent tasks
                var tasks = TaskManager.LoadByProjectID(projectPublicID);
                tasks = UGITTaskManager.MapRelationshipObjects(tasks);

                ///to get mapped relation data of module Instance dependency
                moduleInstDepny = tasks.FirstOrDefault(x => x.ID == moduleInstDepny.ID);
                if (moduleInstDepny.ParentTaskID != 0 || moduleInstDepny.ChildCount > 0)
                {
                    TaskManager.PropagateTaskEffect(ref tasks, moduleInstDepny);
                    TaskManager.SaveAllTasks(tasks);
                }

                //TaskCache.ReloadProjectTasks("SVC", moduleInstDepny.ParentInstance);
                LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(project);
                if (currentStage != null && currentStage.StageTypeChoice == StageType.Assigned.ToString())
                {
                    TaskManager.StartTasks(moduleInstDepny.TicketId);
                    TaskManager.MoveSVCTicket(moduleInstDepny.TicketId);
                }

                // Send cancel notification to SVC ticket requestor if configured
                if (ConfigVariableHelper.GetValueAsBool(ConfigConstants.SVCTaskCancelNotifyRequestor) && project != null && project[DatabaseObjects.Columns.TicketRequestor] != null)
                {
                    if (ticketRequest != null)
                    {
                        string subject = string.Format("Task Cancelled: {0}", moduleInstDepny.Title);
                        string body = string.Format("Task: <b>[{0}]</b> of service request <b>[{1}: {2}]</b> has been cancelled.", moduleInstDepny.Title, UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.TicketId), UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));
                        UserProfileManager userProfileManager = new UserProfileManager(context);
                        List<UsersEmail> usersEmails = userProfileManager.GetUsersEmail(project, new string[] { DatabaseObjects.Columns.TicketRequestor }, false);
                        ticketRequest.SendEmailToEmailList(project, subject, body, usersEmails);
                    }
                }

                // Check if SVC Instance needed to be put on Hold/UnHold
                TaskManager.DoHoldUnHoldSVCInstance(context, moduleInstDepny.TicketId);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void chkAccountTask_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAccountTask.Checked)
            {
                chkAutoUserCreation.Visible = true;
                chkAutoFillRequestor.Visible = true;

            }
            else
            {
                chkAutoUserCreation.Visible = false;
                chkAutoFillRequestor.Visible = false;
            }
        }

        protected void chkApprovalRequired_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApprovalRequired.Checked)
            {
                fsApproval.Visible = true;
                peApprover.Visible = true;
            }
            else
            {
                fsApproval.Visible = false;
            }
        }

        protected void cvPPLUserName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtUsername.Visible && ddlStatus.SelectedValue == "Completed" /*&& pplUserName.ResolvedEntities.Count <= 0*/)
            {
                args.IsValid = false;
            }
        }
        //SpDelta 202(Skill Implementation - show allocation by skill)
        void BindAllocation()
        {
            gridAllocation.DataSource = GetSkillUserData();
        }
        protected void gridAllocation_DataBinding(object sender, EventArgs e)
        {
            if (gridAllocation.DataSource == null)
            {
                gridAllocation.DataSource = GetSkillUserData();
            }
        }
        //SpDelta 202(Skill Implementation - show allocation by skill)
        private List<Allocation> GetSkillUserData()
        {


            DateTime startdt = dtcStartDate.Value == null ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcStartDate.Date,
                   enddt = dtcDueDate.Value == null ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcDueDate.Date;


            ////DateTime startdt = dtcStartDate.IsDateEmpty ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcStartDate.SelectedDate,
            //enddt = dtcDueDate.IsDateEmpty ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcDueDate.SelectedDate;
            //
            List<Allocation> listAllocation = new List<Allocation>();

            List<string> userIds = new List<string>();
            //List<UserProfile> lstUserProfile = uGITCache.UserProfileCache.GetEnabledUsers();

            List<UserProfile> lstUserProfile = HttpContext.Current.GetOwinContext().Get<UserProfileManager>().GetEnabledUsers();
            //
            List<string> skillSet = new List<string>();
            string[] hdnskill = hdnSkillText.Value.Split(',');
            foreach (string item in hdnskill)
            {
                skillSet.Add(item);
            }

            foreach (UserProfile user in lstUserProfile)
            {
                if (user.Skills != null)
                {
                    string[] skills = user.Skills.Split(',').ToArray(); //Select(x => x.value).ToArray();

                    foreach (string item in skills)
                    {
                        if (skillSet.Contains(item))
                        {
                            //userIds.Add(Convert.ToInt32(user.Id));
                            userIds.Add(user.Id);
                        }
                    }
                }
            }
            userIds = userIds.Distinct().ToList();
            ////foreach (int userID in userIds)
            foreach (string userID in userIds)
            {
                Allocation dataAllocation = new Allocation();

                double totalPctAllocation = Convert.ToDouble(allocationManager.AllocationPercentage(Convert.ToString(userID), startdt, enddt));

                dataAllocation.ResourceId = Convert.ToString(userID);
                UserProfile profile = UserManager.LoadById(Convert.ToString(userID));
                if (profile != null)
                {
                    dataAllocation.Resource = profile.Name;
                    //dataAllocation.UserSkill = string.Join(",", profile.Skills.Select(x => x.Value).ToArray());
                    ////dataAllocation.UserSkill = string.Join(",", profile.Skills.Split(',').ToArray());
                    dataAllocation.UserSkill = profile.Skills;
                }
                dataAllocation.UserFullAllocation = totalPctAllocation;
                dataAllocation.FullAllocation = uHelper.CreateAllocationBar(totalPctAllocation);

                listAllocation.Add(dataAllocation);
            }

            return listAllocation.OrderBy(x => x.UserFullAllocation).ToList();
        }
        protected void dxUpdateSkill_Click(object sender, EventArgs e)
        {
            List<object> fieldValues = gridAllocation.GetSelectedFieldValues(new string[] { "ResourceId", "Resource" });

            if (fieldValues.Count == 0)
            {
                lblSkillErrorMessage.Visible = true;
                return;
            }

            string struserId = string.Empty;
            string struserName = string.Empty;
            foreach (object[] item in fieldValues)
            {
                struserId += item[0].ToString();
                UserProfile user = UserManager.GetUserById(Convert.ToString(item[0]));
                if (user != null)
                {
                    if (assignToList.Where(x => x.LoginName == user.UserName).Count() == 0)
                    {
                        assignToList.Add(new UGITAssignTo(user.Name, user.UserName, "100"));
                    }
                }
            }

            //assignToList = assignToList.Where(x => x.UserName != "").ToList();
            rAssignToPct.DataSource = assignToList;
            rAssignToPct.DataBind();

            hdnReptRowcount.Value = Convert.ToString(assignToList.Count);
            grdSkill.ShowOnPageLoad = false;
        }
        //
        protected void btnSkillAutoCalculate_Click(object sender, ImageClickEventArgs e)
        {
            lblSkillErrorMessage.Visible = false;
            gridAllocation.Selection.UnselectAll();
            grdSkill.ShowOnPageLoad = true;

            //BindAllocation();


            gridAllocation.DataBind();

            if (gridAllocation.VisibleRowCount > 0)
            {
                dxUpdateSkill.Visible = true;
            }
            else
            {
                dxUpdateSkill.Visible = false;
            }
        }

        protected void rAssignToPct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblPEAssignTo = (Label)e.Item.FindControl("lblPEAssignTo");
                TextBox textboxPct = (TextBox)e.Item.FindControl("txtPercentage");
                HiddenField hiddenField = (HiddenField)e.Item.FindControl("hdUserBoxId");
                UserValueBox peopleEditor = new UserValueBox();
                HtmlGenericControl divPeopleEditor = e.Item.FindControl("divAssignedToPct") as HtmlGenericControl;

                peopleEditor.ID = "peAssignedToPct_" + e.Item.ItemIndex;
                peopleEditor.CssClass = "userValueBox-dropDown";
                peopleEditor.UserTokenBoxAdd.ClientInstanceName = "peAssignedToPct_" + e.Item.ItemIndex;
                if (!string.IsNullOrEmpty(lblPEAssignTo.Text))
                {
                    UserProfile user = UserManager.GetUserByUserName(lblPEAssignTo.Text);
                    if (user != null)
                    {
                        DateTime startdt = dtcStartDate.Value == null ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcStartDate.Date,
                    enddt = dtcDueDate.Value == null ? DateTime.Now.ToString("MM/dd/yyyy").ToDateTime() : dtcDueDate.Date;
                        peopleEditor.SetValues(user.Id);
                        double totalPctAllocation = Convert.ToDouble(allocationManager.AllocationPercentage(user.Id, startdt, enddt));

                        if (totalPctAllocation > 100)
                        {
                            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
                            imgFlag.ToolTip = string.Format("Allocated: {0}%", Convert.ToString(totalPctAllocation));
                            imgFlag.Visible = true;
                        }
                    }
                    btnAssignToPct.Enabled = true;
                }
                else
                {
                    //peopleEditor.CommaSeparatedAccounts = "";
                    //btnAssignToPct.Enabled = false;
                }
                hiddenField.Value = peopleEditor.ID;
                divPeopleEditor.Controls.Add(peopleEditor);

                //set client click of delete button
                ASPxButton btn = e.Item.FindControl("imgbtnDelete") as ASPxButton;
                string btclick = String.Format("deleteEmptyRecord('{0}','{1}')", btn.CommandArgument, e.Item.ItemIndex);
                btn.ClientSideEvents.Click = "function(){ " + btclick + "}"; // String.Format("function(){ deleteEmptyRecord({0}) }", btn.CommandArgument);
                textboxPct.Attributes.Add("onblur", String.Format("validatePctAllocation('{0}')", textboxPct.ClientID));
            }
        }

        protected void rAssignToPct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var itemToRemove = assignToList.FirstOrDefault(a => a.UserName == Convert.ToString(e.CommandArgument));
                if (itemToRemove != null)
                    assignToList.Remove(itemToRemove);

                rAssignToPct.DataSource = assignToList;
                rAssignToPct.DataBind();
                hdnReptRowcount.Value = Convert.ToString(rAssignToPct.Items.Count);
            }

        }

        public void CheckIfPortalExists(string title)
        {
            DataRow item = Ticket.GetCurrentTicket(context, "PMM", projectPublicID);
            DataTable dtFormLayout = UGITUtility.ToDataTable<ModuleFormLayout>(ugitModule.List_FormLayout);
            bool documentControlExists = false; //Check if document control exists for the current module or not.
            if (dtFormLayout != null)
            {
                DataRow[] drDocument = dtFormLayout.Select($"{DatabaseObjects.Columns.FieldDisplayName}='DocumentControl' and {DatabaseObjects.Columns.FieldName} ='#Control#'");
                if (drDocument.Length > 0)
                    documentControlExists = true;//Option exists in module form layout
            }

            bool isExists = false;
            isExists = UserManager.IsUserPresentInField(User, item, Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketStageActionUserTypes)), true);
            if (isExists)
            {
                string docName = projectPublicID.Replace("-", "_");
                if (docName.Trim() == null)
                {
                    pDocuments.Style.Add("display", "none");
                }
                else
                {
                    lblPortalError.InnerText = "Go to document(s) tab to create Folder first for linking/uploading document(s).";
                    lblPortalError.Style.Add("text-decoration", "underline");
                    lblPortalError.Style.Add("display", "block");
                    pDocuments.Style.Add("display", "none");
                    divPortal.Style.Add("display", "none");
                    pAddDocuments.Style.Add("display", "block");
                    //Turn on the appropriate document upload option.
                    if (documentControlExists)  
                    {
                        UploadAndLinkDocuments.Visible = true; //Display the buttons to Link and upload Documents / Create Document Portal
                        taskFileUploadControl.Visible = false;
                    }
                    else
                    {
                        taskFileUploadControl.Visible = true; //turn on the DB file upload mechanism, same as file upload in tickets.
                        UploadAndLinkDocuments.Visible = false;
                    }
                }
            }
            else
            {
                pDocuments.Style.Add("display", "none");
                pAddDocuments.Style.Add("display", "none");
            }
        }

        private void PrepareAllocationGrid()
        {
            if (gridAllocation.Columns.Count <= 1)
            {

                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                colId.FieldName = "Resource";
                colId.Caption = DatabaseObjects.Columns.Resource;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = "UserSkill";
                colId.Caption = "Skills";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.EditItemTemplate = new ReadOnlyTemplate();
                gridAllocation.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = "FullAllocation";
                colId.Caption = "Total Allocation";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                gridAllocation.Columns.Add(colId);
            }
        }

        protected void chkTaskReminderEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTaskReminderEnable.Checked)
            {
                int taskReminderDefaultDays = UGITUtility.StringToInt(ConfigVariableHelper.GetValue(ConfigConstants.TaskReminderDefaultDays), 1);
                int taskReminderDefaultInterval = UGITUtility.StringToInt(ConfigVariableHelper.GetValue(ConfigConstants.TaskReminderDefaultInterval), 7);
                ddlCount.SelectedIndex = ddlCount.Items.IndexOf(ddlCount.Items.FindByValue(taskReminderDefaultDays.ToString()));
                ddlRepeatCount.SelectedIndex = ddlRepeatCount.Items.IndexOf(ddlRepeatCount.Items.FindByValue(taskReminderDefaultInterval.ToString()));
            }
            else
            {
                ddlCount.SelectedValue = "0";
                ddlRepeatCount.SelectedValue = "0";
            }
        }

        protected void chkRepeatEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkRepeatEnable.Checked)
            {
                divRecurringTask.Visible = false;

            }
            else
            {
                divRecurringTask.Visible = true;

            }
        }

        private void FillApplicationType(DropDownList dropDown)
        {
            dropDown.Items.Clear();
            List<ServiceQuestion> applicationAccessQues = service.Questions.Where(x => x.QuestionType == "ApplicationAccessRequest").ToList();
            dropDown.DataValueField = DatabaseObjects.Columns.Id;
            dropDown.DataTextField = DatabaseObjects.Columns.QuestionTitle;
            dropDown.DataSource = applicationAccessQues;
            dropDown.DataBind();
        }

        protected void ddlApplicationAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (service == null || service.Questions == null)
                return;
            ServiceQuestion applicationQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(ddlApplicationAccess.SelectedValue));
            DataTable dtApplications = BindApplications();
            if (dtApplications == null)
                return;
            if (applicationQuestion == null || applicationQuestion.QuestionTypeProperties == null)
                return;
            if (applicationQuestion.QuestionTypeProperties.Contains("application"))
            {
                string selectedApplications = applicationQuestion.QuestionTypePropertiesDicObj["application"];
                if (selectedApplications.ToLower() != "all")
                {
                    string[] strApp = selectedApplications.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> applicationIds = new List<string>();
                    foreach (string item in strApp)
                    {


                        string applicationId = item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1];

                        applicationIds.Add(applicationId.Split('-')[1]);

                    }
                    string appIds = string.Join("','", applicationIds);

                    if (!string.IsNullOrEmpty(appIds))
                    {
                        DataRow[] dr = dtApplications.Select(string.Format("{0} in ('{1}')", DatabaseObjects.Columns.ID, appIds));


                        if (dr.Length > 0)
                            dtApplications = dr.CopyToDataTable();

                    }
                }

                GridLookupApplicationQuestion.DataSource = dtApplications;
            }
        }

        protected void GridLookupApplicationQuestion_Load(object sender, EventArgs e)
        {
            if (ddlTypes.SelectedItem.Text == ServiceSubTaskType.AccessTask)
            {
                string selectedValue = Convert.ToString(GridLookupApplicationQuestion.Value);
                ddlApplicationAccess_SelectedIndexChanged(ddlApplicationAccess, null);
                GridLookupApplicationQuestion.DataBind();
                if (ddlApplicationAccess.Items.Count > 1)
                    tdApplicationAccess.Style.Remove("Display");
            }
        }

        private DataTable BindApplications()
        {
            string openTicketQuery = string.Format(" {0}!=1", DatabaseObjects.Columns.TicketClosed, "< FieldRef Name = '{0}' />< FieldRef Name = '{1}' />< FieldRef Name = '{2}' />< FieldRef Name = '{3}' />< FieldRef Name = '{4}' /> ",
                DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.CategoryNameChoice, DatabaseObjects.Columns.SubCategory, DatabaseObjects.Columns.Title);

            DataTable dtApplications = GetTableDataManager.ExecuteQuery($"select {DatabaseObjects.Columns.ID}, {DatabaseObjects.Columns.TicketId}, {DatabaseObjects.Columns.CategoryNameChoice}, {DatabaseObjects.Columns.SubCategory}, {DatabaseObjects.Columns.Title} from {DatabaseObjects.Tables.Applications} where {DatabaseObjects.Columns.TicketClosed} != '1'  ");

            return dtApplications;
        }

        private List<UGITTask> OrderServiceTasks(List<UGITTask> tasks)
        {
            List<UGITTask> orderedTasks = new List<UGITTask>();

            tasks = tasks.OrderBy(x => x.ItemOrder).ToList();
            foreach (UGITTask fTask in tasks)
            {
                fTask.ItemOrder = tasks.IndexOf(fTask) + 1;
                orderedTasks.Add(fTask);
            }

            return orderedTasks;
        }

        protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTypes.SelectedIndex == 0)
                taskType = ServiceSubTaskType.Task;
            else if (ddlTypes.SelectedIndex == 1)
                taskType = ServiceSubTaskType.AccountTask;
            else if (ddlTypes.SelectedIndex == 2)
                taskType = ServiceSubTaskType.AccessTask;
            trApplicationAccess.Visible = false;
            tdApplicationAccess.Style.Add("Display", "none");
            tdAccountTask.Style.Add("Display", "none");
            if (taskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower())
            {
                tdAccountTask.Style.Remove("Display");
                chkAutoUserCreation.Visible = true;
                chkAutoFillRequestor.Visible = true;
            }
            else if (taskType.ToLower() == ServiceSubTaskType.AccessTask.ToLower())
            {
                FillApplicationType(ddlApplicationAccess);
                ddlApplicationAccess_SelectedIndexChanged(ddlApplicationAccess, null);
                trApplicationAccess.Visible = true;
                GridLookupApplicationQuestion.DataBind();
                GridLookupApplicationQuestion.GridView.Style.Add("width", "168px");
                if (ddlApplicationAccess.Items.Count > 1)
                    tdApplicationAccess.Style.Remove("Display");
            }
        }

        protected void aspxTriggerTaskHold_Click(object sender, EventArgs e)
        {
            commentsHoldPopup.ShowOnPageLoad = false;
            #region copy code from SP
            commentsHoldPopup.ShowOnPageLoad = false;
            ASPxButton btAction = sender as ASPxButton;
            TaskManager.TaskOnHold(popedHoldComments.Text.Trim(), aspxdtOnHoldDate.Date, aspxOnHoldReason.Text, moduleInstDepny, moduleName: ModuleName);
            TaskManager.DoHoldUnHoldSVCInstance(context, moduleInstDepny.ParentInstance, UGITUtility.StringToInt(moduleInstDepny.ID), latestComment: popedHoldComments.Text.Trim()); // Put SVC ticket on hold
            //TaskCache.ReloadProjectTasks("SVC", moduleInstDepny.ParentInstance);
            if (btAction.ID == "NotifyHold")
                HoldTaskMailSendNotification(moduleInstDepny, "Hold");

            aspxdtOnHoldDate.Value = null;
            aspxOnHoldReason.SelectedIndex = -1;
            popedHoldComments.Text = string.Empty;

            uHelper.ClosePopUpAndEndResponse(Context, true);
            #endregion
        }

        protected void aspxTriggerUnholdTask_Click(object sender, EventArgs e)
        {
            commentsUnHoldPopup.ShowOnPageLoad = false;
            RemoveHold(true);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void pnlAssignToPct_Callback(object sender, CallbackEventArgsBase e)
        {
            assignToList.Clear();
            foreach (RepeaterItem item in rAssignToPct.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var peopleEditor = (UserValueBox)item.FindControl("peAssignedToPct_" + item.ItemIndex);
                    var textboxPct = (TextBox)item.FindControl("txtPercentage");
                    UGITAssignTo assignToItem = new UGITAssignTo();

                    if (peopleEditor.GetValuesAsList() != null && peopleEditor.GetValuesAsList().Count > 0)
                    {
                        UserValueBox entity = (UserValueBox)peopleEditor;
                        if (entity != null && entity != null)
                        {
                            UserProfile user = UserManager.GetUserById(peopleEditor.GetValuesAsList()[0]);
                            if (user != null)
                            {
                                assignToItem.LoginName = user.UserName;
                                assignToItem.UserName = user.Name;
                                assignToItem.Percentage = textboxPct.Text;
                            }

                            assignToList.Add(assignToItem);
                        }
                        btnAssignToPct.Enabled = true;
                    }
                    else
                    {
                        if (!assignToList.Exists(x => x.UserName == string.Empty))
                            assignToList.Add(new UGITAssignTo("", "", ""));
                    }
                }
            }

            if (e.Parameter.Contains("Delete"))
            {
                string itemargument = UGITUtility.SplitString(e.Parameter, Constants.Separator2)[1];
                var itemToRemove = itemargument; //assignToList.FirstOrDefault(a => a.UserName == Convert.ToString(itemargument));
                if (itemToRemove != null)
                    assignToList.RemoveAt(Convert.ToInt32(itemargument));
                else
                    assignToList.RemoveAll(x => x.UserName == string.Empty || x.UserName == null);
            }
            else
            {
                if (!isRowCount)
                    assignToList.Add(new UGITAssignTo("", "", ""));
            }
            rAssignToPct.DataSource = assignToList;
            rAssignToPct.DataBind();
            hdnReptRowcount.Value = Convert.ToString(rAssignToPct.Items.Count);
        }

        protected void BtApproveTask_Click(object sender, EventArgs e)
        {
            ptasks = TaskManager.LoadByProjectID(ModuleName, projectPublicID);
            moduleInstDepny = TaskManager.LoadByID(taskID);

            string oldStatus = moduleInstDepny.Status;
            double oldPctComplete = moduleInstDepny.PercentComplete;

            UGITTask task = null;
            try
            {
                if (!isDuplicate && taskID > 0)
                {
                    task = ptasks.FirstOrDefault(x => x.ID == taskID);
                }

                if (oldStatus != Constants.InProgress || oldPctComplete != 100)
                {
                    if (!string.IsNullOrEmpty(task.Predecessors))
                    {
                        var PredecessorsStaus = task.Predecessors.Split(',');

                        foreach (var statusOfPredecessorsTask in PredecessorsStaus)
                        {
                            var previousTask = TaskManager.LoadByID(Convert.ToInt64(statusOfPredecessorsTask));

                            //if (previousTask.Status == Constants.Waiting || previousTask.Status == Constants.NotStarted)
                            if (previousTask.Status != Constants.Completed)
                            {
                                isPreviousTask = true;
                                taskName = $"{previousTask.ItemOrder}-{previousTask.Title}";
                                //lblTaskStatus.Text = $"Complete Predecessors Task which is  {previousTask.Title}";
                                //return;
                            }
                            //else
                            //{
                            //     TaskWithPredecessors(task);
                            //}

                        }
                        if (!isPreviousTask)
                        {
                            TaskWithPredecessors(task);

                        }
                        else
                        {
                            lblTaskStatus.Text = $"Complete Predecessors Task which is  {taskName}";
                            return;
                        }
                    }

                    else//Check for first task
                    {
                        TaskWithPredecessors(task);
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        protected void BtRejectReason_Click(object sender, EventArgs e)
        {
            UGITTask ugitTask = TaskManager.LoadByID(Convert.ToInt64(taskID));
            ugitTask.Comment = ugitTask.Comment.Insert(0, "[Reject Reason]: " + popedRejectComments.Text.Trim() + ";#False<;#>");
            ugitTask.Status = Constants.Cancelled;
            ugitTask.Approver = null;
            TaskManager.SaveTask(ref ugitTask, ModuleName, projectPublicID);
            //}
            uHelper.ClosePopUpAndEndResponse(Context, true, "/refreshpage/");
        }


        private void TaskWithPredecessors(UGITTask task)
        {
            task.Approver = peApprover.GetValues();
            task.Status = Constants.InProgress;
            task.PercentComplete = 0;
            //completed on..
            task.CompletionDate = DateTime.Now;
            task.Changes = true;
            TaskManager.PropagateTaskStatusEffect(ref ptasks, task);
            TaskManager.SaveTask(ref task, ModuleName, projectPublicID);
            uHelper.ClosePopUpAndEndResponse(Context, true, "/refreshpage/");
        }

        private bool ShowActualHours(string moduleName)
        {
            bool showActualHours = false;
            UGITModule module = ObjModuleViewManager.LoadByName(ModuleName);
            if (module.ModuleName.ToLower().Equals("pmm"))
                showActualHours = ConfigVariableHelper.GetValueAsBool(ConfigConstants.EnableProjStdWorkItems) ? false : module.ActualHoursByUser;
            else
                showActualHours = module.ActualHoursByUser;
            return showActualHours;
        }

        private void UpdateUserAccess()
        {
            RemoveUserAccess removeUserAccess = (RemoveUserAccess)pnlRemoveUserAccess.Controls[0];

            RemoveAccessList removeAccessList = new RemoveAccessList();
            if (removeUserAccess.RemoveAccessList != null)
            {
                if (removeUserAccess.RemoveAccessList.SelectionType.ToLower() == "removespecificaccess")
                {
                    List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
                    removeAccessList.ModuleRoleRelationList = removeUserAccess.RemoveAccessList.ModuleRoleRelationList;
                    removeAccessList.ServiceMatrixDataList = removeUserAccess.RemoveAccessList.ServiceMatrixDataList;
                    removeAccessList.SelectionType = removeUserAccess.RemoveAccessList.SelectionType;
                    removeAccessList.UserId = removeUserAccess.RemoveAccessList.UserId;
                }
                else
                {
                    XmlDocument docRemoveAll = new XmlDocument();
                    string applicationAccessXml = HttpContext.Current.Server.HtmlDecode(moduleInstDepny.ServiceApplicationAccessXml);
                    docRemoveAll.LoadXml(applicationAccessXml);
                    removeAccessList = (RemoveAccessList)uHelper.DeSerializeAnObject(docRemoveAll, removeAccessList);
                    removeUserAccess.UpdateRemoveAccessList(removeAccessList);
                }
                XmlDocument doc = uHelper.SerializeObject(removeAccessList);
                if (doc != null)
                {
                    moduleInstDepny.ServiceApplicationAccessXml = Server.HtmlEncode(doc.OuterXml);
                }
            }

            //if (ddlStatus.SelectedValue == Constants.Completed)
            //{
            //    int userID = removeAccessList.UserId;
            //    if (!string.IsNullOrEmpty(removeAccessList.SelectionType) && removeAccessList.SelectionType.ToLower() == "removespecificaccess")
            //    {
            //        if (removeAccessList.ServiceMatrixDataList != null && removeAccessList.ServiceMatrixDataList.Count() > 0)
            //        {
            //            // For Application Role Assignee

            //            //   ApplicationAccess.UpdateApplicationSpecificAccess(userID, removeAccessList.ServiceMatrixDataList);
            //        }
            //    }
            //    else if (removeAccessList.SelectionType.ToLower() == "removeallaccess")
            //    {
            //        string history = string.Empty;
            //        SPFieldUserValue userRoleAssignee = new SPFieldUserValue(_spWeb, Convert.ToString(userID));
            //        string applicationId = removeAccessList.ModuleRoleRelationList.Select(c => c.ApplicationId).FirstOrDefault();
            //        SPQuery spQuery = new SPQuery();
            //        spQuery.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='True'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>",
            //                                       DatabaseObjects.Columns.APPTitleLookup, uHelper.StringToInt(applicationId), DatabaseObjects.Columns.ApplicationRoleAssign, userID);
            //        spQuery.ViewFields = string.Concat(
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Id),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Title),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleLookup),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.APPTitleLookup),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationModulesLookup),
            //            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleAssign));
            //        SPListItemCollection spListItemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, spQuery);
            //        if (spListItemColl != null && spListItemColl.Count > 0)
            //        {
            //            if (userRoleAssignee != null && removeAccessList != null && removeAccessList.ServiceMatrixDataList != null && removeAccessList.ServiceMatrixDataList.Count() > 0)
            //            {
            //                history = string.Format("{0} has deleted relation sd between", userRoleAssignee.User.Name);
            //            }
            //            foreach (ServiceMatrixData serviceMatrix in removeAccessList.ServiceMatrixDataList)
            //            {
            //                foreach (ServiceData gridData in serviceMatrix.lstGridData)
            //                {
            //                    history = string.Format("{0} module {1}- role {2},", history, gridData.RowName, gridData.ColumnName);
            //                }
            //                history = history.Substring(0, history.Length - 1);
            //            }
            //            while (spListItemColl.Count > 0)
            //            {
            //                spListItemColl[0].Delete();
            //            }
            //            SPListItem spListItemApp = uHelper.GetListItem(DatabaseObjects.Lists.Applications, DatabaseObjects.Columns.Id, applicationId, new string[] { DatabaseObjects.Columns.History });
            //            uHelper.CreateHistory(_spWeb.CurrentUser, history, spListItemApp);
            //        }
            //    }
            //}

        }

        /// <summary>
        /// This method is used to add task info to the context cache, which will be used to set task grid focus on the task obtained from this cache
        /// </summary>
        private void AddTaskInfoToContextCache()
        {
            StringBuilder seletedParams = new StringBuilder();
            seletedParams.AppendFormat("taskid={0}", Request["taskid"]);
            seletedParams.AppendFormat("&taskindex={0}", Request["taskIndex"]);
            Context.Cache.Add(string.Format("TaskInfo-{0}", User.Id), seletedParams.ToString(), null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
        }

        #region Method to Initiate Updating the records for current task in ScheduleActions List
        private void InitiateScheduleActionsInBackground(long taskId, bool needTaskReminder, Services service)
        {
            try
            {
                //ScheduleTaskReminder(project, moduleInstDepny, userMultiLookup);
                // Need to create ModuleInstanceDependency here instead of passing it in to prevent SPWeb references from parent web
                List<UGITTask> taskList = new List<UGITTask>();
                UGITTask task = TaskManager.LoadByID(taskId);
                taskList.Add(task);
                UGITTaskManager.UpdateScheduleActions(context, taskList, needTaskReminder, service);
            }
            catch (Exception error)
            {
                ULog.WriteException(error);
            }
        }
        #endregion

        /// <summary>
        /// This method is used to check if user exists in assigned users
        /// </summary>
        /// <param name="moduleInstDepny"></param>
        /// <returns></returns>
        private bool IsCurrentUserAssignedToTask(UGITTask moduleInstDepny)
        {
            if (moduleInstDepny == null || string.IsNullOrEmpty(moduleInstDepny.AssignedTo) || UserManager.GetUserInfosById(moduleInstDepny.AssignedTo).Count <= 0)
                return false;

            List<string> lstAssignedUsers = moduleInstDepny.AssignedTo.Split(',').ToList();

            // Check if current user is in the list
            bool userExists = lstAssignedUsers.Any(x => x == User.Id);

            // check if lstAssignedUsers contains any group
            // if yes then check if current user exists in that group
            if (!userExists)
            {

                foreach (string assignedUserId in lstAssignedUsers)
                {
                    if (UserManager.CheckUserIsGroup(assignedUserId))
                    {
                        userExists = UserManager.CheckUserInGroup(User.Id, assignedUserId);
                    }
                }
            }

            return userExists;
        }

        void MakeReadOnly()
        {
            lbTitle.Visible = true;
            lbStatus.Visible = true;
            lbAssignedTo.Visible = true;
            lbDueDate.Visible = true;
            lbEstimatedHours.Visible = true;
            lbActualHours.Visible = true;
            lbDescription.Visible = true;
            lbWeight.Visible = true;
            txtComment.Visible = false;
            txtTitle.Visible = true;
            ddlStatus.Visible = false;
            peAssignedTo.Visible = false;
            //trPredecessors.Visible = false;
            dtcDueDate.Visible = true;
            txtEstimatedHours.Visible = false;
            txtActualHours.Visible = false;
            txtDescription.Visible = true;
            txtWeight.Visible = false;
            lbPctComplete.Visible = true;
            txtPctComplete.Visible = false;
            btActions.Visible = false;

            peApprover.Visible = false;
            lnkbtnAssignApprover.Visible = false;
            assignApprover.Visible = false;
            lbApprover.Visible = true;


            // Allow task assignees, owner & admins to re-assign task or add/edit approvers in read-only mode
            if (UserManager.IsUserPresentInField(User, project, DatabaseObjects.Columns.TicketOwner) || UserManager.IsTicketAdmin(User) ||
            (moduleInstDepny != null && IsCurrentUserAssignedToTask(moduleInstDepny)))
            {
                lbTitle.Visible = false;
                lbDescription.Visible = false;
                lbDueDate.Visible = false;
                lbAssignedTo.Visible = false;
                peAssignedTo.Visible = true;
                btActions.Visible = true;

                txtTitle.Visible = true;
                txtDescription.Visible = true;
                dtcDueDate.Visible = true;
                peApprover.Visible = true;
                lnkbtnAssignApprover.Visible = true;
                assignApprover.Visible = true;
                lbApprover.Visible = false;
            }

            btActionscancel.Visible = !btActions.Visible;
        }

        private void ShowAttachmentList(UGITTask task, bool enableEdit)
        {
            pAddattachment.Visible = false;
            if (enableEdit)
                pAddattachment.Visible = true;

            pAddattachment.Style.Add(HtmlTextWriterStyle.Display, "block");
            string deleteElement = string.Empty;
            if (ModuleName == "SVC")
            {
                if ((task.AttachedFiles == null || task.AttachedFiles.Count == 0) /*&& project.Attachments.Count == 0*/)
                    return;
            }
            else
            {
                if (task.AttachedFiles == null || task.AttachedFiles.Count == 0)
                    return;
            }


            ddlExistingAttc.Items.Clear();
            foreach (string key in task.AttachedFiles.Keys)
            {
                ddlExistingAttc.Items.Add(new ListItem(key, task.AttachedFiles[key]));
                if (enableEdit)
                    deleteElement = string.Format("<label onclick='removeAttachment(this);'><img src='/_layouts/15/images/ugovernit/delete-icon.png' alt='Delete'/><span style='display:none;'>{0}</span></label>", key);
                string documentUrl = UGITUtility.GetAbsoluteURL(string.Format("{0}", task.AttachedFiles[key]));
                pAttachment.Controls.Add(new LiteralControl(string.Format("<div class='fileitem fileread'><span style='cursor:pointer;' onclick='window.open(\"{1}\")'>{0}</span>{2}</div>", key.Replace("'", "\'").Replace("\"", "\\\""), documentUrl, deleteElement)));
            }
            // Need to be fixed
            //if (ModuleName == "SVC")
            //{
            //    foreach (string fileName in project.Attachments)
            //    {
            //        string documentUrl = UGITUtility.GetAbsoluteURL(string.Format("{0}", UGITUtility.GetAbsoluteURL(project.Attachments.UrlPrefix + fileName)));
            //        pAttachment.Controls.Add(new LiteralControl(string.Format("<div class='fileitem fileread'><span style='cursor:pointer;' onclick='window.open(\"{1}\")'>{0}</span></div>", fileName.Replace("'", "\'").Replace("\"", "\\\""), documentUrl)));
            //    }
            //}

            if (ddlExistingAttc.Items.Count >= 9)
                pAddattachment.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        void TicketHoursViews_AfterSave(object sender, EventArgs e)
        {
            if (ModuleName == "SVC")
            {
                //TaskCache isn't implemented yet
                //TaskCache.ReloadProjectTasks(ModuleName, projectPublicID);
            }
            else if (ModuleName == "PMM" || ModuleName == "NPR" || ModuleName == "TSK" || TaskManager.IsModuleTasks(ModuleName))
            {
                if (project != null && ptasks != null && ptasks.Count > 0)
                {
                    UGITTask task = ptasks.FirstOrDefault(x => x.ID == taskID);
                    //TaskCache.ReloadProjectTasksByProjectIdBasedonUpdatedTasks(ModuleName, projectPublicID, new List<UGITTask>() { task }, project);

                    TaskManager.CalculateProjectStartEndDate(ModuleName, ptasks, project);
                    ticketRequest.CommitChanges(project);
                }
            }

        }

        protected void rfvActualHours_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddlStatus.SelectedItem != null && ddlStatus.SelectedItem.Text.ToLower() == "completed")
            {
                if (txtActualHours.Text.Trim() == string.Empty || txtActualHours.Text.Trim() == "0")
                {
                    args.IsValid = false;
                }
            }
        }

        /// <summary>
        /// This method is used to uncancel the previously cancelled task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkUncancelTask_Click(object sender, EventArgs e)
        {
            if (ModuleName == "SVC" && moduleInstDepny != null && moduleInstDepny.Behaviour == "Task")
            {
                string message = string.Empty;
                message = string.Format("UNCANCELLED Task [{0}] which was {1}", moduleInstDepny.Title, moduleInstDepny.Status);
                uHelper.CreateHistory(User, message, project, context);

                // Get currentStage and assignedStage for SVC Ticket life cycle
                LifeCycle lifeCycle = ticketRequest.GetTicketLifeCycle(project);
                LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(project);
                LifeCycleStage assignedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == StageType.Assigned.ToString());

                // Set the status of current task to Waiting if below condition is true
                if (hasIncompletePredecessors() || (currentStage != null && assignedStage != null && currentStage.StageStep < assignedStage.StageStep))
                {
                    moduleInstDepny.Status = Constants.Waiting;
                    moduleInstDepny.PercentComplete = 0;
                }
                else if (currentStage != null && assignedStage != null && currentStage.StageStep >= assignedStage.StageStep)
                {
                    moduleInstDepny.Status = Constants.InProgress;
                    moduleInstDepny.PercentComplete = 0;
                }

                moduleInstDepny.CompletedBy = null;
                moduleInstDepny.CompletionDate = DateTime.MinValue;
                TaskManager.Save(moduleInstDepny, context);


                if (currentStage != null && currentStage.StageTypeChoice == StageType.Assigned.ToString())
                {
                    TaskManager.StartTasks(moduleInstDepny.TicketId);
                    TaskManager.MoveSVCTicket(moduleInstDepny.TicketId);
                }

                // Send cancel notification to SVC ticket requestor if configured
                if (ConfigVariableHelper.GetValueAsBool(ConfigConstants.SVCTaskCancelNotifyRequestor) && project != null && project[DatabaseObjects.Columns.TicketRequestor] != null)
                {
                    if (ticketRequest != null)
                    {
                        string subject = string.Format("Task Uncancelled: {0}", moduleInstDepny.Title);
                        string body = string.Format("Task: <b>[{0}]</b> of service request <b>[{1}: {2}]</b> has been uncancelled.", moduleInstDepny.Title, UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.TicketId), UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.Title));
                        UserProfileManager userProfileManager = new UserProfileManager(context);
                        List<UsersEmail> usersEmails = userProfileManager.GetUsersEmail(project, new string[] { DatabaseObjects.Columns.TicketRequestor }, false);
                        ticketRequest.SendEmailToEmailList(project, subject, body, usersEmails);
                    }
                }

                // Check if SVC Instance needed to be put on Hold/UnHold
                TaskManager.DoHoldUnHoldSVCInstance(context, moduleInstDepny.TicketId);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        /// <summary>
        /// This method is used to Un-Hold the Task
        /// </summary>
        /// <param name="sendNotification"></param>
        protected void RemoveHold(bool sendNotification)
        {
            string reasonComment = popedUnHoldComments.Text.Trim();
            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
            TaskManager.TaskUnHold(reasonComment, moduleInstDepny);
            //TaskManager.TaskUnHold(reasonComment, taskID);     // check for taskid here in case of svc
            //


            if (sendNotification)
            {
                HoldTaskMailSendNotification(moduleInstDepny, "UnHold");
            }

        }

        private void HoldTaskMailSendNotification(UGITTask moduleInstDepny, string actionType)
        {
            List<UserProfile> users = null;
            //If Approver & AssignedTo are null throw exception
            if (moduleInstDepny.Approver != null && moduleInstDepny.ApprovalStatus == TaskApprovalStatus.Pending)
            {
                users = UserManager.GetUserInfosById(moduleInstDepny.Approver);
            }
            else if (moduleInstDepny.AssignedTo != null)
            {
                users = UserManager.GetUserInfosById(moduleInstDepny.AssignedTo);
            }

            if (users == null || users.Count == 0)
                return;

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

                string url = string.Format("{0}?taskType={1}&viewtype={2}&projectID={3}&taskID={4}&moduleName={5}", UGITUtility.GetAbsoluteURL(Constants.HomePage), "task", "1", projectPublicID, moduleInstDepny.ID, ModuleName);
                string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, context.TenantID);
                string greeting = ConfigVariableHelper.GetValue("Greeting");
                string signature = ConfigVariableHelper.GetValue("Signature");
                StringBuilder taskEmail = new StringBuilder();
                taskEmail.AppendFormat("{0} {1}<br /><br />", greeting, mailToNames.ToString());
                if (actionType == "Hold")
                {
                    if (isHold)
                        taskEmail.AppendFormat("The hold-till date for task <b>\"{0}\"</b> has been changed by {1}.<br>", moduleInstDepny.Title, User.Name);
                    else
                        taskEmail.AppendFormat("Task <b>\"{0}\"</b> has been put on hold by {1}.<br>", moduleInstDepny.Title, User.Name);
                }
                else
                {
                    taskEmail.AppendFormat("Task <b>\"{0}\"</b> has been taken off hold by {1}.<br>", moduleInstDepny.Title, User.Name);
                }

                taskEmail.Append("<br /><br />" + signature + "<br />");
                taskEmail.Append(emailFooter);
                string emailSubject = string.Empty;
                if (actionType == "Hold")
                {
                    if (isHold)
                        emailSubject = string.Format("{0} {1}: {2}", moduleInstDepny.TicketId, "Task On Hold Date Changed", moduleInstDepny.Title);
                    else
                        emailSubject = string.Format("{0} {1}: {2}", moduleInstDepny.TicketId, "Task On Hold", moduleInstDepny.Title);

                }
                else
                {
                    emailSubject = string.Format("{0} {1}: {2}", moduleInstDepny.TicketId, "Task Hold Removed", moduleInstDepny.Title);
                }
                string attachUrl = string.Empty;
                if (UGITUtility.StringToBoolean(ConfigVariableHelper.GetValue(ConfigConstants.AttachTaskCalendarEntry)))
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

                MailMessenger mailMessage = new MailMessenger(context);
                mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl });

                //if (ConfigVariableHelper.GetValueAsBool(ConfigConstants.KeepSVCTaskNotifications) && !string.IsNullOrWhiteSpace(ModuleName) && ModuleName == "SVC")
                //    mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl }, saveToTicketId: moduleInstDepny.ParentInstance); // Pass ticketID to save email
                //else
                //    mailMessage.SendMail(string.Join(",", emails.ToArray()), emailSubject, "", taskEmail.ToString(), true, new string[] { attachUrl });
            }
        }

        [WebMethod]
        public void TestAjax()
        {
            HyperLink hyperLink = new HyperLink();
            hyperLink.ID = "testhyper";
            hyperLink.Text = "chetan";
            //testPanel.Controls.Add(hyperLink);
        }

        protected void btnLinkDocument_Click(object sender, EventArgs e)
        {
            //Upload upload = new Upload();
            //upload.LinkDocument();
            //Try to pass dynamic value and if documentment created or not

            DocumentManagementController.Index("", projectPublicID, Convert.ToString(Request["folderName"]));
        }

        #region Method to Refresh the list of ServiceMatrixData by eliminating the deleted App Modules and App Roles
        /// <summary>
        /// This method is used to refresh the list of ServiceMatrixData by eliminating the deleted App Modules and App Roles
        /// </summary>
        /// <param name="lstServiceMatrixData"></param>
        /// <param name="serviceMatrix"></param>
        /// <returns></returns>
        public static List<ServiceMatrixData> RefreshAppModulesAndRoles(List<ServiceMatrixData> lstServiceMatrixData, ServiceMatrix serviceMatrix)
        {
            if (lstServiceMatrixData == null || lstServiceMatrixData.Count == 0 || serviceMatrix == null || serviceMatrix.ServiceMatrixDataList == null)
                return lstServiceMatrixData;

            List<ServiceMatrixData> lstRefreshedMatrixdata = new List<ServiceMatrixData>();
            ServiceMatrixData currentMatrixData = null;
            ServiceMatrixData defaultMatrixData = null;

            foreach (ServiceMatrixData serviceMatrixData in lstServiceMatrixData)
            {
                currentMatrixData = serviceMatrixData;
                defaultMatrixData = serviceMatrix.ServiceMatrixDataList.Where(x => x.ID == currentMatrixData.ID).Select(y => y).FirstOrDefault();

                if (defaultMatrixData != null)
                {
                    // Remove deleted App Modules
                    currentMatrixData.lstColumnsNames = currentMatrixData.lstColumnsNames.Where(x => defaultMatrixData.lstColumnsNames.Contains(x)).ToList();

                    // Remove deleted App Roles
                    currentMatrixData.lstRowsNames = currentMatrixData.lstRowsNames.Where(x => defaultMatrixData.lstRowsNames.Contains(x)).ToList();
                }

                lstRefreshedMatrixdata.Add(currentMatrixData);
            }

            return lstRefreshedMatrixdata;
        }
        #endregion Method to Refresh the list of ServiceMatrixData by eliminating the deleted App Modules and App Roles

        protected void lnkbtnAssignApprover_Click(object sender, EventArgs e)
        {
            string newApprovers = peApprover.GetValues();
            if (newApprovers != null && newApprovers != moduleInstDepny.Approver)
            {
                // Approvers added or changed by user, change status to "Waiting", notify approvers
                moduleInstDepny.Status = Constants.Waiting;
                moduleInstDepny.ApprovalStatus = TaskApprovalStatus.Pending;
                moduleInstDepny.ApprovalType = ApprovalType.All;

                //moduleInstDepny.task = string.Join(Constants.Separator, newApprovers.Select(x => x.LookupId));

                //SPList tasksList = SPListHelper.GetSPList(DatabaseObjects.Lists.TicketRelationship);
                //SPListItem taskItem = SPListHelper.GetSPListItem(tasksList, moduleInstDepny.ID);
                //if (taskItem != null)
                //    moduleInstDepny.sendEmailToApprover(taskItem, newApprovers);
            }
            else
            {
                // Approvers cleared out by user, change status to "In Progress" if not waiting for SVC approval and not waiting for predecessors
                if (moduleInstDepny.Status == Constants.Waiting && ticketCurrentStage != null && ticketCurrentStage.StageTypeChoice == StageType.Assigned.ToString() && !hasIncompletePredecessors())
                    moduleInstDepny.Status = Constants.InProgress;

                if (moduleInstDepny.Approver != null)
                {
                    moduleInstDepny.ApprovalStatus = TaskApprovalStatus.Approved;
                    moduleInstDepny.ApprovalType = ApprovalType.All;
                }
                else
                {
                    moduleInstDepny.ApprovalStatus = TaskApprovalStatus.None;
                    moduleInstDepny.ApprovalType = ApprovalType.None;
                }
            }

            moduleInstDepny.Approver = newApprovers;

            TaskManager.Save(moduleInstDepny);

            //TaskCache.ReloadProjectTasks(moduleName, projectPublicID);
            bool needTaskReminder = UGITUtility.StringToBoolean(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.EnableTaskReminder));
            // Initiate Updating the records for current task in ScheduleActions List
            if (ModuleName == ModuleNames.SVC && moduleInstDepny != null && moduleInstDepny.SubTaskType == "task")
            {
                ThreadStart starter = delegate { InitiateScheduleActionsInBackground(moduleInstDepny.ID, needTaskReminder, service); };
                Thread thread = new Thread(starter);
                thread.IsBackground = true;
                thread.Start();
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void ddlRequestTypeSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList tempdd = new DropDownList();
            string module = ddlModuleDetail.SelectedItem.Value;
            int moduleID = 0;
            int.TryParse(module, out moduleID);
            ObjModuleViewManager = new ModuleViewManager(context);
            UGITModule uGITModule = ObjModuleViewManager.LoadByName(module);
            DataRow moduleRow = UGITUtility.ObjectToData(uGITModule).Rows[0];

            string selectedSubCategory = ddlRequestTypeSubCategory.SelectedValue;
            DropDownList temp = uHelper.GetCategoryWithSubCategoriesDropDown(context, moduleRow);
            ddlRequestTypeSubCategory.Items.Clear();
            if (temp.Items.Count > 0)
                ddlRequestTypeSubCategory.Items.AddRange(temp.Items.OfType<ListItem>().ToArray());
            if (ddlRequestTypeSubCategory.Items.Count > 0)
                ddlRequestTypeSubCategory.SelectedIndex = ddlRequestTypeSubCategory.Items.IndexOf(ddlRequestTypeSubCategory.Items.FindByValue(selectedSubCategory));

            if (ddlRequestTypeSubCategory.SelectedIndex > 0)
            {

                string[] paramsData = ddlRequestTypeSubCategory.SelectedValue.Split(new string[] { ";#;" }, StringSplitOptions.None);
                string category = string.Empty;
                string subCategory = string.Empty;

                if (paramsData.Length > 0)
                    category = paramsData[0];
                if (paramsData.Length > 1)
                    subCategory = paramsData[1];
                tempdd = uHelper.GetRequestTypesWithCategoriesDropDownOnChange(context, moduleRow, category, false, false, subCategory, null);
            }
            else
            {
                tempdd = uHelper.GetRequestTypesWithCategoriesDropDown(context, moduleRow);
            }
            ddlTicketRequestType.Items.Clear();
            ddlTicketRequestType.Items.Insert(0, new ListItem("--Choose Later--", ""));
            foreach (ListItem item in tempdd.Items)
            {
                ddlTicketRequestType.Items.Add(item);
            }
        }
        // Return true if current tasks has any incomplete predecesors
        private bool hasIncompletePredecessors()
        {
            bool incompletePredecessors = false;
            List<UGITTask> taskPredecessors = TaskManager.GetTaskPredecessors(ModuleName, moduleInstDepny);
            if (taskPredecessors != null && taskPredecessors.Count > 0)
                incompletePredecessors = taskPredecessors.Any(x => x.Status != Constants.Completed && x.Status != Constants.Cancelled);
            return incompletePredecessors;
        }
        protected void ddlTicketRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string module = ddlModuleDetail.SelectedItem.Value;
            int moduleID = 0;
            int.TryParse(module, out moduleID);
            ObjModuleViewManager = new ModuleViewManager(context);
            UGITModule uGITModule = ObjModuleViewManager.LoadByName(module);
            DataRow moduleRow = UGITUtility.ObjectToData(uGITModule).Rows[0];

            string selectedSubCategory = ddlRequestTypeSubCategory.SelectedValue;
            DropDownList temp = uHelper.GetCategoryWithSubCategoriesDropDown(context, moduleRow);
            ddlRequestTypeSubCategory.Items.Clear();
            if (temp.Items.Count > 0)
                ddlRequestTypeSubCategory.Items.AddRange(temp.Items.OfType<ListItem>().ToArray());
            if (ddlRequestTypeSubCategory.Items.Count > 0)
                ddlRequestTypeSubCategory.SelectedIndex = ddlRequestTypeSubCategory.Items.IndexOf(ddlRequestTypeSubCategory.Items.FindByValue(selectedSubCategory));


            string strmoduleName = Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]);
            RequestTypeManager requestTypeManager = new RequestTypeManager(context);

            List<ModuleRequestType> selectedRTS = requestTypeManager.Load(x => x.ModuleNameLookup.EqualsIgnoreCase(strmoduleName));

            //DataRow[] selectedRTS = uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.ModuleNameLookup, strmoduleName);
            if (selectedRTS != null && selectedRTS.Count > 0)
            {
                ModuleRequestType dr = selectedRTS.FirstOrDefault(x => UGITUtility.ObjectToString(x.ID) == ddlTicketRequestType.SelectedValue);
                if (dr != null)
                {
                    string selectedCategory = dr.Category;
                    if (!string.IsNullOrWhiteSpace(dr.SubCategory))
                        selectedCategory = selectedCategory + ";#;" + dr.SubCategory;

                    if (ddlRequestTypeSubCategory.Items.Count > 0)
                        ddlRequestTypeSubCategory.SelectedIndex = ddlRequestTypeSubCategory.Items.IndexOf(ddlRequestTypeSubCategory.Items.FindByValue(selectedCategory));
                    DropDownList tempdd = new DropDownList();
                    if (ddlRequestTypeSubCategory.SelectedIndex > 0)
                    {
                        string[] paramsData = ddlRequestTypeSubCategory.SelectedValue.Split(new string[] { ";#;" }, StringSplitOptions.None);
                        string category = string.Empty;
                        string subCategory = string.Empty;

                        if (paramsData.Length > 0)
                            category = paramsData[0];
                        if (paramsData.Length > 1)
                            subCategory = paramsData[1];
                        tempdd = uHelper.GetRequestTypesWithCategoriesDropDownOnChange(context, moduleRow, category, false, false, subCategory, null);
                    }
                    else
                    {
                        tempdd = uHelper.GetRequestTypesWithCategoriesDropDown(context, moduleRow);
                    }
                    ddlTicketRequestType.Items.Clear();
                    ddlTicketRequestType.Items.Insert(0, new ListItem("--Choose Later--", ""));
                    foreach (ListItem item in tempdd.Items)
                    {
                        ddlTicketRequestType.Items.Add(item);
                    }
                    string selectedRequestTypeID = UGITUtility.ObjectToString(dr.ID);
                    if (ddlTicketRequestType.Items.Count > 0)
                        ddlTicketRequestType.SelectedIndex = ddlTicketRequestType.Items.IndexOf(ddlTicketRequestType.Items.FindByValue(selectedRequestTypeID));
                }
            }

        }
    }
}