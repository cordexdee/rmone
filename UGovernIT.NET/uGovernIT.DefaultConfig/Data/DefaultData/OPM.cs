using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DefaultConfig;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class OPM : IModule
    {
        public static string userId = "1";
        public static string managerUserId = "1";
        public static string userName = ""; // Obtained from userId in UGovernITDefault.Initialize()

        public static string[] HideInTicketTemplate = { "Attachments", "AssetLookup", "Comment", "CreationDate", "Initiator", "RequestTypeCategory", "Status" };
        public static int currStageID = 0;
        public static bool loadRequestTypes = true;
        public static bool loadModuleStages = true;

        public static Hashtable moduleStartStages = new Hashtable();
        public static Hashtable moduleAssignedStages = new Hashtable();
        public static Hashtable moduleResolvedStages = new Hashtable();
        public static Hashtable moduleTestedStages = new Hashtable();
        public static Hashtable moduleClosedStages = new Hashtable();
        public static int nprMgrApprovalStageID;
        public static int nprPMOStageID;
        public static int nprITGovernanceStageID;
        public static int nprITSteeringeStageID;
        public static int nprApprovedStageID;
        public static int pmmStartStageID;
        public static int pmmClosedStageID;
        protected static bool enableSecurityApprovalStage = true;
        protected static bool enableManagerApprovalStage = true;
        protected static bool enableTestingStage = false;
        protected static bool enablePendingCloseStage = false;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Opportunity Management",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "Opportunity",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/Opportunity",
                    ModuleHoldMaxStage = 3,
                    Title = "Opportunity Management (OPM)",
                    ModuleDescription = "This module is used to manage opportunities for potential projects with prospective and current clients. It manages the entire life cycle from lead identification, to bid and proposal management.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/Opm",
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = false,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    KeepItemOpen = true,
                    ShowComment = true,
                    AllowEscalationFromList = true,
                    EnableRMMAllocation = true,
                    EnableLayout = true
                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "OPM";
            }
        }

        public List<ACRType> GetACRTypes()
        {
            List<ACRType> mList = new List<ACRType>();
            return mList;
        }

        public List<DRQRapidType> GetDRQRapidTypes()
        {
            List<DRQRapidType> mList = new List<DRQRapidType>();
            return mList;
        }

        public List<DRQSystemArea> GetDRQSystemAreas()
        {
            List<DRQSystemArea> mList = new List<DRQSystemArea>();
            return mList;
        }

        public List<ModuleFormTab> GetFormTabs()
        {

            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");
            //mList.Add(new ModuleFormTab() { TabName = "General", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General" });
            //mList.Add(new ModuleFormTab() { TabName = "PMO Assessment", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "PMO Assessment" });
            //mList.Add(new ModuleFormTab() { TabName = "Planning", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Planning" });
            //mList.Add(new ModuleFormTab() { TabName = "Opportunity Management", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Opportunity Management" });
            //mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            //mList.Add(new ModuleFormTab() { TabName = "Bids", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Bids" });
            //mList.Add(new ModuleFormTab() { TabName = "Estimates", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Estimates" });
            //mList.Add(new ModuleFormTab() { TabName = "Close Opportunity", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Close Opportunity" });
            //mList.Add(new ModuleFormTab() { TabName = "History", TabId = 9, TabSequence = 9, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            mList.Add(new ModuleFormTab() { TabName = "Summary", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Summary" });
            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Project Team", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Project Team" });
            mList.Add(new ModuleFormTab() { TabName = "External Team", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "External Team" });
            mList.Add(new ModuleFormTab() { TabName = "Documents", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Documents" });
            mList.Add(new ModuleFormTab() { TabName = "Proposal", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Proposal" });
            mList.Add(new ModuleFormTab() { TabName = "Tasks", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Tasks" });
            mList.Add(new ModuleFormTab() { TabName = "Checklist", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Checklist" });
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 9, TabSequence = 9, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 10, TabSequence = 10, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "Close Opportunity", TabId = 11, TabSequence = 11, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Close Opportunity" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 12, TabSequence = 12, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });
            mList.Add(new ModuleFormTab() { TabName = "Owner Contract", TabId = 13, TabSequence = 16, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Owner Contract" });

            return mList;

        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            return mList;
        }
        public List<ModuleSeverity> GetModuleSeverity()
        {

            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            return mList;
        }


        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            return mList;
        }
        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            return mList;
        }
        public List<LifeCycleStage> GetLifeCycleStage()
        {

            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 6;

            string closeStageID = (currStageID + numStages - 1).ToString();
            int StageStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Name = "Identify Opportunity",
                StageTitle = "Identify Opportunity",
                UserWorkflowStatus = "Create new Opportunity",
                UserPrompt = "<b>Please fill the form to open a new OPM ticket.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Initiated",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Created Opportunity",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Re-Submit",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Assign",
                StageTitle = "Assign",
                UserWorkflowStatus = "Done",
                UserPrompt = "<b>Marketing:</b> Please assign a staff person",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Assigned Opportunity",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Assigned Opportunity",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Advance",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Proposal Development",
                StageTitle = "Proposal Development",
                UserWorkflowStatus = "Proposal Development",
                UserPrompt = "<b>Marketing:</b> Please enter opportunity related details",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Develop Proposal",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Owner approved fix implementation & closed the ticket",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Advance",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Interview",
                StageTitle = "Interview",
                UserWorkflowStatus = "Interview",
                UserPrompt = "<b>Marketing:</b> Add interview notes as an attachment if needed.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Interview Set",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Marketing approved fix implementation & closed the ticket",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Advance",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "ACR",
                StageTitle = "ACR",
                UserWorkflowStatus = "Manage Proposal",
                UserPrompt = "<b>Marketing:</b> Please fill Awaiting Client Response.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Awaiting Client Response",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Marketing approved fix implementation & closed the ticket",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Advance",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Closed",
                StageTitle = "Closed",
                UserWorkflowStatus = "Opportunity is closed, but you can add comments or can be re-opened by Owner",
                UserPrompt = "Opportunity is Closed",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Closed",
                ActionUser = "Admin",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "Closed",
                RejectActionDescription = "",
                ReturnActionDescription = "Owner re-opened ticket",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageAllApprovalsRequired = false,
                StageWeight = 0,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });


            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;

            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "OpportunityType", FieldDisplayName = "Opportunity Type", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Title", FieldDisplayName = "Opportunity Name", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ProposalDeadline", FieldDisplayName = "E Copy Due Date", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = "Center" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "InterviewDate", FieldDisplayName = "Interview Date", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "OwnerUser", FieldDisplayName = "Assigned To", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Estimator", FieldDisplayName = "Estimator", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ProjectManager", FieldDisplayName = "Project Manager", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Superintendent", FieldDisplayName = "Superintendent", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CreationDate", FieldDisplayName = "Created On", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "miniview=True", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "RequestTypeLookup", FieldDisplayName = "Project Type", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "miniview=True", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMUrgency", FieldDisplayName = "Urgency", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "MarketSector", FieldDisplayName = "Market Sector", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Status", FieldDisplayName = "Stage", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CloseDate", FieldDisplayName = "Close Date", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "PreconStartDate", FieldDisplayName = "Precon Start Date", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMBusinessUnit", FieldDisplayName = "Business Unit", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ApproxContractValue", FieldDisplayName = "Contract Value", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "Currency", TextAlignment = "Right" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Reason", FieldDisplayName = "Reason", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMOpportunityStatus", FieldDisplayName = "Opportunity Status", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "EstimatedConstructionStart", FieldDisplayName = "Estimated Const. Start", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "EstimatedConstructionEnd", FieldDisplayName = "Estimated Const. End", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "TicketId", FieldDisplayName = "ID", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "miniview=True", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMCompanyLookup", FieldDisplayName = "Company", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "StreetAddress1", FieldDisplayName = "Address", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "City", FieldDisplayName = "City", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "AwardedorLossDate", FieldDisplayName = "Awarded/LossDate", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });

            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {

            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            return mList;

        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");

            int seqNum = 0;
            // Tab 0: General
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Opportunity Name", FieldDisplayName = "Opportunity Name", FieldName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Company", FieldDisplayName = "Company", FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Opportunity Status", FieldDisplayName = "Opportunity Status", FieldName = "CRMOpportunityStatus", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Signed NDA", FieldDisplayName = "Signed NDA", FieldName = "SignedNDA", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldDisplayName = "Assigned To", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", FieldName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Type", FieldDisplayName = "Project Type", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Studio", FieldDisplayName = "Studio", FieldName = "Studio", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Business Unit", FieldDisplayName = "Business Unit", FieldName = "CRMBusinessUnit", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Project Type", FieldDisplayName = "Secondary Project Type", FieldName = "AdditionalInfo", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            //mList.Add(new ModuleFormLayout() { Title = "Project Complexity", FieldDisplayName = "Project Complexity", FieldName = "CRMProjectComplexity", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Owner Contract Type", FieldDisplayName = "Owner Contract Type", FieldName = "OwnerContractType", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "BCCI Sector", FieldDisplayName = "BCCI Sector", FieldName = "SectorChoice", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Market Sector", FieldDisplayName = "Market Sector", FieldName = "MarketSector", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "CRM Urgency", FieldDisplayName = "CRM Urgency", FieldName = "CRMUrgency", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Opportunity Type", FieldDisplayName = "Opportunity Type", FieldName = "OpportunityType", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Chance of Success", FieldDisplayName = "Chance of Success", FieldName = "ChanceOfSuccess", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldDisplayName = "Project Info", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Street", FieldDisplayName = "Street", FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldDisplayName = "Contract Value", FieldName = "ApproxContractValue", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction Start", FieldDisplayName = "Est Construction Start", FieldName = "EstimatedConstructionStart", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", FieldName = "City", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Net Rentable Sq Ft", FieldDisplayName = "Net Rentable Sq Ft", FieldName = "UsableSqFtNum", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction End", FieldDisplayName = "Est Construction End", FieldName = "EstimatedConstructionEnd", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", FieldName = "StateLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Gross Sq Ft", FieldDisplayName = "Gross Sq Ft", FieldName = "RetailSqftNum", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Duration(In weeks)", FieldDisplayName = "Duration(In weeks)", FieldName = "CRMDuration", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", FieldName = "Zip", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "LEED", FieldDisplayName = "LEED", FieldName = "LeadLevel", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "WELL", FieldDisplayName = "WELL", FieldName = "WELLLevels", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Precon Start Date", FieldDisplayName = "Precon Start Date", FieldName = "PreconStartDate", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Precon End Date", FieldDisplayName = "Precon End Date", FieldName = "PreconEndDate", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldDisplayName = "Project Info", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Competitors", FieldDisplayName = "Competitors", FieldName = "Competition", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });

            seqNum = 0;
            //1
            mList.Add(new ModuleFormLayout() { Title = "Opportunity Info", FieldDisplayName = "Opportunity Info", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CPRProjectTitleControl", FieldDisplayName = "CPRProjectTitleControl", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Title", FieldDisplayName = "Project Title", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Dashboard", FieldDisplayName = "Dashboard", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "ProjectTeam", FieldDisplayName = "ProjectTeam", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "TaskGraph", FieldDisplayName = "TaskGraph", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "ProjectSummaryControl", FieldDisplayName = "ProjectSummaryControl", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CPR Dashboard", FieldDisplayName = "CPR Dashboard", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "TimeLine", FieldDisplayName = "TimeLine", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "TimelineControl", FieldDisplayName = "TimelineControl", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "TimeLine", FieldDisplayName = "TimeLine", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //2
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Opportunity Name", FieldDisplayName = "Opportunity Name", FieldName = "Title", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Company", FieldDisplayName = "Company", FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Opportunity Status", FieldDisplayName = "Opportunity Status", FieldName = "CRMOpportunityStatus", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Signed NDA", FieldDisplayName = "Signed NDA", FieldName = "SignedNDA", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldDisplayName = "Assigned To", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", FieldName = "Description", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "Related LEM ID", FieldDisplayName = "Related LEM ID", FieldName = "LEMIdLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Type", FieldDisplayName = "Project Type", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Studio", FieldDisplayName = "Studio", FieldName = "Studio", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Business Unit", FieldDisplayName = "Business Unit", FieldName = "CRMBusinessUnit", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Project Type", FieldDisplayName = "Secondary Project Type", FieldName = "AdditionalInfo", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Complexity", FieldDisplayName = "Project Complexity", FieldName = "CRMProjectComplexity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Owner Contract Type", FieldDisplayName = "Owner Contract Type", FieldName = "OwnerContractType", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "BCCI Sector", FieldDisplayName = "BCCI Sector", FieldName = "SectorChoice", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Market Sector", FieldDisplayName = "Market Sector", FieldName = "MarketSector", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Opportunity Type", FieldDisplayName = "Opportunity Type", FieldName = "OpportunityType", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Chance of Success", FieldDisplayName = "Chance of Success", FieldName = "ChanceOfSuccess", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Lead Priority", FieldName = "SuccessChance", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayName = "Lead Priority", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Urgency", FieldDisplayName = "Urgency", FieldName = "CRMUrgency", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldDisplayName = "Project Info", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Street", FieldDisplayName = "Street", FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldDisplayName = "Contract Value", FieldName = "ApproxContractValue", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction Start", FieldDisplayName = "Est Construction Start", FieldName = "EstimatedConstructionStart", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", FieldName = "City", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Net Rentable Sq Ft", FieldDisplayName = "Net Rentable Sq Ft", FieldName = "UsableSqFtNum", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction End", FieldDisplayName = "Est Construction End", FieldName = "EstimatedConstructionEnd", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", FieldName = "StateLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Gross Sq Ft", FieldDisplayName = "Gross Sq Ft", FieldName = "RetailSqftNum", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Duration(Weeks)", FieldDisplayName = "Duration(Weeks)", FieldName = "CRMDuration", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", FieldName = "Zip", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "LEED", FieldDisplayName = "LEED", FieldName = "LeadLevel", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "WELL", FieldDisplayName = "WELL", FieldName = "WELLLevels", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Precon Start Date", FieldDisplayName = "Precon Start Date", FieldName = "PreconStartDate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Precon End Date", FieldDisplayName = "Precon End Date", FieldName = "PreconEndDate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldDisplayName = "Project Info", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Competitors", FieldDisplayName = "Competitors", FieldName = "Competition", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //3
            mList.Add(new ModuleFormLayout() { Title = "Internal Team", FieldDisplayName = "Internal Team", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CRMProjectAllocation", FieldDisplayName = "CRMProjectAllocation", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Internal Team", FieldDisplayName = "Internal Team", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Team Notes", FieldDisplayName = "Team Notes", FieldName = "Note", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //4
            mList.Add(new ModuleFormLayout() { Title = "External Team", FieldDisplayName = "External Team", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "RelatedCompanies", FieldDisplayName = "RelatedCompanies", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "External Team", FieldDisplayName = "External Team", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //5
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "DocumentControl", FieldDisplayName = "DocumentControl", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //6
            mList.Add(new ModuleFormLayout() { Title = "Proposal", FieldDisplayName = "Proposal", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Proposal Recipient", FieldDisplayName = "Proposal Recipient", FieldName = "ProposalRecipient", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Job Walk Date", FieldDisplayName = "Job Walk Date", FieldName = "JobWalkthroughDate", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Additional Recipients", FieldDisplayName = "Additional Recipients", FieldName = "AdditionalRecipients", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Question Due Date", FieldDisplayName = "Question Due Date", FieldName = "QuestionDueDate", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "E Copy Deadline", FieldDisplayName = "E Copy Deadline", FieldName = "ProposalDeadline", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Weekly General Conditions", FieldDisplayName = "Weekly General Conditions", FieldName = "WeeklyGeneralConditions", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Hard Copy Deadline", FieldDisplayName = "Hard Copy Deadline", FieldName = "HardCopyDeadline", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Fee(%)", FieldDisplayName = "Fee(%)", FieldName = "Fee", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Interview Date", FieldDisplayName = "Interview Date", FieldName = "InterviewDate", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Insurance(%)", FieldDisplayName = "Insurance(%)", FieldName = "Insurance", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Decision Date", FieldDisplayName = "Decision Date", FieldName = "DecisionDate", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Proposal", FieldDisplayName = "Proposal", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });

            seqNum = 0;
            //7
            mList.Add(new ModuleFormLayout() { Title = "Activities", FieldDisplayName = "Activities", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "preconditionlist", FieldDisplayName = "preconditionlist", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Activities", FieldDisplayName = "Activities", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //8
            mList.Add(new ModuleFormLayout() { Title = "CheckList", FieldDisplayName = "CheckList", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CheckListProjectView", FieldDisplayName = "CheckListProjectView", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CheckList", FieldDisplayName = "CheckList", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //9
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //10
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", FieldName = "Comment", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });

            seqNum = 0;
            //11
            mList.Add(new ModuleFormLayout() { Title = "Close Opportunity", FieldDisplayName = "Close Opportunity", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Close Date", FieldDisplayName = "Close Date", FieldName = "ClosedDateOnly", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Lost To", FieldDisplayName = "Lost To", FieldName = "CompetitorName", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Reason", FieldDisplayName = "Reason", FieldName = "Reason", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Close Opportunity", FieldDisplayName = "Close Opportunity", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //12
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "IsReadOnly=True;#IsInFrame=False", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //13
            mList.Add(new ModuleFormLayout() { Title = "Contract Details", FieldDisplayName = "Contract Details", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "CRMOwnerContractDetails", FieldDisplayName = "CRMOwnerContractDetails", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Contract Details", FieldDisplayName = "Contract Details", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Waiver of Consequential Damages", FieldDisplayName = "Waiver of Consequential Damages", FieldName = "WaiverDamages", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Lien Waivers", FieldDisplayName = "Lien Waivers", FieldName = "LienWaiver", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Retainage", FieldDisplayName = "Retainage", FieldName = "Retainage", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Waiver of Subrogation", FieldDisplayName = "Waiver of Subrogation", FieldName = "WaiverSubrogation", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Subcontractor Default Insurance", FieldDisplayName = "Subcontractor Default Insurance", FieldName = "SubcontractorDefaultInsurance", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Diverse Certifications", FieldDisplayName = "Diverse Certifications", FieldName = "DiverseCertificationChoice", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Dispute Resolution", FieldDisplayName = "Dispute Resolution", FieldName = "DisputeResolution", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Payment & Performance Bonds", FieldDisplayName = "Payment & Performance Bonds", FieldName = "PaymentAndPerformanceBonds", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Executive Order 11246 Requirements", FieldDisplayName = "Executive Order 11246 Requirements", FieldName = "ExecutiveOrderRequirements", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Bonus/Incentive", FieldDisplayName = "Bonus/Incentive", FieldName = "Bonus", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Extended General Conditions for Delay", FieldDisplayName = "Extended General Conditions for Delay", FieldName = "GeneralConditionsDelay", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Subcontractor Mark-Ups", FieldDisplayName = "Subcontractor Mark-Ups", FieldName = "SubContractorMarkUp", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Liquidated Damages", FieldDisplayName = "Liquidated Damages", FieldName = "LiquidatedDamages", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Change Orders", FieldDisplayName = "Change Orders", FieldName = "ApprovedChangeOrders", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Builder`s Risk", FieldDisplayName = "Builder`s Risk", FieldName = "BuilderRisk", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Disputed Work Cap", FieldDisplayName = "Disputed Work Cap", FieldName = "DisputedWorkCap", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Certifying Agency", FieldDisplayName = "Certifying Agency", FieldName = "CertifyingAgency", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Savings", FieldDisplayName = "Savings", FieldName = "Savings", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Payments", FieldDisplayName = "Payments", FieldName = "Payments", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "ContractStatusChoice", FieldDisplayName = "ContractStatusChoice", FieldName = "ContractStatusChoice", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "RetainageChoice", FieldDisplayName = "RetainageChoice", FieldName = "RetainageChoice", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "ContractNotes", FieldDisplayName = "ContractNotes", FieldName = "ContractNotes", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });

            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("RequestType");

            mList.Add(new ModuleRequestType() { Title = "Auditorium", ModuleNameLookup = ModuleName, RequestType = "Auditorium", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Cafeteria", ModuleNameLookup = ModuleName, RequestType = "Cafeteria", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Corridors", ModuleNameLookup = ModuleName, RequestType = "Corridors", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Elevator/Escalator", ModuleNameLookup = ModuleName, RequestType = "Elevator/Escalator", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Restroom", ModuleNameLookup = ModuleName, RequestType = "Restroom", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Storefront/Entry", ModuleNameLookup = ModuleName, RequestType = "Storefront/Entry", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Industrial Building", ModuleNameLookup = ModuleName, RequestType = "Industrial Building", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Mixed Use", ModuleNameLookup = ModuleName, RequestType = "Mixed Use", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Office Building", ModuleNameLookup = ModuleName, RequestType = "Office Building", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Office Park/Campus Development", ModuleNameLookup = ModuleName, RequestType = "Office Park/Campus Development", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Warehouse", ModuleNameLookup = ModuleName, RequestType = "Warehouse", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Broadcast Studio", ModuleNameLookup = ModuleName, RequestType = "Broadcast Studio", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Classroom", ModuleNameLookup = ModuleName, RequestType = "Classroom", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Data Center", ModuleNameLookup = ModuleName, RequestType = "Data Center", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Fitness Center/Gym", ModuleNameLookup = ModuleName, RequestType = "Fitness Center/Gym", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Interconnecting Stair", ModuleNameLookup = ModuleName, RequestType = "Interconnecting Stair", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Laboratory", ModuleNameLookup = ModuleName, RequestType = "Laboratory", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Market Ready/Spec Suite", ModuleNameLookup = ModuleName, RequestType = "Market Ready/Spec Suite", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Office Space", ModuleNameLookup = ModuleName, RequestType = "Office Space", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Mixed Use > Mixed Use Office", ModuleNameLookup = ModuleName, RequestType = "Mixed Use Office", Category = "Construction", SubCategory = "Mixed Use - 16", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Mixed Use > Mixed Use Residential", ModuleNameLookup = ModuleName, RequestType = "Mixed Use Residential", Category = "Construction", SubCategory = "Mixed Use - 16", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Apartments", ModuleNameLookup = ModuleName, RequestType = "Apartments", Category = "Construction", SubCategory = "Residential - 17", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Condominiums", ModuleNameLookup = ModuleName, RequestType = "Condominiums", Category = "Construction", SubCategory = "Residential - 17", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Student Housing", ModuleNameLookup = ModuleName, RequestType = "Student Housing", Category = "Construction", SubCategory = "Residential - 17", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > ADA Access", ModuleNameLookup = ModuleName, RequestType = "ADA Access", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Hardscape", ModuleNameLookup = ModuleName, RequestType = "Hardscape", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Landscaping", ModuleNameLookup = ModuleName, RequestType = "Landscaping", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Playground", ModuleNameLookup = ModuleName, RequestType = "Playground", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Core And Shell", ModuleNameLookup = ModuleName, RequestType = "Core And Shell", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Day Two", ModuleNameLookup = ModuleName, RequestType = "Day Two", Category = "Construction", SubCategory = "Day Two - 11", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Demolition", ModuleNameLookup = ModuleName, RequestType = "Demolition", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Distribution Center/Warehouse", ModuleNameLookup = ModuleName, RequestType = "Distribution Center/Warehouse", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Elevator/Escalator", ModuleNameLookup = ModuleName, RequestType = "Elevator/Escalator", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Historic", ModuleNameLookup = ModuleName, RequestType = "Historic", Category = "Construction", SubCategory = "Historic - 08", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Hospitality", ModuleNameLookup = ModuleName, RequestType = "Hospitality", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Interiors - 01", ModuleNameLookup = ModuleName, RequestType = "Interiors - 01", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Lobby", ModuleNameLookup = ModuleName, RequestType = "Lobby", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Lobby", ModuleNameLookup = ModuleName, RequestType = "Lobby", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Seismic Structural - 07", ModuleNameLookup = ModuleName, RequestType = "Seismic Structural - 07", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Service", ModuleNameLookup = ModuleName, RequestType = "Service", Category = "Construction", SubCategory = "Service - 12", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Service - 12", ModuleNameLookup = ModuleName, RequestType = "Service - 12", Category = "Construction", SubCategory = "Service - 12", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Warranty", ModuleNameLookup = ModuleName, RequestType = "Warranty", Category = "Construction", SubCategory = "Warranty - 15", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Architecture", ModuleNameLookup = ModuleName, RequestType = "Architecture", Category = "Professional Services", SubCategory = "Architecture - 02", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction Management", ModuleNameLookup = ModuleName, RequestType = "Construction Management", Category = "Professional Services", SubCategory = "Construction Management - 02", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Permit Services", ModuleNameLookup = ModuleName, RequestType = "Permit Services", Category = "Professional Services", SubCategory = "Permit Services - 02", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > LEED", ModuleNameLookup = ModuleName, RequestType = "LEED", Category = "Professional Services", SubCategory = "Sustainability - 02", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > Living Building Challenge", ModuleNameLookup = ModuleName, RequestType = "Living Building Challenge", Category = "Professional Services", SubCategory = "Sustainability - 02", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > WELL", ModuleNameLookup = ModuleName, RequestType = "WELL", Category = "Professional Services", SubCategory = "Sustainability - 02", FunctionalAreaLookup = 1, WorkflowType = "SkipApprovals", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "PSG-Design Build", ModuleNameLookup = ModuleName, RequestType = "PSG-Design Build", Category = "Professional Services", SubCategory = "PSG-Design Build - 02", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {

            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            //int StageStep = 0;
            // mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AdditionalInfo", Title = "AdditionalInfo", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AdditionalRecipients", Title = "AdditionalRecipients", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApproxContractValue", Title = "ApproxContractValue", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApproxContractValue", Title = "ApproxContractValue", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApproxContractValue", Title = "ApproxContractValue", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApproxContractValue", Title = "ApproxContractValue", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BuildingOwner", Title = "BuildingOwner", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BuildingOwnerCompany", Title = "BuildingOwnerCompany", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ChanceOfSuccess", Title = "ChanceOfSuccess", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "City", Title = "City", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ClosedDateOnly", Title = "ClosedDateOnly", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CompanyArchitect", Title = "CompanyArchitect", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CompanyEngineer", Title = "CompanyEngineer", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Competition", Title = "Competition", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CompetitorName", Title = "CompetitorName", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ConstructionManagementCompany", Title = "ConstructionManagementCompany", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ConstructionManagerContact", Title = "ConstructionManagerContact", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContactLookup", Title = "ContactLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CRMBusinessUnit", Title = "CRMBusinessUnit", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            //mList.Add(new ModuleRoleWriteAccess() { FieldName = "CRMProjectComplexity", Title = "CRMProjectComplexity", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CRMCompanyLookup", Title = "CRMCompanyLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CRMDuration", Title = "CRMDuration", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CRMOpportunityStatus", Title = "CRMOpportunityStatus", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DecisionDate", Title = "DecisionDate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EngineerContact", Title = "EngineerContact", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstimatedConstructionEnd", Title = "EstimatedConstructionEnd", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstimatedConstructionStart", Title = "EstimatedConstructionStart", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Fee", Title = "Fee(%)", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "GoNoGo", Title = "GoNoGo", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HardCopyDeadline", Title = "HardCopyDeadline", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HardCopyRequired", Title = "HardCopyRequired", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Insurance", Title = "Insurance(%)", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InterviewDate", Title = "InterviewDate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "JobWalkthroughDate", Title = "JobWalkthroughDate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LeadLevel", Title = "LeadLevel", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "MarketSector", Title = "MarketSector", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CRMUrgency", Title = "CRMUrgency", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerContractType", Title = "OwnerContractType", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OpportunityType", Title = "OpportunityType", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Note", Title = "Note", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectTeamLookup", Title = "ProjectTeamLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProposalDeadline", Title = "ProposalDeadline", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProposalRecipient", Title = "ProposalRecipient", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "QuestionDueDate", Title = "QuestionDueDate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Reason", Title = "Reason", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RetailSqft", Title = "RetailSqft", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RetailSqftNum", Title = "RetailSqftNum", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SignedNDA", Title = "SignedNDA", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "StreetAddress1", Title = "StreetAddress1", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CloseDate", Title = "CloseDate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", Title = "Description", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", Title = "Owner", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", Title = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", Title = "Title", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "StateLookup", Title = "StateLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsableSqFt", Title = "UsableSqFt", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsableSqFtNum", Title = "UsableSqFtNum", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "WeeklyGeneralConditions", Title = "WeeklyGeneralConditions", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "WELLLevels", Title = "WELLLevels", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Zip", Title = "Zip", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SectorChoice", Title = "BCCI Sector", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Studio", Title = "Studio", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OpportunitySize", Title = "OpportunitySize", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectMovingForward", Title = "ProjectMovingForward", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DesiredCompletionDate", Title = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeCategory", Title = "RequestTypeCategory", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LeadLevel", Title = "LeadLevel", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OpportunitySize", Title = "OpportunitySize", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectMovingForward", Title = "ProjectMovingForward", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORP", Title = "ORP", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRP", Title = "PRP", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeCategory", Title = "RequestTypeCategory", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LeadLevel", Title = "LeadLevel", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BusinessUnit", Title = "BusinessUnit", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualStartDate", Title = "ActualStartDate", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LeadLevel", Title = "LeadLevel", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LeadLevel", Title = "LeadLevel", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PreparedBy", Title = "PreparedBy", ModuleNameLookup = ModuleName, StageStep = 5, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProposalAmount", Title = "ProposalAmount", ModuleNameLookup = ModuleName, StageStep = 5, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SubmissionDate", Title = "SubmissionDate", ModuleNameLookup = ModuleName, StageStep = 5, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LeadLevel", Title = "LeadLevel", ModuleNameLookup = ModuleName, StageStep = 5, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ClientAcceptance", Title = "ClientAcceptance", ModuleNameLookup = ModuleName, StageStep = 6, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TargetStartDate", Title = "TargetStartDate", ModuleNameLookup = ModuleName, StageStep = 6, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LeadLevel", Title = "LeadLevel", ModuleNameLookup = ModuleName, StageStep = 6, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");
            //int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName+"-"+"Identify Opportunity", ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Assign", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Go/No Go", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Develop Approach", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Manage Bids", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Prepare Estimates", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Manage Bids", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Manage Bids", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Closed", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4 });
            return mList;

        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {

            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            //int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Identify Opportunity",
            //    Status = "Identify Opportunity",
            //    EmailTitle = "New Opportunity Ticket Created [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] has been initiated and needs clarification.<br><br>" +
            //                                            "Please enter the required information and re-submit the ticket.",
            //    ModuleNameLookup = ModuleName,
            //    StageStep = stageID
            //});

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Assign",
            //    Status ="Assign",
            //    EmailTitle = "OPM Ticket Pending Assignment [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
            //                                            "Please assign a PRP and enter any other required fields",
            //    ModuleNameLookup  = ModuleName,
            //    StageStep = (++stageID)});

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Go/No Go",
            //    Status = "Go/No Go",
            //    EmailTitle = "OPM Ticket Assigned [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] has been assigned to you for Go/No Go.<br><br>" +
            //                                            "Once complete, please enter the analysis in the ticket.",
            //    ModuleNameLookup = ModuleName,
            //    StageStep = (++stageID) });

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Develop Approach",
            //    Status = "Develop Approach",
            //    EmailTitle = "OPM Ticket Pending Implementation [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] has completed analysis and is pending fix implementation.<br><br>" +
            //                                        "Please review the analysis, implement the recommendations and close the ticket.<br><br>[$IncludeActionButtons$]",
            //    ModuleNameLookup = ModuleName,
            //    StageStep = (++stageID)});

            //mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Manage Bids", Status = "Manage Bids", EmailTitle = "OPM Ticket Closed [$TicketId$]: [$Title$]", EmailBody = "OPM Ticket ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (++stageID)});

            //mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Prepare Estimates", Status = "Prepare Estimates", EmailTitle = "OPM Ticket On Hold [$TicketId$]: [$Title$]", EmailBody = "OPM Ticket ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null});

            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();

            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Unassigned", ViewName = "Opportunity", TabOrder = 1, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "Opportunity", TabOrder = 2, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "Opportunity", TabOrder = 3, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "Opportunity", TabOrder = 4, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "Opportunity", TabOrder = 5, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Open", ViewName = ModuleName, TabOrder = 6, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "Opportunity", TabOrder = 7, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "Opportunity", TabOrder = 8, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "Opportunity", TabOrder = 9, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "ON Hold" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Closed", ViewName = "Opportunity", TabOrder = 10, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            return tabs;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {

        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
    }
}
