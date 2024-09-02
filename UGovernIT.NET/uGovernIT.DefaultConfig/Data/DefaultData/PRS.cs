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
    public class PRS : IModule
    {
        public static string userId = "1";
        public static string managerUserId = "1";
        public static string userName = ""; // Obtained from userId in UGovernITDefault.Initialize()

        public string[] HideInTemplate = { "Attachments", "AssetLookup", "Comment", "CreationDate", "Initiator", "RequestTypeCategory", "Status" };


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
        protected string moduleId = "1";
        protected static bool enableTestingStage = false;
        protected static bool enablePendingCloseStage = false;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Problem Resolution",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "PRS",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/prstickets",
                    ModuleHoldMaxStage = 4,
                    Title = "Problem Resolution (PRS)",
                    ModuleDescription = "This module is used to report a technical problem that requires IT intervention to fix. Used to report all technology problems including: desktop or laptop errors, printing difficulties, e-mail problems, network connection or system issues, cellular telephone trouble, software or application errors, and other pressing assistance requests.",                   
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/prs",
                    ModuleType = ModuleType.SMS,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    UseInGlobalSearch = true,
                    KeepItemOpen = true,
                    ShowBottleNeckChart=true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "PRS";
            }
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();

            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = "PRS" });
            mList.Add(new ModuleFormTab() { TabName = "Work Activity", TabId = 2, TabSequence = 2, ModuleNameLookup = "PRS" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 3, TabSequence = 3, ModuleNameLookup = "PRS" });
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 4, TabSequence = 4, ModuleNameLookup = "PRS" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 5, TabSequence = 5, ModuleNameLookup = "PRS" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 6, TabSequence = 6, ModuleNameLookup = "PRS" });

            return mList;
        }



        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // PRS
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "RequestorUser", FieldDisplayName = "Requested By", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "SelfAssign", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum, ColumnType = "ProgressBar" });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "RequestTypeCategory", FieldDisplayName = "Category", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "RequestTypeLookup", FieldDisplayName = "Problem Type", IsDisplay = false, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsDisplay = false, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "Age", FieldDisplayName = "Age", IsDisplay = true, IsUseInWildCard = true, SortOrder = 2, FieldSequence = ++seqNum });// Sort 1
                                                                                                                                                                                                   //mList.Add(new ModuleColumns() { ModuleNameLookup = "PRS", "DueIn", "Due In", "1", "0", "" });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "CreationDate", FieldDisplayName = "Created On", IsDisplay = false, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "ActualCompletionDate", FieldDisplayName = "Resolution Date", IsDisplay = false, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "CloseDate", FieldDisplayName = "Closed On", IsDisplay = false, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "StageActionUsersUser", FieldDisplayName = "Waiting On", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", IsDisplay = false, IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "PRPUser", FieldDisplayName = "PRP", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "ORPUser", FieldDisplayName = "ORP", FieldSequence = ++seqNum, IsUseInWildCard = true, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "TesterUser", FieldDisplayName = "Tester", IsUseInWildCard = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });

            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "ActualHours", FieldDisplayName = "Actual Hours", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "ResolutionComments", FieldDisplayName = "Resolution", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PRS", FieldName = "InitiatorResolvedChoice", FieldDisplayName = " Initiator Resolved", IsUseInWildCard = true, FieldSequence = ++seqNum });
            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");
            List<string[]> dataList = new List<string[]>();
            // Start from ID 1
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 6;
            if (!enableTestingStage)
                numStages--;
            if (!enablePendingCloseStage)
                numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();
            int moduleStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Initiated",
                UserPrompt = "<b>Please fill the form to open a new PRS ticket.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "InitiatorUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Ticket Submitted",
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

            string pendingAssignmentStageID = currStageID.ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "PRP Assigned",
                Name = "Pending Assignment",
                StageTitle = "Pending Assignment",
                UserPrompt = "<b>Owner:</b>Please assign a Primary Responsible Person (PRP) and the Estimated hours on work activity tab.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser;#PRPGroupUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PRP / ORP Assigned",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Assign PRP",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "SelfAssign=true;#DoNotWaitForActionUser=true",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Assigned",
                Name = "Assignment",
                StageTitle = "Assignment",
                UserPrompt = "<b>PRP:</b> Please enter Resolution and other required fields and resolve the ticket",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "PRPUser;#ORPUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(pendingAssignmentStageID == "" ? "0" : pendingAssignmentStageID),
                ApproveActionDescription = "PRP / ORP Assigned",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Resolved",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 50,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            if (enableTestingStage)
            {

                mList.Add(new LifeCycleStage()
                {
                    Action = "Testing",
                    Name = "Testing",
                    StageTitle = "Awaiting testing to be completed by the Tester",
                    UserPrompt = "<b>Tester:</b> Please approve/reject the resolution after completing testing",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    ActionUser = "TesterUser",
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = 0,
                    StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                    ApproveActionDescription = "Tester approved resolution",
                    RejectActionDescription = "",
                    ReturnActionDescription = "",
                    StageApproveButtonName = "Approve",
                    StageRejectedButtonName = "",
                    StageReturnButtonName = "Return",
                    StageTypeLookup = 3,
                    StageAllApprovalsRequired = false,
                    StageWeight = 20,
                    ShortStageTitle = "",
                    CustomProperties = "",
                    SkipOnCondition = "[TicketRequestTypeWorkflow] = 'NoTest'",
                    StageTypeChoice = "Testing"
                });
            }

            moduleTestedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            if (enablePendingCloseStage)
            {

                mList.Add(new LifeCycleStage()
                {
                    Name = "Pending Close",
                    StageTitle = "Pending Close",
                    UserWorkflowStatus = "Awaiting Owner's approval",
                    UserPrompt = "<b>Owner:</b> Please approve the ticket.",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    Action = "Owner Closed",
                    ActionUser = "OwnerUser",
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
                    StageWeight = 20,
                    SkipOnCondition = "",
                    CustomProperties = ""
                });
            }

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "Ticket Closed",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser;#Asset Managers",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "Owner approved resolution & closed ticket",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 5,
                StageAllApprovalsRequired = false,
                StageWeight = 0,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });


            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "DesiredCompletionDate",
                KeyName = "DesiredCompletionDate",
                KeyValue = "TomorrowsDate",
                ModuleStepLookup = "1",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "LocationLookup",
                KeyName = "LocationLookup",
                KeyValue = "RequestorLocation",
                ModuleStepLookup = "1",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "InitiatorResolved",
                KeyName = "InitiatorResolvedChoice",
                KeyValue = "No",
                ModuleStepLookup = "1",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "ImpactLookup",
                KeyName = "ImpactLookup",
                KeyValue = "3",
                ModuleStepLookup = "1",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "SeverityLookup",
                KeyName = "SeverityLookup",
                KeyValue = "3",
                ModuleStepLookup = "1",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "BusinessManager",
                KeyName = "BusinessManager",
                KeyValue = "RequestorManager",
                ModuleStepLookup = "2",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "EstimatedHours",
                KeyName = "EstimatedHours",
                KeyValue = "0.5",
                ModuleStepLookup = "2",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "ActualCompletionDate",
                KeyName = "ActualCompletionDate",
                KeyValue = "TodaysDate",
                ModuleStepLookup = "4",
                CustomProperties = ""
            });

            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "PctComplete",
                KeyName = "PctComplete",
                KeyValue = "1",
                ModuleStepLookup = "4",
                CustomProperties = ""
            });
            return mList;

        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");
            int seqNum = 0;
            // Tab 1: General
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, FieldDisplayWidth = 2, FieldName = "Title", HideInTemplate = HideInTemplate.Contains("Title") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });
            mList.Add(new ModuleFormLayout() { Title = "Status", FieldDisplayName = "Status", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "Status", HideInTemplate = HideInTemplate.Contains("Status") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });

            mList.Add(new ModuleFormLayout() { Title = "Initiator", FieldDisplayName = "Initiator", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "Initiator", HideInTemplate = HideInTemplate.Contains("Initiator") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });
            mList.Add(new ModuleFormLayout() { Title = "Initiated Date", FieldDisplayName = "Created On", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "CreationDate", HideInTemplate = HideInTemplate.Contains("Title") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });
            mList.Add(new ModuleFormLayout() { Title = "Desired Completion Date", FieldDisplayName = "Target On", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", HideInTemplate = HideInTemplate.Contains("DesiredCompletionDate") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });

            mList.Add(new ModuleFormLayout() { Title = "Requested By", FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "RequestorUser", HideInTemplate = HideInTemplate.Contains("Requestor") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });
            mList.Add(new ModuleFormLayout() { Title = "Location", FieldDisplayName = "Location", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "LocationLookup", HideInTemplate = HideInTemplate.Contains("LocationLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });
            mList.Add(new ModuleFormLayout() { Title = "Request Source", FieldDisplayName = "Request Source", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "RequestSourceChoice", HideInTemplate = HideInTemplate.Contains("RequestSource") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, FieldDisplayWidth = 3, FieldName = "Description", HideInTemplate = HideInTemplate.Contains("Description") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });

            mList.Add(new ModuleFormLayout() { Title = "Problem Type", FieldDisplayName = "Problem Type", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", HideInTemplate = HideInTemplate.Contains("RequestTypeLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, FieldDisplayWidth = 1, FieldName = "OwnerUser", HideInTemplate = HideInTemplate.Contains("Owner") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, TabId = 1 });


            mList.Add(new ModuleFormLayout() { Title = "Functional Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("FunctionalAreaLookup") ? true : false, FieldDisplayName = "Functional Area" });
            mList.Add(new ModuleFormLayout() { Title = "Resolved on Initial Call", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InitiatorResolvedChoice", HideInTemplate = HideInTemplate.Contains("InitiatorResolvedChoice") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldDisplayName = "Resolved On Initial Call" });

            mList.Add(new ModuleFormLayout() { Title = "Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImpactLookup", HideInTemplate = HideInTemplate.Contains("ImpactLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldDisplayName = "Impact" });
            mList.Add(new ModuleFormLayout() { Title = "Severity", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SeverityLookup", HideInTemplate = HideInTemplate.Contains("SeverityLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldDisplayName = "Severity" });
            mList.Add(new ModuleFormLayout() { Title = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PriorityLookup", HideInTemplate = HideInTemplate.Contains("PriorityLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldDisplayName = "Priority" });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Attachments", HideInTemplate = HideInTemplate.Contains("Attachments") ? true : false, ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum, FieldDisplayName = "Attachment" });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsPrivate", HideInTemplate = HideInTemplate.Contains("IsPrivate") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldDisplayName = "Private" });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 2: Work Activity
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Primary Responsible Person (PRP)", FieldDisplayName = "Primary Responsible Person (PRP)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PRPUser", HideInTemplate = HideInTemplate.Contains("PRP") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Other Responsible (ORPs)", FieldDisplayName = "Other Responsible (ORPs)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ORPUser", HideInTemplate = HideInTemplate.Contains("ORP") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Estimated Hours", FieldDisplayName = "Estimated Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "EstimatedHours", HideInTemplate = HideInTemplate.Contains("EstimatedHours") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Estimated Completion Date", FieldDisplayName = "Estimated Completion Date", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "TargetCompletionDate", HideInTemplate = HideInTemplate.Contains("TargetCompletionDate") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Total Hold Time (mins)", FieldDisplayName = "Total Hold Time (mins)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "TotalHoldDuration", HideInTemplate = HideInTemplate.Contains("TotalHoldDuration") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Comment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "Comment", HideInTemplate = HideInTemplate.Contains("Comment") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Resolution", FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Actual Hours", FieldDisplayName = "Actual Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ActualHours", HideInTemplate = HideInTemplate.Contains("ActualHours") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Resolution Type", FieldDisplayName = "Resolution Type", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ResolutionTypeChoice", HideInTemplate = HideInTemplate.Contains("ResolutionType") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Actual Completion Date", FieldDisplayName = "Actual Completion Date", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ActualCompletionDate", HideInTemplate = HideInTemplate.Contains("ActualCompletionDate") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "% Complete", FieldDisplayName = "% Complete", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "PctComplete", HideInTemplate = HideInTemplate.Contains("PctComplete") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Resolution Description", FieldDisplayName = "Resolution Description", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "ResolutionComments", HideInTemplate = HideInTemplate.Contains("ResolutionComments") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });

            if (enableTestingStage)
            {
                mList.Add(new ModuleFormLayout() { Title = "Tester", FieldDisplayName = "Tester", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "TesterUser", HideInTemplate = HideInTemplate.Contains("Tester") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            }
            mList.Add(new ModuleFormLayout() { Title = "Resolution", FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            // Tab 3: Approvals
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            // Tab 4: Emails
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "Emails", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "TicketEmails", FieldDisplayName = "TicketEmails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "Emails", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Scheduled Escalations", FieldDisplayName = "Scheduled Escalations", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "ScheduleActions", FieldDisplayName = "ScheduleActions", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Scheduled Escalations", FieldDisplayName = "Scheduled Escalations", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Past Escalations", FieldDisplayName = "Past Escalations", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "ScheduleActionsArchive", FieldDisplayName = "ScheduleActionsArchive", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Past Escalations", FieldDisplayName = "Past Escalations", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            // Tab 5: Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "WikiRelatedTickets", FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            //Tab 6
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", HideInTemplate = HideInTemplate.Contains("#Control#") ? true : false, ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });
            // New ticket formFieldName =
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", HideInTemplate = HideInTemplate.Contains("Title") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Requested By", FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestorUser", HideInTemplate = HideInTemplate.Contains("TicketRequestor") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Location", FieldDisplayName = "Location", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LocationLookup", HideInTemplate = HideInTemplate.Contains("TicketDesiredCompletionDate") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Desired Completion Date", FieldDisplayName = "Desired Completion Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", HideInTemplate = HideInTemplate.Contains("IsPrivate") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Request Source", FieldDisplayName = "Request Source", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RequestSourceChoice", HideInTemplate = HideInTemplate.Contains("TicketRequestSource") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            //mList.Add(new ModuleFormLayout() { Title ="Related Asset", ModuleNameLookup = ModuleName, TabId = 0, "1", "AssetLookup", ShowInMobile=true, CustomProperties= "" , FieldSequence=++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Resolved on Initial Call", FieldDisplayName = "Resolved on Initial Call", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "InitiatorResolvedChoice", HideInTemplate = HideInTemplate.Contains("TicketInitiatorResolved") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", HideInTemplate = HideInTemplate.Contains("TicketDescription") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Problem Type", FieldDisplayName = "Problem Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", HideInTemplate = HideInTemplate.Contains("TicketRequestTypeLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", HideInTemplate = HideInTemplate.Contains("TicketOwner"), ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Resolution Description", FieldDisplayName = "Resolution Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "ResolutionComments", HideInTemplate = HideInTemplate.Contains("TicketResolutionComments") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "Actual Hours", FieldDisplayName = "Actual Hours", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ActualHours", HideInTemplate = HideInTemplate.Contains("TicketActualHours") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Impact", FieldDisplayName = "Impact", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImpactLookup", HideInTemplate = HideInTemplate.Contains("TicketImpactLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Severity", FieldDisplayName = "Severity", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SeverityLookup", HideInTemplate = HideInTemplate.Contains("TicketSeverityLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PriorityLookup", HideInTemplate = HideInTemplate.Contains("TicketPriorityLookup") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Attachments", HideInTemplate = HideInTemplate.Contains("Attachments") ? true : false, ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "IsPrivate", HideInTemplate = HideInTemplate.Contains("IsPrivate") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            string currTab = string.Empty;
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            List<string[]> dataList = new List<string[]>();

            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Agreement Summary - Application", "Non-ERP Applications"), RequestType = "Agreement Summary - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Agreement Summary - Technical", "Non-ERP Applications"), RequestType = "Agreement Summary - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "AS/400 Operations Problem", "Operations Support"), RequestType = "AS/400 Operations Problem", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Operations Support", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "ATT Managed Network", "Network"), RequestType = "ATT Managed Network", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Network", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Blackberry/Cell Phone", "Hardware"), RequestType = "Blackberry/Cell Phone", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Bugzilla - Application", "Other 3rd-party SW"), RequestType = "Bugzilla - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Bugzilla - Technical", "Other 3rd-party SW"), RequestType = "Bugzilla - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Citrix Issue", "Network"), RequestType = "Citrix Issue", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Network", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Data Center Room", "Operations Support"), RequestType = "Data Center Room", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Operations Support", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Data Tracker - Application", "BI"), RequestType = "Data Tracker - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Data Tracker - Technical", "BI"), RequestType = "Data Tracker - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Desktop Software", "Desktop Software"), RequestType = "Desktop Software", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Desktop Software", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Digital Dashboard - Application", "BI"), RequestType = "Digital Dashboard - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Digital Dashboard - Technical", "BI"), RequestType = "Digital Dashboard - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "DSRP - Application", "Non-ERP Applications"), RequestType = "DSRP - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "DSRP - Technical", "Non-ERP Applications"), RequestType = "DSRP - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "EAM", "JDEdwards"), RequestType = "EAM", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "EDI", "Non-ERP Applications"), RequestType = "EDI", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Exchange Server", "Hardware"), RequestType = "Exchange Server", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Hardware Problem - Infrastructure", "Hardware"), RequestType = "Hardware Problem - Infrastructure", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Hardware Problem - User", "Hardware"), RequestType = "Hardware Problem - User", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Internet - Technical", "Internet/Intranet"), RequestType = "Internet - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Internet/Intranet", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Intranet - Application", "Internet/Intranet"), RequestType = "Intranet - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Internet/Intranet", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Intranet - Technical", "Internet/Intranet"), RequestType = "Intranet - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Internet/Intranet", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "KRONOS - Application", "Non-ERP Applications"), RequestType = "KRONOS - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "KRONOS - Technical", "Non-ERP Applications"), RequestType = "KRONOS - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Labelview/Markum", "Other 3rd-party SW"), RequestType = "Labelview/Markum", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "LAN Problem", "Network"), RequestType = "LAN Problem", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Network", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Laser Vault", "Other 3rd-party SW"), RequestType = "Laser Vault", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Legacy Programming Problem", "Legacy"), RequestType = "Legacy Programming Problem", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Legacy", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Legacy Release Record", "Legacy"), RequestType = "Legacy Release Record", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Legacy", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Material Specifications - Application", "Non-ERP Applications"), RequestType = "Material Specifications - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Material Specifications - Technical", "Non-ERP Applications"), RequestType = "Material Specifications - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "On-Site Support", "On-Site Support"), RequestType = "On-Site Support", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "On-Site Support", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "OPS System Software", "Other 3rd-party SW"), RequestType = "OPS System Software", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Oracle/JDE/E1 - Application", "JDEdwards"), RequestType = "Oracle/JDE/E1 - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Oracle/JDE/E1 - Technical", "JDEdwards"), RequestType = "Oracle/JDE/E1 - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Outlook - Application", "Desktop Software"), RequestType = "Outlook - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Desktop Software", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Preactor - Application", "Other 3rd-party SW"), RequestType = "Preactor - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Preactor - Technical ", "Other 3rd-party SW"), RequestType = "Preactor - Technical ", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Printer Problem/AS400", "Hardware"), RequestType = "Printer Problem/AS400", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Printer Problem/Network", "Hardware"), RequestType = "Printer Problem/Network", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Printer/Personal", "Hardware"), RequestType = "Printer/Personal", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "QlikView - Application", "BI"), RequestType = "QlikView - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "QlikView - Technical", "BI"), RequestType = "QlikView - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Quick Review", "Other 3rd-party SW"), RequestType = "Quick Review", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "QuickR - Application", "Other 3rd-party SW"), RequestType = "QuickR - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "QuickR - Technical", "Other 3rd-party SW"), RequestType = "QuickR - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Reset password", "Operations Support"), RequestType = "Reset password", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Operations Support", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Reset password - JDE", "JDEdwards"), RequestType = "Reset password - JDE", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "RF Scanner", "Hardware"), RequestType = "RF Scanner", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "R&D Project Request - Application", "Non-ERP Applications"), RequestType = "R&D Project Request - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "R&D Project Request - Technical", "Non-ERP Applications"), RequestType = "R&D Project Request - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Sametime", "Other 3rd-party SW"), RequestType = "Sametime", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Service Tracking System", "Other 3rd-party SW"), RequestType = "Service Tracking System", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Storage Request Issue", "Network"), RequestType = "Storage Request Issue", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Network", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "STRAP - Application", "Non-ERP Applications"), RequestType = "STRAP - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "STRAP - Technical", "Non-ERP Applications"), RequestType = "STRAP - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Telephones (landline)", "Hardware"), RequestType = "Telephones (landline)", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Hardware", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "TMS - Application", "Non-ERP Applications"), RequestType = "TMS - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "TMS - Technical", "Non-ERP Applications"), RequestType = "TMS - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "TPMS (Account Review) - Application", "Non-ERP Applications"), RequestType = "TPMS (Account Review) - Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "TPMS (Account Review) - Technical", "Non-ERP Applications"), RequestType = "TPMS (Account Review) - Technical", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "VPN/Remote Access ", "Network"), RequestType = "VPN/Remote Access ", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Network", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Wan Issue", "Network"), RequestType = "Wan Issue", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Network", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Web-Conferencing", "Other 3rd-party SW"), RequestType = "Web-Conferencing", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            List<string[]> dataList = new List<string[]>();
            int moduleStep = 0;

            // All stages
            dataList.Add(new string[] { "Attachments", ModuleName, moduleStep.ToString(), "0", "0", "", "1", "" });
            dataList.Add(new string[] { "IsPrivate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            // dataList.Add(new string[] { "Comment", ModuleName, moduleStep.ToString(), "0", "0", "", "1", "" });

            // New , plus returned
            moduleStep++;
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "Description", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "RequestorUser", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "BusinessManagerUser", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "RequestSourceChoice", ModuleName, moduleStep.ToString(), "1", "1", "", "1", "" });
            dataList.Add(new string[] { "AssetLookup", ModuleName, moduleStep.ToString(), "0", "0", "", "1", "" });
            dataList.Add(new string[] { "RequestTypeLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "SeverityLookup", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "ImpactLookup", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "DesiredCompletionDate", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "FunctionalArea", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "HelpDeskCall", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "PriorityLookup", ModuleName, moduleStep.ToString(), "0", "1", "", "1", "" });
            dataList.Add(new string[] { "InitiatorResolvedChoice", ModuleName, moduleStep.ToString(), "1", "1", "", "1", "" });
            dataList.Add(new string[] { "ResolutionComments", ModuleName, moduleStep.ToString(), "1", "1", "", "1", "" });
            dataList.Add(new string[] { "ActualHours", ModuleName, moduleStep.ToString(), "1", "0", "", "1", "" });
            dataList.Add(new string[] { "LocationLookup", ModuleName, "1", "1", "0", "", "0", "" });

            // Stage 2 - Pending Assignment
            moduleStep++;
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "Description", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "RequestTypeLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "ImpactLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "SeverityLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "PRPUser", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "ORPUser", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "EstimatedHours", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "TargetCompletionDate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });

            // Stage 3 - Assigned
            moduleStep++;
            dataList.Add(new string[] { "PRPUser", ModuleName, moduleStep.ToString(), "1", "1", "OwnerUser;#PRPGroupUser", "0", "" });
            dataList.Add(new string[] { "ORPUser", ModuleName, moduleStep.ToString(), "0", "1", "OwnerUser;#PRPGroupUser", "0", "" });
            dataList.Add(new string[] { "TargetCompletionDate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "ActualHours", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "PctComplete", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "ActualCompletionDate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "ResolutionComments", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "ResolutionTypeChoice", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            if (enableTestingStage)
                dataList.Add(new string[] { "TesterUser", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });

            // Stage 4 - Testing
            if (enableTestingStage)
            {
                moduleStep++;
                dataList.Add(new string[] { "TesterUser", ModuleName, moduleStep.ToString(), "1", "1", "OwnerUser;#PRPUser", "0", "" });
            }

            // Stage 5 - Pending Close
            if (enablePendingCloseStage)
            {
                moduleStep++;
                dataList.Add(new string[] { "ResolutionComments", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            }


            foreach (string[] data in dataList)
            {
                ModuleRoleWriteAccess mRoleWriteAccess = new ModuleRoleWriteAccess();
                mRoleWriteAccess.Title = data[2] + " - " + data[0];
                mRoleWriteAccess.FieldName = data[0];
                mRoleWriteAccess.ModuleNameLookup = data[1];
                mRoleWriteAccess.StageStep = Convert.ToInt32(data[2]);
                mRoleWriteAccess.FieldMandatory = Convert.ToBoolean(Convert.ToInt32(data[3]));
                mRoleWriteAccess.ShowEditButton = Convert.ToBoolean(Convert.ToInt32(data[4]));
                mRoleWriteAccess.ShowWithCheckbox = Convert.ToBoolean(Convert.ToInt32("0"));
                //if (!string.IsNullOrEmpty(data[5]))
                //    mRoleWriteAccess.ActionUser = data[5];
                mRoleWriteAccess.HideInServiceMapping = Convert.ToBoolean(Convert.ToInt32(data[6]));
                mRoleWriteAccess.CustomProperties = data[7];
                mList.Add(mRoleWriteAccess);

            }
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  StatusMapping");

            List<string[]> dataList = new List<string[]>();

            int stageID = Convert.ToInt32(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            dataList.Add(new string[] { "Initiated", ModuleName, stageID.ToString(), "1" });
            dataList.Add(new string[] { "Pending Assignment", ModuleName, (++stageID).ToString(), "1" });
            dataList.Add(new string[] { "Assigned", ModuleName, (++stageID).ToString(), "2" });
            if (enableTestingStage)
                dataList.Add(new string[] { "Testing", ModuleName, (++stageID).ToString(), "2" });
            if (enablePendingCloseStage)
                dataList.Add(new string[] { "Pending Close", ModuleName, (++stageID).ToString(), "2" });
            dataList.Add(new string[] { "Closed", ModuleName, (++stageID).ToString(), "4" });
            foreach (string[] data in dataList)
            {
                ModuleStatusMapping mStatusMapping = new ModuleStatusMapping();
                mStatusMapping.Title = data[1] + " - " + data[0];
                mStatusMapping.ModuleNameLookup = data[1];
                mStatusMapping.StageTitleLookup = Convert.ToInt32(data[2]);
                mStatusMapping.GenericStatusLookup = Convert.ToInt32(data[3]);
                mList.Add(mStatusMapping);
            }
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");

            List<string[]> dataList = new List<string[]>();
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            dataList.Add(new string[] { "Initiated", "New PRS Ticket Initiated [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] has been initiated and needs clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.", ModuleName, stageID.ToString() , "TicketInitiator"});

            dataList.Add(new string[] { "Requestor Notification",  "New Ticket Created [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketIdWithoutLink$] has been created on your behalf.<br><br>" +
                                                        "Please review the details below. You will be notified when the ticket is resolved.", ModuleName, (stageID+1).ToString() , "TicketRequestor"});

            dataList.Add(new string[] { "Pending Assignment",  "New PRS Ticket Pending Assignment [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields", ModuleName, (++stageID).ToString(), "TicketOwner" });

            dataList.Add(new string[] { "Assigned",  "PRS Ticket Assigned [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] has been assigned to you for resolution.<br><br>" +
                                                        "Once resolved, please enter the resolution description and actual hours in the ticket, and assign a tester.", ModuleName, (++stageID).ToString(), "TicketPRP;#TicketORP"});

            if (enableTestingStage)
            {
                dataList.Add(new string[] { "Testing", "PRS Ticket Awaiting Testing [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] has been assigned to you for testing.<br><br>" +
                                                        "Please test the resolution and approve or reject the resolution.<br><br>[$IncludeActionButtons$]", ModuleName, (++stageID).ToString(), "TicketTester"});
            }

            if (enablePendingCloseStage)
            {
                dataList.Add(new string[] { "Pending Close", "PRS Ticket Pending Close [$TicketId$]: [$Title$]",  @"PRS Ticket ID [$TicketId$] has been resolved and is pending close.<br><br>" +
                                                "Please review the resolution and close the ticket.<br><br>[$IncludeActionButtons$]", ModuleName, (++stageID).ToString(), "TicketOwner"});
            }

            dataList.Add(new string[] { "Closed", "PRS Ticket Closed [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketId$] has been closed.", ModuleName, (stageID + 1).ToString(), "TicketOwner" });
            dataList.Add(new string[] { "Closed - Requestor", "Ticket Closed [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketIdWithoutLink$] has been closed.", ModuleName, (++stageID).ToString(), "TicketRequestor" });

            dataList.Add(new string[] { "On-Hold", "PRS Ticket On Hold [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketId$] has been placed on hold.", ModuleName, null, "TicketOwner" });
            dataList.Add(new string[] { "On-Hold - Requestor", "Ticket On Hold [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketIdWithoutLink$] has been placed on hold.", ModuleName, null, "TicketRequestor" });

            foreach (string[] data in dataList)
            {
                ModuleTaskEmail mTaskEmail = new ModuleTaskEmail();
                mTaskEmail.Title = data[3] + " - " + data[0];
                mTaskEmail.Status = data[0];
                mTaskEmail.EmailTitle = data[1];
                mTaskEmail.EmailBody = data[2];
                mTaskEmail.ModuleNameLookup = data[3];
                mTaskEmail.StageStep = Convert.ToInt32(data[4]);
                //mTaskEmail.EmailUserTypes = data[5];
                mList.Add(mTaskEmail);
            }
            return mList;
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

        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "PRS", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "PRS", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "PRS", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            return mList;
        }
        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            mList.Add(new ModuleImpact() { ModuleNameLookup = "PRS", Title = "Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "PRS", Title = "Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "PRS", Title = "Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            return mList;
        }
        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            // Console.WriteLine("  RequestPriority");
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 1 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 1, SeverityLookup = 3, PriorityLookup = 2 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 1 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 2, SeverityLookup = 3, PriorityLookup = 3 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 2 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 3, SeverityLookup = 2, PriorityLookup = 3 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 3, SeverityLookup = 3, PriorityLookup = 3 });
            return mList;
        }
        public List<ModuleSeverity> GetModuleSeverity()
        {
            // Console.WriteLine(" TicketSeverity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "PRS", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "PRS", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "PRS", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-Initiator", UserTypes = "Initiator", ColumnName = "Initiator", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-Requestor", UserTypes = "Requestor", ColumnName = "Requestor", ManagerOnly = false, ITOnly = false, CustomProperties = "DisableEmailTicketLink=true" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-Business Manager", UserTypes = "Business Approver", ColumnName = "BusinessManager", ManagerOnly = false, ITOnly = false, CustomProperties = "ManagerOf=TicketRequestor" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-Owner", UserTypes = "Owner", ColumnName = "Owner", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-PRP", UserTypes = "PRP", ColumnName = "PRP", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-ORP", UserTypes = "ORP", ColumnName = "ORP", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-Tester", UserTypes = "Tester", ColumnName = "Tester", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PRS", Title = "PRS-PRP Group", UserTypes = "PRP Group", ColumnName = "PRPGroup", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "PRS", TabOrder = 1, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "PRS", TabOrder = 2, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "PRS", TabOrder = 3, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "PRS", TabOrder = 4, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "PRS", TabOrder = 5, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "PRS", TabOrder = 6, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "PRS", TabOrder = 7, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "PRS", TabOrder = 8, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "PRS", TabOrder = 9, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "PRS", TabOrder = 10, ModuleNameLookup = "PRS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });

            return tabs;
        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "PRS", Title = "Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "PRS", Title = "Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "PRS", Title = "Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "PRS", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "PRS", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "PRS", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });

            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "PRS", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "PRS", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "PRS", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "PRS", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4, IsVIP = true });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 1, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 3 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 2, SeverityLookup = 0, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "PRS", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });

        }
    }
}
