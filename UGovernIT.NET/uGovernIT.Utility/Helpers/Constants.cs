
namespace uGovernIT.Utility
{
    public class Constants
    {
        public const string RegisterTenantMenuToolip = "Tenant Registration";
        public const string Cancelled = "Cancelled";
        public const string Template = "Template";
        public const string passwordQuestion = "What is your favorite color";
        public const string passwordAnswer = "Blue";
        public enum ProjectType { ActiveProject, CurrentProjects, ApprovedNPRs, PendingApprovalNPRs, RejectedProject, CompletedProjects, All, OnHold };

        public static string HoldButtonText = "Put on Hold";
        public static string UnHoldButtonText = "Remove Hold";
        public static string Display = "display";
        public static string ReturnButtonText = "Return";
        public static string RejectButtonText = "Reject";
        public static string ApproveButtonText = "Approve";
        public static string ReopenButtonText = "Re-Open";
        public static string OverallStatusKey = "[$OverallStatus$]";
        
        public static string ProjectIcon = "ProjectIcon";
        public static string FooterText = "FooterText";
        public static string ApprovedDefault = "Approved/Default";
        #region TimeSheet Constant variables
        public static string TimeSheetEmailSubject = "Timesheet Status";
        public const string TimeEntry = "Time Entry";
        public const string Returned = "Returned";
        public const string PendingApproval = "Pending Approval";
        public const string Approved = "Approved";
        public const string FunctionLockEditing = "LockEditing();";
        public const string CallLockEditing = "CallLockEditing";
        public const string SendForApproval = "SendForApproval";
        public const string Return = "Return";
        public const string TimeSheetReturnStatus = "Returned";
        public const string TimesheetPendingApprovalStatus = "Sent for Approval";
        public const string TimesheetApprovedStatus = "Approved";
        public const string Updated = "Updated";
        public const string User = "User";
        public const string UTC = "UTC";
        public const string Status = "Status";
        public const string Comment = "Comment";
        public const string SignOff = "Sign Off";
        public const string ApprovalMode = "Approval";
        public const string SignOffMode = "SignOff";
        #endregion TimeSheet Constant variables
        /// <summary>
        /// ;#
        /// </summary>
        public static string Separator = ";#";
        /// <summary>
        /// ;~
        /// </summary>
        /// 

        public static string Separator1 = ";~";
        //public static string Separator1 = ";#";
        /// <summary>
        /// ~
        /// </summary>      
        public static string Separator2 = "~";
        //public static string Separator2 = ":";
        /// <summary>
        /// ~
        /// </summary>
        public static string Separator3 = "~~";

        /// <summary>
        /// ~
        /// </summary>
        public static string Separator4 = "~#";
        public static string Separator5 = ";";
        public static string NewLineSeperator = "\n";
        /// <summary>
        /// /br
        /// </summary>
        public static string BreakLineSeparator = "</br>";
        /// <summary>
        /// ,
        /// </summary>
        public static string Separator6 = ",";
        public static string DashSeparator = "-";

        public static string SpaceSeparator = " ";
        public static string Blank = "";


        public static string Separator7 = ":";
        public static string Separator8 = "`#~";
        public static string Separator9 = "!#";
        public static string Separator10 = ",#";
        public static string Separator11 = ";# ";
        public static string UTCPrefix = "UTC:";
        public static string SeparatorForVersions = "<;#>";
        public static string BRTag = "<br/>";
        public static string UserInfoSeparator = "; ";
        public static string CommentSeparator = " | ";
        public static string CreateBaselineText = "New Baseline";
        public static string ShowBaselineText = "Show Baseline";
        public static string ImportNPRText = "Create PMM";
        public static string ShowProjectReportText = "Project Report";
        public static string ShowCIOReportText = "CIO Report";
        public static string TaskGanttName = "TaskGanttChart";
        public static string CloseButtonText = "Close Project";
        public static string StaffingBudget = "StaffingBudget";
        public static string OnHoldStatus = "On Hold";

        public static string FiscalStartDate = "FiscalStartDate";
        public static string FiscalEndDate = "FiscalEndDate";

        public static string TokenStart = "[$";
        public static string TokenEnd = "$]";
        public static string MailTokenStart = "MailTokenStart";
        public static string MailTokenValueSeparator = "MailTokenValueSeparator";
        public static string MailTokenEnd = "MailTokenEnd";
        public static string DesiredCompletionDateValidation = "DesiredCompletionDateValidation";

        //Workflow Types.
        public static string Assigned = "Assigned";

        public static string QuickClose = "Quick";
        public static string NoTest = "NoTest";
        public static string Requisition = "Requisition";

        public static string AutoApprove = "autoapprove";
        public static string DoNotWaitForActionUser = "donotwaitforactionuser";
        public static string AssetRelationDescriptionCreate = "Created relation with asset";
        public static string TicketRelationDescriptionCreate = "Created relation with ticket";

        public static string ErrorMsgBudgetItemRequired = "Please enter at least 1 Budget Item before submitting for IT Review.";
        public static string ErrorMsgRMMOverlappingDates = "Identical Work Items with overlapping dates are not allowed";
        public static string ErrorMsgRMMUnDeleted = "";
        public static string ErrorMsgRMMInvalidDates = "Allocation End Date cannot be before Start Date";
        public static string ErrorMsgRMMInvalidWorkItem = "Incomplete Work Item selection";
        public static string ErrorMsgRMMInvalidAllocationPct = "Invalid or Missing Allocation %";
        public static string ErrorMsgRMMDuplicateAllocation = "Overlapping allocations are not permitted. Save unsuccessful.";
        public static string ErrorMsgRMMStartAndEndDate = "StartDate should be less then EndDate.";
        public static string QuickTSRCategoryName = "QuickTSRCategoryName";
        //public static string HardwareCategoryName = "HardwareCategoryName";
        public static string TokenRegx = "\\[\\$[\\w |&\\//:@#%()-_'\"]*\\$\\]";

        public static string AllITDepartments = "-- All IT Departments";
        public static string AllDepartments = "-- All Departments";
        public static string AllGroups = "-- All Groups";
        public static string AllUsers = "All Users";

        // RMM Types
        public static string RMMLevel1PMMProjectsType = "PMM";
        public static string RMMLevel1TSKProjectsType = "TSK";
        public static string RMMLevel1NPRProjectsType = "NPR";

        public static string RMMShowOriginalTaskName = "RMMShowOriginalTaskName";

