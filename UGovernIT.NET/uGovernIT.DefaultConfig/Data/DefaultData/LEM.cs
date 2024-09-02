using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class LEM : IModule
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
                    ShortName = "Lead Management",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "Lead",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/Lead",
                    ModuleHoldMaxStage = 0,
                    Title = "Lead Management (LEM)",
                    ModuleDescription = "This module is used to track lead activities and potential opportunities that can be identified from them.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/LEM",
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    HideWorkFlow = false,
                    EnableLayout = true
                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "LEM";
            }
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            // Console.WriteLine("  ModuleFormTab");            
            mList.Add(new ModuleFormTab() { TabName = "Summary", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Summary" });
            mList.Add(new ModuleFormTab() { TabName = "Tasks", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Tasks" });
            mList.Add(new ModuleFormTab() { TabName = "Activity", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Activity" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            int seqNum = 0;
            // LEM            
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "TicketId", FieldDisplayName = "ID", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CreationDate", FieldDisplayName = "Created On", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "miniview=1", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Title", FieldDisplayName = "Project", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "miniview=1", ColumnType = "", TextAlignment = "", SortOrder = 1 });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMCompanyLookup", FieldDisplayName = "Company", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMUrgency", FieldDisplayName = "Urgency", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "SuccessChance", FieldDisplayName = "Lead Priority", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Status", FieldDisplayName = "Stage", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "ProgressBar", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "RequestTypeLookup", FieldDisplayName = "Project Type", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = true, ShowInMobile = true, CustomProperties = "miniview=1", ColumnType = "", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "EstimatedConstructionStart", FieldDisplayName = "Esitmated Start Date", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = true, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ChanceOfSuccess", FieldDisplayName = "Chance of Success", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, ShowInMobile = true, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "RankingCriteriaTotal", FieldDisplayName = "Score", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = false, ShowInMobile = true, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "Owner", FieldDisplayName = "Owner", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = false, ShowInMobile = true, CustomProperties = "", ColumnType = "MultiUser", TextAlignment = "Left" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "LeadStatus", FieldDisplayName = "Status", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "StreetAddress1", FieldDisplayName = "Address", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "City", FieldDisplayName = "City", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "ApproxContractValue", FieldDisplayName = "Contract Value", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "Currency", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "CRMBusinessUnit", FieldDisplayName = "Business Unit", FieldSequence = ++seqNum, IsDisplay = false, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });
            mList.Add(new ModuleColumn() { CategoryName = ModuleName, FieldName = "UsableSqFtNum", FieldDisplayName = "Net Rentable SQ FT", FieldSequence = ++seqNum, IsDisplay = true, DisplayForClosed = false, ShowInMobile = false, CustomProperties = "", ColumnType = "", TextAlignment = "" });

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
            int numStages = 4;
            string closeStageID = (currStageID + numStages - 1).ToString();
            int moduleStep = 0;

            dataList.Add(new string[] { "Identify",
                "Identify new lead","<b>Please fill the form to create a new lead.</b>",
                (++moduleStep).ToString(), ModuleName,
                "Initiated", "Admin",
                 (++currStageID).ToString(), closeStageID, "",
                "Ticket Submitted", "", "",
                "Re-Submit", "Close", "", "1",
                "0", "10" , "" ,"", "", "Initiated" });

            dataList.Add(new string[] { "Assign",
                "Assignment state","",
                (++moduleStep).ToString(), ModuleName,
                "Active state", "Admin",
                (++currStageID).ToString(), closeStageID, "",
                "Assign", "", "",
                "Assign", "Reject", "Return", "",
                "0", "20" , "" , "", "","" });

            dataList.Add(new string[] { "Cultivate",
                "Cultivate state","",
                (++moduleStep).ToString(), ModuleName,
                "Cultivate state", "Admin",
                (++currStageID).ToString(), closeStageID, "",
                "Cultivate state", "", "",
                "Approve", "Reject", "Return", "",
                "0", "20" , "" , "", "","" });


            //moduleClosedStages.Add(ModuleName, currStageID);
            dataList.Add(new string[] { "Closed",
                "Ticket is closed, but you can add comments or can be re-opened by Owner","Ticket is closed",
                (++moduleStep).ToString(), ModuleName,
                "Closed","Admin",
                "", "", (currStageID-1).ToString(),
                "Closed", "", "Owner re-opened ticket",
                "", "", "Re-Open", "5",
                "0", "0" , "", "", "" ,"Closed" });

            // SANITY CHECK :-)
            if (closeStageID != currStageID.ToString())
            {
                string msg = "ERROR: Closed Stage ID: " + currStageID.ToString() + " not as expected: " + closeStageID;
                throw new System.Exception(msg);
            }


            foreach (string[] data in dataList)
            {
                LifeCycleStage item = new LifeCycleStage();
                //item.Title = data[0];
                item.StageTitle = data[0];
                item.UserWorkflowStatus = data[1];
                item.UserPrompt = data[2];
                item.StageStep = Convert.ToInt32(data[3]);
                item.ModuleNameLookup = data[4];

                item.Action = data[5];
                item.ActionUser = data[6];

                if (!string.IsNullOrEmpty(data[7]))
                    item.StageApprovedStatus = Convert.ToInt32(data[7]);

                if (!string.IsNullOrEmpty(data[8]))
                    item.StageRejectedStatus = Convert.ToInt32(data[8]);

                if (!string.IsNullOrEmpty(data[9]))
                    item.StageReturnStatus = Convert.ToInt32(data[9]);

                item.ApproveActionDescription = data[10];
                item.RejectActionDescription = data[11];
                item.ReturnActionDescription = data[12];

                item.StageApproveButtonName = data[13];
                item.StageRejectedButtonName = data[14];
                item.StageReturnButtonName = data[15];

                if (!string.IsNullOrEmpty(data[16]))
                    item.StageTypeLookup = Convert.ToInt32(data[16]);

                item.StageAllApprovalsRequired = Convert.ToBoolean(Convert.ToInt32(data[17]));
                item.StageWeight = Convert.ToInt32(data[18]);
                item.ShortStageTitle = data[19];
                item.CustomProperties = data[20];
                item.SkipOnCondition = data[21];
                item.StageTypeChoice = data[22];
                mList.Add(item);
            }

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
            mList.Add(new ModuleFormLayout() { Title = "General", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "General", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Lead Name", FieldName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Lead Name", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Client Company", FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Client Company", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Assigned To", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Client Contact", FieldName = "ContactLookup", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Client Contact", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Reason Type", FieldName = "ReasonType1", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Reason Type", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Lead Source Contact", FieldName = "LeadSource", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Lead Source Contact", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Lead Source Company", FieldName = "LeadSourceCompanyLabel", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Lead Source Company", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Description", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "#PlaceHolder#", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "#PlaceHolder#", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "General", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Classification", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Type", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Project Type", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Business Unit", FieldName = "CRMBusinessUnit", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Business Unit", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Project Type", FieldName = "AdditionalInfo", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Secondary Project Type", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "PlaceHolder", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "BCCI Sector", FieldDisplayName = "BCCI Sector", FieldName = "SectorChoice", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Market Sector", FieldName = "MarketSector", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Market Sector", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "CRM Urgency", FieldName = "CRMUrgency", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "CRM Urgency", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Notes", FieldName = "AdditionalDetail", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Notes", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Signed NDA", FieldDisplayName = "Signed NDA", FieldName = "SignedNDA", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });
            mList.Add(new ModuleFormLayout() { Title = "Lead Priority", FieldName = "SuccessChance", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Lead Priority", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Classification", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Project Info", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Street", FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Street", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldName = "ApproxContractValue", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Contract Value", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction Start", FieldName = "EstimatedConstructionStart", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Est Construction Start", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldName = "City", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "City", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Net Rentable Sq Ft", FieldName = "UsableSqFtNum", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Net Rentable Sq Ft", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction End", FieldName = "EstimatedConstructionEnd", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Est Construction End", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldName = "StateLookup", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "State", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Gross Sq Ft", FieldName = "RetailSqftNum", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Gross Sq Ft", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "PlaceHolder", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldName = "Zip", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Zip", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "PlaceHolder", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Project Info", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Miscellaneous", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Attachments", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayName = "Miscellaneous", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });

            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Cultivation", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayName = "Cultivation", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "preconditionlist", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayName = "preconditionlist", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Cultivation", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayName = "Cultivation", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });

            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Activity", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayName = "Activity", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "ContactActivity", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayName = "ContactActivity", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "Activity", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayName = "Activity", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });

            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayName = "Comments", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldName = "Comment", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayName = "Add Comment", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayName = "Comments", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });

            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayName = "History", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldName = "#Control#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayName = "History", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayName = "History", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true });


            seqNum = 0;
            // New ticket form
            mList.Add(new ModuleFormLayout() { Title = "General", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "General", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Lead Name", FieldName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Lead Name", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Client Company", FieldName = "CRMCompanyLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Client Company", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Assigned To", FieldName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Assigned To", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Client Contact", FieldName = "ContactLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Client Contact", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Lead Source Contact", FieldName = "LeadSource", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Lead Source Contact", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Description", FieldName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Description", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true, HideInTemplate = false, ColumnType = Constants.ColumnType.NoteField });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "PlaceHolder", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "General", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "General", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Classification", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Type", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Project Type", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Business Unit", FieldName = "CRMBusinessUnit", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Business Unit", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Secondary Project Type", FieldName = "AdditionalInfo", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Secondary Project Type", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "PlaceHolder", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "BCCI Sector", FieldDisplayName = "BCCI Sector", FieldName = "SectorChoice", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum), HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Market Sector", FieldName = "MarketSector", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Market Sector", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "CRM Urgency", FieldName = "CRMUrgency", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "CRM Urgency", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Notes", FieldName = "AdditionalDetail", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Notes", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Signed NDA", FieldDisplayName = "Signed NDA", FieldName = "SignedNDA", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, ShowInMobile = true, CustomProperties = "", FieldSequence = ++seqNum, HideInTemplate = true });
            //mList.Add(new ModuleFormLayout() { Title = "Lead Priority", FieldName = "SuccessChance", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Lead Priority", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Classification", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Classification", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Project Info", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Street", FieldName = "StreetAddress1", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Street", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Contract Value", FieldName = "ApproxContractValue", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Contract Value", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction Start", FieldName = "EstimatedConstructionStart", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Est Construction Start", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "City", FieldName = "City", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "City", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Net Rentable Sq Ft", FieldName = "UsableSqFtNum", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Net Rentable Sq Ft", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Est Construction End", FieldName = "EstimatedConstructionEnd", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Est Construction End", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "State", FieldName = "StateLookup", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "State", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Gross Sq Ft", FieldName = "RetailSqftNum", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Gross Sq Ft", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "PlaceHolder", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Zip", FieldName = "Zip", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Zip", FieldSequence = ++seqNum, FieldDisplayWidth = 1, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "PlaceHolder", FieldName = "#PlaceHolder#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "PlaceHolder", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Project Info", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Project Info", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldName = "#GroupStart#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Miscellaneous", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Attachments", FieldSequence = ++seqNum, FieldDisplayWidth = 2, ShowInMobile = true, HideInTemplate = false });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldName = "#GroupEnd#", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayName = "Miscellaneous", FieldSequence = ++seqNum, FieldDisplayWidth = 3, ShowInMobile = true, HideInTemplate = false });


            string currTab = string.Empty;
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            //List<string[]> dataList = new List<string[]>();
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Corridors", Category = "Construction", SubCategory = "Seismic Structural - 07", RequestType = "Corridors", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Elevator/Escalator", Category = "Construction", SubCategory = "Seismic Structural - 07", RequestType = "Elevator/Escalator", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Lobby", Category = "Construction", SubCategory = "Seismic Structural - 07", RequestType = "Lobby", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Restroom", Category = "Construction", SubCategory = "Seismic Structural - 07", RequestType = "Restroom", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Base Building > Storefront/Entry", Category = "Construction", SubCategory = "Seismic Structural - 07", RequestType = "Storefront/Entry", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Industrial Building", Category = "Construction", SubCategory = "Ground Up - 09", RequestType = "Industrial Building", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Mixed Use", Category = "Construction", SubCategory = "Ground Up - 09", RequestType = "Mixed Use", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Office Building", Category = "Construction", SubCategory = "Ground Up - 09", RequestType = "Office Building", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Office Park/Campus Development", Category = "Construction", SubCategory = "Ground Up - 09", RequestType = "Office Park/Campus Development", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Ground Up > Warehouse", Category = "Construction", SubCategory = "Ground Up - 09", RequestType = "Warehouse", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Auditorium", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Auditorium", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Broadcast Studio", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Broadcast Studio", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Cafeteria", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Cafeteria", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Classroom", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Classroom", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Data Center", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Data Center", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Fitness Center/Gym", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Fitness Center/Gym", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Interconnecting Stair", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Interconnecting Stair", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Laboratory", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Laboratory", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Market Ready/Spec Suite", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Market Ready/Spec Suite", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Interiors > Office Space", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Office Space", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Mixed Use > Mixed Use Office", Category = "Construction", SubCategory = "Mixed Use - 16", RequestType = "Mixed Use Office", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Mixed Use > Mixed Use Residential", Category = "Construction", SubCategory = "Mixed Use - 16", RequestType = "Mixed Use Residential", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Apartments", Category = "Construction", SubCategory = "Residential - 17", RequestType = "Apartments", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Condominiums", Category = "Construction", SubCategory = "Residential - 17", RequestType = "Condominiums", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Residential > Student Housing", Category = "Construction", SubCategory = "Residential - 17", RequestType = "Student Housing", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > ADA Access", Category = "Construction", SubCategory = "Site Work - 14", RequestType = "ADA Access", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Hardscape", Category = "Construction", SubCategory = "Site Work - 14", RequestType = "Hardscape", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Landscaping", Category = "Construction", SubCategory = "Site Work - 14", RequestType = "Landscaping", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction > Site Work > Playground", Category = "Construction", SubCategory = "Site Work - 14", RequestType = "Playground", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Core And Shell", Category = "Construction", SubCategory = "Seismic Structural - 07", RequestType = "Core And Shell", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Day Two", Category = "Construction", SubCategory = "Day Two - 11", RequestType = "Day Two", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Demolition", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Demolition", WorkflowType = "Full", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Distribution Center/Warehouse", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Distribution Center/Warehouse", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Elevator/Escalator", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Elevator/Escalator", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Historic", Category = "Construction", SubCategory = "Historic - 08", RequestType = "Historic", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Hospitality", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Hospitality", WorkflowType = "Full", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Lobby", Category = "Construction", SubCategory = "Interiors - 01", RequestType = "Lobby", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Service", Category = "Construction", SubCategory = "Service - 12", RequestType = "Service", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Warranty", Category = "Construction", SubCategory = "Warranty - 15", RequestType = "Warranty", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Architecture", Category = "Professional Services", SubCategory = "Architecture - 02", RequestType = "Architecture", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Construction Management", Category = "Professional Services", SubCategory = "Construction Management - 02", RequestType = "Construction Management", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Permit Services", Category = "Professional Services", SubCategory = "Permit Services - 02", RequestType = "Permit Services", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > LEED", Category = "Professional Services", SubCategory = "Sustainability - 02", RequestType = "LEED", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > Living Building Challenge", Category = "Professional Services", SubCategory = "Sustainability - 02", RequestType = "Living Building Challenge", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "Professional Services > Sustainability > WELL", Category = "Professional Services", SubCategory = "Sustainability - 02", RequestType = "WELL", WorkflowType = "SkipApprovals", FunctionalAreaLookup = 1, Owner = null, ModuleNameLookup = ModuleName });
            mList.Add(new ModuleRequestType() { Title = "PSG-Design Build", Category = "Professional Services", SubCategory = "PSG-Design Build - 02", RequestType = "PSG-Design Build", WorkflowType = "Full", FunctionalAreaLookup = null, Owner = null, ModuleNameLookup = ModuleName });

            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            List<string[]> dataList = new List<string[]>();
            int moduleStep = 0;


            dataList.Add(new string[] { "AdditionalDetail", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "AdditionalInfo", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "ApproxContractValue", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Attachments", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "City", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "ContactLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "CRMBusinessUnit", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "CRMCompanyLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "EstimatedConstructionEnd", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "EstimatedConstructionStart", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "LeadSource", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "LeadSourceCompanyLabel", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "MarketSector", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "CRMUrgency", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "ReasonType", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RetailSqft", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RetailSqftNum", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StreetAddress1", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "SuccessChance", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Description", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Owner", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RequestTypeLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Title", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "StateLookup", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "UsableSqFt", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "UsableSqFtNum", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "Zip", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "SectorChoice", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "SignedNDA", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "ContractValue", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ProjectedSize", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "RequestTypeCategory", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "ContractValue", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ProjectedSize", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ActualCompletionDate", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RequestTypeCategory", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "ContractValue", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ProjectedSize", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ActualCompletionDate", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RequestTypeCategory", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });

            moduleStep++;
            dataList.Add(new string[] { "ContractValue", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ProjectedSize", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });
            dataList.Add(new string[] { "ActualCompletionDate", ModuleName, moduleStep.ToString(), "0", "1", "0", "0", "" });
            dataList.Add(new string[] { "RequestTypeCategory", ModuleName, moduleStep.ToString(), "1", "1", "0", "0", "" });


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
            dataList.Add(new string[] { "Identify", ModuleName, stageID.ToString(), "1" });
            dataList.Add(new string[] { "Assign", ModuleName, (++stageID).ToString(), "1" });
            dataList.Add(new string[] { "Cultivate", ModuleName, (++stageID).ToString(), "2" });
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


            List<string[]> dataList = new List<string[]>();
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());

            dataList.Add(new string[] { "Identify Opportunity", "New Opportunity Ticket Created [$TicketId$]: [$Title$]", @"OPM Ticket ID [$TicketId$] has been initiated and needs clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the ticket.", moduleId, stageID.ToString() , "Initiator"});

            dataList.Add(new string[] { "Assign",  "OPM Ticket Pending Assignment [$TicketId$]: [$Title$]", @"OPM Ticket ID [$TicketId$] is pending PRP assignment.<br><br>" +
                                                        "Please assign a PRP and enter any other required fields", moduleId, (++stageID).ToString(), "Owner" });

            dataList.Add(new string[] { "Go/No Go",  "OPM Ticket Assigned [$TicketId$]: [$Title$]", @"OPM Ticket ID [$TicketId$] has been assigned to you for Go/No Go.<br><br>" +
                                                        "Once complete, please enter the analysis in the ticket.", moduleId, (++stageID).ToString(), "PRP;#ORP" });

            dataList.Add(new string[] { "Develop Approach", "OPM Ticket Pending Implementation [$TicketId$]: [$Title$]", @"OPM Ticket ID [$TicketId$] has completed analysis and is pending fix implementation.<br><br>" +
                                                    "Please review the analysis, implement the recommendations and close the ticket.<br><br>[$IncludeActionButtons$]", moduleId, (++stageID).ToString(), "Owner" });

            dataList.Add(new string[] { "Manage Bids", "OPM Ticket Closed [$TicketId$]: [$Title$]", "OPM Ticket ID [$TicketId$] has been closed.", moduleId, (++stageID).ToString(), "Owner" });

            dataList.Add(new string[] { "Prepare Estimates", "OPM Ticket On Hold [$TicketId$]: [$Title$]", "OPM Ticket ID [$TicketId$] has been placed on hold.", moduleId, null, "Owner" });


            foreach (string[] data in dataList)
            {
                ModuleTaskEmail item = new ModuleTaskEmail();
                item.Title = data[3] + " - " + data[0];
                item.Status = data[0];
                item.EmailTitle = data[1];
                item.EmailBody = data[2];
                item.ModuleNameLookup = data[3];

                if (string.IsNullOrEmpty(data[4]))
                    item.StageStep = Convert.ToInt32(data[4]);

                item.EmailUserTypes = data[5];

                mList.Add(item);
            }


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
            mList.Add(new ModuleUserType() { ModuleNameLookup = ModuleName, Title = ModuleName + " -Owner", UserTypes = "Owner", ColumnName = "Owner", ManagerOnly = false, ITOnly = false, CustomProperties = "" });
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "Lead", TabOrder = 1, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "Lead", TabOrder = 2, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "Lead", TabOrder = 3, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "Lead", TabOrder = 4, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "Lead", TabOrder = 5, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "Open", ViewName = "Lead", TabOrder = 6, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "Lead", TabOrder = 7, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "Lead", TabOrder = 8, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = false, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "onhold", TabDisplayName = "On Hold", ViewName = "Lead", TabOrder = 9, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "On Hold" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "Closed", ViewName = "Lead", TabOrder = 10, ModuleNameLookup = ModuleName, ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
            return tabs;
        }

        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        { }
    }
}
