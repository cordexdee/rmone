using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    //public class CON : IModule
    public class Contact : IModule
    {
        public static string userId = "1";
        public static string managerUserId = "1";
        public static string userName = ""; // Obtained from userId in UGovernITDefault.Initialize()

        public string[] HideInTemplate = { "Attachments", "AssetLookup", "Comment", "CreationDate", "Initiator", "RequestTypeCategory", "Status" };


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
        protected string moduleId = "1";
        protected static bool enableTestingStage = false;
        protected static bool enablePendingCloseStage = false;

        public UGITModule Module
        {
            get
            {
                return new UGITModule()
                {
                    ModuleName = ModuleName,
                    ShortName = "Contact Management",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "CRMContact",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/Contacts",
                    ModuleHoldMaxStage = 4,
                    Title = "Contact Management (CON)",
                    ModuleDescription = "This module is used to manage various contacts.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/AddContact",
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    HideWorkFlow = true,
                    EnableLayout = true
                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "CON";
            }
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");            
            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Activity", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Activity" });            
            mList.Add(new ModuleFormTab() { TabName = "Related Contacts", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Contacts" });            
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }
               
        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
           
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Status", FieldDisplayName = "Status", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "TicketId", FieldDisplayName = "ID", IsDisplay = false, IsUseInWildCard = false, DisplayForClosed = true, SortOrder = 1, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Title", FieldDisplayName = "Name", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMCompanyLookup", FieldDisplayName = "Company", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, SortOrder = 1, FieldSequence = ++seqNum, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "MobilePhone", FieldDisplayName = "Mobile", IsDisplay = true, DisplayForClosed = true, FieldSequence = ++seqNum, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "EmailAddress", FieldDisplayName = "Email", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ContactMethod", FieldDisplayName = "Contact Method", IsDisplay = true, DisplayForClosed = true, FieldSequence = ++seqNum, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Initiator", FieldDisplayName = "Created By", IsDisplay = true, DisplayForClosed = true, IsUseInWildCard = true, FieldSequence = ++seqNum, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CreationDate", FieldDisplayName = "Created On", IsDisplay = true, DisplayForClosed = true, FieldSequence = ++seqNum, ColumnType = "MultiUser", TextAlignment = "Center" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "OwnerUser", FieldDisplayName = "Assigned To", IsDisplay = true, DisplayForClosed = true, FieldSequence = ++seqNum, TextAlignment = "Left" });
            
                        
            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            // Console.WriteLine("  ModuleStages");
            List<string[]> dataList = new List<string[]>();
            // Start from ID 1
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 3;
            //if (!enableTestingStage)
            //    numStages--;
            //if (!enablePendingCloseStage)
            //    numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();
            int moduleStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Action = "Contact Created",
                Name = "Initiated",
                StageTitle = "Create",
                UserPrompt = "<b>Please fill the form to create a new contact.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Contact Created",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Re-Submit",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            //string pendingAssignmentStageID = currStageID.ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Active state",
                Name = "Active state",
                StageTitle = "Active",
                UserPrompt = "",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "De-activate",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 0,
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.None)
            });

            //string assignedStageID = currStageID.ToString();
            //moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Do Not Use",
                StageTitle = "Do Not Use",
                UserPrompt = "Do Not Use",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "Admin",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = Convert.ToInt32(currStageID - 1),
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "Owner activated contact",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Activate",
                StageTypeLookup = 5,
                StageAllApprovalsRequired = false,
                StageWeight = 0,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });

            //moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
              
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            // Console.WriteLine("  ModuleDefaultValues");

            /*
            string startStageID = Convert.ToString(moduleStartStages[ModuleName]);

            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "DesiredCompletionDate",
                KeyName = "DesiredCompletionDate",
                KeyValue = "TomorrowsDate",
                ModuleStepLookup = startStageID,
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Initiator",
                KeyName = "Initiator",
                KeyValue = "LoggedInUser",
                ModuleStepLookup = startStageID,
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "CreationDate",
                KeyName = "CreationDate",
                KeyValue = "TodaysDate",
                ModuleStepLookup = startStageID,
                CustomProperties = ""
            });
            mList.Add(new ModuleDefaultValue()
            {
                ModuleNameLookup = ModuleName,
                Title = ModuleName + " - " + "Owner",
                KeyName = "Owner",
                KeyValue = "LoggedInUser",
                ModuleStepLookup = startStageID,
                CustomProperties = ""
            });
            */

            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");
            int seqNum = 0;
            // Tab 1: General
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Prefix", FieldDisplayName = "Prefix", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Prefix", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "NameTitle", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Company", FieldDisplayName = "Company", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "First Name", FieldDisplayName = "First Name", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "FirstName", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Direct Phone", FieldDisplayName = "Direct Phone", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Telephone", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Street Address1", FieldDisplayName = "Street Address1", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Middle Name", FieldDisplayName = "Middle Name", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "MiddleName", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Mobile Phone", FieldDisplayName = "Mobile Phone", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "MobilePhone", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Street Address2", FieldDisplayName = "Street Address2", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "StreetAddress2", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Last Name", FieldDisplayName = "Last Name", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "LastName", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Fax", FieldDisplayName = "Fax", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Fax", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "City", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Addressed As", FieldDisplayName = "Addressed As", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "AddressedAs", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Email Address", FieldDisplayName = "Email Address", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "EmailAddress", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "StateLookup", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Contact Method", FieldDisplayName = "Contact Method", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "ContactMethod", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Email", FieldDisplayName = "Secondary Email", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "SecondaryEmail", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Zip", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldDisplayName = "Assigned To", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "OwnerUser", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", TabId = 1, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", TabId = 1, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Description", ModuleNameLookup = ModuleName, HideInTemplate = true, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, HideInTemplate = true });

            seqNum = 0;
            // Tab 2: Activity
            mList.Add(new ModuleFormLayout() { Title = "Contact Activity", FieldDisplayName = "Contact Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "ContactActivity", FieldDisplayName = "ContactActivity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Contact Activity", FieldDisplayName = "Contact Activity", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });

            seqNum = 0;
            // Tab 3: Related Contacts
            mList.Add(new ModuleFormLayout() { Title = "Related Contact", FieldDisplayName = "Related Contact", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CustomRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Related Contact", FieldDisplayName = "Related Contact", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });

            seqNum = 0;
            //Tab 4: History
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#Control#" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupEnd#" });

            seqNum = 0;
            // New Contact
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Prefix", FieldDisplayName = "Prefix", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Prefix", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "NameTitle", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Company", FieldDisplayName = "Company", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "First Name", FieldDisplayName = "First Name", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "FirstName", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Direct Phone", FieldDisplayName = "Direct Phone", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Telephone", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Street Address1", FieldDisplayName = "Street Address1", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Middle Name", FieldDisplayName = "Middle Name", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "MiddleName", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Mobile Phone", FieldDisplayName = "Mobile Phone", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "MobilePhone", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Street Address2", FieldDisplayName = "Street Address2", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "StreetAddress2", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Last Name", FieldDisplayName = "Last Name", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "LastName", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Fax", FieldDisplayName = "Fax", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Fax", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "City", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Addressed As", FieldDisplayName = "Addressed As", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "AddressedAs", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Email Address", FieldDisplayName = "Email Address", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "EmailAddress", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "StateLookup", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Contact Method", FieldDisplayName = "Contact Method", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "ContactMethod", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Email", FieldDisplayName = "Secondary Email", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "SecondaryEmail", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Zip", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldDisplayName = "Assigned To", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "OwnerUser", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "Description", ModuleNameLookup = ModuleName, HideInTemplate = false, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldDisplayName = "PlaceHolder", TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, HideInTemplate = false });


            string currTab = string.Empty;
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            //List<string[]> dataList = new List<string[]>();            
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            List<string[]> dataList = new List<string[]>();
            int moduleStep = 0;

            /*
            // All stages
            dataList.Add(new string[] { "Attachments", ModuleName, moduleStep.ToString(), "0", "0", "", "1", "" });
            dataList.Add(new string[] { "IsPrivate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            // dataList.Add(new string[] { "Comment", ModuleName, moduleStep.ToString(), "0", "0", "", "1", "" });

            // New , plus returned
            moduleStep++;
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "Description", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "Requestor", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "BusinessManager", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "RequestSource", ModuleName, moduleStep.ToString(), "1", "1", "", "1", "" });
            dataList.Add(new string[] { "AssetLookup", ModuleName, moduleStep.ToString(), "0", "0", "", "1", "" });
            dataList.Add(new string[] { "RequestTypeLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "SeverityLookup", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "ImpactLookup", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "DesiredCompletionDate", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "FunctionalArea", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "HelpDeskCall", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "PriorityLookup", ModuleName, moduleStep.ToString(), "0", "1", "", "1", "" });
            dataList.Add(new string[] { "InitiatorResolved", ModuleName, moduleStep.ToString(), "1", "1", "", "1", "" });
            dataList.Add(new string[] { "ResolutionComments", ModuleName, moduleStep.ToString(), "1", "1", "", "1", "" });
            dataList.Add(new string[] { "ActualHours", ModuleName, moduleStep.ToString(), "1", "0", "", "1", "" });
            dataList.Add(new string[] { "LocationLookup", ModuleName, "1", "1", "0", "", "0", "" });

            // Stage 2 - Pending Assignment
            moduleStep++;
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "Description", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "RequestTypeLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "ImpactLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "SeverityLookup", ModuleName, moduleStep.ToString(), "1", "1", "", "0", "" });
            dataList.Add(new string[] { "PRP", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "ORP", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "EstimatedHours", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "TargetCompletionDate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });

            // Stage 3 - Assigned
            moduleStep++;
            dataList.Add(new string[] { "PRP", ModuleName, moduleStep.ToString(), "1", "1", "Owner;#PRPGroup", "0", "" });
            dataList.Add(new string[] { "ORP", ModuleName, moduleStep.ToString(), "0", "1", "Owner;#PRPGroup", "0", "" });
            dataList.Add(new string[] { "TargetCompletionDate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "ActualHours", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "PctComplete", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "ActualCompletionDate", ModuleName, moduleStep.ToString(), "0", "0", "", "0", "" });
            dataList.Add(new string[] { "ResolutionComments", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            dataList.Add(new string[] { "ResolutionType", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });
            if (enableTestingStage)
                dataList.Add(new string[] { "Tester", ModuleName, moduleStep.ToString(), "1", "0", "", "0", "" });

            */

            dataList.Add(new string[] { "AddressedAs", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "City", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "ContactMethod", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "CRMCompanyLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "CustomTicketRelationship", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "EmailAddress", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Fax", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "LastName", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "MobilePhone", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "NameTitle", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Prefix", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "SecondaryEmail", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StreetAddress1", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StreetAddress2", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Telephone", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Description", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "OwnerUser", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "FirstName", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "MiddleName", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StateLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Zip", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "CRMCompanyLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Mobile", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "State", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "CRMCompanyLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Mobile", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "State", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });


            foreach (string[] data in dataList)
            {
                ModuleRoleWriteAccess mRoleWriteAccess = new ModuleRoleWriteAccess();
                mRoleWriteAccess.Title = data[2] + " - " + data[0];
                mRoleWriteAccess.FieldName = data[0];
                mRoleWriteAccess.ModuleNameLookup = data[1];
                mRoleWriteAccess.StageStep = Convert.ToInt32(data[2]);
                mRoleWriteAccess.FieldMandatory = Convert.ToBoolean(Convert.ToInt32(data[3]));
                mRoleWriteAccess.ShowEditButton = Convert.ToBoolean(Convert.ToInt32(data[4]));
                mRoleWriteAccess.ShowWithCheckbox = Convert.ToBoolean(Convert.ToInt32("0"));
                //if (!string.IsNullOrEmpty(data[5]))
                //    mRoleWriteAccess.ActionUser = data[5];
                mRoleWriteAccess.HideInServiceMapping = Convert.ToBoolean(Convert.ToInt32(data[6]));
                mRoleWriteAccess.CustomProperties = data[7];
                mList.Add(mRoleWriteAccess);

            }
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  StatusMapping");

            List<string[]> dataList = new List<string[]>();

            int stageID = Convert.ToInt32(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            dataList.Add(new string[] { "Initiated", ModuleName, stageID.ToString(), "1" });
            dataList.Add(new string[] { "Active", ModuleName, (++stageID).ToString(), "1" });
            dataList.Add(new string[] { "Do Not Use", ModuleName, (++stageID).ToString(), "2" });
            
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

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");

            /*
            dataList.Add(new string[] { "Initiated", "New PRS Ticket Initiated [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] has been initiated and needs clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.", ModuleName, stageID.ToString() , "TicketInitiator"});

            dataList.Add(new string[] { "Requestor Notification",  "New Ticket Created [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketIdWithoutLink$] has been created on your behalf.<br><br>" +
                                                        "Please review the details below. You will be notified when the ticket is resolved.", ModuleName, (stageID+1).ToString() , "TicketRequestor"});

            dataList.Add(new string[] { "Pending Assignment",  "New PRS Ticket Pending Assignment [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields", ModuleName, (++stageID).ToString(), "TicketOwner" });

            dataList.Add(new string[] { "Assigned",  "PRS Ticket Assigned [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] has been assigned to you for resolution.<br><br>" +
                                                        "Once resolved, please enter the resolution description and actual hours in the ticket, and assign a tester.", ModuleName, (++stageID).ToString(), "TicketPRP;#TicketORP"});

            if (enableTestingStage)
            {
                dataList.Add(new string[] { "Testing", "PRS Ticket Awaiting Testing [$TicketId$]: [$Title$]", @"PRS Ticket ID [$TicketId$] has been assigned to you for testing.<br><br>" +
                                                        "Please test the resolution and approve or reject the resolution.<br><br>[$IncludeActionButtons$]", ModuleName, (++stageID).ToString(), "TicketTester"});
            }

            if (enablePendingCloseStage)
            {
                dataList.Add(new string[] { "Pending Close", "PRS Ticket Pending Close [$TicketId$]: [$Title$]",  @"PRS Ticket ID [$TicketId$] has been resolved and is pending close.<br><br>" +
                                                "Please review the resolution and close the ticket.<br><br>[$IncludeActionButtons$]", ModuleName, (++stageID).ToString(), "TicketOwner"});
            }

            dataList.Add(new string[] { "Closed", "PRS Ticket Closed [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketId$] has been closed.", ModuleName, (stageID + 1).ToString(), "TicketOwner" });
            dataList.Add(new string[] { "Closed - Requestor", "Ticket Closed [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketIdWithoutLink$] has been closed.", ModuleName, (++stageID).ToString(), "TicketRequestor" });

            dataList.Add(new string[] { "On-Hold", "PRS Ticket On Hold [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketId$] has been placed on hold.", ModuleName, null, "TicketOwner" });
            dataList.Add(new string[] { "On-Hold - Requestor", "Ticket On Hold [$TicketId$]: [$Title$]", "PRS Ticket ID [$TicketIdWithoutLink$] has been placed on hold.", ModuleName, null, "TicketRequestor" });

            foreach (string[] data in dataList)
            {
                ModuleTaskEmail mTaskEmail = new ModuleTaskEmail();
                mTaskEmail.Title = data[3] + " - " + data[0];
                mTaskEmail.Status = data[0];
                mTaskEmail.EmailTitle = data[1];
                mTaskEmail.EmailBody = data[2];
                mTaskEmail.ModuleNameLookup = data[3];
                mTaskEmail.StageStep = Convert.ToInt32(data[4]);
                //mTaskEmail.EmailUserTypes = data[5];
                mList.Add(mTaskEmail);
            }
            */

            /*
            List<string[]> dataList = new List<string[]>();
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            dataList.Add(new string[] { "Create", "New Company Created [$TicketId$]: [$Title$]", @"COM Ticket ID [$TicketId$] has been initiated.<br><br>" +
                                                        "Please enter the required information.", ModuleName, stageID.ToString() , "Initiator"});

            dataList.Add(new string[] { "Active",  "Company is Activated  [$TicketId$]: [$Title$]", @"COM Ticket ID [$TicketId$] is Active.<br><br>" +
                                                        "This can be re-activated using close button", ModuleName, (++stageID).ToString(), "Owner" });

            dataList.Add(new string[] { "Do Not Use",  "Company is De-activated [$TicketId$]: [$Title$]", @"COM Ticket ID [$TicketId$] has been de-activated.<br><br>" +
                                                        "This can be re-activated using re-open button", ModuleName, (++stageID).ToString(), "Owner" });

            
            foreach (string[] data in dataList)
            {
                ModuleTaskEmail item = new ModuleTaskEmail();
                item.Title = data[3] + " - " + data[0];
                item.Status = data[0];
                item.EmailTitle = data[1];
                item.EmailBody = data[2];
                item.ModuleNameLookup = data[3];

                if(string.IsNullOrEmpty(data[4]))
                    item.StageStep = Convert.ToInt32(data[4]);

                item.EmailUserTypes = data[5];

                mList.Add(item);
            }
            */

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

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("ModuleUserTypes");
            //mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Initiator", UserTypes = "Initiator", ColumnName = "Initiator", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            //mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Requestor", UserTypes = "Requestor", ColumnName = "Requestor", ManagerOnly = false, ITOnly = false, CustomProperties = "DisableEmailTicketLink=true" });
            //mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Business Manager", UserTypes = "Business Approver", ColumnName = "BusinessManager", ManagerOnly = false, ITOnly = false, CustomProperties = "ManagerOf=TicketRequestor" });
            mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Owner", UserTypes = "Owner", ColumnName = "OwnerUser", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            //mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -PRP", UserTypes = "PRP", ColumnName = "PRP", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            //mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -ORP", UserTypes = "ORP", ColumnName = "ORP", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            //mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Tester", UserTypes = "Tester", ColumnName = "Tester", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            //mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -PRP Group", UserTypes = "PRP Group", ColumnName = "PRPGroup", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "CRMContact", TabOrder = 1, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "CRMContact", TabOrder = 2, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "CRMContact", TabOrder = 3, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "CRMContact", TabOrder = 4, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "CRMContact", TabOrder = 5, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Active", ViewName = "CRMContact", TabOrder = 6, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "CRMContact", TabOrder = 7, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "CRMContact", TabOrder = 8, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "CRMContact", TabOrder = 9, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Inactive", ViewName = "CRMContact", TabOrder = 10, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            return tabs;
        }

        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        { }
    }
}
