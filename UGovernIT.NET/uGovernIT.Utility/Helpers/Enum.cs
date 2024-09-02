using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    public enum ProjectFormatUnit
    {
        pjMinutes = 3,
        pjElapsedMinutes = 4,
        pjHours = 5,
        pjElapsedHours = 6,
        pjDays = 7,
        pjElapsedDays = 8,
        pjWeeks = 9,
        pjElapsedWeeks = 10,
        pjMonths = 11,
        pjElapsedMonths = 12,
        pjMinutesEstimated = 35,
        pjElapsedMinutesEstimated = 36,
        pjHoursEstimated = 37,
        pjElapsedHoursEstimated = 38,
        pjDaysEstimated = 39,
        pjElapsedDaysEstimated = 40,
        pjWeeksEstimated = 41,
        pjElapsedWeeksEstimated = 42,
        pjMonthsEstimated = 43,
        pjElapsedMonthsEstimated = 44,
        PjPercentage = 19

    }

    public enum ProjectTaskLinkType
    {
        pjFinishToFinish = 0,
        pjFinishToStart = 1,
        pjStartToFinish = 2,
        pjStartToStart = 3
    }

    public enum UGITLogSeverity
    {
        Emergency,
        Alert,
        Critical,
        Error,
        Warning,
        Notice,
        Information,
        Debug
    }

    public enum UGITLogCategory
    {
        UserProfile,
        Import,
        Admin,
        Module
    }

    public enum ErrorType
    {
        Mandatory,
        Error,
        Review
    }

    public struct ServiceSubTaskType
    {
        public const string Task = "task";
        public const string AccessTask = "Access Task";
        public const string AccountTask = "account task";
        public const string ApplicationAccessRequest = "applicationaccessrequest";
    }
    public struct ApprovalType
    {
        public const string None = "none";
        public const string All = "all";
        public const string AnyOne = "any one";
    }
    public struct TaskApprovalStatus
    {
        public const string None = "none";
        public const string NotStarted = "notstarted";
        public const string Approved = "approved";
        public const string Pending = "pending";
        public const string Rejected = "rejected";

    }

    public enum DashboardType
    {
        Panel,
        Chart,
        Query,
        Link,
        Analytic,
        Control
    }

    public enum DashboardLayoutType
    {
        Autosize,
        FixedSize,
    }

    public enum ReportingPeriod
    {
        All_Requests = 0,
        Current_Month = 1,
        Previous_Month = 2,
        Current_Quater = 3,
        Year_To_Date = 4,
        Custom = 5
    }
    public enum StageType
    {
        None,
        Initiated,
        Approved,
        Assigned,
        Resolved,
        Tested,
        Closed,
        
    }
    public enum DrillDownType
    {
        Auto, Year, Month, Days
    }

    public enum TicketStatus
    {
        All = 0,
        Open = 1,
        Closed = 2,
        WaitingOnMe = 3,
        Unassigned = 4,
        Approved = 5,
        Department = 6,
        OnHold = 7,
        MyProject = 8
    }

    public enum TicketPriority
    {
        All = 0,
        High = 1,
        Medium = 2,
        Low = 3,
    }

    public enum ZoomLevel
    {
        Weekly = 0,
        Monthly = 1,
        Quarterly = 2,
        HalfYearly = 3,
        Yearly = 4
    }

    public enum ControlTypes
    {
        TextBox, DropDown, RadioButton
    }

    /// <summary>
    /// only for Impact Severity and Priority
    /// </summary>
    public enum ViewMode
    {
        Impact,
        Severity,
        Priority
    }
    public enum BudgetType
    {
        NPR,
        PMM,
        ITG,
        PMMActuals,
        PMMBudgetBaseline
    }

    public enum ColumnStyle
    {
        MonthNumber, MonthYear
    }

    public enum ScheduleActionType
    {
        Alert,
        AutoStageMove,
        Email,
        EscalationEmail,
        Reminder,

        Query,
        Report,
        UnHoldTicket,
        UnHoldTask,
        RefreshCache,
        ScheduledTicket
    }

    public enum AgentJobStatus
    {
        Success,
        Fail,
        NoAction
    }

    public enum UGITTaskProposalStatus
    {
        Not_Proposed,
        Pending_Date,
        Pending_AssignTo,
        Approved
    }


    public enum UGITControlType
    {
        ProjectType,
        FunctionArea,
        Projects,
        Priority,
        Progress
    }

    public enum TypeOfReport
    {
        TicketSummary,
        ProjectReport,
        TSKProjectReport,
        TaskSummary,
        GanttView,
        SummaryByTechnician,
        WeeklyTeamReport,
        NonPeakHoursPerformance,
        SurveyFeedbackReport,
        OnePagerReport,
        ProjectStatusReport
    }
    public enum FormMode
    {
        Edit, New
    }

    public enum BrowserVersion
    {
        Desktop, Mobile
    }

    public enum TicketOwnerBinding
    {
        Auto,
        Disabled,
        ClientSide
    }

    public enum DocumentUpdateResponse
    {
        NoError,
        FolderSizeExceed,
        PortalSizeExceed,
        NoOfDocumentExceed,
        InvalidType,
        TypeIsNotAllowed,
        FileAlreadyExist,
        UnknowError,
        UnAuthorizedAuthor,
        UnAuthorizedReviewer
    }
    public enum ReportingFrequencyType
    {
        Fixed_Frequency,
        By_Day_of_Month
    }
    public enum MonthReportingFrequencyType
    {
        Day_of_Month,
        Business_Day_of_Month
    }

    public enum ReminderType
    {
        After,
        Before
    }

    public enum CustomFilterTab
    {
        UnAssigned,
        WaitingOnMe,
        MyOpenTickets,
        MyGroupTickets,
        MyDepartmentTickets,
        ResolvedTickets,
        AllTickets,
        OpenTickets,
        CloseTickets,
        MyCloseTickets,
        OnHold
    }

    public enum TicketActionType
    {
        //Added by mudassir 23 march 2020
        None, Created, Approved, Returned, Rejected,OnHold, Elevated
            //
    }

    public enum DepartmentLevel
    {
        Company, Division, Department
    }

    public enum ModuleType
    {
        All,
        SMS,
        Governance,
        Project
    }

    public enum ResourceAllocationType
    {
        Allocation,
        Estimated,
        Planned
    }


    public class Enums
    {
        public enum TableDisplayTypes { General, TableLayout, Grouped };
        public enum Approver
        {
            ITOwner,
            AccessAdmin
        }
        public enum UserType
        {
            NewADUser,
            NewFBAUser
        }
        public enum BudgetStatus
        {
            PendingApproval,
            Approve,
            Reject
        }
        public enum HistoryActions
        {
            Create = 1,
            Modify = 2,
            Delete = 3,
            FileUpload = 4,
            FileDownload = 5,
            Approve = 6,
            Reject = 7,
            ReviewStart = 8,
            ReviewCancel = 9,
            View = 10,
            Preapprove = 11,
            CheckIn = 12,
            CheckOut = 13,
            DiscardCheckOut = 14,
            LinkCreated = 15,
            Purged = 16
        }
        
        public enum Agent
        {
            ResetPassword = 1,
            AutoAssign = 2,
            Service =3
        }

        public enum SubscriptionType
        {
            SimpleUser = 1,
            AdvanceUser = 2
        }

    }
    public enum ControlMode
    {
        Invalid = 0,
        Display = 1,
        Edit = 2,
        New = 3
    }

    public enum FieldType
    {
        None,
        UserField,
        Lookup,
        Choices,
        Date,
        Percentage,
        Currency,
        Attachments,
        GroupField,
        MultiUser,
        DateTime
    }

    public enum BatchMode
    {
        None, Edit, Create
    }

    public enum FieldDesignMode
    {
        Normal, WithEdit, WithCheckbox, WithEditAndCheckbox
    }

    public enum RatingDisplayMode
    {
        RatingBar,
        Dropdown,
        RadioButtonsH,
        RadioButtonsV
    }

    public enum ShowMode
    {
        Both,
        ChartOnly,
        TableOnly
    }

    public enum ReviewType
    {
        Dislike,
        Like,
        Favorite,
        UnFavorite
    }

    public enum ReviewStatus
    {
        likeWiki,
        FavoriteWiki
    }


    public enum ResourceAvailabilityType
    {
        FullyAvailable,
        PartiallyAvailable,
        AllResource
    }

    public enum ResourceFilterType
    {
        Complexity,
        Count,
        Volume
    }


  

    public enum PMMMode
    {
        Scratch=1,
        PMM=2,
        MPP=3,
        NPR=4,
        Template=5
    }

    

    public enum UserTypes
    {
        ADUser = 0,
        FBAUser = 0
    }

    public enum Availability
    {
        FullyAvailable,
        NearToFullyAvailable,
        PartiallyAvailable,
        NotAvailable
    }

    public enum UpdateType
    {
        PastAndFuture,
        Future
    }

    public enum TimeFrame
    {
        Days,
        Weeks,
        Months
    }
    public enum GanttFormat
    {
        Days,
        Percentage
    }
}

