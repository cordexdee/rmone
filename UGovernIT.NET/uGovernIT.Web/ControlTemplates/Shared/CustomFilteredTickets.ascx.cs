using DevExpress.Web;
using DevExpress.Web.Rendering;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Helper;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.Helpers;
using DevExpress.Data.Filtering;
using DevExpress.XtraPrinting;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using System.Runtime.Remoting.Messaging;
using Antlr.Runtime.Misc;
using System.IO;
using uGovernIT.Web.Modules;

namespace uGovernIT.Web
{
    public partial class CustomFilteredTickets : UserControl
    {
        #region Properties

        public bool HideReport { get; set; }
        public bool HideModuleDetail { get; set; }
        public bool HideStatusOverProgressBar { get; set; }
        public bool HideModuleDesciption { get; set; }
        public bool HideModuleImage { get; set; }
        public bool HideMyFiltersLinks { get; set; }
        public bool HideNewTicketButton { get; set; }
        public bool showExportIcons { get; set; } = true;
        public int PageSize { get; set; }
        public bool EnableNewButton { get; set; }

        public string IsTrailUserDir { get; set; }
        public string ServiceID { get; set; }
        public bool IsHomedashboardCtrl { get; set; }
        public bool IsShowResourceAllocationBtn { get; set; }
        public string ViewName { get; set; }
        public string ModuleName
        {
            get
            {
                if (ViewState["modulename"] == null)
                    return string.Empty;
                else
                    return Convert.ToString(ViewState["modulename"]);
            }
            set { ViewState["modulename"] = value; }
        }
        public string UserType { get; set; }
        public bool IsPreview { get; set; }
        public bool HidePager { get; set; }
        public bool IsDashboard { get; set; }
        public bool IsInIframe { get; set; }
        public DataTable FilteredTable { get; set; }
        public bool HideGlobalSearch { get; set; }
        public bool isGlobalSearch = false;
        public bool ShowActionColumn { get; set; }
        public bool isLocalSearch = false;
        public bool DisableStateManagment { get; set; }
        public string SortString { get; set; }
        public string[] columnStringArray = { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title };
        public string HelpUrl { get; set; }
        public string URLFieldName { get; set; }
        public DataTable ColumnsAggregate { get; set; }
        public string PopupWidth { get; set; }
        public string PopupHeight { get; set; }
        public bool FilterMode { get; set; }

        // new property. for my home project..
        public string MyHomeTab { get; set; }
        public bool IsHomePage { get; set; }
        public string CategoryName { get; set; }
        public bool IsLoginWizard { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public bool DisplayOnDashboard { get; set; }
        /// <summary>
        /// 1 for Open, 2 for closed and vice-versa
        /// </summary>
        public TicketStatus MTicketStatus { get; set; }
        public bool IsFilteredTableExist { get; set; }
        public bool HideAllTicketTab { get; set; }
        //SpDelta 42(Implementation of asset/ticket import)
        public UGITModule currentUGITModule = null;
        //private bool showHoldUnholdOptions = false;
        public string importTitle = string.Empty;
        protected string AppCMDBImportTitle = string.Empty;
        //
        public bool ShowClearFilter
        {
            get
            {
                return showClearFilter;
            }

            set
            {
                showClearFilter = value;
            }
        }
        public string MoreUrl { get; set; }

        /// <summary>
        /// Used to transfer tabname information from Home Page
        /// </summary>
        public string HomeTabName { get; set; }
        public bool ShowTimeToo { get; set; }
        public string TimeUnit { get; set; }

        public string TicketNavigationURL { get; set; }
        public bool IsSLAMetricsDrilldown { get; set; }

        public bool ShowCompactView { get; set; }
        public bool ShowBandedrows { get; set; }
        public bool IsManagerView { get; set; }
        public string ManagerViewModule { get; set; }

        public int CloseOutPeriod
        {
            get
            {
                return uHelper.getCloseoutperiod(_context);
            }
        }
        
        public bool HasAccessToCreateTemplate {
            get
            {
                return !uHelper.HideAllocationTemplate(HttpContext.Current.GetManagerContext())
                    && UserManager.IsResourceAdmin(HttpContext.Current.CurrentUser());
            }
        }
        #endregion

        #region Variables
        string ticketTitle = string.Empty;
        string prefixTitle = string.Empty, prefixcolumn = DatabaseObjects.Columns.TicketId;
        public bool selectOnly;
        public string selectedTicketId;
        public DateTime globalStartDate;
        public DateTime globalEndDate;
        public string reportUrl = string.Empty;
        public int tabCount = 0;
        public string createDuplicateUrl = string.Empty;
        public string urlSubTicket;
        public UserProfile User;
        public UserProfileManager UserManager;

        protected bool isPopup;
        protected int rowCount;
        protected DataTable resultedTable;
        protected string typeName = "Ticket";
        //protected string sortExpression = "";
        protected string sourceURL;
        protected string ganttReportUrl = string.Empty;
        protected string projectReportUrl = string.Empty;
        protected string projectReportSummaryUrl = string.Empty;
        protected string applicationReportUrl = string.Empty;
        protected string moduleRowTitle;
        //ticket url
        protected string ticketURL = string.Empty;
        protected string wildCardWaterMark = "Search String";
        protected bool isWildCardFiltered;
        protected string importUrl;
        protected bool enableTicketListExport;
        protected string summaryUrl;
        protected string summaryPopupTitle;
        protected bool initiateExport;
        protected string exportPath;
        protected string exportType;
        protected string reportPath;
        protected string bottelNeckChartPath;
        protected string filterDetail = string.Empty;
        protected string TicketURlBatchEditing = string.Empty;
        protected string waitingOnMeTabName = "WaitingOnMeTabName"; //ConfigurationVariable.GetValue("WaitingOnMeTabName", "Waiting On Me");
        protected string editTaskFormUrl = string.Empty;
        protected bool isAuthorizedToViewTicket;
        //protected SPListItem currentTicket;
        protected bool isActionUser;
        protected string NPRprojectReportSummaryUrl = string.Empty;
        protected ModuleStatisticResponse moduleStatResult;
        protected bool enableModuleAgent;
        protected string clipboardUrl = string.Empty;
        protected int holdMaxStage = 0;
        public bool IsServiceDocPanel { get; set; }
        public bool ColumnSorted { get; set; }

        protected bool showClearFilter;
        private bool showNewTicketAtHome;
        private string queryString = string.Empty;
        private string subQueryString = string.Empty;
        private string viewTicketsPath;
        private string wildcard = string.Empty;
        private string globalSearchString = string.Empty;
        protected int autoRefreshListFrequency = 0;

        private DataTable projectMonitorsStateTable;
        private DataTable moduleMonitorOptions;
        ModuleMonitorOptionManager MonitorOptionManagerObj;
        private DataRow[] moduleMonitorsTable;
        private Ticket moduleRequest;

        //Import Variable
        private string absoluteUrlImport = "Layouts/uGovernIT/DelegateControl.aspx?control={0}&listName={1}";
        public string NewCompanyUrl = UGITUtility.GetAbsoluteURL("/Pages/COM?");
        public string NewContactUrl = UGITUtility.GetAbsoluteURL("/Pages/AddContact");
        public string ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/RMOne/");

        private DataRow moduleRow;
        private List<ModuleColumn> moduleColumns;
        private List<ModuleColumn> moduleCols;
        private List<ModuleColumn> moduleCols_;
        private string globalFilterDetialForPDF = string.Empty;
        private DataTable relationalTickets;
        //private string TenantID = string.Empty;
        private bool isAdmin;
        private bool isHelpDesk;

        protected string ajaxHelperPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ajaxhelper.aspx");
        protected string AccountControllerURL = UGITUtility.GetAbsoluteURL("/api/Account");
        protected string TicketTemplateURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplatelist");
        protected string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx");
        protected string batchCreateURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=batchCreate");
        public string TicketReAssignmentUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketReAssignment");
        public string TicketManualEscalationUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ManualEscalation");
        public string TicketCloseOrRejectUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketCloseOrReject");
        public string TicketReOpenUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketReOpen");
        public string ServiceURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard&serviceID=")); //UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/ServiceWizard.aspx?serviceID=");
        public string groupScorecardPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=groupscorecard");
        public string ProjectSimilarityUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=projectsimilaritycontrol");
        public string ProjectPlanUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=ProjectPlan");
        public string ProjectCardViewUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=ProjectCard");
        public string TicketHoldUnHoldUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketHoldUnHold");
        protected string svcprojectTaskListPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/delegatecontrol.aspx?control=projecttasklist");
        public string TrackProjectStageUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=trackprojectstagehistory");
        protected string WinAndLossReportURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=winandlossreport");
        public string NewLeadUrl = UGITUtility.GetAbsoluteURL("/Pages/LEM?moduleFrom=COM");
        public string NewOpportunityUrl = UGITUtility.GetAbsoluteURL("/Pages/OPM?moduleFrom=LEM");
        public string NewProjectUrl = UGITUtility.GetAbsoluteURL("/Pages/CPR?moduleFrom=OPM");
        public string NewOPMUrl = UGITUtility.GetAbsoluteURL("/Pages/OPM");
        public string NewProjectSummaryPageUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=projectsummary");
        public string NewOPMWizardPageUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=newopmwizard");
        public string NewContactSummaryPageUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=crmcontactdetails");
        public string CreateProjectTagsUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=createprojecttags");
        //public string UpdateStatisticsUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=updatestatistics");

        public FieldConfiguration field = null;
        public ModuleColumn moduleColumn = null;

        //Custom report for crm

        protected string CombinedLostJobReportURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=combinedlostjobreport");
        protected string CoreServiceReportURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=coreservicereport");
        protected string BusinessUnitDistributionReportURL = UGITUtility.GetAbsoluteURL("/layouts/uGovernIT/DelegateControl.aspx?control=BusinessUnitDistributionReport");
        protected bool defaultBanding;
        protected bool EnableCustomizeColumns;
        public string allocationGanttUrl = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/DelegateControl.aspx?control=ProjectGantt");
        #endregion

        #region Events

        public ConfigurationVariableManager _configurationVariableManager;
        private TicketRelationshipHelper _ticketRelationshipHelper;
        public ApplicationContext _context;
        private DashboardManager _dashboardManager = null;
        private DashboardPanelViewManager _dashboardPanelViewManager = null;
        private PrioirtyViewManager _priorityViewManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private ModuleFormTabManager _moduleFormTabManager = null;
        private FieldConfigurationManager _fmanger = null;
        private StatisticsManager _statisticsManager = null;
        private DepartmentManager _departmentManager= null;
        private TicketManager _ticketManager = null;

