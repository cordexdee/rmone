using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class COM : IModule
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
                    ShortName = "Company Management",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "CRMCompany",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/company",
                    ModuleHoldMaxStage = 4,
                    Title = "Company Management (COM)",
                    ModuleDescription = "This module is used to manage various companies & contacts and also tracks the activities performed by contacts.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/com",
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
                return "COM";
            }
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");            
            mList.Add(new ModuleFormTab() { TabName = "Company Info", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Company Details", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Work Activity" });
            mList.Add(new ModuleFormTab() { TabName = "Contacts", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Related Companies", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }
               
        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // COM            
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Initiator", FieldDisplayName = "Initiator", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "TicketId", FieldDisplayName = "ID", IsDisplay = false, IsUseInWildCard = true, DisplayForClosed = true,  FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Title", FieldDisplayName = "Company Name", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = true, FieldSequence = ++seqNum, TextAlignment = "Left", SortOrder = 1 });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "StreetAddress1", FieldDisplayName = "Address", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum, TextAlignment = "Left" , ColumnType = "String"});
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ModuleStepLookup", FieldDisplayName = "Status", IsDisplay = false, DisplayForClosed = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "RelationshipTypeLookup", FieldDisplayName = "Relationship Type", IsDisplay = true, IsUseInWildCard = true, DisplayForClosed = false, FieldSequence = ++seqNum, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CreationDate", FieldDisplayName = "Created On", IsDisplay = false, DisplayForClosed = true, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "RequestTypeLookup", FieldDisplayName = "Business Type", IsDisplay = true, IsUseInWildCard = true, FieldSequence = ++seqNum, TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "OwnerUser", FieldDisplayName = "Assigned To", IsDisplay = true, DisplayForClosed = true, FieldSequence = ++seqNum, ColumnType = "MultiUser", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "City", FieldDisplayName = "City", IsDisplay = false, DisplayForClosed = false,  FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "StateLookup", FieldDisplayName = "State", IsDisplay = false, DisplayForClosed = false, FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Zip", FieldDisplayName = "Zip", IsDisplay = false, DisplayForClosed = false, FieldSequence = ++seqNum });
                        
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
                Action = "Initiated",
                Name = "Initiated",
                StageTitle = "Create",
                UserPrompt = "<b>Please fill the form to create a new company.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "Admin",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Company Created",
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
                StageApproveButtonName = "",
                StageRejectedButtonName = "De-activate",
                StageReturnButtonName = "",
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
                ReturnActionDescription = "Owner activated company",
                StageApproveButtonName = "",
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
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Company Name", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "Title" });
            mList.Add(new ModuleFormLayout() { Title = "Abbreviated Name", FieldDisplayName = "Abbreviated Name", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "ShortName" });
            mList.Add(new ModuleFormLayout() { Title = "Street 1", FieldDisplayName = "Street 1", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "StreetAddress1" });
            mList.Add(new ModuleFormLayout() { Title = "Street 2", FieldDisplayName = "Street 2", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "StreetAddress2" });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldDisplayName = "Assigned To", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "OwnerUser" });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "City" });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "StateLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "Zip" });
            //mList.Add(new ModuleFormLayout() { Title = "Country", FieldDisplayName = "Country", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Country") ? true : false, FieldName = "Country" });
            mList.Add(new ModuleFormLayout() { Title = "Telephone", FieldDisplayName = "Telephone", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "Telephone" });
            mList.Add(new ModuleFormLayout() { Title = "Primary Contact", FieldDisplayName = "Primary Contact", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "ContactLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Fax", FieldDisplayName = "Fax", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "Fax" });
            mList.Add(new ModuleFormLayout() { Title = "General Email", FieldDisplayName = "General Email", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "EmailAddress" });
            mList.Add(new ModuleFormLayout() { Title = "Website URL", FieldDisplayName = "Website URL", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "WebsiteUrl" });
            mList.Add(new ModuleFormLayout() { Title = "Master Agreement", FieldDisplayName = "Master Agreement", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "MasterAgreement" });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "Description", ColumnType = Constants.ColumnType.NoteField });

            //mList.Add(new ModuleFormLayout() { Title = "Contact Owner", FieldDisplayName = "Contact Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Owner") ? true : false, FieldName = "Owner" });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupEnd#" });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "Relationship Type", FieldDisplayName = "Relationship Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "RelationshipTypeLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Business Type", FieldDisplayName = "Business Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "RequestTypeLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Business Type", FieldDisplayName = "Secondary Business Type", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "AdditionalInfo" });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupEnd#" });
            //mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, FieldName = "#GroupStart#" });
            //mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Attachments") ? true : false, FieldName = "Attachments" });
            //mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("IsPrivate") ? true : false, FieldName = "IsPrivate" });
            //mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, FieldName = "#GroupEnd#" });

            seqNum = 0;
            // Tab 2: Company Details
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "Federal ID #", FieldDisplayName = "Federal ID #", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "FederalID" });
            mList.Add(new ModuleFormLayout() { Title = "SIC Code", FieldDisplayName = "SIC Code", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "SICCode" });
            mList.Add(new ModuleFormLayout() { Title = "Contractor License #", FieldDisplayName = "Contractor License #", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "ContractorLicense" });
            mList.Add(new ModuleFormLayout() { Title = "Division", FieldDisplayName = "Division", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Division" });
            mList.Add(new ModuleFormLayout() { Title = "Annual Revenues", FieldDisplayName = "Annual Revenues", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "AnnualRevenues" });
            mList.Add(new ModuleFormLayout() { Title = "Ownership Type", FieldDisplayName = "Ownership Type", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "OwnershipType" });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "#GroupEnd#" });
            mList.Add(new ModuleFormLayout() { Title = "Agreements and Certifications", FieldDisplayName = "Agreements and Certifications", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "#GroupStart#" });
            //mList.Add(new ModuleFormLayout() { Title = "Master Agreement", FieldDisplayName = "Master Agreement", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "MasterAgreement" });
            mList.Add(new ModuleFormLayout() { Title = "Regions of Work", FieldDisplayName = "Regions of Work", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "RegionsofWork" });
            mList.Add(new ModuleFormLayout() { Title = "Types of Work", FieldDisplayName = "Types of Work", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "TypesofWork" });
            mList.Add(new ModuleFormLayout() { Title = "Status Attribute", FieldDisplayName = "Status Attribute", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Status" });
            mList.Add(new ModuleFormLayout() { Title = "Union Affiliation", FieldDisplayName = "Union Affiliation", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "UnionAffiliation" });           
            mList.Add(new ModuleFormLayout() { Title = "Certifications", FieldDisplayName = "Certifications", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Certifications" });
            mList.Add(new ModuleFormLayout() { Title = "#PlaceHolder#", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayName = "#PlaceHolder#", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Agreements and Certifications", FieldDisplayName = "Agreements and Certifications", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "#GroupEnd#" });

            mList.Add(new ModuleFormLayout() { Title = "", FieldDisplayName = "", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "Bonding Information", FieldDisplayName = "Bonding Information", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Comment" });
            mList.Add(new ModuleFormLayout() { Title = "", FieldDisplayName = "", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "#GroupEnd#" });

            seqNum = 0;
            // Tab 3: Contacts
            mList.Add(new ModuleFormLayout() { Title = "Contacts", FieldDisplayName = "Contacts", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "Contacts", FieldDisplayName = "Contacts", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#Control#" });
            mList.Add(new ModuleFormLayout() { Title = "Contacts", FieldDisplayName = "Contacts", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupEnd#" });

            seqNum = 0;
            // Tab 4: Related Companies
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Companies", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "CustomRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Companies", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });

            seqNum = 0;
            // Tab 5: Comments
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });

            seqNum = 0;
            //Tab 6: History
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#Control#" });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 6, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "History", FieldSequence = ++seqNum, HideInTemplate = true, FieldName = "#GroupEnd#" });

            seqNum = 0;
            // New ticket form
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Company Name", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "Title" });
            mList.Add(new ModuleFormLayout() { Title = "Abbreviated Name", FieldDisplayName = "Abbreviated Name", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "ShortName" });
            mList.Add(new ModuleFormLayout() { Title = "Street 1", FieldDisplayName = "Street 1", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "StreetAddress1" });
            mList.Add(new ModuleFormLayout() { Title = "Street 2", FieldDisplayName = "Street 2", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "StreetAddress2" });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldDisplayName = "Assigned To", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "OwnerUser" });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldDisplayName = "City", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "City" });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldDisplayName = "State", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "StateLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldDisplayName = "Zip", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Zip" });
            //mList.Add(new ModuleFormLayout() { Title = "Country", FieldDisplayName = "Country", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Country") ? true : false, FieldName = "Country" });
            mList.Add(new ModuleFormLayout() { Title = "Telephone", FieldDisplayName = "Telephone", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Telephone" });
            mList.Add(new ModuleFormLayout() { Title = "Primary Contact", FieldDisplayName = "Primary Contact", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "ContactLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Fax", FieldDisplayName = "Fax", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Fax" });
            mList.Add(new ModuleFormLayout() { Title = "General Email", FieldDisplayName = "General Email", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "EmailAddress" });
            mList.Add(new ModuleFormLayout() { Title = "Website URL", FieldDisplayName = "Website URL", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "WebsiteUrl" });
            mList.Add(new ModuleFormLayout() { Title = "Master Agreement", FieldDisplayName = "Master Agreement", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "MasterAgreement" });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "Description", ColumnType = Constants.ColumnType.NoteField });
            //mList.Add(new ModuleFormLayout() { Title = "Contact Owner", FieldDisplayName = "Contact Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Owner") ? true : false, FieldName = "Owner" });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "#GroupEnd#" });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "#GroupStart#" });
            mList.Add(new ModuleFormLayout() { Title = "Relationship Type", FieldDisplayName = "Relationship Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "RelationshipTypeLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Business Type", FieldDisplayName = "Business Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "RequestTypeLookup" });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Business Type", FieldDisplayName = "Secondary Business Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate =  false, FieldName = "AdditionalInfo" });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldDisplayName = "Classification", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = false, FieldName = "#GroupEnd#" });
            //mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupStart#") ? true : false, FieldName = "#GroupStart#" });
            //mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, ShowInMobile = false, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("Attachments") ? true : false, FieldName = "Attachments" });
            //mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("IsPrivate") ? true : false, FieldName = "IsPrivate" });
            //mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = HideInTemplate.Contains("#GroupEnd#") ? true : false, FieldName = "#GroupEnd#" });


            string currTab = string.Empty;
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            //List<string[]> dataList = new List<string[]>();

            mList.Add(new ModuleRequestType() { Title = "Aerospace", RequestType = "Aerospace", ModuleNameLookup = ModuleName, Category = "Aerospace", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Automotive > Car Wash", RequestType = "Car Wash", ModuleNameLookup = ModuleName, Category = "Aerospace", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Automotive > Service & Gas Station", RequestType = "Service & Gas Station", ModuleNameLookup = ModuleName, Category = "Aerospace", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "ModuleNamemunity > Auditorium", RequestType = "Auditorium", ModuleNameLookup = ModuleName, Category = "ModuleNamemunity", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "ModuleNamemunity > ModuleNamemunity Center", RequestType = "ModuleNamemunity Center", ModuleNameLookup = ModuleName, Category = "ModuleNamemunity", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "ModuleNamemunity > Convention Center", RequestType = "Convention Center", ModuleNameLookup = ModuleName, Category = "ModuleNamemunity", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "ModuleNamemunity > Library", RequestType = "Library", ModuleNameLookup = ModuleName, Category = "ModuleNamemunity", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "ModuleNamemunity > Museum", RequestType = "Museum", ModuleNameLookup = ModuleName, Category = "ModuleNamemunity", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Management", RequestType = "Management", ModuleNameLookup = ModuleName, Category = "Consulting", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Architecture", RequestType = "Architecture", ModuleNameLookup = ModuleName, Category = "Design", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Marketing/Advertising", RequestType = "Marketing/Advertising", ModuleNameLookup = ModuleName, Category = "Consulting", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Staffing", RequestType = "Staffing", ModuleNameLookup = ModuleName, Category = "Consulting", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Public Relations", RequestType = "Public Relations", ModuleNameLookup = ModuleName, Category = "Consulting", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Corporate > Law Firm", RequestType = "Law Firm", ModuleNameLookup = ModuleName, Category = "Consulting", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Corporate > Retail Corporate Office", RequestType = "Retail Corporate Office", ModuleNameLookup = ModuleName, Category = "Consulting", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Education > Boarding School", RequestType = "Boarding School", ModuleNameLookup = ModuleName, Category = "Education", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Education > College/University", RequestType = "College/University", ModuleNameLookup = ModuleName, Category = "Education", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Day Care", RequestType = "Day Care", ModuleNameLookup = ModuleName, Category = "Education", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "K-12", RequestType = "K-12", ModuleNameLookup = ModuleName, Category = "Education", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Pre-school", RequestType = "Pre-school", ModuleNameLookup = ModuleName, Category = "Education", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Vocational Training", RequestType = "Vocational Training", ModuleNameLookup = ModuleName, Category = "Education", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Entertainment", RequestType = "Entertainment", ModuleNameLookup = ModuleName, Category = "Entertainment", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Entertainment > Casino", RequestType = "Casino", ModuleNameLookup = ModuleName, Category = "Entertainment", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Entertainment > Concert Hall", RequestType = "Concert Hall", ModuleNameLookup = ModuleName, Category = "Entertainment", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Entertainment > Movie Theater", RequestType = "Movie Theater", ModuleNameLookup = ModuleName, Category = "Entertainment", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Entertainment > Social Club", RequestType = "Social Club", ModuleNameLookup = ModuleName, Category = "Entertainment", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Entertainment > TV/Radio Station", RequestType = "TV/Radio Station", ModuleNameLookup = ModuleName, Category = "Entertainment", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Accounting", RequestType = "Accounting", ModuleNameLookup = ModuleName, Category = "Financial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Banking", RequestType = "Banking", ModuleNameLookup = ModuleName, Category = "Financial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Insurance", RequestType = "Insurance", ModuleNameLookup = ModuleName, Category = "Financial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Asset Management", RequestType = "Asset Management", ModuleNameLookup = ModuleName, Category = "Financial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Tax Services", RequestType = "Tax Services", ModuleNameLookup = ModuleName, Category = "Financial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Venture Capital", RequestType = "Venture Capital", ModuleNameLookup = ModuleName, Category = "Financial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Government > Courthouse", RequestType = "Courthouse", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Embassy", RequestType = "Embassy", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Government > Fire Station", RequestType = "Fire Station", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Government Offices", RequestType = "Government Offices", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Government > Military Office", RequestType = "Military Office", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Parks & Recreation", RequestType = "Parks & Recreation", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Government > Police Station", RequestType = "Police Station", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Government > Post Office", RequestType = "Post Office", ModuleNameLookup = ModuleName, Category = "Government", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Equipment/Medical Device", RequestType = "Equipment/Medical Device", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Health Care Providers", RequestType = "Health Care Providers", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Facilities/Medical Office", RequestType = "Facilities/Medical Office", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Managed Care Facilities", RequestType = "Managed Care Facilities", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Healthcare > Pharmaceutical", RequestType = "Pharmaceutical", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Healthcare > Rehabilitation Center", RequestType = "Rehabilitation Center", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Casinos", RequestType = "Casinos", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Social Clubs/Leisure Facilities", RequestType = "Social Clubs/Leisure Facilities", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Hotels/Resorts", RequestType = "Hotels/Resorts", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Restaurants/Bars", RequestType = "Restaurants/Bars", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Industrial > Manufacturing Facility", RequestType = "Manufacturing Facility", ModuleNameLookup = ModuleName, Category = "Industrial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Industrial > Winery/Brewery", RequestType = "Winery/Brewery", ModuleNameLookup = ModuleName, Category = "Industrial", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Non Profit > Charity", RequestType = "Charity", ModuleNameLookup = ModuleName, Category = "Non Profit", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Non Profit > Foundation", RequestType = "Foundation", ModuleNameLookup = ModuleName, Category = "Non Profit", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Non Profit > Philanthropy", RequestType = "Philanthropy", ModuleNameLookup = ModuleName, Category = "Non Profit", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Brokerage", RequestType = "Brokerage", ModuleNameLookup = ModuleName, Category = "Real Estate", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Development", RequestType = "Development", ModuleNameLookup = ModuleName, Category = "Real Estate", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Real Estate", RequestType = "Real Estate", ModuleNameLookup = ModuleName, Category = "ModuleNamemercial Real Estate", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Religious > Chapel/Church", RequestType = "Chapel/Church", ModuleNameLookup = ModuleName, Category = "Religious", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Religious > Mosque", RequestType = "Mosque", ModuleNameLookup = ModuleName, Category = "Religious", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Religious > Temple", RequestType = "Temple", ModuleNameLookup = ModuleName, Category = "Religious", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Home Furnishing Retail", RequestType = "Home Furnishing Retail", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Apparel Retail", RequestType = "Apparel Retail", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Department Stores", RequestType = "Department Stores", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Internet Retail", RequestType = "Internet Retail", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Home Furnishings", RequestType = "Home Furnishings", ModuleNameLookup = ModuleName, Category = "Consumer Goods", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Retail > Jewelry Store", RequestType = "Jewelry Store", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Retail > Shopping Center", RequestType = "Shopping Center", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Specialty Stores", RequestType = "Specialty Stores", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Food and Drug", RequestType = "Food and Drug", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Automotive", RequestType = "Automotive", ModuleNameLookup = ModuleName, Category = "Retail", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Sports and Recreation", RequestType = "Sports and Recreation", ModuleNameLookup = ModuleName, Category = "Sports and Recreation", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Sports > Fitness Center/Studio", RequestType = "Fitness Center/Studio", ModuleNameLookup = ModuleName, Category = "Sports and Recreation", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Sports > Recreation Center", RequestType = "Recreation Center", ModuleNameLookup = ModuleName, Category = "Sports and Recreation", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Gaming", RequestType = "Gaming", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Internet", RequestType = "Internet", ModuleNameLookup = ModuleName, Category = "Mass Media", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Hardware and Equipment", RequestType = "Hardware and Equipment", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "IT Services", RequestType = "IT Services", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Semiconductor", RequestType = "Semiconductor", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Software", RequestType = "Software", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Airport", RequestType = "Airport", ModuleNameLookup = ModuleName, Category = "Transportation", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Parking Garage", RequestType = "Parking Garage", ModuleNameLookup = ModuleName, Category = "Transportation", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Transportation", RequestType = "Transportation", ModuleNameLookup = ModuleName, Category = "Transportation", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Construction Management", RequestType = "Construction Management", ModuleNameLookup = ModuleName, Category = "Construction", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Engineering", RequestType = "Engineering", ModuleNameLookup = ModuleName, Category = "Design", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Furniture", RequestType = "Furniture", ModuleNameLookup = ModuleName, Category = "Design", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "General Contractors", RequestType = "General Contractors", ModuleNameLookup = ModuleName, Category = "Construction", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Subcontractors", RequestType = "Subcontractors", ModuleNameLookup = ModuleName, Category = "Construction", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Suppliers", RequestType = "Suppliers", ModuleNameLookup = ModuleName, Category = "Construction", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Accessories", RequestType = "Accessories", ModuleNameLookup = ModuleName, Category = "Consumer Goods", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Apparel", RequestType = "Apparel", ModuleNameLookup = ModuleName, Category = "Consumer Goods", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Electronics", RequestType = "Electronics", ModuleNameLookup = ModuleName, Category = "Consumer Goods", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Household Products", RequestType = "Household Products", ModuleNameLookup = ModuleName, Category = "Consumer Goods", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Textiles", RequestType = "Textiles", ModuleNameLookup = ModuleName, Category = "Consumer Goods", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Electrical Power", RequestType = "Electrical Power", ModuleNameLookup = ModuleName, Category = "Energy", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Petroleum", RequestType = "Petroleum", ModuleNameLookup = ModuleName, Category = "Energy", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Solar", RequestType = "Solar", ModuleNameLookup = ModuleName, Category = "Energy", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Fintech", RequestType = "Fintech", ModuleNameLookup = ModuleName, Category = "Financial", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Food and Beverage", RequestType = "Food and Beverage", ModuleNameLookup = ModuleName, Category = "Food and Beverage", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Legal", RequestType = "Legal", ModuleNameLookup = ModuleName, Category = "Legal", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Biotechnology", RequestType = "Biotechnology", ModuleNameLookup = ModuleName, Category = "Life Science", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Pharmaceutical", RequestType = "Pharmaceutical", ModuleNameLookup = ModuleName, Category = "Life Science", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Broadcasting", RequestType = "Broadcasting", ModuleNameLookup = ModuleName, Category = "Mass Media", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Film", RequestType = "Film", ModuleNameLookup = ModuleName, Category = "Mass Media", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Music", RequestType = "Music", ModuleNameLookup = ModuleName, Category = "Mass Media", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "News", RequestType = "News", ModuleNameLookup = ModuleName, Category = "Mass Media", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Publishing", RequestType = "Publishing", ModuleNameLookup = ModuleName, Category = "Mass Media", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "TeleModuleNamemunications", RequestType = "TeleModuleNamemunications", ModuleNameLookup = ModuleName, Category = "ModuleNamemunications", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Utilities", RequestType = "Utilities", ModuleNameLookup = ModuleName, Category = "Utilities", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Mass Media", RequestType = "Mass Media", ModuleNameLookup = ModuleName, Category = "ModuleNamemunications", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Consumer Goods", RequestType = "Consumer Goods", ModuleNameLookup = ModuleName, Category = "Consumer Products", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Food and Beverage", RequestType = "Food and Beverage", ModuleNameLookup = ModuleName, Category = "Consumer Products", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Education", RequestType = "Education", ModuleNameLookup = ModuleName, Category = "Education/Institutional", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Government", RequestType = "Government", ModuleNameLookup = ModuleName, Category = "Education/Institutional", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Healthcare > Healthcare", RequestType = "Healthcare", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Healthcare > Life Sciences", RequestType = "Life Sciences", ModuleNameLookup = ModuleName, Category = "Healthcare", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Hospitality", RequestType = "Hospitality", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Retail", RequestType = "Retail", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Entertainment", RequestType = "Entertainment", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Sports and Recreation", RequestType = "Sports and Recreation", ModuleNameLookup = ModuleName, Category = "Hospitality", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Non Profit > Non Profit", RequestType = "Non Profit", ModuleNameLookup = ModuleName, Category = "Non Profit", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Consulting", RequestType = "Consulting", ModuleNameLookup = ModuleName, Category = "Professional Services", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Financial (excluding Fintech)", RequestType = "Financial (excluding Fintech)", ModuleNameLookup = ModuleName, Category = "Professional Services", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Legal", RequestType = "Legal", ModuleNameLookup = ModuleName, Category = "Professional Services", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Technology", RequestType = "Technology", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Fintech (request type level)", RequestType = "Fintech (request type level)", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Aerospace", RequestType = "Aerospace", ModuleNameLookup = ModuleName, Category = "Other", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Energy", RequestType = "Energy", ModuleNameLookup = ModuleName, Category = "Other", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Manufacturing", RequestType = "Manufacturing", ModuleNameLookup = ModuleName, Category = "Other", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Transportation", RequestType = "Transportation", ModuleNameLookup = ModuleName, Category = "Other", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Utilities", RequestType = "Utilities", ModuleNameLookup = ModuleName, Category = "Other", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Management/Services", RequestType = "Management/Services", ModuleNameLookup = ModuleName, Category = "Real Estate", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Online Services", RequestType = "Online Services", ModuleNameLookup = ModuleName, Category = "Technology", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = true });
            mList.Add(new ModuleRequestType() { Title = "Manufacturing", RequestType = "Manufacturing", ModuleNameLookup = ModuleName, Category = "Manufacturing", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Telecommunications", RequestType = "Telecommunications", ModuleNameLookup = ModuleName, Category = "Telecommunications", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services> Consulting", RequestType = "Consulting", ModuleNameLookup = ModuleName, Category = "Professional Services", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services> Financial (excluding Fintech)", RequestType = "Financial (excluding Fintech)", ModuleNameLookup = ModuleName, Category = "Professional Services", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Professional Services> Legal", RequestType = "Legal", ModuleNameLookup = ModuleName, Category = "Professional Services", Owner = null, FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false });
            mList.Add(new ModuleRequestType() { Title = "Logistics", RequestType = "Logistics", ModuleNameLookup = ModuleName, Category = "Transportation", Owner = null, FunctionalAreaLookup = null, WorkflowType = "Full", Deleted = false });
            
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

            dataList.Add(new string[] { "AdditionalInfo", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "AnnualRevenues", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Certifications", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "City", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "ContactLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Contacts", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Country", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "ContractorLicense", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Division", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "EmailAddress", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Fax", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "FederalID", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "MasterAgreement", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "OwnershipType", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RelationshipType", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RelationshipTypeLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "SICCode", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StreetAddress1", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StreetAddress2", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Telephone", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Description", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "OwnerUser", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RequestTypeLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ShortName", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StateLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "WebsiteUrl", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "UnionAffiliation", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RegionsofWork", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "TypesofWork", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Zip", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "RequestTypeCategory", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "StateLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "RequestTypeCategory", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "StateLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });


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
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "CRMCompany", TabOrder = 1, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "CRMCompany", TabOrder = 2, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "CRMCompany", TabOrder = 3, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "CRMCompany", TabOrder = 4, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "CRMCompany", TabOrder = 5, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Active", ViewName = "CRMCompany", TabOrder = 6, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "CRMCompany", TabOrder = 7, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "CRMCompany", TabOrder = 8, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "CRMCompany", TabOrder = 9, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Inactive", ViewName = "CRMCompany", TabOrder = 10, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            return tabs;
        }

        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        { }
    }
}
