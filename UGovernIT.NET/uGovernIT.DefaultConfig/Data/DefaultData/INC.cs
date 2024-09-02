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
    public class INC : IModule
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

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Outage Incidents",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "INC",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/incidents",
                    ModuleHoldMaxStage = 4,
                    Title = "Outages (INC)",
                    ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details. Or, select a ticket by clicking checkbox and clicking Actions button.",
                    ThemeColor = "Accent6",
                    StaticModulePagePath = "/Pages/inc",
                    ModuleType = ModuleType.SMS,
                    EnableModule = true,
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

        public string ModuleName
        {
            get
            {
                return "INC";
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
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            mList.Add(new ModuleImpact() { ModuleNameLookup = "INC", Title = "INC-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "INC", Title = "INC-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "INC", Title = "INC-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            return mList;
        }
        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4 });
            return mList;
        }
        public List<ModuleSeverity> GetModuleSeverity()
        {
            // Console.WriteLine(" TicketSeverity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "INC", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "INC", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "INC", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });
            return mList;
        }

        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            // Console.WriteLine("RequestPriority");
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 21, SeverityLookup = 19, PriorityLookup = 22 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 21, SeverityLookup = 20, PriorityLookup = 22 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 21, SeverityLookup = 21, PriorityLookup = 23 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 22, SeverityLookup = 19, PriorityLookup = 22 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 22, SeverityLookup = 21, PriorityLookup = 23 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 22, SeverityLookup = 21, PriorityLookup = 24 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 23, SeverityLookup = 19, PriorityLookup = 23 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 23, SeverityLookup = 21, PriorityLookup = 24 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 23, SeverityLookup = 21, PriorityLookup = 24 });
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "INC", Title = "INC-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "INC", Title = "INC-Business Manager", UserTypes = "Business Approver", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "INC", Title = "INC-Requestor", UserTypes = "Requestor", ColumnName = "RequestorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "INC", Title = "INC-Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "INC", Title = "INC-PRP", UserTypes = "PRP", ColumnName = "PRPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "INC", Title = "INC-ORP", UserTypes = "ORP", ColumnName = "ORPUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });

            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            // Start from ID - 52
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 6;
            string closeStageID = (currStageID + numStages - 1).ToString();

            List<string[]> dataList = new List<string[]>();
            mList.Add(new LifeCycleStage()
            {
                Action = "New Incident",
                Name = "New Incident",
                StageTitle = "New Incident",
                UserPrompt = "<b>&nbsp;Please complete the form to open a New Incident.</b>",
                StageStep = 1,
                ModuleNameLookup = ModuleName,
                ActionUser = "InitiatorUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "New Incident Created",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Re-Submit",
                StageRejectedButtonName = "Reject",
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
            mList.Add(new LifeCycleStage()
            {
                Action = "Pending Approval",
                Name = "Pending Approval",
                StageTitle = "Pending Approval",
                UserPrompt = "<b>Business Manager:</b>&nbsp;Please approve the ticket.",
                UserWorkflowStatus = "Awaiting Business manager's approval",
                StageStep = 2,
                ModuleNameLookup = ModuleName,
                ActionUser = "BusinessManagerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Manager approved request",
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
                SkipOnCondition = "[TicketRequestTypeWorkflow] = 'SkipApprovals'",
                StageTypeChoice = Convert.ToString(StageType.None)
            });


            mList.Add(new LifeCycleStage()
            {
                Action = "PRP Assigned",
                Name = "Pending Assignment",
                StageTitle = "Pending Assignment",
                UserPrompt = "<b>Owner:</b>&nbsp;Please assign a Primary Responsible Person (PRP) and the Estimated hours on work activity tab.",
                UserWorkflowStatus = "Awaiting Primary Responsible Person (PRP) assignment by Incident Owner",
                StageStep = 3,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
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
                SkipOnCondition = "[TicketRequestTypeWorkflow] = 'SkipApprovals'",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            string assignStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Assigned",
                Name = "Assigned",
                UserWorkflowStatus = "Awaiting resolution by Primary Responsible Person(PRP)/Other Responsible Person(ORP)",
                StageTitle = "Assigned",
                UserPrompt = "<b>PRP/ORP:</b>&nbsp;Please enter Resolution Description.",
                StageStep = 4,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser;#PRPUser;#ORPUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PRP resolved incident",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Resolved",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 60,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            moduleTestedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Pending Close",
                UserWorkflowStatus = "Awaiting Owner approval to close the incident",
                StageTitle = "Pending Close",
                UserPrompt = "<b>Owner:</b>&nbsp;Please approve the resolution to close the incident",
                StageStep = 5,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(assignStageID == "" ? "0" : assignStageID),
                ApproveActionDescription = "Owner approved resolution and closed incident",
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
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Pending Close",
                UserWorkflowStatus = "Incident is closed, but you can add comments or can be re-opened by Owner",
                StageTitle = "Closed",
                UserPrompt = "Incident Closed",
                StageStep = 5,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "Owner approved resolution and closed incident",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 3,
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
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "RequestorUser", FieldDisplayName = "Requested By", IsDisplay = true, ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "Attachments", FieldDisplayName = " ", IsUseInWildCard = true, IsDisplay = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsUseInWildCard = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "Age", FieldDisplayName = "Age", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "CreationDate", FieldDisplayName = "Created On", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "CloseDate", FieldDisplayName = "Closed On", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "StageActionUsersUser", FieldDisplayName = "Waiting On", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "PRPUser", FieldDisplayName = "PRP", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "ORPUser", FieldDisplayName = "ORP", ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "TesterUser", FieldDisplayName = "Tester", ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "INC", FieldName = "RequestTypeLookup", FieldDisplayName = "Incident Type", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");
            int seqNum = 0;
            // Tab 1
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Request Source", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestSourceChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Business Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Affected Application / Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Asset/Server", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AssetLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Incident Occurred", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OccurrenceDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Incident Detected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "DetectionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Severity", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "TicketSeverityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impacts Organization", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImpactsOrganization", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impacts Users", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "AffectedUsersUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impacts Department(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 2: Assign PRP & ORP
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Primary Responsible Person (PRP)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PRP", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Other Responsible (ORPs)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ORP", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Hours spent", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ActualHours", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Outage Duration (hrs)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "OutageHours", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution Description", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "ResolutionComments", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 3: Approvals
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 4: Emails
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });

            // Tab 5: Related s
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 6: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

            // New  Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Request Source", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestSourceChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Business Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Affected Application / Area", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Asset / Server", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AssetLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Incident Occurred", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OccurrenceDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Incident Detected", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "DetectionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Severity", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Severity", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impacts Organization", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImpactsOrganization", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impacts Users", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "AffectedUsersUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impacts Department(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Asset / Server", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "AssetLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");


            /*
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "Broderick", RequestType = "Broderick", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 4, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "BI" + " > " + "Data Tracker", RequestType = "Data Tracker", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "BI", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "DSRP", RequestType = "DSRP", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "E1 - EAM", RequestType = "E1 - EAM", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "E1 - Accounts Payable", RequestType = "E1 - Accounts Payable", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "E1 - Fixed Assets", RequestType = "E1 - Fixed Assets", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "E1 - General Ledger", RequestType = "E1 - General Ledger", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "E1 - Technical", RequestType = "E1 - Technical", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "EDI", RequestType = "EDI", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "WESB" + " > " + "Enterprise Integration", RequestType = "Enterprise Integration", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "WESB", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "FaxStar", RequestType = "FaxStar", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 4, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Finance", RequestType = "Finance", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Internet/Intranet" + " > " + "Intranet", RequestType = "Intranet", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Internet/Intranet", FunctionalAreaLookup = 5, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "Kronos", RequestType = "Kronos", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "LaserVault", RequestType = "LaserVault", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Legacy" + " > " + "Legacy Application", RequestType = "Legacy Application", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Legacy", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Manufacturing", RequestType = "Manufacturing", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "BI" + " > " + "OBIEE", RequestType = "OBIEE", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "BI", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "ODS (Object Distribution System)", RequestType = "ODS (Object Distribution System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 3, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "OMS (Object Mirrorring System)", RequestType = "OMS (Object Mirrorring System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 3, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "PC Miler", RequestType = "PC Miler", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 4, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "PowerLock", RequestType = "PowerLock", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 4, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Procurement", RequestType = "Procurement", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "BI" + " > " + "QlikView", RequestType = "QlikView", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "BI", FunctionalAreaLookup = 2, WorkflowType = "Full" });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "STRAP", RequestType = "STRAP", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Sales Order Entry", RequestType = "Sales Order Entry", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Sales Order Management", RequestType = "Sales Order Management", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "Screen Manager", RequestType = "Screen Manager", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 4, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "Service Tracking System", RequestType = "Service Tracking System", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 5, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Supply Chain", RequestType = "Supply Chain", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other 3rd-party SW" + " > " + "Sysco Quick Review", RequestType = "Sysco Quick Review", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "TMS (Traffic Management System)", RequestType = "TMS (Traffic Management System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "TPMS (Trade Promotion Management System)", RequestType = "TPMS (Trade Promotion Management System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Technical", RequestType = "Technical", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Non-ERP Applications" + " > " + "TerraTech", RequestType = "TerraTech", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "WESB" + " > " + "WESB", RequestType = "WESB", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "WESB", FunctionalAreaLookup = 2, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "JDEdwards" + " > " + "Warehouse Mgmnt.", RequestType = "Warehouse Mgmnt.", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", FunctionalAreaLookup = 1, WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            */

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



            //Editable in all stages
            mList.Add(new ModuleRoleWriteAccess() { Title = 0 + " - " + "AmountDisputed", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            //mList.Add(new ModuleRoleWriteAccess() { "Comment", ModuleNameLookup =ModuleName, "0", "0", "0", "0", "" });

            // Stage 1 - New 
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "RequestSource", FieldName = "RequestSourceChoice", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "OccurrenceDate", FieldName = "OccurrenceDate", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "DetectionDate", FieldName = "DetectionDate", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "PRSLookup", FieldName = "PRSLookup", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "AssetLookup", FieldName = "AssetLookup", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "ImpactsOrganization", FieldName = "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "BeneficiariesLookup", FieldName = "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 1 + " - " + "AffectedUsers", FieldName = "AffectedUsersUser", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });

            // Stage 2 - Manager Approval
            mList.Add(new ModuleRoleWriteAccess() { Title = 2 + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, });
            mList.Add(new ModuleRoleWriteAccess() { Title = 2 + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 2 + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 2 + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 2 + " - " + "ImpactsOrganization", FieldName = "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 2 + " - " + "BeneficiariesLookup", FieldName = "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 2 + " - " + "AffectedUsers", FieldName = "AffectedUsersUser", ModuleNameLookup = ModuleName, StageStep = 2, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            // Stage 3 - Pending Assignment
            mList.Add(new ModuleRoleWriteAccess() { Title = 3 + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 3 + " - " + "PRP", FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 3 + " - " + "ORP", FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 3 + " - " + "ImpactsOrganization", FieldName = "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 3 + " - " + "BeneficiariesLookup", FieldName = "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 3 + " - " + "AffectedUsers", FieldName = "AffectedUsersUser", ModuleNameLookup = ModuleName, StageStep = 3, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });

            // Stage 4 - Assigned
            mList.Add(new ModuleRoleWriteAccess() { Title = 4 + " - " + "PRP", FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 4 + " - " + "ORP", FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 4 + " - " + "ActualHours", FieldName = "ActualHours", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 4 + " - " + "ResolutionComments", FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 4 + " - " + "ImpactsOrganization", FieldName = "ImpactsOrganization", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 4 + " - " + "BeneficiariesLookup", FieldName = "Beneficiaries", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = 4 + " - " + "AffectedUsers", FieldName = "AffectedUsersUser", ModuleNameLookup = ModuleName, StageStep = 4, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            // Stage 5 - Pending Close
            mList.Add(new ModuleRoleWriteAccess() { Title = 5 + " - " + "ResolutionComments", FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = 5, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  StatusMapping");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "New Incident", ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Pending Approval", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Pending Assignment", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 1 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Pending Resolution", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Pending Close", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Closed", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4 });
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "New Incident",
                Status = "New Incident",
                EmailTitle = "Incident Returned [$TicketId$]: [$Title$]",
                EmailBody = @"Incident TicketID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter any required information and re-submit it",
                ModuleNameLookup = ModuleName,
                StageStep = stageID
            });

            // Two emails in stage 2
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Pending Approval - Requestor",
                Status = "Pending Approval - Requestor",
                EmailTitle = "New Incident Created [$TicketId$]: [$Title$]",
                EmailBody = @"Incident [$TicketId$] has been created on your behalf.<br><br>" +
                                                        "Please review the details below. You will be notified when the incident is resolved.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Pending Approval",
                Status = "Pending Approval",
                EmailTitle = "New Incident Pending Approval [$TicketId$]: [$Title$]",
                EmailBody = @"Incident [$TicketId$] is pending your approval.<br><br>" +
                                                        "Please assign/update the owner and approve/reject the incident.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = stageID
            });

            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Pending Assignment",
                Status = "Pending Assignment",
                EmailTitle = "Incident Pending Assignment [$TicketId$]: [$Title$]",
                EmailBody = @"Incident [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please review the incident and assign a PRP",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + " - " + "Assigned",
                Status = "Assigned",
                EmailTitle = "Incident Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"Incident [$TicketId$] has been assigned to you for resolution.<br><br>" +
                                                        "Once resolved, please enter the resolution description and actual hours, and mark as resolved.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Pending Close", Status = "Pending Close", EmailTitle = "Incident Pending Close [$TicketId$]: [$Title$]", EmailBody = "Incident [$TicketId$] has been resolved and is pending close.<br><br>[$IncludeActionButtons$]", ModuleNameLookup = ModuleName, StageStep = (++stageID) });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "Closed", Status = "Closed", EmailTitle = "Incident Closed [$TicketId$]: [$Title$]", EmailBody = "Incident TicketID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (++stageID) });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + " - " + "On-Hold", Status = "On-Hold", EmailTitle = "Incident On Hold [$TicketId$]: [$Title$]", EmailBody = "Incident TicketID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null });
            return mList;
        }



        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "Incidents", TabOrder = 1, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "Incidents", TabOrder = 2, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "Incidents", TabOrder = 3, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "Incidents", TabOrder = 4, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "Incidents", TabOrder = 5, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "Incidents", TabOrder = 6, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "Incidents", TabOrder = 7, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "Incidents", TabOrder = 8, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "Incidents", TabOrder = 9, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "Incidents", TabOrder = 10, ModuleNameLookup = "INC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            return tabs;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "INC", Title = "INC-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "INC", Title = "INC-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "INC", Title = "INC-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "INC", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "INC", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "INC", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });

            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "INC", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4, IsVIP = true });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 1, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 2, SeverityLookup = 0, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "INC", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });

        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
    }
}
