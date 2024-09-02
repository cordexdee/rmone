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
    public class DRQ : IModule
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
        protected static bool enableSecurityApprovalStage = true;
        protected static bool enableManagerApprovalStage = true;
        protected static bool enablePendingCloseStage = true;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Change Management",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "DRQ",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/drqtickets",
                    ModuleHoldMaxStage = 5,
                    // Title = "Change Management (DmRequestType)",
                    Title = "Change Management (DRQ)",
                    //ModuleDescription = "This module is used to create a deployment request to implement changes approved as part of an ACR, PRS, or TSR request.",
                    ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details. Or, select a ticket by clicking checkbox and clicking Actions button.",
                    ThemeColor = "Accent6",
                    StaticModulePagePath = "/Pages/drq",
                    ModuleType = ModuleType.SMS,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    AllowDelete = true,
                    EnableNewsOnHomePage = true,
                    EnableLayout=true,
                    UseInGlobalSearch=true,
                    KeepItemOpen = true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "DRQ";
            }
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "RequestorUser", FieldDisplayName = "Requested By", IsDisplay = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum, ColumnType = "ProgressBar" });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "RapidRequest", FieldDisplayName = "Urgent", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
          
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "Age", FieldDisplayName = "Age", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, SortOrder = 1, IsAscending = false, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "CreationDate", FieldDisplayName = "Created On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "TargetStartDate", FieldDisplayName = "Start Date", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "TargetCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "CloseDate", FieldDisplayName = "Closed On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "StageActionUsersUser", FieldDisplayName = "Waiting On", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum, CustomProperties = "fieldtype=multiuser", ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "PRPUser", FieldDisplayName = "PRP", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "ORPUser", FieldDisplayName = "ORP", IsUseInWildCard = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "DRQ", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, FieldSequence = ++seqNum });
            return mList;
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");

            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Work Activity", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Work Activity" });
            //mList.Add(new ModuleFormTab() { TabName = "Summmary", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Summmary" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 6, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            mList.Add(new ModuleImpact() { ModuleNameLookup = "DRQ", Title = "DRQ-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "DRQ", Title = "DRQ-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "DRQ", Title = "DRQ-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            return mList;
        }


        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4 });

            return mList;
        }
        public List<ModuleSeverity> GetModuleSeverity()
        {
            // Console.WriteLine(" TicketSeverity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "DRQ", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "DRQ", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "DRQ", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });
            return mList;
        }

        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            // Console.WriteLine("RequestPriority");
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 12, SeverityLookup = 10, PriorityLookup = 10 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 12, SeverityLookup = 11, PriorityLookup = 10 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 12, SeverityLookup = 12, PriorityLookup = 11 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 13, SeverityLookup = 10, PriorityLookup = 10 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 13, SeverityLookup = 11, PriorityLookup = 11 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 13, SeverityLookup = 12, PriorityLookup = 12 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 14, SeverityLookup = 10, PriorityLookup = 11 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 14, SeverityLookup = 11, PriorityLookup = 12 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 14, SeverityLookup = 12, PriorityLookup = 12 });
            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Requestor", UserTypes = "Requestor", ColumnName = "RequestorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Application Manager", UserTypes = "Application Manager", ColumnName = "ApplicationManagerUser", ManagerOnly = true, ITOnly = true, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-DR/BR Manager", UserTypes = "DR Manager", ColumnName = "DRBRManagerUser", ManagerOnly = true, ITOnly = true, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Business Manager", UserTypes = "Business Approver", ColumnName = "BusinessManagerUser", ManagerOnly = true, ITOnly = true, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Security Manager", UserTypes = "Security Approver", ColumnName = "SecurityManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Infrastructure Manager", UserTypes = "Infrastructure Manager", ColumnName = "InfrastructureManagerUser", ManagerOnly = true, ITOnly = true, CustomProperties = "" });

            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-PRP", UserTypes = "PRP", ColumnName = "PRPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-ORP", UserTypes = "ORP", ColumnName = "ORPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "DRQ", Title = "DRQ-Tester", UserTypes = "Tester", ColumnName = "TesterUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }


        public List<UGITModule> GetUGITModule()
        {
            throw new NotImplementedException();
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");

            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Initiated", ModuleNameLookup = ModuleName, StageTitleLookup = stageID });
            if (enableManagerApprovalStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Manager Approval", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            if (enableSecurityApprovalStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Security/DR Approval", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Application/Infrastructure Approval", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Review & Assign Resource", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Implementation", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "DR Closeout", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            if (enablePendingCloseStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Pending Close", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Closed", ModuleNameLookup = ModuleName, StageTitleLookup = ++stageID });

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
            //Tab 0  (New Ticket Form)
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IT Approver", FieldDisplayName = "IT Approver", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Related Request Type", FieldDisplayName = "Related Request Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Request ID", FieldDisplayName = "Related Request ID", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RelatedRequestID", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Urgent Request", FieldDisplayName = "Urgent Request", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DRQRapidTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description of Change", FieldDisplayName = "Description of Change", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "Purpose of Change", FieldDisplayName = "Purpose of Change", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "ChangePurpose", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Requested Implementation Date & Time", FieldDisplayName = "Requested Implementation Date & Time", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Start", FieldDisplayName = "Start", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "TargetStartDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "End", FieldDisplayName = "End", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TargetCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Requested Implementation Date & Time", FieldDisplayName = "Requested Implementation Date & Time", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Implementation Planning", FieldDisplayName = "Implementation Planning", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Test Plan", FieldDisplayName = "Test Plan", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "TestPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Test Plan Responsible", FieldDisplayName = "Test Plan Responsible", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TesterUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Deployment Plan", FieldDisplayName = "Deployment Plan", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DeploymentPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Deployment Responsible", FieldDisplayName = "Deployment Responsible", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DeploymentResponsibleUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Production Verification Plan", FieldDisplayName = "Production Verification Plan", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "ProductionVerificationPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Production Verify Responsible", FieldDisplayName = "Production Verify Responsible", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ProductionVerifyResponsibleUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Recovery Plan", FieldDisplayName = "Recovery Plan", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RecoveryPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Rollback Responsible", FieldDisplayName = "Rollback Responsible", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RollbackResponsibleUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Implementation Planning", FieldDisplayName = "Implementation Planning", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Disaster Recovery Preparedness", FieldDisplayName = "Disaster Recovery Preparedness", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "DR/BR Impact?", FieldDisplayName = "DR/BR Impact?", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DRBRImpactChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Risk", FieldDisplayName = "Risk", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RiskChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Change to Existing Application/Server?", FieldDisplayName = "Change to Existing Application/Server?", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DRQChangeTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Change to DR Replication Process", FieldDisplayName = "Change to DR Replication Process", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DRReplicationChangeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Explain DR/BR Impact", FieldDisplayName = "Explain DR/BR Impact", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DRBRDescription", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Has testing been done?", FieldDisplayName = "Has testing been done?", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TestingDoneChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Disaster Recovery Preparedness", FieldDisplayName = "Disaster Recovery Preparedness", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact Details", FieldDisplayName = "Impact Details", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impacts Organization", FieldDisplayName = "Impacts Organization", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImpactsOrganization", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Impacts User(s)", FieldDisplayName = "Impacts User(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "UsersAffectedUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impacts Department(s)", FieldDisplayName = "Impacts Department(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Describe Impact", FieldDisplayName = "Describe Impact", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "UserImpactDetails", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Outage Description", FieldDisplayName = "Outage Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Outage", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Outage Duration (hrs)", FieldDisplayName = "Outage Duration (hrs)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "Duration", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Locations Affected", FieldDisplayName = "Locations Affected", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "LocationMultLookup", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact Area(s)", FieldDisplayName = "Impact Area(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DRQSystemsLookup", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Application(s)", FieldDisplayName = "Application(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "APPTitleLookup", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact Details", FieldDisplayName = "Impact Details", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "User Notification", FieldDisplayName = "User Notification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "User Notification Required?", FieldDisplayName = "User Notification Required?", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "IsUserNotificationRequired", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "To Be Sent By", FieldDisplayName = "To Be Sent By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Send By Date", FieldDisplayName = "Send By Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ToBeSentByDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Auto Send", FieldDisplayName = "Auto Send", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AutoSend", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Notification Text", FieldDisplayName = "Notification Text", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "NotificationText", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "User Notification", FieldDisplayName = "User Notification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 1
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Status", FieldDisplayName = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Initiator", FieldDisplayName = "Initiator", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InitiatorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IT Approver", FieldDisplayName = "IT Approver", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Initiated Date", FieldDisplayName = "Initiated Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CreationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "DR/BR Manager", FieldDisplayName = "DR/BR Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DRBRManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Infrastructure Manager", FieldDisplayName = "Infrastructure Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InfrastructureManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Application Manager", FieldDisplayName = "Application Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ApplicationManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Related Request Type", FieldDisplayName = "Related Request Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Request ID", FieldDisplayName = "Related Request ID", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RelatedRequestID", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Urgent Request", FieldDisplayName = "Urgent Request", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DRQRapidTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description of Change", FieldDisplayName = "Description of Change", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "Purpose of Change", FieldDisplayName = "Purpose of Change", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ChangePurpose", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Requested Implementation Date & Time", FieldDisplayName = "Requested Implementation Date & Time", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Requested Implementation Start Date", FieldDisplayName = "Requested Implementation Start Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "TargetStartDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Requested Implementation End Date", FieldDisplayName = "Requested Implementation End Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TargetCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Requested Implementation Date & Time", FieldDisplayName = "Requested Implementation Date & Time", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Implementation Planning", FieldDisplayName = "Implementation Planning", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Test Plan", FieldDisplayName = "Test Plan", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "TestPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Test Plan Responsible", FieldDisplayName = "Test Plan Responsible", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TesterUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Deployment Plan", FieldDisplayName = "Deployment Plan", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DeploymentPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Deployment Responsible", FieldDisplayName = "Deployment Responsible", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DeploymentResponsibleUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Production Verification Plan", FieldDisplayName = "Production Verification Plan", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "ProductionVerificationPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Production Verify Responsible", FieldDisplayName = "Production Verify Responsible", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProductionVerifyResponsibleUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Recovery Plan", FieldDisplayName = "Recovery Plan", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "RecoveryPlan", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Rollback Responsible", FieldDisplayName = "Rollback Responsible", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RollbackResponsibleUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Implementation Planning", FieldDisplayName = "Implementation Planning", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Disaster Recovery Preparedness", FieldDisplayName = "Disaster Recovery Preparedness", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "DR/BR Impact?", FieldDisplayName = "DR/BR Impact?", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DRBRImpactChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Risk", FieldDisplayName = "Risk", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RiskChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Change to Existing Application/Server?", FieldDisplayName = "Change to Existing Application/Server?", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DRQChangeTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Change to DR Replication Process", FieldDisplayName = "Change to DR Replication Process", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DRReplicationChangeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Explain DR/BR Impact", FieldDisplayName = "Explain DR/BR Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DRBRDescription", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Has testing been done?", FieldDisplayName = "Has testing been done?", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "TestingDoneChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Disaster Recovery Preparedness", FieldDisplayName = "Disaster Recovery Preparedness", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact Details", FieldDisplayName = "Impact Details", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impacts Organization", FieldDisplayName = "Impacts Organization", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImpactsOrganization", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Department(s) Affected", FieldDisplayName = "Department(s) Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Users Affected", FieldDisplayName = "Users Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "UsersAffectedUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Describe Impact", FieldDisplayName = "Describe Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "UserImpactDetails", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Outage Description", FieldDisplayName = "Outage Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Outage", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Outage Duration (hrs)", FieldDisplayName = "Outage Duration (hrs)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Duration", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Locations Affected", FieldDisplayName = "Locations Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "LocationMultLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact Areas", FieldDisplayName = "Impact Areas", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DRQSystemsLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Application Affected", FieldDisplayName = "Application Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "APPTitleLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Impact Details", FieldDisplayName = "Impact Details", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "User Notification", FieldDisplayName = "User Notification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Is User Notification Required?", FieldDisplayName = "Is User Notification Required?", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsUserNotificationRequired", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "To Be Sent In The Name Of", FieldDisplayName = "To Be Sent In The Name Of", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Send By Date", FieldDisplayName = "Send By Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ToBeSentByDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Auto Send", FieldDisplayName = "Auto Send", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AutoSend", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Notification Text", FieldDisplayName = "Notification Text", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "NotificationText", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "User Notification", FieldDisplayName = "User Notification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 2 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Primary Responsible Person (PRP)", FieldDisplayName = "Primary Responsible Person (PRP)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PRPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Other Responsible (ORPs)", FieldDisplayName = "Other Responsible (ORPs)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ORPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Assignment", FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comments / Work Activity", FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Resolution", FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Actual Start", FieldDisplayName = "Actual Start", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ScheduledStartDateTime", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Actual End", FieldDisplayName = "Actual End", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ScheduledEndDateTime", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Implementation Plan/Comments", FieldDisplayName = "Implementation Plan/Comments", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "ResolutionComments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "Resolution", FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 3: Summary
            //seqNum = 0;
            //mList.Add(new ModuleFormLayout() { Title = "Summary", FieldDisplayName = "Summary", ModuleNameLookup = ModuleName, TabId = 3,  FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Summary" , FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "ServiceQuestionSummary", FieldDisplayName = "ServiceQuestionSummary", ModuleNameLookup = ModuleName, TabId = 3,  FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false" , FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Summary", FieldDisplayName = "Summary", ModuleNameLookup = ModuleName, TabId = 3,  FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Summary" , FieldSequence = (++seqNum) });

            // Tab 4: Approvals 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 5: Emails
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "TicketEmails", FieldDisplayName = "TicketEmails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });

            // Tab 6: Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "WikiRelatedTickets", FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 7: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

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
                EmailTitle = "DRQ Ticket Returned [$TicketId$]: [$Title$]",
                EmailBody = @"DRQ Ticket ID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter enter the required information and re-submit the ticket.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID,
                EmailUserTypes = "InitiatorUser"
            });
            if (enableManagerApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Manager Approval",
                    EmailTitle = "New DRQ Ticket Opened [$TicketId$]: [$Title$]",
                    EmailBody = @"DRQ Ticket ID [$TicketId$] is pending IT Manager approval.<br><br>" +
                                                        "Please review the request and approve/reject.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = ++stageID,
                    EmailUserTypes = "BusinessManagerUser"
                });
            }

            if (enableSecurityApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Security/DR Approval",
                    EmailTitle = "DRQ Ticket Pending Security/DR Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"DRQ Ticket ID [$TicketId$] is pending Security & DR Approval.<br><br>" +
                                                        "Please review the request and approve/reject.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = ++stageID,
                    EmailUserTypes = "SecurityManagerUser;#DRBRManagerUser"
                });
            }

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Application/Infrastructure Approval",
                EmailTitle = "DRQ Ticket Pending Appl./Infra. Approval [$TicketId$]: [$Title$]",
                EmailBody = @"DRQ Ticket ID [$TicketId$] is pending Application Manager & Infrastructure Manager approval.<br><br>" +
                                                        "Please review the request and approve/reject.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "ApplicationManagerUser;#InfrastructureManagerUser"
            });
            mList.Add(new ModuleTaskEmail()
            {
                Status = "Review & Assign Resource",
                EmailTitle = "DRQ Ticket Pending Assignment [$TicketId$]: [$Title$]",
                EmailBody = @"DRQ Ticket ID [$TicketId$] is pending resource assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "OwnerUser"
            });
            mList.Add(new ModuleTaskEmail()
            {
                Status = "Implementation",
                EmailTitle = "DRQ Ticket Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"DRQ Ticket ID [$TicketId$] has been assigned to you for resolution.<br><br>" +
                                                        "Once resolved, please enter the resolution description and actual hours in the ticket",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "PRP;#ORP"
            });
            mList.Add(new ModuleTaskEmail()
            {
                Status = "DR Closeout",
                EmailTitle = "DRQ Ticket Implementation Complete [$TicketId$]: [$Title$]",
                EmailBody = @"DRQ Ticket ID [$TicketId$] has completed implementation.<br><br>" +
                                                        "Please review and approve the impelmentation.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "DRBRManagerUser"
            });
            if (enablePendingCloseStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Pending Close",
                    EmailTitle = "DRQ Ticket Pending Close [$TicketId$]: [$Title$]",
                    EmailBody = @"DRQ Ticket ID [$TicketId$] has completed implementation and is pending close.<br><br>" +
                                                        "Please review and close the ticket.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = ++stageID,
                    EmailUserTypes = "InfrastructureManagerUser"
                });
            }

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Closed",
                EmailTitle = "DRQ Ticket Closed [$TicketId$]: [$Title$]",
                EmailBody = "DRQ Ticket ID [$TicketId$] has been closed.",
                ModuleNameLookup = ModuleName,
                StageStep = ++stageID,
                EmailUserTypes = "InitiatorUser;#OwnerUser;#InfrastructureManagerUser;#DRBRManagerUser"
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "On-Hold",
                EmailTitle = "DRQ Ticket On Hold [$TicketId$]: [$Title$]",
                EmailBody = "DRQ Ticket ID [$TicketId$] has been placed on hold.",
                ModuleNameLookup = ModuleName,
                StageStep = null,
                EmailUserTypes = "OwnerUser;#RequestorUser"
            });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");

            int moduleStep = 0;

            //Editable in all stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", Title = moduleStep.ToString() + " - " + "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            //dataList.Add(new string[] { "TicketComment", ModuleName, moduleStep.ToString(), "0", "0", "0", "" });

            //Editable in stage 1 - New Ticket
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", Title = moduleStep.ToString() + " - " + "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", Title = moduleStep.ToString() + " - " + "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ChangePurpose", Title = moduleStep.ToString() + " - " + "ChangePurpose", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQRapidTypeLookup", Title = moduleStep.ToString() + " - " + "DRQRapidTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", Title = moduleStep.ToString() + " - " + "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RelatedRequestID", Title = moduleStep.ToString() + " - " + "RelatedRequestID", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TargetStartDate", Title = moduleStep.ToString() + " - " + "TargetStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TargetCompletionDate", Title = moduleStep.ToString() + " - " + "TargetCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BusinessManagerUser", Title = moduleStep.ToString() + " - " + "BusinessManager", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProductionVerificationPlan", Title = moduleStep.ToString() + " - " + "ProductionVerificationPlan", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RecoveryPlan", Title = moduleStep.ToString() + " - " + "RecoveryPlan", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DeploymentPlan", Title = moduleStep.ToString() + " - " + "DeploymentPlan", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TestPlan", Title = moduleStep.ToString() + " - " + "TestPlan", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TesterUser", Title = moduleStep.ToString() + " - " + "Tester", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DeploymentResponsibleUser", Title = moduleStep.ToString() + " - " + "DeploymentResponsible", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProductionVerifyResponsibleUser", Title = moduleStep.ToString() + " - " + "ProductionVerifyResponsible", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RollbackResponsibleUser", Title = moduleStep.ToString() + " - " + "RollbackResponsible", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRBRImpactChoice", Title = moduleStep.ToString() + " - " + "DRBRImpact", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRBRDescription", Title = moduleStep.ToString() + " - " + "DRBRDescription", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RiskChoice", Title = moduleStep.ToString() + " - " + "Risk", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TestingDoneChoice", Title = moduleStep.ToString() + " - " + "TestingDone", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQChangeTypeChoice", Title = moduleStep.ToString() + " - " + "DRQChangeType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRReplicationChangeChoice", Title = moduleStep.ToString() + " - " + "DRReplicationChange", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AssetLookup", Title = moduleStep.ToString() + " - " + "AssetLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactsOrganization", Title = moduleStep.ToString() + " - " + "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BeneficiariesLookup", Title = moduleStep.ToString() + " - " + "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsersAffectedUser", Title = moduleStep.ToString() + " - " + "UsersAffected", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UserImpactDetails", Title = moduleStep.ToString() + " - " + "UserImpactDetails", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationMultLookup", Title = moduleStep.ToString() + " - " + "LocationMultLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Outage", Title = moduleStep.ToString() + " - " + "Outage", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQSystemsLookup", Title = moduleStep.ToString() + " - " + "DRQSystemsLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "APPTitleLookup", Title = moduleStep.ToString() + " - " + "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Duration", Title = moduleStep.ToString() + " - " + "Duration", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsUserNotificationRequired", Title = moduleStep.ToString() + " - " + "IsUserNotificationRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", Title = moduleStep.ToString() + " - " + "Requestor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ToBeSentByDate", Title = moduleStep.ToString() + " - " + "ToBeSentByDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AutoSend", Title = moduleStep.ToString() + " - " + "AutoSend", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "NotificationText", Title = moduleStep.ToString() + " - " + "NotificationText", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            // State 2 - Manager Approval
            if (enableManagerApprovalStage)
                moduleStep++;

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactsOrganization", Title = moduleStep.ToString() + " - " + "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BeneficiariesLookup", Title = moduleStep.ToString() + " - " + "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsersAffectedUser", Title = moduleStep.ToString() + " - " + "UsersAffected", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UserImpactDetails", Title = moduleStep.ToString() + " - " + "UserImpactDetails", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationMultLookup", Title = moduleStep.ToString() + " - " + "LocationMultLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Outage", Title = moduleStep.ToString() + " - " + "Outage", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQSystemsLookup", Title = moduleStep.ToString() + " - " + "DRQSystemsLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Duration", Title = moduleStep.ToString() + " - " + "Duration", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsUserNotificationRequired", Title = moduleStep.ToString() + " - " + "IsUserNotificationRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", Title = moduleStep.ToString() + " - " + "Requestor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ToBeSentByDate", Title = moduleStep.ToString() + " - " + "ToBeSentByDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AutoSend", Title = moduleStep.ToString() + " - " + "AutoSend", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "NotificationText", Title = moduleStep.ToString() + " - " + "NotificationText", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            // Stage 3 - Security/DR Approval
            if (enableSecurityApprovalStage)
                moduleStep++;

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactsOrganization", Title = moduleStep.ToString() + " - " + "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BeneficiariesLookup", Title = moduleStep.ToString() + " - " + "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsersAffectedUser", Title = moduleStep.ToString() + " - " + "UsersAffected", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UserImpactDetails", Title = moduleStep.ToString() + " - " + "UserImpactDetails", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationMultLookup", Title = moduleStep.ToString() + " - " + "LocationMultLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Outage", Title = moduleStep.ToString() + " - " + "Outage", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQSystemsLookup", Title = moduleStep.ToString() + " - " + "DRQSystemsLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "APPTitleLookup", Title = moduleStep.ToString() + " - " + "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Duration", Title = moduleStep.ToString() + " - " + "Duration", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsUserNotificationRequired", Title = moduleStep.ToString() + " - " + "IsUserNotificationRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", Title = moduleStep.ToString() + " - " + "Requestor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ToBeSentByDate", Title = moduleStep.ToString() + " - " + "ToBeSentByDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AutoSend", Title = moduleStep.ToString() + " - " + "AutoSend", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "NotificationText", Title = moduleStep.ToString() + " - " + "NotificationText", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            // Stage 4 - Appl/Infra Approval
            moduleStep++;

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactsOrganization", Title = moduleStep.ToString() + " - " + "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BeneficiariesLookup", Title = moduleStep.ToString() + " - " + "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsersAffectedUser", Title = moduleStep.ToString() + " - " + "UsersAffected", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UserImpactDetails", Title = moduleStep.ToString() + " - " + "UserImpactDetails", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationMultLookup", Title = moduleStep.ToString() + " - " + "LocationMultLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Outage", Title = moduleStep.ToString() + " - " + "Outage", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQSystemsLookup", Title = moduleStep.ToString() + " - " + "DRQSystemsLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "APPTitleLookup", Title = moduleStep.ToString() + " - " + "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Duration", Title = moduleStep.ToString() + " - " + "Duration", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsUserNotificationRequired", Title = moduleStep.ToString() + " - " + "IsUserNotificationRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", Title = moduleStep.ToString() + " - " + "Requestor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ToBeSentByDate", Title = moduleStep.ToString() + " - " + "ToBeSentByDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AutoSend", Title = moduleStep.ToString() + " - " + "AutoSend", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "NotificationText", Title = moduleStep.ToString() + " - " + "NotificationText", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            //Editable in stage 5 - Pending Assignment
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", Title = moduleStep.ToString() + " - " + "PRP", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", Title = moduleStep.ToString() + " - " + "ORP", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactsOrganization", Title = moduleStep.ToString() + " - " + "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BeneficiariesLookup", Title = moduleStep.ToString() + " - " + "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsersAffectedUser", Title = moduleStep.ToString() + " - " + "UsersAffected", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UserImpactDetails", Title = moduleStep.ToString() + " - " + "UserImpactDetails", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationMultLookup", Title = moduleStep.ToString() + " - " + "LocationMultLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Outage", Title = moduleStep.ToString() + " - " + "Outage", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQSystemsLookup", Title = moduleStep.ToString() + " - " + "DRQSystemsLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "APPTitleLookup", Title = moduleStep.ToString() + " - " + "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Duration", Title = moduleStep.ToString() + " - " + "Duration", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsUserNotificationRequired", Title = moduleStep.ToString() + " - " + "IsUserNotificationRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", Title = moduleStep.ToString() + " - " + "Requestor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ToBeSentByDate", Title = moduleStep.ToString() + " - " + "ToBeSentByDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AutoSend", Title = moduleStep.ToString() + " - " + "AutoSend", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "NotificationText", Title = moduleStep.ToString() + " - " + "NotificationText", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            //Editable in stage 6 - Implementation
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", Title = moduleStep.ToString() + " - " + "PRP", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "OwnerUser" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", Title = moduleStep.ToString() + " - " + "ORP", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "OwnerUser;#PRPUser" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ScheduledStartDateTime", Title = moduleStep.ToString() + " - " + "ScheduledStartDateTime", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ScheduledStartDateTime", Title = moduleStep.ToString() + " - " + "ScheduledStartDateTime", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionComments", Title = moduleStep.ToString() + " - " + "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactsOrganization", Title = moduleStep.ToString() + " - " + "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BeneficiariesLookup", Title = moduleStep.ToString() + " - " + "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UsersAffectedUser", Title = moduleStep.ToString() + " - " + "UsersAffected", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = true, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "UserImpactDetails", Title = moduleStep.ToString() + " - " + "UserImpactDetails", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationMultLookup", Title = moduleStep.ToString() + " - " + "LocationMultLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Outage", Title = moduleStep.ToString() + " - " + "Outage", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DRQSystemsLookup", Title = moduleStep.ToString() + " - " + "DRQSystemsLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "APPTitleLookup", Title = moduleStep.ToString() + " - " + "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Duration", Title = moduleStep.ToString() + " - " + "Duration", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsUserNotificationRequired", Title = moduleStep.ToString() + " - " + "IsUserNotificationRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", Title = moduleStep.ToString() + " - " + "Requestor", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ToBeSentByDate", Title = moduleStep.ToString() + " - " + "ToBeSentByDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "AutoSend", Title = moduleStep.ToString() + " - " + "AutoSend", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "NotificationText", Title = moduleStep.ToString() + " - " + "NotificationText", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, ActionUser = "" });

            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            //mList.Add(new ModuleRequestType() { RequestType = "ACR", Title = "ACR", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            //mList.Add(new ModuleRequestType() { RequestType = "PMM", Title = "PMM", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            //mList.Add(new ModuleRequestType() { RequestType = "TSK", Title = "TSR", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            //mList.Add(new ModuleRequestType() { RequestType = "TSR", Title = "TSR", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });

            //mList.Add(new ModuleRequestType() { RequestType = "PRS", Title = "PRS", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            //mList.Add(new ModuleRequestType() { RequestType = "BTS", Title = "BTS", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            //mList.Add(new ModuleRequestType() { RequestType = "INC", Title = "INC", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            //mList.Add(new ModuleRequestType() { RequestType = "N/A", Title = "N/A", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "DRQ", WorkflowType = "Full", Owner = ConfigData.Variables.Name });

            mList.Add(new ModuleRequestType() { Title = "ACR", Category = "DRQ", RequestType = "ACR", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "INC", Category = "DRQ", RequestType = "INC", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "N/A", Category = "DRQ", RequestType = "N/A", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "PMM", Category = "DRQ", RequestType = "PMM", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "PRS", Category = "DRQ", RequestType = "PRS", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "TSR", Category = "DRQ", RequestType = "TSR", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", });

            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            // Start from ID 22
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 9;
            int moduleStep = 0;
            if (!enableSecurityApprovalStage)
                numStages--;
            if (!enableManagerApprovalStage)
                numStages--;
            if (!enablePendingCloseStage)
                numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Initiated",
                UserPrompt = "<b>&nbsp;Please complete the form to open a new Change Management.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ActionUser = "InitiatorUser;#OwnerUser;#RequestorUser;#Admin;#BusinessManagerUser",
                ApproveActionDescription = "New Request Created",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "(Re)Submit",
                StageRejectedButtonName = "Cancel",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            ///Not a stable stage :)
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
                    SkipOnCondition = "",
                    StageTypeChoice = Convert.ToString(StageType.None)
                });
            }

            if (enableSecurityApprovalStage)
            {
                mList.Add(new LifeCycleStage()
                {
                    Action = "Security/DR Approval",
                    Name = "Security Approval",
                    StageTitle = "Security Approval",
                    UserWorkflowStatus ="Awaiting Security / DR Manager's approval",
                    ActionUser = "SecurityManagerUser;#DRBRManagerUser;#BusinessManagerUser;#OwnerUser;#RequestorUser;#Admin;#InitiatorUser",
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
                    StageWeight = 10,
                    ShortStageTitle = "",
                    CustomProperties = "",
                    SkipOnCondition = "[TicketRapidRequest] = 'Yes'",
                    StageTypeChoice = Convert.ToString(StageType.None)
                });
            }

            mList.Add(new LifeCycleStage()
            {
                Action = "Application/Infrastructure Approval",
                Name = "Application/Infrastructure Approval",
                StageTitle = "Application/Infrastructure Approval",
                ActionUser = "ApplicationManagerUser;#InfrastructureManagerUser",
                UserWorkflowStatus = "Awaiting Application/Infrastructure Manager's approval",
                UserPrompt = "<b>Application/Infrastructure Manager:</b>&nbsp;Please approve the ticket.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Application/Infrastructure manager approved request",
                RejectActionDescription = "Application/Infrastructure manager rejected request",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "[Urgent Request] = 'Yes'",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Review & Assign Resource",
                Name = "Review & Assign Resource",
                StageTitle = "Awaiting PRP assignment by Owner",
                ActionUser = "OwnerUser",
                UserWorkflowStatus = "Awaiting Primary Responsible Person(PRP) assignment by Owner",
                UserPrompt = "<b>Owner:</b>&nbsp;Please assign a Primary Responsible Person (PRP) and the Estimated hours on work activity tab.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Owner assigned PRP/ORP",
                RejectActionDescription = "Owner assigned PRP/ORP",
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
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Implementation",
                Name = "Implementation",
                StageTitle = "Awaiting resolution by PRP/ORP",
                ActionUser = "PRP;#ORP",
                UserWorkflowStatus = "Awaiting resolution by Primary Responsible Person(PRP) /Other Responsible Person(ORP) ",
                UserPrompt = "<b>PRP:</b>&nbsp;Please complete implementation details and click on Implementation Done",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PRP implemented DRQ",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Implementation Done",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 40,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "DR Closeout",
                Name = "DR Closeout",
                StageTitle = "Awaiting Final approval by DR Manager",
                ActionUser = "DRBRManagerUser",
                UserWorkflowStatus = "Awaiting Final approval by DR Manager",
                UserPrompt = "<b>DR Manager:</b>&nbsp;Please approve the implementation",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                ApproveActionDescription = "DR Manager approved implementation",
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
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            moduleTestedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Infrastructure Manager Closed",
                Name = "Pending Close",
                StageTitle = "Awaiting Final approval by DR Manager",
                ActionUser = "InfrastructureManagerUser",
                UserWorkflowStatus = "Awaiting Infrastructure Manager approval to close the ticket",
                UserPrompt = "<b>Infrastructure Manager:</b>&nbsp;Please approve the implementation to close the ticket",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                ApproveActionDescription = "DR Manager approved implementation",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 4,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Tested)
            });

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            mList.Add(new LifeCycleStage()
            {
                Action = "Ticket is closed, but you can add comments or can be re-opened by Owner",
                Name = "Closed",
                StageTitle = "Ticket Closed",
                ActionUser = "OwnerUser",
                UserWorkflowStatus ="Ticket is closed, but you can add comments or can be re-opened by Owner",
                UserPrompt = "Closed",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Closed",
                RejectActionDescription = "",
                ReturnActionDescription = "Owner re-opened ticket",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 5,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });


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
            // Console.WriteLine("  DRQRapidTypes");

            mList.Add(new DRQRapidType() { Title = "Add Patch" });
            mList.Add(new DRQRapidType() { Title = "Bug Fix" });
            mList.Add(new DRQRapidType() { Title = "Data Fix" });
            mList.Add(new DRQRapidType() { Title = "Infrastructure Change" });
            mList.Add(new DRQRapidType() { Title = "Operational Necessity" });

            return mList;
        }

        public List<DRQSystemArea> GetDRQSystemAreas()
        {
            List<DRQSystemArea> mList = new List<DRQSystemArea>();
            // Console.WriteLine("  DRQSystemAreas");

            mList.Add(new DRQSystemArea() { Title = "Unix (AIX)" });
            mList.Add(new DRQSystemArea() { Title = "Windows" });
            mList.Add(new DRQSystemArea() { Title = "Linux" });
            mList.Add(new DRQSystemArea() { Title = "iSeries (AS/400)" });

            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "DRQTickets", TabOrder = 1, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "DRQTickets", TabOrder = 2, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Items", ViewName = "DRQTickets", TabOrder = 3, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Items", ViewName = "DRQTickets", TabOrder = 4, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Items", ViewName = "DRQTickets", TabOrder = 5, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Items", ViewName = "DRQTickets", TabOrder = 6, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Items", ViewName = "DRQTickets", TabOrder = 7, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Items", ViewName = "DRQTickets", TabOrder = 8, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Items", ViewName = "DRQTickets", TabOrder = 9, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "DRQTickets", TabOrder = 10, ModuleNameLookup = "DRQ", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });

            return tabs;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            //Assigning vale for impacts
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "DRQ", Title = "DRQ-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "DRQ", Title = "DRQ-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "DRQ", Title = "DRQ-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            //Assigning vale for serverities
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "DRQ", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "DRQ", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "DRQ", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });

            //Assigning vale for priorities
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "2-High", uPriority = "2-High", ItemOrder = 1, Color = "#FF7F1D" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 , Color = "#FFED14" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3, Color = "#cccfd2" });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "DRQ", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4, IsVIP = true, Color = "#FF351F" });

            //Assigning value from request priorities
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 1, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 2, SeverityLookup = 0, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "DRQ", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });


        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
    }
}

