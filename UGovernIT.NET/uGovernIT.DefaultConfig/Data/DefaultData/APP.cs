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
    public class APP : IModule
    {
        protected static bool enableOwnerApprovalStage = true;
        protected static bool enableFinanceApprovalStage = true;
        protected static bool enableLegalApprovalStage = true;
        protected static bool enablePurchasingStage = true;
        public static string userId = "1";
        public static string managerUserId = "1";
        public static string userName = ""; // Obtained from userId in UGovernITDefault.Initialize()

        public static string[] HideInTicketTemplate = { "Attachments", "AssetLookup", "Comment", "CreationDate", "Initiator", "RequestTypeCategory", "Status" };

        public static int currStageID = 0;
        public static bool loadRequestTypes = true;
        public static bool loadModuleStages = true;
        public static string adminGroupName = "uGovernIT Admins";
        public static string membersGroupName = "uGovernIT Members";
        public static int totalNumberOfTickets = 0;

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
                    ShortName = "Application Management",
                    CategoryName = "Resource Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "Applications",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/application",
                    ModuleHoldMaxStage = 0,
                    Title = "Application Management (APP)",
                    ModuleDescription = "This module is used to track applications, application support groups, access and application infrastructure.",                    
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/app",
                    ModuleType = ModuleType.Governance,
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
                return "APP";
            }
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {

        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
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
            mList.Add(new ModuleFormTab() { TabName = "Application Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Modules", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Work Activity" });
            mList.Add(new ModuleFormTab() { TabName = "Roles", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Roles" });
            mList.Add(new ModuleFormTab() { TabName = "Access Control", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Access Control" });
            mList.Add(new ModuleFormTab() { TabName = "Application Infrastructure", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Application Infrastructure" });
            mList.Add(new ModuleFormTab() { TabName = "Passwords", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Passwords" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            return mList;
            //throw new NotImplementedException();
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

            // Start from ID 74
            //currStageID = 74;
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 3;
            int moduleStep = 0;

            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Name = "Create",
                StageTitle = "Create",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Please fill the form to create a new Application</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Create",
                ActionUser = "OwnerUser;#AccessAdminUser;#" + adminGroupName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Application Created",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Create",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });
            mList.Add(new LifeCycleStage()
            {
                Name = "Prepare/Upgrade",
                StageTitle = "Prepare/Upgrade",
                UserWorkflowStatus = "",
                UserPrompt = "",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "",
                ActionUser = "OwnerUser;#AdminUser;#uGovernIT Admins;#" + adminGroupName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Application Created",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Activate",
                StageRejectedButtonName = "Retire",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Active",
                StageTitle = "Active",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Application is Active</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Active",
                ActionUser = "OwnerUser;#AccessAdminUser;#" + adminGroupName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Application Retired",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Retire",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 90,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });
            mList.Add(new LifeCycleStage()
            {
                Name = "Decommission",
                StageTitle = "Decommission",
                UserWorkflowStatus = "",
                UserPrompt = "<b>Application is Active</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Decommissioned",
                ActionUser = "OwnerUser;#AdminUser;#uGovernIT Admins;#" + adminGroupName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Application Retired",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Retire",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Reactivate",
                StageAllApprovalsRequired = false,
                StageWeight = 90,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Retired",
                StageTitle = "Retired",
                UserWorkflowStatus = "Application is retired, but you can re-activate",
                UserPrompt = "Application Retired",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Retired",
                ActionUser = "OwnerUser;#AccessAdminUser;#" + adminGroupName,
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "Application Re-Activated",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Activate",
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
            // APP
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "TicketId", FieldDisplayName = "Application ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "CategoryNameChoice", FieldDisplayName = "Category", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", ColumnType = "ProgressBar", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "OwnerUser", FieldDisplayName = "IT Owner", IsDisplay = true, DisplayForClosed = true, ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "BusinessManagerUser", FieldDisplayName = "Business Owner", IsDisplay = true, DisplayForClosed = true, ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "DepartmentLookup", FieldDisplayName = "Department", IsDisplay = true, DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "AccessAdminUser", FieldDisplayName = "Access Admin", IsDisplay = true, DisplayForClosed = true, ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "SupportedByUser", FieldDisplayName = "Supported By", IsDisplay = true, DisplayForClosed = true, ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "APP", FieldName = "CloseDate", FieldDisplayName = "Retired On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
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
            // Tab 1
            mList.Add(new ModuleFormLayout() { Title = "Application", FieldDisplayName = "Application", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Application Name", FieldDisplayName = "Application Name", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Status", FieldDisplayName = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Category", FieldDisplayName = "Category", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CategoryName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sub-Category", FieldDisplayName = "Sub-Category", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SubCategory", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Supported By", FieldDisplayName = "Supported By", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SupportedByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Department", FieldDisplayName = "Department", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DepartmentLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IT Owner", FieldDisplayName = "IT Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Business Owner", FieldDisplayName = "Business Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Access Admin", FieldDisplayName = "Access Admin", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AccessAdminUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approver(s)", FieldDisplayName = "Approver(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ApproverUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approval Needed", FieldDisplayName = "Approval Needed", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ApprovalTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Functional Area", FieldDisplayName = "Functional Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sync to Request Type", FieldDisplayName = "Sync to Request Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SyncToRequestType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sync at Module Level", FieldDisplayName = "Sync at Module Level", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SyncAtModuleLevel", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });


            //mList.Add(new ModuleFormLayout() { Title = "# of Licenses", FieldDisplayName = "# of Licenses", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "NumLicensesTotal", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "LicenseBasis", FieldDisplayName = "LicenseBasis", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "LicenseBasis", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "# of Users", FieldDisplayName = "# of Users", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "NumUsers", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Licensing Notes", FieldDisplayName = "Licensing Notes", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Licensing Notes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Hosting Type", FieldDisplayName = "Hosting Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "HostingType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Vendor", FieldDisplayName = "Vendor", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "VendorLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "In Production Since", FieldDisplayName = "In Production Since", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InProductionSince", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Freq of Upgrades", FieldDisplayName = "Freq of Upgrades", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FrequencyOfUpgrades", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Next Planned Upgrade", FieldDisplayName = "Next Planned Upgrade", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "NextPlannedMajorUpgrade", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Version Installed", FieldDisplayName = "Version Installed", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "VersionInstalled", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Latest Release", FieldDisplayName = "Latest Release", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "VersionLatestRelease", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });


            mList.Add(new ModuleFormLayout() { Title = "Application Description", FieldDisplayName = "Application Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "Application", FieldDisplayName = "Application", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 2: Components 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Modules", FieldDisplayName = "Modules", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ApplicationModules", FieldDisplayName = "Application Modules", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Modules", FieldDisplayName = "Modules", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 3: Roles
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Roles", FieldDisplayName = "Roles", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Application Role", FieldDisplayName = "Application Role", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Roles", FieldDisplayName = "Roles", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 4: Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Access Control", FieldDisplayName = "Access Control", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ModuleRoleMapControl", FieldDisplayName = "ModuleRole Map Control", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Access Control", FieldDisplayName = "Access Control", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 5
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Application Infrastructure", FieldDisplayName = "Application Infrastructure", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ServersControl", FieldDisplayName = "Servers Control", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Application Infrastructure", FieldDisplayName = "Application Infrastructure", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 6
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Password", FieldDisplayName = "Password", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "PasswordControl", FieldDisplayName = "Password Control", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Password", FieldDisplayName = "Password", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 7
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Lifecycle", FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 8
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Application", FieldDisplayName = "Application", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Application Name", FieldDisplayName = "Application Name", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Category", FieldDisplayName = "Category", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "CategoryName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sub-Category", FieldDisplayName = "Sub-Category", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SubCategory", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Supported By", FieldDisplayName = "Supported By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SupportedByUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Department", FieldDisplayName = "Department", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DepartmentLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IT Owner", FieldDisplayName = "IT Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Business Owner", FieldDisplayName = "Business Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Access Admin", FieldDisplayName = "Access Admin", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AccessAdminUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approver(s)", FieldDisplayName = "Approver(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ApproverUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approval Needed", FieldDisplayName = "Approval Needed", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ApprovalTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //mList.Add(new ModuleFormLayout() { Title = "# of Licenses",FieldDisplayName = "# of Licenses",ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1,FieldName = "NumLicensesTotal",ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "LicenseBasis", FieldDisplayName = "License Basis", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "LicenseBasis", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //mList.Add(new ModuleFormLayout() { Title = "Licensing Notes", FieldDisplayName = "Licensing Notes", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "NumLicensesTotalNotes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Functional Area", FieldDisplayName = "Functional Area", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sync to Request Type", FieldDisplayName = "Sync to Request Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SyncToRequestType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Sync at Module Level", FieldDisplayName = "Sync at Module Level", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SyncAtModuleLevel", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Application Description", FieldDisplayName = "Application Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "Application", FieldDisplayName = "Application", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            /*
            mList.Add(new ModuleRequestType() { Title = "Application" + " > " + "WEB", RequestType = "WEB", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Application", FunctionalAreaLookup = 1, WorkflowType = "Requisition", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Application" + " > " + "WINDOW", RequestType = "WINDOW", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Application", FunctionalAreaLookup = 1, WorkflowType = "Requisition", IsDeleted = false, Owner = ConfigData.Variables.Name });
            */

            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "3rd Party Apps", "3rd Party Tool"), Category = "3rd Party Apps", RequestType = "3rd Party Tool", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "BI/Analytical", "BI Tool"), Category = "BI/Analytical", RequestType = "BI Tool", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Collaboration", "Collaboration Tool"), Category = "Collaboration", RequestType = "Collaboration Tool", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "CRM", "CRM Modules"), Category = "CRM", RequestType = "CRM Module", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Custom Solutions", "Custom Software"), Category = "Custom Solutions", RequestType = "Custom Software", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Accounts Payable"), Category = "ERP", RequestType = "Accounts Payable", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Accounts Receivable"), Category = "ERP", RequestType = "Accounts Receivable", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Finance"), Category = "ERP", RequestType = "Finance", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Fixed Assets"), Category = "ERP", RequestType = "Fixed Assets", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "General Ledger"), Category = "ERP", RequestType = "General Ledger", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Manufacturing"), Category = "ERP", RequestType = "Manufacturing", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Procurement"), Category = "ERP", RequestType = "Procurement", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Sales Order Management"), Category = "ERP", RequestType = "Sales Order Management", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Supply Chain"), Category = "ERP", RequestType = "Supply Chain", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "ERP", "Warehouse Management"), Category = "ERP", RequestType = "Warehouse Management", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Legacy", "Legacy Application"), Category = "Legacy", RequestType = "Legacy Application", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", WorkflowType = "Full", Owner = "Admin", PRPGroup = "Admin" });


            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int moduleStep = 0;
            // All stages
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "CategoryName", FieldName = "CategoryName", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "SubCategory", FieldName = "SubCategory", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "FunctionalAreaLookup", FieldName = "FunctionalAreaLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "AccessAdmin", FieldName = "AccessAdminUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "SupportedBy", FieldName = "SupportedByUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "SyncToRequestType", FieldName = "SyncToRequestType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "SyncAtModuleLevel", FieldName = "SyncAtModuleLevel", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "Approver", FieldName = "ApproverUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "ApprovalType", FieldName = "ApprovalTypeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "DepartmentLookup", FieldName = "DepartmentLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "NumLicensesTotal", FieldName = "NumLicensesTotal", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "LicenseBasis", FieldName = "LicenseBasisChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "Licensing Notes", FieldName = "NumLicensesTotalNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "VersionLatestRelease", FieldName = "VersionLatestRelease", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "VersionInstalled", FieldName = "VersionInstalled", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "NextPlannedMajorUpgrade", FieldName = "NextPlannedMajorUpgrade", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "FrequencyOfUpgrades", FieldName = "FrequencyOfUpgradesChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "InProductionSince", FieldName = "InProductionSince", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "VendorLookup", FieldName = "VendorLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "HostingType", FieldName = "HostingTypeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, CustomProperties = "", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = ModuleName + " - " + "NumUsers", FieldName = "NumUsers", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, CustomProperties = "", ShowWithCheckbox = false });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("TicketStatusMapping");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Created", ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Active", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Retired", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4 });
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            return mList;

        }

        public List<UGITModule> GetUGITModule()
        {
            List<UGITModule> mList = new List<UGITModule>();
            return mList;

        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "APP", Title = "Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "APP", Title = "Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "APP", Title = "Business Owner", UserTypes = "Business Owner", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "APP", Title = "Access Admin", UserTypes = "Admin", ColumnName = "AccessAdminUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "APP", Title = "Support", UserTypes = "Support", ColumnName = "SupportedByUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "Applications", TabOrder = 1, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "Applications", TabOrder = 2, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Active", ViewName = "Applications", TabOrder = 3, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "Applications", TabOrder = 4, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "Applications", TabOrder = 5, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Active", ViewName = "Applications", TabOrder = 6, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "Decommisioned", ViewName = "Applications", TabOrder = 7, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "Applications", TabOrder = 8, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Retired", ViewName = "Applications", TabOrder = 9, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "Applications", TabOrder = 10, ModuleNameLookup = "APP", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            return tabs;
        }
    }
}