        public static string ProjectSummaryGanttView = "ProjectSummaryGanttView";

        public static string MobileHomepagePath = @"/_layouts/15/mobile/HomePage.aspx?";
        public static string MobileApproveRejectPath = @"/_layouts/15/mobile/ApproveRejectPage.aspx?";
        public static string MobileFilterTicketPath = @"/_layouts/15/mobile/FilterTickets.aspx?";
        public static string MobileTicketEdit = @"/_layouts/15/mobile/TicketDetail.aspx?";
        public static string MobileTicketNew = @"/_layouts/15/mobile/NewTicket.aspx?";
        public static string MobileDashboard = @"/_layouts/15/mobile/HomePage.aspx?";
        public static string MobileService = @"/_layouts/15/mobile/ServiceWizardPage.aspx?";
        public static string MobileEditTasks = @"/_layouts/15/mobile/EditTask.aspx?";

        // Cookie names
        public static string SortExpression = "SortExpression";
        public static string SortDirection = "SortDirection";
        public static string FilterExpression = "FilterExpression";
        public static string CurrentPageIndex = "CurrentPageIndex";
        public static string FromDateExpression = "FromDateExpression";
        public static string ToDateExpression = "ToDateExpression";
        public static string SortedModule = "SortedModule";
        public static string WildCardExpression = "WildCardExpression";
        public static string UseManageStateCookies = "UseManageStateCookies";
        public static string CurrentSelectBudgetItemID = "CurrentSelectBudgetItemID";
        public static string mytab = "mytab";

        public static string UGITAPass = "ugitapass";
        public static string ShowAdvanceFilter = "AdvanceFilter";
        public static string _GridPageSize = "_GridPageSize";

        //ModuleConstraints
        public static string NewTask = "NewTask";
        public static string NewConstraint = "NewConstraint";
        public static string NewRule = "NewRule";
        public static string NewDocumentReview = "NewDocumentReview";
        public static string Completed = "Completed";
        public static string NotStarted = "Not Started";
        public static string InProgress = "In Progress";
        public static string Waiting = "Waiting";
        public static string Pending = "Pending";
        public const string ExitCriteria = "ExitCriteria";   //this is used as modulename to save task in cache
        public const string QuestionInputObj = "QuestionInputObj";
        public const string ReportScheduleDict = "ReportScheduleDict";
        public const string MailServerName = "outlook.office365.com";
        public const string ApplicationApprover = "ApplicationApprover";

        // Special Service categories used for Survey & Service Agent
        public static string ModuleService = "services";
        public static string ModuleFeedback = "~ModuleFeedback~";
        public static string ModuleFeedbackNoTilt = "ModuleFeedback";
        public static string ModuleAgent = "~ModuleAgent~";
        public static string ModuleField = "~ModuleField~";

        public static string DefaultMenuFontSize = "9";
        public static int DefaultSubMenuHeight = 50;
        /// <summary>
        /// Service question type. string in small case
        /// </summary>
        /// 
        //Chart BG Color
        public static string ChartBGColor = "#E7EAFE";
        public const string DxReport = "DxReport";
        public static class ServiceQuestionType
        {
            public const string SINGLECHOICE = "singlechoice";
            public const string MULTICHOICE = "multichoice";
            public const string CHECKBOX = "checkbox";
            public const string TEXTBOX = "textbox";
            public const string USERFIELD = "userfield";
            public const string DATETIME = "datetime";
            public const string REQUESTTYPE = "requesttype";
            public const string LOOKUP = "lookup";
            public const string Number = "number";
            public const string Rating = "rating";
            public const string ApplicationAccess = "applicationaccessrequest";
            public const string RemoveAccess = "removeuseraccess";
            public const string Assets = "assets lookup";
            public const string Resources = "resources";
            //  public const string ApplicationAccessRequest = "application access request";
        }
        public static string Kb = "KB";
        public static string mb = "MB";
        public static string gb = "GB";
        public static string bytes = "Bytes";

        public static string Folder = "Folder";
        public static string File = "File";
        public static string CheckInImage = "~/Content/images/uGovernIT/DocumentLibraryManagement/checkoutoverlay.gif";
        public static string True = "True";
        public static string DeleteFileTitle = "File Deletion Request";
        public static string DeleteFolderTitle = "Folder Deletion Request";

        // Message board contants

        public const string MessageBoardHideIfEmpty = "MessageBoardHideIfEmpty";

        // Document status
        public static string DocumentInitialStatus = "Draft";
        public static string DocumentProgressStatus = "Pending Approval";
        public static string DocumentCompletedStatus = "Approved";

        public static double DefaultRedLevel = 80;
        public static double DefaultYellowLevel = 60;

        public static string columnSeprator = ";#";
        public static string rowSeprator = "##";

        public static string mailSubjectToReview = "Document Pending Review";
        public static string mailBodyToReview = "A new document is pending your review and approval.";

        public static string SearchText = "SearchText";
        public static string SelectedTab = "SelectedTab";
        public static string SelectedFolder = "SelectedFolder";
        public static string SelectedPortal = "SelectedPortal";
        public static string PageIndex = "PageIndex";

        public static string DownLoadDocumentID = "DownLoadDocumentID";

        public static string AllPortals = "All Portals";
        public static string NA = "-N/A-";

        public static class WorkflowType
        {
            public static string Sequential = "Sequential";
            public static string Concurrent = "Concurrent";
        }
        public static class SLAConstants
        {
            public const string Minutes = "Minutes";
            public const string Hours = "Hours";
            public const string Days = "Days";
            public const int MinutesHourConversionFactor = 60;
        }

        public enum NavigationTypes
        {
            Navigate, Popup, NewWindow
        }
        public static class MessageTypeValues
        {
            public const string Ok = "Ok";
            public const string Information = "Information";
            public const string Warning = "Warning";
            public const string Critical = "Critical";
            public const string Reminder = "Reminder";
            public const string None = "None";
        }

        public static class TaskType
        {
            public const string Milestone = "Milestone";
            public const string Deliverable = "Deliverable";
            public const string Receivable = "Receivable";
            public const string Ticket = "Ticket";
            public const string Task = "Task";
        }

        public const string PMMIssue = "PMMIssue";
        public const string MalformedURLMsg = "Malformed URL";
        public static string Red = "red";
        public static string Yellow = "yellow";
        public static string Green = "green";
        public static string LightGreen = "lightgreen";
        public static string Gray = "gray";
        public static string Orange = "orange";
        public static string HomePage = "/Default.aspx";
        public static string HomePagePath = "/Pages/Home";

