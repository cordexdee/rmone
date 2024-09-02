using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class RMM : IModule
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
        protected string moduleId = "8";

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "People Management",
                    CategoryName = "Resource Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "ResourceAllocation",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/rmmtickets",
                    ModuleHoldMaxStage = 1,
                    Title = "People Management (RMM)",
                    ModuleDescription = "This module is used to manage resources and resource allocation, as well as to collect actual hours spent on various activities.",                
                    ThemeColor = "Accent5",
                    StaticModulePagePath = "/Pages/rmm",
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
                return "RMM";
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
            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
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

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("RequestType");
            /*
            mList.Add(new ModuleRequestType() { Title="Data Center Monitoring", ModuleNameLookup = ModuleName, RequestCategory = "Lights-On Support", Category="Infrastructure",IsDeleted = false, Owner = ConfigData.Variables.Name});
            mList.Add(new ModuleRequestType() { Title = "Backups", ModuleNameLookup = ModuleName, RequestCategory = "Lights-On Support", Category = "Infrastructure", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "User Training", ModuleNameLookup = ModuleName, RequestCategory = "Lights-On Support", Category = "Training", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Staff Meeting", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Admin", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Performance Planning & Eval", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Admin", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Miscellaneous", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Admin", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Seminar", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Training", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Conference", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Training", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Online Class", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Training", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Holiday", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Time Off", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Vacation", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Time Off", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Jury Duty", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Time Off", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Bereavement", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Time Off", IsDeleted = false, Owner = ConfigData.Variables.Name });
            */

            /*
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Data Center Monitoring", Title = "Data Center Monitoring", Category = "Infrastructure", RequestCategory = "Lights-On Support", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Backups", Title = "Backups", Category = "Infrastructure", RequestCategory = "Lights-On Support", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "User Training", Title = "User Training", Category = "Training", RequestCategory = "Lights-On Support", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Staff Meeting", Title = "Staff Meeting", Category = "Admin", RequestCategory = "Other", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Performance Planning & Eval", Title = "Performance Planning & Eval", Category = "Admin", RequestCategory = "Other", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Miscellaneous", Title = "Miscellaneous", Category = "Admin", RequestCategory = "Other", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Seminar", Title = "Seminar", Category = "Training", RequestCategory = "Other", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Conference", Title = "Conference", Category = "Training", RequestCategory = "Other", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Online Class", Title = "Online Class", Category = "Training", RequestCategory = "Other", WorkflowType = "", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Sick Time", Title = "Sick Time", Category = "PTO", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Vacation/Personal Time", Title = "Vacation/Personal Time", Category = "PTO", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Time Used", Title = "Time Used", Category = "Unpaid Time Off", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Time Used", Title = "Time Used", Category = "Jury Duty", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Time Used", Title = "Time Used", Category = "Bereavement Leave", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Sick Time", Title = "Sick Time", Category = "PTO - Regular PT", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Vacation/Personal Time", Title = "Vacation/Personal Time", Category = "PTO - Regular PT", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Time Used", Title = "Time Used", Category = "Vacation - Union", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Time Used", Title = "Time Used", Category = "Vacation - Union 15 Days", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Time Used", Title = "Time Used", Category = "Military Leave", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            mList.Add(new ModuleRequestType { ModuleNameLookup = ModuleName, RequestType = "Time Used", Title = "Time Used", Category = "Executive PTO", RequestCategory = "Time Off", WorkflowType = "Full", Owner = null, IsDeleted = false });
            */

            mList.Add(new ModuleRequestType() { Title = "Backups", Category = "Infrastructure", RequestType = "Backups", ModuleNameLookup = ModuleName, RequestCategory = "Lights-On Support", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Bereavement", Category = "Time Off", RequestType = "Bereavement", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Conference", Category = "Training", RequestType = "Conference", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Data Center Monitoring", Category = "Infrastructure", RequestType = "Data Center Monitoring", ModuleNameLookup = ModuleName, RequestCategory = "Lights-On Support", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Holiday", Category = "Time Off", RequestType = "Holiday", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Jury Duty", Category = "Time Off", RequestType = "Jury Duty", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Miscellaneous", Category = "Admin", RequestType = "Miscellaneous", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Online Class", Category = "Training", RequestType = "Online Class", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Performance Planning & Eval", Category = "Admin", RequestType = "Performance Planning & Eval", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Planning Time Off", Category = "Time Off", RequestType = "Planning Time Off", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Seminar", Category = "Training", RequestType = "Seminar", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Staff Meeting", Category = "Admin", RequestType = "Staff Meeting", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "User Training", Category = "Training", RequestType = "User Training", ModuleNameLookup = ModuleName, RequestCategory = "Lights-On Support", WorkflowType = "Full", Owner = "Admin" });
            mList.Add(new ModuleRequestType() { Title = "Vacation", Category = "Time Off", RequestType = "Vacation", ModuleNameLookup = ModuleName, RequestCategory = "Other", WorkflowType = "Full", Owner = "Admin" });
            
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
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
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "RMM", TabOrder = 1, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "RMM", TabOrder = 2, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "RMM", TabOrder = 3, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true , TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "RMM", TabOrder = 4, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "RMM", TabOrder = 5, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "RMM", TabOrder = 6, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "RMM", TabOrder = 7, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "RMM", TabOrder = 8, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "RMM", TabOrder = 9, ModuleNameLookup = "RMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
