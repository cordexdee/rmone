using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernITDefault.Data.DefaultData
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
                    OwnerBinding = "Auto",
                    SyncAppsToRequestType = false,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = false
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
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "Author", FieldDisplayName = "Author", IsDisplay = true , FieldSequence=++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "WikiAverageScore", FieldDisplayName = "Average Score", IsDisplay = true , FieldSequence=++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "WIKI", FieldName = "WikiViews", FieldDisplayName = "# of Views", IsDisplay = true , FieldSequence=++seqNum });
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
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            Console.WriteLine("RequestType");
            mList.Add(new ModuleRequestType() { Title ="Application Support", RequestType = "Application Support", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full",IsDeleted =false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title ="Desktop Support", RequestType = "Desktop Support", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title ="IT Internal", RequestType = "IT Internal", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title ="Miscellaneous", RequestType = "Miscellaneous", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "General", WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
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
            Console.WriteLine("WikiLeftNavigation");
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