        // MyModuleColumns categories
        public static string MyDashboardTicket = "MyDashboard";
        public static string MyHomeTicketTab = "MyHomeTab";
        public static string MyTaskTab = "MyTaskTab";
        public static string MyProjectTab = "MyProjectTab";

        public static string SitePages = "Site Pages";
        public static string Pages = "Pages";
        public const string VendorIssue = "VendorIssue";
        public const string VendorRisks = "VendorRisks";
        public const string Admin = "Admin";
        public const string NewAdminUI = "NewAdminUI";
        public const string NewLoginWizard = "NewLoginWizard";

        //ResourceTab
        public const string ResourceTab = "ResourceTab";

        //ResouceCardView
        public const string ResourceCardView = "ResourceCardView ";
        public const string PMMWorkItemDropdown = "PMMWorkItemDropdown";
       
        public const string DivisionGLCode = "DivisionGLCode";
        public const string StudioTitle = "StudioTitle";
        public const string CRMSummary = "CRMSummary";
        public const string CRMAllocationCount = "CRMAllocationCount";

        public class PasswordChangeMessage
        {
            public static string FeatureNotEnableMessage = "This feature is not configured, please contact your administrator.";
            public static string PasswordNotMatch = "The passwords that you entered did not match.";
            public static string PasswordIncorrect = "The user name or password is incorrect.";
            public static string LogonFailure = "Logon failure: unknown user name or bad password.";
            public static string UnauthorizedAccess = "Access is denied";
            public static string PasswordPolicy = "The password does not meet the password policy requirements - check the minimum password length, password complexity and password history requirements.";
        }

        public class UserAccountMessage
        {
            public static string InactiveUser = "This user is inactive.";
            public static string InactiveAccount = "This account is inactive.";
            public static string IsTenantDeleted = "This account doesn't exist ";
            public static string IsAccountIdExist = "Invalid account Id";
            public static string IsUseNameExist = "Invalid user name";
        }

        public static int DefaultPasswordExpirationPeriod = 90;
        public const string NewUserNameFormat = "[$FirstName$][$LastName$]";

        public static string BlackTheme = "Black";
        public static string BusinessInitiatives = "BusinessInitiatives";
        public static string MyAssets = "MyAssets";
        public static string MyDashboardIssues = "MyDashboardIssues";
        public static string SVCTask = "SVCTask";
        public static string TSKTask = "TSKTask";
        public static string NPRResource = "NPRResource";
        public static string NPRBudget = "NPRBudget";
        public static string PMMActuals = "PMMActuals";
        public static string Union = "UNION";

        public static class SettingMenuType
        {
            public static string SignOut = "SignOut";
            public static string Admin = "Admin";
            public static string ChangeLook = "ChangeLook";
        }
        public static class ConfigVariableType
        {
            public const string Bool = "Bool";
            public const string Text = "Text";
            public const string Password = "Password";
            public const string Attachments = "Attachments";
            public const string User = "User";
            public const string Date = "Date";
        }
        public static class SLACategory
        {
            public static string Resolution = "Resolution";
            public static string Close = "Close";
            public static string RequestorContact = "Requestor Contact";
            public static string Assignment = "Assignment";
            public static string Other = "Other";
        }
        public static class EmailStatus
        {
            public const string InProgress = "InProgress";
            public const string Delivered = "Delivered";
            public const string Failed = "Failed";
        }

        public static class PageTitle
        {
            public const string Login = "Login";
            public const string Admin = "Admin";
            public const string Home = "Home";
            public const string Rmm = "RMM";
            public const string SuperAdmin = "SuperAdmin";
            public const string HLP = "Help Cards";

            public const string LEM = "Lead";
            public const string COM = "Company";
            public const string CNS = "CRMServices";
            public const string CON = "Contacts";
            public const string CPR = "CRMProject";
            public const string OPM = "Opportunity";
        }

        public static class ServiceViewType
        {
            public const string ListView = "ListView";
            public const string ButtonView = "ButtonView";
            public const string DropdownView = "DropdownView";
            public const string IconView = "IconView";
        }
        //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
        public static class TicketEventType
        {
            public const string Hold = "Hold";
            public const string HoldExpired = "Hold Expired";
            public const string HoldRemoved = "Hold Removed";
            public const string Created = "Created";
            public const string Assigned = "Assigned";
            public const string Resolved = "Resolved";
            public const string Closed = "Closed";
            public const string OwnerChange = "Owner Change";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
        }
        //

        public static class ColumnType
        {
            public const string NoteField = "NoteField";
            public const string Currency = "Currency";
            public const string Percentage = "Percentage";
            public const string Phone = "Phone";
            public const string User = "User";
            public const string Group = "Group";
            public const string UserGroup = "UserGroup";
            public const string Date = "Date";
            public const string DateTime = "DateTime";
        }

        public static class Cookie
        {
            public const string GanttViewExpandedUsers = "GanttViewExpandedUsers";
            public const string GanttViewCollapsedUsers = "GanttViewCollapsedUsers";
            public const string IsGanttViewExpanded = "IsGanttViewExpanded";
        }
    }

    public static class CustomProperties
    {
        public static string SelfAssign = "selfassign";
        public static string QuickClose = "quickclose";
        public static string BaseLine = "baseline";
        public static string NewNotification = "newnotification";
        public static string UpdateNotification = "updatenotification";
        public static string ResolvedNotification = "resolvednotification";
        public static string OnResolveNotification = "onresolvenotification";
        public static string DisableEmailTicketLink = "disableemailticketlink";
        public static string DisplayForClosed = "displayforclosed";
        public static string FieldType = "fieldtype";
        public static string Override = "override";
        public static string ManagerOf = "managerof";
        public static string CheckAssigneeToAllTask = "checkassigneetoalltask";
        public static string CloseTicketOnHoldExpiration = "CloseTicketOnHoldExpiration";

