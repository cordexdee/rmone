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
    public class NPR : IModule
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
        protected string moduleId = "6";
        protected static bool enableManagerApprovalStage = true;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "New Project Requests",
                    CategoryName = "Project Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "NPR",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/nprrequests",
                    ModuleHoldMaxStage = 4,
                    Title = "New Project Request (NPR)",
                    ModuleDescription = "This module is used to request new projects requiring significant IT resources. Use this module to request something that does not currently exist (Application, Web site, Database etc.) or for major upgrades. Typically, this request requires significant IT time and resources and spans a month or more in analysis / development time and effort.",                    
                    ThemeColor = "Accen3",
                    StaticModulePagePath = "/Pages/npr",
                    ModuleType = ModuleType.Governance,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    AllowDelete = true,
                    UseInGlobalSearch = true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "NPR";
            }
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "TicketId", FieldDisplayName = "Project ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
            //mList.Add(new ModuleColumns() { ModuleNameLookup = "NPR", "CompanyMultiLookup", "Company", "1", "0", ";#fieldtype=multilookup" }); 
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "RequestTypeLookup", FieldDisplayName = "Project Type", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "ProjectRank", FieldDisplayName = "Rank", IsUseInWildCard = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "Age", FieldDisplayName = "Age", IsUseInWildCard = true, FieldSequence = ++seqNum });
            //mList.Add(new ModuleColumns() { ModuleNameLookup = "NPR", "ActualCompletionDate", "Target Date", "1" }); // Sort 2
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, SortOrder = 1, IsAscending = true, FieldSequence = ++seqNum }); // Sort 2
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "CreationDate", FieldDisplayName = "Created On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "CloseDate", FieldDisplayName = "Closed On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "StageActionUsersUser", FieldDisplayName = "Waiting On", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "fieldtype=multiuser", DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "IsPrivate", FieldDisplayName = "IsPrivate", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "PMMIdLookup", FieldDisplayName = "PMM ID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "ProjectInitiativeLookup", FieldDisplayName = "Project Initiative", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "SponsorsUser", FieldDisplayName = "Sponsors", ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "NPR", FieldName = "StakeHoldersUser", FieldDisplayName = "Stakeholders", ColumnType = "MultiUser", IsUseInWildCard = true, FieldSequence = ++seqNum });
            return mList;
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");
            mList.Add(new ModuleFormTab() { TabName = "General", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General" });
            mList.Add(new ModuleFormTab() { TabName = "PMO Assessment", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "PMO Assessment" });
            mList.Add(new ModuleFormTab() { TabName = "Planning", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Planning" });
            mList.Add(new ModuleFormTab() { TabName = "Schedule", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Schedule" });
            mList.Add(new ModuleFormTab() { TabName = "Resource Sheet", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Resource Sheet" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 8, TabSequence = 8, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            mList.Add(new ModuleImpact() { ModuleNameLookup = "NPR", Title = "NPR-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "NPR", Title = "NPR-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            mList.Add(new ModuleImpact() { ModuleNameLookup = "NPR", Title = "NPR-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            return mList;
        }

        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4 });
            return mList;
        }
        public List<ModuleSeverity> GetModuleSeverity()
        {
            // Console.WriteLine(" Severity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "NPR", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "NPR", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            mList.Add(new ModuleSeverity() { ModuleNameLookup = "NPR", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });
            return mList;
        }
        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            // Console.WriteLine("RequestPriority");
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 18, SeverityLookup = 16, PriorityLookup = 16 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 18, SeverityLookup = 17, PriorityLookup = 16 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 18, SeverityLookup = 18, PriorityLookup = 17 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 19, SeverityLookup = 16, PriorityLookup = 16 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 19, SeverityLookup = 17, PriorityLookup = 17 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 19, SeverityLookup = 18, PriorityLookup = 18 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 20, SeverityLookup = 16, PriorityLookup = 17 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 20, SeverityLookup = 17, PriorityLookup = 18 });
            mList.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 20, SeverityLookup = 18, PriorityLookup = 18 });
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "NPR", Title = "NPR-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "NPR", Title = "NPR-Business Manager", UserTypes = "Business Manager", ColumnName = "BusinessManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "NPR", Title = "NPR-Sponsors", UserTypes = "Sponsor", ColumnName = "SponsorsUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "NPR", Title = "NPR-Stake Holders", UserTypes = "Stake Holder", ColumnName = "StakeHoldersUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "NPR", Title = "NPR-Project Director", UserTypes = "Project Director", ColumnName = "ProjectDirector", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "NPR", Title = "NPR-Project Manager", UserTypes = "Project Manager", ColumnName = "ProjectManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "NPR", Title = "NPR-Analyst", UserTypes = "Analyst", ColumnName = "AssignedAnalyst", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }



        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  StatusMapping");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "New Project Request", ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1 });
            if (enableManagerApprovalStage)
                mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Manager Approval", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "PMO Assessment", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "IT Governance Review", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "IT Steering Committee Review", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Approved", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + "-" + "Closed", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 4 });

            ModuleStatusMapping mStatusMapping = new ModuleStatusMapping();
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            int startStageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            int pmoStageID = startStageID + 2;
            if (!enableManagerApprovalStage)
                pmoStageID--;

            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Requestor",
                KeyName = "Requestor",
                KeyValue = "LoggedInUser",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "BusinessManager",
                KeyName = "BusinessManager",
                KeyValue = "LoggedInUserManagerIfNotManager",
                ModuleStepLookup = startStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "IsITGApprovalRequired",
                KeyName = "IsITGApprovalRequired",
                KeyValue = "1",
                ModuleStepLookup = pmoStageID.ToString(),
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "IsSteeringApprovalRequired",
                KeyName = "IsSteeringApprovalRequired",
                KeyValue = "1",
                ModuleStepLookup = pmoStageID.ToString(),
                CustomProperties = ""
            });

            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");


            // Tab 1
            int seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiator", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InitiatorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiated Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CreationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Target Completion", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Business Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Sponsor(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SponsorsUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Stake Holder(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "StakeHoldersUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiative", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectInitiativeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Rank", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectRank", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Primary Beneficiaries", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Functional Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Locations Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "LocationLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Affected", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "APPTitleLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Module", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ModuleNameLookup = ModuleName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Supporting Materials", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Problem Being Solved", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProblemBeingSolved", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Solution / Benefits", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectBenefits", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Scope", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectScope", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Assumptions", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectAssumptions", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Risks", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "ProjectRiskNotes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Supporting Materials", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 2
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Class", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ProjectClassLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Replacement?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ClassificationTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Platform", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ClassificationChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Size Class", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ClassificationSizeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "IT Governance Review Required?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "IsITGApprovalRequired", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Steering Committee Approval Required?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "IsSteeringApprovalRequired", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Notes", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "ClassificationNotes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Analysis", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Scope", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ClassificationScope", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Complexity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ProjectComplexityChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Organizational Impact", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "OrganizationalImpactChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Usability", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "TechnologyUsabilityChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Reliability", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "TechnologyReliability", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Security", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "TechnologySecurity", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Internal Capability", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "InternalCapabilityChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Vendor Support", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "VendorSupportChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Adoption Risk", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "AdoptionRiskChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impact Notes", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "TechnologyImpact", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Analysis", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Estimate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Estimated Min Duration (Days)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ProjectEstDurationMinDays", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Estimated Max Duration (Days)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ProjectEstDurationMaxDays", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Planned Duration (Days)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "Duration", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Estimated Min Size (Hours)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ProjectEstSizeMinHrs", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Estimated Max Size (Hours)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ProjectEstSizeMaxHrs", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Planned Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "EstimatedHours", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Estimate", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "How Do You Justify the Expenditures?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Revenue Increase", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ImpactRevenueIncreaseChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Business Growth", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ImpactBusinessGrowth", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Reduces Risk", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ImpactReducesRiskChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
                       
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Contribution To Strategy", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ContributionToStrategyChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Payback Period / Cost Savings", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PaybackCostSavingsChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Customer Benefit", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "CustomerBenefitChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Productivity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ImpactIncreasesProductivityChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Regulatory / Compliance", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "RegulatoryChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "IT Lifecycle Refresh (HW/SW)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ITLifecycleRefreshChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
                        
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Reduces Expenses", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ImpactReducesExpensesChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Improves Decision Making", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ImpactDecisionMakingChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Break Even In (months)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "BreakEvenIn", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Eliminates Head Count (#)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "EliminatesHeadcount", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ROI (%)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ROI", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Other Describe", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "OtherDescribe", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "How Do You Justify the Expenditures?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Budget Approved?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Approved Amount ($)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ApprovedRFEAmount", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "GL Code", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ApprovedRFE", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Budget Type", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ApprovedRFEType", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Budget Approved?", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 3: Planning   
            seqNum = 0;
            // Note: control already has group/fieldsets built-in
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "NPRBudget", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = (++seqNum) });

            //Tab 4
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Tasks", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "TasksList", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Tasks", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 5
            seqNum = 0;
            // Resource Sheet 
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resource", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ProjectResourceDetail", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resource", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 6:
            seqNum = 0;
            // Lifecycle 
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Lifecycle", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Stage Exit Criteria
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Stage Exit Criteria", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "PreconditionList", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Stage Exit Criteria", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 7
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 7, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 8
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 8, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

            // New  - Tab 0
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Business Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BusinessManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Target Completion", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Sponsor(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SponsorsUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Stake Holder(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "StakeHoldersUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiative", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ProjectInitiativeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Rank", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ProjectRank", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Primary Beneficiaries", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Functional Area", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Locations Affected", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "LocationLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Affected", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "APPTitleLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Module", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ModuleNameLookup = ModuleName", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Supporting Materials", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Problem Being Solved", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "ProblemBeingSolved", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Solution / Benefits", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "ProjectBenefits", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Scope", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "ProjectScope", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Assumptions", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "ProjectAssumptions", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Project Risks", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "ProjectRiskNotes", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Supporting Materials", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleTaskEmail()
            {
                Status = "New Project Request",
                EmailTitle = "NPR Request Returned [$TicketId$]: [$Title$]",
                EmailBody = @"NPR Request TicketID [$TicketId$] has been returned for clarification.<br><br>" +
                                                         "Please enter the required information and re-submit the .",
                ModuleNameLookup = ModuleName,
                StageStep = stageID
            });
            if (enableManagerApprovalStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Title = ModuleName + "-" + "Manager Approval",
                    Status = "Manager Approval",
                    EmailTitle = "NPR Request Pending Manager Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"NPR Request TicketID [$TicketId$] is pending Manager Approval.<br><br>" +
                                                         "Please review the request and approve or reject it.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID)
                });
            }

            mList.Add(new ModuleTaskEmail()

            {
                Title = ModuleName + "-" + "IT PMO Assessment",
                Status = "IT PMO Assessment",
                EmailTitle = "NPR Request Pending PMO Assessment [$TicketId$]: [$Title$] ",
                EmailBody = @"NPR Request TicketID [$TicketId$] is pending PMO Assessment.<br><br>" +
                                                         "Please assess the request, create the project plan and budget and approve or reject it.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()

            {
                Title = ModuleName + "-" + "IT Governance Review",
                Status = "IT Governance Review",
                EmailTitle = "NPR Request Pending IT Governance Review [$TicketId$]: [$Title$]",
                EmailBody = @"NPR Request TicketID [$TicketId$] is pending IT Governance Review.<br><br>" +
                                                      "Please review the request and approve or reject it.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + "-" + "IT Steering Committee Review",
                Status = "IT Steering Committee Review",
                EmailTitle = "NPR Request Pending IT Steering Committee Review [$TicketId$]: [$Title$]",
                EmailBody = @"NPR Request TicketID [$TicketId$] is pending IT Steering Committee Review.<br><br>" +
                                                         "Please review the request and approve or reject it.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + "-" + "Approved", Status = "Approved", EmailTitle = "NPR Request Approved [$TicketId$]: [$Title$]", EmailBody = @"NPR Request TicketID [$TicketId$] has been approved.", ModuleNameLookup = ModuleName, StageStep = (++stageID) });
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + "-" + "Closed", Status = "Closed", EmailTitle = "NPR Request Closed [$TicketId$]: [$Title$]", EmailBody = "NPR Request TicketID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (++stageID) });

            mList.Add(new ModuleTaskEmail() { Title = ModuleName + "-" + "On-Hold", Status = "On-Hold", EmailTitle = "NPR  On Hold [$Id$]: [$Title$]", EmailBody = "NPR  TicketID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null });
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int moduleStep = 0;

            //Editable in all stages
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false, });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsPrivate", FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            //dataList.Add(new ModuleRoleWriteAccess() { "Comment", ModuleNameLookup = ModuleName, "0", "0", "0", "0", "0", "" });

            // Stage 1 - Initiation
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsProjectBudgeted", FieldName = "IsProjectBudgeted", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Beneficiaries", FieldName = "BeneficiariesLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Sponsors", FieldName = "SponsorsUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "BusinessManager", FieldName = "BusinessManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "StakeHolders", FieldName = "StakeHoldersUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImprovesOperationalEfficiency", FieldName = "ImprovesOperationalEfficiency", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ReducesCost", FieldName = "ReducesCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImprovesRevenues", FieldName = "ImprovesRevenues", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoAlternative", FieldName = "NoAlternative", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoAlternativeOtherDescribe", FieldName = "NoAlternativeOtherDescribe", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "BreakEvenIn", FieldName = "BreakEvenIn", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "EliminatesHeadcount", FieldName = "EliminatesHeadcount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ROI", FieldName = "ROI", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "OtherDescribe", FieldName = "OtherDescribe", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "CapitalExpenditure", FieldName = "CapitalExpenditure", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "CapitalExpenditureNotes", FieldName = "CapitalExpenditureNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LocationLookup", FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "APPTitleLookup", FieldName = "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "CapitalExpenditureNotes", FieldName = "CapitalExpenditureNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProblemBeingSolved", FieldName = "ProblemBeingSolved", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectScope", FieldName = "ProjectScope", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectAssumptions", FieldName = "ProjectAssumptions", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectBenefits", FieldName = "ProjectBenefits", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRiskNotes", FieldName = "ProjectRiskNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalAreaLookup", FieldName = "FunctionalAreaLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectClassLookup", FieldName = "ProjectClassLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectInitiativeLookup", FieldName = "ProjectInitiativeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank", FieldName = "ProjectRank", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank2", FieldName = "ProjectRank2", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank3", FieldName = "ProjectRank3", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "RapidRequest", FieldName = "RapidRequest", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Classification", FieldName = "ClassificationChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ClassificationType", FieldName = "ClassificationTypeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            // Stage 2 - Manager Approval     
            if (enableManagerApprovalStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProblemBeingSolved", FieldName = "ProblemBeingSolved", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectScope", FieldName = "ProjectScope", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectAssumptions", FieldName = "ProjectAssumptions", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectBenefits", FieldName = "ProjectBenefits", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRiskNotes", FieldName = "ProjectRiskNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank", FieldName = "ProjectRank", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank2", FieldName = "ProjectRank2", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank3", FieldName = "ProjectRank3", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Beneficiaries", FieldName = "BeneficiariesLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Sponsors", FieldName = "SponsorsUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "StakeHolders", FieldName = "StakeHoldersUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalAreaLookup", FieldName = "FunctionalAreaLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalAreaLookup", FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalAreaLookup", FieldName = "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalAreaLookup", FieldName = "ModuleNameLookup = ModuleName", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });

                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImprovesOperationalEfficiency", FieldName = "ImprovesOperationalEfficiency", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ReducesCost", FieldName = "ReducesCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImprovesRevenues", FieldName = "ImprovesRevenues", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoAlternative", FieldName = "NoAlternative", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoAlternativeOtherDescribe", FieldName = "NoAlternativeOtherDescribe", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "BreakEvenIn", FieldName = "BreakEvenIn", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "EliminatesHeadcount", FieldName = "EliminatesHeadcount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ROI", FieldName = "ROI", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "OtherDescribe", FieldName = "OtherDescribe", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = true, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "CapitalExpenditure", FieldName = "CapitalExpenditure", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "CapitalExpenditureNotes", FieldName = "CapitalExpenditureNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
                mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            }

            // Stage 3 - PMO Approval
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProblemBeingSolved", FieldName = "ProblemBeingSolved", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectScope", FieldName = "ProjectScope", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectAssumptions", FieldName = "ProjectAssumptions", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectBenefits", FieldName = "ProjectBenefits", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRiskNotes", FieldName = "ProjectRiskNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Beneficiaries", FieldName = "BeneficiariesLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Sponsors", FieldName = "SponsorsUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "StakeHolders", FieldName = "StakeHoldersUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalAreaLookup", FieldName = "FunctionalAreaLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectClassLookup", FieldName = "ProjectClassLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank", FieldName = "ProjectRank", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank2", FieldName = "ProjectRank2", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank3", FieldName = "ProjectRank3", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LocationLookup", FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "APPTitleLookup", FieldName = "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Classification", FieldName = "ClassificationTypeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "ClassificationChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ClassificationSize", FieldName = "ClassificationSizeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ClassificationScope", FieldName = "ClassificationScope", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ClassificationImpact", FieldName = "ClassificationImpact", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ClassificationNotes", FieldName = "ClassificationNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologyUsability", FieldName = "TechnologyUsabilityChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologyAvailability", FieldName = "TechnologyReliability", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologyAvailability", FieldName = "TechnologyAvailability", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologyIntegration", FieldName = "TechnologyIntegration", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologySecurity", FieldName = "TechnologySecurity", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologyRisk", FieldName = "TechnologyRisk", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologyImpact", FieldName = "TechnologyImpact", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfReports", FieldName = "NoOfReports", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfReportsNotes", FieldName = "NoOfReportsNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfScreens", FieldName = "NoOfScreens", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfScreensNotes", FieldName = "NoOfScreensNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Complexity", FieldName = "Complexity", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ComplexityNotes", FieldName = "ComplexityNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Technology", FieldName = "Technology", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TechnologyNotes1", FieldName = "TechnologyNotes1", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "MetricsNotes", FieldName = "MetricsNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImprovesOperationalEfficiency", FieldName = "ImprovesOperationalEfficiency", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ReducesCost", FieldName = "ReducesCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImprovesRevenues", FieldName = "ImprovesRevenues", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoAlternative", FieldName = "NoAlternative", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoAlternativeOtherDescribe", FieldName = "NoAlternativeOtherDescribe", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "BreakEvenIn", FieldName = "BreakEvenIn", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsITGApprovalRequired", FieldName = "IsITGApprovalRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsSteeringApprovalRequired", FieldName = "IsSteeringApprovalRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "EliminatesHeadcount", FieldName = "EliminatesHeadcount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ROI", FieldName = "ROI", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "OtherDescribe", FieldName = "OtherDescribe", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AnalyticsPriority", FieldName = "AnalyticsPriority", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AnalyticsRisk", FieldName = "AnalyticsRisk", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AnalyticsCost", FieldName = "AnalyticsCost", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AnalyticsSchedule", FieldName = "AnalyticsSchedule", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AnalyticsROI", FieldName = "AnalyticsROI", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AnalyticsArchitecture", FieldName = "AnalyticsArchitecture", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRisk", FieldName = "ProjectRisk", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFE", FieldName = "ApprovedRFE", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEAmount", FieldName = "ApprovedRFEAmount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEType", FieldName = "ApprovedRFEType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfFTEs", FieldName = "NoOfFTEs", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfFTEsNotes", FieldName = "NoOfFTEsNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfConsultants", FieldName = "NoOfConsultants", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "NoOfConsultantsNotes", FieldName = "NoOfConsultantsNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "CannotStartBefore", FieldName = "CannotStartBefore", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "CannotStartBeforeNotes", FieldName = "CannotStartBeforeNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDateNotes", FieldName = "DesiredCompletionDateNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ConstraintNotes", FieldName = "ConstraintNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactRevenueIncrease", FieldName = "ImpactRevenueIncreaseChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactBusinessGrowth", FieldName = "ImpactBusinessGrowth", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactReducesRisk", FieldName = "ImpactReducesRiskChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactIncreasesProductivity", FieldName = "ImpactIncreasesProductivityChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactReducesExpenses", FieldName = "ImpactReducesExpensesChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactDecisionMaking", FieldName = "ImpactDecisionMakingChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectComplexity", FieldName = "ProjectComplexityChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ScheduleComplexity", FieldName = "ScheduleComplexity", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "InternalCapability", FieldName = "InternalCapabilityChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "OrganizationalImpact", FieldName = "OrganizationalImpactChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "VendorSupport", FieldName = "VendorSupportChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AdoptionRisk", FieldName = "AdoptionRiskChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Contribution To Strategy", FieldName = "ContributionToStrategyChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Payback Cost Savings", FieldName = "PaybackCostSavingsChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Customer Benefit", FieldName = "CustomerBenefitChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Regulatory", FieldName = "RegulatoryChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IT Lifecycle Refresh", FieldName = "ITLifecycleRefreshChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstSizeMinHrs", FieldName = "ProjectEstSizeMinHrs", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstSizeMaxHrs", FieldName = "ProjectEstSizeMaxHrs", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstDurationMinDays", FieldName = "ProjectEstDurationMinDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstDurationMaxDays", FieldName = "ProjectEstDurationMaxDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            // Stage 4 - IT Governance approval
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProblemBeingSolved", FieldName = "ProblemBeingSolved", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectScope", FieldName = "ProjectScope", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectAssumptions", FieldName = "ProjectAssumptions", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectBenefits", FieldName = "ProjectBenefits", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRiskNotes", FieldName = "ProjectRiskNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Beneficiaries", FieldName = "BeneficiariesLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Sponsors", FieldName = "SponsorsUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "StakeHolders", FieldName = "StakeHoldersUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FunctionalAreaLookup", FieldName = "FunctionalAreaLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectClassLookup", FieldName = "ProjectClassLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank", FieldName = "ProjectRank", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsSteeringApprovalRequired", FieldName = "IsSteeringApprovalRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LocationLookup", FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "APPTitleLookup", FieldName = "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsSteeringApprovalRequired", FieldName = "IsSteeringApprovalRequired", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFE", FieldName = "ApprovedRFE", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEAmount", FieldName = "ApprovedRFEAmount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEType", FieldName = "ApprovedRFEType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactRevenueIncrease", FieldName = "ImpactRevenueIncreaseChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactBusinessGrowth", FieldName = "ImpactBusinessGrowth", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactReducesRisk", FieldName = "ImpactReducesRiskChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactIncreasesProductivity", FieldName = "ImpactIncreasesProductivityChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactReducesExpenses", FieldName = "ImpactReducesExpensesChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ImpactDecisionMaking", FieldName = "ImpactDecisionMakingChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectComplexity", FieldName = "ProjectComplexityChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ScheduleComplexity", FieldName = "ScheduleComplexity", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "InternalCapability", FieldName = "InternalCapabilityChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "OrganizationalImpact", FieldName = "OrganizationalImpactChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "VendorSupport", FieldName = "VendorSupportChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AdoptionRisk", FieldName = "AdoptionRiskChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstSizeMinHrs", FieldName = "ProjectEstSizeMinHrs", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowWithCheckbox = false, HideInServiceMapping = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstSizeMaxHrs", FieldName = "ProjectEstSizeMaxHrs", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowWithCheckbox = false, HideInServiceMapping = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstDurationMinDays", FieldName = "ProjectEstDurationMinDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowWithCheckbox = false, HideInServiceMapping = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectEstDurationMaxDays", FieldName = "ProjectEstDurationMaxDays", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowWithCheckbox = false, HideInServiceMapping = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            // Stage 5 - IT Steering Committee approval
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProblemBeingSolved", FieldName = "ProblemBeingSolved", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectScope", FieldName = "ProjectScope", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectAssumptions", FieldName = "ProjectAssumptions", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectBenefits", FieldName = "ProjectBenefits", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRiskNotes", FieldName = "ProjectRiskNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Beneficiaries", FieldName = "BeneficiariesLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Sponsors", FieldName = "SponsorsUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "StakeHolders", FieldName = "StakeHoldersUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectRank", FieldName = "ProjectRank", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LocationLookup", FieldName = "LocationLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "APPTitleLookup", FieldName = "APPTitleLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = true });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TargetStartDate", FieldName = "TargetStartDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TargetCompletionDate", FieldName = "TargetCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFE", FieldName = "ApprovedRFE", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEAmount", FieldName = "ApprovedRFEAmount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEType", FieldName = "ApprovedRFEType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Requestor", FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            // Stage 6 - Approved
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TotalCostsNotes", FieldName = "TotalCostsNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TotalStaffHeadcountNotes", FieldName = "TotalStaffHeadcountNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TotalOffSiteConsultantHeadcountNotes", FieldName = "TotalOffSiteConsultantHeadcountNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "TotalOnSiteConsultantHeadcountNotes", FieldName = "TotalOnSiteConsultantHeadcountNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "RiskScoreNotes", FieldName = "RiskScoreNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ArchitectureScoreNotes", FieldName = "ArchitectureScoreNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ProjectScoreNotes", FieldName = "ProjectScoreNotes", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFE", FieldName = "ApprovedRFE", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEAmount", FieldName = "ApprovedRFEAmount", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "ApprovedRFEType", FieldName = "ApprovedRFEType", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, HideInServiceMapping = false });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            /*
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "ERP", RequestType = "ERP", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "CRM", RequestType = "CRM", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "Custom Apps", RequestType = "Custom Apps", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "Collaboration", RequestType = "Collaboration", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "BI/Analytical", RequestType = "BI/Analytical", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Business Applications" + ">" + "Digital Transformation", RequestType = "Digital Transformation", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Business Applications", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });

            //Fast Track
            mList.Add(new ModuleRequestType() { Title = "Fast-Track" + ">" + "CMM", RequestType = "CMM", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Fast-Track", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Fast-Track" + ">" + "Go Direct to PMM", RequestType = "Go Direct to PMM", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Fast-Track", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
             //Infrastructure
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Data Center", RequestType = "Data Center", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Office Management", RequestType = "Office Management", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "User Hardware", RequestType = "User Hardware", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Telecommunications", RequestType = "Telecommunications", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Infrastructure" + ">" + "Other Infrastructure", RequestType = "Other Infrastructure", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Infrastructure", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });

            //other
            mList.Add(new ModuleRequestType() { Title = "Other" + ">" + "Other", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other", FunctionalAreaLookup = 1, WorkflowType = "Full", IsDeleted = false, Owner = ConfigData.Variables.Name });

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
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Innovation", "AI/ML"), Category = "Innovation", RequestType = "AI/ML", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 2 });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Innovation", "AR/VR/MR"), Category = "Innovation", RequestType = "AR/VR/MR", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 1 });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Regulatory/Compliance", "Other Manadatory"), Category = "Regulatory/Compliance", RequestType = "Other Manadatory", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Regulatory/Compliance", "Sox Compliance"), Category = "Regulatory/Compliance", RequestType = "Sox Compliance", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO" });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Services", "Cloud Migration"), Category = "Services", RequestType = "Cloud Migration", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 3 });
            mList.Add(new ModuleRequestType() { Title = string.Format("{0} > {1}", "Services", "Digital Transformation"), Category = "Services", RequestType = "Digital Transformation", ModuleNameLookup = ModuleName, WorkflowType = "Full", Owner = "PMO", EstimatedHours = 4 });

            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");
            // Start from ID - 37
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 7;
            int moduleStep = 0;
            if (!enableManagerApprovalStage)
                numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Name = "New Project Request",
                StageTitle = "Account Closure",
                UserWorkflowStatus = "Account Closure",
                UserPrompt = "<b>Please fill the form to open a New Project Request.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "New Request",
                ActionUser = "InitiatorUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "New Request Created",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Submit",
                StageRejectedButtonName = "Cancel Request",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            // Not a stable stage :)
            mList.Add(new LifeCycleStage()
            {
                Name = "Manager Approval",
                StageTitle = "Manager Approval",
                UserWorkflowStatus = "Awaiting Business Manager Approval",
                UserPrompt = "<b>Business Manager:</b>Please review and classify project",               
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Manager Approval",
                ActionUser = "BusinessManagerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Business Manager approved request",
                RejectActionDescription = "Business Manager Rejected Project Request (Closed)",
                ReturnActionDescription = "Business Manager returned to initiator",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject & Close",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "AllowEmailApproval=true",
                StageTypeChoice = ""
            });

            nprPMOStageID = currStageID;
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, nprPMOStageID);

            mList.Add(new LifeCycleStage()
            {
                Name = "IT PMO Assessment",
                StageTitle = "IT PMO Assessment",
                UserWorkflowStatus = "Awaiting PMO Assessment",
                UserPrompt = "<b>PMO:</b> Please approve the project request",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "IT PMO Assessment",
                ActionUser = "PMO",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PMO completed assessment",
                RejectActionDescription = "",
                ReturnActionDescription = "PMO returned to initiator",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject & Close",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "PMOReview=true;#autoapprove=false",
                StageTypeChoice = ""
            });

            nprITGovernanceStageID = currStageID;

            mList.Add(new LifeCycleStage()
            {
                Name = "IT Governance Review",
                StageTitle = "IT Governance Review",
                UserWorkflowStatus = "Awaiting IT Governance Review",
                UserPrompt = "<b>ITG:</b> Please approve the project request",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "IT Governance Approval",
                ActionUser = "IT Governance",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = nprPMOStageID,
                ApproveActionDescription = "IT Governance committee approved request",
                RejectActionDescription = "IT Governance committee rejected request",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject & Close",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "[IsITGApprovalRequired] = 'False'",
                CustomProperties = "ITGReview=true;#autoapprove=false",
                StageTypeChoice = ""
            });

            nprITSteeringeStageID = currStageID;
            mList.Add(new LifeCycleStage()
            {
                Name = "IT Steering Committee Review",
                StageTitle = "IT Steering Committee Review",
                UserWorkflowStatus = "Awaiting IT Steering Committee Review",
                UserPrompt = "<b>IT Steering Committee:</b> Please approve the project request.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "IT Steering Committee Approval",
                ActionUser = "IT Steering Committee",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = nprITGovernanceStageID,
                ApproveActionDescription = "IT Governance committee approved request",
                RejectActionDescription = "IT Governance committee rejected request",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject & Close",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "[Steering Committee Approval Required] = 'False'",
                CustomProperties = "ITSCReview=true;#autoapprove=false",
                StageTypeChoice = ""
            });

            nprApprovedStageID = currStageID;
            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            moduleTestedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            mList.Add(new LifeCycleStage()
            {
                Name = "Approved",
                StageTitle = "Approved",
                UserWorkflowStatus = "Approved to start the project",
                UserPrompt = "<b>Initiator:</b> Please update the Finalize Project tab and import the project into PMM",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Closed",
                ActionUser = "InitiatorUser;#PMO",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Closed",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Closeout Project",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "autoapprove=false;#readytoimport=true",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Name = "Closed",
                StageTitle = "Closed",
                UserWorkflowStatus = "Request is closed, but you can add comments or can be re-opened",
                UserPrompt = "Closed",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Closed",
                ActionUser = "InitiatorUser;#PMO",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "Request re-opened",
                StageApproveButtonName = "Closeout Project",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageAllApprovalsRequired = false,
                StageWeight = 0,
                SkipOnCondition = "",
                CustomProperties = "",
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
            return mList;
        }

        public List<DRQSystemArea> GetDRQSystemAreas()
        {
            List<DRQSystemArea> mList = new List<DRQSystemArea>();
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Unassigned", ViewName = "NPRRequests", TabOrder = 1, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "NPRRequests", TabOrder = 2, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Projects", ViewName = "NPRRequests", TabOrder = 3, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true,  TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Projects", ViewName = "NPRRequests", TabOrder = 4, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Projects", ViewName = "NPRRequests", TabOrder = 5, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Projects", ViewName = "NPRRequests", TabOrder = 6, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Opem Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Projects", ViewName = "NPRRequests", TabOrder = 7, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Projects", ViewName = "NPRRequests", TabOrder = 8, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Projects", ViewName = "NPRRequests", TabOrder = 9, ModuleNameLookup = "NPR", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "NPRRequests", TabOrder = 10, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            return tabs;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "NPR", Title = "NPR-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "NPR", Title = "NPR-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "NPR", Title = "NPR-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "NPR", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "NPR", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "NPR", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });

            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "NPR", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4, IsVIP = true });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 1, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 2, SeverityLookup = 0, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "NPR", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });


        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
    }
}
