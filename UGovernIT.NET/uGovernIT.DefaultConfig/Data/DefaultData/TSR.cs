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
    public class TSR : IModule
    {
        public static string userId = string.Empty;
        public static string managerUserId = "1";
        public static string userName = string.Empty; // Obtained from userId in UGovernITDefault.Initialize()
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
        protected static bool enableTestingStage = true;
        protected static bool enablePendingCloseStage = true;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Technical Service Request",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "TSR",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/tsrtickets",
                    ModuleHoldMaxStage = 6,
                    Title = "Technical Service Request (TSR)",
                    //ModuleDescription = "This module is used to report technology issues that need IT intervention such as PC errors, printing errors, email issues, etc. or to request IT services such as new system account(s), procure equipment / software or request support. If not initiated by a Manager or above, certain types of requests may require Manager approval.",
                    ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details. Or, select a ticket by clicking checkbox and clicking Actions button.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/tsr",
                    ModuleType = ModuleType.SMS,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    AllowDelete=true,
                    EnableNewsOnHomePage=true,
                    EnableLayout=true,
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
                return "TSR";
            }
        }


        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");
            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Work Activity", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Work Activity" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            //mList.Add(new ModuleImpact() { ModuleNameLookup = "TSR", Title = "Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            //mList.Add(new ModuleImpact() { ModuleNameLookup = "TSR", Title = "Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            //mList.Add(new ModuleImpact() { ModuleNameLookup = "TSR", Title = "Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");
            currStageID = 0;
            // Start from ID 7
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 8;
            int moduleStep = 0;
            if (!enableSecurityApprovalStage)
                numStages--;
            if (!enableManagerApprovalStage)
                numStages--;
            if (!enableTestingStage)
                numStages--;
            if (!enablePendingCloseStage)
                numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage() { Action= "Initiated", Name= "Initiated", StageTitle= "Initiated", UserPrompt= "<b>&nbsp;Please complete the form to open a new Technical Service Request.</b>",
                StageStep= ++moduleStep, ModuleNameLookup= ModuleName, StageApprovedStatus= ++currStageID, StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID) ,
                StageReturnStatus = 0, ActionUser= "InitiatorUser;#OwnerUser;#RequestorUser;#Admin;#BusinessManagerUser", ApproveActionDescription = "Ticket Submitted", RejectActionDescription="", ReturnActionDescription="", StageApproveButtonName = "(Re)Submit",
                StageRejectedButtonName = "Cancel", StageReturnButtonName = "", StageTypeLookup=1, StageAllApprovalsRequired=false, StageWeight=5, ShortStageTitle="",
                CustomProperties= "QuickClose=true", SkipOnCondition="",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            if (enableManagerApprovalStage)
            {
                mList.Add(new LifeCycleStage()
                {
                    Action = "Manager Approval",
                    Name = "Manager Approval",
                    StageTitle = "Manager Approval",
                    UserWorkflowStatus = "Awaiting Business Manager Approval",
                    ActionUser = "BusinessManagerUser;#OwnerUser;#RequestorUser;#Admin;#InitiatorUser",
                    UserPrompt = "<b>Business Manager:</b>&nbsp;Please approve the ticket.",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                    StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                    ApproveActionDescription = "Business Manager approved request",
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
                    SkipOnCondition = "[RequestTypeWorkflow] = 'SkipApprovals'",
                    StageTypeChoice = "Manager Approval"
                });
            }

            if (enableSecurityApprovalStage)
            {
               
                mList.Add(new LifeCycleStage()
                {
                    Action = "Security Approval",
                    Name = "Security Approval",
                    StageTitle = "Security Approval",
                    ActionUser= "SecurityManagerUser;#InitiatorUser;#OwnerUser;#RequestorUser;#Admin;#BusinessManagerUser",
                    UserWorkflowStatus= "Awaiting Security manager's approval",
                    UserPrompt = "<b>Security Manager:</b>&nbsp;Please approve the ticket.",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                    StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                    ApproveActionDescription = "Security Manager approved request",
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
                    SkipOnCondition = "[RequestTypeCategory] <> 'Account Setup' || [RequestTypeWorkflow] = 'SkipApprovals'",
                    StageTypeChoice = ""
                });
            }

            string pendingAssignmentStageID = currStageID.ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "PRP Assigned",
                Name = "Pending Assignment",
                StageTitle = "Pending Assignment",
                UserPrompt = "<b>Owner:</b>&nbsp;Please assign a Primary Responsible Person (PRP) and the Estimated hours on work activity tab.",
                UserWorkflowStatus= "Awaiting Primary Responsible Person (PRP) assignment by Owner",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser= "OwnerUser;#PRPGroupUser",
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
                StageWeight = 20,
                ShortStageTitle = "",
                CustomProperties = "SelfAssign=true;#DoNotWaitForActionUser=true",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Request Completed",
                Name = "Assigned",
                StageTitle = "Assigned",
                UserPrompt = "<b>PRP:</b>&nbsp;Please enter Resolution Description and other required fields",
                UserWorkflowStatus= "Awaiting resolution by Primary Responsible Person (PRP)/Other Responsible Person (ORP)",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "PRPUser;#ORPUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PRP/ORP completed request",
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
                StageTypeChoice = "Assigned"
            });

            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            if (enableTestingStage)
            {
                mList.Add(new LifeCycleStage()
                {
                    Action = "Testing Complete",
                    Name = "Testing",
                    StageTitle = "Testing",
                    UserPrompt = "<b>Tester:</b>&nbsp;Please approve/reject the resolution after completing testing",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    ActionUser = "TesterUser",
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                    StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                    ApproveActionDescription = "Tester approved resolution",
                    RejectActionDescription = "",
                    ReturnActionDescription = "",
                    StageApproveButtonName = "Approve",
                    StageRejectedButtonName = "",
                    StageReturnButtonName = "Return",
                    StageTypeLookup = 3,
                    StageAllApprovalsRequired = false,
                    StageWeight = 10,
                    ShortStageTitle = "",
                    CustomProperties = "",
                    SkipOnCondition = "[RequestTypeWorkflow] = 'NoTest' || [Tester] = 'null'  || [Tester] = ''",
                    StageTypeChoice = "Tested"
                });
            }

            if (enablePendingCloseStage)
            {
                moduleTestedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

                mList.Add(new LifeCycleStage()
                {
                    Action = "Owner Closed",
                    Name = "Pending Close",
                    StageTitle = "Pending Close",
                    UserPrompt = "<b>Owner:</b>&nbsp;Please approve the resolution to close the ticket",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    ActionUser = "OwnerUser",
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                    StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                    ApproveActionDescription = "Owner approved resolution & closed ticket",
                    RejectActionDescription = "",
                    ReturnActionDescription = "",
                    StageApproveButtonName = "Approve",
                    StageRejectedButtonName = "",
                    StageReturnButtonName = "Return",
                    StageTypeLookup = 4,
                    StageAllApprovalsRequired = false,
                    StageWeight = 5,
                    ShortStageTitle = "",
                    CustomProperties = "",
                    SkipOnCondition = "",
                    StageTypeChoice = "Resolved"
                });
            }

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "Ticket Closed",
                UserWorkflowStatus= "Ticket is closed, but you can add comments or can be re-opened by Owner",
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

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;

            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Requestor",
                FieldName = "RequestorUser",
                FieldDisplayName = "Requested By",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "MultiUser",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Attachments",
                FieldName = "Attachments",
                FieldDisplayName = "",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = true,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
          
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - TicketId",
                FieldName = "TicketId",
                FieldDisplayName = "Ticket Id",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = false
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - SelfAssign",
                FieldName = "SelfAssign",
                FieldDisplayName = "",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Title",
                FieldName = "Title",
                FieldDisplayName = "Title",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Status",
                FieldName = "Status",
                FieldDisplayName = "Progress",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "ProgressBar",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - RequestTypeCategory",
                FieldName = "RequestTypeCategory",
                FieldDisplayName = "Category",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - RequestTypeLookup",
                FieldName = "RequestTypeLookup",
                FieldDisplayName = "Request Type",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - ModuleStepLookup",
                FieldName = "ModuleStepLookup",
                FieldDisplayName = "Status",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - PriorityLookup",
                FieldName = "PriorityLookup",
                FieldDisplayName = "Priority",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - CreationDate",
                FieldName = "CreationDate",
                FieldDisplayName = "Created On",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - CloseDate",
                FieldName = "CloseDate",
                FieldDisplayName = "Close On",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - DesiredCompletionDate",
                FieldName = "DesiredCompletionDate",
                FieldDisplayName = "Target Date",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Age",
                FieldName = "Age",
                FieldDisplayName = "Age",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
         
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - NextSLATime",
                FieldName = "NextSLATime",
                FieldDisplayName = "Step Due",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "SLADate",
                SortOrder = 1,
                IsAscending = true
            });
            //mList.Add(new ModuleColumn()
            //{
            //    CategoryName = "TSR",
            //    Title = "TSR - Requestor",
            //    FieldName = "Requestor",
            //    FieldDisplayName = "Requested By",
            //    FieldSequence = ++seqNum,
            //    IsDisplay = true,
            //    IsUseInWildCard = true,
            //    ShowInMobile = false,
            //    CustomProperties = "",
            //    DisplayForClosed = true,
            //    DisplayForReport = false,
            //    ColumnType = "MultiUser",
            //    SortOrder = 1,
            //    IsAscending = true
            //});
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Owner",
                FieldName = "OwnerUser",
                FieldDisplayName = "Owner",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "MultiUser",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - StageActionUsers",
                FieldName = "StageActionUsersUser",
                FieldDisplayName = "Waiting On",
                FieldSequence = ++seqNum,
                IsDisplay = true,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "fieldtype=multiuser",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "MultiUser",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - FunctionalAreaLookup",
                FieldName = "FunctionalAreaLookup",
                FieldDisplayName = "Functional Area",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - PRP",
                FieldName = "PRPUser",
                FieldDisplayName = "PRP",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = true,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - ORP",
                FieldName = "ORPUser",
                FieldDisplayName = "ORP",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "MultiUser",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - ID",
                FieldName = "ID",
                FieldDisplayName = "DBID",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - OnHold",
                FieldName = "OnHold",
                FieldDisplayName = "OnHold",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Initiator",
                FieldName = "InitiatorUser",
                FieldDisplayName = "Initiator",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - Tester",
                FieldName = "TesterUser",
                FieldDisplayName = "Tester",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "MultiUser",
                SortOrder = 1,
                IsAscending = true
            });
            mList.Add(new ModuleColumn()
            {
                CategoryName = "TSR",
                Title = "TSR - InitiatorResolved",
                FieldName = "InitiatorResolvedChoice",
                FieldDisplayName = "Initiator Resolved",
                FieldSequence = ++seqNum,
                IsDisplay = false,
                IsUseInWildCard = true,
                ShowInMobile = false,
                CustomProperties = "",
                DisplayForClosed = false,
                DisplayForReport = false,
                ColumnType = "",
                SortOrder = 1,
                IsAscending = true
            });
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            int startStageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            int pendingAssignmentStageID = startStageID + 3;
            if (!enableManagerApprovalStage)
                pendingAssignmentStageID--;
            if (!enableSecurityApprovalStage)
                pendingAssignmentStageID--;
            int assignedStageID = pendingAssignmentStageID + 1;
            int resolvedStageID = pendingAssignmentStageID + 2;

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
                Title = ModuleName + " - " + "InitiatorResolved",
                KeyName = "InitiatorResolvedChoice",
                KeyValue = "No",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
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
                Title = ModuleName + " - " + "ImpactLookup",
                KeyName = "ImpactLookup",
                KeyValue = Helper.getTableIDByModuleTitle(DatabaseObjects.Tables.TicketImpact, DatabaseObjects.Columns.Title + "='Impacts Individual' and " + DatabaseObjects.Columns.ModuleNameLookup + "='" + ModuleName + "'"),//"6",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "SeverityLookup",
                KeyName = "SeverityLookup",
                KeyValue = Helper.getTableIDByModuleTitle(DatabaseObjects.Tables.TicketSeverity, DatabaseObjects.Columns.Title + "='Low' and " + DatabaseObjects.Columns.ModuleNameLookup + "='" + ModuleName + "'"),//"6",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "LocationLookup",
                KeyName = "LocationLookup",
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
                Title = ModuleName + " - " + "RequestSource",
                KeyName = "RequestSource",
                KeyValue = "Phone",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "EstimatedHours",
                KeyName = "EstimatedHours",
                KeyValue = "0.5",
                ModuleStepLookup = pendingAssignmentStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "InitiatorResolved",
                KeyName = "InitiatorResolvedChoice",
                KeyValue = "No",
                ModuleStepLookup = assignedStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "PctComplete",
                KeyName = "PctComplete",
                KeyValue = "1",
                ModuleStepLookup = resolvedStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "ActualCompletionDate",
                KeyName = "ActualCompletionDate",
                KeyValue = "TodaysDate",
                ModuleStepLookup = resolvedStageID.ToString(),
                CustomProperties = ""
            });

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

            mList.Add(new ModuleFormLayout() { Title = "Initiator", FieldDisplayName = "Initiator", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "InitiatorUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Initiated Date", FieldDisplayName = "Creation Date", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CreationDate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Desired Completion Date", FieldDisplayName = "Desired Completion Date", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Requested By", FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Location", FieldDisplayName = "Location", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "LocationLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Request Source", FieldDisplayName = "Request Source", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestSourceChoice", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Business Manager", FieldDisplayName = "Business Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Security Approver", FieldDisplayName = "Security Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SecurityManagerUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Related Asset", FieldDisplayName = "Related Asset", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AssetLookup", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Request Type", FieldDisplayName = "Request Type", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Functional Area", FieldDisplayName = "Functional", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Resolved on Initial Call", FieldDisplayName = "Resolved on Initial Call", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "InitiatorResolvedChoice", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Billing Department", FieldDisplayName = "Department", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "DepartmentLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "GLCode", FieldDisplayName = "GL Code", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "GLCode", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "ImpactLookup", FieldDisplayName = "Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Severity", FieldDisplayName = "Severity", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SeverityLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Private", FieldDisplayName = "Private", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            #region Tab - 2
            // Tab 2: Work Activity
            seqNum = 0;
            #region assignment
            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Primary Responsible Person (PRP)", FieldDisplayName = "PRP", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "PRPUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Other Responsible (ORPs)", FieldDisplayName = "ORP", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "ORPUser", ShowInMobile = true, CustomProperties = "" });
            
            mList.Add(new ModuleFormLayout() { Title = "Estimated Hours", FieldDisplayName = "Estimated Hours", FieldName = "EstimatedHours", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Est. Completion Date", FieldDisplayName = "Target Completion Date", FieldName = "TargetCompletionDate", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Total Hold Time (mins)", FieldDisplayName = "Total Hold Duration", FieldName = "TotalHoldDuration", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment",ModuleNameLookup = ModuleName, TabId=2, FieldDisplayWidth = 3, FieldSequence = (++seqNum), FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            #endregion assignment
            #region Comments
            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3,  FieldName= "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3,  FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            #endregion Comments
            #region Resolution
            mList.Add(new ModuleFormLayout() { Title = "Resolution", FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldSequence = (++seqNum), FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Actual Hours", FieldDisplayName = "Actual Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldSequence = (++seqNum), FieldName = "ActualHours", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Resolution Type", FieldDisplayName = "Resolution Type", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldSequence = (++seqNum), FieldName = "ResolutionTypeChoice", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Actual Complete Date", FieldDisplayName = "Actual Complete Date", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ActualCompletionDate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "% Complete", FieldDisplayName = "% Complete", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldSequence = (++seqNum), FieldName = "PctComplete", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Resolution Description", FieldDisplayName = "Resolution Description", ModuleNameLookup = ModuleName, FieldSequence = (++seqNum), TabId = 2, FieldDisplayWidth = 2,  FieldName = "ResolutionComments", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });

            if (enableTestingStage)
            {
                // Must be on a line by itself since gets hidden for "NoTest" workflow type
                mList.Add(new ModuleFormLayout() { Title = "Tester (Skips Testing if left blank)", FieldDisplayName = "Tester", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "TesterUser", ShowInMobile = true, CustomProperties = "" });
                //dataList.Add(new string[] { "Tester (Skips Testing if left blank)", ModuleName, "2", "3", "Tester", "1", "" });
            }
            mList.Add(new ModuleFormLayout() { Title = "Resolution", FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            #endregion Resolution
            //dataList.Add(new string[] { "Resolution", ModuleName, "2", "3", "#GroupEnd#", "1", "" });

            // Sub-Tasks
            //dataList.Add(new string[] { "Sub-Tasks", ModuleName, "2", "3", "#GroupStart#", "1", "" });
            //dataList.Add(new string[] { "PreconditionList", ModuleName, "2", "3", "#Control#", "1", "" });
            //dataList.Add(new string[] { "Sub-Tasks", ModuleName, "2", "3", "#GroupEnd#", "1", "" });
            #endregion Tab - 2
           // Tab 4: Approvals 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });


            // Tab 5: Emails
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Scheduled Escalation", FieldDisplayName = "Scheduled Escalation", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "ScheduleActions", FieldDisplayName = "Schedule Actions", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Scheduled Escalation", FieldDisplayName = "Scheduled Escalation", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Past Escalation", FieldDisplayName = "Past Escalation", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "ScheduleActionsArchive", FieldDisplayName = "Schedule Actions Archive", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Past Escalation", FieldDisplayName = "Past Escalation", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 6: Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Custom Ticket Relationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Wiki Related Tickets", FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 7
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // New Ticket Form
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Requested By", FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Location", FieldDisplayName = "Location", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "LocationLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Request Source", FieldDisplayName = "Request Source", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestSourceChoice", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Business Manager", FieldDisplayName = "Business Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Related Asset", FieldDisplayName = "Related Asset", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AssetLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Desired Completion Date", FieldDisplayName = "Desired Completion Date", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Request Type", FieldDisplayName = "Request Type", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Resolved on Initial Call", FieldDisplayName = "Resolved on Initial Call", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "InitiatorResolvedChoice", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Private", FieldDisplayName = "Private", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Billing Department", FieldDisplayName = "Department", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "DepartmentLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "GL Code", FieldDisplayName = "GL Code", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "GLCode", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Resolution Description", FieldDisplayName = "Resolution Comments", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "ResolutionComments", ShowInMobile = true, CustomProperties = "", ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "Actual Hours", FieldDisplayName = "Actual Hours", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ActualHours", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Impact", FieldDisplayName = "Impact", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Severity", FieldDisplayName = "Severity", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SeverityLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
                  
            string currTab = string.Empty;
            return mList;
        }
       
        public List<ModuleRequestType> GetModuleRequestType()
        {
            //List<ModuleRequestType> mList = new List<ModuleRequestType>();
            /*
            List<ModuleRequestType> mSubList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");

            mSubList.Add(new ModuleRequestType() { Category = "Application Software", SubCategory = "JD Edwards" });
            mSubList.Add(new ModuleRequestType() { Category = "Application Software", SubCategory = "Salesforce.com" });
            mSubList.Add(new ModuleRequestType() { Category = "Application Software", SubCategory = "Oracle" });
            mSubList.Add(new ModuleRequestType() { Category = "Application Software", SubCategory = "SAP"});
            mSubList.Add(new ModuleRequestType() { Category = "Application Software", SubCategory = "Service Prime"});
            mSubList.Add(new ModuleRequestType() { Category = "Application Software", SubCategory = "3rd Party"});
            //Applications
            mSubList.Add(new ModuleRequestType() { Category = "Applications", SubCategory = "Cloud" });
            mSubList.Add(new ModuleRequestType() { Category = "Applications", SubCategory = "Digital Transformation" });
            mSubList.Add(new ModuleRequestType() { Category = "Applications", SubCategory = "Hybrid" });
            mSubList.Add(new ModuleRequestType() { Category = "Applications", SubCategory = "On-Prem" });
            //Data Centre
            mSubList.Add(new ModuleRequestType() { Category = "Data Center", SubCategory = "Security Systems" });
            mSubList.Add(new ModuleRequestType() { Category = "Data Center", SubCategory = "Backup Systems" });
            mSubList.Add(new ModuleRequestType() { Category = "Data Center", SubCategory = "System Software" });
            mSubList.Add(new ModuleRequestType() { Category = "Data Center", SubCategory = "Database Systems" });
            mSubList.Add(new ModuleRequestType() { Category = "Data Center", SubCategory = "Server"});
            mSubList.Add(new ModuleRequestType() { Category = "Data Center", SubCategory = "Storage"});
            //Desktop software
            mSubList.Add(new ModuleRequestType() { Category = "Desktop Software", SubCategory = "Adobe"});
            mSubList.Add(new ModuleRequestType() { Category = "Desktop Software", SubCategory = "Email/Outlook" });
            mSubList.Add(new ModuleRequestType() { Category = "Desktop Software", SubCategory = "Content Management" });
            mSubList.Add(new ModuleRequestType() { Category = "Desktop Software", SubCategory = "MS Office" });
            mSubList.Add(new ModuleRequestType() { Category = "Desktop Software", SubCategory = "Social Media" });
            mSubList.Add(new ModuleRequestType() { Category = "Desktop Software", SubCategory = "Other"});
            //Field Software
            mSubList.Add(new ModuleRequestType() { Category = "Field Software", SubCategory = "Production Scheduling" });
            mSubList.Add(new ModuleRequestType() { Category = "Field Software", SubCategory = "Quality Management" });
            mSubList.Add(new ModuleRequestType() { Category = "Field Software", SubCategory = "Route Management" });
            //Network
            mSubList.Add(new ModuleRequestType() { Category = "Network", SubCategory = "Internet" });
            mSubList.Add(new ModuleRequestType() { Category = "Network", SubCategory = "Router" });
            mSubList.Add(new ModuleRequestType() { Category = "Network", SubCategory = "LAN" });
            mSubList.Add(new ModuleRequestType() { Category = "Network", SubCategory = "Accelerator"});
            mSubList.Add(new ModuleRequestType() { Category = "Network", SubCategory = "Switch" });
            mSubList.Add(new ModuleRequestType() { Category = "Network", SubCategory = "VPN" });
            mSubList.Add(new ModuleRequestType() { Category = "Network", SubCategory = "WAN" });
            //Other
            mSubList.Add(new ModuleRequestType() { Category = "Other", SubCategory = "Other" });
            //Security
            mSubList.Add(new ModuleRequestType() { Category = "Security", SubCategory = "Websense" });
            mSubList.Add(new ModuleRequestType() { Category = "Security", SubCategory = "Access Management" });
            mSubList.Add(new ModuleRequestType() { Category = "Security", SubCategory = "Social Media" });
            mSubList.Add(new ModuleRequestType() { Category = "Security", SubCategory = "Content Management" });
            mSubList.Add(new ModuleRequestType() { Category = "Security", SubCategory = "Email" });
            mSubList.Add(new ModuleRequestType() { Category = "Security", SubCategory = "Firewall" });
            //User Device
            mSubList.Add(new ModuleRequestType() { Category = "User Devices", SubCategory = "Laptop"});
            mSubList.Add(new ModuleRequestType() { Category = "User Devices", SubCategory = "Other" });
            mSubList.Add(new ModuleRequestType() { Category = "User Devices", SubCategory = "Phone" });
            mSubList.Add(new ModuleRequestType() { Category = "User Devices", SubCategory = "Tablets" });
            mSubList.Add(new ModuleRequestType() { Category = "User Devices", SubCategory = "Printer" });
            mSubList.Add(new ModuleRequestType() { Category = "User Devices", SubCategory = "Smartphone" });
            mSubList.Add(new ModuleRequestType() { Category = "User Devices", SubCategory = "Desktop" });

            string[] applicationSoftwareReqTypes = { "Access", "Install/Uninstall", "Issue",  "Upgrade", "Other",};
            string[] dataCenterSoftwareReqTypes = { "Hardware Upgrade", "Software Upgrade", "Outage", "Performance", "Purchase", "Security", "Dispose", "Other" };
            string[] desktopSoftwareReqTypes = { "Access", "Install/Uninstall", "Performance", "Purchase", "Upgrade", "Other" };
            string[] networkReqTypes = { "Connectivity", "Performance", "Security", "Capacity", "Other" };
            string[] securityReqTypes = { "Hardware Upgrades", "Software Upgrades", "Outage", "Performance", "Purchase", "Security", "Email", "Other", "New User Setup" };
            string[] userDevicesReqTypes = { "Issue", "Broken", "Lost", "Virus Attack", "Performance", "Hardware Upgrades", "Software Upgrades", "Purchase", "Transfer", "Dispose", "Other", "Laptop-Presentation", "Laptop-Loaner", "End Point Discovery" };
            string[] taxPaymentsReqTypes = { "Ad Hoc", "Scheduled-Automated", "Scheduled-Manual", "Generate Tax File" };

            //newly added requesttypemodification
            string[] applicationSoftwareReqTypesFor3rdParty = { "TMS" };
            //Application
            string[] ApplicationsForCloud = { "Cloud" };
            string[] ApplicationsForDigitalTransformation = { "Digital Transformation" };
            string[] ApplicationsForHybrid = { "Hybrid" };
            string[] ApplicationsForOnPrem = { "On-Prem" };
            //DataCentre
            string[] dataCenterForSecuritySystem = { "Hardware Upgrade", "Outage", "Performance", "Purchase", "Security", "Dispose", "Other" };
            string[] dataCenterForServer = { "Software Upgrade", "Outage", "Performance", "Purchase", "Security", "Dispose", "Other" };
            // Field Software
            string [] FieldSoftwareForRequestType = { "Access", "Issue", "Upgrade" };
            // Network
            string [] NetworkRequestTypeForInternet = { "Access" };
            //Other
            string[] OtherRequestType = { "Other" };
            //Security 
            string [] SecurityRequestTypeForWebsense = { "Access" };
            string[] securityReqTypesAccessManagement = {"Account Setup", "Hardware Upgrades", "Software Upgrades", "Outage", "Performance", "Purchase", "Security", "Email", "Other", "New User Setup" };
            string [] securityReqTypesContentManagement = { "Hardware Upgrades", "Software Upgrades", "Outage", "Performance", "Purchase", "Security", "Email", "Other" };
            //User Devices
            //string[] userDevicesReqTypesOtherd = {  "Broken", "Dispose", "End Point Discovery" , "Hardware Upgrades", "Issue", "Laptop-Loaner", "Laptop-Presentation", "Lost", "Other", "Performance", "Purchase",   "Software Upgrades", "Transfer", "Virus Attack"};
            string[] userDevicesReqTypesOther = {  "Broken", "Dispose",  "Hardware Upgrades",  "Lost", "Other", "Performance", "Purchase",   "Software Upgrades", "Transfer", "Virus Attack"};
            string[] userDevicesReqTypesPhone = {  "Broken", "Dispose",  "Hardware Upgrades","Issue",  "Lost", "Other", "Performance", "Purchase",   "Software Upgrades", "Transfer", "Virus Attack"};
            string[] userDevicesReqTypesPrinter = {  "Broken", "Dispose",  "Hardware Upgrades", "Lost", "Other", "Performance", "Purchase",   "Software Upgrades", "Transfer", "Virus Attack"};
            string[] userDevicesReqTypesSmartPhone = {  "Broken",   "Hardware Upgrades","Issues", "Lost", "Other", "Performance", "Purchase",   "Software Upgrades", "Transfer"};
            string[] userDevicesReqTypesDeskTop = {  "Broken", "Dispose",  "Hardware Upgrades","Issues", "Lost", "Other", "Performance", "Purchase",   "Software Upgrades", "Transfer", "Virus Attack" };

            foreach (ModuleRequestType list in mSubList)
            {
                string category = list.Category;
                string subCategory = list.SubCategory;
                string functionalArea = string.Empty;
                string[] requestTypes = null;
                if (category == "Application Software")
                {
                    functionalArea = "1"; // Business Systems
                    requestTypes = applicationSoftwareReqTypes;
                    if (subCategory.Equals("3rd Party", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = applicationSoftwareReqTypesFor3rdParty;
                    }
                    //if (subCategory.Equals("JD Edwards", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    string[] applicationSoftwareReqTypesForJDEdwards = { "Access", "Install/Uninstall", "Issue", "Upgrade", "Other", };
                    //    requestTypes = applicationSoftwareReqTypesForJDEdwards;
                    //}
                    //if(subCategory.Equals("JD Edwards", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    string[] applicationSoftwareReqTypesForJDEdwards = { "Access", "Install/Uninstall", "Issue", "Upgrade", "Other", };
                    //    requestTypes = applicationSoftwareReqTypesForJDEdwards;

                    //}
                }
                else if (category == "Applications")
                {
                    functionalArea = "3"; // Production Services
                    if (subCategory.Equals("Cloud", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = ApplicationsForCloud;
                    }
                    if (subCategory.Equals("Digital Transformation", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = ApplicationsForDigitalTransformation;
                    }
                    if (subCategory.Equals("Hybrid", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = ApplicationsForHybrid;
                    }
                    if (subCategory.Equals("On-Prem", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = ApplicationsForOnPrem;
                    }

                }
                else if (category == "Data Center")
                {
                    functionalArea = "3"; // Production Services
                    requestTypes = dataCenterSoftwareReqTypes;
                    if (subCategory.Equals("Security Systems", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = dataCenterForSecuritySystem;
                    }
                    if (subCategory.Equals("Server", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = dataCenterForServer;
                    }
                }
                else if (category == "Desktop Software")
                {
                    functionalArea = "5"; // User Services
                    requestTypes = desktopSoftwareReqTypes;
                }
                else if (category == "Field Software")
                {
                    functionalArea = "1"; // Business Systems
                    requestTypes = FieldSoftwareForRequestType;
                }
                else if (category == "Network")
                {
                    functionalArea = "4"; // Technical Services
                    requestTypes = networkReqTypes;
                    if (subCategory.Equals("Internet", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = NetworkRequestTypeForInternet;
                    }

                }
                else if (category == "Other")
                {
                    functionalArea = "4";
                    requestTypes = OtherRequestType;
                }
                else if (category == "Security")
                {
                    functionalArea = "4"; // Technical Services
                    requestTypes = securityReqTypes;
                    if (subCategory.Equals("Websense", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = SecurityRequestTypeForWebsense;
                    }
                    if (subCategory.Equals("Access Management", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = securityReqTypesAccessManagement;
                    }
                    if (subCategory.Equals("Content Management", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = securityReqTypesContentManagement;
                    }
                }
                else if (category == "User Devices")
                {
                    functionalArea = "5"; // User Services
                    requestTypes = userDevicesReqTypes;
                    if (subCategory.Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = userDevicesReqTypesOther;
                    }
                    if (subCategory.Equals("Phone", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = userDevicesReqTypesPhone;
                    }
                    if (subCategory.Equals("Printer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = userDevicesReqTypesPrinter;
                    }
                    if (subCategory.Equals("Smartphone", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = userDevicesReqTypesSmartPhone;
                    }
                    if (subCategory.Equals("DeskTop", StringComparison.InvariantCultureIgnoreCase))
                    {
                        requestTypes = userDevicesReqTypesDeskTop;
                    }
                }
                
                else
                {
                    // Console.WriteLine("**** ERROR: UNKNOWN CATEGORY ****");
                    //return;
                }
                foreach (string requestType in requestTypes)
                {
                    ModuleRequestType obj = new ModuleRequestType();
                    obj.Title = string.Format("{0} > {1} > {2}", category, subCategory, requestType);
                    obj.ModuleNameLookup = ModuleName;
                    obj.Category = category;
                    obj.SubCategory = subCategory;
                    obj.RequestType = requestType;
                    obj.RequestCategory = "Ticketing Support";
                    obj.Owner = ConfigData.Variables.Name;
                    obj.FunctionalAreaLookup = Convert.ToInt32(functionalArea);
                    //mRequestType.PRPGroup = userValue;
                    if (list.RequestType == "Access")
                        obj.WorkflowType = "Access";
                    else if (requestType == "Purchase" || requestType.Contains("Upgrade"))
                        obj.WorkflowType = "Full";
                    else
                        obj.WorkflowType = "SkipApprovals";
                    obj.SortToBottom = (requestType == "Other");
                    obj.IsDeleted = false;
                    mList.Add(obj);
                }
               
            }
            */

            List<ModuleRequestType> mList = new List<ModuleRequestType>();

            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "3rd Party Tool", "Access"), Category = "Business Applications", SubCategory = "3rd Party Tool", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "3rd Party Tool", "Install/Uninstall"), Category = "Business Applications", SubCategory = "3rd Party Tool", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "3rd Party Tool", "Issues"), Category = "Business Applications", SubCategory = "3rd Party Tool", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "3rd Party Tool", "Other"), Category = "Business Applications", SubCategory = "3rd Party Tool", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "3rd Party Tool", "Upgrade"), Category = "Business Applications", SubCategory = "3rd Party Tool", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "BI/Analytical", "Access"), Category = "Business Applications", SubCategory = "BI/Analytical", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "BI/Analytical", "Install/Uninstall"), Category = "Business Applications", SubCategory = "BI/Analytical", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "BI/Analytical", "Issues"), Category = "Business Applications", SubCategory = "BI/Analytical", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "BI/Analytical", "Other"), Category = "Business Applications", SubCategory = "BI/Analytical", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "BI/Analytical", "Upgrade"), Category = "Business Applications", SubCategory = "BI/Analytical", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Collaboration", "Access"), Category = "Business Applications", SubCategory = "Collaboration", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Collaboration", "Install/Uninstall"), Category = "Business Applications", SubCategory = "Collaboration", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Collaboration", "Issues"), Category = "Business Applications", SubCategory = "Collaboration", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Collaboration", "Other"), Category = "Business Applications", SubCategory = "Collaboration", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Collaboration", "Upgrade"), Category = "Business Applications", SubCategory = "Collaboration", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "CRM", "Access"), Category = "Business Applications", SubCategory = "CRM", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "CRM", "Install/Uninstall"), Category = "Business Applications", SubCategory = "CRM", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "CRM", "Issues"), Category = "Business Applications", SubCategory = "CRM", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "CRM", "Other"), Category = "Business Applications", SubCategory = "CRM", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "CRM", "Upgrade"), Category = "Business Applications", SubCategory = "CRM", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Custom Solutions", "Access"), Category = "Business Applications", SubCategory = "Custom Solutions", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Custom Solutions", "Install/Uninstall"), Category = "Business Applications", SubCategory = "Custom Solutions", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Custom Solutions", "Issues"), Category = "Business Applications", SubCategory = "Custom Solutions", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Custom Solutions", "Other"), Category = "Business Applications", SubCategory = "Custom Solutions", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Custom Solutions", "Upgrade"), Category = "Business Applications", SubCategory = "Custom Solutions", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "ERP", "Access"), Category = "Business Applications", SubCategory = "ERP", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "ERP", "Install/Uninstall"), Category = "Business Applications", SubCategory = "ERP", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "ERP", "Issues"), Category = "Business Applications", SubCategory = "ERP", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "ERP", "Other"), Category = "Business Applications", SubCategory = "ERP", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "ERP", "Upgrade"), Category = "Business Applications", SubCategory = "ERP", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Legacy", "Access"), Category = "Business Applications", SubCategory = "Legacy", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Legacy", "Install/Uninstall"), Category = "Business Applications", SubCategory = "Legacy", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Legacy", "Issues"), Category = "Business Applications", SubCategory = "Legacy", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Legacy", "Other"), Category = "Business Applications", SubCategory = "Legacy", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Business Applications", "Legacy", "Upgrade"), Category = "Business Applications", SubCategory = "Legacy", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "Application Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Dispose"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Hardware Upgrades"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Other"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Outage"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Performance"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Purchase"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Security"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Backup Systems", "Software Upgrades"), Category = "Data Center", SubCategory = "Backup Systems", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Dispose"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Hardware Upgrades"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Other"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Outage"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Performance"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Purchase"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Security"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Security Systems", "Software Upgrades"), Category = "Data Center", SubCategory = "Security Systems", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Dispose"), Category = "Data Center", SubCategory = "Servers", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Hardware Upgrades"), Category = "Data Center", SubCategory = "Servers", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Other"), Category = "Data Center", SubCategory = "Servers", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Outage"), Category = "Data Center", SubCategory = "Servers", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Performance"), Category = "Data Center", SubCategory = "Servers", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Purchase"), Category = "Data Center", SubCategory = "Servers", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Security"), Category = "Data Center", SubCategory = "Servers", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Servers", "Software Upgrades"), Category = "Data Center", SubCategory = "Servers", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Dispose"), Category = "Data Center", SubCategory = "Storage", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Hardware Upgrades"), Category = "Data Center", SubCategory = "Storage", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Other"), Category = "Data Center", SubCategory = "Storage", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Outage"), Category = "Data Center", SubCategory = "Storage", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Performance"), Category = "Data Center", SubCategory = "Storage", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Purchase"), Category = "Data Center", SubCategory = "Storage", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Security"), Category = "Data Center", SubCategory = "Storage", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "Storage", "Software Upgrades"), Category = "Data Center", SubCategory = "Storage", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Dispose"), Category = "Data Center", SubCategory = "System Software", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Hardware Upgrades"), Category = "Data Center", SubCategory = "System Software", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Other"), Category = "Data Center", SubCategory = "System Software", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Outage"), Category = "Data Center", SubCategory = "System Software", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Performance"), Category = "Data Center", SubCategory = "System Software", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Purchase"), Category = "Data Center", SubCategory = "System Software", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Security"), Category = "Data Center", SubCategory = "System Software", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Data Center", "System Software", "Software Upgrades"), Category = "Data Center", SubCategory = "System Software", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Infrastructure" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Content Management", "Access"), Category = "Desktop Software", SubCategory = "Content Management", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Content Management", "Install/Uninstall"), Category = "Desktop Software", SubCategory = "Content Management", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Content Management", "Other"), Category = "Desktop Software", SubCategory = "Content Management", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Content Management", "Performance"), Category = "Desktop Software", SubCategory = "Content Management", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Content Management", "Purchase"), Category = "Desktop Software", SubCategory = "Content Management", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Content Management", "Upgrade"), Category = "Desktop Software", SubCategory = "Content Management", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Email", "Access"), Category = "Desktop Software", SubCategory = "Email", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Email", "Install/Uninstall"), Category = "Desktop Software", SubCategory = "Email", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Email", "Other"), Category = "Desktop Software", SubCategory = "Email", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Email", "Performance"), Category = "Desktop Software", SubCategory = "Email", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Email", "Purchase"), Category = "Desktop Software", SubCategory = "Email", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Email", "Upgrade"), Category = "Desktop Software", SubCategory = "Email", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Other", "Access"), Category = "Desktop Software", SubCategory = "Other", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Other", "Install/Uninstall"), Category = "Desktop Software", SubCategory = "Other", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Other", "Other"), Category = "Desktop Software", SubCategory = "Other", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Other", "Performance"), Category = "Desktop Software", SubCategory = "Other", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Other", "Purchase"), Category = "Desktop Software", SubCategory = "Other", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Other", "Upgrade"), Category = "Desktop Software", SubCategory = "Other", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "PDF Reader", "Access"), Category = "Desktop Software", SubCategory = "PDF Reader", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "PDF Reader", "Install/Uninstall"), Category = "Desktop Software", SubCategory = "PDF Reader", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "PDF Reader", "Other"), Category = "Desktop Software", SubCategory = "PDF Reader", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "PDF Reader", "Performance"), Category = "Desktop Software", SubCategory = "PDF Reader", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "PDF Reader", "Purchase"), Category = "Desktop Software", SubCategory = "PDF Reader", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "PDF Reader", "Upgrade"), Category = "Desktop Software", SubCategory = "PDF Reader", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Productivity Suite", "Access"), Category = "Desktop Software", SubCategory = "Productivity Suite", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Productivity Suite", "Install/Uninstall"), Category = "Desktop Software", SubCategory = "Productivity Suite", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Productivity Suite", "Other"), Category = "Desktop Software", SubCategory = "Productivity Suite", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Productivity Suite", "Performance"), Category = "Desktop Software", SubCategory = "Productivity Suite", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Productivity Suite", "Purchase"), Category = "Desktop Software", SubCategory = "Productivity Suite", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Productivity Suite", "Upgrade"), Category = "Desktop Software", SubCategory = "Productivity Suite", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Social Media", "Access"), Category = "Desktop Software", SubCategory = "Social Media", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Social Media", "Install/Uninstall"), Category = "Desktop Software", SubCategory = "Social Media", RequestType = "Install/Uninstall", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Social Media", "Other"), Category = "Desktop Software", SubCategory = "Social Media", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Social Media", "Performance"), Category = "Desktop Software", SubCategory = "Social Media", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Social Media", "Purchase"), Category = "Desktop Software", SubCategory = "Social Media", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Desktop Software", "Social Media", "Upgrade"), Category = "Desktop Software", SubCategory = "Social Media", RequestType = "Upgrade", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Accelerators", "Capacity"), Category = "Network", SubCategory = "Accelerators", RequestType = "Capacity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Accelerators", "Connectivity"), Category = "Network", SubCategory = "Accelerators", RequestType = "Connectivity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Accelerators", "Other"), Category = "Network", SubCategory = "Accelerators", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Accelerators", "Performance"), Category = "Network", SubCategory = "Accelerators", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Accelerators", "Security"), Category = "Network", SubCategory = "Accelerators", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Internet", "Access"), Category = "Network", SubCategory = "Internet", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "LAN", "Capacity"), Category = "Network", SubCategory = "LAN", RequestType = "Capacity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "LAN", "Connectivity"), Category = "Network", SubCategory = "LAN", RequestType = "Connectivity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "LAN", "Other"), Category = "Network", SubCategory = "LAN", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "LAN", "Performance"), Category = "Network", SubCategory = "LAN", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "LAN", "Security"), Category = "Network", SubCategory = "LAN", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Routers", "Capacity"), Category = "Network", SubCategory = "Routers", RequestType = "Capacity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Routers", "Connectivity"), Category = "Network", SubCategory = "Routers", RequestType = "Connectivity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Routers", "Other"), Category = "Network", SubCategory = "Routers", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Routers", "Performance"), Category = "Network", SubCategory = "Routers", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Routers", "Security"), Category = "Network", SubCategory = "Routers", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Switches", "Capacity"), Category = "Network", SubCategory = "Switches", RequestType = "Capacity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Switches", "Connectivity"), Category = "Network", SubCategory = "Switches", RequestType = "Connectivity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Switches", "Other"), Category = "Network", SubCategory = "Switches", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Switches", "Performance"), Category = "Network", SubCategory = "Switches", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "Switches", "Security"), Category = "Network", SubCategory = "Switches", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "VPN", "Capacity"), Category = "Network", SubCategory = "VPN", RequestType = "Capacity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "VPN", "Connectivity"), Category = "Network", SubCategory = "VPN", RequestType = "Connectivity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "VPN", "Other"), Category = "Network", SubCategory = "VPN", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "VPN", "Performance"), Category = "Network", SubCategory = "VPN", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "VPN", "Security"), Category = "Network", SubCategory = "VPN", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "WAN", "Capacity"), Category = "Network", SubCategory = "WAN", RequestType = "Capacity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "WAN", "Connectivity"), Category = "Network", SubCategory = "WAN", RequestType = "Connectivity", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "WAN", "Other"), Category = "Network", SubCategory = "WAN", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "WAN", "Performance"), Category = "Network", SubCategory = "WAN", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Network", "WAN", "Security"), Category = "Network", SubCategory = "WAN", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Network Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Account Setup"), Category = "Security", SubCategory = "Access Management", RequestType = "Account Setup", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Email"), Category = "Security", SubCategory = "Access Management", RequestType = "Email", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Hardware Upgrades"), Category = "Security", SubCategory = "Access Management", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "New User Setup"), Category = "Security", SubCategory = "Access Management", RequestType = "New User Setup", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Other"), Category = "Security", SubCategory = "Access Management", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Outage"), Category = "Security", SubCategory = "Access Management", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Performance"), Category = "Security", SubCategory = "Access Management", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Purchase"), Category = "Security", SubCategory = "Access Management", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Security"), Category = "Security", SubCategory = "Access Management", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Access Management", "Software Upgrades"), Category = "Security", SubCategory = "Access Management", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Email"), Category = "Security", SubCategory = "Content Management", RequestType = "Email", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Hardware Upgrades"), Category = "Security", SubCategory = "Content Management", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Other"), Category = "Security", SubCategory = "Content Management", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Outage"), Category = "Security", SubCategory = "Content Management", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Performance"), Category = "Security", SubCategory = "Content Management", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Purchase"), Category = "Security", SubCategory = "Content Management", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Security"), Category = "Security", SubCategory = "Content Management", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Management", "Software Upgrades"), Category = "Security", SubCategory = "Content Management", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Content Scan/Block", "Access"), Category = "Security", SubCategory = "Content Scan/Block", RequestType = "Access", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Hardware Upgrades"), Category = "Security", SubCategory = "Email", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Issues"), Category = "Security", SubCategory = "Email", RequestType = "Issues", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Other"), Category = "Security", SubCategory = "Email", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Outage"), Category = "Security", SubCategory = "Email", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Performance"), Category = "Security", SubCategory = "Email", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Purchase"), Category = "Security", SubCategory = "Email", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Security"), Category = "Security", SubCategory = "Email", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Email", "Software Upgrades"), Category = "Security", SubCategory = "Email", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Email"), Category = "Security", SubCategory = "Firewalls", RequestType = "Email", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Hardware Upgrades"), Category = "Security", SubCategory = "Firewalls", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Other"), Category = "Security", SubCategory = "Firewalls", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Outage"), Category = "Security", SubCategory = "Firewalls", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Performance"), Category = "Security", SubCategory = "Firewalls", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Purchase"), Category = "Security", SubCategory = "Firewalls", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Security"), Category = "Security", SubCategory = "Firewalls", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Firewalls", "Software Upgrades"), Category = "Security", SubCategory = "Firewalls", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Email"), Category = "Security", SubCategory = "Social Media", RequestType = "Email", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Hardware Upgrades"), Category = "Security", SubCategory = "Social Media", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Other"), Category = "Security", SubCategory = "Social Media", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Outage"), Category = "Security", SubCategory = "Social Media", RequestType = "Outage", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Performance"), Category = "Security", SubCategory = "Social Media", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Purchase"), Category = "Security", SubCategory = "Social Media", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Security"), Category = "Security", SubCategory = "Social Media", RequestType = "Security", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "Security", "Social Media", "Software Upgrades"), Category = "Security", SubCategory = "Social Media", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Security Management" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Broken"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Broken", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Dispose"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "End Point Discovery"), Category = "User Devices", SubCategory = "Desktops", RequestType = "End Point Discovery", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Hardware Upgrades"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Lost"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Lost", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Other"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Performance"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Purchase"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Rebuild"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Rebuild", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Software Upgrades"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Transfer"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Transfer", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Desktops", "Virus Attack"), Category = "User Devices", SubCategory = "Desktops", RequestType = "Virus Attack", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Broken"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Broken", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Dispose"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Hardware Upgrades"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Laptop-Loaner"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Laptop-Loaner", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Laptop-Presentation"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Laptop-Presentation", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Lost"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Lost", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Other"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Performance"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Purchase"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Software Upgrades"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Transfer"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Transfer", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Laptops", "Virus Attack"), Category = "User Devices", SubCategory = "Laptops", RequestType = "Virus Attack", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Broken"), Category = "User Devices", SubCategory = "Other", RequestType = "Broken", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Dispose"), Category = "User Devices", SubCategory = "Other", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Hardware Upgrades"), Category = "User Devices", SubCategory = "Other", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Lost"), Category = "User Devices", SubCategory = "Other", RequestType = "Lost", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Other"), Category = "User Devices", SubCategory = "Other", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Performance"), Category = "User Devices", SubCategory = "Other", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Purchase"), Category = "User Devices", SubCategory = "Other", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Software Upgrades"), Category = "User Devices", SubCategory = "Other", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Transfer"), Category = "User Devices", SubCategory = "Other", RequestType = "Transfer", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Other", "Virus Attack"), Category = "User Devices", SubCategory = "Other", RequestType = "Virus Attack", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Broken"), Category = "User Devices", SubCategory = "Phones", RequestType = "Broken", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Dispose"), Category = "User Devices", SubCategory = "Phones", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Hardware Upgrades"), Category = "User Devices", SubCategory = "Phones", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Lost"), Category = "User Devices", SubCategory = "Phones", RequestType = "Lost", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Other"), Category = "User Devices", SubCategory = "Phones", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Performance"), Category = "User Devices", SubCategory = "Phones", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Purchase"), Category = "User Devices", SubCategory = "Phones", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Software Upgrades"), Category = "User Devices", SubCategory = "Phones", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Transfer"), Category = "User Devices", SubCategory = "Phones", RequestType = "Transfer", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Phones", "Virus Attack"), Category = "User Devices", SubCategory = "Phones", RequestType = "Virus Attack", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Broken"), Category = "User Devices", SubCategory = "Printers", RequestType = "Broken", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Dispose"), Category = "User Devices", SubCategory = "Printers", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Hardware Upgrades"), Category = "User Devices", SubCategory = "Printers", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Lost"), Category = "User Devices", SubCategory = "Printers", RequestType = "Lost", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Other"), Category = "User Devices", SubCategory = "Printers", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Performance"), Category = "User Devices", SubCategory = "Printers", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Purchase"), Category = "User Devices", SubCategory = "Printers", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Software Upgrades"), Category = "User Devices", SubCategory = "Printers", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Transfer"), Category = "User Devices", SubCategory = "Printers", RequestType = "Transfer", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Printers", "Virus Attack"), Category = "User Devices", SubCategory = "Printers", RequestType = "Virus Attack", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Broken"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Broken", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Dispose"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Hardware Upgrades"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Lost"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Lost", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Other"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Performance"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Purchase"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Software Upgrades"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Transfer"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Transfer", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Smart Phones", "Virus Attack"), Category = "User Devices", SubCategory = "Smart Phones", RequestType = "Virus Attack", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Broken"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Broken", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Dispose"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Dispose", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Hardware Upgrades"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Hardware Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Lost"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Lost", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Other"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Performance"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Performance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Purchase"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Purchase", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Software Upgrades"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Software Upgrades", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Requisition", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Transfer"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Transfer", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "IT Desktop" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1} > {2}", "User Devices", "Tablets", "Virus Attack"), Category = "User Devices", SubCategory = "Tablets", RequestType = "Virus Attack", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "SkipApprovals", Owner = "Admin", PRPGroup = "IT Desktop" });
            
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            
            int moduleStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory=false, ShowEditButton=false, ShowWithCheckbox=false, HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            // dataList.Add(new string[] { "TicketComment", ModuleName StageStep = moduleStep, "0", "0", "", "1", "" });

            // Stage 1 - New ticket
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false,HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DepartmentLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "GLCode", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SecurityManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false,HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FunctionalAreaLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false,HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HelpDeskCall", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false,HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false,HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestSourceChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false,HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "InitiatorResolvedChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false,HideInServiceMapping=true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName=  "UGITIssueType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName=  "PRPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            // Stage 2 - Manager approval
            if (enableManagerApprovalStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "UGITIssueType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName=  "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowWithCheckbox = false, HideInServiceMapping =true });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "DepartmentLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "GLCode", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            }

            // Stage 3 - Security Approval (no fields)
            if (enableSecurityApprovalStage)
                moduleStep++;

            // Stage 4 - Pending Assignment
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UGITIssueType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DepartmentLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "GLCode", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstimatedHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TargetCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            // Stage 5 - Assigned
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser= "OwnerUser;#PRPGroupUser", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser= "OwnerUser;#PRPGroupUser", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TargetCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PctComplete", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionTypeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            if (enableTestingStage)
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "TesterUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
           // mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketRequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
           // mList.Add(new ModuleRoleWriteAccess() { FieldName = "UGITIssueType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            // Stage 6 - Testing
            if (enableTestingStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false ,ShowWithCheckbox=false,HideInServiceMapping=true });
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "TesterUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true,ActionUser= "OwnerUser;#PRPUser", ShowWithCheckbox = false });
            }

            // Stage 7 - Pending Close
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
            // Console.WriteLine("StatusMapping");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1, Title = ModuleName + "-" + "Initiated" });
            if (enableManagerApprovalStage)
                mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1, Title = ModuleName + "-" + "Manager Approval" });
            if (enableSecurityApprovalStage)
                mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1, Title = ModuleName + "-" + "Security Approval" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1, Title = ModuleName + "-" + "Pending Assignment" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + "-" + "Assigned" });
            if (enableTestingStage)
                mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + "-" + "Testing" });
            if (enablePendingCloseStage)
                mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2, Title = ModuleName + "-" + "Pending Close" });
            mList.Add(new ModuleStatusMapping() { ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4, Title = ModuleName + "-" + "Closed" });
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("TaskEmails");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleTaskEmail()
            {
                Status = "Initiated",
                EmailTitle = "TSR Ticket Returned [$TicketId$]: [$Title$]",
                EmailBody = @"TSR Ticket ID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.",
                ModuleNameLookup = ModuleName, StageStep = stageID,
                EmailUserTypes = "InitiatorUser",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Initiated"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Requestor Notification",
                EmailTitle = "New TSR Ticket Created [$TicketId$]: [$Title$]",
                EmailBody = @"TSR Ticket ID [$TicketIdWithoutLink$] has been created on your behalf.<br><br>" +
                                                        "Please review the details below. You will be notified when the ticket is resolved.",
                ModuleNameLookup = ModuleName, StageStep = (stageID + 1),
                EmailUserTypes = "RequestorUser",
                SendEvenIfStageSkipped = true,
                Title = ModuleName + " - " + "Requestor Notification"
            });
            if (enableManagerApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Manager Approval",
                    EmailTitle = "New TSR Ticket Pending Manager Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"TSR Ticket ID [$TicketId$] is pending Manager Approval.<br><br>" +
                                                        "Please approve or reject the request.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,StageStep = (++stageID),
                    EmailUserTypes = "BusinessManagerUser",
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Manager Approval"
                });
            }
            if (enableSecurityApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Security Approval",
                    EmailTitle = "TSR Ticket Pending Security Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"TSR Ticket ID [$TicketId$] is pending Security Approval.<br><br>" +
                                                        "Please approve or reject the request",
                    ModuleNameLookup = ModuleName, StageStep = (++stageID),
                    EmailUserTypes = "SecurityManagerUser",
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Security Approval"
                });
            }

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Pending Assignment",
                EmailTitle = "TSR Ticket Pending Assignment [$TicketId$]: [$Title$]",
                EmailBody = @"TSR Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields",
                ModuleNameLookup = ModuleName, StageStep = (++stageID),
                EmailUserTypes = "OwnerUser",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Pending Assignment"
            });
            mList.Add(new ModuleTaskEmail()
            {
                Status = "Assigned - Notify PRP/ORP",
                EmailTitle = "TSR Ticket Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"TSR Ticket ID [$TicketId$] has been assigned to you for resolution.<br><br>" +
                                                        "Once resolved, please enter the resolution description and actual hours in the ticket, and assign a tester.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID),
                EmailUserTypes = "PRPUser;#ORPUser",
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
                EmailUserTypes = "InitiatorUser",
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
                EmailUserTypes = "RequestorUser",
                SendEvenIfStageSkipped = false,
                Title = ModuleName + " - " + "Assigned"
            });

            if (enableTestingStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Testing",
                    EmailTitle = "TSR Ticket Awaiting Testing [$TicketId$]: [$Title$]",
                    EmailBody = @"TSR Ticket ID [$TicketId$] has been assigned to you for testing.<br><br>" +
                                                        "Please test the resolution and approve or reject the resolution.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName, StageStep = (++stageID),
                    EmailUserTypes = "TesterUser",
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Testing"
                });
            }
            if (enablePendingCloseStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Pending Close",
                    EmailTitle = "TSR Ticket Pending Close [$TicketId$]: [$Title$]",
                    EmailBody = @"TSR Ticket ID [$TicketId$] has completed testing and is pending close.<br><br>" +
                                                        "Please review the resolution and close the ticket.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName, StageStep = (++stageID),
                    EmailUserTypes = "OwnerUser",
                    SendEvenIfStageSkipped = false,
                    Title = ModuleName + " - " + "Pending Close"
                });
            }
            mList.Add(new ModuleTaskEmail() { Status = "Closed", EmailTitle = "TSR Ticket Closed [$TicketId$]: [$Title$]", EmailBody = "TSR Ticket ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (stageID + 1), EmailUserTypes = "OwnerUser", SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "Closed" });
            mList.Add(new ModuleTaskEmail() { Status = "Closed - Requestor", EmailTitle = "TSR Ticket Closed [$TicketId$]: [$Title$]", EmailBody = "TSR Ticket ID [$TicketIdWithoutLink$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (++stageID), EmailUserTypes = "RequestorUser", SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "Closed" });
            mList.Add(new ModuleTaskEmail() { Status = "On-Hold", EmailTitle = "TSR Ticket On Hold [$TicketId$]: [$Title$]", EmailBody = "TSR Ticket ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null, EmailUserTypes = "TicketOwner", SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "On-Hold" });
            mList.Add(new ModuleTaskEmail() { Status = "On-Hold - Requestor", EmailTitle = "TSR Ticket On Hold [$TicketId$]: [$Title$]", EmailBody = "TSR Ticket ID [$TicketIdWithoutLink$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null, EmailUserTypes = "RequestorUser", SendEvenIfStageSkipped = false, Title = ModuleName + " - " + "On-Hold - Requestor" });
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
            //mList.Add(new ModulePrioirty() { ModuleNameLookup = "TSR", Title = "2-High", uPriority = "2-High", ItemOrder = 4 });
            //mList.Add(new ModulePrioirty() { ModuleNameLookup = "TSR", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 5 });
            //mList.Add(new ModulePrioirty() { ModuleNameLookup = "TSR", Title = "4-Low", uPriority = "4-Low", ItemOrder = 6 });
            //mList.Add(new ModulePrioirty() { ModuleNameLookup = "TSR", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4 });
            return mList;
        }

        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            //// Console.WriteLine("RequestPriority");
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = Convert.ToInt32(Helper.getTableIDByModuleTitle( DatabaseObjects.Tables.TicketImpact,DatabaseObjects.Columns.Title+"='Impacts Individual' and '"+DatabaseObjects.Columns.ModuleNameLookup+"='"+ModuleName+"'")), SeverityLookup = Convert.ToInt32(Helper.getTableIDByModuleTitle(DatabaseObjects.Tables.TicketSeverity, DatabaseObjects.Columns.Title + "=Low' and '" + DatabaseObjects.Columns.ModuleNameLookup + "='" + ModuleName + "'")), PriorityLookup = Convert.ToInt32(Helper.getTableIDByModuleTitle(DatabaseObjects.Tables.TicketPriority, DatabaseObjects.Columns.Title + "='4-Low' and '" + DatabaseObjects.Columns.ModuleNameLookup + "='" + ModuleName + "'")) });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 1 });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 1, SeverityLookup = 3, PriorityLookup = 2 });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 2 });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 2, SeverityLookup = 3, PriorityLookup = 3 });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 3, SeverityLookup = 1, PriorityLookup = 2 });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 3, SeverityLookup = 2, PriorityLookup = 3 });
            //mList.Add(new ModulePriorityMap() { ModuleNameLookup = "TSR", ImpactLookup = 3, SeverityLookup = 3, PriorityLookup = 3 });
            return mList;
        }

        public List<ModuleSeverity> GetModuleSeverity()
        {
            //// Console.WriteLine(" TicketSeverity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            //mList.Add(new ModuleSeverity() { ModuleNameLookup = "TSR", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            //mList.Add(new ModuleSeverity() { ModuleNameLookup = "TSR", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            //mList.Add(new ModuleSeverity() { ModuleNameLookup = "TSR", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });
            return mList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="impacts"></param>
        /// <param name="serverities"></param>
        /// <param name="priorities"></param>
        /// <param name="mapping">User need to pass index of serverity, impact and priority. system will convert index into id before db entry</param>
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            impacts.Add(new ModuleImpact() { ModuleNameLookup = ModuleName,  Title = "Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = ModuleName, Title = "Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = ModuleName, Title = "Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            serverities.Add(new ModuleSeverity() { ModuleNameLookup = ModuleName, Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = ModuleName, Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = ModuleName, Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });

            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "2-High", uPriority = "2-High", ItemOrder = 4 ,Color = "#FF7F1D" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 5,Color = "#FFED14" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "4-Low", uPriority = "4-Low", ItemOrder = 6,Color= "#cccfd2" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4,IsVIP=true,Color= "#FF351F" });


            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 0 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 2, SeverityLookup = 1, PriorityLookup =2});
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup =1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 1, SeverityLookup =0, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 0 });

        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "Requestor", UserTypes = "Requestor", ColumnName = "RequestorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "DisableEmailTicketLink=true" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "Business Manager", UserTypes = "Business Approver", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "ManagerOf=TicketRequestor" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "Security Manager", UserTypes = "Security Approver", ColumnName = "SecurityManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "Infrastructure Manager", UserTypes = "Infrastructure Manager", ColumnName = "InfrastructureManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "PRP", UserTypes = "PRP", ColumnName = "PRPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "ORP", UserTypes = "ORP", ColumnName = "ORPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "Tester", UserTypes = "Tester", ColumnName = "TesterUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSR", Title = "PRP Group", UserTypes = "PRP Group", ColumnName = "PRPGroupUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Unassigned", ViewName = "TSRTickets", TabOrder = 1, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "TSRTickets", TabOrder = 2, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Items", ViewName = "TSRTickets", TabOrder = 3, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Items", ViewName = "TSRTickets", TabOrder = 4, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Items", ViewName = "TSRTickets", TabOrder = 5, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Items", ViewName = "TSRTickets", TabOrder = 6, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Items", ViewName = "TSRTickets", TabOrder = 7, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Items", ViewName = "TSRTickets", TabOrder = 8, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Items", ViewName = "TSRTickets", TabOrder = 9, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "TSRTickets", TabOrder = 10, ModuleNameLookup = "TSR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            return tabs;
        }
    }
}
