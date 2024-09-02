using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class HLP: IModule
    {
        public UGITModule Module 
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Help",
                    CategoryName = "Help",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "HelpCard",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/HelpCards",
                    ModuleHoldMaxStage = 0,
                    Title = "Help (HLP)",
                    ModuleDescription = "This module is used to create and manage help cards.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/HLP",
                    ModuleType = ModuleType.Governance,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = false,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = false,
                    //EnableForceValidation = false
                    EnableNewsOnHomePage = false
                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "HLP";
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
        public List<ModuleSeverity> GetModuleSeverity()
        {

            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();

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
            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // WIK
            mList.Add(new ModuleColumn() { CategoryName = "HLP", FieldName = "TicketId", FieldDisplayName = "Help Card ID", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "HLP", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "HLP", FieldName = "Category", FieldDisplayName = "Category", IsDisplay = true, FieldSequence = ++seqNum });

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

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("RequestType");
            mList.Add(new ModuleRequestType() { Title = "Application Support", RequestType = "Application Support", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });

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

        

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();

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
