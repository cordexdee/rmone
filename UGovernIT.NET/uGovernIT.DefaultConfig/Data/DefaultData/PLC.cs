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
    public class PLC : IModule
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
                    ShortName = "Proposal Lifecycle",
                    CategoryName = "Proposal Lifecycle",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "PLCRequest",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/sitePages/plctickets" ,
                    ModuleHoldMaxStage = 0,
                    Title = "Proposal Lifecycle (PLC)",
                    ModuleDescription = "This module is used to manage Customer Proposal Lifecycle.",
                    ThemeColor = "Accent1",
                    StaticModulePagePath = "/Pages/plc",
                    ModuleType = ModuleType.Governance,
                    EnableModule = false,
                    CustomProperties = "",
                    OwnerBindingChoice = "Auto",
                    SyncAppsToRequestType = true,
                    EnableWorkflow = true,
                    EnableEventReceivers = true,
                    EnableCache = true,
                    UseInGlobalSearch = true

                };
            }
        }

        public string ModuleName
        {
            get
            {
                return "PLC";
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
            mList.Add(new ModuleFormTab() { TabName = "Lifecycle", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Lifecycle" });
            mList.Add(new ModuleFormTab() { TabName = "Emails", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Emails" });
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
            int numStages = 4;
            //if (!enableTestingStage)
            //    numStages--;
            //if (!enablePendingCloseStage)
            //    numStages--;
            string closeStageID = (currStageID + numStages - 1).ToString();
            int StageStep = 0;

            mList.Add(new LifeCycleStage()
            {
                Action = "Identify needs",
                Name = "Identify needs",
                UserWorkflowStatus = "Initiate New Proposal",
                StageTitle = "Identify needs",
                UserPrompt = "<b>Please fill the form to create a new Proposal.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "InitiatorUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Create",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Create",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 20,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Proposal Phase",
                Name = "Proposal Phase",
                UserWorkflowStatus = "Proposal Phase convert to award",
                StageTitle = "Proposal Phase",
                UserPrompt = "<b>Owner: </b>Please enter the # of Licenses, price, billing start date and assign a account manager to finished the stage",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = Convert.ToInt32(startStageID == "" ? "0" : startStageID),
                ApproveActionDescription = "Proposal Phase",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Proposal Phase",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "Return",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 50,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Award",
                Name = "Award",
                UserWorkflowStatus = "Proposal is converted into Accounts",
                StageTitle = "Award",
                UserPrompt = "Proposal is converted into Accounts",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Closed",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Closed",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            moduleClosedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);

            mList.Add(new LifeCycleStage()
            {
                Action = "Proposal Closed",
                Name = "Closed",
                UserWorkflowStatus = "Proposal Closed",
                StageTitle = "Closed",
                UserPrompt = "<b>Proposal is closed but you can re-open it.</b>",
                StageStep = ++StageStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Closed",
                RejectActionDescription = "",
                ReturnActionDescription = "Account Manager can Re-Open Customer Account",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Re-Open",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 10,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
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
            int seqNum = 0;
            // Tab 1
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId=1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IT Staff Size", FieldDisplayName = "IT Staff Size",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ITStaffSize", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Components Needed", FieldDisplayName = "Components Needed",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "ComponentsNeeded", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Scope Of Services", FieldDisplayName = "Scope Of Services",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ScopeOfServices", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Price", FieldDisplayName = "Price",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "Price", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Billing Start Date", FieldDisplayName = "Billing Start Date",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "BillingStartDate", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Follow Up", FieldDisplayName = "Follow Up", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "FollowUp", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Chance Of Success", FieldDisplayName = "Chance Of Success",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "ChanceOfSuccess", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Plan Resources", FieldDisplayName = "Plan Resources",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "PlanResources", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Account Manager", FieldDisplayName = "Account Manager",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "AccountManager", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Type", FieldDisplayName = "Type",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "OwnerUser", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "No Of Licenses", FieldDisplayName = "No Of Licenses",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "NoOfLicenses", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "TicketDescription", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "Comment", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments",  ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 1, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 1, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            // Tab 2: Approvals
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "ApprovalTab", FieldDisplayName = "ApprovalTab", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Approvals", FieldDisplayName = "Approvals", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            // Tab 3: Emails
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "TicketEmails", FieldDisplayName = "TicketEmails", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties ="" });
            mList.Add(new ModuleFormLayout() { Title = "Emails", FieldDisplayName = "Emails", ModuleNameLookup = ModuleName, TabId = 3, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "Emails", FieldSequence = (++seqNum) });

            // Tab 4: Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="" });
            mList.Add(new ModuleFormLayout() { Title = "CustomTicketRelationship", FieldDisplayName = "CustomTicketRelationship",  ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties ="" });
            mList.Add(new ModuleFormLayout() { Title = "Related Tickets", FieldDisplayName = "Related Tickets", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties ="" });

            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "WikiRelatedTickets", FieldDisplayName = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Related Wikis", FieldDisplayName = "Related Wikis", ModuleNameLookup = ModuleName, TabId = 4, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "", FieldSequence = (++seqNum) });
            //Tab 5: History
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 5, FieldDisplayWidth = 3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=true", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "History", FieldDisplayName = "History", ModuleNameLookup = ModuleName, TabId = 2, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Title", FieldDisplayName = "Title", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Title", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "IT Staff Size", FieldDisplayName = "IT Staff Size", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ITStaffSize", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Components Needed", FieldDisplayName = "Components Needed", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "ComponentsNeeded", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Scope Of Services", FieldDisplayName = "Scope Of Services", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ScopeOfServices", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Price", FieldDisplayName = "Price", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "Price",ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Billing Start Date", FieldDisplayName = "Billing Start Date", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "BillingStartDate", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Follow Up", FieldDisplayName = "Follow Up", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "FollowUp",ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Chance Of Success", FieldDisplayName = "Chance Of Success", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "ChanceOfSuccess", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Plan Resources", FieldDisplayName = "Plan Resources", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "PlanResources", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Account Manager", FieldDisplayName = "Account Manager", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "AccountManager", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Type", FieldDisplayName = "Type", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "RequestTypeLookup", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Owner", FieldDisplayName = "Owner", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "TicketOwner", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "No Of Licenses", FieldDisplayName = "No Of Licenses", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 1, FieldName = "NoOfLicenses", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Description", FieldDisplayName = "Description", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "TicketDescription", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "General", FieldDisplayName = "General", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Add Comment", FieldDisplayName = "Add Comment", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "TicketComment", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Comments", FieldDisplayName = "Comment", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupStart#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Attachments", FieldDisplayName = "Attachments", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 2, FieldName = "Attachments", ShowInMobile = false, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Is Private", FieldDisplayName = "Is Private", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "IsPrivate", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { Title = "Miscellaneous", FieldDisplayName = "Miscellaneous", ModuleNameLookup = ModuleName, TabId = 0, FieldDisplayWidth = 3, FieldName = "#GroupEnd#", ShowInMobile = true, CustomProperties ="", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            mList.Add(new ModuleRequestType() { Title = "New", RequestType = "New", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Proposal", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Revised", RequestType = "Revised", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support",  Category = "Proposal", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Supplemental", RequestType = "Supplemental", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Proposal", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Continuation", RequestType = "Continuation", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support",  Category = "Proposal", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Pre-proposal/Notice of Intent", RequestType = "Pre-proposal/Notice of Intent", ModuleNameLookup = ModuleName, RequestCategory = "Ticketing Support", Category = "Proposal", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int StageStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "IsPrivate", FieldName = "IsPrivate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });

            StageStep = 1;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ITStaffSize", FieldName = "ITStaffSize", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ComponentsNeeded", FieldName = "ComponentsNeeded", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ScopeOfServices", FieldName = "ScopeOfServices", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Price", FieldName = "Price", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BillingStartDate", FieldName = "BillingStartDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FollowUp", FieldName = "FollowUp", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ChanceOfSuccess", FieldName = "ChanceOfSuccess", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PlanResources", FieldName = "PlanResources", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AccountManager", FieldName = "AccountManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "NoOfLicenses", FieldName = "NoOfLicenses", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = false, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "TicketDescription", FieldName = "TicketDescription", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });

            StageStep = 2;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Title", FieldName = "Title", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ITStaffSize", FieldName ="ITStaffSize", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ComponentsNeeded", FieldName = "ComponentsNeeded", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ScopeOfServices", FieldName = "ScopeOfServices", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "Price", FieldName = "Price", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "BillingStartDate", FieldName = "BillingStartDate", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });

            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "FollowUp", FieldName = "FollowUp", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "ChanceOfSuccess", FieldName = "ChanceOfSuccess", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "PlanResources", FieldName = "PlanResources", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AccountManager", FieldName = "AccountManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "NoOfLicenses", FieldName = "NoOfLicenses", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "TicketDescription", FieldName = "TicketDescription", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "RequestTypeLookup", FieldName = "RequestTypeLookup", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = false, ShowWithCheckbox = false, CustomProperties ="" });

            StageStep = 3;
            mList.Add(new ModuleRoleWriteAccess() { Title = StageStep + " - " + "AccountManager", FieldName = "AccountManager", ModuleNameLookup = ModuleName, StageStep = StageStep, FieldMandatory = true, ShowEditButton = true, ShowWithCheckbox = false, CustomProperties="" });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");
            return mList;
           
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {

            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Unassigned", ViewName = "PLC", TabOrder = 1, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true ,TablabelName= "Unassigned"});
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "PLC", TabOrder = 2, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "PLC", TabOrder = 3, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "PLC", TabOrder = 4, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "PLC", TabOrder = 5, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "PLC", TabOrder = 6, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "PLC", TabOrder = 7, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "PLC", TabOrder = 8, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "PLC", TabOrder = 9, ModuleNameLookup = "PLC", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Clsoed Items" });
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
