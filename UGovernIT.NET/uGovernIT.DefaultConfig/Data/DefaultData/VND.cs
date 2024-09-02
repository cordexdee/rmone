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

    public class VND : IModule
    {

        protected string moduleId = "19";
        protected static bool enableOwnerApprovalStage = true;
        protected static bool enableFinanceApprovalStage = true;
        public static string userId = "1";
        public static string managerUserId = "1";
        public static string userName = ""; // Obtained from userId in UGovernITDefault.Initialize()

        public static string[] HideInTemplate = { "Attachments", "AssetLookup", "Comment", "CreationDate", "Initiator", "RequestTypeCategory", "Status" };


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
        protected string ModuleName = "VND";
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
                    ShortName = "Vendor Management",
                    CategoryName = "Vendor Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "VendorMSA",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/vndtickets",
                    ModuleHoldMaxStage = 0,
                    Title = "Vendor Management (VND)",
                    ModuleDescription = "This module is used to create, approve and manage contracts with external vendors for tracking contract costs, approval workflows and automated expiration reminders.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/vnd",
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
                return "VND";
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
            mList.Add(new ModuleFormTab() { TabName = "General", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General" });
            mList.Add(new ModuleFormTab() { TabName = "SOW", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "SOW" });
            mList.Add(new ModuleFormTab() { TabName = "Performance", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Performance" });
            mList.Add(new ModuleFormTab() { TabName = "Invoices", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Invoices" });
            mList.Add(new ModuleFormTab() { TabName = "Documents", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Documents" });
            mList.Add(new ModuleFormTab() { TabName = "Deliverables", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Deliverables" });
            mList.Add(new ModuleFormTab() { TabName = "Governance", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Governance" });
            mList.Add(new ModuleFormTab() { TabName = "Action Log", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Action Log" });
            mList.Add(new ModuleFormTab() { TabName = "Dashboard", TabId = 9, TabSequence = 9, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Dashboard" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 10, TabSequence = 10, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 11, TabSequence = 11, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "Contract Changes", TabId = 12, TabSequence = 12, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Contract Changes" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 13, TabSequence = 13, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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


        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VND", Title = "VND-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VND", Title = "VND-MGS Manager", UserTypes = "MGS Manager", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VND", Title = "VND-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VND", Title = "VND-Financial Manager", UserTypes = "Financial Manager", ColumnName = "FinancialManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VND", Title = "VND-Performance Manager", UserTypes = "Performance Manager", ColumnName = "PerformanceManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VND", Title = "VND-Delivery Manager", UserTypes = "Delivery Manager", ColumnName = "ServiceDeliveryManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VND", Title = "VND-Functional Manager", UserTypes = "Functional Manager", ColumnName = "FunctionalManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
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
            int moduleStep = 0;

            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Create new Ticket",
                UserPrompt = "<b>Please fill the form to open a new VPM Request.</b>",
                StageStep = moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "VPM Submitted",
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
                UserPrompt = "<b>MSA is Active till the expired date.</b>",
                StageStep = moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Close MSA",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Expire Now",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Cancel MSA",
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
                Action = "VPM is expired, but you can add comments",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "<b>VPM Closed</b>",
                StageStep = moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "Owner re-opened MSA",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-open MSA",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
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
            int seqNum = 0;
            // VND
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "TicketId", FieldDisplayName = "MSA #", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
            //mList.Add(new ModuleColumns() { ModuleNameLookup = "VND", FieldName ="Status", "Progress", "1" , FieldSequence=++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "OnHold", FieldDisplayName = "OnHold", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "CreationDate", FieldDisplayName = "Created On", IsDisplay = true, DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "PctComplete", FieldDisplayName = "% Comp", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "CloseDate", FieldDisplayName = "Closed On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VND", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            int startStageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");
            int seqNum = 0;

            // Tab 1
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Status", FieldDisplayName = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Ref ID", FieldDisplayName = "Ref ID", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "KeyRefUniqueID", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Currency", FieldDisplayName = "Currency", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Currency", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldDisplayName = "Contract Value", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ContractValue", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Contract Owner", FieldDisplayName = "Contract Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FinancialManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Signing Date", FieldDisplayName = "Signing Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ContractSigningDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Effective Start Date", FieldDisplayName = "Effective Start Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "EffectiveStartDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Effective End Date", FieldDisplayName = "Effective End Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "EffectiveEndDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Service Durations", FieldDisplayName = "Service Durations", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Service Durations", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorServiceDurationList", FieldDisplayName = "VendorServiceDurationList", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Service Durations", FieldDisplayName = "Service Durations", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Service Durations", SkipOnCondition = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination By Client", FieldDisplayName = "Termination By Client", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination For Convenience", FieldDisplayName = "Termination For Convenience", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TerminationByClient", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Notice Period", FieldDisplayName = "Termination Notice Period", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TerminationNoticePeriod", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Trigger(s)", FieldDisplayName = "Termination Trigger(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ClientTerminationTriggers", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination for Force Majeure Event", FieldDisplayName = "Termination for Force Majeure Event", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TerminationbyClientForceMajeureEvent", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination for Incurred Liability", FieldDisplayName = "Termination for Incurred Liability", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TerminationForIncurredLiability", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Additional Comments", FieldDisplayName = "Additional Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AdditionalComments", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination By Client", FieldDisplayName = "Termination By Client", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination By Vendor", FieldDisplayName = "Termination By Vendor", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination By Vendor", FieldDisplayName = "Termination By Vendor", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TerminationByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Notice Period", FieldDisplayName = "Termination Notice Period", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TerminationNoticePeriodByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Trigger(s)", FieldDisplayName = "Termination Trigger(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TerminationTriggerByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Additional Comments", FieldDisplayName = "Additional Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "AdditionalCommentsByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Charges", FieldDisplayName = "Termination Charges", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "TerminationChargesByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination By Vendor", FieldDisplayName = "Termination By Vendor", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Disentanglement", FieldDisplayName = "Disentanglement", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Disentanglement Term (months)", FieldDisplayName = "Disentanglement Term (months)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DisentanglementTerm", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Disentanglement Transition Plan (days)", FieldDisplayName = "Disentanglement Transition Plan (days)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DisentanglementTransitionPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Disentanglement", FieldDisplayName = "Disentanglement", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Financial Payment Terms", FieldDisplayName = "Financial Payment Terms", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Payment Due (in days)", FieldDisplayName = "Payment Due (in days)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PaymentDueTerm", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Payment Delay Interest", FieldDisplayName = "Payment Delay Interest", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "PaymentDelayInterest", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Other Payment Terms", FieldDisplayName = "Other Payment Terms", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "OtherPaymentTerms", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Financial Payment Terms", FieldDisplayName = "Financial Payment Terms", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Vendor Key Personnel", FieldDisplayName = "Vendor Key Personnel", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Vendor Key Personnel", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorKeyPersonnelList", FieldDisplayName = "VendorKeyPersonnelList", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Vendor Key Personnel", FieldDisplayName = "Vendor Key Personnel", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Vendor Key Personnel", SkipOnCondition = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Approved Subcontractors", FieldDisplayName = "Approved Subcontractors", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Approved Subcontractors", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorApprovedSubContractorsList", FieldDisplayName = "VendorApprovedSubContractorsList", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Approved Subcontractors", FieldDisplayName = "Approved Subcontractors", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Approved Subcontractors", SkipOnCondition = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Legal", FieldDisplayName = "Legal", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Jurisdiction", FieldDisplayName = "Jurisdiction", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LegalJurisdiction", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Compliance List", FieldDisplayName = "Compliance List", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LegalCompliances", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Publicity Allowed?", FieldDisplayName = "Publicity Allowed?", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LegalPublicity", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Warranties & Representations", FieldDisplayName = "Warranties & Representations", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "LegalWarranties", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Legal", FieldDisplayName = "Legal", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Issue & Dispute Resolution", FieldDisplayName = "Issue & Dispute Resolution", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Issue & Dispute Resolution", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Resolution Process Exist", FieldDisplayName = "Resolution Process Exist", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "IssueResolutionProcessExists", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Resolution Process Summary", FieldDisplayName = "Resolution Process Summary", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "IssueResolutionProcessSummary", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Issue & Dispute Resolution", FieldDisplayName = "Issue & Dispute Resolution", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Issue & Dispute Resolution", SkipOnCondition = "", FieldSequence = ++seqNum });
            seqNum = 0;
            //Tab 2: SOW
            mList.Add(new ModuleFormLayout() { Title = "SOW", FieldDisplayName = "SOW", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorSOWControl", FieldDisplayName = "VendorSOWControl", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "SOW", FieldDisplayName = "SOW", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //Tab 3: Performance
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Performance", FieldDisplayName = "Performance", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorPMControl", FieldDisplayName = "VendorPMControl", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Performance", FieldDisplayName = "Performance", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //Tab 4: Invoices
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Invoices", FieldDisplayName = "Invoices", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorFMControl", FieldDisplayName = "VendorFMControl", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Invoices", FieldDisplayName = "Invoices", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //Tab 5 - Documents
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "DocumentControl", FieldDisplayName = "DocumentControl", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //Tab 6: Deliverables
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Deliverables & Obligations", FieldDisplayName = "Deliverables & Obligations", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Deliverables & Obligations", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorReportList", FieldDisplayName = "VendorReportList", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Deliverables & Obligations", FieldDisplayName = "Deliverables & Obligations", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Deliverables & Obligations", SkipOnCondition = "", FieldSequence = ++seqNum });

            //Tab 7: Governance
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Governance", FieldDisplayName = "Governance", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Governance", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorMeetingList", FieldDisplayName = "VendorMeetingList", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Governance", FieldDisplayName = "Governance", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Governance", SkipOnCondition = "", FieldSequence = ++seqNum });

            //Tab 8: Issues & Risks
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Issues", FieldDisplayName = "Issues", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Issues", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorIssueList", FieldDisplayName = "VendorIssueList", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Issues", FieldDisplayName = "Issues", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Issues", SkipOnCondition = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Risks", FieldDisplayName = "Risks", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Risks", SkipOnCondition = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorRisksList", FieldDisplayName = "VendorRisksList", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Risks", FieldDisplayName = "Risks", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Risks", SkipOnCondition = "", FieldSequence = ++seqNum });

            //Tab 9: SLA Dashboard
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Performance Dashboard", FieldDisplayName = "Performance Dashboard", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Performance Dashboard", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VendorSLADashboard", FieldDisplayName = "VendorSLADashboard", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 2, FieldName = "#Control#", CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Performance Dashboard", FieldDisplayName = "Performance Dashboard", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Performance Dashboard", FieldSequence = ++seqNum });

            seqNum = 0;
            // Tab 10: Lifecycle 
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 11: Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //Tab 12 - Contract
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Contract Changes", FieldDisplayName = "Contract Changes", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "VCCRequests", FieldDisplayName = "VCCRequests", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Contract Changes", FieldDisplayName = "Contract Changes", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //Tab 13: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 13, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Ref ID", FieldDisplayName = "Ref ID", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "KeyRefUniqueID", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Currency", FieldDisplayName = "Currency", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "Currency", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldDisplayName = "Contract Value", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ContractValue", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Contract Owner", FieldDisplayName = "Contract Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FinancialManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Signing Date", FieldDisplayName = "Signing Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ContractSigningDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Effective Start Date", FieldDisplayName = "Effective Start Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "EffectiveStartDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Effective End Date", FieldDisplayName = "Effective End Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "EffectiveEndDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination By Client", FieldDisplayName = "Termination By Client", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination For Convenience", FieldDisplayName = "Termination For Convenience", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TerminationByClient", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Notice Period", FieldDisplayName = "Termination Notice Period", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TerminationNoticePeriod", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Trigger(s)", FieldDisplayName = "Termination Trigger(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ClientTerminationTriggers", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination for Force Majeure Event", FieldDisplayName = "Termination for Force Majeure Event", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "TerminationbyClientForceMajeureEvent", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination for Incurred Liability", FieldDisplayName = "Termination for Incurred Liability", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TerminationForIncurredLiability", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Additional Comments", FieldDisplayName = "Additional Comments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "AdditionalComments", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination By Client", FieldDisplayName = "Termination By Client", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination By Vendor", FieldDisplayName = "Termination By Vendor", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination By Vendor", FieldDisplayName = "Termination By Vendor", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "TerminationByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Notice Period", FieldDisplayName = "Termination Notice Period", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TerminationNoticePeriodByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Trigger", FieldDisplayName = "Termination Trigger", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "TerminationTriggerByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Termination Charges", FieldDisplayName = "Termination Charges", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TerminationChargesByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Additional Comments", FieldDisplayName = "Additional Comments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "AdditionalCommentsByVendor", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Termination By Vendor", FieldDisplayName = "Termination By Vendor", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Disentanglement", FieldDisplayName = "Disentanglement", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Disentanglement Term (months)", FieldDisplayName = "Disentanglement Term (months)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DisentanglementTerm", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Disentanglement Transition Plan (days)", FieldDisplayName = "Disentanglement Transition Plan (days)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DisentanglementTransitionPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Disentanglement", FieldDisplayName = "Disentanglement", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Legal", FieldDisplayName = "Legal", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Jurisdiction", FieldDisplayName = "Jurisdiction", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LegalJurisdiction", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Compliance List", FieldDisplayName = "Compliance List", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LegalCompliances", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Publicity Allowed?", FieldDisplayName = "Publicity Allowed?", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LegalPublicity", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Warranties & Representations", FieldDisplayName = "Warranties & Representations", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "LegalWarranties", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Legal", FieldDisplayName = "Legal", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Financial Payment Terms", FieldDisplayName = "Financial Payment Terms", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Payment Due (in days)", FieldDisplayName = "Payment Due (in days)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PaymentDueTerm", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Payment Delay Interest", FieldDisplayName = "Payment Delay Interest", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "PaymentDelayInterest", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Other Payment Terms", FieldDisplayName = "Other Payment Terms", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "OtherPaymentTerms", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Financial Payment Terms", FieldDisplayName = "Financial Payment Terms", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //dataList.Add(new string[] { "Miscellaneous", ModuleNameLookup = ModuleName, "0", "3", "#GroupStart#", "1", "" });
            //dataList.Add(new string[] { "Attachments", ModuleNameLookup = ModuleName, "0", "3", "Attachments", "0", "" });
            //dataList.Add(new string[] { "Miscellaneous", ModuleNameLookup = ModuleName, "0", "3", "#GroupEnd#", "1", "" });

            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int moduleStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "KeyRefUniqueID", FieldName = "KeyRefUniqueID", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AdditionalInformation", FieldName = "AdditionalInformation", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Currency", FieldName = "Currency", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ContractValue", FieldName = "ContractValue", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ContractSigningDate", FieldName = "ContractSigningDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "EffectiveStartDate", FieldName = "EffectiveStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "EffectiveEndDate", FieldName = "EffectiveEndDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FinancialManager", FieldName = "FinancialManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TerminationByClient", FieldName = "TerminationByClient", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ClientTerminationTriggers", FieldName = "ClientTerminationTriggers", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TerminationbyClientForceMajeureEvent", FieldName = "TerminationbyClientForceMajeureEvent", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TerminationForIncurredLiability", FieldName = "TerminationForIncurredLiability", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AdditionalComments", FieldName = "AdditionalComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TerminationByVendor", FieldName = "TerminationByVendor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TerminationNoticePeriodByVendor", FieldName = "TerminationNoticePeriodByVendor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TerminationTriggerByVendor", FieldName = "TerminationTriggerByVendor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TerminationChargesByVendor", FieldName = "TerminationChargesByVendor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AdditionalCommentsByVendor", FieldName = "AdditionalCommentsByVendor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DisentanglementTerm", FieldName = "DisentanglementTerm", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DisentanglementTransitionPlan", FieldName = "DisentanglementTransitionPlan", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LegalJurisdiction", FieldName = "LegalJurisdiction", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LegalCompliances", FieldName = "LegalCompliances", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LegalPublicity", FieldName = "LegalPublicity", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LegalWarranties", FieldName = "LegalWarranties", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PaymentDueTerm", FieldName = "PaymentDueTerm", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PaymentDelayInterest", FieldName = "PaymentDelayInterest", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "OtherPaymentTerms", FieldName = "OtherPaymentTerms", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IssueResolutionProcessExists", FieldName = "IssueResolutionProcessExists", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IssueResolutionProcessSummary", FieldName = "IssueResolutionProcessSummary", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");

            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "VND", TabOrder = 1, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "VND", TabOrder = 2, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "VND", TabOrder = 3, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "VND", TabOrder = 4, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "VND", TabOrder = 5, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "VND", TabOrder = 6, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "VND", TabOrder = 7, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "VND", TabOrder = 8, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName="All Items"});
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "VND", TabOrder = 9, ModuleNameLookup = "VND", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
