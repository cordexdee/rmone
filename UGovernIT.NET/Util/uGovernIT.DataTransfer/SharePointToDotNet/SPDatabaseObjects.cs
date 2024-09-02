using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.DataTransfer.SharePointToDotNet
{
    public class SPDatabaseObjects
    {
        /// <summary>
        /// List Names
        /// </summary>
        /// 
        public static class Lists
        {
            public static string MailTokenColumnName = "MailTokenColumnName";
            public static string ModuleFormTab = "ModuleFormTab";
            public static string ModuleStages = "ModuleStages";
            public static string RequestRoleWriteAccess = "RequestRoleWriteAccess";
            public static string FormLayout = "FormLayout";
            public static string ModuleColumns = "ModuleColumns";
            public static string Modules = "Modules";
            public static string RequestType = "RequestType";
            public static string NumUsers2 = "NumUsers2";
            public static string TicketImpact = "TicketImpact";
            public static string TicketSeverity = "TicketSeverity";
            public static string RequestPriority = "RequestPriority";
            public static string ModuleWorkflowHistory = "ModuleWorkflowHistory";
            public static string ModuleDefaultValues = "ModuleDefaultValues";
            public static string TaskEmails = "TaskEmails";
            public static string ModuleMonitors = "ModuleMonitors";
            public static string ModuleMonitorOptions = "ModuleMonitorOptions";
            public static string ProjectMonitorState = "ProjectMonitorState";
            public static string ProjectClass = "ProjectClass";
            public static string ProjectInitiative = "ProjectInitiative";
            public static string ACRTypes = "ACRTypes";
            public static string DRQSystemAreas = "DRQSystemAreas";
            public static string DRQRapidTypes = "DRQRapidTypes";
            public static string ProjectMonitorStateHistory = "ProjectMonitorStateHistory";
            public static string TicketTemplates = "TicketTemplates";
            public static string NPRRequest = "NPRRequest";
            public static string NPRTasks = "NPRTasks";

            public static string SessionIdConstant = "_uGIT_Id";
            public static string DashboardSummary = "DashboardSummary";
            public static string GenericTicketStatus = "GenericTicketStatus";
            public static string TicketPriority = "TicketPriority";
            public static string SLARule = "SLARule";
            public static string ChartFormula = "ChartFormula";
            public static string StageType = "StageType";
            // public static string ChartDimensions = "ChartDimensions";
            // public static string ChartDimensions2 = "ChartDimensions2";
            //  public static string ChartExpression = "ChartExpression";
            public static string ChartFilters = "ChartFilters";

            public static string ChartTemplates = "ChartTemplates";
            public static string ConfigurationVariable = "ConfigurationVariable";
            public static string ModuleUserTypes = "ModuleUserTypes";
            public static string TicketRelationship = "TicketRelationship";
            public static string TicketRelationships = "TicketRelationships";
            public static string SWQuestions = "SWQuestions";
            public static string SWChoices = "SWChoices";
            public static string RequestCategories = "RequestCategories";
            public static string ResourceAllocation = "ResourceAllocation";
            public static string ResourceTimeSheet = "ResourceTimeSheet";
            public static string ResourceWorkItems = "ResourceWorkItems";
            public static string PMMProjects = "PMMProjects";
            public static string ResourceUsageSummaryWeekWise = "ResourceUsageSummaryWeekWise";
            public static string ResourceUsageSummaryMonthWise = "ResourceUsageSummaryMonthWise";

            public static string Services = "Services";
            public static string ServiceSections = "ServiceSections";
            public static string ServiceTickets = "ServiceTickets";
            public static string ServiceTicketRelationships = "ServiceTicketRelationships";
            public static string ServiceTicketDefaultValues = "ServiceTicketDefaultValues";
            public static string ServiceQuestions = "ServiceQuestions";
            public static string ServiceVariables = "ServiceVariables";
            public static string ServiceCategories = "ServiceCategories";
            public static string PRSTicket = "PRSTicket";
            public static string SVCRequests = "SVCRequests";
            public static string ClientAdminConfigurationLists = "ClientAdminConfigurationLists";

            // add by shiv for Escalation
            public static string TicketEscalationQueue = "TicketEscalationQueue";
            public static string EscalationRule = "EscalationRule";
            public static string TicketStatusMapping = "TicketStatusMapping";
            public static string EscalationLog = "EscalationLog";
            public static string HolidayAndWorkingHours = "HolidaysAndWorkDaysCalendar";

            public static string BudgetCategories = "BudgetCategories";
            public static string NPRBudget = "NPRBudget";
            public static string NPRMonthlyBudget = "NPRMonthlyBudget";
            public static string ITGMonthlyBudget = "ITGMonthlyBudget";
            public static string DashboardPanels = "DashboardPanels";
            public static string ITGovernance = "ITGovernance";
            public static string DashboardFactTables = "DashboardFactTables";

            //add by Piyush
            public static string Department = "Department";
            public static string Location = "Location";
            public static string SubLocation = "SubLocation";

            public static string Assets = "Assets";
            public static string AssetReferences = "AssetReferences";
            public static string AssetVendors = "AssetVendors";
            public static string AssetModels = "AssetModels";
            public static string AssetRelations = "AssetRelations";
            public static string LeftSideNavigation = "LeftSideNavigation";
            public static string TSRTicket = "TSRTicket";
            public static string TicketsByMail = "TicketsByMail";
            public static string MailedTickets = "MailedTicket";

            // PMM
            public static string PMMProjectsHistory = "PMMProjectsHistory";
            public static string PMMTasks = "PMMTasks";
            public static string PMMBudget = "PMMBudget";
            public static string PMMMonthlyBudget = "PMMMonthlyBudget";
            public static string PMMComments = "PMMComments";
            public static string PMMIssues = "PMMIssues";
            public static string PMMBudgetActuals = "PMMBudgetActuals";
            public static string DecisionLog = "DecisionLog";

            public static string PMMTasksHistory = "PMMTasksHistory";
            public static string PMMBudgetHistory = "PMMBudgetHistory";
            public static string PMMMonthlyBudgetHistory = "PMMMonthlyBudgetHistory";
            public static string PMMCommentsHistory = "PMMCommentsHistory";
            public static string PMMIssuesHistory = "PMMIssuesHistory";

            public static string AssetIncidentRelations = "AssetIncidentRelations";
            public static string PMMBaselineDetail = "PMMBaselineDetail";
            public static string GlobalFilterPerDashboard = "GlobalFilterPerDashboard";
            public static string DashboardPanelGroup = "DashboardPanelGroup";
            public static string SprintTasks = "SprintTasks";
            public static string Sprint = "Sprint";
            public static string ProjectReleases = "ProjectReleases";
            public static string SprintSummary = "SprintSummary";
            public static string ModuleUserStatistics = "ModuleUserStatistics";

            // Incidents 
            public static string INCTicket = "INCTicket";

            //Project Summary Report Gantt Chart
            public static string ProjectSummary = "ProjectSummary";

            //TSK
            public static string TSKProjects = "TSKProjects";
            public static string TSKTasks = "TSKTasks";

            public static string MyModuleColumns = "MyModuleColumns";

            //ITG
            public static string ITGBudget = "ITGBudget";
            public static string ITGActual = "ITGActual";
            public static string Company = "Company";
            public static string CompanyDivisions = "CompanyDivisions";

            public static string DashboardPanelView = "DashboardPanelView";
            public static string ClientAdminCategory = "ClientAdminCategory";
            //Functional Area
            public static string FunctionalAreas = "FunctionalAreas";
            public static string EmailQueue = "EmailQueue";
            public static string UserRoles = "UserRoles";
            public static string RequestTypeByLocation = "RequestTypeByLocation";
            public static string MenuNavigation = "MenuNavigation";
            public static string MenuNavigationMobile = "MenuNavigationMobile";
            public static string MessageBoard = "MessageBoard";
            public static string PageEditor = "PageEditor";

            //Added by Sachin
            public static string GovernanceLinkCategory = "GovernanceLinkCategory";
            public static string GovernanceLinkItems = "GovernanceLinkItems";

            ///Agent Job Schduler
            public static string ScheduleActions = "ScheduleActions";
            public static string ScheduleActionsArchive = "ScheduleActionsArchive";
            public static string SurveyFeedback = "SurveyFeedback";
            public static string AnalyticDashboards = "AnalyticDashboards";

            ///Wiki
            public static string WikiArticles = "WikiArticles";
            public static string WikiDiscussion = "WikiDiscussion";
            public static string WikiLinks = "WikiLinks";
            public static string WikiReview = "WikiReview";

            ///Application Management
            public static string Applications = "Applications";
            public static string ApplicationModules = "ApplicationModules";
            public static string ApplicationRole = "ApplicationRole";
            public static string ApplModuleRoleRelationship = "ApplModuleRoleRelationship";
            public static string Environment = "Environment";
            public static string ApplicationServers = "ApplicationServers";
            public static string ApplicationPassword = "ApplicationPassword";

            //Module Stage Preconditions
            public static string ModuleStageConstraints = "ModuleStageConstraints";
            public static string ProjectLifeCycles = "ProjectLifeCycles";
            public static string ProjectLifeCycleStages = "ProjectLifeCycleStages";
            public static string UGITTaskTemplates = "UGITTaskTemplates";
            public static string TaskTemplateItems = "TaskTemplateItems";
            public static string ModuleStageConstraintTemplates = "ModuleStageConstraintTemplates";
            public static string ACRTicket = "ACRTicket";
            public static string BTSTicket = "BTSTicket";
            public static string DRQTicket = "DRQTicket";
            public static string NPRTicket = "NPRTicket";
            //Document Management
            public static string PortalInfo = "PortalInfo";
            public static string DocTypeInfo = "DocTypeInfo";
            //public static string WorkflowTask = "WorkflowTask";
            public static string WorkflowHistory = "Workflow History";
            public static string PingInformation = "PingInformation";
            public static string History = "History";
            public static string ConfigurationList = "DMConfigTable";
            public static string DocumentWorkflowHistory = "DocumentWorkflowHistory";

            public static string DMDepartment = "DMDepartment";
            public static string Vendor = "DMVendor";
            public static string DocumentType = "DocumentType";
            public static string DocumentInfoList = "DMDocumentInfo";
            public static string Projects = "DMProjects";
            public static string Tags = "DMTags";
            public static string DocumentHistory = "DMDocumentHistory";
            public static string DocumentLinkList = "DMDocumentLink";
            public static string UserSkills = "UserSkills";
            public static string ResourceAllocationMonthly = "ResourceAllocationMonthly";

            public static string EventCategories = "EventCategories";
            public static string PMMEvents = "PMMEvents";
            public static string ModuleWorkflowHistoryArchive = "ModuleWorkflowHistory_Archive";
            public static string HolidaysAndWorkDaysCalendar = "HolidaysAndWorkDaysCalendar";

            public static string ADUserMapping = "ADUserMapping";
            public static string TicketEmails = "TicketEmails";
            public static string UGITLog = "UGITLog";
            public static string WikiLeftNavigation = "WikiLeftNavigation";
            public static string UserInformationList = "User Information List";
            public static string Contracts = "Contracts";
            public static string NPRResources = "NPRResources";
            public static string adminauth = "adminauth";
            public static string VendorSOWInvoiceDetail = "VendorSOWInvoiceDetail";
            public static string VendorSOWFees = "VendorSOWFees";
            public static string VendorSOWContImprovement = "VendorSOWContImprovement";
            public static string VendorResourceCategory = "VendorResourceCategory";
            public static string VendorServiceDuration = "VendorServiceDuration";
            public static string VendorSLAPerformance = "VendorSLAPerformance";
            public static string VendorReport = "VendorReport";
            public static string VendorMSAMeeting = "VendorMSAMeeting";
            public static string VendorSOW = "VendorSOW";
            public static string VendorSLA = "VendorSLA";
            public static string VendorMSA = "VendorMSA";
            public static string VCCRequest = "VCCRequest";
            //Investment Management
            public static string InvDistribution = "InvDistribution";
            public static string Investments = "Investments";
            public static string Investors = "Investors";

            //BCS: External List
            public static string DepartmentExternal = "DepartmentExternal";
            public static string UserExternal = "UserExternal";
            public static string LinkView = "LinkView";
            public static string LinkCategory = "LinkCategory";
            public static string LinkItems = "LinkItems";

            public static string VendorSOWInvoices = "VendorSOWInvoices";
            public static string VendorIssues = "VendorIssues";

            //Asset Management
            public static string ImageSoftware = "ImageSoftware";
            public static string AssetsStatus = "AssetsStatus";
            public static string VendorRisks = "VendorRisks";
            public static string PMMRisks = "PMMRisks";
            public static string VendorApprovedSubcontractors = "VendorApprovedSubcontractors";
            public static string VendorKeyPersonnel = "VendorKeyPersonnel";

            ///CRM: Customer Related Lists
            public static string PLCRequest = "PLCRequest";
            public static string Customers = "Customers";
            public static string ModuleTasks = "ModuleTasks";

            //OPM: Oppurtunity Management
            public static string Organization = "Organization";
            public static string Contacts = "Contacts";
            public static string Resources = "Resources";
            public static string MonthlyBudget = "MonthlyBudget";
            public static string Bid = "Bid";

            //TicketToEmail
            public static string EmailToTicket = "EmailToTicket";
            //VND
            public static string VendorReportInstance = "VendorReportInstance";
            public static string VendorPOLineItems = "VendorPOLineItems";
            public static string VendorPO = "VendorPO";
            public static string ImageSoftwareMap = "ImageSoftwareMap";
            //Resource Management
            public static string ResourceManagement = "ResourceManagement";


            public static string setugittheme = "setugittheme";
            public static string ComposedLooks = "Composed Looks";

            public static string SiteAssets = "Site Assets";
            public static string VendorVPM = "VendorVPM";
            public static string TicketWorkflowSLASummary = "TicketWorkflowSLASummary";

            //TicketEmailFooter list
            public static string TicketEmailFooter = "TicketEmailFooter";
            public static string EnableMigrate = "EnableMigrate";

            public static string Themes = "Theme Gallery";

            public static string ModuleAgent = "ModuleAgent";

            public static string AssetIntegrationConfiguration = "AssetIntegrationConfiguration";
            public static string BusinessStrategy = "BusinessStrategy";
            public static string CRMProjectAllocation = "CRMProjectAllocation";
            public static string VendorType = "VendorType";
            public static string TicketEvents = "TicketEvents";
            public static string TicketHours = "TicketHours";
            public static string ProjectSimilarityConfig = "ProjectSimilarityConfig";
            public static string State = "State";
            public static string AnsiCodes = "AnsiCodes";
            public static string AdditionalRemarksCodeList = "AdditionalRemarksCodeList";
            public static string TicketRefreshList = "TicketRefreshList";

            public static string ResourceTimeSheetSignOff = "ResourceTimeSheetSignOff";
            public static string FieldAliasCollection = "FieldAliasCollection";
            public static string ProjectAllocations = "ProjectAllocations";
            public static string ProjectStageHistory = "ProjectStageHistory";
            public static string ProjectStandardWorkItems = "ProjectStandardWorkItems";
            public static string Client = "Client";

            public static string EmployeeTypes = "EmployeeTypes";
            public static string TicketCountTrends = "TicketCountTrends";
        }

        public static class Columns
        {
            public static string SoftwareMajorVersion = "SoftwareMajorVersion";
            public static string SoftwareMinorVersion = "SoftwareMinorVersion";
            public static string SoftwarePatchRevision = "SoftwarePatchRevision";
            public static string NextPlannedMajorUpgrade = "NextPlannedMajorUpgrade";
            public static string NextUpgradeDate = "NextUpgradeDate";
            public static string NumberOfSeats = "NumberOfSeats";
            
            public static string HostingType = "HostingType";
            public static string FrequencyOfTypicalUse = "FrequencyOfTypicalUse";
            public static string FrequencyOfUpgradesNotes = "FrequencyOfUpgradesNotes";
            public static string FrequencyOfUpgrades = "FrequencyOfUpgrades";
            public static string VersionLatestRelease = "VersionLatestRelease";
            public static string LicenseBasis = "LicenseBasis";
            public static string Approver = "Approver";
            public static string Approver2 = "Approver2";
            public static string AppLifeCycle = "AppLifeCycle";
            public static string SecurityDescription = "SecurityDescription";
            public static string SupportedBrowsers = "SupportedBrowsers";
            public static string NumUsers2 = "NumUsers2";
            public static string Authentication = "Authentication";
            public static string IsPerformanceTestingDone = "IsPerformanceTestingDone";
            public static string RCARequested = "RCARequested";
            public static string RCADisabled = "RCADisabled";
            public static string Quantity = "Quantity";
            public static string Quantity2 = "Quantity2";
            public static string AfterHours = "AfterHours";
            public static string QuoteAmount = "QuoteAmount";
            public static string ChargeBackAmount = "ChargeBackAmount";
            public static string PODate = "PODate";
            public static string AssetCondition = "AssetCondition";
            public static string PackingListNumber = "PackingListNumber";
            public static string TicketDivisionManager = "TicketDivisionManager";
            public static string TicketBusinessManager2 = "TicketBusinessManager2";
            public static string TicketDepartmentManager = "TicketDepartmentManager";
            public static string ProjectCode = "ProjectCode";
            public static string LocationTag = "LocationTag";
            public static string UsesADAuthentication = "UsesADAuthentication";
            public static string UGITNewUserName = "UGITNewUserName";
            public static string UGITNewUserDisplayName = "UGITNewUserDisplayName";
            public static string UGITTitle = "UGITTitle";
            public static string RFSSubmissionDate = "RFSSubmissionDate";
            public static string RFSFormComplete = "RFSFormComplete";
            public static string TargetType = "TargetType";
            public static string GovernanceLinkCategoryLookup = "GovernanceLinkCategoryLookup";
            public static string Id = "ID";
            public static string TicketAge = "TicketAge";
            public static string TicketDueIn = "TicketDueIn";
            public static string SelfAssign = "SelfAssign";
            public static string Description = "Body";
            public static string TicketId = "TicketId";
            public static string TicketIdWithoutLink = "TicketIdWithoutLink";
            public static string TicketStatus = "TicketStatus";
            public static string TicketActualStartDate = "TicketActualStartDate";
            public static string TicketActualCompletionDate = "TicketActualCompletionDate";
            public static string TicketSecurityManager = "TicketSecurityManager";
            public static string TicketGLCode = "TicketGLCode";
            public static string TicketResolutionComments = "TicketResolutionComments";
            public static string TicketAnalysisDetails = "TicketAnalysisDetails";
            public static string TicketResolutionType = "TicketResolutionType";
            public static string TicketOnHold = "TicketOnHold";
            public static string TicketPriorityLookup = "TicketPriorityLookup";
            public static string ElevatedPriority = "ElevatedPriority";

            public static string TicketRequestTypeLookup = "TicketRequestTypeLookup";
            public static string RequestTypeDescription = "RequestTypeDescription";
            public static string LocationDescription = "LocationDescription";
            public static string TicketRequestTypeCategory = "TicketRequestTypeCategory";
            public static string TicketImpactLookup = "TicketImpactLookup";
            public static string TicketSeverityLookup = "TicketSeverityLookup";
            public static string TicketDesiredCompletionDate = "TicketDesiredCompletionDate";
            public static string TicketTargetCompletionDate = "TicketTargetCompletionDate";
            public static string TicketPctComplete = "TicketPctComplete";
            public static string RequestTypeOwner = "RequestTypeOwner";
            public static string TicketOwner = "TicketOwner";
            public static string TicketTicketDRBRManager = "TicketDRBRManager";
            public static string TicketPRP = "TicketPRP";
            public static string TicketORP = "TicketORP";
            public static string TicketTester = "TicketTester";
            public static string TicketBusinessManager = "TicketBusinessManager";
            public static string TicketApplicationManager = "TicketApplicationManager";
            public static string TicketInitiator = "TicketInitiator";
            public static string TicketRequestor = "TicketRequestor";
            public static string TicketComment = "TicketComment";
            public static string Actuals = "Actuals";
            public static string EmailID = "EMail";
            public static string EmailTitle = "EmailTitle";
            public static string EmailBody = "EmailBody";
            public static string IncludesStaffing = "IncludesStaffing";
            public static string AllowDraftMode = "AllowDraftMode";
            public static string FunctionalAreaLookup = "FunctionalAreaLookup";

            public static string Impact = "Impact";
            public static string Severity = "Severity";
            public static string Priority = "uPriority";
            public static string TaskPriority = "Priority";

            public static string TicketStakeHolders = "TicketStakeHolders";
            public static string TicketSponsors = "TicketSponsors";
            public static string TicketProjectScore = "TicketProjectScore";
            public static string TicketProjectManager = "TicketProjectManager";
            public static string TicketProjectDirector = "TicketProjectDirector";
            public static string ProjectCoordinators = "ProjectCoordinators";

            public static string ProjectMonitorWeight = "ProjectMonitorWeight";
            public static string ProjectMonitorNotes = "ProjectMonitorNotes";
            public static string ModuleMonitorOptionLEDClassLookup = "ModuleMonitorOptionLEDClassLookup";
            public static string ModuleMonitorMultiplier = "ModuleMonitorMultiplier";
            public static string TicketBeneficiaries = "TicketBeneficiaries";
            public static string RequestTypeEscalationManager = "RequestTypeEscalationManager";
            public static string RequestTypeBackupEscalationManager = "RequestTypeBackupEscalationManager";
            public static string KeyWords = "KeyWords";
            public static string EstProjectSpend = "EstProjectSpend";
            public static string EstProjectSpendComment = "EstProjectSpendComment";

            public static string TicketNPRIdLookup = "TicketNPRIdLookup";
            public static string NPRResourceLookup = "NPRResourceLookup";
            public static string ModuleMonitorOptionNameLookup = "ModuleMonitorOptionNameLookup";
            public static string ModuleMonitorNameLookup = "ModuleMonitorNameLookup";

            public static string TicketInfrastructureManager = "TicketInfrastructureManager";
            public static string TicketEstimatedHours = "TicketEstimatedHours";
            public static string TicketDeveloperTotalHours = "TicketDeveloperTotalHours";
            public static string TicketProjectReferenceDescription = "TicketProjectReferenceDescription";
            public static string TicketStageActionUsers = "TicketStageActionUsers";
            public static string TicketStageActionUserTypes = "TicketStageActionUserTypes";

            public static string StageTitle = "StageTitle";
            public static string ShortStageTitle = "ShortStageTitle";
            public static string StageWeight = "StageWeight";
            public static string StageApprovedStatus = "StageApprovedStatus";
            public static string StageRejectedStatus = "StageRejectedStatus";
            public static string StageReturnStatus = "StageReturnStatus";
            public static string CurrentStageStartDate = "CurrentStageStartDate";
            public static string LastSequence = "LastSequence";
            public static string LastSequenceDate = "LastSequenceDate";
            public static string StageApproveButtonName = "StageApproveButtonName";
            public static string StageRejectedButtonName = "StageRejectedButtonName";
            public static string StageReturnButtonName = "StageReturnButtonName";
            public static string ShowBaselineButtons = "ShowBaselineButtons";
            public static string StageAllApprovalsRequired = "StageAllApprovalsRequired";
            public static string StageClosedBy = "StageClosedBy";
            public static string StageClosedByName = "StageClosedByName";
            public static string ApproveActionDescription = "ApproveActionDescription";
            public static string RejectActionDescription = "RejectActionDescription";
            public static string ReturnActionDescription = "ReturnActionDescription";
            public static string SkipOnCondition = "SkipOnCondition";

            public static string ModuleName = "ModuleName";
            public static string MilestoneDescription = "MilestoneDescription";
            public static string MilestoneStartDate = "MilestoneStartDate";
            public static string MilestoneEndDate = "MilestoneEndDate";
            public static string TicketPMMIdLookup = "TicketPMMIdLookup";
            public static string ScrumLifeCycle = "ScrumLifeCycle";
            public static string MilestoneParentId = "MilestoneParentId";
            public static string ReloadCache = "ReloadCache";
            public static string ReturnCommentOptional = "ReturnCommentOptional";
            public static string ModuleId = "ModuleId";
            public static string ModuleTicketTable = "ModuleTicketTable";
            public static string ModuleStep = "ModuleStep";
            public static string ModuleStepLookup = "ModuleStepLookup";
            public static string ModuleAutoApprove = "ModuleAutoApprove";
            public static string ModuleDescription = "ModuleDescription";
            public static string ModelDescription = "ModelDescription";
            public static string ProjectMonitorName = "ProjectMonitorName";
            public static string ModuleMonitorOption = "ModuleMonitorOption";
            public static string UserPrompt = "UserPrompt";
            public static string AuthorizedToView = "AuthorizedToView";
            public static string AuthorizedToEdit = "AuthorizedToEdit";
            public static string AuthorizedToApprove = "AuthorizedToApprove";
            public static string ActionUser = "ActionUser";
            public static string DataEditor = "DataEditor";
            public static string ActionUserType = "ActionUserType";
            public static string EmailUserTypes = "EmailUserTypes";
            public static string UserWorkflowStatus = "UserWorkflowStatus";
            public static string ModuleHoldMaxStage = "ModuleHoldMaxStage";
            public static string FieldMandatory = "FieldMandatory";
            public static string EnableZoomIn = "EnableZoomIn";

            public static string DateIdentified = "DateIdentified";
            public static string DecisionStatus = "DecisionStatus";
            public static string DecisionSource = "DecisionSource";
            public static string DecisionMaker = "DecisionMaker";
            public static string DateAssigned = "DateAssigned";
            public static string Decision = "Decision";
            public static string DecisionDate = "DecisionDate";
            public static string Source = "Source";
            public static string AdditionalComments = "AdditionalComments";

            public static string TicketRisk = "TicketRisk";
            public static string TicketName = "TicketName";
            public static string TicketMonitors = "TicketMonitors";
            public static string DRReplicationChange = "DRReplicationChange";
            public static string DRQChangeType = "DRQChangeType";
            public static string TicketRequestType = "TicketRequestType";
            public static string OutageHours = "OutageHours";
            public static string RequestCategory = "RequestCategory";
            public static string RequestTypeSubCategory = "RequestTypeSubCategory";
            public static string TicketCreationDate = "TicketCreationDate";
            public static string GenericStatusLookup = "GenericStatusLookup";
            public static string ModuleNameLookup = "ModuleNameLookup";

            public static string ModuleStageMultiLookup = "ModuleStageMultiLookup";
            public static string ShowStageTransitionButtons = "ShowStageTransitionButtons";


            public static string ModuleIdLookup = "ModuleIdLookup";
            public static string GenericStatus = "GenericStatus";
            public static string SLACategory = "SLACategory";
            public static string SLAHours = "SLAHours";
            public static string SLATarget = "SLATarget";
            public static string SLAUnit = "SLAUnit";
            public static string MinThreshold = "MinThreshold";
            public static string MaxThreshold = "MaxThreshold";
            public static string HigherIsBetter = "HigherIsBetter";
            public static string Weightage = "Weightage";
            public static string Reward = "Reward";
            public static string Penalty = "Penalty";

            public static string AllowBatchClose = "AllowBatchClose";
            public static string AllowTicketDelete = "AllowTicketDelete";
            public static string HideWorkFlow = "HideWorkFlow";

            public static string TicketPriority = "TicketPriority";
            public static string SLAMet = "SLAMet";
            public static string DefaultUser = "DefaultUser";
            public static string TicketRapidRequest = "TicketRapidRequest";
            public static string DRQRapidTypeLookup = "DRQRapidTypeLookup";
            public static string WorkflowType = "WorkflowType";
            public static string EnableWorkflow = "EnableWorkflow";
            public static string EnableLayout = "EnableLayout";
            public static string TicketRequestTypeWorkflow = "TicketRequestTypeWorkflow";

            public static string TabId = "TabId";
            public static string TabName = "TabName";
            public static string TableName = "TableName";
            public static string ColumnName = "ColumnName";
            public static string ColumnValue = "ColumnValue";
            public static string Formula = "Formula";
            public static string FormulaValue = "FormulaValue";
            public static string StageType = "Stage Type";
            public static string Title = "Title";

            public static string UGITSubTaskType = "UGITSubTaskType";
            public static string AutoCreateUser = "AutoCreateUser";
            public static string AutoFillRequestor = "AutoFillRequestor";
            public static string QuestionID = "QuestionID";
            public static string QuestionProperties = "QuestionProperties";

            public static string ErrorMsg = "ErrorMsg";

            public static string Created = "Created";
            public static string TicketDescription = "TicketDescription";
            public static string StageTitleLookup = "StageTitleLookup";
            public static string EndStageTitleLookup = "EndStageTitleLookup";
            public static string SLADuration = "SLADuration";
            public static string Duration = "Duration";
            public static string ModuleStageType = "ModuleStageType";
            public static string StageTypeLookup = "StageTypeLookup";
            public static string ChartObject = "ChartObject";
            public static string ChartTemplateIds = "ChartTemplateIds";
            public static string ValueAsId = "ValueAsId";
            public static string TicketInitiatorResolved = "TicketInitiatorResolved";
            public static string KeyName = "KeyName";
            public static string KeyValue = "KeyValue";
            public static string ModuleRelativePagePath = "ModuleRelativePagePath";
            public static string UserTypes = "UserTypes";
            public static string TicketProjectReference = "TicketProjectReference";
            public static string ChildTicketId = "ChildTicketId";
            public static string ParentTicketId = "ParentTicketId";
            public static string ProjectID = "ProjectID";

            public static string SWQuestion = "SWQuestion";
            public static string SWQuestionType = "SWQuestionType";
            public static string ThemeColor = "ThemeColor";
            public static string Color = "Color";
            public static string Attachments = "Attachments";
            public static string ItemOrder = "ItemOrder";
            public static string NavigationType = "NavigationType";
            public static string StaticModulePagePath = "StaticModulePagePath";
            public static string RequestCategoryType = "RequestCategoryType";
            public static string ContentType = "ContentType";
            public static string WorkItemType = "WorkItemType";
            public static string WorkItem = "WorkItem";
            public static string WorkItemLink = "WorkItemLink";

            public static string PctAllocation = "PctAllocation";
            public static string Resource = "Resource";
            public static string AllocationStartDate = "AllocationStartDate";
            public static string AllocationEndDate = "AllocationEndDate";
            public static string Category = "Category";
            public static string RequestType = "RequestType";
            public static string CategoryLabel = "#Category#";
            public static string SubWorkItem = "SubWorkItem";
            public static string WorkDate = "WorkDate";
            public static string WorkDescription = "WorkDescription";
            public static string HoursTaken = "HoursTaken";
            public static string IsDeleted = "IsDeleted";
            public static string Deleted = "Deleted";
            public static string History = "History";
            public static string ResourceWorkItemLookup = "ResourceWorkItemLookup";
            public static string TicketActualHours = "TicketActualHours";
            public static string ResourceId = "ResourceId";
            public static string ModuleType = "ModuleType";
            public static string EnableRMMAllocation = "EnableRMMAllocation";
            public static string EnableEventReceivers = "EnableEventReceivers";
            public static string WorkItemID = "WorkItemID";
            public static string Manager = "Manager";
            public static string WeekStartDate = "WeekStartDate";
            public static string AllocationHour = "AllocationHour";
            public static string ActualHour = "ActualHour";
            public static string PctActual = "PctActual";
            public static string MonthStartDate = "MonthStartDate";

            public static string IsVariable = "IsVariable";
            public static string ServiceTicketTitleLookup = "ServiceTicketTitleLookup";
            public static string ServiceTitleLookup = "ServiceTitleLookup";
            public static string ServiceSectionsTitleLookup = "ServiceSectionsTitleLookup";
            public static string ServiceQuestionTitleLookup = "ServiceQuestionTitleLookup";
            public static string TicketSVCRequestLookup = "TicketSVCRequestLookup";
            public static string AskUser = "AskUser";
            public static string PickValueFrom = "PickValueFrom";
            public static string WebPartHelpText = "WebPartHelpText";

            public static string TicketRequestSource = "TicketRequestSource";
            public static string TicketReSubmissionDate = "TicketReSubmissionDate";
            public static string TicketTotalHoldDuration = "TicketTotalHoldDuration";
            public static string TicketOnHoldStartDate = "TicketOnHoldStartDate";

            public static string UserFieldXML = "UserFieldXML";
            public static string ShowEditButton = "ShowEditButton";
            public static string ShowPartialEdit = "ShowPartialEdit";
            public static string ShowWithCheckBox = "ShowWithCheckBox";
            public static string FieldDisplayWidth = "FieldDisplayWidth";
            public static string FieldDisplayName = "FieldDisplayName";
            public static string TemplateType = "TemplateType";
            public static string WebpartID = "WebpartID";
            public static string IsShowInSideBar = "IsShowInSideBar";
            public static string FontStyle = "FontStyle";
            public static string HeaderFontStyle = "HeaderFontStyle";
            public static string DashboardPanelInfo = "DashboardPanelInfo";
            public static string DashboardPermission = "DashboardPermission";
            public static string DashboardDescription = "DashboardDescription";
            public static string Modified = "Modified";
            public static string DashboardType = "DashboardType";
            public static string ListName = "ListName";
            public static string IsHideTitle = "IsHideTitle";
            public static string IsHideDescription = "IsHideDescription";
            public static string PanelWidth = "PanelWidth";
            public static string PanelHeight = "PanelHeight";
            public static string ITStaff = "ITStaff";
            public static string ITConsultant = "ITConsultant";
            public static string TicketITManager = "TicketITManager";
            public static string TicketTotalCost = "TicketTotalCost";
            public static string TicketTotalCostsNotes = "TicketTotalCostsNotes";
            public static string TicketTotalStaffHeadcount = "TicketTotalStaffHeadcount";
            public static string TicketTotalStaffHeadcountNotes = "TicketTotalStaffHeadcountNotes";
            public static string TicketTotalConsultantHeadcount = "TicketTotalConsultantHeadcount";
            public static string TicketTotalConsultantHeadcountNotes = "TicketTotalConsultantHeadcountNotes";
            public static string TicketRiskScore = "TicketRiskScore";
            public static string TicketRiskScoreNotes = "TicketRiskScoreNotes";
            public static string TicketArchitectureScore = "TicketArchitectureScore";
            public static string TicketArchitectureScoreNotes = "TicketArchitectureScoreNotes";
            public static string TicketNoOfFTEs = "TicketNoOfFTEs";
            public static string TicketNoOfConsultants = "TicketNoOfConsultants";
            public static string TicketConstraintNotes = "TicketConstraintNotes";
            public static string TicketNoOfFTEsNotes = "TicketNoOfFTEsNotes";
            public static string TicketProjectScoreNotes = "TicketProjectScoreNotes";
            public static string BudgetAcronym = "BudgetAcronym";
            public static string ProjectClassLookup = "ProjectClassLookup";
            public static string ProjectInitiativeLookup = "ProjectInitiativeLookup";

            public static string TicketClassification = "TicketClassification";
            public static string TicketClassificationType = "TicketClassificationType";
            public static string TicketClassificationImpact = "TicketClassificationImpact";
            public static string TicketClassificationSize = "TicketClassificationSize";
            public static string ProjectConstraints = "ProjectConstraints";
            public static string TicketClassificationNotes = "TicketClassificationNotes";
            public static string ProjectComplexity = "ProjectComplexity";

            public static string AssignedAnalyst = "AssignedAnalyst";
            public static string SharedServices = "SharedServices";
            public static string ApplicationMultiLookup = "ApplicationMultiLookup";
            public static string TicketStrategicInitiative = "TicketStrategicInitiative";

            public static string DisplayForClosed = "DisplayForClosed";
            public static string DisplayForReport = "DisplayForReport";
            public static string ColumnType = "ColumnType";

            public static string CustomProperties = "CustomProperties";
            public static string LocationMultLookup = "LocationMultLookup";
            public static string TicketProjectScope = "TicketProjectScope";
            public static string TicketProjectAssumptions = "TicketProjectAssumptions";
            public static string TicketProjectBenefits = "TicketProjectBenefits";
            public static string ProjectRiskNotes = "ProjectRiskNotes";
            public static string ProblemBeingSolved = "ProblemBeingSolved";

            public static string TicketTotalOffSiteConsultantHeadcount = "TicketTotalOffSiteConsultantHeadcount";
            public static string TicketTotalOnSiteConsultantHeadcount = "TicketTotalOnSiteConsultantHeadcount";
            public static string ModuleMonitorOptionIdLookup = "ModuleMonitorOptionIdLookup";
            public static string ProjectNote = "ProjectNote";
            public static string ProjectNoteType = "ProjectNoteType";
            public static string BaselineNum = "BaselineNum";
            public static string BaselineDate = "BaselineDate";
            public static string TicketNoOfConsultantsNotes = "TicketNoOfConsultantsNotes";
            public static string ProjectCostNote = "ProjectCostNote";
            public static string ProjectCost = "ProjectCost";
            public static string ProjectScheduleNote = "ProjectScheduleNote";
            public static string BaselineComment = "BaselineComment";
            public static string NavigationDescription = "NavigationDescription";
            public static string LeftNavigationLookup = "LeftNavigationLookup";
            public static string TabSequence = "TabSequence";

            public static string ContributionToStrategy = "ContributionToStrategy";
            public static string PaybackCostSavings = "PaybackCostSavings";
            public static string CustomerBenefit = "CustomerBenefit";
            public static string Regulatory = "Regulatory";
            public static string ITLifecycleRefresh = "ITLifecycleRefresh";

            public static string ProjectContractDecision = "ProjectContractDecision";
            public static string ProjectDataDecision = "ProjectDataDecision";
            public static string ProjectApplicationDecision = "ProjectApplicationDecision";
            public static string ContractStatus = "ContractStatus";
            public static string ProjectDataStatus = "ProjectDataStatus";
            public static string ProjectApplicationStatus = "ProjectApplicationStatus";
            public static string TicketOwner2 = "TicketOwner2";
            
            public static string TicketITManager2 = "TicketITManager2";
            public static string CostSavings = "CostSavings";
            public static string HighLevelRequirements = "HighLevelRequirements";
            public static string SpecificInclusions = "SpecificInclusions";
            public static string SpecificExclusions = "SpecificExclusions";
            public static string ProjectDeliverables = "ProjectDeliverables";

            public static string ManagerOnly = "ManagerOnly";
            public static string ITOnly = "ITOnly";
            public static string Groups = "Groups";
            public static string UPriority = "uPriority";
            public static string UGITImageUrl = "UGITImageUrl";
            public static string WithoutPanel = "WithoutPanel";
            public static string UGITHeight = "UGITHeight";
            public static string UGITWidth = "UGITWidth";
            public static string LoadDefaultValue = "LoadDefaultValue";
            public static string SectionName = "SectionName";
            public static string NotificationText = "NotificationText";
            public static string TicketToBeSentByDate = "TicketToBeSentByDate";
            public static string EmailOnDate = "EmailOnDate";
            public static string MailTo = "MailTo";
            public static string MailSubject = "MailSubject";
            //added by shiv for escalation functionality

            public static string EscalationRuleIDLookup = "EscalationRuleIDLookup";
            public static string SlaRuleIdLookup = "SLARuleIdLookup";
            public static string NextEscalationTime = "NextEscalationTime";
            public static string EscalationToEmails = "EscalationToEmails";
            public static string EscalationToRoles = "EscalationToRoles";
            public static string EscalationMinutes = "EscalationMinutes";
            public static string EscalationFrequency = "EscalationFrequency";
            public static string EscalationEmailBody = "EscalationEmailBody";
            public static string EscalationSentTime = "EscalationSentTime";
            public static string EscalationSentTo = "EscalationSentTo";
            public static string EscalationMailSubject = "EscalationMailSubject";
            public static string SLAType = "SLAType";
            public static string TicketRequestTypeMulLookup = "TicketRequestTypeMulLookup";
            public static string ServerFunctions = "ServerFunctions";
            // HolidayAndWorking hour List coloumn

            public static string WorkingHourFrom = "WorkdayStartTime";
            public static string WorkingHourTo = "WorkdayEndTime";
            public static string HolidayCategory = "Category";
            public static string EventDate = "EventDate";
            public static string EndDate = "EndDate";

            // ConfigTable Column

            public static string WorkingDays = "WorkingDays";
            public static string UseCalendar = "UseCalendar";
            public static string WeekendDays = "WeekendDays";
            public static string FieldName = "FieldName";

            //NPR Tasks
            public static string StartDate = "StartDate";
            public static string DueDate = "DueDate";
            public static string AssignedTo = "AssignedTo";
            public static string AssignedToID = "AssignedToID";
            public static string PercentComplete = "PercentComplete";
            public static string ParentTask = "ParentTask";
            public static string Predecessors = "Predecessors";
            public static string PredecessorsID = "PredecessorsID";
            public static string Status = "Status";
            public static string TaskGroup = "TaskGroup";
            public static string Body = "Body";
            public static string IsPrivate = "IsPrivate";
            public static string ProjectRank = "ProjectRank";
            public static string ProjectRank2 = "ProjectRank2";
            public static string ProjectRank3 = "ProjectRank3";
            public static string PredecessorsByOrder = "PredecessorsByOrder";
            public static string IsCritical = "IsCritical";
            public static string EstimatedHours = "EstimatedHours";
            public static string ProjectTitle = "ProjectTitle";
            public static string RequestedResources = "RequestedResources";
            public static string HourlyRate = "HourlyRate";
            //Budget
            public static string BudgetCategory = "BudgetCategory";
            public static string BudgetType = "BudgetType";
            public static string BudgetLookup = "BudgetLookup";
            public static string BudgetSubCategory = "BudgetSubCategory";
            public static string BudgetItem = "BudgetItem";
            public static string BudgetDescription = "BudgetDescription";
            public static string BudgetAmount = "BudgetAmount";
            public static string BudgetAmountWithLink = "BudgetAmountWithLink";
            public static string BudgetCOA = "BudgetCOA";
            public static string BudgetIdLookup = "BudgetIdLookup";
            public static string ResourceCost = "ResourceCost";
            public static string EstimatedCost = "EstimatedCost";
            public static string Variance = "Variance";
            public static string IsAutoCalculated = "IsAutoCalculated";
            public static string AllocationAmount = "AllocationAmount";
            public static string PMMBudgetLookup = "PMMBudgetLookup";
            public static string BudgetTypeCOA = "BudgetTypeCOA";
            public static string BudgetStatus = "BudgetStatus";
            public static string TicketCapitalExpenditure = "TicketCapitalExpenditure";
            public static string CapitalExpenditure = "CapitalExpenditure";

            public static string UserSkill = "UserSkill";

            //ITG
            public static string CostToCompletion = "CostToCompletion";
            public static string IsITGApprovalRequired = "IsITGApprovalRequired";
            public static string IsSteeringApprovalRequired = "IsSteeringApprovalRequired";
            public static string BudgetStartDate = "BudgetStartDate";
            public static string BudgetEndDate = "BudgetEndDate";
            public static string ITGBudgetLookup = "ITGBudgetLookup";
            public static string ActualCost = "ActualCost";
            public static string CompanyTitleLookup = "CompanyTitleLookup";
            public static string InvoiceNumber = "InvoiceNumber";
            public static string PONumber = "PONumber";
            public static string GLCode = "GLCode";
            public static string DepartmentDescription = "DepartmentDescription";
            public static string FunctionalAreaDescription = "FunctionalAreaDescription";
            public static string UGITOwner = "UGITOwner";
            public static string NonProjectPlannedTotal = "NonProjectPlanedTotal";
            public static string NonProjectActualTotal = "NonProjectActualTotal";
            public static string ProjectPlannedTotal = "ProjectPlanedTotal";
            public static string ProjectActualTotal = "ProjectActualTotal";
            public static string DivisionLookup = "DivisionLookup";

            //AssetVendors
            public static string VendorName = "VendorName";
            public static string Location = "VendorLocation";
            public static string VendorPhone = "VendorPhone";
            public static string VendorEmail = "VendorEmail";
            public static string VendorAddress = "VendorAddress";
            public static string ContactName = "ContactName";
            public static string VendorType = "Vendor Type";
            public static string ProductServiceDescription = "Product Service Description";
            public static string SupportHours = "SupportHours";

            //AssetModels
            public static string ModelName = "ModelName";
            public static string VendorLookup = "VendorLookup";


            //Assets
            public static string AssetName = "AssetName";
            public static string AssetDescription = "AssetDescription";
            public static string AssetOwner = "AssetOwner";
            public static string AssetTagNum = "AssetTagNum";
            public static string DepartmentLookup = "DepartmentLookup";
            public static string LocationLookup = "LocationLookup";
            public static string SubLocationLookup = "SubLocationLookup";
            public static string AssetModelLookup = "AssetModelLookup";
            public static string DeletionDate = "DeletionDate";
            public static string DeletedBy = "DeletedBy";
            public static string TSRIdLookup = "TSRIdLookup";
            public static string TSKIDLookup = "TSKIDLookup";
            public static string SoftwareTitle = "SoftwareTitle";
            public static string SoftwareVersion = "SoftwareVersion";
            public static string SoftwareKey = "SoftwareKey";
            public static string SubLocationTagLookup = "SubLocationTagLookup";
            public static string ParentAssetId = "ParentAssetId";
            public static string ParentAssetTag = "ParentAssetTag";
            public static string RelatedTicketId = "RelatedTicketId";
            public static string SerialNum1 = "SerialNum1";
            public static string SerialNum2 = "SerialNum2";
            public static string SerialNum3 = "SerialNum3";
            public static string Manufacturer = "Manufacturer";

            //AssetReference
            public static string AssetReferenceType = "AssetReferenceType";
            public static string AssetReferenceNum = "AssetReferenceNum";
            public static string AssetLookup = "AssetLookup";

            //AssetRelations
            public static string Asset1 = "Asset1";
            public static string Asset2 = "Asset2";
            public static string AssetTagNumLookup = "AssetTagNumLookup";


            public static string DashboardMultiLookup = "DashboardMultiLookup";
            public static string UGITDescription = "UGITDescription";
            public static string ManagerLookup = "ManagerLookup";
            public static string ResourceHourlyRate = "ResourceHourlyRate";
            public static string MobilePhone = "MobilePhone";
            public static string JobTitle = "JobTitle";
            public static string IT = "IT";
            public static string IsConsultant = "IsConsultant";
            public static string IsManager = "IsManager";
            public static string IsIT = "IsIT";

            //new entries for "out of office calender" start
            public static string EnableOutofOffice = "EnableOutofOffice";
            public static string LeaveToDate = "LeaveToDate";
            public static string LeaveFromDate = "LeaveFromDate";
            public static string DelegateUserOnLeave = "DelegateUserOnLeave";
            public static string DelegateUserFor = "DelegateUserFor";
            //new entries for "out of office calender" end

            public static string UserRole = "UserRole";
            public static string TicketUser = "TicketUser";
            public static string IsActionUser = "IsActionUser";
            public static string PRPGroup = "PRPGroup";
            //public static Guid ProjectCost;

            public static string IsDisplay = "IsDisplay";
            public static string IsUseInWildCard = "IsUseInWildCard";
            public static string FieldSequence = "FieldSequence";

            // IncidentsTracking
            public static string OccurrenceDate = "OccurrenceDate";
            public static string DetectionDate = "DetectionDate";
            public static string AffectedUsers = "AffectedUsers";
            public static string ImpactsOrganization = "ImpactsOrganization";
            public static string TicketOtherDescribe = "TicketOtherDescribe";
            
            //
            public static string HideSummary = "HideSummary";
            public static string HideThankYouScreen = "HideThankYouScreen";
            public static string TicketBAAnalysisHours = "TicketBAAnalysisHours";
            public static string TicketDeveloperCodingHours = "TicketDeveloperCodingHours";
            public static string TicketBATotalHours = "TicketBATotalHours";
            public static string TicketBATestingHours = "TicketBATestingHours";
            public static string TicketDeveloperSupportHours = "TicketDeveloperSupportHours";
            public static string TicketTotalHours = "TicketTotalHours";
            public static string OrganizationalImpact = "OrganizationalImpact";
            
            public static string CategoryName = "CategoryName";
            public static string ServiceDescription = "ServiceDescription";
            public static string IsActivated = "IsActivated";

            public static string ProjectPhasePctComplete = "ProjectPhasePctComplete";

            public static string RelatedRequestID = "RelatedRequestID";
            public static string PRSLookup = "PRSLookup";

            public static string MSProjectImportExportEnabled = "MSProjectImportExportEnabled";
            public static string AllocationID = "AllocationID";
            public static string TimeSheetID = "TimeSheetID";
            public static string CacheTable = "CacheTable";
            public static string CacheAfter = "CacheAfter";
            public static string CacheThreshold = "CacheThreshold";
            public static string ExpiryDate = "ExpiryDate";
            public static string CacheMode = "CacheMode";
            public static string RefreshMode = "RefreshMode";

            public static string TicketTargetStartDate = "TicketTargetStartDate";

            public static string ScheduledStartDateTime = "ScheduledStartDateTime";
            public static string ScheduledEndDateTime = "ScheduledEndDateTime";

            public static string EnableModule = "EnableModule";
            public static string ResourceName = "ResourceName";
            public static string ManagerName = "ManagerName";
            public static string TaskEstimatedHours = "TaskEstimatedHours";
            public static string TaskActualHours = "TaskActualHours";
            public static string ShowComment = "ShowComment";



            //Dasgboard Query

            public static string SubCategory = "SubCategory";
            public static string DashboardModuleMultiLookup = "DashboardModuleMultiLookup";

            public static string ServiceCategoryNameLookup = "ServiceCategoryNameLookup";
            public static string TokenName = "TokenName";
            public static string ConditionalLogic = "ConditionalLogic";
            public static string QuestionTypeProperties = "QuestionTypeProperties";
            public static string UGITTaskType = "UGITTaskType";
            public static string UGITTaskStatus = "UGITTaskStatus";
            public static string UGITPredecessors = "UGITPredecessors";
            public static string UGITStartDate = "UGITStartDate";
            public static string UGITEndDate = "UGITEndDate";
            public static string UGITCost = "UGITCost";
            public static string UGITAssignedTo = "UGITAssignedTo";
            public static string TicketCloseDate = "TicketCloseDate";
            public static string UGITSourceID = "UGITSourceID";
            public static string UGITViewType = "UGITViewType";
            public static string UGITShortName = "UGITShortName";
            public static string UGITDaysToComplete = "UGITDaysToComplete";
            public static string UGITLevel = "UGITLevel";
            public static string UGITContribution = "UGITContribution";
            public static string UGITChildCount = "UGITChildCount";
            public static string UGITDuration = "UGITDuration";
            public static string UGITEstimatedHours = "UGITEstimatedHours";
            public static string TaskBehaviour = "TaskBehaviour";
            public static string BehaviourIcon = "BehaviourIcon";
            public static string TicketClosed = "TicketClosed";

            public static string TicketRejected = "TicketRejected";

            public static string UGITNavigationType = "UGITNavigationType";
            public static string UGITProposedDate = "UGITProposedDate";
            public static string UGITProposedStatus = "UGITProposedStatus";

            public static string NextActivity = "NextActivity";
            public static string NextMilestone = "NextMilestone";
            public static string ServiceCategoryName = "ServiceCategoryName";
            public static string ServiceName = "ServiceName";

            //Report Configurator.
            public static string ReportDefinitionLookup = "ReportDefinitionLookup";
            public static string DashboardPanelId = "DashboardPanelId";
            public static string CreateParentServiceRequest = "CreateParentServiceRequest";
            public static string AllowAttachmentsToChild = "AllowAttachmentsToChild";
            public static string SectionConditionalLogic = "SectionConditionalLogic";
            public static string OwnerApprovalRequired = "OwnerApprovalRequired";
            public static string AllowServiceTasksInBackground = "AllowServiceTasksInBackground";

            public static string IsAllTaskComplete = "IsAllTaskComplete";
            public static string QuestionMapVariables = "QuestionMapVariables";

            //Category Configurator
            public static string ClientAdminCategoryLookup = "ClientAdminCategoryLookup";
            public static string UGITComment = "UGITComment";
            public static string UGITResolutionDate = "UGITResolutionDate";
            public static string UGITResolution = "UGITResolution";
            public static string VNDActionType = "VNDActionType";

            public static string AutoSend = "AutoSend";
            public static string IsUserNotificationRequired = "IsUserNotificationRequired";
            public static string UserRoleLookup = "UserRoleLookup";
            public static string LandingPage = "LandingPage";
            public static string Enabled = "Enabled";
            public static string TicketDuration = "TicketDuration";
            public static string SLADaysRoundUpDown = "SLADaysRoundUpDown";
            public static string EscalationDescription = "EscalationDescription";

            public static string MenuDisplayType = "MenuDisplayType";
            public static string NavigationUrl = "NavigationUrl";
            public static string MenuParentLookup = "MenuParentLookup";

            ///Agent Job Scheduler Columns     
            public static string StartTime = "StartTime";
            public static string ActionType = "ActionType";
            public static string EmailIDTo = "EmailIDTo";
            public static string EmailIDCC = "EmailIDCC";
            public static string Recurring = "Recurring";
            public static string RecurringInterval = "RecurringInterval";
            public static string RecurringEndDate = "RecurringEndDate";
            public static string Log = "Log";
            public static string AgentJobStatus = "AgentJobStatus";
            public static string NotifyInPlainText = "NotifyInPlainText";

            ///Contract management Module related columns
            public static string InitialCost = "InitialCost";
            public static string AnnualMaintenanceCost = "AnnualMaintenanceCost";
            public static string ContractStartDate = "ContractStartDate";
            public static string ContractExpirationDate = "ContractExpirationDate";
            public static string ReminderDate = "ReminderDate";
            public static string ReminderTo = "ReminderTo";
            public static string ReminderBody = "ReminderBody";
            public static string NeedReview = "NeedReview";
            public static string TicketFinanceManager = "TicketFinanceManager";
            public static string TicketLegal = "TicketLegal";
            public static string TicketPurchasing = "TicketPurchasing";
            public static string TermType = "TermType";
            public static string RepeatInterval = "RepeatInterval";
            public static string UserLocation = "UserLocation";
            public static string UserDepartment = "UserDepartment";
            public static string Rating1 = "Rating1";
            public static string Rating2 = "Rating2";
            public static string Rating3 = "Rating3";
            public static string Rating4 = "Rating4";
            public static string Rating5 = "Rating5";
            public static string Rating6 = "Rating6";
            public static string Rating7 = "Rating7";
            public static string Rating8 = "Rating8";
            public static string TotalRating = "TotalRating";
            public static string DashboardID = "DashboardID";
            public static string AnalyticID = "AnalyticID";
            public static string AnalyticName = "AnalyticName";
            public static string AnalyticVID = "AnalyticVID";

            public static string Author = "Author";
            public static string HideInServiceMapping = "HideInServiceMapping";

            //Wiki Columns
            public static string WikiDescription = "WikiDescription";
            public static string WikiScore = "WikiScore";
            public static string WikiFavorites = "WikiFavorites";
            public static string WikiLikedBy = "WikiLikedBy";
            public static string WikiDislikedBy = "WikiDislikedBy";
            public static string WikiLikesCount = "WikiLikesCount";
            public static string WikiDislikesCount = "WikiDislikesCount";
            public static string WikiHistory = "WikiHistory";
            public static string WikiViewsCount = "WikiViews";
            public static string IsLiked = "IsLiked";
            public static string IsDisLiked = "IsDisLiked";
            public static string WikiRequestType = "RequestTypeLookUp";
            public static string WikiSnapshot = "WikiSnapshot";

            ///Application Columns
            public static string ApplicationModulesLookup = "ApplicationModulesLookup";
            public static string ApplicationRoleModuleLookup = "ApplicationRoleModuleLookup";

            public static string EnvironmentLookup = "EnvironmentLookup";
            public static string APPTitleLookup = "APPTitleLookup";
            public static string ApplicationRoleAssign = "ApplicationRoleAssign";
            public static string ApplicationRoleLookup = "ApplicationRoleLookup";
            public static string AccessAdmin = "AccessAdmin";
            public static string SupportedBy = "SupportedBy";
            public static string AssetsTitleLookup = "AssetsTitleLookup";
            public static string APPUserName = "APPUserName";
            public static string Password = "Password";
            public static string EncryptedPassword = "EncryptedPassword";
            public static string APPPasswordTitle = "APPPasswordTitle";
            public static string AccessManageLevel = "AccessManageLevel";
            public static string Owner = "Owner";
            public static string BusinessManager = "Business Manager";

            public static string LinkTitle = "LinkTitle";
            public static string URL = "URL";
            public static string Comments = "Comments";
            public static string LinkDescription = "Description";
            public static string WikiUserType = "WikiUserType";
            public static string WikiAverageScore = "WikiAverageScore";
            public static string WikiRatingDetails = "WikiRatingDetails";
            public static string WikiDiscussionCount = "WikiDiscussionCount";
            public static string WikiLinksCount = "WikiLinksCount";
            public static string WikiServiceRequestCount = "WikiServiceRequestCount";

            //ModuleConstraints
            public static string TaskDueDate = "TaskDueDate";
            public static string ContentTypeId = "ContentTypeId";

            /// Message Board            
            public static string MessageType = "MessageType";
            public static string Expires = "Expires";
            public static string ProjectLifeCycleLookup = "ProjectLifeCycleLookup";
            public static string TaskTemplateLookup = "TaskTemplateLookup";
            public static string IsMilestone = "IsMilestone";
            public static string StageStep = "StageStep";
            public static string ShowInMobile = "ShowInMobile";

            // New Location and Functional Area columns
            public static string FunctionalAreaTitle = "FunctionalAreaTitle";
            public static string UGITState = "UGITState";
            public static string UGITCountry = "UGITCountry";
            public static string UGITRegion = "UGITRegion";
            public static string UserSkillMultiLookup = "UserSkillMultiLookup";

            //Document Management Columns
            public static string PortalName = "PortalName";
            public static string PortalOwner = "PortalOwner";
            public static string PortalDescription = "PortalDescription";
            public static string AlternateOwner = "AlternateOwner";
            public static string NumVersions = "NumVersions";
            public static string KeepDocsAlive = "KeepDocsAlive";
            public static string NumFiles = "NumFiles";
            public static string FolderName = "FolderName";
            public static string SizeOfFolder = "SizeOfFolder";
            public static string ReviewRequired = "ReviewRequired";
            public static string ModifiedBy = "Modified_x0020_By";
            public static string FileNavigationUrl = "FileNavigationUrl";
            public static string ImageUrl = "ImageUrl";
            public static string ToolTip = "ToolTip";
            public static string FolderOwner = "FolderOwner";
            public static string SizeOfFile = "SizeOfFile";
            public static string IsSizeUnlimited = "IsSizeUnlimited";
            public static string RestrictConfigureTypeOnly = "AllowAllTypes";
            public static string IsAllOfficeFormats = "IsAllOfficeFormats";
            public static string IsImage = "IsImage";
            public static string IsMultimedia = "IsMultimedia";
            public static string IsPdf = "IsPdf";
            public static string IsFolderProperties = "IsFolderProperties";
            public static string IsPortalProjectType = "IsPortalProjectType";
            public static string TypeOfFile = "TypeOfFile";
            public static string CheckInImage = "CheckInImage";
            public static string CheckInComment = "CheckInComment";
            public static string Extensions = "Extensions";
            public static string Type = "Type";
            public static string Red = "Red";
            public static string Yellow = "Yellow";
            public static string Green = "Green";
            public static string NumPings = "NumPings";
            public static string PingUser = "PingUser";
            public static string TaskID = "TaskID";
            public static string DMLocation = "Location";
            public static string CheckInAuthority = "CheckInAuthority";

            //History List Column
            //public static string DocumentID = "DocumentID";
            public static string DocName = "DocName";
            public static string DocLocation = "DocLocation";
            public static string EventData = "EventData";
            public static string EventType = "EventType";
            public static string EventTriggeredByUser = "EventTriggeredByUser";
            public static string EventOccuredOnTime = "EventOccuredOnTime";
            public static string DocumentStatus = "DMDocumentStatus";

            //DocumentWorkflow History table
            public static string DocWorkflowHistory = "DocWorkflowHistory";
            public static string DocumentUrl = "DocumentUrl";

            //Mislenious Column

            public static string CheckOutBy = "CheckOutBy";
            public static string Version = "Version";
            public static string VersionNo = "_UIVersionString";
            public static string FileSizeDisplay = "FileSizeDisplay";
            public static string DocIcon = "DocIcon";
            public static string ClassApplied = "ClassApplied";

            //public static string WorkflowLink = "WorkflowLink";
            // public static string WorkflowTaskStatus = "WorkflowTaskStatus";
            //public static string WorkflowStatus = "WorkflowStatus";
            public static string DocID = "DocID";

            public static string Name = "Name";
            public static string FileLeafRef = "FileLeafRef";
            //public static string Workflow = "Workflow";
            public static string PingComments = "PingComments";
            public static string DocumentID = "DocumentID";
            public static string IsDocumentID = "IsDocumentID";
            public static string NotifyOwnerBeforeDeletion = "NotifyOwnerBeforeDeletion";
            public static string NotifyAuthorBeforeDeletion = "NotifyAuthorBeforeDeletion";
            public static string NotifyOwnerOnDocumentUpload = "NotifyOwnerOnDocUpload";
            //public static string WorkflowName = "WorkflowName";

            public static string GroupName = "GroupName";
            public static string GroupOwner = "GroupOwner";
            public static string PermissionLevels = "PermissionLevels";
            public static string Url = "Url";
            public static string Readers = "Readers";
            public static string Authors = "Authors";
            public static string LinkFileName = "LinkFileName";
            public static string CCUser = "CCUser";
            public static string CopyTo = "CopyTo";

            public static string DMKeyValue = "DMKeyValue";

            public static string DMDepartment = "DocumentDepartment";
            public static string Vendor = "VendorName";
            public static string Customer = "CustomerName";
            public static string DocumentType = "DocumentType";

            public static string DMVendorLookup = "DMVendorLookup";
            // public static string CustomerLookup = "CustomerLookup";
            public static string DocumentTypeLookup = "DocumentTypeLookup";
            public static string DepartmentNameLookup = "DepartmentNameLookup";
            public static string FileName = "FileName";
            public static string Tags = "Tags";
            public static string DocumentComments = "DocumentComments";
            public static string UniqueId = "UniqueId";
            public static string DMSDescription = "DMSDescription";
            public static string ReviewStep = "ReviewStep";
            public static string CurrentApprover = "CurrentApprover";
            public static string DocumentVersion = "DocVersion";
            public static string ApprovedVersion = "ApprovedVersion";
            public static string NotifyAuthor = "NotifyAuthor";
            public static string NotifyReader = "NotifyReader";
            public static string KeepNYear = "KeepNYear";
            public static string Project = "Project";
            public static string ProjectLookup = "ProjectLookup";

            public static string NotifyOnReviewStart = "NotifyUserOnReviewStart";
            public static string NotifyOnReviewComplete = "NotifyOnReviewComplete";


            //It is fro file metadata field
            public static string DocumentControlID = "DocumentControlID";

            public static string PortalId = "PortalId";

            public static string Acronym = "Acronym";
            public static string PrefixAcronym = "PrefixAcronym";
            public static string OverrideReaders = "OverrideReaders";
            public static string TagName = "TagName";

            public static string HistoryDate = "HistoryDate";
            public static string Action = "Action";
            public static string SequenceNo = "SequenceNo";
            public static string ExpirationDate = "ExpirationDate";
            public static string ReviewCycle = "ReviewCycle";
            public static string NumOfReviewCycle = "NumOfReviewCycle";
            public static string TargetPortalID = "TargetPortalID";
            public static string LinkText = "LinkText";
            public static string FolderUrl = "FolderUrl";
            public static string FolderID = "FolderID";
            public static string IsShortcutLink = "IsShortcutLink";
            public static string DocRevision = "UVersion";
            public static string SourcePortalID = "SourcePortalID";
            public static string SourceFolderID = "SourceFolderID";
            public static string AlwaysReviewRequired = "AlwaysReviewRequired";
            public static string AlwaysNotifyOwnerOnReviewComplete = "AlwaysNotifyOwnerOnReviewComplete";
            public static string AlwaysNotifyOwnerOnReviewStart = "AlwaysNotifyOwnerOnReviewStart";

            public static string ReviewCycle1 = "ReviewCycle1";
            public static string ReviewCycle2 = "ReviewCycle2";
            public static string ReviewCycle3 = "ReviewCycle3";
            public static string ReviewCycle4 = "ReviewCycle4";
            public static string ReviewCycle5 = "ReviewCycle5";
            public static string ReviewCycle6 = "ReviewCycle6";
            public static string ReviewCycle7 = "ReviewCycle7";
            public static string ReviewCycle8 = "ReviewCycle8";
            public static string ReviewCycle9 = "ReviewCycle9";
            public static string ReviewCycle10 = "ReviewCycle10";

            public static string UseDefaultCategory = "UseDefaultCategory";
            public static string DMDocumentStatus = "DMDocumentStatus";

            public static string DMDocType1 = "DMDocType1";
            public static string DMDocType2 = "DMDocType2";
            public static string DMDocType3 = "DMDocType3";
            public static string DMDocType4 = "DMDocType4";
            public static string DMDocType5 = "DMDocType5";

            public static string DepartmentID = "DepartmentID";
            public static string VendorID = "VendorID";
            public static string DocumentTypeID = "DocumentTypeID";
            public static string Owners = "Owners";
            public static string IncludeActionUsers = "IncludeActionUsers";
            public static string CompanyMultiLookup = "CompanyMultiLookup";
            public static string DivisionMultiLookup = "DivisionMultiLookup";
            public static string AppReferenceInfo = "AppReferenceInfo";
            public static string ProjectSummaryNote = "ProjectSummaryNote";
            public static string EnableCache = "EnableCache";
            public static string SyncAppsToRequestType = "SyncAppsToRequestType";
            public static string ServiceApplicationAccessXml = "ServiceApplicationAccessXml";
            public static string TicketApprovedRFE = "TicketApprovedRFE";
            public static string TicketApprovedRFEAmount = "TicketApprovedRFEAmount";
            public static string TicketApprovedRFEType = "TicketApprovedRFEType";
            public static string IsLatestVersion = "IsLatestVersion";
            public static string ProjectHealth = "ProjectHealth";
            public static string SyncToRequestType = "SyncToRequestType";
            public static string SyncAtModuleLevel = "SyncAtModuleLevel";
            public static string PctPlannedAllocation = "PctPlannedAllocation";
            public static string PlannedAllocationHour = "PlannedAllocationHour";
            public static string ServiceWizardOnly = "ServiceWizardOnly";
            public static string RequestorDepartment = "RequestorDepartment";
            public static string RequestorCompany = "RequestorCompany";
            public static string RequestorDivision = "RequestorDivision";
            public static string TicketOnHoldTillDate = "TicketOnHoldTillDate";
            public static string OnHoldReason = "OnHoldReason";
            public static string UnapprovedAmount = "UnapprovedAmount";
            public static string IssueImpact = "IssueImpact";
            public static string AccomplishmentDate = "AccomplishmentDate";
            public static string UseDesiredCompletionDate = "UseDesiredCompletionDate";

            // Project Calendar
            public static string UGITStatus = "UGITStatus";
            public static string UGITEventType = "UGITEventType";
            public static string UGITItemColor = "UGITItemColor";
            public static string RecurrenceInfo = "RecurrenceInfo";
            public static string EventLocation = "Location";
            public static string fAllDayEvent = "fAllDayEvent";
            public static string OutOfOffice = "OutOfOffice";
            public static string ShowOnProjectCalendar = "ShowOnProjectCalendar";

            public static string AttachmentRequired = "AttachmentRequired";
            public static string ADProperty = "ADProperty";
            public static string UserProperty = "UserProperty";
            public static string EmailReplyTo = "EmailReplyTo";
            public static string EmailIDFrom = "EmailIDFrom";
            public static string StoreTicketEmails = "StoreTicketEmails";
            public static string WorkflowSkipStages = "WorkflowSkipStages";
            public static string IsIncomingMail = "IsIncomingMail";
            public static string SortOrder = "SortOrder";
            public static string IsAscending = "IsAscending";

            public static string Picture = "Picture";
            public static string UseInGlobalSearch = "UseInGlobalSearch";
            public static string AttachedDocuments = "AttachedDocuments";
            public static string TaskRepeatInterval = "TaskRepeatInterval";
            //Scrum
            public static string SprintLookup = "SprintLookup";
            public static string SprintOrder = "SprintOrder";
            public static string TaskStatus = "TaskStatus";
            public static string SprintDuration = "SprintDuration";
            public static string RemainingHours = "RemainingHours";
            public static string ReleaseID = "ReleaseID";
            public static string ReleaseDate = "ReleaseDate";
            public static string ReleaseLookup = "ReleaseLookup";


            public static string UGITAssignToPct = "UGITAssignToPct";
            public static string UGITWebsiteUrl = "UGITWebsiteUrl";

            //Skill Resource 
            public static string UserSkillLookup = "UserSkillLookup";
            public static string _ResourceType = "_ResourceType";

            //other 

            public static string Percent_Complete = "% Complete";
            public static string EstimatedRemainingHours = "EstimatedRemainingHours";
            public static string TaskReminderDays = "TaskReminderDays";
            public static string TaskReminderEnabled = "TaskReminderEnabled";

            public static string AutoAdjustAllocations = "AutoAdjustAllocations";
            public static string DocumentLibraryName = "DocumentLibraryName";
            public static string VendorMSALookup = "VendorMSALookup";
            public static string VendorSOWLookup = "VendorSOWLookup";
            public static string VendorSLALookup = "VendorSLALookup";
            public static string VendorResourceSubCategoryLookup = "VendorResourceSubCategoryLookup";
            public static string ResourceQuantity = "ResourceQuantity";
            public static string InvoiceItemAmount = "InvoiceItemAmount";
            public static string SOWInvoiceLookup = "SOWInvoiceLookup";
            public static string SOWInvoiceAmount = "SOWInvoiceAmount";
            public static string SOWInvoiceDate = "SOWInvoiceDate";
            public static string VendorSLAReportingStart = "VendorSLAReportingStart";
            public static string VendorSLAReportingDate = "VendorSLAReportingDate";
            public static string VendorSLAMet = "VendorSLAMet";
            public static string VendorSLAPerformanceNumber = "VendorSLAPerformanceNumber";
            public static string VendorVPMLookup = "VendorVPMLookup";
            public static string VendorSLANameLookup = "VendorSLANameLookup";
            public static string VendorSLALookupID = "VendorSLALookupID";
            public static string SLANumber = "SLANumber";
            public static string CRNumber = "CRNumber";

            public static string ContractValue = "ContractValue";
            public static string SOWInvoiceActualAmount = "SOWInvoiceActualAmount";

            //investment Management
            public static string InvestorID = "InvestorID";
            public static string InvestorShortName = "InvestorShortName";
            public static string InvestorName = "InvestorName";
            public static string StreetAddress = "StreetAddress";
            public static string WorkCity = "WorkCity";
            //public static string UGITState = "UGITState";
            public static string WorkZip = "WorkZip";
            public static string EmployerIdentificationNumber = "EmployerIdentificationNumber";
            public static string Responsible = "Responsible";
            public static string UpdateDate = "UpdateDate";
            public static string InvestorStatus = "InvestorStatus";
            public static string Custodian = "Custodian";
            public static string AddedDate = "AddedDate";
            public static string OtherAddress = "OtherAddress";
            public static string Contact = "Contact";
            public static string AccountType = "AccountType";
            public static string LastName = "LastName";
            public static string EmailAddress = "EmailAddress";

            public static string InvestorShortNameLookup = "InvestorShortNameLookup";
            public static string Investment = "Investment";
            public static string AcquireDate = "AcquireDate";
            public static string INVType = "INVType";
            public static string ExpectedExit = "ExpectedExit";
            public static string ReturnYield = "ReturnYield";
            public static string InvestmentManagers = "InvestmentManagers";

            public static string SLAsMissed = "SLAsMissed";
            public static string FixedFees = "FixedFees";
            public static string SOWNoOfUnit = "SOWNoOfUnit";
            public static string SOWUnitRate = "SOWUnitRate";
            public static string SOWFeeUnit = "SOWFeeUnit";
            public static string SOWFeeUnit2 = "SOWFeeUnit2";
            public static string SOWAdditionalUnitRate = "SOWAdditionalUnitRate";
            public static string SOWReducedUnitRate = "SOWReducedUnitRate";
            public static string SOWAnnualChangePct = "SOWAnnualChangePct";
            public static string SOWDeadBandPct = "SOWDeadBandPct";

            public static string VendorReportingType = "VendorReportingType";
            public static string ReportingFrequencyUnit = "ReportingFrequencyUnit";
            public static string ReportingFrequency = "ReportingFrequency";
            public static string ReportingSLA = "ReportingSLA";
            public static string SLAMissedPenalty = "SLAMissedPenalty";
            public static string ReportingRecepients = "ReportingRecepients";
            public static string ReportingStartDate = "ReportingStartDate";
            public static string ClientObligations = "ClientObligations";


            public static string VendorMeetingType = "VendorMeetingType";
            public static string VendorMeetingAgenda = "VendorMeetingAgenda";
            public static string MeetingFrequencyUnit = "MeetingFrequencyUnit";
            public static string MeetingFrequency = "MeetingFrequency";
            public static string MeetingParticipants = "MeetingParticipants";
            public static string MeetingMaterial = "MeetingMaterial";
            public static string ServiceStartDate = "ServiceStartDate";
            public static string ServiceEndDate = "ServiceEndDate";
            public static string InvestorIDLookup = "InvestorIDLookup";
            public static string InvestmentIDLookup = "InvestmentIDLookup";
            public static string DistributionType = "DistributionType";
            public static string DistributionDate = "DistributionDate";
            public static string DistributionQuarter = "DistributionQuarter";
            public static string DistributionAmount = "DistributionAmount";
            public static string VendorSOWFeeLookup = "VendorSOWFeeLookup";

            public static string ActionTypeData = "ActionTypeData";
            public static string Report = "Report";
            public static string AttachmentFormat = "AttachmentFormat";
            public static string AlertCondition = "AlertCondition";
            public static string VariableAmount = "VariableAmount";
            public static string GlobalFilterRequired = "GlobalFilterRequired";
            //TicketTemplate
            public static string FieldValues = "FieldValues";

            public static string HideInTicketTemplate = "HideInTicketTemplate";

            //RequestType list extension
            public static string AssignmentSLA = "AssignmentSLA";
            public static string ResolutionSLA = "ResolutionSLA";
            public static string CloseSLA = "CloseSLA";
            public static string RequestorContactSLA = "RequestorContactSLA";
            public static string AutoCreateDocumentLibrary = "AutoCreateDocumentLibrary";
            public static string NextSLAType = "NextSLAType";
            public static string NextSLATime = "NextSLATime";
            public static string ShowNextSLA = "ShowNextSLA";
            public static string SLAConfiguration = "SLAConfiguration";

            public static string RequestorContacted = "RequestorContacted";
            public static string VendorRiskImpact = "VendorRiskImpact";
            public static string RiskProbability = "RiskProbability";
            public static string MitigationPlan = "MitigationPlan";
            public static string ContingencyPlan = "ContingencyPlan";

            public static string LinkCategoryLookup = "LinkCategoryLookup";
            public static string LinkViewLookup = "LinkViewLookup";
            public static string EnableCustomReturn = "EnableCustomReturn";

            public static string VendorIssueImpact = "VendorIssueImpact";
            public static string SubContractorService = "SubContractorService";
            public static string ApprovedSubContractorName = "ApprovedSubContractorName";

            public static string EnableQuickTicket = "EnableQuickTicket";
            public static string ApproveIcon = "ApproveIcon";
            public static string ReturnIcon = "ReturnIcon";
            public static string RejectIcon = "RejectIcon";
            public static string VendorSOWNameLookup = "VendorSOWNameLookup";
            public static string VendorMSANameLookup = "VendorMSANameLookup";
            public static string SOWContinuousImprovementPct = "SOWContinuousImprovementPct";
            public static string SOWContinuousImprovementPeriod = "SOWContinuousImprovementPeriod";
            public static string SOWContinuousImprovementPeriodUnit = "SOWContinuousImprovementPeriodUnit";
            public static string DisableNewTicketConfirmation = "DisableNewTicketConfirmation";
            public static string TicketOwnerBinding = "TicketOwnerBinding";
            public static string AllowChangeTicketType = "AllowChangeTicketType";
            public static string AllowBatchEditing = "AllowBatchEditing";
            public static string AllowDeleteFunctionality = "AllowDeleteFunctionality";
            public static string KeyRefUniqueID = "KeyRefUniqueID";
            public static string AgreementNumber = "AgreementNumber";
            public static string AllowBatchCreate = "AllowBatchCreate";
            public static string AllowEscalationFromList = "AllowEscalationFromList";
            public static string AllowReassignFromList = "AllowReassignFromList";
            public static string TaskSkill = "TaskSkill";
            public static string TaskSkillId = "TaskSkillId";
            public static string DeskLocation = "DeskLocation";
            public static string AuthorizedToCreate = "AuthorizedToCreate";
            public static string NotificationEmail = "NotificationEmail";
            public static string ApproveLevelAmount = "ApproveLevelAmount";
            public static string MatchAllKeywords = "MatchAllKeywords";

            ///Customer
            public static string FirstName = "FirstName";
            public static string Company = "Company";
            public static string WorkAddress = "WorkAddress";
            public static string IdentifiedBy = "IdentifiedBy";
            public static string AcquiredBy = "AcquiredBy";
            public static string AccountManager = "AccountManager";
            public static string CampaignInfo = "CampaignInfo";
            public static string CampaignStrategy = "CampaignStrategy";
            public static string CustomerSource = "CustomerSource";
            public static string SourceDetails = "SourceDetails";
            public static string LeadStatus = "LeadStatus";
            public static string ChanceOfSuccess = "ChanceOfSuccess";
            public static string Quality = "Quality";
            public static string CampaignUsed = "CampaignUsed";
            public static string AccountName = "AccountName";
            public static string AccountAddress = "AccountAddress";
            public static string CustomerAccountSize = "CustomerAccountSize";
            public static string CustomerAccountType = "CustomerAccountType";
            public static string CustomerAccountVertical = "CustomerAccountVertical";
            public static string Revenues = "Revenues";
            public static string NoOfEmployee = "NoOfEmployee";
            public static string SICCode = "SICCode";
            public static string LinkedInPage = "LinkedInPage";
            public static string FacebookPage = "FacebookPage";
            public static string TwitterPage = "TwitterPage";

            ///Proposal
            public static string ITStaffSize = "ITStaffSize";
            public static string ComponentsNeeded = "ComponentsNeeded";
            public static string ScopeOfServices = "ScopeOfServices";
            public static string Price = "Price";
            public static string BillingStartDate = "BillingStartDate";
            public static string FollowUp = "FollowUp";
            public static string PlanResources = "PlanResources";
            public static string CRMTicketLookup = "CRMTicketLookup";
            public static string NoOfLicenses = "NoOfLicenses";

            public static string ShowTicketSummary = "ShowTicketSummary";

            public static string AcceptanceCriteria = "AcceptanceCriteria";
            public static string DeliverableAttributes = "DeliverableAttributes";
            public static string DeliverableMode = "DeliverableMode";
            public static string Editor = "Editor";

            public static string AutoCalculate = "AutoCalculate";
            public static string QuestionTitle = "QuestionTitle";


            public static string RequestorNotificationOnComment = "RequestorNotificationOnComment";
            public static string ActionUserNotificationOnComment = "ActionUserNotificationOnComment";
            public static string InitiatorNotificationOnComment = "InitiatorNotificationOnComment";
            public static string RequestorNotificationOnCancel = "RequestorNotificationOnCancel";
            public static string ActionUserNotificationOnCancel = "ActionUserNotificationOnCancel";
            public static string InitiatorNotificationOnCancel = "InitiatorNotificationOnCancel";

            public static string WaitingOnMeIncludesGroups = "WaitingOnMeIncludesGroups";
            public static string MyRequestIncludesGroups = "MyRequestIncludesGroups";
            public static string WaitingOnMeExcludesResolved = "WaitingOnMeExcludesResolved";
            public static string chkWaitingOnMeIncludeGroups = "chkWaitingOnMeIncludeGroups";
            public static string TargetURL = "TargetURL";
            //public static string ToolTip = "ToolTip";

            public static string SelectedTabNumber = "SelectedTabNumber";
            public static string ApproveButtonTooltip = "ApproveButtonTooltip";
            public static string RejectButtonTooltip = "RejectButtonTooltip";
            public static string ReturnButtonTooltip = "ReturnButtonTooltip";
            public static string TrimContentAfter = "TrimContentAfter";
            public static string TicketRequestTypeSubCategory = "TicketRequestTypeSubCategory";

            //new columns name for resource Allocation Monthly.
            public static string ResourceWorkItemType = "ResourceWorkItemType";
            public static string ResourceWorkItem = "ResourceWorkItem";
            public static string ResourceSubWorkItem = "ResourceSubWorkItem";
            public static string EnablePasswordExpiration = "EnablePasswordExpiration";
            public static string PasswordExpiryDate = "PasswordExpiryDate";
            public static string MGSSubmittedDate = "MGSSubmittedDate";

            public static string CompletionDate = "CompletionDate";
            public static string ResolutionTypes = "ResolutionTypes";
            public static string SortToBottom = "SortToBottom";
            public static string StageCapacityNormal = "StageCapacityNormal";
            public static string StageCapacityMax = "StageCapacityMax";

            public static string ShowBottleNeckChart = "ShowBottleNeckChart";
            public static string OpenTicketChart = "OpenTicketChart";
            public static string CloseTicketChart = "CloseTicketChart";
            public static string VIPTicket = "VIPTicket";
            public static string CompletedBy = "CompletedBy";

            //OPM: Oppurtunity Management
            public static string LegalName = "LegalName";
            public static string StreetAddress1 = "StreetAddress1";
            public static string StreetAddress2 = "StreetAddress2";
            public static string City = "City";
            public static string Zip = "Zip";
            public static string Address1 = "Address1";
            public static string Address2 = "Address2";
            public static string Telephone = "Telephone";
            public static string Fax = "Fax";
            public static string OrganizationType = "OrganizationType";
            public static string BusinessType = "BusinessType";
            public static string FederalID = "FederalID";
            public static string ContractorLicense = "ContractorLicense";
            public static string Division = "Division";
            public static string MasterAgreement = "MasterAgreement";
            public static string WorkRegion = "WorkRegion";
            public static string WorkType = "WorkType";
            public static string CRMStatus = "CRMStatus";
            public static string Trade = "Trade";
            public static string Certifications = "Certifications";
            public static string CompanyName = "CompanyName";
            public static string Notes = "Notes";
            public static string OrganizationNote = "OrganizationNote";
            public static string OrganizationStatus = "OrganizationStatus";


            public static string UGITFirstName = "UGITFirstName";
            public static string UGITMiddleName = "UGITMiddleName";
            public static string AddressedAs = "AddressedAs";
            public static string SecondaryEmail = "SecondaryEmail";
            public static string Mobile = "Mobile";
            public static string ContactType = "ContactType";
            public static string OrganizationLookup = "OrganizationLookup";

            //Bid
            public static string BidSequence = "BidSequence";
            public static string BidArea = "BidArea";
            public static string ContactLookup = "ContactLookup";
            public static string BidAmount = "BidAmount";
            public static string TargetAmount = "TargetAmount";
            public static string BidDate = "BidDate";

            //VND
            public static string ReportInstanceStatus = "ReportInstanceStatus";
            public static string NoDocumentNeeded = "NoDocumentNeeded";
            public static string ReportFrequencyType = "ReportFrequencyType";
            public static string ReportMonthFrequencyType = "ReportMonthFrequencyType";
            public static string ReminderDays = "ReminderDays";
            public static string VendorReportLookup = "VendorReportLookup";
            public static string UGITDueDate = "UGITDueDate";
            public static string ReceivedOn = "ReceivedOn";
            public static string AcceptedOn = "AcceptedOn";
            public static string EffectiveStartDate = "EffectiveStartDate";
            public static string EffectiveEndDate = "EffectiveEndDate";
            public static string VendorPOLineItemLookup = "VendorPOLineItemLookup";
            public static string VendorPOLookup = "VendorPOLookup";
            public static string VendorPOLineItems = "VendorPOLineItems";
            public static string LineItemNumber = "LineItemNumber";
            public static string BilledAmount = "BilledAmount";
            public static string PrespentAmount = "PrespentAmount";

            public static string ImageOptionLookup = "ImageOptionLookup";
            public static string OS = "OS";
            public static string ReminderType = "ReminderType";
            public static string IsVIP = "IsVIP";
            public static string VendorTimeZone = "VendorTimeZone";
            public static string SupportCredentials = "SupportCredentials";
            public static string AccountRepPhone = "AccountRepPhone";
            public static string AccountRepMobile = "AccountRepMobile";
            public static string AccountRepEmail = "AccountRepEmail";
            public static string AccountRepName = "AccountRepName";
            //Menu Bar Navigational Url
            public static string LinkUrl = "LinkUrl";
            public static string ParentID = "ParentID";

            //Theme
            public static string MasterPageUrl = "MasterPageUrl";
            public static string ThemeUrl = "ThemeUrl";

            public static string FontSchemeUrl = "FontSchemeUrl";
            public static string DisableWorkflowNotifications = "DisableWorkflowNotifications";
            public static string SendEvenIfStageSkipped = "SendEvenIfStageSkipped";
            public static string ReopenCount = "ReopenCount";


            // ResourceTab
            public static string RoleName = "RoleName";
            public static string Role = "Role";
            public static string JobProfile = "JobProfile";
            public static string Job = "Job";
            public static string Consultant = "Consultant";
            public static string TitleLink = "TitleLink";
            public static string ManagerLink = "ManagerLink";
            public static string Skills = "Skills";
            public static string FunctionalArea = "FunctionalArea";
            public static string LoginName = "LoginName";
            public static string LocationId = "LocationId";
            public static string RoleId = "RoleId";

            //To show user service input
            public static string UserQuestionSummary = "UserQuestionSummary";

            public static string EnableApproval = "EnableApproval";
            public static string Approvalstatus = "ApprovalStatus";
            public static string TaskAssignee = "TaskAssignee";
            public static string ApproverID = "ApproverID";

            public static string ApprovalType = "ApprovalType";
            public static string TaskActionUser = "TaskActionUser";
            public static string ApprovedBy = "ApprovedBy";
            public static string TextAlignment = "TextAlignment";
            public static string PreloadAllModuleTabs = "PreloadAllModuleTabs";

            public static string EnableModuleAgent = "EnableModuleAgent";
            public static string AutoRefreshListFrequency = "AutoRefreshListFrequency";

            //Site Assets
            public static string UserProfile = "User Profile";
            public static string SiteAssets = "SiteAssets";
            //Enable import export project
            public static string EnableProjectExportImport = "EnableProjectExportImport";
            //For user profile Phone
            public static string Phone = "Phone";
            public static string HideTicketFooter = "HideTicketFooter";
            public static string AssignmentSLAMet = "AssignmentSLAMet";
            public static string RequestorContactSLAMet = "RequestorContactSLAMet";
            public static string ResolutionSLAMet = "ResolutionSLAMet";
            public static string CloseSLAMet = "CloseSLAMet";
            public static string OtherSLAMet = "OtherSLAMet";
            public static string ALLSLAsMet = "ALLSLAsMet";

            //Ticket workflow SLA Summary
            public static string RuleNameLookup = "RuleNameLookup";
            public static string TargetTime = "TargetTime";
            public static string ActualTime = "ActualTime";
            public static string SLARuleName = "SLARuleName";
            public static string OnHoldDuration = "OnHoldDuration";
            public static string StartStageStep = "StartStageStep";
            public static string EndStageStep = "EndStageStep";
            public static string StartStageName = "StartStageName";
            public static string EndStageName = "EndStageName";
            public static string StageStartDate = "StageStartDate";
            public static string StageEndDate = "StageEndDate";
            public static string Module = "Module";
            public static string MessageId = "MessageId";
            public static string ServiceCategoryType = "ServiceCategoryType";
            public static string TicketTestingTotalHours = "TicketTestingTotalHours";
            public static string IssueTypeOptions = "IssueTypeOptions";
            public static string UGITIssueType = "UGITIssueType";

            public static string MenuBackground = "MenuBackground";
            public static string SubMenuStyle = "SubMenuStyle";
            public static string SubMenuItemPerRow = "SubMenuItemPerRow";
            public static string SubMenuItemAlignment = "SubMenuItemAlignment";
            public static string MenuName = "MenuName";
            public static string MenuFontColor = "MenuFontColor";
            public static string MenuItemSeparation = "MenuItemSeparation";
            public static string MenuTextAlignment = "TextMenuAlignment";


            public static string TicketEmailType = "TicketEmailType";

            public static string SPReported = "SPReported";
            public static string ITMSReported = "ITMSReported";
            public static string VendorPerformanceWaiver = "VendorPerformanceWaiver";
            //VPM
            public static string TotalSLAs = "TotalSLAs";
            public static string TotalSLAsMet = "TotalSLAsMet";
            public static string SLAsMetComment = "SLAsMetComment";
            public static string MissedSLAsComment = "MissedSLAsComment";
            public static string NotDueSLAs = "NotDueSLAs";
            public static string NotDueSLAsComment = "NotDueSLAsComment";
            public static string OtherSLAs = "OtherSLAs";
            public static string OtherSLAsComment = "OtherSLAsComment";
            public static string Timeliness = "Timeliness";
            public static string Completeness = "Completeness";
            public static string SLCreditsDue = "SLCreditsDue";
            public static string SLCreditsDueComment = "SLCreditsDueComment";
            public static string SLDefaults = "SLDefaults";
            public static string SLDefaultsComment = "SLDefaultsComment";

            public static string RootCauseAnalysis = "RootCauseAnalysis";
            public static string RootCauseAnalysisComment = "RootCauseAnalysisComment";
            public static string Waiver = "Waiver";
            public static string WaiverComment = "WaiverComment";
            public static string ExclusionException = "ExclusionException";
            public static string ExclusionExceptionComment = "ExclusionExceptionComment";
            public static string MissedVPMSLAs = "MissedVPMSLAs";
            public static string ContractChange = "ContractChange";
            public static string ContractChangeComment = "ContractChangeComment";
            public static string SLAComments = "SLAComments";
            public static string CustomizeFormat = "CustomizeFormat";
            public static string IsDisabled = "IsDisabled";
            public static string CategoryItemOrder = "CategoryItemOrder";
            public static string KeepItemOpen = "KeepItemOpen";
            public static string AutoAssignPRP = "AutoAssignPRP";
            public static string EmailToTicketSender = "EmailToTicketSender";
            public static string WorkingHoursStart = "WorkingHoursStart";
            public static string WorkingHoursEnd = "WorkingHoursEnd";
            public static string EnableNewTicketsOnHomePage = "EnableNewTicketsOnHomePage";
            public static string StagingId = "StagingId";
            public static string ExternalType = "ExternalType";
            public static string ExternalTicketID = "ExternalTicketID";
            public static string NICAddress = "NICAddress";
            public static string IPAddress = "IPAddress";
            public static string HostName = "HostName";
            public static string AcquisitionDate = "AcquisitionDate";
            public static string SerialAssetDetail = "SerialAssetDetail";
            public static string WarrantyType = "WarrantyType";
            public static string WarrantyExpirationDate = "WarrantyExpirationDate";

            public static string TotalAmount = "TotalAmount";
            public static string AmountLeft = "AmountLeft";
            public static string AllMonth = "AllMonth";
            public static string MonthLeft = "MonthLeft";
            public static string Issues = "Issues";
            public static string RiskLevel = "RiskLevel";
            public static string BusinessStrategy = "BusinessStrategy";
            public static string BusinessStrategyLookup = "BusinessStrategyLookup";
            public static string DateExpression = "DateExpression";
            public static string UserRoleType = "UserRoleType";
            public static string DRQSystemsLookup = "DRQSystemsLookup";
            public static string BusinessId = "BusinessId";
            public static string InitiativeId = "InitiativeId";
            public static string BusinessStrategyDescription = "BusinessStrategyDescription";
            public static string InitiativeDescription = "InitiativeDescription";
            public static string NumUsers = "NumUsers";
            public static string VendorTypeLookup = "VendorTypeLookup";
            public static string NumLicensesTotal = "NumLicensesTotal";
            public static string ServiceProvider = "ServiceProvider";
            public static string EmployeeID = "EmployeeID";

            public static string PRP = "PRP";
            public static string SubmittedBy = "SubmittedBy";
            public static string CurrentUser = "CurrentUser";
            public static string AdditionalReminderNeeded = "AdditionalReminderNeeded";
            public static string AdditionalReminderDays = "AdditionalReminderDays";
            public static string AdditionalReminderType = "AdditionalReminderType";
            public static string TotalActualHours = "TotalActualHours";
            public static string ActualHoursByUser = "ActualHoursByUser";
            public static string EnableCloseOnHoldExpire = "EnableCloseOnHoldExpire";
            public static string TicketClosedBy = "TicketClosedBy";
            public static string TicketResolvedBy = "TicketResolvedBy";
            public static string EmailStatus = "EmailStatus";
            public static string EmailError = "EmailError";
            public static string Remaining = "Remaining";
            public static string TicketEventTime = "TicketEventTime";
            public static string TicketEventBy = "TicketEventBy";
            public static string PlannedEndDate = "PlannedEndDate";
            public static string TicketEventType = "TicketEventType";
            public static string EventReason = "EventReason";
            public static string Automatic = "Automatic";
            public static string DisableStageExitCriteriaDelete = "DisableStageExitCriteriaDelete";
            public static string EnableTicketImport = "EnableTicketImport";
            public static string DisableAutoApprove = "DisableAutoApprove";
            public static string TicketAssignedBy = "TicketAssignedBy";
            public static string ReportingFrequencyPeriod = "ReportingFrequencyPeriod";
            public static string AutoApproveOnStageTasks = "AutoApproveOnStageTasks";
            public static string CreatedBy = "CreatedBy";
            public static string ProjectName = "ProjectName";
            public static string NumOptionTerms = "NumOptionTerms";
            public static string OptionTermPeriod = "OptionTermPeriod";
            public static string ProjectState = "ProjectState";
            public static string ServiceTag = "ServiceTag";
            public static string SubTaskTitle = "SubTaskTitle";
            public static string SubTaskId = "SubTaskId";
            public static string TaskActualStartDate = "TaskActualStartDate";
            public static string SLADisabled = "SLADisabled";
            public static string CompletionMessage = "CompletionMessage";
            public static string FCRCategorization = "FCRCategorization";
            public static string ShowTasksInProjectTasks = "ShowTasksInProjectTasks";
            public static string UGITStateLookup = "UGITStateLookup";
            public static string VideoLink = "VideoLink";
            public static string UGITWebsite = "UGITWebsite";
            public static string LinkforVideo = "LinkforVideo";
            public static string DepartmentFunctionChoice = "DepartmentFunctionChoice";
            public static string AnsiCode = "AnsiCode";
            public static string NotifyOnNewVersion = "NotifyOnNewVersion";
            public static string BulkRequestCount = "BulkRequestCount";
            public static string StartResolutionSLAFromAssigned = "StartResolutionSLAFromAssigned";
            public static string SPNumerator = "SPNumerator";
            public static string SPDenominator = "SPDenominator";
            public static string VMONumerator = "VMONumerator";
            public static string VMODenominator = "VMODenominator";
            public static string SPResult = "SPRResult";
            public static string VMOResult = "VMORResult";
            public static string ResultMatch = "RResultMatch";
            public static string ContractReference = "ContractReference";
            public static string BreakFix = "BreakFix";

            //Column for Rating icons
            public static string RatingIconPath = "RatingIconPath";
            public static string RatingValue = "RatingValue";
            public static string Key = "Key";
            public static string NotificationDisabled = "NotificationDisabled";
            public static string Attendees = "Attendees";
            public static string Agenda = "Agenda";
            public static string MeetingRoomLocation = "MeetingRoomLocation";
            public static string TeleconferenceBridge = "TeleconferenceBridge";
            public static string ScreenSharing = "ScreenSharing";
            public static string EstimateNo = "EstimateNo";
            public static string CRMProjectID = "CRMProjectID";
            public static string PlannedStartDate = "PlannedStartDate";
            public static string EstimatedStartDate = "EstimatedStartDate";
            public static string EstimatedEndDate = "EstimatedEndDate";
            public static string PctEstimatedAllocation = "PctEstimatedAllocation";

            public static string SignOffStatus = "SignOffStatus";
            public static string ShowDeleteButton = "ShowDeleteButton";

            public static string EmailEventType = "EmailEventType";
            public static string LabourCharges = "LabourCharges";
            public static string ScheduleWeekday = "ScheduleWeekday";
            public static string ScheduleWeekdayOfMonth = "ScheduleWeekdayOfMonth";
            public static string OptionalAttendees = "OptionalAttendees";
            public static string BudgetCategoryLookup = "BudgetCategoryLookup";
            public static string StandardWorkItem = "StandardWorkItem";
            public static string EMail = "EMail";
            public static string SipAddress = "SipAddress";
            public static string EnableTaskReminder = "EnableTaskReminder";
            public static string Reminders = "Reminders";
            public static string TruncateTextTo = "TruncateTextTo";

            // New NPR/PMM columns
            public static string ClientLookup = "ClientLookup";
            public static string CustomerProgram = "CustomerProgram";
            public static string ProductCode = "ProductCode";
            public static string ProductName = "ProductName";
            public static string Use24x7Calendar = "Use24x7Calendar";

            public static string IsInternal = "IsInternal";
            public static string Address = "Address";

            public static string VendorVPMNameLookup = "VendorVPMNameLookup";
            public static string KeyPersonnel = "KeyPersonnel";
            public static string HasAttachment = "HasAttachment";
            public static string NumWaiversRequested = "NumWaiversRequested";
            public static string NumWaiversGranted = "NumWaiversGranted";
            public static string ApprovedCost = "ApprovedCost";

            public static string EmployeeTypeLookup = "EmployeeTypeLookup";

            public static string EmployeeType = "EmployeeType";
            public static string NextMilestoneDate = "NextMilestoneDate";
            public static string TermDate = "TermDate";
            public static string ExtensionCriteria = "ExtensionCriteria";
            public static string RenewalCriteria = "RenewalCriteria";
            public static string NonContractual = "NonContractual";

            public static string ReportReceived = "ReportReceived";
            public static string InternalContact = "InternalContact";
            public static string VendorAccountNum = "VendorAccountNum";
            public static string AssetDisposition = "AssetDisposition";
            public static string KeepTicketCounts = "KeepTicketCounts";
            public static string InitiatedDate = "InitiatedDate";
            public static string AssignedDate = "AssignedDate";
            public static string ResolvedDate = "ResolvedDate";
            public static string ClosedDate = "ClosedDate";
            public static string EndOfDay = "EndOfDay";
            public static string NumCreated = "NumCreated";
            public static string NumResolved = "NumResolved";
            public static string NumClosed = "NumClosed";
            public static string TotalActive = "TotalActive";
            public static string TotalResolved = "TotalResolved";
            public static string TotalClosed = "TotalClosed";
            public static string TotalOnHold = "TotalOnHold";
            public static string ResolutionDate = "ResolutionDate";

            public static string SelectedTabs = "SelectedTabs";
            public static string FileLocation = "FileLocation";
            public static string CustomPropertiesOther = "CustomPropertiesOther";
            public static string DisableLinks = "DisableLinks";
            public static string DisableDiscussion = "DisableDiscussion";
            public static string DisableRelatedItems = "DisableRelatedItems";
            public static string RelatedItems = "RelatedItems";
            public static string LeftPaneExpanded = "LeftPaneExpanded";
            public static string BottomPaneExpanded = "BottomPaneExpanded";
            public static string ReviewStatus = "ReviewStatus";
            public static string ReviewType = "ReviewType";
            public static string TicketStatusChanged = "TicketStatusChanged";
            public static string AdoptionRisk = "AdoptionRisk";
            public static string AnalyticsArchitecture = "AnalyticsArchitecture";
            public static string ManagerApprovalNeeded = "ManagerApprovalNeeded";

            public static string AnalyticsCost = "AnalyticsCost";
            public static string AnalyticsRisk = "AnalyticsRisk";
            public static string TicketROI = "TicketROI";
            public static string AnalyticsROI = "AnalyticsROI";
            public static string AnalyticsSchedule = "AnalyticsSchedule";
            public static string BreakEvenIn = "TicketBreakEvenIn";
            public static string TicketClassificationScope = "TicketClassificationScope";
            public static string CannotStartBefore = "TicketCannotStartBefore";
            public static string CannotStartBeforeNotes = "TicketCannotStartBeforeNotes";
            public static string CapitalExpenditureNotes = "CapitalExpenditureNotes";
            public static string CapitalExpense = "TicketCapitalExpense";
            public static string ComplexityNotes = "TicketComplexityNotes";
            public static string ConstraintNotes = "TicketConstraintNotes";
            public static string ITSteeringCommitteeApproval = "ITSteeringCommitteeApproval";
            public static string ImpactBusinessGrowth = "ImpactBusinessGrowth";
            public static string EliminatesHeadcount = "TicketEliminatesHeadcount";
            public static string DesiredCompletionDateNotes = "TicketDesiredCompletionDateNotes";
            
            public static string ImpactDecisionMaking = "ImpactDecisionMaking";
            public static string ImpactIncreasesProductivity = "ImpactIncreasesProductivity";
            public static string ImpactReducesExpenses = "ImpactReducesExpenses";
            public static string ImpactReducesRisk = "ImpactReducesRisk";
            public static string ImpactRevenueIncrease = "ImpactRevenueIncrease";
            public static string ImprovesOperationalEfficiency = "TicketImprovesOperationalEfficiency";
            public static string ImprovesRevenues = "TicketImprovesRevenues";
            public static string InternalCapability = "InternalCapability";
            public static string IsProjectBudgeted = "IsProjectBudgeted";
            public static string ITGReviewApproval = "ITGReviewApproval";
            public static string ApproxContractValue = "ApproxContractValue";
            public static string MetricsNotes = "TicketMetricsNotes";
            public static string NoAlternative = "TicketNoAlternative";
            public static string NoAlternativeOtherDescribe = "TicketNoAlternativeOtherDescribe";
            public static string NoOfConsultants = "TicketNoOfConsultants";
            public static string NoOfConsultantsNotes = "TicketNoOfConsultantsNotes";
            public static string NoOfReports = "TicketNoOfReports";
            public static string NoOfReportsNotes = "TicketNoOfReportsNotes";
            public static string NoOfScreens = "TicketNoOfScreens";
            public static string NoOfScreensNotes = "TicketNoOfScreensNotes";
            public static string ProjectAssumptionsDescription = "TicketProjectAssumptionsDescription";
            public static string ProjectBenefits = "TicketProjectBenefits";
            public static string ProjectBenefitsDescription = "TicketProjectBenefitsDescription";
            public static string ProjectComplexityChoice = "ProjectComplexity";
            public static string ProjectEstDurationMaxDays = "ProjectEstDurationMaxDays";
            public static string ProjectEstDurationMinDays = "ProjectEstDurationMinDays";
            public static string ProjectEstSizeMaxHrs = "ProjectEstSizeMaxHrs";
            public static string ProjectEstSizeMinHrs = "ProjectEstSizeMinHrs";
            public static string ProjectScopeDescription = "TicketProjectScopeDescription";
            public static string PRPGroupUser = "PRPGroupUser";
            public static string RapidRequest = "TicketRapidRequest";
            public static string ReducesCost = "TicketReducesCost";
            public static string RegulatoryCompliance = "TicketRegulatoryCompliance";
            public static string ScheduleComplexity = "ScheduleComplexity";
            public static string SecurityManagerUser = "SecurityManagerUser";
            public static string ServiceLookUp = "ServiceLookUp";
            public static string SponsorsUser = "SponsorsUser";
            public static string StakeHoldersUser = "StakeHoldersUser";
            public static string StrategicInitiative = "TicketStrategicInitiative";
            public static string Technology = "TicketTechnology";
            public static string TechnologyAvailability = "TicketTechnologyAvailability";
            public static string TechnologyImpact = "TicketTechnologyImpact";
            public static string TechnologyIntegration = "TicketTechnologyIntegration";
            public static string TechnologyNotes1 = "TicketTechnologyNotes1";
            public static string TechnologyReliability = "TicketTechnologyReliability";
            public static string TechnologyRisk = "TicketTechnologyRisk";
            public static string TechnologySecurity= "TicketTechnologySecurity";
            public static string TechnologyUsability= "TicketTechnologyUsability";
            public static string TotalOffSiteConsultantHeadcountNotes = "TicketTotalOffSiteConsultantHeadcountNotes";
            public static string TotalOnSiteConsultantHeadcount = "TicketTotalOnSiteConsultantHeadcount";
            public static string TotalOnSiteConsultantHeadcountNotes = "TicketTotalOnSiteConsultantHeadcountNotes";
            public static string TotalStaffHeadcount = "TicketTotalStaffHeadcount";
            public static string TotalStaffHeadcountNotes = "TicketTotalStaffHeadcountNotes";
            public static string VendorSupportChoice = "VendorSupport"; 
            public static string ResolvedByUser = "TicketResolvedBy";
            public static string ProjectDuration = "TicketProjectDuration";
            public static string CostOfLabor = "CostOfLabor";
            public static string TicketCategory = "TicketCategory";
            public static string RCAType = "RCAType";
            public static string TicketTechnologySecurity = "TicketTechnologySecurity";
            public static string TicketTechnologyUsability = "TicketTechnologyUsability";
            public static string TicketTechnologyReliability = "TicketTechnologyReliability";
            public static string PctROI = "PctROI";
            public static string VendorSupport = "VendorSupport";
            public static string EndOfSupportDate = "EndOfSupportDate";
            public static string EndOfExtendedSupportDate = "EndOfExtendedSupportDate";
            public static string EndOfSoftwareMaintenanceDate = "EndOfSoftwareMaintenanceDate";
            public static string Standard = "Standard";
            public static string StandardReviewDate = "StandardReviewDate";
            public static string EndOfSecurityUpdatesDate = "EndOfSecurityUpdatesDate";
            public static string BackedUpComponents = "BackedUpComponents";
            public static string NonStandardConfiguration = "NonStandardConfiguration";
            public static string OrderNum = "OrderNum";
            public static string NextStandardReviewDate = "NextStandardReviewDate";
            public static string ContractLookup = "ContractLookup";
            public static string EndOfSaleDate = "EndOfSaleDate";
            public static string ProductReleaseDate = "ProductReleaseDate";
            public static string SSLCertExpiration = "SSLCertExpiration";
            public static string StandardRefreshPeriod = "StandardRefreshPeriod";
            public static string EndOfLifeDate = "EndOfLifeDate";
            public static string ProductionCritical = "ProductionCritical";
            public static string Firmware = "Firmware";
            public static string VersionNumber   = "EndOfLifeDate";
            public static string Unmanaged = "Unmanaged";
            public static string SSLCertName = "SSLCertName";
            public static string FinalCountermeasure = "FinalCountermeasure";

            public const string Archived = "Archived";
            public const string FriendlyName = "FriendlyName";
            public const string Handle = "Handle";
            public const string HasPrivateKey = "HasPrivateKey";
            public const string IssuerName = "IssuerName";
            public const string NotAfter = "NotAfter";
            public const string NotBefore = "NotBefore";
            public const string PublicKey = "PublicKey";
            public const string RawData = "RawData";
            public const string SignatureAlgorithm = "SignatureAlgorithm";
            public const string Subject = "Subject";
            public const string SubjectName = "SubjectName";
            public const string Thumbprint = "Thumbprint";
            public const string DnsNameList = "DnsNameList";
            public const string EnhancedKeyUsageList = "EnhancedKeyUsageList";
            public const string SendAsTrustedIssuer = "SendAsTrustedIssuer";
            public const string PaymentTerms = "PaymentTerms";
            public const string ManufacturingContact = "ManufacturingContact";
            public const string VendorContact = "VendorContact";
            public const string SalesRepName = "SalesRepName";
            public const string SalesRepContact = "SalesRepContact";
            public const string CurrentFundingProject = "CurrentFundingProject";
            public const string CurrentGWOFields = "CurrentGWOFields";
            

        }
    }
}
