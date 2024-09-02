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
    public class INV : IModule
    {
        protected static bool enableOwnerApprovalStage = true;
        protected static bool enableFinanceApprovalStage = true;
        protected static bool enableLegalApprovalStage = true;
        protected static bool enablePurchasingStage = true;
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
                    ModuleName = "INV",
                    ShortName = "INV",
                    CategoryName = "Service Management",
                    LastSequence = 0,
                    LastSequenceDate = DateTime.ParseExact(DateTime.Now.ToString("MM-dd-yyyy"), "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    ModuleTable = "INV",
                    ModuleAutoApprove = false,
                    ModuleRelativePagePath = "/Pages/invtickets",
                    ModuleHoldMaxStage = 4,
                    Title = "Outages (INV)",
                    ModuleDescription = "This module is used to track outages, disasters or other unplanned interruptions. This may also include an unplanned reduction in the quality of an IT Service. The objective of incident management is to restore normal operations as quickly as possible with the least possible impact on either the business or the user, at a cost-effective price.",
                    ThemeColor = "Accent6",
                    StaticModulePagePath = "/Pages/inv",
                    ModuleType = ModuleType.SMS,
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
                return string.Empty;
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
            mList.Add(new ModuleFormTab() { TabName = "General Information", TabId = 1, TabSequence = 1, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "General Information" });
            mList.Add(new ModuleFormTab() { TabName = "Investments", TabId = 2, TabSequence = 2, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Investments" });
            mList.Add(new ModuleFormTab() { TabName = "Investment Distribution", TabId = 3, TabSequence = 3, ModuleNameLookup = ModuleName, CustomProperties = "IsSummaryTab=true", Title = ModuleName + " - " + "Investment Distribution" });
            mList.Add(new ModuleFormTab() { TabName = "Related Tickets", TabId = 4, TabSequence = 4, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Related Tickets" });
            mList.Add(new ModuleFormTab() { TabName = "Comments", TabId = 5, TabSequence = 5, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "Comments" });
            mList.Add(new ModuleFormTab() { TabName = "History", TabId = 6, TabSequence = 6, ModuleNameLookup = ModuleName, CustomProperties = "", Title = ModuleName + " - " + "History" });

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

            // Start from ID 66
            currStageID = 0;
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            int numStages = 3;
            int moduleStep = 0;
            //if (!enableOwnerApprovalStage)
            //    numStages--;
            //if (!enableFinanceApprovalStage)
            //    numStages--;
            //if (!enableLegalApprovalStage)
            //    numStages--;
            //if (!enablePurchasingStage)
            //    numStages--;

            string closeStageID = (currStageID + numStages - 1).ToString();

            mList.Add(new LifeCycleStage()
            {
                Action = "Initiated",
                Name = "Initiated",
                UserWorkflowStatus = "Awaiting resolution by PRP/ORP",
                StageTitle = "Assigned",
                UserPrompt = "<b>Please fill the form to open a new Investor Account.</b>",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "InitiatorUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = Convert.ToInt32(closeStageID == "" ? "0" : closeStageID),
                StageReturnStatus = 0,
                ApproveActionDescription = "Ticket Submitted",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Re-Submit",
                StageRejectedButtonName = "Close",
                StageReturnButtonName = "Return",
                StageTypeLookup = 1,
                StageAllApprovalsRequired = false,
                StageWeight = 5,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = Convert.ToString(StageType.Initiated)
            });

            //if (enableOwnerApprovalStage)
            //{
            //    dataList.Add(new string[] { "Owner Approval", 
            //    "Awaiting Owner's approval","<b>Owner:</b> Please approve the Contract.", 
            //    (++moduleStep).ToString(), ModuleName, 
            //    "Owner Approval", "TicketOwner", 
            //    (++currStageID).ToString(), closeStageID, (currStageID-2).ToString(),
            //    "Owner approved request", "", "",
            //    "Approve", "Reject", "Return", "",
            //    "0", "10" , "", ""  , ""  });
            //}

            //if (enableFinanceApprovalStage)
            //{
            //    dataList.Add(new string[] { "Finance Approval", 
            //    "Awaiting Finance Manager's approval","<b>Finance Manager:</b> Please approve the Contract.", 
            //    (++moduleStep).ToString(), ModuleName, 
            //    "Finance Approval", "TicketFinanceManager", 
            //    (++currStageID).ToString(), closeStageID, (currStageID-2).ToString(), 
            //    "Finance Manager approved request", "", "",
            //    "Approve", "Reject", "Return", "",
            //    "0", "10" , "" ,"" , "[NeedReview] <> 'Yes'"  });
            //}

            //if (enableLegalApprovalStage)
            //{
            //    dataList.Add(new string[] { "Legal Approval", 
            //    "Awaiting Legal approval","<b>Legal:</b> Please approve the Contract.", 
            //    (++moduleStep).ToString(), ModuleName, 
            //    "Legal Approval", "TicketLegal", 
            //    (++currStageID).ToString(), closeStageID, (currStageID-2).ToString(), 
            //    "Legal approved request", "", "",
            //    "Approve", "Reject", "Return", "",
            //    "0", "10" , "" ,"" , "[NeedReview] <> 'Yes'"  });
            //}

            //if (enablePurchasingStage)
            //{
            //    dataList.Add(new string[] { "Purchasing", 
            //    "Awaiting Purchasing approval","<b>Purchasing:</b> Please approve the Contract.", 
            //    (++moduleStep).ToString(), ModuleName, 
            //    "Purchasing Approval", "TicketPurchasing", 
            //    (++currStageID).ToString(), "", (currStageID-2).ToString(), 
            //    "Purchasing approved request", "", "",
            //    "Approve", "Reject", "Return", "",
            //    "0", "10" , "" ,"" , "[NeedReview] <> 'Yes'"  });
            //}

            mList.Add(new LifeCycleStage()
            {
                Action = "Investor Active",
                Name = "Active",
                UserWorkflowStatus = "Investor is Active till the expired date.",
                StageTitle = "Assigned",
                UserPrompt = "Investor Active",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = ++currStageID,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "Close Investor",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "Close Now",
                StageRejectedButtonName = "",
                StageReturnButtonName = "Return",
                StageTypeLookup = 2,
                StageAllApprovalsRequired = false,
                StageWeight = 45,
                ShortStageTitle = "",
                CustomProperties = "",
                SkipOnCondition = "",
                StageTypeChoice = ""
            });

            mList.Add(new LifeCycleStage()
            {
                Action = "Closed",
                Name = "Closed",
                UserWorkflowStatus = "Contract is expired, but you can add comments",
                StageTitle = "Closed",
                UserPrompt = "Contract Closed",
                StageStep = ++moduleStep,
                ModuleNameLookup = ModuleName,
                ActionUser = "OwnerUser",
                StageApprovedStatus = 0,
                StageRejectedStatus = 0,
                StageReturnStatus = 0,
                ApproveActionDescription = "",
                RejectActionDescription = "",
                ReturnActionDescription = "",
                StageApproveButtonName = "",
                StageRejectedButtonName = "",
                StageReturnButtonName = "",
                StageTypeLookup = 3,
                StageAllApprovalsRequired = false,
                StageWeight = 0,
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
            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            // Console.WriteLine("  FormLayout");
            int seqNum = 0;
            // Tab 1
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =2, FieldName = "Title", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Status", Title = "Status", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "Status", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Initiator", Title = "Initiator", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "InitiatorUser", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Created", Title = "Created", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =2, FieldName = "TicketCreationDate", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", Title = "Owner", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "OwnerUser", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", Title = "Description", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName = "Description", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Details", Title = "Investor Details", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor ID", Title = "Investor ID", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "InvestorID", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Short Name", Title = "Investor Short Name", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "InvestorShortName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Name", Title = "Investor Name", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "InvestorName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Street Address", Title = "Street Address", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =2, FieldName = "StreetAddress", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "City", Title = "City", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "WorkCity", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "State", Title = "State", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "UGITState", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Zip Code", Title = "Zip Code", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "WorkZip", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "First Name", Title = "First Name", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "FirstName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Last Name", Title = "Last Name", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "LastName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "EIN", Title = "EIN", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "EmployerIdentificationNumber", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Responsible", Title = "Responsible", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "Responsible", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Update Date", Title = "Update Date", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "UpdateDate", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Status", Title = "Investor Status", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "InvestorStatus", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Custodian", Title = "Custodian", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "Custodian", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Other Address", Title = "Other Address", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =2, FieldName = "OtherAddress", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Added Date", Title = "Added Date", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "AddedDate", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Contact", Title = "Contact", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "Contact", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "EmailAddress", Title = "EmailAddress", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "EmailAddress", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Account Type", Title = "Account Type", ModuleNameLookup = ModuleName, TabId =1, FieldDisplayWidth =1, FieldName = "AccountType", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Details", Title = "Investor Details", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", Title = "Attachments", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "" });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId =1,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            // Tab 2: Approvals 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investments", Title = "Investments", ModuleNameLookup = ModuleName, TabId =2,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "InvestmentsTab", Title = "InvestmentsTab", ModuleNameLookup = ModuleName, TabId =2,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investments", Title = "Investments", ModuleNameLookup = ModuleName, TabId =2,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            // Tab 3: Approvals 
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Distribution", Title = "Distribution", ModuleNameLookup = ModuleName, TabId =3,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "INVDistributionTab", Title = "INVDistributionTab", ModuleNameLookup = ModuleName, TabId =3,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Distribution", Title = "Distribution", ModuleNameLookup = ModuleName, TabId =3,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            //Tab 4: Related Tickets
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", Title = "Related Tickets", ModuleNameLookup = ModuleName, TabId =4,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "CustomTicketRelationship", Title = "CustomTicketRelationship", ModuleNameLookup = ModuleName, TabId =4,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Tickets", Title = "Related Tickets", ModuleNameLookup = ModuleName, TabId =4,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", Title = "Related Wikis", ModuleNameLookup = ModuleName, TabId =4,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "WikiRelatedTickets", Title = "WikiRelatedTickets", ModuleNameLookup = ModuleName, TabId =4,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Related Wikis", Title = "Related Wikis", ModuleNameLookup = ModuleName, TabId =4,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            // Tab 5: Comments
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments", Title = "Comments", ModuleNameLookup = ModuleName, TabId =5,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Add Comment", Title = "Add Comment", ModuleNameLookup = ModuleName, TabId =5,FieldDisplayWidth =3, FieldName = "Comment", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Comments", Title = "Comments", ModuleNameLookup = ModuleName, TabId =5,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            //Tab 6
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId =6,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId =6,FieldDisplayWidth =3, FieldName = "#Control#", ShowInMobile = true, CustomProperties = "IsReadOnly=true;#IsInFrame=false", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "History", Title = "History", ModuleNameLookup = ModuleName, TabId =6,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile = true, CustomProperties = "History", FieldSequence = (++seqNum) });

            // New Ticket Form
            seqNum = 0;
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            // first row
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Title", Title = "Title", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =2, FieldName = "Title", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Owner", Title = "Owner", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "OwnerUser", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Description", Title = "Description", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName = "Description", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "General", Title = "General", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Details", Title = "Investor Details", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor ID", Title = "Investor ID", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "InvestorID", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Short Name", Title = "Investor Short Name", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "InvestorShortName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Name", Title = "Investor Name", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "InvestorName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Street Address", Title = "Street Address", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =2, FieldName = "StreetAddress", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "City", Title = "City", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "WorkCity", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "State", Title = "State", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "UGITState", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Zip Code", Title = "Zip Code", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "WorkZip", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "First Name", Title = "First Name", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "FirstName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Last Name", Title = "Last Name", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "LastName", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "EIN", Title = "EIN", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "EmployerIdentificationNumber", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Responsible", Title = "Responsible", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "Responsible", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Update Date", Title = "Update Date", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "UpdateDate", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Status", Title = "Investor Status", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "InvestorStatus", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Custodian", Title = "Custodian", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "Custodian", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Other Address", Title = "Other Address", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =2, FieldName = "OtherAddress", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Added Date", Title = "Added Date", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "AddedDate", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Contact", Title = "Contact", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "Contact", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "EmailAddress", Title = "EmailAddress", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "EmailAddress", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Account Type", Title = "Account Type", ModuleNameLookup = ModuleName, TabId =0, FieldDisplayWidth =1, FieldName = "AccountType", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Investor Details", Title = "Investor Details", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });

            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName ="#GroupStart#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Attachments", Title = "Attachments", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName = "Attachments", ShowInMobile = false, CustomProperties = "", FieldSequence = (++seqNum) });
            mList.Add(new ModuleFormLayout() { FieldDisplayName = "Miscellaneous", Title = "Miscellaneous", ModuleNameLookup = ModuleName, TabId =0,FieldDisplayWidth =3, FieldName ="#GroupEnd#", ShowInMobile =true,CustomProperties ="", FieldSequence = (++seqNum) });
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            // Console.WriteLine("  RequestType");
            mList.Add(new ModuleRequestType() { Title = "Software", RequestType = "Software", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Investment", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Support", RequestType = "Support", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Investment", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Services", RequestType = "Services", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Investment", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            mList.Add(new ModuleRequestType() { Title = "Other", RequestType = "Other", ModuleNameLookup = ModuleName, RequestCategory = "", Category = "Investment", FunctionalAreaLookup = 1, WorkflowType = "Full", Deleted = false, Owner = ConfigData.Variables.Name });
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            // Console.WriteLine("  RequestRoleWriteAccess");
            int moduleStep = 0;

            // All stages
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Attachments", FieldName = "Attachments", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsPrivate", FieldName = "IsPrivate", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });

            // Stage 1 - New ticket
            moduleStep++;
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Title", FieldName = "Title", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "RequestTypeLookup", FieldName = "Description", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "IsPrivate", FieldName = "RequestTypeLookup", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "DesiredCompletionDate", FieldName = "DesiredCompletionDate", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Owner", FieldName = "OwnerUser", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });

            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "InvestorID", FieldName = "InvestorID", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "InvestorShortName", FieldName = "InvestorShortName", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "InvestorName", FieldName = "InvestorName", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "StreetAddress", FieldName = "StreetAddress", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "WorkCity", FieldName = "WorkCity", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "UGITState", FieldName = "UGITState", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "WorkZip", FieldName = "WorkZip", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "FirstName", FieldName = "FirstName", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "LastName", FieldName = "LastName", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "EmployerIdentificationNumber", FieldName = "EmployerIdentificationNumber", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Responsible", FieldName = "Responsible", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory = false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "UpdateDate", FieldName = "UpdateDate", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "InvestorStatus", FieldName = "InvestorStatus", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Custodian", FieldName = "Custodian", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AddedDate", FieldName = "AddedDate", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "OtherAddress", FieldName = "OtherAddress", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "Contact", FieldName = "Contact", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "EmailAddress", FieldName = "EmailAddress", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            mList.Add(new ModuleRoleWriteAccess() { Title = moduleStep + " - " + "AccountType", FieldName = "AccountType", ModuleNameLookup =ModuleName, StageStep = moduleStep, FieldMandatory =false,ShowEditButton =false,ShowWithCheckbox=false,CustomProperties ="" });
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            // Console.WriteLine("  TicketStatusMapping");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Initiated", ModuleNameLookup = ModuleName, StageTitleLookup = stageID, GenericStatusLookup = 1 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Active", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 2 });
            mList.Add(new ModuleStatusMapping() { Title = ModuleName + " - " + "Closed", ModuleNameLookup = ModuleName, StageTitleLookup = (++stageID), GenericStatusLookup = 3 });
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            // Console.WriteLine("  TaskEmails");
            int stageID = int.Parse(moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString());
            mList.Add(new ModuleTaskEmail() {
                Title = ModuleName + "-" + "Initiated",
                Status = "Initiated",
                EmailTitle = "Contract Returned [$TicketId$]: [$Title$]",
                EmailBody = @"Contract ID [$TicketId$] has been returned for clarification.<br><br>" +
                                                        "Please enter the required information and re-submit the request.",
                ModuleNameLookup = ModuleName,
                StageStep = stageID});

            if (enableOwnerApprovalStage)
            {
                mList.Add(new ModuleTaskEmail() {
                    Title = ModuleName + "-" + "Owner Approval",
                    Status = "Owner Approval",
                    EmailTitle = "New Contract Pending Owner Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract ID [$TicketId$] is pending Owner Approval.<br><br>" +
                                                        "Please approve or reject the request.",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID)});
            }

            if (enableFinanceApprovalStage)
            {
                mList.Add(new ModuleTaskEmail() {
                    Title = ModuleName + "-" + "Finance Approval",
                    Status = "Finance Approval",
                    EmailTitle = "Contract Pending Finance Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract ID [$TicketId$] is pending Finance Approval.<br><br>" +
                                                        "Please approve or reject the request.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID)});
            }

            if (enableLegalApprovalStage)
            {
                mList.Add(new ModuleTaskEmail() {
                    Title = ModuleName + "-" + "Legal Approval",
                    Status = "Legal Approval",
                    EmailTitle = "Contract Pending Legal Approval [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract ID [$TicketId$] is pending Legal Approval.<br><br>" +
                                                        "Please approve or reject the request.<br><br>[$IncludeActionButtons$]",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID)});
            }

            if (enablePurchasingStage)
            {
                mList.Add(new ModuleTaskEmail() {
                    Title = ModuleName + "-" + "Purchasing",
                    Status = "Purchasing",
                    EmailTitle = "Contract Pending Purchasing [$TicketId$]: [$Title$]",
                    EmailBody = @"Contract [$TicketId$] is pending purchasing.<br><br>" +
                                                        "Please enter PO number if applicable after complete",
                    ModuleNameLookup = ModuleName,
                    StageStep = (++stageID)});
            }

            mList.Add(new ModuleTaskEmail() { Title = ModuleName + "-" + "On - Hold", Status = "Active", EmailTitle = "Contract Activated [$TicketId$]: [$Title$]", EmailBody = @"Contract ID [$TicketId$] has been approved for activation.<br><br>", ModuleNameLookup = ModuleName, StageStep = (++stageID)});

            mList.Add(new ModuleTaskEmail()
            {
                Title = ModuleName + "-" + "Expired",
                Status = "Expired",
                EmailTitle = "Contract Expired [$TicketId$]: [$Title$]",
                EmailBody = @"Contract [$TicketId$] has expired.<br><br>" +
                                                        "Please renew or close the contract.",
                ModuleNameLookup = ModuleName,
                StageStep = (++stageID)
            });

            mList.Add(new ModuleTaskEmail() {Title= ModuleName+"-"+ "Closed", Status = "Closed", EmailTitle = "Contract Closed [$TicketId$]: [$Title$]", EmailBody = "Contract ID [$TicketId$] has been closed.", ModuleNameLookup = ModuleName, StageStep = (stageID + 1)});
            mList.Add(new ModuleTaskEmail() { Title = ModuleName + "-"+"On - Hold", Status = "On-Hold", EmailTitle = "Contract On Hold [$TicketId$]: [$Title$]", EmailBody = "Contract ID [$TicketId$] has been placed on hold.", ModuleNameLookup = ModuleName, StageStep = null });
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
            tabs.Add(new TabView() { TabName = "unassigned", TabDisplayName = "Unassigned", ViewName = "INV", TabOrder = 1, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Unassigned" });
            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "INV", TabOrder = 2, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Waiting On Me" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Open Tickets", ViewName = "INV", TabOrder = 3, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Open Items" });
            tabs.Add(new TabView() { TabName = "mygrouptickets", TabDisplayName = "My Group Tickets", ViewName = "INV", TabOrder = 4, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "My Group Items" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "Department Tickets", ViewName = "INV", TabOrder = 5, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Department Items" });
            tabs.Add(new TabView() { TabName = "allopentickets", TabDisplayName = "All Open Tickets", ViewName = "INV", TabOrder = 6, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Open Items" });
            tabs.Add(new TabView() { TabName = "allresolvedtickets", TabDisplayName = "All Resolve Tickets", ViewName = "INV", TabOrder = 7, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Resolved Items" });
            tabs.Add(new TabView() { TabName = "alltickets", TabDisplayName = "All Tickets", ViewName = "INV", TabOrder = 8, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "All Items" });
            tabs.Add(new TabView() { TabName = "allclosedtickets", TabDisplayName = "All Close Tickets", ViewName = "INV", TabOrder = 9, ModuleNameLookup = "INV", ColumnViewName = "MyHomeTab", ShowTab = true, TablabelName = "Closed Items" });
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
