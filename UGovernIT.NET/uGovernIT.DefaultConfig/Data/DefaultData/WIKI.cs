using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class WIKI : IWIKIModule
    {
        public static string userId = "1";
        public static string managerUserId = "1";
        public static string userName = ""; // Obtained from userId in UGovernITDefault.Initialize()

        public static string[] HideInTemplate = { "Attachments", "AssetLookup", "TicketComment", "TicketCreationDate", "TicketInitiator", "TicketRequestTypeCategory", "TicketStatus" };


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
                    ShortName = "Knowledge Management",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "WikiArticles",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/WIKITickets",
                    ModuleHoldMaxStage = 0,
                    Title = "Knowledge Management (WIKI)",
                    ModuleDescription = "This module is used to create and manage wiki articles.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/WIKI",
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
                return "WIKI";
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
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "TicketId", FieldDisplayName = "Wiki ID", IsDisplay = true ,IsUseInWildCard=true, FieldSequence=++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true ,IsUseInWildCard=true,SortOrder=1,IsAscending=true, FieldSequence=++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "Author", FieldDisplayName = "Author", IsDisplay = true , IsUseInWildCard = true, FieldSequence =++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "WikiAverageScore", FieldDisplayName = "Average Score", IsDisplay = true , IsUseInWildCard = true, FieldSequence =++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "WikiViews", FieldDisplayName = "# of Views", IsDisplay = true , IsUseInWildCard = true, FieldSequence =++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "RequestTypeLookup", FieldDisplayName = "RequestType", IsDisplay = true ,IsUseInWildCard=true, FieldSequence=++seqNum });
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
            /*
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("RequestType");
            mList.Add(new ModuleRequestType() { Title ="Application Support", RequestType = "Application Support", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full",IsDeleted =false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title ="Desktop Support", RequestType = "Desktop Support", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title ="IT Internal", RequestType = "IT Internal", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title ="Miscellaneous", RequestType = "Miscellaneous", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });

            //Request type for Service Management category
            mList.Add(new ModuleRequestType() { Title = "Application Change Request", RequestType = "Application Change Request", ModuleNameLookup = ModuleName, RequestCategory = "Other", Category = "Service Management", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Bug Tracking", RequestType = "Bug Tracking", ModuleNameLookup = ModuleName, RequestCategory="Other", Category = "Service Management", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Change Management", RequestType = "Change Management", ModuleNameLookup = ModuleName, RequestCategory="Other", Category = "Service Management", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Root Cause Analysis", RequestType = "Root Cause Analysis", ModuleNameLookup = ModuleName, RequestCategory="Other", Category = "Service Management", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Shared Services", RequestType = "Shared Services", ModuleNameLookup = ModuleName, RequestCategory="Other", Category = "Service Management", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Technical Service Request", RequestType = "Technical Service Request", ModuleNameLookup = ModuleName, RequestCategory="Other", Category = "Service Management", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Outage Incidents", RequestType = "Outage Incidents", ModuleNameLookup = ModuleName, RequestCategory="Other", Category = "Service Management", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            */

            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            mList.Add(new ModuleRequestType() { Title = "Analytics", Category = "Governance", SubCategory = "Overall IT Management", RequestType = "Analytics", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Application Support", Category = "General", SubCategory = "", RequestType = "Application Support", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Desktop Support", Category = "General", SubCategory = "", RequestType = "Desktop Support", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "IT Internal", Category = "General", SubCategory = "", RequestType = "IT Internal", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "IT Steering Committee", Category = "Governance", SubCategory = "Overall IT Management", RequestType = "IT Steering Committee", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Miscellaneous", Category = "General", SubCategory = "", RequestType = "Miscellaneous", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "None", Category = "Governance", SubCategory = "PMO", RequestType = "None", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Portfolio Improvement", Category = "Governance", SubCategory = "PMO", RequestType = "Portfolio Improvement", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Portfolio Management", Category = "Governance", SubCategory = "PMO", RequestType = "Portfolio Management", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Project Analysis Process", Category = "Governance", SubCategory = "PMO", RequestType = "Project Analysis Process", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Project Management", Category = "Governance", SubCategory = "PMO", RequestType = "Project Management", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Project Request Process", Category = "Governance", SubCategory = "PMO", RequestType = "Project Request Process", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Root Cause Analysis", Category = "Service Management", SubCategory = "", RequestType = "Root Cause Analysis", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Service Catalog", Category = "Governance", SubCategory = "Service Management", RequestType = "Service Catalog", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Service Design", Category = "Governance", SubCategory = "Service Management", RequestType = "Service Design", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Service Improvement", Category = "Governance", SubCategory = "Service Management", RequestType = "Service Improvement", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Service Operation", Category = "Governance", SubCategory = "Service Management", RequestType = "Service Operation", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Service Strategy", Category = "Governance", SubCategory = "Service Management", RequestType = "Service Strategy", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Service Transition", Category = "Governance", SubCategory = "Service Management", RequestType = "Service Transition", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Shared Services", Category = "Service Management", SubCategory = "", RequestType = "Shared Services", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            mList.Add(new ModuleRequestType() { Title = "Suppliers", Category = "Governance", SubCategory = "Overall IT Management", RequestType = "Suppliers", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "Admin", });
            
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

        

        public List<WikiLeftNavigation> GetWikiLeftNavigation()
        {
            List<WikiLeftNavigation> mList = new List<WikiLeftNavigation>();
            // Console.WriteLine("WikiLeftNavigation");
            mList.Add(new WikiLeftNavigation() { Title="All Wikis", ColumnType="AllWiki", ImageUrl = "/Content/Images/IdeaGroup.png", ItemOrder = 1, ConditionalLogic="" });
            mList.Add(new WikiLeftNavigation() { Title="My Wikis", ColumnType = "MyWiki", ImageUrl = "/Content/Images/MyWikis.png", ItemOrder = 4, ConditionalLogic = "" });
            mList.Add(new WikiLeftNavigation() { Title ="Favorite Wikis", ColumnType = "FavoriteWiki", ImageUrl = "/Content/Images/FavoriteStar.png", ItemOrder = 2, ConditionalLogic = "" });
            mList.Add(new WikiLeftNavigation() { Title ="Popular Wikis", ColumnType = "PopularWiki", ImageUrl = "/Content/Images/PopularWikis.png", ItemOrder = 3, ConditionalLogic = "" });
            mList.Add(new WikiLeftNavigation() { Title ="Archived Wikis", ColumnType = "ArchiveWiki", ImageUrl = "/Content/Images/IdeaGroup.png", ItemOrder = 5, ConditionalLogic = "" });
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
