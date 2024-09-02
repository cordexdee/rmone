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
    public class TSK : IModule
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
                    ShortName = "Task Lists",
                    CategoryName = "Project Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "TSK",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/tskprojects",
                    ModuleHoldMaxStage = 2,
                    Title = "Task Lists (TSK)",
                     ModuleDescription = "This module provides lightweight project management and is used to manage simple task lists that don't require the additional functionality of budgets, baselines, actuals tracking, etc. Managers can create simple task lists which can be assigned to team members. Team members can mark assigned tasks as complete from their My Tasks section.",
                    StaticModulePagePath = "/Pages/tsk",
                    ModuleType = ModuleType.Governance,
                    EnableModule = true,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = false,
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
                return "TSK";
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

            mList.Add(new ModuleFormTab() { TabName = "General", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General" });
            mList.Add(new ModuleFormTab() { TabName = "Tasks", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Tasks" });
            mList.Add(new ModuleFormTab() { TabName = "Resource Sheet", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Resource Sheet" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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
            // Console.WriteLine("TaskEmails");
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "TSK", Title = "2-High", uPriority = "2-High", ItemOrder = 1 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "TSK", Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 2 });
            mList.Add(new ModulePrioirty() { ModuleNameLookup = "TSK", Title = "4-Low", uPriority = "4-Low", ItemOrder = 3 });
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSK", Title = "TSK-Initiator", UserTypes = "Initiator", ColumnName = "InitiatorUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSK", Title = "TSK-Project Manager", UserTypes = "Project Manager", ColumnName = "ProjectManagerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSK", Title = "TSK-Sponsors", UserTypes = "Sponsors", ColumnName = "SponsorsUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = "TSK", Title = "TSK-Stake Holders", UserTypes = "Stake Holder", ColumnName = "StakeHoldersUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");

            List<string[]> dataList = new List<string[]>();

            // Start from id - 58
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 3;
            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Initiated",
                UserPrompt = "<b>Project is in Initiated / Identification Phase.</b>",
                StageStep = 1,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ActionUser = "InitiatorUser",
                ApproveActionDescription = "Ticket Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "(Re)Submit",
                StageRejectedButtonName = "Cancel",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Active Phase",
                Name = "Active",
                StageTitle = "Active",
                UserPrompt = "<b>Project is Active</b>",
                StageStep = 2,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ActionUser = "ProjectManagerUser;#InitiatorUser;#PMO",
                ApproveActionDescription = "Close",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Closeout Project",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = "Initiated"
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed Phase",
                Name = "Closed",
                StageTitle = "Closed",
                UserPrompt = "<b>Project is Closed</b>",
                StageStep = 3,
                ModuleNameLookup = ModuleName,
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 1,
                ActionUser = "ProjectManagerUser;#InitiatorUser;#PMO",
                ApproveActionDescription = "Close",
                RejectActionDescription = "",
                ReturnActionDescription = "Project re-opened",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });

            // TSK doesn't use stages so just manually set it to closed stage
            currStageID = int.Parse(closeStageID);

            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "TicketId", FieldDisplayName = "Project ID", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, SortOrder = 1, FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "Status", FieldDisplayName = "Progress", DisplayForClosed = true, ColumnType = "ProgressBar", IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsUseInWildCard = true, FieldSequence = ++seqNum });
           
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "PctComplete", FieldDisplayName = "% Comp", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "Created", FieldDisplayName = "Created On", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "CloseDate", FieldDisplayName = "Closed On", IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "ActualCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, CustomProperties = "fieldtype=datetime;#useforglobaldatefilter=true", DisplayForClosed = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "ProjectManagerUser", FieldDisplayName = "Task Manager", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "ProjectInitiativeLookup", FieldDisplayName = "Initiative", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "NextActivity", FieldDisplayName = "Next Activity", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "NextMilestone", FieldDisplayName = "Next Milestone", DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = "TSK", FieldName = "BeneficiariesLookup", FieldDisplayName = "Company", DisplayForClosed = false, IsUseInWildCard = false, FieldSequence = ++seqNum });

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

            // Tab 1: General 
            // Display Basic project infomation title, manager, benieficaries, sponsors
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Task Manager", FieldDisplayName = "Task Manager", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ProjectManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Project Status", FieldDisplayName = "Project Status", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Status", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Primary Beneficiaries", FieldDisplayName = "Primary Beneficiaries", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { Title = "Functional Area", FieldDisplayName = "Functional Area", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });



            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Attachment
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            // Tab 2: Project tasks
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Tasks", FieldDisplayName = "Tasks", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "TasksList", FieldDisplayName = "TasksList", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Tasks", FieldDisplayName = "Tasks", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 3: Resources
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Resources", FieldDisplayName = "Resources", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ProjectResourceDetail", FieldDisplayName = "ProjectResourceDetail", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", CustomProperties = "IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Resources", FieldDisplayName = "Resources", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 4: Related tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 5: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            //Tab 0
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Title", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Task Manager", FieldDisplayName = "Task Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "ProjectManagerUser", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Priority", FieldDisplayName = "Priority", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PriorityLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Primary Beneficiaries", FieldDisplayName = "Primary Beneficiaries", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "BeneficiariesLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Functional Area", FieldDisplayName = "Functional Area", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FunctionalAreaLookup", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });



            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Description", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), ColumnType = Constants.ColumnType.NoteField });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            return mList;
            //throw new NotImplementedException();
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            // Tab 1
            mList.Add(new ModuleRoleWriteAccess() { Title = 0 + "-" + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = 0 + "-" + "ProjectManager", FieldName = "ProjectManagerUser", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = 0 + "-" + "Beneficiaries", FieldName = "BeneficiariesLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = 0 + "-" + "FunctionalAreaLookup", FieldName = "FunctionalAreaLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = false, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = 0 + "-" + "PriorityLookup", FieldName = "PriorityLookup", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
            mList.Add(new ModuleRoleWriteAccess() { Title = 0 + "-" + "Description", FieldName = "Description", ModuleNameLookup = ModuleName, StageStep = 0, FieldMandatory = true, ShowWithCheckbox = false, ShowEditButton = true });
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

        public List<UGITModule> GetUGITModule()
        {
            List<UGITModule> mList = new List<UGITModule>();
            return mList;

        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "TSKProjects", TabOrder = 1, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName= "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "TSKProjects", TabOrder = 2, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "TSKProjects", TabOrder = 3, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "TSKProjects", TabOrder = 4, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "TSKProjects", TabOrder = 5, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "TSKProjects", TabOrder = 6, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "TSKProjects", TabOrder = 7, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "TSKProjects", TabOrder = 8, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "TSKProjects", TabOrder = 9, ModuleNameLookup = "TSK", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            return tabs;
        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {
            impacts.Add(new ModuleImpact() { ModuleNameLookup = ModuleName, Title = "Impacts Organization", Impact = "Impacts Organization", ItemOrder = 1 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = ModuleName, Title = "Impacts Department", Impact = "Impacts Department", ItemOrder = 2 });
            impacts.Add(new ModuleImpact() { ModuleNameLookup = ModuleName, Title = "Impacts Individual", Impact = "Impacts Individual", ItemOrder = 3 });

            serverities.Add(new ModuleSeverity() { ModuleNameLookup = ModuleName, Name = "High", Title = "High", Severity = "High", ItemOrder = 1 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = ModuleName, Name = "Medium", Title = "Medium", Severity = "Medium", ItemOrder = 2 });
            serverities.Add(new ModuleSeverity() { ModuleNameLookup = ModuleName, Name = "Low", Title = "Low", Severity = "Low", ItemOrder = 3 });

            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "2-High", uPriority = "2-High", ItemOrder = 4 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "3-Medium", uPriority = "3-Medium", ItemOrder = 5 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "4-Low", uPriority = "4-Low", ItemOrder = 6 });
            priorities.Add(new ModulePrioirty() { ModuleNameLookup = ModuleName, Title = "1-Critical", uPriority = "1-Critical", ItemOrder = 4, IsVIP = true });


            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 2, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 1, SeverityLookup = 2, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 0, SeverityLookup = 2, PriorityLookup = 1 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 2, SeverityLookup = 1, PriorityLookup = 2 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 1, SeverityLookup = 1, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 0, SeverityLookup = 1, PriorityLookup = 0 });

            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 2, SeverityLookup = 0, PriorityLookup = 1 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 1, SeverityLookup = 0, PriorityLookup = 0 });
            mapping.Add(new ModulePriorityMap() { ModuleNameLookup = ModuleName, ImpactLookup = 0, SeverityLookup = 0, PriorityLookup = 3 });
        }
    }
}
