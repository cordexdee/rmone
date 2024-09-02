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
    public class VSL : IModule
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
        protected string ModuleName = "VSL";
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
                    ShortName = "Vendor SLA Management",
                    CategoryName = "Vendor SLA Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "VendorSLA",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/sitePages/vsltickets",
                    ModuleHoldMaxStage = 0,
                    Title = "Vendor SLA Management (VSL)",
                    ModuleDescription = "This module is used to create, approve and manage finance with external vendors for tracking contract costs, approval workflows and automated expiration reminders.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/sitePages/vsl",
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    UseInGlobalSearch = true

                };
            }
        }

        string IModule.ModuleName
        {
            get
            {
                return "VSL";
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
            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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

            List<string[]> dataList = new List<string[]>();

            // Start from ID 66
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 3;
            int StageStep = 0;


            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Create new Ticket",
                UserPrompt = "<b>Please fill the form to open a new SLA Request.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "SLA Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "(Re)Submit",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Cancel",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });


            mList.Add(new LifeCycleStage()
            {
                Action = "Active",
                Name = "Active",
                StageTitle = "Active",
                UserPrompt = "<b>SLA is Active till the expired date.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Close",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Expire Now",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Cancel",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 45,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "SOW is expired, but you can add comments",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "<b>SOW Closed</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-open",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 45,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });
            return mList;

        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            // Console.WriteLine("  ModuleDefaultValues");
            int seqNum = 0;
            // VSL
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "TicketId", FieldDisplayName = "SLA ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "VendorSOWLookup", FieldDisplayName = "SOW #", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "SLANumber", FieldDisplayName = "SLA #", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "Status", FieldDisplayName = "Status", IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "StageStep", FieldDisplayName = "Status", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "SLAUnit", FieldDisplayName = "Unit", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "SLATarget", FieldDisplayName = "Target", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "MinThreshold", FieldDisplayName = "Minimum", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "HigherIsBetter", FieldDisplayName = "Higher Is Better", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "MeasurementFrequence", FieldDisplayName = "Measurement Frequency", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "Weightage", FieldDisplayName = "Weightage", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSL", FieldName = "Penalty", FieldDisplayName = "Penalty", IsUseInWildCard = true, FieldSequence = ++seqNum });

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
            // Console.WriteLine("FormLayout");
            int seqNum = 0;
            // Tab 1
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "SOW", FieldDisplayName = "SOW", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "SLA #", FieldDisplayName = "SLA #", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SLANumber", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "SLA Type", FieldDisplayName = "SLA Type", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Unit", FieldDisplayName = "Unit", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SLAUnit", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Target", FieldDisplayName = "Target", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SLATarget", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Minimum Requirement", FieldDisplayName = "Minimum Requirement", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "MinThreshold", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Measurement Frequency", FieldDisplayName = "Measurement Frequency", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "MeasurementFrequency", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Higher Is Better", FieldDisplayName = "Higher Is Better", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "HigherIsBetter", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Penalty (as % of monthly fees)", FieldDisplayName = "Penalty (as % of monthly fees)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Penalty", ShowInMobile = true, CustomProperties = "" });



            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            // Tab 2: Lifecycle 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // Tab 3: Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 4: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History" });

            // New Ticket Form
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "SOW", FieldDisplayName = "SOW", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "SLA #", FieldDisplayName = "SLA #", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SLANumber", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "SLA Type", FieldDisplayName = "SLA Type", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Unit", FieldDisplayName = "Unit", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SLAUnit", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Target", FieldDisplayName = "Target", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SLATarget", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Minimum Requirement", FieldDisplayName = "Minimum Requirement", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "MinThreshold", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Measurement Frequency", FieldDisplayName = "Measurement Frequency", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "MeasurementFrequency", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Higher Is Better", FieldDisplayName = "Higher Is Better", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "HigherIsBetter", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Penalty (as % of monthly fees)", FieldDisplayName = "Penalty (as % of monthly fees)", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Penalty", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            mList.Add(new ModuleRequestType() { Title = "SLA", RequestType = "SLA", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "SLA Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "KPI", RequestType = "KPI", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "SLA Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int StageStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SLADescription", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SLAUnit", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SLATarget", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "MinThreshold", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "MaxThreshold", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HigherIsBetter", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Reward", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Penalty", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SLANumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "MeasurementFrequency", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("TaskEmails");
            return mList;
        }

        public List<UGITModule> GetUGITModule()
        {
            List<UGITModule> mList = new List<UGITModule>();
            // // Console.WriteLine("TaskEmails");
            return mList;
            // throw new NotImplementedException();
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSL", Title = "VSL-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSL", Title = "VSL-MGS Manager", UserTypes = "MGS Manager", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSL", Title = "VSL-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSL", Title = "VSL-Financial Manager", UserTypes = "Financial Manager", ColumnName = "FinancialManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSL", Title = "VSL-Performance Manager", UserTypes = "Performance Manager", ColumnName = "PerformanceManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSL", Title = "VSL-Delivery Manager", UserTypes = "Delivery Manager", ColumnName = "ServiceDeliveryManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSL", Title = "VSL-Functional Manager", UserTypes = "Functional Manager", ColumnName = "FunctionalManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "VSL", TabOrder = 1, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "VSL", TabOrder = 2, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "VSL", TabOrder = 3, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "VSL", TabOrder = 4, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "VSL", TabOrder = 5, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "VSL", TabOrder = 6, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "VSL", TabOrder = 7, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "VSL", TabOrder = 8, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "VSL", TabOrder = 9, ModuleNameLookup = "VSL", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
