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
    public class VPM : IModule
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
        protected string ModuleName = "VPM";
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
                    ShortName = "Vendor Performance Management",
                    CategoryName = "Vendor Performance Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "VendorVPM",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/vpmtickets",
                    ModuleHoldMaxStage = 0,
                    Title = "Vendor Performance Management (VPM)",
                    ModuleDescription = "This module is used to create, approve and manage Performance with external vendors for tracking contract costs, approval workflows.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/sitePages/vfm",
                    ModuleType = ModuleType.SMS,
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
                return "VPM";
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
            mList.Add(new ModuleFormTab() { TabName = "SLA Performance", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "SLA Performance" });
            mList.Add(new ModuleFormTab() { TabName = "Documents", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Documents" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VPM", Title = "VPM-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VPM", Title = "VPM-MGS Manager", UserTypes = "MGS Manager", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VPM", Title = "VPM-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VPM", Title = "VPM-Financial Manager", UserTypes = "Financial Manager", ColumnName = "FinancialManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VPM", Title = "VPM-Performance Manager", UserTypes = "Performance Manager", ColumnName = "PerformanceManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VPM", Title = "VPM-Delivery Manager", UserTypes = "Delivery Manager", ColumnName = "ServiceDeliveryManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VPM", Title = "VPM-Functional Manager", UserTypes = "Functional Manager", ColumnName = "FunctionalManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            // Start from ID 66
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 16;
            int StageStep = 0;

            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Create new Ticket",
                UserPrompt = "<b>Please fill the form to open a new MSA Request.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "MSA Submitted",
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
                Action = "Data Entry",
                Name = "Data Entry",
                StageTitle = "Data Entry",
                UserPrompt = "<b>MGS Analyst:</b> Please enter SLA performance numbers.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Data Entry Done",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Cancel",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Awaiting MGS Manager's approval",
                Action = "Manager Review",
                Name = "Manager Review",
                StageTitle = "Manager Review",
                UserPrompt = "<b>MGS Manager:</b> Please approve the performance report.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "BusinessManagerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Manager approved request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Attach Performance Report",
                Name = "Attach Performance Report",
                StageTitle = "Attach Performance Report",
                UserPrompt = "<b>Performance Manager:</b> Please Attach Report.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Attach Performance Report",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Attach Performance Report",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Service Delivery Approval",
                Name = "Service Delivery Approval",
                StageTitle = "Service Delivery Approval",
                UserPrompt = "<b>Service Delivery Manager:</b> Please approve the report.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Service Delivery Approval",
                ActionUser = "ServiceDeliveryManager",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Attach Performance Report",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "VMO Approval",
                Name = "VMO Approval",
                StageTitle = "VMO Approval",
                UserPrompt = "<b>Performance Manager</b>: Please approve the report",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "VMO Approval",
                ActionUser = "PerformanceManager",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "VMO Approval",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Internal Meeting Report",
                Name = "Internal Meeting Report",
                StageTitle = "Internal Meeting Report",
                UserPrompt = "Attach Internal Meeting Report",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Internal Meeting Report",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "VMO Approval",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });


            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Internal Meeting Report",
                Name = "Internal Meeting Report",
                StageTitle = "Internal Meeting Report",
                UserPrompt = "Attach Internal Meeting Report",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Internal Meeting Report",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Attach Joint Meeting Report",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Report Attached",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Identifies Performance Issues",
                Name = "Identifies Performance Issues",
                StageTitle = "Identifies Performance Issues",
                UserPrompt = "Attach Internal Meeting Report",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Identifies Performance Issues",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Identifies Performance Issues",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Report Attached",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Vendor Acceptance",
                Name = "Vendor Acceptance",
                StageTitle = "Vendor Acceptance",
                UserPrompt = "Vendor Acceptance",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Vendor Acceptance",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Vendor Acceptance",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Report Attached",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RootCauseAnalysisNeeded] = false",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Report Received",
                Name = "Report Received",
                StageTitle = "Report Received",
                UserPrompt = "Report Received",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Report Received",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Report Received",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Report Received",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RootCauseAnalysisNeeded] = false",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Upload Report",
                Name = "Upload Report",
                StageTitle = "Upload Report",
                UserPrompt = "Upload Report",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Upload Report",
                ActionUser = "",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Upload Report",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Upload Report",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RootCauseAnalysisNeeded] = false",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Report Joint Review",
                Name = "Report Joint Review",
                StageTitle = "Report Joint Review",
                UserPrompt = "Report Joint Review",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Upload Report",
                ActionUser = "",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Report Joint Review",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Reviewed",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RootCauseAnalysisNeeded] = false",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Vendor Reponsibility",
                Name = "Vendor Reponsibility",
                StageTitle = "Vendor Reponsibility",
                UserPrompt = "Vendor Reponsibility",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Vendor Reponsibility",
                ActionUser = "",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Vendor Reponsibility",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Vendor Reponsibility",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RootCauseAnalysisNeeded] = false",
                StageTypeChoice = ""
            });


            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Vendor Feedback",
                Name = "Vendor Feedback",
                StageTitle = "Vendor Feedback",
                UserPrompt = "Vendor Feedback",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Vendor Feedback",
                ActionUser = "",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Vendor Feedback",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Vendor Feedback",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RootCauseAnalysisNeeded] = false",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "MSA Close",
                Name = "Closed",
                StageTitle = "MSA Close",
                UserPrompt = "MSA Close",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "MSA Close",
                ActionUser = "OwnerUser",
                StageApprovedStatus = 0,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-open",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[RootCauseAnalysisNeeded] = false",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });

            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // VPM
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "TicketId", FieldDisplayName = "Report #", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "VendorSLAReportingDate", FieldDisplayName = "Reporting Date", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview = true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview = true", DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "VendorSOWLookup", FieldDisplayName = "SOW", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview = true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "Status", FieldDisplayName = "Status", IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "StageStep", FieldDisplayName = "Status", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "SLAsMissed", FieldDisplayName = "SLAs Missed", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview = true", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VPM", FieldName = "NextSLATime", FieldDisplayName = "Step Due", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "SLADate", FieldSequence = ++seqNum });
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");
            int seqNum = 0;
            // Tab 1
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "SOW #", FieldDisplayName = "SOW #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Reporting Date", FieldDisplayName = "Reporting Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "VendorSLAReportingDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "# of SLAs Missed", FieldDisplayName = "# of SLAs Missed", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "SLAsMissed", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Contract Owner", FieldDisplayName = "Contract Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "MGS Manager", FieldDisplayName = "MGS Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FinancialManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });


            //Tab 2: SLA Performance
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "SLA Performance", FieldDisplayName = "SLA Performance", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "SLA Performance", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "VendorPMControl", FieldDisplayName = "VendorPMControl", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "SLA Performance", FieldDisplayName = "SLA Performance", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "SLA Performance", FieldSequence = (++seqNum) });

            //Tab 3 - Documents
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "DocumentControl", FieldDisplayName = "DocumentControl", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 4: Lifecycle 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 5: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History" });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Reporting Date", FieldDisplayName = "Reporting Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "VendorSLAReportingDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "SOW", FieldDisplayName = "SOW", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Contract Owner", FieldDisplayName = "Contract Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "MGS Manager", FieldDisplayName = "MGS Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FinancialManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>(); // Console.WriteLine("  RequestType");
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int StageStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "IsPrivate", FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 1;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLALookup", FieldName = "VendorSLALookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLAReportingDate", FieldName = "VendorSLAReportingDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialManager", FieldName = "FinancialManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 2;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLALookup", FieldName = "VendorSLALookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLAReportingDate", FieldName = "VendorSLAReportingDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialManager", FieldName = "FinancialManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 3;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLALookup", FieldName = "VendorSLALookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLAReportingDate", FieldName = "VendorSLAReportingDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialManager", FieldName = "FinancialManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 4;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLALookup", FieldName = "VendorSLALookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSLAReportingDate", FieldName = "VendorSLAReportingDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialManager", FieldName = "FinancialManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RootCauseAnalysisNeeded", FieldName = "RootCauseAnalysisNeeded", ModuleNameLookup = ModuleName, StageStep = 9, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorAcceptsFailure", FieldName = "VendorAcceptsFailure", ModuleNameLookup = ModuleName, StageStep = 10, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });


            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SyncToRequestType", FieldName = "SyncToRequestType", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SyncAtModuleLevel", FieldName = "SyncAtModuleLevel", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Data Entry",
                Status = "Data Entry",
                EmailTitle = "New Performance Report Pending Data Entry [$TicketId$]: [$Title$]",
                EmailBody = @"Performance Report ID [$TicketId$]: [$Title$] is pending data entry by MGS.<br><br>" +
                                                        "Please complete data entry and submit for approval.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Manager Review",
                Status = "Manager Review",
                EmailTitle = "Performance Report Pending Manager Review [$TicketId$]: [$Title$]",
                EmailBody = @"Performance Report ID [$TicketId$]: [$Title$] is pending MGS Manager Review.<br><br>" +
                                                        "Please approve or reject the performance report.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Attach Performance Report",
                Status = "Attach Performance Report",
                EmailTitle = "Performance Analysis Report Pending for [$TicketId$]: [$Title$]",
                EmailBody = @"[$TicketId$]: [$Title$] is pending attachment of Performance Report.<br><br>" +
                                                        "Please review and attach the performance report.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Service Delivery Approval",
                Status = "Service Delivery Approval",
                EmailTitle = "Performance Report Pending Service Delivery Approval [$TicketId$]: [$Title$]",
                EmailBody = @"Performance Report ID [$TicketId$]: [$Title$] is pending approval by Service Delivery Manager.<br><br>" +
                                                        "Please approve or reject the performance report.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "VMO Approval",
                Status = "VMO Approval",
                EmailTitle = "Performance Report Pending VMO Approval [$TicketId$]: [$Title$]",
                EmailBody = @"Performance Report [$TicketId$]: [$Title$] is pending VMO Approval.<br><br>" +
                                                        "Please approve or reject the performance report.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Internal Meeting Report",
                Status = "Internal Meeting Report",
                EmailTitle = "Performance Report Pending Internal Meeting Report [$TicketId$]: [$Title$]",
                EmailBody = @"Performance Report ID [$TicketId$]: [$Title$] is pending attachment of internal meeting report.<br><br>" +
                                                        "Please attach the internal meeting report and click on Report Attached.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Report Joint Review", Status = "Report Joint Review", EmailTitle = "Performance Report Pending Joint Review [$TicketId$]: [$Title$]", EmailBody = "Performance Report ID [$TicketId$]: [$Title$] is pending joint review.", ModuleNameLookup = ModuleName, StageStep = (stageID + 6) });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "On-Hold", Status = "On-Hold", EmailTitle = "Performance Report On Hold [$TicketId$]: [$Title$]", EmailBody = "Performance Report ID [$TicketId$]: [$Title$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "VPM", TabOrder = 1, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "VPM", TabOrder = 2, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "VPM", TabOrder = 3, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "VPM", TabOrder = 4, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "VPM", TabOrder = 5, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "VPM", TabOrder = 6, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "VPM", TabOrder = 7, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "VPM", TabOrder = 8, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "VPM", TabOrder = 9, ModuleNameLookup = "VPM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