        ///added by maruf for schedule action escalation
        public static string EscalationRuleId = "escalationruleid";
        public static string SLARuleId = "slaruleid";
        public static string RequestTypeId = "requesttypeid";
        ///Schedule Action using in module stages.
        public static string ScheduledAutoStage = "scheduledautostage";
        public static string ScheduledTriggerFieldName = "scheduledtriggerfieldname";
        public static string ScheduledTriggerStageid = "scheduledtriggerstageid";
        public static string ScheduledRepeatInterval = "schedulerepeatinterval";
        public static string QueryParameters = "queryparameteres";
        public static string CustomIconReject = "customiconreject";
        public static string CustomIconApprove = "customiconapprove";
        public static string CustomIconReturn = "customiconreturn";
        public static string DoNotWaitForActionUser = "donotwaitforactionuser";
        public static string AutoApprove = "autoapprove";
        public static string UseforGlobalDateFilter = "useforglobaldatefilter";
        public static string ReadyToImport = "readytoimport";
        public static string PMOReview = "pmoreview";
        public static string ITSCReview = "itscreview";
        public static string ITGReview = "itgreview";
        public static string IsScrumTab = "isscrumtab";
        public static string MiniView = "miniview";
        public static string TicketAutoCloseAfter = "ticketautocloseafter";
        public static string ExtendedKey = "extendedkey";
        // Form Layout
        public static string TreeView = "treeview";
        public static string IsReadOnly = "isreadonly";
        public static string IsInFrame = "isinframe";

        public static string IsDocumentTab = "isdocumenttab";
        public static string IsDeliverableTab = "isdeliverabletab";
        public static string IsActionLogTab = "isactionlogtab";
        public static string IsSummaryTab = "issummarytab";

        public static string AllowEmailApproval = "AllowEmailApproval";
    }

    public static class ConfigConstants
    {
        public static string TenantCountCritical = "TenantCountCritical";
        public static string TenantCountHigh = "TenantCountHigh";
        public static string TicketCountCritical = "TicketCountCritical";
        public static string TicketCountHigh = "TicketCountHigh";
        public static string ServiceCountCritical = "ServiceCountCritical";
        public static string ServiceCountHigh = "ServiceCountHigh";
        public static string LogCleanDuration = "LogCleanDuration";
        
        public static string RegistrationMessage = "RegistrationMessage";
        public static string TenantConfirmationMessage = "TenantConfirmationMessage";
        public static string VerificationMail = "VerificationMail";
        public static string CredentialsMail = "CredentialsMail";
        public static string RefreshAllocationForModule = "RefreshAllocationForModule";
        public static string ResourceAllocationFontColor = "ResourceAllocationFontColor";
        public static string ResourceAllocationColor = "ResourceAllocationColor";
        public static string ResourceAllocationColorPalete = "ResourceAllocationColorPalete";
        public static string HideBenchCost = "HideBenchCost";
        public static string HideAnalyticsValue = "HideAnalyticsValue";
        public static string AllowAllocationViewAll = "AllowAllocationViewAll";
        public static string AllowAllocationForSelf = "AllowAllocationForSelf";
        public static string DisableSVCNewSubTasks = "DisableSVCNewSubTasks";
        public static string NewTenantServiceTicket = "NewTenantServiceTicket";
        public static string IsEmailVerificationActivated = "IsEmailVerificationActivated";
        public static string ForwardMailAddress = "ForwardMailAddress";
        public static string DisableSVCNewSubTickets = "DisableSVCNewSubTickets";
        public static string EnableSVCTaskHold = "EnableSVCTaskHold";
        public static string EnableAPPAccessImport = "EnableAPPAccessImport";
        public static string IssueTypeIsMultiChoice = "IssueTypeIsMultiChoicev";
        public static string HideServiceQuestionGraphic = "HideServiceQuestionGraphic";
        public static string MirrorAccessFrom = "mirroraccessfrom";
        public static string InitiativeLevel1Name = "InitiativeLevel1Name";
        public static string InitiativeLevel2Name = "InitiativeLevel2Name";
        public static string FBAMembershipProvider = "FBAMembershipProvider";
        public static string KeepExitCriteriaNotifications = "KeepExitCriteriaNotifications";
        public static string DisableAdminModules = "DisableAdminModules";
        //Added 10 march 2020
        public static string CommentsDefaultToPrivate = "CommentsDefaultToPrivate";

        //Added 24 march 2020
        public static string DRQCalendarMinStage = "DRQCalendarMinStage";

        //Enable Email To Ticket
        public static string EnableEmailToTicket = "EnableEmailToTicket";
        public static string EmailToTicketUsesOAuth2 = "EmailToTicketUsesOAuth2";
        public static string EmailToTicketCredentials = "EmailToTicketCredentials";
        public static string CopyTicketActualsToRMM = "CopyTicketActualsToRMM";
        public static string AutoCreateFBAUser = "AutoCreateFBAUser";
        public static string AutoCreateNewUser = "AutoCreateNewUser";
        public static string EnableRequestTypeSubCategory = "EnableRequestTypeSubCategory";
        public static string MembersGroup = "MembersGroup";
        public static string AdminGroup = "AdminGroupName"; // Super Admin group
        public static string DRQNotificationText = "DRQNotificationText";
        public static string DRQNotificationSubject = "DRQNotificationSubject";
        public static string NumServicesPerCategory = "NumServicesPerCategory";
        public static string EnablePMMBudgetImportExport = "EnablePMMBudgetImportExport";
        public static string PMMBudgetNeedsApproval = "PMMBudgetNeedsApproval";
        public static string PMMBudgetAllowEdit = "PMMBudgetAllowEdit";
        public static string PMMBudgetApprover = "PMMBudgetApprover";
        public static string NPRITGovApprover = "NPRITGovApprover";
        public static string NPRITSCApprover = "NPRITSCApprover";
        public static string CMTReminderSubject = "CMTReminderSubject";
        public static string AnalyticUrl = "AnalyticUrl";
        public static string AnalyticAuth = "AnalyticAuth";
        public static string FooterText = "FooterText";
        public static string FooterLogo = "FooterLogo";
        public static string FooterLogoLink = "FooterLogoLink";
        public static string WikiCreators = "WikiCreators";
        public static string WikiOwners = "WikiOwners";
        public static string PMOGroup = "PMOGroup";
        public static string IncludeAllProjectMonitors = "IncludeAllProjectMonitors";

