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
    public class VFM : IModule
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
        protected string ModuleName = "VFM";
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
                    ShortName = "Vendor Invoice Management",
                    CategoryName = "Vendor Invoice Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "VendorSOWInvoices",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/vfmtickets" + ModuleName,
                    ModuleHoldMaxStage = 0,
                    Title = "Vendor Finance Management (VFM)",
                    ModuleDescription = "This module is used to create, approve and manage finance with external vendors for tracking contract costs, approval workflows and automated expiration reminders.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/vfm",
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
                return "VFM";
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
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }



        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VFM", Title = "VFM-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VFM", Title = "VFM-MGS Manager", UserTypes = "MGS Manager", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VFM", Title = "VFM-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VFM", Title = "VFM-Financial Manager", UserTypes = "Financial Manager", ColumnName = "FinancialManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VFM", Title = "VFM-Performance Manager", UserTypes = "Performance Manager", ColumnName = "PerformanceManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VFM", Title = "VFM-Delivery Manager", UserTypes = "Delivery Manager", ColumnName = "ServiceDeliveryManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "VFM", Title = "VFM-Functional Manager", UserTypes = "Functional Manager", ColumnName = "FunctionalManager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
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
            // Start from ID 66
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 7;
            int StageStep = 0;

            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Create new Ticket",
                UserPrompt = "<b>Please fill the form to submit a new invoice.</b>",
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
                Action = "Enter SLA Performance Numbers",
                Name = "Data Entry",
                StageTitle = "Create new Ticket",
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
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Awaiting MGS Manager's approval",
                Name = "Manager Review",
                StageTitle = "Manager Review",
                UserPrompt = "<b>MGS Manager:</b> Please approve the Invoice.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "MGS Manager Approval",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "MGS Manager approved invoice",
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
                UserWorkflowStatus = "Awaiting Service Delivery Manager's approval",
                Name = "Service Delivery Approval",
                StageTitle = "Service Delivery Approval",
                UserPrompt = "<b>Service Delivery Manager:</b> Please approve the Invoice.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Service Delivery Approval",
                ActionUser = "ServiceDeliveryManager",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Service Delivery Manager approved invoice",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Awaiting Service Delivery Manager's approval",
                Name = "VMO Finance Review",
                StageTitle = "VMO Finance Review",
                UserPrompt = "<b>Finance Manager:</b> Please approve the Invoice.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Finance Approval",
                ActionUser = "FinancialManagerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Finance Manager approved request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Pay Invoice",
                Name = "Pay Invoice",
                StageTitle = "Pay Invoice",
                UserPrompt = "Pay Invoice",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Pay Invoice",
                ActionUser = "MGS",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Invoice Paid",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Invoice Paid",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 25,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                UserWorkflowStatus = "Invoice Paid",
                Name = "Invoice Paid",
                StageTitle = "Invoice Paid",
                UserPrompt = "Invoice Rejected",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Invoice Rejected",
                ActionUser = "MGS",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
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
            // VFM
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "TicketId", FieldDisplayName = "Invoice #", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "SOWInvoiceDate", FieldDisplayName = "Invoice Date", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "VendorSOWLookup", FieldDisplayName = "SOW", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "miniview=true", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "Status", FieldDisplayName = "Status", IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "StageStep", FieldDisplayName = "Status", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "SOWInvoiceAmount", FieldDisplayName = "Invoice Amount", CustomProperties = "miniview=true", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "VFM", FieldName = "NextSLATime", FieldDisplayName = "Step Due", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "SLADate", FieldSequence = ++seqNum });
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
            // Tab 1
            int seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "SOW #", FieldDisplayName = "SOW #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "PO #", FieldDisplayName = "PO #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PONumber", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Invoice #", FieldDisplayName = "Invoice #", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InvoiceNumber", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Invoice Date", FieldDisplayName = "Invoice Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SOWInvoiceDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Invoice Amount", FieldDisplayName = "Invoice Amount", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SOWInvoiceAmount", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Amount Paid", FieldDisplayName = "Amount Paid", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "AmountPaid", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Amount Disputed", FieldDisplayName = "Amount Disputed", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AmountDisputed", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Contract Owner", FieldDisplayName = "Contract Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "MGS Manager", FieldDisplayName = "MGS Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FinancialManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Invoice Items", FieldDisplayName = "Invoice Items", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Invoice Items", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "VendorFMControl", FieldDisplayName = "VendorFMControl", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Invoice Items", FieldDisplayName = "Invoice Items", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Invoice Items", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //mList.Add(new ModuleFormLayout(){ "Miscellaneous", ModuleName, "1", FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="" });
            //mList.Add(new ModuleFormLayout(){ "Attachments", ModuleName, "1", FieldDisplayWidth =3, "Attachments", "0", "" });
            //mList.Add(new ModuleFormLayout(){ "Miscellaneous", ModuleName, "1", FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="" });

            //Tab 2 - Documents
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "DocumentControl", FieldDisplayName = "DocumentControl", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 3: Lifecycle 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 4: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "SOW #", FieldDisplayName = "SOW #", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "VendorSOWLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "PO #", FieldDisplayName = "PO #", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PONumber", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Invoice #", FieldDisplayName = "Invoice #", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "InvoiceNumber", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Invoice Date", FieldDisplayName = "Invoice Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SOWInvoiceDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Invoice Amount", FieldDisplayName = "Invoice Amount", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SOWInvoiceAmount", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Contract Owner", FieldDisplayName = "Contract Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "MGS Manager", FieldDisplayName = "MGS Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Financial Manager", FieldDisplayName = "Financial Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FinancialManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Performance Manager", FieldDisplayName = "Performance Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PerformanceManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Service Delivery Manager", FieldDisplayName = "Service Delivery Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ServiceDeliveryManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Functional Manager", FieldDisplayName = "Functional Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FunctionalManager", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
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
            // All stages
            int StageStep = 0;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "IsPrivate", FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 1;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SOWInvoiceDate", FieldName = "SOWInvoiceDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SOWInvoiceAmount", FieldName = "SOWInvoiceAmount", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialManager", FieldName = "FinancialManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "InvoiceNumber", FieldName = "InvoiceNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PONumber", FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 2;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SOWInvoiceDate", FieldName = "SOWInvoiceDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SOWInvoiceAmount", FieldName = "SOWInvoiceAmount", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialManager", FieldName = "FinancialManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "InvoiceNumber", FieldName = "InvoiceNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PONumber", FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountPaid", FieldName = "AmountPaid", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountDisputed", FieldName = "AmountDisputed", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 3;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SOWInvoiceDate", FieldName = "SOWInvoiceDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "SOWInvoiceAmount", FieldName = "SOWInvoiceAmount", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "VendorSOWLookup", FieldName = "VendorSOWLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FinancialManager", FieldName = "FinancialManagerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PerformanceManager", FieldName = "PerformanceManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ServiceDeliveryManager", FieldName = "ServiceDeliveryManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FunctionalManager", FieldName = "FunctionalManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "InvoiceNumber", FieldName = "InvoiceNumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PONumber", FieldName = "PONumber", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountPaid", FieldName = "AmountPaid", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountDisputed", FieldName = "AmountDisputed", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 4;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountPaid", FieldName = "AmountPaid", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountDisputed", FieldName = "AmountDisputed", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 5;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountPaid", FieldName = "AmountPaid", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountDisputed", FieldName = "AmountDisputed", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 6;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountPaid", FieldName = "AmountPaid", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountDisputed", FieldName = "AmountDisputed", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 7;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountPaid", FieldName = "AmountPaid", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AmountDisputed", FieldName = "AmountDisputed", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
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
                EmailTitle = "New Invoice Pending Data Entry [$TicketId$]: [$Title$]",
                EmailBody = @"Invoice ID [$TicketId$]: [$Title$] is pending data entry by MGS.<br><br>" +
                                                        "Please complete data entry and submit for approval.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Manager Review",
                Status = "Manager Review",
                EmailTitle = "Invoice Pending Manager Review [$TicketId$]: [$Title$]",
                EmailBody = @"Invoice ID [$TicketId$]: [$Title$] is pending MGS Manager Review.<br><br>" +
                                                        "Please approve or reject the invoice.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Service Delivery Approval",
                Status = "Service Delivery Approval",
                EmailTitle = "Invoice Pending Service Delivery Approval [$TicketId$]: [$Title$]",
                EmailBody = @"Invoice ID [$TicketId$]: [$Title$] is pending approval by Service Delivery Manager.<br><br>" +
                                                        "Please approve or reject the invoice.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "VMO Finance Review",
                Status = "VMO Finance Review",
                EmailTitle = "Invoice Pending VMO Finance Review [$TicketId$]: [$Title$]",
                EmailBody = @"Invoice [$TicketId$]: [$Title$] is pending VMO Finance Review.<br><br>" +
                                                        "Please approve or reject the invoice.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Pay Invoice",
                Status = "Pay Invoice",
                EmailTitle = "Invoice Pending Payment [$TicketId$]: [$Title$]",
                EmailBody = @"Invoice ID [$TicketId$]: [$Title$] has been approved for payment.<br><br>" +
                                                        "Please pay the invoice and click on Invoice Paid.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Invoice Paid", Status = "Invoice Paid", EmailTitle = "Invoice Paid [$TicketId$]: [$Title$]", EmailBody = "Invoice ID [$TicketId$]: [$Title$] has been paid.", ModuleNameLookup = ModuleName, StageStep = (stageID + 1) });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "On-Hold", Status = "On-Hold", EmailTitle = "Invoice On Hold [$TicketId$]: [$Title$]", EmailBody = "Invoice ID [$TicketId$]: [$Title$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Unassigned", ViewName = "VFM", TabOrder = 1, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "VFM", TabOrder = 2, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "VFM", TabOrder = 3, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "VFM", TabOrder = 4, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "VFM", TabOrder = 5, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "VFM", TabOrder = 6, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "VFM", TabOrder = 7, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "VFM", TabOrder = 8, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "VFM", TabOrder = 9, ModuleNameLookup = "VFM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
