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
    public class RCA : IModule
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
                    ShortName = "Root Cause Analysis",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "RCA",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/sitePages/rcatickets",
                    ModuleHoldMaxStage = 3,
                    Title = "Root Cause Analysis (RCA)",
                    //ModuleDescription = "This module is used to report a technical problem that requires IT intervention to fix. Used to report all technology problems including: desktop or laptop errors, printing difficulties, e-mail problems, network connection or system issues, cellular telephone trouble, software or application errors, and other pressing assistance requests.",
                    ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details. Or, select a ticket by clicking checkbox and clicking Actions button.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/rca",
                    ModuleType = ModuleType.SMS,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    AllowDelete = true,
                    UseInGlobalSearch = true,
                    KeepItemOpen = true,
                    ShowBottleNeckChart = true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "RCA";
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
            mList.Add(new ModuleFormTab() { TabName = "Work Activity", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Work Activity" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Related Wikis", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Wikis" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }
        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "RCA", Title = "1-High", uPriority = "1-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "RCA", Title = "2-Medium", uPriority = "2-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "RCA", Title = "3-Low", uPriority = "3-Low", ItemOrder = 3 });
            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            mList.Add(new ModuleImpact() { ModuleNameLookup = "RCA", Title = "RCA-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "RCA", Title = "RCA-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            //mList.Add(new ModuleImpact() { ModuleNameLookup = "BTS", Title = "BTS-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });
            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "RCA", Title = "RCA-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "RCA", Title = "RCA-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "RCA", Title = "RCA-PRP", UserTypes = "PRP", ColumnName = "PRPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "RCA", Title = "RCA-ORP", UserTypes = "ORP", ColumnName = "ORPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "RCA", Title = "RCA-PRP Group", UserTypes = "PRP Group", ColumnName = "PRPGroupUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
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
        public List<LifeCycleStage> GetLifeCycleStage()
        {

            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            List<string[]> dataList = new List<string[]>();

            // Start from ID 1
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 5;

            string closeStageID = (currStageID + numStages - 1).ToString();
            int StageStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Name = "Creation",
                StageTitle = "Creation",
                UserWorkflowStatus = "Create new Ticket",
                UserPrompt = "<b>&nbsp;Please complete the form to open a new Root Cause Analysis.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Initiated",
                ActionUser = "InitiatorUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Ticket Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Re-Submit",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });


            mList.Add(new LifeCycleStage()
            {
                Name = "Pending Assignment",
                StageTitle = "Pending Assignment",
                UserWorkflowStatus = "Awaiting Primary Responsible Person(PRP) assignment by Owner",
                UserPrompt = "<b>Owner:</b>&nbsp;Please assign a Primary Responsible Person (PRP) and the Estimated hours on work activity tab.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "PRP Assigned",
                ActionUser = "OwnerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "PRP/ORP Assigned",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Assign PRP",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 45,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            string assignedStageID = currStageID.ToString();

            mList.Add(new LifeCycleStage()
            {
                Name = "Analysis",
                StageTitle = "Analysis",
                UserWorkflowStatus = "Analysis",
                UserPrompt = "<b>PRP/ORP:</b>&nbsp;Please collect the tickets related to this issue in the collector and analyze the root cause.",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Analysis",
                ActionUser = "OwnerUser;#PRPUser;#ORPUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Analysis",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Analysis Done",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 50,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Name = "Implement Fix",
                StageTitle = "Implement Fix",
                UserWorkflowStatus = "Awaiting implementation of the recommendations",
                UserPrompt = "<b>Owner:</b>&nbsp;Please review the analysis & approve the fix implementation to close the ticket",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Owner Closed",
                ActionUser = "OwnerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Owner approved fix implementation & closed the ticket",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Name = "Closed",
                StageTitle = "Closed",
                UserWorkflowStatus = "Ticket is closed, but you can add comments or can be re-opened by Owner",
                UserPrompt = "Ticket Closed",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Closed",
                ActionUser = "OwnerUser",
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
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // RCA
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "Age", FieldDisplayName = "Age", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, SortOrder = 1, FieldSequence = ++seqNum }); // Sort 1

            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "CreationDate", FieldDisplayName = "Created On", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "ActualCompletionDate", FieldDisplayName = "Resolution Date", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "CloseDate", FieldDisplayName = "Closed On", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "StageActionUsersUser", FieldDisplayName = "Waiting On", IsDisplay = true, IsUseInWildCard = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "PRPUser", FieldDisplayName = "PRP", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "ORPUser", FieldDisplayName = "ORP", IsUseInWildCard = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "TesterUser", FieldDisplayName = "Tester", IsUseInWildCard = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "ActualHours", FieldDisplayName = "Actual Hours", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "ResolutionComments", FieldDisplayName = "Resolution", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "RCA", FieldName = "InitiatorResolved", FieldDisplayName = " Initiator Resolved", IsUseInWildCard = true, FieldSequence = ++seqNum });
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
                Title = ModuleName + " - " + "DesiredCompletionDate",
                KeyName = "DesiredCompletionDate",
                KeyValue = "TomorrowsDate",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Initiator",
                KeyName = "Initiator",
                KeyValue = "LoggedInUser",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "CreationDate",
                KeyName = "CreationDate",
                KeyValue = "TodaysDate",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Owner",
                KeyName = "Owner",
                KeyValue = "LoggedInUser",
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
            // Tab 1: General
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Status", FieldDisplayName = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Initiator", FieldDisplayName = "Initiator", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InitiatorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Initiated Date", FieldDisplayName = "Initiated Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CreationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "RCA Type", FieldDisplayName = "RCA Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RCATypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact", FieldDisplayName = "Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Collector", FieldDisplayName = "Collector", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Collector", FieldDisplayName = "Collector", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });


            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 2: Work Activity
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Primary Responsible Person (PRP)", FieldDisplayName = "Primary Responsible Person (PRP)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PRPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Other Responsible (ORPs)", FieldDisplayName = "Other Responsible (ORPs)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ORPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Analysis", FieldDisplayName = "Analysis", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Analysis Details", FieldDisplayName = "Analysis Details", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "AnalysisDetails", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Analysis", FieldDisplayName = "Analysis", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Discussion", FieldDisplayName = "Discussion", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Discussion", FieldDisplayName = "Discussion", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 3: Approvals
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 4: Related Wikis
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "WikiRelatedTickets", FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 5-History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

            // New ticket form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "RCA Type", FieldDisplayName = "RCA Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RCATypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact", FieldDisplayName = "Impact", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
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
            // Console.WriteLine("RequestRoleWriteAccess");
            int StageStep = 0;
            // All stages
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "IsPrivate", FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });

            // New ticket, plus returned
            StageStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "RCAType", FieldName = "RCATypeChoice", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ImpactLookup", FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });

            // Stage 2 - Pending Assignment
            StageStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "PriorityLookup", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ImpactLookup", FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "PRP", FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ORP", FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });

            // Stage 3 - Collection & Analysis
            StageStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "PRP", FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ORP", FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "TargetCompletionDate", FieldName = "TargetCompletionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ActualCompletionDate", FieldName = "ActualCompletionDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "ResolutionType", FieldName = "ResolutionTypeChoice", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "AnalysisDetails", FieldName = "AnalysisDetails", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });

            // Stage 5 - Implement Fix
            StageStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + "-" + "AnalysisDetails", FieldName = "AnalysisDetails", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, HideInServiceMapping = false, CustomProperties = "", ShowWithCheckbox = false });
            return mList;

        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {

            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("TicketStatusMapping");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1, Title = ModuleName + " - " + "Creation" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1, Title = ModuleName + " - " + "Assignment" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + " - " + "Collection" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + " - " + "Analysis" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + " - " + "Fix" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4, Title = ModuleName + " - " + "Closed" });
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {

            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Creation",
                Status = "Creation",
                EmailTitle = "New RCA Ticket Created [$TicketId$]: [$Title$]",
                EmailBody = @"RCA Ticket ID [$TicketId$] has been initiated and needs clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID
            });

            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Pending Assignment",
                Status = "Pending Assignment",
                EmailTitle = "RCA Ticket Pending Assignment [$TicketId$]: [$Title$]",
                EmailBody = @"RCA Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });

            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Collection & Analysis",
                Status = "Collection & Analysis",
                EmailTitle = "RCA Ticket Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"RCA Ticket ID [$TicketId$] has been assigned to you for collection & analysis.<br><br>" +
                                                        "Once complete, please enter the analysis in the ticket.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });

            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Implement Fix",
                Status = "Implement Fix",
                EmailTitle = "RCA Ticket Pending Implementation [$TicketId$]: [$Title$]",
                EmailBody = @"RCA Ticket ID [$TicketId$] has completed analysis and is pending fix implementation.<br><br>" +
                                                    "Please review the analysis, implement the recommendations and close the ticket.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });

            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Closed", Status = "Closed", EmailTitle = "RCA Ticket Closed [$TicketId$]: [$Title$]", EmailBody = "RCA Ticket ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (++stageID), EmailUserTypes = "TicketOwner" });

            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "On-Hold", Status = "On-Hold", EmailTitle = "RCA Ticket On Hold [$TicketId$]: [$Title$]", EmailBody = "RCA Ticket ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null, EmailUserTypes = "TicketOwner" });
            return mList;

        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "RCATickets", TabOrder = 1, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "RCATickets", TabOrder = 2, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Items", ViewName = "RCATickets", TabOrder = 3, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Items", ViewName = "RCATickets", TabOrder = 4, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Items", ViewName = "RCATickets", TabOrder = 5, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Deaprtment Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Items", ViewName = "RCATickets", TabOrder = 6, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Items", ViewName = "RCATickets", TabOrder = 7, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Items", ViewName = "RCATickets", TabOrder = 8, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "All Items " });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Items", ViewName = "RCATickets", TabOrder = 9, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "RCATickets", TabOrder = 10, ModuleNameLookup = "RCA", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "On Hold" });

            return tabs;
        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "RCA", Title = "RCA-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "RCA", Title = "RCA-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "RCA", Title = "RCA-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "RCA", Title = "1-High", uPriority = "1-High", ItemOrder = 1 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "RCA", Title = "2-Medium", uPriority = "2-Medium", ItemOrder = 2 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "RCA", Title = "3-Low", uPriority = "3-Low", ItemOrder = 3 });

        }
    }
}