        public static string AutoRedirectInvalidURL = "AutoRedirectInvalidURL";
        public static string AppLicenseThresholdPct = "AppLicenseThresholdPct";
        public static string DownloadOnly = "DownloadOnly";
        public static string PageSize = "PageSize";
        public static string CompanyName = "CompanyName";
        public static string AccountName = "AccountName";
        public static string LogoImagePath = "LogoImagePath";
        public static string LogoLinkUrl = "LogoLinkUrl";
        public static string WikiSurveyScore = "WikiSurveyScore";
        public static string EnableEscalations = "EnableEscalations";
        public static string ShowBudgetImport = "ShowBudgetImport";
        public static string AutoADSyncEnabled = "AutoADSyncEnabled";
        public static string SendEmail = "SendEmail";
        public static string ReportLogo = "ReportLogo";
        public static string HeaderLogo = "HeaderLogo";
        public static string SignitureLogo = "SignitureLogo";
        public static string EnableCommentNotification = "EnableCommentNotification";
        public static string TicketAdminGroup = "TicketAdminGroup";
        public static string EnableDivision = "EnableDivision";
        public static string OpenTicketsChart = "OpenTicketsChart";
        public static string OpenProjectsChart = "OpenProjectsChart";
        public static string ClosedTicketsChart = "ClosedTicketsChart";
        public static string ClosedProjectsChart = "ClosedProjectsChart";
        public static string ShowDepartmentDetail = "ShowDepartmentDetail";
        public static string OverallProjectScoreColor = "OverallProjectScoreColor";
        public static string RMMTypeActiveProjects = "RMMTypeActiveProjects";
        public static string RMMTypeProjectsPendingApproval = "RMMTypeProjectsPendingApproval";
        public static string RMMDashboardView = "RMMDashboardView";
        public static string ProjectCompletionUsingEstHours = "ProjectCompletionUsingEstHours";
        public static string ResourceAdminGroup = "ResourceAdminGroup";
        public static string ExperienceTagAdminGroup = "ExperienceTagAdminGroup";
        public static string ExperienceTagAllowMultiselect = "ExperienceTagAllowMultiselect";
        public static string ExperienceTagAllowGroupFilter = "ExperienceTagAllowGroupFilter";
        public static string EnableAppModuleRoles = "EnableAppModuleRoles";
        public static string TSRAppCategory = "TSRAppCategory";
        public static string ProjectSimilarityColorScale = "ProjectSimilarityColorScale";
        public static string ResourceSelectFilter = "ResourceSelectFilter";
        public static string DefaultLifeCycle = "DefaultLifeCycle";

        public static string CommentsNewestFirst = "CommentsNewestFirst";
        public static string AutoCreateRMMProjectAllocation = "AutoCreateRMMProjectAllocation";
        public static string LinkActualFromRMMActual = "LinkActualFromRMMActual";

        public static string Greeting = "Greeting";
        public static string Signature = "Signature";
        public static string CreateUserMailSubject = "CreateUserMailSubject";
        public static string CreateUserMailBody = "CreateUserMailBody";

        public static string ShowGroupTicketsTab = "ShowGroupTicketsTab";
        public static string EnableMobileRequest = "EnableMobileRequest";

        public static string ImportADUserGroup = "ImportADUserGroup";
        public static string ADReportEmail = "ADReportEmail";
        public static string ADUserCredential = "ADUserCredential";
        public static string EnableUGITLogging = "EnableUGITLogging";
        public static string DefaultAvatar = "DefaultAvatar";
        public static string UseInGlobalSearch = "UseInGlobalSearch";
        public static string AllowZeroHoursOnImport = "AllowZeroHoursOnImport";
        public static string AllowCommentDelete = "AllowCommentDelete";
        public static string ShowContractExpiryBefore = "ShowContractExpiryBefore";
        public static string RemoveContractExpiryAfter = "RemoveContractExpiryAfter";
        public static string WeekendDays = "WeekendDays";
        public static string DefaultSprintDuration = "DefaultSprintDuration";
        public static string ShowSPRibbon = "ShowSPRibbon";
        public static string AllowAddFromMyTasks = "AllowAddFromMyTasks";
        public static string OnlyShowFirstActionUser = "OnlyShowFirstActionUser";
        public static string PMMCreateGroup = "PMMCreateGroup";
        public static string DefaultUserRole = "DefaultUserRole";
        public static string DefaultGroup = "DefaultGroup";
        public static string SprintChartView = "SprintChartView";
        public static string PreventEstimatedHoursEdit = "PreventEstimatedHoursEdit";
        public static string NotifyPMMManagerOnStart = "NotifyPMMManagerOnStart";
        public static string uGovernITLogo = "uGovernITLogo";
        public static string uGovernITLogoLink = "uGovernITLogoLink";
        public static string ADAdminCredential = "ADAdminCredential";
        public static string PasswordPolicy = "PasswordPolicy";
        public static string DomainName = "DomainName";
        public static string AssetUniqueTag = "AssetUniqueTag";
        public static string showDefaultHelpPageButton = "showDefaultHelpPageButton";
        public static string DefaultPageUrl = "DefaultPageUrl";

        public static string EnableADPasswordReset = "EnableADPasswordReset";
        public static string ITDepartmentNames = "ITDepartmentNames";
        public static string DefaultLocation = "DefaultLocation";
        public static string FTPUserImportMapLocationToDesk = "FTPUserImportMapLocationToDesk";
        public static string EnableImportUsers = "EnableImportUsers";
        public static string DefaultPassword = "DefaultPassword";
        public static string VendorSLADashboard = "VendorSLADashboard";

        public static string RequestorContactSLAName = "RequestorContactSLAName";
        public static string RequestTypeSLAWarningTime = "RequestTypeSLAWarningTime";
        public static string RequestTypeSLAWarningBody = "RequestTypeSLAWarningBody";
        public static string RequestTypeSLAEscalationRoles = "RequestTypeSLAEscalationRoles";
        public static string RequestTypeSLAEscalationBody = "RequestTypeSLAEscalationBody";
        public static string RequestTypeSLAEscalationRecurrance = "RequestTypeSLAEscalationRecurrance";
        public static string RequestTypeSLAEscalationSubject = "RequestTypeSLAEscalationSubject";
        public static string RequestTypeSLAWarningSubject = "RequestTypeSLAWarningSubject";

        public static string AttachTaskCalendarEntry = "AttachTaskCalendarEntry";
        public static string EnableAdminImport = "EnableAdminImport";
        public static string VNDDocumentAuthors = "VNDDocumentAuthors";
        public static string VNDDocumentReaders = "VNDDocumentReaders";
        public static string VNDDocumentOwners = "VNDDocumentOwners";
        public static string FileUploadHandlers = "FileUploadHandlers";
        public static string EnableUserSyncTimerJob = "EnableUserSyncTimerJob";
        public static string PasswordExpirationPeriod = "PasswordExpirationPeriod";
        public static string ForcePasswordChangeForNewUsers = "ForcePasswordChangeForNewUsers";
        public static string VIPGroup = "VIPGroup";
        public static string PRPGroup = "PRP Group";
        public static string ScheduleActionArchiveExpiration = "ScheduleActionArchiveExpiration";
        //for asset import excel
        public static string AssetAdmin = "AssetAdmin";

