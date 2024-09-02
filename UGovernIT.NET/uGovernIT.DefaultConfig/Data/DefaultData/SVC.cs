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
    public class SVC : IModule
    {
        protected static bool enableManagerApprovalStage = true;
        protected static bool enableSecurityApprovalStage = true;
        protected static bool enablePendingCloseStage = true;
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
                    ShortName = "Shared Services",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "SVCRequests",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/svcrequests",
                    ModuleHoldMaxStage = 0,
                    Title = "Services (SVC)",
                    //ModuleDescription = "This module is used to create and track end-user service requests which can include several sub-tickets or tasks.",
                    ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details. Or, select a ticket by clicking checkbox and clicking Actions button.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/svc",
                    ModuleType = ModuleType.SMS,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    UseInGlobalSearch = true,
                    KeepItemOpen=true,
                    ShowBottleNeckChart=true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "SVC";
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

            mList.Add(new ModuleFormTab() { TabName = "Service", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Service" });
            mList.Add(new ModuleFormTab() { TabName = "Summary", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Summary" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });
            mList.Add(new ModuleFormTab() { TabName = "LifeCycle", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Tasks" });

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
            mList.Add(new ModuleUserType() { ModuleNameLookup = "SVC", Title = "SVC-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "SVC", Title = "SVC-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "SVC", Title = "SVC-Requestor", UserTypes = "Requestor", ColumnName = "RequestorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "SVC", Title = "SVC-Approver", UserTypes = "Approver", ColumnName = "ApproverUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            List<string[]> dataList = new List<string[]>();

            // Start from id - 61
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 11;
            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Initiated",
                UserPrompt = "<b>Service Inititated.</b>",
                UserWorkflowStatus = "Initiated Phase",
                StageStep = 1,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 5,
                StageReturnStatus = 0,
                ActionUser = "InitiatorUser",
                ApproveActionDescription = "Ticket Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "QuickClose=true",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated),


            });

            string approvalStageID = currStageID.ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Approved",
                Name = "Awaiting Approval",
                StageTitle = "Awaiting Approval",
                UserPrompt = "<b>Owner Approval: Owner please approve or reject the request</b>",
                UserWorkflowStatus = "",
                StageStep = 2,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 4,
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ActionUser = "ApproverUser;#OwnerUser;#RequestorUser;#InitiatorUser",
                ApproveActionDescription = "Owner Approved",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "Owner Approval Phase",
                CustomProperties = "CheckAssigneeToAllTask=false;#AllowEmailApproval=true",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            string assignedStageID = currStageID.ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Assigned",
                Name = "In Progress",
                StageTitle = "In Progress",
                UserPrompt = "<b>Assigned -&nbsp;Service Request will move forward when all sub-tickets & tasks are completed.</b>",
                UserWorkflowStatus = "In Progress",
                StageStep = 3,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 4,
                StageReturnStatus = 1,
                ActionUser = "OwnerUser;#ApproverUser",
                ApproveActionDescription = "Resolved",
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
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            string createIdStageID = currStageID.ToString();

            //mList.Add(new LifeCycleStage()
            //{
            //    Action = "ID Created",
            //    Name = "Create ID",
            //    StageTitle = "Create ID",
            //    UserPrompt = "<b> Complete all sub-tasks to move to next stage</b>",
            //    StageStep = 4,
            //    ModuleNameLookup = ModuleName,
            //    StageApprovedStatus = ++currStageID,
            //    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
            //    StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
            //    ActionUser = "",
            //    ApproveActionDescription = "",
            //    RejectActionDescription = "",
            //    ReturnActionDescription = "",
            //    StageApproveButtonName =  "Approve" ,
            //    StageRejectedButtonName = "Reject" ,
            //    StageReturnButtonName =   "Return" ,
            //    StageTypeLookup = 1,
            //    StageAllApprovalsRequired = false,
            //    StageWeight = 5,
            //    ShortStageTitle = "",
            //    CustomProperties = "",
            //    SkipOnCondition = "[Description] <> 'Please complete new employee on-boarding process.'",
            //    StageType = Convert.ToString(StageType.Assigned)
            //});

            //string createAccountStageID = currStageID.ToString();

            //mList.Add(new LifeCycleStage()
            //{
            //    Action = "Create account by agent",
            //    Name = "Create account",
            //    StageTitle = "Create account",
            //    UserPrompt = "<b> Complete all sub-tasks to move to next stage</b>",
            //    StageStep = 5,
            //    ModuleNameLookup = ModuleName,
            //    StageApprovedStatus = ++currStageID,
            //    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
            //    StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
            //    ActionUser = "",
            //    ApproveActionDescription = "",
            //    RejectActionDescription = "",
            //    ReturnActionDescription = "",
            //    StageApproveButtonName = "Approve",
            //    StageRejectedButtonName = "Reject",
            //    StageReturnButtonName = "Return",
            //    StageTypeLookup = 1,
            //    StageAllApprovalsRequired = false,
            //    StageWeight = 5,
            //    ShortStageTitle = "",
            //    CustomProperties = "",
            //    SkipOnCondition = "[Description] <> 'Please complete new employee on-boarding process.'",
            //    StageType = Convert.ToString(StageType.Assigned),
            //    UserWorkflowStatus= "Create account"
            //});

            //string BuySetupLaptopID = currStageID.ToString();

            //mList.Add(new LifeCycleStage()
            //{
            //    Action = "",
            //    Name = "Buy & Setup Laptop",
            //    StageTitle = "Buy & Setup Laptop",
            //    UserPrompt = "<b> Complete all sub-tasks to move to next stage</b>",
            //    StageStep = 6,
            //    ModuleNameLookup = ModuleName,
            //    StageApprovedStatus = ++currStageID,
            //    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
            //    StageReturnStatus = Convert.ToInt32(createIdStageID == "" ? "0" : createIdStageID),
            //    ActionUser = "",
            //    ApproveActionDescription = "",
            //    RejectActionDescription = "",
            //    ReturnActionDescription = "",
            //    StageApproveButtonName = "Approve",
            //    StageRejectedButtonName = "Reject",
            //    StageReturnButtonName = "Return",
            //    StageTypeLookup = 1,
            //    StageAllApprovalsRequired = false,
            //    StageWeight = 5,
            //    ShortStageTitle = "",
            //    CustomProperties = "",
            //    SkipOnCondition = "[Description] <> 'Please complete new employee on-boarding process.'",
            //});

            //string buySetupIPhoneID = currStageID.ToString();

            //mList.Add(new LifeCycleStage()
            //{
            //    Action = "",
            //    Name = "Buy & Setup iPhone",
            //    StageTitle = "Buy & Setup iPhone",
            //    UserPrompt = "<b> Complete all sub-tasks to move to next stage</b>",
            //    StageStep = 7,
            //    ModuleNameLookup = ModuleName,
            //    StageApprovedStatus = ++currStageID,
            //    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
            //    StageReturnStatus = Convert.ToInt32(createIdStageID == "" ? "0" : createIdStageID),
            //    ActionUser = "",
            //    ApproveActionDescription = "",
            //    RejectActionDescription = "",
            //    ReturnActionDescription = "",
            //    StageApproveButtonName = "Approve",
            //    StageRejectedButtonName = "Reject",
            //    StageReturnButtonName = "Return",
            //    StageTypeLookup = 1,
            //    StageAllApprovalsRequired = false,
            //    StageWeight = 5,
            //    ShortStageTitle = "",
            //    CustomProperties = "",
            //    SkipOnCondition = "[Description] <> 'Please complete new employee on-boarding process.'",
            //});

            //string extensionVoicemailID = currStageID.ToString();

            //mList.Add(new LifeCycleStage()
            //{
            //    Action = "",
            //    Name = "Setup Phone extension and Voicemail",
            //    StageTitle = "Setup Phone extension and Voicemail",
            //    UserPrompt = "<b> Complete all sub-tasks to move to next stage</b>",
            //    StageStep = 8,
            //    ModuleNameLookup = ModuleName,
            //    StageApprovedStatus = ++currStageID,
            //    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
            //    StageReturnStatus = Convert.ToInt32(buySetupIPhoneID == "" ? "0" : buySetupIPhoneID),
            //    ActionUser = "",
            //    ApproveActionDescription = "",
            //    RejectActionDescription = "",
            //    ReturnActionDescription = "",
            //    StageApproveButtonName = "Approve",
            //    StageRejectedButtonName = "Reject",
            //    StageReturnButtonName = "Return",
            //    StageTypeLookup = 1,
            //    StageAllApprovalsRequired = false,
            //    StageWeight = 5,
            //    ShortStageTitle = "",
            //    CustomProperties = "",
            //    SkipOnCondition = "[Description] <> 'Please complete new employee on-boarding process.'",
            //});

            //string setupVPNAccessID = currStageID.ToString();

            //mList.Add(new LifeCycleStage()
            //{
            //    Action = "",
            //    Name = "Setup VPN access",
            //    StageTitle = "Setup VPN access",
            //    UserPrompt = "<b> Complete all sub-tasks to move to next stage</b>",
            //    StageStep = 9,
            //    ModuleNameLookup = ModuleName,
            //    StageApprovedStatus = ++currStageID,
            //    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
            //    StageReturnStatus = Convert.ToInt32(extensionVoicemailID == "" ? "0" : extensionVoicemailID),
            //    ActionUser = "",
            //    ApproveActionDescription = "",
            //    RejectActionDescription = "",
            //    ReturnActionDescription = "",
            //    StageApproveButtonName = "Approve",
            //    StageRejectedButtonName = "Reject",
            //    StageReturnButtonName = "Return",
            //    StageTypeLookup = 1,
            //    StageAllApprovalsRequired = false,
            //    StageWeight = 5,
            //    ShortStageTitle = "",
            //    CustomProperties = "",
            //    SkipOnCondition = "[Description] <> 'Please complete new employee on-boarding process.'",
            //});




            string pendingCloseStageID = currStageID.ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Pending Close",
                Name = "Pending Close",
                StageTitle = "Pending Close",
                UserPrompt = "<b>Pending Close</b>",
                UserWorkflowStatus = "Pending Close",
                StageStep = 4,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 3,
                ActionUser = "OwnerUser",
                ApproveActionDescription = "Owner Closed Request",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[Owner Approval Required] = 'False'",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed Phase",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "<b>Service request has been completed.</b>",
                UserWorkflowStatus = "Closed Phase",
                StageStep = 5,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 3,
                ActionUser = "OwnerUser",
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });

            currStageID = int.Parse(closeStageID);


            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // SVC
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "RequestorUser", FieldDisplayName = "Requestor", IsUseInWildCard = true, IsDisplay = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "TicketId", FieldDisplayName = "Request ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });         
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "CreationDate", FieldDisplayName = "Created On", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, SortOrder = 2, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "PctComplete", FieldDisplayName = "% Comp", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "CloseDate", FieldDisplayName = "Closed On", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "SVC", FieldName = "ApproverUser", FieldDisplayName = "Approver", IsUseInWildCard = true, FieldSequence = ++seqNum });
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

            List<string[]> dataList = new List<string[]>();
            int seqNum = 0;
            // Tab 1: General 
            // Display Basic project infomation title, manager, benieficaries, sponsors
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Initiator", FieldDisplayName = "Initiator", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "InitiatorUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Initiated Date", FieldDisplayName = "Initiated Date", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CreationDate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Approver", FieldDisplayName = "Approver", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "ApproverUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Desired Completion Date", FieldDisplayName = "Desired Completion Date", ModuleNameLookup = ModuleName, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Tickets/Tasks", FieldDisplayName = "Tickets/Tasks", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Tickets/Tasks" });
            mList.Add(new ModuleFormLayout() { Title = "TasksList", FieldDisplayName = "Tasks", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false" });
            mList.Add(new ModuleFormLayout() { Title = "Tickets/Tasks", FieldDisplayName = "Tickets/Tasks", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Tickets/Tasks" });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 2 - Summary
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Summary", FieldDisplayName = "Summary", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "ServiceQuestionSummary", FieldDisplayName = "ServiceQuestionSummary", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false" });
            mList.Add(new ModuleFormLayout() { Title = "Summary", FieldDisplayName = "Summary", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // Tab 3 - Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 4 - History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History" });

            //Tab 5 - Life Cycles tab
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Tickets/Tasks", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Lifecycles" });
            mList.Add(new ModuleFormLayout() { Title = "approvaltab", FieldDisplayName = "approvaltab", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Tickets/Tasks", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Lifecycles" });


            string currTab = string.Empty;
            foreach (string[] data in dataList)
            {
                ModuleFormLayout mFormLayout = new ModuleFormLayout();
                mFormLayout.Title = data[0];
                mFormLayout.FieldDisplayName = data[0];
                mFormLayout.ModuleNameLookup = data[1];
                if (currTab != data[2])
                {
                    currTab = data[2];
                    seqNum = 0;
                }
                mFormLayout.TabId = Convert.ToInt32(currTab);
                mFormLayout.FieldSequence = (++seqNum);
                mFormLayout.FieldDisplayWidth = Convert.ToInt32(data[3]);
                mFormLayout.FieldName = data[4];
                mFormLayout.ShowInMobile = Convert.ToBoolean(Convert.ToInt32(data[5]));
                mFormLayout.CustomProperties = data[6];

                mFormLayout.HideInTemplate = Convert.ToBoolean(Convert.ToInt32(HideInTemplate.Contains(data[4]) ? "1" : "0"));

                mList.Add(mFormLayout);
            }
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            // All stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false, HideInServiceMapping = true, Title = 0 + " - " + "Attachments" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false, HideInServiceMapping = false, Title = 0 + " - " + "IsPrivate" });

            // Tab 1
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true, HideInServiceMapping = false, Title = 1 + " - " + "Title" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true, HideInServiceMapping = false, Title = 1 + " - " + "TicketDescription" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true, HideInServiceMapping = false, Title = 1 + " - " + "TicketRequestor" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true, HideInServiceMapping = false, Title = 1 + " - " + "TicketDesiredCompletionDate" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApproverUser", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true, HideInServiceMapping = false, Title = 1 + " - " + "Approver" });
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

            List<string[]> dataList = new List<string[]>();
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Initiated",
                EmailTitle = "SVC Request Returned [$TicketId$]: [$Title$]",
                EmailBody = @"SVC Request ID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID,
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Initiated"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Requestor Notification",
                EmailTitle = "New SVC Request Created [$TicketId$]: [$Title$]",
                EmailBody = @"SVC Request ID [$TicketIdWithoutLink$] has been created on your behalf.<br><br>" +
                                                        "Please review the details below. You will be notified when the ticket is resolved.",
                ModuleNameLookup = ModuleName,
                StageStep = (stageID + 1),
                SendEvenIfStageSkipped = true,
                Title = ModuleName + " - " + "Requestor Notification"
            });

            if (enableManagerApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Manager Approval",
                    EmailTitle = "New SVC Request Pending Manager Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"SVC Ticket ID [$TicketId$] is pending approval.<br><br>" +
                                                        "Please approve or reject the request.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID),
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Manager Approval"
                });
            }

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Assigned",
                EmailTitle = "SVC Request Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"SVC Request ID [$TicketId$] has been assigned to you for resolution.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID),
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Assigned"
            }); // Disabled (no user type) - not usually needed

            if (enablePendingCloseStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Pending Close",
                    EmailTitle = "SVC Request Pending Close [$TicketId$]: [$Title$]",
                    EmailBody = @"SVC Request ID [$TicketId$] has completed testing and is pending close.<br><br>" +
                                                        "Please review the resolution and close the ticket.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID),
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Pending Close"
                });
            }

            mList.Add(new ModuleTaskEmail()
            { Status = "Closed", EmailTitle = "SVC Request Closed [$TicketId$]: [$Title$]", EmailBody = "SVC Request ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (stageID + 1), SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "Closed" });
            mList.Add(new ModuleTaskEmail() { Status = "Closed - Requestor", EmailTitle = "SVC Request Closed [$TicketId$]: [$Title$]", EmailBody = "SVC Request ID [$TicketIdWithoutLink$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (++stageID), SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "Closed - Requestor" });

            mList.Add(new ModuleTaskEmail() { Status = "On-Hold", EmailTitle = "SVC Request On Hold [$TicketId$]: [$Title$]", EmailBody = "SVC Request ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null, SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "On-Hold" });
            mList.Add(new ModuleTaskEmail() { Status = "On-Hold - Requestor", EmailTitle = "SVC Request On Hold [$TicketId$]: [$Title$]", EmailBody = "SVC Request ID [$TicketIdWithoutLink$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null, SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "On-Hold - Requestor" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "SVCRequests", TabOrder = 1, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "SVCRequests", TabOrder = 2, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Items", ViewName = "SVCRequests", TabOrder = 3, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Items", ViewName = "SVCRequests", TabOrder = 4, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Items", ViewName = "SVCRequests", TabOrder = 5, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Items", ViewName = "SVCRequests", TabOrder = 6, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Items", ViewName = "SVCRequests", TabOrder = 7, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Items", ViewName = "SVCRequests", TabOrder = 8, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Items", ViewName = "SVCRequests", TabOrder = 9, ModuleNameLookup = "SVC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            return tabs;
        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {

        }
    }
}
