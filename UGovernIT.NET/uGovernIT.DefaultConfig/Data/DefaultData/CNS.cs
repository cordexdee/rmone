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
    public class CNS : IModule
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
                    ShortName = "Service Projects",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "CRMServices",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/CRMServices",
                    ModuleHoldMaxStage = 3,
                    Title = "Service Projects (CNS)",
                    ModuleDescription = "This module is used to monitor and manage projects in progress that have either come through an opportunity or directly started.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/CNS",
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = false,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    KeepItemOpen = true,
                    ShowComment = true,
                    AllowEscalationFromList = true,
                    EnableRMMAllocation = true,
                    EnableLayout = true
                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "CNS";
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
            //Completed
            mList.Add(new ModuleFormTab() { TabName = "Summary", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Summary" });
            mList.Add(new ModuleFormTab() { TabName = "General", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General" });
            mList.Add(new ModuleFormTab() { TabName = "Project Team", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Project Team" });
            mList.Add(new ModuleFormTab() { TabName = "External Team", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "External Team" });
            mList.Add(new ModuleFormTab() { TabName = "Schedule", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Schedule" });
            mList.Add(new ModuleFormTab() { TabName = "Tasks", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Tasks" });
            mList.Add(new ModuleFormTab() { TabName = "Metrics", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Metrics" });
            mList.Add(new ModuleFormTab() { TabName = "Project Close", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Project Close" });            
            mList.Add(new ModuleFormTab() { TabName = "Resource Sheet", TabId = 9, TabSequence = 9, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Resource Sheet" });            
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 10, TabSequence = 10, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 11, TabSequence = 11, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 12, TabSequence = 12, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 9;

            string closeStageID = (currStageID + numStages - 1).ToString();
            int StageStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Name = "Project Setup",
                StageTitle = "Project Setup",
                UserWorkflowStatus = "Initiate new CRM service request",
                UserPrompt = "<b>Please fill the form to open a new CNS request.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Initiated",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Scope",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Scope",
                StageTitle = "Scope",
                UserWorkflowStatus = "Review",
                UserPrompt = "Please review the request and click Approach button to move to the next stage",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Review",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Review",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Estimate",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Estimate",
                StageTitle = "Estimate",
                UserWorkflowStatus = "Develop Approach",
                UserPrompt = "Prepare the overall approach and click Manage to move to the next stage",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Owner Closed",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Owner approved fix implementation & closed the ticket",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Proposal",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Prepare Proposal",
                StageTitle = "Prepare Proposal",
                UserWorkflowStatus = "Manage Project using&nbsp; Procore",
                UserPrompt = "Select Response button to move to the next stage",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Owner Closed",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Manage Project using&nbsp; Procore",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Manager Review",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Review Proposal",
                StageTitle = "Review Proposal",
                UserWorkflowStatus = "",
                UserPrompt = "Prepare response and submit to Client, and then click Waiting to move to the next stage",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Prepare Response",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Send Proposal",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Waiting for Client",
                StageTitle = "Waiting for Client",
                UserWorkflowStatus = "",
                UserPrompt = "If contract is awarded, click the Award button, if lost or cancelled, click the Loss/Cancel button",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Contract Award",
                StageTitle = "Contract Award",
                UserWorkflowStatus = "",
                UserPrompt = "Assemble the team, initiate project with Procore and select the Start button to move to the next stage",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Start",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = "awardstage",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Project Start",
                StageTitle = "Project Start",
                UserWorkflowStatus = "",
                UserPrompt = "Manage the project using the Procore tool, and when complete, select the Complete button to move to the next stage",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "",
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Complete",
                StageRejectedButtonName = "Loss/Cancel",
                StageReturnButtonName = "Go Back",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "projectstart",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Closed",
                StageTitle = "Closed",
                UserWorkflowStatus = "",
                UserPrompt = "",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Closed",
                ActionUser = "Admin",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
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
            // completed
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "EstimateNo", FieldDisplayName = "Est #", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ProjectId", FieldDisplayName = "Proj #", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Title", FieldDisplayName = "Project", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMCompanyLookup", FieldDisplayName = "Company", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Address", FieldDisplayName = "Property Address", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Estimator", FieldDisplayName = "Estimator", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ProjectManager", FieldDisplayName = "Project Manager", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Superintendent", FieldDisplayName = "Superintendent", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMProjectStatus", FieldDisplayName = "Status", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "PreconStartDate", FieldDisplayName = "Precon Date", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "EstimatedConstructionStart", FieldDisplayName = "Construction Start", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMDuration", FieldDisplayName = "Project Duration", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "EstimatedConstructionEnd", FieldDisplayName = "Const. End", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "DateTime", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "RevisedBudget", FieldDisplayName = "Budget", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ApproxContractValue", FieldDisplayName = "Contract", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = false, ShowInMobile = true, CustomProperties = "", ColumnType = "Currency", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "RequestTypeLookup", FieldDisplayName = "Project Type", FieldSequence = (++seqNum), IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "OnHold", FieldDisplayName = "OnHold", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "PctComplete", FieldDisplayName = "% Comp", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Status", FieldDisplayName = "Stage", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = "ProgressBar", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "StageStep", FieldDisplayName = "Step", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Initiator", FieldDisplayName = "Initiator", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Owner", FieldDisplayName = "Owner", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = true, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Comment", FieldDisplayName = "Comment", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CloseDate", FieldDisplayName = "Closed On", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = null, TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMBusinessUnit", FieldDisplayName = "Business Unit", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = "Center" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "AwardedorLossDate", FieldDisplayName = "Award/Loss Date", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "DateTime", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "StreetAddress1", FieldDisplayName = "Address", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "City", FieldDisplayName = "City", FieldSequence = (++seqNum), IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "String", TextAlignment = null });
                       
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {

            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            return mList;

        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");
            // Completed
            int seqNum = 0;
            //0
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Estimate", FieldDisplayName = "Estimate", FieldName = "EstimateNo", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Name", FieldDisplayName = "Project Name", FieldName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Client Company", FieldDisplayName = "Client Company", FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Business Unit", FieldDisplayName = "Business Unit", FieldName = "CRMBusinessUnit", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Approximate Contract Value", FieldDisplayName = "Approximate Contract Value", FieldName = "ApproxContractValue", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Client Contact", FieldDisplayName = "Client Contact", FieldName = "ContactLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Architect", FieldDisplayName = "Architect", FieldName = "Architect", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Proposal Type", FieldDisplayName = "Proposal Type", FieldName = "OpportunityType", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Street", FieldDisplayName = "Street", FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", FieldName = "City", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", FieldName = "StateLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", FieldName = "Zip", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Country", FieldDisplayName = "Country", FieldName = "Country", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Gross Sq Ft", FieldDisplayName = "Gross Sq Ft", FieldName = "RetailSqftNum", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Net Rentable Sq Ft", FieldDisplayName = "Net Rentable Sq Ft", FieldName = "UsableSqFtNum", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Status", FieldDisplayName = "Project Status", FieldName = "CRMProjectStatus", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", FieldName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Type", FieldDisplayName = "Project Type", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Studio", FieldDisplayName = "Studio", FieldName = "Studio", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Owner Contract Type", FieldDisplayName = "Owner Contract Type", FieldName = "OwnerContractType", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", FieldName = "Owner1", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Additional Info", FieldDisplayName = "Additional Info", FieldName = "AdditionalInfo", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Precon Start Date", FieldDisplayName = "Precon Start Date", FieldName = "PreconStartDate", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Bid Due Date", FieldDisplayName = "Bid Due Date", FieldName = "BidDueDate", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction Start", FieldDisplayName = "Est Construction Start", FieldName = "EstimatedConstructionStart", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Duration(weeks)", FieldDisplayName = "Duration(weeks)", FieldName = "CRMDuration", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction End", FieldDisplayName = "Est Construction End", FieldName = "EstimatedConstructionEnd", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Private", FieldDisplayName = "Private", FieldName = "IsPrivate", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });

            seqNum = 0;
            //1
            mList.Add(new ModuleFormLayout() { Title = "Project Title", FieldDisplayName = "Project Title", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CPRProjectTitleControl", FieldDisplayName = "CPRProjectTitleControl", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Title", FieldDisplayName = "Project Title", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CPR Dashboard", FieldDisplayName = "CPR Dashboard", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "ProjectTeam", FieldDisplayName = "ProjectTeam", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "TaskGraph", FieldDisplayName = "TaskGraph", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "ProjectSummaryControl", FieldDisplayName = "ProjectSummaryControl", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CPR Dashboard", FieldDisplayName = "CPR Dashboard", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Timeline", FieldDisplayName = "Timeline", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "TimelineControl", FieldDisplayName = "TimelineControl", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Timeline", FieldDisplayName = "Timeline", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //2
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Estimate #", FieldDisplayName = "Estimate #", FieldName = "EstimateNo", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project #", FieldDisplayName = "Project #", FieldName = "ProjectId", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Proposal Type", FieldDisplayName = "Proposal Type", FieldName = "OpportunityType", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Name", FieldDisplayName = "Project Name", FieldName = "Title", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Address", FieldDisplayName = "Project Address", FieldName = "Address", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Street", FieldDisplayName = "Street", FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", FieldName = "City", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", FieldName = "StateLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", FieldName = "Zip", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Country", FieldDisplayName = "Country", FieldName = "Country", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Client Company", FieldDisplayName = "Client Company", FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Business Unit", FieldDisplayName = "Business Unit", FieldName = "CRMBusinessUnit", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Client Contact", FieldDisplayName = "Client Contact", FieldName = "ContactLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Status", FieldDisplayName = "Project Status", FieldName = "CRMProjectStatus", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Approximate Contract Value", FieldDisplayName = "Approximate Contract Value", FieldName = "ApproxContractValue", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Awarded/Loss Date", FieldDisplayName = "Awarded/Loss Date", FieldName = "AwardedorLossDate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Leed Level", FieldDisplayName = "Lead Level", FieldName = "LeadLevel", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Gross Sq Ft", FieldDisplayName = "Gross Sq Ft", FieldName = "RetailSqftNum", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Net Rentable Sq Ft", FieldDisplayName = "Net Rentable Sq Ft", FieldName = "UsableSqFtNum", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", FieldName = "Description", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Type", FieldDisplayName = "Project Type", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Owner Contract Type", FieldDisplayName = "Owner Contract Type", FieldName = "OwnerContractType", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Studio", FieldDisplayName = "Studio", FieldName = "Studio", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Additional Info", FieldDisplayName = "Additional Info", FieldName = "AdditionalInfo", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", FieldName = "Owner1", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Precon Start Date", FieldDisplayName = "Precon Start Date", FieldName = "PreconStartDate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Bid Due Date", FieldDisplayName = "Bid Due Date", FieldName = "BidDueDate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction Start", FieldDisplayName = "Est Construction Start", FieldName = "EstimatedConstructionStart", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Duration(Weeks)", FieldDisplayName = "Duration(Weeks)", FieldName = "CRMDuration", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction End", FieldDisplayName = "Est Construction End", FieldName = "EstimatedConstructionEnd", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Private", FieldDisplayName = "Private", FieldName = "IsPrivate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //3
            mList.Add(new ModuleFormLayout() { Title = "Project Internal Team", FieldDisplayName = "Project Internal Team", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CRMProjectAllocation", FieldDisplayName = "CRMProjectAllocation", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Internal Team", FieldDisplayName = "Project Internal Team", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //4
            mList.Add(new ModuleFormLayout() { Title = "Project External Team", FieldDisplayName = "Project External Team", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "RelatedCompanies", FieldDisplayName = "RelatedCompanies", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Project External Team", FieldDisplayName = "Project External Team", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //5
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "TasksList", FieldDisplayName = "TasksList", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //6
            mList.Add(new ModuleFormLayout() { Title = "Activities", FieldDisplayName = "Activities", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "preconditionlist", FieldDisplayName = "preconditionlist", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Activities", FieldDisplayName = "Activities", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //7
            mList.Add(new ModuleFormLayout() { Title = "Managing Changes", FieldDisplayName = "Managing Changes", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "# of RFIs", FieldDisplayName = "# of RFIs", FieldName = "RFIs", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "AVG Days To Respond To RFIs", FieldDisplayName = "AVG Days To Respond To RFIs", FieldName = "DaysRespondToRFIs", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PCCOs", FieldDisplayName = "PCCOs", FieldName = "PCOs", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "AVG Days to Close a PCCO", FieldDisplayName = "AVG Days to Close a PCCO", FieldName = "DaysToCloseCCO", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "CCOs", FieldDisplayName = "CCOs", FieldName = "CCOs", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Managing Changes", FieldDisplayName = "Managing Changes", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Budget", FieldDisplayName = "Budget", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Original Budget", FieldDisplayName = "Original Budget", FieldName = "BudgetAmount", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Approved PCCOs", FieldDisplayName = "Approved PCCOs", FieldName = "ApprovedChangeOrders", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Final Budget", FieldDisplayName = "Final Budget", FieldName = "CurrentBudget", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Planned Labor Hours", FieldDisplayName = "Planned Labor Hours", FieldName = "PlannedLabourHours", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Actual Labor Hours", FieldDisplayName = "Actual Labor Hours", FieldName = "ActualLabourHours", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Variance", FieldDisplayName = "Variance", FieldName = "LabourHourVariance", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Billings", FieldDisplayName = "Project Billings", FieldName = "ProjectBillings", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Collections", FieldDisplayName = "Project Collections", FieldName = "ProjectCollections", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Budget", FieldDisplayName = "Budget", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Commitments", FieldDisplayName = "Commitments", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Original Commitments", FieldDisplayName = "Original Commitments", FieldName = "OriginalCommitments", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "All CCOs", FieldDisplayName = "All CCOs", FieldName = "AllCCOs", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Final Commitment", FieldDisplayName = "Final Commitment", FieldName = "FinalCommitment", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Internal CCOs", FieldDisplayName = "Internal CCOs", FieldName = "InternalCCOs", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Client Paid", FieldDisplayName = "Client Paid", FieldName = "IntClientPaid", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Unpaid Costs", FieldDisplayName = "Unpaid Costs", FieldName = "IntUnpaidCosts", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "External CCOs", FieldDisplayName = "External CCOs", FieldName = "ExternalCCOs", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Client Paid", FieldDisplayName = "Client Paid", FieldName = "ExtClientPaid", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Unpaid Costs", FieldDisplayName = "Unpaid Costs", FieldName = "ExtUnpaidCosts", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Commitments", FieldDisplayName = "Commitments", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Financials", FieldDisplayName = "Project Financials", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Actual Cost", FieldDisplayName = "Actual Cost", FieldName = "ProjectCost", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Planned Profit(%)", FieldDisplayName = "Planned Profit(%)", FieldName = "PctPlannedProfit", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Gap(%)", FieldDisplayName = "Gap(%)", FieldName = "PctGap", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Financials", FieldDisplayName = "Project Financials", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Quality", FieldDisplayName = "Quality", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "# Of Punch List Items", FieldDisplayName = "# Of Punch List Items", FieldName = "PunchListItems", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Days To Resolve Punch List", FieldDisplayName = "Days To Resolve Punch List", FieldName = "DaysToResolvePunchList", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Quality", FieldDisplayName = "Quality", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Orignal Const Start", FieldDisplayName = "Orignal Const Start", FieldName = "OrignalStartDate", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Actual Const Start", FieldDisplayName = "Actual Const Start", FieldName = "ActualStartDate", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Variance", FieldDisplayName = "Variance", FieldName = "StartDateVariance", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Orignal Const End", FieldDisplayName = "Orignal Const End", FieldName = "OrignalEndDate", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Actual Const End", FieldDisplayName = "Actual Const End", FieldName = "RevisedEndDate", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Variance", FieldDisplayName = "Variance", FieldName = "EndDateVariance", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Schedule", FieldDisplayName = "Schedule", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });

            seqNum = 0;
            //8
            mList.Add(new ModuleFormLayout() { Title = "CheckList", FieldDisplayName = "CheckList", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CheckListProjectView", FieldDisplayName = "CheckListProjectView", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CheckList", FieldDisplayName = "CheckList", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //9
            mList.Add(new ModuleFormLayout() { Title = "Resource Availability", FieldDisplayName = "Resource Availability", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "ResourceAvailability", FieldDisplayName = "ResourceAvailability", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Resource Availability", FieldDisplayName = "Resource Availability", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //10
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });

            seqNum = 0;
            //11
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", FieldName = "Comment", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });

            seqNum = 0;
            //12
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "IsReadOnly=True;#IsInFrame=False", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum), HideInTemplate = true });
                       

            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("RequestType");


            mList.Add(new ModuleRequestType() { Title = "Cafeteria", ModuleNameLookup = ModuleName, RequestType = "Cafeteria", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Elevator/Escalator", ModuleNameLookup = ModuleName, RequestType = "Elevator/Escalator", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Lobby", ModuleNameLookup = ModuleName, RequestType = "Lobby", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Restroom", ModuleNameLookup = ModuleName, RequestType = "Restroom", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Storefront/Entry", ModuleNameLookup = ModuleName, RequestType = "Storefront/Entry", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Industrial Building", ModuleNameLookup = ModuleName, RequestType = "Industrial Building", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Mixed Use", ModuleNameLookup = ModuleName, RequestType = "Mixed Use", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Office Building", ModuleNameLookup = ModuleName, RequestType = "Office Building", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Office Park/Campus Development", ModuleNameLookup = ModuleName, RequestType = "Office Park/Campus Development", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Warehouse", ModuleNameLookup = ModuleName, RequestType = "Warehouse", Category = "Construction", SubCategory = "Ground Up - 09", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Auditorium", ModuleNameLookup = ModuleName, RequestType = "Auditorium", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Broadcast Studio", ModuleNameLookup = ModuleName, RequestType = "Broadcast Studio", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Classroom", ModuleNameLookup = ModuleName, RequestType = "Classroom", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Data Center", ModuleNameLookup = ModuleName, RequestType = "Data Center", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Fitness Center/Gym", ModuleNameLookup = ModuleName, RequestType = "Fitness Center/Gym", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Interconnecting Stair", ModuleNameLookup = ModuleName, RequestType = "Interconnecting Stair", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Laboratory", ModuleNameLookup = ModuleName, RequestType = "Laboratory", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Market Ready/Spec Suite", ModuleNameLookup = ModuleName, RequestType = "Market Ready/Spec Suite", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Office Space", ModuleNameLookup = ModuleName, RequestType = "Office Space", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Mixed Use > Mixed Use Office", ModuleNameLookup = ModuleName, RequestType = "Mixed Use Office", Category = "Construction", SubCategory = "Mixed Use - 16", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Mixed Use > Mixed Use Residential", ModuleNameLookup = ModuleName, RequestType = "Mixed Use Residential", Category = "Construction", SubCategory = "Mixed Use - 16", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Apartments", ModuleNameLookup = ModuleName, RequestType = "Apartments", Category = "Construction", SubCategory = "Residential - 17", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Condominiums", ModuleNameLookup = ModuleName, RequestType = "Condominiums", Category = "Construction", SubCategory = "Residential - 17", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Student Housing", ModuleNameLookup = ModuleName, RequestType = "Student Housing", Category = "Construction", SubCategory = "Residential - 17", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > ADA Access", ModuleNameLookup = ModuleName, RequestType = "ADA Access", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Hardscape", ModuleNameLookup = ModuleName, RequestType = "Hardscape", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Landscaping", ModuleNameLookup = ModuleName, RequestType = "Landscaping", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Playground", ModuleNameLookup = ModuleName, RequestType = "Playground", Category = "Construction", SubCategory = "Site Work - 14", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Core And Shell", ModuleNameLookup = ModuleName, RequestType = "Core And Shell", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Corridors", ModuleNameLookup = ModuleName, RequestType = "Corridors", Category = "Construction", SubCategory = "Seismic Structural - 07", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Day Two", ModuleNameLookup = ModuleName, RequestType = "Day Two", Category = "Construction", SubCategory = "Day Two - 11", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Demolition", ModuleNameLookup = ModuleName, RequestType = "Demolition", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Distribution Center/Warehouse", ModuleNameLookup = ModuleName, RequestType = "Distribution Center/Warehouse", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Elevator/Escalator", ModuleNameLookup = ModuleName, RequestType = "Elevator/Escalator", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Historic", ModuleNameLookup = ModuleName, RequestType = "Historic", Category = "Construction", SubCategory = "Historic - 08", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Hospitality", ModuleNameLookup = ModuleName, RequestType = "Hospitality", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Lobby", ModuleNameLookup = ModuleName, RequestType = "Lobby", Category = "Construction", SubCategory = "Interiors - 01", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Service", ModuleNameLookup = ModuleName, RequestType = "Service", Category = "Construction", SubCategory = "Service - 12", FunctionalAreaLookup = 16, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Warranty", ModuleNameLookup = ModuleName, RequestType = "Warranty", Category = "Construction", SubCategory = "Warranty - 15", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Architecture", ModuleNameLookup = ModuleName, RequestType = "Architecture", Category = "Professional Services", SubCategory = "Architecture - 02", FunctionalAreaLookup = 9, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction Management", ModuleNameLookup = ModuleName, RequestType = "Construction Management", Category = "Professional Services", SubCategory = "Architecture - 02", FunctionalAreaLookup = 9, WorkflowType = "Full", Owner = null, Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Construction Management", ModuleNameLookup = ModuleName, RequestType = "Construction Management", Category = "Professional Services", SubCategory = "Construction Management - 02", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Engineering", ModuleNameLookup = ModuleName, RequestType = "Engineering", Category = "Professional Services", SubCategory = "Architecture - 02", FunctionalAreaLookup = 9, WorkflowType = "Full", Owner = null, Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Permit Services", ModuleNameLookup = ModuleName, RequestType = "Permit Services", Category = "Professional Services", SubCategory = "Permit Services - 02", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > LEED", ModuleNameLookup = ModuleName, RequestType = "LEED", Category = "Professional Services", SubCategory = "Sustainability - 02", FunctionalAreaLookup = 9, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > Living Building Challenge", ModuleNameLookup = ModuleName, RequestType = "Living Building Challenge", Category = "Professional Services", SubCategory = "Sustainability - 02", FunctionalAreaLookup = 9, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > WELL", ModuleNameLookup = ModuleName, RequestType = "WELL", Category = "Professional Services", SubCategory = "Sustainability - 02", FunctionalAreaLookup = 9, WorkflowType = "Full", Owner = null, Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "PSG-Design Build", ModuleNameLookup = ModuleName, RequestType = "PSG-Design Build", Category = "Professional Services", SubCategory = "PSG-Design Build - 02", FunctionalAreaLookup = null, WorkflowType = "Full", Owner = null, Deleted = false });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {

            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");

            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualLabourHours", FieldName = "ActualLabourHours", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "AdditionalInfo", FieldName = "AdditionalInfo", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "AllCCOs", FieldName = "AllCCOs", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ApproxContractValue", FieldName = "ApproxContractValue", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Architect", FieldName = "Architect", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Attachments", FieldName = "Attachments", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = "AwardedorLossDate", FieldName = "AwardedorLossDate", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BidDueDate", FieldName = "BidDueDate", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BrokerContact", FieldName = "BrokerContact", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BuildingOwner", FieldName = "BuildingOwner", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "City", FieldName = "City", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CompanyArchitect", FieldName = "CompanyArchitect", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CompanyEngineer", FieldName = "CompanyEngineer", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ConstructionManagementCompany", FieldName = "ConstructionManagementCompany", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ConstructionManagerContact", FieldName = "ConstructionManagerContact", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CRMBusinessUnit", FieldName = "CRMBusinessUnit", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CRMCompanyLookup", FieldName = "CRMCompanyLookup", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CRMDuration", FieldName = "CRMDuration", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CRMProjectStatus", FieldName = "CRMProjectStatus", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "DaysToResolvePunchList", FieldName = "DaysToResolvePunchList", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EndDateVariance", FieldName = "EndDateVariance", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EngineerContact", FieldName = "EngineerContact", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatedConstructionEnd", FieldName = "EstimatedConstructionEnd", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatedConstructionStart", FieldName = "EstimatedConstructionStart", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimateNo", FieldName = "EstimateNo", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ExtClientPaid", FieldName = "ExtClientPaid", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ExternalCCOs", FieldName = "ExternalCCOs", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ExtUnpaidCosts", FieldName = "ExtUnpaidCosts", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "FinalCommitment", FieldName = "FinalCommitment", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "IntClientPaid", FieldName = "IntClientPaid", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "InternalCCOs", FieldName = "InternalCCOs", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "IntUnpaidCosts", FieldName = "IntUnpaidCosts", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "IsPrivate", FieldName = "IsPrivate", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "LabourHourVariance", FieldName = "LabourHourVariance", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "LeadLevel", FieldName = "LeadLevel", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OpportunityType", FieldName = "OpportunityType", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OriginalCommitments", FieldName = "OriginalCommitments", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PctGap", FieldName = "PctGap", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PctPlannedProfit", FieldName = "PctPlannedProfit", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PlannedLabourHours", FieldName = "PlannedLabourHours", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PreconStartDate", FieldName = "PreconStartDate", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectId", FieldName = "ProjectId", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectNote", FieldName = "ProjectNote", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectTeamLookup", FieldName = "ProjectTeamLookup", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PunchListItems", FieldName = "PunchListItems", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RetailSqft", FieldName = "RetailSqft", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RetailSqftNum", FieldName = "RetailSqftNum", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "StartDateVariance", FieldName = "StartDateVariance", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "StreetAddress1", FieldName = "StreetAddress1", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Duration", FieldName = "Duration", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RequestTypeLookup", FieldName = "RequestTypeLookup", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Title", FieldName = "Title", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Country", FieldName = "Country", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "StateLookup", FieldName = "StateLookup", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "UsableSqFt", FieldName = "UsableSqFt", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "UsableSqFtNum", FieldName = "UsableSqFtNum", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Zip", FieldName = "Zip", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OwnerContractType", FieldName = "OwnerContractType", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Studio", FieldName = "Studio", StageStep = 0, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = "ContactLookup", FieldName = "ContactLookup", StageStep = 1, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 1, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 1, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Description", FieldName = "Description", StageStep = 1, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Owner", FieldName = "Owner", StageStep = 1, ModuleNameLookup = ModuleName, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });


            mList.Add(new ModuleRoleWriteAccess() { Title = "APMPctAlloc", FieldName = "APMPctAlloc", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "AssistantProjectManager", FieldName = "AssistantProjectManager", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Broker", FieldName = "Broker", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ContactLookup", FieldName = "ContactLookup", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Estimator", FieldName = "Estimator", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatorPctAlloc", FieldName = "EstimatorPctAlloc", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectExecutive", FieldName = "ProjectExecutive", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectManagerPctAlloc", FieldName = "ProjectManagerPctAlloc", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Superintendent", FieldName = "Superintendent", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "SuperintendentPctAlloc", FieldName = "SuperintendentPctAlloc", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Description", FieldName = "Description", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Owner", FieldName = "Owner", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectManager", FieldName = "ProjectManager", StageStep = 2, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });


            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "APMPctAlloc", FieldName = "APMPctAlloc", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ApprovedChangeOrders", FieldName = "ApprovedChangeOrders", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "AssistantProjectManager", FieldName = "AssistantProjectManager", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Broker", FieldName = "Broker", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetAmount", FieldName = "BudgetAmount", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetContingency", FieldName = "BudgetContingency", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetEndDate", FieldName = "BudgetEndDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetStartDate", FieldName = "BudgetStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CCOs", FieldName = "CCOs", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ContactLookup", FieldName = "ContactLookup", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ContactLookup", FieldName = "ContactLookup", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ContractAmount", FieldName = "ContractAmount", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CostPerformance", FieldName = "CostPerformance", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CurrentBudget", FieldName = "CurrentBudget", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CurrentCommitments", FieldName = "CurrentCommitments", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "DaysRespondToRFIs", FieldName = "DaysRespondToRFIs", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "DaysToCloseCCO", FieldName = "DaysToCloseCCO", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatedConstructionEnd", FieldName = "EstimatedConstructionEnd", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatedConstructionStart", FieldName = "EstimatedConstructionStart", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatedCost", FieldName = "EstimatedCost", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Estimator", FieldName = "Estimator", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatorPctAlloc", FieldName = "EstimatorPctAlloc", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "NonCommittedCosts", FieldName = "NonCommittedCosts", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OrignalEndDate", FieldName = "OrignalEndDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OrignalStartDate", FieldName = "OrignalStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OverUnders", FieldName = "OverUnders", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PCOs", FieldName = "PCOs", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PendingOversUnder", FieldName = "PendingOversUnder", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PendingRevisions", FieldName = "PendingRevisions", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectBillings", FieldName = "ProjectBillings", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectCollections", FieldName = "ProjectCollections", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectCost", FieldName = "ProjectCost", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectedBudget", FieldName = "ProjectedBudget", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectedCommitments", FieldName = "ProjectedCommitments", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectExecutive", FieldName = "ProjectExecutive", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectManagerPctAlloc", FieldName = "ProjectManagerPctAlloc", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RevisedBudget", FieldName = "RevisedBudget", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RevisedEndDate", FieldName = "RevisedEndDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RevisedStartDate", FieldName = "RevisedStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RFIs", FieldName = "RFIs", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ScheduleVariance", FieldName = "ScheduleVariance", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "SubcontractChangeOrders", FieldName = "SubcontractChangeOrders", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "SubcontractPendingRevisions", FieldName = "SubcontractPendingRevisions", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Superintendent", FieldName = "Superintendent", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "SuperintendentPctAlloc", FieldName = "SuperintendentPctAlloc", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Description", FieldName = "Description", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Description", FieldName = "Description", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Owner", FieldName = "Owner", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "Owner", FieldName = "Owner", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectManager", FieldName = "ProjectManager", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "TargetStartDate", FieldName = "TargetStartDate", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "UsableSqFt", FieldName = "UsableSqFt", StageStep = 4, ModuleNameLookup = ModuleName, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });


            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ApprovedChangeOrders", FieldName = "ApprovedChangeOrders", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetAmount", FieldName = "BudgetAmount", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetContingency", FieldName = "BudgetContingency", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetEndDate", FieldName = "BudgetEndDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "BudgetStartDate", FieldName = "BudgetStartDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CCOs", FieldName = "CCOs", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ContractAmount", FieldName = "ContractAmount", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CostPerformance", FieldName = "CostPerformance", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CurrentBudget", FieldName = "CurrentBudget", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "CurrentCommitments", FieldName = "CurrentCommitments", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "DaysRespondToRFIs", FieldName = "DaysRespondToRFIs", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "DaysToCloseCCO", FieldName = "DaysToCloseCCO", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "EstimatedCost", FieldName = "EstimatedCost", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "NonCommittedCosts", FieldName = "NonCommittedCosts", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OrignalEndDate", FieldName = "OrignalEndDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OrignalStartDate", FieldName = "OrignalStartDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "OverUnders", FieldName = "OverUnders", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PCOs", FieldName = "PCOs", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PendingOversUnder", FieldName = "PendingOversUnder", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "PendingRevisions", FieldName = "PendingRevisions", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectBillings", FieldName = "ProjectBillings", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectCollections", FieldName = "ProjectCollections", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectCost", FieldName = "ProjectCost", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectedBudget", FieldName = "ProjectedBudget", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ProjectedCommitments", FieldName = "ProjectedCommitments", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RevisedBudget", FieldName = "RevisedBudget", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RevisedEndDate", FieldName = "RevisedEndDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RevisedStartDate", FieldName = "RevisedStartDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "RFIs", FieldName = "RFIs", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ScheduleVariance", FieldName = "ScheduleVariance", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "SubcontractChangeOrders", FieldName = "SubcontractChangeOrders", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "SubcontractPendingRevisions", FieldName = "SubcontractPendingRevisions", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualCompletionDate", FieldName = "ActualCompletionDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "ActualStartDate", FieldName = "ActualStartDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = "TargetStartDate", FieldName = "TargetStartDate", StageStep = 7, ModuleNameLookup = ModuleName, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                       
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");
            //int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName+"-"+"Identify Opportunity", ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Assign", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Go/No Go", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Develop Approach", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Manage Bids", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Prepare Estimates", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Manage Bids", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Manage Bids", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            //mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Closed", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4 });
            return mList;

        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {

            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            //int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Identify Opportunity",
            //    Status = "Identify Opportunity",
            //    EmailTitle = "New Opportunity Ticket Created [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] has been initiated and needs clarification.<br><br>" +
            //                                            "Please enter the required information and re-submit the ticket.",
            //    ModuleNameLookup = ModuleName,
            //    StageStep = stageID
            //});

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Assign",
            //    Status ="Assign",
            //    EmailTitle = "OPM Ticket Pending Assignment [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
            //                                            "Please assign a PRP and enter any other required fields",
            //    ModuleNameLookup  = ModuleName,
            //    StageStep = (++stageID)});

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Go/No Go",
            //    Status = "Go/No Go",
            //    EmailTitle = "OPM Ticket Assigned [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] has been assigned to you for Go/No Go.<br><br>" +
            //                                            "Once complete, please enter the analysis in the ticket.",
            //    ModuleNameLookup = ModuleName,
            //    StageStep = (++stageID) });

            //mList.Add(new ModuleTaskEmail()
            //{
            //    Title = ModuleName + " - " + "Develop Approach",
            //    Status = "Develop Approach",
            //    EmailTitle = "OPM Ticket Pending Implementation [$TicketId$]: [$Title$]",
            //    EmailBody = @"OPM Ticket ID [$TicketId$] has completed analysis and is pending fix implementation.<br><br>" +
            //                                        "Please review the analysis, implement the recommendations and close the ticket.<br><br>[$IncludeActionButtons$]",
            //    ModuleNameLookup = ModuleName,
            //    StageStep = (++stageID)});

            //mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Manage Bids", Status = "Manage Bids", EmailTitle = "OPM Ticket Closed [$TicketId$]: [$Title$]", EmailBody = "OPM Ticket ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (++stageID)});

            //mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Prepare Estimates", Status = "Prepare Estimates", EmailTitle = "OPM Ticket On Hold [$TicketId$]: [$Title$]", EmailBody = "OPM Ticket ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null});

            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Initiator", UserTypes = "Initiator", ColumnName = "Initiator", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Project Manager", UserTypes = "Project Manager", ColumnName = "Project Manager", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();

            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "CRMServices", TabOrder = 1, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "CRMServices", TabOrder = 2, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "CRMServices", TabOrder = 3, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "CRMServices", TabOrder = 4, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "CRMServices", TabOrder = 5, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Pipeline", ViewName = "CRMServices", TabOrder = 6, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "Under Construction", ViewName = "CRMServices", TabOrder = 7, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "CRMServices", TabOrder = 8, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "CRMServices", TabOrder = 9, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Closed", ViewName = "CRMServices", TabOrder = 10, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