        public static string UserAccountCreationFormatToken = "UserAccountCreationFormatToken";
        public static string NewUSer = "newuser";
        public static string ExistingUser = "existinguser";

        public static string AutoFillTicket = "AutoFillTicket";
        public static string ApplicationAccessPageSize = "ApplicationAccessPageSize";
        public static string AgeColorByTargetCompletion = "AgeColorByTargetCompletion";
        public static string NotifyRequestorOnTargetDateChange = "NotifyRequestorOnTargetDateChange";
        public static string NotifyTaskAssignee = "NotifyTaskAssignee";
        public static string MessageBoardAdmins = "MessageBoardAdmins";
        public static string AccessRequestMode = "accessrequestmode";
        public static string BlockedEmailToTicketSenders = "BlockedEmailToTicketSenders";
        public static string AutoCloseChildTickets = "AutoCloseChildTickets";
        public static string NotifyNewPRPORPOnly = "NotifyNewPRPORPOnly";
        public static string DepartmentLevel1Name = "DepartmentLevel1Name";
        public static string DepartmentLevel2Name = "DepartmentLevel2Name";
        public static string DepartmentLevel3Name = "DepartmentLevel3Name";
        public static string TaskReminderDefaultDays = "TaskReminderDefaultDays";
        public static string TaskReminderDefaultInterval = "TaskReminderDefaultInterval";

        public static string EnableTicketReopenByRequestor = "EnableTicketReopenByRequestor";
        public static string TaskReminderDefaultEnable = "TaskReminderDefaultEnable";

        public static string ProjectHealthAlertsEnable = "ProjectHealthAlertsEnable";

        public static string UgitTheme = "UgitTheme";
        public static string HideHomeMenuItem = "HideHomeMenuItem";
        public static string EmailToTicketIgnoreAttachments = "EmailToTicketIgnoreAttachments";
        public static string EmailToTicketAllowedAttachmentsExtn = "EmailToTicketAllowedAttachmentsExtn";
        public static string ModuleAgentButtonsEnable = "ModuleAgentButtonsEnable";
        public static string EnableMigrate = "EnableMigrate";
        public const string General = "General";
        public const string WorkdayStartTime = "WorkdayStartTime";
        public const string WorkdayEndTime = "WorkdayEndTime";
        public static string AssetIntegrationConfigurations = "AssetIntegrationConfigurations";
        public static string EnablePowerSearch = "EnablePowerSearch";
        public static string SmtpCredentials = "SmtpCredentials";
        public static string LinkViewLists = "LinkViewLists";
        public static string ReportImageAndBackGround = "ReportImageAndBackGround";
        public static string ReportImages = "ReportImages";

        public static string SetUserGroup = "SetUserGroup";
        public static string GeneralGroups = "GeneralGroups";
        public const string ProjectReportDefaults = "ProjectReportDefaults";

        //Account uniqueness with company,email,company and email
        public const string UniqueAccount = "UniqueAccount";
        public const string Email = "email";
        public const string UserName = "username";

        //for user
        public const string UniqueAccountForUser = "UniqueAccountForUser";

        //Trail tenant
        public const string NumberOfFreeUserAccounts = "NumberOfFreeUserAccounts";
        public const string NewTenantCreationPerDay = "NewTenantCreationPerDay";
        public const string ModelSite = "ModelSite";

        public const string NameOfWorkflowStepToShowTask = "NameOfWorkflowStepToShowTask";

        public static string DefaultPercentageAllocation = "DefaultPercentageAllocation";
        public static string AllowZeroResolutionHrs = "AllowZeroResolutionHrs";
        public static string EnableProjStdWorkItems = "EnableProjStdWorkItems";
        public static string AllowPTOEdit = "AllowPTOEdit";
        public static string KeepSVCTaskNotifications = "KeepSVCTaskNotifications";
        public static string UpdateSVCPRPORP = "UpdateSVCPRPORP";
        public static string EnableProjStdWorkItemsAfterDate = "EnableProjStdWorkItemsAfterDate";
        public static string TimesheetMode = "TimesheetMode";
        public static string SVCTaskCancelNotifyRequestor = "SVCTaskCancelNotifyRequestor";
        public static string HoldCommentAlwaysMandatory = "HoldCommentAlwaysMandatory";
        public static string widgetsHelpCardCategory = "widgetsHelpCardCategory";

