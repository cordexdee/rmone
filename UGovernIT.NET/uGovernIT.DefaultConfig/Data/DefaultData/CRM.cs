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
    public class CRM : IModule
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
                    ShortName = "Customer Relationship Management",
                    CategoryName = "Customer Relationship Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "Customers",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/sitePages/reqeustlist?module=" + ModuleName,
                    ModuleHoldMaxStage = 0,
                    Title = "Customer Relationship Management (CRM)",
                    ModuleDescription = "This module is used to manage Customer Details.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/sitePages/request?module=" + ModuleName,
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = false,
                    EnableWorkflow = false,
                    EnableEventReceivers = true,
                    EnableCache = false,
                    EnableLayout = true
                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "CRM";
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
            mList.Add(new ModuleFormTab() { TabName = "Account Info", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Account Info" });
            mList.Add(new ModuleFormTab() { TabName = "Proposals", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Proposals" });
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Emails" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 7, TabSequence = 7, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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
            // Console.WriteLine("  ModuleStages");

            List<string[]> dataList = new List<string[]>();

            // Start from ID 1
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 6;
            //if (!enableTestingStage)
            //    numStages--;
            //if (!enablePendingCloseStage)
            //    numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();
            int StageStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Name = "Leads",
                StageTitle = "Leads",
                UserWorkflowStatus = "Awaiting resolution by PRP/ORP",
                UserPrompt = "<b>Please fill the form to create a new Customer.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Leads",
                ActionUser = "Initiator",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Create",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Create",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = "Leads"
            });

            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);

            mList.Add(new LifeCycleStage()
            {
                Name = "Campaign",
                StageTitle = "Campaign",
                UserWorkflowStatus = "Attempts to Convert a Lead to a Potential",
                UserPrompt = "<b>Owner: </b>Please Enter the Campaign Used and Add Task",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Campaign",
                ActionUser = "Owner",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Assigned Campaign",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Assigned Campaign",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = "Campaign"
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Potential Opportunity",
                StageTitle = "Potential Opportunity",
                UserWorkflowStatus = "Attempts to Convert a Lead to a Potential",
                UserPrompt = "<b>Owner: </b>Please enter following fields(Lead Status, Change of Success, Opportunity Owner and Tasks)",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Potential Opportunity",
                ActionUser = "Owner;#AcquiredBy",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Opportunity",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Opportunity",
                StageRejectedButtonName = "Reject",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

           
            mList.Add(new LifeCycleStage()
            {
                Name = "Account Setup",
                StageTitle = "Account Setup",
                UserWorkflowStatus = "",
                UserPrompt = "<b> Setup Account for Customer</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Account Setup",
                ActionUser = "AccountManager",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Setup Account",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Setup Account",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Name = "Account Management",
                StageTitle = "Account Management",
                UserWorkflowStatus = "Account Management",
                UserPrompt = "<b>Managing Account of Customer till account Closed.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Account Management",
                ActionUser = "AccountManager",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Close Account",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Close Account",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageAllApprovalsRequired = false,
                StageWeight = 50,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = ""
            });


            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
    
            mList.Add(new LifeCycleStage()
            {
                Name = "Account Closure",
                StageTitle = "Account Closure",
                UserWorkflowStatus = "Account Closure",
                UserPrompt = "<b>Account is Closed but Account Manager can open it.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                Action = "Account Closed",
                ActionUser = "Initiator;#AccountManager",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = currStageID - 2,
                ApproveActionDescription = "Closed",
                RejectActionDescription = "",
                ReturnActionDescription = "Account Manager can Re-Open Customer Account",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                SkipOnCondition = "",
                CustomProperties = "",
                StageTypeChoice = Convert.ToString(StageType.Closed)
            });

            
            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
                // Console.WriteLine("  ModuleDefaultValues");
            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {

            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");

            List<string[]> dataList = new List<string[]>();
            int seqNum = 0;
            // Tab 1
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General" , ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="" });

            mList.Add(new  ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Title", ShowInMobile = true, CustomProperties ="" });
            mList.Add(new  ModuleFormLayout() { Title = "First Name", FieldDisplayName = "First Name", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "FirstName", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Last Name", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1 , FieldName = "LastName", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Email", FieldDisplayName = "Email", ModuleNameLookup = ModuleName, TabId = 1 ,FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "EmailAddress", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Phone", FieldDisplayName = "Phone", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "VendorPhone", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Company", FieldDisplayName = "Company", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Company", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "URL", FieldDisplayName = "URL", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "URL", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Address", FieldDisplayName = "Address", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "WorkAddress", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Job-Title", FieldDisplayName = "Job-Title", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "JobTitle", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Lead Owner", FieldDisplayName = "Lead Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IdentifiedBy", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Type", FieldDisplayName = "Type", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "TicketOwner", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "TicketDescription", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Info", FieldDisplayName = "Campaign Info", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Info", FieldDisplayName = "Campaign Info", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CampaignInfo", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Campaign Strategy", FieldDisplayName = "Campaign Strategy", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CampaignStrategy", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Source", FieldDisplayName = "Source", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CustomerSource", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Source Details", FieldDisplayName = "Source Details", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "SourceDetails", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Lead Status", FieldDisplayName = "Lead Status", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "LeadStatus", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Chance of Success", FieldDisplayName = "Chance of Success", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "ChanceOfSuccess", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Quality", FieldDisplayName = "Quality", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Quality", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Used", FieldDisplayName = "Campaign Used", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth=3, FieldName = "CampaignUsed", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Info", FieldDisplayName = "Campaign Info", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Action", FieldDisplayName = "Campaign Action", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "CampiagnTasks", FieldDisplayName = "CampiagnTasks", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Campaign Action", FieldDisplayName = "Campaign Action", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "TicketComment", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // Tab 2: Account Info
            seqNum = 0;
            mList.Add(new  ModuleFormLayout() { Title = "Account Info", FieldDisplayName = "Account Info", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Name", FieldDisplayName = "Name", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AccountName", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Address", FieldDisplayName = "Address", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AccountAddress", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Type", FieldDisplayName = "Type", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CustomerAccountType", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Size", FieldDisplayName = "Size", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CustomerAccountSize", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Vertical", FieldDisplayName = "Vertical", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CustomerAccountVertical", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Revenues", FieldDisplayName = "Revenues", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Revenues", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "No of Employee", FieldDisplayName = "No of Employee", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "NoOfEmployee", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "SIC Code", FieldDisplayName = "SIC Code", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "SICCode", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Linkedin Page", FieldDisplayName = "Linkedin Page", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "LinkedInPage", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Facebook Page", FieldDisplayName = "Facebook Page", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "FacebookPage", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Twitter Page", FieldDisplayName = "Twitter Page", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "TwitterPage", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Account Manager", FieldDisplayName = "Account Manager", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AccountManager", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Account Info", FieldDisplayName = "Account Info", ModuleNameLookup = ModuleName, TabId = 2, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // Tab 3: Approvals
            seqNum = 0;
            mList.Add(new  ModuleFormLayout() { Title = "Proposals", FieldDisplayName = "Proposals", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "proposallist", FieldDisplayName = "proposallist", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Proposals", FieldDisplayName = "Proposals", ModuleNameLookup = ModuleName, TabId = 3, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // Tab 4: Approvals
            seqNum = 0;
            mList.Add(new  ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 4, FieldSequence = (++seqNum), FieldDisplayWidth =3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            // Tab 5: Emails
            seqNum = 0;
            mList.Add(new  ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Emails" });
            mList.Add(new  ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 5, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Emails" });

            // Tab 6: Related Tickets
            seqNum = 0;
            mList.Add(new  ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "WikiRelatedTickets", FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 6, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            //Tab 7: History
            seqNum = 0;
            mList.Add(new  ModuleFormLayout() { Title ="History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties="History" });
            mList.Add(new  ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true" });
            mList.Add(new  ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId=7, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History"});

            // New Ticket Form
            seqNum = 0;
            mList.Add(new  ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Title", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "First Name", FieldDisplayName = "First Name", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "FirstName", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Last Name", FieldDisplayName = "Last Name", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "LastName", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Email", FieldDisplayName = "Email", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "EmailAddress", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Phone", FieldDisplayName = "Phone", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "VendorPhone", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Company", FieldDisplayName = "Company", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "Company", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Access Admin", FieldDisplayName = "Access Admin", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "AccessAdmin", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "URL", FieldDisplayName = "URL", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "URL", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Address", FieldDisplayName = "Address", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "WorkAddress", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Job-Title", FieldDisplayName = "Job-Title", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "JobTitle", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Identified by", FieldDisplayName = "Identified by", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IdentifiedBy", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Type", FieldDisplayName = "Type", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "TicketOwner", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "TicketDescription", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 3 ,FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Info", FieldDisplayName = "Campaign Info", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Info", FieldDisplayName = "Campaign Info", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CampaignInfo", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Campaign Strategy", FieldDisplayName = "Campaign Strategy", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "CampaignStrategy", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Source", FieldDisplayName = "Source", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth= 1, FieldName = "CustomerSource", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Source Details", FieldDisplayName = "Source Details", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "SourceDetails", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Campaign Info", FieldDisplayName = "Campaign Info", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });

            mList.Add(new  ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 3,FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldSequence = (++seqNum), FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties = "" });
            mList.Add(new  ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId=0, FieldSequence = (++seqNum), FieldDisplayWidth = 3,FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "" });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("RequestRoleWriteAccess");

            List<string[]> dataList = new List<string[]>();
            int StageStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 1;
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "FirstName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "LastName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "EmailAddress", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "VendorPhone", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "Company", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory =true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "URL", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "WorkAddress", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "JobTitle", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "IdentifiedBy", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory =true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "AcquiredBy", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CampaignInfo", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CampaignStrategy", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerSource", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox=false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "SourceDetails", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "TicketOwner", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "TicketDescription", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 2;
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CampaignInfo", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CampaignStrategy", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerSource", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox=false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "SourceDetails", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "TicketOwner", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CampaignUsed", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 3;
            mList.Add(new  ModuleRoleWriteAccess() { FieldName ="LeadStatus", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "ChanceOfSuccess", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "Quality", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 4;
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "AccountManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "AccountName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "AccountAddress", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerAccountSize", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerAccountType", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerAccountVertical", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "NoOfEmployee", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "SICCode", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "LinkedInPage", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "FacebookPage", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "TwitterPage", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "Revenues", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties = "" });

            StageStep = 5;
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "AccountManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "AccountName", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "AccountAddress", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true,ShowWithCheckbox=false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerAccountSize", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerAccountType", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "CustomerAccountVertical", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "NoOfEmployee", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "SICCode", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "LinkedInPage", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "FacebookPage", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties = "" });
            mList.Add(new  ModuleRoleWriteAccess() { FieldName = "TwitterPage", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory =true, ShowEditButton =true, ShowWithCheckbox=false, CustomProperties = "" });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("TicketStatusMapping");
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("TaskEmails");
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "UnAssigned", ViewName = "Customers", TabOrder = 1, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "Customers", TabOrder = 2, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "Customers", TabOrder = 3, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "Customers", TabOrder = 4, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "Customers", TabOrder = 5, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "Customers", TabOrder = 6, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "Customers", TabOrder = 7, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "Customers", TabOrder = 8, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "Customers", TabOrder = 9, ModuleNameLookup = "CRM", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
