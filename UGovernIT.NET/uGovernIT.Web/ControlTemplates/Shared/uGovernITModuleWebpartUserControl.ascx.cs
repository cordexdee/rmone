using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Manager;
using System.Collections.Specialized;
using uGovernIT.Utility;
using uGovernIT.Web.ControlTemplates.Shared;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Web.Models;
using System.Configuration;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using uGovernIT.Utility.Entities.Common;
//using OwinAuth.Identity;
//using Microsoft.AspNet.Identity;

//using Microsoft.AspNet.Identity;
//using OwinAuth.Identity;
using uGovernIT.Web.Helpers;
using Constants = uGovernIT.Utility.Constants;
using uGovernIT.Manager.Helper;
using System.Web.DynamicData;
using System.Threading;
using uGovernIT.Util.Log;
using System.Net;

namespace uGovernIT.Web
{
    public partial class uGovernITModuleWebpartUserControl : UserControl
    {
        #region configuration datatables
        protected RequesetTypeDependentModel requestTypeDependent;
        public LifeCycle lifeCycle;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        EscalationProcess objEscalationProcess = null;
        ModuleViewManager ObjModuleViewManager = null;
        TicketManager ObjTicketManager = null;
        ConfigurationVariableManager objConfigurationVariableHelper = null;
        ModuleFormTabManager ObjModuleFormTabManager = null;
        RequestTypeManager ObjRequestTypeManager = null;
        //CRMProjectAllocationManager ObjCRMProjectAllocationMgr = null;
        ProjectEstimatedAllocationManager ObjCRMProjectAllocationMgr = null;
        RelatedCompanyManager ObjRelatedCompanyMgr = null;

        #endregion
        #region Helper Variables
        protected TicketControls ticketControls;
        public ResetPasswordAgent resetPasswordAgent;
        public bool isShowResetPasswordErrorPopup = false;
        public string restpasswordagentmessage = string.Empty;
        public string ResetPasswordCloseMessage;
        public AgentSummary agentSummary;
        public AgentSummary agentSummaryOnLoad;
        protected DataTable TicketSchema;
        protected DataRow[] thisList;
        protected DataRow currentTicket;
        protected DataRow newTicket;
        protected DataRow saveTicket;
        public bool enableStudioDivisionHierarchy;

        protected long newTicketStageId;

        protected int currentTicketId;
        protected int moduleId = 1;

        public string currentTicketPublicID;
        public string currentModuleName;
        public string agentTitle;
        public string description;
        public string agentType;
        public string PhraseSearch;


        protected string PRP = string.Empty;
        protected string currentModulePagePath;
        protected string currentModuleListPagePath;
        protected string incidentTicketModulePagePath;
        protected string[] categories;


        //Current Ticket stage variables
        protected int currentStep;
        protected int ticketOnHold;

        protected string defaultStartStage;
        protected string returnStageId = string.Empty;
        protected string currentStageActionUserType;
        protected string authorizedToEditUsers;

        protected bool isActionUser;
        protected bool isAuthorizedToViewTicket;
        protected bool printEnable;

        //List<UserInfo> authorizedToViewModuleUsers;



        //action button variables
        protected bool showApproveButton;
        protected bool showRejectButton;
        protected bool showHoldUnholdButton;
        protected bool showBaselineButtons;
        protected bool showCreateAssetButton;
        protected bool showSelfAssignButton;
        protected bool showCreateIncidentButton;
        protected bool showAdminEditButton;
        protected bool adminOverride;

        //protected bool quickClose;
        //protected bool versionedFieldChanged;
        protected string currentCreateTSR;

        private string approveButtonName;
        private string rejectButtonName;
        private string approveButtonImage;
        private string rejectButtonImage;
        private string returnButtonName;

        private TicketTemplateManager _ticketTemplateManager = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private TicketManager _ticketManager = null;
        private ModuleFormTabManager _moduleFormTabManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private EscalationProcess _escalationProcess = null;
        private AvailablePRPAndAssignTo _availablePRPAndAssignTo = null;
        private StatisticsManager _statisticsManager = null;


        protected bool showReturnButton = true;
        protected bool showDraftButton = false;
        protected bool IsAdmin;
        protected bool IsItemConverted = false;

        protected Button uploadButton;

        // default variables
        private DateTime baselineDate;

        protected Table tabTable;
        protected Table groupedTabTable;
        protected Table tableViewTable;

        protected int tableViewRowCount;
        protected int idColumn;
        protected static int totalColumnsInDisplay = 3;

        protected double totalModuleWeight;
        protected double currentTicketStageWeight;

        protected FileUpload attachment;
        //private SPAttachmentCollection attachments = null;
        protected Label lblCategoryType;
        protected Label lblCategoryTypeNew;

        protected bool setAlternateStageGraphicLabel;

        protected string moduleTypeTitle;
        //private bool rendering;

        //Import IDs variables
        protected string requestOwnerId;
        protected string requestorContainerId;
        protected string relatedAssetContainerId;
        protected string requestOwnerIdDisplayed;
        protected string priorityIdDisplayed;
        protected string incidentOwnerId;
        protected string priorityId;

        //NPR related Variables
        protected string nprId;
        protected string nprDescription;
        protected string nprTitle;
        protected string nprManager;

        protected string missingMandatoryFields;

        protected string requestTypeList = DatabaseObjects.Tables.RequestType;

        // Used to track JUST invalid input, not missing input
        // Valid is used in Btn_Click to track both
        protected bool invalidInput;

        protected string ajaxHelper = string.Empty;
        private string changedFields;
        private bool actionUserChanged = false;
        protected bool holdCommentAlwaysMandatory;
        protected string pmmBudgetReportUrl;   //To store the budget report url.
        protected string pmmActualsReportUrl;   //To store the budget report url.
        protected string pmmReportUrl; //To store the pmm project report url.
        protected string pmmResourceReportUrl;
        protected string tskReportUrl; //To store the TSK List project report url.
        protected string itgBudgetReportUrl;   //To store the budget report url.
        protected string itgActualsReportUrl;   //To store the budget report url.

        public string issueType = "";
        public string resolutionType = "";
        protected string userLocation = string.Empty;
        protected string requestorLocation = string.Empty;

        protected string[] ItemConversionMessage = new string[2];   //To store Item Conversion message, with Hyperlink and header Text.
        //protected int templateID = 0;
        protected long templateID = 0;
        //Added by mudassir 10 march 2020
        public bool commentsDefaultToPrivate;
        protected int closeoutperiod = 0;
        //
        public int ModuleId
        {
            get
            {
                return moduleId;
            }
            set
            {
                moduleId = value;
            }
        }

        protected DropDownList ddlCategory = new DropDownList();
        protected DropDownList ddlRequestType = new DropDownList();
        protected DropDownList ddlTicketProjectRelated = new DropDownList();
        protected DropDownList ddlTicketProjectReference = new DropDownList();
        protected Label lblProjectDescription = new Label();
        protected bool isCategoryToBeDisplayed = false;
        protected bool isDisableOwnerBinding = false;

        protected Ticket TicketRequest;
        private LifeCycleStage currentStage;

        protected string LifeCycleStageURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=lifecyclestage");
        protected string TicketEmailURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ticketemail");
        protected string SaveAsTemplateURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplate");
        protected string editCommentURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=editcomment");
        protected string userDetailURL = UGITUtility.GetAbsoluteURL("/ControlTemplates/RMM/userinfo.aspx?");
        protected string printtabpagebreak = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=printtabpagebreak");
        protected string compactViewUrl = "/Layouts/uGovernIT/DelegateControl.aspx?ctrl=PMM.ProjectCompactView&TicketId={0}";
        private const string absoluteUrlView = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&TicketId={2}&Type={3}&Module={4}&ticketrelation=1";
        //Add for agent
        protected string NewAllocationUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=crmprojectallocationaddedit&ID=0");
        protected string ProjectExternalTeamUrl = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?control=relatedcompanies");
        public string NewProjectUrl = UGITUtility.GetAbsoluteURL("/Pages/CPR?moduleFrom=OPM");
        public string NewOpportunityUrl = UGITUtility.GetAbsoluteURL("/Pages/OPM?moduleFrom=LEM");
        public string NewCompanyUrl = UGITUtility.GetAbsoluteURL("/Pages/COM?");
        public string NewContactUrl = UGITUtility.GetAbsoluteURL("/Pages/AddContact?");

        public string AgentProjectFieldsUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=AgentProjectFields");

        public string strProjectExternalUrl = UGITUtility.GetAbsoluteURL("/Pages/CPR?moduleFrom=OPM");
        //public string LeadPriorityUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&showSearchOption=false&control=RankingCriteriaView&isreadonly=False&ticketId=");

        protected string pmmprojectERHReportUrl;
        Dictionary<string, string> fieldValuesFromUrl = new Dictionary<string, string>();
        //new variable;
        protected string strStageType;
        private Boolean IsRequestTypeChange = false;
        protected double width;

        //new variable;
        public static DataRow[] spListitemCollection;
        public List<string> TicketIdList { get; set; }
        protected const string Varies = "<Value Varies>";
        protected string elevatedPrioirty;
        private const string SELECTEDTABCOOKIE = "TicketSelectedTab";
        private string isPageBreakup;
        private string selectedTabs;
        private string oldProjectId = string.Empty;

        private DateTime oldTargetCompletionDate = DateTime.MinValue.Date;
        private DateTime newtargetcompletiondate = DateTime.MinValue.Date;
        //private bool notificationdontsend;
        protected bool preloadAllModuleTabs;
        private bool isFieldLevelAccessIsPersent;
        protected bool confirmChildTicketsClose;
        protected showInMobile showInMobile = null;
        protected List<string> uPRPorORPList = new List<string>();
        // protected SPFieldUserValueCollection newPRPorORPUsers = new SPFieldUserValueCollection();
        protected bool isCurrentStageAssignedStage;
        protected string PRPGroup = string.Empty;
        DataRow copyTicket;
        public string ServiceURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=serviceswizard&serviceID=");
        public string TicketManualEscalationUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=ManualEscalation");
        public string ServiceTaskWorkFlow = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=servicetaskworkfLow");
        public string openCloseTicketsForRequestorUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=openandcloseforrequestor");
        protected string clipboardUrl = string.Empty;
        public bool IsDuplicate;
        public string TrackProjectStageUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=trackprojectstagehistory");
        public bool IsNewSubticket;
        public UserProfile user;
        public UserProfileManager UserManager;
        private string TenantID = TenantHelper.GetTanantID();
        private FieldConfigurationManager _fieldConfigurationManager = null;
        ModuleViewManager moduleViewManager = null;

        public StringBuilder sbText = new StringBuilder();
        public Label lblText = null;
        List<ModuleFormLayout> alllayoutItems = null;
        protected string ajaxHelperPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        public string ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/RMOne/");
        protected DataTable resetPasswordUserList = new DataTable();
        public bool? isAddREsolutionTime = false;
        public String strResolutionTime = String.Empty;
        public string reportUrl = string.Empty;
        protected string userId = HttpContext.Current.CurrentUser().Id;
        protected bool TrackProjectStageHistory;
        public string pickerListUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=listpicker");
        FieldConfigurationManager configFieldManager;
        public int ShortNameLength;
        public bool IsCMICMandatory { get; set; }
        #endregion


        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fieldConfigurationManager == null)
                {
                    _fieldConfigurationManager = new FieldConfigurationManager(context);
                }
                return _fieldConfigurationManager;
            }
        }

        protected TicketTemplateManager TicketTemplateManager
        {
            get
            {
                if (_ticketTemplateManager == null)
                {
                    _ticketTemplateManager = new TicketTemplateManager(context);
                }
                return _ticketTemplateManager;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {

            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(context);
                }
                return _configurationVariableManager;
            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(context);
                }
                return _ticketManager;
            }
        }

        protected ModuleFormTabManager ModuleFormTabManager
        {
            get
            {
                if (_moduleFormTabManager == null)
                {
                    _moduleFormTabManager = new ModuleFormTabManager(context);
                }
                return _moduleFormTabManager;
            }

        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(context);
                }
                return _moduleViewManager;
            }
        }

        protected EscalationProcess EscalationProcess
        {
            get
            {
                if (_escalationProcess == null)
                {
                    _escalationProcess = new EscalationProcess(context);
                }
                return _escalationProcess;
            }
        }

        protected AvailablePRPAndAssignTo AvailablePRPAndAssignTo
        {
            get
            {
                if (_availablePRPAndAssignTo == null)
                {
                    _availablePRPAndAssignTo = new AvailablePRPAndAssignTo(context);
                }
                return _availablePRPAndAssignTo;
            }
        }
        public bool HasAnyPastAllocation
        {
            get
            {
                if(currentTicket!=null)
                    return uHelper.HasAnyPastAllocation(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.TicketId]));
                else
                    return false;
            }
        }

        #region Method

        protected override void OnInit(EventArgs e)
        {
            objEscalationProcess = new EscalationProcess(context);
            ObjModuleViewManager = new ModuleViewManager(context);
            ObjTicketManager = new TicketManager(context);
            objConfigurationVariableHelper = new ConfigurationVariableManager(context);
            ObjModuleFormTabManager = new ModuleFormTabManager(context);
            moduleViewManager = new ModuleViewManager(context);
            ObjRequestTypeManager = new RequestTypeManager(context);
            ObjCRMProjectAllocationMgr = new ProjectEstimatedAllocationManager(context);
            ObjRelatedCompanyMgr = new RelatedCompanyManager(context);
            showInMobile = new showInMobile(context);
            TrackProjectStageHistory = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.TrackProjectStageHistory);
            configFieldManager = new FieldConfigurationManager(context);
            _statisticsManager = new StatisticsManager(context);

            if (Request["PhraseSearch"] == "1")
            {
                this.agentTitle = Request["Title"];
                this.description = Request["Description"];
                this.agentType = Request["Agenttype"];
                this.PhraseSearch = Request["PhraseSearch"];
            }

            holdCommentAlwaysMandatory = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.HoldCommentAlwaysMandatory);
            enableStudioDivisionHierarchy = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.EnableStudioDivisionHierarchy);

            ajaxHelper = UGITUtility.GetAbsoluteURL("/api/account/");
            if (Page.User.Identity.IsAuthenticated)
            {
                user = HttpContext.Current.CurrentUser();
                UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            }

            fieldValuesFromUrl = GetFieldValuesFromUrl(Request);
            aspxdtOnHoldDate.MinDate = DateTime.Today.AddDays(1);
            aspxdtOnHoldDate.EditFormat = EditFormat.Date;
            aspxdtOnHoldDate.TimeSectionProperties.Visible = false;

            commentsDefaultToPrivate = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.CommentsDefaultToPrivate);
            if (!chkAddPrivate.Checked)
                chkAddPrivate.Checked = commentsDefaultToPrivate;

            if (!string.IsNullOrEmpty(Request["Width"]))
            {
                width = Convert.ToDouble(Request["Width"]);
            }
            //Duplicate Ticket
            if (Request["Duplicate"] != null)
                IsDuplicate = true;

            if (Request["NewSubticket"] != null)
                IsNewSubticket = true;

            // Override the moduleId if it is coming in request to create a new ticket for another module.
            if (Request["TargetModuleId"] != null)
                this.moduleId = Convert.ToInt32(Request["TargetModuleId"]);

            if (Request["Module"] != null)
                currentModuleName = Convert.ToString(Request["Module"]);

            //Set Configuration tables 
            SetConfigurationTables();
            //set whether to set alternate graphic label or not
            setAlternateStageGraphicLabel = ConfigurationVariableManager.GetValueAsBool("AlternateLabel");
            //Check printout required or not
            if (Request["enablePrint"] != null)
                printEnable = true;

            if (Request["isPageBreakup"] != null)
            {
                isPageBreakup = Request["isPageBreakup"];
                hdnpagebreakup.Value = isPageBreakup;
                selectedTabs = Request["selectedTabs"];
            }

            if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
            {

                NewTicketWorkFlowDiv.Visible = false;
                topGraphicDiv.Visible = false;
                ASPxPopupActionMenu.PopupVerticalAlign = PopupVerticalAlign.Below;
                ASPxPopupActionMenu.PopupHorizontalAlign = PopupHorizontalAlign.RightSides;
            }



            if (Request["TemplateId"] != null && Convert.ToInt64(Request["TemplateId"]) > 0)
            {
                templateID = Convert.ToInt64(Request["TemplateId"]);
            }

            if (Request[eAll.UniqueID] == "1")
                adminOverride = true;

            GetDefaultData();
            string userActivityLogMsg;
            if (currentTicketId == 0)
            {

                btnDiv.Visible = false;
                if (templateID > 0)
                {
                    CreateNewTicketFromTemplate(templateID);
                    userActivityLogMsg = $"{context.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Started New Template Record: {currentModuleName}";
                }
                else
                {
                    CreateNewTicket();
                    userActivityLogMsg = $"{context.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Started New Record: {currentModuleName}";
                }
            }
            else
            {
                userActivityLogMsg = $"{context.TenantAccountId}|{HttpContext.Current.CurrentUser()?.Name}: Opened Record: {currentTicketPublicID}";
            }
            if (!IsPostBack)
                Util.Log.ULog.WriteLog(userActivityLogMsg);

            if (!IsPostBack && !string.IsNullOrEmpty(currentModuleName) && currentModuleName != "ITG")
            {
                //if (currentTicketId > 0 && !string.IsNullOrEmpty(currentTicketPublicID))
                //    Log.AuditTrail(SPContext.Current.Web.CurrentUser, string.Format("opened ticket {0}", currentTicketPublicID), Request.Url);
                //else if (!string.IsNullOrEmpty(currentModuleName))
                //    Log.AuditTrail(SPContext.Current.Web.CurrentUser, string.Format("started creating new {0} ticket", currentModuleName), Request.Url);
                //else
                //    Log.AuditTrail(SPContext.Current.Web.CurrentUser, Request.Url);
            }

            //if(currentModuleName == "SVC")
            //{
            //    aGotoworkflow.Visible = true;
            //}

            BindTabs();
            //Bind tabs detail
            if (currentTicket != null)
            {
                int prePostbackStageStep = 0;
                if (IsPostBack && int.TryParse(Request[hdnTicketCurrentStage.UniqueID], out prePostbackStageStep) && prePostbackStageStep != currentStage.StageStep)
                {
                    //throw new UGITException(string.Format("This {0} has been updated by another user, please re-open and then retry your changes.", uHelper.moduleTypeName(currentModuleName).ToLower()));
                }

                ////new for get the all ticket data.
                if (Request.QueryString["AllTickets"] != null && Request.QueryString["BatchEditing"] == "true")
                {
                    TicketIdList = Convert.ToString(Request["AllTickets"]).Split(new string[] { "," }, StringSplitOptions.None).ToList();
                    LoadTicketsCollection();
                }


                if (Request["BatchEditing"] == "true")
                {
                    ticketMsgContainer.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                if (ddlOnHoldReason.Items.Count == 0 && UGITUtility.IfColumnExists(DatabaseObjects.Columns.OnHoldReasonChoice, currentTicket.Table))
                {
                    UGITModule moduleObj = ObjModuleViewManager.GetByName(currentModuleName);
                    FieldConfiguration configField = configFieldManager.GetFieldByFieldName(DatabaseObjects.Columns.OnHoldReasonChoice, moduleObj.ModuleName);
                    if (configField != null)
                    {
                        string[] dataRequestSource = UGITUtility.SplitString(configField.Data, uGovernIT.Utility.Constants.Separator);
                        ddlOnHoldReason.DataSource = dataRequestSource.ToList();
                        ddlOnHoldReason.DataBind();
                    }
                    string onholdreason = UGITUtility.ObjectToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.OnHoldReasonChoice));
                    if (!string.IsNullOrEmpty(onholdreason))
                    {
                        if (ddlOnHoldReason.Items.IndexOf(ddlOnHoldReason.Items.FindByText(onholdreason)) < 0)
                        {
                            ddlOnHoldReason.Items.Add(onholdreason);
                        }
                        ddlOnHoldReason.SelectedIndex = ddlOnHoldReason.Items.IndexOf(ddlOnHoldReason.Items.FindByText(onholdreason));
                        txtOnHoldReason.Text = onholdreason;
                    }
                }
                if (ticketOnHold == 1)
                {
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketOnHoldTillDate, currentTicket.Table))
                    {
                        aspxdtOnHoldDate.Date = UGITUtility.StringToDateTime(UGITUtility.ObjectToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketOnHoldTillDate)));
                    }
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketComment, currentTicket.Table))
                    {
                        string allComments = UGITUtility.ObjectToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketComment));
                        string comment = string.Empty;
                        if (!string.IsNullOrWhiteSpace(allComments))
                        {
                            comment = allComments.Split(new string[] { Constants.SeparatorForVersions }, StringSplitOptions.None).ToList().Where(x => x.Contains("On Hold Till")).LastOrDefault();
                            if (!string.IsNullOrWhiteSpace(comment) && comment.Split(new string[] { "Comment:" }, StringSplitOptions.None).Length > 1)
                            {
                                popedHoldComments.Text = comment.Split(new string[] { "Comment:" }, StringSplitOptions.None)[1];
                            }
                        }
                    }
                }
                else
                {
                    aspxdtOnHoldDate.Date = DateTime.Now.AddDays(1);
                }
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.IconBlob))
                {
                    object imageBytes = UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.IconBlob);
                    if (imageBytes != null && imageBytes != DBNull.Value)
                        imgTicketIcon.ImageUrl = "data:image;base64," + Convert.ToBase64String((byte[])imageBytes);
                    else
                    {
                        string defaultIconUrl = ConfigurationVariableManager.GetValue(Constants.ProjectIcon);
                        if (!string.IsNullOrEmpty(defaultIconUrl) && System.IO.File.Exists(Server.MapPath(defaultIconUrl)))
                            imgTicketIcon.ImageUrl = defaultIconUrl;
                        else
                            imgTicketIcon.ImageUrl = "/Content/Images/Project.jpg";
                    }
                }
            }

            //Delete cookie budgetyear which is set to show budget distribution on seleted year in NPR project.
            //Cookie keep the selected year to show same distribution in print mode.
            if (!printEnable)
                UGITUtility.DeleteCookie(Request, Response, "budgetyear");

            if (!IsPostBack)
            {
                ////it will refresh the state of document library control
                //DocumentHelper.ClearCookies(Request, Response);
                //UGITTaskHelper.ClearCookies(Request, Response, currentTicketPublicID);
            }
            strProjectExternalUrl = string.Format("{0}&ticketId={1}", ProjectExternalTeamUrl, currentTicketPublicID);
            trchkEnableCloseOnHoldExpire.Visible = TicketRequest.Module.EnableCloseOnHoldExpire;
            var list = (List<string>)Session["relatedTicket"];

            if (Session["isRequestTaskCompleted"] != null)
            {
                if (Session["isRequestTaskCompleted"].ToString() == "true")
                {
                    Button_Click(btnGroupActioner, EventArgs.Empty);
                }
            }

            var moduleData = ModuleViewManager.LoadByName(currentModuleName);

            if (moduleData != null)
            {
                if (moduleData.EnableBaseLine)
                    showBaselineButtons = moduleData.EnableBaseLine;
                if (moduleData.EnableLinkSimilarTickets.HasValue)
                    relatedTitleButton.Visible = moduleData.EnableLinkSimilarTickets.Value;
                if (moduleData.EnableIcon)
                    imgTicketIcon.Visible = true;
            }
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
        }
        /// <summary>
        /// Page load function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            closeoutperiod = uHelper.getCloseoutperiod(context);
            //string listpickerUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=listpicker");
            clipboardUrl = string.Format("{1}?TicketId={2}&ModuleName={3}", UGITUtility.GetAbsoluteURL(ConfigurationManager.AppSettings["apiBaseUrl"]), UGITUtility.GetAbsoluteURL(uGovernIT.Utility.Constants.HomePagePath), currentTicketPublicID, currentModuleName);
            reportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            BindModuleFormTab();
            ShortNameLength = UGITUtility.StringToInt(ConfigurationVariableManager.GetValue(ConfigConstants.ShortNameCharacters));

            if (!Page.IsPostBack)
            {
                int selectedTabFromEmail = 0;
                if (Session["tabDetailFromEmail"] != null)
                {
                    selectedTabFromEmail = Convert.ToInt32(Session["tabDetailFromEmail"]);
                }
                if (Session["isFromSuprAdmin"] == null)
                {
                    Session.Abandon();
                }

                
                compactViewUrl = UGITUtility.GetAbsoluteURL(string.Format(compactViewUrl, currentTicketPublicID));

                int selectedTab = 0;
                if (hdnActiveTab.Value == string.Empty)
                {
                    string cookieValue = UGITUtility.GetCookieValue(Request, SELECTEDTABCOOKIE);
                    selectedTab = 0;
                    if (currentStage != null)
                        selectedTab = currentStage.SelectedTabNumber;
                    //selectedTab = currentStage.SelectedTab;
                    string activeTabCookie= UGITUtility.GetCookieValue(Request, "currentActiveTab");                   
                    if (Request["showTab"] != null)
                    {                        
                        if (!String.IsNullOrEmpty(activeTabCookie))
                        {
                            int.TryParse(activeTabCookie.Trim(), out selectedTab);
                            UGITUtility.DeleteCookie(Request, Response, "currentActiveTab");
                        }
                        else
                        {
                            int.TryParse(Request["showTab"].Trim(), out selectedTab);
                        }
                    }
                    else if (selectedTabFromEmail > 0)
                    {
                        selectedTab = selectedTabFromEmail;
                    }
                    else if (!string.IsNullOrWhiteSpace(cookieValue))
                    {
                        int.TryParse(cookieValue, out selectedTab);
                        UGITUtility.DeleteCookie(Request, Response, SELECTEDTABCOOKIE);
                    }

                    if (selectedTab <= 0)
                        selectedTab = 0;

                    hdnActiveTab.Value = selectedTab.ToString();
                }

                if (moduleContainer.Visible)
                {
                    //Bind Menu, workflow graphic, initialize action buttons
                    if (currentTicket != null)
                    {
                        BindMenu();
                        BindStageGraphic();//Comment to check  by Munna on 17-04-2017
                        InitializeButtons();
                    }
                    else
                    {
                        createButton.Visible = true;
                        createButton.Text = "Create";
                        createButton.CssClass += " linkbutton";
                    }

                    switch (currentModuleName.ToLower())
                    {
                        case "itg":
                            cancelButtonLI.Visible = false;
                            break;
                        default:
                            break;
                    }

                    // Remove duplicate file upload from new ticket or existing ticket
                    if (currentTicketId <= 0)
                    {
                        //int maxNoOfFiles = 1; // Convert.ToInt32(objConfigurationVariableHelper.GetValue("NewTicketMaxFiles"));
                        //List<string> attachedFiles = new List<string>();
                        //for (int i = 0; i < maxNoOfFiles; i++)
                        //{
                        //    FileUpload itemAttachment = (FileUpload)this.FindControl("fileupload" + i);
                        //    if (itemAttachment != null && itemAttachment.HasFile)
                        //    {
                        //        if (itemAttachment.HasFile && attachedFiles.FirstOrDefault(x => x == itemAttachment.FileName) == null)
                        //        {
                        //            attachedFiles.Add(itemAttachment.FileName);
                        //        }
                        //        else
                        //        {
                        //            itemAttachment.PostedFile.InputStream.Close();
                        //            itemAttachment.PostedFile.InputStream.Dispose();
                        //        }
                        //    }
                        //}
                    }
                    else if (attachment != null && attachment.HasFile)
                    {
                        List<string> uploadedFiles = new List<string>();
                        //if (currentTicket != null)
                        //{
                        //     uploadedFiles = currentTicket.Attachments.Cast<string>().ToList();
                        //}
                        //else if (saveTicket != null)
                        //{
                        //      uploadedFiles = saveTicket.Attachments.Cast<string>().ToList();
                        //}

                        if (uploadedFiles != null && uploadedFiles.FirstOrDefault(x => x == attachment.PostedFile.FileName) != null)
                        {
                            attachment.PostedFile.InputStream.Close();
                            attachment.PostedFile.InputStream.Dispose();
                        }
                    }

                    if (currentTicketId > 0)
                    {
                        string message = string.Empty;
                        bool isAllTaskComplete = UGITModuleConstraint.GetPendingConstraintsStatus(currentTicketPublicID, currentStep, ref message, context);
                        if (isAllTaskComplete)
                        {
                            areAllTaskComplete.Value = uGovernIT.Utility.Constants.Completed;

                        }
                        else
                        {
                            areAllTaskComplete.Value = uGovernIT.Utility.Constants.Pending;
                            taskNames_Pending.Value = message;
                        }

                        //new line for stages...
                        strStageType = currentStage.StageTypeChoice;
                    }

                    //for macro..
                    //BindTabs();

                    //Display help text if exist
                    try
                    {
                        uHelper.DisplayHelpTextLink(context, currentModuleName, helpTextContainer, HelpTextNewTicket);
                        if (helpTextContainer.Visible)
                            helpTextWrap.Visible = true;
                        else
                            helpTextWrap.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        helpTextWrap.Visible = false;
                        uGovernIT.Util.Log.ULog.WriteException(ex, "Could not load Helper icon - ModulesWebPartUserControl");
                    }
                }
            }

            // when moving opportunity to precon
            if (currentModuleName == "OPM")
            {
                BindModuleStages("CPR");

                var oppStatus = FieldConfigurationManager.Load(x => x.FieldName.Equals(DatabaseObjects.Columns.CRMOpportunityStatus, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Data).FirstOrDefault().Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);

                if (oppStatus != null)
                {
                    if (ddlOpportunityStatus.Items.Count == 0)
                    {
                        ddlOpportunityStatus.DataSource = oppStatus;
                        ddlOpportunityStatus.DataBind();
                    }
                }
            }

            if (showBaselineButtons)
            {
                createBaselineButtonH.ID = "createBaseline";
            }
            if (currentModuleName != ModuleNames.PMM)
            {

                divCompactViewImg.Visible = false;
                compactViewContainer.Visible = false;
            }
            if (currentTicketId > 0)
            {
                strStageType = currentStage.StageTypeChoice;
            }

        }

        private void BindModuleStages(string moduleName)
        {
            ddlStageToMove.Items.Clear();

            long selectedId = -1;

            Ticket ticketCPR = new Ticket(context, "CPR");
            if (ticketCPR.Module.List_LifeCycles != null && ticketCPR.Module.List_LifeCycles.Count > 0)
            {
                List<LifeCycleStage> stages = ticketCPR.Module.List_LifeCycles[0].Stages.Where(x=>x.StageTypeChoice != "Closed").ToList();
                if (stages == null)
                    return;
                foreach (LifeCycleStage lcStage in stages)
                {
                    if (lcStage.CustomProperties.ToLower() == "awardstage")
                    {
                        selectedId = lcStage.ID;
                    }
                    ddlStageToMove.Items.Add(new ListItem(lcStage.Name, Convert.ToString(lcStage.ID)));
                }
            }

            if (selectedId > -1)
            {
                ddlStageToMove.SelectedValue = Convert.ToString(selectedId);
            }
        }

        /// <summary>
        /// Gets all the lists, default data and permissions for the webpart.
        /// </summary>
        private void GetDefaultData()
        {
            //Reset important variables value
            isActionUser = false;
            showApproveButton = false;
            showRejectButton = false;
            showHoldUnholdButton = false;
            showReturnButton = false;
            showDraftButton = false;
            showAdminEditButton = false;

            //showBaselineLI.Visible = false;
            //createBaselineLI.Visible = false;
            isAuthorizedToViewTicket = true;

            //quickClose = false;
            ticketOnHold = 0;

            unauthorizedPanel.Visible = false;
            moduleContainer.Visible = true;

            //SPWeb thisWeb = SPContext.Current.Web;

            moduleTypeTitle = UGITUtility.moduleTypeName(currentModuleName);

            UGITModule module = ModuleViewManager.LoadByName(currentModuleName);
            DataTable thisListTable = TicketManager.GetTableSchemaDetail(module.ModuleTable, string.Empty);
            //thisList = SPListHelper.GetSPList(TicketRequest.Module.ModuleTicketTable);
            thisList = thisListTable.Select();
            TicketSchema = thisListTable;


            saveAsDraftButtonContainer.Visible = false;

            //Set default value of currentstep number, newticketstageId and currentStageId
            //Which will be first stage by default;
            lifeCycle = TicketRequest.Module.List_LifeCycles.FirstOrDefault();
            //lifeCycle = TicketRequest.Module.List_LifeCycles.FirstOrDefault(x => x.Name == DatabaseObjects.Columns.ProjectLifeCycleLookup);
            if (lifeCycle != null && lifeCycle.Stages.Count > 0)
            {
                currentStep = lifeCycle.Stages[0].StageStep;
                newTicketStageId = lifeCycle.Stages[0].ID;
                currentStage = lifeCycle.Stages[0];
                totalModuleWeight = lifeCycle.Stages.Sum(x => x.StageWeight);
            }



            #region Set ticket ID
            if (!string.IsNullOrEmpty(Request[DatabaseObjects.Columns.TicketId]) || !string.IsNullOrEmpty(Request[DatabaseObjects.Columns.Id]))
            {
                string tID = Convert.ToString(Request[DatabaseObjects.Columns.TicketId]).Trim();
                if (string.IsNullOrEmpty(Request[DatabaseObjects.Columns.TicketId]))
                {
                    tID = Convert.ToString(Request[DatabaseObjects.Columns.Id]).Trim();
                }

                if (!int.TryParse(tID, out currentTicketId))
                {
                    if (context.TenantID != context.CurrentUser.TenantID)
                    {
                        context.TenantID = context.CurrentUser.TenantID;
                    }
                    DataRow ticketCollection = Ticket.GetCurrentTicket(context, TicketRequest.Module.ModuleName, tID);
                    currentTicket = null;
                    currentTicketId = 0;
                    currentTicketPublicID = string.Empty;
                    try // Faster than if (ticketCollection.Count > 0)
                    {
                        currentTicketId = Convert.ToInt32(ticketCollection["ID"]);
                        currentTicket = ticketCollection;
                        currentTicketPublicID = Convert.ToString(ticketCollection["TicketId"]);

                    }
                    catch (Exception ex)
                    {
                        //Log.WriteException(ex, "Error getting ticket");
                        Util.Log.ULog.WriteException(ex);
                    }
                }
                else
                {
                    DataRow ticketCollection = Ticket.GetCurrentTicket(context, TicketRequest.Module.ModuleName, tID);
                    currentTicket = ticketCollection;
                }
            }
            else if (currentModuleName.Equals("ITG", StringComparison.CurrentCultureIgnoreCase))
            {
                //For ITG currentTicketID always be default ticket 1
                currentTicketId = 1;
                panelDetail.Visible = true;
                panelNewTicket.Visible = false;
                //TicketNoLiteral.Visible = false;
                //currentTicket = SPListHelper.GetSPListItem(thisList, currentTicketId);
            }
            else if (currentTicketIdHidden != null && currentTicketIdHidden.Value != "0")
            {
                currentTicketId = int.Parse(currentTicketIdHidden.Value);
                //currentTicket = SPListHelper.GetSPListItem(thisList, currentTicketId);
            }

            #endregion

            if (currentTicketId > 0 && currentTicket == null)
            {
                // Invalid Url if ticket is not exist
                currentTicketId = 0;
                currentTicket = null;
                Response.Write("Invalid URL: ticket not found!");
                Response.End();
            }
            //SetAuthorization();   //temp : MK
            //Get currentTicket if null
            //Should we move this near line no 360 ?
            if (currentTicketId > 0)
            {
                currentTicketIdHidden.Value = currentTicketId.ToString();
                panelDetail.Visible = true;
                panelNewTicket.Visible = false;
                currentTicketPublicID = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);

                //Fetch lifecycle if module is pmm
                //if (currentModuleName.ToLower() == "pmm")
                //{
                string lifeCycleName = string.Empty;

                if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.AgentSummary))
                {
                    agentSummaryOnLoad = Newtonsoft.Json.JsonConvert.DeserializeObject<AgentSummary>((currentTicket[DatabaseObjects.Columns.AgentSummary]).ToString());


                }

                if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.ProjectLifeCycleLookup))
                {
                    LifeCycleManager lcHelper = new LifeCycleManager(context);
                    lifeCycle = lcHelper.LoadProjectLifeCycles().FirstOrDefault(x => x.ID == Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup])); // TicketRequest.Module.List_LifeCycles.FirstOrDefault(x => x.ID ==  Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ProjectLifeCycleLookup]));
                    if (lifeCycle != null)
                    {
                        totalModuleWeight = lifeCycle.Stages.Sum(x => x.StageWeight);
                        string pmoGroup = ConfigurationVariableManager.GetValue(ConfigConstants.PMOGroup);
                        if (UserManager.CheckUserIsInGroup(pmoGroup, UserManager.GetUserByUserName(Context.User.Identity.Name)))
                        {
                            divLifecycle.Visible = true;
                            lblLifecycleText.Text = lifeCycle.Name;
                        }
                        else
                        {
                            divLifecycle.Visible = false;
                            lblLifecycleText.Text = lifeCycle.Name;
                        }
                    }
                }
                // }

                //Check if current ticket is on Hold
                if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketOnHold))
                    ticketOnHold = UGITUtility.StringToInt(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketOnHold));

                //Fetch current stage detail based on modulestep
                if (lifeCycle != null && UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.StageStep))
                {
                    int StageStep = 0;
                    if (int.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.StageStep)), out StageStep))
                    {
                        currentStep = StageStep;
                        currentStage = lifeCycle.Stages.FirstOrDefault(x => x.StageStep == StageStep);
                    }
                }

                // Safety net to prevent crash
                if (currentStage == null && lifeCycle != null && lifeCycle.Stages.Count > 0)
                    currentStage = lifeCycle.Stages[0];

                // Get current Stage description which will be shown in message box
                if (currentStage != null && currentStage.UserPrompt != null && currentModuleName != "PMM")
                {
                    currentStageDescriptionLiteral.Text = currentStage.UserPrompt + "<br />";
                }

                hdnTicketCurrentStage.Value = currentStage.StageStep.ToString();

                //Get Workflow status of current ticket
                TicketWorkflowStatusLiteral.Text = currentStage.UserWorkflowStatus;
                // TicketNoLiteral.Text = Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketId));

                //Stage percentage complete - shows up at the top status bar.
                if (lifeCycle != null && lifeCycle.Stages.Count > 0 && currentStage != null)
                {
                    currentTicketStageWeight = lifeCycle.Stages.Where(x => x.StageStep < currentStage.StageStep).Sum(x => x.StageWeight);
                }

                if (currentModuleName == "PMM")
                {
                    double pctComplete = 0D;
                    Double.TryParse(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketPctComplete]), out pctComplete);
                    if (pctComplete > 0.999 && pctComplete < 1.000)
                        pctComplete = 99.9; // Don't show 100% unless all the way done!
                    else
                        pctComplete = Math.Floor(pctComplete); //Math.Round(pctComplete * 100, 1, MidpointRounding.AwayFromZero); // Round to nearest 0.1

                    if (ticketOnHold == 1)
                        TicketWorkflowStatusLiteral.Text = string.Format("On Hold ({0}% complete)", pctComplete);
                    else
                        TicketWorkflowStatusLiteral.Text = string.Format("{1} ({0}% complete)", pctComplete, currentTicket[DatabaseObjects.Columns.TicketStatus]);
                }
                else if (currentModuleName == "SVC")
                {
                    double pctComplete = UGITUtility.StringToDouble(currentTicket[DatabaseObjects.Columns.TicketPctComplete]);
                    //it calculates the pct complete for old svc. if some body update svc task then other code update pct complete automatically. 
                    //in that case, pct complete will come up with value, then no need to run this code.
                    if (pctComplete <= 0)
                    {
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> dependencies = taskManager.LoadByProjectID(currentTicketPublicID);
                        if (dependencies.Count > 0)
                        {
                            double totalHours = dependencies.Sum(x => x.EstimatedHours);
                            List<UGITTask> completedTasks = dependencies.Where(x => x.Status == Constants.Completed).ToList();

                            double completedHours = 0;
                            if (completedTasks.Count > 0)
                                completedHours = completedTasks.Sum(x => x.EstimatedHours);

                            if (totalHours > 0)
                                pctComplete = (completedHours / totalHours);

                            if (totalHours <= 0)
                            {
                                totalHours = dependencies.Sum(x => x.Weight);
                                completedHours = completedTasks.Sum(x => x.Weight);
                                if (totalHours > 0)
                                {
                                    pctComplete = (completedHours / totalHours);
                                }
                            }

                            currentTicket[DatabaseObjects.Columns.TicketPctComplete] = Math.Round(pctComplete, 3);
                        }
                    }

                    TicketWorkflowStatusLiteral.Text = "(" + Math.Round(Convert.ToDecimal(pctComplete * 100), 0) + "% complete) " + TicketWorkflowStatusLiteral.Text;
                }
                else if (totalModuleWeight > 0)
                {
                    TicketWorkflowStatusLiteral.Text = "(" + Math.Round(Decimal.Parse(((currentTicketStageWeight / totalModuleWeight) * 100).ToString()), 0) + "% complete) " + TicketWorkflowStatusLiteral.Text;

                    ///to show next sla expiration  time and type
                    if (TicketRequest.Module.ShowNextSLA)
                    {
                        if (!UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.TicketOnHold]) && UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.NextSLAType) && UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.NextSLATime))
                        {
                            if (currentTicket[DatabaseObjects.Columns.NextSLAType] != null && currentTicket[DatabaseObjects.Columns.NextSLATime] != null)
                            {
                                if (Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.NextSLATime]) <= DateTime.Now)
                                    lblEscalationMessage.Attributes.Add("style", "color: red");     // Red
                                else
                                    lblEscalationMessage.Attributes.Add("style", "color: #3344A2"); // Dark Blue

                                lblEscalationMessage.Text = string.Format("<b>{0} Due: {1}</b>", Convert.ToString(currentTicket[DatabaseObjects.Columns.NextSLAType]), string.Format("{0:MMM-dd-yyyy hh:mm tt}", Convert.ToDateTime(currentTicket[DatabaseObjects.Columns.NextSLATime])));
                            }
                        }
                    }
                }

                //Check authorization of loggedin user

                TicketRelationshipHelper objTicketHelper = new TicketRelationshipHelper(context);
                #region To enable confirmChildTicketsClose popup
                if (currentStage.ApprovedStage != null && currentStage.ApprovedStage.StageTypeChoice == Convert.ToString(StageType.Closed) && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCloseChildTickets))
                {
                    //List<ModuleInstanceDependency> ticketChilds = ModuleInstanceDependency.LoadByPublicID(currentTicketPublicID);
                    List<TicketRelation> ticketChilds = objTicketHelper.GetOpenChildTicket(currentTicketPublicID);
                    if (ticketChilds != null && ticketChilds.Count > 0)
                        confirmChildTicketsClose = true;
                }
                #endregion

                #region get PRP/ORP user for checking change on client side
                isCurrentStageAssignedStage = currentStage.StageTypeChoice == Convert.ToString(StageType.Assigned);
                if (isCurrentStageAssignedStage)
                {
                    if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketPRP))
                    {
                        //SPFieldUserValueCollection prpCollection = uHelper.GetUserCollectionFromValue(SPContext.Current.Web, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketPRP]));
                        uPRPorORPList = UGITUtility.ConvertStringToList(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketPRP]), ",");

                    }

                    if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketORP))
                    {
                        if (uPRPorORPList.Count > 0)
                            uPRPorORPList.AddRange(UGITUtility.ConvertStringToList(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketORP]), ","));
                        else
                            uPRPorORPList = UGITUtility.ConvertStringToList(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketORP]), ",");

                    }
                }
                #endregion

                // Get name of PRP Group
                List<TicketRelation> ParentList = objTicketHelper.GetTicketParentList(currentTicketPublicID);

                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.PRPGroup, currentTicket.Table))
                {

                    PRPGroup = Convert.ToString(currentTicket[DatabaseObjects.Columns.PRPGroup]);//lookupVal.LookupValue;
                }
                else
                {
                    string requestType = Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketRequestTypeLookup));
                    int requestTypeId = 0;
                    if (int.TryParse(requestType.Split(';')[0], out requestTypeId) && requestTypeId > 0)
                    {
                        DataRow[] rows = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.Id + "=" + requestTypeId).Select();
                        if (rows != null && rows.Length > 0)
                            PRPGroup = Convert.ToString(rows[0][DatabaseObjects.Columns.PRPGroup]);
                    }
                }
            }
            else
            {
                ////check if the user is set in "IsAuthorizedToCreate" field only that user can create the ticket :Start
                IsAdmin = UserManager.IsUGITSuperAdmin(user) || UserManager.IsTicketAdmin(user);
                bool allowshowAddNew = true;
                if (!string.IsNullOrEmpty(TicketRequest.Module.AuthorizedToCreate))
                {
                    allowshowAddNew = false;
                    //string[] AuthorizedToCreateUser = TicketRequest.Module.AuthorizedToCreate.Split(Convert.ToChar(uGovernIT.Utility.Constants.Separator6));
                    //foreach (String uInfo in AuthorizedToCreateUser)
                    //{
                    //    // not using IsGroup property for current design
                    //    if ((/*uInfo.IsGroup &&*/ UserManager.CheckUserIsInGroup(uInfo, user)) ||
                    //        user.Id == uInfo)
                    //    {
                    //        allowshowAddNew = true;
                    //        break;
                    //    }
                    //}

                    List<string> AuthorizedToCreateUser = UGITUtility.ConvertStringToList(TicketRequest.Module.AuthorizedToCreate, Constants.Separator6);
                    if (AuthorizedToCreateUser.Count > 0)
                    {
                        if (AuthorizedToCreateUser.Contains(user.Id) || UserManager.IsUserinGroups(user.Id, AuthorizedToCreateUser))
                        {
                            allowshowAddNew = true;
                        }
                    }
                }

                if (allowshowAddNew == true || IsAdmin)
                {
                    isAuthorizedToViewTicket = true;
                }
                else
                {
                    isAuthorizedToViewTicket = false;
                }

                // Show ticket data if user is authorized to view this ticket
                if (isAuthorizedToViewTicket)
                {
                    unauthorizedPanel.Visible = false;
                    moduleContainer.Visible = true;
                }
                else
                {
                    unauthorizedPanel.Visible = true;
                    moduleContainer.Visible = false;
                    return;
                }

                //check if the user is set in "IsAuthorizedToCreate" field only that user can create the ticket : end
                panelDetail.Visible = false;
                panelNewTicket.Visible = true;

                if (TicketRequest.Module.AllowDraftMode)
                {
                    showDraftButton = true;
                    saveAsDraftButtonContainer.Visible = true;
                }
            }

            // If current stage is less than or equal to max hold stage, show hold button
            // For PMM since we use custom lifecycles, enable in all stages
            if (TicketRequest.Module.ModuleHoldMaxStage >= currentStep || currentModuleName == "PMM" || currentModuleName == "TSK")
            {
                showHoldUnholdButton = true;
            }
            //Added by mudassir 10 march 2020
            //chkNotifyCommentRequestor.Checked = chkNotifyRequestor.Checked = TicketRequest.Module.RequestorNotificationOnComment;
            chkNotifyRequestor.Checked = TicketRequest.Module.RequestorNotificationOnComment;
            //
            SetAuthorization();

            ticketControls = new TicketControls(module, currentTicket);

        }

        /// <summary>
        /// Bind menu of module item
        /// </summary>
        private void BindMenu()
        {
            if (TicketRequest.Module.List_FormTab.Count > 0 && tbcDetailTabs != null && tbcDetailTabs.Tabs.Count == 0)
            {
                bool isMobileDevice = HttpContext.Current.Request.Browser.IsMobileDevice;
                List<ModuleFormTab> lstFormTab = new List<ModuleFormTab>();
                if (Request.QueryString["BatchEditing"] == "true")
                {
                    var tabs = (from t in TicketRequest.Module.List_FormTab
                                join l in TicketRequest.Module.List_FormLayout on
                                t.TabId equals l.TabId
                                join r in TicketRequest.Module.List_RoleWriteAccess on
                                l.FieldName equals r.FieldName
                                where r.ModuleNameLookup == currentModuleName
                                select t).Distinct();

                    if (tabs != null && tabs.Count() > 0)
                    {
                        lstFormTab.AddRange(tabs);
                    }

                    if (lstFormTab.Count > 0)
                    {
                        if (lstFormTab.Count > 0)
                        {
                            foreach (ModuleFormTab tab in lstFormTab.OrderBy(x => x.TabSequence).ToList())
                            {
                                //if (tab.Prop_IsScrumTab.HasValue && currentTicket != null && !uHelper.StringToBoolean(currentTicket[DatabaseObjects.Columns.ScrumLifeCycle]))
                                //    continue;

                                //if (tab.prop_IsSummaryTab.HasValue && currentTicket != null && !uHelper.IsSPItemExist(currentTicket, DatabaseObjects.Columns.UserQuestionSummary))
                                //    continue;

                                tbcDetailTabs.Tabs.Add(new DevExpress.Web.Tab(tab.TabName, Convert.ToString(tab.TabId)));
                            }
                        }
                    }
                }
                else
                {
                    //Check authorization to view tab
                    bool authorizedToViewTab = true;
                    List<string> authorizedToViewList = new List<string>();
                    foreach (ModuleFormTab tab in TicketRequest.Module.List_FormTab.OrderBy(x => x.TabSequence).ToList())
                    {
                        //if (tab.Prop_IsScrumTab.HasValue && currentTicket != null && !uHelper.StringToBoolean(currentTicket[DatabaseObjects.Columns.ScrumLifeCycle]))
                        //    continue;

                        //if (tab.prop_IsSummaryTab.HasValue && currentTicket != null && !uHelper.IsSPItemExist(currentTicket, DatabaseObjects.Columns.UserQuestionSummary))
                        //    continue;

                        authorizedToViewList = UGITUtility.ConvertStringToList(tab.AuthorizedToView, Constants.Separator6);
                        authorizedToViewTab = true;
                        if (authorizedToViewList.Count > 0)
                        {
                            authorizedToViewTab = false;
                            if (tab.AuthorizedToView.Contains(user.Id) || UserManager.IsUserinGroups(user.Id, authorizedToViewList))
                            {
                                authorizedToViewTab = true;
                            }
                        }

                        if (authorizedToViewTab)
                        {
                            if (isMobileDevice == true)
                            {
                                if (showInMobile.showTabinMobile(tab))
                                {
                                    if (tab.TabName == "Scrum")
                                    {
                                        if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.ScrumLifeCycle) && Convert.ToBoolean(currentTicket[DatabaseObjects.Columns.ScrumLifeCycle]))
                                            tbcDetailTabs.Tabs.Add(new DevExpress.Web.Tab(tab.TabName, Convert.ToString(tab.TabId)));
                                    }
                                    else
                                        tbcDetailTabs.Tabs.Add(new DevExpress.Web.Tab(tab.TabName, Convert.ToString(tab.TabId)));
                                }
                            }
                            else
                            {
                                if (tab.TabName == "Scrum")
                                {
                                    if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.ScrumLifeCycle) && Convert.ToBoolean(currentTicket[DatabaseObjects.Columns.ScrumLifeCycle]))
                                        tbcDetailTabs.Tabs.Add(new DevExpress.Web.Tab(tab.TabName, Convert.ToString(tab.TabId)));
                                }
                                else
                                    tbcDetailTabs.Tabs.Add(new DevExpress.Web.Tab(tab.TabName, Convert.ToString(tab.TabId)));
                            }
                        }
                    }
                }
                for (int i = 0; i < tbcDetailTabs.Tabs.Count; i++)
                {
                    tbcDetailTabs.Tabs[i].Name = Convert.ToString(i + 1);
                }
            }

            //build print option menu
            List<ModuleFormTab> tabsToPrint = TicketRequest.Module.List_FormTab.Where(x => tbcDetailTabs.Tabs.FindByText(x.TabName) != null).OrderBy(y => y.TabSequence).ToList();
            //tabsToPrint.Sort((x, y) => x.TabSequence.CompareTo(y.TabSequence));
            chkSelectTabList.DataSource = tabsToPrint;
            chkSelectTabList.DataBind();
        }

        protected void tbcDetailTabs_TabDataBound(object source, DevExpress.Web.TabControlEventArgs e)
        {

        }

        /// <summary>
        /// Stage progress graphic
        /// </summary>
        private void BindStageGraphic()
        {
            if (lifeCycle != null)
            {
                List<LifeCycleStage> lifeCycleStages = new List<LifeCycleStage>();
                List<LifeCycleStage> newlistLifeCycleStage = new List<LifeCycleStage>();

                if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.WorkflowSkipStages) &&
                    !string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.WorkflowSkipStages])))
                {
                    string[] stagestepArray = Convert.ToString(currentTicket[DatabaseObjects.Columns.WorkflowSkipStages]).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    int pos;
                    foreach (var item in lifeCycle.Stages)
                    {
                        pos = Array.IndexOf(stagestepArray, Convert.ToString(item.StageStep));
                        if (pos < 0)
                        {
                            lifeCycleStages.Add(item);
                        }
                    }
                }
                else
                {
                    lifeCycleStages = lifeCycle.Stages;
                }


                // Get module details
                // DataRow moduleRow = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules).AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == currentModuleName || x.Field<int>(DatabaseObjects.Columns.Id) == moduleId);
                UGITModule moduleRow = ModuleViewManager.LoadByName(currentModuleName);
                if (moduleRow != null)
                {
                    //this show/hide the workflow stage..
                    if (UGITUtility.StringToBoolean(moduleRow.HideWorkFlow))
                    {
                        topGraphicDiv.Visible = false;
                        ticketMsgContainer.Visible = false;
                    }
                    else
                    {
                        int count = 0;
                        foreach (var item1 in lifeCycleStages)
                        {
                            count++;
                            LifeCycleStage stageitem = (LifeCycleStage)item1.Clone();
                            stageitem.StageStep = count;
                            stageitem.IconUrl = item1.IconUrl;
                            stageitem.NavigationType = item1.NavigationType;
                            stageitem.NavigationUrl = item1.NavigationUrl;

                            newlistLifeCycleStage.Add(stageitem);
                        }
                        if (currentModuleName.ToLower() == "pmm")
                        {
                            StageRepeater.DataSource = newlistLifeCycleStage;
                            StageRepeater.DataBind();
                        }
                        else
                        {
                            lvStepSections.DataSource = newlistLifeCycleStage;
                            lvStepSections.DataBind();
                        }
                        //newTicketStageRepeater.DataSource = lifeCycle.Stages;
                        //newTicketStageRepeater.DataBind();

                    }
                }

            }
            else
            {
                topGraphicDiv.Visible = false;
                ticketMsgContainer.Visible = false;
            }
        }

        /// <summary>
        /// Initializes the buttons in the webpart.
        /// </summary>
        private void InitializeButtons()
        {
            updateButtonLI.Visible = false;
            rejectButtonLI.Visible = false;
            approveButtonLI.Visible = false;
            returnButtonLI.Visible = false;
            importPMMLI.Visible = false;
            //closeButtonLI.Visible = false;
            closeButtonLI.Attributes.Add("style", "display:none");

            // new button for award.
            awardButtonLI.Visible = false;
            statusButtonLI.Visible = false;
            advanceToProjectLI.Visible = false;

            currentCreateTSR = string.Empty;
            selfAssignButtonLI.Visible = false;
            superAdminEditButtonLI.Visible = false;
            btnsuperAdminEditButton.Visible = false;
            saveAsDraftButtonContainer.Visible = false;
            updateButtonLI.Style.Add(HtmlTextWriterStyle.Display, "block");

            switch (currentModuleName.ToLower())
            {
                case "pmm":
                    pnlReport.Visible = true;
                    pmmReportUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/delegatecontrol.aspx?control=projectreportview&PMMId={0}&projectYear={1}", currentTicketId, DateTime.Today.Year));
                    pmmResourceReportUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/delegatecontrol.aspx?control=resourcereport&PMMId={0}", currentTicketId));
                    pmmprojectERHReportUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=estimatedremaininghoursreport&TicketPublicId={0}&ProjectId={1}", currentTicketPublicID, currentTicketId));
                    break;
                case "itg":
                    updateButtonLI.Visible = false;
                    cancelButtonLI.Visible = false;
                    pnlReport.Visible = false;
                    break;
                default:
                    pnlReport.Visible = false;
                    break;
            }

            // Since Save button now keeps ticket open, cancel button is titled Close
            cancelButton.Text = "Close";

            if (isActionUser && ticketOnHold != 1)
            {
                if (currentTicket != null)
                {
                    updateButton.Visible = true;
                    updateButton.Text = "Save";
                    updateButton.CssClass += " linkbutton";
                    updateButtonLI.Visible = true;
                    updateButtonSC.Visible = true;
                }

                if (showRejectButton)
                {
                    btnrejectButton.ID = "rejectButton";
                    rejectWithCommentsButton.Visible = true;
                    rejectWithCommentsButton.Text = rejectButtonName;
                    rejectWithCommentsButton.ImageUrl = currentStage.RejectIcon;
                    if (!string.IsNullOrEmpty(rejectButtonImage))
                    {
                        rejectWithCommentsButton.Style.Add("background-image", "url('" + rejectButtonImage + "')");
                    }
                    //if (quickClose)
                    //{
                    //    rejectWithCommentsButton.CssClass = "approve linkbutton";
                    //}
                    //else
                    //    rejectWithCommentsButton.CssClass = "reject linkbutton";

                    rejectButtonLI.Visible = true;

                    rejectWithCommentsButton.ToolTip = currentStage.RejectActionTooltip;
                    btnrejectButton.ToolTip = currentStage.RejectActionTooltip;
                }
                if (showApproveButton)
                {
                    approveButton.ID = "approveButton";
                    approveButton.Visible = true;
                    approveButton.Text = approveButtonName;
                    approveButton.CssClass += " linkbutton";
                    if (!string.IsNullOrEmpty(approveButtonImage))
                    {
                        approveButton.Style.Add("background-image", "url('" + approveButtonImage + "')");
                    }
                    approveButtonLI.Visible = true;
                    approveButton.ToolTip = currentStage.ApproveActionTooltip;
                    approveButton.ImageUrl= currentStage.ApproveIcon;
                    returnButtond.ToolTip = currentStage.ReturnActionToolip;
                    returnWithCommentsButton.ToolTip = currentStage.ReturnActionToolip;

                }
                if (showHoldUnholdButton)
                {
                    btnHoldButton.Text = uGovernIT.Utility.Constants.HoldButtonText;
                }
                if (showReturnButton)
                {
                    returnWithCommentsButton.Text = returnButtonName;
                    returnButtond.Text = returnButtonName;
                    returnButtond.Visible = true;
                    returnButtond.CssClass += " linkbutton";
                    returnWithCommentsButton.CssClass += " linkbutton";
                    if (!string.IsNullOrEmpty(currentStage.ReturnIcon))
                    {
                        returnButtond.Style.Add("background-image", "url('" + currentStage.ReturnIcon + "')");
                        returnWithCommentsButton.Style.Add("background-image", "url('" + currentStage.ReturnIcon + "')");
                    }
                    returnWithCommentsButton.ImageUrl = currentStage.ReturnIcon;
                    returnButtonLI.Visible = true;
                }


                if (showBaselineButtons)
                {
                    createBaselineButtonH.ID = "createBaseline";
                }

                switch (currentModuleName.ToLower())
                {
                    case "pmm":
                        if (currentTicket != null)
                        {
                            //closeButtonLI.Visible = true;
                            closeButtonLI.Attributes.Add("style", "display:none");
                            closeButton.Text = uGovernIT.Utility.Constants.CloseButtonText;
                            closeButton.Attributes.Add("href", string.Format("javascript:closePMMTicket('{0}');", currentTicket["ID"]));
                            btncloseButtonH.CommandArgument = "ClosePMM";
                        }
                        break;
                    case "tsk":

                        pnlReport.Visible = false;
                        tskReportUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/delegatecontrol.aspx?control=tskprojectreportview&TSKId={0}", currentTicketId));
                        break;
                    case "inc":
                        {
                            LifeCycleStage resolvedStage = lifeCycle.Stages.FirstOrDefault(x => x.StageTypeChoice == "Resolved");
                            if (resolvedStage != null)
                                if (currentStage.StageStep < resolvedStage.StageStep) // Incident is pending assignment or resolution
                                {
                                    btnSendNotification.Attributes.Add("href", "javascript:");
                                }
                                else // Incident resolved or closed
                                {
                                    btnSendNotification.Attributes.Add("href", "javascript:");
                                    lblIncidentBody.Text = "Notes";
                                    lblActions.Visible = true;
                                    txtActions.Visible = true;
                                }
                        }
                        break;
                    case "cpr":
                    case "cns":
                        {
                            // award button..
                            if (lifeCycle != null)
                            {
                                LifeCycleStage awardStage = TicketRequest.GetTicketAwardStage(lifeCycle);
                                if (awardStage != null && currentStage.StageStep < awardStage.StageStep)
                                {
                                    awardButtonLI.Visible = true;
                                    awardButton.Text = "Award";
                                    awardButton.CommandArgument = "AwardCPR";
                                }
                            }
                        }
                        break;
                    case "opm":

                        if (lifeCycle != null)
                        {
                            LifeCycleStage closeStage = TicketRequest.GetTicketCloseStage(lifeCycle);
                            if (closeStage != null && currentStage.StageStep != closeStage.StageStep)
                            {
                                statusButtonLI.Visible = true;
                                statusButton.Text = "Lost/Hold";

                                advanceToProjectLI.Visible = true;
                                advanceToProject.Text = "Advance to Project";
                            }
                        }
                        break;
                }
            }
            else if (isActionUser && ticketOnHold == 1 && showHoldUnholdButton)
            {
                btnUnHoldButton.Text = uGovernIT.Utility.Constants.UnHoldButtonText;
            }

            if (showSelfAssignButton)
            {
                selfAssignButtonLI.Visible = true;
                selfAssignButton.Visible = true;
            }

            if (showAdminEditButton)
            {
                superAdminEditButtonLI.Visible = true;
                btnsuperAdminEditButton.Visible = true;
            }
            //Condition to show save button if ticket is On Hold and admin overrides the ticket
            if (ticketOnHold == 1 && isActionUser)
            {
                updateButton.Visible = true;
                updateButton.Text = "Save";
                updateButton.CssClass += " linkbutton";
                updateButtonLI.Visible = true;
                updateButtonSC.Visible = true;
            }


            //hide buttons and control in case of batch editing..
            if (Request["BatchEditing"] == "true")
            {
                //hide the buttons...
                updateButtonLI.Visible = true;
                rejectButtonLI.Visible = false;
                approveButtonLI.Visible = false;
                returnButtonLI.Visible = false;
                importPMMLI.Visible = false;
                //closeButtonLI.Visible = false;
                closeButtonLI.Attributes.Add("style", "display:none");
                currentCreateTSR = string.Empty;
                selfAssignButtonLI.Visible = false;
                superAdminEditButtonLI.Visible = false;
                btnsuperAdminEditButton.Visible = false;
                saveAsDraftButtonContainer.Visible = false;
                updateButton.Visible = true;
                importNPRLI.Visible = false;
                updateButtonSC.Visible = true;
                // award button
                awardButtonLI.Visible = false;

                //hide the stage and text...
                //stageDescriptionContainer.Visible = false;
            }

            if (!updateButtonLI.Visible && isFieldLevelAccessIsPersent)
            {
                updateButtonLI.Visible = true;
                updateButtonLI.Style.Add(HtmlTextWriterStyle.Display, "none");
                updateButton.Visible = true;
                updateButtonSC.Visible = true;
            }
        }

        /// <summary>
        /// Check authorization of existing existing ticket whether loggedin user have permission to edit of not
        /// it also check which button will be shown or not
        /// </summary>
        protected void SetAuthorization()
        {
            // PMMBaseline pMMBaseline = new PMMBaseline(currentTicketId, baselineDate, context);
            ////Show baseline

            if (currentModuleName == "PMM" && Request["showBaseline"] != null && UGITUtility.StringToBoolean(Request["showBaseline"].Trim()) && Request["baselineNum"] != null)
            {
                int baselineNumber = 0;
                int.TryParse(Request["baselineNum"].Trim(), out baselineNumber);

                if (currentTicket != null)
                {

                    //List<PMMBaseline> baselines = PMMBaseline.GetBaselines(currentTicketId);
                    //PassTicketID
                    List<Baseline> baselines = Baseline.GetBaselines(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), context);

                    Baseline baseline = baselines.FirstOrDefault(x => x.BaselineNum == baselineNumber);
                    impMessageBox.Style.Add(HtmlTextWriterStyle.Display, "none");

                    if (baseline != null)
                    {
                        impMessageBox.Style.Add(HtmlTextWriterStyle.Display, "block");
                        importantMessageBox.InnerHtml = "Showing Baseline: <b>" + baseline.BaselineComment + " [" + baseline.BaselineDate.ToString("MMM dd yyyy h:mm tt") + "]</b>. <a href='javascript:' onclick='removeBaseline()'>Click Here</a> to show current data.";
                        currentTicket = Baseline.LoadProjectBaseline(currentTicket, baselineNumber, context);
                    }
                }
            }


            ////Check if current user is super-admin or ticket admin - if so enable IsAdmin
            if (user != null)
            {
                IsAdmin = UserManager.IsRole(RoleType.UGITSuperAdmin, user.UserName) || UserManager.IsRole(RoleType.Admin, user.UserName) || UserManager.IsRole(RoleType.TicketAdmin, user.UserName);
            }

            ////Check if current user is super-admin or ticket admin - if so enable Admin Override button
            showAdminEditButton = IsAdmin && currentModuleName != "PMM" && currentModuleName != "ITG";

            //Get Approve, Reject & Return button names
            if (currentStage != null)
            {
                approveButtonName = currentStage.StageApproveButtonName;
                rejectButtonName = currentStage.StageRejectedButtonName;
                returnButtonName = currentStage.StageReturnButtonName;

                //Get Approve, Reject button custom Image.
                if (!string.IsNullOrEmpty(currentStage.ApproveIcon))
                    approveButtonImage = currentStage.ApproveIcon;
                else if (!string.IsNullOrEmpty(currentStage.Prop_CustomIconApprove))
                    approveButtonImage = currentStage.Prop_CustomIconApprove;

                if (!string.IsNullOrEmpty(currentStage.RejectIcon))
                    rejectButtonImage = currentStage.RejectIcon;
                else if (!string.IsNullOrEmpty(currentStage.Prop_CustomIconReject))
                    rejectButtonImage = currentStage.Prop_CustomIconReject;

                // Check approve button will be shown or not
                if (currentStage.ApprovedStage != null && !string.IsNullOrEmpty(approveButtonName))
                    showApproveButton = true;

                if (currentStage.RejectStage != null && !string.IsNullOrEmpty(rejectButtonName))
                    showRejectButton = true;

                if (currentStage.ReturnStage != null && !string.IsNullOrEmpty(returnButtonName))
                {
                    TicketManager ticketMGR = new TicketManager(context);
                    ModuleViewManager moduleMGR = new ModuleViewManager(context);
                    UGITModule companyModule = moduleMGR.LoadByName(ModuleNames.CPR);
                    DataTable dtAllTickets = ticketMGR.GetAllTickets(companyModule);
                    DataRow[] _ticketrows = dtAllTickets.Select($"OPMIdLookup = '{Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId])}'");

                    if (_ticketrows.Count() > 0)
                    {
                        showReturnButton = false;
                    }
                    else
                    {
                        showReturnButton = true;
                    }
                    
                    returnStageId = string.Format("{0}{1}{2}", currentStage.ReturnStage.ID, uGovernIT.Utility.Constants.Separator, currentStage.ReturnStage.Name);
                }

                //Get From Admin->Module flag for Showbaselinebutton
                //if (currentStage.Prop_BaseLine.HasValue && currentStage.Prop_BaseLine.Value)
                // showBaselineButtons = true;
            }
            //Check loggedin user has permission or not
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketStageActionUserTypes))
            {
                string ticketStageActionUserTypes = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]);
                if (!string.IsNullOrEmpty(ticketStageActionUserTypes) && ticketStageActionUserTypes.IndexOf(Constants.Separator10) != -1)
                    ticketStageActionUserTypes = ticketStageActionUserTypes.Replace(Constants.Separator10, Constants.Separator);

                List<string> currentStageActionUserTypes = UGITUtility.ConvertStringToList(ticketStageActionUserTypes, uGovernIT.Utility.Constants.Separator);

                // Get all authorized user names
                string userNames = string.Empty;
                if (currentStageActionUserTypes != null && currentStageActionUserTypes.Count > 0)
                {
                    if (ConfigurationVariableManager.GetValueAsBool("OnlyShowFirstActionUser"))
                    {
                        List<string> fristuser = new List<string>() { currentStageActionUserTypes.FirstOrDefault() };
                        // Just want to show first username in status
                        userNames = UserManager.CommaSeparatedNamesFrom(currentTicket, fristuser, uGovernIT.Utility.Constants.Separator);
                    }
                    else
                    {
                        // Add all names to list
                        userNames = UserManager.CommaSeparatedNamesFrom(currentTicket, currentStageActionUserTypes.Distinct().ToList(), uGovernIT.Utility.Constants.Separator);
                    }
                }

                // Now check if CURRENT user is authorized
                foreach (string currentStageActionUserType1 in currentStageActionUserTypes)
                {
                    if (currentStageActionUserType1 != string.Empty)
                    {
                        if (UserManager.IsUserPresentInField(user, currentTicket, currentStageActionUserType1, true))
                        {
                            isActionUser = true;
                            if (currentModuleName.ToLower() == "tsr" && currentStage.StageTypeChoice == StageType.Closed.ToString())
                            {
                                showCreateAssetButton = true;
                            }
                            break; // No need to keep going!
                        }
                    }
                }

                if (userNames != string.Empty)
                {
                    TicketWorkflowStatusLiteral.Text += " Awaiting On  (" + userNames + ") ";
                }
            }

            //Check for Admin override mode + admin user
            //if (showAdminEditButton && adminOverride)//Comment to Check on 11-04-17
            //    isActionUser = true;
            if (adminOverride)
            {
                isActionUser = true;
            }
            //Self-Assign Button Visibility
            if (!isActionUser && (currentModuleName.ToLower() == "tsr" || currentModuleName.ToLower() == "prs") &&
                currentStage.Prop_SelfAssign.HasValue && currentStage.Prop_SelfAssign.Value == true)
            {
                if (UserManager.CheckUserIsInGroup(PRPGroup, user))
                    showSelfAssignButton = true;
            }

            isAuthorizedToViewTicket = true;

            //            //Check item-level authorization
            if (!isActionUser && // If Not Ticket action user
                !UserManager.IsUGITSuperAdmin(user) && // And Not super admin
                !UserManager.IsUserPresentInModuleUserTypes(user, currentTicket, currentModuleName)) // And not named user in ticket
            {
                // Hide if user is not authorized to view module OR ticket marked private (and user not named user)

                // Change on the 07/12/2016
                //if (!UserManager.IsUserAuthorizedToViewModule(user, TicketRequest.Module) ||
                //    (UGITUtility.IfColumnExists(DatabaseObjects.Columns.IsPrivate, currentTicket.Table) && UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.IsPrivate])))
                //{
                //    isAuthorizedToViewTicket = false;
                //}
            }


            // Show ticket data if user is authorized to view this ticket
            if (isAuthorizedToViewTicket)
            {
                unauthorizedPanel.Visible = false;
                moduleContainer.Visible = true;
            }
            else
            {
                unauthorizedPanel.Visible = true;
                moduleContainer.Visible = false;
            }

            ////Check for Quick TSR, if its a newly created Quick TSR we disable to ApproveButton and also the Feedback popup.
            ////if (isActionUser && uHelper.IsQuickClose(currentModuleName, currentTicket))
            ////{
            ////    //showApproveButton = false;
            ////    quickClose = true;
            ////}

            //Create Incident button Visibility in PRS Module -taken out for now since can be done from Related Tickets
            switch (currentModuleName.ToLower())
            {
                case "prs":
                    if (lifeCycle.Stages.Exists(x => x.StageStep > currentStep && x.StageTypeChoice == StageType.Closed.ToString()))
                    {
                        showCreateIncidentButton = true;
                    }
                    break;
            }

            ////Show Comment action if showcomment property is true.
            //if (TicketRequest.Module.ShowComment)
            //    addCommentLI.Visible = true;

            if (currentStage.Prop_ReadyToImport.HasValue && currentStage.Prop_ReadyToImport.Value && Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketPMMIdLookup)) == string.Empty)
            {
                importNPRLI.Visible = true;
                nprTitle = Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.Title));
                UGITTaskManager objTask = new UGITTaskManager(context);
                string startdate = objTask.GetMinimumTaskStartDate(currentModuleName, currentTicketPublicID);
                string importUrl = UGITUtility.GetAbsoluteURL($"/Layouts/ugovernit/DelegateControl.aspx?control=AddNewProject&module=NPR&ticketid={currentTicketPublicID}&title={nprTitle}&startdate={startdate}");
                string funciton = string.Format(" javascript:UgitOpenPopupDialog('{0}', 'NPRTicketId={1}', 'PMM Project Import Wizard', '90', '90', 0, '{2}')", importUrl, currentTicketPublicID, Uri.EscapeUriString(Request.Url.AbsolutePath));
                importNPRButton.ClientSideEvents.Click = "function(s,e){" + funciton + "}";
            }

            //Disable edit, if ticket is in waiting status and stage step 1. it means ticket is waiting for something to finish. like SVC
            if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.TicketStatus) && UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.StageStep)
                && Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketStatus)).ToLower() == "waiting" && Convert.ToInt32(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.StageStep)) == 1)
            {
                //showAdminEditButton = false;
                showApproveButton = false;
                showRejectButton = false;
                showReturnButton = false;
                showHoldUnholdButton = false;
                showDraftButton = false;
                showSelfAssignButton = false;
                //if (!showAdminEditButton)
                //      isActionUser = false;
            }

        }

        /// <summary>
        /// Module Tabs
        /// </summary>
        protected void BindTabs()
        {
            if (TicketRequest.Module.List_FormTab.Count > 0)
            {
                //Added for getting tabsequence to maintain tab on summary tab from agent view 
                var tabvalue = TicketRequest.Module.List_FormTab.Where(m => m.TabName == "Summary").Select(m => m.TabSequence).FirstOrDefault();
                Response.Cookies["TicketSelectedTabValue"].Value = tabvalue.ToString();
                //END
                tabRepeater.Controls.Clear();
                if (selectedTabs == null || selectedTabs == string.Empty)
                {
                    if (Request.QueryString["BatchEditing"] == "true")
                    {
                        List<ModuleFormTab> formTab = new List<ModuleFormTab>();
                        var tabs = (from t in TicketRequest.Module.List_FormTab
                                    join l in TicketRequest.Module.List_FormLayout on
                                    t.TabId equals l.TabId
                                    join r in TicketRequest.Module.List_RoleWriteAccess on
                                    l.FieldName equals r.FieldName
                                    where r.ModuleNameLookup == currentModuleName
                                    orderby t.TabSequence
                                    select t).Distinct();

                        if (tabs != null && tabs.Count() > 0)
                        {
                            foreach (ModuleFormTab tab in tabs.OrderBy(t => t.TabSequence).ToList())
                            {
                                formTab.Add(tab);
                            }
                            // formTab.AddRange(tabs);
                        }

                        if (formTab.Count > 0)
                        {
                            BindDataSourceToTabs(formTab);
                        }
                    }
                    else
                    {
                        List<ModuleFormTab> lstFormTab = new List<ModuleFormTab>();

                        foreach (ModuleFormTab tab in TicketRequest.Module.List_FormTab.OrderBy(t => t.TabSequence).ToList())
                        {
                            //if (tab.Prop_IsScrumTab.HasValue && currentTicket != null && !uHelper.StringToBoolean(currentTicket[DatabaseObjects.Columns.ScrumLifeCycle]))
                            //    continue;

                            //if (tab.prop_IsSummaryTab.HasValue && currentTicket != null && !uHelper.IsSPItemExist(currentTicket, DatabaseObjects.Columns.UserQuestionSummary))
                            //    continue;


                            if (HttpContext.Current.Request.Browser.IsMobileDevice == true)
                            {
                                if (showInMobile.showTabinMobile(tab))
                                {
                                    lstFormTab.Add(tab);

                                }
                            }
                            else
                            {
                                lstFormTab.Add(tab);
                            }
                            //lstFormTab.Add(tab);
                        }

                        //lstFormTab.Sort((x, y) => x.TabSequence.CompareTo(y.TabSequence));
                        BindDataSourceToTabs(lstFormTab);


                        ////When Attachment field control is not rendered on page, then its show some weird value in field control value.
                        ////Add temporary attachment control will solve this issue.
                        if (currentTicket != null)
                        {
                            //SPField f = thisList.Fields.GetFieldByInternalName(DatabaseObjects.Columns.Attachments);
                            //Control bfc = SPControls.GetSharePointControls(this.Context, f, thisList, currentTicket, SPControlMode.Display, moduleId);

                            //if (adminOverride)
                            //    bfc.ID += "_temp_AdminOverride";
                            //else
                            //    bfc.ID += "_temp";

                            //if (tempAttachment.Controls.Count < 1)
                            //    tempAttachment.Controls.Add(bfc);
                        }
                    }
                }
                else
                {
                    List<ModuleFormTab> lstFormTab = new List<ModuleFormTab>();
                    if (TicketRequest != null && TicketRequest.Module.List_FormTab.Count > 0)
                    {
                        List<string> checktabs = new List<string>();
                        if (selectedTabs != null)
                            checktabs = selectedTabs.Split(',').ToList();
                        if (checktabs.Count > 0)
                        {
                            var tabs = TicketRequest.Module.List_FormTab.Where(a => checktabs.Contains(Convert.ToString(a.ID)) && a.ModuleNameLookup == currentModuleName);


                            foreach (var tab in tabs.OrderBy(t => t.TabSequence).ToList())
                            {
                                //if (tab.Prop_IsScrumTab.HasValue && currentTicket != null && !uHelper.StringToBoolean(currentTicket[DatabaseObjects.Columns.ScrumLifeCycle]))
                                //    continue;

                                //if (tab.prop_IsSummaryTab.HasValue && currentTicket != null && !uHelper.IsSPItemExist(currentTicket, DatabaseObjects.Columns.UserQuestionSummary))
                                //    continue;

                                lstFormTab.Add(tab);
                            }
                        }
                    }

                    BindDataSourceToTabs(lstFormTab);

                    ////When Attachment field control is not rendered on page, then its show some weird value in field control value.
                    ////Add temporary attachment control will solve this issue.
                    if (currentTicket != null)
                    {
                        //SPField f = thisList.Fields.GetFieldByInternalName(DatabaseObjects.Columns.Attachments);
                        //Control bfc = SPControls.GetSharePointControls(this.Context, f, thisList, currentTicket, SPControlMode.Display, moduleId);

                        //if (adminOverride)
                        //    bfc.ID += "_temp_AdminOverride";
                        //else
                        //    bfc.ID += "_temp";

                        //if (tempAttachment.Controls.Count < 1)
                        //    tempAttachment.Controls.Add(bfc);
                    }

                }
            }
        }
        private void BindDataSourceToTabs(List<ModuleFormTab> formTab)
        {
            int index = 1;
            for (int i = 0; i < formTab.Count; i++)
            {
                bool authorizedToViewTab = true;
                List<string> authorizedToViewList = UGITUtility.ConvertStringToList(formTab[i].AuthorizedToView, uGovernIT.Utility.Constants.Separator6);
                if (authorizedToViewList.Count > 0)
                {
                    authorizedToViewTab = false; ;
                    if (formTab[i].AuthorizedToView.Contains(user.Id) || UserManager.IsUserinGroups(user.Id, authorizedToViewList))
                    {
                        authorizedToViewTab = true;
                    }
                }
                if (authorizedToViewTab)
                {
                    formTab[i].TabSequenceOnScreen = index;
                    index++;
                }
                else {
                    formTab[i].TabSequenceOnScreen = 0;
                }
            }
            tabRepeater.DataSource = formTab.OrderBy(y => y.TabSequence);
            tabRepeater.DataBind();
        }

        /// <summary>
        /// Called on creation of each module tab. Populated the panel inside the tab with the current tab fields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ModuleTabItemCreated(object sender, RepeaterItemEventArgs e)
        {
            #region default tab
            Enums.TableDisplayTypes tableType = new Enums.TableDisplayTypes();
            if (Request.QueryString["AllTickets"] != null && Request.QueryString["BatchEditing"] == "true")
            {
                TicketIdList = Convert.ToString(Request["AllTickets"]).Split(new string[] { "," }, StringSplitOptions.None).ToList();
                LoadTicketsCollection();
            }
            tableType = Enums.TableDisplayTypes.General;
            int tableCellsLength = 0;
            if (currentTicket == null)
            {
                return;
            }

            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }
            e.Item.ID = e.Item.ItemIndex + "repeaterItem";
            #region BatchEditing is False
            if (Request.QueryString["BatchEditing"] != "true")
            {
                ticketControls.BatchEdit = BatchMode.None;

                if (null != e.Item.FindControl("tabContainer") && null != e.Item.DataItem)
                {
                    tabTable = new Table();
                    tabTable.EnableTheming = true;
                    tabTable.Rows.Clear();
                    tabTable.Attributes.Add("style", "border-collapse:collapse;width:100%");
                    // tabTable.CssClass = "ticket_container";
                    int columnCount = 0, fieldWidth = 1;
                    TableRow row = new TableRow();

                    ModuleFormTab formTab = (ModuleFormTab)e.Item.DataItem;
                    tabTable.ID = formTab.TabId + "tab";

                    //Check authorization to view tab
                    bool authorizedToViewTab = true, authorizedToEditTab = true;
                    List<string> authorizedToViewList = UGITUtility.ConvertStringToList(formTab.AuthorizedToView, uGovernIT.Utility.Constants.Separator6);
                    List<string> authorizedToEditList = UGITUtility.ConvertStringToList(formTab.AuthorizedToEdit, uGovernIT.Utility.Constants.Separator6);
                    if (authorizedToViewList.Count > 0)
                    {
                        authorizedToViewTab = true;
                        if (UserManager.IsUserExistInList(user, authorizedToViewList))
                        {
                            authorizedToViewTab = true;
                        }
                    }
                    //Check authorization to edit tab
                    if (authorizedToEditList.Count > 0)
                    {
                        authorizedToEditTab = false;
                        if (UserManager.IsUserExistInList(user, authorizedToEditList))
                        {
                            authorizedToEditTab = true;
                        }
                    }
                    if (authorizedToViewTab || authorizedToEditTab)
                    {
                        //SPQuery oQuery = new SPQuery();

                        HiddenField tabIdHidden = new HiddenField();
                        tabIdHidden.ID = "tabIdHidden";
                        tabIdHidden.Value = formTab.TabId.ToString();  // temp code
                        ((Panel)e.Item.FindControl("tabContainer")).Controls.Add(tabIdHidden);

                        Table mainTable = tabTable;
                        string legend = string.Empty;
                        bool isGroup = false;
                        Panel groupedPanelContainer = null;
                        double tabID = Convert.ToDouble(tabIdHidden.Value);

                        //List<ModuleFormLayout> layoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabNumber == tabID && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();
                        //List<ModuleFormLayout> alllayoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabId == tabID && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();

                        alllayoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabId == tabID && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();

                        List<ModuleFormLayout> layoutItems = new List<ModuleFormLayout>();
                        foreach (ModuleFormLayout removetabField in alllayoutItems.ToList())
                        {
                            // Add field to form if no skip condition configured OR if skip condition evaluates to false (i.e. don't skip)
                            if (string.IsNullOrEmpty(removetabField.SkipOnCondition) ||
                                FormulaBuilder.EvaluateFormulaExpression(context, removetabField.SkipOnCondition, currentTicket) == false)
                            {
                                layoutItems.Add(removetabField);
                            }
                        }
                        int totalcount = 0;
                        int loopcount = 0;
                        ModuleRoleWriteAccess roleWriteAccess = null;
                        ModuleRoleWriteAccess roleWriteAccessTemp = null;
                        foreach (ModuleFormLayout tabField in layoutItems)
                        {
                            string fieldDisplayName = tabField.FieldDisplayName;
                            if (tabField.TargetURL != string.Empty && tabField.TargetURL != null)
                            {
                                //Dictionary<string, string> customProperties = uHelper.GetCustomProperties(, Constants.Separator);
                                //KeyValuePair<string, string> pageParam = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "fileurl");
                                string width = "100";
                                string height = "100";

                                string href = string.Format(@"javascript:UgitOpenPopupDialog('{0}','','Help - {2}','{3}','{4}',0,'{1}')", tabField.TargetURL, Server.UrlEncode(Request.Url.AbsolutePath), tabField.FieldDisplayName, width, height);
                                fieldDisplayName = string.Format("<a title=\"{3}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{2}\" href=\"{1}\">{0}</a>", fieldDisplayName, href, currentModuleName, tabField.Tooltip);
                                if (tabField.TargetType.EqualsIgnoreCase("HelpCard"))
                                {
                                    width = "23";
                                    height = "75";
                                    fieldDisplayName = tabField.FieldDisplayName;
                                    href = string.Format(@"javascript:openHelpCard('{0}','{1}')", tabField.TargetURL, tabField.FieldDisplayName);
                                    fieldDisplayName = string.Format("<a title=\"{3}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{2}\" href=\"{1}\">{0}</a>", fieldDisplayName, href, currentModuleName, tabField.Tooltip);
                                }
                            }
                            else if (tabField.Tooltip != string.Empty && tabField.Tooltip != null)
                            {
                                fieldDisplayName = string.Format("<span title=\"{2}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{1}\">{0}</span>", fieldDisplayName, currentModuleName, tabField.Tooltip);

                            }
                            string fieldName = tabField.FieldName;
                            fieldWidth = tabField.FieldDisplayWidth;

                            #region layout Adjustment
                            //new condition for layout adjustment...
                            totalcount = totalcount + tabField.FieldDisplayWidth;

                            if (totalcount % 3 == 0)
                            {
                                fieldWidth = tabField.FieldDisplayWidth;
                            }
                            else if (totalcount % 3 == 1)
                            {
                                if (layoutItems.Count > loopcount + 1)
                                {
                                    if ((tabField.FieldDisplayWidth + layoutItems[loopcount + 1].FieldDisplayWidth) <= 3)
                                    {
                                        fieldWidth = tabField.FieldDisplayWidth;
                                    }
                                    else
                                    {
                                        fieldWidth = tabField.FieldDisplayWidth + 2;
                                        totalcount = totalcount + 2;
                                    }
                                }
                            }
                            else if (totalcount % 3 == 2)
                            {
                                if (layoutItems.Count > loopcount + 1)
                                {
                                    if ((tabField.FieldDisplayWidth + layoutItems[loopcount + 1].FieldDisplayWidth) <= 3)
                                    {
                                        fieldWidth = tabField.FieldDisplayWidth;
                                    }
                                    else
                                    {
                                        fieldWidth = tabField.FieldDisplayWidth + 1;
                                        totalcount = totalcount + 1;
                                    }
                                }
                            }
                            loopcount++;
                            #endregion
                            Table historyTable = new Table();

                            #region Render Form Layout Controls
                            #region Group
                            if (fieldName == "#GroupStart#" || fieldName == "#GroupEnd#")
                            {
                                if (fieldName == "#GroupStart#")
                                {
                                    tableType = Enums.TableDisplayTypes.Grouped;
                                    groupedTabTable = new Table();
                                    groupedTabTable.Attributes.Add("style", "width:100%");
                                    groupedTabTable.CssClass = "ticket_container";
                                    groupedPanelContainer = new Panel();
                                    isGroup = true;
                                }
                                else if (groupedPanelContainer != null && fieldName == "#GroupEnd#")
                                {
                                    if (fieldDisplayName != null)
                                    {
                                        groupedPanelContainer.GroupingText = Convert.ToString(fieldDisplayName);
                                    }
                                    else
                                    {
                                        groupedPanelContainer.GroupingText = "-";
                                    }
                                    groupedPanelContainer.Controls.Add(groupedTabTable);
                                    row = new TableRow();

                                    addToTableInSameCell(row, groupedPanelContainer, "", "", 3, tabTable);
                                    tableType = Enums.TableDisplayTypes.General;
                                    isGroup = false;
                                    groupedPanelContainer = null;
                                    columnCount = totalColumnsInDisplay;
                                }
                                row = new TableRow();
                            }
                            #endregion
                            #region Table
                            else if (fieldName == "#TableStart#" || fieldName == "#TableEnd#")
                            {
                                if (fieldName == "#TableStart#")
                                {
                                    if (row.Cells.Count > 0)
                                    {
                                        tableViewRowCount = 0;
                                        tabTable.Rows.Add(row);
                                        row = new TableRow();
                                    }
                                    tableType = Enums.TableDisplayTypes.TableLayout;
                                    tableViewTable = new Table();
                                    tableViewTable.CssClass = "pmm-constraints-table";
                                    //tableViewTable.BorderWidth = 1;
                                    //tableViewTable.Style.Add("border-collapse", "collapse");
                                    tableViewTable.Attributes.Add("rules", "all");
                                    tableViewRowCount = 0;
                                    tableCellsLength = fieldWidth;
                                    tableViewTable.Attributes.Add("style", "width:100%");

                                    string[] tableHeaders = fieldDisplayName.Split('#');
                                    legend = tableHeaders[0];
                                    foreach (string header in tableHeaders)
                                    {
                                        TableCell cell = new TableCell();
                                        string[] headerSplit = header.Split(';');
                                        cell.Text = headerSplit[0];
                                        cell.Style.Add("font-weight", "bold");
                                        // cell.BorderColor = Color.Black;
                                        //cell.BorderStyle = BorderStyle.NotSet;
                                        if (headerSplit.Length > 1)
                                            cell.Style.Add("width", headerSplit[1]);
                                        cell.CssClass = "ugit-tdetaillabel pmm-constraints-table-td";
                                        cell.HorizontalAlign = HorizontalAlign.Left;
                                        row.Cells.Add(cell);
                                    }
                                    row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                                    if (row.Cells.Count > 1)
                                        row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    tableViewTable.Rows.Add(row);
                                    row = new TableRow();
                                }
                                else if (fieldName == "#TableEnd#")
                                {
                                    if (row.Cells.Count > 0)
                                    {
                                        tableViewRowCount = 0;
                                        tableViewTable.Rows.Add(row);
                                        row = new TableRow();
                                    }
                                    foreach (TableRow tableRow in tableViewTable.Rows)
                                    {
                                        tableRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                                        if (tableRow.Cells.Count > 1)
                                            tableRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    Panel groupedPanel = new Panel();
                                    groupedPanel.GroupingText = fieldDisplayName;
                                    groupedPanel.Controls.Add(tableViewTable);
                                    columnCount += fieldWidth;
                                    if (columnCount > totalColumnsInDisplay)
                                    {
                                        for (int j = columnCount - fieldWidth; j < totalColumnsInDisplay; j++)
                                        {
                                            addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, tabTable);
                                        }
                                        columnCount = fieldWidth;
                                        row = new TableRow();
                                    }
                                    addToTableInSameCell(row, groupedPanel, "", "", fieldWidth, tabTable);
                                    tableType = Enums.TableDisplayTypes.General;
                                }
                                row = new TableRow();
                                row.BorderStyle = BorderStyle.None;
                                row.BorderWidth = 1;
                            }
                            #endregion
                            #region Control
                            else if (fieldName == "#Control#" || fieldName == "Comment")
                            {
                                //Deleget baseline parameters to all controls
                                //ModuleRoleWriteAccess roleWriteAccess = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStep || x.StageStep == 0) && x.FieldName.ToLower() == fieldDisplayName.ToLower());
                                roleWriteAccess = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStep || x.StageStep == 0) && x.FieldName.ToLower() == fieldDisplayName.ToLower());
                                long rowAccessID = 0;
                                if (roleWriteAccess != null)
                                    rowAccessID = roleWriteAccess.ID;

                                string baselineParams = string.Empty;
                                if (Request["showBaseline"] != null && UGITUtility.StringToBoolean(Request["showBaseline"].Trim()) && Request["baselineNum"] != null)
                                {
                                    baselineParams = string.Format("&showBaseline={0}&baselineNum={1}", Request["showBaseline"], Request["baselineNum"].Trim());
                                }

                                Control control = null;
                                if (fieldDisplayName.Replace(" ", "").Equals("ProjectPlanTimeSheet", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    //ProjectPlanTimeSheet ctr = (ProjectPlanTimeSheet)Page.LoadControl("/_CONTROLTEMPLATES/15/uGovernIT/ProjectPlanTimeSheet.ascx");
                                    //ctr.nprId = currentTicketId;
                                    //control = ctr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("NPRBudget", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=modulebudget{3}", Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("PMMTasksList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    //bool showTreeview = tabField.Prop_TreeView.HasValue ? tabField.Prop_TreeView.Value : false;
                                    //Guid newFrameId = Guid.NewGuid();
                                    //string url =  UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&pmmid={0}&ticketid={4}&showTreeview={1}&frameObjId={2}&control=PMMTasksList{3}",
                                    //                                    currentTicketId, showTreeview, newFrameId, baselineParams, currentTicketPublicID));
                                    //LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    //control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ProjectStatusDetail", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&pmmid={0}&frameObjId={1}&ticketId={3}&control=ProjectStatusDetail{2}",
                                                                                        currentTicketId, newFrameId, baselineParams, currentTicketPublicID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ProjectBudgetDetail", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=modulebudget{3}",
                                                                                        Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ProjectVarianceReport", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&pmmid={0}&frameObjId={1}&control=ProjectVarianceReport{2}",
                                                                                        currentTicketId, newFrameId, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ITGBudgetManagement", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={1}&isreadonly={0}&control=ITGBudgetManagement{2}",
                                                                                        !authorizedToEditTab, newFrameId, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("GovernanceReview", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&&frameObjId={0}&isreadonly={1}&control=GovernanceReview{2}",
                                                                                        newFrameId, !authorizedToEditTab, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ITGCommitteeReview", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={1}&isreadonly={0}&control=ITGCommitteeReview{2}",
                                                                                        !authorizedToEditTab, newFrameId, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ITGovernanceReview", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={0}&isreadonly={1}&control=ITGovernanceReview{2}",
                                                                                        newFrameId, !authorizedToEditTab, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ITGPortfolio", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=ITGPortfolio&isreadonly={1}{2}",
                                                                                        newFrameId, !authorizedToEditTab, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("NewProjectTask", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=NewProjectTask{3}",
                                                                                        Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("TaskListTree", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=tasklisttree{3}",
                                                                                        Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("History", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    bool isInFrame = false;//tabField.Prop_IsInFrame.HasValue ? tabField.Prop_IsInFrame.Value : false;

                                    if (isInFrame)
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketId={0}&frameObjId={1}&control=History&listName={2}{3}",
                                                                                            Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentTicket.Table.TableName, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else
                                    {
                                        History history = (History)Page.LoadControl("/ControlTemplates/Shared/History.ascx");
                                        history.ReadOnly = true;
                                        history.TicketId = currentTicketPublicID;
                                        history.ListName = this.currentModuleName;
                                        control = history;
                                    }
                                }

                                else if (fieldDisplayName.Equals("ProjectEstimatedAllocation", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=CRMProjectAllocationNew&isreadonly={2}&ticketId={0}&module={3}&rwID={4}",
                                                                                    currentTicketPublicID, newFrameId, !authorizedToEditTab, currentModuleName, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='250' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("CRMProjectAllocation", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=CRMProjectAllocationNew&isreadonly={2}&ticketId={0}&module={3}&ConfirmBeforeClose=true&rwID={4}",
                                                                                    currentTicketPublicID, newFrameId, !authorizedToEditTab, currentModuleName, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='250' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ProjectTeam", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=ProjectTeam&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='220' class='noMinimumHeight' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("DocumentControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=DocumentControl&ticketId={0}&module={2}",
                                                                                    currentTicketPublicID, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='220' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("TaskGraph", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=TaskGraph&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='235' class='noMinimumHeight' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("ProjectSummaryControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isModuleWebpartChild=true&frameObjId={1}&control=ProjectSummaryControl&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='235' class='noMinimumHeight' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("ProjectSummaryControlNew", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isModuleWebpartChild=true&frameObjId={1}&control=ProjectSummary&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='235' class='noMinimumHeight' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("TimelineControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&currentTicketId={0}&control=timelinecontrol&rwID={3}&ListName={4}&ticketID={5}",
                                                                                        //currentTicketId, currentModuleName, newFrameId, rowAccessID, currentTicket.ParentList.Title, currentTicketPublicID));
                                                                                        currentTicketId, currentModuleName, newFrameId, rowAccessID, string.Empty, currentTicketPublicID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("ResourceAvailability", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=resourceavailability&AllocationViewType=ProjectAllocation&isreadonly={2}&ticketId={0}{3}&moduleId={4}&pStartDate={5}&pEndDate={6}",
                                                                                    currentTicket[DatabaseObjects.Columns.TicketId].ToString(), newFrameId, !authorizedToEditTab, baselineParams, moduleId, currentTicket[DatabaseObjects.Columns.PreconStartDate], currentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }



                                else if (fieldDisplayName.Equals("ViewSubContractor", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=ViewSubContractor&rwID={3}{4}",
                                                                                        currentTicketPublicID, newFrameId, currentModuleName, rowAccessID, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }


                                else if (fieldDisplayName.Equals("Contacts", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&showSearchOption=false&control=Contacts&isreadonly={2}&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId, !authorizedToEditTab));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }

                                else if (fieldDisplayName.Equals("ContactActivity", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&showSearchOption=false&control=ContactActivity&isreadonly={2}&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId, !authorizedToEditTab));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }

                                else if (fieldDisplayName.Equals("RankingCriteriaView", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&showSearchOption=false&control=RankingCriteriaView&isreadonly={2}&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId, !authorizedToEditTab));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }





                                else if (fieldDisplayName.Equals("CheckListProjectView", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=CheckListProjectView&rwID={3}{4}",
                                                                                        currentTicketPublicID, newFrameId, currentModuleName, rowAccessID, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("RelatedCompanies", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=RelatedCompanies&rwID={3}{4}",
                                                                                        currentTicketPublicID, newFrameId, currentModuleName, rowAccessID, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("CPRProjectTitleControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=CPRProjectTitleControl&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='150' class='noMinimumHeight' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("AddProjectExperienceTags", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=AddProjectExperienceTags&ticketId={0}&from=agentTab",
                                                                                    currentTicketPublicID, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='150' class='noMinimumHeight' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("CRMOwnerContractDetails", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=CRMOwnerContractDetails&ticketId={0}&from=agentTab",
                                                                                    currentTicketPublicID, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' class='noMinimumHeight' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ResourcePlan", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=ResourcePlan&isreadonly={2}&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId, !authorizedToEditTab));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("Bid", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=Bid&isreadonly={2}&ticketId={0}",
                                                                                    currentTicketPublicID, newFrameId, !authorizedToEditTab));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("TaskTicketRelationship", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=TasksList&rwID={3}{4}",
                                                                                       Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, rowAccessID, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("CustomTicketRelationship", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=CustomTicketRelationship&isreadonly={2}&ticketId={0}{3}",
                                                                                    currentTicket[DatabaseObjects.Columns.TicketId].ToString(), newFrameId, !authorizedToEditTab, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("Tasks", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=TasksList&rwID={3}{4}&BatchEdit=true&MarkAsComplet=true&NewRecuringTask=true",
                                                                                       Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, rowAccessID, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));

                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("AssetRelatedWithAssets", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=AssetRelatedWithAssets&isreadonly={2}&Id={0}{3}&ticketId={4}",
                                                                                        currentTicket[DatabaseObjects.Columns.Id].ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentTicket[DatabaseObjects.Columns.TicketId].ToString()));

                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }

                                else if (fieldDisplayName.Replace(" ", "").Equals("ApplicationsRelatedAssetsView", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=ApplicationsRelatedAssetsView&isreadonly={2}&Id={0}{3}&ticketId={4}",
                                                                                        currentTicket[DatabaseObjects.Columns.Id].ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentTicket[DatabaseObjects.Columns.TicketId].ToString()));

                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("AssetRelatedWithTickets", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=AssetRelatedWithTickets&isreadonly={2}&Id={0}{3}&ticketId={4}",
                                                                                        currentTicket[DatabaseObjects.Columns.Id].ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentTicket[DatabaseObjects.Columns.TicketId].ToString()));

                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ImageOptionUserControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=ImageOptionUserControl&isreadonly={2}&Id={0}{3}&ticketId={4}",
                                                                                        currentTicket[DatabaseObjects.Columns.Id].ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentTicket[DatabaseObjects.Columns.TicketId].ToString()));

                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("AssetsStatusUserControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=AssetsStatusUserControl&isreadonly={2}&Id={0}{3}&ticketId={4}",
                                                                                        currentTicket[DatabaseObjects.Columns.Id].ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentTicket[DatabaseObjects.Columns.TicketId].ToString()));

                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("TicketRelationshipTree", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=TicketRelationshipTree&isreadonly={2}&ticketId={0}{3}&module={4}&Id={5}",
                                                                                    UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketId).ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentModuleName, currentTicket[DatabaseObjects.Columns.Id].ToString()));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("approvaltab", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=approvaltab&moduleName={1}&ticketID={2}&CurrentWorkflowStep={3}{4}",
                                                                                    newFrameId, currentModuleName, UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketId).ToString(), currentStep, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("TasksList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=NewProjectTask{3}&folderName=Tasks2",
                                                                                         Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ITGBudgetEditor", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&frameObjId={1}&isreadonly={0}&control=ITGBudgetEditor{2}",
                                                                                        !authorizedToEditTab, newFrameId, baselineParams));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ModuleInstRelations", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&frameObjId={2}&isreadonly={1}&control=ModuleInstRelations&ModuleName={3}",
                                                                                        currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("projectdocuments", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&frameObjId={2}&isreadonly={1}&control=projectdocuments&ModuleName={3}",
                                                                                        currentTicketPublicID, !authorizedToEditTab, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("applicationmodules", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ID={0}&frameObjId={2}&isreadonly={1}&control=applicationmodules&ModuleName={3}",
                                                                                        currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("applicationrole", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ID={0}&frameObjId={2}&isreadonly={1}&control=applicationrole&ModuleName={3}",
                                                                                        currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ModuleRoleMapControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ID={0}&frameObjId={2}&isreadonly={1}&control=modulerolemapcontrol&ModuleName={3}&ticketID={4}",
                                                                                        currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName, currentTicketPublicID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ServersControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ID={0}&frameObjId={2}&isreadonly={1}&control=applicationserver&ModuleName={3}",
                                                                                        currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("PasswordControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ID={0}&frameObjId={2}&isreadonly={1}&control=applicationpassword&ModuleName={3}&ticketID={4}",
                                                                                        currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName, currentTicketPublicID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("preconditionlist", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    string lookup = Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStep]);
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&publicID={0}&frameObjId={2}&isreadonly={1}&control=preconditionlist&ModuleName={3}&stageID={4}", currentTicketPublicID, !authorizedToEditTab, newFrameId, currentModuleName, lookup));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("WikiRelatedTickets", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=WikiRelatedTickets&isreadonly={2}&ticketId={0}{3}&moduleName={4}",
                                                                                    currentTicket[DatabaseObjects.Columns.TicketId].ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ProjectResourceDetail", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&Module={4}&projectPublicID={0}&frameObjId={1}&control=ProjectResourceDetail&isreadonly={2}{3}",
                                                                                        currentTicketPublicID, newFrameId, !authorizedToEditTab, baselineParams, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='250' height='250' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("CRMProjectAllocationViewNew", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=crmprojectallocationnew&ticketId={0}&module={2}&ConfirmBeforeClose=true",
                                                                                    currentTicketPublicID, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='220' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                //new line added for case..
                                else if (fieldDisplayName.Replace(" ", "").Equals("PMMEventsCalendar", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?isdlg=1&isudlg=1&Module={1}&projectPublicID={0}&control=PMMEventsCalendar&frameObjId={2}",
                                                                                        currentTicket[DatabaseObjects.Columns.TicketId].ToString(), currentModuleName, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='250' height='550'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Equals("DocumentControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=DocumentControl&ticketId={0}&module={2}",
                                                                                    currentTicketPublicID, newFrameId, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='220' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }


                                else if (fieldDisplayName.Replace(" ", "").Equals("PMMDocumentGrid", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=PMMDocumentGrid&ModuleName={4}&isreadonly={2}&docName={0}{3}",
                                                                                    Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]).Replace("-", "_"), newFrameId, !authorizedToEditTab, baselineParams, currentModuleName));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("Emails", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=ticketemails&frameObjId={2}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='10' height='200'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("PMMSprints", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=PMMSprints&frameObjId={2}&folderName=sprints&isTabActive=true",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("InvestmentsControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&isudlg=1&control=investmentcontrol&ModuleName={0}&InvestorID={2}",
                                                                                     currentModuleName, newFrameId, Convert.ToString(currentTicket[DatabaseObjects.Columns.Id])));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("DistributionControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&isudlg=1&control=distributioncontrol&ModuleName={0}&InvestorID={2}",
                                                                                    currentModuleName, newFrameId, Convert.ToString(currentTicket[DatabaseObjects.Columns.Id])));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorSOWControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorSOWControl&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VCCRequests", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=vendorvcccontrol&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorFMControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorFMControl&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorPMControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorPMControl&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorSOWFeeList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorSOWFeeList&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorReportList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorReportList&rwID={3}&ShowSelectedInstance={4}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID, Request["ShowSelectedInstance"]));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorMeetingList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorMeetingList&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorServiceDurationList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorServiceDurationList&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("vendorsladashboard", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=vendorsladashboard",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("vendorissuelist", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=vendorissuelist&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorRisksList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorRisksList&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorApprovedSubContractorsList", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorApprovedSubContractorsList&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("vendorkeypersonnellist", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=vendorkeypersonnellist&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorSOWContImprovementlist", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorSOWContImprovementlist&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("VendorPOControl", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorPOControl&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Replace(" ", "").Equals("scheduleactions", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=scheduleactions&IsModuleWebpart=true&IsArchive=false",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='10' minheight='130'  frameborder='0'  id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("scheduleactionsarchive", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=scheduleactions&IsModuleWebpart=true&IsArchive=true",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='10' minheight='130' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("proposallist", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isudlg=1&Module={1}&PublicTicketID={0}&Customerid={3}&control=proposallistcontrol",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, currentTicketId));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("ModuleDocumentGridview", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Panel pltr = new Panel();
                                    pltr.CssClass = "framepanel";
                                    string newFrameId = string.Format("{0}_{1}", tabField.TabId, fieldDisplayName);
                                    HiddenField hdn = new HiddenField();
                                    hdn.ID = string.Format("{0}_{1}", newFrameId, "mandatory");

                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=moduledocumentgridview&rwID={3}",
                                                                                        currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    pltr.Controls.Add(hdn);
                                    pltr.Controls.Add(lCtr);
                                    //control = lCtr;
                                    control = pltr;
                                }
                                else if (fieldDisplayName.Replace(" ", "").Replace(" ", "").Equals("ServiceQuestionSummary", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&currentTicketId={0}&control=servicequestionsummary&rwID={3}&ListName={4}",
                                                                                        currentTicketId, currentModuleName, newFrameId, rowAccessID, currentTicket["Title"]));

                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)'  frameurl='{0}' width='100%' height='250'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;


                                }
                                else if (fieldDisplayName.Replace(" ", "").Equals("AddComment", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    
                                    #region Versioned Fields Comments
                                    string fieldcol = TicketSchema.Columns[fieldName].ColumnName.ToString();
                                    if (fieldcol == DatabaseObjects.Columns.TicketComment)
                                    {
                                        row = new TableRow();
                                        bool oldestFirst = !ConfigurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
                                        List<HistoryEntry> historyList = uHelper.GetHistory(currentTicket, DatabaseObjects.Columns.TicketComment, oldestFirst);

                                        if (historyList.Count > 0)
                                        {
                                            string strUrl = Request.Url.AbsolutePath + "?TicketId=" + currentTicketPublicID;
                                            string strEditBtn = "<div class='ullisting'>";
                                            strEditBtn += "<ul>";

                                            strEditBtn += "<li runat='server' id='libtnAdd' class='' onmouseover=\"this.className=\'tabhover'\" onmouseout=\"this.className = ''\">";
                                            strEditBtn += "<a id='btnAdd' onclick='commentsAddPopup.Show();' title='Add'><img style='height:20px;width:20px;' src='/Content/Images/plus-blue-new.png'/></a>";
                                            strEditBtn += "</li>";
                                            strEditBtn += "</ul>";
                                            strEditBtn += " </div>";
                                            bool showPrivateComment = false;
                                            UserProfileManager userProfileManager = new UserProfileManager(context);
                                            // checking comment if the current user is in ticket owner, in PRP Group, TicketORP,TicketPRP
                                            if (IsAdmin || userProfileManager.CheckUserIsInGroup(PRPGroup, context.CurrentUser) ||
                                                 userProfileManager.IsUserPresentInField(context.CurrentUser, currentTicket, DatabaseObjects.Columns.TicketOwner) ||
                                                 userProfileManager.IsUserPresentInField(context.CurrentUser, currentTicket, DatabaseObjects.Columns.TicketORP) ||
                                                 userProfileManager.IsUserPresentInField(context.CurrentUser, currentTicket, DatabaseObjects.Columns.TicketPRP))
                                            {
                                                showPrivateComment = true;
                                            }

                                            int count = 0;
                                            historyList = historyList.OrderByDescending(o => UGITUtility.StringToDateTime(o.created)).ToList();
                                            foreach (HistoryEntry historyEntry in historyList)
                                            {
                                                if (historyEntry.createdBy == null)
                                                    continue; // Malformed entry, skip!
                                                              // Show all comment if the user is a ticket owner, in PRS Group, TicketORP,TicketPRP
                                                if (!historyEntry.IsPrivate || showPrivateComment || historyEntry.createdBy == context.CurrentUser.Name)
                                                {
                                                    // Add private icon if the comment is marked as private and also add an image button if requestor is notified for the comment
                                                    count++;
                                                    
                                                    row = new TableRow();
                                                    addToTable(row, strEditBtn, "", "", 3, groupedTabTable);
                                                }
                                            }
                                        }

                                        if (groupedTabTable.Rows.Count == 0)
                                        {
                                            //string strEditBtn = "No comments <div style='float:right;'> <a  onclick = 'commentsAddPopup.Show()' style ='padding-top: 10px;'>";
                                            //strEditBtn += "<span class='button-bg'>";
                                            //strEditBtn += "<b style = 'float: left; font-weight: normal;' > Add</b>";
                                            //strEditBtn += "<i style ='float: left; position: relative; top:0;' >";
                                            //strEditBtn += "<img src='/Content/images/add_icon.png' style='border: none;' title='' alt=''>";
                                            //strEditBtn += "</i>";
                                            //strEditBtn += "</span>";
                                            //strEditBtn += "</a></div>";

                                            string strEditBtn = "<div class='ullisting'>";
                                            strEditBtn += "<ul>";

                                            strEditBtn += "<li runat='server' id='libtnAdd' class='' onmouseover=\"this.className=\'tabhover'\" onmouseout=\"this.className = ''\">";
                                            strEditBtn += "<a id='btnAdd' onclick='commentsAddPopup.Show();' title='Add'><img style='height:20px;width:20px;' src='/Content/Images/plus-blue-new.png'/></a>";
                                            strEditBtn += "</li>";
                                            strEditBtn += "</ul>";
                                            strEditBtn += " </div>";


                                            row = new TableRow();

                                            addToTable(row, strEditBtn, "", "", 3, groupedTabTable);
                                        }
                                    }

                                    Guid newFrameId = Guid.NewGuid();
                                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&Module={1}&TicketID={0}&control=editcomment&frameObjId={2}&listName{3}&ctype=Comment&currentTicket{4}",
                                                        currentTicketPublicID, currentModuleName, newFrameId, currentTicket.Table.TableName, currentTicket));
                                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='10' height='200'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    control = lCtr;

                                    #endregion


                                }
                                if (control != null)
                                {
                                    if (isGroup && groupedPanelContainer != null)
                                    {
                                        //addToTableInSameCell(row, control, string.Empty, string.Empty, 3, tabTable);
                                        //groupedPanelContainer.Controls.Add(control);
                                        row = new TableRow();
                                        addToTableInSameCell(row, control, string.Empty, string.Empty, 3, groupedTabTable);
                                        row = new TableRow();
                                    }
                                    else
                                    {
                                        Panel groupedPanel;
                                        groupedPanel = new Panel();
                                        groupedPanel.Controls.Add(control);
                                        addToTableInSameCell(row, groupedPanel, string.Empty, string.Empty, 3, tabTable);
                                        row = new TableRow();
                                    }
                                    columnCount = totalColumnsInDisplay;
                                }
                            }
                            #endregion
                            #region Label
                            else if (fieldName == "#Label#")
                            {
                                Label label = new Label();
                                label.Text = fieldDisplayName;

                                if (tableType == Enums.TableDisplayTypes.Grouped)
                                    addToTable(row, label, fieldDisplayName, 1, groupedTabTable);
                                else if (tableType == Enums.TableDisplayTypes.General)
                                    addToTable(row, label, fieldDisplayName, 1);
                                else if (tableType == Enums.TableDisplayTypes.TableLayout)
                                {
                                    if (tableViewRowCount >= tableCellsLength)
                                    {
                                        tableViewRowCount = 0;
                                        tableViewTable.Rows.Add(row);
                                        row = new TableRow();
                                    }
                                    addToTableInSameCellTableLayout(row, label, 1, tableViewTable, !(label.Text.Contains("Notes")));

                                }

                            }
                            #endregion
                            #region Empty
                            else if (fieldName == "#PlaceHolder#")
                            {
                                Label label = new Label();
                                if (tableType != Enums.TableDisplayTypes.TableLayout)
                                {
                                    columnCount += fieldWidth;
                                    if (columnCount > totalColumnsInDisplay)
                                    {
                                        columnCount = fieldWidth;
                                        row = new TableRow();
                                    }
                                }

                                if (tableType == Enums.TableDisplayTypes.Grouped)
                                    addToTicketTable(row, label, string.Empty, fieldWidth, groupedTabTable);
                                else if (tableType == Enums.TableDisplayTypes.General)
                                    addToTable(row, label, string.Empty, fieldWidth);
                                else if (tableType == Enums.TableDisplayTypes.TableLayout)
                                {
                                    if (tableViewRowCount >= tableCellsLength)
                                    {
                                        tableViewRowCount = 0;
                                        tableViewTable.Rows.Add(row);
                                        row = new TableRow();
                                    }
                                    addToTableInSameCellTableLayout(row, label, fieldWidth, tableViewTable);
                                }
                            }
                            #endregion
                            #region Rest of Controls
                            else
                            {
                                if (TicketSchema.Columns.Contains(fieldName))
                                {
                                    DataColumn fieldColumn = TicketSchema.Columns[fieldName];
                                    FieldDesignMode fieldDesignMode = FieldDesignMode.Normal;
                                    RequiredFieldValidator rfv = null;
                                    //by Manish Hada, if current user is not authenticated then not a single field should be in edit mode.
                                    ControlMode ctrMode = ControlMode.Display;
                                    if (isActionUser)
                                    {
                                        ctrMode = ControlMode.Edit;
                                    }

                                    //get issue type
                                    if (fieldName == DatabaseObjects.Columns.Category)
                                    {
                                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.Category, currentTicket.Table))
                                        {
                                            issueType = Convert.ToString(currentTicket[DatabaseObjects.Columns.Category]);
                                        }
                                    }
                                    //Get resolution type
                                    if (fieldName == DatabaseObjects.Columns.TicketResolutionType)
                                    {
                                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketResolutionType, currentTicket.Table))
                                        {
                                            resolutionType = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketResolutionType]);
                                        }
                                    }
                                    Control bfc = ticketControls.GetControls(fieldColumn, ctrMode, fieldDesignMode, "", tabField);
                                    if (bfc != null)
                                    {
                                        //Todo
                                        //if (!string.IsNullOrEmpty(Request[hdnMacroTicketTemplate.UniqueID]) && Convert.ToInt32(Request[hdnMacroTicketTemplate.UniqueID]) > 0 && ctrMode == ControlMode.Edit)
                                        //    MacroTicket(bfc, f);

                                        #region Versioned Fields TicketResolutionComments
                                        DataTable ticketHours = null;
                                        if (currentTicket != null && Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ID]) != 0 && fieldName == DatabaseObjects.Columns.ResolutionComments)
                                        {
                                            ticketHours = GetActualHoursList();
                                            List<HistoryEntry> historyList = uHelper.GetHistory(currentTicket, DatabaseObjects.Columns.TicketResolutionComments, false);

                                            if (historyList.Count > 0 && (!TicketRequest.Module.ActualHoursByUser || ticketHours == null || ticketHours.Rows.Count == 0))
                                            {
                                                // new line of code for edit buton....
                                                string strUrl = Request.Url.AbsolutePath + "?TicketId=" + currentTicketPublicID;
                                                string strEditbtn = string.Empty;
                                                if (IsAdmin && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowCommentDelete))
                                                    strEditbtn = "<div style='float:right;'> <img src='/Content/images/editNewIcon.png' onclick=getTicketResolutionComment('" + currentTicket["TicketId"] + "','" + currentTicket.Table.TableName + "','" + Server.UrlEncode(strUrl) + "') style='cursor:pointer'/></div>";

                                                foreach (HistoryEntry historyEntry in historyList)
                                                {
                                                    TableRow historyRow = new TableRow();
                                                    string txt = historyEntry.entry.Contains("\n") ? "<br/>" + historyEntry.entry : historyEntry.entry;
                                                    txt = uHelper.FindAndConvertToAnchorTag(txt);
                                                    addToTableInSameCell(historyRow, strEditbtn + "<span style='white-space:pre-wrap'><b>[" + historyEntry.created + "] " + historyEntry.createdBy + ":</b> " + txt + "</span>", string.Empty, 3, historyTable);
                                                    strEditbtn = string.Empty;
                                                }
                                            }
                                            row = new TableRow();

                                            //List<HistoryEntry> historyList = uHelper.GetHistory(currentTicket, DatabaseObjects.Columns.ResolutionComments, false);
                                            //if (historyList.Count > 0)
                                            //{

                                            //    // new line of code for edit buton....
                                            //    string strUrl = Request.Url.AbsolutePath + "?TicketID=" + currentTicketPublicID;
                                            //    string strEditbtn = string.Empty;
                                            //    if (IsAdmin && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowCommentDelete))
                                            //        strEditbtn = "<div style='float:right;'> <img src='/Content/Images/editNewIcon.png' onclick=getTicketResolutionComment('" + currentTicket["TicketId"] + "','" + currentTicket.Table.TableName + "','" + Server.UrlEncode(strUrl) + "') style='cursor:pointer; width:20px;'/> </div>";

                                            //    UserProfile historyUser = null;
                                            //    foreach (HistoryEntry historyEntry in historyList)
                                            //    {
                                            //        if (!string.IsNullOrEmpty(historyEntry.createdBy))
                                            //        {
                                            //            historyUser = UserManager.GetUserById(historyEntry.createdBy);
                                            //            if (historyUser != null)
                                            //            {
                                            //                historyEntry.createdBy = historyUser.Name;
                                            //                historyEntry.Picture = historyUser.Picture;
                                            //            }
                                            //        }
                                            //        TableRow historyRow = new TableRow();
                                            //        if (!string.IsNullOrEmpty(historyEntry.entry))
                                            //        {
                                            //            string txt = historyEntry.entry.Contains("\n") ? "</br>" + historyEntry.entry : historyEntry.entry;
                                            //            txt = UGITUtility.FindAndConvertToAnchorTag(txt);
                                            //            historyEntry.entry = txt;
                                            //            //if (!TicketRequest.Module.ActualHoursByUser)
                                            //                //addToTableInSameCell(historyRow, strEditbtn + "<span style='white-space:pre-wrap'><b>[" + historyEntry.created + "] " + historyEntry.createdBy + ":</b> " + txt + "</span>", string.Empty, 3, historyTable);
                                            //                addToCommentsTable(historyRow, historyEntry, strEditbtn, 3, historyTable);
                                            //        }
                                            //        strEditbtn = string.Empty;
                                            //    }
                                            //}
                                            //row = new TableRow();
                                            //row.BorderStyle = BorderStyle.Solid;
                                            //row.BorderWidth = 1;
                                        }
                                        #endregion
                                        // Get the roleWriteAccess from cache
                                        // Select for current step.
                                        roleWriteAccess = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStep || x.StageStep == 0) && x.FieldName == TicketSchema.Columns[fieldName].ColumnName);
                                        roleWriteAccessTemp = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => x.FieldName == TicketSchema.Columns[fieldName].ColumnName);

                                        //check the value of editbutton and showwithcheckbox value.
                                        bool editButton = false;
                                        if (roleWriteAccess != null)
                                        {
                                            if (roleWriteAccess.ShowEditButton && roleWriteAccess.ShowWithCheckbox)
                                                fieldDesignMode = FieldDesignMode.WithEditAndCheckbox;
                                            else if (roleWriteAccess.ShowEditButton)
                                            {
                                                ctrMode = ControlMode.Edit;
                                                fieldDesignMode = FieldDesignMode.WithEdit;
                                            }
                                            else if (roleWriteAccess.ShowWithCheckbox)
                                            {
                                                fieldDesignMode = FieldDesignMode.WithCheckbox;
                                                fieldWidth = tabField.FieldDisplayWidth;
                                            }
                                            editButton = roleWriteAccess.ShowEditButton;
                                            //showWithCheckBox = roleWriteAccess.ShowWithCheckbox;

                                            //As pre the BTS-23-001219 request below line commented 
                                            // bfc = ticketControls.GetControls(fieldColumn, ctrMode, fieldDesignMode, "", tabField);
                                            //bfc = ticketControls.GetControls(fieldColumn,ctrMode,fieldDesignMode, "", tabField.TabId.ToString(), tabField.FieldDisplayName);
                                        }

                                        try
                                        {
                                            if (roleWriteAccess != null || (adminOverride && roleWriteAccessTemp != null))
                                            {
                                                string fieldLevelUserType = string.Empty;
                                                //Check for FieldLevel authorization
                                                if (roleWriteAccess != null)
                                                    fieldLevelUserType = roleWriteAccess.ActionUser;

                                                bool fieldLevelAuthority = false;
                                                if (fieldLevelUserType != string.Empty)
                                                {
                                                    if (UserManager.IsUserPresentInField(user, currentTicket, fieldLevelUserType, true))
                                                    {
                                                        fieldLevelAuthority = true;
                                                        isFieldLevelAccessIsPersent = true;
                                                    }
                                                }

                                                if (isActionUser || fieldLevelAuthority || fieldName == DatabaseObjects.Columns.TicketComment || adminOverride)
                                                {
                                                    ControlMode mode = ControlMode.Edit;

                                                    // If in Admin Override mode, show in full edit mode without edit button
                                                    if (adminOverride)
                                                    {
                                                        editButton = false;
                                                        fieldDesignMode = FieldDesignMode.Normal;
                                                    }

                                                    // Allow only selected groups like Admin, to edit ProjectId, can be set from Admin -> FormLayout -> Permissions.
                                                    if (fieldName == DatabaseObjects.Columns.ProjectID && fieldLevelAuthority == false)
                                                    {
                                                        editButton = false;
                                                        mode = ControlMode.Display;
                                                    }

                                                    // If printing keep in display mode
                                                    // Also cannot change TicketInitiatorResolved once set since it controls the workflow
                                                    if (printEnable || (fieldName == DatabaseObjects.Columns.TicketInitiatorResolved && currentModuleName == "PRS" && currentStep == 1))
                                                    {
                                                        mode = ControlMode.Display;
                                                        editButton = false;
                                                    }

                                                    if (fieldName == DatabaseObjects.Columns.TicketAnalysisDetails)
                                                    {
                                                        editButton = true;
                                                    }

                                                    // If in override mode, prevent existing comments shown in edit textbox else keeps appending existing data
                                                    if (adminOverride && fieldName == DatabaseObjects.Columns.TicketResolutionComments)
                                                    {
                                                        mode = ControlMode.New;
                                                    }
                                                    bfc = ticketControls.GetControls(fieldColumn, mode, fieldDesignMode, "", tabField);

                                                    //TODO
                                                    //if (!string.IsNullOrEmpty(Request[hdnMacroTicketTemplate.UniqueID]) && Convert.ToInt32(Request[hdnMacroTicketTemplate.UniqueID]) > 0 && mode == ControlMode.Edit)
                                                    //    MacroTicket(bfc, f);

                                                    //Add Actual Hours by User
                                                }

                                                if (roleWriteAccess != null && roleWriteAccess.FieldMandatory)
                                                {
                                                    fieldDisplayName += "<b style=\"color:red\">* </b>";
                                                    rfv = new RequiredFieldValidator();
                                                    rfv.ControlToValidate = bfc.ID;
                                                }
                                                if (fieldName.Contains("Analytics"))
                                                {
                                                    fieldDisplayName += "<img src='/Content/images/analytics.png' onclick=getScore('" + tabRepeater.ClientID + "_ctl00_" + bfc.ClientID + "') style='cursor:pointer' />";
                                                }
                                                else if (fieldName.Contains("Date"))
                                                {

                                                }
                                            }
                                            else
                                            {
                                                //TODO
                                                //if (fieldName.Contains("Analytics") && (((NumberField)bfc).Value == null || Convert.ToString(((NumberField)bfc).Value) != string.Empty))
                                                //{
                                                //    row = new TableRow();
                                                //    continue;
                                                //}
                                                bfc = ticketControls.GetControls(fieldColumn, ControlMode.Display, fieldDesignMode, "", tabField);

                                                //TODO
                                                ////if (!string.IsNullOrEmpty(Request[hdnMacroTicketTemplate.UniqueID]) && Convert.ToInt32(Request[hdnMacroTicketTemplate.UniqueID]) > 0)
                                                ////    MacroTicket(bfc, f);

                                                ////Remove comment add label if it is not in edit more. Show comment trail only.
                                                if (fieldName == DatabaseObjects.Columns.TicketComment)
                                                {
                                                    bfc = null;
                                                }
                                            }

                                            if (fieldName == DatabaseObjects.Columns.TicketOwner)
                                            {
                                                incidentOwnerId = requestOwnerIdDisplayed = tabRepeater.ClientID + "_ctl00_" + bfc.ClientID;
                                            }
                                            else if (fieldName == DatabaseObjects.Columns.TicketPriority)
                                            {
                                                priorityIdDisplayed = tabRepeater.ClientID + "_ctl00_" + bfc.ClientID;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            //TODO
                                            Util.Log.ULog.WriteException(ex);
                                        }

                                        if (tableType != Enums.TableDisplayTypes.TableLayout)
                                        {
                                            columnCount += fieldWidth;
                                            if (columnCount > totalColumnsInDisplay && fieldName != "ResolutionComments")//!TicketSchema.Columns.Contains("TicketResolutionComments"))
                                            {
                                                for (int j = columnCount - fieldWidth; j < totalColumnsInDisplay; j++)
                                                {
                                                    if (tableType == Enums.TableDisplayTypes.Grouped)
                                                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                                                    else if (tableType == Enums.TableDisplayTypes.General)
                                                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1);
                                                }
                                                columnCount = fieldWidth;
                                                row = new TableRow();
                                                row.BorderStyle = BorderStyle.Solid;
                                                row.BorderWidth = 1;
                                            }
                                        }

                                        if (fieldName == (DatabaseObjects.Columns.TicketBusinessManager)
                                            || fieldName == (DatabaseObjects.Columns.TicketGLCode)
                                            || fieldName == (DatabaseObjects.Columns.TicketTester))
                                        {
                                            row.CssClass += " " + TicketSchema.Columns[fieldName].ColumnName + "Row";
                                        }

                                        if (TicketSchema.Columns[fieldName].ColumnName.Contains("HoldDuration"))
                                        {
                                            Literal ltrlHoldTime = new Literal();
                                            ltrlHoldTime.Text = $"<div class='editTicket_holdTime'>{uHelper.GetFormattedHoldTime(context, currentTicket).ToString()}</div>";
                                            bfc = (Control)ltrlHoldTime;
                                        }
                                        if (historyTable.Rows.Count < 1)
                                        {

                                            if (tableType == Enums.TableDisplayTypes.Grouped)
                                                //addToTable(row, bfc, fieldDisplayName, fieldWidth, groupedTabTable);
                                                addToTicketTable(row, bfc, fieldDisplayName, fieldWidth, groupedTabTable, null, null, fieldName);
                                            else if (tableType == Enums.TableDisplayTypes.General)
                                                addToTable(row, bfc, fieldDisplayName, fieldWidth);
                                            else if (tableType == Enums.TableDisplayTypes.TableLayout)
                                            {
                                                if (tableViewRowCount >= tableCellsLength)
                                                {
                                                    tableViewRowCount = 0;
                                                    tableViewTable.Rows.Add(row);
                                                    row = new TableRow();
                                                }
                                                addToTableInSameCellTableLayout(row, bfc, fieldWidth, tableViewTable);
                                            }
                                        }
                                        else
                                        {
                                            if (tableType == Enums.TableDisplayTypes.Grouped)
                                                //addToTable(row, bfc, fieldDisplayName, fieldWidth, groupedTabTable, (Control)historyTable);
                                                addToTicketTable(row, bfc, fieldDisplayName, fieldWidth, groupedTabTable, (Control)historyTable);
                                            else if (tableType == Enums.TableDisplayTypes.General)
                                                addToTable(row, bfc, fieldDisplayName, fieldWidth, (Control)historyTable);

                                        }

                                        #region Versioned Fields Comments
                                        string fieldcol = TicketSchema.Columns[fieldName].ColumnName.ToString();
                                        if (fieldcol == DatabaseObjects.Columns.TicketComment)
                                        {
                                            row = new TableRow();
                                            bool oldestFirst = !ConfigurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
                                            List<HistoryEntry> historyList = uHelper.GetHistory(currentTicket, DatabaseObjects.Columns.TicketComment, oldestFirst);

                                            if (historyList.Count > 0)
                                            {
                                                string strUrl = Request.Url.AbsolutePath + "?TicketId=" + currentTicketPublicID;
                                                string strEditBtn = "<div class='ullisting'>";
                                                strEditBtn += "<ul>";

                                                bool isCommentOwner = historyList.Any(x => !string.IsNullOrEmpty(x.createdBy) && x.createdBy.Equals(context.CurrentUser.Name));
                                                if ((IsAdmin || isCommentOwner) && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowCommentDelete))
                                                {
                                                    strEditBtn += "<li runat='server' id='libtnEdit' class='' onmouseover=\"this.className=\'tabhover'\" onmouseout=\"this.className = ''\">";
                                                    strEditBtn += "<a id='btnEdit' onclick=getTicketComment('" + currentTicket[DatabaseObjects.Columns.TicketId] + "','" + currentTicket.Table.TableName + "','" + Server.UrlEncode(strUrl) + "')  title='Edit'><img style='height:20px;width:20px;' src='/Content/Images/editNewIcon.png'/></a>";
                                                    strEditBtn += "</li>";
                                                }
                                                strEditBtn += "<li runat='server' id='libtnAdd' class='' onmouseover=\"this.className=\'tabhover'\" onmouseout=\"this.className = ''\">";
                                                strEditBtn += "<a id='btnAdd' onclick='commentsAddPopup.Show();' title='Add'><img style='height:20px;width:20px;' src='/Content/Images/plus-blue-new.png'/></a>";
                                                strEditBtn += "</li>";
                                                strEditBtn += "</ul>";
                                                strEditBtn += " </div>";
                                                bool showPrivateComment = false;
                                                UserProfileManager userProfileManager = new UserProfileManager(context);
                                                // checking comment if the current user is in ticket owner, in PRP Group, TicketORP,TicketPRP
                                                if (IsAdmin || userProfileManager.CheckUserIsInGroup(PRPGroup, context.CurrentUser) ||
                                                     userProfileManager.IsUserPresentInField(context.CurrentUser, currentTicket, DatabaseObjects.Columns.TicketOwner) ||
                                                     userProfileManager.IsUserPresentInField(context.CurrentUser, currentTicket, DatabaseObjects.Columns.TicketORP) ||
                                                     userProfileManager.IsUserPresentInField(context.CurrentUser, currentTicket, DatabaseObjects.Columns.TicketPRP))
                                                {
                                                    showPrivateComment = true;
                                                }

                                                int count = 0;
                                                historyList = historyList.OrderByDescending(o => UGITUtility.StringToDateTime(o.created)).ToList();
                                                foreach (HistoryEntry historyEntry in historyList)
                                                {
                                                    if (historyEntry.createdBy == null)
                                                        continue; // Malformed entry, skip!
                                                    // Show all comment if the user is a ticket owner, in PRS Group, TicketORP,TicketPRP
                                                    if (!historyEntry.IsPrivate || showPrivateComment || historyEntry.createdBy == context.CurrentUser.Name)
                                                    {
                                                        // Add private icon if the comment is marked as private and also add an image button if requestor is notified for the comment
                                                        string privateIcon = string.Empty;
                                                        string notificationBtn = string.Empty;

                                                        if (historyEntry.IsPrivate && showPrivateComment)
                                                        {
                                                            privateIcon += "<img src='/Content/images/lock16X16.png' style='border: none; vertical-align:bottom; padding-right: 5px; width:18px;' title='Private' alt=''>";
                                                        }

                                                        if (!string.IsNullOrEmpty(historyEntry.NotificationID) && IsTicketEmailExist(UGITUtility.StringToInt(historyEntry.NotificationID)))
                                                        {
                                                            notificationBtn += "<a  onclick=showTicketEmail('" + historyEntry.NotificationID + "','" + currentTicketPublicID + "','" + Server.UrlEncode(strUrl) + "') style ='padding-right: 5px;'>";
                                                            notificationBtn += "<img src='/Content/images/notify16X16.png' style='border: none; vertical-align:bottom; cursor: pointer; width:18px;' title='Notified to Requestor' alt=''>";
                                                            notificationBtn += "</a>";
                                                        }

                                                        var userProfile = UserManager.GetUserInfoByIdOrName(historyEntry.createdBy);
                                                        if (userProfile != null)
                                                        {
                                                            if (!string.IsNullOrEmpty(userProfile.Name))
                                                                historyEntry.createdBy = userProfile.Name;

                                                            if (!string.IsNullOrEmpty(userProfile.Picture) && System.IO.File.Exists(Server.MapPath(userProfile.Picture)))
                                                                historyEntry.Picture = userProfile.Picture;
                                                            else
                                                                historyEntry.Picture = "/Content/Images/userNew.png";
                                                        }
                                                        else
                                                        {
                                                            historyEntry.Picture = "/Content/Images/userNew.png";
                                                        }

                                                        string wrapperClass = count % 2 == 1 ? " aproval-alternate-row" : string.Empty;
                                                        count++;

                                                        if (tableType == Enums.TableDisplayTypes.Grouped)
                                                        {
                                                            string txt = historyEntry.entry.Contains("\n") ? "<br/>" + historyEntry.entry : historyEntry.entry;
                                                            txt = uHelper.FindAndConvertToAnchorTag(txt);
                                                            string htmlContent = string.Format("<div class=\"col-md-12 col-sm-12 col-xs-12 history_wrap{5}\"><div class=\"row comment-wrap\"><div class=\"col-md-2 col-sm-4 col-xs-4 imgPadding\">"
                                                                + "<div class=\" history_img\"><img src={0} /><label class=\"history_img_name\">{1}</label></div></div>"
                                                                + "<div class=\"col-md-2 col-sm-4 col-xs-12 date_time_padding\"><div class=\"history_date_time\"><span>{2}</span></div></div>"
                                                                + "<div class=\"col-md-8 col-sm-4 col-xs-8 comment-padding\"><div class=\"history_data\"><p>{4} {3}</p></div></div></div></div>", historyEntry.Picture, historyEntry.createdBy
                                                                , historyEntry.created, txt, privateIcon + notificationBtn, wrapperClass);
                                                            addToTable(row, strEditBtn + htmlContent, "", "&nbsp;", 3, groupedTabTable);
                                                            strEditBtn = string.Empty;
                                                        }
                                                        else if (tableType == Enums.TableDisplayTypes.General)
                                                        {
                                                            string txt = historyEntry.entry.Contains("\n") ? "<br/>" + historyEntry.entry : historyEntry.entry;
                                                            string htmlContent = string.Format("<div class=\"col-md-12 col-sm-12 col-xs-12 history_wrap{5}\"><div class=\"row comment-wrap\"><div class=\"col-md-2 col-sm-4 col-xs-4 imgPadding\">"
                                                                + "<div class=\" history_img\"><img src={0} /><label class=\"history_img_name\">{1}</label></div></div>"
                                                                + "<div class=\"col-md-2 col-sm-4 col-xs-12 date_time_padding\"><div class=\"history_date_time\"><span>{2}</span></div></div>"
                                                                + "<div class=\"col-md-8 col-sm-4 col-xs-8 comment-padding\"><div class=\"history_data\"><p>{4} {3}</p></div></div></div></div>", historyEntry.Picture, historyEntry.createdBy
                                                                , historyEntry.created, txt, privateIcon + notificationBtn, wrapperClass);
                                                            addToTable(row, htmlContent, "", "", 3, historyTable);
                                                        }

                                                        row = new TableRow();

                                                    }
                                                }
                                            }

                                            if (groupedTabTable.Rows.Count == 0)
                                            {
                                                string strEditBtn = "No comments <div style='float:right;'> <a  onclick = 'commentsAddPopup.Show()' style ='padding-top: 10px;'>";
                                                strEditBtn += "<span class='button-bg'>";
                                                strEditBtn += "<b style = 'float: left; font-weight: normal;' > Add</b>";
                                                strEditBtn += "<i style ='float: left; position: relative; top:0;' >";
                                                strEditBtn += "<img src='/Content/images/add_icon.png' style='border: none;' title='' alt=''>";
                                                strEditBtn += "</i>";
                                                strEditBtn += "</span>";
                                                strEditBtn += "</a></div>";

                                                row = new TableRow();

                                                addToTable(row, strEditBtn, "", "", 3, groupedTabTable);
                                            }


                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion
                            #endregion
                        }

                        if (columnCount < totalColumnsInDisplay)
                        {

                            for (int j = columnCount; j < totalColumnsInDisplay; j++)
                            {
                                if (tableType == Enums.TableDisplayTypes.Grouped)
                                    addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                                else if (tableType == Enums.TableDisplayTypes.General)
                                    addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1);

                            }
                        }
                    }
                    else
                    {
                        Label unAuthorized = new Label();
                        unAuthorized.Text = "Access Denied.";

                        TableCell newCell = new TableCell();
                        newCell.ColumnSpan = 4;
                        newCell.Controls.Add(unAuthorized);
                        row.Cells.Add(newCell);
                        tabTable.Rows.Add(row);
                        columnCount = totalColumnsInDisplay;
                    }
                    ((Panel)e.Item.FindControl("tabContainer")).Controls.Add(tabTable);
                }
            }
            #endregion
            #region Batch editing
            if (Request.QueryString["BatchEditing"] == "true")
            {
                ticketControls.BatchEdit = BatchMode.Edit;

                if (spListitemCollection != null && spListitemCollection.Length > 0)
                {
                    DataTable dtItems = spListitemCollection.CopyToDataTable();
                    ticketControls.DataList = dtItems.Copy();
                    //DataView view = new DataView(dtItems);
                    if (null != e.Item.FindControl("tabContainer") && null != e.Item.DataItem)
                    {
                        tabTable = new Table();
                        tabTable.EnableTheming = true;
                        tabTable.Rows.Clear();
                        tabTable.Attributes.Add("style", "border-collapse:collapse;width:100%");
                        int columnCount = 0, fieldWidth = 1;
                        TableRow row = new TableRow();
                        ModuleFormTab formTab = (ModuleFormTab)e.Item.DataItem;
                        //Check authorization to view tab
                        bool authorizedToViewTab = true, authorizedToEditTab = true;
                        List<string> authorizedToViewList = UGITUtility.ConvertStringToList(formTab.AuthorizedToView, uGovernIT.Utility.Constants.Separator6);
                        List<string> authorizedToEditList = UGITUtility.ConvertStringToList(formTab.AuthorizedToEdit, uGovernIT.Utility.Constants.Separator6);

                        if (authorizedToViewList.Count > 0)
                        {
                            authorizedToViewTab = false;
                            if (UserManager.IsUserExistInList(user, authorizedToViewList))
                            {
                                authorizedToViewTab = true;
                            }
                        }
                        //Check authorization to edit tab
                        if (authorizedToEditList.Count > 0)
                        {
                            authorizedToEditTab = false;
                            if (UserManager.IsUserExistInList(user, authorizedToViewList))
                            {
                                authorizedToEditTab = true;
                            }
                        }
                        if (authorizedToViewTab || authorizedToEditTab)
                        {
                            //SPQuery oQuery = new SPQuery();
                            HiddenField tabIdHidden = new HiddenField();
                            tabIdHidden.ID = "tabIdHidden";
                            tabIdHidden.Value = formTab.TabId.ToString();
                            ((Panel)e.Item.FindControl("tabContainer")).Controls.Add(tabIdHidden);

                            Table mainTable = tabTable;
                            string legend = string.Empty;
                            bool isGroup = false;
                            Panel groupedPanelContainer = null;
                            double tabID = Convert.ToDouble(tabIdHidden.Value);

                            //List<ModuleFormLayout> layoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabNumber == tabID && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();
                            List<ModuleFormLayout> alllayoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabId == tabID && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();

                            List<ModuleFormLayout> layoutItems = new List<ModuleFormLayout>();
                            foreach (ModuleFormLayout removetabField in alllayoutItems)
                            {
                                // Add field to form if no skip condition configured OR if skip condition evaluates to false (i.e. don't skip)
                                if (removetabField.FieldName != DatabaseObjects.Columns.Attachments && (string.IsNullOrEmpty(removetabField.SkipOnCondition) ||
                                    FormulaBuilder.EvaluateFormulaExpression(context, removetabField.SkipOnCondition, currentTicket) == false))
                                {
                                    layoutItems.Add(removetabField);
                                }
                            }

                            int totalcount = 0;
                            int loopcount = 0;
                            foreach (ModuleFormLayout tabField in layoutItems)
                            {
                                string fieldDisplayName = tabField.FieldDisplayName;
                                if (tabField.TargetURL != string.Empty)
                                {
                                    //Dictionary<string, string> customProperties = uHelper.GetCustomProperties(tabField.TargetURL, Constants.Separator);
                                    //KeyValuePair<string, string> pageParam = customProperties.FirstOrDefault(x => x.Key.Trim().ToLower() == "fileurl");
                                    string width = "100";
                                    string height = "100";
                                    string href = string.Format(@"javascript:UgitOpenPopupDialog('{0}','','Help - {2}','{3}','{4}',0,'{1}')", tabField.TargetURL, Server.UrlEncode(Request.Url.AbsolutePath), tabField.FieldDisplayName, width, height);
                                    fieldDisplayName = string.Format("<a title=\"{3}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{2}\" href=\"{1}\">{0}</a>", fieldDisplayName, href, currentModuleName, tabField.Tooltip);
                                    if (tabField.TargetType.EqualsIgnoreCase("HelpCard"))
                                    {
                                        width = "23";
                                        height = "75";
                                        fieldDisplayName = tabField.FieldDisplayName;
                                        href = string.Format(@"javascript:openHelpCard('{0}','{1}')", tabField.TargetURL, tabField.FieldDisplayName);
                                        fieldDisplayName = string.Format("<a title=\"{3}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{2}\" href=\"{1}\">{0}</a>", fieldDisplayName, href, currentModuleName, tabField.Tooltip);
                                    }


                                }
                                else if (tabField.Tooltip != string.Empty)
                                {
                                    fieldDisplayName = string.Format("<span title=\"{2}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{1}\">{0}</span>", fieldDisplayName, currentModuleName, tabField.Tooltip);
                                }
                                string fieldName = tabField.FieldName;
                                fieldWidth = tabField.FieldDisplayWidth;

                                #region layout Adjustment
                                //new condition for layout adjustment...
                                totalcount = totalcount + tabField.FieldDisplayWidth;

                                if (totalcount % 3 == 0)
                                {
                                    fieldWidth = tabField.FieldDisplayWidth;
                                }
                                else if (totalcount % 3 == 1)
                                {
                                    if (layoutItems.Count > loopcount + 1)
                                    {
                                        if ((tabField.FieldDisplayWidth + layoutItems[loopcount + 1].FieldDisplayWidth) <= 3)
                                        {
                                            fieldWidth = tabField.FieldDisplayWidth;
                                        }
                                        else
                                        {
                                            fieldWidth = tabField.FieldDisplayWidth + 2;
                                            totalcount = totalcount + 2;
                                        }
                                    }
                                }
                                else if (totalcount % 3 == 2)
                                {
                                    if (layoutItems.Count > loopcount + 1)
                                    {
                                        if ((tabField.FieldDisplayWidth + layoutItems[loopcount + 1].FieldDisplayWidth) <= 3)
                                        {
                                            fieldWidth = tabField.FieldDisplayWidth;
                                        }
                                        else
                                        {
                                            fieldWidth = tabField.FieldDisplayWidth + 1;
                                            totalcount = totalcount + 1;
                                        }
                                    }
                                }

                                loopcount++;
                                #endregion

                                Table historyTable = new Table();

                                #region Render Form Layout Controls
                                #region Group
                                if (fieldName == "#GroupStart#" || fieldName == "#GroupEnd#")
                                {
                                    if (fieldName == "#GroupStart#")
                                    {
                                        tableType = Enums.TableDisplayTypes.Grouped;
                                        groupedTabTable = new Table();
                                        groupedTabTable.Attributes.Add("style", "width:100%");
                                        groupedPanelContainer = new Panel();
                                        isGroup = true;
                                    }
                                    else if (groupedPanelContainer != null && fieldName == "#GroupEnd#")
                                    {
                                        if (fieldDisplayName != null)
                                        {
                                            groupedPanelContainer.GroupingText = Convert.ToString(fieldDisplayName);
                                        }
                                        else
                                        {
                                            groupedPanelContainer.GroupingText = "-";
                                        }
                                        groupedPanelContainer.Controls.Add(groupedTabTable);
                                        row = new TableRow();

                                        addToTableInSameCell(row, groupedPanelContainer, "", "", 3, tabTable);
                                        tableType = Enums.TableDisplayTypes.General;
                                        isGroup = false;
                                        groupedPanelContainer = null;
                                        columnCount = totalColumnsInDisplay;
                                    }
                                    row = new TableRow();
                                }
                                #endregion
                                #region Table
                                else if (fieldName == "#TableStart#" || fieldName == "#TableEnd#")
                                {
                                    if (fieldName == "#TableStart#")
                                    {
                                        if (row.Cells.Count > 0)
                                        {
                                            tableViewRowCount = 0;
                                            tabTable.Rows.Add(row);
                                            row = new TableRow();
                                        }
                                        tableType = Enums.TableDisplayTypes.TableLayout;
                                        tableViewTable = new Table();
                                        tableViewTable.BorderWidth = 1;
                                        tableViewTable.Style.Add("border-collapse", "collapse");
                                        tableViewTable.Attributes.Add("rules", "all");
                                        tableViewRowCount = 0;
                                        tableCellsLength = fieldWidth;
                                        tableViewTable.Attributes.Add("style", "width:100%");

                                        string[] tableHeaders = fieldDisplayName.Split('#');
                                        legend = tableHeaders[0];
                                        foreach (string header in tableHeaders)
                                        {
                                            TableCell cell = new TableCell();
                                            string[] headerSplit = header.Split(';');
                                            cell.Text = headerSplit[0];
                                            cell.Style.Add("font-weight", "bold");
                                            cell.BorderColor = Color.Black;
                                            cell.BorderStyle = BorderStyle.NotSet;
                                            if (headerSplit.Length > 1)
                                                cell.Style.Add("width", headerSplit[1]);
                                            cell.CssClass = "ms-selectednav ";
                                            cell.HorizontalAlign = HorizontalAlign.Left;
                                            row.Cells.Add(cell);
                                        }
                                        row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                                        if (row.Cells.Count > 1)
                                            row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                        tableViewTable.Rows.Add(row);
                                        row = new TableRow();
                                    }
                                    else if (fieldName == "#TableEnd#")
                                    {
                                        if (row.Cells.Count > 0)
                                        {
                                            tableViewRowCount = 0;
                                            tableViewTable.Rows.Add(row);
                                            row = new TableRow();
                                        }
                                        foreach (TableRow tableRow in tableViewTable.Rows)
                                        {
                                            tableRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                                            if (tableRow.Cells.Count > 1)
                                                tableRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        Panel groupedPanel = new Panel();
                                        groupedPanel.GroupingText = fieldDisplayName;
                                        groupedPanel.Controls.Add(tableViewTable);
                                        columnCount += fieldWidth;
                                        if (columnCount > totalColumnsInDisplay)
                                        {
                                            for (int j = columnCount - fieldWidth; j < totalColumnsInDisplay; j++)
                                            {
                                                addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, tabTable);
                                            }
                                            columnCount = fieldWidth;
                                            row = new TableRow();
                                        }
                                        addToTableInSameCell(row, groupedPanel, "", "", fieldWidth, tabTable);
                                        tableType = Enums.TableDisplayTypes.General;
                                    }
                                    row = new TableRow();
                                    row.BorderStyle = BorderStyle.None;
                                    row.BorderWidth = 1;
                                }
                                #endregion
                                #region Control
                                else if (fieldName == "#Control#")
                                {
                                    ModuleRoleWriteAccess roleWriteAccess = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStep || x.StageStep == 0) && x.FieldName.ToLower() == fieldDisplayName.ToLower());
                                    long rowAccessID = 0;
                                    if (roleWriteAccess != null)
                                        rowAccessID = roleWriteAccess.ID;

                                    //Deleget baseline parameters to all controls
                                    string baselineParams = string.Empty;
                                    if (Request["showBaseline"] != null && UGITUtility.StringToBoolean(Request["showBaseline"].Trim()) && Request["baselineNum"] != null)
                                    {
                                        baselineParams = string.Format("&showBaseline={0}&baselineNum={1}", Request["showBaseline"], Request["baselineNum"].Trim());
                                    }

                                    Control control = null;
                                    if (fieldDisplayName.Equals("ProjectPlanTimeSheet", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        //ProjectPlanTimeSheet ctr = (ProjectPlanTimeSheet)Page.LoadControl("/_controltemplates/15/uGovernIT/ProjectPlanTimeSheet.ascx");
                                        //ctr.nprId = currentTicketId;
                                        //control = ctr;
                                    }
                                    else if (fieldDisplayName.Equals("NPRBudget", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ticketID={0}&module={2}&frameObjId={1}&control=modulebudget{3}", Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("PMMTasksList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        bool showTreeview = false;//tabField.Prop_TreeView.HasValue ? tabField.Prop_TreeView.Value : false;

                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?pmmid={0}&ticketid={4}&showTreeview={1}&frameObjId={2}&control=PMMTasksList{3}",
                                                                            currentTicketId, showTreeview, newFrameId, baselineParams, currentTicketPublicID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }

                                    else if (fieldDisplayName.Equals("ProjectStatusDetail", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?pmmid={0}&frameObjId={1}&control=ProjectStatusDetail{2}",
                                                                                            currentTicketId, newFrameId, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ProjectBudgetDetail", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.asp?ticketID={0}&module={2}&frameObjId={1}&control=modulebudget{3}&IsTabActive=true",
                                                                                            Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ProjectVarianceReport", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?pmmid={0}&frameObjId={1}&control=ProjectVarianceReport{2}",
                                                                                            currentTicketId, newFrameId, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ITGBudgetManagement", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?frameObjId={1}&isreadonly={0}&control=ITGBudgetManagement{2}",
                                                                                            !authorizedToEditTab, newFrameId, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("GovernanceReview", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?&frameObjId={0}&isreadonly={1}&control=GovernanceReview{2}",
                                                                                            newFrameId, !authorizedToEditTab, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ITGCommitteeReview", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?frameObjId={1}&isreadonly={0}&control=ITGCommitteeReview{2}",
                                                                                            !authorizedToEditTab, newFrameId, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ITGovernanceReview", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?&frameObjId={0}&isreadonly={1}&control=ITGovernanceReview{2}",
                                                                                            newFrameId, !authorizedToEditTab, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ITGPortfolio", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?frameObjId={0}&control=ITGPortfolio&isreadonly={1}{2}",
                                                                                            newFrameId, !authorizedToEditTab, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("NewProjectTask", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ticketID={0}&frameObjId={1}&isSelectdTab=true&control=NewProjectTask{3}",
                                                                                            Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentModuleName, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='200' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("History", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        bool isInFrame = true;//tabField..HasValue ? tabField.Prop_IsInFrame.Value : false;

                                        if (isInFrame)
                                        {
                                            Guid newFrameId = Guid.NewGuid();
                                            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?ticketId={0}&frameObjId={1}&control=History&listName={2}{3}",
                                                                                                Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), newFrameId, currentTicket[DatabaseObjects.Columns.Title], baselineParams));
                                            LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                            control = lCtr;
                                        }
                                        else
                                        {
                                            History history = (History)Page.LoadControl("/Controltemplates/Shared/History.ascx");
                                            history.ReadOnly = true;
                                            history.TicketId = currentTicketPublicID;
                                            //history.ListName = currentTicket.ParentList.Title;
                                            control = history;
                                        }
                                    }
                                    else if (fieldDisplayName.Equals("ResourcePlan", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=ResourcePlan&isreadonly={2}&ticketId={0}",
                                                                                        currentTicketPublicID, newFrameId, !authorizedToEditTab));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("Bid", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=Bid&isreadonly={2}&ticketId={0}",
                                                                                        currentTicketPublicID, newFrameId, !authorizedToEditTab));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("Custom Relationship", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=CustomTicketRelationship&isreadonly={2}&ticketId={0}{3}",
                                                                                        currentTicket[DatabaseObjects.Columns.TicketId].ToString(), newFrameId, !authorizedToEditTab, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("TicketRelationshipTree", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=TicketRelationshipTree&isreadonly={2}&ticketId={0}{3}&module={4}&Id={5}",
                                                                                        UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketId).ToString(), newFrameId, !authorizedToEditTab, baselineParams, currentModuleName, currentTicket[DatabaseObjects.Columns.Id].ToString()));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("approvaltab", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={0}&control=approvaltab&moduleID={1}&ticketID={2}&CurrentWorkflowStep={3}{4}",
                                                                                        newFrameId, moduleId, UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketId).ToString(), currentStep, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("TasksList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ticketID={0}&module={2}&frameObjId={1}&control=TasksList&rwID={3}{4}",
                                                                                            currentTicketId, newFrameId, currentModuleName, rowAccessID, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ITGBudgetEditor", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?frameObjId={1}&isreadonly={0}&control=ITGBudgetEditor{2}",
                                                                                            !authorizedToEditTab, newFrameId, baselineParams));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ModuleInstRelations", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ticketID={0}&frameObjId={2}&isreadonly={1}&control=ModuleInstRelations&ModuleName={3}",
                                                                                            currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("projectdocuments", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ticketID={0}&frameObjId={2}&isreadonly={1}&control=projectdocuments&ModuleName={3}",
                                                                                            currentTicketPublicID, !authorizedToEditTab, newFrameId, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("application modules", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ID={0}&frameObjId={2}&isreadonly={1}&control=applicationmodules&ModuleName={3}",
                                                                                            currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("applicationrole", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ID={0}&frameObjId={2}&isreadonly={1}&control=applicationrole&ModuleName={3}",
                                                                                            currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ModuleRoleMapControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ID={0}&frameObjId={2}&isreadonly={1}&control=modulerolemapcontrol&ModuleName={3}&ticketID={4}",
                                                                                            currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName, currentTicketPublicID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ServersControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ID={0}&frameObjId={2}&isreadonly={1}&control=applicationserver&ModuleName={3}",
                                                                                            currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("PasswordControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ID={0}&frameObjId={2}&isreadonly={1}&control=applicationpassword&ModuleName={3}&ticketID={4}",
                                                                                            currentTicketId, !authorizedToEditTab, newFrameId, currentModuleName, currentTicketPublicID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("preconditionlist", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        // LookupValue lookup = new LookupValue(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]));
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?publicID={0}&frameObjId={2}&isreadonly={1}&control=preconditionlist&ModuleName={3}&stageID={4}", currentTicketPublicID, !authorizedToEditTab, newFrameId, currentModuleName, Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ModuleStepLookup])));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' style='width:100%;min-width:850px;' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("WikiRelatedTickets", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=WikiRelatedTickets&isreadonly={2}&ticketId={0}{3}&moduleId={4}",
                                                                                        currentTicket[DatabaseObjects.Columns.TicketId].ToString(), newFrameId, !authorizedToEditTab, baselineParams, moduleId));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("ProjectResourceDetail", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?Module={4}&projectPublicID={0}&frameObjId={1}&control=ProjectResourceDetail&isreadonly={2}{3}",
                                                                                            currentTicketPublicID, newFrameId, !authorizedToEditTab, baselineParams, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='250' height='250' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }

                                    //new line added for case..

                                    else if (fieldDisplayName.Equals("PMMEventsCalendar", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?Module={1}&projectPublicID={0}&control=PMMEventsCalendar&frameObjId={2}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' addheight='250' height='550'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    //else if (fieldDisplayName.Equals("DocumentControl", StringComparison.CurrentCultureIgnoreCase))
                                    //{
                                    //    //Guid newFrameId = Guid.NewGuid();
                                    //    //string url =  UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&&frameObjId={1}&control=DocumentControl&isreadonly={3}&moduleName={2}&PublicTicketID={4}&docName={0}{5}",
                                    //    //                                                  DocumentHelper.GetDocumentLibraryName(currentTicket), newFrameId, currentModuleName, !authorizedToEditTab, currentTicketPublicID, baselineParams));
                                    //    //if (!string.IsNullOrEmpty(Request["FolderId"]))
                                    //    //{
                                    //    //    url = string.Format("{0}&FolderId={1}", url, Request["FolderId"]);
                                    //    //}

                                    //    //LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                    //    //control = lCtr;
                                    //}
                                    else if (fieldDisplayName.Equals("DocumentControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=DocumentControl&ticketId={0}&module={2}",
                                                                                        currentTicketPublicID, newFrameId, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='220' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("PMMDocumentGrid", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=PMMDocumentGrid&ModuleName={4}&isreadonly={2}&docName={0}{3}",
                                                                                        Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]).Replace("-", "_"), newFrameId, !authorizedToEditTab, baselineParams, currentModuleName));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("PMMSprints", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?Module={1}&PublicTicketID={0}&control=PMMSprints&frameObjId={2}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("InvestmentsControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=investmentcontrol&ModuleName={0}&InvestorID={2}",
                                                                                         currentModuleName, newFrameId, Convert.ToString(currentTicket[DatabaseObjects.Columns.Id])));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("DistributionControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?&frameObjId={1}&control=distributioncontrol&ModuleName={0}&InvestorID={2}",
                                                                                        currentModuleName, newFrameId, Convert.ToString(currentTicket[DatabaseObjects.Columns.Id])));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='300' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorSOWControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorSOWControl&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorFMControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorFMControl&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorPMControl", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorPMControl&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorSOWFeeList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorSOWFeeList&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorReportList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorReportList&rwID={3}&ShowSelectedInstance={4}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID, Request["ShowSelectedInstance"]));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorMeetingList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorMeetingList&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorServiceDurationList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorServiceDurationList&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("vendorsladashboard", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=vendorsladashboard",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("vendorissuelist", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=vendorissuelist&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("scheduleactions", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=scheduleactions&IsModuleWebpart=true&IsArchive=false",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("scheduleactionsarchive", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=scheduleactions&IsModuleWebpart=true&IsArchive=true",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorRisksList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorRisksList&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("PMMRisksList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&Module={1}&PublicTicketID={0}&control=PMMRisksList&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorApprovedSubContractorsList", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorApprovedSubContractorsList&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;

                                    }
                                    else if (fieldDisplayName.Equals("vendorkeypersonnellist", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=vendorkeypersonnellist&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    else if (fieldDisplayName.Equals("VendorSOWContImprovementlist", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Guid newFrameId = Guid.NewGuid();
                                        string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?frameObjId={2}&isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=VendorSOWContImprovementlist&rwID={3}",
                                                                                            currentTicketPublicID, currentModuleName, newFrameId, rowAccessID));
                                        LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        // LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%'  height='380'  frameborder='0' id='{1}'></iframe>", url, newFrameId));
                                        control = lCtr;
                                    }
                                    if (control != null)
                                    {
                                        if (isGroup && groupedPanelContainer != null)
                                        {
                                            //addToTableInSameCell(row, control, string.Empty, string.Empty, 3, tabTable);
                                            //groupedPanelContainer.Controls.Add(control);
                                            row = new TableRow();
                                            addToTableInSameCell(row, control, string.Empty, string.Empty, 3, groupedTabTable);
                                            row = new TableRow();
                                        }
                                        else
                                        {
                                            Panel groupedPanel;
                                            groupedPanel = new Panel();
                                            groupedPanel.Controls.Add(control);
                                            addToTableInSameCell(row, groupedPanel, string.Empty, string.Empty, 3, tabTable);
                                            row = new TableRow();
                                        }
                                        columnCount = totalColumnsInDisplay;
                                    }
                                }
                                #endregion
                                #region Label
                                else if (fieldName == "#Label#")
                                {
                                    Label label = new Label();
                                    label.Text = fieldDisplayName;

                                    if (tableType == Enums.TableDisplayTypes.Grouped)
                                        addToTable(row, label, fieldDisplayName, 1, groupedTabTable);
                                    else if (tableType == Enums.TableDisplayTypes.General)
                                        addToTable(row, label, fieldDisplayName, 1);
                                    else if (tableType == Enums.TableDisplayTypes.TableLayout)
                                    {
                                        if (tableViewRowCount >= tableCellsLength)
                                        {
                                            tableViewRowCount = 0;
                                            tableViewTable.Rows.Add(row);
                                            row = new TableRow();
                                        }
                                        addToTableInSameCellTableLayout(row, label, 1, tableViewTable, !(label.Text.Contains("Notes")));

                                    }

                                }
                                #endregion
                                #region Empty
                                else if (fieldName == "#PlaceHolder#")
                                {
                                    Label label = new Label();
                                    if (tableType != Enums.TableDisplayTypes.TableLayout)
                                    {
                                        columnCount += fieldWidth;
                                        if (columnCount > totalColumnsInDisplay)
                                        {
                                            columnCount = fieldWidth;
                                            row = new TableRow();

                                        }
                                    }

                                    if (tableType == Enums.TableDisplayTypes.Grouped)
                                        addToTicketTable(row, label, string.Empty, fieldWidth, groupedTabTable);
                                    else if (tableType == Enums.TableDisplayTypes.General)
                                        addToTable(row, label, string.Empty, fieldWidth);
                                    else if (tableType == Enums.TableDisplayTypes.TableLayout)
                                    {
                                        if (tableViewRowCount >= tableCellsLength)
                                        {
                                            tableViewRowCount = 0;
                                            tableViewTable.Rows.Add(row);
                                            row = new TableRow();
                                        }
                                        addToTableInSameCellTableLayout(row, label, fieldWidth, tableViewTable);
                                    }
                                }
                                #endregion
                                #region Rest of Controls
                                else
                                {
                                    if (dtItems.Columns.Contains(fieldName))
                                    {
                                        DataTable dtList = thisList.CopyToDataTable();
                                        FieldDesignMode fieldDesignMode = FieldDesignMode.Normal;
                                        if (dtList.Columns.Contains(fieldName))
                                        {
                                            DataColumn f = dtList.Columns[fieldName];
                                            RequiredFieldValidator rfv = null;
                                            //by Manish Hada, if current user is not authenticated then not a single field should be in edit mode.

                                            ControlMode ctrMode = ControlMode.Display;
                                            if (isActionUser)
                                            {
                                                ctrMode = ControlMode.Edit;
                                            }

                                            Control bfc;
                                            //if (distinctValues.Rows.Count > 1)

                                            bfc = ticketControls.GetControls(f, ctrMode, fieldDesignMode, "", tabField);
                                            //else
                                            //bfc = ticketControls.GetControls(f, currentTicket, ctrMode, true, false, "", tabField, newTicket, false);

                                            if (bfc != null)
                                            {
                                                #region Versioned Fields TicketResolutionComments
                                                DataTable ticketHours = null;
                                                if (currentTicket != null && Convert.ToInt32(currentTicket[DatabaseObjects.Columns.ID]) != 0 && fieldName == DatabaseObjects.Columns.ResolutionComments)
                                                {
                                                    ticketHours = GetActualHoursList();
                                                    List<HistoryEntry> historyList = uHelper.GetHistory(currentTicket, DatabaseObjects.Columns.TicketResolutionComments, false);
                                                    if (historyList.Count > 0 && (!TicketRequest.Module.ActualHoursByUser))
                                                    {
                                                        // new line of code for edit buton....
                                                        string strUrl = Request.Url.AbsolutePath + "?TicketId=" + currentTicketPublicID;
                                                        string strEditbtn = string.Empty;
                                                        if (IsAdmin && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowCommentDelete))
                                                            strEditbtn = "<div style='float:right;'> <img src='/Content/images/editNewIcon.png' onclick=getTicketResolutionComment('" + currentTicket["TicketId"] + "','" + currentTicket.Table.TableName + "','" + Server.UrlEncode(strUrl) + "') style='cursor:pointer'/></div>";

                                                        foreach (HistoryEntry historyEntry in historyList)
                                                        {
                                                            TableRow historyRow = new TableRow();
                                                            string txt = historyEntry.entry.Contains("\n") ? "<br/>" + historyEntry.entry : historyEntry.entry;
                                                            txt = uHelper.FindAndConvertToAnchorTag(txt);
                                                            addToTableInSameCell(historyRow, strEditbtn + "<span style='white-space:pre-wrap'><b>[" + historyEntry.created + "] " + historyEntry.createdBy + ":</b> " + txt + "</span>", string.Empty, 3, historyTable);
                                                            strEditbtn = string.Empty;
                                                        }
                                                    }
                                                    row = new TableRow();
                                                    //if (currentTicket != null && !string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.ID])) && f.ColumnName == DatabaseObjects.Columns.TicketResolutionComments)
                                                    //{
                                                    //    List<HistoryEntry> historyList = uHelper.GetHistory(currentTicket, DatabaseObjects.Columns.TicketResolutionComments, false);
                                                    //    if (historyList.Count > 0)
                                                    //    {

                                                    //        // new line of code for edit buton....
                                                    //        string strUrl = Request.Url.AbsolutePath + "?TicketId=" + currentTicketPublicID;
                                                    //        string strEditbtn = string.Empty;
                                                    //        if (IsAdmin && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowCommentDelete))
                                                    //            strEditbtn = "<div style='float:right;'> <img src='/Content/images/edit.gif' onclick=getTicketResolutionComment('" + currentTicket[DatabaseObjects.Columns.ID] + "','','" + Server.UrlEncode(strUrl) + "') style='cursor:pointer'/> </div>";

                                                    //        foreach (HistoryEntry historyEntry in historyList)
                                                    //        {
                                                    //            TableRow historyRow = new TableRow();
                                                    //            string txt = historyEntry.entry.Contains("\n") ? "</br>" + historyEntry.entry : historyEntry.entry;
                                                    //            txt = uHelper.FindAndConvertToAnchorTag(txt);
                                                    //            if (!TicketRequest.Module.ActualHoursByUser)
                                                    //                addToTableInSameCell(historyRow, strEditbtn + "<span style='white-space:pre-wrap'><b>[" + historyEntry.created + "] " + historyEntry.createdBy + ":</b> " + txt + "</span>", string.Empty, 3, historyTable);
                                                    //            strEditbtn = string.Empty;
                                                    //        }
                                                    //    }
                                                    //    row = new TableRow();
                                                    //    row.BorderStyle = BorderStyle.Solid;
                                                    //    row.BorderWidth = 1;
                                                }
                                                #endregion
                                                if (IsAdmin)
                                                {
                                                    adminOverride = true;
                                                }
                                                else
                                                {
                                                    adminOverride = false;
                                                }
                                                ModuleRoleWriteAccess roleWriteAccess = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => x.FieldName == f.ColumnName);
                                                ModuleRoleWriteAccess roleWriteAccessTemp = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => x.FieldName == f.ColumnName);
                                                //check the value of editbutton and showwithcheckbox value.
                                                bool editButton = false, showWithCheckBox = false;
                                                if (roleWriteAccess != null)
                                                {
                                                    if (roleWriteAccess.ShowEditButton && roleWriteAccess.ShowWithCheckbox)
                                                        fieldDesignMode = FieldDesignMode.WithEditAndCheckbox;
                                                    else if (roleWriteAccess.ShowEditButton)
                                                        fieldDesignMode = FieldDesignMode.WithEdit;
                                                    else if (roleWriteAccess.ShowWithCheckbox)
                                                        fieldDesignMode = FieldDesignMode.WithCheckbox;
                                                    editButton = roleWriteAccess.ShowEditButton;
                                                    showWithCheckBox = roleWriteAccess.ShowWithCheckbox;
                                                }
                                                try
                                                {
                                                    if (roleWriteAccess != null || (adminOverride && roleWriteAccessTemp != null))
                                                    {
                                                        string fieldLevelUserType = string.Empty;
                                                        //Check for FieldLevel authorization
                                                        if (roleWriteAccess != null)
                                                            if (!string.IsNullOrWhiteSpace(roleWriteAccess.ActionUser))
                                                                fieldLevelUserType = string.Join(uGovernIT.Utility.Constants.Separator, roleWriteAccess.ActionUser.ToArray());

                                                        bool fieldLevelAuthority = false;
                                                        if (fieldLevelUserType != string.Empty)
                                                        {
                                                            if (UserManager.IsUserPresentInField(user, currentTicket, fieldLevelUserType, true))
                                                            {
                                                                fieldLevelAuthority = true;
                                                                isFieldLevelAccessIsPersent = true;
                                                            }
                                                        }

                                                        if (isActionUser || fieldLevelAuthority || f.ColumnName == DatabaseObjects.Columns.TicketComment || adminOverride || Request["BatchEditing"] == "true")
                                                        {
                                                            ControlMode mode = ControlMode.Edit;

                                                            // If in Admin Override mode, show in full edit mode without edit button
                                                            //else will show in mode that is defined in role write access
                                                            if (adminOverride)
                                                            {
                                                                editButton = false;
                                                                showWithCheckBox = false;
                                                            }

                                                            //// If printing keep in display mode
                                                            //// Also cannot change TicketInitiatorResolved once set since it controls the workflow
                                                            //if (printEnable ||
                                                            //    (f.InternalName == DatabaseObjects.Columns.TicketInitiatorResolved && (currentModuleName == "PRS" || currentModuleName == "TSR") && currentStep == 1))
                                                            //{
                                                            //    mode = SPControlMode.Display;
                                                            //    editButton = false;
                                                            //}

                                                            //// If in override mode, prevent existing comments shown in edit textbox else keeps appending existing data
                                                            //if (adminOverride && f.InternalName == DatabaseObjects.Columns.TicketResolutionComments)
                                                            //{
                                                            //    mode = SPControlMode.New;
                                                            //}



                                                            bfc = ticketControls.GetControls(f, mode, fieldDesignMode, "", tabField);

                                                        }
                                                        if (roleWriteAccess != null && roleWriteAccess.FieldMandatory)
                                                        {
                                                            fieldDisplayName += "<b style=\"color:red\">* </b>";
                                                            rfv = new RequiredFieldValidator();
                                                            rfv.ControlToValidate = bfc.ID;
                                                        }
                                                        if (fieldName.Contains("Analytics"))
                                                        {
                                                            fieldDisplayName += "<img src='/Content/images/analytics.png' onclick=getScore('" + tabRepeater.ClientID + "_ctl00_" + bfc.ClientID + "') style='cursor:pointer' />";
                                                        }
                                                        else if (fieldName.Contains("Date"))
                                                        {

                                                        }
                                                    }
                                                    else
                                                    {
                                                        //if (fieldName.Contains("Analytics") && (((NumberField)bfc).Value == null || Convert.ToString(((NumberField)bfc).Value) != string.Empty))
                                                        //{
                                                        row = new TableRow();
                                                        // continue;
                                                        // }

                                                        bfc = ticketControls.GetControls(f, ControlMode.Edit, fieldDesignMode, "", tabField);

                                                        //Remove comment add label if it is not in edit more. Show comment trail only.
                                                        if (fieldName == DatabaseObjects.Columns.TicketComment)
                                                        {
                                                            bfc = null;
                                                        }
                                                    }



                                                    if (f.ColumnName == DatabaseObjects.Columns.TicketOwner)
                                                    {
                                                        incidentOwnerId = requestOwnerIdDisplayed = tabRepeater.ClientID + "_ctl00_" + bfc.ClientID;
                                                    }
                                                    else if (f.ColumnName == DatabaseObjects.Columns.TicketPriority)
                                                    {
                                                        priorityIdDisplayed = tabRepeater.ClientID + "_ctl00_" + bfc.ClientID;
                                                    }
                                                    else if (f.ColumnName == DatabaseObjects.Columns.TicketRequestTypeLookup)
                                                    {

                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    //Log.WriteException(ex, "ModuleWebPartUserControl - ModuleTabItemCreated()");
                                                    Util.Log.ULog.WriteException(ex);
                                                }

                                                if (tableType != Enums.TableDisplayTypes.TableLayout)
                                                {
                                                    columnCount += fieldWidth;
                                                    if (columnCount > totalColumnsInDisplay && (f.ColumnName != "ResolutionComments"))
                                                    {
                                                        for (int j = columnCount - fieldWidth; j < totalColumnsInDisplay; j++)
                                                        {
                                                            if (tableType == Enums.TableDisplayTypes.Grouped)
                                                                addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                                                            else if (tableType == Enums.TableDisplayTypes.General)
                                                                addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1);
                                                        }
                                                        columnCount = fieldWidth;
                                                        row = new TableRow();
                                                        row.BorderStyle = BorderStyle.Solid;
                                                        row.BorderWidth = 1;
                                                    }
                                                }

                                                if (f.ColumnName == DatabaseObjects.Columns.TicketBusinessManager
                                                    || f.ColumnName == DatabaseObjects.Columns.TicketGLCode
                                                    || f.ColumnName == DatabaseObjects.Columns.TicketTester)
                                                {
                                                    row.CssClass += " " + f.ColumnName + "Row";
                                                }
                                                if (f.ColumnName == DatabaseObjects.Columns.Attachments)
                                                {
                                                    TableRow tr1 = new TableRow();
                                                    TableCell td1 = new TableCell();

                                                    Table table = new Table();
                                                    TableRow tr = new TableRow();
                                                    TableCell td = new TableCell();
                                                    LinkButton deleteFile = new LinkButton();


                                                    table.Attributes.Add("class", "fileuploaderContainer");
                                                    List<string> fileNames = new List<string>();
                                                    int i = 0;

                                                    foreach (string fileName in ("" + currentTicket[DatabaseObjects.Columns.Attachments]).Split(',').ToArray())
                                                    {
                                                        tr = new TableRow();
                                                        tr.Attributes.Add("id", "fileContainer" + i);

                                                        td = new TableCell();

                                                        HyperLink uploadedFile = new HyperLink();
                                                        uploadedFile.ID = "file" + i.ToString();
                                                        uploadedFile.CssClass = "file" + i.ToString() + " inputFile";
                                                        uploadedFile.Text = fileName;
                                                        uploadedFile.NavigateUrl = UGITUtility.GetAbsoluteURL(currentTicket[DatabaseObjects.Columns.Attachments] + fileName);
                                                        td.Controls.Add(uploadedFile);
                                                        tr.Cells.Add(td);

                                                        // Only Action users Or Admin can delete attachments
                                                        if (isActionUser || adminOverride)
                                                        {
                                                            td = new TableCell();
                                                            deleteFile = new LinkButton();
                                                            deleteFile.Text = "Delete";
                                                            deleteFile.CssClass = "deleteFile" + i.ToString();
                                                            deleteFile.OnClientClick = "DeleteFile(this," + i + ");";
                                                            deleteFile.Click += new EventHandler(deleteFileButton_Click);
                                                            td.Controls.Add(deleteFile);
                                                            tr.Cells.Add(td);
                                                        }
                                                        table.Rows.Add(tr);

                                                        i++;
                                                    }

                                                    // COMMENTED OUT: Allow anyone to ADD attachments
                                                    //if (isActionUser)
                                                    {
                                                        attachment = new FileUpload();
                                                        attachment.CssClass = "ms-fileinput inputFile";
                                                        uploadButton = new Button();
                                                        uploadButton.Click += new EventHandler(uploadBUtton_Click);
                                                        uploadButton.Text = "Attach";
                                                        // This is now handled server-side
                                                        //uploadButton.OnClientClick = "return ValidateFileName();";

                                                        tr = new TableRow();
                                                        td = new TableCell();
                                                        td.Controls.Add(attachment);
                                                        tr.Attributes.Add("class", "uploadfileinputcontainer");
                                                        tr.Cells.Add(td);


                                                        td = new TableCell();
                                                        td.Controls.Add(uploadButton);
                                                        tr.Cells.Add(td);


                                                        table.Rows.Add(tr);
                                                        table.CssClass = "idAttachmentsTable";
                                                    }

                                                    TableRow bfcRow = new TableRow();
                                                    TableCell bfcCell = new TableCell();
                                                    bfcRow.Cells.Add(bfcCell);
                                                    bfcRow.Attributes.Add("class", "defaultattachmentcontainer");
                                                    bfcCell.Controls.Add(bfc);
                                                    table.Rows.Add(bfcRow);
                                                    bfc = table;

                                                }
                                                if (f.ColumnName.Contains("HoldDuration"))
                                                {
                                                    Literal ltrlHoldTime = new Literal();
                                                    ltrlHoldTime.Text = uHelper.GetFormattedHoldTime(currentTicket, context);//GetHoldTime(currentTicket).ToString();
                                                    bfc = (Control)ltrlHoldTime;
                                                }


                                                if (historyTable.Rows.Count < 1)
                                                {

                                                    if (tableType == Enums.TableDisplayTypes.Grouped)
                                                        //addToTable(row, bfc, fieldDisplayName, fieldWidth, groupedTabTable);
                                                        addToTicketTable(row, bfc, fieldDisplayName, fieldWidth, groupedTabTable, null, null, fieldName);
                                                    else if (tableType == Enums.TableDisplayTypes.General)
                                                        addToTable(row, bfc, fieldDisplayName, fieldWidth);
                                                    else if (tableType == Enums.TableDisplayTypes.TableLayout)
                                                    {
                                                        if (tableViewRowCount >= tableCellsLength)
                                                        {
                                                            tableViewRowCount = 0;
                                                            tableViewTable.Rows.Add(row);
                                                            row = new TableRow();
                                                        }
                                                        addToTableInSameCellTableLayout(row, bfc, fieldWidth, tableViewTable);
                                                    }
                                                }
                                                else
                                                {
                                                    if (tableType == Enums.TableDisplayTypes.Grouped)
                                                        addToTable(row, bfc, fieldDisplayName, fieldWidth, groupedTabTable, (Control)historyTable);
                                                    else if (tableType == Enums.TableDisplayTypes.General)
                                                        addToTable(row, bfc, fieldDisplayName, fieldWidth, (Control)historyTable);

                                                }

                                                #region Versioned Fields Comments
                                                if (f.ColumnName == DatabaseObjects.Columns.TicketComment)
                                                {
                                                    row = new TableRow();
                                                    row.BorderStyle = BorderStyle.Solid;
                                                    row.BorderWidth = 1;

                                                    bool oldestFirst = !ConfigurationVariableManager.GetValueAsBool(ConfigConstants.CommentsNewestFirst);
                                                    List<HistoryEntry> historyList = uHelper.GetHistory(currentTicket, DatabaseObjects.Columns.TicketComment, oldestFirst);
                                                    if (historyList.Count > 0)
                                                    {

                                                        // new line of code for edit buton....
                                                        string strUrl = Request.Url.AbsolutePath + "?TicketId=" + currentTicketPublicID;
                                                        string strEditBtn = string.Empty;
                                                        if (IsAdmin && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AllowCommentDelete))
                                                            strEditBtn = "<div style='float:right;'> <img src='/Content/images/edit.gif' onclick=getTicketComment('" + currentTicket[DatabaseObjects.Columns.TicketId] + "','','" + Server.UrlEncode(strUrl) + "') style='cursor:pointer'/> </div>";

                                                        foreach (HistoryEntry historyEntry in historyList)
                                                        {
                                                            var userProfile = UserManager.GetUserInfoById(historyEntry.createdBy); //UserManager.GetUserByUserName(x.createdBy);
                                                            if (userProfile != null)
                                                            {
                                                                historyEntry.createdBy = userProfile.Name;
                                                            }

                                                            if (tableType == Enums.TableDisplayTypes.Grouped)
                                                            {
                                                                string txt = historyEntry.entry.Contains("\n") ? "</br>" + historyEntry.entry : historyEntry.entry;
                                                                addToTable(row, strEditBtn + "<span style='white-space:pre-wrap'><b>[" + historyEntry.created + "]</b> " + txt + "</span>", historyEntry.createdBy, "", 3, groupedTabTable);
                                                                strEditBtn = string.Empty;
                                                            }
                                                            else if (tableType == Enums.TableDisplayTypes.General)
                                                            {
                                                                string txt = historyEntry.entry.Contains("\n") ? "</br>" + historyEntry.entry : historyEntry.entry;
                                                                addToTable(row, "<span style='white-space:pre-wrap'><b>[" + historyEntry.created + "]</b> " + txt + "</span>", historyEntry.createdBy, "", 3, historyTable);
                                                            }

                                                            row = new TableRow();
                                                            row.BorderStyle = BorderStyle.Solid;
                                                            row.BorderWidth = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row = new TableRow();
                                                        row.BorderStyle = BorderStyle.Solid;
                                                        row.BorderWidth = 1;
                                                        addToTable(row, "No comments", "", "", 3, groupedTabTable);
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }
                            if (columnCount < totalColumnsInDisplay)
                            {
                                for (int j = columnCount; j < totalColumnsInDisplay; j++)
                                {
                                    if (tableType == Enums.TableDisplayTypes.Grouped)
                                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                                    else if (tableType == Enums.TableDisplayTypes.General)
                                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1);
                                }
                            }
                        }
                        else
                        {
                            Label unAuthorized = new Label();
                            unAuthorized.Text = "Access Denied.";

                            TableCell newCell = new TableCell();
                            newCell.ColumnSpan = 4;
                            newCell.Controls.Add(unAuthorized);
                            row.Cells.Add(newCell);
                            tabTable.Rows.Add(row);
                            columnCount = totalColumnsInDisplay;
                        }
                        ((Panel)e.Item.FindControl("tabContainer")).Controls.Add(tabTable);
                    }
                }
            }
            #endregion
            #endregion
        }
        //Added by mudassir 10 march 2020
        private bool IsTicketEmailExist(int itemId)
        {
            bool itemExists = false;

            if (itemId == 0)
                return itemExists;

            EmailsManager ObjEmailsManager = new EmailsManager(context);
            Email ticketEmail = new Email();
            ticketEmail = ObjEmailsManager.LoadByID(itemId);


            if (ticketEmail != null && Convert.ToString(ticketEmail.EmailStatus).ToLower() == "delivered")
                itemExists = true;

            return itemExists;
        }
        //End
        protected int GetHoldTime(DataRow ticket)
        {
            int holdTime = 0;
            if (UGITUtility.IsSPItemExist(ticket, DatabaseObjects.Columns.TicketOnHoldStartDate))
            {
                if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketTotalHoldDuration, ticket.Table))
                    holdTime = UGITUtility.StringToInt(ticket[DatabaseObjects.Columns.TicketTotalHoldDuration]);

                TimeSpan timeSpan = (DateTime.Now).Subtract((DateTime)UGITUtility.GetSPItemValue(ticket, DatabaseObjects.Columns.TicketOnHoldStartDate));
                holdTime += (int)timeSpan.TotalMinutes;
            }
            return holdTime;
        }

        /// <summary>
        /// This will approve all the tasks which are pending. and move the ticket to next stage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void approveOnTaskComplete_Click(object sender, EventArgs e)
        {
            UGITModuleConstraint.MarkAllTaskComplete(currentTicketPublicID, currentStep, context);
            Button_Click(approveButton, e);
        }

        /// <summary>
        /// Called on all Buttons Clicks in the webpart.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Click(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "_refreshPraent", "1");
            UGITUtility.DeleteCookie(Request, Response, "ticketDivision");
            string senderValue = actionEventID.Value;

            #region startbuttonclick
            string changedStage = string.Empty;
            missingMandatoryFields = string.Empty;
            bool valid = true;
            bool ignoreConstraintValidation = false;
            invalidInput = false;
            errorMsg.Text = string.Empty;
            errorMsgNew.Text = string.Empty;
            invalidErrorMsg.Text = string.Empty;
            string oldPRPValue = string.Empty;

            string currentTicketStatus = string.Empty;
            string senderID = (sender == null ? string.Empty : ((Control)sender).ID);
            if (!string.IsNullOrEmpty(senderValue))
            {
                senderID = senderValue;
            }
            if (Session["isRequestTaskCompleted"] != null)
            {
                if (Session["isRequestTaskCompleted"].ToString() == "true")
                {
                    senderID = "approvebuttonhidden";
                    Session["isRequestTaskCompleted"] = false;
                }
            }
            if (Convert.ToBoolean(hdnIgnoreConstraintValidation.Value))
            {
                ignoreConstraintValidation = true;
            }

            //if (showBaselineButtons)
            //{
            //    senderID= "createBaseline";
            //}
            bool bClose = false;
            bool closePopup = false;
            bool redirectAgain = false;
            string originalSenderID = senderID;
            // set the flags to keep the popup open or to close it
            if (senderID == "updateButtonSC" || senderID == "hdnUpdateButtonSC")
            {
                closePopup = true;
                redirectAgain = false;
                senderID = senderID.Substring(0, senderID.Length - 2);
                bClose = true;
            }
            else if (senderID == "hdnUpdateButton")
            {
                closePopup = false;
                redirectAgain = true;
            }

            string actionPerformed = string.Empty;

            actionUserChanged = false;
            changedFields = string.Empty;
            ////Check if we need to save a new ticket or an existing one.
            List<TicketColumnError> errors = new List<TicketColumnError>();
            List<TicketColumnValue> formValues = new List<TicketColumnValue>();
            resolveInitiator.ShowOnPageLoad = false;
            #region Get the current ticket object
            bool updateChangesInHistory = true;



            if (senderID == "createButton" || senderID == "createCloseButton" || senderID == "saveAsDraftButton" || senderID == "relatedTitleButton")
            {
                updateChangesInHistory = false;
                //add parameter due to batch editing.
                formValues = GetValuesFromControls(true, false);
                bool ignoreMandatory = false;
                if (senderID == "saveAsDraftButton")
                    ignoreMandatory = true;
                DataRow row = TicketSchema.NewRow();
                TicketRequest.Validate(formValues, row, errors, ignoreMandatory, adminOverride, 1);

                ValidateAndSetAttachments(errors, panelNewTicket, newTicket, true);

                //Copy comment from old ticket to new ticket
                if (copyTicket != null)
                {

                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketComment, copyTicket.Table) &&
                        newTicket != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketComment, newTicket.Table))
                    {
                        newTicket[DatabaseObjects.Columns.TicketComment] = copyTicket[DatabaseObjects.Columns.TicketComment];

                    }
                    uHelper.CreateHistory(user, string.Format("Converted from original ticket {0}", copyTicket[DatabaseObjects.Columns.TicketId]), newTicket, false, context);
                }
                if (Request["batchCreate"] != null)
                    errors.Remove(errors.Find(x => x.InternalFieldName == DatabaseObjects.Columns.TicketRequestor));

                if (errors.Count > 0)
                    valid = false;

                saveTicket = newTicket;
                //if (senderID == "createButton")
                //    notificationdontsend = true;
            }
            else
            {
                if (Request["BatchEditing"] == "true")
                {
                    formValues = GetValuesFromControls(false, true);
                }
                else
                {
                    formValues = GetValuesFromControls(false, false);
                    bool ignoreMandatory = true;
                    if (senderID == "approveButton" || senderID == "quickCloseTicket" || senderID == "createBaseline" || senderID == "approvebuttonhidden" || senderID== "lnkStatus")
                        ignoreMandatory = false;
                    TicketRequest.Validate(formValues, currentTicket, errors, ignoreMandatory, adminOverride, 1, ignoreConstraintValidation);
                    if (errors.Count > 0)
                        valid = false;

                    saveTicket = currentTicket;

                    actionUserChanged = IsActionUserChanged(saveTicket, formValues);
                    //if (senderID == "superAdminEditButton")
                    //    notificationdontsend = true;
                }
            }
            #endregion
            //bool closePopup = false;
            //bool redirectAgain = false;

            if (saveTicket != null)
            {
                string oldPriority = Convert.ToString(UGITUtility.GetSPItemValue(saveTicket, DatabaseObjects.Columns.TicketPriorityLookup));
                currentTicketStatus = Convert.ToString(UGITUtility.GetSPItemValue(saveTicket, DatabaseObjects.Columns.TicketStatus));
                #region Approve Button Clicked
                if (valid && (senderID == "approveButton" || senderID == "btnSendNotification" || senderID == "btnCancelNotification" || senderID == "approvebuttonhidden"))
                {
                    if (senderID == "approveButton" || senderID == "approvebuttonhidden")
                        actionPerformed = Convert.ToString(TicketActionType.Approved);

                    if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                        oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;
                    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);
                    bool closeChildren = false;
                    //if (confirmChildTicketsClose && hdnCloseTicketType.Value == "1")
                    //    closeChildren = true;
                    if (hdnCloseTicketType.Value == "1")
                    {
                        closeChildren = true;
                        ApproveTicketRequest(errors, senderID, actionPerformed: actionPerformed);
                    }
                    else if (hdnCloseTicketType.Value == "0")
                        ApproveTicketRequest(errors, senderID, actionPerformed: actionPerformed);
                    else
                        ApproveTicketRequest(errors, senderID, true, actionPerformed: actionPerformed);
                    TicketRequest.CheckRequestType(saveTicket, false);
                    //ApproveTicketRequest(errors, senderID);
                    if (errors.Count > 0)
                    {
                        valid = false;
                        if (errors.Exists(x => x.Message == "closechildtickets"))
                        {
                            confirmCloseTicketPopup.ShowOnPageLoad = true;
                            confirmCloseTicketPopup.PopupVerticalOffset = -259;
                            confirmCloseTicketPopup.PopupHorizontalAlign = PopupHorizontalAlign.WindowCenter;
                            confirmCloseTicketPopup.PopupAlignCorrection = PopupAlignCorrection.Disabled;
                            confirmCloseTicketPopup.PopupVerticalAlign = PopupVerticalAlign.Middle;
                            return;
                        }
                    }
                    else
                        closePopup = true;
                    currentTicketIdHidden.Value = saveTicket["ID"].ToString();
                    currentTicketId = Convert.ToInt32(saveTicket["ID"]);
                    currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                    //close children of current tickets
                    if (closeChildren)
                    {
                        List<Tuple<string, object>> sColumnValues = new List<Tuple<string, object>>();
                        string resolutionType = string.Empty;
                        UserProfile prp = null;
                        if (UGITUtility.IsSPItemExist(saveTicket, DatabaseObjects.Columns.TicketPRP))
                            prp = uHelper.GetUser(context, saveTicket, DatabaseObjects.Columns.TicketPRP);

                        if (UGITUtility.IsSPItemExist(saveTicket, DatabaseObjects.Columns.TicketResolutionType))
                            resolutionType = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketResolutionType]);
                        TicketRelationshipHelper ticketHelper = new TicketRelationshipHelper(context);
                        ticketHelper.CloseTickets(currentTicketPublicID, 2, txtConfrmToCloseComment.Text.Trim(), user, resolutionType);
                    }
                    //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);
                }

                #endregion

                #region Award CPR

                //else if (valid && sender != null && (sender is LinkButton) && ((LinkButton)sender).CommandArgument == "AwardCPR")
                else if (valid && senderID == "CommentsAward")
                {
                    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, context.CurrentUser.Id);
                    string previousStage = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStatus]);
                    LifeCycleStage AwardStage = TicketRequest.GetTicketAwardStage(lifeCycle);

                    if (AwardStage != null)
                    {
                        if (currentModuleName != "OPM")
                        {
                            //if (string.IsNullOrEmpty(Convert.ToString(saveTicket[DatabaseObjects.Columns.CRMProjectID])))
                            //    saveTicket[DatabaseObjects.Columns.CRMProjectID] = TicketRequest.GetNewCPRProjectNo(moduleId, SPContext.Current.Web);
                        }

                        saveTicket[DatabaseObjects.Columns.TicketStatus] = AwardStage.Name;
                        saveTicket[DatabaseObjects.Columns.StageStep] = AwardStage.StageStep;
                        //saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Join(Constants.UserInfoSeparator, AwardStage.ActionUser.ToArray());
                        saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = AwardStage.ActionUser;
                        saveTicket[DatabaseObjects.Columns.ModuleStepLookup] = AwardStage.ID;

                        if (!string.IsNullOrEmpty(txtCommentsAward.Text))
                        {
                            saveTicket[DatabaseObjects.Columns.Comment] = uHelper.GetCommentString(user, txtCommentsAward.Text.Trim(), saveTicket, DatabaseObjects.Columns.Comment, false);
                        }

                        //TicketRequest.CommitChanges(saveTicket);

                        string historyTxt = previousStage + " =>  " + AwardStage.Name + " Comment: " + txtCommentsAward.Text;
                        // string historyTxt = "Comment: " + txtCommentsAward.Text;
                        uHelper.CreateHistory(user, historyTxt, saveTicket, context);

                        TicketRequest.CommitChanges(saveTicket);

                        if (chkSendAwardNotification.Checked)
                        {
                            MailMessenger mail = new MailMessenger(context);

                            string CcMail = string.Empty;
                            string mailTo = string.Empty;
                            StringBuilder mailToList = new StringBuilder();

                            if (cbIncludeActionUser.Checked)
                            {
                                string stageActionUser = objEscalationProcess.getStageActionUsers(saveTicket);
                                //UserProfile.UsersInfo actionUseruserInfo = UserProfile.GetUserInfo(saveTicket, stageActionUser);
                                //string actionUseruserEmails=  Convert.ToString(UserManager.GetUserEmailId(stageActionUser, Constants.Separator));
                                string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailIdByGroupOrUserName(stageActionUser, Constants.Separator));
                                string[] separator = { Constants.UserInfoSeparator };
                                if (!string.IsNullOrEmpty(actionUseruserEmails))
                                {
                                    string[] actionUserEmails = actionUseruserEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < actionUserEmails.Length; i++)
                                    {
                                        string actionUser = Convert.ToString(actionUserEmails[i]);
                                        if (actionUser != string.Empty)
                                        {
                                            if (mailToList.Length != 0)
                                                mailToList.Append(";");
                                            mailToList.Append(actionUser);
                                        }
                                    }
                                }
                            }

                            if (mailToList.Length > 0)
                                mailTo = UGITUtility.RemoveDuplicateEmails(Convert.ToString(mailToList), Constants.Separator5);

                            CcMail = UGITUtility.RemoveDuplicateCcFromToEmails(mailTo, Convert.ToString(UserManager.GetUserEmailId(awardGridLookup.GetValues())), Constants.Separator5);

                            if (string.IsNullOrEmpty(mailTo) && string.IsNullOrEmpty(CcMail))
                                return;

                            if (string.IsNullOrEmpty(mailTo) && !string.IsNullOrEmpty(CcMail))
                            {
                                mailTo = CcMail;
                                CcMail = string.Empty;
                            }
                            mail.SendMail(mailTo, txtMailSubject.Text, CcMail, EmailHtmlBody.Html, true, new string[] { }, true, chkSendMailfromLoggedInUserAward.Checked, UGITUtility.ObjectToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                        }

                        closePopup = true;
                    }
                    //uHelper.ClosePopUpAndEndResponse(Context);
                }
                #endregion Award CPR

                #region Move To Precon
                else if (valid && senderID == "MoveToPrecon")
                {
                    MoveToPreconByLink(true, senderID);
                    closePopup = true;
                }
                #endregion Move To Precon

                #region Status OPM                                
                else if (valid && senderID == "lnkStatus")
                {
                    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, context.CurrentUser.Id);
                    string historyTxt = string.Empty;
                    string previousStage = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStatus]);

                    if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.CRMOpportunityStatus))
                        saveTicket[DatabaseObjects.Columns.CRMOpportunityStatus] = hndOpportunityStatus.Value;

                    if (!string.IsNullOrEmpty(txtStatusResson.Text))
                        saveTicket[DatabaseObjects.Columns.Reason] = txtStatusResson.Text;

                    if (hndOpportunityStatus.Value == "Precon")
                    {
                        if (!string.IsNullOrEmpty(txtStatusMessage.Text))
                        {
                            saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, txtStatusMessage.Text.Trim(), saveTicket, DatabaseObjects.Columns.TicketComment, false);
                        }
                        TicketRequest.CommitChanges(saveTicket, senderID, Request.Url);
                    }
                    else if (hndOpportunityStatus.Value == "Awarded")
                    {
                        UGITModule opmModule = moduleViewManager.LoadByName(ModuleNames.CPR);
                        LifeCycleStage AwardStage = TicketRequest.GetTicketAwardStage(opmModule.List_LifeCycles.FirstOrDefault());

                        if (AwardStage != null)
                        {
                            saveTicket[DatabaseObjects.Columns.TicketStatus] = AwardStage.Name;
                            saveTicket[DatabaseObjects.Columns.StageStep] = AwardStage.StageStep;
                            saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Join(Constants.Separator, AwardStage.ActionUser.ToArray());
                            saveTicket[DatabaseObjects.Columns.ModuleStepLookup] = AwardStage.ID;
                            if (!string.IsNullOrEmpty(txtStatusMessage.Text))
                            {
                                saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, txtStatusMessage.Text.Trim(), saveTicket, DatabaseObjects.Columns.TicketComment, false);
                            }

                            historyTxt += previousStage + " =>  " + AwardStage.Name;

                        }

                        if (dtcTicketAwardLossDate.Date != null && dtcTicketAwardLossDate.Date != DateTime.MinValue)
                            saveTicket[DatabaseObjects.Columns.AwardedLossDate] = dtcTicketAwardLossDate.Date;

                        TicketRequest.CommitChanges(saveTicket, senderID, Request.Url);
                    }
                    else if (hndOpportunityStatus.Value == "Cancelled" || hndOpportunityStatus.Value == "Lost" || hndOpportunityStatus.Value == "Declined")
                    {
                        LifeCycleStage closedStage = TicketRequest.GetTicketCloseStage(lifeCycle);

                        if (closedStage != null)
                        {

                            Ticket baseTicket = new Ticket(context, "OPM");
                            baseTicket.CloseTicket(saveTicket, string.Empty);

                            if (!string.IsNullOrEmpty(txtStatusMessage.Text))
                            {
                                saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, txtStatusMessage.Text.Trim(), saveTicket, DatabaseObjects.Columns.TicketComment, false);
                            }

                            if (dtcTicketAwardLossDate.Date != null && dtcTicketAwardLossDate.Date != DateTime.MinValue)
                            {
                                saveTicket[DatabaseObjects.Columns.AwardedLossDate] = dtcTicketAwardLossDate.Date;

                                if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.ClosedDateOnly))
                                    currentTicket[DatabaseObjects.Columns.ClosedDateOnly] = dtcTicketAwardLossDate.Date;
                            }

                            TicketRequest.CommitChanges(saveTicket, senderID, Request.Url);
                            historyTxt += previousStage + " =>  " + closedStage.Name;
                        }


                        if (chkOPMEmail.Checked)
                        {
                            string ToMail = string.Empty;
                            StringBuilder mailCcList = new StringBuilder();
                            string mailCc = string.Empty;

                            //List<object> vals = statusMTPUserGroups.GridView.GetSelectedFieldValues("Id");       
                            ToMail = UGITUtility.RemoveDuplicateEmails(Convert.ToString(UserManager.GetUserEmailId(statusMTPUserGroups.GetValues())), Constants.Separator5);

                            if (chkOPSActionUser.Checked)
                            {
                                string stageActionUser = objEscalationProcess.getStageActionUsers(saveTicket);
                                //UserProfile.UsersInfo actionUseruserInfo = UserProfile.GetUserInfo(saveTicket, stageActionUser);
                                //string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailId(stageActionUser));
                                string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailIdByGroupOrUserName(stageActionUser, Constants.Separator));
                                string[] separator = { Constants.UserInfoSeparator };
                                if (!string.IsNullOrEmpty(actionUseruserEmails))
                                {
                                    string[] actionUserEmails = actionUseruserEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < actionUserEmails.Length; i++)
                                    {
                                        string actionUser = Convert.ToString(actionUserEmails[i]);
                                        if (actionUser != string.Empty)
                                        {
                                            if (mailCcList.Length != 0)
                                                mailCcList.Append(";");
                                            mailCcList.Append(actionUser);
                                        }
                                    }
                                }
                            }

                            if (mailCcList.Length > 0)
                                mailCc = UGITUtility.RemoveDuplicateCcFromToEmails(ToMail, Convert.ToString(mailCcList), Constants.Separator5);

                            MailMessenger mail = new MailMessenger(context);

                            StringBuilder emailBody = new StringBuilder();
                            emailBody.AppendFormat(txtStatusMailBody.Html);

                            emailBody.AppendFormat("<br/><br/>Thanks");
                            txtStatusMailBody.Html = emailBody.ToString();
                            emailBody.Clear();

                            if (string.IsNullOrEmpty(ToMail))
                                return;

                            mail.SendMail(ToMail, txtStatusMailSubject.Text, mailCc, txtStatusMailBody.Html, true, new string[] { }, true, 
                                Convert.ToBoolean(hdnSendMailFromStatus.Value), UGITUtility.ObjectToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtStatusMessage.Text))
                        {
                            saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, txtStatusMessage.Text.Trim(), saveTicket, DatabaseObjects.Columns.TicketComment, false);
                        }
                        TicketRequest.CommitChanges(saveTicket, senderID, Request.Url);
                        historyTxt += previousStage + " =>  " + hndOpportunityStatus.Value;


                        if (chkOPMEmail.Checked)
                        {
                            MailMessenger mail = new MailMessenger(context);
                            StringBuilder emailBody = new StringBuilder();
                            StringBuilder mailCcList = new StringBuilder();

                            string ToMail = string.Empty;
                            string mailCc = string.Empty;
                            ToMail = UGITUtility.RemoveDuplicateEmails(Convert.ToString(UserManager.GetUserEmailId(statusMTPUserGroups.GetValues())), Constants.Separator5);

                            if (chkOPSActionUser.Checked)
                            {
                                string stageActionUser = objEscalationProcess.getStageActionUsers(saveTicket);
                                //UserProfile.UsersInfo actionUseruserInfo = UserProfile.GetUserInfo(saveTicket, stageActionUser);
                                //string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailId(stageActionUser));
                                string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailIdByGroupOrUserName(stageActionUser, Constants.Separator));
                                string[] separator = { Constants.UserInfoSeparator };
                                if (!string.IsNullOrEmpty(actionUseruserEmails))
                                {
                                    string[] actionUserEmails = actionUseruserEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < actionUserEmails.Length; i++)
                                    {
                                        string actionUser = Convert.ToString(actionUserEmails[i]);
                                        if (actionUser != string.Empty)
                                        {
                                            if (mailCcList.Length != 0)
                                                mailCcList.Append(";");
                                            mailCcList.Append(actionUser);
                                        }
                                    }
                                }
                            }

                            if (mailCcList.Length > 0)
                                mailCc = UGITUtility.RemoveDuplicateCcFromToEmails(ToMail, Convert.ToString(mailCcList), Constants.Separator5);

                            emailBody.AppendFormat(txtStatusMailBody.Html);
                            emailBody.AppendFormat("<br/><br/>Thanks");
                            txtStatusMailBody.Html = emailBody.ToString();
                            emailBody.Clear();

                            if (string.IsNullOrEmpty(ToMail) && !string.IsNullOrEmpty(mailCc))
                            {
                                ToMail = mailCc;
                                mailCc = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(ToMail))
                                mail.SendMail(ToMail, txtStatusMailSubject.Text, mailCc, txtStatusMailBody.Html, true, new string[] { }, true, Convert.ToBoolean(hdnSendMailFromStatus.Value), UGITUtility.ObjectToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                        }
                    }

                    closePopup = true;

                    if (!string.IsNullOrEmpty(historyTxt))
                        uHelper.CreateHistory(user, historyTxt, saveTicket, context);

                    if (hndOpportunityStatus.Value == "Awarded" && chkboxStageMoveToPrecon.Checked && chkStatusOpenCPR.Checked)
                    {
                        closePopup = false;
                        string script = "<script type=\"text/javascript\"> TicketStageToMovePrecon(); </script>";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", script);
                    }
                    else if (hndOpportunityStatus.Value == "Awarded" || hndOpportunityStatus.Value == "Precon")
                    {
                        MoveToPreconByLink(true, senderID);
                    }

                    if (Convert.ToBoolean(hndCompleteTasksOnItemClose.Value))
                    {
                        CompleteTasksOnItemClose(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                    }

                    GetDefaultData();
                    BindStageGraphic();
                }
                #endregion Status OPM

                #region Reject Button
                else if (senderID == "rejectButton" || senderID == "rejectWithCommentsButton")
                {
                    // Note this is for both Reject & Cancel at 1st stage
                    if (senderID == "rejectButton" && popedRejectComments.Text.Trim() == string.Empty && currentModuleName != "CPR" && currentModuleName != "CNS" && currentModuleName != "OPM")
                    {
                        errors.Add(TicketColumnError.AddError("Please enter comment."));
                        valid = false;
                    }

                    if (valid)
                    {
                        actionPerformed = Convert.ToString(TicketActionType.Rejected);
                        if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                            oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                        if (currentModuleName == "LEM")
                            saveTicket[DatabaseObjects.Columns.ReasonType] = ddlLEDRejectType.SelectedValue;
                        TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);

                        string rejectComments = UGITUtility.WrapComment(popedRejectComments.Text, "reject");
                        if (currentModuleName == ModuleNames.CPR || currentModuleName == ModuleNames.OPM || currentModuleName == ModuleNames.CNS)
                            rejectComments = UGITUtility.WrapComment(popedLossRejectComments.Text, "reject");
                        TicketRequest.RejectTicket(errors, saveTicket, rejectComments, performedActionName: actionPerformed);
                        
                        if (valid)
                            closePopup = true;

                        if (currentModuleName == "CPR" || currentModuleName == "CNS" || currentModuleName == "OPM")
                        {
                            //Remove below comments after RMM module is completed.
                            //ResourceAllocation.ReleaseResourcesForClosingProject(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));

                            if (chkSendLossEmail.Checked)
                            {
                                string ccMail = string.Empty;
                                StringBuilder mailToList = new StringBuilder();
                                if (cbIncludeLossActionUser.Checked)
                                {
                                    string stageActionUser = objEscalationProcess.getStageActionUsers(saveTicket);
                                    string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailIdByGroupOrUserName(stageActionUser, Constants.Separator));
                                    string[] separator = { Constants.UserInfoSeparator };
                                    if (!string.IsNullOrEmpty(actionUseruserEmails))
                                    {
                                        string[] actionUserEmails = actionUseruserEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                        for (int i = 0; i < actionUserEmails.Length; i++)
                                        {
                                            string actionUser = Convert.ToString(actionUserEmails[i]);
                                            if (actionUser != string.Empty)
                                            {
                                                if (mailToList.Length != 0)
                                                    mailToList.Append(";");
                                                mailToList.Append(actionUser);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    mailToList.Append( Convert.ToString(UserManager.GetUserEmailId(txtTo.GetValues())));
                                }

                                string mailTo = UGITUtility.RemoveDuplicateEmails(Convert.ToString(mailToList), Constants.Separator5);
                                if(currentModuleName != "CPR" && currentModuleName != "CNS" && currentModuleName != "OPM")
                                   ccMail = UGITUtility.RemoveDuplicateCcFromToEmails(mailTo, Convert.ToString(UserManager.GetUserEmailId(lossGridLookup.GetValues())), Constants.Separator5);

                                //modification changes the mailto cc and cc to mailto.
                                MailMessenger mail = new MailMessenger(context);
                                mail.SendMail(mailTo, txtMailLossSubject.Text, ccMail, EmailLossHtmlBody.Html, true, new string[] { }, true, false, UGITUtility.ObjectToString(saveTicket[DatabaseObjects.Columns.TicketId]));

                            }
                        }
                        else if (currentModuleName == ModuleNames.COM)
                        {
                            bool InactivateRelatedContactsOfCompany = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.InactivateRelatedContactsOfCompany);
                            if (InactivateRelatedContactsOfCompany)
                            {
                                InactivateRelatedContacts(currentTicketId, currentTicketPublicID);
                            }
                        }
                        else
                        {
                            // Send reject/cancel notification to action users
                            if (TicketRequest.Module.ActionUserNotificationOnCancel || TicketRequest.Module.RequestorNotificationOnCancel || TicketRequest.Module.InitiatorNotificationOnCancel)
                            {
                                //string ticketId = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                                //string title = Convert.ToString(saveTicket[DatabaseObjects.Columns.Title]);
                                //string moduleType = uHelper.moduleTypeName(currentModuleName);
                                //string subject = string.Format("{0} Cancelled: {1}", moduleType, title);
                                //string mailBody = string.Format("The {0} <b>{1}</b> was cancelled. <br/><br/>{2}", moduleType, title, uHelper.WrapCommentForEmail(popedRejectComments.Text, "reject"));
                                //if (TicketRequest.Module.ActionUserNotificationOnCancel)
                                //    TicketRequest.SendEmailToActionUsers(saveTicket, subject, mailBody);
                                //if (TicketRequest.Module.RequestorNotificationOnCancel)
                                //    TicketRequest.SendEmailToRequestor(saveTicket, subject, mailBody);
                                //if (TicketRequest.Module.InitiatorNotificationOnCancel)
                                //    TicketRequest.SendEmailToInitiator(saveTicket, subject, mailBody);
                            }

                            // Delete any pending escalations for this ticket
                            // Got to do it here since escalations block below may not be executed if not valid
                            EscalationProcess.DeleteEscalation(saveTicket);
                        }

                        currentTicketIdHidden.Value = saveTicket["ID"].ToString();
                        currentTicketId = Convert.ToInt32(saveTicket["ID"]);
                        currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                    }
                    //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);

                    if (Convert.ToBoolean(hndCompleteTasksOnItemClose.Value))
                    {
                        CompleteTasksOnItemClose(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                    }
                }
                #endregion
                #region Create Button Clicked
                else if (valid && (senderID == "createButton" || senderID == "createCloseButton" || senderID == "saveAsDraftButton" || senderID == "relatedTitleButton") && valid)
                {
                    try // Need to figure out why frequent crashes when creating ticket
                    {
                        actionPerformed = Convert.ToString(TicketActionType.Created);
                        if (Request["batchCreate"] == "true")
                        {
                            string[] userid = Request["userIds"].Split(',');
                            foreach (string item in userid)
                            {
                                DataRow newListItem = TicketSchema.NewRow();
                                saveTicket = newListItem;
                                NewCreateTicket(valid, senderID, errors, formValues, updateChangesInHistory, item, performedActionName: actionPerformed);

                                //added condition for escalation process..
                                if (valid && senderID != "superAdminEditButton" && saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                                {
                                    //EscalationProcess escalate = new EscalationProcess(thisWeb);
                                    if (saveTicket != null && saveTicket[DatabaseObjects.Columns.TicketId] != null) // && saveTicket[DatabaseObjects.Columns.TicketPriorityLookup] != null)
                                    {
                                        // Recreate escalation emails
                                        EscalationProcess.DeleteEscalation(saveTicket);
                                        EscalationProcess.GenerateEscalationInBackground(TicketRequest, currentTicketPublicID, moduleId);

                                    }
                                }
                            }
                        }
                        else
                        {
                            NewCreateTicket(valid, senderID, errors, formValues, updateChangesInHistory, user.Id, performedActionName: actionPerformed);
                            if (resetPasswordAgent != null && resetPasswordAgent.IsResetPasswordAgentActivated == true)
                            {
                                if (resetPasswordAgent.IsRequestorIsGroup == true || resetPasswordAgent.IsInitiatorEqualRequestor == true)
                                {
                                    return;
                                }
                            }
                        }
                        redirectAgain = false;
                        uHelper.ClosePopUpAndEndResponse(Context, false);


                    }
                    catch (Exception ex)
                    {
                        Util.Log.ULog.WriteException(ex);
                        errorMsgNew.Text = "Something went wrong.Kindly contact admin.";
                    }
                }

                #endregion
                #region Update Button Clicked
                else if (valid && senderID == "updateButton" && Request["BatchEditing"] != "true" || senderID == "btnupdateButtonSC")
                {
                    string oldcrmComplexity = string.Empty;
                    if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                        oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                    if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.CRMProjectComplexity) && saveTicket[DatabaseObjects.Columns.CRMProjectComplexity] != DBNull.Value)
                        oldcrmComplexity = Convert.ToString(saveTicket[DatabaseObjects.Columns.CRMProjectComplexity]);

                    //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
                    if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketPRP, saveTicket.Table))
                        oldPRPValue = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketPRP]);
                    //
                    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);
                    // code to remove Value from Lead Priority while Save button is clicked repeatedly.
                    if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.SuccessChance))
                    {
                        string leadPriority = Convert.ToString(saveTicket[DatabaseObjects.Columns.SuccessChance]);
                        //if (!string.IsNullOrEmpty(leadPriority) && leadPriority.IndexOf(' ') != -1)

                        if (!string.IsNullOrEmpty(leadPriority))
                        {
                            if (leadPriority.IndexOf(" (") != -1)
                            {
                                leadPriority = leadPriority.Substring(0, leadPriority.IndexOf(" ("));
                            }

                            //saveTicket[DatabaseObjects.Columns.SuccessChance] = leadPriority.Substring(0, leadPriority.IndexOf(' '));
                            // Made changes to save only leadPriority (text), remove Numbers, if any
                            saveTicket[DatabaseObjects.Columns.SuccessChance] = leadPriority;
                        }
                    }

                    if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.CRMProjectComplexity) && UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.ApproxContractValue))
                    {
                        if (saveTicket[DatabaseObjects.Columns.ApproxContractValue] != DBNull.Value)
                        {
                            saveTicket[DatabaseObjects.Columns.CRMProjectComplexity] = uHelper.GetProjectComplexity(context, Convert.ToDouble(saveTicket[DatabaseObjects.Columns.ApproxContractValue]));
                        }
                    }

                    ProjectEstimatedAllocationManager projectEstimatedAllocationMGR = new ProjectEstimatedAllocationManager(context);
                    if (hdnAutoUpdateAllocaionDates.Value.ToLower() == "true")
                    {
                        projectEstimatedAllocationMGR.UpdatedAllocationDates(saveTicket[DatabaseObjects.Columns.TicketId].ToString(), saveTicket, 
                            hdnUpdatePastDates.Value.ToLower() == "true" ? true : false);
                    }

                    string error = TicketRequest.CommitChanges(saveTicket, senderID, Request.Url, donotUpdateEscalations: actionUserChanged);
                    UGITTaskManager objTask = new UGITTaskManager(context);
                    if (objTask.IsModuleTasks(currentModuleName))
                    {
                        List<UGITTask> currentTicketDetails = objTask.Load(x => x.RelatedTicketID == UGITUtility.ObjectToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                        foreach (UGITTask item in currentTicketDetails)
                        {
                            item.Title = UGITUtility.ObjectToString(saveTicket[DatabaseObjects.Columns.Title]);
                            objTask.Save(item);
                        }
                    }
                    if (!string.IsNullOrEmpty(error))
                    {
                        errors.Add(TicketColumnError.AddError(error));
                        valid = false;
                    }


                    if (valid)
                    {
                        if (actionUserChanged)
                        {
                            TicketRequest.CheckRequestType(saveTicket, false);
                            string actionUserTypes = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]);
                            if (!string.IsNullOrEmpty(actionUserTypes) && actionUserTypes.IndexOf(Constants.Separator10) != -1)
                                actionUserTypes = actionUserTypes.Replace(Constants.Separator10, Constants.Separator);

                            saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context, actionUserTypes, saveTicket);
                            TicketRequest.CommitChanges(saveTicket, senderID, Request.Url);

                            //if (objConfigurationVariableHelper.GetConfigVariableValue(ConfigConstants.NotifyNewPRPORPOnly)!=null)
                            //    TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleId, newPRPorORPUsers);
                            //else
                            //    TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleId);

                            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
                            // Save ticket event entry when PRP changed
                            string newPRPValue = UGITUtility.GetSPItemValueAsString(saveTicket, DatabaseObjects.Columns.TicketPRP);
                            if (!string.IsNullOrEmpty(newPRPValue) && newPRPValue != oldPRPValue)
                            {
                                LifeCycleStage ticketStage = TicketRequest.GetTicketCurrentStage(saveTicket);
                                if (ticketStage.StageTypeChoice == StageType.Assigned.ToString())
                                {

                                    string affectedUsers = context.CurrentUser.Id;
                                    TicketEventManager eventHelper = new TicketEventManager(context, currentModuleName, currentTicketPublicID);
                                    eventHelper.LogEvent(Constants.TicketEventType.Assigned, ticketStage, affectedUsers: affectedUsers);

                                }
                            }
                        }

                        // Update the Schedule Action for Email if the module is DRQ.
                        if (currentModuleName == "DRQ")
                        {
                            //AgentJobHelper agent = new AgentJobHelper(thisWeb);
                            //agent.UpdateDRQScheduleActionEmail(saveTicket, moduleId);
                            ////Ticket.UpdateEMailQueue(saveTicket,thisWeb);
                        }
                        else if (currentModuleName == "CMT")
                        {
                            //AgentJobHelper agentHelper = new AgentJobHelper(SPContext.Current.Web);
                            //agentHelper.UpdateCMTScheduleActionReminder(saveTicket, moduleId);
                        }

                        if (!string.IsNullOrEmpty(currentStage.SkipOnCondition) && FormulaBuilder.EvaluateFormulaExpression(context, currentStage.SkipOnCondition, saveTicket))
                        {
                            ApproveTicketRequest(errors, senderID);
                        }
                        //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(moduleId, saveTicket);
                        currentTicketId = Convert.ToInt32(saveTicket["ID"]);
                        currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                        currentTicketIdHidden.Value = Convert.ToString(saveTicket["ID"]);
                        stageDescriptionContainer.Visible = true;

                        //set cookie to refresh parent page when he saved detail. which force parent to refresh.
                        var refreshParentID = UGITUtility.GetCookieValue(Request, "framePopupID");
                        if (!string.IsNullOrWhiteSpace(refreshParentID))
                        {
                            UGITUtility.CreateCookie(Response, "refreshParent", refreshParentID);
                        }
                        if (senderID == "updateButton" && bClose == false)
                            redirectAgain = true;
                        //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);
                    }
                }
                #endregion
                #region Hold-UnHold Button Clicked
                else if (senderID == "UnHoldButton" || senderID == "HoldButton")
                {
                    if (senderID == "HoldButton")
                    {
                        string msg = string.Empty;
                        if (string.IsNullOrEmpty(aspxdtOnHoldDate.Text))
                        {
                            if (msg == string.Empty)
                                errors.Add(TicketColumnError.AddError("Please enter the Hold Till date."));
                            else
                                errors.Add(TicketColumnError.AddError("Please enter comment and Hold Till date."));
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            errors.Add(TicketColumnError.AddError(msg));
                            valid = false;
                        }

                        if (valid)
                        {
                            actionPerformed = Convert.ToString(TicketActionType.OnHold);
                            closePopup = true;
                            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                                oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                            TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);
                            DateTime holdTill = new DateTime(aspxdtOnHoldDate.Date.Year, aspxdtOnHoldDate.Date.Month, aspxdtOnHoldDate.Date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                            bool isEnableCloseOnHoldExpire = chkEnableCloseOnHoldExpire.Checked;

                            string holdReason = string.Empty;
                            if (hdnOnHoldReason.Value == "0")
                            {
                                holdReason = ddlOnHoldReason.SelectedItem.Text;
                            }
                            else
                            {
                                holdReason = txtOnHoldReason.Text;
                                UGITModule moduleObj = ObjModuleViewManager.GetByName(currentModuleName);
                                FieldConfiguration configField = configFieldManager.GetFieldByFieldName(DatabaseObjects.Columns.OnHoldReasonChoice, moduleObj.ModuleTable);
                                if (configField != null && !string.IsNullOrWhiteSpace(holdReason))
                                {
                                    if (!string.IsNullOrWhiteSpace(configField.Data))
                                    {
                                        configField.Data = !string.IsNullOrWhiteSpace(hdnRequestOnHoldReason.Value)
                                            ? configField.Data.Replace(hdnRequestOnHoldReason.Value, holdReason)
                                            : !UGITUtility.SplitString(configField.Data, Constants.Separator).Contains(holdReason)
                                                ? configField.Data + Constants.Separator + holdReason
                                                : configField.Data;
                                    }
                                    else
                                    {
                                        configField.Data = holdReason;
                                    }
                                    configFieldManager.Update(configField);
                                }
                            }
                            TicketRequest.HoldTicket(saveTicket, currentModuleName, popedHoldComments.Text.Trim(), holdTill, holdReason, isEnableCloseOnHoldExpire);
                            TicketRequest.CommitChanges(saveTicket, "");
                            ObjCRMProjectAllocationMgr.UpdatedAllocationType(currentTicketPublicID, true);

                            //Send Email notification.
                            if (senderID == "NotifyOnHoldButton")
                                TicketRequest.sendEmailToActionUsersOnHoldStage(currentTicket, Convert.ToString(moduleId), UGITUtility.WrapCommentForEmail(popedHoldComments.Text.Trim(), "hold", holdTill, holdReason), performedActionName: actionPerformed);
                        }

                    }
                    else if (senderID == "UnHoldButton")
                    {
                        if (popedUnHoldComments.Text.Trim() == string.Empty)
                        {
                            errors.Add(TicketColumnError.AddError("Please enter comment."));
                            valid = false;
                        }


                        if (valid)
                        {
                            closePopup = true;
                            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                                oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                            TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);
                            string unholdComments = UGITUtility.WrapComment(popedUnHoldComments.Text.Trim(), "unhold");
                            TicketRequest.UnHoldTicket(saveTicket, string.Empty, unholdComments);
                            TicketRequest.CommitChanges(saveTicket, Request.Url.ToString());
                            ObjCRMProjectAllocationMgr.UpdatedAllocationType(currentTicketPublicID, false);

                            AgentJobHelper agentHelper = new AgentJobHelper(context);
                            agentHelper.CancelUnHoldTicket(currentTicketPublicID);
                            TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, UGITUtility.ObjectToString(moduleId),
                                                                 string.Format("<b>This {0} has been taken off hold.</b>", UGITUtility.moduleTypeName(currentModuleName).ToLower(), UGITUtility.WrapCommentForEmail(popedUnHoldComments.Text.Trim(), "UnHold")), string.Empty);
                        }

                    }

                    currentTicketId = Convert.ToInt32(saveTicket[DatabaseObjects.Columns.Id]);
                    currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                    currentTicketIdHidden.Value = saveTicket["ID"].ToString();
                    //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);
                }
                #endregion
                #region Return Button Clicked
                else if (senderID == "returnButton" || senderID == "returnWithCommentsButton")
                {
                    UGITModule currentModuleRow = ModuleViewManager.LoadByName(currentModuleName);
                    //an condition added for update the current stage on the base of request type.
                    if (popedReturnComments.Text == string.Empty && !IsRequestTypeChange && !TicketRequest.Module.ReturnCommentOptional)
                    {
                        errors.Add(TicketColumnError.AddError("Please enter comment."));
                        valid = false;
                    }

                    if (valid)
                    {
                        actionPerformed = Convert.ToString(TicketActionType.Returned);
                        string returnType = (currentTicketStatus.ToLower() == "closed" ? "reopen" : "return");
                        ///to set Re-Open count for report.
                        if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ReopenCount, saveTicket.Table))
                        {
                            if (returnType == "reopen")
                            {
                                int reopencount = Convert.ToInt32(UGITUtility.StringToInt(saveTicket[DatabaseObjects.Columns.ReopenCount]));
                                reopencount = reopencount + 1;
                                saveTicket[DatabaseObjects.Columns.ReopenCount] = reopencount;
                            }
                        }
                        string returnComment = UGITUtility.WrapComment(popedReturnComments.Text.Trim(), returnType);
                        int? returnStageStep = null;
                        //if (trPopedReturnStageDDL.Visible && ddlPopedReturnStages.SelectedItem != null)
                        //    returnStageStep = Convert.ToInt32(ddlPopedReturnStages.SelectedValue);

                        //condition for change request type... new..
                        if (IsRequestTypeChange)
                        {
                            // Clear out PRP value
                            TicketColumnValue tempValue = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketPRP);
                            if (tempValue != null)
                                tempValue.Value = "";

                            // Clear out ORP value
                            tempValue = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketORP);
                            if (tempValue != null)
                                tempValue.Value = "";


                        }

                        if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                            oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;


                        TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);
                        if (IsRequestTypeChange)
                            TicketRequest.CheckRequestType(saveTicket, false);

                        // Change status to "Returned" only if not request type change
                        TicketRequest.Return(moduleId, currentTicket, returnComment, !IsRequestTypeChange, returnStageStep);
                        TicketRequest.CommitChanges(saveTicket, senderID, Request.Url);

                        currentTicketId = Convert.ToInt32(saveTicket["ID"]);
                        currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                        currentTicketIdHidden.Value = saveTicket["ID"].ToString();
                        TicketRequest.SendEmailToActionUsers(returnStageId, currentTicket, UGITUtility.ObjectToString(moduleId), string.Empty, UGITUtility.WrapCommentForEmail(popedReturnComments.Text.Trim(), returnType), performedAction: actionPerformed);
                        closePopup = false;
                        redirectAgain = true;
                        //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);
                        uHelper.ClosePopUpAndEndResponse(Context, redirectAgain, Convert.ToString(Request.Url));
                    }
                }
                #endregion

                #region CreateBaseline
                //check sender id
                else if (valid && senderID == "createBaseline" && hiddenCreateBaseline.Value == "true")//check senderid
                {
                    if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                        oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;


                    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, "0");

                    TicketRequest.CommitChanges(saveTicket, Request.Url.ToString());

                    hiddenCreateBaseline.Value = string.Empty;

                    baselineDate = DateTime.Now;

                    Baseline baseline = new Baseline(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]), baselineDate, context);

                    baseline.BaselineComment = createBaselineDesc.Text.Trim();

                    saveTicket.SetModified();


                    // Create baseline: Note CreateBaseline generates new version using pmmItem.Update()
                    baseline.CreateBaseline(saveTicket, currentModuleName, currentTicketId);

                    createBaselineDesc.Text = "";
                }
                #endregion
                #region Create TSR
                else if (senderID == "createTSRButton")
                {

                }
                #endregion
                #region Close PMM
                else if (valid && sender != null && senderID == "ClosePMM")
                {
                    if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate))
                        oldTargetCompletionDate = UGITUtility.StringToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]);

                    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, context.CurrentUser.Id);

                    TicketRequest.ClosePMMTicket(saveTicket, string.Empty);
                    TicketRequest.CommitChanges(saveTicket);
                    redirectAgain = false;
                    uHelper.ClosePopUpAndEndResponse(Context, true, Convert.ToString(Request.Url));
                }
                #endregion #region Self Assign
                #region Self Assigned
                //else if (senderID == "selfAssignConfirm")
                //{
                //    // Re-check still in "Pending Assignment" in case ticket has moved since we opened!
                //    if (currentStage != null && currentStage.Prop_SelfAssign == true)
                //    {
                //        if (saveTicket.Fields.ContainsField(DatabaseObjects.Columns.TicketTargetCompletionDate))
                //            oldtargetcompletiondate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                //        TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, 0);

                //        LifeCycleStage nextStage = currentStage.ApprovedStage;

                //        //**TicketRequest.LogStageTransition(saveTicket, currentStage, nextStage, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]));

                //        string historyDescription = string.Format("{0} => (Self Assigned)", uHelper.GetSPItemValue(saveTicket, DatabaseObjects.Columns.TicketStatus));
                //        uHelper.CreateHistory(thisWeb.CurrentUser, historyDescription, currentTicket);

                //        saveTicket[DatabaseObjects.Columns.TicketPRP] = thisWeb.CurrentUser.ID;
                //        saveTicket[DatabaseObjects.Columns.TicketEstimatedHours] = Convert.ToDouble(popedEstimatedHours.Text);

                //        saveTicket[DatabaseObjects.Columns.TicketStatus] = nextStage.Name;
                //        // saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = GetStageActionUsersByStageTitle(Convert.ToString(stage[DatabaseObjects.Columns.StageApprovedStatus]));
                //        saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes] = string.Join(Constants.Separator, nextStage.ActionUser.ToArray());
                //        saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), saveTicket);

                //        nextStage = TicketRequest.SetNextStage(thisWeb, saveTicket, moduleId);

                //        saveTicket[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.Now.ToString();
                //        TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleId);
                //        TicketRequest.CommitChanges(saveTicket, Request.Url);
                //        closePopup = true;
                //    }
                //}
                #endregion
                #region Admin Overrride
                else if (senderID == "superAdminEditButton")
                {
                    adminOverride = true;
                    eAll.Value = "1";
                    errorMsg.Text = string.Empty;
                    GetDefaultData();
                    showAdminEditButton = false;
                    BindMenu();
                    errors = new List<TicketColumnError>();
                    valid = true;
                    try
                    {
                        //new for get the all ticket data.
                        if (Request.QueryString["AllTickets"] != null && Request.QueryString["BatchEditing"] == "true")
                        {
                            TicketIdList = Convert.ToString(Request["AllTickets"]).Split(new string[] { "," }, StringSplitOptions.None).ToList();
                            LoadTicketsCollection();
                        }
                        BindTabs(); // Crashes in here - is this needed?
                    }
                    catch { };
                    BindStageGraphic();//Commented By Munna
                    InitializeButtons();

                    LoadActionPopupMenu(ASPxPopupActionMenu);
                }
                #endregion
                #region Incident Tracking
                //// In case of Inciden Tracking Resolve stage before resolving 
                //if (senderID == "btnSendNotification" && valid == true)
                //{
                //    if (saveTicket.Fields.ContainsField(DatabaseObjects.Columns.TicketTargetCompletionDate))
                //        oldtargetcompletiondate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                //    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, 0);
                //    TicketRequest.CommitChanges(saveTicket, Request.Url);
                //    closePopup = true;
                //    uHelper.SendIncidentNotification(saveTicket[DatabaseObjects.Columns.TicketId].ToString(), txtNotificationTo.Text, txtNotificationBody.Text, txtActions.Text, chkImpactedUser.Checked);
                //}
                #endregion
                #region Add Comment
                //else if (senderID == "addTicketHoursBt")
                //{
                //    if (txtTicketHours.Text.Trim() != string.Empty && saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketResolutionComments))
                //    {

                //        DataTable spTicketHours = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketHours);
                //        DataRow item = spTicketHours.NewRow();
                //        TicketHours ticketHour = new TicketHours();
                //        ticketHour.TicketID = currentTicketPublicID;
                //        ticketHour.StageStep = currentStep;
                //        //Allow user to enter max 24 hours in a day
                //        double workHours = UGITUtility.StringToDouble(txtTicketHours.Text);
                //        if (workHours > 24)
                //            workHours = 24;

                //        DateTime workDate = dtcTicketHoursDate.Date;
                //        ticketHour.HoursTaken= workHours;
                //        ticketHour.Comment = txtResolutionDescription.Text;
                //        ticketHour.Resource = user.Id;
                //        ticketHour.WorkDate = workDate;
                //      TicketHoursManager.AddUpdate(ticketHour);

                //        //Get Total actual hours for current stage
                //       string query = string.Format("{0}='{1}' and {2}={3}", DatabaseObjects.Columns.TicketId, currentTicketPublicID, DatabaseObjects.Columns.StageStep, currentStep);
                //        DataRow[] spColl = spTicketHours.Select(query);
                //        double totalActualHours = 0;
                //        if (spColl != null && spColl.Count() > 0)
                //        {
                //            DataTable dt = spColl.CopyToDataTable();
                //            totalActualHours = UGITUtility.StringToDouble(dt.Compute("SUM(" + DatabaseObjects.Columns.HoursTaken + ")", string.Empty));
                //        }

                //        TicketColumnValue rcColumn = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketResolutionComments);
                //        if (rcColumn == null)
                //        {
                //            rcColumn = new TicketColumnValue();
                //            rcColumn.InternalFieldName = DatabaseObjects.Columns.TicketResolutionComments;
                //            formValues.Add(rcColumn);
                //        }
                //        rcColumn.Value = txtResolutionDescription.Text.Trim();

                //        TicketColumnValue rAHColumn = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketActualHours);
                //        if (rAHColumn == null)
                //        {
                //            rAHColumn = new TicketColumnValue();
                //            rAHColumn.InternalFieldName = DatabaseObjects.Columns.TicketActualHours;
                //            formValues.Add(rAHColumn);
                //        }
                //        rAHColumn.Value = totalActualHours;

                //        TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);
                //        TicketRequest.CommitChanges(saveTicket,"", donotUpdateEscalations: actionUserChanged);
                //        pcTicketHours.ShowOnPageLoad = false;
                //        redirectAgain = true;

                //        // Notify requestor or action user of new comment if configured in Modules list for this module
                //        if (TicketRequest.Module.ActionUserNotificationOnComment || chkNotifyCommentRequestor.Checked || TicketRequest.Module.InitiatorNotificationOnComment)
                //        {
                //            string ticketId = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                //            string mailBody = string.Format("{0} added the following comment to this ticket: <br/><br/>{1}",user.Name, txtResolutionDescription.Text.Trim());
                //            string subject = string.Format("New Comment added to ticket: {0}", ticketId);
                //            if (TicketRequest.Module.ActionUserNotificationOnComment)
                //                TicketRequest.SendEmailToActionUsers(saveTicket, subject, mailBody);
                //            if (chkNotifyCommentRequestor.Checked)
                //                //TicketRequest.SendEmailToRequestor(saveTicket, subject, mailBody);
                //                if (TicketRequest.Module.InitiatorNotificationOnComment) ;
                //                //TicketRequest.SendEmailToInitiator(saveTicket, subject, mailBody);
                //        }


                //        //Update user working hours inside resource timesheet if setting is enabled
                //        if (UGITUtility.StringToBoolean(objConfigurationVariableHelper.GetValue(ConfigConstants.CopyTicketActualsToRMM)))
                //        {
                //            int requestTypeLookup = Convert.ToInt16(saveTicket[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                //            if (requestTypeLookup > 0)
                //            {
                //                ModuleRequestType requestType = TicketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.ID == requestTypeLookup);
                //                if (requestType != null)
                //                {
                //                    //ResourceWorkItem workItem = new Helpers.ResourceWorkItem(SPContext.Current.Web.CurrentUser.ID);
                //                    //workItem.Level1 = requestType.RMMCategory;
                //                    //workItem.Level2 = requestType.Category;

                //                    //ResourceTimeSheet.UpdateWorkingHours(SPContext.Current.Web, workItem, SPContext.Current.Web.CurrentUser.ID, workDate, workHours, false);
                //                }
                //            }
                //        }
                //    }
                //}
                else if (senderID == "addCommentBt")
                {
                    if (txtAddComment.Text.Trim() != string.Empty)
                    {
                        if (txtAddComment.Text.Trim() != string.Empty)
                        {
                            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                                oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                            TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, user.Id);
                            saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, txtAddComment.Text.Trim(), saveTicket, DatabaseObjects.Columns.TicketComment, chkAddPrivate.Checked);
                            TicketRequest.CommitChanges(saveTicket, senderID, Request.Url, donotUpdateEscalations: actionUserChanged);

                            // Notify requestor or action user of new comment if configured in Modules list for this module
                            //Added by mudassir 10 march 2020
                            //if (TicketRequest.Module.ActionUserNotificationOnComment || TicketRequest.Module.RequestorNotificationOnComment || TicketRequest.Module.InitiatorNotificationOnComment)
                            if (TicketRequest.Module.ActionUserNotificationOnComment || chkNotifyRequestor.Checked || TicketRequest.Module.InitiatorNotificationOnComment)
                            //
                            {
                                //Added by mudassir 10 march 2020
                                if (chkNotifyRequestor.Checked)
                                {
                                    //ActionHistory.ActionName = "addcomment";
                                    //Add Plain Text and No-Ticket Link Option
                                    //bool isHtmlBody = true;
                                    //bool disableTicketLink = false;
                                    //if (chkPlainText.Checked)
                                    //    isHtmlBody = false;
                                    //if (chkDisableTicketLink.Checked)
                                    //    disableTicketLink = true;

                                }
                                //
                                string ticketId = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                                string mailBody = string.Format("{0} added the following comment to this ticket: <br/><br/>{1}", user.UserName, txtAddComment.Text.Trim());
                                string subject = string.Format("New Comment added to ticket: {0}", ticketId);
                                if (TicketRequest.Module.ActionUserNotificationOnComment)
                                    TicketRequest.SendEmailToActionUsers(saveTicket, subject, mailBody);
                                if (TicketRequest.Module.RequestorNotificationOnComment)
                                    TicketRequest.SendEmailToRequestor(saveTicket, subject, mailBody);
                                if (TicketRequest.Module.InitiatorNotificationOnComment)
                                    TicketRequest.SendEmailToInitiator(saveTicket, subject, mailBody);
                            }

                            if (actionUserChanged)
                            {
                                TicketRequest.CheckRequestType(saveTicket, false);
                                string actionusertypes = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]);
                                if (!string.IsNullOrEmpty(actionusertypes) && actionusertypes.IndexOf(Constants.Separator10) != -1)
                                    actionusertypes = actionusertypes.Replace(Constants.Separator10, Constants.Separator);

                                saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context, actionusertypes, saveTicket);
                                TicketRequest.CommitChanges(saveTicket, "");

                                if (ConfigurationVariableManager.GetValueAsBool(ConfigConstants.NotifyNewPRPORPOnly))
                                    TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, currentModuleName, null);
                                else
                                    TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, currentModuleName, null);
                            }

                            // Update the Schedule Action for Email if the module is DRQ.
                            if (currentModuleName == "DRQ")
                            {
                                //AgentJobHelper agent = new AgentJobHelper(thisWeb);
                                //agent.UpdateDRQScheduleActionEmail(saveTicket, moduleId);
                            }
                            else if (currentModuleName == "CMT")
                            {
                                //AgentJobHelper agentHelper = new AgentJobHelper(SPContext.Current.Web);
                                //agentHelper.UpdateCMTScheduleActionReminder(saveTicket, moduleId);
                            }


                            string listUrl = Request.Path;
                            NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            if (!string.IsNullOrEmpty(hdnActiveTab.Value))
                                queryCollection.Set("showTab", hdnActiveTab.Value);

                            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
                            //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);
                            Response.Redirect(listUrl);
                        }
                    }
                    //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);

                    redirectAgain = true;
                }

                #endregion
                #region Hidden Button Clicked
                //else if (valid && senderID == "hdnUpdateButton")
                //{
                //    if (saveTicket.Fields.ContainsField(DatabaseObjects.Columns.TicketTargetCompletionDate))
                //        oldtargetcompletiondate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                //    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, 0);
                //    string error = TicketRequest.CommitChanges(saveTicket, Request.Url, donotUpdateEscalations: actionUserChanged);

                //    if (!string.IsNullOrEmpty(error))
                //    {
                //        errors.Add(TicketColumnError.AddError(error));
                //        valid = false;
                //    }


                //    if (valid)
                //    {
                //        //For handeling Send Email to new added PRP/ORP user only..  Start
                //        if (actionUserChanged)
                //        {
                //            TicketRequest.CheckRequestType(thisWeb, saveTicket, uGITCache.GetModuleDetails(currentModuleName), false);
                //            saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), saveTicket);
                //            TicketRequest.CommitChanges(saveTicket, Request.Url);

                //            if (objConfigurationVariableHelper.GetConfigVariableValueAsBool(ConfigConstants.NotifyNewPRPORPOnly, SPContext.Current.Web))
                //                TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleId, newPRPorORPUsers);
                //            else
                //                TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleId);
                //        }

                //        // Update the Schedule Action for Email if the module is DRQ.
                //        if (currentModuleName == "DRQ")
                //        {
                //            AgentJobHelper agent = new AgentJobHelper(thisWeb);
                //            agent.UpdateDRQScheduleActionEmail(saveTicket, moduleId);
                //            //Ticket.UpdateEMailQueue(saveTicket,thisWeb);
                //        }
                //        else if (currentModuleName == "CMT")
                //        {
                //            AgentJobHelper agentHelper = new AgentJobHelper(SPContext.Current.Web);
                //            agentHelper.UpdateCMTScheduleActionReminder(saveTicket, moduleId);
                //        }

                //        if (!string.IsNullOrEmpty(currentStage.SkipOnCondition) && FormulaBuilder.EvaluateFormulaExpression(context,currentStage.SkipOnCondition, saveTicket))
                //        {
                //            ApproveTicketRequest(thisWeb, errors);
                //        }

                //        uGITCache.ModuleDataCache.UpdateOpenTicketsCache(moduleId, saveTicket);

                //        currentTicketId = saveTicket.ID;
                //        currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                //        currentTicketIdHidden.Value = saveTicket.ID.ToString();
                //        stageDescriptionContainer.Visible = true;
                //        if (senderID != "updateButton" && senderID != "hdnUpdateButton")
                //            closePopup = true;

                //        //set cookie to refresh parent page when he saved detail. which force parent to refresh.
                //        var refreshParentID = uHelper.GetCookieValue(Request, "framePopupID");
                //        if (!string.IsNullOrWhiteSpace(refreshParentID))
                //        {
                //            uHelper.CreateCookie(Response, "refreshParent", refreshParentID);
                //        }
                //        redirectAgain = true;
                //    }
                //}
                #endregion

                else if (valid && (senderID == "quickCloseTicket") && !(Request["batchCreate"] == "true"))
                {
                    TicketColumnValue resolution = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketResolutionComments);
                    if (resolution == null)
                    {
                        resolution = new TicketColumnValue();
                        resolution.InternalFieldName = DatabaseObjects.Columns.TicketResolutionComments;
                        formValues.Add(resolution);
                    }
                    resolution.Value = txtareaResolutionDesc.Text.Trim();
                    TicketColumnValue resolutionHrs = formValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketActualHours);
                    if (resolutionHrs == null)
                    {
                        resolutionHrs = new TicketColumnValue();
                        resolutionHrs.InternalFieldName = DatabaseObjects.Columns.TicketActualHours;
                        formValues.Add(resolutionHrs);
                    }
                    resolutionHrs.Value = txtActualHrs.Text.Trim();
                    if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && !string.IsNullOrEmpty(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate])))
                        oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;
                    else
                        oldTargetCompletionDate = DateTime.Now.Date;

                    TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, HttpContext.Current.CurrentUser().Id);

                    #region QuickClose
                    TicketRequest.QuickClose(moduleId, saveTicket, senderID);
                    #endregion

                    string error = TicketRequest.CommitChanges(saveTicket, HttpContext.Current.Request.Url.AbsolutePath);
                    if (string.IsNullOrWhiteSpace(error))
                        closePopup = true;

                }


                string newPriority = Convert.ToString(UGITUtility.GetSPItemValue(saveTicket, DatabaseObjects.Columns.TicketPriorityLookup));
                if (valid && Request["batchCreate"] != "true" && newPriority != null && !string.IsNullOrWhiteSpace(newPriority) && (oldPriority == null || oldPriority != newPriority))
                {
                    string oldPtr = string.Empty;
                    if (!string.IsNullOrEmpty(oldPriority))
                        oldPtr = oldPriority;
                    TicketRequest.NotifyOnElevateTicket(saveTicket, newPriority, oldPtr);
                }

                //Log information that update operation is performed on ticket
                Util.Log.ULog.WriteLog(context.TenantAccountId + "|" + HttpContext.Current.CurrentUser()?.Name + ": Saved Record: " + currentTicketPublicID);
            }
            #region validate ticket for batch editing
            if (senderID == "updateButton" && Request["BatchEditing"] == "true" && spListitemCollection != null)
            {
                foreach (DataRow batchEditItem in spListitemCollection)
                {
                    TicketRequest.Validate(formValues, batchEditItem, errors, true, true, 1);
                    if (errors.Count > 0)
                    {
                        valid = false;
                        break;
                    }
                }
            }
            #endregion
            #region save the data of batch editing
            if (valid && senderID == "updateButton" && Request["BatchEditing"] == "true" && spListitemCollection != null)
            {
                foreach (DataRow batchRow in spListitemCollection)
                {
                    saveTicket = batchRow;
                    actionUserChanged = IsActionUserChanged(saveTicket, formValues);


                    if (UGITUtility.IsSPItemExist(saveTicket, DatabaseObjects.Columns.TicketTargetCompletionDate))
                        oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                    TicketRequest.SetItemValues(saveTicket, formValues, true, true, user.Id, true);
                    string error = TicketRequest.CommitChanges(saveTicket, senderID, Request.Url, donotUpdateEscalations: actionUserChanged);
                    if (!string.IsNullOrEmpty(error))
                    {
                        errors.Add(TicketColumnError.AddError(error));
                        valid = false;
                    }

                    if (valid)
                    {
                        if (actionUserChanged)
                        {
                            TicketRequest.CheckRequestType(saveTicket, false);
                            // TicketRequest.CheckRequestType(saveTicket, uGITCache.GetModuleDetails(currentModuleName), false);
                            string actionUserTypes = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]);
                            if (!string.IsNullOrEmpty(actionUserTypes) && actionUserTypes.IndexOf(Constants.Separator10) != -1)
                                actionUserTypes = actionUserTypes.Replace(Constants.Separator10, Constants.Separator);

                            saveTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context, actionUserTypes, saveTicket);
                            TicketRequest.CommitChanges(saveTicket, senderID, Request.Url);
                            //if (objConfigurationVariableHelper.GetConfigVariableValueAsBool(ConfigConstants.NotifyNewPRPORPOnly, SPContext.Current.Web))
                            //    TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleId, newPRPorORPUsers);
                            //else
                            //    TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, moduleId);

                        }

                        // Update the Schedule Action for Email if the module is DRQ.
                        if (currentModuleName == "DRQ")
                        {
                            // AgentJobHelper agent = new AgentJobHelper(thisWeb);
                            //agent.UpdateDRQScheduleActionEmail(saveTicket, moduleId);
                            //Ticket.UpdateEMailQueue(saveTicket,thisWeb);
                        }
                        else if (currentModuleName == "CMT")
                        {
                            //AgentJobHelper agentHelper = new AgentJobHelper(SPContext.Current.Web);
                            //agentHelper.UpdateCMTScheduleActionReminder(saveTicket, moduleId);
                        }

                        if (!string.IsNullOrEmpty(currentStage.SkipOnCondition) && FormulaBuilder.EvaluateFormulaExpression(context, currentStage.SkipOnCondition, saveTicket))
                        {
                            ApproveTicketRequest(errors, senderID);
                        }

                        //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(moduleId, saveTicket);//commented on 30-5-2017
                    }
                    // currentTicketId = saveTicket.ID;
                    // currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
                    // currentTicketIdHidden.Value = saveTicket.ID.ToString();
                    closePopup = true;
                    //TicketRequest.UpdateTicketCache(saveTicket, currentModuleName);
                }
                //BTS-22-000883: Save and Close should close the popup window after saving.
                if (!(senderValue == "updateButtonSC" || senderValue == "hdnUpdateButtonSC"))
                {
                    redirectAgain = true;
                }
            }
            #endregion
            #region Refresh the current ticket object and UI

            //// Hide the new ticket form and display the created ticket


            if (!valid)
            {
                List<TicketColumnError> mandatoryErrors = errors.Where(x => x.Type == ErrorType.Mandatory).ToList();
                foreach (TicketColumnError er in mandatoryErrors)
                {
                    TicketColumnValue colVal = formValues.FirstOrDefault(x => x.InternalFieldName == er.InternalFieldName);
                    //er.Message = string.Format("<a href='#'  onclick=\"showTab('li_','Tab_','tabPanelContainer_','{0}','6')\" >{1}</a>", colVal.TabNumber, er.DisplayName);
                    er.Message = string.Format("<a href='#' onClick='MndtryFldTbShow({1});' return false;> {0}</a>", er.DisplayName, colVal.TabNumber);

                }
                if (mandatoryErrors.Count > 0)
                {
                    errorMsgNew.Text = string.Format("Please enter the mandatory fields: {0}", string.Join(",", mandatoryErrors.Select(x => x.Message).ToArray()));
                }

                List<TicketColumnError> nonMandatoryErrors = errors.Where(x => x.Type != ErrorType.Mandatory).ToList();
                invalidErrorMsgNew.Text = string.Format("{0}", string.Join(",", nonMandatoryErrors.Select(x => x.Message).ToArray()));

                errorMsg.Text = errorMsgNew.Text;
                if (errorMsg.Text == string.Empty)
                {
                    //errorMsg.Visible = false;
                }

                invalidErrorMsg.Text = invalidErrorMsgNew.Text;
                //stageDescriptionContainer.Visible = false;
                ticketMsgContainer.Visible = true;
            }
            else
            {
                if (errorMsg.Text != "")
                {
                    errorMsgNew.Text = errorMsg.Text;
                }
                
                initialStageDescriptionLiteral.Visible = false;
            }

            #endregion
            #region Requestor Notification on Target Completion Date Change

            if (saveTicket != null)
            {
                if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                    newtargetcompletiondate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

                //if (ConfigurationVariable.GetValueAsBool(ConfigConstants.NotifyRequestorOnTargetDateChange))
                //{
                //    if (!notificationdontsend && valid && oldtargetcompletiondate.Date != newtargetcompletiondate.Date)
                //    {
                //        List<UserProfile> userprofile = new List<UserProfile>();
                //        if (saveTicket.Fields.ContainsField(DatabaseObjects.Columns.TicketRequestor))
                //        {
                //            SPFieldUserValueCollection userColl = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketRequestor]));
                //            if (userColl.Count > 0)
                //            {
                //                SPUser user = userColl[0].User;
                //                StringBuilder mailBody = new StringBuilder();
                //                mailBody = mailBody.Append(string.Format("The estimated completion date for this {0} has changed from {1} to {2} by {3}",
                //                                                        uHelper.moduleTypeName(currentModuleName),
                //                                                        oldtargetcompletiondate.ToString("MMM-dd-yyyy"),
                //                                                        newtargetcompletiondate.ToString("MMM-dd-yyyy"),
                //                                                        SPContext.Current.Web.CurrentUser.Name));
                //                string subject = string.Format("Estimated Completion Date Changed for {0} {1}",
                //                                                uHelper.moduleTypeName(currentModuleName),
                //                                                Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                //                TicketRequest.SendEmailToRequestor(saveTicket, subject, mailBody.ToString());
                //            }
                //        }
                //    }
                //}
            }

            #endregion
            ////Keep Popup Open if KeepTicketOpen is enable at module level
            if (valid && !string.IsNullOrWhiteSpace(currentModuleName) && (senderID != "superAdminEditButton" && senderID != "createButton" && senderID != "createCloseButton" && senderID != "saveAsDraftButton" && senderID != "ClosePMM"))
            {
                if ((TicketRequest.Module.KeepItemOpen || senderID == "updateButton") && bClose == false && senderID != "btnupdateButtonSC")
                {
                    closePopup = false;
                    redirectAgain = true;
                }
                else
                {
                    uHelper.ClosePopUpAndEndResponse(Context, true, true);
                }
            }
            if (closePopup && errors.Count == 0)
                uHelper.ClosePopUpAndEndResponse(Context, true, "/refreshpage/");


            if (Request["batchCreate"] == "true" && valid)
            {
                TicketRequest.UpdateTicketCache(currentTicket, currentModuleName);
                uHelper.ClosePopUpAndEndResponse(Context, true, currentModuleListPagePath);
            }

            if (IsItemConverted == true)
            {
                redirectAgain = false;
                uHelper.InformationPopup(Context, Server.UrlEncode(ItemConversionMessage[0]), ItemConversionMessage[1], false);
            }
            if (senderID == "UnHoldButton" || senderID == "HoldButton")
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            else if (redirectAgain)
            {
                UGITUtility.CreateCookie(Response, SELECTEDTABCOOKIE, hdnActiveTab.Value);
                Response.Redirect(Request.Url.ToString());
            }

            #endregion
        }

        private void NewCreateTicket(bool valid, string senderID, List<TicketColumnError> errors, List<TicketColumnValue> formValues, bool updateChangesInHistory, string userID, string performedActionName = "")
        {
            UGITUtility.DeleteCookie(Request, Response, "ticketDivision");

            if (saveTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTargetCompletionDate) && saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate] != DBNull.Value)
                oldTargetCompletionDate = Convert.ToDateTime(saveTicket[DatabaseObjects.Columns.TicketTargetCompletionDate]).Date;

            if (currentModuleName != ModuleNames.CMDB)
            {

                resetPasswordAgent = PasswordResetAgent(formValues);

                if (resetPasswordAgent.IsResetPasswordAgentActivated == false)
                {
                    Boolean AutoFillTicket = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoFillTicket);
                    if (AutoFillTicket)
                    {
                        AutoUpdateTicket(formValues);
                    }
                }

            }


            TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, updateChangesInHistory, userID);

            if (currentModuleName != ModuleNames.CMDB)
            {
                if (resetPasswordAgent.IsResetPasswordAgentActivated == true)
                {
                    if (resetPasswordAgent.IsRequestorIsGroup == true || resetPasswordAgent.IsInitiatorEqualRequestor == true)
                    {
                        restpasswordagentmessage = string.Empty;
                        if (resetPasswordAgent.IsInitiatorEqualRequestor == true)
                        {
                            restpasswordagentmessage = $"Initiator and Requestor cannot be same for Password Reset Agent.</br>Please select valid user as requestor.</br>Please use change password option, to reset your own password.";
                            ResetPasswordCloseMessage = $"New Password is not reset";
                        }
                        else
                        {
                            restpasswordagentmessage = $"Password Reset Agent does not work with user group as Requestor.</br>Please select valid user as requestor.";
                            ResetPasswordCloseMessage = $"New Password is not reset";
                        }
                        isShowResetPasswordErrorPopup = true;
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "<script>showResetPassWordValidation();</Script>", false);
                        return;
                        //saveTicket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(user, restpasswordagentmessage, saveTicket, DatabaseObjects.Columns.TicketComment, false);
                    }
                    else
                    {
                        ResetPasswordCloseMessage = $"New password was sent to {resetPasswordAgent.requestors}";
                    }

                }
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["OldTicketId"])))
            {
                if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.ProjectID))
                {
                    saveTicket[DatabaseObjects.Columns.ProjectID] = oldProjectId;
                }
            }

            if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.ERPJobID) && UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.ERPJobIDNC) && !string.IsNullOrEmpty(UGITUtility.ObjectToString(saveTicket[DatabaseObjects.Columns.ERPJobIDNC])))
            {
                int ERPJobIDNoofCharacters = UGITUtility.StringToInt(ConfigurationVariableManager.GetValue(ConfigConstants.ERPJobIDNoofCharacters));
                saveTicket[DatabaseObjects.Columns.ERPJobID] = Convert.ToString(saveTicket[DatabaseObjects.Columns.ERPJobIDNC]).Substring(0, ERPJobIDNoofCharacters);
            }
            uHelper.FillShortNameIfExists(saveTicket, context);
            
            TicketRequest.Create(saveTicket, user, PRP, resetPasswordAgent);

            if (hdnMode.Value != null && hdnMode.Value != "")
            {
                //LeadRanking leadRanking = new LeadRanking();
                LeadRankingManager leadRankingManager = new LeadRankingManager(context);
                List<LeadRanking> lstleadRanking = leadRankingManager.Load(x => x.TicketId == hdnMode.Value);
                foreach (LeadRanking lr in lstleadRanking)
                {
                    LeadRanking leadRanking = new LeadRanking();
                    leadRanking.Ranking = lr.Ranking;
                    leadRanking.RankingCriteria = lr.RankingCriteria;
                    leadRanking.Weight = lr.Weight;
                    leadRanking.WeightedScore = lr.WeightedScore;
                    leadRanking.Description = lr.Description;
                    leadRanking.TicketId = saveTicket[DatabaseObjects.Columns.TicketId].ToString();
                    leadRankingManager.Insert(leadRanking);
                }
            }

            string error = TicketRequest.CommitChanges(saveTicket, senderID, Request.Url, donotUpdateEscalations: true);
            if (hdnMode.Value != null && hdnMode.Value != "")
            {
                LeadRankingManager leadRankingManager = new LeadRankingManager(context);
                DataTable CurrentTicket = null;
                string tableName = DatabaseObjects.Tables.Opportunity;
                string query2 = string.Format("{0}='{1}' ",
                     DatabaseObjects.Columns.TicketId, saveTicket[DatabaseObjects.Columns.TicketId].ToString());

                CurrentTicket = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Opportunity, $"{query2} and TenantID='{context.TenantID}'"); //spList.GetItems(spQuery);
                tableName = DatabaseObjects.Tables.Opportunity;

                //Decimal Ranking = 0;
                //Decimal Weight = 0;
                Decimal WeightScore = 0;
                Decimal RCT = 0;
                if (CurrentTicket != null && CurrentTicket.Rows.Count > 0)
                {
                    long DbTicketId = Convert.ToInt64(CurrentTicket.Rows[0]["ID"].ToString());

                    List<LeadRanking> lstleadRanking = leadRankingManager.Load(x => x.TicketId == hdnMode.Value);
                    if (lstleadRanking != null && lstleadRanking.Count != 0)
                    {

                        foreach (LeadRanking LR in lstleadRanking)
                        {

                            WeightScore = LR.WeightedScore + WeightScore;
                        }
                        RCT = (Math.Round((WeightScore / lstleadRanking.Count), 1));

                    }
                    Dictionary<String, object> values = new Dictionary<string, object>();
                    values.Add(DatabaseObjects.Columns.RankingCriteriaTotal, RCT.ToString());

                    int success = (int)GetTableDataManager.UpdateItem<int>(tableName, Convert.ToInt64(DbTicketId), values);
                    hdnMode.Value = "";
                }
            }

            PasswordResetMail(resetPasswordUserList, saveTicket[DatabaseObjects.Columns.TicketId].ToString());
            error = TicketRequest.CommitChanges(saveTicket, senderID, Request.Url, donotUpdateEscalations: true, agentSummary: agentSummary);
            #region Dependent Code
            #region QuickClose and close
            TicketRequest.QuickClose(moduleId, saveTicket, senderID, performedActionName: performedActionName);

            if (resetPasswordAgent != null)
            {
                if (resetPasswordAgent.IsResetPasswordAgentActivated)
                {

                    TicketRequest.CloseTicket(saveTicket, uHelper.GetCommentString(user, ResetPasswordCloseMessage, saveTicket, DatabaseObjects.Columns.TicketResolutionComments, false));

                }
            }
            #endregion
            currentTicketId = UGITUtility.StringToInt(Convert.ToString(saveTicket["ID"]));
            currentTicketPublicID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);
            //saveTicket = SPListHelper.GetSPListItem(thisList, currentTicketId);
            currentTicketIdHidden.Value = Convert.ToString(saveTicket["ID"]); //saveTicket.ID.ToString();
            currentTicket = saveTicket;
            #endregion
            //ModuleWorkflowHistory moduleWorkflowHistory = new ModuleWorkflowHistory();

            error = TicketRequest.CommitChanges(saveTicket, senderID, Request.Url, agentSummary: agentSummary);

            if (!string.IsNullOrEmpty(error))
            {
                errors.Add(TicketColumnError.AddError(error));
                valid = false;
            }

            if (!string.IsNullOrEmpty(PRP) && !saveTicket.IsNull(DatabaseObjects.Columns.TicketRequestTypeLookup) && currentModuleName != ModuleNames.CMDB)
            {
                isAddREsolutionTime = true;

                ApproveTicketRequest(errors, "approvebuttonhidden", true);
            }


            if (valid)
            {
                panelNewTicket.Style.Add("display", "block");
                panelDetail.Style.Add("display", "none");
                string moduleName = Convert.ToString(currentModuleName);
                if (userID != null)
                {
                    #region Show popup with new ticket ID
                    //keep ticket open when keepticketopen is enable for module
                    if (TicketRequest.Module.KeepItemOpen && senderID == "saveAsDraftButton")
                    {
                        UGITUtility.CreateCookie(Response, currentModuleName + "-" + SELECTEDTABCOOKIE, hdnActiveTab.Value);
                        var qs = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        qs.Set("TicketId", Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                        string url = Request.Url.AbsoluteUri.Split('?')[0];
                        Response.Redirect(string.Format("{0}?{1}", url, qs.ToString()));
                    }
                    else
                    {
                        //if (!TicketRequest.Module.DisableNewConfirmation &&  /*!UGITUtility.StringToBoolean(Request["hpac"])&&*/ !TicketRequest.IsQuickClose(currentTicket))
                        if (!TicketRequest.Module.DisableNewConfirmation && !UGITUtility.StringToBoolean(Request["hpac"]) && !TicketRequest.IsQuickClose(currentTicket))
                        {
                            string ticketID = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);

                            if (moduleName == "CPR" || moduleName == "CNS")
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(saveTicket[DatabaseObjects.Columns.EstimateNo])))
                                    ticketID = Convert.ToString(saveTicket[DatabaseObjects.Columns.EstimateNo]);
                            }

                            // var dataExist = GetTableDataManager.IsLookaheadTicketExists(moduleName, TenantID, Convert.ToString(saveTicket[DatabaseObjects.Columns.Title]), Convert.ToString(saveTicket[DatabaseObjects.Columns.ID]), Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketRequestTypeLookup]));
                            var list = (List<string>)Session["relatedTicket"];
                            if (list != null && list.Count > 0)
                            {
                                foreach (string val in list)
                                {
                                    TicketRelationshipHelper tRelation = new TicketRelationshipHelper(context, Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]), val);
                                    int rowEffected = tRelation.CreateRelation(context);
                                }
                            }

                            string SuperAdminId = string.Empty;
                            if (Session["isFromSuprAdmin"] != null)
                            {
                                SuperAdminId = Session["isFromSuprAdmin"].ToString();
                            }
                            Session.Clear();
                            Session["isFromSuprAdmin"] = SuperAdminId;

                            string newModuleTicketTitle = string.Format("{0}: {1}", saveTicket[DatabaseObjects.Columns.TicketId], UGITUtility.ReplaceInvalidCharsInURL(UGITUtility.TruncateWithEllipsis(Convert.ToString(saveTicket[DatabaseObjects.Columns.Title]), 100, string.Empty)));
                            string newModuleTicketLink = string.Format("<a href=\"javascript:\" id='myAnchor' onclick=\"PopupCloseBeforeOpenUgit('{1}','ticketId={0}','{2}', 90, 90, 0, '{3}' );  \">{0}</a>", saveTicket[DatabaseObjects.Columns.TicketId], currentModuleListPagePath, newModuleTicketTitle, Request["source"]);
                            string typeName = UGITUtility.newTicketTitle(moduleName);
                            string informationMsg = string.Format("<div class='informationMsg'><div class='infoMsgSuccess-title'>{0} Created: <div class='cteatedTicketNum'>{1}</div></div></div>",
                                                                    typeName, newModuleTicketLink);
                            string header = string.Format("{0} Created", typeName);
                            TicketRequest.UpdateTicketCache(currentTicket, currentModuleName);
                            uHelper.InformationPopup(Context, Server.UrlEncode(informationMsg), header, false);
                        }
                        else
                        {
                            uHelper.ClosePopUpAndEndResponse(Context);
                        }
                    }
                    #endregion
                }
                #region RelatedTicket
                if (!UGITUtility.StringToBoolean(Request["OnlyCopy"]))
                {
                    // If the ticket is creating from related ticket page.
                    string realtedTicketId = HttpContext.Current.Request["ParentId"];
                    if (!string.IsNullOrEmpty(realtedTicketId))
                    {
                        TicketRelationshipHelper tRelation = new TicketRelationshipHelper(context, realtedTicketId, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
                        int rowEffected = tRelation.CreateRelation(context);
                    }
                    // Used for Ticket if it has the Related ticket id as parent ticket id in its New Ticket Form (Like In DRQ Module).
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.RelatedRequestID, currentTicket.Table) && Convert.ToString(currentTicket[DatabaseObjects.Columns.RelatedRequestID]) != "N/A" && Convert.ToString(currentTicket[DatabaseObjects.Columns.RelatedRequestID]) != string.Empty)
                    {
                        TicketRelationshipHelper t1 = new TicketRelationshipHelper(context, Convert.ToString(currentTicket[DatabaseObjects.Columns.RelatedRequestID]), Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]));
                        t1.CreateRelation(context);
                    }
                }
                #endregion

                #region add exit criteria tasks from templates
                //UGITModuleConstraint.CreateModuleStageTasksInTicket(context, currentTicketPublicID, moduleName);
                //UGITModuleConstraint.ConfigureCurrentModuleStageTask(context, saveTicket);
                #endregion

                #region Close LEM or OPM

                bool closeLeadWhileCreatingOpportunity = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.CloseLeadWhileCreatingOpportunity);
                bool fromLead = false;
                DataRow spItemLEMOROPM = null;
                if (Request.QueryString["LeadId"] != null)
                {
                    fromLead = true;
                    long leadId = Convert.ToInt64(Request.QueryString["LeadId"]);
                    spItemLEMOROPM = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Lead, $" {DatabaseObjects.Columns.ID} = {leadId}").Select()[0];
                }
                else if (Request.QueryString["LeadTicketId"] != null)
                {
                    fromLead = true;
                    spItemLEMOROPM = Ticket.GetCurrentTicket(context, "LEM", Convert.ToString(Request.QueryString["LeadTicketId"]));
                }
                else if (Request.QueryString["OpportunityId"] != null)
                {
                    long opmId = Convert.ToInt64(Request.QueryString["OpportunityId"]);
                    spItemLEMOROPM = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Opportunity, $" {DatabaseObjects.Columns.ID} = {opmId}").Select()[0];
                }
                else if (Request.QueryString["OpportunityTicketId"] != null)
                {
                    spItemLEMOROPM = Ticket.GetCurrentTicket(context, "OPM", Convert.ToString(Request.QueryString["OpportunityTicketId"]));
                }

                if (spItemLEMOROPM != null)
                {
                    if (!fromLead || (closeLeadWhileCreatingOpportunity && fromLead))
                    {
                        string module = uHelper.getModuleNameByTicketId(Convert.ToString(spItemLEMOROPM[DatabaseObjects.Columns.TicketId]));
                        Ticket baseTicket = new Ticket(context, module);
                        uHelper.CreateHistory(context.CurrentUser, string.Format("Converted to item {0}", saveTicket[DatabaseObjects.Columns.TicketId]), spItemLEMOROPM, context);
                        baseTicket.CloseTicket(spItemLEMOROPM, string.Empty);

                        uHelper.CreateHistory(context.CurrentUser, string.Format("Converted from original item {0}", spItemLEMOROPM[DatabaseObjects.Columns.TicketId]), saveTicket, context);
                        TicketRequest.CommitChanges(saveTicket);
                        baseTicket.CommitChanges(spItemLEMOROPM);
                        baseTicket.UpdateTicketCache(spItemLEMOROPM, module);

                        CopyProjectTeamFromOPMtoCPR(Convert.ToString(spItemLEMOROPM[DatabaseObjects.Columns.TicketId]), Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                        CopyExternalTeamFromOPMtoCPR(Convert.ToString(spItemLEMOROPM[DatabaseObjects.Columns.TicketId]), Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));

                        // Copy Documents from OPM to CPR
                        var repositoryService = new uGovernIT.DMS.Amazon.DMSManagerService(context);
                        repositoryService.CopyDMSDirectory(Convert.ToString(spItemLEMOROPM[DatabaseObjects.Columns.TicketId]), Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
                    }

                    if (Request.QueryString["CompleteTasks"] != null && Convert.ToBoolean(Request.QueryString["CompleteTasks"]) == true)
                    {
                        CompleteTasksOnItemClose(Convert.ToString(spItemLEMOROPM[DatabaseObjects.Columns.TicketId]));
                    }
                }
                #endregion
            }

            //new line of code for change ticket type....
            ArchiveOldTicket();
            //return valid;
            TicketRequest.UpdateTicketCache(currentTicket, currentModuleName);
        }

        private string AutoSetResolutionTime(string currentModuleName, string Title)
        {
            List<double> lstOfResolutiontime = new List<double>();
            string strfinalResolutionTime = string.Empty;
            DataTable FilteredTicketOnTitle = GetTableDataManager.autoSetResolutionTime(currentModuleName, TenantID, Title);

            if (FilteredTicketOnTitle != null && FilteredTicketOnTitle.Rows.Count != 0)
            {
                foreach (DataRow dr in FilteredTicketOnTitle.Rows)
                {
                    DateTime ActualCompletionDate = Convert.ToDateTime(dr[DatabaseObjects.Columns.CloseDate].ToString());
                    DateTime startDate = Convert.ToDateTime(dr[DatabaseObjects.Columns.Created].ToString());
                    UGITUtility.GetValueBar(startDate, ActualCompletionDate);
                    var result = uHelper.GetTotalWorkingDayTimeMinuteBetween(context, startDate, ActualCompletionDate);
                    if (result != 0)
                    {
                        lstOfResolutiontime.Add(result);
                    }


                }
                if (lstOfResolutiontime != null)
                {
                    if (lstOfResolutiontime.Count != 0)
                    {
                        double minResolutionTime = lstOfResolutiontime.Min();
                        int hour = Convert.ToInt32(minResolutionTime) / 60;
                        int minute = Convert.ToInt32(minResolutionTime) % 60;
                        int days = Convert.ToInt32(hour) / 8;
                        int dayhour = Convert.ToInt32(hour) % 8;
                        if (days >= 1)
                        {
                            strfinalResolutionTime = $"{days} days, {dayhour} hr : {minute} min";
                        }
                        else if (hour >= 1)
                        {
                            strfinalResolutionTime = $"{hour} hr : {minute} min";
                        }
                        else
                        {
                            strfinalResolutionTime = $"{minute} min";
                        }
                    }
                }



            }

            return strfinalResolutionTime;
        }

        private string AutoUpdateTicket(List<TicketColumnValue> formValues)
        {
            var title = string.Empty;
            var ticketId = string.Empty;
            var ticketRequestTypeLookup = "";
            string strRequestTypeLookUp = string.Empty;
            //Boolean strLookaheadTicketExists = true;
            ModuleRequestType requestTypeObj = null;
            UGITModule moduleObj = ObjModuleViewManager.GetByName(currentModuleName);

            if (formValues != null && formValues.Count > 0)
            {
                foreach (var item in formValues)
                {
                    if (item.InternalFieldName == DatabaseObjects.Columns.Title)
                        title = Convert.ToString(item.Value);
                    if (item.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup)
                        ticketRequestTypeLookup = Convert.ToString(item.Value);



                }
            }

            if (moduleObj.EnableLinkSimilarTickets.HasValue && moduleObj.EnableLinkSimilarTickets == true)
            {
                if (GetTableDataManager.IsLookaheadTicketExists(moduleObj.ModuleTable, TenantID, title, ""))
                {
                    strResolutionTime = AutoSetResolutionTime(currentModuleName, title);
                    var RequestTypeTable = GetTableDataManager.autoSetRequestType(currentModuleName, TenantID, title);

                    if (RequestTypeTable != null && RequestTypeTable.Rows.Count != 0)
                    {
                        int RequestTypeMax = Convert.ToInt32(RequestTypeTable.Compute("max([co])", string.Empty));
                        RequestTypeTable = RequestTypeTable.Select($"co = {RequestTypeMax}").CopyToDataTable();
                    }
                    if (RequestTypeTable != null && RequestTypeTable.Rows.Count != 0)
                    {
                        strRequestTypeLookUp = RequestTypeTable.Rows[0][DatabaseObjects.Columns.TicketRequestTypeLookup].ToString();
                        RequestTypeManager requestTypeManager = new RequestTypeManager(context);
                        requestTypeObj = requestTypeManager.LoadByID(Convert.ToInt64(strRequestTypeLookUp));
                    }

                    var PRPGroupOpen = GetTableDataManager.autoSetPRP(currentModuleName, TenantID, title, DatabaseObjects.Columns.TicketPRP, 0);
                    var PRPGroupClosed = GetTableDataManager.autoSetPRP(currentModuleName, TenantID, title, DatabaseObjects.Columns.TicketPRP, 1);

                    PRP = AvailablePRPAndAssignTo.GetPRPOrAssignTo(PRPGroupOpen, PRPGroupClosed, DatabaseObjects.Columns.TicketPRP);

                    if (!string.IsNullOrEmpty(PRP) || (!string.IsNullOrEmpty(strRequestTypeLookUp)))
                    {
                        agentSummary = new AgentSummary()
                        {
                            AgentName = "Auto Assign",
                            AgentType = "AutoAssign",
                            ModuleName = currentModuleName,
                            IsAgentActivated = true,
                            //StageSteps = "1;4;5;"
                        };
                    }

                    //if (PRPGroupClosed != null && PRPGroupClosed.Rows.Count != 0)
                    //{
                    //    int PRPGroupClosedMax = Convert.ToInt32(PRPGroupClosed.Compute("max([co])", string.Empty));
                    //    PRPGroupClosed = PRPGroupClosed.Select($"co = {PRPGroupClosedMax}").CopyToDataTable();
                    //    if (PRPGroupClosed.Rows.Count == 1)
                    //    {
                    //        PRP = PRPGroupClosed.Rows[0]["PRP"].ToString();
                    //    }
                    //    else
                    //    {
                    //        foreach (DataRow dr in PRPGroupClosed.Rows)
                    //        {
                    //            if (dr["PRP"] != null)
                    //            {
                    //                DataTable matchingPRP = new DataTable();
                    //                var matchingPRP1 = PRPGroupOpen.Select($"PRP = '{dr["PRP"]}'");
                    //                if (matchingPRP1 != null && matchingPRP1.Count() > 0)
                    //                {
                    //                    NewMatch.Merge(matchingPRP1.CopyToDataTable());
                    //                }
                    //                else
                    //                {
                    //                    NotMatch = PRPGroupClosed.NewRow();
                    //                    NotMatch = dr;
                    //                }

                    //            }


                    //        }
                    //        if (NotMatch != null)
                    //        {
                    //            PRP = NotMatch["PRP"].ToString();
                    //        }
                    //        else
                    //        {
                    //            int PRPGroupOpendMin = Convert.ToInt32(NewMatch.Compute("min([co])", string.Empty));
                    //            PRPGroupOpen = PRPGroupOpen.Select($"co = {PRPGroupOpendMin}").CopyToDataTable();
                    //            if (PRPGroupOpen != null)
                    //            {
                    //                PRP = PRPGroupOpen.Rows[0]["PRP"].ToString();
                    //            }

                    //        }


                    //    }

                    //}
                    //else if (PRPGroupOpen != null && PRPGroupOpen.Rows.Count != 0)
                    //{
                    //    int PRPGroupOpendMin = Convert.ToInt32(PRPGroupOpen.Compute("min([co])", string.Empty));
                    //    PRPGroupOpen = PRPGroupOpen.Select($"co = {PRPGroupOpendMin}").CopyToDataTable();
                    //    PRP = PRPGroupOpen.Rows[0]["PRP"].ToString();
                    //}
                    //else
                    //{
                    //    PRP = string.Empty;
                    //}

                    foreach (TicketColumnValue formVal in formValues)
                    {
                        if (formVal.InternalFieldName == DatabaseObjects.Columns.TicketPRP)
                        {
                            formVal.Value = PRP;
                        }
                        if (formVal.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup)
                        {
                            if (formVal.Value == null || formVal.Value.ToString() == String.Empty || formVal.Value.ToString() == "")
                            {
                                formVal.Value = strRequestTypeLookUp;
                            }
                            else
                            {

                            }

                        }

                    }
                }
            }
            return "";
        }


        private ResetPasswordAgent PasswordResetAgent(List<TicketColumnValue> formValues)
        {
            ResetPasswordAgent resetPasswordAgent = new ResetPasswordAgent();
            string requestorList = string.Empty;
            bool IsResetPasswordAgentActivated = false;
            UserProfile user = new UserProfile();
            String ticketRequestor = String.Empty;
            String Initiator = String.Empty;
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            IdentityResult result;
            UserProfileManager umanager;
            umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
            var title = string.Empty;
            Role role = new Role();

            if (formValues != null && formValues.Count > 0)
            {
                foreach (var item in formValues)
                {
                    if (item.InternalFieldName == DatabaseObjects.Columns.Title)
                        title = Convert.ToString(item.Value);
                    if (item.InternalFieldName == DatabaseObjects.Columns.TicketRequestor)
                        ticketRequestor = Convert.ToString(item.Value);
                    if (item.InternalFieldName == DatabaseObjects.Columns.TicketInitiator)
                        Initiator = Convert.ToString(item.Value);

                }
            }

            user = HttpContext.Current.CurrentUser();
            if (user != null)
            {
                Initiator = user.Id;
            }

            var RequestorList = ticketRequestor.Split(',');

            IsResetPasswordAgentActivated = GetTableDataManager.IsResetPasswordExists(TenantID, title);
            UserRoleManager userRoleManager = new UserRoleManager(context);

            if (RequestorList.Length == 1)
            {
                if (ticketRequestor == Initiator)
                {
                    resetPasswordAgent.IsInitiatorEqualRequestor = true;
                }
                role = userRoleManager.Load(x => x.Id == ticketRequestor).SingleOrDefault();
                if (role != null)
                {
                    resetPasswordAgent.IsRequestorIsGroup = true;
                }
            }

            if (IsResetPasswordAgentActivated == true)
            {
                agentSummary = new AgentSummary()
                {
                    AgentName = "Reset Password",
                    AgentType = "ResetPassword",
                    ModuleName = currentModuleName,
                    IsAgentActivated = true
                };
            }

            if (IsResetPasswordAgentActivated && resetPasswordAgent.IsRequestorIsGroup != true && resetPasswordAgent.IsInitiatorEqualRequestor != true)
            {
                resetPasswordUserList.Columns.Add("Username", typeof(string));
                resetPasswordUserList.Columns.Add("Password", typeof(string));
                resetPasswordUserList.Columns.Add("UserMail", typeof(string));
                resetPasswordUserList.Columns.Add("name", typeof(string));
                resetPasswordUserList.Columns.Add("AccountId", typeof(string));

                foreach (string requestor in RequestorList)
                {
                    role = userRoleManager.Load(x => x.Id == requestor).SingleOrDefault();
                    if (role == null && requestor != Initiator)
                    {
                        user = umanager.GetUserById(requestor);
                        string newPassword = umanager.GeneratePassword();

                        string passwordToken = umanager.GeneratePasswordResetToken(user.Id.Trim());
                        result = umanager.ResetPassword(user.Id.Trim(), passwordToken, newPassword);

                        if (result.Succeeded)
                        {


                        }

                        DataRow dr = resetPasswordUserList.NewRow();
                        dr["Username"] = user.UserName;
                        dr["name"] = user.Name;
                        dr["Password"] = newPassword;
                        dr["UserMail"] = user.Email;
                        dr["AccountId"] = context.TenantAccountId;
                        resetPasswordUserList.Rows.Add(dr);
                        requestorList += user.Name + ',';
                    }

                }

                //setting PRP as Admin
                role = userRoleManager.Load(x => x.Name.ToLower() == "Admin".ToLower()).SingleOrDefault();
                if (role != null)
                {
                    PRP = role.Id;
                    PRP = "ResetPassword";
                }

            }
            resetPasswordAgent.IsResetPasswordAgentActivated = IsResetPasswordAgentActivated;
            resetPasswordAgent.requestors = requestorList;

            return resetPasswordAgent;
        }

        private void PasswordResetMail(DataTable mailUserList, string ticketId)
        {
            if (mailUserList != null)
            {
                foreach (DataRow dr in mailUserList.Rows)
                {

                    string UserName = dr["Username"].ToString();
                    string password = dr["Password"].ToString();
                    string accountId = dr["AccountId"].ToString();

                    string accountIdForMail = uGovernITCrypto.Encrypt(accountId, "ugitpass");
                    string userNameForMail = uGovernITCrypto.Encrypt(UserName, "ugitpass");
                    string passwordForMail = uGovernITCrypto.Encrypt(password, "ugitpass");
                    string SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/TenantLogin?id={accountIdForMail}&acc={userNameForMail}&di={passwordForMail}";


                    var sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append("<head></head>");
                    sb.Append("<body>");

                    sb.Append("<p>");
                    sb.Append("Welcome to Service Prime!");
                    sb.Append("<br>");
                    sb.Append("<br>");
                    sb.Append($"Hello {dr["name"].ToString()}");
                    sb.Append("<br>");
                    sb.Append("<strong>");
                    sb.AppendFormat("Service Prime Ticket : {0} created and your password has been reset", ticketId);
                    sb.Append("</strong>");
                    sb.Append("<br>");
                    sb.AppendFormat("Please find details below:");
                    sb.Append("<br>");
                    sb.AppendFormat("Username: {0}", UserName);
                    sb.Append("<br>");
                    sb.AppendFormat("Password: {0}", password);
                    sb.Append("<br>");
                    sb.Append("<br>");
                    sb.AppendFormat("Please <a href={0}> click here  </a> to  login with new password.", SiteUrl);

                    sb.Append("<br>");
                    sb.Append("<br>");


                    sb.Append("Thanks & Regards");
                    sb.Append("<br>");
                    sb.Append("HelpDesk Team");
                    sb.Append("<br>");
                    sb.Append("Service Prime");
                    sb.Append("<br>");

                    sb.Append("</p>");
                    sb.Append("</body>");
                    sb.Append("</html>");
                    String subject = "Your password is successfully reset!";
                    var mail = new MailMessenger(HttpContext.Current.GetManagerContext());
                    var response = mail.SendMail(dr["UserMail"].ToString(), subject, "", sb.ToString(), true);


                }
            }

        }





        private void ArchiveOldTicket()
        {
            #region archiveOldticket
            if (Request.QueryString["OldTicketId"] != null)
            {
                string oldTicketId = Request.QueryString["OldTicketId"];
                string history = string.Empty;

                DataTable spListModuleWorkFlowArchive = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleWorkflowHistoryArchive, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                string moduleName = uHelper.getModuleNameByTicketId(oldTicketId);
                DataRow spItem = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(oldTicketId), oldTicketId);

                if (spItem != null)
                {
                    //Copying in ModuleWorkflowHistoryArchive list and deleteing from ModuleWorkflowHistory list
                    //SPQuery query = new SPQuery();
                    //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketId, oldTicketId);
                    string query = string.Format("{0} ='{1}'", DatabaseObjects.Columns.TicketId, oldTicketId);
                    DataRow[] moduleWorkFlowItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleWorkflowHistory, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(query);
                    if (moduleWorkFlowItemColl != null && moduleWorkFlowItemColl.Count() > 0)
                    {
                        int cnt = moduleWorkFlowItemColl.Count();
                        DataTable dataTablemoduleWFlow = moduleWorkFlowItemColl.CopyToDataTable();
                        while (dataTablemoduleWFlow.Rows.Count > 0)
                        {
                            DataRow moduleWorkItem = moduleWorkFlowItemColl[0];
                            if (spListModuleWorkFlowArchive != null)
                            {
                                DataRow moduleWorkFlowHistoryArchiveItem = spListModuleWorkFlowArchive.NewRow();
                                CopySpItem(moduleWorkItem, moduleWorkFlowHistoryArchiveItem);
                            }
                            TicketRequest.Recycle(moduleWorkItem);
                            dataTablemoduleWFlow.Rows.RemoveAt(0);
                        }
                        history = string.Format("Deleted {0} items from  WorkflowHistory List.", cnt);
                    }

                    //Deleting from ModuleUserStatistics list
                    DataRow[] moduleUserStatsColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(query);
                    if (moduleUserStatsColl != null && moduleUserStatsColl.Count() > 0)
                    {
                        history = string.Format(" {0} Deleted following items from  ModuleUserStatistics List:", history);
                        DataTable filtermoduleStats = moduleUserStatsColl.CopyToDataTable();
                        while (filtermoduleStats.Rows.Count > 0)
                        {
                            DataRow moduleStats = moduleUserStatsColl[0];
                            history = string.Format("{0} UserRole {1}- TicketUser {2},", history, moduleStats[DatabaseObjects.Columns.UserRole], moduleStats[DatabaseObjects.Columns.TicketUser]);
                            TicketRequest.Recycle(moduleStats);
                            filtermoduleStats.Rows.RemoveAt(0);
                        }
                        history = history.Substring(0, history.Length - 1);
                        history = string.Format("{0}.", history);
                    }

                    //Deleting from TicketRelationship list
                    //SPQuery queryTicketRelation = new SPQuery();
                    //queryTicketRelation.Query = string.Format("<Where><Or><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></Or></Where>", DatabaseObjects.Columns.ParentTicketId, oldTicketId, DatabaseObjects.Columns.ChildTicketId, oldTicketId);
                    string queryTicketRelation = string.Format("{0}='{1}' or {2}='{3}'", DatabaseObjects.Columns.ParentTicketId, oldTicketId, DatabaseObjects.Columns.ChildTicketId, oldTicketId);
                    DataRow[] ticketRelationshipColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketRelation, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select(queryTicketRelation);
                    if (ticketRelationshipColl != null && ticketRelationshipColl.Count() > 0)
                    {
                        DataTable dataTableRelationship = ticketRelationshipColl.CopyToDataTable();
                        history = string.Format(" {0} Deleted following items from  TicketRelationship List:", history);
                        while (dataTableRelationship.Rows.Count > 0)
                        {
                            DataRow ticketRelationItem = ticketRelationshipColl[0];
                            history = string.Format("{0} ParentTicketId {1}- ChildTicketId {2},", history, ticketRelationItem[DatabaseObjects.Columns.ParentTicketId], ticketRelationItem[DatabaseObjects.Columns.ChildTicketId]);
                            TicketRequest.Recycle(ticketRelationItem);
                            TicketRequest.ArchiveTickets(spItem);
                            //dataTableRelationship.Rows.RemoveAt(0);
                        }
                        history = history.Substring(0, history.Length - 1);
                        history = string.Format("{0}.", history);
                    }

                    //Copying in Ticket archive list and deleting from ticket list
                    // NOTE: Delete does not happen UNLESS there is an archive table!
                    string module = uHelper.getModuleNameByTicketId(Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]));
                    UGITModule ticketModule = ModuleViewManager.GetByName(module);
                    string listName = string.Format("{0}_Archive", ticketModule.ModuleTable);
                    DataTable spList = GetTableDataManager.GetTableData(listName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                    if (spList != null)
                    {
                        // Copy to archive
                        DataRow archiveListItem = spList.NewRow();
                        TicketRelationshipHelper trHelper = new TicketRelationshipHelper(context);
                        history = string.Format(" {0} Deleted Ticket => Id: {1}, Title: {2} from List: {3}.", history, spItem[DatabaseObjects.Columns.TicketId], spItem[DatabaseObjects.Columns.Title], trHelper.GetParentTickets(uHelper.getModuleNameByTicketId(Convert.ToString(spItem[DatabaseObjects.Columns.TicketId])), Convert.ToString(spItem[DatabaseObjects.Columns.TicketId])));
                        archiveListItem[DatabaseObjects.Columns.History] = string.Format("{0} {1}", spItem[DatabaseObjects.Columns.History], history);
                        CopySpItem(spItem, archiveListItem);
                        // Delete original
                        TicketRequest.Recycle(spItem);
                        TicketRequest.ArchiveTickets(spItem);
                    }
                }
                int moduleID = uHelper.getModuleIdByModuleName(context, moduleName);


            }
            #endregion
        }

        private void CopySpItem(DataRow sourceItem, DataRow destinationItem)
        {
            foreach (DataColumn field in sourceItem.Table.Columns)
            {
                if (field.ColumnName != "Attachments" && field.ColumnName != "History")
                {
                    if (destinationItem.Table.Columns.Contains(field.ColumnName))
                    {
                        destinationItem[field.ColumnName] = sourceItem[field.ColumnName];
                    }
                }
            }
        }

        /// <summary>
        /// create funtion for use in approve and update button.
        /// </summary>
        /// <param name="thisWeb"></param>
        /// <returns></returns>
        /// 
        private bool taskValidation(LifeCycleStage oldCurrentStage)
        {
            //LifeCycleStage oldCurrentStage = TicketRequest.GetTicketCurrentStage(saveTicket);
            UGITTaskManager taskManager = new UGITTaskManager(context);
            List<UGITTask> depncies = taskManager.LoadByProjectID(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
            List<UGITTask> currentStageTask = depncies.Where(x => x.StageStep == oldCurrentStage.StageStep && x.Status != "Completed").ToList();
            if (currentStageTask.Count > 0)
            {
                return false;
            }
            return true;
        }



        private void ApproveTicketRequest(List<TicketColumnError> errors, string senderId, bool checkChildTickets = false, string actionPerformed = "")
        {
            LifeCycleStage oldCurrentStage = TicketRequest.GetTicketCurrentStage(saveTicket);

            string error = TicketRequest.Approve(user, saveTicket, adminOverride);

            if (!taskValidation(oldCurrentStage))
            {
                error = "Complete all linked tasks of current stage";
            }
            LifeCycleStage newCurrentStage = TicketRequest.GetTicketCurrentStage(saveTicket);
            if (string.IsNullOrEmpty(error) && checkChildTickets && newCurrentStage.StageTypeChoice == Convert.ToString(StageType.Closed) && ConfigurationVariableManager.GetValueAsBool(ConfigConstants.AutoCloseChildTickets))
            {
                TicketRelationshipHelper tHelper = new TicketRelationshipHelper(context);
                List<TicketRelation> ticketChilds = tHelper.GetTicketChildList(currentTicketPublicID);
                if (ticketChilds != null && ticketChilds.Count > 0)
                {
                    confirmChildTicketsClose = true;
                    error = "closechildtickets";
                }
            }

            if (string.IsNullOrEmpty(error))
            {
                TicketRequest.AssignModuleSpecificDefaults(saveTicket);
                error = TicketRequest.CommitChanges(saveTicket, senderId, Request.Url, agentSummary: agentSummary);

                // Update Ticket.
                if (string.IsNullOrEmpty(error))
                {
                    //Send Email notification.
                    bool sendNotification = true;
                    if (newCurrentStage == oldCurrentStage && newCurrentStage.StageAllApprovalsRequired)
                        sendNotification = false;
                    if (sendNotification)
                        TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), saveTicket, UGITUtility.ObjectToString(moduleId), null, isAddREsolutionTime, strResolutionTime, performedAction: actionPerformed);

                    //This will ensure start date and end date are updated if they have previous dates from template
                    UGITModuleConstraint.ConfigureCurrentModuleStageTask(HttpContext.Current.GetManagerContext(), currentStage.StageStep + 1, currentTicketPublicID);
                }
                else
                {
                    errors.Add(TicketColumnError.AddError(error));
                }
            }
            else
            {
                errors.Add(TicketColumnError.AddError(error));
            }
        }

        protected void ClearFieldsForReturnStage(int currentStageId)
        {

        }

        /// <summary>
        /// Overrides the base class CreateChildControls function. All child controls to be binded here.
        /// </summary>
        protected override void CreateChildControls()
        {

        }

        /// <summary>
        /// Overriden Render function.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        /// <summary>
        /// Documents are attached here to the ticket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uploadBUtton_Click(object sender, EventArgs e)
        {
            #region uploadBUtton_Click
            //bool noError = true;
            //errorMsg.Visible = false;
            //List<string> uploadedFiles = new List<string>();
            //if (currentTicket == null)
            //{
            //    if (saveTicket == null)
            //        saveTicket = newTicket;
            //}
            //else
            //    saveTicket = currentTicket;

            //uploadedFiles = saveTicket.Attachments.Cast<string>().ToList();
            //attachments = saveTicket.Attachments;


            //if (attachment.PostedFile != null && uploadedFiles != null)
            //{
            //    List<TicketColumnError> errors = new List<TicketColumnError>();
            //    List<TicketColumnValue> formValues = new List<TicketColumnValue>();

            //    if (attachment.HasFile)
            //    {
            //        string fileName = Path.GetFileName(attachment.PostedFile.FileName);
            //        if (fileName != string.Empty && System.Text.RegularExpressions.Regex.Match(fileName, @"[~#%&*{}\<>?/+|""]").Success)
            //        {
            //            errors.Add(TicketColumnError.AddError("Attachment Name cannot contain [~#%&*{}\\<>?/+|\"]"));
            //        }
            //        else if (uploadedFiles.FirstOrDefault(x => x.ToLower() == fileName.ToLower()) != null)
            //        {
            //            errors.Add(TicketColumnError.AddError("You cannot attach multiple files with the same name."));
            //        }
            //    }
            //    else
            //    {
            //        errors.Add(TicketColumnError.AddError("You cannot attach multiple files with the same name."));
            //    }

            //    if (errors.Count > 0)
            //    {
            //        noError = false;
            //    }

            //    if (noError)
            //    {
            //        formValues = GetValuesFromControls(false, false);
            //        //TicketRequest.Validate(formValues, saveTicket, errors, true, adminOverride, 1);

            //        //Save Attachment
            //        if (errors.Count == 0)
            //        {
            //            //TicketRequest.SetItemValues(saveTicket, formValues, adminOverride, true, 0);
            //            string fileName = Path.GetFileName(attachment.PostedFile.FileName);
            //            attachments.Add(fileName, attachment.FileBytes);
            //            string historyMsg = string.Format("Attached file: {0}", fileName);
            //            //uHelper.CreateHistory(SPContext.Current.Web.CurrentUser, historyMsg, saveTicket, false);

            //            //TicketRequest.CommitChanges(saveTicket, Request.Url);


            //            //attachment.PostedFile.InputStream.Close();
            //            //attachment.PostedFile.InputStream.Dispose();
            //        }
            //        else
            //        {
            //            noError = false;
            //        }
            //    }


            //    if (!noError)
            //    {
            //        List<TicketColumnError> mandatoryErrors = errors.Where(x => x.Type == ErrorType.Mandatory).ToList();
            //        foreach (TicketColumnError er in mandatoryErrors)
            //        {
            //            TicketColumnValue colVal = formValues.FirstOrDefault(x => x.InternalFieldName == er.InternalFieldName);
            //            er.Message = string.Format("<a href='#'  onclick=\"showTab('li_','Tab_','tabPanelContainer_','{0}','6')\" >{1}</a>", colVal.TabNumber, er.DisplayName);
            //        }
            //        if (mandatoryErrors.Count > 0)
            //        {
            //            errorMsgNew.Text = string.Format("Please enter the mandatory fields: {0}", string.Join(", ", mandatoryErrors.Select(x => x.Message).ToArray()));
            //        }

            //        List<TicketColumnError> nonMandatoryErrors = errors.Where(x => x.Type != ErrorType.Mandatory).ToList();
            //        invalidErrorMsgNew.Text = string.Format("{0}", string.Join(", ", nonMandatoryErrors.Select(x => x.Message).ToArray()));

            //        errorMsg.Text = errorMsgNew.Text;
            //        if (errorMsg.Text == string.Empty)
            //        {
            //            errorMsg.Visible = false;
            //        }

            //        invalidErrorMsg.Text = invalidErrorMsgNew.Text;
            //        stageDescriptionContainer.Visible = false;
            //    }
            //    else
            //    {
            //        errorMsgNew.Text = errorMsg.Text;
            //        initialStageDescriptionLiteral.Visible = false;

            //        Response.Redirect(HttpContext.Current.Request.RawUrl);
            //        Response.End();
            //    }
            //}
            #endregion
        }

        protected void deleteFileButton_Click(object sender, EventArgs e)
        {
            #region deleteFileButton_Click
            //List<string> fileNames = new List<string>();
            //foreach (string fileName in currentTicket.Attachments)
            //{
            //    fileNames.Add(fileName);
            //}

            //List<string> deletedFiles = new List<string>();
            //foreach (string fileName in fileNames)
            //{
            //    if (fileName == deletedFile.Value)
            //    {
            //        deletedFiles.Add(fileName);
            //        currentTicket.Attachments.Delete(fileName);
            //        break;
            //    }
            //}

            //if (deletedFiles.Count > 0)
            //{
            //    string historyMsg = string.Format("Deleted attachment: {0}", string.Join(", ", deletedFiles.ToArray()));
            //    if (deletedFiles.Count > 1)
            //    {
            //        historyMsg = string.Format("Deleted attachments: {0}", string.Join(", ", deletedFiles.ToArray()));
            //    }
            //    //uHelper.CreateHistory(SPContext.Current.Web.CurrentUser, historyMsg, currentTicket, false);
            //}

            ////currentTicket.UpdateOverwriteVersion();
            //// added new line for cache.by Deepak
            ////uGITCache.ModuleDataCache.UpdateOpenTicketsCache(moduleId, currentTicket);
            //Response.Redirect(HttpContext.Current.Request.RawUrl);
            //Response.End();
            #endregion
        }

        private void MoveToPreconByLink(bool closeOPM, string senderID)
        {
            if (currentModuleName == "OPM")
            {
                try
                {
                    DataTable crmProjects = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMProject, $"{DatabaseObjects.Columns.ID}= -1");  // Just need Table structure
                    if (crmProjects == null)
                        return;
                    DataRow spListItem = crmProjects.NewRow(); 

                    if (chkboxStageMoveToPrecon.Checked)
                    {
                        #region Create NEW CPR

                        string CPRModule = "CPR";
                        if (saveTicket != null)
                        {
                            Ticket newTicket = new Ticket(context, CPRModule);

                            foreach (DataColumn spField in crmProjects.Columns)
                            {
                                //spListItem[spField.ToString()] = "";
                                if (spField.ColumnName.Equals(DatabaseObjects.Columns.ID)
                                    || spField.ColumnName.Equals(DatabaseObjects.Columns.Attachments)
                                    || spField.ColumnName.Equals(DatabaseObjects.Columns.History)
                                    || spField.ColumnName.Equals(DatabaseObjects.Columns.TicketRequestTypeSubCategory)
                                    || spField.ColumnName.Equals(DatabaseObjects.Columns.TicketRequestTypeWorkflow)
                                    || spField.ColumnName.Equals(DatabaseObjects.Columns.TicketRequestTypeCategory))
                                {
                                    continue;
                                }

                                if (spField.ColumnName.Equals(DatabaseObjects.Columns.TicketId))
                                {
                                    spListItem[UGITUtility.ObjectToString(spField.ColumnName)] = newTicket.GetNewTicketId();
                                }
                                else if (spField.ColumnName.Equals(DatabaseObjects.Columns.ProjectID) && UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.ProjectID))
                                {
                                    spListItem[UGITUtility.ObjectToString(spField.ColumnName)] = saveTicket[DatabaseObjects.Columns.ProjectID];
                                }
                                else if (spField.ColumnName.Equals(DatabaseObjects.Columns.TicketInitiator))
                                {
                                    spListItem[UGITUtility.ObjectToString(spField.ColumnName)] = context.CurrentUser.Id;
                                }
                                else if (spField.ColumnName.Equals(DatabaseObjects.Columns.TicketRequestTypeLookup))
                                {
                                    long OldRequestType = 0;
                                    if (spListItem[DatabaseObjects.Columns.TicketRequestTypeLookup] != DBNull.Value)
                                        OldRequestType = UGITUtility.StringToLong(spListItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                                    else if (saveTicket[DatabaseObjects.Columns.TicketRequestTypeLookup] != DBNull.Value)
                                        OldRequestType = UGITUtility.StringToLong(saveTicket[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                                    else
                                        continue;

                                    ModuleRequestType requestType = TicketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.ID == OldRequestType);
                                    if (requestType != null)
                                    {
                                        var crmProjectRequestTypes = ObjRequestTypeManager.Load(x => x.Category == requestType.Category && x.SubCategory == requestType.SubCategory && x.RequestType == requestType.RequestType && x.ModuleNameLookup == CPRModule).FirstOrDefault();
                                        if (crmProjectRequestTypes != null)
                                        {
                                            spListItem[spField.ColumnName] = crmProjectRequestTypes.ID;
                                        }
                                    }
                                }
                                else
                                {
                                    if (UGITUtility.IfColumnExists(spListItem, spField.ColumnName) && UGITUtility.IfColumnExists(saveTicket, spField.ColumnName))
                                    {
                                        spListItem[spField.ColumnName] = saveTicket[spField.ColumnName.ToString()];
                                    }
                                }
                            }

                            LifeCycle lifeCycle = newTicket.Module.List_LifeCycles.FirstOrDefault(x => x.ID == 0);
                            LifeCycleStage moveToStage = null;

                            if (!string.IsNullOrEmpty(Convert.ToString(hdnStagetoMove.Value)))
                                moveToStage = lifeCycle.Stages.FirstOrDefault(x => x.ID == Convert.ToInt16(hdnStagetoMove.Value));
                            else
                                moveToStage = lifeCycle.Stages.FirstOrDefault(x => x.ID == Convert.ToInt16(ddlStageToMove.SelectedValue));

                            if (moveToStage != null)
                            {
                                spListItem[DatabaseObjects.Columns.TicketStatus] = moveToStage.Name;
                                spListItem[DatabaseObjects.Columns.StageStep] = moveToStage.StageStep;
                                spListItem[DatabaseObjects.Columns.TicketStageActionUserTypes] = moveToStage.ActionUser; //string.Join(Constants.Separator, moveToStage.ActionUser.ToArray());
                                spListItem[DatabaseObjects.Columns.ModuleStepLookup] = moveToStage.ID;
                            }
                            //uHelper.GetUsersAsString(_context, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), currentTicket)
                            spListItem[DatabaseObjects.Columns.TicketStageActionUsers] = context.CurrentUser.Id;

                            spListItem[DatabaseObjects.Columns.TicketOPMIdLookup] = Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]);

                            if (UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.ERPJobID) && UGITUtility.IfColumnExists(spListItem, DatabaseObjects.Columns.ERPJobIDNC) && spListItem[DatabaseObjects.Columns.ERPJobIDNC] != DBNull.Value && spListItem[DatabaseObjects.Columns.ERPJobIDNC] != DBNull.Value && Convert.ToString(saveTicket[DatabaseObjects.Columns.ERPJobIDNC]) != "")
                            {
                                int ERPJobIDNoofCharacters = UGITUtility.StringToInt(ConfigurationVariableManager.GetValue(ConfigConstants.ERPJobIDNoofCharacters));
                                string erpjobidnc = Convert.ToString(saveTicket[DatabaseObjects.Columns.ERPJobIDNC]);
                                if(!string.IsNullOrEmpty(erpjobidnc) && erpjobidnc.Length > 8)
                                    spListItem[DatabaseObjects.Columns.ERPJobID] = Convert.ToString(saveTicket[DatabaseObjects.Columns.ERPJobIDNC]).Substring(0, ERPJobIDNoofCharacters);
                            }

                            string error = newTicket.CommitChanges(spListItem, senderID, Request.Url);

                            if (!string.IsNullOrEmpty(error))
                                return;

                            #region add exit criteria tasks from templates
                            ULog.WriteException("Method ConfigureModuleStageTaskInTicket Called In Event MoveToPreconByLink on Page uGovernitModuleWebpartUserControl.ascx: ticketid " + Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]));
                            UGITModuleConstraint.CreateModuleStageTasksInTicket(context, Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]), CPRModule);
                            UGITModuleConstraint.ConfigureCurrentModuleStageTask(context, spListItem);
                            #endregion

                            #region Create Project internal team from OPM to CPR.
                            CopyProjectTeamFromOPMtoCPR(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]), Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]));
                            #endregion

                            #region Create Project External team from OPM to CPR.
                            CopyExternalTeamFromOPMtoCPR(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]), Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]));
                            #endregion

                            #region Copy Documents from OPM to CPR
                            var repositoryService = new uGovernIT.DMS.Amazon.DMSManagerService(context);
                            repositoryService.CopyDMSDirectory(Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]), Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]));
                            #endregion

                            if (closeOPM)
                            {
                                // Need to check below code later.                            
                                Ticket baseTicket = new Ticket(context, "OPM");
                                // Need to reload ticket to avoid conflict
                                baseTicket.CloseTicket(saveTicket, string.Empty);
                                uHelper.CreateHistory(context.CurrentUser, string.Format("Converted from original item {0} to {1}", saveTicket[DatabaseObjects.Columns.TicketId], spListItem[DatabaseObjects.Columns.TicketId]), saveTicket, context);

                                if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.ClosedDateOnly) && UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.CloseDate))
                                    saveTicket[DatabaseObjects.Columns.ClosedDateOnly] = saveTicket[DatabaseObjects.Columns.CloseDate];
                                //creating duplicate entries in awarding ticket
                                //if (UGITUtility.IfColumnExists(saveTicket, DatabaseObjects.Columns.Comment))
                                //    saveTicket[DatabaseObjects.Columns.Comment] = uHelper.GetCommentString(context.CurrentUser, txtStatusMessage.Text, saveTicket, DatabaseObjects.Columns.Comment, false);

                                TicketRequest.CommitChanges(saveTicket);

                                DataRow dr = Ticket.GetCurrentTicket(context, ModuleNames.CPR, Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]));
                                uHelper.CreateHistory(context.CurrentUser, string.Format("Converted from original item {0}", saveTicket[DatabaseObjects.Columns.TicketId]), dr, context);

                                ObjTicketManager.Save(ObjModuleViewManager.LoadByName(ModuleNames.CPR), dr);
                                CreateItemConvertedMessage(Convert.ToString(spListItem[DatabaseObjects.Columns.TicketId]), Convert.ToString(spListItem[DatabaseObjects.Columns.Title]), ModuleNames.CPR, UGITUtility.GetAbsoluteURL(newTicket.Module.StaticModulePagePath));
                            }
                        }

                        #endregion
                    }

                    if (!chkStatusOpenCPR.Checked && chkOPMEmail.Checked)
                    {
                        string ToMail = string.Empty;
                        StringBuilder mailCcList = new StringBuilder();
                        string mailCc = string.Empty;

                        ToMail = UGITUtility.RemoveDuplicateEmails(Convert.ToString(UserManager.GetUserEmailId(statusMTPUserGroups.GetValues())), Constants.Separator5);
                        mailCcList = UserManager.GetUserEmailId(ccUsersBox.GetValues());
                        if (chkOPSActionUser.Checked)
                        {
                            string stageActionUser = objEscalationProcess.getStageActionUsers(spListItem);
                            //string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailId(stageActionUser));
                            string actionUseruserEmails = Convert.ToString(UserManager.GetUserEmailIdByGroupOrUserName(stageActionUser, Constants.Separator));

                            string[] separator = { Constants.UserInfoSeparator };
                            if (!string.IsNullOrEmpty(actionUseruserEmails))
                            {
                                string[] actionUserEmails = actionUseruserEmails.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < actionUserEmails.Length; i++)
                                {
                                    string actionUser = Convert.ToString(actionUserEmails[i]);
                                    if (actionUser != string.Empty)
                                    {
                                        if (mailCcList.Length != 0)
                                            mailCcList.Append(";");
                                        mailCcList.Append(actionUser);
                                    }
                                }
                            }
                        }

                        if (mailCcList.Length > 0)
                            mailCc = UGITUtility.RemoveDuplicateCcFromToEmails(ToMail, Convert.ToString(mailCcList), Constants.Separator5);

                        MailMessenger mail = new MailMessenger(context);

                        StringBuilder emailBody = new StringBuilder();
                        emailBody.AppendFormat(txtStatusMailBody.Html);

                        if (chkboxStageMoveToPrecon.Checked && spListItem != null)
                        {
                            string urlCPR = string.Format("{0}?TicketId={2}&ModuleName={1}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath), "CPR", spListItem[DatabaseObjects.Columns.TicketId]);
                            emailBody.AppendFormat(string.Format("<br/>Please click {0} to view the Precon record. <br/><br/>Thanks", string.Format("<a href='{0}' target='_blank'>here</a>", urlCPR)));
                        }
                        txtStatusMailBody.Html = emailBody.ToString();
                        emailBody.Clear();

                        if (string.IsNullOrEmpty(ToMail))
                            return;

                        mail.SendMail(ToMail, txtStatusMailSubject.Text, mailCc, txtStatusMailBody.Html, true, new string[] { }, false, 
                            Convert.ToBoolean(hdnSendMailFromStatus.Value), UGITUtility.ObjectToString(spListItem[DatabaseObjects.Columns.TicketId]));
                    }
                }catch(Exception ex)
                {
                    ULog.WriteException("Method MovToPreconByLink:" + ex.Message);
                }
            }
        }

        private void CreateItemConvertedMessage(string ticketid, string title, string module, string moduleListPagePath)
        {
            string newModuleTicketTitle = string.Format("{0}: {1}", ticketid, uHelper.ReplaceInvalidCharsInURL(UGITUtility.TruncateWithEllipsis(title, 100, string.Empty)));
            string newModuleTicketLink = string.Format("<a href=\"javascript:\" id='myAnchor' onclick=\"PopupCloseBeforeOpenUgit('{1}','ticketId={0}','{2}', 90, 90, 0, '{3}' );  \">{0}</a>", ticketid, moduleListPagePath, newModuleTicketTitle, Request["source"]);
            string typeName = UGITUtility.newTicketTitle(module);
            string informationMsg = string.Format("<div class='row informationMsg'><div>{0} Created : <div class='createdTicket-id'>{1}</div></div></div>",
                                                    typeName, newModuleTicketLink);
            string header = string.Format("{0} Created", typeName);

            ItemConversionMessage[0] = informationMsg;
            ItemConversionMessage[1] = header;
            IsItemConverted = true;
        }
        protected void CreateNewTicket()
        {
            if (newTicket == null)
                newTicket = TicketSchema.NewRow();

            //newTopGraphicPanel.Visible = false;
            //if (lifeCycle != null && lifeCycle.Stages.Count >= 3)
            //{
            //    newTopGraphicPanel.Visible = true;
            //    newTicketStageRepeater.DataSource = lifeCycle.Stages;
            //    newTicketStageRepeater.DataBind();
            //}
            NewTicketWorkFlowDiv.Visible = !TicketRequest.Module.HideWorkFlow;
            if (lifeCycle != null && lifeCycle.Stages.Count >= 3)
            {
                NewTicketWorkFlowListView.DataSource = lifeCycle.Stages;
                NewTicketWorkFlowListView.DataBind();

            }

            if (lifeCycle != null && lifeCycle.Stages.Count > 0)
            {
                initialStageDescriptionLiteral.Text = lifeCycle.Stages[0].UserPrompt;
            }

            newTicketTable.Rows.Clear();
            newTicketTable.Controls.Clear();
            newTicketTable.BorderWidth = 0;
            newTicketTable.BorderStyle = BorderStyle.Solid;
            int columnCount = 0, fieldWidth = 1;
            TableRow row = new TableRow();
            row.BorderStyle = BorderStyle.None;
            row.BorderWidth = 0;

            List<ModuleFormLayout> collFormLayoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabId == 0 && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();

            Table mainTable = tabTable;
            Enums.TableDisplayTypes tableType = new Enums.TableDisplayTypes();
            tableType = Enums.TableDisplayTypes.General;
            string legend = string.Empty;
            int sectionFieldSequence = 0;
            DataRow sourceTicket = null;
            DataRow parentTicket = null;
            //new line for ticket type change..
            DataRow oldTicket = null;

            UGITModule module = ModuleViewManager.LoadByName(currentModuleName);

            #region CreateNewFromSourceTicket

            if (HttpContext.Current.Request["ParentId"] != null)
            {
                string parentTicketId = Convert.ToString(Request.QueryString["ParentId"]);
                parentTicket = TicketManager.GetTicketTableBasedOnTicketId(uHelper.getModuleNameByTicketId(parentTicketId), parentTicketId).Select()[0];

                //excute if taskid is coming. 
                //copy task title, description, duedate from task and put it in project item so that new ticket can copy value from it.
                if (HttpContext.Current.Request["taskId"] != null && HttpContext.Current.Request["taskId"].Trim() != string.Empty && parentTicket != null)
                {
                    int taskID = 0;
                    int.TryParse(HttpContext.Current.Request["taskId"].Trim(), out taskID);
                    //int parentModuleId = uHelper.getModuleIdByTicketID(SPContext.Current.Web, Request.QueryString["ParentId"]);
                    DataRow parentModuleRow = UGITUtility.ObjectToData(module).Rows[0]; //.GetModuleDetails(parentModuleId);
                    string parentModuleName = Convert.ToString(parentModuleRow[DatabaseObjects.Columns.ModuleName]);
                    UGITTask taskItem = null;
                    UGITTaskManager ModuleTaskManager = new UGITTaskManager(context);
                    if (parentModuleName == "PMM")
                    {
                        taskItem = ModuleTaskManager.LoadByID(taskID);// SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMTasks, taskID);
                    }
                    else if (parentModuleName == "TSK")
                    {
                        taskItem = ModuleTaskManager.LoadByID(taskID);//SPListHelper.GetSPListItem(DatabaseObjects.Lists.TSKTasks, taskID);
                    }

                    if (taskItem != null)
                    {
                        parentTicket[DatabaseObjects.Columns.Title] = taskItem.Title;
                        parentTicket[DatabaseObjects.Columns.TicketDescription] = taskItem.Description;
                        if (parentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketDesiredCompletionDate))
                            parentTicket[DatabaseObjects.Columns.TicketDesiredCompletionDate] = taskItem.DueDate;
                    }
                }
            }



            #endregion

            // Assign start stage of module ticket
            newTicket[DatabaseObjects.Columns.ModuleStepLookup] = newTicketStageId;

            //Get and set default value of module for item
            TicketRequest.AssignModuleSpecificDefaults(newTicket);

            // Get the source Ticket if the source ticket id is passed.
            // SourceTicketId is passed from CreateNew Incident  button from PRS Module.
            if (Request.QueryString["SourceTicketId"] != null)
            {
                string sourceTicketId = Request.QueryString["SourceTicketId"];
                sourceTicket = TicketManager.GetTicketTableBasedOnTicketId(uHelper.getModuleNameByTicketId(sourceTicketId), sourceTicketId).Select()[0];
                parentTicket = sourceTicket;
                if (IsDuplicate || IsNewSubticket)
                    parentTicket[DatabaseObjects.Columns.Title] = sourceTicket[DatabaseObjects.Columns.Title] + " - Copy";
            }
            else
            {
                //Set the newTicket object as the default parent ticket, sci
                parentTicket = newTicket;
            }

            if (Request.QueryString["OldTicketId"] != null)
            {
                try
                {
                    string oldTicketId = Request.QueryString["OldTicketId"];
                    //oldTicket = TicketManager.GetTicketTableBasedOnTicketId(uHelper.getModuleNameByTicketId(oldTicketId), oldTicketId).Rows[0];
                    oldTicket = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(oldTicketId), oldTicketId);
                    if (oldTicket != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketDescription, oldTicket.Table))
                    {
                        oldTicket[DatabaseObjects.Columns.TicketDescription] = string.Format("*** Converted from original ticket {0} ***\n\n{1}", oldTicket[DatabaseObjects.Columns.TicketId], oldTicket[DatabaseObjects.Columns.TicketDescription]);
                        copyTicket = oldTicket;
                    }
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex);
                }
            }

            DataTable ticketDt = TicketSchema;
            DataRow newRow = ticketDt.NewRow();

            //Reset Password
            if (PhraseSearch == "1")
            {
                newRow["Title"] = agentTitle;

                newRow["Description"] = agentTitle;
            }

            ticketControls = new TicketControls(module, newRow);
            if (Request.QueryString["OldTicketId"] != null)
            {
                ticketControls.SourceItem = ticketDt.NewRow();
                if (oldTicket != null)
                    CopySpItem(oldTicket, ticketControls.SourceItem);
            }
            else
                ticketControls.SourceItem = parentTicket;

            ticketControls.BatchEdit = BatchMode.None;

            //Set user id inside requestor if batch create is enable
            if (UGITUtility.StringToBoolean(Request.QueryString["batchCreate"]))
            {
                ticketControls.BatchEdit = BatchMode.Create;
                if (ticketDt.Columns.Contains(DatabaseObjects.Columns.TicketRequestor))
                    newRow[DatabaseObjects.Columns.TicketRequestor] = Request["userIds"];
            }
            var sequenceGroupStart = collFormLayoutItems.Select(x => x.FieldSequence).FirstOrDefault();

            //Create New ticket form layout
            foreach (ModuleFormLayout tabField in collFormLayoutItems)
            {
                string fieldDisplayText = tabField.FieldDisplayName;
                fieldWidth = tabField.FieldDisplayWidth;

                if (tabField.TargetURL != string.Empty && tabField.TargetURL != null)
                {
                    string width = "100";
                    string height = "100";


                    string href = string.Format(@"javascript:UgitOpenPopupDialog('{0}','','Help - {2}','{3}','{4}',0,'{1}')", tabField.TargetURL, Server.UrlEncode(Request.Url.AbsolutePath), tabField.FieldDisplayName, width, height);
                    fieldDisplayText = string.Format("<a title=\"{3}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{2}\" href=\"{1}\">{0}</a>", fieldDisplayText, href, currentModuleName, tabField.Tooltip);

                    if (tabField.TargetType.EqualsIgnoreCase("HelpCard"))
                    {
                        width = "23";
                        height = "75";
                        fieldDisplayText = tabField.FieldDisplayName;
                        href = string.Format(@"javascript:openHelpCard('{0}','{1}')", tabField.TargetURL, tabField.FieldDisplayName);
                        fieldDisplayText = string.Format("<a title=\"{3}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{2}\" href=\"{1}\">{0}</a>", fieldDisplayText, href, currentModuleName, tabField.Tooltip);
                    }
                }
                else if (tabField.Tooltip != string.Empty && tabField.TargetURL != null)
                {
                    fieldDisplayText = string.Format("<span title=\"{2}\" class=\"labeltooltip\"  displayName=\"{0}\" modulename=\"{1}\">{0}</span>", fieldDisplayText, currentModuleName, tabField.Tooltip);
                }
                string fieldInternalName = tabField.FieldName;

                if (fieldInternalName == "#GroupStart#" || fieldInternalName == "#GroupEnd#")
                {
                    if (fieldInternalName == "#GroupStart#")
                    {
                        tableType = Enums.TableDisplayTypes.Grouped;
                        groupedTabTable = new Table();
                        groupedTabTable.Attributes.Add("style", "width:100%");
                        legend = fieldDisplayText;
                        sectionFieldSequence = tabField.FieldSequence;

                    }
                    else if (fieldInternalName == "#GroupEnd#")
                    {
                        CreateShowhideSectionHyperLink(sectionFieldSequence, sequenceGroupStart, legend);
                        tableType = Enums.TableDisplayTypes.General;
                    }
                    row = new TableRow();
                    row.BorderStyle = BorderStyle.None;
                    row.BorderWidth = 0;
                    columnCount = 0;// reset column count so next column will be draw from start in new section
                }
                else if (TicketSchema.Columns.Contains(fieldInternalName))
                {
                    DataColumn fieldColumn = ticketDt.Columns[fieldInternalName];
                    //SPField f = thisList.Fields.GetFieldByInternalName(fieldInternalName);
                    Control bfc = null;
                    //Control bfc = uGovernIT.Helpers.SPControls.GetSharePointControls(this.Context, f, thisList, newTicket, SPControlMode.New, moduleId, parentTicket);
                    //if (bfc != null)
                    //{
                    FieldDesignMode designMode = FieldDesignMode.Normal;
                    ModuleRoleWriteAccess ticketRoleWriteAccessTemp = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStep || x.StageStep == 0) && x.FieldName == fieldInternalName);
                    try
                    {
                        bool showWithCheckBox = false;
                        //sourceTicket.CopyToDataTable();
                        if (ticketRoleWriteAccessTemp != null)
                        {
                            if (ticketRoleWriteAccessTemp.ShowWithCheckbox && ticketRoleWriteAccessTemp.ShowEditButton)
                                designMode = FieldDesignMode.WithEditAndCheckbox;
                            else if (ticketRoleWriteAccessTemp.ShowEditButton)
                                designMode = FieldDesignMode.WithEdit;
                            else if (ticketRoleWriteAccessTemp.ShowWithCheckbox)
                                designMode = FieldDesignMode.WithCheckbox;


                            showWithCheckBox = ticketRoleWriteAccessTemp.ShowWithCheckbox;

                            bfc = ticketControls.GetControls(fieldColumn, ControlMode.New, designMode, "", tabField);

                            if (fieldDisplayText.Contains("Analytics"))
                            {
                                fieldDisplayText += "<img src='/Content/images/analytics.png' />";
                            }
                            if (ticketRoleWriteAccessTemp.FieldMandatory)
                            {
                                fieldDisplayText += "<b style=\"color:red\">* </b>";
                            }
                        }
                        else
                        {
                            designMode = FieldDesignMode.WithEdit;
                            bfc = ticketControls.GetControls(fieldColumn, ControlMode.Display, designMode, "", tabField);
                        }

                        if (TicketSchema.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                        {
                            requestOwnerId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ClientID;
                        }
                        else if (TicketSchema.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                        {
                            priorityId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ClientID;
                        }
                        else if (fieldDisplayText.Contains("Requested "))
                        {
                            requestorContainerId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ID;
                        }
                        else if (fieldDisplayText.Contains("Related Asset"))
                        {
                            relatedAssetContainerId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log.WriteException(ex, "ModuleWebPart- CreateNetTicket()");
                        Util.Log.ULog.WriteException(ex);
                    }

                    columnCount += fieldWidth;
                    if (columnCount > totalColumnsInDisplay)
                    {
                        for (int j = columnCount - fieldWidth; j < totalColumnsInDisplay; j++)
                        {
                            if (tableType == Enums.TableDisplayTypes.Grouped)
                                addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                            else if (tableType == Enums.TableDisplayTypes.General)
                                addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, newTicketTable);
                        }
                        columnCount = fieldWidth;
                        row = new TableRow();
                        row.BorderStyle = BorderStyle.Solid;
                        row.BorderWidth = 1;
                    }


                    if (fieldInternalName == DatabaseObjects.Columns.TicketResolutionComments
                        || fieldInternalName == DatabaseObjects.Columns.TicketBusinessManager
                        || fieldInternalName == DatabaseObjects.Columns.TicketGLCode
                        || fieldInternalName == DatabaseObjects.Columns.TicketLegal
                        || fieldInternalName == DatabaseObjects.Columns.TicketFinanceManager
                        || fieldInternalName == DatabaseObjects.Columns.TicketPurchasing
                        || fieldInternalName == DatabaseObjects.Columns.TicketTester)
                    {
                        row.CssClass += " " + fieldInternalName + "Row";
                    }
                    if (tableType == Enums.TableDisplayTypes.Grouped)

                        //addToTable(row, bfc, fieldDisplayText, fieldWidth, groupedTabTable);
                        addToTicketTable(row, bfc, fieldDisplayText, fieldWidth, groupedTabTable, sequenceGroupStart, sectionFieldSequence, fieldInternalName);
                    else if (tableType == Enums.TableDisplayTypes.General)
                        addToTable(row, bfc, fieldDisplayText, fieldWidth, newTicketTable, fieldInternalName);

                    #region CreateFromSourceTicket
                    //Fill the values from source ticket is source ticket id is passed & the fields match
                    //if (Request.QueryString["SourceTicketId"] != null && sourceTicket != null && uHelper.IsSPItemExist(sourceTicket, fieldInternalName))
                    //{
                    //    SPFieldType fieldType = sourceTicket.Fields.GetFieldByInternalName(fieldInternalName).Type;
                    //    switch (fieldType)
                    //    {
                    //        case SPFieldType.Text:
                    //            {
                    //                TextField tt = (TextField)(bfc);
                    //                tt.Text = sourceTicket[fieldInternalName].ToString();
                    //            }
                    //            break;
                    //        case SPFieldType.Note:
                    //            {
                    //                Microsoft.SharePoint.WebControls.NoteField nt = (Microsoft.SharePoint.WebControls.NoteField)(bfc);
                    //                nt.Text = sourceTicket[fieldInternalName].ToString();
                    //            }
                    //            break;
                    //        case SPFieldType.Choice:
                    //            {
                    //                DropDownChoiceField ddl = (Microsoft.SharePoint.WebControls.DropDownChoiceField)(bfc);
                    //                ddl.Value = sourceTicket[fieldInternalName];
                    //            }
                    //            break;
                    //        case SPFieldType.DateTime:
                    //            {
                    //                Microsoft.SharePoint.WebControls.DateTimeField dtc = (Microsoft.SharePoint.WebControls.DateTimeField)(bfc);
                    //                dtc.Value = sourceTicket[fieldInternalName];
                    //            }
                    //            break;
                    //        case SPFieldType.User:
                    //            if (bfc.GetType() == typeof(Microsoft.SharePoint.WebControls.UserField))
                    //            {
                    //                Microsoft.SharePoint.WebControls.UserField usr = (Microsoft.SharePoint.WebControls.UserField)(bfc);
                    //                usr.Value = sourceTicket[fieldInternalName];
                    //            }
                    //            break;
                    //        case SPFieldType.Lookup:
                    //            if (fieldInternalName == DatabaseObjects.Columns.TicketImpactLookup
                    //                || fieldInternalName == DatabaseObjects.Columns.TicketSeverityLookup)
                    //            {
                    //                ((DropDownList)(bfc)).SelectedValue = uHelper.GetSimilarLookupValue(((SPFieldLookup)sourceTicket.Fields.GetFieldByInternalName(fieldInternalName)), moduleId.ToString(), fieldInternalName, uHelper.SplitString(sourceTicket[fieldInternalName], Constants.Separator)[1]);
                    //            }
                    //            NEED TO ADD: Field - Specific code for impact, severity, priority, Related Asset, etc.
                    //             /* THESE DON'T WORK, PLEASE FIX!!
                    //            if (fieldInternalName == DatabaseObjects.Columns.AssetLookup)
                    //            {
                    //                try
                    //                {
                    //                    string assetId = uHelper.SplitString(sourceTicket[DatabaseObjects.Columns.AssetLookup], ";#", 0);
                    //                    ((DropDownList)((Microsoft.SharePoint.WebControls.LookupField)(bfc)).Controls[0]).SelectedValue = assetId;
                    //                }
                    //                catch
                    //                {
                    //                     if exception comes while getting the control the it will select blank.
                    //                     no need to throw exception
                    //                }
                    //            }
                    //            else if (fieldInternalName == DatabaseObjects.Columns.PRSLookup) // Used for creating incident from PRS only
                    //            {
                    //                ((DropDownList)(bfc)).SelectedValue = Convert.ToString(sourceTicket[DatabaseObjects.Columns.TicketId]);
                    //            }
                    //            else */
                    //            if (bfc.GetType() == typeof(DropDownList))
                    //                {
                    //                    string lookupID = uHelper.SplitString(sourceTicket[fieldInternalName], ";#", 0);
                    //                    ((DropDownList)(bfc)).SelectedValue = lookupID;
                    //                }
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    #endregion

                    #region CreateFromCompany
                    // Fill the values from source ticket is source ticket id is passed & the fields match

                    FieldConfiguration field = null;
                    foreach (var item in bfc.Controls)
                    {
                        if (Request.QueryString["CompanyId"] != null)
                        {
                            int companyId = Convert.ToInt32(Request.QueryString["CompanyId"]);

                            var spCompanyItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMCompany, " id=" + companyId).Select()[0];

                            // SPListItem spCompanyItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMCompany, companyId);
                            if (fieldInternalName == DatabaseObjects.Columns.ContactLookup)
                            {
                                if (UGITUtility.IsSPItemExist(spCompanyItem, fieldInternalName))
                                {

                                    ((LookUpValueBox)item).SetValues(Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spCompanyItem[fieldInternalName]))));
                                    // gdLookup.SetValues(Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spCompanyItem[fieldInternalName]))));
                                }
                            }
                            //else if (fieldInternalName == "CRMCompanyTitleLookup")
                            //else if (fieldInternalName == DatabaseObjects.Columns.CRMCompanyLookup)
                            //{
                            //    //  ((LookUpValueBox)item).SetValues(Convert.ToString(companyId));
                            //    ((CustomListDropDown)bfc).SetValue(Convert.ToString(spCompanyItem[DatabaseObjects.Columns.TicketId]));
                            //}
                            /*
                            else if (fieldInternalName == DatabaseObjects.Columns.StreetAddress1
                                      || fieldInternalName == DatabaseObjects.Columns.StreetAddress2
                                       || fieldInternalName == DatabaseObjects.Columns.City
                                || fieldInternalName == DatabaseObjects.Columns.Zip || fieldInternalName == DatabaseObjects.Columns.UGITState)
                            {
                                 ((ASPxTextBox)item).Text = Convert.ToString(spCompanyItem[fieldInternalName]);
                                // ((TextBox)(bfc)).Text = Convert.ToString(spCompanyItem[fieldInternalName]);
                                // ((TextBox)(bfc)).Text = Convert.ToString(spCompanyItem[fieldInternalName]);//TextField
                            }
                            */
                            else if (fieldInternalName == DatabaseObjects.Columns.StateLookup && UGITUtility.GetLookupID(Convert.ToString(spCompanyItem[fieldInternalName])) > 1)
                            {
                                ((LookUpValueBox)item).SetValues(Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spCompanyItem[fieldInternalName]))));
                            }
                            else
                            {
                                if (UGITUtility.IfColumnExists(spCompanyItem, fieldInternalName))
                                {
                                    if (item is ASPxTextBox)
                                        ((ASPxTextBox)item).Text = Convert.ToString(spCompanyItem[fieldInternalName]);
                                    else if (item is UserValueBox)
                                        ((UserValueBox)item).SetValues(Convert.ToString(spCompanyItem[fieldInternalName]));
                                    else if (item is LookUpValueBox)
                                        ((LookUpValueBox)item).SetValues(Convert.ToString(spCompanyItem[fieldInternalName]));
                                    else if (item is TextBox)
                                        ((TextBox)item).Text = Convert.ToString(spCompanyItem[fieldInternalName]);
                                }
                            }
                        }

                        else if (Request.QueryString["CompanyTicketId"] != null)
                        {

                            string companyTicketId = Convert.ToString(Request.QueryString["CompanyTicketId"]);
                            var spCompanyItem = Ticket.GetCurrentTicket(context, "COM", companyTicketId);

                            if (fieldInternalName == DatabaseObjects.Columns.ContactLookup)
                            {
                                if (UGITUtility.IsSPItemExist(spCompanyItem, fieldInternalName))
                                {
                                    ((LookUpValueBox)item).SetValues(Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spCompanyItem[fieldInternalName]))));
                                }
                            }
                            //else if (fieldInternalName == "CRMCompanyTitleLookup")
                            else if (fieldInternalName == DatabaseObjects.Columns.CRMCompanyLookup)
                            {
                                //((LookUpValueBox)item).SetValues(Convert.ToString(spCompanyItem[DatabaseObjects.Columns.Id]));
                                ((CustomListDropDown)bfc).SetValue(Convert.ToString(companyTicketId));
                            }
                            /*
                            else if (fieldInternalName == DatabaseObjects.Columns.StreetAddress1
                                      || fieldInternalName == DatabaseObjects.Columns.StreetAddress2
                                       || fieldInternalName == DatabaseObjects.Columns.City
                                || fieldInternalName == DatabaseObjects.Columns.Zip || fieldInternalName == DatabaseObjects.Columns.UGITState)
                            {
                                ((ASPxTextBox)item).Text = Convert.ToString(spCompanyItem[fieldInternalName]);
                            }
                            */
                            //else if (fieldInternalName == DatabaseObjects.Columns.UGITStateLookup)
                            //{
                            //    ((DropDownList)item).SelectedValue = Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spCompanyItem[fieldInternalName])));
                            //}
                            else if (fieldInternalName == DatabaseObjects.Columns.StateLookup)
                            {
                                ((LookUpValueBox)item).SetValues(Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spCompanyItem[fieldInternalName]))));
                            }
                            else
                            {
                                if (UGITUtility.IfColumnExists(spCompanyItem, fieldInternalName))
                                {
                                    if (item is ASPxTextBox)
                                        ((ASPxTextBox)item).Text = Convert.ToString(spCompanyItem[fieldInternalName]);
                                    else if (item is UserValueBox)
                                        ((UserValueBox)item).SetValues(Convert.ToString(spCompanyItem[fieldInternalName]));
                                    else if (item is LookUpValueBox)
                                        ((LookUpValueBox)item).SetValues(Convert.ToString(spCompanyItem[fieldInternalName]));
                                    else if (item is TextBox)
                                        ((TextBox)item).Text = Convert.ToString(spCompanyItem[fieldInternalName]);
                                }
                            }
                        }

                        #endregion

                        #region CreateFromLeadOrOPM
                        // Fill the values from source ticket is source ticket id is passed & the fields match


                        DataRow spItem = null;

                        if (Request.QueryString["LeadId"] != null)
                        {
                            int leadId = Convert.ToInt32(Request.QueryString["LeadId"]);
                            spItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Lead, " id=" + leadId).Select()[0];//filter tenantid

                            if (!string.IsNullOrEmpty(Convert.ToString(spItem[DatabaseObjects.Columns.TicketId])))
                                hdnMode.Value = Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]);

                            // spItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.Lead, leadId);
                            // if (newTicket != null && newTicket.Field.ContainsField(DatabaseObjects.Columns.TicketLEMIdLookup))
                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.LEMIdLookup))
                                newTicket[DatabaseObjects.Columns.LEMIdLookup] = Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.SuccessChance))
                                newTicket[DatabaseObjects.Columns.SuccessChance] = Convert.ToString(spItem[DatabaseObjects.Columns.SuccessChance]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.BCCISector))
                                newTicket[DatabaseObjects.Columns.BCCISector] = Convert.ToString(spItem[DatabaseObjects.Columns.BCCISector]);
                            /*
                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.ProjectID))
                                newTicket[DatabaseObjects.Columns.ProjectID] = Convert.ToString(spItem[DatabaseObjects.Columns.ProjectID]);
                            */
                            if (fieldInternalName == DatabaseObjects.Columns.CRMDuration)
                            {
                                if (UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionStart) && UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                                {
                                    // NumberField tt = (NumberField)(bfc);
                                    ASPxTextBox tt = (ASPxTextBox)(item);
                                    DateTime startDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                    DateTime endDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);

                                    int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, startDate, endDate);
                                    int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                                    tt.Value = noOfWeeks;
                                }
                            }

                        }

                        else if (Request.QueryString["LeadTicketId"] != null)
                        {
                            hdnMode.Value = Convert.ToString(Request.QueryString["LeadTicketId"]);
                            //leadidg.Text = Convert.ToString(Request.QueryString["LeadTicketId"]);
                            spItem = Ticket.GetCurrentTicket(context, "LEM", Convert.ToString(Request.QueryString["LeadTicketId"]));
                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.LEMIdLookup))
                                newTicket[DatabaseObjects.Columns.LEMIdLookup] = Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.SuccessChance))
                                newTicket[DatabaseObjects.Columns.SuccessChance] = Convert.ToString(spItem[DatabaseObjects.Columns.SuccessChance]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.BCCISector))
                                newTicket[DatabaseObjects.Columns.BCCISector] = Convert.ToString(spItem[DatabaseObjects.Columns.BCCISector]);
                            /*
                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.ProjectID))
                                newTicket[DatabaseObjects.Columns.ProjectID] = Convert.ToString(spItem[DatabaseObjects.Columns.ProjectID]);
                            */
                            if (fieldInternalName == DatabaseObjects.Columns.CRMDuration)
                            {
                                if (UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionStart) && UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                                {
                                    //NumberField tt = (NumberField)(bfc);
                                    ASPxTextBox tt = (ASPxTextBox)(item);
                                    DateTime startDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                    DateTime endDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);

                                    int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, startDate, endDate);
                                    int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                                    tt.Value = noOfWeeks;
                                }
                            }
                        }

                        else if (Request.QueryString["OpportunityId"] != null)
                        {
                            int opmId = Convert.ToInt32(Request.QueryString["OpportunityId"]);
                            spItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Opportunity, $"{DatabaseObjects.Columns.Id}={opmId} and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select()[0];//filter tenantid

                            //spItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.Opportunity, opmId);
                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketOPMIdLookup))
                                newTicket[DatabaseObjects.Columns.TicketOPMIdLookup] = Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.BCCISector))
                                newTicket[DatabaseObjects.Columns.BCCISector] = Convert.ToString(spItem[DatabaseObjects.Columns.BCCISector]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.ProjectID))
                                newTicket[DatabaseObjects.Columns.ProjectID] = Convert.ToString(spItem[DatabaseObjects.Columns.ProjectID]);

                            if (fieldInternalName == DatabaseObjects.Columns.CRMDuration)
                            {
                                if (UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionStart) && UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                                {
                                    //NumberField tt = (NumberField)(bfc);
                                    ASPxTextBox tt = (ASPxTextBox)(item);
                                    DateTime startDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                    DateTime endDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);

                                    int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, startDate, endDate);
                                    int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                                    tt.Value = noOfWeeks;
                                }
                            }
                        }

                        else if (Request.QueryString["OpportunityTicketId"] != null)
                        {
                            spItem = Ticket.GetCurrentTicket(context, "OPM", Convert.ToString(Request.QueryString["OpportunityTicketId"]));

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketOPMIdLookup))
                                newTicket[DatabaseObjects.Columns.TicketOPMIdLookup] = Convert.ToString(Request.QueryString["OpportunityTicketId"]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.BCCISector))
                                newTicket[DatabaseObjects.Columns.BCCISector] = Convert.ToString(spItem[DatabaseObjects.Columns.BCCISector]);

                            if (newTicket != null && newTicket.Table.Columns.Contains(DatabaseObjects.Columns.ProjectID))
                                newTicket[DatabaseObjects.Columns.ProjectID] = Convert.ToString(spItem[DatabaseObjects.Columns.ProjectID]);

                            if (fieldInternalName == DatabaseObjects.Columns.CRMDuration)
                            {
                                if (UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionStart) && UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                                {
                                    //NumberField tt = (NumberField)(bfc);
                                    ASPxTextBox tt = (ASPxTextBox)(item);
                                    DateTime startDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                                    DateTime endDate = Convert.ToDateTime(spItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);

                                    int noOfWorkingDays = uHelper.GetTotalWorkingDaysBetween(context, startDate, endDate);
                                    int noOfWeeks = uHelper.GetWeeksFromDays(context, noOfWorkingDays);
                                    tt.Value = noOfWeeks;
                                }
                            }
                        }

                        else if (Request.QueryString["CPRTicketId"] != null)
                        {
                            spItem = Ticket.GetCurrentTicket(context, "CPR", Convert.ToString(Request.QueryString["CPRTicketId"]));

                            if (Request["moduleFrom"] == "CPP")
                                newTicket[DatabaseObjects.Columns.TicketCPRIdLookup] = Convert.ToInt32(spItem[DatabaseObjects.Columns.Id]);

                            if (fieldInternalName == DatabaseObjects.Columns.TicketComment || fieldInternalName == DatabaseObjects.Columns.TicketActualStartDate || fieldInternalName == DatabaseObjects.Columns.TicketActualCompletionDate)
                            {
                                continue;
                            }
                        }


                        if (spItem != null && (UGITUtility.IsSPItemExist(spItem, fieldInternalName) || (fieldInternalName == DatabaseObjects.Columns.TicketOwner || fieldInternalName == DatabaseObjects.Columns.OwnerUser)))  // Owner & OwnerUser fields are same, but referred differently in different modules
                        {
                            //FieldConfiguration field = FieldConfigurationManager. GetFieldByFieldName(fieldInternalName);
                            field = FieldConfigurationManager.GetFieldByFieldName(fieldInternalName);
                            string fieldtype = string.Empty;
                            if (field != null)
                                fieldtype = field.Datatype;
                            switch (fieldtype)
                            {
                                case "System.String":
                                    {
                                        // market sector issue fix  when we add this condition.
                                        if (fieldInternalName == "MarketSector")
                                        {
                                            Label tt = (Label)(item);
                                            tt.Text = Convert.ToString(spItem[fieldInternalName]);
                                        }
                                        else if (fieldInternalName == DatabaseObjects.Columns.LeadSourceCompanyLabel)
                                        {
                                            Label tt = (Label)(item);
                                            tt.Text = Convert.ToString(spItem[fieldInternalName]);
                                        }
                                        else
                                        {
                                            // TextField tt = (TextField)(bfc);
                                            ASPxTextBox tt = (ASPxTextBox)(item);
                                            tt.Text = Convert.ToString(spItem[fieldInternalName]);
                                        }
                                    }
                                    break;
                                case "NoteField":
                                    {
                                        //if (bfc is Microsoft.SharePoint.WebControls.NoteField)
                                        if (item is ASPxTextBox)
                                        {
                                            // Microsoft.SharePoint.WebControls.NoteField nt = (Microsoft.SharePoint.WebControls.NoteField)(bfc);
                                            ASPxTextBox nt = (ASPxTextBox)(item);
                                            nt.Text = spItem[fieldInternalName].ToString();
                                        }
                                        else if (item is TextBox)
                                        {
                                            TextBox tb = (TextBox)(item);
                                            tb.Text = spItem[fieldInternalName].ToString();
                                        }
                                    }
                                    break;
                                case "Choices":
                                    {
                                        if (item is LookUpValueBox)
                                        {
                                            LookUpValueBox gdLookup = (LookUpValueBox)(item);
                                            gdLookup.Value = spItem[fieldInternalName].ToString();
                                        }

                                        //if (item is ASPxComboBox)
                                        //{
                                        //    ASPxComboBox ddl = (ASPxComboBox)(item);
                                        //    ddl.Value = spItem[fieldInternalName];
                                        //}
                                        //else if (bfc is UGITDropDown)
                                        //{
                                        //    UGITDropDown ugitDDL = (UGITDropDown)(bfc);
                                        //    ugitDDL.DropDown.Value = spItem[fieldInternalName];
                                        //}

                                    }
                                    break;
                                //case SPFieldType.DateTime:
                                case "DateTime":
                                case "Date":
                                    {
                                        ASPxDateEdit dtc = null;

                                        if (item is ASPxDateEdit)
                                        {
                                            dtc = (ASPxDateEdit)(item);
                                        }
                                        else if (bfc.Controls.Count > 0 && bfc.Controls[0] is ASPxDateEdit)//check value
                                        {
                                            dtc = (ASPxDateEdit)(bfc.Controls[0]);
                                        }
                                        if (dtc != null)
                                        {
                                            dtc.Value = spItem[fieldInternalName];
                                        }
                                    }
                                    break;
                                case "User"://Check users
                                case "UserField":
                                    if (item.GetType() == typeof(UserValueBox))
                                    {
                                        /*
                                        // Microsoft.SharePoint.WebControls.UserField usr = (Microsoft.SharePoint.WebControls.UserField)(bfc);
                                        UserValueBox usr = (UserValueBox)(item);
                                        if (fieldInternalName == DatabaseObjects.Columns.TicketOwner && UGITUtility.IsSPItemExist(newTicket, fieldInternalName))
                                        {
                                            //usr.Value = newTicket[fieldInternalName];
                                            usr.GetValues();//checked value 
                                        }
                                        else if (fieldInternalName == DatabaseObjects.Columns.OwnerUser && UGITUtility.IsSPItemExist(newTicket, fieldInternalName))
                                        {                                            
                                            usr.GetValues();
                                        }
                                        else
                                        {
                                            //  usr.Value = spItem[fieldInternalName];                                            
                                        }
                                        */

                                        UserValueBox usr = (UserValueBox)(item);
                                        if (fieldInternalName == DatabaseObjects.Columns.TicketOwner || fieldInternalName == DatabaseObjects.Columns.OwnerUser)
                                        {
                                            if (UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.TicketOwner))
                                            {
                                                usr.SetValues(Convert.ToString(spItem[DatabaseObjects.Columns.TicketOwner]));
                                            }
                                            else if (UGITUtility.IsSPItemExist(spItem, DatabaseObjects.Columns.OwnerUser))
                                            {
                                                usr.SetValues(Convert.ToString(spItem[DatabaseObjects.Columns.OwnerUser]));
                                            }
                                        }
                                        else
                                        {
                                            usr.SetValues(Convert.ToString(spItem[fieldInternalName]));
                                        }
                                    }
                                    break;
                                //  case SPFieldType.Lookup:
                                case "Lookup":

                                    if (item.GetType() == typeof(DropDownList))
                                    {
                                        string lookupID = UGITUtility.SplitString(spItem[fieldInternalName], ";#", 0);
                                        ((DropDownList)(item)).SelectedValue = lookupID;
                                    }
                                    //else if (bfc is CustomListDropDown)
                                    else if (item is LookUpValueBox)
                                    {
                                        string value = string.Empty;
                                        if (((LookUpValueBox)item).IsMulti)
                                        {
                                            value = Convert.ToString(spItem[fieldInternalName]);
                                        }
                                        else
                                        {
                                            value = Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spItem[fieldInternalName])));
                                        }

                                        ((LookUpValueBox)item).SetValues(value);
                                    }
                                    else if (bfc is CustomListDropDown)
                                    {
                                        string value = string.Empty;
                                        if (((CustomListDropDown)bfc).IsMult)
                                        {
                                            value = Convert.ToString(spItem[fieldInternalName]);
                                        }
                                        else
                                        {
                                            value = Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spItem[fieldInternalName])));
                                        }
                                        ((CustomListDropDown)bfc).SetValue(value);
                                    }
                                    //else if (item is CustomListDropDown)
                                    //{
                                    //    string value = string.Empty;
                                    //    if (((CustomListDropDown)item).IsMult)
                                    //    {
                                    //        value = Convert.ToString(spItem[fieldInternalName]);
                                    //    }
                                    //    else
                                    //    {
                                    //        value = Convert.ToString(UGITUtility.GetLookupID(Convert.ToString(spItem[fieldInternalName])));
                                    //    }

                                    //    ((CustomListDropDown)item).SetValue(value);
                                    //}


                                    else if (fieldInternalName == DatabaseObjects.Columns.TicketRequestTypeLookup)
                                    {
                                        if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
                                        {
                                            //if (bfc.Controls.Count > 0 && ((Panel)bfc.Controls[0].Controls[0]).Controls[1].Controls[0].Controls[0].Controls[0].Controls[0] is DropDownList)
                                            if (bfc.Controls.Count > 0 && (item is LookupValueBoxEdit))
                                            {
                                                LookupValueBoxEdit ddl = (LookupValueBoxEdit)(item);

                                                //((DropDownList)((Panel)bfc.Controls[0].Controls[0]).Controls[1].Controls[0].Controls[0].Controls[0].Controls[0]).SelectedValue = Convert.ToString(uHelper.GetLookupID(Convert.ToString(spItem[fieldInternalName])));
                                                //  string displayText = Convert.ToString(uHelper.GetLookupValue(Convert.ToString(spItem[fieldInternalName])));
                                                long requestType = 0;
                                                UGITUtility.IsNumber(Convert.ToString(spItem[fieldInternalName]), out requestType);
                                                var objRequestType = ObjRequestTypeManager.LoadByID(requestType);

                                                if (objRequestType != null)
                                                {
                                                    long reqType = ObjRequestTypeManager.Load(x => x.Category == objRequestType.Category && x.SubCategory == objRequestType.SubCategory && x.RequestType == objRequestType.RequestType && x.ModuleNameLookup == "OPM" && x.Deleted == false).Select(x => x.ID).FirstOrDefault();
                                                    if (reqType != 0)
                                                    {
                                                        ddl.Value = Convert.ToString(reqType);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxGridLookup))
                                    {

                                        string lookupID = UGITUtility.SplitString(spItem[fieldInternalName], ";#", 0);
                                        if (((ASPxGridLookup)(item)).GridView.JSProperties.Keys.Contains("cpValue"))
                                            ((ASPxGridLookup)(item)).GridView.JSProperties["cpValue"] = lookupID;
                                        else
                                            ((ASPxGridLookup)(item)).GridView.JSProperties.Add("cpValue", lookupID);

                                        ((ASPxGridLookup)(item)).GridView.Selection.SelectRowByKey(lookupID);
                                    }
                                    break;
                                case "Currency":
                                    {
                                        //  CurrencyField tt = (CurrencyField)(bfc);
                                        ASPxTextBox tt = (ASPxTextBox)(item);
                                        tt.Text = Convert.ToString(spItem[fieldInternalName]);
                                    }
                                    break;

                                case "Number":
                                    {
                                        //  if (bfc is NumberField)
                                        if (item is ASPxTextBox)
                                        {
                                            ASPxTextBox tt = (ASPxTextBox)(item);
                                            tt.Text = Convert.ToString(spItem[fieldInternalName]);
                                        }
                                    }
                                    break;
                                default:
                                    if (item is ASPxTextBox)
                                    {
                                        ASPxTextBox tt = (ASPxTextBox)(item);
                                        tt.Text = Convert.ToString(spItem[fieldInternalName]);
                                    }

                                    if (item is ASPxCheckBox)
                                    {
                                        ASPxCheckBox tt = (ASPxCheckBox)(item);
                                        tt.Checked = Convert.ToBoolean(spItem[fieldInternalName]);
                                    }

                                    break;
                            }
                        }
                    }
                    #endregion
                }

                else if (fieldInternalName == "#PlaceHolder#")
                {
                    Label label = new Label();
                    columnCount += fieldWidth;
                    if (columnCount > totalColumnsInDisplay)
                    {
                        columnCount = fieldWidth;
                        row = new TableRow();

                    }

                    if (tableType == Enums.TableDisplayTypes.Grouped)
                        addToTicketTable(row, label, string.Empty, fieldWidth, groupedTabTable);

                    else if (tableType == Enums.TableDisplayTypes.General)
                        addToTable(row, label, string.Empty, fieldWidth, newTicketTable);

                }
            }
            if (columnCount < totalColumnsInDisplay)
            {
                for (int j = columnCount; j < totalColumnsInDisplay; j++)
                {
                    if (tableType == Enums.TableDisplayTypes.Grouped)
                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                    else if (tableType == Enums.TableDisplayTypes.General)
                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, newTicketTable);
                }
            }

            ////When Attachment field control is not rendered on page, then its show some weird value in field control value.
            ////Add temporary attachment control will solve this issue.
            //if (newTicket != null)
            //{
            //    SPField f = thisList.Fields.GetFieldByInternalName(DatabaseObjects.Columns.Attachments);
            //    Control bfc = SPControls.GetSharePointControls(this.Context, f, thisList, newTicket, SPControlMode.Display, moduleId);
            //    bfc.ID += "_temp";
            //    tempAttachment.Controls.Add(bfc);
            //}
        }

        protected void CreateNewTicketFromTemplate(long templateID)
        {
            UGITModule module = ModuleViewManager.LoadByName(currentModuleName);
            UGITUtility.DeleteCookie(Request, Response, "ticketDivision");

            if (newTicket == null)
                newTicket = TicketManager.GetCachedModuleTableSchema(module).NewRow();

            //  TicketTemplateManager templateManager = new TicketTemplateManager(HttpContext.Current.GetManagerContext());
            TicketTemplate spListItem = TicketTemplateManager.LoadByID(templateID);  // SPListHelper.GetSPListItem(DatabaseObjects.Lists.TicketTemplates, templateID);

            if (spListItem == null)
                return;
            string fieldvalues = Convert.ToString(spListItem.FieldValues);

            if (fieldvalues == null)
                return;
            string[] values = fieldvalues.Split(new string[] { uGovernIT.Utility.Constants.Separator3 }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in values)
            {
                string value = string.Empty;
                string field = string.Empty;
                if (!string.IsNullOrEmpty(s))
                {
                    string[] attributes = s.Split(new string[] { uGovernIT.Utility.Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries);
                    if (attributes.Length == 3)
                    {
                        field = attributes[0];
                        value = attributes[2];
                        if (attributes[1] == "Date" || attributes[1] == "DateTime")
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                DateTime dtValue;
                                string addedToNowStr = ExpressionCalc.ExecuteFunctions(context, value);
                                if (DateTime.TryParse(addedToNowStr, out dtValue))
                                {
                                    value = addedToNowStr;
                                }
                                //string addedToNowStr = ExpressionCalc.GetParsedDateExpression(context,value);// value.Replace("f:adddays(Today,", string.Empty).Replace(")", string.Empty).Trim();
                                //int addToNow = 0;
                                //int.TryParse(addedToNowStr, out addToNow);
                                //int days;
                                //if (addToNow >= 0)
                                //{
                                //    days = addToNow;
                                //}
                                //else
                                //{
                                //    days = Math.Abs(addToNow);
                                //}
                                //value = addedToNowStr;// DateTime.Today.AddDays(days).ToShortDateString();
                            }
                        }
                    }
                    if (newTicket.Table.Columns.Contains(field))
                        newTicket[field] = value;
                }
            }

            //newTopGraphicPanel.Visible = false;
            //NewTicketWorkFlowDiv.Visible = !TicketRequest.Module.HideWorkFlow;
            //if (lifeCycle != null && lifeCycle.Stages.Count > 3)
            //{
            //    //newTopGraphicPanel.Visible = true;
            //    newTicketStageRepeater.DataSource = lifeCycle.Stages;
            //    newTicketStageRepeater.DataBind();
            //}

            NewTicketWorkFlowDiv.Visible = !TicketRequest.Module.HideWorkFlow;
            if (lifeCycle != null && lifeCycle.Stages.Count > 3)
            {
                newTicketStageRepeater.DataSource = lifeCycle.Stages;
                newTicketStageRepeater.DataBind();
            }


            if (lifeCycle != null && lifeCycle.Stages.Count > 0)
            {
                initialStageDescriptionLiteral.Text = lifeCycle.Stages[0].UserPrompt;
            }

            newTicketTable.Rows.Clear();
            newTicketTable.Controls.Clear();
            newTicketTable.BorderWidth = 0;
            newTicketTable.BorderStyle = BorderStyle.Solid;
            int columnCount = 0, fieldWidth = 1;
            TableRow row = new TableRow();
            row.BorderStyle = BorderStyle.None;
            row.BorderWidth = 0;

            List<ModuleFormLayout> collFormLayoutItems = TicketRequest.Module.List_FormLayout.Where(x => x.TabId == 0 && x.FieldSequence > 0).OrderBy(x => x.FieldSequence).ToList();

            Table mainTable = tabTable;
            Enums.TableDisplayTypes tableType = new Enums.TableDisplayTypes();
            tableType = Enums.TableDisplayTypes.General;
            string legend = string.Empty;
            int tabFieldSequence = 0;



            // Assign start stage of module ticket
            newTicket[DatabaseObjects.Columns.ModuleStepLookup] = newTicketStageId;

            //Get and set default value of module for item
            TicketRequest.AssignModuleSpecificDefaults(newTicket);

            //Get first field sequence from formlayout
            var sequenceGroupStart = collFormLayoutItems.Select(x => x.FieldSequence).FirstOrDefault();

            //Create New ticket form layout
            foreach (ModuleFormLayout tabField in collFormLayoutItems)
            {
                fieldWidth = tabField.FieldDisplayWidth;
                string fieldDisplayText = tabField.FieldDisplayName;
                string fieldInternalName = tabField.FieldName;


                if (fieldInternalName == "#GroupStart#" || fieldInternalName == "#GroupEnd#")
                {
                    if (fieldInternalName == "#GroupStart#")
                    {
                        tableType = Enums.TableDisplayTypes.Grouped;
                        groupedTabTable = new Table();
                        var groupedTabCell = new TableCell();
                        var groupTabRow = new TableRow();
                        groupedTabTable.Attributes.Add("style", "width:100%");
                        legend = fieldDisplayText;
                        tabFieldSequence = tabField.FieldSequence;

                    }
                    else if (fieldInternalName == "#GroupEnd#")
                    {
                        CreateShowhideSectionHyperLink(tabFieldSequence, sequenceGroupStart, legend);
                        tableType = Enums.TableDisplayTypes.General;
                    }
                    row = new TableRow();
                    row.BorderStyle = BorderStyle.None;
                    row.BorderWidth = 0;
                }
                else if (TicketSchema.Columns.Contains(fieldInternalName))
                {
                    //SPField f = thisList.Fields.GetFieldByInternalName(fieldInternalName);
                    string value = values.FirstOrDefault(x => x.StartsWith(fieldInternalName));
                    string type = string.Empty;
                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] attributes = value.Split(new string[] { uGovernIT.Utility.Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries);
                        if (attributes.Length == 3)
                        {
                            type = attributes[1];
                            value = attributes[2];
                        }
                    }
                    Control bfc = null;
                    //Control bfc = uGovernIT.Helpers.SPControls.GetSharePointControls(this.Context, f, thisList, newTicket, SPControlMode.New, moduleId, parentTicket);
                    //if (bfc != null)
                    //{
                    ticketControls = new TicketControls(module, newTicket);
                    ticketControls.SourceItem = newTicket;
                    ModuleRoleWriteAccess ticketRoleWriteAccessTemp = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStep || x.StageStep == 0) && x.FieldName == fieldInternalName);
                    try
                    {
                        bool showWithCheckBox = false;

                        if (ticketRoleWriteAccessTemp != null)
                        {
                            showWithCheckBox = ticketRoleWriteAccessTemp.ShowWithCheckbox;
                            bfc = ticketControls.GetControls(TicketSchema.Columns[fieldInternalName], ControlMode.New, FieldDesignMode.Normal, "", tabField); //uGovernIT.Helpers.SPControls.GetSharePointControls(this.Context, f, thisList, newTicket, SPControlMode.New, moduleId, false, showWithCheckBox, newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), ""), newTicket);
                            if (fieldDisplayText.Contains("Analytics"))
                            {
                                fieldDisplayText += "<img src='/Content/images/analytics.png' />";
                            }
                            if (ticketRoleWriteAccessTemp.FieldMandatory)
                            {
                                fieldDisplayText += "<b style=\"color:red\">* </b>";
                            }
                        }
                        else
                        {
                            bfc = ticketControls.GetControls(TicketSchema.Columns[fieldInternalName], ControlMode.Display, FieldDesignMode.WithEdit, "", tabField); // uGovernIT.Helpers.SPControls.GetSharePointControls(this.Context, f, thisList, newTicket, SPControlMode.Display, moduleId, newTicket);
                        }
                        if (TicketSchema.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                        {
                            requestOwnerId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ClientID;
                        }
                        else if (TicketSchema.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                        {
                            priorityId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ClientID;
                        }
                        else if (TicketSchema.Columns.Contains(DatabaseObjects.Columns.Attachments))
                        {
                            //Add Custom attachment option for new ticket when user can add multiple attachment
                            int maxNoOfFiles = Convert.ToInt32(ConfigurationVariableManager.GetValue("NewTicketMaxFiles"));
                            HtmlTable table = new HtmlTable();
                            HtmlTableRow tr = new HtmlTableRow();
                            HtmlTableCell td = new HtmlTableCell();
                            tr.Attributes.Add("id", "fileContainer0");
                            table.Attributes.Add("class", "fileuploaderContainer");
                            FileUpload fileUpload = new FileUpload();
                            fileUpload.CssClass = "ms-fileinput inputFile";
                            fileUpload.ID = "fileupload" + 0;
                            td.Controls.Add(fileUpload);
                            tr.Cells.Add(td);
                            LinkButton delete = new LinkButton();
                            delete.Text = "Delete";
                            delete.Attributes.Add("href", "javascript:");
                            delete.CssClass = "deletefiles";
                            delete.OnClientClick = "deleteFileFromUpload(this, 0," + maxNoOfFiles + "); return false;";
                            td = new HtmlTableCell();
                            td.Controls.Add(delete);
                            tr.Cells.Add(td);
                            tr.Style.Add("display", "block");
                            table.Rows.Add(tr);

                            for (int i = 1; i < maxNoOfFiles; i++)
                            {
                                tr = new HtmlTableRow();
                                td = new HtmlTableCell();
                                tr.Attributes.Add("id", "fileContainer" + i);
                                tr.Style.Add("display", "none");
                                FileUpload fileUploadHidden = new FileUpload();
                                fileUploadHidden.CssClass = "ms-fileinput";
                                fileUploadHidden.ID = "fileupload" + i;

                                td.Controls.Add(fileUploadHidden);
                                tr.Cells.Add(td);

                                // add delete link button
                                delete = new LinkButton();
                                delete.Text = "Delete";
                                delete.CssClass = "deletefiles";
                                delete.OnClientClick = "deleteFileFromUpload(this," + i + "," + maxNoOfFiles + "); return false;";
                                delete.Attributes.Add("href", "javascript:");
                                td = new HtmlTableCell();
                                td.Controls.Add(delete);

                                tr.Cells.Add(td);

                                table.Rows.Add(tr);
                            }


                            // add "Add More Files" link button at the bottom.
                            tr = new HtmlTableRow();
                            LinkButton addMore = new LinkButton();
                            addMore.Text = "Add More Files";
                            addMore.Attributes.Add("href", "javascript:");
                            addMore.OnClientClick = "addMoreFilesForUpload(this, 0, " + maxNoOfFiles + ");return false;";
                            addMore.CssClass = "addmorefiles";
                            td = new HtmlTableCell();
                            td.Controls.Add(addMore);
                            tr.Cells.Add(td);
                            table.Rows.Add(tr);

                            HtmlTableRow bfcRow = new HtmlTableRow();
                            HtmlTableCell bfcCell = new HtmlTableCell();
                            bfcRow.Cells.Add(bfcCell);
                            bfcCell.Controls.Add(bfc);
                            table.Rows.Add(bfcRow);
                            bfc = table;
                        }
                        else if (fieldDisplayText.Contains("Requested "))
                        {
                            requestorContainerId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ID;
                        }
                        else if (fieldDisplayText.Contains("Related Asset"))
                        {
                            relatedAssetContainerId = newTicketTable.ClientID.Replace(newTicketTable.ID.ToString(), "") + bfc.ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log.WriteException(ex, "ModuleWebPart- CreateNetTicket()");
                        Util.Log.ULog.WriteException(ex);
                    }

                    columnCount += fieldWidth;
                    if (columnCount > totalColumnsInDisplay)
                    {
                        for (int j = columnCount - fieldWidth; j < totalColumnsInDisplay; j++)
                        {
                            if (tableType == Enums.TableDisplayTypes.Grouped)
                                addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                            else if (tableType == Enums.TableDisplayTypes.General)
                                addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, newTicketTable);
                        }
                        columnCount = fieldWidth;
                        row = new TableRow();
                        row.BorderStyle = BorderStyle.Solid;
                        row.BorderWidth = 1;
                    }


                    if (fieldInternalName == DatabaseObjects.Columns.ResolutionComments
                        || fieldInternalName == DatabaseObjects.Columns.TicketBusinessManager
                        || fieldInternalName == DatabaseObjects.Columns.TicketGLCode
                        || fieldInternalName == DatabaseObjects.Columns.TicketLegal
                        || fieldInternalName == DatabaseObjects.Columns.TicketFinanceManager
                        || fieldInternalName == DatabaseObjects.Columns.TicketPurchasing
                        || fieldInternalName == DatabaseObjects.Columns.TicketTester)
                    {
                        row.CssClass += " " + fieldInternalName + "Row";
                    }
                    if (tableType == Enums.TableDisplayTypes.Grouped)
                        //                      addToTable(row, bfc, fieldDisplayText, fieldWidth, groupedTabTable);
                        addToTicketTable(row, bfc, fieldDisplayText, fieldWidth, groupedTabTable, sequenceGroupStart, tabFieldSequence);

                    else if (tableType == Enums.TableDisplayTypes.General)
                        addToTable(row, bfc, fieldDisplayText, fieldWidth, newTicketTable);


                }
            }
            if (columnCount < totalColumnsInDisplay)
            {
                for (int j = columnCount; j < totalColumnsInDisplay; j++)
                {
                    if (tableType == Enums.TableDisplayTypes.Grouped)
                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, groupedTabTable);
                    else if (tableType == Enums.TableDisplayTypes.General)
                        addToTable(row, "&nbsp;", "&nbsp;", "&nbsp;", 1, newTicketTable);
                }
            }
        }

        protected void DdlTicketBaselines_PreRender(object sender, EventArgs e)
        {
            if (showBaselineButtons && currentTicketId > 0)
            {
                ddlTicketBaselines.Items.Clear();
                //Pass ticket id
                List<Baseline> baselines = Baseline.GetBaselines(Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]), context).OrderByDescending(x => x.BaselineDate).ToList();

                foreach (Baseline baseline in baselines)
                {
                    string comment = UGITUtility.ReplaceInvalidCharsInURL(UGITUtility.TruncateWithEllipsis(baseline.BaselineComment, 50, ""));
                    ListItem item = new ListItem(string.Format("{0} ({1})", baseline.BaselineDate.ToString("MMM dd yyyy h:mm tt"), comment), string.Format("{0}##{1}", baseline.BaselineNum.ToString(), baseline.BaselineDate.ToString("MMM dd yyyy h:mm tt")));
                    ddlTicketBaselines.Items.Add(item);
                    item.Attributes.Add("title", baseline.BaselineComment);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (currentTicket != null && UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.LocationLookup))
            {
                //FieldLookupValue lookupVal =  FieldLookupValue(Convert.ToString((UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.LocationLookup)));
                requestorLocation = "";//lookupVal.LookupId > 0 ? lookupVal.LookupId.ToString() : string.Empty;
            }

            //hide buttons when showing baseline data of PMM
            if (currentModuleName == "PMM" && Request["showBaseline"] != null && UGITUtility.StringToBoolean(Request["showBaseline"].Trim()) && Request["baselineNum"] != null)
            {
                //showReportLI.Visible = false;
                approveButtonLI.Visible = false;
                //HoldButtonLI.Visible = false;
                returnButtonLI.Visible = false;
                saveAsDraftButton.Visible = false;
                // createBaselineLI.Visible = false;
                rejectButtonLI.Visible = false;
                //  notificationButtonLI.Visible = false;
                updateButtonLI.Visible = false;
                //  UnHoldButtonLI.Visible = false;
                //  createAssetButtonLI.Visible = false;
                importPMMLI.Visible = false;
            }



            base.OnPreRender(e);
        }

        protected void StageRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LifeCycleStage stage = (LifeCycleStage)e.Item.DataItem;
                Repeater repeator = (Repeater)sender;
                List<LifeCycleStage> allStages = (List<LifeCycleStage>)repeator.DataSource;
                int stageIdex = allStages.IndexOf(stage);
                int wfCurrentStep = currentStage.StageStep;
                LifeCycleStage cStage = allStages.FirstOrDefault(x => x.ID == currentStage.ID);
                if (cStage != null)
                    wfCurrentStep = cStage.StageStep;

                HtmlTableCell tdStage = (HtmlTableCell)e.Item.FindControl("tdStage");
                Literal lbStageNumber = (Literal)e.Item.FindControl("lbStageNumber");
                HtmlGenericControl stageTitleContainer = (HtmlGenericControl)e.Item.FindControl("stageTitleContainer");
                Literal stageTitle = (Literal)e.Item.FindControl("stageTitle");
                HtmlGenericControl activeStageArrow = (HtmlGenericControl)e.Item.FindControl("activeStageArrow");
                HtmlTableCell tdStepLine = (HtmlTableCell)e.Item.FindControl("tdStepLine");
                HtmlImage tdStepLineimg = (HtmlImage)e.Item.FindControl("tdStepLineimg");
                HtmlGenericControl tdshowallrelatedticketdiv = (HtmlGenericControl)e.Item.FindControl("tdshowallrelatedticketdiv");
                HtmlGenericControl tdshowallrelatedticketbeforediv = (HtmlGenericControl)e.Item.FindControl("tdshowallrelatedticketbeforediv");
                HtmlImage imgAgent = (HtmlImage)e.Item.FindControl("imgAgent");
                HtmlGenericControl lblAgent = (HtmlGenericControl)e.Item.FindControl("lblAgent");
                LifeCycleStage localstagerealstagestep = new LifeCycleStageManager(context).LoadByID(stage.ID);
                HtmlGenericControl stageNo = (HtmlGenericControl)e.Item.FindControl("stageNo");

                if (currentModuleName == "SVC")
                {
                    string stageNameToShowTaskConfig = (ConfigurationVariableManager.GetValue(ConfigConstants.NameOfWorkflowStepToShowTask)).ToString();
                    var arrStageNameToShowTask = stageNameToShowTaskConfig.Split(';');
                    foreach (string stageName in arrStageNameToShowTask)
                    {
                        if (stage.StageTitle.Equals(stageName, StringComparison.InvariantCultureIgnoreCase))
                            tdStage.Attributes.Add("onclick", "gotoTaskWorkFlow()");
                    }

                }
                if (tdshowallrelatedticketdiv != null)
                {
                    if (stage.StageStep == 1)
                        tdshowallrelatedticketbeforediv.Attributes.Add("class", "stagerelatedicon stageicon_" + 0);

                    tdshowallrelatedticketdiv.Attributes.Add("class", "stagerelatedicon stageicon_" + stage.StageStep);
                    tdshowallrelatedticketdiv.Attributes.Add("style", "position: unset;");
                }
                //1,2,3,4,5,6,7,8
                //1,2,3,4,5,6,7
                //currentstage = 3

                if (stage.StageStep < wfCurrentStep)
                {
                    //       tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg_visited.gif");
                    tdStage.Attributes.Add("class", "node nodeComplete");
                    stageNo.Attributes.Add("class", "node-stageno textWhite");

                }
                else if (stage.StageStep == wfCurrentStep)
                {
                    if (ticketOnHold != 1)
                    {
                        //   tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg_active.gif");
                        tdStage.Attributes.Add("class", "node nodeInprogress");
                        stageNo.Attributes.Add("class", "node-stageno textWhite");
                    }
                    else
                    {
                        //   tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg-red.gif");
                        tdStage.Attributes.Add("class", "node nodeComplete");
                        stageNo.Attributes.Add("class", "node-stageno textWhite");
                    }
                }
                else
                {
                    //    tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg.gif");
                    tdStage.Attributes.Add("class", "node");
                    stageNo.Attributes.Add("class", "node-stageno textBlack");
                }
                if (agentSummaryOnLoad != null)
                {
                    lblAgent.InnerText = agentSummaryOnLoad.AgentName;
                    var steps = agentSummaryOnLoad.StageSteps.Split(';');
                    if (steps != null && steps.Length > 0)
                    {
                        if (steps[0] == localstagerealstagestep.StageStep.ToString() && agentSummaryOnLoad.AgentType != "ResetPassword")
                        {
                            imgAgent.Visible = true;
                            lblAgent.Visible = true;
                        }

                        if (steps.Contains(localstagerealstagestep.StageStep.ToString()) && agentSummaryOnLoad.AgentType != "ResetPassword")
                        {
                            tdStage.Attributes.Add("class", "node nodeAgentComplete");
                            stageNo.Attributes.Add("class", "node-stageno textWhite");
                        }
                        if (agentSummaryOnLoad.AgentType == "ResetPassword")
                        {
                            if (localstagerealstagestep.StageTypeChoice == "Closed")
                            {
                                imgAgent.Visible = true;
                                lblAgent.Visible = true;
                            }
                            if (steps.Contains(localstagerealstagestep.StageStep.ToString()) && localstagerealstagestep.StageStep >= 4)
                            {
                                tdStage.Attributes.Add("class", "node nodeAgentComplete");
                                stageNo.Attributes.Add("class", "node-stageno textWhite");
                            }
                        }
                    }
                }

                tdStage.Attributes.Add("title", stage.StageTitle);

                lbStageNumber.Text = Convert.ToString(stage.StageStep);
                if (wfCurrentStep % 2 != 0)
                {
                    if ((stage.StageStep % 2 == 0 && setAlternateStageGraphicLabel))
                    {
                        stageTitleContainer.Attributes.Add("class", "stage-titlecontainer alternategraphiclabel workflow-lable");
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "-28px");
                    }
                    else
                    {
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "40px");
                    }
                }
                else
                {
                    if (stage.StageStep % 2 != 0 && setAlternateStageGraphicLabel)
                    {
                        stageTitleContainer.Attributes.Add("class", "stage-titlecontainer alternategraphiclabel workflow-lable");
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "-28px");
                    }
                    else
                    {
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "40px");
                    }
                }

                if (stage.StageStep > 9)
                {
                    if (stageNo != null)
                        stageNo.Style.Add("left", "6px");
                }

                stageTitle.Text = stage.StageTitle;

                if (stage.StageStep == wfCurrentStep)
                {
                    activeStageArrow.Attributes.Add("class", "activestagearrow oldactiveStageArrow");
                }

                if (stage.StageStep < wfCurrentStep)
                {
                    if (stage.StageStep == 1)
                    {
                        //tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline_active.gif");
                        tdStepLine.Attributes.Add("class", "step complete");
                    }
                    else
                    {
                        tdStepLine.Attributes.Add("class", "step complete");
                        //tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline_active.gif");
                    }
                }
                //else if (stage.StageStep == wfCurrentStep && currentModuleName == "PMM")
                //{
                //    // Show progress between stages for PMM
                //    tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline.gif");
                //    DataTable currentTicketTasks = TaskCache.GetAllTasksByProjectID("PMM", currentTicketPublicID);
                //    DataRow dr = currentTicketTasks.AsEnumerable().Where(x => x.Field<int>(DatabaseObjects.Columns.StageStep) == wfCurrentStep).FirstOrDefault();
                //    double percentComplete = -1;
                //    if (dr != null)
                //        double.TryParse(Convert.ToString(dr[DatabaseObjects.Columns.PercentComplete]), out percentComplete);

                //    if (percentComplete > 0D && percentComplete < 100D)
                //    {
                //        double totalweight = ((Convert.ToInt32(stage.Weight) / totalModuleWeight) * 1000);
                //        if (tdStepLineimg != null)
                //            tdStepLineimg.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", ((totalweight * percentComplete) / 100)));
                //        tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg_active_inprogress.gif");
                //    }
                //    else
                //    {
                //        tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline.gif");
                //        if (tdStepLineimg != null)
                //            tdStepLineimg.Style.Add(HtmlTextWriterStyle.Display, "none");
                //    }
                //}
                else
                {
                    //tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline.gif");
                    tdStepLine.Attributes.Add("class", "step inprogress");
                    if (tdStepLineimg != null)
                        tdStepLineimg.Style.Add(HtmlTextWriterStyle.Display, "none");
                }


                if (totalModuleWeight > 0)
                {
                    tdStepLine.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", (((Convert.ToInt32(stage.StageWeight) / totalModuleWeight) * 1000))));
                    //tdStepLineimg.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", (((Convert.ToInt32(stage.Weight) / totalModuleWeight) * 1000))));
                }
                else
                {
                    tdStepLine.Style.Add(HtmlTextWriterStyle.Width, "100px");
                }

                if (stageIdex == allStages.Count - 1)
                {
                    tdStepLine.Visible = false;
                }

            }
        }

        #endregion

        #region Helper Methods
        private void addToTable(TableRow row, Control ctrl, string labelName, int width, Control validator)
        {
            addToTable(row, ctrl, labelName, width, tabTable, validator);
        }

        private void addToTable(TableRow row, Control ctrl, string labelName, int width)
        {
            //addToTable(row, ctrl, labelName, width, tabTable);
            addToTicketTable(row, ctrl, labelName, width, tabTable);
        }

        private void addToTable(TableRow row, Control ctrl, int width)
        {
            if (ctrl == null)
                return;

            row.CssClass = "ugit-tdetaillabel whiteborder";
            row.BorderStyle = BorderStyle.Solid;
            TableCell newCell = new TableCell();
            //TableCell newCellType = new TableCell();

            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            //newCellType.BorderStyle = BorderStyle.None;
            //newCellType.BorderWidth = 0;
            newCell.Controls.Add(ctrl);
            newCell.ForeColor = System.Drawing.Color.Black;

            newCell.Height = 30;
            newCell.CssClass = "tableCell ugit-tdetaillabelval";
            newCell.ColumnSpan = (width * 2);
            if (newCell.ColumnSpan > 0)
                newCell.Width = new Unit(16 * newCell.ColumnSpan, UnitType.Percentage);
            else
                newCell.Width = new Unit(16, UnitType.Percentage);

            row.Cells.Add(newCell);
            tabTable.Rows.Add(row);
        }

        private void addToTableInSameCellTableLayout(TableRow row, Control ctrl, int width, Table table)
        {
            addToTableInSameCellTableLayout(row, ctrl, width, table, false);
        }

        private void addToTableInSameCellTableLayout(TableRow row, Control ctrl, int width, Table table, bool isLabel)
        {

            tableViewRowCount++;
            row.CssClass = "ugit-tdetaillabel";
            TableCell newCell = new TableCell();
            //newCell.BorderStyle = BorderStyle.NotSet;
            //newCell.BorderColor = Color.Black;
            newCell.Controls.Add(ctrl);
            if (isLabel)
            {
                newCell.CssClass = " labelCell ugit-tdetaillabel pmm-constraints-table-td";
            }
            else
            {
                newCell.CssClass = " ugit-tdetaillabelval";
            }
            row.Cells.Add(newCell);


        }

        private void addToTable(TableRow row, Control ctrl, string labelName, int width, Table table, string fieldinternalname = "")
        {
            if (ctrl == null)
                return;

            row.CssClass += " whiteborder duplicateSvc_popupTr";
            row.BorderStyle = BorderStyle.Solid;
            TableCell newCell = new TableCell();
            TableCell newCellType = new TableCell();

            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            newCell.CssClass = "duplicateSvc_popupInputTd";
            newCellType.BorderStyle = BorderStyle.None;
            newCellType.BorderWidth = 0;

            newCellType.Text = labelName;
            //newCell.BackColor = System.Drawing.Color.White;
            newCellType.ForeColor = System.Drawing.Color.Black;
            newCellType.Font.Bold = true;
            newCellType.CssClass += " tableCell labelCell ugit-tdetaillabel whiteborder duplicateSvc_popupTd";



            if (newCellType.ColumnSpan > 0)
                newCellType.Width = new Unit(16 * newCellType.ColumnSpan, UnitType.Percentage);
            else
                newCellType.Width = new Unit(16, UnitType.Percentage);

            newCellType.Height = 30;

            newCell.Controls.Add(ctrl);
            newCell.ForeColor = System.Drawing.Color.Black;

            newCell.Height = 30;
            newCell.CssClass += " tableCell ugit-tdetaillabelval";
            newCell.ColumnSpan = (width * 2) - 1;

            if (newCell.ColumnSpan > 0)
                newCell.Width = new Unit(16 * newCell.ColumnSpan, UnitType.Percentage);
            else
                newCell.Width = new Unit(16, UnitType.Percentage);

            row.Cells.Add(newCellType);
            row.Cells.Add(newCell);
            table.Rows.Add(row);

        }

        private void addToTicketTable(TableRow row, Control ctrl, string labelName, int width, Table table, int? sequence = null, int? tabSequence = null, string fieldinternalname = "")
        {
            if (ctrl == null)
                return;

            if (tabSequence != sequence)
            {
                table.CssClass = "ticket_container new-ticket-table hideAndShowTables";
            }

            else
                table.CssClass = "ticket_container new-ticket-table";
            // table.ID = "sectionHideShow";

            row.BorderStyle = BorderStyle.None;
            TableCell newCell = new TableCell();
            Panel newCellType = new Panel();

            lblText = new Label();
            lblText.CssClass = "field_box_label";
            newCell.CssClass = "field_box block_td";
            newCellType.CssClass = "flex-container";

            lblText.Text = labelName;
            Label lblShort = new Label();
            lblShort.ID = "lblShortId";
            lblShort.CssClass = "lblShortNamecss";

            newCellType.Controls.Add(lblText);

            if (fieldinternalname == "ShortName")
            {
                ShortNameLength = UGITUtility.StringToInt(ConfigurationVariableManager.GetValue(ConfigConstants.ShortNameCharacters));
                lblShort.Visible = true;
                lblShort.Text = @"<font color = red > Max " + ShortNameLength + " characters are allowed </font>";
                newCellType.Controls.Add(lblShort);

            }
             

            newCell.ColumnSpan = width * 2;
            if (!string.IsNullOrWhiteSpace(fieldinternalname) && fieldinternalname == DatabaseObjects.Columns.TicketRequestor &&
            (currentModuleName == "TSR" || currentModuleName == "PRS" || currentModuleName == "SVC" || currentModuleName == "ACR"))
            {
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                img.ToolTip = "Recent Tickets";
                img.CssClass += " timelineopencloseedit";
                img.ImageUrl = "/content/images/timeline.png";
                img.Attributes.Add("onclick", "javascript:ShowRequestorOpenClosedTickets();");
                newCellType.Controls.Add(img);
            }
            newCell.Controls.Add(newCellType);
            newCell.Controls.Add(ctrl);

            row.Cells.Add(newCell);
            table.Rows.Add(row);
        }

        private void addToTable(TableRow row, Control ctrl, string labelName, int width, Table table, Control validator)
        {
            row.CssClass += " ugit-tdetaillabel whiteborder";
            row.BorderStyle = BorderStyle.Solid;
            TableCell newCell = new TableCell();
            TableCell newCellType = new TableCell();

            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            newCellType.BorderStyle = BorderStyle.None;
            newCellType.BorderWidth = 0;

            newCellType.Text = labelName;
            //newCell.BackColor = System.Drawing.Color.White;
            newCellType.ForeColor = System.Drawing.Color.Black;
            newCellType.Font.Bold = true;
            newCellType.CssClass += " tableCell labelCell ugit-tdetaillabel whiteborder";


            if (newCellType.ColumnSpan > 0)
                newCellType.Width = new Unit(16 * newCellType.ColumnSpan, UnitType.Percentage);
            else
                newCellType.Width = new Unit(16, UnitType.Percentage);
            newCellType.Height = 30;

            newCell.Controls.Add(ctrl);
            newCell.ForeColor = System.Drawing.Color.Black;
            newCell.Width = 200;
            newCell.Height = 30;
            newCell.CssClass += " tableCell ugit-tdetaillabelval";
            newCell.ColumnSpan = (width * 2) - 1;

            if (newCell.ColumnSpan > 0)
                newCell.Width = new Unit(16 * newCell.ColumnSpan, UnitType.Percentage);
            else
                newCell.Width = new Unit(16, UnitType.Percentage);

            if (validator != null)
            {
                newCell.Controls.Add(validator);
            }
            row.Cells.Add(newCellType);
            row.Cells.Add(newCell);
            table.Rows.Add(row);
        }

        private void addToTicketTable(TableRow row, Control ctrl, string labelName, int width, Table table, Control validator)
        {
            table.CssClass = "ticket_container new-ticket-table";
            row.BorderStyle = BorderStyle.None;
            TableCell newCell = new TableCell();

            lblText = new Label();
            lblText.CssClass = "field_box_label";
            newCell.CssClass = "field_box block_td";

            lblText.Text = labelName;

            newCell.Controls.Add(lblText);
            newCell.Controls.Add(ctrl);

            newCell.ColumnSpan = width * 2;

            if (validator != null)
            {
                newCell.Controls.Add(validator);
            }

            row.Cells.Add(newCell);
            table.Rows.Add(row);

            /*
            row.CssClass += " ugit-tdetaillabel whiteborder";
            row.BorderStyle = BorderStyle.Solid;
            TableCell newCell = new TableCell();
            TableCell newCellType = new TableCell();

            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            newCellType.BorderStyle = BorderStyle.None;
            newCellType.BorderWidth = 0;

            newCellType.Text = labelName;
            //newCell.BackColor = System.Drawing.Color.White;
            newCellType.ForeColor = System.Drawing.Color.Black;
            newCellType.Font.Bold = true;
            newCellType.CssClass += " tableCell labelCell ugit-tdetaillabel whiteborder";


            if (newCellType.ColumnSpan > 0)
                newCellType.Width = new Unit(16 * newCellType.ColumnSpan, UnitType.Percentage);
            else
                newCellType.Width = new Unit(16, UnitType.Percentage);
            newCellType.Height = 30;

            newCell.Controls.Add(ctrl);
            newCell.ForeColor = System.Drawing.Color.Black;
            newCell.Width = 200;
            newCell.Height = 30;
            newCell.CssClass += " tableCell ugit-tdetaillabelval";
            newCell.ColumnSpan = (width * 2) - 1;

            if (newCell.ColumnSpan > 0)
                newCell.Width = new Unit(16 * newCell.ColumnSpan, UnitType.Percentage);
            else
                newCell.Width = new Unit(16, UnitType.Percentage);

            if (validator != null)
            {
                newCell.Controls.Add(validator);
            }
            row.Cells.Add(newCellType);
            row.Cells.Add(newCell);
            table.Rows.Add(row);
            */
        }

        private void addToTable(TableRow row, string value, string labelName, string type, int width)
        {
            addToTable(row, value, labelName, type, width, tabTable);
        }

        private void addToTable(TableRow row, string value, string labelName, string type, int width, Table table)
        {
            row.CssClass += " ";
            TableCell newCell = new TableCell();
            Panel newCellType = new Panel();
            Panel controlPanel = new Panel();

            Label lbName = new Label();
            lbName.Text = labelName;
            newCellType.Controls.Add(lbName);
            newCell.Controls.Add(newCellType);
            //if (newCellType.ColumnSpan > 0)
            //    newCellType.Width = new Unit(16 * newCellType.ColumnSpan, UnitType.Percentage);
            //else
            //    newCellType.Width = new Unit(16, UnitType.Percentage);

            //newCellType.Height = 30;
            newCellType.CssClass += " labelCell ";
            Label lbValue = new Label();
            lbValue.Text = value;
            controlPanel.Controls.Add(lbValue);
            newCell.Controls.Add(controlPanel);
            //newCell.Height = 30;
            if (type == string.Empty)
            {
                newCell.ForeColor = ColorTranslator.FromHtml("#4a90e2");
                newCell.ColumnSpan = (width * 2);
                newCell.CssClass = "tableCell ";
            }

            if (newCell.ColumnSpan > 0)
                newCell.Width = new Unit(16 * newCell.ColumnSpan, UnitType.Percentage);
            else
            {
                newCell.Width = new Unit(16, UnitType.Percentage);
                newCell.CssClass = "field_box";
            }
            row.Cells.Add(newCell);

            table.Rows.Add(row);

            //row.CssClass += " ugit-tdetaillabel whiteborder";
            //row.BorderStyle = BorderStyle.Solid;
            //TableCell newCell = new TableCell();
            //TableCell newCellType = new TableCell();

            //newCell.BorderStyle = BorderStyle.None;
            //newCell.BorderWidth = 0;
            //newCellType.BorderStyle = BorderStyle.None;
            //newCellType.BorderWidth = 0;

            //newCellType.Text = labelName;
            //if (newCellType.ColumnSpan > 0)
            //    newCellType.Width = new Unit(16 * newCellType.ColumnSpan, UnitType.Percentage);
            //else
            //    newCellType.Width = new Unit(16, UnitType.Percentage);

            //newCellType.Height = 30;
            //newCellType.CssClass += " tableCell labelCell ugit-tdetaillabel whiteborder";
            //newCell.Text = value;
            //newCell.Height = 30;
            //if (type == string.Empty)
            //{
            //    newCell.ForeColor = System.Drawing.Color.Black;
            //    newCell.ColumnSpan = (width * 2) - 1;
            //    newCell.CssClass = "tableCell ugit-tdetaillabelval";
            //}

            //if (newCell.ColumnSpan > 0)
            //    newCell.Width = new Unit(16 * newCell.ColumnSpan, UnitType.Percentage);
            //else
            //    newCell.Width = new Unit(16, UnitType.Percentage);
            //row.Cells.Add(newCellType);
            //row.Cells.Add(newCell);

            //table.Rows.Add(row);

        }
        //Commented below code
        private void addToCommentsTable(TableRow row, HistoryEntry value, string labelName, int width, Table table)
        {
            table.CssClass = "col-md-12 col-sm-12 col-xs-12 information-wrap comment-tbl";
            row.CssClass = "row comment-wrap comment-container";

            TableCell newCell = new TableCell();
            newCell.CssClass = "col-md-12 col-sm-12 col-xs-12 verticalAlign";

            sbText.Clear();
            sbText.AppendFormat("<div class='row comment-wrap'>");

            sbText.AppendFormat("</div>{0}", labelName);


            sbText.AppendFormat("<div class='col-md-3 col-xs-3' style='padding:5px !important'>");
            sbText.AppendFormat("<div class='img-container'>");
            bool fileExists = (File.Exists(Server.MapPath(value.Picture)) ? true : false);
            if (fileExists)
            {
                sbText.AppendFormat("<img src='{0}'", value.Picture);
            }
            else
            {
                sbText.AppendFormat("<img src='{0}'", "/Content/images/useravtar64x64.png");
            }
            sbText.AppendFormat("<label class='img-name'>{0}</label>", value.createdBy);

            sbText.AppendFormat("</div>");


            sbText.AppendFormat("</div>");



            //sbText.AppendFormat("<div class='col-md-4 col-xs-4 comment-padding'>");
            //sbText.AppendFormat("<div class='comment-data'>");




            //sbText.AppendFormat("</div>");
            if (value.created != null)
            {
                sbText.AppendFormat("<div class='col-md-3 col-xs-3 comment-days' style='padding:5px !important'>");
                if (value.IsPrivate)
                {
                    sbText.AppendFormat("<img src='{0}'", "/Content/images/lock16X16.png");
                    sbText.Append("title='Private' alt='' >");
                }
                else
                {
                    sbText.AppendFormat("<img src='{0}'", "/Content/images/notify16X16.png");
                    sbText.Append("title='Notify Requestor' alt='' >");
                }

                sbText.AppendFormat("<span title='{0} day(s) ago' class='dateTimeLayout'>{1}</span>", DateTime.Today.Subtract(DateTime.Parse(value.created)).Days.ToString(), value.created);
                sbText.AppendFormat("</div>");

            }

            //sbText.AppendFormat("</div>");


            sbText.AppendFormat("<div class='col-md-3 col-xs-3 comment-data' style='padding:5px !important; width:45%'>");
            sbText.AppendFormat("<p>{0}</p>", value.entry);
            sbText.AppendFormat("</div>");

            //sbText.AppendFormat("<div class='col-md-2 col-xs-12 comment-padding'>");
            //sbText.AppendFormat("<div class='comment-days'>");
            //sbText.AppendFormat("<span title='{0} day(s) ago' class='dateTimeLayout'>{1}</span>", DateTime.Today.Subtract(DateTime.Parse(value.created)).Days.ToString(), value.created);
            //sbText.AppendFormat("</div>");

            //sbText.AppendFormat("</div>");

            newCell.Text = sbText.ToString();
            /*
            newCell.Text = $"<div class='row comment-wrap'>" +
                                $"<div class='col-md-1 comment-padding'>" +
                                    $"<div class='img-container'>" +
                                        $"<img src='{value.Picture}'>" +
                                        $"<label class='img-name'>{value.createdBy}</label>" +
                                    $"</div>" +
                                $"</div>" +
                                $"<div class='col-md-9 comment-padding'>" +
                                    $"<div class='comment-data'>" +
                                        $"<p>{value.entry}</p>" +
                                    $"</div>" +
                                $"</div>" +
                                $"<div class='col-md-2 comment-padding'>" +
                                    $"<div class='comment-days'>" +
                                        $"<span title='{ DateTime.Today.Subtract(DateTime.Parse(value.created)).Days.ToString()} day(s) ago' style='cursor:pointer;'>{value.created}</span>" +
                                    $"</div>{labelName}" +
                                $"</div>" +
                            $"</div>";
            */

            row.Cells.Add(newCell);
            table.Rows.Add(row);
        }

        private void addToCommentsTable(TableRow row, string value, Table table)
        {
            table.CssClass = "comment-tbl";
            row.CssClass = "comment-container";

            TableCell newCell = new TableCell();

            newCell.CssClass = "col-md-2 col-sm-2 col-xs-12 noData";
            newCell.Text = value;

            row.Cells.Add(newCell);

            table.Rows.Add(row);
        }

        private void addToTableInSameCell(TableRow row, string value, string labelName, string type, int width, Table table)
        {
            row.CssClass = "ugit-tdetaillabel whiteborder";
            row.BorderStyle = BorderStyle.Dotted;
            TableCell newCell = new TableCell();
            TableCell newCellType = new TableCell();

            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            newCellType.BorderStyle = BorderStyle.None;
            newCellType.BorderWidth = 0;

            newCellType.Text = labelName;
            newCellType.CssClass += " tableCell ugit-tdetaillabelval";
            newCell.Text = value;
            newCell.Height = 30;
            if (type == string.Empty)
            {
                newCell.ForeColor = System.Drawing.Color.Black;
                newCell.ColumnSpan = (width * 2) - 1;
                newCell.CssClass += " tableCell ugit-tdetaillabelval";
            }

            row.Cells.Add(newCellType);
            row.Cells.Add(newCell);

            table.Rows.Add(row);

        }

        private void addToTableInSameCell(TableRow row, string text, string type, double width, Table table)
        {
            TableCell newCell = new TableCell();
            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            newCell.Height = 30;
            newCell.Text = text;
            newCell.ColumnSpan = int.Parse((width * 2).ToString());
            if (type == string.Empty)
            {
                newCell.ForeColor = System.Drawing.Color.Black;
            }

            row.Cells.Add(newCell);
            table.Rows.Add(row);
            table.Attributes.Add("Width", "100%");
        }

        private void addToTableInSameCell(TableRow row, Control ctrl, string labelName, string type, double width, Table table)
        {
            TableCell newCell = new TableCell();

            newCell.BorderStyle = BorderStyle.None;
            newCell.BorderWidth = 0;
            newCell.Controls.Add(ctrl);
            newCell.Height = 30;
            newCell.ColumnSpan = int.Parse((width * 2).ToString());
            if (type == string.Empty)
            {
                newCell.ForeColor = Color.Black;
            }
            row.Cells.Add(newCell);

            table.Rows.Add(row);

        }

        private List<UserProfile> GetAllUsersFromGroupsAndUsers()  //  currenty not in use
        {

            List<UserProfile> users = new List<UserProfile>();
            //foreach (SPFieldUserValue userOrGroup in usersAndGroups)
            //{
            //    if (userOrGroup.User == null)
            //    {
            //        SPGroup group = SPContext.Current.Site.RootWeb.Groups.GetByID(userOrGroup.LookupId);
            //        foreach (SPUser user in group.Users)
            //        {
            //            UserProfile profile = UserProfile.LoadById(user.ID);
            //            if (profile != null)
            //                users.Add(profile);
            //        }
            //    }
            //    else
            //    {
            //        UserProfile profile = UserProfile.LoadById(userOrGroup.User.ID);
            //        if (profile != null)
            //            users.Add(profile);
            //    }
            //}
            return users;
        }

        public static string GetVersionedMultiLineTextAsPlainText(string key)
        {
            StringBuilder sb = new StringBuilder();
            //foreach (SPListItemVersion version in item.Web.Lists[item.ParentList.ID].Items[item.UniqueId].Versions)
            //{
            //    SPFieldMultiLineText field = version.Fields[key] as SPFieldMultiLineText;
            //    if (field != null)
            //    {
            //        string comment = field.GetFieldValueAsText(version[key]);
            //        if (comment != null && comment.Trim() != string.Empty)
            //        {
            //            sb.Append("\n\r");
            //            sb.Append(version.CreatedByUser.LookupValue).Append(" (");
            //            sb.Append(version.Created.ToString("MMM-d-yyyy hh:mm tt"));
            //            sb.Append(")");
            //            sb.Append(comment);
            //        }
            //    }
            //}
            return sb.ToString();
        }
        /// <summary>
        /// This function checks the user type field in ModuleUserType table to select the manager or IT person.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        //private Boolean CheckUserType(SPField f, BaseFieldControl control)
        //{
        //    //Boolean valid = true;
        //    //if (control.Value != null && Convert.ToString(control.Value) != string.Empty && control.Value.ToString() != "")
        //    //{
        //    //    DataTable ModulUserTypesTable = uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleUserTypes);

        //    //    string query = string.Format("{0}='{1}' And ( {2}= '{3}' OR {2} = '{4}' )", DatabaseObjects.Columns.ModuleNameLookup, currentModuleName, DatabaseObjects.Columns.ColumnName, f.Title, f.InternalName);
        //    //    DataRow[] moduleUserTypes = ModulUserTypesTable.Select(query);

        //    //    SPUser spUser = null;
        //    //    SPFieldUserValueCollection uCollection = new SPFieldUserValueCollection(control.Web, control.Value.ToString());
        //    //    if (uCollection.Count > 0)
        //    //    {
        //    //        int uID = uCollection[0].LookupId;
        //    //        spUser = uCollection[0].User;
        //    //        if (uID == 0 || uID == -1)
        //    //        {
        //    //            spUser = control.Web.EnsureUser(uCollection[0].LookupValue);
        //    //        }

        //    //    }

        //    //    if (spUser != null && spUser.ID > 0)
        //    //    {
        //    //        UserProfile user = UserProfile.LoadById(spUser.ID);
        //    //        if (moduleUserTypes.Length > 0)
        //    //        {
        //    //            if (moduleUserTypes[0][DatabaseObjects.Columns.Groups] != null
        //    //                && Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.Groups]) != string.Empty)
        //    //            {
        //    //                string[] groups = Convert.ToString(moduleUserTypes[0][DatabaseObjects.Columns.Groups]).Split(';');

        //    //                if (groups.Length > 0 && user != null && UserProfile.CheckUserIsInGroup(groups, user.ID))
        //    //                {
        //    //                    valid = UserProfile.CheckUserISManager(moduleUserTypes[0], user);
        //    //                }
        //    //                else
        //    //                {
        //    //                    valid = false;
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                valid = UserProfile.CheckUserISManager(moduleUserTypes[0], user);
        //    //            }
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        return false;
        //    //    }
        //    //}

        //    //return valid;
        //}

        private bool CheckUserISManager(DataRow moduleUserTypeItem, UserProfile user)
        {
            bool valid = true;

            if (Convert.ToString(moduleUserTypeItem[DatabaseObjects.Columns.ManagerOnly]) == "1")
            {
                if (user.IsManager != true)
                {
                    valid = false;
                }
            }
            if (Convert.ToString(moduleUserTypeItem[DatabaseObjects.Columns.ITOnly]) == "1")
            {
                if (user.IsIT != true)
                {
                    valid = false;
                }
            }
            return valid;
        }

        /// <summary>
        /// Get Module Configurations and set into specified config tables
        /// </summary>
        private void SetConfigurationTables()
        {
            if (string.IsNullOrEmpty(currentModuleName))
                return;
            TicketRequest = new Ticket(context, currentModuleName, user);

            //Get the PMM Budget report url
            pmmBudgetReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=projectbudgetreport");
            pmmActualsReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=projectactualsreport");

            //Get the PMM Budget report url
            itgBudgetReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=itgbudgetreport");
            itgActualsReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=itgactualsreport");

            currentModuleName = TicketRequest.Module.ModuleName;
            currentModulePagePath = UGITUtility.GetAbsoluteURL(TicketRequest.Module.ModuleRelativePagePath);  //TicketRequest.Module.DetailPageUrl
            currentModuleListPagePath = UGITUtility.GetAbsoluteURL(TicketRequest.Module.StaticModulePagePath);         //TicketRequest.Module.ListPageUrl

            //Get the INC module page url.
            incidentTicketModulePagePath = UGITUtility.GetAbsoluteUrltForModule("INC");

            // authorizedToViewModuleUsers = TicketRequest.Module.AuthorizedToView;

            //Get user location
            if (user != null)
                userLocation = Convert.ToString(user.LocationId);

            if (TicketRequest.Module.List_Priorities.Count > 0)
            {
                if (TicketRequest.Module.List_Priorities.Exists(x => x.IsVIP))
                    elevatedPrioirty = TicketRequest.Module.List_Priorities.FirstOrDefault(x => x.IsVIP).Title;
                else
                    elevatedPrioirty = TicketRequest.Module.List_Priorities.OrderBy(x => x.ItemOrder).Last().Title;
            }

            preloadAllModuleTabs = TicketRequest.Module.PreloadAllModuleTabs;
        }

        protected string GenericEmailContent()
        {
            return string.Empty;
            //if (currentTicket == null)
            //    return string.Empty;

            //SPListItem item = currentTicket;
            //string ticketType = uHelper.moduleTypeName(moduleId);
            //string subject = string.Format("{0} {1}: {2}", ticketType, item[DatabaseObjects.Columns.TicketId], item[DatabaseObjects.Columns.Title]);
            //StringBuilder href = new StringBuilder();
            //href.AppendFormat("mailto:inbound@kelltontech.com?subject={0}&body=", Uri.EscapeDataString("Supper Mobility Week - Give me Free Pass."));
            //StringBuilder emailBody = new StringBuilder();
            //emailBody.AppendFormat("Hi,\r\n\r\n");
            //emailBody.AppendFormat("Please send me the code for my free pass to Super Mobility Week.\r\n\r\n",
            //                       ticketType.ToLower(), item[DatabaseObjects.Columns.TicketId], item[DatabaseObjects.Columns.Title]);
            //emailBody.AppendFormat("Name:\r\n\r\nEmail Address:\r\nTelephone Number:\r\nDesignation & Organization:",
            //                       SPContext.Current.Web.CurrentUser.Name, uHelper.GetTicketDetailsForEmailFooter(item, uHelper.getModuleNameByTicketId(Convert.ToString(item[DatabaseObjects.Columns.TicketId])), false));
            //href.AppendFormat(Uri.EscapeDataString(emailBody.ToString()));
            //return uHelper.TruncateWithEllipsis(href.ToString(), 2000); // IE chokes on links longer than 2000 characters!
        }

        public List<TicketColumnValue> GetValuesFromControls(bool newTicket, bool batchEditing)
        {
            List<TicketColumnValue> ctrValues = new List<TicketColumnValue>();
            if (newTicket)
            {
                GetTabValues(panelNewTicket, 0, ctrValues, batchEditing);
            }
            else
            {
                foreach (Control rCtrl in tabRepeater.Controls)
                {
                    if (null != rCtrl.FindControl("tabIdHidden"))
                    {
                        int tabId = 0;
                        if (int.TryParse(((HiddenField)rCtrl.FindControl("tabIdHidden")).Value.Trim(), out tabId))
                            GetTabValues(rCtrl, tabId, ctrValues, batchEditing);
                    }
                }
            }

            //Save value from Url.
            foreach (string key in fieldValuesFromUrl.Keys)
            {
                TicketColumnValue columnV = ctrValues.FirstOrDefault(x => x.InternalFieldName == key);
                if (columnV == null)
                    columnV = new TicketColumnValue();

                columnV.InternalFieldName = key;
                columnV.Value = fieldValuesFromUrl[key];
                columnV.TabNumber = 0;
                ctrValues.Add(columnV);
            }


            //Remove department and GLCode if request type is not of Requisition type
            TicketColumnValue requestTypeColumn = ctrValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup);
            if (requestTypeColumn != null)
            {
                string rLookup = Convert.ToString(requestTypeColumn.Value);
                if (rLookup != null && string.IsNullOrEmpty(rLookup))
                {
                    ModuleRequestType rType = TicketRequest.Module.List_RequestTypes.FirstOrDefault()/*(x => x.ID == rLookup.LookupId)*/;
                    if (rType != null && rType.WorkflowType != "Requisition")
                    {
                        /*
                        TicketColumnValue departmentClm = ctrValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.DepartmentLookup);
                        if (departmentClm != null)
                            ctrValues.Remove(departmentClm);

                        TicketColumnValue glcodeClm = ctrValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketGLCode);
                        if (glcodeClm != null)
                            ctrValues.Remove(glcodeClm);
                        */
                    }
                }
            }

            //add resolution description if confirm close popup is prompted.
            if (confirmChildTicketsClose)
            {
                TicketColumnValue resolutionComment = ctrValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketResolutionComments);
                if (resolutionComment == null)
                {
                    resolutionComment = new TicketColumnValue();
                    resolutionComment.InternalFieldName = DatabaseObjects.Columns.TicketResolutionComments;
                    resolutionComment.DisplayName = "Resolution";
                    resolutionComment.Value = txtConfrmToCloseComment.Text.Trim();
                    ctrValues.Add(resolutionComment);
                }
                else if (string.IsNullOrWhiteSpace(Convert.ToString(resolutionComment.Value)))
                {
                    resolutionComment.Value = txtConfrmToCloseComment.Text.Trim();
                }
            }

            if (uPRPorORPList != null && !string.IsNullOrEmpty(txtPRPorORPChangeComment.Text.Trim()))
            {
                TicketColumnValue comment = ctrValues.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketComment);
                if (comment == null)
                {
                    comment = new TicketColumnValue();
                    comment.InternalFieldName = DatabaseObjects.Columns.TicketComment;
                    comment.DisplayName = "Comments";
                    ctrValues.Add(comment);
                }
                comment.Value = txtPRPorORPChangeComment.Text.Trim();
            }

            return ctrValues;
        }

        //public void GetTabValues(Control ctrl, int tabID, List<TicketColumnValue> ctrValues, bool batchEditing) { }

        public void GetTabValues(Control ctrl, int tabID, List<TicketColumnValue> ctrValues, bool batchEditing)
        {
            List<ModuleFormLayout> layouts = TicketRequest.Module.List_FormLayout.Where(x => x.ModuleNameLookup == currentModuleName && x.TabId == tabID).ToList();
            Control baseFieldCtr;
            string groupDisplayName = string.Empty;

            ModuleRoleWriteAccess roleAccessField;
            foreach (ModuleFormLayout layout in layouts)
            {
                bool foundFieldValue = false;
                TicketColumnValue cValue = new TicketColumnValue();

                if (layout.FieldName.ToLower() == "#groupstart#")
                    groupDisplayName = layout.FieldDisplayName;

                if (layout.FieldName.StartsWith("#") && layout.FieldName.ToLower() != "#control#")
                    continue;

                //ModuleRoleWriteAccess roleAccessField;
                if (layout.FieldName.ToLower() == "#control#")
                    roleAccessField = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStage.StageStep || x.StageStep == 0) && x.FieldName == layout.FieldDisplayName);
                else
                    roleAccessField = TicketRequest.Module.List_RoleWriteAccess.FirstOrDefault(x => (x.StageStep == currentStage.StageStep || x.StageStep == 0) && x.FieldName == layout.FieldName);

                if (roleAccessField == null && !adminOverride)
                    continue;


                if (layout.FieldName.ToLower() == "#control#")
                {
                    // condition for batch editing....
                    string controlId = string.Format("{0}_{1}_{2}", tabID, layout.FieldDisplayName, "mandatory");

                    Control tControl = (ctrl).FindControl(controlId);
                    if (tControl != null && tControl is HiddenField)
                    {
                        foundFieldValue = true;
                        HiddenField hdn = tControl as HiddenField;
                        cValue.DisplayName = layout.FieldDisplayName;
                        cValue.InternalFieldName = layout.FieldName;
                        cValue.TabNumber = tabID;
                        cValue.Value = hdn.Value;
                        cValue.ErrorDisplayName = groupDisplayName;
                    }

                    if (foundFieldValue)
                        ctrValues.Add(cValue);
                }
                else
                {
                    string controlId = string.Empty;
                    controlId = layout.FieldName + '_' + tabID;
                    Control tControl = null;
                    Control hiddenCtrl = null;
                    //condition for batch editing
                    //Check hidden field value and go for next step if value == False 
                    if (batchEditing)
                    {
                        hiddenCtrl = (ctrl).FindControl(controlId + "_batchEditHidden");
                        if (hiddenCtrl == null)
                            hiddenCtrl = (ctrl).FindControlRecursive(controlId + "_batchEditHidden");
                        if (hiddenCtrl != null)
                        {
                            HiddenField hiddenEdit = hiddenCtrl as HiddenField;
                            if (hiddenEdit.Value == "True")
                            {
                                continue;
                            }
                        }
                    }
                    if (tabID == 0)
                    {
                        tControl = (ctrl).FindControlRecursive(controlId);
                        if (tControl == null)
                            tControl = (ctrl).FindControlRecursive(controlId);
                        if (layout.FieldName == DatabaseObjects.Columns.TicketDesiredCompletionDate)
                        {
                            tControl.Controls[0].Visible = false;
                        }
                    }
                    else
                    {
                        tControl = (ctrl).FindControlRecursive(controlId);
                        if (tControl == null)
                            tControl = (ctrl).FindControlRecursive(controlId);
                    }
                    //TicketColumnValue cValue = new TicketColumnValue();
                    cValue.DisplayName = layout.FieldDisplayName;
                    cValue.InternalFieldName = layout.FieldName;// f.InternalName;
                    cValue.TabNumber = tabID;

                    //bool foundFieldValue = false;
                    //new line of code for batch editing...                    
                    //If ticket having VIP priority then only
                    if (layout.FieldName == DatabaseObjects.Columns.TicketPriorityLookup)
                    {
                        CheckBox vipCheck = (CheckBox)ctrl.FindControl(controlId + "_elevatecheck");
                        if (vipCheck != null)
                        {
                            TicketColumnValue cElevatedPriority = new TicketColumnValue();
                            cElevatedPriority.DisplayName = DatabaseObjects.Columns.ElevatedPriority;
                            cElevatedPriority.InternalFieldName = DatabaseObjects.Columns.ElevatedPriority;
                            cElevatedPriority.TabNumber = tabID;
                            cElevatedPriority.Value = vipCheck.Checked;
                            ctrValues.Add(cElevatedPriority);
                        }
                    }
                    //    else if (tControl is ASPxDropDownEdit && currentModuleName == "VCC")
                    //    {
                    //        //For VCC either one (MSA/SOW) is selected.
                    //        foundFieldValue = true;
                    //        ASPxDropDownEdit aspxdropdownedit = (ASPxDropDownEdit)tControl;
                    //        if (Convert.ToString(aspxdropdownedit.KeyValue).StartsWith("pmsaid"))
                    //        {
                    //            TicketColumnValue cMSAValue = new TicketColumnValue();
                    //            cMSAValue.DisplayName = cValue.DisplayName;
                    //            cMSAValue.InternalFieldName = DatabaseObjects.Columns.VendorMSALookup;
                    //            cMSAValue.TabNumber = tabID;
                    //            int msaID = 0;
                    //            int.TryParse(Convert.ToString(aspxdropdownedit.KeyValue).Replace("pmsaid", "").Trim(), out msaID);
                    //            if (msaID > 0)
                    //                cMSAValue.Value = new SPFieldLookupValue(msaID.ToString());

                    //            ctrValues.Add(cMSAValue);
                    //        }
                    //        else
                    //        {
                    //            int sowID = 0;
                    //            int.TryParse(Convert.ToString(aspxdropdownedit.KeyValue), out sowID);
                    //            if (sowID > 0)
                    //                cValue.Value = new SPFieldLookupValue(sowID.ToString());
                    //        }
                    //    }
                    //}
                    #endregion
                    if (tControl == null)
                        continue;
                    if (tControl is DropDownList)
                    {
                        foundFieldValue = true;
                        DropDownList ddl = tControl as DropDownList;
                        cValue.Value = ddl.SelectedValue;
                    }
                    else if (tControl is ASPxDateEdit)
                    {
                        foundFieldValue = true;
                        ASPxDateEdit dde = tControl as ASPxDateEdit;
                        cValue.Value = Convert.ToString(dde.Value);

                    }
                    else if (tControl is ASPxComboBox)
                    {
                        foundFieldValue = true;
                        ASPxComboBox ddl = tControl as ASPxComboBox;
                        cValue.Value = ddl.Value;
                    }
                    else if (tControl is ASPxDropDownEdit)
                    {
                        foundFieldValue = true;
                        ASPxDropDownEdit ddl = tControl as ASPxDropDownEdit;
                        cValue.Value = ddl.KeyValue;
                    }
                    else if (tControl is ASPxMemo)
                    {
                        foundFieldValue = true;
                        ASPxMemo richTextControl = (ASPxMemo)tControl;
                        cValue.Value = richTextControl.Text;
                    }
                    else if (tControl is ASPxTextBox)
                    {
                        foundFieldValue = true;
                        ASPxTextBox txt = tControl as ASPxTextBox;
                        cValue.Value = txt.Text;
                    }
                    else if (tControl is TextBox)
                    {
                        foundFieldValue = true;
                        TextBox txtCtr = tControl as TextBox;
                        cValue.Value = txtCtr.Text.Trim();
                    }
                    else if (tControl is UserValueBox)
                    {
                        foundFieldValue = true;
                        UserValueBox gdLookup = tControl as UserValueBox;
                        cValue.Value = gdLookup.GetValues();
                    }
                    else if (tControl is LookUpValueBox)
                    {
                        foundFieldValue = true;
                        LookUpValueBox gdLookup = tControl as LookUpValueBox;

                        cValue.Value = gdLookup.GetValues();
                    }
                    else if (tControl is LookupValueBoxEdit)
                    {
                        foundFieldValue = true;
                        LookupValueBoxEdit gdLookup = tControl as LookupValueBoxEdit;
                        cValue.Value = gdLookup.GetValues();
                    }
                    else if (tControl is Label)
                    {
                        foundFieldValue = true;
                        Label lbl = tControl as Label;
                        cValue.Value = !string.IsNullOrWhiteSpace(lbl.Text) && lbl.Text.Contains('-')
                            && lbl.Text.Trim().Length == 1 ? string.Empty : lbl.Text;

                    }
                    else if (tControl is UGITFileUploadManager)
                    {
                        foundFieldValue = true;
                        UGITFileUploadManager ufile = tControl as UGITFileUploadManager;
                        cValue.Value = ufile.GetImageUrl();
                    }
                    else if (tControl is DepartmentDropdownList)
                    {
                        foundFieldValue = true;
                        DepartmentDropdownList departmentCtr = (DepartmentDropdownList)tControl;
                        cValue.Value = departmentCtr.Value;
                    }
                    else if (tControl is AssetLookupDropDownList)
                    {
                        foundFieldValue = true;
                        AssetLookupDropDownList Ctr = (AssetLookupDropDownList)tControl;
                        cValue.Value = Ctr.GetValues();
                    }
                    else if (tControl is NumberValueBox)
                    {
                        foundFieldValue = true;
                        NumberValueBox customcupeCtr = tControl as NumberValueBox;
                        cValue.Value = customcupeCtr.GetValue();
                    }
                    else if (tControl is FileUploadControl)
                    {
                        foundFieldValue = true;
                        FileUploadControl fileUploadControl = tControl as FileUploadControl;
                        cValue.Value = fileUploadControl.GetValue();
                    }
                    else if (tControl is ASPxCheckBox)
                    {
                        foundFieldValue = true;
                        ASPxCheckBox checkBox = tControl as ASPxCheckBox;
                        cValue.Value = checkBox.Checked;
                    }
                    else if (tControl is CheckBox)
                    {
                        foundFieldValue = true;
                        CheckBox checkBox = tControl as CheckBox;
                        cValue.Value = checkBox.Checked;
                    }

                    else if (tControl is CustomListDropDown)
                    {
                        foundFieldValue = true;
                        CustomListDropDown ContactCtrl = (CustomListDropDown)tControl;

                        if (ContactCtrl.GetValue() == "<Value Varies>")
                        {
                            foundFieldValue = false;
                        }
                        else
                        {
                            CustomListDropDown customListDropDownCrl = (CustomListDropDown)tControl;

                            //if (lookupVals != null && lookupVals.Count > 0)
                            //{
                            //    if (lookupField.AllowMultipleValues)
                            //    {
                            //        cValue.Value = lookupVals;
                            //    }
                            //    else
                            //    {
                            //        cValue.Value = lookupVals[0];
                            //    }
                            //}
                            cValue.Value = customListDropDownCrl.GetValue();
                        }
                    }

                    else if (tControl is Control)
                    {
                        baseFieldCtr = (Control)tControl;
                        //if (baseFieldCtr.ControlMode != TicketControls.ControlMode.Display)
                        //{
                        //    foundFieldValue = true;
                        //    cValue.Value = baseFieldCtr.Value;
                        //}
                    }

                    if (foundFieldValue)
                        ctrValues.Add(cValue);
                }
            }
            var checkTenant = ctrValues.Any(a => a.InternalFieldName == "TenantID");
            if (!checkTenant)
            {
                TicketColumnValue addValue = new TicketColumnValue();
                addValue.DisplayName = "TenantID";
                addValue.InternalFieldName = "TenantID";
                addValue.TabNumber = tabID;
                addValue.Value = TenantID;
                ctrValues.Add(addValue);
            }
            List<TicketColumnValue> ctrValues1 = ctrValues;
        }

        private void ValidateAndSetAttachments(List<TicketColumnError> errors, Control ctrl, DataRow item, bool newTicket)
        {

            //int maxNoOfFiles = Convert.ToInt32(objConfigurationVariableHelper.GetValue("NewTicketMaxFiles"));
            // SPAttachmentCollection attachments = item.Attachments;
            // List<string> attachedFiles = attachments.Cast<string>().ToList();
            // List<string> newFiles = new List<string>();
            //  List<HttpPostedFile> uploadedFiles = new List<HttpPostedFile>();


            //if (copyTicket != null && copyTicket.Attachments != null && copyTicket.Attachments.Count > 0)
            //{
            //    for (int i = 0; i < copyTicket.Attachments.Count; i++)
            //    {
            //        HiddenField itemAttachment = (HiddenField)ctrl.FindControl("hdnCopyAttachment" + i);
            //        string fileName = copyTicket.Attachments[i];
            //        string fileUrl = UGITUtility.GetAbsoluteURL(copyTicket.Attachments.UrlPrefix + fileName);
            //        SPAttachmentCollection attachmentsa = copyTicket.Attachments;
            //        if (itemAttachment != null && itemAttachment.Value == fileUrl)
            //        {
            //            SPFile file = SPContext.Current.Web.GetFile(fileUrl);
            //            if (file != null)
            //            {
            //                newFiles.Add(fileName);
            //                byte[] imageData = file.OpenBinary();
            //                item.Attachments.Add(fileName, imageData);
            //            }
            //        }
            //    }
            //}


            //for (int i = 0; i < maxNoOfFiles; i++)
            //{
            //    FileUpload itemAttachment = (FileUpload)ctrl.FindControl("fileupload" + i);
            //    if (itemAttachment != null && itemAttachment.HasFile && attachedFiles.FirstOrDefault(x => x == itemAttachment.FileName) == null)
            //    {
            //        string fileName = Path.GetFileName(itemAttachment.PostedFile.FileName);
            //        if (fileName != string.Empty && System.Text.RegularExpressions.Regex.Match(fileName, @"[~#%&*{}\<>?/+|""]").Success)
            //        {
            //            errors.Add(TicketColumnError.AddError("Attachment Name connot contain [~#%&*{}\\<>?/+|\"]"));
            //        }
            //        else
            //        {
            //            attachments.Add(fileName, itemAttachment.FileBytes);
            //            newFiles.Add(fileName);
            //            itemAttachment.PostedFile.InputStream.Close();
            //            itemAttachment.PostedFile.InputStream.Dispose();
            //        }
            //    }
            //    else if (itemAttachment != null && !itemAttachment.HasFile && itemAttachment.PostedFile != null)
            //    {
            //        string fileName = Path.GetFileName(itemAttachment.PostedFile.FileName);
            //        if (fileName != string.Empty && System.Text.RegularExpressions.Regex.Match(fileName, @"[~#%&*{}\<>?/+|""]").Success)
            //        {
            //            errors.Add(TicketColumnError.AddError("Attachment Name connot contain [~#%&*{}\\<>?/+|\"]"));
            //        }
            //    }
            //}
            FileUploadControl fileUploadControl = (FileUploadControl)ctrl.FindControlRecursive("Attachments_0"); //ctrl as FileUploadControl;

            if (fileUploadControl == null) //Added condition to skip, if Attachment control is not added on form.
                return;

            DocumentManager manager = new DocumentManager(context);
            string filevalue = fileUploadControl.GetValue();
            List<string> currentvalue = UGITUtility.SplitString(filevalue, ",").ToList();
            List<string> fileName = new List<string>();
            foreach (string id in currentvalue)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    Document document = manager.Get(x => x.FileID == id);
                    if (document != null)
                        fileName.Add(document.Name);
                }
            }
            if (!filevalue.Equals(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.Attachments])))
            {
                string historyMsg = string.Format("Attached file(s): {0}", string.Join(",", fileName));
                uHelper.CreateHistory(context.CurrentUser, historyMsg, item, false, context);

            }

        }

        private bool IsActionUserChanged(DataRow item, List<TicketColumnValue> columnVals)
        {
            bool isChanged = false;
            //newPRPorORPUsers = new SPFieldUserValueCollection();
            List<UserProfile> newPRPorORPUsers = new List<UserProfile>();
            List<UserProfile> pPRPAndORPList = new List<UserProfile>();
            List<UserProfile> oldPRPAndORPList = new List<UserProfile>();
            //SPFieldUserValueCollection pPRPAndORPList = new SPFieldUserValueCollection();
            //SPFieldUserValueCollection oldPRPAndORPList = new SPFieldUserValueCollection();

            string actionUsersType = Convert.ToString(item[DatabaseObjects.Columns.TicketStageActionUserTypes]);
            if (!string.IsNullOrEmpty(actionUsersType) && actionUsersType.IndexOf(Constants.Separator10) != -1)
                actionUsersType = actionUsersType.Replace(Constants.Separator10, Constants.Separator);
            if (!string.IsNullOrEmpty(actionUsersType))
            {
                List<TicketColumnValue> colVals = columnVals.Where(x => actionUsersType.Contains(x.InternalFieldName)).ToList();

                foreach (TicketColumnValue c in colVals)
                {
                    if (c.Value == null || Convert.ToString(c.Value).Contains("<Value Varies>"))
                        continue;


                    //if (c.Value is SPFieldUserValue)
                    //{
                    //    pPRPAndORPList.Add(c.Value as SPFieldUserValue);
                    //}
                    //        else
                    //        {
                    //            pPRPAndORPList.AddRange(c.Value as SPFieldUserValueCollection);
                    //        }
                }

                foreach (TicketColumnValue c in colVals)
                {
                    //        SPFieldUserValueCollection itemVal = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(item[c.InternalFieldName]));
                    //        if (itemVal != null && itemVal.Count > 0)
                    //        {
                    //            oldPRPAndORPList.AddRange(itemVal);
                    //        }
                }

                //    if (oldPRPAndORPList.Count != pPRPAndORPList.Count)
                //    {
                //        isChanged = true;
                //    }

                //    foreach (SPFieldUserValue uVal in pPRPAndORPList)
                //    {
                //        if (!oldPRPAndORPList.Exists(x => x.LookupId == uVal.LookupId))
                //        {
                //            newPRPorORPUsers.Add(uVal);
                //            isChanged = true;
                //        }
                //    }
                if (columnVals.Exists(x => x.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup))
                {
                    string itemVal = Convert.ToString(item[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                    string newVal = Convert.ToString(columnVals.FirstOrDefault(x => x.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup).Value);
                    if (itemVal != newVal)
                    {
                        isChanged = true;
                    }
                }
            }
            return isChanged;
        }

        private Dictionary<string, string> GetFieldValuesFromUrl(HttpRequest request)
        {
            Dictionary<string, string> fieldValues = new Dictionary<string, string>();
            List<string> fieldValUrl = Request.QueryString.AllKeys.Where(x => x != null && x.StartsWith("js_")).ToList();
            foreach (string paramKey in fieldValUrl)
            {
                string field = paramKey.Replace("js_", string.Empty);
                if (fieldValues.ContainsKey(field))
                    fieldValues[field] = Request.QueryString.Get(paramKey);
                else
                    fieldValues.Add(field, Request.QueryString.Get(paramKey));
            }
            return fieldValues;
        }

        protected void btnRequestTypeChangeOk_Click(object sender, EventArgs e)
        {
            currentTicket[DatabaseObjects.Columns.TicketPRP] = "";
            currentTicket[DatabaseObjects.Columns.TicketORP] = "";
            //currentTicket.UpdateOverwriteVersion();
            pcRequestTypeChange.ShowOnPageLoad = false;
            IsRequestTypeChange = true;
            actionEventID.Value = "";
            Button_Click(returnWithCommentsButton, e);
        }

        //private void CopySpItem(SPListItem sourceItem, SPListItem destinationItem)
        //{
        //    foreach (SPField field in sourceItem.Fields)
        //    {
        //        if (!field.ReadOnlyField && !field.Hidden && field.InternalName != "Attachments" && field.InternalName != "History")
        //        {
        //            if (destinationItem.Fields.ContainsField(field.InternalName))
        //            {
        //                destinationItem[field.InternalName] = sourceItem[field.InternalName];
        //            }
        //        }
        //    }
        //    destinationItem.Update();
        //}

        protected void LoadTicketsCollection()
        {
            string ticketIDs = "";
            string tableName = ModuleViewManager.GetModuleTableName(currentModuleName);
            foreach (string s in TicketIdList)
            {
                ticketIDs += "'" + s + "',";
            }
            ticketIDs = ticketIDs.Length > 0 ? ticketIDs.Substring(0, ticketIDs.Length - 1) : ticketIDs;
            //spListitemCollection = GetTableDataManager.GetTableData(tableName, DatabaseObjects.Columns.TicketId + " in  (" + ticketIDs + ") and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select();
            spListitemCollection = GetTableDataManager.GetTableData(tableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.TicketId} in (" + ticketIDs + ")").Select();
            //if (spListitemCollection == null && TicketIdList.Count > 0)
            //{
            //    SPQuery spQuery = new SPQuery();
            //    string qry = "<Where><In><FieldRef Name=\"TicketId\" /><Values>";

            //    foreach (string s in TicketIdList)
            //    {
            //        qry += string.Format("<Value Type=\"Text\">{0}</Value>", s);
            //    }
            //    qry += "</Values></In></Where>";

            //    spQuery.Query = qry;

            //    spListitemCollection = thisList.GetItems(spQuery);
            //}
        }

        private DataTable LoadMacroTicketData()
        {
            DataTable dtMacroTicket = new DataTable();
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' /><Value Type='Text'>{3}</Value></Eq></And></Where><OrderBy><FieldRef Name='{4}' Ascending='True' /></OrderBy>", DatabaseObjects.Columns.ModuleNameLookup, ModuleId, DatabaseObjects.Columns.TemplateType, "Macro", DatabaseObjects.Columns.Title);
            string query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleId, DatabaseObjects.Columns.TemplateType, "Macro", DatabaseObjects.Columns.Title);
            DataRow[] rowCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketTemplates, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(query, DatabaseObjects.Columns.Title + "  Asc");
            if (rowCollection != null && rowCollection.Count() > 0)
            {
                dtMacroTicket = rowCollection.CopyToDataTable();
                return dtMacroTicket;
            }
            else
            {
                return null;
            }
        }

        protected void MacroTicket(Control bfc)
        {
            //SPListItem spListItem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.TicketTemplates, Convert.ToInt32(Request[hdnMacroTicketTemplate.UniqueID]));
            //string fieldvalues = Convert.ToString(spListItem[DatabaseObjects.Columns.FieldValues]);
            //string[] values = fieldvalues.Split(new string[] { Constants.Separator3 }, StringSplitOptions.RemoveEmptyEntries);

            //string fieldValue = string.Empty;
            #region CreateFromTemplate
            //if (!string.IsNullOrEmpty(f.InternalName))
            //{
            //    //SPField f = thisList.Fields.GetFieldByInternalName(fieldInternalName);
            //    string value = values.FirstOrDefault(x => x.StartsWith(f.InternalName));
            //    string type = string.Empty;
            //    if (!string.IsNullOrEmpty(value))
            //    {
            //        string[] attributes = value.Split(new string[] { Constants.Separator4 }, StringSplitOptions.RemoveEmptyEntries);
            //        if (attributes.Length == 3)
            //        {
            //            type = attributes[1];
            //            fieldValue = attributes[2];
            //        }
            //        // }


            //        switch ((SPFieldType)Enum.Parse(typeof(SPFieldType), type))
            //        {
            //            case SPFieldType.Text:
            //                {
            //                    TextField tt = (TextField)(bfc);
            //                    tt.Text = fieldValue;
            //                }
            //                break;
            //            case SPFieldType.Number:
            //                {
            //                    double dblValue = 0;
            //                    if (double.TryParse(fieldValue, out dblValue))
            //                    {
            //                        NumberField nn = (NumberField)bfc;
            //                        nn.Value = fieldValue;
            //                    }
            //                }
            //                break;
            //            case SPFieldType.Note:
            //                {
            //                    if (bfc is TextBox)
            //                    {
            //                        TextBox nt = (TextBox)(bfc);
            //                        nt.Text = fieldValue;

            //                    }
            //                    else
            //                    {
            //                        Microsoft.SharePoint.WebControls.NoteField nt = (Microsoft.SharePoint.WebControls.NoteField)(bfc);
            //                        nt.Text = fieldValue;
            //                    }
            //                }
            //                break;
            //            case SPFieldType.Choice:
            //                {
            //                    if (bfc.GetType() == typeof(UGITDropDown))
            //                    {
            //                        ((UGITDropDown)bfc).DropDown.Value = fieldValue;
            //                    }
            //                    else if (bfc.GetType() == typeof(DropDownChoiceField))
            //                    {
            //                        ((DropDownChoiceField)bfc).Value = fieldValue;
            //                    }
            //                }
            //                break;
            //            case SPFieldType.DateTime:
            //                {

            //                    DateTime dtValue;
            //                    if (DateTime.TryParse(fieldValue, out dtValue))
            //                    {
            //                        Microsoft.SharePoint.WebControls.DateTimeField dtc = (Microsoft.SharePoint.WebControls.DateTimeField)bfc;
            //                        ((DateTimeControl)(dtc.Controls[0].Controls[1])).SelectedDate = dtValue;
            //                    }

            //                }
            //                break;
            //            case SPFieldType.User:
            //                if (bfc.GetType() == typeof(Microsoft.SharePoint.WebControls.UserField))
            //                {
            //                    Microsoft.SharePoint.WebControls.UserField usr = (Microsoft.SharePoint.WebControls.UserField)(bfc);
            //                    usr.Value = fieldValue;
            //                }
            //                break;
            //            case SPFieldType.Lookup:
            //                string lookupID = uHelper.SplitString(fieldValue, Constants.Separator, 0);
            //                if (bfc.GetType() == typeof(DropDownList))
            //                {
            //                    ((DropDownList)(bfc)).SelectedValue = lookupID;
            //                }
            //                else if (f.InternalName == DatabaseObjects.Columns.TicketRequestTypeLookup)
            //                {
            //                    if (uGITCache.ModuleConfigCache.GetConfigVariableValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
            //                    {
            //                        if (((Panel)bfc.Controls[0].Controls[0]).Controls[1].Controls[0].Controls[0].Controls[0].Controls[0] is DropDownList)
            //                        {
            //                            ((DropDownList)((Panel)bfc.Controls[0].Controls[0]).Controls[1].Controls[0].Controls[0].Controls[0].Controls[0]).SelectedValue = lookupID;
            //                        }
            //                    }
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //}
            #endregion
        }

        protected void ASPxPopupActionMenu_Load(object sender, EventArgs e)
        {
            LoadActionPopupMenu(sender as ASPxPopupMenu);
        }

        private void LoadActionPopupMenu(ASPxPopupMenu menu)
        {
            ASPxPopupActionMenu.Items.Clear();
            menu.ItemImage.Width = 16;
            if (Request["BatchEditing"] != "true")
            {
                if (TicketRequest.Module.ShowComment)
                    menu.Items.Add("Comment", "Comment", "/Content/ButtonImages/comments.png");

                menu.Items.Add("Copy Link to Clipboard", "CopyLinktoClipboard", "/Content/images/duplicate.png");
                //menu.Items.Add("Compact View", "CompactView", "/Content/images/Menu/SubMenu/SVC_32x32.svg"); //commented because we are showing it top left side of screen

                //macro items..
                if (isActionUser || adminOverride)
                {
                    DataTable dtMacroTicketData = LoadMacroTicketData();
                    if (dtMacroTicketData != null && dtMacroTicketData.Rows.Count > 0)
                    {
                        DevExpress.Web.MenuItem submenu = new DevExpress.Web.MenuItem("Macro", "Macro", "/Content/ButtonImages/execute.png");
                        foreach (DataRow rowItem in dtMacroTicketData.Rows)
                        {
                            submenu.Items.Add(new DevExpress.Web.MenuItem(Convert.ToString(rowItem[DatabaseObjects.Columns.Title]), Convert.ToString(rowItem[DatabaseObjects.Columns.Id]), "/Content/images/ButtonImages/execute.png"));
                        }
                        menu.Items.Add(submenu);
                    }
                }

                if (isActionUser || adminOverride || IsAdmin)
                {
                    DevExpress.Web.MenuItem agentItem = uHelper.AddAgentItem(HttpContext.Current.GetManagerContext(), currentModuleName, currentStage.Name);
                    if (agentItem != null)
                        menu.Items.Add(agentItem);
                }

                if (isActionUser && currentModuleName.ToLower() == "inc")
                    menu.Items.Add("Notify", "Notify", "/Content/ButtonImages/notify16X16.png");

                if (currentModuleName.ToLower() != "itg")
                    menu.Items.Add("Print", "Print", "/Content/images/printer.png");

                // Issue with these button so not adding hold/unhold button on request action menu. 
                if (IsAdmin && ticketOnHold != 1 && showHoldUnholdButton)
                {
                    DevExpress.Web.MenuItem putonholditem = new DevExpress.Web.MenuItem("Put on Hold", "Putonhold", "/Content/images/lock16X16.png");
                    //putonholditem.ItemStyle.BackgroundImage.ImageUrl = "/Content/images/firstnavbgRed1X28.png";
                    putonholditem.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                    putonholditem.ItemStyle.ForeColor = Color.White;
                    menu.Items.Add(putonholditem);
                }

                if (currentModuleName == ModuleNames.CPR || currentModuleName == ModuleNames.OPM)
                {
                    DevExpress.Web.MenuItem putonholditem = new DevExpress.Web.MenuItem("Contract Summary", "ContractSummary", "/Content/images/Reports.png");
                    putonholditem.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                    putonholditem.ItemStyle.ForeColor = Color.White;
                    menu.Items.Add(putonholditem);
                }

                if (IsAdmin && ticketOnHold == 1 && showHoldUnholdButton)
                {
                    DevExpress.Web.MenuItem putonunholditem = new DevExpress.Web.MenuItem("Remove Hold", "PutonUnhold", "/Content/images/unlock16X16.png");
                    //putonunholditem.ItemStyle.BackgroundImage.ImageUrl = "/Content/images/firstnavbgGreen1X28.png";
                    putonunholditem.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                    putonunholditem.ItemStyle.ForeColor = Color.White;
                    menu.Items.Add(putonunholditem);

                    DevExpress.Web.MenuItem editonholditem = new DevExpress.Web.MenuItem("Edit Hold", "Putonhold", "/Content/images/lock16X16.png");
                    editonholditem.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                    editonholditem.ItemStyle.ForeColor = Color.White;
                    menu.Items.Add(editonholditem);
                }
                if (ModuleViewManager.GetByName(currentModuleName).EnableQuick)
                    menu.Items.Add("Save as Template", "SaveasTemplate", "/Content/Images/saveAsTemplate-blue.png");




                if ((isActionUser || adminOverride || IsAdmin) && showBaselineButtons)
                {
                    if (!string.IsNullOrEmpty(currentModuleName) && Request["showBaseline"] == null && Request["baselineNum"] == null)
                    {
                        menu.Items.Add("New Baseline", "NewBaseline", "/Content/images/newBaseline.png");
                        DevExpress.Web.MenuItem closeproject = new DevExpress.Web.MenuItem(uGovernIT.Utility.Constants.CloseButtonText, "CloseProject", "/Content/images/closeProject.png");
                        //closeproject.ItemStyle.BackgroundImage.ImageUrl = "/Content/images/firstnavbgRed1X28.png";
                        closeproject.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                        menu.Items.Add(closeproject);
                        menu.Items.Add("Show Baseline", "ShowBaseline", "/Content/images/showBaseline.png");
                    }

                }

                /*
                if ((currentModuleName == "CPR" || currentModuleName == "CNS") && (isActionUser || (UserManager.IsSuperAdmin(context.CurrentUser))))  //
                {
                    //menu.Items.Add("Send To Procore", "SendToProcore", "/_layouts/15/images/uGovernIT/baseline-restore16X16.png");

                    menu.Items.Add("Move Project to Procore", "AgentProcore", "/Content/Images/baseline-restore16X16.png");

                    //menu.Items.Add("New Permit", "NewPermit", "/_layouts/15/images/uGovernIT/add_icon.png");
                }
                */

                if (currentModuleName == "CPR" && (isActionUser || (UserManager.IsUGITSuperAdmin(context.CurrentUser))))
                {
                    menu.Items.Add("New Permit", "NewPermit", "/Content/Images/new_task.png");
                }

                if (currentModuleName == "LEM" && (isActionUser || (UserManager.IsUGITSuperAdmin(context.CurrentUser))))
                {
                    menu.Items.Add("Create Opportunity From Lead", "NewOpportunity", "/Content/Images/new_task.png");
                }

                if ((currentModuleName == "LEM" || currentModuleName == "OPM" || currentModuleName == "CNS" || currentModuleName == "CPR") && (isActionUser || (UserManager.IsUGITSuperAdmin(context.CurrentUser))))
                {
                    menu.Items.Add("Create New Company", "NewCompany", "/Content/Images/new_task.png");
                    menu.Items.Add("Create New Contact", "NewContact", "/Content/Images/new_task.png");
                }

                // Disabled action menu items for BCCI
                //if (currentModuleName == "OPM" && (isActionUser || (UserProfile.IsSuperAdmin(SPContext.Current.Web.CurrentUser))))
                //{
                //    menu.Items.Add("Assign to PreCon", "MoveToPrecon", "/_layouts/15/images/uGovernIT/add_icon.png");
                //}

                //if ((currentModuleName == "OPM" || currentModuleName == "CPR" || currentModuleName == "CNS") && (isActionUser || (UserProfile.IsSuperAdmin(SPContext.Current.Web.CurrentUser))))
                //{
                //    menu.Items.Add("Assign Internal Project Team", "AddAllocation", "/_layouts/15/images/uGovernIT/add_icon.png");
                //}

                if ((currentModuleName == "CPR" || currentModuleName == "CNS" || currentModuleName == "OPM") && (isActionUser || (UserManager.IsUGITSuperAdmin(context.CurrentUser))))
                {
                    menu.Items.Add("Assign External Project Team", "ProjectExternal", "/Content/Images/new_task.png");
                }
                if (currentModuleName == ModuleNames.CPR && (isActionUser || (UserManager.IsUGITSuperAdmin(context.CurrentUser))))
                {
                    menu.Items.Add("Update Project Statistics", "UpdateProjectStatistics", "/Content/Images/statusBlue.png");
                }

                if ((currentModuleName == "OPM") && (isActionUser || UserManager.IsUGITSuperAdmin(context.CurrentUser)))
                    menu.Items.Add("Initial Email", "EscalationEmail", "/Content/Images/mail-send.png");

                //new condition for show/hide the button for batch Escalation.
                if ((currentModuleName == "OPM" || currentModuleName == "CPR" || currentModuleName == "CNS" || currentModuleName == "LEM" || currentModuleName == "CPP") && TicketRequest.Module.AllowEscalationFromList && (isActionUser || (UserManager.IsUGITSuperAdmin(context.CurrentUser))))
                    menu.Items.Add("Email", "Escalation", "/Content/Images/mail-send.png");


                if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableProjectMetricSync))
                {
                    if ((currentModuleName == "CPR") && (isActionUser || (UserManager.IsUGITSuperAdmin(context.CurrentUser))))
                        menu.Items.Add("Metric", "Metric", "/_layouts/15/images/uGovernIT/metrics16X16.png");
                }

                if (menu.Items.Count > 0)
                    lnkbtnActionMenu.ClientVisible = true;
            }
        }

        protected void tempbutton_Click(object sender, EventArgs e)
        {
            BindTabs();
        }

        public Control findControl(Control ctrl, string CtrlId)
        {
            Control returnValue = null;
            foreach (Control c in ctrl.Controls)
            {
                string controlid = c.ID;
                if (controlid == CtrlId)
                {
                    returnValue = c;
                    return returnValue;
                }
                if (c.HasControls())
                {
                    return findControl(c, CtrlId);
                }
            }
            return returnValue;
        }

        private void BindModuleFormTab()
        {
            ddlSelectTab.Items.Clear();
            List<ModuleFormTab> LstFormTab = new List<ModuleFormTab>();
            LstFormTab = ModuleFormTabManager.Load($"ModuleNameLookup='{currentModuleName}'").OrderBy(y => y.TabSequence).ToList();
            LstFormTab.Sort((x, y) => x.TabSequence.CompareTo(y.TabSequence));
            ddlSelectTab.DataTextField = "TabName";
            ddlSelectTab.DataValueField = "TabSequence";

            //ddlSelectTab.DataSource = LstFormTab;

            if (HttpContext.Current.Request.Browser.IsMobileDevice == true)
                ddlSelectTab.DataSource = LstFormTab.Where(x => x.ShowInMobile == true);
            else
                ddlSelectTab.DataSource = LstFormTab;

            ddlSelectTab.DataBind();
            ddlSelectTab.Items.Insert(0, new ListItem("Default", "0"));
        }

        protected void RelatedTitle_Click(object sender, EventArgs e)
        {
            var title = string.Empty;
            var ticketId = string.Empty;
            var ticketRequestTypeLookup = "";
            string listpickerUrl = string.Empty;
            string url = string.Empty;
            var formValues = GetValuesFromControls(true, false);
            UGITModule moduleObj = ObjModuleViewManager.GetByName(currentModuleName);
            if (formValues != null && formValues.Count > 0)
            {
                foreach (var item in formValues)
                {
                    if (item.InternalFieldName == DatabaseObjects.Columns.Title)
                        title = Convert.ToString(item.Value);
                    if (item.InternalFieldName == DatabaseObjects.Columns.TicketRequestTypeLookup)
                        ticketRequestTypeLookup = Convert.ToString(item.Value);
                }
            }
            if (!string.IsNullOrEmpty(title))
            {
                if (moduleObj.EnableLinkSimilarTickets.HasValue && moduleObj.EnableLinkSimilarTickets == true)
                {
                    var dataExist = GetTableDataManager.IsLookaheadTicketExists(moduleObj.ModuleTable, TenantID, title, ticketRequestTypeLookup);
                    if (dataExist)
                    {

                        listpickerUrl = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&Title={3} &Type=TicketRelation&lookahead=true&RequestType={4}&CurrentModulePagePath={5}";
                        url = UGITUtility.GetAbsoluteURL(string.Format(listpickerUrl, "listpicker", "Picker List", currentModuleName, title, ticketRequestTypeLookup, currentModuleListPagePath, "", ""));
                    }
                    else
                    {
                        listpickerUrl = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&Title={3} &Type=EmptyTitle&lookahead=true&RequestType={4}&CurrentModulePagePath={5}";
                        url = UGITUtility.GetAbsoluteURL(string.Format(listpickerUrl, "listpicker", "Picker List", currentModuleName, title, ticketRequestTypeLookup, currentModuleListPagePath, "", ""));
                    }
                }
            }
            else
            {
                listpickerUrl = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&pageTitle={1}&isdlg=1&isudlg=1&Module={2}&Title={3} &Type=EmptyTitle&lookahead=true&RequestType={4}&CurrentModulePagePath={5}";
                url = UGITUtility.GetAbsoluteURL(string.Format(listpickerUrl, "listpicker", "Picker List", currentModuleName, title, ticketRequestTypeLookup, currentModuleListPagePath, "", ""));
            }
            uHelper.PickerListPopup(Context, url);
        }

        private void CreateShowhideSectionHyperLink(int tabFieldSequence, int sequenceGroupStart, string legend)
        {
            Panel groupedPanel = new Panel();
            groupedPanel.GroupingText = legend;

            if (tabFieldSequence != sequenceGroupStart)
            {
                HyperLink hyperLink = new HyperLink();
                hyperLink.ID = "showHide" + legend;
                hyperLink.CssClass = "imageForToggle showLess";
                hyperLink.Text = "<i id='collapseIcon' class='fas fa-minus'></i>";
                groupedPanel.Controls.Add(hyperLink);
            }
            groupedPanel.Controls.Add(groupedTabTable);
            var row = new TableRow();
            row.BorderStyle = BorderStyle.None;
            row.BorderWidth = 0;
            row.CssClass = "SectionHeader";

            addToTableInSameCell(row, groupedPanel, "", "", 3, newTicketTable);
        }

        private void SetIcons(HtmlGenericControl activeIconDiv, HtmlImage imgSection, string SectionIconUrl, string DefaultIconCss, bool Active = false)
        {
            if (!string.IsNullOrEmpty(SectionIconUrl))
            {
                if (Active)
                    activeIconDiv.Attributes.Add("class", "step_content active");
                else
                    activeIconDiv.Attributes.Add("class", "step_content");

                imgSection.Src = SectionIconUrl;
                imgSection.Visible = true;
            }
            else
            {
                if (Active)
                    activeIconDiv.Attributes.Add("class", $"step_content {DefaultIconCss} active");
                else
                    activeIconDiv.Attributes.Add("class", $"step_content {DefaultIconCss}");
                imgSection.Visible = true;
                imgSection.Src = "/Content/Images/check-icon-transparent.png";
            }
        }
        private void SetIcons(HtmlGenericControl activeIconDiv, HtmlImage imgSection, string SectionIconUrl, string DefaultIconCss, bool Active = false, bool coreColor = false)
        {
            if (!string.IsNullOrEmpty(SectionIconUrl))
            {
                if (Active)
                    activeIconDiv.Attributes.Add("class", "step_content active");
                else
                    activeIconDiv.Attributes.Add("class", "step_content");

                imgSection.Src = SectionIconUrl;
                imgSection.Visible = true;
            }
            else
            {
                if (coreColor)
                    activeIconDiv.Attributes.Add("class", $"{DefaultIconCss}");
                else
                    activeIconDiv.Attributes.Add("class", $"step_content {DefaultIconCss}");
                imgSection.Visible = true;
                imgSection.Src = "/Content/Images/check-icon-transparent.png";
            }
        }
        protected void NewTicketWorkFlowListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string SectionIconUrl = string.Empty;

                LifeCycleStage stage = (LifeCycleStage)e.Item.DataItem;
                ListView repeator = (ListView)sender;
                List<LifeCycleStage> allStages = (List<LifeCycleStage>)repeator.DataSource;
                int stageIdex = allStages.IndexOf(stage);
                int wfCurrentStep = currentStage.StageStep;
                LifeCycleStage cStage = allStages.FirstOrDefault(x => x.ID == currentStage.ID);
                if (cStage != null)
                    wfCurrentStep = cStage.StageStep;

                HtmlGenericControl lineWorkflow = (HtmlGenericControl)e.Item.FindControl("lineWorkflow");


                Label lb = (Label)e.Item.FindControl("sectionSideBarContainer");
                HtmlGenericControl activeIconDiv = (HtmlGenericControl)e.Item.FindControl("activeIconDiv");
                HtmlGenericControl divHelp = (HtmlGenericControl)e.Item.FindControl("divHelp");
                HtmlGenericControl btnHoverContainer = (HtmlGenericControl)e.Item.FindControl("hoverContainer");
                HtmlImage imgSection = (HtmlImage)e.Item.FindControl("imgSection");
                HtmlImage imgShowHelp = (HtmlImage)e.Item.FindControl("imgShowHelp");
                HtmlImage imgShowWorkFlow = (HtmlImage)e.Item.FindControl("imgShowWorkFlow");
                HtmlGenericControl stepIcon = (HtmlGenericControl)e.Item.FindControl("stepIcon");
                HtmlGenericControl tdshowallrelatedticketdiv = (HtmlGenericControl)e.Item.FindControl("tdshowallrelatedticketdiv");
                HtmlGenericControl tdshowallrelatedticketbeforediv = (HtmlGenericControl)e.Item.FindControl("tdshowallrelatedticketbeforediv");

                if (activeIconDiv == null || divHelp == null || btnHoverContainer == null || imgSection == null || imgShowHelp == null || imgShowWorkFlow == null)
                    return;
                btnHoverContainer.Visible = false;
                imgShowHelp.Visible = false;
                imgShowWorkFlow.Visible = false;
                string stageActionBtn = string.Empty;
                if (!string.IsNullOrEmpty(stage.NavigationType) && !string.IsNullOrEmpty(stage.NavigationUrl))
                {
                    btnHoverContainer.Visible = true;
                    imgShowHelp.Visible = true;
                    // divHelp.Attributes.Add("onclick", string.Format(@"javascript:UgitOpenPopupDialog('{0}','','Help - {2}','{3}','{4}',0,'{1}')", stage.NavigationUrl, Server.UrlEncode(Request.Url.AbsolutePath), stage.StageTitle, 100, 100));
                    // divHelp.Attributes.Add("title", "click here for help");
                    imgShowHelp.Attributes.Add("onclick", string.Format(@"javascript:UgitOpenPopupDialog('{0}','','Help - {2}','{3}','{4}',0,'{1}')", stage.NavigationUrl, Server.UrlEncode(Request.Url.AbsolutePath), stage.StageTitle, 100, 100));
                    imgShowHelp.Attributes.Add("title", "click here for help");

                    if (stage.NavigationType.EqualsIgnoreCase("HelpCard"))
                    {
                        // divHelp.Attributes.Add("onclick", string.Format(@"javascript:openHelpCard('{0}','{1}')", stage.NavigationUrl, stage.StageTitle));
                        imgShowHelp.Attributes.Add("onclick", string.Format(@"javascript:openHelpCard('{0}','{1}')", stage.NavigationUrl, stage.StageTitle));

                    }
                }
                if (currentModuleName == "SVC")
                {
                    string stageNameToShowTaskConfig = (ConfigurationVariableManager.GetValue(ConfigConstants.NameOfWorkflowStepToShowTask)).ToString();
                    var arrStageNameToShowTask = stageNameToShowTaskConfig.Split(';');
                    foreach (string stageName in arrStageNameToShowTask)
                    {
                        if (stage.StageTitle.Equals(stageName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            imgShowWorkFlow.Visible = true;
                            btnHoverContainer.Visible = true;
                            // divHelp.Attributes.Add("onclick", "gotoTaskWorkFlow()");
                            imgShowWorkFlow.Attributes.Add("onclick", "gotoTaskWorkFlow()");


                        }


                    }

                }
                if (tdshowallrelatedticketdiv != null)
                {
                    if (stage.StageStep == 1)
                    {
                        tdshowallrelatedticketbeforediv.Attributes.Add("class", "stagerelatedicon stageicon_" + 0);
                        tdshowallrelatedticketdiv.Attributes.Add("class", "stagerelatedicon stageicon_" + 0);
                    }
                    tdshowallrelatedticketdiv.Attributes.Add("class", "stagerelatedicon stageicon_" + stage.StageStep);
                    tdshowallrelatedticketdiv.Attributes.Add("style", "position: unset;");
                }

                lb.Text = stage.StageTitle;
                if (!string.IsNullOrEmpty(stage.IconUrl))
                {
                    SectionIconUrl = stage.IconUrl;
                }


                if (!uHelper.IsCPRModuleEnabled(context))
                {
                    if (stage.StageStep < wfCurrentStep)
                    {

                        SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info", true);
                        lineWorkflow.Attributes.Add("class", "line -background bg-col-blue active");
                    }
                    else if (stage.StageStep == wfCurrentStep)
                    {
                        if (ticketOnHold != 1)
                        {
                            SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info", true);
                            lineWorkflow.Attributes.Add("class", "line -background bg-col-blue active");
                        }
                        else
                        {
                            SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info", true);
                            lineWorkflow.Attributes.Add("class", "line -background bg-col-blue active");
                            stepIcon.Attributes.Add("class", "step_number nodeOnhold");
                        }
                    }
                    else
                    {
                        SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info", false);
                    }

                    if (allStages.LastOrDefault().ID == stage.ID)
                    {
                        lineWorkflow.Attributes.Add("class", "line-background bg-col-blue active");
                    }
                }
                else
                {
                    int resolvedStageStep = 0;
                    int closedStageStep = 0;
                    LifeCycleStage resolvedStage = allStages.FirstOrDefault(x => x.StageTypeChoice == StageType.Resolved.ToString());
                    LifeCycleStage closedStage = allStages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());
                    if (resolvedStage != null)
                        resolvedStageStep = resolvedStage.StageStep;
                    if (closedStage != null)
                        closedStageStep = closedStage.StageStep;
                    if (stage.StageStep < resolvedStageStep)
                    {

                        SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info precon", false, true);
                        lineWorkflow.Attributes.Add("class", "line precon-lines bg-col-blue active");
                    }
                    else if (stage.StageStep >= resolvedStageStep && stage.StageStep < closedStageStep)
                    {
                        SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info const", false, true);
                        lineWorkflow.Attributes.Add("class", "line const-line bg-col-blue active");
                    }
                    else
                    {
                        SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info closeout", false, true);
                        lineWorkflow.Attributes.Add("class", "line clousout-line bg-col-blue active");
                    }

                    if (stage.StageStep > currentStage.StageStep)
                    {
                        SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info lightgrey", false, true);
                    }
                    if (stage.StageStep > currentStage.StageStep - 1)
                    {
                        lineWorkflow.Attributes.Add("style", "border-color:lightgrey;");
                    }
                    if (stage.StageStep == currentStage.StageStep && ticketOnHold == 1)
                    {
                        SetIcons(activeIconDiv, imgSection, SectionIconUrl, "employee-info nodeOnhold", false, true);
                    }
                }
            }
        }

        protected void commentsAwardPopup_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            string buttonname = UGITUtility.ObjectToString(e.Parameter);
            string popupid = String.Empty;
            if(source != null)
            {
                popupid = (source as DevExpress.Web.ASPxPopupControl).ID;
            }
            PopulateEmailFields(popupid);

            if (currentModuleName == "CPR" || currentModuleName == "CNS" || currentModuleName == "OPM")
            {
                string PreConUserGroups = objConfigurationVariableHelper.GetValue("PreConUserGroups");
                //if(!UserManager.IsAdmin(HttpContext.Current.CurrentUser()))
                if (!UserManager.IsUserinGroups(PreConUserGroups, HttpContext.Current.CurrentUser().UserName, Constants.Separator6) && string.IsNullOrEmpty(PreConUserGroups))
                {
                    chkboxStageMoveToPrecon.Checked = false;
                    chkboxStageMoveToPrecon.Enabled = false;
                    lblPermissionMsg.Visible = true;
                    if (source != null && (source as DevExpress.Web.ASPxPopupControl).ID == "statusPopup")
                    {
                        UGITModule module = moduleViewManager.LoadByName("CPR"); //uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "CPR");
                        if (!string.IsNullOrEmpty(module.AuthorizedToCreate))
                        {
                            foreach (string uInfo in module.AuthorizedToCreate.Split(new string[] { "," }, StringSplitOptions.None))
                            {
                                if (UserManager.CheckUserIsInGroup(uInfo, user))
                                {
                                    chkboxStageMoveToPrecon.Checked = true;
                                    chkboxStageMoveToPrecon.Enabled = true;
                                    lblPermissionMsg.Visible = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (source != null && (source as DevExpress.Web.ASPxPopupControl).ID == "statusPopup")
                {
                    ModuleFormLayoutManager moduleFormLayoutManager = new ModuleFormLayoutManager(context);
                    ModuleFormLayout moduleFormLayout = moduleFormLayoutManager.Load(x => x.ModuleNameLookup == currentModuleName && x.FieldName == DatabaseObjects.Columns.ERPJobIDNC).FirstOrDefault();
                    if (moduleFormLayout != null)
                        hndERPJobID.Value = moduleFormLayout.FieldDisplayName;
                    if (string.IsNullOrEmpty(UGITUtility.ObjectToString(hndERPJobID.Value)))
                        hndERPJobID.Value = DatabaseObjects.Columns.ERPJobIDNC;

                    RequestRoleWriteAccessManager roleWriteAccessManager = new RequestRoleWriteAccessManager(HttpContext.Current.GetManagerContext());
                    ModuleRoleWriteAccess roleWriteAccessObj = roleWriteAccessManager.Load(x => x.ModuleNameLookup == currentModuleName
                                            && x.FieldName == DatabaseObjects.Columns.ERPJobIDNC && x.FieldMandatory == true && x.StageStep == currentStep).FirstOrDefault();
                    if (roleWriteAccessObj != null)
                        IsCMICMandatory = true;
                    
                    if (buttonname == "advanceToProject")
                    {
                        trOpportunityStatus.Visible = false;
                        txtStatusMailSubject.Text = $"Opportunity {Convert.ToString(currentTicket[DatabaseObjects.Columns.Title])} Awarded";
                        lblAwardLoss.Text = "Award Date";
                    }
                    else if(buttonname == "statusButton")
                    {
                        trOpportunityStatus.Visible = false;
                        trddlStageToMove.Visible = false;
                        trStageToMove.Visible = false;
                        txtStatusMailSubject.Text = $"Opportunity {Convert.ToString(currentTicket[DatabaseObjects.Columns.Title])} Lost";
                        lblAwardLoss.Text = "Loss Date";
                        madatoryLabel.Visible = true;
                    }
                }

                if(source != null && (source as DevExpress.Web.ASPxPopupControl).ID == "commentsRejectPopup")
                {
                    if(currentModuleName == "CPR" || currentModuleName == "CNS" || currentModuleName == "OPM")
                    {
                        tr3.Visible = false;
                        trToBox.Visible = true;
                    }
                    else
                    {
                        tr3.Visible = true;
                        trToBox.Visible = false;
                    }
                }
            }
        }

        private void PopulateEmailFields(string popupid)
        {
            if ((currentModuleName == "CPR" || currentModuleName == "CNS" || currentModuleName == "OPM") && (chkSendAwardNotification.Checked || chkSendLossEmail.Checked) && currentTicket != null)
            {
                StringBuilder emailBody = new StringBuilder();
                emailBody.AppendFormat("Hello, <br><br>");

                if (currentModuleName != "OPM")
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.CRMProjectID])))
                        emailBody.AppendFormat("<b>Project No: {0} </b><br>", currentTicket[DatabaseObjects.Columns.CRMProjectID]);
                    else
                        emailBody.AppendFormat("<b>Estimate No: {0} </b><br>", currentTicket[DatabaseObjects.Columns.EstimateNo]);
                }

                emailBody.AppendFormat("Project Name: {0} <br>", uHelper.ReplaceInvalidCharsInURL(Convert.ToString(currentTicket[DatabaseObjects.Columns.Title])));

                List<string> lstAddress = new List<string>();
                string address = string.Empty;

                if (!string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.StreetAddress1])))
                    lstAddress.Add(Convert.ToString(currentTicket[DatabaseObjects.Columns.StreetAddress1]));

                if (!string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.City])))
                    lstAddress.Add(Convert.ToString(currentTicket[DatabaseObjects.Columns.City]));

                if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.StateLookup)
                    && !string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.StateLookup])))
                    //lstAddress.Add(uHelper.GetLookupValue(Convert.ToString(currentTicket[DatabaseObjects.Columns.StateLookup])));
                    lstAddress.Add(Convert.ToString(GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.State, DatabaseObjects.Columns.Title, Convert.ToString(currentTicket[DatabaseObjects.Columns.StateLookup]), context.TenantID)));

                if (!string.IsNullOrEmpty(Convert.ToString(currentTicket[DatabaseObjects.Columns.Zip])))
                    lstAddress.Add(Convert.ToString(currentTicket[DatabaseObjects.Columns.Zip]));

                address = string.Join(", ", lstAddress);

                emailBody.AppendFormat("Address: {0} <br>", address);
                emailBody.AppendFormat("Contract Amount: {0:C0} <br/><br/>", currentTicket[DatabaseObjects.Columns.ApproxContractValue]);
                emailBody.AppendFormat("<span id='mailBodyComment'>[Your content goes here]</span>");
                //emailBody.AppendFormat("[Your content goes here]");

                if (currentModuleName != "OPM")
                {
                    emailBody.AppendFormat("<br/><br/>Thanks,<br/>Precon Team<br/>");
                    EmailHtmlBody.Html = emailBody.ToString();
                    EmailLossHtmlBody.Html = emailBody.ToString();
                }
                else
                {
                    hndOpsTitle.Value = uHelper.ReplaceInvalidCharsInURL(Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]));

                    string urlOPM = string.Format("{0}?TicketId={2}&ModuleName={1}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath), "OPM", currentTicket[DatabaseObjects.Columns.TicketId]);
                    string opmLink = string.Format("<a href='{0}' target='_blank'>{1}</a>", urlOPM, currentTicket[DatabaseObjects.Columns.TicketId]);

                    emailBody.AppendFormat("<br/><br/>Please click {0} to view the record. <br/>", opmLink);
                    emailBody.AppendFormat($"<br/><br/>Thanks,<br/>{context.CurrentUser.Name}<br/>");
                    
                    if (popupid == "commentsRejectPopup")
                        EmailLossHtmlBody.Html = emailBody.ToString();
                    else
                        txtStatusMailBody.Html = emailBody.ToString();
                    
                }


                emailBody.Clear();

                string title = "Project Email";
                if(popupid == "commentsRejectPopup") 
                    title = uHelper.ReplaceInvalidCharsInURL(Convert.ToString(currentTicket[DatabaseObjects.Columns.Title])) + " - Cancel";

                if (currentModuleName == "CPR" || currentModuleName == "CNS")
                {
                    if (UGITUtility.IsSPItemExist(currentTicket, DatabaseObjects.Columns.CRMProjectStatus))
                    {
                        title = string.Format("{0}: {1}", currentTicket[DatabaseObjects.Columns.CRMProjectStatus], currentTicket[DatabaseObjects.Columns.Title]);
                    }
                    else
                    {
                        title = Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]);
                    }
                }

                txtMailSubject.Text = title;
                txtMailLossSubject.Text = title;
            }
        }

        protected void CopyProjectTeamFromOPMtoCPR(string OPMticketId, string CPRticketId)
        {
            GlobalRoleManager roleManager = new GlobalRoleManager(context);

            //List<CRMProjectAllocation> collection = ObjCRMProjectAllocationMgr.Load(x => x.TicketId == Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
            //List<ProjectEstimatedAllocation> collection = ObjCRMProjectAllocationMgr.Load(x => x.TicketId == Convert.ToString(saveTicket[DatabaseObjects.Columns.TicketId]));
            List<ProjectEstimatedAllocation> collection = ObjCRMProjectAllocationMgr.Load(x => x.TicketId == OPMticketId);

            List<UserWithPercentage> lstUserWithPercetage = new List<UserWithPercentage>();
            string roleName = string.Empty;

            if (collection != null && collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    //Earlier CRMProjectAllocation is used, now ProjectEstimatedAllocation table is used
                    //CRMProjectAllocation crmProjectAllocation = new CRMProjectAllocation();
                    ProjectEstimatedAllocation crmProjectAllocation = new ProjectEstimatedAllocation();
                    crmProjectAllocation.TicketId = CPRticketId;
                    crmProjectAllocation.PctAllocation = item.PctAllocation;
                    crmProjectAllocation.AssignedTo = item.AssignedTo;
                    crmProjectAllocation.Type = item.Type;
                    crmProjectAllocation.Title = item.Title;
                    crmProjectAllocation.AllocationStartDate = item.AllocationStartDate;
                    crmProjectAllocation.AllocationEndDate = item.AllocationEndDate;
                    crmProjectAllocation.Duration = item.Duration;

                    ObjCRMProjectAllocationMgr.Insert(crmProjectAllocation);

                    GlobalRole uRole = roleManager.Get(x => x.Id == crmProjectAllocation.Type);
                    if (uRole != null)
                        roleName = uRole.Name;
                    else
                        roleName = string.Empty;

                    lstUserWithPercetage.Add(new UserWithPercentage() { UserId = crmProjectAllocation.AssignedTo, Percentage = Convert.ToDouble(crmProjectAllocation.PctAllocation), StartDate = Convert.ToDateTime(crmProjectAllocation.AllocationStartDate), EndDate = Convert.ToDateTime(crmProjectAllocation.AllocationEndDate), RoleTitle = roleName, ProjectEstiAllocId = UGITUtility.ObjectToString(crmProjectAllocation.ID), RoleId = crmProjectAllocation.Type });
                }

                //if (spListItem != null)
                if (!string.IsNullOrEmpty(CPRticketId))
                {
                    bool autoCreateRMMProjectAllocation = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
                    if (autoCreateRMMProjectAllocation)
                    {
                        string webUrl1 = Request.Url.ToString();
                        if (lstUserWithPercetage != null && lstUserWithPercetage.Count > 0)
                        {
                            ThreadStart threadStartMethodUpdateCPRProjectAllocation = delegate () { ResourceAllocationManager.CPRResourceAllocation(context, uHelper.getModuleNameByTicketId(CPRticketId), Convert.ToString(CPRticketId), lstUserWithPercetage); };
                            Thread sThreadStartMethodUpdateCPRProjectAllocation = new Thread(threadStartMethodUpdateCPRProjectAllocation);
                            sThreadStartMethodUpdateCPRProjectAllocation.Start();
                        }
                    }
                }
            }
        }

        protected void CopyExternalTeamFromOPMtoCPR(string OPMticketId, string CPRticketId)
        {
            List<RelatedCompany> lstRelatedCompany = ObjRelatedCompanyMgr.Load(x => x.TicketID == OPMticketId);
            if (lstRelatedCompany != null && lstRelatedCompany.Count > 0)
            {
                foreach (var item in lstRelatedCompany)
                {
                    RelatedCompany relatedCompany = new RelatedCompany();
                    relatedCompany.TicketID = CPRticketId;
                    relatedCompany.ContactLookup = item.ContactLookup;
                    //relatedCompany.CRMCompanyTitleLookup = item.CRMCompanyTitleLookup;
                    relatedCompany.CRMCompanyLookup = item.CRMCompanyLookup;
                    relatedCompany.Title = item.Title;
                    relatedCompany.ItemOrder = item.ItemOrder;
                    relatedCompany.RelationshipTypeLookup = item.RelationshipTypeLookup;

                    ObjRelatedCompanyMgr.Insert(relatedCompany);
                }
            }
        }

        // When a CRMCompany is inactivated (through Workflow), Related Contacts should also be inactivated.
        private void InactivateRelatedContacts(int currentTicketId, string currentTicketPublicID)
        {
            Ticket baseTicket = null;
            //DataTable dtContacts = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $" {DatabaseObjects.Columns.CRMCompanyTitleLookup} = {currentTicketId}");
            DataTable dtContacts = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $" {DatabaseObjects.Columns.CRMCompanyLookup} = '{currentTicketPublicID}' and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            foreach (DataRow item in dtContacts.Rows)
            {
                baseTicket = new Ticket(context, ModuleNames.CON);
                uHelper.CreateHistory(context.CurrentUser, $"Contact Inactivated due to Company with Id: {currentTicketPublicID} Inactivated.", item, context);
                baseTicket.CloseTicket(item, string.Empty);
                baseTicket.CommitChanges(item);
                baseTicket.UpdateTicketCache(item, ModuleNames.CON);
            }
        }

        private void CompleteTasksOnItemClose(string TicketId)
        {
            ThreadStart tsCompleteTasks = delegate () { CompleteStageExitTasks(TicketId); CompleteModuleTasks(TicketId); };
            Thread tCompleteTasks = new Thread(tsCompleteTasks);
            tCompleteTasks.Start();
        }

        private void CompleteStageExitTasks(string ticketId)
        {
            ModuleStageConstraintsManager objModuleStageConstraintsManager = new ModuleStageConstraintsManager(context);
            List<ModuleStageConstraints> moduleTaskItem = objModuleStageConstraintsManager.Load(x => x.TicketId == ticketId && (!x.TaskStatus.EqualsIgnoreCase(Constants.Completed) || x.TaskStatus == null)).ToList();
            if (moduleTaskItem != null && moduleTaskItem.Count > 0)
            {
                moduleTaskItem.ForEach(
                        x =>
                        {
                            x.TaskStatus = Constants.Completed;
                            x.CompletionDate = DateTime.Now;
                            x.CompletedBy = user.Id;
                            x.PercentComplete = 100;
                        });
                objModuleStageConstraintsManager.UpdateItems(moduleTaskItem);
            }
        }

        private void CompleteModuleTasks(string ticketId)
        {
            UGITTaskManager uGITTaskManager = new UGITTaskManager(context);
            List<UGITTask> tasks = uGITTaskManager.LoadByProjectID(uHelper.getModuleNameByTicketId(ticketId), ticketId);
            if (tasks != null && tasks.Count > 0)
            {
                tasks.ForEach(
                        x =>
                        {
                            x.Status = Constants.Completed;
                            x.CompletionDate = DateTime.Now;
                            x.CompletedBy = user.Id;
                            x.PercentComplete = 100;
                        });
                uGITTaskManager.UpdateItems(tasks);
            }
        }

        protected void btnUploadTicketIcon_Click(object sender, EventArgs e)
        {
            if (uploadTicketIcon.UploadedFiles != null && uploadTicketIcon.UploadedFiles.Count() > 0)
            {
                UGITModule moduleObj = ObjModuleViewManager.LoadByName(currentModuleName);
                Ticket ticketObj = new Ticket(context, currentModuleName);
                byte[] imageBytes = uploadTicketIcon.UploadedFiles[0].FileBytes;
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                    {
                        int h = 40;
                        int w = 40;

                        using (Bitmap b = new Bitmap(img, new Size(w, h)))
                        {
                            using (MemoryStream ms2 = new MemoryStream())
                            {
                                b.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imageBytes = ms2.ToArray();
                            }
                        }
                    }
                }

                currentTicket[DatabaseObjects.Columns.IconBlob] = imageBytes;
                ticketObj.CommitChanges(currentTicket);
                if (imageBytes != null)
                    imgTicketIcon.ImageUrl = "data:image;base64," + Convert.ToBase64String((byte[])imageBytes);
            }
        }
        public DataTable GetActualHoursList()
        {
            TicketHoursManager ticketHoursManager = new TicketHoursManager(context);
            List<string> viewFields = new List<string>();
            string query = string.Empty;
            if (TicketIdList != null && TicketIdList.Count > 0)
                query = string.Join(",", TicketIdList.Select(s => $"'{s}'"));
            else
                query = string.Format("'{0}'", currentTicketPublicID);

            viewFields.Add(DatabaseObjects.Columns.TicketId);
            viewFields.Add(DatabaseObjects.Columns.ModuleNameLookup);
            viewFields.Add(DatabaseObjects.Columns.StageStep);
            viewFields.Add(DatabaseObjects.Columns.Resource);
            viewFields.Add(DatabaseObjects.Columns.WorkDate);
            viewFields.Add(DatabaseObjects.Columns.TicketComment);
            viewFields.Add(DatabaseObjects.Columns.HoursTaken);
            string columns = UGITUtility.ConvertListToString(viewFields, Constants.Separator6);
            DataTable dt = ticketHoursManager.GetActualHoursList(query, columns);
            return dt;
        }
        protected void contractSummary_Callback(object sender, CallbackEventArgsBase e)
        {
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
            string documentPath = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/Content/htmlfiles/"), "ContractSummaryReport.html");
            var template = File.ReadAllText(documentPath);
            if (currentTicket != null && template != null)
            {
                string companyName = string.Empty;
                string ContractAssignedTo = context.UserManager.GetUsersProfile().Where(x => x.Id == UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ContractAssignedToUser]))?.FirstOrDefault()?.Name ?? "";
                DataRow dr = ObjTicketManager.GetByTicketIdFromCache(ModuleNames.COM, UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.CRMCompanyLookup]));
                if (dr != null)
                {
                    companyName = UGITUtility.ObjectToString(dr[DatabaseObjects.Columns.Title]);
                }

                template = template.Replace("<<UserName>>", context.CurrentUser.Name)
                    .Replace("<<CMICNO>>", currentModuleName == "OPM"
                        ? (!string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobIDNC]))
                            ? "(" + UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobIDNC]) + ")" : "")
                        : (!string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobID])))
                            ? "(" + UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobID]) + ")" : "")
                    .Replace("<<CMICNUMBER>>", currentModuleName == "OPM"
                        ? !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobIDNC]))
                            ?  UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobIDNC]) : ""
                        : !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobID]))
                            ? UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.ERPJobID]) : "")
                    .Replace("<<SignatureTitle>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.SignatureTitle], "Chief Legal Officer"))
                    .Replace("<<ProjectTitle>>", UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.Title]))
                    .Replace("<<CurrentDate>>", DateTime.Now.ToString("MMM d, yyyy"))
                    .Replace("<<ContractType>>", UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.OwnerContractTypeChoice]))
                    .Replace("<<Fee>>", !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.FeePct])) ? UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.FeePct]) + "%" : "")
                    .Replace("<<ContractAmount>>", "$" + UGITUtility.StringToInt(currentTicket[DatabaseObjects.Columns.ApproxContractValue]).ToString("#,##0"))
                    .Replace("<<StartDate>>", UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]), false))
                    .Replace("<<EndDate>>", UGITUtility.GetDateStringInFormat(Convert.ToString(currentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]), false))
                    .Replace("<<LiquidatedDamages>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.LiquidatedDamages]))
                    .Replace("<<Bonus>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.Bonus]))
                    .Replace("<<Savings>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.Savings]))
                    .Replace("<<GeneralConditionForDelay>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.GeneralConditionsDelay]))
                    .Replace("<<Payments>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.Payments]))
                    .Replace("<<WaiverOfConsequential>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.WaiverDamages]))
                    .Replace("<<LienWaivers>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.LienWaiver]))
                    .Replace("<<Retainage>>", UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.RetainageChoice]))
                    .Replace("<<ChangeOrders>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.ChangeOrders]))
                    .Replace("<<SubcontractorMarkUps>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.SubContractorMarkUp]))
                    .Replace("<<Insurance>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.Insurance]))
                    .Replace("<<BuilderRisk>>", UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.BuilderRisk]) ? "Yes" : "No")
                    .Replace("<<WaiverofSubrogation>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.WaiverSubrogation]))
                    .Replace("<<DisputeResolution>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.DisputeResolution]))
                    .Replace("<<DisputedWorkCap>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.DisputedWorkCap]))
                    .Replace("<<SubcontractorDefaultInsurance>>", UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.SubcontractorDefaultInsurance]) ? "Yes" : "No")
                    .Replace("<<PaymentBond>>", UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.PaymentAndPerformanceBonds]) ? "Yes" : "No")
                    .Replace("<<DiverseCertificationChoice>>", UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.DiverseCertificationChoice]))
                    .Replace("<<ProjectType>>", fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.RequestTypeLookup, UGITUtility.ObjectToString(this.currentTicket[DatabaseObjects.Columns.RequestTypeLookup])))
                    .Replace("<<ExecutiveOrder>>", UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.ExecutiveOrderRequirements]) ? "Yes" : "No")
                    .Replace("<<SignedNDA>>", UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.SignedNDA]) ? "Yes" : "No")
                    .Replace("<<Contingency>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.Contingency]))
                    .Replace("<<SubstantialCompletion>>", UGITUtility.GetDateStringInFormat(UGITUtility.ObjectToString(currentTicket[DatabaseObjects.Columns.SubstantialCompletion]), false))
                    .Replace("<<Warranties>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.Warranties]))
                    .Replace("<<SpecialProvisions>>", UGITUtility.GetNullOrDefaultString(currentTicket[DatabaseObjects.Columns.SpecialProvisions]))
                    .Replace("<<ContractAssignedTo>>", ContractAssignedTo)
                    .Replace("<<CRMClient>>", companyName);

                using (DevExpress.XtraRichEdit.RichEditDocumentServer wordProcessor = new DevExpress.XtraRichEdit.RichEditDocumentServer())
                {
                    wordProcessor.HtmlText = template;
                    DevExpress.XtraPrinting.PdfExportOptions options = new DevExpress.XtraPrinting.PdfExportOptions();
                    options.DocumentOptions.Author = "";
                    options.Compressed = false;
                    options.ImageQuality = DevExpress.XtraPrinting.PdfJpegImageQuality.Highest;
                    string outputPath = uHelper.GetTempFolderPath();
                    string pdfFilePath = string.Empty;
                    pdfFilePath = System.IO.Path.Combine(outputPath, "ContractSummaryReport_"+ currentTicketPublicID+ ".pdf");
                    using (FileStream pdfFileStream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        wordProcessor.ExportToPdf(pdfFileStream, options);
                    }
                    UGITUtility.CreateCookie(Response, "ReportGenerated", "1");
                    //System.Diagnostics.Process.Start(pdfFilePath);
                }
            }
        }

        protected void ASPxCallbackPanel_Actions_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter != null)
            {
                if (currentModuleName != ModuleNames.CPR)
                    return;

                string[] query = e.Parameter.Split('&');
                string action = query[0].Split('=')[1];
                List<string> ticketIDs = query[1].Split('=')[1].Split(',').ToList();

                if (action == "UpdateStatistics")
                {
                    _statisticsManager.ProcessStatistics(ticketIDs, _moduleViewManager.LoadByName(currentModuleName));
                    UGITUtility.CreateCookie(Response, SELECTEDTABCOOKIE, hdnActiveTab.Value);
                    try
                    {
                        HttpContext.Current.Response.Redirect(Request.Url.ToString());
                    }
                    catch (ApplicationException)
                    {
                        HttpContext.Current.Response.RedirectLocation = Request.Url.ToString();
                    }
                }
            }
        }
    }
}