        public static string WaitingOnMeTabName = "WaitingOnMeTabName";
        public static string NPRBudgetMandatory = "NPRBudgetMandatory";
        public static string TicketAgeExcludesHoldTime = "TicketAgeExcludesHoldTime";
        public static string Predecessors = "Predecessors";
        public static string IsTaskFollowers = "IsTaskFollowers";
        public static string ProcoreBaseUrl = "ProcoreBaseUrl";
        public static string ProcoreCompanyId = "ProcoreCompanyId";
        public static string ProcoreProjectId = "ProcoreProjectId";
        public static string ProcoreClientSecretId = "ProcoreClientSecretId";
        public static string ProcoreRefreshToken = "ProcoreRefreshToken";
        public static string ProcoreToken = "ProcoreToken";
        public static string EnableProjectMetricSync = "EnableProjectMetricSync";
        public static string CloseLeadWhileCreatingOpportunity = "CloseLeadWhileCreatingOpportunity";
        public static string EnableUpdateTicketsOnWorkflowChange = "EnableUpdateTicketsOnWorkflowChange";
        public static string uGovernITDateFormat = "uGovernITDateFormat";
        public static string AdfsDomainName = "AdfsDomainName";
        public static string InactivateRelatedContactsOfCompany = "InactivateRelatedContactsOfCompany";
        public static string ResourceHourlyRate = "ResourceHourlyRate";
        public static string OpenTicketsShowOnHold = "OpenTicketsShowOnHold";
        public static string MyQueueShowOnHold = "MyQueueShowOnHold";
        public static string ResolvedTicketsShowOnHold = "ResolvedTicketsShowOnHold";
        public static string MyTicketsShowOnHold = "MyTicketsShowOnHold";
        public static string OnePagerMilestonesOnly = "OnePagerMilestonesOnly";
        public static string TrackProjectStageHistory = "TrackProjectStageHistory";
        public static string ChangeAllocationDatesAutomatically = "ChangeAllocationDatesAutomatically";
        public static string AllocationOverbookingFactor = "AllocationOverbookingFactor";
        public static string EnableLockUnlockAllocation = "EnableLockUnlockAllocation";
        public static string HideAllocationTemplate = "HideAllocationTemplate";
        public static string EnableBudgetCategoryType = "EnableBudgetCategoryType";
        public static string AccessTokenExpiration = "AccessTokenExpiration";
        public static string APIAllowPartialUpdate = "APIAllowPartialUpdate";
        public static string UpdateResourceAllocation = "UpdateResourceAllocation";
        public static string AllocationImportEnabled = "AllocationImportEnabled";
        public static string EnableVNDContractExpireEmail = "EnableVNDContractExpireEmail";
        public static string ReminderSubject = "ReminderSubject";
        public static string ContractExpirationReminderBody = "ContractExpirationReminderBody";
        public static string DisableRequestListBanding = "DisableRequestListBanding";
        public static string ShowTotalCapicityFTE = "ShowTotalCapicityFTE";
        public static string DisablePlannedAllocation = "DisablePlannedAllocation";
        public static string SendUserCredentialMail = "SendUserCredentialMail";
        public static string EnableRMMAssignment = "EnableRMMAssignment";
        public static string EnableGanttProfilePic = "EnableGanttProfilePic";
        public static string ShowTotalAllocationsInSearch = "ShowTotalAllocationsInSearch";
        public static string EnforcePhaseConstraints = "EnforcePhaseConstraints";
        public static string PhaseSummaryView = "PhaseSummaryView";
        public static string NPRTaskdate = "NPRTaskdate";
        public static string AllowAllUserEmails = "AllowAllUserEmails";
        public static string DisableCustomFilterTab = "DisableCustomFilterTab";
        public static string EnableCustomizeColumns = "EnableCustomizeColumns";
        public static string ShowERPJobID = "ShowERPJobID";
        public static string ERPJobIDName = "ERPJobIDName";
        public static string ERPJobIDNCName = "ERPJobIDNCName";
        public static string AdminGanttFormat = "AdminGanttFormat";
        public static string ShowBenchProjectDepartment = "ShowBenchProjectDepartment";
        public static string PTORequestType = "PTORequestType";
        public static string ERPIDLABEL = "ERPIDLABEL";
        public static string ERPJobIDNoofCharacters = "ERPJobIDNoofCharacters";
        public static string EnableAssetSyncTimerJob = "EnableAssetSyncTimerJob";
        public static string HidePTOonGantt = "HidePTOonGantt";
        public static string HidePTOonMgrView = "HidePTOonMgrView";
        public static string CloseoutPeriod = "CloseoutPeriod";
        public static string MgrViewAllocDays = "MgrViewAllocDays";
        public static string AllowEditInGantt = "AllowEditInGantt";
        public static string IncludeNonProjectTime = "IncludeNonProjectTime";
        public static string IncludeSoftAllocations = "IncludeSoftAllocations";
        public static string SupportEmail = "SupportEmail";
        public static string EnableStudioDivisionHierarchy = "EnableStudioDivisionHierarchy";
        public static string EnableSimilarityFunction = "EnableSimilarityFunction";
        public static string DivisionLabel = "DivisionLabel";
        public static string StudioLabel = "StudioLabel";
        public static string CloseoutDueDays = "CloseoutDueDays";
        public static string ShortNameCharacters = "ShortNameCharacters";
        public static string AllowRecreatingExperienceTags = "AllowRecreatingExperienceTags";
        public static string DefaultRMMDisplayMode = "DefaultRMMDisplayMode";
        public static string DefaultAllocationPercentage = "DefaultAllocationPercentage";
        public static string DefaultNormalizedScore = "DefaultNormalizedScore";
        public static string DefaultComparisonMetricType = "DefaultComparisonMetricType";
        public static string ProjectComparisonMatrixColor = "ProjectComparisonMatrixColor";
        public static string MinimumSimilarityScore = "MinimumSimilarityScore";
        public static string HideServiceAccountUsers = "HideServiceAccountUsers";
        public static string DefaultView = "DefaultView";
    }

    public static class uGITFormatConstants
    {
        public const string DateFormat = "{0:MMM-dd-yyyy}";
        public const string RecordNumberFormat = "{0:#}";
        public const string PrecentFormat = "{0:0.##%}";
    }


    public static class ReportScheduleConstant
    {
        //using for pmm component 
        public const string ShowAccomplishment = "ShowAccomplishment";
        public const string ShowPlan = "ShowPlan";
        public const string ShowRisks = "ShowRisks";
        public const string ShowIssues = "ShowIssues";
        public const string ShowDecisionLog = "ShowDecisionLog";
        public const string ShowStatus = "ShowStatus";
        public const string ShowSummaryGanttChart = "ShowSummaryGanttChart";
        public const string ShowAllTask = "ShowAllTask";
        public const string ShowMilestone = "ShowMilestone";
        public const string ShowDeliverable = "ShowDeliverable";
        public const string ShowReceivable = "ShowReceivable";
        public const string CalculateExpected = "CalculateExpected";
        public const string ShowProjectDescription = "ShowProjectDescription";
        public const string ShowBudgetSummary = "ShowBudgetSummary";
        public const string ShowPlannedvsActualByCategory = "ShowPlannedvsActualByCategory";
        public const string ShowPlannedvsActualByBudgetItem = "ShowPlannedvsActualByBudgetItem";
        public const string ShowPlannedvsActualByMonth = "ShowPlannedvsActualByMonth";
        public const string ShowProjectRoles = "ShowProjectRoles";
        public const string ShowResourceAllocation = "ShowResourceAllocation";
        public const string ShowMonitorState = "ShowMonitorState";
        public const string ids = "ids";