        string previousTab = string.Empty;
        string currentTab = string.Empty;
        string activeTab = string.Empty;
        List<ProjectEstimatedAllocation> projectAllocations = null;
        //ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fmanger == null)
                {
                    _fmanger = new FieldConfigurationManager(_context);
                }
                return _fmanger;
            }
        }

        protected DashboardManager DashboardManager
        {
            get
            {
                if (_dashboardManager == null)
                {
                    _dashboardManager = new DashboardManager(_context);
                }
                return _dashboardManager;
            }
        }

        protected DashboardPanelViewManager DashboardPanelViewManager
        {
            get
            {
                if (_dashboardPanelViewManager == null)
                {
                    _dashboardPanelViewManager = new DashboardPanelViewManager(_context);
                }
                return _dashboardPanelViewManager;
            }
        }

        protected PrioirtyViewManager PriorityViewManager
        {
            get
            {
                if (_priorityViewManager == null)
                {
                    _priorityViewManager = new PrioirtyViewManager(_context);
                }
                return _priorityViewManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(_context);
                }
                return _moduleViewManager;
            }
        }

        protected ModuleFormTabManager ModuleFormTabManager
        {
            get
            {
                if (_moduleFormTabManager == null)
                {
                    _moduleFormTabManager = new ModuleFormTabManager(_context);
                }
                return _moduleFormTabManager;
            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(_context);
                }
                return _ticketManager;
            }
        }

        public CustomFilteredTickets()
        {
            _context = HttpContext.Current.GetManagerContext();
            _ticketRelationshipHelper = new TicketRelationshipHelper(_context);
            _configurationVariableManager = new ConfigurationVariableManager(_context);
            _moduleViewManager = new ModuleViewManager(_context);
            _priorityViewManager = new PrioirtyViewManager(_context);
            _moduleFormTabManager = new ModuleFormTabManager(_context);
            _ticketManager = new TicketManager(_context);
            _dashboardPanelViewManager = new DashboardPanelViewManager(_context);
            _departmentManager = new DepartmentManager(_context);
            //[+][SANKET][21-08-2023][Added manager for type binding in CRM Company]
            //_relationshipTypeManager = new CRMRelationshipTypeManager(_context);
            _statisticsManager = new StatisticsManager(_context);
            ModuleName = string.Empty;
            UserType = "all";
            MTicketStatus = TicketStatus.All;
            PageSize = 15;
            moduleColumns = new List<ModuleColumn>();
        }

        void Page_PreInit(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            if (IsManagerView)
                ModuleName = ManagerViewModule;
            defaultBanding = _configurationVariableManager.GetValueAsBool(ConfigConstants.DisableRequestListBanding);
            if (ModuleName == "CON")
                btnNewContact.Visible = true;

            if (Request.QueryString.AllKeys.Contains("IsServiceDocPanel"))
            {
                if (Convert.ToBoolean(Request.QueryString["IsServiceDocPanel"]))
                {
                    cTicketPreviewPanel.Visible = false;
                    grid.Visible = false;
                    mModuleNewTicketPanel.Visible = false;
                }
            }

            #region ITHELPDESK
            if (Request.Path == "/pages/HomeHelpDeskTSR" || Request.Path == "/pages/HomeHelpDeskDRQ" || Request.Path == "/pages/HomeHelpDeskACR" || Request.Path == "/pages/HomeHelpDeskSVC")
            {
                TSR.Visible = true;
                ACR.Visible = true;
                DRQ.Visible = true;
                SVC.Visible = true;
            }
            #endregion
            if (ModuleName == ModuleNames.PMM)
            {
                MonitorOptionManagerObj = new ModuleMonitorOptionManager(_context);
                editTaskFormUrl = UGITUtility.GetAbsoluteURL("/ControlTemplates/uGovernIT/edittask.aspx");
            }
            if (Request.QueryString.AllKeys.Contains("enablenewbutton"))
                EnableNewButton = Request.QueryString["enablenewbutton"] != null ? Convert.ToBoolean(Request.QueryString["enablenewbutton"]) : false;
            IsPreview = Request.QueryString["IsPreview"] != null ? Convert.ToBoolean(Request.QueryString["IsPreview"]) : IsPreview;
            IsDashboard = Request.QueryString["IsDashboard"] != null ? Convert.ToBoolean(Request.QueryString["IsDashboard"]) : IsDashboard;
            if (Request.QueryString.AllKeys.Contains("dID"))
                IsDashboard = true;
            urlSubTicket = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx");
            User = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            if (IsHomePage == true)
                rootDiv.Attributes.Add("class", "homeDb_gridContainer ticket_contentWrap table-responsive col-md-12 col-sm-12 col-xs-12 px-0");

            if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
            {
                ASPxPopupCustomFilterActionMenu.PopupVerticalAlign = PopupVerticalAlign.Below;
                ASPxPopupCustomFilterActionMenu.PopupHorizontalAlign = PopupHorizontalAlign.Center;
                ExportPopupMenu.PopupVerticalAlign = PopupVerticalAlign.Below;
                ExportPopupMenu.PopupHorizontalAlign = PopupHorizontalAlign.OutsideRight;
            }

            grid.SettingsBehavior.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            grid.SettingsPager.AlwaysShowPager = true;
            grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            grid.SettingsPager.ShowDisabledButtons = true;
            grid.SettingsPager.ShowNumericButtons = true;
            grid.SettingsPager.NumericButtonCount = 5;
            grid.SettingsPager.ShowSeparators = true;
            grid.SettingsPager.ShowDefaultImages = true;
            grid.AutoGenerateColumns = false;
            grid.SettingsText.EmptyHeaders = "  ";
            grid.SettingsBehavior.AllowSelectByRowClick = true;
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
            grid.Settings.GridLines = GridLines.Horizontal;
            grid.Styles.Header.CssClass = ShowCompactView ? "homeGrid_headerColumn-c" : "homeGrid_headerColumn";
            grid.SettingsCommandButton.ShowAdaptiveDetailButton.Styles.Style.CssClass = "homeGrid_openBTn";
            grid.SettingsCommandButton.HideAdaptiveDetailButton.Styles.Style.CssClass = "homeGrid_closeBTn";
            grid.SettingsPopup.HeaderFilter.Width = 300;
            grid.SettingsPopup.HeaderFilter.Height = 300;
            grid.StylesPopup.HeaderFilter.Content.CssClass = "ITSMSearchFilter_content";
            grid.StylesPopup.HeaderFilter.ButtonPanel.CancelButton.CssClass = "ITSMFilter_cancelBtn";
            grid.StylesPopup.HeaderFilter.ButtonPanel.OkButton.CssClass = "ITSMFilter_okBtn";
            grid.StylesPopup.HeaderFilter.Footer.CssClass = "ITSMFilterFooter_btnWrap";
            grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            grid.SettingsBehavior.AllowDragDrop = false;
            grid.Settings.ShowHeaderFilterBlankItems = false;
            grid.SettingsPager.Position = PagerPosition.Bottom;
            grid.SettingsPopup.HeaderFilter.Width = 300;
            grid.SettingsPopup.HeaderFilter.Height = 300;
            grid.EnableRowsCache = false;
            if (!IsPreview && _configurationVariableManager.GetValueAsBool(ConfigConstants.EnablePowerSearch))
            {
                grid.Settings.ShowFilterBar = GridViewStatusBarMode.Hidden;
                grid.SettingsFilterControl.ViewMode = FilterControlViewMode.VisualAndText;
                grid.SettingsFilterControl.AllowHierarchicalColumns = false;
                grid.SettingsFilterControl.ShowAllDataSourceColumns = true;
                grid.SettingsFilterControl.HierarchicalColumnPopupHeight = 200;
                grid.SettingsFilterControl.VisualTabText = "Power Search";
                grid.SettingsFilterControl.MaxHierarchyDepth = 0;
                grid.StylesFilterControl.Operation.CssClass = "PowerSearch";
                grid.FilterControlColumnsCreated += Grid_FilterControlColumnsCreated;
                grid.CustomJSProperties += grid_CustomJSProperties;
            }

            importTitle = string.Format("Import {0}", UGITUtility.moduleTypeName(this.ModuleName));
            importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", DatabaseObjects.Tables.Assets));

            // Get UGIT Module
            UGITModule currentUGITModule = _moduleViewManager.LoadByName(this.ModuleName);
            if (currentUGITModule != null)
            {
                absoluteUrlImport = absoluteUrlImport.Replace("&listName", "&Module");
                importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", currentUGITModule.ModuleName));
                holdMaxStage = currentUGITModule.ModuleHoldMaxStage;

                //do not show hold and un-hold option in action menu if maxholdstage at module level set to zero
                //For PMM since we use custom lifecycles, enable in all stages
                //if (currentUGITModule.HoldMaxStage > 0 || this.ModuleName == "PMM")
                //    showHoldUnholdOptions = true;

                //set navigation url to switch between next and previous ticket on modulewebpart
                TicketNavigationURL = UGITUtility.GetAbsoluteURL(currentUGITModule.StaticModulePagePath);
            }

            if(ModuleName != null && ModuleName == ModuleNames.CMDB)
                relationalTickets = _ticketRelationshipHelper.GetTicketRelationshipDT(); //TicketRelationship.LoadRelationshipData(SPContext.Current.Web);

            //Check if current user is super-admin or ticket admin(Configuration Variable)
            isAdmin = UserManager.IsUGITSuperAdmin(User) || UserManager.IsTicketAdmin(User) || UserManager.IsAdmin(User);

            // Check if current user is part of helpdesk group
            isHelpDesk = UserManager.CheckUserIsInGroup(_configurationVariableManager.GetValue("HelpDeskGroup"), User);

            BindModuleFormTab();
            // Set page size
            if (!IsPostBack)
            {
                //new dropdown for sms modules..
                BindModuleListItem();
                //BindModuleFormTab();
                FillModules();

                if (this.DisplayOnDashboard && !string.IsNullOrEmpty(Width))
                {
                    rootDiv.Style.Add("width", Width);
                    rootDiv.Style.Add("height", Height);
                    rootDiv.Style.Add("overflow-y", "auto");
                    rootDiv.Style.Add("padding", "16px");
                    grid.SettingsCookies.StorePaging = false;
                }

                if (!IsPreview && !DisplayOnDashboard)
                {
                    int cookiespageSize = 0;
                    int.TryParse(UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants._GridPageSize), out cookiespageSize);
                    if (cookiespageSize != 0)
                        PageSize = cookiespageSize;
                }
            }

            if (IsPreview)
            {
                if (PageSize > 0)
                    grid.SettingsPager.PageSize = PageSize;
                else
                    grid.SettingsPager.PageSize = 10;

                // grid.SettingsBehavior.AllowSort = false;
                grid.Settings.ShowHeaderFilterButton = true;
                grid.SettingsCookies.StorePaging = false;


            }
            else if (PageSize > 0)
                grid.SettingsPager.PageSize = PageSize;
            else if (IsDashboard)
                grid.SettingsPager.PageSize = PageSize = 15;
            else
                grid.SettingsPager.PageSize = PageSize = 20;

            bool advanceMode;
            bool.TryParse(UGITUtility.GetCookieValue(Request, Constants.ShowAdvanceFilter), out advanceMode);
            FilterMode = advanceMode;
            string frmTrailUser = string.Empty;

            // code to hide/show toolbar contains customize columns on grid.
            if (grid.Toolbars.Count > 0)
            {
                EnableCustomizeColumns = _configurationVariableManager.GetValueAsBool(ConfigConstants.EnableCustomizeColumns);
                grid.Toolbars.FindByName("CustomizeColumns").Visible = EnableCustomizeColumns;
            }

            ConfigureTemplateGrid();

            //Fix for BTS-22-000793: My Projects not showing on Home Page
            /*
            if (ModuleName == "PMM" || (IsHomePage && !string.IsNullOrEmpty(MyHomeTab) && (MyHomeTab.ToLower() == "myprojecttab" || MyHomeTab.ToLower() == "businessinitiatives")))
            {
                this.ModuleName = "PMM";
            }
            */

            base.OnInit(e);
        }

        private void Grid_FilterControlColumnsCreated(object source, FilterControlColumnsCreatedEventArgs e)
        {
            if (moduleColumns != null)
            {
                List<string> removedColumns = new List<string>();
                foreach (FilterControlColumn column in e.Columns)
                {

                    ModuleColumn row = moduleColumns.FirstOrDefault(x => x.FieldName == column.PropertyName);
                    if (row != null)
                    {
                        column.DisplayName = Convert.ToString(row.FieldDisplayName);
                        if (string.IsNullOrWhiteSpace(column.DisplayName))
                            column.DisplayName = column.PropertyName;
                        if (column.PropertyName.Contains("Lookup"))
                        {
                            column.ColumnType = FilterControlColumnType.String;

                        }
                    }
                    else
                    {
                        removedColumns.Add(column.PropertyName);
                    }
                }


                foreach (var dc in removedColumns)
                {
                    e.Columns.RemoveAt(e.Columns[dc].Index);
                }

                List<FilterControlColumn> sColumns = e.Columns.Cast<FilterControlColumn>().OrderBy(x => x.DisplayName).ToList();
                e.Columns.Clear();
                foreach (FilterControlColumn cl in sColumns)
                {
                    if (cl.PropertyName.Contains("Lookup"))
                        cl.ColumnType = FilterControlColumnType.String;
                    e.Columns.Add(cl);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HasAccessToCreateTemplate)
                pcccRequestTypeChange.Visible = true;
            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(_context);
            projectAllocations = cRMProjectAllocationManager.Load(x => x.Deleted != true);

            clipboardUrl = string.Format("{0}?", UGITUtility.ToAbsoluteUrl(Constants.HomePagePath));
            reportPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=report");
            ganttReportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ganttreport");
            enableTicketListExport = false;
            bottelNeckChartPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=workflowbottleneck");
            reportUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            if (Request["ShowBandedRows"] != null)
                ShowBandedrows = UGITUtility.StringToBoolean(Request["ShowBandedRows"]);
            if (Request["ShowCompactRows"] != null)
                ShowCompactView = UGITUtility.StringToBoolean(Request["ShowCompactRows"]);
            if (UGITUtility.StringToBoolean(Request["initiateExport"]) || UGITUtility.StringToBoolean(hdInitiateExport.Value))
            {
                initiateExport = true;
                exportType = Request["exportType"];
                if (!string.IsNullOrEmpty(hdExportType.Value))
                    exportType = hdExportType.Value.Trim();
            }
            sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            viewTicketsPath = UGITUtility.GetAbsoluteURL(_configurationVariableManager.GetValue("FilterTicketsPageUrl"));

            if (sourceURL.Contains("/default.aspx"))
                ddlCustomFilterHome.Visible = false;
            if (IsPostBack)
            {
                if (hndFilterStatus.Value.Contains("__ShowFilterMode__"))
                    FilterMode = true;
                else if (hndFilterStatus.Value.Contains("__HideFilterMode__"))
                    FilterMode = false;
            }
            else
            {
                //Set default value in global filter
                txtWildCard.Text = wildCardWaterMark;
                txtWildCard.CssClass = "WaterMarkClass";
                txtWildCard.Attributes.Add("onfocus", "WatermarkFocus(this, '" + wildCardWaterMark + "');");
                txtWildCard.Attributes.Add("onblur", "WatermarkBlur(this, '" + wildCardWaterMark + "');");
                if (!DisableStateManagment)
                {
                    UGITUtility.DeleteCookie(Request, Response, Constants.UseManageStateCookies);
                    // Get the value from cookies when no value is entered in search box and cookies has some value for search.
                    if (txtWildCard.Text.Trim().Replace(wildCardWaterMark, string.Empty) == string.Empty)
                        txtWildCard.Text = UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants.WildCardExpression);

                    DateTime filterDate;
                    if (DateTime.TryParse(UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants.FromDateExpression), out filterDate))
                        dtFrom.Date = filterDate;

                    if (DateTime.TryParse(UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants.ToDateExpression), out filterDate))
                        dtTo.Date = filterDate;
                }
            }

            if (FilterMode)
            {
                if (!IsPreview)
                {
                    grid.Settings.ShowFilterRow = true;
                    grid.Settings.ShowFilterRowMenu = true;
                    imgAdvanceMode.Attributes.Add("title", "Simple Filter");
                    imgAdvanceMode.Src = UGITUtility.GetAbsoluteURL("/Content/Images/red-filter.png");
                }
                grid.Settings.ShowHeaderFilterButton = false;

            }
            else
            {
                grid.Settings.ShowFilterRow = false;
                if (!IsPreview)
                    grid.Settings.ShowHeaderFilterButton = true;
                imgAdvanceMode.Attributes.Add("title", "Advanced Filter");
                imgAdvanceMode.Src = UGITUtility.GetAbsoluteURL("/Content/Images/Newfilter.png");
            }

            //GetSelected ticket id to do some operation on it
            selectedTicketId = selectedTicketIdHidden.Value;
            //Assign default class to tabs
            //filterTab.ActiveTab = null;
            // Apply Filtering, sorting , paging on gridview and also specify page size
            cTicketPreviewPanel.Visible = false;
            if (IsPreview)
            {
                cTicketPreviewPanel.Visible = true;
                grid.SettingsPager.Visible = false;
            }

            if (_configurationVariableManager.GetValueAsBool(ConfigConstants.DisableCustomFilterTab))
            {
                ddlCustomFilterHome.ClientVisible = true;
                filterTab.ClientVisible = false;
            }
            else
            {
                ddlCustomFilterHome.ClientVisible = false;
                filterTab.ClientVisible = true;
            }

            moreTicketLink.Visible = false;

            //Show module dropdown when globalSearchString come in querystring
            globalfilterByModule.Visible = false;
            if (Request.QueryString["isGlobalSearch"] == "true")
            {
                globalfilterByModule.Visible = true;
                pGlobalFiltersTable.Attributes.Add("width", "950px");
                if (ddlModuleName.Items.Count <= 0)
                {

                    DataTable dtModuleList = ModuleViewManager.LoadAllModules();  //uGITCache.GetModuleList(ModuleType.All);
                    if (dtModuleList.Columns.Contains(DatabaseObjects.Columns.UseInGlobalSearch))
                    {
                        DataRow[] modulesRows = dtModuleList.Select(string.Format("{0} = {1}", DatabaseObjects.Columns.UseInGlobalSearch, true));
                        foreach (DataRow row in modulesRows)
                        {
                            ddlModuleName.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.ModuleName]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                        }

                        ddlModuleName.Items.Insert(0, new ListItem("--All--", "0"));
                        // set the "All" in Field Drop Down.
                        lstFilteredFields.Items.Insert(0, new ListItem("--All--", "0"));
                    }
                }

                if (!IsPostBack)
                {
                    txtWildCard.Text = Convert.ToString(Request.QueryString["globalSearchString"]);
                }
            }


            if (Request.QueryString["ExperienceTagLookup"] != null)
            {
                HideMyFiltersLinks = true;
                HideModuleDesciption = true;
            }

            // Get module details
            // temp assignment
            if (Request.QueryString["Module"] != null)
                ModuleName = Convert.ToString(Request.QueryString["Module"]);
            else
                ModuleName = ModuleName;

            if (!string.IsNullOrEmpty(ModuleName))
            {
                if (filterTab.Tabs.Count <= 0)
                {
                    filterTab.CssClass = "ActiveTabClass";
                    //Temp code to test dynamic tabs
                    TabViewManager tabViewManager = new TabViewManager(_context);
                    List<TabView> tabViewrows = null;
                    bool ShowTabFlag = true;
                    if (Request.QueryString["isClosed"]!=null && Request.QueryString["isClosed"] == "true")
                        ShowTabFlag = false;

                    if (ShowTabFlag)
                    {
                        tabViewrows = tabViewManager.Load(x => x.ModuleNameLookup == ModuleName && x.ShowTab == true && x.ViewName==ViewName).OrderBy(x => x.TabOrder).ToList();
                        foreach (TabView item in tabViewrows)
                        {
                            Tab newtab = new Tab(Convert.ToString(item.TabDisplayName), Convert.ToString(item.TabName));
                            filterTab.Tabs.Add(newtab);
                        }
                    }
                    else
                    {
                        filterTab.Tabs.Clear();
                        tabViewrows = tabViewManager.Load(x => x.ModuleNameLookup == ModuleName && x.TabName == "allclosedtickets" && x.ViewName == ViewName).OrderBy(x => x.TabOrder).ToList();
                        foreach (var item in tabViewrows)
                        {
                            if (item.TabName == "allclosedtickets")
                            {
                                Tab _newtab = new Tab(Convert.ToString(item.TabDisplayName), Convert.ToString(item.TabName));
                                filterTab.Tabs.Add(_newtab);
                            }
                        }
                    }
                    if (tabViewrows.Count > 0)
                        CategoryName = tabViewrows[0].ColumnViewName;
                }
            }

            UGITModule module = ModuleViewManager.LoadByName(ModuleName, true);
            Page.Title = module != null ? module.ModuleName + "Tickets" : "";
            DataTable moduledt = UGITUtility.ObjectToData(module);
            if (moduledt.Rows.Count > 0)
                moduleRow = moduledt.Rows[0];
            // Manager.ModulesHelper.getUgitModule(this.ModuleName);//  uGITCache.GetDataTable(DatabaseObjects.Lists.Modules).AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == this.ModuleName || x.Field<int>(DatabaseObjects.Columns.Id) == moduleId);
            if (module != null)
            {
                ModuleName = module.ModuleName;
                createDuplicateUrl= TicketURlBatchEditing = UGITUtility.GetAbsoluteURL(module.StaticModulePagePath);
                moduleRowTitle = module.Title;
                enableModuleAgent = UGITUtility.StringToBoolean(module.EnableModuleAgent);
                bool moduleAgentButtonsEnable = true;//ConfigurationVariable.GetValueAsBool(ConfigConstants.ModuleAgentButtonsEnable);
                if (enableModuleAgent && !string.IsNullOrEmpty(ModuleName) && moduleAgentButtonsEnable && isAdmin)
                {
                    ServicesManager servicesManager = new ServicesManager(_context);
                    List<Services> dt = servicesManager.Load(x => x.ServiceType == "ModuleAgent" && x.ModuleNameLookup == ModuleName && x.IsActivated == true);  // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.Services, query).GetDataTable();

                    if (dt != null && dt.Count > 0)
                    {
                        foreach (Services dr in dt)
                        {
                            ASPxButton agentButton = new ASPxButton();
                            agentButton.ClientInstanceName = "agentButton";
                            agentButton.AutoPostBack = false;
                            agentButton.EnableClientSideAPI = true;
                            agentButton.CssClass = "btn-agents";
                            agentButton.Text = Convert.ToString(dr.Title);
                            agentButton.ID = "AgentTab~" + Convert.ToInt16(dr.ID) + "~" + Convert.ToString(dr.ModuleStage);
                            agentButton.ClientSideEvents.Click = "function(s,e){ setAgentLink('" + agentButton.Text + "', '" + Convert.ToInt16(dr.ID) + "'); }";
                            agentButton.ClientVisible = false;
                            pnlAgentBtns.Controls.Add(agentButton);

                        }
                    }
                }
                if (ModuleName=="CNS"|| ModuleName == "CPR")
                {
                    lnkbtnCustomMenuItem.ImageUrl = string.Empty;
                    ASPxPopupCustomMenuItem.PopupAction = PopupAction.None;
                }
            }

            grid.SettingsCookies.CookiesID = GetCookiePrefix() + "_Grid_Cookies";
            LoadModuleColumns();
            showClearFilter = IsFilterOn();  //!IsPreview && 

            //Clear the cookies of filter data when user clicks on Clear Filter OR when user jumps to another module.
            if ((Request.Form[hndClearFilter.UniqueID] != null && Request.Form[hndClearFilter.UniqueID].ToString().Contains("__ClearFilter__")))
            {
                ClearALLFilters();
                ClearFilterStateCookies();
            }

            //Show hide details
            ShowHideDetailsWithBasicSetting(moduleRow);
            if (!IsPostBack || hndClearFilter.Value.Contains("__ClearFilter__"))
                GetFilteredData();

            if (filterTab.Tabs != null)
            {
                filterTab.ActiveTab = filterTab.Tabs.FindByName(lblcustomfilterselectedvalue.Text);
            }

            //BindTemplateGrid();
            foreach (GridViewColumn column in grid.Columns)
            {
                if (column.GetType().Name == "GridViewCommandColumn") { continue; }

                if (FilterMode || (column as GridViewDataColumn).FieldName == "SelfAssign" || (column as GridViewDataColumn).FieldName == Constants.CRMSummary || (column as GridViewDataColumn).FieldName == Constants.CRMAllocationCount || (column as GridViewDataColumn).FieldName == DatabaseObjects.Columns.Attachments) // column.Name.StartsWith("projecthealth") ||
                {
                    (column as GridViewDataColumn).Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                }
                else
                {
                    (column as GridViewDataColumn).Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                    (column as GridViewDataColumn).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
                column.Caption = column.Caption.ToUpper();
            }

            int oldPageSize = 0;
            if (UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants._GridPageSize) != null && !IsPreview)
            {
                int.TryParse(UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants._GridPageSize), out oldPageSize);

                if (oldPageSize > 0)
                    PageSize = oldPageSize;
            }

            if (!string.IsNullOrEmpty(hdnPopupControl.Value))
                BindTemplateGridByAction(Convert.ToString(hdnPopupControl.Value));


            //string frmTrailUser = string.Empty;
            //var requestFromMail = Request.UrlReferrer.AbsoluteUri.Split('?');

            ////Var request = Request["ftu"]
            //if (requestFromMail.Count() > 1)
            //{
            //    frmTrailUser = Convert.ToString(requestFromMail[1].ToString());

            //}

            //if (!string.IsNullOrEmpty(frmTrailUser) && frmTrailUser.ToLower() == "ftu")
            //{

            //string edititem = "/ControlTemplates/RMM/userinfo.aspx?uID=0&newUser=1&ismail=1";
            //var serviceManager = new ServicesManager(_context);
            //TenantOnBoardingHelper tenantOnBoardingHelper = new TenantOnBoardingHelper(_context);
            //var listNewTenantServiceTitle = tenantOnBoardingHelper.GetNewTenantTemplateServiceTitles();
            //var services = serviceManager.LoadAllServices("services").OrderBy(x => x.CategoryId).OrderBy(x => x.ItemOrder).ToList();
            //long ServiceID  = serviceManager.LoadAllServices("services").Where(x => listNewTenantServiceTitle.Any(y => y.EqualsIgnoreCase(x.Title))).Select(x => x.ID).FirstOrDefault();


            //string script = String.Format("window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ServicesWizard&serviceID={0}&requestPage=selfregistration&type=self', '', 'Service:Registration of new tenant', 90, 90, 0, '%2flayouts%2fugovernit%2fdelegatecontrol.aspx')", ServiceID);
            //string script = string.Format("JavaScript:UgitOpenPopupDialog('{0}','','Add New User','625px','700',false)", edititem);

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", script, true);
            //Page.ClientScript.(this.GetType(), "", script);
            //Button_Click(btNewbutton, EventArgs.Empty);

            //}
            string strWidth = UGITUtility.GetCookieValue(Request, "screenWidth");
            if (!string.IsNullOrWhiteSpace(strWidth) && int.TryParse(strWidth, out int screenWidth))
            {
                if (screenWidth < 1400)
                {
                    CardView.SettingsPager.SettingsTableLayout.ColumnCount = 3;
                }
            }

            currentUGITModule = _moduleViewManager.LoadByName(this.ModuleName, true);
            if (currentUGITModule != null && currentUGITModule.EnableTicketImport && (UserManager.IsAdmin(HttpContext.Current.CurrentUser()) || UserManager.IsUserinGroups(_configurationVariableManager.GetValue(ConfigConstants.TicketAdminGroup), _context.CurrentUser.UserName)))
            {
                if (ModuleName.ToUpper() == "CNS" || ModuleName.ToUpper() == "CPR" || ModuleName.ToUpper() == "OPM")
                {
                    ModuleExcelImport excelImport = new ModuleExcelImport();
                    if (excelImport.ModuleImportRunning(this.ModuleName))
                    {
                        string importMsg = string.Format("Import Running: {0}%", excelImport.ModuleImportPercentageComplete(this.ModuleName));
                        ImportExcel.ToolTip = importMsg;
                        ImportExcel.Enabled = false;
                    }
                    else
                    {
                        ImportExcel.ToolTip = string.Format("Excel {0} Import", UGITUtility.moduleTypeName(this.ModuleName));
                    }
                    ImportExcel.Visible = true;
                }
            }
            if (!IsPostBack|| hndClearFilter.Value.Contains("__ClearFilter__"))
            {
                grid.DataBind();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            PreRenderData();

            base.OnPreRender(e);
        }

        private void PreRenderData()
        {
            grid.SettingsPager.PageSize = PageSize;
            if (initiateExport)
            {
                if (Request["wildCard"] != null && Request["wildCard"].Trim() != string.Empty)
                {
                    txtWildCard.Text = Request["wildCard"].Trim().Replace(wildCardWaterMark, string.Empty);
                }

                DateTime filterDate;
                if (Request["startDate"] != null && DateTime.TryParse(Request["startDate"].Trim(), out filterDate))
                {
                    dtFrom.Date = filterDate;
                }

                if (Request["endDate"] != null && DateTime.TryParse(Request["endDate"].Trim(), out filterDate))
                {
                    dtTo.Date = filterDate;
                }
            }
            else
            {
                //Disable state Managment if not required
                if (!DisableStateManagment)
                {
                    // If the cookies is not null then pickup the filter expression from Cookies or set else directly from DataSourceObject
                    if (IsPostBack || string.IsNullOrEmpty(this.ModuleName))
                    {
                        UGITUtility.CreateCookie(Response, Constants.SortedModule, this.ModuleName);

                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(dtFrom.Date)))
                            UGITUtility.CreateCookie(Response, GetCookiePrefix() + Constants.FromDateExpression, UGITUtility.ObjectToString(dtFrom.Date));
                        else
                            UGITUtility.DeleteCookie(Request, Response, GetCookiePrefix() + Constants.FromDateExpression);

                        if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(dtTo.Date)))
                            UGITUtility.CreateCookie(Response, GetCookiePrefix() + Constants.ToDateExpression, UGITUtility.ObjectToString(dtTo.Date));
                        else
                            UGITUtility.DeleteCookie(Request, Response, GetCookiePrefix() + Constants.ToDateExpression);

                        if (txtWildCard.Text.Trim().Replace(wildCardWaterMark, string.Empty) != string.Empty)
                            UGITUtility.CreateCookie(Response, GetCookiePrefix() + Constants.WildCardExpression, txtWildCard.Text.Trim());
                        else
                            UGITUtility.DeleteCookie(Request, Response, GetCookiePrefix() + Constants.WildCardExpression);
                    }
                }
            }

            UISettingAfterFilter();

            // Hide tabs with zero tickets (except a few)
            if (moduleStatResult != null)
            {
                foreach (KeyValuePair<string, int> keyvalue in moduleStatResult.TabCounts)
                {
                    if (keyvalue.Value == 0 && filterTab.Tabs.FindByName(keyvalue.Key) != null)
                    {
                        string tabName = filterTab.Tabs.FindByName(keyvalue.Key).Name;
                        if (tabName == FilterTab.waitingonme)
                            filterTab.Tabs.FindByName(keyvalue.Key).Visible = false;
                        else if (tabName == FilterTab.myopentickets)
                            filterTab.Tabs.FindByName(keyvalue.Key).Visible = false;
                        else if (tabName == FilterTab.unassigned)
                            filterTab.Tabs.FindByName(keyvalue.Key).Visible = false;
                        else if (tabName == FilterTab.mygrouptickets)
                            filterTab.Tabs.FindByName(keyvalue.Key).Visible = false;
                        else if (tabName == FilterTab.departmentticket)
                            filterTab.Tabs.FindByName(keyvalue.Key).Visible = false;
                        else if (tabName == FilterTab.allresolvedtickets)
                            filterTab.Tabs.FindByName(keyvalue.Key).Visible = false;
                        //Added by mudassir 17 march 2020 SPDelta 15(Support for separate "On Hold" tab in ticket lists)
                        else if (tabName == FilterTab.OnHold)
                            filterTab.Tabs.FindByName(keyvalue.Key).Visible = false;
                        //
                    }
                }
            }

            // If IsPreview is true 
            //if (IsPreview && resultedTable != null)

            if (IsPreview)
            {
                if (EnableNewButton)
                {
                    if (showNewTicketAtHome)
                        lnkNewTicket.Visible = true;

                    DataTable moduletable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                    DataRow[] dRowQuickTicket = moduletable.Select(string.Format("{0}=1 AND {1}=1", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableQuickTicket));

                    if (dRowQuickTicket != null && dRowQuickTicket.Length > 0)
                        lnkQuickTicket.Visible = true;
                }
                //filteredListUpperhead.Visible = false;
                if (resultedTable != null)
                {
                    if (resultedTable.Rows.Count >= grid.SettingsPager.PageSize)
                    {
                        cTicketPreviewPanel.Visible = true;
                        moreTicketLink.Visible = true;
                    }

                }
                moreTicketLink.Attributes.Add("href", "javascript:");

                // Set "More >>" popup title
                string title = string.Format("My Requests {0}", UserType.Equals("my", StringComparison.CurrentCultureIgnoreCase) ? string.Empty : "As " + UserType);
                if (MTicketStatus == TicketStatus.WaitingOnMe)
                    title = waitingOnMeTabName; //"Waiting On Me";
                else if (MTicketStatus == TicketStatus.Department)
                    title = "My Department Requests";
                else if (UserType == "mytask")
                    title = "My Tasks";

                if (!string.IsNullOrEmpty(ModuleName))
                {
                    title = "My Projects";
                    moreTicketLink.OnClientClick = string.Format("window.parent.UgitOpenPopupDialog('{0}', 'Module={5}&Status={4}&UserType={2}&showalldetail=false;showglobalfilter=true', '{1}', 90, 90, 0, '{3}')", viewTicketsPath, title, UserType, Server.UrlEncode(Request.Url.AbsolutePath), MTicketStatus.ToString(), ModuleName);
                }
                else
                {
                    moreTicketLink.OnClientClick = string.Format("window.parent.UgitOpenPopupDialog('{0}', 'Module=All&Status={4}&UserType={2}&showalldetail=false;showglobalfilter=true', '{1}', 90, 90, 0, '{3}')", viewTicketsPath, title, UserType, Server.UrlEncode(Request.Url.AbsolutePath), MTicketStatus.ToString());
                }

                if (!string.IsNullOrEmpty(MoreUrl))
                {
                    moreTicketLink.OnClientClick = MoreUrl;
                }
            }

            //Creates url with all local filter. this URL will be used to in export functionality
            StringBuilder filterDetailString = new StringBuilder();
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(Request.Url.PathAndQuery);
            if (string.IsNullOrEmpty(Request.Url.Query))
            {
                urlBuilder.Append("?");
            }

            if (txtWildCard.Text != string.Empty && !txtWildCard.Text.Equals(wildCardWaterMark, StringComparison.CurrentCultureIgnoreCase))
            {
                urlBuilder.AppendFormat("&{0}={1}", "wildCard", txtWildCard.Text);
                filterDetailString.AppendFormat(" <b>Search String:</b><i>{0}</i>", txtWildCard.Text);
            }

            if (dtFrom.Text != null)
            {
                urlBuilder.AppendFormat("&{0}={1}", "startDate", dtFrom.Date.ToString("MMM-dd-yyyy"));
                filterDetailString.AppendFormat(" <b>Start Date:</b><i>{0}</i>", dtFrom.Date.ToString("MMM-dd-yyyy"));
            }
            if (dtTo.Text != null)
            {
                urlBuilder.AppendFormat("&{0}={1}", "endDate", dtTo.Date.ToString("MMM-dd-yyyy"));
                filterDetailString.AppendFormat("; <b>End Date:</b><i>{0}</i>", dtTo.Date.ToString("MMM-dd-yyyy"));
            }

            exportPath = urlBuilder.ToString();
            exportURL.Value = urlBuilder.ToString();
            hdExportType.Value = exportType;

            if (initiateExport)
            {
                filterDetail = filterDetailString.ToString();
            }

            if (enableTicketListExport)
            {
                onlyExcelExport.Visible = false;
                exportAction.Visible = true;
            }
            else
            {
                if (IsManagerView)
                {
                    onlyExcelExport.Visible = false;
                    exportAction.Visible = true;
                }
            }
            //if (this.ModuleName == "CMDB" && UserProfile.CheckUserIsInGroup(ConfigurationVariable.GetValue(ConfigConstants.AssetAdmin), SPContext.Current.Web.CurrentUser))
            //    onlyExcelImport.Visible = true;

            //grid.DataBind();

            if (moduleColumns.Where(x => x.ShowInCardView == true).Count() > 0)
                CardView.DataBind();

            if (hndClearFilter.Value.Contains("__ClearFilter__"))
            {
                hndClearFilter.Value = string.Empty;
                if (IsPreview)
                {
                    ClearALLFilters();
                    ClearFilterStateCookies();
                }
            }

            if (!IsDashboard && !IsPreview)
            {
                if (grid.Columns.Count > 0)
                {
                    grid.Columns[0].Visible = true;
                    grid.Columns[0].VisibleIndex = 0;
                }
            }
            else
            {
                if (grid.Columns.Count > 0)
                {
                    grid.Columns[0].Visible = true;
                    if (grid.Columns[0] is GridViewCommandColumn)
                    {
                        grid.Columns[0].Visible = false;
                        grid.Columns[0].VisibleIndex = -1;
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string value = string.Empty;
            if (initiateExport)
            {
                if (enableTicketListExport && (exportType == "pdf" || exportType == "image"))
                {

                    string headerTitle = string.Empty;
                    if (!string.IsNullOrEmpty(ModuleName))
                    {
                        headerTitle = string.Format("<b>Filter:</b><i>{0} {3} {1} {2}</i>", UserType, MTicketStatus, UGITUtility.moduleTypeName(ModuleName), ModuleName);
                    }

                    StringBuilder sb = new StringBuilder();
                    HtmlTextWriter tw = new HtmlTextWriter(new System.IO.StringWriter(sb));
                    //Render the page to the new HtmlTextWriter which actually writes to the stringbuilder
                    base.Render(tw);
                    //Get the rendered content
                    string sContent = sb.ToString();

                    //Now output it to the page, if you want
                    writer.Write(sContent);
                    string html = sb.ToString();

                    ExportReport convert = new ExportReport();
                    convert.HeaderHeading = globalFilterDetialForPDF;
                    convert.HeaderSubHeading = filterDetail;
                    convert.ScriptsEnabled = true;
                    convert.ShowFooter = true;
                    convert.ShowHeader = true;
                    int reportType = 0;
                    string reportTypeString = "pdf";
                    string contentType = "Application/pdf";
                    if (exportType == "image")
                    {
                        reportType = 1;
                        reportTypeString = "png";
                        contentType = "image/png";
                    }
                    convert.ReportType = reportType;
                    html = string.Format(@"<html><head></head><body>{0}</body></html>", html);
                    byte[] bytes = convert.GetReportFromHTML(html, "");

                    string fileName = string.Format("export.{0}", reportTypeString);
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentType = contentType;
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
                else if (exportType == "excel")
                {
                    DataTable filteredDataTable = new DataTable();
                    Dictionary<string, string> colDatatype = new Dictionary<string, string>(); 
                    string FilterExpression = grid.FilterExpression;

                    if (resultedTable != null && resultedTable.Rows.Count > 0 && grid != null && !string.IsNullOrEmpty(grid.FilterExpression))
                    {
                        try
                        {

                            filteredDataTable = resultedTable.Clone();
                            int startVisibleIndex = grid.VisibleStartIndex;
                            for (int i = startVisibleIndex; i < grid.VisibleRowCount; i++)
                            {
                                DataRow dataRow = filteredDataTable.NewRow();
                                dataRow = (DataRow)grid.GetDataRow(i);
                                if (dataRow != null)
                                    filteredDataTable.Rows.Add(dataRow.ItemArray);
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error exporting filtered ticket list");
                            filteredDataTable = resultedTable.Clone();
                        }
                    }
                    else if (resultedTable != null && grid != null && grid.FilterExpression == string.Empty && grid.VisibleRowCount == 0)
                        filteredDataTable = resultedTable.Clone();
                    else if (resultedTable != null)
                        filteredDataTable = resultedTable;

                    List<string> selectedColumns = new List<string>();
                    moduleColumns = moduleColumns.OrderBy(x => x.FieldSequence).ToList();
                    if (filteredDataTable != null)
                    {
                        foreach (ModuleColumn column in moduleColumns)
                        {
                            if (!selectedColumns.Contains(column.FieldName) && filteredDataTable.Columns.Contains(column.FieldName))
                            {
                                selectedColumns.Add(column.FieldName);
                                //BTS-23-001177: Query Export CSV or Excel not recognized as Date format.
                                if(!string.IsNullOrEmpty(column.FieldDisplayName) && !colDatatype.ContainsKey(column.FieldDisplayName))
                                    colDatatype.Add(column.FieldDisplayName, column.ColumnType);
                            }
                        }

                        DataView view = filteredDataTable.DefaultView; //(DataView)ticketDataSource.Select();
                        if (view != null)
                        {
                            DataTable dtResultedTable = null;
                            if (selectedColumns.Count > 2)
                            {
                                dtResultedTable = ((DataView)view).ToTable(true, selectedColumns.ToArray());
                            }
                            else
                            {
                                selectedColumns.Clear();
                                foreach (DataColumn column in filteredDataTable.Columns)
                                {
                                    if (column.ColumnName.ToLower() != "id" && column.ColumnName.ToLower() != "created" && column.ColumnName.ToLower() != "me" && column.ColumnName.ToLower() != "today" && column.ColumnName.ToLower() != "modified" && column.ColumnName.ToLower() != "today" && column.ColumnName.ToLower() != "me" &&
                                        column.ColumnName != DatabaseObjects.Columns.ModuleNameLookup)
                                    {
                                        selectedColumns.Add(column.ColumnName);
                                    }
                                }
                                dtResultedTable = ((DataView)view).ToTable(true, selectedColumns.ToArray());

                            }

                            if (dtResultedTable.Columns.Contains(DatabaseObjects.Columns.Attachments))
                                dtResultedTable.Columns.Remove(DatabaseObjects.Columns.Attachments);

                            if (dtResultedTable.Columns.Contains(DatabaseObjects.Columns.ID))
                                dtResultedTable.Columns.Remove(DatabaseObjects.Columns.ID);

                            DataTable newTable = dtResultedTable.Clone();
                            foreach (DataColumn dc in newTable.Columns)
                            {
                                if (dc.DataType == typeof(int) || dc.DataType == typeof(System.Int64))
                                {
                                    dc.DataType = typeof(string);
                                }
                            }
                            if (resultedTable == null)
                            {
                                return;
                            }

                            foreach (DataRow row in resultedTable.Rows)
                            {
                                newTable.ImportRow(row);
                            }
                            dtResultedTable.Clear();


                            foreach (DataColumn column in newTable.Columns)
                            {
                                ModuleColumn row = moduleColumns.FirstOrDefault(x => x.FieldName == column.ColumnName);
                                if (row != null && !string.IsNullOrEmpty(row.FieldDisplayName) && !newTable.Columns.Contains(row.FieldDisplayName))
                                {
                                    column.ColumnName = row.FieldDisplayName;
                                    column.Caption = row.FieldDisplayName;

                                    if (grid.FilterExpression.Contains($"[{row.FieldName}]"))
                                        FilterExpression = FilterExpression.Replace($"[{row.FieldName}]", $"[{row.FieldDisplayName}]");
                                }
                            }
                            for (int j = 0; j < newTable.Columns.Count; j++)
                            {
                                if (newTable.Columns[j].DataType == typeof(DateTime))
                                {
                                    for (int i = 0; i < newTable.Rows.Count; i++)
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(newTable.Rows[i][j])))
                                        {
                                            newTable.Rows[i][j] = Convert.ToDateTime(newTable.Rows[i][j]).Date;
                                        }
                                    }
                                }
                            }
                            //for (int i = 0; i < newTable.Rows.Count; i++)
                            //{
                            //    for (int j = 0; j < newTable.Columns.Count; j++)
                            //    {
                            //        try
                            //        {
                            //            if (!string.IsNullOrEmpty(Convert.ToString(newTable.Rows[i][j])))
                            //            {
                            //                moduleColumn = moduleColumns.FirstOrDefault(x => x.FieldDisplayName == newTable.Columns[j].ColumnName);
                            //                //if (moduleColumn.FieldName.EndsWith("$"))
                            //                //    moduleColumn.FieldName = moduleColumn.FieldName.Remove(moduleColumn.FieldName.Length - 1, 1);
                            //                // moduleColumns. (x => x.FieldDisplayName == newTable.Columns[j].ColumnName)
                            //                if (moduleColumn != null)
                            //                    field = FieldConfigurationManager.GetFieldByFieldName(moduleColumn.FieldName);
                            //                else
                            //                    field = FieldConfigurationManager.GetFieldByFieldName(newTable.Columns[j].ColumnName);

                            //                if (field != null && (field.Datatype == "Lookup" || field.Datatype == "UserField"))
                            //                {
                            //                    value = FieldConfigurationManager.GetFieldConfigurationData(field, Convert.ToString(newTable.Rows[i][j]), null, Constants.Separator6);
                            //                    if (!string.IsNullOrWhiteSpace(value))
                            //                    {
                            //                        newTable.Rows[i][j] = value.Replace(',', ';');
                            //                    }
                            //                    else
                            //                    {
                            //                        newTable.Rows[i][j] = String.Empty;
                            //                    }
                            //                }
                            //                else if (moduleColumn != null && !string.IsNullOrEmpty(moduleColumn.ColumnType) && (moduleColumn.ColumnType == "Lookup" || moduleColumn.ColumnType == "UserField"))
                            //                {
                            //                    value = FieldConfigurationManager.GetFieldConfigurationData(field, Convert.ToString(newTable.Rows[i][j]), null, Constants.Separator6, moduleColumn: moduleColumn);
                            //                    if (!string.IsNullOrWhiteSpace(value))
                            //                    {
                            //                        newTable.Rows[i][j] = value.Replace(',', ';');
                            //                    }
                            //                    else
                            //                    {
                            //                        newTable.Rows[i][j] = String.Empty;
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        catch (Exception e)
                            //        {

                            //            throw;
                            //        }

                            //    }
                            //}

                            if (!string.IsNullOrEmpty(FilterExpression))
                            {
                                DataRow[] rows = newTable.Select(FilterExpression);
                                if (rows != null && rows.Count() > 0)
                                    newTable = rows.CopyToDataTable();
                            }

                            string csvData = UGITUtility.ConvertTableToCSV(newTable, colDatatype);
                            string attachment = string.Format("attachment; filename={0}.csv", "Export");
                            Response.Clear();
                            Response.ClearHeaders();
                            Response.ClearContent();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", attachment);
                            Response.ContentType = "text/csv";
                            Response.Write(csvData.ToString());
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            base.Render(writer);
        }
        //SpDelta 40(Implementation of Show in tabs functionality to Custom filtered tickets.)
        public DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        //static DataTable ConvertListToDataTable1(List<ModuleColumn> list)
        //{
        //    // New table.
        //    DataTable table = new DataTable();

        //    // Get max columns.
        //    int columns = 0;
        //    foreach (var array in list)
        //    {
        //        //if (array. > columns)
        //        //{
        //            columns = array;
        //        //}
        //    }


        //    // Add columns.
        //    for (int i = 0; i < columns; i++)
        //    {
        //        table.Columns.Add();
        //    }

        //    // Add rows.
        //    foreach (var array in list)
        //    {
        //        table.Rows.Add(array);
        //    }




        //    return table;
        //}
        //
        #region ITHELPDESKCTREL
        protected void TSR_Click(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "TSRClicked", "True");
            Response.Redirect("~/pages/HomeHelpDeskTSR");
        }
        protected void SVC_Click(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "SVCClicked", "True");
            UGITUtility.CreateCookie(Response, "TSRClicked", "False");
            Response.Redirect("~/pages/HomeHelpDeskSVC");
        }
        protected void DRQ_Click(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "DRQClicked", "True");
            UGITUtility.CreateCookie(Response, "TSRClicked", "False");
            Response.Redirect("~/pages/HomeHelpDeskDRQ");
        }
        protected void ACR_Click(object sender, EventArgs e)
        {
            UGITUtility.CreateCookie(Response, "ACRClicked", "True");
            UGITUtility.CreateCookie(Response, "TSRClicked", "False");
            Response.Redirect("~/pages/HomeHelpDeskACR");
        }
        #endregion

        /// <summary>
        /// Show related tickets of selected ticket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtSubTicketButton_Click(object sender, EventArgs e)
        {
            //string ticketId = hfTicketInfo.Value;
            //if (ticketId != null)
            //{
            //    CustomTicketRelationShip relation = (CustomTicketRelationShip)Page.LoadControl("/CONTROLTEMPLATES/uGovernIT/CustomTicketRelationship.ascx");
            //    relation.LoadData = true;
            //    relation.ParentLevel = 0;
            //    relation.ChildrenLevel = -1;
            //    relation.ShowParent = false;
            //    relation.ShowDelete = false;
            //    relation.AddChild = false;
            //    relation.TicketId = ticketId.Trim();
            //    relation.ParentDetailOnDemand = false;
            //    relation.ChildDetailOnDemand = false;
            //    relation.HideExpendCollapse = true;
            //    relation.HideRootNode = true;
            //    subTicketPanel.Controls.Add(relation);
            //}
        }

        protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item;
            DataTable moduleColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
            //   DataTable moduleColumns = uGITCache.ModuleConfigCache.LoadModuleListByName("",DatabaseObjects.Tables.ModuleColumns); //uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
            lstFilteredFields.Items.Clear();
            if (Convert.ToInt32(ddlModuleName.SelectedValue) != 0)
            {
                string selectQuery = string.Format("{0} = '{1}' and {2}= {3}", DatabaseObjects.Columns.CategoryName, ddlModuleName.SelectedItem.Text, DatabaseObjects.Columns.IsUseInWildCard, "True");
                DataRow[] moduleColumnRows = moduleColumns.Select(selectQuery);

                foreach (DataRow dataRow in moduleColumnRows)
                {
                    item = new ListItem(Convert.ToString(dataRow[DatabaseObjects.Columns.FieldDisplayName]), Convert.ToString(dataRow[DatabaseObjects.Columns.FieldName]));
                    lstFilteredFields.Items.Add(item);
                }
            }
            item = new ListItem("--All--");
            lstFilteredFields.Items.Insert(0, item);
        }

        #endregion

        #region ASPxGridView Events

        protected void grid_DataBinding(object sender, EventArgs e)
        {

           DevExpress.Web.ASPxGridView grid = sender as DevExpress.Web.ASPxGridView;
            grid.JSProperties["cpPageSize"] = grid.SettingsPager.PageSize;

            if (resultedTable == null || resultedTable.Rows.Count == 0)
            {
                GetFilteredData();
            }

            if (resultedTable != null && resultedTable.Rows.Count > 0)
            {
                // Show/Hide grid columns for current selected tab
                if (filterTab.ActiveTab != null)
                {
                    activeTab = filterTab.ActiveTab.Name;
                    string selectedTab = UGITUtility.GetCookieValue(Request, GetCookiePrefix() + "_SelectedTabName");
                    if (!string.IsNullOrEmpty(selectedTab))
                    {
                        activeTab = selectedTab;
                        previousTab = activeTab;
                    }
                }

                // Added condition to fix Column chooser issue (BTS-21-000312)
                if (columnMoved.Contains("NonModuleColumns"))
                {
                    if (Convert.ToString(columnMoved["NonModuleColumns"]) == "false")
                        ShowHideColumnsForSelectedTab();
                }
                else
                    ShowHideColumnsForSelectedTab();

                //
                if (!string.IsNullOrEmpty(Request["ExperienceTagLookup"]) && !string.IsNullOrEmpty(Request["ExperienceTagUser"]))
                {
                    UserProjectExperienceManager userExperiencedTagMGR = new UserProjectExperienceManager(_context);
                    List<string> userProjectExperiences = userExperiencedTagMGR.Load(x => x.UserId == UGITUtility.ObjectToString(Request["ExperienceTagUser"])
                    && x.TagLookup == UGITUtility.StringToInt(Request["ExperienceTagLookup"])).Select(x => x.ProjectID).ToList();

                    resultedTable = resultedTable.AsEnumerable().Where(o => userProjectExperiences.Contains(o.Field<string>(DatabaseObjects.Columns.TicketId)))?.CopyToDataTable() ?? null;
                }
                
                resultedTable.AcceptChanges();
                grid.DataSource = resultedTable;
                CardView.DataSource = resultedTable;
            }
            else
            {
                grid.DataSource = null;
            }
            if (!string.IsNullOrEmpty(ModuleName) && ModuleName.ToLower() == "cmdb")
            {
                grid.SettingsText.EmptyDataRow = "No Assets Assigned";
            }
            //if (tabChangedcommand.Contains("istabChanged"))
            //{
            //    tabChangedcommand.Clear();
            //}
            //}
            //if (openTicketDialogCommand.Contains("openTicketDialogCommand"))
            //{
            //    openTicketDialogCommand.Clear();
            //}
        }


        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string viewUrl = string.Empty;
            string title = string.Empty;
            string func = string.Empty;
            string ticketId = string.Empty;
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            //In case of ResourceTab/AspNetUsers Dashboards Gridview should not be clickable like in Demo Sharepoint site.
            if (ModuleName == Utility.Constants.ResourceTab && !string.IsNullOrEmpty(Request["dID"]))
                return;
            //because the code will not find TicketId column for every chart when CategoryName is MyHomeTab
            if (ModuleName == "" && CategoryName == Constants.MyHomeTicketTab && !string.IsNullOrEmpty(Request["dashboardID"]))
                return;

            string moduleName = ModuleName;
            DataRow currentRow = grid.GetDataRow(e.VisibleIndex);
            if (currentRow == null)
                return; // No rows in table, nothing to do!


            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
            {
                moduleName = Convert.ToString(currentRow[DatabaseObjects.Columns.ModuleNameLookup]);
            }

            if (string.IsNullOrEmpty(moduleName))
            {
                ticketId = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                if (ticketId.Length > 0)
                    moduleName = ticketId.Substring(0, ticketId.IndexOf("-"));

            }
            //start

            if (currentRow != null)
            {
                DataRow moduleDetail = moduleRow;
                if (moduleDetail == null)
                {
                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(moduleName, true));
                        if (moduledt.Rows.Count > 0)
                            moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(moduleName);
                    }
                }


                //Get Row ID
                //string ticketId = string.Empty;
                ticketId = string.Empty;

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketId) && Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]) != string.Empty)
                {
                    ticketId = currentRow[DatabaseObjects.Columns.TicketId].ToString().Trim();
                }
                else if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id))
                {
                    ticketId = currentRow[DatabaseObjects.Columns.Id].ToString();
                }
                typeName = UGITUtility.moduleTypeName(moduleName);
                if (moduleDetail != null)
                {
                    viewUrl = string.Empty;
                    if (moduleDetail[DatabaseObjects.Columns.ModuleRelativePagePath] != null)
                        viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                    if (moduleName == ModuleNames.PMM)
                        viewUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?ctrl=PMM.ProjectCompactView"));
                    if (ticketId != string.Empty)
                    {
                        if (typeName == "Asset")
                        {
                            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.AssetName))
                            {
                                ticketTitle = UGITUtility.TruncateWithEllipsis(Convert.ToString(currentRow[DatabaseObjects.Columns.AssetName]), 100, string.Empty);
                            }
                            //title = string.Format("{0}: {1}", Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]), ticketTitle);
                        }
                        else
                        {
                            ticketTitle = UGITUtility.TruncateWithEllipsis(currentRow[DatabaseObjects.Columns.Title].ToString(), 100, string.Empty);
                        }

                        prefixcolumn = uHelper.getAltTicketIdField(_context, moduleName);
                        if (UGITUtility.IfColumnExists(currentRow, prefixcolumn))
                            prefixTitle = UGITUtility.ObjectToString(currentRow[prefixcolumn]);
                        if (!string.IsNullOrWhiteSpace(ticketTitle))
                            title = string.Format("{0}: {1}", string.IsNullOrEmpty(prefixTitle) ? ticketId : prefixTitle, ticketTitle);
                    }
                }
                title = UGITUtility.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
                title = title.Replace("\r\n", ""); // Embedded newlines in title prevent popups from opening

                // Prasad: Edit the width & height of ticket popups here!

                if (!string.IsNullOrEmpty(viewUrl))
                {
                    string width = string.IsNullOrEmpty(this.PopupWidth) ? "90" : this.PopupWidth;
                    string height = string.IsNullOrEmpty(this.PopupHeight) ? "90" : this.PopupHeight;
                    func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketId), title, sourceURL, width, height);
                }

                //e.Row.Attributes.Add("onClick", func);
                e.Row.Attributes.Add("onmouseover", string.Format("item_mousehover(this, '{0}')", ticketId));
                e.Row.Attributes.Add("onmouseout", string.Format("item_mouseout(this, '{0}')", ticketId));

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && currentRow[DatabaseObjects.Columns.Id] != null)
                    e.Row.Attributes.Add("ticketId", Convert.ToString(currentRow[DatabaseObjects.Columns.Id]));
                e.Row.Style.Add("cursor", "pointer");

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                {
                    if (moduleRequest != null && moduleRequest.Module != null && moduleRequest.Module.List_Priorities.Exists(x => x.IsVIP && x.Name == Convert.ToString(currentRow[DatabaseObjects.Columns.TicketPriorityLookup])))
                    {
                        e.Row.CssClass += " vipticket";
                    }
                }
            }
            e.Row.CssClass += ShowCompactView ? " customrowheight-c" : " customrowheight";

            //end
            Ticket objTicket = new Ticket(_context, moduleName);

            bool enableTicketReopenByRequestor = true; //objConfigurationVariableHelper.GetConfigVariableValueAsBool(ConfigConstants.EnableTicketReopenByRequestor);
            GridViewTableDataCell editCell = null;
            GridViewDataColumn cellColumn = null;
            int cellIndex = 0;
            int truncateTextTo = 0;
            bool allowExe = false;
            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    string width = string.IsNullOrEmpty(this.PopupWidth) ? "95" : this.PopupWidth;
                    string height = string.IsNullOrEmpty(this.PopupHeight) ? "90" : this.PopupHeight;
                    editCell = (GridViewTableDataCell)cell;
                    cellColumn = editCell.Column as GridViewDataColumn;
                    cellIndex = cellColumn.VisibleIndex;
                    string fieldName = cellColumn.FieldName;
                    string path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=ugitmodulewebpart&isreadonly=true&Module=" + moduleName;
                    if (fieldName != Constants.CRMSummary)
                    {
                        func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}'); return false;", path, string.Format("TicketId={0}", ticketId), title, sourceURL, width, height);
                        //func = string.Format("event.stopPropagation(); openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}'); return false;", viewUrl, string.Format("TicketId={0}", ticketId), title, sourceURL, width, height);
                        editCell.Attributes.Add("onClick", func);
                    }
                    if (IsDashboard || IsPreview)
                        cellIndex = cellColumn.VisibleIndex - 1;
                    if (cellIndex == 1)
                        editCell.CssClass = "edit-ticket-cell";
                    // adjust cell alignment with header alignment for left aligned column
                    if (cellColumn.CellStyle.HorizontalAlign == HorizontalAlign.Left || cellColumn.CellStyle.HorizontalAlign == HorizontalAlign.Right)
                    {
                        editCell.Style.Add(HtmlTextWriterStyle.PaddingLeft, "7px");
                    }
                    
                    moduleColumn = moduleColumns.FirstOrDefault(x => x.FieldName == fieldName);
                    string columnType = string.Empty;
                    truncateTextTo = 0;

                    if (moduleColumn != null && !string.IsNullOrWhiteSpace(moduleColumn.ColumnType))
                        columnType = moduleColumn.ColumnType.ToLower();

                    if (columnType == "string" && UGITUtility.StringToInt(moduleColumn.TruncateTextTo) > 0)
                        truncateTextTo = UGITUtility.StringToInt(moduleColumn.TruncateTextTo);

                    field = FieldConfigurationManager.GetFieldByFieldName(fieldName);

                    string fieldColumnType = string.Empty;
                    if (field != null)
                    {
                        fieldColumnType = Convert.ToString(field.Datatype);
                    }
                    else if (!string.IsNullOrEmpty(columnType))
                        fieldColumnType = columnType;

                    if (fieldName == DatabaseObjects.Columns.TicketPriorityLookup)
                    {
                        #region SP Code
                        string priority = Convert.ToString(e.GetValue(fieldName));
                        if (!string.IsNullOrEmpty(priority) && !string.IsNullOrEmpty(moduleName))
                        {
                            UGITModule uModule = currentUGITModule != null ? currentUGITModule : _moduleViewManager.LoadByName(moduleName, true);
                            ModulePrioirty currentpriority = null;
                            if (uModule != null && uModule.List_Priorities.Count > 0)
                                currentpriority = uModule.List_Priorities.FirstOrDefault(x => x.ModuleNameLookup == moduleName && (x.Title == priority || x.ID == UGITUtility.StringToLong(priority)));
                            string priorityTitle = currentpriority == null ? priority : currentpriority.Title;
                            string priorityText = "<div style='white-space:nowrap; border-radius:4px; padding:6px 2px; text-align:center; '>" + priorityTitle + "</div>";
                            if (currentpriority != null)
                            {
                                Color colorName = ColorTranslator.FromHtml(currentpriority.Color);
                                Color foreColor = new Color();
                                if (!string.IsNullOrWhiteSpace(currentpriority.Color))
                                {
                                    foreColor = UGITUtility.IdealTextColor(colorName);
                                }

                                priorityText = "<div style='white-space:nowrap;border-radius:4px; padding:6px 2px; text-align:center;color:" + foreColor.Name + ";background-color:" + currentpriority.Color + "'>" + currentpriority.Title + "</div>";
                                priority = currentpriority.Title;
                            }

                            editCell.Text = priorityText;
                        }
                        if (!string.IsNullOrEmpty(priority) && priority.Contains("High") || priority.Contains("Critical"))
                        {
                            //editCell.Style.Add(HtmlTextWriterStyle.Color, "#e24a7a");
                            //editCell.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                        #endregion
                    }
                    if (fieldName == DatabaseObjects.Columns.ShortName && UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.ShortName))
                    {
                        editCell.Text = string.Format("<span class='quickedit jqtooltip' title='{0}'> {1}</span>", UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.Title]), e.GetValue(DatabaseObjects.Columns.ShortName));
                    }
                    else if (fieldName == Constants.CRMAllocationCount)
                    {
                        if (UGITUtility.IfColumnExists(currentRow, "ResourceAllocationCount"))
                        {
                            path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + ticketId + "&module=" + ModuleName;
                            func = string.Format("javascript:UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path, string.Format("moduleName={0}&ConfirmBeforeClose=true", ModuleName), title, sourceURL);
                            editCell.Attributes.Add("onClick", func);
                            if (currentRow["ResourceAllocationCount"] != DBNull.Value)
                            {
                                string[] values = UGITUtility.SplitString(e.GetValue("ResourceAllocationCount"), Constants.Separator);
                                int totalAllocation = int.Parse(values[0]);
                                int unAllocatedResource = int.Parse(values[1]);
                                int filledAllocation = totalAllocation - unAllocatedResource;
                                if (totalAllocation == 0 && !uHelper.HideAllocationTemplate(_context) && (ModuleName == "CPR" || ModuleName == "OPM" || ModuleName == "CNS"))
                                {
                                    string popupTitle = "Add Resource Allocations to " + (ModuleName == "CPR" ? "Project" : ModuleName == "OPM" ? "Opportunity" : "Service");
                                    path = NewOPMWizardPageUrl + "&ticketId=" + ticketId + "&module=" + ModuleName + "&selectionmode=NewAllocatonsFromProjects&title=" + title;
                                    func = string.Format("javascript:UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path, "", popupTitle, sourceURL);
                                    editCell.Attributes.Add("onClick", func);
                                }
                                if (filledAllocation > 0)
                                {
                                    editCell.Text = string.Format("<span class='resourceAllocationRed{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, ShowCompactView ? "-c" : "");
                                    if (filledAllocation == totalAllocation)
                                        editCell.Text = string.Format("<span class='resourceAllocationBlue{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, ShowCompactView ? "-c" : "");
                                }
                                else
                                    editCell.Text = string.Format("<span class='resourceAllocationRed{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, ShowCompactView ? "-c" : "");

                            }
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketId)
                    {
                        //Is child ticket exist then show plus icon
                        string ticketPublicID = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                        bool relationExist = false;
                        if (moduleName == "CMDB")
                            relationExist = _ticketRelationshipHelper.IsRelationExist(ticketPublicID, relationalTickets);
                        else
                            relationExist = _ticketRelationshipHelper.IsChildExist(ticketPublicID, relationalTickets);

                        editCell.Text = string.Format("<span style='padding-left:7px;'>{0}</span> ", ticketPublicID);
                        if (moduleName != "SVC" && relationExist)
                            editCell.Text = string.Format("<img class='subticketaction' src='/Content/images/plus.png' onclick='event.cancelBubble= true; LoadSubTicket(this,\"{0}\");' alt='+' /><span style='display:inline-block;'>{0}</span> ", ticketPublicID);
                    }
                    else if (fieldName == DatabaseObjects.Columns.ERPJobID)
                    {
                        string eRPJobID = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ERPJobID));
                        if (!string.IsNullOrWhiteSpace(eRPJobID) && !ShowCompactView)
                            editCell.Text = string.Format("<div class='erpJobId-cmic'>{0}</div>", eRPJobID);
                    }
                    else if (fieldName == DatabaseObjects.Columns.ERPJobIDNC)
                    {
                        string eRPJobIDNC = Convert.ToString(e.GetValue(DatabaseObjects.Columns.ERPJobIDNC));
                        if (!string.IsNullOrWhiteSpace(eRPJobIDNC) && !ShowCompactView)
                            editCell.Text = string.Format("<div class='erpJobId-cmic'>{0}</div>", eRPJobIDNC);
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketStatus || fieldName == DatabaseObjects.Columns.ModuleStepLookup || fieldName == DatabaseObjects.Columns.ModuleStepLookup + "$")
                    {
                        // Check if we need to show field as a progress bar
                        if (columnType == "progressbar" && !string.IsNullOrEmpty(moduleName))
                        {
                            LifeCycleStage currentStage = null;
                            LifeCycle defaultLifeCycle = objTicket.GetTicketLifeCycle(currentRow);
                            if (defaultLifeCycle != null)
                            {
                                double totalWeight = defaultLifeCycle.Stages.Sum(x => x.StageWeight);
                                currentStage = objTicket.GetTicketCurrentStage(currentRow);
                                if (currentStage != null)
                                {
                                    double tillStageWeight = defaultLifeCycle.Stages.Where(x => x.StageStep < currentStage.StageStep).Sum(x => x.StageWeight);
                                    int pctComplete = (int)(tillStageWeight / totalWeight * 100);
                                    editCell.Text = UGITUtility.GetProgressBar(defaultLifeCycle, currentStage, currentRow, fieldName, false, false);
                                }
                            }
                        }
                    }
                    else if (columnType == "indicatorlight")
                    {
                        double score = 0;
                        Double.TryParse(Convert.ToString(currentRow[fieldName]), out score);
                        editCell.Text = string.Format("<img src='{0}' alt='health'/>", uHelper.GetHealthIndicatorImageUrl(_context, score));
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketAge)
                    {
                        bool ageColorByTargetCompletion = _configurationVariableManager.GetValueAsBool(ConfigConstants.AgeColorByTargetCompletion);
                        int ticketAge = UGITUtility.StringToInt(e.GetValue(DatabaseObjects.Columns.TicketAge));
                        editCell.Text = UGITUtility.GetAgeBar(currentRow, ageColorByTargetCompletion, ticketAge);
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketPctComplete || columnType == "percentage")
                    {
                        double pctComplete;
                        if (double.TryParse(Convert.ToString(e.GetValue(fieldName)), out pctComplete))
                        {
                            // Display 0-1 value as % with 1 or two decimals (if needed)
                            //if (fieldName == DatabaseObjects.Columns.TicketPctComplete)
                            //    pctComplete *= 100;

                            if (pctComplete > 99.9 && pctComplete < 100)
                                editCell.Text = "99.9%"; // Don't show 100% unless all the way done!
                            else
                                editCell.Text = string.Format("{0:0.#}%", pctComplete);
                        }
                    }
                    else if (columnType == "currency")
                    {
                        double amount;
                        if (double.TryParse(Convert.ToString(e.GetValue(fieldName)), out amount))
                            editCell.Text = string.Format("{0:C}", amount);
                    }
                    else if (columnType == "number")
                    {
                        int number;
                        if (int.TryParse(Convert.ToString(e.GetValue(fieldName)), out number))
                            editCell.Text = string.Format("{0:n0}", number);
                    }
                    else if (columnType.ToLower() == "sladate" || columnType == "sladatetime")
                    {
                        string cssClass = string.Empty;
                        string cellText = string.Empty;
                        DateTime slaDateTime = DateTime.MinValue;
                        if (DateTime.TryParse(Convert.ToString(e.GetValue(fieldName)), out slaDateTime))
                        {
                            string divFormat = @"<div class='{0}'>{1}</div>";
                            cellText = string.Format(divFormat, cssClass, string.Empty);
                            if (slaDateTime.Date < DateTime.Now.Date)
                                cssClass = "R";
                            else if (slaDateTime.Date == DateTime.Now.Date)
                                cssClass = "Y";
                            else
                                cssClass = "G";
                            cellText = string.Format(divFormat, cssClass, UGITUtility.GetDateStringInFormat(slaDateTime, columnType == "sladatetime")); // Show with or without time depending on type
                        }
                        editCell.Text = cellText;
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketDueIn)
                    {
                        if (Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketDesiredCompletionDate)) != string.Empty)
                            editCell.Text = UGITUtility.GetDueIn((DateTime)(e.GetValue(DatabaseObjects.Columns.TicketDesiredCompletionDate)));
                    }
                    else if (fieldName == DatabaseObjects.Columns.SelfAssign)
                    {
                        LifeCycleStage currentStage = null;
                        if (!string.IsNullOrEmpty(moduleName))
                        {
                            currentStage = objTicket.GetTicketCurrentStage();
                            if (currentStage != null && currentStage.Prop_SelfAssign.HasValue && currentStage.Prop_SelfAssign.Value)
                            {
                                string groupName = string.Empty;
                                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.PRPGroup))
                                {
                                    groupName = Convert.ToString(currentRow[DatabaseObjects.Columns.PRPGroup]);
                                }
                                else
                                {
                                    if (Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketPRP)) == string.Empty
                                        && Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketRequestTypeLookup)) != string.Empty)
                                    {
                                        // groupName = uHelper.GetPRPGroupFromRequestTypeValue(moduleName, Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketRequestTypeLookup)), Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketRequestTypeCategory)));
                                    }
                                }
                            }
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketComment || fieldName == DatabaseObjects.Columns.TicketResolutionComments || fieldName == DatabaseObjects.Columns.ProjectSummaryNote || fieldName == DatabaseObjects.Columns.History)
                    {
                        string rawData = Convert.ToString(e.GetValue(fieldName));
                        // Gets ALL entries
                        // gvRow.Cells[i].Text = uHelper.GetFormattedHistoryString(rawData, true);
                        // Just get the latest entry
                        string[] versionsDelim = { Constants.SeparatorForVersions };
                        string[] versions = rawData.Split(versionsDelim, StringSplitOptions.RemoveEmptyEntries);
                        if (versions != null && versions.Length != 0)
                        {
                            string latestEntry = versions[versions.Length - 1];
                            // Assume <version1>$;#$<version2>$;#$<version3>
                            string[] versionDelim = { Constants.Separator };
                            string[] versionData = latestEntry.Split(versionDelim, StringSplitOptions.None);
                            editCell.Text = latestEntry;
                            if (versionData.GetLength(0) == 3)
                            {
                                // Assume <userID>;#<timestamp>;#<text>
                                string createdBy = versionData[0];
                                DateTime createdDate;
                                string created = string.Empty;
                                if (versionData[1].StartsWith(Constants.UTCPrefix, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (DateTime.TryParse(versionData[1].Substring(Constants.UTCPrefix.Length), out createdDate))
                                        created = createdDate.ToLocalTime().ToString("MMM-d-yyyy");
                                }
                                else
                                {
                                    if (DateTime.TryParse(versionData[1], out createdDate))
                                        created = createdDate.ToString("MMM-d-yyyy");
                                }
                                string entry = versionData[2];
                                if (fieldName == DatabaseObjects.Columns.TicketResolutionComments)
                                    editCell.Text = entry;
                                else
                                    editCell.Text = string.Format("<b>{0}</b>: {1}", created, entry);
                            }
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.Title)
                    {
                        if (enableTicketReopenByRequestor && MTicketStatus.ToString() == "Closed" && ModuleName == "" && UserType == "my")
                        {
                            Panel panel = new Panel();
                            TableCell tCell = e.Row.Cells[cellIndex];
                            Label lbTaskAction = new Label();
                            string ticketPublicID = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                            //string title = UGITUtility.TruncateWithEllipsis(Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title)), 45, string.Empty);
                            title = UGITUtility.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
                            title = title.Replace("\r\n", ""); // Embedded newlines in title prevent popups from opening

                            panel.Attributes.Add("style", "position:relative;float:left;width:100%;");
                            lbTaskAction.CssClass = "action-container hide";
                            lbTaskAction.Text = string.Format("<img style='cursor: pointer; height:16px;width:16px;' src='/Content/images/return.png' title='Re-open ticket' onclick='event.cancelBubble= true; ReopenticketPopupCall(this,\"{0}\",\"{1}\");' />", ticketPublicID, title);
                            panel.Controls.Add(lbTaskAction);
                            tCell.Controls.Add(panel);

                            //Change background on mouse over
                            e.Row.Attributes.Add("onmouseover", "showTasksActions(this);");
                            e.Row.Attributes.Add("onmouseout", "hideTasksActions(this);");
                            allowExe = true;
                        }

                        if (!IsSLAMetricsDrilldown)
                        {
                            editCell.Style.Add(HtmlTextWriterStyle.Width, "300px");
                            editCell.Style.Add("min-width", "300px");
                        }
                    }
                    else if (fieldName == Constants.CRMSummary)
                        editCell.Text = String.Format("<div><a title='{1}' width='30px' height='30px' onclick='javascript:openProjectSummaryPage(this)' TicketId='{0}' ticketTitle='{1}' IsSummary=true>{2}</a></div>", ticketId, title, GenerateSummaryIcon(grid.GetDataRow(e.VisibleIndex), ticketId));

                    else if (columnType == "boolean")
                    {
                        editCell.Text = string.Empty;
                        if (e.GetValue(fieldName) != null)
                        {
                            bool value = UGITUtility.StringToBoolean(e.GetValue(fieldName));
                            editCell.Text = (value == true ? "Yes" : "No");
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketTotalHoldDuration)
                        editCell.Text = uHelper.GetFormattedHoldTime(currentRow, _context);
                    // truncateTextTo has value of length of string
                    if (!allowExe && truncateTextTo > 0)
                    {
                        string cellText = Convert.ToString(e.GetValue(fieldName));
                        editCell.ToolTip = cellText;
                        cellText = UGITUtility.TruncateWithEllipsis(cellText, truncateTextTo);
                        cellText = uHelper.ReplaceInvalidCharsInURL(cellText);
                        editCell.Text = cellText;
                    }
                    if (ModuleName == "PMM")
                    {
                        if (((GridViewDataColumn)editCell.Column).Name.StartsWith("projecthealth"))
                        {
                            string ticketID = Convert.ToString(e.KeyValue);
                            DataRow monitorRow = moduleMonitorsTable.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.MonitorName) == ((GridViewDataColumn)editCell.Column).FieldName);
                            long monitorId = 0;
                            if (monitorRow != null)
                                monitorId = UGITUtility.StringToLong(monitorRow[DatabaseObjects.Columns.ID]);
                            DataRow[] projectMonitorState = projectMonitorsStateTable.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.ModuleMonitorNameLookup, monitorId));
                            DataTable dt = new DataTable();
                            dt.Columns.Add(DatabaseObjects.Columns.ModuleMonitorName);
                            dt.Columns.Add(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup);
                            dt.Columns.Add(DatabaseObjects.Columns.ModuleMonitorOptionNameLookup);
                            dt.Columns.Add(DatabaseObjects.Columns.ProjectMonitorNotes);
                            if (projectMonitorState.Length > 0)
                            {
                                DataRow commRow = dt.NewRow();
                                DataRow monitorOptionRow = moduleMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == monitorId && x.Field<long>(DatabaseObjects.Columns.ID) == UGITUtility.StringToLong(projectMonitorState[0][DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                                //moduleMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup == Convert.ToString(monitorId) && x.Field<string>(DatabaseObjects.Columns.ID)== Convert.ToString(projectMonitorState[DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                                if (monitorOptionRow != null)
                                {
                                    commRow[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = monitorOptionRow[DatabaseObjects.Columns.ModuleMonitorOptionName];
                                    commRow[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = monitorOptionRow[DatabaseObjects.Columns.ModuleMonitorOptionLEDClass];
                                }
                                commRow[DatabaseObjects.Columns.ModuleMonitorName] = monitorRow[DatabaseObjects.Columns.MonitorName];
                                commRow[DatabaseObjects.Columns.ProjectMonitorNotes] = projectMonitorState[0][DatabaseObjects.Columns.ProjectMonitorNotes];
                                editCell.Text = UGITUtility.GetMonitorsGraphic(commRow);
                                //e.Row.Cells[cellIndex].Text = UGITUtility.GetMonitorsGraphic(commRow);
                            }
                        }
                    }

                    if (fieldName == DatabaseObjects.Columns.CRMCompanyLookup && !string.IsNullOrEmpty(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.CRMCompanyLookup])))
                    {
                        editCell.Text = UGITUtility.ObjectToString(currentRow["CRMCompanyTitle"]);
                    }

                    if (fieldName == "CRMCompanyTitle")
                    {
                        editCell.Style.Add(HtmlTextWriterStyle.Width, "250px");
                        editCell.Style.Add("min-width", "250px");
                    }
                    #region Code copied from OnHtmlDataCellPrepared event

                    string fieldNamevalue = string.Empty;

                    if (fieldName == DatabaseObjects.Columns.Attachments)
                    {
                        fieldNamevalue = Convert.ToString(e.GetValue(fieldName));
                        if (!string.IsNullOrEmpty(fieldNamevalue))
                        {
                            editCell.Text = string.Format("<img src='{0}'></img>", "/Content/images/attach.png");
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.TicketRequestor)
                    {

                        fieldNamevalue = Convert.ToString(e.GetValue(fieldName));
                        if (moduleColumns != null)
                            moduleColumn = moduleColumns.FirstOrDefault(x => x.FieldName == fieldName);
                        //FieldConfigurationManager Fieldmanger = new FieldConfigurationManager(context);
                        if (moduleColumn != null && !string.IsNullOrEmpty(moduleColumn.ColumnType) && (moduleColumn.ColumnType.Equals("MultiUser") || moduleColumn.ColumnType.Equals("UserField")))//FieldConfigurationManager.GetFieldByFieldName(e.DataColumn.FieldName) != null
                        {
                            var userlist = UGITUtility.ConvertStringToList(fieldNamevalue, Constants.Separator6);
                            var userPicture = "";
                            string userPictureUrl = "";
                            string userName = "";
                            var sb = new System.Text.StringBuilder();
                            if (userlist.Count > 1)
                            {
                                foreach (var user in userlist)
                                {
                                    var userProfile = UserManager.GetUserById(user);
                                    if (userProfile != null)
                                    {
                                        userName = FieldConfigurationManager.GetFieldConfigurationData(fieldName, user, moduleColumn: moduleColumn);
                                        sb.Append(userName).Append(",");
                                    }
                                }

                                if (sb.Length > 0)
                                    userName = sb.Remove(sb.Length - 1, 1).ToString();

                                if (!string.IsNullOrEmpty(userName))
                                {
                                    userPicture += $"<table class='RequestdByMultiUser_Table'><tr><td class='multiUserImg_wrap'><img src='/Content/Images/MultiUser.png' onerror= 'this.src =\"/Content/Images/MultiUser.png\"' title='{userName}' style ='border-radius: 50%; height: 25px; width: 25px; margin-right: 10px;'></img></td>" +
                                        $"<td><p class='multi_requester_name'>" + userName + "</p></td></tr></table>";
                                }
                            }
                            else
                            {
                                foreach (var user in userlist)
                                {
                                    var userProfile = UserManager.GetUserById(user);
                                    if (userProfile != null)
                                    {
                                        userName = FieldConfigurationManager.GetFieldConfigurationData(fieldName, user, moduleColumn: moduleColumn);
                                        if (!string.IsNullOrEmpty(userName))
                                        {
                                            userPictureUrl = userProfile.Picture;
                                            userPicture += $"<img src='{userPictureUrl}' onerror= 'this.src =\"/Content/Images/People94X94.png\"' title='{userName}' style ='border-radius: 50%; height: 25px; width: 25px; margin-right: 10px;' class='grid_img'></img> " + userName;
                                        }
                                    }
                                }

                            }

                            editCell.Text = userPicture;
                            editCell.HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (fieldName == DatabaseObjects.Columns.DepartmentLookup)
                    {
                        if (e.GetValue(fieldName) is string && !string.IsNullOrWhiteSpace((string)e.GetValue(fieldName)))
                        {
                            bool enableDivision = _configurationVariableManager.GetValueAsBool(ConfigConstants.EnableDivision);
                            if (enableDivision)
                            {
                                if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.DivisionLookup))
                                {
                                    string division = UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.DivisionLookup]);
    
                                    if (!string.IsNullOrWhiteSpace(division))
                                    {
                                        editCell.Text = division.Trim() + " > " + e.GetValue(fieldName);
                                    }
                                    else
                                    {
                                        if (UGITUtility.IfColumnExists(currentRow, $"{DatabaseObjects.Columns.DepartmentLookup}$Id"))
                                        {
                                            string deptID = UGITUtility.ObjectToString(currentRow[$"{DatabaseObjects.Columns.DepartmentLookup}$Id"]);
                                            if (!string.IsNullOrWhiteSpace(deptID))
                                            {
                                                string divisionName = _departmentManager.GetDivisionName(Convert.ToInt64(deptID));
                                                if (!string.IsNullOrWhiteSpace(divisionName))
                                                {
                                                    editCell.Text = divisionName + " > " + e.GetValue(fieldName);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    #endregion

                    #region Code copied from ASPX Grid View 
                    try
                    {
                        // if (!Page.IsPostBack)
                        {

                            Type dataType = typeof(string);
                            if (resultedTable.Columns.Contains(fieldName))
                                dataType = resultedTable.Columns[fieldName].DataType;
                            if (dataType == typeof(DateTime))
                            {
                                fieldNamevalue = Convert.ToString(e.GetValue(fieldName));
                                FieldConfigurationManager fmanger = new FieldConfigurationManager(_context);

                                if (!string.IsNullOrEmpty(fieldNamevalue))
                                {
                                    if (fmanger.GetFieldByFieldName(fieldName) != null)
                                    {
                                        string value = fmanger.GetFieldConfigurationData(fieldName, fieldNamevalue);
                                        editCell.Text = value;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                    #endregion
                }
            }

    }

        protected void grid_BeforeHeaderFilterFillItems(object sender, ASPxGridViewBeforeHeaderFilterFillItemsEventArgs e)
        {
        }

        protected void grid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            List<KeyValuePair<string, string>> nameCollection = new List<KeyValuePair<string, string>>();
            foreach (FilterValue fValue in e.Values)
            {
                if (!string.IsNullOrEmpty(fValue.ToString()))
                {
                    nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
                }
            }
            nameCollection = nameCollection.Distinct().ToList();
            e.Values.Clear();
            foreach (KeyValuePair<string, string> s in nameCollection)
            {
                FilterValue v = new FilterValue(s.Key, s.Value);
                e.Values.Add(v);
            }
            string fieldName = e.Column.FieldName;
            DataRow[] drs = moduleMonitorsTable != null ? moduleMonitorsTable.Where(x => x.Field<string>(DatabaseObjects.Columns.MonitorName) == fieldName).ToArray() : null;
            if ((drs != null && drs.Length == 0) || ModuleName != "PMM")
            {
                FilterValue fvBlanks = new FilterValue("(Blanks)", "Blanks", string.Format("not ([{0}] is not null and [{0}] <> '')", e.Column.FieldName)); //not ([{0}] is not null and [{0}] <> '')
                FilterValue fvNonBlanks = new FilterValue("(Non Blanks)", "Non Blanks", string.Format("[{0}] is not null and [{0}] <> ''", e.Column.FieldName));
                e.Values.Insert(0, fvNonBlanks);
                e.Values.Insert(0, fvBlanks);
            }
        }

        protected void grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ShowClearFilter = IsFilterOn();  //!IsPreview && 
            if (grid.JSProperties.ContainsKey("cpShowClearFilter"))
            {
                grid.JSProperties["cpShowClearFilter"] = IsFilterOn(); //!IsPreview && 
            }
            else
            {
                grid.JSProperties.Add("cpShowClearFilter", IsFilterOn()); // !IsPreview && 
            }

            if (grid.JSProperties.ContainsKey("cpClientID"))
            {
                grid.JSProperties["cpClientID"] = customMessageContainer.ClientID; //!IsPreview && 
            }
            else
            {
                grid.JSProperties.Add("cpClientID", customMessageContainer.ClientID); // !IsPreview && 
            }
            //SpDelta 40(Implementation of Show in tabs functionality to Custom filtered tickets.)
            if (e.CallbackName == "SORT" || e.CallbackName == "FILTERROWMENU" || e.CallbackName == "APPLYHEADERCOLUMNFILTER")
            {
                grid.PageIndex = 0;
                if (e.CallbackName == "SORT")
                {
                    UGITUtility.DeleteCookie(Request, Response, GetCookiePrefix() + "_SelectedTabName");
                    UGITUtility.CreateCookie(Response, GetCookiePrefix() + "_SelectedTabName", activeTab);
                }
            }

            if (e.CallbackName == "PAGERONCLICK" && e.Args.Length > 0 && e.Args[0].StartsWith("PSP"))
            {
                int pageCurrentSize = 0;
                int.TryParse(Convert.ToString(e.Args[0]).Replace("PSP", " ").Trim(), out pageCurrentSize);
                PageSize = pageCurrentSize;
                int oldPageSize = 0;

                if (UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants._GridPageSize) != null)
                {
                    int.TryParse(UGITUtility.GetCookieValue(Request, GetCookiePrefix() + Constants._GridPageSize), out oldPageSize);
                }
                oldPageSize = pageCurrentSize;
                UGITUtility.CreateCookie(Response, GetCookiePrefix() + Constants._GridPageSize, Convert.ToString(oldPageSize));
            }

            //
        }


        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            //// Added below condition, as drilldown on Charts is crashing (e.Value containing actual value instead of Id).
            //if (IsDashboard)
            //    return;

            //if (e.Column.FieldName == DatabaseObjects.Columns.Attachments)
            //{
            //    if (UGITUtility.StringToBoolean(e.Value))
            //    {
            //        e.DisplayText = string.Format("<img src='{0}'></img>", "/content/images/attach.gif");

            //    }
            //    else
            //    {
            //        e.DisplayText = "";
            //    }
            //}
            //if (e.Column.FieldName.Contains("User"))
            //{
            //    string userIDs = Convert.ToString(e.Value);
            //    if (!string.IsNullOrEmpty(userIDs))
            //    {
            //        if (userIDs != null)
            //        {
            //            string separator = Constants.Separator6;
            //            if (userIDs.Contains(Constants.Separator))
            //                separator = Constants.Separator;
            //            List<string> userlist = UGITUtility.ConvertStringToList(userIDs, separator);

            //            string commanames = UserManager.CommaSeparatedNamesFrom(userlist, Constants.Separator6);
            //            e.DisplayText = !string.IsNullOrEmpty(commanames) ? commanames : string.Empty;
            //        }
            //    }
            //}
            //if (e.Column.FieldName.Contains("Lookup"))
            //{
            //    string lookupid = Convert.ToString(e.Value);

            //    if (string.IsNullOrEmpty(lookupid))
            //        return;

            //    string values = FieldConfigurationManager.GetFieldConfigurationData(e.Column.FieldName, Convert.ToString(e.Value));
            //    if (!string.IsNullOrEmpty(values))
            //    {
            //        e.DisplayText = values;
            //    }
            //    //LookUpValueCollection lookUPValueCollection = new LookUpValueCollection(_context, e.Column.FieldName, lookupid, true);
            //    //if (lookUPValueCollection.lookupValues != null && lookUPValueCollection.lookupValues.Count > 0)
            //    //{
            //    //    e.DisplayText = UGITUtility.ObjectToString(lookUPValueCollection.lookupValues[0].Value);
            //    //}
            //}
            //string values = Convert.ToString(e.GetFieldValue(e.Column.FieldName));
            ////fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            //string value = string.Empty;
            ////field = FieldConfigurationManager.GetFieldByFieldName(e.Column.FieldName, this.ModuleName);
            //if (moduleColumns != null)
            //    moduleColumn = moduleColumns.FirstOrDefault(x => x.FieldName == e.Column.FieldName);

            //if (moduleColumn != null && (!string.IsNullOrEmpty(moduleColumn.ColumnType) || moduleColumn.FieldName.EndsWith("Lookup")))
            //{
            //    if (moduleColumn.FieldName == moduleColumn.FieldName && !moduleColumn.FieldName.EndsWith("Lookup") && (moduleColumn.ColumnType.Equals("MultiUser") || moduleColumn.ColumnType.Equals("UserField")))//(field != null && e.Column.FieldName == field.FieldName && field.Datatype == "UserField")
            //    {
            //        if (!moduleColumn.ColumnType.Equals("MultiUser"))
            //        {
            //            var managervalue = UserManager.GetUserById(values);
            //            if (managervalue != null)
            //            {
            //                e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
            //                e.DisplayText = managervalue.Name;
            //            }
            //            else
            //            {
            //                e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
            //                e.DisplayText = "";
            //            }
            //        }
            //        else
            //        {
            //            List<UserProfile> managervalue = UserManager.GetUserInfosById(values);
            //            if (managervalue != null)
            //            {
            //                e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
            //                e.DisplayText = string.Join(",", managervalue.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name));
            //            }
            //            else
            //            {
            //                e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
            //                e.DisplayText = "";
            //            }
            //        }
            //    }

            //    if (e.Column.FieldName == moduleColumn.FieldName && (moduleColumn.ColumnType == "Lookup" || moduleColumn.FieldName.EndsWith("Lookup")))
            //    {
            //        var fieldValue = FieldConfigurationManager.GetFieldConfigurationData(e.Column.FieldName, values);
            //        if (!string.IsNullOrEmpty(fieldValue))
            //        {
            //            e.Column.Settings.FilterMode = ColumnFilterMode.DisplayText;
            //            e.DisplayText = fieldValue;
            //        }
            //    }
            //}

        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
            string value = "";
            string Oldvalue = "";
            //
            Ticket ticketRequest = new Ticket(_context, this.ModuleName);
            ASPxGridView grid = sender as ASPxGridView;
            var ticketIds = grid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.TicketId });

            if (!grid.JSProperties.ContainsKey("cpAssignToMe"))
            {
                grid.JSProperties.Add("cpAssignToMe", string.Empty);
            }
            if (e != null)
            {
                if (e.Parameters == "AssignToMe")
                {
                    string actionUsers = ticketRequest.GetStageActionUsers(Request["StageTitle"], ModuleName);

                    if (ticketIds == null || ticketIds.Count == 0)
                    {
                        grid.JSProperties["cpAssignToMe"] = "Please select at least one item.";
                        return;
                    }
                    DataRow[] moduleStagesRow;
                    //DataRow spLItem;
                    DataRow ticket = null;

                    moduleStagesRow = UGITUtility.ToDataTable<LifeCycleStage>(ticketRequest.Module.List_LifeCycles[0].Stages).Select();                    //uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup, this.ModuleName).OrderBy(x => x.Field<double>(DatabaseObjects.Columns.ModuleStep)).ToArray();

                    foreach (string ticketId in ticketIds)
                    {
                        if (ticketRequest.Module != null)
                        {
                            TicketManager ticketManager = new TicketManager(_context);
                            DataTable ticketDT = ticketManager.GetTicketTableBasedOnTicketId(this.ModuleName, ticketId);  // SPListHelper.GetListItem(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), DatabaseObjects.Columns.TicketId, ticketId, "Text", SPContext.Current.Web);
                            if (ticketDT.Rows.Count > 0)
                                ticket = ticketDT.Select()[0];
                        }

                        if (ticket != null)
                        {
                            try
                            {
                                if (!UserManager.IsActionUser(ticket, User) && !isAdmin)
                                {
                                    grid.JSProperties["cpAssignToMe"] = "You don't have permission to self-assign the selected ticket(s)";
                                    return;
                                }

                                string currentStage = UGITUtility.SplitString(ticket[DatabaseObjects.Columns.ModuleStepLookup].ToString(), Constants.Separator, 1);
                                //SPFieldLookupValue currentStageLookup = new SPFieldLookupValue(Convert.ToString(ticket[DatabaseObjects.Columns.ModuleStepLookup]));
                                DataRow currentStageRow = moduleStagesRow.FirstOrDefault(x => x.Field<Int32>(DatabaseObjects.Columns.StageStep).ToString() == Convert.ToString(ticket[DatabaseObjects.Columns.StageStep]));
                                string customProperties = Convert.ToString(currentStageRow[DatabaseObjects.Columns.CustomProperties]);
                                Dictionary<string, string> dicCustomProperties = UGITUtility.GetCustomProperties(Convert.ToString(currentStageRow[DatabaseObjects.Columns.CustomProperties]), Constants.Separator);
                                bool IsSelfAssign = false;
                                if (dicCustomProperties.ContainsKey(CustomProperties.SelfAssign))
                                {
                                    IsSelfAssign = Convert.ToBoolean(dicCustomProperties[CustomProperties.SelfAssign]);
                                }
                                if (Convert.ToString(currentStageRow[DatabaseObjects.Columns.StageType]) != "Assigned" && !IsSelfAssign)
                                {
                                    if (!UGITUtility.StringToBoolean(Convert.ToString(currentStageRow[DatabaseObjects.Columns.AllowReassignFromList])) && (ticketIds == null || ticketIds.Count == 0))
                                    {
                                        grid.JSProperties["cpAssignToMe"] = "The selected ticket(s) cannot be re-assigned in the current stage";
                                        return;
                                    }
                                }

                                if (!IsSelfAssign && Convert.ToString(currentStageRow[DatabaseObjects.Columns.StageType]) != "Assigned" && (ticketIds == null || ticketIds.Count == 0))
                                {
                                    grid.JSProperties["cpAssignToMe"] = "The selected ticket(s) cannot be re-assigned in the current stage";
                                    return;
                                }

                                //SPFieldUserValue userValue = new SPFieldUserValue(SPContext.Current.Web, SPContext.Current.Web.CurrentUser.ID, SPContext.Current.Web.CurrentUser.Name);
                                //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
                                //List<UserProfile> userValue = _context.UserManager.GetUserInfosById(HttpContext.Current.GetManagerContext().CurrentUser.Id);
                                //value = string.Join<string>(",", userValue.Select(x => x.Id));
                                List<UserProfile> olduserValue = _context.UserManager.GetUserInfosById(Convert.ToString(ticket[DatabaseObjects.Columns.TicketPRP]));
                                Oldvalue = string.Join<string>(",", olduserValue.Select(x => x.Id));
                                string userValue = _context.CurrentUser.Id;

                                LifeCycleStage currentLifeCycleStage = ticketRequest.GetTicketCurrentStage(ticket);
                                //
                                ticket[DatabaseObjects.Columns.TicketPRP] = User.Id;
                                //ticket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(HttpContext.Current.GetManagerContext(), Convert.ToString(ticket[DatabaseObjects.Columns.TicketStageActionUserTypes]), ticket);

                                //made changes for AssignToMe functionality isn't working 
                                ticket[DatabaseObjects.Columns.TicketStageActionUsers] = User.Id;


                                if (IsSelfAssign) //Checking for Pending Assignment stage.
                                {
                                    ticketRequest.Approve(User, ticket, true);
                                }
                                //ticket.Update();
                                ticketRequest.CommitChanges(ticket, string.Empty);
                                //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
                                if (userValue != Oldvalue)
                                {
                                    TicketEventManager eventHelper = new TicketEventManager(_context, ticketRequest.Module.ModuleName, ticketId);
                                    eventHelper.LogEvent(Constants.TicketEventType.Assigned, currentLifeCycleStage, false, "", "", null, value);
                                }
                                //
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex, "Error re-assigning ticket");
                            }

                            //Update change after updating ticket
                            //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(Convert.ToInt16(spLItem[DatabaseObjects.Columns.ModuleId]), ticket);
                        }
                    }
                    resultedTable = null;
                }
                else
                {
                    grid.SettingsPager.PageSize = Convert.ToInt32(e.Parameters);
                    PageSize = Convert.ToInt32(e.Parameters);
                    UGITUtility.CreateCookie(Response, GetCookiePrefix() + Constants._GridPageSize, Convert.ToString(grid.SettingsPager.PageSize));
                }

                grid.DataBind();
            }
            grid.DataBind();
        }

        protected void grid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.JSProperties["cpPageSize"] = grid.SettingsPager.PageSize;
        }

        protected void cbRecords_Init(object sender, EventArgs e)
        {
            Int32[] values = { 10, 15, 20, 25, 50, 100 };
            ASPxComboBox cb = sender as ASPxComboBox;
            for (int i = 0; i < values.Length; i++)
            {
                cb.Items.Add(values[i].ToString(), values[i]);
            }
            cb.Value = PageSize;
        }

        #endregion

        #region Utility functions

        void LoadModuleColumns()
        {
            //if (IsSLAMetricsDrilldown)
            //    return;

            GetModuleNameByDashboardId();
            if (IsHomePage && !string.IsNullOrEmpty(MyHomeTab))
                CategoryName = CategoryName;
            else if (IsLoginWizard)
                CategoryName = "LoginWizard";
            else if (IsManagerView)
                CategoryName = ManagerViewModule;
            else if (string.IsNullOrEmpty(ModuleName))
                CategoryName = "MyHomeTab";
            else { CategoryName = ModuleName; }

            moduleColumns = moduleCols_ = CacheHelper<object>.Get($"ModuleColumnslistview{_context.TenantID}") as List<ModuleColumn>;
            if (moduleColumns == null)
            {
                ModuleColumnManager moduleColumnManager = new ModuleColumnManager(_context);
                moduleColumns = moduleColumnManager.Load(x => x.CategoryName == CategoryName);
                CacheHelper<object>.AddOrUpdate($"ModuleColumnslistview{_context.TenantID}", moduleColumnManager.Load());
            }
            else
            {
                moduleColumns = moduleColumns.Where(x => x.CategoryName == CategoryName).ToList();
            }

            //ModuleColumnManager moduleColumnManager = new ModuleColumnManager(_context);
            //moduleColumns = moduleColumnManager.Load(x => x.CategoryName == CategoryName);

            //foreach (ModuleColumn moduleColumn in moduleColumns)
            //{
            //    if (moduleColumn.FieldName.EndsWith("Lookup") || moduleColumn.FieldName.EndsWith("User"))
            //    {
            //        moduleColumn.FieldName = moduleColumn.FieldName + "$";
            //    }

            //}
            if (IsDashboard)
            {
                if (!string.IsNullOrEmpty(Request["dID"]))
                {
                    int dID = 0;
                    int.TryParse(Request["dID"], out dID);
                    Guid linkID = Guid.Empty;
                    if (Request["kID"] != null)
                    {
                        try
                        {
                            linkID = new Guid(Request["kID"]);
                        }
                        catch { }
                    }
                    Dashboard dashboard = DashboardManager.LoadPanelById(dID, true);
                    PanelSetting panel = dashboard.panel as PanelSetting;
                    DashboardPanelLink panelLink = panel.Expressions.FirstOrDefault(x => x.LinkID == linkID);
                    if (panelLink != null && !string.IsNullOrEmpty(panelLink.ShowColumns_Category))
                        moduleColumns = moduleCols_.Where(x => x.CategoryName == panelLink.ShowColumns_Category).ToList();
                    else if (panelLink != null && !string.IsNullOrEmpty(ModuleName))
                        moduleColumns = moduleCols_.Where(x => x.CategoryName == ModuleName).ToList();
                    else
                        moduleColumns = moduleCols_.Where(x => x.CategoryName == Constants.MyDashboardTicket).ToList();
                }
                else if (ModuleName == "" && CategoryName == Constants.MyHomeTicketTab && !string.IsNullOrEmpty(Request["dashboardID"]))
                {
                    //Code added by Inderjeet Kaur on 28-09-2022 to fix drill down of charts when ModuleName =""
                    //Format columns because this data will bind with the grid as it is. Remove $id columns, and rename Lookup and User columns. 
                    //for (int i = resultedTable.Columns.Count - 1; i >= 0; i--)
                    //{
                    //    DataColumn dc = resultedTable.Columns[i];
                    //    if (dc.ColumnName.ToLower().Contains("$id") || dc.ColumnName.ToLower() == DatabaseObjects.Columns.TenantID.ToLower())
                    //        resultedTable.Columns.Remove(dc);
                    //    else if (dc.ColumnName.EndsWith("Lookup") || dc.ColumnName.EndsWith("User"))
                    //    {
                    //        dc.ColumnName = dc.ColumnName.Replace("Lookup", "").Replace("User", "");
                    //    }
                    //}
                }
                else
                {
                    moduleColumns = moduleCols_.Where(x => x.CategoryName == Constants.MyDashboardTicket).ToList();
                }

            }
            else
            {
                if (moduleColumns == null)
                    moduleColumns = moduleCols_.Where(x => x.CategoryName == Constants.MyHomeTicketTab).ToList();
            }

            if (moduleColumns != null)
            {
                moduleColumns = moduleColumns.OrderBy(x => x.FieldSequence).ToList();
            }
            //foreach (ModuleColumn moduleColumn in moduleColumns)
            //{
            //    if (moduleColumn.FieldName.EndsWith("Lookup") || moduleColumn.FieldName.EndsWith("User"))
            //    {
            //        moduleColumn.FieldName = moduleColumn.FieldName + "$";
            //    }

            //}
        }


        protected string GetCookiePrefix()
        {
            string cookiePrefix = "Home_" + MTicketStatus.ToString();
            if (IsDashboard)
            {
                cookiePrefix = "Dashboard";
            }
            else if (!string.IsNullOrEmpty(this.ModuleName))
            {
                cookiePrefix = this.ModuleName;
            }
            else if (Request.QueryString.ToString().Contains("globalSearchString"))
            {
                cookiePrefix = "GlobalSearch";
            }
            return cookiePrefix;
        }

        /// <summary>
        /// This method checks if any Query exists for currrent Module.
        /// </summary>
        /// <returns></returns>
        private Boolean IsReportexist()
        {
            //DataTable modules = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules);
            //DataTable dashboardPanels = uGITCache.GetDataTable(DatabaseObjects.Lists.DashboardPanels);
            //if (dashboardPanels != null)
            //{
            //    dashboardPanels = dashboardPanels.Copy();
            //    dashboardPanels.Columns.Add("ModuleNameTemp");
            //    dashboardPanels.Columns["ModuleNameTemp"].Expression = string.Format("'{0}'+{1}+'{0}'", Constants.Separator, DatabaseObjects.Columns.DashboardModuleMultiLookup);
            //    DataRow[] tempRows = null;
            //    tempRows = dashboardPanels.Select(string.Format("{0} like '%{2}{1}{2}%'", "ModuleNameTemp", ModuleName, Constants.Separator));
            //    if (tempRows.Count() > 0)
            //        return true;
            //}

            return false;
        }

        private void ClearALLFilters()
        {
            dtFrom.Text = "";
            dtTo.Text = "";
            txtWildCard.Text = string.Empty;
            ddlModuleName.ClearSelection();
            lstFilteredFields.ClearSelection();
            pGlobalFilters.CssClass = string.Empty;
            grid.FilterExpression = string.Empty;
            customMessageContainer.Style.Add("display", "none");
            ShowClearFilter = false;
        }

        private void ClearFilterStateCookies()
        {
            UGITUtility.DeleteCookie(Request, Response, GetCookiePrefix() + Constants.FromDateExpression);
            UGITUtility.DeleteCookie(Request, Response, GetCookiePrefix() + Constants.ToDateExpression);
            UGITUtility.DeleteCookie(Request, Response, GetCookiePrefix() + Constants.WildCardExpression);

            // Clear globalSearchString parameter if user clears filter from globalsearch window
            if (Request.QueryString.ToString().Contains("globalSearchString"))
            {
                string listUrl = Request.Url.GetLeftPart(UriPartial.Path);
                NameValueCollection queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                string searchString = queryCollection.Get("globalSearchString");
                if (!string.IsNullOrEmpty(searchString))
                {
                    queryCollection.Set("globalSearchString", string.Empty);
                    listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
                    if (!Page.IsCallback)
                        Response.Redirect(listUrl, true);
                }
            }
        }

        private bool IsFilterOn()
        {
            return (txtWildCard.Text.Trim().Replace(wildCardWaterMark, string.Empty) != string.Empty ||
                    //!dtFrom.IsDateEmpty || !dtTo.IsDateEmpty || 
                    (grid.FilterEnabled && !string.IsNullOrEmpty(grid.FilterExpression)) && !DisplayOnDashboard); // (grid.SortCount > 0) ||
        }

        /// <summary>
        /// Show hide details like logo, description, newbutton, global search, tabs
        /// Set some basic settings like (ticketurl)
        /// </summary>
        private void ShowHideDetailsWithBasicSetting(DataRow moduleDetail)
        {
            cModuleDetailPanel.Visible = true;
            moduleLogo.Visible = false;
            moduleDescription.Visible = false;
            cModuleFilteredLinksPanel.Visible = false;
            cModuleGlobalFilterPanel.Visible = false;
            btNewbutton.Visible = false;
            btnNewContact.Visible = false;
            //filteredListUpperhead.Visible = false;
            mModuleNewTicketPanel.Visible = false;

            string closedTicketTitle = string.Empty;
            if(Convert.ToBoolean(Request.QueryString["isClosed"]) == true)
            {
                string strClosedTicketsTitleModuleWise = UGITUtility.ObjectToString(_configurationVariableManager.GetValue("ProjectLibraryModuleWiseTitle"));
                Dictionary<string, string> cpClosedTicketsTitleModuleWise = UGITUtility.GetCustomProperties(strClosedTicketsTitleModuleWise, Constants.Separator);
                closedTicketTitle = UGITUtility.ObjectToString(cpClosedTicketsTitleModuleWise[Convert.ToString(moduleDetail[DatabaseObjects.Columns.ModuleName]).ToLower()]);
            }            

            string moduleName = moduleDetail != null ? Convert.ToString(moduleDetail[DatabaseObjects.Columns.ModuleName]) : string.Empty;

            if (!string.IsNullOrWhiteSpace(moduleName))
                moduleRequest = new Ticket(_context, moduleName, User);
            else if (!string.IsNullOrWhiteSpace(Request["ModuleName"]))
                moduleRequest = new Ticket(_context, Request["ModuleName"], User);


            if (moduleRequest != null && moduleRequest.Module != null)
            {
                //Show logo
                if (HideModuleImage)
                {
                    moduleLogo.Visible = false;
                    cModuleImgPanel.Visible = false;
                    moduleLogo.ImageUrl = moduleRequest.Module.IconUrl;
                }
                else
                {
                    moduleLogo.Visible = true;
                    cModuleImgPanel.Visible = true;
                    moduleLogo.ImageUrl = moduleRequest.Module.IconUrl;
                }

                //Show description
                if (HideModuleDesciption)
                {
                    moduleDescription.Visible = false;
                }
                else
                {
                    moduleDescription.Visible = true;
                    if (Convert.ToBoolean(Request.QueryString["isClosed"]) == true)
                    {
                        moduleDescription.Text = string.Format("<div class='m-desc-shortname'>{0}</div>", closedTicketTitle);
                    }
                    else
                    {
                        if (uHelper.IsCPRModuleEnabled(_context))
                        {
                            moduleDescription.Text = string.Format("<div class='m-desc-shortname'>{0}</div>", moduleRequest.Module.ShortName);
                        }
                        else
                        {
                            moduleDescription.Text = moduleRequest.Module.ModuleDescription;
                        }
                    }
                }

                typeName = UGITUtility.moduleTypeName(moduleName); //BTS-22-000880

                //Show tabls
                if (!HideMyFiltersLinks)
                {
                    cModuleFilteredLinksPanel.Visible = true;
                }
                else
                {
                    cModuleFilteredLinksPanel.Visible = false;
                }


                //Show new button
                if (!HideNewTicketButton)
                {
                    if (ModuleName == "CON")
                    {
                        btnNewContact.Visible = true;
                    }
                    else
                    {
                        btNewbutton.Visible = true;
                    }

                    mModuleNewTicketPanel.Visible = true;

                    //Module Logo link
                    moduleLogo.Style.Add("border", "2px outset #F0F0F0");
                    moduleLogo.Attributes.Add("onmousedown", "$(this).css('border','2px inset #F0F0F0')");
                    moduleLogo.Attributes.Add("onmouseup", "$(this).css('border','2px outset #F0F0F0')");
                    moduleLogo.Attributes.Add("onmouseout", "$(this).css('border','2px outset #F0F0F0')");



                    btNewbutton.Attributes.Add("href", "javascript:");
                    //string url =  UGITUtility.GetAbsoluteURL(moduleRequest.Module.DetailPageUrl);
                    string url = UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath);
                    string title = UGITUtility.newTicketTitle(moduleName); /*"New " + ModuleName + " Ticket";*/
                    int width = 90;
                    int height = 90;
                    if (moduleName == "NPR" || moduleName == "INC")
                    {
                        moduleLogo.ToolTip = btNewbutton.ToolTip = "Create New " + typeName;//BTS-22-000880
                    }
                    else if (moduleName == "PMM")
                    {
                        title = "Project Import Wizard";
                        moduleLogo.ToolTip = btNewbutton.ToolTip = "Import Project"; //BTS-22-000880
                        width = 60;
                        height = 90;
                        url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl?ctrl=PMM.AddNewProject");
                    }
                    else if (moduleName == "SVC")
                    {
                        title = "New Request";
                        moduleLogo.ToolTip = btNewbutton.ToolTip = "New Request";
                        url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard")); //UGITUtility.GetAbsoluteURL("/SitePages/ServiceWizard.aspx");
                    }
                    else
                    {
                        moduleLogo.ToolTip = btNewbutton.ToolTip = "New " + typeName; //BTS-22-000880
                    }

                    //btNewbutton.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', 'TicketId=0', '{1}', '90', '90', 0);", url, title));

                    if (moduleName.Equals("CON", StringComparison.InvariantCultureIgnoreCase))
                    {
                        btNewbutton.Attributes.Add("onclick", "NewContactbyCompany();");
                        moduleLogoLink.Attributes.Add("onclick", "NewContactbyCompany();");
                    }
                    else
                    {
                        btNewbutton.Attributes.Add("onclick", string.Format("openTicketDialog('{0}', 'TicketId=0', '{1}', '{3}', '{4}', 0,'{2}');", url, title, sourceURL, width, height));
                        moduleLogoLink.Attributes.Add("onclick", string.Format("openTicketDialog('{0}', 'TicketId=0', '{1}', '{3}', '{4}', 0, '{2}');", url, title, sourceURL, width, height));
                    }
                    // For Asset Management
                    if (moduleName.Equals("CMDB", StringComparison.InvariantCultureIgnoreCase) && isAdmin)
                    {
                        width = 80;
                        height = 80;
                        url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetvendorview");
                        btVendors.Attributes.Add("onclick", string.Format("openTicketDialog('{0}', '', '{1}', '{3}', '{4}', 0,'{2}');", url, "Vendors", sourceURL, width, height));

                        url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=assetmodelview");
                        btAssetModels.Attributes.Add("onclick", string.Format("openTicketDialog('{0}', '', '{1}', '{3}', '{4}', 0,'{2}');", url, "Asset Models", sourceURL, width, height));
                    }
                }
                else
                {
                    btNewbutton.Visible = false;
                }
                ticketURL = UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath);
                autoRefreshListFrequency = UGITUtility.StringToInt(moduleRequest.Module.AutoRefreshListFrequency);

                #region show/hide new and quick ticket button ...

                bool allowshowAddNew = true;
                if (!string.IsNullOrEmpty(moduleRequest.Module.AuthorizedToCreate)/*&& Convert.ToInt32( moduleRequest.Module.AuthorizedToCreate) > 0*/)
                {
                    allowshowAddNew = false;
                    List<string> authorizedToCreateList = UGITUtility.ConvertStringToList(moduleRequest.Module.AuthorizedToCreate, Constants.Separator6);
                    if (authorizedToCreateList.Count > 0)
                    {
                        //if (UserManager.IsUserExistInList(User, authorizedToCreateList))
                        if (authorizedToCreateList.Contains(User.Id) || UserManager.IsUserinGroups(User.Id, authorizedToCreateList))
                        {
                            allowshowAddNew = true;
                            //break;
                        }
                    }
                    //foreach (UserInfo uInfo in moduleRequest.Module.AuthorizedToCreate)
                    //{
                    //    //if ((uInfo.IsGroup && UserProfile.CheckUserIsInGroup(uInfo.ID, SPContext.Current.Web.CurrentUser)) ||
                    //    //    SPContext.Current.Web.CurrentUser.ID == uInfo.ID)
                    //    //{
                    //    //    allowshowAddNew = true;
                    //    //    break;
                    //    //}
                    //}
                }

                if (allowshowAddNew == false)
                {
                    btNewbutton.Visible = false;
                    btnNewContact.Visible = false;
                    moduleLogoLink.Attributes.Remove("onclick");
                    btQuickTicket.Visible = false;
                }
                else
                {
                    if (ModuleName == "CON")
                    {
                        btnNewContact.Visible = true;
                    }
                    else
                    {
                        if (!HideNewTicketButton)
                        {
                            btNewbutton.Visible = true;
                        }
                        //btNewbutton.Visible = true;
                    }

                    btQuickTicket.Visible = moduleRequest.Module.EnableQuick;
                }

                //here we set the quick ticket button text.

                //BTS-22-000880
                btQuickTicket.Text = "Quick " + typeName;
                //btQuickTicket.Text = "Quick " + "Ticket";


                // Show/Hide Buttons for asset Management Module
                if (moduleName.Equals("CMDB", StringComparison.InvariantCultureIgnoreCase) && isAdmin)
                {
                    btVendors.Visible = true;
                    btAssetModels.Visible = true;
                }
                else
                {
                    btVendors.Visible = false;
                    btAssetModels.Visible = false;
                }

                #endregion
            }

            //Show Global filter
            if (!HideGlobalSearch)
            {
                cModuleGlobalFilterPanel.Visible = true;
            }


            if (string.IsNullOrEmpty(moduleLogo.ImageUrl))
            {
                moduleLogo.Visible = false;
            }

            if (uHelper.IsCPRModuleEnabled(_context))
            {
                moduleLogoLink.Visible = false;
                moduleLogo.Visible = false;
            }

            if (HideModuleDetail)
            {
                moduleLogoLink.Visible = false;
                cModuleDescriptionPanel.Visible = false;
                moduleLogo.Visible = false;
                cModuleDetailPanel.Visible = false; // hide for main Home dashboard
                moduleDescription.Visible = false;
                cModuleFilteredLinksPanel.Visible = false;
                cModuleGlobalFilterPanel.Visible = false;
                if (IsHomedashboardCtrl)
                {
                    cModuleFilteredLinksPanel.Visible = true;
                    cModuleGlobalFilterPanel.Visible = true;

                    if (Convert.ToString(Request["Status"]).EqualsIgnoreCase("all"))
                        ddlCustomFilterHome.Visible = true;
                    else
                        ddlCustomFilterHome.Visible = false;

                    btQuickTicket.Visible = false;
                    dvActions.Visible = false;
                    btnNewContact.Visible = false;
                }
                else
                {
                    cModuleFilteredLinksPanel.Visible = false;
                    cModuleGlobalFilterPanel.Visible = false;
                }
                btNewbutton.Visible = false;
                mModuleNewTicketPanel.Visible = false;
            }

            //Load All Monitor in case of pmm projects
            if (ModuleName == "PMM" || (IsHomePage && !string.IsNullOrEmpty(MyHomeTab) && (MyHomeTab.ToLower() == "myprojecttab" || MyHomeTab.ToLower() == "businessinitiatives")))
            {
                //projectMonitorsStateTable = uGITCache.LoadTable(DatabaseObjects.Lists.ProjectMonitorState);
                //moduleMonitorsTable = uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleMonitors, string.Format("{0}='PMM'", DatabaseObjects.Columns.ModuleNameLookup));
                projectMonitorsStateTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'"); //uGITCache.LoadTable(DatabaseObjects.Tables.ProjectMonitorState);
                moduleMonitorsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitors, $"{DatabaseObjects.Columns.ModuleNameLookup}='{ModuleName}' and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'").Select(); //uGITCache.GetDataTable(DatabaseObjects.Tables.ModuleMonitors, string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleName));
            }
            if (Request.QueryString != null && Request.QueryString.AllKeys.Length > 0)
            {
                if (Convert.ToString(Request.QueryString["control"]) == "problemreportdrildowndata")
                {
                    ddlCustomFilterHome.Visible = false;
                    lnkbtnActionMenu.Visible = false;
                    btnCardView.Visible = false;
                    btQuickTicket.Visible = false;
                    moduleLogo.Visible = true;
                    cModuleImgPanel.Visible = false;
                    moduleLogo.Visible = false;
                    moduleDescription.Visible = false;
                    btNewbutton.Visible = false;
                    imgReport.Visible = false;
                    cModuleDescriptionPanel.Visible = false;
                }
            }
        }
        /// <summary>
        /// Apply all filters and get data
        /// </summary>
        /// <returns></returns>
        private DataTable GetFilteredData()
        {
            GetBaseFilteredData();
            GetGlobalFilteredData();
            if (resultedTable != null && resultedTable.Rows.Count > 0)
            {
                if (!IsPostBack || hndClearFilter.Value.Contains("__ClearFilter__"))
                    GenerateColumns();
            }

            UGITModule uGITModule = currentUGITModule;
            
            if (resultedTable != null && resultedTable.Columns.Contains(DatabaseObjects.Columns.IsPrivate) && !_context.UserManager.IsUGITSuperAdmin(_context.CurrentUser))
            {
                DataTable privateTickets = null;
                EnumerableRowCollection<DataRow> dc = resultedTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.IsPrivate) && UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.IsPrivate]));
                if (dc != null && dc.Count() > 0)
                    privateTickets = dc.CopyToDataTable();
                if (privateTickets != null && privateTickets.Rows.Count > 0)
                {
                    var arrOfModules = (from dtPrivate in privateTickets.AsEnumerable()
                                        select new
                                        {
                                            Module = dtPrivate.Field<string>(DatabaseObjects.Columns.TicketId).Split('-')[0]
                                        });
                    DataTable authorizedPrivateTickets = privateTickets.Clone();
                    if (arrOfModules != null && arrOfModules.Count() > 0)
                    {
                        List<string> lstOfModules = arrOfModules.Select(x => x.Module).Distinct().ToList();
                        List<string> lstOfAuthorizeUser = _context.UserManager.GetUserRoles(_context.CurrentUser.Id).Select(x => x.Id).ToList();
                        lstOfAuthorizeUser.Add(_context.CurrentUser.Id);
                        foreach (string module in lstOfModules)
                        {
                            DataRow[] rowColl = null;
                            if (uGITModule == null || module != uGITModule.ModuleName)
                                uGITModule = _moduleViewManager.LoadByName(module, true);
                            if (uGITModule == null)
                                continue;
                            List<string> userTypeColumnColl = uGITModule.List_ModuleUserTypes.Select(x => x.ColumnName).Distinct().ToList();

                            StringBuilder queryExpress = new StringBuilder();
                            List<string> qExpress = new List<string>();
                            //userTypeColumnColl.ForEach(x =>
                            //{
                            //    lstOfAuthorizeUser.ForEach(y => qExpress.Add(string.Format("{0} like ('%{1}%')", x, y)));
                            //});
                            string strUsers = string.Join(",", lstOfAuthorizeUser.Select(x => "'" + x + "'").ToList());
                            foreach (string statusName in userTypeColumnColl)
                            {
                                if (privateTickets != null && UGITUtility.IfColumnExists(privateTickets.NewRow(), statusName))
                                {
                                    qExpress.Add(string.Format("{0} in ({1})", statusName, strUsers));
                                }
                            }


                            if (qExpress.Count > 0)
                                queryExpress.AppendFormat("{0}", string.Join(" OR ", qExpress));
                            if (!string.IsNullOrEmpty(queryExpress.ToString()))
                            {
                                rowColl = privateTickets.Select(queryExpress.ToString());
                                if (rowColl != null && rowColl.Length > 0)
                                    authorizedPrivateTickets.Merge(rowColl.CopyToDataTable());
                            }
                        }
                        //DataRow[] nonPrivateTickets = resultedTable.AsEnumerable().Where(x => !UGITUtility.StringToBoolean(x[DatabaseObjects.Columns.IsPrivate])).ToArray();
                        //if (nonPrivateTickets != null && nonPrivateTickets.Length > 0)
                        //    resultedTable = nonPrivateTickets.CopyToDataTable();
                        //else
                        //    resultedTable = resultedTable.Clone();
                        //if (authorizedPrivateTickets != null && authorizedPrivateTickets.Rows.Count > 0)
                        //    resultedTable.Merge(authorizedPrivateTickets);
                    }
                }
            }
            // filtering result set by TenantID            
            //resultedTable = resultedTable.AsEnumerable().Where(x => x.Field<string>("TenantID") != null && x.Field<string>("TenantID").Equals(TenantID, StringComparison.InvariantCultureIgnoreCase)).CopyToDataTable();
            return resultedTable;
        }

        private void GetModuleNameByDashboardId()
        {
            if (IsDashboard)
            {
                ChartSetting chartSetting = null;
                Dashboard dashboard = null;
                int dID = 0;
                int.TryParse(Request["dashboardID"], out dID);

                if (dID == 0)
                {
                    int.TryParse(Request["dID"], out dID);
                }

                Guid linkID = Guid.Empty;
                if (Request["kID"] != null)
                {
                    try
                    {
                        linkID = new Guid(Request["kID"]);
                    }
                    catch { }
                }
                if (dID > 0 && linkID != Guid.Empty)
                {
                    dashboard = DashboardManager.LoadPanelById(dID, true);
                    PanelSetting panel = dashboard.panel as PanelSetting;
                    DashboardPanelLink panelLink = panel.Expressions.FirstOrDefault(x => x.LinkID == linkID);
                    ExpressionCalc expCalc = new ExpressionCalc(panelLink.DashboardTable, _context);
                    DataRow[] selecteModules = ModuleViewManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleTicketTable, panelLink.DashboardTable));
                    string mName = string.Empty;
                    if (selecteModules.Length > 0)
                    {
                        mName = Convert.ToString(selecteModules[0][DatabaseObjects.Columns.ModuleName]);
                    }
                    else if (panelLink.DashboardTable.ToLower().Contains("-opentickets") || panelLink.DashboardTable.ToLower().Contains("-closedtickets"))
                    {
                        string[] dashboardTable = panelLink.DashboardTable.Split('-');
                        mName = dashboardTable[0];
                    }
                    ModuleColumnManager columnManager = new ModuleColumnManager(_context);
                    //Added by Inderjeet Kaur on 20/09/2022
                    //When Dashboard table is AspNetUsers, then fetch display column as per ResourceTab.
                    if (panelLink.DashboardTable == DatabaseObjects.Tables.AspNetUsers)
                    {
                        mName = Utility.Constants.ResourceTab;
                    }
                    DataRow[] columnRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'").Select(string.Format("{0}='{1}' and ({2}={3} or {4} IS NOT NULL)", DatabaseObjects.Columns.CategoryName, mName, DatabaseObjects.Columns.IsDisplay, true, DatabaseObjects.Columns.SelectedTabs));
                    int dashboardViewID = 0;
                    int.TryParse(Request["viewID"], out dashboardViewID);
                    if (dashboardViewID > 0)
                    {
                        //DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(_context);

                        DashboardPanelView view = _dashboardPanelViewManager.LoadViewByID(dashboardViewID, true);
                        string globalFilter = string.Empty;
                        globalFilter = DashboardHelper.GetGlobalFilter(_context, Request["globalFilter"], view, expCalc.FactTable);
                        expCalc.ApplyFilter(globalFilter);
                    }
                    DataTable filteredTable = expCalc.GetFilteredData(panelLink);

                    //columnRow.CopyToDataTable().DefaultView.Sort = "FieldSequence asc";
                    List<string> columns = columnRow.AsEnumerable().Select(r => r.Field<string>(DatabaseObjects.Columns.FieldName)).ToList();
                    //DataTable filteredTable = expCalc.GetFilteredData(panelLink);
                    if (dID == 0)
                    {
                        //for (int i = 0; i < columns.Count; i++)
                        //{
                        //    if (columns[i].EndsWith("Lookup") || columns[i].EndsWith("User"))
                        //    {
                        //        columns[i] = columns[i] + "$";

                        //    }
                        //}
                    }
                    // Added TicketId column, as resultedTable requires it & without it is throwing exception while binding CardView.
                    if (columns.Count > 0 && !columns.Contains(DatabaseObjects.Columns.TicketId) && filteredTable.Columns.Contains(DatabaseObjects.Columns.TicketId))
                    {
                        columns.Add(DatabaseObjects.Columns.TicketId);
                    }

                    if (columns != null && columns.Count > 0)
                    {
                        DataView dv = new DataView(filteredTable);
                        columns = columns.Intersect(filteredTable.Columns.Cast<DataColumn>()
                                      .Select(x => x.ColumnName)
                                      .ToList()).ToList();
                        filteredTable = dv.ToTable(false, columns.ToArray());
                    }

                    string coluName = Request["varName"];
                    string coluVal = Request["varVal"];

                    if (filteredTable != null && filteredTable.Rows.Count > 0 && coluName != null && coluVal != null)
                    {
                        filteredTable = (from ft in filteredTable.AsEnumerable()
                                         where ft.Field<string>(coluName) == coluVal
                                         select ft).CopyToDataTable();
                    }
                    this.FilteredTable = filteredTable;
                    resultedTable = filteredTable;
                    if (resultedTable != null && !resultedTable.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                    {
                        resultedTable.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
                        resultedTable.Columns[DatabaseObjects.Columns.ModuleNameLookup].Expression = string.Format("'{0}'", mName); //string.Format("'{0}'", ModuleName);
                    }

                    AddProjectTeamAgent(resultedTable, mName);

                    if (mName != string.Empty)
                    {
                        this.ModuleName = mName;
                    }
                    else
                    {
                        this.HideNewTicketButton = true;
                        this.HideModuleDesciption = true;
                        this.HideModuleImage = true;
                        this.HideMyFiltersLinks = true;
                        this.HideNewTicketButton = true;
                        this.HideReport = true;
                    }
                }
                else
                {
                    cTicketPreviewPanel.Visible = false;

                    int dashboardID = 0;
                    int.TryParse(Request["dashboardID"], out dashboardID);
                    int dashboardViewID = 0;
                    int.TryParse(Request["dashboardViewID"], out dashboardViewID);



                    DashboardPanelView view = _dashboardPanelViewManager.LoadViewByID(dashboardViewID, true);

                    int dimension = 0;
                    int.TryParse(Request["dimension"], out dimension);
                    string localFilter = Request["lFilter"];
                    string globalFilter = ChartHelper.GetGlobalFilter(_context, Request["gFilter"], view);
                    string datapointFilter = Request["eFilter"];

                    if (dashboardID > 0)
                    {
                        dashboard = DashboardManager.LoadPanelById(dID, true);
                        chartSetting = (ChartSetting)dashboard.panel;

                        if (chartSetting !=null && chartSetting.FactTable.ToLower().Contains("-opentickets") || chartSetting.FactTable.ToLower().Contains("-alltickets") || chartSetting.FactTable.ToLower().Contains("-closedtickets"))
                        {
                            string[] dashboardTable = chartSetting.FactTable.Split('-');
                            ModuleName = dashboardTable[0];
                        }

                        ChartHelper chartHelper = new ChartHelper(chartSetting, _context);
                        chartHelper.ChartTitle = string.Empty;
                        chartHelper.DrillDownFilter = dimension;
                        chartHelper.LocalFilter = localFilter;
                        chartHelper.GlobalFilter = globalFilter;
                        chartHelper.DatapointFilter = datapointFilter;
                        resultedTable = chartHelper.GetFilteredData();
                        if (ModuleName == "" && CategoryName == Constants.MyHomeTicketTab)
                        {
                            //Code added by Inderjeet Kaur on 28-09-2022 to fix drill down of charts when ModuleName =""
                            //Format columns because this data will bind with the grid as it is. Remove $id columns, and rename Lookup and User columns. 
                            //for (int i = resultedTable.Columns.Count - 1; i >= 0; i--)
                            //{
                            //    DataColumn dc = resultedTable.Columns[i];
                            //    if (dc.ColumnName.ToLower().Contains("$id") || dc.ColumnName.ToLower() == DatabaseObjects.Columns.TenantID.ToLower())
                            //        resultedTable.Columns.Remove(dc);
                            //    else if (dc.ColumnName.EndsWith("Lookup") || dc.ColumnName.EndsWith("User"))
                            //    {
                            //        dc.ColumnName = dc.ColumnName.Replace("Lookup", "").Replace("User", "");
                            //    }
                            //}
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Apply base filtered 
        /// </summary>
        private void GetBaseFilteredData()
        {
            if (FilteredTable != null)
            {
                IsFilteredTableExist = true;
            }
            if (IsManagerView)
            {
                if (filterTab.Tabs.Count > 0)
                {
                    Tab activetab = filterTab.ActiveTab;
                    filterTab.Tabs.Clear();
                    filterTab.Tabs.Add(activetab);
                }
            }
            if (IsFilteredTableExist)
            {
                resultedTable = FilteredTable;
            }
            else if (!IsDashboard)
            {
                LoadFilteredTickets();
            }
            else if (IsDashboard)
            {
                GetModuleNameByDashboardId();
            }

            if (IsHomedashboardCtrl && Convert.ToString(Request["Status"]).EqualsIgnoreCase("all"))
            {
                LoadFilteredTickets();
            }

            CustomGridModifications();

        }
        /// <summary>
        /// Use this for adding "Calculated fields" in the Filter tickets table
        /// </summary>
        private void CustomGridModifications()
        {
            if (resultedTable == null)
            {
                return;
            }
            DataRow[] columns = null;
            DataTable dt = (DataTable)CacheHelper<object>.Get($"ModuleColumns{_context.TenantID}", _context.TenantID);
            if (dt == null)
            {
                ModuleColumnManager moduleColumnManager = new ModuleColumnManager(_context);
                columns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'").Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, ModuleName));
                //moduleColumnManager.GetDataTable().Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, ModuleName));
            }
            else
            {
                columns = dt.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.CategoryName, ModuleName));
            }
            #region Calculated Columns
            if (!resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketDueIn)
                && resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
            {
                //DataTable columns = uGITCache.ModuleConfigCache.LoadModuleListByName(ModuleName, DatabaseObjects.Tables.ModuleColumns); //uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
                // Check if we have a column by TicketAge in the ModulesColumns table for the current module
                DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == ModuleName
                    && x.Field<bool>(DatabaseObjects.Columns.IsDisplay) == true
                    && x.Field<string>(DatabaseObjects.Columns.FieldName) == DatabaseObjects.Columns.TicketDueIn).ToArray(); //.OrderBy(x => x.Field<double>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                if ((selectedColumns != null && selectedColumns.Count() > 0) || ModuleName == string.Empty)
                {
                    resultedTable.Columns.Add(DatabaseObjects.Columns.TicketDueIn, typeof(int));
                }
            }
            if (!resultedTable.Columns.Contains(DatabaseObjects.Columns.SelfAssign))
            {
                //DataTable columns = uGITCache.ModuleConfigCache.LoadModuleListByName(ModuleName, DatabaseObjects.Tables.ModuleColumns); // uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
                // Check if we have a column by SelfAssign in the ModulesColumns table for the current module
                DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == ModuleName
                    // && x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True"
                    && x.Field<string>(DatabaseObjects.Columns.FieldName) == DatabaseObjects.Columns.SelfAssign).ToArray(); //.OrderBy(x => x.Field<double>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                if (selectedColumns != null && selectedColumns.Count() > 0)
                {
                    resultedTable.Columns.Add(DatabaseObjects.Columns.SelfAssign, typeof(string));
                }
            }

            /*
            if (!resultedTable.Columns.Contains("CRMAllocationCount"))
            {
                //DataTable columns = uGITCache.ModuleConfigCache.LoadModuleListByName(ModuleName, DatabaseObjects.Tables.ModuleColumns); // uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
                // Check if we have a column by SelfAssign in the ModulesColumns table for the current module
                //DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == ModuleName
                // && x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True"
                //&& x.Field<string>(DatabaseObjects.Columns.FieldName) == "CRMAllocationCount").ToArray(); //.OrderBy(x => x.Field<double>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                if (ModuleName == "OPM" || ModuleName == "CPR" || ModuleName == "CNS" || IsShowResourceAllocationBtn)
                {
                    resultedTable.Columns.Add("CRMAllocationCount", typeof(string));
                }
            }
            */
            AddProjectTeamAgent(resultedTable, ModuleName);
            AddDateModificationAgent(resultedTable, ModuleName);
            if (moduleRow == null)
            {
                int moduleId = 0;
                int.TryParse(this.ModuleName, out moduleId);
                DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(ModuleName, true));
                if (moduledt.Rows.Count > 0)
                    moduleRow = moduledt.Rows[0]; //uGITCache.GetDataTable(DatabaseObjects.Lists.Modules).AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == this.ModuleName || x.Field<int>(DatabaseObjects.Columns.Id) == moduleId);

            }

            //Use this to add column for monitor in case of PMM 
            if (ModuleName.Trim().ToLower() == "pmm" && moduleRow != null)
            {
                if (moduleMonitorsTable == null)
                {
                    //Load All Monitor in case of pmm projects
                    projectMonitorsStateTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'"); //uGITCache.LoadTable(DatabaseObjects.Tables.ProjectMonitorState);
                    moduleMonitorsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitors, $"{DatabaseObjects.Columns.ModuleNameLookup}='{ModuleName}' and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'").Select(); //uGITCache.GetDataTable(DatabaseObjects.Tables.ModuleMonitors, string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleName));
                }
                if (moduleMonitorsTable != null && moduleMonitorsTable.Length > 0)
                {
                    moduleMonitorOptions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitorOptions, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                    foreach (DataRow monitor in moduleMonitorsTable)
                    {
                        string colName = string.Empty;
                        long colID = 0;
                        if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.MonitorName) && !resultedTable.Columns.Contains(UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString()))
                        {
                            colName = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString();
                            colID = UGITUtility.StringToLong(UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.ID).ToString());
                            resultedTable.Columns.Add(colName);
                        }

                        foreach (DataRow dr in resultedTable.Rows)
                        {
                            DataRow drData = projectMonitorsStateTable.Select(string.Format("{0} = '{1}' and {2} = '{3}'", DatabaseObjects.Columns.TicketId, Convert.ToString(dr[DatabaseObjects.Columns.TicketId]), DatabaseObjects.Columns.ModuleMonitorNameLookup, colID)).FirstOrDefault();
                            if (drData != null)
                            {
                                DataRow drOption = moduleMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ID) == UGITUtility.StringToLong(drData[DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                                dr[colName] = Convert.ToString(drOption[DatabaseObjects.Columns.ModuleMonitorOptionLEDClass]).Replace("LED", string.Empty);
                            }
                        }
                    }
                }
            }

            //Use this to fill default values in the newly added columns,
            if (moduleColumns.Count == 0)
            {
                LoadModuleColumns();
            }
            if (moduleColumns.Count > 0)
            {
                //ModuleColumn[] multiuserColumns = moduleColumns.Where(x => x.ColumnType == "MultiUser" || x.ColumnType == "MultiLookup").ToArray();

                //if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketDueIn) || multiuserColumns.Length > 0)
                //{
                //    foreach (DataRow row in resultedTable.Rows)
                //    {
                //        if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketDueIn) && resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketDesiredCompletionDate) && Convert.ToString(row[DatabaseObjects.Columns.TicketDesiredCompletionDate]) != string.Empty)
                //            row[DatabaseObjects.Columns.TicketDueIn] = (UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.TicketDesiredCompletionDate]) - DateTime.Now).Days;

                //        if (moduleColumns.Count > 0)
                //        {
                //            foreach (ModuleColumn moduleColumn in multiuserColumns)
                //            {
                //                if (resultedTable.Columns.Contains(moduleColumn.FieldName))
                //                    row[moduleColumn.FieldName] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[moduleColumn.FieldName]));
                //            }
                //        }
                //    }
                //}
            }
            else if (IsDashboard)
            {
                // Needed because in dashboard case we don't use ModuleColumns, yet need to handle multi-user values in Owner field
                foreach (DataRow row in resultedTable.Rows)
                {
                    if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                        row[DatabaseObjects.Columns.TicketOwner] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[DatabaseObjects.Columns.TicketOwner]));
                }
            }


            if (ModuleName.Trim().ToLower() == "cmdb" && resultedTable != null && !resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketId))
            {
                resultedTable.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string), string.Format("[{0}]", DatabaseObjects.Columns.AssetTagNum));
            }
            #endregion

            #region Custom Sorting
            DataView ticketsView = resultedTable.DefaultView;

            if (!string.IsNullOrEmpty(SortString))
            {
                ticketsView.Sort = SortString;
            }
            else
            {
                string sortString = string.Empty;

                if (IsPreview || ModuleName.Trim() == string.Empty)
                {
                    if (resultedTable.Columns.Contains(DatabaseObjects.Columns.Modified))
                    {
                        sortString = string.Format("{0} DESC", DatabaseObjects.Columns.Modified);
                    }
                    else if (resultedTable.Columns.Contains(DatabaseObjects.Columns.Id))
                    {
                        sortString = "ID DESC";
                    }
                }
                //Commented by Inderjeet Kaur for BTS-24-001484
                //else if (tabType.Value.Trim().Equals("allclosedtickets", StringComparison.CurrentCultureIgnoreCase)) // Closed tickets tab always sorted by TicketID (latest first)
                //{
                //    if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketId))
                //    {
                //        sortString = "TicketID DESC";
                //    }
                //}

                #region Sorting Based on ModuleColumns
                else
                {
                    if (moduleColumns != null && moduleColumns.Count > 0)
                    {
                        ModuleColumn[] dtModuleColRow = moduleColumns.Where(x => x.SortOrder > 0).OrderBy(x => x.SortOrder).ToArray();
                        //string moduleColExpression = string.Format("{0} IS NOT NULL OR  {0} <> '{1}'", DatabaseObjects.Columns.SortOrder, "0");
                        //string moduleColSortExpression = string.Format("{0} ASC", DatabaseObjects.Columns.SortOrder);

                        if (dtModuleColRow != null && dtModuleColRow.Length > 0)
                        {  //Sorting logic added by Inderjeet Kaur on 20/09/2022. This is same as in User Management page.
                            if (CategoryName == Utility.Constants.ResourceTab)
                            {
                                List<string> sortedQuery = new List<string>();
                                ModuleColumnManager columnManager = new ModuleColumnManager(_context);
                                List<ModuleColumn> sortedColumns = columnManager.Load(x => x.CategoryName == "ResourceTab" && x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
                                foreach (ModuleColumn column in sortedColumns)
                                {
                                    sortedQuery.Add(column.FieldName + " " + (column.IsAscending == true ? "ASC" : "DESC"));
                                }
                                sortString = string.Join(", ", sortedQuery);
                            }
                            else
                            {
                                List<string> ModuleColNameList = new List<string>();
                                foreach (ModuleColumn rowitem in dtModuleColRow)
                                {
                                    if (resultedTable.Columns.Contains(rowitem.FieldName))
                                    {
                                        string strexp = string.Empty;
                                        if (rowitem.IsAscending.HasValue && rowitem.IsAscending.Value == true)
                                            strexp = "ASC";
                                        else
                                            strexp = "DESC";
                                        ModuleColNameList.Add(rowitem.FieldName + " " + strexp);
                                    }
                                }
                                sortString = String.Join(",", ModuleColNameList.ToArray());
                            }
                        }
                        else
                        {
                            if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketId))
                            {
                                sortString = string.Format("{0} DESC", DatabaseObjects.Columns.TicketId);
                            }
                        }
                    }
                    else
                    {
                        if (resultedTable.Columns.Contains(DatabaseObjects.Columns.Modified))
                        {
                            sortString = string.Format("{0} DESC", DatabaseObjects.Columns.Modified);
                        }
                        else if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketId))
                        {
                            sortString = "TicketID DESC";
                        }
                    }
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(sortString))
                    ticketsView.Sort = sortString;
            }

            resultedTable = ticketsView.ToTable();
            resultedTable.AcceptChanges();
            #endregion
        }

        private void AddDateModificationAgent(DataTable resultedTable, string moduleName)
        {
            if (!resultedTable.Columns.Contains(Constants.CRMSummary))
            {
                
                    resultedTable.Columns.Add(Constants.CRMSummary, typeof(string));
                
            }
        }

        private void AddProjectTeamAgent(DataTable resultedTable, string moduleName)
        {
            if (!resultedTable.Columns.Contains(Constants.CRMAllocationCount))
            {
                if (ModuleName == ModuleNames.OPM || ModuleName == ModuleNames.CPR || ModuleName == ModuleNames.CNS || IsShowResourceAllocationBtn)
                {
                    resultedTable.Columns.Add(Constants.CRMAllocationCount, typeof(string));
                }
            }
        }

        private void LoadFilteredTickets()
        {
            ModuleStatistics moduleStatistics = new ModuleStatistics(_context);
            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            //mRequest.SPWebObj = "";// SPContext.Current.Web;
            mRequest.UserID = User.Id;
            mRequest.Tabs = new List<string>();

            if (cModuleFilteredLinksPanel.Visible)
            {
                foreach (Tab t in filterTab.Tabs)
                {
                    mRequest.Tabs.Add(t.Name);
                }
            }

            if (!string.IsNullOrWhiteSpace(UserType))
                UserType = UserType.Trim().ToLower();


            if (cModuleFilteredLinksPanel.Visible == true && ModuleName != string.Empty && !IsPostBack)
            {
                if (UGITUtility.GetCookie(Request, ModuleName + "_tabChoice") != null)
                {
                    UserType = UGITUtility.GetCookieValue(Request, ModuleName + "_tabChoice");
                }

                try
                {
                    HttpCookie cookie = UGITUtility.GetCookie(Request, ModuleName + "_MTicketStatus");
                    if (cookie != null)
                        MTicketStatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), cookie.Value);
                    else
                    {
                        if (Request.QueryString["isClosed"] != null && Request.QueryString["isClosed"] == "true")
                        {
                            MTicketStatus = TicketStatus.Closed;
                        }
                        else
                        {
                            MTicketStatus = TicketStatus.Open;
                        }
                        if (!string.IsNullOrEmpty(Request["status"]))
                        {
                            switch (Request["status"].ToString())
                            {
                                case "myopentickets":
                                    MTicketStatus = TicketStatus.Open;
                                    UserType = "my";
                                    break;
                                case "mygrouptickets":
                                    MTicketStatus = TicketStatus.Open;
                                    break;
                                case "allclosedtickets":
                                    MTicketStatus = TicketStatus.Closed;
                                    break;
                                case "allopentickets":
                                    MTicketStatus = TicketStatus.Open;
                                    break;
                                case "waitingonme":
                                    MTicketStatus = TicketStatus.WaitingOnMe;
                                    break;
                                case "unassigned":
                                    MTicketStatus = TicketStatus.Unassigned;
                                    break;
                                case "alltickets":
                                    MTicketStatus = TicketStatus.All;
                                    break;
                                case "allresolvedtickets":
                                    MTicketStatus = TicketStatus.Approved;
                                    break;
                                case "myclosedtickets":
                                    MTicketStatus = TicketStatus.Closed;
                                    break;
                                case "onhold":
                                    MTicketStatus = TicketStatus.OnHold;
                                    break;
                                default:
                                    MTicketStatus = TicketStatus.Open;
                                    break;
                            }
                        }
                    }

                }
                catch
                {
                    MTicketStatus = TicketStatus.Open;
                }

                if (tabType.Value == string.Empty)
                {
                    if (UserType == "my" && MTicketStatus == TicketStatus.Open)
                    {
                        tabType.Value = "myopentickets";
                    }

                    else if (UserType == "mygroup" && MTicketStatus == TicketStatus.Open)
                    {
                        tabType.Value = "mygrouptickets";
                    }
                    else if (UserType == "all" && MTicketStatus == TicketStatus.Open)
                    {
                        tabType.Value = "allopentickets";
                    }
                    else if (UserType == "all" && MTicketStatus == TicketStatus.Closed)
                    {
                        tabType.Value = "allclosedtickets";
                    }
                    else if (UserType == "all" && MTicketStatus == TicketStatus.WaitingOnMe)
                    {
                        tabType.Value = "waitingonme";
                    }
                    else if (UserType == "all" && MTicketStatus == TicketStatus.Unassigned)
                    {
                        tabType.Value = "unassigned";
                    }
                    else if (UserType == "all" && MTicketStatus == TicketStatus.All)
                    {
                        tabType.Value = "alltickets";
                    }
                    else if (UserType == "all" && MTicketStatus == TicketStatus.Department)
                    {
                        tabType.Value = "departmentticket";
                    }
                    else if (UserType == "all" && MTicketStatus == TicketStatus.Approved)
                    {
                        tabType.Value = "allresolvedtickets";
                    }
                    else if (UserType == "my" && MTicketStatus == TicketStatus.Closed)
                    {
                        tabType.Value = "myclosedtickets";
                    }
                    //Added by mudassir 17 march 2020  SPDelta 15(Support for separate "On Hold" tab in ticket lists)
                    else if (UserType == "all" && MTicketStatus == TicketStatus.OnHold)
                    {
                        tabType.Value = "onhold";
                    }
                    else if (tabType.Value.Trim().Equals("onhold", StringComparison.CurrentCultureIgnoreCase))
                    {
                        UserType = "all";
                        MTicketStatus = TicketStatus.OnHold;

                        mRequest.CurrentTab = FilterTab.OnHold;
                    }
                    //
                }
            }



            if (tabType.Value.Trim().Equals("myopentickets", StringComparison.CurrentCultureIgnoreCase))
            {
                //if (UserType.Equals("my", StringComparison.CurrentCultureIgnoreCase))
                //{
                UserType = "my";
                //}
                MTicketStatus = TicketStatus.Open;
                mRequest.CurrentTab = FilterTab.myopentickets;

            }
            if (tabType.Value.Trim().Equals("mygrouptickets", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "mygroup";
                MTicketStatus = TicketStatus.Open;
                mRequest.CurrentTab = FilterTab.mygrouptickets;
            }
            else if (tabType.Value.Trim().Equals("allopentickets", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "all";
                MTicketStatus = TicketStatus.Open;
                mRequest.CurrentTab = FilterTab.allopentickets;
            }
            else if (tabType.Value.Trim().Equals("allclosedtickets", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "all";
                MTicketStatus = TicketStatus.Closed;
                mRequest.CurrentTab = FilterTab.allclosedtickets;
            }
            else if (tabType.Value.Trim().Equals("waitingonme", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "all";
                MTicketStatus = TicketStatus.WaitingOnMe;
                mRequest.CurrentTab = FilterTab.waitingonme;

            }
            else if (tabType.Value.Trim().Equals("unassigned", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "all";
                MTicketStatus = TicketStatus.Unassigned;
                mRequest.CurrentTab = FilterTab.unassigned;

            }
            else if (tabType.Value.Trim().Equals("alltickets", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "all";
                MTicketStatus = TicketStatus.All;
                mRequest.CurrentTab = FilterTab.alltickets;
            }
            else if (tabType.Value.Trim().Equals("departmentticket", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "my";
                MTicketStatus = TicketStatus.Department;
                mRequest.CurrentTab = FilterTab.departmentticket;
            }
            else if (tabType.Value.Trim().Equals("allresolvedtickets", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "all";
                MTicketStatus = TicketStatus.Approved;
                mRequest.CurrentTab = FilterTab.allresolvedtickets;
            }
            else if (tabType.Value.Trim().Equals("myclosedtickets", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "my";
                MTicketStatus = TicketStatus.Closed;

                mRequest.CurrentTab = FilterTab.myclosedtickets;
            }
            else if (tabType.Value.Trim().Equals("onhold", StringComparison.CurrentCultureIgnoreCase))
            {
                UserType = "all";
                MTicketStatus = TicketStatus.OnHold;

                mRequest.CurrentTab = FilterTab.OnHold;
            }

            if (IsPostBack && ModuleName != string.Empty && cModuleFilteredLinksPanel.Visible == true)
            {
                UGITUtility.CreateCookie(Response, ModuleName + "_tabChoice", UserType);
                UGITUtility.CreateCookie(Response, ModuleName + "_MTicketStatus", MTicketStatus.ToString());
            }


            if (UserType == "my" && MTicketStatus == TicketStatus.Open)
            {
                mRequest.CurrentTab = FilterTab.myopentickets;
            }
            else if (UserType == "mygroup" && MTicketStatus == TicketStatus.Open)
            {
                mRequest.CurrentTab = FilterTab.mygrouptickets;
            }
            else if (UserType == "all" && MTicketStatus == TicketStatus.Open)
            {
                mRequest.CurrentTab = FilterTab.allopentickets;
            }
            else if (UserType == "all" && MTicketStatus == TicketStatus.Closed)
            {
                mRequest.CurrentTab = FilterTab.allclosedtickets;
            }
            else if (UserType == "all" && MTicketStatus == TicketStatus.WaitingOnMe)
            {
                mRequest.CurrentTab = FilterTab.waitingonme;
            }
            else if (UserType == "all" && MTicketStatus == TicketStatus.Unassigned)
            {
                mRequest.CurrentTab = FilterTab.unassigned;
            }
            else if (UserType == "all" && MTicketStatus == TicketStatus.All)
            {
                mRequest.CurrentTab = FilterTab.alltickets;
            }
            else if (MTicketStatus == TicketStatus.Department)
            {
                mRequest.CurrentTab = FilterTab.departmentticket;
            }
            else if (UserType == "all" && MTicketStatus == TicketStatus.Approved)
            {
                mRequest.CurrentTab = FilterTab.allresolvedtickets;
            }
            else if (UserType == "my" && MTicketStatus == TicketStatus.Closed)
            {
                mRequest.CurrentTab = FilterTab.myclosedtickets;
            }

            if (mRequest.CurrentTab != null && (!mRequest.Tabs.Contains(mRequest.CurrentTab)) && mRequest.Tabs.Count > 0)
            {
                mRequest.CurrentTab = mRequest.Tabs.First();
            }

            if (ModuleName == string.Empty)
            {
                // Home page case 
                cModuleGlobalFilterPanel.Visible = !HideGlobalSearch;
                List<ModuleStatisticResponse> stats = null;

                if (MTicketStatus == TicketStatus.WaitingOnMe)
                {
                    mRequest.CurrentTab = FilterTab.waitingonme;
                    mRequest.ModuleType = ModuleType.SMS;
                    stats = moduleStatistics.LoadAll(mRequest);
                }
                else if (MTicketStatus == TicketStatus.Department)
                {
                    mRequest.CurrentTab = FilterTab.departmentticket;
                    mRequest.ModuleType = ModuleType.SMS;
                    stats = moduleStatistics.LoadAll(mRequest);
                }
                else if (MTicketStatus == TicketStatus.MyProject)
                {
                    mRequest.CurrentTab = FilterTab.myopentickets;
                    mRequest.ModuleType = ModuleType.Project;
                    stats = moduleStatistics.LoadAll(mRequest);
                }

                else if (Request["isGlobalSearch"] == "true")
                {
                    mRequest.CurrentTab = FilterTab.alltickets;
                    mRequest.ModuleType = ModuleType.SMS;
                    mRequest.IsGlobalSearch = true;
                    stats = moduleStatistics.LoadAll(mRequest);
                }
                else
                {
                    if (UserType != "" && UserType != "all" && UserType != "my" && UserType != "mygroup")
                    {
                        resultedTable = moduleStatistics.GetMyOpenTicketDataByRole(User, UserType);
                    }
                    else
                    {
                        mRequest.ModuleType = ModuleType.SMS;
                        stats = moduleStatistics.LoadAll(mRequest);
                    }
                }
                if (stats == null)
                    return;

                moduleStatResult = new ModuleStatisticResponse();


                List<string> cols = null;
                moduleCols = null;
                moduleCols = CacheHelper<object>.Get($"ModuleColumnslistview{_context.TenantID}") as List<ModuleColumn>;
                if (moduleCols == null)
                {
                    ModuleColumnManager columnManager = new ModuleColumnManager(_context);
                    cols = columnManager.Load(x => x.CategoryName.EqualsIgnoreCase("MyHomeTab") && x.IsDisplay == true).Select(x => x.FieldName).ToList();
                    CacheHelper<object>.AddOrUpdate($"ModuleColumnslistview{_context.TenantID}", columnManager.Load());
                }
                else
                {
                    cols = moduleCols.Where(x => x.CategoryName.EqualsIgnoreCase("MyHomeTab") && x.IsDisplay == true).Select(x => x.FieldName).ToList();
                }


                //ModuleColumnManager columnManager = new ModuleColumnManager(_context);
                //List<string> cols = columnManager.Load(x => x.CategoryName.EqualsIgnoreCase("MyHomeTab") && x.IsDisplay == true).Select(x => x.FieldName).ToList();

                DataTable mytable = new DataTable();
                foreach (ModuleStatisticResponse mStat in stats)
                {
                    if (mStat.ResultedData != null && uHelper.IsUserAuthorizedToViewModule(mStat.ModuleName))
                    {
                        if (!mStat.ResultedData.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                        {
                            mStat.ResultedData.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
                            mStat.ResultedData.Columns[DatabaseObjects.Columns.ModuleNameLookup].Expression = ""; //string.Format("'{0}'", mStat.ModuleName);
                        }
                        if (mStat.ResultedData.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                        {
                            if (mytable.Rows.Count <= 0)
                            {
                                mytable = mStat.ResultedData;

                                // **  Added code to show selected columns in Global search popup.
                                if (cols.Count > 0)
                                {
                                    foreach (var item in cols)
                                    {
                                        if (!UGITUtility.IfColumnExists(item, mytable))
                                        {
                                            mytable.Columns.Add(item, typeof(string));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                mytable.Merge(mStat.ResultedData, false, MissingSchemaAction.Ignore);
                            }
                            //mytable.Merge(mStat.ResultedData);
                        }

                        foreach (string tab in mRequest.Tabs)
                        {
                            if (mStat.TabCounts.ContainsKey(tab))
                                moduleStatResult.TabCounts[tab] = mStat.TabCounts[tab];
                            else
                                moduleStatResult.TabCounts[tab] = 0;
                        }
                    }
                }
                #region New User creation which is normal user dont have any role
                var user = HttpContext.Current.CurrentUser();
                var userRoleList = UserManager.GetUserRoles(user.Id);//b
                if (userRoleList.Count == 0)
                {
                    // Added below condition, as Grid in UserHomePage is crashing, whe logged in with new User, & unable to find column RequestorUser
                    //string query = string.Format("{0}= '{1}' OR {2}= '{3}' OR {4}='{5}'", DatabaseObjects.Columns.CreatedByUser, user.Id, DatabaseObjects.Columns.TicketRequestor, user.Id, DatabaseObjects.Columns.TicketInitiator, user.Id, DatabaseObjects.Columns.TicketBusinessManager, user.Id, DatabaseObjects.Columns.TicketApplicationManager, user.Id);
                    string query = string.Empty;
                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.CreatedByUser, mytable))
                        query = string.Format("{0}= '{1}'", DatabaseObjects.Columns.CreatedByUser, user.Id);

                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRequestor, mytable))
                    {
                        if (!string.IsNullOrEmpty(query))
                            query = query + string.Format(" OR {0}= '{1}'", DatabaseObjects.Columns.TicketRequestor, user.Id);
                        else
                            query = string.Format("{0}= '{1}'", DatabaseObjects.Columns.TicketRequestor, user.Id);
                    }

                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketInitiator, mytable))
                    {
                        if (!string.IsNullOrEmpty(query))
                            query = query + string.Format(" OR {0}= '{1}'", DatabaseObjects.Columns.TicketInitiator, user.Id);
                        else
                            query = string.Format("{0}= '{1}'", DatabaseObjects.Columns.TicketInitiator, user.Id);
                    }

                    if (mytable != null && mytable.Rows.Count > 0)
                    {
                        DataRow[] dataRows = mytable.Select(query);
                        if (dataRows.Count() > 0)
                        {
                            mytable = dataRows.CopyToDataTable();
                            resultedTable = mytable;
                        }
                        else
                        {
                            resultedTable = null;
                        }
                    }
                }

                #endregion
                else
                {
                    resultedTable = mytable;
                }

                enableTicketListExport = false;
            }
            else
            {

                // Filter ticket page case 
                mRequest.ModuleName = ModuleName;
                moduleStatResult = moduleStatistics.Load(mRequest, true);
                lblcustomfilterselectedvalue.Text = mRequest.CurrentTab;
                if ((UserType == "my" && string.IsNullOrEmpty(Request["UserType"])) && moduleStatResult.CurrentTabCount == 0 && (tabType.Value == "myopentickets" || tabType.Value == ""))
                {
                    tabType.Value = "allopentickets";
                    UserType = "all";
                    MTicketStatus = TicketStatus.Open;
                    mRequest.CurrentTab = FilterTab.allopentickets;
                }

                if (moduleStatResult.ResultedData != null && !moduleStatResult.ResultedData.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                {
                    moduleStatResult.ResultedData.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
                    moduleStatResult.ResultedData.Columns[DatabaseObjects.Columns.ModuleNameLookup].Expression = "'" + ModuleName + "'";
                }
                if (ModuleName == "TSR" && tabType.Value == FilterTab.unassigned)
                {
                    moduleStatResult.ResultedData.Columns.Add(DatabaseObjects.Columns.SelfAssign, typeof(string));
                }
                //var filteredRows = moduleStatResult.ResultedData.AsEnumerable().Where(x => x.Field<string>("TenantID") != null && x.Field<string>("TenantID").Equals(TenantID, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                //if (filteredRows != null && filteredRows.Length > 0)
                //{
                //    resultedTable = filteredRows.CopyToDataTable();
                //}
                resultedTable = moduleStatResult.ResultedData;
                //DataView dv = moduleStatResult.ResultedData.DefaultView;
                //dv.Sort = "StageActionUsersUser asc";
                //resultedTable = dv.ToTable();
                enableTicketListExport = true;
            }
        }

        /// <summary>
        /// Bind datacolumn to spgridview
        /// </summary>
        private void GenerateColumns()
        {
            Match match = null;
            grid.Columns.Clear();
            if ((hdnFilter.Contains("UserType") && Convert.ToString(hdnFilter["UserType"]) != UserType) || (hdnFilter.Contains("MTicketStatus")
                && Convert.ToString(hdnFilter["MTicketStatus"]) != Convert.ToString(MTicketStatus)))
            {
                //Get list for filter columns
                if (!string.IsNullOrEmpty(grid.FilterExpression))
                {
                    Regex pattern = new Regex(@"(?<=\[)(.*?)(?=\])");
                    match = pattern.Match(grid.FilterExpression);
                }
                grid.Columns.Clear();
            }

            hdnFilter.Set("UserType", UserType);
            hdnFilter.Set("MTicketStatus", MTicketStatus.ToString());

            if (grid.Columns.Count == 0)
            {
                // added new column checkbox.
                GridViewCommandColumn dataTextColumn = new GridViewCommandColumn();
                dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                dataTextColumn.HeaderStyle.CssClass += " jqtooltip";
                dataTextColumn.Caption = " ";
                dataTextColumn.ShowSelectCheckbox = true;
                dataTextColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
                dataTextColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                dataTextColumn.HeaderStyle.Font.Bold = true;
                dataTextColumn.Width = GetColumnWidth("CheckBox");
                dataTextColumn.VisibleIndex = 0;
                //dataTextColumn.CellStyle.CssClass += " bottomBorderZero";
                grid.Columns.Add(dataTextColumn);

                #region "Generate Columns"

                DataRow[] selectedColumns = new DataRow[0];
                DataTable table = null;
                //DataTable table = ConvertListToDataTable(moduleColumns.OrderBy(x => x.FieldSequence).ToList());
                if (!string.IsNullOrEmpty(Request["dID"]))
                {
                    List<ModuleColumn> _cols = new List<ModuleColumn>();
                    table = UGITUtility.ToDataTable(moduleColumns.OrderBy(x => x.FieldSequence).ToList());
                }
                else
                {
                    table = UGITUtility.ToDataTable(moduleColumns.OrderBy(x => x.FieldSequence).ToList());
                }
                DataRow[] drCollModuleColumns = table.Select();
                bool isColSelectedTabsExist = (drCollModuleColumns != null && drCollModuleColumns.Length > 0) ? UGITUtility.IfColumnExists(DatabaseObjects.Columns.selectedTabs, drCollModuleColumns.CopyToDataTable()) : false;

                if (MTicketStatus == TicketStatus.Closed)
                {
                    if (isColSelectedTabsExist)
                    {
                        // 1. Check if the module column has values in SelectedTabs, if yes then check if the value is either "all" or contains "allclosedtickets"
                        // 2. Second check is if the SelectedTabs is null or empty then check if DisplayForClosed is 1 for the mdoule column
                        selectedColumns = drCollModuleColumns.Where(x => (!string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                                && (x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Contains("allclosedtickets")
                                    || x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Equals("all")))
                                    || (string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                                        && x.Field<string>(DatabaseObjects.Columns.DisplayForClosed) == "True")).ToArray();             //.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.FieldSequence))
                    }
                    else
                    {
                        selectedColumns = drCollModuleColumns.Where(x => x.Field<string>(DatabaseObjects.Columns.DisplayForClosed) == "True").ToArray();                //.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.FieldSequence))
                    }
                }
                else
                {
                    if (IsManagerView)
                    {
                        selectedColumns = drCollModuleColumns.Where(x => (!string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                               && (x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Equals("all")
                                   || x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Contains("all")))
                                   || (string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                                       && x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True")).ToArray();
                    }
                    else if (isColSelectedTabsExist)
                    {
                        // 1. Check if the module column has values in SelectedTabs, if yes then check if the value is either "all" or contains the activeTabName i.e. current tab name
                        // 2. Second check is if the SelectedTabs is null or empty then check if IsDisplay is 1 for the mdoule column
                        selectedColumns = drCollModuleColumns.Where(x => (!string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                                && (x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Equals("all")
                                    || x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Contains(tabType.Value)))
                                    || (string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                                        && x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True")).ToArray();                //.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.FieldSequence))
                    }
                    else
                    {
                        selectedColumns = drCollModuleColumns.Where(x => x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True").ToArray();               //.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.FieldSequence))
                    }
                }

                // check if filter column is not in displayed column list remove filter expression.
                if (match != null)
                {
                    foreach (Group gm in match.Groups)
                    {
                        DataRow[] dr = new DataRow[0];
                        dr = selectedColumns.Where(x => x.Field<string>(DatabaseObjects.Columns.FieldName) == gm.Value.ToString() && x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True"
                            && (MTicketStatus != TicketStatus.Closed || (UGITUtility.StringToBoolean(x.Field<string>(DatabaseObjects.Columns.DisplayForClosed))))).ToArray();

                        if (dr.Length == 0)
                        {
                            grid.FilterExpression = string.Empty;
                            grid.FilterEnabled = false;
                            break;
                        }
                    }
                }

                Dictionary<string, string> customProperties = new Dictionary<string, string>();
                StringBuilder filterDataFields = new StringBuilder();
                //moduleRequest = new Ticket(ModuleName);

                #region ShowInMobileCheck
                /* Temporarily commenting for conflict resolution & code merging.
                bool isMobileDevice = HttpContext.Current.Request.Browser.IsMobileDevice;

                if (isMobileDevice)
                {
                    selectedColumns = selectedColumns.Where(x => x.ShowInMobile).ToList();
                }
                */
                #endregion

                // Had to change type below to DataRow to fix issue.
                foreach (DataRow moduleColumn in selectedColumns)
                {
                    //1. check for column exist is resultedtable or not
                    //2. Check if closed tickets are being shown then only specified column will be shown.
                    bool textAlignmentValueExists = false;
                    HorizontalAlign alignment = HorizontalAlign.Center;

                    if (moduleColumn.Table.Columns.Contains(DatabaseObjects.Columns.TextAlignment) &&
                        !string.IsNullOrEmpty(Convert.ToString(moduleColumn[DatabaseObjects.Columns.TextAlignment])))
                    {
                        textAlignmentValueExists = true;
                        alignment = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), Convert.ToString(moduleColumn[DatabaseObjects.Columns.TextAlignment]));
                    }

                    string fieldName = Convert.ToString(moduleColumn["FieldName"]);

                    // Check if need to show in case of closed status
                    bool showForClosed = false;
                    if (isColSelectedTabsExist)
                    {
                        showForClosed = drCollModuleColumns.Any(x => x.Field<string>(DatabaseObjects.Columns.FieldName) == fieldName
                            && ((!string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs)) && (x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Contains("allclosedtickets") || x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Equals("all")))
                                || (string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs)) && x.Field<string>(DatabaseObjects.Columns.DisplayForClosed) == "True")));
                    }
                    else
                    {
                        showForClosed = drCollModuleColumns.Any(x => x.Field<string>(DatabaseObjects.Columns.FieldName) == fieldName && x.Field<string>(DatabaseObjects.Columns.DisplayForClosed) == "True");
                    }

                    if (resultedTable != null && (resultedTable.Columns.Contains(fieldName) || fieldName.ToLower() == "projectmonitors") && (MTicketStatus != TicketStatus.Closed || showForClosed))
                    {
                        Type dataType = typeof(string);
                        if (resultedTable.Columns.Contains(fieldName))
                            dataType = resultedTable.Columns[fieldName].DataType;

                        GridViewDataTextColumn colId = null;
                        GridViewDataDateColumn dateTimeColumn = null;

                        //DataRow[] drSortorder = table.Select($"{DatabaseObjects.Columns.SortOrder} = 1");
                        //if (drSortorder == null && drSortorder.Length == 0)
                        //    drSortorder = table.Select($"{DatabaseObjects.Columns.FieldName} = {DatabaseObjects.Columns.Title}");

                        if (fieldName.ToLower() == DatabaseObjects.Columns.TicketStatus.ToLower())
                        {
                            colId = new GridViewDataTextColumn();

                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = fieldName;
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketStatus);
                            colId.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.True;
                            grid.Columns.Add(colId);
                        }
                        else if (fieldName.ToLower() == DatabaseObjects.Columns.TicketAge.ToLower())
                        {
                            colId = new GridViewDataTextColumn();
                            colId.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = fieldName;
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketAge);
                            colId.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.True;
                            grid.Columns.Add(colId);
                        }
                        else if (fieldName.ToLower() == DatabaseObjects.Columns.Attachments.ToLower())
                        {
                            colId = new GridViewDataTextColumn();
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                            colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                            colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = fieldName;
                            colId.Caption = string.Format("<img src='{0}'></img>", "/Content/images/attach.gif");
                            colId.HeaderStyle.Font.Bold = true;
                            colId.Width = GetColumnWidth(DatabaseObjects.Columns.Attachments);
                            grid.Columns.Add(colId);
                        }
                        else if (fieldName.ToLower() == "projectmonitors" && ModuleName.Trim().ToLower() == "pmm")
                        {
                            string headerText = string.Empty;
                            if (moduleMonitorsTable != null && moduleMonitorsTable.Length > 0)
                            {
                                for (int mi = 0; mi < moduleMonitorsTable.Length; mi++)
                                {
                                    DataRow monitor = moduleMonitorsTable[mi];
                                    if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.Title))
                                    {
                                        colId = new GridViewDataTextColumn();
                                        colId.PropertiesTextEdit.EncodeHtml = false;
                                        colId.FieldName = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString();
                                        colId.Name = string.Format("projecthealth{0}", mi);
                                        colId.Caption = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.Title).ToString(); // "Issues Scope $$$$ OnTime Risk";  // 5 monitors: Issues, Scope, Budget, Time, Risk
                                        colId.Width = GetColumnWidth("ProjectHealth");
                                        colId.CellStyle.HorizontalAlign = textAlignmentValueExists ? alignment : HorizontalAlign.Center;
                                        colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                                        colId.HeaderStyle.Font.Bold = true;
                                        colId.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.True;
                                        grid.Columns.Add(colId);

                                    }
                                }
                            }
                        }
                        else if (fieldName.ToLower() == Constants.CRMSummary.ToLower())
                        {
                            GridViewDataTextColumn dateModificationAgent = new GridViewDataTextColumn();
                            dateModificationAgent.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            dateModificationAgent.HeaderStyle.HorizontalAlign = HorizontalAlign.NotSet;
                            dateModificationAgent.Width = Unit.Pixel(60);
                            dateModificationAgent.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            dateModificationAgent.FieldName = Constants.CRMSummary;
                            dateModificationAgent.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            dateModificationAgent.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                            dateModificationAgent.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            dateModificationAgent.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                            grid.Columns.Add(dateModificationAgent);
                        }
                        else if (fieldName.ToLower() == Constants.CRMAllocationCount.ToLower())
                        {
                            GridViewDataTextColumn ResourceEdit = new GridViewDataTextColumn();
                            ResourceEdit.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            ResourceEdit.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                            ResourceEdit.Width = Unit.Pixel(60);
                            ResourceEdit.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            ResourceEdit.FieldName = Constants.CRMAllocationCount;
                            ResourceEdit.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            ResourceEdit.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                            ResourceEdit.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                            ResourceEdit.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                            grid.Columns.Add(ResourceEdit);
                        }
                        else if (dataType == typeof(DateTime))
                        {
                            dateTimeColumn = new GridViewDataDateColumn();
                            dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                            dateTimeColumn.FieldName = fieldName;
                            dateTimeColumn.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                            if (moduleRequest != null && moduleRequest.Module != null)
                            {
                                List<ModuleColumn> lstModuleColumn = moduleRequest.Module.List_ModuleColumns;
                                ModuleColumn currentModuleColumn = lstModuleColumn.FirstOrDefault(x => x.FieldName == fieldName);
                                if (currentModuleColumn != null && currentModuleColumn.ColumnType == "DateTime")
                                    dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy hh:mm:ss tt}";
                            }
                            dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                            dateTimeColumn.HeaderStyle.Font.Bold = true;
                            dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                            dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            dateTimeColumn.Width = GetColumnWidth("DateTime");
                            dateTimeColumn.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.True;
                            dateTimeColumn.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;

                            //if (drSortorder != null && drSortorder.Length > 0)
                            //{
                            //    if (fieldName.EqualsIgnoreCase(Convert.ToString(drSortorder[0][DatabaseObjects.Columns.FieldName])))
                            //        colId.SortOrder = UGITUtility.StringToBoolean(drSortorder[0][DatabaseObjects.Columns.IsAscending]) == true ? DevExpress.Data.ColumnSortOrder.Ascending : DevExpress.Data.ColumnSortOrder.Descending;
                            //}

                            grid.Columns.Add(dateTimeColumn);
                        }                        
                        else
                        {
                            colId = new GridViewDataTextColumn();
                            colId.FieldName = fieldName;
                            colId.Caption = Convert.ToString(moduleColumn["FieldDisplayName"]);
                            colId.Settings.FilterMode = ColumnFilterMode.DisplayText;
                            colId.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                            if (fieldName.ToLower() == "CRMCompanyLookup".ToLower())
                            {
                                colId.FieldName = "CRMCompanyTitle";
                            }
                            if (fieldName.ToLower() == DatabaseObjects.Columns.Title.ToLower())
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                                //colId.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                            }
                            else if (fieldName.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketId);
                                colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                            }
                            else if (fieldName.ToLower() == DatabaseObjects.Columns.TicketPctComplete.ToLower())
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketPctComplete);
                                colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                            }
                            else
                            {
                                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                                if (fieldName.ToLower() == DatabaseObjects.Columns.TicketPriority.ToLower() || fieldName.ToLower() == DatabaseObjects.Columns.ERPJobID.ToLower() ||
                                    fieldName.ToLower() == DatabaseObjects.Columns.TicketPriorityLookup.ToLower())
                                {
                                    colId.Width = GetColumnWidth(fieldName);

                                }
                            }

                            if (fieldName.ToLower() == "selfassign")
                            {
                                colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                colId.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                                colId.PropertiesTextEdit.EncodeHtml = false;
                                colId.Caption = string.Format("<img src='{0}'></img>", "/Content/ButtonImages/self-assign.png");
                                colId.Width = GetColumnWidth("SelfAssign");
                            }
                            else
                            {
                                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                            }

                            if (dataType == typeof(double) || dataType == typeof(float) || dataType == typeof(decimal))
                            {
                                colId.PropertiesEdit.DisplayFormatString = "#.##";
                                colId.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
                            }
                            if (dataType == typeof(int))
                            {
                                colId.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
                            }

                            colId.HeaderStyle.Font.Bold = true;

                            if (textAlignmentValueExists)
                            {
                                colId.HeaderStyle.HorizontalAlign = alignment;
                                colId.CellStyle.HorizontalAlign = alignment;
                            }
                            colId.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.True;

                            //if (drSortorder != null && drSortorder.Length > 0)
                            //{
                            //    if (fieldName.EqualsIgnoreCase(Convert.ToString(drSortorder[0][DatabaseObjects.Columns.FieldName])))
                            //        colId.SortOrder = UGITUtility.StringToBoolean(drSortorder[0][DatabaseObjects.Columns.IsAscending]) == true ? DevExpress.Data.ColumnSortOrder.Ascending : DevExpress.Data.ColumnSortOrder.Descending;
                            //}

                            grid.Columns.Add(colId);
                        }
                    }
                }

                if (grid.Columns.Count <= 3 && resultedTable != null && resultedTable.Rows.Count > 0)
                {
                    grid.Columns.Clear();

                    // added new column checkbox.
                    GridViewCommandColumn dataTextColumnCmd = new GridViewCommandColumn();
                    dataTextColumnCmd.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                    dataTextColumnCmd.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    dataTextColumnCmd.Caption = " ";
                    dataTextColumnCmd.ShowSelectCheckbox = true;
                    dataTextColumnCmd.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                    dataTextColumnCmd.HeaderStyle.Font.Bold = true;
                    dataTextColumnCmd.Width = GetColumnWidth("CheckBox");
                    dataTextColumnCmd.VisibleIndex = 0;
                    grid.Columns.Add(dataTextColumnCmd);

                    List<ModuleColumn> lstModuleColumn = (moduleRequest != null && moduleRequest.Module != null) ? moduleRequest.Module.List_ModuleColumns : null;

                    GridViewDataTextColumn colId = null;
                    foreach (DataColumn column in resultedTable.Columns)
                    {
                        if (column.ColumnName.ToLower() != "id" && column.ColumnName.ToLower() != "created" && column.ColumnName.ToLower() != "modified" && column.ColumnName.ToLower() != "today" && column.ColumnName.ToLower() != "me" &&
                            column.ColumnName != DatabaseObjects.Columns.ModuleNameLookup)
                        {
                            if (column.DataType == typeof(DateTime))
                            {
                                ModuleColumn currentModuleColumn = lstModuleColumn != null ? lstModuleColumn.FirstOrDefault(x => x.FieldName == column.ColumnName) : null;

                                GridViewDataDateColumn dateTimeColumn = new GridViewDataDateColumn();
                                dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                dateTimeColumn.FieldName = column.ColumnName;
                                dateTimeColumn.Caption = UGITUtility.AddSpaceBeforeWord(column.ColumnName);

                                if ((currentModuleColumn != null && currentModuleColumn.ColumnType == "DateTime") || ShowTimeToo)
                                    dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy hh:mm:ss tt}";
                                else
                                    dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";

                                dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                                dateTimeColumn.HeaderStyle.Font.Bold = true;
                                dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                                dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                                dateTimeColumn.Width = GetColumnWidth("DateTime");
                                dateTimeColumn.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.True;
                                grid.Columns.Add(dateTimeColumn);
                            }
                            else
                            {
                                colId = new GridViewDataTextColumn();
                                if (column.ColumnName == DatabaseObjects.Columns.Title)
                                {
                                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                                }
                                else
                                {
                                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                    colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                }

                                if (column.ColumnName == DatabaseObjects.Columns.TicketId)
                                {
                                    colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                                    colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketId);
                                }
                                else if (column.ColumnName == DatabaseObjects.Columns.TicketPriority || column.ColumnName == DatabaseObjects.Columns.TicketPriorityLookup)
                                {
                                    colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketPriorityLookup);
                                }
                                else if (column.ColumnName == DatabaseObjects.Columns.TicketStatus)
                                {
                                    colId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketStatus);
                                }

                                colId.PropertiesTextEdit.EncodeHtml = false;
                                colId.FieldName = column.ColumnName;
                                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                                colId.Caption = UGITUtility.AddSpaceBeforeWord(column.ColumnName);
                                colId.HeaderStyle.Font.Bold = true;

                                if (column.DataType == typeof(double) || column.DataType == typeof(float) || column.DataType == typeof(decimal))
                                    colId.PropertiesEdit.DisplayFormatString = "0.##";
                                colId.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.True;
                                grid.Columns.Add(colId);
                            }
                        }
                    }
                }
                
                SetCustomizationWindowColumns();

                // Binding Columns for CardView

                var CardColumns = moduleColumns.Where(x => x.ShowInCardView == true).ToList();
                CardView.Columns.Clear();
                CardView.CardLayoutProperties.Items.Clear();
                if (CardColumns != null && CardColumns.Count > 0)
                {
                    CardViewColumnLayoutItem lItem = null;
                    CardViewCommandLayoutItem item = new CardViewCommandLayoutItem();
                    item.ShowSelectCheckbox = false;
                    item.HorizontalAlign = FormLayoutHorizontalAlign.Left;
                    CardView.CardLayoutProperties.Items.AddCommandItem(item);

                    CardViewColumn col = null;
                    foreach (ModuleColumn moduleColumn in CardColumns)
                    {
                        col = new CardViewColumn();
                        col.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        col.FieldName = moduleColumn.FieldName;
                        col.Caption = moduleColumn.FieldDisplayName;
                        col.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                        col.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        col.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;

                        //if (resultedTable.Columns[col.FieldName].DataType == typeof(DateTime))
                        //    col.Settings.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;

                        CardView.Columns.Add(col);

                        lItem = new CardViewColumnLayoutItem();
                        lItem.ColumnName = moduleColumn.FieldName;
                        lItem.CssClass += "commonLayout";
                        CardView.CardLayoutProperties.Items.AddColumnItem(lItem);
                    }
                }
                else
                {
                    btnCardView.Visible = false;
                    CardView.Visible = false;
                }

                #endregion
            }
        }

        /// <summary>
        /// Method to add columns in Column Chooser
        /// </summary>
        private void SetCustomizationWindowColumns()
        {
            if (!string.IsNullOrEmpty(ModuleName) && UGITUtility.GetCookie(Request, ModuleName + "Columns") != null)
            {
                for (int i = 0; i < grid.DataColumns.Count; i++)
                {
                    if (HttpUtility.UrlDecode(UGITUtility.GetCookieValue(Request, ModuleName + "Columns")).Contains(grid.DataColumns[i].FieldName))
                        grid.DataColumns[i].Visible = false;
                }
            }
        }

        /// <summary>
        /// This method is used to show/hide the grid columns for the selected tab.
        /// Tab wise visibilty of column is configured in Request Lists on module level.
        /// </summary>
        protected void ShowHideColumnsForSelectedTab()
        {
            if (IsSLAMetricsDrilldown)
            {
                foreach (var gridColumn in grid.DataColumns)
                {
                    gridColumn.Width = GetColumnWidth(gridColumn.FieldName);
                    gridColumn.Visible = true;
                }
                return;
            }
            //Added by Inderjeet Kaur on 20/09/2022 to handle $ columns and change the KeyFieldName from TicketId to UserName
            if (!string.IsNullOrEmpty(Request["dID"]) && CategoryName == Utility.Constants.ResourceTab)
            {
                grid.KeyFieldName = DatabaseObjects.Columns.UserName;
            }
            if (ModuleName == "" && CategoryName == Constants.MyHomeTicketTab && !string.IsNullOrEmpty(Request["dashboardID"]))
                grid.KeyFieldName = DatabaseObjects.Columns.ID;

            DataTable table = ConvertListToDataTable(moduleColumns.OrderBy(x => x.FieldSequence).ToList());
            DataRow[] drCollModuleColumns = table.Select();

            // Exit if column SelectedTabs doesn't exist in moduleColumns that means modulesColumns data is coming from MyModuleColumns list.
            bool isColSelectedTabsExist = (drCollModuleColumns != null && drCollModuleColumns.Length > 0) ? UGITUtility.IfColumnExists(DatabaseObjects.Columns.selectedTabs, drCollModuleColumns.CopyToDataTable()) : false;

            if (!isColSelectedTabsExist)
                return;

            if (!string.IsNullOrEmpty(activeTab) && string.IsNullOrEmpty(previousTab))
                previousTab = activeTab;

            if (!string.IsNullOrEmpty(previousTab) && !string.IsNullOrEmpty(activeTab) && activeTab != previousTab)
            {
                previousTab = activeTab;
                grid.Columns.Clear();
                GenerateColumns();
            }

            DataRow[] selectedColumns = new DataRow[0];

            if (MTicketStatus == TicketStatus.Closed)
            {
                // 1. Check if the module column has values in SelectedTabs, if yes then check if the value is either "all" or contains "allclosedtickets"
                // 2. Second check is if the SelectedTabs is null or empty then check if DisplayForClosed is 1 for the mdoule column
                selectedColumns = drCollModuleColumns.Where(x => (!string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                        && (x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Contains("allclosedtickets")
                            || x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Equals("all")))
                            || (string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                                && x.Field<string>(DatabaseObjects.Columns.DisplayForClosed) == "True")).ToArray();             //.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.FieldSequence))
            }
            else
            {
                // 1. Check if the module column has values in SelectedTabs, if yes then check if the value is either "all" or contains the activeTabName i.e. current tab name
                // 2. Second check is if the SelectedTabs is null or empty then check if IsDisplay is 1 for the mdoule column
                selectedColumns = drCollModuleColumns.Where(x => (!string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                        && (x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Equals("all")
                            || x.Field<string>(DatabaseObjects.Columns.SelectedTabs).Contains(tabType.Value)))
                            || (string.IsNullOrEmpty(x.Field<string>(DatabaseObjects.Columns.SelectedTabs))
                                && x.Field<string>(DatabaseObjects.Columns.IsDisplay) == "True")).ToArray();                //.OrderBy(x => x.Field<string>(DatabaseObjects.Columns.FieldSequence))
            }

            // Show/hide grid columns
            // For project monitors
            bool hasProjectMonitor = false;
            if (moduleMonitorsTable != null && moduleMonitorsTable.Length > 0)
                hasProjectMonitor = selectedColumns.AsEnumerable().Any(x => Convert.ToString(x[DatabaseObjects.Columns.FieldName]).Equals("ProjectMonitors"));

            bool colExists = false;
            foreach (var gridColumn in grid.DataColumns)
            {
                if (gridColumn.FieldName.ToLower() == "CRMCompanyTitle".ToLower())
                    gridColumn.FieldName = DatabaseObjects.Columns.CRMCompanyLookup;

                colExists = selectedColumns.AsEnumerable().Any(x => Convert.ToString(x[DatabaseObjects.Columns.FieldName]).Equals(gridColumn.FieldName));
                //For project monitors Commented SPDelta 40 as projectmonitor column is not present
                if (!colExists && hasProjectMonitor)
                {
                    DataRow monitorrow = moduleMonitorsTable.AsEnumerable().FirstOrDefault(x => Convert.ToString(x[DatabaseObjects.Columns.MonitorName]).Equals(gridColumn.FieldName));
                    if (monitorrow != null)
                        colExists = true;
                }
                //Code added by Inderjeet Kaur on 28-09-2022 to fix drill down of charts when ModuleName =""
                //Display all the columns of the resultset in grid by setting visible = true 
                if (ModuleName == "" && CategoryName == Constants.MyHomeTicketTab && !string.IsNullOrEmpty(Request["dashboardID"]))
                    colExists = true;

                if (!string.IsNullOrEmpty(ModuleName) && UGITUtility.GetCookie(Request, ModuleName + "Columns") != null)
                {
                    if (HttpUtility.UrlDecode(UGITUtility.GetCookieValue(Request, ModuleName + "Columns")).Contains(gridColumn.FieldName))
                        gridColumn.Visible = false;
                    else
                        gridColumn.Visible = colExists;
                }
                else
                    gridColumn.Visible = colExists;
            }
            foreach (var cardColumn in CardView.VisibleColumns)
            {
                colExists = UGITUtility.IfColumnExists(cardColumn.FieldName, resultedTable); //selectedColumns.AsEnumerable().Any(x => Convert.ToString(x[DatabaseObjects.Columns.FieldName]).Equals(cardColumn.FieldName));
                cardColumn.Visible = colExists;
                moduleColumns.Where(x => x.FieldName.EqualsIgnoreCase(cardColumn.FieldName)).FirstOrDefault().ShowInCardView = colExists;
            }
        }

        private Unit GetColumnWidth(string columnName)
        {
            Unit width = new Unit("30px");

            if (columnName == DatabaseObjects.Columns.TicketPriority || columnName == DatabaseObjects.Columns.TicketPriorityLookup)
                width = new Unit("60px");
            else if (columnName == DatabaseObjects.Columns.TicketId)
                width = new Unit("105px");
            else if (columnName == "DateTime")
                width = new Unit("105px");
            else if (columnName == DatabaseObjects.Columns.TicketStatus)
                width = new Unit("125px");
            else if (columnName == DatabaseObjects.Columns.TicketAge)
                width = new Unit("50px");
            else if (columnName == DatabaseObjects.Columns.ProjectHealth || columnName == DatabaseObjects.Columns.ProjectRank)
                width = new Unit("30px");
            else if (columnName == DatabaseObjects.Columns.SelfAssign)
                width = new Unit("20px");
            else if (columnName == DatabaseObjects.Columns.Attachments)
                width = new Unit("8px");
            else if (columnName == DatabaseObjects.Columns.ERPJobID)
                width = new Unit("120px");
            else if (columnName == "CheckBox")
                width = new Unit("20px");
            // else use default width from above

            return width;
        }

        /// <summary>
        /// Apply Global filter
        /// </summary>
        private void GetGlobalFilteredData()
        {
            pGlobalFilters.CssClass = "globalfilter";
            pGlobalFiltersTable.Attributes.Add("class", "bordercolps");
            //allTicketLink.CssClass = "tickettypetab";

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MinValue;
            if (dtFrom.Text != string.Empty)
            {
                startDate = dtFrom.Date;
            }

            if (dtTo.Text != string.Empty)
            {
                endDate = dtTo.Date;
            }

            //if from date empty and to date is not empty then set to date in from
            if (dtFrom.Text == string.Empty && dtTo.Text != string.Empty)
            {
                startDate = dtTo.Date;
            }

            //if to date empty and from date is not empty then set from date in to
            if (dtFrom.Text != string.Empty && dtTo.Text == string.Empty)
            {
                endDate = dtFrom.Date;
            }

            string globalFilter = string.Empty;
            string fieldName = string.Empty;

            if (txtWildCard.Text.Trim() != string.Empty && !txtWildCard.Text.Equals(wildCardWaterMark, StringComparison.CurrentCultureIgnoreCase))
            {
                globalFilter = txtWildCard.Text.Trim();
            }
            else if (!IsPostBack && ddlModuleName.SelectedItem != null && ddlModuleName.SelectedItem.Text == "--All--")
            {
                string queryString = Request.QueryString["globalSearchString"];
                if (!string.IsNullOrEmpty(queryString) && !queryString.Equals(wildCardWaterMark, StringComparison.CurrentCultureIgnoreCase))
                {
                    txtWildCard.Text = globalFilter = queryString;
                    ddlModuleName.ClearSelection();
                    ListItem moduleItem = ddlModuleName.Items.FindByText(globalFilter.ToUpper());
                    if (moduleItem != null)
                    {
                        moduleItem.Selected = true;
                        txtWildCard.Text = wildCardWaterMark;
                        globalFilter = string.Empty;
                    }
                }
            }

            bool isModuleSelected = false;
            string moduleName = ModuleName;
            if (ddlModuleName.SelectedItem != null && ddlModuleName.SelectedItem.Text != "--All--")
            {
                moduleName = ddlModuleName.SelectedItem.Text;
                isModuleSelected = true;
            }

            if (!string.IsNullOrEmpty(globalFilter))
            {
                if (!globalFilter.Contains("*"))
                {
                    globalFilter = string.Format("*{0}*", globalFilter);
                }
                globalFilter = globalFilter.Replace("*", "%");
            }

            // if the field name is selected for search
            if (lstFilteredFields.SelectedItem != null && lstFilteredFields.SelectedItem.Text != "--All--")
            {
                // store the internal name of the field.
                fieldName = (lstFilteredFields.SelectedValue);
            }

            if (startDate != DateTime.MinValue || endDate != DateTime.MinValue || !string.IsNullOrEmpty(globalFilter) || isModuleSelected)
            {
                isWildCardFiltered = true;
                pGlobalFilters.CssClass = "globalfilter";
                pGlobalFiltersTable.Attributes.Add("class", "bordercolps");
                //allTicketLink.CssClass = "tickettypetab";
                if (resultedTable != null)
                    resultedTable = WildCardFilter(resultedTable, globalFilter, moduleName, startDate, endDate, fieldName);
            }
            else if (Request.QueryString["isGlobalSearch"] == "true")
            {
                // If called from global search, only show results when we have valid search parameters
                resultedTable = null;
            }
        }

        /// <summary>
        /// Helper function which apply global filter on resulted data
        /// </summary>
        /// <param name="table"></param>
        /// <param name="wildCard"></param>
        /// <param name="ModuleName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private DataTable WildCardFilter(DataTable table, string wildCard, string moduleName, DateTime startDate, DateTime endDate, string fieldName)
        {
            if (table == null)
                return null;

            try
            {
                //FieldConfigurationManager configFieldManager = new FieldConfigurationManager(_context);
                FieldConfiguration configField = null;
                DataRow[] localFilteredRows = table.Select();
                if (!string.IsNullOrEmpty(moduleName))
                {
                    if (table.Columns.Contains(DatabaseObjects.Columns.TicketId))
                        localFilteredRows = table.Select(DatabaseObjects.Columns.TicketId + " Like '" + moduleName + "%'");
                }

                if (!string.IsNullOrEmpty(wildCard))
                {
                    // DataTable columns =  GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns);  //uGITCache.ModuleConfigCache.LoadModuleListByName(ModuleName, DatabaseObjects.Tables.ModuleColumns);// uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
                    List<string> selectedColumns = new List<string>();

                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        selectedColumns = moduleColumns.Where(x => x.CategoryName == moduleName && x.IsUseInWildCard).Select(x => x.FieldName).Distinct().ToList();
                    }
                    else
                    {
                        selectedColumns = moduleColumns.Where(x => x.IsUseInWildCard == true).Select(x => x.FieldName).Distinct().ToList();
                    }

                    //To search in a field.
                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        selectedColumns = selectedColumns.Where(x => x == fieldName).ToList();
                    }

                    StringBuilder queryExpression = new StringBuilder();
                    if (selectedColumns.Count > 0)
                    {
                        for (int i = 0; i < selectedColumns.Count; i++)
                        {
                            string column = selectedColumns[i];
                            if (table.Columns.Contains(column))
                            {
                                configField = FieldConfigurationManager.GetFieldByFieldName(column);

                                if (configField == null)
                                {
                                    if (table.Columns[column].DataType == typeof(string))
                                    {
                                        if (queryExpression.Length > 0)
                                            queryExpression.Append(" or ");

                                        queryExpression.AppendFormat("{0} like '{1}'", column, wildCard);
                                    }
                                }
                                else
                                {
                                    string value = FieldConfigurationManager.GetFieldConfigurationIdByName(configField.FieldName, Convert.ToString(wildCard.Replace("%", "")));

                                    if (queryExpression.Length > 0 && !string.IsNullOrEmpty(value) && value != "''")
                                    {
                                        if (configField.Datatype.Equals("Date", StringComparison.InvariantCultureIgnoreCase) || configField.Datatype.Equals("DateTime", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                        }
                                        else if (configField.Datatype.Equals("Choices", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            queryExpression.Append(" or ");
                                            queryExpression.AppendFormat("{0} like {1}", column, value);
                                        }
                                        else
                                        {
                                            queryExpression.Append(" or ");
                                            queryExpression.AppendFormat("{0} in ({1})", column, value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Array.Clear(localFilteredRows, 0, localFilteredRows.Count());
                    }
                    //Fiter data by global string
                    if (localFilteredRows != null && localFilteredRows.Length > 0)
                    {
                        localFilteredRows = localFilteredRows.CopyToDataTable().Select(Convert.ToString(queryExpression));
                    }
                }

                //Fiter data between start and end date
                if (startDate.Date != DateTime.MinValue || endDate.Date != DateTime.MinValue)
                {
                    string dateColumnName = string.Empty;
                    ModuleColumn[] multiuserColumns = new ModuleColumn[0];
                    if (moduleColumns.Count > 0)
                    {
                        multiuserColumns = moduleColumns.Where(x => !string.IsNullOrWhiteSpace(x.CustomProperties) && (x.CustomProperties.Contains("fieldtype=datetime") || x.CustomProperties.Contains("useforglobaldatefilter=true") || x.CustomProperties.Contains("useforglobaldatefilter=1"))).ToArray();
                    }

                    if (multiuserColumns.Length > 0 && multiuserColumns[0].FieldName != null && table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                    {
                        dateColumnName = Convert.ToString(multiuserColumns[0].FieldName).Trim();
                    }
                    else if (table.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                    {
                        dateColumnName = DatabaseObjects.Columns.TicketCreationDate;
                    }
                    else if (table.Columns.Contains(DatabaseObjects.Columns.Created))
                    {
                        dateColumnName = DatabaseObjects.Columns.Created;
                    }

                    if (dateColumnName != string.Empty && localFilteredRows.Length > 0)
                    {
                        localFilteredRows = localFilteredRows.Where(x => x.IsNull(dateColumnName) != true && x.Field<DateTime>(dateColumnName) != null && x.Field<DateTime>(dateColumnName).ToString() != string.Empty && x.Field<DateTime>(dateColumnName).Date >= startDate.Date && x.Field<DateTime>(dateColumnName).Date <= endDate.Date).ToArray();
                    }
                }

                if (localFilteredRows != null && localFilteredRows.Length > 0)
                {
                    return localFilteredRows.CopyToDataTable();
                }
                else
                {
                    return table.Clone();
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
                return null;
            }
        }

        /// <summary>
        /// show selected tab and apply global filter effect in UI
        /// </summary>
        private void UISettingAfterFilter()
        {
            if (Page.IsPostBack)
                return;

            string selectedTab = string.Empty;

            if (HideReport)
            {
                imgReport.Visible = false;
            }

            if (showExportIcons)
            {
                iconContainer.Visible = true;
            }

            List<TabView> tabViewrows = new List<TabView>();
            if (!string.IsNullOrEmpty(ModuleName))
            {
                globalFilterDetialForPDF = string.Format("{0} {1}s: ", ModuleName.ToUpper(), UGITUtility.moduleTypeName(ModuleName));

                TabViewManager tabViewManager = new TabViewManager(_context);
                tabViewrows = tabViewManager.Load(x => x.ModuleNameLookup == ModuleName && x.ViewName==ViewName).ToList();
            }

            //Shows help icon if help url persent
            if (!string.IsNullOrEmpty(HelpUrl))
            {
                helpLinkContainer.Visible = true;
                btHelpLink.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}' , '', '{1}', '1000px', '90', 0, 0)", UGITUtility.GetAbsoluteURL(HelpUrl), "Help Documentation"));
                btHelpLink.HRef = "javascript:Void(0)";
            }

            //Shows Summary icon only in case of TSK and PMM
            //summaryUrl =  UGITUtility.GetAbsoluteURL(string.Format("/_layouts/15/ugovernit/taskSummary.aspx?moduleName={0}", ModuleName));
            summaryUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=tsksummaryselector&Module={0}", ModuleName));
            summaryPopupTitle = "Task Summary Report";

            showClearFilter = IsFilterOn(); //!IsPreview &&
            //left align new button in popup case
            if (IsInIframe)
            {
                mModuleActionsContainer.Style.Add("float", "left");
            }

            int count = 0;
            if (resultedTable != null)
            {
                count = resultedTable.Rows.Count;
            }

            ddlCustomFilterHome.Items.Clear();

            // Added below block, to skip dropdown item, with no records eg. when myopentickets is default selected tab & it doesn't contain records, this tab is selected
            // with blank in dropdown & empty grid, in Request list.

            if (moduleStatResult != null && moduleStatResult.CurrentTab != null && moduleStatResult.TabCounts[moduleStatResult.CurrentTab] == 0)
            {
                selectedTab = moduleStatResult.CurrentTab;
                var tabs = moduleStatResult.TabCounts.Where(x => x.Value > 0).ToList();
                if (tabs != null && tabs.Count() > 0)
                    moduleStatResult.CurrentTab = moduleStatResult.TabCounts.FirstOrDefault(x => x.Value > 0).Key;
            }


            foreach (Tab tab in filterTab.Tabs)
            {
                if (!string.IsNullOrWhiteSpace(tab.Name))
                {
                    switch (tab.Name)
                    {
                        case FilterTab.waitingonme:
                            {
                                #region Waiting On me
                                if (filterTab.Tabs.FindByName(FilterTab.waitingonme) != null && moduleStatResult != null)
                                {
                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.waitingonme);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                    string tabName = tabViewrows.Where(x => x.TabName == FilterTab.waitingonme).First().TabDisplayName;
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                    //if (!string.IsNullOrWhiteSpace(UpdateWaitingOnMeTab))
                                    //    tabName = UpdateWaitingOnMeTab;
                                    ddlCustomFilterHome.Items.Clear();
                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == FilterTab.waitingonme)
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = "waitingonme";
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {
                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }

                                        globalFilterDetialForPDF += waitingOnMeTabName;
                                    }
                                }

                                #endregion
                                break;
                            }

                        case FilterTab.myopentickets:
                            {
                                #region My open
                                if (filterTab.Tabs.FindByName(FilterTab.myopentickets) != null && moduleStatResult != null)
                                {
                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.myopentickets);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                    string tabName = tabViewrows.Where(x => x.TabName == FilterTab.myopentickets).First().TabDisplayName;
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                    //if (!string.IsNullOrWhiteSpace(UpdateMyOpenTicketsTab))
                                    //    tabName = UpdateMyOpenTicketsTab;

                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == tab.Name)
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = "myopentickets";
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {

                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }
                                        if (!string.IsNullOrEmpty(ModuleName))
                                            globalFilterDetialForPDF += tabName;
                                    }
                                }
                                #endregion
                                break;
                            }

                        case FilterTab.mygrouptickets:
                            {
                                #region my group ticket
                                if (filterTab.Tabs.FindByName(FilterTab.mygrouptickets) != null && moduleStatResult != null)
                                {
                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.mygrouptickets);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                    string tabName = string.Format(tabViewrows.Where(x => x.TabName == FilterTab.mygrouptickets).First().TabDisplayName);
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                    //if (!string.IsNullOrWhiteSpace(UpdateMyGroupTicketsTab))
                                    //    tabName = UpdateMyGroupTicketsTab;

                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == "mygrouptickets")
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = "mygrouptickets";
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {
                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }
                                        if (!string.IsNullOrEmpty(ModuleName))
                                            globalFilterDetialForPDF += tabName;
                                    }
                                }
                                #endregion
                                break;
                            }
                        case FilterTab.departmentticket:
                            {
                                #region my Deparment
                                if (filterTab.Tabs.FindByName(FilterTab.departmentticket) != null && moduleStatResult != null)
                                {
                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.departmentticket);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                    string tabName = "My Department";
                                    TabView tabView = tabViewrows.Where(x => x.TabName == FilterTab.departmentticket).FirstOrDefault();
                                    if (tabView != null)
                                        tabName = tabView.TabDisplayName;
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                    //if (!string.IsNullOrWhiteSpace(UpdateMyDepartmentTicketsTab))
                                    //    tabName = UpdateMyDepartmentTicketsTab;

                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == tab.Name)
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = "departmentticket";
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {

                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }

                                        if (!string.IsNullOrEmpty(ModuleName))
                                            globalFilterDetialForPDF += tabName;
                                    }
                                }

                                #endregion
                                break;
                            }

                        case FilterTab.unassigned:
                            {
                                #region Unassigned
                                if (filterTab.Tabs.FindByName(FilterTab.unassigned) != null && moduleStatResult != null)
                                {
                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.unassigned);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                    string tabName = tabViewrows.Where(x => x.TabName == FilterTab.unassigned).First().TabDisplayName;
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                    //if (!string.IsNullOrWhiteSpace(UpdateUnAssignedTab))
                                    //    tabName = UpdateUnAssignedTab;

                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == tab.Name)
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = "unassigned";
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {
                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }

                                        if (!string.IsNullOrEmpty(ModuleName))
                                            globalFilterDetialForPDF += tabName;
                                    }
                                }
                                #endregion
                                break;
                            }

                        case FilterTab.allresolvedtickets:
                            {
                                #region Resolved
                                if (filterTab.Tabs.FindByName(FilterTab.allresolvedtickets) != null && moduleStatResult != null)
                                {
                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.allresolvedtickets);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);

                                    string tabName = tabViewrows.Where(x => x.TabName == FilterTab.allresolvedtickets).First().TabDisplayName;
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                    //else if (ModuleName == "NPR")
                                    //    tabName = "Approved";

                                    //if (!string.IsNullOrWhiteSpace(UpdateResolvedTicketsTab))
                                    //    tabName = UpdateResolvedTicketsTab;

                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == tab.Name)
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = "allresolvedtickets";
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {
                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }

                                        if (!string.IsNullOrEmpty(ModuleName))
                                            globalFilterDetialForPDF += tabName;
                                    }
                                }

                                #endregion
                                break;
                            }

                        case FilterTab.allclosedtickets:
                            {
                                #region Closed
                                if (filterTab.Tabs.FindByName(FilterTab.allclosedtickets) != null && moduleStatResult != null)
                                {

                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.allclosedtickets);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                    string tabName = tabViewrows.Where(x => x.TabName == FilterTab.allclosedtickets).First().TabDisplayName;
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);

                                    //if (!string.IsNullOrWhiteSpace(UpdateCloseTicketTab))
                                    //    tabName = UpdateCloseTicketTab;

                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == tab.Name)
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = "allclosedtickets";
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {
                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }

                                        if (!string.IsNullOrEmpty(ModuleName))
                                            globalFilterDetialForPDF += tabName;
                                    }
                                }
                                break;
                                #endregion
                            }

                        case FilterTab.alltickets:
                            {
                                #region all tickets
                                if (filterTab.Tabs.FindByName(FilterTab.alltickets) != null && moduleStatResult != null)
                                {

                                    int resultCount = 0;
                                    //Tab tab = filterTab.Tabs.FindByName(FilterTab.alltickets);
                                    moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                    string tabName = tabViewrows.Where(x => x.TabName == FilterTab.alltickets).First().TabDisplayName;
                                    //if (tabName.Contains(string.Format("({0})", resultCount)))
                                    //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);

                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                    if (resultCount > 0)
                                        ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                    if (moduleStatResult.CurrentTab == tab.Name)
                                    {
                                        filterTab.ActiveTab = tab;
                                        tabType.Value = FilterTab.alltickets;
                                        ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                        if (isWildCardFiltered == true)
                                        {
                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                        }

                                        if (!string.IsNullOrEmpty(ModuleName))
                                            globalFilterDetialForPDF += tabName;
                                    }
                                }

                                #endregion
                                break;
                            }

                        case FilterTab.allopentickets:
                            {
                                #region all open
                                {
                                    int resultCount = 0;
                                    if (moduleStatResult != null)
                                    {
                                        //Tab tab = filterTab.Tabs.FindByName(FilterTab.allopentickets);
                                        if (tab != null)
                                        {
                                            moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                            string tabName = tabViewrows.Where(x => x.TabName == FilterTab.allopentickets).First().TabDisplayName;
                                            //if (tabName.Contains(string.Format("({0})", resultCount)))
                                            //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                            //if (!string.IsNullOrWhiteSpace(UpdateOpenTicketTab))
                                            //    tabName = UpdateOpenTicketTab;

                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                            //if (resultCount > 0)
                                            // it shd show, user know if open ticket count is 0;
                                            ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                            if (moduleStatResult.CurrentTab == tab.Name)
                                            {
                                                filterTab.ActiveTab = tab;
                                                tabType.Value = "allopentickets";
                                                ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                                if (isWildCardFiltered == true)
                                                {
                                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                                }

                                                if (!string.IsNullOrEmpty(ModuleName))
                                                    globalFilterDetialForPDF += tabName;
                                            }
                                        }
                                        else
                                        {
                                            if (moduleStatResult.CurrentTab == FilterTab.alltickets && moduleStatResult.TabCounts.Count == 1)
                                            {
                                                ddlCustomFilterHome.Items.Add("All Tickets", "alltickets");
                                                ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                            }
                                        }

                                    }
                                }

                                #endregion
                                break;
                            }

                        case FilterTab.OnHold:
                            {
                                //Added by mudassir 17 march 2020  SPDelta 15(Support for separate "On Hold" tab in ticket lists)
                                #region OnHold
                                {
                                    int resultCount = 0;
                                    if (moduleStatResult != null)
                                    {
                                        //Tab tab = filterTab.Tabs.FindByName(FilterTab.OnHold);
                                        if (tab != null)
                                        {
                                            moduleStatResult.TabCounts.TryGetValue(tab.Name, out resultCount);
                                            string tabName = tabViewrows.Where(x => x.TabName == FilterTab.OnHold).First().TabDisplayName;
                                            //if (tabName.Contains(string.Format("({0})", resultCount)))
                                            //    tabName = tabName.Replace(string.Format("({0})", resultCount), string.Empty);
                                            //if (!string.IsNullOrWhiteSpace(UpdateOpenTicketTab))
                                            //    tabName = UpdateOpenTicketTab;

                                            tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                            if (resultCount > 0)
                                                ddlCustomFilterHome.Items.Add(tab.Text, tab.Name);
                                            if (moduleStatResult.CurrentTab == tab.Name)
                                            {
                                                filterTab.ActiveTab = tab;
                                                tabType.Value = "onhold"; //"allopentickets";
                                                ddlCustomFilterHome.SelectedIndex = ddlCustomFilterHome.Items.Count - 1;
                                                if (isWildCardFiltered == true)
                                                {
                                                    tab.Text = string.Format("{0} ({1})", tabName, resultCount);
                                                }

                                                if (!string.IsNullOrEmpty(ModuleName))
                                                    globalFilterDetialForPDF += tabName;
                                            }
                                        }
                                    }
                                }
                                #endregion
                                break;
                            }
                    }
                }
            }
            //
            if (!IsPreview && initiateExport)
            {
                cModuleFilteredLinksPanel.Visible = false;
                cModuleGlobalFilterPanel.Visible = false;
                moduleLogoLink.Attributes.Add("onClick", "javascript:");
                mModuleNewTicketPanel.Visible = false;
                iconContainer.Visible = false;

            }

            if (resultedTable != null)
                tabCount = resultedTable.Rows.Count;

            if (moduleStatResult != null && moduleStatResult.CurrentTab != null && !string.IsNullOrEmpty(selectedTab))
                moduleStatResult.CurrentTab = selectedTab;



        }

        private void LoadFilteredFieldsControls(DataTable resultedTable)
        {
            ListItem item;
            if (resultedTable != null)
            {
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketId))
                {
                    item = new ListItem("Ticket ID", DatabaseObjects.Columns.TicketId);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketStatus))
                {
                    item = new ListItem("Status", DatabaseObjects.Columns.TicketStatus);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                {
                    item = new ListItem("Owner", DatabaseObjects.Columns.TicketOwner);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketInitiator))
                {
                    item = new ListItem("Initiator", DatabaseObjects.Columns.TicketInitiator);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketRequestor))
                {
                    item = new ListItem("Requestor", DatabaseObjects.Columns.TicketRequestor);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
                {
                    item = new ListItem("PRP", DatabaseObjects.Columns.TicketPRP);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketORP))
                {
                    item = new ListItem("ORP", DatabaseObjects.Columns.TicketORP);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketTester))
                {
                    item = new ListItem("Tester", DatabaseObjects.Columns.TicketTester);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    item = new ListItem("Initiated Date", DatabaseObjects.Columns.TicketCreationDate);
                    lstFilteredFields.Items.Add(item);
                }
                if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
                {
                    item = new ListItem("Waiting On", DatabaseObjects.Columns.TicketStageActionUsers);
                    lstFilteredFields.Items.Add(item);

                    if (resultedTable.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                    {
                        item = new ListItem("Request Type", DatabaseObjects.Columns.TicketRequestTypeLookup);
                        lstFilteredFields.Items.Add(item);
                    }
                }

                lstFilteredFields.Items.Insert(0, new ListItem("--Select Field--", ""));
            }
        }

        protected void gridTemplate_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.RelationshipType)
                e.Cell.Text = string.Join(",", UGITUtility.SplitString(e.CellValue, Constants.Separator, StringSplitOptions.RemoveEmptyEntries));
            // Commented below code, as Owner column in LEM -> create OPM popup is showing guid instead of names.
            //if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketOwner)
            //    e.Cell.Text = string.Join(",", uHelper.GetMultiLookupValue(Convert.ToString(e.CellValue)));
        }
        private void BindTemplateGridByAction(string action)
        {
            if (!string.IsNullOrEmpty(ModuleName))
                BindTemplateGrid();
            else
            {
                var module = ModuleViewManager.LoadAllModules();
                DataRow[] dRow = module.Select(string.Format("{0}=1 AND {1}=1", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableNewTicketsOnHomePage));

                // DataTable moduletable = uGITCache.GetDataTable(DatabaseObjects.Lists.Modules, SPContext.Current.Web);
                // DataRow[] dRow = module.Select(string.Format("{0}=1 AND {1}=1", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableNewTicketsOnHomePage));
                BindTemplateGrid(dRow.Select(x => x.Field<string>(DatabaseObjects.Columns.ModuleName)).ToArray());
            }
            gridTemplate.SettingsPopup.HeaderFilter.Width = 300;
            gridTemplate.SettingsPopup.HeaderFilter.Height = 300;
            gridTemplate.StylesPopup.HeaderFilter.Content.CssClass = "SearchFilter_content";
            gridTemplate.StylesPopup.HeaderFilter.ButtonPanel.CancelButton.CssClass = "Filter_cancelBtn";
            gridTemplate.StylesPopup.HeaderFilter.ButtonPanel.OkButton.CssClass = "Filter_okBtn";
            gridTemplate.StylesPopup.HeaderFilter.Footer.CssClass = "FilterFooter_btnWrap";
            switch (action)
            {

                case "ShowTicketTemplateList":

                    PopupControl.Width = new Unit(250, UnitType.Pixel);
                    PopupControl.HeaderText = "Select template to create new item";
                    gridTemplate.Columns.Clear();
                    gridTemplate.Visible = true;

                    GridViewDataTextColumn colTId = null;
                    hndType.Value = "Template";
                    colTId = new GridViewDataTextColumn();
                    colTId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    colTId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                    colTId.PropertiesTextEdit.EncodeHtml = false;
                    colTId.FieldName = Convert.ToString("Title");
                    colTId.Caption = Convert.ToString("Template");
                    colTId.HeaderStyle.Font.Bold = true;
                    colTId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                    //colTId.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                    colTId.Width = GetColumnWidth(DatabaseObjects.Columns.TicketStatus);
                    gridTemplate.Columns.Add(colTId);
                    BindTemplateGrid();
                    break;

                case "NewLeadFromCompany":
                case "NewContactFromCompany":
                    if (hdnPopupControlName.Value == "NewLeadFromCompany")
                        PopupControl.HeaderText = "Select Company";
                    else
                        PopupControl.HeaderText = "Select Company";

                    GenerateNewTicketGrid(DatabaseObjects.Tables.CRMCompany, "COM");
                    
                    gridTemplate.Visible = true;
                    gridTemplate.SettingsPager.PageSize = 15;
                    PopupControl.Width = new Unit(800, UnitType.Pixel);
                    break;

                case "NewOpportunityFromLead":
                    PopupControl.HeaderText = "Select Lead to create New Opportunity";
                    GenerateNewTicketGrid(DatabaseObjects.Tables.Lead, "LEM");
                    gridTemplate.Visible = true;
                    PopupControl.Width = new Unit(1200, UnitType.Pixel);
                    break;
                case "NewProjectFromOpportunity":
                case "NewCPOFromOpportunity":
                    PopupControl.HeaderText = "Select Opportunity to create New Project";
                    GenerateNewTicketGrid(DatabaseObjects.Tables.Opportunity, "OPM");
                    gridTemplate.Visible = true;
                    PopupControl.Width = new Unit(1200, UnitType.Pixel);
                    break;
                case "NewServiceFromOpportunity":
                    PopupControl.HeaderText = "Select Opportunity to create New Service";
                    GenerateNewTicketGrid(DatabaseObjects.Tables.Opportunity, "OPM");
                    gridTemplate.Visible = true;
                    PopupControl.Width = new Unit(1200, UnitType.Pixel);
                    break;
                default:
                    break;
            }
        }

        protected void PopupControl_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                if (!PopupControl.JSProperties.ContainsKey("cpWidth"))
                {
                    PopupControl.JSProperties["cpWidth"] = 0;
                }

                if (!PopupControl.JSProperties.ContainsKey("cpHeaderText"))
                {
                    PopupControl.JSProperties["cpHeaderText"] = 0;
                }

                BindTemplateGridByAction(e.Parameter.ToString());
            }
            //  BindTemplateGrid();
        }
        protected void pcSaveAsTemplate_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            //ctrSaveAllocationAsTemplate.ProjectID

            ctrSaveAllocationAsTemplate.ModuleName = ModuleName;
            ctrSaveAllocationAsTemplate.ProjectID = hdnticketId.Value;
            ctrSaveAllocationAsTemplate.PopupID = pcSaveAsTemplate.ClientID;
            DataRow ticketRow = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, hdnticketId.Value);
            if (ticketRow != null)
            {
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.PreconStartDate))
                    ctrSaveAllocationAsTemplate.PreconStartDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconStartDate]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.PreconEndDate))
                    ctrSaveAllocationAsTemplate.PreconEndDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.PreconEndDate]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionStart))
                    ctrSaveAllocationAsTemplate.ConstStartDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionStart]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionEnd))
                    ctrSaveAllocationAsTemplate.ConstEndDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.CloseoutStartDate))
                    ctrSaveAllocationAsTemplate.CloseOutStartDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutStartDate]);
                if (ticketRow.Table.Columns.Contains(DatabaseObjects.Columns.CloseoutDate))
                    ctrSaveAllocationAsTemplate.CloseOutEndDate = UGITUtility.StringToDateTime(ticketRow[DatabaseObjects.Columns.CloseoutDate]);
            }
        }

        private void GenerateNewTicketGrid(string listName, string moduleName)
        {
            hndNewTicketTitle.Value = UGITUtility.newTicketTitle(this.ModuleName);
            gridTemplate.Columns.Clear();

            //SPList spResult = SPListHelper.GetSPList(listName);
            PopupControl.JSProperties["cpWidth"] = 1200;
            UGITModule moduleData = ModuleViewManager.LoadByName(moduleName, true);
            DataTable dtResult = TicketManager.GetOpenTickets(moduleData);
            hndType.Value = moduleName;
            GridViewDataTextColumn colTId = new GridViewDataTextColumn();
            colTId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colTId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            colTId.PropertiesTextEdit.EncodeHtml = false;
            colTId.FieldName = Convert.ToString("Title");
            colTId.Caption = Convert.ToString("Template");
            colTId.HeaderStyle.Font.Bold = true;
            colTId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            gridTemplate.Columns.Add(colTId);
            gridTemplate.DataSource = dtResult.DefaultView.ToTable(true,DatabaseObjects.Columns.ID,DatabaseObjects.Columns.Title);
            gridTemplate.DataBind();

        }

        private void ConfigureTemplateGrid()
        {
            gridTemplate.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            gridTemplate.AutoGenerateColumns = false;
            gridTemplate.SettingsBehavior.AllowDragDrop = false;
            gridTemplate.SettingsText.EmptyHeaders = "  ";
            gridTemplate.SettingsPager.PageSizeItemSettings.Visible = true;
            gridTemplate.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;
            gridTemplate.SettingsBehavior.AllowSort = false;
            gridTemplate.SettingsBehavior.AllowSelectByRowClick = true;
            gridTemplate.SettingsBehavior.EnableRowHotTrack = true;
            gridTemplate.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            gridTemplate.Styles.AlternatingRow.CssClass = "ugitlight1lightest";
            gridTemplate.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            gridTemplate.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
        }

        private void BindTemplateGrid(string[] moduleList = null)
        {
            Ticket objTicket = new Ticket(_context, ModuleName);
            if (moduleList == null)
            {
                moduleList = new string[] { ModuleName };
            }
            DataTable fieldTable = new DataTable();
            fieldTable.Columns.Add(DatabaseObjects.Columns.ID, typeof(long));
            fieldTable.Columns.Add(DatabaseObjects.Columns.Title);
            fieldTable.Columns.Add(DatabaseObjects.Columns.ModuleName);
            fieldTable.Columns.Add(DatabaseObjects.Columns.ModuleDescription);
            fieldTable.Columns.Add(DatabaseObjects.Columns.LinkUrl);
            fieldTable.Columns.Add(DatabaseObjects.Columns.TemplateType);
            if (moduleList.Length > 0)
            {
                DataTable dtResult = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketTemplates, $"TenantId='{_context.TenantID}'");
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    DataTable dtModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"TenantId='{_context.TenantID}'");
                    string moduleInQuery = string.Join(",", moduleList.Select(x => string.Format("'{0}'", x)));
                    DataRow[] moduleRows = dtModules.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.ModuleName, moduleInQuery));
                    foreach (DataRow moduleRow in moduleRows)
                    {
                        DataRow[] drRows = dtResult.Select(string.Format("{0} = '{1}' And {2} in ('Ticket','Macro')", DatabaseObjects.Columns.ModuleNameLookup, moduleRow[DatabaseObjects.Columns.ModuleName], DatabaseObjects.Columns.TemplateType));
                        //if (hdnPopupControlName.Value == "ShowTicketTemplateList")
                        //{
                        hndNewTicketTitle.Value = UGITUtility.newTicketTitle(this.ModuleName);
                        if (drRows != null && drRows.Length > 0)
                        {
                            foreach (DataRow dr in drRows)
                            {
                                if (!gridTemplate.JSProperties.ContainsKey(string.Format("cpUrl_{0}", dr[DatabaseObjects.Columns.Id])))
                                    gridTemplate.JSProperties.Add(string.Format("cpUrl_{0}", dr[DatabaseObjects.Columns.ID]), UGITUtility.GetAbsoluteURL(Convert.ToString(moduleRow[DatabaseObjects.Columns.StaticModulePagePath])));
                                fieldTable.Rows.Add(
                                    dr[DatabaseObjects.Columns.ID],
                                    dr[DatabaseObjects.Columns.Title],
                                    moduleRow[DatabaseObjects.Columns.ModuleName],
                                    moduleRow[DatabaseObjects.Columns.Title],
                                    moduleRow[DatabaseObjects.Columns.StaticModulePagePath],
                                    dr[DatabaseObjects.Columns.TemplateType]);
                            }
                        }
                        //}
                    }
                }
            }
            if (fieldTable.Rows.Count > 0)
            {
                fieldTable.DefaultView.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.ModuleDescription, DatabaseObjects.Columns.Title);
                fieldTable = fieldTable.DefaultView.ToTable();
                gridTemplate.DataSource = fieldTable;
                gridTemplate.DataBind();
            }

        }

        protected void grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

        }

        private void BindModuleListItem()
        {
            /*
            DataTable dtModules;
            DataTable AlldtModules = ModuleViewManager.LoadAllModules();
            if (AlldtModules == null || AlldtModules.Rows.Count == 0)
                return;

            DataRow[] dr = AlldtModules.Select(string.Format("{0}=true and {1}<>'{2}'", DatabaseObjects.Columns.AllowChangeTicketType, DatabaseObjects.Columns.ModuleName, ModuleName));
            if (dr.Length > 0)
                dtModules = dr.CopyToDataTable();
            else
                dtModules = AlldtModules.Clone();
            */

            var dtModules = ModuleViewManager.Load(x => x.AllowChangeType && x.ModuleName != ModuleName).Select(x => new { x.Title, x.ModuleName }).ToList();


            if (dtModules == null && dtModules.Count <= 0)
            {
                dtModules = ModuleViewManager.LoadAllModule().Select(x => new { x.Title, x.ModuleName }).ToList();
            }

            if (dtModules != null && dtModules.Count > 0)
            {
                ddlModuleListItems.DataSource = dtModules;
                ddlModuleListItems.DataTextField = DatabaseObjects.Columns.Title;
                ddlModuleListItems.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModuleListItems.DataBind();
                trModuleListItemMessage.Visible = false;
                trModulelistItem.Visible = true;
            }
            else
            {
                trModuleListItemMessage.Visible = true;
                trModulelistItem.Visible = false;
                btnChangeTicketTypeSave.Enabled = false;
            }
        }

        private void BindModuleFormTab()
        {
            ddlSelectTab.Items.Clear();
            if (!string.IsNullOrEmpty(ModuleName))
            {
                List<ModuleFormTab> LstFormTab = new List<ModuleFormTab>();
                LstFormTab = _moduleViewManager.LoadByName(ModuleName).List_FormTab.Where(x => x.ModuleNameLookup == ModuleName).OrderBy(x => x.TabSequence).ToList();
                ddlSelectTab.DataTextField = "TabName";
                ddlSelectTab.DataValueField = "TabSequence";

                if (HttpContext.Current.Request.Browser.IsMobileDevice == true)
                    ddlSelectTab.DataSource = LstFormTab.Where(x => x.ShowInMobile == true);
                else
                    ddlSelectTab.DataSource = LstFormTab;

                ddlSelectTab.DataBind();

                ddlSelectTab.Items.Insert(0, new ListItem("Default", "0"));
            }
        }

        protected void pcChangeTicketType_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                if (!pcChangeTicketType.JSProperties.ContainsKey("cpTicketUrl"))
                {
                    pcChangeTicketType.JSProperties.Add("cpTicketUrl", false);
                }

                DataTable moduledt = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(e.Parameter, true));
                DataRow moduleDetail = moduleRow;
                if (moduledt.Rows.Count > 0)
                {
                    moduleDetail = moduledt.Rows[0];// uGITCache.GetModuleDetails(e.Parameter);
                }
                if (moduleDetail[DatabaseObjects.Columns.StaticModulePagePath] != null)
                {
                    pcChangeTicketType.JSProperties["cpTicketUrl"] = UGITUtility.GetAbsoluteURL(moduleDetail[DatabaseObjects.Columns.StaticModulePagePath].ToString());
                }
            }
        }

        private void FillModules()
        {
            /*
            cblModules.Items.Clear();
            DataTable dtModules = UGITUtility.ObjectToData(ModuleViewManager.LoadByName(ModuleName));// uGITCache.ModuleConfigCache.LoadModuleDtByName(ModuleName);
            var modules = dtModules.AsEnumerable()
                                   .Where(x => !x.IsNull(DatabaseObjects.Columns.ShowTicketSummary)
                                             && x.Field<bool>(DatabaseObjects.Columns.ShowTicketSummary)
                                             && !x.IsNull(DatabaseObjects.Columns.EnableModule)
                                             && x.Field<bool>(DatabaseObjects.Columns.EnableModule));
            if (modules.Count() > 0)
            {
                cblModules.DataSource = modules.CopyToDataTable();
                cblModules.DataTextField = DatabaseObjects.Columns.Title;
                cblModules.DataValueField = DatabaseObjects.Columns.ModuleName;
                cblModules.DataBind();
            }
            */
            cblModules.Items.Clear();
            var modules = ModuleViewManager.Load(x => x.ModuleName == ModuleName).Where(x => x.ShowSummary == true && x.EnableModule == true).Select(x => new { x.Title, x.ModuleName }).ToList();
            if (modules != null && modules.Count > 0)
            {
                cblModules.DataSource = modules;
                cblModules.DataTextField = DatabaseObjects.Columns.Title;
                cblModules.DataValueField = DatabaseObjects.Columns.ModuleName;
                cblModules.DataBind();
            }

            //   ddlModules.Items.Insert(0, new ListItem("All", "All"));
        }

        #endregion

        protected void AspxPopupMenuNewTicket_Load(object sender, EventArgs e)
        {
            AspxPopupMenuNewTicket.Items.Clear();

            if (EnableNewButton)
            {
                DataTable moduletable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                DataRow[] dRow = moduletable.Select(string.Format("{0}=1 AND {1}=1", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.EnableNewTicketsOnHomePage));

                if (dRow != null && dRow.Length > 0 && IsHomePage)
                {
                    showNewTicketAtHome = true;
                    DataTable dtOut = null;
                    DataTable dtIn = dRow.CopyToDataTable();
                    dtIn.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                    dtOut = dtIn.DefaultView.ToTable();

                    if (UserManager.IsAdmin(User))
                    {
                        if (dtOut.Rows.Count == 1)
                        {
                            //lnkNewTicket1.Attributes.Add("onclick",string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}', '{3}', '{1}', '90', '90', 0, '{2}')", UGITUtility.GetAbsoluteURL(Convert.ToString(dtOut.Rows[0][DatabaseObjects.Columns.StaticModulePagePath])), string.Format("New {0} Ticket", dtOut.Rows[0][DatabaseObjects.Columns.ModuleName]), Server.UrlEncode(Request.Url.AbsolutePath), "homepage = 1"));
                            lnkNewTicket1.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}', '{3}', '{1}', '90', '90', 0, '{2}')", UGITUtility.GetAbsoluteURL(Convert.ToString(dtOut.Rows[0][DatabaseObjects.Columns.StaticModulePagePath])), string.Format("New {0} Ticket", dtOut.Rows[0][DatabaseObjects.Columns.UGITShortName]), Server.UrlEncode(Request.Url.AbsolutePath), "homepage = 1"));
                            AspxPopupMenuNewTicket.PopupElementID = "";
                            lnkNewTicket1.ImageUrl = "";
                        }
                        else
                        {
                            foreach (DataRow dr in dtOut.Rows)
                            {
                                string popupTitle = UGITUtility.moduleTypeName(Convert.ToString(dr[DatabaseObjects.Columns.ModuleName]));
                                //DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem(Convert.ToString(dr[DatabaseObjects.Columns.Title]), string.Empty, string.Format("/content/images/Menu/SubMenuBlue/{0}_32x32.svg", dr[DatabaseObjects.Columns.ModuleName]), string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}', '{3}', '{1}', '90', '90', 0, '{2}')", UGITUtility.GetAbsoluteURL(Convert.ToString(dr[DatabaseObjects.Columns.StaticModulePagePath])), string.Format("New {0} Ticket", popupTitle), Server.UrlEncode(Request.Url.AbsolutePath), "homepage=1"));
                                DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem($"{popupTitle} ({dr[DatabaseObjects.Columns.ModuleName]})", string.Empty, string.Format("/content/images/Menu/SubMenuBlue/{0}_32x32.svg", dr[DatabaseObjects.Columns.ModuleName]), string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}', '{3}', '{1}', '90', '90', 0, '{2}')", UGITUtility.GetAbsoluteURL(Convert.ToString(dr[DatabaseObjects.Columns.StaticModulePagePath])), string.Format("New {0} Item", popupTitle), Server.UrlEncode(Request.Url.AbsolutePath), "homepage=1"));
                                item.ItemStyle.CssClass = "ccsNewbuttonAtHome";
                                item.Image.Width = Unit.Pixel(16);
                                item.Image.Height = Unit.Pixel(16);
                                AspxPopupMenuNewTicket.Items.Add(item);
                            }
                        }
                    }
                    else
                    {
                        showNewTicketAtHome = false;
                    }
                }
                else
                    showNewTicketAtHome = false;
            }
        }

        protected void ASPxPopupMenuReportList_Load(object sender, EventArgs e)
        {
            if (Request.QueryString != null && Request.QueryString.AllKeys.Length > 0)
            {
                if (Convert.ToString(Request.QueryString["control"]) == "problemreportdrildowndata")
                {
                    imgReport.Visible = false;
                }
            }
            else
            {
                imgReport.Visible = true;
            }

            DashboardManager dashboardManager = new DashboardManager(_context);
            ASPxPopupMenuReportList.Items.Clear();
            //ASPxPopupMenuReportList.ItemImage.Height = 16;
            ASPxPopupMenuReportList.ItemImage.Width = 16;
            int reportCount = 0;
            ASPxPopupMenu menu = sender as ASPxPopupMenu;

            if (!string.IsNullOrEmpty(ModuleName))
            {
                if (ModuleName.ToUpper() == "PMM" || ModuleName.ToUpper() == "NPR")
                {
                    reportCount++;
                    menu.Items.Add("Business Initiatives", "BusinessStrategy", "/Content/images/statusBlue.png");

                    reportCount++;
                    menu.Items.Add("Projects By Due Date", "BusinessStrategyCompletionDate", "/Content/images/Active-Projects.png");

                    reportCount++;
                    menu.Items.Add("Project Summary", "ProjectSummary", "/Content/images/summary.png");
                }

                if (ModuleName.ToUpper() == "TSK" || ModuleName.ToUpper() == "PMM")
                {
                    if (ModuleName.ToUpper() == "TSK")
                    {
                        reportCount++;
                        menu.Items.Add("Task Report", "TaskReport", "/Content/images/Active-Projects.png");
                    }
                    else
                    {
                        reportCount++;
                        menu.Items.Add("Project Report", "ProjectReport", "/Content/images/executive-summary.png");
                        menu.Items.Add("1-Pager Report", "ProCompactReport", "/Content/images/Active-Projects.png");
                    }

                    reportCount++;
                    menu.Items.Add("Task Summary", "TaskSummary", "/Content/images/summary.png");
                }

                if (ModuleName.ToUpper() == "PMM" || ModuleName.ToUpper() == "NPR")
                {
                    reportCount++;
                    menu.Items.Add("Gantt View", "GanttView", "/Content/images/utility.png");
                }
                else if (ModuleName.ToUpper() == "DRQ")
                {
                    reportCount++;
                    menu.Items.Add("Calendar View", "CalendarView", "/Content/images/calender_active.png");
                }
                else if (ModuleName.ToUpper() == "APP")
                {
                    reportCount++;
                    menu.Items.Add("Application Report", "ApplicationReport", "/Content/images/Application-Report.png");
                }

                if (moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.ShowTicketSummary]))
                {
                    reportCount++;
                    if (ModuleName.ToUpper() != "CPR" && ModuleName.ToUpper() != "OPM" && ModuleName.ToUpper() != "LEM" && ModuleName.ToUpper() != "CNS")
                    {
                        menu.Items.Add(string.Format("{0} Summary", UGITUtility.moduleTypeName(ModuleName)), "TicketSummary", "/Content/images/TicketSummary.png");
                        menu.Items.Add("Summary By Technician", "TicketSummaryByPRP", "/Content/images/summary-by-technician.png");
                    }
                }

                if (ModuleName.ToUpper() == "TSR")
                {
                    reportCount++;
                    menu.Items.Add("Weekly Team Report", "WeeklyTeamPerformance", "/Content/images/Weekly-Team-Report.png");
                    menu.Items.Add("Non-Peak Hour Performance", "HelpDeskPerformance", "/Content/images/Non-Peak-Hour-Performance.png");
                }
                if (moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.ShowBottleNeckChart]))
                {
                    reportCount++;
                    menu.Items.Add("Bottleneck Chart", "BottleNeckChart", "/Content/images/pie-chart.svg");
                }
                if (ModuleName.ToUpper() == "SVC")
                {
                    reportCount++;
                    menu.Items.Add("Pending Service Tasks", "SVCProjectTask", "/Content/images/icons/email-icon-black.png");
                }

                if (ModuleName.ToUpper() == "PMM") // && uGITCache.GetConfigVariableValueAsBool(ConfigConstants.TrackProjectStageHistory))
                {
                    reportCount++;
                    menu.Items.Add(string.Format("Project Stage History"), "TrackProjectStageHistory", "/Content/images/icons/email-icon-black.png");
                }

                //if (ModuleName.ToUpper() == "CPR" || ModuleName.ToUpper() == "CNS")
                //{
                //    reportCount++;
                //    menu.Items.Add("Executive Summary", "ExecutiveSummary", "/Content/images/executive-summary.png");
                //}

                if (ModuleName.ToUpper() == "CPR" || ModuleName.ToUpper() == "CNS" || ModuleName.ToUpper() == "OPM")
                {
                    reportCount++;
                    menu.Items.Add("Combined Lost Job Report", "CombinedLostJobReport", "/Content/images/combined-Lost-Job-Report.png");

                    //reportCount++;
                    //menu.Items.Add("Combined Job Report", "CombinedJobReport", "/Content/images/combined-Lost-Job-Report.png");

                    reportCount++;
                    menu.Items.Add("Division Distribution Report", "BusinessUnitDistributionReport", "/Content/images/Business-Unit-Distribution.png");

                    reportCount++;
                    menu.Items.Add("Studio Specific Report", "StudioSpecificReport", "/Content/images/Business-Unit-Distribution.png");

                    //reportCount++;
                    //menu.Items.Add("Resource Scheduler Report", "ResourceSchedulerReport", "/Content/images/GanttChart.png");
                }

                if (ModuleName.ToUpper() == "OPM")
                {
                    reportCount++;
                    menu.Items.Add(string.Format("OPM Wins and Losses"), "OPMWinsAndLosses", "/Content/images/icon_question.png");
                }



                //show reports (query report) Module..
                //List<string> expressions = new List<string>();
                //expressions.Add(string.Format("{0}={1}", DatabaseObjects.Columns.DashboardType, DashboardType.Query));
                //expressions.Add(string.Format("<Neq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Neq>", DatabaseObjects.Columns.IsActivated, 0));
                //expressions.Add(string.Format("<Contains><FieldRef Name='{0}'/><Value Type='LookupMulti'>{1}</Value></Contains>", DatabaseObjects.Columns.DashboardModuleMultiLookup, ModuleName));
                //expressions.Add(string.Format(@"<Or><Or><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='User'>{1}</Value></Eq>
                // <Membership Type='CurrentUserGroups'><FieldRef Name='{0}'/></Membership></Or>
                // <IsNull><FieldRef Name='{0}' /></IsNull></Or>", DatabaseObjects.Columns.AuthorizedToView, context.CurrentUser.Id));
                //string query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.DashboardType, DashboardType.Query, DatabaseObjects.Columns.IsActivated, 0);

                // DataTable dashboardTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels).AsEnumerable().Where(x => x.Field<long>(DatabaseObjects.Columns.DashboardType) == (long)DashboardType.Query && !x.Field<bool>(DatabaseObjects.Columns.IsActivated) && x.Field<string>(DatabaseObjects.Columns.DashboardModuleMultiLookup).Contains(ModuleName) || x.Field<string>(DatabaseObjects.Columns.AuthorizedToView) == HttpContext.Current.CurrentUser().Id).CopyToDataTable();

                //List<Dashboard> dashboardTable = dashboardManager.Load(x => x.DashboardType.Equals(DashboardType.Query) && !(x.IsActivated.GetValueOrDefault()) && x.DashboardModuleMultiLookup.Contains(ModuleName));

                if (!string.IsNullOrEmpty(ModuleName) && !ModuleName.EqualsIgnoreCase("all"))
                {
                    string moduleId = Convert.ToString(_moduleViewManager.GetModuleIdByName(ModuleName));
                    //List<Dashboard> dashboardTable = dashboardManager.Load(x => x.DashboardType.Equals(DashboardType.Query) && x.IsActivated == true && x.DashboardModuleMultiLookup.Contains(moduleId));
                    List<Dashboard> dashboardTable = dashboardManager.Load($"{DatabaseObjects.Columns.DashboardType}={(int)DashboardType.Query} and {DatabaseObjects.Columns.IsActivated}={1} and {DatabaseObjects.Columns.DashboardModuleMultiLookup} like '%{moduleId}%'");


                    if (dashboardTable != null && dashboardTable.Count > 0)
                    {
                        foreach (Dashboard rowItem in dashboardTable)
                        {
                            reportCount++;
                            menu.Items.Add(UGITUtility.TruncateWithEllipsis(rowItem.Title, 25), rowItem.Title + Constants.Separator + Convert.ToString(rowItem.ID), "/Content/images/executive-summary.png");
                        }
                    }

                    if (UGITUtility.StringToBoolean(_context.ConfigManager.GetValue("ShowQueryReports")) && !HideModuleDetail)
                    {
                        reportCount++;
                        menu.Items.Add("Query Reports", "QueryReport", "/Content/images/Query-Reports.png");
                    }
                }
            }
            if (reportCount == 0)
            {
                imgReport.Visible = false;
            }
        }

        protected void ASPxPopupCustomMenuItem_Load(object sender, EventArgs e)
        {
            UGITModule module = ModuleViewManager.LoadByName(ModuleName, true);
            ASPxPopupCustomMenuItem.Items.Clear();
            ASPxPopupMenu menu = sender as ASPxPopupMenu;
            menu.ItemImage.Height = 20;
            menu.ItemImage.Width = 18;
            if (btNewbutton.Visible)
            {
                menu.Items.Add(btNewbutton.ToolTip, "NewRequest", "/Content/images/newBaseline-black.png");
            }

            if (btnNewContact.Visible)
            {
                menu.Items.Add(btnNewContact.ToolTip, "NewContactbyCompany", "/Content/images/newBaseline-black.png");
            }

            //if (btQuickTicket.Visible)
            //{
            //    menu.Items.Add("New Project From Template", "NewProjectFromTemplate", "/Content/images/newBaseline.png");
            //}

            if (ModuleName.ToUpper() == "CPR")
            {
                menu.Items.Add("New Project From Opportunity", "NewProjectFromOpportunity", "/Content/images/newBaseline-black.png");
            }

            if (ModuleName.ToUpper() == "CPR" && (isActionUser || UserManager.IsUGITSuperAdmin(User) || isAdmin)) //SPContext.Current.Web.CurrentUser
            {
                menu.Items.Add("New Company", "NewCompany", "/Content/images/newBaseline-black.png");
                menu.Items.Add("New Contact", "NewContact", "/Content/images/newBaseline-black.png");
            }

            if (ModuleName.ToUpper() == ModuleNames.OPM /*|| ModuleName.ToUpper() == ModuleNames.CNS*/)
            {
                //menu.Items.Add("New Opportunity", "NewOpportunity", "/Content/images/newBaseline.png");
                menu.Items.Add("New Opportunity From Lead", "AdvancedLead", "/Content/images/newBaseline.png");
                menu.Items.Add("New Project From Opportunity", "OpenOpportunity", "/Content/images/newBaseline.png");
            }


            if (module != null && module.EnableAddNewButton == true && Convert.ToBoolean(Request.QueryString["isClosed"]) == false)
                dvMenuItems.Visible = true;
            else
                dvMenuItems.Visible = false;
        }

        protected void ASPxPopupCustomFilterActionMenu_Load(object sender, EventArgs e)
        {
            ASPxPopupCustomFilterActionMenu.Items.Clear();
            ASPxPopupMenu menu = sender as ASPxPopupMenu;
            menu.ItemImage.Height = 20;
            menu.ItemImage.Width = 18;
            bool useNewBlackIcon = false;

            if (ModuleName.ToUpper() == "CPR")
            {
                useNewBlackIcon = true;
            }

            // NOTE: Add in alphabetical order here so menu items are shown alphabetically

            // Assign To Me
            if (MTicketStatus != TicketStatus.Closed && moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.AllowReassignFromList]))
                menu.Items.Add("Assign To Me", "AssignToMe", "/Content/ButtonImages/assigntome_black.png");

            // Agents sub-menu
            if (MTicketStatus != TicketStatus.Closed && isAdmin)
            {
                DevExpress.Web.MenuItem agentItem = uHelper.AddAgentItem(_context, ModuleName, string.Empty);
                if (agentItem != null)
                    menu.Items.Add(agentItem);
            }

            // Batch Create
            if (MTicketStatus != TicketStatus.Closed && moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.AllowBatchCreate]) && (isAdmin || isHelpDesk))
                menu.Items.Add("Batch Create", "BatchCreate", "/Content/images/newBaseline-black.png");

            // Batch Edit
            if (MTicketStatus != TicketStatus.Closed && moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.AllowBatchEditing]) && (isAdmin || isHelpDesk))
            {
                menu.Items.Add("Edit", "BatchEdit", useNewBlackIcon ? "/Content/images/icons/edit-black.png" : "/Content/images/editIcon-new.png");
            }
            // Change Ticket Type
            if (MTicketStatus != TicketStatus.Closed && moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.AllowChangeTicketType])) // && (isAdmin || isHelpDesk))
                menu.Items.Add("Change Item Type", "ChangeTicketType", "/Content/ButtonImages/changeTicketType-black.png");

            // Comment
            if (moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.ShowComment]))
                menu.Items.Add("Comment", "Comment", "/Content/Images/Comment_Black.png");
            if (ModuleName.ToUpper() == "COM")
            {
                menu.Items.Add("New Lead", "NewLead", "/Content/images/add_icon-black.png");
                menu.Items.Add("Project View", "ProjectCardView", "/Content/images/newBaseline-black.png");
            }
            else if (ModuleName.ToUpper() == "CON")
            {
                menu.Items.Add("Project View", "ProjectCardView", "/Content/images/newBaseline-black.png");
                menu.Items.Add("New Contact From Company", "NewContactFromCompany", "/Content/images/newBaseline-black.png");
            }

            else if (ModuleName.ToUpper() == "LEM")
            {
                menu.Items.Add("New Lead From Company", "NewLeadFromCompany", "/Content/images/newBaseline-black.png");
                menu.Items.Add("New Opportunity From Lead", "NewOpportunity", "/Content/images/newBaseline-black.png");
                menu.Items.Add("New CPR From Lead", "NewCPR", "/Content/images/newBaseline-black.png");
            }
            //else if (ModuleName.ToUpper() == "OPM")
            //{
            //    menu.Items.Add("New Opportunity From Lead", "NewOpportunityFromLead", "/Content/images/newBaseline.png");
            //    menu.Items.Add("New Opportunity Wizard", "NewOPMWizard", "/Content/images/newBaseline.png");
            //    //menu.Items.Add("Assign to PreCon", "NewProject", "/_layouts/15/images/uGovernIT/add_icon.png");
            //}
            //else if (ModuleName.ToUpper() == "CPR")
            //{
            //    menu.Items.Add("New Project From Opportunity", "NewProjectFromOpportunity", "/Content/images/newBaseline.png");
            //}
            else if (ModuleName.ToUpper() == "CNS")
            {
                menu.Items.Add("New Service From Opportunity", "NewServiceFromOpportunity", "/Content/images/newBaseline-black.png");
            }

            if ((ModuleName.ToUpper() == "LEM" || ModuleName.ToUpper() == "CNS") && (isActionUser || UserManager.IsUGITSuperAdmin(User) || isAdmin)) //SPContext.Current.Web.CurrentUser
            {
                menu.Items.Add("Create New Company", "NewCompany", "/Content/images/newBaseline-black.png");
                menu.Items.Add("Create New Contact", "NewContact", "/Content/images/newBaseline-black.png");
            }


            //new condition for show/hide the button for batch Create.
            if (moduleRow != null && UGITUtility.StringToBoolean(Convert.ToString(moduleRow[DatabaseObjects.Columns.AllowBatchCreate])) && isAdmin)
            {
                if (menu.Items.FindByText("Batch Create") == null)
                {
                    menu.Items.Add("Batch Create", "BatchCreate", "/Content/images/newBaseline-black.png");
                }

            }

            if (!(ModuleName.ToUpper() == "CNS" || ModuleName.ToUpper() == "CPR" || ModuleName.ToUpper() == "OPM"))
            {
                // Copy ticket link to clipboard
                menu.Items.Add("Copy Link to Clipboard", "CopyLinktoClipboard", "/Content/images/copyLinktoClipboard-black.png");

                // Manual Escalation (email)
                if (moduleRow != null && UGITUtility.StringToBoolean(Convert.ToString(moduleRow[DatabaseObjects.Columns.AllowEscalationFromList]))) // && (isAdmin || isHelpDesk))
                    menu.Items.Add("Email", "Escalation", "/Content/images/icons/email-icon-black.png");
            }

            //Duplicate
            if (ModuleName.ToUpper() != "PMM")
            {
                menu.Items.Add("Duplicate", "Duplicate", useNewBlackIcon ? "/Content/images/icons/duplicate-new.png" : "/Content/images/duplicate-black.png");
            }

            //// Quick Close
            if (MTicketStatus != TicketStatus.Closed && moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.AllowBatchClose]) && (isAdmin || isHelpDesk))
                menu.Items.Add("Quick Close", "QuickClose", "/Content/images/reject16X16_black.png");

            // (Re)assign
            if (MTicketStatus != TicketStatus.Closed && moduleRow != null && UGITUtility.StringToBoolean(moduleRow[DatabaseObjects.Columns.AllowReassignFromList]))
                menu.Items.Add("(Re)assign", "Reassign", "/Content/images/ReAssign_black.png");

            // Re-Open ticket from close stage
            if (MTicketStatus == TicketStatus.Closed)
                menu.Items.Add("(Re)open", "Reopen", "/Content/ButtonImages/return.png");

            // Project Similarity & project plan
            if (ModuleName == "PMM")
            {
                menu.Items.Add("Project Similarity", "ProjectSimilarity", "/Content/images/newBaseline-black.png");
                menu.Items.Add("Project Plan", "ProjectPlan", "/Content/images/GanttChart.png");
                menu.Items.Add("Excel Import ", "ExcelImport", "/Content/images/GanttChart.png");
                importTitle = string.Format("Import {0}", uHelper.GetModuleTitle(this.ModuleName)); //removed s from title bcz in case of opportunity it will show opportunitys
                importUrl = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlImport, "importexcelfile", "PMM"));
            }

            // import allocations
            if (ModuleName == ModuleNames.CPR || ModuleName == ModuleNames.CNS)
            {
                menu.Items.Add("Compare Projects", "ProjectSimilarity", useNewBlackIcon ? "/Content/images/icons/compare-icon.png" : "/Content/images/newBaseline-black.png");
                if (HasAccessToCreateTemplate)
                {
                    menu.Items.Add("Create Template", "CreateTemplate", "/Content/images/icons/template.png");
                }
                //menu.Items.Add("Allocate Resource", "ImportAllocations", "/Content/images/importtasks.png");
            }
            if (ModuleName == ModuleNames.OPM)
            {
                menu.Items.Add("Compare Opportunities", "ProjectSimilarity", useNewBlackIcon ? "/Content/images/icons/compare-icon.png" : "/Content/images/newBaseline-black.png");
                if (HasAccessToCreateTemplate)
                {
                    menu.Items.Add("Create Template", "CreateTemplate", "/Content/images/icons/template.png");
                }
            }
            //SpDelta 42(Implementation of asset/ticket import)
            //ConfigurationVariableManager ObjConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
            UGITModule currentUGITModule = _moduleViewManager.LoadByName(this.ModuleName, true);
            // Excel Import
            if (this.ModuleName == "CMDB" && currentUGITModule != null && currentUGITModule.EnableTicketImport && UserManager.IsUserinGroups(_configurationVariableManager.GetValue(ConfigConstants.AssetAdmin), _context.CurrentUser.UserName))
            {
                ModuleExcelImport excelImport = new ModuleExcelImport();
                if (excelImport.ModuleImportRunning(this.ModuleName))
                {
                    string importMsg = string.Format("Import Running: {0}%", excelImport.ModuleImportPercentageComplete(this.ModuleName));
                    menu.Items.Add(importMsg, "", "/Content/images/upload-icon.png");
                }
                else
                    menu.Items.Add("Excel Asset Import", "ExcelImport", "/Content/images/upload-icon.png");
            }
            else if (currentUGITModule != null && currentUGITModule.EnableTicketImport && (UserManager.IsAdmin(HttpContext.Current.CurrentUser()) || UserManager.IsUserinGroups(_configurationVariableManager.GetValue(ConfigConstants.TicketAdminGroup), _context.CurrentUser.UserName)))
            {
                if (!(ModuleName.ToUpper() == "CNS" || ModuleName.ToUpper() == "CPR" || ModuleName.ToUpper() == "OPM"))
                {
                    ModuleExcelImport excelImport = new ModuleExcelImport();
                    if (excelImport.ModuleImportRunning(this.ModuleName))
                    {
                        string importMsg = string.Format("Import Running: {0}%", excelImport.ModuleImportPercentageComplete(this.ModuleName));
                        menu.Items.Add(importMsg, "", "/Content/images/upload-icon.png");
                    }
                    else
                        menu.Items.Add(string.Format("Excel {0} Import", UGITUtility.moduleTypeName(this.ModuleName)), "ExcelImport", "/Content/images/upload-icon.png");
                }
            }
            // App -CMDB Relationship Import
            ////if ((this.ModuleName == ModuleNames.CMDB || this.ModuleName == ModuleNames.APP) && isAdmin)
            ////{
            ////    string menuItemTitle = this.ModuleName == ModuleNames.CMDB ? "Application Relationship Import" : "Asset Relationship Import";
            ////    AppCMDBImportTitle = menuItemTitle;
            ////    ModuleExcelImport excelImport = new ModuleExcelImport();
            ////    if (excelImport.ModuleImportRunning(this.ModuleName))
            ////    {
            ////        string importMsg = string.Format("Import Running: {0}%", excelImport.ModuleImportPercentageComplete(this.ModuleName));
            ////        menu.Items.Add(importMsg, "", "/Content/images/import.png");
            ////    }
            ////    else
            ////        menu.Items.Add(menuItemTitle, "AppCMDBRelationShipImport", "/Content/images/import.png");
            ////}
            //
            if (MTicketStatus != TicketStatus.Closed && (isAdmin || isHelpDesk))
            {
                DevExpress.Web.MenuItem putonholditem = new DevExpress.Web.MenuItem("Put on Hold", "Putonhold", "/Content/images/lock-black.png");
                //putonholditem.ItemStyle.BackgroundImage.ImageUrl = "/Content/images/firstnavbgRed1X28.png";
                putonholditem.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                //putonholditem.ItemStyle.ForeColor = Color.Red;
                menu.Items.Add(putonholditem);

                DevExpress.Web.MenuItem putonunholditem = new DevExpress.Web.MenuItem("Remove Hold", "PutonUnhold", "/Content/images/unlock-black.png");
                //putonunholditem.ItemStyle.BackgroundImage.ImageUrl = "/Content/images/firstnavbgGreen1X28.png";
                putonunholditem.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                //putonunholditem.ItemStyle.ForeColor = ColorTranslator.FromHtml("#5DE9BF");
                menu.Items.Add(putonunholditem);
            }
            if ((ModuleName == ModuleNames.CPR || ModuleName == ModuleNames.CNS || ModuleName == ModuleNames.OPM) && (isAdmin || isHelpDesk))
            {
                DevExpress.Web.MenuItem createProjectTags = new DevExpress.Web.MenuItem("Update Project Tags", "UpdateTags", "/Content/images/tag-black.png");
                createProjectTags.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                menu.Items.Add(createProjectTags);
            }
            if (ModuleName == ModuleNames.CPR && (isAdmin || isHelpDesk))
            {
                //DevExpress.Web.MenuItem createProjectStat = new DevExpress.Web.MenuItem("Update Project Statistics", "UpdateStatistics", "/Content/images/graph_stat.png");
                //createProjectStat.ItemStyle.BackgroundImage.Repeat = BackgroundImageRepeat.RepeatX;
                //menu.Items.Add(createProjectStat);
            }
            if (Request.QueryString != null && Request.QueryString.AllKeys.Length > 0)
            {
                if (Convert.ToString(Request.QueryString["control"]) == "problemreportdrildowndata")
                {
                    lnkbtnActionMenu.Visible = false;
                }
            }
            else
            {
                if (menu.Items.Count > 0)
                    lnkbtnActionMenu.Visible = true;
            }

        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            PreRenderData();
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
            //FieldConfigurationManager fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            //string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
            //if (!string.IsNullOrEmpty(value))
            //{
            //    e.Cell.Text = value;
            //}

            //FieldConfigurationManager Fieldmanger = new FieldConfigurationManager(_context);

            string values = string.Empty;

            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Attachments)
            {
                values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                if (!string.IsNullOrEmpty(values))
                {
                    e.Cell.Text = string.Format("<img src='{0}'></img>", "/Content/images/attach.png");
                }
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketPriorityLookup)
            {
                //values = FieldConfigurationManager.GetFieldConfigurationData(e.DataColumn.FieldName, Convert.ToString(e.GetValue(e.DataColumn.FieldName)));
                //if (!string.IsNullOrEmpty(values))
                //{
                //    e.Cell.Text = UGITUtility.GetPriorityStatus(values, e.Cell.HorizontalAlign);
                //}
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.RelationshipType)
            {
                //values = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.RelationshipTypeLookup, Convert.ToString(e.GetValue(DatabaseObjects.Columns.RelationshipTypeLookup)));
                //if (!string.IsNullOrEmpty(values))
                //{
                //    e.Cell.Text = values;
                //}
            }
            else if (e.DataColumn.FieldName == DatabaseObjects.Columns.TicketRequestor)
            {

                values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                if (moduleColumns != null)
                    moduleColumn = moduleColumns.FirstOrDefault(x => x.FieldName == e.DataColumn.FieldName);
                //FieldConfigurationManager Fieldmanger = new FieldConfigurationManager(context);
                if (moduleColumn != null && !string.IsNullOrEmpty(moduleColumn.ColumnType) && (moduleColumn.ColumnType.Equals("MultiUser") || moduleColumn.ColumnType.Equals("UserField")))//FieldConfigurationManager.GetFieldByFieldName(e.DataColumn.FieldName) != null
                {
                    var userlist = UGITUtility.ConvertStringToList(values, Constants.Separator6);
                    var userPicture = "";
                    string userPictureUrl = "";
                    string userName = "";
                    var sb = new System.Text.StringBuilder();
                    if (userlist.Count > 1)
                    {
                        foreach (var user in userlist)
                        {
                            var userProfile = UserManager.GetUserById(user);
                            if (userProfile != null)
                            {
                                userName = FieldConfigurationManager.GetFieldConfigurationData(e.DataColumn.FieldName, user, moduleColumn: moduleColumn);
                                sb.Append(userName).Append(",");
                            }
                        }

                        if (sb.Length > 0)
                            userName = sb.Remove(sb.Length - 1, 1).ToString();

                        if (!string.IsNullOrEmpty(userName))
                        {
                            userPicture += $"<table class='RequestdByMultiUser_Table'><tr><td class='multiUserImg_wrap'><img src='/Content/Images/MultiUser.png' onerror= 'this.src =\"/Content/Images/MultiUser.png\"' title='{userName}' style ='border-radius: 50%; height: 25px; width: 25px; margin-right: 10px;'></img></td>" +
                                $"<td><p class='multi_requester_name'>" + userName + "</p></td></tr></table>";
                        }
                    }
                    else
                    {
                        foreach (var user in userlist)
                        {
                            var userProfile = UserManager.GetUserById(user);
                            if (userProfile != null)
                            {
                                userName = FieldConfigurationManager.GetFieldConfigurationData(e.DataColumn.FieldName, user, moduleColumn: moduleColumn);
                                if (!string.IsNullOrEmpty(userName))
                                {
                                    userPictureUrl = userProfile.Picture;
                                    userPicture += $"<img src='{userPictureUrl}' onerror= 'this.src =\"/Content/Images/People94X94.png\"' title='{userName}' style ='border-radius: 50%; height: 25px; width: 25px; margin-right: 10px;' class='grid_img'></img> " + userName;
                                }
                            }
                        }

                    }

                    e.Cell.Text = userPicture;
                    e.Cell.HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (grid.JSProperties.ContainsKey("cpRowCount"))
                grid.JSProperties["cpRowCount"] = resultedTable != null ? resultedTable.Rows.Count : 0;
            else
                grid.JSProperties.Add("cpRowCount", resultedTable != null ? resultedTable.Rows.Count : 0);


        }

        protected void grid_pageChanged(object sender, EventArgs e)
        {

        }

        protected void CardView_HtmlCardPrepared(object sender, ASPxCardViewHtmlCardPreparedEventArgs e)
        {
            string func = string.Empty;
            string TitleUrl = string.Empty;
            string path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=ugitmodulewebpart&isreadonly=true&Module=" + ModuleName;
            DataRow currentRow = CardView.GetDataRow(e.VisibleIndex);
            string cardViewCss = string.Empty;
            if (currentRow == null)
                return;

            //string ticketId = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));
            string ticketId = string.Empty;
            if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.TicketId))
                ticketId = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));

            string title = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
            string pretitle = string.Empty;
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.CRMProjectID))
            {
                if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.CRMProjectID) && !string.IsNullOrEmpty(Convert.ToString(currentRow[DatabaseObjects.Columns.CRMProjectID])))
                    pretitle = Convert.ToString(currentRow[DatabaseObjects.Columns.CRMProjectID]);
                else if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.EstimateNo) && !string.IsNullOrEmpty(Convert.ToString(currentRow[DatabaseObjects.Columns.EstimateNo])))
                    pretitle = Convert.ToString(currentRow[DatabaseObjects.Columns.EstimateNo]);
                else
                    pretitle = ticketId;
            }
            else
                pretitle = ticketId;

            if (!string.IsNullOrWhiteSpace(title))
                title = string.Format("{0}: {1}", pretitle, UGITUtility.TruncateWithEllipsis(title, 100, string.Empty));
            else
                title = pretitle;

            title = UGITUtility.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
            title = title.Replace("\r\n", ""); // Embedded newlines in title prevent popups from opening

            if (!string.IsNullOrEmpty(ticketId))
            {
                //e.Card.Attributes.Add("onclick", string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}');", (UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath)), string.Format("TicketId={0}", ticketId), title, sourceURL, (string.IsNullOrEmpty(this.PopupWidth) ? "97" : this.PopupWidth), (string.IsNullOrEmpty(this.PopupHeight) ? "97" : this.PopupHeight)));
                TitleUrl = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}');", (UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath)), string.Format("TicketId={0}", ticketId), title, sourceURL, (string.IsNullOrEmpty(this.PopupWidth) ? "97" : this.PopupWidth), (string.IsNullOrEmpty(this.PopupHeight) ? "97" : this.PopupHeight));
            }

            if (currentRow["ResourceAllocationCount"] != DBNull.Value)
            {
                path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=0&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + ticketId + "&module=" + ModuleName;
                func = string.Format("javascript:UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path, string.Format("moduleName={0}&ConfirmBeforeClose=true", ModuleName), title, sourceURL);
                //e.Card.Attributes.Add("onClick", func);
                string[] rvalues = UGITUtility.SplitString(currentRow["ResourceAllocationCount"], Constants.Separator);
                int totalAllocation = int.Parse(rvalues[0]);
                string allocations = string.Empty;
                int unAllocatedResource = int.Parse(rvalues[1]);
                string allocationClass = string.Empty;
                int filledAllocation = totalAllocation - unAllocatedResource;
                if (filledAllocation > 0)
                {
                    if (totalAllocation == 0 && !uHelper.HideAllocationTemplate(_context) && (ModuleName == "CPR" || ModuleName == "OPM" || ModuleName == "CNS"))
                    {
                        string popupTitle = "Add Resource Allocations to " + (ModuleName == "CPR" ? "Project" : ModuleName == "OPM" ? "Opportunity" : "Service");
                        path = NewOPMWizardPageUrl + "&ticketId=" + ticketId + "&module=" + ModuleName + "&selectionmode=NewAllocatonsFromProjects&title=" + title;
                        func = string.Format("javascript:UgitOpenPopupDialog('{0}','{1}','{2}','95','95',false,'{3}');", path, "", popupTitle, sourceURL);
                        e.Card.Attributes.Add("onClick", func);
                    }                    
                    if (filledAllocation == totalAllocation)
                    {
                        //allocationClass = "greenCircle";
                        allocationClass = "blueCircle";
                        allocations = string.Format("<span class='blueCircle{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, ShowCompactView ? "-c" : "");
                    }
                    else
                    {
                        allocationClass = "redCircle";
                        allocations =  string.Format("<span class='redCircle{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, ShowCompactView ? "-c" : "");
                    }
                }
                else
                {
                    allocationClass = "redCircle";
                    allocations = string.Format("<span class='redCircle{3} quickedit jqtooltip' title='Allocate Resource\n{0} Allocations, {2} Unfilled'>{1}/{0}</span>", totalAllocation, filledAllocation, unAllocatedResource, ShowCompactView ? "-c" : "");
                }
                e.Card.Attributes.Add("titleClick", func.ToString());
                e.Card.Attributes.Add("totalAllocation", allocations.ToString());
                e.Card.Attributes.Add("allocationClass", allocationClass);
                e.Card.Attributes.Add("TitleURL", TitleUrl.ToString());
            }
            if (currentRow["StageStep"] != DBNull.Value)
            {
                cardViewCss = this.GetCardOuterBoxCss(ModuleName, HttpContext.Current.CurrentUser(), int.Parse(currentRow["StageStep"].ToString()));
            }

            e.Card.Style.Add("cursor", "pointer");
            e.Card.CssClass += " cardouterbox " + cardViewCss;
        }

        protected void CardView_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            if (btnCardView.Visible == true)
                return;

            if (moduleColumns.Count <= 0)
                return;

            if (string.IsNullOrEmpty(e.Column.FieldName))
                return;

            string values = string.Empty;
            DataRow currentRow = CardView.GetDataRow(e.VisibleIndex);
            string ticketId = string.Empty;
            if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.TicketId))
                ticketId = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));

            string title = Convert.ToString(CardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));
            string pretitle = string.Empty;
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.CRMProjectID))
            {
                if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.CRMProjectID) && !string.IsNullOrEmpty(Convert.ToString(currentRow[DatabaseObjects.Columns.CRMProjectID])))
                    pretitle = Convert.ToString(currentRow[DatabaseObjects.Columns.CRMProjectID]);
                else if (UGITUtility.IfColumnExists(currentRow, DatabaseObjects.Columns.EstimateNo) && !string.IsNullOrEmpty(Convert.ToString(currentRow[DatabaseObjects.Columns.EstimateNo])))
                    pretitle = Convert.ToString(currentRow[DatabaseObjects.Columns.EstimateNo]);
                else
                    pretitle = ticketId;
            }
            else
                pretitle = ticketId;

            if (!string.IsNullOrWhiteSpace(title))
                title = string.Format("{0}: {1}", pretitle, UGITUtility.TruncateWithEllipsis(title, 100, string.Empty));
            else
                title = pretitle;

            title = UGITUtility.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
            title = title.Replace("\r\n", ""); // Embedded newlines in title prevent popups from opening

            if (e.Value != DBNull.Value)
            {
                //values = FieldConfigurationManager.GetFieldConfigurationData(e.Column.FieldName, Convert.ToString(e.Value));
                //if (!string.IsNullOrEmpty(values))
                //{
                //    e.DisplayText = values;
                //}
            }
            e.DisplayText = string.Format("<div onclick=\"openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}');\">{6}</div>", (UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath)), string.Format("TicketId={0}", ticketId), title, sourceURL, (string.IsNullOrEmpty(this.PopupWidth) ? "97" : this.PopupWidth), (string.IsNullOrEmpty(this.PopupHeight) ? "97" : this.PopupHeight), e.Value);
            e.EncodeHtml = false;
            if (e.Column.FieldName.Equals(DatabaseObjects.Columns.Title))
            {
                e.DisplayText = $"<div class='cardtitle'>{e.Value}</div>";
                e.EncodeHtml = false;
                e.Column.Caption = string.Empty;

            }
            if (e.Column.FieldName.Equals(DatabaseObjects.Columns.ERPJobID))
            {
                e.DisplayText = $"<div class='cardticket'>{e.Value}</div>";
                e.EncodeHtml = false;
                e.Column.Caption = string.Empty;
            }
            string columnType = moduleColumns.FirstOrDefault(x => x.FieldName == e.Column.FieldName).ColumnType;
            if (columnType.EqualsIgnoreCase("currency"))
            {
                double amount;
                if (double.TryParse(Convert.ToString(e.Value), out amount))
                {
                    e.DisplayText = string.Format("{0:C}", amount);
                }
            }
            else if (columnType.EqualsIgnoreCase("number"))
            {
                int number;
                if (int.TryParse(Convert.ToString(e.Value), out number))
                {
                    e.DisplayText = string.Format("{0:n0}", number);
                }
            }
            else if (columnType.EqualsIgnoreCase("date") || columnType.EqualsIgnoreCase("datetime"))
            {
                DateTime dt;
                if (DateTime.TryParse(Convert.ToString(e.Value), out dt))
                {
                    if (columnType.EqualsIgnoreCase("date"))
                        e.DisplayText = string.Format("{0:MMM-dd-yyyy}", dt);
                    else if (columnType.EqualsIgnoreCase("datetime"))
                        e.DisplayText = string.Format("{0:MMM-dd-yyyy HH:mm:ss}", dt);
                }
            }

        }
        /// <summary>
        /// Get card view css based on const, precon or closed.
        /// </summary>
        /// <param name="currentModuleName"></param>
        /// <param name="user"></param>
        /// <param name="stagStem"></param>
        /// <returns></returns>
        public string GetCardOuterBoxCss(string currentModuleName, UserProfile user, int stagStep) 
        {
            Ticket TicketRequest = new Ticket(_context, currentModuleName, user);
            LifeCycle lifeCycle = TicketRequest.Module.List_LifeCycles.FirstOrDefault();
            List<LifeCycleStage> allStages = lifeCycle.Stages;
            int resolvedStageStep = 0;
            int closedStageStep = 0;
            LifeCycleStage resolvedStage = allStages.FirstOrDefault(x => x.StageTypeChoice == StageType.Resolved.ToString());
            LifeCycleStage closedStage = allStages.FirstOrDefault(x => x.StageTypeChoice == StageType.Closed.ToString());
            if (resolvedStage != null)
                resolvedStageStep = resolvedStage.StageStep;
            if (closedStage != null)
                closedStageStep = closedStage.StageStep;

            if (stagStep < resolvedStageStep)
            {
                return "preconClass";
            }
            else if (stagStep >= resolvedStageStep && stagStep < closedStageStep)
            {
                if (stagStep + 1 == closedStageStep)
                {
                    return "constClass-1";
                }
                return "constClass";
            }
            else
            {
                return "clousOutClass";
            }
        }
        protected void btnGridView_Click(object sender, EventArgs e)
        {
            btnGridView.Visible = false;
            btnCardView.Visible = true;

            grid.Visible = true;
            CardView.Visible = false;
        }

        protected void btnCardView_Click(object sender, EventArgs e)
        {
            btnGridView.Visible = true;
            btnCardView.Visible = false;

            grid.Visible = false;
            CardView.Visible = true;
        }

        protected void CardView_HeaderFilterFillItems(object sender, ASPxCardViewHeaderFilterEventArgs e)
        {
        }

        protected void grid_CustomFilterExpressionDisplayText(object sender, CustomFilterExpressionDisplayTextEventArgs e)
        {
            //string operatorstring = e.DisplayText.ToString();
            ////CriteriaOperator oper = e.Criteria as CriteriaOperator;
            ////BinaryOperator operatorObj = CriteriaOperator.Parse(e.Criteria.ToString(), out OperandValue[] criteriaParametersList) as BinaryOperator;
            ////GroupOperator operatorObj1 = CriteriaOperator.Parse(e.Criteria.ToString()) as GroupOperator;
            //MatchCollection userguids = Regex.Matches(operatorstring, @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}");
            //Dictionary<string, string> userkeycollection = new Dictionary<string, string>();

            //for (int i = 0; i < userguids.Count; i++)
            //{
            //    string matchId = userguids[i].Value; //Set Match to the value from the match
            //    UserProfile filteruser = UserManager.GetUserById(matchId);
            //    if (filteruser != null)
            //    {
            //        userkeycollection.Add(filteruser.Id, filteruser.Name);
            //    }
            //}
            //foreach (var item in userkeycollection)
            //{
            //    operatorstring = operatorstring.Replace(item.Key, item.Value);
            //}
            //e.DisplayText = operatorstring;
        }

        protected void ExportPopupMenu_Load(object sender, EventArgs e)
        {
            ExportPopupMenu.Items.Clear();
            ASPxPopupMenu menu = sender as ASPxPopupMenu;
            menu.ItemImage.Width = 18;
            menu.Items.Add("Excel", "Excel", "/Content/images/icons/excel-icon.png");
            menu.Items.Add("Pdf", "Pdf", "/Content/images/icons/pdf-icon.png");

            if (ModuleName.ToUpper() == "CNS" || ModuleName.ToUpper() == "CPR" || ModuleName.ToUpper() == "OPM")
            {
                menu.Items.Add("Link", "CopyLinktoClipboard", "/Content/images/icons/link-icon.png");
                if (moduleRow != null && UGITUtility.StringToBoolean(Convert.ToString(moduleRow[DatabaseObjects.Columns.AllowEscalationFromList]))) // && (isAdmin || isHelpDesk))
                {
                    menu.Items.Add("Email", "email", "/Content/images/icons/email-icon-black.png");
                }
            }
        }

        protected void ExportPopupMenu_ItemClick(object source, MenuItemEventArgs e)
        {
            if (e.Item.Name == "Pdf")
            {
                PdfExportOptions options = new PdfExportOptions();
                options.Compressed = false;

                grid.SettingsExport.PaperKind = System.Drawing.Printing.PaperKind.A2;
                grid.SettingsExport.Landscape = true;
                grid.SettingsExport.SplitDataCellAcrossPages = false;
                grid.SettingsExport.FileName = "Export_" + ModuleName;
                foreach (var item in grid.Columns)
                {
                    if (item.GetType() == typeof(GridViewDataTextColumn))
                    {
                        GridViewDataColumn column = item as GridViewDataColumn;
                        if (column.FieldName == DatabaseObjects.Columns.Attachments)
                            column.Visible = false;
                        if (column.Caption.StartsWith("<IMG"))
                            column.Caption = column.FieldName;
                    }
                }
                //grid.Columns[DatabaseObjects.Columns.SelfAssign].Visible = false;
                //grid.Columns[DatabaseObjects.Columns.Attachments].Visible = false;
                grid.ExportPdfToResponse(options);
            }
        }

        public string GenerateSummaryIcon(DataRow currentTicket, string projectID)
        {
            //ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(_context);
            List<ProjectEstimatedAllocation> projectEstimatedAllocation = projectAllocations.Where(x => x.TicketId == projectID).ToList();
            //ProjectEstimatedAllocation projectEstimatedAllocation = projectAllocations.Where(x => x.TicketId == projectID).FirstOrDefault();
            bool isAllocInPrecon = false, isAllocInConst = false, isAllocInCloseOut = false;
            string oText = string.Empty;
            if (currentTicket != null && projectEstimatedAllocation != null)
            {
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconStartDate) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.PreconEndDate))
                {
                    DateTime preconStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconStartDate]);
                    DateTime preconEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.PreconEndDate]);

                    if (preconEnd != DateTime.MinValue && preconStart != DateTime.MinValue)
                    {
                        isAllocInPrecon = projectEstimatedAllocation.Any(o => o.AllocationStartDate <= preconEnd && o.AllocationEndDate >= preconStart);
                    }
                }
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionStart) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.EstimatedConstructionEnd))
                {
                    DateTime constStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    DateTime constEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]);
                    if (constStart != DateTime.MinValue && constEnd != DateTime.MinValue)
                    {
                        isAllocInConst = projectEstimatedAllocation.Any(o => o.AllocationStartDate <= constEnd && o.AllocationEndDate >= constStart);
                    }
                }
                if (UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutStartDate) &&
                    UGITUtility.IfColumnExists(currentTicket, DatabaseObjects.Columns.CloseoutDate))
                {
                    DateTime closesoutEnd = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutDate]);
                    DateTime closesoutStart = UGITUtility.StringToDateTime(currentTicket[DatabaseObjects.Columns.CloseoutStartDate]);
                    if (closesoutEnd == DateTime.MinValue && closesoutStart != DateTime.MinValue)
                    {
                        closesoutEnd = closesoutStart.AddWorkingDays(CloseOutPeriod);
                    }
                    if (closesoutEnd != DateTime.MinValue && closesoutStart != DateTime.MinValue)
                    {
                        isAllocInCloseOut = projectEstimatedAllocation.Any(o => o.AllocationStartDate <= closesoutEnd && o.AllocationEndDate >= closesoutStart);
                    }
                }
            }
            oText = string.Format("<div class='alloctype'><i class='{0}' style='margin-right:5px;font-size: 17px; color:#52BED9'></i>" +
                                "<i class='{1}' style='margin-right:5px;font-size: 17px; color:#005C9B'></i>" +
                                "<i class='{2}' style='font-size: 17px; color:#351B82'></i></div>",
                                isAllocInPrecon ? "fa fa-circle" : "far fa-circle",
                                isAllocInConst ? "fa fa-circle" : "far fa-circle",
                                isAllocInCloseOut ? "fa fa-circle" : "far fa-circle");
            return oText;
        }

        public string GenerateCompanyDetailsIcon(string title, DataRow currentRow, DataTable resultedTable)
        {
            string oText = string.Empty;
            string comIconUrl = string.Empty;
            string iconTitle = string.Empty;
            //oText = string.Format("<img src='/Content/Images/WritingPad.png' style='height:45px;width:65px;' class='prpUserProfile' title='Expand All' />");
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.IconBlob) && !string.IsNullOrEmpty(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.IconBlob])))
            {                
                object imageBytes = UGITUtility.GetSPItemValue(currentRow, DatabaseObjects.Columns.IconBlob);
                if (imageBytes != null && imageBytes != DBNull.Value)
                {
                    comIconUrl = "data:image;base64," + Convert.ToBase64String((byte[])imageBytes);
                }
                oText = string.Format("<img src=\"" + comIconUrl + "\" style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; color: white; font-weight: 600;\"/>");
            }
            else if (resultedTable.Rows.Count > 0 && ModuleName == ModuleNames.CON)
            {
                DataTable TicketId= resultedTable.AsEnumerable().Any(o => o.Field<string>("CRMCompanyLookup") == UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.CRMCompanyLookup]))
                        ? resultedTable.AsEnumerable().Where(o => o.Field<string>("CRMCompanyLookup") == UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.CRMCompanyLookup])).Distinct().CopyToDataTable()
                        : null;
                if (TicketId != null && TicketId.Rows[0]["CRMCompanyLookup"] != null)
                {
                    Ticket ticket = new Ticket(_context, ModuleNames.COM);
                    DataRow CompanyTicket = Ticket.GetCurrentTicket(_context, ModuleName, TicketId.Rows[0]["CRMCompanyLookup"].ToString());
                    if (CompanyTicket != null)
                    {
                        object imageBytes = CompanyTicket[DatabaseObjects.Columns.IconBlob];
                        if (imageBytes != null && imageBytes != DBNull.Value)
                        {
                            comIconUrl = "data:image;base64," + Convert.ToBase64String((byte[])imageBytes);
                        }
                        oText = string.Format("<img src=\"" + comIconUrl + "\" style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                        "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; color: white; font-weight: 600;\"/>");
                    }
                    else if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.CRMCompanyLookup])))
                    {
                        iconTitle = AcronymTitle(UGITUtility.ObjectToString(currentRow["CRMCompanyTitle"]));
                        oText = string.Format("<span class=\"ProfilePic\" id=\"companyTitle\" style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                        "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; padding: 18px 0px 0px 0px; color: white; font-weight: 600;" +
                        "font-size: 12px;\">" + iconTitle.ToString().ToUpper() + "</span>");
                    }
                    else
                    {
                        iconTitle = AcronymTitle(title);
                        if (iconTitle != null)
                        {
                            oText = string.Format("<span class=\"ProfilePic\" id=\"companyTitle\" style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                            "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; padding: 18px 0px 0px 0px; color: white; font-weight: 600;" +
                            "font-size: 12px;\">" + iconTitle.ToString().ToUpper() + "</span>");
                        }
                        else
                        {
                            oText = string.Format("<img src='/Content/Images/Project.jpg' style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                            "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; color: white; font-weight: 600;\"/>");
                        }
                    }
                }
                else
                {
                    oText = string.Format("<img src='/Content/Images/Project.jpg' style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                            "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; color: white; font-weight: 600;\"/>");
                }
            }
            else 
            {
                iconTitle = AcronymTitle(title);
                if (iconTitle != null)
                {
                    oText = string.Format("<span class=\"ProfilePic\" id=\"companyTitle\" style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                    "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; padding: 18px 0px 0px 0px; color: white; font-weight: 600;" +
                    "font-size: 12px;\">" + iconTitle.ToString().ToUpper() + "</span>");
                }
                else
                {
                    oText = string.Format("<img src='/Content/Images/Project.jpg' style=\"display: block; width: 50px !important; height: 50px !important; border-radius: 50%" +
                    "; font-size: larger; font-family: 'Roboto', sans-serif !important; background-color: #789CCE; color: white; font-weight: 600;\"/>");
                }                
            }
            
            return oText;
        }

        protected void ASPxCallbackPanel_Actions_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter != null)
            {
                if (ModuleName != ModuleNames.CPR)
                    return;
                string[] query = e.Parameter.Split('&');
                string action = query[0].Split('=')[1];
                List<string> ticketIDs = query[1].Split('=')[1].Split(',').ToList();
                if (action == "UpdateStatistics")
                {
                    _statisticsManager.ProcessStatistics(ticketIDs, _moduleViewManager.LoadByName(this.ModuleName));
                }
            }
        }

        public string AcronymTitle(string title)
        {
            string oText = string.Empty;
            StringBuilder iconTitle = new StringBuilder();
            string pattern = "[^a-zA-Z]";

            if (!string.IsNullOrEmpty(title))
            {
                string[] splitTitle = title.Trim().Split(' ');
                foreach (var item in splitTitle)
                {
                    if (item != "")
                    {
                        string clearTitle = Regex.Replace(item, pattern, "");
                        if (!string.IsNullOrEmpty(clearTitle))
                        {
                            iconTitle.Append(clearTitle.Substring(0, 1));
                        }
                    }
                }
                oText = iconTitle.ToString();
            }
            return oText;
        }

    }
}
