using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public class DatabaseObjects
    {
        /// <summary>
        /// List Names
        /// </summary>
        /// 
        public static class Tables
        {
            public const string TenantRegistration = "TenantRegistration";
            public const string GlobalRole = "Roles";
            public const string ProjectAllocationTemplates = "ProjectAllocationTemplates";
            public const string FieldConfiguration = "FieldConfiguration";
            public const string MailTokenColumnName = "Config_MailTokenColumnName";
            public const string ModuleFormTab = "Config_Module_ModuleFormTab";
            public const string ModuleStages = "Config_Module_ModuleStages";
            public const string RequestRoleWriteAccess = "Config_Module_RequestRoleWriteAccess";
            public const string FormLayout = "Config_Module_FormLayout";
            public const string ModuleColumns = "Config_Module_ModuleColumns";
            public const string Modules = "Config_Modules";
            public const string RequestType = "Config_Module_RequestType";
            public const string TicketImpact = "Config_Module_Impact";
            public const string TicketSeverity = "Config_Module_Severity";
            public const string RequestPriority = "Config_Module_RequestPriority";
            public const string ModuleWorkflowHistory = "ModuleWorkflowHistory";
            public const string ModuleDefaultValues = "Config_Module_DefaultValues";
            public const string TaskEmails = "Config_Module_TaskEmails";
            public const string ModuleMonitors = "Config_ModuleMonitors";
            public const string ModuleMonitorOptions = "Config_ModuleMonitorOptions";
            public const string ProjectMonitorState = "ProjectMonitorState";

            public const string ProjectClass = "config_ProjectClass";
            public const string ProjectInitiative = "Config_ProjectInitiative";
            public const string ACRTypes = "ACRTypes";
            public const string Tenant = "Tenant";
            public const string DRQSystemAreas = "DRQSystemAreas";
            public const string DRQRapidTypes = "DRQRapidTypes";
            public const string ProjectMonitorStateHistory = "ProjectMonitorStateHistory";
            public const string ModuleMonitorOptionsHistory = "ModuleMonitorOptionsHistory";

            public const string TicketTemplates = "Templates";
            public const string NPRRequest = "NPR";
            public const string NPRTasks = "NPRTasks";
            public const string UserInfo = "AspNetUsers";
            public const string Emails = "Emails";
            public const string SessionIdConstant = "_uGIT_Id";
            public const string DashboardSummary = "DashboardSummary";
            public const string GenericTicketStatus = "GenericStatus";
            public const string TicketPriority = "Config_Module_Priority";
            public const string SLARule = "Config_Module_SLARule";
            public const string ChartFormula = "ChartFormula";
            public const string StageType = "Config_Module_StageType";
            // public const string ChartDimensions = "ChartDimensions";
            // public const string ChartDimensions2 = "ChartDimensions2";
            //  public const string ChartExpression = "ChartExpression";
            public const string ChartFilters = "ChartFilters";
            public const string TicketHours = "TicketHours";
            public const string ChartTemplates = "ChartTemplates";
            public const string ConfigurationVariable = "Config_ConfigurationVariable";
            public const string State = "State";
            public const string ModuleUserTypes = "Config_Module_ModuleUserTypes";
            public const string TicketRelation = "TicketRelation";
            public const string TicketRelationship = "Relationship";
            public const string TicketRelationships = "Relationships";
            public const string SWQuestions = "SWQuestions";
            public const string SWChoices = "SWChoices";
            public const string RequestCategories = "RequestCategories";
            public const string ResourceAllocation = "ResourceAllocation";
            public const string ResourceTimeSheet = "ResourceTimeSheet";
            public const string ResourceWorkItems = "ResourceWorkItems";
            public const string PMMProjects = "PMM";
            public const string ResourceUsageSummaryWeekWise = "ResourceUsageSummaryWeekWise";
            public const string ResourceUsageSummaryMonthWise = "ResourceUsageSummaryMonthWise";
            public const string ResourceProjectComplexity = "Summary_ResourceProjectComplexity";
            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
            public const string TicketEvents = "TicketEvents";
            //
            public const string Services = "Config_Services";
            public const string ServiceUpdates_Master = "ServiceUpdates_Master";
            public const string ServiceSections = "Config_Service_ServiceSections";
            public const string ServiceTickets = "ServiceTickets";
            public const string ServiceTicketRelationships = "ServiceTicketRelationships";
            public const string ServiceTicketDefaultValues = "Config_Service_ServiceDefaultValues";
            public const string ServiceQuestions = "Config_Service_ServiceQuestions";
            public const string ServiceVariables = "ServiceVariables";
            public const string ServiceCategories = "Config_Service_ServiceCategories";
            public const string PRSTicket = "PRS";
            public const string SVCRequests = "SVCRequests";
            public const string ClientAdminConfigurationLists = "Config_ClientAdminConfigurationLists";
            public const string ProjectStageHistory = "ProjectStageHistory";

            // add by shiv for Escalation
            public const string TicketEscalationQueue = "TicketEscalationQueue";
            public const string EscalationRule = "Config_Module_EscalationRule";
            public const string TicketStatusMapping = "Config_Module_StatusMapping";
            public const string EscalationLog = "EscalationLog";
            public const string HolidayAndWorkingHours = "HolidaysAndWorkDaysCalendar";

            public const string BudgetCategories = "Config_BudgetCategories";
            public const string NPRBudget = "NPRBudget";
            public const string NPRMonthlyBudget = "NPRMonthlyBudget";

            public const string ITGMonthlyBudget = "ITGMonthlyBudget";
            public const string DashboardPanels = "Config_Dashboard_DashboardPanels";
            public const string ITGovernance = "ITGovernance";
            public const string DashboardFactTables = "Config_Dashboard_DashboardFactTables";
            public const string CheckListTaskStatus = "CheckListTaskStatus";
            public const string CheckLists = "CheckLists";
            public const string CheckListRoles = "CheckListRoles";
            public const string CheckListRoleTemplates = "CheckListRoleTemplates";
            public const string CheckListTasks = "CheckListTasks";
            public const string CheckListTemplates = "CheckListTemplates";
            public const string RankingCriteriaMaster = "Config_Master_RankingCriteria";
            public const string LeadRanking = "LeadRanking";
            public const string LeadCriteria = "Config_LeadCriteria";
            public const string ProjectComplexity = "Config_ProjectComplexity";
            public const string CheckListTaskTemplates = "CheckListTaskTemplates";
            public const string SubContractor = "SubContractor";


            //add by Piyush
            public const string Department = "Department";
            public const string Location = "Location";
            public const string Assets = "Assets";
            public const string AssetReferences = "AssetReferences";
            public const string AssetVendors = "AssetVendors";
            public const string AssetModels = "AssetModels";
            public const string AssetRelations = "AssetRelations";
            public const string LeftSideNavigation = "LeftSideNavigation";
            public const string TSRTicket = "TSR";
            public const string TicketsByMail = "TicketsByMail";
            public const string MailedTickets = "MailedTicket";

            // PMM
            public const string PMMTasks = "PMMTasks";
            public const string PMMBudget = "PMMBudget";
            public const string PMMMonthlyBudget = "PMMMonthlyBudget";
            public const string PMMComments = "PMMComments";
            public const string PMMIssues = "PMMIssues";
            public const string PMMBudgetActuals = "PMMBudgetActuals";
            public const string ModuleBudgetActuals = "ModuleBudgetActuals";
            public const string ModuleBudgetActualsHistory = "ModuleBudgetActualsHistory";



            public const string AssetIncidentRelations = "AssetIncidentRelations";

            //PMMBaseline
            public const string ModuleTasksHistory = "ModuleTasksHistory";
            public const string PMMBudgetHistory = "PMMBudgetHistory";
            public const string PMMMonthlyBudgetHistory = "PMMMonthlyBudgetHistory";
            public const string PMMCommentsHistory = "PMMCommentsHistory";
            public const string PMMIssuesHistory = "PMMIssuesHistory";
            public const string BaseLineDetails = "BaseLineDetails";
            public const string PMMHistory = "PMMHistory";

            public const string GlobalFilterPerDashboard = "GlobalFilterPerDashboard";
            public const string DashboardPanelGroup = "DashboardPanelGroup";
            public const string SprintTasks = "SprintTasks";
            public const string Sprint = "Sprint";
            public const string ProjectReleases = "ProjectReleases";
            public const string SprintSummary = "SprintSummary";
            public const string ModuleUserStatistics = "ModuleUserStatistics";
            public const string ProjectAllocations = "ProjectPlannedAllocation";
            // Incidents 
            public const string INCTicket = "INCTicket";

            //Project Summary Report Gantt Chart
            public const string ProjectSummary = "ProjectSummary";

            //TSK
            public const string TSKProjects = "TSK";
            public const string TSKTasks = "TSKTasks";

            //ITG
            public const string ITGBudget = "ITGBudget";
            public const string ITGActual = "ITGActual";
            public const string Company = "Company";
            public const string CompanyDivisions = "CompanyDivisions";

            public const string DashboardPanelView = "Config_Dashboard_DashboardPanelView";
            public const string ClientAdminCategory = "Config_ClientAdminCategory";
            //Functional Area
            public const string FunctionalAreas = "FunctionalAreas";
            public const string EmailQueue = "EmailQueue";
            public const string UserRoles = "Config_UserRoles";
            public const string RequestTypeByLocation = "RequestTypeByLocation";
            public const string MenuNavigation = "Config_MenuNavigation";
            public const string MenuNavigationMobile = "MenuNavigationMobile";
            public const string MessageBoard = "MessageBoard";
            public const string PageEditor = "Config_PageConfiguration";

            //Added by Sachin
            public const string GovernanceLinkCategory = "GovernanceLinkCategory";
            public const string GovernanceLinkItems = "GovernanceLinkItems";

            ///Agent Job Schduler
            public const string SchedulerActions = "SchedulerActions";

            public const string SchedulerActionArchives = "SchedulerActionArchives";
            public const string SurveyFeedback = "SurveyFeedback";
            public const string AnalyticDashboards = "AnalyticDashboards";

            ///Wiki
            public const string WikiArticles = "WikiArticles";
            public const string WikiDiscussion = "WikiDiscussion";
            public const string WikiLinks = "WikiLinks";
            public const string WikiReview = "WikiReview";
            public const string WikiCategory = "Config_WikiLeftNavigation";
            public const string WikiReviews = "WikiReviews";
            public const string WikiContents = "WikiContents";
            public const string WikiMenuLeftNavigation = "WikiMenuLeftNavigation";

            //HelpCard HELP
            public const string HelpCard = "HelpCard";
            public const string HelpCardContent = "HelpCardContent";

            ///Application Management
            public const string Applications = "Applications";
            public const string ApplicationModules = "ApplicationModules";
            public const string ApplicationRole = "ApplicationRole";
            public const string ApplModuleRoleRelationship = "ApplicationAccess";
            public const string Environment = "Environment";
            public const string ApplicationServers = "ApplicationServers";
            public const string ApplicationPassword = "ApplicationPassword";

            //Module Stage Preconditions
            public const string ModuleStageConstraints = "ModuleStageConstraints";
            public const string ProjectLifeCycles = "Config_ModuleLifeCycles";
            public const string ProjectLifeCycleStages = "ProjectLifeCycleStages";
            public const string UGITTaskTemplates = "TaskTemplates";       /*"UGITTaskTemplates";*/ // change by mayank singh 31/01/2017
            public const string TaskTemplateItems = "TaskTemplateItems";
            public const string ModuleStageConstraintTemplates = "ModuleStageConstraintTemplates";
            public const string ACRTicket = "ACRTicket";
            public const string BTSTicket = "BTSTicket";
            public const string DRQTicket = "DRQTicket";
            public const string NPRTicket = "NPRTicket";
            //Document Management
            public const string PortalInfo = "PortalInfo";
            public const string DocTypeInfo = "DocTypeInfo";
            //public const string WorkflowTask = "WorkflowTask";
            public const string WorkflowHistory = "Workflow History";
            public const string PingInformation = "PingInformation";
            public const string History = "History";
            public const string ConfigurationList = "DMConfigTable";
            public const string DocumentWorkflowHistory = "DocumentWorkflowHistory";

            public const string DMDepartment = "DMDepartment";
            public const string Vendor = "DMVendor";
            public const string DocumentType = "DMDocumentTypeList";
            public const string DocumentInfoList = "DMDocumentInfo";
            public const string Projects = "DMProjects";
            public const string Tags = "DMTags";
            public const string DocumentHistory = "DMDocumentHistory";
            public const string DocumentLinkList = "DMDocumentLink";
            public const string UserSkills = "UserSkills";
            public const string UserCertificates = "UserCertificates";
            public const string ResourceAllocationMonthly = "ResourceAllocationMonthly";

            public const string EventCategories = "Config_EventCategories";
            public const string PMMEvents = "PMMEvents";
            public const string ModuleWorkflowHistoryArchive = "ModuleWorkflowHistory_Archive";
            public const string HolidaysAndWorkDaysCalendar = "HolidaysAndWorkDaysCalendar";

            public const string ADUserMapping = "ADUserMapping";
            public const string TicketEmails = "TicketEmails";
            public const string UGITLog = "Log";  //UGITLog changed by mayank singh
            public const string WikiLeftNavigation = "Config_WikiLeftNavigation";
            public const string UserInformationList = "AspNetUsers";
            public const string Contracts = "Contracts";
            public const string NPRResources = "NPRResources";
            public const string adminauth = "adminauth";
            public const string VendorSOWInvoiceDetail = "VendorSOWInvoiceDetail";
            public const string VendorSOWFees = "VendorSOWFees";
            public const string VendorSOWContImprovement = "VendorSOWContImprovement";
            public const string VendorResourceCategory = "VendorResourceCategory";
            public const string VendorServiceDuration = "VendorServiceDuration";
            public const string VendorSLAPerformance = "VendorSLAPerformance";
            public const string VendorReport = "VendorReport";
            public const string VendorMSAMeeting = "VendorMSAMeeting";
            public const string VendorSOW = "VendorSOW";
            public const string VendorSLA = "VendorSLA";
            public const string VendorMSA = "VendorMSA";

            //Investment Management
            public const string InvDistribution = "InvDistribution";
            public const string Investments = "Investments";
            public const string Investors = "Investors";

            //BCS: External List
            public const string DepartmentExternal = "DepartmentExternal";
            public const string UserExternal = "UserExternal";
            public const string LinkView = "LinkView";
            public const string LinkCategory = "LinkCategory";
            public const string LinkItems = "LinkItems";

            public const string VendorSOWInvoices = "VendorSOWInvoices";
            public const string VendorIssues = "VendorIssues";

            //Asset Management
            public const string ImageSoftware = "ImageSoftware";
            public const string AssetsStatus = "AssetsStatus";
            public const string VendorRisks = "VendorRisks";
            public const string PMMRisks = "PMMRisks";
            public const string VendorApprovedSubcontractors = "VendorApprovedSubcontractors";
            public const string VendorKeyPersonnel = "VendorKeyPersonnel";

            ///CRM: Customer Related Lists
            public const string PLCRequest = "PLCRequest";
            public const string Customers = "Customers";
            public const string ModuleTasks = "ModuleTasks";

            //LEM:Lead Management
            public const string Lead = "Lead";


            //COM:Company Management
            public const string CRMCompany = "CRMCompany";

            //OPM: Oppurtunity Management
            public const string Opportunity = "Opportunity";
            public const string Organization = "Organization";
            public const string Contacts = "Contacts";
            public const string Resources = "Resources";
            public const string MonthlyBudget = "MonthlyBudget";
            public const string Bid = "Bid";
            public static string CRMContact = "CRMContact";
            public const string CRMActivities = "CRMActivities";
            public const string CRMRelationshipType = "CRMRelationshipType";
            public static string CRMEstimate = "CRMEstimate";
            public const string CRMProjectAllocation = "ProjectEstimatedAllocation";
            //RelatedCompanies
            public const string RelatedCompanies = "RelatedCompanies";

            public const string CRMProject = "CRMProject";

            //TicketToEmail
            public const string EmailToTicket = "EmailToTicket";
            //VND
            public const string VendorReportInstance = "VendorReportInstance";
            public const string VendorPOLineItems = "VendorPOLineItems";
            public const string VendorPO = "VendorPO";
            public const string ImageSoftwareMap = "ImageSoftwareMap";
            //Resource Management
            public const string ResourceManagement = "ResourceManagement";


            public const string setugittheme = "setugittheme";
            public const string ComposedLooks = "Composed Looks";

            public const string SiteAssets = "Site Assets";
            public const string VendorVPM = "VendorVPM";
            public const string TicketWorkflowSLASummary = "WorkflowSLASummary";

            //TicketEmailFooter list
            public const string TicketEmailFooter = "TicketEmailFooter";
            public const string EmailFooter = "EmailFooter";
            public const string Themes = "Theme Gallery";
            public const string EnableMigrate = "EnableMigrate";
            public const string ModuleAgent = "ModuleAgent";
            public const string AssetIntegrationConfiguration = "AssetIntegrationConfiguration";
            public const string AspNetUsers = "AspNetUsers";
            public const string AspNetUserRoles = "AspNetUserRoles";
            public const string AspNetUserLogins = "AspNetUserLogins";
            public const string AspNetUserClaims = "AspNetUserClaims";
            public const string AspNetRoles = "aspnetroles";
            public const string UserProfile = "UserProfile";
            public const string VendorType = "VendorType";
            public const string TabView = "Config_TabView";
            public const string ProjectSimilarityConfig = "ProjectSimilarityConfig";
            public const string ProjectSimilarityMetrics = "ProjectSimilarityMetrics";

            public const string InformationSchema = "INFORMATION_SCHEMA.COLUMNS";
            public const string Appointment = "Appointment";
            public const string UsersRoles = "UserRoles";
            public const string LandingPages = "LandingPages";

            public static string ProcoreMapping = "ProcoreMapping";
            public static string ProcoreFieldsMapping = "ProcoreFieldsMapping";

            // added by mayank new //
            public const string ModuleMonthlyBudget = "ModuleMonthlyBudget";
            public const string ModuleBudget = "ModuleBudget";
            public const string ModuleBudgetHistory = "ModuleBudgetHistory";
            public const string ModuleMonthlyBudgetHistory = "ModuleMonthlyBudgetHistory";
            public const string PageConfiguration = "Config_PageConfiguration";
            public const string SubLocation = "SubLocation";
            public const string Documents = "Documents";
            public const string Report_ConfigData = "Report_ConfigData";
            public const string DecisionLog = "DecisionLog";
            public const string DecisionLogHistory = "DecisionLogHistory";

            //DMS Tables
            public const string DMSAccess = "DMSAccess";
            public const string DMSAspnetApplications = "[DMSAspnetApplications]";
            public const string AspnetMembership = "aspnet_Membership";
            public const string AspnetDMSRoles = "aspnet_Roles";
            public const string AspnetDMSUsers = "aspnet_Users";
            public const string AspnetDMSUsersInRoles = "aspnet_UsersInRoles";
            public const string DMSCustomer = "DMSCustomer";
            public const string DMSDirectory = "DMSDirectory";
            public const string DMSDocument = "DMSDocument";
            public const string DMSFileAuditLog = "DMSFileAuditLog";
            public const string DMSUsersFilesAuthorization = "DMSUsersFilesAuthorization";
            public const string DMSTenantDocumentsDetails = "DMSTenant_Documents_Details";
            public const string DMDocumentTypeList = "DMDocumentTypeList";
            //Phrase table
            public const string Phrase = "Phrase";
            //Agents table
            public const string Agents = "Agents";
            // RMM
            public const string ResourceTimeSheetSignOff = "ResourceTimeSheetSignOff";
            public const string ProjectStandardWorkItems = "ProjectStandardWorkItems";
            public const string ReportMenu = "ReportMenu";
            

            // Run In Background Task Status
            public const string BackgroundProcessStatus = "BackgroundProcessStatus";
            public const string BusinessStrategy = "BusinessStrategy";
            public const string TicketCountTrends = "TicketCountTrends";
            public const string EmployeeTypes = "EmployeeTypes";

            public const string Analytic_AnalysisDashboards = "Analytic_AnalysisDashboards";
            public const string Analytic_DashboardGroups = "Analytic_DashboardGroups";
            public const string Analytic_DashboardModelInputs = "Analytic_DashboardModelInputs";
            public const string Analytic_DataIntegrations = "Analytic_DataIntegrations";
            public const string Analytic_ETSchemaDrafts = "Analytic_ETSchemaDrafts";
            public const string Analytic_ETSchemaInfoes = "Analytic_ETSchemaInfoes";
            public const string Analytic_ETTables = "Analytic_ETTables";
            public const string Analytic_ModelCategories = "Analytic_ModelCategories";
            public const string Analytic_ModelFeatureOutputs = "Analytic_ModelFeatureOutputs";
            public const string Analytic_ModelInputs = "Analytic_ModelInputs";
            public const string Analytic_ModelOutputMappers = "Analytic_ModelOutputMappers";
            public const string Analytic_ModelOutputs = "Analytic_ModelOutputs";
            public const string Analytic_Models = "Analytic_Models";
            public const string Analytic_ModelSectionOutputs = "Analytic_ModelSectionOutputs";
            public const string Analytic_ModelVersions = "Analytic_ModelVersions";
            public const string Analytic_Questions = "Analytic_Questions";
            public const string Analytic_SideLinks = "Analytic_SideLinks";
            public const string Analytic_ModelSubSectionOutputs = "Analytic_ModelSubSectionOutputs";
            public const string Analytic_Interpretations = "Analytic_Interpretations";
            public const string MyAssets = "MyAssets";
            public const string Studio = "Studio";
            public const string ExperiencedTags = "ExperiencedTags";
            public const string UserProjectExperience = "UserProjectExperience";

            //Statistics
            public const string StatisticsConfiguration = "StatisticsConfiguration";
            public const string Statistics = "Statistics";
        }

        public static class Columns
        {
            public static string HostingTypeChoice = "HostingTypeChoice";
            public static string FrequencyOfTypicalUse = "FrequencyOfTypicalUse";
            public static string FrequencyOfUpgradesNotes = "FrequencyOfUpgradesNotes";
            public static string FrequencyOfUpgradesChoice = "FrequencyOfUpgradesChoice";
            public static string NextPlannedMajorUpgrade = "NextPlannedMajorUpgrade";
            public static string NextUpgradeDate = "NextUpgradeDate";
            public static string Numberofseats = "Numberofseats";
            
            public static string SoftwareMajorVersion = "SoftwareMajorVersion";
            public static string SoftwareMinorVersion = "SoftwareMinorVersion";
            public static string SoftwarePatchRevision = "SoftwarePatchRevision";
            

            public const string TotalActualHours = "TotalActualHours";
            public const string VersionLatestRelease = "VersionLatestRelease";
            public const string LicenseBasisChoice = "LicenseBasisChoice";
            public const string ResolvedByUser = "ResolvedByUser";
            public const string Attachments = "Attachments";
            public const string DivisionManagerUser = "DivisionManagerUser";
            public const string AdjustHijriDays = "AdjustHijriDays";
            public const string AfterHours = "AfterHours";
            public const string Age = "Age";
            public static string IsPerformanceTestingDone = "IsPerformanceTestingDone";
            public const string AltCalendarType = "AltCalendarType";
            public const string AppLifeCycleChoice = "AppLifeCycleChoice";
            public const string Approver2User = "Approver2User";
            public const string Archived = "Archived";
            public const string AssetCondition = "AssetCondition";
            public const string AssignedAnalyst = "AssignedAnalyst";
            public const string AssignedByUser = "AssignedByUser";
            public const string AuthenticationChoice = "AuthenticationChoice";
            public const string AutoFillRequestor = "AutoFillRequestor";
            public const string BackedUpComponentsChoice = "BackedUpComponentsChoice";
            public const string BottomPaneExpanded = "BottomPaneExpanded";
            public const string BreakEvenIn = "BreakEvenIn";
            public const string BreakFix = "BreakFix";
            public const string BudgetLookup = "BudgetLookup";
            public const string BulkRequestCount = "BulkRequestCount";
            public const string BusinessManager2User = "BusinessManager2User";
            public const string CalendarType = "CalendarType";
            public const string CalendarViewOptions = "CalendarViewOptions";
            public const string CellPhoneNumber = "CellPhoneNumber";
            public const string ChargeBackAmount = "ChargeBackAmount";
            public const string ChildTicketId = "ChildTicketId";
            public const string ApproverUser = "ApproverUser";
            
            public const string ClassificationScope = "ClassificationScope";
            public const string ClassificationSizeChoice = "ClassificationSizeChoice";
            public const string ClientLookup = "ClientLookup";
            public const string CloseDate = "CloseDate";
            public const string ClosedByUser = "ClosedByUser";
            
            public const string ContentLanguages = "ContentLanguages";
            public const string ContentTypeDisp = "ContentTypeDisp";
            public const string ContractLookup = "ContractLookup";
            public const string ContractStatusChoice = "ContractStatusChoice";
            public const string ContractTitleLookup = "ContractTitleLookup";
            public const string ContributionToStrategy = "ContributionToStrategy";
            public const string CostSavings = "CostSavings";
            public const string CreationDate = "CreationDate";
            public const string CurrentFundingProject = "CurrentFundingProject";
            public const string CurrentGWOFields = "CurrentGWOFields";
            public const string CustomerBenefit = "CustomerBenefit";
            public const string CustomerProgram = "CustomerProgram";
            public const string DepartmentManagerUser = "DepartmentManagerUser";
            public const string DisableDiscussion = "DisableDiscussion";
            public const string DisableRelatedItems = "DisableRelatedItems";
            public const string DiscussionLastUpdated = "DiscussionLastUpdated";
            public const string DiscussionTitleLookup = "DiscussionTitleLookup";
            
            public const string DnsNameList = "DnsNameList";
            public const string EditUser = "EditUser";
            public const string ElevatedPriority = "ElevatedPriority";
            public const string EliminatesHeadcount = "EliminatesHeadcount";
            public const string EnableTaskReminder = "EnableTaskReminder";
            public const string EnableZoomIn = "EnableZoomIn";
            public const string EndOfExtendedSupportDate = "EndOfExtendedSupportDate";
            public const string EndOfLifeDate = "EndOfLifeDate";
            public const string EndOfSaleDate = "EndOfSaleDate";
            public const string EndOfSecurityUpdatesDate = "EndOfSecurityUpdatesDate";
            public const string EndOfSoftwareMaintenanceDate = "EndOfSoftwareMaintenanceDate";
            public const string EndOfSupportDate = "EndOfSupportDate";
            public const string EnhancedKeyUsageList = "EnhancedKeyUsageList";
            public const string Extensions = "Extensions";
            public const string ExternalID = "ExternalID";
            public const string ExternalType = "ExternalType";
            public const string FCRCategorization = "FCRCategorization";
            public const string FileLocation = "FileLocation";
            public const string FinalCountermeasure = "FinalCountermeasure";
            public const string Firmware = "Firmware";
            public const string FriendlyName = "FriendlyName";
            public const string GroupEdit = "GroupEdit";
            public const string GroupLink = "GroupLink";
            public const string Handle = "Handle";
            public const string HasPrivateKey = "HasPrivateKey";
            public const string HighLevelRequirements = "HighLevelRequirements";
            public const string ImnName = "ImnName";
            public const string ImpactBusinessGrowth = "ImpactBusinessGrowth";
            public const string ImpactBusinessGrowthChoice = "ImpactBusinessGrowthChoice";
            public const string ImpactDecisionMakingChoice = "ImpactDecisionMakingChoice";
            public const string ImpactIncreasesProductivityChoice = "ImpactIncreasesProductivityChoice";
            public const string ImpactReducesExpensesChoice = "ImpactReducesExpensesChoice";
            public const string ImpactReducesRiskChoice = "ImpactReducesRiskChoice";
            public const string ImpactRevenueIncreaseChoice = "ImpactRevenueIncreaseChoice";
            public const string InStock = "InStock";
            public const string InternalCapability = "InternalCapability";
            public const string InternalCapabilityChoice = "InternalCapabilityChoice";
            public const string IsActive = "IsActive";
            public const string IsDefault = "IsDefault";
            public const string IsITGApprovalRequired = "IsITGApprovalRequired";
            
            public const string IsQuestion = "IsQuestion";
            public const string IsSiteAdmin = "IsSiteAdmin";
            public const string IsSteeringApprovalRequired = "IsSteeringApprovalRequired";
            public const string IssuerName = "IssuerName";
            public const string IssueTypeChoice = "IssueTypeChoice";
            public const string ITLifecycleRefreshChoice = "ITLifecycleRefreshChoice";
            public const string ITManager2User = "ITManager2User";
            public const string LastReplyByUser = "LastReplyByUser";
            public const string LeftPaneExpanded = "LeftPaneExpanded";
            public const string LocalAdmins = "LocalAdmins";
            public const string Locale = "Locale";
            public const string ManufacturingContact = "ManufacturingContact";
            public const string MatchAllKeywords = "MatchAllKeywords";
            public const string ModuleStep = "StageStep";
            public const string MUILanguages = "MUILanguages";
            public const string MyEditor = "MyEditor";
            public const string Name = "Name";
            public const string NameWithPicture = "NameWithPicture";
            public const string NameWithPictureAndDetails = "NameWithPictureAndDetails";
            public const string NextMilestoneDate = "NextMilestoneDate";
 
            public const string NextStandardReviewDate = "NextStandardReviewDate";
            public const string NonStandardConfiguration = "NonStandardConfiguration";
            public const string NotAfter = "NotAfter";
            public const string NotBefore = "NotBefore";
            public const string Notes = "Notes";
            public const string NotificationDisabled = "NotificationDisabled";
            public const string NumUsers2 = "NumUsers2";
            public const string OnHold = "OnHold";
            public const string OnHoldReason = "OnHoldReason";
            public const string OnHoldReasonChoice = "OnHoldReasonChoice";
            public const string OnHoldStartDate = "OnHoldStartDate";
            public const string OnHoldTillDate = "OnHoldTillDate";
            public const string OrderNum = "OrderNum";
            public const string OrganizationalImpactChoice = "OrganizationalImpactChoice";
            public const string ORPUser = "ORPUser";
            public const string OtherDescribe = "OtherDescribe";
            public const string OwnerUser = "OwnerUser";
            public const string OwnerUser2User = "OwnerUser2User";
            public const string OwnerUserApprovalRequired = "OwnerUserApprovalRequired";
            public const string PackingListNumber = "PackingListNumber";
            public const string ParentId = "ParentId";
            public const string ParentTicketId = "ParentTicketId";
            public const string PaybackCostSavings = "PaybackCostSavings";
            public const string PaymentTerms = "PaymentTerms";
            public const string PctROI = "PctROI";
            public const string PictureDisp = "PictureDisp";
            public const string PMMIdLookup = "PMMIdLookup";
            public const string PODate = "PODate";
            public const string Predecessor = "Predecessor";
            public const string ProductCode = "ProductCode";
            public const string ProductionCritical = "ProductionCritical";
            public const string ProductName = "ProductName";
            public const string ProductReleaseDate = "ProductReleaseDate";
            public const string ProjectApplicationDecision = "ProjectApplicationDecision";
            public const string ProjectApplicationStatusChoice = "ProjectApplicationStatusChoice";
            public const string ProjectCode = "ProjectCode";
            public const string ProjectConstraints = "ProjectConstraints";
            public const string ProjectContractDecision = "ProjectContractDecision";
            public const string ProjectCoordinators = "ProjectCoordinators";
            public const string ProjectDataDecision = "ProjectDataDecision";
            public const string ProjectDataStatusChoice = "ProjectDataStatusChoice";
            public const string ProjectDeliverables = "ProjectDeliverables";
            public const string ProjectDirector = "ProjectDirector";
            public const string ProjectEstDurationMaxDays = "ProjectEstDurationMaxDays";
            public const string ProjectEstDurationMinDays = "ProjectEstDurationMinDays";
            public const string ProjectEstSizeMaxHrs = "ProjectEstSizeMaxHrs";
            public const string ProjectEstSizeMinHrs = "ProjectEstSizeMinHrs";
            public const string ProjectState = "ProjectState";
            public const string PRPUser = "PRPUser";
            public const string PublicKey = "PublicKey";
            public const string Quantity = "Quantity";
            public const string SubLocationLookup = "SubLocationLookup";
            public const string Quantity2 = "Quantity2";
            public const string QuestionID = "QuestionID";
            public const string QuestionProperties = "QuestionProperties";
            public const string QuoteAmount = "QuoteAmount";
            public const string RawData = "RawData";
            public const string RCADisabled = "RCADisabled";
            public const string RCARequested = "RCARequested";
            public const string Regulatory = "Regulatory";
            public const string RegulatoryChoice = "RegulatoryChoice";
            public const string Rejected = "Rejected";
            public const string RelatedItems = "RelatedItems";
            public const string ReleaseDate = "ReleaseDate";
            public const string ReleaseID = "ReleaseID";
            public const string ReopenCount = "ReopenCount";
            public const string RequestorUserContactSLA = "RequestorUserContactSLA";
            public const string RequestTypeCategory = "RequestTypeCategory";
            public const string RequestTypeSubCategory = "RequestTypeSubCategory";
            public const string ResolutionDate = "ResolutionDate";
            
            public const string SalesRepContact = "SalesRepContact";
            public const string SalesRepName = "SalesRepName";
            public const string SecurityDescription = "SecurityDescription";
            public const string SendAsTrustedIssuer = "SendAsTrustedIssuer";
            public const string ServiceLookup = "ServiceLookup";
            public const string ServiceQuestionTitleLookup = "ServiceQuestionTitleLookup";
            public const string ShortestThreadIndexIdLookup = "ShortestThreadIndexIdLookup";
            public const string SignatureAlgorithm = "SignatureAlgorithm";
            public const string SipAddress = "SipAddress";
            public const string SLADisabled = "SLADisabled";
            public const string SpecificExclusions = "SpecificExclusions";
            public const string SpecificInclusions = "SpecificInclusions";
            public const string SSLCertExpiration = "SSLCertExpiration";
            public const string SSLCertName = "SSLCertName";
            public const string StandardChoice = "StandardChoice";
            public const string StandardRefreshPeriod = "StandardRefreshPeriod";
            public const string StandardReviewDate = "StandardReviewDate";
            public const string Status = "Status";
            public const string StrategicInitiative = "StrategicInitiative";
            public const string StrategicInitiativeChoice = "StrategicInitiativeChoice";
            public const string Subject = "Subject";
            public const string SubjectName = "SubjectName";
           
            public const string SupplierLookup = "SupplierLookup";
            public const string SupportedBrowsersChoice = "SupportedBrowsersChoice";
            public const string SVCRequestLookup = "SVCRequestLookup";
            public const string TaskActualStartDate = "TaskActualStartDate";
            public const string TechnologyImpact = "TechnologyImpact";
            public const string TechnologyImpactChoice = "TechnologyImpactChoice";
            public const string TechnologyReliability = "TechnologyReliability";
            public const string TechnologyReliabilityChoice = "TechnologyReliabilityChoice";
            public const string TechnologySecurity = "TechnologySecurity";
            public const string TechnologySecurityChoice = "TechnologySecurityChoice";
            public const string TechnologyUsabilityChoice = "TechnologyUsabilityChoice";
            public const string Thumbprint = "Thumbprint";
            public const string Time24 = "Time24";
            public const string TimeZone = "TimeZone";
            
            public const string TotalHoldDuration = "TotalHoldDuration";
            public const string UGITNewUserDisplayName = "UGITNewUserDisplayName";
            public const string Unmanaged = "Unmanaged";
            public const string UserFieldXML = "UserFieldXML";
            public const string UserInfoHidden = "UserInfoHidden";
            public const string UserSelection = "UserSelection";
            public const string VendorContact = "VendorContact";
            public const string VendorSupport = "VendorSupport";
            public const string VersionNumber = "VersionNumber";
            
            public const string WikiDislikedByUser = "WikiDislikedByUser";
            public const string WikiID = "WikiID";
            public const string WikiLikedByUser = "WikiLikedByUser";
            public const string WorkDayEndHour = "WorkDayEndHour";
            public const string WorkDays = "WorkDays";
            public const string WorkDayStartHour = "WorkDayStartHour";


            public const string ProjectName = "ProjectName";
            public const string StandardWorkItem = "StandardWorkItem";
            public const string RankingCriteriaTotal = "RankingCriteriaTotal";
            public const string BusinessStrategyLookup = "BusinessStrategyLookup";
            public const string ProjectCapacity = "ProjectCapacity";
            public const string RevenueCapacity = "RevenueCapacity";
            //public const string BusinessStrategyLookup = "BusinessStrategyLookup";
            public const String AgentSummary = "AgentSummary";
            
            public const string PRP = "PRPUser";
            
            public const string CostCodeLookup = "CostCodeLookup";
            public const string ServerFunctions = "ServerFunctionsChoice";
            public const string RelatedModule = "RelatedModule";
            public const string RelatedTicketID = "RelatedTicketID";
            public const string TemplateName = "TemplateName";
            public const string Data = "Data";
            public const string CurrentUser = "CurrentUser";
            public static string NumUsers = "NumUsers";
            public static string NumLicensesTotal = "NumLicensesTotal";
            public const string DataEditors = "DataEditors";
            public const string ColumnNameSchema = "COLUMN_NAME";
            public const string DATA_TYPE = "DATA_TYPE";
            public const string UGITNewUserName = "NewUserName";
            public const string UGITTitle = "UGITTitle";
            public const string RFSSubmissionDate = "RFSSubmissionDate";
            public const string RFSFormComplete = "RFSFormComplete";
            public const string TargetType = "TargetType";
            public const string GovernanceLinkCategoryLookup = "GovernanceLinkCategoryLookup";
            public const string ID = "ID";
            public const string Id = "Id";
            public const string ManagerID = "ManagerUser";
            public const string TicketAge = "Age";
            public const string TicketDueIn = "DueIn";
            public const string SelfAssign = "SelfAssign";
            public const string Description = "Description";
            //public const string ExternalID = "ExternalID";
            public const string OwnerContractTypeChoice = "OwnerContractTypeChoice";
            public const string TicketId = "TicketId";
            public const string TicketIdWithoutLink = "TicketIdWithoutLink";
            public const string TicketStatus = "Status";
            public static string PreconStartDate = "PreconStartDate";
            public static string PreconEndDate = "PreconEndDate";
            public const string TicketActualStartDate = "ActualStartDate";
            public const string TicketActualCompletionDate = "ActualCompletionDate";
            public const string TicketSecurityManager = "SecurityManagerUser";
            public const string TicketGLCode = "GLCode";
            public const string TicketResolutionComments = "ResolutionComments";
            public const string ResolutionComments = "ResolutionComments";
            public const string TicketAnalysisDetails = "AnalysisDetails";
            public const string TicketResolutionType = "ResolutionTypeChoice";
            public const string TicketOnHold = "OnHold";
            public const string TicketClosedBy = "ClosedByUser";
            public const string TicketResolvedBy = "ResolvedByUser";
            public const string TicketAssignedBy = "AssignedBy";
            public const string TicketAssignedByUser = "AssignedByUser";
            public const string TicketInitiatedDate = "InitiatedDate";
            public const string TicketAssignedDate = "AssignedDate";
            public const string TicketResolvedDate = "ResolvedDate";
            public const string TicketTestedDate = "TestedDate";
            public const string TicketClosedDate = "ClosedDate";
            public const string TicketRejected = "Rejected";
            public const string TicketPriorityLookup = "PriorityLookup";
            public const string TicketRequestTypeLookup = "RequestTypeLookup";
            public const string RequestTypeDescription = "Description";
            public const string LocationDescription = "LocationDescription";
            public const string TicketRequestTypeCategory = "RequestTypeCategory";
            public const string TicketImpactLookup = "ImpactLookup";
            public const string TicketSeverityLookup = "SeverityLookup";
            public const string TicketDesiredCompletionDate = "DesiredCompletionDate";
            public const string TicketTargetCompletionDate = "TargetCompletionDate";
            public const string TicketPctComplete = "PctComplete";
            public const string RequestTypeOwner = "Owner";
            public const string TicketOwner = "OwnerUser";
            public const string TicketTicketDRBRManager = "DRBRManagerUser";
            public const string TicketPRP = "PRPUser";
            public const string TicketORP = "ORPUser";
            public const string TicketTester = "TesterUser";
            public const string TicketBusinessManager = "BusinessManagerUser";
            public const string TicketApplicationManager = "ApplicationManagerUser";
            public const string TicketInitiator = "InitiatorUser";
            public const string TicketRequestor = "RequestorUser";
            public const string TicketComment = "Comment";
            public const string Actuals = "Actuals";
            public const string EmailID = "EMail";
            public const string EmailTitle = "EmailTitle";
            public const string EmailBody = "EmailBody";
            public const string IncludesStaffing = "IncludesStaffing";
            public const string AllowDraftMode = "AllowDraftMode";
            public const string FunctionalAreaLookup = "FunctionalAreaLookup";
            public const string DisableStageExitCriteriaDelete = "DisableStageExitCriteriaDelete";
            public const string Impact = "Impact";
            public const string TechnologyIntegration = "TechnologyIntegration";
            public const string Severity = "Severity";
            public const string Priority = "uPriority";
            public const string TaskPriority = "Priority";
            public const string ApprovalDate = "ApprovalDate";
            public const string TicketStakeHolders = "StakeHoldersUser";
            public const string TicketSponsors = "SponsorsUser";
            public const string TicketProjectScore = "ProjectScore";
            public const string TicketProjectManager = "ProjectManagerUser";
            public const string TicketProjectCoordinators = "ProjectCoordinators";
            public const string TicketProjectDirector = "ProjectDirector";
            public const string ProjectMonitorWeight = "ProjectMonitorWeight";
            public const string ProjectMonitorNotes = "ProjectMonitorNotes";
            public const string ModuleMonitorOptionLEDClassLookup = "ModuleMonitorOptionLEDClassLookup";
            public const string ModuleMonitorOptionLEDClass = "ModuleMonitorOptionLEDClass";
            public const string ModuleMonitorMultiplier = "ModuleMonitorMultiplier";
            public const string TicketBeneficiaries = "BeneficiariesLookup";
            public const string RequestTypeEscalationManager = "EscalationManagerUser";
            public const string RequestTypeBackupEscalationManager = "BackupEscalationManagerUser";
            public const string KeyWords = "KeyWords";
            public const string EstProjectSpend = "EstProjectSpend";
            public const string EstProjectSpendComment = "EstProjectSpendComment";
            public const string isRole = "isRole";
            public const string TicketNPRIdLookup = "NPRIdLookup";
            public const string ModuleMonitorOptionNameLookup = "ModuleMonitorOptionNameLookup";
            public const string ModuleMonitorOptionName = "ModuleMonitorOptionName";
            public const string ModuleMonitorNameLookup = "ModuleMonitorNameLookup";
            public const string ModuleMonitorName = "ModuleMonitorName";
            public const string TicketInfrastructureManager = "InfrastructureManagerUser";
            public const string TicketEstimatedHours = "EstimatedHours";
            public const string TicketDeveloperTotalHours = "DeveloperTotalHours";
            public const string TicketProjectReferenceDescription = "ProjectReferenceDescription";
            public const string TicketStageActionUsers = "StageActionUsersUser";
            public const string TicketStageActionUserTypes = "StageActionUserTypes";

            public const string StageTitle = "StageTitle";
            public const string ShortStageTitle = "ShortStageTitle";
            public const string StageWeight = "StageWeight";
            public const string StageApprovedStatus = "StageApprovedStatus";
            public const string StageRejectedStatus = "StageRejectedStatus";
            public const string StageReturnStatus = "StageReturnStatus";
            public const string CurrentStageStartDate = "CurrentStageStartDate";
            public const string LastSequence = "LastSequence";
            public const string LastSequenceDate = "LastSequenceDate";
            public const string StageApproveButtonName = "StageApproveButtonName";
            public const string StageRejectedButtonName = "StageRejectedButtonName";
            public const string StageReturnButtonName = "StageReturnButtonName";
            public const string ShowBaselineButtons = "ShowBaselineButtons";
            public const string StageAllApprovalsRequired = "StageAllApprovalsRequired";
            public const string StageClosedBy = "StageClosedBy";
            public const string StageClosedByName = "StageClosedByName";
            public const string ApproveActionDescription = "ApproveActionDescription";
            public const string RejectActionDescription = "RejectActionDescription";
            public const string ReturnActionDescription = "ReturnActionDescription";
            public const string SkipOnCondition = "SkipOnCondition";

            public const string RCAType = "RCAType";

            public const string KeepTicketCounts = "KeepTicketCounts";
            public const string ModuleName = "ModuleName";
            public const string MilestoneDescription = "MilestoneDescription";
            public const string MilestoneStartDate = "MilestoneStartDate";
            public const string MilestoneEndDate = "MilestoneEndDate";
            public const string TicketPMMIdLookup = "PMMIdLookup";
            public const string ScrumLifeCycle = "ScrumLifeCycle";
            public const string MilestoneParentId = "MilestoneParentId";
            public const string ReloadCache = "ReloadCache";
            public const string ReturnCommentOptional = "ReturnCommentOptional";
            public const string ModuleId = "ModuleId";
            public const string ModuleTicketTable = "ModuleTable";
            
            public const string Module_Step = "ModuleStep";
            public const string StepLookup = "StepLookup";
            public const string ModuleStepLookup = "ModuleStepLookup";
            public const string ModuleAutoApprove = "ModuleAutoApprove";
            public const string ModuleDescription = "ModuleDescription";
            public const string ModelDescription = "ModelDescription";
            public const string ProjectMonitorName = "ProjectMonitorName";
            public const string MonitorName = "MonitorName";
            public const string ModuleMonitorOption = "ModuleMonitorOption";
            public const string UserPrompt = "UserPrompt";
            public const string AuthorizedToView = "AuthorizedToView";
            public const string AuthorizedToViewUsers = "AuthorizedToViewUsers";
            public const string AuthorizedToEdit = "AuthorizedToEdit";
            public const string AuthorizedToApprove = "AuthorizedToApprove";
            public const string ActionUser = "ActionUser";
            public const string ActionUserType = "ActionUserType";
            public const string EmailUserTypes = "EmailUserTypes";
            public const string UserWorkflowStatus = "UserWorkflowStatus";
            public const string ModuleHoldMaxStage = "ModuleHoldMaxStage";
            public const string FieldMandatory = "FieldMandatory";
            public const string TicketRisk = "Risk";
            public const string TicketName = "Name";
            public const string TicketMonitors = "Monitors";
            public const string DRReplicationChange = "DRReplicationChangeChoice";
            public const string DRQChangeType = "DRQChangeTypeChoice";
            public const string TicketRequestType = "RequestType";
            public const string RequestCategory = "RequestCategory";
            public const string SubCategory = "SubCategory";
            public const string TicketCreationDate = "CreationDate";
            public const string TicketCreatedDate = "Created";
            public const string GenericStatusLookup = "GenericStatusLookup";
            public const string ModuleNameLookup = "ModuleNameLookup";

            public const string ModuleStageMultiLookup = "ModuleStage";
            public const string ShowStageTransitionButtons = "ShowStageTransitionButtons";


            public const string ModuleIdLookup = "ModuleIdLookup";
            public const string GenericStatus = "GenericStatus";
            public const string SLACategory = "SLACategoryChoice";
            public const string SLACategoryChoice = "SLACategoryChoice";
            public const string SLAHours = "SLAHours";
            public const string SLATarget = "SLATarget";
            public const string SLAUnit = "SLAUnit";
            public const string MinThreshold = "MinThreshold";
            public const string MaxThreshold = "MaxThreshold";
            public const string HigherIsBetter = "HigherIsBetter";
            public const string Weightage = "Weightage";
            public const string Reward = "Reward";
            public const string Penalty = "Penalty";

            public const string AllowBatchClose = "AllowBatchClose";
            public const string AllowTicketDelete = "AllowDelete";
            public const string HideWorkFlow = "HideWorkFlow";

            public const string TicketPriority = "PriorityLookup";
            public const string SLAMet = "SLAMet";
            public const string DefaultUser = "DefaultUser";
            public const string TicketRapidRequest = "RapidRequest";
            public const string DRQRapidTypeLookup = "DRQRapidTypeLookup";
            public const string WorkflowType = "WorkflowType";
            public const string EnableWorkflow = "EnableWorkflow";
            public const string EnableLayout = "EnableLayout";
            public const string TicketRequestTypeWorkflow = "RequestTypeWorkflow";

            public const string TabId = "TabId";
            public const string TabName = "TabName";
            public const string TableName = "TableName";
            public const string ColumnName = "ColumnName";
            public const string ColumnValue = "ColumnValue";
            public const string Formula = "Formula";
            public const string FormulaValue = "FormulaValue";
            public const string StageType = "StageTypeChoice";
            public const string StageTypeChoice = "StageTypeChoice";
            public const string Title = "Title";
            public const string LocationTag = "LocationTag";
            public const string Address1 = "Address1";
            public const string Address2 = "Address2";

            public const string UGITSubTaskType = "SubTaskType";
            public const string AutoCreateUser = "AutoCreateUser";
            public const string ErrorMsg = "ErrorMsg";

            public const string Created = "Created";
            public const string TicketDescription = "Description";
            public const string StageTitleLookup = "StageTitleLookup";
            public const string EndStageTitleLookup = "EndStageTitleLookup";
            public const string SLADuration = "SLADuration";
            public const string Duration = "Duration";
            public const string ProjectDuration = "ProjectDuration";
            public const string ModuleStageType = "ModuleStageType";
            public const string StageTypeLookup = "StageTypeLookup";
            public const string ChartObject = "ChartObject";
            public const string ChartTemplateIds = "ChartTemplateIds";
            public const string ValueAsId = "ValueAsId";
            public const string TicketInitiatorResolved = "InitiatorResolvedChoice";
            public const string KeyName = "KeyName";
            public const string KeyValue = "KeyValue";
            public const string ConfigVariableType = "Type";
            public const string ModuleRelativePagePath = "ModuleRelativePagePath";
            public const string UserTypes = "UserTypes";
            public const string TicketProjectReference = "TicketProjectReference";
            public const string ChildId = "ChildID";
            public const string CurrentTicketId = "CurrentTicketID";
            public const string ChildModuleName = "ChildModuleName";
            public const string ParentModuleName = "ParentModuleName";
            public const string ProjectID = "ProjectId";
            public static string CRMProjectID = "ProjectId";
            public const string SWQuestion = "SWQuestion";
            public const string SWQuestionType = "SWQuestionType";
            public const string ThemeColor = "ThemeColor";
            public const string IsTicketAttachment = "isAttachment";
            public const string ItemOrder = "ItemOrder";
            public const string NavigationType = "NavigationType";
            public const string constModulePagePath = "constModulePagePath";
            public const string RequestCategoryType = "RequestCategoryType";
            public const string ContentType = "ContentType";
            public const string WorkItemType = "WorkItemType";
            public const string WorkItem = "WorkItem";
            public const string WorkItemLink = "WorkItemLink";
            public const string ERPJobID = "ERPJobID";
            public const string ERPJobIDNC = "ERPJobIDNC";
            public const string RequestTypeLookup = "RequestTypeLookup";
            public const string Insurance = "Insurance";
            public const string PctAllocation = "PctAllocation";
            public const string ActualPctAllocation = "ActualPctAllocation";
            public const string IsSummaryRow = "IsSummaryRow";
            public const string Resource = "ResourceUser";
            public const string RResource = "Resource";
            public const string AllocationStartDate = "AllocationStartDate";
            public const string AllocationEndDate = "AllocationEndDate";
            public const string Category = "Category";
            public const string CategoryChoice = "CategoryChoice";
            public const string CategoryLabel = "#Category#";
            public const string SubWorkItem = "SubWorkItem";
            public const string SubSubWorkItem = "SubSubWorkItem";
            public const string SubSubWorkItemLink = "SubSubWorkItemLink";
            public const string WorkDate = "WorkDate";
            public const string WorkDescription = "WorkDescription";
            public const string HoursTaken = "HoursTaken";
            //public const string IsDeleted = "Deleted";
            public const string IsDeletedColumn = "IsDeleted";
            public const string History = "History";
            public const string ResourceWorkItemLookup = "ResourceWorkItemLookup";
            public const string TicketActualHours = "ActualHours";
            public const string ResourceId = "ResourceId";
            public const string ModuleType = "ModuleType";
            public const string EnableRMMAllocation = "EnableRMMAllocation";
            public const string EnableEventReceivers = "EnableEventReceivers";
            public const string WorkItemID = "WorkItemID";
            public const string Manager = "ManagerUser";
            public const string WeekStartDate = "WeekStartDate";
            public const string AllocationHour = "AllocationHour";
            public const string ActualHour = "ActualHour";
            public const string PctActual = "PctActual";
            public const string MonthStartDate = "MonthStartDate";
            public const string SignOffStatus = "SignOffStatus";
            public const string IsVariable = "IsVariable";
            public const string ServiceTicketTitleLookup = "ServiceTicketTitleLookup";
            public const string ServiceTitleLookup = "ServiceLookup";
            public const string ServiceSectionsTitleLookup = "ServiceSectionsTitleLookup";
           
            public const string TicketSVCRequestLookup = "SVCRequestLookup";
            public const string AskUser = "AskUser";
            public const string PickValueFrom = "PickValueFrom";
            public const string WebPartHelpText = "WebPartHelpText";

            public const string RequestSource = "RequestSourceChoice";
            public const string InitiatorResolved = "InitiatorResolved";
            public const string TicketReSubmissionDate = "ReSubmissionDate";
            public const string TicketTotalHoldDuration = "TotalHoldDuration";
            public const string TicketOnHoldStartDate = "OnHoldStartDate";

            public const string ShowEditButton = "ShowEditButton";
            public const string ShowPartialEdit = "ShowPartialEdit";
            public const string ShowWithCheckBox = "ShowWithCheckBox";
            public const string ShowWithCheckbox = "ShowWithCheckbox";
            public const string FieldDisplayWidth = "FieldDisplayWidth";
            public const string FieldDisplayName = "FieldDisplayName";
            public const string TemplateType = "TemplateType";
            public const string WebpartID = "WebpartID";
            public const string IsShowInSideBar = "IsShowInSideBar";
            public const string FontStyle = "FontStyle";
            public const string HeaderFontStyle = "HeaderFontStyle";
            public const string DashboardPanelInfo = "DashboardPanelInfo";
            public const string DashboardPermission = "DashboardPermissionUser";
            public const string DashboardDescription = "DashboardDescription";
            public const string Modified = "Modified";
            public const string DashboardType = "DashboardType";
            public const string ListName = "ListName";
            public const string IsHideTitle = "IsHideTitle";
            public const string IsHideDescription = "IsHideDescription";
            public const string PanelWidth = "PanelWidth";
            public const string PanelHeight = "PanelHeight";
            public const string ITStaff = "ITStaff";
            public const string ITConsultant = "ITConsultant";
            public const string TicketITManager = "ITManager";
            public const string ITManagerUser = "ITManagerUser";
            public const string TicketTotalCost = "TotalCost";
            public const string TicketTotalCostsNotes = "TotalCostsNotes";
            public const string TicketTotalStaffHeadcount = "TotalStaffHeadcount";
            public const string TicketTotalStaffHeadcountNotes = "TotalStaffHeadcountNotes";
            public const string TicketTotalConsultantHeadcount = "TotalConsultantHeadcount";
            public const string TicketTotalConsultantHeadcountNotes = "TotalConsultantHeadcountNotes";
            public const string TicketRiskScore = "RiskScore";
            public const string TicketRiskScoreNotes = "RiskScoreNotes";
            public const string TicketArchitectureScore = "ArchitectureScore";
            public const string TicketArchitectureScoreNotes = "ArchitectureScoreNotes";
            public const string TicketNoOfFTEs = "NoOfFTEs";
            public const string TicketNoOfConsultants = "NoOfConsultants";
            public const string TicketConstraintNotes = "ConstraintNotes";
            public const string TicketNoOfFTEsNotes = "NoOfFTEsNotes";
            public const string TicketProjectScoreNotes = "ProjectScoreNotes";
            public const string BudgetAcronym = "BudgetAcronym";
            public const string ProjectClassLookup = "ProjectClassLookup";
            public const string ProjectInitiativeLookup = "ProjectInitiativeLookup";

            public const string TicketClassification = "ClassificationChoice";
            public const string TicketClassificationType = "ClassificationTypeChoice";
            public const string TicketClassificationImpact = "ClassificationImpact";

            public const string DisplayForClosed = "DisplayForClosed";
            public const string DisplayForReport = "DisplayForReport";
            public const string ColumnType = "ColumnType";
            public const string UserRoleType = "UserRoleType";

            public const string CustomProperties = "CustomProperties";
            public const string LocationMultLookup = "LocationMultLookup";
            public const string TicketProjectScope = "ProjectScope";
            public const string TicketProjectAssumptions = "ProjectAssumptions";
            public const string TicketProjectBenefits = "ProjectBenefits";
            public const string ProjectRiskNotes = "ProjectRiskNotes";
            public const string ProblemBeingSolved = "ProblemBeingSolved";

            public const string TicketTotalOffSiteConsultantHeadcount = "TotalOffSiteConsultantHeadcount";
            public const string TicketTotalOnSiteConsultantHeadcount = "TotalOnSiteConsultantHeadcount";
            public const string ModuleMonitorOptionIdLookup = "ModuleMonitorOptionIdLookup";
            public const string ProjectNote = "ProjectNote";
            public const string ProjectNoteType = "ProjectNoteType";
            public const string BaselineNum = "BaselineNum";
            public const string Baseline = "Baseline";
            public const string BaselineId = "BaselineId";
            public const string BaselineDate = "BaselineDate";
            public const string TicketNoOfConsultantsNotes = "NoOfConsultantsNotes";
            public const string ProjectCostNote = "ProjectCostNote";
            public const string ProjectCost = "ProjectCost";
            public const string ProjectScheduleNote = "ProjectScheduleNote";
            public const string BaselineComment = "BaselineComment";
            public const string NavigationDescription = "NavigationDescription";
            public const string LeftNavigationLookup = "LeftNavigationLookup";
            public const string TabSequence = "TabSequence";

            public const string ManagerOnly = "ManagerOnly";
            public const string ITOnly = "ITOnly";
            public const string Groups = "GroupsUser";
            public const string UPriority = "uPriority";
            public const string UGITImageUrl = "ImageUrl";
            public const string WithoutPanel = "WithoutPanel";
            public const string MenuHeight = "MenuHeight";
            public const string MenuWidth = "MenuWidth";
            public const string LoadDefaultValue = "LoadDefaultValue";

            public const string SectionName = "SectionName";
            public const string NotificationText = "NotificationText";
            public const string TicketToBeSentByDate = "ToBeSentByDate";
            public const string EmailOnDate = "EmailOnDate";
            public const string MailTo = "MailTo";
            public const string MailSubject = "MailSubject";
            //added by shiv for escalation functionality

            public const string EscalationRuleIDLookup = "EscalationRuleIDLookup";
            public const string SlaRuleIdLookup = "SLARuleIdLookup";
            public const string NextEscalationTime = "NextEscalationTime";
            public const string EscalationToEmails = "EscalationToEmails";
            public const string EscalationToRoles = "EscalationToRoles";
            public const string EscalationMinutes = "EscalationMinutes";
            public const string EscalationFrequency = "EscalationFrequency";
            public const string EscalationEmailBody = "EscalationEmailBody";
            public const string EscalationSentTime = "EscalationSentTime";
            public const string EscalationSentTo = "EscalationSentTo";
            public const string EscalationMailSubject = "EscalationMailSubject";
            public const string SLAType = "SLAType";
            public const string TicketRequestTypeMulLookup = "RequestTypeMulLookup";
            //SPDelta 40
            public static string SelectedTabs = "SelectedTabs";
            //
            //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
            public static string TicketEventTime = "EventTime";
            public static string TicketEventBy = "TicketEventBy";
            public static string PlannedEndDate = "PlannedEndDate";
            public static string TicketEventType = "TicketEventType";
            public static string EventReason = "EventReason";
            public static string Automatic = "Automatic";
            public static string SubTaskTitle = "SubTaskTitle";
            public static string SubTaskId = "SubTaskId";
            //
            // HolidayAndWorking hour List coloumn

            public const string WorkingHourFrom = "WorkdayStartTime";
            public const string WorkingHourTo = "WorkdayEndTime";
            public const string HolidayCategory = "Category";
            public const string EventDate = "EventDate";
            public const string EndDate = "EndDate";

            // ConfigTable Column

            public const string WorkingDays = "WorkingDays";
            public const string UseCalendar = "UseCalendar";
            public const string Holidays = "WeekendDays";
            public const string FieldName = "FieldName";


            //NPR Tasks
            public const string StartDate = "StartDate";
            public const string DueDate = "DueDate";
            public const string AssignedTo = "AssignedToUser";
            public const string AssignedToID = "AssignedToID";
            public const string PercentComplete = "PercentComplete";
            public const string ParentTask = "ParentTask";
            public const string ParentTaskID = "ParentTaskID";
            public const string Predecessors = "Predecessors";
            public const string PredecessorsID = "PredecessorsID";
            
            public const string TaskGroup = "Group";
            
            public const string IsPrivate = "IsPrivate";
            public const string ProjectRank = "ProjectRank";
            public const string ProjectRank2 = "ProjectRank2";
            public const string ProjectRank3 = "ProjectRank3";
            public const string PredecessorsByOrder = "PredecessorsByOrder";
            public const string IsCritical = "IsCritical";
            public const string EstimatedHours = "EstimatedHours";
            public const string ProjectTitle = "ProjectTitle";
            public const string RequestedResources = "RequestedResourcesUser";

            //Budget
            public const string BudgetCategory = "BudgetCategory";
            public const string BudgetCategoryName = "BudgetCategoryName";
            public const string BudgetType = "BudgetType";
            public const string BudgetTypeChoice = "BudgetTypeChoice";
            public const string BudgetCategoryLookup = "BudgetCategoryLookup";
            public const string BudgetSubCategory = "BudgetSubCategory";
            public const string BudgetItem = "BudgetItem";
            public const string BudgetDescription = "BudgetDescription";
            public const string BudgetAmount = "BudgetAmount";
            public const string BudgetAmountWithLink = "BudgetAmountWithLink";
            public const string BudgetCOA = "BudgetCOA";
            public const string BudgetIdLookup = "BudgetIdLookup";
            public const string ResourceCost = "ResourceCost";
            public const string EstimatedCost = "EstimatedCost";
            public const string Variance = "Variance";
            public const string IsAutoCalculated = "IsAutoCalculated";
            public const string AllocationAmount = "AllocationAmount";
            public const string PMMBudgetLookup = "PMMBudgetLookup";
            public const string ModuleBudgetLookup = "ModuleBudgetLookup";
            public const string BudgetTypeCOA = "BudgetTypeCOA";
            public const string BudgetStatus = "BudgetStatus";
            public const string CapitalExpenditure = "CapitalExpenditure";

            public const string UserSkill = "UserSkill";
            public const string DateExpression = "DateExpression";
            //ITG
            public const string CostToCompletion = "CostToCompletion";

            public const string BudgetStartDate = "BudgetStartDate";
            public const string BudgetEndDate = "BudgetEndDate";
            public const string ITGBudgetLookup = "ITGBudgetLookup";
            public const string ActualCost = "ActualCost";
            public const string CompanyTitleLookup = "CompanyTitleLookup";
            public const string InvoiceNumber = "InvoiceNumber";
            public const string PONumber = "PONumber";
            public const string GLCode = "GLCode";
            public const string DepartmentDescription = "DepartmentDescription";
            public const string FunctionalAreaDescription = "FunctionalAreaDescription";
            public const string UGITOwner = "UGITOwner";
            public const string NonProjectPlannedTotal = "NonProjectPlanedTotal";
            public const string NonProjectActualTotal = "NonProjectActualTotal";
            public const string ProjectPlannedTotal = "ProjectPlanedTotal";
            public const string ProjectActualTotal = "ProjectActualTotal";
            public const string DivisionLookup = "DivisionLookup";
            public const string DivisionIdLookup = "DivisionIdLookup";
            #region AssetVendor
            //AssetVendors
            public const string VendorName = "VendorName";
            public const string Location = "Location";
            public const string VendorLocation = "VendorLocation";
            public const string VendorPhone = "VendorPhone";
            public const string VendorEmail = "VendorEmail";
            public const string VendorAddress = "VendorAddress";
            public const string ContactName = "ContactName";
            public const string VendorWebsiteURL = "WebsiteURL";
            //added by Munna On 06-02-2017
            public const string AccountRepEmail = "VendorAccountRepEmail";
            public const string AccountRepName = "ContactName";
            public const string AccountRepPhone = "VendorAccountRepPhone";
            public const string AccountRepMobile = "VendorAccountRepMobile";
            public const string SupportCredentials = "VendorSupportCredentials";
            public const string VendorTypeLookup = "VendorType";
            public const string ProductServiceDescription = "ProductServiceDesc";
            public const string SupportHours = "VendorSupportHours";
            public const string VendorTimeZone = "VendorTimeZoneChoice";
            #endregion

            #region Vendor Type

            public const string VendorTypeTitle = "Title";
            public const string VendorTypeDesc = "VTDescription";
            #endregion
            //AssetModels
            public const string ModelName = "ModelName";
            public const string VendorLookup = "VendorLookup";


            //Assets
            public const string AssetName = "AssetName";
            public const string AssetDescription = "AssetDescription";
            public const string AssetOwner = "OwnerUser";
            public const string AssetTagNum = "AssetTagNum";
            public const string DepartmentLookup = "DepartmentLookup";
            public const string LocationLookup = "LocationLookup";
            public const string AssetModelLookup = "AssetModelLookup";
            public const string DeletionDate = "DeletionDate";
            public const string DeletedBy = "DeletedBy";
            public const string TSRIdLookup = "TSRIdLookup";
            public const string TSKIDLookup = "TSKIDLookup";
            public const string SoftwareTitle = "SoftwareTitle";
            public const string SoftwareVersion = "SoftwareVersion";
            public const string SoftwareKey = "SoftwareKey";


            //AssetReference
            public const string AssetReferenceType = "AssetReferenceType";
            public const string AssetReferenceNum = "AssetReferenceNum";
            public const string AssetLookup = "AssetLookup";
            public const string AssetMultiLookup = "AssetMultiLookup";
            public const string AssetDispositionChoice = "AssetDispositionChoice";

            //AssetRelations
            public const string Asset1 = "Asset1";
            public const string Asset2 = "Asset2";
            public const string AssetTagNumLookup = "AssetTagNumLookup";


            public const string DashboardMultiLookup = "DashboardMultiLookup";
            public const string UGITDescription = "Description";   //UGITDescription changed by mayank//
            public const string ManagerLookup = "ManagerLookup";
            public const string ResourceHourlyRate = "HourlyRate";
            public const string MobilePhone = "MobilePhone";
            public const string JobTitle = "JobTitle";
            public const string IT = "IT";
            public const string IsConsultant = "IsConsultant";
            public const string IsManager = "IsManager";
            public const string IsIT = "IsIT";
            public const string IsServiceAccount = "IsServiceAccount";
            public const string UserRoleIdLookup = "UserRoleIdLookup";

            //new entries for "out of office calender" start
            public const string EnableOutofOffice = "EnableOutofOffice";
            public const string LeaveToDate = "LeaveToDate";
            public const string LeaveFromDate = "LeaveFromDate";
            public const string DelegateUserOnLeave = "DelegateUserOnLeave";
            public const string DelegateUserFor = "DelegateUserFor";
            //new entries for "out of office calender" end

            public const string UserRole = "UserRole";
            public const string TicketUser = "UserName";  // changed from ticketuser to username bcz cannot create colun in db with ticketuser and user
            public const string IsActionUser = "IsActionUser";
            public const string PRPGroup = "PRPGroupUser";
            //public const Guid ProjectCost;

            public const string IsDisplay = "IsDisplay";
            public const string IsUseInWildCard = "IsUseInWildCard";
            public const string FieldSequence = "FieldSequence";
            public const string ShowInCardView = "ShowInCardView";

            // IncidentsTracking
            public const string OccurrenceDate = "OccurrenceDate";
            public const string DetectionDate = "DetectionDate";
            public const string AffectedUsers = "AffectedUsersUser";
            public const string ImpactsOrganization = "ImpactsOrganization";

            //
            public const string HideSummary = "HideSummary";
            public const string HideThankYouScreen = "HideThankYouScreen";
            public const string TicketBAAnalysisHours = "BAAnalysisHours";
            public const string TicketDeveloperCodingHours = "TicketDeveloperCodingHours";
            public const string TicketBATotalHours = "BATotalHours";
            public const string TicketBATestingHours = "BATestingHours";
            public const string TicketDeveloperSupportHours = "DeveloperSupportHours";
            public const string TicketTotalHours = "TotalHours";

            public const string CategoryName = "CategoryName";
            public const string CategoryNameChoice = "CategoryNameChoice";
            public const string ServiceDescription = "ServiceDescription";
            public const string IsActivated = "IsActivated";

            public const string ProjectPhasePctComplete = "ProjectPhasePctComplete";

            public const string RelatedRequestID = "RelatedRequestID";
            public const string PRSLookup = "PRSLookup";

            public const string MSProjectImportExportEnabled = "MSProjectImportExportEnabled";
            public const string AllocationID = "AllocationID";
            public const string TimeSheetID = "TimeSheetID";
            public const string CacheTable = "CacheTable";
            public const string CacheAfter = "CacheAfter";
            public const string CacheThreshold = "CacheThreshold";
            public const string ExpiryDate = "ExpiryDate";
            public const string CacheMode = "CacheMode";
            public const string RefreshMode = "RefreshMode";

            public const string TicketTargetStartDate = "TargetStartDate";

            public const string ScheduledStartDateTime = "ScheduledStartDateTime";
            public const string ScheduledEndDateTime = "ScheduledEndDateTime";

            public const string EnableModule = "EnableModule";
            public const string ResourceName = "ResourceNameUser";
            public const string ManagerName = "ManagerName";
            public const string TaskEstimatedHours = "EstimatedHours";
            public const string TaskEstimatedHours1 = "TaskEstimatedHours";
            public const string TaskActualHours = "ActualHours";
            public const string ActualHours = "TaskActualHours";
            public const string ShowComment = "ShowComment";
            public const string HourlyRate = "HourlyRate";
            public const string LabourCharges = "LabourCharges";

            //Dasgboard Query

            public const string DashboardModuleMultiLookup = "DashboardModuleMultiLookup";

            public const string ServiceCategoryNameLookup = "CategoryId";
            public const string TokenName = "TokenName";
            public const string ConditionalLogic = "ConditionalLogic";
            public const string QuestionTypeProperties = "QuestionTypeProperties";
            public const string UGITTaskType = "TaskType";
            public const string UGITTaskStatus = "TaskStatus";
            public const string UGITPredecessors = "Predecessors";
            public const string UGITStartDate = "StartDate";
            public const string UGITEndDate = "EndDate";
            public const string UGITCost = "Cost";
            public const string UGITAssignedTo = "AssignedToUser";
            public const string TicketCloseDate = "CloseDate";
            
            public const string ClosedDateOnly = "ClosedDateOnly";
            public const string UGITSourceID = "UGITSourceID";
            public const string UGITViewType = "ViewType";
            public const string UGITShortName = "ShortName";
            public const string UGITDaysToComplete = "DaysToComplete";
            public const string UGITLevel = "Level";
            public const string UGITContribution = "Contribution";
            public const string UGITChildCount = "ChildCount";
            public const string UGITDuration = "Duration";
            public const string UGITEstimatedHours = "EstimatedHours";
            public const string TaskBehaviour = "Behaviour";
            public const string BehaviourIcon = "BehaviourIcon";
            public const string TicketClosed = "Closed";
            public const string Closed = "Closed";
            public const string CloseOutDate = "CloseOutDate";

            public const string UGITNavigationType = "NavigationType";
            public const string UGITProposedDate = "ProposedDate";
            public const string UGITProposedStatus = "ProposedStatus";

            public const string NextActivity = "NextActivity";
            public const string NextMilestone = "NextMilestone";
            public const string ServiceCategoryName = "ServiceCategoryName";
            public const string ServiceName = "ServiceName";

            //Report Configurator.
            public const string ReportDefinitionLookup = "ReportDefinitionLookup";
            public const string DashboardPanelId = "DashboardPanelId";
            public const string CreateParentServiceRequest = "CreateParentServiceRequest";
            public const string SectionConditionalLogic = "SectionConditionalLogic";
            public const string OwnerApprovalRequired = "OwnerApprovalRequired";
            public const string AllowServiceTasksInBackground = "AllowServiceTasksInBackground";

            public const string IsAllTaskComplete = "IsAllTaskComplete";
            public const string QuestionMapVariables = "QuestionMapVariables";

            //Category Configurator
            public const string ClientAdminCategoryLookup = "ClientAdminCategoryLookup";
            public const string UGITComment = "UGITComment";
            public const string UGITResolutionDate = "UGITResolutionDate";
            public const string UGITResolution = "UGITResolution";
            public const string VNDActionType = "VNDActionType";

            public const string AutoSend = "AutoSend";
            public const string IsUserNotificationRequired = "IsUserNotificationRequired";
            public const string UserRoleLookup = "UserRoleLookup";
            public const string LandingPage = "LandingPage";
            public const string Enabled = "Enabled";
            public const string TicketDuration = "Duration";
            public const string SLADaysRoundUpDown = "SLADaysRoundUpDownChoice";
            public const string EscalationDescription = "EscalationDescription";

            public const string MenuDisplayType = "MenuDisplayType";
            public const string NavigationUrl = "NavigationUrl";
            public const string MenuParentLookup = "MenuParentLookup";

            ///Agent Job Scheduler Columns added by Maruf.     
            public const string StartTime = "StartTime";
            public const string ActionType = "ActionType";
            public const string EmailIDTo = "EmailIDTo";
            public const string EmailIDCC = "EmailIDCC";
            public const string Recurring = "Recurring";
            public const string RecurringInterval = "RecurringInterval";
            public const string RecurringEndDate = "RecurringEndDate";
            public const string Log = "Log";
            public const string AgentJobStatus = "AgentJobStatus";

            ///Contract management Module related columns Enter by Maruf
            public const string InitialCost = "InitialCost";
            public const string AnnualMaintenanceCost = "AnnualMaintenanceCost";
            public const string ContractStartDate = "ContractStartDate";
            public const string ContractExpirationDate = "ContractExpirationDate";
            public const string ReminderDate = "ReminderDate";
            public const string ReminderTo = "ReminderToUser";
            public const string ReminderBody = "ReminderBody";
            public const string NeedReview = "NeedReview";
            public const string TicketFinanceManager = "FinanceManagerUser";
            public const string TicketLegal = "LegalUser";
            public const string TicketPurchasing = "PurchasingUser";
            public const string TermType = "TermType";
            public const string RepeatInterval = "RepeatInterval";
            public const string UserLocation = "UserLocation";
            public const string LocationName = "Location";
            public const string UserDepartment = "UserDepartment";
            public const string Rating1 = "Rating1";
            public const string Rating2 = "Rating2";
            public const string Rating3 = "Rating3";
            public const string Rating4 = "Rating4";
            public const string Rating5 = "Rating5";
            public const string Rating6 = "Rating6";
            public const string Rating7 = "Rating7";
            public const string Rating8 = "Rating8";
            public const string TotalRating = "TotalRating";
            public const string DashboardID = "DashboardID";
            public const string AnalyticID = "AnalyticID";
            public const string AnalyticName = "AnalyticName";
            public const string AnalyticVID = "AnalyticVID";

            public const string Author = "Author";
            public const string HideInServiceMapping = "HideInServiceMapping";

            //Wiki Columns
            public const string WikiDescription = "WikiDescription";
            public const string WikiScore = "WikiScore";
            public const string WikiFavorites = "WikiFavorites";
            public const string WikiLikedBy = "WikiLikedBy";
            public const string WikiDislikedBy = "WikiDislikedBy";
            public const string WikiLikesCount = "WikiLikesCount";
            public const string WikiDislikesCount = "WikiDislikesCount";
            public const string WikiHistory = "WikiHistory";
            public const string WikiViewsCount = "WikiViews";
            public const string IsLiked = "IsLiked";
            public const string IsDisLiked = "IsDisLiked";
            public const string WikiRequestType = "RequestTypeLookUp";
            public const string WikiContentID = "WikiContentID";

            ///Application Columns
            public const string ApplicationModulesLookup = "ApplicationModulesLookup";
            public const string ApplicationRoleModuleLookup = "ApplicationRoleModuleLookup";

            public const string EnvironmentLookup = "EnvironmentLookup";
            public const string APPTitleLookup = "APPTitleLookup";
            public const string ApplicationRoleAssign = "ApplicationRoleAssignUser";
            public const string ApplicationRoleLookup = "ApplicationRoleLookup";
            public const string AccessAdmin = "AccessAdminUser";
            public const string SupportedBy = "SupportedByUser";
            public const string AssetsTitleLookup = "AssetsTitleLookup";
            public const string APPUserName = "APPUserName";
            public const string Password = "Password";
            public const string EncryptedPassword = "EncryptedPassword";
            public const string APPPasswordTitle = "APPPasswordTitle";
            public const string AccessManageLevel = "AccessManageLevel";
            public const string Owner = "OwnerUser";
            public const string BusinessManager = "BusinessManager";
            public const string UserName = "UserName";

            public const string LinkTitle = "LinkTitle";
            public const string URL = "URL";
            public const string Comment = "Comment";
            public const string Comments = "Comments";
            public const string LinkDescription = "Description";
            public const string WikiUserType = "WikiUserType";
            public const string WikiAverageScore = "WikiAverageScore";
            public const string WikiRatingDetails = "WikiRatingDetails";
            public const string WikiDiscussionCount = "WikiDiscussionCount";
            public const string WikiLinksCount = "WikiLinksCount";
            public const string WikiServiceRequestCount = "WikiServiceRequestCount";

            //ModuleConstraints
            public const string TaskDueDate = "TaskDueDate";
            public const string ContentTypeId = "ContentTypeId";

            /// Message Board            
            public const string MessageType = "MessageType";
            public const string Expires = "Expires";
            public const string ProjectLifeCycleLookup = "ProjectLifeCycleLookup";
            public const string LifeCycle = "LifeCycle";
            public const string LifeCycleName = "LifeCycleName";
            public const string TaskTemplateLookup = "TaskTemplateLookup";
            public const string IsMilestone = "IsMilestone";
            public const string StageStep = "StageStep";
            public const string ShowInMobile = "ShowInMobile";

            // New Location and Functional Area columns
            public const string FunctionalAreaTitle = "FunctionalAreaTitle";
            public const string FunctionalAreaTitleLookup = "FunctionalAreaTitleLookup";
            public const string UGITState = "State";
            public const string UGITCountry = "Country";
            public const string UGITRegion = "Region";
            public const string UserSkillMultiLookup = "UserSkillMultiLookup";

            //Document Management Columns
            public const string PortalName = "PortalName";
            public const string PortalOwner = "PortalOwner";
            public const string PortalDescription = "PortalDescription";
            public const string AlternateOwner = "AlternateOwner";
            public const string NumVersions = "NumVersions";
            public const string KeepDocsAlive = "KeepDocsAlive";
            public const string NumFiles = "NumFiles";
            public const string FolderName = "FolderName";
            public const string SizeOfFolder = "SizeOfFolder";
            public const string ReviewRequired = "ReviewRequired";
            public const string ModifiedByUser = "ModifiedByUser";
            public const string FileNavigationUrl = "FileNavigationUrl";
            public const string ImageUrl = "ImageUrl";
            public const string ToolTip = "ToolTip";
            public const string FolderOwner = "FolderOwner";
            public const string SizeOfFile = "SizeOfFile";
            public const string IsSizeUnlimited = "IsSizeUnlimited";
            public const string RestrictConfigureTypeOnly = "AllowAllTypes";
            public const string IsAllOfficeFormats = "IsAllOfficeFormats";
            public const string IsImage = "IsImage";
            public const string IsMultimedia = "IsMultimedia";
            public const string IsPdf = "IsPdf";
            public const string IsFolderProperties = "IsFolderProperties";
            public const string IsPortalProjectType = "IsPortalProjectType";
            public const string TypeOfFile = "TypeOfFile";
            public const string CheckInImage = "CheckInImage";
            public const string CheckInComment = "CheckInComment";
            
            public const string Type = "Type";
            public const string Red = "Red";
            public const string Yellow = "Yellow";
            public const string Green = "Green";
            public const string NumPings = "NumPings";
            public const string PingUser = "PingUser";
            public const string TaskID = "TaskID";
            public const string DMLocation = "Location";
            public const string CheckInAuthority = "CheckInAuthority";

            //History List Column
            //public const string DocumentID = "DocumentID";
            public const string DocName = "DocName";
            public const string DocLocation = "DocLocation";
            public const string EventData = "EventData";
            public const string EventType = "EventType";
            public const string EventTriggeredByUser = "EventTriggeredByUser";
            public const string EventOccuredOnTime = "EventOccuredOnTime";
            public const string DocumentStatus = "DMDocumentStatus";

            //DocumentWorkflow History table
            public const string DocWorkflowHistory = "DocWorkflowHistory";
            public const string DocumentUrl = "DocumentUrl";
            public const string CreatedByUser = "CreatedByUser";

            //Mislenious Column

            public const string CheckOutBy = "CheckOutBy";
            public const string Version = "Version";
            public const string VersionNo = "_UIVersionString";
            public const string FileSizeDisplay = "FileSizeDisplay";
            public const string DocIcon = "DocIcon";
            public const string ClassApplied = "ClassApplied";

            //public const string WorkflowLink = "WorkflowLink";
            // public const string WorkflowTaskStatus = "WorkflowTaskStatus";
            //public const string WorkflowStatus = "WorkflowStatus";
            public const string DocID = "DocID";

           
            public const string FileLeafRef = "FileLeafRef";
            //public const string Workflow = "Workflow";
            public const string PingComments = "PingComments";
            public const string DocumentID = "DocumentID";
            public const string IsDocumentID = "IsDocumentID";
            public const string NotifyOwnerBeforeDeletion = "NotifyOwnerBeforeDeletion";
            public const string NotifyOwnerOnDocumentUpload = "NotifyOwnerOnDocUpload";
            //public const string WorkflowName = "WorkflowName";

            public const string GroupName = "GroupName";
            public const string GroupOwner = "GroupOwner";
            public const string PermissionLevels = "PermissionLevels";
            public const string Url = "Url";
            public const string Readers = "Readers";
            public const string Authors = "Authors";
            public const string LinkFileName = "LinkFileName";
            public const string CCUser = "CCUser";
            public const string CopyTo = "CopyTo";

            public const string DMKeyValue = "DMKeyValue";

            public const string DMDepartment = "DocumentDepartment";
            public const string Vendor = "VendorName";
            public const string Customer = "CustomerName";
            public const string DocumentType = "DocumentType";

            public const string DMVendorLookup = "DMVendorLookup";
            // public const string CustomerLookup = "CustomerLookup";
            public const string DocumentTypeLookup = "DocumentTypeLookup";
            public const string DepartmentNameLookup = "DepartmentNameLookup";
            public const string LocationNameLookup = "LocationNameLookup";
            public const string FunctionareaNameLookup = "FunctionareaNameLookup";
            public const string FileName = "FileName";
            public const string Tags = "Tags";
            public const string DocumentComments = "DocumentComments";
            public const string UniqueId = "UniqueId";
            public const string DMSDescription = "DMSDescription";
            public const string ReviewStep = "ReviewStep";
            public const string CurrentApprover = "CurrentApprover";
            public const string DocumentVersion = "DocVersion";
            public const string ApprovedVersion = "ApprovedVersion";
            public const string NotifyAuthor = "NotifyAuthor";
            public const string NotifyReader = "NotifyReader";
            public const string KeepNYear = "KeepNYear";
            public const string Project = "Project";
            public const string ProjectLookup = "ProjectLookup";

            public const string NotifyOnReviewStart = "NotifyUserOnReviewStart";
            public const string NotifyOnReviewComplete = "NotifyOnReviewComplete";

            public const string Approver = "ApproverUser";

            //It is fro file metadata field
            public const string DocumentControlID = "DocumentControlID";

            public const string PortalId = "PortalId";

            public const string Acronym = "Acronym";
            public const string PrefixAcronym = "PrefixAcronym";
            public const string OverrideReaders = "OverrideReaders";
            public const string TagName = "TagName";

            public const string HistoryDate = "HistoryDate";
            public const string Action = "Action";
            public const string SequenceNo = "SequenceNo";
            public const string ExpirationDate = "ExpirationDate";
            public const string ReviewCycle = "ReviewCycle";
            public const string NumOfReviewCycle = "NumOfReviewCycle";
            public const string TargetPortalID = "TargetPortalID";
            public const string LinkText = "LinkText";
            public const string FolderUrl = "FolderUrl";
            public const string FolderID = "FolderID";
            public const string IsShortcutLink = "IsShortcutLink";
            public const string DocRevision = "UVersion";
            public const string SourcePortalID = "SourcePortalID";
            public const string SourceFolderID = "SourceFolderID";
            public const string AlwaysReviewRequired = "AlwaysReviewRequired";
            public const string AlwaysNotifyOwnerOnReviewComplete = "AlwaysNotifyOwnerOnReviewComplete";
            public const string AlwaysNotifyOwnerOnReviewStart = "AlwaysNotifyOwnerOnReviewStart";

            public const string ReviewCycle1 = "ReviewCycle1";
            public const string ReviewCycle2 = "ReviewCycle2";
            public const string ReviewCycle3 = "ReviewCycle3";
            public const string ReviewCycle4 = "ReviewCycle4";
            public const string ReviewCycle5 = "ReviewCycle5";
            public const string ReviewCycle6 = "ReviewCycle6";
            public const string ReviewCycle7 = "ReviewCycle7";
            public const string ReviewCycle8 = "ReviewCycle8";
            public const string ReviewCycle9 = "ReviewCycle9";
            public const string ReviewCycle10 = "ReviewCycle10";

            public const string UseDefaultCategory = "UseDefaultCategory";
            public const string DMDocumentStatus = "DMDocumentStatus";

            public const string DMDocType1 = "DMDocType1";
            public const string DMDocType2 = "DMDocType2";
            public const string DMDocType3 = "DMDocType3";
            public const string DMDocType4 = "DMDocType4";
            public const string DMDocType5 = "DMDocType5";

            public const string DepartmentID = "DepartmentID";
            public const string VendorID = "VendorID";
            public const string DocumentTypeID = "DocumentTypeID";
            public const string Owners = "Owners";
            public const string IncludeActionUsers = "IncludeActionUsers";
            public const string CompanyMultiLookup = "CompanyMultiLookup";
            public const string CompanyIdLookup = "CompanyIdLookup";
            public const string DivisionMultiLookup = "DivisionMultiLookup";
            public const string AppReferenceInfo = "AppReferenceInfo";
            public const string ProjectSummaryNote = "ProjectSummaryNote";
            public const string EnableCache = "EnableCache";
            public const string SyncAppsToRequestType = "SyncAppsToRequestType";
            public const string ServiceApplicationAccessXml = "ServiceApplicationAccessXml";
            public const string TicketApprovedRFE = "ApprovedRFE";
            public const string TicketApprovedRFEAmount = "ApprovedRFEAmount";
            public const string TicketApprovedRFEType = "ApprovedRFEType";
            public const string IsLatestVersion = "IsLatestVersion";
            public const string ProjectHealth = "ProjectHealth";
            public const string SyncToRequestType = "SyncToRequestType";
            public const string SyncAtModuleLevel = "SyncAtModuleLevel";
            public const string PctPlannedAllocation = "PctPlannedAllocation";
            public const string PlannedAllocationHour = "PlannedAllocationHour";
            //public const string PlannedEndDate = "PlannedEndDate";
            public const string PlannedStartDate = "PlannedStartDate";
            public const string PctEstimatedAllocation = "PctEstimatedAllocation";
            public const string EstimatedEndDate = "EstimatedEndDate";
            public const string EstimatedStartDate = "EstimatedStartDate";
            public const string EstStartDate = "EstStartDate";
            public const string EstEndDate = "EstEndDate";
            public const string ServiceWizardOnly = "ServiceWizardOnly";
            public const string RequestorDepartment = "RequestorDepartment";
            public const string RequestorCompany = "RequestorCompany";
            public const string RequestorDivision = "RequestorDivision";
            public const string TicketOnHoldTillDate = "OnHoldTillDate";
            
            public const string UnapprovedAmount = "UnapprovedAmount";
            public const string IssueImpact = "IssueImpact";
            public const string AccomplishmentDate = "AccomplishmentDate";
            public const string UseDesiredCompletionDate = "UseDesiredCompletionDate";

            // Project Calendar
            public const string UGITStatus = "UGITStatus";
            public const string UGITEventType = "UGITEventType";
            public const string UGITItemColor = "ItemColor";
            public const string RecurrenceInfo = "RecurrenceInfo";
            public const string EventLocation = "Location";
            public const string fAllDayEvent = "fAllDayEvent";
            public const string OutOfOffice = "OutOfOffice";
            public const string ShowOnProjectCalendar = "ShowOnProjectCalendar";

            public const string AttachmentRequired = "AttachmentRequired";
            public const string ADProperty = "ADProperty";
            public const string UserProperty = "UserProperty";
            public const string EmailReplyTo = "EmailReplyTo";
            public const string EmailIDFrom = "EmailIDFrom";
            public const string StoreTicketEmails = "StoreEmails";
            public const string WorkflowSkipStages = "WorkflowSkipStages";
            public const string IsIncomingMail = "IsIncomingMail";
            public const string EmailStatus = "EmailStatus";
            public const string EmailError = "EmailError";
            public const string SortOrder = "SortOrder";
            public const string IsAscending = "IsAscending";

            public const string Picture = "Picture";
            public const string UseInGlobalSearch = "UseInGlobalSearch";
            public const string ShowMyDeptTickets = "ShowMyDeptTickets";
            public const string LinkedDocuments = "LinkedDocuments";
            public const string TaskRepeatInterval = "RepeatInterval";
            //Scrum
            public const string SprintLookup = "SprintLookup";
            public const string SprintOrder = "SprintOrder";
            public const string TaskStatus = "TaskStatus";
            public const string SprintDuration = "SprintDuration";
            public const string RemainingHours = "RemainingHours";
            
            public const string ReleaseLookup = "ReleaseLookup";

            public const string UGITAssignToPct = "AssignToPct";
            public const string UGITWebsiteUrl = "WebsiteUrl";

            //Skill Resource 
            public const string UserSkillLookup = "UserSkillLookup";
            public const string UserCertificateLookup = "UserCertificateLookup";
            public const string _ResourceType = "_ResourceType";

            //Group
            public const string UserGroup = "UserGroup";
            //other 

            public const string Percent_Complete = "% Complete";
            public const string EstimatedRemainingHours = "EstimatedRemainingHours";
            public const string TaskReminderDays = "ReminderDays";
            public const string TaskReminderEnabled = "ReminderEnabled";

            public const string AutoAdjustAllocations = "AutoAdjustAllocations";
            public const string DocumentLibraryName = "DocumentLibraryName";
            public const string VendorMSALookup = "VendorMSALookup";
            public const string VendorSOWLookup = "VendorSOWLookup";
            public const string VendorSLALookup = "VendorSLALookup";
            public const string VendorResourceSubCategoryLookup = "VendorResourceSubCategoryLookup";
            public const string ResourceQuantity = "ResourceQuantity";
            public const string InvoiceItemAmount = "InvoiceItemAmount";
            public const string SOWInvoiceLookup = "SOWInvoiceLookup";
            public const string SOWInvoiceAmount = "SOWInvoiceAmount";
            public const string SOWInvoiceDate = "SOWInvoiceDate";
            public const string VendorSLAReportingDate = "VendorSLAReportingDate";
            public const string VendorSLAMet = "VendorSLAMet";
            public const string VendorSLAPerformanceNumber = "VendorSLAPerformanceNumber";
            public const string VendorVPMLookup = "VendorVPMLookup";
            public const string VendorSLANameLookup = "VendorSLANameLookup";
            public const string VendorSLALookupID = "VendorSLALookupID";
            public const string SLANumber = "SLANumber";
            public const string CRNumber = "CRNumber";

            public const string ContractValue = "ContractValue";
            public const string SOWInvoiceActualAmount = "SOWInvoiceActualAmount";

            //investment Management
            public const string InvestorID = "InvestorID";
            public const string InvestorShortName = "InvestorShortName";
            public const string InvestorName = "InvestorName";
            public const string StreetAddress = "StreetAddress";
            public const string WorkCity = "WorkCity";
            //public const string UGITState = "UGITState";
            public const string WorkZip = "WorkZip";
            public const string EmployerIdentificationNumber = "EmployerIdentificationNumber";
            public const string Responsible = "Responsible";
            public const string UpdateDate = "UpdateDate";
            public const string InvestorStatus = "InvestorStatus";
            public const string Custodian = "Custodian";
            public const string AddedDate = "AddedDate";
            public const string OtherAddress = "OtherAddress";
            public const string Contact = "Contact";
            public const string AccountType = "AccountType";
            public const string LastName = "LastName";
            public const string EmailAddress = "EmailAddress";

            public const string InvestorShortNameLookup = "InvestorShortNameLookup";
            public const string Investment = "Investment";
            public const string AcquireDate = "AcquireDate";
            public const string INVType = "INVType";
            public const string ExpectedExit = "ExpectedExit";
            public const string ReturnYield = "ReturnYield";
            public const string InvestmentManagers = "InvestmentManagers";

            public const string SLAsMissed = "SLAsMissed";
            public const string FixedFees = "FixedFees";
            public const string FeePct = "FeePct";
            public const string SOWNoOfUnit = "SOWNoOfUnit";
            public const string SOWUnitRate = "SOWUnitRate";
            public const string SOWFeeUnit = "SOWFeeUnit";
            public const string SOWFeeUnit2 = "SOWFeeUnit2";
            public const string SOWAdditionalUnitRate = "SOWAdditionalUnitRate";
            public const string SOWReducedUnitRate = "SOWReducedUnitRate";
            public const string SOWAnnualChangePct = "SOWAnnualChangePct";
            public const string SOWDeadBandPct = "SOWDeadBandPct";

            public const string VendorReportingType = "VendorReportingType";
            public const string ReportingFrequencyUnit = "ReportingFrequencyUnit";
            public const string ReportingFrequency = "ReportingFrequency";
            public const string ReportingSLA = "ReportingSLA";
            public const string SLAMissedPenalty = "SLAMissedPenalty";
            public const string ReportingRecepients = "ReportingRecepients";
            public const string ReportingStartDate = "ReportingStartDate";
            public const string ClientObligations = "ClientObligations";


            public const string VendorMeetingType = "VendorMeetingType";
            public const string VendorMeetingAgenda = "VendorMeetingAgenda";
            public const string MeetingFrequencyUnit = "MeetingFrequencyUnit";
            public const string MeetingFrequency = "MeetingFrequency";
            public const string MeetingParticipants = "MeetingParticipants";
            public const string MeetingMaterial = "MeetingMaterial";
            public const string ServiceStartDate = "ServiceStartDate";
            public const string ServiceEndDate = "ServiceEndDate";
            public const string InvestorIDLookup = "InvestorIDLookup";
            public const string InvestmentIDLookup = "InvestmentIDLookup";
            public const string DistributionType = "DistributionType";
            public const string DistributionDate = "DistributionDate";
            public const string DistributionQuarter = "DistributionQuarter";
            public const string DistributionAmount = "DistributionAmount";
            public const string VendorSOWFeeLookup = "VendorSOWFeeLookup";

            public const string ActionTypeData = "ActionTypeData";
            public const string Report = "Report";
            public const string AttachmentFormat = "AttachmentFormat";
            public const string AlertCondition = "AlertCondition";
            public const string VariableAmount = "VariableAmount";
            public const string GlobalFilterRequired = "GlobalFilterRequired";
            //TicketTemplate
            public const string FieldValues = "FieldValues";

            public const string selectedTabs = "selectedTabs";

            public const string HideInTicketTemplate = "HideInTemplate";

            //RequestType list extension
            public const string AssignmentSLA = "AssignmentSLA";
            public const string ResolutionSLA = "ResolutionSLA";
            public const string CloseSLA = "CloseSLA";
            public const string RequestorContactSLA = "RequestorContactSLA";
            public const string AutoCreateDocumentLibrary = "AutoCreateDocumentLibrary";
            public const string NextSLAType = "NextSLAType";
            public const string NextSLATime = "NextSLATime";
            public const string ShowNextSLA = "ShowNextSLA";

            public const string StartResolutionSLAFromAssigned = "StartResolutionSLAFromAssigned";
            public const string SLAConfiguration = "SLAConfiguration";

            public const string RequestorContacted = "RequestorContacted";
            public const string VendorRiskImpact = "VendorRiskImpact";
            public const string RiskProbability = "RiskProbability";
            public const string MitigationPlan = "MitigationPlan";
            public const string ContingencyPlan = "ContingencyPlan";

            public const string LinkCategoryLookup = "LinkCategoryLookup";
            public const string LinkViewLookup = "LinkViewLookup";
            public const string EnableCustomReturn = "EnableCustomReturn";

            public const string VendorIssueImpact = "VendorIssueImpact";
            public const string SubContractorService = "SubContractorService";
            public const string ApprovedSubContractorName = "ApprovedSubContractorName";

            public const string EnableQuickTicket = "EnableQuick";
            public const string ApproveIcon = "ApproveIcon";
            public const string ReturnIcon = "ReturnIcon";
            public const string RejectIcon = "RejectIcon";
            public const string VendorSOWNameLookup = "VendorSOWNameLookup";
            public const string VendorMSANameLookup = "VendorMSANameLookup";
            public const string SOWContinuousImprovementPct = "SOWContinuousImprovementPct";
            public const string SOWContinuousImprovementPeriod = "SOWContinuousImprovementPeriod";
            public const string SOWContinuousImprovementPeriodUnit = "SOWContinuousImprovementPeriodUnit";
            public const string DisableNewTicketConfirmation = "DisableNewTicketConfirmation";
            public const string TicketOwnerBinding = "OwnerBindingChoice";
            public const string AllowChangeTicketType = "AllowChangeType";
            public const string AllowBatchEditing = "AllowBatchEditing";
            public const string AllowDeleteFunctionality = "AllowDeleteFunctionality";
            public const string KeyRefUniqueID = "KeyRefUniqueID";
            public const string AgreementNumber = "AgreementNumber";
            public const string AllowBatchCreate = "AllowBatchCreate";
            public const string AllowEscalationFromList = "AllowEscalationFromList";
            public const string AllowReassignFromList = "AllowReassignFromList";
            public const string TaskSkill = "TaskSkill";
            public const string TaskSkillId = "TaskSkillId";
            public const string DeskLocation = "DeskLocation";
            public const string AuthorizedToCreate = "AuthorizedToCreate";
            public const string NotificationEmail = "NotificationEmail";
            public const string ApproveLevelAmount = "ApproveLevelAmount";

            ///Customer
            public const string FirstName = "FirstName";
            public const string Company = "Company";
            public const string WorkAddress = "WorkAddress";
            public const string IdentifiedBy = "IdentifiedBy";
            public const string AcquiredBy = "AcquiredBy";
            public const string AccountManager = "AccountManager";
            public const string CampaignInfo = "CampaignInfo";
            public const string CampaignStrategy = "CampaignStrategy";
            public const string CustomerSource = "CustomerSource";
            public const string SourceDetails = "SourceDetails";
            public const string LeadStatus = "LeadStatus";
            public const string ChanceOfSuccess = "ChanceOfSuccessChoice";
            public const string SuccessChance = "SuccessChance";
            public const string BCCISector = "SectorChoice";
            public const string Quality = "Quality";
            public const string CampaignUsed = "CampaignUsed";
            public const string AccountName = "AccountName";
            public const string AccountAddress = "AccountAddress";
            public const string CustomerAccountSize = "CustomerAccountSize";
            public const string CustomerAccountType = "CustomerAccountType";
            public const string CustomerAccountVertical = "CustomerAccountVertical";
            public const string Revenues = "Revenues";
            public const string NoOfEmployee = "NoOfEmployee";
            public const string SICCode = "SICCode";
            public const string LinkedInPage = "LinkedInPage";
            public const string FacebookPage = "FacebookPage";
            public const string TwitterPage = "TwitterPage";
            public const string CRMUrgency = "CRMUrgency";
            public const string StudioLookup = "StudioLookup";

            ///Proposal
            public const string ITStaffSize = "ITStaffSize";
            public const string ComponentsNeeded = "ComponentsNeeded";
            public const string ScopeOfServices = "ScopeOfServices";
            public const string Price = "Price";
            public const string BillingStartDate = "BillingStartDate";
            public const string FollowUp = "FollowUp";
            public const string PlanResources = "PlanResources";
            public const string CRMTicketLookup = "CRMTicketLookup";
            public const string NoOfLicenses = "NoOfLicenses";

            public const string ShowTicketSummary = "ShowSummary";

            public const string AcceptanceCriteria = "AcceptanceCriteria";
            public const string DeliverableAttributes = "DeliverableAttributes";
            public const string DeliverableMode = "DeliverableMode";
            public const string Editor = "Editor";

            public const string AutoCalculate = "AutoCalculate";
            public const string QuestionTitle = "QuestionTitle";

            public const string RequestorNotificationOnComment = "RequestorNotificationOnComment";
            public const string ActionUserNotificationOnComment = "ActionUserNotificationOnComment";
            public const string InitiatorNotificationOnComment = "InitiatorNotificationOnComment";
            public const string RequestorNotificationOnCancel = "RequestorNotificationOnCancel";
            public const string ActionUserNotificationOnCancel = "ActionUserNotificationOnCancel";
            public const string InitiatorNotificationOnCancel = "InitiatorNotificationOnCancel";

            public const string WaitingOnMeIncludesGroups = "WaitingOnMeIncludesGroups";
            public const string chkWaitingOnMeIncludeGroups = "chkWaitingOnMeIncludeGroups";
            public const string WaitingOnMeExcludesResolved = "WaitingOnMeExcludesResolved";
            public const string chkWaitingOnMeExcludeResolved = "chkWaitingOnMeExcludeResolved";
            public const string TargetURL = "TargetURL";
            //public const string ToolTip = "ToolTip";

            public const string SelectedTabNumber = "SelectedTabNumber";
            public const string ApproveButtonTooltip = "ApproveButtonTooltip";
            public const string RejectButtonTooltip = "RejectButtonTooltip";
            public const string ReturnButtonTooltip = "ReturnButtonTooltip";
            public const string TrimContentAfter = "TrimContentAfter";
            public const string TicketRequestTypeSubCategory = "RequestTypeSubCategory";

            //new columns name for resource Allocation Monthly.
            public const string ResourceWorkItemType = "ResourceWorkItemType";
            public const string ResourceWorkItem = "ResourceWorkItem";
            public const string ResourceSubWorkItem = "ResourceSubWorkItem";
            public const string EnablePasswordExpiration = "EnablePasswordExpiration";
            public const string PasswordExpiryDate = "PasswordExpiryDate";
            public const string MGSSubmittedDate = "MGSSubmittedDate";

            public const string CompletionDate = "CompletionDate";
            public const string ResolutionTypes = "ResolutionTypes";
            public const string SortToBottom = "SortToBottom";
            public const string StageCapacityNormal = "StageCapacityNormal";
            public const string StageCapacityMax = "StageCapacityMax";

            public const string ShowBottleNeckChart = "ShowBottleNeckChart";
            public const string OpenTicketChart = "OpenChart";
            public const string CloseTicketChart = "CloseChart";
            public const string VIPTicket = "VIPTicket";
            public const string CompletedBy = "CompletedBy";
            public const string RelationshipType = "RelationshipType";


            //OPM: Oppurtunity Management
            public const string LegalName = "LegalName";
            public const string StreetAddress1 = "StreetAddress1";
            public const string StreetAddress2 = "StreetAddress2";
            public const string City = "City";
            public const string Zip = "Zip";
            public const string Telephone = "Telephone";
            public const string Fax = "Fax";
            public const string OrganizationType = "OrganizationType";
            public const string BusinessType = "BusinessType";
            public const string FederalID = "FederalID";
            public const string ContractorLicense = "ContractorLicense";
            public const string Division = "Division";
            public const string MasterAgreement = "MasterAgreement";
            public const string WorkRegion = "WorkRegion";
            public const string WorkType = "WorkType";
            public const string CRMStatus = "CRMStatus";
            public const string Trade = "Trade";
            public const string Certifications = "Certifications";
            public const string CompanyName = "CompanyName";
            
            public static string AssistantProjectManager = "AssistantProjectManager";
            public static string Estimator = "EstimatorUser";
            public static string NameTitle = "NameTitle";
            public const string OrganizationNote = "OrganizationNote";
            public const string OrganizationStatus = "OrganizationStatus";
            public static string CRMCompanyTitleLookup = "CRMCompanyTitleLookup";
            public static string CRMCompanyTitle = "CRMCompanyTitle";
            public static string CRMCompanyLookup = "CRMCompanyLookup";
            public static string TicketLEMIdLookup = "LEMIdLookup";
            public static string LEMIdLookup = "LEMIdLookup";
            public static string TicketOPMIdLookup = "OPMIdLookup";
            public static string ProposalRecipient = "ProposalRecipientLookup"; //"ProposalRecipient";
            public static string AdditionalRecipients = "AdditionalRecipientsLookup"; //"AdditionalRecipients";
            public static string ClientName = "ClientName";
            //public static string CRMDuration = "CRMDuration";

            public const string UGITFirstName = "FirstName";
            public const string UGITMiddleName = "MiddleName";
            public const string AddressedAs = "AddressedAs";
            public const string SecondaryEmail = "SecondaryEmail";
            public const string Mobile = "Mobile";
            public const string ContactType = "ContactType";
            public const string OrganizationLookup = "OrganizationLookup";
            //public const string CRMCompanyTitleLookup = "CRMCompanyTitleLookup";
            public static string CRMOpportunityStatus = "CRMOpportunityStatusChoice";
            public static string BusinessUnitLookup = "BusinessUnitLookup";
            public static string LeadSourceCompanyLabel = "LeadSourceCompanyLabel";
            public static string LeadSource = "LeadSourceLookup";
            public static string Reason = "Reason";
            public static string AwardedLossDate = "AwardedorLossDate";

            //COM
            public static string RelationshipTypeLookup = "RelationshipTypeLookup";


            //Bid
            public const string BidSequence = "BidSequence";
            public const string BidArea = "BidArea";
            public const string ContactLookup = "ContactLookup";
            public const string BidAmount = "BidAmount";
            public const string TargetAmount = "TargetAmount";
            public const string BidDate = "BidDate";

            // CRMProject
            public static string EstimatedConstructionStart = "EstimatedConstructionStart";
            public static string ApproxContractValue = "ApproxContractValue";
            public static string ApproxContractValueRpt = "ApproxContractValueRpt";
            public static string EstimatedConstructionEnd = "EstimatedConstructionEnd";
            public static string LiquidatedDamages = "LiquidatedDamages";
            public static string Bonus = "Bonus";
            public static string Savings = "Savings";
            public static string GeneralConditionsDelay = "GeneralConditionsDelay";
            public static string Payments = "Payments";
            public static string WaiverDamages = "WaiverDamages";
            public static string LienWaiver = "LienWaiver";
            public static string Retainage = "Retainage";
            public const string RetainageChoice = "RetainageChoice";
            public static string ApprovedChangeOrders = "ApprovedChangeOrders";
            public static string ChangeOrders = "ChangeOrders";
            public static string SubContractorMarkUp = "SubContractorMarkUp";
            public static string BuilderRisk = "BuilderRisk";
            public static string WaiverSubrogation = "WaiverSubrogation";
            public static string DisputeResolution = "DisputeResolution";
            public static string DisputedWorkCap = "DisputedWorkCap";
            public static string SubcontractorDefaultInsurance = "SubcontractorDefaultInsurance";
            public static string PaymentAndPerformanceBonds = "PaymentAndPerformanceBonds";
            public static string DiverseCertificationChoice = "DiverseCertificationChoice";
            public static string CertifyingAgency = "CertifyingAgency";
            public static string ExecutiveOrderRequirements = "ExecutiveOrderRequirements";
            public static string SignedNDA = "SignedNDA";

            public static string Contingency = "Contingency";
            public static string SubstantialCompletion = "SubstantialCompletion";
            public static string Warranties = "Warranties";
            public static string SpecialProvisions = "SpecialProvisions";
            public static string ContractExecSumChoice = "ContractExecSumChoice";
            public static string GoAfterChoice = "GoAfterChoice";
            public static string ContractInERP = "ContractInERP";
            public static string SignatureTitle = "SignatureTitle";
            public static string ContractAssignedToUser = "ContractAssignedToUser";
            public static string UsableSqFtNum = "UsableSqFtNum";
            public static string CloseoutStartDate = "CloseoutStartDate";
            public static string PreconDuration = "PreconDuration";
            public static string EstimatedConstructionDuration = "EstimatedConstructionDuration";

            public static string NumTags = "NumTags";
            public static string NumUniqueRoles = "NumUniqueRoles";
            public static string NumAllocations = "NumAllocations";
            public static string NumContractAmountChanges = "NumContractAmountChanges";
            public static string PerContractAmountChange = "PerContractAmountChange";
            public static string NumResourceAllocationChanges = "NumResourceAllocationChanges";
            public static string NumScheduleChanges = "NumScheduleChanges";
            public static string PerScheduleChanges = "PerScheduleChanges";
            public static string PerTimeTakenToFillUnfilledRoles = "PerTimeTakenToFillUnfilledRoles";
            public static string GLInsurance = "GLInsurance";
            public static string SDI = "SDI";
            public static string Bond = "Bond";
            public static string AuditRights = "AuditRights";
            //Not in CRMProject
            public static string ReasonType = "ReasonType";

            //VND
            public const string ReportInstanceStatus = "ReportInstanceStatus";
            public const string NoDocumentNeeded = "NoDocumentNeeded";
            public const string ReportFrequencyType = "ReportFrequencyType";
            public const string ReportMonthFrequencyType = "ReportMonthFrequencyType";
            public const string ReminderDays = "ReminderDays";
            public const string VendorReportLookup = "VendorReportLookup";
            public const string UGITDueDate = "DueDate";
            public const string ReceivedOn = "ReceivedOn";
            public const string AcceptedOn = "AcceptedOn";
            public const string EffectiveStartDate = "EffectiveStartDate";
            public const string EffectiveEndDate = "EffectiveEndDate";
            public const string VendorPOLineItemLookup = "VendorPOLineItemLookup";
            public const string VendorPOLookup = "VendorPOLookup";
            public const string VendorPOLineItems = "VendorPOLineItems";
            public const string LineItemNumber = "LineItemNumber";
            public const string BilledAmount = "BilledAmount";
            public const string PrespentAmount = "PrespentAmount";

            public const string ImageOptionLookup = "ImageOptionLookup";
            public const string OS = "OS";
            public const string ReminderType = "ReminderType";
            public const string IsVIP = "IsVIP";

            //CRMActivities
            public static string ActivityStatus = "ActivityStatus";

            //Permit
            public static string TicketCPRIdLookup = "CPRIdLookup";


            //Menu Bar Navigational Url
            public const string LinkUrl = "LinkUrl";
            public const string ParentID = "ParentID";

            //Theme
            public const string MasterPageUrl = "MasterPageUrl";
            public const string ThemeUrl = "ThemeUrl";

            public const string FontSchemeUrl = "FontSchemeUrl";
            public const string DisableWorkflowNotifications = "DisableWorkflowNotifications";
            public const string SendEvenIfStageSkipped = "SendEvenIfStageSkipped";
             


            // ResourceTab
            public const string RoleName = "RoleName";
            public const string RoleNameChoice = "RoleNameChoice";
            public const string Role = "Role";
            public const string JobProfile = "JobProfile";
            public const string Job = "Job";
            public const string Consultant = "Consultant";
            public const string TitleLink = "TitleLink";
            public const string ManagerLink = "ManagerLink";
            public const string Skills = "Skills";
            public const string FunctionalArea = "FunctionalArea";
            public const string LoginName = "LoginName";
            public const string LocationId = "LocationId";
            public const string RoleId = "RoleId";
            public const string JobType = "JobType";
            public const string GlobalRoleID = "GlobalRoleId";

            //To show user service input
            public const string UserQuestionSummary = "UserQuestionSummary";

            public const string EnableApproval = "EnableApproval";
            public const string Approvalstatus = "ApprovalStatus";
            public const string TaskAssignee = "TaskAssignee";
            public const string ApproverID = "ApproverID";

            public const string ApprovalType = "ApprovalTypeChoice";
            public const string TaskActionUser = "TaskActionUser";
            public const string ApprovedBy = "ApprovedByUser";
            public const string TextAlignment = "TextAlignmentChoice";
            public const string PreloadAllModuleTabs = "PreloadAllModuleTabs";
            public const string Address = "Address";
            public const string AllowActualHoursByUser = "AllowActualHoursByUser";

            public const string EnableModuleAgent = "EnableModuleAgent";
            public const string AutoRefreshListFrequency = "AutoRefreshListFrequency";

            //Site Assets
            public const string UserProfile = "User Profile";
            public const string SiteAssets = "SiteAssets";
            //Enable import export project
            public const string EnableProjectExportImport = "EnableProjectExportImport";
            //For user profile Phone
            public const string Phone = "Phone";
            public const string PhoneNumber = "PhoneNumber";
            public const string HideTicketFooter = "HideTicketFooter";
            public const string HideFooter = "HideFooter";
            public const string AssignmentSLAMet = "AssignmentSLAMet";
            public const string RequestorContactSLAMet = "RequestorContactSLAMet";
            public const string ResolutionSLAMet = "ResolutionSLAMet";
            public const string CloseSLAMet = "CloseSLAMet";
            public const string OtherSLAMet = "OtherSLAMet";
            public const string ALLSLAsMet = "ALLSLAsMet";

            //Ticket workflow SLA Summary
            public const string RuleNameLookup = "RuleNameLookup";
            public const string TargetTime = "TargetTime";
            public const string ActualTime = "ActualTime";
            public const string SLARuleName = "SLARuleName";
            public const string OnHoldDuration = "OnHoldDuration";
            public const string StartStageStep = "StartStageStep";
            public const string EndStageStep = "EndStageStep";
            public const string StartStageName = "StartStageName";
            public const string EndStageName = "EndStageName";
            public const string StageStartDate = "StageStartDate";
            public const string StageEndDate = "StageEndDate";
            public const string Module = "Module";
            public const string MessageId = "MessageId";
            public const string ServiceCategoryType = "ServiceCategoryType";
            public static string CRMDuration = "CRMDuration"; //"Duration";
            public static string ExternalProjectId = "ExternalProjectId";
            public static string ProcoreColumnName = "ProcoreColumnName";
            public static string InternalColumnName = "InternalColumnName";
            public const string ServiceType = "ServiceType";
            public const string TicketTestingTotalHours = "TestingTotalHours";
            public const string IssueTypeOptions = "IssueTypeOptions";
            public const string UGITIssueType = "IssueType";
         

            public const string MenuBackground = "MenuBackground";
            public const string SubMenuStyle = "SubMenuStyle";
            public const string SubMenuItemPerRow = "SubMenuItemPerRow";
            public const string SubMenuItemAlignment = "SubMenuItemAlignment";
            public const string MenuName = "MenuName";
            public const string MenuFontColor = "MenuFontColor";
            public const string MenuItemSeparation = "MenuItemSeparation";
            public const string MenuTextAlignment = "TextMenuAlignment";


            public const string TicketEmailType = "TicketEmailType";

            public const string SPReported = "SPReported";
            public const string ITMSReported = "ITMSReported";
            public const string VendorPerformanceWaiver = "VendorPerformanceWaiver";
            //VPM
            public const string TotalSLAs = "TotalSLAs";
            public const string TotalSLAsMet = "TotalSLAsMet";
            public const string SLAsMetComment = "SLAsMetComment";
            public const string MissedSLAsComment = "MissedSLAsComment";
            public const string NotDueSLAs = "NotDueSLAs";
            public const string NotDueSLAsComment = "NotDueSLAsComment";
            public const string OtherSLAs = "OtherSLAs";
            public const string OtherSLAsComment = "OtherSLAsComment";
            public const string Timeliness = "Timeliness";
            public const string Completeness = "Completeness";
            public const string SLCreditsDue = "SLCreditsDue";
            public const string SLCreditsDueComment = "SLCreditsDueComment";
            public const string SLDefaults = "SLDefaults";
            public const string SLDefaultsComment = "SLDefaultsComment";

            public const string RootCauseAnalysis = "RootCauseAnalysis";
            public const string RootCauseAnalysisComment = "RootCauseAnalysisComment";
            public const string Waiver = "Waiver";
            public const string WaiverComment = "WaiverComment";
            public const string ExclusionException = "ExclusionException";
            public const string ExclusionExceptionComment = "ExclusionExceptionComment";
            public const string MissedVPMSLAs = "MissedVPMSLAs";
            public const string ContractChange = "ContractChange";
            public const string ContractChangeComment = "ContractChangeComment";
            public const string SLAComments = "SLAComments";
            public const string CustomizeFormat = "CustomizeFormat";
            public const string IsDisabled = "IsDisabled";
            public const string CategoryItemOrder = "CategoryItemOrder";
            public const string KeepItemOpen = "KeepItemOpen";
            public const string AutoAssignPRP = "AutoAssignPRP";
            public const string EmailToTicketSender = "EmailToTicketSender";
            public const string WorkingHoursStart = "WorkingHoursStart";
            public const string WorkingHoursEnd = "WorkingHoursEnd";
            public const string EnableNewTicketsOnHomePage = "EnableNewsOnHomePage";

            public const string ViewName = "ViewName";
            public const string StaticModulePagePath = "StaticModulePagePath";
           
            public static string WarrantyType = "WarrantyType";
            public static string WarrantyExpirationDate = "WarrantyExpirationDate";
            public static string NICAddress = "NICAddress";
            public static string IPAddress = "IPAddress";
            public static string HostName = "HostName";
            public static string AcquisitionDate = "AcquisitionDate";
            public static string SerialAssetDetail = "SerialAssetDetail";
            public static string ExternalTicketID = "ExternalTicketID";
            public static string OutageHours = "OutageHours";
            public static string SubLocationTagLookup = "SubLocationTagLookup";
           
            public const string Deleted = "Deleted";

            public static string UsableSqFt = "UsableSqFtNum";
            public static string RetailSqftNum = "RetailSqftNum";

            public static string UGITStateLookup = "UGITStateLookup";
            public static string StateLookup = "StateLookup";
            public static string CRMProjectStatus = "CRMProjectStatusChoice";

            public static string CheckListTemplates = "CheckListTemplates";
            public static string CheckListRoleTemplates = "CheckListRoleTemplates";
            public static string CheckListTaskTemplates = "CheckListTaskTemplates";
            public static string CheckLists = "CheckLists";
            public static string CheckListRoles = "CheckListRoles";
            public static string CheckListTasks = "CheckListTasks";
            public static string CheckListTaskStatus = "CheckListTaskStatus";
            public static string CheckListTemplateLookup = "CheckListTemplateLookup";
            public static string CheckListRoleLookup = "CheckListRoleLookup";
            public static string CheckListTaskLookup = "CheckListTaskLookup";
            public static string SubContractorLookup = "SubContractorLookup";
            public static string TaskLookup = "TaskLookup";

            public static string UGITFollowers = "UGITFollowers";

            public static string FieldSetName = "FieldSetName";
            public static string ProcoreMappingLookup = "ProcoreMappingLookup";
            public static string ProcoreUtilityLookup = "ProcoreUtilityLookup";

            public static string PreSetValue = "PreSetValue";
            // ReportConfig columns
            public const string ReportTitle = "ReportTitle";
            public const string ReportType = "ReportType";
            //DecisionLog
            public const string DecisionMaker = "DecisionMakerUser";
            //Project By Due Date
            public const string BusinessId = "BusinessId";
            public const string BusinessStrategyDescription = "BusinessStrategyDescription";
            public const string InitiativeDescription = "InitiativeDescription";
            public const string BusinessStrategy = "BusinessStrategy";
            public const string InitiativeId = "InitiativeId";
            public const string TotalAmount = "TotalAmount";
            public const string AmountLeft = "AmountLeft";
            public const string AllMonth = "AllMonth";
            public const string MonthLeft = "MonthLeft";
            public const string Issues = "Issues";
            public const string RiskLevel = "RiskLevel";
            public const string RiskLevelChoice = "RiskLevelChoice";
            public const string DisableNewConfirmation = "DisableNewConfirmation";
            public const string TenantID = "TenantID";





            public const string CheckListLookup = "CheckListLookup";
            public const string UGITCheckListTaskStatus = "UGITCheckListTaskStatus";
            public const string AutoLoadChecklist = "AutoLoadChecklist";
            public const string EstimateNo = "EstimateNo";

            public const string PunchListItems = "PunchListItems";
            public const string RFIs = "RFIs";
            public const string CCOs = "CCOs";
            public const string CurrentBudget = "CurrentBudget";
            public const string OriginalCommitments = "OriginalCommitments";
            public const string CostVariance = "CostVariance";
            public const string OrignalStartDate = "OrignalStartDate";
            public const string OrignalEndDate = "OrignalEndDate";
            public const string PctPlannedProfit = "PctPlannedProfit";
            public const string ClientChangeOrders = "ClientChangeOrders";
            public const string ProjectBillings = "ProjectBillings";
            public const string ActualStartDate = "ActualStartDate";
            public const string ActualEndDate = "ActualEndDate";
            public const string RevisedEndDate = "RevisedEndDate";
            public const string ProjectComplexity = "ProjectComplexityChoice";  //changed from crmprojectcomplexity to projectcomplexity
            public const string CRMProjectComplexity = "CRMProjectComplexityChoice";
            public const string AcquisitionCost = "AcquisitionCost";
            public const string ActualAcquisitionCost = "ActualAcquisitionCost";
            public const string ActualProjectCost = "ActualProjectCost";
            public const string ForecastedProjectCost = "ForecastedProjectCost";
            public const string CustomerReview = "CustomerReview";

            //UserControl helpers field
            public const string CompanyEngineer = "CompanyEngineer";
            public const string ConstructionManagementCompany = "ConstructionManagementCompany";
            public const string CompanyArchitect = "CompanyArchitect";
            public const string EngineerContact = "EngineerContact";
            public const string BuildingOwner = "BuildingOwner";
            public const string ConstructionManagerContact = "ConstructionManagerContact";
            public const string Architect = "Architect";
            public const string BrokerContact = "BrokerContact";
            public const string MarketSector = "MarketSector";

            public const string ProposalDeadline = "ProposalDeadline";
            public const string JobWalkthroughDate = "JobWalkthroughDate";
            public const string QuestionDueDate = "QuestionDueDate";
            public const string HardCopyDeadline = "HardCopyDeadline";
            public const string InterviewDate = "InterviewDate";
            public const string DecisionDate = "DecisionDate";

            public const string Phrase = "Phrase";
            public const string AgentType = "AgentType";
            public const string RequestType = "RequestType";
            public const string RequestTypeTitle = "RequestTypeTitle";
            public const string Use24x7Calendar = "Use24x7Calendar";

            //Tenant
            public const string AccountId = "AccountId";
            public const string TruncateTextTo = "TruncateTextTo";
            public const string Subscription = "Subscription";

             
            public static string ResolvedDate = "ResolvedDate";
            public static string ClosedDate = "ClosedDate";

            public static string EndOfDay = "EndOfDay";
            public static string NumCreated = "NumCreated";
            public static string NumResolved = "NumResolved";
            public static string NumClosed = "NumClosed";
            public static string TotalActive = "TotalActive";
            public static string TotalOnHold = "TotalOnHold";
            public static string TotalResolved = "TotalResolved";
            public static string TotalClosed = "TotalClosed";
            public static string EscalationManager = "EscalationManagerUser";
             
            public const string CustomPropertiesOther = "CustomPropertiesOther";
            public const string DisableLinks = "DisableLinks";
            
            public const string DataEditor = "DataEditor";
             
            
             
            public const string Weight = "Weight";
            
            public const string LocalAdminUser = "LocalAdminUser";
            public const string SerialNum1 = "SerialNum1";
            public const string SerialNum2 = "SerialNum2";
            public const string SerialNum3 = "SerialNum3";
            public const string Manufacturer = "ManufacturerChoice";
             
            public const string RegisteredBy = "RegisteredByUser";
            public const string ReplacementType = "ReplacementTypeChoice";
            public const string SetupCompletedBy = "SetupCompletedByUser";
            public const string InstalledBy = "InstalledByUser";
            public const string PreviousOwner1 = "PreviousOwner1User";
            public const string PreviousOwner2 = "PreviousOwner2User";
            public const string PreviousOwner3 = "PreviousOwner3User";
            public const string IconBlob = "IconBlob";
            public const string AdoptionRiskChoice = "AdoptionRiskChoice";
            public const string AnalyticsArchitecture = "AnalyticsArchitecture";
            
            public const string StatusChanged = "StatusChanged";
            public const string ManagerApprovalNeeded = "ManagerApprovalNeeded";
            public const string AnalyticsCost = "AnalyticsCost";
            public const string AnalyticsRisk = "AnalyticsRisk";
            public const string ROI = "ROI";
            public const string AnalyticsROI = "AnalyticsROI";
            public const string AnalyticsSchedule = "AnalyticsSchedule";
            public const string CannotStartBefore = "CannotStartBefore";
            public const string CannotStartBeforeNotes = "CannotStartBeforeNotes";
            public const string CapitalExpense = "CapitalExpense";
            public const string CapitalExpenditureNotes = "CapitalExpenditureNotes";
            public const string ClassificationNotes = "ClassificationNotes";
            
            public const string ClassificationScopeChoice = "ClassificationScopeChoice";
            public const string TicketClassificationScope = "ClassificationScope";
            public const string ComplexityNotes = "ComplexityNotes";
            public const string ConstraintNotes = "ConstraintNotes";
            public const string ITSteeringCommitteeApproval = "ITSteeringCommitteeApproval";
            public const string DesiredCompletionDateNotes = "DesiredCompletionDateNotes";
             
            public const string ImprovesOperationalEfficiency = "ImprovesOperationalEfficiency";
            public const string ImprovesRevenues = "ImprovesRevenues";
           
            public const string IsProjectBudgeted = "IsProjectBudgeted";
            public const string ITGReviewApproval = "ITGReviewApproval";
    
            public const string MetricsNotes = "MetricsNotes";
            public const string NoAlternative = "NoAlternative";
            public const string NoAlternativeOtherDescribe = "NoAlternativeOtherDescribe";
            public const string NoOfConsultants = "NoOfConsultants";
            public const string NoOfConsultantsNotes = "NoOfConsultantsNotes";
            public const string NoOfReports = "NoOfReports";
            public const string NoOfReportsNotes = "NoOfReportsNotes";
            public const string NoOfScreens = "NoOfScreens";
            public const string NoOfScreensNotes = "NoOfScreensNotes";
            public const string ProjectAssumptionsDescription = "ProjectAssumptionsDescription";
            public const string ProjectBenefits = "ProjectBenefits";
            public const string ProjectBenefitsDescription = "ProjectBenefitsDescription";
            public const string ProjectComplexityChoice = "ProjectComplexityChoice";
            public const string ProjectScopeDescription = "ProjectScopeDescription";
            public const string PRPGroupUser = "PRPGroupUser";
            public const string RapidRequest = "RapidRequest";
            public const string ReducesCost = "ReducesCost";
            public const string RegulatoryCompliance = "RegulatoryCompliance";
            public const string ScheduleComplexity = "ScheduleComplexity";
            public const string SecurityManagerUser = "SecurityManagerUser";
            public const string ServiceLookUp = "ServiceLookUp";
            public const string SponsorsUser = "SponsorsUser";
            public const string StakeHoldersUser = "StakeHoldersUser";
            public const string Technology = "Technology";
            public const string TechnologyAvailability = "TechnologyAvailability";
            public const string TechnologyNotes1 = "TechnologyNotes1";
            public const string TechnologyRisk = "TechnologyRisk";
            public const string TotalOffSiteConsultantHeadcountNotes = "TotalOffSiteConsultantHeadcountNotes";
            public const string TotalOnSiteConsultantHeadcount = "TotalOnSiteConsultantHeadcount";
            public const string TotalOnSiteConsultantHeadcountNotes = "TotalOnSiteConsultantHeadcountNotes";
            public const string TotalStaffHeadcount = "TotalStaffHeadcount";
            public const string TotalStaffHeadcountNotes = "TotalStaffHeadcountNotes";
            public const string VendorSupportChoice = "VendorSupportChoice";
            public const string SharedServices = "SharedServices";
           
            public const string PaybackCostSavingsChoice = "PaybackCostSavingsChoice";
            public const string CustomerBenefitChoice = "CustomerBenefitChoice";
            public const string Body = "Body";
            
            public const string CostOfLabor = "CostOfLabor";
            public const string RCATypeChoice = "RCATypeChoice";
            public const string AltTicketIdField = "AltTicketIdField";
            public const string CloseoutDate = "CloseoutDate";
            public const string SoftAllocation = "SoftAllocation";
            public const string NonChargeable = "NonChargeable";
            public const string IsAllocInPrecon = "IsAllocInPrecon";
            public const string IsAllocInConst = "IsAllocInConst";
            public const string IsAllocInCloseOut = "IsAllocInCloseOut";
            public const string IsSoftAllocInPrecon = "IsSoftAllocInPrecon";
            public const string IsSoftAllocInConst = "IsSoftAllocInConst";
            public const string IsSoftAllocInCloseOut = "IsSoftAllocInCloseOut";
            public const string IsStartDateBeforePrecon = "IsStartDateBeforePrecon";
            public const string IsStartDateBetweenPreconAndConst = "IsStartDateBetweenPreconAndConst";
            public const string IsStartDateBetweenConstAndCloseOut = "IsStartDateBetweenConstAndCloseOut";

            public const string ExperiencedTags = "ExperiencedTags";
            public const string TagMultiLookup = "TagMultiLookup";
            public const string TagLookup = "TagLookup";
            public const string UserId = "UserId";
            public const string Complexity = "Complexity";
            public const string ResourceHoursPrecon = "ResourceHoursPrecon";
            public const string ResourceHoursActual = "ResourceHoursActual";
            public const string ResourceHoursBilledtoDate = "ResourceHoursBilledtoDate";
            public const string ResourceHoursRemaining = "ResourceHoursRemaining";
            public const string TotalResourceHours = "TotalResourceHours";
            public const string TotalResourceCost = "TotalResourceCost";
            public const string ResouceHoursBilled = "ResouceHoursBilled";
            public const string Volatility = "Volatility";
            public const string OpportunityTargetChoice = "OpportunityTargetChoice";
            public const string BidDueDate = "BidDueDate";
            public const string OpportunityTypeChoice = "OpportunityTypeChoice";

            public const string ProjectLeadUser = "ProjectLeadUser";
            public const string LeadEstimatorUser = "LeadEstimatorUser";
            public const string LeadSuperintendentUser = "LeadSuperintendentUser";
            public const string ShortName = "ShortName";
            
        }

        public static class ContentType
        {
            public const string DocumentLibraryManagementCT = "DocumentLibraryManagementCT";
            public const string FolderCT = "FolderCT";
            public const string ModuleRuleCT = "ModuleRuleCT";
            public const string ModuleTaskCT = "ModuleTaskCT";
            public const string DocumentApproveTaskCT = "DocumentApproveTaskCT";
        }

        public class DataTypes
        {
            public const string SMALLNUMBER = "System.Int32";//ADDED BY MUNNA
            public const string Number = "System.Int64";
            public const string UserType = "UserType";
            public const string Lookup = "Lookup";
            public const string MultiLookup = "MultiLookup";
            public const string Choice = "Choice";
            public const string Attachment = "";
            public const string DateTime = "System.DateTime";
            public const string Boolean = "Boolean";
            public const string Currency = "Currency";
            public const string Counter = "Counter";
            public const string UserField = "UserField";
            public const string Double = "Double";
            public const string Decimal = "Decimal";
            public const string Choices = "Choices";
        }

        public static class Views
        {
            public const string DashboardSummary = "vw_DashboardSummary";
            public const string ResourceUsageSummaryWeekWise = "vw_ResourceUsageSummaryWeekWise";
            public const string ResourceUsageSummaryMonthWise = "vw_ResourceUsageSummaryMonthWise";
            public const string SprintSummary = "vw_SprintSummary";
        }
    }
}