        ///Using in pmm and tsk projects report 
        public const string ProjectType = "ProjectType";
        public const string Department = "Department";
        public const string FunctionAreas = "FunctionAreas";
        public const string TicketPriority = "TicketPriority";
        public const string Category = "Category";
        public const string SubCategory = "SubCategory";
        public const string RequestType = "RequestType";
        public const string ProjectInitiative = "ProjectInitiative";
        public const string ProjectClass = "ProjectClass";
        public const string Projects = "Projects";
        public const string MonitorState = "MonitorState";
        public const string Accomplishment = "Accomplishment";
        public const string ImmediatePlans = "ImmediatePlans";
        public const string Issues = "Issues";
        public const string Tasks = "Tasks";
        public const string SummaryTasks = "SummaryTasks";
        public const string PlannedvsActualsbyCategory = "PlannedvsActualsbyCategory";
        public const string PlannedMonthlyBudget = "PlannedMonthlyBudget";
        public const string ActualMonthlyBudget = "ActualMonthlyBudget";
        public const string BudgetAllocation = "BudgetAllocation";
        public const string Module = "Module";
        public const string TicketStatus = "Status";

        ///Using in Schedule edit form
        public const string Report = "Report";
        public const string AttachmentFormat = "AttachmentFormat";
        public const string AlertCondition = "AlertCondition";
        public const string Excel = "Excel";
        public const string Pdf = "Pdf";
        public const string CustomRecurrence = "CustomRecurrence";
        public const string BusinessHours = "BusinessHours";
        ///ticket summary constant.
        public const string SortByModule = "SortByModule";
        public const string SortBy = "SortBy";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string CurrentUser = "CurrentUser";

        ///Using in Schedule edit form for query
        public const string Where = "Where";
        public const string GroupByCategory = "GroupByCategory";
        public const string IncludeORP = "IncludeORP";
        public const string IncludeCounts = "IncludeCounts";
        public const string ITManagers = "ITManagers";
        public const string DateRange = "DateRange";
        public const string NonPeakHourWindow = "NonPeakHourWindow";
        public const string WorkingWindowStartTime = "WorkingWindowStartTime";
        public const string WorkingWindowEndTime = "WorkingWindowEndTime";
        public const string Type = "Type";
        public const string Survey = "Survey";
        //SpDelta 70
        public static string FrequencyType = "FrequencyType";
        public static string FrequencyUnit = "FrequencyUnit";
        public static string MonthFrequencyType = "MonthFrequencyType";
        public static string EveryOfMonth = "EveryOfMonth";
        public static string DayOfMonth = "DayOfMonth";
        public static string Frequency = "Frequency";
        public static string EveryXMonths = "EveryXMonths";
        //
    }

    public static class RecurringTicketScheduleConstant
    {
        //For ScheduleActions New/Edit for Module -> Templates
        public const string TemplateId = "TemplateId";
    }


    public static class RegularExpressionConstant
    {
        public static string ConditionExpress = @"\[([A-Z])\w+](>|<|>=|<=|==)[0-9]+";
        public static string OperatorTypeExpress = @"[<>=]+";
        public static string ValueExpress = @"[0-9]+";

    }

    public static class CAMLQuery
    {
        public static string Where = "Where";
        public static string And = "And";
        public static string Or = "Or";
        public static string Eq = "Eq";
        public static string Neq = "Neq";
        public static string Contains = "Contains";
        public static string FieldRef = "FieldRef";
        public static string Value = "Value";
        public static string Name = "Name";
        public static string LookupId = "LookupId";
        public static string TRUE = "TRUE";
        public static string FALSE = "FALSE";
        public static string Choice = "Choice";
        public static string Lookup = "Lookup";
        public static string Boolean = "Boolean";
        public static string Type = "Type";
        public static string LookupMulti = "LookupMulti";
        public static string User = "User";
        public static string CurrentUserGroups = "CurrentUserGroups";
        public static string Text = "Text";
        public static string Gt = "Gt";
        public static string Lt = "Lt";
        public static string Geq = "Geq";
        public static string Leq = "Leq";
        public static string IsNull = "IsNull";
        public static string Membership = "Membership";
        public static string OrderBy = "OrderBy";
        public static string Ascending = "Ascending";
    }

    public static class ModuleNames
    {
        public const string ACR = "ACR";
        public const string APP = "APP";
        public const string DRQ = "DRQ";
        public const string BTS = "BTS";
        public const string CMDB = "CMDB";
        public const string CMT = "CMT";
        public const string CRM = "CRM";
        public const string EDM = "EDM";
        public const string INC = "INC";
        public const string INV = "INV";
        public const string ITG = "ITG";
        public const string NPR = "NPR";
        public const string PLC = "PLC";
        public const string PMM = "PMM";
        public const string PRS = "PRS";
        public const string RMM = "RMM";
        public const string SVC = "SVC";
        public const string TSK = "TSK";
        public const string TSR = "TSR";
        public const string VFM = "VFM";
        public const string VND = "VND";
        public const string VPM = "VPM";
        public const string VSL = "VSL";
        public const string VSW = "VSW";
        public const string WIK = "WIK";
        public const string VCC = "VCC";
        public const string RCA = "RCA";
        public const string CNS = "CNS";
        public const string OPM = "OPM";
        public const string LEM = "LEM";
        public const string CPR = "CPR";
        public const string CON = "CON";
        public const string COM = "COM";
        public const string WIKI = "WIKI";
    }

    public static class MenuTitleDisplayType
    {
        public const string TitleOnly = "titleonly";
        public const string IconOnly = "icononly";
        public const string Both = "both";
    }
    public static class MenuAllignmentType
    {
        public const string TopLeft = "Top Left";
        public const string TopCenter = "Top Center";
        public const string TopRight = "Top Right";
        public const string BottomLeft = "Bottom Left";
        public const string BottomCenter = "Bottom Center";
        public const string BottomRight = "Bottom Right";
        public const string Center = "Center";
    }

    public static class FilterTab
    {
        public const string unassigned = "unassigned";
        public const string waitingonme = "waitingonme";
        public const string myopentickets = "myopentickets";
        public const string mygrouptickets = "mygrouptickets";
        public const string departmentticket = "departmentticket";
        public const string allopentickets = "allopentickets";
        public const string allresolvedtickets = "allresolvedtickets";
        public const string alltickets = "alltickets";
        public const string allclosedtickets = "allclosedtickets";
        public const string myclosedtickets = "myclosedtickets";
        public const string myproject = "myproject";
        //Added by Mudassir 17 march 2020  SPDelta 15(Support for separate "On Hold" tab in ticket lists)
        public const string OnHold = "onhold";
        //
        public const string allholdtickets = "allholdtickets";
    }
    public class CacheKey
    {
        public const string USERSTATISTICS = " USERSTATISTICS";
    }

    public enum PanelViewType
    {
        Default, Bars, Circular
    }

    public static class JobType
    {
        public const string Billable = "Billable";
        public const string Overhead = "Overhead";
    }
}
