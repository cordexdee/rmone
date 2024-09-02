using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DefaultConfig;
using static uGovernIT.DefaultConfig.Data.DefaultData.ConfigData;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{

    public class ACR : IModule
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
        protected static bool enableManagerApprovalStage = true;
        protected static bool enablePendingCloseStage = true;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Application Change Request",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "ACR",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/acrtickets",
                    ModuleHoldMaxStage = 5,
                    Title = "Application Change Request (ACR)",
                    // ModuleDescription = "This module is used to track requests for application or data changes for existing applications in production. This may include enhancements, fixes, new development or maintenance.",
                    ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details. Or, select a ticket by clicking checkbox and clicking Actions button.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/acr",
                    ModuleType = ModuleType.SMS,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    AllowDelete = true,
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableCache = true,
                    EnableEventReceivers = true,
                    EnableNewsOnHomePage = true,
                    EnableLayout = true,
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
                return "ACR";
            }
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");
            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Work Activity", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Work Activity" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });
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
            int numStages = 7;
            int moduleStep = 0;
            if (!enableManagerApprovalStage)
                numStages--;
            if (!enablePendingCloseStage)
                numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Name = "Initiated",
                StageTitle = "Initiated",
                UserWorkflowStatus = "Click on create button",
                UserPrompt = "<b>&nbsp;Please complete the form to open a new Application Change Request.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Initiated",
                ActionUser = "Initiator;#Owner;#Requestor;#Admin;#BusinessManager",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Ticket Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "(Re)Submit",
                StageRejectedButtonName = "Cancel",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated.ToString())
            });
            if (enableManagerApprovalStage)
            {
                ///Not a stable stage :)
                mList.Add(new LifeCycleStage()
                {
                    Name = "Manager Approval",
                    StageTitle = "Manager Approval",
                    UserWorkflowStatus = "Awaiting Business manager's approval",
                    UserPrompt = "<b>Business Manager:</b>&nbsp;Please approve the ticket.",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    Action = "Manager Approval",
                    ActionUser = "BusinessManager;#Owner;#Requestor;#Admin;#Initiator",
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                    StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                    ApproveActionDescription = "Manager approved request",
                    RejectActionDescription = "Manager rejected request (closed)",
                    ReturnActionDescription = "",
                    StageApproveButtonName = "Approve",
                    StageRejectedButtonName = "Reject",
                    StageReturnButtonName = "Return",
                    StageAllApprovalsRequired = false,
                    StageWeight = 5,
                    SkipOnCondition = "[RequestTypeWorkflow] = 'SkipApprovals'",
                    CustomProperties = "AllowEmailApproval=true",
                    StageTypeChoice = Convert.ToString(StageType.None)
                });
            }
            mList.Add(new LifeCycleStage()
            {
                Name = "Pending Assignment",
                StageTitle = "Pending Assignment",
                UserWorkflowStatus = "Awaiting Primary Responsible Person(PRP) assignment by Owner",
                UserPrompt = "<b>Owner:</b>&nbsp;Please assign a Primary Responsible Person (PRP) and the Estimated hours on work activity tab.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "PRP Assigned",
                ActionUser = "BusinessManager;#Owner;#Requestor;#Admin;#Initiator",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Owner assigned PRP/ORP",
                RejectActionDescription = "Owner rejected request (closed)",
                ReturnActionDescription = "",
                StageApproveButtonName = "Assign PRP",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);


            mList.Add(new LifeCycleStage()
            {
                Name = "Assigned",
                StageTitle = "Assigned",
                UserWorkflowStatus = "Awaiting resolution by Primary Responsible Person(PRP)/Other Responsible Person(ORP)",
                UserPrompt = "<b>PRP:</b>&nbsp;Please enter Resolution Description and other required fields, and assign a Tester",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Tester Assigned",
                ActionUser = "PRP;#ORP",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PRP/ORP completed request and assigned tester",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Assign Tester",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 40,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });
            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            mList.Add(new LifeCycleStage()
            {
                Name = "Testing",
                StageTitle = "Testing",
                UserWorkflowStatus = "Awaiting testing to be completed by the Tester",
                UserPrompt = "<b>Tester:</b>&nbsp;Please approve/reject the resolution after completing testing",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Tester Complete",
                ActionUser = "Tester",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                ApproveActionDescription = "Tester approved resolution",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 30,
                SkipOnCondition = "autoapprove=false",
                CustomProperties = "[RequestTypeWorkflow] = 'NoTest' || [Tester] = 'null'  || [Tester] = ''",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Security Review",
                StageTitle = "Security Review",
                UserWorkflowStatus = "Awaiting testing to be completed by the Tester",
                UserPrompt = "<b>Tester:</b>&nbsp;Please approve/reject the resolution after completing testing",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Security Review",
                ActionUser = "IT Governance",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 30,
                StageTypeChoice = Convert.ToString(StageType.None)
            });
            if (enablePendingCloseStage)
            {
                mList.Add(new LifeCycleStage()
                {
                    Name = "Pending Close",
                    StageTitle = "Pending Close",
                    UserWorkflowStatus = "Awaiting Owner's approval",
                    UserPrompt = "<b>Owner:</b>&nbsp;Please approve the ticket.",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    Action = "Owner Closed",
                    ActionUser = "Owner",
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = 0,
                    StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                    ApproveActionDescription = "Owner approved resolution & closed ticket",
                    RejectActionDescription = "",
                    ReturnActionDescription = "",
                    StageApproveButtonName = "Approve",
                    StageRejectedButtonName = "Reject",
                    StageReturnButtonName = "Return",
                    StageAllApprovalsRequired = false,
                    StageWeight = 5,
                    SkipOnCondition = "AllowEmailApproval=true",
                    CustomProperties = "",
                    StageTypeChoice = Convert.ToString(StageType.Tested)
                });
            }

            mList.Add(new LifeCycleStage()
            {
                Name = "Closed",
                StageTitle = "Closed",
                UserWorkflowStatus = "Ticket is closed, but you can add comments or can be re-opened by Owner",
                UserPrompt = "Ticket Closed",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Closed",
                ActionUser = "Owner; PRP Group",
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
            int seqNum = 0;
            List<ModuleColumn> mList = new List<ModuleColumn>();
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "RequestorUser", FieldDisplayName = "Requested By", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = false, DisplayForReport = false, FieldSequence = ++seqNum, ColumnType = "ProgressBar" });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "RequestTypeCategory", FieldDisplayName = "Category", IsDisplay = false, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "RequestTypeLookup", FieldDisplayName = "Application", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "ACRTypeTitleLookup", FieldDisplayName = "ACR Type", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsDisplay = false, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = false, DisplayForReport = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "Age", FieldDisplayName = "Age", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = false, DisplayForReport = false, ColumnType = "", SortOrder = 1, IsAscending = false, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "CreationDate", FieldDisplayName = "Created On", IsDisplay = false, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, DisplayForReport = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "CloseDate", FieldDisplayName = "Closed On", IsDisplay = false, IsUseInWildCard = true, CustomProperties = "", DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "StageActionUsersUser", FieldDisplayName = "Waiting On", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum, CustomProperties = "fieldtype = multiuser", });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "PRPUser", FieldDisplayName = "PRP", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "ORPUser", FieldDisplayName = "ORP", IsUseInWildCard = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "ACR", FieldName = "TesterUser", FieldDisplayName = "Tester", IsUseInWildCard = true, FieldSequence = ++seqNum });
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            int startStageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            int pendingAssignmentStageID = startStageID + 2;
            if (!enableManagerApprovalStage)
                pendingAssignmentStageID--;
            int assignedStageID = pendingAssignmentStageID + 1;
            int testingStageID = pendingAssignmentStageID + 2;

            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Requestor",
                KeyName = "Requestor",
                KeyValue = "LoggedInUser",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "BusinessManager",
                KeyName = "BusinessManager",
                KeyValue = "RequestorManagerIfNotManager",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Lookup",
                KeyName = "Lookup",
                KeyValue = "RequestorLocation",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "DeskLocation",
                KeyName = "DeskLocation",
                KeyValue = "RequestorDeskLocation",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "ActualStartDate",
                KeyName = "ActualStartDate",
                KeyValue = "TodaysDate",
                ModuleStepLookup = assignedStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "ActualCompletionDate",
                KeyName = "ActualCompletionDate",
                KeyValue = "TodaysDate",
                ModuleStepLookup = testingStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "PctComplete",
                KeyName = "PctComplete",
                KeyValue = "1",
                ModuleStepLookup = testingStageID.ToString(),
                CustomProperties = "override=true"
            });

            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");

            List<string[]> dataList = new List<string[]>();
            string[] HideInTicketTemplate = { "Attachments", "AssetLookup", "Comment", "CreationDate", "Initiator", "RequestTypeCategory", "Status" };


            // Tab 1: General
            int seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, HideInTemplate = HideInTicketTemplate.Contains("#GroupStart#") ? true : false, CustomProperties = "", TabId = 1, FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, HideInTemplate = HideInTicketTemplate.Contains("Title") ? true : false, CustomProperties = "", TabId = 1, FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Status", Title = "Status", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, HideInTemplate = HideInTicketTemplate.Contains("Status") ? true : false, CustomProperties = "", TabId = 1, FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiator", Title = "Initiator", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "InitiatorUser", ShowInMobile = true, HideInTemplate = HideInTicketTemplate.Contains("Initiator") ? true : false, CustomProperties = "", TabId = 1, FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiated Date", Title = "Initiated Date", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "CreationDate", ShowInMobile = true, HideInTemplate = HideInTicketTemplate.Contains("CreationDate") ? true : false, CustomProperties = "", TabId = 1, FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Desired Completion Date", Title = "Desired Completion Date", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, HideInTemplate = HideInTicketTemplate.Contains("DesiredCompletionDate") ? true : false, CustomProperties = "", TabId = 1, FieldSequence = (++seqNum) });


            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", Title = "Requested By", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, HideInTemplate = HideInTicketTemplate.Contains("Requestor") ? true : false, CustomProperties = "", TabId = 1, FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Business Manager", Title = "Business Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", Title = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application", Title = "Application", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", Title = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Functional Area", Title = "Functional Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ACR Type", Title = "ACR Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 8, FieldName = "ACRTypeTitleLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Organizational Impact", Title = "Release Cycle", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OrganizationalImpact", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Risk", Title = "Risk", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RiskLevel", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Release Cycle", Title = "Release Cycle", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ReleaseID", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Reason for Change", Title = "Reason for Change", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Importance", Title = "Importance", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SeverityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Priority", Title = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", Title = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "", Title = "Is Business Impact Doc Attached?", ModuleNameLookup = ModuleName,TabId = "1", "1", "IsBusinessImpactDocAttached", "1", "" });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Is Private", Title = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 2: Work Activity
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Assignment", Title = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Primary Responsible Person (PRP)", Title = "Primary Responsible Person (PRP)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PRPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Other Responsible (ORPs)", Title = "Other Responsible (ORPs)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ORPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Estimated Total Hours", Title = "Estimated Total Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "EstimatedHours", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Assignment", Title = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments / Work Activity", Title = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Add Comment", Title = "Add Comment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments / Work Activity", Title = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution", Title = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Actual Start Date", Title = "Actual Start Date", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ActualStartDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Actual Complete Date", Title = "Actual Complete Date", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ActualCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "% Complete", Title = "% Complete", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PctComplete", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Actual Analysis Hours", Title = "Actual Analysis Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "BATotalHours", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Actual Development Hours", Title = "Actual Development Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "DeveloperTotalHours", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Total Actual Hours", Title = "Total Actual Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ActualHours", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution Description", Title = "Resolution Description", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "ResolutionComments", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Tester", Title = "Tester", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "TesterUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Is Performance Testing Done?", Title = "Is Performance Testing Done?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "IsPerformanceTestingDone", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution", Title = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Sub-Tasks
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "", Title = "Sub-Tasks", ModuleNameLookup = ModuleName,TabId = 2, 3, "#GroupStart#", "1", "" });
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "", Title = "PreconditionList", ModuleNameLookup = ModuleName,TabId = 2, 3, "#Control#", "1", "" });
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "", Title = "Sub-Tasks", ModuleNameLookup = ModuleName,TabId = 2, 3, "#GroupEnd#", "1", "" });

            ////Tab 3: Summary
            //seqNum = 0;
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "Summary", Title = "Summary", ModuleNameLookup = ModuleName,TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Summary", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "ServiceQuestionSummary", Title = "ServiceQuestionSummary", ModuleNameLookup = ModuleName,TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "Summary", Title = "Summary", ModuleNameLookup = ModuleName,TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Summary", FieldSequence = (++seqNum) });

            // Tab 4: Approvals 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Approvals", Title = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ApprovalTab", Title = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Approvals", Title = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 5: Emails
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", Title = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", Title = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", Title = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });

            // Tab 6: Related s
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", Title = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "CustomTicketRelationship", Title = "CustomRelationship", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", Title = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", Title = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "WikiRelatedTickets", Title = "WikiRelateds", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", Title = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 7: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ACR Type", Title = "ACR Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ACRTypeTitleLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", Title = "Requested By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Business Manager", Title = "Business Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Desired Completion Date", Title = "Desired Completion Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", Title = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application", Title = "Application", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", Title = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });


            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Organizational Impact", Title = "OrganizationalImpact", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OrganizationalImpact", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Risk", Title = "Risk", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RiskLevel", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Release Cycle", Title = "Release Cycle", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ReleaseID", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Reason for Change", Title = "Reason for Change", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Importance", Title = "Importance", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SeverityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Priority", Title = "Priority", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", Title = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Private", Title = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "", Title = "Is Business Impact Doc Attached?", ModuleNameLookup = ModuleName,TabId = "0", "1", "IsBusinessImpactDocAttached", "1", "" });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            //List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            /*
            //CRM
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "CRM", "CRM"), RequestType = "CRM", ModuleNameLookup = ModuleName, Category = "CRM", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            //Digital
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Digital Transformation", "Digital Transformation"), RequestType = "Digital Transformation", ModuleNameLookup = ModuleName, Category = "Digital Transformation", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            //ERP
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "CMM"), RequestType = "CMM", ModuleNameLookup = ModuleName, Category = "ERP", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "ProFolio Management Modules"), RequestType = "ProFolio Management Modules", ModuleNameLookup = ModuleName, Category = "ERP", SubCategory = "Sage Software", WorkflowType = "Full", Owner = Variables.Name });
            //Governance
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Governance", "Governance"), RequestType = "Governance", ModuleNameLookup = ModuleName, Category = "Governance", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            //HR
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "HR", "HR"), RequestType = "HR", ModuleNameLookup = ModuleName, Category = "HR", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            //Internet
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Internet/Intranet", "Intranet"), RequestType = "Intranet", ModuleNameLookup = ModuleName, Category = "Internet/Intranet", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            //JDEdwards
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "E1 - Accounts Payable"), RequestType = "E1 - Accounts Payable", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "E1 - EAM"), RequestType = "E1 - EAM", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "E1 - Fixed Assets"), RequestType = "E1 - Fixed Assets", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "E1 - General Ledger"), RequestType = "E1 - General Ledger", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "E1 - Technical"), RequestType = "E1 - Technical", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Finance"), RequestType = "Finance", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Manufacturing"), RequestType = "Manufacturing", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Procurement"), RequestType = "Procurement", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Sales Order Entry"), RequestType = "Sales Order Entry", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Sales Order Management"), RequestType = "Sales Order Management", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Supply Chain"), RequestType = "Supply Chain", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Technical"), RequestType = "Technical", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "JDEdwards", "Warehouse Mgmnt."), RequestType = "Warehouse Mgmnt.", ModuleNameLookup = ModuleName, Category = "JDEdwards", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            //LEGACY
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Legacy", "Legacy Application"), RequestType = "Legacy Application", ModuleNameLookup = ModuleName, Category = "Legacy", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            //Non -ERP 
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "Custom Solutions"), RequestType = "Custom Solutions", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "DSRP"), RequestType = "DSRP", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "EDI"), RequestType = "EDI", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "Kronos"), RequestType = "Kronos", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "PC Miler"), RequestType = "PC Miler", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "STRAP"), RequestType = "STRAP", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "TerraTech"), RequestType = "TerraTech", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "TMS (Traffic Management System)"), RequestType = "TMS (Traffic Management System)", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Non-ERP Applications", "TPMS (Trade Promotion Management System)"), RequestType = "TPMS (Trade Promotion Management System)", ModuleNameLookup = ModuleName, Category = "Non-ERP Applications", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });

            //Other
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other", "Custom Solutions"), RequestType = "Other", ModuleNameLookup = ModuleName, Category = "Other", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });


            //other-3rd party SW
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "Broderick"), RequestType = "Broderick", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "FaxStar"), RequestType = "FaxStar", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "LaserVault"), RequestType = "LaserVault", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "ODS (Object Distribution System)"), RequestType = "ODS (Object Distribution System)", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "OMS (Object Mirrorring System)"), RequestType = "OMS (Object Mirrorring System)", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "PowerLock"), RequestType = "PowerLock", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "QlikView"), RequestType = "QlikView", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "Screen Manager"), RequestType = "Screen Manager", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
           // mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "Service Tracking System"), RequestType = "Service Tracking System", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Other 3rd-party SW", "Sysco Quick Review"), RequestType = "Sysco Quick Review", ModuleNameLookup = ModuleName, Category = "Other 3rd-party SW", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });

            //WSEB
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "WESB", "Enterprise Integration"), RequestType = "Enterprise Integration", ModuleNameLookup = ModuleName, Category = "WESB", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "WESB", "WESB"), RequestType = "WESB", ModuleNameLookup = ModuleName, Category = "WESB", SubCategory = "", WorkflowType = "Full", Owner = Variables.Name });
            
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "BI", "Data Tracker"), RequestType = "Data Tracker", ModuleNameLookup = ModuleName, Category = "Ticketing Support", SubCategory = "BI", WorkflowType = "Full", Owner = Variables.Name });
            
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "BI", "OBIEE"), RequestType = "OBIEE", ModuleNameLookup = ModuleName, Category = "Ticketing Support", SubCategory = "BI", WorkflowType = "Full", Owner = Variables.Name });
            
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "BI", "QlikView"), RequestType = "QlikView", ModuleNameLookup = ModuleName, Category = "Ticketing Support", SubCategory = "BI", WorkflowType = "Full", Owner = Variables.Name });
            */

            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "3rd Party Apps", "3rd Party Tool"), Category = "3rd Party Apps", RequestType = "3rd Party Tool", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "BI/Analytical", "BI Tool"), Category = "BI/Analytical", RequestType = "BI Tool", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Collaboration", "Collaboration Tool"), Category = "Collaboration", RequestType = "Collaboration Tool", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "CRM", "CRM Modules"), Category = "CRM", RequestType = "CRM Modules", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Custom Solutions", "Custom Software"), Category = "Custom Solutions", RequestType = "Custom Software", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Accounts Payable"), Category = "ERP", RequestType = "Accounts Payable", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Accounts Receivable"), Category = "ERP", RequestType = "Accounts Receivable", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Finance"), Category = "ERP", RequestType = "Finance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Fixed Assets"), Category = "ERP", RequestType = "Fixed Assets", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "General Ledger"), Category = "ERP", RequestType = "General Ledger", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Manufacturing"), Category = "ERP", RequestType = "Manufacturing", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Procurement"), Category = "ERP", RequestType = "Procurement", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Sales Order Management"), Category = "ERP", RequestType = "Sales Order Management", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Supply Chain"), Category = "ERP", RequestType = "Supply Chain", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Warehouse Management"), Category = "ERP", RequestType = "Warehouse Management", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Legacy", "Legacy Application"), Category = "Legacy", RequestType = "Legacy Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {

            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");

            int moduleStep = 0;

            // Editable in all stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            //dataList.Add(new string[] { "TicketComment", ModuleName, moduleStep.ToString(), "0", "0", "0", "", "1" });

            //Editable in stage 1 - New ticket
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SecurityManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FunctionalArea", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HelpDeskCall", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsBusinessImpactDocAttached", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ACRTypeTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            // mList.Add(new ModuleRoleWriteAccess() { FieldName = "OrganizationalImpact", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RiskLevel", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReleaseID", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });

            //Editable in stage 2 - Manager Approval
            if (enableManagerApprovalStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                //mList.Add(new ModuleRoleWriteAccess() { FieldName = "OrganizationalImpact", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RiskLevel", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReleaseID", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });

            }

            //Editable in stage 3 - Pending Assignment
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstimatedHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });

            //mList.Add(new ModuleRoleWriteAccess() { FieldName = "OrganizationalImpact", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RiskLevel", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ReleaseID", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            //Editable in stage 4 - Assigned
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "Owner;#PRPGroup", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "Owner;#PRPGroup", ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TesterUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PctComplete", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsPerformanceTestingDone", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BATotalHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DeveloperTotalHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });

            // Editable in stage 5 - Testing
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TesterUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "Owner;#PRP", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsPerformanceTestingDone", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, HideInServiceMapping = true });
            // Stage 6 - Pending Close
            if (enablePendingCloseStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            }
            return mList;

        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  StatusMapping");

            List<string[]> dataList = new List<string[]>();
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1, Title = ModuleName + "-" + "Initiated" });
            if (enableManagerApprovalStage)
                mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1, Title = ModuleName + "-" + "Manager Approval" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1, Title = ModuleName + "-" + "Pending Assignment" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + "-" + "Assigned" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + "-" + "Testing" });
            if (enablePendingCloseStage)
                mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + "-" + "Pending Close" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4, Title = ModuleName + "-" + "Closed" });

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
                EmailTitle = "ACR Ticket Returned [$TicketId$]: [$Title$]",
                EmailBody = @"ACR Ticket ID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID,
                EmailUserTypes = "Initiator",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Initiated"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Requestor Notification",
                EmailTitle = "New ACR Ticket Created [$TicketId$]: [$Title$]",
                EmailBody = @"ACR Ticket ID [$TicketIdWithoutLink$] has been created on your behalf.<br><br>" +
                                                        "Please review the details below. You will be notified when the ticket is resolved.",
                ModuleNameLookup = ModuleName,
                StageStep = (stageID + 1),
                EmailUserTypes = "Requestor",
                SendEvenIfStageSkipped = true,
                Title = ModuleName + " - " + "Requestor Notification"
            });

            if (enableManagerApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Manager Approval",
                    EmailTitle = "New ACR Ticket Pending Manager Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"ACR Ticket ID [$TicketId$] is pending Manager Approval.<br><br>" +
                                                         "Please approve or reject the request.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID),
                    EmailUserTypes = "BusinessManager",
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Manager Approval"
                });
            }

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Pending Assignment",
                EmailTitle = "ACR Ticket Pending Assignment [$TicketId$]: [$Title$]",
                EmailBody = @"ACR Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID),
                EmailUserTypes = "Owner",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Pending Assignment"
            });
            mList.Add(new ModuleTaskEmail()
            {
                Status = "Assigned",
                EmailTitle = "ACR Ticket Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"ACR Ticket ID [$TicketId$] has been assigned to you for resolution.<br><br>" +
                                                        "Once resolved, please enter the resolution description and actual hours in the ticket, and assign a tester.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID),
                EmailUserTypes = "PRP;#ORP",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Assigned"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Assigned- Ticket is created and assigned",
                EmailTitle = "Ticket [$TicketId$]: [$Title$] is created and assigned to [$PRP$]",
                EmailBody = @"Ticket [$TicketID$] is created and assigned to [$PRP$]." +
                                   "Expected resolution time is [$ExpectedResolution$]." +
                                       "You will be notified by email when resolved.",
                ModuleNameLookup = ModuleName,
                StageStep = (stageID),
                EmailUserTypes = "Initiator",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Assigned"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Assigned - Notify Requester",
                EmailTitle = "TSR Ticket Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"TSR Ticket ID[$TicketIdWithoutLink$] has been assigned to a technician." +
                                   "Ticket Descripton: [$Title$]" +
                                   "You will be notified when the issue is resolved.",
                ModuleNameLookup = ModuleName,
                StageStep = (stageID),
                EmailUserTypes = "Requestor",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Assigned"
            });







            mList.Add(new ModuleTaskEmail()
            {
                Status = "Testing",
                EmailTitle = "ACR Ticket Awaiting Testing [$TicketId$]: [$Title$]",
                EmailBody = @"ACR Ticket ID [$TicketId$] has been assigned to you for testing.<br><br>" +
                                                       "Please test the resolution and approve or reject the resolution.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID),
                EmailUserTypes = "Tester",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Testing"
            });

            if (enablePendingCloseStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Pending Close",
                    EmailTitle = "ACR Ticket Pending Close [$TicketId$]: [$Title$]",
                    EmailBody = @"ACR Ticket ID [$TicketId$] has completed testing and is pending close.<br><br>" +
                                                        "Please review the resolution and close the ticket.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID),
                    EmailUserTypes = "Owner",
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Pending Close"
                });
            }

            mList.Add(new ModuleTaskEmail() { Status = "Closed", EmailTitle = "ACR Ticket Closed [$TicketId$]: [$Title$]", EmailBody = "ACR Ticket ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (stageID + 1), EmailUserTypes = "Owner", SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "Closed" });

            mList.Add(new ModuleTaskEmail() { Status = "On-Hold", EmailTitle = "ACR Ticket On Hold [$TicketId$]: [$Title$]", EmailBody = "ACR Ticket ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null, EmailUserTypes = "Owner", SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "On-Hold" });

            return mList;

        }

        public List<ACRType> GetACRTypes()
        {
            List<ACRType> mList = new List<ACRType>();
            // Console.WriteLine("  ACRTypes");

            mList.Add(new ACRType() { Title = "Break / Fix" });
            mList.Add(new ACRType() { Title = "Data Change" });
            mList.Add(new ACRType() { Title = "Enhancement" });
            mList.Add(new ACRType() { Title = "Maintenance" });
            mList.Add(new ACRType() { Title = "New Development" });

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

        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", IsVIP = true, Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4 });
            return mList;
        }

        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            // Console.WriteLine("  RequestPriority");
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 7, SeverityLookup = 7, PriorityLookup = 7 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 7, SeverityLookup = 8, PriorityLookup = 7 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 7, SeverityLookup = 9, PriorityLookup = 8 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 8, SeverityLookup = 7, PriorityLookup = 7 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 8, SeverityLookup = 8, PriorityLookup = 8 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 8, SeverityLookup = 9, PriorityLookup = 9 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 9, SeverityLookup = 7, PriorityLookup = 8 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 9, SeverityLookup = 8, PriorityLookup = 9 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 9, SeverityLookup = 9, PriorityLookup = 9 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 10, SeverityLookup = 7, PriorityLookup = 7 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 10, SeverityLookup = 8, PriorityLookup = 8 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 10, SeverityLookup = 9, PriorityLookup = 9 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 11, SeverityLookup = 7, PriorityLookup = 7 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 11, SeverityLookup = 8, PriorityLookup = 8 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 11, SeverityLookup = 9, PriorityLookup = 9 });
            return mList;
        }

        public List<ModuleSeverity> GetModuleSeverity()
        {
            // Console.WriteLine(" TicketSeverity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "ACR", Name = "Mission Critical", Title = "Mission Critical", Severity = "Mission Critical", ItemOrder = 1 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "ACR", Name = "Necessary", Title = "Necessary", Severity = "Necessary", ItemOrder = 2 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "ACR", Name = "Nice To Have", Title = "Nice To Have", Severity = "Nice To Have", ItemOrder = 3 });
            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "Requestor", UserTypes = "Requestor", ColumnName = "RequestorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "Business Manager", UserTypes = "Business Approver", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "ManagerOf=TicketRequestor" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "PRP", UserTypes = "PRP", ColumnName = "PRPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "ORP", UserTypes = "ORP", ColumnName = "ORPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "Tester", UserTypes = "Tester", ColumnName = "TesterUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "ACR", Title = "PRP Group", UserTypes = "PRP Group", ColumnName = "PRPGroupUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            mList.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Compliance Related", Impact = "Compliance Related", ItemOrder = 1 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Fixes a Bug", Impact = "Fixes a Bug", ItemOrder = 2 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Improves Efficiency / Reduces Costs", Impact = "Improves Efficiency / Reduces Costs", ItemOrder = 3 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Increases Value/Revenue", Impact = "Increases Value/Revenue", ItemOrder = 4 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Other", Impact = "Other", ItemOrder = 5 });
            return mList;

        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Unassigned", ViewName = "ACRTickets", TabOrder = 1, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "ACRTickets", TabOrder = 2, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Items", ViewName = "ACRTickets", TabOrder = 3, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Items", ViewName = "ACRTickets", TabOrder = 4, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Items", ViewName = "ACRTickets", TabOrder = 5, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Open Items", ViewName = "ACRTickets", TabOrder = 6, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "Resolved Items", ViewName = "ACRTickets", TabOrder = 7, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Items", ViewName = "ACRTickets", TabOrder = 8, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Closed Items", ViewName = "ACRTickets", TabOrder = 9, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "ACRTickets", TabOrder = 10, ModuleNameLookup = "ACR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            return tabs;
        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            //Assigning value for impact
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Compliance Related", Impact = "Compliance Related", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Fixes a Bug", Impact = "Fixes a Bug", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Improves Efficiency / Reduces Costs", Impact = "Improves Efficiency / Reduces Costs", ItemOrder = 4 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Increases Value/Revenue", Impact = "Increases Value/Revenue", ItemOrder = 5 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "Other", Impact = "Other", ItemOrder = 6 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "ACR", Title = "One-Time Functionality", Impact = "One-Time Functionality", ItemOrder = 7 });

            //Assigning value for severity
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "ACR", Name = "Mission Critical", Title = "Mission Critical", Severity = "Mission Critical", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "ACR", Name = "Necessary", Title = "Necessary", Severity = "Necessary", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "ACR", Name = "Nice To Have", Title = "Nice To Have", Severity = "Nice To Have", ItemOrder = 3 });
            //serverities.Add(new ModuleSeverity() { ModuleNameLookup = "ACR", Name = "Vip Required", Title = "Vip Required", Severity = "Vip Required", ItemOrder = 4 });

            //Assigning value for prioriry
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", Title = "2-High", uPriority = "2-High", ItemOrder = 1, Color = "#FF7F1D" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 ,Color = "#FFED14" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3, Color = "#cccfd2" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "ACR", Title = "1-Critical", IsVIP = true, uPriority = "1-Critical", ItemOrder = 4, Color = "#FF351F" });

            // Pass value in ImpactLookup, SeverityLookup and priorityLookup, value always assign by index of list of respctive lookup

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 1, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 2, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 3, SeverityLookup = 0, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 4, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 5, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 6, SeverityLookup = 0, PriorityLookup = 0 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 3, SeverityLookup = 1, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 4, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 5, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 6, SeverityLookup = 1, PriorityLookup = 1 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 3, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 4, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 5, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 6, SeverityLookup = 2, PriorityLookup = 2 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 3, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 4, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 5, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "ACR", ImpactLookup = 6, SeverityLookup = 2, PriorityLookup = 2 });
        }
    }
}

