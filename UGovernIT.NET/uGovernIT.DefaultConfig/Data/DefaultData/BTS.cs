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
    public class BTS : IModule
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
        protected string moduleId = "5";
        protected static bool enablePendingCloseStage = true;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Bug Tracking",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "BTS",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/btstickets",
                    ModuleHoldMaxStage = 4,
                    Title = "Bug Tracking System (BTS)",
                    //ModuleDescription = "This module is used to report and track bugs.",
                    ModuleDescription = "Click 'New' to create a new ticket. After ticket is created, click row on ticket grid to view details. Or, select a ticket by clicking checkbox and clicking Actions button.",                  
                    ThemeColor = "Accent6",
                    StaticModulePagePath = "/Pages/bts",
                    ModuleType = ModuleType.SMS,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = false,
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
                return "BTS";
            }
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "RequestorUser", FieldDisplayName = "Requested By", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum, ColumnType = "ProgressBar" });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "RequestTypeLookup", FieldDisplayName = "Application", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "CategoryChoice", FieldDisplayName = "Issue Type", IsDisplay = true, DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "Age", FieldDisplayName = "Age", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, SortOrder = 1, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "CreationDate", FieldDisplayName = "Created On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "CloseDate", FieldDisplayName = "Closed On", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "InitiatorUser", FieldDisplayName = "Initiator", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "OwnerUser", FieldDisplayName = "Owner", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "StageActionUsersUser", FieldDisplayName = "Waiting On", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum, ColumnType = "MultiUser", CustomProperties = "fieldtype=multiuser" });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", IsUseInWildCard = true, FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "PRPUser", FieldDisplayName = "PRP", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "ORPUser", FieldDisplayName = "ORP", FieldSequence = ++seqNum, IsUseInWildCard = true, ColumnType = "MultiUser" });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "ID", FieldDisplayName = "DBID", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "OnHold", FieldDisplayName = "OnHold", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "BTS", FieldName = "TesterUser", FieldDisplayName = "Tester", IsUseInWildCard = true, FieldSequence = ++seqNum });
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

            return mList;
        }

        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("TaskEmails");
            return mList;
        }
        public List<ModuleSeverity> GetModuleSeverity()
        {
            // Console.WriteLine(" TicketSeverity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "BTS", Title = "BTS-Initiator", UserTypes = "InitiatorUser", ColumnName = "Initiator", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "BTS", Title = "BTS-Requestor", UserTypes = "RequestorUser", ColumnName = "Requestor", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "BTS", Title = "BTS-Owner", UserTypes = "OwnerUser", ColumnName = "Owner", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "BTS", Title = "BTS-PRP", UserTypes = "PRPUser", ColumnName = "PRP", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "BTS", Title = "BTS-ORP", UserTypes = "ORPUser", ColumnName = "ORP", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "BTS", Title = "BTS-Tester", UserTypes = "TesterUser", ColumnName = "Tester", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }


        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            // Console.WriteLine("RequestPriority");
            return mList;
        }

        public List<UGITModule> GetUGITModule()
        {
            throw new NotImplementedException();
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");

            List<string[]> dataList = new List<string[]>();
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            dataList.Add(new string[] { "Initiated", ModuleName, stageID.ToString(), "1" });
            dataList.Add(new string[] { "Pending Assignment", ModuleName, (++stageID).ToString(), "1" });
            dataList.Add(new string[] { "Assigned", ModuleName, (++stageID).ToString(), "2" });
            dataList.Add(new string[] { "Testing", ModuleName, (++stageID).ToString(), "2" });
            if (enablePendingCloseStage)
                dataList.Add(new string[] { "Pending Close", ModuleName, (++stageID).ToString(), "2" });
            dataList.Add(new string[] { "Closed", ModuleName, (++stageID).ToString(), "4" });

            foreach (string[] data in dataList)
            {
                ModuleStatusMapping mStatusMapping = new ModuleStatusMapping();
                mStatusMapping.Title = data[1] + " - " + data[0];
                mStatusMapping.ModuleNameLookup = data[1];
                mStatusMapping.StageTitleLookup = Convert.ToInt32(data[2]);
                mStatusMapping.GenericStatusLookup = Convert.ToInt32(data[3]);
                mList.Add(mStatusMapping);
            }
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();

            // Console.WriteLine("  ModuleDefaultValues");

            List<string[]> dataList = new List<string[]>();
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Requestor",
                KeyName = "Requestor",
                KeyValue = "LoggedInUser",
                ModuleStepLookup = "31",
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "ImpactLookup",
                KeyName = "ImpactLookup",
                KeyValue = "15",
                ModuleStepLookup = "31",
                CustomProperties = ""

            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "SeverityLookup",
                KeyName = "SeverityLookup",
                KeyValue = "15",
                ModuleStepLookup = "31",
                CustomProperties = ""
            });

            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");

            int seqNum = 0;

            // Tab 1: General
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Status", Title = "Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiator", Title = "Initiator", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "InitiatorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiated Date", Title = "Initiated Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CreationDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Desired Completion Date", Title = "Desired Completion Date", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", Title = "Requested By", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Functional Area", Title = "Functional Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", Title = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Name", Title = "Application Name", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", Title = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Issue Type", Title = "Issue Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "CategoryChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Area (if any)", Title = "Application Area (if any)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ApplicationModule", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Sample Record ID(s)", Title = "Sample Record ID(s)", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "RelatedRecords", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impact", Title = "Impact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Severity", Title = "Severity", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "SeverityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Priority", Title = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", Title = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 2: Work Activity
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Assignment", Title = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Primary Responsible Person (PRP)", Title = "Primary Responsible Person (PRP)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "PRPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Other Responsible (ORPs)", Title = "Other Responsible (ORPs)", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ORPUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Estimated Hours", Title = "Estimated Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "EstimatedHours", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Assignment", Title = "Assignment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments / Work Activity", Title = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Add Comment", Title = "Add Comment", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments / Work Activity", Title = "Comments / Work Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution", Title = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Actual Hours", Title = "Actual Hours", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, FieldName = "ActualHours", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution Type", Title = "Resolution Type", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 2, FieldName = "ResolutionTypeChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution Description", Title = "Resolution Description", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "ResolutionComments", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Tester", Title = "Tester", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "TesterUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Resolution", Title = "Resolution", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 3: Approvals 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Approvals", Title = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "ApprovalTab", Title = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Approvals", Title = "Approvals", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 4: Emails
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", Title = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", Title = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Emails", Title = "Emails", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = ++seqNum });

            // Tab 5: Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", Title = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "CustomTicketRelationship", Title = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", Title = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", Title = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "WikiRelatedTickets", Title = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", Title = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            // Tab 6-History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum });

            seqNum = 0;
            // New Request form
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Requested By", Title = "Requested By", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RequestorUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Desired Completion Date", Title = "Desired Completion Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "DesiredCompletionDate", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", Title = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Name", Title = "Application Name", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", Title = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Issue Type", Title = "Issue Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "CategoryChoice", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Application Area (if any)", Title = "Application Area (if any)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ApplicationModule", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Sample Record ID(s)", Title = "Sample Record ID(s)", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "RelatedRecords", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Impact", Title = "Impact", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ImpactLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Severity", Title = "Severity", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "SeverityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Priority", Title = "Priority", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Classification", Title = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", Title = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum });

            string currTab = string.Empty;

            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");

            List<string[]> dataList = new List<string[]>();
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Initiated",
                EmailTitle = "BTS Ticket Returned [$TicketId$]: [$Title$]",
                EmailBody = @"BTS Ticket ID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID,
                EmailUserTypes = "Initiator",
                Title = "Initiated" + ModuleName
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Requestor Notification",
                EmailTitle = "New BTS Ticket Created [$TicketId$]: [$Title$]",
                EmailBody = @"BTS Ticket ID [$TicketId$] has been created on your behalf.<br><br>" +
                                                        "Please review the details below. You will be notified when the ticket is resolved.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID + 1,
                EmailUserTypes = "Requestor",
                Title = "Requestor Notification" + Module
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Pending Assignment",
                EmailTitle = "New BTS Ticket Pending Assignment [$TicketId$]: [$Title$]",
                EmailBody = @"BTS Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields",
                ModuleNameLookup = ModuleName,
                StageStep = stageID + 1,
                EmailUserTypes = "Owner",
                Title = "Pending Assignment" + Module
            });

            mList.Add(new ModuleTaskEmail()
            {
                Status = "Assigned",
                EmailTitle = "BTS Ticket Assigned [$TicketId$]: [$Title$]",
                EmailBody = @"BTS Ticket ID [$TicketId$] has been assigned to you for resolution.<br><br>" +
                                                        "Once resolved, please enter the resolution description and actual hours in the ticket, and assign a tester.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID + 1,
                EmailUserTypes = "PRP;#ORP",
                Title = "Assigned" + Module
            });
            mList.Add(new ModuleTaskEmail()
            {
                Status = "Testing",
                EmailTitle = "BTS Ticket Awaiting Testing [$TicketId$]: [$Title$]",
                EmailBody = @"BTS Ticket ID [$TicketId$] has been assigned to you for testing.<br><br>" +
                                                        "Please test the resolution and approve or reject the resolution.<br><br>[$IncludeActionButtons$]",
                ModuleNameLookup = ModuleName,
                StageStep = stageID + 1,
                EmailUserTypes = "Tester",
                Title = "Testing" + Module
            });

            if (enablePendingCloseStage)
            {
                mList.Add(new ModuleTaskEmail()
                {
                    Status = "Pending Close",
                    EmailTitle = "BTS Ticket Pending Close [$TicketId$]: [$Title$]",
                    EmailBody = @"BTS Ticket ID [$TicketId$] has been resolved and is pending close.<br><br>" +
                                                        "Please review and close the ticket.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = stageID + 1,
                    EmailUserTypes = "Owner",
                    Title = "Owner" + Module
                });
            }

            mList.Add(new ModuleTaskEmail() { Status = "Closed", EmailTitle = "BTS Ticket Closed [$TicketId$]: [$Title$]", EmailBody = "BTS Ticket ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = stageID + 1, EmailUserTypes = "Owner;#Requestor", Title = "Owner;#Requestor" + Module });

            mList.Add(new ModuleTaskEmail() { Status = "On-Hold", EmailTitle = "BTS Ticket On Hold [$TicketId$]: [$Title$]", EmailBody = "BTS Ticket ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null, EmailUserTypes = "Owner;#Requestor", Title = "Owner;#Requestor" + Module });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();

            // Console.WriteLine("  RequestRoleWriteAccess");
            int moduleStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            //dataList.Add(new string[] { "TicketComment", ModuleName, moduleStep.ToString(), "0", "0", "" });

            // Stage 1 - New ticket
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestorUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "DesiredCompletionDate", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "FunctionalArea", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "HelpDeskCall", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApplicationModule", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CategoryChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApplicationName", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RelatedRecords", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });

            // Stage 2 - Pending Assignment
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "EstimatedHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ImpactLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "SeverityLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ApplicationModule", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "RelatedRecords", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "CategoryChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false });

            // Stage 3 - Assigned
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PRPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "Owner", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ORPUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = true, ActionUser = "Owner;#PRP", ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "PctComplete", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });//
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ActualHours", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionTypeChoice", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TesterUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false });

            // Stage 4 - Testing
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "TesterUser", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = true, ShowEditButton = true, ActionUser = "Owner;#PRP", ShowWithCheckbox = false });

            // Stage 5 - Pending Close
            if (enablePendingCloseStage)
            {
                moduleStep++;
                mList.Add(new ModuleRoleWriteAccess() { FieldName = "ResolutionComments", ModuleNameLookup = ModuleName, StageStep = moduleStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false });
            }

            return mList;

        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");

            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Broderick", "Other 3rd-party SW"), RequestType = "Broderick", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Data Tracker", "BI"), RequestType = "Data Tracker", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "DSRP", "Non-ERP Applications"), RequestType = "DSRP", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "E1 - EAM", "JDEdwards"), RequestType = "E1 - EAM", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "E1 - Accounts Payable", "JDEdwards"), RequestType = "E1 - Accounts Payable", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "E1 - Fixed Assets", "JDEdwards"), RequestType = "E1 - Fixed Assets", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "E1 - General Ledger", "JDEdwards"), RequestType = "E1 - General Ledger", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "E1 - Technical", "JDEdwards"), RequestType = "E1 - Technical", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "EDI", "Non-ERP Applications"), RequestType = "EDI", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Enterprise Integration", "WESB"), RequestType = "Enterprise Integration", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "WESB", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "FaxStar", "Other 3rd-party SW"), RequestType = "FaxStar", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Finance", "JDEdwards"), RequestType = "Finance", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Intranet", "Internet/Intranet"), RequestType = "Intranet", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Internet/Intranet", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Kronos", "Non-ERP Applications"), RequestType = "Kronos", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "LaserVault", "Other 3rd-party SW"), RequestType = "LaserVault", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Legacy Application", "Legacy"), RequestType = "Legacy Application", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Legacy", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Manufacturing", "JDEdwards"), RequestType = "Manufacturing", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "OBIEE", "BI"), RequestType = "OBIEE", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "ODS (Object Distribution System)", "Other 3rd-party SW"), RequestType = "ODS (Object Distribution System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "OMS (Object Mirrorring System)", "Other 3rd-party SW"), RequestType = "OMS (Object Mirrorring System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "PC Miler", "Non-ERP Applications"), RequestType = "PC Miler", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "PowerLock", "Other 3rd-party SW"), RequestType = "PowerLock", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Procurement", "JDEdwards"), RequestType = "Procurement", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "QlikView", "BI"), RequestType = "QlikView", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "BI", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "QlikView", "Non-ERP Applications"), RequestType = "STRAP", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Sales Order Entry", "JDEdwards"), RequestType = "Sales Order Entry", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Sales Order Management", "JDEdwards"), RequestType = "Sales Order Management", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Screen Manager", "Other 3rd-party SW"), RequestType = "Screen Manager", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Service Tracking System", "Other 3rd-party SW"), RequestType = "Service Tracking System", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Supply Chain", "JDEdwards"), RequestType = "Supply Chain", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Sysco Quick Review", "Other 3rd-party SW"), RequestType = "Sysco Quick Review", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Other 3rd-party SW", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "TMS (Traffic Management System)", "Non-ERP Applications"), RequestType = "TMS (Traffic Management System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "TPMS (Trade Promotion Management System)", "Non-ERP Applications"), RequestType = "TPMS (Trade Promotion Management System)", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Technical", "JDEdwards"), RequestType = "Technical", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "TerraTech", "Non-ERP Applications"), RequestType = "TerraTech", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Non-ERP Applications", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "WESB", "WESB"), RequestType = "WESB", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "WESB", WorkflowType = "Full", Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = string.Format("{1} > {0}", "Warehouse Mgmnt.", "JDEdwards"), RequestType = "Warehouse Mgmnt.", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "JDEdwards", WorkflowType = "Full", Owner = ConfigData.Variables.Name });

            return mList;

        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            List<string[]> dataList = new List<string[]>();

            // Start from ID - 31
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 6;
            int moduleStep = 0;
            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Initiated",
                UserWorkflowStatus = "Create new Ticket",
                UserPrompt = "<b>&nbsp;Please complete the form to open a new Bug Tracking System.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ActionUser = "Initiator",
                ApproveActionDescription = "Ticket Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "(Re)Submit",
                StageRejectedButtonName = "Cancel",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "QuickClose=true",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "PRP Assigned",
                Name = "Pending Assignment",
                StageTitle = "Pending Assignment",
                UserWorkflowStatus = "Awaiting Primary Responsible Person(PRP) assignment by Owner",
                UserPrompt = "<b>Owner:</b>&nbsp;Please assign a Primary Responsible Person (PRP) and the Estimated hours on work activity tab.",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "Owner;#PRPGroup",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PRP / ORP Assigned",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Assign PRP",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "SelfAssign=true;#DoNotWaitForActionUser=true",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);
            mList.Add(new LifeCycleStage()
            {
                Name = "Assigned",
                StageTitle = "Assigned",
                UserWorkflowStatus = "Awaiting resolution by Primary Responsible Person(PRP)/Other Responsible Person(ORP)",
                UserPrompt = "<b>PRP:</b>&nbsp;Please enter Resolution Description and other required fields, and assign a Tester",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Tester Assigned",
                ActionUser = "PRP;#ORP",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "PRP/ORP completed request and assigned tester",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Assign Tester",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 45,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Assigned)
            });

            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            mList.Add(new LifeCycleStage()
            {
                Name = "Testing",
                StageTitle = "Testing",
                UserWorkflowStatus = "Awaiting testing to be completed by the Tester",
                UserPrompt = "<b>Tester:</b>&nbsp;Please approve/reject the resolution after completing testing",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                Action = "Tester Complete",
                ActionUser = "Tester",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                ApproveActionDescription = "Tester approved resolution",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 30,
                SkipOnCondition = "autoapprove=false",
                CustomProperties = "[RequestTypeWorkflow] = 'NoTest' || [Tester] = 'null'  || [Tester] = ''",
                StageTypeChoice = Convert.ToString(StageType.Resolved)
            });

            moduleTestedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            if (enablePendingCloseStage)
            {

                mList.Add(new LifeCycleStage()
                {
                    Name = "Pending Close",
                    StageTitle = "Pending Close",
                    UserWorkflowStatus = "Awaiting Owner's approval",
                    UserPrompt = "<b>Owner:</b>&nbsp;Please approve the ticket.",
                    StageStep = ++moduleStep,
                    ModuleNameLookup = ModuleName,
                    Action = "Owner Closed",
                    ActionUser = "Owner",
                    StageApprovedStatus = ++currStageID,
                    StageRejectedStatus = 0,
                    StageReturnStatus = Convert.ToInt32(assignedStageID == "" ? "0" : assignedStageID),
                    ApproveActionDescription = "Owner approved resolution & closed ticket",
                    RejectActionDescription = "",
                    ReturnActionDescription = "",
                    StageApproveButtonName = "Approve",
                    StageRejectedButtonName = "Reject",
                    StageReturnButtonName = "Return",
                    StageAllApprovalsRequired = false,
                    StageWeight = 20,
                    SkipOnCondition = "",
                    CustomProperties = "",
                    StageTypeChoice = Convert.ToString(StageType.Resolved)
                });
            }

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "Ticket Closed",
                UserWorkflowStatus = "Ticket is closed,but you can add comments or can be re - opened by Owner",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "Owner;#Asset Managers",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Owner approved resolution & closed ticket",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Approve",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 5,
                StageAllApprovalsRequired = false,
                StageWeight = 0,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
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
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "BTSTickets", TabOrder = 1, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "BTSTickets", TabOrder = 2, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Items", ViewName = "BTSTickets", TabOrder = 3, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Items", ViewName = "BTSTickets", TabOrder = 4, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Items", ViewName = "BTSTickets", TabOrder = 5, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Items", ViewName = "BTSTickets", TabOrder = 6, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Items", ViewName = "BTSTickets", TabOrder = 7, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Items", ViewName = "BTSTickets", TabOrder = 8, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Closed Items", ViewName = "BTSTickets", TabOrder = 9, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "BTBTSTicketsS", TabOrder = 10, ModuleNameLookup = "BTS", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });

            return tabs;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            //Assiging value for impacts
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "BTS", Title = "BTS-Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "BTS", Title = "BTS-Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = "BTS", Title = "BTS-Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            //Assigning value for severities
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "BTS", Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "BTS", Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = "BTS", Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });

            //Assiging value for priorities
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "BTS", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "BTS", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "BTS", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = "BTS", Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4, IsVIP = true });

            //Mapping Request Priority List based on relative lookup and lookup value is index from relative list
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 3 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 1, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 2, SeverityLookup = 0, PriorityLookup = 0 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 1 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = "BTS", ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });
        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
    }
}
