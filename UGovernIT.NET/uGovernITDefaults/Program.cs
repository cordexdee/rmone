using System;
using UGovernITDefault;

namespace uGovernITDefault
{
    class Program
    {
        public static void Main(string[] args)
        {
            ShowHelp();
            args = AskParameters(args);
            if (args.Length > 0 && args[0] == "script")
            {
                ScriptProgram.Execute(new string[0]);

                Main(new string[0]);
            }
            else if (args.Length >= 1 && args[0] == "data")
            {
                DataProgram.Execute(new string[0]);
                Main(new string[0]);
            }
            else if (args.Length >= 1 && args[0] == "migration")
            {
                MigrationProgram.Execute(new string[0]);
                Main(new string[0]);
            }
            else
            {
                Main(new string[0]);
            }
        }

        public static string[] AskParameters(string[] args)
        {
            Console.Write("> ");

            string arguments = Console.ReadLine();
            string[] argAry = arguments.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < argAry.Length; i++)
            {
                argAry[i] = argAry[i].Trim();
            }

            if (argAry.Length <= 0)
            {
                argAry = AskParameters(args);
            }
            else if (argAry[0].ToLower() == "?")
            {
                ShowHelp();
                argAry = AskParameters(args);
            }
            else if (argAry[0].ToLower() == "exit")
            {
                Environment.Exit(0);
            }

            return argAry;
        }

