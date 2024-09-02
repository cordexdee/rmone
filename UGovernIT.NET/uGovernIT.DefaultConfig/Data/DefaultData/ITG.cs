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
    public class ITG : IModule
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

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Governance",
                    CategoryName = "Resource Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "ITGovernance",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/itgtickets",
                    ModuleHoldMaxStage = 1,
                    Title = "Governance (ITG)",
                    ModuleDescription = "This module is used to manage and track IT Budgets, as well as to monitor and approve IT projects.",
                    ThemeColor = "Accent5",
                    StaticModulePagePath = "/Pages/itg",
                    ModuleType = ModuleType.Governance,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = false,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = false,
                    UseInGlobalSearch = true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "ITG";
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
            mList.Add(new ModuleFormTab() { TabName = "Project Portfolio", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Project Portfolio" });
            mList.Add(new ModuleFormTab() { TabName = "Budget Categories", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Budget Categories" });
            mList.Add(new ModuleFormTab() { TabName = "Non-Project Budget", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Non-Project Budget" });
            mList.Add(new ModuleFormTab() { TabName = "Pending Review", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Pending Review" });

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

            List<string[]> dataList = new List<string[]>();

            // start from id - 51
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);

            mList.Add(new LifeCycleStage()
            {
                Name = "Project Status",
                StageTitle = "Project Status",
                UserWorkflowStatus = "Project Status",
                UserPrompt = "<b>Project Status</b>",
                StageStep = 1,
                ModuleNameLookup = ModuleName,
                Action = "Account Closed",
                ActionUser = "ITG;#PMO;#IT Governance;#IT Steering Committee",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 0,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            
            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
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
            mList.Add(new ModuleFormLayout() { Title = "Project Summary",FieldDisplayName = "Project Summary", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = 1,FieldDisplayWidth =3, FieldName = "#GroupStart#", ShowInMobile =true, CustomProperties = "Project Summary" });
            mList.Add(new ModuleFormLayout() { Title = "ITGPortfolio", FieldDisplayName = "ITGPortfolio", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = 2,FieldDisplayWidth =3, FieldName ="#Control#", ShowInMobile =true, CustomProperties = "IsReadOnly=true;#IsInFrame=true" });
            mList.Add(new ModuleFormLayout() { Title = "Project Summary", FieldDisplayName = "Project Summary", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = 3,FieldDisplayWidth =3, FieldName = "#GroupEnd#", ShowInMobile =true, CustomProperties = "Project Summary" });
            mList.Add(new ModuleFormLayout() { Title = "ITGBudgetManagement", FieldDisplayName = "ITGBudgetManagement", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = 2,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile =true, CustomProperties = "IsReadOnly=true;#IsInFrame=true" });
            mList.Add(new ModuleFormLayout() { Title = "ITGBudgetEditor", FieldDisplayName = "ITGBudgetEditor", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = 2,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile =true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Pending Review", FieldDisplayName = "Pending Review", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = 2,FieldDisplayWidth =3, FieldName = "#GroupStart#", ShowInMobile =true, CustomProperties = "Pending Review" });
            mList.Add(new ModuleFormLayout() { Title = "GovernanceReview", FieldDisplayName = "GovernanceReview", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = 2,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile =true, CustomProperties = "IsReadOnly=true;#IsInFrame=true" });
            mList.Add(new ModuleFormLayout() { Title = "Pending Review", FieldDisplayName = "Pending Review", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = 3,FieldDisplayWidth =3, FieldName = "#GroupEnd#", ShowInMobile =true, CustomProperties = "Pending Review" });
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
            // Stage 1
            mList.Add(new ModuleRoleWriteAccess(){ Title = ModuleName+"-"+"Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = 1, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton =false });
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
            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "ITG", TabOrder = 1, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "ITG", TabOrder = 2, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "ITG", TabOrder = 3, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "ITG", TabOrder = 4, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "ITG", TabOrder = 5, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Open", ViewName = "ITG", TabOrder = 6, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "ITG", TabOrder = 7, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "ITG", TabOrder = 8, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Closed", ViewName = "ITG", TabOrder = 9, ModuleNameLookup = "ITG", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
