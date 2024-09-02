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
    public class VCC : IModule
    {
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
        protected string ModuleName = "VCC";
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
                    ShortName = "Vendor Contract Change",
                    CategoryName = "Vendor Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "VCCRequest",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/sitePages/vcctickets",
                    ModuleHoldMaxStage = 0,
                    Title = "Vendor Contract Change (VCC)",
                    ModuleDescription = "This module is used to track vendor contract changes.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/sitePages/vcc",
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
                return "VCC";
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
            mList.Add(new ModuleFormTab() { TabName = "Documents", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Documents" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VCC", Title = "VCC - Initiator", UserTypes = "Initiator", ColumnName = "Initiator", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VCC", Title = "VCC - MGS Manager", UserTypes = "MGS Manager", ColumnName = "BusinessManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VCC", Title = "VCC - Commercial Management Lead", UserTypes = "Commercial Management Lead", ColumnName = "CommercialMgtLead", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
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
            currStageID = 0;

            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 6;
            int StageStep = 0;

            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Request for Service",
                Name = "Request for Service",
                StageTitle = "Initiated",
                UserPrompt = "Please fill the form to open a new Contract Change Request.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ActionUser = "MGS",
                ApproveActionDescription = "New Request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Re-Submit",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "",
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
                Action = "Receive & Review CR Proposal",
                Name = "Receive & Review CR Proposal",
                StageTitle = "Receive & Review CR Proposal",
                UserPrompt = "Waiting on change request and proposal.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ActionUser = "MGS",
                ApproveActionDescription = "Proposal Review",
                RejectActionDescription = "Close",
                ReturnActionDescription = "Close",
                StageApproveButtonName = "Received",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "Initiate Request",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = "Review CR Proposal"
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Proposal Complete",
                Name = "Proposal Complete",
                StageTitle = "Proposal Complete",
                UserPrompt = "<b>CML:<b> Please review the proposal for completeness and approve.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ActionUser = "MGS",
                ApproveActionDescription = "SCRB Approval",
                RejectActionDescription = "Close",
                ReturnActionDescription = "Return for CR and Proposal",
                StageApproveButtonName = "Reviewed",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "Return for CR and Proposal",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 25,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = "Review CR Proposal"
            });


            mList.Add(new LifeCycleStage()
            {
                Action = "SCRB Approval",
                Name = "SCRB Approval",
                StageTitle = "SCRB Approval",
                UserPrompt = "Request is pending SCRB Approval",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ActionUser = "CommercialMgtLead",
                ApproveActionDescription = "SCRB Approval",
                RejectActionDescription = "Close",
                ReturnActionDescription = "Return for  Proposal Review",
                StageApproveButtonName = "Approved",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "Return for Proposal Review",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 25,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = "Review CR Proposal"
            });


            mList.Add(new LifeCycleStage()
            {
                Action = "Execute Amendment",
                Name = "Execute Amendment",
                StageTitle = "Execute Amendment",
                UserPrompt = "Please implement the changes in the contact",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ActionUser = "MGS",
                ApproveActionDescription = "Execute Amendment",
                RejectActionDescription = "Close",
                ReturnActionDescription = "Return for  Proposal Review",
                StageApproveButtonName = "Executed",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "Return for Proposal Review",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 25,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = "Review CR Proposal"
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "Change Request is complete",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ActionUser = "MGS",
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "Re-Open",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 1,
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
            //VCC
            int seqNum = 0;
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "TicketId", FieldDisplayName = "Request ID", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "MiniView=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "VendorMSALookup", FieldDisplayName = "MSA ID", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "MiniView=true", DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "MiniView=true", DisplayForClosed = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "RequestTypeLookup", FieldDisplayName = "Change Type", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "MGSSubmissionDate", FieldDisplayName = "MGS Submission", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "MiniView=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "RFSSubmitted", FieldDisplayName = "RFS Submitted", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "RFSSubmissionDate", FieldDisplayName = "RFS Submission", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "StageStep", FieldDisplayName = "Status", CustomProperties = "MiniView=true", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "NextSLATime", FieldDisplayName = "Step Due", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "SLADate", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "RequestorName", FieldDisplayName = "Requestor Name", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "RequestorOrganization", FieldDisplayName = "Requestor Org", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "RFSFormComplete", FieldDisplayName = "RFS Form Complete", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "TargetCompletionDate", FieldDisplayName = "Proposal Date", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "DescriptionofService", FieldDisplayName = "Description of Service", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "SubmissionDate", FieldDisplayName = "CR Submission", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "FinancialImpactAmount", FieldDisplayName = "Financial Impact", ColumnType = "Currency", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "SLAImpact", FieldDisplayName = "SLA Impact", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "Description", FieldDisplayName = " Description", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "CRSignedDate", FieldDisplayName = "CR Signed Date", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "AmendmentReceived", FieldDisplayName = "Amendment Received", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "AmendmentEffectiveDate", FieldDisplayName = "Amendment Effective Date", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "AmendmentId", FieldDisplayName = "Amendment #", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "DeliverableImpact", FieldDisplayName = "Deliverable Impact", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "CommercialMgtLead", FieldDisplayName = "Commercial Mgt Lead", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VCC", FieldName = "OnHold", FieldDisplayName = "OnHold", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
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
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Request For Service", Title = "Request For Service", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "MSA/SOW", Title = "MSA/SOW", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "CR #", Title = "CR #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "CRNumber", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "MGS Submitted Date", Title = "MGS Submitted Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "MGSSubmittedDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "RFS Submitted?", Title = "RFS Submitted?", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RFSSubmitted", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "RFS Submission Date", Title = "RFS Submission Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RFSSubmissionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "POC", Title = "POC", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestorName", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requestor Organization", Title = "Requestor Organization", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestorOrganization", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Commercial Mgt Lead", Title = "Commercial Mgt Lead", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CommercialMgtLead", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description of Service", Title = "Description of Service", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "DescriptionofService", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Request For Service", Title = "Request For Service", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Change Request", Title = "Change Request", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Proposal Submission Date", Title = "Proposal Submission Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SubmissionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Change Type", Title = "Change Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "SCRB Date", Title = "SCRB Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SCRBDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Proposed Cost", Title = "Proposed Cost", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FinancialImpactAmount", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "SLA Impact", Title = "SLA Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "SLAImpact", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description of Change", Title = "Description of Change", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "CR Signed Date", Title = "CR Signed Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CRSignedDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Amendment Received", Title = "Amendment Received", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AmendmentReceived", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Amendment Effective Date", Title = "Amendment Effective Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AmendmentEffectiveDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Amendment #", Title = "Amendment #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AmendmentId", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Deliverable Impact", Title = "Deliverable Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DeliverableImpact", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Change Request", Title = "Change Request", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Document Upload", Title = "Document Upload", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ModuleDocumentGridview", Title = "ModuleDocumentGridview", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Document Upload", Title = "Document Upload", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 2: Documents 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Documents", Title = "Documents", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "DocumentControl", Title = "DocumentControl", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Documents", Title = "Documents", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 3: Lifecycle 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Lifecycle", Title = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ApprovalTab", Title = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Lifecycle", Title = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 4: Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments", Title = "Comments", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Add Comment", Title = "Add Comment", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments", Title = "Comments", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            //Tab 5: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Request For Service", Title = "Request For Service", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "MSA/SOW", Title = "MSA/SOW", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "MGS Submitted Date", Title = "MGS Submitted Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "MGSSubmittedDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "RFS Submitted?", Title = "RFS Submitted?", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RFSSubmitted", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "RFS Submission Date", Title = "RFS Submission Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RFSSubmissionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "POC", Title = "POC", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestorName", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requestor Organization", Title = "Requestor Organization", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestorOrganization", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Commercial Mgt Lead", Title = "Commercial Mgt Lead", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "CommercialMgtLead", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description of Service", Title = "Description of Service", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "DescriptionofService", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Request For Service", Title = "Request For Service", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });


            string currTab = string.Empty;

            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();

            // Console.WriteLine("  RequestType");

            mList.Add(new ModuleRequestType() { Title = "MSA Change", RequestType = "MSA Change", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "VCC Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "SOW Change", RequestType = "SOW Change", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "VCC Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "New SOW", RequestType = "New SOW", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "VCC Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "SLA Change", RequestType = "SLA Change", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "VCC Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Pricing Change", RequestType = "Pricing Change", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "VCC Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Service Addition", RequestType = "Service Addition", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "VCC Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Service Deletion", RequestType = "Service Deletion", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "VCC Type", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });

            return mList;

        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int StageStep = 0;

            // All stages
            //dataList.Add(new string[] { "Attachments", ModuleNameLookup = ModuleName, StageStep.ToString(), "0", "0", "", "" });

            StageStep = 1; // Initiated
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmitted", FieldName = "RFSSubmitted", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmissionDate", FieldName = "RFSSubmissionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorName", FieldName = "RequestorName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorOrganization", FieldName = "RequestorOrganization", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "MGSSubmittedDate", FieldName = "MGSSubmittedDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "DescriptionofService", FieldName = "DescriptionofService", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CommercialMgtLead", FieldName = "CommercialMgtLead", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CRNumber", FieldName = "CRNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialImpactAmount", FieldName = "FinancialImpactAmount", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SCRBDate", FieldName = "SCRBDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SLAImpact", FieldName = "SLAImpact", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ModuleDocumentGridview", FieldName = "ModuleDocumentGridview", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            StageStep = 2; // Receive CR & Proposal
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmitted", FieldName = "RFSSubmitted", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmissionDate", FieldName = "RFSSubmissionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorName", FieldName = "RequestorName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorOrganization", FieldName = "RequestorOrganization", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "MGSSubmittedDate", FieldName = "MGSSubmittedDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "DescriptionofService", FieldName = "DescriptionofService", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CommercialMgtLead", FieldName = "CommercialMgtLead", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SubmissionDate", FieldName = "SubmissionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialImpactAmount", FieldName = "FinancialImpactAmount", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SLAImpact", FieldName = "SLAImpact", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CRNumber", FieldName = "CRNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SCRBDate", FieldName = "SCRBDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ModuleDocumentGridview", FieldName = "ModuleDocumentGridview", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            StageStep = 3; // Proposal Review
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmitted", FieldName = "RFSSubmitted", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmissionDate", FieldName = "RFSSubmissionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorName", FieldName = "RequestorName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorOrganization", FieldName = "RequestorOrganization", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "MGSSubmittedDate", FieldName = "MGSSubmittedDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "DescriptionofService", FieldName = "DescriptionofService", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CommercialMgtLead", FieldName = "CommercialMgtLead", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SubmissionDate", FieldName = "SubmissionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialImpactAmount", FieldName = "FinancialImpactAmount", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SLAImpact", FieldName = "SLAImpact", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CRNumber", FieldName = "CRNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SCRBDate", FieldName = "SCRBDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ModuleDocumentGridview", FieldName = "ModuleDocumentGridview", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            StageStep = 4; // SCRB Approval
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmitted", FieldName = "RFSSubmitted", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSSubmissionDate", FieldName = "RFSSubmissionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorName", FieldName = "RequestorName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestorOrganization", FieldName = "RequestorOrganization", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RFSFormComplete", FieldName = "RFSFormComplete", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "MGSSubmittedDate", FieldName = "MGSSubmittedDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "DescriptionofService", FieldName = "DescriptionofService", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CommercialMgtLead", FieldName = "CommercialMgtLead", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SCRBDate", FieldName = "SCRBDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SubmissionDate", FieldName = "SubmissionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialImpactAmount", FieldName = "FinancialImpactAmount", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SLAImpact", FieldName = "SLAImpact", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CRNumber", FieldName = "CRNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SCRBDate", FieldName = "SCRBDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ModuleDocumentGridview", FieldName = "ModuleDocumentGridview", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            StageStep = 5; // Excecute Amendment
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CRSignedDate", FieldName = "CRSignedDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmendmentReceived", FieldName = "AmendmentReceived", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmendmentEffectiveDate", FieldName = "AmendmentEffectiveDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmendmentId", FieldName = "AmendmentId", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "DeliverableImpact", FieldName = "DeliverableImpact", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ModuleDocumentGridview", FieldName = "ModuleDocumentGridview", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "CRNumber", FieldName = "CRNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SCRBDate", FieldName = "SCRBDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ActionUser = "", CustomProperties = "", ShowWithCheckbox = false });

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
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "VCC", TabOrder = 1, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "VCC", TabOrder = 2, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "VCC", TabOrder = 3, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "VCC", TabOrder = 4, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "VCC", TabOrder = 5, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "VCC", TabOrder = 6, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "VCC", TabOrder = 7, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "VCC", TabOrder = 8, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "VCC", TabOrder = 9, ModuleNameLookup = "VCC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
