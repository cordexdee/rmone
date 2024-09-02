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
    public class CMT : IModule
    {
        protected static bool enableOwnerApprovalStage = true;
        protected static bool enableFinanceApprovalStage = true;
        protected static bool enableLegalApprovalStage = true;
        protected static bool enablePurchasingStage = true;
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

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Contract Management",
                    CategoryName = "Resource Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "Contracts",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/contracts",
                    ModuleHoldMaxStage = 0,
                    Title = "Contract Management (CMT)",
                    ModuleDescription = "This module is used to create, approve and manage contracts with external vendors for tracking contract costs, approval workflows and automated expiration reminders.",
                    //ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details.  Or, select a ticket by clicking checkbox and clicking Actions button.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/CMT",
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    AllowDelete = true,
                    UseInGlobalSearch = true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "CMT";
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
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });

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
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "CMT", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "CMT", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "CMT", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "CMT", Title = "CMT-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "CMT", Title = "CMT-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "CMT", Title = "CMT-Finance Manager", UserTypes = "Finance Manager", ColumnName = "FinanceManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "CMT", Title = "CMT-Legal", UserTypes = "Legal", ColumnName = "LegalUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "CMT", Title = "CMT-Purchasing", UserTypes = "Purchasing", ColumnName = "PurchasingUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
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
            int numStages = 8;
            int moduleStep = 0;
            if (!enableOwnerApprovalStage)
                numStages--;
            if (!enableFinanceApprovalStage)
                numStages--;
            if (!enableLegalApprovalStage)
                numStages--;
            if (!enablePurchasingStage)
                numStages--;

            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Initiated",
                UserPrompt = "<b>Please fill the form to open a new Contract Request.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ActionUser = "Initiator",
                ApproveActionDescription = "Ticket Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "(Re)Submit",
                StageRejectedButtonName = "Cancel",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "QuickClose=true",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Owner Approval",
                Name = "Owner Approval",
                StageTitle = "Manager Approval",
                ActionUser = "BusinessManager",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Owner approved request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RequestTypeWorkflow] = 'SkipApprovals'",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Finance Approval",
                Name = "Finance Approval",
                StageTitle = "Finance Approval",
                ActionUser = "SecurityManager",
                UserPrompt = "<b>Finance Manager:</b> Please approve the Contract.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Finance Manager approved request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[NeedReview] <> 'Yes'",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Legal Approval",
                Name = "Legal Approval",
                StageTitle = "Legal Approval",
                ActionUser = "LegalManager",
                UserPrompt = "<b>Legal:</b> Please approve the Contract.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Legal approved request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[NeedReview] <> 'Yes'",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Purchasing",
                Name = "Purchasing",
                StageTitle = "Awaiting Purchasing approval",
                ActionUser = "LegalManager",
                UserPrompt = "<b>Legal:</b> Please approve the Contract.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Purchasing approved request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[NeedReview] <> 'Yes'",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Active",
                Name = "Active",
                StageTitle = "Contract Active",
                ActionUser = "Owner",
                UserPrompt = "Contract Active",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Contract Expired",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Expire Now",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 45,
                ShortStageTitle = "",
                CustomProperties = "scheduledautostage=true#scheduledtriggerfieldname=ContractExpirationDate",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Expired",
                Name = "Expired",
                StageTitle = "Contract Expired",
                ActionUser = "Owner",
                UserPrompt = "Contract is expired, but you add comments or re-new by Owner",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Owner Closed Contract",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Close",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Renew",
                StageTypeLookup = 3,
                StageAllApprovalsRequired = false,
                StageWeight = 15,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Closed",
                StageTitle = "Contract Closed",
                ActionUser = "Owner",
                UserPrompt = "Contract is expired, but you add comments ",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Owner Closed Contract",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-open",
                StageTypeLookup = 3,
                StageAllApprovalsRequired = false,
                StageWeight = 0,
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
            // CMT
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "TicketId", FieldDisplayName = "Request ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "VendorLookup", FieldDisplayName = "Vendor", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 2, IsAscending = true, FieldSequence = ++seqNum }); // Sort 2
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "RequestTypeLookup", FieldDisplayName = "Contract Type", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "OnHold", FieldDisplayName = "OnHold", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "ContractStartDate", FieldDisplayName = "Contract Start", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "ContractExpirationDate", FieldDisplayName = "Expiration", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "CloseDate", FieldDisplayName = "Closed On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "CMT", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            int startStageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "NeedReview",
                KeyName = "NeedReview",
                KeyValue = "Yes",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "RepeatInterval",
                KeyName = "RepeatInterval",
                KeyValue = "None",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");

            int seqNum = 0;

            // Tab 1
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Title") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Status", FieldDisplayName = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Status") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Initiator", FieldDisplayName = "Initiator", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InitiatorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Initiator") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Created", FieldDisplayName = "Created", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "CreationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("CreationDate") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Owner") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Need Review?", FieldDisplayName = "Need Review?", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "NeedReview", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("NeedReview") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Desired Approval Date", FieldDisplayName = "Desired Approval Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", SkipOnCondition = "[NeedReview] <> 'Yes'", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("DesiredCompletionDate") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Finance Manager", FieldDisplayName = "Finance Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FinanceManagerUser", ShowInMobile = true, CustomProperties = "", SkipOnCondition = "[NeedReview] <> 'Yes'", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("FinanceManager") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Legal Manager", FieldDisplayName = "Legal Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LegalUser", ShowInMobile = true, CustomProperties = "", SkipOnCondition = "[NeedReview] <> 'Yes'", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Legal") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Purchasing", FieldDisplayName = "Purchasing", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PurchasingUser", ShowInMobile = true, CustomProperties = "", SkipOnCondition = "[NeedReview] <> 'Yes'", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Purchasing") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Description") ? true : false, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Contract Details", FieldDisplayName = "Contract Details", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Contract Type", FieldDisplayName = "Contract Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("RequestTypeLookup") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Vendor", FieldDisplayName = "Vendor", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "VendorLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("VendorLookup") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Term Type", FieldDisplayName = "Term Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TermType", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("TermType") ? true : false }); //Annual, monthly, adhoc

            mList.Add(new ModuleFormLayout() { Title = "Initial Cost $", FieldDisplayName = "Initial Cost $", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InitialCost", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("InitialCost") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Annual Maintenance $", FieldDisplayName = "Annual Maintenance $", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AnnualMaintenanceCost", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("AnnualMaintenanceCost") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "License Count", FieldDisplayName = "License Count", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LicenseCount", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("LicenseCount") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Contract Start Date", FieldDisplayName = "Contract Start Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ContractStartDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ContractStartDate") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Contract Expiration Date", FieldDisplayName = "Contract Expiration Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ContractExpirationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ContractExpirationDate") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Renewal Cancel Notice Days", FieldDisplayName = "Renewal Cancel Notice Days", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RenewalCancelNoticeDays", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("RenewalCancelNoticeDays") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Reminder To", FieldDisplayName = "Reminder To", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReminderToUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderTo") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Reminder Days (before expiration)", FieldDisplayName = "Reminder Days (before expiration)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReminderDays", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderDays") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Reminder Date", FieldDisplayName = "Reminder Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReminderDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderDate") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Repeat every", FieldDisplayName = "Repeat every", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RepeatInterval", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("RepeatInterval") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Reminder Body", FieldDisplayName = "Reminder Body", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "ReminderBody", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderBody") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("IsPrivate") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "PO Number(s)", FieldDisplayName = "PO Number(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "PONumber", ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("PONumber") ? true : false });


            mList.Add(new ModuleFormLayout() { Title = "Contract Details", FieldDisplayName = "Contract Details", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            // Tab 2: Approvals 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            //Tab 3: Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "WikiRelatedTickets", FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            // Tab 4: Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Comment") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            //Tab 5
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", SkipOnCondition = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", SkipOnCondition = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Title") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Owner") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Need Review?", FieldDisplayName = "Need Review?", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "NeedReview", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("NeedReview") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Desired Approval Date", FieldDisplayName = "Desired Approval Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("DesiredCompletionDate") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Finance Manager", FieldDisplayName = "Finance Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "FinanceManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("FinanceManager") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Legal", FieldDisplayName = "Legal", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LegalUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Legal") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Purchasing", FieldDisplayName = "Purchasing", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "PurchasingUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Purchasing") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Description") ? true : false, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Contract Details", FieldDisplayName = "Contract Details", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Contract Type", FieldDisplayName = "Contract Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("RequestTypeLookup") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Vendor", FieldDisplayName = "Vendor", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "VendorLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("VendorLookup") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Term Type", FieldDisplayName = "Term Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TermType", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("TermType") ? true : false }); //Annual, monthly, adhoc

            mList.Add(new ModuleFormLayout() { Title = "Initial Cost $", FieldDisplayName = "Initial Cost $", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "InitialCost", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("InitialCost") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Annual Maintenance $", FieldDisplayName = "Annual Maintenance $", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AnnualMaintenanceCost", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("AnnualMaintenanceCost") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "License Count", FieldDisplayName = "License Count", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LicenseCount", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("LicenseCount") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Start Date", FieldDisplayName = "Start Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ContractStartDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ContractStartDate") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Expiration Date", FieldDisplayName = "Expiration Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ContractExpirationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ContractExpirationDate") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Renewal Cancel Notice Days", FieldDisplayName = "Renewal Cancel Notice Days", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RenewalCancelNoticeDays", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("RenewalCancelNoticeDays") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Reminder To", FieldDisplayName = "Reminder To", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReminderToUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderTo") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Reminder Days", FieldDisplayName = "Reminder Days", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReminderDays", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderDays") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Reminder Date", FieldDisplayName = "Reminder Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReminderDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderDate") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Repeat every", FieldDisplayName = "Repeat every", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RepeatInterval", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("RepeatInterval") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Reminder Body", FieldDisplayName = "Reminder Body", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "ReminderBody", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("ReminderBody") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("IsPrivate") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "PO Number(s)", FieldDisplayName = "PO Number(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "PONumber", ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("PONumber") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Contract Details", FieldDisplayName = "Contract Details", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Attachments") ? true : false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false });


            string currTab = string.Empty;
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            /*
            mList.Add(new ModuleRequestType() { Title = "Software", RequestType = "Software", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Contract", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Support", RequestType = "Support", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Contract", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Services", RequestType = "Services", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Contract", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Contract", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            */

            mList.Add(new ModuleRequestType() { Title = "Hardware", Category = "Contract", RequestType = "Hardware", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Other", Category = "Contract", RequestType = "Other", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Services", Category = "Contract", RequestType = "Services", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Software", Category = "Contract", RequestType = "Software", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Support", Category = "Contract", RequestType = "Support", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin" });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");

            int moduleStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });

            // Stage 1 - New 
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "NeedReview", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FinanceManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LegalUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PurchasingUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitialCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AnnualMaintenanceCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LicenseCount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderToUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderBody", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TermType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RepeatInterval", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalCancelNoticeDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });

            // Stage 2 - Owner Approval (no fields)
            if (enableOwnerApprovalStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitialCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "AnnualMaintenanceCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderToUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderBody", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "TermType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RepeatInterval", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "FinanceManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "LegalUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "PurchasingUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalCancelNoticeDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            }

            // Stage 3 - Finance Approval
            if (enableFinanceApprovalStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitialCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "AnnualMaintenanceCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderToUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderBody", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "TermType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RepeatInterval", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "FinanceManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "LegalUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "PurchasingUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalCancelNoticeDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            }

            // Stage 4 - Legal Approval
            if (enableLegalApprovalStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitialCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "AnnualMaintenanceCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderToUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderBody", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "TermType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RepeatInterval", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "FinanceManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "LegalUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "PurchasingUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalCancelNoticeDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            }

            // Stage 5 - PurchasingUser
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitialCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AnnualMaintenanceCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderToUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderBody", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TermType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RepeatInterval", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FinanceManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LegalUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PurchasingUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalCancelNoticeDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });

            // Stage 6 - Active
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitialCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AnnualMaintenanceCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderToUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderBody", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TermType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RepeatInterval", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FinanceManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LegalUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PurchasingUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalCancelNoticeDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });

            // Stage 7 - Expired
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitialCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AnnualMaintenanceCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ContractExpirationDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderToUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderBody", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TermType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RepeatInterval", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FinanceManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LegalUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PurchasingUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RenewalCancelNoticeDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReminderDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "" });

            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");

            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Initiated", ModuleNameLookup = ModuleName, StageTitleLookup = stageID });
            if (enableOwnerApprovalStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Owner Approval", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            if (enableFinanceApprovalStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Finance Approval", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            if (enableLegalApprovalStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Legal Approval", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            if (enablePurchasingStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Purchasing", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Active", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Expired", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Closed", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });

            return mList;

        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");

            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Initiated",
                EmailTitle = "Contract Returned [$TicketId$]: [$Title$]",
                EmailBody = @"Contract ID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the request.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID,
                EmailUserTypes = "Initiator"
            });

            if (enableOwnerApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Owner Approval",
                    EmailTitle = "New Contract Pending Owner Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract ID [$TicketId$] is pending Owner Approval.<br><br>" +
                                                        "Please approve or reject the request.",
                    ModuleNameLookup = ModuleName,
                    StageStep = ++stageID,
                    EmailUserTypes = "Owner"
                });
            }

            if (enableFinanceApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Finance Approval",
                    EmailTitle = "Contract Pending Finance Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract ID [$TicketId$] is pending Finance Approval.<br><br>" +
                                                        "Please approve or reject the request.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = ++stageID,
                    EmailUserTypes = "FinanceManager"
                });
            }

            if (enableLegalApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Legal Approval",
                    EmailTitle = "Contract Pending Legal Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract ID [$TicketId$] is pending Legal Approval.<br><br>" +
                                                        "Please approve or reject the request.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = ++stageID,
                    EmailUserTypes = "Legal"
                });
            }

            if (enablePurchasingStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Purchasing",
                    EmailTitle = "Contract Pending Purchasing [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract [$TicketId$] is pending purchasing.<br><br>" +
                                                        "Please enter PO number if applicable after complete",
                    ModuleNameLookup = ModuleName,
                    StageStep = ++stageID,
                    EmailUserTypes = "Purchasing"
                });
            }

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Active",
                EmailTitle = "Contract Activated [$TicketId$]: [$Title$]",
                EmailBody = @"Contract ID [$TicketId$] has been approved for activation.<br><br>",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "Owner"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Expired",
                EmailTitle = "Contract Expired [$TicketId$]: [$Title$]",
                EmailBody = @"Contract [$TicketId$] has expired.<br><br>" +
                                                        "Please renew or close the contract.",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "Owner"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Closed",
                EmailTitle = "Contract Closed [$TicketId$]: [$Title$]",
                EmailBody = "Contract ID [$TicketId$] has been closed.",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "Owner"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "On-Hold",
                EmailTitle = "Contract On Hold [$TicketId$]: [$Title$]",
                EmailBody = "Contract ID [$TicketId$] has been placed on hold.",
                ModuleNameLookup = ModuleName,
                StageStep = null,
                EmailUserTypes = "Owner"
            });

            return mList;

        }

        public List<UGITModule> GetUGITModule()
        {
            List<UGITModule> mList = new List<UGITModule>();
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Awaiting Approval", ViewName = "CMT", TabOrder = 1, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "CMT", TabOrder = 2, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Items", ViewName = "CMT", TabOrder = 3, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Items", ViewName = "CMT", TabOrder = 4, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Items", ViewName = "CMT", TabOrder = 5, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Items", ViewName = "CMT", TabOrder = 6, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "Expired", ViewName = "CMT", TabOrder = 7, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Items", ViewName = "CMT", TabOrder = 8, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Items", ViewName = "CMT", TabOrder = 9, ModuleNameLookup = "CMT", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });


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
