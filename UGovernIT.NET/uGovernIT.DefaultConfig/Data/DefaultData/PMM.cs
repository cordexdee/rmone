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
    public class PMM : IPMMModule
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
        protected string moduleId = "7";

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Current Projects",
                    CategoryName = "Project Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "PMM",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/pmmprojects",
                    ModuleHoldMaxStage = 0,
                    Title = "Project Management Module (PMM)",
                    ModuleDescription = "This module is used to monitor and manage projects in progress that have been approved through the NPR module.",
                    ThemeColor = "Accent3",
                    StaticModulePagePath = "/Pages/pmm",
                    ModuleType = ModuleType.Governance,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = false,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    AllowDelete = true,
                    UseInGlobalSearch = true,
                    EnableBaseLine=true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "PMM";
            }
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "TicketId", FieldDisplayName = "Project ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, DisplayForReport = true, SortOrder = 1, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, DisplayForReport = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, DisplayForReport = true, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            //mList.Add(new ModuleColumns() { ModuleNameLookup = "PMM", "CompanyMultiLookup", "Company", "1", "0", ";#fieldtype=multilookup" }); 
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", DisplayForClosed = true, IsUseInWildCard = true, DisplayForReport = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "RequestTypeLookup", FieldDisplayName = "Project Type", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, DisplayForReport = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, DisplayForReport = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectRank", FieldDisplayName = "Rank", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "StageStep", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectManagerUser", FieldDisplayName = "Proj Mgr(s)", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, DisplayForReport = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "PctComplete", FieldDisplayName = "% Comp", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, DisplayForReport = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "fieldtype=datetime;#useforglobaldatefilter=true", DisplayForClosed = false, DisplayForReport = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ActualStartDate", FieldDisplayName = "Planned Start", CustomProperties = "fieldtype=datetime", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ActualCompletionDate", FieldDisplayName = "Planned Close", CustomProperties = "fieldtype=datetime", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "Created", FieldDisplayName = "Created On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum }); // PMM doesn't update TicketCreationDate
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "CloseDate", FieldDisplayName = "Closed On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectScore", FieldDisplayName = "Project Score", ColumnType = "IndicatorLight", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectInitiativeLookup", FieldDisplayName = "Program", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "IsPrivate", FieldDisplayName = "IsPrivate", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectLifeCycleLookup", FieldDisplayName = "Lifecycle", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "BeneficiariesLookup", FieldDisplayName = "Beneficiaries", ColumnType = "MultiLookup", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "TotalCost", FieldDisplayName = "Total Budget", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectCost", FieldDisplayName = "Total Actual", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ApprovedRFE", FieldDisplayName = "GL Code", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ApprovedRFEAmount", FieldDisplayName = "Approved Amount", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ApprovedRFEType", FieldDisplayName = "Budget Type", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectSummaryNote", FieldDisplayName = "Project Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "SponsorsUser", FieldDisplayName = "Sponsors", ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "StakeHoldersUser", FieldDisplayName = "Stakeholders", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "NextActivity", FieldDisplayName = "Next Activity", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "NextMilestone", FieldDisplayName = "Next Milestone", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "PMM", FieldName = "ProjectMonitors", FieldDisplayName = "Monitor", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, DisplayForReport = true, FieldSequence = ++seqNum });
            return mList;
        }
        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");
            mList.Add(new ModuleFormTab() { TabName = "Summary", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Summary" });
            mList.Add(new ModuleFormTab() { TabName = "Project Plan", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Project Plan" });
            mList.Add(new ModuleFormTab() { TabName = "Scrum", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Scrum" });
            mList.Add(new ModuleFormTab() { TabName = "Status", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Status" });
            mList.Add(new ModuleFormTab() { TabName = "Budget", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Budget" });
            mList.Add(new ModuleFormTab() { TabName = "Documents", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Documents" });
            mList.Add(new ModuleFormTab() { TabName = "Calendar", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Calendar" });
            mList.Add(new ModuleFormTab() { TabName = "Sunset Review", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Sunset Review" });
            mList.Add(new ModuleFormTab() { TabName = "Project Team", TabId = 9, TabSequence = 9, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + "-" + "Project Team" });
            mList.Add(new ModuleFormTab() { TabName = "Resource Sheet", TabId = 10, TabSequence = 10, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Resource Sheet" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 11, TabSequence = 11, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 12, TabSequence = 12, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }
        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            return mList;
        }
        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "PMM", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "PMM", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "PMM", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "PMM", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4 });
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PMM", Title = "PMM-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PMM", Title = "PMM-Sponsors", UserTypes = "Sponsor", ColumnName = "SponsorsUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PMM", Title = "PMM-Stake Holders", UserTypes = "Stake Holder", ColumnName = "StakeHoldersUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            //mList.Add(new ModuleUserType() { ModuleNameLookup = "PMM", Title = "PMM-Project Director", UserTypes = "Project Director", ColumnName = "ProjectDirector", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "PMM", Title = "PMM-Project Manager", UserTypes = "Project Manager", ColumnName = "ProjectManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            // mList.Add(new ModuleUserType() { ModuleNameLookup = "PMM", Title = "PMM-Analyst", UserTypes = "Analyst", ColumnName = "AssignedAnalyst", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
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
        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
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
            
            List<string[]> dataList = new List<string[]>();
            int seqNum = 0;
            // Tab 1: General 
            // Display Basic project infomation title, manager, benieficaries, sponsors
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related NPR ID", FieldDisplayName = "Related NPR ID", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "NPRIdLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Initiated", FieldDisplayName = "Initiated", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Created", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Target Go-Live", FieldDisplayName = "Target Go-Live", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Enable Scrum", FieldDisplayName = "Enable Scrum", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ScrumLifeCycle", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Sponsors", FieldDisplayName = "Sponsors", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SponsorsUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Manager", FieldDisplayName = "Project Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Initiative", FieldDisplayName = "Initiative", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectInitiativeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Functional Area", FieldDisplayName = "Functional Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Primary Beneficiaries", FieldDisplayName = "Primary Beneficiaries", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Project Class", FieldDisplayName = "Project Class", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectClassLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Locations Affected", FieldDisplayName = "Locations Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "LocationMultLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Application Affected", FieldDisplayName = "Application Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "APPTitleLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Application Functionality", FieldDisplayName = "Application Functionality", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "ModuleName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Project Type", FieldDisplayName = "Project Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Rank", FieldDisplayName = "Project Rank", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectRank", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "GL Code", FieldDisplayName = "GL Code", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ApprovedRFE", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approved Amount ($)", FieldDisplayName = "Approved Amount ($)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ApprovedRFEAmount", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Budget Type", FieldDisplayName = "Budget Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ApprovedRFEType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Supporting Materials", FieldDisplayName = "Supporting Materials", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Problem Being Solved", FieldDisplayName = "Problem Being Solved", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProblemBeingSolved", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Solution / Benefits", FieldDisplayName = "Solution / Benefits", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectBenefits", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Scope", FieldDisplayName = "Project Scope", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectScope", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Assumptions", FieldDisplayName = "Project Assumptions", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectAssumptions", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Risks", FieldDisplayName = "Project Risks", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectRiskNotes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Supporting Materials", FieldDisplayName = "Supporting Materials", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Recent Documents", FieldDisplayName = "Recent Documents", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "PMMDocumentGrid", FieldDisplayName = "PMMDocumentGrid", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Recent Documents", FieldDisplayName = "Recent Documents", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Attachment

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 2
            seqNum = 0;
            //  Display project task editable
            mList.Add(new ModuleFormLayout() { Title = "Project Tasks", FieldDisplayName = "Project Tasks", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "TasksList", FieldDisplayName = "TasksList", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Tasks", FieldDisplayName = "Project Tasks", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 3 - Sprints
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "PMMSprints", FieldDisplayName = "PMMSprints", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 4 - Project status
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Status Details", FieldDisplayName = "Status Details", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ProjectStatusDetail", FieldDisplayName = "ProjectStatusDetail", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Status Details", FieldDisplayName = "Status Details", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 5 - Project budget
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Estimated Spend to Completion", FieldDisplayName = "Estimated Spend to Completion", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Estimated Spend ($)", FieldDisplayName = "Estimated Spend ($)", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 1, FieldName = "EstProjectSpend", CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comment", FieldDisplayName = "Comment", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 2, FieldName = "EstProjectSpendComment", CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Estimated Spend to Completion", FieldDisplayName = "Estimated Spend to Completion", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ProjectBudgetDetail", FieldDisplayName = "ProjectBudgetDetail", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 6 - Documents
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "DocumentControl", FieldDisplayName = "DocumentControl", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Documents", FieldDisplayName = "Documents", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 7 - Project Calendar
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Project Calendar", FieldDisplayName = "Project Calendar", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "PMMEventsCalendar", FieldDisplayName = "PMMEventsCalendar", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Calendar", FieldDisplayName = "Project Calendar", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 8 - Sunset Review
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Benefits", FieldDisplayName = "Benefits", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Benefits Experienced", FieldDisplayName = "Benefits Experienced", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "BenefitsExperienced", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Improves Operational Efficiency", FieldDisplayName = "Improves Operational Efficiency", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "ImpactAsOperationalEfficiency", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Saves Money", FieldDisplayName = "Saves Money", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "ImpactAsSaveMoney", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Improves Revenues", FieldDisplayName = "Improves Revenues", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "ImpactAsImprovesRevenues", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Breakeven in", FieldDisplayName = "Breakeven in", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "BreakevenMonth", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Eliminates HeadCount", FieldDisplayName = "Eliminates HeadCount", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "EliminatesStaffHeadCount", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ROI", FieldDisplayName = "ROI", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "ROI", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Benefits", FieldDisplayName = "Benefits", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Lessons Learned

            mList.Add(new ModuleFormLayout() { Title = "Lessons Learned", FieldDisplayName = "Lessons Learned", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Describe Lessons Learned", FieldDisplayName = "Describe Lessons Learned", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "LessonsLearned", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Lessons Learned", FieldDisplayName = "Lessons Learned", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Project Contraints

            mList.Add(new ModuleFormLayout() { Title = "Constraints;15%#Values;15%#Notes;60%", FieldDisplayName = "Constraints;15%#Values;15%#Notes;60%", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#TableStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "No Of FTEs", FieldDisplayName = "No Of FTEs", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "#Label#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = ".", FieldDisplayName = ".", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "TicketNoOfFTEs", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = ".", FieldDisplayName = ".", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "TicketNoOfFTEsNotes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "No Of Consultants", FieldDisplayName = "No Of Consultants", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "#Label#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = ".", FieldDisplayName = ".", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "TicketNoOfConsultants", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = ".", FieldDisplayName = ".", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 1, FieldName = "TicketNoOfConsultantsNotes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Constraints", FieldDisplayName = "Constraints", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#TableEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Project Variances

            mList.Add(new ModuleFormLayout() { Title = "Project Variances", FieldDisplayName = "Project Variances", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ProjectVarianceReport", FieldDisplayName = "ProjectVarianceReport", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Variances", FieldDisplayName = "Project Variances", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 9 - Project Team
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Project Team", FieldDisplayName = "Project Team", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ProjectEstimatedAllocation", FieldDisplayName = "ProjectEstimatedAllocation", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, FieldName = "#Control#", CustomProperties = "IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Team", FieldDisplayName = "Project Team", ModuleNameLookup = ModuleName, TabId = 9, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 10 - Resources
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Resources", FieldDisplayName = "Resources", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ProjectResourceDetail", FieldDisplayName = "ProjectResourceDetail", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, FieldName = "#Control#", CustomProperties = "IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Resources", FieldDisplayName = "Resources", ModuleNameLookup = ModuleName, TabId = 10, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 11 - Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 11, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 12

            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 12, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // NOTE: All fields are writeable in all stages

            //Tab 1
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectManager", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketDesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Sponsors", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BeneficiariesLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectClassLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectInitiativeLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FunctionalAreavLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectRank", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectRank2", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectRank3", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketDescription", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketRequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectIteration", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPGroupUser", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LocationMultLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ModuleName", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketProjectScope", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketProjectAssumptions", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketProjectBenefits", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectRiskNotes", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProblemBeingSolved", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "OwnerUser", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPGroupUser", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketClassification", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketClassificationType", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketClassificationImpact", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ScrumLifeCycle", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });

            //Tab 2
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ProjectStatus", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false });

            //Tab 4
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BenefitsExperienced", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactAsOperationalEfficiency", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = true, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactAsSaveMoney", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = true, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactAsImprovesRevenues", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = true, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "BreakevenMonth", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = true, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EliminatesStaffHeadCount", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = true, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ROI", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = true, ShowEditButton = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketNoOfFTEsNotes", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TicketNoOfConsultantsNotes", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstProjectSpend", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "LessonsLearned", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false });

            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstProjectSpend", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstProjectSpendComment", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });

            //Tab 6
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DocumentControl", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = false });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            /*
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "ERP", RequestType = "ERP", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "3rd-Party Apps", RequestType = "3rd-Party Apps", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "Custom Apps", RequestType = "Custom Apps", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "Collaboration", RequestType = "Collaboration", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "BI/Analytical", RequestType = "BI/Analytical", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });

            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Data Center", RequestType = "Data Center", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Office Management", RequestType = "Office Management", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "User Hardware", RequestType = "User Hardware", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Telecommunications", RequestType = "Telecommunications", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Other Infrastructure", RequestType = "Other Infrastructure", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });

            mList.Add(new ModuleRequestType() { Title = "Regulatory/Compliance" + ">" + "Sox Compliance", RequestType = "Sox Compliance", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Regulatory/Compliance", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Regulatory/Compliance" + ">" + "Other Mandatory", RequestType = "Other Mandatory", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Regulatory/Compliance", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            */

            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Business Applications", "3rd Party Apps"), Category = "Business Applications", RequestType = "3rd Party Apps", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Business Applications", "BI/Analytical"), Category = "Business Applications", RequestType = "BI/Analytical", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Business Applications", "Collaboration"), Category = "Business Applications", RequestType = "Collaboration", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Business Applications", "CRM"), Category = "Business Applications", RequestType = "CRM", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Business Applications", "Custom Solutions"), Category = "Business Applications", RequestType = "Custom Solutions", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Business Applications", "ERP"), Category = "Business Applications", RequestType = "ERP", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Business Applications", "Legacy"), Category = "Business Applications", RequestType = "Legacy", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Infrastructure", "Data Center"), Category = "Infrastructure", RequestType = "Data Center", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Infrastructure", "Office Management"), Category = "Infrastructure", RequestType = "Office Management", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Infrastructure", "Other Infrastructure"), Category = "Infrastructure", RequestType = "Other Infrastructure", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Infrastructure", "Telecommunications"), Category = "Infrastructure", RequestType = "Telecommunications", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Infrastructure", "User Hardware"), Category = "Infrastructure", RequestType = "User Hardware", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Innovation", "AI/ML"), Category = "Innovation", RequestType = "AI/ML", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 6 });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Innovation", "AR/VR/MR"), Category = "Innovation", RequestType = "AR/VR/MR", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 5 });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Regulatory/Compliance", "Other Manadatory"), Category = "Regulatory/Compliance", RequestType = "Other Manadatory", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Regulatory/Compliance", "Sox Compliance"), Category = "Regulatory/Compliance", RequestType = "Sox Compliance", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Services", "Cloud Migration"), Category = "Services", RequestType = "Cloud Migration", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 7 });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Services", "Digital Transformation"), Category = "Services", RequestType = "Digital Transformation", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 8 });


            return mList;

        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            List<string[]> dataList = new List<string[]>();

            // Start from id - 44
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            pmmStartStageID = currStageID;
            int numStages = 7;
            string closeStageID = (currStageID + numStages - 1).ToString();
            pmmClosedStageID = int.Parse(closeStageID);

            mList.Add(new LifeCycleStage()
            {
                Name = "Concept",
                StageTitle = "Concept",
                UserWorkflowStatus = "Concept Phase",
                UserPrompt = "<b>Project is in Concept / Identification Phase.</b>",
                StageStep = 1,
                ModuleNameLookup = ModuleName,
                Action = "Concept",
                ActionUser = "ProjectManagerUser;#PMO",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Concept",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Plan",
                StageTitle = "Plan",
                UserWorkflowStatus = "Plan Phase",
                UserPrompt = "<b>Project is in Planning Phase.</b>",
                StageStep = 2,
                ModuleNameLookup = ModuleName,
                Action = "Plan",
                ActionUser = "ProjectManagerUser;#PMO",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Plan",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Design",
                StageTitle = "Design",
                UserWorkflowStatus = "Design Phase",
                UserPrompt = "<b>Project is in Design Phase.</b>",
                StageStep = 3,
                ModuleNameLookup = ModuleName,
                Action = "Design",
                ActionUser = "ProjectManagerUser;#PMO",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Design",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });


            mList.Add(new LifeCycleStage()
            {
                Name = "Build",
                StageTitle = "Build",
                UserWorkflowStatus = "Build Phase",
                UserPrompt = "<b>Project is in Build / Implementation Phase.</b>",
                StageStep = 4,
                ModuleNameLookup = ModuleName,
                Action = "Build",
                ActionUser = "ProjectManagerUser;#PMO",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Build",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 30,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Test",
                StageTitle = "Test",
                UserWorkflowStatus = "Test Phase",
                UserPrompt = "<b>Project is in Testing Phase.</b>",
                StageStep = 5,
                ModuleNameLookup = ModuleName,
                Action = "Test",
                ActionUser = "ProjectManagerUser;#PMO",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Test",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });


            mList.Add(new LifeCycleStage()
            {
                Name = "Deploy",
                StageTitle = "Deploy",
                UserWorkflowStatus = "Deploy Phase",
                UserPrompt = "b>Project is in Deployment / Rollout Phase.</b>",
                StageStep = 6,
                ModuleNameLookup = ModuleName,
                Action = "Deploy",
                ActionUser = "ProjectManagerUser;#PMO",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Deploy",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Closeout",
                StageTitle = "Closeout",
                UserWorkflowStatus = "Closeout Phase",
                UserPrompt = "<b>Project has been closed out.</b>",
                StageStep = 7,
                ModuleNameLookup = ModuleName,
                Action = "Closeout",
                ActionUser = "ProjectManagerUser;#PMO",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Concept",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });

            // PMM doesn't use stages so just manually set it to closed stage
            currStageID = int.Parse(closeStageID);

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
            return mList;
        }

        public List<DRQSystemArea> GetDRQSystemAreas()
        {
            List<DRQSystemArea> mList = new List<DRQSystemArea>();
            return mList;
        }

        public List<ProjectClass> GetProjectClass()
        {
            List<ProjectClass> mList = new List<ProjectClass>();
            // Console.WriteLine("ProjectClass");
            mList.Add(new ProjectClass() { Title = "Lights On", ProjectNote = "Projects that are necessary to continuing operations" });
            mList.Add(new ProjectClass() { Title = "Efficiency/Productivity", ProjectNote = "Projects that provide substantial value by significantly improving processes, are measured on ROI (revenue, cost, hours), or provide key analytical capability" });
            mList.Add(new ProjectClass() { Title = "Strategic", ProjectNote = "Projects in support of company strategic initiatives or that produce breakthrough capabilities that can transform the business" });
            mList.Add(new ProjectClass() { Title = "Regulatory/Compliance", ProjectNote = "Projects necessary for legal or regulatory compliance or to protect company assets" });
            return mList;
        }
        public List<ProjectInitiative> GetProjectInitiative()
        {
            List<ProjectInitiative> mList = new List<ProjectInitiative>();
            // Console.WriteLine("  ProjectInitiative");
            mList.Add(new ProjectInitiative() { Title = "BI Initiative", ProjectNote = "Business Intelligence Projects" });
            mList.Add(new ProjectInitiative() { Title = "ERP Migration", ProjectNote = "ERP Migration Initiative" });
            mList.Add(new ProjectInitiative() { Title = "DR Initiative", ProjectNote = "DR Rollout Initiative" });
            return mList;
        }
        public List<ModuleLifeCycle> GetProjectLifeCycles()
        {
            List<ModuleLifeCycle> mList = new List<ModuleLifeCycle>();
            // Console.WriteLine("  ProjectLifeCycles");
            mList.Add(new ModuleLifeCycle() { Title = "Waterfall 5-Stage", Description = "5-Stage shortened waterfall cycle" });
            mList.Add(new ModuleLifeCycle() { Title = "Waterfall 7-Stage", Description = "7-Stage long waterfall cycle" });
            return mList;
        }

        public List<ProjectLifecycleStages> GetProjectLifecycleStages()
        {
            List<ProjectLifecycleStages> mList = new List<ProjectLifecycleStages>();
            // Console.WriteLine("  ProjectLifeCycleStages");
            // 5-stage waterfall
            mList.Add(new ProjectLifecycleStages() { Title = "Identification", Description = "Identification", ProjectLifeCycleLookup = 1, ModuleStep = 1, StageWeight = 1 });
            mList.Add(new ProjectLifecycleStages() { Title = "Evaluation", Description = "Evaluation", ProjectLifeCycleLookup = 1, ModuleStep = 2, StageWeight = 2 });
            mList.Add(new ProjectLifecycleStages() { Title = "Initiation", Description = "Initiation", ProjectLifeCycleLookup = 1, ModuleStep = 3, StageWeight = 1 });
            mList.Add(new ProjectLifecycleStages() { Title = "Execution", Description = "Execution", ProjectLifeCycleLookup = 1, ModuleStep = 4, StageWeight = 3 });
            mList.Add(new ProjectLifecycleStages() { Title = "Completion", Description = "Completion", ProjectLifeCycleLookup = 1, ModuleStep = 5, StageWeight = 1 });

            // 7-stage waterfall
            mList.Add(new ProjectLifecycleStages() { Title = "Concept", Description = "Concept", ProjectLifeCycleLookup = 2, ModuleStep = 1, StageWeight = 1 });
            mList.Add(new ProjectLifecycleStages() { Title = "Plan", Description = "Plan", ProjectLifeCycleLookup = 2, ModuleStep = 2, StageWeight = 1 });
            mList.Add(new ProjectLifecycleStages() { Title = "Design", Description = "Design", ProjectLifeCycleLookup = 2, ModuleStep = 3, StageWeight = 2 });
            mList.Add(new ProjectLifecycleStages() { Title = "Build", Description = "Build", ProjectLifeCycleLookup = 2, ModuleStep = 4, StageWeight = 4 });
            mList.Add(new ProjectLifecycleStages() { Title = "Test", Description = "Test", ProjectLifeCycleLookup = 2, ModuleStep = 5, StageWeight = 2 });
            mList.Add(new ProjectLifecycleStages() { Title = "Deploy", Description = "Deploy", ProjectLifeCycleLookup = 2, ModuleStep = 6, StageWeight = 1 });
            mList.Add(new ProjectLifecycleStages() { Title = "Closeout", Description = "Closeout", ProjectLifeCycleLookup = 2, ModuleStep = 7, StageWeight = 1 });
            return mList;
        }

        public List<EventCategory> GetEventCategories()
        {
            List<EventCategory> mList = new List<EventCategory>();
            // Console.WriteLine("  EventCategories");
            mList.Add(new EventCategory() { Title = "Status Meeting", ItemColor = "#99CCFF" });
            mList.Add(new EventCategory() { Title = "Project Event", ItemColor = "#FF0000" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "PMMProjects", TabOrder = 1, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "PMMProjects", TabOrder = 2, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Projects", ViewName = "PMMProjects", TabOrder = 3, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Projects", ViewName = "PMMProjects", TabOrder = 4, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Projects", ViewName = "PMMProjects", TabOrder = 5, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Deapartment Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Projects", ViewName = "PMMProjects", TabOrder = 6, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Projects", ViewName = "PMMProjects", TabOrder = 7, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Itmes" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Projects", ViewName = "PMMProjects", TabOrder = 8, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Projects", ViewName = "PMMProjects", TabOrder = 9, ModuleNameLookup = "PMM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "PMMProjects", TabOrder = 10, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
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
