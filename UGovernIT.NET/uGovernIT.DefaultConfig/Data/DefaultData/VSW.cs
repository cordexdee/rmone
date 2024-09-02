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
    public class VSW : IModule
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
        protected string ModuleName = "VSW";
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
                    ShortName = "Vendor SOW Management",
                    CategoryName = "Vendor SOW Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "VendorSOW",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/vswtickets",
                    ModuleHoldMaxStage = 0,
                    Title = "Vendor SOW Management (VSW)",
                    ModuleDescription = "This module is used to create, approve and manage finance with external vendors for tracking contract costs, approval workflows and automated expiration reminders.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/vsw",
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
                return "VSW";
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
            mList.Add(new ModuleFormTab() { TabName = "Fees", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Fees" });
            mList.Add(new ModuleFormTab() { TabName = "Documents", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Documents" });
            mList.Add(new ModuleFormTab() { TabName = "Dashboard", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Dashboard" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });
            mList.Add(new ModuleFormTab() { TabName = "Purchase Orders", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Purchase Orders" });

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
                UserPrompt = "<b>Please fill the form to open a new PRS ticket.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "SOW Submitted",
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
                UserPrompt = "<b>SOW is Active till the expired date.</b>",
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
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });
            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // VSW
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "TicketId", FieldDisplayName = "SOW #", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "AgreementNumber", FieldDisplayName = "Agreement #", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "VendorMSALookup", FieldDisplayName = "MSA", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "Status", FieldDisplayName = "Status", IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "StageStep", FieldDisplayName = "Status", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "RequestTypeLookup", FieldDisplayName = "Contract Type", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsUseInWildCard = true, IsDisplay = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VSW", FieldName = "EffectiveEndDate", FieldDisplayName = "End Date", IsDisplay = true, CustomProperties = "miniview=true", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
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
            List<string[]> dataList = new List<string[]>();
            int seqNum = 0;
            // Tab 1
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Status", FieldDisplayName = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Agreement Number", FieldDisplayName = "Agreement Number", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AgreementNumber", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Ref ID", FieldDisplayName = "Ref ID", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "KeyRefUniqueID", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Signing Date", FieldDisplayName = "Signing Date", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ContractSigningDate", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Contract Type", FieldDisplayName = "Contract Type", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "FinancialManager", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldDisplayName = "Contract Value", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ContractValue", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Start Date", FieldDisplayName = "Start Date", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "EffectiveStartDate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "End Date", FieldDisplayName = "End Date", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "EffectiveEndDate", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "Additional Information", FieldDisplayName = "Additional Information", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "AdditionalInformation", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Partial Termination Possible", FieldDisplayName = "Partial Termination Possible", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IsPartialTermination", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Continuous Improvement", FieldDisplayName = "Continuous Improvement", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "VendorSOWContImprovementList", FieldDisplayName = "VendorSOWContImprovementList", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = false, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Continuous Improvement", FieldDisplayName = "Continuous Improvement", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 2: SOW Fees
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "SOW Fees", FieldDisplayName = "SOW Fees", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "SOW Fees" });
            mList.Add(new ModuleFormLayout() { Title = "VendorSOWFeeList", FieldDisplayName = "VendorSOWFeeList", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "SOW Fees", FieldDisplayName = "SOW Fees", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "SOW Fees" });

            //Tab 3 - Documents
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "DocumentControl", FieldDisplayName = "DocumentControl", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 4: SLA Dashboard
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Performance Dashboard", FieldDisplayName = "Performance Dashboard", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Performance Dashboard" });
            mList.Add(new ModuleFormLayout() { Title = "VendorSLADashboard", FieldDisplayName = "VendorSLADashboard", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Performance Dashboard", FieldDisplayName = "Performance Dashboard", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Performance Dashboard" });

            // Tab 5: Lifecycle 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // Tab 6: Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "TicketComment", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 7: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History" });

            // Tab 8: Purchase Orders
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Purchase Orders", FieldDisplayName = "Purchase Orders", ModuleNameLookup = ModuleName, TabId = 8, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Purchase Orders" });
            mList.Add(new ModuleFormLayout() { Title = "VendorPOControl", FieldDisplayName = "VendorPOControl", ModuleNameLookup = ModuleName, TabId = 8, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Purchase Orders", FieldDisplayName = "Purchase Orders", ModuleNameLookup = ModuleName, TabId = 8, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Purchase Orders" });


            // New Ticket Form

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Agreement Number", FieldDisplayName = "Agreement Number", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AgreementNumber", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Ref ID", FieldDisplayName = "Ref ID", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "KeyRefUniqueID", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Signing Date", FieldDisplayName = "Signing Date", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ContractSigningDate", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Contract Type", FieldDisplayName = "Contract Type", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "FinancialManager", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldDisplayName = "Contract Value", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ContractValue", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Start Date", FieldDisplayName = "Start Date", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "EffectiveStartDate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "End Date", FieldDisplayName = "End Date", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "EffectiveEndDate", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "Additional Information", FieldDisplayName = "Additional Information", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "AdditionalInformation", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Partial Termination Possible", FieldDisplayName = "Partial Termination Possible", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IsPartialTermination", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("RequestType");
            mList.Add(new ModuleRequestType() { Title = "SOW", RequestType = "SOW", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Contract Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Work Order", RequestType = "Work Order", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Contract Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Change Order", RequestType = "Change Order", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Contract Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("RequestRoleWriteAccess");
            int StageStep = 0;
            // All stages
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            // Stage 0 - New ticket
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Attachments", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ContractSigningDate", FieldName = "ContractSigningDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "AgreementNumber", FieldName = "AgreementNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "KeyRefUniqueID", FieldName = "KeyRefUniqueID", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "AdditionalInformation", FieldName = "AdditionalInformation", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ContractValue", FieldName = "ContractValue", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "EffectiveStartDate", FieldName = "EffectiveStartDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "EffectiveEndDate", FieldName = "EffectiveEndDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "IsPartialTermination", FieldName = "IsPartialTermination", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "FinancialManager", FieldName = "FinancialManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("TicketStatusMapping");
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
            throw new NotImplementedException();
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSW", Title = "VSW-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSW", Title = "VSW-MGS Manager", UserTypes = "MGS Manager", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSW", Title = "VSW-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSW", Title = "VSW-Financial Manager", UserTypes = "Financial Manager", ColumnName = "FinancialManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSW", Title = "VSW-Performance Manager", UserTypes = "Performance Manager", ColumnName = "PerformanceManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSW", Title = "VSW-Delivery Manager", UserTypes = "Delivery Manager", ColumnName = "ServiceDeliveryManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VSW", Title = "VSW-Functional Manager", UserTypes = "Functional Manager", ColumnName = "FunctionalManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "VSW", TabOrder = 1, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "VSW", TabOrder = 2, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "VSW", TabOrder = 3, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "VSW", TabOrder = 4, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "VSW", TabOrder = 5, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Depatment Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "VSW", TabOrder = 6, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "VSW", TabOrder = 7, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "VSW", TabOrder = 8, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "VSW", TabOrder = 9, ModuleNameLookup = "VSW", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