        public static void ShowHelp()
        {
            Console.WriteLine("Please Enter:");
            Console.WriteLine("  data - to fill configuration");
            Console.WriteLine("  script - to run a script");
            Console.WriteLine("  migration - to run migration");
            Console.WriteLine("  ?  - for help");
            Console.WriteLine("  exit - to exit console window");
            Console.WriteLine("");
        }
    }

    #region "notusedcode"
    //public class uGovernITFillDefault
    //{
    //    public static string userId = UserCreate.userID;
    //    public static string managerUserId = "1";
    //    public static string userName = UserCreate.userName; // Obtained from userId in UGovernITDefault.Initialize()

    //    public static string[] HideInTemplate = { "Attachments", "AssetLookup", "TicketComment", "TicketCreationDate", "TicketInitiator", "TicketRequestTypeCategory", "TicketStatus" };

    //    ///// Default URL used when none passed in
    //    ////public static string url = "http://demo.ugovernit.com/";  // Demo site

    //    //public static string url = "http://winserver/";

    //    //public static string adminGroupName = "uGovernIT Admins";
    //    //public static string membersGroupName = "uGovernIT Members";
    //    //public static int totalNumberOfTickets = 0;
    //    //public static string guideMeUrl = "/SitePages/GuideMe.aspx";

    //    public static int currStageID = 0;
    //    public static bool loadRequestTypes = true;
    //    public static bool loadModuleStages = true;
    //    public static bool loadLocations = true;
    //    public static bool loadDepartments = true;

    //    public static bool loadBudgetCategories = true;
    //    public static bool loadVNDModules = true;

    //    public static bool userFieldsEditable = true;


    //    public static Hashtable moduleStartStages = new Hashtable();
    //    public static Hashtable moduleAssignedStages = new Hashtable();
    //    public static Hashtable moduleResolvedStages = new Hashtable();
    //    public static Hashtable moduleTestedStages = new Hashtable();
    //    public static Hashtable moduleClosedStages = new Hashtable();
    //    public static int nprMgrApprovalStageID;
    //    public static int nprPMOStageID;
    //    public static int nprITGovernanceStageID;
    //    public static int nprITSteeringeStageID;
    //    public static int nprApprovedStageID;
    //    public static int pmmStartStageID;
    //    public static int pmmClosedStageID;
    //    public static IStore dbStore = new uGovernITData();

    //    public static void Execute()
    //    {

    //        Console.WriteLine("                      uGovernIT - Default Entries");
    //        //Console.WriteLine("Loading into site: {0}\n", url);
            

    //        UGovernITDefault defaults = new UGovernITDefault();
    //        defaults.Initialize();

    //        PRS prsDefaults = new PRS();
    //        prsDefaults.Initialize();

    //        TSR tsrDefaults = new TSR();
    //       tsrDefaults.Initialize();

    //        ACR acrDefaults = new ACR();
    //        acrDefaults.Initialize();

    //        DRQ drqDefaults = new DRQ();
    //        drqDefaults.Initialize();

    //        BTS uBugDefaults = new BTS();
    //        uBugDefaults.Initialize();

    //        NPR nprDefaults = new NPR();
    //        nprDefaults.Initialize();

    //        PMM pmmDefaults = new PMM();
    //        pmmDefaults.Initialize();

    //        RMM rmmDefaults = new RMM();
    //        rmmDefaults.Initialize();

    //        Asset assetDefaults = new Asset();
    //        assetDefaults.Initialize();

    //        ITG itgDefaults = new ITG();
    //        itgDefaults.Initialize();

    //        //Asset assetDefaults = new Asset();
    //        //assetDefaults.Initialize();

    //        INC incDefaults = new INC();
    //        incDefaults.Initialize();

    //        TSK tskList = new TSK();
    //        tskList.Initialize();

    //        SVC svcList = new SVC();
    //        svcList.Initialize();

    //        CMT cmtList = new CMT();
    //        cmtList.Initialize();

    //        APP appDefaults = new APP();
    //        appDefaults.Initialize();

    //        RCA rcaDefaults = new RCA();
    //        rcaDefaults.Initialize();

    //        VND vndList = new VND();
    //        vndList.Initialize();

    //        VPM vpmList = new VPM();
    //        vpmList.Initialize();

    //        VFM vfmList = new VFM();
    //        vfmList.Initialize();

    //        VSW vswList = new VSW();
    //        vswList.Initialize();

    //        VSL vslList = new VSL();
    //        vslList.Initialize();

    //        VCC vccList = new VCC();
    //        vccList.Initialize();

    //        CRM crmList = new CRM();
    //        crmList.Initialize();

    //        OPM opmList = new OPM();
    //        opmList.Initialize();

    //        //Dashboard dashboardList = new Dashboard();
    //        //dashboardList.Initialize();


    //        Console.WriteLine("uGovernIT defaults entered successfully.\n");
    //        Console.ReadLine();
    //    }
    //    class UGovernITDefault
    //    {
    //        protected static IDefault iDefaultModule = dbStore.LoaduGovernITDefault();
    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: Project level default entries at " + DateTime.Now.ToString());
                
    //            insertItemsToModule();// Done tested
    //            addItemToMenuNavigation();//9 Done tested
    //            addItemsToGenericStatus();
    //            addItemsToTicketPriority();//1 Done tested
    //            addItemsToTicketImpact();//2 Done tested
    //            addItemsToTicketSeverity();//3 Done tested
    //            addItemsToModuleColumns();//4 Done tested
    //            addItemsToRequestPriority();//5 Done tested
    //            addItemToStageType();//6 Done tested
    //            addItemToConfigurationVariable();//7 Done tested
    //            addItemToModuleUserTypes();//8 Done tested
    //            addMonitorsByModule();//14 Done tested
    //            addMonitorOptions();//15 Done tested
    //            if (loadLocations)
    //                addItemToLocation();//16  Done tested
    //            addItemToCompany();//17   Done tested
    //            if (loadDepartments)
    //            {
    //                addItemToDepartment();//18 Done tested
    //                AddFunctionalAreas();//19 Done tested
    //            }
    //            addItemToMailTokenColumnName();//13 Done tested
    //            if (loadBudgetCategories)
    //                addBudgetCategories();//20  Done tested
    //            addMyModuleColumns();//10 Done tested
    //            addClientAdminCategory(); //Done tested
    //            addClientConfigurationLists();//Done tested
    //            addItemsToMessageBoard();//12  Done tested
    //            addItemsToFieldConfiguration();//Done tested
    //            addItemstToTabView();

    //            Console.WriteLine("Finished: Project level default entries at " + DateTime.Now.ToString());
    //        }

    //        public void insertItemsToModule()
    //        {
    //            List<UGITModule> modules = iDefaultModule.GetUGITModule();

    //            foreach (UGITModule module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "AutoApprove" && x != "HoldMaxStage" && x != "ListPageUrl" && x != "DetailPageUrl" && x != "DisableNewTicketConfirmation" && x != "StoreTicketEmails" && x != "ShowMyDeptTickets" && x != "EnableQuickTicket" && x != "AllowChangeTicketType" && x != "ShowTicketSummary" && x != "IconUrl" && x != "KeepTicketOpen" && x != "List_LifeCycles" && x != "List_FormLayout" && x != "List_RoleWriteAccess" && x != "List_FormTab" && x != "List_DefaultValues" && x != "List_TaskEmail" && x != "List_RequestTypes" && x != "List_PriorityMaps" && x != "List_ModuleColumns" && x != "List_ModuleUserTypes" && x != "List_Priorities" && x != "List_Impacts" && x != "List_Severities" && x != "List_RequestTypeByLocation" && x != "TicketListViewFields" && x != "TabNumber"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@AutoApprove" && x != "@HoldMaxStage" && x != "@ListPageUrl" && x != "@DetailPageUrl" && x != "@DisableNewTicketConfirmation" && x != "@StoreTicketEmails" && x != "@ShowMyDeptTickets" && x != "@EnableQuickTicket" && x != "@AllowChangeTicketType" && x != "@ShowTicketSummary" && x != "@IconUrl" && x != "@KeepTicketOpen" && x != "@List_LifeCycles" && x != "@List_FormLayout" && x != "@List_RoleWriteAccess" && x != "@List_FormTab" && x != "@List_DefaultValues" && x != "@List_TaskEmail" && x != "@List_RequestTypes" && x != "@List_PriorityMaps" && x != "@List_ModuleColumns" && x != "@List_ModuleUserTypes" && x != "@List_Priorities" && x != "@List_Impacts" && x != "@List_Severities" && x != "@List_RequestTypeByLocation" && x != "@TicketListViewFields" && x != "@TabNumber"));
    //                int? result = uGITDAL.insertData<UGITModule>(module, "Config_Modules", columns, values);
    //            }

    //        }
    //        public void addItemToModuleUserTypes()
    //        {
    //            List<ModuleUserType> modules = iDefaultModule.GetModuleUserType();
    //            foreach (ModuleUserType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "FieldName" && x != "Prop_ManagerOf" && x != "Prop_DisableEmailTicketLink" && x != "DefaultUser"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@FieldName" && x != "@Prop_ManagerOf" && x != "@Prop_DisableEmailTicketLink" && x != "@DefaultUser"));
    //                int? result = uGITDAL.insertData<ModuleUserType>(module, "Config_Module_ModuleUserTypes", columns, values);
    //            }

    //        }
    //        public void addItemToMenuNavigation()
    //        {
    //            List<MenuNavigation> modules = iDefaultModule.GetMenuNavigation();
    //            foreach (MenuNavigation module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "Parent" && x != "CustomizeFormat" && x != "IconUrl" && x != "ImageUrl" && x != "AttachmentUrl" && x != "Children" && x != "ChildCount" && x != "HasChildren" && x != "Item" && x != "Path" && x != "Type" && x != "MenuName" && x != "MenuHeight" && x != "MenuWidth" && x != "MenuBackground" && x != "SubMenuStyle" && x != "MenuFontColor" && x != "MenuItemSeparation" && x != "MenuTextAlignment" && x != " CustomizeFormat" && x != "SubMenuItemPerRow" && x != "SubMenuItemAlignment" && x != "MenuFontSize" && x != "MenuFontFontFamily" && x != "MenuIconAlignment"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@Parent" && x != "@CustomizeFormat" && x != "@IconUrl" && x != "@ImageUrl" && x != "@AttachmentUrl" && x != "@Children" && x != "@ChildCount" && x != "@HasChildren" && x != "@Item" && x != "@Path" && x != "@Type" && x != "@MenuName" && x != "@MenuHeight" && x != "@MenuWidth" && x != "@MenuBackground" && x != "@SubMenuStyle" && x != "@MenuFontColor" && x != "@MenuItemSeparation" && x != "@MenuTextAlignment" && x != "@ CustomizeFormat" && x != "@SubMenuItemPerRow" && x != "@SubMenuItemAlignment" && x != "@MenuFontSize" && x != "@MenuFontFontFamily" && x != "@MenuIconAlignment"));
    //                int? result = uGITDAL.insertData<MenuNavigation>(module, "Config_MenuNavigation", columns, values);
    //            }

    //        }
    //        public void addItemToConfigurationVariable()
    //        {
    //            List<ConfigurationVariable> modules = iDefaultModule.GetConfigurationVariable();
    //            foreach (ConfigurationVariable module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ConfigurationVariable>(module, "Config_ConfigurationVariable", columns, values);
    //            }

    //        }
    //        public void addItemToStageType()
    //        {
    //            List<Module_StageType> modules = iDefaultModule.GetModuleStageType();
    //            foreach (Module_StageType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<Module_StageType>(module, "Config_Module_StageType", columns, values);
    //            }


    //        }
    //        public void addItemsToRequestPriority()
    //        {
    //            List<ModulePriorityMap> modules = iDefaultModule.GetModulePriorityMap();
    //            foreach (ModulePriorityMap module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModulePriorityMap>(module, "Config_Module_RequestPriority", columns, values);
    //            }

    //        }
    //        public void addItemsToModuleColumns()
    //        {
    //            List<ModuleColumns> modules = iDefaultModule.GetModuleColumns();
    //            foreach (ModuleColumns module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "Prop_UseforGlobalDateFilter" && x != "Prop_MiniView" && x != "AssetLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@Prop_UseforGlobalDateFilter" && x != "@Prop_MiniView" && x != "@AssetLookup"));
    //                int? result = uGITDAL.insertData<ModuleColumns>(module, "Config_Module_ModuleColumns", columns, values);
    //            }

    //        }
    //        public void addItemsToTicketPriority()
    //        {
    //            List<ModulePrioirty> modules = iDefaultModule.GetModulePrioirty();
    //            foreach (ModulePrioirty module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "IsDeleted" && x != "IsVIP" && x != "NotifyTo" && x != "Name"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@IsDeleted" && x != "@IsVIP" && x != "@NotifyTo" && x != "@Name"));
    //                int? result = uGITDAL.insertData<ModulePrioirty>(module, "Config_Module_Priority", columns, values);
    //            }
    //        }
    //        public void addItemsToTicketImpact()
    //        {
    //            List<ModuleImpact> modules = iDefaultModule.GetModuleImpact();
    //            foreach (ModuleImpact module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "Name"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@Name"));
    //                int? result = uGITDAL.insertData<ModuleImpact>(module, "Config_Module_Impact", columns, values);
    //            }

    //        }
    //        public void addItemsToTicketSeverity()
    //        {
    //            List<ModuleSeverity> modules = iDefaultModule.GetModuleSeverity();
    //            foreach (ModuleSeverity module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "Name"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@Name"));
    //                int? result = uGITDAL.insertData<ModuleSeverity>(module, "Config_Module_Severity", columns, values);
    //            }

    //        }

    //        public void addItemsToGenericStatus()
    //        {
    //            List<GenericTicketStatus> modules = iDefaultModule.GetGenericTicketStatus();
    //            foreach (GenericTicketStatus module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<GenericTicketStatus>(module, "GenericStatus", columns, values);
    //            }

    //        }

    //        public void addItemToLocation()
    //        {
    //            List<Location> modules = iDefaultModule.GetLocation();
    //            foreach (Location module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "Country" && x != "Region" && x != "State" && x != "IsDeleted"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@Country" && x != "@Region" && x != "@State" && x != "@IsDeleted"));
    //                int? result = uGITDAL.insertData<Location>(module, "Location", columns, values);
    //            }

    //        }
    //        public void addBudgetCategories()
    //        {
    //            List<BudgetCategories> modules = iDefaultModule.GetBudgetCategories();
    //            foreach (BudgetCategories module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "AuthorizedToEdit" && x != "AuthorizedToView" && x != "BudgetType" && x != "BudgetTypeCOA" && x != "CapitalExpenditure" && x != "IsDeleted"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@AuthorizedToEdit" && x != "@AuthorizedToView" && x != "@BudgetType" && x != "@BudgetTypeCOA" && x != "@CapitalExpenditure" && x != "@IsDeleted"));
    //                int? result = uGITDAL.insertData<BudgetCategories>(module, "Config_BudgetCategories", columns, values);
    //            }
    //        }

    //        public void addMonitorsByModule()
    //        {

    //            List<ModuleMonitors> modules = iDefaultModule.GetModuleMonitors();
    //            foreach (ModuleMonitors module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "ShortName"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@ShortName"));
    //                int? result = uGITDAL.insertData<ModuleMonitors>(module, "Config_ModuleMonitors", columns, values);
    //            }
    //        }
    //        public void addMonitorOptions()
    //        {
    //            List<ModuleMonitorOptions> modules = iDefaultModule.GetModuleMonitorOptions();
    //            foreach (ModuleMonitorOptions module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleMonitorOptions>(module, "Config_ModuleMonitorOptions", columns, values);
    //            }

    //        }

    //        public void addItemToDepartment()
    //        {
    //            List<Department> modules = iDefaultModule.GetDepartment();
    //            foreach (Department module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "CompanyLookup" && x != "DivisionLookup" && x != "DivisionIdLookup" && x != "IsDeleted"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@CompanyLookup" && x != "@DivisionLookup" && x != "@DivisionIdLookup" && x != "@IsDeleted"));
    //                int? result = uGITDAL.insertData<Department>(module, "Department", columns, values);
    //            }
    //        }
    //        public void AddFunctionalAreas()
    //        {
    //            List<FunctionalAreas> modules = iDefaultModule.GetFunctionalAreas();
    //            foreach (FunctionalAreas module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<FunctionalAreas>(module, "FunctionalAreas", columns, values);
    //            }
    //        }
    //        public void addItemToCompany()
    //        {
    //            List<Company> modules = iDefaultModule.GetCompany();
    //            foreach (Company module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "Name" && x != "Departments" && x != "CompanyDivisions"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@Name" && x != "@Departments" && x != "@CompanyDivisions"));
    //                int? result = uGITDAL.insertData<Company>(module, "Company", columns, values);
    //            }
    //        }
    //        public void addItemToMailTokenColumnName()
    //        {
    //            List<MailTokenColumnName> modules = iDefaultModule.GetMailTokenColumnName();
    //            foreach (MailTokenColumnName module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<MailTokenColumnName>(module, "Config_MailTokenColumnName", columns, values);
    //            }
    //        }
    //        public void addMyModuleColumns()
    //        {
    //            List<MyModuleColumns> modules = iDefaultModule.GetMyModuleColumns();
    //            foreach (MyModuleColumns module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TextAlignment" && x != "CustomProperties" && x != "DisplayForReport"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TextAlignment" && x != "@CustomProperties" && x != "@DisplayForReport"));
    //                int? result = uGITDAL.insertData<MyModuleColumns>(module, "Config_MyModuleColumns", columns, values);
    //            }
    //        }
    //        public void addClientAdminCategory()
    //        {
    //            List<ClientAdminCategory> mClientAdminCategory = iDefaultModule.GetClientAdminCategory();
    //            foreach (ClientAdminCategory module in mClientAdminCategory)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ClientAdminCategory>(module, "Config_ClientAdminCategory", columns, values);
    //            }
    //        }
    //        public void addClientConfigurationLists()
    //        {
    //            List<ClientAdminConfigurationLists> modules = iDefaultModule.GetClientAdminConfigurationLists();
    //            foreach (ClientAdminConfigurationLists module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ClientAdminConfigurationLists>(module, "Config_ClientAdminConfigurationLists", columns, values);
    //            }

    //        }
    //        public void addItemsToMessageBoard()
    //        {
    //            List<MessageBoard> modules = iDefaultModule.GetMessageBoard();
    //            foreach (MessageBoard module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "NavigationUrl" && x != "TicketId"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@NavigationUrl" && x != "@TicketId"));
    //                int? result = uGITDAL.insertData<MessageBoard>(module, "MessageBoard", columns, values);
    //            }
    //        }
    //        public void addItemsToFieldConfiguration()
    //        {
    //            iDefaultModule.UpdateFieldConfigData();
    //        }

    //        public void addItemstToTabView()
    //        {
    //            iDefaultModule.UpdateTabView();
    //        }

    //    }
       
        


    //    class PRS
    //    {
    //        protected static IModule prsModule = dbStore.LoadPRSModule();

    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: PRS Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();
    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: PRS Ticket at " + DateTime.Now.ToString());
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = prsModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = prsModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);

    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            Console.WriteLine("  RequestRoleWriteAccess");

    //            List<ModuleRoleWriteAccess> modules = prsModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            Console.WriteLine("  TaskEmails");

    //            List<ModuleTaskEmail> modules = prsModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            Console.WriteLine("  ModuleFormTab");

    //            List<ModuleFormTab> tabs = prsModule.GetFormTabs();
    //            foreach (ModuleFormTab tab in tabs)
    //            {
    //                string columns = string.Join(",", tab.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", tab.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(tab, "Config_Module_ModuleFormTab", columns, values);
    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = prsModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);

    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = prsModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = prsModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }
    //    class TSR
    //    {
    //        protected static IModule trsModule = dbStore.LoadTSRModule();

    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: TSR Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();  //// Done tested
    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();//// Done tested

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages(); ////Done tested
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }

    //            addItemsToModuleFormTab(); ////Done Tested
    //            addItemsToFormLayout(); ////Done Tested
    //            addItemToModuleDefaultValues(); ////need to check
    //            Console.WriteLine("Finished: TSR Ticket at " + DateTime.Now.ToString());
    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = trsModule.GetModuleRequestType();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = trsModule.GetModuleRoleWriteAccess();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = trsModule.GetLifeCycleStage();
    //            uGovernIT.DAL.uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = trsModule.GetModuleTaskEmail();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = trsModule.GetFormTabs();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = trsModule.GetModuleFormLayout();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = trsModule.GetModuleDefaultValue();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = trsModule.GetModuleStatusMapping();
    //            uGITDAL.InsertItem(modules);
    //        }
    //    }
    //    class ACR
    //    {
    //        protected static IModule acrModule = dbStore.LoadACRModule();
    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: ACR Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();
    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemsToACRTypes(); //Done with Insert of data.
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: ACR Ticket at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToACRTypes()
    //        {
    //            List<ACRTypes> modules = acrModule.GetACRTypes();
    //            foreach (ACRTypes module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ACRTypes>(module, "ACRTypes", columns, values);

    //            }
    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = acrModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);

    //            }

    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = acrModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = acrModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = acrModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = acrModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = acrModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);

    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = acrModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);

    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = acrModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);

    //            }
    //        }
    //    }
    //    class DRQ
    //    {

    //        protected static IModule drqModule = dbStore.LoadDRQModule();
    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: DRQ Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            addItemsToRequestTypes();
    //            if (loadModuleStages)
    //            {
    //                addItemsToStages(); // Done
    //                addItemsToStatusMappingList(); // Done
    //                addItemsToTaskEmails(); // Done
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemsToDRQSystemAreas();
    //            addItemsToDRQRapidTypes();
    //            Console.WriteLine("Finished: DRQ Ticket at " + DateTime.Now.ToString());
    //        }
    //        public void addItemsToDRQSystemAreas()
    //        {
    //            List<DRQSystemAreas> modules = drqModule.GetDRQSystemAreas();
    //            foreach (DRQSystemAreas module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<DRQSystemAreas>(module, "DRQSystemAreas", columns, values);

    //            }

    //        }
    //        public void addItemsToDRQRapidTypes()
    //        {
    //            List<DRQRapidTypes> modules = drqModule.GetDRQRapidTypes();
    //            foreach (DRQRapidTypes module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<DRQRapidTypes>(module, "DRQRapidTypes", columns, values);

    //            }

    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = drqModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }

    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = drqModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = drqModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = drqModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);

    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = drqModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = drqModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);

    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = drqModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);


    //            }
    //        }
    //    }
    //    class BTS
    //    {
    //        protected static IModule btsModule = dbStore.LoadBTSModule();

    //        public void Initialize()
    //        {
    //            //Default values for Lists
    //            Console.WriteLine("Started: BTS Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();
    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: BTS Ticket at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = btsModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);

    //            }

    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = btsModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = btsModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = btsModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = btsModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);

    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = btsModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);

    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = btsModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = btsModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);

    //            }
    //        }
    //    }
    //    class NPR
    //    {
    //        protected static IModule nprModule = dbStore.LoadNPRModule();

    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: NPR Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }

    //            addItemsToRequestTypes();

    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            //CreateNPRTaskView();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: NPR Ticket at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = nprModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = nprModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = nprModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = nprModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = nprModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);

    //            }
    //        }
    //        //public void CreateNPRTaskView()
    //        //{
    //        //    Console.WriteLine("  NPRTask View");

    //        //    System.Collections.Specialized.StringCollection viewFields = new System.Collections.Specialized.StringCollection();

    //        //    viewFields.Add("Title");
    //        //    viewFields.Add("StartDate");
    //        //    viewFields.Add("DueDate");
    //        //    viewFields.Add("Predecessors");
    //        //    viewFields.Add("TicketNPRIdLookup");
    //        //    SPView view = .Lists["NPRTasks.Views.Add("TaskGanttChart", viewFields, string.Empty, 20, true, false, SPViewCollection.SPViewType.Gantt, false);
    //        //    view.ViewData = @"<FieldRef Name=""Title"" Type=""GanttTitle"" /><FieldRef Name=""StartDate"" Type=""GanttStartDate"" /><FieldRef Name=""DueDate"" Type=""GanttEndDate"" /><FieldRef Name=""PercentComplete"" Type=""GanttPercentComplete"" /><FieldRef Name=""Predecessors"" Type=""GanttPredecessors"" />";
    //        //    view.Update();
    //        //}
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = nprModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);

    //            }

    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = nprModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {

    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = nprModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {

    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);

    //            }
    //        }
    //    }


    //    class PMM
    //    {
    //        protected static IPMMModule pmmModule = dbStore.LoadPMMModule();
    //        public void Initialize()
    //        {
    //            //Default values for Lists
    //            Console.WriteLine("Started: PMM Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadModuleStages)
    //                addItemsToStages();
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            //CreatePMMTaskView();

    //            addItemsToRequestTypes();

    //            AddProjectClasses();
    //            AddProjectInitiative();
    //            AddProjectLifecycles();
    //            AddProjectLifecycleStages();
    //            AddEventCategories();
    //            Console.WriteLine("Finished: PMM Ticket at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = pmmModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = pmmModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = pmmModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);

    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = pmmModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);

    //            }
    //        }
    //        //public void CreatePMMTaskView()
    //        //{
    //        //    Console.WriteLine("  PMMTask View");

    //        //    System.Collections.Specialized.StringCollection viewFields = new System.Collections.Specialized.StringCollection();

    //        //    viewFields.Add("Title");
    //        //    viewFields.Add("StartDate");
    //        //    viewFields.Add("DueDate");
    //        //    viewFields.Add("AssignedTo");
    //        //    viewFields.Add("Status");
    //        //    viewFields.Add("Priority");
    //        //    viewFields.Add("PercentComplete");
    //        //    viewFields.Add("Predecessors");
    //        //    viewFields.Add("TicketPMMIdLookup");
    //        //    SPView view = .Lists["PMMTasks.Views.Add("TaskGanttChart", viewFields, string.Empty, 20, true, false, SPViewCollection.SPViewType.Gantt, false);
    //        //    view.ViewData = @"<FieldRef Name=""Title"" Type=""GanttTitle"" /><FieldRef Name=""StartDate"" Type=""GanttStartDate"" /><FieldRef Name=""DueDate"" Type=""GanttEndDate"" /><FieldRef Name=""PercentComplete"" Type=""GanttPercentComplete"" /><FieldRef Name=""Predecessors"" Type=""GanttPredecessors"" />";
    //        //    view.Update();
    //        //}
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = pmmModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }


    //        }
    //        public void AddProjectClasses()
    //        {
    //            List<ProjectClass> modules = pmmModule.GetProjectClass();
    //            foreach (ProjectClass module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ProjectClass>(module, "Config_ProjectClass", columns, values);
    //            }
    //        }
    //        public void AddProjectInitiative()
    //        {
    //            List<ProjectInitiative> modules = pmmModule.GetProjectInitiative();
    //            foreach (ProjectInitiative module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ProjectInitiative>(module, "Config_ProjectInitiative", columns, values);
    //            }
    //        }
    //        public void AddProjectLifecycles()
    //        {
    //            List<ModuleLifeCycles> modules = pmmModule.GetProjectLifeCycles();
    //            foreach (ModuleLifeCycles module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleLifeCycles>(module, "Config_ProjectLifeCycles", columns, values);
    //            }
    //        }
    //        public void AddProjectLifecycleStages()
    //        {
    //            List<ProjectLifecycleStages> modules = pmmModule.GetProjectLifecycleStages();
    //            foreach (ProjectLifecycleStages module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ProjectLifecycleStages>(module, "Config_ProjectLifeCycleStages", columns, values);
    //            }
    //        }
    //        public void AddEventCategories()
    //        {
    //            List<EventCategories> modules = pmmModule.GetEventCategories();
    //            foreach (EventCategories module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<EventCategories>(module, "Config_EventCategories", columns, values);
    //            }
    //        }
    //    }
    //    class RMM
    //    {
    //        protected static IModule rmmModule = dbStore.LoadRMMModule();
    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: RMM  Defaults at " + DateTime.Now.ToString());
    //            addItemsToRequestTypes();
    //            Console.WriteLine("Finished: RMM  Defaults at " + DateTime.Now.ToString());
    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = rmmModule.GetModuleRequestType();
    //            uGITDAL.InsertItem(modules);
    //        }
    //    }

    //    class ITG
    //    {
    //        protected static IModule itgModule = dbStore.LoadITGModule();

    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: ITG Ticket at " + DateTime.Now.ToString());
    //            if (loadModuleStages)
    //                addItemsToStages();
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            Console.WriteLine("Finished: ITG Ticket at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = itgModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = itgModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = itgModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = itgModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);

    //            }
    //        }
    //    }

    //    class Asset
    //    {
    //        public static IAssetModule assetModule = dbStore.LoadAssetModule();
    //        public void Initialize()
    //        {

    //            //Default values for Lists
    //            Console.WriteLine("Started: Asset defaults at.. " + DateTime.Now.ToString());
    //            //addItemsToStages();  //done
    //            //addItemsToModuleFormTab(); //done
    //            //addItemsToRequestRoleWriteAccess(); //done
    //            //addItemsToFormLayout();//done
    //            addAssetVendors();
    //            // addAssetModels();
    //            Console.WriteLine("Finished: Asset defaults at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = assetModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addAssetVendors()
    //        {
    //            List<AssetVendors> modules = assetModule.GetAssetVendors();
    //            foreach (AssetVendors module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "WebsiteUrl" && x != "IsDeleted"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@WebsiteUrl" && x != "@IsDeleted"));
    //                int? result = uGITDAL.insertData<AssetVendors>(module, "AssetVendors", columns, values);
    //            }

    //        }
    //        public void addAssetModels()
    //        {
    //            List<AssetModels> modules = assetModule.GetAssetModels();
    //            foreach (AssetModels module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "BudgetLookup" && x != "VendorLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@BudgetLookup" && x != "@VendorLookup"));
    //                int? result = uGITDAL.insertData<AssetModels>(module, "AssetModels", columns, values);
    //            }

    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = assetModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = assetModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = assetModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }

    //        }
    //    }
    //    class INC
    //    {
    //        protected static IModule incModule = dbStore.LoadINCModule();

    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: INC Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: INC Ticket at " + DateTime.Now.ToString());
    //        }

    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = incModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = incModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = incModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);

    //            }

    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = incModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = incModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = incModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            //List<ModuleDefaultValue> modules = incModule.GetModuleDefaultValue();
    //            //foreach (ModuleDefaultValue module in modules)
    //            //{
    //            //    string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //            //    string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //            //    int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            //}
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = incModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }

    //    class TSK
    //    {
    //        protected static IModule tskModule = dbStore.LoadTSKModule();
    //        public void Initialize()
    //        {
    //            //Default values for Lists
    //            Console.WriteLine("Started: TSK Projects at " + DateTime.Now.ToString());

    //            if (loadModuleStages)
    //                addItemsToStages();

    //            addItemsToRequestRoleWriteAccess();
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            Console.WriteLine("Finished: TSK Projects at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = tskModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = tskModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }

    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = tskModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = tskModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }

    //        }
    //    }
    //    class SVC
    //    {
    //        protected static IModule svcModule = dbStore.LoadSVCModule();
    //        public void Initialize()
    //        {

    //            //Default values for Lists
    //            Console.WriteLine("Started: SVC Projects at " + DateTime.Now.ToString());

    //            if (loadModuleStages)
    //                addItemsToStages();
    //            addItemsToRequestRoleWriteAccess();
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemsToTaskEmails();
    //            Console.WriteLine("Finished: SVC Projects at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = svcModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = svcModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);

    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = svcModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = svcModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = svcModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);

    //            }
    //        }
    //    }
    //    class CMT
    //    {
    //        protected static IModule cmtModule = dbStore.LoadCMTModule();
    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: CMT Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            addItemsToRequestTypes();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: CMT Request at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = cmtModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = cmtModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = cmtModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = cmtModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);

    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = cmtModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);

    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = cmtModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }

    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = cmtModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = cmtModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }

    //    class WIKI
    //    {

    //        protected static IWIKIModule wikiModule = dbStore.LoadWIKIModule();

    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: CMT Request at " + DateTime.Now.ToString());
    //            addItemsToRequestTypes();
    //            addItemsToWikiLeftNavigation();
    //            Console.WriteLine("Finished: CMT Request at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = wikiModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToWikiLeftNavigation()
    //        {
    //            List<WikiLeftNavigation> modules = wikiModule.GetWikiLeftNavigation();
    //            foreach (WikiLeftNavigation module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<WikiLeftNavigation>(module, "Config_WikiLeftNavigation", columns, values);
    //            }
    //        }
    //    }

    //    class APP
    //    {
    //        protected static IModule appModule = dbStore.LoadAPPModule();
    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: APP Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                //addItemsToStatusMappingList();
    //                //addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            //addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: APP Request at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = appModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }

    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = appModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = appModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = appModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);

    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = appModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = appModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }

    //    class INV
    //    {
    //        protected static IModule invModule = dbStore.LoadINVModule();
    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: INV Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();
    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            //addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: INV Request at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = invModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = invModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);

    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = invModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = invModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);

    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = invModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);

    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = invModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        //public void addItemToModuleDefaultValues()
    //        //{
    //        //    Console.WriteLine("  ModuleDefaultValues");

    //        //    int startStageID = int.Parse(moduleStartStages[ModuleName].ToString());

    //        //    List<string[]> dataList = new List<string[]>();

    //        //    dataList.Add(new string[] { ModuleName, "NeedReview", "Yes", startStageID.ToString(), "" });
    //        //    dataList.Add(new string[] { ModuleName, "RepeatInterval", "None", startStageID.ToString(), "" });

    //        //    SPListItemCollection listItems = .Lists["ModuleDefaultValues.Items;
    //        //    foreach (string[] data in dataList)
    //        //    {
    //        //        SPListItem item = listItems.Add();
    //        //        mDefaultValues.Title = data[0] + " - " + data[1];
    //        //        mDefaultValues.ModuleNameLookup = data[0];
    //        //        mDefaultValues.KeyName = data[1];
    //        //        mDefaultValues.KeyValue = data[2];
    //        //        mDefaultValues.ModuleStepLookup = data[3];
    //        //        mDefaultValues.CustomProperties = data[4];
    //        //        item.Update();
    //        //    }
    //        //}
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = invModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }
    //    class VND  ///need to check the data
    //    {
    //        protected static IModule vndModule = dbStore.LoadVNDModule();
    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: VND Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            addItemsToResourceCategories();
    //            Console.WriteLine("Finished: VND Request at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = vndModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = vndModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = vndModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = vndModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = vndModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = vndModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = vndModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = vndModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //        public void addItemsToResourceCategories()
    //        {//Need  to work
    //         //Console.WriteLine("  VendorResourceCategory");

    //            //List<string[]> dataList = new List<string[]>();

    //            //dataList.Add(new string[] { "Services", "100", "Developer", "100-001" });
    //            //dataList.Add(new string[] { "Services", "100", "BSA", "100-002" });
    //            //dataList.Add(new string[] { "Services", "100", "DBA", "100-003" });
    //            //dataList.Add(new string[] { "Services", "100", "PM", "100-004" });
    //            //dataList.Add(new string[] { "Services", "100", "QA", "100-005" });
    //            //dataList.Add(new string[] { "Services", "100", "Support", "100-006" });

    //            //dataList.Add(new string[] { "Other", "200", "Sofware", "200-001" });
    //            //dataList.Add(new string[] { "Other", "200", "Misc", "200-002" });


    //            //if (listItems.Count > 0)
    //            //{
    //            //    Console.WriteLine("    Found existing data, skipped");
    //            //    return;
    //            //}

    //            //foreach (string[] data in dataList)
    //            //{

    //            //    item["Title = data[0] + "-" + data[2];
    //            //    item["CategoryName = data[0];
    //            //    item["CategoryCode = data[1];
    //            //    item["SubCategory = data[2];
    //            //    item["SubCategoryCode = data[3];
    //            //    item["UGITDescription = item["Title;

    //            //}
    //        }
    //    }
    //    class VPM
    //    {
    //        protected static IModule vpmModule = dbStore.LoadVPMModule();
    //        protected string ModuleName = "VPM"; //20
    //        protected static bool enableOwnerApprovalStage = true;

    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: VPM Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemsToTaskEmails();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: VPM Request at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = vpmModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }

    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = vpmModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }

    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = vpmModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);

    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = vpmModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }

    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = vpmModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = vpmModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = vpmModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }

    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = vpmModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }

    //    class VFM
    //    {
    //        protected static IModule vfmModule = dbStore.LoadVFMModule();
    //        protected string ModuleName = "21";

    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: VFM Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemsToTaskEmails();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: VFM Request at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = vfmModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = vfmModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = vfmModule.GetLifeCycleStage();
    //            foreach (LifeCycleStage module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "LifeCycle" && x != "ApprovedStage" && x != "RejectStage" && x != "ReturnStage" && x != "lifeCycle" && x != "Description" && x != "Weight" && x != "SelectedTab" && x != "Prop_QuickClose" && x != "Prop_SelfAssign" && x != "Prop_DoNotWaitForActionUser" && x != "Prop_AutoApprove" && x != "Prop_BaseLine" && x != "Prop_CustomIconApprove" && x != "Prop_CustomIconReject" && x != "Prop_CustomIconReturn" && x != "Prop_NewNotification" && x != "Prop_UpdateNotification" && x != "Prop_OnResolveNotification" && x != "Prop_ResolvedNotification" && x != "Prop_CheckAssigneeToAllTask" && x != "Prop_ReadyToImport" && x != "Prop_PMOReview" && x != "Prop_ITGReview" && x != "Prop_ITSCReview" && x != "ApproveActionTooltip" && x != "RejectActionTooltip" && x != "ReturnActionToolip" && x != "LifeCycleID" && x != "LifeCycleName" && x != "Prop_AllowEmailApproval"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@LifeCycle" && x != "@ApprovedStage" && x != "@RejectStage" && x != "@ReturnStage" && x != "@lifeCycle" && x != "@Description" && x != "@Weight" && x != "@SelectedTab" && x != "@Prop_QuickClose" && x != "@Prop_SelfAssign" && x != "@Prop_DoNotWaitForActionUser" && x != "@Prop_AutoApprove" && x != "@Prop_BaseLine" && x != "@Prop_CustomIconApprove" && x != "@Prop_CustomIconReject" && x != "@Prop_CustomIconReturn" && x != "@Prop_NewNotification" && x != "@Prop_UpdateNotification" && x != "@Prop_OnResolveNotification" && x != "@Prop_ResolvedNotification" && x != "@Prop_CheckAssigneeToAllTask" && x != "@Prop_ReadyToImport" && x != "@Prop_PMOReview" && x != "@Prop_ITGReview" && x != "@Prop_ITSCReview" && x != "@ApproveActionTooltip" && x != "@RejectActionTooltip" && x != "@ReturnActionToolip" && x != "@LifeCycleID" && x != "@LifeCycleName" && x != "@Prop_AllowEmailApproval"));
    //                int? result = uGITDAL.insertData<LifeCycleStage>(module, "Config_Module_ModuleStages", columns, values);
    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = vfmModule.GetModuleTaskEmail();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = vfmModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = vfmModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = vfmModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = vfmModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }

    //    class VSW
    //    {

    //        protected static IModule vswModule = dbStore.LoadVSWModule();
    //        protected string ModuleName = "22";

    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: VSW Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToRequestTypes();
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: VSW Request at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = vswModule.GetModuleRequestType();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = vswModule.GetModuleRoleWriteAccess();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = vswModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = vswModule.GetModuleTaskEmail();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = vswModule.GetFormTabs();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = vswModule.GetModuleFormLayout();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = vswModule.GetModuleDefaultValue();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = vswModule.GetModuleStatusMapping();
    //            uGITDAL.InsertItem(modules);
    //        }
    //    }

    //    class VSL
    //    {
    //        protected static IModule vslModule = dbStore.LoadVSLModule();

    //        public void Initialize()
    //        {
    //            Console.WriteLine("Started: VSW Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToRequestTypes();
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: VSW Request at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestTypes()
    //        {

    //            List<ModuleRequestType> modules = vslModule.GetModuleRequestType();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = vslModule.GetModuleRoleWriteAccess();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = vslModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = vslModule.GetModuleTaskEmail();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = vslModule.GetFormTabs();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = vslModule.GetModuleFormLayout();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = vslModule.GetModuleDefaultValue();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = vslModule.GetModuleStatusMapping();
    //            uGITDAL.InsertItem(modules);
    //        }
    //    }
    //    class VCC
    //    {
    //        protected static IModule vccModule = dbStore.LoadVCCModule();

    //        // protected string ModuleName = "24";
    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: VCC Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToRequestTypes();
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: VCC Request at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = vccModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = vccModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = vccModule.GetLifeCycleStage();
    //            foreach (LifeCycleStage module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "LifeCycle" && x != "ApprovedStage" && x != "RejectStage" && x != "ReturnStage" && x != "lifeCycle" && x != "Description" && x != "Weight" && x != "SelectedTab" && x != "Prop_QuickClose" && x != "Prop_SelfAssign" && x != "Prop_DoNotWaitForActionUser" && x != "Prop_AutoApprove" && x != "Prop_BaseLine" && x != "Prop_CustomIconApprove" && x != "Prop_CustomIconReject" && x != "Prop_CustomIconReturn" && x != "Prop_NewNotification" && x != "Prop_UpdateNotification" && x != "Prop_OnResolveNotification" && x != "Prop_ResolvedNotification" && x != "Prop_CheckAssigneeToAllTask" && x != "Prop_ReadyToImport" && x != "Prop_PMOReview" && x != "Prop_ITGReview" && x != "Prop_ITSCReview" && x != "ApproveActionTooltip" && x != "RejectActionTooltip" && x != "ReturnActionToolip" && x != "LifeCycleID" && x != "LifeCycleName" && x != "Prop_AllowEmailApproval"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@LifeCycle" && x != "@ApprovedStage" && x != "@RejectStage" && x != "@ReturnStage" && x != "@lifeCycle" && x != "@Description" && x != "@Weight" && x != "@SelectedTab" && x != "@Prop_QuickClose" && x != "@Prop_SelfAssign" && x != "@Prop_DoNotWaitForActionUser" && x != "@Prop_AutoApprove" && x != "@Prop_BaseLine" && x != "@Prop_CustomIconApprove" && x != "@Prop_CustomIconReject" && x != "@Prop_CustomIconReturn" && x != "@Prop_NewNotification" && x != "@Prop_UpdateNotification" && x != "@Prop_OnResolveNotification" && x != "@Prop_ResolvedNotification" && x != "@Prop_CheckAssigneeToAllTask" && x != "@Prop_ReadyToImport" && x != "@Prop_PMOReview" && x != "@Prop_ITGReview" && x != "@Prop_ITSCReview" && x != "@ApproveActionTooltip" && x != "@RejectActionTooltip" && x != "@ReturnActionToolip" && x != "@LifeCycleID" && x != "@LifeCycleName" && x != "@Prop_AllowEmailApproval"));
    //                int? result = uGITDAL.insertData<LifeCycleStage>(module, "Config_Module_ModuleStages", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = vccModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = vccModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = vccModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = vccModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }

    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = vccModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //    }
    //    class CRM
    //    {

    //        protected static IModule crmModule = dbStore.LoadCRMModule();
    //        protected string ModuleName = "26";

    //        protected static bool enableOwnerApprovalStage = true;

    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: CRM Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemsToTaskEmails();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: CRM Request at " + DateTime.Now.ToString());


    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = crmModule.GetModuleRequestType();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = crmModule.GetModuleRoleWriteAccess();
    //            uGITDAL.InsertItem(modules);


    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = crmModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = crmModule.GetModuleTaskEmail();
    //            uGITDAL.InsertItem(modules);


    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = crmModule.GetFormTabs();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = crmModule.GetModuleFormLayout();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = crmModule.GetModuleDefaultValue();
    //            uGITDAL.InsertItem(modules);

    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = crmModule.GetModuleStatusMapping();
    //            uGITDAL.InsertItem(modules);

    //        }
    //    }

    //    class PLC
    //    {

    //        protected static IModule plcModule = dbStore.LoadPLCModule();
    //        protected string ModuleName = "27";

    //        protected static bool enableOwnerApprovalStage = true;

    //        public void Initialize()
    //        {

    //            Console.WriteLine("Started: PLC Request at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemsToTaskEmails();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: PLC Request at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = plcModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = plcModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }

    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = plcModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);

    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = plcModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = plcModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }

    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = plcModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }

    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = plcModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }

    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = plcModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }

    //        }
    //    }


    //    class RCA
    //    {
    //        protected static IModule rcaModule = dbStore.LoadRCAModule();
    //        public void Initialize()
    //        {
    //            //Default values for Lists
    //            Console.WriteLine("Started: RCA Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }
    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: RCA Ticket at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = rcaModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = rcaModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = rcaModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = rcaModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = rcaModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = rcaModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = rcaModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }

    //    class OPM
    //    {
    //        protected static IModule opmModule = dbStore.LoadOPMModule();

    //        protected string ModuleName = "25";
    //        protected static bool enableTestingStage = false;
    //        protected static bool enablePendingCloseStage = false;

    //        public void Initialize()
    //        {
    //            //Default values for Lists
    //            Console.WriteLine("Started: OPM Ticket at " + DateTime.Now.ToString());
    //            addItemsToRequestRoleWriteAccess();

    //            if (loadRequestTypes)
    //                addItemsToRequestTypes();

    //            if (loadModuleStages)
    //            {
    //                addItemsToStages();
    //                addItemsToStatusMappingList();
    //                addItemsToTaskEmails();
    //            }

    //            addItemsToModuleFormTab();
    //            addItemsToFormLayout();
    //            addItemToModuleDefaultValues();
    //            Console.WriteLine("Finished: OPM Ticket at " + DateTime.Now.ToString());

    //        }

    //        public void addItemsToRequestTypes()
    //        {
    //            List<ModuleRequestType> modules = opmModule.GetModuleRequestType();
    //            foreach (ModuleRequestType module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TaskTemplateLookup" && x != "FunctionalAreaLookup" && x != "BackupEscalationManager" && x != "ORP" && x != "EscalationManager" && x != "Owner" && x != "SubCategory" && x != "ApplicationModulesLookup"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TaskTemplateLookup" && x != "@FunctionalAreaLookup" && x != "@BackupEscalationManager" && x != "@ORP" && x != "@EscalationManager" && x != "@Owner" && x != "@SubCategory" && x != "@ApplicationModulesLookup"));
    //                int? result = uGITDAL.insertData<ModuleRequestType>(module, "Config_Module_RequestType", columns, values);
    //            }
    //        }
    //        public void addItemsToTaskEmails()
    //        {
    //            List<ModuleTaskEmail> modules = opmModule.GetModuleTaskEmail();
    //            foreach (ModuleTaskEmail module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleTaskEmail>(module, "Config_Module_TaskEmails", columns, values);
    //            }
    //        }
    //        public void addItemsToRequestRoleWriteAccess()
    //        {
    //            List<ModuleRoleWriteAccess> modules = opmModule.GetModuleRoleWriteAccess();
    //            foreach (ModuleRoleWriteAccess module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleRoleWriteAccess>(module, "Config_Module_RequestRoleWriteAccess", columns, values);
    //            }
    //        }
    //        public void addItemsToStages()
    //        {
    //            List<LifeCycleStage> modules = opmModule.GetLifeCycleStage();
    //            uGITDAL.InsertItem(modules);
    //        }
    //        public void addItemsToModuleFormTab()
    //        {
    //            List<ModuleFormTab> modules = opmModule.GetFormTabs();
    //            foreach (ModuleFormTab module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID" && x != "TabSequence"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID" && x != "@TabSequence"));
    //                int? result = uGITDAL.insertData<ModuleFormTab>(module, "Config_Module_ModuleFormTab", columns, values);
    //            }
    //        }
    //        public void addItemsToFormLayout()
    //        {
    //            List<ModuleFormLayout> modules = opmModule.GetModuleFormLayout();
    //            foreach (ModuleFormLayout module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleFormLayout>(module, "Config_Module_FormLayout", columns, values);
    //            }
    //        }
    //        public void addItemToModuleDefaultValues()
    //        {
    //            List<ModuleDefaultValue> modules = opmModule.GetModuleDefaultValue();
    //            foreach (ModuleDefaultValue module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleDefaultValue>(module, "ModuleDefaultValues", columns, values);
    //            }
    //        }
    //        public void addItemsToStatusMappingList()
    //        {
    //            List<ModuleStatusMapping> modules = opmModule.GetModuleStatusMapping();
    //            foreach (ModuleStatusMapping module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleStatusMapping>(module, "Config_Module_StatusMapping", columns, values);
    //            }
    //        }
    //    }
    //    class Dashboard
    //    {
    //        public static IDashboard dhbdModule = dbStore.LoadDashboard();
    //        public void Initialize()
    //        {

    //            //Default values for Lists 
    //            Console.WriteLine("Started: Dashboard at " + DateTime.Now.ToString());
    //            if (loadModuleStages)
    //            {
    //                //addItemsToSLARule();
    //            }

    //            // addItemToDashboardSummary(); need to work
    //            // addItemToChartFilters();
    //            // addItemToFormulas();
    //            addItemToChartTemplates();
    //            //addItemsToEscalationRule();
    //            //addItemsToDashboardFactTable();
    //            Console.WriteLine("Finished: Dashboard at " + DateTime.Now.ToString());

    //        }
    //        public void addItemsToSLARule()
    //        {
    //            List<ModuleSLARule> modules = dhbdModule.GetModuleSLARule();
    //            foreach (ModuleSLARule module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleSLARule>(module, " Config_Module_SLARule", columns, values);
    //            }


    //        }
    //        public void addItemsToEscalationRule()
    //        {
    //            List<ModuleEscalationRule> modules = dhbdModule.GetModuleEscalationRule();
    //            foreach (ModuleEscalationRule module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ModuleEscalationRule>(module, " Config_Module_EscalationRule", columns, values);
    //            }

    //        }
    //        //public void addItemToDashboardSummary()
    //        //{
    //        //    Console.WriteLine("  DashboardSummary fields");

    //        //    listItems = web.Lists["DashboardSummary.Items;
    //        //    SPList stageTypeList = web.Lists["StageType;
    //        //    SPFieldChoice magerStages = (SPFieldChoice)stageTypeList.Fields["Stage Type;
    //        //    foreach (string stage in magerStages.Choices)
    //        //    {
    //        //        string fieldName = stage + "Date";
    //        //        listItems.List.Fields.Add(fieldName, SPFieldType.DateTime, false);
    //        //    }
    //        //}
    //        public void addItemToChartFilters()
    //        {
    //            List<ChartFilters> modules = dhbdModule.GetChartFilters();
    //            foreach (ChartFilters module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ChartFilters>(module, "ChartFilters", columns, values);
    //            }
    //        }
    //        public void addItemToFormulas()
    //        {
    //            List<ChartFormula> modules = dhbdModule.GetChartFormula();
    //            foreach (ChartFormula module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ChartFormula>(module, "ChartFormula", columns, values);
    //            }

    //        }
    //        public void addItemToChartTemplates()
    //        {
    //            List<ChartTemplates> modules = dhbdModule.GetChartTemplates();
    //            foreach (ChartTemplates module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<ChartTemplates>(module, "ChartTemplates", columns, values);
    //            }
    //        }
    //        public void addItemsToDashboardFactTable()
    //        {
    //            List<DashboardFactTables> modules = dhbdModule.GetDashboardFactTables();
    //            foreach (DashboardFactTables module in modules)
    //            {
    //                string columns = string.Join(",", module.GetType().GetProperties().Select(x => x.Name).Where(x => x != "ID"));
    //                string values = string.Join(",", module.GetType().GetProperties().Select(x => "@" + x.Name).Where(x => x != "@ID"));
    //                int? result = uGITDAL.insertData<DashboardFactTables>(module, "Config_Dashboard_DashboardFactTables", columns, values);
    //            }

    //        }
    //    }
    //}
    #endregion "notusedcode"
}





